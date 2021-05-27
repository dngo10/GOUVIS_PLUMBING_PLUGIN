using GouvisPlumbingNew.DATABASE.Controllers;
using GouvisPlumbingNew.DATABASE.DBModels;
using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DATABASE.DBModels.BaseBlockModel
{
    abstract class BlockModelBase
    {
        public long ID = ConstantName.invalidNum;
        public string handle = ConstantName.invalidStr;
        public Point3dModel position = null;
        public Matrix3dModel matrixTransform = null;
        public DwgFileModel file = null;

        protected virtual void CopyModel(BlockModelBase model)
        {
            ID = model.ID;
            handle = model.handle;
            position = model.position;
            matrixTransform = model.matrixTransform;
            file = model.file;
        }

        protected virtual void WriteToDatabase0(SQLiteConnection connection)
        {
            position.WriteToDatabase(connection);
            matrixTransform.WriteToDatabase(connection);
        }

        /// <summary>
        /// Get ID from database FROM DATABASE, just to get ID.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="position"></param>
        /// <param name="connection"></param>
        protected virtual void UpdateToDatabase0(long DBMatrixID, long DBPositionID, SQLiteConnection connection)
        {
            this.position.ID = DBMatrixID;
            this.matrixTransform.ID = DBPositionID;

            DBPoint3D.UpdateRow(position, connection);
            DBMatrix3d.Update(connection, matrixTransform);
        }
    }
}
