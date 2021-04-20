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
        public List<double> origin;
        public List<double> pointTop;
        public List<double> pointBottom;
        public string handle;
        public List<double> position;
    }
}
