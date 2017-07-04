using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using BD = Bentley.DgnPlatformNET;
using BCOM = Bentley.Interop.MicroStationDGN;
using System.Runtime.InteropServices;
using BM = Bentley.MstnPlatformNET;
using BDE = Bentley.DgnPlatformNET.Elements;
using BG = Bentley.GeometryNET;

using HCHXCodeQueryLib;


namespace PDIWT_MS.Test
{
    public static class KeyinTest
    {
        public static void TestAsync()
        {
            Task t = new Task(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                System.Windows.Forms.MessageBox.Show("Test!", "Ha", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            });
            t.Start();
            BD.DgnModel activemodel = Program.GetActiveDgnModel();
            //Singleton mode
            BM.MessageCenter messageCenter = BM.MessageCenter.Instance;
            foreach (var levelHandle in activemodel.GetLevelCache().GetHandles())
            {
                messageCenter.ShowInfoMessage(levelHandle.DisplayName, levelHandle.Name + "_" + levelHandle.DisplayName, Bentley.MstnPlatformNET.MessageAlert.None);
            }
            System.Windows.Forms.MessageBox.Show("Test!", "Ha", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
        }

        public static void TestGetVolum()
        {
            double volume, area, closureError, ixy, ixz, iyz;
            Bentley.GeometryNET.DPoint3d centroid, moment, principalMoment, pricipalDirection;
            BD.DgnModel activemodel = Program.GetActiveDgnModel();
            double uor_per_master = activemodel.GetModelInfo().UorPerMaster;
            foreach (var item in activemodel.GetGraphicElements())
            {
                IntPtr nativeElePtr = item.GetNativeElementRef();
                IntPtr nativeEdp;
                IntPtr filePos = IntPtr.Zero;
                if (nativeElePtr != null)
                {
                    Marshal.BentleyMarshal.mdlElmdscr_getByElemRef(out nativeEdp, nativeElePtr, activemodel.GetNative(), 0, filePos);

                    if (0 == Marshal.BentleyMarshal.mdlMeasure_volumeProperties(out volume, out area, out closureError, out centroid, out moment, out ixy, out ixz, out iyz, out principalMoment, out pricipalDirection, nativeEdp, uor_per_master / 10))
                    {
                        BD.NotificationManager.OutputMessage(new BD.NotifyMessageDetails(Bentley.DgnPlatformNET.OutputMessagePriority.Information, volume.ToString(), volume.ToString(), BD.NotifyTextAttributes.DynamicScroll, BD.OutputMessageAlert.None));
                    }
                }
            }
        }

        public static void TestDgnDocManager()
        {
            BD.DgnDocumentManager.DgnBrowserStatus bstatus;
            BD.DgnDocumentBrowserDefaults dgndocdefaults = new BD.DgnDocumentBrowserDefaults { DefaultDirectory = @"C:\", DialogTitle = "Test" };
            BD.DgnDocument dgndoc = BD.DgnDocumentManager.OpenDocumentDialog(out bstatus, dgndocdefaults);
            //bstatus show NOintergation GUI
            if (bstatus == BD.DgnDocumentManager.DgnBrowserStatus.Success)
            {
                System.Windows.MessageBox.Show("Open " + dgndoc.FileName + "!");
            }
        }

        //public static void TestMSDocManager()
        //{
        //    //继承自DgnDocmentManager 实现了相应的接口，可以拿来使用。
        //    BM.MSDocumentManager msdm = BM.MSDocumentManager.Manager;
        //    BM.MSDocumentOpenDialogParams msodp = new BM.MSDocumentOpenDialogParams();
        //    msodp.SetDialogTitle("Test");
        //    msodp.SetDefaultDir(@"D:\");
        //    msodp.SetDefaultFilter("*.dgn");
        //    BD.DgnDocument dgndoc = msdm.OpenDocumentDialog(msodp, BM.FileListAttr.Default, BD.DgnDocument.FetchMode.Read);
        //    if (dgndoc != null)
        //    {
        //        System.Windows.MessageBox.Show(dgndoc.FileName);
        //    }
        //}

        public static void TestWPFToolSetting()
        {
            WPFTest tool = new WPFTest(0, 0);
            tool.InstallNewInstance();
        }

        //public static void TestSharedCell()
        //{
        //    BCOM.Application app = BM.InteropServices.Utilities.ComApp;

        //    BM.MSDocumentManager msdm = BM.MSDocumentManager.Manager;
        //    BM.MSDocumentOpenDialogParams msodp = new BM.MSDocumentOpenDialogParams();
        //    msodp.SetDialogTitle("载入CellLibrary");
        //    msodp.SetDefaultFilter("*.cel");
        //    BD.DgnDocument dgndoc = msdm.OpenDocumentDialog(msodp, BM.FileListAttr.Default, BD.DgnDocument.FetchMode.Read);
        //    if (null != dgndoc)
        //    {
        //        app.AttachCellLibrary(dgndoc.FileName);
        //        //BCOM.Point3d p = app.Point3dZero();
        //        //BCOM.SharedCellElement sharedcell = app.CreateSharedCellElement3("JD1", ref p, true);
        //        BD.Elements.SharedCellElement sharecell = new BD.Elements.SharedCellElement(BM.Session.Instance.GetActiveDgnModel(), null, "JD1", BG.DPoint3d.Zero, BG.DMatrix3d.Identity, BG.DPoint3d.FromXYZ(1, 1, 1));
        //        System.Windows.MessageBox.Show($"CellName:{sharecell.CellName}\n");

        //    }


        //}

        public static void TestLibObj()
        {
            //var p = Program.GetActiveDgnFile().GetLoadedModelsCollection();
            IntPtr filep;
            uint i = PDIWT_MS.Marshal.BentleyMarshal.mdlCell_getLibraryObject(out filep, @"D:\项目\BIM实习\梅山二期\建模中间文件\码头\celllib\节点库.cel", true);
            BD.DgnFile dgnFile = BD.DgnFile.GetDgnFile(filep);
            string name = string.Empty;
            foreach (var index in dgnFile.GetModelIndexCollection())
            {
                if (index.CellPlacementOptions == Bentley.DgnPlatformNET.CellPlacementOptions.CanBePlacedAsCell)
                {
                    name += " " + index.Name + "\n";
                }
            }
            System.Windows.MessageBox.Show(name);
        }

        public static void TestHCHXQueryLib()
        {

            HCHXCodeQuery clr1 = new HCHXCodeQuery();

            AllLayerInfo idInfo = new AllLayerInfo();
            HCHXCodeQueryErrorCode status = clr1.QueryById(ref idInfo, "6782");
            OutputResult("D:\\QueryAllResult6782.txt", idInfo);


            status = clr1.QueryById(ref idInfo, "4358");
            OutputResult("D:\\QueryAllResult4358.txt", idInfo);

            ColumnLayerInfoArray columnLayerInfo = new ColumnLayerInfoArray();
            Point3d startPoint = new Point3d();
            Point3d endPoint = new Point3d();
            startPoint.X = 173928.573996;
            startPoint.Y = 46617.477392;
            startPoint.Z = 36875.226081;

            endPoint.X = 28714.044367;
            endPoint.Y = 130457.125163;
            endPoint.Z = -801521.251635;
            status = clr1.QueryByRay(ref columnLayerInfo, startPoint, endPoint);

            OutputResult("D:\\QueryAllResultRay.txt", columnLayerInfo);

            System.Windows.MessageBox.Show("Finished");
        }

        public static void TestPileQuery()
        {
            HCHXCodeQuery clr1 = new HCHXCodeQuery();

            ColumnLayerInfoArray columnLayerInfo = new ColumnLayerInfoArray();
            Point3d startPoint = new Point3d();
            Point3d endPoint = new Point3d();
            startPoint.X = -1342015;
            startPoint.Y = 681099;
            startPoint.Z = 789160;

            endPoint.X = -1342015;
            endPoint.Y = 681099;
            endPoint.Z = -1264281;
            HCHXCodeQueryErrorCode status = clr1.QueryByRay(ref columnLayerInfo, startPoint, endPoint);

            OutputResult("D:\\Longe.txt", columnLayerInfo);


            ColumnLayerInfoArray columnLayerInfo1 = new ColumnLayerInfoArray();
            Point3d startPoint1 = new Point3d();
            Point3d endPoint1 = new Point3d();
            startPoint1.X = -942187;
            startPoint1.Y = 681099;
            startPoint1.Z = 789160;

            endPoint1.X = -942187;
            endPoint1.Y = 681099;
            endPoint1.Z = -588254;
            status = clr1.QueryByRay(ref columnLayerInfo1, startPoint1, endPoint1);

            OutputResult("D:\\Small.txt", columnLayerInfo1);

        }
        private static void OutputResult(string fileName, AllLayerInfo allInfo)
        {
            if (null == allInfo.BaseInfo)
                return;

            FileStream fs = new FileStream(fileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            if (allInfo.BaseInfo.Count <= 0)
                return;
            //用foreach的循环Dictionary的子项时用的是KeyValuePair
            foreach (KeyValuePair<string, CodeInfo> codeInfo in allInfo.BaseInfo)
            {
                sw.Write("--------------------------------------------------------------------\r\n");
                sw.Write(codeInfo.Value.ToString());

                ColumnLayerInfoArray columnLayerInfo;
                if (!allInfo.IntersectLayerInfos.TryGetValue(codeInfo.Key, out columnLayerInfo))
                    continue;

                sw.Write(columnLayerInfo.ToString());

            }

            sw.Flush();
            sw.Close();
            fs.Close();

        }

        private static void OutputResult(string fileName, ColumnLayerInfoArray columnInfos)
        {
            if (null == columnInfos.m_layers)
                return;

            FileStream fs = new FileStream(fileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            foreach (ColumnLayerInfo columnInfo in columnInfos.m_layers)
            {
                sw.Write(columnInfo.ToString());
            }

            sw.Flush();
            sw.Close();
            fs.Close();

        }

        public static void TestSoild()
        {
            BG.DgnSphere sphere = new Bentley.GeometryNET.DgnSphere(BG.DPoint3d.Zero, 1000);

        }

        public static void ScanBSplineSurface()
        {
            BCOM.Application app = Program.COM_App;
            BCOM.ElementScanCriteria sc = new BCOM.ElementScanCriteriaClass();
            sc.ExcludeAllTypes();
            sc.IncludeType(BCOM.MsdElementType.BsplineSurface);

            var ee = app.ActiveModelReference.Scan(sc);
            //var firstcurrent = ee.Current;
            var bsplinesurface = new List<BCOM.BsplineSurface>();
            while (ee.MoveNext())
            {
                bsplinesurface.Add(ee.Current.AsBsplineSurfaceElement().ExtractBsplineSurface());
            }
            //foreach (var surface in bsplinesurface)
            //{
            //    //var surfacehandler = app.CreatePropertyHandler(surface);
            //    //string s= surfacehandler.GetDisplayString();
            //}
            BCOM.Point3d p1 = app.Point3dFromXYZ(200, 110, 100);
            BCOM.Ray3d ray1 = new Bentley.Interop.MicroStationDGN.Ray3d()
            {
                Origin = app.Point3dFromXYZ(200, 110, 100),
                Direction = app.Point3dFromXYZ(0, 0, -200)
            };
            BCOM.Point3d insertpoint = app.Point3dZero();
            BCOM.Point2d uv = app.Point2dZero();
            foreach (var bsp in bsplinesurface)
            {
                if(bsp.IntersectRay3d(ref insertpoint,ref uv,ref ray1))
                {
                    string s = insertpoint.ToString();
                    string s2 = uv.ToString();
                }
            }          
        }

        public static void TestDimsionElement()
        {
            BCOM.Application app = Program.COM_App;
            var m = app.Matrix3dIdentity();
            var dim = app.CreateDimensionElement1(null, ref m, Bentley.Interop.MicroStationDGN.MsdDimType.AngleLocation);
            var p0 = app.Point3dFromXY(100, 100);
            var p1 = app.Point3dFromXY(000, 0);
            var p2 = app.Point3dFromXY(0, 100);
            dim.AddReferencePoint(app.ActiveModelReference, ref p0);
            dim.AddReferencePoint(app.ActiveModelReference, ref p1);
            dim.AddReferencePoint(app.ActiveModelReference, ref p2);
            app.ActiveModelReference.AddElement(dim);
        }
    }
}
