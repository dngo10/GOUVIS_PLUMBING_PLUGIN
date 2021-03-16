using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.HELPERS
{
    class TableSchedule
    {
        public Table table = new Table();
        public FixtureBeingUsedArea FBUA;
        public TableSchedule(FixtureBeingUsedArea FBUA)
        {
            this.FBUA = FBUA;
        }

        private Table CreateTable(List<FixtureDetails> FixtureDetails, Transaction tr, InsertPoint insertPoint)
        {
            Table t = new Table();
            t.Layer = TableScheduleName.TableLayer;
            t.Width = TableScheduleName.Width;

            int rowsNum = 2 + FixtureDetails.Count;
            int columnsNum = 8;

            t.InsertRows(0, 1.5, rowsNum);
            t.InsertColumns(0, 1, columnsNum);

            //Merge the Title ROWS
            CellRange crTitle = CellRange.Create(t, 0, 0, 0, columnsNum - 1);
            t.MergeCells(crTitle);

            //GENERAL SETTINGS
            t.Cells.TextHeight = TableScheduleName.GeneralTextHeight;
            t.Cells.ContentColor = Color.FromColorIndex(ColorMethod.ByAci, 2);
            t.Cells.Alignment = CellAlignment.MiddleCenter;


            //Column Description -- Align Left
            t.Columns[columnsNum - 1].Alignment = CellAlignment.MiddleLeft;

            //Set WIDTH FOR EARCH COLUMN:
            t.Columns[0].Width = 0.67;
            t.Columns[1].Width = 2;
            t.Columns[2].Width = 0.5;
            t.Columns[3].Width = 0.5;
            t.Columns[4].Width = 0.5;
            t.Columns[5].Width = 0.5;
            t.Columns[6].Width = 0.5;

            //Set General ROWS HEIGHT;
            for (int i = 0; i < rowsNum; i++) { t.Rows[i].Height = TableScheduleName.GeneralRowHeight;}

            t.Rows[0].Height = TableScheduleName.TitleRowHeight;
            t.Cells[0, 0].TextString = insertPoint.NAME;
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
                t.Cells[i, 1].TextString = FD.FixtureName;
                t.Cells[i, 2].TextString = FD.CW_DIA;
            }

            return null;
        }
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
