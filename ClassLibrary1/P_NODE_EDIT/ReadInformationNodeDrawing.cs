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
using ClassLibrary1.P_NODE_EDIT;

namespace GouvisPluminbNew.P_NODE_EDIT
{
    class ReadInformationInNodeDrawing
    {
        /// <summary>
        /// Run node file, select all fixtures in there to make Fixture Schedule Table.
        /// There should be only one boxName block in P_NODE, the function will only select one.
        /// </summary>
        /// <param name="boxName">dynamic block. It's a box that all FixtureDetails are inside</param>
        /// <param name="blockNameToBeSelected">In this case FixtureDetails block</param>
        /// <param name="P_NodeFileDirectory">Location of P_NODE file</param>
        public static NODEDWG FindAllFixturesBeingUsed(string P_NodeFileDirectory)
        {
            NODEDWG nodeP = new NODEDWG();

            Document nodeDoc = Application.DocumentManager.Open(P_NodeFileDirectory, true);

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
                                        nodeP.fixtureAreaSET.Add(bref);
                                    }
                                }
                                else if (bref.Name == ConstantName.FixtureDetailsBox)
                                {
                                    nodeP.fixtureDetailSET.Add(bref);
                                }else if(bref.Name == ConstantName.InsertPoint)
                                {
                                    nodeP.insertionPointSET.Add(bref);
                                }else if(bref is Table)
                                {
                                    Table t = bref as Table;
                                    nodeP.tableSET.Add(t);
                                }
                            }
                        }
                    }
                }
            }
            nodeDoc.CloseAndDiscard();
            return nodeP;
        }
    }
}
