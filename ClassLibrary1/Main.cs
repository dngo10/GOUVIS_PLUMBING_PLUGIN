using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GouvisPluminbNew.P_NODE_EDIT;
using ClassLibrary1.HELPERS;
using ClassLibrary1.P_NODE_EDIT;
using Autodesk.AutoCAD.ApplicationServices;
using ClassLibrary1.DATABASE;
using System.Data.SQLite;
using System.IO;
using ClassLibrary1.DATABASE.Controllers;
using Autodesk.AutoCAD.DatabaseServices;

namespace ClassLibrary1
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
