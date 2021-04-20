using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DATABASE
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
		public static long DeleteRow(SQLiteConnection connection, int ID)
        {
			string commandStr = DBMatrix3dCommands.DeleteRow(ID);
			using (SQLiteCommand command = connection.CreateCommand())
            {
				command.CommandText = commandStr;
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
		public static long Update(SQLiteConnection connection, IList<double> matrix, int ID)
        {
			string commandStr = DBMatrix3dCommands.Update(ID, matrix);
			using (SQLiteCommand command = connection.CreateCommand())
			{
				command.CommandText = commandStr;
				long check = command.ExecuteNonQuery();
				if (check == 1)
				{
					return connection.LastInsertRowId;
				}
				else if (check == 0)
				{
					throw new Exception("DBMatrix3d -> Update -> No Row Is Update.");
				}
				throw new Exception("DBMatrix3d -> Update -> Update Is Not Successful.");
			}
		}
		public static long Insert(SQLiteConnection connection, IList<double> matrix)
        {
			string commandStr = DBMatrix3dCommands.Insert(matrix);
			using(SQLiteCommand command = connection.CreateCommand())
            {
				command.CommandText = commandStr;
				long check = command.ExecuteNonQuery();
				if(check == 1)
                {
					return connection.LastInsertRowId;
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
			string commandStr = DBMatrix3dCommands.DeleteTable();
			using(SQLiteCommand command = connection.CreateCommand())
            {
				command.CommandText = commandStr;
				command.ExecuteNonQuery();
            }
        }
		public static void CreateTable(SQLiteConnection connection)
        {
			string commandStr = DBMatrix3dCommands.CreateTable();
			using(SQLiteCommand command = connection.CreateCommand())
            {
				command.CommandText = commandStr;
				command.ExecuteNonQuery();
            }
        }
    }

	class DBMatrix3dCommands
    {
		public static string DeleteRow(int ID)
        {
			return string.Format("DELETE FROM '{0}' WHERE '{1}' = {2};", DBMatrixName.name, DBMatrixName.ID, ID);
        }
		public static string Update(int ID, IList<double> matrix)
        {
			string command = string.Format("UPDATE {0} SET ", DBMatrixName.name);
			command += string.Format("'{0}' = {1}, ", DBMatrixName.R00, matrix[0]);
			command += string.Format("'{0}' = {1}, ", DBMatrixName.R01, matrix[1]);
			command += string.Format("'{0}' = {1}, ", DBMatrixName.R02, matrix[2]);
			command += string.Format("'{0}' = {1}, ", DBMatrixName.R03, matrix[3]);
			command += string.Format("'{0}' = {1}, ", DBMatrixName.R10, matrix[4]);
			command += string.Format("'{0}' = {1}, ", DBMatrixName.R11, matrix[5]);
			command += string.Format("'{0}' = {1}, ", DBMatrixName.R12, matrix[6]);
			command += string.Format("'{0}' = {1}, ", DBMatrixName.R13, matrix[7]);
			command += string.Format("'{0}' = {1}, ", DBMatrixName.R20, matrix[8]);
			command += string.Format("'{0}' = {1}, ", DBMatrixName.R21, matrix[9]);
			command += string.Format("'{0}' = {1}, ", DBMatrixName.R22, matrix[10]);
			command += string.Format("'{0}' = {1}, ", DBMatrixName.R23, matrix[11]);
			command += string.Format("'{0}' = {1}, ", DBMatrixName.R30, matrix[12]);
			command += string.Format("'{0}' = {1}, ", DBMatrixName.R31, matrix[13]);
			command += string.Format("'{0}' = {1}, ", DBMatrixName.R32, matrix[14]);
			command += string.Format("'{0}' = {1}, ", DBMatrixName.R33, matrix[15]);
			command += string.Format("WHERE '{0}' = '{1}';", DBMatrixName.ID, ID);
			return command;
		}
		public static string Insert(IList<double> matrix)
        {
			string command = string.Format("INSERT INTO {0} ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}') VALUES ({17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30}, {31}, {32});",
					DBMatrixName.name,
					DBMatrixName.R00, DBMatrixName.R01, DBMatrixName.R02, DBMatrixName.R03,
					DBMatrixName.R10, DBMatrixName.R11, DBMatrixName.R12, DBMatrixName.R13,
					DBMatrixName.R20, DBMatrixName.R21, DBMatrixName.R22, DBMatrixName.R23,
					DBMatrixName.R30, DBMatrixName.R31, DBMatrixName.R32, DBMatrixName.R33,
					matrix[0], matrix[1], matrix[2], matrix[3],
					matrix[4], matrix[5], matrix[6], matrix[7],
					matrix[8], matrix[9], matrix[10], matrix[11],
					matrix[12], matrix[13], matrix[14], matrix[15]
				);
			return command;
		}
		public static string CreateTable()
        {
			string command =    string.Format("CREATE TABLE '{0}' ( ", DBMatrixName.name);
			command += string.Format("'{0}'	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, ", DBMatrixName.ID);
			command += string.Format("'{0}'	REAL NOT NULL,", DBMatrixName.R00);
			command += string.Format("'{0}'	REAL NOT NULL,", DBMatrixName.R01);
			command += string.Format("'{0}'	REAL NOT NULL,", DBMatrixName.R02);
			command += string.Format("'{0}'	REAL NOT NULL,", DBMatrixName.R03);
			command += string.Format("'{0}'	REAL NOT NULL,", DBMatrixName.R10);
			command += string.Format("'{0}'	REAL NOT NULL,", DBMatrixName.R11);
			command += string.Format("'{0}'	REAL NOT NULL,", DBMatrixName.R12);
			command += string.Format("'{0}'	REAL NOT NULL,", DBMatrixName.R13);
			command += string.Format("'{0}'	REAL NOT NULL,", DBMatrixName.R20);
			command += string.Format("'{0}'	REAL NOT NULL,", DBMatrixName.R21);
			command += string.Format("'{0}'	REAL NOT NULL,", DBMatrixName.R22);
			command += string.Format("'{0}'	REAL NOT NULL,", DBMatrixName.R23);
			command += string.Format("'{0}'	REAL NOT NULL,", DBMatrixName.R30);
			command += string.Format("'{0}'	REAL NOT NULL,", DBMatrixName.R31);
			command += string.Format("'{0}'	REAL NOT NULL,", DBMatrixName.R32);
			command += string.Format("'{0}'	REAL NOT NULL", DBMatrixName.R33);
			command += string.Format(");");

			return command;
		}

		public static string DeleteTable()
        {
			return string.Format("DROP TABLE IF EXISTS {0};", DBMatrixName.name);
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
