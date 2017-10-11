using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using BD = Bentley.DgnPlatformNET;
using BCOM = Bentley.Interop.MicroStationDGN;
using System.Runtime.InteropServices;
using System.Windows;
using BM = Bentley.MstnPlatformNET;

namespace PDIWT_MS_CZ
{
    class KeyinCommands
    {
        #region (PDIWT_MS CZ) command
        public static void DrawAll(string unparsed)
        {
            PDIWTEncrypt.EncryptEntrance.BeforeKeyExecute(Views.MainView.ShowWindow);
        }
        #endregion
    }
}
