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
        public const string DwgsPathFile = @"^.+.dwg$";
        public const string DwgsPNoteFile = @"^.+p_notes.dwg$";


        //DatabaseName;

        //CONSTANT PATH
        public static string programFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        public static string projectManagerExePath = @"C:\Users\dngo\source\repos\Plumbing_Gouvis\ProjectManager\bin\x64\Debug\ProjectManager.exe";

        //BATCH COMMAND
        public static string batchCommand = string.Format(@"start """" ""{0}"" %cd%", projectManagerExePath);

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

        public const string databasePostFix = "_Data.db";
        public const string batFileName = "App.bat";

        public const string TEMPPATH = "C:\\Plumbing_Template\\TEMPLATE_FILE_V1_p_notes.dwg";
    }

    //DATABASE TABLE

}
