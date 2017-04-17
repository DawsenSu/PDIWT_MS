using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;


namespace PDIWT_BentleyDll_install.ViewModels
{
    public class installViewModel : ViewModelBase
    {

        public string InstallDllPath
        {
            get { return GetProperty(() => InstallDllPath); }
            set { SetProperty(() => InstallDllPath, value); }
        }

        public string DgnLibPath
        {
            get { return GetProperty(() => DgnLibPath); }
            set { SetProperty(() => DgnLibPath, value); }
        }
        [Command]
        public void Install()
        {
            string bentleyLibPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            bentleyLibPath += @"\Bentley\MicroStation\10.0.0_3\prefs";
            if (!Directory.Exists(bentleyLibPath))
            {
                MessageBox.Show("不存在" + bentleyLibPath + "目录\n请检查本机是否正确安装MircoStation Connect Update3!", "安装错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DgnLibPath = bentleyLibPath;
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey hkSoftware = hklm.OpenSubKey("Software");
            RegistryKey hkBentley = hkSoftware.OpenSubKey("Bentley");
            RegistryKey hkMircoStation = hkBentley.OpenSubKey("MicroStation");
            string subKeyTemp = hkMircoStation.GetSubKeyNames()[1];
            //RegistryKey hkdll = hkMircoStation.OpenSubKey("{39949BF5-7E21-4A7B-A640-6E7199B7D588}");
            //InstallDllPath = (string)hkdll.GetValue("InstallDir") + @"\MicroStation\Mdlapps";

        }
    }
}