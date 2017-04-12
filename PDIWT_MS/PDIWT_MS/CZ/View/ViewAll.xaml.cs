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

namespace PDIWT_MS.CZ.View
{
    /// <summary>
    /// Interaction logic for ViewAll.xaml
    /// </summary>
    public partial class ViewAll : UserControl
    {
        public ViewAll()
        {
            InitializeComponent();
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {

        }

        private void TableView_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {
            hole_gc.SetCellValue(e.RowHandle, "HoleLength", 0);
            hole_gc.SetCellValue(e.RowHandle, "HoleWidth", 0);
            hole_gc.SetCellValue(e.RowHandle, "HoleHeight", 0);
            hole_gc.SetCellValue(e.RowHandle, "XDis", 0);
            hole_gc.SetCellValue(e.RowHandle, "YDis", 0);
            hole_gc.SetCellValue(e.RowHandle, "ZDis", 0);
        }
    }
}
