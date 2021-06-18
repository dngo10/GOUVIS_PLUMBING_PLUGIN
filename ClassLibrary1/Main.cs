using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GouvisPlumbingNew.HELPERS;
using GouvisPlumbingNew.P_NODE_EDIT;
using Autodesk.AutoCAD.ApplicationServices;
using GouvisPlumbingNew.DATABASE;
using System.Data.SQLite;
using System.IO;
using GouvisPlumbingNew.DATABASE.Controllers;
using Autodesk.AutoCAD.DatabaseServices;
using ClassLibrary1.PNOTE;
using System.Windows.Forms;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using ClassLibrary1.FIXTUREUNIT;

namespace GouvisPlumbingNew
{
    public class Main
    {
        //TESTING
        [CommandMethod("ReadNoteData")]
        public void ReadData()
        {
            string dwgPath = Application.DocumentManager.MdiActiveDocument.Name;
            string dbPath = GoodiesPath.GetDatabasePathFromDwgPath(dwgPath);

            if (string.IsNullOrEmpty(dbPath))
            {
                MessageBox.Show("Couldn't find the database!");
                return;
            }

            SQLiteConnection connection = PlumbingDatabaseManager.OpenSqliteConnection(dbPath);
            connection.Open();
            using (SQLiteTransaction sqlTr = connection.BeginTransaction())
            {
                //P_NODE_EDIT.TestingFunction.testing1(ConstantName.TEMPPATH);
                ReadPNote.ReadDataPNode(connection);
                sqlTr.Commit();
                connection.Close();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        [CommandMethod("ADDFIXTUREUNIT")]
        public void RunTest4()
        {
            FixtureUnitInsert.InsertFixtureUnit();
        }

        [CommandMethod("ADDTABLE")]
        public void RunTest3()
        {
            string dwgPath = Application.DocumentManager.MdiActiveDocument.Name;
            string dbPath = GoodiesPath.GetDatabasePathFromDwgPath(dwgPath);

            if (string.IsNullOrEmpty(dbPath))
            {
                MessageBox.Show("Couldn't find the database!");
                return;
            }

            SQLiteConnection connection = PlumbingDatabaseManager.OpenSqliteConnection(dbPath);
            connection.Open();
            using (SQLiteTransaction sqlTr = connection.BeginTransaction())
            {
                WritePNote.WriteScheduleTableToNote(connection);
                sqlTr.Commit();
                connection.Close();
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        [CommandMethod("ADDFIXTURE")]
        public void AddFixture()
        {
            
        }

        
    }
}
