using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using Microsoft.Win32;
using System.IO;
using System.Security.Cryptography;

namespace PDIWTEncrypt
{
    class PdiwtEncrypt
    {
        public PdiwtEncrypt()
        {
            KeyWord = "wirttenbysudongsheng20171006";
        }
        private string KeyWord;

        /// <summary>
        /// 获得用key加密后的字符串，用于对比是否激活
        /// </summary>
        /// <returns>用key加密后的字符串</returns>
        public string GenerateActivationCodeString()
        {
            string codestr;
            return EncryptMacStringWithKey(KeyWord, GetComputerRelatedString(), out codestr) == EncryptStatues.Success ? codestr : null;
        }

        /// <summary>
        /// 获得与本机cpu和disk相关的序列号
        /// </summary>
        /// <returns>返回cpu与硬盘字符串合并后MD5的散列数</returns>
        public string GetComputerRelatedString()
        {
            ComputerInfo computerinfo = ComputerInfo.Instance();
            string cpudiskstring = computerinfo.CpuID + computerinfo.DiskID;
            return GetMd5Hash(MD5.Create(), cpudiskstring).ToUpper();
        }

        /// <summary>
        /// 计算input的MD5的散列值
        /// </summary>
        /// <param name="md5Hash">MD5对象的实例</param>
        /// <param name="input">需要计算散列值的字符串</param>
        /// <returns>计算的散列值</returns>
        private string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// 获取输入字符串的偶数或者奇数位置的子字符串(字符位置以0开头)
        /// </summary>
        /// <param name="inputstring">输入字符串</param>
        /// <param name="takeodd">如果为true则获得偶数位置字符串；false获得奇数位置字符串</param>
        /// <returns>获得的字符串</returns>
        private string GetOddAndEvenSubString(string inputstring, bool takeodd)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < inputstring.Length; i++)
            {
                if (takeodd && (i % 2 == 0))
                    stringBuilder.Append(inputstring[i]);
                if (!takeodd && (i % 2 == 1))
                    stringBuilder.Append(inputstring[i]);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 通过本机MAC物理地址和传入的字符串计算加密字符
        /// </summary>
        /// <param name="key">传入的秘钥字符串</param>
        /// <param name="inputstring">传入的与本机有关的字符串</param>
        /// <param name="codestring">获得的加密密码</param>
        /// <returns>如果成功，返回状态为Success</returns>
        private EncryptStatues EncryptMacStringWithKey(string key, string inputstring, out string codestring)
        {
            codestring = string.Empty;
            if (inputstring == null)
                return EncryptStatues.CanNotGetMacString;
            //Get mac hash code string and extract odd posistion chars
            //Get key hash code string adn extract even posistion chars
            string inputhashoddstring = GetOddAndEvenSubString(GetMd5Hash(MD5.Create(), inputstring), true);
            string keyhashevenstring = GetOddAndEvenSubString(GetMd5Hash(MD5.Create(), key), false);
            //Get the substring length
            int hashstringlength = inputhashoddstring.Length;
            int[] indexnums = new int[hashstringlength];
            for (int i = 0; i < hashstringlength; i++)
                indexnums[i] = (inputhashoddstring[i] + keyhashevenstring[i]) % 36;

            StringBuilder sb = new StringBuilder();
            string fixstring = "abcdefghijklmnopqrstuvwxyz0123456789";
            for (int i = 0; i < hashstringlength; i++)
            {
                sb.Append(fixstring[indexnums[i]]);
                if (i % 4 == 3 && i != (hashstringlength - 1))
                    sb.Append("-");
            }
            codestring = sb.ToString().ToUpper();
            return EncryptStatues.Success;
        }

        enum EncryptStatues
        {
            Success,
            CanNotGetMacString
        }
    }

    class ComputerInfo
    {
        private static ComputerInfo _instance;
        private ComputerInfo()
        {
            MacAddress = GetMacAddress();
            CpuID = GetCpuID();
            DiskID = GetDiskID();
        }
        public static ComputerInfo Instance()
        {
            if (_instance == null)
                _instance = new ComputerInfo();
            return _instance;
        }
        public string MacAddress { get; set; }
        public string CpuID { get; set; }
        public string DiskID { get; set; }

        /// <summary>
        ///  获取本机的MAC物理地址字符串（以-号分隔）
        /// </summary>
        /// <returns>如读取成功则返回MAC地址字符串；否则返回null</returns>
        private string GetMacAddress()
        {
            try
            {
                string mac = null;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"])
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                return mac.Replace(':', '-');
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取本机cpu的序列号信息
        /// </summary>
        /// <returns>cpu的序列号字符串；获取失败则返回null</returns>
        private string GetCpuID()
        {
            try
            {
                string cpuid = null;
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (var mo in moc)
                {
                    cpuid += mo["ProcessorId"].ToString();
                }
                return cpuid;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获得本机的硬盘序列号
        /// </summary>
        /// <returns>如果成功则返回硬盘序列号字符串；如果无法获取则返回null</returns>
        private string GetDiskID()
        {
            try
            {
                string diskid = null;
                ManagementClass mc = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (var mo in moc)
                {
                    diskid += mo["Model"].ToString();
                }
                return diskid;
            }
            catch
            {
                return null;
            }
        }
    }

    static class RegistryUtilites
    {
        /// <summary>
        /// 将激活秘钥写入注册项SOFTWARE\Bentley\PDIWT_MSADDIN中
        /// </summary>
        /// <param name="infomation">要写入的信息（秘钥）</param>
        /// <param name="key">存储infomation的键</param>
        public static void WriteActivationKeyToRegistry(string infomation, string key = "ActivationKey")
        {
            RegistryKey pdiwtmsaddinkey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Bentley\PDIWT_MSADDIN");
            pdiwtmsaddinkey.SetValue(key, infomation);
            pdiwtmsaddinkey.Close();
        }
        /// <summary>
        /// 获得注册项中SOFTWARE\Bentley\PDIWT_MSADDIN的ActivationKey键值
        /// </summary>
        /// <returns>如果存在则返回键值内容；如果不存在则返回null</returns>
        public static string GetActivationKeyFromRegistry()
        {
            RegistryKey pdiwtmsaddinkey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Bentley\PDIWT_MSADDIN");
            string activationkey = pdiwtmsaddinkey.GetValue("ActivationKey")?.ToString();
            return activationkey;
        }
    }
}
