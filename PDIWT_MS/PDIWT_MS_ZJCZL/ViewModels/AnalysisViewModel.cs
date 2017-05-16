using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;

using PDIWT_MS_ZJCZL.Interface;
using PDIWT_MS_ZJCZL.Models.Piles;
using System.ComponentModel;


namespace PDIWT_MS_ZJCZL.ViewModels
{
    public class AnalysisViewModel : ViewModelBase
    {
        public AnalysisViewModel(ObservableCollection<PileBase> piles)
        {
            this.Piles = piles;
        }

        public List<PileAnalysisInfo> GetPileAnalysisData()
        {
            var pileAnalysisData = new List<PileAnalysisInfo>();
            foreach (var pile in Piles)
            {
                double holdlength = 0;
                if (pile is SolidPile)
                    holdlength = ((SolidPile)pile).SolidPileSoilLayerInfoProp.Last().PileInSoilLayerLength;
                else if (pile is FillingPile)
                    holdlength = ((FillingPile)pile).FillingPileSoilLayerInfoProp.Last().PileInSoilLayerLength;
                pileAnalysisData.Add(new PileAnalysisInfo()
                {
                    PileCode = pile.PileCode,
                    Force = 0,
                    PileBearingForce = ((IPileBearingCapacity)pile).CalculateQd(),
                    PilePullingForce = ((IPileBearingCapacity)pile).CalculateQt(),
                    PileHoldLength = holdlength,
                    PileLength = pile.PilePropertyInfo.GetPileLength(),
                    PileDiameter = pile.PilePropertyInfo.PileDiameter
                });
            }
            return pileAnalysisData;
        }

        public IEnumerable GetPileHoldLengthVsD()
        {
            var data = GetPileAnalysisData();
            var result = from pile in data
                         let lvsd = pile.PileHoldLength / pile.PileDiameter
                         select new { PileCode = pile.PileCode, PileLVSD = lvsd, PileLength = pile.PileLength };
            return result;
        }

        //public ObservableCollection<PileAnalysisInfo> PileAnalysisData { get; set; }
        //{
        //    get { return GetProperty(() => PileAnalysisData); }
        //    set { SetProperty(() => PileAnalysisData, value); }
        //}

        public ObservableCollection<PileBase> Piles
        {
            get { return GetProperty(() => Piles); }
            set { SetProperty(() => Piles, value); }
        }
    }

    public class PileAnalysisInfo
    {
        public string PileCode { get; set; }
        public double Force { get; set; }
        public double PileBearingForce { get; set; }
        public double PilePullingForce { get; set; }
        public double PileHoldLength { get; set; }
        public double PileLength { get; set; }
        public double PileDiameter { get; set; }
    }
}