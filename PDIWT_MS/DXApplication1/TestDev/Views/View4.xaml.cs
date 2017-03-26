using System;
using System.Collections.Generic;
using System.Data;
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

namespace TestDev.Views
{
    /// <summary>
    /// Interaction logic for View4.xaml
    /// </summary>
    public partial class View4 : UserControl
    {
        public View4()
        {
            InitializeComponent();
            grid.ItemsSource = TestDev.Data.ProductList.GetData();
        }
    }
}
