using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using ClassLibrary1.HELPERS;
using GouvisPlumbingNew.DATABASE.Controllers;
using GouvisPlumbingNew.DATABASE.DBModels;
using GouvisPlumbingNew.HELPERS;
using GouvisPlumbingNew.PNOTE;
using System.Data.SQLite;
using System.Linq;
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

            FileStatus fileStatus =  Goodies.CanOpenToWrite(notePathFull);

            //Document doc = Goodies.CanOpenToWrite1(notePathFull);
            if (fileStatus == null || fileStatus.db == null) return;
            //if (doc == null) return;

            DwgFileModel fileModel = PlumbingDatabaseManager.GetNotePath(connection);
            if (notePath == fileModel.relativePath)
            {
                if (GoodiesPath.IsDateTheSame(notePathFull, connection))
                {

                    NODEDWG note = ReadPNote.ReadDataPNode(connection);


                    InsertPoint ip = note.InsertPointSet.Where(insertpoint => insertpoint.model.alias == "FS").FirstOrDefault();
                    Table table = TableSchedule.CreateTable(note.FixtureDetailSet, ip);
                    Document doc = Application.DocumentManager.GetDocument(fileStatus.db);
                    using (doc.LockDocument())
                    {
                        using (fileStatus.db)
                        {
                            TableSchedule.DeleteTableSchedule(fileStatus.db, XDataHelperName.tableSchedule);

                            using (Transaction tr = fileStatus.db.TransactionManager.StartTransaction())
                            {
                                BlockTable bt = (BlockTable)tr.GetObject(fileStatus.db.BlockTableId, OpenMode.ForRead);
                                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                                Goodies.InsertTable(table, tr, btr);

                                table.Layer = ConstantName.TABLE;
                                XDataHelper.AddTableXData(ref table, tr, fileStatus.db);
                                tr.Commit();
                                tr.Dispose();
                                TableData tableData = new TableData(table, tr, fileStatus.db);
                                tableData.model.file = fileModel;
                                tableData.model.WriteToDatabase(connection);
                            }
                            TableSchedule.AddBlockToTable(table, fileStatus.db, note.FixtureDetailSet);
                            fileStatus.Save();
                        }
                    }
                    fileStatus.ReturnPreviousDocument();
                }
            }
        }
    }
}

//DART everywhere.
