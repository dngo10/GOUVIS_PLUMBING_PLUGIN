using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GouvisPlumbingNew.HELPERS;
using Autodesk.AutoCAD.Geometry;
using GouvisPlumbingNew.DATABASE.DBModels;
using GouvisPlumbingNew.DATABASE.Controllers;
using System.Data.SQLite;

namespace GouvisPlumbingNew.HELPERS
{
    public class FixtureBeingUsedArea : ObjectData
    {
        //x and y are width and height of the XY dynamic dimension.

        public FixtureBeingUsedAreaModel model = null;

        //public List<FixtureDetails> FDList = new List<FixtureDetails>();

        public FixtureBeingUsedArea(BlockReference block)
        {
            GetTopAndBottomPoint(block);
        }

        public FixtureBeingUsedArea(FixtureBeingUsedAreaModel model)
        {
            this.model = model;
        }

        private void GetTopAndBottomPoint(BlockReference block)
        {
            model = new FixtureBeingUsedAreaModel();
            model.handle = Goodies.ConvertHandleToString(block.Handle);
            model.position = new Point3dModel(block.Position.ToArray());
            model.matrixTransform = new Matrix3dModel(block.BlockTransform.ToArray());

            DynamicBlockReferencePropertyCollection dynBlockPropCol = block.DynamicBlockReferencePropertyCollection;
            foreach(DynamicBlockReferenceProperty dynProp in dynBlockPropCol)
            {
                if (dynProp.PropertyName.Equals(DBFixtureBeingUsedAreaName.X)){
                    model.X = (double)dynProp.Value;
                }else if (dynProp.PropertyName.Equals(DBFixtureBeingUsedAreaName.Y))
                {
                    model.Y = (double)dynProp.Value;
                }else if (dynProp.PropertyName.Equals("Origin"))
                {
                    model.origin = new Point3dModel(((Point3d)dynProp.Value).ToArray());
                }
            }

            Point3d pointTop = new Point3d(model.origin.X, model.origin.Y, 0);

            //Use the regular X,Y coordinate, DO NOT use Game or Web coordinate
            Point3d pointBottom = new Point3d(model.origin.X + model.X, model.origin.Y - model.Y, 0);
            pointTop = pointTop.TransformBy(block.BlockTransform);
            pointBottom = pointBottom.TransformBy(block.BlockTransform);

            model.pointBottom = new Point3dModel(pointBottom.ToArray());
            model.pointTop = new Point3dModel(pointTop.ToArray());
        }

        private bool IsInsideTheBox(Point3d point)
        {
            bool xInside = (point.X > model.pointTop.X && point.X < model.pointBottom.X) ||
                           (point.X <= model.pointTop.X && point.X >= model.pointBottom.X);

            bool yInside = (point.Y > model.pointTop.Y && point.Y < model.pointBottom.Y) ||
                           (point.Y <= model.pointTop.Y && point.Y >= model.pointBottom.Y);

            return xInside && yInside;
        }

        public bool IsInsideTheBox(FixtureDetails fd)
        {
            Point3d pos = new Point3d(fd.model.position.X, fd.model.position.Y, fd.model.position.Z);
            return IsInsideTheBox(pos);
        }

        public bool IsInsideTheBox(BlockReference bref)
        {
            return IsInsideTheBox(bref.Position);
        }


    }
}
