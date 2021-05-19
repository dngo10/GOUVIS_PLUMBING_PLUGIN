using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using ClassLibrary1.HELPERS;
using GouvisPlumbingNew.DATABASE.DBModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace GouvisPlumbingNew.HELPERS
{
    class Goodies
    {
        /***
         *This Must Run Inside a Transaction. 
         *Or it will create errors.
         *This is to get Program
         */
        public static string GetDynamicName(BlockReference bref)
        {
            try
            {
                BlockTableRecord blockTableRecord = (BlockTableRecord)bref.DynamicBlockTableRecord.GetObject(OpenMode.ForRead);
                return blockTableRecord.Name;
            }
            catch (Exception e)
            {
                Console.WriteLine("The HELPERS/GetDyamicName function error, check it " + e.Message);
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

        public static void InsertTable(Table table, Transaction tr, BlockTableRecord btr)
        {
            btr.AppendEntity(table);
            tr.AddNewlyCreatedDBObject(table, true);
        }

        /// <summary>
        /// COPY A BLOCK (WITH BLOCK NAME) from ONE FILE TO ANOTHER.
        /// </summary>
        /// <param name="blockDrawingFile"></param>
        /// <param name="blockDestinationFile"></param>
        /// <param name="blockName"></param>
        public static void AddBlockToDrawing(string blockDrawingFile, string blockDestinationFile, string blockName)
        {
            if (!GoodiesPath.IsFileLocked(blockDestinationFile) && !GoodiesPath.IsFileReadOnly(blockDestinationFile))
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
            using (Transaction tr = db.TransactionManager.StartTransaction())
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
        public static void InsertDynamicBlockToTableCell(Cell tCell, Database db, string BlockName, FixtureDetails df)
        {
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {

                //tCell.DeleteContent();
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForWrite);
                if (bt.Has(BlockName))
                {
                    tCell.BlockTableRecordId = bt[BlockName];
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockName], OpenMode.ForRead);
                    if (btr.HasAttributeDefinitions)
                    {
                        foreach (ObjectId id in btr)
                        {
                            DBObject dbObj = tr.GetObject(id, OpenMode.ForRead);
                            if (dbObj is AttributeDefinition)
                            {
                                AttributeDefinition attDef = (AttributeDefinition)dbObj;
                                if (attDef.Tag == HexNoteName.ID)
                                {
                                    tCell.SetBlockAttributeValue(id, df.model.TAG);
                                }
                                else if (attDef.Tag == HexNoteName.NUM)
                                {
                                    tCell.SetBlockAttributeValue(id, df.model.NUMBER);
                                }
                            }
                        }
                    }
                }
                tr.Commit();
            }
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
                    if (lineTypeId != null)
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

        //THIS IS FROM AUTOCAD DEVELOPER -- Check if file is locked/readonly.
        //https://spiderinnet1.typepad.com/blog/2015/02/autocad-net-reliably-check-file-lockreadonly.html

        //Layer Manager ONLY works with "current drawing". So you have OPEN the DRAWING YOU
        //WANT TO EDIT in order to make it work.

        //in process;
        public static void CreateLayoutInCurrentDrawing(Database db)
        {
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                ObjectId layOutDict = db.LayoutDictionaryId;
                DBDictionary dBOjbect = (DBDictionary)tr.GetObject(layOutDict, OpenMode.ForRead);
                foreach (DBDictionaryEntry kv in dBOjbect)
                {
                    string LayoutName = kv.Key;
                    ObjectId id = kv.Value;
                    //LayoutEdit obj = (LayoutEdit)tr.GetObject(id, OpenMode.ForRead);
                    LayoutManager lm = LayoutManager.Current;
                }
            }
        }

        public static List<string> GetListOfDocumentOpening()
        {
            DocumentCollection docCol = Application.DocumentManager;
            List<string> docPathList = new List<string>();

            foreach (Document doc in docCol)
            {
                docPathList.Add(doc.Name);
            }

            return docPathList;
        }

        /// <summary>
        /// Whether or you can write to dwg WHILE RUNNING AutoCAD,
        /// This is NOT intended for dotnet core console.
        /// </summary>
        /// <param name="path">Full DWG PATH</param>
        /// <returns></returns>
        public static FileStatus CanOpenToWrite(string path)
        {
            FileStatus fileStatus = new FileStatus(3, path);
            if (!File.Exists(path))
            {
                Console.WriteLine("CanOpenToWrite: File does not exists.");
                return new FileStatus(3, path);
            }

            if (path == Application.DocumentManager.MdiActiveDocument.Name)
            {
                return new FileStatus(0, path);
            }

            if (GetListOfDocumentOpening().Contains(path))
            {
                foreach (Document doc in Application.DocumentManager)
                {
                    if (doc.Name == path)
                    {
                        //Application.DocumentManager.MdiActiveDocument = doc;
                        return new FileStatus(1, path);
                    }
                }
            }

            if (!GoodiesPath.IsFileLocked(path))
            {
                //Document doc = Application.DocumentManager.Open(path, false, "");
                //Application.DocumentManager.MdiActiveDocument = doc;
                return new FileStatus(2, path);
            }

            Console.WriteLine("CanOpenToWrite: Can't open file, Problem unknown");
            return new FileStatus(3, path);
        }


        public static Document CanOpenToWrite1(string path)
        {
            FileStatus fileStatus = new FileStatus(3, path);
            if (!File.Exists(path))
            {
                Console.WriteLine("CanOpenToWrite: File does not exists.");
                return null;
            }

            if (path == Application.DocumentManager.MdiActiveDocument.Name)
            {
                return Application.DocumentManager.MdiActiveDocument;
            }

            if (GetListOfDocumentOpening().Contains(path))
            {
                foreach (Document doc in Application.DocumentManager)
                {
                    if (doc.Name == path)
                    {
                        Application.DocumentManager.MdiActiveDocument = doc;
                        Application.DocumentManager.DocumentActivationEnabled = true;
                        return Application.DocumentManager.MdiActiveDocument;
                    }
                }
            }

            if (!GoodiesPath.IsFileLocked(path))
            {
                Document doc = Application.DocumentManager.Open(path, false, "");
                Application.DocumentManager.MdiActiveDocument = doc;
                Application.DocumentManager.DocumentActivationEnabled = true;
                return Application.DocumentManager.MdiActiveDocument;
            }

            Console.WriteLine("CanOpenToWrite: Can't open file, Problem unknown");
            return null;
        }
        public static string ConvertHandleToString(Handle handle)
        {
            return handle.ToString();
        }

        public static IList<double> ConvertPoint3dToArray(Point3d point)
        {
            return new List<double>(point.ToArray());
        }

        public static IList<double> ConvertMatrix3dToArray(Matrix3d matrix)
        {
            return new List<double>(matrix.ToArray());
        }

        public static Handle ConvertStringToHandle(string handleStr)
        {
            long val = Convert.ToInt64(handleStr.ToString(), 16);
            Handle handle = new Handle(val);
            return handle;
        }

        public static Point3d ConvertArrayToPoint3d(IList<double> array)
        {
            return new Point3d(array[0], array[1], array[2]);
        }

        public static Matrix3d ConvertArrayToMatrix3d(IList<double> array)
        {
            return new Matrix3d(array.ToArray());
        }

        public static Document GetDocumentFromDwgpath(string dwgPath)
        {
            if (!File.Exists(dwgPath))
            {
                return null;
            }

            if (Application.DocumentManager.MdiActiveDocument.Name == dwgPath)
            {
                return Application.DocumentManager.MdiActiveDocument;
            }

            foreach (Document doc in Application.DocumentManager)
            {
                if (doc.Name == dwgPath)
                {
                    return doc;
                }
            }
            return null;
        }
    }
}
