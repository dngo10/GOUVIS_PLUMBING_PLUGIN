using ClassLibrary1.DATABASE.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
		public static FixtureDetailsModel SelectRow(SQLiteConnection connection, string handle)
        {
			string commandStr = DBFixtureDetailsCommands.SelectRow(handle);
			using(SQLiteCommand command = connection.CreateCommand())
            {
				command.CommandText = commandStr;
				SQLiteDataReader reader = command.ExecuteReader();
            }
        }
		public static long DeleteRow(SQLiteConnection connection, FixtureDetailsModel fixture)
        {
			string commandStr = DBFixtureDetailsCommands.DeleteRow(fixture.ID);
			using(SQLiteCommand command = connection.CreateCommand())
            {
				command.CommandText = commandStr;
				long check = command.ExecuteNonQuery();
				if(check == 1)
                {
					return connection.LastInsertRowId;
                }else if(check == 0)
                {
					//throw new Exception()
					return 0;
                }
                else
                {
					return -1;
                }
            }
        }
		public static long UpdateRow(SQLiteConnection connection, FixtureDetailsModel fixtureDetailsModel)
        {
			long index;
			string commandStr = DBFixtureDetailsCommands.UpdateRow(fixtureDetailsModel);
			using (SQLiteCommand command = connection.CreateCommand())
			{
				command.CommandText = commandStr;
				long check = command.ExecuteNonQuery();
				if (check == 1)
				{
					index = connection.LastInsertRowId;
					return index;
				}
				else if (check == 0)
				{
					throw new Exception("DBFixtureDetails -> UpdateRow -> No Row is Updated.");
				}
				throw new Exception("DBFixtureDetails -> UpdateRow -> Insert Not Successful.");
			}
		}
		public static long InsertRow(SQLiteConnection connection, FixtureDetailsModel fixtureDetailsModel)
        {
			long index;
			string commandStr = DBFixtureDetailsCommands.InsertRow(fixtureDetailsModel);
			using(SQLiteCommand command = connection.CreateCommand())
            {
				command.CommandText = commandStr;
				long check = command.ExecuteNonQuery();
				if(check == 1)
                {
					index = connection.LastInsertRowId;
					return index;
                }else if(check == 0)
                {
					throw new Exception("DBFixtureDetails -> InsertRow -> No Row is Inserted.");
                }
				throw new Exception("DBFixtureDetails -> InsertRow -> Insert Not Successful.");
            }
        }
		public static void CreateTable(SQLiteConnection connection)
        {
			using(SQLiteCommand command = connection.CreateCommand())
            {
				command.CommandText = DBFixtureDetailsCommands.CreateTable();
				command.ExecuteNonQuery();
            }
        }
		public static void DeleteTable(SQLiteConnection connection)
        {
			using(SQLiteCommand command = connection.CreateCommand())
            {
				command.CommandText = DBFixtureDetailsCommands.DeleteTable();
				command.ExecuteNonQuery();
            }
        }
    }

	class DBFixtureDetailsCommands
    {
		public static string SelectRow(long ID)
        {
			return string.Format("SELECT * FROM {0} WHERE '{1}' = {2};", DBFixtureDetailsNames.name, DBFixtureDetailsNames.ID, ID);
        }
		public static string SelectRow(string handle)
        {
			return string.Format("SELECT * FROM {0} WHERE '1' = {2};", DBFixtureDetailsNames.name, DBFixtureDetailsNames.HANDLE, handle);
        }
		public static string DeleteRow(long ID)
        {
			string command = string.Format("DELETE FROM '{0}' WHERE '{1}' = {2};", DBFixtureDetailsNames.name, DBFixtureDetailsNames.ID, ID);
			return command;
        }
		public static string UpdateRow(FixtureDetailsModel fixtureDetailsModel)
        {
			StringBuilder sb = new StringBuilder();
			sb.Append(string.Format("UPDATE {0} SET ", DBFixtureDetailsNames.name));
			sb.Append(string.Format("'{0}' = {1} ,", DBFixtureDetailsNames.POSITION_ID, fixtureDetailsModel.position.ID));
			sb.Append(string.Format("'{0}' = {1} ,", DBFixtureDetailsNames.TRANSFORM_ID, fixtureDetailsModel.matrixTransform.ID));
			sb.Append(string.Format("'{0}' = '{1}' ,", DBFixtureDetailsNames.HANDLE, fixtureDetailsModel.handle));
			sb.Append(string.Format("'{0}' = {1} ,", DBFixtureDetailsNames.INDEXX, fixtureDetailsModel.INDEX));
			sb.Append(string.Format("'{0}' = '{1}' ,", DBFixtureDetailsNames.FIXTURE_NAME, fixtureDetailsModel.FIXTURENAME));
			sb.Append(string.Format("'{0}' = '{1}' ,", DBFixtureDetailsNames.TAG, fixtureDetailsModel.TAG));
			sb.Append(string.Format("'{0}' = {1} ,", DBFixtureDetailsNames.NUMBER, fixtureDetailsModel.NUMBER));
			sb.Append(string.Format("'{0}' = {1} ,", DBFixtureDetailsNames.CW_DIA, fixtureDetailsModel.CW_DIA));
			sb.Append(string.Format("'{0}' = {1} ,", DBFixtureDetailsNames.HW_DIA, fixtureDetailsModel.HW_DIA));
			sb.Append(string.Format("'{0}' = {1} ,", DBFixtureDetailsNames.WASTE_DIA, fixtureDetailsModel.WASTE_DIA));
			sb.Append(string.Format("'{0}' = {1} ,", DBFixtureDetailsNames.VENT_DIA, fixtureDetailsModel.VENT_DIA));
			sb.Append(string.Format("'{0}' = {1} ,", DBFixtureDetailsNames.STORM_DIA, fixtureDetailsModel.STORM_DIA));
			sb.Append(string.Format("'{0}' = {1} ,", DBFixtureDetailsNames.WSFU, fixtureDetailsModel.WSFU));
			sb.Append(string.Format("'{0}' = {1} ,", DBFixtureDetailsNames.CWSFU, fixtureDetailsModel.CWSFU));
			sb.Append(string.Format("'{0}' = {1} ,", DBFixtureDetailsNames.CWSFU, fixtureDetailsModel.CWSFU));
			sb.Append(string.Format("'{0}' = {1} ,", DBFixtureDetailsNames.DFU, fixtureDetailsModel.DFU));
			sb.Append(string.Format("'{0}' = '{1}' WHERE ", DBFixtureDetailsNames.DESCRIPTION, fixtureDetailsModel.DESCRIPTION));
			sb.Append(string.Format("'{0}' = {1};", DBFixtureDetailsNames.ID, fixtureDetailsModel.ID));

			return sb.ToString();
		}
		public static string InsertRow(FixtureDetailsModel fixtureDetailsModel)
        {
			string command = string.Format("INSERT INTO '{0}' ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}') VALUES ",
				DBFixtureDetailsNames.name, DBFixtureDetailsNames.POSITION_ID, DBFixtureDetailsNames.TRANSFORM_ID, DBFixtureDetailsNames.HANDLE, DBFixtureDetailsNames.INDEXX, DBFixtureDetailsNames.FIXTURE_NAME, DBFixtureDetailsNames.TAG,
				DBFixtureDetailsNames.NUMBER, DBFixtureDetailsNames.CW_DIA, DBFixtureDetailsNames.HW_DIA, DBFixtureDetailsNames.WASTE_DIA, DBFixtureDetailsNames.VENT_DIA, DBFixtureDetailsNames.STORM_DIA, DBFixtureDetailsNames.WSFU, DBFixtureDetailsNames.CWSFU,
				DBFixtureDetailsNames.HWSFU, DBFixtureDetailsNames.DFU, DBFixtureDetailsNames.DESCRIPTION
				);
			command += string.Format("({0}, {1} ,'{2}' ,{3} ,'{4}' ,'{5}' , {6} ,{7} ,{8} ,{9} ,{10} ,{11} ,{12} ,{13} ,{14} , {15}, '{16}');",
				fixtureDetailsModel.position.ID, fixtureDetailsModel.matrixTransform.ID, fixtureDetailsModel.handle, fixtureDetailsModel.INDEX, 
				fixtureDetailsModel.FIXTURENAME, fixtureDetailsModel.TAG, fixtureDetailsModel.NUMBER, fixtureDetailsModel.CW_DIA, fixtureDetailsModel.HW_DIA, 
				fixtureDetailsModel.WASTE_DIA, fixtureDetailsModel.VENT_DIA, fixtureDetailsModel.STORM_DIA, fixtureDetailsModel.WSFU, fixtureDetailsModel.CWSFU,
				fixtureDetailsModel.HWSFU, fixtureDetailsModel.DFU, fixtureDetailsModel.DESCRIPTION
				);

			return command;
		}
		public static string CreateTable()
        {
			string command = string.Format($"CREATE TABLE '{DBFixtureDetailsNames.name}' (");
			command += string.Format($"'{DBFixtureDetailsNames.ID}' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, ");
			command += string.Format($"'{DBFixtureDetailsNames.POSITION_ID}' INTEGER NOT NULL, ");
			command += string.Format($"'{DBFixtureDetailsNames.TRANSFORM_ID}' INTEGER NOT NULL, ");
			command += string.Format($"'{DBFixtureDetailsNames.HANDLE}' TEXT NOT NULL, ");
			command += string.Format($"'{DBFixtureDetailsNames.INDEXX}' TEXT NOT NULL, ");
			command += string.Format($"'{DBFixtureDetailsNames.FIXTURE_NAME}' TEXT NOT NULL, ");
			command += string.Format($"'{DBFixtureDetailsNames.TAG}' TEXT NOT NULL, ");
			command += string.Format($"'{DBFixtureDetailsNames.NUMBER}' NUMERIC NOT NULL, ");
			command += string.Format($"'{DBFixtureDetailsNames.CW_DIA}' REAL, ");
			command += string.Format($"'{DBFixtureDetailsNames.HW_DIA}' REAL, ");
			command += string.Format($"'{DBFixtureDetailsNames.WASTE_DIA}' NUMERIC, ");
			command += string.Format($"'{DBFixtureDetailsNames.VENT_DIA}' NUMERIC, ");
			command += string.Format($"'{DBFixtureDetailsNames.STORM_DIA}' NUMERIC, ");
			command += string.Format($"'{DBFixtureDetailsNames.WSFU}' NUMERIC, ");
			command += string.Format($"'{DBFixtureDetailsNames.CWSFU}' NUMERIC, ");
			command += string.Format($"'{DBFixtureDetailsNames.HWSFU}' NUMERIC, ");
			command += string.Format($"'{DBFixtureDetailsNames.DFU}' NUMERIC, ");
			command += string.Format($"'{DBFixtureDetailsNames.DESCRIPTION}' TEXT, ");
			command += string.Format($"FOREIGN KEY('{DBFixtureDetailsNames.TRANSFORM_ID}') REFERENCES '{DBMatrixName.name}' ON DELETE CASCADE, ");
			command += string.Format($"FOREIGN KEY('{DBFixtureDetailsNames.POSITION_ID}') REFERENCES '{DBPoint3DName.tableName}' ON DELETE CASCADE");
			command += string.Format(");");
			return command;
        }

		public static string DeleteTable()
        {
			return string.Format("DROP TABLE IF EXISTS {0};", DBFixtureDetailsNames.name);
		}
    }

	class DBFixtureDetailsNames
    {
		public static string name = "FIXTURE_DETAILS";
		public static string ID = "ID";
		public static string POSITION_ID = "POSITION_ID";
		public static string TRANSFORM_ID = "TRANSFORM_ID";
		public static string HANDLE = "HANDLE";
		public static string INDEXX = "INDEXX";
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
