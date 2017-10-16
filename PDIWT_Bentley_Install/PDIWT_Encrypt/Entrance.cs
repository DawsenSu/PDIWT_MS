using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;

namespace PDIWT_Encrypt
{
    public static class Entrance
    {
        public static void VerifyActivationState(Action action)
        {
            var pdiwtkey = Registry.CurrentUser.OpenSubKey(@"Bentley\MicroStation\PDIWTMSADDIN");
            string activationprogrampath = pdiwtkey?.GetValue("ActivateProgramPath") as string;
            if (string.IsNullOrEmpty(activationprogrampath))
            {
                MessageBox.Show("未找到激活程序，安装过程中出现错误，请卸载后重新安装！", "激活错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            PDIWTEncrypt _pdiwt = new PDIWTEncrypt();
            if (RegistryUtilities.GetActivationKeyFromRegistry() != _pdiwt.GenerateActivationCodeString())
            {
                Process.Start(activationprogrampath);
            }
            else
            {
                action();
            }
        }
    }
}
