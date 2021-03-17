using Autodesk.AutoCAD.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.P_NODE_EDIT
{
    class UpdateDataInNodeDrawing
    {
        public static void UpdateNodeDrawing(string nodeDrawingFile)
        {
            Document doc = Application.DocumentManager.Open(nodeDrawingFile, false);
            
        } 
    }
}
