//using Autodesk.AutoCAD.ApplicationServices;
//using Autodesk.AutoCAD.DatabaseServices;
//using Autodesk.AutoCAD.EditorInput;
//using Autodesk.AutoCAD.Geometry;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using GouvisPlumbingNew.HELPERS;
//using GouvisPlumbingNew.P_NODE_EDIT;
//using System.IO;
//using GouvisPlumbingNew.DATABASE.Controllers;
//
//namespace GouvisPluminbNew.P_NODE_EDIT
//{
//    class ReadInformationInNodeDrawing
//    {
//        /// <summary>
//        /// Run node file, select all fixtures in there to make Fixture Schedule Table.
//        /// There should be only one boxName block in P_NODE, the function will only select one.
//        /// </summary>
//        /// <param name="boxName">dynamic block. It's a box that all FixtureDetails are inside</param>
//        /// <param name="blockNameToBeSelected">In this case FixtureDetails block</param>
//        /// <param name="P_NodeFileDirectory">Location of P_NODE file</param>
//        public static NODEDWG FindAllFixturesBeingUsed(string pNotePath)
//        {
//
//            bool isActiveDoc = false;
//            if (!System.IO.File.Exists(pNotePath))
//            {
//                Console.WriteLine("ReadInformationInNodeDrawing->FindAllFixturesBeingUsed: ERR: pNotePath does not exist");
//                return null;
//            }
//                
//            if (!GoodiesPath.IsNotePath(pNotePath))
//            {
//                Console.WriteLine("ReadInformationInNodeDrawing->FindAllFixturesBeingUsed: ERR: pNotePath does not look like a *p_notes.dwg file");
//                return null;
//            }
//
//            Document nodeDoc = null;
//
//            if (pNotePath == Application.DocumentManager.MdiActiveDocument.Name)
//            {
//                nodeDoc = Application.DocumentManager.MdiActiveDocument;
//                isActiveDoc = true;
//            }else if (Goodies.GetListOfDocumentOpening().Contains(pNotePath))
//            {
//                nodeDoc = Application.DocumentManager.Open(pNotePath, true);
//            }else if(File.Exists(pNotePath) && GoodiesPath.IsNotePath(pNotePath) && !GoodiesPath.IsFileLocked(pNotePath))
//            {
//                nodeDoc = Application.DocumentManager.Open(pNotePath, true);
//            }
//
//            NODEDWG nodeP = new NODEDWG();
//
//            //Document nodeDoc = Application.DocumentManager.Open("C:\\Plumbing_Template\\TEMPLATE_FILE_V1.dwg", true);
//
//            using (nodeDoc.LockDocument())
//            {
//                Database nodeDb = nodeDoc.Database;
//
//                Dictionary<string,Layout> layoutDict =   LayoutEdit.GetLayout(nodeDb);
//
//                using (Transaction tr = nodeDb.TransactionManager.StartTransaction())
//                {
//                    BlockTable bt = (BlockTable)tr.GetObject(nodeDb.BlockTableId, OpenMode.ForRead);
//                    foreach (Layout layout in layoutDict.Values)
//                    {
//                        BlockTableRecord layoutBlkRec = (BlockTableRecord)tr.GetObject(layout.BlockTableRecordId, OpenMode.ForRead);
//                        foreach (ObjectId id in layoutBlkRec)
//                        {
//                            DBObject obj = tr.GetObject(id, OpenMode.ForRead);
//                            if (obj is BlockReference)
//                            {
//                                BlockReference bref = (BlockReference)obj;
//                                if (Goodies.IsBlockInTheDrawing(bref))
//                                {
//                                    if (bref.IsDynamicBlock)
//                                    {
//
//                                        string brefName = Goodies.GetDynamicName(bref, nodeDoc.Editor);
//                                        if (brefName.Equals(ConstantName.FixtureInformationArea))
//                                        {
//                                            FixtureBeingUsedArea dbA = new FixtureBeingUsedArea(bref);
//                                            nodeP.fixtureAreaSET.Add(dbA);
//                                        }
//                                    }
//                                    else if (bref.Name == ConstantName.FixtureDetailsBox)
//                                    {
//                                        FixtureDetails FD = new FixtureDetails(bref, tr);
//                                        nodeP.fixtureDetailSET.Add(FD);
//                                    }
//                                    else if (bref.Name == ConstantName.InsertPoint)
//                                    {
//                                        nodeP.insertionPointSET.Add(bref);
//                                    }
//                                    else if (bref is Table)
//                                    {
//                                        Table t = bref as Table;
//                                        nodeP.tableSET.Add(t);
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            if (!isActiveDoc)
//            {
//                nodeDoc.CloseAndDiscard();
//            }
//            
//            return nodeP;
//        }
//    }
//}
