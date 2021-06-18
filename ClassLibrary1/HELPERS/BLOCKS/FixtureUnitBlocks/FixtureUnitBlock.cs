using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.HELPERS.BLOCKS.FixtureUnitBlocks
{
    class FixtureUnitBlock
    {
        public static Dictionary<string, FixtureUnitBlockStatic> FixtureUnitDict = new Dictionary<string, FixtureUnitBlockStatic>
        {
            {"FIX_35" , new FixtureUnitBlockStatic(0, 0, 0) },

            //Drain Only (Single Drain)
            {"FIX_36", new FixtureUnitBlockStatic(1, 0, 0) },
            {"FIX_1",  new FixtureUnitBlockStatic(2, 0, 0) },
            { "FIX_2", new FixtureUnitBlockStatic(5, 0, 0) },
            { "FIX_3", new FixtureUnitBlockStatic(6, 0, 0) },
            { "FIX_4", new FixtureUnitBlockStatic(0, 0, 1) },

            //Stub Only (Single)
            { "FIX_37", new FixtureUnitBlockStatic(0, 1, 0) },
            { "FIX_5",  new FixtureUnitBlockStatic(0, 2, 0) },

            //H AND C Only
            { "FIX_38",  new FixtureUnitBlockStatic(0, 3, 0)},

            //One stud, One Drain
            { "FIX_39", new FixtureUnitBlockStatic(2, 2, 0)},
            { "FIX_6",  new FixtureUnitBlockStatic(1, 2, 0)},
            { "FIX_7",  new FixtureUnitBlockStatic(1, 1, 0)},
            { "FIX_8",  new FixtureUnitBlockStatic(2, 1, 0)},

            //2 Stud, One Drain
            { "FIX_40", new FixtureUnitBlockStatic(2, 3, 0)},
            { "FIX_9",  new FixtureUnitBlockStatic(1, 3, 0)},

            //vent and one drain:
            { "FIX_41", new FixtureUnitBlockStatic(2, 0, 1)},
            { "FIX_10", new FixtureUnitBlockStatic(1, 0, 1)},

            //vent down and one drain:
            {"FIX_42", new FixtureUnitBlockStatic(2, 0, 2) },
            {"FIX_11", new FixtureUnitBlockStatic(1, 0, 2)},

            //vent down, one stud, one drain:
            {"FIX_43", new FixtureUnitBlockStatic(1, 2, 2)},
            {"FIX_12", new FixtureUnitBlockStatic(2, 2, 2)},
            {"FIX_13", new FixtureUnitBlockStatic(2, 1, 2)},
            {"FIX_14", new FixtureUnitBlockStatic(1, 1, 2)},

            //vent down, 2 stud, one drain:
            {"FIX_44", new FixtureUnitBlockStatic(2, 3, 2)},
            {"FIX_15", new FixtureUnitBlockStatic(2, 3, 2)},

            //vent up, 1 stud, 1 drain:
            {"FIX_45", new FixtureUnitBlockStatic(1, 1, 1)},
            {"FIX_16", new FixtureUnitBlockStatic(2, 1, 1)},
            {"FIX_17", new FixtureUnitBlockStatic(2, 2, 1)},
            {"FIX_18", new FixtureUnitBlockStatic(1, 2, 1)},

            //vent up, 2 stud, 1 drain:
            { "FIX_46", new FixtureUnitBlockStatic(1, 3, 1) },
            {"FIX_19",  new FixtureUnitBlockStatic(2, 3, 1)},

            //double drain only:
            {"FIX_47", new FixtureUnitBlockStatic(3, 0, 0)},
            {"FIX_20", new FixtureUnitBlockStatic(4, 0, 0)},

            //double drain, 1 stud:
            {"FIX_48", new FixtureUnitBlockStatic(3, 2, 0)},
            {"FIX_21", new FixtureUnitBlockStatic(4, 2, 0)},
            {"FIX_22", new FixtureUnitBlockStatic(4, 1, 0)},
            {"FIX_23", new FixtureUnitBlockStatic(3, 1, 0)},

            //double drain, 2 stud:
            {"FIX_49", new FixtureUnitBlockStatic(3, 3, 0)},
            {"FIX_26", new FixtureUnitBlockStatic(4, 3, 0)},

            //double drain, 1 vent up:
            {"FIX_50", new FixtureUnitBlockStatic(3, 0, 1)},
            {"FIX_24", new FixtureUnitBlockStatic(4, 0, 1)},

            //double drain, 1 vent down:
            {"FIX_51", new FixtureUnitBlockStatic(3, 0, 2)},
            {"FIX_25", new FixtureUnitBlockStatic(4, 0, 2)},

            //double drain, 1 stud, 1 vent down:
            {"FIX_52", new FixtureUnitBlockStatic(3, 2, 2)},
            {"FIX_27", new FixtureUnitBlockStatic(4, 2, 2)},
            {"FIX_28", new FixtureUnitBlockStatic(4, 1, 2)},
            {"FIX_29", new FixtureUnitBlockStatic(3, 1, 2)},

            //double drain, 2 stud, 1 vent down:
            {"FIX_53", new FixtureUnitBlockStatic(3, 3, 2)},
            {"FIX_30", new FixtureUnitBlockStatic(4, 3, 2)},

            //double drian 1 stud, 1 vent up:
            {"FIX_54", new FixtureUnitBlockStatic(3, 1, 1)},
            {"FIX_31", new FixtureUnitBlockStatic(4, 1, 1)},
            {"FIX_32", new FixtureUnitBlockStatic(4, 2, 1)},
            {"FIX_33", new FixtureUnitBlockStatic(3, 2, 1)},

            //double drain, 2 stud, vent up:
            {"FIX_55", new FixtureUnitBlockStatic(3, 3, 1)},
            {"FIX_34", new FixtureUnitBlockStatic(4, 3, 1)}
        };

        public static FixtureUnitBlockStatic GetFixtureUnitStatus(string name)
        {
            if (FixtureUnitDict.ContainsKey(name))
            {
                return new FixtureUnitBlockStatic(FixtureUnitDict[name]);
            }
            return null;
        }

    }

    class FixtureUnitBlockStatic
    {
        public int drain; //0: no drain, 1: single drain above, 2: single drain below, 3: double drain above, 4: double drain below
        public int waterSupply; // 0: no water supply, 1: hot water supply, 2: cold water supply, 3: hot and cold water supply
        public int vent; //0: no vent, 1: vent up, 2: vent down

        public FixtureUnitBlockStatic(int drain, int wasterSupply, int vent)
        {
            this.drain = drain;
            this.waterSupply = wasterSupply;
            this.vent = vent;
        }

        public FixtureUnitBlockStatic(FixtureUnitBlockStatic unit)
        {
            this.drain = unit.drain;
            this.waterSupply = unit.waterSupply;
            this.vent = unit.vent;
        }

        public bool HasDoubleDrainButNotAtOrigin()
        {
            return this.waterSupply != 0 && (this.drain == 3 || this.drain == 4);
        }



        public bool IsSingleDrainOnly()
        {
            return this.waterSupply == 0 && this.vent == 0 && (this.drain == 1 || this.drain == 2);
        }

        public bool IsDoubleDrainOnly()
        {
            return this.waterSupply == 0 && this.vent == 0 && (this.drain == 3 || this.drain == 4);
        }

        public bool HasVent()
        {
            return this.vent > 0;
        }

        public bool HasDrain()
        {
            return this.drain > 0;
        }

        public bool HasSingleStud()
        {
            return this.waterSupply == 1 || this.waterSupply == 2;
        }

        public bool HasHotStud()
        {
            return this.waterSupply == 1;
        }

        public bool HasColdStud()
        {
            return this.waterSupply == 2;
        }

        public bool HasHotColdStud()
        {
            return this.waterSupply == 3;
        }

        public bool IsVentOnly()
        {
            return this.vent == 0 && this.drain == 0;
        }
    }
}
