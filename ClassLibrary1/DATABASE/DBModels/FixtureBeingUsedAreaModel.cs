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
    class FixtureBeingUsedAreaModel : BlockModelBase
    {
        public double X = ConstantName.invalidNum;
        public double Y = ConstantName.invalidNum;
        public Point3dModel origin = null;
        public Point3dModel pointTop = null;
        public Point3dModel pointBottom = null;

        public void WriteToDataBase(SQLiteConnection connection)
        {
            WriteToDatabase0(connection);

            origin.WriteToDatabase(connection);
            pointTop.WriteToDatabase(connection);
            pointBottom.WriteToDatabase(connection);

            //File must be inserted to Database first (meaning it must have ID).

            if(!DBFixtureBeingUsedArea.HasRow(connection, handle, file.ID))
            {
                var temp = this;
                ID = DBFixtureBeingUsedArea.InsertRow(connection, ref temp);
                WriteToDatabase0(connection);


                origin.WriteToDatabase(connection);
                pointTop.WriteToDatabase(connection);
                pointBottom.WriteToDatabase(connection);
            }
            else
            {
                FixtureBeingUsedAreaModel model = DBFixtureBeingUsedArea.SelectRow(connection, handle, file.ID);
                ID = model.ID;

                UpdateToDatabase0(model.matrixTransform.ID, model.position.ID, connection);

                origin.ID = model.origin.ID;
                pointTop.ID = model.pointTop.ID;
                pointBottom.ID = model.pointBottom.ID;

                origin.WriteToDatabase(connection);
                pointTop.WriteToDatabase(connection);
                pointBottom.WriteToDatabase(connection);

                DBFixtureBeingUsedArea.UpdateRow(connection, this);
            }
        }
    }
}
