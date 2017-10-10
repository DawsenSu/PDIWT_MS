using System;
using System.Collections.Generic;
using System.IO;
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
using System.Diagnostics;
using Bentley.DgnPlatformNET;
using BMWPF = Bentley.MstnPlatformNET.WPF;

namespace PDIWT_MS_CZ.Views
{
    /// <summary>
    /// Interaction logic for EncryptView.xaml
    /// </summary>
    public partial class EncryptView : Window
    {
        private EncryptView()
        {
            InitializeComponent();
            _pdiwt = new PdiwtEncrypt();
            textbox_ComputerRelated.Text = _pdiwt.GetComputerRelatedString();
            m_wpfHelper = new BMWPF.WPFInteropHelper(this);
            m_wpfHelper.Attach(Program.Addin, true, "EncryptView");
        }

        static EncryptView m_windowhost;
        BMWPF.WPFInteropHelper m_wpfHelper;
        private readonly PdiwtEncrypt _pdiwt;
        public static void ShowWindow()
        {
            if (m_windowhost != null)
            {
                m_windowhost.Focus();
                return;
            }
            m_windowhost = new EncryptView();
            m_windowhost.Show();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            m_wpfHelper.Detach();
            m_wpfHelper.Dispose();
            m_windowhost = null;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ActivateButton_OnClick(object sender, RoutedEventArgs e)
        {
            string licensefilepath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Bentley\MicroStation\license.lic";
            if (textbox_ActivationKey.Text != _pdiwt.GenerateActivationCodeString())
            {
                MessageBox.Show("激活码错误请重新输入！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                textbox_ActivationKey.Text = string.Empty;
                return;
            }
            File.WriteAllText(licensefilepath, textbox_ActivationKey.Text);
            MessageBox.Show("激活成功，请再次运行程序", "激活成功", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

    }
}
