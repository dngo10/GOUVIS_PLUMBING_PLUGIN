using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using GouvisPlumbingNew.DATABASE.Controllers;
using GouvisPlumbingNew.DATABASE.DBModels;
using GouvisPlumbingNew.HELPERS;
using GouvisPlumbingNew.PNOTE;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace ClassLibrary1.PNOTE
{
    class ReadPNote
    {
        public static void ReadDwgPNote()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;

            string notePath = GoodiesPath.GetNotePathFromADwgPath(doc.Name);
            if (!string.IsNullOrEmpty(notePath))
            {
                if(doc.Name == notePath)
                {
                    //Read this
                }
                else
                {
                    Database db = new Database();
                    try
                    {
                        db.ReadDwgFile(notePath, FileOpenMode.OpenForReadAndAllShare, false, "");
                    }catch(Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
        }

        public static void GetData(Database db, Document doc)
        {
            NODEDWG note = new NODEDWG();

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);
                foreach(ObjectId id in btr)
                {
                    DBObject obj = tr.GetObject(id, OpenMode.ForRead);
                    if (obj is BlockReference)
                    {
                        BlockReference bref = (BlockReference)obj;
                        if (Goodies.IsBlockInTheDrawing(bref))
                        {
                            if (bref.IsDynamicBlock)
                            {

                                string brefName = Goodies.GetDynamicName(bref);
                                if (brefName.Equals(ConstantName.FixtureInformationArea))
                                {
                                    FixtureBeingUsedArea dbA = new FixtureBeingUsedArea(bref);
                                    note.FixtureBoxSet.Add(dbA);
                                }
                            }
                            else if (bref.Name == ConstantName.FixtureDetailsBox)
                            {
                                FixtureDetails FD = new FixtureDetails(bref, tr);
                                note.FixtureDetailSet.Add(FD);
                            }
                            else if (bref.Name == ConstantName.InsertPoint)
                            {
                                InsertPoint IP = new InsertPoint(bref, tr);
                                note.InsertPointSet.Add(IP);
                            }
                        }
                    }
                }
            }
        }
    }
}
