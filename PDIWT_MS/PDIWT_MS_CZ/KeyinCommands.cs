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
            BeforeKeyExecute(Views.ViewCZ.ShowWindow);
        }
        #endregion

        #region Activation
        private static void BeforeKeyExecute(Action doAction)
        {
            string licensefilepath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)+ @"\Bentley\MicroStation\license.lic";
            PdiwtEncrypt pdiwt= new PdiwtEncrypt();
            if (File.Exists(licensefilepath) && File.ReadAllText(licensefilepath) == pdiwt.GenerateActivationCodeString())
            {
                doAction();
            }
            else
            {
                Views.EncryptView.ShowWindow();
            }
        }
        #endregion
    }
}
