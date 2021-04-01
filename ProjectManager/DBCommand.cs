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
            public const string isPNotes = "ISP_NOTES";

            public static string CreateFileTable()
            {
                return string.Format(@"CREATE TABLE IF NOT EXISTS '{0}' ('{1}' INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE, '{2}' TEXT UNIQUE, '{3}' INTEGER NOT NULL, '{4}' INTEGER);",
                    DBFileTable.name, DBFileTable.id, DBFileTable.path, DBFileTable.modDate, DBFileTable.isPNotes);
            }

            public static string InsertIntoFileTable(string relativePath, long modifiedDate, int isPNotes)
            {
                return string.Format(@"INSERT OR REPLACE INTO '{0}' ('{1}', '{2}', '{3}') VALUES ('{4}', '{5}', '{6}')",
                    DBFileTable.name, DBFileTable.path, DBFileTable.modDate, DBFileTable.isPNotes, relativePath, modifiedDate, isPNotes);
            }

            public static string GetPNoteStringFromTable()
            {
                //SELECT RELATIVE_PATH , MODIFIEDDATE FROM FILE WHERE ISP_NOTES = 1;
                return string.Format(@"SELECT {0}, {1} FROM {2} WHERE {3} = 1;", DBFileTable.path, DBFileTable.modDate, DBFileTable.name, DBFileTable.isPNotes);
            }

            public static string GetDwgsStringFromTable()
            {
                //SELECT RELATIVE_PATH , MODIFIEDDATE FROM FILE WHERE ISP_NOTES = 0;
                return string.Format(@"SELECT {0}, {1} FROM {2} WHERE {3} = 0;", DBFileTable.path, DBFileTable.modDate, DBFileTable.name, DBFileTable.isPNotes);
            }
        }

        //
        //FILE TABLE
        //


        public static string SelectAllFromATable(string fileTable)
        {
            return string.Format("SELECT * FROM '{0}'", fileTable);
        }

        public static string GetConnectionString(string dbFullPath)
        {
            return string.Format(@"data source = ""{0}"";PRAGMA journal_mode=WAL; PRAGMA foreign_keys = ON;", dbFullPath);
        }
    }
}
