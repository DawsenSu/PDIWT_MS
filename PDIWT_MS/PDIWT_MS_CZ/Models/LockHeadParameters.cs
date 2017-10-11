using System;
using System.Collections.Generic;
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
        public SidePier LH_SidePier { get; set; }
        public List<EmptyRectBox> LH_EmptyRectBoxs { get; set; } = new List<EmptyRectBox>();
        public List<EmptyZPlanBox> LH_EmptyZPlanBoxs { get; set; } = new List<EmptyZPlanBox>();
        public DoorSill LH_DoorSill { get; set; }
        public LocalConcertationCulvert LH_LocalConcertationCulvert { get; set; }
        public ShortCulvert LH_ShortCulvert { get; set; }
        public EnergyDisspater LH_EnergyDisspater { get; set; }
        public Baffle LH_Baffle { get; set; }

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
        public ShapeIGrooving IGrooving { get; set; }
        public ShapeTGrooving TGrooving { get; set; }
    }
    /// <summary>
    /// I型切槽
    /// </summary>
    public class ShapeIGrooving
    {
        public double GroovingHeight { get; set; }
        public double GroovingTopLength { get; set; }
        public double GroovingBootomLength { get; set; }
        public double GroovingGradient { get; set; }
    }
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
        public double PierXY_A { get; set; }
        public double PierXY_B { get; set; }
        public double PierXY_C { get; set; }
        public double PierXY_D { get; set; }
        public double PierXY_E { get; set; }
        public double PierXY_F { get; set; }
        public double PierHeight { get; set; }
        public double PierChamfer_Tx { get; set; }
        public double PierChamfer_Ty { get; set; }
        public double PierChamfer_R { get; set; }
    }
    #endregion

    #region 闸首空箱参数集（1.长方形空箱 2.Z平面任意形状空箱）
    /// <summary>
    /// 长方形空箱
    /// </summary>
    public class EmptyRectBox
    {
        // 位置参数
        public double XDis { get; set; }
        public double YDis { get; set; }
        public double ZDis { get; set; }

        //形状参数
        public double EmptyBoxLength { get; set; }
        public double EmptyBoxWidth { get; set; }
        public double EmptyBoxHeight { get; set; }

        public List<EmptyBoxEdgeChameferInfo> ChameferInfos { get; set; } = new List<EmptyBoxEdgeChameferInfo>();
    }

    /// <summary>
    /// Z平面任意形状空箱
    /// </summary>
    public class EmptyZPlanBox
    {
        // 位置参数
        public double XDis { get; set; }
        public double YDis { get; set; }
        public double ZDis { get; set; }
        //形状参数
        public double EmptyBoxHeight { get; set; }

        public List<DPoint2d> Point2Ds { get; set; } = new List<DPoint2d>();
        public List<EmptyBoxEdgeChameferInfo> ChameferInfos { get; set; } = new List<EmptyBoxEdgeChameferInfo>();
    }

    /// <summary>
    /// 空箱倒角信息（倒角边标识，是否倒角，倒角长度，倒角宽度）
    /// </summary>
    public class EmptyBoxEdgeChameferInfo
    {
        public EdgeIndicator EdgeFlag { get; set; }
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
        public double DoorSill_A { get; set; }
        public double DoorSill_B { get; set; }
        public double DoorSill_C { get; set; }
        public double DoorSill_D { get; set; }
        public double DoorSill_E { get; set; }
        public double DoorSill_F { get; set; }
        public double DoorSillHeight { get; set; }
    }
    #endregion

    #region 输水廊道参数：1.包括短廊道输水 2.局部集中输水
    /// <summary>
    /// 局部集中输水
    /// </summary>
    public class LocalConcertationCulvert
    {
        //形状参数
        public double Culvert_Height { get; set; }
        public double Culvert_A { get; set; }
        public double Culvert_B { get; set; }
        public double Culvert_C { get; set; }
        public double Culvert_D { get; set; }
        public double Culvert_E { get; set; }
        public double Culvert_F { get; set; }
        public double Culvert_Chamfer_R1 { get; set; }
        public double Culvert_Chamfer_R2 { get; set; }
        public double Culvert_Chamfer_R3 { get; set; }
        public double Culvert_Chamfer_R4 { get; set; }
        //位置参数
        public double Culvert_Pier_BackDis { get; set; }
        //分流墩参数
        public double WaterDivision_R1 { get; set; }
        public double WaterDivision_R2 { get; set; }
        public double WaterDivision_R3 { get; set; }
        public double WaterDivision_A { get; set; }
        public double WaterDivision_B { get; set; }
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
        public double Culvert_E { get; set; }
        public double Culvert_R1 { get; set; }
        public double Culvert_R2 { get; set; }
        //位置参数
        public double Culvert_Pier_BackDis { get; set; }
    }
    #endregion

    #region 消能工参数
    /// <summary>
    /// 消能工参数：包括出水格栅参数及消力坎参数
    /// </summary>
    public class EnergyDisspater
    {
        //出水格栅
        public double Grille_TwolineInterval { get; set; }
        public List<double> GrillWidthList { get; set; } = new List<double>();
        //消力坎参数
    }

    public class Baffle
    {
        public double Baffle_MidMidDis { get; set; }
        public double Baffle_Width { get; set; }
        public double Baffle_Height { get; set; }
    }

    #endregion
}
