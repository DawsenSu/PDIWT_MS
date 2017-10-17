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
    /// <summary>
    /// EmptyBoxSummaryUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class EmptyBoxSummaryUserControl : UserControl
    {
        public EmptyBoxSummaryUserControl()
        {
            InitializeComponent();
        }

        private void RectEmptyBox_ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ListBox_EmtyBoxSummary.Items.Add(new ListBoxItem() {Content = "1"});
            ContentControl_EmptyBoxDetail.Content = new RectEmptyBoxUserControl();
        }

        private void ZPlanEmptyBox_ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ListBox_EmtyBoxSummary.Items.Add(new ListBoxItem() { Content = "2" });
            ContentControl_EmptyBoxDetail.Content = new ZPlanEmptyBoxUserControl();
        }
    }
}
