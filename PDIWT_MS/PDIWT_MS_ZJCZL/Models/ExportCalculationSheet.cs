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

    public class ExportCalculationSheet
    {
        
        List<PileBase> m_piles = new List<PileBase>();

        public ExportCalculationSheet(List<PileBase> piles)
        {
            m_piles = piles;
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
                    totalsheet.Cells[rowstart + i, 4].Value = m_piles[i].CalculateQt();
                }
                totalsheet.Cells[1, 1, totalsheet.Dimension.Rows, 4].AutoFitColumns(0);

                ExcelWorksheet subsheet = package.Workbook.Worksheets.Add("桩基承载力分项计算");


                package.Save();
            }
        }
    }
}
