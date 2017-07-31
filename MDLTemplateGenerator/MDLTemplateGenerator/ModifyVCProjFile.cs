using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDLTemplateGenerator
{
    class ModifyVCProjFile
    {
        private string VCPrjName = String.Empty;
        private string VCPrjDir = String.Empty;
        private XDocument xdoc;
        private XElement root;
        private XNamespace xNs;
        public ModifyVCProjFile(string vcprjPath)
        {
            VCPrjName = Path.GetFileNameWithoutExtension(vcprjPath);
            VCPrjDir = Path.GetDirectoryName(vcprjPath) + "\\";
            xdoc = XDocument.Load(vcprjPath);
            root = xdoc.Root;
            xNs = root.GetDefaultNamespace();
        }

        void AddFiles()
        {
            root.Add(
                    new XElement(xNs + "ItemGroup",
                        new XElement(xNs + "ClInclude", new XAttribute("Include", $"{VCPrjName}.h")),
                        new XElement(xNs + "ClInclude", new XAttribute("Include", $"{VCPrjName}ids.h"))
                    ));
            root.Add(
                    new XElement(xNs + "ItemGroup",
                        new XElement(xNs + "ClCompile", new XAttribute("Include", $"{VCPrjName}.cpp"))
                    ));
            root.Add(
                    new XElement(xNs + "ItemGroup",
                        new XElement(xNs + "None", new XAttribute("Include", $"chinese\\{VCPrjName}msg.r")),
                        new XElement(xNs + "None", new XAttribute("Include", $"{VCPrjName}.r")),
                        new XElement(xNs + "None", new XAttribute("Include", $"{VCPrjName}cmd.r")),
                        new XElement(xNs + "None", new XAttribute("Include", $"{VCPrjName}.mke"))
                    ));
        }

        void AddIncludeAndDebugPath()
        {
            root.Add(
                    new XElement(xNs + "PropertyGroup",
                        new XAttribute("Condition", @"'$(Configuration)|$(Platform)'=='Debug|x64'"),
                        new XElement(xNs + "IncludePath", new XText("$(MSSDKINC);$(IncludePath)")),
                        new XElement(xNs + "LocalDebuggerCommand", new XText("$(MSAPP)\\$(ProjectName).dll")),
                        new XElement(xNs + "DebuggerFlavor", new XText("WindowsLocalDebugger"))
                    ));
        }

        void AddRFileToRscFilter()
        {
            XElement filterroot = XElement.Load(VCPrjDir+VCPrjName+ ".vcxproj.filters");
            var res = from filter in filterroot.Descendants(filterroot.GetDefaultNamespace() + "Filter")
                where filter.Attribute("Include")?.Value == "资源文件"
                select filter.Descendants(filterroot.GetDefaultNamespace() + "Extensions").First();
            XElement extensionElement = res.First();
            if(!extensionElement.Value.Split(';').Contains("r"))
                extensionElement.SetValue(extensionElement.Value+";r");
            filterroot.Save(VCPrjDir + VCPrjName + ".vcxproj.filters");
        }

        public void Modify()
        {
            AddRFileToRscFilter();
            AddFiles();
            AddIncludeAndDebugPath();
            root.Save(VCPrjDir + VCPrjName + ".vcxproj");
            //xdoc.Save( VCPrjDir + VCPrjName +".vcxproj");
        }
    }
}
