using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;

namespace PDIWT_MS_PiledWharf.Models.Soil
{
    public class SoilLayerInfo : ObservableObject
    {
        // 土层类型名称
        private string _SoilLayerTypeName;
        public string SoilLayerTypeName
        {
            get { return _SoilLayerTypeName; }
            set { Set(ref _SoilLayerTypeName, value); }
        }
        // 土层编号
        private string _SoilLayerNum;
        public string SoilLayerNum
        {
            get { return _SoilLayerNum; }
            set { Set(ref _SoilLayerNum, value); }
        }
        // 极限侧摩阻力标准值qfi(kN)
        private double _Qfi;
        public double Qfi
        {
            get { return _Qfi; }
            set { Set(ref _Qfi, value); }
        }
        // 
        private double _Qri;
        public double Qri
        {
            get { return _Qri; }
            set { Set(ref _Qri, value); }
        }
        // 折减系数ξi
        private double _Xii;
        public double Xii
        {
            get { return _Xii; }
            set { Set(ref _Xii, value); }
        }
        // 侧阻力计算系数ξfi
        private double _Xifi;
        public double Xifi
        {
            get { return _Xifi; }
            set { Set(ref _Xifi, value); }
        }
        // 抗拔折减系数ξfi
        private double _Xifi2;
        public double Xifi2
        {
            get { return _Xifi2; }
            set { Set(ref _Xifi2, value); }
        }
        // 桩侧阻力尺寸效应系数
        private double _PsiSi;
        public double PsiSi
        {
            get { return _PsiSi; }
            set { Set(ref _PsiSi, value); }
        }
        // 阻力增强系数
        private double _Betasi;
        public double Betasi
        {
            get { return _Betasi; }
            set { Set(ref _Betasi, value); }
        }

    }
}
