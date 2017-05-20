using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BG = Bentley.GeometryNET;

using HCHXCodeQueryLib;

namespace PDIWT_MS_ZJCZL.Interface
{
    public interface IPileProperty
    {
        // unit:m
        double PileDiameter { get; set; }
        // unit:m
        double PileInnerDiameter { get; set; }
        // unit:m
        Point3d PileTopPoint { get; set; }
        // unit:m
        Point3d PileBottomPoint { get; set; }
        // unit:m
        double WaterLevel { get; set; }
        // unit: KN/m
        double PileWeight { get; set; }
        // unit: KN/m
        double PileUnderWaterWeight { get; set; }
        // unit: m
        double GetPilePerimeter();
        // unit: m
        double GetPileLength();
        // no unit
        double GetCosAlpha();
        // unit: m^2
        double GetPileCrossSectionArea();
        // unit: m^2
        double GetPileOutLineArea();
        // unit: kn
        double GetPileGravity();
        // pileLength: unit m
        void SetPileLength(double pileLength);
    }
}
