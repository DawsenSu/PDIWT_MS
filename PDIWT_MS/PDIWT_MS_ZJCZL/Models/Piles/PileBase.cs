using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;

using PDIWT_MS_ZJCZL.Interface;
using PDIWT_MS_ZJCZL.Models.Soil;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace PDIWT_MS_ZJCZL.Models.Piles
{
    public abstract class PileBase : BindableBase
    {
        protected virtual double CalculateQt<T>(ObservableCollection<T> pileLayerInfoProp, double gammaR) where T : SoilLayerInfoBase
        {
            if (pileLayerInfoProp == null || pileLayerInfoProp.Count == 0)
                throw new ArgumentNullException($"{PileCode}的PileLayerInfo属性为null或者为empty");
            double accumlatenum = 0;
            foreach (var pilesoil in pileLayerInfoProp)
                accumlatenum += pilesoil.Xii * pilesoil.PileInSoilLayerLength * pilesoil.Qfi;
            return (PilePropertyInfo.GetPilePerimeter() * accumlatenum + PilePropertyInfo.GetPileGravity() * PilePropertyInfo.GetCosAlpha()) / gammaR;
        }

        public string PileCode
        {
            get { return GetProperty(() => PileCode); }
            set { SetProperty(() => PileCode, value); }
        }
        public long PileId
        {
            get { return GetProperty(() => PileId); }
            set { SetProperty(() => PileId, value); }
        }
        public IPileProperty PilePropertyInfo
        {
            get { return GetProperty(() => PilePropertyInfo); }
            set { SetProperty(() => PilePropertyInfo, value); }
        }
    }
}
