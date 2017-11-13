using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDIWT_MS_PiledWharf.Models.Piles
{
    public class SteelPCPile : PileBase
    {
        //private double _GammaR;
        //public double GammaR
        //{
        //    get { return _GammaR; }
        //    set { Set(ref _GammaR, value); }
        //}
        //private double _Eta;
        //public double Eta
        //{
        //    get { return _Eta; }
        //    set { Set(ref _Eta, value); }
        //}

        public override double CalculateQd(double gammar, params double[] otherparameters)
        {
            var _pilepieceinfos = GetPilePieceInEachSoilLayerInfos();
            if (_pilepieceinfos.Count == 0) return -1;
            double _acculatenum = 0;
            foreach (var pilepiece in _pilepieceinfos)
                _acculatenum += pilepiece.PilePieceLength * pilepiece.CurrentSoilLayerInfo.Qfi;
            return (ICrossSection.GetOutPerimeter(0) * _acculatenum + otherparameters[0] * _pilepieceinfos.Last().CurrentSoilLayerInfo.Qri*ICrossSection.GetBottomSectionArea())/ gammar;
        }
    }
}
