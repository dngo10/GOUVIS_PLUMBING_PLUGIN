using Autodesk.AutoCAD.DatabaseServices;
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

namespace GouvisPlumbingNew.PNOTE
{
    class NODEDWG
    {
        public DwgFileModel model;
        public SortedSet<FixtureBeingUsedArea> FixtureBoxSet = new SortedSet<FixtureBeingUsedArea>();
        public SortedSet<FixtureDetails> FixtureDetailSet = new SortedSet<FixtureDetails>(Comparer<FixtureDetails>.Create((a, b) => a.model.INDEX.CompareTo(b.model.INDEX)));
        public SortedSet<InsertPoint> InsertPointSet = new SortedSet<InsertPoint>();

        public NODEDWG()
        {
            model = new DwgFileModel();
            FixtureBoxSet = new SortedSet<FixtureBeingUsedArea>();
            FixtureDetailSet = new SortedSet<FixtureDetails>();
            InsertPointSet = new SortedSet<InsertPoint>();
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
            model.WriteToDatabase(connection);
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
                ip.m
            }
        }
    }
}
