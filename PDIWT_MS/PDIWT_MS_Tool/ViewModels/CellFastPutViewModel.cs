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
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Editors.Helpers;
using EPPlus.DataExtractor;
using PDIWT_MS_CPP;
using OfficeOpenXml;

using BM = Bentley.MstnPlatformNET;
using BD = Bentley.DgnPlatformNET;

namespace PDIWT_MS_Tool.ViewModels
{
    public class CellFastPutViewModel : ViewModelBase
    {
        public ObservableCollection<ElementProp> ElementProps
        {
            get { return GetProperty(() => ElementProps); }
            set { SetProperty(() => ElementProps, value); }
        }
        public ObservableCollection<string> CellNameTypes
        {
            get { return GetProperty(() => CellNameTypes); }
            set { SetProperty(() => CellNameTypes, value); }
        }
        public int PutCellProgress
        {
            get { return GetProperty(() => PutCellProgress); }
            set { SetProperty(() => PutCellProgress, value); }
        }

        public string Prompt
        {
            get { return GetProperty(() => Prompt); }
            set { SetProperty(() => Prompt, value); }
        }
        public string Status
        {
            get { return GetProperty(() => Status); }
            set { SetProperty(() => Status, value); }
        }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            ElementProps = new ObservableCollection<ElementProp>();
            CellNameTypes = new ObservableCollection<string>();
            Prompt = Resources.PromptHeader;
            Status = Resources.StatusHeader;
            PutCellProgress = 0;
        }

        [Command]
        public void OpenCellLib()
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


        [Command]
        public void ImportFromFile()
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
                            .WithProperty(p => p.AngelX, "E")
                            .WithProperty(p => p.AngelY, "F")
                            .WithProperty(p => p.AngelZ, "G")
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
                                    AngelX = elementProp.AngelX,
                                    AngelY = elementProp.AngelY,
                                    AngelZ = elementProp.AngelZ
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

        public bool CanImportFromFile()
        {
            return CellNameTypes.Count != 0;
        }
        [Command]
        public void FastPut()
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
        public bool CanFastPut()
        {
            return ElementProps.Count != 0;
        }

        [Command]
        public void DeleteAllRows()
        {
            ElementProps.Clear();
        }

        public bool CanDeleteAllRows()
        {
            return ElementProps.Count != 0;
        }
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
        public double AngelX { get; set; }
        public double AngelY { get; set; }
        public double AngelZ { get; set; }
    }
}