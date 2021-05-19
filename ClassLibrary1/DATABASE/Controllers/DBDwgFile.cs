using ClassLibrary1.DATABASE.Controllers;
using ClassLibrary1.DATABASE.DBModels;
using GouvisPlumbingNew.DATABASE.DBModels;
using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GouvisPlumbingNew.DATABASE.Controllers
{
    /*
     CREATE TABLE "FILE" (
	"ID"	INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE,
	"RELATIVE_PATH"	TEXT UNIQUE,
	"MODIFIEDDATE"	INTEGER NOT NULL,
	"ISP_NOTES"	INTEGER
    );
     */

    class DBDwgFile
    {
        public static List<DwgFileModel> GetDwgFilesExceptPNote(SQLiteConnection connection)
        {
            List<DwgFileModel> dwgs = new List<DwgFileModel>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBDwgFileCommands.GetDwgFiles(command);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DwgFileModel model = GetFile(reader);

                    dwgs.Add(model);
                }
                reader.Close();
            }
            return dwgs;
        }

        public static DwgFileModel GetPNote(SQLiteConnection connection)
        {
            DwgFileModel model = null;
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBDwgFileCommands.GetPNoteFile(command);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        model = GetFile(reader);
                    }
                    reader.Close();
                }

            }
            return model;
        }

        /// <summary>
        /// Check if the connection has path.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="path">relative path</param>
        /// <returns></returns>
        public static bool HasRowPath(SQLiteConnection connection, string path)
        {
            long count = 0;

            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBDwgFileCommands.SelectCount(command, path);
                count = Convert.ToInt64(command.ExecuteScalar());
            }
            return count == 1;
        }

        public static bool HasRowID(SQLiteConnection connection, long ID)
        {
            long count = 0;
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBDwgFileCommands.SelectCount(command, ID);
                count = Convert.ToInt64(command.ExecuteScalar());
            }
            return count == 1;
        }

        public static DwgFileModel SelectRow(SQLiteConnection connection, long ID)
        {
            DwgFileModel model = null;
            using(SQLiteCommand command = connection.CreateCommand())
            {
                DBDwgFileCommands.SelectRow(command, ID);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    model = GetFile(reader);
                }
                reader.Close();
            }
            return model;
        }

        public static void UpdateRow(SQLiteConnection connection, DwgFileModel model)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBDwgFileCommands.UpdateRow(command, model);
                long check = command.ExecuteNonQuery();
                if (check == 1)
                {
                    return;
                }
                else if (check == 0)
                {
                    //throw new Exception("DBFixtureDetails -> UpdateRow -> No Row is Updated.");
                    return;
                }
                throw new Exception("DBFixtureDetails -> UpdateRow -> Update Not Successful.");
            }
        }

        public static DwgFileModel SelectRow(SQLiteConnection connection, string relPath)
        {
            DwgFileModel model = null;
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBDwgFileCommands.SelectRow(command, relPath);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    model = GetFile(reader);
                }
                reader.Close();
                
            }
            return model;
        }

        private static DwgFileModel GetFile(SQLiteDataReader reader)
        {
            DwgFileModel model = new DwgFileModel();
            model.ID = (long)reader[DBDwgFileName.ID];

            string tempRelPath = (string)reader[DBDwgFileName.RELATIVE_PATH];
            model.relativePath = tempRelPath;

            model.isP_Notes = (long)reader[DBDwgFileName.ISP_NOTES];
            model.modifieddate = (long)reader[DBDwgFileName.MODIFIEDDATE];

            return model;
        }
        public static void DeleteRow(SQLiteConnection connection, DwgFileModel model)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                //MUST DELETE CHILD FIRST.
                List<FixtureDetailsModel> fixtures = DBFixtureDetails.SelectRows(connection, model.ID);
                List<FixtureBeingUsedAreaModel> areas = DBFixtureBeingUsedArea.SelectRows(connection, model.ID);
                List<InsertPointModel> insertPoints = DBInsertPoint.SelectRows(connection, model.ID);
                List<TableModel> tables = DBTable.SelectRows(connection, model.ID);

                foreach (FixtureDetailsModel fixture in fixtures)
                {
                    DBFixtureDetails.DeleteRow(connection, fixture);
                }

                foreach (FixtureBeingUsedAreaModel area in areas)
                {
                    DBFixtureBeingUsedArea.DeleteRow(connection, area);
                }

                foreach (InsertPointModel insertPoint in insertPoints)
                {
                    DBInsertPoint.DeleteRow(insertPoint, connection);
                }

                foreach (TableModel table in tables)
                {
                    DBTable.DeleteRow(connection, table);
                }

                DBDwgFileCommands.DeleteRow(command, model.ID);
                long check = command.ExecuteNonQuery();
            }
        }

        public static long InsertRow(SQLiteConnection connection, ref DwgFileModel model)
        {
            using(SQLiteCommand command = connection.CreateCommand())
            {
                DBDwgFileCommands.InsertRow(command, model);
                long check = command.ExecuteNonQuery();
                if(check == 1)
                {
                    return connection.LastInsertRowId;
                }
                else if (check == 0)
                {
                    throw new Exception("DBDwgFile -> insertRow -> No Row is inserted.");
                }
                throw new Exception("DBDwgFile -> insertRow -> Row insertion not successful.");
            }
        }
        public static void DeleteTable(SQLiteConnection connection)
        {
            using(SQLiteCommand command = connection.CreateCommand())
            {
                DBDwgFileCommands.DeleteTable(command);
                command.ExecuteNonQuery();
            }
        }
        public static void CreateTable(SQLiteConnection connection)
        {
            using(SQLiteCommand command = connection.CreateCommand())
            {
                DBDwgFileCommands.CreateTable(command);
                command.ExecuteNonQuery();
            }
        }
    }
    class DBDwgFileCommands
    {
        public static void GetDwgFiles(SQLiteCommand command)
        {
            int isNote = 0;
            Dictionary<string, string> conDict = new Dictionary<string, string> { { DBDwgFileName.ISP_NOTES, DBDwgFileName_AT.iNote } };
            Dictionary<string, object> paraDict = new Dictionary<string, object> { { DBDwgFileName_AT.iNote, isNote } };

            DBCommand.SelectRow(DBDwgFileName.name, conDict, paraDict, command);
        }
        public static void GetPNoteFile(SQLiteCommand command)
        {
            int isNote = 1;
            Dictionary<string, string> conDict = new Dictionary<string, string> { { DBDwgFileName.ISP_NOTES, DBDwgFileName_AT.iNote } };
            Dictionary<string, object> paraDict = new Dictionary<string, object> { { DBDwgFileName_AT.iNote, isNote} };

            DBCommand.SelectRow(DBDwgFileName.name, conDict, paraDict, command);
        }

        public static void SelectCount(SQLiteCommand command, string relPath)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string> { { DBDwgFileName.RELATIVE_PATH, DBDwgFileName_AT.relPath } };
            Dictionary<string, object> paraDict = new Dictionary<string, object> { { DBDwgFileName_AT.relPath, relPath } };
            DBCommand.SelectCount(DBDwgFileName.name, conDict, paraDict, command);
        }

        public static void SelectCount(SQLiteCommand command, long ID)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string> { { DBDwgFileName.ID, DBDwgFileName_AT.id } };
            Dictionary<string, object> paraDict = new Dictionary<string, object> { { DBDwgFileName_AT.id, ID } };
            DBCommand.SelectCount(DBDwgFileName.name, conDict, paraDict, command);
        }
        public static void SelectRow(SQLiteCommand command, long ID)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string> { { DBDwgFileName.ID, DBDwgFileName_AT.id } };
            Dictionary<string, object> paraDict = new Dictionary<string, object> { { DBDwgFileName_AT.id, ID } };
            DBCommand.SelectRow(DBDwgFileName.name, conDict, paraDict, command);
        }

        public static void SelectRow(SQLiteCommand command, string relPath)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string> { {DBDwgFileName.RELATIVE_PATH, DBDwgFileName_AT.relPath} };
            Dictionary<string, object> paraDict = new Dictionary<string, object> { {DBDwgFileName_AT.relPath, relPath} };

            DBCommand.SelectRow(DBDwgFileName.name, conDict, paraDict, command);
        }
        public static void DeleteRow(SQLiteCommand command, long ID)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string> { {DBDwgFileName.ID, DBDwgFileName_AT.id} };
            Dictionary<string, object> paraDict = new Dictionary<string, object> { {DBDwgFileName_AT.id, ID} };
            DBCommand.DeleteRow(DBDwgFileName.name, conDict, paraDict, command);
        }


        //FIX LATER
        public static void GetAllFixtureBeingUsedAreaID(SQLiteCommand command, DwgFileModel model)
        {

            StringBuilder builder = new StringBuilder();
            builder.Append($"SELECT * FROM '{DBFixtureBeingUsedAreaName.name}' WHERE {DBFixtureBeingUsedAreaName.FILE_ID} = @id;");

            command.CommandText = builder.ToString();
            command.Parameters.Add(new SQLiteParameter("@id", model.ID));
        }

        public static void GetAllFixtureDetailsID(SQLiteCommand command, DwgFileModel model)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"SELECT {DBFixtureDetailsNames.ID} FROM {DBFixtureDetailsNames.name} WHERE {DBFixtureDetailsNames.FILE_ID} = @id;");
            command.CommandText = builder.ToString();
            command.Parameters.Add(new SQLiteParameter("@id", model.ID));
        }



        public static void UpdateRow(SQLiteCommand command, DwgFileModel model)
        {
            string relPath = model.relativePath;

            List<List<object>> items = new List<List<object>>()
            {
                new List<object>{ DBDwgFileName.RELATIVE_PATH, DBDwgFileName_AT.relPath, relPath },
                new List<object>{ DBDwgFileName.MODIFIEDDATE, DBDwgFileName_AT.modDate, model.modifieddate },
                new List<object>{ DBDwgFileName.ISP_NOTES, DBDwgFileName_AT.iNote, model.isP_Notes},
            };

            Dictionary<string, string> conDict = new Dictionary<string, string> { {DBDwgFileName.ID, DBDwgFileName_AT.id} };

            Dictionary<string, string> variables = new Dictionary<string, string>();
            Dictionary<string, object> paradict = new Dictionary<string, object>();

            foreach(List<object> item in items)
            {
                variables.Add((string)item[0], (string)item[1]);
                paradict.Add((string)item[1], item[2]);
            }

            paradict.Add(DBDwgFileName.ID, model.ID);

            DBCommand.UpdateRow(DBDwgFileName.name, variables, conDict, paradict, command);
        }
        public static void InsertRow(SQLiteCommand command, DwgFileModel model)
        {
            string relPath = model.relativePath;

            List<List<object>> items = new List<List<object>>
            {
                new List<object>{DBDwgFileName.ISP_NOTES, DBDwgFileName_AT.iNote, model.isP_Notes},
                new List<object>{DBDwgFileName.MODIFIEDDATE, DBDwgFileName_AT.modDate, model.modifieddate},
                new List<object>{DBDwgFileName.RELATIVE_PATH, DBDwgFileName_AT.relPath, relPath}
            };

            List<string> variables = new List<string>();
            Dictionary<string, object> paraDict = new Dictionary<string, object>();
            foreach (List<object> item in items)
            {
                variables.Add((string)item[0]);
                paraDict.Add((string)item[1], item[2]);
            }

            DBCommand.InsertCommand(DBDwgFileName.name, variables, paraDict, command);
        }

        public static void DeleteTable(SQLiteCommand command)
        {
            DBCommand.DeleteTable(DBDwgFileName.name, command);
        }
        public static void CreateTable(SQLiteCommand command)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(string.Format("CREATE TABLE IF NOT EXISTS {0} (", DBDwgFileName.name));
            builder.Append(string.Format($"'{DBDwgFileName.ID}' INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE,"));
            builder.Append(string.Format($"'{DBDwgFileName.RELATIVE_PATH}' TEXT NOT NULL UNIQUE,"));
            builder.Append(string.Format($"'{DBDwgFileName.MODIFIEDDATE}' INTEGER NOT NULL,"));
            builder.Append(string.Format($"'{DBDwgFileName.ISP_NOTES}' INTEGER"));
            builder.Append(string.Format(");"));

            command.CommandText = builder.ToString();
        }
    }

    class DBDwgFileName
    {
        public static string name = "DB_FILE";
        public static string ID = "ID";
        public static string RELATIVE_PATH = "RELATIVE_PATH";
        public static string MODIFIEDDATE = "MODIFIEDDATE";
        public static string ISP_NOTES = "ISP_NOTES";
    }

    class DBDwgFileName_AT
    {
        public static string name = "@name";
        public static string id = "@id";
        public static string relPath = "@relpath";
        public static string modDate = "@moddate";
        public static string iNote = "@inote";
    }
}
