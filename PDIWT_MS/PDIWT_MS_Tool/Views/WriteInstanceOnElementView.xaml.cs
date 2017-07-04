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
using System.Windows.Navigation;
using System.Windows.Shapes;

using BMWPF = Bentley.MstnPlatformNET.WPF;
using BM = Bentley.MstnPlatformNET;

namespace PDIWT_MS_Tool.Views
{
    /// <summary>
    /// Interaction logic for WriteInstanceView.xaml
    /// </summary>
    public partial class WriteInstanceOnElementView : UserControl
    {
        private WriteInstanceOnElementView(BM.AddIn addIn)
        {
            InitializeComponent();
            this.DataContext = new ViewModels.WriteInstanceOnElementViewModel();
            m_addIn = addIn;
            this.Unloaded += WriteInstanceView_Unloaded;
        }
        public static WriteInstanceOnElementView Instance;
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
            m_toolHost.Height = 250;
            m_toolHost.Title = "向元素中附加属性";
            Instance = new WriteInstanceOnElementView(addIn);
            m_toolHost.Content = Instance;
            m_toolHost.Attach(addIn);
            m_toolHost.Show();
        }
        private void WriteInstanceView_Unloaded(object sender, RoutedEventArgs e)
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
