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
    using Models.Piles;
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {

        }

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
            set { Set(ref _SelectedPile, value); }
        }





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
            var _pointquery = new  PDIWT_MS_PiledWharf_CPP.PointQuery();
            var _pileinfo = _pointquery.GetPilePieceSoilLayerInfos(new Bentley.GeometryNET.DPoint3d(0, 0, 0), new Bentley.GeometryNET.DPoint3d(0, 0, -1e8));
            var _res = _pointquery.QueryResult;
            MessageBox.Show(Enum.GetName(typeof(PDIWT_MS_PiledWharf_CPP.QueryResultStatus), _res));
            StringBuilder _sb = new StringBuilder();
            foreach (var _pile in _pileinfo)
            {
                _sb.Append(String.Format("Name:{0} topz:{1} bottomz:{2} length:{3}\n", _pile.CurrentSoilLayerInfo.SoilLayerNum, _pile.PileTopZ_InCurrentSoilLayer, _pile.PileBottomZ_InCurrentSoilLayer, _pile.PilePieceLength));
            }
            MessageBox.Show(_sb.ToString());
        }
    }
}