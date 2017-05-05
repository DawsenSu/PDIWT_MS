using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HCHXCodeQueryLib;

namespace PDIWT_MS_ZJCZL.Interface
{
    public interface IPileProperty
    {
        double PileDiameter { get; set; }

        double PileInnerDiameter { get; set; }

        Point3d PileTopPoint { get; set; }

        Point3d PileBottomPoint { get; set; }

        double WaterLevel { get; set; }

        double PileWeight { get; set; }

        double PileUnderWaterWeight { get; set; }

        double GetPilePerimeter();

        double GetPileLength();

        double GetCosAlpha();

        double GetPileCrossSectionArea();

        double GetPileGravity();
    }
}
