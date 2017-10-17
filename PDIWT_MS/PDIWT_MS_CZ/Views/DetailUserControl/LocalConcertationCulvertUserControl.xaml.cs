using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Grid;

namespace PDIWT_MS_CZ.Views.DetailUserControl
{
    /// <summary>
    /// LocalConcertationCulvertUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class LocalConcertationCulvertUserControl : UserControl
    {
        public LocalConcertationCulvertUserControl()
        {
            DataContext = new ViewModels.DetailUserControl.LocalConcertationCulvertUCViewModel();

            InitializeComponent();
        }
        //private void TableView_InitNewRow(object sender, InitNewRowEventArgs e)
        //{
        //    if (GridControl_Grill.VisibleRowCount % 2 == 0)
        //        GridControl_Grill.SetCellValue(e.RowHandle, "IntervalType", "格栅间距");
        //    else
        //        GridControl_Grill.SetCellValue(e.RowHandle, "IntervalType", "格栅宽度");
        //}
    }
}
