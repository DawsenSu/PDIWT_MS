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
            var _locator = new ViewModels.ViewModelLocator();
            this.DataContext = _locator.DrawPileAxisVM;
            m_addIn = addIn;
            this.Unloaded += DrawPileAxisView_Unloaded;
        }

        public static DrawPileAxisView Instance;
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
            m_toolHost.Width = 300;
            m_toolHost.Height = 180;
            m_toolHost.Title = "绘制桩中心线";
            Instance = new DrawPileAxisView(addIn);
            m_toolHost.Content = Instance;
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
                Instance = null;
            }
        }

    }
}
