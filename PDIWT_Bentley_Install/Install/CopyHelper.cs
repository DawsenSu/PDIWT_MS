using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Install
{
    class CopyHelper
    {
        /// <summary>
        /// 拷贝文件夹下的所有文件及文件夹，覆盖
        /// </summary>
        /// <param name="srcdir">原文件夹</param>
        /// <param name="desdir">目标文件夹</param>
        public static void CopyDirectory(string srcdir, string desdir)
        {
            string folderName = srcdir.Substring(srcdir.LastIndexOf("\\") + 1);
            string desfolderdir = desdir + "\\" + folderName;
            if (desdir.LastIndexOf("\\") == (desdir.Length-1))
            {
                desfolderdir = desdir + folderName;
            }
            string[] filenames = Directory.GetFileSystemEntries(srcdir);
            foreach (var file in filenames)
            {
                if (Directory.Exists(file))
                {
                    string currentdir = desfolderdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                    if (!Directory.Exists(currentdir))
                        Directory.CreateDirectory(currentdir);
                    CopyDirectory(file, desfolderdir);
                }
                else
                {
                    string srcfilename = file.Substring(file.LastIndexOf("\\") + 1);
                    srcfilename = desfolderdir + "\\" + srcfilename;
                    if (!Directory.Exists(desfolderdir))
                        Directory.CreateDirectory(desfolderdir);
                    File.Copy(file, srcfilename, true);
                }
            }
        }
    }
}
