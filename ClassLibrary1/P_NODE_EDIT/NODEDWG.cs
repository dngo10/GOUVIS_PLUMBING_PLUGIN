using Autodesk.AutoCAD.DatabaseServices;
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
        public HashSet<BlockReference> fixtureAreaSET;
        public HashSet<BlockReference> fixtureDetailSET;
        public HashSet<BlockReference> insertionPointSET;
        public HashSet<Table> tableSET;

        public NODEDWG()
        {
            fixtureAreaSET = new HashSet<BlockReference>();
            fixtureDetailSET = new HashSet<BlockReference>();
            insertionPointSET = new HashSet<BlockReference>();
            tableSET = new HashSet<Table>();
        }
    }

    class NODEDWGDATA
    {
        public NODEDWG node;
        public SortedSet<FixtureBeingUsedArea> FixtureBoxSet = new SortedSet<FixtureBeingUsedArea>();
        public SortedSet<FixtureDetails> FixtureDetailSet = new SortedSet<FixtureDetails>(Comparer<FixtureDetails>.Create((a,b) => a.index.CompareTo(b.index)));
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
            foreach(BlockReference bref in node.fixtureAreaSET)
            {
                FixtureBeingUsedArea FBUA = new FixtureBeingUsedArea(bref);
                FixtureBoxSet.Add(FBUA);
            }

            foreach(BlockReference bref in node.fixtureDetailSET)
            {
                FixtureDetails FD = new FixtureDetails(bref, tr);
                FixtureDetailSet.Add(FD);
            }
            
            // THIS ONE IS GOING TO BE A VERY BIG BLOCK.
            foreach(BlockReference bref in node.insertionPointSET)
            {
                InsertPoint IP = new InsertPoint(bref, tr);
                foreach(Table t in node.tableSET)
                {
                    IP.addTable(t.Position, t);
                }
                InsertPointSet.Add(IP);
            }
        }
    }
}
