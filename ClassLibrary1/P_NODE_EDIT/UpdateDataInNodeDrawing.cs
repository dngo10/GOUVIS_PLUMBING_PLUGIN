using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using ClassLibrary1.HELPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClassLibrary1.P_NODE_EDIT
{
    class UpdateDataInNodeDrawing
    {

        //THIS WILL UPDATE THE HELL OUT OF NODE DRAWING.
        //YOU MUST OPEN NODE DRAWING
        public void UpdateNodeDrawing(NODEDWGDATA nodeData)
        {
            Regex rx = new Regex(ConstantName.fsPattern);

            Document doc = Application.DocumentManager.MdiActiveDocument;
            //if(Goodies.CanWriteDwgFile(nodeDrawingFile, out doc))
            //{
                using (Database db = doc.Database)
                {
                    using(Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        foreach(InsertPoint ip in nodeData.InsertPointSet)
                        {
                            if (rx.IsMatch(ip.ALIAS))
                            {
                                if (ip.UpdateHandle(db))
                                {
                                    Table T = TableSchedule.CreateTable(nodeData.FixtureDetailSet, ip);
                                    T.Layer = "0";
                                    db.AddDBObject(T);
                                    tr.AddNewlyCreatedDBObject(T, true);
                                    tr.Commit();
                                }
                            }
                        }           
                    }
                }
            //}
        }

        //This will update Table Schedule.
        public void UpdateTableSchedule(NODEDWG nodeData)
        {
        }
    }
}
