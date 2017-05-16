using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using PDIWT_MS_ZJCZL.Models.Soil;
namespace PDIWT_MS_ZJCZL.Models.Soil
{
    class PostgroutingFillingPileSoilInfo : SoilLayerInfoBase
    {
        [DisplayName("阻力增强系数")]
        public double BetaSi
        {
            get { return GetProperty(() => BetaSi); }
            set { SetProperty(() => BetaSi, value); }
        }
        [DisplayName("桩侧阻力尺寸效应")]
        public double PsiSi
        {
            get { return GetProperty(() => PsiSi); }
            set { SetProperty(() => PsiSi, value); }
        }
    }
}
