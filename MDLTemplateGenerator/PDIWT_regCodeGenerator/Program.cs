using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace PDIWT_regCodeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(GetSystemInfo.GetUserName());
            //Console.WriteLine(GetSystemInfo.GetMacAddrLocal());
            //Console.WriteLine(GetSystemInfo.GetClientLocalIPv6Address());
            //Console.WriteLine(GetSystemInfo.GetClientLocalIPv4Address());
            //Console.WriteLine(GetSystemInfo.GetCpuID());
            //Console.WriteLine(GetSystemInfo.GetTotalPhysicalMemory());
            //Console.WriteLine(GetSystemInfo.GetMotherBoardID());
            string macinfo = GetSystemInfo.GetMacAddrLocal();
            MD5 md5Provider = new MD5CryptoServiceProvider();
            byte[] array1 = md5Provider.ComputeHash(Encoding.Default.GetBytes(macinfo));
            string mainKey = "45EB32A4C777AC0DC607D97DEA937D82";
            StringBuilder sb = new StringBuilder();
            foreach (byte t in array1)
            {
                sb.Append(mainKey[t % mainKey.Length]);
            }

            Console.WriteLine(sb.ToString());
            Console.ReadKey();
        }
    }
}
