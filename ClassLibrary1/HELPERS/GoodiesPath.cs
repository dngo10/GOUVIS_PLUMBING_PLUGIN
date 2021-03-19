using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.HELPERS
{
    class GoodiesPath
    {
        public static string GetAccoreConsolePath()
        {
            string result = "";
            string autoDesk = "\\AutoDesk";
            string programFilePath = ConstantName.programFilePath;
            if(Directory.Exists(programFilePath + autoDesk))
            {
                int year = DateTime.Now.Year + 1;
                for(int i = year; i > 2013; i--)
                {
                    string AutoCADPath = programFilePath + autoDesk + "\\AutoCAD " + i.ToString();
                    if (Directory.Exists(AutoCADPath))
                    {
                        string AccorePath = AutoCADPath + "\\accoreconsole.exe";
                        if (File.Exists(AccorePath))
                        {
                            result = AccorePath;
                        }
                    }
                }
            }
            return result;
        }
    }
}
