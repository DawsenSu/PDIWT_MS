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
    public class PileInfoClass
    {
        [DisplayName("桩ID"), Description("DGN文件中桩的Id号"), Category("桩信息"), ReadOnly(true)]
        public double PileId { get; set; }
        [DisplayName("桩编号"), Description("桩的编号信息"), Category("桩信息"), ReadOnly(true)]
        public string PileCode { get; set; }
        [DisplayName("土层信息"), Category("桩穿过的土层信息"), ReadOnly(true)]
        public ObservableCollection<SoilInfoClass> SoilInfo { get; set; }
    }

    public class SoilInfoClass
    {
        [DisplayName("土层编号"), ReadOnly(true)]
        public string SoilLayerNum { get; set; }
        [DisplayName("土层名称"), ReadOnly(true)]
        public string SoilLayerName { get; set; }
        [DisplayName("与此土层顶层交点"),ReadOnly(true)]
        public Point3d TopPoint { get; set; }
        [DisplayName("与此土层顶层交点"), ReadOnly(true)]
        public Point3d BottomPoint { get; set; }
        [DisplayName("相交段长度(mm)"), ReadOnly(true)]
        public double Length { get; set; }
        public SoilInfoClass() { }
        public SoilInfoClass(string soillayernum, string soillayername,Point3d toppoint, Point3d bottompoint)
        {
            SoilLayerNum = soillayernum;
            SoilLayerName = soillayername;
            TopPoint = toppoint;
            BottomPoint = bottompoint;
            Length = Math.Round(Math.Sqrt(Math.Pow((TopPoint.X - BottomPoint.X), 2) + Math.Pow((TopPoint.Y - BottomPoint.Y), 2) + Math.Pow((TopPoint.Z - BottomPoint.Z), 2)),0);
        }

        public override string ToString()
        {
            return SoilLayerName + "土层";
        }
    }

    public class SearchPanel
    {
        public SearchType Type { get; set; }
        public string Name { get; set; }
    }

    public enum SearchType
    {
        ById,
        ByRay
    }
}
