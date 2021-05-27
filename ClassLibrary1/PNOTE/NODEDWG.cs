using Autodesk.AutoCAD.DatabaseServices;
using ClassLibrary1.DATABASE.Controllers;
using ClassLibrary1.DATABASE.DBModels;
using ClassLibrary1.HELPERS.BLOCKS;
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
        public SortedSet<FixtureUnit> FixtureUnitsSet = new SortedSet<FixtureUnit>(Comparer<FixtureUnit>.Create((a, b) => a.model.ID.CompareTo(b.model.ID)));
        public SortedSet<AreaBorder> AreaModelSet = new SortedSet<AreaBorder>(Comparer<AreaBorder>.Create((a, b) => a.model.ID.CompareTo(b.model.ID)));

        public NODEDWG()
        {
            file = new DwgFileModel();
        }


        /// <summary>
        /// Update Database, FILE MUST BE INITIATED.
        /// </summary>
        /// <param name="connection"></param>
        public void UpdateDatabase(SQLiteConnection connection)
        {
            List<FixtureBeingUsedAreaModel> handle1 = DBFixtureBeingUsedArea.SelectRows(connection, file.ID);
            List<FixtureDetailsModel> handle2 = DBFixtureDetails.SelectRows(connection, file.ID);
            List<InsertPointModel> handle3 = DBInsertPoint.SelectRows(connection, file.ID);
            List<FixtureUnitModel> handle4 = DBFixture_Unit.SelectRows(connection, file.ID);
            List<AreaBorderModel> handle5 =  DBAreaBorder.SelectRows(connection, file.ID);

            List<string> toDelete1 = HandleToDelete(handle1, FixtureBoxSet).ToList();
            List<string> toDelete2 = HandleToDelete(handle2, FixtureDetailSet).ToList();
            List<string> toDelete3 = HandleToDelete(handle3, InsertPointSet).ToList();
            List<string> toDelete4 = HandleToDelete(handle4, FixtureUnitsSet).ToList();
            List<string> toDelete5 = HandleToDelete(handle5, AreaModelSet).ToList();

            foreach(string handle in toDelete1) { DBFixtureBeingUsedArea.DeleteRow(connection, handle, file.ID);}
            foreach(string handle in toDelete2) { DBFixtureDetails.DeleteRow(connection, handle, file.ID); }
            foreach(string handle in toDelete3) { DBInsertPoint.DeleteRow(connection, handle, file.ID); }
            foreach(string handle in toDelete4) { DBFixture_Unit.DeleteRow(connection, handle, file.ID); }
            foreach(string handle in toDelete5) { DBAreaBorder.DeleteRow(connection, handle, file.ID); }

            WriteToDataBase(connection);

        }




        /// <summary>
        /// Get a list of Handles to delete in Database.
        /// </summary>
        /// <param name="set1">Set of Handles in Database </param>
        /// <param name="set2">Set of handles after Read DWG file</param>
        /// <returns></returns>
        public IEnumerable<string> HandleToDelete(IEnumerable<dynamic> set1, IEnumerable<dynamic> set2)
        {
            HashSet<string> h1 = GetHandles(set1);
            HashSet<string> h2 = GetHandles(set2);

            return h1.Except(h2);
        }

        public HashSet<string> GetHandles(IEnumerable<dynamic> set){
            HashSet<string> result = new HashSet<string>();
            foreach(dynamic type in set)
            {
                result.Add(type.handle);
            }
            return result;
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
            foreach(TableData table in TableDataSet)
            {
                table.model.WriteToDatabase(connection);
            }

            foreach(AreaBorder am in AreaModelSet)
            {
                am.model.WriteToDatabase
            }

            foreach(FixtureUnit fixture in FixtureUnitsSet)
            {
                fixture.model.
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
            TableDataSet.Clear();

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

            foreach (TableModel model in DBTable.SelectRows(connection, file.ID))
            {
                TableData table = new TableData(model);
                TableDataSet.Add(table);
            }
        }
    }
}
