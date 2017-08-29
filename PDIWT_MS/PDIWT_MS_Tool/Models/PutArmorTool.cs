using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Office.Utils;
using PDIWT_MS_CPP;
using PDIWT_MS_Tool.Properties;

using BD = Bentley.DgnPlatformNET;
using BM = Bentley.MstnPlatformNET;
using BG = Bentley.GeometryNET;
using BDE = Bentley.DgnPlatformNET.Elements;

namespace PDIWT_MS_Tool.Models
{
    class PutArmorTool : BD.DgnElementSetTool
    {
        private PutArmorTool(string cellname, double u, double v,bool isoutrect) : base()
        {
            cellName = cellname;
            uaxisoffset = u;
            vaxisoffset = v;
            isOutRect = isoutrect;
        }

        public override BD.StatusInt OnElementModify(BDE.Element element)
        {
            BDE.ShapeElement shape = element as BDE.ShapeElement;
            if (shape == null)
                return BD.StatusInt.Error;
            BG.CurveVector curveVector = shape.GetCurveVector();
            BG.DTransform3d world2LoaclDTransform3D;
            BG.DTransform3d loacl2WorlDTransform3D;
            BG.DRange3d shapeRange3D;
            curveVector.IsPlanar(out loacl2WorlDTransform3D, out world2LoaclDTransform3D, out shapeRange3D);
            BG.DMatrix3d rotMatrix3D = loacl2WorlDTransform3D.Matrix;
            List<BG.DPoint3d> points = new List<BG.DPoint3d>();
            curveVector.GetPrimitive(0).TryGetLineString(points);

            BG.DSegment3d linex = new BG.DSegment3d(points[0], points[1]);
            BG.DSegment3d liney = new BG.DSegment3d(points[0], points[3]);

            int ucellnum = 0;
            int vcellnum = 0;
            if (isOutRect)
            {
                ucellnum = (int)Math.Ceiling(linex.Length / uaxisoffset);
                vcellnum = (int)Math.Ceiling(liney.Length / vaxisoffset);
            }
            else
            {
                ucellnum = (int)Math.Floor(linex.Length / uaxisoffset);
                vcellnum = (int)Math.Floor(liney.Length / vaxisoffset);
            }

            double ufraction = uaxisoffset / linex.Length;
            double vfraction = vaxisoffset / liney.Length;
            for (int i = 0; i < vcellnum; i++)
            {
                BG.DPoint3d yaxisPoint = liney.PointAtFraction(i * vfraction);
                for (int j = 0; j < ucellnum; j++)
                {
                    BG.DPoint3d xaxisPoint = linex.PointAtFraction(j * ufraction);
                    BG.DVector3d xyDVector3D = BG.DVector3d.Add(BG.DPoint3d.Subtract(xaxisPoint, points[0]), BG.DPoint3d.Subtract(yaxisPoint, points[0]));
                    BG.DPoint3d putPoint3D = BG.DPoint3d.Add(points[0], xyDVector3D);
                    CellFunction.PlaceCell(new ArmorCellInfo()
                    {
                        CellName = cellName,
                        CellTrans = rotMatrix3D,
                        Origin = putPoint3D
                    });
                    Bentley.UI.Threading.DispatcherHelper.DoEvents();
                }
            }
            return BD.StatusInt.Success;
        }

        protected override void OnRestartTool()
        {
            ExitTool();
        }

        protected override bool WantAccuSnap() => true;
        protected override bool WantAutoLocate() => true;
        protected override bool NeedAcceptPoint() => false;
        protected override bool WantDynamics() => false;
        protected override bool OnResetButton(BD.DgnButtonEvent ev)
        {
            ExitTool();
            return true;
        }

        protected override bool OnPostLocate(BD.HitPath path, out string cantAcceptReason)
        {
            BDE.Element ele = path.GetHeadElement();
            if (ele.ElementType != BD.MSElementType.Shape)
            {
                cantAcceptReason = Resources.PutArmorToolRejectReason;
                return false;
            }
            BDE.ShapeElement shape = ele as BDE.ShapeElement;
            BG.CurveVector shapeCurveVector = shape.GetCurveVector();
            BG.DTransform3d world2LoaclDTransform3D;
            BG.DTransform3d loacl2WorlDTransform3D;
            BG.DRange3d shapeRange3D;
            if (!shapeCurveVector.IsPlanar(out loacl2WorlDTransform3D, out world2LoaclDTransform3D, out shapeRange3D))
            {
                cantAcceptReason = Resources.PutArmorToolRejectReason2;
                return false;
            }
            if (!IsRectangel(shapeCurveVector))
            {
                cantAcceptReason = Resources.PutArmorToolRejectReason3;
                return false;
            }
            return base.OnPostLocate(path, out cantAcceptReason);
        }

        private bool IsRectangel(BG.CurveVector curveVector)
        {
            int PointNume;
            if (curveVector.Count != 1 || !curveVector.IsClosedPath || !curveVector.GetPrimitive(0).TryGetLineStringPointCount(out PointNume) || PointNume != 5)
            {
                return false;
            }
            List<BG.DPoint3d> points = new List<BG.DPoint3d>();
            curveVector.GetPrimitive(0).TryGetLineString(points);
            List<BG.DVector3d> lines = new List<BG.DVector3d>();
            for (int i = 0; i < points.Count - 1; i++)
                lines.Add(BG.DPoint3d.Subtract(points[i + 1], points[i]));
            for (int i = 0; i < lines.Count - 1; i++)
            {
                if (!lines[i].IsPerpendicularTo(lines[i + 1]))
                {
                    return false;
                }
            }
            return true;
        }

        protected override void SetupAndPromptForNextAction()
        {
            mc.StatusCommand = Resources.PutArmorToolName;
            string prompt = ElementAgenda.IsEmpty ? Resources.PutArmorToolChoosePlan : Resources.PutArmorToolConfirm;
            mc.StatusPrompt = prompt;
        }

        private BM.MessageCenter mc = BM.MessageCenter.Instance;
        private double uaxisoffset;
        private double vaxisoffset;
        private string cellName;
        private bool isOutRect;

        public static void InstallNewInstance(string cellname, double uaxisoffset, double vaxisoffset, bool isoutrect)
        {
            PutArmorTool tool = new PutArmorTool(cellname, uaxisoffset, vaxisoffset,isoutrect);
            tool.InstallTool();
        }
    }
}
