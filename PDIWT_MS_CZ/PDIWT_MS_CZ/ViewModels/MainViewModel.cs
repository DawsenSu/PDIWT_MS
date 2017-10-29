using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

using Bentley.DgnPlatformNET;
using Bentley.GeometryNET;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.CommandWpf;

using ExtendedXmlSerialization;

using PDIWT_MS_CZ.Models;
using PDIWT_MS_CZ.Properties;

using MessageBox = System.Windows.MessageBox;


namespace PDIWT_MS_CZ.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _prompt;
        public string Prompt
        {
            get { return _prompt; }
            set { Set(ref _prompt, value); }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set { Set(ref _status, value); }
        }

        private LockHeadParameters _cz_lockheadparameters;
        public LockHeadParameters CZ_LockHeadParameters
        {
            get { return _cz_lockheadparameters; }
            set { Set(ref _cz_lockheadparameters, value); }
        }
        private bool _IsVerified;
        public bool IsVerified
        {
            get { return _IsVerified; }
            set { Set(ref _IsVerified, value); }
        }


        public MainViewModel()
        {
            InitialzeParameters();
            IsVerified = false;
            Prompt = "模块加载成功";
            Status = Resources.Status_Success;
            Messenger.Default.Register<bool>(this, "ParameterChanged", _ => 
            {
                Prompt = "参数集设置发生变化，请重新验证参数";
                IsVerified = false;
            });
        }
        ~MainViewModel()
        {
            Messenger.Default.Unregister(this);
        }

        //船闸模块参数CZ_LockHeadParameters赋初值
        private void InitialzeParameters()
        {
            CZ_LockHeadParameters = new LockHeadParameters()
            {
                LH_BaseBoard = new BaseBoard()
                {
                    BaseBoardLength = 28000,
                    BaseBoardWidth = 40000,
                    BaseBoardHeight = 3000,
                    EntranceWidth = 2300,
                    IsGrooving = true,
                    TGrooving = new ShapeTGrooving()
                    {
                        GroovingHeight = 2000,
                        GroovingBackLength = 8000,
                        GroovingFrontLength = 8000,
                        GroovingWidth = 22000,
                        GroovingGradient = 1
                    }
                },
                LH_SidePier = new SidePier()
                {
                    PierHeight = 17200,
                    PierXY_A = 8500,
                    PierXY_B = 2100,
                    PierXY_C = 2100,
                    PierXY_D = 10000,
                    PierXY_E = 16000,
                    PierXY_F = 2000,
                    IsChamfered = true,
                    PierChamfer_Tx = 1387,
                    PierChamfer_Ty = 1387,
                    PierChamfer_R = 1000
                },
                LH_DoorSill = new DoorSill()
                {
                    DoorSillHeight = 4300,
                    DoorSill_A = 23000,
                    DoorSill_B = 2260,
                    DoorSill_C = 6650,
                    DoorSill_D = 10000,
                    DoorSill_E = 2248.2215,
                    DoorSill_F = 2754.7785
                },
                LH_CulvertChoosenIndex = 1,
                LH_ShortCulvert = new ShortCulvert()
                {
                    Culvert_Pier_RightDis = 1800,
                    Culvert_Baseboard_BottomDis = 2000,
                    Culvert_Width = 3500,
                    Culvert_A = 700,
                    Culvert_B = 300,
                    Culvert_C = 16000,
                    Culvert_D = 2000,
                    Culvert_R1 = 2000,
                    Culvert_R2 = 4600
                },
                LH_LocalConcertationCulvert = new LocalConcertationCulvert()
                {
                    Culvert_Height = 3500,
                    Culvert_Pier_BackDis = 1500,
                    Culvert_A = 18200,
                    Culvert_B = 2600,
                    Culvert_C = 4600,
                    Culvert_D = 6300,
                    Culvert_E = 24500,
                    Culvert_F = 4700,
                    IsChamfered = true,
                    Culvert_Chamfer_R1 = 6400,
                    Culvert_Chamfer_R2 = 1800,
                    Culvert_Chamfer_R3 = 1000,
                    Culvert_Chamfer_R4 = 1000,
                    IsIncludeWaterDivision = true,
                    Culvert_WaterDivision = new WaterDivision()
                    {
                        WaterDivision_A = 650,
                        WaterDivision_B = 0,
                        WaterDivision_R1 = 4400,
                        WaterDivision_R2 = 4700,
                        WaterDivision_R3 = 250
                    },
                    IsIncludeEnergyDisspater = true,
                    Culvert_EnergyDisspater = new EnergyDisspater()
                    {
                        Grille_TwolineInterval = 500,
                        GrilleWidthList = new ObservableCollection<GrillInterval>()
                        {
                            new GrillInterval() {Interval = 250, RoundChamferRadius = 100},
                            new GrillInterval() {Interval =300 ,RoundChamferRadius = 0},
                            new GrillInterval() {Interval = 500, RoundChamferRadius = 100},
                            new GrillInterval() {Interval =300 ,RoundChamferRadius = 0},
                            new GrillInterval() {Interval = 500, RoundChamferRadius = 100},
                            new GrillInterval() {Interval =300 ,RoundChamferRadius = 0},
                            new GrillInterval() {Interval = 500, RoundChamferRadius = 100},
                            new GrillInterval() {Interval = 500 ,RoundChamferRadius = 0},
                            new GrillInterval() {Interval = 500, RoundChamferRadius = 100},
                            new GrillInterval() {Interval = 500 ,RoundChamferRadius = 0},
                            new GrillInterval() {Interval = 500, RoundChamferRadius = 100},
                            new GrillInterval() {Interval = 800 ,RoundChamferRadius = 0},
                            new GrillInterval() {Interval = 800, RoundChamferRadius = 100},
                            new GrillInterval() {Interval = 800 ,RoundChamferRadius = 0},
                            new GrillInterval() {Interval = 800, RoundChamferRadius = 100},
                            new GrillInterval() {Interval = 800 ,RoundChamferRadius = 0},
                            new GrillInterval() {Interval = 800, RoundChamferRadius = 100},
                            new GrillInterval() {Interval = 1000, RoundChamferRadius = 0}
                        }
                    },
                    IsIncludeBaffle = true,
                    Culvert_Baffle = new ObservableCollection<Baffle>()
                    {
                        new Baffle() {Baffle_MidMidDis = 0, Baffle_Height = 2000, Baffle_Width = 250},
                        new Baffle() {Baffle_MidMidDis = 4500, Baffle_Height = 1500, Baffle_Width = 500},
                        new Baffle() {Baffle_MidMidDis = 9000, Baffle_Height = 1000, Baffle_Width = 500}
                    }
                },
                LH_EmptyRectBoxs = new ObservableCollection<RectEmptyBox>()
                {
                    new RectEmptyBox()  //1
                    {
                        XDis = 800,
                        YDis = 900,
                        ZDis = 300,
                        EmptyBoxLength = 4200,
                        EmptyBoxWidth = 6900,
                        EmptyBoxHeight = 8400,
                        ChamferInfos = new ObservableCollection<EmptyBoxEdgeChameferInfo>()
                        {
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 0,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 1,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = false,
                                EdgeIndicator = 2,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = false,
                                EdgeIndicator = 3,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 4,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = false,
                                EdgeIndicator = 5,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = false,
                                EdgeIndicator = 6,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 7,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 8,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 9,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 10,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 11,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            }
                        }
                    },
                    new RectEmptyBox() //2
                    {
                        XDis = 800,
                        YDis = 10900,
                        ZDis = 300,
                        EmptyBoxLength = 6000,
                        EmptyBoxWidth = 4800,
                        EmptyBoxHeight = 2200,
                        ChamferInfos = new ObservableCollection<EmptyBoxEdgeChameferInfo>()
                        {
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 0,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 1,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = false,
                                EdgeIndicator = 2,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = false,
                                EdgeIndicator = 3,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 4,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = false,
                                EdgeIndicator = 5,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = false,
                                EdgeIndicator = 6,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 7,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 8,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 9,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 10,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 11,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            }
                        }
                    },
                    new RectEmptyBox() //3
                    {
                        XDis = 800,
                        YDis = 10900,
                        ZDis = 3300,
                        EmptyBoxLength = 6000,
                        EmptyBoxWidth = 4800,
                        EmptyBoxHeight = 5400,
                        ChamferInfos = new ObservableCollection<EmptyBoxEdgeChameferInfo>()
                        {
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 0,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 1,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = false,
                                EdgeIndicator = 2,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = false,
                                EdgeIndicator = 3,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 4,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = false,
                                EdgeIndicator = 5,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = false,
                                EdgeIndicator = 6,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 7,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 8,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 9,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 10,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 11,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            }
                        }
                    },
                    new RectEmptyBox() //4
                    {
                        XDis = 800,
                        YDis = 17900,
                        ZDis = 300,
                        EmptyBoxLength = 2700,
                        EmptyBoxWidth = 4800,
                        EmptyBoxHeight = 8400,
                       ChamferInfos = new ObservableCollection<EmptyBoxEdgeChameferInfo>()
                        {
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 0,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 1,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = false,
                                EdgeIndicator = 2,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = false,
                                EdgeIndicator = 3,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 4,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = false,
                                EdgeIndicator = 5,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = false,
                                EdgeIndicator = 6,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 7,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 8,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 9,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 10,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 11,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            }
                        }
                    },
                    new RectEmptyBox() //5
                    {
                        XDis = 800,
                        YDis = 21600,
                        ZDis = 300,
                        EmptyBoxLength = 5500,
                        EmptyBoxWidth = 4800,
                        EmptyBoxHeight = 8400,
                       ChamferInfos = new ObservableCollection<EmptyBoxEdgeChameferInfo>()
                        {
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 0,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 1,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = false,
                                EdgeIndicator = 2,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = false,
                                EdgeIndicator = 3,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 4,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = false,
                                EdgeIndicator = 5,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = false,
                                EdgeIndicator = 6,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 7,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 8,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 9,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 10,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 11,
                                ChamferLength = 500,
                                ChamferWidth = 500
                            }
                        }
                    }
                },
                LH_EmptyZPlanBoxs = new ObservableCollection<ZPlanEmptyBox>()
                {
                    new ZPlanEmptyBox()
                    {
                        XDis = 800,
                        YDis = 5800,
                        ZDis = 300,
                        EmptyBoxHeight = 8400,
                        ZPlanInfos= new ObservableCollection<ZPlanInfo>()
                        {
                            new ZPlanInfo()
                            {
                                X =  0,
                                Y=0,
                                BoxEdgeChamferInfo= new EmptyBoxEdgeChameferInfo()
                                {
                                    EdgeIndicator = 0,
                                    IsChamfered = true,
                                    ChamferLength = 1,
                                    ChamferWidth = 1
                                }
                            },
                            new ZPlanInfo()
                            {
                                X=6400,
                                Y=0,
                                BoxEdgeChamferInfo = new EmptyBoxEdgeChameferInfo()
                                {
                                    EdgeIndicator = 1,
                                    IsChamfered = true,
                                    ChamferLength = 2,
                                    ChamferWidth = 2
                                }
                            },
                            new ZPlanInfo()
                            {
                                X=6400,
                                Y=1000,
                                BoxEdgeChamferInfo = new EmptyBoxEdgeChameferInfo()
                                {
                                    EdgeIndicator = 1,
                                    IsChamfered = true,
                                    ChamferLength = 2,
                                    ChamferWidth = 2
                                }
                            },
                            new ZPlanInfo()
                            {
                                X=3100,
                                Y=4300,
                                BoxEdgeChamferInfo = new EmptyBoxEdgeChameferInfo()
                                {
                                    EdgeIndicator = 2,
                                    IsChamfered = true,
                                    ChamferLength = 3,
                                    ChamferWidth = 3
                                }
                            },
                            new ZPlanInfo()
                            {
                                X = 0,
                                Y = 4300,
                                BoxEdgeChamferInfo = new EmptyBoxEdgeChameferInfo()
                                {
                                    EdgeIndicator = 3,
                                    IsChamfered = true,
                                    ChamferLength = 4,
                                    ChamferWidth = 4
                                }
                            }
                        }
                    }
                }
            };
        }

        #region 验证参数
        private RelayCommand _VerifyParam;
        public RelayCommand VerifyParam => _VerifyParam ?? (_VerifyParam = new RelayCommand(ExecuteVerifyParam));
        public void ExecuteVerifyParam()
        {
            string _verifyresult = CZ_LockHeadParameters.IsParametersValid();
            if (Resources.Verified == _verifyresult)
            {
                IsVerified = true;
                Status = Resources.Status_Success;
            }
            else
            {
                Status = Resources.Status_Fail;
            }
            Prompt = _verifyresult;
            
        }

        private RelayCommand _UpdateParam;
        public RelayCommand UpdateParam=> _UpdateParam ?? (_UpdateParam = new RelayCommand(ExecuteUpdateParam));
        public void ExecuteUpdateParam()
        {
            CZ_LockHeadParameters.LH_SidePier.PierXY_E = CZ_LockHeadParameters.LH_BaseBoard.BaseBoardLength - CZ_LockHeadParameters.LH_SidePier.PierXY_D - CZ_LockHeadParameters.LH_SidePier.PierXY_F;
            CZ_LockHeadParameters.LH_DoorSill.DoorSill_A = CZ_LockHeadParameters.LH_BaseBoard.BaseBoardWidth - 2 * CZ_LockHeadParameters.LH_SidePier.PierXY_A;
        }
        #endregion

        #region 绘图
        private RelayCommand _DrawAll;
        public RelayCommand DrawAll => _DrawAll ?? ( _DrawAll = new RelayCommand(ExecuteDrawAll, ()=>IsVerified));
        public void ExecuteDrawAll()
        {
            PDIWT_MS_CZ_CPP.LockHeadDrawing Drawing = new PDIWT_MS_CZ_CPP.LockHeadDrawing(CZ_LockHeadParameters);
            if (Drawing.DoDraw() == 0)
            {
                Prompt = "绘制成功";
                Status = Resources.Status_Success;
            }
            else
            {
                Prompt = "绘制过程出现错误,请检查参数是否有误";
                Status = Resources.Status_Fail;
            }


        }

        private RelayCommand _ResetParam;
        public RelayCommand ResetParam => _ResetParam ?? (_ResetParam = new RelayCommand(ExecuteResetParam));
        public void ExecuteResetParam()
        {
            InitialzeParameters();
        }
        #endregion


        #region 模板输入输出
        private RelayCommand _inputtemplate;
        public RelayCommand InputTemplate => _inputtemplate ?? (_inputtemplate = new RelayCommand(ExecuteInputTemplate));
        private void ExecuteInputTemplate()
        {
            try
            {
                OpenFileDialog ofDialog = new OpenFileDialog()
                {
                    Title = "导入参数模板",
                    Filter = Resources.XMLFilter
                };
                if (ofDialog.ShowDialog() == DialogResult.OK)
                {
                    ExtendedXmlSerializer serializer = new ExtendedXmlSerializer();
                    string xmlstring = File.ReadAllText(ofDialog.FileName);
                    CZ_LockHeadParameters = serializer.Deserialize<LockHeadParameters>(xmlstring);
                    Prompt = ofDialog.FileName + "导入成功";
                    Status = Resources.Status_Success;
                }
            }
            catch (Exception e)
            {
                Prompt = "发生错误" + e.Message;
                Status = Resources.Status_Fail;
            }
        }

        private RelayCommand _outputtemplate;
        public RelayCommand OutputTemplate => _outputtemplate ?? (_outputtemplate = new RelayCommand(ExecuteOutputTemplate,()=>IsVerified));
        public void ExecuteOutputTemplate()
        {
            try
            {
                SaveFileDialog sfDialog = new SaveFileDialog()
                {
                    Title = "导出参数模板",
                    Filter = Resources.XMLFilter
                };
                if (sfDialog.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfDialog.FileName))
                        File.Delete(sfDialog.FileName);
                    ExtendedXmlSerializer seriliSerializer = new ExtendedXmlSerializer();
                    var xml = seriliSerializer.Serialize(CZ_LockHeadParameters);
                    Console.WriteLine(xml);
                    File.WriteAllText(sfDialog.FileName, xml);
                    Prompt = "模板文件输出至" + sfDialog.FileName;
                    Status = Resources.Status_Success;
                }
            }
            catch (Exception e)
            {
                Prompt = "发生错误" + e.Message;
                Status = Resources.Status_Fail;
            }

        }
        #endregion


        private RelayCommand _test;
        public RelayCommand Test => _test ?? (_test = new RelayCommand(ExecuteTest));
        public void ExecuteTest()
        {
            PDIWT_MS_CZ_CPP.LockHeadDrawing Drawing = new PDIWT_MS_CZ_CPP.LockHeadDrawing(CZ_LockHeadParameters);
            Drawing.DoTest();
        }



    }
}