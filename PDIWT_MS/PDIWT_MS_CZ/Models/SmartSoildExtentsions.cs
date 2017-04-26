using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bentley.Interop.MicroStationDGN;


namespace PDIWT_MS_CZ.Models
{
    public static class SmartSoildExtentsions
    {
        public static SmartSolidElement CreateChameferedBox(this SmartSolid ss, double width, double length, double height, ChameferFace ExcludeChameferFace = ChameferFace.All, double chameferLength = 0)
        {
            Application app = Program.COM_App;
            SmartSolidElement boxele = ss.CreateSlab(null, width, length, height);
            if ((ExcludeChameferFace != ChameferFace.All) && (chameferLength != 0) && (new double[]{ width, length,height}.Min()/2 >= chameferLength))
            {
                double halfwidth = width / 2.0, halflength = length / 2.0, halfheight = height / 2.0;
                Point3d[] middleedgePoints =
                {
                    app.Point3dFromXYZ(halfwidth,-halflength,0),
                    app.Point3dFromXYZ(halfwidth,halflength,0),
                    app.Point3dFromXYZ(-halfwidth,halflength,0),
                    app.Point3dFromXYZ(-halfwidth,-halflength,0),
                    app.Point3dFromXYZ(0,-halflength,-halfheight),
                    app.Point3dFromXYZ(0,-halflength,halfheight),
                    app.Point3dFromXYZ(0,halflength,halfheight),
                    app.Point3dFromXYZ(0,halflength,-halfheight),
                    app.Point3dFromXYZ(halfwidth,0,-halfheight),
                    app.Point3dFromXYZ(halfwidth,0,halfheight),
                    app.Point3dFromXYZ(-halfwidth,0,halfheight),
                    app.Point3dFromXYZ(-halfwidth,0,-halfheight)
                };
                Dictionary<int, int[]> vertexedgerelation = new Dictionary<int, int[]>
                {
                    {0,new int[] {0,5,9} },
                    {1,new int[] {1,6,9} },
                    {2,new int[] {2,6,10} },
                    {3,new int[] {3,5,10} },
                    {4,new int[] {0,4,8} },
                    {5,new int[] {1,7,8} },
                    {6,new int[] {2,7,11} },
                    {7,new int[] {3,4,11} }
                };
                Dictionary<string, int[]> faceedgerelation = new Dictionary<string, int[]>
                {
                    {"Front", new int[] {0,3,4,5} },
                    {"Back", new int[] {1,2,6,7} },
                    {"Right", new int[] {0,1,8,9} },
                    {"Left", new int[] {2,3,10,11} },
                    {"Top", new int[] {5,6,9,10} },
                    {"Bottom", new int[] {4,7,8,11} }
                };
                List<int> chameferededge = new List<int>();
                for (int i = 0; i < 12; i++)
                {
                    chameferededge.Add(i);
                }

                if((ExcludeChameferFace & ChameferFace.Front) !=0)
                    chameferededge = chameferededge.Except(faceedgerelation["Front"]).ToList();
                if ((ExcludeChameferFace & ChameferFace.Back) != 0)
                    chameferededge = chameferededge.Except(faceedgerelation["Back"]).ToList();
                if ((ExcludeChameferFace & ChameferFace.Top) != 0)
                    chameferededge = chameferededge.Except(faceedgerelation["Top"]).ToList();
                if ((ExcludeChameferFace & ChameferFace.Bottom) != 0)
                    chameferededge = chameferededge.Except(faceedgerelation["Bottom"]).ToList();
                if ((ExcludeChameferFace & ChameferFace.Right) != 0)
                    chameferededge = chameferededge.Except(faceedgerelation["Right"]).ToList();
                if ((ExcludeChameferFace & ChameferFace.Left) != 0)
                    chameferededge = chameferededge.Except(faceedgerelation["Left"]).ToList();
                for (int i = 0; i < chameferededge.Count; i++)
                {
                    boxele = ss.ChamferEdge(boxele, ref middleedgePoints[chameferededge[i]], chameferLength, chameferLength, true);
                }

                if (((ExcludeChameferFace & ChameferFace.XAxis) == 0) && 
                    ((ExcludeChameferFace & ChameferFace.YAxis) == 0) && 
                    ((ExcludeChameferFace & ChameferFace.ZAxis) == 0))
                {
                    double halfwidth_wcham = halfwidth - chameferLength, halflength_wcham = halflength - chameferLength, halfheight_wcham = halfheight - chameferLength;
                    Point3d[] champlusPoints =
                    {
                    app.Point3dFromXYZ(halfwidth_wcham,-halflength_wcham,halfheight),
                    app.Point3dFromXYZ(halfwidth_wcham,-halflength,halfheight_wcham),
                    app.Point3dFromXYZ(halfwidth,-halflength_wcham,halfheight_wcham)
                    };
                    ShapeElement champlus_shape = app.CreateShapeElement1(null, ref champlusPoints, MsdFillMode.Filled);
                    SmartSolidElement champlus_ele_template = ss.ExtrudeClosedPlanarCurve(champlus_shape, 2 * chameferLength, 0, true);
                    SmartSolidElement[] champlus_eleList = new SmartSolidElement[8];
                    champlus_eleList[0] = champlus_ele_template.Clone().AsSmartSolidElement;
                    champlus_eleList[1] = champlus_eleList[0].Clone().AsSmartSolidElement;
                    champlus_eleList[1].Mirror(ref middleedgePoints[10], ref middleedgePoints[9]);
                    champlus_eleList[2] = champlus_eleList[1].Clone().AsSmartSolidElement;
                    champlus_eleList[2].Mirror(ref middleedgePoints[5], ref middleedgePoints[6]);
                    champlus_eleList[3] = champlus_eleList[0].Clone().AsSmartSolidElement;
                    champlus_eleList[3].Mirror(ref middleedgePoints[5], ref middleedgePoints[6]);
                    champlus_eleList[4] = champlus_eleList[0].Clone().AsSmartSolidElement;
                    champlus_eleList[4].Mirror3d(ref middleedgePoints[0], ref middleedgePoints[1], ref middleedgePoints[2]);
                    champlus_eleList[5] = champlus_eleList[4].Clone().AsSmartSolidElement;
                    champlus_eleList[5].Mirror(ref middleedgePoints[11], ref middleedgePoints[8]);
                    champlus_eleList[6] = champlus_eleList[5].Clone().AsSmartSolidElement;
                    champlus_eleList[6].Mirror(ref middleedgePoints[4], ref middleedgePoints[7]);
                    champlus_eleList[7] = champlus_eleList[4].Clone().AsSmartSolidElement;
                    champlus_eleList[7].Mirror(ref middleedgePoints[4], ref middleedgePoints[7]);
                    List<int> addplusvertex = new List<int>();
                    foreach (var verealtion in vertexedgerelation)
                    {
                        if (verealtion.Value.Except(chameferededge).Count() == 0)
                            addplusvertex.Add(verealtion.Key);
                    }
                    foreach (var plusindex in addplusvertex)
                    {
                        boxele = ss.SolidSubtract(boxele, champlus_eleList[plusindex]);
                    }
                }                
            }
            return boxele;
        }
    }

   [Flags]
    public enum ChameferFace
    {
        None = XAxis | YAxis | ZAxis,
        Front = 0x0001,
        Back = 0x0002,
        Right = 0x0004,
        Left = 0x0008,
        Top = 0x0010,
        Bottom = 0x0020,
        XAxis = Left | Right,
        YAxis = Front | Back,
        ZAxis = Top | Bottom,
        All = 0
    }
}
