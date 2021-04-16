using ClassLibrary1.HELPERS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data.SQLite;
using ClassLibrary1.DATABASE;

namespace ProjectManager
{
    //This class is used exclusively for managing SQLITE Database.

    class DatabaseManager
    {

        public static ProjectElement projectElement;


        public static bool HasDataBaseFolder(out string dBDirectory)
        {
            dBDirectory = "";
            if (string.IsNullOrEmpty(Model.ProjectFolder)) return false;

            string[] directories = Directory.GetDirectories(Model.ProjectFolder);

            foreach (string directory in directories)
            {
                Regex rex = new Regex(ConstantName.databaseFolderPattern, RegexOptions.IgnoreCase);
                if (rex.Match(directory).Success)
                {
                    dBDirectory = directory;
                    return true;
                }
            }
            return false;
        }

        public static void UpdateInitDatabase()
        {
            if (projectElement == null) return;
            if (projectElement.P_NOTE == null) return;
            if (projectElement.Dwgs == null) return;
            if (Model.ProjectFolder == null) return;

            //Trying to Get ProjectNumber

            string dbFolder = "";
            if (!HasDataBaseFolder(out dbFolder))
            {
                CreateDatabaseFolder(out dbFolder);
                PlumbingDatabaseManager.CreateDatabase(dbFolder);
            }
            else
            {
                if (!PlumbingDatabaseManager.DoesDatabaseExist(dbFolder))
                {
                    PlumbingDatabaseManager.CreateDatabase(dbFolder);
                }
            }
            string batFilePath = Directory.GetParent(dbFolder).FullName + "\\" + ConstantName.batFileName;
            File.WriteAllText(batFilePath, ConstantName.batchCommand);
        }

        public static void CreateDatabaseFolder(out string dbFolder)
        {
            dbFolder = "";
            if (string.IsNullOrEmpty(Model.ProjectFolder)) return;

            if (GoodiesPath.IsDirectoryWritable(Model.ProjectFolder))
            {
                string dataBaseFolder = "\\" + ConstantName.centerFolder;
                dbFolder = Model.ProjectFolder + dataBaseFolder;
                Directory.CreateDirectory(dbFolder);
            }
        }

        public static bool DeleteDatabaseFolder()
        {
            bool result = false;
            if (Directory.Exists(Model.ProjectFolder))
            {
                string[] directories = Directory.GetDirectories(Model.ProjectFolder);

                Regex rex = new Regex(ConstantName.databaseFolderPattern, RegexOptions.IgnoreCase);


                foreach (string dir in directories)
                {
                    Match m = rex.Match(dir);
                    if (m.Success)
                    {
                        try
                        {
                            string[] files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
                            foreach (string file in files)
                            {
                                File.Delete(file);
                            }

                            Directory.Delete(dir, true);
                            result = true;
                        }
                        catch (Exception e)
                        {
                            string msg = string.Format("Can't delete database folder: {0}, Exception: {1}", dir, e.Message);
                            PlumbingDatabaseManager.DebugMessage(msg);
                            result = false;
                        }
                    }
                }
            }
            return result;
        }

    }
}
