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

namespace PDIWT_MS.Test
{
    /// <summary>
    /// ToolSettingWPF.xaml 的交互逻辑
    /// </summary>
    public partial class WPFTestToolSetting : UserControl
    {
        #region Bentley DockableWindow
        private static BM.WPF.ToolSettingsHost currentControl;
        private BM.AddIn m_addIn;

        private WPFTestToolSetting(BM.AddIn addIn)
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

            currentControl = new BM.WPF.ToolSettingsHost();
            currentControl.Title = "Tool Setting Test";
            currentControl.Content = new WPFTestToolSetting(addIn);
            currentControl.Attach(addIn);
            currentControl.Show();
        }


        #endregion

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }
        
        public static void CloseWindow()
        {
            if (null != currentControl)
            {
                currentControl.Detach();
                currentControl = null;
            }
        }
    }
}
