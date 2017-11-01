using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OfficeOpenXml;

using BD = Bentley.DgnPlatformNET;
using PDIWT_MS_Tool.Properties;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OfficeOpenXml.Style;

namespace PDIWT_MS_Tool.ViewModels
{
    public class LevelExportViewModel : ViewModelBase
    {

        public LevelExportViewModel()
        {
            IsOnlyUsedLevel = true;
            IsIncludeRefLevel = false;
        }

        private bool _IsOnlyUsedLevel;
        public bool IsOnlyUsedLevel
        {
            get { return _IsOnlyUsedLevel; }
            set { Set(ref _IsOnlyUsedLevel, value); }
        }

        private bool _IsIncludeRefLevel;
        public bool IsIncludeRefLevel
        {
            get { return _IsIncludeRefLevel; }
            set { Set(ref _IsIncludeRefLevel, value); }
        }

        private RelayCommand _ExportToExcel;
        public RelayCommand ExportToExcel => _ExportToExcel ?? (_ExportToExcel = new RelayCommand(ExecuteExportToExcel));
        public void ExecuteExportToExcel()
        {
            string activeFileName = Program.GetActiveDgnFile().GetFileName();
            string defaultFileName = Path.GetDirectoryName(activeFileName);
            string initialDirectory = Path.GetFileNameWithoutExtension(activeFileName);
            SaveFileDialog sfDialog = new SaveFileDialog { Filter = Resources.ExcelFilter, RestoreDirectory = true, InitialDirectory = initialDirectory, FileName = defaultFileName };

            if (sfDialog.ShowDialog() == DialogResult.OK)
            {

                string outputFileName = sfDialog.FileName;
                if (File.Exists(outputFileName))
                    File.Delete(outputFileName);

                List<string> dgnFileNameList = new List<string> {activeFileName};
                if(IsIncludeRefLevel)
                    GetAttachmentsDgnFiles(ref dgnFileNameList,activeFileName);
                List<LevelInfo> levelInfos = new List<LevelInfo>();
                foreach (var dgnFileName in dgnFileNameList)
                {
                    GetLevelInfo(ref levelInfos,dgnFileName);
                }
                Export(outputFileName, levelInfos);
                MessageBox.Show($"文件保存至{outputFileName}","生成完毕", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //[Command]
        //public void Test()
        //{
        //    string activeDgnFileName = Program.GetActiveDgnFile().GetFileName();
        //    List<string> dgnFileNameList = new List<string> {activeDgnFileName};
        //    GetAttachmentsDgnFiles(ref dgnFileNameList, activeDgnFileName);
        //}

        private void GetLevelInfo(ref List<LevelInfo> levelInfos,string inDgnFileName)
        {
            BD.DgnDocument inDgnDocument = BD.DgnDocument.CreateForLocalFile(inDgnFileName);
            BD.DgnFile inDgnFile = BD.DgnFile.Create(inDgnDocument, BD.DgnFileOpenMode.ReadOnly).DgnFile;
            BD.StatusInt openFileStatusInt;
            if (BD.DgnFileStatus.Success != inDgnFile.LoadDgnFile(out openFileStatusInt))
                return;
            if (BD.StatusInt.Success != inDgnFile.FillDictionaryModel())
                return;
            string inDgnFileNameWithoutExt = Path.GetFileNameWithoutExtension(inDgnFile.GetFileName());
            var fileLevelCache = inDgnFile.GetLevelCache();
            var handles = fileLevelCache.GetHandles();
            foreach (var handle in handles)
            {
                if (IsOnlyUsedLevel && !fileLevelCache.IsLevelUsedInFile(handle.LevelId))
                    continue;
                uint colorint = handle.GetByLevelColor().Color;
                BD.ColorInformation colorInformation = BD.DgnColorMap.ExtractElementColorInfo(colorint,
                    handle.GetByLevelColor().GetDefinitionFile());
                levelInfos.Add(new LevelInfo
                {
                    DgnFileName = inDgnFileNameWithoutExt,
                    LevelName = handle.Name,
                    LevelColor = colorInformation.ColorDefinition.SystemColor
                });
            }
        }

        private void GetAttachmentsDgnFiles(ref List<string> dgnFileNameList, string inDgnFileName)
        {
            BD.DgnDocument inDgnDocument = BD.DgnDocument.CreateForLocalFile(inDgnFileName);
            BD.DgnFile inDgnFile = BD.DgnFile.Create(inDgnDocument, BD.DgnFileOpenMode.ReadOnly).DgnFile;
            BD.StatusInt openFileStatusInt;
            if(BD.DgnFileStatus.Success != inDgnFile.LoadDgnFile(out openFileStatusInt))
                return;
            if(BD.StatusInt.Success != inDgnFile.FillDictionaryModel())
                return;
            var dgnModelIndexs = inDgnFile.GetModelIndexCollection();
            foreach (var dgnModelIndex in dgnModelIndexs)
            {
                BD.StatusInt loadModelStatusInt;
                var dgnModel = inDgnFile.LoadRootModelById(out loadModelStatusInt, dgnModelIndex.Id);
                if (BD.StatusInt.Success != loadModelStatusInt) continue;
                var dgnAttachmentList = dgnModel.GetDgnAttachments();
                foreach (var dgnAttachment in dgnAttachmentList)
                {
                    if (dgnAttachment.IsMissingFile || dgnAttachment.IsMissingModel) continue;
                    var attachedDgnFileName = dgnAttachment.GetAttachFullFileSpec(true);
                    if (!dgnFileNameList.Contains(attachedDgnFileName))
                        dgnFileNameList.Add(attachedDgnFileName);
                    else
                        return;
                    if (attachedDgnFileName != null)
                        GetAttachmentsDgnFiles(ref dgnFileNameList, attachedDgnFileName);
                }
            }
        }

        private void Export(string outputFileName, IEnumerable<LevelInfo> levelInfos)
        {
            var distictFileName = (from levelinfo in levelInfos
                                   select levelinfo.DgnFileName).Distinct();
            using (ExcelPackage package = new ExcelPackage(new FileInfo(outputFileName)))
            {
                foreach (var fileName in distictFileName)
                {
                    var addsheet = package.Workbook.Worksheets.Add(fileName);
                    addsheet.Cells["A1"].Value = "图层名";
                    addsheet.Cells["B1"].Value = "ARGB颜色数值";
                    addsheet.Cells["C1"].Value = "颜色显示";
                }
                foreach (var levelInfo in levelInfos)
                {
                    var addSheet = package.Workbook.Worksheets[levelInfo.DgnFileName];
                    int insertRowNum = addSheet.Dimension.Rows + 1;
                    addSheet.Cells[insertRowNum, 1].Value = levelInfo.LevelName;
                    addSheet.Cells[insertRowNum, 2].Value =
                        string.Format(
                            $"{levelInfo.LevelColor.A},{levelInfo.LevelColor.R},{levelInfo.LevelColor.G},{levelInfo.LevelColor.B}");
                    addSheet.Cells[insertRowNum, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    addSheet.Cells[insertRowNum, 3].Style.Fill.BackgroundColor.SetColor(levelInfo.LevelColor);
                }
                foreach (var excelWorksheet in package.Workbook.Worksheets)
                {
                    var allCells =
                        excelWorksheet.Cells[1, 1, excelWorksheet.Dimension.Rows, excelWorksheet.Dimension.Columns];
                    allCells.AutoFitColumns(0);
                }
                package.Save();
            }
        }

    }

    public class LevelInfo
    {
        public string DgnFileName { get; set; }
        public string LevelName { get; set; }
        public Color LevelColor { get; set; }
    }
}