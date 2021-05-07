using GouvisPlumbingNew.DATABASE.DBModels;
using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DATABASE.DBModels.BaseBlockModel
{
    abstract class BlockModelBase
    {
        public long ID = ConstantName.invalidNum;
        public string handle = ConstantName.invalidStr;
        public Point3dModel position = null;
        public Matrix3dModel matrixTransform = null;
        public DwgFileModel file = null;
    }
}
