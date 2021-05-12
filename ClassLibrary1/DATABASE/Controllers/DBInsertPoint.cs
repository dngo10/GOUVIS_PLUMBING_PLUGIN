using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using GouvisPlumbingNew.DATABASE.DBModels;
using GouvisPlumbingNew.HELPERS;
using ClassLibrary1.DATABASE.Controllers.BlockInterFace;

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
                    Matrix3dModel matrix = DBMatrix3d.SelectRow(connection, (long)reader[DBInsertPointName.MATRIX_ID]);

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
        public static InsertPointModel SelectRow(SQLiteConnection connection, string handle, long fileID)
        {
            InsertPointModel model = null;
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBInsertPointCommands.SelectRow(command, handle, fileID);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string alias = (string)reader[DBInsertPointName.ALIAS];
                    string name = (string)reader[DBInsertPointName.NAME];
                    long ID = (long)reader[DBInsertPointName.ID];
                    Point3dModel pos = DBPoint3D.SelectRow(connection, (long)reader[DBInsertPointName.POSITION_ID]);
                    string handle1 = (string)reader[DBInsertPointName.HANDLE];
                    DwgFileModel file = DBDwgFile.SelectRow(connection, (long)reader[DBInsertPointName.FILE_ID]);
                    Matrix3dModel matrix = DBMatrix3d.SelectRow(connection, (long)reader[DBInsertPointName.MATRIX_ID]);

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
                    Matrix3dModel matrix = DBMatrix3d.SelectRow(connection, (long)reader[DBInsertPointName.MATRIX_ID]);

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

        public static long DeleteRow(InsertPointModel model, SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                if(DBInsertPoint.HasRow(connection, model.ID))
                {
                    DBInsertPointCommands.DeleteRow(command, model.ID);
                    long check = command.ExecuteNonQuery();
                    if (model.position != null) DBPoint3D.DeleteRow(model.position.ID, command.Connection);
                    if (model.matrixTransform != null) DBMatrix3d.DeleteRow(command.Connection, model.matrixTransform.ID);
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
                Console.WriteLine("No Round is Inserted");
                return ConstantName.invalidNum;
            }
        }
    }

    class DBInsertPointCommands {

        public static void SelectCount(SQLiteCommand command, long ID)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string> { { DBInsertPointName.ID, DBInsertPointName_AT.id } };
            Dictionary<string, object> paraDict = new Dictionary<string, object> { { DBInsertPointName_AT.id, ID } };
            DBCommand.SelectRow(DBInsertPointName.tableName, conDict, paraDict, command);
        }

        public static void SelectRows(SQLiteCommand command, long fileID)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string> { { DBInsertPointName.FILE_ID, DBInsertPointName_AT.file } };
            Dictionary<string, object> paraDict = new Dictionary<string, object> { { DBInsertPointName_AT.file, fileID } };
            DBCommand.SelectRow(DBInsertPointName.tableName, conDict, paraDict, command);
        }

        public static void SelectRow(SQLiteCommand command, string handle, long file_ID)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string> {
                { DBInsertPointName.HANDLE, DBInsertPointName_AT.handle },
                {DBInsertPointName.FILE_ID, DBInsertPointName_AT.file } };

            Dictionary<string, object> paraDict = new Dictionary<string, object> { { DBInsertPointName_AT.handle, handle},
                                                                                   { DBInsertPointName_AT.file, file_ID} };
            DBCommand.SelectRow(DBInsertPointName.tableName, conDict, paraDict, command);
        }

        public static void SelectRow(SQLiteCommand command, long ID)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string> { { DBInsertPointName.ID, DBInsertPointName_AT.id } };
            Dictionary<string, object> paraDict = new Dictionary<string, object> { { DBInsertPointName_AT.id, ID } };
            DBCommand.SelectRow(DBInsertPointName.tableName, conDict, paraDict, command);
        }

        public static void DeleteRow(SQLiteCommand command, long ID)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string> { {DBInsertPointName.ID, DBInsertPointName_AT.id} };
            Dictionary<string, object> paraDict = new Dictionary<string, object> { {DBInsertPointName_AT.id, ID} };
            DBCommand.DeleteRow(DBInsertPointName.tableName, conDict, paraDict, command);
        }

        public static void UpdateRow(SQLiteCommand command, InsertPointModel model)
        {
            List<List<object>> items = new List<List<object>> {
                new List<object>{DBInsertPointName.ALIAS, DBInsertPointName_AT.alias, model.alias},
                new List<object>{DBInsertPointName.NAME, DBInsertPointName_AT.value_name, model.name},
                new List<object>{DBInsertPointName.HANDLE, DBInsertPointName_AT.handle, model.handle},
                new List<object>{DBInsertPointName.POSITION_ID, DBInsertPointName_AT.position, model.position.ID},
                new List<object>{DBInsertPointName.MATRIX_ID, DBInsertPointName_AT.matrix, model.matrixTransform.ID},
                new List<object>{DBInsertPointName.FILE_ID, DBInsertPointName_AT.file, model.file.ID},
            };

            Dictionary<string, string> variables = new Dictionary<string, string>();
            Dictionary<string, string> conditions = new Dictionary<string, string> { {DBInsertPointName.ID, DBInsertPointName_AT.id} };
            Dictionary<string, object> paraDict = new Dictionary<string, object>();

            foreach(List<object> item in items)
            {
                variables.Add((string)item[0], (string)item[1]);
                paraDict.Add((string)item[1], item[2]);
            }

            paraDict.Add(DBBlockName_AT.id, model.ID);

            DBCommand.UpdateRow(DBInsertPointName.tableName, variables, conditions, paraDict, command);
        }

        public static void InsertRow(SQLiteCommand command, InsertPointModel model)
        {
            List<List<object>> items = new List<List<object>> {
                new List<object>{DBInsertPointName.ALIAS, DBInsertPointName_AT.alias, model.alias},
                new List<object>{DBInsertPointName.NAME, DBInsertPointName_AT.value_name, model.name},
                new List<object>{DBInsertPointName.HANDLE, DBInsertPointName_AT.handle, model.handle},
                new List<object>{DBInsertPointName.POSITION_ID, DBInsertPointName_AT.position, model.position.ID},
                new List<object>{DBInsertPointName.MATRIX_ID, DBInsertPointName_AT.matrix, model.matrixTransform.ID},
                new List<object>{DBInsertPointName.FILE_ID, DBInsertPointName_AT.file, model.file.ID},
            };

            List<string> variables = new List<string>();
            Dictionary<string, object> paraDict = new Dictionary<string, object>();

            foreach(List<object> item in items)
            {
                variables.Add((string)item[0]);
                paraDict.Add((string)item[1], item[2]);
            }


            DBCommand.InsertCommand(DBInsertPointName.NAME, variables, paraDict, command);
        }

        public static void DeleteTable(SQLiteCommand command)
        {
            DBCommand.DeleteTable(DBInsertPointName.NAME, command);
        }
        public static void CreateTable(SQLiteCommand command){
            StringBuilder builder = new StringBuilder();
            builder.Append($"CREATE TABLE '{DBInsertPointName.tableName}'( ");
            builder.Append($"'{DBInsertPointName.ID}'    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, ");
            builder.Append($"'{DBInsertPointName.ALIAS}' TEXT, ");
            builder.Append($"'{DBInsertPointName.NAME}'  TEXT, ");
            builder.Append($"'{DBInsertPointName.HANDLE}'    TEXT NOT NULL,");
            builder.Append($"'{DBInsertPointName.POSITION_ID}'   INTEGER NOT NULL, ");
            builder.Append($"'{DBInsertPointName.MATRIX_ID}'  INTEGER NOT NULL, ");
            builder.Append($"'{DBInsertPointName.FILE_ID}'   INTEGER NOT NULL, ");
            builder.Append($"FOREIGN KEY('{DBInsertPointName.POSITION_ID}') REFERENCES '{DBPoint3DName.tableName}'('{DBPoint3DName.ID}') ON DELETE CASCADE, ");
            builder.Append($"FOREIGN KEY('{DBInsertPointName.FILE_ID}') REFERENCES '{DBDwgFileName.name}'('{DBDwgFileName.ID}') ON DELETE CASCADE, ");
            builder.Append($"FOREIGN KEY('{DBInsertPointName.MATRIX_ID}') REFERENCES '{DBMatrixName.name}'('{DBMatrixName.ID}') ON DELETE CASCADE ");
            builder.Append(");");

            command.CommandText = builder.ToString();
        }
    }

    class DBInsertPointName : DBBlockName {
        public const string ALIAS = "ALIAS";
        public const string NAME = "NAME";

        public const string tableName = "INSERT_POINT";
    }

    class DBInsertPointName_AT: DBBlockName_AT
    {
        //Value_Name: is a parameter attribute in block, NOT the name of Database Table
        public const string value_name = "@name";
        public const string alias = "@alias";
    }
}
