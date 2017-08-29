using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Bentley.DgnPlatformNET.DgnEC;
using Bentley.UI.Controls.WinForms;
using BM = Bentley.MstnPlatformNET;
using BD = Bentley.DgnPlatformNET;

using OfficeOpenXml;
using EPPlus.DataExtractor;


namespace PDIWT_MS_Tool.Models
{
    public class DgnModelCreator
    {
        public int CreateModels(List<DgnModelInfo> inputDgnModelInfoList)
        {
            var activeDgnFile = Program.GetActiveDgnFile();
            int successAddModels = 0;
            foreach (var info in inputDgnModelInfoList)
            {
                BD.DgnModelStatus outDgnModelStatus;
                var newCreatedModel = activeDgnFile.CreateNewModel(out outDgnModelStatus, info.ModelName,
                    validModelTypeDictionary[info.ModelType], validModelDimensionDictionary[info.ModelDimension], null);
                if (outDgnModelStatus != BD.DgnModelStatus.Success)
                {
                    BM.MessageCenter.Instance.ShowErrorMessage(info.ModelName + ":" + outDgnModelStatus, info.ModelName + ":" + outDgnModelStatus, true);
                    continue;
                }
                var newCreatedModelInfo = newCreatedModel.GetModelInfo();
                newCreatedModelInfo.Description = info.Description;
                newCreatedModelInfo.DefaultRefLogical = info.RefName;
                newCreatedModelInfo.IsInCellList = info.CanBePutAsCell;
                newCreatedModelInfo.IsAnnotationCell = info.CanBePutAsAnnotationCell;
                newCreatedModelInfo.CellType = validCellTypeDictionary[info.CellType];
                if (newCreatedModel.SetModelInfo(newCreatedModelInfo) != BD.DgnModelStatus.Success)
                {
                    BM.MessageCenter.Instance.ShowErrorMessage($"{info.ModelName}模型信息赋值失败", $"{info.ModelName}模型信息赋值失败", true);
                    continue;
                }
                newCreatedModel.SaveModelSettings();
                successAddModels++;
            }
            return successAddModels;
        }

        public List<DgnModelInfo> ReadModelInfoFromExcel(string excelFilePath)
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                var sheet = package.Workbook.Worksheets[1];
                var data = sheet.Extract<DgnModelInfo>()
                    .WithProperty(p => p.ModelType, "A")
                    .WithProperty(p => p.ModelDimension, "B")
                    .WithProperty(p => p.ModelName, "C")
                    .WithProperty(p => p.Description, "D")
                    .WithProperty(p => p.RefName, "E")
                    .WithProperty(p => p.CanBePutAsCell, "F")
                    .WithProperty(p => p.CanBePutAsAnnotationCell, "G")
                    .WithProperty(p => p.CellType, "H")
                    .GetData(2, row => row != sheet.Dimension.Rows + 1)
                    .ToList();
                int importedDataNum = data.Count;
                List<string> nameList = new List<string>();
                foreach (var info in data)
                {
                    if (ValidateModelInfo(info) != ModelInfoStatus.Good)
                        data.Remove(info);
                    nameList.Add(info.ModelName);
                }
                if (importedDataNum != data.Count)
                    throw new InvalidDataException("输入文件中存在无效条目");
                if (nameList.Count != nameList.Distinct().Count())
                    throw new DuplicateNameException("模型名称重复");
                return data;
            }
        }

        private ModelInfoStatus ValidateModelInfo(DgnModelInfo modelInfo)
        {
            if (!validModelTypeDictionary.ContainsKey(modelInfo.ModelType))
                return ModelInfoStatus.InvalidModelType;
            if (!validModelDimensionDictionary.ContainsKey(modelInfo.ModelDimension))
                return ModelInfoStatus.InvalidModelDimension;
            if (!validCellTypeDictionary.ContainsKey(modelInfo.CellType))
                return ModelInfoStatus.InvalidCellType;
            return ModelInfoStatus.Good;
        }

        public int OutputModelInfo(string filePath)
        {
            FileInfo excelFileInfo = new FileInfo(filePath);
            using (ExcelPackage package = new ExcelPackage(excelFileInfo))
            {
                var sheet = package.Workbook.Worksheets.Add("模型信息");
                var titles = new string[] { "类型", "模型维度", "名称", "描述", "参考逻辑名称", "是否作为单元", "是否作为注释单元", "单元类型" };
                for (int i = 0; i < titles.Length; i++)
                    sheet.Cells[1, i + 1].Value = titles[i];
                var activeDgnFile = Program.GetActiveDgnFile();
                List<DgnModelInfo> modelInfos = new List<DgnModelInfo>();
                var reversedValidModelTypeDictionary = validModelTypeDictionary.ToDictionary(p => p.Value, p => p.Key);
                var reversedValidModelDimensionDictionary = validModelDimensionDictionary.ToDictionary(p => p.Value, p => p.Key);
                var reversedValidCellTypeDictionary = validCellTypeDictionary.ToDictionary(p => p.Value, p => p.Key);
                int insertRowNum = 2;
                foreach (var model in activeDgnFile.GetModelIndexCollection())
                {
                    BD.StatusInt loadStatusInt;
                    var dgnModel = activeDgnFile.LoadRootModelById(out loadStatusInt,
                        activeDgnFile.FindModelIdByName(model.Name));
                    if (loadStatusInt != BD.StatusInt.Success) continue;
                    var dgnModelInfo = dgnModel.GetModelInfo();
                    sheet.Cells[insertRowNum, 1].Value = reversedValidModelTypeDictionary[model.ModelType];
                    sheet.Cells[insertRowNum, 2].Value = reversedValidModelDimensionDictionary[model.Is3D];
                    sheet.Cells[insertRowNum, 3].Value = model.Name;
                    sheet.Cells[insertRowNum, 4].Value = model.Description;
                    sheet.Cells[insertRowNum, 5].Value = dgnModelInfo.DefaultRefLogical;
                    sheet.Cells[insertRowNum, 6].Value = (model.CellPlacementOptions & BD.CellPlacementOptions.CanBePlacedAsCell) != 0;
                    sheet.Cells[insertRowNum, 7].Value = (model.CellPlacementOptions & BD.CellPlacementOptions.CanBePlacedAsAnnotationCell) != 0;
                    sheet.Cells[insertRowNum, 8].Value = reversedValidCellTypeDictionary[dgnModelInfo.CellType];
                    insertRowNum ++;
                }
                sheet.Cells[1,1,sheet.Dimension.Rows,sheet.Dimension.Columns].AutoFitColumns(0);
                package.Save();
            }
            return 0;
        }

        private Dictionary<string, BD.DgnModelType> validModelTypeDictionary = new Dictionary<string, BD.DgnModelType>()
        {
            { "设计", BD.DgnModelType.Normal},
            { "绘图", BD.DgnModelType.Drawing},
            { "图纸", BD.DgnModelType.Sheet}
        };

        private Dictionary<int, bool> validModelDimensionDictionary = new Dictionary<int, bool>()
        {
            {2,false },
            {3,true }
        };

        private Dictionary<string, BD.CellLibraryType> validCellTypeDictionary = new Dictionary
            <string, BD.CellLibraryType>()
        {
            {"图形", BD.CellLibraryType.Graphic },
            {"点", BD.CellLibraryType.Point },
            {"参数化", BD.CellLibraryType.Parametric }
        };
    }

    enum ModelInfoStatus
    {
        Good,
        InvalidModelType,
        InvalidModelDimension,
        InvalidCellType
    }
    public class DgnModelInfo
    {
        public string ModelType { get; set; }
        public int ModelDimension { get; set; }
        public string ModelName { get; set; }
        public string Description { get; set; }
        public string RefName { get; set; }
        public bool CanBePutAsCell { get; set; }
        public bool CanBePutAsAnnotationCell { get; set; }
        public string CellType { get; set; }
    }
}