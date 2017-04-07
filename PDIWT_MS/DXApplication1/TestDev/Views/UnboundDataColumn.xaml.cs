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
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using DevExpress.Mvvm.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace TestDev.Views
{
    /// <summary>
    /// Interaction logic for UnboundDataColumn.xaml
    /// </summary>
    public partial class UnboundDataColumn : DXWindow
    {
        public UnboundDataColumn()
        {
            InitializeComponent();
            grid.ItemsSource = new ObservableCollectionCore<Goods> { new Goods { ProductName = "good1", UnitPrice=1,  UnitsOnOrder =3},
                new Goods {ProductName ="test", UnitPrice=3, UnitsOnOrder=3 } };
        }

        private void grid_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                int price = Convert.ToInt32(e.GetListSourceFieldValue("UnitPrice"));
                int unitOnOrder = Convert.ToInt32(e.GetListSourceFieldValue("UnitsOnOrder"));
                e.Value = price * unitOnOrder;
            }
        }

        private void TableView_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {
            grid.SetCellValue(e.RowHandle, "ProductName", "New1");
        }
    }
    class Goods
    {
        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int UnitsOnOrder { get; set; }
    }
    public enum UserRole
    {
        [Image("pack://application:,,,/DevExpress.Images.v16.1;component/DevAV/Actions/Driving_32x32.png"), Display(Name ="Admin",Description ="High level of access",Order =1)]
        Administrator,
        [Display(Name = "Moderator", Description = "Average level of access", Order = 2)]
        Moderator,
        [Display(Name = "User", Description = "Low level of access", Order = 3)]
        User
    }
}
