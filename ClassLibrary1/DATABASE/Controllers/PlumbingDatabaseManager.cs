using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data.SQLite;
using GouvisPlumbingNew.DATABASE.DBModels;
using ClassLibrary1.DATABASE.Controllers;

namespace GouvisPlumbingNew.DATABASE.Controllers
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
            SQLiteConnection sqliteConn = new SQLiteConnection(DBCommand.GetConnectionString(databasePath));
            try
            {
                sqliteConn.Open();
                SQLiteTransaction sqliteTrans = sqliteConn.BeginTransaction();

                DBMatrix3d.CreateTable(sqliteConn);
                DBPoint3D.CreateTable(sqliteConn);
                DBInsertPoint.CreateTable(sqliteConn);
                DBFixtureDetails.CreateTable(sqliteConn);
                DBFixtureBeingUsedArea.CreateTable(sqliteConn);
                DBDwgFile.CreateTable(sqliteConn);
                DBTable.CreateTable(sqliteConn);

                if (projectElement.P_NOTE != null) projectElement.P_NOTE.WriteToDatabase(sqliteConn);

                if(projectElement.Dwgs != null)
                foreach(DwgFileModel model in projectElement.Dwgs)
                {
                        model.WriteToDatabase(sqliteConn);
                }

                sqliteTrans.Commit();
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
            sqlConn.Open();
            using(SQLiteTransaction sqlTr = sqlConn.BeginTransaction())
            {
                projectElement = ReadFileTableDataBase(sqlConn);
                sqlConn.Close();
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            
        }

        public static ProjectElement ReadFileTableDataBase(SQLiteConnection sqlConn)
        {
            //Do Directory.GetParent 2 times, and we get the projectPath;

            SQLiteTransaction tr = sqlConn.BeginTransaction();
            DwgFileModel notePathFe = GetNotePath(sqlConn);
            HashSet<DwgFileModel> feDwgs = GetDwgsPath(sqlConn);
            projectElement = new ProjectElement(notePathFe, feDwgs);

            return projectElement;
        }


        //Return all Dwgs relatives path, EXCEPT Note_Path file
        public static HashSet<DwgFileModel> GetDwgsPath(SQLiteConnection sqlConn)
        {
            HashSet<DwgFileModel> dwgsSet = new HashSet<DwgFileModel>();
            List<DwgFileModel> files = DBDwgFile.GetDwgFilesExceptPNote(sqlConn);
            foreach(DwgFileModel file in files)
            {
                dwgsSet.Add(file);
            }
            return dwgsSet;
        }

        //SQL connection must be OPEN.
        //This function should be put between transaction.
        public static DwgFileModel GetNotePath(SQLiteConnection sqlConn)
        {
            DwgFileModel model = DBDwgFile.GetPNote(sqlConn);
            return model;
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
        public DwgFileModel P_NOTE;
        public HashSet<DwgFileModel> Dwgs;

        public ProjectElement() { }

        public ProjectElement(DwgFileModel P_NOTE, HashSet<DwgFileModel> dwgs)
        {
            this.P_NOTE = P_NOTE;
            this.Dwgs = dwgs;
        }

        public bool ContainsPath(string relativePath)
        {
            foreach (DwgFileModel fe in Dwgs)
            {
                if (relativePath.Contains(fe.relativePath))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

