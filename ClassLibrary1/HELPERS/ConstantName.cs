using System;

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


        //PATTERN FOR
        /// <summary>
        /// fsPattern: Pattern for regular expression of Alisa.
        /// </summary>
        public const string fsPattern = "^[F f][S s].*$";
        public const string projectNumberPattern = @"^.*(\d{5})(.*)$";
        public static string databaseFolderPattern = string.Format(@"^.*{0}$", centerFolder);


        //DatabaseName;

        //CONSTANT PATH
        public static string programFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

        public const string PlumbingFolderName = "PLBG";


        //XDRIVE PROJECT
        public const string XDriveProject = @"\\Ge-fs1\mep\Projects";
        public const string XDriveHuanProject = @"\\Ge-fs1\mep\HUAN'S MEP PROJECTS";
        public const string XDrive = @"\\Ge-fs1\mep";

        //CONSTANT JSON
        //These are prefix, this will be added with project number for eachProject
        public const string jsonFilePrefix = "ProjData";
        public const string batFilePrefix = "Run";
        public const string centerFolder = "_Manager";

        public const string databasePostFix = "_Data";

        

    }

    //DATABASE TABLE

}
