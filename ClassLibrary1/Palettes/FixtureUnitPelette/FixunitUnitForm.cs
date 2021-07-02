using GouvisPlumbingNew.PNOTE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary1.ProjectManagement;
using GouvisPlumbingNew.HELPERS;
using ClassLibrary1.HELPERS.BLOCKS.FixtureUnitBlocks;

namespace ClassLibrary1.Palettes.FixtureUnitPelette
{
    public partial class FixtureUnitForm : Form
    {
        NODEDWG node;

        List<string> name = new List<string>();
        List<FixtureDetails> details = new List<FixtureDetails>();
        public FixtureUnitForm(NODEDWG node)
        {
            InitializeComponent();
            this.node = node;
            VentTypeComboBox.Items.AddRange(FixtureUnitBlockStatic.ventType.ToArray());
            DrainTypeComboBox.Items.AddRange(FixtureUnitBlockStatic.drainType.ToArray());
        }

        private void FixtureUnitForm_Load(object sender, EventArgs e)
        {
            
            if(node == null)
            {
                this.Close();
            }
            else
            {
                PopulateForm();
            }
        }

        private void PopulateForm()
        {
            if (node == null) return;

            foreach (FixtureDetails fd in node.FixtureDetailSet){
                name.Add($"[{fd.model.ID}] {fd.model.TAG}  {fd.model.NUMBER}");
                details.Add(fd);
            }

            ClearAll();

            FixtureComboBox.Items.AddRange(name.ToArray());
            FixtureComboBox.SelectedIndex = 0;

            VentTypeComboBox.Items.AddRange(new string[]{ "Up", "Down" });
            VentTypeComboBox.SelectedIndex = 0;

            DrainTypeComboBox.Items.AddRange(new string[] { "Single", "Double" });
            DrainTypeComboBox.SelectedIndex = 0;

        }

        private void ClearAll()
        {
            name.Clear();
            details.Clear();

            FixtureComboBox.Items.Clear();
            //VentTypeComboBox.Items.Clear(); //no need to clear, you only need it once.
            //DrainTypeComboBox.Items.Clear(); //no need to clear, it will not change
            HWCheckBox.Checked = false;
            CWCheckBox.Checked = false;
            VentCheckBox.Checked = false;
            DrainCheckBox.Checked = false;
        }

        private void VentCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!VentCheckBox.Checked) VentTypeComboBox.Enabled = false;
            else VentTypeComboBox.Enabled = true;
        }

        private void DrainCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!DrainCheckBox.Checked) DrainTypeComboBox.Enabled = false;
            else DrainTypeComboBox.Enabled = true;
        }

        private void FixtureComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FixtureDetails df = details[FixtureComboBox.SelectedIndex];

            if(df.model.HWSFU != ConstantName.invalidNum)
            {
                HWCheckBox.Checked = true;
                HWFUTextBox.Text = df.model.HWSFU.ToString();
                HWDiaTextBox.Text = df.model.HW_DIA.ToString();
            }
            else
            {
                HWCheckBox.Checked = false;
                HWFUTextBox.Text = "";
                HWDiaTextBox.Text = "";
            }

            if(df.model.CWSFU != ConstantName.invalidNum)
            {
                CWCheckBox.Checked = true;
                CWFUTextBox.Text = df.model.CWSFU.ToString();
                ColdWaterDiaTextBox.Text = df.model.CW_DIA.ToString();
            }
            else
            {
                CWCheckBox.Checked = false;
                CWFUTextBox.Text = "";
                ColdWaterDiaTextBox.Text = "";
            }

            if(df.model.VENT_DIA != ConstantName.invalidNum)
            {
                VentCheckBox.Checked = true;
                ventDiaTextBox.Text = df.model.VENT_DIA.ToString();
            }
            else
            {
                VentCheckBox.Checked = false;
                ventDiaTextBox.Text = "";
            }

            if(df.model.DFU != ConstantName.invalidNum)
            {
                DrainCheckBox.Checked = true;
                DrainFUTextBox.Text = df.model.DFU.ToString();
                drainDiaTextBox.Text = df.model.WASTE_DIA.ToString();
            }
            else
            {
                DrainCheckBox.Checked = false;
                DrainFUTextBox.Text = "";
                drainDiaTextBox.Text = "";
            }
        }

        private void InsertButton_Click(object sender, EventArgs e)
        {
            FixtureDetails detail = details[FixtureComboBox.SelectedIndex];
            int DrainType = 0;
            int WaterSupplyType = 0;
            int VentType = 0;

            if (!DrainCheckBox.Checked)
            {
                DrainType = 0;
            }
            else
            {
                DrainType = DrainTypeComboBox.SelectedIndex + 1;
            }

            if (!VentCheckBox.Checked)
            {
                VentType = 0;
            }
            else
            {
                VentType = Vent
            }

            
        }
    }

    public class FUInfo
    {
        public FixtureDetails detail;
        public bool WaterSupplyType;
        public bool VentType;
        public bool DrainType;
    }
}
