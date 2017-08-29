using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Bentley.ECObjects.Instance;
using DevExpress.Mvvm.Native;
using BD = Bentley.DgnPlatformNET;
using BG = Bentley.GeometryNET;
using BDE = Bentley.DgnPlatformNET.Elements;
using BM = Bentley.MstnPlatformNET;
using PDIWT_MS_ZJCZL_Re.Properties;

using OfficeOpenXml;
using EPPlus.DataExtractor;
namespace PDIWT_MS_ZJCZL_Re.Models
{
    class DrawLineElementFromExcelFile
    {
        public FileInfo PileAxisFileInfo { get; set; }
        readonly double deg2rad = Math.PI / 180;
        private string[] propertStrings =
        {
            "PileType", //0										
            "PileCrossSectionType", //1
            "PileAxisNet__x0028__H__x002D__V__x0029__", //2
            "PileSideLength__x0028__m__x0029__", //3
            "PileInnerSideLength__x0028__m__x0029__", //4
            "PileWeigth__x0028__kN__x002F__m__x0029__", //5
            "PileUnderWaterWeigth__x0028__kN__x002F__m__x0029__" //6
        };

        private Dictionary<PileType, string> piletypeStringDictionary = new Dictionary<PileType, string>()
        {
            {PileType.Solid, "实心桩或桩端封闭"},
            {PileType.SteelAndPercastConcrete, "钢管桩与预制混凝土管桩" }
        };

        private Dictionary<PileCrossSectionType, string> pileCrossSectionDictionary = new Dictionary
            <PileCrossSectionType, string>()
        {
            {PileCrossSectionType.Annular, "环形"},
            {PileCrossSectionType.Square, "方型截面"},
            {PileCrossSectionType.SquareWithCircleHole, "方形空心截面"},
            {PileCrossSectionType.Polygon, "多边形截面"}
        };

        public DrawLineElementFromExcelFile(string excelFilePath)
        {
            PileAxisFileInfo = new FileInfo(excelFilePath);
        }

        private List<PileAxisInfo> ExtractPileAxisInfo()
        {
            ExcelPackage package = new ExcelPackage(PileAxisFileInfo);
            ExcelWorksheet sheet = package.Workbook.Worksheets[1];
            var pileAxises = sheet
                            .Extract<PileAxisInfo>()
                            .WithProperty(p => p.PileCode, "A")
                            .WithProperty(p => p.TopX, "B")
                            .WithProperty(p => p.TopY, "C")
                            .WithProperty(p => p.TopZ, "D")
                            .WithProperty(p => p.Skewness, "E")
                            .WithProperty(p => p.RotationDegree, "F")
                            .WithProperty(p => p.Length, "G")
                            .WithProperty(p => p.Type, "H")
                            .WithProperty(p => p.CrossSectionType, "I")
                            .WithProperty(p => p.SideLength, "J")
                            .WithProperty(p => p.InnerSideLength, "K")
                            .WithProperty(p => p.Weight, "L")
                            .WithProperty(p => p.UnderWaterWeight, "M")
                            .GetData(2, rowindex => rowindex != sheet.Dimension.Rows + 1)
                            .ToList();
            package.Dispose();
            return pileAxises;
        }

        BD.ItemTypeLibrary CreateItemTypeLibrary()
        {
            BD.ItemTypeLibrary library = BD.ItemTypeLibrary.FindByName("PDIWT_ZJCZL", Program.GetActiveDgnFile());
            if (library == null)
            {
                library = BD.ItemTypeLibrary.Create("PDIWT_ZJCZL", Program.GetActiveDgnFile());
                BD.ItemType pile = library.AddItemType("PileProperty");
                BD.CustomProperty pileProperty = pile.AddProperty("PileType");
                pileProperty.Type = BD.CustomProperty.TypeKind.String;
                pileProperty.DefaultValue = "实心桩或桩端封闭";
                var pileCrossSection = pile.AddProperty("PileCrossSectionType");
                pileCrossSection.Type = BD.CustomProperty.TypeKind.String;
                pileCrossSection.DefaultValue = "方形截面";
                var pileCode = pile.AddProperty("PileAxisNet(H-V)");
                pileCode.Type = BD.CustomProperty.TypeKind.String;
                var pileSideLength = pile.AddProperty("PileSideLength(m)");
                pileSideLength.Type = BD.CustomProperty.TypeKind.Double;
                pileSideLength.DefaultValue = 1;
                var pileInnerSideLength = pile.AddProperty("PileInnerSideLength(m)");
                pileInnerSideLength.Type = BD.CustomProperty.TypeKind.Double;
                pileInnerSideLength.DefaultValue = 1;
                var pileWeigth = pile.AddProperty("PileWeigth(kN/m)");
                pileWeigth.Type = BD.CustomProperty.TypeKind.Double;
                pileWeigth.DefaultValue = 25;
                var pileUnderWaterWeight = pile.AddProperty("PileUnderWaterWeigth(kN/m)");
                pileUnderWaterWeight.Type = BD.CustomProperty.TypeKind.Double;
                pileUnderWaterWeight.DefaultValue = 15;
                library.Write();
            }
            return library;
        }

        BD.LevelHandle CreatePileAxisLevel()
        {
            var levelcache = Program.GetActiveDgnModel().GetFileLevelCache();
            string levelnametofind = "PileAxis";
            var levelhandle = levelcache.GetLevelByName(levelnametofind, true);
            if (levelhandle.IsValid == true)
            {
                return levelhandle;
            }
            else
            {
                var pileaxislevelhandle = levelcache.CreateLevel(levelnametofind);
                if (pileaxislevelhandle.IsValid)
                {
                    pileaxislevelhandle.Description = Resources.PileAxis;
                    pileaxislevelhandle.SetByLevelColor(new BD.LevelDefinitionColor(3, Program.GetActiveDgnFile()));
                    levelcache.Write();
                    return pileaxislevelhandle;
                }
                return null;
            }
        }

        private BD.StatusInt SetLineElementProp(PileAxisInfo pileAxisInfo, BDE.LineElement lineElement, BD.ItemTypeLibrary library)
        {
            BD.ItemType pileItemType = library.GetItemTypeByName("PileProperty");
            if (pileItemType == null) return BD.StatusInt.Error;
            BD.CustomItemHost pileAxisHost = new BD.CustomItemHost(lineElement, true);
            var pileAxisPropInstance = pileAxisHost.ApplyCustomItem(pileItemType);
            if (pileAxisPropInstance == null) return BD.StatusInt.Error;
            pileAxisPropInstance.SetString(propertStrings[0], piletypeStringDictionary[pileAxisInfo.Type]);
            pileAxisPropInstance.SetString(propertStrings[1], pileCrossSectionDictionary[pileAxisInfo.CrossSectionType]);
            pileAxisPropInstance.SetString(propertStrings[2], pileAxisInfo.PileCode);
            pileAxisPropInstance.SetDouble(propertStrings[3], pileAxisInfo.SideLength);
            pileAxisPropInstance.SetDouble(propertStrings[4], pileAxisInfo.InnerSideLength);
            pileAxisPropInstance.SetDouble(propertStrings[5], pileAxisInfo.Weight);
            pileAxisPropInstance.SetDouble(propertStrings[6], pileAxisInfo.UnderWaterWeight);
            return pileAxisPropInstance.ScheduleChanges(lineElement);
        }



        public void DrawLines()
        {
            var activemodel = Program.GetActiveDgnModel();
            //Create ItemTypeLibrary
            BD.ItemTypeLibrary pilePropItemTypeLibrary = CreateItemTypeLibrary();

            //Create PileAxisLevelHandle
            var pileaxislevelhandle = CreatePileAxisLevel();
            if (pileaxislevelhandle == null) return;
            BM.SettingsActivator.SetActiveLevel(pileaxislevelhandle.LevelId);
            BD.ElementPropertiesSetter levelSetter = new BD.ElementPropertiesSetter();
            levelSetter.SetLevel(pileaxislevelhandle.LevelId);

            //Get the PileAxisInfomation And Create the Pile and Schedule Itemtype
            var pileAxisInfoList = ExtractPileAxisInfo();

            BG.DPoint3d top, bottom;
            double uorPerMillimeter = activemodel.GetModelInfo().UorPerMeter / 1000;
            double tolerance = 1e-5;
            foreach (var pileAxisInfo in pileAxisInfoList)
            {
                top = BG.DPoint3d.FromXYZ(pileAxisInfo.TopX, pileAxisInfo.TopY, pileAxisInfo.TopZ);
                if (Math.Abs(pileAxisInfo.Skewness) < tolerance)
                {
                    bottom = top - BG.DPoint3d.UnitY * pileAxisInfo.Length;
                }
                else
                {
                    double temp = pileAxisInfo.Length / (Math.Sqrt(pileAxisInfo.Skewness * pileAxisInfo.Skewness + 1));
                    bottom = top + new BG.DPoint3d(Math.Cos(pileAxisInfo.RotationDegree * deg2rad), Math.Sin(pileAxisInfo.RotationDegree * deg2rad), -pileAxisInfo.Skewness) * temp;
                }
                top.ScaleInPlace(uorPerMillimeter);
                bottom.ScaleInPlace(uorPerMillimeter);

                BG.DSegment3d lineDSegment3D = new BG.DSegment3d(top, bottom);
                BDE.LineElement line = new BDE.LineElement(activemodel, null, lineDSegment3D);
                levelSetter.Apply(line);

                if (SetLineElementProp(pileAxisInfo, line, pilePropItemTypeLibrary) == BD.StatusInt.Error)
                {
                    BM.MessageCenter.Instance.ShowErrorMessage($"{pileAxisInfo.PileCode}桩写入属性失败", "写入属性失败", false);
                    continue;
                }
                line.AddToModel();
            }
        }
    }

    public class PileAxisInfo
    {
        public string PileCode { get; set; }
        public double TopX { get; set; }
        public double TopY { get; set; }
        public double TopZ { get; set; }
        public double Skewness { get; set; }
        public double RotationDegree { get; set; }
        public double Length { get; set; } //unit mm
        public PileType Type { get; set; }
        public PileCrossSectionType CrossSectionType { get; set; }
        public double SideLength { get; set; }
        public double InnerSideLength { get; set; }
        public double Weight { get; set; }
        public double UnderWaterWeight { get; set; }
    }

}
