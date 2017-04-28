using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;

using HCHXCodeQueryLib;
using PDIWT_MS_ZJCZL.Models;
using System.Linq;

namespace PDIWT_MS_ZJCZL.ViewModels
{
    public class ViewModelCalculateByRay : ViewModelBase
    {        
        public ObservableCollection<PileInfoClass> Piles
        {
            get { return GetProperty(() => Piles); }
            set { SetProperty(() => Piles, value); }
        }

        public ViewAddPileViewModel AddPileViewModel { get; private set; }


        [Command]
        public void Calculate()
        {
        }
        [Command]
        public void AddPile()
        {
            AddPileViewModel = new ViewAddPileViewModel();
            AddPileViewModel.Piles = Piles;
            var addpileview = new Views.ViewAddPile();
            addpileview.DataContext = AddPileViewModel;
            addpileview.ShowDialog();
        }

        [Command]
        public void RemovePile(PileInfoClass pileinfo)
        {
            Piles.Remove(pileinfo);
        }
        public bool CanRemovePile(PileInfoClass pileinfo)
        {
            return Piles.Count > 0;
        }

        //public string GetPileTypeString(PileType pt)
        //{
        //    string piletypestring = string.Empty;
        //    switch (pt)
        //    {
        //        case PileType.Solid:
        //            piletypestring = "实心桩或桩端封闭";
        //            break;
        //        case PileType.SteelAndPercastConcrete:
        //            piletypestring = "管桩";
        //            break;
        //        case PileType.Filling:
        //            piletypestring = "灌注桩";
        //            break;
        //        case PileType.Socketed:
        //            piletypestring = "嵌岩桩";
        //            break;
        //        case PileType.PostgroutingFilling:
        //            piletypestring = "后注浆灌注桩";
        //            break;
        //    }
        //    return piletypestring;
        //}

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            Piles = new ObservableCollection<PileInfoClass>
            {
                new PileInfoClass
                {
                    PileId = 924,
                    PileCode = "Test1",
                    PileDiameter = 10,
                    PileTypeInfo = PileType.Filling,
                    SoilInfo = new ObservableCollection<SoilInfoClass>()
                    {
                        new SoilInfoClass() { SoilLayerName="layer1", SoilLayerNum="0-0" },
                        new SoilInfoClass() { SoilLayerName="layer2", SoilLayerNum="0-1" }
                    },
                    CalParameter = new CalculateParameter { GammaR = 1.2 },
                    Result = new CalculateResult { UltimateBearingCapacity= 100, UltimatePullingCapacity =100 }
                },
                new PileInfoClass
                {
                    PileId = 100,
                    PileCode = "Test2",
                    PileDiameter = 12,
                    PileTypeInfo = PileType.PostgroutingFilling,
                    SoilInfo = new ObservableCollection<SoilInfoClass>()
                    {
                        new SoilInfoClass() { SoilLayerName="layer3", SoilLayerNum="1-0" },
                        new SoilInfoClass() { SoilLayerName="layer4", SoilLayerNum="1-1" }
                    },
                    CalParameter = new CalculateParameter { GammaR = 1.3 },
                    Result = new CalculateResult { UltimateBearingCapacity = 200, UltimatePullingCapacity = 200 }
                }
            };
            //AddPileViewModel = new ViewAddPileViewModel();
            //((ISupportParameter)AddPileViewModel).Parameter = Piles;
        }
    }
}