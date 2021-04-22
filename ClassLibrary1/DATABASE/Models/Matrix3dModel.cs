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
        public List<double> index;
        public long ID;

        public Matrix3dModel(IList<double> index, long ID = ConstantName.invalid)
        {
            this.index = new List<double>(index);
            this.ID = ID;
        }
    }
}
