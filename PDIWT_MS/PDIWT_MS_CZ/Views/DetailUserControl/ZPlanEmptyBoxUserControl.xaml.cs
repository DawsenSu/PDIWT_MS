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
using DevExpress.XtraExport.Helpers;
using PDIWT_MS_CZ.Models;

namespace PDIWT_MS_CZ.Views.DetailUserControl
{
    /// <summary>
    /// ZPlanEmptyBoxUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ZPlanEmptyBoxUserControl : UserControl
    {
        public ZPlanEmptyBoxUserControl()
        {
            InitializeComponent();
            DataContext = new ViewModels.DetailUserControl.ZPlanEmptyBoxUCViewModel();
        }

        private void DrawXYPlan_ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var xyplaninfos = DataGrid_Info.ItemsSource;
            var xyplanpolygon = new Polygon()
            {
                Stroke = new SolidColorBrush(Colors.Red),
                StrokeThickness = 1,
                Fill = new SolidColorBrush(Colors.LightYellow),
                RenderTransform = new ScaleTransform(100,100),
                RenderTransformOrigin = new Point(0,0)
            };
            foreach (var xyplaninfo in xyplaninfos)
            {
                var info = (ZPlanInfo) xyplaninfo;
                xyplanpolygon.Points.Add(new Point(info.Point2D.X,info.Point2D.Y));
            }
            Canvas_XY.Children.Add(xyplanpolygon);
        }
    }
}
