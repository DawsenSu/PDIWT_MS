using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using CommandLine;
using CommandLine.Text;

namespace MDLTemplateGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (Parser.Default.ParseArguments(args,options))
            {
                if (!File.Exists(options.VCProjectPath))
                {
                    Console.WriteLine($"无法打开VC项目文件：{options.VCProjectPath}");
                    return;
                }
                if (options.IsOnlyMke)
                {
                    MakeFileGenerator fileGenerator = new MakeFileGenerator(options.VCProjectPath);
                    fileGenerator.GenerateMakeFile();
                    Console.WriteLine("mke文件生成");
                }
                else
                {
                    ResourceFileGenerator rscFileGenerator = new ResourceFileGenerator(options.VCProjectPath);
                    rscFileGenerator.GenerateRscources();
                    Console.WriteLine("资源文件生成");
                    ClassFileGenerator classFileGenerator = new ClassFileGenerator(options.VCProjectPath);
                    classFileGenerator.GenerateClasses();
                    Console.WriteLine("程序文件生成");
                    ModifyVCProjFile modifyVc = new ModifyVCProjFile(options.VCProjectPath);
                    modifyVc.Modify();
                    Console.WriteLine("VC工程文件修改完成");
                    MakeFileGenerator fileGenerator = new MakeFileGenerator(options.VCProjectPath);
                    fileGenerator.GenerateMakeFile();
                    Console.WriteLine("mke文件生成");
                }
            }

            //string vcprpjpath =
            //    @"D:\Programma\Source\C#\MDL\PDIWT_MS\PDIWT_MS\MDLTemplateGenerator\MDLTemplateGenerator\MDLTest\MDLTest\MDLTest.vcxproj";
            //ResourceFileGenerator rscFileGenerator = new ResourceFileGenerator(vcprpjpath);
            //rscFileGenerator.GenerateRscources();

            //ClassFileGenerator classFileGenerator = new ClassFileGenerator(vcprpjpath);
            //classFileGenerator.GenerateClasses();

            //ModifyVCProjFile modifyVc = new ModifyVCProjFile(vcprpjpath);
            //modifyVc.Modify();

            //MakeFileGenerator fileGenerator = new MakeFileGenerator(vcprpjpath);
            //fileGenerator.GenerateMakeFile();

            //Console.WriteLine("创建完成！");
            //Console.ReadKey();
        }
    }

    class Options
    {
        [Option('m',"only-mke", HelpText = "只根据VCProject生成MakeFile")]
        public bool IsOnlyMke { get; set; }
        [Option('f',"vcprojpath",HelpText = "VC工程文件的位置",Required = true)]
        public string VCProjectPath { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
