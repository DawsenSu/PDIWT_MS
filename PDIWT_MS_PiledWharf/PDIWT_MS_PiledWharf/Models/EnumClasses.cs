using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDIWT_MS_PiledWharf.Models
{
    class EnumClasses
    {
    }

    public enum PileType
    {
        Solid,
        SteelAndPercastConcrete
    }
    public enum PileCrossSectionType
    {
        Annular,
        Square,
        SquareWithCircleHole,
        Polygon
    }
}
