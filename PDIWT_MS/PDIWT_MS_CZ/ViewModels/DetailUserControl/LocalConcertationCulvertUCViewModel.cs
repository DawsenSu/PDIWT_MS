using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bentley.DgnPlatformNET;
using DevExpress.Mvvm;
using PDIWT_MS_CZ.Models;

namespace PDIWT_MS_CZ.ViewModels.DetailUserControl
{
    public class LocalConcertationCulvertUCViewModel : ViewModelBase
    {
        public LocalConcertationCulvert CZ_LocalConcertationCulvert
        {
            get { return GetProperty(() => CZ_LocalConcertationCulvert); }
            set { SetProperty(() => CZ_LocalConcertationCulvert, value); }
        }
        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            CZ_LocalConcertationCulvert = new LocalConcertationCulvert()
            {
                Culvert_Height = 1,
                Culvert_Pier_BackDis = 2,
                Culvert_A = 3,
                Culvert_B = 4,
                Culvert_C = 5,
                Culvert_D = 6,
                Culvert_E = 7,
                Culvert_F = 8,
                IsChamfered = true,
                Culvert_Chamfer_R1 = 9,
                Culvert_Chamfer_R2 = 10,
                Culvert_Chamfer_R3 = 11,
                Culvert_Chamfer_R4 = 12,
                IsIncludeWaterDivision = true,
                Culvert_WaterDivision = new WaterDivision()
                {
                    WaterDivision_A = 1,
                    WaterDivision_B = 2,
                    WaterDivision_R1 = 3,
                    WaterDivision_R2 = 4,
                    WaterDivision_R3 = 5
                },
                IsIncludeEnergyDisspater = true,
                Culvert_EnergyDisspater = new EnergyDisspater()
                {
                    Grille_TwolineInterval = 1,
                    GrilleWidthList = new List<double>() {1, 2, 3, 4, 5}
                },
                IsIncludeBaffle = true,
                Culvert_Baffle = new List<Baffle>()
                {
                    new Baffle() {Baffle_MidMidDis = 1, Baffle_Height = 2, Baffle_Width = 3},
                    new Baffle() {Baffle_MidMidDis = 12, Baffle_Height = 22, Baffle_Width = 32},
                    new Baffle() {Baffle_MidMidDis = 13, Baffle_Height = 23, Baffle_Width = 33},
                    new Baffle() {Baffle_MidMidDis = 14, Baffle_Height = 24, Baffle_Width = 34},
                    new Baffle() {Baffle_MidMidDis = 15, Baffle_Height = 25, Baffle_Width = 35}
                }
            };
        }
    }
}