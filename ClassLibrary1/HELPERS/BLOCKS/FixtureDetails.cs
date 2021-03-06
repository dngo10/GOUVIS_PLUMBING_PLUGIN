using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using GouvisPlumbingNew.DATABASE.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GouvisPlumbingNew.HELPERS
{
    public class FixtureDetails : ObjectData
    {
        public FixtureDetailsModel model = new FixtureDetailsModel();

        public FixtureDetails(BlockReference bref, Transaction tr)
        {
            FillOutVariable(bref, tr);
        }

        public FixtureDetails(FixtureDetailsModel model)
        {
            this.model = model;
        }

        private void FillOutVariable(BlockReference bref, Transaction tr)
        {
            model.position = new Point3dModel(bref.Position.ToArray());
            model.handle = Goodies.ConvertHandleToString(bref.Handle);
            model.matrixTransform = new Matrix3dModel(bref.BlockTransform.ToArray());

            foreach (ObjectId id in bref.AttributeCollection)
            {
                AttributeReference aRef = (AttributeReference)tr.GetObject(id, OpenMode.ForRead);
                string textString = aRef.TextString;
                double number;
                bool isNumber = double.TryParse(textString, out number);

                if (isNumber)
                {
                    if (aRef.Tag == FixtureDetailsName.index)
                    {
                        model.INDEX = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.CW_DIA)
                    {
                        model.CW_DIA = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.HW_DIA)
                    {
                        model.HW_DIA = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.WASTE_DIA)
                    {
                        model.WASTE_DIA = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.VENT_DIA)
                    {
                        model.VENT_DIA = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.STORM_DIA)
                    {
                        model.STORM_DIA = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.WSFU)
                    {
                        model.WSFU = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.CWSFU)
                    {
                        model.CWSFU = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.HWSFU)
                    {
                        model.HWSFU = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.DFU)
                    {
                        model.DFU = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.number)
                    {
                        model.NUMBER = textString;
                    }
                }
                else if (aRef.Tag == FixtureDetailsName.number)
                {
                    model.NUMBER = textString;
                }
                else if (aRef.Tag == FixtureDetailsName.tag)
                {
                    model.TAG = textString;
                }
                else if (aRef.Tag == FixtureDetailsName.FixtureName)
                {
                    model.FIXTURENAME = textString;
                }
                else if (aRef.Tag == FixtureDetailsName.DESCRIPTION)
                {
                    model.DESCRIPTION = textString;
                }
            }
        }
    }



    
}
