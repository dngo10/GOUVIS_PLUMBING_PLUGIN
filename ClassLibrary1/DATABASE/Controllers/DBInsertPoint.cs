using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using GouvisPlumbingNew.DATABASE.DBModels;

namespace GouvisPlumbingNew.DATABASE.Controllers
{
    /*
    CREATE TABLE "INSERT_POINT" (
	    "ID"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	    "ALIAS"	TEXT,
	    "NAME"	TEXT,
	    "HANDLE"	TEXT NOT NULL,
	    "POSITION_ID"	INTEGER NOT NULL,
	    "TRANSFORM_ID"	INTEGER NOT NULL,
	    "FILE_ID"	INTEGER NOT NULL,
    	FOREIGN KEY("POSITION_ID") REFERENCES "INSERT_POINT"("ID") ON DELETE CASCADE,
	    FOREIGN KEY("FILE_ID") REFERENCES "FILE"("ID") ON DELETE CASCADE,
	    FOREIGN KEY("TRANSFORM_ID") REFERENCES "MATRIX3D"("ID") ON DELETE CASCADE
    );
     */
    class DBInsertPoint
    {
        public static List<InsertPointModel> SelectRows(SQLiteConnection connection, long fileID)
        {
            List<InsertPointModel> insertPoints = new List<InsertPointModel>();
            using(SQLiteCommand command = connection.CreateCommand())
            {
                DBInsertPointCommands.SelectRows(command, fileID);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string alias = (string)reader[DBInsertPointName.ALIAS];
                    string name = (string)reader[DBInsertPointName.NAME];
                    long ID = (long)reader[DBInsertPointName.ID];
                    string handle = (string)reader[DBInsertPointName.HANDLE];
                    Point3dModel pos = DBPoint3D.SelectRow(connection, (long)reader[DBInsertPointName.POSITION_ID]);

                    DwgFileModel file = DBDwgFile.SelectRow(connection, (long)reader[DBInsertPointName.FILE_ID]);
                    Matrix3dModel matrix = DBMatrix3d.SelectRow(connection, (long)reader[DBInsertPointName.TRANSFORM_ID]);

                    InsertPointModel model = new InsertPointModel(alias, name, ID, file, handle, pos, matrix);
                    insertPoints.Add(model);
                }
                reader.Close();
            }

            return insertPoints;
        }
        public static bool HasRow(SQLiteConnection connection, long ID)
        {
            long count = 0;
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBInsertPointCommands.SelectCount(command, ID);
                count = Convert.ToInt64(command.ExecuteScalar());

            }
            return count == 1;
        }
        public static InsertPointModel SelectRow(SQLiteConnection connection, string handle)
        {
            InsertPointModel model = null;
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBInsertPointCommands.SelectRow(command, handle);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string alias = (string)reader[DBInsertPointName.ALIAS];
                    string name = (string)reader[DBInsertPointName.NAME];
                    long ID = (long)reader[DBInsertPointName.ID];
                    Point3dModel pos = DBPoint3D.SelectRow(connection, (long)reader[DBInsertPointName.POSITION_ID]);
                    string handle1 = (string)reader[DBInsertPointName.HANDLE];
                    DwgFileModel file = DBDwgFile.SelectRow(connection, (long)reader[DBInsertPointName.FILE_ID]);
                    Matrix3dModel matrix = DBMatrix3d.SelectRow(connection, (long)reader[DBInsertPointName.TRANSFORM_ID]);

                    model = new InsertPointModel(alias, name, ID, file, handle1, pos, matrix);
                }
                reader.Close();
            }
            return model;
        }
        public static InsertPointModel SelectRow(SQLiteConnection connection, long ID)
        {
            InsertPointModel model = null;
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBInsertPointCommands.SelectRow(command, ID);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string alias = (string)reader[DBInsertPointName.ALIAS];
                    string name = (string)reader[DBInsertPointName.NAME];
                    string handle = (string)reader[DBInsertPointName.HANDLE];
                    Point3dModel pos = DBPoint3D.SelectRow(connection, (long)reader[DBInsertPointName.POSITION_ID]);
                    long ID1 = (long)reader[DBInsertPointName.ID];
                    DwgFileModel file = DBDwgFile.SelectRow(connection,(long)reader[DBInsertPointName.FILE_ID]);
                    Matrix3dModel matrix = DBMatrix3d.SelectRow(connection, (long)reader[DBInsertPointName.TRANSFORM_ID]);

                    model = new InsertPointModel(alias, name, ID1, file, handle, pos, matrix);
                }
                reader.Close();
            }
            return model;
        }

        public static void CreateTable(SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBInsertPointCommands.CreateTable(command);
                command.ExecuteNonQuery();
            }
        }

        public static void DeleteTable(SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBInsertPointCommands.DeleteTable(command);
                command.ExecuteNonQuery();
            }

        }
        public static long InsertRow(ref InsertPointModel model, SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBInsertPointCommands.InsertRow(command, model);
                int check = command.ExecuteNonQuery();
                if (check == 1)
                {
                    model.ID = connection.LastInsertRowId;
                    return model.ID;
                }
                else if (check == 0)
                {
                    throw new Exception("DBInsertPoint -> Insert -> No Point Was Inserted");
                }
                throw new Exception("DBInsertPoint -> Insert -> Insert Point not successful.");
            }
        }
        public static long UpdateRow(InsertPointModel model, SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBInsertPointCommands.UpdateRow(command, model);
                int check = command.ExecuteNonQuery();
                if (check == 1)
                {
                    return connection.LastInsertRowId;
                }
                else if (check == 0)
                {
                    throw new Exception("DBInsertPoint -> UpdateRow -> No Row is Updated.");
                }
                throw new Exception("DBInsertPoint -> UpdateRow -> Update Point not successful.");
            }

        }

        public static long DeleteRow(long ID, SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBInsertPointCommands.DeleteRow(command, ID);
                long check = command.ExecuteNonQuery();
                if (check == 1)
                {
                    return connection.LastInsertRowId;
                }
                else if (check == 0)
                {
                    throw new Exception("DBInsertPoint -> DeleteRow -> No Row is Deleted");
                }
                throw new Exception("DBInsertPoint -> DeleteRow -> Delete Row not successful");
            }
        }
    }

    class DBInsertPointCommands {

        public static void SelectCount(SQLiteCommand command, long ID)
        {
            string commandStr = $"SELECT COUNT(*) FROM {DBInsertPointName.tableName} WHERE {DBInsertPointName.ID} = @id;";
            command.CommandText = commandStr;
            command.Parameters.Add(new SQLiteParameter("@id", ID));
        }

        public static void SelectRows(SQLiteCommand command, long fileID)
        {
            string commandStr = $"SELECT * FROM {DBInsertPointName.tableName} WHERE {DBInsertPointName.FILE_ID} = @fileID;";
            command.CommandText = commandStr;
            command.Parameters.Add(new SQLiteParameter("@fileID", fileID));
        }

        public static void SelectRow(SQLiteCommand command, string handle)
        {
            string commandStr = $"SELECT * FROM {DBInsertPointName.tableName} WHERE {DBInsertPointName.HANDLE} = @handle;";
            command.CommandText = commandStr;
            command.Parameters.Add(new SQLiteParameter("@handle", handle));
        }

        public static void SelectRow(SQLiteCommand command, long ID)
        {
            string commandStr = $"SELECT * FROM {DBInsertPointName.tableName} WHERE {DBInsertPointName.ID} = @id;";
            command.CommandText = commandStr;
            command.Parameters.Add(new SQLiteParameter("@id", ID));
        }

        public static void DeleteRow(SQLiteCommand command, long ID)
        {
            string commandStr = $"DELETE FROM {DBInsertPointName.tableName} WHERE {DBInsertPointName.ID} = @id;";
            command.CommandText = commandStr;
            command.Parameters.Add(new SQLiteParameter("@id", ID));
        }

        public static void UpdateRow(SQLiteCommand command, InsertPointModel model)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"UPDATE '{DBInsertPointName.tableName}' SET ");
            builder.Append($"'{DBInsertPointName.ALIAS}' = @alias , ");
            builder.Append($"'{DBInsertPointName.NAME}' = @name , ");
            builder.Append($"'{DBInsertPointName.POSITION_ID}' = @pos , ");
            builder.Append($"'{DBInsertPointName.HANDLE}' = @handle , ");
            builder.Append($"'{DBInsertPointName.TRANSFORM_ID}' = @transform , ");
            builder.Append($"'{DBInsertPointName.FILE_ID}' = @file WHERE ");
            builder.Append($"'{DBInsertPointName.ID}' = @id;");

            command.CommandText = builder.ToString();
            command.Parameters.Add(new SQLiteParameter("@alias", model.alias));
            command.Parameters.Add(new SQLiteParameter("@name", model.name));
            command.Parameters.Add(new SQLiteParameter("@pos", model.position.ID));
            command.Parameters.Add(new SQLiteParameter("@handle", model.handle));
            command.Parameters.Add(new SQLiteParameter("@transform", model.matrixTransform.ID));
            command.Parameters.Add(new SQLiteParameter("@file", model.file.ID));

        }

        public static void InsertRow(SQLiteCommand command, InsertPointModel model)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("INSERT INTO '{0}' ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}') VALUES (@alias, @name, @handle, @pos, @transform, @file);",
                DBInsertPointName.tableName,
                DBInsertPointName.ALIAS,
                DBInsertPointName.NAME,
                DBInsertPointName.HANDLE,
                DBInsertPointName.POSITION_ID,
                DBInsertPointName.TRANSFORM_ID,
                DBInsertPointName.FILE_ID
                ));
            command.CommandText = builder.ToString();
            command.Parameters.Add(new SQLiteParameter("@alias", model.alias));
            command.Parameters.Add(new SQLiteParameter("@name", model.name));
            command.Parameters.Add(new SQLiteParameter("@handle", model.handle));
            command.Parameters.Add(new SQLiteParameter("@pos", model.position.ID));
            command.Parameters.Add(new SQLiteParameter("@transform", model.matrixTransform.ID));
            command.Parameters.Add(new SQLiteParameter("@file", model.file.ID));
        }

        public static void DeleteTable(SQLiteCommand command)
        {
            string commandstr = $"DROP TABLE IF EXISTS '{DBInsertPointName.tableName}';";
            command.CommandText = commandstr;
        }
        public static void CreateTable(SQLiteCommand command){
            StringBuilder builder = new StringBuilder();
            builder.Append($"CREATE TABLE '{DBInsertPointName.tableName}'( ");
            builder.Append($"'{DBInsertPointName.ID}'    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, ");
            builder.Append($"'{DBInsertPointName.ALIAS}' TEXT, ");
            builder.Append($"'{DBInsertPointName.NAME}'  TEXT, ");
            builder.Append($"'{DBInsertPointName.HANDLE}'    TEXT NOT NULL,");
            builder.Append($"'{DBInsertPointName.POSITION_ID}'   INTEGER NOT NULL, ");
            builder.Append($"'{DBInsertPointName.TRANSFORM_ID}'  INTEGER NOT NULL, ");
            builder.Append($"'{DBInsertPointName.FILE_ID}'   INTEGER NOT NULL, ");
            builder.Append($"FOREIGN KEY('{DBInsertPointName.POSITION_ID}') REFERENCES '{DBPoint3DName.tableName}'('{DBPoint3DName.ID}') ON DELETE CASCADE, ");
            builder.Append($"FOREIGN KEY('{DBInsertPointName.FILE_ID}') REFERENCES '{DBDwgFileName.name}'('{DBDwgFileName.ID}') ON DELETE CASCADE, ");
            builder.Append($"FOREIGN KEY('{DBInsertPointName.TRANSFORM_ID}') REFERENCES '{DBMatrixName.name}'('{DBMatrixName.ID}') ON DELETE CASCADE ");
            builder.Append(");");

            command.CommandText = builder.ToString();
        }
    }

    class DBInsertPointName {
        public const string ID = "ID";
        public const string ALIAS = "ALIAS";
        public const string NAME = "NAME";
        public const string HANDLE = "HANDLE";
        public const string POSITION_ID = "POSITION_ID";
        public const string TRANSFORM_ID = "TRANSFORM_ID";
        public const string FILE_ID = "FILE_ID";
        public const string tableName = "INSERT_POINT";
    }
}
