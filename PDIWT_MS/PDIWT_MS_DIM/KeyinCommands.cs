using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDIWT_MS_DIM
{
    using Models;

    class KeyinCommands
    {
        public static void DIM(string unparsed)
        {
            FastWholeDimTool.InstallNewInstance();
        }
    }
}
