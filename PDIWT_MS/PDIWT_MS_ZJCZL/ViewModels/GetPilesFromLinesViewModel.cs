using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.CodeView;


using BD = Bentley.DgnPlatformNET;
using BDE = Bentley.DgnPlatformNET.Elements;
using BG = Bentley.GeometryNET;

using PDIWT_MS_ZJCZL.Models.Piles;
using PDIWT_MS_ZJCZL.Models.Factory;
using PDIWT_MS_ZJCZL.Interface;
using PDIWT_MS_ZJCZL.Models;
using PDIWT_MS_ZJCZL.Models.PileCrossSection;


namespace PDIWT_MS_ZJCZL.ViewModels
{
    public class GetPilesFromLinesViewModel : ViewModelBase
    {
        public Action CloseAction;//“关闭”代理

        public ObservableCollection<PileBase> Piles
        {
            get { return GetProperty(() => Piles); }
            set { SetProperty(() => Piles, value); }
        }

        public ObservableCollection<BD.LevelHandle> AllDgnLevels
        {
            get { return GetProperty(() => AllDgnLevels); }
            set { SetProperty(() => AllDgnLevels, value); }
        }
        public BD.LevelHandle SelectedDgnLevel
        {
            get { return GetProperty(() => SelectedDgnLevel); }
            set { SetProperty(() => SelectedDgnLevel, value); }
        }
        public PileType SelectedPileType
        {
            get { return GetProperty(() => SelectedPileType); }
            set { SetProperty(() => SelectedPileType, value); }
        }
        public SoildPileCrossSectionType SelectedPileCrossSectionType
        {
            get { return GetProperty(() => SelectedPileCrossSectionType); }
            set { SetProperty(() => SelectedPileCrossSectionType, value); }
        }
        public string PileName
        {
            get { return GetProperty(() => PileName); }
            set { SetProperty(() => PileName, value); }
        }
        public double PileDiameter
        {
            get { return GetProperty(() => PileDiameter); }
            set { SetProperty(() => PileDiameter, value); }
        }
        public double PileInnerDiameter
        {
            get { return GetProperty(() => PileInnerDiameter); }
            set { SetProperty(() => PileInnerDiameter, value); }
        }
        public double PileWeight
        {
            get { return GetProperty(() => PileWeight); }
            set { SetProperty(() => PileWeight, value); }
        }
        public double PileUnderwaterWeight
        {
            get { return GetProperty(() => PileUnderwaterWeight); }
            set { SetProperty(() => PileUnderwaterWeight, value); }
        }
        public double GammaR
        {
            get { return GetProperty(() => GammaR); }
            set { SetProperty(() => GammaR, value); }
        }
        public double Gammas
        {
            get { return GetProperty(() => Gammas); }
            set { SetProperty(() => Gammas, value); }
        }
        public double Hr
        {
            get { return GetProperty(() => Hr); }
            set { SetProperty(() => Hr, value); }
        }
        public double WaterLevel
        {
            get { return GetProperty(() => WaterLevel); }
            set { SetProperty(() => WaterLevel, value); }
        }
        public double PolygonCrossSectionArea
        {
            get { return GetProperty(() => PolygonCrossSectionArea); }
            set { SetProperty(() => PolygonCrossSectionArea, value); }
        }
        public double PolygonCrossSectionPerimeter
        {
            get { return GetProperty(() => PolygonCrossSectionPerimeter); }
            set { SetProperty(() => PolygonCrossSectionPerimeter, value); }
        }


        protected override void OnInitializeInRuntime()
        {
            var alllevel = new ObservableCollection<BD.LevelHandle>();
            BD.DgnFile activefile = Program.GetActiveDgnFile();
            BD.FileLevelCache flevel = activefile.GetLevelCache();
            foreach (var levelhandle in flevel.GetHandles())
            {
                alllevel.Add(levelhandle);
            }
            AllDgnLevels = alllevel;

            //PileCrossSectionTypes = new Dictionary<string, PileCrossSectionType>
            //{
            //    {"圆形截面桩", PileCrossSectionType.Roundness },
            //    {"方形截面桩", PileCrossSectionType.Square },
            //    {"环形截面桩", PileCrossSectionType.Annular }
            //};
            SelectedPileCrossSectionType = SoildPileCrossSectionType.Square;
            SelectedPileType = PileType.Solid;
            PileWeight = 25;
            PileUnderwaterWeight = 15;
        }

        [Command]
        public void Add()
        {
            try
            {
                if(SelectedDgnLevel == null)
                {
                    MessageBox.Show("未选择线段所在图层\n请重新选择图层", "未选择图层", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                //扫描图中的线段
                BD.ScanCriteria sc = new Bentley.DgnPlatformNET.ScanCriteria();
                sc.SetModelRef(Program.GetActiveDgnModelRef());
                BD.BitMask bm = new BD.BitMask(true);
                bm.Set((uint)BD.MSElementType.Line);
                sc.SetElementTypeTest(bm);

                var elelist = new List<BDE.Element>();
                sc.Scan((ele, dgnmodel) =>
                {
                    var level = Program.GetActiveDgnModel().GetLevelCache().GetLevel(ele.LevelId, true);
                    if (ele.ElementType == BD.MSElementType.Line && ele.LevelId == SelectedDgnLevel.LevelId)
                    {
                        elelist.Add(ele);
                    }
                    return BD.StatusInt.Success;
                });
                if (elelist.Count == 0)
                {
                    MessageBox.Show("所选图层中不存在线段元素\n请重新选择图层或绘制代表桩的线段", "所选图层无线段", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                PileFactory pilefactory = null;

                var pileproplist = new List<IPileProperty>();
                var pilenamelist = new List<string>();
                var pileIdlist = new List<long>();

                switch (SelectedPileType)
                {
                    case PileType.Solid:
                        pilefactory = new SolidPileFactory(GammaR);
                        foreach (var pile in elelist)
                        {
                            //BG.DRange3d range;
                            //((BDE.LineElement)pile).CalcElementRange(out range);//有问题

                            BG.DPoint3d startp, endp;
                            if(!((BDE.LineElement)pile).GetCurveVector().GetStartEnd(out startp, out endp))
                            {
                                System.Windows.Forms.MessageBox.Show($"无法获取Pile ID:{pile.ElementId}的起始点和终点");
                                return;
                            }
                            switch (SelectedPileCrossSectionType)
                            {
                                case SoildPileCrossSectionType.SquareWithRoundHole:
                                    pileproplist.Add(new SquareWithRoundHolePileGeometry() { PileDiameter = this.PileDiameter, PileInnerDiameter =this.PileInnerDiameter, PileWeight = this.PileWeight, PileUnderWaterWeight = this.PileUnderwaterWeight, WaterLevel = WaterLevel, PileTopPoint = startp.DPoint3dToPoint3d(1e-4), PileBottomPoint = endp.DPoint3dToPoint3d(1e-4) });
                                    break;
                                case SoildPileCrossSectionType.Square:
                                    pileproplist.Add(new SquarePileGeometry() { PileDiameter = this.PileDiameter, PileWeight = this.PileWeight, PileUnderWaterWeight = this.PileUnderwaterWeight, WaterLevel = WaterLevel, PileTopPoint = startp.DPoint3dToPoint3d(1e-4), PileBottomPoint = endp.DPoint3dToPoint3d(1e-4) });
                                    break;
                                case SoildPileCrossSectionType.Polygon:
                                    pileproplist.Add(new PolygonPileGeometry() { CrossSectionArea= this.PolygonCrossSectionArea, PileCrossSectionPerimeter = this.PolygonCrossSectionPerimeter, PileWeight = this.PileWeight, PileUnderWaterWeight = this.PileUnderwaterWeight, WaterLevel = WaterLevel, PileTopPoint = startp.DPoint3dToPoint3d(1e-4), PileBottomPoint = endp.DPoint3dToPoint3d(1e-4) });
                                    break;
                            }

                            var pilecodereaddatablock = pile.GetLinkage((ushort)BD.ElementLinkageId.String);
                            string pilename = string.Empty;
                            if (null != pilecodereaddatablock)
                                //throw new NotSupportedException($"Pile Id:{pile.ElementId} doesn't contain pilecode linkage");
                                pilename = pilecodereaddatablock.ReadString();
                            else
                                pilename = pile.Description + " ID:" + pile.ElementId.ToString();
                            pilenamelist.Add(PileName + pilename);
                            pileIdlist.Add(pile.ElementId);
                        }
                        break;
                    case PileType.SteelAndPercastConcrete:
                        pilefactory = new SteelAndPercastConcretePileFactory(GammaR);
                        foreach (var pile in elelist)
                        {
                            //BG.DRange3d range;
                            //((BDE.LineElement)pile).CalcElementRange(out range);

                            BG.DPoint3d startp, endp;
                            if (!((BDE.LineElement)pile).GetCurveVector().GetStartEnd(out startp, out endp))
                            {
                                System.Windows.Forms.MessageBox.Show($"无法获取Pile ID:{pile.ElementId}的起始点和终点");
                                return;
                            }
                            pileproplist.Add(new AnnularPileGeometry() { PileDiameter = this.PileDiameter, PileInnerDiameter = this.PileInnerDiameter, PileWeight = this.PileWeight, PileUnderWaterWeight = this.PileUnderwaterWeight, WaterLevel = WaterLevel, PileTopPoint = startp.DPoint3dToPoint3d(1e-4), PileBottomPoint = endp.DPoint3dToPoint3d(1e-4) });

                            var pilecodereaddatablock = pile.GetLinkage((ushort)BD.ElementLinkageId.String);
                            string pilename = string.Empty;
                            if (null != pilecodereaddatablock)
                                //throw new NotSupportedException($"Pile Id:{pile.ElementId} doesn't contain pilecode linkage");
                                pilename = pilecodereaddatablock.ReadString();
                            pilenamelist.Add(PileName + pilename);
                            pileIdlist.Add(pile.ElementId);
                        }
                        break;
                        //case PileType.Filling:
                        //    pilefactory = new FillingPileFactory();
                        //    break;
                        //case PileType.Socketed:
                        //    pilefactory = new SocketedPileFactory();
                        //    break;
                        //case PileType.PostgroutingFilling:
                        //    pilefactory = new PostgroutingFillingPileFactory();
                        //    break;
                }

                //foreach (var pile in elelist)
                //{
                //    BG.DRange3d range;
                //    ((BDE.LineElement)pile).CalcElementRange(out range);
                //    switch (SelectedPileCrossSectionType)
                //    {
                //        case PileCrossSectionType.Roundness:
                //            pileproplist.Add(new RoundnessPileGeometry() { PileDiameter = this.PileDiameter, PileWeight = this.PileWeight, PileUnderWaterWeight = this.PileUnderwaterWeight, WaterLevel = WaterLevel, PileTopPoint = range.High.DPoint3dToPoint3d(1e-4), PileBottomPoint = range.Low.DPoint3dToPoint3d(1e-4) });
                //            break;
                //        case PileCrossSectionType.Square:
                //            pileproplist.Add(new SquarePileGeometry() { PileDiameter = this.PileDiameter, PileWeight = this.PileWeight, PileUnderWaterWeight = this.PileUnderwaterWeight, WaterLevel = WaterLevel, PileTopPoint = range.High.DPoint3dToPoint3d(1e-4), PileBottomPoint = range.Low.DPoint3dToPoint3d(1e-4) });
                //            break;
                //        case PileCrossSectionType.Annular:
                //            pileproplist.Add(new AnnularPileGeometry() { PileDiameter = this.PileDiameter, PileInnerDiameter = this.PileInnerDiameter, PileWeight = this.PileWeight, PileUnderWaterWeight = this.PileUnderwaterWeight, WaterLevel = WaterLevel, PileTopPoint = range.High.DPoint3dToPoint3d(1e-4), PileBottomPoint = range.Low.DPoint3dToPoint3d(1e-4) });
                //            break;
                //    }
                //    pilenamelist.Add(PileName + pile.GetLinkage((ushort)BD.ElementLinkageId.String).ReadString());
                //    pileIdlist.Add(pile.ElementId);
                //}
                var templist = pilefactory?.CreateNewPileArray(pileproplist.ToArray(), pilenamelist.ToArray(), pileIdlist.ToArray());
                Piles.AddRange(templist);

                CloseAction();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }          
        }

        [Command]
        public void Close()
        {
            CloseAction();
        }
    }
    public enum SoildPileCrossSectionType
    {
        [Display(Name = "方形截面"), Image("pack://application:,,,/PDIWT_MS_ZJCZL;component/Resources/Image/Square_16.png")]
        Square,
        [Display(Name = "方形圆空心截面"),Image("pack://application:,,,/PDIWT_MS_ZJCZL;component/Resources/Image/SquareWithRoundHole_16.png")]
        SquareWithRoundHole,
        [Display(Name = "多边形截面"), Image("pack://application:,,,/PDIWT_MS_ZJCZL;component/Resources/Image/Polygon_16.png")]
        Polygon
    }

    public enum PileType
    {
        [Display(Name = "实心桩或桩端封闭")]
        Solid,                      //桩身实心火桩端封闭
        [Display(Name = "钢管桩与预制混凝土管桩")]
        SteelAndPercastConcrete    //钢管桩与预制混凝土管桩
        //[Display( Name = "灌注桩")]
        //Filling,                    //灌注桩
        //[Display(Name = "嵌岩桩")]
        //Socketed,                   //嵌岩桩
        //[Display(Name = "后注浆灌注桩")]
        //PostgroutingFilling         //后注浆灌注桩
    }

}