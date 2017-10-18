using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.XtraPrinting.Native;
using PDIWT_MS_CZ.Models;
using PDIWT_MS_CZ.ViewModels.DetailUserControl;
using PDIWT_MS_Tool.Views;

namespace PDIWT_MS_CZ.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        [Command]
        public void Test()
        {
            MessageBox.Show(CZ_LockHeadParameters.LH_LocalConcertationCulvert.Culvert_Baffle.Count.ToString());
        }

        [BindableProperty]
        public LockHeadParameters CZ_LockHeadParameters { get; set; }


        public MainViewModel()
        {
            CZ_LockHeadParameters = new LockHeadParameters()
            {
                LH_BaseBoard = new BaseBoard()
                {
                    BaseBoardLength = 100,
                    BaseBoardWidth = 200,
                    BaseBoardHeight = 300,
                    EntranceWidth = 400,
                    IsGrooving = true,
                    TGrooving = new ShapeTGrooving()
                    {
                        GroovingHeight = 0,
                        GroovingBackLength = 1,
                        GroovingFrontLength = 2,
                        GroovingWidth = 3,
                        GroovingGradient = 4
                    }
                },
                LH_SidePier = new SidePier()
                {
                    PierHeight = 1,
                    PierXY_A = 2,
                    PierXY_B = 3,
                    PierXY_C = 4,
                    PierXY_D = 5,
                    PierXY_E = 6,
                    PierXY_F = 7,
                    IsChamfered = true,
                    PierChamfer_Tx = 8,
                    PierChamfer_Ty = 9,
                    PierChamfer_R = 10
                },
                LH_DoorSill = new DoorSill()
                {
                    DoorSillHeight = 1,
                    DoorSill_A = 2,
                    DoorSill_B = 3,
                    DoorSill_C = 4,
                    DoorSill_D = 5,
                    DoorSill_E = 6,
                    DoorSill_F = 7
                },
                LH_CulvertChoosenIndex = 1,
                LH_ShortCulvert = new ShortCulvert()
                {
                    Culvert_Pier_BackDis = 1,
                    Culvert_Width = 2,
                    Culvert_A = 3,
                    Culvert_B = 4,
                    Culvert_C = 5,
                    Culvert_D = 6,
                    Culvert_R1 = 7,
                    Culvert_R2 = 8
                },
                LH_LocalConcertationCulvert = new LocalConcertationCulvert()
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
                        GrilleWidthList = new List<GrillInterval>()
                        {
                            new GrillInterval() {Interval = 1},
                            new GrillInterval() {Interval =2},
                            new GrillInterval() {Interval = 3},
                            new GrillInterval() {Interval = 4}
                        }
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
                }
            };
        }
    }
}