using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GouvisPlumbingNew.DATABASE.DBModels
{
    class Point3dModel
    {
        public double X = ConstantName.invalidNum;
        public double Y = ConstantName.invalidNum;
        public double Z = ConstantName.invalidNum;
        public long ID = ConstantName.invalidNum;

        public Point3dModel(double X, double Y, double Z, long ID = ConstantName.invalidNum)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.ID = ID;
        }

        public Point3dModel(IList<double> args, long ID = ConstantName.invalidNum)
        {
            X = args[0];
            Y = args[1];
            Z = args[2];
            this.ID = ID;
        }
    }
}
