using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using Bentley.DgnPlatformNET;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;

namespace PDIWT_MS_ZJCZL_Re.ViewModels
{
    public class ViewMainViewModel : ViewModelBase
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
                Filter = "Excel 2007 - 2016|*.xlsx",
                Title = "选择输入文件"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            try
            {
                //var dgnlinefromexcel = new DgnLineFromExcelFile(new System.IO.FileInfo(ofd.FileName));
                //dgnlinefromexcel.DrawLines();
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