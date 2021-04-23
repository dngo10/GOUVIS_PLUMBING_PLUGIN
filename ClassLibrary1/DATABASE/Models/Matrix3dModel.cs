using ClassLibrary1.HELPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DATABASE.Models
{
    class Matrix3dModel
    {
        public List<double> index = null;
        public long ID = ConstantName.invalidNum;

        public Matrix3dModel(IList<double> index, long ID = ConstantName.invalidNum)
        {
            this.index = new List<double>(index);
            this.ID = ID;
        }
    }
}
