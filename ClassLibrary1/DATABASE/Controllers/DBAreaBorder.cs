using ClassLibrary1.DATABASE.Controllers.BlockInterFace;
using ClassLibrary1.DATABASE.DBModels;
using GouvisPlumbingNew.DATABASE.Controllers;
using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DATABASE.Controllers
{
    class DBAreaBorder
    {
        public static BlockGeneral<AreaBorderModel> gBlock = new BlockGeneral<AreaBorderModel>(GetModelFromReader, DBAreaBorderNames.name);
        public static List<AreaBorderModel> SelectRows(SQLiteConnection connection, long fileID) { return gBlock.SelectRows(fileID, connection); }
        public static List<AreaBorderModel> SelectRows(SQLiteConnection connection, string relPath) { return gBlock.SelectRows(relPath, connection); }
        public static List<AreaBorderModel> SelectRows(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
        { return gBlock.SelectRows(conDict, paraDict, connection); }

        public static AreaBorderModel SelectRow(SQLiteConnection connection, long ID) { return gBlock.SelectRow(ID, connection); }
        public static AreaBorderModel SelectRow(SQLiteConnection connection, string handle, long file_ID) { return gBlock.SelectRow(handle, file_ID, connection); }
        public static AreaBorderModel SelectRow(SQLiteConnection connection, string handle, string relPath) { return gBlock.SelectRow(handle, relPath, connection); }
        public static AreaBorderModel SelectRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
        { return gBlock.SelectRow(conDict, paraDict, connection); }

        public static bool HasRow(SQLiteConnection connection, long ID) { return gBlock.HasRow(ID, connection); }
        public static bool HasRow(SQLiteConnection connection, string handle, long fileID) { return gBlock.HasRow(handle, fileID, connection); }
        public static bool HasRow(SQLiteConnection connection, string handle, string relPath) { return gBlock.HasRow(handle, relPath, connection); }
        public static bool HasRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
        { return gBlock.HasRow(conDict, paraDict, connection); }

        public static void DeleteRow(SQLiteConnection connection, long ID)
        {
            AreaBorderModel model = SelectRow(connection, ID);
            gBlock.DeleteRow(ID, connection);
            DeleteOthers(model, connection);
        }

        public static void DeleteRow(SQLiteConnection connection, string handle, long fileID)
        {
            AreaBorderModel model = SelectRow(connection, handle, fileID);
            gBlock.DeleteRow(handle, fileID, connection);
            DeleteOthers(model, connection);
        }
        public static void DeleteRow(SQLiteConnection connection, string handle, string relPath)
        {
            AreaBorderModel model = SelectRow(connection, handle, relPath);
            gBlock.DeleteRow(handle, relPath, connection);
            DeleteOthers(model, connection);
        }
        public static void DeleteRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
        {
            AreaBorderModel model = SelectRow(connection, conDict, paraDict);
            gBlock.DeleteRow(conDict, paraDict, connection);
            DeleteOthers(model, connection);
        }

        private static void DeleteOthers(AreaBorderModel model, SQLiteConnection connection)
        {
            if (model != null)
            {
                DBPoint3D.DeleteRow(connection, model.file.ID);
                DBMatrix3d.DeleteRow(connection, model.matrixTransform.ID);

                if (model.position != null) DBPoint3D.DeleteRow(connection, model.position.ID);
                if (model.origin != null) DBPoint3D.DeleteRow(connection, model.origin.ID);
                if (model.pointTop != null) DBPoint3D.DeleteRow(connection, model.pointTop.ID);
                if (model.pointBottom != null) DBPoint3D.DeleteRow(connection, model.pointBottom.ID);
                if (model.matrixTransform != null) DBMatrix3d.DeleteRow(connection, model.matrixTransform.ID);
            }
        }

        public static void DeleteTable(SQLiteConnection connection) { gBlock.DeleteTable(connection); }


        private static AreaBorderModel GetModelFromReader(SQLiteDataReader reader, SQLiteConnection connection)
        {
            AreaBorderModel model = new AreaBorderModel();
            model.ID = (long)reader[DBAreaBorderNames.ID];
            model.handle = (string)reader[DBAreaBorderNames.HANDLE];

            model.position = DBPoint3D.SelectRow(connection, (long)reader[DBAreaBorderNames.POSITION_ID]);
            model.pointTop = DBPoint3D.SelectRow(connection, (long)reader[DBAreaBorderNames.POINT_TOP_ID]);
            model.pointBottom = DBPoint3D.SelectRow(connection, (long)reader[DBAreaBorderNames.POINT_BOTTOM_ID]);
            model.origin = DBPoint3D.SelectRow(connection, (long)reader[DBAreaBorderNames.ORIGIN_ID]);

            model.matrixTransform = DBMatrix3d.SelectRow(connection, (long)reader[DBAreaBorderNames.MATRIX_ID]);
            model.X = (double)reader[DBAreaBorderNames.X];
            model.Y = (double)reader[DBAreaBorderNames.Y];
            model.file = DBDwgFile.SelectRow(connection, (long)reader[DBAreaBorderNames.FILE_ID]);

            if (model.ID == ConstantName.invalidNum)
            {
                model = null;
            }

            return model;
        }

        public static long UpdateRow(SQLiteConnection connection, AreaBorderModel model)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBAreaBorderCommands.UpdateRow(command, model);
                long check = command.ExecuteNonQuery();
                if (check == 1)
                {
                    model.ID = connection.LastInsertRowId;
                    return model.ID;
                }
                else if (check == 0)
                {
                    //throw new Exception("FixtureBeingUsedAreaModel -> UpdateRow -> No Row is Updated.");
                }
                throw new Exception("FixtureBeingUsedAreaModel -> UpdateRow -> Insert Not Successful.");
            }
        }

        public static long InsertRow(SQLiteConnection connection, ref AreaBorderModel model)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {

                DBAreaBorderCommands.InsertRow(model, command);
                long check = command.ExecuteNonQuery();
                if (check == 1)
                {
                    model.ID = connection.LastInsertRowId;
                    return model.ID;
                }
                else if (check == 0)
                {
                    throw new Exception("DBFixtureBeingUsed -> insertRow -> No Row is inserted.");
                }
                throw new Exception("DBFixtureBeingUsed -> insertRow -> Row insertion not successful.");
            }
        }
        public static void CreateTable(SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBAreaBorderCommands.CreateTable(command);
                command.ExecuteNonQuery();
            }
        }
    }

    class DBAreaBorderCommands
    {
        public static void UpdateRow(SQLiteCommand command, AreaBorderModel model)
        {
            List<List<object>> items = GetItems(model);

            Dictionary<string, string> variables = new Dictionary<string, string>();
            Dictionary<string, string> conDict = new Dictionary<string, string> { { DBAreaBorderNames.ID, DBAreaBorderNames_AT.id } };
            Dictionary<string, object> paraDict = new Dictionary<string, object>();

            foreach (List<object> item in items)
            {
                variables.Add((string)item[0], (string)item[1]);
                paraDict.Add((string)item[1], item[2]);
            }

            DBCommand.UpdateRow(DBAreaBorderNames.name, variables, conDict, paraDict, command);
            command.ExecuteNonQuery();
        }
        public static void InsertRow(AreaBorderModel model, SQLiteCommand command)
        {
            List<List<object>> items = GetItems(model);

            List<string> variables = new List<string>();
            Dictionary<string, object> paraDict = new Dictionary<string, object>();

            foreach (List<object> item in items)
            {
                variables.Add((string)item[0]);
                paraDict.Add((string)item[1], item[2]);
            }

            DBCommand.InsertCommand(DBAreaBorderNames.name, variables, paraDict, command);
        }

        private static List<List<object>> GetItems(AreaBorderModel model)
        {
            List<List<object>> items = new List<List<object>> {
                new List<object>{ DBAreaBorderNames.HANDLE, DBAreaBorderNames_AT.handle, model.handle },
                new List<object>{ DBAreaBorderNames.POSITION_ID, DBAreaBorderNames_AT.position, model.position.ID },
                new List<object>{ DBAreaBorderNames.X, DBAreaBorderNames_AT.x, model.X },
                new List<object>{ DBAreaBorderNames.Y, DBAreaBorderNames_AT.y, model.Y },
                new List<object>{ DBAreaBorderNames.ORIGIN_ID, DBAreaBorderNames_AT.origin, model.origin.ID },
                new List<object>{ DBAreaBorderNames.POINT_TOP_ID, DBAreaBorderNames_AT.top, model.pointTop.ID },
                new List<object>{ DBAreaBorderNames.POINT_BOTTOM_ID, DBAreaBorderNames_AT.bottom, model.pointBottom.ID },
                new List<object>{ DBAreaBorderNames.MATRIX_ID, DBAreaBorderNames_AT.matrix, model.matrixTransform.ID },
                new List<object>{ DBAreaBorderNames.FILE_ID, DBAreaBorderNames_AT.file, model.file.ID },
            };
            return items;
        }

        public static void CreateTable(SQLiteCommand command)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("CREATE TABLE '{0}'(", DBAreaBorderNames.name));
            builder.Append(string.Format("'{0}' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,", DBAreaBorderNames.ID));
            builder.Append(string.Format("'{0}' TEXT, ", DBAreaBorderNames.TYPE));
            builder.Append(string.Format("'{0}' TEXT, ", DBAreaBorderNames.ALIAS));
            builder.Append(string.Format("'{0}' TEXT NOT NULL,", DBAreaBorderNames.HANDLE));
            builder.Append(string.Format("'{0}' INTEGER NOT NULL,", DBAreaBorderNames.POSITION_ID));
            builder.Append(string.Format("'{0}' REAL NOT NULL,", DBAreaBorderNames.X));
            builder.Append(string.Format("'{0}' REAL NOT NULL,", DBAreaBorderNames.Y));
            builder.Append(string.Format("'{0}' INTEGER NOT NULL,", DBAreaBorderNames.ORIGIN_ID));
            builder.Append(string.Format("'{0}' INTEGER NOT NULL,", DBAreaBorderNames.POINT_TOP_ID));
            builder.Append(string.Format("'{0}' INTEGER NOT NULL,", DBAreaBorderNames.POINT_BOTTOM_ID));
            builder.Append(string.Format("'{0}' INTEGER NOT NULL,", DBAreaBorderNames.MATRIX_ID));
            builder.Append(string.Format("'{0}' INTEGER NOT NULL,", DBAreaBorderNames.FILE_ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE,", DBAreaBorderNames.POINT_TOP_ID, DBPoint3DName.tableName, DBPoint3DName.ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE,", DBAreaBorderNames.ORIGIN_ID, DBPoint3DName.tableName, DBPoint3DName.ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE,", DBAreaBorderNames.POINT_BOTTOM_ID, DBPoint3DName.tableName, DBPoint3DName.ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE,", DBAreaBorderNames.POSITION_ID, DBPoint3DName.tableName, DBPoint3DName.ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE,", DBAreaBorderNames.FILE_ID, DBDwgFileName.name, DBDwgFileName.ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE ", DBAreaBorderNames.MATRIX_ID, DBMatrixName.name, DBMatrixName.ID));
            builder.Append(string.Format(");"));
            command.CommandText = builder.ToString();
        }
    }

    class DBAreaBorderNames : DBBlockName
    {
        public const string TYPE = "TYPE";
        public const string ALIAS = "ALIAS";
        public const string ORIGIN_ID = "ORIGIN_ID";
        public const string POINT_BOTTOM_ID = "POINT_BOTTOM_ID";
        public const string POINT_TOP_ID = "POINT_TOP_ID";
        public const string X = "X";
        public const string Y = "Y";
        public const string name = "BORDER_AREA";
    }

    class DBAreaBorderNames_AT : DBBlockName_AT
    {
        public const string origin = "@origin";
        public const string bottom = "@bottom";
        public const string top = "@top";
        public const string x = "@x";
        public const string y = "@y";
    }
}
