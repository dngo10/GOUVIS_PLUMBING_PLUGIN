using ClassLibrary1.DATABASE.Controllers.BlockInterFace;
using GouvisPlumbingNew.DATABASE.DBModels;
using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GouvisPlumbingNew.DATABASE.Controllers
{
    //THIS CLASS STORE FUNCTIONS TO CREATE DATABASE QUERIES
    class DBCommand
    {
        //NEW COMMAND:
        //SELECT * FROM 'DB_TABLE' WHERE HANDLE = "1FE61" AND 'FILE_ID' IN (SELECT 'FILE_ID' FROM 'DB_FILE' WHERE RELATIVE_PATH = "\TEMPLATE_FILE_V1_p_notes.dwg" LIMIT 1); 
        public static string GetConnectionString(string dbFullPath)
        {
            return string.Format(@"data source = ""{0}"";PRAGMA journal_mode=WAL; PRAGMA foreign_keys = ON;", dbFullPath);
        }

        public static void DeleteTable(string tableName, SQLiteCommand command)
        {
            command.CommandText = $"DROP TABLE IF EXISTS {tableName}; ";
        }

        public static void DeleteRow(string tableName, Dictionary<string, string> conditions, Dictionary<string, object> paraDict, SQLiteCommand command)
        {
            string conStr = CreateConditionString(conditions);
            command.CommandText = $"DELETE FROM '{tableName}' WHERE {conStr}; ";
            SetCommandParameter(command, paraDict);
        }

        public static void DeleteRow(string tableName, long ID, SQLiteCommand command)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string>
            {
                {DBBlockName.ID, DBBlockName_AT.id}
            };

            Dictionary<string, object> paraDict = new Dictionary<string, object>
            {
                {DBBlockName_AT.id, ID}
            };

            DeleteRow(tableName, conDict, paraDict, command);
        }

        public static void DeleteRow(string tableName, string handle, long File_ID, SQLiteCommand command)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string>
            {
                {DBBlockName.HANDLE, DBBlockName_AT.handle},
                {DBBlockName.FILE_ID, DBBlockName_AT.file }
            };

            Dictionary<string, object> paraDict = new Dictionary<string, object>
            {
                {DBBlockName_AT.handle, handle},
                {DBBlockName_AT.file, File_ID}
            };

            DeleteRow(tableName, conDict, paraDict, command);
        }

        public static void DeleteRow(string tableName, string handle, string relPath, SQLiteCommand command)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string>
            {
                {DBBlockName.HANDLE, DBBlockName_AT.handle}
            };

            Dictionary<string, object> paraDict = new Dictionary<string, object>
            {
                {DBBlockName_AT.handle, handle},
                {DBDwgFileName_AT.relPath, relPath}
            };

            string conStr = CreateConditionString(conDict);

            string cmdStr = $"DELETE FROM '{tableName}' WHERE {conStr} AND {SelectFileRow()} ;";

            command.CommandText = cmdStr;
            SetCommandParameter(command, paraDict);
        }



        public static void SelectCount(string tableName, string handle, string relPath, SQLiteCommand command)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string>
            {
                {DBBlockName.HANDLE, DBBlockName_AT.handle}
            };

            Dictionary<string, object> paraDict = new Dictionary<string, object>
            {
                {DBBlockName_AT.handle, handle},
                {DBDwgFileName_AT.relPath, relPath}
            };

            string conStr = CreateConditionString(conDict);

            string cmdStr = $"SELECT COUNT(*) FROM '{tableName}' WHERE {conStr} AND {SelectFileRow()} ;";

            command.CommandText = cmdStr;
            SetCommandParameter(command, paraDict);
        }

        public static void SelectCount(string tableName, string handle, long file_ID, SQLiteCommand command)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string>
            {
                {DBBlockName.HANDLE, DBBlockName_AT.handle},
                {DBBlockName.FILE_ID, DBBlockName_AT.file }
            };

            Dictionary<string, object> paraDict = new Dictionary<string, object>
            {
                {DBBlockName_AT.handle, handle},
                {DBBlockName_AT.file, file_ID}
            };

            SelectCount(tableName, conDict, paraDict, command);
        }

        public static void SelectCount(string tableName, long ID, SQLiteCommand command)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string>
            {
                {DBBlockName.ID, DBBlockName_AT.id}
            };

            Dictionary<string, object> paraDict = new Dictionary<string, object>
            {
                {DBBlockName_AT.id, ID}
            };

            SelectCount(tableName, conDict, paraDict, command);
        }

        public static void SelectRows(string tableName, string relPath, SQLiteCommand command)
        {
            Dictionary<string, object> paraDict = new Dictionary<string, object>
            {
                {DBDwgFileName_AT.relPath, relPath}
            };

            string cmdStr = $"SELECT * FROM {tableName} WHERE {SelectFileRow()};";
            command.CommandText = cmdStr;
            SetCommandParameter(command, paraDict);
        }

        public static void SelectRows(string tableName, long file_ID, SQLiteCommand command)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string>
            {
                {DBBlockName.FILE_ID, DBBlockName_AT.file}
            };

            Dictionary<string, object> paraDict = new Dictionary<string, object>
            {
                {DBBlockName_AT.file, file_ID}
            };

            SelectRow(tableName, conDict, paraDict, command);
        }

        public static void SelectRow(string tableName, long ID, SQLiteCommand command)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string>
            {
                {DBBlockName.ID, DBBlockName_AT.id}
            };

            Dictionary<string, object> paraDict = new Dictionary<string, object>
            {
                {DBBlockName_AT.id, ID}
            };

            SelectRow(tableName, conDict, paraDict, command);
        }

        public static void SelectRow(string tableName, string handle, long file_ID, SQLiteCommand command)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string>
            {
                {DBBlockName.HANDLE, DBBlockName_AT.handle},
                {DBBlockName.FILE_ID, DBBlockName_AT.file }
            };

            Dictionary<string, object> paraDict = new Dictionary<string, object>
            {
                {DBBlockName_AT.handle, handle},
                {DBBlockName_AT.file, file_ID}
            };

            SelectRow(tableName, conDict, paraDict, command);
        }

        public static void SelectRow(string tableName, string handle, string relPath, SQLiteCommand command)
        {
            Dictionary<string, string> conDict = new Dictionary<string, string>
            {
                {DBBlockName.HANDLE, DBBlockName_AT.handle}
            };

            Dictionary<string, object> paraDict = new Dictionary<string, object>
            {
                {DBBlockName_AT.handle, handle},
                {DBDwgFileName_AT.relPath, relPath}
            };

            string conStr = CreateConditionString(conDict);

            string cmdStr = $"SELECT * FROM '{tableName}' WHERE {conStr} AND {SelectFileRow()} ;";

            command.CommandText = cmdStr;
            SetCommandParameter(command, paraDict);
        }

        /// <summary>
        /// Return a part of command form checkin File.
        /// </summary>
        /// <param name="relPath"></param>
        /// <returns></returns>
        private static string SelectFileRow()
        {
            return $"{DBBlockName.FILE_ID} IN (SELECT '{DBBlockName.FILE_ID}') FROM '{DBDwgFileName.name}' WHERE {DBDwgFileName.RELATIVE_PATH} = {DBDwgFileName_AT.relPath} LIMIT 1)";
        }

        public static void SelectRow(string tableName,  Dictionary<string, string> conditions, Dictionary<string, object> paraDict, SQLiteCommand command)
        {
            string conStr = CreateConditionString(conditions);

            command.CommandText = $"SELECT * FROM '{tableName}' WHERE {conStr};";
            SetCommandParameter(command, paraDict);
        }

        public static void SelectCount(string tableName, Dictionary<string, string> conditions, Dictionary<string, object> paraDict, SQLiteCommand command)
        {
            string conStr = CreateConditionString(conditions);

            command.CommandText = $"SELECT COUNT(*) FROM '{tableName}' WHERE {conStr} ;";
            SetCommandParameter(command, paraDict);
        }

        public static void InsertCommand(string tableName,
                   List<string> variables,
                   Dictionary<string, object> paraDict,
                   SQLiteCommand command
                   )
        {
            string vari = string.Join(" , ", variables);
            string atIndi = string.Join(" , ", paraDict.Keys.ToList());

            command.CommandText = $"INSERT INTO '{tableName}' ({vari}) VALUES ({atIndi}) ;";
            SetCommandParameter(command, paraDict);
        }

        public static void UpdateRow(string tableName,
                Dictionary<string, string> variables,
                Dictionary<string, string> conditions,
                Dictionary<string, object> paraDict,
                SQLiteCommand command)
        {
            List<string> subCommand = new List<string>();
            foreach(KeyValuePair<string, string> kv in variables)
            {
                subCommand.Add($" {kv.Key} = {kv.Value} ");
            }
            string s_command = string.Join(" , ", subCommand);
            string s_condition = CreateConditionString(conditions);
            command.CommandText = $"UPDATE '{tableName}' SET {s_command} WHERE {s_condition} ; ";
            SetCommandParameter(command, paraDict);
        }

        public static void SetCommandParameter(SQLiteCommand command, Dictionary<string, object> parameters)
        {
            foreach(KeyValuePair<string, object> kv in parameters)
            {
                command.Parameters.Add(new SQLiteParameter(kv.Key, kv.Value));
            }
        }

        private static string CreateConditionString(Dictionary<string, string> conditions)
        {
            List<string> terms = new List<string>();

            foreach (KeyValuePair<string, string> kv in conditions)
            {
                terms.Add($" {kv.Key}={kv.Value} ");                
            }

            string conStr = string.Join(" AND ", terms);
            return conStr;
        }
    }
}
