using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.HELPERS
{
    class GetXYDynamicPoints
    {
        /// <summary>
        /// Get XYPoints Location of the BLOCK, these are for information points only, you can't
        /// edit Block using these points.
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="dynBlock"></param>
        /// <returns></returns>
        public static HashSet<Point3d> GetXYGripPoints(Transaction tr, BlockReference dynBlock)
        {
            if (dynBlock.IsDynamicBlock)
            {
                DynamicBlockReferencePropertyCollection dynPropCol = dynBlock.DynamicBlockReferencePropertyCollection;
                foreach(DynamicBlockReferenceProperty dynProp in dynPropCol)
                {
                    
                }
            }
            return null;
        }
    }
}
