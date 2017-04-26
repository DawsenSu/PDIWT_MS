using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;

using OfficeOpenXml;

namespace PDIWT_MS_CZ.Models
{
    //internal sealed class GetCZQuantityTable
    //{
    //    public GetCZQuantityTable(string projectName ,string projectPhase, string designPerson, string checkPerson, QuantityResult result)
    //    {
    //        ProjectName = projectName; ProjectPhase = projectPhase; DesignPerson = designPerson; CheckPerson = checkPerson; Result = result;
    //    }
    //    #region CZ QuantitiesTable Property
    //    public string ProjectName { get; set; }
    //    public string ProjectPhase { get; set; }
    //    public string DesignPerson { get; set; }
    //    public string CheckPerson { get; set; }
    //    public QuantityResult Result { get; set; }

    //    List<QuantityRowDef> TableContent { get; set; }
    //    string[] m_columeName = new string[] { "序号", "分部分项工程名称", "单位", "工程量", "扩大系数", "备注" };
    //    #endregion

    //    private List<QuantityRowDef> CreateContent()
    //    {
    //        var content = new List<QuantityRowDef>
    //        {
    //            new QuantityRowDef { ItemOrder=1, ItemName= "素混凝土垫层", Unit="m^3", QuantityFormula ="(闸首长+2)*（闸首宽+2）*0.15", Quantity = Result.Item1, EnlargeFactor = 1.1, Memo ="C15，厚150mm" },
    //            new QuantityRowDef { ItemOrder=2, ItemName= "现浇钢筋混凝土底板", Unit="m^3", QuantityFormula ="闸首长x闸首宽x3+门槛-门槛部分廊道-出水格栅", Quantity = Result.Item2, EnlargeFactor = 1.1, Memo ="C25（含廊道、门龛），底板最厚处7.6m" },
    //            new QuantityRowDef { ItemOrder=3, ItemName= "底板钢筋", Unit="t", QuantityFormula ="底板量*0.05", Quantity = Result.Item3, EnlargeFactor = 1.1, Memo ="HRB400" },
    //            new QuantityRowDef { ItemOrder=4, ItemName= "底板二期混凝土", Unit="m^3", QuantityFormula ="450", Quantity = 450, EnlargeFactor = 1.1, Memo ="C30微膨胀混凝土，施工宽缝" },
    //            new QuantityRowDef { ItemOrder=5, ItemName= "现浇钢筋砼边墩（上部）", Unit="m^3", QuantityFormula ="边墩面积x（边墩高度-底板厚度）-下部边墩", Quantity =Result.Item5, EnlargeFactor = 1.1, Memo ="C25" },
    //            new QuantityRowDef { ItemOrder=6, ItemName= "边墩钢筋（上部）", Unit="t", QuantityFormula ="", EnlargeFactor = 1.1, Memo ="HRB400" },
    //            new QuantityRowDef { ItemOrder=7, ItemName= "现浇钢筋砼边墩（下部）", Unit="m^3", QuantityFormula ="边墩面积x廊道底至门槛顶的高度-廊道体积（不含门槛段）",Quantity=Result.Item7, EnlargeFactor = 1.1, Memo ="HRB400" },
    //            new QuantityRowDef { ItemOrder=8, ItemName= "边墩钢筋（下部）", Unit="t", QuantityFormula ="", EnlargeFactor = 1.1, Memo ="HRB400" },
    //            new QuantityRowDef { ItemOrder=9, ItemName= "边墩二期混凝土", Unit="m^3", QuantityFormula ="176.12", Quantity=176.12, EnlargeFactor = 1.1, Memo ="C30微膨胀混凝土" },
    //            new QuantityRowDef { ItemOrder=10, ItemName= "钢护板、护角", Unit="t", QuantityFormula ="65", Quantity= 65, EnlargeFactor = 1.1, Memo ="Q235喷锌防腐，不含检修门槽处护角" },
    //            new QuantityRowDef { ItemOrder=11, ItemName= "钢护板锚筋", Unit="t", QuantityFormula ="65*0.021", Quantity =1.365,  EnlargeFactor = 1.1, Memo ="HPB300级Φ12" },
    //            new QuantityRowDef { ItemOrder=12, ItemName= "甲种爬梯", Unit="t", QuantityFormula ="（边墩高度-3）x0.313x2", Quantity=Result.Item12, EnlargeFactor = 1.1, Memo ="18.3m/座，每个闸首2座" },
    //            new QuantityRowDef { ItemOrder=13, ItemName= "乙种爬梯", Unit="t", QuantityFormula ="", EnlargeFactor = 1.1, Memo ="12.3m/座，每个闸首2座" },
    //            new QuantityRowDef { ItemOrder=14, ItemName= "栏杆", Unit="t", QuantityFormula ="边墩边长之和x2", Quantity = Result.Item14, EnlargeFactor = 1.1, Memo ="钢管及扁钢加工，每个闸首栏杆长度154m" },
    //            new QuantityRowDef { ItemOrder=15, ItemName= "铸铁水尺", Unit="m", QuantityFormula ="", EnlargeFactor = 1.1, Memo ="" },
    //            new QuantityRowDef { ItemOrder=16, ItemName= "SC镀锌钢管", Unit="m", QuantityFormula ="16", Quantity=16, EnlargeFactor = 1.1, Memo ="Φ219x6" },
    //            new QuantityRowDef { ItemOrder=17, ItemName= "SC镀锌钢管", Unit="m", QuantityFormula ="39.6", Quantity=39.6, EnlargeFactor = 1.1, Memo ="Φ102x5" },
    //            new QuantityRowDef { ItemOrder=18, ItemName= "紫铜止水", Unit="m", QuantityFormula ="（边墩高度+船闸宽度/2）x2", Quantity = Result.Item18, EnlargeFactor = 1.1, Memo ="闸首与导航墙、闸室连接处" },
    //            new QuantityRowDef { ItemOrder=19, ItemName= "镀锌铁皮止水", Unit="m", QuantityFormula ="闸首结构长度*2", Quantity= Result.Item19, EnlargeFactor = 1.1, Memo ="" },
    //            new QuantityRowDef { ItemOrder=20, ItemName= "观测点", Unit="个", QuantityFormula ="16", Quantity =16, EnlargeFactor = 1.0, Memo ="每闸首永久:8个，临时：8个，预埋铜钉" },
    //            new QuantityRowDef { ItemOrder=21, ItemName="帷幕灌浆", Unit="m", QuantityFormula="（闸室总长+5x2+船闸宽度）x2", Quantity = Result.Item21, EnlargeFactor=1.0, Memo="深度8m" }
    //        };
    //        return content;
    //    }

    //    public void OutputQuantity(FileInfo outputfileinfo)
    //    {
    //        using (ExcelPackage package = new ExcelPackage(outputfileinfo))
    //        {
    //            ExcelWorksheet sheet = package.Workbook.Worksheets.Add("工程量清单");
    //            //表头
    //            sheet.Cells["A1:G1"].Merge = true;
    //            sheet.Cells["A1"].Value = "闸首工程量表";
    //            sheet.Cells["A1"].Style.Font.Bold = true;
    //            //基本信息
    //            sheet.Cells["A2"].Value = "工程名称：";
    //            sheet.Cells["B2:D2"].Merge = true;
    //            sheet.Cells["B2"].Value = ProjectName;
    //            sheet.Cells["B2"].Style.Font.Bold = true;
    //            sheet.Cells["E2"].Value = "设计阶段：";
    //            sheet.Cells["F2"].Value = ProjectPhase;
    //            sheet.Cells["A3"].Value = "设计人员：";
    //            sheet.Cells["B3"].Value = DesignPerson;
    //            sheet.Cells["C3"].Value = "校核人员";
    //            sheet.Cells["D3"].Value = CheckPerson;
    //            sheet.Cells["E3"].Value = "提取时间:";
    //            sheet.Cells["F3"].Value = DateTime.Now.ToShortDateString();
    //            //表格内容
    //            sheet.Cells["A5"].Value = m_columeName[0];
    //            sheet.Cells["B5"].Value = m_columeName[1];
    //            sheet.Cells["C5"].Value = m_columeName[2];
    //            sheet.Cells["D5:E5"].Merge = true;
    //            sheet.Cells["D5"].Value = m_columeName[3];
    //            sheet.Cells["F5"].Value = m_columeName[4];
    //            sheet.Cells["G5"].Value = m_columeName[5];
    //            int rowindent = 6,row;
    //            //生成表格内容
    //            var cont = CreateContent();
    //            for (int i = 0; i < cont.Count; i++)
    //            {
    //                row = i + rowindent;
    //                sheet.Cells[row, 1].Value = cont[i].ItemOrder;
    //                sheet.Cells[row, 2].Value = cont[i].ItemName;
    //                sheet.Cells[row, 3].Value = cont[i].Unit;
    //                sheet.Cells[row, 4].Value = cont[i].QuantityFormula;
    //                sheet.Cells[row, 5].Value = cont[i].Quantity;
    //                sheet.Cells[row, 6].Value = cont[i].EnlargeFactor;
    //                sheet.Cells[row, 7].Value = cont[i].Memo;
    //            }
    //            sheet.TabColor = System.Drawing.Color.Red;
    //            sheet.Cells["A4:G4"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
    //            sheet.Cells["A4:G4"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);

    //            var redfontrow = new int[] { 4, 9, 10, 11, 16, 17 };
    //            foreach (var redrow in redfontrow)
    //            {
    //                int worldredfontrow = redrow + 5;
    //                sheet.Row(worldredfontrow).Style.Font.Color.SetColor(System.Drawing.Color.Red);
    //            }

    //            using (var totalCells = sheet.Cells[1, 1, 5 + cont.Count, 7])
    //            {
    //                totalCells.Style.Font.Size = 10;
    //                totalCells.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
    //                totalCells.AutoFitColumns(0);
    //                totalCells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
    //            }

    //            //set some document properties
    //            package.Workbook.Properties.Author = "设计一所 苏东升";
    //            package.Workbook.Properties.SetCustomPropertyValue("作者邮箱", "sudongsheng@pdiwt.com.cn");
    //            package.Workbook.Properties.Comments = "由参数化船闸模块自动生成";
    //            package.Workbook.Properties.Company = "中交水运规划设计院有限公司";
    //            package.Save();
    //        }
    //    }

    //}
    public sealed class QuantityRowDef
    {
        public int ItemOrder { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public string QuantityFormula { get; set; }
        public double Quantity { get; set; }
        public double EnlargeFactor { get; set; }
        public string Memo { get; set; }
    }
    public sealed class QuantityResult
    {
        public double Item1 { get; set; }
        public double Item2 { get; set; }
        public double Item3 { get; set; }
        public double Item5 { get; set; }
        public double Item7 { get; set; }
        public double Item12 { get; set; }
        public double Item14 { get; set; }
        public double Item18 { get; set; }
        public double Item19 { get; set; }
        public double Item21 { get; set; }
    }
}
