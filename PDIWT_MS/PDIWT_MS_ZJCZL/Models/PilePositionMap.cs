using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BCOM = Bentley.Interop.MicroStationDGN;
using BG = Bentley.GeometryNET;

namespace PDIWT_MS_ZJCZL.Models
{
    using Piles;
    using Interface;
    using PileCrossSection;
    using HCHXCodeQueryLib;

    class PilePositionMap
    {
        public PilePositionMap(ObservableCollection<PileBase> piles)
        {
            Piles = piles;
            app = Program.COM_App;
            uorpermaster = app.ActiveModelReference.UORsPerMasterUnit;
        }
        #region Field
        BCOM.Application app;
        double uorpermaster;
        #endregion

        #region Properties
        public ObservableCollection<PileBase> Piles { get; private set; }
        #endregion

        #region Method
        public BCOM.ModelReference AddNewModelReference(string modelname)
        {
            List<string> modelnames = new List<string>();
            foreach (BCOM.ModelReference model in app.ActiveDesignFile.Models)
                modelnames.Add(model.Name);
            if (modelnames.Contains(modelname))
                app.ActiveDesignFile.Models.Delete(app.ActiveDesignFile.Models[modelname]);
            return app.ActiveDesignFile.Models.Add(app.ActiveModelReference, modelname, modelname, BCOM.MsdModelType.Normal, false);
        }
        //posistion unit:m
        BCOM.LineElement[] CreateArrowElement(Point3d position, IPileProperty pileprop)
        {
            //double uorpermaster = app.ActiveModelReference.UORsPerMasterUnit;
            BCOM.Point3d centroidp = position.Point3dToBCOMPoint3d(1e4 / uorpermaster);
            centroidp.Z = 0; // 平面
            BG.DVector3d linevector = new BG.DVector3d(pileprop.PileTopPoint.Point3dToDPoint3d(), pileprop.PileBottomPoint.Point3dToDPoint3d());
            double xyrotationRad = linevector.AngleXY.Radians;
            double arrowbarlength = 5 * pileprop.PileDiameter * 1e4 / uorpermaster;

            BCOM.Point3d startp = app.Point3dZero(), endp = app.Point3dFromXY(arrowbarlength, 0);
            BCOM.LineElement bar = app.CreateLineElement2(null, ref startp, ref endp);
            BCOM.Point3d wingendp = app.Point3dFromXY(-arrowbarlength / 3, 0);
            var wing1 = app.CreateLineElement2(null, ref startp, ref wingendp);
            var wing2 = wing1.Clone().AsLineElement();
            wing1.RotateAboutZ(ref startp, app.Radians(25));
            wing2.RotateAboutZ(ref startp, app.Radians(-25));
            wing1.Move(ref endp); wing2.Move(ref endp);
            var lineArray = new BCOM.LineElement[] { bar, wing1, wing2 };
            foreach (var line in lineArray)
            {
                line.RotateAboutZ(ref startp, xyrotationRad);
                line.Move(ref centroidp);
                line.Color = 2;
                //line.LineWeight = 3;
            }
            return lineArray;
        }
        BCOM.Element CreatePileCrossSectionElement(IPileProperty pileprop)
        {
            //double uorpermaster = app.ActiveModelReference.UORsPerMasterUnit;
            BCOM.Matrix3d m = app.Matrix3dIdentity();
            BCOM.Point3d centroidp = pileprop.PileTopPoint.Point3dToBCOMPoint3d(1e4 / uorpermaster);
            centroidp.Z = 0; // xy平面
            double pilediameter = pileprop.PileDiameter * 1e4 / uorpermaster;
            int colorindex = 3;
            if ((pileprop is SquarePileGeometry) || (pileprop is SquarePileGeometry))
            {
                BCOM.Point3d[] squarvertexs =
                {
                    app.Point3dFromXY(pilediameter/2,pilediameter/2),
                    app.Point3dFromXY(-pilediameter/2,pilediameter/2),
                    app.Point3dFromXY(-pilediameter/2,-pilediameter/2),
                    app.Point3dFromXY(pilediameter/2,-pilediameter/2)
                };
                BCOM.ShapeElement shape = app.CreateShapeElement1(null, ref squarvertexs, BCOM.MsdFillMode.NotFilled);
                shape.Move(ref centroidp);
                shape.Color = colorindex;
                return shape;
            }
            else
            {
                var c = app.CreateEllipseElement2(null, ref centroidp, pilediameter / 2, pilediameter / 2, ref m, BCOM.MsdFillMode.NotFilled);
                c.Color = colorindex;
                return c;
            }
        }
        BCOM.TextElement CreatePileSkewnessText(IPileProperty pileprop)
        {
            //double uorpermaster = app.ActiveModelReference.UORsPerMasterUnit;
            BCOM.Point3d centroidp = pileprop.PileTopPoint.Point3dToBCOMPoint3d(1e4 / uorpermaster);
            centroidp.Z = 0; // xy plane
            BCOM.Point3d zerop = app.Point3dZero();
            BCOM.Point3d textelefirstpoisition = app.Point3dFromXY( pileprop.PileDiameter * 1e4 / uorpermaster, 0);
            BCOM.Matrix3d identitym = app.Matrix3dIdentity();
            BCOM.TextElement textele = app.CreateTextElement1(null, Utilities.GetPileSkewnessString(pileprop.GetCosAlpha()), ref textelefirstpoisition, ref identitym);

            BG.DVector3d linevector = new BG.DVector3d(pileprop.PileTopPoint.Point3dToDPoint3d(), pileprop.PileBottomPoint.Point3dToDPoint3d());
            double xyrotationRad = linevector.AngleXY.Radians;
            textele.RotateAboutZ(ref zerop, xyrotationRad);
            textele.Move(ref centroidp);
            return textele;
        }
        //position unit:m
        BCOM.TextElement CreatePilePositionText(Point3d p, string text)
        {
            BCOM.Point3d centroidp = p.Point3dToBCOMPoint3d(1e4 / uorpermaster);
            centroidp.Z = 0; // xy plane            
            BCOM.Matrix3d m = app.Matrix3dIdentity();
            BCOM.TextElement textele = app.CreateTextElement1(null, text, ref centroidp, ref m);
            return textele;
        }
        void GetHVAxisStringList(IEnumerable<string> pilecodes, out List<string> haxisstrings, out List<string> vaxisstrings)
        {
            haxisstrings = new List<string>();
            vaxisstrings = new List<string>();
            foreach (var str in pilecodes)
            {
                string[] tempstr = str.Split(new char[] { '-' });
                if (tempstr.Length < 2)
                    throw new InvalidOperationException($"{str}无法通过'-'字符分割成横纵轴网坐标");
                haxisstrings.Add(tempstr[tempstr.Length - 2]);
                vaxisstrings.Add(tempstr[tempstr.Length - 1]);
            }
            haxisstrings = haxisstrings.Distinct().ToList();
            vaxisstrings = vaxisstrings.Distinct().ToList();
        }

        //BCOM.LineElement[] CreateAxisGrid()
        //{
        //    Tuple<BCOM.Point3d, BCOM.Point3d> temp = new Tuple<BCOM.Point3d, BCOM.Point3d>(app.Point3dZero(), app.Point3dZero());
        //    app.createli
        //}

        public void CreateMap()
        {
            BCOM.ModelReference modelref = AddNewModelReference("桩位图");
            modelref.Activate();

            List<BCOM.Element> elelist = new List<BCOM.Element>();
            BG.DVector3d pilevector;
            foreach (var pile in Piles)
            {
                pilevector = new BG.DVector3d(pile.PilePropertyInfo.PileTopPoint.Point3dToDPoint3d(), pile.PilePropertyInfo.PileBottomPoint.Point3dToDPoint3d());
                if (!pilevector.IsParallelOrOppositeTo(BG.DVector3d.UnitZ))
                    elelist.AddRange(CreateArrowElement(pile.PilePropertyInfo.PileTopPoint, pile.PilePropertyInfo));
                elelist.Add(CreatePileSkewnessText(pile.PilePropertyInfo));
                elelist.Add(CreatePileCrossSectionElement(pile.PilePropertyInfo));
                elelist.Add(CreatePilePositionText(pile.PilePropertyInfo.PileTopPoint, pile.PileCode));
            }

            foreach (var ele in elelist)
                app.ActiveModelReference.AddElement(ele);
            
            app.MessageCenter.AddMessage("桩位图绘制完成", "请在[桩位图]模型中查看", BCOM.MsdMessageCenterPriority.Info);
        }
        #endregion
    }
}
