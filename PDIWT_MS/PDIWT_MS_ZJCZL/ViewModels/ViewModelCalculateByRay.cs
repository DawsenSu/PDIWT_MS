using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;

using HCHXCodeQueryLib;
using PDIWT_MS_ZJCZL.Interface;
using PDIWT_MS_ZJCZL.Models;
using PDIWT_MS_ZJCZL.Models.Piles;
using PDIWT_MS_ZJCZL.Models.Soil;
using System.Linq;

namespace PDIWT_MS_ZJCZL.ViewModels
{
    public class ViewModelCalculateByRay : ViewModelBase
    {        
        public ObservableCollection<PileBase> Piles
        {
            get { return GetProperty(() => Piles); }
            set { SetProperty(() => Piles, value); }
        }

        public PileBase CurrentPile
        {
            get { return GetProperty(() => CurrentPile); }
            set
            {
                if (SetProperty(() => CurrentPile, value))
                {
                    Qd = -1;
                    Qt = -1;
                }
            }
        }

        public double Qd
        {
            get { return GetProperty(() => Qd); }
            set { SetProperty(() => Qd, value); }
        }
        public double Qt
        {
            get { return GetProperty(() => Qt); }
            set { SetProperty(() => Qt, value); }
        }

        [Command]
        public void Calculate()
        {
            var ipile = CurrentPile as IPileBearingCapacity;
            if (ipile!=null)
            {
                Qd = ipile.CalculateQd();
                Qt = ipile.CalculateQt();
            }
        }
        [Command]
        public void AddPile()
        {
            //var AddPileViewModel = new ViewAddPileViewModel();
            //AddPileViewModel.Piles = Piles;
            //var addpileview = new Views.ViewAddPile();
            //addpileview.DataContext = AddPileViewModel;
            //addpileview.ShowDialog();
            Piles.Add(new SolidPile
            {
                PileId = new Random().Next(),
                PileCode = "Test" + new Random().Next().ToString(),
                PilePropertyInfo = new RoundnessPileGeometry()
                {
                    PileBottomPoint = new Point3d { X = 1, Y = 1, Z = 1 },
                    PileTopPoint = new Point3d { X = 1, Y = 1, Z = 0 },
                    PileDiameter = 10,
                    WaterLevel = 0.5,
                    PileInnerDiameter = 0,
                    PileUnderWaterWeight = 15,
                    PileWeight = 25
                },
                GammaR = 1.2,
                Qr = 10,
                SolidPileSoilLayerInfoProp = new ObservableCollection<SoilLayerInfoBase>
                    {
                        new SoilLayerInfoBase { SoilLayerName = "Tes1", SoilLayerNum = "0-0-1", PileInSoilLayerLength =0.5, Qfi =1, Xii=1 },
                        new SoilLayerInfoBase { SoilLayerName = "Tes2", SoilLayerNum = "0-0-2", PileInSoilLayerLength =0.1, Qfi =21, Xii=13 },
                        new SoilLayerInfoBase { SoilLayerName = "Tes3", SoilLayerNum = "0-0-3", PileInSoilLayerLength =0.2, Qfi =13, Xii=14 }
                    }
            });
        }

        [Command]
        public void RemovePile()
        {
            Piles.Remove(CurrentPile);
        }
        public bool CanRemovePile()
        {
            return Piles.Count > 0;
        }
                
        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            Piles = new ObservableCollection<PileBase>
            {
                new SolidPile
                {
                    PileId = 924,
                    PileCode = "Test1",
                    PilePropertyInfo =  new RoundnessPileGeometry()
                    {
                      PileBottomPoint = new Point3d { X=1, Y=1,Z =1 },
                      PileTopPoint = new Point3d { X=1, Y=1,Z =0 },
                      PileDiameter =10,
                      WaterLevel = 0.5,
                      PileInnerDiameter = 0,
                      PileUnderWaterWeight =15,
                      PileWeight = 25
                    },
                    GammaR = 1.2,
                    Qr = 10,
                    SolidPileSoilLayerInfoProp= new ObservableCollection<SoilLayerInfoBase>
                    {
                        new SoilLayerInfoBase { SoilLayerName = "Tes1", SoilLayerNum = "0-0-1", PileInSoilLayerLength =0.5, Qfi =1, Xii=1 },
                        new SoilLayerInfoBase { SoilLayerName = "Tes2", SoilLayerNum = "0-0-2", PileInSoilLayerLength =0.1, Qfi =21, Xii=13 },
                        new SoilLayerInfoBase { SoilLayerName = "Tes3", SoilLayerNum = "0-0-3", PileInSoilLayerLength =0.2, Qfi =13, Xii=14 }
                    }
                },
                new FillingPile
                {
                    PileId = 9,
                    PileCode = "Test2",
                    PilePropertyInfo =  new SquarePileGeometry()
                    {
                      PileBottomPoint = new Point3d { X=1, Y=1,Z =1 },
                      PileTopPoint = new Point3d { X=1, Y=1,Z =0 },
                      PileDiameter =10,
                      WaterLevel = 0.5,
                      PileInnerDiameter = 0,
                      PileUnderWaterWeight =15,
                      PileWeight = 25
                    },
                    GammaR = 1.6,
                    Qr = 11,
                    FillingPileSoilLayerInfoProp = new ObservableCollection<FillingPileSoilLayerInfo>
                    {
                        new FillingPileSoilLayerInfo { SoilLayerName = "Tes11", SoilLayerNum = "0-1-1", PileInSoilLayerLength =0.2, Qfi =3, Xii=7, PsiSi =10 },
                        new FillingPileSoilLayerInfo { SoilLayerName = "Tes12", SoilLayerNum = "0-1-2", PileInSoilLayerLength =0.3, Qfi =2, Xii=7, PsiSi =10 },
                        new FillingPileSoilLayerInfo { SoilLayerName = "Tes13", SoilLayerNum = "0-1-3", PileInSoilLayerLength =0.4, Qfi =6, Xii=7, PsiSi =10 }
                    },
                    PsiP=1
                }
            };
            CurrentPile = Piles[0];
            //AddPileViewModel = new ViewAddPileViewModel();
            //((ISupportParameter)AddPileViewModel).Parameter = Piles;
        }
    }
}