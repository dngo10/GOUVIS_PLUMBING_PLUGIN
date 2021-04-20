using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DATABASE
{

	/*
CREATE TABLE "FIXTURE_DETAILS" (
	"ID"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	"POSITION_ID"	INTEGER NOT NULL,
	"TRANFORM_ID"	INTEGER NOT NULL,
	"HANDLE"	NUMERIC NOT NULL,
	"INDEX"	TEXT NOT NULL,
	"FIXTURE_NAME"	TEXT NOT NULL,
	"TAG"	TEXT NOT NULL,
	"NUMBER"	NUMERIC NOT NULL,
	"CW_DIA"	REAL,
	"HW_DIA"	REAL,
	"WASTE_DIA"	NUMERIC,
	"VENT_DIA"	NUMERIC,
	"STORM_DIA"	NUMERIC,
	"WSFU"	NUMERIC,
	"CWSFU"	NUMERIC,
	"HWSFU"	NUMERIC,
	"DFU"	NUMERIC,
	"DESCRIPTION"	TEXT,
	FOREIGN KEY("TRANFORM_ID") REFERENCES "MATRIX3D" ON DELETE CASCADE,
	FOREIGN KEY("POSITION_ID") REFERENCES "POINT3D" ON DELETE CASCADE
);
     
     */
	class DBFixtureDetails
    {
		public static string
    }

	class DBFixtureDetailsCommands
    {
		public static string InsertRow()
        {

        }
		public static string CreateTable()
        {
			string command = string.Format($"CREATE TABLE '{DBFixtureDetailsName.name}' (");
			command += string.Format($"'{DBFixtureDetailsName.ID}' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, ");
			command += string.Format($"'{DBFixtureDetailsName.POSITION_ID}' INTEGER NOT NULL, ");
			command += string.Format($"'{DBFixtureDetailsName.TRANFORM_ID}' INTEGER NOT NULL, ");
			command += string.Format($"'{DBFixtureDetailsName.HANDLE}' NUMERIC NOT NULL, ");
			command += string.Format($"'{DBFixtureDetailsName.INDEX}' TEXT NOT NULL, ");
			command += string.Format($"'{DBFixtureDetailsName.FIXTURE_NAME}' TEXT NOT NULL, ");
			command += string.Format($"'{DBFixtureDetailsName.TAG}' TEXT NOT NULL, ");
			command += string.Format($"'{DBFixtureDetailsName.NUMBER}' NUMERIC NOT NULL, ");
			command += string.Format($"'{DBFixtureDetailsName.CW_DIA}' REAL, ");
			command += string.Format($"'{DBFixtureDetailsName.HW_DIA}' REAL, ");
			command += string.Format($"'{DBFixtureDetailsName.WASTE_DIA}' NUMERIC, ");
			command += string.Format($"'{DBFixtureDetailsName.VENT_DIA}' NUMERIC, ");
			command += string.Format($"'{DBFixtureDetailsName.STORM_DIA}' NUMERIC, ");
			command += string.Format($"'{DBFixtureDetailsName.WSFU}' NUMERIC, ");
			command += string.Format($"'{DBFixtureDetailsName.CWSFU}' NUMERIC, ");
			command += string.Format($"'{DBFixtureDetailsName.HWSFU}' NUMERIC, ");
			command += string.Format($"'{DBFixtureDetailsName.DFU}' NUMERIC, ");
			command += string.Format($"'{DBFixtureDetailsName.DESCRIPTION}' TEXT, ");
			command += string.Format($"FOREIGN KEY('{DBFixtureDetailsName.TRANFORM_ID}') REFERENCES '{DBMatrixName.name}' ON DELETE CASCADE, ");
			command += string.Format($"FOREIGN KEY('{DBFixtureDetailsName.POSITION_ID}') REFERENCES '{DBPoint3DName.tableName}' ON DELETE CASCADE");
			command += string.Format(");");
			return command;
        }

		public static string DeleteTable()
        {
			return string.Format("DROP TABLE IF EXISTS {0};", DBFixtureDetailsName.name);
		}
    }

	class DBFixtureDetailsName
    {
		public static string name = "FIXTURE_DETAILS";
		public static string ID = "ID";
		public static string POSITION_ID = "POSITION_ID";
		public static string TRANFORM_ID = "TRANFORM_ID";
		public static string HANDLE = "HANDLE";
		public static string INDEX = "INDEX";
		public static string FIXTURE_NAME = "FIXTURE_NAME";
		public static string TAG = "TAG";
		public static string NUMBER = "NUMBER";
		public static string CW_DIA = "CW_DIA";
		public static string HW_DIA = "HW_DIA";
		public static string WASTE_DIA = "WASTE_DIA";
		public static string VENT_DIA = "VENT_DIA";
		public static string STORM_DIA = "STORM_DIA";
		public static string WSFU = "WSFU";
		public static string CWSFU = "CWSFU";
		public static string HWSFU = "HWSFU";
		public static string DFU = "DFU";
		public static string DESCRIPTION = "DESCRIPTION";
	}
}
