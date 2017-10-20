using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bentley.GeometryNET;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;

namespace PDIWT_MS_CZ.Models
{
    //闸首参数
    public class LockHeadParameters
    {
        [BindableProperty]
        public BaseBoard LH_BaseBoard { get; set; }
        [BindableProperty]
        public DoorSill LH_DoorSill { get; set; }
        [BindableProperty]
        public SidePier LH_SidePier { get; set; }
        [BindableProperty]
        public byte LH_CulvertChoosenIndex { get; set; } //0-> short; 1->local
        [BindableProperty]
        public ShortCulvert LH_ShortCulvert { get; set; }
        [BindableProperty]
        public LocalConcertationCulvert LH_LocalConcertationCulvert { get; set; }
        [BindableProperty]
        public ObservableCollection<RectEmptyBox> LH_EmptyRectBoxs { get; set; } = new ObservableCollection<RectEmptyBox>();
        [BindableProperty]
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
        [BindableProperty]
        public double BaseBoardLength { get; set; }
        [BindableProperty]
        public double BaseBoardWidth { get; set; }
        [BindableProperty]
        public double BaseBoardHeight { get; set; }
        [BindableProperty]
        public double EntranceWidth { get; set; }
        [BindableProperty]
        public bool IsGrooving { get; set; }
        [BindableProperty]
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
        [BindableProperty]
        public double GroovingHeight { get; set; }
        [BindableProperty]
        public double GroovingFrontLength { get; set; }
        [BindableProperty]
        public double GroovingBackLength { get; set; }
        [BindableProperty]
        public double GroovingWidth { get; set; }
        [BindableProperty]
        public double GroovingGradient { get; set; }
    }
    #endregion

    #region 边墩参数
    public class SidePier
    {
        [BindableProperty]
        public double PierHeight { get; set; }
        [BindableProperty]
        public double PierXY_A { get; set; }
        [BindableProperty]
        public double PierXY_B { get; set; }
        [BindableProperty]
        public double PierXY_C { get; set; }
        [BindableProperty]
        public double PierXY_D { get; set; }
        [BindableProperty]
        public double PierXY_E { get; set; }
        [BindableProperty]
        public double PierXY_F { get; set; }
        [BindableProperty]
        public bool IsChamfered { get; set; }
        [BindableProperty]
        public double PierChamfer_Tx { get; set; }
        [BindableProperty]
        public double PierChamfer_Ty { get; set; }
        [BindableProperty]
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
        [BindableProperty]
        public double XDis { get; set; }
        [BindableProperty]
        public double YDis { get; set; }
        [BindableProperty]
        public double ZDis { get; set; }
        //形状参数
        [BindableProperty]
        public double EmptyBoxLength { get; set; }
        [BindableProperty]
        public double EmptyBoxWidth { get; set; }
        [BindableProperty]
        public double EmptyBoxHeight { get; set; }

        [BindableProperty]
        public ObservableCollection<EmptyBoxEdgeChameferInfo> ChamferInfos { get; set; } = new ObservableCollection<EmptyBoxEdgeChameferInfo>();
    }

    /// <summary>
    /// Z平面任意形状空箱
    /// </summary>
    public class ZPlanEmptyBox
    {
        // 位置参数
        [BindableProperty]
        public double XDis { get; set; }
        [BindableProperty]
        public double YDis { get; set; }
        [BindableProperty]
        public double ZDis { get; set; }
        //形状参数
        [BindableProperty]
        public double EmptyBoxHeight { get; set; }

        [BindableProperty]
        public ObservableCollection<ZPlanInfo> ZPlanInfos { get; set; } = new ObservableCollection<ZPlanInfo>();
    }

    public class ZPlanInfo
    {
        [BindableProperty]
        public double X { get; set; }
        [BindableProperty]
        public double Y { get; set; }
        [BindableProperty]
        public EmptyBoxEdgeChameferInfo BoxEdgeChamferInfo { get; set; }
    }
    /// <summary>
    /// 空箱倒角信息（倒角边标识，是否倒角，倒角长度，倒角宽度）
    /// </summary>
    public class EmptyBoxEdgeChameferInfo
    {
        [BindableProperty]
        public int EdgeIndicator { get; set; }
        [BindableProperty]
        public bool IsChamfered { get; set; }
        [BindableProperty]
        public double ChamferLength { get; set; }
        [BindableProperty]
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
        [BindableProperty]
        public double DoorSillHeight { get; set; }
        [BindableProperty]
        public double DoorSill_A { get; set; }
        [BindableProperty]
        public double DoorSill_B { get; set; }
        [BindableProperty]
        public double DoorSill_C { get; set; }
        [BindableProperty]
        public double DoorSill_D { get; set; }
        [BindableProperty]
        public double DoorSill_E { get; set; }
        [BindableProperty]
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
        [BindableProperty]
        public double Culvert_Pier_BackDis { get; set; }

        //形状参数
        [BindableProperty]
        public double Culvert_Height { get; set; }
        [BindableProperty]
        public double Culvert_A { get; set; }
        [BindableProperty]
        public double Culvert_B { get; set; }
        [BindableProperty]
        public double Culvert_C { get; set; }
        [BindableProperty]
        public double Culvert_D { get; set; }
        [BindableProperty]
        public double Culvert_E { get; set; }
        [BindableProperty]
        public double Culvert_F { get; set; }
        [BindableProperty]
        public bool IsChamfered { get; set; }
        [BindableProperty]
        public double Culvert_Chamfer_R1 { get; set; }
        [BindableProperty]
        public double Culvert_Chamfer_R2 { get; set; }
        [BindableProperty]
        public double Culvert_Chamfer_R3 { get; set; }
        [BindableProperty]
        public double Culvert_Chamfer_R4 { get; set; }
        //消能工参数
        [BindableProperty]
        public bool IsIncludeWaterDivision { get; set; }
        [BindableProperty]
        public WaterDivision Culvert_WaterDivision { get; set; }

        [BindableProperty]
        public bool IsIncludeEnergyDisspater { get; set; }
        [BindableProperty]
        public EnergyDisspater Culvert_EnergyDisspater { get; set; }

        [BindableProperty]
        public bool IsIncludeBaffle { get; set; }
        [BindableProperty]
        public ObservableCollection<Baffle> Culvert_Baffle { get; set; }
    }
    /// <summary>
    /// 短廊道参数
    /// </summary>
    public class ShortCulvert
    {
        [BindableProperty]
        public double Culvert_Width { get; set; }
        [BindableProperty]
        public double Culvert_A { get; set; }
        [BindableProperty]
        public double Culvert_B { get; set; }
        [BindableProperty]
        public double Culvert_C { get; set; }
        [BindableProperty]
        public double Culvert_D { get; set; }
        [BindableProperty]
        public double Culvert_R1 { get; set; }
        [BindableProperty]
        public double Culvert_R2 { get; set; }
        //位置参数
        [BindableProperty]
        public double Culvert_Pier_BackDis { get; set; }
    }
    #endregion

    #region 消能工参数

    /// <summary>
    /// 消能工参数：包括分流墩、出水格栅参数及消力坎参数
    /// </summary>
    public class WaterDivision
    {
        [BindableProperty]
        public double WaterDivision_R1 { get; set; }
        [BindableProperty]
        public double WaterDivision_R2 { get; set; }
        [BindableProperty]
        public double WaterDivision_R3 { get; set; }
        [BindableProperty]
        public double WaterDivision_A { get; set; }
        [BindableProperty]
        public double WaterDivision_B { get; set; }
    }

    public class EnergyDisspater
    {
        //出水格栅
        [BindableProperty]
        public double Grille_TwolineInterval { get; set; }
        [BindableProperty]
        public ObservableCollection<GrillInterval> GrilleWidthList { get; set; } = new ObservableCollection<GrillInterval>();
        //消力坎参数
    }

    public class GrillInterval
    {
        [BindableProperty]
        public double Interval { get; set; }
        [BindableProperty]
        public double RoundChamferRadius { get; set; }
    }

    public class Baffle
    {
        [BindableProperty]
        public double Baffle_MidMidDis { get; set; }
        [BindableProperty]
        public double Baffle_Width { get; set; }
        [BindableProperty]
        public double Baffle_Height { get; set; }
    }
    #endregion
}
