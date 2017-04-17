using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bentley.DgnPlatformNET.Elements;
using BD = Bentley.DgnPlatformNET;

namespace PDIWT_MS_ZJCZL.Models
{
    class SelectPileTool : BD.DgnElementSetTool
    {
        public SelectPileTool(int toolID, int toolName) : base() { }

        public override BD.StatusInt OnElementModify(Element element)
        {
            return BD.StatusInt.Success;
        }

        protected override void OnRestartTool()
        {
            InstallNewInstance();
        }
        
        public void InstallNewInstance()
        {
            SelectPileTool tool = new SelectPileTool(0, 0);
            tool.InstallTool();
        }
        protected override bool OnDataButton(BD.DgnButtonEvent ev)
        {
            BD.HitPath hitPath = DoLocate(ev, true, 0);
            if (null != hitPath)
            {
                Element ele = hitPath.GetHeadElement();
                SelectedEleID = ele.ElementId.ToString();
                return true;
            }
            else
                return false;
        }

        protected override bool OnResetButton(BD.DgnButtonEvent ev)
        {
            ExitTool();
            return true;
        }
        ////不需要确认点
        //protected override bool NeedAcceptPoint()
        //{
        //    return false;
        //}
        public string SelectedEleID { get; private set; }

    }
}
