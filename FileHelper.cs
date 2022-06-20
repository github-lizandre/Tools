using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class FileHelper
    {
        public static string GetNewestFileInDirectory(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            FileInfo myFile = (from f in directory.GetFiles()
                               orderby f.LastWriteTime descending
                               select f).First();

            return myFile.FullName;
        }
    }
}
