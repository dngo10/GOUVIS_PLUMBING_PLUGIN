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
    /// <summary>
    /// This class is to access and modify database.
    /// </summary>
    /// 
    /*
    CREATE TABLE "POINT3D" (
	"ID"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	"X"	REAL NOT NULL,
	"Y"	REAL NOT NULL,
	"Z"	REAL NOT NULL
    )*/

    class DBPoint3D
    {
        public static BlockGeneral<Point3dModel> gBlock = new BlockGeneral<Point3dModel>(GetItem, DBPoint3DName.tableName);
        //public static List<Point3dModel> SelectRows(SQLiteConnection connection, long fileID) { return gBlock.SelectRows(fileID, connection); }
        //public static List<Point3dModel> SelectRows(SQLiteConnection connection, string relPath) { return gBlock.SelectRows(relPath, connection); }
        public static List<Point3dModel> SelectRows(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
        { return gBlock.SelectRows(conDict, paraDict, connection); }

        public static Point3dModel SelectRow(SQLiteConnection connection, long ID) { return gBlock.SelectRow(ID, connection); }
        //public static Point3dModel SelectRow(SQLiteConnection connection, string handle, long file_ID) { return gBlock.SelectRow(handle, file_ID, connection); }
        //public static Point3dModel SelectRow(SQLiteConnection connection, string handle, string relPath) { return gBlock.SelectRow(handle, relPath, connection); }
        public static Point3dModel SelectRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
        { return gBlock.SelectRow(conDict, paraDict, connection); }

        public static bool HasRow(SQLiteConnection connection, long ID) { return gBlock.HasRow(ID, connection); }
        //public static bool HasRow(SQLiteConnection connection, string handle, long fileID) { return gBlock.HasRow(handle, fileID, connection); }
        //public static bool HasRow(SQLiteConnection connection, string handle, string relPath) { return gBlock.HasRow(handle, relPath, connection); }
        public static bool HasRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
        { return gBlock.HasRow(conDict, paraDict, connection); }

        public static void DeleteRow(SQLiteConnection connection, long ID)
        {
            gBlock.DeleteRow(ID, connection);
        }

        //public static void DeleteRow(SQLiteConnection connection, string handle, long fileID)
        //{
        //    gBlock.DeleteRow(handle, fileID, connection);
        //}
        //public static void DeleteRow(SQLiteConnection connection, string handle, string relPath)
        //{
        //    gBlock.DeleteRow(handle, relPath, connection);
        //}
        public static void DeleteRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
        {
            gBlock.DeleteRow(conDict, paraDict, connection);
        }

        public static void DeleteTable(SQLiteConnection connection) { gBlock.DeleteTable(connection); }

        public static Point3dModel GetItem(SQLiteDataReader reader, SQLiteConnection connection)
        {

            long ID1 = (long)reader[DBPoint3DName.ID];
            double X = (double)reader[DBPoint3DName.X];
            double Y = (double)reader[DBPoint3DName.Y];
            double Z = (double)reader[DBPoint3DName.Z];

            return new Point3dModel(X, Y, Z, ID1);
        }

        /// <summary>
        /// Create table DBPoint3d Table
        /// </summary>
        /// <param name="connection"></param>
        public static void CreateTable(SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBPoint3DCommands.CreateTable(command);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Insert a point to Table, return id the row inserted.
        /// After inserting, ID will be added to model
        /// </summary>
        /// <param name="point"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static long InsertRow(ref Point3dModel model, SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBPoint3DCommands.Insert(command, model);
                int check = command.ExecuteNonQuery();
                if (check == 1)
                {
                    model.ID = connection.LastInsertRowId;
                    return model.ID;
                }
                else if (check == 0)
                {
                    throw new Exception("DBPoint3dCommands -> Insert -> No Point Was Inserted");
                }
                throw new Exception("DBPoint3D -> Insert -> Insert Point not successful.");
            }
        }
        public static long UpdateRow(Point3dModel model, SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBPoint3DCommands.Update(command, model);
                int check = command.ExecuteNonQuery();
                return check;
            }

        }
    }

    class DBPoint3DCommands
    {
        public static void CreateTable(SQLiteCommand command)
        {
            string commandStr = string.Format("CREATE TABLE IF NOT EXISTS '{0}'('{1}' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, '{2}' REAL NOT NULL, '{3}' REAL NOT NULL, '{4}' REAL NOT NULL);",
                                    DBPoint3DName.tableName,
                                    DBPoint3DName.ID,
                                    DBPoint3DName.X,
                                    DBPoint3DName.Y,
                                    DBPoint3DName.Z
                                    );
            command.CommandText = commandStr;
        }


        public static void Insert(SQLiteCommand command, Point3dModel model)
        {
            //"INSERT INTO POINT3D ('X', 'Y', 'Z') VALUES (034.34, 233, 0);"
            List<string> variables = new List<string> { DBPoint3DName.X, DBPoint3DName.Y, DBPoint3DName.Z };
            Dictionary<string, object> paraDict = new Dictionary<string, object> { {DBPoint3D_AT.x, model.X },
                                                                                  {DBPoint3D_AT.y, model.Y },
                                                                                  {DBPoint3D_AT.z, model.Z}
                                                                                };

            DBCommand.InsertCommand(DBPoint3DName.tableName, variables, paraDict, command);
        }



        //UPDATE POINT3D SET 'X' =1, 'Y' = 2, 'Z' = 3 WHERE ID = 3;
        public static void Update(SQLiteCommand command, Point3dModel model)
        {
            List<List<object>> itemList = new List<List<object>>
            {
                new List<object>{DBPoint3DName.X, DBPoint3D_AT.x, model.X},
                new List<object>{DBPoint3DName.Y, DBPoint3D_AT.y, model.Y},
                new List<object>{DBPoint3DName.Z, DBPoint3D_AT.z, model.Z}
            };

            Dictionary<string, string> variable = new Dictionary<string, string>();
            Dictionary<string, object> paraDict = new Dictionary<string, object>();
            Dictionary<string, string> conDict = new Dictionary<string, string> { { DBPoint3DName.ID, DBPoint3D_AT.id} };

            foreach(List<object> item in itemList)
            {
                variable.Add((string)item[0], (string)item[1]);
                paraDict.Add((string)item[1], item[2]);
            };

            paraDict.Add(DBPoint3D_AT.id, model.ID);
            DBCommand.UpdateRow(DBPoint3DName.tableName, variable, conDict, paraDict, command);
        }

    }

    class DBPoint3DName
    {
        //ID MUST BE THE SAME STRING AS DBBlockName's ID
        public static string tableName = "POINT3D";
        public static string ID = "ID";
        public static string X = "X";
        public static string Y = "Y";
        public static string Z = "Z";
    }

    class DBPoint3D_AT
    {
        //id MUST BE THE SAME STRING AS DBBlockName_AT's id
        public static string name = "@name";
        public static string id = "@id";
        public static string x = "@x";
        public static string y = "@y";
        public static string z = "@z";
    }
}
