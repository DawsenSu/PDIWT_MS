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
using DevExpress.Xpf.Grid;
using BMWPF = Bentley.MstnPlatformNET.WPF;

namespace PDIWT_MS_CZ.Views
{
    /// <summary>
    /// Interaction logic for ViewCZ.xaml
    /// </summary>
    public partial class ViewCZ : Window
    {
        private ViewCZ()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.ViewCZViewModel();
            m_wpfHelper = new BMWPF.WPFInteropHelper(this);
            m_wpfHelper.Attach(Program.Addin, true, "ViewCZ");
        }

        static ViewCZ m_windowhost;
        BMWPF.WPFInteropHelper m_wpfHelper;

        public static void ShowWindow()
        {
            if (m_windowhost != null)
            {
                m_windowhost.Focus();
                return;
            }
            m_windowhost = new ViewCZ();
            m_windowhost.Icon = new BitmapImage(new Uri("pack://application:,,,/PDIWT_MS_CZ;component/Resources/Image/Icons/Wharf.ico", UriKind.RelativeOrAbsolute));
            m_windowhost.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            m_wpfHelper.Detach();
            m_wpfHelper.Dispose();
            m_windowhost = null;
        }

        private void TableView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            if(gridcontrol_gs.VisibleRowCount%2 == 0)
                gridcontrol_gs.SetCellValue(e.RowHandle, "IntervalType", "格栅间距");
            else
                gridcontrol_gs.SetCellValue(e.RowHandle, "IntervalType", "格栅宽度");
        }

    }
}
