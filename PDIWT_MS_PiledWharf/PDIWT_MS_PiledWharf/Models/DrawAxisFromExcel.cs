using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using Bentley.MstnPlatformNET;
using Bentley.DgnPlatformNET;
using Bentley.DgnPlatformNET.Elements;
using Bentley.GeometryNET;
using Microsoft.Win32;
using OfficeOpenXml;
using EPPlus.DataExtractor;

namespace PDIWT_MS_PiledWharf.Models
{
    using PDIWT_MS_PiledWharf_CPP;
    using System.Windows;

    public class DrawAxisFromExcel
    {
        public DrawAxisFromExcel(FileInfo importexcelfileinfo)
        {
            ImportExcelFileInfo = importexcelfileinfo;
        }
        public FileInfo ImportExcelFileInfo { get; private set; }

        readonly double deg2rad = Math.PI / 180;

        List<PileAxisInfo> ExtractPileInfo()
        {
            List<PileAxisInfo> piles = new List<PileAxisInfo>();
            using (ExcelPackage package = new ExcelPackage(ImportExcelFileInfo))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets[1];
                piles = sheet.Extract<PileAxisInfo>()
                             .WithProperty(p => p.GridHorizontal, "A")
                             .WithProperty(p => p.GridVertical, "B")
                             .WithProperty(p => p.TopX, "C")
                             .WithProperty(p => p.TopY, "D")
                             .WithProperty(p => p.TopZ, "E")
                             .WithProperty(p => p.Skewness, "F")
                             .WithProperty(p => p.RotationDegree, "G")
                             .WithProperty(p => p.Length, "H")
                             .WithProperty(p => p.Type, "I")
                             .WithProperty(p => p.CrossSectionType, "J")
                             .WithProperty(p => p.SideLength, "K")
                             .WithProperty(p => p.InnerDiameter, "L")
                             .WithProperty(p => p.Weight, "M")
                             .WithProperty(p => p.UnderWaterWeight, "N")
                             .GetData(2, rowindex => rowindex <= sheet.Dimension.Rows)
                             .ToList();
            }
            return piles;
        }

        public StatusInt DrawAxies()
        {
            // Import pileAxis schema
            PDIWTSchemaImportSatutes schemaimportstatus = PileAxis.ImportPDIWTSchema();
            if (schemaimportstatus != PDIWTSchemaImportSatutes.Success_ImportSchema && schemaimportstatus != PDIWTSchemaImportSatutes.Success_UpdateSchema)
                return StatusInt.Error;

            // Extract pileinfos
            var piles = ExtractPileInfo();

            // Draw pile sequence
            double scale = Program.GetActiveDgnModel().GetModelInfo().UorPerMeter / 1000; //mm
            foreach (var pile in piles)
            {
                DPoint3d top = new DPoint3d(pile.TopX * scale, pile.TopY * scale, pile.TopZ * scale);
                DPoint3d bottom = new DPoint3d();
                if (pile.Skewness == 0)
                    bottom = top - pile.Length * scale * 1000 * DPoint3d.UnitZ;
                else
                {
                    double temp = pile.Length * scale * 1000 / (Math.Sqrt(pile.Skewness * pile.Skewness + 1));
                    bottom = top + new DPoint3d(Math.Cos(pile.RotationDegree * deg2rad), Math.Sin(pile.RotationDegree * deg2rad), -pile.Skewness) * temp;
                }
                //create axis
                LineElement _line;
                PileAxis.Create(new List<DPoint3d>() { top, bottom }, out _line);
                if (_line == null) return StatusInt.Error;
                _line.AddToModel();
                //attach property
                if (StatusInt.Error == PileAxis.AttachProperty(_line, pile)) return StatusInt.Error;

            }
            return StatusInt.Success;
        }
    }

    public static class DrawAxisFromExcelLogicalWrapper
    {
        public static void Do()
        {
            OpenFileDialog ofdlg = new OpenFileDialog()
            {
                Title = "打开桩轴线信息列表",
                Filter = Properties.Resources.ExcelFilter
            };
            try
            {
                if (ofdlg.ShowDialog() == true)
                {
                    DrawAxisFromExcel drawaxis = new DrawAxisFromExcel(new FileInfo(ofdlg.FileName));
                    StatusInt drawstatus = drawaxis.DrawAxies();
                    if (drawstatus == StatusInt.Error)
                        MessageCenter.Instance.ShowErrorMessage("绘制错误", "从Excel表格中绘制桩中心线过程中出现错误", MessageAlert.Balloon);
                    else
                        MessageCenter.Instance.ShowInfoMessage("绘制成功", "绘制成功", false);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
