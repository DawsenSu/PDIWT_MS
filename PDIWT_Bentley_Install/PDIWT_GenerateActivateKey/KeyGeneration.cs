using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using CommandLine;
using CommandLine.Text;

namespace PDIWT_Encrypt
{
    class KeyGeneration
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {
                if (File.Exists(options.InputFilePath))
                {
                    PDIWTEncrypt pdiwt = new PDIWTEncrypt();
                    StringBuilder outputsb = new StringBuilder();
                    using (StreamReader sr = new StreamReader(options.InputFilePath))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            outputsb.AppendLine(line + " ==> " + pdiwt.GenerateActivationCodeString(line));
                        }
                    }
                    File.WriteAllText(options.OutputFilePath, outputsb.ToString());
                    Console.WriteLine($"输入文件{Path.GetFileName(options.InputFilePath)}处理完成！");
                }
                else
                {
                    Console.WriteLine($"=>不存在{options.InputFilePath}");
                }

            }
        }
    }

    class Options
    {
        [Option('i', "InputFile", DefaultValue = "SerialNumbers.txt", HelpText = "包含序列号的输入文件，每行仅包含一个字符串")]
        public string InputFilePath { get; set; }
        [Option('o', "OutputFilePath", DefaultValue = "ActivationKeys.txt", HelpText = "生成的激活秘钥文件")]
        public string OutputFilePath { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("PDIWTEncrypt", "1.0"),
                Copyright = new CopyrightInfo("苏东升", 2017),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddPreOptionsLine("Usage: PDIWTEncrypt -i xxx.txt -o XXX.txt");
            help.AddOptions(this);
            return help;
        }
    }
}
