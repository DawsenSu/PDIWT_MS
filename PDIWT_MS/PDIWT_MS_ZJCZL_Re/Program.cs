using System;
using Bentley.MstnPlatformNET.InteropServices;
using BCOM = Bentley.Interop.MicroStationDGN;
using BD = Bentley.DgnPlatformNET;
using BM = Bentley.MstnPlatformNET;

namespace PDIWT_MS_ZJCZL_Re
{

    [BM.AddInAttribute(MdlTaskID = "PDIWT_MS_ZJCZL_Re")]
    internal sealed class Program : BM.AddIn
    {
        public static Program Addin;
        private Program(IntPtr mdlDesc) : base(mdlDesc)
        {
            Addin = this;
        }
        protected override int Run(string[] commandLine)
        {
            ReloadEvent += PDIWT_MS_ZJCZL_Re_ReloadEvent;
            UnloadedEvent += PDIWT_MS_ZJCZL_Re_UnloadedEvent;
            return 0;
        }
        protected override void OnUnloading(UnloadingEventArgs eventArgs)
        {
            base.OnUnloading(eventArgs);
        }
        private void PDIWT_MS_ZJCZL_Re_ReloadEvent(BM.AddIn sender, ReloadEventArgs eventArgs)
        {

        }
        private void PDIWT_MS_ZJCZL_Re_UnloadedEvent(BM.AddIn sender, UnloadedEventArgs eventArgs)
        {

        }
        //
        public static BD.DgnFile GetActiveDgnFile() => BM.Session.Instance.GetActiveDgnFile();
        public static BD.DgnModelRef GetActiveDgnModelRef() => BM.Session.Instance.GetActiveDgnModelRef();
        public static BD.DgnModel GetActiveDgnModel() => BM.Session.Instance.GetActiveDgnModel();

        public static BCOM.Application COM_App
        {
            get
            {
                return Utilities.ComApp;
            }
        }
        //private static ResourceManager resourceManager
        //{
        //    get
        //    {
        //        return Properties.Resources.ResourceManager;
        //    }
        //}
    }
}
