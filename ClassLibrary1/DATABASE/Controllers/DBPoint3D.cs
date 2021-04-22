using ClassLibrary1.DATABASE.Models;
using ClassLibrary1.HELPERS;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DATABASE
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
            string commandStr = DBPoint3DCommands.SelectRow(ID);
            using(SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = commandStr;
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
            string commandStr = DBPoint3DCommands.CreateTable();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = commandStr;
                command.ExecuteNonQuery();
            }
        }

        public static void DeleteTable(SQLiteConnection connection)
        {
            string commandStr = DBPoint3DCommands.DeleteTable();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = commandStr;
                command.ExecuteNonQuery();
            }

        }

        /// <summary>
        /// Insert a point to Table, return id the row inserted.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static long InsertRow(IList<double> point, SQLiteConnection connection)
        {
            string commandStr = DBPoint3DCommands.Insert(point[0], point[1], point[2]);
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = commandStr;
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
        public static long UpdateRow(IList<double> point, int ID, SQLiteConnection connection)
        {
            string commandStr = DBPoint3DCommands.Update(ID, point[0], point[1], point[2]);

            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = commandStr;
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

        public static long DeleteRow(int ID, SQLiteConnection connection)
        {
            string commandStr = DBPoint3DCommands.DeleteRow(ID);
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = commandStr;
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
        public static string SelectRow(long ID)
        {
            return string.Format("SELECT * FROM {0} WHERE '{1}' = {2};", DBPoint3DName.tableName, DBPoint3DName.ID, ID);
        }
        public static string DeleteTable()
        {
            return string.Format("DROP TABLE IF EXISTS '{0}';", DBPoint3DName.tableName);
        }

        public static string DeleteRow(int ID)
        {
            return string.Format("DELETE FROM '{0}' WHERE '{1}' = {2};", DBPoint3DName.tableName, DBPoint3DName.ID, ID);
        }
        public static string CreateTable()
        {
            return string.Format("CREATE TABLE IF NOT EXISTS '{0}'('{1}' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, '{2}' REAL NOT NULL, '{3}' REAL NOT NULL, '{4}' REAL NOT NULL));",
                                    DBPoint3DName.tableName,
                                    DBPoint3DName.ID,
                                    DBPoint3DName.X,
                                    DBPoint3DName.Y,
                                    DBPoint3DName.Z
                                    );
        }


        public static string Insert(double X, double Y, double Z)
        {
            //"INSERT INTO POINT3D ('X', 'Y', 'Z') VALUES (034.34, 233, 0);"
            return string.Format("INSERT INTO '{0}' ('{1}', '{2}', '{3}') VALUES ({4}, {5}, {6});",
                                    DBPoint3DName.tableName,
                                    DBPoint3DName.X,
                                    DBPoint3DName.Y,
                                    DBPoint3DName.Z,
                                    X,Y,Z
                                    );
        }



        //UPDATE POINT3D SET 'X' =1, 'Y' = 2, 'Z' = 3 WHERE ID = 3;
        public static string Update(int ID, double X, double Y, double Z)
        {
            return string.Format("UPDATE {0} SET '{1}' = {2}, '{3}' = {4}, '{5}' = {6} WHERE {7} = {8};",
                DBPoint3DName.tableName,
                DBPoint3DName.X, X,
                DBPoint3DName.Y, Y,
                DBPoint3DName.Z, Z,
                DBPoint3DName.ID, ID
                );
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
