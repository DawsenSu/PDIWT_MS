using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using PDIWT_MS_PiledWharf.Extension.Attribute;
using PDIWT_MS_PiledWharf.Models.Piles.CrossSection;

namespace PDIWT_MS_PiledWharf.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    using Models;

    public class DrawPileAxisViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the DrawPileAxisViewModel class.
        /// </summary>
        public DrawPileAxisViewModel()
        {
            _PileTypes = EnumDisPlayNameHelper.GetPileTypeNameList().ToObservableCollection();
            _SelectedPileType = _PileTypes[0];
            _PileCrossSectionTypes = EnumDisPlayNameHelper.GetPileCrossSectionTypeNameList().ToObservableCollection();
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

        public bool IsPileSideLengthShow => _SelectedCrossSectionType == GetCrossTypeNameString(typeof(SquareCrossSection)) || _SelectedCrossSectionType == GetCrossTypeNameString(typeof(SquareWithRoundHoleCrossSection));


        private double _PileOutterDiameter;
        public double PileOutterDiameter
        {
            get { return _PileOutterDiameter; }
            set { Set(ref _PileOutterDiameter, value); }
        }

        public bool IsPileOutterDiameterShow => _SelectedCrossSectionType == GetCrossTypeNameString(typeof(AnnularCrossSection));

        private double _PileInnerDiameter;
        public double PileInnerDiameter
        {
            get { return _PileInnerDiameter; }
            set { Set(ref _PileInnerDiameter, value); }
        }

        public bool IsPileInnerDiameterShow => _SelectedCrossSectionType == GetCrossTypeNameString(typeof(AnnularCrossSection)) || _SelectedCrossSectionType == GetCrossTypeNameString(typeof(SquareWithRoundHoleCrossSection));

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

        string GetCrossTypeNameString(Type crosssectiontype)
        {
            var attrri = crosssectiontype.GetCustomAttribute<EnumDisplayNameAttribute>();
           return attrri != null ? attrri.DisplayName : crosssectiontype.Name;
        }

        public PileAxisInfo ToInfo()
        {
            PileAxisInfo info = new PileAxisInfo();
            info.GridHorizontal = _PileGridHorizontal;
            info.GridVertical = _PileGridVertical;
            info.Type = _SelectedPileType;
            info.CrossSectionType = _SelectedCrossSectionType;
            if (_SelectedCrossSectionType == GetCrossTypeNameString(typeof(AnnularCrossSection)))
            {
                info.SideLength = _PileOutterDiameter;
                info.InnerDiameter = _PileInnerDiameter;
            }
            else if(_SelectedCrossSectionType == GetCrossTypeNameString(typeof(SquareCrossSection)))
            {
                info.SideLength = _PileSideLength;
                info.InnerDiameter = 0;
            }
            else if(_SelectedCrossSectionType == GetCrossTypeNameString(typeof(SquareWithRoundHoleCrossSection)))
            {
                info.SideLength = _PileSideLength;
                info.SideLength = _PileInnerDiameter;
            }
            info.Weight = _PileWeight;
            info.UnderWaterWeight = _PileUnderWaterWeight;
            return info;
        }

    }
}