using System;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using System.Windows.Forms;
using System.IO;
using System.Windows;
using System.Collections.Generic;

using PDIWT_MS_CZ.Models;
using OfficeOpenXml;

namespace PDIWT_MS_CZ.ViewModels
{
    public class ExcelOutputViewModel : ViewModelBase
    {
        //private ExcelOutputViewModel()
        //{
        //    ProjectName = "红花船闸";
        //    ProjectPhase = "初设";
        //    DesignPerson = "张丽媛";
        //    CheckPerson = "王帅";
        //}
        //public static readonly ExcelOutputViewModel instance = new ExcelOutputViewModel();

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            ProjectName = "红花船闸";
            ProjectPhase = "初设";
            DesignPerson = "张丽媛";
            CheckPerson = "王帅";
            //Quantity = CreateContent();
        }

        public string ProjectName
        {
            get { return GetProperty(() => ProjectName); }
            set { SetProperty(() => ProjectName, value); }
        }
        public string ProjectPhase
        {
            get { return GetProperty(() => ProjectPhase); }
            set { SetProperty(() => ProjectPhase, value); }
        }
        public string DesignPerson
        {
            get { return GetProperty(() => DesignPerson); }
            set { SetProperty(() => DesignPerson, value); }
        }
        public string CheckPerson
        {
            get { return GetProperty(() => CheckPerson); }
            set { SetProperty(() => CheckPerson, value); }
        }
        public List<QuantityRowDef> Quantity
        {
            get { return GetProperty(() => Quantity); }
            set { SetProperty(() => Quantity, value); }
        }


        [Command]
        public void Output()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "选择保存工程量表的路径";
            sfd.Filter = "Excel 2007-2016文件|*.xlsx";
            sfd.RestoreDirectory = true;
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(sfd.FileName))
                {
                    File.Delete(sfd.FileName);
                }
                OutputQuantity(new FileInfo(sfd.FileName));
                
                System.Windows.MessageBox.Show($"工程量表生成完成\n文件保存至{sfd.FileName}", "输出完成", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        
        private void OutputQuantity(FileInfo outputfileinfo)
        {
            using (ExcelPackage package = new ExcelPackage(outputfileinfo))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("工程量清单");
                //表头
                sheet.Cells["A1:H1"].Merge = true;
                sheet.Cells["A1"].Value = "闸首工程量表";
                sheet.Cells["A1"].Style.Font.Bold = true;
                sheet.Cells["A1"].Style.Font.Size = 14;
                //基本信息
                sheet.Cells["A2"].Value = "工程名称：";
                sheet.Cells["B2:D2"].Merge = true;
                sheet.Cells["B2"].Value = ProjectName;
                sheet.Cells["B2"].Style.Font.Bold = true;
                sheet.Cells["E2"].Value = "设计阶段：";
                sheet.Cells["F2"].Value = ProjectPhase;
                sheet.Cells["A3"].Value = "设计人员：";
                sheet.Cells["B3"].Value = DesignPerson;
                sheet.Cells["C3"].Value = "校核人员：";
                sheet.Cells["D3"].Value = CheckPerson;
                sheet.Cells["E3"].Value = "提取时间：";
                sheet.Cells["F3"].Value = DateTime.Now.ToShortDateString();
                //内容表头
                for (int i = 0; i < m_columeName.Length; i++)
                {
                    sheet.Cells[5, i + 1].Value = m_columeName[i];
                }
                //生成表格内容
                int rowindent = 6, row;
                for (int i = 0; i < Quantity.Count; i++)
                {
                    row = i + rowindent;
                    sheet.Cells[row, 1].Value = Quantity[i].ItemOrder;
                    sheet.Cells[row, 2].Value = Quantity[i].ItemName;
                    sheet.Cells[row, 3].Value = Quantity[i].Unit;
                    sheet.Cells[row, 4].Value = Quantity[i].QuantityFormula;
                    sheet.Cells[row, 5].Value = Quantity[i].Quantity;
                    sheet.Cells[row, 6].Value = Quantity[i].EnlargeFactor;
                    sheet.Cells[row, 7].FormulaR1C1 = "RC[-2]*RC[-1]";
                    sheet.Cells[row, 8].Value = Quantity[i].Memo;
                }
                //表格样式
                sheet.TabColor = System.Drawing.Color.Red;
                //sheet.Cells["A4:G4"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                //sheet.Cells["A4:G4"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);
                sheet.Cells["A4:H4"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.DashDot;
                sheet.Cells["A4:H4"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                var redfontrow = new int[] { 4, 9, 10, 11, 16, 17, 21 };
                foreach (var redrow in redfontrow)
                {
                    int worldredfontrow = redrow + 5;
                    sheet.Row(worldredfontrow).Style.Font.Color.SetColor(System.Drawing.Color.Red);
                }
                sheet.Cells[6, 5, 5 + Quantity.Count, 7].Style.Numberformat.Format = "#,#0.0";
                using (var totalCells = sheet.Cells[1, 1, 5 + Quantity.Count, m_columeName.Length])
                {
                    totalCells.Style.Font.Size = 10;
                    totalCells.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
                    totalCells.AutoFitColumns(0);
                    totalCells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                //set some document properties
                package.Workbook.Properties.Author = "设计一所 苏东升";
                package.Workbook.Properties.SetCustomPropertyValue("作者邮箱", "sudongsheng@pdiwt.com.cn");
                package.Workbook.Properties.Comments = "由参数化船闸模块自动生成";
                package.Workbook.Properties.Company = "中交水运规划设计院有限公司";
                package.Save();
            }
        }
        #region Field
        List<QuantityRowDef> TableContent { get; set; }
        string[] m_columeName = new string[] { "序号", "分部分项工程名称", "单位", "计算公式", "计算工程量", "扩大系数", "提交工程量","备注" };
        #endregion

    }
}