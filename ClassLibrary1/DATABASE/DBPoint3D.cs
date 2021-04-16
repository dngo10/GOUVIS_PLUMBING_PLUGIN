using Autodesk.AutoCAD.Geometry;
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
        public static void CreateTable()
        {

        }
        public static void Insert(Point3d point, SQLiteConnection connection)
        {
            string command = "INSERT INTO POINT3D ('X', 'Y', 'Z') VALUES (034.34, 233, 0);"
            return -1;
        }
    }

    class DBPoint3DCommands
    {
        string createTable = string.Format("CREATE TABLE IF NOT EXISTS '{0}'('{1}' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, '{2}' REAL NOT NULL, '{3}' REAL NOT NULL, '{4}' REAL NOT NULL));",
                                            DBPoint3DName.tableName,
                                            DBPoint3DName.ID,
                                            DBPoint3DName.X,
                                            DBPoint3DName.Y,
                                            DBPoint3DName.Z
                                            );

        public static string Insert(double X, double Y, double Z)
        {
            return string.Format("INSERT INTO '{0}' ('{1}', '{2}', '{3}') VALUES ({4}, {5}, {6});",
                                    DBPoint3DName.tableName,
                                    DBPoint3DName.X,
                                    DBPoint3DName.Y,
                                    DBPoint3DName.Z,
                                    X,Y,Z
                                    );
        }

        //UPDATE POINT3D SET 'X' =1, 'Y' = 2, 'Z' = 3 WHERE ID = 3;
        public static string Update(int id, double X, double Y, double Z)
        {
            return string.Format("UPDATE {0} SET '{1}' = {4}, '{2}' = {5}, '{3}' = {6} WHERE {7} = {8};",
                DBPoint3DName.tableName,
                DBPoint3DName.X,
                DBPoint3DName.Y,
                DBPoint3DName.Z,
                X,Y,Z,
                DBPoint3DName.ID,
                id
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
