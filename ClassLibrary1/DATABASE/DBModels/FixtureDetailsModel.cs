using ClassLibrary1.DATABASE.DBModels.BaseBlockModel;
using GouvisPlumbingNew.DATABASE.Controllers;
using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GouvisPlumbingNew.DATABASE.DBModels
{
    public class FixtureDetailsModel : BlockModelBase
    {
        public double INDEX = ConstantName.invalidNum;
        public string FIXTURENAME = ConstantName.invalidStr;
        public string TAG = ConstantName.invalidStr;
        public string NUMBER = ConstantName.invalidStr;
        public double CW_DIA = ConstantName.invalidNum;
        public double HW_DIA = ConstantName.invalidNum;
        public double WASTE_DIA = ConstantName.invalidNum;
        public double VENT_DIA = ConstantName.invalidNum;
        public double STORM_DIA = ConstantName.invalidNum;
        public double WSFU = ConstantName.invalidNum;
        public double CWSFU = ConstantName.invalidNum;
        public double HWSFU = ConstantName.invalidNum;
        public double DFU = ConstantName.invalidNum;
        public string DESCRIPTION = ConstantName.invalidStr;

        public void WriteToDatabase(SQLiteConnection connection)
        {
            

            //File must be inserted to Database first (meaning it must have ID).

            if(!DBFixtureDetails.HasRow(connection, handle, file.ID))
            {
                WriteToDatabase0(connection);
                var temp = this;
                ID = DBFixtureDetails.InsertRow(connection, ref temp);
            }
            else
            {
                FixtureDetailsModel model = DBFixtureDetails.SelectRow(connection, handle, file.ID);
                ID = model.ID;

                UpdateToDatabase0(model.matrixTransform.ID, model.position.ID, connection);
                DBFixtureDetails.UpdateRow(connection, this);
            }

        }
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
