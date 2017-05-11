using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG = Bentley.GeometryNET;

using HCHXCodeQueryLib;

namespace PDIWT_MS_ZJCZL.Models
{
    public static class HCHXQueryLibExtentsion
    {
        public static List<ColumnLayerInfo> GetSortedColumnLayerList(this ColumnLayerInfoArray colarray)
        {
            var layerList = from layer in colarray.m_layers
                            orderby layer.BasePosition.Z descending
                            select layer;
            return layerList.ToList();
        }
        public static Point3d Scale(this Point3d p,double scale)
        {
            return new Point3d() { X = p.X * scale, Y = p.Y * scale, Z = p.Z * scale };
        }
        public static double Distance(this Point3d p, Point3d p1)=> Math.Sqrt(Math.Pow(p.X - p1.X, 2) + Math.Pow(p.Y - p1.Y, 2) + Math.Pow(p.Z - p1.Z, 2));

        public static Point3d DPoint3dToPoint3d(this BG.DPoint3d p,double scale =1)
        {
            return new Point3d() { X = p.X*scale, Y = p.Y*scale, Z = p.Z*scale };
        }
    }
}
