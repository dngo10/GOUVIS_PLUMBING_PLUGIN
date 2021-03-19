using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.HELPERS
{
    class LayoutEdit
    {
        public static void GetLayout(Database db)
        {
            using(Transaction tr = db.TransactionManager.StartTransaction())
            {
                ObjectId layoutDictId = db.LayoutDictionaryId;
                DBDictionary layoutDict = (DBDictionary)tr.GetObject(layoutDictId, OpenMode.ForRead);


            }
        }
    }

    class LayOutInfo
    {
        Handle handle;
    }
}
