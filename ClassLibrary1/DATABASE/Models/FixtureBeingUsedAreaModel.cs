using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DATABASE.Models
{
    class FixtureBeingUsedAreaModel
    {
        //x and y are width and height of the XY dynamic dimension.
        public const string width = "X";
        public const string height = "Y";
        public const string basePoint = "Origin";
        public double X;
        public double Y;
        public Point3dModel origin;
        public Point3dModel pointTop;
        public Point3dModel pointBottom;
        public string handle;
        public Point3dModel position;
        public Matrix3dModel matrixTransform;
    }
}
