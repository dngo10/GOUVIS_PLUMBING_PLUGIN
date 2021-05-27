using ClassLibrary1.DATABASE.Controllers;
using ClassLibrary1.DATABASE.DBModels.BaseBlockModel;
using GouvisPlumbingNew.DATABASE.DBModels;
using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DATABASE.DBModels
{
    class FixtureUnitModel : BlockModelBase
    {
        public double INDEX = ConstantName.invalidNum;
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
        public string blockName = ConstantName.invalidStr;

        public Point3dModel tagPos = null;
        public Point3dModel ventPos = null;
        public Point3dModel drainPos = null;
        public Point3dModel hotStub = null;
        public Point3dModel coldStub = null;
        public long drainType = ConstantName.invalidNum;
        public double studLength = ConstantName.invalidNum;

        public Point3dModel R1 = null; // Drain Position
        public double A2 = ConstantName.invalidNum; //Vent Angle
        public double Y2 = ConstantName.invalidNum; //For Double
        public double X2 = ConstantName.invalidNum;
        public double X2_2 = ConstantName.invalidNum;
        public double A3 = ConstantName.invalidNum; // Drain Angle
        public double A1 = ConstantName.invalidNum; //HC angle
        public double D1 = ConstantName.invalidNum;
        public Point3dModel V = null; // Vent Position
        public Point3dModel M = null;// Tag Position

        public void WriteToDabase(SQLiteConnection connection)
        {
            if(!DBFixture_Unit.HasRow(connection, handle, file.ID))
            {
                var temp = this;
                WriteToDatabase0(connection);
                if (tagPos != null) tagPos.WriteToDatabase(connection);
                if (ventPos != null) ventPos.WriteToDatabase(connection);
                if (drainPos != null) drainPos.WriteToDatabase(connection);
                if (hotStub != null) hotStub.WriteToDatabase(connection);
                if (coldStub != null) coldStub.WriteToDatabase(connection);
                if (R1 != null) R1.WriteToDatabase(connection);
                if (V != null) V.WriteToDatabase(connection);

                //Tag can never null
                M.WriteToDatabase(connection);
                ID = DBFixture_Unit.InsertRow(connection, ref temp);
            }
            else
            {
                FixtureUnitModel model = DBFixture_Unit.SelectRow(connection, handle, file.ID);
                ID = model.ID;
                UpdateToDatabase0(model.matrixTransform.ID, model.position.ID, connection);

                if (model.tagPos != null) { tagPos.ID = model.tagPos.ID; tagPos.WriteToDatabase(connection); }
                if (model.ventPos != null) { ventPos.ID = model.ventPos.ID; ventPos.WriteToDatabase(connection); }
                if (model.drainPos != null) { drainPos.ID = model.drainPos.ID; drainPos.WriteToDatabase(connection); }
                if (model.hotStub != null) { hotStub.ID = model.hotStub.ID; hotStub.WriteToDatabase(connection); }
                if (model.coldStub != null) { coldStub.ID = model.coldStub.ID; coldStub.WriteToDatabase(connection); }
                if (model.R1 != null) { R1.ID = model.R1.ID; R1.WriteToDatabase(connection); }
                if (model.V != null) { V.ID = model.V.ID; V.WriteToDatabase(connection); }
                M.ID = model.M.ID; M.WriteToDatabase(connection);

                DBFixture_Unit.UpdateRow(connection, this);

            }
        }
    }
}
