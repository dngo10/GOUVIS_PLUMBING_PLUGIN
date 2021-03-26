using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using ClassLibrary1.HELPERS;

namespace ClassLibrary1.P_NODE_EDIT
{
    class TestingFunction
    {
        public static void testing1(string FileDWGPath)
        {
            Document doc = Application.DocumentManager.Open(FileDWGPath, false);

        }
        
    }
}
