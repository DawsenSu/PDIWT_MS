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

using BM = Bentley.MstnPlatformNET;
using BD = Bentley.DgnPlatformNET;

namespace PDIWT_MS
{
    /// <summary>
    /// wpfUC.xaml 的交互逻辑
    /// </summary>
    public partial class wpfUC : UserControl
    {
        private static BM.WPF.DockableWindow currentControl;
        private BM.AddIn m_addIn;

        public wpfUC(BM.AddIn addIn)
        {
            m_addIn = addIn;
            InitializeComponent();
        }

        internal static void ShowWindow(BM.AddIn addIn)
        {
            if (null != currentControl)
            {
                currentControl.Focus();
                return;
            }
            currentControl = new BM.WPF.DockableWindow();
            currentControl.Attach(addIn, "Control", new System.Drawing.Size(100, 200));
            currentControl.Content = new wpfUC(addIn);
            currentControl.Title = "Test";
            //必须先连接上在设置
            currentControl.WindowContent.CanDockVertically = false;
            currentControl.Show();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            currentControl = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello world!");
        }
    }
}
