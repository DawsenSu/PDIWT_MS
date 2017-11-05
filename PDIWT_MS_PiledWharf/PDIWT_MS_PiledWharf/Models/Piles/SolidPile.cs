using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDIWT_MS_PiledWharf.Models.Piles
{
    public class SolidPile : PileBase
    {
        private double _GammaR;
        public double GammaR
        {
            get { return _GammaR; }
            set { Set(ref _GammaR, value); }
        }

        public override double CalculateQd()
        {
            var _pilepieceinfos = GetPilePieceInEachSoilLayerInfos();
            if (_pilepieceinfos.Count == 0) return -1;
            double _accumlatenum = 0;
            foreach (var pilepiece in _pilepieceinfos)
                _accumlatenum += pilepiece.PilePieceLength * pilepiece.CurrentSoilLayerInfo.Qfi;
            return (ICrossSection.GetOutPerimeter(0) * _accumlatenum + _pilepieceinfos.Last().CurrentSoilLayerInfo.Qri * ICrossSection.GetBottomSectionArea()) / _GammaR;
        }

    }
}
