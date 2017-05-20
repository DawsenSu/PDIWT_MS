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
    public class FillingPile : PileBase
    {
        public override double CalculateQd()
        {
            if (FillingPileSoilLayerInfoProp == null || FillingPileSoilLayerInfoProp.Count == 0)
                throw new ArgumentNullException($"{PileCode}的PileLayerInfo属性为null或者为empty");
            double accumlatenum = 0;
            foreach (var pilesoil in FillingPileSoilLayerInfoProp)
                accumlatenum += pilesoil.PsiSi * pilesoil.PileInSoilLayerLength * pilesoil.Qfi;
            return (PilePropertyInfo.GetPilePerimeter() * accumlatenum + PsiP * Qr * PilePropertyInfo.GetPileOutLineArea()) / GammaR;
        }
        public override double CalculateTd()
        {
            return base.CalculateTd(FillingPileSoilLayerInfoProp, GammaR);
        }

        public double Qr
        {
            get { return GetProperty(() => Qr); }
            set { SetProperty(() => Qr, value); }
        }
        public double PsiP
        {
            get { return GetProperty(() => PsiP); }
            set { SetProperty(() => PsiP, value); }
        }
        public ObservableCollection<FillingPileSoilLayerInfo> FillingPileSoilLayerInfoProp
        {
            get { return GetProperty(() => FillingPileSoilLayerInfoProp); }
            set { SetProperty(() => FillingPileSoilLayerInfoProp, value); }
        }
        public double GammaR
        {
            get { return GetProperty(() => GammaR); }
            set { SetProperty(() => GammaR, value); }
        }

    }
}
