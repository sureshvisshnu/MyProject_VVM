using System;
using System.Collections.Generic;
using System.Windows.Forms;
using fa.api.Accounting;
using fa.api.Hms;
using fa.model.Accounting.Masters;
using fa.model.Hms.Master;
using fa.views.account.masters;
using fa.views.employee;

namespace fa.views.common
{

    public enum FormAccountSearchGridIndex
    {
        NAME, TYPE, ADDRESS, PHONE, ID, PATIENT
    }

    public partial class FormAccountSearch : Form
    {
        public bool IncludeCustomers { get; set; }
        public bool IncludeSuppliers { get; set; }
        public bool IncludeGeneralAccounts { get; set; }
        public bool IncludeEmployees { get; set; }
        public long AccountGroupForHelpId { get; set; }
        public bool FromSales {  get; set; }

        public static string SearchOutput = "No Account found!";
        public static string NoAccountfoundErrorMsg = "You do not have any Account, Please add a Account!";
        public static string ChooseAccountErrorMsg = "Please choose Account";
        public static string EnterAccountNameErrorMsg = "Please enter Account Name";
        public static string DeleteAccountErrorMsg = "Your choosen account is remove, Please press Go button then choose Account";
        public bool isFromSale = false;

        public FormAccountSearch()
        {
            InitializeComponent();
        }

        FormBase parent = null!;
        public FormAccountSearch(Object sender)
        {
            parent = (FormBase)sender;
            InitializeComponent();
        }
        private void FormAccountSearch_Load(object sender, EventArgs e)
        {
            ResetForm();
            toolStrip.Select();
            SendKeys.Send("{tab}");
        }
        private void ResetForm()
        {
            Cursor.Current = Cursors.WaitCursor;
            NewAccountSetup();
            TextBoxSearchAccount.ResetText();
            BtnSelect.Enabled = false;
            isFromSale = FromSales ? true : false;
            loadAccounts();
            TextBoxSearchAccount.Select();
            Cursor.Current = Cursors.Default;
        }
        private void NewAccountSetup()
        {
            customerToolStripMenuItem.Enabled = customerToolStripMenuItem.Visible = IncludeCustomers;
            supplierToolStripMenuItem.Enabled = supplierToolStripMenuItem.Visible = IncludeSuppliers;
            accountToolStripMenuItem.Enabled = accountToolStripMenuItem.Visible = IncludeGeneralAccounts;
            employeeToolStripMenuItem.Enabled = employeeToolStripMenuItem.Visible = IncludeEmployees;

        }
        private void loadAccounts()
        {
            GridViewAccountSearch.Rows.Clear();
            string FilterString = TextBoxSearchAccount.Text.Trim();
            BtnSelect.Enabled = false;
            List<AccountHelperData> Accounts = AccountHelper.GetHelpData(0, IncludeCustomers, IncludeSuppliers, IncludeGeneralAccounts, IncludeEmployees, FilterString, Global.Company);
            IList<Patient> patients = new List<Patient>();
            if (Accounts.Count > 0)
            {
                foreach (AccountHelperData Account in Accounts)
                {
                    int row = GridViewAccountSearch.Rows.Add();
                    GridViewAccountSearch.Rows[row].Cells[(int)FormAccountSearchGridIndex.NAME].Value = Account.Name;
                    GridViewAccountSearch.Rows[row].Cells[(int)FormAccountSearchGridIndex.TYPE].Value = Account.GroupName;
                    GridViewAccountSearch.Rows[row].Cells[(int)FormAccountSearchGridIndex.ID].Value = Account.Id;
                    GridViewAccountSearch.Rows[row].Cells[(int)FormAccountSearchGridIndex.PATIENT].Value = false;
                    if (Account.Type == AccountType.CUSTOMER || Account.Type == AccountType.SUPPLIER)
                    {
                        GridViewAccountSearch.Rows[row].Cells[(int)FormAccountSearchGridIndex.ADDRESS].Value = Account.Address;
                        GridViewAccountSearch.Rows[row].Cells[(int)FormAccountSearchGridIndex.PHONE].Value = Account.Phone;
                    }
                }
                BtnSelect.Enabled = true;
            }
            if (Global.Company.BusinessType == BuisnessType.Hospital && isFromSale)
            {
                patients = PatientManager.Instance.ListAllPatientWithSearchString(Global.Company.CompanyId, FilterString);
                if (patients.Count > 0)
                {
                    Customer PatientPurchaseAccount = null!;
                    if (Global.Company.PatientPurchaseAccountId != null)
                    {
                        PatientPurchaseAccount = CustomerManager.Instance.GetCustomerById((long)Global.Company.PatientPurchaseAccountId!);
                    }
                    foreach (var patient in patients)
                    {
                        int row = GridViewAccountSearch.Rows.Add();
                        GridViewAccountSearch.Rows[row].Cells[(int)FormAccountSearchGridIndex.NAME].Value = patient.Name;
                        GridViewAccountSearch.Rows[row].Cells[(int)FormAccountSearchGridIndex.TYPE].Value = PatientPurchaseAccount != null ? PatientPurchaseAccount?.AccountGroup?.Name?? "" : "";
                        GridViewAccountSearch.Rows[row].Cells[(int)FormAccountSearchGridIndex.ID].Value = patient.Id;
                        GridViewAccountSearch.Rows[row].Cells[(int)FormAccountSearchGridIndex.ADDRESS].Value = patient.PatientNumber + "\n" + patient.Address.FullAddress;
                        GridViewAccountSearch.Rows[row].Cells[(int)FormAccountSearchGridIndex.PHONE].Value = patient!.ContactInfo!.Mobile != "" ? patient.ContactInfo.Mobile : patient?.ContactInfo?.Phone ?? "";
                        GridViewAccountSearch.Rows[row].Cells[(int)FormAccountSearchGridIndex.PATIENT].Value = true;
                    }
                    BtnSelect.Enabled = true;
                }
            }
            if (Accounts.Count == 0 && patients.Count == 0)
            {
                SearchErrorMsg.Text = "No information found...";
                BtnSelect.Enabled = false;
            }
        }
        
        private void BtnSearchAccount_Click(object sender, EventArgs e)
        {
            SearchErrorMsg.Text = "";
            if (!string.IsNullOrEmpty(TextBoxSearchAccount.Text.Trim()))
            {
                loadAccounts();
            }
            else
            {
                SearchErrorMsg.Text = EnterAccountNameErrorMsg;
            }
        }
        private void FormAccountSearch_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            TextBoxSearchAccount.ResetText();
            BtnSelect.Enabled = false;
            loadAccounts();
            TextBoxSearchAccount.Select();
            Cursor.Current = Cursors.Default;
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (GridViewAccountSearch.Rows.Count > 0)
            {
                if (GridViewAccountSearch.CurrentRow.Index > -1)
                {
                    //if (AccountManager.Instance.GetAccountById(long.Parse(GridViewAccountSearch.CurrentRow.Cells[4].Value.ToString())) != null)
                    //{
                    parent.AccountIdTransport.ResetText();
                    parent.AccountIdTransport.Text = GridViewAccountSearch.CurrentRow.Cells[4].Value.ToString();
                    parent.checkBoxIsPatient.Checked = (bool)GridViewAccountSearch.CurrentRow.Cells[5].Value;
                    this.Close();
                    //}
                    //else
                    //{
                    //    SearchErrorMsg.Text = DeleteAccountErrorMsg;
                    //}
                }
                else
                {
                    SearchErrorMsg.Text = ChooseAccountErrorMsg;
                }
            }
        }

        private void GridViewAccountSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                BtnSelect_Click(sender, e);
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSelect.PerformClick();
            }
            else if (keyData == (Keys.F3))
            {
                accountToolStripMenuItem.PerformClick();
            }
            else if (keyData == (Keys.F4))
            {
                supplierToolStripMenuItem.PerformClick();
            }
            else if (keyData == (Keys.F5))
            {
                customerToolStripMenuItem.PerformClick();
            }
            else if (keyData == (Keys.F6))
            {
                employeeToolStripMenuItem.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return true;
            }
            try
            {
                if (GridFocus)
                {
                    if (keyData == (Keys.Tab) && GridViewAccountSearch.CurrentRow.Index > -1)
                    {
                        if (GridViewAccountSearch.CurrentCell.RowIndex != GridViewAccountSearch.Rows.Count - 1)
                        {
                            GridViewAccountSearch.CurrentCell = GridViewAccountSearch[0, GridViewAccountSearch.CurrentCell.RowIndex + 1];
                        }
                        else
                        {
                            if (GridViewAccountSearch.CurrentCell.ColumnIndex == 2)
                            {
                                SendKeys.Send("{+tab}");
                            }
                            BtnSelect.Select();
                        }

                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewAccountSearch.CurrentRow.Index > -1)
                    {
                        if (GridViewAccountSearch.CurrentRow.Index != 0)
                        {
                            GridViewAccountSearch.CurrentCell = GridViewAccountSearch[0, GridViewAccountSearch.CurrentCell.RowIndex];
                        }
                        else
                        {
                            TextBoxSearchAccount.Select();
                            if (GridViewAccountSearch.CurrentCell.ColumnIndex == 0)
                            {
                                TextBoxSearchAccount.Focus();
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private bool GridFocus = false;
        private void GridViewAccountSearch_Leave(object sender, EventArgs e)
        {
            GridFocus = false;
        }

        private void GridViewAccountSearch_Enter(object sender, EventArgs e)
        {
            GridFocus = true;
        }

        private void accountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormGeneralAccounts FormGeneralAccounts = new FormGeneralAccounts(this);
            FormGeneralAccounts.CreateAccountOnLoad = true;
            FormGeneralAccounts.ShowDialog(this);
            loadAccounts();
            Cursor.Current = Cursors.Default;
        }

        private void supplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormSupplier FormSupplier = new FormSupplier(this);
            FormSupplier.CreateSupplierOnLoad = true;
            FormSupplier.ShowDialog(this);
            loadAccounts();
            Cursor.Current = Cursors.Default;
        }

        private void customerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormCustomers FormCustomers = new FormCustomers(this);
            FormCustomers.CreateCustomerOnLoad = true;
            FormCustomers.ShowDialog(this);
            loadAccounts();
            Cursor.Current = Cursors.Default;
        }
        private void employeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormEmployee FormEmployee = new FormEmployee(this);
            FormEmployee.CreateEmployeeOnLoad = true;
            FormEmployee.ShowDialog(this);
            loadAccounts();
            Cursor.Current = Cursors.Default;
        }
        private void GridViewAccountSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == Convert.ToChar(Keys.Enter))
            {
                BtnSelect_Click(sender, e);
            }
        }

        private void TextBoxSearchAccount_TextChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (string.IsNullOrEmpty(TextBoxSearchAccount.Text))
            {
                loadAccounts();
            }
            if(GridViewAccountSearch.RowCount > 0)
            {
                SearchErrorMsg.Text = "";
            }
            Cursor.Current = Cursors.Default;
        }

        private void BtnSelect_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxSearchAccount.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewAccountSearch.Rows.Count > 0)
                {
                    GridViewAccountSearch.Select();
                    GridViewAccountSearch.CurrentCell = GridViewAccountSearch[0, 0];
                }
                else
                {
                    TextBoxSearchAccount.Select();
                }
            }
        }

        private void TextBoxSearchAccount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearchAccount_Click(sender, e);
            }
            if (e.KeyCode == Keys.Down)
            {
                if (GridViewAccountSearch.Rows.Count > 0)
                {
                    GridViewAccountSearch.Select();
                    GridViewAccountSearch.CurrentCell = GridViewAccountSearch[0, 0];
                }
            }
        }

        private void GridViewAccountSearch_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


    }
}
