using ClassLibrary1.DATABASE.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DATABASE.Controllers
{

    /*
     CREATE TABLE "FIXTURE_BEING_USED_AREA" (
	    "ID"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	    "HANDLE"	TEXT NOT NULL,
	    "POSITION_ID"	INTEGER NOT NULL,
	    "X"	REAL NOT NULL,
	    "Y"	REAL NOT NULL,
	    "ORIGIN_ID"	INTEGER NOT NULL,
	    "POINT_TOP_ID"	INTEGER NOT NULL,
	    "POINT_BOTTOM_ID"	INTEGER NOT NULL,
	    "TRANSFORM_ID"	INTEGER NOT NULL,
	    FOREIGN KEY("POINT_TOP_ID") REFERENCES "POINT3D" ON DELETE CASCADE,
	    FOREIGN KEY("ORIGIN_ID") REFERENCES "POINT3D" ON DELETE CASCADE,
	    FOREIGN KEY("POINT_BOTTOM_ID") REFERENCES "POINT3D" ON DELETE CASCADE,
	    FOREIGN KEY("POSITION_ID") REFERENCES "POINT3D" ON DELETE CASCADE
        );
     */

    class DBFixtureBeingUsedArea
    {
        public static FixtureBeingUsedAreaModel SelectRow(SQLiteConnection connection, string handle)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBFixtureBeingUsedAreaCommands.SelectRow(command, handle);
                return GetFixtureBeingUsedAreaModel(command);
            }
        }
        public static FixtureBeingUsedAreaModel SelectRow(SQLiteConnection connection, long ID)
        {
            using(SQLiteCommand command = connection.CreateCommand())
            {
                DBFixtureBeingUsedAreaCommands.SelectRow(command, ID);
                return GetFixtureBeingUsedAreaModel(command);
            }
        }

        private static FixtureBeingUsedAreaModel GetFixtureBeingUsedAreaModel(SQLiteCommand command)
        {
            FixtureBeingUsedAreaModel model = null;

            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                model = new FixtureBeingUsedAreaModel();
                model.ID = (long)reader[DBFixtureBeingUsedAreaName.ID];
                model.handle = (string)reader[DBFixtureBeingUsedAreaName.HANDLE];
                model.position = DBPoint3D.SelectRow(command.Connection, (long)reader[DBFixtureBeingUsedAreaName.POSITION_ID]);
                model.pointTop = DBPoint3D.SelectRow(command.Connection, (long)reader[DBFixtureBeingUsedAreaName.POINT_TOP_ID]);
                model.pointBottom = DBPoint3D.SelectRow(command.Connection, (long)reader[DBFixtureBeingUsedAreaName.POINT_BOTTOM_ID]);
                model.matrixTransform = DBMatrix3d.SelectRow(command.Connection, (long)reader[DBFixtureBeingUsedAreaName.TRANSFORM_ID]);
                model.X = (double)reader[DBFixtureBeingUsedAreaName.X];
                model.Y = (double)reader[DBFixtureBeingUsedAreaName.Y];
                model.file = DBDwgFile.SelectRow(command.Connection,(long)reader[DBFixtureBeingUsedAreaName.FILE_ID]);
            }

            return model;
        }

        public static void DeleteRow(SQLiteConnection connection, FixtureBeingUsedAreaModel model)
        {
            using(SQLiteCommand command = connection.CreateCommand())
            {
                DBFixtureBeingUsedAreaCommands.DeleteRow(command, model.ID);
                DBPoint3D.DeleteRow(model.position.ID, connection);
                DBPoint3D.DeleteRow(model.origin.ID, connection);
                DBPoint3D.DeleteRow(model.pointTop.ID, connection);
                DBPoint3D.DeleteRow(model.pointBottom.ID, connection);
                DBMatrix3d.DeleteRow(connection, model.matrixTransform.ID);
            }
        }
        
        public static long InsertRow(SQLiteConnection connection, FixtureBeingUsedAreaModel model)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBFixtureBeingUsedAreaCommands.InsertRow(model, command);
                long check = command.ExecuteNonQuery();
                if(check == 1)
                {
                    return connection.LastInsertRowId;
                }else if(check == 0)
                {
                    throw new Exception("DBFixtureBeingUsed -> insertRow -> No Row is inserted.");
                }
                throw new Exception("DBFixtureBeingUsed -> insertRow -> Row insertion not successful.");
            }
        }
        public static void CreateTable(SQLiteConnection connection)
        {
            using(SQLiteCommand command = connection.CreateCommand())
            {
                DBFixtureBeingUsedAreaCommands.CreateTable(command);
                command.ExecuteNonQuery();
            }
        }

        public static void DeleteTable(SQLiteConnection connection)
        {
            using(SQLiteCommand command = connection.CreateCommand())
            {
                DBFixtureBeingUsedAreaCommands.DeleteTable(command);
                command.ExecuteNonQuery();
            }
        }
    }

    class DBFixtureBeingUsedAreaCommands
    {
        public static void SelectRow(SQLiteCommand command, string handle)
        {
            command.CommandText = string.Format("SELECT * FROM '{0}' WHERE '{1}' = @handle;", DBFixtureBeingUsedAreaName.name, DBFixtureBeingUsedAreaName.HANDLE);
            command.Parameters.Add(new SQLiteParameter("@handle", handle));
        }
        public static void SelectRow(SQLiteCommand command, long ID)
        {
            command.CommandText = string.Format("SELECT * FROM '{0}' WHERE '{1}' = @id", DBFixtureBeingUsedAreaName.name, DBFixtureBeingUsedAreaName.ID);
            command.Parameters.Add(new SQLiteParameter("@id", ID));
        }
        public static void DeleteRow(SQLiteCommand command, long ID)
        {
            command.CommandText = string.Format("DELETE FROM '{0}' WHERE '{1}' = @id;", DBFixtureBeingUsedAreaName.name, DBFixtureBeingUsedAreaName.ID);
            command.Parameters.Add(new SQLiteParameter("@id", ID));
        }

        public static void DeleteRow(SQLiteCommand command, string handle)
        {
            command.CommandText = string.Format("DELETE FROM '{0}' WHERE '{1}' = @handle;", DBFixtureBeingUsedAreaName.name, DBFixtureBeingUsedAreaName.HANDLE);
            command.Parameters.Add(new SQLiteParameter("@handle", handle));
        }
        public static void UpdateRow(SQLiteCommand command, FixtureBeingUsedAreaModel model)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("UPDATE {0} SET ", DBFixtureBeingUsedAreaName.name));
            builder.Append(string.Format("'{0}' = @handle ,", DBFixtureBeingUsedAreaName.HANDLE));
            builder.Append(string.Format("'{0}' = @position ,", DBFixtureBeingUsedAreaName.POSITION_ID));
            builder.Append(string.Format("'{0}' = @X ,", DBFixtureBeingUsedAreaName.X));
            builder.Append(string.Format("'{0}' = @Y ,", DBFixtureBeingUsedAreaName.Y));
            builder.Append(string.Format("'{0}' = @origin ,", DBFixtureBeingUsedAreaName.ORIGIN_ID));
            builder.Append(string.Format("'{0}' = @pointop ,", DBFixtureBeingUsedAreaName.POINT_BOTTOM_ID));
            builder.Append(string.Format("'{0}' = @pointbottom ,", DBFixtureBeingUsedAreaName.TRANSFORM_ID));
            builder.Append(string.Format("'{0}' = @file WHERE ", DBFixtureBeingUsedAreaName.FILE_ID));
            builder.Append(string.Format("'{0}' = @id;", DBFixtureBeingUsedAreaName.ID));

            command.CommandText = builder.ToString();
            command.Parameters.Add(new SQLiteParameter("@handle", model.handle));
            command.Parameters.Add(new SQLiteParameter("@position", model.position.ID));
            command.Parameters.Add(new SQLiteParameter("@X", model.X));
            command.Parameters.Add(new SQLiteParameter("@Y", model.Y));
            command.Parameters.Add(new SQLiteParameter("@origin", model.origin.ID));
            command.Parameters.Add(new SQLiteParameter("@pointop", model.pointTop.ID));
            command.Parameters.Add(new SQLiteParameter("@pointbottom", model.pointBottom.ID));
            command.Parameters.Add(new SQLiteParameter("@file", model.file.ID));
            command.Parameters.Add(new SQLiteParameter("@id", model.ID));
        }
        public static void InsertRow(FixtureBeingUsedAreaModel model, SQLiteCommand command)
        {
            string commandStr = string.Format("INSERT INTO {0} ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}') VALUES (@handle, @position, @X , @Y, @origin, @pointTop, @pointBottom, @matrix, @file);",
                DBFixtureBeingUsedAreaName.name,
                DBFixtureBeingUsedAreaName.HANDLE,
                DBFixtureBeingUsedAreaName.POSITION_ID,
                DBFixtureBeingUsedAreaName.X,
                DBFixtureBeingUsedAreaName.Y,
                DBFixtureBeingUsedAreaName.ORIGIN_ID,
                DBFixtureBeingUsedAreaName.POINT_TOP_ID,
                DBFixtureBeingUsedAreaName.POINT_BOTTOM_ID,
                DBFixtureBeingUsedAreaName.TRANSFORM_ID,
                DBFixtureBeingUsedAreaName.FILE_ID
                );

            command.CommandText = commandStr;
            command.Parameters.Add(new SQLiteParameter("@handle", model.handle));
            command.Parameters.Add(new SQLiteParameter("@position", model.position.ID));
            command.Parameters.Add(new SQLiteParameter("@X", model.X));
            command.Parameters.Add(new SQLiteParameter("@Y", model.Y));
            command.Parameters.Add(new SQLiteParameter("@origin", model.origin.ID));
            command.Parameters.Add(new SQLiteParameter("@pointTop", model.pointTop.ID));
            command.Parameters.Add(new SQLiteParameter("@pointBottom", model.pointBottom.ID));
            command.Parameters.Add(new SQLiteParameter("@matrix", model.matrixTransform.ID));
            command.Parameters.Add(new SQLiteParameter("@file", model.file.ID));
        }
        public static void CreateTable(SQLiteCommand command)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("CREATE TABLE '{0}'(", DBFixtureBeingUsedAreaName.name));
            builder.Append(string.Format("'{0}' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,", DBFixtureBeingUsedAreaName.ID));
            builder.Append(string.Format("'{0}' TEXT NOT NULL,", DBFixtureBeingUsedAreaName.HANDLE));
            builder.Append(string.Format("'{0}' INTEGER NOT NULL,", DBFixtureBeingUsedAreaName.POSITION_ID));
            builder.Append(string.Format("'{0}' REAL NOT NULL,", DBFixtureBeingUsedAreaName.X));
            builder.Append(string.Format("'{0}' REAL NOT NULL,", DBFixtureBeingUsedAreaName.Y));
            builder.Append(string.Format("'{0}' INTEGER NOT NULL,", DBFixtureBeingUsedAreaName.ORIGIN_ID));
            builder.Append(string.Format("'{0}' INTEGER NOT NULL,", DBFixtureBeingUsedAreaName.POINT_TOP_ID));
            builder.Append(string.Format("'{0}' INTEGER NOT NULL,", DBFixtureBeingUsedAreaName.POINT_BOTTOM_ID));
            builder.Append(string.Format("'{0}' INTEGER NOT NULL,", DBFixtureBeingUsedAreaName.TRANSFORM_ID));
            builder.Append(string.Format("'{0}' INTEGER NOT NULL,", DBFixtureBeingUsedAreaName.FILE_ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE,", DBFixtureBeingUsedAreaName.POINT_TOP_ID, DBPoint3DName.tableName, DBPoint3DName.ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE,", DBFixtureBeingUsedAreaName.ORIGIN_ID, DBPoint3DName.tableName, DBPoint3DName.ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE,", DBFixtureBeingUsedAreaName.POINT_BOTTOM_ID, DBPoint3DName.tableName, DBPoint3DName.ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE,", DBFixtureBeingUsedAreaName.POSITION_ID, DBPoint3DName.tableName, DBPoint3DName.ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE,", DBFixtureBeingUsedAreaName.FILE_ID, DBDwgFileName.name, DBDwgFileName.ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE ", DBFixtureBeingUsedAreaName.TRANSFORM_ID, DBMatrixName.name, DBMatrixName.ID)); 
            builder.Append(string.Format(");"));
            command.CommandText = builder.ToString();
        }

        public static void DeleteTable(SQLiteCommand command)
        {
            command.CommandText = string.Format("DROP TABLE IF EXISTS '{0}';", DBFixtureBeingUsedAreaName.name);
        }
    }

    class DBFixtureBeingUsedAreaName
    {
        //x and y are width and height of the XY dynamic dimension.
        public const string ID = "ID";
        public const string HANDLE = "HANDLE";
        public const string POSITION_ID = "POSITION_ID";
        public const string ORIGIN_ID = "ORIGIN_ID";
        public const string POINT_BOTTOM_ID = "POINT_BOTTOM_ID";
        public const string POINT_TOP_ID = "POINT_TOP_ID";
        public const string TRANSFORM_ID = "TRANSFORM_ID";
        public const string X = "X";
        public const string Y = "Y";
        public const string name = "FIXTURE_BEING_USED_AREA";
        public const string FILE_ID = "FILE_ID";

        //USED ONLY FOR BLOCKREF
        public const string basePoint = "Origin";
    }
}
