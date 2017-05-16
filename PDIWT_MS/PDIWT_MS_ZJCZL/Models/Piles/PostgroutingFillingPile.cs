using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

using PDIWT_MS_ZJCZL.Interface;
using PDIWT_MS_ZJCZL.Models.Soil;
using DevExpress.Mvvm;

namespace PDIWT_MS_ZJCZL.Models.Piles
{
    [Serializable]
    class PostgroutingFillingPile : PileBase
    {
        public override double CalculateQd()
        {
            if (PostgroutingFillingPileSoilLayerInfoProp == null || PostgroutingFillingPileSoilLayerInfoProp.Count == 0)
                throw new ArgumentNullException($"{PileCode}的PostgroutingFillingPileLayerInfoProp属性为null或者为empty");
            double accumlatenum = 0;
            foreach (var pilesoil in PostgroutingFillingPileSoilLayerInfoProp)
                accumlatenum += pilesoil.BetaSi * pilesoil.PsiSi * pilesoil.Qfi * pilesoil.PileInSoilLayerLength;
            return (PilePropertyInfo.GetPilePerimeter() * accumlatenum + BetaP * PsiP * Qr * PilePropertyInfo.GetPileCrossSectionArea()) / GammaR;
        }

        public override double CalculateQt()
        {
            return base.CalculateQt(PostgroutingFillingPileSoilLayerInfoProp, GammaR);
        }

        public double BetaP
        {
            get { return GetProperty(() => BetaP); }
            set { SetProperty(() => BetaP, value); }
        }
        public double GammaR
        {
            get { return GetProperty(() => GammaR); }
            set { SetProperty(() => GammaR, value); }
        }
        public double PsiP
        {
            get { return GetProperty(() => PsiP); }
            set { SetProperty(() => PsiP, value); }
        }
        public ObservableCollection<PostgroutingFillingPileSoilInfo> PostgroutingFillingPileSoilLayerInfoProp
        {
            get { return GetProperty(() => PostgroutingFillingPileSoilLayerInfoProp); }
            set { SetProperty(() => PostgroutingFillingPileSoilLayerInfoProp, value); }
        }
        public double Qr
        {
            get { return GetProperty(() => Qr); }
            set { SetProperty(() => Qr, value); }
        }

    }
}
