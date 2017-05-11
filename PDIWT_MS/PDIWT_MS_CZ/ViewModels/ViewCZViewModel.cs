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
using System.Xml.Serialization;

using PDIWT_MS_CZ.Models;

using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;

using BM = Bentley.MstnPlatformNET;
using BD = Bentley.DgnPlatformNET;
using Bentley.Interop.MicroStationDGN;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace PDIWT_MS_CZ.ViewModels
{
    [XmlRoot(ElementName ="参数模板")]
    public class ViewCZViewModel : ViewModelBase
    {
        Bentley.Interop.MicroStationDGN.Application app = Program.COM_App;
        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();

            CurrentSSLDType = new Dictionary<SSLDType, string>
            {
                {SSLDType.Dispersed,"分散输水"},
                {SSLDType.Endfiling,"集中输水"}
            };
            SelectedSSLDType = CurrentSSLDType.First();

            DB_Length = 28000; DB_Width = 40000; DB_Thickness = 3000; DB_DoorWidth = 23000;
            IsIncludeDBRemoved = true;
            DB_Removed_Thinckness = 2000; DB_Removed_A = 8000; DB_Removed_B = 8000; DB_Removed_C = 11000; DB_Removed_N = 1;

            BDun_Thickness = 17200;
            /*BDun_A = 8500;*/ BDun_B = 10000; BDun_C = 2100;/* BDun_D = 16000;*/ BDun_E = 2100; BDun_F = 2000;
            IsIncludeBDunChamfer = true; BDun_Tx = 1387; BDun_Ty = 1387; BDun_R1 = 1000;

            MK_Thickness = 4300;
            /*MK_A = 11500;*/ MK_B = 2260; MK_C = 6650; /*MK_D = 10000;*/ MK_E = 2248.2215; MK_F = 2754.7785;

            SSLD_Thickness = 3500; SSLD_YDis = 1500;
            SSLD_A = 6300; SSLD_B = 18200; SSLD_C = 2600; SSLD_D = 24500; SSLD_E = 4700; SSLD_F = 4600;
            SSLD_R1 = 0; SSLD_R2 = 0; SSLD_R3 = 1800; SSLD_R4 = 6400;

            SSLD_Endfilling_XDis = 1800; SSLD_Endfilling_Width = 3500;
            SSLD_Endfilling_A = 2600; SSLD_Endfilling_B = 2000; SSLD_Endfilling_C = 16000; SSLD_Endfilling_D = 7000;

            IsIncludeDivisionPier = true;
            DivisionPier_R1 = 4400; DivisionPier_R2 = 4700; DivisionPier_R3 = 250; DivisionPier_A = 650; DivisionPier_B = 0;

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
            GS_MidWidth = 500;
            GS_IntervalList = new ObservableCollection<GSProperty>
            {
                new GSProperty { Interval=250,IntervalType="格栅间距" },
                new GSProperty { Interval=300,IntervalType="格栅宽度" },
                new GSProperty { Interval=500,IntervalType="格栅间距" },
                new GSProperty { Interval=300,IntervalType="格栅宽度" },
                new GSProperty { Interval=500,IntervalType="格栅间距" },
                new GSProperty { Interval=300,IntervalType="格栅宽度" },
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

            IsIncludeBaffle = true;
            BaffleList = new ObservableCollection<BaffleProperty>
            {
                new BaffleProperty { XDis = 0, Width = 250, Height =2000 },
                new BaffleProperty { XDis = 4500, Width = 500, Height =1500 },
                new BaffleProperty { XDis = 9000, Width = 500, Height =1000 }

            };
        }
        #region SLLDType
        [XmlIgnore]
        public Dictionary<SSLDType,string> CurrentSSLDType
        {
            get { return GetProperty(() => CurrentSSLDType); }
            set { SetProperty(() => CurrentSSLDType, value); }
        }
        public KeyValuePair<SSLDType,string> SelectedSSLDType
        {
            get { return GetProperty(() => SelectedSSLDType); }
            set { SetProperty(() => SelectedSSLDType, value); }
        }
        
        #endregion

        #region VertifyProperty
        [XmlIgnore]
        public string ErrorInfo
        {
            get { return GetProperty(() => ErrorInfo); }
            set { SetProperty(() => ErrorInfo, value); }
        }

        #endregion

        #region DB Property
        [XmlElement(ElementName = "底板长")]
        public double DB_Length
        {
            get { return GetProperty(() => DB_Length); }
            set { SetProperty(() => DB_Length, value); }
        }
        [XmlElement(ElementName = "底板宽")]
        public double DB_Width
        {
            get { return GetProperty(() => DB_Width); }
            set { SetProperty(() => DB_Width, value); }
        }
        [XmlElement(ElementName = "底板高")]
        public double DB_Thickness
        {
            get { return GetProperty(() => DB_Thickness); }
            set { SetProperty(() => DB_Thickness, value); }
        }
        [XmlElement(ElementName ="口门宽度")]
        public double DB_DoorWidth
        {
            get { return GetProperty(() => DB_DoorWidth); }
            set { SetProperty(() => DB_DoorWidth, value); }
        }

        [XmlElement(ElementName ="底板是否有切槽")]

        public bool IsIncludeDBRemoved
        {
            get { return GetProperty(() => IsIncludeDBRemoved); }
            set { SetProperty(() => IsIncludeDBRemoved, value); }
        }
        [XmlElement(ElementName ="切槽高度")]
        public double DB_Removed_Thinckness
        {
            get { return GetProperty(() => DB_Removed_Thinckness); }
            set { SetProperty(() => DB_Removed_Thinckness, value); }
        }
        [XmlElement(ElementName ="切槽参数A")]
        public double DB_Removed_A
        {
            get { return GetProperty(() => DB_Removed_A); }
            set { SetProperty(() => DB_Removed_A, value); }
        }
        [XmlElement(ElementName ="切槽参数B")]
        public double DB_Removed_B
        {
            get { return GetProperty(() => DB_Removed_B); }
            set { SetProperty(() => DB_Removed_B, value); }
        }
        [XmlElement(ElementName ="切槽参数C")]
        public double DB_Removed_C
        {
            get { return GetProperty(() => DB_Removed_C); }
            set { SetProperty(() => DB_Removed_C, value); }
        }
        [XmlElement(ElementName ="切槽坡度N")]
        public double DB_Removed_N
        {
            get { return GetProperty(() => DB_Removed_N); }
            set { SetProperty(() => DB_Removed_N, value); }
        }
        #endregion

        #region BDun Property
        [XmlElement(ElementName = "边墩高")]
        public double BDun_Thickness
        {
            get { return GetProperty(() => BDun_Thickness); }
            set { SetProperty(() => BDun_Thickness, value); }
        }
        //[XmlElement(ElementName = "边墩参数A")]
        //public double BDun_A
        //{
        //    get { return GetProperty(() => BDun_A); }
        //    set { SetProperty(() => BDun_A, value); }
        //}
        [XmlElement(ElementName = "边墩参数B")]
        public double BDun_B
        {
            get { return GetProperty(() => BDun_B); }
            set { SetProperty(() => BDun_B, value); }
        }
        [XmlElement(ElementName = "边墩参数C")]
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
        [XmlElement(ElementName = "边墩参数E")]
        public double BDun_E
        {
            get { return GetProperty(() => BDun_E); }
            set { SetProperty(() => BDun_E, value); }
        }
        [XmlElement(ElementName = "边墩参数F")]
        public double BDun_F
        {
            get { return GetProperty(() => BDun_F); }
            set { SetProperty(() => BDun_F, value); }
        }
        [XmlElement(ElementName = "边墩参数Tx")]
        public double BDun_Tx
        {
            get { return GetProperty(() => BDun_Tx); }
            set { SetProperty(() => BDun_Tx, value); }
        }
        [XmlElement(ElementName = "边墩参数Ty")]
        public double BDun_Ty
        {
            get { return GetProperty(() => BDun_Ty); }
            set { SetProperty(() => BDun_Ty, value); }
        }
        [XmlElement(ElementName = "边墩参数R1")]
        public double BDun_R1
        {
            get { return GetProperty(() => BDun_R1); }
            set { SetProperty(() => BDun_R1, value); }
        }
        [XmlElement(ElementName = "边墩是否包含倒角")]
        public bool IsIncludeBDunChamfer
        {
            get { return GetProperty(() => IsIncludeBDunChamfer); }
            set { SetProperty(() => IsIncludeBDunChamfer, value); }
        }

        #endregion

        #region MK Property
        [XmlElement(ElementName = "门槛高")]
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
        [XmlElement(ElementName = "门槛参数B")]
        public double MK_B
        {
            get { return GetProperty(() => MK_B); }
            set { SetProperty(() => MK_B, value); }
        }
        [XmlElement(ElementName = "门槛参数C")]
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
        [XmlElement(ElementName = "门槛参数E")]
        public double MK_E
        {
            get { return GetProperty(() => MK_E); }
            set { SetProperty(() => MK_E, value); }
        }
        [XmlElement(ElementName = "门槛参数F")]
        public double MK_F
        {
            get { return GetProperty(() => MK_F); }
            set { SetProperty(() => MK_F, value); }
        }
        #endregion

        #region SSLD_dispersed Property
        [XmlElement(ElementName = "输水廊道高")]
        public double SSLD_Thickness
        {
            get { return GetProperty(() => SSLD_Thickness); }
            set { SetProperty(() => SSLD_Thickness, value); }
        }
        [XmlElement(ElementName = "输水廊道边距的Y坐标")]
        public double SSLD_YDis
        {
            get { return GetProperty(() => SSLD_YDis); }
            set { SetProperty(() => SSLD_YDis, value); }
        }
        [XmlElement(ElementName = "输水廊道参数A")]
        public double SSLD_A
        {
            get { return GetProperty(() => SSLD_A); }
            set { SetProperty(() => SSLD_A, value); }
        }
        [XmlElement(ElementName = "输水廊道参数B")]
        public double SSLD_B
        {
            get { return GetProperty(() => SSLD_B); }
            set { SetProperty(() => SSLD_B, value); }
        }
        [XmlElement(ElementName = "输水廊道参数C")]
        public double SSLD_C
        {
            get { return GetProperty(() => SSLD_C); }
            set { SetProperty(() => SSLD_C, value); }
        }
        [XmlElement(ElementName = "输水廊道参数D")]
        public double SSLD_D
        {
            get { return GetProperty(() => SSLD_D); }
            set { SetProperty(() => SSLD_D, value); }
        }
        [XmlElement(ElementName = "输水廊道参数E")]
        public double SSLD_E
        {
            get { return GetProperty(() => SSLD_E); }
            set { SetProperty(() => SSLD_E, value); }
        }
        [XmlElement(ElementName = "输水廊道参数F")]
        public double SSLD_F
        {
            get { return GetProperty(() => SSLD_F); }
            set { SetProperty(() => SSLD_F, value); }
        }
        [XmlElement(ElementName = "输水廊道参数R1")]
        public double SSLD_R1
        {
            get { return GetProperty(() => SSLD_R1); }
            set { SetProperty(() => SSLD_R1, value); }
        }
        [XmlElement(ElementName = "输水廊道参数R2")]
        public double SSLD_R2
        {
            get { return GetProperty(() => SSLD_R2); }
            set { SetProperty(() => SSLD_R2, value); }
        }
        [XmlElement(ElementName = "输水廊道参数R3")]
        public double SSLD_R3
        {
            get { return GetProperty(() => SSLD_R3); }
            set { SetProperty(() => SSLD_R3, value); }
        }
        [XmlElement(ElementName = "输水廊道参数R4")]
        public double SSLD_R4
        {
            get { return GetProperty(() => SSLD_R4); }
            set { SetProperty(() => SSLD_R4, value); }
        }
        #endregion

        #region DivisionPier
        [XmlElement(ElementName = "是否包含分水墩")]
        public bool IsIncludeDivisionPier
        {
            get { return GetProperty(() => IsIncludeDivisionPier); }
            set { SetProperty(() => IsIncludeDivisionPier, value); }
        }
        [XmlElement(ElementName = "分水墩参数R1")]
        public double DivisionPier_R1
        {
            get { return GetProperty(() => DivisionPier_R1); }
            set { SetProperty(() => DivisionPier_R1, value); }
        }
        [XmlElement(ElementName = "分水墩参数R2")]
        public double DivisionPier_R2
        {
            get { return GetProperty(() => DivisionPier_R2); }
            set { SetProperty(() => DivisionPier_R2, value); }
        }
        [XmlElement(ElementName = "分水墩参数R3")]
        public double DivisionPier_R3
        {
            get { return GetProperty(() => DivisionPier_R3); }
            set { SetProperty(() => DivisionPier_R3, value); }
        }
        [XmlElement(ElementName = "分水墩参数A")]
        public double DivisionPier_A
        {
            get { return GetProperty(() => DivisionPier_A); }
            set { SetProperty(() => DivisionPier_A, value); }
        }
        [XmlElement(ElementName = "分水墩参数B")]
        public double DivisionPier_B
        {
            get { return GetProperty(() => DivisionPier_B); }
            set { SetProperty(() => DivisionPier_B, value); }
        }
        #endregion

        #region SSLD_Endfilling Property

        public double SSLD_Endfilling_XDis
        {
            get { return GetProperty(() => SSLD_Endfilling_XDis); }
            set { SetProperty(() => SSLD_Endfilling_XDis, value); }
        }
        //public double SSLD_Endfilling_Thickness
        //{
        //    get { return GetProperty(() => SSLD_Endfilling_Thickness); }
        //    set { SetProperty(() => SSLD_Endfilling_Thickness, value); }
        //}
        public double SSLD_Endfilling_Width
        {
            get { return GetProperty(() => SSLD_Endfilling_Width); }
            set { SetProperty(() => SSLD_Endfilling_Width, value); }
        }
        public double SSLD_Endfilling_A
        {
            get { return GetProperty(() => SSLD_Endfilling_A); }
            set { SetProperty(() => SSLD_Endfilling_A, value); }
        }
        public double SSLD_Endfilling_B
        {
            get { return GetProperty(() => SSLD_Endfilling_B); }
            set { SetProperty(() => SSLD_Endfilling_B, value); }
        }
        public double SSLD_Endfilling_C
        {
            get { return GetProperty(() => SSLD_Endfilling_C); }
            set { SetProperty(() => SSLD_Endfilling_C, value); }
        }
        public double SSLD_Endfilling_D
        {
            get { return GetProperty(() => SSLD_Endfilling_D); }
            set { SetProperty(() => SSLD_Endfilling_D, value); }
        }
        #endregion

        #region Hole Property
        [XmlArray(ElementName = "空箱参数集合")]
        public ObservableCollection<HoleProperty> HoleParamList
        {
            get { return GetProperty(() => HoleParamList); }
            set { SetProperty(() => HoleParamList, value); }
        }
        [XmlElement(ElementName = "是否包含切角空箱")]
        public bool IsIncludeTrapHole
        {
            get { return GetProperty(() => IsIncludeTrapHole); }
            set { SetProperty(() => IsIncludeTrapHole, value); }
        }
        [XmlElement(ElementName = "切角空箱X方向长度")]
        public double TrapHoleXLength
        {
            get { return GetProperty(() => TrapHoleXLength); }
            set { SetProperty(() => TrapHoleXLength, value); }
        }
        [XmlElement(ElementName = "切角空箱Y方向长度")]
        public double TrapHoleYLength
        {
            get { return GetProperty(() => TrapHoleYLength); }
            set { SetProperty(() => TrapHoleYLength, value); }
        }
        [XmlElement(ElementName = "切角空箱X方向切角长度")]
        public double TrapHoleXLengthCorner
        {
            get { return GetProperty(() => TrapHoleXLengthCorner); }
            set { SetProperty(() => TrapHoleXLengthCorner, value); }
        }
        [XmlElement(ElementName = "切角空箱Y方向切角长度")]
        public double TrapHoleYLengthCorner
        {
            get { return GetProperty(() => TrapHoleYLengthCorner); }
            set { SetProperty(() => TrapHoleYLengthCorner, value); }
        }
        [XmlElement(ElementName = "切角空箱距边墩角的X距离")]
        public double TrapHoleXDis
        {
            get { return GetProperty(() => TrapHoleXDis); }
            set { SetProperty(() => TrapHoleXDis, value); }
        }
        [XmlElement(ElementName = "切角空箱距边墩角的Y距离")]
        public double TrapHoleYDis
        {
            get { return GetProperty(() => TrapHoleYDis); }
            set { SetProperty(() => TrapHoleYDis, value); }
        }
        [XmlElement(ElementName = "切角空箱距边墩角的Z距离")]
        public double TrapHoleZDis
        {
            get { return GetProperty(() => TrapHoleZDis); }
            set { SetProperty(() => TrapHoleZDis, value); }
        }
        [XmlElement(ElementName = "切角空箱高度")]
        public double TrapHoleThickness
        {
            get { return GetProperty(() => TrapHoleThickness); }
            set { SetProperty(() => TrapHoleThickness, value); }
        }
        #endregion

        #region GS Property

        [XmlElement(ElementName = "是否包含出水格栅")]
        public bool IsIncludeGs
        {
            get { return GetProperty(() => IsIncludeGs); }
            set { SetProperty(() => IsIncludeGs, value); }
        }
        [XmlElement(ElementName = "出水格栅中间肋板宽度")]
        public double GS_MidWidth
        {
            get { return GetProperty(() => GS_MidWidth); }
            set { SetProperty(() => GS_MidWidth, value); }
        }
        [XmlArray(ElementName = "出水格栅尺寸集合")]
        public ObservableCollection<GSProperty> GS_IntervalList
        {
            get { return GetProperty(() => GS_IntervalList); }
            set { SetProperty(() => GS_IntervalList, value); }
        }
        #endregion

        #region Baffle Property
        [XmlElement(ElementName = "是否包含消力坎")]
        public bool IsIncludeBaffle
        {
            get { return GetProperty(() => IsIncludeBaffle); }
            set { SetProperty(() => IsIncludeBaffle, value); }
        }
        [XmlArray(ElementName = "消力坎参数集合")]
        public ObservableCollection<BaffleProperty> BaffleList
        {
            get { return GetProperty(() => BaffleList); }
            set { SetProperty(() => BaffleList, value); }
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

        public double GetBDun_A()
        {
            return (DB_Width - DB_DoorWidth) / 2.0;
        }
        public double GetBDun_D()
        {
            return DB_Length - BDun_B - BDun_F;
        }
        public double GetMK_A()
        {
            return DB_DoorWidth/2;
        }
        public double GetMK_D()
        {
            return BDun_B;
        }

        SmartSolidElement GetDB()
        {
            SmartSolidElement ele_db = app.SmartSolid.CreateSlab(null, DB_Width / 2, DB_Length, DB_Thickness);
            Point3d eledboffset = app.Point3dFromXYZ(-DB_Width / 4, DB_Length / 2, DB_Thickness / 2);
            ele_db.Move(ref eledboffset);
            //if (IsIncludeDBRemoved)
            //{
            //    Point3d[] bdremovedpoints =
            //    {
            //        app.Point3dFromXY(0,0),
            //        app.Point3dFromXY(0,DB_Removed_A+DB_Removed_B),
            //        app.Point3dFromXY(-DB_Width/2,DB_Removed_A+DB_Removed_B),
            //        app.Point3dFromXY(-DB_Width/2,DB_Removed_B),
            //        app.Point3dFromXY(-DB_Removed_C,DB_Removed_B),
            //        app.Point3dFromXY(-DB_Removed_C,0)
            //    };
            //    ShapeElement db_removed_shape = app.CreateShapeElement1(null, ref bdremovedpoints, MsdFillMode.Filled);
            //    SmartSolidElement ele_db_removed = app.SmartSolid.ExtrudeClosedPlanarCurve(db_removed_shape, DB_Removed_Thinckness, 0, true);
            //    Point3d[] dbremovededgemidpoints =
            //    {
            //        app.Point3dFromXYZ(-DB_Removed_C/2,0,DB_Removed_Thinckness),
            //        app.Point3dFromXYZ(-DB_Removed_C,(DB_Removed_B+DB_Removed_Thinckness/DB_Removed_N)/2,DB_Removed_Thinckness),
            //        app.Point3dFromXYZ(-(DB_Width/2+DB_Removed_C)/2,DB_Removed_B,DB_Removed_Thinckness)
            //    };
            //    for (int i = 0; i < dbremovededgemidpoints.Length; i++)
            //        ele_db_removed = app.SmartSolid.ChamferEdge(ele_db_removed, ref dbremovededgemidpoints[i], DB_Removed_Thinckness, DB_Removed_Thinckness / DB_Removed_N, true);

            //    Point3d db_removed_offset = app.Point3dFromXY(0, DB_Length - DB_Removed_A - DB_Removed_B);
            //    ele_db_removed.Move(ref db_removed_offset);
            //    ele_db = app.SmartSolid.SolidSubtract(ele_db, ele_db_removed);
            //}
            return ele_db;
        }
        SmartSolidElement GetDBRemoved()
        {
            Point3d[] bdremovedpoints =
                {
                    app.Point3dFromXY(0,0),
                    app.Point3dFromXY(0,DB_Removed_A+DB_Removed_B),
                    app.Point3dFromXY(-DB_Width/2,DB_Removed_A+DB_Removed_B),
                    app.Point3dFromXY(-DB_Width/2,DB_Removed_B),
                    app.Point3dFromXY(-DB_Removed_C,DB_Removed_B),
                    app.Point3dFromXY(-DB_Removed_C,0)
                };
            ShapeElement db_removed_shape = app.CreateShapeElement1(null, ref bdremovedpoints, MsdFillMode.Filled);
            SmartSolidElement ele_db_removed = app.SmartSolid.ExtrudeClosedPlanarCurve(db_removed_shape, DB_Removed_Thinckness, 0, true);
            Point3d[] dbremovededgemidpoints =
            {
                    app.Point3dFromXYZ(-DB_Removed_C/2,0,DB_Removed_Thinckness),
                    app.Point3dFromXYZ(-DB_Removed_C,(DB_Removed_B+DB_Removed_Thinckness/DB_Removed_N)/2,DB_Removed_Thinckness),
                    app.Point3dFromXYZ(-(DB_Width/2+DB_Removed_C)/2,DB_Removed_B,DB_Removed_Thinckness)
            };
            for (int i = 0; i < dbremovededgemidpoints.Length; i++)
                ele_db_removed = app.SmartSolid.ChamferEdge(ele_db_removed, ref dbremovededgemidpoints[i], DB_Removed_Thinckness, DB_Removed_Thinckness / DB_Removed_N, true);

            Point3d db_removed_offset = app.Point3dFromXY(0, DB_Length - DB_Removed_A - DB_Removed_B);
            ele_db_removed.Move(ref db_removed_offset);
            return ele_db_removed;
        }
        SmartSolidElement GetBDun()
        {
            double bdun_a = GetBDun_A();
            double bdun_d = GetBDun_D();
            Point3d[] bdun_shapepoints =
                {
                    app.Point3dFromXY(0,0),
                    app.Point3dFromXY(bdun_a,0),
                    app.Point3dFromXY(bdun_a,BDun_B),
                    app.Point3dFromXY(bdun_a - BDun_C,BDun_B),
                    app.Point3dFromXY(bdun_a - BDun_C,BDun_B + bdun_d),
                    app.Point3dFromXY(bdun_a - BDun_C + BDun_E,BDun_B + bdun_d),
                    app.Point3dFromXY(bdun_a - BDun_C + BDun_E,BDun_B + bdun_d + BDun_F),
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
        SmartSolidElement GetSSLD_Endfilling()
        {
            Point3d[] slld2points =
            {
                app.Point3dFromXYZ(0,0,0),
                app.Point3dFromXYZ(0,SSLD_Endfilling_C,0),
                app.Point3dFromXYZ(0,DB_Length-SSLD_Endfilling_D,SSLD_Endfilling_B),
                app.Point3dFromXYZ(0,DB_Length,SSLD_Endfilling_B)
            };
            ChainableElement[] linestringele = new ChainableElement[4];
            Point3d[] pointline1 =
            {
                app.Point3dFromXYZ(0,SSLD_Endfilling_C,SSLD_Endfilling_A),
                app.Point3dFromXYZ(0,0,SSLD_Endfilling_A),
                slld2points[0],
                slld2points[1]
            };
            linestringele[0] = app.CreateLineElement1(null, ref pointline1);
            BsplineCurve bscurve = new BsplineCurveClass(); //利用bsplinecurveclass获得bsplinecurve的实例引用
            Point3d[] bscurvepolepoints = 
            {
                slld2points[1],
                app.Point3dFromXYZ(0, SSLD_Endfilling_C+SSLD_Endfilling_B/2, 0),
                app.Point3dFromXYZ(0, DB_Length - SSLD_Endfilling_D-SSLD_Endfilling_B/2, SSLD_Endfilling_B),
                slld2points[2]
            };
            bscurve.SetPoles(ref bscurvepolepoints);
            linestringele[1] = app.CreateBsplineCurveElement1(null, bscurve);
            Point3d[] pointline2 =
            {
                slld2points[2],
                slld2points[3],
                app.Point3dFromXYZ(0,DB_Length,SSLD_Endfilling_B+SSLD_Endfilling_A),
                app.Point3dFromXYZ(0,DB_Length-SSLD_Endfilling_D,SSLD_Endfilling_B+SSLD_Endfilling_A)
            };
            linestringele[2] = app.CreateLineElement1(null, ref pointline2);
            Point3d[] reversebscurvepoints =
            {
                app.Point3dFromXYZ(0,DB_Length-SSLD_Endfilling_D,SSLD_Endfilling_B+SSLD_Endfilling_A),
                app.Point3dFromXYZ(0, DB_Length - SSLD_Endfilling_D-SSLD_Endfilling_B/2, SSLD_Endfilling_B+SSLD_Endfilling_A),
                app.Point3dFromXYZ(0, SSLD_Endfilling_C+SSLD_Endfilling_B/2, SSLD_Endfilling_A),
                app.Point3dFromXYZ(0,SSLD_Endfilling_C,SSLD_Endfilling_A),
            };
            BsplineCurve rebscurve = new BsplineCurveClass();
            rebscurve.SetPoles(ref reversebscurvepoints);
            linestringele[3] = app.CreateBsplineCurveElement1(null, rebscurve);
            ComplexShapeElement complexshapeele = app.CreateComplexShapeElement1(ref linestringele, MsdFillMode.Filled);
            SmartSolidElement ele = app.SmartSolid.ExtrudeClosedPlanarCurve(complexshapeele, SSLD_Endfilling_Width, 0, true);
            Point3d ele_offset = app.Point3dFromXYZ(-DB_Width / 2 + SSLD_Endfilling_XDis, 0, DB_Thickness);
            ele.Move(ref ele_offset);
            return ele;
        }
        SmartSolidElement GetDivisionPier()
        {
            var elemList = new List<ChainableElement>();
            Point3d[] pntArray =
            {
                app.Point3dFromXY(0,0),
                app.Point3dFromXY(0,2*DivisionPier_R3),
                app.Point3dFromXY(-DivisionPier_A, 2*DivisionPier_R3),
                app.Point3dFromXY(-DivisionPier_A - DivisionPier_R1, 2*DivisionPier_R3 + DivisionPier_R1),
                app.Point3dFromXY(-DivisionPier_A - DivisionPier_R1, 2*DivisionPier_R3 + DivisionPier_R1 + DivisionPier_B),
                app.Point3dFromXY(-DivisionPier_A - DivisionPier_R2, 2*DivisionPier_R3 + DivisionPier_R1 + DivisionPier_B),
                app.Point3dFromXY(-DivisionPier_A - DivisionPier_R2, DivisionPier_R2),
                app.Point3dFromXY(-DivisionPier_A,0)
            };
            Point3d[] arccenterpntArray =
            {
                app.Point3dFromXY(DivisionPier_R3,DivisionPier_R3),
                app.Point3dFromXY(-DivisionPier_A, 2* DivisionPier_R3 + DivisionPier_R1),
                app.Point3dFromXY(-(DivisionPier_A+(DivisionPier_R1+DivisionPier_R2)/2.0),2*DivisionPier_R3 + DivisionPier_R1 + DivisionPier_B + Math.Abs(DivisionPier_R2-DivisionPier_R1)/2),
                app.Point3dFromXY(-DivisionPier_A, DivisionPier_R2)
            };
            elemList.Add(app.CreateArcElement3(null, ref pntArray[0], ref arccenterpntArray[0], ref pntArray[1]));
            elemList.Add(app.CreateLineElement2(null, ref pntArray[1], ref pntArray[2]));
            var starttangent = new Ray3d();
            starttangent.Origin = pntArray[2];
            starttangent.Direction = app.Point3dFromXY(-100, 0);
            //elemList.Add(app.CreateArcElement4(null, ref pntArray[2], ref arccenterpntArray[1], ref pntArray[3]));
            elemList.Add(app.CreateArcElement4(null, ref starttangent, ref pntArray[3]));
            elemList.Add(app.CreateLineElement2(null, ref pntArray[3], ref pntArray[4]));
            elemList.Add(app.CreateArcElement3(null, ref pntArray[4], ref arccenterpntArray[2], ref pntArray[5]));
            elemList.Add(app.CreateLineElement2(null, ref pntArray[5], ref pntArray[6]));
            elemList.Add(app.CreateArcElement1(null, ref pntArray[6], ref arccenterpntArray[3], ref pntArray[7]));
            elemList.Add(app.CreateLineElement2(null, ref pntArray[7], ref pntArray[0]));
            ChainableElement[] elemArray = elemList.ToArray();
            ComplexShapeElement complexshape = app.CreateComplexShapeElement1(ref elemArray, MsdFillMode.Filled);
            Point3d move_offset = app.Point3dFromXYZ(-GetMK_A() - DivisionPier_R3, SSLD_YDis + SSLD_A / 2 - DivisionPier_R3, DB_Thickness);
            SmartSolidElement ele = app.SmartSolid.ExtrudeClosedPlanarCurve(complexshape, SSLD_Thickness, 0, true);
            ele.Move(move_offset);
            return ele;
            //return complexshape ;
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
            foreach (var gs_prop in GS_IntervalList)
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
                for (int i = 0; i < GS_IntervalList.Count; i++)
                {
                    if (i % 2 == 1)
                    {
                        ele_gs_temp = GetChamferedBox(GS_IntervalList[i].Interval, gs_length, gs_heigth);
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
        List<SmartSolidElement> GetBaffleList()
        {
            var bafflelist = new List<SmartSolidElement>();

            SmartSolidElement tempele = null;
            Point3d ele_offset;
            foreach (var baffleprop in BaffleList)
            {
                tempele = app.SmartSolid.CreateSlab(null, baffleprop.Width, SSLD_A, baffleprop.Height);
                ele_offset = app.Point3dFromXYZ(-baffleprop.Width / 2 - baffleprop.XDis, SSLD_YDis + SSLD_A / 2, DB_Thickness + baffleprop.Height / 2);
                tempele.Move(ref ele_offset);
                bafflelist.Add(tempele);
            }

            return bafflelist;
        }

        SmartSolidElement ComDrawAll()
        {
            SmartSolidElement ele_db = GetDB();
            SmartSolidElement ele_db_removed = null;
            if(IsIncludeDBRemoved)
                ele_db_removed = GetDBRemoved();
            SmartSolidElement ele_bdun = GetBDun();
            SmartSolidElement ele_mk = GetMK();
            SmartSolidElement ele_ssld = null;
            switch (SelectedSSLDType.Key)
            {
                case SSLDType.Dispersed:
                    ele_ssld = GetSSLD();
                    break;
                case SSLDType.Endfiling:
                    ele_ssld = GetSSLD_Endfilling();
                    break;
            }
            SmartSolidElement ele_divisionpier = null;
            if (IsIncludeDivisionPier)
                ele_divisionpier = GetDivisionPier();
            List<SmartSolidElement> ele_holeList = GetHoleList();
            List<SmartSolidElement> ele_gsList = GetGSList();
            List<SmartSolidElement> ele_baffleList = new List<SmartSolidElement>();
            if (IsIncludeBaffle)
                ele_baffleList = GetBaffleList();
            
            #region Sub\Union action
            SmartSolidElement cz, czLeft, czRigth;
            czLeft = app.SmartSolid.SolidUnion(ele_db, ele_bdun);
            czLeft = app.SmartSolid.SolidUnion(czLeft, ele_mk);
            if(ele_ssld!=null)
                czLeft = app.SmartSolid.SolidSubtract(czLeft, ele_ssld);
            if ((ele_divisionpier != null) && (SelectedSSLDType.Key == SSLDType.Dispersed))
                czLeft = app.SmartSolid.SolidUnion(czLeft, ele_divisionpier);
            foreach (var ele_hole in ele_holeList)
                czLeft = app.SmartSolid.SolidSubtract(czLeft, ele_hole);
            //分散输水才会有格栅和消力坎
            if (SelectedSSLDType.Key == SSLDType.Dispersed)
            {
                foreach (var ele_gs in ele_gsList)
                    czLeft = app.SmartSolid.SolidSubtract(czLeft, ele_gs);
                foreach (var ele_baffle in ele_baffleList)
                    czLeft = app.SmartSolid.SolidUnion(czLeft, ele_baffle);
            }

            if (ele_db_removed!= null)
                czLeft = app.SmartSolid.SolidSubtract(czLeft, ele_db_removed);

                

            czRigth = czLeft.Clone().AsSmartSolidElement;
            Point3d cz_mirrorStart, cz_mirrorEnd;
            cz_mirrorStart = app.Point3dZero();
            cz_mirrorEnd = app.Point3dFromXY(0, 1);
            czRigth.Mirror(ref cz_mirrorStart, ref cz_mirrorEnd);
            cz = app.SmartSolid.SolidUnion(czLeft, czRigth);
            return cz;
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
            dbunedgelength += 2 * (GetBDun_A() + BDun_B + GetBDun_D() + BDun_E + BDun_F);
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
            if (DB_Removed_A + DB_Removed_B > DB_Length)
                sb.Append("底板切槽参数a+b大于底板长度");
            if (DB_Removed_C > DB_Width / 2)
                sb.Append("底板切槽参数c大于1/2底板宽度");
            if (DB_Removed_Thinckness > DB_Thickness)
                sb.Append("底板切槽厚度大于底板厚度");

            if (SSLD_Thickness > MK_Thickness)
                sb.Append("输水廊道厚度 > 门槛厚度\n");
            if ((SSLD_YDis + SSLD_A) > GetMK_D())
                sb.Append("输水廊道Y轴距离 + 输水廊道 a > 门槛 d\n");
            if (SSLD_B > DB_Width / 2)
                sb.Append("输水廊道 b > 底板宽度 / 2\n");
            if ((SSLD_B - SSLD_C) < (GetMK_A() + BDun_C))
                sb.Append("输水廊道 b-c < 门槛a + 边墩c\n");
            if ((SSLD_YDis + SSLD_D) > DB_Width)
                sb.Append("输水廊道Y轴距离 + 输水廊道孔 d < 底板宽\n");

            if ((SSLD_Endfilling_C + SSLD_Endfilling_D) > DB_Length)
                sb.Append("输水廊道参数C+D大于底板长度");
            if ((SSLD_Endfilling_A + SSLD_Endfilling_B) > (BDun_Thickness - DB_Thickness))
                sb.Append("输水廊道参数A+B大于（边墩高度-底板高度）");
            if ((SSLD_Endfilling_Width + SSLD_Endfilling_XDis) > (GetBDun_A() - BDun_C))
                sb.Append("输水廊道（宽+XDis）>边墩（A-C）");
            if (DivisionPier_R2 < DivisionPier_R1)
                sb.Append("分流墩参数R1 > R2 \n");
            for (int i = 0; i < HoleParamList.Count; i++)
            {
                if ((HoleParamList[i].ZDis + HoleParamList[i].HoleHeight) > BDun_Thickness)
                    sb.Append($"第{i}号空箱Z轴距离 + 高度 > 边墩高度\n");
                if ((HoleParamList[i].YDis + HoleParamList[i].HoleLength) > DB_Length)
                    sb.Append($"第{i}号空箱Y轴距离 + 长度 >  底板长度\n");
            }
            var intervallist = from gs in GS_IntervalList
                               select gs.Interval;
            if (intervallist.Sum() > GetMK_A())
                sb.Append($"出水格栅数列的总长{intervallist.Sum()}大于{GetMK_A()}\n");

            if (GS_IntervalList.Count % 2 != 0)
                sb.Append($"出水格栅的数组个数为{GS_IntervalList.Count}，应为偶数\n");

            for (int i = 0; i < BaffleList.Count; i++)
            {
                if (BaffleList[i].Height > SSLD_Thickness)
                {
                    sb.Append($"第{i + 1}号消力坎高度{BaffleList[i].Height}大于输水廊道{SSLD_Thickness}高度\n");
                }
            }
            ErrorInfo = sb.ToString();
            if (string.IsNullOrEmpty(ErrorInfo))
                ErrorInfo = "所有参数均正确";
        }

        [Command]
        public void PreviewDraw()
        {
            var previewview = new Views.Preview();
            var previewmodel = new PreviewViewModel();
            previewmodel.CZ = ComDrawAll();
            previewview.DataContext = previewmodel;
            previewview.ShowDialog();
        }
        public bool CanPreviewDraw()
        {
            return CanDrawAll();
        }

        [Command]
        public void DrawAll()
        {
            try
            {
                SmartSolidElement czele = ComDrawAll();
                app.ActiveModelReference.AddElement(czele);
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

        public ImageSource TestImage
        {
            get { return GetProperty(() => TestImage); }
            set { SetProperty(() => TestImage, value); }
        }
        [Command]
        public void Test()
        {
            SmartSolidElement testele = GetDivisionPier();
            Point3d origin = app.Point3dZero();
            testele.Rotate(ref origin, Math.PI / 4, Math.PI / 4, Math.PI / 4);
            IntPtr metafilehandle = new IntPtr(testele.DrawToEnhancedMetafile(200, 200, true));
            Bitmap bitmap = new Bitmap(new System.Drawing.Imaging.Metafile(metafilehandle, true));
            TestImage = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        [Command]
        public void ExportTemplate()
        {
            try
            {
                System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
                sfd.Title = "输入人字门闸首参数模板的保存文件名称";
                sfd.Filter = "XML文件|*.xml";
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string fileName = sfd.FileName;
                    if (File.Exists(fileName))
                        File.Delete(fileName);
                    XmlSerializerHelper.SaveToXml(fileName, this, null, null);
                    ErrorInfo = "模板参数保存至" + fileName + "\n参数模板导出成功!";
                    //MessageBox.Show("模板参数保存至"+fileName, "参数模板导出成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "参数模板输出错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public bool CanExportTemplate()
        {
            return CanDrawAll();
        }
        [Command]
        public void ImportTemplate()
        {
            try
            {
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Title = "选择人字门闸首参数模板的导入文件名称";
                ofd.Filter = "XML文件|*.xml";
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {  
                    ViewCZViewModel readedViewModel = XmlSerializerHelper.LoadFromXml<ViewCZViewModel>(ofd.FileName);
                    if (readedViewModel != null)
                    {
                        SelectedSSLDType = readedViewModel.SelectedSSLDType;

                        DB_Length = readedViewModel.DB_Length; DB_Width = readedViewModel.DB_Width; DB_Thickness = readedViewModel.DB_Thickness;DB_DoorWidth = readedViewModel.DB_DoorWidth;
                        IsIncludeDBRemoved = readedViewModel.IsIncludeDBRemoved; DB_Removed_Thinckness = readedViewModel.DB_Removed_Thinckness; DB_Removed_A = readedViewModel.DB_Removed_A; DB_Removed_B = readedViewModel.DB_Removed_B; DB_Removed_C = readedViewModel.DB_Removed_C; DB_Removed_N = readedViewModel.DB_Removed_N;

                        BDun_Thickness = readedViewModel.BDun_Thickness;
                        /*BDun_A = readedViewModel.BDun_A; */BDun_B = readedViewModel.BDun_B; BDun_C = readedViewModel.BDun_C; BDun_E = readedViewModel.BDun_E; BDun_F = readedViewModel.BDun_F;
                        IsIncludeBDunChamfer = readedViewModel.IsIncludeBDunChamfer; BDun_Tx = readedViewModel.BDun_Tx; BDun_Ty = readedViewModel.BDun_Ty; BDun_R1 = readedViewModel.BDun_R1;

                        MK_Thickness = readedViewModel.MK_Thickness;
                        MK_B = readedViewModel.MK_B; MK_C = readedViewModel.MK_C; MK_E = readedViewModel.MK_E; MK_F = readedViewModel.MK_F;

                        SSLD_Thickness = readedViewModel.SSLD_Thickness; SSLD_YDis = readedViewModel.SSLD_YDis;
                        SSLD_A = readedViewModel.SSLD_A; SSLD_B = readedViewModel.SSLD_B; SSLD_C = readedViewModel.SSLD_C; SSLD_D = readedViewModel.SSLD_D; SSLD_E = readedViewModel.SSLD_E; SSLD_F = readedViewModel.SSLD_F;
                        SSLD_R1 = readedViewModel.SSLD_R1; SSLD_R2 = readedViewModel.SSLD_R2; SSLD_R3 = readedViewModel.SSLD_R3; SSLD_R4 = readedViewModel.SSLD_R4;

                        SSLD_Endfilling_XDis = readedViewModel.SSLD_Endfilling_XDis; SSLD_Endfilling_Width = readedViewModel.SSLD_Endfilling_Width;
                        SSLD_Endfilling_A = readedViewModel.SSLD_Endfilling_A; SSLD_Endfilling_B = readedViewModel.SSLD_Endfilling_B;SSLD_Endfilling_C = readedViewModel.SSLD_Endfilling_C;SSLD_Endfilling_D = readedViewModel.SSLD_Endfilling_D;

                        IsIncludeDivisionPier = readedViewModel.IsIncludeDivisionPier;
                        DivisionPier_R1 = readedViewModel.DivisionPier_R1; DivisionPier_R2 = readedViewModel.DivisionPier_R2; DivisionPier_R3 = readedViewModel.DivisionPier_R3; DivisionPier_A = readedViewModel.DivisionPier_A ; DivisionPier_B = readedViewModel.DivisionPier_B;

                        //HoleParamList.Clear();
                        int halfnum = readedViewModel.HoleParamList.Count / 2;
                        for (int i = 0; i < halfnum; i++)
                        {
                            readedViewModel.HoleParamList.RemoveAt(0);
                        }
                        HoleParamList = readedViewModel.HoleParamList;
                        TrapHoleThickness = readedViewModel.TrapHoleThickness; IsIncludeTrapHole = readedViewModel.IsIncludeTrapHole;
                        TrapHoleXLength = readedViewModel.TrapHoleXLength; TrapHoleYLength = readedViewModel.TrapHoleYLength; TrapHoleXLengthCorner = readedViewModel.TrapHoleXLengthCorner; TrapHoleYLengthCorner = readedViewModel.TrapHoleYLengthCorner;
                        TrapHoleXDis = readedViewModel.TrapHoleXDis; TrapHoleYDis = readedViewModel.TrapHoleYDis; TrapHoleZDis = readedViewModel.TrapHoleZDis;

                        IsIncludeGs = readedViewModel.IsIncludeGs;
                        GS_MidWidth = readedViewModel.GS_MidWidth;
                        //GS_IntervalList.Clear();
                        halfnum = readedViewModel.GS_IntervalList.Count / 2;
                        for (int i = 0; i < halfnum; i++)
                        {
                            readedViewModel.GS_IntervalList.RemoveAt(0);
                        }
                        GS_IntervalList = readedViewModel.GS_IntervalList;

                        IsIncludeBaffle = readedViewModel.IsIncludeBaffle;
                        //BaffleList.Clear();
                        halfnum = readedViewModel.BaffleList.Count / 2;
                        for (int i = 0; i < halfnum; i++)
                        {
                            readedViewModel.BaffleList.RemoveAt(0);
                        }
                        BaffleList = readedViewModel.BaffleList;
                    }
                    ErrorInfo = ofd.FileName + "参数模板导入成功!";
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "参数模板导入错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public enum SSLDType
    {
        Dispersed,
        Endfiling
    }

}