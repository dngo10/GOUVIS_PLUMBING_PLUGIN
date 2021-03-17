using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.HELPERS
{
    class InsertPoint : ObjectData
    {
        public string NAME;
        public string ALIAS;
        public ItemHold item;

        public InsertPoint(BlockReference bref, Transaction tr)
        {
            handle = bref.Handle;
            position = bref.Position;
            foreach(ObjectId id in bref.AttributeCollection)
            {
                AttributeReference aRef = (AttributeReference)tr.GetObject(id, OpenMode.ForRead);
                if(aRef.Tag == InsertPointName.NAME)
                {
                    NAME = aRef.TextString;
                }else if(aRef.Tag == InsertPointName.ALIAS)
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
    }

    class InsertPointName
    {
        public static string NAME = "NAME";
        public static string ALIAS = "ALIAS";
    }

    class ItemHold
    {
        public Handle itemHandle;
        public Point3d itemPosition;
    }
}
