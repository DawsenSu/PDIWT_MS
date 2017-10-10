using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDIWTEncrypt
{
    public class EncryptEntrance
    {
        #region Activation
        private static void BeforeKeyExecute(Action doAction)
        {
            string licensefilepath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Bentley\MicroStation\license.lic";
            PdiwtEncrypt pdiwt = new PdiwtEncrypt();
            if (File.Exists(licensefilepath) && File.ReadAllText(licensefilepath) == pdiwt.GenerateActivationCodeString())
            {
                doAction();
            }
            else
            {
                EncryptView.ShowWindow();
            }
        }
        #endregion
    }
}
