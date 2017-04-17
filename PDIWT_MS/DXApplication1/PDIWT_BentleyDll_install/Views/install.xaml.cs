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

namespace PDIWT_BentleyDll_install.Views
{
    /// <summary>
    /// Interaction logic for install.xaml
    /// </summary>
    public partial class install : UserControl
    {
        public install()
        {
            InitializeComponent();
            DataContext = new ViewModels.installViewModel();
        }
    }
}
