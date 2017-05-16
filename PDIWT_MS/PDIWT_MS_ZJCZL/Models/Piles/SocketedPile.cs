using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using PDIWT_MS_ZJCZL.Interface;
using PDIWT_MS_ZJCZL.Models.Soil;
using DevExpress.Mvvm;


namespace PDIWT_MS_ZJCZL.Models.Piles
{
    class SocketedPile : PileBase, IPileBearingCapacity
    {
        public double CalculateQd()
        {
            if (SocketedPileSoilLayerInfoProp == null || SocketedPileSoilLayerInfoProp.Count == 0)
                throw new ArgumentNullException($"{PileCode}的SocketedPileSoilInfoProp属性为null或者为empty");
            double accumlatenum = 0;
            foreach (var pilesoil in SocketedPileSoilLayerInfoProp)
                accumlatenum += pilesoil.Xifi * pilesoil.Qfi * pilesoil.PileInSoilLayerLength;
            return PilePropertyInfo.GetPilePerimeter() * accumlatenum / GammaCs + (PilePropertyInfo.GetPilePerimeter() * Xis * Hr + Xip * PilePropertyInfo.GetPileCrossSectionArea()) * Frk / GammaCr;
        }

        public double CalculateQt()
        {
            if (Hr >= 3 * PilePropertyInfo.PileDiameter)
            {
                if (SocketedPileSoilLayerInfoProp == null || SocketedPileSoilLayerInfoProp.Count == 0)
                    throw new ArgumentNullException($"{PileCode}的SocketedPileSoilInfoProp属性为null或者为empty");
                double accumlatenum = 0;
                foreach (var pilesoil in SocketedPileSoilLayerInfoProp)
                    accumlatenum += pilesoil.Xifi2 * pilesoil.Xifi * pilesoil.Qfi * pilesoil.PileInSoilLayerLength;
                return (PilePropertyInfo.GetPilePerimeter() * accumlatenum + PilePropertyInfo.GetPileGravity() * PilePropertyInfo.GetCosAlpha()) / GammaTs + PilePropertyInfo.GetPilePerimeter() * Xis2 * Frk * Hr / GammaTr;
            }
            else
                throw new ApplicationException(" 嵌入深度小于3倍桩径，无法计算Qt");
        }

        public double Frk
        {
            get { return GetProperty(() => Frk); }
            set { SetProperty(() => Frk, value); }
        }
        public double GammaCr
        {
            get { return GetProperty(() => GammaCr); }
            set { SetProperty(() => GammaCr, value); }
        }
        public double GammaCs
        {
            get { return GetProperty(() => GammaCs); }
            set { SetProperty(() => GammaCs, value); }
        }
        public double Hr
        {
            get { return GetProperty(() => Hr); }
            set { SetProperty(() => Hr, value); }
        }
        public ObservableCollection<SocketedPileSoilInfo> SocketedPileSoilLayerInfoProp
        {
            get { return GetProperty(() => SocketedPileSoilLayerInfoProp); }
            set { SetProperty(() => SocketedPileSoilLayerInfoProp, value); }
        }
        public double Xip
        {
            get { return GetProperty(() => Xip); }
            set { SetProperty(() => Xip, value); }
        }
        public double Xis
        {
            get { return GetProperty(() => Xis); }
            set { SetProperty(() => Xis, value); }
        }
        public double Xis2
        {
            get { return GetProperty(() => Xis2); }
            set { SetProperty(() => Xis2, value); }
        }
        public double GammaTs
        {
            get { return GetProperty(() => GammaTs); }
            set { SetProperty(() => GammaTs, value); }
        }
        public double GammaTr
        {
            get { return GetProperty(() => GammaTr); }
            set { SetProperty(() => GammaTr, value); }
        }
    }
}
