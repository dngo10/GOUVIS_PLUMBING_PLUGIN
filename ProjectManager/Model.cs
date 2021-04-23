using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1.HELPERS;
using ClassLibrary1.DATABASE;
using System.Windows.Forms;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using ClassLibrary1.DATABASE.Controllers;

namespace ProjectManager
{
    class Model
    {
        public static string ProjectFolder;
        //if file is checked, it's in here.
        public static void GetFiles(TextBox textBox, DataGridView dataGridView, TreeView treeView)
        {
            
            CommonOpenFileDialog nodeFileDialog = new CommonOpenFileDialog();
            nodeFileDialog.Title = "Find P Note .dwg File";

            do
            {
                CommonFileDialogResult nodeResult = nodeFileDialog.ShowDialog();

                if (nodeResult == CommonFileDialogResult.Ok)
                {
                    string PNotePath = nodeFileDialog.FileName;
                    int i = CheckNodePath(PNotePath);
                    if (i == 2)
                    {
                        break;
                    }
                    else if (i == 0)
                    {
                        return;
                    }else if(i == 1)
                    {
                        continue;
                    }
                }
                else
                {
                    return;
                }
            } while (true);

            

            //LINES BELOW HAVE TO BE IN ORDER

            textBox.Text = nodeFileDialog.FileName;

            GetFilesDrag(textBox, dataGridView, treeView);
        }

        //This function is similar to GetFiles, but PNote is defined
        public static void GetFilesDrag(TextBox textBox, DataGridView dataGridView, TreeView treeView)
        {
            //They will clear up everything, so we have to keep path.
            ClearUp(treeView, dataGridView);

            FileElement pe = new FileElement();
            pe.lastModified = File.GetLastWriteTimeUtc(textBox.Text);
            pe.relativePath = "\\" + Path.GetFileName(textBox.Text);

            DatabaseManager.projectElement = new ProjectElement(pe, new HashSet<FileElement>());

            //TextBox Hash Path... to find project number
            setProjectDirectory(textBox);

            GetFilesInTreeView(treeView, textBox.Text);
        }


        public static void setProjectDirectory(TextBox textBox)
        {
            ProjectFolder = Path.GetDirectoryName(textBox.Text);
            //ProjectNumber = DatabaseManager.GuessProjectNumber();
        }


        private static void ClearUp(TreeView SetUpFolderTreeView, DataGridView setupGridView)
        {
            Model.ProjectFolder = "";
            SetUpFolderTreeView.Nodes.Clear();
            PlumbingDatabaseManager.clear();
            setupGridView.Rows.Clear();
        }

        public static bool UpdateDatabase()//For now, update mean delete and recreate.
        {
            string dbFolder;
            if(DatabaseManager.HasDataBaseFolder(out dbFolder))
            {
                string msg = string.Format("There is an existing Database Folder(s) {0}, you do want to overwrite it?",
                    Path.GetFileNameWithoutExtension(dbFolder));
                DialogResult dResult = MessageBox.Show(msg, "Database Folder Found !", MessageBoxButtons.YesNo);
                if(dResult == DialogResult.Yes)
                {
                    if (DatabaseManager.DeleteDatabaseFolder())
                    {
                        DatabaseManager.UpdateInitDatabase();
                        return true;
                    }
                    else
                    {
                        string msg1 = string.Format("Cannot delete the Folder {0}. Also make sure there should be only one database for each project",dbFolder);
                        MessageBox.Show(msg1, "Can't delete Database Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                DatabaseManager.UpdateInitDatabase();
                return true;
            }
            return true;
        }

        //Return:
        // 0 -- Abort/Cancel
        // 1 -- Retry
        // 2 -- Ignore
        private static int CheckNodePath(string nodePath)
        {
            if (!GoodiesPath.IsDwgPath(nodePath))
            {
                string msg = string.Format("P_NOTES file ({0}) is not a .dwg file", nodePath);
                MessageBox.Show(msg, "Not a DWG file", MessageBoxButtons.OK);
                return 0;
            }else if (!GoodiesPath.IsNotePath(nodePath))
            {
                string msg = string.Format("Note file {0} does not look like a P_NOTES file.", nodePath);
                DialogResult result = MessageBox.Show(msg, "Does not look like P_NOTES file", MessageBoxButtons.AbortRetryIgnore);
                if(result == DialogResult.Ignore)
                {
                    return 2;
                }else if(result == DialogResult.Retry)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            return 2;
        }

        //Populate gridView
        public static void GetDatas(DataGridView dataGridView, string PNotePath)
        {
            dataGridView.Rows.Clear();

            foreach (FileElement fileElement in DatabaseManager.projectElement.Dwgs)
            {
                string filePath = fileElement.relativePath;
                string p = Path.GetFileName(filePath);

                dataGridView.Rows.Add(p, filePath);
            }
        }
        /// <summary>
        /// Fill up the Tree View, Assume "path" is VALID and path is Directory to PLBG folder.
        /// Because one project can have multiple PnotePath, each notePath is associated with a couple of files
        /// </summary>
        /// <param name="path">direction to PLBG folder</param>
        /// <param name="tree">tree view in winform</param>
        public static void GetFilesInTreeView(TreeView tree, string PNotePath)
        {
            TreeNode treePLBGNode = new TreeNode();
            GetAllDwgFilesInCurrentDirectory(Path.GetDirectoryName(PNotePath), ref treePLBGNode);

            tree.Nodes.Add(treePLBGNode);

            //Remove P_Note node.
            RemoveNodeTree(PNotePath, ref tree);
        }

        //This is for the TreeNode in Setup using Recursion
        public static void GetAllDwgFilesInCurrentDirectory(string path, ref TreeNode treeNode)
        {

            string[] directories = Directory.GetDirectories(path);
            TreeNode tn = new TreeNode(Path.GetFileNameWithoutExtension(path));
            tn.ImageIndex = 0;
            foreach (string directory in directories)
            {
                TreeNode newTn = new TreeNode(Path.GetFileNameWithoutExtension(directory));
                newTn.ImageIndex = 0; //1 is folder icon created in winform constructor
                GetAllDwgFilesInCurrentDirectory(directory, ref newTn);
                tn.Nodes.Add(newTn);
            }

            string[] files = Directory.GetFiles(path, "*.dwg");
            foreach (string file in files)
            {
                TreeNode newTn = new TreeNode(Path.GetFileName(file));
                newTn.ImageIndex = 1; //0 is dwg icon created in winform constructor
                tn.Nodes.Add(newTn);
            }

            treeNode = tn;
        }

        private static void RemoveNodeTree(string pNotePath, ref TreeView treeView)
        {
            if (treeView.Nodes.Count == 1)
            {
                TreeNode node = treeView.Nodes[0];
                TreeNode pNote = _SearchNoteNode(pNotePath, node);
                pNote.Remove();
            }

        }

        private static TreeNode _SearchNoteNode(string pNotePath, TreeNode node)
        {
            TreeNode PNote = null;
            foreach (TreeNode childNode in node.Nodes)
            {
                if (childNode.Nodes.Count == 0)
                {
                    string fullPath = ProjectFolder + GetTreeNotePath(childNode);
                    if (pNotePath == fullPath)
                    {
                        PNote = childNode;
                    }
                }
                else
                {
                    PNote = _SearchNoteNode(pNotePath, childNode);
                }
            }
            return PNote;
        }

        //At this point, the projectPath MUST HAVE BEEN INITIATED
        //Add/Remove a file in ChoosenDwgFiles;
        //Function GetData() should be called after this.
        public static void CheckedTreeNode(TreeNode tn, string currentNotePath)
        {

            if (string.IsNullOrEmpty(ProjectFolder)) return;

            string relativeDwgPath = GetTreeNotePath(tn);

            if (GoodiesPath.IsDwgPath(relativeDwgPath))
            {
                FileElement fe = new FileElement();
                fe.lastModified = GetModifiedOfFile(relativeDwgPath);
                if (fe.lastModified == DateTime.MinValue)
                {
                    //CHECK THIS
                }
                fe.relativePath = relativeDwgPath;

                //string fullDwgPath = Root + relativeDwgPath;
                if (tn.Checked)
                {
                    DatabaseManager.projectElement.Dwgs.Add(fe);
                }
                else
                {
                    DatabaseManager.projectElement.Dwgs.Remove(fe);
                }
            }
        }

        public static DateTime GetModifiedOfFile(string relativaPath)
        {
            string fullPath = ProjectFolder + relativaPath;

            if (File.Exists(fullPath))
            {
                return File.GetLastWriteTimeUtc(fullPath);
            }

            return DateTime.MinValue;
        }

        //You must understand that the check box event (in winform) is called Recursively, you don't
        //have to check whether down the nodes collection has child or not;
        private static string GetTreeNotePath(TreeNode tn)
        {
            if (tn.Parent == null)
            {
                return string.Format("");
            }
            return string.Format(@"{0}\{1}", GetTreeNotePath(tn.Parent), tn.Text);
        }

        //READ DATABASE -- IMPORTANCE --

        //RETURN RELATIVE PATH OF OF PNOTE (to put on text file)
        public static string ReadDatabase(string databasePath)
        {
            if (File.Exists(databasePath))
            {
                ProjectFolder = Directory.GetParent(Directory.GetParent(databasePath).FullName).FullName;
                PlumbingDatabaseManager.ReadDataAtBeginning(databasePath);
                //DatabaseManager.GuessProjectNumber();
                return DatabaseManager.projectElement.P_NOTE.relativePath;
            }return "";
        }

        public static void UpdateTheForm(TreeView tree, DataGridView gridView, string Pnotepath)
        {
            GetFilesInTreeView(tree, Pnotepath);
            //GetDatas(gridView, Pnotepath);
            ExpandAndCheckedTree(tree);
        }

        /// <summary>
        ///Assume the projectElement is filled after read,
        ///And TreeView is filled after read
        ///Now Indicate tree expansion and checked
        ///
        ///This is a process of READING projectElement
        ///Find Dwgs SELECTED BEFORE
        ///AND Indicate them on TREEVIEW
        /// </summary>
        /// <param name="tree">setupFolderTreeView</param>
        public static void ExpandAndCheckedTree(TreeView tree)
        {
            foreach(TreeNode tn in tree.Nodes)
            {
                if(tn.Nodes.Count > 0)
                {
                    CheckNode(tn);
                }
            }
        }

        private static void CheckNode(TreeNode tn)
        {
            if (GoodiesPath.IsDwgPath(tn.FullPath) && DatabaseManager.projectElement.ContainsPath(tn.FullPath))
            {
                tn.Checked = true;
                //This function start with parent folder of dwgs
                ExpandNode(tn.Parent);
            }
            else
            {
                if(tn.Nodes.Count >= 1)
                {
                    foreach (TreeNode treeNode in tn.Nodes)
                    {
                        CheckNode(treeNode);
                    }
                }
            }
        }

        //This function start with parent folder of dwgs
        private static void ExpandNode(TreeNode tn)
        {
            if (tn == null) return;

            var query = from TreeNode childTn in tn.Nodes
                        where childTn.Checked == false
                        select childTn;

            var node = query.FirstOrDefault();

            if(node == null)
            {
                tn.Checked = true;
            }

            if(!tn.IsExpanded) tn.Expand();
            ExpandNode(tn.Parent);
        }
    }
}
