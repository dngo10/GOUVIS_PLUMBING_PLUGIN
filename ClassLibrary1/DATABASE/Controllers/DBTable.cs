using GouvisPlumbingNew.DATABASE.Controllers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1.DATABASE.DBModels;

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
    }

    class DBTableCommands
    {
		public void UpdateRow(SQLiteCommand command, TableModel model)
        {
			StringBuilder builder = new StringBuilder();
			builder.Append("UPDATE TABLE {} SET ");
			builder.Append("")
        }

		public void InsertRow(SQLiteCommand command, TableModel model)
        {
			string commandStr = string.Format("INSERT INTO '{0}' ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}') VALUES (@handle, @position, @transform, @file, @alias, @a_value);",
				DBTableName.name,
				DBTableName.HANDLE,
				DBTableName.POSITION_ID,
				DBTableName.TRANSFORM_ID,
				DBTableName.FILE_ID,
				DBTableName.ALIAS,
				DBTableName.A_VALUE
				);

			command.CommandText = commandStr;
			command.Parameters.Add(new SQLiteParameter("@handle", model.HANDLE));
			command.Parameters.Add(new SQLiteParameter("@position", model.position.ID));
			command.Parameters.Add(new SQLiteParameter("@transform", model.matrixTransform.ID));
			command.Parameters.Add(new SQLiteParameter("@file", model.file.ID));
			command.Parameters.Add(new SQLiteParameter("@alias", model.ALIAS));
			command.Parameters.Add(new SQLiteParameter("@a_value", model.A_VALUE));
		}
		public void CreateTable(SQLiteCommand command)
        {
			StringBuilder builder = new StringBuilder();
			builder.Append($"CREATE TABLE {DBTableName.name}( ");
			builder.Append($"'{DBTableName.ID}' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, ");
			builder.Append($"'{DBTableName.HANDLE}' TEXT NOT NULL, ");
			builder.Append($"'{DBTableName.POSITION_ID}' INTEGER NOT NULL, ");
			builder.Append($"'{DBTableName.TRANSFORM_ID}' INTEGER NOT NULL, ");
			builder.Append($"'{DBTableName.FILE_ID}' INTEGER NOT NULL, ");
			builder.Append($"'{DBTableName.ALIAS}' TEXT, ");
			builder.Append($"'{DBTableName.A_VALUE}' TEXT, ");
			builder.Append($"FOREIGN KEY('{DBTableName.POSITION_ID}') REFERENCES '{DBPoint3DName.tableName}'('{DBPoint3DName.ID}') ON DELETE CASCADE, ");
			builder.Append($"FOREIGN KEY('{DBTableName.FILE_ID}') REFERENCES '{DBDwgFileName.name}'('{DBDwgFileName.ID}') ON DELETE CASCADE, ");
			builder.Append($"FOREIGN KEY('{DBTableName.TRANSFORM_ID}') REFERENCES '{DBMatrixName.name}'('{DBMatrixName.ID}') ON DELETE CASCADE ");
			builder.Append($");");

			command.CommandText = builder.ToString();
		}
    }


    class DBTableName
    {
		public const string name = "TABLE";
		public const string ID = "ID";
		public const string HANDLE = "HANDLE";
		public const string POSITION_ID = "POSITION_ID";
		public const string TRANSFORM_ID = "TRANSFORM_ID";
		public const string FILE_ID = "FILE_ID";
		public const string ALIAS = "ALIAS";
		public const string A_VALUE = "A_VALUE";
    }
}


