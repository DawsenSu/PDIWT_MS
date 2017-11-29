using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Bentley.DgnPlatformNET;
using Bentley.DgnPlatformNET.Elements;
using Bentley.MstnPlatformNET;
using Bentley.GeometryNET;

namespace PDIWT_MS_PiledWharf.Models
{
    using Piles;
    using Piles.CrossSection;
    using Interface;

    public class PilePositionMap
    {
        public PilePositionMap(ObservableCollection<PileBase> piles)
        {
            Piles = piles;
            uorpermeter = Program.GetActiveDgnModel().GetModelInfo().UorPerMeter;
            newmodel = AddNewDgnModel("桩位图");
        }

        double uorpermeter;
        public ObservableCollection<PileBase> Piles { get; private set; }
        public DgnModel newmodel;

        DgnModel AddNewDgnModel(string modelname)
        {
            List<string> modelnames = new List<string>();
            DgnFile activedgnfile = Program.GetActiveDgnFile();
            foreach (var model in activedgnfile.GetLoadedModelsCollection())
                modelnames.Add(model.ModelName);
            int i = 1;
            while (modelnames.Contains(modelname))
            {
                modelname += i.ToString();
                i++;
            }
            activedgnfile.DeleteModel(activedgnfile.FindLoadedModelById(activedgnfile.FindModelIdByName(modelname)));
            //{
            //    if (model.ModelName == modelname)
            //        activedgnfile.DeleteModel(model);
            //}
            DgnModelStatus _dgnmodelstatus;
            return activedgnfile.CreateNewModel(out _dgnmodelstatus, modelname, DgnModelType.Normal, false, Program.GetActiveDgnModel());
        }

        LineElement[] CreateArrowElement(PileBase pile)
        {
            DPoint3d position = new DPoint3d(pile.TopPoint);
            position.ScaleInPlace(uorpermeter);
            position.Z = 0;
            double xyrotationrad = (pile.BottomPoint - pile.TopPoint).AngleXY.Radians;
            DMatrix3d _wholerot = DMatrix3d.FromEulerAngles(EulerAngles.FromRadians(0, 0, xyrotationrad));
            DTransform3d _wholetrans = DTransform3d.FromMatrixAndTranslation(_wholerot, position);

            double arrowbarlength = 5 * Math.Sqrt(pile.ICrossSection.GetBottomSectionArea()) * uorpermeter;

            DPoint3d endpoint = DPoint3d.FromXY(arrowbarlength, 0);
            LineElement bar = new LineElement(newmodel, null, new DSegment3d(DPoint3d.Zero, endpoint));
            LineElement wing1 = new LineElement(newmodel, null, new DSegment3d(DPoint3d.FromXY(arrowbarlength * 2 / 3, 0), endpoint));
            LineElement wing2 = new LineElement(newmodel, null, new DSegment3d(DPoint3d.FromXY(-arrowbarlength * 2 / 3, 0), endpoint));
            DTransform3d wing1rot = DTransform3d.FromRotationAroundLine(endpoint, DVector3d.UnitZ, Angle.FromDegrees(25));
            DTransform3d wing2rot = DTransform3d.FromRotationAroundLine(endpoint, DVector3d.UnitZ, Angle.FromDegrees(-25));
            wing1.ApplyTransform(new TransformInfo(wing1rot));
            wing2.ApplyTransform(new TransformInfo(wing2rot));
            LineElement[] linearray = new LineElement[] { bar, wing1, wing2 };
            foreach (var line in linearray)
            {
                line.ApplyTransform(new TransformInfo(_wholetrans));
            }
            return linearray;
        }

        TextHandlerBase CreatePileSkewnessText(PileBase pile)
        {
            DPoint3d position = new DPoint3d(pile.TopPoint);
            position.ScaleInPlace(uorpermeter);
            position.Z = 0;
            double xyrotationrad = (pile.BottomPoint - pile.TopPoint).AngleXY.Radians;
            DMatrix3d _wholerot = DMatrix3d.FromEulerAngles(EulerAngles.FromRadians(0, 0, xyrotationrad));
            DTransform3d _wholetrans = DTransform3d.FromMatrixAndTranslation(_wholerot, position);

            TextBlock textblock = new TextBlock(newmodel);
            textblock.AppendText(GetPileSkewnessString(Math.Cos(xyrotationrad)));
            TextHandlerBase textele = TextHandlerBase.CreateElement(null, textblock);
           
            textele.ApplyTransform(new TransformInfo(_wholetrans));
            return textele;
        }

        TextHandlerBase CreatePilePositionText(PileBase pile)
        {
            DPoint3d position = new DPoint3d(pile.TopPoint);
            position.ScaleInPlace(uorpermeter);
            position.Z = 0;
            DTransform3d _wholetrans = DTransform3d.FromTranslation(position);

            TextBlock textblock = new TextBlock(newmodel);
            textblock.AppendText(pile.Code);

            TextHandlerBase textele = TextHandlerBase.CreateElement(null, textblock);
            textele.ApplyTransform(new TransformInfo(_wholetrans));
            return textele;
        }

        Element CreatePileCrossSectionElement(PileBase pile)
        {
            DPoint3d position = new DPoint3d(pile.TopPoint);
            position.ScaleInPlace(uorpermeter);
            position.Z = 0;

            var crosssection = pile.ICrossSection;
            if (crosssection is AnnularCrossSection)
            {
                AnnularCrossSection cs = crosssection as AnnularCrossSection;
                EllipseElement outercircle = new EllipseElement(newmodel, null, new DEllipse3d(position, DVector3d.FromXY(cs.OuterDiameter * uorpermeter, 0), DVector3d.FromXY(0, cs.OuterDiameter * uorpermeter)));
                return outercircle;
            }
            else if (crosssection is SquareCrossSection)
            {
                SquareCrossSection cs = crosssection as SquareCrossSection;
                double sidelength = cs.SideLength * uorpermeter;
                DPoint3d[] squarevertexs =
                {
                    DPoint3d.FromXY(sidelength / 2, sidelength /2 )  + position,
                    DPoint3d.FromXY(-sidelength / 2, sidelength /2 )+ position,
                    DPoint3d.FromXY(-sidelength / 2, -sidelength /2 )+ position,
                    DPoint3d.FromXY(sidelength / 2, -sidelength /2 )+ position
                };
                ShapeElement shapeele = new ShapeElement(newmodel, null, squarevertexs);
                return shapeele;
            }
            else
            {
                SquareWithRoundHoleCrossSection cs = crosssection as SquareWithRoundHoleCrossSection;
                double sidelength = cs.SideLength * uorpermeter;
                DPoint3d[] squarevertexs =
                {
                    DPoint3d.FromXY(sidelength / 2, sidelength /2 )  + position,
                    DPoint3d.FromXY(-sidelength / 2, sidelength /2 )+ position,
                    DPoint3d.FromXY(-sidelength / 2, -sidelength /2 )+ position,
                    DPoint3d.FromXY(sidelength / 2, -sidelength /2 )+ position
                };
                ShapeElement shapeele = new ShapeElement(newmodel, null, squarevertexs);
                return shapeele;
            }
        }

        string GetPileSkewnessString(double cosa)
        {
            if (Math.Abs(cosa - 1) < 1e-4)
            {
                return "直桩";
            }
            else
            {
                return string.Format("1:{0}", Math.Round(1.0 / Math.Sqrt(1 - cosa * cosa) / cosa, 0));
            }
        }


        public void CreateMap()
        {
            List<Element> elelist = new List<Element>();
            foreach (var pile in Piles)
            {
                if (!new DSegment3d(pile.TopPoint, pile.BottomPoint).UnitTangent.IsParallelOrOppositeTo(DVector3d.UnitX))
                    elelist.AddRange(CreateArrowElement(pile));
                elelist.Add(CreatePileSkewnessText(pile));
                elelist.Add(CreatePilePositionText(pile));
                //elelist.Add(CreatePileCrossSectionElement(pile));
            }
            foreach (var ele in elelist)
            {
                ele.AddToModel();
            }
            MessageCenter.Instance.ShowInfoMessage("桩位图绘制完成", $"请在{newmodel.ModelName}模型中查看", false);
        }
    }
}
