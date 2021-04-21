using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1.HELPERS;
using Autodesk.AutoCAD.Geometry;
using ClassLibrary1.DATABASE.Models;

namespace ClassLibrary1.HELPERS
{
    class FixtureBeingUsedArea : ObjectData
    {
        //x and y are width and height of the XY dynamic dimension.

        FixtureBeingUsedAreaModel fixtureBeingUsedAreaModel;

        //public List<FixtureDetails> FDList = new List<FixtureDetails>();

        public FixtureBeingUsedArea(BlockReference block)
        {
            GetTopAndBottomPoint(block);
        }

        private void GetTopAndBottomPoint(BlockReference block)
        {
            fixtureBeingUsedAreaModel.handle  = block.Handle.ToString();
            fixtureBeingUsedAreaModel.position = new Point3dModel(block.Position.ToArray());
            fixtureBeingUsedAreaModel.matrixTransform = new Matrix3dModel(block.BlockTransform.ToArray());

            DynamicBlockReferencePropertyCollection dynBlockPropCol = block.DynamicBlockReferencePropertyCollection;
            foreach(DynamicBlockReferenceProperty dynProp in dynBlockPropCol)
            {
                if (dynProp.PropertyName.Equals(FixtureBeingUsedAreaModel.width)){
                    fixtureBeingUsedAreaModel.X = (double)dynProp.Value;
                }else if (dynProp.PropertyName.Equals(FixtureBeingUsedAreaModel.height))
                {
                    fixtureBeingUsedAreaModel.Y = (double)dynProp.Value;
                }else if (dynProp.PropertyName.Equals(FixtureBeingUsedAreaModel.basePoint))
                {
                    fixtureBeingUsedAreaModel.origin = new Point3dModel(((Point3d)dynProp.Value).ToArray());
                }
            }

            Point3d pointTop = new Point3d(fixtureBeingUsedAreaModel.origin.X, fixtureBeingUsedAreaModel.origin.Y, 0);

            //Use the regular X,Y coordinate, DO NOT use Game or Web coordinate
            Point3d pointBottom = new Point3d(fixtureBeingUsedAreaModel.origin.X + fixtureBeingUsedAreaModel.X, fixtureBeingUsedAreaModel.origin.Y - fixtureBeingUsedAreaModel.Y, 0);
            pointTop = pointTop.TransformBy(block.BlockTransform);
            pointBottom = pointBottom.TransformBy(block.BlockTransform);

            fixtureBeingUsedAreaModel.pointBottom = new Point3dModel(pointBottom.ToArray());
            fixtureBeingUsedAreaModel.pointTop = new Point3dModel(pointTop.ToArray());
        }

        private bool IsInsideTheBox(Point3d point)
        {
            bool xInside = (point.X > fixtureBeingUsedAreaModel.pointTop.X && point.X < fixtureBeingUsedAreaModel.pointBottom.X) ||
                           (point.X <= fixtureBeingUsedAreaModel.pointTop.X && point.X >= fixtureBeingUsedAreaModel.pointBottom.X);

            bool yInside = (point.Y > fixtureBeingUsedAreaModel.pointTop.Y && point.Y < fixtureBeingUsedAreaModel.pointBottom.Y) ||
                           (point.Y <= fixtureBeingUsedAreaModel.pointTop.Y && point.Y >= fixtureBeingUsedAreaModel.pointBottom.Y);

            return xInside && yInside;
        }

        public bool IsInsideTheBox(BlockReference bref)
        {
            return IsInsideTheBox(bref.Position);
        }
    }
}
