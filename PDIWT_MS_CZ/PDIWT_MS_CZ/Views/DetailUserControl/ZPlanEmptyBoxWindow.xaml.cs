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
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Core.Native;
using DevExpress.XtraExport.Helpers;
using PDIWT_MS_CZ.Models;

namespace PDIWT_MS_CZ.Views.DetailUserControl
{
    /// <summary>
    /// ZPlanEmptyBoxUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ZPlanEmptyBoxWindow : Window
    {
        public ZPlanEmptyBoxWindow()
        {
            InitializeComponent();
        }

        private void DrawZPlanShape()
        {
            Canvas_XY.Children.Clear();

            var xyplaninfos = DataGrid_Info.ItemsSource;
            PointCollection points = new PointCollection();
            foreach (var xyplaninfo in xyplaninfos)
            {
                var info = (ZPlanInfo)xyplaninfo;
                points.Add(new Point(info.X, info.Y));
            }
            var maxpoinstx = points.Max(p => p.X);
            var maxpoinsty = points.Max(p => p.Y);
            double scale = 280 / Math.Max(maxpoinstx, maxpoinsty);

            PointCollection saclePointCollection = new PointCollection();
            foreach (var point in points)
            {
                saclePointCollection.Add(new Point(point.X * scale, point.Y * scale));
            }
            var polygon_drawing = new Polygon()
            {
                Points = saclePointCollection,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
            Canvas_XY.Children.Add(polygon_drawing);
            Canvas.SetLeft(polygon_drawing, 10);
            Canvas.SetTop(polygon_drawing, 10);


            int i = 0;
            foreach (var point in saclePointCollection)
            {
                TextBlock text = new TextBlock() { Text = i.ToString() };
                text.RenderTransform = new ScaleTransform(1, -1);
                Canvas_XY.Children.Add(text);
                Canvas.SetLeft(text, point.X + 11);
                Canvas.SetTop(text, point.Y + 11);
                i++;
            }
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DrawZPlanShape();
        }
    }
}
