using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using Bentley.DgnPlatformNET;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;

using PDIWT_MS_ZJCZL_Re.Properties;
using PDIWT_MS_ZJCZL_Re.Models;
namespace PDIWT_MS_ZJCZL_Re.ViewModels
{

    public class MainViewModel : ViewModelBase
    {
        //public ObservableCollection<PileBase> Piles
        //{
        //    get { return GetProperty(() => Piles); }
        //    set { SetProperty(() => Piles, value); }
        //}

        [Command]
        public void DrawPileLineFromFile()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = Resources.ExcelFileFilter,
                Title = Resources.OpenFileDlgTitle
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            try
            {
                var dgnlinefromexcel = new DrawLineElementFromExcelFile(ofd.FileName);
                dgnlinefromexcel.DrawLines();
                MessageBox.Show("绘制完成！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        [Command]
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

        [Command]
        public void Test()
        {
            
        }
    }
}