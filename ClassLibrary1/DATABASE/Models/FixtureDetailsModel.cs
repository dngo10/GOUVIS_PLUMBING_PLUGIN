using ClassLibrary1.HELPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DATABASE.Models
{
    class FixtureDetailsModel
    {
        public string handle = "";
        public Point3dModel position;
        public Matrix3dModel matrixTransform;
        public double INDEX = ConstantName.invalid;
        public string FIXTURENAME = "";
        public string TAG = "";
        public double NUMBER = ConstantName.invalid;
        public double CW_DIA = ConstantName.invalid;
        public double HW_DIA = ConstantName.invalid;
        public double WASTE_DIA = ConstantName.invalid;
        public double VENT_DIA = ConstantName.invalid;
        public double STORM_DIA = ConstantName.invalid;
        public double WSFU = ConstantName.invalid;
        public double CWSFU = ConstantName.invalid;
        public double HWSFU = ConstantName.invalid;
        public double DFU = ConstantName.invalid;
        public string DESCRIPTION = "";
        public long ID;
    }

    static class FixtureDetailsName
    {
        public static string index = "INDEX";
        public static string FixtureName = "FIXTURE_NAME";
        public static string tag = "TAG";
        public static string number = "NUMBER";
        public static string CW_DIA = "CW_DIA";
        public static string HW_DIA = "HW_DIA";
        public static string WASTE_DIA = "WASTE_DIA";
        public static string VENT_DIA = "VENT_DIA";
        public static string STORM_DIA = "STORM_DIA";
        public static string WSFU = "WSFU";
        public static string CWSFU = "CWSFU";
        public static string HWSFU = "HWSFU";
        public static string DFU = "DFU";
        public static string DESCRIPTION = "DESCRIPTION";
    }
}
