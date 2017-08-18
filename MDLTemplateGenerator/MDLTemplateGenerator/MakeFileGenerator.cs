using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace MDLTemplateGenerator
{
    public class MakeFileGenerator
    {
        private readonly string VCProjectFilePath = string.Empty;
        private StringBuilder sb = new StringBuilder();
        /*
         * Constructor
         */

        public MakeFileGenerator(string vcprojectfilepath)
        {
            VCProjectFilePath = vcprojectfilepath;
        }

        /*
         * GenerateMkeFile
         */
        private void GenerateVariableDefinition(string appName)
        {
            string vDcontent =
                "#--------------------------------------------------------------------------------------\n#\n#\n" +
                "# $Copyright: (c) 2017 PDIWT Systems, Incorporated. All rights reserved. $\n#\n#\n" +
                "#--------------------------------------------------------------------------------------\n\n" +
                "#----------------------------------------------------------------------\n" +
                "# Variable Defination\n" +
                "#----------------------------------------------------------------------\n" +
                $"appName = {appName}\n" +
                "#MS = C:/PROGRA~1/Bentley/MICROS~1/MICROS~1/\n\n\n" +
                "%warn MS = $(MS)\n" +
                "%warn MSMDE = $(MSMDE)\n" +
                "%warn MSMDE_OUTPUT = $(MSMDE_OUTPUT)\n\n" +
                "baseDir = $(_MakeFilePath)\n" +
                "includeDir = $(baseDir)\n" +
                "sourceDir = $(baseDir)\n" +
                "langSpec = $(baseDir)chinese/\n" +
                "mdlapps = $(MS)Mdlapps/\n\n\n" +
                "nameToDefine = __$(appName)_BUILD__\n" +
                "%include $(mdlMki)cdefapnd.mki\n\n" +
                "PolicyFile = MicroStationPolicy.mki\n\n" +
                "DEFAULT_TARGET_PROCESSOR_ARCHITECTURE = x64\n\n" +
                "mdlMki = $(MSMDE)mki/\n" +
                "mdlLibs = $(MSMDE)library/\n" +
                "mdlInc = $(MSMDE)include/\n\n\n\n" +
                "%include $(mdlMki)mdl.mki\n\n" +
                "o = $(MSMDE_OUTPUT)objects/$(appName)/\n\n" +
                "genSrc = $(o)\n\n" +
                "mdlapps = $(MS)Mdlapps/\n\n" +
                "#----------------------------------------------------------------------\n" +
                "# Create needed output directories if they don't exist\n" +
                "#----------------------------------------------------------------------\n" +
                "always:\n" +
                "\t!~@mkdir $(o)\n" +
                "\t!~@mkdir $(rscObjects)\n" +
                "\t!~@mkdir $(reqdObjs)\n\n" +
                "#----------------------------------------------------------------------\n" +
                "# Inlucde the Heaher files\n" +
                "#----------------------------------------------------------------------\n" +
                "dirToSearch = $(genSrc)\n" +
                "%include $(mdlMki)cincapnd.mki\n\n" +
                "dirToSearch = $(mdlInc)\n" +
                "%include $(mdlMki)cincapnd.mki\n\n" +
                "dirToSearch = $(baseDir)\n" +
                "%include $(mdlMki)cincapnd.mki\n\n" +
                "dirToSearch = $(MSMDE)mki/\n" +
                "%include cincapnd.mki\n\n" +
                "dirToSearch = $(MSMDE)include/\n" +
                "%include cincapnd.mki\n\n";
            sb.Append(vDcontent);
        }

        private void GenerateAppRescoure()
        {
            string aRContent =
                "#-----------------------------------------------------------------------\n" +
                "# Generate app res using rcomp\n" +
                "#-----------------------------------------------------------------------\n" +
                "$(rscObjects)$(appName).rsc     :	$(baseDir)$(appName).r\n\n";
            sb.Append(aRContent);
        }

        private void GenerateCmdRescourceAndHeadFile(IEnumerable<string> rscDependence)
        {
            StringBuilder rscAndHeaderGenerator = new StringBuilder();
            rscAndHeaderGenerator.Append("#-----------------------------------------------------------------------\n" +
                                         "# Generate commandTable and headerfile using rcomp\n" +
                                         "#-----------------------------------------------------------------------\n");
            StringBuilder tempBuilder = new StringBuilder();
            foreach (var rscdep in rscDependence)
                tempBuilder.Append("$(baseDir)" + rscdep + ".h\t");
            tempBuilder.Append("\n\n");

            rscAndHeaderGenerator.Append("$(baseDir)$(appName)cmd.h       : $(baseDir)$(appName)cmd.r\t");
            rscAndHeaderGenerator.Append(tempBuilder);
            rscAndHeaderGenerator.Append("$(rscObjects)$(appName)cmd.rsc  : $(baseDir)$(appName)cmd.r\t");
            rscAndHeaderGenerator.Append(tempBuilder);
            sb.Append(rscAndHeaderGenerator);
        }

        private void GenerateLanguageRescource(IEnumerable<string> langDependence)
        {
            StringBuilder langBuilder = new StringBuilder();
            langBuilder.Append("#----------------------------------------------------------------------\n" +
                               "# Gernerate language resource file using rcomp\n" +
                               "#----------------------------------------------------------------------\n" +
                               "$(rscObjects)$(appName)msg.rsc  :	$(langSpec)$(appName)msg.r\t");
            foreach (var langdep in langDependence)
            {
                langBuilder.Append("$(baseDir)" + langdep + ".h\t");
            }
            langBuilder.Append("\n\n");
            sb.Append(langBuilder);
        }

        private void GenerateTypeRescource(IEnumerable<string> typeDependence)
        {
            StringBuilder typeBuilder = new StringBuilder();
            typeBuilder.Append("#----------------------------------------------------------------------\n" +
                               "# Gernerate application's type & resouce file using rsctype & rcomp\n" +
                               "#----------------------------------------------------------------------\n");
            StringBuilder tempBuilder = new StringBuilder();
            foreach (var typedep in typeDependence)
            {
                tempBuilder.Append("$(baseDir)" + typedep + ".h\t");
            }
            tempBuilder.Append("\n\n");
            typeBuilder.Append("$(rscObjects)$(appName)type.r   :	$(baseDir)$(appName)Type.mt\t");
            typeBuilder.Append(tempBuilder);
            typeBuilder.Append("$(rscObjects)$(appName)type.rsc :	$(rscObjects)$(appName)Type.r\t");
            typeBuilder.Append(tempBuilder);
            sb.Append(typeBuilder);
        }

        private void GenerateUIRescource(IEnumerable<string> uiDependence)
        {
            StringBuilder uirscBuilder = new StringBuilder();
            uirscBuilder.Append("#----------------------------------------------------------------------\n" +
                                "# Gernate all UI resouce\n" +
                                "#----------------------------------------------------------------------\n" +
                                "$(rscObjects)$(appName)ui.rsc       :	$(baseDir)$(appName)ui.r\t");
            foreach (var uidep in uiDependence)
            {
                uirscBuilder.Append("$(baseDir)" + uidep + ".h\t");
            }
            uirscBuilder.Append("\n\n");
            sb.Append(uirscBuilder);
        }

        private void GenerateRescourcesDependence(IEnumerable<string> rscFileNameList)
        {
            StringBuilder rDepContentBuilder = new StringBuilder();
            rDepContentBuilder.Append("#----------------------------------------------------------------------\n" +
                                      "# Define macros for files included in our link and resource merge\n" +
                                      "#----------------------------------------------------------------------\nappRscs         =");
            foreach (var fileName in rscFileNameList)
                rDepContentBuilder.Append("$(rscObjects)" + fileName + ".rsc\t");
            rDepContentBuilder.Append("\n\n");
            sb.Append(rDepContentBuilder);
        }

        private void GenerateLinkLibrary()
        {
            string LinkLibContent =
                "#----------------------------------------------------------------------\n" +
                "# Set up to use linkLibrary.mki (legacy: dlmcomp.mki and dlmlink.mki)\n" +
                "#----------------------------------------------------------------------\n\n" +
                "DLM_NAME = $(appName)\n" +
                "DLM_DEST = $(mdlapps)\n" +
                "DLM_EXPORT_DEST = $(mdlapps)\n" +
                "DLM_OBJECT_FILES = $(MultiCompileObjectList)\n" +
                "DLM_OBJECT_DEST = $(o)\n" +
                "DLM_SPECIAL_LINKOPT = -fixed:no # Notify linker this library does not require a fixed base address to load\n" +
                "DLM_NO_DLS = 0         # USE DLLEXPORT IN .CPP\n" +
                "DLM_NO_DEF = 1\n" +
                "DLM_NOENTRY = 1\n" +
                "DLM_NO_MANIFEST = 1         # If not set linker embeds your current (developer) patched MSVCRT version manifest in output dll.  This is not desirable and produces side-by-side CLIENT ERROR: 14001)\n" +
                "DLM_NO_SIGN = 1         # If not set and no certificate found, ERROR: 'singleton' is not recognized as an internal or external command\n" +
                "%include   dlmAllLibs.mki\n\n";
            sb.Append(LinkLibContent);
        }

        //Todo: 需要将cpp的依赖头文件解析，并加入到依赖列表中
        private void GenerateCompileDependence(IEnumerable<string> cppFileName)
        {
            StringBuilder compileContentBuilder = new StringBuilder();
            compileContentBuilder.Append("#------------------------------------------------\n" +
                                         "# Compile the source files for the DLM\n" +
                                         "#------------------------------------------------\n" +
                                         "MultiCompileDepends = $(_MakeFileSpec)\n" +
                                         "%include MultiCppCompileRule.mki\n\n");
            foreach (var filename in cppFileName)
            {
                compileContentBuilder.Append(
                    $"$(o){filename}$(oext)	:	$(sourceDir){filename}.cpp	${{MultiCompileDepends}}	$(allDepends)\n\n");
            }
            compileContentBuilder.Append("%include MultiCppCompileGo.mki\n\n" +
                                         "always:\n" +
                                         "    !~@mkdir $(DLM_DEST)\n\n" +
                                         "%include $(mdlMki)dlmlink.mki\n\n");
            sb.Append(compileContentBuilder);
        }

        private void GenerateLinkMa()
        {
            string maContent =
                "#----------------------------------------------------------------------\n" +
                "# Link the MA\n" +
                "#----------------------------------------------------------------------\n" +
                "MA_NAME = $(appName)\n" +
                "RIGHTSCOMPLIANT = false\n" +
                "MA_DEST = $(mdlApps)\n" +
                "MA_RSC_FILES = $(rscObjects)$(appName)cmd.rsc\n" +
                "MA_NO_VERSION = 1\n" +
                "MA_EXT = .ma\n" +
                "cmdFile = $(o)$(MA_NAME)link.cmd\n\n";
            sb.Append(maContent);

        }

        private void GenerateMergeObj()
        {
            string mergObj =
                "#-----------------------------------------------------------------------\n" +
                "# Merge Objects into one file\n" +
                "#-----------------------------------------------------------------------\n" +
                "#$(reqdObjs)$(appName).mi : $(appRscs)\n" +
                "\"$(MA_DEST)$(MA_NAME)$(MA_EXT)\" : $(appRscs)\n" +
                "    $(msg)\n" +
                "    >$(o)make.opt\n" +
                "    -o$@\n" +
                "    $(appRscs)\n" +
                "    <\n" +
                "    $(RLibCmd) @$(o)make.opt\n" +
                "    ~time\n\n";
            sb.Append(mergObj);
        }

        /*
         * Get CppFile's Names and RFile's Names
         */

        QueryStatus GetCppAndRNames(ref IEnumerable<string> cppFileNames, ref IEnumerable<string> rFileNames)
        {
            if (!File.Exists(VCProjectFilePath))
                return QueryStatus.CanNotopenFile;
            XDocument xdoc = XDocument.Load(VCProjectFilePath);
            XElement rootElement = xdoc.Root;
            if (rootElement == null)
                return QueryStatus.QueryError;
            XNamespace vcnameNs = rootElement.GetDefaultNamespace();
            cppFileNames = from cppfile in rootElement.Descendants(vcnameNs + "ClCompile")
                           where cppfile.Attribute("Include") != null
                           let cppname = cppfile.Attribute("Include")?.Value
                           where cppname.Split('.').Last().ToLower() == "cpp"
                           select cppname.Split('.').First();

            rFileNames = from rfile in rootElement.Descendants(vcnameNs + "None")
                         where rfile.Attribute("Include") !=null
                         let rname = rfile.Attribute("Include")?.Value
                         where rname.Split('.').Last().ToLower() == "r" || rname.Split('.').Last().ToLower() == "mt"
                         select rname.Split('.').First().Split('\\').Last();
            return QueryStatus.Success;
        }

        /*
         * Get Cppfile Denpendenc file
         */

        QueryStatus GetRDependenceFiles(string rfilePath, out List<string> depFileNames)
        {
            depFileNames = new List<string>();
            if (!File.Exists(rfilePath))
                return QueryStatus.CanNotopenFile;

            var pattern = "(?<=(\\s)*#include(\\s)*\"[\\.\\\\]*)(\\w)+(?=\\.[Hh]\")";
            var options = RegexOptions.ExplicitCapture;
            var regex = new Regex(pattern, options, TimeSpan.FromMilliseconds(1000));
            var input = File.ReadAllText(rfilePath);

            var match = regex.Matches(input);

            if (match.Count != 0)
            {
                depFileNames.AddRange(from object entery in match select entery.ToString());
            }
            return QueryStatus.Success;
        }

        public void GenerateMakeFile()
        {
            IEnumerable<string> cppFileNameList = new List<string>();
            IEnumerable<string> rscFileNameList = new List<string>();
            GetCppAndRNames(ref cppFileNameList, ref rscFileNameList);

            var appName = Path.GetFileNameWithoutExtension(VCProjectFilePath);
            var projectPath = Path.GetDirectoryName(VCProjectFilePath) + "\\";
            GenerateVariableDefinition(appName);
            GenerateAppRescoure();
            List<string> cmdRdepList;
            GetRDependenceFiles(projectPath + appName + ".r", out cmdRdepList);
            GenerateCmdRescourceAndHeadFile(cmdRdepList);

            if (rscFileNameList.Contains(appName + "msg"))
            {
                List<string> msgRdepList;
                GetRDependenceFiles(projectPath + "chinese\\" + appName + "msg.r", out msgRdepList);
                GenerateLanguageRescource(msgRdepList);
            }
            if (rscFileNameList.Contains(appName + "type"))
            {
                List<string> typeDepList;
                GetRDependenceFiles(projectPath + appName + "type.mt", out typeDepList);
                GenerateTypeRescource(typeDepList);
            }
            if (rscFileNameList.Contains(appName + "ui"))
            {
                List<string> uiDepList;
                GetRDependenceFiles(projectPath + appName + "ui.r", out uiDepList);
                GenerateUIRescource(uiDepList);
            }
            GenerateRescourcesDependence(rscFileNameList);
            GenerateLinkLibrary();
            GenerateCompileDependence(cppFileNameList);
            GenerateLinkMa();
            GenerateMergeObj();

            File.WriteAllText(projectPath + appName + ".mke", sb.ToString());
        }

        enum QueryStatus
        {
            CanNotopenFile,
            QueryError,
            Success
        }

    }
}