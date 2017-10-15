using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Diagnostics;

namespace PDIWT_StartUp
{
    class Program
    {
        static void Main(string[] args)
        {
            string msapplicationpath = GetMicroStationPath();
            if (msapplicationpath != null)
            {
                StartUpMSWithConfiguration(msapplicationpath);
            }
            else
                MessageBox.Show("本地计算机未安装MicroStation CONNECT Edition\n程序启动失败", "启动失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //Console.ReadKey();

        }

        [RegistryPermission(SecurityAction.PermitOnly,Read = @"HKEY_LOCAL_MACHINE\SOFTWARE\Bentley\MicroStation\{39949BF5-7E21-4A7B-A640-6E7199B7D588}")]
        public static string GetMicroStationPath()
        {
            string mslocation = null;
            RegistryKey msregistryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Bentley\MicroStation\{39949BF5-7E21-4A7B-A640-6E7199B7D588}");
            mslocation = msregistryKey?.GetValue("ApplicationPath") as string;
            return mslocation;
        }

        public static void StartUpMSWithConfiguration(string mspath)
        {
            ProcessStartInfo msprocessStartInfo = new ProcessStartInfo()
            {
                FileName = mspath,
                Arguments=@"-WKPDIWT -WWXQH"
            };
            Process.Start(msprocessStartInfo);            
        }
    }
}
