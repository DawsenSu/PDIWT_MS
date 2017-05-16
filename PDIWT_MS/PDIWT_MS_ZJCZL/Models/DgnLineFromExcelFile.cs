using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using BD = Bentley.DgnPlatformNET;
using BG = Bentley.GeometryNET;
using BDE = Bentley.DgnPlatformNET.Elements;
using BM = Bentley.MstnPlatformNET;

using OfficeOpenXml;
using EPPlus.DataExtractor;

namespace PDIWT_MS_ZJCZL.Models
{
    public class DgnLineFromExcelFile
    {
        public DgnLineFromExcelFile(FileInfo importexcelfileinfo)
        {
            ImportExcelFileInfo = importexcelfileinfo;
        }
        public FileInfo ImportExcelFileInfo { get; set; }

        readonly double deg2rad = Math.PI / 180;

        List<PileExtractInfomation> ExtractPileInfo()
        {
            List<PileExtractInfomation> piles = new List<PileExtractInfomation>();
            using (ExcelPackage package = new ExcelPackage(ImportExcelFileInfo))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets[1];
                piles = sheet
                        .Extract<PileExtractInfomation>()
                        .WithProperty(p => p.PileCode, "A")
                        .WithProperty(p => p.TopX, "B")
                        .WithProperty(p => p.TopY, "C")
                        .WithProperty(p => p.TopZ, "D")
                        .WithProperty(p => p.Skewness, "E")
                        .WithProperty(p => p.RotationDegree, "F")
                        .WithProperty(p => p.Length, "G")
                        .GetData(2, (rowindex) => { if (rowindex > sheet.Dimension.Rows) return false; else return true; })
                        .ToList();

            }
            return piles;
        }

        public void DrawLines()
        {
            var activemodel = Program.GetActiveDgnModel();
            var levelcache = activemodel.GetFileLevelCache();
            var levelhandle = levelcache.GetLevelByName("PileAxis");
            if (levelhandle.Status == BD.LevelCacheErrorCode.CannotFindLevel)
            {
                levelhandle = levelcache.CreateLevel("PileAxis");
            }
            levelcache.Write();
            var leveledithandle = levelhandle.GetEditHandle();
            leveledithandle.Description = "桩的轴中心线";
            BM.SettingsActivator.SetActiveLevel(levelhandle.LevelId);

            BD.ElementPropertiesSetter levelsetter = new BD.ElementPropertiesSetter();
            levelsetter.SetLevel(levelhandle.LevelId);
            levelsetter.SetColor(3);

            BG.DPoint3d top, bottom;
            var pilelist = ExtractPileInfo();
            foreach (var pile in pilelist)
            {
                top = new BG.DPoint3d(pile.TopX, pile.TopY, pile.TopZ);
                if (pile.Skewness == 0)
                    bottom = top - BG.DPoint3d.UnitZ * pile.Length;
                else
                {
                    double temp = pile.Length / (Math.Sqrt(pile.Skewness * pile.Skewness + 1));
                    bottom = top + new BG.DPoint3d(Math.Cos(pile.RotationDegree * deg2rad), Math.Sin(pile.RotationDegree * deg2rad), -pile.Skewness) * temp;
                }
                BG.DSegment3d lineseg = new BG.DSegment3d(top * 10, bottom * 10);
                BDE.LineElement line = new BDE.LineElement(activemodel, null, lineseg);
                levelsetter.Apply(line);

                var wb = new BDE.WriteDataBlock();
                wb.WriteString(pile.PileCode);
                line.AppendLinkage((ushort)BD.ElementLinkageId.String, wb);
                line.AddToModel();
            }
        }
    }
    public class PileExtractInfomation
    {
        public string PileCode { get; set; }
        public double TopX { get; set; }
        public double TopY { get; set; }
        public double TopZ { get; set; }
        public double Skewness { get; set; }
        public double RotationDegree { get; set; }
        public double Length { get; set; }
    }
}
