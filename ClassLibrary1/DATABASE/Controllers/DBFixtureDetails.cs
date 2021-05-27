using ClassLibrary1.DATABASE.Controllers.BlockInterFace;
using GouvisPlumbingNew.DATABASE.Controllers;
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
		public static BlockGeneral<FixtureDetailsModel> gBlock = new BlockGeneral<FixtureDetailsModel>(GetFixture, FixtureDetailsName.FixtureName);
		public static List<FixtureDetailsModel> SelectRows(SQLiteConnection connection, long fileID) { return gBlock.SelectRows(fileID, connection); }
		public static List<FixtureDetailsModel> SelectRows(SQLiteConnection connection, string relPath) { return gBlock.SelectRows(relPath, connection); }
		public static List<FixtureDetailsModel> SelectRows(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
		{ return gBlock.SelectRows(conDict, paraDict, connection); }

		public static FixtureDetailsModel SelectRow(SQLiteConnection connection, long ID) { return gBlock.SelectRow(ID, connection); }
		public static FixtureDetailsModel SelectRow(SQLiteConnection connection, string handle, long file_ID) { return gBlock.SelectRow(handle, file_ID, connection); }
		public static FixtureDetailsModel SelectRow(SQLiteConnection connection, string handle, string relPath) { return gBlock.SelectRow(handle, relPath, connection); }
		public static FixtureDetailsModel SelectRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
		{ return gBlock.SelectRow(conDict, paraDict, connection); }

		public static bool HasRow(SQLiteConnection connection, long ID) { return gBlock.HasRow(ID, connection); }
		public static bool HasRow(SQLiteConnection connection, string handle, long fileID) { return gBlock.HasRow(handle, fileID, connection); }
		public static bool HasRow(SQLiteConnection connection, string handle, string relPath) { return gBlock.HasRow(handle, relPath, connection); }
		public static bool HasRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
		{ return gBlock.HasRow(conDict, paraDict, connection); }

		public static void DeleteRow(SQLiteConnection connection, long ID)
		{
			FixtureDetailsModel model = SelectRow(connection, ID);
			gBlock.DeleteRow(ID, connection);
			DeleteOthers(model, connection);
		}

		public static void DeleteRow(SQLiteConnection connection, string handle, long fileID)
		{
			FixtureDetailsModel model = SelectRow(connection, handle, fileID);
			gBlock.DeleteRow(handle, fileID, connection);
			DeleteOthers(model, connection);
		}
		public static void DeleteRow(SQLiteConnection connection, string handle, string relPath)
		{
			FixtureDetailsModel model = SelectRow(connection, handle, relPath);
			gBlock.DeleteRow(handle, relPath, connection);
			DeleteOthers(model, connection);
		}
		public static void DeleteRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
		{
			FixtureDetailsModel model = SelectRow(connection, conDict, paraDict);
			gBlock.DeleteRow(conDict, paraDict, connection);
			DeleteOthers(model, connection);
		}

		private static void DeleteOthers(FixtureDetailsModel model, SQLiteConnection connection)
		{
			if (model != null)
			{
				DBPoint3D.DeleteRow(connection, model.file.ID);
				DBMatrix3d.DeleteRow(connection, model.matrixTransform.ID);
			}
		}

		public static void DeleteTable(SQLiteConnection connection) { gBlock.DeleteTable(connection); }


		private static FixtureDetailsModel GetFixture(SQLiteDataReader reader, SQLiteConnection connection)
        {
			FixtureDetailsModel model;

			long POSITION_ID = (long)reader[DBFixtureDetailsNames.POSITION_ID];
			long TRANSFORM_ID = (long)reader[DBFixtureDetailsNames.MATRIX_ID];
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
			long ID = (long)reader[DBFixtureDetailsNames.ID];
			long FILE_ID = (long)reader[DBFixtureDetailsNames.FILE_ID];

			Matrix3dModel matrix = DBMatrix3d.SelectRow(connection, TRANSFORM_ID);
			Point3dModel position = DBPoint3D.SelectRow(connection, POSITION_ID);
			DwgFileModel file = DBDwgFile.SelectRow(connection, FILE_ID);

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
			model.ID = ID;
			model.file = file;

			return model;
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
					//throw new Exception("DBFixtureDetails -> UpdateRow -> No Row is Updated.");
					return 0;
				}
				throw new Exception("DBFixtureDetails -> UpdateRow -> Update Not Successful.");
			}
		}
		public static long InsertRow(SQLiteConnection connection, ref FixtureDetailsModel model)
        {
			using(SQLiteCommand command = connection.CreateCommand())
            {
				DBFixtureDetailsCommands.InsertRow(model, command);
				long check = command.ExecuteNonQuery();
				if(check == 1)
                {
					model.ID = connection.LastInsertRowId;
					return model.ID;
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
    }

	class DBFixtureDetailsCommands
    {
		public static void UpdateRow(FixtureDetailsModel model, SQLiteCommand command)
        {
			DBPoint3D.UpdateRow(model.position, command.Connection);
			DBMatrix3d.Update(command.Connection, model.matrixTransform);
			
			List<List<object>> items = getListItems(model);

			Dictionary<string, string> variables = new Dictionary<string, string>();
			Dictionary<string, string> conDict = new Dictionary<string, string> { {DBFixtureDetailsNames.ID, DBFixtureDetailsNames_AT.id} };
			Dictionary<string, object> paraDict = new Dictionary<string, object>();

			foreach(List<object> item in items)
            {
				variables.Add((string)item[0], (string)item[1]);
				paraDict.Add((string)item[1], item[2]);
            }

			paraDict.Add(DBFixtureDetailsNames_AT.id, model.ID);

			DBCommand.UpdateRow(DBFixtureDetailsNames.name, variables, conDict, paraDict, command);
			command.ExecuteNonQuery();
		}
		public static void InsertRow(FixtureDetailsModel model, SQLiteCommand command)
        {
			List<List<object>> items = getListItems(model);

			List<string> variables = new List<string>();
			Dictionary<string, object> paraDict = new Dictionary<string, object>();

			foreach(List<object> item in items){
				variables.Add((string)item[0]);
				paraDict.Add((string)item[1], item[2]);
            }

			DBCommand.InsertCommand(DBFixtureDetailsNames.name, variables, paraDict, command);
			command.ExecuteNonQuery();
			model.ID = command.Connection.LastInsertRowId;
		}

		private static List<List<object>> getListItems(FixtureDetailsModel model)
        {
			List<List<object>> items = new List<List<object>> {
				new List<object>{DBFixtureDetailsNames.POSITION_ID, DBFixtureDetailsNames_AT.position, model.position.ID },
				new List<object>{DBFixtureDetailsNames.MATRIX_ID, DBFixtureDetailsNames_AT.matrix, model.matrixTransform.ID },
				new List<object>{DBFixtureDetailsNames.HANDLE, DBFixtureDetailsNames_AT.handle, model.handle },
				new List<object>{DBFixtureDetailsNames.INDEXX, DBFixtureDetailsNames_AT.indexx, model.INDEX },
				new List<object>{DBFixtureDetailsNames.FIXTURE_NAME, DBFixtureDetailsNames_AT.fixture, model.FIXTURENAME },
				new List<object>{DBFixtureDetailsNames.TAG, DBFixtureDetailsNames_AT.tag, model.TAG },
				new List<object>{DBFixtureDetailsNames.NUMBER, DBFixtureDetailsNames_AT.num, model.NUMBER },
				new List<object>{DBFixtureDetailsNames.CW_DIA, DBFixtureDetailsNames_AT.cwD, model.CW_DIA },
				new List<object>{DBFixtureDetailsNames.HW_DIA, DBFixtureDetailsNames_AT.hwD, model.HW_DIA },
				new List<object>{DBFixtureDetailsNames.WASTE_DIA, DBFixtureDetailsNames_AT.wD, model.WASTE_DIA },
				new List<object>{DBFixtureDetailsNames.VENT_DIA, DBFixtureDetailsNames_AT.vD, model.VENT_DIA },
				new List<object>{DBFixtureDetailsNames.STORM_DIA, DBFixtureDetailsNames_AT.sD, model.STORM_DIA },
				new List<object>{DBFixtureDetailsNames.WSFU, DBFixtureDetailsNames_AT.wsfu, model.WSFU },
				new List<object>{DBFixtureDetailsNames.CWSFU, DBFixtureDetailsNames_AT.cwsfu, model.CWSFU },
				new List<object>{DBFixtureDetailsNames.HWSFU, DBFixtureDetailsNames_AT.hwsfu, model.HWSFU },
				new List<object>{DBFixtureDetailsNames.DFU, DBFixtureDetailsNames_AT.dfu, model.DFU },
				new List<object>{DBFixtureDetailsNames.DESCRIPTION, DBFixtureDetailsNames_AT.desc, model.DESCRIPTION },
				new List<object>{DBFixtureDetailsNames.FILE_ID, DBFixtureDetailsNames_AT.file, model.file.ID }
			};

			return items;
		}

		public static void CreateTable(SQLiteCommand command)
        {
			StringBuilder builder = new StringBuilder();
			builder.Append(string.Format($"CREATE TABLE IF NOT EXISTS '{DBFixtureDetailsNames.name}' ("));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.ID}' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.POSITION_ID}' INTEGER NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.MATRIX_ID}' INTEGER NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.HANDLE}' TEXT NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.INDEXX}' REAL NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.FIXTURE_NAME}' TEXT NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.TAG}' TEXT NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.NUMBER}' TEXT NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.CW_DIA}' REAL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.HW_DIA}' REAL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.WASTE_DIA}' REAL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.VENT_DIA}' REAL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.STORM_DIA}' REAL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.WSFU}' REAL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.CWSFU}' REAL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.HWSFU}' REAL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.DFU}' REAL, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.DESCRIPTION}' TEXT, "));
			builder.Append(string.Format($"'{DBFixtureDetailsNames.FILE_ID}' INTEGER NOT NULL, "));
			builder.Append(string.Format($"FOREIGN KEY('{DBFixtureDetailsNames.MATRIX_ID}') REFERENCES '{DBMatrixName.name}'('{DBMatrixName.ID}') ON DELETE CASCADE, "));
			builder.Append(string.Format($"FOREIGN KEY('{DBFixtureDetailsNames.FILE_ID}') REFERENCES '{DBMatrixName.name}'('{DBDwgFileName.ID}') ON DELETE CASCADE, "));
			builder.Append(string.Format($"FOREIGN KEY('{DBFixtureDetailsNames.POSITION_ID}') REFERENCES '{DBPoint3DName.tableName}'('{DBPoint3DName.ID}') ON DELETE CASCADE"));
			builder.Append(string.Format(");"));
			command.CommandText = builder.ToString();
        }

		public static void DeleteTable(SQLiteCommand command)
        {
			DBCommand.DeleteTable(DBFixtureDetailsNames.name, command);
		}
    }

	class DBFixtureDetailsNames : DBBlockName
    {
		public static string name = "FIXTURE_DETAILS";

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

	class DBFixtureDetailsNames_AT : DBBlockName_AT
    {
		public static string indexx = "@indexx";
		public static string fixture = "@fixture";
		public static string tag = "@tag";
		public static string num = "@num";
		public static string cwD = "@cwD";
		public static string hwD = "@hwD";
		public static string wD = "@wasteDia";
		public static string vD = "@ventDia";
		public static string sD = "@stormIDia";
		public static string wsfu = "@wsfu";
		public static string cwsfu = "@cwsfu";
		public static string hwsfu = "@hwsfu";
		public static string dfu = "@dfu";
		public static string desc = "@desc";
	}
}
