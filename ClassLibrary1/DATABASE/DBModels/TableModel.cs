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

        public void ReadFromDatabase(SQLiteConnection connection)
        {
            if(this.ID != ConstantName.invalidNum)
            {
                TableModel model = DBTable.SelectRow(connection, ID);
                if(model != null)
                {
                    CopyModel(model);
                }
                
            }else if(this.file.ID != ConstantName.invalidNum && handle != ConstantName.invalidStr)
            {
                TableModel model = DBTable.SelectRow()
            }
        }

        private void CopyModel(TableModel model)
        {
            base.CopyModel(model);
            ALIAS = model.ALIAS;
            A_VALUE = model.A_VALUE;
        }

        public void WriteToDatabase(SQLiteConnection connection)
        {
            matrixTransform.WriteToDatabase(connection);
            position.WriteToDatabase(connection);
            DBTable.CreateTable(connection);
            if(!DBTable.HasRow(connection, ID))
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
