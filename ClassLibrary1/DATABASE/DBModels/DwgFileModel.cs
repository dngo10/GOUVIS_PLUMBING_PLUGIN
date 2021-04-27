using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GouvisPlumbingNew.DATABASE.DBModels
{
    class DwgFileModel : IEqualityComparer<DwgFileModel>, IEquatable<DwgFileModel>
    {
        public long ID;
        public string relativePath;
        public long modifieddate;
        public long isP_Notes;

        //CODE BELOW IS USED TO COMPARE FOR HASHSET.

        public bool Equals(DwgFileModel x, DwgFileModel y)
        {
            bool xIsNull = ReferenceEquals(x, null);
            bool yIsNull = ReferenceEquals(y, null);
            if (xIsNull && yIsNull) return true;
            if (xIsNull && !yIsNull) return false;
            if (!xIsNull && yIsNull) return true;

            bool isXempty = string.IsNullOrEmpty(x.relativePath);
            bool isYempty = string.IsNullOrEmpty(y.relativePath);
            if (isXempty && isYempty) { return true; }
            if ((isXempty && !isYempty) || (!isXempty && isYempty)) { return false; }

            return x.relativePath == y.relativePath;
        }

        public bool Equals(DwgFileModel other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }
            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }
            if (this.GetType() != other.GetType())
            {
                return false;
            }

            bool isXempty = string.IsNullOrEmpty(relativePath);
            bool isYempty = string.IsNullOrEmpty(other.relativePath);
            if (isXempty && isYempty) { return true; }
            if ((isXempty && !isYempty) || (!isXempty && isYempty)) { return false; }

            return relativePath == other.relativePath;

        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, this))
            {
                DwgFileModel fe = obj as DwgFileModel;
                return Equals(fe);
            }
            return false;
        }

        public int GetHashCode(DwgFileModel obj)
        {
            return obj.relativePath.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.relativePath.GetHashCode();
        }

        public static bool operator ==(DwgFileModel x, DwgFileModel y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(DwgFileModel x, DwgFileModel y)
        {
            return !x.Equals(y);
        }
    }
}
