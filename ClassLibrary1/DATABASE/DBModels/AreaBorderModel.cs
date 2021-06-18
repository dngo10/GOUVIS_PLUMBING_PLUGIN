using ClassLibrary1.DATABASE.DBModels.BaseBlockModel;
using GouvisPlumbingNew.DATABASE.DBModels;
using GouvisPlumbingNew.HELPERS;
using ClassLibrary1.DATABASE.Controllers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GouvisPlumbingNew.DATABASE.Controllers;

namespace ClassLibrary1.DATABASE.DBModels
{
    class AreaBorderModel : BlockModelBase
    {
        public string type = "";
        public string alias = "";

        public double X = ConstantName.invalidNum;
        public double Y = ConstantName.invalidNum;
        public Point3dModel origin = null;
        public Point3dModel pointTop = null;
        public Point3dModel pointBottom = null;

        public void WriteToDatabase(SQLiteConnection connection) {
            var temp = this;

            WriteToDatabase0(connection); 

            origin.WriteToDatabase(connection);
            pointBottom.WriteToDatabase(connection);
            pointTop.WriteToDatabase(connection);

            if (!DBAreaBorder.HasRow(connection, handle, file.ID))
            {
                WriteToDatabase0(connection);
                origin.ID = DBPoint3D.InsertRow(ref origin, connection);
                pointTop.ID = DBPoint3D.InsertRow(ref pointTop, connection);
                pointBottom.ID = DBPoint3D.InsertRow(ref pointBottom, connection);
            }
            else
            {
                AreaBorderModel model = DBAreaBorder.SelectRow(connection, handle, file.ID);
                ID = model.ID;

                UpdateToDatabase0(model.matrixTransform.ID, model.position.ID, connection);

                origin.ID = model.ID;
                pointTop.ID = model.ID;
                pointBottom.ID = model.ID;

                DBPoint3D.UpdateRow(origin, connection);
                DBPoint3D.UpdateRow(pointTop, connection);
                DBPoint3D.UpdateRow(pointBottom, connection);

                DBAreaBorder.UpdateRow(connection, this);
            }
        }
    }

    class AreaBroderModelName
    {
        public const string type = "TYPE";
        public const string alias = "ALIAS";
    }
}
