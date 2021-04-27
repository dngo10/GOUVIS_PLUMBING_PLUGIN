using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GouvisPlumbingNew.HELPERS
{
    class InsertPoint : ObjectData
    {
        public string NAME;
        public string ALIAS;
        public ItemHold item;

        public InsertPoint(BlockReference bref, Transaction tr)
        {
            FilloutVariables(bref, tr);
        }

        private void FilloutVariables(BlockReference bref, Transaction tr)
        {
            handle = bref.Handle;
            position = bref.Position;
            foreach (ObjectId id in bref.AttributeCollection)
            {
                AttributeReference aRef = (AttributeReference)tr.GetObject(id, OpenMode.ForRead);
                if (aRef.Tag == InsertPointName.NAME)
                {
                    NAME = aRef.TextString;
                }
                else if (aRef.Tag == InsertPointName.ALIAS)
                {
                    ALIAS = aRef.TextString;
                }
            }
        }

        public void addTable(Point3d position, DBObject obj)
        {
            if(this.position.DistanceTo(position) < 0.5)
            {
                item = new ItemHold();
                item.itemHandle = obj.Handle;
                item.itemPosition = position;
            }
        }

        //Database Must be in write mode
        //This is to update location in case it is moved;
        public bool UpdateHandle(Database db)
        {
            bool result = false;
            BlockReference bref = (BlockReference)Goodies.GetDBObjFromHandle(handle, db);
            if(bref != null && !bref.IsErased)
            {
                using(Transaction tr = db.TransactionManager.StartTransaction())
                {
                    FilloutVariables(bref, tr);
                    result = true;
                }
            }

            //Set Item Back To null;
            if(item != null)
            {
                item.DeleteItemHold(db);
                item = null;
            }
            return result;
        }
    }

    class InsertPointName
    {
        public static string NAME = "NAME";
        public static string ALIAS = "ALIAS";
    }

    //ItemHold can ONLY HOLD TABLE AND BLOCK (things that have POSITION);
    class ItemHold
    {
        public Handle itemHandle;
        public Point3d itemPosition;

        // Dabatabase must be writable
        public void DeleteItemHold(Database db)
        {
            BlockReference bref = (BlockReference)Goodies.GetDBObjFromHandle(itemHandle, db);
            if(bref != null && !bref.IsErased)
            {
                using(Transaction tr = db.TransactionManager.StartTransaction())
                {
                    bref.Erase();
                    tr.Commit();
                }
            }
        }
    }
}
