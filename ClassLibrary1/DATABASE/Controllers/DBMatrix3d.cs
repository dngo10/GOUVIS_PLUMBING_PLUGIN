using ClassLibrary1.DATABASE.Controllers.BlockInterFace;
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
     CREATE TABLE "MATRIX3D" (
	"ID"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	"R00"	REAL NOT NULL,
	"R01"	REAL NOT NULL,
	"R02"	REAL NOT NULL,
	"R03"	REAL NOT NULL,
	"R10"	REAL NOT NULL,
	"R11"	REAL NOT NULL,
	"R12"	REAL NOT NULL,
	"R13"	REAL NOT NULL,
	"R20"	REAL NOT NULL,
	"R21"	REAL NOT NULL,
	"R22"	REAL NOT NULL,
	"R23"	REAL NOT NULL,
	"R30"	REAL NOT NULL,
	"R31"	REAL NOT NULL,
	"R32"	REAL NOT NULL,
	"R33"	REAL NOT NULL
	);
     */
	class DBMatrix3d
    {
		public static BlockGeneral<Matrix3dModel> gBlock = new BlockGeneral<Matrix3dModel>(GetItem, DBMatrixName.name);
		//public static List<Matrix3dModel> SelectRows(SQLiteConnection connection, long fileID) { return gBlock.SelectRows(fileID, connection); }
		//public static List<Matrix3dModel> SelectRows(SQLiteConnection connection, string relPath) { return gBlock.SelectRows(relPath, connection); }
		public static List<Matrix3dModel> SelectRows(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
		{ return gBlock.SelectRows(conDict, paraDict, connection); }

		public static Matrix3dModel SelectRow(SQLiteConnection connection, long ID) { return gBlock.SelectRow(ID, connection); }
		//public static Matrix3dModel SelectRow(SQLiteConnection connection, string handle, long file_ID) { return gBlock.SelectRow(handle, file_ID, connection); }
		//public static Matrix3dModel SelectRow(SQLiteConnection connection, string handle, string relPath) { return gBlock.SelectRow(handle, relPath, connection); }
		public static Matrix3dModel SelectRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
		{ return gBlock.SelectRow(conDict, paraDict, connection); }

		public static bool HasRow(SQLiteConnection connection, long ID) { return gBlock.HasRow(ID, connection); }
		//public static bool HasRow(SQLiteConnection connection, string handle, long fileID) { return gBlock.HasRow(handle, fileID, connection); }
		//public static bool HasRow(SQLiteConnection connection, string handle, string relPath) { return gBlock.HasRow(handle, relPath, connection); }
		public static bool HasRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
		{ return gBlock.HasRow(conDict, paraDict, connection); }

		public static void DeleteRow(SQLiteConnection connection, long ID)
		{
			gBlock.DeleteRow(ID, connection);
		}

		//public static void DeleteRow(SQLiteConnection connection, string handle, long fileID)
		//{
		//	gBlock.DeleteRow(handle, fileID, connection);
		//}
		//public static void DeleteRow(SQLiteConnection connection, string handle, string relPath)
		//{
		//	gBlock.DeleteRow(handle, relPath, connection);
		//}
		public static void DeleteRow(SQLiteConnection connection, Dictionary<string, string> conDict, Dictionary<string, object> paraDict)
		{
			gBlock.DeleteRow(conDict, paraDict, connection);
		}

		public static void DeleteTable(SQLiteConnection connection) { gBlock.DeleteTable(connection); }

		/// <summary>
		/// This Will return 1 model, if ID not found, will return null.
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="ID">ID of matrix in database</param>
		/// <returns></returns>
		/// 

		private static Matrix3dModel GetItem(SQLiteDataReader reader, SQLiteConnection connection)
        {
			double R00 = (double)reader[DBMatrixName.R00];
			double R01 = (double)reader[DBMatrixName.R01];
			double R02 = (double)reader[DBMatrixName.R02];
			double R03 = (double)reader[DBMatrixName.R03];
			double R10 = (double)reader[DBMatrixName.R10];
			double R11 = (double)reader[DBMatrixName.R11];
			double R12 = (double)reader[DBMatrixName.R12];
			double R13 = (double)reader[DBMatrixName.R13];
			double R20 = (double)reader[DBMatrixName.R20];
			double R21 = (double)reader[DBMatrixName.R21];
			double R22 = (double)reader[DBMatrixName.R22];
			double R23 = (double)reader[DBMatrixName.R23];
			double R30 = (double)reader[DBMatrixName.R30];
			double R31 = (double)reader[DBMatrixName.R31];
			double R32 = (double)reader[DBMatrixName.R32];
			double R33 = (double)reader[DBMatrixName.R33];
			long ID = (long)reader[DBMatrixName.ID];

			double[] arr = new double[] { R00, R01, R02, R03, R10, R11, R12, R13, R20, R21, R22, R23, R30, R31, R32, R33 };
			return new Matrix3dModel(arr, ID);
		}

		public static long Update(SQLiteConnection connection, Matrix3dModel model)
        {
			using (SQLiteCommand command = connection.CreateCommand())
			{
				DBMatrix3dCommands.Update(command, model);
				long check = command.ExecuteNonQuery();
				if (check == 1)
				{
					return connection.LastInsertRowId;
				}
				else if (check == 0)
				{
					//throw new Exception("DBMatrix3d -> Update -> No Row Is Update.");
				}
				throw new Exception("DBMatrix3d -> Update -> Update Is Not Successful.");
			}
		}
		public static long Insert(SQLiteConnection connection, ref Matrix3dModel model)
        {
			using(SQLiteCommand command = connection.CreateCommand())
            {
				DBMatrix3dCommands.Insert(command, model);
				long check = command.ExecuteNonQuery();
				if(check == 1)
                {
					model.ID =  connection.LastInsertRowId;
					return model.ID;
                }
                else if(check == 0)
                {
					throw new Exception("DBMatrix3d -> Insert -> No Row Is Inserted.");
                }
				throw new Exception("DBMatrix3d -> Insert -> Insert Is Not Successful.");
            }
        }
		public static void CreateTable(SQLiteConnection connection)
        {
			using(SQLiteCommand command = connection.CreateCommand())
            {
				DBMatrix3dCommands.CreateTable(command);
				command.ExecuteNonQuery();
            }
        }
    }

	class DBMatrix3dCommands
    {
		public static void Update(SQLiteCommand command, Matrix3dModel model)
        {
			List<List<object>> items = getItems(model);

			Dictionary<string, string> conDict = new Dictionary<string, string> { { DBMatrixName.ID, DBMatrixName_AT.id } };
			Dictionary<string, string> variables = new Dictionary<string, string>();
			Dictionary<string, object> paraDict = new Dictionary<string, object>();

			foreach (List<object> item in items)
			{
				variables.Add((string)item[0], (string)item[1]);
				paraDict.Add((string)item[1], item[2]);
			}

			paraDict.Add(DBMatrixName_AT.id, model.ID);

			DBCommand.UpdateRow(DBMatrixName.name, variables, conDict, paraDict, command);
		}
		public static void Insert(SQLiteCommand command, Matrix3dModel model)
        {
			List<List<object>> items = getItems(model);
			List<string> variables = new List<string>();
			Dictionary<string, object> paraDict = new Dictionary<string, object>();

			foreach(List<object> item in items)
            {
				variables.Add((string)item[0]);
				paraDict.Add((string)item[1], item[2]);
            }

			DBCommand.InsertCommand(DBMatrixName.name, variables, paraDict, command);
		}

		private static List<List<object>> getItems(Matrix3dModel model)
        {
			List<List<object>> items = new List<List<object>>
			{
				new List<object>{DBMatrixName.R00, DBMatrixName_AT.r00, model.index[0] },
				new List<object>{DBMatrixName.R01, DBMatrixName_AT.r01, model.index[1] },
				new List<object>{DBMatrixName.R02, DBMatrixName_AT.r02, model.index[2] },
				new List<object>{DBMatrixName.R03, DBMatrixName_AT.r03, model.index[3] },
				new List<object>{DBMatrixName.R10, DBMatrixName_AT.r10, model.index[4] },
				new List<object>{DBMatrixName.R11, DBMatrixName_AT.r11, model.index[5] },
				new List<object>{DBMatrixName.R12, DBMatrixName_AT.r12, model.index[6] },
				new List<object>{DBMatrixName.R13, DBMatrixName_AT.r13, model.index[7] },
				new List<object>{DBMatrixName.R20, DBMatrixName_AT.r20, model.index[8] },
				new List<object>{DBMatrixName.R21, DBMatrixName_AT.r21, model.index[9] },
				new List<object>{DBMatrixName.R22, DBMatrixName_AT.r22, model.index[10] },
				new List<object>{DBMatrixName.R23, DBMatrixName_AT.r23, model.index[11] },
				new List<object>{DBMatrixName.R30, DBMatrixName_AT.r30, model.index[12] },
				new List<object>{DBMatrixName.R31, DBMatrixName_AT.r31, model.index[13] },
				new List<object>{DBMatrixName.R32, DBMatrixName_AT.r32, model.index[14] },
				new List<object>{DBMatrixName.R33, DBMatrixName_AT.r33, model.index[15] }
			};

			return items;
		}

		public static void CreateTable(SQLiteCommand command)
        {
			StringBuilder builder = new StringBuilder();
			builder.Append(string.Format("CREATE TABLE IF NOT EXISTS '{0}' ( ", DBMatrixName.name));
			builder.Append(string.Format("'{0}' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, ", DBMatrixName.ID));
			builder.Append(string.Format("'{0}' REAL NOT NULL,", DBMatrixName.R00));
			builder.Append(string.Format("'{0}' REAL NOT NULL,", DBMatrixName.R01));
			builder.Append(string.Format("'{0}' REAL NOT NULL,", DBMatrixName.R02));
			builder.Append(string.Format("'{0}' REAL NOT NULL,", DBMatrixName.R03));
			builder.Append(string.Format("'{0}' REAL NOT NULL,", DBMatrixName.R10));
			builder.Append(string.Format("'{0}' REAL NOT NULL,", DBMatrixName.R11));
			builder.Append(string.Format("'{0}' REAL NOT NULL,", DBMatrixName.R12));
			builder.Append(string.Format("'{0}' REAL NOT NULL,", DBMatrixName.R13));
			builder.Append(string.Format("'{0}' REAL NOT NULL,", DBMatrixName.R20));
			builder.Append(string.Format("'{0}' REAL NOT NULL,", DBMatrixName.R21));
			builder.Append(string.Format("'{0}' REAL NOT NULL,", DBMatrixName.R22));
			builder.Append(string.Format("'{0}' REAL NOT NULL,", DBMatrixName.R23));
			builder.Append(string.Format("'{0}' REAL NOT NULL,", DBMatrixName.R30));
			builder.Append(string.Format("'{0}' REAL NOT NULL,", DBMatrixName.R31));
			builder.Append(string.Format("'{0}' REAL NOT NULL,", DBMatrixName.R32));
			builder.Append(string.Format("'{0}' REAL NOT NULL", DBMatrixName.R33));
			builder.Append(string.Format(");"));

			command.CommandText = builder.ToString();
		}

		public static void DeleteTable(SQLiteCommand command)
        {
			DBCommand.DeleteTable(DBMatrixName.name, command);
		}
    }

	class DBMatrixName
    {
		public static string name = "MATRIX3D";
		public static string ID = "ID"; //MUST BE THE SAME AS DBBLOCK
		public static string R00 = "R00";
		public static string R01 = "R01";
		public static string R02 = "R02";
		public static string R03 = "R03";
		public static string R10 = "R10";
		public static string R11 = "R11";
		public static string R12 = "R12";
		public static string R13 = "R13";
		public static string R20 = "R20";
		public static string R21 = "R21";
		public static string R22 = "R22";
		public static string R23 = "R23";
		public static string R30 = "R30";
		public static string R31 = "R31";
		public static string R32 = "R32";
		public static string R33 = "R33";
	}

	class DBMatrixName_AT
	{
		public static string name = "@name";
		public static string id = "@id"; //MUST BE THE SAME AS DBBLOCK
		public static string r00 = "@r00";
		public static string r01 = "@r01";
		public static string r02 = "@r02";
		public static string r03 = "@r03";
		public static string r10 = "@r10";
		public static string r11 = "@r11";
		public static string r12 = "@r12";
		public static string r13 = "@r13";
		public static string r20 = "@r20";
		public static string r21 = "@r21";
		public static string r22 = "@r22";
		public static string r23 = "@r23";
		public static string r30 = "@r30";
		public static string r31 = "@r31";
		public static string r32 = "@r32";
		public static string r33 = "@r33";
	}
}
