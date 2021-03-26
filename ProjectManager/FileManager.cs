using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary1.HELPERS;

namespace ProjectManager
{
    class FileManager
    {
        //Because we don't know where the Project is, we can't assume it's always in X drive.
        public static string PlumbingFolder;

        //Get Directory, Return Null/Empty mean there is no PLBG
        //This will also check whether or not projectNumber receieved is a NUMBER or or PATH.
        //There is no need to put a warning after this.
        //Return path/to/plumbingfolder
        //public static string GetIntoDirectory(string projectNumber)
        //{
        //    projectNumber = projectNumber.Trim();
        //
        //    if (Directory.Exists(projectNumber))
        //    {
        //        if (IsThisAValidDirectory(projectNumber))
        //        {
        //            return ProjectPath + "\\" + PlumbingFolder;
        //        }
        //        else
        //        {
        //            string msg = string.Format("Project Path ({0}) Does Not Have Plumbing Folder", projectNumber);
        //            MessageBox.Show(msg, "Plumbing Folder Not Found", MessageBoxButtons.OK);
        //            return "";
        //        }
        //    }
        //    
        //    //Check in X drive
        //
        //    string projectDirectory = ConstantName.XDriveProject + string.Format(@"\{0}", projectNumber);
        //    string huanProjectDirectory = ConstantName.XDriveHuanProject + string.Format(@"\{0}", projectNumber);
        //    string XprojectDirectory = GoodiesPath.ConvertPathToXDrive(projectDirectory);
        //    string XhuanProjectDirectory = GoodiesPath.ConvertPathToXDrive(huanProjectDirectory);
        //
        //    string officialPath = "";
        //
        //    if (Directory.Exists(projectDirectory))
        //    {
        //        officialPath = projectDirectory;
        //    }else if (Directory.Exists(huanProjectDirectory))
        //    {
        //        officialPath = huanProjectDirectory;
        //    }else if (Directory.Exists(XprojectDirectory))
        //    {
        //        officialPath = XprojectDirectory;
        //    }else if (Directory.Exists(XhuanProjectDirectory))
        //    {
        //        officialPath = XhuanProjectDirectory;
        //    }
        //
        //    if (!Directory.Exists(officialPath))
        //    {
        //        string msg = string.Format("Project folder ({0}) Not found", projectNumber);
        //        MessageBox.Show(msg, "Project Folder Not Found", MessageBoxButtons.OK);
        //        return "";
        //    }
        //
        //    if (IsThisAValidDirectory(officialPath))
        //    {
        //        return ProjectPath + "\\" + PlumbingFolder;
        //    }
        //    else
        //    {
        //        string msg = string.Format("There is no PLBG in project {0}", projectDirectory);
        //        MessageBox.Show(msg, "PLBG Folder Not Found", MessageBoxButtons.OK);
        //        return "";
        //    }
        //}

        //This function WILL MODIFIED static value of project.
        //Check the PLBG NAME
        //public static bool IsThisAValidDirectory(string path)
        //{
        //    if (Directory.Exists(path))
        //    {
        //        if(Path.GetFileNameWithoutExtension(path).ToUpper() == ConstantName.PlumbingFolderName)
        //        {
        //            PlumbingFolder = Path.GetFileNameWithoutExtension(path);
        //        }
        //        else
        //        {
        //            if (DoesItHavePlumbingFolder(path))
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        //This function WILL MODIFIED static value of project.
        //private static bool DoesItHavePlumbingFolder(string path)
        //{
        //    string[] directories = Directory.GetDirectories(path);
        //    foreach(string dir in directories)
        //    {
        //        string folderName = Path.GetFileNameWithoutExtension(dir);
        //        if (folderName.ToUpper().Trim() == ConstantName.PlumbingFolderName)
        //        {
        //            PlumbingFolder = folderName;
        //            ProjectPath = path;
        //            ProjectNumber = Path.GetFileNameWithoutExtension(path);
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //Ignore etransmit, offline, backup, and program folder
        public static string[] GetAllDwgFileInDirectory(string path)
        {
            var myFiles = Directory
                .GetFiles(path, "*.dwg");

            List<string> paths = new List<string>();
            foreach(string p in myFiles)
            {
                if(!(p.ToLower().Contains("_backup") ||
                   p.ToLower().Contains("etransmit") ||
                   p.ToLower().Contains("offline")) ||
                   p.ToLower().Contains(ConstantName.centerFolder)
                  )
                {
                    paths.Add(p);
                }
            }
            return paths.ToArray();
        }

        public static string GetFullPath(string relativePath)
        {
            return PlumbingFolder + relativePath;
        }
    }
}
