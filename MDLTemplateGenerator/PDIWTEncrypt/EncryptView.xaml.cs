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

namespace PDIWTEncrypt
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
        }

        static EncryptView _windowhost;
        private readonly PdiwtEncrypt _pdiwt;
        public static void ShowWindow()
        {
            if (_windowhost != null)
            {
                _windowhost.Focus();
                return;
            }
            _windowhost = new EncryptView();
            _windowhost.Show();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _windowhost = null;
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
            RegistryUtilites.WriteActivationKeyToRegistry(licensefilepath, textbox_ActivationKey.Text);
            MessageBox.Show("激活成功，请再次运行程序", "激活成功", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(textbox_ComputerRelated.Text);
                MessageBox.Show("已将序列号复制到剪贴板！", "复制成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("复制错误！");
            }
        }

        private void PasteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IDataObject data = Clipboard.GetDataObject();
                if(data.GetDataPresent(DataFormats.Text))
                {
                    textbox_ActivationKey.Text = (string)data.GetData(DataFormats.UnicodeText, true);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("粘贴错误！");
            }
        }
    }
}
