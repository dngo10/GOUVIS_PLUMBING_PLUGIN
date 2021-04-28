using GouvisPlumbingNew.DATABASE.Controllers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GouvisPlumbingNew.DATABASE.DBModels
{
    class InsertPointModel
    {
        public string alias;
        public string name;

        public long ID;
        public DwgFileModel file;
        public string handle;
        public Point3dModel position;
        public Matrix3dModel matrixTransform;

        public InsertPointModel(string alias, string name, long ID, DwgFileModel file, string handle, Point3dModel position, Matrix3dModel matrixTransform)
        {
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
            position.WriteToDatabase(connection);
            matrixTransform.WriteToDatabase(connection);

            //File must be inserted to Database first (meaning it must have ID).

            if(!DBInsertPoint.HasRow(connection, ID))
            {
                var temp = this;
                ID = DBInsertPoint.InsertRow(ref temp, connection);
            }
            else
            {
                DBInsertPoint.UpdateRow(this, connection);
            }

        }
    }
}
