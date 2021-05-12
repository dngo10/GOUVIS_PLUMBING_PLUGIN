using Autodesk.AutoCAD.DatabaseServices;
using ClassLibrary1.HELPERS;
using GouvisPlumbingNew.DATABASE.Controllers;
using GouvisPlumbingNew.DATABASE.DBModels;
using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GouvisPlumbingNew.PNOTE
{
    class NODEDWG
    {
        public DwgFileModel file = null;
        public SortedSet<FixtureBeingUsedArea> FixtureBoxSet = new SortedSet<FixtureBeingUsedArea>(Comparer<FixtureBeingUsedArea>.Create((a, b) => a.model.ID.CompareTo(b.model.ID)));
        public SortedSet<FixtureDetails> FixtureDetailSet = new SortedSet<FixtureDetails>(Comparer<FixtureDetails>.Create((a, b) => a.model.INDEX.CompareTo(b.model.INDEX)));
        public SortedSet<InsertPoint> InsertPointSet = new SortedSet<InsertPoint>(Comparer<InsertPoint>.Create((a, b) => a.model.ID.CompareTo(b.model.ID)));
        public SortedSet<TableData> TableDataSet = new SortedSet<TableData>(Comparer<TableData>.Create((a, b) => a.model.ID.CompareTo(b.model.ID)));

        public NODEDWG()
        {
            file = new DwgFileModel();
        }

        /// <summary>
        /// Write to sqlite Database.
        /// This only write File Model into database, other things must be written manually.
        /// BecareFull, this should only be written once (at best, once).
        /// </summary>
        /// <param name="connection">sqlite connection</param>
        /// <returns></returns>
        public void WriteToDataBase(SQLiteConnection connection)
        {
            file.WriteToDatabase(connection);
            foreach(FixtureBeingUsedArea fixture in FixtureBoxSet)
            {
                fixture.model.WriteToDataBase(connection);
            }

            foreach(FixtureDetails df in FixtureDetailSet)
            {
                df.model.WriteToDatabase(connection);
            }

            foreach(HELPERS.InsertPoint ip in InsertPointSet)
            {
                ip.model.WriteToDataBase(connection);
            }
        }

        /// <summary>
        /// Read Node File From Database, Fill Out The NODEDWG
        /// Path must be FULL PATH of a DWG from the database
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="path">Note PATH</param>
        public void ReadFromDatabase(SQLiteConnection connection, string pathDwg)
        {
            FixtureBoxSet.Clear();
            InsertPointSet.Clear();
            FixtureDetailSet.Clear();

            string relPath = GoodiesPath.MakeRelativePath(pathDwg);
            file = DBDwgFile.SelectRow(connection, relPath);
            if(file == null || file.ID == ConstantName.invalidNum)
            {
                MessageBox.Show("NODEDWG -> ReadFromDatabase -> File Not Found");
                return;
            }
            foreach(FixtureBeingUsedAreaModel fixtureBox in DBFixtureBeingUsedArea.SelectRows(connection, file.ID))
            {
                FixtureBeingUsedArea fixtureArea = new FixtureBeingUsedArea(fixtureBox);
                //this line is very important. You have to make sure they have file model in each fixtureArea;
                //this won't complicate the program as we pass the pointer.
                FixtureBoxSet.Add(fixtureArea);
            }
            
            foreach(FixtureDetailsModel detailModel in DBFixtureDetails.SelectRows(connection, file.ID))
            {
                FixtureDetails fd = new FixtureDetails(detailModel);
                FixtureDetailSet.Add(fd);
            }

            foreach(InsertPointModel insertPointModel in DBInsertPoint.SelectRows(connection, file.ID))
            {
                InsertPoint ip = new InsertPoint(insertPointModel);
                InsertPointSet.Add(ip);
            }
        }
    }
}
