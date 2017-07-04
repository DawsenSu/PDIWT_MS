using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;

using BM = Bentley.MstnPlatformNET;
using BD = Bentley.DgnPlatformNET;
using BCOM = Bentley.Interop.MicroStationDGN;

namespace PDIWT_MS_ZJCZL
{
    [BM.AddIn]
    internal sealed class Program : BM.AddIn
    {
        public static Program Addin = null;

        private Program(IntPtr mdlDesc) : base(mdlDesc)
        {
            Addin = this;
        }

        protected override int Run(string[] commandLine)
        {
            ReloadEvent += new ReloadEventHandler(PDIWT_MS_ReloadEvent);
            UnloadedEvent += new UnloadedEventHandler(PDIWT_MS_UnloadedEvent);
            return 0;
        }

        protected override void OnUnloading(UnloadingEventArgs eventArgs)
        {
            base.OnUnloading(eventArgs);
        }

        private void PDIWT_MS_ReloadEvent(BM.AddIn sender, ReloadEventArgs eventArgs)
        {

        }
        private void PDIWT_MS_UnloadedEvent(BM.AddIn sender, UnloadedEventArgs eventArgs)
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
                return BM.InteropServices.Utilities.ComApp;
            }
        }



        private static ResourceManager resourceManager
        {
            get
            {
                return Properties.Resources.ResourceManager;
            }
        }
    }
}
