using GouvisPlumbingNew.DATABASE.DBModels;
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
		public static bool HasRow(SQLiteConnection connection, long ID)
        {
			long count = 0;
			using (SQLiteCommand command = connection.CreateCommand())
			{
				DBMatrix3dCommands.SelectCount(command, ID);
				count = Convert.ToInt64(command.ExecuteScalar());
			}
			return count == 1;
		}

		/// <summary>
		/// This Will return 1 model, if ID not found, will return null.
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="ID">ID of matrix in database</param>
		/// <returns></returns>
		public static Matrix3dModel SelectRow(SQLiteConnection connection, long ID)
        {
			Matrix3dModel model = null;
			using(SQLiteCommand command = connection.CreateCommand())
            {
				DBMatrix3dCommands.SelectRow(command, ID);
				SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
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

					double[] arr = new double[] { R00, R01, R02, R03, R10, R11, R12, R13, R20, R21, R22, R23, R30, R31, R32, R33 };
					model = new Matrix3dModel(arr, ID);
				}
				reader.Close();
            }
			return model;
		}
		public static long DeleteRow(SQLiteConnection connection, long ID)
        {
			using (SQLiteCommand command = connection.CreateCommand())
            {
				DBMatrix3dCommands.DeleteRow(command, ID);
				long check = command.ExecuteNonQuery();
				if (check == 1)
				{
					return connection.LastInsertRowId;
				}
				else if (check == 0)
				{
					throw new Exception("DBMatrix3d -> DeleteRow -> No Row is Deleted");
				}
				throw new Exception("DBMatrix3d -> DeleteRow -> Delete Row not successful");
			}
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
		public static void DropTable(SQLiteConnection connection)
        {
			using(SQLiteCommand command = connection.CreateCommand())
            {
				DBMatrix3dCommands.DeleteTable(command);
				command.ExecuteNonQuery();
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
		public static void SelectCount(SQLiteCommand command, long ID)
		{
			command.CommandText = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = @id;", DBMatrixName.name, DBMatrixName.ID);
			command.Parameters.Add(new SQLiteParameter("@id", ID));
		}
		public static void SelectRow(SQLiteCommand command, long ID)
        {
			command.CommandText = string.Format("SELECT * FROM {0} WHERE {1} = @id;", DBMatrixName.name, DBMatrixName.ID);
			command.Parameters.Add(new SQLiteParameter("@id", ID));
        }

		public static void DeleteRow(SQLiteCommand command, long ID)
        {
			command.CommandText = string.Format("DELETE FROM {0} WHERE {1} = @id;", DBMatrixName.name, DBMatrixName.ID);
			command.Parameters.Add(new SQLiteParameter("@id", ID));

        }
		public static void Update(SQLiteCommand command, Matrix3dModel model)
        {
			StringBuilder builder = new StringBuilder();
			builder.Append(string.Format("UPDATE {0} SET ", DBMatrixName.name));
			builder.Append(string.Format("'{0}' = @R00, ", DBMatrixName.R00));
			builder.Append(string.Format("'{0}' = @R01, ", DBMatrixName.R01));
			builder.Append(string.Format("'{0}' = @R02, ", DBMatrixName.R02));
			builder.Append(string.Format("'{0}' = @R03, ", DBMatrixName.R03));
			builder.Append(string.Format("'{0}' = @R10, ", DBMatrixName.R10));
			builder.Append(string.Format("'{0}' = @R11, ", DBMatrixName.R11));
			builder.Append(string.Format("'{0}' = @R12, ", DBMatrixName.R12));
			builder.Append(string.Format("'{0}' = @R13, ", DBMatrixName.R13));
			builder.Append(string.Format("'{0}' = @R20, ", DBMatrixName.R20));
			builder.Append(string.Format("'{0}' = @R21, ", DBMatrixName.R21));
			builder.Append(string.Format("'{0}' = @R22, ", DBMatrixName.R22));
			builder.Append(string.Format("'{0}' = @R23, ", DBMatrixName.R23));
			builder.Append(string.Format("'{0}' = @R30, ", DBMatrixName.R30));
			builder.Append(string.Format("'{0}' = @R31, ", DBMatrixName.R31));
			builder.Append(string.Format("'{0}' = @R32, ", DBMatrixName.R32));
			builder.Append(string.Format("'{0}' = @R33, ", DBMatrixName.R33));
			builder.Append(string.Format("WHERE '{0}' = @id;", DBMatrixName.ID));

			command.CommandText = builder.ToString();
			command.Parameters.Add(new SQLiteParameter("@R00", model.index[0]));
			command.Parameters.Add(new SQLiteParameter("@R01", model.index[1]));
			command.Parameters.Add(new SQLiteParameter("@R02", model.index[2]));
			command.Parameters.Add(new SQLiteParameter("@R03", model.index[3]));
			command.Parameters.Add(new SQLiteParameter("@R10", model.index[4]));
			command.Parameters.Add(new SQLiteParameter("@R11", model.index[5]));
			command.Parameters.Add(new SQLiteParameter("@R12", model.index[6]));
			command.Parameters.Add(new SQLiteParameter("@R13", model.index[7]));
			command.Parameters.Add(new SQLiteParameter("@R20", model.index[8]));
			command.Parameters.Add(new SQLiteParameter("@R21", model.index[9]));
			command.Parameters.Add(new SQLiteParameter("@R22", model.index[10]));
			command.Parameters.Add(new SQLiteParameter("@R23", model.index[11]));
			command.Parameters.Add(new SQLiteParameter("@R30", model.index[12]));
			command.Parameters.Add(new SQLiteParameter("@R31", model.index[13]));
			command.Parameters.Add(new SQLiteParameter("@R32", model.index[14]));
			command.Parameters.Add(new SQLiteParameter("@R33", model.index[15]));
			command.Parameters.Add(new SQLiteParameter("@id", model.ID));


		}
		public static void Insert(SQLiteCommand command, Matrix3dModel model)
        {
			string commandStr = string.Format("INSERT INTO {0} ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}') VALUES (@R00, @R01, @R02, @R03, @R10, @R11, @R12, @R13, @R20, @R21, @R22, @R23, @R30, @R31, @R32, @R33);",
					DBMatrixName.name,
					DBMatrixName.R00, DBMatrixName.R01, DBMatrixName.R02, DBMatrixName.R03,
					DBMatrixName.R10, DBMatrixName.R11, DBMatrixName.R12, DBMatrixName.R13,
					DBMatrixName.R20, DBMatrixName.R21, DBMatrixName.R22, DBMatrixName.R23,
					DBMatrixName.R30, DBMatrixName.R31, DBMatrixName.R32, DBMatrixName.R33
				);

			command.CommandText = commandStr;
			command.Parameters.Add(new SQLiteParameter("@R00", model.index[0]));
			command.Parameters.Add(new SQLiteParameter("@R01", model.index[1]));
			command.Parameters.Add(new SQLiteParameter("@R02", model.index[2]));
			command.Parameters.Add(new SQLiteParameter("@R03", model.index[3]));
			command.Parameters.Add(new SQLiteParameter("@R10", model.index[4]));
			command.Parameters.Add(new SQLiteParameter("@R11", model.index[5]));
			command.Parameters.Add(new SQLiteParameter("@R12", model.index[6]));
			command.Parameters.Add(new SQLiteParameter("@R13", model.index[7]));
			command.Parameters.Add(new SQLiteParameter("@R20", model.index[8]));
			command.Parameters.Add(new SQLiteParameter("@R21", model.index[9]));
			command.Parameters.Add(new SQLiteParameter("@R22", model.index[10]));
			command.Parameters.Add(new SQLiteParameter("@R23", model.index[11]));
			command.Parameters.Add(new SQLiteParameter("@R30", model.index[12]));
			command.Parameters.Add(new SQLiteParameter("@R31", model.index[13]));
			command.Parameters.Add(new SQLiteParameter("@R32", model.index[14]));
			command.Parameters.Add(new SQLiteParameter("@R33", model.index[15]));
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
			command.CommandText = string.Format("DROP TABLE IF EXISTS {0};", DBMatrixName.name);
		}
    }

	class DBMatrixName
    {
		public static string name = "MATRIX3D";
		public static string ID = "ID";
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
}
