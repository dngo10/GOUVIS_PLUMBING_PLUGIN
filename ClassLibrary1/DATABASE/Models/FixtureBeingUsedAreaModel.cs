using ClassLibrary1.HELPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DATABASE.Models
{
    class FixtureBeingUsedAreaModel
    {
        public double X = ConstantName.invalidNum;
        public double Y = ConstantName.invalidNum;
        public Point3dModel origin = null;
        public Point3dModel pointTop = null;
        public Point3dModel pointBottom = null;
        public string handle = ConstantName.invalidStr;
        public long ID = ConstantName.invalidNum;
        public Point3dModel position = null;
        public Matrix3dModel matrixTransform = null;
        public DwgFileModel file = null;
    }
}
