using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using PDIWT_MS_CZ.Models;

namespace PDIWT_MS_CZ.Views.DetailUserControl
{
    /// <summary>
    /// EmptyBoxSummaryUserControl.xaml 的交互逻辑
    /// </summary>
    // todo: 改成MVVM模式
    public partial class EmptyBoxSummaryUserControl : UserControl
    {
        public EmptyBoxSummaryUserControl()
        {
            InitializeComponent();
        }

        private void RectEmptyBox_ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            RectEmptyBoxWindow rectEmptyBoxWindow = new RectEmptyBoxWindow()
            {
                DataContext = ((Button)sender).DataContext
            };
            rectEmptyBoxWindow.ShowDialog();
        }

        private void ZPlanEmptyBox_ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ZPlanEmptyBoxWindow zPlanEmptyBoxWindow = new ZPlanEmptyBoxWindow()
            {
                DataContext = ((Button) sender).DataContext
            };
            zPlanEmptyBoxWindow.ShowDialog();
        }
    }
}
