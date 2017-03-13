using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BD = Bentley.DgnPlatformNET;
using BCOM = Bentley.Interop.MicroStationDGN;
using System.Runtime.InteropServices;

namespace PDIWT_MS
{
    internal class KeyinCommands
    {
        #region (PDIWT_MS Test) command
        public static void Test(string unparsed)
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
                    mdlElmdscr_getByElemRef(out nativeEdp, nativeElePtr, activemodel.GetNative(), 0, filePos);
                    if (0 == mdlMeasure_volumeProperties(out volume, out area, out closureError, out centroid, out moment, out ixy, out ixz, out iyz, out principalMoment, out pricipalDirection, nativeEdp, uor_per_master / 10))
                    {
                        BD.NotificationManager.OutputMessage(new Bentley.DgnPlatformNET.NotifyMessageDetails(Bentley.DgnPlatformNET.OutputMessagePriority.Information, volume.ToString(), volume.ToString(), BD.NotifyTextAttributes.DynamicScroll, BD.OutputMessageAlert.None));
                    }
                }
            }
        }

        [DllImport("ustation.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdlMeasure_volumeProperties(out double volume, out double area, out double closureError, out Bentley.GeometryNET.DPoint3d centroid, out Bentley.GeometryNET.DPoint3d moment, out double ixy, out double ixz, out double iyz, out Bentley.GeometryNET.DPoint3d principalMoments, out Bentley.GeometryNET.DPoint3d principalDirections, IntPtr edp, double tolerance);
        //通过elemRef获得elmdscr
        [DllImport("ustation.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint mdlElmdscr_getByElemRef(out  IntPtr edPP, IntPtr elemRef, IntPtr modelRef, int expandSharedCells, IntPtr filePos);

        #endregion

        public static void TestWPF(string unparsed)
        {
            wpfUC.ShowWindow(Program.Addin);
        }

        #region Tools
        public static void QuickInsert(string unparsed)
        {
            Tools.QuickInsertUC.ShowWindow(Program.Addin);
        }

        public static void GetQuantities(string unparsed)
        {
            Tools.GetQuantitiesUC.ShowWindow(Program.Addin);
        }
        #endregion
    }
}
