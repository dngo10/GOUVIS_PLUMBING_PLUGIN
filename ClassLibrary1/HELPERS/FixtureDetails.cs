using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.HELPERS
{
    class FixtureDetails : ObjectData
    {
        public double index = ConstantName.invalid;
        public string FixtureName = "";
        public string tag = "";
        public double number = ConstantName.invalid;
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

        public FixtureDetails(BlockReference bref, Transaction tr)
        {
            FillOutVariable(bref, tr);
        }

        private void FillOutVariable(BlockReference bref, Transaction tr)
        {
            position = bref.Position;
            handle = bref.Handle;
            foreach (ObjectId id in bref.AttributeCollection)
            {
                AttributeReference aRef = (AttributeReference)tr.GetObject(id, OpenMode.ForRead);
                string textString = aRef.TextString;
                double number;
                bool isNumber = double.TryParse(textString, out number);

                if (isNumber)
                {
                    if (aRef.Tag == FixtureDetailsName.index)
                    {
                        index = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.number)
                    {
                        this.number = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.CW_DIA)
                    {
                        CW_DIA = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.HW_DIA)
                    {
                        HW_DIA = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.WASTE_DIA)
                    {
                        WASTE_DIA = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.VENT_DIA)
                    {
                        VENT_DIA = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.STORM_DIA)
                    {
                        STORM_DIA = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.WSFU)
                    {
                        WSFU = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.CWSFU)
                    {
                        CWSFU = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.HWSFU)
                    {
                        HWSFU = number;
                    }
                    else if (aRef.Tag == FixtureDetailsName.DFU)
                    {
                        DFU = number;
                    }
                }
                else if (aRef.Tag == FixtureDetailsName.tag)
                {
                    tag = textString;
                }
                else if (aRef.Tag == FixtureDetailsName.FixtureName)
                {
                    FixtureName = textString;
                }
                else if (aRef.Tag == FixtureDetailsName.DESCRIPTION)
                {
                    DESCRIPTION = textString;
                }
            }
        }

        public void UpdateFixtureDetail(Database db)
        {
            BlockReference bref = (BlockReference)Goodies.GetDBObjFromHandle(handle, db);
            if(bref != null && !bref.IsErased)
            {
                using(Transaction tr = db.TransactionManager.StartTransaction())
                {
                    FillOutVariable(bref, tr);
                }
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
