using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.Collections;
using System.IO;

using PDIWT_MS_CZ.Models;

using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;

using BM = Bentley.MstnPlatformNET;
using BD = Bentley.DgnPlatformNET;
using Bentley.Interop.MicroStationDGN;
using System.Windows.Forms;

namespace PDIWT_MS_CZ.ViewModels
{
    public class ViewCZViewModel : ViewModelBase
    {
        Bentley.Interop.MicroStationDGN.Application app = Program.COM_App;
        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();

            DB_Length = 28000; DB_Width = 40000; DB_Thickness = 3000;

            BDun_Thickness = 17200;
            BDun_A = 8500; BDun_B = 10000; BDun_C = 2100;/* BDun_D = 16000;*/ BDun_E = 2100; BDun_F = 2000;
            IsIncludeBDunChamfer = true; BDun_Tx = 1387; BDun_Ty = 1387; BDun_R1 = 1000;

            MK_Thickness = 4300;
            /*MK_A = 11500;*/ MK_B = 2260; MK_C = 6650; /*MK_D = 10000;*/ MK_E = 2248.2215; MK_F = 2754.7785;

            SSLD_Thickness = 3500; SSLD_YDis = 1500;
            SSLD_A = 6300; SSLD_B = 18200; SSLD_C = 2600; SSLD_D = 24500; SSLD_E = 4700; SSLD_F = 4600;
            SSLD_R1 = 0; SSLD_R2 = 0; SSLD_R3 = 1800; SSLD_R4 = 6400;

            HoleParamList = new ObservableCollection<HoleProperty>()
            {
                new HoleProperty { HoleHeight=8400,  HoleLength =4200, HoleWidth =6900, ChamferLength=500, XDis=800, YDis=900, ZDis=300},
                new HoleProperty { HoleHeight=2200,  HoleLength =6000, HoleWidth =4800, ChamferLength=500, XDis=800, YDis=10900, ZDis=300 },
                new HoleProperty { HoleHeight=5400,  HoleLength =6000, HoleWidth =4800, ChamferLength=500, XDis=800, YDis=10900, ZDis=3300 },
                new HoleProperty { HoleHeight=8400,  HoleLength =2700, HoleWidth =4800, ChamferLength=500, XDis=800, YDis=17900, ZDis=300 },
                new HoleProperty { HoleHeight=8400,  HoleLength =5500, HoleWidth =4800, ChamferLength=500, XDis=800, YDis=21600, ZDis=300 }
            };
            TrapHoleThickness = 8400; IsIncludeTrapHole = true;
            TrapHoleXLength = 6400; TrapHoleYLength = 4300; TrapHoleXLengthCorner = 3300; TrapHoleYLengthCorner = 3300;
            TrapHoleXDis = 800; TrapHoleYDis = 5800; TrapHoleZDis = 300;

            IsIncludeGs = true;
            GS_MidWidth = 1000;
            GS_Intervals = new ObservableCollection<GSProperty>
            {
                new GSProperty { Interval=250,IntervalType="格栅间距" },
                new GSProperty { Interval=500,IntervalType="格栅宽度" },
                new GSProperty { Interval=500,IntervalType="格栅间距" },
                new GSProperty { Interval=500,IntervalType="格栅宽度" },
                new GSProperty { Interval=500,IntervalType="格栅间距" },
                new GSProperty { Interval=500,IntervalType="格栅宽度" },
                new GSProperty { Interval=500,IntervalType="格栅间距" },
                new GSProperty { Interval=500,IntervalType="格栅宽度" },
                new GSProperty { Interval=500,IntervalType="格栅间距" },
                new GSProperty { Interval=500,IntervalType="格栅宽度" },
                new GSProperty { Interval=500,IntervalType="格栅间距" },
                new GSProperty { Interval=800,IntervalType="格栅宽度" },
                new GSProperty { Interval=800,IntervalType="格栅间距" },
                new GSProperty { Interval=800,IntervalType="格栅宽度" },
                new GSProperty { Interval=800,IntervalType="格栅间距" },
                new GSProperty { Interval=800,IntervalType="格栅宽度" },
                new GSProperty { Interval=800,IntervalType="格栅间距" },
                new GSProperty { Interval=1000,IntervalType="格栅宽度" }
            };
        }

        #region VertifyProperty
        public string ErrorInfo
        {
            get { return GetProperty(() => ErrorInfo); }
            set { SetProperty(() => ErrorInfo, value); }
        }

        #endregion

        #region DB Property
        public double DB_Length
        {
            get { return GetProperty(() => DB_Length); }
            set { SetProperty(() => DB_Length, value); }
        }
        public double DB_Width
        {
            get { return GetProperty(() => DB_Width); }
            set { SetProperty(() => DB_Width, value); }
        }
        public double DB_Thickness
        {
            get { return GetProperty(() => DB_Thickness); }
            set { SetProperty(() => DB_Thickness, value); }
        }
        #endregion

        #region BDun Property
        public double BDun_Thickness
        {
            get { return GetProperty(() => BDun_Thickness); }
            set { SetProperty(() => BDun_Thickness, value); }
        }
        public double BDun_A
        {
            get { return GetProperty(() => BDun_A); }
            set { SetProperty(() => BDun_A, value); }
        }
        public double BDun_B
        {
            get { return GetProperty(() => BDun_B); }
            set { SetProperty(() => BDun_B, value); }
        }
        public double BDun_C
        {
            get { return GetProperty(() => BDun_C); }
            set { SetProperty(() => BDun_C, value); }
        }
        //public double BDun_D
        //{
        //    get { return GetProperty(() => BDun_D); }
        //    set { SetProperty(() => BDun_D, value); }
        //}
        public double BDun_E
        {
            get { return GetProperty(() => BDun_E); }
            set { SetProperty(() => BDun_E, value); }
        }
        public double BDun_F
        {
            get { return GetProperty(() => BDun_F); }
            set { SetProperty(() => BDun_F, value); }
        }

        public double BDun_Tx
        {
            get { return GetProperty(() => BDun_Tx); }
            set { SetProperty(() => BDun_Tx, value); }
        }
        public double BDun_Ty
        {
            get { return GetProperty(() => BDun_Ty); }
            set { SetProperty(() => BDun_Ty, value); }
        }
        public double BDun_R1
        {
            get { return GetProperty(() => BDun_R1); }
            set { SetProperty(() => BDun_R1, value); }
        }
        public bool IsIncludeBDunChamfer
        {
            get { return GetProperty(() => IsIncludeBDunChamfer); }
            set { SetProperty(() => IsIncludeBDunChamfer, value); }
        }

        #endregion

        #region MK Property
        public double MK_Thickness
        {
            get { return GetProperty(() => MK_Thickness); }
            set { SetProperty(() => MK_Thickness, value); }
        }
        //public double MK_A
        //{
        //    get { return GetProperty(() => MK_A); }
        //    set { SetProperty(() => MK_A, value); }
        //}
        public double MK_B
        {
            get { return GetProperty(() => MK_B); }
            set { SetProperty(() => MK_B, value); }
        }
        public double MK_C
        {
            get { return GetProperty(() => MK_C); }
            set { SetProperty(() => MK_C, value); }
        }
        //public double MK_D
        //{
        //    get { return GetProperty(() => MK_D); }
        //    set { SetProperty(() => MK_D, value); }
        //}
        public double MK_E
        {
            get { return GetProperty(() => MK_E); }
            set { SetProperty(() => MK_E, value); }
        }
        public double MK_F
        {
            get { return GetProperty(() => MK_F); }
            set { SetProperty(() => MK_F, value); }
        }
        #endregion

        #region SSLD Property
        public double SSLD_Thickness
        {
            get { return GetProperty(() => SSLD_Thickness); }
            set { SetProperty(() => SSLD_Thickness, value); }
        }
        public double SSLD_YDis
        {
            get { return GetProperty(() => SSLD_YDis); }
            set { SetProperty(() => SSLD_YDis, value); }
        }
        public double SSLD_A
        {
            get { return GetProperty(() => SSLD_A); }
            set { SetProperty(() => SSLD_A, value); }
        }
        public double SSLD_B
        {
            get { return GetProperty(() => SSLD_B); }
            set { SetProperty(() => SSLD_B, value); }
        }
        public double SSLD_C
        {
            get { return GetProperty(() => SSLD_C); }
            set { SetProperty(() => SSLD_C, value); }
        }
        public double SSLD_D
        {
            get { return GetProperty(() => SSLD_D); }
            set { SetProperty(() => SSLD_D, value); }
        }
        public double SSLD_E
        {
            get { return GetProperty(() => SSLD_E); }
            set { SetProperty(() => SSLD_E, value); }
        }
        public double SSLD_F
        {
            get { return GetProperty(() => SSLD_F); }
            set { SetProperty(() => SSLD_F, value); }
        }
        public double SSLD_R1
        {
            get { return GetProperty(() => SSLD_R1); }
            set { SetProperty(() => SSLD_R1, value); }
        }
        public double SSLD_R2
        {
            get { return GetProperty(() => SSLD_R2); }
            set { SetProperty(() => SSLD_R2, value); }
        }
        public double SSLD_R3
        {
            get { return GetProperty(() => SSLD_R3); }
            set { SetProperty(() => SSLD_R3, value); }
        }
        public double SSLD_R4
        {
            get { return GetProperty(() => SSLD_R4); }
            set { SetProperty(() => SSLD_R4, value); }
        }
        #endregion

        #region Hole Property
        public ObservableCollection<HoleProperty> HoleParamList
        {
            get { return GetProperty(() => HoleParamList); }
            set { SetProperty(() => HoleParamList, value); }
        }

        public bool IsIncludeTrapHole
        {
            get { return GetProperty(() => IsIncludeTrapHole); }
            set { SetProperty(() => IsIncludeTrapHole, value); }
        }

        public double TrapHoleXLength
        {
            get { return GetProperty(() => TrapHoleXLength); }
            set { SetProperty(() => TrapHoleXLength, value); }
        }

        public double TrapHoleYLength
        {
            get { return GetProperty(() => TrapHoleYLength); }
            set { SetProperty(() => TrapHoleYLength, value); }
        }

        public double TrapHoleXLengthCorner
        {
            get { return GetProperty(() => TrapHoleXLengthCorner); }
            set { SetProperty(() => TrapHoleXLengthCorner, value); }
        }

        public double TrapHoleYLengthCorner
        {
            get { return GetProperty(() => TrapHoleYLengthCorner); }
            set { SetProperty(() => TrapHoleYLengthCorner, value); }
        }

        public double TrapHoleXDis
        {
            get { return GetProperty(() => TrapHoleXDis); }
            set { SetProperty(() => TrapHoleXDis, value); }
        }

        public double TrapHoleYDis
        {
            get { return GetProperty(() => TrapHoleYDis); }
            set { SetProperty(() => TrapHoleYDis, value); }
        }

        public double TrapHoleZDis
        {
            get { return GetProperty(() => TrapHoleZDis); }
            set { SetProperty(() => TrapHoleZDis, value); }
        }

        public double TrapHoleThickness
        {
            get { return GetProperty(() => TrapHoleThickness); }
            set { SetProperty(() => TrapHoleThickness, value); }
        }
        #endregion

        #region GS Property

        public bool IsIncludeGs
        {
            get { return GetProperty(() => IsIncludeGs); }
            set { SetProperty(() => IsIncludeGs, value); }
        }
        public double GS_MidWidth
        {
            get { return GetProperty(() => GS_MidWidth); }
            set { SetProperty(() => GS_MidWidth, value); }
        }
        public ObservableCollection<GSProperty> GS_Intervals
        {
            get { return GetProperty(() => GS_Intervals); }
            set { SetProperty(() => GS_Intervals, value); }
        }
        #endregion

        #region Methods
        //可以使用SmartSoild.Blend命令来创建圆角
        SmartSolidElement GetCylinderCorner(Point3d Cornerorigin, double r, double height, double rotdegree = 0)
        {
            SmartSolidElement cl = app.SmartSolid.CreateCylinder(null, r, height);
            SmartSolidElement box = app.SmartSolid.CreateSlab(null, r, r, height);
            Point3d box_offset = app.Point3dFromXY(-r / 2, -r / 2);
            box.Move(ref box_offset);
            SmartSolidElement corner = app.SmartSolid.SolidSubtract(box, cl);
            Point3d cl_offset = app.Point3dFromXYZ(Cornerorigin.X + r, Cornerorigin.Y + r, Cornerorigin.Z + height / 2);
            corner.Move(ref cl_offset);
            corner.RotateAboutZ(Cornerorigin, app.Radians(rotdegree));
            return corner;
        }
        void GetCylinderConer_C(/*Point3d corOrigin, double r, double height, double rotdegere =0*/)
        {
            ChainableElement[] elmArray = new ChainableElement[2];
            Point3d[] pntArray =
            {
                app.Point3dFromXY(0,10),
                app.Point3dFromXY(0,0),
                app.Point3dFromXY(10,0)
            };
            elmArray[0] = app.CreateLineElement1(null, ref pntArray);
            Point3d centerPoint = app.Point3dFromXY(10, 10);
            elmArray[1] = app.CreateArcElement1(null, ref pntArray[2], ref centerPoint, pntArray[0]);
            ComplexShapeElement oComplexShape = app.CreateComplexShapeElement1(ref elmArray, MsdFillMode.Filled);
            SmartSolidElement outele = app.SmartSolid.ExtrudeClosedPlanarCurve(oComplexShape, 10, 0, true);
            outele.Move(ref pntArray[0]);
            app.ActiveModelReference.AddElement(outele);
        }
        // 返回点的坐标在正方体的（后，左，上）角 
        //通过测试
        SmartSolidElement GetChamferedBox(double width, double length, double height, double chamferLength = 0)
        {
            SmartSolidElement boxele = app.SmartSolid.CreateChameferedBox(width, length, height, ChameferFace.Top, chamferLength);
            Point3d boxele_offset = app.Point3dFromXYZ(width / 2, length / 2, -height / 2);
            boxele.Move(ref boxele_offset);
            return boxele;
        }

        double GetBDun_D()
        {
            return DB_Length - BDun_B - BDun_F;
        }
        double GetMK_A()
        {
            return DB_Width / 2.0 - BDun_A;
        }
        double GetMK_D()
        {
            return BDun_B;
        }

        SmartSolidElement GetDB()
        {
            SmartSolidElement ele_db = app.SmartSolid.CreateSlab(null, DB_Width / 2, DB_Length, DB_Thickness);
            Point3d eledboffset = app.Point3dFromXYZ(-DB_Width / 4, DB_Length / 2, DB_Thickness / 2);
            ele_db.Move(ref eledboffset);
            return ele_db;
        }
        SmartSolidElement GetBDun()
        {
            double bdun_d = GetBDun_D();
            Point3d[] bdun_shapepoints =
                {
                    app.Point3dFromXY(0,0),
                    app.Point3dFromXY(BDun_A,0),
                    app.Point3dFromXY(BDun_A,BDun_B),
                    app.Point3dFromXY(BDun_A - BDun_C,BDun_B),
                    app.Point3dFromXY(BDun_A - BDun_C,BDun_B + bdun_d),
                    app.Point3dFromXY(BDun_A - BDun_C + BDun_E,BDun_B + bdun_d),
                    app.Point3dFromXY(BDun_A - BDun_C + BDun_E,BDun_B + bdun_d + BDun_F),
                    app.Point3dFromXY(0,BDun_B + bdun_d + BDun_F)
                };
            ShapeElement bdun_shape = app.CreateShapeElement1(null, ref bdun_shapepoints, MsdFillMode.Filled);
            SmartSolidElement ele_bdun = app.SmartSolid.ExtrudeClosedPlanarCurve(bdun_shape, BDun_Thickness, 0, true);
            if (IsIncludeBDunChamfer)
            {
                Point3d bdun_Thickness_2 = app.Point3dFromXYZ(0, 0, BDun_Thickness / 2);
                if (BDun_Tx > 0 && BDun_Ty > 0)
                {
                    Point3d bdun_chamferpoint = app.Point3dAdd(ref bdun_shapepoints[3], ref bdun_Thickness_2);
                    ele_bdun = app.SmartSolid.ChamferEdge(ele_bdun, ref bdun_chamferpoint, BDun_Ty, BDun_Tx, true);
                }
                if (BDun_R1 > 0)
                {
                    Point3d bdun_blendpoint = app.Point3dAdd(ref bdun_Thickness_2, ref bdun_shapepoints[5]);
                    ele_bdun = app.SmartSolid.BlendEdgeWithVariableRadius(ele_bdun, ref bdun_blendpoint, BDun_R1, true);
                }
            }
            Point3d ele_bdun_offset = app.Point3dFromXY(-DB_Width / 2, 0);
            ele_bdun.Move(ref ele_bdun_offset);
            return ele_bdun;
        }
        SmartSolidElement GetMK()
        {
            double mk_a = GetMK_A();
            double mk_d = GetMK_D();
            Point3d[] mk_shapepoints =
            {
                app.Point3dFromXY(0,0),
                app.Point3dFromXY(-mk_a,0),
                app.Point3dFromXY(-mk_a,mk_d),
                app.Point3dFromXY(-mk_a + MK_B,mk_d + MK_E),
                app.Point3dFromXY(-mk_a + MK_B +MK_C , mk_d + MK_E +MK_F),
                app.Point3dFromXY(0, mk_d + MK_E +MK_F)
            };
            ShapeElement mk_shape = app.CreateShapeElement1(null, ref mk_shapepoints, MsdFillMode.Filled);
            Point3d mk_shape_offset = app.Point3dFromXYZ(0, 0, DB_Thickness);
            mk_shape.Move(ref mk_shape_offset);
            SmartSolidElement ele_mk = app.SmartSolid.ExtrudeClosedPlanarCurve(mk_shape, 0, -MK_Thickness, true);
            return ele_mk;
        }
        SmartSolidElement GetSSLD()
        {
            Point3d[] ssld_shapeppoints =
            {
                app.Point3dFromXY(0,0),
                app.Point3dFromXY(-SSLD_B,0), //[1], R4
                app.Point3dFromXY(-SSLD_B,SSLD_D),//[2], R2
                app.Point3dFromXY(-SSLD_B + SSLD_F,SSLD_D),
                app.Point3dFromXY(-SSLD_B + SSLD_F,SSLD_D - SSLD_E),
                app.Point3dFromXY(-SSLD_B + SSLD_C,SSLD_D - SSLD_E),//[5], R1
                app.Point3dFromXY(-SSLD_B + SSLD_C,SSLD_A),//[6], R3
                app.Point3dFromXY(0, SSLD_A)
                };
            ShapeElement ssld_shape = app.CreateShapeElement1(null, ref ssld_shapeppoints, MsdFillMode.Filled);
            SmartSolidElement ele_ssld = app.SmartSolid.ExtrudeClosedPlanarCurve(ssld_shape, -SSLD_Thickness, 0, true);

            if (SSLD_R4 > 0)
                ele_ssld = app.SmartSolid.SolidSubtract(ele_ssld, GetCylinderCorner(ssld_shapeppoints[1], SSLD_R4, SSLD_Thickness));
            if (SSLD_R3 > 0)
                ele_ssld = app.SmartSolid.SolidUnion(ele_ssld, GetCylinderCorner(ssld_shapeppoints[6], SSLD_R3, SSLD_Thickness));
            if (SSLD_R2 > 0)
                ele_ssld = app.SmartSolid.SolidSubtract(ele_ssld, GetCylinderCorner(ssld_shapeppoints[2], SSLD_R2, SSLD_Thickness, -90));
            if (SSLD_R1 > 0)
                ele_ssld = app.SmartSolid.SolidUnion(ele_ssld, GetCylinderCorner(ssld_shapeppoints[5], SSLD_R1, SSLD_Thickness, -90));
            Point3d ssld_shape_offset = app.Point3dFromXYZ(0, SSLD_YDis, DB_Thickness);
            ele_ssld.Move(ref ssld_shape_offset);
            return ele_ssld;
        }
        List<SmartSolidElement> GetHoleList()
        {
            var ele_holeList = new List<SmartSolidElement>();
            SmartSolidElement ele_hole_temp;
            Point3d hole_offset_temp;
            Point3d BDun_left_down_up_Point = app.Point3dFromXYZ(-DB_Width / 2, 0, BDun_Thickness);
            foreach (var holeinfo in HoleParamList)
            {
                //ele_hole_temp = app.SmartSolid.CreateSlab(null, holeinfo.HoleWidth, holeinfo.HoleLength, holeinfo.HoleHeight);
                ele_hole_temp = GetChamferedBox(holeinfo.HoleWidth, holeinfo.HoleLength, holeinfo.HoleHeight, holeinfo.ChamferLength);
                hole_offset_temp = app.Point3dFromXYZ(-DB_Width / 2 + holeinfo.XDis, holeinfo.YDis, BDun_Thickness - holeinfo.ZDis);
                ele_hole_temp.Move(ref hole_offset_temp);
                ele_holeList.Add(ele_hole_temp);
            }
            if (IsIncludeTrapHole)
            {
                SmartSolidElement ele_traphole = GetChamferedBox(TrapHoleXLength, TrapHoleYLength, TrapHoleThickness);
                Point3d ele_traphole_Chamferoffset = app.Point3dFromXYZ(TrapHoleXLength, TrapHoleYLength, -TrapHoleThickness / 2);
                ele_traphole = app.SmartSolid.ChamferEdge(ele_traphole, ref ele_traphole_Chamferoffset, TrapHoleYLengthCorner, TrapHoleXLengthCorner, true);
                Point3d ele_traphole_offset = app.Point3dFromXYZ(-DB_Width / 2 + TrapHoleXDis, TrapHoleYDis, BDun_Thickness - TrapHoleZDis);
                ele_traphole.Move(ref ele_traphole_offset);
                ele_holeList.Add(ele_traphole);
            }
            return ele_holeList;
        }
        List<SmartSolidElement> GetGSList()
        {
            double gs_length = (SSLD_A - GS_MidWidth) / 2;
            double gs_heigth = MK_Thickness - SSLD_Thickness;
            double gs_zcoordinate = DB_Thickness + MK_Thickness;
            var gs_XCoordinate = new List<double>();
            foreach (var gs_prop in GS_Intervals)
            {
                gs_XCoordinate.Add(-gs_prop.Interval);
            }
            var ele_gs_list = new List<SmartSolidElement>();
            if (gs_XCoordinate.Count > 1 && IsIncludeGs)
            {
                for (int i = 1; i < gs_XCoordinate.Count; i++)
                    gs_XCoordinate[i] = gs_XCoordinate[i] + gs_XCoordinate[i - 1];
                //绘制下半边
                SmartSolidElement ele_gs_temp;
                Point3d temp_offset;
                for (int i = 0; i < GS_Intervals.Count; i++)
                {
                    if (i % 2 == 1)
                    {
                        ele_gs_temp = GetChamferedBox(GS_Intervals[i].Interval, gs_length, gs_heigth);
                        temp_offset = app.Point3dFromXYZ(gs_XCoordinate[i], SSLD_YDis, gs_zcoordinate);
                        ele_gs_temp.Move(ref temp_offset);
                        ele_gs_list.Add(ele_gs_temp);
                    }
                }
                //克隆上半部
                Point3d gs_mirrorPoint1 = app.Point3dFromXY(0, SSLD_YDis + SSLD_A / 2);
                Point3d gs_mirrorPoint2 = app.Point3dFromXY(-100, SSLD_YDis + SSLD_A / 2);
                double gs_ele_halfcount = ele_gs_list.Count;
                for (int i = 0; i < gs_ele_halfcount; i++)
                {
                    ele_gs_temp = ele_gs_list[i].Clone().AsSmartSolidElement;
                    ele_gs_temp.Mirror(ref gs_mirrorPoint1, ref gs_mirrorPoint2);
                    ele_gs_list.Add(ele_gs_temp);
                }
            }
            return ele_gs_list;
        }

        void ComDrawAll()
        {
            SmartSolidElement ele_db = GetDB();
            SmartSolidElement ele_bdun = GetBDun();
            SmartSolidElement ele_mk = GetMK();
            SmartSolidElement ele_ssld = GetSSLD();
            List<SmartSolidElement> ele_holeList = GetHoleList();
            List<SmartSolidElement> ele_gsList = GetGSList();

            #region Sub\Union action
            SmartSolidElement cz, czLeft, czRigth;
            czLeft = app.SmartSolid.SolidUnion(ele_db, ele_bdun);
            czLeft = app.SmartSolid.SolidUnion(czLeft, ele_mk);
            czLeft = app.SmartSolid.SolidSubtract(czLeft, ele_ssld);
            foreach (var ele_hole in ele_holeList)
            {
                czLeft = app.SmartSolid.SolidSubtract(czLeft, ele_hole);
            }

            foreach (var ele_gs in ele_gsList)
            {
                czLeft = app.SmartSolid.SolidSubtract(czLeft, ele_gs);
            }
            czRigth = czLeft.Clone().AsSmartSolidElement;
            Point3d cz_mirrorStart, cz_mirrorEnd;
            cz_mirrorStart = app.Point3dZero();
            cz_mirrorEnd = app.Point3dFromXY(0, 1);
            czRigth.Mirror(ref cz_mirrorStart, ref cz_mirrorEnd);
            cz = app.SmartSolid.SolidUnion(czLeft, czRigth);
            app.ActiveModelReference.AddElement(cz);
            #endregion
        }
        #endregion



        private List<QuantityRowDef> CreateContent()
        {
            #region ModifyTheResult
            QuantityResult result = new QuantityResult();

            result.Item1 = (DB_Length + 2) * (DB_Width + 2) * 0.15 * 1e-9;

            double mkvol = GetMK().ComputeVolume()*2;
            double mkssldvol = 0;// SSLD_A * SSLD_Thickness * MK_A * 2; //存在问题
            SmartSolidElement ssld_insectbox = app.SmartSolid.CreateSlab(null, GetMK_A(), SSLD_A + SSLD_R3, SSLD_Thickness);
            Point3d point_offset = app.Point3dFromXYZ(-GetMK_A() / 2, SSLD_YDis + (SSLD_A + SSLD_R3) / 2, SSLD_Thickness / 2 + DB_Thickness);
            ssld_insectbox.Move(ref point_offset);
            SmartSolidElement tempssld = GetSSLD();
            //app.ActiveModelReference.AddElement(app.SmartSolid.SolidIntersect(ssld_insectbox, tempssld));
            mkssldvol = app.SmartSolid.SolidIntersect(ssld_insectbox, tempssld).ComputeVolume() * 2;
            double gsallvol = 0;
            foreach (var gs in GetGSList())
            {
                gsallvol += gs.ComputeVolume();
            }
            gsallvol *= 2;
            result.Item2 = (DB_Length * DB_Width * DB_Thickness + mkvol - mkssldvol - gsallvol) * 1e-9;

            result.Item3 = result.Item2 * 0.05;

            SmartSolidElement tempbdun = GetBDun();
            double bdunshapearea = tempbdun.ComputeVolume() / BDun_Thickness;
            double bdun_ssldvol = tempssld.ComputeVolume() * 2 - mkssldvol;
            result.Item7 = (bdunshapearea * MK_Thickness * 2 - bdun_ssldvol) * 1e-9;

            double holelistvol = 0;
            var holelist = GetHoleList();
            foreach (var holeele in holelist)
            {
                holelistvol += holeele.ComputeVolume();
            }
            holelistvol *= 2e-9;
            result.Item5 = bdunshapearea * (BDun_Thickness - DB_Thickness - MK_Thickness) * 2 * 1e-9 - result.Item7  -holelistvol;
            result.Item12 = (BDun_Thickness - DB_Thickness) * 0.313 * 2;
            double dbunedgelength = 0;
            dbunedgelength += 2 * (BDun_A + BDun_B + GetBDun_D() + BDun_E + BDun_F);
            if (IsIncludeBDunChamfer)
            {
                dbunedgelength -= (2 * BDun_R1 + BDun_Tx + BDun_Ty);
                dbunedgelength += (Math.Sqrt(BDun_Tx * BDun_Tx + BDun_Ty * BDun_Ty) + 1 / 2.0 * Math.PI * BDun_R1);
            }
            result.Item14 = dbunedgelength * 2;
            result.Item18 = (BDun_Thickness + DB_Width / 2) * 2 * 1e-3;
            result.Item19 = DB_Length * 2 * 1e-3;

            #endregion

            var content = new List<QuantityRowDef>
            {
                new QuantityRowDef { ItemOrder=1, ItemName= "素混凝土垫层", Unit="m^3", QuantityFormula ="(闸首长+2)*（闸首宽+2）*0.15", Quantity = result.Item1, EnlargeFactor = 1.1, Memo ="C15，厚150mm" },
                new QuantityRowDef { ItemOrder=2, ItemName= "现浇钢筋混凝土底板", Unit="m^3", QuantityFormula ="闸首长x闸首宽x底板厚+门槛-门槛部分廊道-出水格栅", Quantity = result.Item2, EnlargeFactor = 1.1, Memo ="C25（含廊道、门龛），底板最厚处7.6m" },
                new QuantityRowDef { ItemOrder=3, ItemName= "底板钢筋", Unit="t", QuantityFormula ="底板量*0.05", Quantity = result.Item3, EnlargeFactor = 1.1, Memo ="HRB400" },
                new QuantityRowDef { ItemOrder=4, ItemName= "底板二期混凝土", Unit="m^3", QuantityFormula ="450", Quantity = 450, EnlargeFactor = 1.1, Memo ="C30微膨胀混凝土，施工宽缝" },
                new QuantityRowDef { ItemOrder=5, ItemName= "现浇钢筋砼边墩（上部）", Unit="m^3", QuantityFormula ="边墩面积x（边墩高度-底板厚度-门槛厚度）-下部边墩-空箱", Quantity =result.Item5, EnlargeFactor = 1.1, Memo ="C25" },
                new QuantityRowDef { ItemOrder=6, ItemName= "边墩钢筋（上部）", Unit="t", QuantityFormula ="", EnlargeFactor = 1.1, Memo ="HRB400" },
                new QuantityRowDef { ItemOrder=7, ItemName= "现浇钢筋砼边墩（下部）", Unit="m^3", QuantityFormula ="边墩面积x门槛高度-廊道体积（不含门槛段）",Quantity=result.Item7, EnlargeFactor = 1.1, Memo ="HRB400" },
                new QuantityRowDef { ItemOrder=8, ItemName= "边墩钢筋（下部）", Unit="t", QuantityFormula ="", EnlargeFactor = 1.1, Memo ="HRB400" },
                new QuantityRowDef { ItemOrder=9, ItemName= "边墩二期混凝土", Unit="m^3", QuantityFormula ="176.12", Quantity=176.12, EnlargeFactor = 1.1, Memo ="C30微膨胀混凝土" },
                new QuantityRowDef { ItemOrder=10, ItemName= "钢护板、护角", Unit="t", QuantityFormula ="65", Quantity= 65, EnlargeFactor = 1.1, Memo ="Q235喷锌防腐，不含检修门槽处护角" },
                new QuantityRowDef { ItemOrder=11, ItemName= "钢护板锚筋", Unit="t", QuantityFormula ="65*0.021", Quantity =1.365,  EnlargeFactor = 1.1, Memo ="HPB300级Φ12" },
                new QuantityRowDef { ItemOrder=12, ItemName= "甲种爬梯", Unit="t", QuantityFormula ="（边墩高度-3）x0.313x2", Quantity=result.Item12, EnlargeFactor = 1.1, Memo ="18.3m/座，每个闸首2座" },
                new QuantityRowDef { ItemOrder=13, ItemName= "乙种爬梯", Unit="t", QuantityFormula ="", EnlargeFactor = 1.1, Memo ="12.3m/座，每个闸首2座" },
                new QuantityRowDef { ItemOrder=14, ItemName= "栏杆", Unit="t", QuantityFormula ="边墩边长之和x2", Quantity = result.Item14, EnlargeFactor = 1.1, Memo ="钢管及扁钢加工，每个闸首栏杆长度154m" },
                new QuantityRowDef { ItemOrder=15, ItemName= "铸铁水尺", Unit="m", QuantityFormula ="", EnlargeFactor = 1.1, Memo ="" },
                new QuantityRowDef { ItemOrder=16, ItemName= "SC镀锌钢管", Unit="m", QuantityFormula ="16", Quantity=16, EnlargeFactor = 1.1, Memo ="Φ219x6" },
                new QuantityRowDef { ItemOrder=17, ItemName= "SC镀锌钢管", Unit="m", QuantityFormula ="39.6", Quantity=39.6, EnlargeFactor = 1.1, Memo ="Φ102x5" },
                new QuantityRowDef { ItemOrder=18, ItemName= "紫铜止水", Unit="m", QuantityFormula ="（边墩高度+船闸宽度/2）x2", Quantity = result.Item18, EnlargeFactor = 1.1, Memo ="闸首与导航墙、闸室连接处" },
                new QuantityRowDef { ItemOrder=19, ItemName= "镀锌铁皮止水", Unit="m", QuantityFormula ="闸首长度*2", Quantity= result.Item19, EnlargeFactor = 1.1, Memo ="" },
                new QuantityRowDef { ItemOrder=20, ItemName= "观测点", Unit="个", QuantityFormula ="16", Quantity =16, EnlargeFactor = 1.0, Memo ="每闸首永久:8个，临时：8个，预埋铜钉" },
                new QuantityRowDef { ItemOrder=21, ItemName="帷幕灌浆", Unit="m", QuantityFormula="（闸室总长+5x2+船闸宽度）x2", Quantity = result.Item21, EnlargeFactor=1.0, Memo="深度8m" }
            };
            return content;
        }

        [Command]
        public void VertifyParam()
        {
            StringBuilder sb = new StringBuilder();
            double uorPerMaster = Program.GetActiveDgnModel().GetModelInfo().UorPerMaster;
            if (uorPerMaster != 10)
            {
#if DEBUG
                sb.Append("t=" + uorPerMaster);
#endif
                sb.Append("当前文件的工作单位不为mm\n请到|设置->文件->设计文件设置->工作单位->主单位（子单位）|中设置\n");
            }

            //if (DB_Length != (BDun_B + BDun_D + BDun_F))
            //    sb.Append("底板长度 != 边墩参数（b+d+f）\n");
            //if (DB_Width / 2 != (BDun_A + MK_A))
            //    sb.Append("底板宽度 / 2 != 边墩 a + 门槛 a \n");
            //if (MK_D != BDun_B)
            //    sb.Append("门槛参数 d != 边墩参数 b\n");

            if (SSLD_Thickness > MK_Thickness)
                sb.Append("疏水孔厚度 > 门槛厚度\n");
            if ((SSLD_YDis + SSLD_A) > GetMK_D())
                sb.Append("疏水孔Y轴距离 + 疏水孔 a > 门槛 d\n");
            if (SSLD_B > DB_Width / 2)
                sb.Append("疏水孔 b > 底板宽度 / 2\n");
            if ((SSLD_B - SSLD_C) < (GetMK_A() + BDun_C))
                sb.Append("疏水孔 b-c < 门槛a + 边墩c\n");
            if ((SSLD_YDis + SSLD_D) > DB_Width)
                sb.Append("疏水孔Y轴距离 + 疏水孔 d < 底板宽\n");
            for (int i = 0; i < HoleParamList.Count; i++)
            {
                if ((HoleParamList[i].ZDis + HoleParamList[i].HoleHeight) > BDun_Thickness)
                    sb.Append($"第{i}号空箱Z轴距离 + 高度 > 边墩高度\n");
                if ((HoleParamList[i].YDis + HoleParamList[i].HoleLength) > DB_Length)
                    sb.Append($"第{i}号空箱Y轴距离 + 长度 >  底板长度\n");
            }
            if (GS_Intervals.Count % 2 != 0)
            {
                sb.Append($"出水格栅的数组个数为{GS_Intervals.Count}，应为偶数\n");
            }
            ErrorInfo = sb.ToString();
            if (string.IsNullOrEmpty(ErrorInfo))
            {
                ErrorInfo = "所有参数均正确";
            }
        }

        [Command]
        public void DrawAll()
        {
            try
            {
                ComDrawAll();
                System.Windows.MessageBox.Show("参数化船闸绘制完成！\n放置在原点（0,0,0,）", "绘制完成", MessageBoxButton.OK, MessageBoxImage.Information);
                ErrorInfo = "参数化船闸绘制完成！";
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString() + "\n\n" + e.InnerException);
                return;
            }

        }
        public bool CanDrawAll()
        {
            return ErrorInfo == "所有参数均正确";
        }

        [Command]
        public void OutputQuantity()
        {
            try
            {
                var outputwindow = new Views.ExcelOutputWindow();
                ExcelOutputViewModel excelviewmodel = outputwindow.DataContext as ExcelOutputViewModel;
                if (excelviewmodel != null)
                {
                    excelviewmodel.Quantity = CreateContent();
                }
                outputwindow.ShowDialog();
                ErrorInfo = "闸首工程量统计完成！";
            }
            catch (Exception e)
            {
                ErrorInfo += "统计工程量出错，请检查当前的种子文件是否为3d，单位为mm\n";
                ErrorInfo += e.ToString() +"\n";
            }

        }
        public bool CanOutputQuantity()
        {
            return CanDrawAll();
        }

        [Command]
        public void Test()
        {
            app.ActiveModelReference.AddElement(app.SmartSolid.CreateChameferedBox(100, 100, 100, ChameferFace.XAxis,10));
        }
    }

}