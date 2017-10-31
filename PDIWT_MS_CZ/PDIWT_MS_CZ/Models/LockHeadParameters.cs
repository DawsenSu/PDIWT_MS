using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel;

namespace PDIWT_MS_CZ.Models
{
    //闸首参数
    public class LockHeadParameters : ObservableObject
    {
        private BaseBoard _LH_BaseBoard;
        public BaseBoard LH_BaseBoard
        {
            get { return _LH_BaseBoard; }
            set { Set(ref _LH_BaseBoard, value); }
        }

        private DoorSill _LH_DoorSill;
        public DoorSill LH_DoorSill
        {
            get { return _LH_DoorSill; }
            set { Set(ref _LH_DoorSill, value); }
        }

        private SidePier _LH_SidePier;
        public SidePier LH_SidePier
        {
            get { return _LH_SidePier; }
            set { Set(ref _LH_SidePier, value); }
        }

        private byte _LH_CulvertChoosenIndex;//0-> short; 1->local
        public byte LH_CulvertChoosenIndex
        {
            get { return _LH_CulvertChoosenIndex; }
            set { Set(ref _LH_CulvertChoosenIndex, value); }
        }

        private ShortCulvert _LH_ShortCulvert;
        public ShortCulvert LH_ShortCulvert
        {
            get { return _LH_ShortCulvert; }
            set { Set(ref _LH_ShortCulvert, value); }
        }

        private LocalConcertationCulvert _LH_LocalConcertationCulvert;
        public LocalConcertationCulvert LH_LocalConcertationCulvert
        {
            get { return _LH_LocalConcertationCulvert; }
            set { Set(ref _LH_LocalConcertationCulvert, value); }
        }

        private ObservableCollection<RectEmptyBox> _LH_EmptyRectBoxs;
        public ObservableCollection<RectEmptyBox> LH_EmptyRectBoxs
        {
            get { return _LH_EmptyRectBoxs;  }
            set { Set(ref _LH_EmptyRectBoxs, value); }
        }

        private ObservableCollection<ZPlanEmptyBox> _LH_EmptyZPlanBoxs;
        public ObservableCollection<ZPlanEmptyBox> LH_EmptyZPlanBoxs
        {
            get { return _LH_EmptyZPlanBoxs; }
            set { Set(ref _LH_EmptyZPlanBoxs, value);  }
        }

        public string IsParametersValid()
        {
            //todo: 添加参数合理性判断
            for (int i = 0; i < LH_EmptyRectBoxs.Count; i++)
            {
                if (LH_EmptyRectBoxs[i].EmptyBoxWidth + LH_EmptyRectBoxs[i].XDis >= LH_SidePier.PierXY_A)
                    return $"{i+1}号长方体空箱宽度{LH_EmptyRectBoxs[i].EmptyBoxWidth}与X向距离{LH_EmptyRectBoxs[i].XDis}超过边墩参数a";
                foreach (var chamefer in LH_EmptyRectBoxs[i].ChamferInfos)
                {
                    if (chamefer.IsChamfered && (chamefer.ChamferLength <= 0 || chamefer.ChamferWidth <= 0))
                        return $"{i+1}号长方体空箱的{chamefer.EdgeIndicator}边倒角为0值或负值";
                }
            }
            for (int i = 0; i < LH_EmptyZPlanBoxs.Count; i++)
            {
                if (LH_EmptyZPlanBoxs[i].ZPlanInfos.Count <= 2)
                    return $"{i+1}号棱柱形空箱的平面几何点数小于2";
            }
            double _sumofedgriilwidth=0;
            foreach (var interval in LH_LocalConcertationCulvert.Culvert_EnergyDisspater.GrilleWidthList)
            {
                _sumofedgriilwidth += interval.Interval;
            }
            if (_sumofedgriilwidth > LH_DoorSill.DoorSill_A / 2)
                return "出水格栅总宽度超过门槛宽度";

            return Properties.Resources.Verified;
        }

    }


    #region 底板参数
    /// <summary>
    /// 闸首底板参数
    /// </summary>
    public class BaseBoard : ObservableObject
    {
        private double _BaseBoardLength;
        public double BaseBoardLength
        {
            get { return _BaseBoardLength; }
            set { Set(ref _BaseBoardLength, value); }
        }

        private double _BaseBoardWidth;
        public double BaseBoardWidth
        {
            get { return _BaseBoardWidth; }
            set { Set(ref _BaseBoardWidth, value); }
        }

        private double _BaseBoardHeight;
        public double BaseBoardHeight
        {
            get { return _BaseBoardHeight; }
            set { Set(ref _BaseBoardHeight, value); }
        }

        private double _EntranceWidth;
        public double EntranceWidth
        {
            get { return _EntranceWidth; }
            set { Set(ref _EntranceWidth, value); }
        }

        private bool _IsGrooving;
        public bool IsGrooving
        {
            get { return _IsGrooving; }
            set { Set(ref _IsGrooving, value); }
        }

        private ShapeTGrooving _TGrooving;
        public ShapeTGrooving TGrooving
        {
            get { return _TGrooving; }
            set { Set(ref _TGrooving, value); }
        }
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
    public class ShapeTGrooving : ObservableObject
    {
        private double _GroovingHeight;
        public double GroovingHeight
        {
            get { return _GroovingHeight; }
            set { Set(ref _GroovingHeight, value); }
        }

        private double _GroovingFrontLength;
        public double GroovingFrontLength
        {
            get { return _GroovingFrontLength; }
            set { Set(ref _GroovingFrontLength, value); }
        }

        private double _GroovingBackLength;
        public double GroovingBackLength
        {
            get { return _GroovingBackLength; }
            set { Set(ref _GroovingBackLength, value); }
        }

        private double _GroovingWidth;
        public double GroovingWidth
        {
            get { return _GroovingWidth; }
            set { Set(ref _GroovingWidth, value); }
        }

        private double _GroovingGradient;
        public double GroovingGradient
        {
            get { return _GroovingGradient; }
            set { Set(ref _GroovingGradient, value); }
        }
    }
    #endregion

    #region 边墩参数
    public class SidePier : ObservableObject
    {
        private double _PierHeight;
        public double PierHeight
        {
            get { return _PierHeight; }
            set { Set(ref _PierHeight, value); }
        }

        private double _PierXY_A;
        public double PierXY_A
        {
            get { return _PierXY_A; }
            set { Set(ref _PierXY_A, value); }
        }

        private double _PierXY_B;
        public double PierXY_B
        {
            get { return _PierXY_B; }
            set { Set(ref _PierXY_B, value); }
        }

        private double _PierXY_C;
        public double PierXY_C
        {
            get { return _PierXY_C; }
            set { Set(ref _PierXY_C, value); }
        }

        private double _PierXY_D;
        public double PierXY_D
        {
            get { return _PierXY_D; }
            set { Set(ref _PierXY_D, value); }
        }

        private double _PierXY_E;
        public double PierXY_E
        {
            get { return _PierXY_E; }
            set { Set(ref _PierXY_E, value); }
        }

        private double _PierXY_F;
        public double PierXY_F
        {
            get { return _PierXY_F; }
            set { Set(ref _PierXY_F, value); }
        }

        private bool _IsChamfered;
        public bool IsChamfered
        {
            get { return _IsChamfered; }
            set { Set(ref _IsChamfered, value); }
        }

        private double _PierChamfer_Tx;
        public double PierChamfer_Tx
        {
            get { return _PierChamfer_Tx; }
            set { Set(ref _PierChamfer_Tx, value); }
        }

        private double _PierChamfer_Ty;
        public double PierChamfer_Ty
        {
            get { return _PierChamfer_Ty; }
            set { Set(ref _PierChamfer_Ty, value); }
        }

        private double _PierChamfer_R;
        public double PierChamfer_R
        {
            get { return _PierChamfer_R; }
            set { Set(ref _PierChamfer_R, value); }
        }

    }
    #endregion

    #region 闸首空箱参数集（1.长方形空箱 2.Z平面任意形状空箱）
    /// <summary>
    /// 长方形空箱
    /// </summary>
    public class RectEmptyBox : ObservableObject
    {
        // 位置参数
        private double _XDis;
        public double XDis
        {
            get { return _XDis; }
            set { Set(ref _XDis, value); }
        }

        private double _YDis;
        public double YDis
        {
            get { return _YDis; }
            set { Set(ref _YDis, value); }
        }

        private double _ZDis;
        public double ZDis
        {
            get { return _ZDis; }
            set { Set(ref _ZDis, value); }
        }
        //形状参数

        private double _EmptyBoxLength;
        public double EmptyBoxLength
        {
            get { return _EmptyBoxLength; }
            set { Set(ref _EmptyBoxLength, value); }
        }

        private double _EmptyBoxWidth;
        public double EmptyBoxWidth
        {
            get { return _EmptyBoxWidth; }
            set { Set(ref _EmptyBoxWidth, value); }
        }

        private double _EmptyBoxHeight;
        public double EmptyBoxHeight
        {
            get { return _EmptyBoxHeight; }
            set { Set(ref _EmptyBoxHeight, value); }
        }

        private ObservableCollection<EmptyBoxEdgeChameferInfo> _ChamferInfos;
        public ObservableCollection<EmptyBoxEdgeChameferInfo> ChamferInfos
        {
            get { return _ChamferInfos; }
            set { Set(ref _ChamferInfos, value); }
        }
    }

    /// <summary>
    /// Z平面任意形状空箱
    /// </summary>
    public class ZPlanEmptyBox : ObservableObject
    {
        // 位置参数
        private double _XDis;
        public double XDis
        {
            get { return _XDis; }
            set { Set(ref _XDis, value); }
        }

        private double _YDis;
        public double YDis
        {
            get { return _YDis; }
            set { Set(ref _YDis, value); }
        }
        private double _ZDis;
        public double ZDis
        {
            get { return _ZDis; }
            set { Set(ref _ZDis, value); }
        }
        //形状参数

        private double _EmptyBoxHeight;
        public double EmptyBoxHeight
        {
            get { return _EmptyBoxHeight; }
            set { Set(ref _EmptyBoxHeight, value); }
        }

        private ObservableCollection<ZPlanInfo> _ZPlanInfos;
        public ObservableCollection<ZPlanInfo> ZPlanInfos
        {
            get { return _ZPlanInfos; }
            set { Set(ref _ZPlanInfos, value); }
        }
    }

    public class ZPlanInfo : ObservableObject
    {
        private double _X;
        public double X
        {
            get { return _X; }
            set { Set(ref _X, value); }
        }
        private double _Y;
        public double Y
        {
            get { return _Y; }
            set { Set(ref _Y, value); }
        }

        private EmptyBoxEdgeChameferInfo _BoxEdgeChamferInfo;
        public EmptyBoxEdgeChameferInfo BoxEdgeChamferInfo
        {
            get { return _BoxEdgeChamferInfo; }
            set { Set(ref _BoxEdgeChamferInfo, value); }
        }
    }
    /// <summary>
    /// 空箱倒角信息（倒角边标识，是否倒角，倒角长度，倒角宽度）
    /// </summary>
    public class EmptyBoxEdgeChameferInfo : ObservableObject
    {
        private int _EdgeIndicator;
        public int EdgeIndicator
        {
            get { return _EdgeIndicator; }
            set { Set(ref _EdgeIndicator, value); }
        }
        private bool _IsChamfered;
        public bool IsChamfered
        {
            get { return _IsChamfered; }
            set { Set(ref _IsChamfered, value); }
        }

        private double _ChamferLength;
        public double ChamferLength
        {
            get { return _ChamferLength; }
            set { Set(ref _ChamferLength, value); }
        }

        private double _ChamferWidth;
        public double ChamferWidth
        {
            get { return _ChamferWidth; }
            set { Set(ref _ChamferWidth, value); }
        }
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
    public class DoorSill : ObservableObject
    {
        private double _DoorSillHeight;
        public double DoorSillHeight
        {
            get { return _DoorSillHeight; }
            set { Set(ref _DoorSillHeight, value); }
        }
        private double _DoorSill_A;
        public double DoorSill_A
        {
            get { return _DoorSill_A; }
            set { Set(ref _DoorSill_A, value); }
        }

        private double _DoorSill_B;
        public double DoorSill_B
        {
            get { return _DoorSill_B; }
            set { Set(ref _DoorSill_B, value); }
        }

        private double _DoorSill_C;
        public double DoorSill_C
        {
            get { return _DoorSill_C; }
            set { Set(ref _DoorSill_C, value); }
        }

        private double _DoorSill_D;
        public double DoorSill_D
        {
            get { return _DoorSill_D; }
            set { Set(ref _DoorSill_D, value); }
        }

        private double _DoorSill_E;
        public double DoorSill_E
        {
            get { return _DoorSill_E; }
            set { Set(ref _DoorSill_E, value); }
        }

        private double _DoorSill_F;
        public double DoorSill_F
        {
            get { return _DoorSill_F; }
            set { Set(ref _DoorSill_F, value); }
        }
    }
    #endregion

    #region 输水廊道参数：1.包括短廊道输水 2.局部集中输水
    /// <summary>
    /// 局部集中输水
    /// </summary>
    public class LocalConcertationCulvert : ObservableObject
    {
        //位置参数

        private double _Culvert_Pier_BackDis;
        public double Culvert_Pier_BackDis
        {
            get { return _Culvert_Pier_BackDis; }
            set { Set(ref _Culvert_Pier_BackDis, value); }
        }
        //形状参数
        private double _Culvert_Height;
        public double Culvert_Height
        {
            get { return _Culvert_Height; }
            set { Set(ref _Culvert_Height, value); }
        }

        private double _Culvert_A;
        public double Culvert_A
        {
            get { return _Culvert_A; }
            set { Set(ref _Culvert_A, value); }
        }

        private double _Culvert_B;
        public double Culvert_B
        {
            get { return _Culvert_B; }
            set { Set(ref _Culvert_B, value); }
        }

        private double _Culvert_C;
        public double Culvert_C
        {
            get { return _Culvert_C; }
            set { Set(ref _Culvert_C, value); }
        }

        private double _Culvert_D;
        public double Culvert_D
        {
            get { return _Culvert_D; }
            set { Set(ref _Culvert_D, value); }
        }

        private double _Culvert_E;
        public double Culvert_E
        {
            get { return _Culvert_E; }
            set { Set(ref _Culvert_E, value); }
        }

        private double _Culvert_F;
        public double Culvert_F
        {
            get { return _Culvert_F; }
            set { Set(ref _Culvert_F, value); }
        }

        private bool _IsChamfered;
        public bool IsChamfered
        {
            get { return _IsChamfered; }
            set { Set(ref _IsChamfered, value); }
        }

        private double _Culvert_Chamfer_R1;
        public double Culvert_Chamfer_R1
        {
            get { return _Culvert_Chamfer_R1; }
            set { Set(ref _Culvert_Chamfer_R1, value); }
        }

        private double _Culvert_Chamfer_R2;
        public double Culvert_Chamfer_R2
        {
            get { return _Culvert_Chamfer_R2; }
            set { Set(ref _Culvert_Chamfer_R2, value); }
        }

        private double _Culvert_Chamfer_R3;
        public double Culvert_Chamfer_R3
        {
            get { return _Culvert_Chamfer_R3; }
            set { Set(ref _Culvert_Chamfer_R3, value); }
        }

        private double _Culvert_Chamfer_R4;
        public double Culvert_Chamfer_R4
        {
            get { return _Culvert_Chamfer_R4; }
            set { Set(ref _Culvert_Chamfer_R4, value); }
        }
        //消能工参数

        private bool _IsIncludeWaterDivision;
        public bool IsIncludeWaterDivision
        {
            get { return _IsIncludeWaterDivision; }
            set { Set(ref _IsIncludeWaterDivision, value); }
        }

        private WaterDivision _Culvert_WaterDivision;
        public WaterDivision Culvert_WaterDivision
        {
            get { return _Culvert_WaterDivision; }
            set { Set(ref _Culvert_WaterDivision, value); }
        }

        private bool _IsIncludeEnergyDisspater;
        public bool IsIncludeEnergyDisspater
        {
            get { return _IsIncludeEnergyDisspater; }
            set { Set(ref _IsIncludeEnergyDisspater, value); }
        }

        private EnergyDisspater _Culvert_EnergyDisspater;
        public EnergyDisspater Culvert_EnergyDisspater
        {
            get { return _Culvert_EnergyDisspater; }
            set { Set(ref _Culvert_EnergyDisspater, value); }
        }

        private bool _IsIncludeBaffle;
        public bool IsIncludeBaffle
        {
            get { return _IsIncludeBaffle; }
            set { Set(ref _IsIncludeBaffle, value); }
        }

        private ObservableCollection<Baffle> _Culvert_Baffle;
        public ObservableCollection<Baffle> Culvert_Baffle
        {
            get { return _Culvert_Baffle; }
            set { Set(ref _Culvert_Baffle, value); }
        }
    }
    /// <summary>
    /// 短廊道参数
    /// </summary>
    public class ShortCulvert : ObservableObject
    {
        private double _Culvert_Width;
        public double Culvert_Width
        {
            get { return _Culvert_Width; }
            set { Set(ref _Culvert_Width, value); }
        }

        private double _Culvert_A;
        public double Culvert_A
        {
            get { return _Culvert_A; }
            set { Set(ref _Culvert_A, value); }
        }

        private double _Culvert_B;
        public double Culvert_B
        {
            get { return _Culvert_B; }
            set { Set(ref _Culvert_B, value); }
        }

        private double _Culvert_C;
        public double Culvert_C
        {
            get { return _Culvert_C; }
            set { Set(ref _Culvert_C, value); }
        }

        private double _Culvert_D;
        public double Culvert_D
        {
            get { return _Culvert_D; }
            set { Set(ref _Culvert_D, value); }
        }

        private double _Culvert_R1;
        public double Culvert_R1
        {
            get { return _Culvert_R1; }
            set { Set(ref _Culvert_R1, value); }
        }

        private double _Culvert_R2;
        public double Culvert_R2
        {
            get { return _Culvert_R2; }
            set { Set(ref _Culvert_R2, value); }
        }
        //位置参数

        private double _Culvert_Pier_RightDis;
        public double Culvert_Pier_RightDis
        {
            get { return _Culvert_Pier_RightDis; }
            set { Set(ref _Culvert_Pier_RightDis, value); }
        }

        private double _Culvert_Baseboard_BottomDis;
        public double Culvert_Baseboard_BottomDis
        {
            get { return _Culvert_Baseboard_BottomDis; }
            set { Set(ref _Culvert_Baseboard_BottomDis, value); }
        }
    }
    #endregion

    #region 消能工参数

    /// <summary>
    /// 消能工参数：包括分流墩、出水格栅参数及消力坎参数
    /// </summary>
    public class WaterDivision : ObservableObject
    {
        private double _WaterDivision_R1;
        public double WaterDivision_R1
        {
            get { return _WaterDivision_R1; }
            set { Set(ref _WaterDivision_R1, value); }
        }

        private double _WaterDivision_R2;
        public double WaterDivision_R2
        {
            get { return _WaterDivision_R2; }
            set { Set(ref _WaterDivision_R2, value); }
        }

        private double _WaterDivision_R3;
        public double WaterDivision_R3
        {
            get { return _WaterDivision_R3; }
            set { Set(ref _WaterDivision_R3, value); }
        }

        private double _WaterDivision_A;
        public double WaterDivision_A
        {
            get { return _WaterDivision_A; }
            set { Set(ref _WaterDivision_A, value); }
        }

        private double _WaterDivision_B;
        public double WaterDivision_B
        {
            get { return _WaterDivision_B; }
            set { Set(ref _WaterDivision_B, value); }
        }

    }

    public class EnergyDisspater : ObservableObject
    {
        //出水格栅
        private double _Grille_TwolineInterval;
        public double Grille_TwolineInterval
        {
            get { return _Grille_TwolineInterval; }
            set { Set(ref _Grille_TwolineInterval, value); }
        }

        private ObservableCollection<GrillInterval> _GrilleWidthList;
        public ObservableCollection<GrillInterval> GrilleWidthList
        {
            get { return _GrilleWidthList; }
            set { Set(ref _GrilleWidthList, value); }
        }


    }

    public class GrillInterval : ObservableObject
    {
        private double _Interval;
        public double Interval
        {
            get { return _Interval; }
            set { Set(ref _Interval, value); }
        }

        private double _RoundChamferRadius;
        public double RoundChamferRadius
        {
            get { return _RoundChamferRadius; }
            set { Set(ref _RoundChamferRadius, value); }
        }
    }

    //消力坎参数
    public class Baffle : ObservableObject
    {
        private double _Baffle_MidMidDis;
        public double Baffle_MidMidDis
        {
            get { return _Baffle_MidMidDis; }
            set { Set(ref _Baffle_MidMidDis, value); }
        }

        private double _Baffle_Width;
        public double Baffle_Width
        {
            get { return _Baffle_Width; }
            set { Set(ref _Baffle_Width, value); }
        }

        private double _Baffle_Height;
        public double Baffle_Height
        {
            get { return _Baffle_Height; }
            set { Set(ref _Baffle_Height, value); }
        }
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
