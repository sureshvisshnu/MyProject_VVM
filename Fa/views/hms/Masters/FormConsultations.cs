using DocumentFormat.OpenXml.Office2010.Excel;
using fa.api.Accounting;
using fa.api.Hms;
using fa.api.utils;
using fa.libraries.utils;
using fa.libraries.Validation;
using fa.model.Employee;
using fa.model.Hms.Master;
using fa.views.hms.masters.upload;
using fa.views.utils.Common;
using Fa.views.utils.Report.Upload;
using FADataAccessLibrary.Api.Hms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace fa.views.hms.Masters
{

    public partial class FormConsultations : FormBase
    {
        public static string EnterNameErrorMsg = "Name could not be empty, Please enter name";
        public static string SaveSuccessMsg = "Save success...";
        public static string DeleteSuccessMsg = "Delete success...";
        public static string EnterNameExistsErrorMsg = "Consultation {0} already exists";
        public static string ErrorDeleteMsg = "Error deleting the consultation!, Please retry";
        public static string DoNotAllowToDeleteMsg = "Error deleting the consultation!, This consultation used in somewhere else";
        public static string FileDnloadErrorMsg = "Finished downloading";
        public static string CreateConsultationsOnloadText = "New {0}";
        public static string UpdateConsultationsOnloadText = "Update Consultations";
        ConsultationManager ConsultationManager = null;
        KeypressValidation KeypressValidation = null;
        public bool CreateConsultationsOnLoad = false;
        public string Id;
        public FormConsultations()
        {
            ConsultationManager = ConsultationManager.Instance;
            KeypressValidation = KeypressValidation.Instance;
            InitializeComponent();
        }
        public FormConsultations(bool IsNew)
        {
            ConsultationManager = ConsultationManager.Instance;
            KeypressValidation = KeypressValidation.Instance;
            InitializeComponent();
            if (TreeViewConsultation.Nodes.Count > 0)
            {
                TreeViewConsultation.SelectedNode = TreeViewConsultation.Nodes[0];
            }
            if (IsNew)
            {
                SetNewEntryForm();
            }
        }
        private void SetNewEntryForm()
        {
            TextBoxConsultationSearch.Visible = false;
            TreeViewConsultation.Visible = false;
            BtnConsultationNew.Visible = false;
            BtnConsultationDelete.Visible = false;
            BtnConsultationEdit.Visible = false;
            BtnImport.Visible = false;
            BtnExport.Visible = false;
            BtnConsultationCancel.Visible = false;
            this.Width = 578;
            this.Height = 575;
            this.Text = "New Consultations Fees";
            TabControlConsultation.Location = new System.Drawing.Point(17, 19);
            BtnConsultationSave.Location = new System.Drawing.Point(340, 477);
            BtnConsultationExit.Location = new System.Drawing.Point(432, 477);
            BtnConsultationNew_Click(this, null);
        }
        private void FormConsultationType_Load(object sender, EventArgs e)
        {
            ResetForm();
            EnableForm(true);
            LoadConsultationWithFilter();
            if (GridViewServiceProvider.Rows.Count == 0)
            {
                LoadServiceProviders();
            }
            if (CreateConsultationsOnLoad)
            {
                TextBoxConsultationSearch.Visible = false;
                TreeViewConsultation.Visible = false;
                BtnConsultationNew.Visible = false;
                BtnConsultationDelete.Visible = false;
                BtnConsultationEdit.Visible = false;
                BtnImport.Visible = false;
                BtnExport.Visible = false;
                BtnConsultationCancel.Visible = false;
                if (Id == null)
                {
                    this.Text = string.Format(CreateConsultationsOnloadText, "Consultations Fees");
                    BtnConsultationNew_Click(this, null);
                }
            }
        }
        private void LoadServiceProviderGrid()
        {
            int i = 0;
            if (TreeViewConsultation.Nodes.Count > 0 && TreeViewConsultation.SelectedNode != null)
            {
                Consultation consultation = ConsultationManager.Instance.GetConsultationById(long.Parse(TreeViewConsultation.SelectedNode.Name));
                IList<Employee> Employees = EmployeeManager.Instance.ListEmployeeByServiceProvider(Global.Company.CompanyId);
                if (Employees != null)
                {
                    foreach (Employee Employee in Employees)
                    {
                        ConsultationDetail consultationDetail = ConsultationDetailManager.Instance.GetConsultationDetailByEmployeeId(Global.Company.CompanyId, Employee.Id, consultation);
                        GridViewServiceProvider.Rows.Add();
                        GridViewServiceProvider.Rows[i].Cells[0].Value = i + 1;
                        GridViewServiceProvider.Rows[i].Cells[1].Value = Employee.Name;
                        GridViewServiceProvider.Rows[i].Cells[2].Value = consultationDetail != null ? consultationDetail.Fee : 0.00;
                        GridViewServiceProvider.Rows[i].Cells[3].Value = Employee.Id;
                        i++;
                    }
                }
            }
        }

        private void LoadConsultationWithFilter()
        {
            string FilterString = TextBoxConsultationSearch.Text.Trim();
            TreeViewConsultation.Nodes.Clear();
            IList<Consultation> Consultation = ConsultationManager.Instance.ListConsultationByCompanyId(Global.Company.CompanyId);
            if (Consultation.Count > 0)
            {
                foreach (var lConsultation in Consultation)
                {
                    if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim()) || lConsultation.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        TreeViewConsultation.Nodes.Add(lConsultation.Id.ToString(), lConsultation.Name);
                        TreeViewConsultation.Nodes[lConsultation.Id.ToString()].ImageIndex = 0;
                    }
                }
            }
            if (TreeViewConsultation.Nodes.Count > 0)
            {
                TreeViewConsultation.ExpandAll();
                TreeViewConsultation.SelectedNode = TreeViewConsultation.Nodes[0];
            }
            else
            {
                TextBoxConsultationName.Select();
            }
        }
        private Consultation GetConsultationInfo()
        {
            if (!CreateConsultationsOnLoad)
            {
                if (TreeViewConsultation.SelectedNode != null)
                {
                    Consultation lConsultation = ConsultationManager.GetConsultationById(long.Parse(TreeViewConsultation.SelectedNode.Name));
                    if (lConsultation != null)
                    {
                        Consultation ConsultationFromDB = ConsultationManager.GetConsultationById(lConsultation.Id);
                        return ConsultationFromDB;
                    }
                }
            }
            return null;
        }
        private void LoadConsultationInfo()
        {
            Consultation ConsultationFromDB = GetConsultationInfo();
            if (ConsultationFromDB != null)
            {
                TextBoxConsultationId.Text = ConsultationFromDB.Id.ToString();
                TextBoxConsultationName.Text = ConsultationFromDB.Name.ToString();
                TextBoxConsultationDisplayAs.Text = ConsultationFromDB.DisplayAs.ToString();
                TextBoxConsultationDescription.Text = ConsultationFromDB.Discription;
            }
        }
        private Consultation GetConsultationFromForm()
        {
            Consultation lConsultation = new Consultation();
            if (!string.IsNullOrEmpty(TextBoxConsultationId.Text))
            {
                lConsultation.Id = Convert.ToInt64(TextBoxConsultationId.Text);
            }
            else
            {
                lConsultation.Id = 0L;
            }
            lConsultation.Name = TextBoxConsultationName.Text.Trim();
            lConsultation.Discription = TextBoxConsultationDescription.Text;
            lConsultation.DisplayAs = TextBoxConsultationDisplayAs.Text.Trim();
            lConsultation.CompanyId = Global.Company.CompanyId;
            if (GridViewServiceProvider.Rows.Count > 0)
            {
                lConsultation.ConsultationDetail = new List<ConsultationDetail> { };
                for (int i = 0; i < GridViewServiceProvider.Rows.Count; i++)
                {
                    ConsultationDetail consultationDetail = new ConsultationDetail();
                    consultationDetail.ConsultationId = lConsultation.Id;
                    consultationDetail.EmployeeId = long.Parse(GridViewServiceProvider.Rows[i].Cells[3].Value.ToString()!);
                    consultationDetail.Fee = double.Parse(GridViewServiceProvider.Rows[i].Cells[2].Value.ToString()!);
                    consultationDetail.CompanyId = Global.Company.CompanyId;
                    lConsultation.ConsultationDetail.Add(consultationDetail);
                }
            }
            return lConsultation;
        }
        private void BtnConsultationNew_Click(object sender, EventArgs e)
        {
            TextBoxConsultationSearch.ResetText();
            ResetForm();
            EnableForm(true);
            LoadServiceProviders();
            TextBoxConsultationName.Select();
            this.formIsDirty = false;
        }
        private void LoadServiceProviders()
        {
            int i = 0;
            IList<Employee> Employees = EmployeeManager.Instance.ListEmployeeByServiceProvider(Global.Company.CompanyId);
            if (Employees != null && Employees.Count > 0)
            {
                foreach (Employee Employee in Employees)
                {
                    GridViewServiceProvider.Rows.Add();
                    GridViewServiceProvider.Rows[i].Cells[0].Value = i + 1;
                    GridViewServiceProvider.Rows[i].Cells[1].Value = Employee.Name;
                    GridViewServiceProvider.Rows[i].Cells[2].Value = 0.00;
                    GridViewServiceProvider.Rows[i].Cells[3].Value = Employee.Id;
                    i++;
                }
            }
        }
        private void BtnConsultationDelete_Click(object sender, EventArgs e)
        {
            ErrorMsgConsultation.Text = "";
            Consultation ConsultationInfo = GetConsultationInfo();
            if (ConsultationInfo != null)
            {
                DialogResult Result = MessageBox.Show("Do you want to delete the Consultation " + ConsultationInfo.Name + "?", "Delete Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                {
                    if (DoctorConsultationManager.Instance.GetDoctorConsultationByConsultationsId(ConsultationInfo.Id) == null)
                    {
                        if (ConsultationManager.DeleteConsultation(ConsultationInfo.Id))
                        {
                            ResetForm();
                            LoadConsultationWithFilter();
                            TextBoxConsultationSearch.Clear();
                            EnableForm(false);
                            TreeViewConsultation.Select();
                            ErrorMsgConsultation.Text = DeleteSuccessMsg;
                        }
                        else
                        {
                            ErrorMsgConsultation.Text = DoNotAllowToDeleteMsg;
                        }
                    }
                    else
                    {
                        ErrorMsgConsultation.Text = DoNotAllowToDeleteMsg;
                    }
                    this.formIsDirty = false;
                }
            }
            else
            {
                ErrorMsgConsultation.Text = ErrorDeleteMsg;
            }
        }
        private void BtnConsultationEdit_Click(object sender, EventArgs e)
        {
            ErrorMsgConsultation.Text = string.Empty;
            LoadConsultationInfo();
            EnableForm(true);
            TextBoxConsultationName.Select();
            this.formIsDirty = false;
        }
        private void BtnConsultationCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show("There are unsaved changes, Do you want cancel?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.No)
                {
                    TextBoxConsultationName.Select();
                    return;
                }
            }
            ResetForm();
            if (string.IsNullOrEmpty(TextBoxConsultationSearch.Text))
            {
                LoadConsultationInfo();
            }
            else
            {
                TextBoxConsultationSearch.Clear();
            }
            EnableForm(false);
            TreeViewConsultation.Select();
            LoadServiceProviderGrid();
            this.formIsDirty = false;
        }
        private void BtnConsultationSave_Click(object sender, EventArgs e)
        {
            if (validateForm())
            {
                Consultation lConsultation = GetConsultationFromForm();
                if (ConsultationManager.ConsultationNameUniqueById(lConsultation))
                {
                    Consultation ConsultationFromDB = null;
                    if (lConsultation.Id == 0)
                    {
                        ConsultationFromDB = ConsultationManager.AddConsultation(lConsultation);
                    }
                    else
                    {
                        Consultation lConsultationById = ConsultationManager.GetConsultationById(lConsultation.Id);
                        if (lConsultationById != null)
                        {
                            ConsultationFromDB = ConsultationManager.UpdateConsultation(lConsultation);
                        }
                    }
                    if (CreateConsultationsOnLoad)
                    {
                        this.formIsDirty = false;
                        this.Close();
                    }
                    ResetForm();
                    if (string.IsNullOrEmpty(TextBoxConsultationSearch.Text))
                    {
                        LoadConsultationWithFilter();
                    }
                    else
                    {
                        TextBoxConsultationSearch.Clear();
                    }
                    TextBoxConsultationId.Text = ConsultationFromDB.Id.ToString();
                    TreeNode TreeNode = new TreeNode();
                    TreeNode = TreeViewConsultation.Nodes[ConsultationFromDB.Id.ToString()];
                    TreeViewConsultation.SelectedNode = TreeNode;
                    TreeViewConsultation.Focus();
                    ErrorMsgConsultation.Text = SaveSuccessMsg;
                }
                else
                {
                    ErrorMsgConsultation.Text = string.Format(EnterNameExistsErrorMsg, lConsultation.Name);
                    TextBoxConsultationName.Select();
                }
            }
        }
        private void BtnConsultationExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private Boolean validateForm()
        {
            ErrorMsgConsultation.Text = "";
            if (string.IsNullOrEmpty(TextBoxConsultationName.Text.Trim()))
            {
                ErrorMsgConsultation.Text = EnterNameErrorMsg;
                TextBoxConsultationName.Select();
                return false;
            }

            return true;
        }
        private void ResetForm()
        {
            ErrorMsgConsultation.Text = string.Empty;
            GridViewServiceProvider.Rows.Clear();
            TextBoxConsultationId.ResetText();
            TextBoxConsultationName.ResetText();
            TextBoxConsultationDisplayAs.ResetText();
            TextBoxConsultationDescription.ResetText();
        }
        private void EnableForm(Boolean enable)
        {
            if (!CreateConsultationsOnLoad)
            {
                Consultation ConsultationInfo = GetConsultationInfo();
                if (TreeViewConsultation.Nodes.Count > 0)
                {
                    TreeViewConsultation.Enabled = !enable;
                    TextBoxConsultationSearch.ReadOnly = enable;
                    TextBoxConsultationSearch.TabStop = !enable;
                }
                else
                {
                    TreeViewConsultation.Enabled = false;
                    TextBoxConsultationSearch.ReadOnly = true;
                    TextBoxConsultationSearch.TabStop = false;
                    BtnConsultationNew.Select();
                }
                TabControlConsultation.TabStop = enable;
                GridViewServiceProvider.Enabled = enable;
                GridViewServiceProvider.TabStop = enable;
                TextBoxConsultationName.ReadOnly = !enable;
                TextBoxConsultationName.TabStop = enable;
                TextBoxConsultationDisplayAs.ReadOnly = !enable;
                TextBoxConsultationDisplayAs.TabStop = enable;
                TextBoxConsultationDescription.ReadOnly = !enable;
                TextBoxConsultationDescription.TabStop = enable;
                if (!enable)
                {
                    BtnConsultationCancel.Enabled = enable;
                    if (TreeViewConsultation.SelectedNode == null)
                    {
                        BtnConsultationDelete.Enabled = enable;
                        BtnConsultationEdit.Enabled = enable;
                    }
                    else
                    {
                        BtnConsultationDelete.Enabled = !enable;
                        BtnConsultationEdit.Enabled = !enable;
                    }
                    BtnConsultationNew.Enabled = !enable;
                    BtnConsultationSave.Enabled = enable;
                }
                else
                {
                    BtnConsultationCancel.Enabled = enable;
                    BtnConsultationDelete.Enabled = !enable;
                    BtnConsultationEdit.Enabled = !enable;
                    BtnConsultationNew.Enabled = !enable;
                    BtnConsultationSave.Enabled = enable;
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnConsultationNew.PerformClick();
            }
            if (keyData == (Keys.F4))
            {
                BtnConsultationDelete.PerformClick();
            }
            if (keyData == (Keys.F7))
            {
                BtnConsultationEdit.PerformClick();
            }
            if (keyData == (Keys.F8))
            {
                BtnConsultationSave.PerformClick();
            }
            if (keyData == (Keys.F10))
            {
                BtnConsultationExit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnConsultationCancel.PerformClick();
                return true;
            }
            if (keyData == Keys.Tab && ActiveControl == TextBoxConsultationDescription)
            {
                if (GridViewServiceProvider.Rows.Count > 0)
                {
                    GridViewServiceProvider.CurrentCell = GridViewServiceProvider.Rows[0].Cells[2];
                    GridViewServiceProvider.BeginEdit(true);
                    return true;
                }
                else
                {
                    BtnConsultationSave.Focus();
                    return true;
                }
            }
            if (keyData == (Keys.Shift | Keys.Tab) && ActiveControl == TextBoxConsultationDescription)
            {
                TextBoxConsultationDisplayAs.Focus();
                return true;
            }
            if (keyData == (Keys.Shift | Keys.Tab) && ActiveControl == TextBoxConsultationDisplayAs)
            {
                TextBoxConsultationName.Focus();
                return true;
            }
            if (keyData == Keys.Tab && GridViewServiceProvider.CurrentCell != null && GridViewServiceProvider.CurrentCell.ColumnIndex == 2)
            {
                if (GridViewServiceProvider.CurrentCell.RowIndex != GridViewServiceProvider.Rows.Count - 1)
                {
                    SendKeys.Send("{tab}{tab}");
                }
            }
            if (keyData == (Keys.Shift | Keys.Tab) && GridViewServiceProvider.CurrentCell != null && GridViewServiceProvider.CurrentCell.ColumnIndex == 2)
            {
                if (GridViewServiceProvider.CurrentCell.RowIndex != 0)
                {
                    SendKeys.Send("{tab}{tab}");
                }
                else
                {
                    TextBoxConsultationDescription.Focus();
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void TextBoxConsultationSearch_TextChanged(object sender, EventArgs e)
        {
            ResetForm();
            LoadConsultationWithFilter();
            if (TreeViewConsultation.Nodes.Count > 0) { BtnConsultationEdit.Enabled = true; BtnConsultationDelete.Enabled = true; TreeViewConsultation.TabStop = true; }
            else { BtnConsultationEdit.Enabled = false; BtnConsultationDelete.Enabled = false; TreeViewConsultation.TabStop = false; }
            this.formIsDirty = false;
        }
        private void TreeViewConsultation_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            node.SelectedImageIndex = node.ImageIndex;
            ResetForm();
            LoadConsultationInfo();
            LoadServiceProviderGrid();
            EnableForm(false);
            this.formIsDirty = false;
        }

        private void TextBoxConsultationName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxConsultationDisplayAs.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnConsultationSave.Select();
            }
        }

        private void BtnConsultationSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxConsultationName.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewServiceProvider.Rows.Count > 0)
                {
                    GridViewServiceProvider.Select();
                    GridViewServiceProvider.CurrentCell = GridViewServiceProvider.Rows[GridViewServiceProvider.Rows.Count - 1].Cells[2];
                    GridViewServiceProvider.BeginEdit(true);
                }
                else
                {
                    TextBoxConsultationDescription.Focus();
                }
            }
        }
        private void TextBoxConsultationSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                TreeViewConsultation.Select();
            }
        }
        private void FormConsultations_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show("There are unsaved changes, Do you want Exit?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.No)
                {
                    TextBoxConsultationName.Select();
                    e.Cancel = true;
                }
            }
        }
        private void BtnImport_Click(object sender, EventArgs e)
        {
            ErrorMsgConsultation.Text = string.Empty;
            ConsultationsFileUpload FileUploadDialog = new ConsultationsFileUpload();
            FileUploadDialog.Text = "Data Upload - Consultations Master";
            FileUploadDialog.ShowDialog();
            if (FileUploadDialog.FileUploadComplete == true)
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                EnableForm(true);
                LoadConsultationWithFilter();
                Cursor.Current = Cursors.Default;
            }
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            ErrorMsgConsultation.Text = string.Empty;
            ConsultationsExportFile ConsultationsExportFiles = new ConsultationsExportFile();
            ConsultationsExportFiles.GenerateFile(Global.Company.CompanyId);
        }
        private void GridViewServiceProvider_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (GridViewServiceProvider.Rows.Count > 0)
            {
                GridViewServiceProvider.Rows[e.RowIndex].Cells[2].ReadOnly = false;
                GridViewServiceProvider.BeginEdit(true);

            }
        }
        private void GridViewServiceProvider_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (GridViewServiceProvider.CurrentCell.ColumnIndex == 2 && e.Control is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }
    }
}
