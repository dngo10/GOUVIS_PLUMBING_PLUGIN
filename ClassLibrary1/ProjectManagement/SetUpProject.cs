using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1.PNOTE;
using GouvisPlumbingNew.DATABASE.Controllers;
using System.Data.SQLite;
using GouvisPlumbingNew.PNOTE;
using System.Windows.Forms;
using ProjectManager;

namespace ClassLibrary1.ProjectManagement
{
    class SetUpProject
    {
        public static NODEDWG SetUp(string currentDWGPath)
        {
            string databasePath = GoodiesPath.GetDatabasePathFromDwgPath(currentDWGPath);
            NODEDWG node;

            if (string.IsNullOrEmpty(databasePath))
            {
                SQLiteConnection connection = PlumbingDatabaseManager.OpenSqliteConnection(databasePath);
                node = ReadPNote.ReadDataPNode(connection);
            }
            else
            {
                while (string.IsNullOrEmpty(databasePath))
                {
                    databasePath = GoodiesPath.GetDatabasePathFromDwgPath(currentDWGPath);
                    MessageBox.Show("This file is not path of any project.", "Project Not Found", MessageBoxButtons.OK);

                    ProgramManagerForm form = new ProgramManagerForm("");
                    form.Show();
                }

                SQLiteConnection connection = PlumbingDatabaseManager.OpenSqliteConnection(databasePath);
                node = ReadPNote.ReadDataPNode(connection);
            }
            return node;
        }
    }
}
