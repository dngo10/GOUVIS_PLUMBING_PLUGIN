using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Windows;
using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.HELPERS
{
    //0 - Active
    //1 - Opening, but not active.
    //2 - Not Opening
    //3 - Can't open it, not writable.
    class FileStatus
    {
        public Database db = new Database(true, false);
        public int status;
        public string dwgPath;
        private string previousDwgPath;

        public void Save()
        {
            if(status == 2)
            {
                db.SaveAs(dwgPath, DwgVersion.Current);
            }
        }

        public void ReturnPreviousDocument()
        {
            if(status == 1) {
                Document doc = Goodies.GetDocumentFromDwgpath(previousDwgPath);
                using (doc.LockDocument())
                {
                    Application.DocumentManager.MdiActiveDocument = doc;
                    Application.DocumentManager.CurrentDocument = doc;
                }

            }
        }

        public FileStatus(int status, string dwgPath)
        {
            this.status = status;
            this.dwgPath = dwgPath;
            if (status == 0)
            {
                db = Application.DocumentManager.MdiActiveDocument.Database;
                
            }else if(status == 1)
            {
                previousDwgPath = Application.DocumentManager.MdiActiveDocument.Name;

                Document doc = Goodies.GetDocumentFromDwgpath(dwgPath);
                db = doc.Database;
                Application.DocumentManager.MdiActiveDocument = doc;
                Application.DocumentManager.CurrentDocument = doc;
                //Application.DocumentManager.MdiActiveDocument = Application.DocumentManager.Open(dwgPath, false);
                //db = null;
            }else if(status == 2)
            {
                db.ReadDwgFile(dwgPath, FileOpenMode.OpenForReadAndAllShare, false, "");
            }
            else if(status == 3)
            {
                db = null;
            }
            else
            {
                db = null;
            }
        }
    }
}
