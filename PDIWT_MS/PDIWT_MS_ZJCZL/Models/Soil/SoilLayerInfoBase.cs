using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using DevExpress.Mvvm;
using System.ComponentModel.DataAnnotations;

namespace PDIWT_MS_ZJCZL.Models.Soil
{
    public class SoilLayerInfoBase : BindableBase
    {
        [DisplayName("土层名称"), ReadOnly(true),Display(Order = 0)]
        public string SoilLayerName
        {
            get { return GetProperty(() => SoilLayerName); }
            set { SetProperty(() => SoilLayerName, value); }
        }
        [DisplayName("土层编号"), ReadOnly(true), Display(Order = 1)]
        public string SoilLayerNum
        {
            get { return GetProperty(() => SoilLayerNum); }
            set { SetProperty(() => SoilLayerNum, value); }
        }
        [DisplayName("桩在此土层内的长度m"), ReadOnly(true), Display(Order = 2)]
        public double PileInSoilLayerLength // unit :m
        {
            get { return GetProperty(() => PileInSoilLayerLength); }
            set { SetProperty(() => PileInSoilLayerLength, value); }
        }
        [DisplayName("极限侧摩阻力标准值qfi(kN)"), Display(Order = 3)]
        public double Qfi
        {
            get { return GetProperty(() => Qfi); }
            set { SetProperty(() => Qfi, value); }
        }
        [DisplayName("阻力计算系数"), Display(Order = 4)]
        public double Xii
        {
            get { return GetProperty(() => Xii); }
            set { SetProperty(() => Xii, value); }
        }
        
    }
}
