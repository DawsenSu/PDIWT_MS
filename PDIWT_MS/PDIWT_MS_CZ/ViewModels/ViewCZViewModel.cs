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

using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;

using BM = Bentley.MstnPlatformNET;
using BD = Bentley.DgnPlatformNET;
using Bentley.Interop.MicroStationDGN;

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
            BDun_A = 8500; BDun_B = 10000; BDun_C = 2100; BDun_D = 16000; BDun_E = 2100; BDun_F = 2000;

            MK_Thickness = 4300;
            MK_A = 11500; MK_B = 2260; MK_C = 6650; MK_D = 10000; MK_E = 2248.2215; MK_F = 2754.7785;

            SSLD_Thickness = 3500; SSLD_YDis = 1500;
            SSLD_A = 6300; SSLD_B = 18200; SSLD_C = 2600; SSLD_D = 24500; SSLD_E = 4700; SSLD_F = 4600;
            SSLD_R1 = 0; SSLD_R2 = 0; SSLD_R3 = 1800; SSLD_R4 = 6400;

            HoleParamList = new ObservableCollection<HoleProperty>()
            {
                new HoleProperty { HoleHeight=8400,  HoleLength =4200, HoleWidth =6900, XDis=800, YDis=900, ZDis=300 },
                new HoleProperty { HoleHeight=2200,  HoleLength =6000, HoleWidth =4800, XDis=800, YDis=10900, ZDis=300 },
                new HoleProperty { HoleHeight=5400,  HoleLength =6000, HoleWidth =4800, XDis=800, YDis=10900, ZDis=3300 },
                new HoleProperty { HoleHeight=8400,  HoleLength =2700, HoleWidth =4800, XDis=800, YDis=17900, ZDis=300 },
                new HoleProperty { HoleHeight=8400,  HoleLength =5500, HoleWidth =4800, XDis=800, YDis=21600, ZDis=300 }
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
        public double BDun_D
        {
            get { return GetProperty(() => BDun_D); }
            set { SetProperty(() => BDun_D, value); }
        }
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
        #endregion

        #region MK Property
        public double MK_Thickness
        {
            get { return GetProperty(() => MK_Thickness); }
            set { SetProperty(() => MK_Thickness, value); }
        }
        public double MK_A
        {
            get { return GetProperty(() => MK_A); }
            set { SetProperty(() => MK_A, value); }
        }
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
        public double MK_D
        {
            get { return GetProperty(() => MK_D); }
            set { SetProperty(() => MK_D, value); }
        }
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

        #endregion

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

        [Command]
        public void DrawAll()
        {
            #region DrawDB
            SmartSolidElement ele_db = app.SmartSolid.CreateSlab(null, DB_Width / 2, DB_Length, DB_Thickness);
            Point3d eledboffset = app.Point3dFromXYZ(-DB_Width / 4, DB_Length / 2, DB_Thickness / 2);
            ele_db.Move(ref eledboffset);
            //app.ActiveModelReference.AddElement(ele_db);
            #endregion

            #region DrawBDun
            Point3d[] bddun_shapepoints =
                {
                    app.Point3dFromXY(0,0),
                    app.Point3dFromXY(BDun_A,0),
                    app.Point3dFromXY(BDun_A,BDun_B),
                    app.Point3dFromXY(BDun_A - BDun_C,BDun_B),
                    app.Point3dFromXY(BDun_A - BDun_C,BDun_B + BDun_D),
                    app.Point3dFromXY(BDun_A - BDun_C + BDun_E,BDun_B + BDun_D),
                    app.Point3dFromXY(BDun_A - BDun_C + BDun_E,BDun_B + BDun_D + BDun_F),
                    app.Point3dFromXY(0,BDun_B + BDun_D + BDun_F)
                };
            ShapeElement bdun_shape = app.CreateShapeElement1(null, ref bddun_shapepoints, MsdFillMode.Filled);
            Point3d bdun_shape_offset = app.Point3dFromXY(-DB_Width / 2, 0);
            bdun_shape.Move(ref bdun_shape_offset);
            SmartSolidElement ele_bdun = app.SmartSolid.ExtrudeClosedPlanarCurve(bdun_shape, BDun_Thickness, 0, true);
            //app.ActiveModelReference.AddElement(ele_bdun);
            #endregion

            #region DrawMK
            Point3d[] mk_shapepoints =
            {
                app.Point3dFromXY(0,0),
                app.Point3dFromXY(-MK_A,0),
                app.Point3dFromXY(-MK_A,MK_D),
                app.Point3dFromXY(-MK_A + MK_B,MK_D + MK_E),
                app.Point3dFromXY(-MK_A + MK_B +MK_C , MK_D + MK_E +MK_F),
                app.Point3dFromXY(0, MK_D + MK_E +MK_F)
            };
            ShapeElement mk_shape = app.CreateShapeElement1(null, ref mk_shapepoints, MsdFillMode.Filled);
            Point3d mk_shape_offset = app.Point3dFromXYZ(0, 0, DB_Thickness);
            mk_shape.Move(ref mk_shape_offset);
            SmartSolidElement ele_mk = app.SmartSolid.ExtrudeClosedPlanarCurve(mk_shape, 0, -MK_Thickness, true);
            //app.ActiveModelReference.AddElement(ele_mk);
            #endregion

            #region DrawSSLD
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
            //app.ActiveModelReference.AddElement(ele_ssld);
            #endregion

            #region DrawHole
            var ele_holeList = new List<SmartSolidElement>();
            SmartSolidElement ele_hole_temp;
            Point3d hole_offset_temp;
            Point3d BDun_left_down_up_Point = app.Point3dFromXYZ(-DB_Width / 2, 0, BDun_Thickness);
            foreach (var holeinfo in HoleParamList)
            {
                ele_hole_temp = app.SmartSolid.CreateSlab(null, holeinfo.HoleWidth, holeinfo.HoleLength, holeinfo.HoleHeight);
                hole_offset_temp = app.Point3dFromXYZ(holeinfo.HoleWidth / 2 - DB_Width / 2 + holeinfo.XDis,
                                                      holeinfo.HoleLength / 2 +holeinfo.YDis, 
                                                      -holeinfo.HoleHeight / 2 + BDun_Thickness -holeinfo.ZDis);
                ele_hole_temp.Move(ref hole_offset_temp);
                ele_holeList.Add(ele_hole_temp);
            }
            //Element[] allElements = ele_holeList.ToArray();
            //app.ActiveModelReference.AddElements(ref allElements);
            #endregion

            #region Sub\Union action
            SmartSolidElement cz, czLeft, czRigth;
            czLeft = app.SmartSolid.SolidUnion(ele_db, ele_bdun);
            czLeft = app.SmartSolid.SolidUnion(czLeft, ele_mk);
            czLeft = app.SmartSolid.SolidSubtract(czLeft, ele_ssld);
            foreach (var ele_hole in ele_holeList)
            {
                czLeft = app.SmartSolid.SolidSubtract(czLeft, ele_hole);
            }
            czRigth = czLeft.Clone().AsSmartSolidElement;
            Point3d cz_mirrorStart, cz_mirrorEnd;
            cz_mirrorStart = app.Point3dZero();
            cz_mirrorEnd = app.Point3dFromXY(0, 1);
            czRigth.Mirror(ref cz_mirrorStart, ref cz_mirrorEnd);
            cz = app.SmartSolid.SolidUnion(czLeft, czRigth);
            app.ActiveModelReference.AddElement(cz);
            #endregion
            
            MessageBox.Show("参数化船闸绘制完成！\n放置在原点（0,0,0,）", "绘制完成", MessageBoxButton.OK, MessageBoxImage.Information);
            ErrorInfo = "参数化船闸绘制完成！";
        }
        public bool CanDrawAll ()
        {
            return ErrorInfo == "所有参数均正确";
        }

        [Command]
        public void VertifyParam()
        {
            StringBuilder sb = new StringBuilder();
            if (DB_Length != (BDun_B + BDun_D + BDun_F))
                sb.Append("底板长度 != 边墩参数（b+d+f）\n");
            if (DB_Width / 2 != (BDun_A + MK_A))
                sb.Append("底板宽度 / 2 != 边墩 a + 门槛 a \n");
            if (MK_D != BDun_B)
                sb.Append("门槛参数 d != 边墩参数 b\n");
            if (SSLD_Thickness > MK_Thickness)
                sb.Append("疏水孔厚度 > 门槛厚度\n");
            if ((SSLD_YDis + SSLD_A) > MK_D)
                sb.Append("疏水孔Y轴距离 + 疏水孔 a > 门槛 d\n");
            if (SSLD_B > DB_Width / 2)
                sb.Append("疏水孔 b > 底板宽度 / 2\n");
            if ((SSLD_B - SSLD_C) < (MK_A + BDun_C))
                sb.Append("疏水孔 b-c < 门槛a + 边墩c\n");
            if ((SSLD_YDis + SSLD_D) > DB_Width)
                sb.Append("疏水孔Y轴距离 + 疏水孔 d < 底板宽\n");
            for (int i = 0; i < HoleParamList.Count; i++)
            {
                if ((HoleParamList[i].ZDis + HoleParamList[i].HoleHeight) > BDun_Thickness)
                    sb.Append($"第{i}号空箱Z轴距离 + 高度 > 边墩高度");
                if((HoleParamList[i].YDis + HoleParamList[i].HoleLength )> DB_Length)
                    sb.Append($"第{i}号空箱Y轴距离 + 长度 >  底板长度");
            }
            ErrorInfo = sb.ToString();
            if (string.IsNullOrEmpty(ErrorInfo))
            {
                ErrorInfo = "所有参数均正确";
            }
        }
    }

    public class HoleProperty
    {
        [Required]
        public double HoleLength { get; set; }
        [Required]
        public double HoleWidth { get; set; }
        [Required]
        public double HoleHeight { get; set; }
        [Required]
        public double XDis { get; set; }
        [Required]
        public double YDis { get; set; }
        [Required]
        public double ZDis { get; set; }
    }
}