using ClassLibrary1.Properties;
using GouvisPlumbingNew.HELPERS;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary1.PNOTE;


namespace ProjectManager
{
    public partial class ProgramManagerForm : Form
    {
        public ProgramManagerForm(string currentDirectory)
        {
            InitializeComponent();
            ImageList imageList = new ImageList();
            imageList.Images.Add(Resources.folder);
            imageList.Images.Add(Resources.DWG);

            if (!string.IsNullOrEmpty(currentDirectory))
            {
                string dataPath = currentDirectory + "\\" + ConstantName.centerFolder + "\\" + ConstantName.databasePostFix;
                if (System.IO.Directory.Exists(currentDirectory) &&
                    System.IO.Directory.Exists(currentDirectory + "\\" + ConstantName.centerFolder) &&
                    System.IO.File.Exists(dataPath)
                    )
                {
                    string relativePNotePath = Model.ReadDatabase(dataPath);
                    P_NODE_PATH_BOX.Text = Model.ProjectFolder + relativePNotePath;
                    Model.UpdateTheForm(SetUpFolderTreeView, setupGridView, P_NODE_PATH_BOX.Text);
                }
            }


            SetUpFolderTreeView.ImageList = imageList;
        }

        private void pNodeSearchButton_Click(object sender, EventArgs e)
        {
            Model.GetFiles(P_NODE_PATH_BOX, setupGridView, SetUpFolderTreeView);
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

        private void ProgramManagerForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                foreach (string path in fileList)
                {
                    if (System.IO.Path.GetFileName(path) == (ConstantName.databasePostFix))
                    {

                        string relativePNotePath = Model.ReadDatabase(path);
                        P_NODE_PATH_BOX.Text = Model.ProjectFolder + relativePNotePath;
                        SetUpFolderTreeView.Nodes.Clear();
                        setupGridView.Rows.Clear();
                        Model.UpdateTheForm(SetUpFolderTreeView, setupGridView, P_NODE_PATH_BOX.Text);
                        break;
                    }
                    else if (System.IO.Path.GetFileName(path) == ConstantName.centerFolder && Directory.Exists(path))
                    {
                        string databasePath = path + "/" + ConstantName.databasePostFix;
                        if (System.IO.File.Exists(databasePath))
                        {
                            string relativePNotePath = Model.ReadDatabase(databasePath);
                            P_NODE_PATH_BOX.Text = Model.ProjectFolder + relativePNotePath;
                            Model.UpdateTheForm(SetUpFolderTreeView, setupGridView, P_NODE_PATH_BOX.Text);
                            break;
                        }
                    }
                    {
                        if (GoodiesPath.IsNotePath(path))
                        {
                            P_NODE_PATH_BOX.Text = path;
                            Model.ProjectFolder = System.IO.Path.GetDirectoryName(path);
                            Model.GetFilesDrag(P_NODE_PATH_BOX, setupGridView, SetUpFolderTreeView);
                            break;
                        }
                    }
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
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

        private void ProgramManagerForm_Load_1(object sender, EventArgs e)
        {
        }
    }
}
