using ClassLibrary1.DATABASE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DATABASE.Controllers
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
        public static c
    }

    class DBFixtureBeingUsedAreaCommands
    {
        public static string SelectRow(string handle)
        {
            return string.Format("SELECT * FROM '{0}' WHERE '{1}' = {2};", DBFixtureBeingUsedAreaName.name, DBFixtureBeingUsedAreaName.HANDLE, handle);
        }
        public static string SelectRow(long ID)
        {
            return string.Format("SELECT * FROM '{0}' WHERE '{1}' = {2}", DBFixtureBeingUsedAreaName.name, DBFixtureBeingUsedAreaName.ID, ID);
        }
        public static string DeleteRow(long ID)
        {
            return string.Format("DELETE FROM '{0}' WHERE '{1}' = {2};", DBFixtureBeingUsedAreaName.name, DBFixtureBeingUsedAreaName.ID, ID);
        }

        public static string DeleteRow(string handle)
        {
            return string.Format("DELETE FROM '{0}' WHERE '{1}' = {2};", DBFixtureBeingUsedAreaName.name, DBFixtureBeingUsedAreaName.HANDLE, handle);
        }
        public static string UpdateRow(FixtureBeingUsedAreaModel model)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("UPDATE {0} SET ", DBFixtureBeingUsedAreaName.name));
            builder.Append(string.Format("'{0}' = {1}", DBFixtureBeingUsedAreaName.HANDLE, model.handle));
            builder.Append(string.Format("'{0}' = {1}", DBFixtureBeingUsedAreaName.POSITION_ID, model.position.ID));
            builder.Append(string.Format("'{0}' = {1}", DBFixtureBeingUsedAreaName.X, model.X));
            builder.Append(string.Format("'{0}' = {1}", DBFixtureBeingUsedAreaName.Y, model.Y));
            builder.Append(string.Format("'{0}' = {1}", DBFixtureBeingUsedAreaName.ORIGIN_ID, model.origin.ID));
            builder.Append(string.Format("'{0}' = {1}", DBFixtureBeingUsedAreaName.POINT_BOTTOM_ID, model.pointBottom.ID));
            builder.Append(string.Format("'{0}' = {1}", DBFixtureBeingUsedAreaName.TRANSFORM_ID, model.matrixTransform.ID));

            return builder.ToString();
        }
        public static string InsertRow(FixtureBeingUsedAreaModel model)
        {
            return string.Format("INSERT INTO {0} ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}') VALUES ({9}, {10}, {11}, {12}, {13}, {14}, {15}, {16});",
                DBFixtureBeingUsedAreaName.name,
                DBFixtureBeingUsedAreaName.HANDLE,
                DBFixtureBeingUsedAreaName.POSITION_ID,
                DBFixtureBeingUsedAreaName.X,
                DBFixtureBeingUsedAreaName.Y,
                DBFixtureBeingUsedAreaName.ORIGIN_ID,
                DBFixtureBeingUsedAreaName.POINT_TOP_ID,
                DBFixtureBeingUsedAreaName.POINT_BOTTOM_ID,
                DBFixtureBeingUsedAreaName.TRANSFORM_ID,
                model.handle,
                model.position.ID,
                model.X,
                model.Y,
                model.origin.ID,
                model.pointTop.ID,
                model.pointBottom.ID,
                model.matrixTransform.ID
                );
        }
        public static string CreateTable()
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
            builder.Append(string.Format("'{0}' INTEGER NOT NULL,", DBFixtureBeingUsedAreaName.TRANSFORM_ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE,", DBFixtureBeingUsedAreaName.POINT_TOP_ID, DBPoint3DName.tableName, DBPoint3DName.ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE,", DBFixtureBeingUsedAreaName.ORIGIN_ID, DBPoint3DName.tableName, DBPoint3DName.ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE,", DBFixtureBeingUsedAreaName.POINT_BOTTOM_ID, DBPoint3DName.tableName, DBPoint3DName.ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE,", DBFixtureBeingUsedAreaName.POSITION_ID, DBPoint3DName.tableName, DBPoint3DName.ID));
            builder.Append(string.Format("FOREIGN KEY('{0}') REFERENCES '{1}'('{2}') ON DELETE CASCADE ", DBFixtureBeingUsedAreaName.TRANSFORM_ID, DBPoint3DName.tableName, DBPoint3DName.ID)); 
            builder.Append(string.Format(");"));
            return builder.ToString();
        }

        public static string DeleteTable()
        {
            return string.Format("DROP TABLE IF EXISTS '{0}';", DBFixtureBeingUsedAreaName.name);
        }
    }

    class DBFixtureBeingUsedAreaName
    {
        //x and y are width and height of the XY dynamic dimension.
        public const string ID = "ID";
        public const string HANDLE = "HANDLE";
        public const string POSITION_ID = "POSITION_ID";
        public const string ORIGIN_ID = "ORIGIN_ID";
        public const string POINT_BOTTOM_ID = "POINT_BOTTOM_ID";
        public const string POINT_TOP_ID = "POINT_TOP_ID";
        public const string TRANSFORM_ID = "TRANSFORM_ID";
        public const string X = "X";
        public const string Y = "Y";
        public const string name = "FIXTURE_BEING_USED_AREA";
    }
}
