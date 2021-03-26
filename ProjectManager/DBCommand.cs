using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager
{
    //THIS CLASS STORE FUNCTIONS TO CREATE DATABASE QUERIES
    class DBCommand
    {
        public class DBFileTable
        {
            public const string name = "FILE";
            public const string path = "RELATIVE_PATH";
            public const string modDate = "MODIFIEDDATE";
            public const string id = "ID";
        }


        public static string CreateFileTable()
        {
            return string.Format(@"CREATE TABLE IF NOT EXISTS '{0}'('{1}' INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE, '{2}' TEXT UNIQUE, '{3}');", 
                DBFileTable.name, DBFileTable.id, DBFileTable.path, DBFileTable.modDate);
        }

        public static string InsertToFileTable(string relativePath, string modifiedDate)
        {
            return string.Format(@"INSERT OR REPLACE INTO '{0}' ('{1}' , '{2}') VALUES ('{3}', '{4}')",
                DBFileTable.name, DBFileTable.path, DBFileTable.modDate, relativePath, modifiedDate);
        }

        public static string GetConnectionString(string dbFullPath)
        {
            return string.Format(@"data source = ""{0}"";PRAGMA journal_mode=WAL; PRAGMA foreign_keys = ON;", dbFullPath);
        }
    }
}
