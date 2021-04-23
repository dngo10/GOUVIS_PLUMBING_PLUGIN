using ClassLibrary1.HELPERS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data.SQLite;

namespace ClassLibrary1.DATABASE.Controllers
{
    //This class is used exclusively for managing SQLITE Database.

    class PlumbingDatabaseManager
    {

        public static ProjectElement projectElement;

        public static void clear()
        {
            if (projectElement == null) return;
            projectElement.P_NOTE = null;
            projectElement.Dwgs = null;
        }

        public static SQLiteConnection OpenSqliteConnection(string dbPath)
        {
            return new SQLiteConnection(DBCommand.GetConnectionString(dbPath));
        }

        public static void InitTable(string databasePath)
        {
            string createFileTable = DBCommand.DBFileTable.CreateFileTable();
            SQLiteConnection sqliteConn = new SQLiteConnection(DBCommand.GetConnectionString(databasePath));
            try
            {
                sqliteConn.Open();
                SQLiteTransaction sqliteTrans = sqliteConn.BeginTransaction();
                SQLiteCommand command = sqliteConn.CreateCommand();
                command.CommandText = createFileTable;
                command.ExecuteNonQuery();

                command.CommandText = DBCommand.DBFileTable.InsertIntoFileTable(projectElement.P_NOTE.relativePath, projectElement.P_NOTE.lastModified.Ticks, 1);
                command.ExecuteNonQuery();

                foreach (FileElement pe in projectElement.Dwgs)
                {
                    command.CommandText = DBCommand.DBFileTable.InsertIntoFileTable(pe.relativePath, pe.lastModified.Ticks, 0);
                    command.ExecuteNonQuery();
                }

                sqliteTrans.Commit();
                command.Dispose();
                sqliteTrans.Dispose();
                sqliteConn.Close();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception e)
            {
                DebugMessage(string.Format(@"Error opening Database: {0}, Error:{1}", databasePath, e.Message));
            }

        }

        public static void CreateDatabase(string folder)
        {
            string dbName = ConstantName.databasePostFix;
            string dbFilePath = folder + "\\" + dbName;

            if (File.Exists(dbFilePath))
            {
                string msg = string.Format("Database {0} already exists.", dbFilePath);
                DebugMessage(msg);
            }
            else
            {
                SQLiteConnection.CreateFile(dbFilePath);
                InitTable(dbFilePath);
            }
        }

        public static bool DoesDatabaseExist(string DataFolder)
        {
            if (!Directory.Exists(DataFolder))
            {
                string msg = string.Format("Data Folder {0} does not exist", Path.GetFileNameWithoutExtension(DataFolder));
                DebugMessage(msg);
                return false;
            }


            string[] files = Directory.GetFiles(DataFolder);

            IEnumerable<string> databaseQuery =
                from file in files
                where file.Contains(string.Format("{0}.db", ConstantName.databasePostFix))
                select file;

            if (databaseQuery.Count() == 1)
            {
                return true;
            }
            else if (databaseQuery.Count() > 1)
            {
                string msg = string.Format("There is more than 1 database in the folder {0}", Path.GetFileNameWithoutExtension(DataFolder));
                DebugMessage(msg);
                return false;
            }
            else if (databaseQuery.Count() == 0)
            {
                string msg = string.Format("There is no database in the folder {0}", Path.GetFileNameWithoutExtension(DataFolder));
                DebugMessage(msg);
                return false;
            }
            return true;
        }

        //READ DATABASE FROM 
        public static void ReadDataAtBeginning(string dbPath)
        {
            SQLiteConnection sqlConn = OpenSqliteConnection(dbPath);
            projectElement = ReadFileTableDataBase(sqlConn);
        }

        public static ProjectElement ReadFileTableDataBase(SQLiteConnection sqlConn)
        {
            sqlConn.Open();
            //Do Directory.GetParent 2 times, and we get the projectPath;

            SQLiteTransaction tr = sqlConn.BeginTransaction();
            FileElement notePathFe = GetNotePath(sqlConn);
            HashSet<FileElement> feDwgs = GetDwgsPath(sqlConn);
            projectElement = new ProjectElement(notePathFe, feDwgs);

            tr.Commit();
            tr.Dispose();
            sqlConn.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            return projectElement;
        }


        //Return all Dwgs relatives path, EXCEPT Note_Path file
        public static HashSet<FileElement> GetDwgsPath(SQLiteConnection sqlConn)
        {
            HashSet<FileElement> dwgsSet = new HashSet<FileElement>();
            SQLiteCommand command = sqlConn.CreateCommand();
            command.CommandText = DBCommand.DBFileTable.GetDwgsStringFromTable();
            SQLiteDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                string relativePath = dataReader.GetString(0);
                DateTime dt = new DateTime(dataReader.GetInt64(1));

                FileElement feDwg = new FileElement(relativePath, dt);
                dwgsSet.Add(feDwg);
            }
            command.Dispose();
            return dwgsSet;
        }

        //SQL connection must be OPEN.
        //This function should be put between transaction.
        public static FileElement GetNotePath(SQLiteConnection sqlConn)
        {
            string notePath = "";
            DateTime dt = DateTime.MinValue;
            SQLiteCommand command = sqlConn.CreateCommand();
            command.CommandText = DBCommand.DBFileTable.GetPNoteStringFromTable();
            SQLiteDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                notePath = dataReader.GetString(0);
                dt = new DateTime(dataReader.GetInt64(1));
            }
            FileElement fe = new FileElement(notePath, dt);
            command.Dispose();
            return fe;
        }

        [ConditionalAttribute("DEBUG")]
        public static void DebugMessage(string message)
        {
            Console.WriteLine(message);
        }
    }


    //UPDRAGE THIS to check DateTime UTC
    class ProjectElement
    {
        public FileElement P_NOTE;
        public HashSet<FileElement> Dwgs;

        public ProjectElement() { }

        public ProjectElement(FileElement P_NOTE, HashSet<FileElement> dwgs)
        {
            this.P_NOTE = P_NOTE;
            this.Dwgs = dwgs;
        }

        public bool ContainsPath(string relativePath)
        {
            foreach (FileElement fe in Dwgs)
            {
                if (relativePath.Contains(fe.relativePath))
                {
                    return true;
                }
            }
            return false;
        }
    }

    class FileElement : IEqualityComparer<FileElement>, IEquatable<FileElement>
    {
        public int ID;
        public string relativePath;
        public DateTime lastModified;

        public FileElement(string relPath, DateTime dt)
        {
            relativePath = relPath;
            lastModified = dt;
        }

        public FileElement() { }


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
            if (isXempty && isYempty) { return true; }
            if ((isXempty && !isYempty) || (!isXempty && isYempty)) { return false; }

            return x.relativePath == y.relativePath;
        }

        public bool Equals(FileElement other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }
            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }
            if (this.GetType() != other.GetType())
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
            if (ReferenceEquals(obj, this))
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

