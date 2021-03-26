using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GouvisPluminbNew.P_NODE_EDIT;

namespace ClassLibrary1
{
    public class Main
    {
        //TESTING

        [CommandMethod("abc")]
        public void RunTest()
        {

            //P_NODE_EDIT.TestingFunction.testing1(pNode_directory);

            ReadInformationInNodeDrawing.FindAllFixturesBeingUsed();
        }
    }
}
