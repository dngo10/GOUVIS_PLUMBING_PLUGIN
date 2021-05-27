using GouvisPlumbingNew.DATABASE.Controllers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1.DATABASE.DBModels;
using GouvisPlumbingNew.DATABASE.DBModels;
using ClassLibrary1.DATABASE.Controllers.BlockInterFace;
using GouvisPlumbingNew.HELPERS;

namespace ClassLibrary1.DATABASE.Controllers
{

    /*
    CREATE TABLE "TABLE" (
	"ID"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	"HANDLE"	TEXT NOT NULL,
	"POSITION_ID"	INTEGER NOT NULL,
	"TRANSFORM_ID"	INTEGER NOT NULL,
	"FILE_ID"	INTEGER NOT NULL,
	"ALIAS"	TEXT,
	"A_VALUE"	TEXT,
	FOREIGN KEY("POSITION_ID") REFERENCES "POINT3D"("ID") ON DELETE CASCADE,
	FOREIGN KEY("FILE_ID") REFERENCES "FILE"("ID") ON DELETE CASCADE,
	FOREIGN KEY("TRANSFORM_ID") REFERENCES "MATRIX3D"("ID") ON DELETE CASCADE
    );
     */
    class DBTable
    {
		public static BlockGeneral<TableModel> gBlock = new BlockGeneral<TableModel>(CreateModel, DBTableName.name);
		public static List<TableModel> SelectRows(SQLiteConnection connection, long fileID) { return gBlock.SelectRows(fileID, connection); }
		public static List<TableModel> SelectRows(SQLiteConnection connection, string relPath) { return gBlock.SelectRows(relPath, connection); }
		public static List<TableModel> SelectRows(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
		{ return gBlock.SelectRows(conDict, paraDict, connection); }

		public static TableModel SelectRow(SQLiteConnection connection, long ID) { return gBlock.SelectRow(ID, connection); }
		public static TableModel SelectRow(SQLiteConnection connection, string handle, long file_ID) { return gBlock.SelectRow(handle, file_ID, connection); }
		public static TableModel SelectRow(SQLiteConnection connection, string handle, string relPath) { return gBlock.SelectRow(handle, relPath, connection); }
		public static TableModel SelectRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
		{ return gBlock.SelectRow(conDict, paraDict, connection); }

		public static bool HasRow(SQLiteConnection connection, long ID) { return gBlock.HasRow(ID, connection); }
		public static bool HasRow(SQLiteConnection connection, string handle, long fileID) { return gBlock.HasRow(handle, fileID, connection); }
		public static bool HasRow(SQLiteConnection connection, string handle, string relPath) { return gBlock.HasRow(handle, relPath, connection); }
		public static bool HasRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
		{ return gBlock.HasRow(conDict, paraDict, connection); }

		public static void DeleteRow(SQLiteConnection connection, long ID)
		{
			TableModel model = SelectRow(connection, ID);
			gBlock.DeleteRow(ID, connection);
			DeleteOthers(model, connection);
		}

		public static void DeleteRow(SQLiteConnection connection, string handle, long fileID)
		{
			TableModel model = SelectRow(connection, handle, fileID);
			gBlock.DeleteRow(handle, fileID, connection);
			DeleteOthers(model, connection);
		}
		public static void DeleteRow(SQLiteConnection connection, string handle, string relPath)
		{
			TableModel model = SelectRow(connection, handle, relPath);
			gBlock.DeleteRow(handle, relPath, connection);
			DeleteOthers(model, connection);
		}
		public static void DeleteRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
		{
			TableModel model = SelectRow(connection, conDict, paraDict);
			gBlock.DeleteRow(conDict, paraDict, connection);
			DeleteOthers(model, connection);
		}

		private static void DeleteOthers(TableModel model, SQLiteConnection connection)
		{
			if (model != null)
			{

				if (model.matrixTransform != null) { DBMatrix3d.DeleteRow(connection, model.matrixTransform.ID); };
				if (model.position != null) { DBPoint3D.DeleteRow(connection, model.position.ID); };
			}
		}

		public static void DeleteTable(SQLiteConnection connection) { gBlock.DeleteTable(connection); }

		private static TableModel CreateModel(SQLiteDataReader reader, SQLiteConnection connection)
        {
			TableModel model = new TableModel();
			model.ID = Convert.ToInt64(reader[DBTableName.ID]);
			model.handle = Convert.ToString(reader[DBTableName.HANDLE]);
			model.matrixTransform = DBMatrix3d.SelectRow(connection, Convert.ToInt64(reader[DBTableName.MATRIX_ID]));
			model.position = DBPoint3D.SelectRow(connection, Convert.ToInt64(reader[DBTableName.POSITION_ID]));
			model.ALIAS = Convert.ToString(reader[DBTableName.ALIAS]);
			model.A_VALUE = Convert.ToString(reader[DBTableName.A_VALUE]);
			model.file = DBDwgFile.SelectRow(connection, Convert.ToInt64(reader[DBTableName.FILE_ID]));

			return model;
		}

		public static void UpdateRow(SQLiteConnection connection, TableModel model)
        {
			using (SQLiteCommand command = connection.CreateCommand())
			{
				DBTableCommands.UpdateRow(command, model);
				long check = command.ExecuteNonQuery();
				DBPoint3D.UpdateRow(model.position, connection);
				DBMatrix3d.Update(connection, model.matrixTransform);
            }
        }
		public static long InsertRow(SQLiteConnection connection, TableModel model)
        {
			using(SQLiteCommand command = connection.CreateCommand())
            {
				DBTableCommands.InsertRow(command, model);
				long check = command.ExecuteNonQuery();
				if (check == 1)
				{
					model.ID = connection.LastInsertRowId;
					return model.ID;
				}else if(check == 0)
                {
					throw new Exception("DBTable -> InsertRow -> Can't insert.");
                }
				throw new Exception("DBTable -> InsertRow -> Failed to Insert.");
			}
        }
		public static void CreateTable(SQLiteConnection connection)
        {
			using(SQLiteCommand command = connection.CreateCommand())
            {
				DBTableCommands.CreateTable(command);
				command.ExecuteNonQuery();
            }
        }
    }

    class DBTableCommands
    {

		public static void UpdateRow(SQLiteCommand command, TableModel model)
        {
			List<List<object>> updateRow = new List<List<object>>
			{
				new List<object> {DBTableName.HANDLE,		DBTableName_AT.handle,		model.handle},
				new List<object> {DBTableName.POSITION_ID,	DBTableName_AT.position,	model.position.ID},
				new List<object> {DBTableName.MATRIX_ID,	DBTableName_AT.matrix,		model.matrixTransform.ID},
				new List<object> {DBTableName.FILE_ID,		DBTableName_AT.file,		model.file.ID},
				new List<object> {DBTableName.ALIAS,		DBTableName_AT.alias,		model.ALIAS},
				new List<object> {DBTableName.A_VALUE,		DBTableName_AT.a_value,		model.A_VALUE}
			};

			Dictionary<string, string> variables = new Dictionary<string, string>();
			Dictionary<string, object> paraDict = new Dictionary<string, object>();

			foreach (List<object> item in updateRow)
			{
				variables.Add((string)item[0], (string)item[1]);
				paraDict.Add((string)item[1], item[2]);
			}

			//This line is important
			paraDict.Add(DBTableName_AT.id, model.ID);

			Dictionary<string, string> conditions = new Dictionary<string, string>
			{
				{DBTableName.ID, DBTableName_AT.id }
			};
			DBCommand.UpdateRow(DBTableName.name, variables, conditions, paraDict, command);

		}

		public static void InsertRow(SQLiteCommand command, TableModel model)
        {
			List<string> variables = new List<string> { DBTableName.HANDLE,
														DBTableName.POSITION_ID,
														DBTableName.MATRIX_ID,
														DBTableName.FILE_ID,
														DBTableName.ALIAS,
														DBTableName.A_VALUE};



			Dictionary<string, object> paraDict = new Dictionary<string, object>
			{
				{DBTableName_AT.handle, model.handle},
				{DBTableName_AT.position, model.position.ID},
				{DBTableName_AT.matrix, model.matrixTransform.ID},
				{DBTableName_AT.file, model.file.ID},
				{DBTableName_AT.alias, model.ALIAS},
				{DBTableName_AT.a_value, model.A_VALUE}
			};

			DBCommand.InsertCommand(DBTableName.name, variables, paraDict, command);
		}

		public static void DeleteTable(SQLiteCommand command)
        {
			DBCommand.DeleteTable(DBTableName.name, command);
        }
		public static void CreateTable(SQLiteCommand command)
        {
			StringBuilder builder = new StringBuilder();
			builder.Append($"CREATE TABLE IF NOT EXISTS {DBTableName.name}( ");
			builder.Append($"'{DBTableName.ID}' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, ");
			builder.Append($"'{DBTableName.HANDLE}' TEXT NOT NULL, ");
			builder.Append($"'{DBTableName.POSITION_ID}' INTEGER NOT NULL, ");
			builder.Append($"'{DBTableName.MATRIX_ID}' INTEGER NOT NULL, ");
			builder.Append($"'{DBTableName.FILE_ID}' INTEGER NOT NULL, ");
			builder.Append($"'{DBTableName.ALIAS}' TEXT, ");
			builder.Append($"'{DBTableName.A_VALUE}' TEXT, ");
			builder.Append($"FOREIGN KEY('{DBTableName.POSITION_ID}') REFERENCES '{DBPoint3DName.tableName}'('{DBPoint3DName.ID}') ON DELETE CASCADE, ");
			builder.Append($"FOREIGN KEY('{DBTableName.FILE_ID}') REFERENCES '{DBDwgFileName.name}'('{DBDwgFileName.ID}') ON DELETE CASCADE, ");
			builder.Append($"FOREIGN KEY('{DBTableName.MATRIX_ID}') REFERENCES '{DBMatrixName.name}'('{DBMatrixName.ID}') ON DELETE CASCADE ");
			builder.Append($");");

			command.CommandText = builder.ToString();
		}
    }


    class DBTableName : DBBlockName
    {
		public const string name = "DB_TABLE";
		public const string ALIAS = "ALIAS";
		public const string A_VALUE = "A_VALUE";
    }

	class DBTableName_AT: DBBlockName_AT
    {
		public const string alias = "@alias";
		public const string a_value = "@a_value";
	}
}


