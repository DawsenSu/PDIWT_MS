using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Collections.ObjectModel;

using Bentley.GeometryNET;

using OfficeOpenXml;

namespace PDIWT_MS_PiledWharf.Models
{
    using Extension.Attribute;
    using Piles;
    using Piles.CrossSection;
    using Soil;
    using Interface;

    public class OutputCalculationSheet
    {
        ObservableCollection<PileBase> m_piles;
        double m_gamma;
        double m_ta;
        double m_waterlevel;
        readonly string[] eachlayerinfoheader;

        public OutputCalculationSheet(ObservableCollection<PileBase> piles, double gamma, double ta, double waterlevel)
        {
            m_piles = piles;
            m_gamma = gamma;
            m_ta = ta;
            m_waterlevel = waterlevel;
            eachlayerinfoheader = new string[] { "土层号", "土层", "层顶高程", "土层厚度li/m", "极限侧摩阻力标准值qfi/kPa"/*, "极限桩端阻力标准值qr/kPa"*/, "qfi * li", "折减系数ξi" };
        }

        public void Export(string filepath)
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(filepath)))
            {
                #region 桩基承载力汇总
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
                totalsheet.Cells["B2"].Value = "桩长(m)";
                totalsheet.Cells["C2"].Value = "桩抗压承载力(kN)";
                totalsheet.Cells["D2"].Value = "桩抗拔承载力(kN)";

                int rowstart = 3;
                for (int i = 0; i < m_piles.Count; i++)
                {
                    totalsheet.Cells[rowstart + i, 1].Value = m_piles[i].Code;
                    totalsheet.Cells[rowstart + i, 2].Value = (m_piles[i].TopPoint - m_piles[i].BottomPoint).Magnitude;
                    totalsheet.Cells[rowstart + i, 3].Value = m_piles[i].CalculateQd(m_gamma, m_ta);
                    totalsheet.Cells[rowstart + i, 4].Value = m_piles[i].CalculateTd(m_gamma, m_waterlevel);
                }
                var alltotalsheetcells = totalsheet.Cells[1, 1, totalsheet.Dimension.Rows, totalsheet.Dimension.Columns];
                alltotalsheetcells.Style.Numberformat.Format = "#0.00";
                alltotalsheetcells.AutoFitColumns(0);
                #endregion

                #region 桩基承载力分项计算
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
                    //subsheet.Cells[startrowindex + 2, 3].Value = "桩径D(m)=";
                    //subsheet.Cells[startrowindex + 2, 5].Value = "孔径d(m)=";
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

                    var actualpile = m_piles[i];
                    ObservableCollection<PilePieceInSoilLayerInfo> _pilesoillayer = actualpile.GetPilePieceInEachSoilLayerInfos();

                    subsheet.Cells[startrowindex, 3].Value = m_gamma;
                    pilesoillayernum = _pilesoillayer.Count;
                    //填充数据项
                    subsheet.Cells[startrowindex, 3].Value = m_gamma;
                    subsheet.Cells[startrowindex + 2, 2].Value = GetPileCrossSectionString(actualpile.ICrossSection);
                    //subsheet.Cells[startrowindex + 2, 4].Value = actualpile.outdiameter;
                    //subsheet.Cells[startrowindex + 2, 6].Value = actualpile.PilePropertyInfo.PileInnerDiameter;
                    SetCrossSectionInfo(subsheet.Cells[startrowindex + 2, 3], subsheet.Cells[startrowindex + 2, 5], subsheet.Cells[startrowindex + 2, 4], subsheet.Cells[startrowindex + 2, 6], actualpile.ICrossSection);
                    subsheet.Cells[startrowindex + 3, 2].Value = (actualpile.TopPoint - actualpile.BottomPoint).Magnitude;
                    subsheet.Cells[startrowindex + 3, 4].Value = actualpile.TopPoint.Z;
                    subsheet.Cells[startrowindex + 3, 6].Value = actualpile.BottomPoint.Z;
                    subsheet.Cells[startrowindex + 4, 2].Value = actualpile.ICrossSection.GetOutPerimeter(0);
                    subsheet.Cells[startrowindex + 4, 4].Value = actualpile.ICrossSection.GetBottomSectionArea();
                    subsheet.Cells[startrowindex + 4, 6].Value = actualpile.ICrossSection.GetActualSectionArea(0);
                    subsheet.Cells[startrowindex + 5, 2].Value = actualpile.UnitWeight;
                    subsheet.Cells[startrowindex + 5, 4].Value = actualpile.UnderWaterUnitWeight;
                    subsheet.Cells[startrowindex + 5, 6].Value = m_waterlevel;
                    double cosaphla = (actualpile.BottomPoint - actualpile.TopPoint).AngleTo(DVector3d.FromXYZ(0,0,-1)).Cos;
                    subsheet.Cells[startrowindex + 6, 2].Value = GetPileSkewnessString(cosaphla);
                    subsheet.Cells[startrowindex + 6, 4].Value = cosaphla;
                    subsheet.Cells[startrowindex + 7, 2].Value = actualpile.Code;

                    for (int j = 0; j < pilesoillayernum; j++)
                    {
                        subsheet.Cells[startrowindex + j + 9, 1].Value = _pilesoillayer[j].CurrentSoilLayerInfo.SoilLayerNum;
                        subsheet.Cells[startrowindex + j + 9, 2].Value = _pilesoillayer[j].CurrentSoilLayerInfo.SoilLayerTypeName;
                        subsheet.Cells[startrowindex + j + 9, 3].Value = _pilesoillayer[j].PileTopZ_InCurrentSoilLayer;
                        subsheet.Cells[startrowindex + j + 9, 4].Value = _pilesoillayer[j].PilePieceLength;
                        subsheet.Cells[startrowindex + j + 9, 5].Value = _pilesoillayer[j].CurrentSoilLayerInfo.Qfi;
                        //subsheet.Cells[startrowindex + j + 9, 6].Value = actualpile.Qr;//应改改为qri
                        subsheet.Cells[startrowindex + j + 9, 6].FormulaR1C1 = "RC[-1] * RC[-2]";
                        subsheet.Cells[startrowindex + j + 9, 7].Value = _pilesoillayer[j].CurrentSoilLayerInfo.Xii;
                    }
                    subsheet.Cells[startrowindex + pilesoillayernum + 9, 2].Value = _pilesoillayer.Last().CurrentSoilLayerInfo.SoilLayerTypeName;
                    subsheet.Cells[startrowindex + pilesoillayernum + 9, 4].Value = _pilesoillayer.Last().CurrentSoilLayerInfo.Qri;
                    subsheet.Cells[startrowindex + pilesoillayernum + 10, 2].Value = actualpile.CalculateQd(m_gamma, m_ta);
                    subsheet.Cells[startrowindex + pilesoillayernum + 10, 4].Value = actualpile.CalculateTd(m_gamma, m_waterlevel);
                    if (actualpile is SteelPCPile)
                    {
                        subsheet.Cells[startrowindex + pilesoillayernum + 9, 5].Value = "承载力折减系数η=";
                        subsheet.Cells[startrowindex + pilesoillayernum + 9, 6].Value = m_ta;
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
                allsubsheetcells.AutoFitColumns(0);
                #endregion

                package.Save();
            }
        }

        string GetPileCrossSectionString(IPileCrossSection pileproperty)
        {
            return pileproperty.GetType().GetCustomAttribute<EnumDisplayNameAttribute>().DisplayName;
        }

        string GetPileSkewnessString(double cosa)
        {
            if (Math.Abs(cosa - 1) < 1e-4)
            {
                return "直桩";
            }
            else
            {
                return string.Format("1:{0}", Math.Round(1.0 / Math.Sqrt(1 - cosa * cosa) / cosa, 0));
            }
        }
    
        void SetCrossSectionInfo(ExcelRange sidelength, ExcelRange innerdiameter, ExcelRange sidelengtvalue, ExcelRange innerdiametervalue, IPileCrossSection crossSection)
        {
            if(crossSection is AnnularCrossSection)
            {
                sidelength.Value = "桩径D(m)=";
                innerdiameter.Value = "孔径d(m)=";
                var cs = crossSection as AnnularCrossSection;
                sidelengtvalue.Value = cs.OuterDiameter;
                innerdiametervalue.Value = cs.InnerDiameter;
            }
            else if(crossSection is SquareCrossSection)
            {
                sidelength.Value = "边长L(m)=";
                var cs = crossSection as SquareCrossSection;
                sidelengtvalue.Value = cs.SideLength;
            }
            else if(crossSection is SquareWithRoundHoleCrossSection)
            {
                sidelength.Value = "边长L(m)=";
                innerdiameter.Value = "孔径d(m)=";
                var cs = crossSection as SquareWithRoundHoleCrossSection;
                sidelengtvalue.Value = cs.SideLength;
                innerdiametervalue.Value = cs.HoleDiameter;
            }
        }
    }
}
