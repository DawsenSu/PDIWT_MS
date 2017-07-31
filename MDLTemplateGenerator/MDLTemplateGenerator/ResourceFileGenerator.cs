using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MDLTemplateGenerator
{
    class ResourceFileGenerator
    {
        private string VCPrjName = String.Empty;
        private string VCPrjDir = String.Empty;
        public ResourceFileGenerator(string vcprjPath)
        {
            VCPrjName = Path.GetFileNameWithoutExtension(vcprjPath);
            VCPrjDir = Path.GetDirectoryName(vcprjPath) + "\\";
        }

        void GenerateMainResource()
        {
            string mainRscContent =
                "#include <Mstn\\MdlApi\\cmdclass.r.h>\n" +
                "#include <Mstn\\MdlApi\\rscdefs.r.h>\n" +
                $"#include \"{VCPrjName}ids.h\"\n\n" +
                $"DllMdlApp DLLAPP_{VCPrjName} =\n" +
                "{\n" +
                $"\tL\"{VCPrjName}\",L\"{VCPrjName}\"\n" +
                "}";
            File.WriteAllText(VCPrjDir + VCPrjName + ".r",mainRscContent);
        }

        void GenerateCmdRescource()
        {
            string cmdRscContent =
                "#pragma suppressREQCmds\n\n" +
                "#include <Mstn\\MdlApi\\cmdclass.r.h>\n" +
                "#include <Mstn\\MdlApi\\rscdefs.r.h>\n" +
                $"#include \"{VCPrjName}ids.h\"\n\n" +
                "/*----------------------------------------------------------------------+\n" +
                "Main command table porotype\n" +
                "{< number >, < subtableidentifier > (or zero if no subtable), < commandclass >, < options >, \"<commandword >\"},\n" +
                "< commandclass >\n" +
                "            Class  Value Class  Value\n" +
                "            PLACEMENT  1  COMPRESS  15\n" +
                "            VIEWING  2  REFERENCE  16\n" +
                "            FENCE  3  DATABASE  17\n" +
                "            PARAMETERS  4  DIMENSION  18\n" +
                "            LOCKS  5  LOCATE  19\n" +
                "            USERCOMMAND  6  TUTORIAL  20\n" +
                "            MANIPULATION  7  WORKINGSET  21\n" +
                "            SHOW  8  ELEMENTLIST  22\n" +
                "            PLOT  9  UNDO  23\n" +
                "            NEWFILE  10  SUBPROCESS  24\n" +
                "            MEASURE  11  VIEWPARAM  25\n" +
                "            INPUT  12  VIEWIMMEDIATE  26\n" +
                "            CELLLIB  13  WINDOWMAN  27\n" +
                "            FILEDESIGN  14  DIALOGMAN  28\n" +
                "            INHERIT\n\n" +
                "< options> DEF | REQ | TRY | CMDSTR(n) | HID\n" +
                "+ ----------------------------------------------------------------------*/\n\n" +
                "#define CT_NONE\t0\n" +
                "#define CT_MAIN\t1\n" +
                "#define CT_PDIWT\t2\n\n" +
                "CommandTable CT_MAIN =\n" +
                "{\n" +
                "\t\t{ 1,\tCT_PDIWT,\tPLACEMENT,\tREQ,\t\"PDIWT\"},\n" +
                "};\n\n" +
                "CommandTable CT_PDIWT =\n" +
                "{\n" +
                $"\t\t{{ 1,\tCT_NONE,\tINHERIT,\tNONE,\t\"{VCPrjName.ToUpper()}\"}},\n" +
                "};\n";
            File.WriteAllText(VCPrjDir + VCPrjName + "cmd.r",cmdRscContent);
        }

        void GeneratemsgRescource()
        {
            Directory.CreateDirectory(VCPrjDir + "chinese");
            string msgRscContent =
                "#include <Mstn\\MdlApi\\cmdclass.r.h>\n" +
                "#include <Mstn\\MdlApi\\rscdefs.r.h>\n" +
                $"#include \"..\\{VCPrjName}ids.h\"\n\n" +
                "MessageList STRINGLISTID_Commands =\n" +
                "{{\n" +
                $"\t{{CMDNAME_{VCPrjName},\"COMAND_{VCPrjName}\"}},\n" +
                "}};\n\n" +
                "MessageList STRINGLISTID_Prompts =\n" +
                "{{\n" +
                $"\t{{PRT_{VCPrjName},\"PRT_{VCPrjName}\"}},\n" +
                "}};\n\n" +
                "MessageList STRINGLISTID_Error =\n" +
                "{{\n" +
                $"\t{{ERR_{VCPrjName},\"ERROR_{VCPrjName}\"}},\n" +
                "}};\n";
            File.WriteAllText(VCPrjDir + $"chinese\\{VCPrjName}msg.r",msgRscContent);
        }

        public void GenerateRscources()
        {
            GenerateMainResource();
            GenerateCmdRescource();
            GeneratemsgRescource();
        }
    }
}
