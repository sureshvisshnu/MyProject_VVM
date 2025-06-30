using fa;
using fa.libraries.utils;
using fa.libraries.Validation;
using fa.views;
using fa.views.hms.Masters;
using Fa.api.Hms;
using FADataAccessLibrary.Api.Hms;
using FADataAccessLibrary.Model.Hms.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fa.views.hms.Masters
{
    public partial class FormQuestion : FormBase
    {
        public static string DoNotAllowToSaveNamExisteMsg = "Allergie {0} already exists";
        public static string ErrorDeleteMsg = "Error deleting the allergie!, please retry";
        public static string DoNotAllowToDeleteMsg = "Allergie {0} used in some where else like Ip visit";
        public static string EnterNameErrorMsg = "Name could not be empty, please enter name";
        public static string EnterKeywordNameExistsErrorMsg = "Keyword {0} already exists";
        public static string EnterKeyWordErrorMsg = "Please enter {0}";
        public static string ActivReasonErrorMsg = "Reason could not be empty, please add reason for inactive";
        public static string FileDnloadErrorMsg = "Finished Downloading";
        public static string NoCategoryFoundErrorMsg = "Before adding allergie, Add allergie category";
        public static string SelectedParentNotValidErrorMsg = "Somthing went wrong, the selected parent is not valid.";
        public static string SelectedAllergieCategoryNotValidErrorMsg = "Somthing went wrong, the selected allergie category is not valid.";
        public static string SelectedAllergieNotValidErrorMsg = "Somthing went wrong, the selected allergie is not valid.";
        public static string CreateAllergieOnloadText = "New {0}";
        public static string UpdateAllergieOnloadText = "Update allergie";
        public static string UniqueNameErrorMsg = "{0} {1} already exists";
        public static string SaveSuccessText = "Saved success...";
        public static string ChooseAllergieCategoryErrorMsg = "Please choose allergie category";
        public static string AllergieRemoveSuccess = "{0} {1} romoved successfully";
        public static string AllergieDeleteNotAllowErrorText = "Cannot delete {0} it's used in some ware else";
        public static string DeleteAllergieConfirmText = "Do you want to delete the allergie {0}?";
        public static string DeleteNotAllowErrorText = "Cannot delete {0} it's used in some ware else";


        QuestionManager AllergieManager = null;
        KeypressValidation KeypressValidation = null;
        DateValidation DateValidation = null;
        public bool CreateAllergieOnLoad = false;
        public string Id;
        public enum AllergiesKeywordsTableColumn
        {
            SNO, NAME, REMOVE, ID
        }
        public FormQuestion()
        {
            InitializeComponent();
            AllergieManager = QuestionManager.Instance;
            KeypressValidation = KeypressValidation.Instance;
            DateValidation = DateValidation.Instance;
            excludedObjects = new string[] { "TextBoxAllergieSearch" };
        }
        private void FormAllergie_Load(object sender, EventArgs e)
        {
            MyFormLoad();
        }
        private void MyFormLoad()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                TreeViewAllergie.Nodes.Clear();
                ResetForm();
                EnableForm(false);
                loadCombo();
                LoadAllergieByCategoryWithFilter();
                if (CreateAllergieOnLoad)
                {
                    TextBoxAllergieSearch.Visible = false;
                    BtnAllergieCancel.Visible = false;
                    BtnAllergieNew.Visible = false;
                    BtnAllergieEdit.Visible = false;
                    BtnAllergieExit.Visible = true;
                    BtnAllergieDelete.Visible = false;
                    BtnExport.Visible = false;
                    BtnImport.Visible = false;
                    TreeViewAllergie.Visible = false;
                    TabControlAllergie.Location = new Point(12, 12);
                    TabControlAllergieCategory.Location = new Point(12, 12);
                    BtnAllergieSave.Location = new Point(TabControlAllergie.Width - 164, TabControlAllergie.Height + 15);
                    BtnAllergieExit.Location = new Point(TabControlAllergie.Width - 74, TabControlAllergie.Height + 15);
                    this.Size = new Size(TabControlAllergie.Right + 25, TabControlAllergie.Bottom + 90);
                    this.CenterToParent();
                    if (Id == null)
                    {
                        this.Text = string.Format(CreateAllergieOnloadText, "Allergie");
                        newAllergieToolStripMenuItem.PerformClick();
                    }
                    else
                    {
                        this.Text = UpdateAllergieOnloadText;
                        PointSaveorUpdatedNode(TreeViewAllergie.Nodes, (Id + "@"));
                        BtnAllergieEdit_Click(this, null);
                    }
                }
                this.formIsDirty = false;

            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private Allergie GetAllergieInfo()
        {
            if (TreeViewAllergie.SelectedNode != null)
            {
                Allergie lAllergie = AllergieManager.GetAllergiesById(long.Parse(TreeViewAllergie.SelectedNode.Name.Trim('@')));
                if (lAllergie != null)
                {
                    Allergie AllergieFromDB = AllergieManager.GetAllergiesById(lAllergie.Id);
                    return AllergieFromDB;
                }
            }
            return null;
        }

        private void ReSequence()
        {
            for (int i = 0; i < GridViewKeywords.Rows.Count; i++)
            {
                GridViewKeywords.Rows[i].Cells[(int)AllergiesKeywordsTableColumn.SNO].Value = i + 1;
            }
        }

        private Allergie GetAllergieFromForm()
        {
            Allergie lAllergie = new Allergie();
            if (!string.IsNullOrEmpty(TextBoxAllergieId.Text))
            {
                lAllergie.Id = Convert.ToInt64(TextBoxAllergieId.Text);
            }
            else
            {
                lAllergie.Id = 0L;
            }
            IList<Allergie> llAllergie = AllergieManager.ListAllergieByCompanyId(Global.Company.CompanyId);
            lAllergie.Codes = (llAllergie == null || llAllergie.Count == 0 ? 1 : (llAllergie.OrderByDescending(x => x.Id).First().Id + 1)).ToString();
            lAllergie.Name = TextBoxAllergieName.Text.Trim();
            lAllergie.Discription = TextBoxAllergieDescription.Text;
            lAllergie.DisplayAs = TextBoxAllergieDisplayas.Text.Trim();
            if (ComboBoxAllergieCategory.SelectedIndex > -1)
            {
                AllergieCategory AllergieCategory = (AllergieCategory)ComboBoxAllergieCategory.Items[ComboBoxAllergieCategory.SelectedIndex];
                if (AllergieCategory != null)
                {
                    AllergieCategory = AllergieManager.GetAllergieCategoryInfoById(AllergieCategory.Id);
                    if (AllergieCategory != null)
                    {
                        lAllergie.AllergieCategoryId = AllergieCategory.Id;
                    }
                }
            }
            lAllergie.IsActive = CheckBoxAllergieIsActive.Checked;
            lAllergie.ReasonForInactive = TextBoxAllergieReasonInactive.Text.Trim();
            lAllergie.CompanyId = Global.Company.CompanyId;
            lAllergie.Keywords = ListAllergieKeyword();

            return lAllergie;
        }
        private IList<AllergieKeyword> ListAllergieKeyword()
        {
            IList<AllergieKeyword> AllergiesKeyword = new List<AllergieKeyword>();
            int Count = GridViewKeywords.Rows.Count;
            if (Count > 1)
            {
                AllergiesKeyword = new List<AllergieKeyword>();
                for (int i = 0; i < GridViewKeywords.Rows.Count - 1; i++)
                {
                    if (GridViewKeywords.Rows[i].Cells[(int)AllergiesKeywordsTableColumn.NAME].Value.ToString().Trim() != "")
                    {
                        AllergieKeyword lAllergiesKeyword = new AllergieKeyword();
                        lAllergiesKeyword.CompanyId = Global.Company.CompanyId;
                        lAllergiesKeyword.Text = GridViewKeywords.Rows[i].Cells[(int)AllergiesKeywordsTableColumn.NAME].Value.ToString().Trim();

                        AllergiesKeyword.Add(lAllergiesKeyword);
                    }
                }
            }
            return AllergiesKeyword;
        }

        private void BtnAllergieDelete_Click(object sender, EventArgs e)
        {
            if (TabControlAllergie.Visible == true)
            {
                Allergie AllergieInfo = GetAllergieInfo();
                AllergieErrorMsg.Text = "";
                if (AllergieInfo != null)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete the allergie " + AllergieInfo.Name + "?", "Delete Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        if (AllergieManager.DeleteAllergie(AllergieInfo.Id))
                        {
                            ResetForm();
                            TabControlAllergie.SelectedTab = TabPageAllergieDetails;
                            loadCombo();
                            LoadAllergieByCategoryWithFilter();
                            TextBoxAllergieSearch.Clear();
                            EnableForm(false);
                            SetFocus();
                            AllergieErrorMsg.Text = string.Format(AllergieRemoveSuccess, "Allergie", AllergieInfo.Name);
                            this.formIsDirty = false;
                        }
                        else
                        {
                            AllergieErrorMsg.Text = string.Format(AllergieDeleteNotAllowErrorText, "Allergie");
                        }
                        this.formIsDirty = false;
                    }
                }
                else
                {
                    DisplaySystemError(SelectedAllergieNotValidErrorMsg);
                    return;
                }
            }
            else if (TabControlAllergieCategory.Visible == true)
            {
                AllergieCategory AllergieCategoryinfo = GetAllergieCategoryInfo();
                AllergieErrorMsg.Text = "";
                if (AllergieCategoryinfo != null)
                {
                    DialogResult Result = MessageBox.Show(string.Format(DeleteAllergieConfirmText, AllergieCategoryinfo.Name), "Delete Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        if (AllergieManager.DeleteAllergieCategory(AllergieCategoryinfo.Id))
                        {
                            ResetForm();
                            TabControlAllergieCategory.SelectedTab = TabPageAllergieCategory;
                            loadCombo();
                            LoadAllergieByCategoryWithFilter();
                            TextBoxAllergieSearch.Clear();
                            EnableForm(false);
                            SetFocus();
                            AllergieErrorMsg.Text = string.Format(AllergieRemoveSuccess, "allergieCategory", AllergieCategoryinfo.Name);
                            this.formIsDirty = false;
                        }
                        else
                        {
                            AllergieErrorMsg.Text = string.Format(DeleteNotAllowErrorText, "allergieCategory");
                        }
                    }

                }
                else
                {
                    DisplaySystemError(SelectedAllergieNotValidErrorMsg);
                    return;
                }
            }
        }

        private void BtnAllergieCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show("There are unsaved changes, Do you want cancel?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.No)
                {
                    FocusAllergieCategory();
                    return;
                }
            }
            ResetForm();
            loadCombo();
            if (string.IsNullOrEmpty(TextBoxAllergieSearch.Text))
            {
                LoadAllergieByCategoryWithFilter();
            }
            else
            {
                TextBoxAllergieSearch.Clear();
            }
            TextBoxAllergieSearch.Select();
            EnableForm(false);
            SetFocus();
            this.formIsDirty = false;
        }

        private void BtnAllergieSave_Click(object sender, EventArgs e)
        {
            if (TabControlAllergieCategory.Visible && ValidateFormAllergieCategory())
            {
                AllergieCategory AllergieCategoryInfo = GetAllergieCategoryFromForm();
                if (AllergieManager.AllergieCategoryNameUniqueById(AllergieCategoryInfo))
                {
                    AllergieCategory lAllergieCategoryFromDB = null;
                    if (AllergieCategoryInfo.Id == 0)
                    {
                        lAllergieCategoryFromDB = AllergieManager.AddAllergieCategory(AllergieCategoryInfo);
                    }
                    else
                    {
                        AllergieCategory lAllergieCategoryById = AllergieManager.GetAllergieCategoryInfoById(AllergieCategoryInfo.Id);
                        if (lAllergieCategoryById != null)
                        {
                            lAllergieCategoryFromDB = AllergieManager.UpdateAllergieCategory(AllergieCategoryInfo);
                        }
                        else
                        {
                            DisplaySystemError(SelectedAllergieCategoryNotValidErrorMsg);
                            return;
                        }
                    }
                    if (CreateAllergieOnLoad)
                    {
                        FormAllergie_Load(sender, e);
                        return;
                    }
                    ResetForm();
                    TabControlAllergieCategory.SelectedTab = TabPageAllergieCategory;
                    loadCombo();
                    if (string.IsNullOrEmpty(TextBoxAllergieSearch.Text))
                    {
                        LoadAllergieByCategoryWithFilter();
                    }
                    else
                    {
                        TextBoxAllergieSearch.Clear();
                    }
                    PointSaveorUpdatedNode(TreeViewAllergie.Nodes, lAllergieCategoryFromDB.Id.ToString());
                    AllergieErrorMsg.Text = SaveSuccessText;
                    EnableForm(false);
                    this.formIsDirty = false;
                }
                else
                {
                    AllergieErrorMsg.Text = string.Format(UniqueNameErrorMsg, "Allergie Category", AllergieCategoryInfo.Name);
                    TextBoxAllergieCategoryName.Select();
                }
            }
            else if (TabControlAllergie.Visible && validateForm())
            {
                Allergie lAllergie = GetAllergieFromForm();
                if (AllergieManager.AllergieNameUniqueById(lAllergie))
                {
                    Allergie AllergieFromDB = null;
                    if (lAllergie.Id == 0)
                    {
                        AllergieFromDB = AllergieManager.AddAllergie(lAllergie);
                    }
                    else
                    {
                        Allergie lAllergieById = AllergieManager.GetAllergiesById(lAllergie.Id);
                        if (lAllergieById != null)
                        {
                            AllergieFromDB = AllergieManager.UpdateAllergie(lAllergie);
                        }
                        else
                        {
                            DisplaySystemError(SelectedAllergieNotValidErrorMsg);
                            return;
                        }
                    }
                    if (CreateAllergieOnLoad)
                    {
                        this.formIsDirty = false;
                        this.Close();
                    }
                    ResetForm();
                    TabControlAllergie.SelectedTab = TabPageAllergieDetails;
                    loadCombo();
                    if (string.IsNullOrEmpty(TextBoxAllergieSearch.Text))
                    {
                        LoadAllergieByCategoryWithFilter();
                    }
                    else
                    {
                        TextBoxAllergieSearch.Clear();
                    }
                    PointSaveorUpdatedNode(TreeViewAllergie.Nodes, AllergieFromDB.Id.ToString() + "@");
                    AllergieErrorMsg.Text = SaveSuccessText;
                    EnableForm(false);
                    this.formIsDirty = false;
                }
                else
                {
                    AllergieErrorMsg.Text = string.Format(UniqueNameErrorMsg, "Allergie", lAllergie.Name);
                    TextBoxAllergieName.Select();
                }
            }
        }
        private void BtnAllergieEdit_Click(object sender, EventArgs e)
        {
            LoadAllergieCategoryInfo();
            EnableForm(true);
            if (TabControlAllergieCategory.Visible)
            {
                if (ComboBoxParentAllergieCategory.SelectedIndex < 0)
                {
                    ComboBoxParentAllergieCategory.Visible = false;
                }
                TabControlAllergieCategory.SelectedTab = TabPageAllergieCategory;
                TextBoxAllergieCategoryName.Select();
            }
            else if (TabControlAllergie.Visible)
            {
                TabControlAllergie.SelectedTab = TabPageAllergieDetails;
                TextBoxAllergieName.Select();
                TextBoxAllergieReasonInactive.ReadOnly = CheckBoxAllergieIsActive.Checked;
            }
            this.formIsDirty = false;
        }
        private Boolean validateForm()
        {
            AllergieErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxAllergieName.Text.Trim()))
            {
                AllergieErrorMsg.Text = EnterNameErrorMsg;
                TextBoxAllergieName.Select();
                return false;
            }
            if (ComboBoxAllergieCategory.SelectedIndex < 0)
            {
                AllergieErrorMsg.Text = ChooseAllergieCategoryErrorMsg;
                TabControlAllergie.SelectedTab = TabPageAllergieDetails;
                ComboBoxAllergieCategory.Select();
                return false;
            }
            if (!CheckBoxAllergieIsActive.Checked && string.IsNullOrEmpty(TextBoxAllergieReasonInactive.Text.Trim()))
            {
                AllergieErrorMsg.Text = ActivReasonErrorMsg;
                TextBoxAllergieReasonInactive.Select();
                return false;
            }
            if (GridViewKeywords.Rows.Count > 1)
            {
                for (int i = 0; i < GridViewKeywords.Rows.Count - 1; i++)
                {
                    if (GridViewKeywords.Rows[i].Cells[(int)AllergiesKeywordsTableColumn.NAME].Value == null)
                    {
                        AllergieErrorMsg.Text = string.Format(EnterKeyWordErrorMsg, GridViewKeywords.Columns[(int)AllergiesKeywordsTableColumn.NAME].HeaderText);
                        GridViewKeywords.Select();
                        GridViewKeywords.CurrentCell = GridViewKeywords[(int)AllergiesKeywordsTableColumn.NAME, i];
                        return false;
                    }

                    int Countings = 0;
                    for (int k = 0; k < GridViewKeywords.Rows.Count - 1; k++)
                    {
                        if (GridViewKeywords.Rows[k].Cells[(int)AllergiesKeywordsTableColumn.NAME].Value != null)
                        {
                            if (GridViewKeywords.Rows[i].Cells[(int)AllergiesKeywordsTableColumn.NAME].Value.ToString() == GridViewKeywords.Rows[k].Cells[(int)AllergiesKeywordsTableColumn.NAME].Value.ToString())
                            {
                                Countings++;
                            }
                        }
                        if (Countings > 1 && GridViewKeywords.Rows[k].Cells[(int)AllergiesKeywordsTableColumn.NAME].Value.ToString().Trim() != "")
                        {
                            AllergieErrorMsg.Text = string.Format(EnterKeywordNameExistsErrorMsg, GridViewKeywords.Rows[k].Cells[(int)AllergiesKeywordsTableColumn.NAME].Value.ToString());
                            GridViewKeywords.Select();
                            GridViewKeywords.CurrentCell = GridViewKeywords[(int)AllergiesKeywordsTableColumn.NAME, k];
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private void BtnAllergieExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ResetForm()
        {
            AllergieErrorMsg.Text = "";

            TextBoxAllergieCategoryId.ResetText();
            TextBoxAllergieCategoryDescription.ResetText();
            TextBoxAllergieCategoryDisplayAs.ResetText();
            TextBoxAllergieCategoryName.ResetText();

            TextBoxAllergieId.ResetText();
            TextBoxAllergieName.ResetText();
            TextBoxAllergieDisplayas.ResetText();
            TextBoxAllergieDescription.ResetText();
            CheckBoxAllergieIsActive.Checked = false;
            TextBoxAllergieReasonInactive.ResetText();
            ComboBoxAllergieCategory.ResetText();
            ComboBoxAllergieCategory.SelectedIndex = -1;
            ComboBoxParentAllergieCategory.SelectedIndex = -1;

            GridViewKeywords.Rows.Clear();
        }

        private void EnableForm(Boolean enable)
        {
            if (AllergieManager.ListAllergieCategoryByCompanyId(Global.Company.CompanyId).Count > 0)
            {
                TreeViewAllergie.Enabled = !enable;
                TextBoxAllergieSearch.ReadOnly = enable;
                TextBoxAllergieSearch.TabStop = !enable;
            }
            else
            {
                TreeViewAllergie.Enabled = false;
                TabControlAllergieCategory.Visible = true;
                TabControlAllergie.Visible = false;
                TextBoxAllergieSearch.ReadOnly = true;
                TextBoxAllergieSearch.TabStop = false;
            }
            TextBoxAllergieCategoryDescription.ReadOnly = !enable;
            TextBoxAllergieCategoryDisplayAs.ReadOnly = !enable;
            TextBoxAllergieCategoryName.ReadOnly = !enable;

            TextBoxAllergieCategoryDescription.TabStop = enable;
            TextBoxAllergieCategoryDisplayAs.TabStop = enable;
            TextBoxAllergieCategoryName.TabStop = enable;

            ComboBoxAllergieCategory.Visible = enable;
            ComboBoxParentAllergieCategory.Visible = enable;
            ComboBoxParentAllergieCategory.TabStop = enable;

            TextBoxAllergieName.ReadOnly = !enable;
            TextBoxAllergieName.TabStop = enable;
            TextBoxAllergieDisplayas.ReadOnly = !enable;
            TextBoxAllergieDisplayas.TabStop = enable;
            ComboBoxAllergieCategory.Visible = enable;
            ComboBoxAllergieCategory.TabStop = enable;
            TextBoxAllergieDescription.ReadOnly = !enable;
            TextBoxAllergieDescription.TabStop = enable;
            CheckBoxAllergieIsActive.Enabled = enable;
            CheckBoxAllergieIsActive.TabStop = enable;
            GridViewKeywords.Enabled = enable;

            if (!enable)
            {
                TextBoxAllergieReasonInactive.ReadOnly = !enable;
                GridViewKeywords.Enabled = enable;
                BtnAllergieCancel.Enabled = enable;
                if (TreeViewAllergie.SelectedNode == null)
                {
                    BtnAllergieDelete.Enabled = enable;
                    BtnAllergieEdit.Enabled = enable;
                }
                else
                {
                    BtnAllergieDelete.Enabled = !enable;
                    BtnAllergieEdit.Enabled = !enable;
                }
                BtnAllergieNew.Enabled = !enable;
                BtnAllergieSave.Enabled = enable;
            }
            else
            {
                BtnAllergieCancel.Enabled = enable;
                BtnAllergieDelete.Enabled = !enable;
                BtnAllergieEdit.Enabled = !enable;
                BtnAllergieNew.Enabled = !enable;
                BtnAllergieSave.Enabled = enable;
                GridViewKeywords.Enabled = enable;
            }
        }

        private void CheckBoxAllergieIsActive_CheckedChanged(object sender, EventArgs e)
        {
            TextBoxAllergieReasonInactive.ReadOnly = CheckBoxAllergieIsActive.Checked;
            TextBoxAllergieReasonInactive.TabStop = !CheckBoxAllergieIsActive.Checked;
            if (CheckBoxAllergieIsActive.Checked)
            {
                TextBoxAllergieReasonInactive.TabStop = false;
                TextBoxAllergieReasonInactive.ResetText();
            }
        }

        private void TextBoxAllergieName_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnAllergieNew.ShowDropDown();
            }
            if (keyData == (Keys.F4))
            {
                BtnAllergieDelete.PerformClick();
            }
            if (keyData == (Keys.F7))
            {
                BtnAllergieEdit.PerformClick();
            }
            if (keyData == (Keys.F8))
            {
                BtnAllergieSave.PerformClick();
            }
            if (keyData == (Keys.F10))
            {
                BtnAllergieExit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnAllergieCancel.PerformClick();
                return true;
            }

            try
            {
                if (GridViewKeywords.CurrentCell != null && GridViewKeywords.CurrentCell.Selected)
                {
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewKeywords.CurrentCell.ColumnIndex == (int)AllergiesKeywordsTableColumn.NAME && GridViewKeywords.CurrentRow.Index != 0)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewKeywords.CurrentCell.ColumnIndex == (int)AllergiesKeywordsTableColumn.NAME && GridViewKeywords.CurrentRow.Index == 0)
                    {
                        SendKeys.Send("{tab}");
                    }
                    if (keyData == (Keys.Tab) && GridViewKeywords.CurrentCell.ColumnIndex == (int)AllergiesKeywordsTableColumn.NAME && (GridViewKeywords.Rows.Count - 1) != GridViewKeywords.CurrentCell.RowIndex)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                    if (keyData == (Keys.Tab) && GridViewKeywords.CurrentCell.ColumnIndex == (int)AllergiesKeywordsTableColumn.NAME && (GridViewKeywords.Rows.Count - 1) == GridViewKeywords.CurrentCell.RowIndex)
                    {
                        GridViewKeywords.CurrentCell = null;
                        BtnAllergieSave.Select();
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void TreeViewAllergie_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            node.SelectedImageIndex = node.ImageIndex;
            ResetForm();
            loadCombo();
            LoadAllergieCategoryInfo();
            EnableForm(false);
            this.formIsDirty = false;
        }

        private void GridViewKeywords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == (int)AllergiesKeywordsTableColumn.REMOVE && GridViewKeywords.Rows.Count - 1 != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete " + (e.RowIndex + 1) + "?", "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        GridViewKeywords.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewKeywords.Rows.RemoveAt(e.RowIndex);
                        ReSequence();
                    }
                }
            }
        }

        private void TextBoxAllergieName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxAllergieDisplayas.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnAllergieSave.Select();
            }
        }

        private void BtnAllergieSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                if (TabControlAllergieCategory.Visible)
                {
                    TextBoxAllergieCategoryName.Select();
                }
                else
                {
                    TabControlAllergie.SelectedTab = TabPageAllergieDetails;
                    TextBoxAllergieName.Select();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (TabControlAllergieCategory.Visible)
                {
                    if (ComboBoxParentAllergieCategory.Visible)
                    { ComboBoxParentAllergieCategory.Select(); }
                    else
                    { TextBoxAllergieDescription.Select(); }
                }
                else
                {
                    e.IsInputKey = true;
                    GridViewKeywords.Select();
                    GridViewKeywords.CurrentCell = GridViewKeywords[1, 0];
                }
            }
        }

        private void GridViewKeywords_Enter(object sender, EventArgs e)
        {
            GridViewKeywords.CurrentCell = GridViewKeywords[1, 0];
        }

        private void FormAllergie_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show("There are unsaved changes, Do you want Exit?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.No)
                {
                    TextBoxAllergieName.Select();
                    e.Cancel = true;
                }
            }
        }

        private void GridViewKeywords_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            ReSequence();
        }

        private void TextBoxAllergieSearch_TextChanged(object sender, EventArgs e)
        {
            ResetForm();
            LoadAllergieByCategoryWithFilter();
            if (TreeViewAllergie.Nodes.Count > 0) { BtnAllergieEdit.Enabled = true; BtnAllergieDelete.Enabled = true; TreeViewAllergie.TabStop = true; }
            else { BtnAllergieEdit.Enabled = false; BtnAllergieDelete.Enabled = false; TreeViewAllergie.TabStop = false; }
            this.formIsDirty = false;
        }

        private void TextBoxAllergieSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                TreeViewAllergie.Select();
            }
        }

        private void GridViewKeywords_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            //AllergieExportFile AllergieExportFiles = new AllergieExportFile();
            //AllergieExportFiles.GenerateFile(Global.Company.CompanyId);
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            AllergieErrorMsg.Text = string.Empty;
            //DiagonosisFileUpload FileUploadDialog = new DiagonosisFileUpload();
            //FileUploadDialog.Text = "Data Upload - Diagonosis Master";
            //FileUploadDialog.ShowDialog();
            //if (FileUploadDialog.FileUploadComplete == true)
            //{
            //    Cursor.Current = Cursors.WaitCursor;
            //    ResetForm();
            //    EnableForm(true);
            //    MyFormLoad();
            //    Cursor.Current = Cursors.Default;
            //}
        }

        private void CheckBoxAllergieIsActive_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if ((e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift) && !CheckBoxAllergieIsActive.Checked)
            {
                e.IsInputKey = true;
                TextBoxAllergieReasonInactive.TabStop = true;
                TextBoxAllergieReasonInactive.Select();
            }
        }

        private void BtnAllergiesNew_ItemClickedEvent(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Category")
            {
                BtnNewAllergiesCategory_Click(sender, e);
            }
            else if (e.ClickedItem.Text == "Allergie")
            {
                newCategoryToolStripMenuItem1_Click(sender, e);
            }
        }
        private void BtnNewAllergiesCategory_Click(object sender, EventArgs e)
        {
            TextBoxAllergieSearch.ResetText();
            TabControlAllergieCategory.Visible = true;
            TabControlAllergie.Visible = false;
            NewClear();
            this.formIsDirty = false;
        }
        private void newCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBoxAllergieSearch.ResetText();
            TabControlAllergieCategory.Visible = true;
            TabControlAllergie.Visible = false;
            NewClear();
            if (TreeViewAllergie != null && TreeViewAllergie.SelectedNode != null && !TreeViewAllergie.SelectedNode.Name.Contains('@'))
            {
                ComboBoxParentAllergieCategory.SelectedIndex = ComboBoxParentAllergieCategory.FindStringExact(TreeViewAllergie.SelectedNode.Text);
            }
            this.formIsDirty = false;
        }

        private void newAllergieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AllergieManager.ListAllergieCategoryByCompanyId(Global.Company.CompanyId).Count > 0)
            {
                TabControlAllergie.Visible = true; ;
                TabControlAllergie.SelectedTab = TabPageAllergieDetails;
                TabControlAllergieCategory.Visible = false;
                TextBoxAllergieSearch.ResetText();
                NewClear();
                ResetForm();
                EnableForm(true);
                if (TreeViewAllergie != null && TreeViewAllergie.SelectedNode != null && !TreeViewAllergie.SelectedNode.Name.Contains('@'))
                {
                    ComboBoxAllergieCategory.SelectedIndex = ComboBoxAllergieCategory.FindStringExact(TreeViewAllergie.SelectedNode.Text);
                }
                TextBoxAllergieName.Select();
                CheckBoxAllergieIsActive.Checked = true;
                this.formIsDirty = false;
            }
            else
            {
                newCategoryToolStripMenuItem.PerformClick();
                AllergieErrorMsg.Text = NoCategoryFoundErrorMsg;
            }
            this.formIsDirty = false;
        }

        private void newCategoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (AllergieManager.ListAllergieCategoryByCompanyId(Global.Company.CompanyId).Count > 0)
            {
                TabControlAllergie.Visible = true; ;
                TabControlAllergie.SelectedTab = TabPageAllergieDetails;
                TabControlAllergieCategory.Visible = false;
                TextBoxAllergieSearch.ResetText();
                NewClear();
                ResetForm();
                EnableForm(true);
                TextBoxAllergieName.Select();
                CheckBoxAllergieIsActive.Checked = true;
                this.formIsDirty = false;
            }
            else
            {
                newCategoryToolStripMenuItem.PerformClick();
                AllergieErrorMsg.Text = NoCategoryFoundErrorMsg;
            }
            this.formIsDirty = false;
        }

        private void NewClear()
        {
            ResetForm();
            EnableForm(true);
            loadCombo();
            if (TabControlAllergie.Visible) { TextBoxAllergieName.Select(); } else { TextBoxAllergieCategoryName.Select(); };
        }
        private void loadCombo()
        {
            ComboUtils.InitializeAllergieCategoryCombo(ComboBoxAllergieCategory, Global.Company.CompanyId);
            ComboUtils.InitializeParentAllergieCategoryCombo(ComboBoxParentAllergieCategory, Global.Company.CompanyId);
        }
        private Boolean ValidateFormAllergieCategory()
        {
            AllergieErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxAllergieCategoryName.Text.Trim()))
            {
                AllergieErrorMsg.Text = string.Format(EnterNameErrorMsg, "Allergie Category");
                TextBoxAllergieCategoryName.Select();
                return false;
            }
            if (ComboBoxParentAllergieCategory.SelectedIndex >= 0 && AllergieManager.GetAllergieCategoryInfoById(((AllergieCategory)ComboBoxParentAllergieCategory.Items[ComboBoxParentAllergieCategory.SelectedIndex]).Id) == null)
            {
                AllergieErrorMsg.Text = SelectedParentNotValidErrorMsg;
                ComboBoxParentAllergieCategory.Select();
                return false;
            }
            return true;
        }
        private AllergieCategory GetAllergieCategoryFromForm()
        {
            AllergieCategory lAllergieCategory = new AllergieCategory();
            if (!string.IsNullOrEmpty(TextBoxAllergieCategoryId.Text))
            {
                lAllergieCategory.Id = Convert.ToInt64(TextBoxAllergieCategoryId.Text);
            }
            else
            {
                lAllergieCategory.Id = 0L;
            }
            lAllergieCategory.Name = TextBoxAllergieCategoryName.Text.Trim();
            lAllergieCategory.Discription = TextBoxAllergieCategoryDescription.Text.Trim();
            lAllergieCategory.DisplayAs = TextBoxAllergieCategoryDisplayAs.Text.Trim();
            lAllergieCategory.CompanyId = Global.Company.CompanyId;
            if (ComboBoxParentAllergieCategory.SelectedIndex > -1)
            {
                AllergieCategory AllergieCategory = (AllergieCategory)ComboBoxParentAllergieCategory.Items[ComboBoxParentAllergieCategory.SelectedIndex];
                if (AllergieCategory != null)
                {
                    AllergieCategory = AllergieManager.GetAllergieCategoryInfoById(AllergieCategory.Id);
                    if (AllergieCategory != null)
                    {
                        lAllergieCategory.ParentAllergieCategoryId = AllergieCategory.Id;
                        lAllergieCategory.IsSubAllergieCategory = true;
                    }
                }
            }
            else
            {
                lAllergieCategory.IsSubAllergieCategory = false;
            }
            return lAllergieCategory;
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            this.formIsDirty = false;
            LoadAllergieByCategoryWithFilter();
            return;
        }
        private void LoadAllergieByCategoryWithFilter()
        {
            string FilterString = TextBoxAllergieSearch.Text.Trim();
            TreeViewAllergie.Nodes.Clear();
            IList<AllergieCategory> AllergieCategory = AllergieManager.ListAllergieCategoryByFilterCompanyId(Global.Company.CompanyId, FilterString);
            IList<Allergie> Allergie = AllergieManager.ListFilterAllergieByCompanyId(Global.Company.CompanyId, FilterString);
            if (AllergieCategory.Count > 0)
            {
                foreach (var lAllergieCategory in AllergieCategory.Where(s => s.ParentAllergieCategoryId == null))
                {
                    TreeNode[] TreeNodes = TreeViewAllergie.Nodes.Cast<TreeNode>().Where(s => s.Text == lAllergieCategory.Name).ToArray();
                    if (TreeNodes.Length == 0)
                    {
                        TreeNode treeRoot = new TreeNode();
                        treeRoot.Text = lAllergieCategory.Name;
                        treeRoot.Name = lAllergieCategory.Id.ToString();
                        treeRoot.ImageIndex = 0;
                        TreeViewAllergie.Nodes.Add(treeRoot);
                        GetChildNode(lAllergieCategory.Id, treeRoot, AllergieCategory, Allergie);

                    }
                }
            }
            if (TreeViewAllergie.Nodes.Count > 0)
            {
                if (!string.IsNullOrEmpty(TextBoxAllergieSearch.Text))
                {
                    TreeViewAllergie.ExpandAll();
                }
                TreeViewAllergie.SelectedNode = TreeViewAllergie.Nodes[0];
            }
        }
        private void GetChildNode(long ParentAllergieCategoryId, TreeNode ParentTreeNode, IList<AllergieCategory> AllergieCategory, IList<Allergie> Allergie)
        {
            foreach (var lAllergieCategory in AllergieCategory.Where(x => x.ParentAllergieCategoryId == ParentAllergieCategoryId))
            {
                TreeNode childNode = new TreeNode();
                childNode.Text = lAllergieCategory.Name;
                childNode.Name = lAllergieCategory.Id.ToString();
                childNode.ImageIndex = 0;
                GetChildNode(lAllergieCategory.Id, childNode, AllergieCategory, Allergie);
                ParentTreeNode.Nodes.Add(childNode);
            }
            foreach (var lAllergie in Allergie.Where(x => x.AllergieCategoryId == ParentAllergieCategoryId).ToList())
            {
                TreeNode childNode = new TreeNode();
                childNode.Text = lAllergie.Name;
                childNode.Name = lAllergie.Id.ToString() + "@";
                childNode.ImageIndex = 1;
                ParentTreeNode.Nodes.Add(childNode);
            }
        }
        private void PointSaveorUpdatedNode(TreeNodeCollection nodes, string findtext)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Name.ToString().Trim() == findtext)
                {
                    node.Expand();
                    node.TreeView.SelectedNode = node.NextNode;
                    TreeViewAllergie.SelectedNode = node;
                    node.TreeView.Focus();
                    break;
                }
                PointSaveorUpdatedNode(node.Nodes, findtext);
            }
        }
        private void LoadAllergieCategoryInfo()
        {
            if (TreeViewAllergie.SelectedNode != null)
            {
                if (!TreeViewAllergie.SelectedNode.Name.Contains('@'))
                {
                    TabControlAllergieCategory.Visible = true;
                    TabControlAllergie.Visible = false;
                    AllergieCategory AllergieCategoryDB = GetAllergieCategoryInfo();
                    if (AllergieCategoryDB != null)
                    {
                        TextBoxAllergieCategoryId.Text = AllergieCategoryDB.Id.ToString();
                        TextBoxAllergieCategoryName.Text = AllergieCategoryDB.Name;
                        TextBoxAllergieCategoryDisplayAs.Text = AllergieCategoryDB.DisplayAs;
                        TextBoxAllergieCategoryDescription.Text = AllergieCategoryDB.Discription;
                        if (AllergieCategoryDB.ParentAllergieCategory != null)
                        {
                            ComboBoxParentAllergieCategory.SelectedIndex = ComboBoxParentAllergieCategory.FindStringExact(AllergieCategoryDB.ParentAllergieCategory.Name);
                            ComboBoxParentAllergieCategory.Text = AllergieCategoryDB.Name;
                        }
                        else
                        {
                            ComboBoxParentAllergieCategory.SelectedIndex = -1;
                        }
                    }
                    else
                    {
                        DisplaySystemError(SelectedAllergieCategoryNotValidErrorMsg);
                        return;
                    }
                }
                else
                {
                    TabControlAllergie.Visible = true; ;
                    TabControlAllergie.SelectedTab = TabPageAllergieDetails;
                    TabControlAllergieCategory.Visible = false;
                    Allergie AllergieFromDB = GetAllergiesInfo();
                    if (AllergieFromDB != null)
                    {
                        TextBoxAllergieId.Text = AllergieFromDB.Id.ToString();
                        TextBoxAllergieName.Text = AllergieFromDB.Name;
                        TextBoxAllergieDisplayas.Text = AllergieFromDB.DisplayAs;
                        TextBoxAllergieDescription.Text = AllergieFromDB.Discription;
                        CheckBoxAllergieIsActive.Checked = AllergieFromDB.IsActive;
                        TextBoxAllergieReasonInactive.Text = AllergieFromDB.ReasonForInactive;
                        ComboBoxAllergieCategory.SelectedIndex = AllergieFromDB.AllergieCategory != null ? ComboBoxAllergieCategory.FindStringExact(AllergieFromDB.AllergieCategory.Name) : 0;
                        ComboBoxAllergieCategory.Text = AllergieFromDB.AllergieCategory.Name;
                        if (AllergieFromDB.Keywords.Count > 0)
                        {
                            GridViewKeywords.Rows.Clear();
                            GridViewKeywords.Rows.Add(AllergieFromDB.Keywords.Count);
                            int i = 0;
                            foreach (AllergieKeyword AllergiesKeyword in AllergieFromDB.Keywords)
                            {
                                GridViewKeywords.Rows[i].Cells[(int)AllergiesKeywordsTableColumn.NAME].Value = AllergiesKeyword.Text;
                                GridViewKeywords.Rows[i].Cells[(int)AllergiesKeywordsTableColumn.REMOVE].Value = "X";
                                GridViewKeywords.Rows[i].Cells[(int)AllergiesKeywordsTableColumn.ID].Value = AllergiesKeyword.Id;
                                ReSequence();
                                i++;
                            }
                        }
                    }
                    else
                    {
                        DisplaySystemError(SelectedAllergieNotValidErrorMsg);
                        return;
                    }
                }
            }
        }
        private AllergieCategory GetAllergieCategoryInfo()
        {
            AllergieCategory CostCenterFromDB = AllergieManager.GetAllergieCategoryInfoById(long.Parse(TreeViewAllergie.SelectedNode.Name));
            if (CostCenterFromDB != null)
            {
                return CostCenterFromDB;
            }
            return null;
        }
        private Allergie GetAllergiesInfo()
        {
            Allergie AllergieDB = AllergieManager.GetAllergiesById(long.Parse(TreeViewAllergie.SelectedNode.Name.Trim('@')));
            if (AllergieDB != null)
            {
                return AllergieDB;
            }
            return null;
        }
        private void FocusAllergieCategory()
        {
            if (TabControlAllergie.Visible)
            {
                TabControlAllergie.SelectedTab = TabPageAllergieDetails;
                TextBoxAllergieName.Select();
            }
            else
            {
                TextBoxAllergieCategoryName.Select();
            }
        }
        private void SetFocus()
        {
            TreeViewAllergie.SelectedNode = null;
            if (TreeViewAllergie.Nodes.Count > 0)
            {
                TreeViewAllergie.SelectedNode = TreeViewAllergie.Nodes[0];
                //TextBoxAllergieSearch.Select();
            }
            else
            {
                BtnAllergieNew.Focus();
            }
        }

        private void comboBoxSwapTextBoxParentAllergieCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxParentAllergieCategory.SelectedIndex < 0) { LabelParentAllergieCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8, FontStyle.Regular); }
            else { LabelParentAllergieCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8, FontStyle.Bold); }
        }

        private void comboBoxSwapTextBoxAllergieCategory_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxAllergieCategory.DroppedDown = false;
        }

        private void TreeViewAllergie_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeViewAllergie.SelectedNode = e.Node;
        }

        private void TreeViewAllergie_Enter(object sender, EventArgs e)
        {
            if (TreeViewAllergie.Nodes == null || TreeViewAllergie.Nodes.Count == 0)
            {
                BtnAllergieNew.Select();
            }
        }
    }
}
