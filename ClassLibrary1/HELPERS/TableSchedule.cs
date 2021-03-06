using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using ClassLibrary1.HELPERS;
using GouvisPlumbingNew.DATABASE.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GouvisPlumbingNew.HELPERS
{
    //This is used to create SCHEDULE TABLE table.
    //It is not tasked to delete one.
    class TableSchedule
    {
        public static Table CreateTable(ICollection<FixtureDetails> FixtureDetails, InsertPoint insertPoint)
        {
            Table t = new Table();
            //t.Layer = TableScheduleName.TableLayer;
            t.Width = TableScheduleName.Width;

            int rowsNum = 2 + FixtureDetails.Count;
            int columnsNum = 8;

            // table already has 1 row, 1 column, that's why we insert - 1;
            t.InsertRows(0, 1.5, rowsNum-1);
            t.InsertColumns(0, 1, columnsNum-1);

            //Merge the Title ROWS
            CellRange crTitle = CellRange.Create(t, 0, 0, 0, columnsNum - 1);
            t.MergeCells(crTitle);

            //GENERAL SETTINGS
            t.Cells.TextHeight = TableScheduleName.GeneralTextHeight;
            t.Cells.ContentColor = Color.FromColorIndex(ColorMethod.ByAci, 2);
            t.Cells.Alignment = CellAlignment.MiddleCenter;


            //Column Description -- Align Left
            t.Columns[columnsNum - 1].Alignment = CellAlignment.MiddleLeft;

            t.Cells[1, 7].Alignment = CellAlignment.MiddleCenter;

            //Set WIDTH FOR EARCH COLUMN:
            t.Columns[0].Width = 0.67;
            t.Columns[1].Width = 2;
            t.Columns[2].Width = 0.5;
            t.Columns[3].Width = 0.5;
            t.Columns[4].Width = 0.5;
            t.Columns[5].Width = 0.5;
            t.Columns[6].Width = 0.5;
            t.Columns[7].Width = 4.25;

            //Set General ROWS HEIGHT;
            for (int index = 0; index < rowsNum; index++) { t.Rows[index].Height = TableScheduleName.GeneralRowHeight;}

            t.Rows[0].Height = TableScheduleName.TitleRowHeight;
            t.Cells[0, 0].TextString = insertPoint.model.name;
            t.Cells[0, 0].TextHeight = TableScheduleName.TitleTextHeight;

            //SET UP SECOND ROW TITLES
            t.Rows[1].Height = 0.75;
            t.Cells[1, 0].TextString = "ITEM";
            t.Cells[1, 1].TextString = "FIXTURE";
            
            t.Cells[1, 2].TextString = "COLD WATER";
            t.Cells[1, 2].Contents[0].Rotation = Math.PI / 2;

            t.Cells[1, 3].TextString = "HOT WATER";
            t.Cells[1, 3].Contents[0].Rotation = Math.PI / 2;

            t.Cells[1, 4].TextString = "WASTE";
            t.Cells[1, 4].Contents[0].Rotation = Math.PI / 2;

            t.Cells[1, 5].TextString = "VENT";
            t.Cells[1, 5].Contents[0].Rotation = Math.PI / 2;

            t.Cells[1, 6].TextString = "STORM DRAIN";
            t.Cells[1, 6].Contents[0].Rotation = Math.PI / 2;

            t.Cells[1, 7].TextString = "DESCRIPTION";

            //Index starts at third orw;
            int i = 2;

            foreach(FixtureDetails FD in FixtureDetails)
            {
                t.Cells[i, 1].TextString = FD.model.FIXTURENAME;

                t.Cells[i, 2].TextString = returnTextStringFinalValue(NumberConverter.ConvertToFractionalNumber(FD.model.CW_DIA));
                t.Cells[i, 3].TextString = returnTextStringFinalValue(NumberConverter.ConvertToFractionalNumber(FD.model.HW_DIA));
                t.Cells[i, 4].TextString = returnTextStringFinalValue(NumberConverter.ConvertToFractionalNumber(FD.model.WASTE_DIA));
                t.Cells[i, 5].TextString = returnTextStringFinalValue(NumberConverter.ConvertToFractionalNumber(FD.model.VENT_DIA));
                t.Cells[i, 6].TextString = returnTextStringFinalValue(NumberConverter.ConvertToFractionalNumber(FD.model.STORM_DIA));
                t.Cells[i, 7].TextString = FD.model.DESCRIPTION;

                i++;
            }

            t.Position = new Point3d(insertPoint.model.position.X, insertPoint.model.position.Y, insertPoint.model.position.Z);

            return t;
        }

        //Return "-" if failed to get fractional number, else return fractional number + " sign.
        private static string returnTextStringFinalValue(string numStr)
        {
            return numStr != "" ? numStr + "\"" : "-";
        }

        public static void UpdateSingleTable(InsertPoint IP, Database db)
        {

        }

        //TABLE MUST BE ADDED TO DRAWING BEFORE THIS PROCESS CAN HAPPEND
        public static void AddBlockToTable(Table table, Database db, SortedSet<FixtureDetails> fixtureDetails)
        {
            using(Transaction tr = db.TransactionManager.StartTransaction())
            {
                Table t = (Table)tr.GetObject(table.ObjectId, OpenMode.ForWrite);
                for (int i = 2; i < t.Rows.Count; i++)
                {
                    Goodies.InsertDynamicBlockToTableCell(t.Cells[i, 0], db, ConstantName.HexNote, fixtureDetails.ElementAt(i - 2));
                }
                tr.Commit();
            }
        }

        //Delete all Database in
        //Add Alias in, as a general function.
        public static void DeleteTableSchedule(Database db, string alias)
        {
            using(Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);

                foreach(ObjectId id in btr)
                {
                    DBObject dbObj = tr.GetObject(id, OpenMode.ForRead);
                    if(dbObj is Table)
                    {
                        Table tb = (Table)dbObj;
                        if (XDataHelper.GetTableType(tb, tr, db) == alias)
                        {
                            tb.UpgradeOpen();
                            if (!tb.IsErased)
                            {
                                tb.Erase();
                            }
                        }
                    }
                }
                tr.Commit();
            }
        }
    }

    class HexNoteName
    {
        public static string ID = "ID";
        public static string NUM = "ID_NUM";
    }

    class TableScheduleName
    {
        public static string TableLayer = ConstantName.TABLE;
        public static double Width = 9.5;
        public static double TitleRowHeight = 0.5;
        public static double TitleTextHeight = 0.1875;
        public static double GeneralRowHeight = 0.67;
        public static double GeneralTextHeight = 3.0/32.0;
    }
}
