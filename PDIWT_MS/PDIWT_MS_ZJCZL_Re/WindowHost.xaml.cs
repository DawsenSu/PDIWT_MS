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
using BMWPF = Bentley.MstnPlatformNET.WPF;

namespace PDIWT_MS_ZJCZL_Re
{
    /// <summary>
    /// WindowHost.xaml 的交互逻辑
    /// </summary>
    public partial class WindowHost : Window
    {
        private WindowHost()
        {
            InitializeComponent();

            _mWpfHelper = new BMWPF.WPFInteropHelper(this);
            _mWpfHelper.Attach(Program.Addin, true, "WindowHost");
        }

        static WindowHost _mWindowhost;
        readonly BMWPF.WPFInteropHelper _mWpfHelper;

        public static void ShowWindow()
        {
            if (_mWindowhost != null)
            {
                _mWindowhost.Focus();
                return;
            }
            _mWindowhost = new WindowHost();
            _mWindowhost.Icon = new BitmapImage(new Uri("pack://application:,,,/PDIWT_MS_ZJCZL_Re;component/Resources/Image/Nonya Kueh.ico", UriKind.RelativeOrAbsolute));
            _mWindowhost.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            _mWpfHelper.Detach();
            _mWpfHelper.Dispose();
            _mWindowhost = null;
        }

    }
}
