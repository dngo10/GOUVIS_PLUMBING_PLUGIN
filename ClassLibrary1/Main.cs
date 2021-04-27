using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GouvisPluminbNew.P_NODE_EDIT;
using GouvisPlumbingNew.HELPERS;
using GouvisPlumbingNew.P_NODE_EDIT;
using Autodesk.AutoCAD.ApplicationServices;
using GouvisPlumbingNew.DATABASE;
using System.Data.SQLite;
using System.IO;
using GouvisPlumbingNew.DATABASE.Controllers;
using Autodesk.AutoCAD.DatabaseServices;

namespace GouvisPlumbingNew
{
    public class Main
    {
        //TESTING

        [CommandMethod("abc")]
        public void RunTest()
        {

            P_NODE_EDIT.TestingFunction.testing1(ConstantName.TEMPPATH);

        }
    }
}
