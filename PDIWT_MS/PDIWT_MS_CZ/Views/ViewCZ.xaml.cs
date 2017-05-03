using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace PDIWT_MS_CZ.Views
{
    /// <summary>
    /// Interaction logic for ViewCZ.xaml
    /// </summary>
    public partial class ViewCZ : UserControl
    {
        public ViewCZ()
        {
            InitializeComponent();
            DataContext = new ViewModels.ViewCZViewModel();
        }

        private void TableView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            if(gridcontrol_gs.VisibleRowCount%2 == 0)
                gridcontrol_gs.SetCellValue(e.RowHandle, "IntervalType", "格栅间距");
            else
                gridcontrol_gs.SetCellValue(e.RowHandle, "IntervalType", "格栅宽度");
        }

    }
}
