using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using ClassLibrary1.DATABASE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.HELPERS
{
    class FixtureDetails : ObjectData
    {
        FixtureDetailsModel fixtureDetails = new FixtureDetailsModel();

        public FixtureDetails(BlockReference bref, Transaction tr)
        {
            FillOutVariable(bref, tr);
        }

        private void FillOutVariable(BlockReference bref, Transaction tr)
        {
            fixtureDetails.position = new Point3dModel(bref.Position.ToArray());
            fixtureDetails.handle = bref.Handle.ToString();
            fixtureDetails.matrixTransform = new Matrix3dModel(bref.BlockTransform.ToArray());
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
                        fixtureDetails.index = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.number)
                    {
                        fixtureDetails.number = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.CW_DIA)
                    {
                        fixtureDetails.CW_DIA = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.HW_DIA)
                    {
                        fixtureDetails.HW_DIA = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.WASTE_DIA)
                    {
                        fixtureDetails.WASTE_DIA = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.VENT_DIA)
                    {
                        fixtureDetails.VENT_DIA = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.STORM_DIA)
                    {
                        fixtureDetails.STORM_DIA = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.WSFU)
                    {
                        fixtureDetails.WSFU = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.CWSFU)
                    {
                        fixtureDetails.CWSFU = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.HWSFU)
                    {
                        fixtureDetails.HWSFU = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.DFU)
                    {
                        fixtureDetails.DFU = number;
                    }
                }
                else if (aRef.Tag == FixtureDetailsName.tag)
                {
                    fixtureDetails.tag = textString;
                }
                else if (aRef.Tag == FixtureDetailsName.FixtureName)
                {
                    fixtureDetails.FixtureName = textString;
                }
                else if (aRef.Tag == FixtureDetailsName.DESCRIPTION)
                {
                    fixtureDetails.DESCRIPTION = textString;
                }
            }
        }

        public void UpdateFixtureDetail(Database db)
        {
            BlockReference bref = (BlockReference)Goodies.GetDBObjFromHandle(handle, db);
            if(bref != null && !bref.IsErased)
            {
                using(Transaction tr = db.TransactionManager.StartTransaction())
                {
                    FillOutVariable(bref, tr);
                }
            }
        }

        /// <summary>
        /// Update Database in manager folder
        /// </summary>
        /// <param name="db">drawing database</param>
        /// <param name=""></param>
        public void UpdateDatabaseManager(string path)
        {
            if (GoodiesPath.HasDwgPathInDatabase(path))
            {
                
            }
        }
    }



    
}
