using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BG = Bentley.GeometryNET;
using BD = Bentley.DgnPlatformNET;

namespace PDIWT_MS_ZJCZL.Models
{
    using Piles;
    using Interface;
    using Factory;
    class PileLengthCalculation
    {
        public PileLengthCalculation(PileBase pile)
        {
            m_pile = pile;
        }
        PileBase m_pile;
        double GetActiveModelBoundaryZ()
        {
            BD.DgnModel activemodel = Program.GetActiveDgnModel();
            BG.DRange3d modelrange;
            activemodel.GetRange(out modelrange);
            return modelrange.Low.Z;
        }
        //unit:m
        BG.DSegment3d GetVirtualPileAxis()
        {
            BG.DSegment3d pileaxis = new BG.DSegment3d(m_pile.PilePropertyInfo.PileTopPoint.Point3dToDPoint3d(), m_pile.PilePropertyInfo.PileBottomPoint.Point3dToDPoint3d());
            double pileaxiszlength = pileaxis.Extent.Z;
            double virtualaxiszlength = pileaxis.StartPoint.Z - 2 * GetActiveModelBoundaryZ() * 1e-4;
            double scale = Math.Abs(virtualaxiszlength / pileaxiszlength);
            BG.DPoint3d lowpoint = pileaxis.PointAtFraction(scale);
            return new BG.DSegment3d(pileaxis.StartPoint, lowpoint);
        }


    }
}
