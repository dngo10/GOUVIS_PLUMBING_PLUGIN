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
	"HANDLE"	TEXT NOT NULL,
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
		public static FixtureDetailsModel SelectRow(SQLiteConnection connection, long ID)
        {
			string commandStr = DBFixtureDetailsCommands.SelectRow(ID);
			return GetFixtureDetail(connection, commandStr);

        }
		public static FixtureDetailsModel SelectRow(SQLiteConnection connection, string handle)
        {
			string commandStr = DBFixtureDetailsCommands.SelectRow(handle);
			return GetFixtureDetail(connection, commandStr);
        }

		private static FixtureDetailsModel GetFixtureDetail(SQLiteConnection connection, string commandStr)
        {
			FixtureDetailsModel model = null;
			using(SQLiteCommand command = connection.CreateCommand())
            {
				command.CommandText = commandStr;
				SQLiteDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					long POSITION_ID = (long)reader[DBFixtureDetailsNames.POSITION_ID];
					long TRANSFORM_ID = (long)reader[DBFixtureDetailsNames.TRANSFORM_ID];
					string HANDLE = (string)reader[DBFixtureDetailsNames.HANDLE];
					double INDEX = (double)reader[DBFixtureDetailsNames.INDEXX];
					string FIXTURE_NAME = (string)reader[DBFixtureDetailsNames.FIXTURE_NAME];
					string TAG = (string)reader[DBFixtureDetailsNames.TAG];
					string NUMBER = (string)reader[DBFixtureDetailsNames.NUMBER];
					double CW_DIA = (double)reader[DBFixtureDetailsNames.CW_DIA];
					double HW_DIA = (double)reader[DBFixtureDetailsNames.HW_DIA];
					double WASTE_DIA = (double)reader[DBFixtureDetailsNames.WASTE_DIA];
					double VENT_DIA = (double)reader[DBFixtureDetailsNames.VENT_DIA];
					double STORM_DIA = (double)reader[DBFixtureDetailsNames.STORM_DIA];
					double WSFU = (double)reader[DBFixtureDetailsNames.WSFU];
					double CWSFU = (double)reader[DBFixtureDetailsNames.CWSFU];
					double HWSFU = (double)reader[DBFixtureDetailsNames.HWSFU];
					double DFU = (double)reader[DBFixtureDetailsNames.DFU];
					string DESCRIPTION = (string)reader[DBFixtureDetailsNames.DESCRIPTION];

					Matrix3dModel matrix = DBMatrix3d.SelectRow(connection, TRANSFORM_ID);
					Point3dModel position = DBPoint3D.SelectRow(connection, POSITION_ID);

					model = new FixtureDetailsModel();
					model.position = position;
					model.matrixTransform = matrix;
					model.handle = HANDLE;
					model.INDEX = INDEX;
					model.FIXTURENAME = FIXTURE_NAME;
					model.TAG = TAG;
					model.NUMBER = NUMBER;
					model.CW_DIA = CW_DIA;
					model.HW_DIA = HW_DIA;
					model.WASTE_DIA = WASTE_DIA;
					model.VENT_DIA = VENT_DIA;
					model.STORM_DIA = STORM_DIA;
					model.WSFU = WSFU;
					model.CWSFU = CWSFU;
					model.HWSFU = HWSFU;
					model.DFU = DFU;
					model.DESCRIPTION = DESCRIPTION;
				}
			}
			return model;
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
			StringBuilder builder = new StringBuilder();
			builder.Append(string.Format($"CREATE TABLE '{DBFixtureDetailsNames.name}' ("));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.ID}' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.POSITION_ID}' INTEGER NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.TRANSFORM_ID}' INTEGER NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.HANDLE}' TEXT NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.INDEXX}' TEXT NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.FIXTURE_NAME}' TEXT NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.TAG}' TEXT NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.NUMBER}' TEXT NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.CW_DIA}' REAL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.HW_DIA}' REAL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.WASTE_DIA}' NUMERIC, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.VENT_DIA}' NUMERIC, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.STORM_DIA}' NUMERIC, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.WSFU}' NUMERIC, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.CWSFU}' NUMERIC, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.HWSFU}' NUMERIC, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.DFU}' NUMERIC, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.DESCRIPTION}' TEXT, "));
			builder.Append(string.Format($"FOREIGN KEY('{DBFixtureDetailsNames.TRANSFORM_ID}') REFERENCES '{DBMatrixName.name}'('{DBMatrixName.ID}') ON DELETE CASCADE, "));
			builder.Append(string.Format($"FOREIGN KEY('{DBFixtureDetailsNames.POSITION_ID}') REFERENCES '{DBPoint3DName.tableName}'('{DBPoint3DName.ID}') ON DELETE CASCADE"));
			builder.Append(string.Format(");"));
			return builder.ToString();
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
