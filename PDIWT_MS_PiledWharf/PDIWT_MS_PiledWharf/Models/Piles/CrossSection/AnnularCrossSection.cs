using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PDIWT_MS_PiledWharf.Interface;
using GalaSoft.MvvmLight;
using PDIWT_MS_PiledWharf.Extension.Attribute;

namespace PDIWT_MS_PiledWharf.Models.Piles.CrossSection
{
    [EnumDisplayName(DisplayName = "环形")]
    public class AnnularCrossSection : ObservableObject, IPileCrossSection
    {
        public AnnularCrossSection(double outdia, double innerdia)
        {
            _OuterDiameter = outdia;
            _InnerDiameter = innerdia;
        }
        private double _OuterDiameter;
        public double OuterDiameter
        {
            get { return _OuterDiameter; }
            set { Set(ref _OuterDiameter, value); }
        }
        private double _InnerDiameter;
        public double InnerDiameter
        {
            get { return _InnerDiameter; }
            set { Set(ref _InnerDiameter, value); }
        }


        public double GetActualSectionArea(double fraction)
        {
            return Math.PI / 4 * (_OuterDiameter * _OuterDiameter - _InnerDiameter * _InnerDiameter);
        }

        public double GetOutPerimeter(double fraction)
        {
            return Math.PI * _OuterDiameter;
        }

        public double GetBottomSectionArea()
        {
            return Math.PI * _OuterDiameter * _OuterDiameter / 4;
        }
    }
}
