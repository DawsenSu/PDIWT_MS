using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GetTotalInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("请输入开挖方量计算文件路径：");
            string filepath = Console.ReadLine();
            while (!System.IO.File.Exists(filepath))
            {
                Console.WriteLine("输入文件不存在，请重新输入：");
                filepath = Console.ReadLine();
            }
            string content = System.IO.File.ReadAllText(filepath);
            //string searchPattern = @"(?<=Area\s*=\s*)(\d*\.\d*)";
            //Match m = Regex.Match(content, searchPattern);
            //double result = 0;
            //while (m.Success)
            //{
            //    result += double.Parse(m.Value);
            //    m = m.NextMatch();
            //}
            //Console.WriteLine($"Total Area = {result} Sq Meter");
            Console.WriteLine("");
            Console.WriteLine("============Sum Table=============");
            SumOutput("Cut", content);
            SumOutput("Fill", content);
            SumOutput("Area", content);
            SumOutput("Balance", content);
            Console.WriteLine("");
            Console.WriteLine("按任意键退出");
            Console.ReadKey();
        }
        static void SumOutput(string sumname,string content)
        {
            string searchPattern = @"(?<="+sumname+@"\s*=\s*)(\d*\.\d*)";
            Match m = Regex.Match(content, searchPattern);
            double result = 0;
            while (m.Success)
            {
                result += double.Parse(m.Value);
                m = m.NextMatch();
            }
            Console.WriteLine($"Total {sumname} = {result}");
        }
    }
}
