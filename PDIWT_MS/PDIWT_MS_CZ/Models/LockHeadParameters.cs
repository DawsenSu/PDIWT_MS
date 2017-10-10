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
    }
    #region 底板参数

    public class BaseBoard
    {
        public double BaseBoardLength { get; set; }
        public double BaseBoardWidth { get; set; }
        public double BaseBoardHeight{ get; set; }
        public double EntranceWidth { get; set; }
        public bool IsGrooved { get; set; }
        
    }

    #endregion

    #region 闸首空箱参数集（1.长方形空箱 2.Z平面任意形状空箱）
    /// <summary>
    /// 长方形空箱
    /// </summary>
    public class EmptyRectBoxParameters
    {
        // 位置参数
        public double XDis { get; set; }
        public double YDis { get; set; }
        public double ZDis { get; set; }

        //形状参数
        public double EmptyBoxLength { get; set; }
        public double EmptyBoxWidth { get; set; }
        public double EmptyBoxHeight { get; set; }

        private List<EmptyBoxEdgeChameferInfo> _mChameferInfos;
        public List<EmptyBoxEdgeChameferInfo> ChameferInfos
        {
            get
            {
                return _mChameferInfos ?? new List<EmptyBoxEdgeChameferInfo>();
            }
            set { _mChameferInfos = value; }
        }
    }
    /// <summary>
    /// Z平面任意形状空箱
    /// </summary>
    public class EmptyZPlanBoxParameters
    {
        // 位置参数
        public double XDis { get; set; }
        public double YDis { get; set; }
        public double ZDis { get; set; }
        //形状参数
        public double EmptyBoxHeight { get; set; }

        private List<DPoint2d> _dPoint2Ds;

        public List<DPoint2d> Point2Ds
        {
            get { return _dPoint2Ds ?? new List<DPoint2d>(); }
            set { _dPoint2Ds = value; }
        }
        private List<EmptyBoxEdgeChameferInfo> _mChameferInfos;
        public List<EmptyBoxEdgeChameferInfo> ChameferInfos
        {
            get { return _mChameferInfos ?? new List<EmptyBoxEdgeChameferInfo>(); }
            set { _mChameferInfos = value; }
        }
    }
    /// <summary>
    /// 空箱倒角信息（倒角边标识，是否倒角，倒角长度，倒角宽度）
    /// </summary>
    public class EmptyBoxEdgeChameferInfo
    {
        public int EdgeFlag { get; set; }
        public bool IsChamefered { get; set; }
        public double ChamferLength { get; set; }
        public double ChamferWidth { get; set; }
    }
    #endregion


}
