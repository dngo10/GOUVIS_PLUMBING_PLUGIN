using ClassLibrary1.HELPERS;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ProjectManager
{
    public partial class ProgramManagerForm : Form
    {
        public ProgramManagerForm()
        {
            InitializeComponent();
            ImageList imageList = new ImageList();
            imageList.Images.Add(Properties.Resources.DWG);
            imageList.Images.Add(Properties.Resources.folder);

            SetUpFolderTreeView.ImageList = imageList;
        }

        private void pNodeSearchButton_Click(object sender, EventArgs e)
        {
            Model.GetFiles(P_NODE_PATH_BOX, projectNumTextBox, setupGridView, SetUpFolderTreeView);
        }

        private void SetUpFolderTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode tn = e.Node;
            foreach (TreeNode childtn in tn.Nodes)
            {
                childtn.Checked = tn.Checked; 
            }
            Model.CheckedTreeNode(tn, P_NODE_PATH_BOX.Text);
            Model.GetDatas(setupGridView, P_NODE_PATH_BOX.Text);
        }

        private void SetupGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dView = sender as DataGridView;
            //IT's always on because the path is at index 1; RowIndex is important.
            DataGridViewCell dCell = dView[1, e.RowIndex];
            string dataPath = (string)dCell.Value;

            string fullPath = Model.ProjectFolder + dataPath;
            fullPath = GoodiesPath.ConvertPathToXDrive(fullPath);

            string cadPath = GoodiesPath.GetAcadPath();

            Process process = new Process();
            ProcessStartInfo info = new ProcessStartInfo(cadPath, "/i " + "\"" + fullPath + "\"");
            process.StartInfo = info;
            process.Start();
        }

        private void FinishButton_Click(object sender, EventArgs e)
        {
            if (Model.UpdateDatabase())
            {
                this.Close();
            }
        }

        private void projectNumTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void ProgramManagerForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                foreach (string path in fileList)
                {
                    if(path.Contains(ConstantName.databasePostFix + ".db"))
                    {
                        string relativePNotePath = Model.ReadDatabase(path);
                        P_NODE_PATH_BOX.Text = Model.ProjectFolder + relativePNotePath;
                        projectNumTextBox.Text = Model.ProjectNumber;
                        Model.UpdateTheForm(SetUpFolderTreeView, setupGridView, P_NODE_PATH_BOX.Text);
                        break;
                    }
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void P_NODE_PATH_BOX_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void ProgramManagerForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void ProgramManagerForm_Load(object sender, EventArgs e)
        {

        }
    }
}
