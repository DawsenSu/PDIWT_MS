using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PDIWT_MS_ZJCZL.Interface;
using PDIWT_MS_ZJCZL.Models.Soil;

namespace PDIWT_MS_ZJCZL.Models.Piles
{
    [Serializable]
    class SolidPile : PileBase
    {
        public override double CalculateQd()
        {
            if (SolidPileSoilLayerInfoProp == null || SolidPileSoilLayerInfoProp.Count == 0)
                throw new ArgumentNullException($"{PileCode}的PileLayerInfo属性为null或者为empty");
            double accumlatenum = 0;
            foreach (var pilesoil in SolidPileSoilLayerInfoProp)
                accumlatenum += pilesoil.PileInSoilLayerLength * pilesoil.Qfi;
            return (PilePropertyInfo.GetPilePerimeter() * accumlatenum + Qr * PilePropertyInfo.GetPileOutLineArea()) / GammaR;
        }
        public override double CalculateTd()
        {
            return base.CalculateTd(SolidPileSoilLayerInfoProp, GammaR);
        }
        public double Qr
        {
            get { return GetProperty(() => Qr); }
            set { SetProperty(() => Qr, value); }
        }
        public ObservableCollection<SoilLayerInfoBase> SolidPileSoilLayerInfoProp
        {
            get { return GetProperty(() => SolidPileSoilLayerInfoProp); }
            set { SetProperty(() => SolidPileSoilLayerInfoProp, value); }
        }
        public double GammaR
        {
            get { return GetProperty(() => GammaR); }
            set { SetProperty(() => GammaR, value); }
        }
    }
}
