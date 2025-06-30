using System;
using System.Collections.Generic;
using System.Windows.Forms;
using fa.libraries.utils;
using fa.views.controls.ComboTreeView;

namespace fa.views.controls.text
{
    public delegate void NodeClickedDelegate(object sender, ComboTreeNodeEventArgs e);

    public partial class CheckedTreeComboBox : UserControl
    {
        public event NodeClickedDelegate? NodeClickedEvent;
        private ComboTreeBox MainTreeView = null!;
        public CheckedTreeComboBox()
        {
            MainTreeView = new ComboTreeBox();
            MainTreeView.ShowCheckBoxes = true;
            InitializeComponent();
        }
        public void CheckedTreeComboBox_NodeClicked(object sender, ComboTreeNodeEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (NodeClickedEvent != null)
                {
                    NodeClickedEvent(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally { Cursor.Current = Cursors.Default; }
        }
        public override string Text
        {
            get
            {
                return TextBox.Text;
            }
            set
            {
                TextBox.Text = value;
            }
        }
        public ComboTreeNodeCollection Nodes
        {
            get
            {
                return MainTreeView.Nodes;
            }
            set
            {
                ComboTreeBox.Nodes = value;
                MainTreeView.Nodes = value;
            }
        }
        public ComboTreeNode TreeNodes
        {
            set
            {
                MainTreeView.Nodes.Add(value);
                ComboTreeBox.Nodes.Add(value);
            }
        }
        public IList<ComboTreeNode> CheckedNodes
        {
            get
            {
                return ListallCheckedNodes();
            }
        }
        public ComboTreeNode SelectedNode
        {
            get
            {
                return ComboTreeBox.SelectedNode;
            }
            set
            {
                ComboTreeBox.SelectedNode = value;
            }
        }
        private IList<ComboTreeNode> ListallCheckedNodes()
        {
            IList<ComboTreeNode> TreeNodes = null!;
            if (MainTreeView.Nodes.Count > 0)
            {
                TreeNodes = new List<ComboTreeNode>();
                foreach (ComboTreeNode node in MainTreeView.Nodes)
                    if (node.Nodes.Count > 0)
                    {
                        if (node.Checked)
                        {
                            TreeNodes.Add(node);
                        }
                        TreeNodes = loadallCheckedChildnode(node, TreeNodes);
                    }
                    else
                    {
                        if (node.Checked)
                        {
                            TreeNodes.Add(node);
                        }
                    }
            }
            return TreeNodes;
        }
        private IList<ComboTreeNode> loadallCheckedChildnode(ComboTreeNode INNode, IList<ComboTreeNode> OutNode)
        {
            foreach (ComboTreeNode node in INNode.Nodes)
            {
                if (node.Nodes.Count > 0)
                {
                    if (node.Checked)
                    {
                        OutNode.Add(node);
                    }
                    OutNode = loadallCheckedChildnode(node, OutNode);
                }
                else
                {
                    if (node.Checked)
                    {
                        OutNode.Add(node);
                    }
                }
            }
            return OutNode;
        }
        private void sizechange()
        {
            this.Height = 21;
            ComboTreeBox.Size = base.Size;
            TextBox.Width = ComboTreeBox.Width - 16;
        }
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ComboTreeBox.Nodes.Clear();
                if (MainTreeView.Nodes.Count > 0)
                {
                    if (!string.IsNullOrEmpty(TextBox.Text))
                    {
                        FilterNodes();
                        ComboTreeBox.ExpandAll();
                    }
                    else
                    {
                        loadallnode();
                        ComboTreeBox.ExpandAll();
                    }
                }
                if (ComboTreeBox.Nodes.Count > 0)
                {
                    if (!ComboTreeBox.DroppedDown)
                    {
                        ComboTreeBox.ShowdropDown();
                    }
                }
            }
            finally
            {

                TextBox.Select();
                if (TextBox.TextLength > 0) { TextBox.SelectionStart = TextBox.TextLength; }

            }
        }
        private void TextBox_Click(object sender, EventArgs e)
        {
            if (!ComboTreeBox.DroppedDown)
            {
                ComboTreeBox.Nodes.Clear();
                FilterNodes();
                if (ComboTreeBox.Nodes.Count > 0)
                {
                    ComboTreeBox.ShowdropDown();
                }
                TextBox.Focus();
            }
            else
            {
                TextBox.Text = string.Empty;
            }
        }
        private void loadallnode()
        {
            ComboTreeNodeCollection nodeCollection = new ComboTreeNodeCollection();
            foreach (ComboTreeNode node in MainTreeView.Nodes)
            {
                ComboTreeNode TreeNode = new ComboTreeNode();
                if (node.Nodes.Count > 0)
                {
                    TreeNode.Name = node.Name;
                    TreeNode.Text = node.Text;
                    TreeNode.Checked = node.Checked;
                    nodeCollection.Add(loadallChildnode(node, TreeNode));
                }
                else
                {
                    TreeNode = new ComboTreeNode();
                    TreeNode.Name = node.Name;
                    TreeNode.Text = node.Text;
                    TreeNode.Checked = node.Checked;
                    nodeCollection.Add(TreeNode);
                }
            }
            ComboTreeBox.Nodes.AddRange(nodeCollection);
        }
        private ComboTreeNode loadallChildnode(ComboTreeNode INNode, ComboTreeNode OutNode)
        {
            foreach (ComboTreeNode node in INNode.Nodes)
            {
                OutNode.Nodes.Add(node);
                if (node.Nodes.Count > 0)
                {
                    loadallChildnode(node, OutNode);
                }
            }
            return OutNode;
        }
        private void FilterNodes()
        {
            ComboTreeNodeCollection nodeCollection = new ComboTreeNodeCollection();
            foreach (ComboTreeNode node in MainTreeView.Nodes)
            {
                if (node.Text != null && node.Text.ToLower().Contains(TextBox.Text.ToLower()))
                {
                    ComboTreeNode TreeNode = new ComboTreeNode();
                    TreeNode.Name = node.Name;
                    TreeNode.Text = node.Text;
                    TreeNode.Checked = node.Checked;
                    if (node.Nodes.Count > 0)
                    {
                        NodeTextChange(node.Nodes, TreeNode);
                    }
                    nodeCollection.Add(TreeNode);
                }
                else
                {
                    if (node.Nodes.Count > 0)
                    {
                        ComboTreeNode TreeNode = new ComboTreeNode();
                        ChNode = TreeNode.Nodes.Count;
                        if (ChNode != NodeTextChange(node.Nodes, TreeNode).Nodes.Count)
                        {
                            TreeNode.Name = node.Name;
                            TreeNode.Text = node.Text!;
                            TreeNode.Checked = node.Checked;
                            nodeCollection.Add(TreeNode);
                        }
                    }
                }
            }
            ComboTreeBox.Nodes.AddRange(nodeCollection);
        }

        int ChNode = 0;
        private ComboTreeNode NodeTextChange(ComboTreeNodeCollection MainTreeNode, ComboTreeNode TreeNode)
        {
            ComboTreeNodeCollection nodeCollection = new ComboTreeNodeCollection();
            foreach (ComboTreeNode node in MainTreeNode)
            {
                if (node.Text.ToLower().Contains(TextBox.Text.ToLower()))
                {
                    if (node.Nodes.Count > 0)
                    {
                        NodeTextChange(node.Nodes, TreeNode);
                    }
                    nodeCollection.Add(node);
                }
                else
                {
                    if (node.Nodes.Count > 0)
                    {

                        if (ChNode != NodeTextChange(node.Nodes, TreeNode).Nodes.Count)
                        {
                            nodeCollection.Add(node);
                        }
                    }
                }
            }
            TreeNode.Nodes.AddRange(nodeCollection);
            return TreeNode;
        }
        private void ComboTreeBox_AfterCheck(object sender, ComboTreeNodeEventArgs e)
        {
            NodeChecking(e, MainTreeView.Nodes);
            CheckedTreeComboBox_NodeClicked(sender, e);
        }
        public void NodeChecking(ComboTreeNodeEventArgs TreeViewEventArgs, ComboTreeNodeCollection MainTreeNode)
        {
            foreach (ComboTreeNode node in MainTreeNode)
            {
                if (node.Name == TreeViewEventArgs.Node.Name && node.Text == TreeViewEventArgs.Node.Text)
                {
                    ComboTreeNode tns = MainTreeView.Nodes.FindByName(TreeViewEventArgs.Node.Name, StringComparison.CurrentCultureIgnoreCase, true);
                    if (tns != null)
                    {
                        if (tns.Checked != TreeViewEventArgs.Node.Checked)
                        {
                            tns.Checked = TreeViewEventArgs.Node.Checked;
                        }
                        MainTreeView.SelectedNode = tns;
                    }
                }
                if (node.Nodes.Count > 0)
                {
                    NodeChecking(TreeViewEventArgs, node.Nodes);
                }
            }
        }
        protected override bool IsInputKey(Keys keyData)
        {
            return true;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab)
            {
                TextBox.Text = string.Empty;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 9)
            {
                TextBox.Text = string.Empty;
                ComboTreeBox.ShowdropDown();
                SendKeys.Send("{Tab}");
            }
            else if (e.KeyChar == 8 && TextBox.Text.Length > 0)
            {
                string Temb = TextBox.Text;
                TextBox.Text = Temb.Remove((TextBox.Text.Length - 1), 1);
            }
            else if (e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
            {
                if (TextBox.SelectionLength != 0) { TextBox.Text = TextBox.Text.Remove(TextBox.SelectionStart, TextBox.SelectionLength); }
                TextBox.Text = TextBox.Text.Insert(TextBox.SelectionStart, e.KeyChar.ToString());
            }
            else
            {
                TextBox.Select();
            }
        }
        private void ComboTreeBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);
        }
        private void TextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);
        }
        private void CheckedTreeComboBox_ClientSizeChanged(object sender, EventArgs e)
        {
            sizechange();
        }
        private void CheckedTreeComboBox_MouseClick(object sender, MouseEventArgs e)
        {
            base.OnMouseClick(e);
        }
    }
}
