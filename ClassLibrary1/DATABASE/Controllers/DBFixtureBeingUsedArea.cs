using ClassLibrary1.DATABASE.Controllers.BlockInterFace;
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
        public static BlockGeneral<FixtureBeingUsedAreaModel> gBlock = new BlockGeneral<FixtureBeingUsedAreaModel>(GetModelFromReader, DBFixtureBeingUsedAreaName.name);
        public static List<FixtureBeingUsedAreaModel> SelectRows(SQLiteConnection connection, long fileID) { return gBlock.SelectRows(fileID, connection); }
        public static List<FixtureBeingUsedAreaModel> SelectRows(SQLiteConnection connection, string relPath) { return gBlock.SelectRows(relPath, connection); }
        public static List<FixtureBeingUsedAreaModel> SelectRows(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
        { return gBlock.SelectRows(conDict, paraDict, connection); }

        public static FixtureBeingUsedAreaModel SelectRow(SQLiteConnection connection, long ID) { return gBlock.SelectRow(ID, connection); }
        public static FixtureBeingUsedAreaModel SelectRow(SQLiteConnection connection, string handle, long file_ID) { return gBlock.SelectRow(handle, file_ID, connection); }
        public static FixtureBeingUsedAreaModel SelectRow(SQLiteConnection connection, string handle, string relPath) { return gBlock.SelectRow(handle, relPath, connection); }
        public static FixtureBeingUsedAreaModel SelectRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
        { return gBlock.SelectRow(conDict, paraDict, connection); }

        public static bool HasRow(SQLiteConnection connection, long ID) { return gBlock.HasRow(ID, connection); }
        public static bool HasRow(SQLiteConnection connection, string handle, long fileID) { return gBlock.HasRow(handle, fileID, connection); }
        public static bool HasRow(SQLiteConnection connection, string handle, string relPath) { return gBlock.HasRow(handle, relPath, connection); }
        public static bool HasRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
        { return gBlock.HasRow(conDict, paraDict, connection); }

        public static void DeleteRow(SQLiteConnection connection, long ID)
        {
            FixtureBeingUsedAreaModel model = SelectRow(connection, ID);
            gBlock.DeleteRow(ID, connection);
            DeleteOthers(model, connection);
        }

        public static void DeleteRow(SQLiteConnection connection, string handle, long fileID)
        {
            FixtureBeingUsedAreaModel model = SelectRow(connection, handle, fileID);
            gBlock.DeleteRow(handle, fileID, connection);
            DeleteOthers(model, connection);
        }
        public static void DeleteRow(SQLiteConnection connection, string handle, string relPath)
        {
            FixtureBeingUsedAreaModel model = SelectRow(connection, handle, relPath);
            gBlock.DeleteRow(handle, relPath, connection);
            DeleteOthers(model, connection);
        }
        public static void DeleteRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
        {
            FixtureBeingUsedAreaModel model = SelectRow(connection, conDict, paraDict);
            gBlock.DeleteRow(conDict, paraDict, connection);
            DeleteOthers(model, connection);
        }

        private static void DeleteOthers(FixtureBeingUsedAreaModel model, SQLiteConnection connection)
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


        private static FixtureBeingUsedAreaModel GetModelFromReader(SQLiteDataReader reader, SQLiteConnection connection)
        {
            FixtureBeingUsedAreaModel model = new FixtureBeingUsedAreaModel();
            model.ID = (long)reader[DBFixtureBeingUsedAreaName.ID];
            model.handle = (string)reader[DBFixtureBeingUsedAreaName.HANDLE];

            model.position = DBPoint3D.SelectRow(connection, (long)reader[DBFixtureBeingUsedAreaName.POSITION_ID]);
            model.pointTop = DBPoint3D.SelectRow(connection, (long)reader[DBFixtureBeingUsedAreaName.POINT_TOP_ID]);
            model.pointBottom = DBPoint3D.SelectRow(connection, (long)reader[DBFixtureBeingUsedAreaName.POINT_BOTTOM_ID]);
            model.origin = DBPoint3D.SelectRow(connection, (long)reader[DBFixtureBeingUsedAreaName.ORIGIN_ID]);

            model.matrixTransform = DBMatrix3d.SelectRow(connection, (long)reader[DBFixtureBeingUsedAreaName.MATRIX_ID]);
            model.X = (double)reader[DBFixtureBeingUsedAreaName.X];
            model.Y = (double)reader[DBFixtureBeingUsedAreaName.Y];
            model.file = DBDwgFile.SelectRow(connection, (long)reader[DBFixtureBeingUsedAreaName.FILE_ID]);

            if (model.ID == ConstantName.invalidNum)
            {
                model = null;
            }

            return model;
        }

        public static long UpdateRow(SQLiteConnection connection, FixtureBeingUsedAreaModel model)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBFixtureBeingUsedAreaCommands.UpdateRow(command, model);
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
        
        public static long InsertRow(SQLiteConnection connection, ref FixtureBeingUsedAreaModel model)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {

                DBFixtureBeingUsedAreaCommands.InsertRow(model, command);
                long check = command.ExecuteNonQuery();
                if(check == 1)
                {
                    model.ID = connection.LastInsertRowId;
                    return model.ID;
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

    }

    class DBFixtureBeingUsedAreaCommands
    {
        public static void UpdateRow(SQLiteCommand command, FixtureBeingUsedAreaModel model)
        {
            DBPoint3D.UpdateRow(model.pointTop, command.Connection);
            DBPoint3D.UpdateRow(model.pointBottom, command.Connection);
            DBPoint3D.UpdateRow(model.origin, command.Connection);
            DBPoint3D.UpdateRow(model.position, command.Connection);
            DBMatrix3d.Update(command.Connection, model.matrixTransform);

            List<List<object>> items = GetItems(model);

            Dictionary<string, string> variables = new Dictionary<string, string>();
            Dictionary<string, string> conDict = new Dictionary<string, string> { { DBFixtureBeingUsedAreaName.ID, DBFixtureBeingUsedAreaName_AT.id } };
            Dictionary<string, object> paraDict = new Dictionary<string, object>();

            foreach (List<object> item in items)
            {
                variables.Add((string)item[0], (string)item[1]);
                paraDict.Add((string)item[1], item[2]);
            }

            DBCommand.UpdateRow(DBFixtureBeingUsedAreaName.name, variables, conDict, paraDict, command);
        }
        public static void InsertRow(FixtureBeingUsedAreaModel model, SQLiteCommand command)
        {
            List<List<object>> items = GetItems(model);

            List<string> variables = new List<string>();
            Dictionary<string, object> paraDict = new Dictionary<string, object>();

            foreach (List<object> item in items)
            {
                variables.Add((string)item[0]);
                paraDict.Add((string)item[1], item[2]);
            }

            DBCommand.InsertCommand(DBFixtureBeingUsedAreaName.name, variables, paraDict, command);
        }

        private static List<List<object>> GetItems(FixtureBeingUsedAreaModel model)
        {
            List<List<object>> items = new List<List<object>> {
                new List<object>{DBFixtureBeingUsedAreaName.HANDLE, DBFixtureBeingUsedAreaName_AT.handle, model.handle },
                new List<object>{DBFixtureBeingUsedAreaName.POSITION_ID, DBFixtureBeingUsedAreaName_AT.position, model.position.ID },
                new List<object>{DBFixtureBeingUsedAreaName.X, DBFixtureBeingUsedAreaName_AT.x, model.X },
                new List<object>{DBFixtureBeingUsedAreaName.Y, DBFixtureBeingUsedAreaName_AT.y, model.Y },
                new List<object>{DBFixtureBeingUsedAreaName.ORIGIN_ID, DBFixtureBeingUsedAreaName_AT.origin, model.origin.ID },
                new List<object>{DBFixtureBeingUsedAreaName.POINT_TOP_ID, DBFixtureBeingUsedAreaName_AT.top, model.pointTop.ID },
                new List<object>{DBFixtureBeingUsedAreaName.POINT_BOTTOM_ID,DBFixtureBeingUsedAreaName_AT.bottom, model.pointBottom.ID },
                new List<object>{DBFixtureBeingUsedAreaName.MATRIX_ID, DBFixtureBeingUsedAreaName_AT.matrix, model.matrixTransform.ID },
                new List<object>{DBFixtureBeingUsedAreaName.FILE_ID, DBFixtureBeingUsedAreaName_AT.file, model.file.ID },
            };

            return items;
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
            builder.Append(string.Format("'{0}' INTEGER NOT NULL,", DBFixtureBeingUsedAreaName.MATRIX_ID));
            builder.Append(string.Format("'{0}' INTEGER NOT NULL,", DBFixtureBeingUsedAreaName.FILE_ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE,", DBFixtureBeingUsedAreaName.POINT_TOP_ID, DBPoint3DName.tableName, DBPoint3DName.ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE,", DBFixtureBeingUsedAreaName.ORIGIN_ID, DBPoint3DName.tableName, DBPoint3DName.ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE,", DBFixtureBeingUsedAreaName.POINT_BOTTOM_ID, DBPoint3DName.tableName, DBPoint3DName.ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE,", DBFixtureBeingUsedAreaName.POSITION_ID, DBPoint3DName.tableName, DBPoint3DName.ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE,", DBFixtureBeingUsedAreaName.FILE_ID, DBDwgFileName.name, DBDwgFileName.ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE ", DBFixtureBeingUsedAreaName.MATRIX_ID, DBMatrixName.name, DBMatrixName.ID)); 
            builder.Append(string.Format(");"));
            command.CommandText = builder.ToString();
        }
    }

    class DBFixtureBeingUsedAreaName :DBBlockName
    {
        //x and y are width and height of the XY dynamic dimension.

        public const string ORIGIN_ID = "ORIGIN_ID";
        public const string POINT_BOTTOM_ID = "POINT_BOTTOM_ID";
        public const string POINT_TOP_ID = "POINT_TOP_ID";
        public const string X = "X";
        public const string Y = "Y";
        public const string name = "FIXTURE_BEING_USED_AREA";

        //USED ONLY FOR BLOCKREF
        //public const string basePoint = "Origin";
    }

    class DBFixtureBeingUsedAreaName_AT: DBBlockName_AT
    {
        public const string origin = "@origin";
        public const string bottom = "@bottom";
        public const string top = "@top";
        public const string x = "@x";
        public const string y = "@y";
        //USED ONLY FOR BLOCKREF
        //public const string basePoint = "@origin";
    }
}
