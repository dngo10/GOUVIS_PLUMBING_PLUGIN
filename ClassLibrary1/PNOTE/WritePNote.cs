using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using GouvisPlumbingNew.DATABASE.Controllers;
using GouvisPlumbingNew.DATABASE.DBModels;
using GouvisPlumbingNew.HELPERS;
using GouvisPlumbingNew.PNOTE;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace ClassLibrary1.PNOTE
{
    //This is used to write table
    class WritePNote
    {
        public static void WriteScheduleTableToNote(SQLiteConnection connection) {
            string dwgPath = Application.DocumentManager.MdiActiveDocument.Name;
            string notePathFull = GoodiesPath.GetNotePathFromADwgPath(dwgPath, connection);
            string notePath = GoodiesPath.MakeRelativePath(notePathFull);

            if (string.IsNullOrEmpty(notePath))
            {
                MessageBox.Show("WriteScheduleTableToNote -> Can't find P_Note file.");
                return;
            }

            Document doc =  Goodies.CanOpenToWrite(notePathFull);
            if (doc == null) return;

            DwgFileModel model = PlumbingDatabaseManager.GetNotePath(connection);
            if (notePath == model.relativePath)
            {
                if (PlumbingDatabaseManager.CheckModified(notePathFull, connection))
                {
                    NODEDWG note = ReadPNote.ReadFromDatabase(connection);
                    InsertPoint ip = note.InsertPointSet.Where(insertpoint => insertpoint.model.alias == "FS").FirstOrDefault();
                    Table table = TableSchedule.CreateTable(note.FixtureDetailSet, ip);
                    using (doc.LockDocument())
                    {
                        using(Database db = doc.Database)
                        {
                            using(Transaction tr = db.TransactionManager.StartTransaction())
                            {
                                table.Layer = "0";
                                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                                Goodies.InsertTable(table, tr, btr);
                                tr.Commit();
                            }
                            //db.Save();
                        }
                    }
                }
            }
        }
    }
}
