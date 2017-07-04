using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BG = Bentley.GeometryNET;
using BD = Bentley.DgnPlatformNET;
using BDE = Bentley.DgnPlatformNET.Elements;
using BCOM = Bentley.Interop.MicroStationDGN;
using BM = Bentley.MstnPlatformNET;

namespace PDIWT_MS_DIM.Models
{
    class FastWholeDimTool : BD.DgnPrimitiveTool
    {
        private List<BG.DPoint3d> points;
        private FastWholeDimTool(int toolName, int toolPrompt) : base(toolName, toolPrompt)
        {
            points = new List<BG.DPoint3d>();
        }

        protected override bool OnDataButton(BD.DgnButtonEvent ev)
        {
            if (points.Count == 0)
                BeginDynamics();

            points.Add(ev.Point);
            if (points.Count < 2)
                return false;

            BDE.LineElement line = new BDE.LineElement(Program.GetActiveDgnModel(), null, new BG.DSegment3d(points[0], points[1]));
            if (null != line)
                line.AddToModel();

            base.OnReinitialize();
            return true;
        }

        protected override void OnDynamicFrame(BD.DgnButtonEvent ev)
        {
            BDE.LineElement line = new BDE.LineElement(Program.GetActiveDgnModel(), null, new BG.DSegment3d(points[0], ev.Point));
            if (null == line)
                return;
            BD.RedrawElems redrawElems = new BD.RedrawElems();
            redrawElems.SetDynamicsViewsFromActiveViewSet(BM.Session.GetActiveViewport());
            redrawElems.DrawMode = BD.DgnDrawMode.TempDraw;
            redrawElems.DrawPurpose = BD.DrawPurpose.Dynamics;
            redrawElems.DoRedraw(line);
        }

        protected override bool OnResetButton(BD.DgnButtonEvent ev)
        {
            ExitTool();
            return true;
        }

        protected override void OnRestartTool()
        {
            InstallNewInstance();
        }

        protected override void OnPostInstall()
        {
            BD.AccuSnap.SnapEnabled = true;
            base.OnPostInstall();
        }

        public static void InstallNewInstance()
        {
            FastWholeDimTool tool = new FastWholeDimTool(0, 0);
            tool.InstallTool();
        }
    }
}
