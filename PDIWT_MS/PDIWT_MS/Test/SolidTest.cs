using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BD = Bentley.DgnPlatformNET;
using BDE = Bentley.DgnPlatformNET.Elements;
using BG = Bentley.GeometryNET;
namespace PDIWT_MS.Test
{
    class SolidTest
    {
        public static void CreateDgnBox()
        {
            BG.DgnBox box = new BG.DgnBox(BG.DPoint3d.Zero, new BG.DPoint3d(100, 100, 100), BG.DVector3d.UnitX, BG.DVector3d.UnitY, 100, 100, 100, 100, true);
            BDE.Element boxEle = BDE.DraftingElementSchema.ToElement(Program.GetActiveDgnModel(), box, null);
            boxEle.AddToModel();
        }
        public static void CreateSphere()
        {
            BG.DgnSphereDetail sphereDetail = new Bentley.GeometryNET.DgnSphereDetail(BG.DPoint3d.Zero, 2000);
            var sphere = BG.SolidPrimitive.CreateDgnSphere(sphereDetail);
            BDE.Element sphereEle = BDE.DraftingElementSchema.ToElement(Program.GetActiveDgnModel(), sphere, null);
            sphereEle.AddToModel();
        }
        public static void CreateExtrudedSolid()
        {
            BG.CurveVector curveVector = BG.CurveVector.CreateDisk(new BG.DEllipse3d(BG.DPoint3d.Zero,BG.DVector3d.UnitX*300,BG.DVector3d.UnitY*200), BG.CurveVector.BoundaryType.Outer);
            BG.DgnExtrusion extrusion = new Bentley.GeometryNET.DgnExtrusion(curveVector, BG.DVector3d.UnitZ * 200, true);
            BDE.Element extrusionEle = BDE.DraftingElementSchema.ToElement(Program.GetActiveDgnModel(), extrusion, null);
            extrusionEle.AddToModel();
        }
    }
}
