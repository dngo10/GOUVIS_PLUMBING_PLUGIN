using GouvisPlumbingNew.DATABASE.Controllers;
using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DATABASE.Controllers.BlockInterFace
{

    class BlockGeneral<T>
    {

        public delegate T Del(SQLiteDataReader reader, SQLiteConnection connection);
        public Del getItem;
        public string tableName;

        public BlockGeneral()
        {}

        public BlockGeneral(Del function, string tableName)
        {
            this.getItem = function;
            this.tableName = tableName;
        }

        public List<T> SelectRows(string relPath, SQLiteConnection connection)
        {
            List<T> items = new List<T>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBCommand.SelectRows(tableName, relPath, command);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    T item = getItem(reader, connection);
                    items.Add(item);
                }
            }
            return items;
        }

        public List<T> SelectRows(long fileID, SQLiteConnection connection)
        {
            List<T> items = new List<T>();
            using(SQLiteCommand command = connection.CreateCommand())
            {
                DBCommand.SelectRows(tableName, fileID, command);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    T item = getItem(reader, connection);
                    items.Add(item);
                }
            }
            return items;
        }

        public List<T> SelectRows(Dictionary<string, string> conDict, Dictionary<string, object> paraDict, SQLiteConnection connection)
        {
            List<T> items = new List<T>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBCommand.SelectRow(tableName, conDict, paraDict, command);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    T item = getItem(reader, connection);
                    items.Add(item);
                }
            }
            return items;
        }

        public T SelectRow(string handle, string relPath, SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                if (HasRow(handle, relPath, connection))
                {
                    DBCommand.SelectRow(tableName, handle, relPath, command);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        return getItem(reader, connection);
                    }        
                }
                else
                {
                    throw new Exception("Can't get object");
                }
            }
            return default(T);
        }

        public T SelectRow(string handle, long fileID, SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                if (HasRow(handle, fileID, connection))
                {
                    DBCommand.SelectRow(tableName, handle, fileID, command);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        return getItem(reader, connection);
                    }
                }
                else
                {
                    throw new Exception("Can't get object");
                }
            }
            return default(T);
        }
        public T SelectRow(long ID, SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                if(HasRow(ID, connection))
                {
                    DBCommand.SelectRow(tableName, ID, command);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        return getItem(reader, connection);
                    }
                }
                else
                {
                    throw new Exception("Can't get object");
                }
            }
            return default(T);
        }

        public T SelectRow(Dictionary<string, string> conDict, Dictionary<string, object> paraDict, SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                if (HasRow(conDict, paraDict, connection))
                {
                    DBCommand.SelectRow(tableName, conDict, paraDict, command);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        return getItem(reader, connection);
                    }
                }
                else
                {
                    throw new Exception("Can't get object");
                }
            }
            return default(T);
        }



        public bool HasRow(string handle, string relFilePath, SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBCommand.SelectCount(tableName, handle, relFilePath, command);
                long count = (long)command.ExecuteScalar();
                return count == 1;
            }
        }

        public bool HasRow(string handle, long fileID, SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBCommand.SelectCount(tableName, handle, fileID, command);
                long count = (long)command.ExecuteScalar();
                return count == 1;
            }
        }

        public bool HasRow(long ID, SQLiteConnection connection)
        {
            using(SQLiteCommand command = connection.CreateCommand())
            {
                DBCommand.SelectCount(tableName, ID, command);
                long count = (long)command.ExecuteScalar();
                return count == 1;
            }
        }

        public bool HasRow(Dictionary<string, string> conDict, Dictionary<string, object> paraDict, SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBCommand.SelectCount(tableName, conDict, paraDict, command);
                long count = (long)command.ExecuteScalar();
                return count == 1;
            }
        }

        public void DeleteRow(long ID, SQLiteConnection connection)
        {
            using(SQLiteCommand command = connection.CreateCommand())
            {
                DBCommand.DeleteRow(tableName, ID, command);
                long check = command.ExecuteNonQuery();
            }
        }

        public void DeleteRow(string handle, long fileID, SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBCommand.DeleteRow(tableName, handle, fileID, command);
                long check = command.ExecuteNonQuery();
            }
        }

        public void DeleteRow(string handle, string relPath, SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBCommand.DeleteRow(tableName, handle, relPath, command);
                long check = command.ExecuteNonQuery();
            }
        }

        public void DeleteRow(Dictionary<string,string> conDict, Dictionary<string,object> paraDict, SQLiteConnection connection)
        {
            using(SQLiteCommand command = connection.CreateCommand())
            {
                DBCommand.DeleteRow(tableName, conDict, paraDict, command);
                long check = command.ExecuteNonQuery();
            }
        }

        public void DeleteTable(SQLiteConnection connection)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                DBCommand.DeleteTable(tableName, command);
                command.ExecuteNonQuery();
            }
        }
    }

    abstract class DBBlockName
    {
        public const string ID = "ID";
        public const string HANDLE = "HANDLE";
        public const string POSITION_ID = "POSITION_ID";
        public const string MATRIX_ID = "MATRIX_ID";
        public const string FILE_ID = "FILE_ID";
    }

    abstract class DBBlockName_AT
    {
        public const string name = "@name";
        public const string id = "@id";
        public const string handle = "@handle";
        public const string position = "@position";
        public const string matrix = "@matrix";
        public const string file = "@file";
    }
}
