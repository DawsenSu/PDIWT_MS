using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using OfficeOpenXml;

namespace PDIWT_MS_ZJCZL.Models
{
    using PDIWT_MS_ZJCZL.Models.Piles;
    using PDIWT_MS_ZJCZL.Models.Soil;
    using PDIWT_MS_ZJCZL.Models.Factory;
    using PDIWT_MS_ZJCZL.Models.PileCrossSection;
    using PDIWT_MS_ZJCZL.Interface;
    using ViewModels;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    public class ExportCalculationSheet
    {

        List<PileBase> m_piles = new List<PileBase>();
        readonly Dictionary<Type, PileType> pileTypeDict;
        readonly Dictionary<Type, SoildPileCrossSectionType> pileCrossSectionDict;
        readonly string[] eachlayerinfoheader;

        public ExportCalculationSheet(List<PileBase> piles)
        {
            m_piles = piles;
            pileTypeDict = new Dictionary<Type, PileType>
            {
                { typeof(SolidPile), PileType.Solid },
                { typeof(SteelAndPercastConcretePile), PileType.SteelAndPercastConcrete }
            };
            pileCrossSectionDict = new Dictionary<Type, SoildPileCrossSectionType>
            {
                { typeof(SquarePileGeometry), SoildPileCrossSectionType.Square },
                { typeof(SquareWithRoundHolePileGeometry), SoildPileCrossSectionType.SquareWithRoundHole },
                { typeof(PolygonPileGeometry), SoildPileCrossSectionType.Polygon }
            };
            eachlayerinfoheader = new string[] { "土层号", "土层", "层顶高程", "土层厚度li/m", "极限侧摩阻力标准值qfi/kPa"/*, "极限桩端阻力标准值qr/kPa"*/, "qfi * li", "折减系数ξi" };
        }

        public void Export(string filepath)
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(filepath)))
            {
                ExcelWorksheet totalsheet = package.Workbook.Worksheets.Add("桩基承载力汇总");
                totalsheet.Cells["A1:D1"].Merge = true;
                using (var title = totalsheet.Cells["A1"])
                {
                    title.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    title.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    title.Style.Font.Bold = true;
                    title.Style.Font.Size = 15;
                    title.Value = "计算结果汇总";
                }
                using (var header = totalsheet.Cells["A2:D2"])
                {
                    header.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    header.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    header.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    header.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);
                    header.Style.Font.Bold = true;
                    header.Style.Font.Size = 10;
                }
                totalsheet.Cells["A2"].Value = "桩编号";
                totalsheet.Cells["B2"].Value = "桩桩长";
                totalsheet.Cells["C2"].Value = "桩抗压承载力(kN)";
                totalsheet.Cells["D2"].Value = "桩抗拔承载力(kN)";

                int rowstart = 3;
                for (int i = 0; i < m_piles.Count; i++)
                {
                    totalsheet.Cells[rowstart + i, 1].Value = m_piles[i].PileCode;
                    totalsheet.Cells[rowstart + i, 2].Value = m_piles[i].PilePropertyInfo.GetPileLength();
                    totalsheet.Cells[rowstart + i, 3].Value = m_piles[i].CalculateQd();
                    totalsheet.Cells[rowstart + i, 4].Value = m_piles[i].CalculateTd();
                }
                var alltotalsheetcells = totalsheet.Cells[1, 1, totalsheet.Dimension.Rows, totalsheet.Dimension.Columns];
                alltotalsheetcells.Style.Numberformat.Format = "#0.00";
                alltotalsheetcells.AutoFitColumns(0);

                ExcelWorksheet subsheet = package.Workbook.Worksheets.Add("桩基承载力分项计算");
                int startrowindex = 1;
                int pilesoillayernum = 0;
                for (int i = 0; i < m_piles.Count; i++)
                {
                    subsheet.Cells[startrowindex, 1].Style.Font.Bold = true;
                    subsheet.Cells[startrowindex, 1].Value = "计算参数:";
                    subsheet.Cells[startrowindex, 2].Value = "分项系数γR=";
                    subsheet.Cells[startrowindex, 4].Value = "拉拔取一样";
                    subsheet.Cells[startrowindex + 1, 1].Style.Font.Bold = true;
                    subsheet.Cells[startrowindex + 1, 1].Value = "桩参数:";
                    subsheet.Cells[startrowindex + 2, 1].Value = "桩型=";
                    subsheet.Cells[startrowindex + 2, 3].Value = "桩径D(m)=";
                    subsheet.Cells[startrowindex + 2, 5].Value = "孔径d(m)=";
                    subsheet.Cells[startrowindex + 3, 1].Value = "桩长(m)=";
                    subsheet.Cells[startrowindex + 3, 3].Value = "桩顶标高(m)=";
                    subsheet.Cells[startrowindex + 3, 5].Value = "桩尖标高(m)=";
                    subsheet.Cells[startrowindex + 4, 1].Value = "周长(m)=";
                    subsheet.Cells[startrowindex + 4, 3].Value = "外轮廓面积(m^2)=";
                    subsheet.Cells[startrowindex + 4, 5].Value = "截面积(m^2)=";
                    subsheet.Cells[startrowindex + 5, 1].Value = "重度(kN/m^3)=";
                    subsheet.Cells[startrowindex + 5, 3].Value = "浮重度γ浮(kN/m^3)=";
                    subsheet.Cells[startrowindex + 5, 5].Value = "施工水位(m)=";
                    subsheet.Cells[startrowindex + 6, 1].Value = "桩斜度=";
                    subsheet.Cells[startrowindex + 6, 3].Value = "α(rad)=";
                    subsheet.Cells[startrowindex + 7, 1].Value = "桩编号：";

                    subsheet.Cells[startrowindex + 7, 1].Style.Font.Bold = true;
                    subsheet.Row(startrowindex + 8).Style.WrapText = true;
                    subsheet.Row(startrowindex + 8).Style.Font.Bold = true;
                    subsheet.Row(startrowindex + 8).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    subsheet.Row(startrowindex + 8).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    for (int j = 0; j < eachlayerinfoheader.Length; j++)
                    {
                        subsheet.Cells[startrowindex + 8, j + 1].Value = eachlayerinfoheader[j];
                        subsheet.Cells[startrowindex + 8, j + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }

                    if (m_piles[i] is SolidPile)
                    {
                        var actualpile = (SolidPile)m_piles[i];
                        subsheet.Cells[startrowindex, 3].Value = actualpile.GammaR;
                        pilesoillayernum = actualpile.SolidPileSoilLayerInfoProp.Count;
                        //填充数据项
                        subsheet.Cells[startrowindex, 3].Value = actualpile.GammaR;
                        subsheet.Cells[startrowindex + 2, 2].Value = GetPileCrossSectionString(actualpile.PilePropertyInfo);
                        subsheet.Cells[startrowindex + 2, 4].Value = actualpile.PilePropertyInfo.PileDiameter;
                        subsheet.Cells[startrowindex + 2, 6].Value = actualpile.PilePropertyInfo.PileInnerDiameter;
                        subsheet.Cells[startrowindex + 3, 2].Value = actualpile.PilePropertyInfo.GetPileLength();
                        subsheet.Cells[startrowindex + 3, 4].Value = actualpile.PilePropertyInfo.PileTopPoint.Z;
                        subsheet.Cells[startrowindex + 3, 6].Value = actualpile.PilePropertyInfo.PileBottomPoint.Z;
                        subsheet.Cells[startrowindex + 4, 2].Value = actualpile.PilePropertyInfo.GetPilePerimeter();
                        subsheet.Cells[startrowindex + 4, 4].Value = actualpile.PilePropertyInfo.GetPileOutLineArea();
                        subsheet.Cells[startrowindex + 4, 6].Value = actualpile.PilePropertyInfo.GetPileCrossSectionArea();
                        subsheet.Cells[startrowindex + 5, 2].Value = actualpile.PilePropertyInfo.PileWeight;
                        subsheet.Cells[startrowindex + 5, 4].Value = actualpile.PilePropertyInfo.PileUnderWaterWeight;
                        subsheet.Cells[startrowindex + 5, 6].Value = actualpile.PilePropertyInfo.WaterLevel;
                        subsheet.Cells[startrowindex + 6, 2].Value = Utilities.GetPileSkewnessString(actualpile.PilePropertyInfo.GetCosAlpha());
                        subsheet.Cells[startrowindex + 6, 4].Value = Math.Acos(actualpile.PilePropertyInfo.GetCosAlpha());
                        subsheet.Cells[startrowindex + 7, 2].Value = actualpile.PileCode;
                        for (int j = 0; j < actualpile.SolidPileSoilLayerInfoProp.Count; j++)
                        {
                            subsheet.Cells[startrowindex + j + 9, 1].Value = actualpile.SolidPileSoilLayerInfoProp[j].SoilLayerNum;
                            subsheet.Cells[startrowindex + j + 9, 2].Value = actualpile.SolidPileSoilLayerInfoProp[j].SoilLayerName;
                            subsheet.Cells[startrowindex + j + 9, 3].Value = actualpile.SolidPileSoilLayerInfoProp[j].PileInSoilLayerTopZ;
                            subsheet.Cells[startrowindex + j + 9, 4].Value = actualpile.SolidPileSoilLayerInfoProp[j].PileInSoilLayerLength;
                            subsheet.Cells[startrowindex + j + 9, 5].Value = actualpile.SolidPileSoilLayerInfoProp[j].Qfi;
                            //subsheet.Cells[startrowindex + j + 9, 6].Value = actualpile.Qr;//应改改为qri
                            subsheet.Cells[startrowindex + j + 9, 6].FormulaR1C1 = "RC[-1] * RC[-2]";
                            subsheet.Cells[startrowindex + j + 9, 7].Value = actualpile.SolidPileSoilLayerInfoProp[j].Xii;
                        }
                        subsheet.Cells[startrowindex + pilesoillayernum + 9, 2].Value = actualpile.SolidPileSoilLayerInfoProp.Last().SoilLayerName;
                        subsheet.Cells[startrowindex + pilesoillayernum + 9, 4].Value = actualpile.Qr;
                        subsheet.Cells[startrowindex + pilesoillayernum + 10, 2].Value = actualpile.CalculateQd();
                        subsheet.Cells[startrowindex + pilesoillayernum + 10, 4].Value = actualpile.CalculateTd();

                    }
                    if (m_piles[i] is SteelAndPercastConcretePile)
                    {
                        var actualpile = (SteelAndPercastConcretePile)m_piles[i];
                        pilesoillayernum = actualpile.SteelAndPercastConcretPileLayerInfoProp.Count;
                        //填充数据项
                        subsheet.Cells[startrowindex, 3].Value = actualpile.GammaR;
                        subsheet.Cells[startrowindex + 2, 2].Value = GetPileCrossSectionString(actualpile.PilePropertyInfo);
                        subsheet.Cells[startrowindex + 2, 4].Value = actualpile.PilePropertyInfo.PileDiameter;
                        subsheet.Cells[startrowindex + 2, 6].Value = actualpile.PilePropertyInfo.PileInnerDiameter;
                        subsheet.Cells[startrowindex + 3, 2].Value = actualpile.PilePropertyInfo.GetPileLength();
                        subsheet.Cells[startrowindex + 3, 4].Value = actualpile.PilePropertyInfo.PileTopPoint.Z;
                        subsheet.Cells[startrowindex + 3, 6].Value = actualpile.PilePropertyInfo.PileBottomPoint.Z;
                        subsheet.Cells[startrowindex + 4, 2].Value = actualpile.PilePropertyInfo.GetPilePerimeter();
                        subsheet.Cells[startrowindex + 4, 4].Value = actualpile.PilePropertyInfo.GetPileOutLineArea();
                        subsheet.Cells[startrowindex + 4, 6].Value = actualpile.PilePropertyInfo.GetPileCrossSectionArea();
                        subsheet.Cells[startrowindex + 5, 2].Value = actualpile.PilePropertyInfo.PileWeight;
                        subsheet.Cells[startrowindex + 5, 4].Value = actualpile.PilePropertyInfo.PileUnderWaterWeight;
                        subsheet.Cells[startrowindex + 5, 6].Value = actualpile.PilePropertyInfo.WaterLevel;
                        subsheet.Cells[startrowindex + 6, 2].Value = Utilities.GetPileSkewnessString(actualpile.PilePropertyInfo.GetCosAlpha());
                        subsheet.Cells[startrowindex + 6, 4].Value = actualpile.PilePropertyInfo.GetCosAlpha();
                        subsheet.Cells[startrowindex + 7, 2].Value = actualpile.PileCode;
                        for (int j = 0; j < actualpile.SteelAndPercastConcretPileLayerInfoProp.Count; j++)
                        {
                            subsheet.Cells[startrowindex + j + 9, 1].Value = actualpile.SteelAndPercastConcretPileLayerInfoProp[j].SoilLayerNum;
                            subsheet.Cells[startrowindex + j + 9, 2].Value = actualpile.SteelAndPercastConcretPileLayerInfoProp[j].SoilLayerName;
                            subsheet.Cells[startrowindex + j + 9, 3].Value = actualpile.SteelAndPercastConcretPileLayerInfoProp[j].PileInSoilLayerTopZ;
                            subsheet.Cells[startrowindex + j + 9, 4].Value = actualpile.SteelAndPercastConcretPileLayerInfoProp[j].PileInSoilLayerLength;
                            subsheet.Cells[startrowindex + j + 9, 5].Value = actualpile.SteelAndPercastConcretPileLayerInfoProp[j].Qfi;
                            //subsheet.Cells[startrowindex + j + 9, 6].Value = actualpile.Qr;//应改改为qri
                            subsheet.Cells[startrowindex + j + 9, 6].FormulaR1C1 = "RC[-1] * RC[-2]";
                            subsheet.Cells[startrowindex + j + 9, 7].Value = actualpile.SteelAndPercastConcretPileLayerInfoProp[j].Xii;

                        }
                        subsheet.Cells[startrowindex + pilesoillayernum + 9, 2].Value = actualpile.SteelAndPercastConcretPileLayerInfoProp.Last().SoilLayerName;
                        subsheet.Cells[startrowindex + pilesoillayernum + 9, 5].Value = "承载力折减系数η=";
                        subsheet.Cells[startrowindex + pilesoillayernum + 9, 4].Value = actualpile.Qr;
                        subsheet.Cells[startrowindex + pilesoillayernum + 9, 6].Value = actualpile.Eta;
                        subsheet.Cells[startrowindex + pilesoillayernum + 10, 2].Value = actualpile.CalculateQd();
                        subsheet.Cells[startrowindex + pilesoillayernum + 10, 4].Value = actualpile.CalculateTd();
                    }
                    subsheet.Cells[startrowindex + pilesoillayernum + 9, 1].Value = "持力层=";
                    subsheet.Cells[startrowindex + pilesoillayernum + 9, 3].Value = "桩端阻力Qr(kPa)=";
                    subsheet.Cells[startrowindex + pilesoillayernum + 10, 1].Value = "承载力设计值Qd(kN)=";
                    subsheet.Cells[startrowindex + pilesoillayernum + 10, 3].Value = "抗拔承载力设计值Td(kN)=";

                    subsheet.Cells[startrowindex + pilesoillayernum + 9, 1].Style.Font.Bold = true;
                    subsheet.Cells[startrowindex + pilesoillayernum + 9, 3].Style.Font.Bold = true;
                    subsheet.Cells[startrowindex + pilesoillayernum + 9, 5].Style.Font.Bold = true;
                    subsheet.Cells[startrowindex + pilesoillayernum + 10, 1].Style.Font.Bold = true;
                    subsheet.Cells[startrowindex + pilesoillayernum + 10, 3].Style.Font.Bold = true;

                    var table = subsheet.Cells[startrowindex + 9, 1, startrowindex + pilesoillayernum + 8, subsheet.Dimension.Columns];
                    table.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    table.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    table.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    subsheet.Cells[startrowindex + pilesoillayernum + 11, 1, startrowindex + pilesoillayernum + 11, subsheet.Dimension.Columns].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    subsheet.Cells[startrowindex + pilesoillayernum + 11, 1, startrowindex + pilesoillayernum + 11, subsheet.Dimension.Columns].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightPink);
                    startrowindex += pilesoillayernum + 12;
                }
                var allsubsheetcells = subsheet.Cells[1, 1, subsheet.Dimension.Rows, subsheet.Dimension.Columns];
                allsubsheetcells.Style.Numberformat.Format = "#0.00";
                //subsheet.Cells[startrowindex + 6, 4].Style.Numberformat.
                allsubsheetcells.AutoFitColumns(0);
                package.Save();
            }
        }

        string GetPileCrossSectionString(IPileProperty pileproperty)
        {
            if (pileCrossSectionDict.ContainsKey(pileproperty.GetType()))
            {
                string enumvalue = pileCrossSectionDict[pileproperty.GetType()].ToString();
                FieldInfo fieldinfo = pileCrossSectionDict[pileproperty.GetType()].GetType().GetField(enumvalue);
                var displayattribute = fieldinfo.GetCustomAttribute<DisplayAttribute>();
                if (displayattribute != null)
                    return displayattribute.Name;
                else
                    return enumvalue;
            }
            else
                return "环形截面桩";
        }
    }
}
