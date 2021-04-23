using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GouvisPluminbNew.P_NODE_EDIT;
using ClassLibrary1.HELPERS;
using ClassLibrary1.P_NODE_EDIT;
using Autodesk.AutoCAD.ApplicationServices;
using ClassLibrary1.DATABASE;
using System.Data.SQLite;

namespace ClassLibrary1
{
    public class Main
    {
        //TESTING

        [CommandMethod("abc")]
        public void RunTest()
        {

            //P_NODE_EDIT.TestingFunction.testing1(pNode_directory);


            //var temp = ReadInformationInNodeDrawing.FindAllFixturesBeingUsed(ConstantName.TEMPPATH);
            //UpdateDataInNodeDrawing.UpdateNodeDrawing(Application.DocumentManager.MdiActiveDocument.Name);

            string tempFile = "C:\\Users\\dngo\\Documents\\TESTING111\\temp1.db";
            SQLiteConnection.CreateFile(tempFile);

            
            string connectionStr = DBCommand.GetConnectionString(tempFile);
            using (SQLiteConnection sqliteConn = new SQLiteConnection(connectionStr))
            {
                sqliteConn.Open();
                using (SQLiteTransaction tr = sqliteConn.BeginTransaction())
                {
                    DBPoint3D.CreateTable(sqliteConn);
                    tr.Commit();
                }
                sqliteConn.Close();
            }


            //sqliteTrans.Commit();
            //command.Dispose();
            //sqliteTrans.Dispose();
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
