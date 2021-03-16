using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.HELPERS
{
    class Goodies
    {
        /***
         *This Must Run Inside a Transaction. 
         *Or it will create errors.
         */
        public static string GetDynamicName(BlockReference bref, Editor ed)
        {
            try
            {
                BlockTableRecord blockTableRecord = (BlockTableRecord)bref.DynamicBlockTableRecord.GetObject(OpenMode.ForRead);
                return blockTableRecord.Name;
            }catch(Exception e)
            {
                ed.WriteMessage("The HELPERS/GetDyamicName function error, check it " + e.Message);
            }
            return null;
        }

        public static bool IsBlockInTheDrawing(BlockReference bref)
        {
            bool isVisible = bref.Visible;
            bool isCanceling = bref.IsCancelling;
            bool isErased = bref.IsErased;
            bool isDisposed = bref.IsDisposed;

            return isVisible && !isCanceling && !isErased && !isDisposed;
        }
    }
}
