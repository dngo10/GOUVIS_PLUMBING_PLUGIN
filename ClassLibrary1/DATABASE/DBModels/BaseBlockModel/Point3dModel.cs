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
    class Point3dModel
    {
        public double X = ConstantName.invalidNum;
        public double Y = ConstantName.invalidNum;
        public double Z = ConstantName.invalidNum;
        public long ID  = ConstantName.invalidNum;

        public Point3dModel(double X, double Y, double Z, long ID = ConstantName.invalidNum)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.ID = ID;
        }

        public Point3dModel(IList<double> args, long ID = ConstantName.invalidNum)
        {
            X = args[0];
            Y = args[1];
            Z = args[2];
            this.ID = ID;
        }

        public void WriteToDatabase(SQLiteConnection connection)
        {
            if (!DBPoint3D.HasRow(connection, ID))
            {
                var temp = this;
                ID = DBPoint3D.InsertRow(ref temp, connection);
            }
            else
            {
                DBPoint3D.UpdateRow(this, connection);
            }
        }

        public void ReadFromDatabase(SQLiteConnection connection, long ID)
        {
            Point3dModel point3dM = DBPoint3D.SelectRow(connection, ID);
            if(point3dM != null)
            {
                X = point3dM.X;
                Y = point3dM.Y;
                Z = point3dM.Z;
                this.ID = point3dM.ID;
            }
        }
    }
}
