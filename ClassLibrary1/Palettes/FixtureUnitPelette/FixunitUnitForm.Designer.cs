
namespace ClassLibrary1.Palettes.FixtureUnitPelette
{
    partial class FixtureUnitForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.InsertButton = new System.Windows.Forms.Button();
            this.HWCheckBox = new System.Windows.Forms.CheckBox();
            this.CWCheckBox = new System.Windows.Forms.CheckBox();
            this.VentCheckBox = new System.Windows.Forms.CheckBox();
            this.FixtureComboBox = new System.Windows.Forms.ComboBox();
            this.DrainCheckBox = new System.Windows.Forms.CheckBox();
            this.DrainTypeComboBox = new System.Windows.Forms.ComboBox();
            this.VentTypeComboBox = new System.Windows.Forms.ComboBox();
            this.HWDiaTextBox = new System.Windows.Forms.TextBox();
            this.HWFUTextBox = new System.Windows.Forms.TextBox();
            this.ColdWaterDiaTextBox = new System.Windows.Forms.TextBox();
            this.CWFUTextBox = new System.Windows.Forms.TextBox();
            this.ventDiaTextBox = new System.Windows.Forms.TextBox();
            this.DrainFUTextBox = new System.Windows.Forms.TextBox();
            this.drainDiaTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // InsertButton
            // 
            this.InsertButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InsertButton.Location = new System.Drawing.Point(84, 173);
            this.InsertButton.Name = "InsertButton";
            this.InsertButton.Size = new System.Drawing.Size(90, 23);
            this.InsertButton.TabIndex = 0;
            this.InsertButton.Text = "Insert";
            this.InsertButton.UseVisualStyleBackColor = true;
            // 
            // HWCheckBox
            // 
            this.HWCheckBox.AutoSize = true;
            this.HWCheckBox.Location = new System.Drawing.Point(12, 62);
            this.HWCheckBox.Name = "HWCheckBox";
            this.HWCheckBox.Size = new System.Drawing.Size(75, 17);
            this.HWCheckBox.TabIndex = 1;
            this.HWCheckBox.Text = "Hot Water";
            this.HWCheckBox.UseVisualStyleBackColor = true;
            // 
            // CWCheckBox
            // 
            this.CWCheckBox.AutoSize = true;
            this.CWCheckBox.Location = new System.Drawing.Point(12, 85);
            this.CWCheckBox.Name = "CWCheckBox";
            this.CWCheckBox.Size = new System.Drawing.Size(79, 17);
            this.CWCheckBox.TabIndex = 2;
            this.CWCheckBox.Text = "Cold Water";
            this.CWCheckBox.UseVisualStyleBackColor = true;
            // 
            // VentCheckBox
            // 
            this.VentCheckBox.AutoSize = true;
            this.VentCheckBox.Location = new System.Drawing.Point(12, 108);
            this.VentCheckBox.Name = "VentCheckBox";
            this.VentCheckBox.Size = new System.Drawing.Size(48, 17);
            this.VentCheckBox.TabIndex = 3;
            this.VentCheckBox.Text = "Vent";
            this.VentCheckBox.UseVisualStyleBackColor = true;
            this.VentCheckBox.CheckedChanged += new System.EventHandler(this.VentCheckBox_CheckedChanged);
            // 
            // FixtureComboBox
            // 
            this.FixtureComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FixtureComboBox.FormattingEnabled = true;
            this.FixtureComboBox.Location = new System.Drawing.Point(12, 12);
            this.FixtureComboBox.Name = "FixtureComboBox";
            this.FixtureComboBox.Size = new System.Drawing.Size(237, 21);
            this.FixtureComboBox.TabIndex = 4;
            this.FixtureComboBox.SelectedIndexChanged += new System.EventHandler(this.FixtureComboBox_SelectedIndexChanged);
            // 
            // DrainCheckBox
            // 
            this.DrainCheckBox.AutoSize = true;
            this.DrainCheckBox.Location = new System.Drawing.Point(12, 128);
            this.DrainCheckBox.Name = "DrainCheckBox";
            this.DrainCheckBox.Size = new System.Drawing.Size(51, 17);
            this.DrainCheckBox.TabIndex = 5;
            this.DrainCheckBox.Text = "Drain";
            this.DrainCheckBox.UseVisualStyleBackColor = true;
            this.DrainCheckBox.CheckedChanged += new System.EventHandler(this.DrainCheckBox_CheckedChanged);
            // 
            // DrainTypeComboBox
            // 
            this.DrainTypeComboBox.FormattingEnabled = true;
            this.DrainTypeComboBox.Location = new System.Drawing.Point(64, 128);
            this.DrainTypeComboBox.Name = "DrainTypeComboBox";
            this.DrainTypeComboBox.Size = new System.Drawing.Size(95, 21);
            this.DrainTypeComboBox.TabIndex = 6;
            // 
            // VentTypeComboBox
            // 
            this.VentTypeComboBox.FormattingEnabled = true;
            this.VentTypeComboBox.Location = new System.Drawing.Point(64, 105);
            this.VentTypeComboBox.Name = "VentTypeComboBox";
            this.VentTypeComboBox.Size = new System.Drawing.Size(66, 21);
            this.VentTypeComboBox.TabIndex = 7;
            // 
            // HWDiaTextBox
            // 
            this.HWDiaTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.HWDiaTextBox.Enabled = false;
            this.HWDiaTextBox.Location = new System.Drawing.Point(177, 62);
            this.HWDiaTextBox.Name = "HWDiaTextBox";
            this.HWDiaTextBox.Size = new System.Drawing.Size(33, 20);
            this.HWDiaTextBox.TabIndex = 8;
            // 
            // HWFUTextBox
            // 
            this.HWFUTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.HWFUTextBox.Enabled = false;
            this.HWFUTextBox.Location = new System.Drawing.Point(216, 62);
            this.HWFUTextBox.Name = "HWFUTextBox";
            this.HWFUTextBox.Size = new System.Drawing.Size(33, 20);
            this.HWFUTextBox.TabIndex = 9;
            // 
            // ColdWaterDiaTextBox
            // 
            this.ColdWaterDiaTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ColdWaterDiaTextBox.Enabled = false;
            this.ColdWaterDiaTextBox.Location = new System.Drawing.Point(177, 83);
            this.ColdWaterDiaTextBox.Name = "ColdWaterDiaTextBox";
            this.ColdWaterDiaTextBox.Size = new System.Drawing.Size(33, 20);
            this.ColdWaterDiaTextBox.TabIndex = 10;
            // 
            // CWFUTextBox
            // 
            this.CWFUTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CWFUTextBox.Enabled = false;
            this.CWFUTextBox.Location = new System.Drawing.Point(216, 83);
            this.CWFUTextBox.Name = "CWFUTextBox";
            this.CWFUTextBox.Size = new System.Drawing.Size(33, 20);
            this.CWFUTextBox.TabIndex = 11;
            // 
            // ventDiaTextBox
            // 
            this.ventDiaTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ventDiaTextBox.Enabled = false;
            this.ventDiaTextBox.Location = new System.Drawing.Point(177, 105);
            this.ventDiaTextBox.Name = "ventDiaTextBox";
            this.ventDiaTextBox.Size = new System.Drawing.Size(33, 20);
            this.ventDiaTextBox.TabIndex = 12;
            // 
            // DrainFUTextBox
            // 
            this.DrainFUTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DrainFUTextBox.Enabled = false;
            this.DrainFUTextBox.Location = new System.Drawing.Point(216, 128);
            this.DrainFUTextBox.Name = "DrainFUTextBox";
            this.DrainFUTextBox.Size = new System.Drawing.Size(33, 20);
            this.DrainFUTextBox.TabIndex = 13;
            // 
            // drainDiaTextBox
            // 
            this.drainDiaTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.drainDiaTextBox.Enabled = false;
            this.drainDiaTextBox.Location = new System.Drawing.Point(177, 128);
            this.drainDiaTextBox.Name = "drainDiaTextBox";
            this.drainDiaTextBox.Size = new System.Drawing.Size(33, 20);
            this.drainDiaTextBox.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(228, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "FU";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(194, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "⌀";
            // 
            // FixtureUnitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(250, 500);
            this.ClientSize = new System.Drawing.Size(258, 512);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.drainDiaTextBox);
            this.Controls.Add(this.DrainFUTextBox);
            this.Controls.Add(this.ventDiaTextBox);
            this.Controls.Add(this.CWFUTextBox);
            this.Controls.Add(this.ColdWaterDiaTextBox);
            this.Controls.Add(this.HWFUTextBox);
            this.Controls.Add(this.HWDiaTextBox);
            this.Controls.Add(this.VentTypeComboBox);
            this.Controls.Add(this.DrainTypeComboBox);
            this.Controls.Add(this.DrainCheckBox);
            this.Controls.Add(this.FixtureComboBox);
            this.Controls.Add(this.VentCheckBox);
            this.Controls.Add(this.CWCheckBox);
            this.Controls.Add(this.HWCheckBox);
            this.Controls.Add(this.InsertButton);
            this.Name = "FixtureUnitForm";
            this.Text = "FixtureUnitForm";
            this.Load += new System.EventHandler(this.FixtureUnitForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button InsertButton;
        private System.Windows.Forms.CheckBox HWCheckBox;
        private System.Windows.Forms.CheckBox CWCheckBox;
        private System.Windows.Forms.CheckBox VentCheckBox;
        private System.Windows.Forms.ComboBox FixtureComboBox;
        private System.Windows.Forms.CheckBox DrainCheckBox;
        private System.Windows.Forms.ComboBox DrainTypeComboBox;
        private System.Windows.Forms.ComboBox VentTypeComboBox;
        private System.Windows.Forms.TextBox HWDiaTextBox;
        private System.Windows.Forms.TextBox HWFUTextBox;
        private System.Windows.Forms.TextBox ColdWaterDiaTextBox;
        private System.Windows.Forms.TextBox CWFUTextBox;
        private System.Windows.Forms.TextBox ventDiaTextBox;
        private System.Windows.Forms.TextBox DrainFUTextBox;
        private System.Windows.Forms.TextBox drainDiaTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}