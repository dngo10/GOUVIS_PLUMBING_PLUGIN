using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GouvisPlumbingNew.HELPERS
{
    class LayoutEdit
    {
        //Return a List with name and DBObject as String;
        public static Dictionary<string, Layout> GetLayout(Database db)
        {
            Dictionary<string, Layout> ans = new System.Collections.Generic.Dictionary<string, Layout>();
            using(Transaction tr = db.TransactionManager.StartTransaction())
            {
                ObjectId layoutDictId = db.LayoutDictionaryId;
                DBDictionary layoutDict = (DBDictionary)tr.GetObject(layoutDictId, OpenMode.ForRead);
                foreach(DBDictionaryEntry kv in layoutDict)
                {
                    Layout layOut = (Layout)tr.GetObject(kv.Value, OpenMode.ForRead);
                    ans.Add(kv.Key, layOut);
                }
            }
            return ans;
        }
    }
}
