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

using Bentley.MstnPlatformNET.WPF;
using Bentley.Windowing;

namespace PDIWT_MS.Test
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        private static Window1 m_window1;
        WPFInteropHelper m_wndHelper;

        public Window1()
        {
            InitializeComponent();

            m_wndHelper = new WPFInteropHelper(this);
            m_wndHelper.Attach(Program.Addin, true, "WPFWindowTest");
        }

        public static void ShowWindow()
        {
            if (m_window1 != null)
            {
                m_window1.Focus();
                return;
            }
            m_window1 = new Window1();
            m_window1.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            m_wndHelper.Detach();
            m_wndHelper.Dispose();
            m_window1 = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Bentley.MstnPlatformNET.WinForms.ECPropertyPane.LevelEditor le = new Bentley.MstnPlatformNET.WinForms.ECPropertyPane.LevelEditor();
            
        }
    }
}
