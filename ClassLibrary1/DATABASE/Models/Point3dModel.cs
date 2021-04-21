﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DATABASE.Models
{
    class Point3dModel
    {
        public double X;
        public double Y;
        public double Z;
        public long ID;

        public Point3dModel(double X, double Y, double Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public Point3dModel(IList<double> args)
        {
            X = args[0];
            Y = args[1];
            Z = args[2];
        }
    }
}
