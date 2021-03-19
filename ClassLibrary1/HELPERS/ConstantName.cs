using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.HELPERS
{
    class ConstantName
    {
        //BLOCK NAMES
        public const string FixtureInformationArea = "FixtureBeingUsedArea";
        public const string FixtureDetailsBox = "FixtureDetails";
        public const string InsertPoint = "InsertPoint";
        public const string HexNote = "Hex-Note";

        //LAYER NAMES
        public const string TABLE = "TABLE";

        //INVALID_NUMBER
        //RETURN THIS EVERYTIME VALUE BECOME INVALID;
        public const int invalid = -2100000000;
        public static Point3d inValidPoint = new Point3d(invalid, invalid, invalid);

        //PATTERN FOR
        /// <summary>
        /// fsPattern: Pattern for regular expression of Alisa.
        /// </summary>
        public const string fsPattern = "^[F f][S s].*$";


        //DatabaseName;

        //CONSTANT PATH
        public static string programFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
    }
}
