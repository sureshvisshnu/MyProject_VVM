using fa.api.Hms;
using fa.libraries.Validation;
using fa.model.hms.common;
using fa.model.Hms.Master;
using fa.views.hms.masters.upload;
using Fa.api.Hms;
using fa.libraries.utils;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using fa.api.utils;
using static fa.views.hms.Masters.FormSymptom;
using fa.model.Hms.Ip;
using Fa.views.utils.Report.Upload;
using static CSJ2K.j2k.codestream.HeaderInfo;
using VisioForge.Libs.MediaFoundation.OPM;
using TextBox = System.Windows.Forms.TextBox;
using fa.views.hms.patient;
using System.Data;
using Point = System.Drawing.Point;
using VisioForge.MediaFramework.Helpers;

namespace fa.views.hms.Masters
{
    public enum TestElementsGridColumn
    {
        SNO, ELEMENTCODE, NAME, ELEMENTSHORTNAME, DESCRIPTION, CLASS, SUBCLASS, UOM, RANGE_FROM, RANGE_TO, SINGLEVALUE, RESULTDURATION, FEE, REMOVE, ID
    }
    public enum KeywordsGridColumn
    {
        SNO, NAME, REMOVE, ID
    }
    public partial class FormMedicalTest : FormBase
    {
        public static string EnterNameErrorMsg = "Name could not be empty, Please enter name";
        public static string EnterTestCodeErrorMsg = "Test code could not be empty, Please enter testcode";
        public static string EnterMedicalTestNameExistsErrorMsg = "MedicalTest {0} already exists";
        public static string EnterKeywordNameExistsErrorMsg = "Keyword {0} already exists";
        public static string EnterElementNameExistsErrorMsg = "Element {0} already exists";
        public static string ErrorDeleteMsg = "Error deleting the medicaltest!, Please retry";
        public static string DoNotAllowToDeleteMedicalTestMsg = "Do not remove the medicaltest {0},It is used in some where else";
        public static string DoNotAllowToDeleteElementMsg = "Do not remove the element {0},It is used in some where else";
        public static string EnterElementorKeyWordErrorMsg = "Please enter {0}";
        public static string ActivReasonErrorMsg = "Reason could not be empty, Please add reason for inactive";
        public static string FileDnloadErrorMsg = "Finished downloading";
        public static string FeeErrorMsg = "Fee could not be empty, Please enter Fee";
        public static string NoCategoryFoundErrorMsg = "Before adding medicaltest, Add category first";
        public static string CreateMedicalTestOnloadText = "New {0}";
        public static string UpdateMedicalTestOnloadText = "Update medicaltest";
        public static string SelectedMedicalTestCategoryNotValidErrorMsg = "Somthing went wrong, The selected medicaltest category is not valid.";
        public static string SelectedMedicalTestNotValidErrorMsg = "Somthing went wrong, The selected medicaltest is not valid.";
        public static string SelectedParentNotValidErrorMsg = "Somthing went wrong, The selected parent is not valid.";
        public static string UniqueNameErrorMsg = "{0} {1} already exists";
        public static string ChooseMedicalTestCategoryErrorMsg = "Please choose medicaltest category";
        public static string ChooseMedicalTestSampleErrorMsg = "Please choose medicaltest sample";
        public static string EnterKeyWordErrorMsg = "Please enter {0}";
        public static string SaveSuccessText = "Saved success...";
        public static string MedicalTestRemoveSuccess = "{0} {1} removed successfully";
        public static string MedicalTestDeleteNotAllowErrorText = "Cannot delete medical test {0}. It is used in somewhere else";
        public static string DeleteMedicalTestConfirmText = "Do you want to delete the medicaltest {0}?";
        public static string DeleteNotAllowErrorText = "Cannot delete {0} it is used in somewhere else!";
        public static string EnterElementErrorMsg = "Atleast one element must be present !!";
        public static string SelectUOMErrorMsg = "Please select the UOM..";

        MedicalTestManager MedicalTestManager = null!;
        KeypressValidation KeypressValidation = null!;
        public bool CreateMedicalTestOnLoad = false;
        public string? Id;

        public FormMedicalTest()
        {
            MedicalTestManager = MedicalTestManager.Instance;
            KeypressValidation = KeypressValidation.Instance;
            InitializeComponent();
            excludedObjects = new string[] { "TextBoxMedicalTestSearch" };
            GridViewElementInformation.RowTemplate.Height = 20;
            if (CreateMedicalTestOnLoad)
            {
                int centerX = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
                int centerY = (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2;
                this.Location = new System.Drawing.Point(centerX, centerY);

                int NewcenterX = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
                int NewcenterY = (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2;
                this.Location = new System.Drawing.Point(NewcenterX, NewcenterY);
            }
        }
        private void MedicalTest_Load(object sender, EventArgs e)
        {
            LoadMedicalTest();
        }

        private void LoadMedicalTest()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                ResetForm();
                EnableForm(false);
                loadMedicalTestCombo();
                LoadMedicalTestByCategoryWithFilter();
                if (CreateMedicalTestOnLoad)
                {
                    TextBoxMedicalTestSearch.Visible = false;
                    BtnMedicalTestCancel.Visible = false;
                    BtnMedicalTestNew.Visible = false;
                    BtnMedicalTestEdit.Visible = false;
                    BtnMedicalTestDelete.Visible = false;
                    TreeViewMedicalTest.Visible = false;
                    BtnExport.Visible = false;
                    BtnImport.Visible = false;
                    this.Width = 1025;
                    this.Height = 525;
                    this.Text = "New Medical Test";
                    TabControlMedicaltestCategory.Location = new System.Drawing.Point(13, 12);
                    TabControlMedicalTest.Location = new System.Drawing.Point(13, 12);
                    BtnMedicalTestSave.Location = new System.Drawing.Point(800, 428);
                    BtnMedicalTestExit.Location = new System.Drawing.Point(895, 428);
                    int centerX = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
                    int centerY = (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2;
                    this.Location = new System.Drawing.Point(centerX, centerY);
                    if (Id == null)
                    {
                        this.Text = string.Format(CreateMedicalTestOnloadText, "MedicalTest");
                        NewMedicalTestToolstrip.PerformClick();
                    }
                    else
                    {
                        this.Text = UpdateMedicalTestOnloadText;
                        PointSaveOrUpdatedNode(TreeViewMedicalTest.Nodes, (Id + "@"));
                        BtnMedicalTestEdit_Click(this, null);
                    }
                }
                this.formIsDirty = false;

            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private MedicalTest GetMedicalTestInfo()
        {
            MedicalTest MedicalTestFromDB = MedicalTestManager.GetMedicalTestById(long.Parse(TreeViewMedicalTest.SelectedNode.Name.Trim('@')));
            if (MedicalTestFromDB != null)
            {
                return MedicalTestFromDB;
            }
            return null!;
        }
        private void LoadMedicalTestInfo()
        {
            MedicalTest MedicalTestFromDB = GetMedicalTestInfo();
            if (MedicalTestFromDB != null)
            {
                GridViewElementInformation.Rows.Clear();
                TextBoxMedicalTestId.Text = MedicalTestFromDB.Id.ToString();
                TextBoxMedicalTestName.Text = MedicalTestFromDB.Name;
                TextBoxMedicalTestDisplayName.Text = MedicalTestFromDB.DisplayAs;
                comboBoxSwapTextBoxMedicalTestCategory.SelectedIndex = MedicalTestFromDB.MedicalTestCategory != null ? comboBoxSwapTextBoxMedicalTestCategory.FindStringExact(MedicalTestFromDB.MedicalTestCategory.Name) : 0;
                comboBoxSwapTextBoxMedicalTestCategory.Text = MedicalTestFromDB.MedicalTestCategory!.Name;
                comboBoxSampleRequirement.SelectedIndex = MedicalTestFromDB.SampleRequirement != null ? comboBoxSampleRequirement.FindStringExact(MedicalTestFromDB.SampleRequirement) : -1;
                comboBoxSampleRequirement.Text = MedicalTestFromDB.SampleRequirement;
                TextBoxMedicalTestDescription.Text = MedicalTestFromDB.Description;
                textBoxShortName.Text = MedicalTestFromDB.TestShortName;
                textBoxTestCode.Text = MedicalTestFromDB.TestCode;
                CheckBoxMedicalTestIsActive.Checked = MedicalTestFromDB.IsActive;
                TextBoxMedicalTestReason.Text = MedicalTestFromDB.Reason;
                CheckBoxMedicalTestHasElement.Checked = MedicalTestFromDB.HasElement;
                if (MedicalTestFromDB.TestElements.Count > 0)
                {
                    GridViewElementInformation.Rows.Add(MedicalTestFromDB.TestElements.Count);
                    int i = 0;
                    foreach (MedicalTestElement MedicalTestElement in MedicalTestFromDB.TestElements)
                    {
                        if (MedicalTestElement != null)
                        {
                            LoadUom(i);
                            GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.SNO].Value = i + 1;
                            GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.ELEMENTCODE].Value = MedicalTestElement.ElementCode?.Trim() ?? string.Empty;
                            GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.NAME].Value = MedicalTestElement.Name?.Trim() ?? string.Empty;
                            GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.ELEMENTSHORTNAME].Value = MedicalTestElement.ElementShortName?.Trim() ?? string.Empty;
                            GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.DESCRIPTION].Value = MedicalTestElement.Description?.Trim() ?? string.Empty;
                            GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.CLASS].Value = MedicalTestElement.Class?.Trim() ?? string.Empty;
                            GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.SUBCLASS].Value = MedicalTestElement.SubClass?.Trim() ?? string.Empty;
                            GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.UOM].Value = MedicalTestElement.UomId ?? 0;
                            GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.RANGE_FROM].Value = MedicalTestElement.RangeFrom?.Trim() ?? string.Empty;
                            GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.RANGE_TO].Value = MedicalTestElement.RangeTo?.Trim() ?? string.Empty;
                            GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.SINGLEVALUE].Value = MedicalTestElement.SingleValue?.Trim() ?? string.Empty;
                            GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.RESULTDURATION].Value = MedicalTestElement.ResultDuration?.Trim() ?? string.Empty;
                            GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.ID].Value = MedicalTestElement.Id;
                            i++;
                        }
                    }
                }
                ReSequence();
                if (MedicalTestFromDB.Keywords.Count > 0)
                {
                    GridViewKeywords.Rows.Clear();
                    GridViewKeywords.Rows.Add(MedicalTestFromDB.Keywords.Count);
                    int i = 0;
                    foreach (MedicalTestKeyword MedicalTestKeyword in MedicalTestFromDB.Keywords)
                    {
                        GridViewKeywords.Rows[i].Cells[(int)KeywordsGridColumn.SNO].Value = i + 1;
                        GridViewKeywords.Rows[i].Cells[(int)KeywordsGridColumn.NAME].Value = MedicalTestKeyword.Text.Trim();
                        GridViewKeywords.Rows[i].Cells[(int)KeywordsGridColumn.ID].Value = MedicalTestKeyword.Id;
                        i++;
                    }
                }
                ReSequenceKeyword();
            }
            else
            {
                DisplaySystemError(SelectedMedicalTestNotValidErrorMsg);
                return;
            }
        }
        private void LoadUom(int Index)
        {
            IList<MedicalTestUOM> values = MedicalTestManager.ListAllNonEmptyMedicalTestUOMByCompanyId(Global.Company.CompanyId);
            (GridViewElementInformation.Rows[Index].Cells[(int)TestElementsGridColumn.UOM] as DataGridViewComboBoxCell)!.DataSource = null;
            (GridViewElementInformation.Rows[Index].Cells[(int)TestElementsGridColumn.UOM] as DataGridViewComboBoxCell)!.DataSource = values;
            (GridViewElementInformation.Rows[Index].Cells[(int)TestElementsGridColumn.UOM] as DataGridViewComboBoxCell)!.ValueMember = "ID";
            (GridViewElementInformation.Rows[Index].Cells[(int)TestElementsGridColumn.UOM] as DataGridViewComboBoxCell)!.DisplayMember = "Name";
        }

        private void LoadMedicalTestCategoryInfo()
        {
            if (TreeViewMedicalTest.SelectedNode != null)
            {
                if (!TreeViewMedicalTest.SelectedNode.Name.Contains('@'))
                {
                    TabControlMedicaltestCategory.Visible = true;
                    TabControlMedicalTest.Visible = false;
                    MedicalTestCategory MedicalTestCategoryDB = GetMedicalTestCategoryInfo();
                    if (MedicalTestCategoryDB != null)
                    {
                        TextBoxMedicalTestCategoryId.Text = MedicalTestCategoryDB.Id.ToString();
                        TextBoxMedicaltestCategoryName.Text = MedicalTestCategoryDB.Name;
                        TextBoxMedicaltestCategoryDisplayAs.Text = MedicalTestCategoryDB.DisplayAs;
                        TextBoxMedicaltestCategoryDescription.Text = MedicalTestCategoryDB.Description;
                        if (MedicalTestCategoryDB.ParentMedicalTestCategory != null)
                        {
                            comboBoxSwapTextBoxMedicalTestParentCategory.SelectedIndex = comboBoxSwapTextBoxMedicalTestParentCategory.FindStringExact(MedicalTestCategoryDB.ParentMedicalTestCategory.Name);
                        }
                        else
                        {
                            comboBoxSwapTextBoxMedicalTestParentCategory.SelectedIndex = -1;
                        }
                    }
                    else
                    {
                        DisplaySystemError(SelectedMedicalTestCategoryNotValidErrorMsg);
                        return;
                    }
                }
                else
                {
                    TabControlMedicalTest.Visible = true;
                    TabControlMedicalTest.SelectedTab = TabTestInformation;
                    TabControlMedicaltestCategory.Visible = false;
                    LoadMedicalTestInfo();
                }
            }
        }

        private MedicalTestCategory GetMedicalTestCategoryInfo()
        {
            MedicalTestCategory CostCenterFromDB = MedicalTestManager.GetMedicalTestCategoryInfoById(long.Parse(TreeViewMedicalTest.SelectedNode.Name));
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
            LoadMedicalTestByCategoryWithFilter();
            return;
        }

        private void ReSequence()
        {
            for (int i = 0; i < GridViewElementInformation.Rows.Count; i++)
            {
                GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.SNO].Value = i + 1;
            }
        }
        private void ReSequenceKeyword()
        {
            for (int i = 0; i < GridViewKeywords.Rows.Count; i++)
            {
                GridViewKeywords.Rows[i].Cells[(int)KeywordsGridColumn.SNO].Value = i + 1;
            }
        }
        private MedicalTest GetMedicalTestFromForm()
        {
            MedicalTest lMedicalTest = new MedicalTest();
            if (!string.IsNullOrEmpty(TextBoxMedicalTestId.Text))
            {
                lMedicalTest.Id = Convert.ToInt64(TextBoxMedicalTestId.Text);
            }
            else
            {
                lMedicalTest.Id = 0L;
            }
            IList<MedicalTest> llMedicalTest = MedicalTestManager.ListMedicalTestByCompanyId(Global.Company.CompanyId);
            lMedicalTest.Name = TextBoxMedicalTestName.Text.Trim();
            lMedicalTest.Description = TextBoxMedicalTestDescription.Text;
            lMedicalTest.DisplayAs = TextBoxMedicalTestDisplayName.Text.Trim();
            lMedicalTest.TestCode = textBoxTestCode.Text.Trim();
            lMedicalTest.TestShortName = textBoxShortName.Text.Trim();
            //.................................................................

            lMedicalTest.IsActive = CheckBoxMedicalTestIsActive.Checked;
            lMedicalTest.Reason = TextBoxMedicalTestReason.Text;
            lMedicalTest.HasElement = CheckBoxMedicalTestHasElement.Checked;
            lMedicalTest.CompanyId = Global.Company.CompanyId;
            lMedicalTest.TestElements = new List<MedicalTestElement>();
            if (CheckBoxMedicalTestHasElement.Checked)
            {
                for (int i = 0; i < GridViewElementInformation.Rows.Count - 1; i++)
                {
                    MedicalTestElement MedicalTestElement = new MedicalTestElement();
                    MedicalTestUOM MedicalTestUOM = null!;
                    string Uom = (string)GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.UOM].FormattedValue;
                    MedicalTestUOM MedicalTestUOMName = MedicalTestManager.GetMedicalTestUOMByName(Uom, Global.Company.CompanyId);
                    if (MedicalTestUOMName == null)
                    {
                        MedicalTestUOM = AddUom(Uom);
                    }
                    else
                    {
                        MedicalTestUOM = MedicalTestUOMName;
                    }
                    MedicalTestElement.CompanyId = Global.Company.CompanyId;
                    MedicalTestElement.Id = GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.ID].Value != null ? long.Parse(GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.ID].Value.ToString()!) : 0L;
                    MedicalTestElement.Name = GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.NAME].Value != null ? GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.NAME].Value.ToString() : "";
                    MedicalTestElement.ElementCode = GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.ELEMENTCODE].Value != null ? GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.ELEMENTCODE].Value.ToString() : "";
                    MedicalTestElement.ElementShortName = GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.ELEMENTSHORTNAME].Value != null ? GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.ELEMENTSHORTNAME].Value.ToString() : "";
                    MedicalTestElement.UomId = MedicalTestUOM.Id;
                    MedicalTestElement.RangeFrom = GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.RANGE_FROM].Value != null ? GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.RANGE_FROM].Value.ToString()! : "";
                    MedicalTestElement.RangeTo = GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.RANGE_TO].Value != null ? GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.RANGE_TO].Value.ToString()! : "";
                    MedicalTestElement.Description = GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.DESCRIPTION].Value != null ? GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.DESCRIPTION].Value.ToString()! : "";
                    MedicalTestElement.SingleValue = GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.SINGLEVALUE].Value != null ? GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.SINGLEVALUE].Value.ToString()! : "";
                    MedicalTestElement.Class = GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.CLASS].Value != null ? GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.CLASS].Value.ToString()! : "";
                    MedicalTestElement.SubClass = GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.SUBCLASS].Value != null ? GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.SUBCLASS].Value.ToString()! : "";
                    MedicalTestElement.ResultDuration = GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.RESULTDURATION].Value != null ? GridViewElementInformation.Rows[i].Cells[(int)TestElementsGridColumn.RESULTDURATION].Value.ToString()! : "";
                    lMedicalTest.TestElements.Add(MedicalTestElement);
                }
            }
            //.....................................................................
            if (comboBoxSwapTextBoxMedicalTestCategory.SelectedIndex > -1)
            {
                MedicalTestCategory MedicalTestCategory = (MedicalTestCategory)comboBoxSwapTextBoxMedicalTestCategory.Items[comboBoxSwapTextBoxMedicalTestCategory.SelectedIndex];
                if (MedicalTestCategory != null)
                {
                    MedicalTestCategory = MedicalTestManager.GetMedicalTestCategoryInfoById(MedicalTestCategory.Id);
                    if (MedicalTestCategory != null)
                    {
                        lMedicalTest.MedicalTestCategoryId = MedicalTestCategory.Id;
                    }
                }
            }
            lMedicalTest.SampleRequirement = comboBoxSampleRequirement.Text;
            lMedicalTest.IsActive = CheckBoxMedicalTestIsActive.Checked;
            lMedicalTest.Reason = TextBoxMedicalTestReason.Text.Trim(); ;
            lMedicalTest.CompanyId = Global.Company.CompanyId;
            lMedicalTest.Keywords = ListMedicalTestKeyword();

            return lMedicalTest;
        }

        private IList<MedicalTestKeyword> ListMedicalTestKeyword()
        {
            IList<MedicalTestKeyword> MedicalTestKeyword = new List<MedicalTestKeyword>();
            int Count = GridViewKeywords.Rows.Count;
            if (Count > 1)
            {
                MedicalTestKeyword = new List<MedicalTestKeyword>();
                for (int i = 0; i < GridViewKeywords.Rows.Count - 1; i++)
                {
                    if (GridViewKeywords.Rows[i].Cells[(int)KeywordsGridColumn.NAME].Value.ToString()!.Trim() != "")
                    {
                        MedicalTestKeyword lMedicalTestKeyword = new MedicalTestKeyword();
                        lMedicalTestKeyword.CompanyId = Global.Company.CompanyId;
                        lMedicalTestKeyword.Text = GridViewKeywords.Rows[i].Cells[(int)KeywordsGridColumn.NAME].Value.ToString()!.Trim();

                        MedicalTestKeyword.Add(lMedicalTestKeyword);
                    }
                }
            }
            return MedicalTestKeyword;
        }

        private void TextBoxMedicalTestSearch_TextChanged(object sender, EventArgs e)
        {
            ResetForm();
            string filterString = TextBoxMedicalTestSearch.Text.Trim();
            LoadMedicalTestByCategoryWithFilter();
            if (TreeViewMedicalTest.Nodes.Count > 0 && !string.IsNullOrEmpty(filterString))
            {
                TreeViewMedicalTest.Nodes[0].Expand();
            }
            if (TreeViewMedicalTest.Nodes.Count > 0) { BtnMedicalTestEdit.Enabled = true; BtnMedicalTestDelete.Enabled = true; }
            else { BtnMedicalTestEdit.Enabled = false; BtnMedicalTestDelete.Enabled = false; }
            this.formIsDirty = false;
        }

        private void TreeViewMedicalTest_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode? node = e.Node;
            node!.SelectedImageIndex = node.ImageIndex;
            node.ExpandAll();
            node.Collapse();
            ResetForm();
            loadMedicalTestCombo();
            LoadMedicalTestCategoryInfo();
            EnableForm(false);
            this.formIsDirty = false;
        }

        private void BtnMedicalTestNew_ItemClickedEvent(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Category")
            {
                BtnNewMedicalTestCategory_Click(sender, e);
            }
            else if (e.ClickedItem.Text == "MedicalTest")
            {
                BtnMedicalTestNew_Click(sender, e);
            }
        }

        private void BtnNewMedicalTestCategory_Click(object sender, EventArgs e)
        {
            TextBoxMedicalTestSearch.ResetText();
            TabControlMedicaltestCategory.Visible = true;
            TabControlMedicalTest.Visible = false;
            MedicalTestClear();
            this.formIsDirty = false;
        }

        private void BtnMedicalTestNew_Click(object sender, EventArgs e)
        {
            if (MedicalTestManager.ListMedicalTestCategoryByCompanyId(Global.Company.CompanyId).Count > 0)
            {
                TabControlMedicaltestCategory.Visible = false;
                TabControlMedicalTest.Visible = true;
                TextBoxMedicalTestSearch.ResetText();
                MedicalTestClear();
                ResetForm();
                EnableForm(true);
                CheckBoxMedicalTestIsActive.Checked = true;
                TabControlMedicalTest.SelectedTab = TabTestInformation;
                TextBoxMedicalTestName.Select();
                this.formIsDirty = false;
            }
            else
            {
                NewCategorytoolstrip.PerformClick();
                MedicalTestErrorMsg.Text = NoCategoryFoundErrorMsg;
            }
            this.formIsDirty = false;
        }

        private void newMedicalTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MedicalTestManager.ListMedicalTestCategoryByCompanyId(Global.Company.CompanyId).Count > 0)
            {
                TabControlMedicaltestCategory.Visible = false;
                TabControlMedicalTest.Visible = true;
                TextBoxMedicalTestSearch.ResetText();
                MedicalTestClear();
                ResetForm();
                EnableForm(true);
                if (TreeViewMedicalTest != null && TreeViewMedicalTest.SelectedNode != null && !TreeViewMedicalTest.SelectedNode.Name.Contains('@'))
                {
                    comboBoxSwapTextBoxMedicalTestCategory.SelectedIndex = comboBoxSwapTextBoxMedicalTestCategory.FindStringExact(TreeViewMedicalTest.SelectedNode.Text);
                }
                CheckBoxMedicalTestIsActive.Checked = true;
                TabControlMedicalTest.SelectedTab = TabTestInformation;
                TextBoxMedicalTestName.Select();
                this.formIsDirty = false;
            }
            else
            {
                NewCategorytoolstrip.PerformClick();
                MedicalTestErrorMsg.Text = NoCategoryFoundErrorMsg;
            }
            this.formIsDirty = false;
        }

        private void MedicalTestClear()
        {
            ResetForm();
            EnableForm(true);
            loadMedicalTestCombo();
            if (TabControlMedicaltestCategory.Visible)
            {
                TextBoxMedicaltestCategoryName.Select();
            }
            else
            {
                TextBoxMedicalTestName.Select();
            };
        }

        private void loadMedicalTestCombo()
        {
            ComboUtils.InitializeMedicalTestCategoryCombo(comboBoxSwapTextBoxMedicalTestCategory, Global.Company.CompanyId);
            ComboUtils.InitializeMedicalTestSampleRequirementCombo(comboBoxSampleRequirement, Global.Company.CompanyId);
            ComboUtils.InitializeParentMedicalTestCategoryCombo(comboBoxSwapTextBoxMedicalTestParentCategory, Global.Company.CompanyId);
        }

        private void NewCategorytoolstrip_Click(object sender, EventArgs e)
        {
            TextBoxMedicalTestSearch.ResetText();
            TabControlMedicaltestCategory.Visible = true;
            TabControlMedicalTest.Visible = false;
            MedicalTestClear();
            if (TreeViewMedicalTest != null && TreeViewMedicalTest.SelectedNode != null && !TreeViewMedicalTest.SelectedNode.Name.Contains('@'))
            {
                comboBoxSwapTextBoxMedicalTestParentCategory.SelectedIndex = comboBoxSwapTextBoxMedicalTestParentCategory.FindStringExact(TreeViewMedicalTest.SelectedNode.Text);
            }
            this.formIsDirty = false;
        }

        private void PointSaveOrUpdatedNode(TreeNodeCollection nodes, string findtext)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Name.ToString().Trim() == findtext)
                {
                    node.Expand();
                    node.TreeView.SelectedNode = node.NextNode;
                    TreeViewMedicalTest.SelectedNode = node;
                    node.TreeView.Focus();
                    break;
                }
                PointSaveOrUpdatedNode(node.Nodes, findtext);
            }
        }

        private void BtnMedicalTestDelete_Click(object sender, EventArgs e)
        {
            if (TabControlMedicalTest.Visible == true)
            {
                MedicalTest MedicalTestInfo = GetMedicalTestInfo();
                MedicalTestErrorMsg.Text = "";
                if (MedicalTestInfo != null)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete the Medical Test, " + MedicalTestInfo.Name + "?", "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        if (MedicalTestManager.DeleteMedicalTest(MedicalTestInfo.Id))
                        {
                            ResetForm();
                            TabControlMedicalTest.SelectedTab = TabTestInformation;
                            loadMedicalTestCombo();
                            LoadMedicalTestByCategoryWithFilter();
                            TextBoxMedicalTestSearch.Clear();
                            EnableForm(false);
                            TreeViewMedicalTest.Select();
                            MedicalTestErrorMsg.Text = string.Format(MedicalTestRemoveSuccess, "MedicalTest", MedicalTestInfo.Name);
                            this.formIsDirty = false;
                        }
                        else
                        {
                            MedicalTestErrorMsg.Text = string.Format(MedicalTestDeleteNotAllowErrorText, "MedicalTest");
                        }
                        this.formIsDirty = false;
                    }
                }
                else
                {
                    DisplaySystemError(SelectedMedicalTestNotValidErrorMsg);
                    return;
                }
            }
            else if (TabControlMedicaltestCategory.Visible)
            {
                MedicalTestCategory medicalTestCategory = GetMedicalTestCategoryInfo();
                MedicalTestErrorMsg.Text = string.Empty;
                if (medicalTestCategory == null)
                {
                    DisplaySystemError(SelectedMedicalTestNotValidErrorMsg);
                    return;
                }
                DialogResult confirmation = MessageBox.Show(string.Format(DeleteMedicalTestConfirmText, medicalTestCategory.Name), "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (confirmation != DialogResult.Yes) { return; };

                List<MedicalTestCategory> ChildCategorys = MedicalTestManager.GetChildMedicalTestCategoryByParentId(medicalTestCategory.Id, Global.Company.CompanyId);
                List<MedicalTest> medicalTests = null!;
                if (ChildCategorys != null && ChildCategorys.Count > 0)
                {
                    foreach (MedicalTestCategory childCategory in ChildCategorys)
                    {
                        medicalTests = MedicalTestManager.Instance.GetMedicalTestByCategoryId(childCategory.Id, Global.Company.CompanyId);

                        if (IsAnyMedicalTestLinkedToConsultation(medicalTests, out string linkedChildTestName))
                        {
                            MedicalTestErrorMsg.Text = string.Format(MedicalTestDeleteNotAllowErrorText, linkedChildTestName);
                            return;
                        }
                    }
                    foreach (MedicalTestCategory childCategory in ChildCategorys)
                    {
                        medicalTests = MedicalTestManager.Instance.GetMedicalTestByCategoryId(childCategory.Id, Global.Company.CompanyId);

                        foreach (var medicalTest in medicalTests)
                        {
                            MedicalTestManager.DeleteMedicalTest(medicalTest.Id);
                        }
                        MedicalTestManager.DeleteMedicalTestCategory(childCategory.Id);
                    }
                }
                medicalTests = MedicalTestManager.Instance.GetMedicalTestByCategoryId(medicalTestCategory.Id, Global.Company.CompanyId);

                if (IsAnyMedicalTestLinkedToConsultation(medicalTests, out string linkedTestName))
                {
                    MedicalTestErrorMsg.Text = string.Format(MedicalTestDeleteNotAllowErrorText, linkedTestName);
                    return;
                }

                foreach (var medicalTest in medicalTests)
                {
                    MedicalTestManager.DeleteMedicalTest(medicalTest.Id);
                }

                if (MedicalTestManager.DeleteMedicalTestCategory(medicalTestCategory.Id))
                {
                    ResetAndReloadForm(medicalTestCategory.Name);
                }
                else
                {
                    MedicalTestErrorMsg.Text = string.Format(DeleteNotAllowErrorText, "MedicalTestCategory");
                }
            }
        }
        private bool IsAnyMedicalTestLinkedToConsultation(List<MedicalTest> medicalTests, out string linkedTestName)
        {
            linkedTestName = string.Empty;

            foreach (var test in medicalTests)
            {
                var consultedTest = ConsultationNoteManager.Instance.GetConsultedLabTestsByMedicalTestId(test.Id);
                if (consultedTest != null)
                {
                    linkedTestName = consultedTest.Name;
                    return true;
                }
            }
            return false;
        }
        private void ResetAndReloadForm(string categoryName)
        {
            ResetForm();
            TabControlMedicaltestCategory.SelectedTab = TabMedicalTestCategory;
            loadMedicalTestCombo();
            LoadMedicalTestByCategoryWithFilter();
            TextBoxMedicalTestSearch.Clear();
            EnableForm(false);
            SetFocus();
            MedicalTestErrorMsg.Text = string.Format(MedicalTestRemoveSuccess, "MedicalTestCategory", categoryName);
            this.formIsDirty = false;
        }

        private void LoadMedicalTestByCategoryWithFilter()
        {
            string filterString = TextBoxMedicalTestSearch.Text.Trim();
            TreeViewMedicalTest.Nodes.Clear();
            IList<MedicalTestCategory> medicalTestCategories = MedicalTestManager.ListMedicalTestCategoryByFilterCompanyId(Global.Company.CompanyId, filterString);
            IList<MedicalTest> medicalTests = MedicalTestManager.ListFilterMedicalTestByCompanyId(Global.Company.CompanyId, filterString);
            if (medicalTests != null && medicalTests.Count > 0)
            {
                var CategoryIds = medicalTests.Select(t => t.MedicalTestCategoryId).Distinct().ToList();

                foreach (var categoryId in CategoryIds)
                {
                    MedicalTestCategory category = MedicalTestManager.GetMedicalTestCategoryInfoById((long)categoryId!);
                    if (category != null && !medicalTestCategories.Any(c => c.Id == category.Id))
                    {
                        medicalTestCategories.Add(category);
                    }
                }
            }
            if (medicalTestCategories != null && medicalTestCategories.Count > 0)
            {
                foreach (var category in medicalTestCategories.Where(c => c.ParentMedicalTestCategoryId == null))
                {
                    TreeNode[] existingNodes = TreeViewMedicalTest.Nodes.Cast<TreeNode>().Where(n => n.Text == category.Name).ToArray();
                    if (existingNodes.Length == 0)
                    {
                        TreeNode rootNode = new TreeNode
                        {
                            Text = category.Name,
                            Name = category.Id.ToString(),
                            ImageIndex = 0
                        };
                        TreeViewMedicalTest.Nodes.Add(rootNode);
                        GetChildNode(category.Id, rootNode, medicalTestCategories, medicalTests!);
                    }
                }
            }
            if (TreeViewMedicalTest.Nodes.Count > 0)
            {
                if (!string.IsNullOrEmpty(filterString))
                {
                    TreeViewMedicalTest.ExpandAll();
                }
                TreeViewMedicalTest.SelectedNode = TreeViewMedicalTest.Nodes[0];
            }
        }


        private void GetChildNode(long parentCategoryId, TreeNode parentNode, IList<MedicalTestCategory> medicalTestCategories, IList<MedicalTest> medicalTests)
        {
            foreach (var category in medicalTestCategories.Where(c => c.ParentMedicalTestCategoryId == parentCategoryId))
            {
                TreeNode categoryNode = new TreeNode
                {
                    Text = category.Name,
                    Name = category.Id.ToString(),
                    ImageIndex = 0
                };
                GetChildNode(category.Id, categoryNode, medicalTestCategories, medicalTests);
                parentNode.Nodes.Add(categoryNode);
            }
            foreach (var test in medicalTests.Where(t => t.MedicalTestCategoryId == parentCategoryId))
            {
                TreeNode testNode = new TreeNode
                {
                    Text = $"{test.Name} (Code: {test.TestCode})",
                    Name = $"{test.Id}@",
                    ImageIndex = 1
                };
                parentNode.Nodes.Add(testNode);
            }
        }

        private void BtnMedicalTestEdit_Click(object? sender, EventArgs? e)
        {
            LoadMedicalTestCategoryInfo();
            EnableForm(true);
            if (TabControlMedicaltestCategory.Visible)
            {
                if (comboBoxSwapTextBoxMedicalTestParentCategory.SelectedIndex < 0)
                {
                    comboBoxSwapTextBoxMedicalTestParentCategory.Visible = false;
                }
                TabControlMedicaltestCategory.SelectedTab = TabMedicalTestCategory;
                TextBoxMedicaltestCategoryName.Select();
            }
            else if (TabControlMedicalTest.Visible)
            {
                TabControlMedicalTest.SelectedTab = TabTestInformation;
                TextBoxMedicalTestName.Select();
                TextBoxMedicalTestReason.ReadOnly = CheckBoxMedicalTestIsActive.Checked;
            }
            this.formIsDirty = false;
        }

        private void BtnMedicalTestCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show("There are unsaved changes, Do you want cancel?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.No)
                {
                    FocusMedicalTestCategory();
                    return;
                }
            }
            ResetForm();
            loadMedicalTestCombo();
            if (string.IsNullOrEmpty(TextBoxMedicalTestSearch.Text))
            {
                LoadMedicalTestCategoryInfo();
            }
            else
            {
                TextBoxMedicalTestSearch.Clear();
            }
            TextBoxMedicalTestSearch.Select();
            EnableForm(false);
            SetFocus();
            this.formIsDirty = false;
        }

        private void FocusMedicalTestCategory()
        {
            if (TabControlMedicalTest.Visible)
            {
                TabControlMedicalTest.SelectedTab = TabTestInformation;
                TextBoxMedicalTestName.Select();
            }
            else
            {
                TextBoxMedicaltestCategoryName.Select();
            }
        }

        private void SetFocus()
        {
            if (TreeViewMedicalTest.Nodes.Count > 0)
            {
                TextBoxMedicalTestSearch.Select();
            }
            else
            {
                BtnMedicalTestNew.Focus();
            }
        }

        private void BtnMedicalTestSave_Click(object sender, EventArgs e)
        {
            if (TabControlMedicaltestCategory.Visible && ValidateFormMedicalTestCategory())
            {
                MedicalTestCategory MedicalTestCategoryInfo = GetMedicalTestCategoryFrom();
                if (MedicalTestManager.MedicalTestCategoryNameUniqueById(MedicalTestCategoryInfo))
                {
                    MedicalTestCategory lMedicalTestCategoryFromDB = null!;
                    if (MedicalTestCategoryInfo.Id == 0)
                    {
                        lMedicalTestCategoryFromDB = MedicalTestManager.AddMedicalTestCategory(MedicalTestCategoryInfo);
                    }
                    else
                    {
                        MedicalTestCategory lMedicalTestCategoryById = MedicalTestManager.GetMedicalTestCategoryInfoById(MedicalTestCategoryInfo.Id);
                        if (lMedicalTestCategoryById != null)
                        {
                            lMedicalTestCategoryFromDB = MedicalTestManager.UpdateMedicalTestCategory(MedicalTestCategoryInfo);
                        }
                        else
                        {
                            DisplaySystemError(SelectedMedicalTestCategoryNotValidErrorMsg);
                            return;
                        }
                    }
                    if (CreateMedicalTestOnLoad)
                    {
                        MedicalTest_Load(sender, e);
                        return;
                    }
                    ResetForm();
                    TabControlMedicaltestCategory.SelectedTab = TabMedicalTestCategory;
                    loadMedicalTestCombo();
                    if (string.IsNullOrEmpty(TextBoxMedicalTestSearch.Text))
                    {
                        LoadMedicalTestByCategoryWithFilter();
                    }
                    else
                    {
                        TextBoxMedicalTestSearch.Clear();
                    }
                    PointSaveOrUpdatedNode(TreeViewMedicalTest.Nodes, lMedicalTestCategoryFromDB.Id.ToString());
                    EnableForm(false);
                    this.formIsDirty = false;
                }
                else
                {
                    MedicalTestErrorMsg.Text = string.Format(UniqueNameErrorMsg, "MedicalTest Category", MedicalTestCategoryInfo.Name);
                    TextBoxMedicaltestCategoryName.Select();
                }
            }
            else if (TabControlMedicalTest.Visible && validateForm())
            {
                MedicalTest lMedicalTest = GetMedicalTestFromForm();
                if (MedicalTestManager.MedicalTestNameUniqueById(lMedicalTest))
                {
                    MedicalTest MedicalTestFromDB = null!;
                    if (lMedicalTest.Id == 0)
                    {
                        MedicalTestFromDB = MedicalTestManager.AddMedicalTest(lMedicalTest);
                    }
                    else
                    {
                        MedicalTest lMedicalTestById = MedicalTestManager.GetMedicalTestById(lMedicalTest.Id);
                        if (lMedicalTestById != null)
                        {
                            MedicalTestFromDB = MedicalTestManager.UpdateMedicalTest(lMedicalTest);
                        }
                        else
                        {
                            DisplaySystemError(SelectedMedicalTestNotValidErrorMsg);
                            return;
                        }
                    }
                    if (CreateMedicalTestOnLoad)
                    {
                        this.formIsDirty = false;
                        this.Close();
                    }
                    ResetForm();
                    TabControlMedicalTest.SelectedTab = TabTestInformation;
                    loadMedicalTestCombo();
                    if (string.IsNullOrEmpty(TextBoxMedicalTestSearch.Text))
                    {
                        LoadMedicalTestByCategoryWithFilter();
                    }
                    else
                    {
                        TextBoxMedicalTestSearch.Clear();
                    }
                    PointSaveOrUpdatedNode(TreeViewMedicalTest.Nodes, MedicalTestFromDB.Id.ToString() + "@");
                    MedicalTestErrorMsg.Text = SaveSuccessText;
                    EnableForm(false);
                    this.formIsDirty = false;
                }
                else
                {
                    MedicalTestErrorMsg.Text = string.Format(UniqueNameErrorMsg, "MedicalTest", lMedicalTest.Name);
                    TextBoxMedicalTestName.Select();
                }
                if (CheckBoxMedicalTestHasElement.Checked)
                {
                    if (GridViewElementInformation.Rows.Count > 1)
                    {
                        return;
                    }
                    else
                    {
                        MedicalTestErrorMsg.Text = EnterElementErrorMsg;
                        EnableForm(true);
                        TabControlMedicalTest.SelectedTab = TabTestElement;
                        GridViewElementInformation.CurrentCell = GridViewElementInformation.Rows[0].Cells[1];
                    }
                }
            }
        }
        private Boolean ValidateFormMedicalTestCategory()
        {
            MedicalTestErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxMedicaltestCategoryName.Text.Trim()))
            {
                MedicalTestErrorMsg.Text = string.Format(EnterNameErrorMsg, "MedicalTest Category");
                TextBoxMedicaltestCategoryName.Select();
                return false;
            }
            if (comboBoxSwapTextBoxMedicalTestParentCategory.SelectedIndex >= 0 && MedicalTestManager.GetMedicalTestCategoryInfoById(((MedicalTestCategory)comboBoxSwapTextBoxMedicalTestParentCategory.Items[comboBoxSwapTextBoxMedicalTestParentCategory.SelectedIndex]).Id) == null)
            {
                MedicalTestErrorMsg.Text = SelectedParentNotValidErrorMsg;
                comboBoxSwapTextBoxMedicalTestParentCategory.Select();
                return false;
            }
            return true;
        }

        private Boolean validateForm()
        {
            MedicalTestErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxMedicalTestName.Text.Trim()))
            {
                MedicalTestErrorMsg.Text = EnterNameErrorMsg;
                TextBoxMedicalTestName.Select();
                return false;
            }
            if (string.IsNullOrEmpty(textBoxTestCode.Text.Trim()))
            {
                MedicalTestErrorMsg.Text = EnterTestCodeErrorMsg;
                textBoxTestCode.Select();
                return false;
            }
            if (comboBoxSwapTextBoxMedicalTestCategory.SelectedIndex < 0)
            {
                MedicalTestErrorMsg.Text = ChooseMedicalTestCategoryErrorMsg;
                TabControlMedicalTest.SelectedTab = TabTestInformation;
                comboBoxSwapTextBoxMedicalTestCategory.Select();
                return false;
            }
            if (!CheckBoxMedicalTestIsActive.Checked && string.IsNullOrEmpty(TextBoxMedicalTestReason.Text.Trim()))
            {
                MedicalTestErrorMsg.Text = ActivReasonErrorMsg;
                TextBoxMedicalTestReason.Select();
                return false;
            }
            if (GridViewKeywords.Rows.Count > 1)
            {
                for (int i = 0; i < GridViewKeywords.Rows.Count - 1; i++)
                {
                    if (GridViewKeywords.Rows[i].Cells[(int)KeywordsGridColumn.NAME].Value == null)
                    {
                        MedicalTestErrorMsg.Text = string.Format(EnterKeyWordErrorMsg, GridViewKeywords.Columns[(int)KeywordsGridColumn.NAME].HeaderText);
                        GridViewKeywords.Select();
                        GridViewKeywords.CurrentCell = GridViewKeywords[(int)KeywordsGridColumn.NAME, i];
                        return false;
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
                        if (Countings > 1 && GridViewKeywords.Rows[k].Cells[(int)KeywordsGridColumn.NAME].Value.ToString()!.Trim() != "")
                        {
                            MedicalTestErrorMsg.Text = string.Format(EnterKeywordNameExistsErrorMsg, GridViewKeywords.Rows[k].Cells[(int)KeywordsGridColumn.NAME].Value.ToString());
                            GridViewKeywords.Select();
                            GridViewKeywords.CurrentCell = GridViewKeywords[(int)KeywordsGridColumn.NAME, k];
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private MedicalTestCategory GetMedicalTestCategoryFrom()
        {
            MedicalTestCategory lMedicalTestCategory = new MedicalTestCategory();
            if (!string.IsNullOrEmpty(TextBoxMedicalTestCategoryId.Text))
            {
                lMedicalTestCategory.Id = Convert.ToInt64(TextBoxMedicalTestCategoryId.Text);
            }
            else
            {
                lMedicalTestCategory.Id = 0L;
            }
            lMedicalTestCategory.Name = TextBoxMedicaltestCategoryName.Text.Trim();
            lMedicalTestCategory.Description = TextBoxMedicaltestCategoryDescription.Text.Trim();
            lMedicalTestCategory.DisplayAs = TextBoxMedicaltestCategoryDisplayAs.Text.Trim();
            lMedicalTestCategory.CompanyId = Global.Company.CompanyId;
            if (comboBoxSwapTextBoxMedicalTestParentCategory.SelectedIndex > -1)
            {
                MedicalTestCategory MedicalTestCategory = (MedicalTestCategory)comboBoxSwapTextBoxMedicalTestParentCategory.Items[comboBoxSwapTextBoxMedicalTestParentCategory.SelectedIndex];
                if (MedicalTestCategory != null)
                {
                    MedicalTestCategory = MedicalTestManager.GetMedicalTestCategoryInfoById(MedicalTestCategory.Id);
                    if (MedicalTestCategory != null)
                    {
                        lMedicalTestCategory.ParentMedicalTestCategoryId = MedicalTestCategory.Id;
                        lMedicalTestCategory.IsSubMedicalTestCategory = true;
                    }
                }
            }
            else
            {
                lMedicalTestCategory.IsSubMedicalTestCategory = false;
            }
            return lMedicalTestCategory;
        }
        private void BtnMedicalTestExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ResetForm()
        {
            MedicalTestErrorMsg.Text = "";
            TextBoxMedicalTestId.ResetText();
            TextBoxMedicalTestName.ResetText();
            TextBoxMedicalTestDisplayName.ResetText();
            textBoxTestCode.ResetText();
            textBoxShortName.ResetText();
            TextBoxMedicalTestDescription.ResetText();
            TextBoxMedicalTestReason.ResetText();
            CheckBoxMedicalTestIsActive.Checked = true;
            CheckBoxMedicalTestHasElement.Checked = false;
            GridViewElementInformation.Rows.Clear();

            TextBoxMedicalTestCategoryId.ResetText();
            TextBoxMedicaltestCategoryName.ResetText();
            TextBoxMedicaltestCategoryDisplayAs.ResetText();
            TextBoxMedicaltestCategoryDescription.ResetText();

            comboBoxSwapTextBoxMedicalTestCategory.SelectedIndex = -1;
            comboBoxSwapTextBoxMedicalTestParentCategory.SelectedIndex = -1;
            comboBoxSampleRequirement.SelectedIndex = -1;
            GridViewKeywords.Rows.Clear();
        }
        private void EnableForm(Boolean enable)
        {
            if (MedicalTestManager.ListMedicalTestCategoryByCompanyId(Global.Company.CompanyId).Count > 0)
            {
                TreeViewMedicalTest.Enabled = !enable;
                TextBoxMedicalTestSearch.ReadOnly = enable;
                TextBoxMedicalTestSearch.TabStop = !enable;
            }
            else
            {
                TreeViewMedicalTest.Enabled = false;
                TabControlMedicaltestCategory.Visible = true;
                TabControlMedicalTest.Visible = false;
                TextBoxMedicalTestSearch.ReadOnly = true;
                TextBoxMedicalTestSearch.TabStop = false;
                BtnMedicalTestNew.Select();
            }
            TextBoxMedicaltestCategoryName.ReadOnly = !enable;
            TextBoxMedicaltestCategoryName.TabStop = enable;
            TextBoxMedicaltestCategoryDisplayAs.ReadOnly = !enable;
            TextBoxMedicaltestCategoryDisplayAs.TabStop = enable;
            TextBoxMedicaltestCategoryDescription.ReadOnly = !enable;
            TextBoxMedicaltestCategoryDescription.TabStop = enable;
            comboBoxSwapTextBoxMedicalTestParentCategory.Visible = enable;
            comboBoxSwapTextBoxMedicalTestParentCategory.TabStop = enable;
            TextBoxMedicalTestName.ReadOnly = !enable;
            TextBoxMedicalTestName.TabStop = enable;
            TextBoxMedicalTestDisplayName.ReadOnly = !enable;
            TextBoxMedicalTestDisplayName.TabStop = enable;
            textBoxShortName.ReadOnly = !enable;
            textBoxShortName.TabStop = enable; 
            textBoxTestCode.ReadOnly = !enable;
            textBoxTestCode.TabStop = enable;
            TextBoxMedicalTestDescription.ReadOnly = !enable;
            TextBoxMedicalTestDescription.TabStop = enable;
            CheckBoxMedicalTestIsActive.Enabled = enable;
            CheckBoxMedicalTestHasElement.Enabled = enable;
            TextBoxMedicalTestReason.ReadOnly = CheckBoxMedicalTestIsActive.Checked;
            TextBoxMedicalTestReason.TabStop = !CheckBoxMedicalTestIsActive.Checked;
            comboBoxSwapTextBoxMedicalTestCategory.Visible = enable;
            comboBoxSwapTextBoxMedicalTestCategory.TabStop = enable;
            comboBoxSampleRequirement.Visible = enable;
            comboBoxSampleRequirement.TabStop = enable;
            GridViewKeywords.Enabled = enable;
            GridViewElementInformation.Enabled = CheckBoxMedicalTestHasElement.Checked;

            if (!enable)
            {
                TextBoxMedicalTestReason.ReadOnly = !enable;
                TextBoxMedicalTestReason.TabStop = enable;
                BtnMedicalTestCancel.Enabled = enable;
                if (TreeViewMedicalTest.SelectedNode == null)
                {
                    BtnMedicalTestDelete.Enabled = enable;
                    BtnMedicalTestEdit.Enabled = enable;
                }
                else
                {
                    BtnMedicalTestDelete.Enabled = !enable;
                    BtnMedicalTestEdit.Enabled = !enable;
                }
                BtnMedicalTestNew.Enabled = !enable;
                BtnMedicalTestSave.Enabled = enable;
                BtnExport.Enabled = !enable;
                BtnImport.Enabled = !enable;
            }
            else
            {
                BtnMedicalTestCancel.Enabled = enable;
                BtnMedicalTestDelete.Enabled = !enable;
                BtnMedicalTestEdit.Enabled = !enable;
                BtnMedicalTestNew.Enabled = !enable;
                BtnMedicalTestSave.Enabled = enable;
                BtnExport.Enabled = !enable;
                BtnImport.Enabled = !enable;
            }
        }

        private void CheckBoxMedicalTestIsActive_CheckedChanged(object sender, EventArgs e)
        {

            TextBoxMedicalTestReason.ReadOnly = CheckBoxMedicalTestIsActive.Checked;
            TextBoxMedicalTestReason.TabStop = !CheckBoxMedicalTestIsActive.Checked;
            if (CheckBoxMedicalTestIsActive.Checked)
            {
                TextBoxMedicalTestReason.ResetText();
            }
        }
        private void CheckBoxMedicalTestHasElement_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxMedicalTestHasElement.Checked)
            {
                GridViewElementInformation.Enabled = true;
            }
            else
            {
                GridViewElementInformation.Enabled = false;
            }
        }
        private void GridViewElementInfo_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.SNO].ReadOnly = true;
            GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.NAME].ReadOnly = true;
            GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.ELEMENTCODE].ReadOnly = true;
            GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.ELEMENTSHORTNAME].ReadOnly = true;
            GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.UOM].ReadOnly = true;
            GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.RANGE_FROM].ReadOnly = true;
            GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.RANGE_TO].ReadOnly = true;
            GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.DESCRIPTION].ReadOnly = true;
            GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.SINGLEVALUE].ReadOnly = true;
            GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.CLASS].ReadOnly = true;
            GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.SUBCLASS].ReadOnly = true;
            GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.RESULTDURATION].ReadOnly = true;
            GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.REMOVE].ReadOnly = true;
            if ((!BtnMedicalTestEdit.Enabled || !BtnMedicalTestNew.Enabled))
            {
                GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.NAME].ReadOnly = false;
                GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.ELEMENTCODE].ReadOnly = false;
                GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.ELEMENTSHORTNAME].ReadOnly = false;
                GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.UOM].ReadOnly = false;
                GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.RANGE_FROM].ReadOnly = false;
                GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.RANGE_TO].ReadOnly = false;
                GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.DESCRIPTION].ReadOnly = false;
                GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.SINGLEVALUE].ReadOnly = false;
                GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.CLASS].ReadOnly = false;
                GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.SUBCLASS].ReadOnly = false;
                GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.RESULTDURATION].ReadOnly = false;
                GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.REMOVE].ReadOnly = false;

                var rangeFromCell = GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.RANGE_FROM];
                var rangeToCell = GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.RANGE_TO];
                var singleValueCell = GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.SINGLEVALUE];

                bool hasRangeFrom = rangeFromCell.Value != null && !string.IsNullOrEmpty(rangeFromCell.Value.ToString());
                bool hasRangeTo = rangeToCell.Value != null && !string.IsNullOrEmpty(rangeToCell.Value.ToString());
                bool singleValue = singleValueCell.Value != null && !string.IsNullOrEmpty(singleValueCell.Value.ToString());

                if (e.ColumnIndex == (int)TestElementsGridColumn.SINGLEVALUE)
                {
                    singleValueCell.ReadOnly = (hasRangeFrom || hasRangeTo) ? true : false;
                }
                if (e.ColumnIndex == (int)TestElementsGridColumn.RANGE_FROM || e.ColumnIndex == (int)TestElementsGridColumn.RANGE_TO)
                {
                    rangeFromCell.ReadOnly = singleValue ? true : false;
                    rangeToCell.ReadOnly = singleValue ? true : false;
                }
            }
        }
        private void GridViewElementInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MedicalTestErrorMsg.Text = string.Empty;
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == (int)TestElementsGridColumn.REMOVE && GridViewElementInformation.Rows.Count - 1 != e.RowIndex && (!BtnMedicalTestEdit.Enabled))
                {

                    if (GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.ID].Value != null)
                    {
                        ConsultedLabTestElements ConsultedLabTestElements = ConsultationNoteManager.Instance.GetConsultedLabTestElementsByMedicalTestElementId(long.Parse(GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.ID].Value.ToString()!));
                        if (ConsultedLabTestElements != null)
                        {
                            MessageBox.Show(string.Format(DoNotAllowToDeleteElementMsg, GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.NAME].Value.ToString()));
                            return;
                        }
                    }
                    DialogResult Result = MessageBox.Show("Do you want to delete row " + GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.SNO].Value + "?", "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        GridViewElementInformation.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewElementInformation.Rows.RemoveAt(e.RowIndex);
                        ReSequence();
                    }
                }
            }
        }
        public class ComboBoxItem
        {
            public string? ID { get; set; }
            public string? Name { get; set; }
        }
        private void GridViewElementInfo_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            LoadUom(e.RowIndex);
            GridViewElementInformation.Rows[e.RowIndex].Cells[(int)TestElementsGridColumn.SNO].Value = e.RowIndex + 1;
        }
        private void GridViewElementInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void GridViewElementInfo_KeyPress1(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                ((ComboBox)GridViewElementInformation.EditingControl).DroppedDown = false;
            }
        }
        private void GridViewElementInfo_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            GridViewElementInformation.CommitEdit(DataGridViewDataErrorContexts.Commit);
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                ((ComboBox)e.Control).MaxLength = 10;
                e.Control.KeyPress += new KeyPressEventHandler(GridViewElementInfo_KeyPress1);

                if (GridViewElementInformation.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }
                else
                {
                    if (GridViewElementInformation.CurrentCell.FormattedValue != null)
                    {
                        MedicalTestUOM MedicalTestUOM = MedicalTestManager.GetMedicalTestUOMByName(GridViewElementInformation.CurrentCell.FormattedValue.ToString(), Global.Company.CompanyId);
                        if (MedicalTestUOM == null)
                        {
                            ((ComboBox)e.Control).SelectedIndex = -1;
                            ((ComboBox)e.Control).Text = GridViewElementInformation.CurrentCell.Value.ToString();
                        }
                    }
                }
            }
            if (e.Control is TextBox textBox)
            {
                if (GridViewElementInformation.CurrentCell.Value == null)
                {
                    textBox.Text = "";
                }
                if (GridViewElementInformation.CurrentCell.ColumnIndex == (int)TestElementsGridColumn.RANGE_FROM || GridViewElementInformation.CurrentCell.ColumnIndex == (int)TestElementsGridColumn.RANGE_TO)
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
                else
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                }
            }
            e.CellStyle.BackColor = Color.White;
            e.CellStyle.ForeColor = Color.Black;
            e.CellStyle.SelectionBackColor = Color.White;
            e.CellStyle.SelectionForeColor = Color.Black;
        }
        private void GridViewElementInfo_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7)
            {
                if (!string.IsNullOrEmpty(GridViewElementInformation.CurrentCell.EditedFormattedValue.ToString()!.Trim()))
                {
                    IList<MedicalTestUOM> values = MedicalTestManager.ListAllMedicalTestUOMByCompanyId(Global.Company.CompanyId);
                    MedicalTestUOM Output = values.ToList().Find(x => x.Name.Equals(GridViewElementInformation.CurrentCell.EditedFormattedValue.ToString()))!;
                    if (Output == null)
                    {
                        GridViewElementInformation.CurrentCell.Value = GridViewElementInformation.CurrentCell.EditedFormattedValue.ToString();
                    }
                    else
                    {
                        GridViewElementInformation.CurrentCell.Value = Output.Id;
                    }
                }
            }
        }
        private MedicalTestUOM AddUom(string UomName)
        {
            MedicalTestUOM lMedicalTestUOM = new MedicalTestUOM();
            lMedicalTestUOM.Name = UomName;
            lMedicalTestUOM.CompanyId = Global.Company.CompanyId;

            MedicalTestUOM MedicalTestUOM = MedicalTestManager.AddMedicalTestUom(lMedicalTestUOM);
            return MedicalTestUOM;
        }
        private void TextBoxMedicalTestName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                textBoxTestCode.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnMedicalTestExit.Focus();
            }
        }
        private void BtnMedicalTestSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            int RowCount = GridViewElementInformation.Rows.Count;
            int ColumnCount = GridViewElementInformation.Columns.Count - 1;
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnMedicalTestCancel.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (TabControlMedicaltestCategory.Visible)
                {
                    TabControlMedicaltestCategory.SelectedTab = TabMedicalTestCategory;
                    if (comboBoxSwapTextBoxMedicalTestParentCategory.Visible)
                    {
                        comboBoxSwapTextBoxMedicalTestParentCategory.Focus();
                    }
                    else
                    {
                        TextBoxMedicaltestCategoryDescription.Focus();
                    }
                    return;
                }
                if (GridViewElementInformation.Enabled)
                {
                    TabControlMedicalTest.SelectedTab = TabTestElement;
                    GridViewElementInformation.CurrentCell = GridViewElementInformation[ColumnCount - 3, RowCount - 1];
                }
                else
                {
                    TabControlMedicalTest.SelectedTab = TabTestInformation;
                    GridViewKeywords.Select();
                }
            }
        }
        private void CheckBoxMedicalTestHasElement_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewElementInformation.Enabled)
                {
                    TabControlMedicalTest.SelectedTab = TabTestElement;
                    GridViewElementInformation.CurrentCell = GridViewElementInformation[1, 0];
                }
                else
                {
                    BtnMedicalTestSave.Select();
                }

            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlMedicalTest.SelectedTab = TabTestInformation;
                if (!TextBoxMedicalTestReason.ReadOnly)
                {
                    TextBoxMedicalTestReason.Select();
                }
                else
                {
                    CheckBoxMedicalTestIsActive.Select();
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnMedicalTestNew.ShowDropDown();
            }
            if (keyData == (Keys.F4))
            {
                BtnMedicalTestDelete.PerformClick();
            }
            if (keyData == (Keys.F7))
            {
                BtnMedicalTestEdit.PerformClick();
            }
            if (keyData == (Keys.F8))
            {
                BtnMedicalTestSave.PerformClick();
            }
            if (keyData == (Keys.F10))
            {
                BtnMedicalTestExit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnMedicalTestCancel.PerformClick();
                return true;
            }
            if (keyData == Keys.Tab && ActiveControl == BtnMedicalTestExit)
            {
                if (TabControlMedicaltestCategory.Visible)
                {
                    TabControlMedicaltestCategory.SelectedTab = TabMedicalTestCategory;
                    TextBoxMedicaltestCategoryName.Focus();
                }
                else
                {
                    TabControlMedicalTest.SelectedTab = TabTestInformation;
                    TextBoxMedicalTestName.Select();
                }
                return true;
            }
            if (keyData == (Keys.Tab | Keys.Shift) && ActiveControl == TextBoxMedicaltestCategoryName)
            {
                BtnMedicalTestExit.Focus();
                return true;
            }
            try
            {
                if (GridViewElementInformation.CurrentCell != null && GridViewElementInformation.CurrentCell.Selected)
                {
                    if (keyData == (Keys.Tab) && GridViewElementInformation.Focused && GridViewElementInformation.CurrentCell.ColumnIndex == (int)TestElementsGridColumn.REMOVE)
                    {
                        if (GridViewElementInformation.CurrentCell.RowIndex != GridViewElementInformation.Rows.Count - 1)
                        {
                            SendKeys.Send("{tab}");
                        }
                        else
                        {
                            BtnMedicalTestSave.Select();
                            return true;
                        }
                    }

                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewElementInformation.CurrentCell.ColumnIndex == (int)TestElementsGridColumn.ELEMENTCODE && GridViewElementInformation.CurrentRow.Index != 0)
                    {
                        SendKeys.Send("{tab}");
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewElementInformation.CurrentCell.ColumnIndex == (int)TestElementsGridColumn.ELEMENTCODE && GridViewElementInformation.CurrentRow.Index == 0 && TabControlMedicalTest.SelectedTab == TabTestElement)
                    {
                        TabControlMedicalTest.SelectedTab = TabTestInformation;
                        GridViewKeywords.Select();
                        GridViewKeywords.CurrentCell = GridViewKeywords[1, GridViewKeywords.Rows.Count - 1];
                        return true;
                    }
                }
                //keyword
                if (GridViewKeywords.Visible && GridViewKeywords.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && GridViewKeywords.CurrentCell.ColumnIndex == (int)KeywordsGridColumn.NAME)
                    {
                        if (GridViewKeywords.CurrentCell.RowIndex != GridViewKeywords.Rows.Count - 1)
                        {
                            SendKeys.Send("{tab}{tab}");
                        }
                        else
                        {
                            GridViewKeywords.CurrentCell = GridViewKeywords[2, 0];
                            if (GridViewElementInformation.Enabled)
                            {
                                TabControlMedicalTest.SelectedTab = TabTestElement;
                                GridViewElementInformation.CurrentCell = GridViewElementInformation[1, 0];
                            }
                            else
                            {
                                BtnMedicalTestSave.Select();
                            }
                        }
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewKeywords.CurrentCell.ColumnIndex == (int)KeywordsGridColumn.NAME && GridViewKeywords.CurrentRow.Index != 0)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewKeywords.CurrentCell.ColumnIndex == (int)KeywordsGridColumn.NAME && GridViewKeywords.CurrentRow.Index == 0 && TabControlMedicalTest.SelectedTab == TabTestInformation)
                    {
                        SendKeys.Send("{tab}");
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void TextBoxMedicalTestSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && TreeViewMedicalTest.Nodes != null && TreeViewMedicalTest.Nodes.Count > 0)
            {
                TreeViewMedicalTest.Select();
            }
        }

        private void FormMedicalTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show("There are unsaved changes, Do you want Exit?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.No)
                {
                    TabControlMedicalTest.SelectedTab = TabTestInformation;
                    TextBoxMedicalTestName.Select();
                    e.Cancel = true;
                }
            }
        }

        private void TreeViewMedicalTest_Enter(object sender, EventArgs e)
        {
            if (TreeViewMedicalTest.Nodes == null || TreeViewMedicalTest.Nodes.Count == 0)
            {
                BtnMedicalTestNew.Select();
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

        private void GridViewKeywords_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            GridViewKeywords.Rows[e.RowIndex].Cells[(int)KeywordsGridColumn.SNO].Value = GridViewKeywords.Rows.Count;
        }

        private void GridViewKeywords_Enter(object sender, EventArgs e)
        {
            GridViewKeywords.CurrentCell = GridViewKeywords[1, 0];
        }

        private void GridViewElementInfo_Enter(object sender, EventArgs e)
        {
        }

        private void GridViewElementInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.BackColor = Color.White;
            e.CellStyle.ForeColor = Color.Black;
        }

        private void GridViewKeywords_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            MedicalTestErrorMsg.Text = string.Empty;
            MedicalTestFileUpload FileUploadDialog = new MedicalTestFileUpload();
            FileUploadDialog.Text = "Data Upload - Medical Test Master";
            FileUploadDialog.ShowDialog();
            if (FileUploadDialog.FileUploadComplete == true)
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                EnableForm(true);
                LoadMedicalTest();
                Cursor.Current = Cursors.Default;
            }
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            MedicalTestExportFile MedicalTestExportFiles = new MedicalTestExportFile();
            MedicalTestExportFiles.GenerateFile(Global.Company.CompanyId);
        }
        private void CheckBoxMedicalTestIsActive_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if ((e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift) && !CheckBoxMedicalTestIsActive.Checked)
            {
                e.IsInputKey = true;
                TextBoxMedicalTestReason.TabStop = true;
                TextBoxMedicalTestReason.Select();
            }
        }

        private void TreeViewMedicalTest_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeViewMedicalTest.SelectedNode = e.Node;
        }
    }
}
