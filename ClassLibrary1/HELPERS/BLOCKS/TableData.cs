using Autodesk.AutoCAD.DatabaseServices;
using ClassLibrary1.DATABASE.DBModels;
using GouvisPlumbingNew.DATABASE.DBModels;
using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.HELPERS
{
    class TableData
    {
        public TableModel model = new TableModel();

        public TableData( BlockReference bref, Transaction tr, Database db)
        {
            model.handle = Goodies.ConvertHandleToString(bref.Handle);
            model.position = new Point3dModel(bref.Position.ToArray());
            model.matrixTransform = new Matrix3dModel(bref.BlockTransform.ToArray());
            model.ALIAS = XDataHelper.GetTableType(bref, tr, db);

            if (string.IsNullOrEmpty(model.ALIAS))
            {
                model = null;
                return;
            }

            model.A_VALUE = ConstantName.invalidStr;
        }

        public TableData(TableModel model)
        {
            this.model = model;
        }
    }
}
