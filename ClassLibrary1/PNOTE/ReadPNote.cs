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
        public static NODEDWG ReadDwgPNoteFile(SQLiteConnection connection)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;

            NODEDWG note = null;

            string notePath = GoodiesPath.GetNotePathFromADwgPath(doc.Name, connection);
            if (!string.IsNullOrEmpty(notePath))
            {
                if(doc.Name == notePath)
                {
                    //Read this
                    using(Database db = doc.Database)
                    {
                        note = GetData(db, connection);
                    }
                }
                else
                {
                    Database db = new Database();
                    try
                    {
                        db.ReadDwgFile(notePath, FileOpenMode.OpenForReadAndAllShare, false, "");
                        note = GetData(db, connection);
                    }catch(Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }

            return note;
        }

        private static NODEDWG GetData(Database db, SQLiteConnection connection)
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

            //Write to Database

            string currentdwgPath = db.Filename;
            string dbPath = GoodiesPath.GetDatabasePathFromDwgPath(currentdwgPath);
            if(string.IsNullOrEmpty(dbPath))
            {
                MessageBox.Show("Could Not Find Database.");
            } 

            note.file = DBDwgFile.GetPNote(connection);

            if(note.file == null)
            {
                MessageBox.Show($"Can't find this {currentdwgPath} in databse.");
                return null;
            }
            foreach(FixtureBeingUsedArea fixtureBox in note.FixtureBoxSet)
            {
                fixtureBox.model.file = note.file;
            }

            foreach(FixtureDetails fd in note.FixtureDetailSet)
            {
                fd.model.file = note.file;
            }
            foreach(InsertPoint ip in note.InsertPointSet)
            {
                ip.model.file = note.file;
            }

            note.WriteToDataBase(connection);

            return note;
        }

        public static NODEDWG ReadFromDatabase(SQLiteConnection connection)
        {
            NODEDWG note = new NODEDWG();

            string path = Application.DocumentManager.MdiActiveDocument.Name;
            string notePath = GoodiesPath.GetNotePathFromADwgPath(path, connection);
            note.ReadFromDatabase(connection, notePath);
            return note;
        }
    }
}
