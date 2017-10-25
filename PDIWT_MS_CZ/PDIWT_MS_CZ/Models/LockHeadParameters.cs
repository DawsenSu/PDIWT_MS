using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bentley.GeometryNET;

namespace PDIWT_MS_CZ.Models
{
    //闸首参数
    public class LockHeadParameters
    {

        public BaseBoard LH_BaseBoard { get; set; }

        public DoorSill LH_DoorSill { get; set; }

        public SidePier LH_SidePier { get; set; }

        public byte LH_CulvertChoosenIndex { get; set; } //0-> short; 1->local

        public ShortCulvert LH_ShortCulvert { get; set; }

        public LocalConcertationCulvert LH_LocalConcertationCulvert { get; set; }

        public ObservableCollection<RectEmptyBox> LH_EmptyRectBoxs { get; set; } = new ObservableCollection<RectEmptyBox>();

        public ObservableCollection<ZPlanEmptyBox> LH_EmptyZPlanBoxs { get; set; } = new ObservableCollection<ZPlanEmptyBox>();
        public bool IsParametersValid()
        {
            //todo: 添加参数合理性判断
            return true;
        }
    }


    #region 底板参数
    /// <summary>
    /// 闸首底板参数
    /// </summary>
    public class BaseBoard
    {

        public double BaseBoardLength { get; set; }

        public double BaseBoardWidth { get; set; }

        public double BaseBoardHeight { get; set; }

        public double EntranceWidth { get; set; }

        public bool IsGrooving { get; set; }

        public ShapeTGrooving TGrooving { get; set; }
    }
    ///// <summary>
    ///// I型切槽
    ///// </summary>
    //public class ShapeIGrooving
    //{
    //    [BindableProperty]
    //    public double GroovingHeight { get; set; }
    //    [BindableProperty]
    //    public double GroovingTopLength { get; set; }
    //    [BindableProperty]
    //    public double GroovingBootomLength { get; set; }
    //    [BindableProperty]
    //    public double GroovingGradient { get; set; }
    //}
    /// <summary>
    /// T型切槽
    /// </summary>
    public class ShapeTGrooving
    {

        public double GroovingHeight { get; set; }

        public double GroovingFrontLength { get; set; }

        public double GroovingBackLength { get; set; }

        public double GroovingWidth { get; set; }

        public double GroovingGradient { get; set; }
    }
    #endregion

    #region 边墩参数
    public class SidePier
    {

        public double PierHeight { get; set; }

        public double PierXY_A { get; set; }

        public double PierXY_B { get; set; }

        public double PierXY_C { get; set; }

        public double PierXY_D { get; set; }

        public double PierXY_E { get; set; }

        public double PierXY_F { get; set; }

        public bool IsChamfered { get; set; }

        public double PierChamfer_Tx { get; set; }

        public double PierChamfer_Ty { get; set; }

        public double PierChamfer_R { get; set; }
    }
    #endregion

    #region 闸首空箱参数集（1.长方形空箱 2.Z平面任意形状空箱）
    /// <summary>
    /// 长方形空箱
    /// </summary>
    public class RectEmptyBox
    {
        // 位置参数

        public double XDis { get; set; }

        public double YDis { get; set; }

        public double ZDis { get; set; }
        //形状参数

        public double EmptyBoxLength { get; set; }

        public double EmptyBoxWidth { get; set; }

        public double EmptyBoxHeight { get; set; }


        public ObservableCollection<EmptyBoxEdgeChameferInfo> ChamferInfos { get; set; } = new ObservableCollection<EmptyBoxEdgeChameferInfo>();
    }

    /// <summary>
    /// Z平面任意形状空箱
    /// </summary>
    public class ZPlanEmptyBox
    {
        // 位置参数

        public double XDis { get; set; }

        public double YDis { get; set; }

        public double ZDis { get; set; }
        //形状参数

        public double EmptyBoxHeight { get; set; }


        public ObservableCollection<ZPlanInfo> ZPlanInfos { get; set; } = new ObservableCollection<ZPlanInfo>();
    }

    public class ZPlanInfo
    {

        public double X { get; set; }

        public double Y { get; set; }

        public EmptyBoxEdgeChameferInfo BoxEdgeChamferInfo { get; set; }
    }
    /// <summary>
    /// 空箱倒角信息（倒角边标识，是否倒角，倒角长度，倒角宽度）
    /// </summary>
    public class EmptyBoxEdgeChameferInfo
    {

        public int EdgeIndicator { get; set; }

        public bool IsChamfered { get; set; }

        public double ChamferLength { get; set; }

        public double ChamferWidth { get; set; }
    }
    [Flags]
    public enum EdgeIndicator
    {
        x0,
        x1,
        x2,
        x3,
        y4,
        y5,
        y6,
        y7,
        z8,
        z9,
        z10,
        z11,
        xplan = x0 | x1 | x2 | x3,
        yplan = y4 | y5 | y6 | y7,
        zplan = z8 | z9 | z10 | z11,
        front = x0 | x3 | z8 | z9,
        back = x1 | x2 | z10 | z11,
        right = y6 | y7 | z9 | z10,
        left = y4 | y5 | z8 | z11,
        top = x2 | x3 | y5 | y6,
        bottom = x0 | x1 | y4 | y7,
        all = xplan | yplan | zplan,
        none
    }
    #endregion

    #region 门槛参数
    /// <summary>
    /// 门槛平面参数a-f，及门槛高度
    /// </summary>
    public class DoorSill
    {

        public double DoorSillHeight { get; set; }

        public double DoorSill_A { get; set; }

        public double DoorSill_B { get; set; }

        public double DoorSill_C { get; set; }

        public double DoorSill_D { get; set; }

        public double DoorSill_E { get; set; }

        public double DoorSill_F { get; set; }
    }
    #endregion

    #region 输水廊道参数：1.包括短廊道输水 2.局部集中输水
    /// <summary>
    /// 局部集中输水
    /// </summary>
    public class LocalConcertationCulvert
    {
        //位置参数

        public double Culvert_Pier_BackDis { get; set; }

        //形状参数

        public double Culvert_Height { get; set; }

        public double Culvert_A { get; set; }

        public double Culvert_B { get; set; }

        public double Culvert_C { get; set; }

        public double Culvert_D { get; set; }

        public double Culvert_E { get; set; }

        public double Culvert_F { get; set; }

        public bool IsChamfered { get; set; }

        public double Culvert_Chamfer_R1 { get; set; }

        public double Culvert_Chamfer_R2 { get; set; }

        public double Culvert_Chamfer_R3 { get; set; }

        public double Culvert_Chamfer_R4 { get; set; }
        //消能工参数

        public bool IsIncludeWaterDivision { get; set; }

        public WaterDivision Culvert_WaterDivision { get; set; }


        public bool IsIncludeEnergyDisspater { get; set; }

        public EnergyDisspater Culvert_EnergyDisspater { get; set; }


        public bool IsIncludeBaffle { get; set; }

        public ObservableCollection<Baffle> Culvert_Baffle { get; set; }
    }
    /// <summary>
    /// 短廊道参数
    /// </summary>
    public class ShortCulvert
    {

        public double Culvert_Width { get; set; }

        public double Culvert_A { get; set; }

        public double Culvert_B { get; set; }

        public double Culvert_C { get; set; }

        public double Culvert_D { get; set; }

        public double Culvert_R1 { get; set; }

        public double Culvert_R2 { get; set; }
        //位置参数

        public double Culvert_Pier_BackDis { get; set; }
    }
    #endregion

    #region 消能工参数

    /// <summary>
    /// 消能工参数：包括分流墩、出水格栅参数及消力坎参数
    /// </summary>
    public class WaterDivision
    {

        public double WaterDivision_R1 { get; set; }

        public double WaterDivision_R2 { get; set; }

        public double WaterDivision_R3 { get; set; }

        public double WaterDivision_A { get; set; }

        public double WaterDivision_B { get; set; }
    }

    public class EnergyDisspater
    {
        //出水格栅

        public double Grille_TwolineInterval { get; set; }

        public ObservableCollection<GrillInterval> GrilleWidthList { get; set; } = new ObservableCollection<GrillInterval>();
        //消力坎参数
    }

    public class GrillInterval
    {

        public double Interval { get; set; }

        public double RoundChamferRadius { get; set; }
    }

    public class Baffle
    {

        public double Baffle_MidMidDis { get; set; }

        public double Baffle_Width { get; set; }

        public double Baffle_Height { get; set; }
    }
    #endregion
    public static class SolveEqution
    {
        public static void GetAlphaAndL(double a, double b, double c, double d, double R1, double R2, out double alpha, out double l)
        {
            alpha = MathNet.Numerics.FindRoots.OfFunction(x => (d - (1 - Math.Cos(x)) * (R1 + R2)) / (b - Math.Sin(x) * (R1 + R2)) - Math.Tan(x), 0, Math.PI / 2);
            l = (d - (1 - Math.Cos(alpha)) * (R1 + R2)) / Math.Sin(alpha);
        }
    }
}
