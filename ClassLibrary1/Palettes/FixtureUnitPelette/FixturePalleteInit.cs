using Autodesk.AutoCAD.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1.ProjectManagement;
using Autodesk.AutoCAD.ApplicationServices;
using GouvisPlumbingNew.PNOTE;

namespace ClassLibrary1.Palettes.FixtureUnitPelette
{
    class FixturePalleteInit
    {
        private static PaletteSet _ps = null;

        public static void InitFixtureUnitPalette()
        {
            string currentDwg = Application.DocumentManager.MdiActiveDocument.Name;
            _ps = new PaletteSet("Fixture Unit");
            NODEDWG node =  SetUpProject.SetUp(currentDwg);
            FixtureUnitForm form = new FixtureUnitForm(node);

            _ps.Add("Fixture Unit", form);
            _ps.DockEnabled = (DockSides)(DockSides.Left | DockSides.Right);

            _ps.Visible = true;


        }
    }
}
