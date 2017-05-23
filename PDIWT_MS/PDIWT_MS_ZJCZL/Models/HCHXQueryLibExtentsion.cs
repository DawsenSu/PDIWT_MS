using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG = Bentley.GeometryNET;
using MES = MathNet.Spatial.Euclidean;
using BCOM = Bentley.Interop.MicroStationDGN;
using HCHXCodeQueryLib;
using Bentley.DgnPlatformNET.DgnEC;
using Bentley.EC.Persistence.Query;
using Bentley.ECObjects.Schema;

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
        public static HCHXCodeQueryErrorCode QueryByRay(this HCHXCodeQuery pilequery, ref ColumnLayerInfoArray result, BG.DSegment3d linesegement)
        {
            return pilequery.QueryByRay(ref result, linesegement.StartPoint.DPoint3dToPoint3d(), linesegement.EndPoint.DPoint3dToPoint3d());
        }

        #region 扩展HCXH Point结构
        public static BCOM.Point3d Point3dToBCOMPoint3d(this Point3d p, double scale)
        {
            return new Bentley.Interop.MicroStationDGN.Point3d() { X = p.X * scale, Y = p.Y * scale, Z = p.Z * scale };
        }
        public static Point3d Scale(this Point3d p, double scale)
        {
            return new Point3d() { X = p.X * scale, Y = p.Y * scale, Z = p.Z * scale };
        }
        public static double Distance(this Point3d p, Point3d p1) => Math.Sqrt(Math.Pow(p.X - p1.X, 2) + Math.Pow(p.Y - p1.Y, 2) + Math.Pow(p.Z - p1.Z, 2));

        public static BG.DPoint3d Point3dToDPoint3d(this Point3d p, double scale = 1)
        {
            return new BG.DPoint3d(p.X * scale, p.Y * scale, p.Z * scale);
        }
        #endregion
        #region 扩展BG.DPoint3d
        public static Point3d DPoint3dToPoint3d(this BG.DPoint3d p, double scale = 1)
        {
            return new Point3d() { X = p.X * scale, Y = p.Y * scale, Z = p.Z * scale };
        }
        #endregion
        #region 扩展BCOM.Point3d
        public static BG.DPoint3d Point3dToDPoint3d(this BCOM.Point3d p, double scale = 1)
        {
            return new BG.DPoint3d(p.X * scale, p.Y * scale, p.Z * scale);
        }
        #endregion


        //其余扩展
        public static MES.Line3D DSegement3dToLine3D(this BG.DSegment3d p)
        {
            var startp = p.StartPoint;
            var endp = p.EndPoint;
            return new MES.Line3D(new MES.Point3D(startp.X, startp.Y, startp.Z), new MES.Point3D(endp.X, endp.Y, endp.Z));
        }
        public static BG.DSegment3d Line3DToDSegement3d(this MES.Line3D l)
        {
            var startp = l.StartPoint;
            var endp = l.EndPoint;
            return new BG.DSegment3d(startp.X, startp.Y, startp.Z, endp.X, endp.Y, endp.Z);
        }
        public static BG.DPoint3d Point3DToDPoint3d(this MES.Point3D p)
        {
            return new BG.DPoint3d(p.X, p.Y, p.Z);
        }  

    }

    //public class Test
    //{
    //    DgnECManager manager = DgnECManager.Manager;
    //    public double TestEC()
    //    {
    //        FindInstancesScope scop = FindInstancesScope.CreateScope(eEndSize, new FindInstancesScopeOption(DgnECHostType.Element, false));
    //        List<IECClass> classes = new List<IECClass>();
    //        var sscop = FindInstancesScope.CreateScope(Program.GetActiveDgnModel(), new FindInstancesScopeOption());
    //        IECSchema iecs = manager.LocateSchema("ds", 1, 0, SchemaMatchType.Exact, null, "sd");
    //        var dd = iecs.GetClasses();
    //        ECQuery query = new ECQuery(dd);
    //        query.SelectClause.SelectAllProperties = true;
    //        manager.FindInstances(scop, query);

    //    }
    //}
}
