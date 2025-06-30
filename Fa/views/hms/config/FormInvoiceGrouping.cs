using fa.api.Hms;
using fa.libraries.utils;
using fa.model.Hms.common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.hms.config
{
    public partial class FormInvoiceGrouping : FormPatientBase
    {
        public FormInvoiceGrouping()
        {
            InitializeComponent();
            excludedObjects = new string[] { "ListBoxInvoiceGroup" };
        }
        public static string EnterNameErrorMsg = "Please enter name";
        public static string UniqueNameErrorMsg = "Invoice group {0} already exists";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string DeleteConfirmText = "Do you want to delete the invoice group {0}?";
        public static string DeleteErrorText = "Error Deleting the invoice group!, Please retry";

        private void FormInvoiceGrouping_Load(object sender, EventArgs e)
        {
            ResetForm();
            LoadTransactionTypeGroupMappings();
            LoadInvoiceGroup();
            this.formIsDirty = false;
        }
        private void LoadInvoiceGroup()
        {
            ListBoxInvoiceGroup.Items.Clear();
            IList<PatientLedgerTransactionTypeGroup> TransactionTypeGroup = HospitalInvoiceGroupManager.Instance.ListAllPatientLedgerTransactionTypeGroup(Global.Company.CompanyId);
            foreach (var Group in TransactionTypeGroup)
            {
                ListBoxInvoiceGroup.Items.Add(Group);
            }
            if (ListBoxInvoiceGroup.Items.Count > 0)
            {
                ListBoxInvoiceGroup.SelectedIndex = 0;
            }
        }
        private PatientLedgerTransactionTypeGroup GetTransactionTypeGroup()
        {
            PatientLedgerTransactionTypeGroup lTransactionTypeGroup = (ListBoxInvoiceGroup.SelectedIndex > -1 ? (PatientLedgerTransactionTypeGroup)ListBoxInvoiceGroup.Items[ListBoxInvoiceGroup.SelectedIndex] : null);
            return (lTransactionTypeGroup != null ? HospitalInvoiceGroupManager.Instance.GetPatientLedgerTransactionTypeGroupById(lTransactionTypeGroup.Id) : null);
        }
        private void LoadTransactionTypeGroup()
        {
            PatientLedgerTransactionTypeGroup TransactionTypeGroupFromDB = GetTransactionTypeGroup();
            if (TransactionTypeGroupFromDB != null)
            {
                TextBoxInvoiceGroupName.Text = TransactionTypeGroupFromDB.Name;
                TextBoxInvoiceGroupDescription.Text = TransactionTypeGroupFromDB.Description;
                TextBoxInvoiceGroupId.Text = TransactionTypeGroupFromDB.Id.ToString();
                LoadTransactionTypeGroupMappings();
                if (TransactionTypeGroupFromDB.PatientLedgerTransactionTypeGroupMappings.Count > 0)
                {
                    foreach (var Mappings in TransactionTypeGroupFromDB.PatientLedgerTransactionTypeGroupMappings)
                    {
                        CheckedListBoxInvoiceGroup.Items.Add(ComboUtils.GetTypeDescription(Mappings.TransactionType), true);
                    }
                }
                this.formIsDirty = false;
            }
            else
            {
                DisplaySystemError("Somthing went wrong, the selected transaction type is not valid.");
                return;
            }
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            this.formIsDirty = false;
            LoadTransactionTypeGroup();
            return;
        }
        private void LoadTransactionTypeGroupMappings()
        {
            CheckedListBoxInvoiceGroup.Items.Clear();
            IList<PatientLedgerTransactionTypeGroupMapping> GroupMapping = HospitalInvoiceGroupManager.Instance.ListAllPatientLedgerTransactionTypeGroupMapping(Global.Company.CompanyId);

            List<TransactionType> Types = Enum.GetValues(typeof(TransactionType)).Cast<TransactionType>().ToList();
            foreach (var Type in Types)
            {
                if (GroupMapping.FirstOrDefault(x => x.TransactionType == Type) == null && Type != TransactionType.PAYMENT && Type != TransactionType.WAIVER)
                {
                    CheckedListBoxInvoiceGroup.Items.Add(ComboUtils.GetTypeDescription(Type));
                }
            }
            if (CheckedListBoxInvoiceGroup.Items.Count > 0)
            {
                CheckedListBoxInvoiceGroup.SelectedIndex = 0;
            }
        }
       

        private PatientLedgerTransactionTypeGroup GetTransactionTypeGroupFromForm()
        {
            PatientLedgerTransactionTypeGroup lTransactionTypeGroup = new PatientLedgerTransactionTypeGroup();
            if (!string.IsNullOrEmpty(TextBoxInvoiceGroupId.Text))
            {
                lTransactionTypeGroup.Id = Convert.ToInt64(TextBoxInvoiceGroupId.Text);
            }
            else
            {
                lTransactionTypeGroup.Id = 0L;
            }
            lTransactionTypeGroup.CompanyId = Global.Company.CompanyId;
            lTransactionTypeGroup.Name = TextBoxInvoiceGroupName.Text.Trim();
            lTransactionTypeGroup.Description = TextBoxInvoiceGroupDescription.Text.Trim();

            return lTransactionTypeGroup;
        }
        private void BtnInvoiceGroupNew_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            LoadInvoiceGroup();
            ResetForm();
            LoadTransactionTypeGroupMappings();
            EnableForm(true);
            TextBoxInvoiceGroupName.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }

        private void BtnInvoiceGroupDelete_Click(object sender, EventArgs e)
        {
            PatientLedgerTransactionTypeGroup TransactionTypeGroupInfo = GetTransactionTypeGroup();
            if (TransactionTypeGroupInfo != null)
            {
                DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, TransactionTypeGroupInfo.Name), "Delete Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Boolean Deleted = HospitalInvoiceGroupManager.Instance.DeletePatientLedgerTransactionTypeGroup(TransactionTypeGroupInfo.Id);
                    if (Deleted)
                    {
                        ResetForm();
                        LoadTransactionTypeGroupMappings();
                        LoadInvoiceGroup();
                        EnableForm(false);
                        this.formIsDirty = false;
                    }
                    else
                    {
                        InvoiceGroupErrorMsg.Text = DeleteErrorText;
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            else
            {
                DisplaySystemError("Somthing went wrong, the selected transaction type is not valid.");
                return;
            }
        }

        private void BtnInvoiceGroupEdit_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            LoadTransactionTypeGroup();
            EnableForm(true);
            TextBoxInvoiceGroupName.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }

        private void BtnInvoiceGroupCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxInvoiceGroupName.Select();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            LoadTransactionTypeGroupMappings();
            LoadInvoiceGroup();
            EnableForm(false);
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private bool ValidateForm()
        {
            InvoiceGroupErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxInvoiceGroupName.Text.Trim()))
            {
                InvoiceGroupErrorMsg.Text = EnterNameErrorMsg;
                TextBoxInvoiceGroupName.Select();
                return false;
            }
            return true;
        }
        private void BtnInvoiceGroupSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Cursor.Current = Cursors.WaitCursor;
                PatientLedgerTransactionTypeGroup lTransactionTypeGroup = GetTransactionTypeGroupFromForm();
                if (HospitalInvoiceGroupManager.Instance.FindNameUnique(lTransactionTypeGroup))
                {
                    if (CheckedListBoxInvoiceGroup.Items.Count > 0)
                    {
                        lTransactionTypeGroup.PatientLedgerTransactionTypeGroupMappings = new List<PatientLedgerTransactionTypeGroupMapping>();
                        foreach (var Type in CheckedListBoxInvoiceGroup.CheckedItems)
                        {
                            PatientLedgerTransactionTypeGroupMapping Mapping = new PatientLedgerTransactionTypeGroupMapping();
                            Mapping.TransactionType = ComboUtils.GetType(Type.ToString());
                            Mapping.CompanyId = Global.Company.CompanyId;
                            if (lTransactionTypeGroup.Id != 0)
                            {
                                Mapping.PatientLedgerTransactionTypeGroupId = lTransactionTypeGroup.Id;
                            }
                            lTransactionTypeGroup.PatientLedgerTransactionTypeGroupMappings.Add(Mapping);
                        }
                    }
                    if (lTransactionTypeGroup.Id == 0)
                    {
                        //if (CheckedListBoxInvoiceGroup.Items.Count > 0)
                        //{
                        //    lTransactionTypeGroup.PatientLedgerTransactionTypeGroupMappings = new List<PatientLedgerTransactionTypeGroupMapping>();
                        //    foreach (TransactionType Type in CheckedListBoxInvoiceGroup.CheckedItems)
                        //    {
                        //        PatientLedgerTransactionTypeGroupMapping Mapping = new PatientLedgerTransactionTypeGroupMapping();
                        //        Mapping.TransactionType = Type;
                        //        Mapping.CompanyId = Global.Company.CompanyId;
                        //        lTransactionTypeGroup.PatientLedgerTransactionTypeGroupMappings.Add(Mapping);
                        //    }
                        //}
                        PatientLedgerTransactionTypeGroup lTransactionTypeGroupFromDB = HospitalInvoiceGroupManager.Instance.AddPatientLedgerTransactionTypeGroup(lTransactionTypeGroup);

                        ResetForm();
                        LoadTransactionTypeGroupMappings();
                        LoadInvoiceGroup();
                        ListBoxInvoiceGroup.SelectedIndex = ListBoxInvoiceGroup.FindStringExact(lTransactionTypeGroupFromDB.Name);
                        EnableForm(false);

                    }
                    else
                    {
                        PatientLedgerTransactionTypeGroup lUserById = HospitalInvoiceGroupManager.Instance.GetPatientLedgerTransactionTypeGroupById(lTransactionTypeGroup.Id);
                        if (lUserById != null)
                        {

                            //if (CheckedListBoxInvoiceGroup.Items.Count > 0)
                            //{
                            //    lTransactionTypeGroup.PatientLedgerTransactionTypeGroupMappings = new List<PatientLedgerTransactionTypeGroupMapping>();
                            //    foreach (TransactionType Type in CheckedListBoxInvoiceGroup.CheckedItems)
                            //    {
                            //        PatientLedgerTransactionTypeGroupMapping Mapping = new PatientLedgerTransactionTypeGroupMapping();
                            //        Mapping.TransactionType = Type;
                            //        Mapping.CompanyId = Global.Company.CompanyId;
                            //        Mapping.PatientLedgerTransactionTypeGroupId = lTransactionTypeGroup.Id;
                            //        lTransactionTypeGroup.PatientLedgerTransactionTypeGroupMappings.Add(Mapping);
                            //    }
                            //}
                            PatientLedgerTransactionTypeGroup lUserFromDB = HospitalInvoiceGroupManager.Instance.UpdatePatientLedgerTransactionTypeGroup(lTransactionTypeGroup);
                            ResetForm();
                            LoadTransactionTypeGroupMappings();
                            LoadInvoiceGroup();
                            ListBoxInvoiceGroup.SelectedIndex = ListBoxInvoiceGroup.FindStringExact(lUserFromDB.Name);
                            EnableForm(false);

                        }
                        else
                        {
                            DisplaySystemError("Somthing went wrong, the selected invoice group is not valid.");
                            return;
                        }

                    }
                    this.formIsDirty = false;

                }
                else
                {
                    InvoiceGroupErrorMsg.Text = string.Format(UniqueNameErrorMsg, lTransactionTypeGroup.Name);
                    TextBoxInvoiceGroupName.Select();
                }
                Cursor.Current = Cursors.Default;
            }
        }
        private void ResetForm()
        {
            InvoiceGroupErrorMsg.Text = string.Empty;
            TextBoxInvoiceGroupDescription.ResetText();
            TextBoxInvoiceGroupName.ResetText();
            TextBoxInvoiceGroupId.ResetText();
        }
        private void EnableForm(Boolean enable)
        {
            if (ListBoxInvoiceGroup.Items.Count > 0)
            {
                ListBoxInvoiceGroup.Enabled = !enable;
            }
            else
            {
                ListBoxInvoiceGroup.Enabled = false;
                BtnInvoiceGroupNew.Select();
            }
            TextBoxInvoiceGroupDescription.ReadOnly = !enable;
            TextBoxInvoiceGroupName.ReadOnly = !enable;
            TextBoxInvoiceGroupDescription.TabStop = enable;
            TextBoxInvoiceGroupName.TabStop = enable;

            CheckedListBoxInvoiceGroup.Enabled = enable;
            if (!enable)
            {
                BtnInvoiceGroupCancel.Enabled = enable;
                if (ListBoxInvoiceGroup.SelectedIndex < 0)
                {
                    BtnInvoiceGroupDelete.Enabled = enable;
                    BtnInvoiceGroupEdit.Enabled = enable;
                }
                else
                {
                    BtnInvoiceGroupDelete.Enabled = !enable;
                    BtnInvoiceGroupEdit.Enabled = !enable;
                }
                BtnInvoiceGroupNew.Enabled = !enable;
                BtnInvoiceGroupSave.Enabled = enable;
            }
            else
            {
                BtnInvoiceGroupCancel.Enabled = enable;
                BtnInvoiceGroupDelete.Enabled = !enable;
                BtnInvoiceGroupEdit.Enabled = !enable;
                BtnInvoiceGroupNew.Enabled = !enable;
                BtnInvoiceGroupSave.Enabled = enable;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnInvoiceGroupNew.PerformClick();
            }
            if (keyData == (Keys.F4))
            {
                BtnInvoiceGroupDelete.PerformClick();
            }
            if (keyData == (Keys.F7))
            {
                BtnInvoiceGroupEdit.PerformClick();
            }
            if (keyData == (Keys.F8))
            {
                BtnInvoiceGroupSave.PerformClick();
            }
            if (keyData == (Keys.F10))
            {
                BtnInvoiceGroupExit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnInvoiceGroupCancel.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void FormInvoiceGrouping_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxInvoiceGroupName.Select();
                    e.Cancel = true;
                }
            }
        }

        private void ListBoxInvoiceGroup_SelectedIndexChanged(object sender, EventArgs e)
        {            
            ResetForm();
            LoadTransactionTypeGroup();
            EnableForm(false);
            ListBoxInvoiceGroup.Select();
            this.formIsDirty = false;
        }

        private void BtnInvoiceGroupExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TextBoxInvoiceGroupName_TextChanged(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;
            if(!TB.Focused)
            {
                this.formIsDirty = false;
                return;
            }
        }

        private void TextBoxInvoiceGroupDescription_TextChanged(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;
            if (!TB.Focused)
            {
                this.formIsDirty = false;
                return;
            }
        }
    }
}
