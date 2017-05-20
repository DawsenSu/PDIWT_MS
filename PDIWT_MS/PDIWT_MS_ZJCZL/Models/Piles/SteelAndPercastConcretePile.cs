using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using PDIWT_MS_ZJCZL.Models.Soil;
using PDIWT_MS_ZJCZL.Interface;
using PDIWT_MS_ZJCZL.Models.Piles;
using DevExpress.Mvvm;

namespace PDIWT_MS_ZJCZL.Models.Piles
{
    [Serializable]
    class SteelAndPercastConcretePile : PileBase
    {
        public override double CalculateQd()
        {
            if (SteelAndPercastConcretPileLayerInfoProp == null || SteelAndPercastConcretPileLayerInfoProp.Count == 0)
                throw new ArgumentNullException($"{PileCode}的PileLayerInfo属性为null或者为empty");
            double accumlatenum = 0;
            foreach (var pilesoil in SteelAndPercastConcretPileLayerInfoProp)
                accumlatenum += pilesoil.PileInSoilLayerLength * pilesoil.Qfi;
            return (PilePropertyInfo.GetPilePerimeter() * accumlatenum + Eta* Qr * PilePropertyInfo.GetPileOutLineArea()) / GammaR;
        }

        public override double CalculateTd()
        {
            return base.CalculateTd(SteelAndPercastConcretPileLayerInfoProp, GammaR);
        }


        public double GammaR
        {
            get { return GetProperty(() => GammaR); }
            set { SetProperty(() => GammaR, value); }
        }
        public double Qr
        {
            get { return GetProperty(() => Qr); }
            set { SetProperty(() => Qr, value); }
        }
        public double Eta
        {
            get { return GetProperty(() => Eta); }
            set { SetProperty(() => Eta, value); }
        }
        public ObservableCollection<SoilLayerInfoBase> SteelAndPercastConcretPileLayerInfoProp
        {
            get { return GetProperty(() => SteelAndPercastConcretPileLayerInfoProp); }
            set { SetProperty(() => SteelAndPercastConcretPileLayerInfoProp, value); }
        }
    }
}
