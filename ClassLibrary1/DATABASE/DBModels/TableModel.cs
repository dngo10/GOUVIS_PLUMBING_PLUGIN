using ClassLibrary1.DATABASE.Controllers;
using ClassLibrary1.DATABASE.DBModels.BaseBlockModel;
using GouvisPlumbingNew.DATABASE.DBModels;
using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DATABASE.DBModels
{
    class TableModel : BlockModelBase
    {
        public string ALIAS = ConstantName.invalidStr;
        public string A_VALUE = ConstantName.invalidStr;

        public TableModel(){}

        public TableModel(string ALIAS)
        {
            this.ALIAS = ALIAS;
        }

        public void WriteToDatabase(SQLiteConnection connection)
        {
            matrixTransform.WriteToDatabase(connection);
            position.WriteToDatabase(connection);
            DBTable.CreateTable(connection);
            if(DBTable.HasRow(connection, ID))
            {
                DBTable.InsertRow(connection, this);
            }
            else
            {
                DBTable.UpdateRow(connection, this);
            }
            
        }
    }
}
