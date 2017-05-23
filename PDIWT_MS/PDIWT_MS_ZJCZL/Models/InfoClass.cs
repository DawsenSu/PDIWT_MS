using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;

using HCHXCodeQueryLib;

namespace PDIWT_MS_ZJCZL.Models
{
    [Obsolete]
    public class PileInfoClass
    {
        //桩体信息
        [DisplayName("桩类型"), Description("桩的类型"),Category("桩信息")]
        //public PileType PileTypeInfo { get; set; }
        //[DisplayName("桩直径"), Description("桩的直径"), Category("桩信息")]
        public double PileDiameter { get; set; }
        [DisplayName("桩长"),Description("桩的长度"),Category("桩信息"),ReadOnly(true)]
        public double PileLength { get; set; }
        [DisplayName("桩编号"), Description("桩的编号信息"), Category("桩信息"), ReadOnly(true)]
        public string PileCode { get; set; }
        [DisplayName("桩ID"), Description("DGN文件中桩的Id号"), Category("桩信息"), ReadOnly(true)]
        public double PileId { get; set; }

        //土层信息
        [DisplayName("土层信息"), Category("桩穿过的土层信息"), ReadOnly(true)]
        public ObservableCollection<SoilInfoClass> SoilInfo { get; set; }

        //计算参数信息
        [DisplayName("单桩承载力计算所用参数"),Category("计算参数")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public CalculateParameter CalParameter { get; set; }

        //计算结果信息
        public CalculateResult Result { get; set; }
    }
    [Obsolete]
    public class SoilInfoClass
    {
        [DisplayName("土层编号"), ReadOnly(true)]
        public string SoilLayerNum { get; set; }
        [DisplayName("土层名称"), ReadOnly(true)]
        public string SoilLayerName { get; set; }
        [DisplayName("桩在此土层内的长度m"),ReadOnly(true)]
        public double Length { get; set; }
        [DisplayName("极限侧摩阻力标准值qfi(kN)")]
        public double Qfi { get; set; }
        [DisplayName("桩端摩阻力标准值qR(kN)")]
        public double Qr { get; set; }

        public SoilInfoClass() { }
        public SoilInfoClass(string soillayernum, string soillayername,double length, double qfi, double qr)
        {
            SoilLayerNum = soillayernum;
            SoilLayerName = soillayername;
            Length = length;
            Qfi = qfi;
            Qr = qr;
        }

        public override string ToString()
        {
            return SoilLayerName;
        }
    }

    [Obsolete]
    public class CalculateParameter
    {
        [DisplayName("垂直承载力分析系数")]
        public double GammaR { get; set; }
    }

    [Obsolete]
    public class CalculateResult
    {
        public double UltimateBearingCapacity { get; set; }
        public double UltimatePullingCapacity { get; set; }
    }

    [Obsolete]
    public class SearchPanel
    {
        public SearchType Type { get; set; }
        public string Name { get; set; }
    }

    [Obsolete]
    public enum SearchType
    {
        ById,
        ByRay
    }

    //public enum PileType
    //{
    //    [Description("实心桩或桩端封闭")]
    //    Solid,                      //桩身实心火桩端封闭
    //    [Description("管桩")]
    //    SteelAndPercastConcrete,    //钢管桩与预制混凝土管桩
    //    [Description("灌注桩")]
    //    Filling,                    //灌注桩
    //    [Description("嵌岩桩")]
    //    Socketed,                   //嵌岩桩
    //    [Description("后注浆灌注桩")]
    //    PostgroutingFilling         //后注浆灌注桩
    //}

    public static class CalculatePileCapacity
    {
        public static CalculateResult Calculate(double diameter, ObservableCollection<SoilInfoClass> soilinfo, CalculateParameter calparmeter)
        {
            double pilearea = Math.PI * Math.Pow(diameter / 2, 2);
            double pileperimeter = Math.PI * diameter;
            double accumlatenum = 0;
            double result;
            if (soilinfo != null && soilinfo.Count > 0)
            {
                foreach (var pileeachlength in soilinfo)
                {
                    accumlatenum += pileeachlength.Length * pileeachlength.Qfi;
                }
                result = (pileperimeter * accumlatenum + soilinfo.Last().Qr * pilearea) / calparmeter.GammaR;
            }
            else
                result = -1;

            return new CalculateResult { UltimateBearingCapacity = result, UltimatePullingCapacity = -1 };
        }
    }
}
