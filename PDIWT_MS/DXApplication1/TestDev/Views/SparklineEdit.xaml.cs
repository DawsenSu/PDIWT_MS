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
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Charts;
using System.Windows.Threading;

namespace TestDev.Views
{
    /// <summary>
    /// Interaction logic for SparklineEdit.xaml
    /// </summary>
    public partial class SparklineEdit : DXWindow
    {
        public SparklineEdit()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(Interval);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime argument = DateTime.Now;
            Series1.Points.BeginInit();
            Series2.Points.BeginInit();
            SeriesPoint[] pointsToUpdate1 = new SeriesPoint[Interval];
            SeriesPoint[] pointsToUpdate2 = new SeriesPoint[Interval];
            for (int i = 0; i < Interval; i++)
            {
                pointsToUpdate1[i] = new SeriesPoint(argument, value1);
                pointsToUpdate2[i] = new SeriesPoint(argument, value2);
                argument = argument.AddMilliseconds(1);
                UpdateValues();
            }
            DateTime minDate = argument.AddSeconds(-TimeInterval);
            int pointsToRemoveCount = 0;
            foreach (var point in Series1.Points)
            {
                if (point.DateTimeArgument < minDate)
                {
                    pointsToRemoveCount++;
                }
            }
            if (pointsToRemoveCount < Series1.Points.Count)
            {
                pointsToRemoveCount--;
            }
            Series1.Points.AddRange(pointsToUpdate1);
            Series2.Points.AddRange(pointsToUpdate2);
            if (pointsToRemoveCount > 0)
            {
                RemovePointsRange(Series1, pointsToRemoveCount);
                RemovePointsRange(Series2, pointsToRemoveCount);
            }
            Series1.Points.EndInit();
            Series2.Points.EndInit();
            axisX.WholeRange.SetMinMaxValues(minDate, argument);
        }

        private void UpdateValues()
        {
            value1 = CalculateNextValue(value1);
            value2 = CalculateNextValue(value2);
        }

        double CalculateNextValue(double value)
        {
            return value + (random.NextDouble() * 10.0 - 5.0);
        }

        void RemovePointsRange(Series series, int pointsToRemoveCount)
        {
            for (int i = 0; i < pointsToRemoveCount; i++)
            {
                series.Points.RemoveAt(0);
            }
        }
        const int Interval = 50;
        DispatcherTimer timer = new DispatcherTimer();
        Random random = new Random();
        double value1 = 10;
        double value2 = -10;
        bool? inProcess = null;

        int TimeInterval { get { return Convert.ToInt32(seInterval.Value); } }
        
        private void btnPasue_Click(object sender, RoutedEventArgs e)
        {
            timer.IsEnabled = !timer.IsEnabled;
            btnPasue.Content = timer.IsEnabled ? "Pause" : "Resume";
        }
    }
}
