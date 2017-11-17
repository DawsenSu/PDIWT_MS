using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BD = Bentley.DgnPlatformNET;
using BM = Bentley.MstnPlatformNET;
using BG = Bentley.GeometryNET;
using BDEC = Bentley.DgnPlatformNET.DgnEC;
using BES = Bentley.ECObjects.Schema;
using BEI = Bentley.ECObjects.Instance;
using BDE = Bentley.DgnPlatformNET.Elements;

namespace PDIWT_MS_PiledWharf.Models
{
    using Views;
    using Properties;
    using Bentley.ECN;

    public class DrawPileAxisTool : BD.DgnPrimitiveTool
    {
        public DrawPileAxisTool(int toolName, int toolPrompt) : base(toolName, toolPrompt)
        {
            DrawPileAxisView.ShowWindow(Program.Addin);
            ViewModels.ViewModelLocator _locator = new ViewModels.ViewModelLocator();
            _DrawPileVM = _locator.DrawPileAxisVM;
        }

        protected override void OnPostInstall()
        {
            BD.AccuSnap.SnapEnabled = true;
            BD.AccuSnap.LocateEnabled = true;
            base.OnPostInstall();
        }

        protected override bool OnDataButton(BD.DgnButtonEvent ev)
        {
            if (_points.Count == 0)
                BeginDynamics();

            _points.Add(ev.Point);
            SetupAndPromptForNextAction();

            if (_points.Count < 2)
                return false;
            BDE.LineElement _lineEle;
            if (PileAxis.Create(_points, out _lineEle) && _lineEle != null)
            {
                _lineEle.AddToModel();
                if (!PileAxis.ImportPDIWTSchema())
                {
                    _mc.ShowErrorMessage("无法导入PDIWT_Wharf schema", "无法导入PDIWT_Wharf schema", BM.MessageAlert.Balloon);
                    return false;
                }
                var _loactor = new ViewModels.ViewModelLocator();
                if( BD.StatusInt.Success != PileAxis.AttachProperty(_lineEle, _loactor.DrawPileAxisVM))
                {
                    _mc.ShowErrorMessage("无法将实例写入元素上", $"无法将实例Pile写入元素{_lineEle.ElementId}上", BM.MessageAlert.Balloon);
                    return false;
                }
            }
            _points.Clear();
            SetupAndPromptForNextAction();

            return CheckSingleShot();
        }

        protected override void OnDynamicFrame(BD.DgnButtonEvent ev)
        {
            List<BG.DPoint3d> _tmppt = new List<BG.DPoint3d>();
            _tmppt.AddRange(_points);
            BDE.LineElement _lineEle;

            _tmppt.Add(ev.Point);
            if (!PileAxis.Create(_tmppt, out _lineEle) || _lineEle == null)
                return;

            BD.RedrawElems _redraw = new BD.RedrawElems();
            _redraw.SetDynamicsViewsFromActiveViewSet(BM.Session.GetActiveViewport());
            _redraw.DrawMode = BD.DgnDrawMode.TempDraw;
            _redraw.DrawPurpose = BD.DrawPurpose.Dynamics;
            _redraw.DoRedraw(_lineEle);
        }

        protected void SetupAndPromptForNextAction()
        {
            switch (_points.Count)
            {
                case 0:
                    _mc.StatusPrompt = "选择第一个点";
                    break;
                case 1:
                    _mc.StatusPrompt = "选择第二个点";
                    break;
                default:
                    break;
            }
            if (_points.Count != 2) return;
            BG.DVector3d xVec = _points[0] - _points[1];
            xVec.NormalizeInPlace();
            BD.AccuDraw.SetContext(BD.AccuDrawFlags.SetXAxis, _points[1], xVec);
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

        protected override void OnCleanup()
        {
            DrawPileAxisView.CloseWindow();
            base.OnCleanup();
        }

        public void InstallNewInstance()
        {
            var tool = new DrawPileAxisTool(0, 0);
            tool.InstallTool();
            _mc.StatusCommand = "绘制桩轴线";
            SetupAndPromptForNextAction();
        }

        BM.MessageCenter _mc = BM.MessageCenter.Instance;
        List<BG.DPoint3d> _points = new List<BG.DPoint3d>();
        ViewModels.DrawPileAxisViewModel _DrawPileVM;
    }

    public class PileAxis
    {
        public static bool Create(List<BG.DPoint3d> points, out BDE.LineElement lineele)
        {
            lineele = null;
            if (points.Count != 2) return false;
            lineele = new BDE.LineElement(Program.GetActiveDgnModel(), null, new BG.DSegment3d(points[0], points[1]));
            return true;
        }

        public static bool ImportPDIWTSchema()
        {
            PDIWT_MS_PiledWharf_CPP.SchmemaHelper _schemahelper = new PDIWT_MS_PiledWharf_CPP.SchmemaHelper();
            return _schemahelper.ImportPDIWTSchema(Resources.PDIWT_Wharf_01_00_ecschema);
        }

        public static BD.StatusInt AttachProperty(BDE.Element ele,ViewModels.DrawPileAxisViewModel viewmodel)
        {
            BDEC.FindInstancesScope scope = BDEC.FindInstancesScope.CreateScope(Program.GetActiveDgnFile(), new BDEC.FindInstancesScopeOption());
            BES.IECSchema schema = BDEC.DgnECManager.Manager.LocateSchemaInScope(scope, "PDIWT_Wharf", 1, 0, BES.SchemaMatchType.Exact);
            if (schema == null)
                return BD.StatusInt.Error;
            BES.IECClass ecclass = schema.GetClass("Pile");
            if (ecclass == null)
                return BD.StatusInt.Error;
            BDEC.DgnECInstanceEnabler instanceEnabler = BDEC.DgnECManager.Manager.ObtainInstanceEnabler(Program.GetActiveDgnFile(), ecclass);
            if (instanceEnabler == null)
                return BD.StatusInt.Error;
            BEI.StandaloneECDInstance instance = instanceEnabler.SharedWipInstance;
            instance.MemoryBuffer.SetStringValue("PileType", -1, viewmodel.SelectedPileType);

            var _pilecrosssection = instance.FindPropertyValue("PileCrossSection", false, false, false);
            _pilecrosssection.StringValue = viewmodel.SelectedPileType;
            var _pilegridhorizontal = instance.FindPropertyValue("PileGridHorizontal", false, false, false);
            _pilegridhorizontal.StringValue = viewmodel.PileGridHorizontal;
            var _pilegridvertical = instance.FindPropertyValue("PileGridVertical", false, false, false);
            _pilegridvertical.StringValue = viewmodel.PileGridVertical;

            var _sidelength = instance.FindPropertyValue("SideLength", false, false, false);
            var _pileinnerdiameter = instance.FindPropertyValue("PileInnerDiameter", false, false, false);

            switch (viewmodel.SelectedCrossSectionType)
            {
                case "方形截面":
                    _sidelength.DoubleValue = viewmodel.PileSideLength;
                    break;
                case "环形截面":
                    _sidelength.DoubleValue = viewmodel.PileOutterDiameter;
                    _pileinnerdiameter.DoubleValue = viewmodel.PileInnerDiameter;
                    break;
                case "方形圆孔截面":
                    _sidelength.DoubleValue = viewmodel.PileSideLength;
                    _pileinnerdiameter.DoubleValue = viewmodel.PileInnerDiameter;
                    break;
                default:
                    break;
            }
            var _pileweight = instance.FindPropertyValue("PileWeight", false, false, false);
            _pileweight.DoubleValue = viewmodel.PileWeight;
            var _pileunderwaterweight = instance.FindPropertyValue("PileUnderWaterWeight", false, false, false);
            _pileunderwaterweight.DoubleValue = viewmodel.PileUnderWaterWeight;
            instanceEnabler.CreateInstanceOnElement(ele, instance, false);
            return BD.StatusInt.Success;
        }
    }
}
