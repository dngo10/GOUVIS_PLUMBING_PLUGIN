using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GouvisPlumbingNew.P_NODE_EDIT
{
    class UpdateDataInNodeDrawing
    {


        //THIS WILL UPDATE THE HELL OUT OF NODE DRAWING.
        //YOU MUST OPEN NODE DRAWING
        public static void UpdateNodeDrawing(NODEDWGDATA nodeData)
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

        /// <summary>
        /// Update Node Drawing
        /// </summary>
        /// <param name="dwgpath">any dwgpath in the project, Note that it must be in the project</param>
        public static void UpdateNodeDrawing(string dwgpath) {
            string notePath = GoodiesPath.GetNotePathFromADwgPath(dwgpath);
            Document doc = Goodies.CanOpenToWrite(notePath);
            
            if(doc != null)
            {
                Application.DocumentManager.MdiActiveDocument = doc;
                using (doc.LockDocument())
                {

                }
            }
        }

        //This will update Table Schedule.
        public static void UpdateTableSchedule(NODEDWG nodeData)
        {
        }
    }
}
