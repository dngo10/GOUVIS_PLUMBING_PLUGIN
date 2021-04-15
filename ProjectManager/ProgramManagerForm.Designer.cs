namespace ProjectManager
{
    partial class ProgramManagerForm
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
            this.FinishButton = new System.Windows.Forms.Button();
            this.Path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.File = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectManagerTab = new System.Windows.Forms.TabControl();
            this.AddButton = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label3 = new System.Windows.Forms.Label();
            this.SetUpFolderTreeView = new System.Windows.Forms.TreeView();
            this.label2 = new System.Windows.Forms.Label();
            this.setupGridView = new System.Windows.Forms.DataGridView();
            this.DwgFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DwgPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pNodeSearchButton = new System.Windows.Forms.Button();
            this.P_NODE_PATH_BOX = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ProjectManagerTab.SuspendLayout();
            this.AddButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.setupGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // FinishButton
            // 
            this.FinishButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.FinishButton.Location = new System.Drawing.Point(372, 528);
            this.FinishButton.Name = "FinishButton";
            this.FinishButton.Size = new System.Drawing.Size(176, 23);
            this.FinishButton.TabIndex = 3;
            this.FinishButton.Text = "Update Project Database";
            this.FinishButton.UseVisualStyleBackColor = true;
            this.FinishButton.Click += new System.EventHandler(this.FinishButton_Click);
            // 
            // Path
            // 
            this.Path.FillWeight = 255.1634F;
            this.Path.HeaderText = "Path";
            this.Path.Name = "Path";
            // 
            // File
            // 
            this.File.FillWeight = 86.79026F;
            this.File.HeaderText = "DWG File";
            this.File.Name = "File";
            // 
            // ProjectManagerTab
            // 
            this.ProjectManagerTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectManagerTab.Controls.Add(this.AddButton);
            this.ProjectManagerTab.Font = new System.Drawing.Font("Arial", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProjectManagerTab.HotTrack = true;
            this.ProjectManagerTab.ItemSize = new System.Drawing.Size(53, 24);
            this.ProjectManagerTab.Location = new System.Drawing.Point(12, 12);
            this.ProjectManagerTab.Name = "ProjectManagerTab";
            this.ProjectManagerTab.SelectedIndex = 0;
            this.ProjectManagerTab.Size = new System.Drawing.Size(870, 510);
            this.ProjectManagerTab.TabIndex = 4;
            // 
            // AddButton
            // 
            this.AddButton.Controls.Add(this.splitContainer1);
            this.AddButton.Controls.Add(this.pNodeSearchButton);
            this.AddButton.Controls.Add(this.P_NODE_PATH_BOX);
            this.AddButton.Controls.Add(this.label1);
            this.AddButton.Location = new System.Drawing.Point(4, 28);
            this.AddButton.Name = "AddButton";
            this.AddButton.Padding = new System.Windows.Forms.Padding(3);
            this.AddButton.Size = new System.Drawing.Size(862, 478);
            this.AddButton.TabIndex = 0;
            this.AddButton.Text = "⚙ setup";
            this.AddButton.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 45);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.SetUpFolderTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.setupGridView);
            this.splitContainer1.Size = new System.Drawing.Size(853, 427);
            this.splitContainer1.SplitterDistance = 281;
            this.splitContainer1.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Maroon;
            this.label3.Location = new System.Drawing.Point(6, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(203, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Do Not Double Click The Check Box";
            // 
            // SetUpFolderTreeView
            // 
            this.SetUpFolderTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SetUpFolderTreeView.CheckBoxes = true;
            this.SetUpFolderTreeView.Location = new System.Drawing.Point(3, 33);
            this.SetUpFolderTreeView.Name = "SetUpFolderTreeView";
            this.SetUpFolderTreeView.Size = new System.Drawing.Size(275, 391);
            this.SetUpFolderTreeView.TabIndex = 5;
            this.SetUpFolderTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.SetUpFolderTreeView_AfterCheck);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label2.Location = new System.Drawing.Point(234, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Other dwg Files";
            // 
            // setupGridView
            // 
            this.setupGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.setupGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.setupGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.setupGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DwgFile,
            this.DwgPath});
            this.setupGridView.Location = new System.Drawing.Point(3, 33);
            this.setupGridView.Name = "setupGridView";
            this.setupGridView.RowHeadersVisible = false;
            this.setupGridView.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.setupGridView.Size = new System.Drawing.Size(562, 391);
            this.setupGridView.TabIndex = 0;
            this.setupGridView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SetupGridView_CellContentDoubleClick);
            // 
            // DwgFile
            // 
            this.DwgFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DwgFile.FillWeight = 18.65101F;
            this.DwgFile.HeaderText = "Dwg";
            this.DwgFile.Name = "DwgFile";
            this.DwgFile.ReadOnly = true;
            this.DwgFile.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DwgFile.ToolTipText = "Dwg file name";
            this.DwgFile.Width = 57;
            // 
            // DwgPath
            // 
            this.DwgPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DwgPath.FillWeight = 158.4062F;
            this.DwgPath.HeaderText = "Path";
            this.DwgPath.Name = "DwgPath";
            this.DwgPath.ReadOnly = true;
            // 
            // pNodeSearchButton
            // 
            this.pNodeSearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pNodeSearchButton.Location = new System.Drawing.Point(784, 16);
            this.pNodeSearchButton.Name = "pNodeSearchButton";
            this.pNodeSearchButton.Size = new System.Drawing.Size(32, 23);
            this.pNodeSearchButton.TabIndex = 3;
            this.pNodeSearchButton.Text = "...";
            this.pNodeSearchButton.UseVisualStyleBackColor = true;
            this.pNodeSearchButton.Click += new System.EventHandler(this.pNodeSearchButton_Click);
            // 
            // P_NODE_PATH_BOX
            // 
            this.P_NODE_PATH_BOX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.P_NODE_PATH_BOX.Location = new System.Drawing.Point(134, 18);
            this.P_NODE_PATH_BOX.Name = "P_NODE_PATH_BOX";
            this.P_NODE_PATH_BOX.Size = new System.Drawing.Size(644, 21);
            this.P_NODE_PATH_BOX.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "P_NODE FILE (.dwg)";
            // 
            // ProgramManagerForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 563);
            this.Controls.Add(this.ProjectManagerTab);
            this.Controls.Add(this.FinishButton);
            this.Name = "ProgramManagerForm";
            this.Text = "Project Manager";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ProgramManagerForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.ProgramManagerForm_DragEnter);
            this.ProjectManagerTab.ResumeLayout(false);
            this.AddButton.ResumeLayout(false);
            this.AddButton.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.setupGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button FinishButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn Path;
        private System.Windows.Forms.DataGridViewTextBoxColumn File;
        private System.Windows.Forms.TabControl ProjectManagerTab;
        private System.Windows.Forms.TabPage AddButton;
        private System.Windows.Forms.DataGridView setupGridView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button pNodeSearchButton;
        private System.Windows.Forms.TextBox P_NODE_PATH_BOX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView SetUpFolderTreeView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridViewTextBoxColumn DwgFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn DwgPath;
        private System.Windows.Forms.Label label3;
    }
}

