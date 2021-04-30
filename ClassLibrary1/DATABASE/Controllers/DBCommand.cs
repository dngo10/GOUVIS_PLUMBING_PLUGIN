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
    }
}
