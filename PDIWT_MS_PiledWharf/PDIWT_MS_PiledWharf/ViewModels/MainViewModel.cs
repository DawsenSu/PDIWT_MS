using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using Bentley.DgnPlatformNET;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace PDIWT_MS_PiledWharf.ViewModels
{

    public class MainViewModel : ViewModelBase
    {
        //public ObservableCollection<PileBase> Piles
        //{
        //    get { return GetProperty(() => Piles); }
        //    set { SetProperty(() => Piles, value); }
        //}

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
            Models.IntersectionPointQuery _test = new Models.IntersectionPointQuery();
            var _list = _test.FindBSElement();
            MessageBox.Show(_list.Count.ToString());
        }
    }
}