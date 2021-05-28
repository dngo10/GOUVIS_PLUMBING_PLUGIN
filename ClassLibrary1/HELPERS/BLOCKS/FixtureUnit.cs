using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using ClassLibrary1.DATABASE.DBModels;
using GouvisPlumbingNew.DATABASE.DBModels;
using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.HELPERS.BLOCKS
{
    class FixtureUnit
    {
        public FixtureUnitModel model;

        public FixtureUnit(BlockReference bRef, Transaction tr)
        {
            model = new FixtureUnitModel();
            model.position = new Point3dModel(bRef.Position.ToArray());
            model.matrixTransform = new Matrix3dModel(bRef.BlockTransform.ToArray());
            model.blockName = Goodies.GetDynamicName(bRef);

            Point3d v = ConstantNameNoExecutable.inValidPoint;
            Point3d m = ConstantNameNoExecutable.inValidPoint;
            Point3d r1 = ConstantNameNoExecutable.inValidPoint;
            foreach (DynamicBlockReferenceProperty prop in bRef.DynamicBlockReferencePropertyCollection)
            {
                

                if(prop.PropertyName == FixtureUnitModelName.A1)
                {
                    model.A1 = (double)prop.Value;
                } else if(prop.PropertyName == FixtureUnitModelName.A2)
                {
                    model.A2 = (double)prop.Value;
                } else if(prop.PropertyName == FixtureUnitModelName.A3)
                {
                    model.A3 = (double)prop.Value;
                } else if(prop.PropertyName == FixtureUnitModelName.D1)
                {
                    model.D1 = (double)prop.Value;
                } else if(prop.PropertyName == FixtureUnitModelName.Y2)
                {
                    model.Y2 = (double)prop.Value;
                } else if(prop.PropertyName == FixtureUnitModelName.X2)
                {
                    model.X2 = (double)prop.Value;
                } else if(prop.PropertyName == FixtureUnitModelName.X2_2)
                {
                    model.X2_2 = (double)prop.Value;
                }else if(prop.PropertyName == FixtureUnitModelName.V)
                {
                    v = (Point3d)prop.Value;
                    model.V = new Point3dModel(v.X, v.Y, v.Z);
                }
                else if (prop.PropertyName == FixtureUnitModelName.M)
                {
                    m = (Point3d)prop.Value;
                    model.M = new Point3dModel(m.X, m.Y, m.Z);
                }
                else if (prop.PropertyName == FixtureUnitModelName.R1)
                {
                    r1 = (Point3d)prop.Value;
                    model.R1 = new Point3dModel(r1.X, r1.Y, r1.Z);
                }
            }

            foreach(ObjectId id in bRef.AttributeCollection)
            {
                AttributeReference attRef = (AttributeReference)tr.GetObject(id, OpenMode.ForRead);
                if (attRef.Tag == FixtureUnitModelName.CWSFU)
                {
                    model.CWSFU = Convert.ToDouble(attRef.TextString);
                }
                else if (attRef.Tag == FixtureUnitModelName.WSFU)
                {
                    model.WSFU = Convert.ToDouble(attRef.TextString);
                }
                else if (attRef.Tag == FixtureUnitModelName.HWSFU)
                {
                    model.HWSFU = Convert.ToDouble(attRef.TextString);
                }
                else if (attRef.Tag == FixtureUnitModelName.VENT_DIA)
                {
                    model.VENT_DIA = Convert.ToDouble(attRef.TextString);
                }
                else if(attRef.Tag == FixtureUnitModelName.WASTE_DIA)
                {
                    model.WASTE_DIA = Convert.ToDouble(attRef.TextString);
                }
                else if(attRef.Tag == FixtureUnitModelName.STORM_DIA)
                {
                    model.STORM_DIA = Convert.ToDouble(attRef.TextString);
                }
                else if(attRef.Tag == FixtureUnitModelName.DFU)
                {
                    model.DFU = Convert.ToDouble(attRef.TextString);
                }    
                else if(attRef.Tag == FixtureUnitModelName.HW_DIA)
                {
                    model.HW_DIA = Convert.ToDouble(attRef.TextString);
                }    
                else if(attRef.Tag == FixtureUnitModelName.CW_DIA)
                {
                    model.CW_DIA = Convert.ToDouble(attRef.TextString);
                }    
                else if(attRef.Tag == FixtureUnitModelName.INDEX)
                {
                    model.INDEX = Convert.ToDouble(attRef.TextString);
                }else if(attRef.Tag == FixtureUnitModelName.TAG)
                {
                    throw new Exception("Check This Out.");
                }
            }

            if(v != ConstantNameNoExecutable.inValidPoint)
            {
                Point3d drawingPoint = v.TransformBy(bRef.BlockTransform);
                model.ventPos = new Point3dModel(drawingPoint.X, drawingPoint.Y, drawingPoint.Z);
            }

            if(m != ConstantNameNoExecutable.inValidPoint)
            {
                Point3d drawingPoint = m.TransformBy(bRef.BlockTransform);
                model.tagPos = new Point3dModel(drawingPoint.X, drawingPoint.Y, drawingPoint.Z);
            }

            if(r1 != ConstantNameNoExecutable.inValidPoint)
            {
                if(model.A3 != ConstantName.invalidNum)
                {
                    model.drainPos = new Point3dModel(bRef.Position.X, bRef.Position.Y, bRef.Position.Z);
                }
                else
                {
                    Point3d point = r1.TransformBy(bRef.BlockTransform);
                    model.drainPos = new Point3dModel(point.X, point.Y, point.Z);
                }
            }

            if(model.A1 != ConstantName.invalidNum)
            {
                //Cold Water Only
                if(model.HW_DIA == ConstantName.invalidNum && )
                {
                    Point3d point = bRef.Position;
                    Point3d coldWaterTipPoint = new Point3d(bRef.X +  )
                }
            }
        }

        /// <summary>
        /// Get Drain Position, Drawing Position.
        /// If Double Drain return 
        /// </summary>
        /// <returns></returns>
        private Point3d GetDoubleDrainTipPoint(BlockReference bref)
        {
            if(model.Y2 != ConstantName.invalidNum && model.A3 != ConstantName.invalidNum && model.drainPos != null && model.V != null)
            {
                Point3d basePoint = new Point3d(model.R1.X, model.R1.Y, model.R1.Z);

                Point3d point = new Point3d(model.R1.X, model.R1.Y + model.Y2, model.R1.Z);
                
                Point3d point1 = point.RotateBy(model.A3, Vector3d.ZAxis, basePoint);

                point1 = point1.TransformBy(bref.BlockTransform);

                return point1;
            }
            else
            {
                return ConstantNameNoExecutable.inValidPoint;
            }
        }
    }
}
