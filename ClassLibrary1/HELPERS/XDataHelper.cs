using Autodesk.AutoCAD.DatabaseServices;
using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.HELPERS
{
    class XDataHelper
    {
        public static string GetTableType(BlockReference bref, Transaction tr, Database db)
        {
            ResultBuffer rb = bref.XData;
            if(rb != null)
            {
                TypedValue[] tvs = rb.AsArray();
                if(tvs[0].TypeCode == (short)DxfCode.ExtendedDataRegAppName && (string)tvs[0].Value == ConstantName.tableAlias)
                {
                    return (string)tvs[1].Value;
                }
            }

            return "";
        }
        public static void AddTableXData(ref Table bref, Transaction tr, Database db)
        {
            AddRegAppTableRecord(db, tr, ConstantName.tableAlias);
            ResultBuffer rb = new ResultBuffer(
                    new TypedValue((int)DxfCode.ExtendedDataRegAppName, ConstantName.tableAlias),
                    new TypedValue((int)DxfCode.ExtendedDataAsciiString, XDataHelperName.tableSchedule)
                );

            bref.XData = rb;
        }

        public static void AddRegAppTableRecord(Database db, Transaction tr, string name)
        {
            RegAppTable rat = (RegAppTable)tr.GetObject(db.RegAppTableId, OpenMode.ForRead);
            if (!rat.Has(ConstantName.tableAlias))
            {
                rat.UpgradeOpen();
                RegAppTableRecord ratr = new RegAppTableRecord();
                ratr.Name = ConstantName.tableAlias;
                rat.Add(ratr);
                tr.AddNewlyCreatedDBObject(ratr, true);
            }
        }
    }

    class XDataHelperName
    {
        //ALIAS;
        public const string tableSchedule = "TS";
        public const string fixtureSchedule = "FS";
    }
}
