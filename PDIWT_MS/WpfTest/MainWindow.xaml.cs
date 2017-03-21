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

namespace WpfTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            (sender as Button).IsEnabled = true;
            await Task.Factory.StartNew(new Action<object>(async (max) =>
            {
                int maxnum = (int)(double)max;
                for (int i = 1; i < maxnum + 1; i++)
                {
                    await Task.Delay(TimeSpan.FromSeconds(0.03));
                    await Dispatcher.BeginInvoke(new Action<int, int>((index, _maxnum) =>
                     {
                         pgb.Value = index;
                         lb_pg.DataContext = Math.Round((double)index / _maxnum * 100, 2).ToString() + "%";
                     }), i, maxnum);
                }
            }), pgb.Maximum);
        }
    }
}
