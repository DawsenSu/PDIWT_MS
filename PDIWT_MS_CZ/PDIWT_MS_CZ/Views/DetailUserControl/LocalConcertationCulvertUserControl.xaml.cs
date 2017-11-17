
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

namespace PDIWT_MS_CZ.Views.DetailUserControl
{
    using Models;
    /// <summary>
    /// LocalConcertationCulvertUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class LocalConcertationCulvertUserControl : UserControl
    {
        public LocalConcertationCulvertUserControl()
        {
            InitializeComponent();
        }

        private void DataGrid_InitializingNewItem_Grill(object sender, InitializingNewItemEventArgs e)
        {
            GrillInterval interval = e.NewItem as GrillInterval;
            if (interval != null)
            {
                interval.Interval = 100;
                interval.RoundChamferRadius = 10;
            }

        }

        private void DataGrid_InitializingNewItem_Baffle(object sender, InitializingNewItemEventArgs e)
        {
            Baffle baffle = e.NewItem as Baffle;
            if(baffle !=null)
            {
                baffle.Baffle_Width = 10;
                baffle.Baffle_Height = 10;
            }
        }
    }
}
