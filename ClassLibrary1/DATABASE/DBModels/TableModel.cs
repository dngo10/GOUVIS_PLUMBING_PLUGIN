using GouvisPlumbingNew.DATABASE.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DATABASE.DBModels
{
    class TableModel
    {
        public long ID;
        public string ALIAS;
        public string A_VALUE;
        public Point3dModel position;
        public Matrix3dModel matrixTransform;
        public DwgFileModel file;
        public string HANDLE;
    }
}
