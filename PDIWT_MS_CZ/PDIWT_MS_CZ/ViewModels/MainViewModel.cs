using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

using Bentley.DgnPlatformNET;
using Bentley.GeometryNET;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

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

        public MainViewModel()
        {
            CZ_LockHeadParameters = new LockHeadParameters()
            {
                LH_BaseBoard = new BaseBoard()
                {
                    BaseBoardLength = 200,
                    BaseBoardWidth = 400,
                    BaseBoardHeight = 50,
                    EntranceWidth = 400,
                    IsGrooving = true,
                    TGrooving = new ShapeTGrooving()
                    {
                        GroovingHeight = 5,
                        GroovingBackLength = 30,
                        GroovingFrontLength =30,
                        GroovingWidth = 20,
                        GroovingGradient = 1
                    }
                },
                LH_SidePier = new SidePier()
                {
                    PierHeight = 200,
                    PierXY_A = 200,
                    PierXY_B = 30,
                    PierXY_C = 25,
                    PierXY_D = 200,
                    PierXY_E = 250,
                    PierXY_F = 25,
                    IsChamfered = true,
                    PierChamfer_Tx = 15,
                    PierChamfer_Ty = 10,
                    PierChamfer_R = 10
                },
                LH_DoorSill = new DoorSill()
                {
                    DoorSillHeight =10,
                    DoorSill_A = 200,
                    DoorSill_B = 50,
                    DoorSill_C = 30,
                    DoorSill_D = 70,
                    DoorSill_E = 20,
                    DoorSill_F = 30
                },
                LH_CulvertChoosenIndex = 1,
                LH_ShortCulvert = new ShortCulvert()
                {
                    Culvert_Pier_BackDis = 30,
                    Culvert_Width = 50,
                    Culvert_A = 100,
                    Culvert_B = 300,
                    Culvert_C = 100,
                    Culvert_D = 150,
                    Culvert_R1 = 50,
                    Culvert_R2 = 80
                },
                LH_LocalConcertationCulvert = new LocalConcertationCulvert()
                {
                    Culvert_Height = 50,
                    Culvert_Pier_BackDis = 2,
                    Culvert_A = 200,
                    Culvert_B = 50,
                    Culvert_C = 100,
                    Culvert_D = 50,
                    Culvert_E = 200,
                    Culvert_F = 30,
                    IsChamfered = true,
                    Culvert_Chamfer_R1 = 9,
                    Culvert_Chamfer_R2 = 8,
                    Culvert_Chamfer_R3 = 11,
                    Culvert_Chamfer_R4 = 12,
                    IsIncludeWaterDivision = true,
                    Culvert_WaterDivision = new WaterDivision()
                    {
                        WaterDivision_A = 10,
                        WaterDivision_B = 5,
                        WaterDivision_R1 = 12,
                        WaterDivision_R2 = 15,
                        WaterDivision_R3 = 5
                    },
                    IsIncludeEnergyDisspater = true,
                    Culvert_EnergyDisspater = new EnergyDisspater()
                    {
                        Grille_TwolineInterval = 1,
                        GrilleWidthList = new ObservableCollection<GrillInterval>()
                        {
                            new GrillInterval() {Interval = 1, RoundChamferRadius = 1},
                            new GrillInterval() {Interval =2 ,RoundChamferRadius = 2},
                            new GrillInterval() {Interval = 3, RoundChamferRadius = 3},
                            new GrillInterval() {Interval = 4, RoundChamferRadius = 4}
                        }
                    },
                    IsIncludeBaffle = true,
                    Culvert_Baffle = new ObservableCollection<Baffle>()
                    {
                        new Baffle() {Baffle_MidMidDis = 1, Baffle_Height = 2, Baffle_Width = 3},
                        new Baffle() {Baffle_MidMidDis = 12, Baffle_Height = 22, Baffle_Width = 32},
                        new Baffle() {Baffle_MidMidDis = 13, Baffle_Height = 23, Baffle_Width = 33},
                        new Baffle() {Baffle_MidMidDis = 14, Baffle_Height = 24, Baffle_Width = 34},
                        new Baffle() {Baffle_MidMidDis = 15, Baffle_Height = 25, Baffle_Width = 35}
                    }
                },
                LH_EmptyRectBoxs = new ObservableCollection<RectEmptyBox>()
                {
                    new RectEmptyBox()
                    {
                        XDis = 1,
                        YDis = 2,
                        ZDis = 3,
                        EmptyBoxLength = 50,
                        EmptyBoxWidth = 60,
                        EmptyBoxHeight = 70,
                        ChamferInfos = new ObservableCollection<EmptyBoxEdgeChameferInfo>()
                        {
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 0,
                                ChamferLength = 0,
                                ChamferWidth = 0
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 1,
                                ChamferLength = 1,
                                ChamferWidth = 1
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 2,
                                ChamferLength = 2,
                                ChamferWidth = 2
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 3,
                                ChamferLength = 3,
                                ChamferWidth = 3
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 4,
                                ChamferLength = 4,
                                ChamferWidth = 4
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 5,
                                ChamferLength = 5,
                                ChamferWidth = 5
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 6,
                                ChamferLength = 6,
                                ChamferWidth = 6
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 7,
                                ChamferLength = 7,
                                ChamferWidth = 7
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 8,
                                ChamferLength = 8,
                                ChamferWidth = 8
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 9,
                                ChamferLength = 9,
                                ChamferWidth = 9
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 10,
                                ChamferLength = 10,
                                ChamferWidth = 10
                            },
                            new EmptyBoxEdgeChameferInfo()
                            {
                                IsChamfered = true,
                                EdgeIndicator = 11,
                                ChamferLength = 11,
                                ChamferWidth = 11
                            }
                        }
                    }
                },
                LH_EmptyZPlanBoxs = new ObservableCollection<ZPlanEmptyBox>()
                {
                    new ZPlanEmptyBox()
                    {
                        XDis = 1,
                        YDis = 2,
                        ZDis = 3,
                        EmptyBoxHeight = 4,
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
                                X=1,
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
                                X=1,
                                Y=1,
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
                                Y = 1,
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
            Prompt = "模块加载成功";
            Status = Resources.Status_Success;
        }

        private RelayCommand _inputtemplate;
        public RelayCommand InputTemplate => _inputtemplate?? (_inputtemplate = new RelayCommand(ExecuteInputTemplate));
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
        public RelayCommand OutputTemplate => _outputtemplate ?? (_outputtemplate = new RelayCommand(ExecuteOutputTemplate));
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

        private RelayCommand _test;
        public RelayCommand Test => _test ?? (_test = new RelayCommand(ExecuteTest));
        public void ExecuteTest()
        {
            //MessageBox.Show(CZ_LockHeadParameters.LH_LocalConcertationCulvert.Culvert_Baffle.Count.ToString());
            PDIWT_MS_CZ_CPP.LockHeadDrawing Drawing = new PDIWT_MS_CZ_CPP.LockHeadDrawing(CZ_LockHeadParameters);
            Drawing.DoDraw();
        }

    }
}