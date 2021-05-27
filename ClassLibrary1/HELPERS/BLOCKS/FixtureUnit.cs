using Autodesk.AutoCAD.DatabaseServices;
using ClassLibrary1.DATABASE.DBModels;
using GouvisPlumbingNew.DATABASE.DBModels;
using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.HELPERS.BLOCKS
{
    class FixtureUnit
    {
        public FixtureUnitModel model;

        public FixtureUnit(BlockReference bRef, Transaction tr)
        {
            model = new FixtureUnitModel();
            model.position = new Point3dModel(bRef.Position.ToArray());
            model.matrixTransform = new Matrix3dModel(bRef.BlockTransform.ToArray());
            model.blockName = Goodies.GetDynamicName(bRef);

            foreach(DynamicBlockReferenceProperty prop in bRef.DynamicBlockReferencePropertyCollection)
            {
                if(prop.PropertyName == "")
                {

                }
            }
        }
    }
}
