using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCOM = Bentley.Interop.MicroStationDGN;
using BD = Bentley.DgnPlatformNET;
using BM = Bentley.MstnPlatformNET;

namespace BentleyTrain01
{

    [BM.AddIn(MdlTaskID = "BentleyTrain01")]
    internal sealed class Program : BM.AddIn
    {
        public static Program Addin = null;
        private Program(IntPtr mdlDesc) : base(mdlDesc)
        {
            Addin = this;
        }
        protected override int Run(string[] commandLine)
        {
            ReloadEvent += new ReloadEventHandler(BentleyTrain01_ReloadEvent);
            UnloadedEvent += new UnloadedEventHandler(BentleyTrain01_UnloadedEvent);
            BeforeNewDesignFileEvent += Program_BeforeNewDesignFileEvent;
            
            return 0;
        }

        private void Program_BeforeNewDesignFileEvent(BM.AddIn sender, BeforeNewDesignFileEventArgs eventArgs)
        {
            MessageBox.Show("Test");
        }

        protected override void OnUnloading(UnloadingEventArgs eventArgs)
        {
            base.OnUnloading(eventArgs);
        }
        private void BentleyTrain01_ReloadEvent(BM.AddIn sender, ReloadEventArgs eventArgs)
        {

        }
        private void BentleyTrain01_UnloadedEvent(BM.AddIn sender, UnloadedEventArgs eventArgs)
        {

        }
        //
        public static BD.DgnFile ActiveDgnFile => BM.Session.Instance.GetActiveDgnFile();
        public static BD.DgnModelRef ActiveDgnModelRef => BM.Session.Instance.GetActiveDgnModelRef();
        public static BD.DgnModel ActiveDgnModel => BM.Session.Instance.GetActiveDgnModel();

        public static BCOM.Application ComApp => BM.InteropServices.Utilities.ComApp;
        //private static ResourceManager resourceManager
        //{
        //    get
        //    {
        //        return Properties.Resources.ResourceManager;
        //    }
        //}
    }
}
