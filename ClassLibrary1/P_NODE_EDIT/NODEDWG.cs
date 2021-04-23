using Autodesk.AutoCAD.DatabaseServices;
using ClassLibrary1.DATABASE.Controllers;
using ClassLibrary1.DATABASE.Models;
using ClassLibrary1.HELPERS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.P_NODE_EDIT
{
    class NODEDWG
    {
        public DwgFileModel model;
        public HashSet<FixtureBeingUsedArea> fixtureAreaSET;
        public HashSet<FixtureDetails> fixtureDetailSET;
        public HashSet<BlockReference> insertionPointSET;
        public HashSet<Table> tableSET;

        public NODEDWG()
        {
            fixtureAreaSET = new HashSet<FixtureBeingUsedArea>();
            fixtureDetailSET = new HashSet<FixtureDetails>();
            insertionPointSET = new HashSet<BlockReference>();
            tableSET = new HashSet<Table>();
        }
    }

    class NODEDWGDATA
    {
        public NODEDWG node;
        public SortedSet<FixtureBeingUsedArea> FixtureBoxSet = new SortedSet<FixtureBeingUsedArea>();
        public SortedSet<FixtureDetails> FixtureDetailSet = new SortedSet<FixtureDetails>(Comparer<FixtureDetails>.Create((a,b) => a.model.INDEX .CompareTo(b.model.INDEX)));
        public SortedSet<InsertPoint> InsertPointSet = new SortedSet<InsertPoint>();
    
        public NODEDWGDATA(string path)
        {
            if (File.Exists(path))
            {
                
            }
        }
    
        public NODEDWGDATA(NODEDWG node, Transaction tr)
        {
            this.node = node;
            foreach(FixtureBeingUsedArea bref in node.fixtureAreaSET)
            {
                FixtureBoxSet.Add(bref);
            }
    
            foreach(FixtureDetails bref in node.fixtureDetailSET)
            {
                FixtureDetailSet.Add(bref);
            }
            
            // THIS ONE IS GOING TO BE A VERY BIG BLOCK.
            //foreach(BlockReference bref in node.insertionPointSET)
            //{
            //    InsertPoint IP = new InsertPoint(bref, tr);
            //    foreach(Table t in node.tableSET)
            //    {
            //        IP.addTable(t.Position, t);
            //    }
            //    InsertPointSet.Add(IP);
            //}
        }
    }
}
