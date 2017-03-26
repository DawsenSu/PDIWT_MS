using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bentley.DgnPlatformNET.Elements;
using BD = Bentley.DgnPlatformNET;
using System.Windows;
using Bentley.MstnPlatformNET;

namespace PDIWT_MS.Test
{
    class WPFTest : BD.DgnElementSetTool
    {
        public WPFTest(int toolID, int toolName) : base()
        {
            WPFTestToolSetting.ShowWindow(Program.Addin); 
            cet.StatusCommand = "Tes";

        }
        public override BD.StatusInt OnElementModify(Element element)
        {
            cet.ShowInfoMessage(element.ElementId.ToString(), "", false);
            return BD.StatusInt.Success;
        }

        protected override void OnRestartTool()
        {
            InstallNewInstance();
        }

        protected override void ExitTool()
        {
            WPFTestToolSetting.CloseWindow();
            base.ExitTool();
        }

        protected override bool OnResetButton(BD.DgnButtonEvent ev)
        {
            ExitTool();
            return true;
        }

        public void InstallNewInstance()
        {
            WPFTest tools = new WPFTest(0, 0);
            tools.InstallTool();
        }
        MessageCenter cet = MessageCenter.Instance;
    }
}
