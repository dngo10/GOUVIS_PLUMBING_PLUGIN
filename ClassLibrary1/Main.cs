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
            string pNode_directory = @"C:\Plumbing_Template\TEMPLATE_FILE_V1.dwg";
            string boxName = "FixtureBeingUsedArea";
            string blockName = "FixtureDetails";

            P_NODE_EDIT.TestingFunction.testing1(pNode_directory);

            //ReadInformationInRectangle.FindAllFixturesBeingUsed(boxName, blockName, pNode_directory);
        }
    }
}
