using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using PDIWT_MS_PiledWharf.Interface;

namespace PDIWT_MS_PiledWharf.Models.Piles.CrossSection
{
    public class SquareCrossSection : ObservableObject, IPileCrossSection
    {
        public SquareCrossSection(double sidelength)
        {
            _SideLength = sidelength;
        }
        private double _SideLength;
        public double SideLength
        {
            get { return _SideLength; }
            set { Set(ref _SideLength, value); }
        }

        public double GetActualSectionArea(double fraction)
        {
            return _SideLength * _SideLength;
        }

        public double GetOutPerimeter(double fraction)
        {
            return 4 * _SideLength;
        }

        public double GetBottomSectionArea()
        {
            return GetActualSectionArea(1);
        }
    }
}
