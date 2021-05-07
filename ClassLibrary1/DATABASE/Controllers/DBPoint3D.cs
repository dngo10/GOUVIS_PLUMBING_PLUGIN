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
        public static bool HasRow(SQLiteConnection connection, long ID)
        {
            long count = 0;
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBPoint3DCommands.SelectCount(command, ID);
                count = Convert.ToInt64(command.ExecuteScalar());
            }
            return count == 1;
        }
        public static Point3dModel SelectRow(SQLiteConnection connection, long ID)
        {
            Point3dModel point = null;
            using(SQLiteCommand command = connection.CreateCommand())
            {
                DBPoint3DCommands.SelectRow(command, ID);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    long ID1 = (long)reader[DBPoint3DName.ID];
                    double X = (double)reader[DBPoint3DName.X];
                    double Y = (double)reader[DBPoint3DName.Y];
                    double Z = (double)reader[DBPoint3DName.Z];
                    
                    point = new Point3dModel(X, Y, Z, ID1);
                }
                reader.Close();
            }
            return point;
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

        public static void DeleteTable(SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBPoint3DCommands.DeleteTable(command);
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
                if (check == 1)
                {
                    return connection.LastInsertRowId;
                }
                else if (check == 0)
                {
                    //throw new Exception("DBPoint3d -> UpdateRow -> No Row is Updated.");
                }
                throw new Exception("DBPoint3D -> UpdateRow -> Update Point not successful.");
            }

        }

        public static long DeleteRow(long ID, SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                if(DBPoint3D.HasRow(connection, ID))
                {
                    DBPoint3DCommands.DeleteRow(command, ID);
                    long check = command.ExecuteNonQuery();
                    if (check == 1)
                    {
                        return connection.LastInsertRowId;
                    }
                    else if (check == 0)
                    {
                        throw new Exception("DBPoint3D -> DeleteRow -> No Row is Deleted");
                    }
                    throw new Exception("DBPoint3d -> DeleteRow -> Delete Row not successful");
                }
                return ConstantName.invalidNum;
            }
        }
    }

    class DBPoint3DCommands
    {
        public static void SelectCount(SQLiteCommand command, long ID)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string> { { DBPoint3DName.ID, DBPoint3D_AT.id } };
            Dictionary<string, object> paraDict = new Dictionary<string, object> { { DBPoint3D_AT.id, ID } };
            DBCommand.SelectCount(DBPoint3DName.tableName, conDict, paraDict, command);
        }

        public static void SelectRow(SQLiteCommand command, long ID)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string> { {DBPoint3DName.ID, DBPoint3D_AT.id} };
            Dictionary<string, object> paraDict = new Dictionary<string, object> { {DBPoint3D_AT.id, ID} };
            DBCommand.SelectRow(DBPoint3DName.tableName, conDict, paraDict, command);
        }
        public static void DeleteTable(SQLiteCommand command)
        {
            DBCommand.DeleteTable(DBPoint3DName.tableName, command);
        }

        public static void DeleteRow(SQLiteCommand command, long ID)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string> { { DBPoint3DName.ID, DBPoint3D_AT.id } };
            Dictionary<string, object> paraDict = new Dictionary<string, object> { { DBPoint3D_AT.id, ID } };
            DBCommand.DeleteRow(DBPoint3DName.tableName, conDict, paraDict, command);
        }
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
        public static string tableName = "POINT3D";
        public static string ID = "ID";
        public static string X = "X";
        public static string Y = "Y";
        public static string Z = "Z";
    }

    class DBPoint3D_AT
    {
        public static string name = "@name";
        public static string id = "@id";
        public static string x = "@x";
        public static string y = "@y";
        public static string z = "@z";
    }
}
