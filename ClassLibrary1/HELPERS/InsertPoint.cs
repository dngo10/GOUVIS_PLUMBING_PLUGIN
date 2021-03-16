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
    class InsertPoint
    {
        public string NAME;
        public string ALIAS;
        public Point3d position;
        public Handle handle;

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
    }

    class InsertPointName
    {
        public static string NAME = "NAME";
        public static string ALIAS = "ALIAS";
    }
}
