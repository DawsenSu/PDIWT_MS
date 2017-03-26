using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BD = Bentley.DgnPlatformNET;
using BCOM = Bentley.Interop.MicroStationDGN;
using System.Runtime.InteropServices;
using BM = Bentley.MstnPlatformNET;
using BG = Bentley.GeometryNET;

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

        public static void TestMSDocManager()
        {
            //继承自DgnDocmentManager 实现了相应的接口，可以拿来使用。
            BM.MSDocumentManager msdm = BM.MSDocumentManager.Manager;
            BM.MSDocumentOpenDialogParams msodp = new BM.MSDocumentOpenDialogParams();
            msodp.SetDialogTitle("Test");
            msodp.SetDefaultDir(@"D:\");
            msodp.SetDefaultFilter("*.dgn");
            BD.DgnDocument dgndoc = msdm.OpenDocumentDialog(msodp, BM.FileListAttr.Default, BD.DgnDocument.FetchMode.Read);
            if (dgndoc != null)
            {
                System.Windows.MessageBox.Show(dgndoc.FileName);
            }
        }

        public static void TestWPFToolSetting()
        {
            WPFTest tool = new WPFTest(0, 0);
            tool.InstallNewInstance();
        }

        public static void TestSharedCell()
        {
            BCOM.Application app = BM.InteropServices.Utilities.ComApp;

            BM.MSDocumentManager msdm = BM.MSDocumentManager.Manager;
            BM.MSDocumentOpenDialogParams msodp = new BM.MSDocumentOpenDialogParams();
            msodp.SetDialogTitle("载入CellLibrary");
            msodp.SetDefaultFilter("*.cel");
            BD.DgnDocument dgndoc = msdm.OpenDocumentDialog(msodp, BM.FileListAttr.Default, BD.DgnDocument.FetchMode.Read);
            if (null!=dgndoc)
            {
                app.AttachCellLibrary(dgndoc.FileName);
                //BCOM.Point3d p = app.Point3dZero();
                //BCOM.SharedCellElement sharedcell = app.CreateSharedCellElement3("JD1", ref p, true);
                BD.Elements.SharedCellElement sharecell = new BD.Elements.SharedCellElement(BM.Session.Instance.GetActiveDgnModel(), null, "JD1", BG.DPoint3d.Zero, BG.DMatrix3d.Identity, BG.DPoint3d.FromXYZ(1, 1, 1));
                System.Windows.MessageBox.Show($"CellName:{sharecell.CellName}\n");
                
            }


        }

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
    }
}
