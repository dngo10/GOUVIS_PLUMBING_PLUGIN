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
        public static Point3dModel SelectRow(SQLiteConnection connection, long ID)
        {
            Point3dModel point = null;
            using(SQLiteCommand command = connection.CreateCommand())
            {
                DBPoint3DCommands.SelectRow(command, ID);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    double X = (double)reader[DBPoint3DName.X];
                    double Y = (double)reader[DBPoint3DName.Y];
                    double Z = (double)reader[DBPoint3DName.Z];
                    point = new Point3dModel(X, Y, Z, ID);
                }
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
        /// </summary>
        /// <param name="point"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static long InsertRow(Point3dModel model, SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBPoint3DCommands.Insert(command, model);
                int check = command.ExecuteNonQuery();
                if (check == 1)
                {
                    return connection.LastInsertRowId;
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
                    throw new Exception("DBPoint3d -> UpdateRow -> No Row is Updated.");
                }
                throw new Exception("DBPoint3D -> UpdateRow -> Update Point not successful.");
            }

        }

        public static long DeleteRow(long ID, SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
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
        }
    }

    class DBPoint3DCommands
    {
        public static void SelectRow(SQLiteCommand command, long ID)
        {
            string commandStr = string.Format("SELECT * FROM {0} WHERE {1} = @ID;", DBPoint3DName.tableName, DBPoint3DName.ID);
            command.CommandText = commandStr;
            command.Parameters.Add(new SQLiteParameter("@ID", ID));
        }
        public static void DeleteTable(SQLiteCommand command)
        {
            string commandStr = string.Format("DROP TABLE IF EXISTS {0};", DBPoint3DName.tableName);
            command.CommandText = commandStr;
        }

        public static void DeleteRow(SQLiteCommand command, long ID)
        {
            string commandStr = string.Format("DELETE FROM {0} WHERE {1} = @ID;", DBPoint3DName.tableName, DBPoint3DName.ID);
            command.CommandText = commandStr;
            command.Parameters.Add(new SQLiteParameter("@ID", ID));
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
            string commandStr = string.Format("INSERT INTO '{0}' ('{1}', '{2}', '{3}') VALUES (@X, @Y, @Z);",
                                    DBPoint3DName.tableName,
                                    DBPoint3DName.X,
                                    DBPoint3DName.Y,
                                    DBPoint3DName.Z
                                    );
            command.CommandText = commandStr;
            command.Parameters.Add(new SQLiteParameter("@X", model.X));
            command.Parameters.Add(new SQLiteParameter("@Y", model.Y));
            command.Parameters.Add(new SQLiteParameter("@Z", model.Z));
        }



        //UPDATE POINT3D SET 'X' =1, 'Y' = 2, 'Z' = 3 WHERE ID = 3;
        public static void Update(SQLiteCommand command, Point3dModel model)
        {
            string commandStr = string.Format("UPDATE {0} SET '{1}' = @X, '{2}' = @Y, '{3}' = @Z WHERE {4} = @ID;",
                DBPoint3DName.tableName,
                DBPoint3DName.X,
                DBPoint3DName.Y,
                DBPoint3DName.Z,
                DBPoint3DName.ID
                );

            command.CommandText = commandStr;
            command.Parameters.Add(new SQLiteParameter("@X", model.X));
            command.Parameters.Add(new SQLiteParameter("@Y", model.Y));
            command.Parameters.Add(new SQLiteParameter("@Z", model.Z));
            command.Parameters.Add(new SQLiteParameter("@ID", model.ID));
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
}
