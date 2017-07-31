using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MDLTemplateGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1 || !File.Exists(args.First()))
            {
                Console.WriteLine("项目路径错误");
                return;
            }
            string vcprpjpath = args.First();

            //string vcprpjpath =
            //    @"D:\Programma\Source\C#\MDL\PDIWT_MS\PDIWT_MS\MDLTemplateGenerator\MDLTemplateGenerator\MDLTest\MDLTest\MDLTest.vcxproj";
            ResourceFileGenerator rscFileGenerator = new ResourceFileGenerator(vcprpjpath);
            rscFileGenerator.GenerateRscources();

            ClassFileGenerator classFileGenerator = new ClassFileGenerator(vcprpjpath);
            classFileGenerator.GenerateClasses();

            ModifyVCProjFile modifyVc = new ModifyVCProjFile(vcprpjpath);
            modifyVc.Modify();

            MakeFileGenerator fileGenerator = new MakeFileGenerator(vcprpjpath);
            fileGenerator.GenerateMakeFile();

            Console.WriteLine("创建完成！");
            //Console.ReadKey();
        }
    }
}
