using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GouvisPlumbingNew.HELPERS
{
    class JigBlock : EntityJig
    {
        Point3d p;
        Entity bref;
        Transaction tr; // The Jig Block Attribute if we have it.
        public Point3d insertPoint;
        private Dictionary<ObjectId, ObjectId> atts = new Dictionary<ObjectId, ObjectId>(); // This is also used to jig Attribute;
        public JigBlock(Entity bref, Transaction tr, Dictionary<ObjectId, ObjectId> atts = null) : base(bref)
        {
            this.bref = bref;
            this.tr = tr;
        }
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            var opts = new JigPromptPointOptions("\nInsertion Point: ");
            opts.UserInputControls = UserInputControls.NoZeroResponseAccepted;
            PromptPointResult ppResult = prompts.AcquirePoint(opts);
            if(ppResult.Status == PromptStatus.OK)
            {
                insertPoint = ppResult.Value;
                return SamplerStatus.OK;
            }
            else
            {
                return SamplerStatus.NoChange;
            }
        }

        protected override bool Update()
        {
            if(bref is BlockReference)
            {
                BlockReference brf = (BlockReference)bref;
                brf.Position = insertPoint;

                if(atts != null)
                {
                    foreach (ObjectId id in brf.AttributeCollection)
                    {
                        AttributeReference attRef = (AttributeReference)tr.GetObject(id, OpenMode.ForRead);
                        if (attRef != null)
                        {
                            attRef.UpgradeOpen();
                            AttributeDefinition adef = (AttributeDefinition)tr.GetObject(atts[attRef.ObjectId], OpenMode.ForRead);
                            attRef.SetAttributeFromBlock(adef, brf.BlockTransform);
                            attRef.AdjustAlignment(brf.Database);
                        }
                    }
                }
            }
            return true;
        }
    }
}
