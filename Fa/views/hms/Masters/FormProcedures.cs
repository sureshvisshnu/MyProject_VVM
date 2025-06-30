using fa.model.Hms.Master;
using fa.views.hms.masters.upload;
using Fa.api.Hms;
using fa.libraries.utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Fa.views.utils.Report.Upload;
using DocumentFormat.OpenXml.Office2010.Excel;
using fa.views.hms.Masters;
using Color = System.Drawing.Color;
using fa.api.utils;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using VisioForge.Libs.MediaFoundation.OPM;
using System.Linq;

namespace fa.views.hms.masters
{
    public enum MedicalProcedureElemntColoumn
    {
        SLNO, NAME, DISC, FEE, REMOVE, ID
    }
    public enum KeywordsGridColumn
    {
        SNO, NAME, REMOVE, ID
    }
    public partial class FormProcedures : FormBase
    {
        public static string NameErrorMsg = "Name could not be empty, Please enter Name";
        public static string ProcedureNameExistsErrorMsg = "Medical Procedure {0} already exists";
        public static string FeeErrorMsg = "Fee could not be empty, Please enter Fee";
        public static string EnterKeywordNameExistsErrorMsg = "Keyword {0} already exists";
        public static string EnterProcedureNameExistsErrorMsg = "Element {0} already exists";
        public static string EnterProcedureorKeyWordErrorMsg = "Please enter {0}";
        public static string DoNotAllowToDeleteMedicalProcedureMsg = "Could not delete the Medical Procedure {0}, its used in use, please make it inactive";
        public static string ErrorDeleteMsg = "Error Deleting the Medical Procedure!, Please retry";
        public static string ActivReasonErrorMsg = "Reason could not be empty, please add reason for inactive";
        public static string FileDnloadErrorMsg = "Finished Downloading";
        public static string CreateMedicalProcedureOnloadText = "New {0}";
        public static string UpdateMedicalProcedureOnloadText = "Update medicalprocedure";
        public static string SelectedMedicalProcedureCategoryNotValidErrorMsg = "Somthing went wrong, The selected medicalprocedure category is not valid.";
        public static string NoMedicalProcedureCategoryFoundErrorMsg = "Before adding medicalprocedure, Add category first";
        public static string SelectedMedicalProcedureNotValidErrorMsg = "Somthing went wrong, The selected medicalprocedure is not valid.";
        public static string MedicalProcedureRemoveSuccess = "{0} {1} removed successfully";
        public static string ProcedureCatDeleteNotAllowErrorText = "Cannot delete {0} the procedure in this category is under use";
        public static string DeleteMedicalProcedureConfirmText = "Do you want to delete the medicalprocedure {0}?";
        public static string DeleteNotAllowErrorText = "Cannot delete {0} it is used in somewhere else!";
        public static string EnterNameErrorMsg = "Name could not be empty, Please enter name";
        public static string EnterCodeErrorMsg = "Procedure Code could not be empty, please enter code";
        public static string SelectedParentNotValidErrorMsg = "Somthing went wrong, The selected parent is not valid.";
        public static string UniqueNameErrorMsg = "{0} {1} already exists";
        public static string ChooseMedicalProcedureCategoryErrorMsg = "Please choose medicalprocedure category";
        public static string SaveSuccessText = "Saved success...";
        public static string CategoryNotAllowErrorText = "Cannot create a category under another category.";


        MedicalProcedureManager MedicalProcedureManager = null!;
        public bool CreateMedicalProcedureOnLoad = false;
        public string? Id;
        private FormProcedures()
        {
            InitializeComponent();
        }
        public FormProcedures(bool IsNew)
        {
            MedicalProcedureManager = MedicalProcedureManager.Instance;
            InitializeComponent();
            if (IsNew)
            {
                SetNewEntryForm();
            }
        }

        private void SetNewEntryForm()
        {
            TreeViewMedicalProcedure.Visible = false;
            TreeViewMedicalProcedure.Visible = false;
            TextBoxProcedureSearch.Visible = false;
            BtnMedicalProcedureNew.Visible = false;
            BtnMedicalProcedureDelete.Visible = false;
            BtnMedicalProcedureEdit.Visible = false;
            BtnMedicalProcedureCancel.Visible = false;
            TabControlMedicalProcedure.Location = new Point(12, 12);
            TabControlMedicalProcedureCategory.Location = new Point(12, 12);
            BtnMedicalProcedureExit.Location = new Point(726, 424);
            BtnMedicalProcedureSave.Location = new Point(637, 424);
            this.Text = "New Medical Procedure";
            this.Width = 839;
            this.Height = 527;
            BtnMedicalProcedureNew_ItemClickedEvent(this, null!);
        }

        private void FormProcedures_Load(object sender, EventArgs e)
        {
            LoadMedicalProcedure();
        }
        private void LoadMedicalProcedure()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                EnableForm(false);
                loadMedicalProcedureCombo();
                LoadMedicalProcedureByCategoryWithFilter();
                if (CreateMedicalProcedureOnLoad)
                {
                    TextBoxProcedureSearch.Visible = false;
                    BtnMedicalProcedureCancel.Visible = false;
                    BtnMedicalProcedureNew.Visible = false;
                    BtnMedicalProcedureEdit.Visible = false;
                    BtnMedicalProcedureDelete.Visible = false;
                    TreeViewMedicalProcedure.Visible = false;
                    BtnExport.Visible = false;
                    BtnImport.Visible = false;
                    if (Id == null)
                    {
                        this.Text = string.Format(CreateMedicalProcedureOnloadText, "MedicalProcedure");
                        NewMedicalProcedureToolStrip.PerformClick();
                    }
                    else
                    {
                        this.Text = UpdateMedicalProcedureOnloadText;
                        PointSaveOrUpdatedNode(TreeViewMedicalProcedure.Nodes, (Id + "@"));
                        BtnMedicalProcedureEdit_Click(this, null!);
                    }
                }
                this.formIsDirty = false;

            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void ResetForm()
        {
            MedicalProcedureErrorMsg.Text = "";
            TextBoxMedicalProcedureId.ResetText();
            TextBoxMedicalProcedureName.ResetText();
            TextBoxMedicalProcedureDisplayName.ResetText();
            TextBoxMedicalProcedureDescription.ResetText();
            TextBoxMedicalProcedureReason.ResetText();
            TextBoxcMedicalProcedureCurrency.Text = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision); ;
            CheckBoxMedicalProcedureIsActive.Checked = true;
            CheckBoxMedicalProcedureHasElement.Checked = false;
            GridViewElementInfo.Rows.Clear();

            TextBoxMedicalProcedureCategoryId.ResetText();
            TextBoxMedicalProcedureCategoryName.ResetText();
            TextBoxMedicalProcedureCategoryDisplayAs.ResetText();
            TextBoxMedicalProcedureCategoryDescription.ResetText();

            comboBoxSwapTextBoxMedicalProcedureCategory.SelectedIndex = -1;
            comboBoxSwapTextBoxMedicalProcedureParentCategory.SelectedIndex = -1;

            GridViewKeywords.Rows.Clear();
        }
        private void EnableForm(Boolean enable)
        {
            if (MedicalProcedureManager.ListMedicalProcedureCategoryByCompanyId(Global.Company.CompanyId).Count > 0)
            {
                TreeViewMedicalProcedure.Enabled = !enable;
                TextBoxProcedureSearch.ReadOnly = enable;
                TextBoxProcedureSearch.TabStop = !enable;
            }
            else
            {
                TreeViewMedicalProcedure.Enabled = false;
                TabControlMedicalProcedureCategory.Visible = true;
                TabControlMedicalProcedure.Visible = false;
                TextBoxProcedureSearch.ReadOnly = true;
                TextBoxProcedureSearch.TabStop = false;
            }
            TextBoxMedicalProcedureCategoryName.ReadOnly = !enable;
            TextBoxMedicalProcedureCategoryName.TabStop = enable;
            TextBoxMedicalProcedureCategoryDisplayAs.ReadOnly = !enable;
            TextBoxMedicalProcedureCategoryDisplayAs.TabStop = enable;
            TextBoxMedicalProcedureCategoryDescription.ReadOnly = !enable;
            TextBoxMedicalProcedureCategoryDescription.TabStop = enable;
            comboBoxSwapTextBoxMedicalProcedureParentCategory.Visible = enable;
            comboBoxSwapTextBoxMedicalProcedureParentCategory.TabStop = enable;
            comboBoxSwapTextBoxMedicalProcedureCategory.Visible = enable;

            TabControlMedicalProcedure.TabStop = enable;
            GridViewKeywords.Enabled = enable;
            TextBoxMedicalProcedureName.ReadOnly = !enable;
            TextBoxMedicalProcedureName.TabStop = enable;
            TextBoxMedicalProcedureDisplayName.ReadOnly = !enable;
            TextBoxMedicalProcedureDisplayName.TabStop = enable;
            TextBoxMedicalProcedureDescription.ReadOnly = !enable;
            TextBoxMedicalProcedureDescription.TabStop = enable;
            CheckBoxMedicalProcedureIsActive.Enabled = enable;
            CheckBoxMedicalProcedureHasElement.Enabled = enable;
            TextBoxcMedicalProcedureCurrency.ReadOnly = !enable;
            TextBoxcMedicalProcedureCurrency.TabStop = enable;
            comboBoxSwapTextBoxMedicalProcedureCategory.Visible = enable;
            comboBoxSwapTextBoxMedicalProcedureCategory.TabStop = enable;

            TextBoxMedicalProcedureReason.ReadOnly = CheckBoxMedicalProcedureIsActive.Checked;
            TextBoxMedicalProcedureReason.TabStop = !CheckBoxMedicalProcedureIsActive.Checked;

            GridViewElementInfo.Enabled = CheckBoxMedicalProcedureHasElement.Checked;
            if (!enable)
            {
                TextBoxMedicalProcedureReason.ReadOnly = !enable; TextBoxMedicalProcedureReason.TabStop = enable;
                GridViewElementInfo.Enabled = enable;
                BtnMedicalProcedureCancel.Enabled = enable;
                if (TreeViewMedicalProcedure.SelectedNode == null)
                {
                    BtnMedicalProcedureDelete.Enabled = enable;
                    BtnMedicalProcedureEdit.Enabled = enable;
                }
                else
                {
                    BtnMedicalProcedureDelete.Enabled = !enable;
                    BtnMedicalProcedureEdit.Enabled = !enable;
                }
                BtnMedicalProcedureNew.Enabled = !enable;
                BtnMedicalProcedureSave.Enabled = enable;
            }
            else
            {
                BtnMedicalProcedureCancel.Enabled = enable;
                BtnMedicalProcedureDelete.Enabled = !enable;
                BtnMedicalProcedureEdit.Enabled = !enable;
                BtnMedicalProcedureNew.Enabled = !enable;
                BtnMedicalProcedureSave.Enabled = enable;
                GridViewKeywords.Enabled = enable;
            }
        }

        private void loadMedicalProcedureCombo()
        {
            ComboUtils.InitializeMedicalProcedureCategoryCombo(comboBoxSwapTextBoxMedicalProcedureCategory, Global.Company.CompanyId);
            ComboUtils.InitializeParentMedicalProcedureCategoryCombo(comboBoxSwapTextBoxMedicalProcedureParentCategory, Global.Company.CompanyId);
        }
        private void PointSaveOrUpdatedNode(TreeNodeCollection nodes, string findtext)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Name.ToString().Trim() == findtext)
                {
                    node.Expand();
                    node.TreeView.SelectedNode = node.NextNode;
                    TreeViewMedicalProcedure.SelectedNode = node;
                    node.TreeView.Focus();
                    break;
                }
                PointSaveOrUpdatedNode(node.Nodes, findtext);
            }
        }

        private MedicalProcedure GetMedicalProcedureInfo()
        {
            if (TreeViewMedicalProcedure.SelectedNode != null)
            {
                MedicalProcedure MedicalProcedure = MedicalProcedureManager.GetMedicalProcedureById(long.Parse(TreeViewMedicalProcedure.SelectedNode.Name.Trim('@')));
                if (MedicalProcedure != null)
                {
                    return MedicalProcedure;
                }
            }
            return null!;
        }
        
        private void LoadMedicalProcedureByCategoryWithFilter()
        {
            string FilterString = TextBoxProcedureSearch.Text.Trim();
            TreeViewMedicalProcedure.Nodes.Clear();

            IList<MedicalProcedureCategory> MedicalProcedureCategory = MedicalProcedureManager.ListMedicalProcedureCategoryByFilterCompanyId(Global.Company.CompanyId, FilterString);
            IList<MedicalProcedure> MedicalProcedure = MedicalProcedureManager.FilterMedicalProcedureByNameCodeCompanyId(Global.Company.CompanyId, FilterString);

            Dictionary<int, TreeNode> categoryNodes = new Dictionary<int, TreeNode>();
            HashSet<int> addedProcedures = new HashSet<int>();  

            foreach (var lMedicalProcedureCategory in MedicalProcedureCategory.Where(s => s.ParentMedicalProcedureCategoryId == null))
            {
                TreeNode treeRoot = new TreeNode
                {
                    Text = lMedicalProcedureCategory.Name,
                    Name = lMedicalProcedureCategory.Id.ToString(),
                    ImageIndex = 0
                };

                TreeViewMedicalProcedure.Nodes.Add(treeRoot);
                categoryNodes[(int)lMedicalProcedureCategory.Id] = treeRoot;

                GetChildNode(lMedicalProcedureCategory.Id, treeRoot, MedicalProcedureCategory, MedicalProcedure, addedProcedures);
            }

            foreach (var procedure in MedicalProcedure)
            {
                if (procedure.MedicalProcedureCategory == null || !categoryNodes.ContainsKey((int)procedure.MedicalProcedureCategory.Id))
                {
                    MedicalProcedureCategory MedicalProceduresCat = MedicalProcedureManager.GetMedicalProcedureCategoryByNameCode(procedure.Name, Global.Company.CompanyId);

                    if (!categoryNodes.ContainsKey((int)MedicalProceduresCat.Id))
                    {
                        TreeNode parentNode = new TreeNode
                        {
                            Text = MedicalProceduresCat.Name,
                            Name = MedicalProceduresCat.Id.ToString(),
                            ImageIndex = 0
                        };

                        TreeViewMedicalProcedure.Nodes.Add(parentNode);
                        categoryNodes[(int)MedicalProceduresCat.Id] = parentNode;

                        GetChildNode(MedicalProceduresCat.Id, parentNode, MedicalProcedureCategory, MedicalProcedure, addedProcedures);
                    }

                    if (!addedProcedures.Contains((int)procedure.Id))
                    {
                        TreeNode procedureNode = new TreeNode
                        {
                            Text = procedure.Name,
                            Name = procedure.Id.ToString(),
                            ImageIndex = 1
                        };

                        categoryNodes[(int)MedicalProceduresCat.Id].Nodes.Add(procedureNode);
                        addedProcedures.Add((int)procedure.Id);  
                    }
                }
            }

            if (TreeViewMedicalProcedure.Nodes.Count > 0)
            {
                if (!string.IsNullOrEmpty(TextBoxProcedureSearch.Text))
                {
                    TreeViewMedicalProcedure.ExpandAll();
                }
                TreeViewMedicalProcedure.SelectedNode = TreeViewMedicalProcedure.Nodes[0];
            }
        }
        private void GetChildNode(long ParentMedicalProcedureCategoryId, TreeNode ParentTreeNode, IList<MedicalProcedureCategory> MedicalProcedureCategory, IList<MedicalProcedure> MedicalProcedure, HashSet<int> addedProcedures)
        {
            foreach (var lMedicalProcedureCategory in MedicalProcedureCategory.Where(x => x.ParentMedicalProcedureCategoryId == ParentMedicalProcedureCategoryId))
            {
                TreeNode childNode = new TreeNode
                {
                    Text = lMedicalProcedureCategory.Name,
                    Name = lMedicalProcedureCategory.Id.ToString(),
                    ImageIndex = 0
                };

                GetChildNode(lMedicalProcedureCategory.Id, childNode, MedicalProcedureCategory, MedicalProcedure, addedProcedures);
                ParentTreeNode.Nodes.Add(childNode);
            }

            foreach (var lMedicalProcedure in MedicalProcedure.Where(x => x.MedicalProcedureCategoryId == ParentMedicalProcedureCategoryId).ToList())
            {
                if (!addedProcedures.Contains((int)lMedicalProcedure.Id))  
                {
                    TreeNode childNode = new TreeNode
                    {
                        Text = lMedicalProcedure.Name,
                        Name = lMedicalProcedure.Id.ToString() + "@",
                        ImageIndex = 1
                    };

                    ParentTreeNode.Nodes.Add(childNode);
                    addedProcedures.Add((int)lMedicalProcedure.Id); 
                }
            }
        }

        private void BtnMedicalProcedureNew_ItemClickedEvent(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e != null)
            {
                if (e.ClickedItem.Text == "Category")
                {
                    BtnNewMedicalProcedureCategory_Click(sender, e);
                }
                else if (e.ClickedItem.Text == "Medical Procedure")
                {
                    BtnMedicalProcedureNew_Click(sender, e);
                }
            }
        }
        private void BtnNewMedicalProcedureCategory_Click(object sender, EventArgs e)
        {
            TextBoxProcedureSearch.ResetText();
            TabControlMedicalProcedureCategory.Visible = true;
            TabControlMedicalProcedure.Visible = false;
            MedicalProcedureClear();
            this.formIsDirty = false;
        }
        private void BtnMedicalProcedureNew_Click(object sender, EventArgs e)
        {
            if (MedicalProcedureManager.ListMedicalProcedureCategoryByCompanyId(Global.Company.CompanyId).Count > 0)
            {
                TabControlMedicalProcedureCategory.Visible = false;
                TabControlMedicalProcedure.Visible = true;
                TextBoxProcedureSearch.ResetText();
                MedicalProcedureClear();
                ResetForm();
                EnableForm(true);
                CheckBoxMedicalProcedureIsActive.Checked = true;
                TabControlMedicalProcedure.SelectedTab = TabProcedureInfo;
                TextBoxMedicalProcedureName.Select();
                this.formIsDirty = false;
            }
            else
            {
                NewCategoryToolStrip.PerformClick();
                MedicalProcedureErrorMsg.Text = NoMedicalProcedureCategoryFoundErrorMsg;
            }
            this.formIsDirty = false;
        }
        private void MedicalProcedureCategorytoolstrip_Click(object sender, EventArgs e)
        {
            TextBoxProcedureSearch.ResetText();
            TabControlMedicalProcedureCategory.Visible = true;
            TabControlMedicalProcedure.Visible = false;
            MedicalProcedureClear();
            if (TreeViewMedicalProcedure != null && TreeViewMedicalProcedure.SelectedNode != null && !TreeViewMedicalProcedure.SelectedNode.Name.Contains('@'))
            {
                comboBoxSwapTextBoxMedicalProcedureParentCategory.SelectedIndex = comboBoxSwapTextBoxMedicalProcedureParentCategory.FindStringExact(TreeViewMedicalProcedure.SelectedNode.Text);
            }
            this.formIsDirty = false;
        }

        private void newMedicalProcedureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MedicalProcedureManager.ListMedicalProcedureCategoryByCompanyId(Global.Company.CompanyId).Count > 0)
            {
                TabControlMedicalProcedureCategory.Visible = false;
                TabControlMedicalProcedure.Visible = true;
                TextBoxProcedureSearch.ResetText();
                MedicalProcedureClear();
                ResetForm();
                EnableForm(true);
                if (TreeViewMedicalProcedure != null && TreeViewMedicalProcedure.SelectedNode != null && !TreeViewMedicalProcedure.SelectedNode.Name.Contains('@'))
                {
                    comboBoxSwapTextBoxMedicalProcedureCategory.SelectedIndex = comboBoxSwapTextBoxMedicalProcedureCategory.FindStringExact(TreeViewMedicalProcedure.SelectedNode.Text);
                }
                CheckBoxMedicalProcedureIsActive.Checked = true;
                TabControlMedicalProcedure.SelectedTab = TabProcedureInfo;
                TextBoxMedicalProcedureName.Select();
                this.formIsDirty = false;
            }
            else
            {
                NewCategoryToolStrip.PerformClick();
                MedicalProcedureErrorMsg.Text = NoMedicalProcedureCategoryFoundErrorMsg;
            }
            this.formIsDirty = false;
        }

        private void MedicalProcedureClear()
        {
            ResetForm();
            EnableForm(true);
            loadMedicalProcedureCombo();
            if (TabControlMedicalProcedureCategory.Visible)
            {
                TextBoxMedicalProcedureCategoryName.Select();
            }
            else
            {
                TextBoxMedicalProcedureName.Select();
            };
        }

        private Boolean ValidateForm()
        {
            MedicalProcedureErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxMedicalProcedureName.Text.Trim()))
            {
                TabControlMedicalProcedure.SelectedTab = TabProcedureInfo;
                MedicalProcedureErrorMsg.Text = NameErrorMsg;
                TextBoxMedicalProcedureName.Select();
                return false;
            }
            if (!CheckBoxMedicalProcedureIsActive.Checked && string.IsNullOrEmpty(TextBoxMedicalProcedureReason.Text.Trim()))
            {
                TabControlMedicalProcedure.SelectedTab = TabProcedureInfo;
                MedicalProcedureErrorMsg.Text = ActivReasonErrorMsg;
                TextBoxMedicalProcedureReason.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxcMedicalProcedureCurrency.Text.Trim()) || TextBoxcMedicalProcedureCurrency.Text == "0.00")  // || TextBoxcMedicalProcedureCurrency.Text == "0")
            {
                TabControlMedicalProcedure.SelectedTab = TabProcedureInfo;
                MedicalProcedureErrorMsg.Text = FeeErrorMsg;
                TextBoxcMedicalProcedureCurrency.Select();
                return false;
            }

            for (int i = 0; i < GridViewKeywords.Rows.Count - 1; i++)
            {
                for (int j = 1; j < 2; j++)
                {
                    if (GridViewKeywords.Rows[i].Cells[(int)KeywordsGridColumn.NAME].Value == null || GridViewKeywords.Rows[i].Cells[(int)KeywordsGridColumn.NAME].Value.ToString()!.Trim() == string.Empty)
                    {
                        TabControlMedicalProcedure.SelectedTab = TabProcedureInfo;
                        MedicalProcedureErrorMsg.Text = string.Format(EnterProcedureorKeyWordErrorMsg, GridViewKeywords.Columns[j].HeaderText);
                        GridViewKeywords.Select();
                        GridViewKeywords.CurrentCell = GridViewKeywords[j, i];
                        return false;
                    }
                }
                int Countings = 0;
                for (int k = 0; k < GridViewKeywords.Rows.Count - 1; k++)
                {
                    if (GridViewKeywords.Rows[k].Cells[(int)KeywordsGridColumn.NAME].Value != null)
                    {
                        if (GridViewKeywords.Rows[i].Cells[(int)KeywordsGridColumn.NAME].Value.ToString() == GridViewKeywords.Rows[k].Cells[(int)KeywordsGridColumn.NAME].Value.ToString())
                        {
                            Countings++;
                        }
                    }
                    if (Countings > 1)
                    {
                        TabControlMedicalProcedure.SelectedTab = TabProcedureInfo;
                        MedicalProcedureErrorMsg.Text = string.Format(EnterKeywordNameExistsErrorMsg, GridViewKeywords.Rows[k].Cells[(int)KeywordsGridColumn.NAME].Value.ToString());
                        GridViewKeywords.Select();
                        GridViewKeywords.CurrentCell = GridViewKeywords[(int)KeywordsGridColumn.NAME, k];
                        return false;
                    }
                }
            }
            if (CheckBoxMedicalProcedureHasElement.Checked && GridViewElementInfo.Rows.Count == 1)
            {
                MedicalProcedureErrorMsg.Text = string.Format(EnterProcedureorKeyWordErrorMsg, "Procedure Elements");
                GridViewElementInfo.CurrentCell = GridViewElementInfo[1, 0];
                GridViewElementInfo.BeginEdit(true);
                return false;
            }
            if (CheckBoxMedicalProcedureHasElement.Checked && GridViewElementInfo.Rows.Count > 1)
            {
                for (int i = 0; i < GridViewElementInfo.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < 3; j++)
                    {
                        if (GridViewElementInfo.Rows[i].Cells[j].FormattedValue == null || GridViewElementInfo.Rows[i].Cells[j].FormattedValue.ToString()!.Trim() == string.Empty)
                        {
                            MedicalProcedureErrorMsg.Text = string.Format(EnterProcedureorKeyWordErrorMsg, GridViewElementInfo.Columns[j].HeaderText);
                            GridViewElementInfo.CurrentCell = GridViewElementInfo[j, i];
                            GridViewElementInfo.BeginEdit(true);
                            return false;
                        }
                    }
                    int Countings = 0;
                    for (int k = 0; k < GridViewElementInfo.Rows.Count - 1; k++)
                    {
                        if (GridViewElementInfo.Rows[k].Cells[(int)MedicalProcedureElemntColoumn.NAME].Value != null)
                        {
                            if (GridViewElementInfo.Rows[i].Cells[(int)MedicalProcedureElemntColoumn.NAME].Value.ToString() == GridViewElementInfo.Rows[k].Cells[(int)MedicalProcedureElemntColoumn.NAME].Value.ToString())
                            {
                                Countings++;
                            }
                        }
                        if (Countings > 1)
                        {
                            MedicalProcedureErrorMsg.Text = string.Format(EnterProcedureNameExistsErrorMsg, GridViewElementInfo.Rows[k].Cells[(int)MedicalProcedureElemntColoumn.NAME].Value.ToString());
                            GridViewElementInfo.Select();
                            GridViewElementInfo.CurrentCell = GridViewElementInfo[(int)MedicalProcedureElemntColoumn.NAME, k];
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnMedicalProcedureNew.ShowDropDown();
            }
            if (keyData == (Keys.F4))
            {
                BtnMedicalProcedureDelete.PerformClick();
            }
            if (keyData == (Keys.F7))
            {
                BtnMedicalProcedureEdit.PerformClick();
            }
            if (keyData == (Keys.F8))
            {
                BtnMedicalProcedureSave.PerformClick();
            }
            if (keyData == (Keys.F10))
            {
                BtnMedicalProcedureExit.PerformClick();
                return true;
            }
            if (keyData == (Keys.Escape))
            {
                BtnMedicalProcedureCancel.PerformClick();
                return true;
            }
            if (keyData == (Keys.Tab) && keyData != (Keys.Shift) && ActiveControl == BtnMedicalProcedureSave)
            {
                if (TabControlMedicalProcedureCategory.SelectedTab == TabPageMedicalProcedureCategory)
                {
                    TextBoxMedicalProcedureCategoryName.Focus();
                }
                if (GridViewKeywords.Enabled && comboBoxSwapTextBoxMedicalProcedureCategory.Visible)
                {
                    TextBoxMedicalProcedureName.Select();
                }
                return true;
            }
            if (keyData == (Keys.Tab) && this.ActiveControl == this.BtnMedicalProcedureEdit)
            {
                TabControlMedicalProcedure.Select();
                TextBoxMedicalProcedureName.Select();
                return true;
            }
            if (keyData == (Keys.Tab) && this.ActiveControl == this.TextBoxMedicalProcedureName)
            {
                TabControlMedicalProcedure.Select();
                TextBoxMedicalProcedureDisplayName.Select();
                return true;
            }
            if (keyData == (Keys.Tab) && this.ActiveControl == this.TextBoxMedicalProcedureDisplayName)
            {
                TabControlMedicalProcedure.Select();
                TextBoxMedicalProcedureDescription.Select();
                return true;
            }
            if (keyData == (Keys.Tab) && this.ActiveControl == this.TextBoxMedicalProcedureDescription)
            {
                TabControlMedicalProcedure.Select();
                comboBoxSwapTextBoxMedicalProcedureCategory.Select();
                return true;
            }
            if (keyData == (Keys.Tab) && this.ActiveControl == this.comboBoxSwapTextBoxMedicalProcedureCategory)
            {
                TabControlMedicalProcedure.Select();
                CheckBoxMedicalProcedureIsActive.Select();
                return true;
            }
            if (keyData == (Keys.Tab) && this.ActiveControl == this.TextBoxMedicalProcedureReason)
            {
                TabControlMedicalProcedure.Select();
                TextBoxcMedicalProcedureCurrency.Select();
                return true;
            }
            if (keyData == (Keys.Tab) && this.ActiveControl == this.TextBoxcMedicalProcedureCurrency)
            {
                GridViewKeywords.Select();
                GridViewKeywords.CurrentCell = GridViewKeywords[1, 0];
                return true;
            }
            if (keyData == (Keys.Tab | Keys.Shift) && this.ActiveControl == this.comboBoxSwapTextBoxMedicalProcedureCategory)
            {
                TabControlMedicalProcedure.Select();
                TextBoxMedicalProcedureDescription.Select();
                return true;
            }
            if (keyData == (Keys.Tab | Keys.Shift) && this.ActiveControl == this.TextBoxMedicalProcedureDescription)
            {
                TabControlMedicalProcedure.Select();
                TextBoxMedicalProcedureDisplayName.Select();
                return true;
            }
            if (keyData == (Keys.Tab | Keys.Shift) && this.ActiveControl == this.TextBoxMedicalProcedureDisplayName)
            {
                TabControlMedicalProcedure.Select();
                TextBoxMedicalProcedureName.Select();
                return true;
            }
            if (keyData == (Keys.Tab | Keys.Shift) && this.ActiveControl == this.TextBoxMedicalProcedureName)
            {
                BtnMedicalProcedureSave.Select();
                return true;
            }
            if (keyData == (Keys.Tab | Keys.Shift) && this.ActiveControl == this.TextBoxMedicalProcedureCategoryName)
            {
                BtnMedicalProcedureSave.Select();
                return true;
            }
            if (keyData == (Keys.Tab | Keys.Shift) && this.ActiveControl == this.BtnMedicalProcedureSave)
            {
                if (TabProcedureInfo.Visible)
                {
                    GridViewKeywords.CurrentCell = GridViewKeywords[1, 0];
                    GridViewKeywords.BeginEdit(true);
                }
                else if (TabPageMedicalProcedureCategory.Visible)
                {
                    if (comboBoxSwapTextBoxMedicalProcedureParentCategory.TextBox.Text != "")
                    {
                        comboBoxSwapTextBoxMedicalProcedureParentCategory.Select();
                    }
                    else
                    {
                        if (comboBoxSwapTextBoxMedicalProcedureParentCategory.Visible)
                        {
                            comboBoxSwapTextBoxMedicalProcedureParentCategory.Select();
                        }
                        else
                        {
                            TextBoxMedicalProcedureCategoryDescription.Select();
                        }
                    }
                }
                return true;
            }
            try
            {
                if (GridViewKeywords.CurrentCell != null && GridViewKeywords.CurrentCell.Selected)
                {
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewKeywords.CurrentCell.ColumnIndex == (int)KeywordsGridColumn.NAME && GridViewKeywords.CurrentRow.Index != 0)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewKeywords.CurrentCell.ColumnIndex == (int)KeywordsGridColumn.NAME && GridViewKeywords.CurrentRow.Index == 0)
                    {
                        SendKeys.Send("{tab}");
                    }
                    if (keyData == (Keys.Tab) && GridViewKeywords.CurrentCell.ColumnIndex == (int)KeywordsGridColumn.NAME && (GridViewKeywords.Rows.Count - 1) != GridViewKeywords.CurrentCell.RowIndex)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                    if (keyData == (Keys.Tab) && GridViewKeywords.CurrentCell.ColumnIndex == (int)KeywordsGridColumn.NAME && (GridViewKeywords.Rows.Count - 1) == GridViewKeywords.CurrentCell.RowIndex)
                    {
                        GridViewKeywords.CurrentCell = null;
                        BtnMedicalProcedureSave.Select();
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void BtnMedicalProcedureSave_Click(object sender, EventArgs e)
        {
            if (TabControlMedicalProcedureCategory.Visible && ValidateFormMedicalProcedureCategory())
            {
                MedicalProcedureCategory MedicalProcedureCategoryInfo = GetMedicalProcedureCategoryFrom();
                if (MedicalProcedureManager.MedicalProcedureCategoryNameUniqueById(MedicalProcedureCategoryInfo))
                {
                    MedicalProcedureCategory lMedicalProcedureCategoryFromDB = null!;
                    if (MedicalProcedureCategoryInfo.Id == 0)
                    {
                        lMedicalProcedureCategoryFromDB = MedicalProcedureManager.AddMedicalProcedureCategory(MedicalProcedureCategoryInfo);
                    }
                    else
                    {
                        MedicalProcedureCategory lMedicalProcedureCategoryById = MedicalProcedureManager.GetMedicalProcedureCategoryInfoById(MedicalProcedureCategoryInfo.Id);
                        if (lMedicalProcedureCategoryById != null)
                        {
                            lMedicalProcedureCategoryFromDB = MedicalProcedureManager.UpdateMedicalProcedureCategory(MedicalProcedureCategoryInfo);
                        }
                        else
                        {
                            DisplaySystemError(SelectedMedicalProcedureCategoryNotValidErrorMsg);
                            return;
                        }
                    }
                    if (CreateMedicalProcedureOnLoad)
                    {
                        FormProcedures_Load(sender, e);
                        return;
                    }
                    ResetForm();
                    TabControlMedicalProcedureCategory.SelectedTab = TabPageMedicalProcedureCategory;
                    loadMedicalProcedureCombo();
                    if (string.IsNullOrEmpty(TextBoxProcedureSearch.Text))
                    {
                        LoadMedicalProcedureByCategoryWithFilter();
                    }
                    else
                    {
                        TextBoxProcedureSearch.Clear();
                    }
                    PointSaveOrUpdatedNode(TreeViewMedicalProcedure.Nodes, lMedicalProcedureCategoryFromDB.Id.ToString());
                    EnableForm(false);
                    MedicalProcedureErrorMsg.Text = SaveSuccessText;
                    this.formIsDirty = false;
                }
                else
                {
                    MedicalProcedureErrorMsg.Text = string.Format(UniqueNameErrorMsg, "MedicalProcedureCategory", MedicalProcedureCategoryInfo.Name);
                    TextBoxMedicalProcedureCategoryName.Select();
                }
            }
            else if (TabControlMedicalProcedure.Visible && validateForm())
            {
                MedicalProcedure lMedicalProcedure = GetMedicalProcedureFromForm();

                if (MedicalProcedureManager.ProcedureCodeByUnique(lMedicalProcedure)) 
                {
                    if (MedicalProcedureManager.MedicalProcedureNameUniqueById(lMedicalProcedure))
                    {
                        MedicalProcedure MedicalProcedureFromDB = null!;

                        if (lMedicalProcedure.Id == 0)
                        {
                            MedicalProcedureFromDB = MedicalProcedureManager.AddMedicalProcedure(lMedicalProcedure);
                        }
                        else
                        {
                            MedicalProcedure lMedicalProcedureById = MedicalProcedureManager.GetMedicalProcedureById(lMedicalProcedure.Id);
                            if (lMedicalProcedureById != null)
                            {
                                MedicalProcedureFromDB = MedicalProcedureManager.UpdateMedicalProcedure(lMedicalProcedure);
                            }
                            else
                            {
                                DisplaySystemError(SelectedMedicalProcedureNotValidErrorMsg);
                                return;
                            }
                        }

                        if (CreateMedicalProcedureOnLoad)
                        {
                            this.formIsDirty = false;
                            this.Close();
                        }

                        ResetForm();
                        TabControlMedicalProcedure.SelectedTab = TabProcedureInfo;
                        loadMedicalProcedureCombo();

                        if (string.IsNullOrEmpty(TextBoxProcedureSearch.Text))
                        {
                            LoadMedicalProcedureByCategoryWithFilter();
                        }
                        else
                        {
                            TextBoxProcedureSearch.Clear();
                        }

                        PointSaveOrUpdatedNode(TreeViewMedicalProcedure.Nodes, MedicalProcedureFromDB.Id.ToString() + "@");
                        MedicalProcedureErrorMsg.Text = SaveSuccessText;
                        EnableForm(false);
                        this.formIsDirty = false;
                    }
                    else
                    {
                        MedicalProcedureErrorMsg.Text = string.Format(UniqueNameErrorMsg, "MedicalProcedure", lMedicalProcedure.Name);
                        TextBoxMedicalProcedureName.Select();
                    }
                }
                else
                {
                    MedicalProcedureErrorMsg.Text = string.Format(UniqueNameErrorMsg, "Medical Procedure Code", lMedicalProcedure.ProcedureCode);
                    TextBoxMedicalProcedureCode.Select();
                }
            }
        }
        private Boolean ValidateFormMedicalProcedureCategory()
        {
            MedicalProcedureErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxMedicalProcedureCategoryName.Text.Trim()))
            {
                MedicalProcedureErrorMsg.Text = string.Format(EnterNameErrorMsg, "MedicalProcedure Category");
                TextBoxMedicalProcedureCategoryName.Select();
                return false;
            }
            if (comboBoxSwapTextBoxMedicalProcedureParentCategory.SelectedIndex >= 0) // && MedicalProcedureManager.GetMedicalProcedureCategoryInfoById(((MedicalProcedureCategory)comboBoxSwapTextBoxMedicalProcedureParentCategory.Items[comboBoxSwapTextBoxMedicalProcedureParentCategory.SelectedIndex]).Id) == null)
            {
                MedicalProcedureErrorMsg.Text = CategoryNotAllowErrorText;
                comboBoxSwapTextBoxMedicalProcedureParentCategory.Select();
                return false;
            }
            return true;
        }
        private Boolean validateForm()
        {
            MedicalProcedureErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxMedicalProcedureName.Text.Trim()))
            {
                MedicalProcedureErrorMsg.Text = EnterNameErrorMsg;
                TextBoxMedicalProcedureName.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxMedicalProcedureCode.Text.Trim()))
            {
                MedicalProcedureErrorMsg.Text = EnterCodeErrorMsg;
                TextBoxMedicalProcedureCode.Select();
                return false;
            }
            if (comboBoxSwapTextBoxMedicalProcedureCategory.SelectedIndex < 0)
            {
                MedicalProcedureErrorMsg.Text = ChooseMedicalProcedureCategoryErrorMsg;
                TabControlMedicalProcedure.SelectedTab = TabProcedureInfo;
                comboBoxSwapTextBoxMedicalProcedureCategory.Select();
                return false;
            }
            if (!CheckBoxMedicalProcedureIsActive.Checked && string.IsNullOrEmpty(TextBoxMedicalProcedureReason.Text.Trim()))
            {
                MedicalProcedureErrorMsg.Text = ActivReasonErrorMsg;
                TextBoxMedicalProcedureReason.Select();
                return false;
            }

            return true;
        }

        private MedicalProcedureCategory GetMedicalProcedureCategoryFrom()
        {
            MedicalProcedureCategory lMedicalProcedureCategory = new MedicalProcedureCategory();
            if (!string.IsNullOrEmpty(TextBoxMedicalProcedureCategoryId.Text))
            {
                lMedicalProcedureCategory.Id = Convert.ToInt64(TextBoxMedicalProcedureCategoryId.Text);
            }
            else
            {
                lMedicalProcedureCategory.Id = 0L;
            }
            lMedicalProcedureCategory.Name = TextBoxMedicalProcedureCategoryName.Text.Trim();
            lMedicalProcedureCategory.Description = TextBoxMedicalProcedureCategoryDescription.Text.Trim();
            lMedicalProcedureCategory.DisplayAs = TextBoxMedicalProcedureCategoryDisplayAs.Text.Trim();
            lMedicalProcedureCategory.CompanyId = Global.Company.CompanyId;
            if (comboBoxSwapTextBoxMedicalProcedureParentCategory.SelectedIndex > -1)
            {
                MedicalProcedureCategory MedicalProcedureCategory = (MedicalProcedureCategory)comboBoxSwapTextBoxMedicalProcedureParentCategory.Items[comboBoxSwapTextBoxMedicalProcedureParentCategory.SelectedIndex];
                if (MedicalProcedureCategory != null)
                {
                    MedicalProcedureCategory = MedicalProcedureManager.GetMedicalProcedureCategoryInfoById(MedicalProcedureCategory.Id);
                    if (MedicalProcedureCategory != null)
                    {
                        lMedicalProcedureCategory.ParentMedicalProcedureCategoryId = MedicalProcedureCategory.Id;
                        lMedicalProcedureCategory.IsSubMedicalProcedureCategory = true;
                    }
                }
            }
            else
            {
                lMedicalProcedureCategory.IsSubMedicalProcedureCategory = false;
            }
            return lMedicalProcedureCategory;
        }

        private MedicalProcedure GetMedicalProcedureFromForm()
        {
            MedicalProcedure ListMedicalProcedure = new MedicalProcedure();
            if (!string.IsNullOrEmpty(TextBoxMedicalProcedureId.Text))
            {
                ListMedicalProcedure.Id = Convert.ToInt64(TextBoxMedicalProcedureId.Text);
            }
            else
            {
                ListMedicalProcedure.Id = 0L;
            }
            ListMedicalProcedure.Name = TextBoxMedicalProcedureName.Text;
            ListMedicalProcedure.ProcedureCode = TextBoxMedicalProcedureCode.Text;
            ListMedicalProcedure.DisplayAs = TextBoxMedicalProcedureDisplayName.Text;
            ListMedicalProcedure.Description = TextBoxMedicalProcedureDescription.Text;
            ListMedicalProcedure.Fee = double.Parse(TextBoxcMedicalProcedureCurrency.Text);
            ListMedicalProcedure.IsActive = CheckBoxMedicalProcedureIsActive.Checked;
            ListMedicalProcedure.Reason = TextBoxMedicalProcedureReason.Text;
            ListMedicalProcedure.HasElement = CheckBoxMedicalProcedureHasElement.Checked;
            ListMedicalProcedure.CompanyId = Global.Company.CompanyId;
            if (comboBoxSwapTextBoxMedicalProcedureCategory.SelectedIndex > -1)
            {
                MedicalProcedureCategory medicalProcedureCategory = (MedicalProcedureCategory)comboBoxSwapTextBoxMedicalProcedureCategory.Items[comboBoxSwapTextBoxMedicalProcedureCategory.SelectedIndex];
                if (medicalProcedureCategory != null)
                {
                    medicalProcedureCategory = MedicalProcedureManager.GetMedicalProcedureCategoryInfoById(medicalProcedureCategory.Id);
                    if (medicalProcedureCategory != null)
                    {
                        ListMedicalProcedure.MedicalProcedureCategoryId = medicalProcedureCategory.Id;
                    }
                }
            }
            //ListMedicalProcedure.ProcedureElements = new List<MedicalProcedureElement>();
            //if (CheckBoxMedicalProcedureHasElement.Checked)
            //{
            //    for (int i = 0; i < GridViewElementInfo.Rows.Count - 1; i++)
            //    {
            //        MedicalProcedureElement MedicalProcedureElement = new MedicalProcedureElement();
            //        MedicalProcedureElement.CompanyId = Global.Company.CompanyId;
            //        MedicalProcedureElement.Id = GridViewElementInfo.Rows[i].Cells[(int)MedicalProcedureElemntColoumn.ID].Value != null ? long.Parse(GridViewElementInfo.Rows[i].Cells[(int)MedicalProcedureElemntColoumn.ID].Value.ToString()) : 0L;
            //        MedicalProcedureElement.Name = GridViewElementInfo.Rows[i].Cells[(int)MedicalProcedureElemntColoumn.NAME].Value.ToString();
            //        MedicalProcedureElement.Discription = GridViewElementInfo.Rows[i].Cells[(int)MedicalProcedureElemntColoumn.DISC].Value.ToString();
            //        MedicalProcedureElement.Fee = double.Parse(GridViewElementInfo.Rows[i].Cells[(int)MedicalProcedureElemntColoumn.FEE].Value.ToString());
            //        ListMedicalProcedure.ProcedureElements.Add(MedicalProcedureElement);
            //    }
            //}
            ListMedicalProcedure.IsActive = CheckBoxMedicalProcedureIsActive.Checked;
            ListMedicalProcedure.Reason = TextBoxMedicalProcedureReason.Text.Trim(); ;
            ListMedicalProcedure.CompanyId = Global.Company.CompanyId;
            ListMedicalProcedure.Keywords = ListMedicalProcedureKeyword();
            //ListMedicalProcedure.Keywords = new List<MedicalProcedureKeyword>();
            //for (int i = 0; i < GridViewKeywords.Rows.Count - 1; i++)
            //{
            //    MedicalProcedureKeyword lMedicalProcedureKeyword = new MedicalProcedureKeyword();
            //    lMedicalProcedureKeyword.CompanyId = Global.Company.CompanyId;
            //    lMedicalProcedureKeyword.Text = GridViewKeywords.Rows[i].Cells[(int)KeywordsGridColumn.NAME].Value.ToString();
            //    ListMedicalProcedure.Keywords.Add(lMedicalProcedureKeyword);
            //}
            return ListMedicalProcedure;
        }
        private IList<MedicalProcedureKeyword> ListMedicalProcedureKeyword()
        {
            IList<MedicalProcedureKeyword> MedicalProcedureKeyword = new List<MedicalProcedureKeyword>();
            int Count = GridViewKeywords.Rows.Count;
            if (Count > 1)
            {
                MedicalProcedureKeyword = new List<MedicalProcedureKeyword>();
                for (int i = 0; i < GridViewKeywords.Rows.Count - 1; i++)
                {
                    if (GridViewKeywords.Rows[i].Cells[(int)KeywordsGridColumn.NAME].Value.ToString()!.Trim() != "")
                    {
                        MedicalProcedureKeyword lMedicalProcedureKeyword = new MedicalProcedureKeyword();
                        lMedicalProcedureKeyword.CompanyId = Global.Company.CompanyId;
                        lMedicalProcedureKeyword.Text = GridViewKeywords.Rows[i].Cells[(int)KeywordsGridColumn.NAME].Value.ToString()!.Trim();

                        MedicalProcedureKeyword.Add(lMedicalProcedureKeyword);
                    }
                }
            }
            return MedicalProcedureKeyword;
        }

        private void LoadMedicalProcedureInfo()
        {
            MedicalProcedure MedicalProcedureFromDB = GetMedicalProcedureInfo();
            if (MedicalProcedureFromDB != null)
            {
                GridViewElementInfo.Rows.Clear();
                TextBoxMedicalProcedureId.Text = MedicalProcedureFromDB.Id.ToString();
                TextBoxMedicalProcedureCode.Text = MedicalProcedureFromDB.ProcedureCode ?? string.Empty;
                TextBoxMedicalProcedureName.Text = MedicalProcedureFromDB.Name;
                TextBoxMedicalProcedureDisplayName.Text = MedicalProcedureFromDB.DisplayAs;
                comboBoxSwapTextBoxMedicalProcedureCategory.SelectedIndex = MedicalProcedureFromDB.MedicalProcedureCategory != null ? comboBoxSwapTextBoxMedicalProcedureCategory.FindStringExact(MedicalProcedureFromDB.MedicalProcedureCategory.Name) : 0;
                comboBoxSwapTextBoxMedicalProcedureCategory.Text = MedicalProcedureFromDB.MedicalProcedureCategory!.Name;
                TextBoxMedicalProcedureDescription.Text = MedicalProcedureFromDB.Description;
                TextBoxcMedicalProcedureCurrency.Text = MedicalProcedureFromDB.Fee.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                CheckBoxMedicalProcedureIsActive.Checked = MedicalProcedureFromDB.IsActive;
                TextBoxMedicalProcedureReason.Text = MedicalProcedureFromDB.Reason;
                //CheckBoxMedicalProcedureHasElement.Checked = MedicalProcedureFromDB.HasElement;
                //if (MedicalProcedureFromDB.ProcedureElements.Count > 0)
                //{
                //    GridViewElementInfo.Rows.Add(MedicalProcedureFromDB.ProcedureElements.Count);
                //    int i = 0;
                //    foreach (MedicalProcedureElement MedicalProcedureElement in MedicalProcedureFromDB.ProcedureElements)
                //    {
                //        GridViewElementInfo.Rows[i].Cells[(int)MedicalProcedureElemntColoumn.SLNO].Value = i + 1;
                //        GridViewElementInfo.Rows[i].Cells[(int)MedicalProcedureElemntColoumn.NAME].Value = MedicalProcedureElement.Name;
                //        GridViewElementInfo.Rows[i].Cells[(int)MedicalProcedureElemntColoumn.DISC].Value = MedicalProcedureElement.Discription;
                //        GridViewElementInfo.Rows[i].Cells[(int)MedicalProcedureElemntColoumn.FEE].Value = MedicalProcedureElement.Fee;
                //        GridViewElementInfo.Rows[i].Cells[(int)MedicalProcedureElemntColoumn.ID].Value = MedicalProcedureElement.Id;
                //        i++;
                //    }
                //}
                ReSequence();
                if (MedicalProcedureFromDB.Keywords.Count > 0)
                {
                    GridViewKeywords.Rows.Clear();
                    GridViewKeywords.Rows.Add(MedicalProcedureFromDB.Keywords.Count);
                    int i = 0;
                    foreach (MedicalProcedureKeyword MedicalProcedureKeyword in MedicalProcedureFromDB.Keywords)
                    {
                        GridViewKeywords.Rows[i].Cells[(int)KeywordsGridColumn.SNO].Value = i + 1;
                        GridViewKeywords.Rows[i].Cells[(int)KeywordsGridColumn.NAME].Value = MedicalProcedureKeyword.Text;
                        GridViewKeywords.Rows[i].Cells[(int)KeywordsGridColumn.ID].Value = MedicalProcedureKeyword.Id;
                        i++;
                    }
                }
                ReSequenceKeyword();
            }
            else
            {
                DisplaySystemError(SelectedMedicalProcedureNotValidErrorMsg);
                return;
            }
        }
        private void ReSequence()
        {
            for (int i = 0; i < GridViewElementInfo.Rows.Count; i++)
            {
                GridViewElementInfo.Rows[i].Cells[(int)MedicalProcedureElemntColoumn.SLNO].Value = i + 1;
            }
        }
        private void ReSequenceKeyword()
        {
            for (int i = 0; i < GridViewKeywords.Rows.Count; i++)
            {
                GridViewKeywords.Rows[i].Cells[(int)KeywordsGridColumn.SNO].Value = i + 1;
            }
        }
        private void GridViewElementInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void BtnMedicalProcedureDelete_Click(object sender, EventArgs e)
        {
            if (TabControlMedicalProcedure.Visible == true)
            {
                MedicalProcedure MedicalProcedureInfo = GetMedicalProcedureInfo();
                MedicalProcedureErrorMsg.Text = "";
                if (MedicalProcedureInfo != null)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete the Medical Procedure, " + MedicalProcedureInfo.Name + "?", "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        if (MedicalProcedureManager.DeleteMedicalProcedure(MedicalProcedureInfo.Id))
                        {
                            ResetForm();
                            TabControlMedicalProcedure.SelectedTab = TabProcedureInfo;
                            loadMedicalProcedureCombo();
                            LoadMedicalProcedureByCategoryWithFilter();
                            TextBoxProcedureSearch.Clear();
                            EnableForm(false);
                            TreeViewMedicalProcedure.Select();
                            MedicalProcedureErrorMsg.Text = string.Format(MedicalProcedureRemoveSuccess, "MedicalProcedure", MedicalProcedureInfo.Name);
                            this.formIsDirty = false;
                        }
                        else
                        {
                            MedicalProcedureErrorMsg.Text = string.Format(DeleteNotAllowErrorText, "MedicalProcedure");
                        }
                        this.formIsDirty = false;
                    }
                    else
                    {
                        MedicalProcedureErrorMsg.Text = ErrorDeleteMsg;
                    }
                }
                else
                {
                    DisplaySystemError(SelectedMedicalProcedureNotValidErrorMsg);
                    return;
                }
            }
            else if (TabControlMedicalProcedureCategory.Visible == true)
            {
                MedicalProcedureCategory MedicalProcedureCategoryinfo = GetMedicalProcedureCategoryInfo();
                MedicalProcedureErrorMsg.Text = "";
                if (MedicalProcedureCategoryinfo != null)
                {
                    DialogResult Result = MessageBox.Show(string.Format(DeleteMedicalProcedureConfirmText, MedicalProcedureCategoryinfo.Name), "Delete Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        if (MedicalProcedureManager.DeleteMedicalProcedureCategory(MedicalProcedureCategoryinfo.Id))
                        {
                            ResetForm();
                            TabControlMedicalProcedureCategory.SelectedTab = TabPageMedicalProcedureCategory;
                            loadMedicalProcedureCombo();
                            LoadMedicalProcedureByCategoryWithFilter();
                            TextBoxProcedureSearch.Clear();
                            EnableForm(false);
                            SetFocus();
                            MedicalProcedureErrorMsg.Text = string.Format(MedicalProcedureRemoveSuccess, "MedicalProcedureCategory", MedicalProcedureCategoryinfo.Name);
                            this.formIsDirty = false;
                        }
                        else
                        {
                            MedicalProcedureErrorMsg.Text = string.Format(ProcedureCatDeleteNotAllowErrorText, " MedicalProcedure Category");
                        }
                    }

                }
                else
                {
                    DisplaySystemError(SelectedMedicalProcedureNotValidErrorMsg);
                    return;
                }
            }
        }
        private void BtnMedicalProcedureCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show("There are unsaved changes, Do you want cancel?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.No)
                {
                    FocusMedicalProcedureCategory();
                    return;
                }
            }
            ResetForm();
            loadMedicalProcedureCombo();
            if (string.IsNullOrEmpty(TextBoxProcedureSearch.Text))
            {
                LoadMedicalProcedureCategoryInfo();
            }
            else
            {
                TextBoxProcedureSearch.Clear();
            }
            EnableForm(false);
            SetFocus();
            this.formIsDirty = false;
        }
        private void FocusMedicalProcedureCategory()
        {
            if (TabControlMedicalProcedure.Visible)
            {
                TabControlMedicalProcedure.SelectedTab = TabProcedureInfo;
                TextBoxMedicalProcedureName.Select();
            }
            else
            {
                TextBoxMedicalProcedureCategoryName.Select();
            }
        }
        private void SetFocus()
        {
            if (TreeViewMedicalProcedure.Nodes.Count > 0)
            {
                TextBoxProcedureSearch.Select();
            }
            else
            {
                BtnMedicalProcedureNew.Focus();
            }
        }

        private void BtnMedicalProcedureExit_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show("There are unsaved changes, Do you want to Exit?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.Yes)
                {
                    this.Close();
                }
                return;
            }
            else
            {
                this.Close();
            }
        }
        private void TreeViewMedicalProcedure_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node!;
            node.SelectedImageIndex = node.ImageIndex;
            ResetForm();
            loadMedicalProcedureCombo();
            LoadMedicalProcedureCategoryInfo();
            EnableForm(false);
            this.formIsDirty = false;
        }
        private void TreeViewMedicalProcedure_Enter(object sender, EventArgs e)
        {
            if (TreeViewMedicalProcedure.Nodes == null || TreeViewMedicalProcedure.Nodes.Count == 0)
            {
                BtnMedicalProcedureNew.Select();
            }
        }
        private void GridViewElementInfo_KeyPress1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                ((ComboBox)GridViewElementInfo.EditingControl).DroppedDown = false;
            }
        }
        private void GridViewElementInfo_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            GridViewElementInfo.CommitEdit(DataGridViewDataErrorContexts.Commit);
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                ((ComboBox)e.Control).MaxLength = 10;
                e.Control.KeyPress += new KeyPressEventHandler(GridViewElementInfo_KeyPress1!);
                if (GridViewElementInfo.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }
                else
                {
                    if (GridViewElementInfo.CurrentCell.FormattedValue != null)
                    {
                        ((ComboBox)e.Control).SelectedIndex = -1;
                        ((ComboBox)e.Control).Text = GridViewElementInfo.CurrentCell.Value.ToString();
                    }
                }
            }
        }
        private void GridViewElementInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MedicalProcedureErrorMsg.Text = string.Empty;
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == (int)MedicalProcedureElemntColoumn.REMOVE && GridViewElementInfo.Rows.Count - 1 != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete row " + GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalProcedureElemntColoumn.SLNO].Value + "?", "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        GridViewElementInfo.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewElementInfo.Rows.RemoveAt(e.RowIndex);
                        ReSequence();
                    }
                }
            }
        }
        private void GridViewElementInfo_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalProcedureElemntColoumn.SLNO].ReadOnly = true;
            GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalProcedureElemntColoumn.DISC].ReadOnly = true;
            GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalProcedureElemntColoumn.FEE].ReadOnly = true;
            if (GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalProcedureElemntColoumn.NAME].Value != null)
            {
                GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalProcedureElemntColoumn.DISC].ReadOnly = false;
                GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalProcedureElemntColoumn.FEE].ReadOnly = false;
            }
        }
        private void GridViewElementInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.BackColor = Color.White;
            e.CellStyle.ForeColor = Color.Black;
        }
        private void GridViewElementInfo_Enter(object sender, EventArgs e)
        {
            GridViewElementInfo.CurrentCell = GridViewElementInfo[1, 0];
        }
        private void GridViewElementInfo_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalProcedureElemntColoumn.SLNO].Value = e.RowIndex + 1;
        }
        private void CheckBoxMedicalProcedureHasElement_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxMedicalProcedureHasElement.Checked)
            {
                GridViewElementInfo.Enabled = true;
            }
            else
            {
                GridViewElementInfo.Enabled = false;
            }
        }
        private void GridViewKeywords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == (int)KeywordsGridColumn.REMOVE && GridViewKeywords.Rows.Count - 1 != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete row " + GridViewKeywords.Rows[e.RowIndex].Cells[(int)KeywordsGridColumn.SNO].Value + "?", "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        GridViewKeywords.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewKeywords.Rows.RemoveAt(e.RowIndex);
                        ReSequenceKeyword();
                    }
                }
            }
        }
        private void GridViewKeywords_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void GridViewKeywords_Enter(object sender, EventArgs e)
        {
            GridViewKeywords.CurrentCell = GridViewKeywords[1, 0];
        }
        private void GridViewKeywords_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            GridViewKeywords.Rows[e.RowIndex].Cells[(int)KeywordsGridColumn.SNO].Value = GridViewKeywords.Rows.Count;
        }
        private void CheckBoxMedicalProcedureIsActive_CheckedChanged(object sender, EventArgs e)
        {
            MedicalProcedureErrorMsg.Text = "";
            TextBoxMedicalProcedureReason.ReadOnly = CheckBoxMedicalProcedureIsActive.Checked;
            TextBoxMedicalProcedureReason.TabStop = !CheckBoxMedicalProcedureIsActive.Checked;
            if (CheckBoxMedicalProcedureIsActive.Checked)
            {
                TextBoxMedicalProcedureReason.ResetText();
            }
        }
        private void BtnMedicalProcedureEdit_Click(object sender, EventArgs e)
        {
            LoadMedicalProcedureCategoryInfo();
            EnableForm(true);
            if (TabControlMedicalProcedureCategory.Visible)
            {
                if (comboBoxSwapTextBoxMedicalProcedureParentCategory.SelectedIndex < 0)
                {
                    comboBoxSwapTextBoxMedicalProcedureParentCategory.Visible = false;
                }
                TabControlMedicalProcedureCategory.SelectedTab = TabPageMedicalProcedureCategory;
                TextBoxMedicalProcedureCategoryName.Select();
            }
            else if (TabControlMedicalProcedure.Visible)
            {
                TabControlMedicalProcedure.SelectedTab = TabProcedureInfo;
                TextBoxMedicalProcedureName.Select();
                TextBoxMedicalProcedureReason.ReadOnly = CheckBoxMedicalProcedureIsActive.Checked;
            }
            this.formIsDirty = false;
        }
        private void LoadMedicalProcedureCategoryInfo()
        {
            if (TreeViewMedicalProcedure.SelectedNode != null)
            {
                if (!TreeViewMedicalProcedure.SelectedNode.Name.Contains('@'))
                {
                    TabControlMedicalProcedureCategory.Visible = true;
                    TabControlMedicalProcedure.Visible = false;
                    MedicalProcedureCategory MedicalProcedureCategoryDB = GetMedicalProcedureCategoryInfo();
                    if (MedicalProcedureCategoryDB != null)
                    {
                        TextBoxMedicalProcedureCategoryId.Text = MedicalProcedureCategoryDB.Id.ToString();
                        TextBoxMedicalProcedureCategoryName.Text = MedicalProcedureCategoryDB.Name;
                        TextBoxMedicalProcedureCategoryDisplayAs.Text = MedicalProcedureCategoryDB.DisplayAs;
                        TextBoxMedicalProcedureCategoryDescription.Text = MedicalProcedureCategoryDB.Description;
                        if (MedicalProcedureCategoryDB.ParentMedicalProcedureCategory != null)
                        {
                            comboBoxSwapTextBoxMedicalProcedureParentCategory.SelectedIndex = comboBoxSwapTextBoxMedicalProcedureParentCategory.FindStringExact(MedicalProcedureCategoryDB.ParentMedicalProcedureCategory.Name);
                            comboBoxSwapTextBoxMedicalProcedureParentCategory.Text = MedicalProcedureCategoryDB.Name;
                        }
                        else
                        {
                            comboBoxSwapTextBoxMedicalProcedureParentCategory.SelectedIndex = -1;
                        }
                    }
                    else
                    {
                        DisplaySystemError(SelectedMedicalProcedureCategoryNotValidErrorMsg);
                        return;
                    }
                }
                else
                {
                    TabControlMedicalProcedure.Visible = true; 
                    TabControlMedicalProcedure.SelectedTab = TabProcedureInfo;
                    TabControlMedicalProcedureCategory.Visible = false;
                    LoadMedicalProcedureInfo();
                }
            }
        }
        private MedicalProcedureCategory GetMedicalProcedureCategoryInfo()
        {
            MedicalProcedureCategory CostCenterFromDB = MedicalProcedureManager.GetMedicalProcedureCategoryInfoById(long.Parse(TreeViewMedicalProcedure.SelectedNode.Name));
            if (CostCenterFromDB != null)
            {
                return CostCenterFromDB;
            }
            return null!;
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            this.formIsDirty = false;
            LoadMedicalProcedureByCategoryWithFilter();
            return;
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            MedicalProcedureErrorMsg.Text = "";
            ProcedureExportFile ProcedureExportFiles = new ProcedureExportFile();
            ProcedureExportFiles.GenerateFile(Global.Company.CompanyId);

        }
        private void BtnImport_Click(object sender, EventArgs e)
        {
            MedicalProcedureErrorMsg.Text = string.Empty;
            ProcedureFileUpload FileUploadDialog = new ProcedureFileUpload();
            FileUploadDialog.Text = "Data Upload - Procedure Master";
            FileUploadDialog.ShowDialog();
            if (FileUploadDialog.FileUploadComplete == true)
            {
                this.Cursor = Cursors.WaitCursor;
                ResetForm();
                EnableForm(false);
                LoadMedicalProcedure();
                this.Cursor = Cursors.Default;
                this.formIsDirty = false;
            }
        }
        private void GetMedicalProcedures()
        {
            string FilterProcedure = TextBoxProcedureSearch.Text.Trim();
            TreeViewMedicalProcedure.Nodes.Clear();
            IList<MedicalProcedure> Procedures = MedicalProcedureManager.Instance.ListMedicalProcedureByCompanyId(Global.Company.CompanyId);
            if (Procedures.Count > 0)
            {
                foreach (var lProcedures in Procedures)
                {
                    if (FilterProcedure == null || string.IsNullOrEmpty(FilterProcedure.Trim()) || lProcedures.Name.IndexOf(FilterProcedure, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        TreeViewMedicalProcedure.Nodes.Add(lProcedures.Id.ToString(), lProcedures.Name);
                        TreeViewMedicalProcedure.Nodes[lProcedures.Id.ToString()].ImageIndex = 0;
                    }
                }
            }
            if (TreeViewMedicalProcedure.Nodes.Count > 0)
            {
                TreeViewMedicalProcedure.ExpandAll();
                TreeViewMedicalProcedure.SelectedNode = TreeViewMedicalProcedure.Nodes[0];
            }
        }
        private void TextBoxProcedureSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && TreeViewMedicalProcedure.Nodes != null && TreeViewMedicalProcedure.Nodes.Count > 0)
            {
                TreeViewMedicalProcedure.Select();
            }
        }

        private void TextBoxProcedureSearch_TextChanged(object sender, EventArgs e)
        {
            ResetForm();
            LoadMedicalProcedureByCategoryWithFilter();
            if (TreeViewMedicalProcedure.Nodes.Count > 0) { BtnMedicalProcedureEdit.Enabled = true; BtnMedicalProcedureDelete.Enabled = true; }
            else { BtnMedicalProcedureEdit.Enabled = false; BtnMedicalProcedureDelete.Enabled = false; }
            this.formIsDirty = false;
        }

        private void CheckBoxMedicalProcedureIsActive_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift && !CheckBoxMedicalProcedureIsActive.Checked)
            {
                e.IsInputKey = true;
                TextBoxMedicalProcedureReason.TabStop = true;
                TextBoxMedicalProcedureReason.Select();
            }
        }

        private void TreeViewMedicalProcedure_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeViewMedicalProcedure.SelectedNode = e.Node;
        }
    }
}
