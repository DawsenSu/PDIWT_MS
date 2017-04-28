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

namespace PDIWT_MS_ZJCZL.Views
{
    /// <summary>
    /// Interaction logic for ViewAddPile.xaml
    /// </summary>
    public partial class ViewAddPile : Window
    {
        public ViewAddPile()
        {
            InitializeComponent();
            //DataContext = new ViewModels.ViewAddPileViewModel();
            //combox_piletype.ItemsSource = Enum.GetNames(typeof(PDIWT_MS_ZJCZL.Models.PileType));
        }
    }
}
