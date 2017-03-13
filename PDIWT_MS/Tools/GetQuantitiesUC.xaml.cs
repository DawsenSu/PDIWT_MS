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

namespace PDIWT_MS.Tools
{
    /// <summary>
    /// GetQuantitiesUC.xaml 的交互逻辑
    /// </summary>
    public partial class GetQuantitiesUC : UserControl
    {
        private static BM.WPF.DockableWindow currentControl;
        private static BM.AddIn m_addIn;

        public GetQuantitiesUC(BM.AddIn addIn)
        {
            m_addIn = addIn;
            InitializeComponent();
        }

        internal static void ShowWindow(BM.AddIn addIn)
        {
            if (null!=currentControl)
            {
                currentControl.Focus();
                return;
            }

            currentControl = new BM.WPF.DockableWindow();
            currentControl.Attach(addIn, "wpfControl", new System.Drawing.Size(150, 100));
            currentControl.Content = new GetQuantitiesUC(addIn);
            //不能停靠
            currentControl.WindowContent.CanDockHorizontally = false;
            currentControl.WindowContent.CanDockVertically = false;
            currentControl.WindowContent.CanDockInCenter = false;
            currentControl.Title = "获得工程量";
            currentControl.Show();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            currentControl = null;
        }
    }
}
