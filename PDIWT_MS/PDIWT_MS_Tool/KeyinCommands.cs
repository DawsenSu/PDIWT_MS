using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BD = Bentley.DgnPlatformNET;
using BCOM = Bentley.Interop.MicroStationDGN;
using System.Runtime.InteropServices;
using BM = Bentley.MstnPlatformNET;
namespace PDIWT_MS_Tool
{
    class KeyinCommands
    {
        #region Armor command
        public static void SetArmorCommand(string unparsed)
        {
            WindowHost.SetArmor_WindowHost.ShowWindow();
        }
        #endregion
    }
}
