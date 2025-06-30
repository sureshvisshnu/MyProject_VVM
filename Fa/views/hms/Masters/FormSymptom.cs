using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using fa.api.Accounting;
using fa.api.utils;
using fa.libraries.utils;
using fa.libraries.Validation;
using fa.model.Employee;
using fa.model.Hms.Master;
using fa.views.hms.masters.upload;
using fa.views.utils.Common;
using Fa.api.Hms;
using Fa.views.utils.Report.Upload;
//using FADataAccessLibrary.Migrations;
using Microsoft.Office.Interop.Excel;
using Syncfusion.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using VisioForge.Core.UI.WPF;
using Point = System.Drawing.Point;
using SymptomCategory = fa.model.Hms.Master.SymptomCategory;

namespace fa.views.hms.Masters
{
    public partial class FormSymptom : FormBase
    {
        public static string DoNotAllowToSaveNamExisteMsg = "Diagnosis {0} already exists";
        public static string ErrorDeleteMsg = "Error deleting the diagnosis!, please retry";
        public static string DoNotAllowToDeleteMsg = "Diagnosis {0} used in somewhere else like Ip visit";
        public static string EnterNameErrorMsg = "Name could not be empty, please enter name";
        public static string EnterCodeErrorMsg = "Symptom Code could not be empty, please enter code";
        public static string EnterKeywordNameExistsErrorMsg = "Keyword {0} already exists";
        public static string EnterKeyWordErrorMsg = "Please enter {0}";
        public static string ActivReasonErrorMsg = "Reason could not be empty, please add reason for inactive";
        public static string FileDnloadErrorMsg = "Finished Downloading";
        public static string NoCategoryFoundErrorMsg = "Before adding diagnosis, Add diagnosis category";
        public static string SelectedParentNotValidErrorMsg = "Somthing went wrong, the selected parent is not valid.";
        public static string SelectedSymptomCategoryNotValidErrorMsg = "Somthing went wrong, the selected diagnosis category is not valid.";
        public static string SelectedSymptomNotValidErrorMsg = "Somthing went wrong, the selected diagnosis is not valid.";
        public static string CreateSymptomOnloadText = "New {0}";
        public static string UpdateSymptomOnloadText = "Update diagnosis";
        public static string UniqueNameErrorMsg = "{0} {1} already exists";
        public static string SaveSuccessText = "Saved success...";
        public static string ChooseSymptomCategoryErrorMsg = "Please choose diagnosis category";
        public static string SymptomRemoveSuccess = "{0} {1} romoved successfully";
        public static string SymptomCatDeleteNotAllowErrorText = "Cannot delete {0} because the symptoms in this category are currently in use.";
        public static string DeleteSymptomConfirmText = "Do you want to delete the Diagnosis {0}?";
        public static string DeleteNotAllowErrorText = "Cannot delete {0} it's used in somewhere else";
        public static string CategoryNotAllowErrorText = "Cannot create a category under another category.";

        SymptomsManager SymptomsManager = null!;
        KeypressValidation KeypressValidation = null!;
        DateValidation DateValidation = null!;
        public bool CreateSymptomOnLoad = false;
        public string? Id;
        public enum SymptomsKeywordsTableColumn
        {
            SNO, NAME, REMOVE, ID
        }
        public FormSymptom()
        {
            InitializeComponent();
            SymptomsManager = SymptomsManager.Instance;
            KeypressValidation = KeypressValidation.Instance;
            DateValidation = DateValidation.Instance;
            excludedObjects = new string[] { "TextBoxSymptomSearch" };
        }
        private void FormSymptom_Load(object sender, EventArgs e)
        {
            MyFormLoad();
        }
        private void MyFormLoad()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                TreeViewSymptom.Nodes.Clear();
                ResetForm();
                EnableForm(false);
                loadCombo();
                LoadSymptomByCategoryWithFilter();
                if (CreateSymptomOnLoad)
                {
                    TextBoxSymptomSearch.Visible = false;
                    BtnSymptomCancel.Visible = false;
                    BtnSymptomNew.Visible = false;
                    BtnSymptomEdit.Visible = false;
                    BtnSymptomExit.Visible = true;
                    BtnSymptomDelete.Visible = false;
                    BtnExport.Visible = false;
                    BtnImport.Visible = false;
                    TreeViewSymptom.Visible = false;
                    TabControlSymptoms.Location = new Point(12, 12);
                    TabControlSymptomCategory.Location = new Point(12, 12);
                    BtnSymptomSave.Location = new Point(TabControlSymptoms.Width - 164, TabControlSymptoms.Height + 15);
                    BtnSymptomExit.Location = new Point(TabControlSymptoms.Width - 74, TabControlSymptoms.Height + 15);
                    this.Size = new Size(TabControlSymptoms.Right + 25, TabControlSymptoms.Bottom + 90);
                    this.CenterToParent();
                    if (Id == null)
                    {
                        this.Text = string.Format(CreateSymptomOnloadText, "Symptom");
                        newSymptomToolStripMenuItem.PerformClick();
                    }
                    else
                    {
                        this.Text = UpdateSymptomOnloadText;
                        PointSaveorUpdatedNode(TreeViewSymptom.Nodes, (Id + "@"));
                        BtnSymptomEdit_Click(this, null!);
                    }
                }
                this.formIsDirty = false;

            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private Symptom GetSymptomInfo()
        {
            if (TreeViewSymptom.SelectedNode != null)
            {
                Symptom lSymptom = SymptomsManager.GetSymptomsById(long.Parse(TreeViewSymptom.SelectedNode.Name.Trim('@')));
                if (lSymptom != null)
                {
                    Symptom SymptomFromDB = SymptomsManager.GetSymptomsById(lSymptom.Id);
                    return SymptomFromDB;
                }
            }
            return null!;
        }

        private void ReSequence()
        {
            for (int i = 0; i < GridViewKeywords.Rows.Count; i++)
            {
                GridViewKeywords.Rows[i].Cells[(int)SymptomsKeywordsTableColumn.SNO].Value = i + 1;
            }
        }

        private Symptom GetSymptomFromForm()
        {
            Symptom lSymptom = new Symptom();
            if (!string.IsNullOrEmpty(TextBoxSymptomId.Text))
            {
                lSymptom.Id = Convert.ToInt64(TextBoxSymptomId.Text);
            }
            else
            {
                lSymptom.Id = 0L;
            }
            IList<Symptom> llSymptom = SymptomsManager.ListSymptomByCompanyId(Global.Company.CompanyId);
            lSymptom.Codes = (llSymptom == null || llSymptom.Count == 0 ? 1 : (llSymptom.OrderByDescending(x => x.Id).First().Id + 1)).ToString();
            lSymptom.Name = TextBoxSymptomName.Text.Trim();
            lSymptom.SymptomCode = TextBoxMedicalSymptomCode.Text.Trim();
            lSymptom.Discription = TextBoxSymptomDescription.Text;
            lSymptom.DisplayAs = TextBoxSymptomDisplayas.Text.Trim();
            if (ComboBoxSymptomCategory.SelectedIndex > -1)
            {
                SymptomCategory symptomCategory = (SymptomCategory)ComboBoxSymptomCategory.Items[ComboBoxSymptomCategory.SelectedIndex];
                if (symptomCategory != null)
                {
                    symptomCategory = SymptomsManager.GetSymptomCategoryInfoById(symptomCategory.Id);
                    if (symptomCategory != null)
                    {
                        lSymptom.SymptomCategoryId = symptomCategory.Id;
                    }
                }
            }
            lSymptom.IsActive = CheckBoxSymptomIsActive.Checked;
            lSymptom.ReasonForInactive = TextBoxSymptomReasonInactive.Text.Trim();
            lSymptom.CompanyId = Global.Company.CompanyId;
            lSymptom.Keywords = ListSymptomKeyword();

            return lSymptom;
        }
        private IList<SymptomKeyword> ListSymptomKeyword()
        {
            IList<SymptomKeyword> SymptomsKeyword = new List<SymptomKeyword>();
            int Count = GridViewKeywords.Rows.Count;
            if (Count > 1)
            {
                SymptomsKeyword = new List<SymptomKeyword>();
                for (int i = 0; i < GridViewKeywords.Rows.Count - 1; i++)
                {
                    if (GridViewKeywords.Rows[i].Cells[(int)SymptomsKeywordsTableColumn.NAME].Value.ToString()!.Trim() != "")
                    {
                        SymptomKeyword lSymptomsKeyword = new SymptomKeyword();
                        lSymptomsKeyword.CompanyId = Global.Company.CompanyId;
                        lSymptomsKeyword.Text = GridViewKeywords.Rows[i].Cells[(int)SymptomsKeywordsTableColumn.NAME].Value.ToString()!.Trim();

                        SymptomsKeyword.Add(lSymptomsKeyword);
                    }
                }
            }
            return SymptomsKeyword;
        }

        private void BtnSymptomDelete_Click(object sender, EventArgs e)
        {
            if (TabControlSymptoms.Visible == true)
            {
                Symptom SymptomInfo = GetSymptomInfo();
                SymptomErrorMsg.Text = "";
                if (SymptomInfo != null)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete the Diagnosis " + SymptomInfo.Name + "?", "Delete Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        if (SymptomsManager.DeleteSymptom(SymptomInfo.Id))
                        {
                            ResetForm();
                            TabControlSymptoms.SelectedTab = TabPageSymptomsDetails;
                            loadCombo();
                            LoadSymptomByCategoryWithFilter();
                            TextBoxSymptomSearch.Clear();
                            EnableForm(false);
                            SetFocus();
                            SymptomErrorMsg.Text = string.Format(SymptomRemoveSuccess, "Symptom", SymptomInfo.Name);
                            this.formIsDirty = false;
                        }
                        else
                        {
                            SymptomErrorMsg.Text = string.Format(DeleteNotAllowErrorText, "Symptom");
                        }
                        this.formIsDirty = false;
                    }
                }
                else
                {
                    DisplaySystemError(SelectedSymptomNotValidErrorMsg);
                    return;
                }
            }
            else if (TabControlSymptomCategory.Visible == true)
            {
                SymptomCategory SymptomCategoryinfo = GetSymptomCategoryInfo();
                SymptomErrorMsg.Text = "";
                if (SymptomCategoryinfo != null)
                {
                    DialogResult Result = MessageBox.Show(string.Format(DeleteSymptomConfirmText, SymptomCategoryinfo.Name), "Delete Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        if (SymptomsManager.DeleteSymptomCategory(SymptomCategoryinfo.Id))
                        {
                            ResetForm();
                            TabControlSymptomCategory.SelectedTab = TabPageSymptomCategory;
                            loadCombo();
                            LoadSymptomByCategoryWithFilter();
                            TextBoxSymptomSearch.Clear();
                            EnableForm(false);
                            SetFocus();
                            SymptomErrorMsg.Text = string.Format(SymptomRemoveSuccess, "Diagnosis Category", SymptomCategoryinfo.Name);
                            this.formIsDirty = false;
                        }
                        else
                        {
                            SymptomErrorMsg.Text = string.Format(SymptomCatDeleteNotAllowErrorText, "Diagnosis Category");
                        }
                    }
                }
                else
                {
                    DisplaySystemError(SelectedSymptomNotValidErrorMsg);
                    return;
                }
            }
        }

        private void BtnSymptomCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show("There are unsaved changes, Do you want cancel?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.No)
                {
                    FocusSymptomCategory();
                    return;
                }
            }
            ResetForm();
            loadCombo();
            if (string.IsNullOrEmpty(TextBoxSymptomSearch.Text))
            {
                LoadSymptomByCategoryWithFilter();
                //LoadSymptomCategoryInfo();
            }
            else
            {
                TextBoxSymptomSearch.Clear();
            }
            TextBoxSymptomSearch.Select();
            EnableForm(false);
            SetFocus();
            this.formIsDirty = false;
        }

        private void BtnSymptomSave_Click(object sender, EventArgs e)
        {
            if (TabControlSymptomCategory.Visible && ValidateFormSymptomCategory())
            {
                SymptomCategory SymptomCategoryInfo = GetSymptomCategoryFromForm();
                if (SymptomsManager.SymptomCategoryNameUniqueById(SymptomCategoryInfo))
                {
                    SymptomCategory lSymptomCategoryFromDB = null!;
                    if (SymptomCategoryInfo.Id == 0)
                    {
                        lSymptomCategoryFromDB = SymptomsManager.AddSymptomCategory(SymptomCategoryInfo);
                    }
                    else
                    {
                        SymptomCategory lSymptomCategoryById = SymptomsManager.GetSymptomCategoryInfoById(SymptomCategoryInfo.Id);
                        if (lSymptomCategoryById != null)
                        {
                            lSymptomCategoryFromDB = SymptomsManager.UpdateSymptomCategory(SymptomCategoryInfo);
                        }
                        else
                        {
                            DisplaySystemError(SelectedSymptomCategoryNotValidErrorMsg);
                            return;
                        }
                    }
                    if (CreateSymptomOnLoad)
                    {
                        FormSymptom_Load(sender, e);
                        return;
                    }
                    ResetForm();
                    TabControlSymptomCategory.SelectedTab = TabPageSymptomCategory;
                    loadCombo();
                    if (string.IsNullOrEmpty(TextBoxSymptomSearch.Text))
                    {
                        LoadSymptomByCategoryWithFilter();
                    }
                    else
                    {
                        TextBoxSymptomSearch.Clear();
                    }
                    PointSaveorUpdatedNode(TreeViewSymptom.Nodes, lSymptomCategoryFromDB.Id.ToString());
                    SymptomErrorMsg.Text = SaveSuccessText;
                    EnableForm(false);
                    this.formIsDirty = false;
                }
                else
                {
                    SymptomErrorMsg.Text = string.Format(UniqueNameErrorMsg, "Symptom Category", SymptomCategoryInfo.Name);
                    TextBoxSymptomCategoryName.Select();
                }
            }
            else if (TabControlSymptoms.Visible && validateForm())
            {
                Symptom lSymptom = GetSymptomFromForm();
                if (SymptomsManager.Instance.SymptomCodeByUnique(lSymptom))
                {
                    if (SymptomsManager.SymptomNameUniqueById(lSymptom))
                    {
                        Symptom SymptomFromDB = null!;
                        if (lSymptom.Id == 0)
                        {
                            SymptomFromDB = SymptomsManager.AddSymptom(lSymptom);
                        }
                        else
                        {
                            Symptom lSymptomById = SymptomsManager.GetSymptomsById(lSymptom.Id);
                            if (lSymptomById != null)
                            {
                                SymptomFromDB = SymptomsManager.UpdateSymptom(lSymptom);
                            }
                            else
                            {
                                DisplaySystemError(SelectedSymptomNotValidErrorMsg);
                                return;
                            }
                        }
                        if (CreateSymptomOnLoad)
                        {
                            this.formIsDirty = false;
                            this.Close();
                        }
                        ResetForm();
                        TabControlSymptoms.SelectedTab = TabPageSymptomsDetails;
                        loadCombo();
                        if (string.IsNullOrEmpty(TextBoxSymptomSearch.Text))
                        {
                            LoadSymptomByCategoryWithFilter();
                        }
                        else
                        {
                            TextBoxSymptomSearch.Clear();
                        }
                        PointSaveorUpdatedNode(TreeViewSymptom.Nodes, SymptomFromDB.Id.ToString() + "@");
                        SymptomErrorMsg.Text = SaveSuccessText;
                        EnableForm(false);
                        this.formIsDirty = false;
                    }
                    else
                    {
                        SymptomErrorMsg.Text = string.Format(UniqueNameErrorMsg, "Symptom", lSymptom.Name);
                        TextBoxSymptomName.Select();
                    }
                }
                else
                {
                    SymptomErrorMsg.Text = string.Format(UniqueNameErrorMsg, "Symptom Code", lSymptom.SymptomCode);
                    TextBoxMedicalSymptomCode.Select();
                }
            }
        }
        private void BtnSymptomEdit_Click(object sender, EventArgs e)
        {
            LoadSymptomCategoryInfo();
            EnableForm(true);

            if (TabControlSymptomCategory.Visible)
            {
                if (ComboBoxParentSymptomCategory.SelectedIndex < 0)
                {
                    ComboBoxParentSymptomCategory.Visible = false;
                }
                TabControlSymptomCategory.SelectedTab = TabPageSymptomCategory;
                TextBoxSymptomCategoryName.Select();
            }
            else if (TabControlSymptoms.Visible)
            {

                TabControlSymptoms.SelectedTab = TabPageSymptomsDetails;

                ComboBoxSymptomCategory.SelectionLength = 0;
                ComboBoxSymptomCategory.TabStop = false;
                ComboBoxSymptomCategory.Enabled = false;
                ComboBoxSymptomCategory.Enabled = true;

                TextBoxSymptomName.Select();
                TextBoxSymptomReasonInactive.ReadOnly = CheckBoxSymptomIsActive.Checked;
            }
            this.formIsDirty = false;
        }
        private Boolean validateForm()
        {
            SymptomErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxSymptomName.Text.Trim()))
            {
                SymptomErrorMsg.Text = EnterNameErrorMsg;
                TextBoxSymptomName.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxMedicalSymptomCode.Text.Trim()))
            {
                SymptomErrorMsg.Text = EnterCodeErrorMsg;
                TextBoxMedicalSymptomCode.Select();
                return false;
            }
            if (ComboBoxSymptomCategory.SelectedIndex < 0)
            {
                SymptomErrorMsg.Text = ChooseSymptomCategoryErrorMsg;
                TabControlSymptoms.SelectedTab = TabPageSymptomsDetails;
                ComboBoxSymptomCategory.Select();
                return false;
            }
            if (!CheckBoxSymptomIsActive.Checked && string.IsNullOrEmpty(TextBoxSymptomReasonInactive.Text.Trim()))
            {
                SymptomErrorMsg.Text = ActivReasonErrorMsg;
                TextBoxSymptomReasonInactive.Select();
                return false;
            }
            if (GridViewKeywords.Rows.Count > 1)
            {
                for (int i = 0; i < GridViewKeywords.Rows.Count - 1; i++)
                {
                    if (GridViewKeywords.Rows[i].Cells[(int)SymptomsKeywordsTableColumn.NAME].Value == null)
                    {
                        SymptomErrorMsg.Text = string.Format(EnterKeyWordErrorMsg, GridViewKeywords.Columns[(int)SymptomsKeywordsTableColumn.NAME].HeaderText);
                        GridViewKeywords.Select();
                        GridViewKeywords.CurrentCell = GridViewKeywords[(int)SymptomsKeywordsTableColumn.NAME, i];
                        return false;
                    }

                    int Countings = 0;
                    for (int k = 0; k < GridViewKeywords.Rows.Count - 1; k++)
                    {
                        if (GridViewKeywords.Rows[k].Cells[(int)SymptomsKeywordsTableColumn.NAME].Value != null)
                        {
                            if (GridViewKeywords.Rows[i].Cells[(int)SymptomsKeywordsTableColumn.NAME].Value.ToString() == GridViewKeywords.Rows[k].Cells[(int)SymptomsKeywordsTableColumn.NAME].Value.ToString())
                            {
                                Countings++;
                            }
                        }
                        if (Countings > 1 && GridViewKeywords.Rows[k].Cells[(int)SymptomsKeywordsTableColumn.NAME].Value.ToString()!.Trim() != "")
                        {
                            SymptomErrorMsg.Text = string.Format(EnterKeywordNameExistsErrorMsg, GridViewKeywords.Rows[k].Cells[(int)SymptomsKeywordsTableColumn.NAME].Value.ToString());
                            GridViewKeywords.Select();
                            GridViewKeywords.CurrentCell = GridViewKeywords[(int)SymptomsKeywordsTableColumn.NAME, k];
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private void BtnSymptomExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ResetForm()
        {
            SymptomErrorMsg.Text = "";

            TextBoxSymptomCategoryId.ResetText();
            TextBoxSymptomCategoryDescription.ResetText();
            TextBoxSymptomCategoryDisplayAs.ResetText();
            TextBoxSymptomCategoryName.ResetText();
            TextBoxMedicalSymptomCode.ResetText();

            TextBoxSymptomId.ResetText();
            TextBoxSymptomName.ResetText();
            TextBoxSymptomDisplayas.ResetText();
            TextBoxSymptomDescription.ResetText();
            CheckBoxSymptomIsActive.Checked = false;
            TextBoxSymptomReasonInactive.ResetText();
            ComboBoxSymptomCategory.ResetText();
            ComboBoxSymptomCategory.SelectedIndex = -1;
            ComboBoxParentSymptomCategory.SelectedIndex = -1;

            GridViewKeywords.Rows.Clear();
        }

        private void EnableForm(Boolean enable)
        {
            if (SymptomsManager.ListSymptomCategoryByCompanyId(Global.Company.CompanyId).Count > 0)
            {
                TreeViewSymptom.Enabled = !enable;
                TextBoxSymptomSearch.ReadOnly = enable;
                TextBoxSymptomSearch.TabStop = !enable;
            }
            else
            {
                TreeViewSymptom.Enabled = false;
                TabControlSymptomCategory.Visible = true;
                TabControlSymptoms.Visible = false;
                TextBoxSymptomSearch.ReadOnly = true;
                TextBoxSymptomSearch.TabStop = false;
            }
            TextBoxSymptomCategoryDescription.ReadOnly = !enable;
            TextBoxSymptomCategoryDisplayAs.ReadOnly = !enable;
            TextBoxSymptomCategoryName.ReadOnly = !enable;

            TextBoxSymptomCategoryDescription.TabStop = enable;
            TextBoxSymptomCategoryDisplayAs.TabStop = enable;
            TextBoxSymptomCategoryName.TabStop = enable;

            ComboBoxSymptomCategory.Visible = enable;
            ComboBoxParentSymptomCategory.Visible = enable;
            ComboBoxParentSymptomCategory.TabStop = enable;

            TextBoxSymptomName.ReadOnly = !enable;
            TextBoxSymptomName.TabStop = enable;
            TextBoxMedicalSymptomCode.ReadOnly = !enable;
            TextBoxMedicalSymptomCode.TabStop = enable;
            TextBoxSymptomDisplayas.ReadOnly = !enable;
            TextBoxSymptomDisplayas.TabStop = enable;
            ComboBoxSymptomCategory.Visible = enable;
            ComboBoxSymptomCategory.TabStop = enable;
            TextBoxSymptomDescription.ReadOnly = !enable;
            TextBoxSymptomDescription.TabStop = enable;
            CheckBoxSymptomIsActive.Enabled = enable;
            CheckBoxSymptomIsActive.TabStop = enable;
            GridViewKeywords.Enabled = enable;

            if (!enable)
            {
                TextBoxSymptomReasonInactive.ReadOnly = !enable;
                GridViewKeywords.Enabled = enable;
                BtnSymptomCancel.Enabled = enable;
                if (TreeViewSymptom.SelectedNode == null)
                {
                    BtnSymptomDelete.Enabled = enable;
                    BtnSymptomEdit.Enabled = enable;
                }
                else
                {
                    BtnSymptomDelete.Enabled = !enable;
                    BtnSymptomEdit.Enabled = !enable;
                }
                BtnSymptomNew.Enabled = !enable;
                BtnSymptomSave.Enabled = enable;
            }
            else
            {
                BtnSymptomCancel.Enabled = enable;
                BtnSymptomDelete.Enabled = !enable;
                BtnSymptomEdit.Enabled = !enable;
                BtnSymptomNew.Enabled = !enable;
                BtnSymptomSave.Enabled = enable;
                GridViewKeywords.Enabled = enable;
            }
        }

        private void CheckBoxSymptomIsActive_CheckedChanged(object sender, EventArgs e)
        {
            TextBoxSymptomReasonInactive.ReadOnly = CheckBoxSymptomIsActive.Checked;
            TextBoxSymptomReasonInactive.TabStop = !CheckBoxSymptomIsActive.Checked;
            if (CheckBoxSymptomIsActive.Checked)
            {
                TextBoxSymptomReasonInactive.TabStop = false;
                TextBoxSymptomReasonInactive.ResetText();
            }
        }

        private void TextBoxSymptomName_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnSymptomNew.ShowDropDown();
            }
            if (keyData == (Keys.F4))
            {
                BtnSymptomDelete.PerformClick();
            }
            if (keyData == (Keys.F7))
            {
                BtnSymptomEdit.PerformClick();
            }
            if (keyData == (Keys.F8))
            {
                BtnSymptomSave.PerformClick();
            }
            if (keyData == (Keys.F10))
            {
                BtnSymptomExit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnSymptomCancel.PerformClick();
                return true;
            }

            try
            {
                if (GridViewKeywords.CurrentCell != null && GridViewKeywords.CurrentCell.Selected)
                {
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewKeywords.CurrentCell.ColumnIndex == (int)SymptomsKeywordsTableColumn.NAME && GridViewKeywords.CurrentRow.Index != 0)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewKeywords.CurrentCell.ColumnIndex == (int)SymptomsKeywordsTableColumn.NAME && GridViewKeywords.CurrentRow.Index == 0)
                    {
                        SendKeys.Send("{tab}");
                    }
                    if (keyData == (Keys.Tab) && GridViewKeywords.CurrentCell.ColumnIndex == (int)SymptomsKeywordsTableColumn.NAME && (GridViewKeywords.Rows.Count - 1) != GridViewKeywords.CurrentCell.RowIndex)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                    if (keyData == (Keys.Tab) && GridViewKeywords.CurrentCell.ColumnIndex == (int)SymptomsKeywordsTableColumn.NAME && (GridViewKeywords.Rows.Count - 1) == GridViewKeywords.CurrentCell.RowIndex)
                    {
                        GridViewKeywords.CurrentCell = null;
                        BtnSymptomSave.Select();
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void TreeViewSymptom_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node!;
            node.SelectedImageIndex = node.ImageIndex;
            ResetForm();
            loadCombo();
            LoadSymptomCategoryInfo();
            EnableForm(false);
            this.formIsDirty = false;
        }

        private void GridViewKeywords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == (int)SymptomsKeywordsTableColumn.REMOVE && GridViewKeywords.Rows.Count - 1 != e.RowIndex)
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

        private void TextBoxSymptomName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxMedicalSymptomCode.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnSymptomSave.Select();
            }
        }

        private void BtnSymptomSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                if (TabControlSymptomCategory.Visible)
                {
                    TextBoxSymptomCategoryName.Select();
                }
                else
                {
                    TabControlSymptoms.SelectedTab = TabPageSymptomsDetails;
                    TextBoxSymptomName.Select();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (TabControlSymptomCategory.Visible)
                {
                    if (ComboBoxParentSymptomCategory.Visible)
                    { ComboBoxParentSymptomCategory.Select(); }
                    else
                    { TextBoxSymptomDescription.Select(); }
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

        private void FormSymptom_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show("There are unsaved changes, Do you want Exit?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.No)
                {
                    TextBoxSymptomName.Select();
                    e.Cancel = true;
                }
            }
        }

        private void GridViewKeywords_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            ReSequence();
        }

        private void TextBoxSymptomSearch_TextChanged(object sender, EventArgs e)
        {
            ResetForm();
            LoadSymptomByCategoryWithFilter();
            if (TreeViewSymptom.Nodes.Count > 0) { BtnSymptomEdit.Enabled = true; BtnSymptomDelete.Enabled = true; TreeViewSymptom.TabStop = true; }
            else { BtnSymptomEdit.Enabled = false; BtnSymptomDelete.Enabled = false; TreeViewSymptom.TabStop = false; }
            this.formIsDirty = false;
        }

        private void TextBoxSymptomSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                TreeViewSymptom.Select();
            }
        }

        private void GridViewKeywords_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            SymptomExportFile SymptomExportFiles = new SymptomExportFile();
            SymptomExportFiles.GenerateFile(Global.Company.CompanyId);
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            SymptomErrorMsg.Text = string.Empty;
            DiagonosisFileUpload FileUploadDialog = new DiagonosisFileUpload();
            FileUploadDialog.Text = "Data Upload - Diagonosis Master";
            FileUploadDialog.ShowDialog();
            if (FileUploadDialog.FileUploadComplete == true)
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                EnableForm(true);
                MyFormLoad();
                Cursor.Current = Cursors.Default;
            }
        }

        private void CheckBoxSymptomIsActive_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if ((e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift) && !CheckBoxSymptomIsActive.Checked)
            {
                e.IsInputKey = true;
                TextBoxSymptomReasonInactive.TabStop = true;
                TextBoxSymptomReasonInactive.Select();
            }
        }

        private void BtnSymptomsNew_ItemClickedEvent(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Category" || e.ClickedItem.Text == "New Category")
            {
                BtnNewSymptomsCategory_Click(sender, e);
            }
            else if (e.ClickedItem.Text == "Diagnosis")
            {
                newCategoryToolStripMenuItem1_Click(sender, e);
            }
        }

        private void BtnNewSymptomsCategory_Click(object sender, EventArgs e)
        {
            //if (TreeViewSymptom.Nodes.Count > 0)
            //{
            //    MessageBox.Show("A symptom category already exists. You cannot create more than one category.",
            //                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return; 
            //}

            TextBoxSymptomSearch.ResetText();
            TabControlSymptomCategory.Visible = true;
            TabControlSymptoms.Visible = false;
            NewClear();
            this.formIsDirty = false;
        }
        private void newCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBoxSymptomSearch.ResetText();
            TabControlSymptomCategory.Visible = true;
            TabControlSymptoms.Visible = false;
            NewClear();
            if (TreeViewSymptom != null && TreeViewSymptom.SelectedNode != null && !TreeViewSymptom.SelectedNode.Name.Contains('@'))
            {
                ComboBoxParentSymptomCategory.SelectedIndex = ComboBoxParentSymptomCategory.FindStringExact(TreeViewSymptom.SelectedNode.Text);
            }
            this.formIsDirty = false;
        }

        private void newSymptomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SymptomsManager.ListSymptomCategoryByCompanyId(Global.Company.CompanyId).Count > 0)
            {
                TabControlSymptoms.Visible = true; ;
                TabControlSymptoms.SelectedTab = TabPageSymptomsDetails;
                TabControlSymptomCategory.Visible = false;
                TextBoxSymptomSearch.ResetText();
                NewClear();
                ResetForm();
                EnableForm(true);
                if (TreeViewSymptom != null && TreeViewSymptom.SelectedNode != null && !TreeViewSymptom.SelectedNode.Name.Contains('@'))
                {
                    ComboBoxSymptomCategory.SelectedIndex = ComboBoxSymptomCategory.FindStringExact(TreeViewSymptom.SelectedNode.Text);
                }
                TextBoxSymptomName.Select();
                CheckBoxSymptomIsActive.Checked = true;
                this.formIsDirty = false;
            }
            else
            {
                newCategoryToolStripMenuItem.PerformClick();
                SymptomErrorMsg.Text = NoCategoryFoundErrorMsg;
            }
            this.formIsDirty = false;
        }

        private void newCategoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (SymptomsManager.ListSymptomCategoryByCompanyId(Global.Company.CompanyId).Count > 0)
            {
                TabControlSymptoms.Visible = true; ;
                TabControlSymptoms.SelectedTab = TabPageSymptomsDetails;
                TabControlSymptomCategory.Visible = false;
                TextBoxSymptomSearch.ResetText();
                NewClear();
                TextBoxSymptomName.Select();
                CheckBoxSymptomIsActive.Checked = true;
                this.formIsDirty = false;
            }
            else
            {
                newCategoryToolStripMenuItem.PerformClick();
                SymptomErrorMsg.Text = NoCategoryFoundErrorMsg;
            }
            this.formIsDirty = false;
        }

        private void NewClear()
        {
            ResetForm();
            EnableForm(true);
            loadCombo();
            if (TabControlSymptoms.Visible) { TextBoxSymptomName.Select(); } else { TextBoxSymptomCategoryName.Select(); };
        }
        private void loadCombo()
        {
            ComboUtils.InitializeSymptomCategoryCombo(ComboBoxSymptomCategory, Global.Company.CompanyId);
            ComboUtils.InitializeParentSymptomCategoryCombo(ComboBoxParentSymptomCategory, Global.Company.CompanyId);
        }
        private Boolean ValidateFormSymptomCategory()
        {
            SymptomErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxSymptomCategoryName.Text.Trim()))
            {
                SymptomErrorMsg.Text = string.Format(EnterNameErrorMsg, "Symptom Category");
                TextBoxSymptomCategoryName.Select();
                return false;
            }
            if (ComboBoxParentSymptomCategory.SelectedIndex >= 0) //&& SymptomsManager.GetSymptomCategoryInfoById(((SymptomCategory)ComboBoxParentSymptomCategory.Items[ComboBoxParentSymptomCategory.SelectedIndex]).Id) == null)
            {
                SymptomErrorMsg.Text = CategoryNotAllowErrorText;
                ComboBoxParentSymptomCategory.Select();
                return false;
            }
            return true;
        }
        private SymptomCategory GetSymptomCategoryFromForm()
        {
            SymptomCategory lSymptomCategory = new SymptomCategory();
            if (!string.IsNullOrEmpty(TextBoxSymptomCategoryId.Text))
            {
                lSymptomCategory.Id = Convert.ToInt64(TextBoxSymptomCategoryId.Text);
            }
            else
            {
                lSymptomCategory.Id = 0L;
            }
            lSymptomCategory.Name = TextBoxSymptomCategoryName.Text.Trim();
            lSymptomCategory.Discription = TextBoxSymptomCategoryDescription.Text.Trim();
            lSymptomCategory.DisplayAs = TextBoxSymptomCategoryDisplayAs.Text.Trim();
            lSymptomCategory.CompanyId = Global.Company.CompanyId;
            if (ComboBoxParentSymptomCategory.SelectedIndex > -1)
            {
                SymptomCategory SymptomCategory = (SymptomCategory)ComboBoxParentSymptomCategory.Items[ComboBoxParentSymptomCategory.SelectedIndex];
                if (SymptomCategory != null)
                {
                    SymptomCategory = SymptomsManager.GetSymptomCategoryInfoById(SymptomCategory.Id);
                    if (SymptomCategory != null)
                    {
                        lSymptomCategory.ParentSymptomCategoryId = SymptomCategory.Id;
                        lSymptomCategory.IsSubSymptomCategory = true;
                    }
                }
            }
            else
            {
                lSymptomCategory.IsSubSymptomCategory = false;
            }
            return lSymptomCategory;
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            this.formIsDirty = false;
            LoadSymptomByCategoryWithFilter();
            return;
        }

        private void LoadSymptomByCategoryWithFilter()
        {
            string FilterString = TextBoxSymptomSearch.Text.Trim();
            TreeViewSymptom.Nodes.Clear();

            IList<SymptomCategory> SymptomCategory = SymptomsManager.ListSymptomCategoryByFilterCompanyId(Global.Company.CompanyId, FilterString);
            IList<Symptom> Symptom = SymptomsManager.ListFilterSymptomByNameCodeCompanyId(Global.Company.CompanyId, FilterString);

            Dictionary<int, TreeNode> categoryNodes = new Dictionary<int, TreeNode>();
            HashSet<int> addedSymptoms = new HashSet<int>();

            foreach (var lSymptomCategory in SymptomCategory.Where(s => s.ParentSymptomCategoryId == null))
            {
                TreeNode treeRoot = new TreeNode
                {
                    Text = lSymptomCategory.Name,
                    Name = lSymptomCategory.Id.ToString(),
                    ImageIndex = 0
                };

                TreeViewSymptom.Nodes.Add(treeRoot);
                categoryNodes[(int)lSymptomCategory.Id] = treeRoot;

                GetChildNode(lSymptomCategory.Id, treeRoot, SymptomCategory, Symptom, addedSymptoms);
            }

            foreach (var symptom in Symptom)
            {
                if (symptom.SymptomCategory == null || !categoryNodes.ContainsKey((int)symptom.SymptomCategory.Id))
                {
                    SymptomCategory symptomCat = SymptomsManager.GetSymptomCategoryByNameCode(symptom.Name, Global.Company.CompanyId);

                    if (!categoryNodes.ContainsKey((int)symptomCat.Id))
                    {
                        TreeNode parentNode = new TreeNode
                        {
                            Text = symptomCat.Name,
                            Name = symptomCat.Id.ToString(),
                            ImageIndex = 0
                        };

                        TreeViewSymptom.Nodes.Add(parentNode);
                        categoryNodes[(int)symptomCat.Id] = parentNode;

                        GetChildNode(symptomCat.Id, parentNode, SymptomCategory, Symptom, addedSymptoms);
                    }

                    if (!addedSymptoms.Contains((int)symptom.Id))
                    {
                        TreeNode symptomNode = new TreeNode
                        {
                            Text = symptom.Name,
                            Name = symptom.Id.ToString(),
                            ImageIndex = 1
                        };

                        categoryNodes[(int)symptomCat.Id].Nodes.Add(symptomNode);
                        addedSymptoms.Add((int)symptom.Id);
                    }
                }
            }

            if (TreeViewSymptom.Nodes.Count > 0)
            {
                if (!string.IsNullOrEmpty(TextBoxSymptomSearch.Text))
                {
                    TreeViewSymptom.ExpandAll();
                }
                TreeViewSymptom.SelectedNode = TreeViewSymptom.Nodes[0];
            }
        }

        private void GetChildNode(long ParentSymptomCategoryId, TreeNode ParentTreeNode, IList<SymptomCategory> SymptomCategory, IList<Symptom> Symptom, HashSet<int> addedSymptoms)
        {
            foreach (var lSymptomCategory in SymptomCategory.Where(x => x.ParentSymptomCategoryId == ParentSymptomCategoryId))
            {
                TreeNode childNode = new TreeNode
                {
                    Text = lSymptomCategory.Name,
                    Name = lSymptomCategory.Id.ToString(),
                    ImageIndex = 0
                };
                ParentTreeNode.Nodes.Add(childNode);
                GetChildNode(lSymptomCategory.Id, childNode, SymptomCategory, Symptom, addedSymptoms);
            }

            foreach (var lSymptom in Symptom.Where(x => x.SymptomCategoryId == ParentSymptomCategoryId))
            {
                if (!addedSymptoms.Contains((int)lSymptom.Id))
                {
                    TreeNode childNode = new TreeNode
                    {
                        Text = lSymptom.Name,
                        Name = lSymptom.Id.ToString() + "@",
                        ImageIndex = 1
                    };
                    ParentTreeNode.Nodes.Add(childNode);
                    addedSymptoms.Add((int)lSymptom.Id);
                }
            }
        }


        private TreeNode FindOrCreateCategoryNode(SymptomCategory category, Dictionary<int, TreeNode> categoryNodes)
        {
            if (categoryNodes.ContainsKey((int)category.Id))
                return categoryNodes[(int)category.Id];

            TreeNode newNode = new TreeNode
            {
                Text = category.Name,
                Name = category.Id.ToString(),
                ImageIndex = 0
            };

            if (category.ParentSymptomCategoryId == null)
            {
                TreeViewSymptom.Nodes.Add(newNode);
            }
            else
            {
                var parentCategory = SymptomsManager.GetSymptomCategoryInfoById(category.ParentSymptomCategoryId.Value);
                TreeNode parentNode = FindOrCreateCategoryNode(parentCategory, categoryNodes);
                parentNode.Nodes.Add(newNode);
            }

            categoryNodes[(int)category.Id] = newNode;
            return newNode;
        }

        private void PointSaveorUpdatedNode(TreeNodeCollection nodes, string findtext)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Name.ToString().Trim() == findtext)
                {
                    node.Expand();
                    node.TreeView.SelectedNode = node.NextNode;
                    TreeViewSymptom.SelectedNode = node;
                    node.TreeView.Focus();
                    break;
                }
                PointSaveorUpdatedNode(node.Nodes, findtext);
            }
        }
        private void LoadSymptomCategoryInfo()
        {
            if (TreeViewSymptom.SelectedNode != null)
            {
                if (!TreeViewSymptom.SelectedNode.Name.Contains('@'))
                {
                    TabControlSymptomCategory.Visible = true;
                    TabControlSymptoms.Visible = false;
                    SymptomCategory SymptomCategoryDB = GetSymptomCategoryInfo();
                    if (SymptomCategoryDB != null)
                    {
                        TextBoxSymptomCategoryId.Text = SymptomCategoryDB.Id.ToString();
                        TextBoxSymptomCategoryName.Text = SymptomCategoryDB.Name;
                        TextBoxSymptomCategoryDisplayAs.Text = SymptomCategoryDB.DisplayAs;
                        TextBoxSymptomCategoryDescription.Text = SymptomCategoryDB.Discription;
                        if (SymptomCategoryDB.ParentSymptomCategory != null)
                        {
                            ComboBoxParentSymptomCategory.SelectedIndex = ComboBoxParentSymptomCategory.FindStringExact(SymptomCategoryDB.ParentSymptomCategory.Name);
                            ComboBoxParentSymptomCategory.Text = SymptomCategoryDB.Name;
                        }
                        else
                        {
                            ComboBoxParentSymptomCategory.SelectedIndex = -1;
                        }
                    }
                    else
                    {
                        DisplaySystemError(SelectedSymptomCategoryNotValidErrorMsg);
                        return;
                    }
                }
                else
                {
                    TabControlSymptoms.Visible = true; ;
                    TabControlSymptoms.SelectedTab = TabPageSymptomsDetails;
                    TabControlSymptomCategory.Visible = false;
                    Symptom SymptomFromDB = GetSymptomsInfo();
                    if (SymptomFromDB != null)
                    {
                        TextBoxSymptomId.Text = SymptomFromDB.Id.ToString();
                        TextBoxSymptomName.Text = SymptomFromDB.Name;
                        TextBoxMedicalSymptomCode.Text = SymptomFromDB.SymptomCode ?? string.Empty;
                        TextBoxSymptomDisplayas.Text = SymptomFromDB.DisplayAs;
                        TextBoxSymptomDescription.Text = SymptomFromDB.Discription;
                        CheckBoxSymptomIsActive.Checked = SymptomFromDB.IsActive;
                        TextBoxSymptomReasonInactive.Text = SymptomFromDB.ReasonForInactive;
                        ComboBoxSymptomCategory.SelectedIndex = SymptomFromDB.SymptomCategory != null ? ComboBoxSymptomCategory.FindStringExact(SymptomFromDB.SymptomCategory.Name) : 0;
                        ComboBoxSymptomCategory.Text = SymptomFromDB.SymptomCategory!.Name;
                        if (SymptomFromDB.Keywords.Count > 0)
                        {
                            GridViewKeywords.Rows.Clear();
                            GridViewKeywords.Rows.Add(SymptomFromDB.Keywords.Count);
                            int i = 0;
                            foreach (SymptomKeyword SymptomsKeyword in SymptomFromDB.Keywords)
                            {
                                GridViewKeywords.Rows[i].Cells[(int)SymptomsKeywordsTableColumn.NAME].Value = SymptomsKeyword.Text;
                                GridViewKeywords.Rows[i].Cells[(int)SymptomsKeywordsTableColumn.REMOVE].Value = "X";
                                GridViewKeywords.Rows[i].Cells[(int)SymptomsKeywordsTableColumn.ID].Value = SymptomsKeyword.Id;
                                ReSequence();
                                i++;
                            }
                        }
                    }
                    else
                    {
                        DisplaySystemError(SelectedSymptomNotValidErrorMsg);
                        return;
                    }
                }
            }
        }
        private SymptomCategory GetSymptomCategoryInfo()
        {
            SymptomCategory CostCenterFromDB = SymptomsManager.GetSymptomCategoryInfoById(long.Parse(TreeViewSymptom.SelectedNode.Name));
            if (CostCenterFromDB != null)
            {
                return CostCenterFromDB;
            }
            return null!;
        }
        private Symptom GetSymptomsInfo()
        {
            Symptom SymptomDB = SymptomsManager.GetSymptomsById(long.Parse(TreeViewSymptom.SelectedNode.Name.Trim('@')));
            if (SymptomDB != null)
            {
                return SymptomDB;
            }
            return null!;
        }
        private void FocusSymptomCategory()
        {
            if (TabControlSymptoms.Visible)
            {
                TabControlSymptoms.SelectedTab = TabPageSymptomsDetails;
                TextBoxSymptomName.Select();
            }
            else
            {
                TextBoxSymptomCategoryName.Select();
            }
        }
        private void SetFocus()
        {
            TreeViewSymptom.SelectedNode = null;
            if (TreeViewSymptom.Nodes.Count > 0)
            {
                TreeViewSymptom.SelectedNode = TreeViewSymptom.Nodes[0];
            }
            else
            {
                BtnSymptomNew.Focus();
            }
        }

        private void comboBoxSwapTextBoxParentSymptomCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxParentSymptomCategory.SelectedIndex < 0) { LabelParentSymptomCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8, FontStyle.Regular); }
            else { LabelParentSymptomCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8, FontStyle.Bold); }
        }

        private void comboBoxSwapTextBoxSymptomCategory_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxSymptomCategory.DroppedDown = false;
        }

        private void TreeViewSymptom_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeViewSymptom.SelectedNode = e.Node;
        }

        private void TreeViewSymptom_Enter(object sender, EventArgs e)
        {
            if (TreeViewSymptom.Nodes == null || TreeViewSymptom.Nodes.Count == 0)
            {
                BtnSymptomNew.Select();
            }
        }

        private void TextBoxMedicalSymptomCode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxSymptomDisplayas.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxSymptomName.Select();
            }
        }
    }
}
