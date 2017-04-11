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

namespace PDIWT_MS.CZ.ViewModels
{
    public class ViewModelAll : ViewModelBase
    {
        Bentley.Interop.MicroStationDGN.Application app = PDIWT_MS.Program.COM_App;

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();

            DB_Length = 40000; DB_Width = 28000; DB_Thickness = 3000;

            BDun_Thickness = 17200;
            BDun_A = 8500; BDun_B = 10000; BDun_C = 2100; BDun_D = 16000; BDun_E = 2100; BDun_F = 2000;

            MK_Thickness = 4300;
            MK_A = 11500; MK_B = 2260; MK_C = 6650; MK_D = 10000; MK_E = 2248.2215; MK_F = 2754.7785;

            SSLD_Thickness = 3500; SSLD_YDis = 1500;
            SSLD_A = 6300; SSLD_B = 18200; SSLD_C = 2600; SSLD_D = 24500; SSLD_E = 4700; SSLD_F = 4600;
            SSLD_R1 = 0; SSLD_R2 = 0; SSLD_R3 = 1800; SSLD_R4 = 6400;
        }

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
            //#region DrawDB
            //SmartSolidElement ele_db = app.SmartSolid.CreateSlab(null, DB_Length / 2 , DB_Width , DB_Thickness);
            //Point3d eledboffset = app.Point3dFromXYZ(-DB_Length / 4, DB_Width / 2, DB_Thickness / 2);
            //ele_db.Move(ref eledboffset);
            //app.ActiveModelReference.AddElement(ele_db);
            //#endregion

            //#region DrawBDun
            //Point3d[] bddun_shapepoints = 
            //    {
            //        app.Point3dFromXY(0,0),
            //        app.Point3dFromXY(BDun_A,0),
            //        app.Point3dFromXY(BDun_A,BDun_B),
            //        app.Point3dFromXY(BDun_A - BDun_C,BDun_B),
            //        app.Point3dFromXY(BDun_A - BDun_C,BDun_B + BDun_D),
            //        app.Point3dFromXY(BDun_A - BDun_C + BDun_E,BDun_B + BDun_D),
            //        app.Point3dFromXY(BDun_A - BDun_C + BDun_E,BDun_B + BDun_D + BDun_F),
            //        app.Point3dFromXY(0,BDun_B + BDun_D + BDun_F)
            //    };            
            //ShapeElement bdun_shape = app.CreateShapeElement1(null, ref bddun_shapepoints, MsdFillMode.Filled);
            //Point3d bdun_shape_offset = app.Point3dFromXY(-DB_Length / 2, 0);
            //bdun_shape.Move(ref bdun_shape_offset);
            //SmartSolidElement ele_bdun = app.SmartSolid.ExtrudeClosedPlanarCurve(bdun_shape, BDun_Thickness, 0, true);
            //app.ActiveModelReference.AddElement(ele_bdun);
            //#endregion

            //#region DrawMK
            //Point3d[] mk_shapepoints =
            //{
            //    app.Point3dFromXY(0,0),
            //    app.Point3dFromXY(-MK_A,0),
            //    app.Point3dFromXY(-MK_A,MK_D),
            //    app.Point3dFromXY(-MK_A + MK_B,MK_D + MK_E),
            //    app.Point3dFromXY(-MK_A + MK_B +MK_C , MK_D + MK_E +MK_F),
            //    app.Point3dFromXY(0, MK_D + MK_E +MK_F)
            //};
            //ShapeElement mk_shape = app.CreateShapeElement1(null, ref mk_shapepoints, MsdFillMode.Filled);
            //Point3d mk_shape_offset = app.Point3dFromXYZ(0, 0, DB_Thickness);
            //mk_shape.Move(ref mk_shape_offset);
            //SmartSolidElement ele_mk = app.SmartSolid.ExtrudeClosedPlanarCurve(mk_shape,0,-MK_Thickness,true);
            //app.ActiveModelReference.AddElement(ele_mk);
            //#endregion

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
            app.ActiveModelReference.AddElement(ele_ssld);
            #endregion

            MessageBox.Show("参数化船闸绘制完成！", "绘制完成", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}