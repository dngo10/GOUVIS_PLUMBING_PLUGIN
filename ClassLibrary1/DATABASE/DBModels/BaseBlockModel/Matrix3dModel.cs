using GouvisPlumbingNew.DATABASE.Controllers;
using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GouvisPlumbingNew.DATABASE.DBModels
{
    public class Matrix3dModel
    {
        public List<double> index = null;
        public long ID = ConstantName.invalidNum;

        public Matrix3dModel(IList<double> index, long ID = ConstantName.invalidNum)
        {
            this.index = new List<double>(index);
            this.ID = ID;
        }

        public void WriteToDatabase(SQLiteConnection connection)
        {
            if(!DBMatrix3d.HasRow(connection, ID))
            {
                var temp = this;
                ID = DBMatrix3d.Insert(connection, ref temp);
            }
            else
            {
                DBMatrix3d.Update(connection, this);
            }
        }

        /// <summary>
        /// Transform with rotation, this means it is already tranformed back to 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public Point3dModel Transform(double r, Point3dModel point)
        {
            List<double> rotationMatrix = new List<double>{ Math.Cos(r), -Math.Sin(r), 0, 0,
                                                            Math.Sin(r), Math.Cos(r), 0, 0,
                                                            0, 0, 1, 0,
                                                            0, 0, 0, 1
                                                            };
            Matrix3dModel T1 = new Matrix3dModel(rotationMatrix);

            return this.Transform(T1.Transform(point));
        }

        /// <summary>
        /// Transform 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="origin"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public Point3dModel Transform(double r, Point3dModel origin, Point3dModel point)
        {
            List<double> arraTranform1 = new List<double> { 1, 0, 0, -origin.X,
                                                            0, 1, 0, -origin.Y,
                                                            0, 0, 1, -origin.Z,
                                                            0, 0, 0, 1};
            Matrix3dModel T1 = new Matrix3dModel(arraTranform1);

            List<double> rotationMatrix = new List<double>{ Math.Cos(r), -Math.Sin(r), 0, 0,
                                                            Math.Sin(r), Math.Cos(r), 0, 0,
                                                            0, 0, 1, 0,
                                                            0, 0, 0, 1
                                                            };
            Matrix3dModel T2 = new Matrix3dModel(rotationMatrix);

            List<double> arraTranform2 = new List<double> { 1, 0, 0, origin.X,
                                                            0, 1, 0, origin.Y,
                                                            0, 0, 1, origin.Z,
                                                            0, 0, 0, 1};
            Matrix3dModel T3 = new Matrix3dModel(arraTranform2);

            Point3dModel tfBack = T1.Transform(point);
            Point3dModel tfRotate = T2.Transform(tfBack);
            Point3dModel tfMove = T3.Transform(tfRotate);
            Point3dModel tfBlockTransform = this.Transform(tfMove);

            return tfBlockTransform;
        }
        public Point3dModel Transform(Point3dModel point)
        {
            List<double> pointList = new List<double> { point.X, point.Y, point.Z, 1 };
            List<double> newPoint = MatrixTransForm(pointList);

            return new Point3dModel(newPoint);
        }

        public Matrix3dModel MatrixTransForm(Matrix3dModel matrix)
        {
            double i1 =     index[0] *matrix.index[0] + index[1] *matrix.index[4] + index[2] *matrix.index[8]  + index[3] *matrix.index[12];
            double i2 =     index[0] *matrix.index[1] + index[1] *matrix.index[5] + index[2] *matrix.index[9]  + index[3] *matrix.index[13];
            double i3 =     index[0] *matrix.index[2] + index[1] *matrix.index[6] + index[2] *matrix.index[10] + index[3] *matrix.index[14];
            double i4 =     index[0] *matrix.index[3] + index[1] *matrix.index[7] + index[2] *matrix.index[11] + index[3] *matrix.index[15];
            double i5 =     index[4] *matrix.index[0] + index[5] *matrix.index[4] + index[6] *matrix.index[8]  + index[7] *matrix.index[12];
            double i6 =     index[4] *matrix.index[1] + index[5] *matrix.index[5] + index[6] *matrix.index[9]  + index[7] *matrix.index[13];
            double i7 =     index[4] *matrix.index[2] + index[5] *matrix.index[6] + index[6] *matrix.index[10] + index[7] *matrix.index[14];
            double i8 =     index[4] *matrix.index[3] + index[5] *matrix.index[7] + index[6] *matrix.index[11] + index[7] *matrix.index[15];
            double i9 =     index[8] *matrix.index[0] + index[9] *matrix.index[4] + index[10]*matrix.index[8]  + index[11]*matrix.index[12];
            double i10 =    index[8] *matrix.index[1] + index[9] *matrix.index[5] + index[10]*matrix.index[9]  + index[11]*matrix.index[13];
            double i11 =    index[8] *matrix.index[2] + index[9] *matrix.index[6] + index[10]*matrix.index[10] + index[11]*matrix.index[14];
            double i12 =    index[8] *matrix.index[3] + index[9] *matrix.index[7] + index[10]*matrix.index[11] + index[11]*matrix.index[15];
            double i13 =    index[12]*matrix.index[0] + index[13]*matrix.index[4] + index[14]*matrix.index[8]  + index[15]*matrix.index[12];
            double i14 =    index[12]*matrix.index[1] + index[13]*matrix.index[5] + index[14]*matrix.index[9]  + index[15]*matrix.index[13];
            double i15 =    index[12]*matrix.index[2] + index[13]*matrix.index[6] + index[14]*matrix.index[10] + index[15]*matrix.index[14];
            double i16 =    index[12]*matrix.index[3] + index[13]*matrix.index[7] + index[14]*matrix.index[11] + index[15]*matrix.index[15];

            List<double> newList = new List<double>
            {
                i1,  i2,  i3,  i4,
                i5,  i6,  i7,  i8,
                i9,  i10, i11, i12,
                i13, i14, i15, i16
            };

            return new Matrix3dModel(newList);
        }

        public List<double> MatrixTransForm(List<double> v)
        {
            return new List<double>{
                index[0]* v[0] + index[1]* v[1] + index[2]* v[2] + index[3]* v[3],
                index[4]* v[0] + index[5]* v[1] + index[6]* v[2] + index[7]* v[3],
                index[8]* v[0] + index[9]* v[1] + index[10]*v[2] + index[11]*v[3],
                index[12]*v[0] + index[13]*v[1] + index[14]*v[2] + index[15]*v[3],
            };
        }
    }
}
