using ClassLibrary1.DATABASE.Controllers;
using ClassLibrary1.DATABASE.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DATABASE.Controllers
{

	/*
CREATE TABLE "FIXTURE_DETAILS" (
	"ID"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	"POSITION_ID"	INTEGER NOT NULL,
	"TRANFORM_ID"	INTEGER NOT NULL,
	"HANDLE"	TEXT NOT NULL,
	"INDEXX"	TEXT NOT NULL,
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
	"FILE_ID"	INTEGER,
	"DESCRIPTION"	TEXT,
	FOREIGN KEY("TRANFORM_ID") REFERENCES "MATRIX3D" ON DELETE CASCADE,
	FOREIGN KEY("FILE_ID") REFERENCES "FILE"("ID") ON DELETE CASCADE,
	FOREIGN KEY("POSITION_ID") REFERENCES "POINT3D" ON DELETE CASCADE
);
     
     */
	class DBFixtureDetails
    {
		public static FixtureDetailsModel SelectRow(SQLiteConnection connection, long ID)
        {
			using (SQLiteCommand command = connection.CreateCommand())
			{
				DBFixtureDetailsCommands.SelectRow(command, ID);
				return GetFixtureDetail(command);
			}
		}
		public static FixtureDetailsModel SelectRow(SQLiteConnection connection, string handle)
        {
			using(SQLiteCommand command = connection.CreateCommand())
            {
				DBFixtureDetailsCommands.SelectRow(command, handle);
				return GetFixtureDetail(command);
			}
			
        }

		private static FixtureDetailsModel GetFixtureDetail(SQLiteCommand command)
        {
			FixtureDetailsModel model = null;
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

				Matrix3dModel matrix = DBMatrix3d.SelectRow(command.Connection, TRANSFORM_ID);
				Point3dModel position = DBPoint3D.SelectRow(command.Connection, POSITION_ID);

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
			return model;
		} 
		public static long DeleteRow(SQLiteConnection connection, FixtureDetailsModel fixture)
        {
			using(SQLiteCommand command = connection.CreateCommand())
            {
				DBFixtureDetailsCommands.DeleteRow(command, fixture.ID);
				long check = command.ExecuteNonQuery();
				if(check == 1)
                {
					DBPoint3D.DeleteRow(fixture.position.ID, command.Connection);
					DBMatrix3d.DeleteRow(command.Connection, fixture.matrixTransform.ID);
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
		public static long UpdateRow(SQLiteConnection connection, FixtureDetailsModel model)
        {
			long index;
			using (SQLiteCommand command = connection.CreateCommand())
			{
				DBFixtureDetailsCommands.UpdateRow(model, command);
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
		public static long InsertRow(SQLiteConnection connection, FixtureDetailsModel model)
        {
			long index;
			using(SQLiteCommand command = connection.CreateCommand())
            {
				DBFixtureDetailsCommands.InsertRow(model, command);
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
				DBFixtureDetailsCommands.CreateTable(command);
				command.ExecuteNonQuery();
            }
        }
		public static void DeleteTable(SQLiteConnection connection)
        {
			using(SQLiteCommand command = connection.CreateCommand())
            {
				DBFixtureDetailsCommands.DeleteTable(command);
				command.ExecuteNonQuery();
            }
        }
    }

	class DBFixtureDetailsCommands
    {
		public static void SelectRow(SQLiteCommand command, long ID)
        {
			string commandStr = string.Format("SELECT * FROM {0} WHERE '{1}' = @id;", DBFixtureDetailsNames.name, DBFixtureDetailsNames.ID);
			command.CommandText = commandStr;
			command.Parameters.Add(new SQLiteParameter("@id", ID));
        }
		public static void SelectRow(SQLiteCommand command, string handle)
        {
			string commandStr = string.Format("SELECT * FROM {0} WHERE '{1}' = @handle;", DBFixtureDetailsNames.name, DBFixtureDetailsNames.HANDLE);
			command.CommandText = commandStr;
			command.Parameters.Add(new SQLiteParameter("@handle", handle));
        }
		public static void DeleteRow(SQLiteCommand command, long ID)
        {
			string commandStr = string.Format("DELETE FROM '{0}' WHERE '{1}' = @id;", DBFixtureDetailsNames.name, DBFixtureDetailsNames.ID);
			command.CommandText = commandStr;
			command.Parameters.Add(new SQLiteParameter("@id", ID));
        }
		public static void UpdateRow(FixtureDetailsModel model, SQLiteCommand command)
        {
			StringBuilder sb = new StringBuilder();
			sb.Append(string.Format("UPDATE {0} SET ", DBFixtureDetailsNames.name));
			sb.Append(string.Format("'{0}' = @position ,", DBFixtureDetailsNames.POSITION_ID));
			sb.Append(string.Format("'{0}' = @matrix ,", DBFixtureDetailsNames.TRANSFORM_ID));
			sb.Append(string.Format("'{0}' = @handle ,", DBFixtureDetailsNames.HANDLE));
			sb.Append(string.Format("'{0}' = @indexx ,", DBFixtureDetailsNames.INDEXX));
			sb.Append(string.Format("'{0}' = @fixture_name ,", DBFixtureDetailsNames.FIXTURE_NAME));
			sb.Append(string.Format("'{0}' = @tag ,", DBFixtureDetailsNames.TAG));
			sb.Append(string.Format("'{0}' = @number ,", DBFixtureDetailsNames.NUMBER));
			sb.Append(string.Format("'{0}' = @cw_dia ,", DBFixtureDetailsNames.CW_DIA));
			sb.Append(string.Format("'{0}' = @hw_dia ,", DBFixtureDetailsNames.HW_DIA));
			sb.Append(string.Format("'{0}' = @waste_dia ,", DBFixtureDetailsNames.WASTE_DIA));
			sb.Append(string.Format("'{0}' = @vent_dia ,", DBFixtureDetailsNames.VENT_DIA));
			sb.Append(string.Format("'{0}' = @storm_dia ,", DBFixtureDetailsNames.STORM_DIA));
			sb.Append(string.Format("'{0}' = @wsfu ,", DBFixtureDetailsNames.WSFU));
			sb.Append(string.Format("'{0}' = @cwsfu ,", DBFixtureDetailsNames.CWSFU));
			sb.Append(string.Format("'{0}' = @hwsfu ,", DBFixtureDetailsNames.CWSFU));
			sb.Append(string.Format("'{0}' = @dfu ,", DBFixtureDetailsNames.DFU));
			sb.Append(string.Format("'{0}' = @file ,", DBFixtureDetailsNames.FILE_ID));
			sb.Append(string.Format("'{0}' = @desc WHERE ", DBFixtureDetailsNames.DESCRIPTION));
			sb.Append(string.Format("'{0}' = @id;", DBFixtureDetailsNames.ID));

			command.CommandText = sb.ToString();
			command.Parameters.Add(new SQLiteParameter("@position", model.position.ID));
			command.Parameters.Add(new SQLiteParameter("@matrix", model.matrixTransform.ID));
			command.Parameters.Add(new SQLiteParameter("@handle", model.handle));
			command.Parameters.Add(new SQLiteParameter("@indexx", model.INDEX));
			command.Parameters.Add(new SQLiteParameter("@fixture_name", model.FIXTURENAME));
			command.Parameters.Add(new SQLiteParameter("@tag", model.TAG));
			command.Parameters.Add(new SQLiteParameter("@number", model.NUMBER));
			command.Parameters.Add(new SQLiteParameter("@cw_dia", model.CW_DIA));
			command.Parameters.Add(new SQLiteParameter("@hw_dia", model.HW_DIA));
			command.Parameters.Add(new SQLiteParameter("@waste_dia", model.WASTE_DIA));
			command.Parameters.Add(new SQLiteParameter("@vent_dia", model.VENT_DIA));
			command.Parameters.Add(new SQLiteParameter("@storm_dia", model.STORM_DIA));
			command.Parameters.Add(new SQLiteParameter("@wsfu", model.WSFU));
			command.Parameters.Add(new SQLiteParameter("@cwsfu", model.CWSFU));
			command.Parameters.Add(new SQLiteParameter("@hwsfu", model.HWSFU));
			command.Parameters.Add(new SQLiteParameter("@dfu", model.DFU));
			command.Parameters.Add(new SQLiteParameter("@desc", model.DESCRIPTION));
			command.Parameters.Add(new SQLiteParameter("@file", model.file.ID));
			command.Parameters.Add(new SQLiteParameter("@id", model.ID));
		}
		public static void InsertRow(FixtureDetailsModel model, SQLiteCommand command)
        {
			string commandStr = string.Format("INSERT INTO '{0}' ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}') VALUES ",
				DBFixtureDetailsNames.name, DBFixtureDetailsNames.POSITION_ID, DBFixtureDetailsNames.TRANSFORM_ID, DBFixtureDetailsNames.HANDLE, DBFixtureDetailsNames.INDEXX, DBFixtureDetailsNames.FIXTURE_NAME, DBFixtureDetailsNames.TAG,
				DBFixtureDetailsNames.NUMBER, DBFixtureDetailsNames.CW_DIA, DBFixtureDetailsNames.HW_DIA, DBFixtureDetailsNames.WASTE_DIA, DBFixtureDetailsNames.VENT_DIA, DBFixtureDetailsNames.STORM_DIA, DBFixtureDetailsNames.WSFU, DBFixtureDetailsNames.CWSFU,
				DBFixtureDetailsNames.HWSFU, DBFixtureDetailsNames.DFU, DBFixtureDetailsNames.DESCRIPTION, DBFixtureDetailsNames.FILE_ID);

			commandStr += string.Format("(@position, @matrix ,@handle ,@indexx ,@fixtureName ,@tag , @number ,@cw_dia ,@hw_dia ,@waste_dia ,@vent_dia ,@storm_dia ,@wsfu ,@cwsfu ,@hwsfu , @dfu, @desc, @file);");

			command.CommandText = commandStr;
			command.Parameters.Add(new SQLiteParameter("@position", model.position.ID));
			command.Parameters.Add(new SQLiteParameter("@matrix", model.matrixTransform.ID));
			command.Parameters.Add(new SQLiteParameter("@handle", model.handle));
			command.Parameters.Add(new SQLiteParameter("@indexx", model.INDEX));
			command.Parameters.Add(new SQLiteParameter("@fixtureName", model.FIXTURENAME));
			command.Parameters.Add(new SQLiteParameter("@tag", model.TAG));
			command.Parameters.Add(new SQLiteParameter("@number", model.NUMBER));
			command.Parameters.Add(new SQLiteParameter("@cw_dia", model.CW_DIA));
			command.Parameters.Add(new SQLiteParameter("@hw_dia", model.HW_DIA));
			command.Parameters.Add(new SQLiteParameter("@waste_dia", model.WASTE_DIA));
			command.Parameters.Add(new SQLiteParameter("@vent_dia", model.VENT_DIA));
			command.Parameters.Add(new SQLiteParameter("@storm_dia", model.STORM_DIA));
			command.Parameters.Add(new SQLiteParameter("@wsfu", model.WSFU));
			command.Parameters.Add(new SQLiteParameter("@cwsfu", model.CWSFU));
			command.Parameters.Add(new SQLiteParameter("@hwsfu", model.HWSFU));
			command.Parameters.Add(new SQLiteParameter("@dfu", model.DFU));
			command.Parameters.Add(new SQLiteParameter("@desc", model.DESCRIPTION));
			command.Parameters.Add(new SQLiteParameter("@file", model.file.ID));
		}
		public static void CreateTable(SQLiteCommand command)
        {
			StringBuilder builder = new StringBuilder();
			builder.Append(string.Format($"CREATE TABLE IF NOT EXISTS '{DBFixtureDetailsNames.name}' ("));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.ID}' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.POSITION_ID}' INTEGER NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.TRANSFORM_ID}' INTEGER NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.HANDLE}' TEXT NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.INDEXX}' INTEGER NOT NULL, "));
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
			builder.Append(string.Format($"'{DBFixtureDetailsNames.FILE_ID}' INTEGER NOT NULL, "));
			builder.Append(string.Format($"FOREIGN KEY('{DBFixtureDetailsNames.TRANSFORM_ID}') REFERENCES '{DBMatrixName.name}'('{DBMatrixName.ID}') ON DELETE CASCADE, "));
			builder.Append(string.Format($"FOREIGN KEY('{DBFixtureDetailsNames.FILE_ID}') REFERENCES '{DBMatrixName.name}'('{DBDwgFileName.ID}') ON DELETE CASCADE, "));
			builder.Append(string.Format($"FOREIGN KEY('{DBFixtureDetailsNames.POSITION_ID}') REFERENCES '{DBPoint3DName.tableName}'('{DBPoint3DName.ID}') ON DELETE CASCADE"));
			builder.Append(string.Format(");"));
			command.CommandText = builder.ToString();
        }

		public static void DeleteTable(SQLiteCommand command)
        {
			string commandStr = string.Format("DROP TABLE IF EXISTS {0};", DBFixtureDetailsNames.name);
			command.CommandText = commandStr;
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
		public static string FILE_ID = "FILE_ID";
	}
}
