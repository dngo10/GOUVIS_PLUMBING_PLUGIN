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
    class Matrix3dModel
    {
        public List<double> index = null;
        public long ID = ConstantName.invalidNum;

        public Matrix3dModel(IList<double> index, long ID = ConstantName.invalidNum)
        {
            this.index = new List<double>(index);
            this.ID = ID;
        }

        public void WriteToDatabase(SQLiteConnection connection)
        {
            if(!DBMatrix3d.HasRow(connection, ID))
            {
                var temp = this;
                ID = DBMatrix3d.Insert(connection, ref temp);
            }
            else
            {
                DBMatrix3d.Update(connection, this);
            }
        }
    }
}
