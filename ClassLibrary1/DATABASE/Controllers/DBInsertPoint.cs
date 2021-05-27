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
        public static BlockGeneral<InsertPointModel> gBlock = new BlockGeneral<InsertPointModel>(GetModel, DBInsertPointName.name);
        public static List<InsertPointModel> SelectRows(SQLiteConnection connection, long fileID){return gBlock.SelectRows(fileID, connection);}
        public static List<InsertPointModel> SelectRows(SQLiteConnection connection, string relPath){return gBlock.SelectRows(relPath, connection); }
        public static List<InsertPointModel> SelectRows(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
        {return gBlock.SelectRows(conDict, paraDict, connection); }

        public static InsertPointModel SelectRow(SQLiteConnection connection, long ID) { return gBlock.SelectRow(ID, connection); }
        public static InsertPointModel SelectRow(SQLiteConnection connection, string handle, long file_ID) { return gBlock.SelectRow(handle, file_ID, connection); }
        public static InsertPointModel SelectRow(SQLiteConnection connection, string handle, string relPath) { return gBlock.SelectRow(handle, relPath, connection); }
        public static InsertPointModel SelectRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
        { return gBlock.SelectRow(conDict, paraDict, connection); }

        public static bool HasRow(SQLiteConnection connection, long ID) { return gBlock.HasRow(ID, connection); }
        public static bool HasRow(SQLiteConnection connection, string handle, long fileID) { return gBlock.HasRow(handle, fileID, connection); }
        public static bool HasRow(SQLiteConnection connection, string handle, string relPath) { return gBlock.HasRow(handle, relPath, connection); }
        public static bool HasRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
        { return gBlock.HasRow(conDict, paraDict, connection); }

        public static void DeleteRow(SQLiteConnection connection, long ID) {
            InsertPointModel model = SelectRow(connection, ID);
            gBlock.DeleteRow(ID, connection);
            DeleteOthers(model, connection);
        }
            
        public static void DeleteRow(SQLiteConnection connection, string handle, long fileID) {
            InsertPointModel model = SelectRow(connection, handle, fileID);
            gBlock.DeleteRow(handle, fileID, connection);
            DeleteOthers(model, connection);
        }
        public static void DeleteRow(SQLiteConnection connection, string handle, string relPath) {
            InsertPointModel model = SelectRow(connection, handle, relPath);
            gBlock.DeleteRow(handle, relPath, connection);
            DeleteOthers(model, connection);
        }
        public static void DeleteRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
        {
            InsertPointModel model = SelectRow(connection, conDict, paraDict);
            gBlock.DeleteRow(conDict, paraDict, connection);
            DeleteOthers(model, connection);
        }

        private static void DeleteOthers(InsertPointModel model, SQLiteConnection connection)
        {
            if(model != null)
            {
                DBPoint3D.DeleteRow(connection, model.position.ID);
                DBMatrix3d.DeleteRow(connection, model.matrixTransform.ID);
            }
        }

        public static void DeleteTable(SQLiteConnection connection){gBlock.DeleteTable(connection);}

        private static InsertPointModel GetModel (SQLiteDataReader reader, SQLiteConnection connection)
        {
            string alias = (string)reader[DBInsertPointName.ALIAS];
            string name = (string)reader[DBInsertPointName.ANAME];
            long ID = (long)reader[DBInsertPointName.ID];
            Point3dModel pos = DBPoint3D.SelectRow(connection, (long)reader[DBInsertPointName.POSITION_ID]);
            string handle1 = (string)reader[DBInsertPointName.HANDLE];
            DwgFileModel file = DBDwgFile.SelectRow(connection, (long)reader[DBInsertPointName.FILE_ID]);
            Matrix3dModel matrix = DBMatrix3d.SelectRow(connection, (long)reader[DBInsertPointName.MATRIX_ID]);

            InsertPointModel model = new InsertPointModel(alias, name, ID, file, handle1, pos, matrix);

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
        public static long UpdateRow(SQLiteConnection connection, InsertPointModel model)
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
    }

    class DBInsertPointCommands {

        public static void UpdateRow(SQLiteCommand command, InsertPointModel model)
        {
            DBPoint3D.UpdateRow(model.position, command.Connection);
            DBMatrix3d.Update(command.Connection, model.matrixTransform);
            
            List<List<object>> items = GetItemsList(model);

            Dictionary<string, string> variables = new Dictionary<string, string>();
            Dictionary<string, string> conditions = new Dictionary<string, string> { {DBInsertPointName.ID, DBInsertPointName_AT.id} };
            Dictionary<string, object> paraDict = new Dictionary<string, object>();

            foreach(List<object> item in items)
            {
                variables.Add((string)item[0], (string)item[1]);
                paraDict.Add((string)item[1], item[2]);
            }

            paraDict.Add(DBBlockName_AT.id, model.ID);
            DBCommand.UpdateRow(DBInsertPointName.name, variables, conditions, paraDict, command);
        }

        public static void InsertRow(SQLiteCommand command, InsertPointModel model)
        {
            List<List<object>> items = GetItemsList(model);

            List<string> variables = new List<string>();
            Dictionary<string, object> paraDict = new Dictionary<string, object>();

            foreach(List<object> item in items)
            {
                variables.Add((string)item[0]);
                paraDict.Add((string)item[1], item[2]);
            }


            DBCommand.InsertCommand(DBInsertPointName.name, variables, paraDict, command);
        }

        private static List<List<object>> GetItemsList(InsertPointModel model)
        {
            List<List<object>> items = new List<List<object>> {
                new List<object>{DBInsertPointName.ALIAS, DBInsertPointName_AT.alias, model.alias},
                new List<object>{DBInsertPointName.ANAME, DBInsertPointName_AT.value_name, model.name},
                new List<object>{DBInsertPointName.HANDLE, DBInsertPointName_AT.handle, model.handle},
                new List<object>{DBInsertPointName.POSITION_ID, DBInsertPointName_AT.position, model.position.ID},
                new List<object>{DBInsertPointName.MATRIX_ID, DBInsertPointName_AT.matrix, model.matrixTransform.ID},
                new List<object>{DBInsertPointName.FILE_ID, DBInsertPointName_AT.file, model.file.ID},
            };

            return items;
        }

        public static void CreateTable(SQLiteCommand command){
            StringBuilder builder = new StringBuilder();
            builder.Append($"CREATE TABLE '{DBInsertPointName.name}'( ");
            builder.Append($"'{DBInsertPointName.ID}'    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, ");
            builder.Append($"'{DBInsertPointName.ALIAS}' TEXT, ");
            builder.Append($"'{DBInsertPointName.ANAME}'  TEXT, ");
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
        public const string ANAME = "NAME";
        public const string name = "INSERT_POINT";
    }

    class DBInsertPointName_AT: DBBlockName_AT
    {
        //Value_Name: is a parameter attribute in block, NOT the name of Database Table
        public const string value_name = "@name";
        public const string alias = "@alias";
    }
}
