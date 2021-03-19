using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

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

        /// <summary>
        /// COPY A BLOCK (WITH BLOCK NAME) from ONE FILE TO ANOTHER.
        /// </summary>
        /// <param name="blockDrawingFile"></param>
        /// <param name="blockDestinationFile"></param>
        /// <param name="blockName"></param>
        public static void AddBlockToDrawing(string blockDrawingFile, string blockDestinationFile, string blockName)
        {
            if(!IsFileLocked(blockDestinationFile) && !IsFileReadOnly(blockDestinationFile))
            {
                using (Database db = new Database())
                {
                    db.ReadDwgFile(blockDrawingFile, System.IO.FileShare.Read, true, "");
                    ObjectIdCollection ids = new ObjectIdCollection();
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                        //BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);
                        if (bt.Has(blockName))
                        {
                            ObjectId id = bt[blockName];
                            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(id, OpenMode.ForRead);
                            ObjectIdCollection IdsCol = new ObjectIdCollection();
                            using (Database dbDest = new Database())
                            {
                                dbDest.ReadDwgFile(blockDestinationFile, System.IO.FileShare.Write, true, "");
                                IdsCol.Add(id);
                                IdMapping iMap = new IdMapping();
                                dbDest.WblockCloneObjects(IdsCol, dbDest.BlockTableId, iMap, DuplicateRecordCloning.Ignore, false);
                                dbDest.Save();
                                dbDest.Dispose();
                            }
                        }
                        tr.Commit();
                    }
                }
            }
        }

        public static void AddBlockToActiveDrawing(string blockDrawingFile, string blockName)
        {
            Database dbDest = Application.DocumentManager.MdiActiveDocument.Database;
            if (HasBlockDefinition(blockName, dbDest)) return;

            using (Database db = new Database())
            {
                db.ReadDwgFile(blockDrawingFile, System.IO.FileShare.Read, true, "");
                ObjectIdCollection ids = new ObjectIdCollection();
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                    //BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);
                    if (bt.Has(blockName))
                    {
                        ObjectId id = bt[blockName];
                        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(id, OpenMode.ForRead);
                        ObjectIdCollection IdsCol = new ObjectIdCollection();
                        IdsCol.Add(id);
                        IdMapping iMap = new IdMapping();
                        dbDest.WblockCloneObjects(IdsCol, dbDest.BlockTableId, iMap, DuplicateRecordCloning.Ignore, false);
                    }
                    tr.Commit();
                }
            }
        }

        private static bool HasBlockDefinition(string BlockName, Database db)
        {
            bool result = false;
            using(Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                result = bt.Has(BlockName);
            }
            return result;
        }

        //INSERT DYNAMIC BLOCK INTO DATABASE
        public static Dictionary<ObjectId, ObjectId> InsertDynamicBlock(string blockName, Database db)
        {
            Dictionary<ObjectId, ObjectId> atts = new System.Collections.Generic.Dictionary<ObjectId, ObjectId>();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForWrite);
                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                if (bt.Has(blockName))
                {
                    BlockReference bref = new BlockReference(Point3d.Origin, bt[blockName]);
                    btr.AppendEntity(bref);
                    tr.AddNewlyCreatedDBObject(bref, true);

                    BlockTableRecord brefRec = (BlockTableRecord)tr.GetObject(bref.BlockTableRecord, OpenMode.ForRead);
                    if (brefRec.HasAttributeDefinitions)
                    {
                        foreach (ObjectId id in btr)
                        {
                            DBObject dbObj = tr.GetObject(id, OpenMode.ForWrite);
                            if (dbObj is AttributeDefinition)
                            {
                                AttributeDefinition newAttriDef = (AttributeDefinition)dbObj;
                                AttributeReference attRef = new AttributeReference();
                                attRef.SetAttributeFromBlock(newAttriDef, bref.BlockTransform);
                                attRef.AdjustAlignment(bref.Database);
                                bref.AttributeCollection.AppendAttribute(attRef);
                                tr.AddNewlyCreatedDBObject(attRef, true);
                                atts.Add(attRef.ObjectId, id);
                            }
                        }
                    }
                }
                tr.Commit();
            }
            return atts;
        }


        //INSERT BLOCK INTO TABLE CELL;
        public static Dictionary<ObjectId,ObjectId> InsertDynamicBlockToTableCell(Cell tCell, Database db, string BlockName)
        {
            Dictionary<ObjectId, ObjectId> atts = new System.Collections.Generic.Dictionary<ObjectId, ObjectId>();
            using(Transaction tr = db.TransactionManager.StartTransaction())
            {
                tCell.DeleteContent();
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                if (bt.Has(BlockName))
                {
                    tCell.BlockTableRecordId = bt[BlockName];
                    BlockReference bref = (BlockReference)tCell.Contents[0].Value;
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockName], OpenMode.ForRead);
                    if (btr.HasAttributeDefinitions)
                    {
                        foreach (ObjectId id in btr)
                        {
                            DBObject dbObj = tr.GetObject(id, OpenMode.ForRead);
                            if (dbObj is AttributeDefinition)
                            {
                                AttributeDefinition attDef = (AttributeDefinition)dbObj;
                                AttributeReference attRef = new AttributeReference();
                                attRef.SetAttributeFromBlock(attDef, bref.BlockTransform);
                                attRef.AdjustAlignment(bref.Database);
                                bref.AttributeCollection.AppendAttribute(attRef);
                                tr.AddNewlyCreatedDBObject(attRef, true);
                                atts.Add(attRef.ObjectId, id);
                            }
                        }
                    }
                }
                tr.Commit();
            }
            return atts;
        }

        public static DBObject GetDBObjFromHandle(Handle handle, Database db)
        {
            ObjectId id;
            BlockReference bref = null;
            if (db.TryGetObjectId(handle, out id))
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    bref = (BlockReference)tr.GetObject(id, OpenMode.ForRead);
                }
            }
            return bref;
        }

        //ADD LAYER NAME;
        public static void AddLayer(string LayerName, Database db, ObjectId lineTypeId, short colorIndex = 255)
        {
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead);
                if (!lt.Has(LayerName))
                {
                    LayerTableRecord ltr = new LayerTableRecord();
                    ltr.Color = Color.FromColorIndex(ColorMethod.ByAci, colorIndex);
                    if(lineTypeId != null)
                    {
                        ltr.LinetypeObjectId = lineTypeId;
                    }
                    lt.UpgradeOpen();
                    lt.Add(ltr);
                    tr.AddNewlyCreatedDBObject(ltr, true);
                }
                tr.Commit();
            }
        }

        public static void AddLineType(string LineTypeName, Database db, string linePattern)
        {

        }

        //THIS IS FROM AUTOCAD DEVELOPER -- Check if file is locked/readonly.
        //https://spiderinnet1.typepad.com/blog/2015/02/autocad-net-reliably-check-file-lockreadonly.html

        public static bool IsFileLocked(string path)
        {
            try
            {
                using (File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    return false;
                }
            }
            catch (IOException e)
            {
                int errorNum = Marshal.GetHRForException(e) & ((1 << 16) - 1);
                return errorNum == 32 || errorNum == 33;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "IsFileLocked Checking");
                return true;
            }
        }

        public static bool IsFileReadOnly(string path)
        {
            return new FileInfo(path).IsReadOnly;
        }

        //Layer Manager ONLY works with "current drawing". So you have OPEN the DRAWING YOU
        //WANT TO EDIT in order to make it work.

        //in process;
        public static void CreateLayoutInCurrentDrawing(Database db)
        {
            using(Transaction tr = db.TransactionManager.StartTransaction())
            {
                ObjectId layOutDict = db.LayoutDictionaryId;
                DBDictionary dBOjbect = (DBDictionary)tr.GetObject(layOutDict, OpenMode.ForRead);
                foreach(DBDictionaryEntry kv in dBOjbect)
                {
                    string LayoutName = kv.Key;
                    ObjectId id = kv.Value;
                    LayoutEdit obj = (LayoutEdit)tr.GetObject(id, OpenMode.ForRead);
                    LayoutManager lm = LayoutManager.Current;
                }
            }
        }

    }
}
