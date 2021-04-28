﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data.SQLite;
using GouvisPlumbingNew.DATABASE.Controllers;
using GouvisPlumbingNew.DATABASE.DBModels;

namespace GouvisPlumbingNew.HELPERS
{
    class GoodiesPath
    {
        public static bool IsFileLocked(string path)
        {
            try
            {
                using (File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    return false;
                }
            }
            catch (IOException e)
            {
                int errorNum = Marshal.GetHRForException(e) & ((1 << 16) - 1);
                return errorNum == 32 || errorNum == 33;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "IsFileLocked Checking");
                return true;
            }
        }

        public static bool IsFileReadOnly(string path)
        {
            return new FileInfo(path).IsReadOnly;
        }
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

        public static string GetAcadPath()
        {
            string result = "";
            string autoDesk = "\\AutoDesk";
            string programFilePath = ConstantName.programFilePath;
            if (Directory.Exists(programFilePath + autoDesk))
            {
                int year = DateTime.Now.Year + 1;
                for (int i = year; i > 2013; i--)
                {
                    string AutoCADPath = programFilePath + autoDesk + "\\AutoCAD " + i.ToString();
                    if (Directory.Exists(AutoCADPath))
                    {
                        string AccorePath = AutoCADPath + "\\acad.exe";
                        if (File.Exists(AccorePath))
                        {
                            result = AccorePath;
                        }
                    }
                }
            }
            return result;
        }

        //This is because autocad can't figure out network drive... the \\Ge-fs1\mep\HUAN'S MEP PROJECTS\ 
        //doesn't mean anything to this software.
        public static string ConvertPathToXDrive(string path)
        {
            if (path.Contains(ConstantName.XDrive))
            {
                path = path.Replace(ConstantName.XDrive, "X:");
                return path;
            }
            return path;
        }


        /// <summary>
        /// Creates a relative path from one file or folder to another.
        /// </summary>
        /// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
        /// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
        /// <returns>The relative path from the start directory to the end path or <c>toPath</c> if the paths are not related.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static string MakeRelativePath(string fromPath, string toPath)
        {
            if (string.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (string.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }
            return relativePath;
        }

        //RETURN RELATIVE PATH BASE ON BASE FOLDER
        public static string MakeRelativePath(string toPath)
        {
            string fromPath = GetBaseFolderPathFromDwgPath(toPath);
            if (string.IsNullOrEmpty(fromPath)) return "";

            if (string.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (string.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }
            return relativePath;
        }

        //Code from: https://stackoverflow.com/questions/1410127/c-sharp-test-if-user-has-write-access-to-a-folder
        public static bool IsDirectoryWritable(string dirPath, bool throwIfFails = false)
        {
            try
            {
                using (FileStream fs = File.Create(
                    Path.Combine(
                        dirPath,
                        Path.GetRandomFileName()
                    ),
                    1,
                    FileOptions.DeleteOnClose)
                )
                { }
                return true;
            }
            catch
            {
                if (throwIfFails)
                    throw;
                else
                    return false;
            }
        }

        /// <summary>
        /// Check if dwg path 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsDwgPath(string path)
        {
            Regex rex = new Regex(ConstantName.DwgsPathFile, RegexOptions.IgnoreCase);
            if (rex.IsMatch(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if dwg is like a Note path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsNotePath(string path)
        {
            Regex rex = new Regex(ConstantName.DwgsPNoteFile, RegexOptions.IgnoreCase);
            if (rex.IsMatch(path))
            {
                return true;
            }
            else
            {
                Console.WriteLine("GoodiesPath->Helper: ERROR: Not a NotePaths .dwg file");
                return false;
            }
        }

        //Given a dwg file path, get it's Database path if exists, if not return ""
        public static string GetDatabasePathFromDwgPath(string path)
        {
            string dataPath = "";
            string directoryPath = Path.GetDirectoryName(path);
            while (!string.IsNullOrEmpty(directoryPath))
            {
                dataPath = directoryPath + "\\" + ConstantName.centerFolder + "\\" + ConstantName.databasePostFix;
                if (File.Exists(dataPath))
                {
                    return dataPath;
                }
                directoryPath = Directory.GetParent(directoryPath).FullName;
            }
            return dataPath;
        }

        public static string GetBaseFolderPathFromDwgPath(string path)
        {
            string databasePath = GetDatabasePathFromDwgPath(path);
            if (!string.IsNullOrEmpty(databasePath))
            {
                return Directory.GetParent(Directory.GetParent(databasePath).FullName).FullName;
            }
            else
            {
                return "";
            }
        }
        public static string GetNotePathFromADwgPath(string path)
        {
            string dataPath = GetDatabasePathFromDwgPath(path);
            if (!string.IsNullOrEmpty(dataPath))
            {
                string directoryPath = Directory.GetParent(Path.GetDirectoryName(path)).FullName;
                SQLiteConnection sqlConn = PlumbingDatabaseManager.OpenSqliteConnection(dataPath);
                sqlConn.Open();
                SQLiteTransaction tr = sqlConn.BeginTransaction();
                DwgFileModel fe = PlumbingDatabaseManager.GetNotePath(sqlConn);
                tr.Dispose();
                sqlConn.Close();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                string pNotePath = directoryPath + fe.relativePath;
                if (File.Exists(pNotePath))
                {
                    return pNotePath;
                }
            }
            return "";
        }

        public static bool HasDwgPathInDatabase(string path)
        {
            string dataPath = GetDatabasePathFromDwgPath(path);
            if (!string.IsNullOrEmpty(dataPath))
            {
                string directoryPath = Directory.GetParent(Path.GetDirectoryName(path)).FullName;
                SQLiteConnection sqlConn = PlumbingDatabaseManager.OpenSqliteConnection(dataPath);
                sqlConn.Open();
                SQLiteTransaction tr = sqlConn.BeginTransaction();
                HashSet<DwgFileModel> feSet = PlumbingDatabaseManager.GetDwgsPath(sqlConn);
                tr.Dispose();
                sqlConn.Close();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                foreach(DwgFileModel fe in feSet)
                {
                    string dwgPath = directoryPath + fe.relativePath;
                    if(dwgPath == path)
                    {
                        return true;
                    }
                }
                Console.WriteLine(string.Format("HasDwgPathInDatabase Func -> Database found, but file from file: {0}", path));
            }
            else
            {
                Console.WriteLine(string.Format("HasDwgPathInDatabase Func -> Could not find database from file: {0}", path));
            }
            return false;
        }
    }
}
