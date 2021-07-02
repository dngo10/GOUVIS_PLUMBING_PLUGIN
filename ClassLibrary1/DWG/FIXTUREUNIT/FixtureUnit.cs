using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.FIXTUREUNIT
{
    class FixtureUnitInsert
    {
        public static void InsertFixtureUnit(FixtureDetails fd)
        {

            Goodies.AddBlockToActiveDrawing("C:\\Users\\dngo\\Desktop\\SAMPLES\\VERSION1.dwg", "FIX_26");
            

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            Database db = doc.Database;

            BlockReference bref =  null;
            Dictionary<ObjectId, ObjectId>  blockToInsert = Goodies.InsertDynamicBlock("FIX_26", db, ref bref);

            using (doc.LockDocument())
            {
                using(Transaction tr = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForRead); 
                }    
            }
        }

        public static void GetCorrectFixture


    }
}
