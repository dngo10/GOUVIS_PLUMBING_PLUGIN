using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DATABASE.Controllers.BlockInterFace
{

    abstract class DBBlockName
    {
        public const string ID = "ID";
        public const string HANDLE = "HANDLE";
        public const string POSITION_ID = "POSITION_ID";
        public const string MATRIX_ID = "MATRIX_ID";
        public const string FILE_ID = "FILE_ID";
    }

    abstract class DBBlockName_AT
    {
        public const string name = "@name";
        public const string id = "@id";
        public const string handle = "@handle";
        public const string position = "@position";
        public const string matrix = "@matrix";
        public const string file = "@file";
    }
}
