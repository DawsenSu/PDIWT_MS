using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Net;

namespace PDIWT_regCodeGenerator
{
    class GetSystemInfo
    {
        public static string GetUserName()
        {
            try
            {
                string strUseerName = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (var mo in moc)
                {
                    strUseerName = mo["UserName"].ToString();
                }
                moc = null;
                mc = null;
                return strUseerName;
            }
            catch
            {
                return "unknown";
            }
        }

        public static string GetMacAddrLocal()
        {
            string macAddr = string.Empty;
            try
            {
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (var mo in moc)
                {
                    if (Convert.ToBoolean(mo["IPEnabled"]) == true)
                    {
                        macAddr = mo["MacAddress"].ToString();
                        macAddr = macAddr.Replace(':', '-');
                    }
                    mo.Dispose();
                }
                if (string.IsNullOrEmpty(macAddr))
                {
                    return "unknown";
                }
                else
                {
                    return macAddr;
                }
            }
            catch
            {
                return "unknown";
            }
        }

        public static string GetClientLocalIPv6Address()
        {
            string strLocalIP = string.Empty;
            try
            {
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress[] ipAddress = ipHost.AddressList;
                foreach (var address in ipAddress)
                {
                    if (address.IsIPv6LinkLocal)
                    {
                        return address.ToString();
                    }
                }
                return "unknown";
            }
            catch
            {
                return "unknown";
            }
        }

        public static string GetClientLocalIPv4Address()
        {
            string strLocalIP = string.Empty;
            try
            {
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress[] ipAddress = ipHost.AddressList;
                foreach (var address in ipAddress)
                {
                    if (!address.IsIPv6LinkLocal)
                    {
                        return address.ToString();
                    }
                }
                return "unknown";
            }
            catch
            {
                return "unknown";
            }
        }

        public static string GetCpuID()
        {
            try
            {
                string strCpuID = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_Processor");
                foreach (var mo in mc.GetInstances())
                {
                    strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                }
                mc.Dispose();
                return strCpuID;
            }
            catch
            {
                return "unknown";
            }
        }

        public static string GetTotalPhysicalMemory()
        {
            try
            {
                string strTotalPhysicalMemory = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    strTotalPhysicalMemory = mo["TotalPhysicalMemory"].ToString();
                }
                moc = null;
                mc = null;
                return strTotalPhysicalMemory;
            }
            catch
            {
                return "unknown";
            }
        }

        public static string GetMotherBoardID()
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_BaseBoard");
                ManagementObjectCollection moc = mc.GetInstances();
                string strID = null;
                foreach (var o in moc)
                {
                    var mo = (ManagementObject) o;
                    strID = mo.Properties["SerialNumber"].Value.ToString();
                    break;
                }
                return strID;
            }
            catch
            {
                return "unknown";
            }
        }
    }
}
