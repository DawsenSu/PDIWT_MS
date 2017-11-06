using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bentley.GeometryNET;
using MathNet.Numerics;
using MathNet.Numerics.Integration;
using PDIWT_MS_PiledWharf.Interface;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using PDIWT_MS_PiledWharf.Models.Soil;

namespace PDIWT_MS_PiledWharf.Models.Piles
{
    public abstract class PileBase :ObservableObject
    {
        //单位m
        private DPoint3d _TopPoint;
        public DPoint3d TopPoint
        {
            get { return _TopPoint; }
            set { Set(ref _TopPoint, value); }
        }
        private DPoint3d _BottomPoint;
        public DPoint3d BottomPoint
        {
            get { return _BottomPoint; }
            set { Set(ref _BottomPoint, value); }
        }
        private string _Code;
        public string Code
        {
            get { return _Code; }
            set { Set(ref _Code, value); }
        }

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set { Set(ref _ID, value); }
        }

        private IPileCrossSection _ICrossSection;
        public IPileCrossSection ICrossSection
        {
            get { return _ICrossSection; }
            set { Set(ref _ICrossSection, value); }
        }

        public abstract double CalculateQd();

        public virtual double CalculateTd(double gammar, double waterlevel, double unitweight = 25, double underwaterunitweight = 15)
        {
            // todo 完成计算结果
            var _pilepieceinfos = GetPilePieceInEachSoilLayerInfos();
            if (_pilepieceinfos.Count == 0) return 0;
            double _accumlatenum = 0;
            foreach (var pilepiece in _pilepieceinfos)
                _accumlatenum += pilepiece.CurrentSoilLayerInfo.Xii * pilepiece.PilePieceLength * pilepiece.CurrentSoilLayerInfo.Qfi;
            //! 以顶端的周长代替整个桩的周长，如变截面需重新编写代码
            return (_ICrossSection.GetOutPerimeter(0) * _accumlatenum + GetActualWeight(unitweight, underwaterunitweight, waterlevel) * GetCosAlpha()) / gammar;

        }

        // 获得桩外周长，国际单位
        protected virtual double GetActualWeight(double unitweight, double underwaterunitweight, double waterlevel)
        {
            double _pileLenght = (_BottomPoint - _TopPoint).Magnitude;
            if (_pileLenght == 0) return -1;
            Func<double, double> f;
            if (waterlevel >= _TopPoint.Z)
            {
                f = x => _ICrossSection.GetActualSectionArea(x / _pileLenght) * underwaterunitweight;
            }
            else if (waterlevel <= _BottomPoint.Z)
            {
                f = x => _ICrossSection.GetActualSectionArea(x / _pileLenght) * unitweight;
            }
            else
            {
                double _uplength = _pileLenght * Math.Abs(_TopPoint.Z - waterlevel) / Math.Abs(_TopPoint.Z - _BottomPoint.Z);
                f = x =>
                {
                    if (x <= _uplength) return _ICrossSection.GetActualSectionArea(x / _pileLenght) * unitweight;
                    else return _ICrossSection.GetActualSectionArea(x / _pileLenght) * underwaterunitweight;
                };
            }
            return NewtonCotesTrapeziumRule.IntegrateAdaptive(f, 0, _pileLenght, 1e-2);
        }

        protected virtual double GetCosAlpha()
        {
            DVector3d _pileVec = _BottomPoint - _TopPoint;
            return _pileVec.AngleTo(DVector3d.FromXYZ(0, 0, -1)).Cos;
        }

        // todo 需要编写程序从模型中提取 (返回数据需要从上到下排序)
        public virtual ObservableCollection<PilePieceInSoilLayerInfo> GetPilePieceInEachSoilLayerInfos()
        {
            return new ObservableCollection<PilePieceInSoilLayerInfo>();
        }

    }
}
