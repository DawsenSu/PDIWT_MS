using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;

namespace PDIWT_MS_ZJCZL.ViewModels
{
    using HCHXCodeQueryLib;
    using PDIWT_MS_ZJCZL.Interface;
    using PDIWT_MS_ZJCZL.Models;
    using PDIWT_MS_ZJCZL.Models.Piles;
    using PDIWT_MS_ZJCZL.Models.Soil;
    using PDIWT_MS_ZJCZL.Models.Factory;
    using PDIWT_MS_ZJCZL.Models.PileCrossSection;

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
        public bool CanCalculate()
        {
            if (CurrentPile != null)
                return true;
            else
                return false;
        }

        [Command]
        public void AddPile()
        {
            try
            {
                PileFactory pileFactory = new SolidPileFactory();
                var pileproperty = new RoundnessPileGeometry()
                {
                    PileTopPoint = new Point3d() { X = 0, Y = 0, Z = 0 },
                    PileBottomPoint = new Point3d() { X = 0, Y = 0, Z = -70 },
                    PileDiameter = 0.03,
                    PileInnerDiameter = 0,
                    PileUnderWaterWeight = 15,
                    PileWeight = 25,
                    WaterLevel = -1
                };
                Piles.Add(pileFactory.CreateNewPile(pileproperty, "Test", 0));

                pileFactory = new FillingPileFactory();
                Piles.Add(pileFactory.CreateNewPile(pileproperty, "Test2", 1));
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString(), "添加桩错误", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
        
        [Command]
        public void RemovePile()
        {
            int currentpileindex = Piles.IndexOf(CurrentPile);
            if (currentpileindex == Piles.Count -1)
            {
                Piles.Remove(CurrentPile);
                if (currentpileindex == 0)
                    CurrentPile = null;
                else
                    CurrentPile = Piles.Last();
            }
            else
            {
                Piles.Remove(CurrentPile);
                CurrentPile = Piles[currentpileindex];

            }
        }
        public bool CanRemovePile()
        {
            return (Piles.Count > 0) && (CurrentPile != null);
        }

        [Command]
        public void DrawPileLineFromFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel 2007 - 2016|*.xlsx";
            ofd.Title = "选择输入文件";
            if (ofd.ShowDialog()== DialogResult.OK)
            {
                try
                {
                    var dgnlinefromexcel = new DgnLineFromExcelFile(new System.IO.FileInfo(ofd.FileName));
                    dgnlinefromexcel.DrawLines();
                    MessageBox.Show("绘制完成！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        [Command]
        public void GetPilesFromLines()
        {
            var view = new Views.GetPilesFromLines();
            var viewmodel = new GetPilesFromLinesViewModel();
            viewmodel.Piles = this.Piles;
            if (viewmodel.CloseAction == null)
                viewmodel.CloseAction = new Action(() => view.Close());
            view.DataContext = viewmodel;
            view.ShowDialog();
        }

        [Command]
        public void Analysis()
        {

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
            //CurrentPile = Piles[0];
            //AddPileViewModel = new ViewAddPileViewModel();
            //((ISupportParameter)AddPileViewModel).Parameter = Piles;
        }
    }
}