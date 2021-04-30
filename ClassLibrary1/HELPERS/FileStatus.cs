using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
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

        public FileStatus(int status, string dwgPath)
        {
            if(status == 0)
            {
                db = Application.DocumentManager.MdiActiveDocument.Database;
            }else if(status == 1 || status == 2)
            {
                //Application.DocumentManager.MdiActiveDocument = Application.DocumentManager.Open(dwgPath, false);
                db.ReadDwgFile(dwgPath, FileOpenMode.OpenForReadAndAllShare, false, "");
            }else if(status == 3)
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
