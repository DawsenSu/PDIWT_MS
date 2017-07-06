using System;
using DevExpress.Xpf.Core;

using Bentley.MstnPlatformNET.WPF;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PDIWT_MS_ZJCZL
{
    /// <summary>
    /// Interaction logic for WindowHost.xaml
    /// </summary>
    public partial class WindowHost : Window
    {
        private static WindowHost m_window1;
        WPFInteropHelper m_wndHelper;

        public WindowHost()
        {
            InitializeComponent();

            m_wndHelper = new WPFInteropHelper(this);
            m_wndHelper.Attach(PDIWT_MS_Test.Program.Addin, true, "WPFWindowTest");
        }

        public static void ShowWindow()
        {
            if (m_window1 != null)
            {
                m_window1.Focus();
                return;
            }
            m_window1 = new WindowHost();
            //m_window1.Icon = new BitmapImage(new Uri("pack://application:,,,/PDIWT_MS_ZJCZL;component/Resources/Image/Nonya Kueh.ico", UriKind.RelativeOrAbsolute));
            m_window1.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            m_wndHelper.Detach();
            m_wndHelper.Dispose();
            m_window1 = null;
        }
    }
}
