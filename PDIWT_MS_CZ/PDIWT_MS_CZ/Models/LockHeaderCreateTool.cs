using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PDIWT_MS_CZ.ViewModels;
using Bentley.GeometryNET;
using Bentley.DgnPlatformNET;
using Bentley.DgnPlatformNET.Elements;
using Bentley.MstnPlatformNET;
using GalaSoft.MvvmLight.Messaging;

namespace PDIWT_MS_CZ.Models
{
    public class LockHeaderCreateTool : DgnPrimitiveTool
    {
        public LockHeaderCreateTool(int toolName, int toolPrompt, LockHeadParameters lhparam) : base(toolName, toolPrompt)
        {


            _scale = Session.Instance.GetActiveDgnModel().GetModelInfo().UorPerMeter / 1000;
            _DrawingValid = StatusInt.Error;
            _lhparam = lhparam;
        }
        private LockHeadParameters _lhparam;
        private double _scale;
        private int _DrawingValid;
        //public int DrawingValid
        //{
        //    get { return _DrawingValid; }
        //    private set { _DrawingValid = value; }
        //}
        protected override bool OnDataButton(DgnButtonEvent ev)
        {
            PDIWT_MS_CZ_CPP.LockHeadDrawing _drawing = new PDIWT_MS_CZ_CPP.LockHeadDrawing(_lhparam);
            _DrawingValid = _drawing.DoDraw(ev.Point.X/_scale,ev.Point.Y/_scale,ev.Point.Z/_scale);
            if (_DrawingValid == 0)
                Messenger.Default.Send<bool>(true, "DrawMessage");
            else
                Messenger.Default.Send<bool>(false, "DrawMessage");
            ExitTool();
            return true;
        }

        protected override void OnDynamicFrame(DgnButtonEvent ev)
        {
            Element _ele = CreateLockHeaderOutBox(ev.Point);
            if (_ele == null)
                return;

            RedrawElems _redrawElems = new RedrawElems();
            _redrawElems.SetDynamicsViewsFromActiveViewSet(Session.GetActiveViewport());
            _redrawElems.DrawMode = DgnDrawMode.TempDraw;
            _redrawElems.DrawPurpose = DrawPurpose.Dynamics;
            _redrawElems.DoRedraw(_ele);

        }

        protected override bool OnResetButton(DgnButtonEvent ev)
        {
            ExitTool();
            return true;
        }

        protected override void OnRestartTool() => InstallNewInstance(ToolId, ToolPromptResourceId, _lhparam);


        protected override void OnPostInstall()
        {
            AccuSnap.SnapEnabled = true;
            BeginDynamics();
            base.OnPostInstall();
        }

        public static void InstallNewInstance(int tooId, int toolPrompt, LockHeadParameters lhparam)
        {
            LockHeaderCreateTool tool = new LockHeaderCreateTool(tooId, toolPrompt, lhparam);
            tool.InstallTool();
        }
        Element CreateLockHeaderOutBox(DPoint3d anchorpoint)
        {
            DPoint3d _basepoint = DPoint3d.Add(anchorpoint, DPoint3d.FromXYZ(-_scale * _lhparam.LH_BaseBoard.BaseBoardWidth / 2, 0, 0));
            DPoint3d _toppoint = DPoint3d.Add(_basepoint, DPoint3d.FromXYZ(0, 0, _scale * (_lhparam.LH_BaseBoard.BaseBoardHeight + _lhparam.LH_SidePier.PierHeight)));
            DgnBox _outbox = new DgnBox(_basepoint, _toppoint,
                DVector3d.UnitX, DVector3d.UnitY,
                _scale * _lhparam.LH_BaseBoard.BaseBoardWidth, _scale * _lhparam.LH_BaseBoard.BaseBoardLength,
                _scale * _lhparam.LH_BaseBoard.BaseBoardWidth, _scale * _lhparam.LH_BaseBoard.BaseBoardLength, true);
            Element _ele = DraftingElementSchema.ToElement(Session.Instance.GetActiveDgnModel(), _outbox, null);
            return _ele;
        }

    }
}
