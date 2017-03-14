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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(Work);
        }

        private void Work()
        {
            Task task = new Task((tb) => Begin(this.first), this.first);
            Task task2 = new Task((tb) => Begin(this.second), this.first);
            Task task3 = new Task((tb) => Begin(this.Three), this.first);
            task.Start();
            task.Wait();
            task2.Start();
            task2.Wait();
            task3.Start();
        }
        private void Begin(TextBlock tb)
        {
            int i = 100000000;
            while (i > 0)
            {
                i--;
            }
            Random random = new Random();
            String Num = random.Next(0, 100).ToString();
            Action<TextBlock, String> updateAction = new Action<TextBlock, string>(UpdateTb);
            tb.Dispatcher.BeginInvoke(updateAction, tb, Num);
        }
        private void UpdateTb(TextBlock tb, string text)
        {
            tb.Text = text;
        }
    }
}
