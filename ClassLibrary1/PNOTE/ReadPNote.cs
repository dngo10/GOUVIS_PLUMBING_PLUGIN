using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Interop;
using ClassLibrary1.HELPERS;
using GouvisPlumbingNew.DATABASE.Controllers;
using GouvisPlumbingNew.DATABASE.DBModels;
using GouvisPlumbingNew.HELPERS;
using GouvisPlumbingNew.PNOTE;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace ClassLibrary1.PNOTE
{
    class ReadPNote
    {
        /// <summary>
        /// ReadPNote is read Note in general, can be from database, can be from dwg depends on current situation.
        /// If dwgPNote is opened, is active or not, is modified or not (can't check whether or not it is modied), refer to read from DWG and updateDatabase.
        /// If dwgPNote is Not open check date, if if equals, read from database.
        /// Else: read from file + udpate todatabase.
        /// FK IT, JUST FOUND DOWN THE INTEROPT SHIT
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static NODEDWG ReadDataPNode(SQLiteConnection connection)
        {
            NODEDWG note = null;

            string notePath = GoodiesPath.GetNotePathFromADwgPath(Application.DocumentManager.MdiActiveDocument.Name, connection);
            if (!string.IsNullOrEmpty(notePath))
            { 
                if (Goodies.GetListOfDocumentOpening().Contains(notePath))
                {
                    Document doc = Goodies.GetDocumentFromDwgpath(notePath);
                    
                    if (doc == null)
                    {
                        Application.ShowAlertDialog("ReadDwgNoteFile -> There is no Document, weird!");
                        return null;
                    }

                    AcadDocument acadDoct = (AcadDocument)doc.GetAcadDocument();
                    if (acadDoct.Saved && GoodiesPath.IsDateTheSame(notePath, connection))
                    {
                        note = ReadFromDatabase(connection);
                    }
                    else
                    {
                        using (Database db = doc.Database)
                        {
                            note = GetData(db, connection);
                        }
                    }
                }
                else
                {
                    Database db = new Database();
                    try
                    {
                        if(GoodiesPath.IsDateTheSame(notePath, connection))
                        {
                            note = ReadFromDatabase(connection);
                        }
                        else
                        {
                            db.ReadDwgFile(notePath, FileOpenMode.OpenForReadAndAllShare, false, "");
                            note = GetData(db, connection);
                        }

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
                                    if(dbA.model != null)
                                    {
                                        note.FixtureBoxSet.Add(dbA);
                                    }
                                    
                                }
                            }
                            else if (bref.Name == ConstantName.FixtureDetailsBox)
                            {
                                FixtureDetails FD = new FixtureDetails(bref, tr);
                                if(FD.model != null)
                                {
                                    note.FixtureDetailSet.Add(FD);
                                }
                                
                            }
                            else if (bref.Name == ConstantName.InsertPoint)
                            {
                                InsertPoint IP = new InsertPoint(bref, tr);
                                if(IP.model != null)
                                {
                                    note.InsertPointSet.Add(IP);
                                }
                                
                            }
                            else if (bref is Table)
                            {
                                TableData tb = new TableData(bref, tr, db);
                                if(tb.model != null)
                                {
                                    note.TableDataSet.Add(tb);
                                }
                                
                            }
                        }
                    }
                }

                List<FixtureDetails> newFixs = new List<FixtureDetails>();

                foreach(FixtureBeingUsedArea fba in note.FixtureBoxSet)
                {
                    foreach(FixtureDetails fd in note.FixtureDetailSet)
                    {
                        if (fba.IsInsideTheBox(fd))
                        {
                            newFixs.Add(fd);
                        }
                    }
                }

                note.FixtureDetailSet.Clear();
                foreach (FixtureDetails fd in newFixs) note.FixtureDetailSet.Add(fd);
            }

            //Write to Database

            string currentdwgPath = db.Filename;
            string dbPath = GoodiesPath.GetDatabasePathFromDwgPath(currentdwgPath);
            if(string.IsNullOrEmpty(dbPath))
            {
                MessageBox.Show("Could Not Find Database.");
            } 

            note.file = DBDwgFile.GetPNote(connection);
            note.file.modifieddate = GoodiesPath.GetModifiedOfFile(GoodiesPath.GetFullPathFromRelativePath(note.file.relativePath, connection)).Ticks;

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
            foreach(TableData table in note.TableDataSet)
            {
                table.model.file = note.file;
            }

            DBDwgFile.DeleteRow(connection, note.file.relativePath);

            note.WriteToDataBase(connection);

            return note;
        }

        private static NODEDWG ReadFromDatabase(SQLiteConnection connection)
        {
            NODEDWG note = new NODEDWG();

            string path = Application.DocumentManager.MdiActiveDocument.Name;
            string notePath = GoodiesPath.GetNotePathFromADwgPath(path, connection);
            note.ReadFromDatabase(connection, notePath);
            return note;
        }
    }
}
