using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1.HELPERS;
using Autodesk.AutoCAD.Geometry;

namespace ClassLibrary1.HELPERS
{
    class FixtureBeingUsedArea : ObjectData
    {
        //x and y are width and height of the XY dynamic dimension.
        const string width = "X";
        const string height = "Y";
        const string basePoint = "Origin";
        public double X;
        public double Y;
        public Point3d origin;
        public Point3d pointTop;
        public Point3d pointBottom;

        public List<FixtureDetails> FDList = new List<FixtureDetails>();

        public FixtureBeingUsedArea(BlockReference block)
        {
            GetTopAndBottomPoint(block);
        }

        private void GetTopAndBottomPoint(BlockReference block)
        {
            handle = block.Handle;
            position = block.Position;
            DynamicBlockReferencePropertyCollection dynBlockPropCol = block.DynamicBlockReferencePropertyCollection;
            foreach(DynamicBlockReferenceProperty dynProp in dynBlockPropCol)
            {
                if (dynProp.PropertyName.Equals(width)){
                    X = (double)dynProp.Value;
                }else if (dynProp.PropertyName.Equals(height))
                {
                    Y = (double)dynProp.Value;
                }else if (dynProp.PropertyName.Equals(basePoint))
                {
                    origin = (Point3d)dynProp.Value;
                }
            }

            pointTop = new Point3d(origin.X, origin.Y, 0);

            //Use the regular X,Y coordinate, DO NOT use Game or Web coordinate
            pointBottom = new Point3d(origin.X + X, origin.Y - Y, 0);
            pointTop = pointTop.TransformBy(block.BlockTransform);
            pointBottom = pointBottom.TransformBy(block.BlockTransform);
        }

        private bool IsInsideTheBox(Point3d point)
        {
            bool xInside = (point.X > pointTop.X && point.X < pointBottom.X) ||
                           (point.X <= pointTop.X && point.X >= pointBottom.X);

            bool yInside = (point.Y > pointTop.Y && point.Y < pointBottom.Y) ||
                           (point.Y <= pointTop.Y && point.Y >= pointBottom.Y);

            return xInside && yInside;
        }

        public bool IsInsideTheBox(BlockReference bref)
        {
            return IsInsideTheBox(bref.Position);
        }
    }
}
