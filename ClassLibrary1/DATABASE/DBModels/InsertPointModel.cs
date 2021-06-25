using ClassLibrary1.DATABASE.DBModels.BaseBlockModel;
using GouvisPlumbingNew.DATABASE.Controllers;
using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GouvisPlumbingNew.DATABASE.DBModels
{
    public class InsertPointModel : BlockModelBase
    {
        public string alias = ConstantName.invalidStr;
        public string name = ConstantName.invalidStr;

        public InsertPointModel(string alias, string name, long ID, DwgFileModel file, string Handle, Point3dModel position, Matrix3dModel matrixTransform)
        {
            this.handle = Handle;
            this.alias = alias;
            this.name = name;
            this.ID = ID;
            this.file = file;
            this.position = position;
            this.matrixTransform = matrixTransform;
        }

        public InsertPointModel()
        {

        }

        public void WriteToDataBase(SQLiteConnection connection)
        {
            //File must be inserted to Database first (meaning it must have ID).

            if(!DBInsertPoint.HasRow(connection, handle, file.ID))
            {
                var temp = this;
                WriteToDatabase0(connection);
                ID = DBInsertPoint.InsertRow(ref temp, connection);
            }
            else
            {
                InsertPointModel model = DBInsertPoint.SelectRow(connection, handle, file.ID);
                ID = model.ID;
                UpdateToDatabase0(model.matrixTransform.ID, model.position.ID, connection);
                DBInsertPoint.UpdateRow(connection, this);
            }
        }
    }
}
