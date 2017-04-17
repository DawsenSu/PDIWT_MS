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
    }
}
