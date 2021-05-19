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
