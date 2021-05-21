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
		public static bool HasRow(SQLiteConnection connection, long ID)
        {
			using(SQLiteCommand command = connection.CreateCommand())
            {
				DBTableCommands.SelectCount(command, ID);
				long check = Convert.ToInt64(command.ExecuteScalar());
				if(check == 1)
                {
					return true;
                }
            }
			return false;
        }

		public static void DeleteRow(SQLiteConnection connection, TableModel model)
        {
			using(SQLiteCommand command = connection.CreateCommand())
            {
				DBTableCommands.DeleteRow(command, model.ID);
				long check = Convert.ToInt64(command.ExecuteNonQuery());
				if(check == 1)
                {
					DBPoint3D.DeleteRow(model.position.ID, connection);
					DBMatrix3d.DeleteRow(connection, model.matrixTransform.ID);
                }
            }
        }

		public static List<TableModel> SelectRows(SQLiteConnection connection, long FILE_ID)
        {
			List<TableModel> models = new List<TableModel>();
			using(SQLiteCommand command = connection.CreateCommand())
            {
				DBTableCommands.SelectRows(command, FILE_ID);
				SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
					models.Add(CreateModel(reader, connection));
                }
            }
			return models;
        }
		public static TableModel SelectRow(SQLiteConnection connection, long ID)
        {
			using(SQLiteCommand command = connection.CreateCommand())
            {
				DBTableCommands.SelectRow(command, ID);
				SQLiteDataReader reader = command.ExecuteReader();

				TableModel model = null;
                while (reader.Read())
                {
					model = CreateModel(reader, connection);
                }
				return model;
            }
        }

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

		public static long UpdateRow(SQLiteConnection connection, TableModel model)
        {
			using (SQLiteCommand command = connection.CreateCommand())
			{
				DBTableCommands.UpdateRow(command, model);
				long check = command.ExecuteNonQuery();

				if (check == 1)
                {
					DBPoint3D.UpdateRow(model.position, connection);
					//Update MORE
					DBMatrix3d.Update(connection, model.matrixTransform);
				}else if(check == 0)
                {
					Console.WriteLine("DBTable -> UpdateRow -> Nothing is updated");
					return ConstantName.invalidNum;
                }
				throw new Exception("DBTable -> UpdateRow -> Failed to Update.");
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
		public static void DeleteTable(SQLiteConnection connection)
        {
			using(SQLiteCommand command = connection.CreateCommand())
            {
				DBTableCommands.DeleteTable(command);
				command.ExecuteNonQuery();
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
		
		public static void SelectCount(SQLiteCommand command, string handle, long file_ID)
        {
			DBCommand.SelectCount(DBDwgFileName.name, handle, file_ID, command);
        }

		public static void SelectCount(SQLiteCommand command, long ID)
        {
			DBCommand.SelectCount(DBTableName.name, ID, command);
        }

		//FIX THIS
		public static void SelectRows(SQLiteCommand command, long File_ID)
		{
			Dictionary<string, string> conDict = new Dictionary<string, string> { { DBTableName.FILE_ID, DBTableName_AT.file } };
			Dictionary<string, object> paraDict = new Dictionary<string, object> { { DBTableName_AT.file, File_ID } };
			DBCommand.SelectRow(DBTableName.name, conDict, paraDict, command);
		}

		public static void SelectRow(SQLiteCommand command, string handle, long file_ID)
        {
			DBCommand.SelectRow(DBTableName.name, handle, file_ID, command);
		}

		public static void SelectRow(SQLiteCommand command, long ID)
        {
			DBCommand.SelectRow(DBTableName.name, ID, command);
		}

		public static void DeleteRow(SQLiteCommand command, string handle, long file_ID)
        {
			DBCommand.DeleteRow(DBTableName.name, handle, file_ID, command);
		}

		public static void DeleteRow(SQLiteCommand command, long ID)
        {
			DBCommand.DeleteRow(DBTableName.name, ID, command);
		}

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


