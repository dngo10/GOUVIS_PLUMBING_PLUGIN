using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.HELPERS
{
    abstract class ObjectData
    {
        public Handle handle;
        public Point3d position;
        public Matrix3d blockTranform;
    }
}
