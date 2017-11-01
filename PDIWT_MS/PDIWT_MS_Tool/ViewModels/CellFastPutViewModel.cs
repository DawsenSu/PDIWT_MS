using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using PDIWT_MS_Tool.Properties;
using System.Windows.Forms;
using Bentley.EC.Persistence.Query;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

using EPPlus.DataExtractor;
using PDIWT_MS_CPP;
using OfficeOpenXml;

using BM = Bentley.MstnPlatformNET;
using BD = Bentley.DgnPlatformNET;

namespace PDIWT_MS_Tool.ViewModels
{
    public class CellFastPutViewModel : ViewModelBase
    {
        private ObservableCollection<ElementProp> _ElementProps;
        public ObservableCollection<ElementProp> ElementProps
        {
            get { return _ElementProps; }
            set { Set(ref _ElementProps, value); }
        }
        private ObservableCollection<string> _CellNameTypes;
        public ObservableCollection<string> CellNameTypes
        {
            get { return _CellNameTypes; }
            set { Set(ref _CellNameTypes, value); }
        }

        private int _PutCellProgress;
        public int PutCellProgress
        {
            get { return _PutCellProgress; }
            set { Set(ref _PutCellProgress, value); }
        }
        private string _Prompt;
        public string Prompt
        {
            get { return _Prompt; }
            set { Set(ref _Prompt, value); }
        }

        private string _Status;
        public string Status
        {
            get { return _Status; }
            set { Set(ref _Status, value); }
        }

        public CellFastPutViewModel()
        {
            ElementProps = new ObservableCollection<ElementProp>();
            CellNameTypes = new ObservableCollection<string>();
            Prompt = Resources.PromptHeader;
            Status = Resources.StatusHeader;
            PutCellProgress = 0;
        }

        private RelayCommand _OpenCellLib;
        public RelayCommand OpenCellLib => _OpenCellLib ?? (_OpenCellLib = new RelayCommand(ExecuteOpenCellLib));
        public void ExecuteOpenCellLib()
        {
            OpenFileDialog cellFileDialog = new OpenFileDialog()
            {
                Filter = Resources.CellLibraryFilter,
                Title = "选择Cell库文件"
            };
            if (cellFileDialog.ShowDialog() == DialogResult.OK)
            {
                BD.DgnDocument cellFileDocument = BD.DgnDocument.CreateForLocalFile(cellFileDialog.FileName);
                BD.DgnFile cellDgnFile = BD.DgnFile.Create(cellFileDocument, BD.DgnFileOpenMode.ReadOnly).DgnFile;
                if (cellDgnFile == null)
                {
                    Prompt = Resources.PromptHeader + $"无法读取{cellFileDialog.FileName}的DgnDocument对象";
                    Status = Resources.StatusHeader + Resources.ErrorString;
                    return;
                }
                BD.StatusInt loadStatusInt;
                if (BD.DgnFileStatus.Success != cellDgnFile.LoadDgnFile(out loadStatusInt))
                {
                    Prompt = Resources.PromptHeader + "无法载入文件";
                    Status = Resources.StatusHeader + Resources.ErrorString;
                    return;
                }
                if (cellDgnFile.FillDictionaryModel() != BD.StatusInt.Success)
                {
                    Prompt = Resources.PromptHeader + "填充模型失败";
                    Status = Resources.StatusHeader + Resources.ErrorString;
                    return;
                }
                CellNameTypes.Clear();
                ElementProps.Clear();
                int index = 0;
                foreach (var modelindex in cellDgnFile.GetModelIndexCollection())
                {
                    BD.DgnModel model = cellDgnFile.LoadRootModelById(out loadStatusInt, modelindex.Id);
                    if (model != null && modelindex.CellPlacementOptions == BD.CellPlacementOptions.CanBePlacedAsCell)
                    {
                        CellNameTypes.Add(model.ModelName + "(" + model.GetModelInfo().CellType.ToString() + ")");
                        index++;
                    }
                }
                string filename;
                if (CellFunction.AttachLibrary(out filename, cellFileDialog.FileName, "") != BD.StatusInt.Success)
                {
                    Prompt = Resources.PromptHeader + "附加模型失败";
                    Status = Resources.StatusHeader + Resources.ErrorString;
                    return;
                }
                Prompt = Resources.PromptHeader + $"{cellFileDialog.SafeFileName}已载入!";
                Status = Resources.StatusHeader + Resources.SuccessString;
            }
        }

        private RelayCommand _ImportFromFile;
        public RelayCommand ImportFromFile => _ImportFromFile ?? (_ImportFromFile = new RelayCommand(ExecuteImportFromFile, CanExecuteImportFromFile));
        public void ExecuteImportFromFile()
        {
            OpenFileDialog excelOpenFileDialog = new OpenFileDialog()
            {
                Filter = Resources.ExcelFilter,
                Title = "选择输入Excel文件"
            };
            if (excelOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var package = new ExcelPackage(new FileInfo(excelOpenFileDialog.FileName)))
                    {
                        var sheet = package.Workbook.Worksheets[1];
                        var data = sheet
                            .Extract<ElementProp>()
                            .WithProperty(p => p.CellName, "A")
                            .WithProperty(p => p.X, "B")
                            .WithProperty(p => p.Y, "C")
                            .WithProperty(p => p.Z, "D")
                            .WithProperty(p => p.AngleX, "E")
                            .WithProperty(p => p.AngleY, "F")
                            .WithProperty(p => p.AngleZ, "G")
                            .GetData(2, row => row != sheet.Dimension.Rows + 1)
                            .ToList();

                        if (data.Count == 0)
                        {
                            Prompt = Resources.PromptHeader + "文件内容为空";
                            Status = Resources.StatusHeader + Resources.ErrorString;
                            return;
                        }
                        var cellNameDictonary = CellNameTypes.ToDictionary(p => p.Split('(').First());
                        ElementProps.Clear();
                        PutCellProgress = 0;
                        double uor = Program.GetActiveDgnModel().GetModelInfo().UorPerMaster;
                        foreach (var elementProp in data)
                        {
                            if (cellNameDictonary.ContainsKey(elementProp.CellName))
                            {
                                ElementProps.Add(new ElementProp()
                                {
                                    CellName = cellNameDictonary[elementProp.CellName],
                                    X = elementProp.X * uor,
                                    Y = elementProp.Y * uor,
                                    Z = elementProp.Z * uor,
                                    AngleX = elementProp.AngleX,
                                    AngleY= elementProp.AngleY,
                                    AngleZ = elementProp.AngleZ
                                });
                            }
                        }
                    }
                    Prompt = Resources.PromptHeader + $"{excelOpenFileDialog.FileName}载入成功";
                    Status = Resources.StatusHeader + Resources.SuccessString;
                }
                catch (Exception e)
                {
                    Prompt = Resources.PromptHeader + e.ToString();
                    Status = Resources.StatusHeader + "文件解析出错";
                    ElementProps.Clear();
                }

            }
        }
        public bool CanExecuteImportFromFile()
        {
            return CellNameTypes.Count != 0;
        }

        private RelayCommand _FastPut;
        public RelayCommand FastPut => _FastPut ?? (_FastPut = new RelayCommand(ExecuteFastPut, CanExecuteFastPut));
        public void ExecuteFastPut()
        {
            PutCellProgress = 0;
            foreach (ElementProp t in ElementProps)
            {
                if (PDIWT_MS_CPP.CellFunction.PlaceCell(t) == 0)
                {
                    Prompt = Resources.PromptHeader + $"{t.CellName}放置失败";
                    Status = Resources.StatusHeader + Resources.ErrorString;
                    BM.MessageCenter.Instance.ShowErrorMessage(Prompt, Prompt, BM.MessageAlert.Balloon);
                    continue;
                }
                PutCellProgress++;
            }
            MessageBox.Show($"{PutCellProgress}个单元放置完成", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Prompt = Resources.PromptHeader + $"{PutCellProgress}个单元放置完成";
            Status = Resources.StatusHeader + Resources.SuccessString;
        }
        public bool CanExecuteFastPut()
        {
            return ElementProps.Count != 0;
        }

        //[Command]
        //public void DeleteAllRows()
        //{
        //    ElementProps.Clear();
        //}

        //public bool CanDeleteAllRows()
        //{
        //    return ElementProps.Count != 0;
        //}
    }

    public static class BentleyMarshal
    {
        [DllImport("ustation.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void mdlCell_getLibraryName(out string filename);

    }

    public class ElementProp
    {
        public string CellName { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double AngleX { get; set; }
        public double AngleY { get; set; }
        public double AngleZ { get; set; }
    }
}