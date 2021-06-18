using ClassLibrary1.DATABASE.Controllers;
using ClassLibrary1.DATABASE.DBModels.BaseBlockModel;
using ClassLibrary1.HELPERS.BLOCKS.FixtureUnitBlocks;
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

        public Point3dModel R1 = null; // Drain Position
        public double A2 = ConstantName.invalidNum; //Vent Angle
        public double Y2 = ConstantName.invalidNum; //For Double
        public double X2 = ConstantName.invalidNum;
        public double X2_2 = ConstantName.invalidNum;
        public double A3 = ConstantName.invalidNum; // Drain Angle
        public double A1 = ConstantName.invalidNum; //HC angle
        public double D1 = ConstantName.invalidNum; // Stud extends length
        public Point3dModel V = null; // Vent Position
        public Point3dModel M = null;// Tag Position
        public FixtureUnitBlockStatic fuS = null;

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

                //Tag can never be null
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

        //It Will return position of drain
        //If it is double drain, it will return 
        public Point3dModel GetDrainPosition()
        {
            if (fuS.IsSingleDrainOnly() || fuS.IsDoubleDrainOnly())
            {
                return position;
            }
            else if (fuS.HasDoubleDrainButNotAtOrigin())
            {
                return matrixTransform.Transform(R1);
            }
            return null;
        }

        public Point3dModel GetDoubleDrainTip()
        {
            if (fuS.HasDoubleDrainButNotAtOrigin())
            {
                Point3dModel tipPoint = new Point3dModel(R1.X, R1.Y + Y2, R1.Z);
                return matrixTransform.Transform(A3, R1, tipPoint);
            }
            return null;
        }

        public Point3dModel GetVentPosition()
        {
            if (fuS.HasVent())
            {
                return matrixTransform.Transform(V);
            }
            else
            {
                return null;
            }
        }

        public Point3dModel GetTagPosition()
        {
            return matrixTransform.Transform(M);
        }

        public Point3dModel GetHotWaterTip()
        {
            if(fuS.HasSingleStud() && fuS.HasHotStud())
            {
                Point3dModel point = new Point3dModel(0, 1 + D1, 0);
                return matrixTransform.Transform(A1, point);
            }
            return null;
        }

        public Point3dModel GetColdWaterTip()
        {
            if (fuS.HasSingleStud() && fuS.HasColdStud())
            {
                Point3dModel point = new Point3dModel(0, 1 + D1, 0);
                return matrixTransform.Transform(A1, point);
            }
            return null;
        }

        /// <summary>
        /// Return a list of points, the first one is hotWaterTipPoint, the second one is coldWaterTipPoint
        /// </summary>
        /// <returns></returns>
        public List<Point3dModel> GetHotColdTip()
        {
            if (fuS.HasHotColdStud())
            {
                Point3dModel hotPoint = new Point3dModel(-5, 1 + D1, 0);
                Point3dModel coldPoint = new Point3dModel(5, 1 + D1, 0);

                return new List<Point3dModel> {
                    matrixTransform.Transform(A1, hotPoint),
                    matrixTransform.Transform(A2, coldPoint)
                };
            }
            return null;
        }
    }

    class FixtureUnitModelName {
        public const string R1 = "R1";
        public const string A2 = "A2"; //Vent Angle
        public const string Y2 = "Y2"; //For Double
        public const string X2 = "X2";
        public const string X2_2 = "X2_2";
        public const string A3 = "A3"; // Drain Angle
        public const string A1 = "A1"; //HC angle
        public const string D1 = "D1";
        public const string V = "V"; // Vent Position
        public const string M = "M";// Tag Position

        public const string INDEX = "INDEX";
        public const string TAG = "TAG";
        public const string NUMBER = "NUMBER";
        public const string CW_DIA = "COLD_DIAMETER";
        public const string HW_DIA = "HOT_DIAMETER";
        public const string WASTE_DIA = "WASTE_DIAMETER";
        public const string VENT_DIA = "VENT_DIAMETER";
        public const string STORM_DIA = "STORM_DIAMETER";
        public const string WSFU = "WSFU";
        public const string CWSFU = "CWSFU";
        public const string HWSFU = "HWSFU";
        public const string DFU = "DFU";
    }
}
