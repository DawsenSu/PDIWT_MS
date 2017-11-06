using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using PDIWT_MS_PiledWharf.Interface;

namespace PDIWT_MS_PiledWharf.Models.Piles.CrossSection
{
    public class SquareWithRoundHoleCrossSection : ObservableObject, IPileCrossSection
    {
        public SquareWithRoundHoleCrossSection(double sidelength, double holediameter)
        {
            _SideLength = sidelength; _HoleDiameter = holediameter;
        }

        private double _SideLength;
        public double SideLength
        {
            get { return _SideLength; }
            set { Set(ref _SideLength, value); }
        }

        private double _HoleDiameter;
        public double HoleDiameter
        {
            get { return _HoleDiameter; }
            set { Set(ref _HoleDiameter, value); }
        }

        public double GetActualSectionArea(double fraction)
        {
            return _SideLength * _SideLength - Math.PI / 4 * _HoleDiameter * _HoleDiameter ;
        }

        public double GetOutPerimeter(double fraction)
        {
            return 4 * _SideLength;
        }

        public double GetBottomSectionArea()
        {
            return _SideLength * _SideLength;
        }
    }
}
