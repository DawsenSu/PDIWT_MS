using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
namespace PDIWT_MS_PiledWharf.Models.Soil
{
    public class PilePieceInSoilLayerInfo : ObservableObject
    {
        private SoilLayerInfo _CurrentSoilLayerInfo;
        public SoilLayerInfo CurrentSoilLayerInfo
        {
            get { return _CurrentSoilLayerInfo; }
            set { Set(ref _CurrentSoilLayerInfo, value); }
        }

        private double _PileTopZ_InCurrentSoilLayer;
        public double PileTopZ_InCurrentSoilLayer
        {
            get { return _PileTopZ_InCurrentSoilLayer; }
            set { Set(ref _PileTopZ_InCurrentSoilLayer, value); }
        }
        private double _PileBottomZ_InCurrentSoilLayer;
        public double PileBottomZ_InCurrentSoilLayer
        {
            get { return _PileBottomZ_InCurrentSoilLayer; }
            set { Set(ref _PileBottomZ_InCurrentSoilLayer, value); }
        }
        private double _PilePieceLength;
        public double PilePieceLength
        {
            get { return _PilePieceLength; }
            set { Set(ref _PilePieceLength, value); }
        }

    }
}
