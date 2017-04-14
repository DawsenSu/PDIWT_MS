using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BD = Bentley.DgnPlatformNET;
using BCOM = Bentley.Interop.MicroStationDGN;
using System.Runtime.InteropServices;
using BM = Bentley.MstnPlatformNET;
namespace PDIWT_MS_ZJCZL
{
    class KeyinCommands
    {
        #region command
        public static void Command(string unparsed)
        {
            WindowHost.ShowWindow();
        }
        #endregion
    }
}
