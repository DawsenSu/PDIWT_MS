using System;
using System.Text;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using Bentley.DgnPlatformNET;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace PDIWT_MS_PiledWharf.ViewModels
{
    using Interface;
    using Models;
    using Models.Soil;
    using Models.Piles;
    using Models.Piles.CrossSection;
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            _Piles = new ObservableCollection<PileBase>()
            {
                new SolidPile()
                {
                    TopPoint = new Bentley.GeometryNET.DPoint3d(0, 0, 0),
                    BottomPoint = new Bentley.GeometryNET.DPoint3d(0, 0, -100),
                    Code = "Pile1",
                    ID = 2120,
                    ICrossSection = new SquareCrossSection(0.5)
                },
                new SolidPile()
                {
                    TopPoint = new Bentley.GeometryNET.DPoint3d(10, 15, 0),
                    BottomPoint = new Bentley.GeometryNET.DPoint3d(10, 10, -80),
                    Code = "Pile2",
                    ID = 333,
                    ICrossSection = new SquareCrossSection(0.6)
                },
            };
            if (_Piles.Count > 0)
                _SelectedPile = _Piles.First();
            _GammaR = 1.2;
            _Eta = 0;
            _WaterLevel = 0;
        }

        #region Properties
        private ObservableCollection<PileBase> _Piles;
        public ObservableCollection<PileBase> Piles
        {
            get { return _Piles; }
            set { Set(ref _Piles, value); }
        }

        private PileBase _SelectedPile;
        public PileBase SelectedPile
        {
            get { return _SelectedPile; }
            set
            {
                Set(ref _SelectedPile, value);
                RaisePropertyChanged(() => SelectedPile_pilepiecesoilinfo);
                RaisePropertyChanged(() => SelectedPile_Qr);
                RaisePropertyChanged(() => SelectedPile_Qd);
                RaisePropertyChanged(() => SelectedPile_Td);
            }
        }

        public ObservableCollection<PilePieceInSoilLayerInfo> SelectedPile_pilepiecesoilinfo
        {
            get { return _SelectedPile?.GetPilePieceInEachSoilLayerInfos(); }
        }

        public double? SelectedPile_Qr
        {
            get
            {
                return _SelectedPile?.GetPilePieceInEachSoilLayerInfos().Last().CurrentSoilLayerInfo.Qri;
            }
        }
        public double? SelectedPile_Qd
        {
            get
            {
                return _SelectedPile?.CalculateQd(_GammaR, _Eta);
            }
        }

        public double? SelectedPile_Td
        {
            get
            {
                return _SelectedPile?.CalculateTd(_GammaR,_WaterLevel);
            }            
        }

        private double _GammaR;
        public double GammaR
        {
            get { return _GammaR; }
            set
            {
                Set(ref _GammaR, value);
                RaisePropertyChanged(() => SelectedPile_Qd);
                RaisePropertyChanged(() => SelectedPile_Td);
            }
        }
        private double _Eta;
        public double Eta
        {
            get { return _Eta; }
            set { Set(ref _Eta, value); }
        }



        private double _WaterLevel;
        public double WaterLevel
        {
            get { return _WaterLevel; }
            set
            {
                Set(ref _WaterLevel, value);
                RaisePropertyChanged(() => SelectedPile_Qd);
                RaisePropertyChanged(() => SelectedPile_Td);
            }
        }
        #endregion

        #region Commands
        private RelayCommand _DrawAxisFromExcelCommand;
        public RelayCommand DrawAxisFromExcelCommand => _DrawAxisFromExcelCommand ?? (_DrawAxisFromExcelCommand = new RelayCommand(ExecuteDrawAxisFromExcelCommand));
        public void ExecuteDrawAxisFromExcelCommand()
        {

        }
        #endregion

        public void DrawPileLineFromFile()
        {
            //OpenFileDialog ofd = new OpenFileDialog
            //{
            //    Filter = Resources.ExcelFileFilter,
            //    Title = Resources.OpenFileDlgTitle
            //};
            //if (ofd.ShowDialog() != DialogResult.OK) return;
            //try
            //{
            //    var dgnlinefromexcel = new DrawLineElementFromExcelFile(ofd.FileName);
            //    dgnlinefromexcel.DrawLines();
            //    MessageBox.Show("绘制完成！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.ToString());
            //}
        }
        public void GetPilesFromLines()
        {
            //var view = new Views.GetPilesFromLines();
            //var viewmodel = new GetPilesFromLinesViewModel();
            //viewmodel.Piles = this.Piles;
            //if (viewmodel.CloseAction == null)
            //    viewmodel.CloseAction = new Action(() => view.Close());
            //view.DataContext = viewmodel;
            //view.ShowDialog();
        }

        private RelayCommand _Test;
        public RelayCommand Test => _Test ?? (_Test = new RelayCommand(ExecuteTest));
        public void ExecuteTest()
        {
            //Models.IntersectionPointQuery _test = new Models.IntersectionPointQuery();
            //var _list = _test.FindBSElement();
            //var _pointquery = new  PDIWT_MS_PiledWharf_CPP.PointQuery();
            //var _pileinfo = _pointquery.GetPilePieceSoilLayerInfos(new Bentley.GeometryNET.DPoint3d(0, 0, 0), new Bentley.GeometryNET.DPoint3d(0, 0, -1e8));
            //var _res = _pointquery.QueryResult;
            //MessageBox.Show(Enum.GetName(typeof(PDIWT_MS_PiledWharf_CPP.QueryResultStatus), _res));
            var _pileinfo = _SelectedPile.GetPilePieceInEachSoilLayerInfos();
            StringBuilder _sb = new StringBuilder();
            foreach (var _pile in _pileinfo)
            {
                _sb.Append(String.Format("Name:{0} topz:{1} bottomz:{2} length:{3}\n", _pile.CurrentSoilLayerInfo.SoilLayerNum, _pile.PileTopZ_InCurrentSoilLayer, _pile.PileBottomZ_InCurrentSoilLayer, _pile.PilePieceLength));
            }
            MessageBox.Show(_sb.ToString());
        }
    }
}