using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GouvisPlumbingNew.DATABASE.DBModels;
using GouvisPlumbingNew.DATABASE.Controllers;

namespace GouvisPlumbingNew.HELPERS
{
    class InsertPoint : ObjectData
    {
        public InsertPointModel model;

        public InsertPoint(BlockReference bref, Transaction tr)
        {
            FilloutVariables(bref, tr);
        }

        public InsertPoint(InsertPointModel model)
        {
            this.model = model;
        }

        private void FilloutVariables(BlockReference bref, Transaction tr)
        {
            model = new InsertPointModel();
            model.handle = Goodies.ConvertHandleToString(bref.Handle);
            model.position = new Point3dModel(bref.Position.ToArray());
            model.matrixTransform = new Matrix3dModel(bref.BlockTransform.ToArray());
            foreach (ObjectId id in bref.AttributeCollection)
            {
                AttributeReference aRef = (AttributeReference)tr.GetObject(id, OpenMode.ForRead);
                if (aRef.Tag == InsertPointName.NAME)
                {
                    model.name = aRef.TextString;
                }
                else if (aRef.Tag == InsertPointName.ALIAS)
                {
                    model.alias = aRef.TextString;
                }
            }
        }
    }

    class InsertPointName
    {
        public static string NAME = "NAME";
        public static string ALIAS = "ALIAS";
    }
}
