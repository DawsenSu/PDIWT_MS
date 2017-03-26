using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.Collections;
using System.IO;

using OfficeOpenXml;

namespace PDIWT_MS.Tools.Model
{
    public class ElementProp
    {
        public int CellNameType { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double AngelX { get; set; }
        public double AngelY { get; set; }
        public double AngelZ { get; set; }
        public ElementProp() { }
        public ElementProp(int i)
        {
            CellNameType = 0;
            X = Y = Z = AngelX = AngelY = AngelZ = i;
        }
    }

    public class CellName
    {
        public CellName(int i)
        {
            Id = i;
            Name = "CellName" + i;
        }
        public CellName() { }
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public interface IGetElementPropFromFile
    {
        ObservableCollection<ElementProp> GetElementPropList(FileInfo fileInfo, ObservableCollection<CellName> cellNameList);
    }

    public class GetElementPropFromExcel : IGetElementPropFromFile
    {
        public ObservableCollection<ElementProp> GetElementPropList(FileInfo fileInfo, ObservableCollection<CellName> cellNameList)
        {
            var outputlist = new ObservableCollection<ElementProp>();
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets[1];
                int rownum = sheet.Dimension.Rows;
                int tempNameType;
                for (int i = 2; i < rownum + 1; i++)
                {
                    tempNameType = -1;
                    foreach (var cellName in cellNameList)
                    {
                        if (cellName.Name == sheet.Cells[i, 1].Text.Trim())
                        {
                            tempNameType = cellName.Id;
                            break;
                        }
                    }
                    if (tempNameType == -1)
                    {
                        throw new InvalidDataException("所加载单元库中不存在" + sheet.Cells[i, 1].Text + "的单元！");
                    }
                    outputlist.Add(new ElementProp
                    {
                        CellNameType = tempNameType,
                        X = ConvertXYZ(sheet.Cells[i, 2].Text,i),
                        Y = ConvertXYZ(sheet.Cells[i, 3].Text,i),
                        Z = ConvertXYZ(sheet.Cells[i, 4].Text,i),
                        AngelX = ConvertAngelXYZ(sheet.Cells[i, 5].Text,i),
                        AngelY = ConvertAngelXYZ(sheet.Cells[i, 6].Text,i),
                        AngelZ = ConvertAngelXYZ(sheet.Cells[i, 7].Text,i)
                    });
                }
            }
            return outputlist;
        }
        private double ConvertXYZ(string xyz, int i)
        {
            double temp;
            if (!double.TryParse(xyz, out temp))
            {
                throw new InvalidDataException("第" + i.ToString() + "行：{" + xyz + "}不能转换为数字!");
            }
            return temp;
        }

        private double ConvertAngelXYZ(string xyz, int i)
        {
            double tempangel = ConvertXYZ(xyz, i);
            if (tempangel > 360 || tempangel < -360)
            {
                throw new InvalidDataException("第" + i.ToString() + "行：{" + tempangel + "}不在[-360,360]区间！");
            }
            return tempangel;
        }
    }
}
