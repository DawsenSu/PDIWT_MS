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
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;

namespace PDIWT_Encrypt.Activation
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _pdiwt = new PDIWTEncrypt();
            textbox_ComputerRelated.Text = _pdiwt.GetComputerRelatedString();
        }
        private readonly PDIWTEncrypt _pdiwt;
        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ActivateButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (textbox_ActivationKey.Text != _pdiwt.GenerateActivationCodeString())
            {
                MessageBox.Show("激活码错误请重新输入！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                textbox_ActivationKey.Text = string.Empty;
                return;
            }
            RegistryUtilities.WriteActivationKeyToRegistry(textbox_ActivationKey.Text);
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
                if (data.GetDataPresent(DataFormats.Text))
                {
                    textbox_ActivationKey.Text = (string)data.GetData(DataFormats.UnicodeText, true)??"";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("粘贴错误！");
            }
        }
    }
}
