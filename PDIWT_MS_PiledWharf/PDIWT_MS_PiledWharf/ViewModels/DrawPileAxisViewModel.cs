using System;
using System.Collections;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;

namespace PDIWT_MS_PiledWharf.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class DrawPileAxisViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the DrawPileAxisViewModel class.
        /// </summary>
        public DrawPileAxisViewModel()
        {
            _PileTypes = new ObservableCollection<string>()
            {
                "实心桩或桩端封闭",
                "钢管桩与预制混凝土管桩"
            };
            _SelectedPileType = _PileTypes[0];
            _PileCrossSectionTypes = new ObservableCollection<string>()
            {
                "环形截面",
                "方形截面",
                "方形圆孔截面",
                "多边形截面"
            };
            _SelectedCrossSectionType = _PileCrossSectionTypes[0];
            _PileWeight = 25;
            _PileUnderWaterWeight = 15;
        }

        private ObservableCollection<string> _PileTypes;
        public ObservableCollection<string> PileTypes
        {
            get { return _PileTypes; }
            set { Set(ref _PileTypes, value); }
        }

        private string _SelectedPileType;
        public string SelectedPileType
        {
            get { return _SelectedPileType; }
            set { Set(ref _SelectedPileType, value); }
        }

        private ObservableCollection<string> _PileCrossSectionTypes;
        public ObservableCollection<string> PileCrossSectionTypes
        {
            get { return _PileCrossSectionTypes; }
            set { Set(ref _PileCrossSectionTypes, value); }
        }
        private string _SelectedCrossSectionType;
        public string SelectedCrossSectionType
        {
            get { return _SelectedCrossSectionType; }
            set
            {
                Set(ref _SelectedCrossSectionType, value);
                RaisePropertyChanged(() => IsPileSideLengthShow);
                RaisePropertyChanged(() => IsPileOutterDiameterShow);
                RaisePropertyChanged(() => IsPileInnerDiameterShow);

            }
        }

        private string _PileGridHorizontal;
        public string PileGridHorizontal
        {
            get { return _PileGridHorizontal; }
            set { Set(ref _PileGridHorizontal, value); }
        }

        private string _PileGridVertical;
        public string PileGridVertical
        {
            get { return _PileGridVertical; }
            set { Set(ref _PileGridVertical, value); }
        }

        private double _PileSideLength;
        public double PileSideLength
        {
            get { return _PileSideLength; }
            set { Set(ref _PileSideLength, value); }
        }

        public bool IsPileSideLengthShow => _SelectedCrossSectionType == "方形截面" || _SelectedCrossSectionType == "方形圆孔截面";


        private double _PileOutterDiameter;
        public double PileOutterDiameter
        {
            get { return _PileOutterDiameter; }
            set { Set(ref _PileOutterDiameter, value); }
        }

        public bool IsPileOutterDiameterShow => _SelectedCrossSectionType == "环形截面";

        private double _PileInnerDiameter;
        public double PileInnerDiameter
        {
            get { return _PileInnerDiameter; }
            set { Set(ref _PileInnerDiameter, value); }
        }

        public bool IsPileInnerDiameterShow => _SelectedCrossSectionType == "环形截面" || _SelectedCrossSectionType == "方形圆孔截面";

        private double _PileWeight;
        public double PileWeight
        {
            get { return _PileWeight; }
            set { Set(ref _PileWeight, value); }
        }

        private double _PileUnderWaterWeight;
        public double PileUnderWaterWeight
        {
            get { return _PileUnderWaterWeight; }
            set { Set(ref _PileUnderWaterWeight, value); }
        }


    }
}