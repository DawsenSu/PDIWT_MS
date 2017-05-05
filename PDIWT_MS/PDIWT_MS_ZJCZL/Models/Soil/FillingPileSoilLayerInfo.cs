using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDIWT_MS_ZJCZL.Models.Soil
{
     public class FillingPileSoilLayerInfo : SoilLayerInfoBase
    {
        [DisplayName("桩侧阻力尺寸效应系数")]
        public double PsiSi
        {
            get { return GetProperty(() => PsiSi); }
            set { SetProperty(() => PsiSi, value); }
        }
    }
}
