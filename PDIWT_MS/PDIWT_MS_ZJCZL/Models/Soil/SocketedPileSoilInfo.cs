using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using DevExpress.Mvvm;

namespace PDIWT_MS_ZJCZL.Models.Soil
{
    class SocketedPileSoilInfo : SoilLayerInfoBase
    {
        [DisplayName("侧阻力计算系数ξfi")]
        public double Xifi
        {
            get { return GetProperty(() => Xifi); }
            set { SetProperty(() => Xifi, value); }
        }
        [DisplayName("抗拔折减系数ξfi'")]
        public double Xifi2
        {
            get { return GetProperty(() => Xifi2); }
            set { SetProperty(() => Xifi2, value); }
        }

    }
}
