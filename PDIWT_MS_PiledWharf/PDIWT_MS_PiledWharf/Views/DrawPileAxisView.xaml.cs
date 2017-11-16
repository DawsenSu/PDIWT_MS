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
using BMWPF = Bentley.MstnPlatformNET.WPF;

namespace PDIWT_MS_PiledWharf.Views
{
    /// <summary>
    /// DrawPileAxisView.xaml 的交互逻辑
    /// </summary>
    public partial class DrawPileAxisView : UserControl
    {
        private DrawPileAxisView(BM.AddIn addIn)
        {
            InitializeComponent();
            this.DataContext = new ViewModels.DrawPileAxisViewModel();
            m_addIn = addIn;
            this.Unloaded += DrawPileAxisView_Unloaded;
        }
        static BMWPF.ToolSettingsHost m_toolHost;
        BM.AddIn m_addIn;

        public static void ShowWindow(BM.AddIn addIn)
        {
            if (m_toolHost != null)
            {
                m_toolHost.Focus();
                return;
            }
            m_toolHost = new BMWPF.ToolSettingsHost();
            m_toolHost.Title = "绘制桩中心线";
            m_toolHost.Content = new DrawPileAxisView(addIn);
            m_toolHost.Attach(addIn);
            m_toolHost.Show();
        }
        private void DrawPileAxisView_Unloaded(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }
        public static void CloseWindow()
        {
            if (m_toolHost != null)
            {
                m_toolHost.Detach();
                m_toolHost.Dispose();
                m_toolHost = null;
            }
        }

    }
}
