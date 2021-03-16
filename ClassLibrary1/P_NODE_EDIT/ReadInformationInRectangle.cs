using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1.HELPERS;

namespace GouvisPluminbNew.P_NODE_EDIT
{
    class ReadInformationInRectangle
    {
        /// <summary>
        /// Run node file, select all fixtures in there to make Fixture Schedule Table.
        /// There should be only one boxName block in P_NODE, the function will only select one.
        /// </summary>
        /// <param name="boxName">dynamic block. It's a box that all FixtureDetails are inside</param>
        /// <param name="blockNameToBeSelected">In this case FixtureDetails block</param>
        /// <param name="P_NodeFileDirectory">Location of P_NODE file</param>
        public static void FindAllFixturesBeingUsed(string boxName, string blockNameToBeSelected, string P_NodeFileDirectory)
        {
            Document nodeDoc = Application.DocumentManager.Open(P_NodeFileDirectory, true);
            FixtureBeingUsedArea fixtureBeingUsedArea = null;

            List<FixtureDetails> FDList = new List<FixtureDetails>();

            using (nodeDoc.LockDocument())
            {
                Database nodeDb = nodeDoc.Database;
                using (Transaction tr = nodeDb.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)tr.GetObject(nodeDb.BlockTableId, OpenMode.ForRead);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);
                    Matrix3d ucsMatrix = nodeDoc.Editor.CurrentUserCoordinateSystem;

                    foreach(ObjectId id in btr)
                    {
                        DBObject obj = tr.GetObject(id, OpenMode.ForRead);
                        if(obj is BlockReference)
                        {
                            BlockReference bref = (BlockReference)obj;
                            if (Goodies.IsBlockInTheDrawing(bref))
                            {
                                if (bref.IsDynamicBlock)
                                {

                                    string brefName = Goodies.GetDynamicName(bref, nodeDoc.Editor);
                                    if (brefName.Equals(ConstantName.FixtureInformationArea))
                                    {
                                        fixtureBeingUsedArea = new FixtureBeingUsedArea(bref, tr, nodeDoc.Editor);
                                    }
                                }
                                else if (bref.Name == ConstantName.FixtureDetailsBox)
                                {
                                    FixtureDetails FD = new FixtureDetails(bref, tr);
                                    FDList.Add(FD);
                                }else if(bref.Name == ConstantName.InsertPoint)
                                {
                                    InsertPoint IP = new InsertPoint(bref, tr);
                                }
                            }
                        }else if(obj is Table)
                        {
                            Table table = (Table)obj;
                            
                        }
                    }

                    if(fixtureBeingUsedArea != null)
                    {
                        foreach (FixtureDetails FD in FDList)
                        {
                            if (fixtureBeingUsedArea.IsInsideTheBox(FD.position))
                            {
                                fixtureBeingUsedArea.FDList.Add(FD);
                            }
                        }
                    }
                }
            }
            nodeDoc.CloseAndDiscard();
        }
    }
}
