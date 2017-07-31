using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDLTemplateGenerator
{
    class ClassFileGenerator
    {
        private string VCPrjName = String.Empty;
        private string VCPrjDir = String.Empty;

        public ClassFileGenerator(string vcprjPath)
        {
            VCPrjName = Path.GetFileNameWithoutExtension(vcprjPath);
            VCPrjDir = Path.GetDirectoryName(vcprjPath) + "\\";
        }

        void GenerateMainCpp()
        {
            string maincppContent =
                $"#include \"{VCPrjName}.h\"\n\n" +
                $"void CommandFunction_{VCPrjName}(WCharCP unparsed){{}}\n\n"+
                "static MdlCommandNumber s_commandNumber[] =\n" +
                "{\n" +
                $"\t{{ CmdHandler(CommandFunction_{VCPrjName}),CMD_PDIWT_{VCPrjName.ToUpper()}}},\n" +
                "\tnullptr\n" +
                "};\n\n" +
                "extern \"C\" void MdlMain(int argc, WCharCP argv[])\n" +
                "{\n" +
                "\tRscFileHandle rfHandle;\n" +
                "\tif (mdlResource_openFile(&rfHandle, nullptr, RSC_READONLY))\n" +
                "\t{\n" +
                "\t\tmdlOutput_errorU(L\"无法打开资源文件\");\n" +
                "\t\tmdlSystem_exit(-1, 1);\n" +
                "\t}\n" +
                "\tmdlSystem_registerCommandNumbers(s_commandNumber);\n" +
                "\tmdlParse_loadCommandTable(nullptr);\n" +
                "\tmdlState_registerStringIds(STRINGLISTID_Commands, STRINGLISTID_Prompts);\n" +
                "}";
            File.WriteAllText(VCPrjDir+VCPrjName+".cpp",maincppContent);
        }

        void GenerateMainHeader()
        {
            string mainHeaderContent =
                "#pragma once\n" +
                "#include <Mstn\\MstnDefs.h>\n" +
                "#include <Mstn\\MdlApi\\MdlApi.h>\n" +
                "#include <Mstn\\MstnPlatformAPI.h>\n" +
                "#include <DgnPlatform\\DgnPlatformApi.h>\n" +
                "#include <Mstn\\PSolid\\mssolid.h>\n" +
                "#include <Mstn\\PSolid\\mssolid.fdf>\n\n" +
                "#include <DgnView\\DgnElementSetTool.h>\n" +
                "#include <DgnView\\DgnTool.h>\n" +
                "#include <DgnView\\AccuDraw.h>\n" +
                "#include <DgnView\\AccuSnap.h>\n\n" +
                $"#include \"{VCPrjName}cmd.h\"\n" +
                $"#include \"{VCPrjName}ids.h\"\n\n" +
                "USING_NAMESPACE_BENTLEY\n" +
                "USING_NAMESPACE_BENTLEY_DGNPLATFORM\n" +
                "USING_NAMESPACE_BENTLEY_MSTNPLATFORM\n" +
                "USING_NAMESPACE_BENTLEY_MSTNPLATFORM_ELEMENT\n\n" +
                "/************************************************************************/\n" +
                "/* Command Function													*/\n" +
                "/************************************************************************/\n" +
                $"void CommandFunction_{VCPrjName}(WCharCP);";
            File.WriteAllText(VCPrjDir + VCPrjName + ".h", mainHeaderContent);
        }

        void GenerateIdHeader()
        {
            string idHeaderContent =
                "#pragma once\n" +
                "/************************************************************************/\n" +
                "/* Application ID														*/\n" +
                "/************************************************************************/\n" +
                $"#define DLLAPP_{VCPrjName}	1\n\n" +
                "/************************************************************************/\n" +
                "/* MessageList'ID														*/\n" +
                "/************************************************************************/\n" +
                "#define STRINGLISTID_Commands	0\n" +
                "#define STRINGLISTID_Prompts	1\n" +
                "#define STRINGLISTID_Error		2\n\n" +
                "/************************************************************************/\n" +
                "/* Command Name ID														*/\n" +
                "/************************************************************************/\n" +
                $"#define CMDNAME_{VCPrjName}	1\n\n" +
                "/************************************************************************/\n" +
                "/* Prompt	ID															*/\n" +
                "/************************************************************************/\n" +
                $"#define PRT_{VCPrjName}	1\n\n" +
                "/************************************************************************/\n" +
                "/* Error string ID														*/\n" +
                "/************************************************************************/\n" +
                $"#define ERR_{VCPrjName}		1\n\n";
            File.WriteAllText(VCPrjDir + VCPrjName + "ids.h", idHeaderContent);
        }

        public void GenerateClasses()
        {
            GenerateMainHeader();
            GenerateIdHeader();
            GenerateMainCpp();
        }
    }
}
