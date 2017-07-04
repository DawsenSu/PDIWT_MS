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
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;

namespace PDIWT_Bentley_Install
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {            
            string mdlappPath = tb_dllpath.Text;
            if (!Directory.Exists(mdlappPath))
            {
                System.Windows.MessageBox.Show("不存在" + mdlappPath + "目录\n请检查选择的MircoStation Connect Update3根目录是否正确！", "安装错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DirectoryInfo dllDire, confDire;
            dllDire = new DirectoryInfo(@".\Dll");
            confDire = new DirectoryInfo(@".\Config");
            foreach (var dllfiles in dllDire.GetFiles())
            {
                dllfiles.CopyTo(tb_dllpath.Text + @"\" + dllfiles.Name, true);
            }
            foreach (var configfiles in confDire.GetFiles())
            {
                configfiles.CopyTo(tb_config.Text + @"\" + configfiles.Name, true);
            }
            System.Windows.MessageBox.Show("安装完成！");
            btn_install.IsEnabled = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //FolderBrowserDialog fbD = new FolderBrowserDialog();
            //fbD.ShowNewFolderButton = false;
            //fbD.Description = "选择MircoStation Connect版的安装根目录";
            ////fbD.RootFolder = Environment.SpecialFolder.CommonProgramFiles;
            //if (fbD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    DirectoryInfo selectedDire = new DirectoryInfo(fbD.SelectedPath);
            //    tempDire = selectedDire.Name;
            //    tb_dllpath.Text = fbD.SelectedPath + @"\MicroStation\Mdlapps";
            //}

            //string bentleyLibPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            //for (int i = 1; i < 10; i++)
            //{
            //    if (!Directory.Exists(bentleyLibPath + @"\Bentley\MicroStation\10.0.0_" + i))
            //    {
            //        if (i == 1)
            //        {
            //            bentleyLibPath += @"\Bentley\MicroStation\10.0.0\prefs";
            //        }
            //        else
            //        {
            //            bentleyLibPath += @"\Bentley\MicroStation\10.0.0_" + (i - 1) + @"\prefs";
            //        }
            //        break;
            //    }
            //}
            //tb_config.Text = bentleyLibPath;
            //btn_install.IsEnabled = true;
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey hkMircoStation = hklm.OpenSubKey(@"SOFTWARE\Bentley\MicroStation\{39949BF5-7E21-4A7B-A640-6E7199B7D588}");        
            tb_dllpath.Text = hkMircoStation?.GetValue("ProgramPath") as string;

        }
        string tempDire;
    }
}
