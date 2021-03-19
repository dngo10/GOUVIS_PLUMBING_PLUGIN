using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.ApplicationServices.Core;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetLayoutInFormation
{
    public class Class1
    {

        //THIS IS TO GET LAYOUT
        public static void getlayout()
        {
            Document doc = Application.DocumentManager.CurrentDocument;
            Database db = doc.Database;
            ObjectId LayoutDictId = db.LayoutDictionaryId;

            PlotSettings plotSettings;
            using (doc.LockDocument())
            {
                LayoutManager layMan = LayoutManager.Current;
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    DBDictionary layOutDict = (DBDictionary)tr.GetObject(LayoutDictId, OpenMode.ForRead);
                    foreach (DBDictionaryEntry kv in layOutDict)
                    {
                        string layoutName = kv.Key;
                        Layout layout = (Layout)tr.GetObject(kv.Value, OpenMode.ForRead);
                        layMan.SetCurrentLayoutId(kv.Value);

                        BlockTableRecord layoutBtr = (BlockTableRecord)tr.GetObject(layout.BlockTableRecordId, OpenMode.ForRead);
                        PlotSettingsValidator psVal = PlotSettingsValidator.Current;
                        foreach(ObjectId id in layoutBtr)
                        {
                            DBObject loObj = tr.GetObject(id, OpenMode.ForRead);
                        }
                    }
                }
            }

        }
    }
}
