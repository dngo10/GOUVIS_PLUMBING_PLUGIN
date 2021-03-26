using ClassLibrary1.HELPERS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data.SQLite;

namespace ProjectManager
{
    //This class is used exclusively for managing SQLITE Database.

    class DatabaseManager
    {

        public static ProjectElement projectElement;

        public static void clear()
        {
            if (projectElement == null) return;
            projectElement.P_NOTE = null;
            projectElement.Dwgs = null;
        }

        public static string GuessProjectNumber()
        {
            string fullPNotePath = Model.ProjectFolder + projectElement.P_NOTE.relativePath;
            Regex rex = new Regex(ConstantName.projectNumberPattern, RegexOptions.IgnoreCase);

            Match match = rex.Match(fullPNotePath);
            string number = "No_Num";

            if (match.Success)
            {
                number = match.Groups[1].Value;
            }

            return number;
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
                CreateDatabase(dbFolder);

            }
            else
            {
                if (!DoesDatabaseExist(dbFolder))
                {
                    CreateDatabase(dbFolder);
                }
            }
        }

        public static void CreateDatabaseFolder(out string dbFolder)
        {
            dbFolder = "";
            if (string.IsNullOrEmpty(Model.ProjectFolder)) return;

            if (GoodiesPath.IsDirectoryWritable(Model.ProjectFolder))
            {
                string dataBaseFolder = "\\_" + Model.ProjectNumber + ConstantName.centerFolder;
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


                foreach(string dir in directories)
                {
                    Match m = rex.Match(dir);
                    if (m.Success)
                    {
                        try
                        {
                            string[] files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
                            foreach(string file in files)
                            {
                                File.Delete(file);
                            }

                            Directory.Delete(dir, true);
                            result = true;
                        }
                        catch (Exception e)
                        {
                            string msg = string.Format("Can't delete database folder: {0}, Exception: {1}", dir, e.Message);
                            debugMessage(msg);
                            result = false;
                        }
                    }
                }
            }
            return result;
        }

        public static void InitTable(string databasePath)
        {
            string createFileTable = DBCommand.CreateFileTable();
            SQLiteConnection sqliteConn = new SQLiteConnection(DBCommand.GetConnectionString(databasePath));
            try
            {
                sqliteConn.Open();
                SQLiteTransaction sqliteTrans = sqliteConn.BeginTransaction();
                SQLiteCommand command = sqliteConn.CreateCommand();
                command.CommandText = createFileTable;
                command.ExecuteNonQuery();

                command.CommandText = DBCommand.InsertToFileTable(projectElement.P_NOTE.relativePath, projectElement.P_NOTE.lastModified.Ticks.ToString());
                command.ExecuteNonQuery();

                foreach (FileElement pe in projectElement.Dwgs)
                {
                    command.CommandText = DBCommand.InsertToFileTable(pe.relativePath, pe.lastModified.Ticks.ToString());
                    command.ExecuteNonQuery();
                }

                sqliteTrans.Commit();
                command.Dispose();
                sqliteTrans.Dispose();
                sqliteConn.Close();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch(Exception e)
            {
                debugMessage(string.Format(@"Error opening Database: {0}, Error:{1}", databasePath, e.Message));
            }

        }

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

        public static void CreateDatabase(string folder)
        {
            string dbName = GetDatabaseName(Model.ProjectNumber);
            string dbFilePath = folder + "\\" + dbName;

            if (File.Exists(dbFilePath))
            {
                string msg = string.Format("Database {0} already exists.", dbFilePath);
                debugMessage(msg);
            }
            else
            {
                SQLiteConnection.CreateFile(dbFilePath);
                InitTable(dbFilePath);
            }
        }

        public static string GetDatabaseName(string number)
        {
            return number + ConstantName.databasePostFix + ".db";
        }

        public static string GetDatabaseFolderName(string number)
        {
            return number + ConstantName.centerFolder;
        }

        public static bool DoesDatabaseExist(string DataFolder)
        {
            if (!Directory.Exists(DataFolder))
            {
                string msg = string.Format("Data Folder {0} does not exist", Path.GetFileNameWithoutExtension(DataFolder));
                debugMessage(msg);
                return false;
            }


            string[] files = Directory.GetFiles(DataFolder);

            IEnumerable<string> databaseQuery =
                from file in files
                where file.Contains(string.Format("{0}.db", ConstantName.databasePostFix))
                select file;

            if(databaseQuery.Count() == 1)
            {
                return true;
            }else if(databaseQuery.Count() > 1)
            {
                string msg = string.Format("There is more than 1 database in the folder {0}", Path.GetFileNameWithoutExtension(DataFolder));
                debugMessage(msg);
                return false;
            }else if(databaseQuery.Count() == 0)
            {
                string msg = string.Format("There is no database in the folder {0}", Path.GetFileNameWithoutExtension(DataFolder));
                debugMessage(msg);
                return false;
            }
            return true;
        }

        [ConditionalAttribute("DEBUG")]
        public static void debugMessage(string message)
        {
            Console.WriteLine(message);
        }
    }


    //UPDRAGE THIS to check DateTime UTC
    class ProjectElement
    {
        public FileElement P_NOTE;
        public HashSet<FileElement> Dwgs;

        public ProjectElement(){}

        public ProjectElement(FileElement P_NOTE, HashSet<FileElement> dwgs)
        {
            this.P_NOTE = P_NOTE;
            this.Dwgs = dwgs;
        }
    }

    class FileElement : IEqualityComparer<FileElement>, IEquatable<FileElement>
    {
        public int ID;
        public string relativePath;
        public DateTime lastModified;


        //CODE BELOW IS USED TO COMPARE FOR HASHSET.

        public bool Equals(FileElement x, FileElement y)
        {
            bool xIsNull = ReferenceEquals(x, null);
            bool yIsNull = ReferenceEquals(y, null);
            if (xIsNull && yIsNull) return true;
            if (xIsNull && !yIsNull) return false;
            if (!xIsNull && yIsNull) return true;

            bool isXempty = string.IsNullOrEmpty(x.relativePath);
            bool isYempty = string.IsNullOrEmpty(y.relativePath);
            if (isXempty && isYempty){ return true; }
            if ((isXempty && !isYempty) || (!isXempty && isYempty)){ return false; }

            return x.relativePath == y.relativePath;
        }

        public bool Equals(FileElement other)
        {
            if(Object.ReferenceEquals(other, null))
            {
                return false;
            }
            if(Object.ReferenceEquals(this, other))
            {
                return true;
            }
            if(this.GetType() != other.GetType())
            {
                return false;
            }

            bool isXempty = string.IsNullOrEmpty(relativePath);
            bool isYempty = string.IsNullOrEmpty(other.relativePath);
            if (isXempty && isYempty) { return true; }
            if ((isXempty && !isYempty) || (!isXempty && isYempty)) { return false; }

            return relativePath == other.relativePath;

        }

        public override bool Equals(object obj)
        {
            if(ReferenceEquals(obj, this))
            {
                FileElement fe = obj as FileElement;
                return Equals(fe);
            }
            return false;
        }

        public int GetHashCode(FileElement obj)
        {
            return obj.relativePath.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.relativePath.GetHashCode();
        }

        public static bool operator ==(FileElement x, FileElement y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(FileElement x, FileElement y)
        {
            return !x.Equals(y);
        }
    }
}
