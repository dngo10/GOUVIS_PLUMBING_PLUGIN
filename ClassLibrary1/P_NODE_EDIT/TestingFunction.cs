using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using ClassLibrary1.DATABASE.Controllers;
using ClassLibrary1.HELPERS;
using GouvisPluminbNew.P_NODE_EDIT;

namespace ClassLibrary1.P_NODE_EDIT
{
    class TestingFunction
    {
        public static void testing1(string FileDWGPath)
        {
            var temp = ReadInformationInNodeDrawing.FindAllFixturesBeingUsed(ConstantName.TEMPPATH);
            //UpdateDataInNodeDrawing.UpdateNodeDrawing(Application.DocumentManager.MdiActiveDocument.Name);

            string tempFile = "C:\\Users\\dngo\\Documents\\TESTING111\\temp1.db";
            if (File.Exists(tempFile)) File.Delete(tempFile);
            SQLiteConnection.CreateFile(tempFile);


            string connectionStr = DBCommand.GetConnectionString(tempFile);
            using (SQLiteConnection sqliteConn = new SQLiteConnection(connectionStr))
            {
                sqliteConn.Open();
                using (SQLiteTransaction tr = sqliteConn.BeginTransaction())
                {
                    DBPoint3D.CreateTable(sqliteConn);
                    DBMatrix3d.CreateTable(sqliteConn);
                    DBFixtureDetails.CreateTable(sqliteConn);
                    DBFixtureBeingUsedArea.CreateTable(sqliteConn);
                    DBDwgFile.CreateTable(sqliteConn);
                    foreach (FixtureBeingUsedArea blockRef in temp.fixtureAreaSET)
                    {
                        long id = DBFixtureBeingUsedArea.InsertRow(sqliteConn, blockRef.fixtureBeingUsedAreaModel);
                    }
                    tr.Commit();
                }
                sqliteConn.Close();
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }
        
    }
}
