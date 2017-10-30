using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using Microsoft.Win32;
namespace PDIWT_Encrypt
{
    public static class RegistryUtilities
    {
        /// <summary>
        /// 将激活秘钥写入注册项\SOFTWARE\Bentley\MicroStation\PDIWTMSADDIN中
        /// </summary>
        /// <param name="infomation">要写入的信息（秘钥）</param>
        /// <param name="key">存储infomation的键</param>
        public static void WriteActivationKeyToRegistry(string infomation, string key = "ActivationKey")
        {
            RegistryKey pdiwtmsaddinkey = Registry.CurrentUser.CreateSubKey(@"Bentley\MicroStation\PDIWTMSADDIN", true);
            pdiwtmsaddinkey.SetValue(key, infomation);
            pdiwtmsaddinkey.Close();
        }

        /// <summary>
        /// 获得注册项中\SOFTWARE\Bentley\MicroStation\PDIWTMSADDIN的ActivationKey键值
        /// </summary>
        /// <returns>如果存在则返回键值内容；如果不存在则返回null</returns>
        public static string GetActivationKeyFromRegistry(string registrykey = "ActivationKey")
        {
            RegistryKey pdiwtmsaddinkey = Registry.CurrentUser.OpenSubKey(@"Bentley\MicroStation\PDIWTMSADDIN", RegistryRights.FullControl);
            string activationkey = pdiwtmsaddinkey?.GetValue(registrykey)?.ToString();
            return activationkey;
        }
    }
}
