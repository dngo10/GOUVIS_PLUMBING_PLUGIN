using ClassLibrary1.DATABASE.Controllers.BlockInterFace;
using ClassLibrary1.DATABASE.DBModels;
using GouvisPlumbingNew.DATABASE.Controllers;
using GouvisPlumbingNew.DATABASE.DBModels;
using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DATABASE.Controllers
{
    class DBFixture_Unit
    {
		public static BlockGeneral<FixtureUnitModel> gBlock = new BlockGeneral<FixtureUnitModel>(getItem, DBFixtureUnitName.name);
		public static List<FixtureUnitModel> SelectRows(SQLiteConnection connection, long fileID) { return gBlock.SelectRows(fileID, connection); }
		public static List<FixtureUnitModel> SelectRows(SQLiteConnection connection, string relPath) { return gBlock.SelectRows(relPath, connection); }
		public static List<FixtureUnitModel> SelectRows(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
		{ return gBlock.SelectRows(conDict, paraDict, connection); }

		public static FixtureUnitModel SelectRow(SQLiteConnection connection, long ID) { return gBlock.SelectRow(ID, connection); }
		public static FixtureUnitModel SelectRow(SQLiteConnection connection, string handle, long file_ID) { return gBlock.SelectRow(handle, file_ID, connection); }
		public static FixtureUnitModel SelectRow(SQLiteConnection connection, string handle, string relPath) { return gBlock.SelectRow(handle, relPath, connection); }
		public static FixtureUnitModel SelectRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
		{ return gBlock.SelectRow(conDict, paraDict, connection); }

		public static bool HasRow(SQLiteConnection connection, long ID) { return gBlock.HasRow(ID, connection); }
		public static bool HasRow(SQLiteConnection connection, string handle, long fileID) { return gBlock.HasRow(handle, fileID, connection); }
		public static bool HasRow(SQLiteConnection connection, string handle, string relPath) { return gBlock.HasRow(handle, relPath, connection); }
		public static bool HasRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
		{ return gBlock.HasRow(conDict, paraDict, connection); }

		public static void DeleteRow(SQLiteConnection connection, long ID)
		{
			FixtureUnitModel model = SelectRow(connection, ID);
			gBlock.DeleteRow(ID, connection);
			DeleteOthers(model, connection);
		}

		public static void DeleteRow(SQLiteConnection connection, string handle, long fileID)
		{
			FixtureUnitModel model = SelectRow(connection, handle, fileID);
			gBlock.DeleteRow(handle, fileID, connection);
			DeleteOthers(model, connection);
		}
		public static void DeleteRow(SQLiteConnection connection, string handle, string relPath)
		{
			FixtureUnitModel model = SelectRow(connection, handle, relPath);
			gBlock.DeleteRow(handle, relPath, connection);
			DeleteOthers(model, connection);
		}
		public static void DeleteRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
		{
			FixtureUnitModel model = SelectRow(connection, conDict, paraDict);
			gBlock.DeleteRow(conDict, paraDict, connection);
			DeleteOthers(model, connection);
		}

		private static void DeleteOthers(FixtureUnitModel model, SQLiteConnection connection)
		{
			if (model != null)
			{
				DBPoint3D.DeleteRow(connection, model.file.ID);
				DBMatrix3d.DeleteRow(connection, model.matrixTransform.ID);

				if (model.matrixTransform != null) { DBMatrix3d.DeleteRow(connection, model.matrixTransform.ID); };
				if (model.position != null) { DBPoint3D.DeleteRow(connection, model.position.ID); };

				if (model.tagPos != null) { DBPoint3D.DeleteRow(connection, model.tagPos.ID); };
				if (model.ventPos != null) { DBPoint3D.DeleteRow(connection, model.ventPos.ID); };
				if (model.hotStub != null) { DBPoint3D.DeleteRow(connection, model.hotStub.ID); };
				if (model.coldStub != null) { DBPoint3D.DeleteRow(connection, model.coldStub.ID); };
				if (model.drainPos != null) { DBPoint3D.DeleteRow(connection, model.drainPos.ID); };
			}
		}

		public static void DeleteTable(SQLiteConnection connection) { gBlock.DeleteTable(connection); }

		private static FixtureUnitModel getItem(SQLiteDataReader reader, SQLiteConnection connection)
        {
			FixtureUnitModel model = new FixtureUnitModel();

			long POSITION_ID = (long)reader[DBFixtureUnitName.POSITION_ID];
			long TRANSFORM_ID = (long)reader[DBFixtureUnitName.MATRIX_ID];
			model.handle = (string)reader[DBFixtureUnitName.HANDLE];
			model.INDEX = (double)reader[DBFixtureUnitName.INDEX];
			model.TAG = (string)reader[DBFixtureUnitName.TAG];
			model.NUMBER = (string)reader[DBFixtureUnitName.NUMBER];
			model.CW_DIA = (double)reader[DBFixtureUnitName.CW_DIA];
			model.HW_DIA = (double)reader[DBFixtureUnitName.HW_DIA];
			model.WASTE_DIA = (double)reader[DBFixtureUnitName.WASTE_DIA];
			model.VENT_DIA = (double)reader[DBFixtureUnitName.VENT_DIA];
			model.STORM_DIA = (double)reader[DBFixtureUnitName.STORM_DIA];
			model.WSFU = (double)reader[DBFixtureUnitName.WSFU];
			model.CWSFU = (double)reader[DBFixtureUnitName.CWSFU];
			model.HWSFU = (double)reader[DBFixtureUnitName.HWSFU];
			model.DFU = (double)reader[DBFixtureUnitName.DFU];
			model.ID = (long)reader[DBFixtureUnitName.ID];
			long FILE_ID = (long)reader[DBFixtureUnitName.FILE_ID];

			long tagID = (long)reader[DBFixtureUnitName.tagPos];
			long ventID = (long)reader[DBFixtureUnitName.ventPos];
			long hotStubID = (long)reader[DBFixtureUnitName.hotStub];
			long coldStubID = (long)reader[DBFixtureUnitName.coldStub];
			long drainPosID = (long)reader[DBFixtureUnitName.drainPos];
			model.drainType = (long)reader[DBFixtureUnitName.drainType];
			model.studLength = (double)reader[DBFixtureUnitName.studLength];

			model.matrixTransform = DBMatrix3d.SelectRow(connection, TRANSFORM_ID);
			model.position = DBPoint3D.SelectRow(connection, POSITION_ID);
			model.file = DBDwgFile.SelectRow(connection, FILE_ID);

			model.tagPos = DBPoint3D.SelectRow(connection, tagID);
			model.ventPos = DBPoint3D.SelectRow(connection, ventID);
			model.hotStub = DBPoint3D.SelectRow(connection, hotStubID);
			model.coldStub = DBPoint3D.SelectRow(connection, coldStubID);
			model.drainPos = DBPoint3D.SelectRow(connection, drainPosID);

			return model;
		}
		public static long UpdateRow(SQLiteConnection connection, FixtureUnitModel model)
        {
			using(SQLiteCommand command = connection.CreateCommand())
            {
				DBFixtureUnitCommands.UpdateRow(model, command);
				long check = command.ExecuteNonQuery();
				return check;
            }
        }
		public static long InsertRow(SQLiteConnection connection, ref FixtureUnitModel model)
        {
			using(SQLiteCommand command = connection.CreateCommand())
            {
				DBFixtureUnitCommands.InsertRow(model, command);
				long check = command.ExecuteNonQuery();
				if(check == 1)
                {
					model.ID = connection.LastInsertRowId;
					return model.ID;
                }
				throw new Exception("DBFixture_Unit -> InserRow -> insertRowNotSuccessful.");
            }
        }
		public static void CreateTable(SQLiteConnection connection)
        {
			using(SQLiteCommand command = connection.CreateCommand())
            {
				DBFixtureUnitCommands.CreateTable(command);
				command.ExecuteNonQuery();
            }
        }
    }

    class DBFixtureUnitCommands
    {
		public static void UpdateRow(FixtureUnitModel model, SQLiteCommand command)
		{
			List<List<object>> items = getListItems(model);

			Dictionary<string, string> variables = new Dictionary<string, string>();
			Dictionary<string, string> conDict = new Dictionary<string, string> { { DBFixtureUnitName.ID, DBFixtureUnitName_AT.id } };
			Dictionary<string, object> paraDict = new Dictionary<string, object>();

			foreach (List<object> item in items)
			{
				variables.Add((string)item[0], (string)item[1]);
				paraDict.Add((string)item[1], item[2]);
			}

			paraDict.Add(DBFixtureUnitName_AT.id, model.ID);

			DBCommand.UpdateRow(DBFixtureUnitName.name, variables, conDict, paraDict, command);
		}
		public static void InsertRow(FixtureUnitModel model, SQLiteCommand command)
		{
			List<List<object>> items = getListItems(model);

			List<string> variables = new List<string>();
			Dictionary<string, object> paraDict = new Dictionary<string, object>();

			foreach (List<object> item in items)
			{
				variables.Add((string)item[0]);
				paraDict.Add((string)item[1], item[2]);
			}

			DBCommand.InsertCommand(DBFixtureUnitName.name, variables, paraDict, command);
		}

		private static List<List<object>> getListItems(FixtureUnitModel model)
		{
			List<List<object>> items = new List<List<object>> {
				new List<object>{DBFixtureUnitName.POSITION_ID, DBFixtureUnitName_AT.position, model.position.ID },
				new List<object>{DBFixtureUnitName.MATRIX_ID, DBFixtureUnitName_AT.matrix, model.matrixTransform.ID },
				new List<object>{DBFixtureUnitName.HANDLE, DBFixtureUnitName_AT.handle, model.handle },
				new List<object>{DBFixtureUnitName.INDEX, DBFixtureUnitName_AT.indexx, model.INDEX },
				new List<object>{DBFixtureUnitName.TAG, DBFixtureUnitName_AT.tag, model.TAG },
				new List<object>{DBFixtureUnitName.NUMBER, DBFixtureUnitName_AT.num, model.NUMBER },
				new List<object>{DBFixtureUnitName.CW_DIA, DBFixtureUnitName_AT.cwD, model.CW_DIA },
				new List<object>{DBFixtureUnitName.HW_DIA, DBFixtureUnitName_AT.hwD, model.HW_DIA },
				new List<object>{DBFixtureUnitName.WASTE_DIA, DBFixtureUnitName_AT.wD, model.WASTE_DIA },
				new List<object>{DBFixtureUnitName.VENT_DIA, DBFixtureUnitName_AT.vD, model.VENT_DIA },
				new List<object>{DBFixtureUnitName.STORM_DIA, DBFixtureUnitName_AT.sD, model.STORM_DIA },
				new List<object>{DBFixtureUnitName.WSFU, DBFixtureUnitName_AT.wsfu, model.WSFU },
				new List<object>{DBFixtureUnitName.CWSFU, DBFixtureUnitName_AT.cwsfu, model.CWSFU },
				new List<object>{DBFixtureUnitName.HWSFU, DBFixtureUnitName_AT.hwsfu, model.HWSFU },
				new List<object>{DBFixtureUnitName.DFU, DBFixtureUnitName_AT.dfu, model.DFU },
				new List<object>{DBFixtureUnitName.FILE_ID, DBFixtureUnitName_AT.file, model.file.ID },

				new List<object>{DBFixtureUnitName.tagPos, DBFixtureUnitName_AT.tagPos, model.tagPos.ID},
				new List<object>{DBFixtureUnitName.ventPos, DBFixtureUnitName_AT.ventPos, model.ventPos.ID },
				new List<object>{DBFixtureUnitName.hotStub, DBFixtureUnitName_AT.hotStub, model.hotStub.ID },
				new List<object>{DBFixtureUnitName.coldStub, DBFixtureUnitName_AT.coldStub, model.coldStub.ID },
				new List<object>{DBFixtureUnitName.drainPos, DBFixtureUnitName_AT.drainPos, model.drainPos.ID },
				new List<object>{DBFixtureUnitName.drainType, DBFixtureUnitName_AT.drainType, model.drainType },
				new List<object>{DBFixtureUnitName.studLength, DBFixtureUnitName_AT.studLength, model.studLength},

			};

			return items;
		}

		public static void CreateTable(SQLiteCommand command)
		{
			StringBuilder builder = new StringBuilder();
			builder.Append(string.Format($"CREATE TABLE IF NOT EXISTS '{DBFixtureUnitName.name}' ("));
			builder.Append(string.Format($"'{DBFixtureUnitName.ID}' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.POSITION_ID}' INTEGER NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.MATRIX_ID}' INTEGER NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.HANDLE}' TEXT NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.INDEX}' REAL NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.TAG}' TEXT NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.NUMBER}' TEXT NOT NULL, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.CW_DIA}' REAL, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.HW_DIA}' REAL, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.WASTE_DIA}' REAL, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.VENT_DIA}' REAL, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.STORM_DIA}' REAL, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.WSFU}' REAL, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.CWSFU}' REAL, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.HWSFU}' REAL, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.DFU}' REAL, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.FILE_ID}' INTEGER NOT NULL, "));

			//FIX THIS
			builder.Append(string.Format($"'{DBFixtureUnitName.tagPos}' INTEGER, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.ventPos}' INTEGER, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.drainPos}' INTEGER, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.hotStub}' INTEGER, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.coldStub}' INTEGER, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.drainType}' INTEGER, "));
			builder.Append(string.Format($"'{DBFixtureUnitName.studLength}' REAL, "));

			builder.Append(string.Format($"FOREIGN KEY('{DBFixtureUnitName.tagPos}') REFERENCES '{DBPoint3DName.tableName}'('{DBPoint3DName.ID}') ON DELETE CASCADE, "));
			builder.Append(string.Format($"FOREIGN KEY('{DBFixtureUnitName.ventPos}') REFERENCES '{DBPoint3DName.tableName}'('{DBPoint3DName.ID}') ON DELETE CASCADE, "));
			builder.Append(string.Format($"FOREIGN KEY('{DBFixtureUnitName.drainPos}') REFERENCES '{DBPoint3DName.tableName}'('{DBPoint3DName.ID}') ON DELETE CASCADE, "));
			builder.Append(string.Format($"FOREIGN KEY('{DBFixtureUnitName.hotStub}') REFERENCES '{DBPoint3DName.tableName}'('{DBPoint3DName.ID}') ON DELETE CASCADE, "));
			builder.Append(string.Format($"FOREIGN KEY('{DBFixtureUnitName.coldStub}') REFERENCES '{DBPoint3DName.tableName}'('{DBPoint3DName.ID}') ON DELETE CASCADE, "));
			builder.Append(string.Format($"FOREIGN KEY('{DBFixtureUnitName.drainType}') REFERENCES '{DBPoint3DName.tableName}'('{DBPoint3DName.ID}') ON DELETE CASCADE, "));


			builder.Append(string.Format($"FOREIGN KEY('{DBFixtureUnitName.MATRIX_ID}') REFERENCES '{DBMatrixName.name}'('{DBMatrixName.ID}') ON DELETE CASCADE, "));
			builder.Append(string.Format($"FOREIGN KEY('{DBFixtureUnitName.FILE_ID}') REFERENCES '{DBMatrixName.name}'('{DBDwgFileName.ID}') ON DELETE CASCADE, "));
			builder.Append(string.Format($"FOREIGN KEY('{DBFixtureUnitName.POSITION_ID}') REFERENCES '{DBPoint3DName.tableName}'('{DBPoint3DName.ID}') ON DELETE CASCADE"));
			builder.Append(string.Format(");"));
			command.CommandText = builder.ToString();
		}

		public static void DeleteTable(SQLiteCommand command)
		{
			DBCommand.DeleteTable(DBFixtureUnitName.name, command);
		}
	}

    class DBFixtureUnitName: DBBlockName
    {
		public const string name = "FIXTURE_UNIT";

        public const string INDEX = "INDEXX";
        public const string TAG = "TAG";
        public const string NUMBER = "NUM";
        public const string CW_DIA = "CW_DIA";
        public const string HW_DIA = "HW_DIA";
        public const string WASTE_DIA = "WASTE_DIA";
        public const string VENT_DIA = "VENT_DIA";
        public const string STORM_DIA = "STORM_DIA";
        public const string WSFU = "WSFU";
        public const string CWSFU = "CWSFU";
        public const string HWSFU = "HWSFU";
        public const string DFU = "HWSFU";

		public const string tagPos = "TAGPOS";
		public const string ventPos = "VENTPOS";
        public const string drainPos = "DRAINPOS";
        public const string hotStub = "HOTSTUB";
        public const string coldStub = "COLDSTUB";
        public const string drainType = "DRAIN_TYPE";
        public const string studLength = "STUD_LENGTH";
    }

    class DBFixtureUnitName_AT : DBBlockName_AT
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

		public const string tagPos = "@tagPos";
        public const string ventPos = "@ventPos";
        public const string drainPos = "@drainPos";
        public const string hotStub = "@hotStub";
        public const string coldStub = "@coldStub";
        public const string drainType = "@drainType";
        public const string studLength = "@studLength";
    }
}
