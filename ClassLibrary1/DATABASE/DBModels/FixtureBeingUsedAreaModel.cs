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
    class FixtureBeingUsedAreaModel
    {
        public double X = ConstantName.invalidNum;
        public double Y = ConstantName.invalidNum;
        public Point3dModel origin = null;
        public Point3dModel pointTop = null;
        public Point3dModel pointBottom = null;
        public string handle = ConstantName.invalidStr;
        public long ID = ConstantName.invalidNum;
        public Point3dModel position = null;
        public Matrix3dModel matrixTransform = null;
        public DwgFileModel file = null;

        public void WriteToDataBase(SQLiteConnection connection)
        {
            origin.WriteToDatabase(connection);
            pointTop.WriteToDatabase(connection);
            pointBottom.WriteToDatabase(connection);
            position.WriteToDatabase(connection);

            matrixTransform.WriteToDatabase(connection);

            //File must be inserted to Database first (meaning it must have ID).

            if(!DBFixtureBeingUsedArea.HasRow(connection, ID))
            {
                var temp = this;
                ID = DBFixtureBeingUsedArea.InsertRow(connection, ref temp);
            }
            else
            {
                DBFixtureBeingUsedArea.UpdateRow(connection, this);
            }
        }

        public void UpdateToDataBase(SQLiteConnection connection)
        {
            DBPoint3D.UpdateRow(origin, connection);
            DBPoint3D.UpdateRow(pointTop, connection);
            DBPoint3D.UpdateRow(pointBottom, connection);
            DBPoint3D.UpdateRow(position, connection);

            DBMatrix3d.Update(connection, matrixTransform);
            DBFixtureBeingUsedArea.UpdateRow(connection, this);
        }
    }
}
