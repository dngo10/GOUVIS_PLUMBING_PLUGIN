using GouvisPlumbingNew.DATABASE.DBModels;
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
                    DwgFileModel model = new DwgFileModel();
                    model.ID = (long)reader[DBDwgFileName.ID];
                    model.relativePath = (string)reader[DBDwgFileName.RELATIVE_PATH];
                    model.isP_Notes = (long)reader[DBDwgFileName.ISP_NOTES];
                    model.modifieddate = (long)reader[DBDwgFileName.MODIFIEDDATE];

                    dwgs.Add(model);
                }
            }
            return dwgs;
        }

        public static DwgFileModel GetPNote(SQLiteConnection connection)
        {
            DwgFileModel model = null;
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBDwgFileCommands.GetPNoteFile(command);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    model = new DwgFileModel();
                    model.ID = (long)reader[DBDwgFileName.ID];
                    model.relativePath = (string)reader[DBDwgFileName.RELATIVE_PATH];
                    model.isP_Notes = (long)reader[DBDwgFileName.ISP_NOTES];
                    model.modifieddate = (long)reader[DBDwgFileName.MODIFIEDDATE];
                }
            }
            return model;
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
                    model = new DwgFileModel();
                    model.ID = (long)reader[DBDwgFileName.ID];
                    model.relativePath = (string)reader[DBDwgFileName.RELATIVE_PATH];
                    model.isP_Notes = (int)reader[DBDwgFileName.ISP_NOTES];
                    model.modifieddate = (long)reader[DBDwgFileName.MODIFIEDDATE];
                }
            }
            return model;
        }
        public static void DeleteRow(SQLiteConnection connection, DwgFileModel model)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                //MUST DELETE CHILD FIRST.
                DBDwgFileCommands.GetAllFixtureDetailsID(command, model);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    long fixtureDetailsID = (long)reader[DBFixtureDetailsNames.ID];
                    FixtureDetailsModel fDetails = DBFixtureDetails.SelectRow(connection, fixtureDetailsID);
                    if(fDetails != null) DBFixtureDetails.DeleteRow(connection, fDetails);
                }

                DBDwgFileCommands.GetAllFixtureBeingUsedAreaID(command, model);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    long fixtureBeingUsedAreaID = (long)reader[DBFixtureBeingUsedAreaName.ID];
                    FixtureBeingUsedAreaModel fDArea = DBFixtureBeingUsedArea.SelectRow(connection, fixtureBeingUsedAreaID);
                    if(fDArea != null) DBFixtureBeingUsedArea.DeleteRow(connection, fDArea);
                }

                DBFixtureBeingUsedAreaCommands.DeleteRow(command, model.ID);
                command.ExecuteNonQuery();
            }
        }

        public static long InsertRow(SQLiteConnection connection, DwgFileModel model)
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
            string commandStr = string.Format("SELECT * FROM {0} WHERE {1} = @ispNote;", DBDwgFileName.name, DBDwgFileName.ISP_NOTES);
            command.CommandText = commandStr;
            int temp = 0;
            command.Parameters.Add(new SQLiteParameter("@ispNote", temp));
        }
        public static void GetPNoteFile(SQLiteCommand command)
        {
            //SELECT RELATIVE_PATH , MODIFIEDDATE FROM FILE WHERE ISP_NOTES = 1;
            string commandStr = string.Format("SELECT * FROM {0} WHERE {1} = @ispNote;", DBDwgFileName.name, DBDwgFileName.ISP_NOTES);
            command.CommandText = commandStr;
            int temp = 1;
            command.Parameters.Add(new SQLiteParameter("@ispNote", temp));
        }
        public static void SelectRow(SQLiteCommand command, long ID)
        {
            string commandStr = string.Format("SELECT * FROM {0} WHERE {1} = @id;", DBDwgFileName.name, DBDwgFileName.ID);
            command.CommandText = commandStr;
            command.Parameters.Add(new SQLiteParameter("@id", ID));
        }
        public static void DeleteRow(SQLiteCommand command, DwgFileModel model)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("DELETE FROM {0} WHERE {1} = @id;", DBDwgFileName.name, DBDwgFileName.ID));
            command.CommandText = builder.ToString();
            command.Parameters.Add(new SQLiteParameter("@id", model.ID));
        }

        public static void GetAllFixtureBeingUsedAreaID(SQLiteCommand command, DwgFileModel model)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"SELECT '{DBFixtureBeingUsedAreaName.ID}' FROM '{DBFixtureBeingUsedAreaName.name}' WHERE '{DBFixtureBeingUsedAreaName.FILE_ID}' = @id;");

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
            StringBuilder builder = new StringBuilder();
            builder.Append($"UPDATE '{DBDwgFileName.name}' SET ");
            builder.Append($"'{DBDwgFileName.RELATIVE_PATH}' = @path ,");
            builder.Append($"'{DBDwgFileName.MODIFIEDDATE}' = @date ,");
            builder.Append($"'{DBDwgFileName.ISP_NOTES}' = @note WHERE ");
            builder.Append($"'{DBDwgFileName.ID}' = @id;");

            command.CommandText = builder.ToString();
            command.Parameters.Add(new SQLiteParameter("@path", model.relativePath));
            command.Parameters.Add(new SQLiteParameter("@date", model.modifieddate));
            command.Parameters.Add(new SQLiteParameter("@id", model.ID));
        }
        public static void InsertRow(SQLiteCommand command, DwgFileModel model)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("INSERT INTO '{0}' ('{1}', '{2}', '{3}') VALUES ",
                DBDwgFileName.name, 
                DBDwgFileName.RELATIVE_PATH,
                DBDwgFileName.MODIFIEDDATE,
                DBDwgFileName.ISP_NOTES));

            builder.Append(string.Format("(@path, @date, @note);"));
            command.CommandText = builder.ToString();
            command.Parameters.Add(new SQLiteParameter("@path", model.relativePath));
            command.Parameters.Add(new SQLiteParameter("@date", model.modifieddate));
            command.Parameters.Add(new SQLiteParameter("@note", model.isP_Notes));

        }
        public static void DeleteTable(SQLiteCommand command)
        {
            string commandStr = string.Format($"DROP TABLE IF EXISTS {DBDwgFileName.name};");
            command.CommandText = commandStr;
        }
        public static void CreateTable(SQLiteCommand command)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(string.Format("CREATE TABLE IF NOT EXISTS {0} (", DBDwgFileName.name));
            builder.Append(string.Format($"'{DBDwgFileName.ID}' INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE,"));
            builder.Append(string.Format($"'{DBDwgFileName.RELATIVE_PATH}' TEXT UNIQUE,"));
            builder.Append(string.Format($"'{DBDwgFileName.MODIFIEDDATE}' INTEGER NOT NULL,"));
            builder.Append(string.Format($"'{DBDwgFileName.ISP_NOTES}' INTEGER"));
            builder.Append(string.Format(");"));

            command.CommandText = builder.ToString();
        }
    }

    class DBDwgFileName
    {
        public static string name = "FILE";
        public static string ID = "ID";
        public static string RELATIVE_PATH = "RELATIVE_PATH";
        public static string MODIFIEDDATE = "MODIFIEDDATE";
        public static string ISP_NOTES = "ISP_NOTES";
    }
}
