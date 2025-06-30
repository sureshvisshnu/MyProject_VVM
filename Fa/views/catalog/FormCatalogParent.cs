using fa.api.catalog;
using fa.model.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace fa.views.catalog
{
    public partial class FormCatalogParent : Form
    {
        public static readonly int SEARCH_TYPE_PARENT = 1;
        public static readonly int SEARCH_TYPE_PARENT_AND_PRODUCTFMLY = 2;

        private int _SearchType = SEARCH_TYPE_PARENT;
        public int SearchType
        {
            get
            {
                return _SearchType;
            }
            set
            {
                if (value == SEARCH_TYPE_PARENT || value == SEARCH_TYPE_PARENT_AND_PRODUCTFMLY)
                    _SearchType = value;
                else
                    _SearchType = SEARCH_TYPE_PARENT;
            }
        }

        public CatalogItemType Type;
        public long CatalogItemId;
        FormCatalog parent = null;
        public static string DeletedParentErrorMsg = "Your Select Parent is Remove, Please press Go button then select the Parent";

        public FormCatalogParent(object sender)
        {
            parent = (FormCatalog)sender;
            InitializeComponent();
        }
        private void FormCatalogParent_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ErrorMsg.Text = "";
            LoadCatalogWithFilter();
            ab2ToolStripCatalogParent.Select();
            SendKeys.Send("{tab}");
            Cursor.Current = Cursors.Default;
        }
        private void LoadCatalogWithFilter()
        {
            try
            {
                BtnSelect.Enabled = true;
                Cursor.Current = Cursors.WaitCursor;
                string FilterString = ToolStripSearchTextBox.Text.Trim();
                TreeViewCatalog.Nodes.Clear();
                IList<CatalogItem> CatalogItems = CatalogItemManager.Instance.ListParentCatalogItemsByCompanyId(Global.Company.CompanyId, FilterString, SearchType);
                if (CatalogItems.Count>0)
                {
                    foreach (var CatalogItem in CatalogItems)
                    {
                        if (!(SearchType == 1 && CatalogItem.Id == CatalogItemId))
                        {
                            TreeNode treeNode = new TreeNode();
                            treeNode.Text = CatalogItem.ToString();
                            treeNode.Name = CatalogItem.Id.ToString();
                            treeNode.ImageIndex = 0;
                            if (CatalogItem.Type == CatalogItemType.CATEGORY && CatalogItem.ParentId == null)
                            {
                                TreeViewCatalog.Nodes.Add(treeNode);
                                LoadChildNodes(treeNode, CatalogItem.Id, CatalogItems);
                            }
                        }
                    }
                }
                else
                {
                    BtnSelect.Enabled = false;
                }

                if (TreeViewCatalog.Nodes.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ToolStripSearchTextBox.Text.Trim()))
                    {
                        TreeViewCatalog.ExpandAll();
                    }
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void LoadChildNodes(TreeNode ParentNode, long ParentId, IList<CatalogItem> CatalogItems)
        {
            foreach (var CatalogItem in CatalogItems)
            {
                if (!(SearchType == 1 && CatalogItem.Id == CatalogItemId))
                {
                    if (CatalogItem.Type != CatalogItemType.PRODUCT && CatalogItem.ParentId == ParentId)
                    {
                        TreeNode ChildNode = new TreeNode();
                        ChildNode.Text = CatalogItem.ToString();
                        if (CatalogItem.Type == CatalogItemType.CATEGORY)
                        {
                            ChildNode.Name = CatalogItem.Id.ToString();
                            ChildNode.ImageIndex = 0;
                            ParentNode.Nodes.Add(ChildNode);
                            LoadChildNodes(ChildNode, CatalogItem.Id, CatalogItems);
                        }
                        else if (CatalogItem.Type == CatalogItemType.PRODUCTFAMILY && SearchType == SEARCH_TYPE_PARENT_AND_PRODUCTFMLY)
                        {
                            ChildNode.Name = CatalogItem.Id.ToString() + "#";
                            ChildNode.ImageIndex = 1;
                            ParentNode.Nodes.Add(ChildNode);
                            LoadChildNodes(ChildNode, CatalogItem.Id, CatalogItems);
                        }
                    }
                }
            }
        }
        private void TextBoxCatalogSearch_TextChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            LoadCatalogWithFilter();
            Cursor.Current = Cursors.Default;
        }

        private void TreeViewCatalog_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (TreeViewCatalog.SelectedNode != null)
            {
                ParentNode();
            }
        }
        private void TextBoxCatalogSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                TreeViewCatalog.Select();
            }
        }

        private void TreeViewCatalog_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter && TreeViewCatalog.SelectedNode!=null)
            {
                ParentNode();
            }
        }
        private void ParentNode()
        {
            Cursor.Current = Cursors.WaitCursor;
            TreeNode TreeNode = TreeViewCatalog.SelectedNode;
            
                if (!TreeNode.Name.Contains('#') && !TreeNode.Name.Contains('@'))
                {
                    if (CatalogItemManager.Instance.GetCatalogItemInfoById(long.Parse(TreeNode.Name)) != null)
                    {
                        if (Type == CatalogItemType.CATEGORY)
                        {
                            parent.TextBoxCategoryParent.Id = TreeNode.Name;
                            parent.TextBoxCategoryParent.Text = TreeNode.Text;
                            this.Close();
                        }
                        else if (Type == CatalogItemType.PRODUCTFAMILY)
                        {
                            parent.TextBoxProductFamilyParent.Id = TreeNode.Name;
                            parent.TextBoxProductFamilyParent.Text = TreeNode.Text;
                            this.Close();
                        }
                    }
                    else
                    {
                        ErrorMsg.Text = DeletedParentErrorMsg;
                    }

                }
                else if (TreeNode.Name.Contains('#'))
                {
                    if (CatalogItemManager.Instance.GetCatalogItemInfoById(long.Parse(TreeNode.Name.Replace("#", ""))) != null)
                    {
                        if (Type == CatalogItemType.PRODUCT)
                        {
                            parent.TextBoxProductParent.Id = TreeNode.Name.Replace("#", "");
                            parent.TextBoxProductParent.Text = TreeNode.Text;
                            this.Close();
                        }
                    }
                    else
                    {
                        ErrorMsg.Text = DeletedParentErrorMsg;
                    }
                }
            
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Escape))
            {
                this.Close();
            }
            else if (keyData == (Keys.F8))
            {
                BtnSelect.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void TreeViewCatalog_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            node.SelectedImageIndex = node.ImageIndex;
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            LoadCatalogWithFilter();
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (TreeViewCatalog.SelectedNode != null)
            {
                ParentNode();
            }
            else if(TreeViewCatalog.Nodes.Count>0)
            {
                ErrorMsg.Text = "Please select parent";
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ToolStripSearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Down)
            {
                TreeViewCatalog.Select();
            }
        }

        private void BtnCancel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ToolStripSearchTextBox.Focus();
            }

            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (BtnSelect.Enabled) BtnSelect.Select();
                else TreeViewCatalog.Select();
            }
        }

        private void ToolStripSearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
