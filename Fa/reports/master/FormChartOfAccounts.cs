using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.model.Accounting.Masters;
using fa.api.Accounting;
using fa.views.account.masters;
using fa.report.accounting.master;
using fa.views.utils.Report.Account;
using fa.views.employee;
using fa.report.common;
using fa.views.controls.grid;
using fa.views.sales;
namespace fa.reports.master
{
    public enum ChartOfAccountColumn
    {
        NAME, DESCRIPTION, TYPE, DETAILS_TYPE, BALANCE, CRDR, ID, PARENT_ID, ROW_TYPE, EDIT
    }
    public partial class FormChartOfAccounts : Form
    {
        AccountManager AccountManager = null;
        ChartOfAccount ChartOfAccount = null;
        public FormChartOfAccounts()
        {
            InitializeComponent();
            AccountManager = AccountManager.Instance;
            ChartOfAccount = new ChartOfAccount();
            ChartOfAccountsGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ChartOfAccountsGridView1.MultiSelect = false;
        }
        private void ChartOfAccountsGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int Index;
            if (e.RowIndex > -1 && e.ColumnIndex == (int)ChartOfAccountColumn.EDIT)
            {
                Index = e.RowIndex;
                if (ChartOfAccountsGridView1.CurrentRow.Cells[(int)ChartOfAccountColumn.ROW_TYPE].Value.ToString() == "0")
                {
                    FormGeneralAccounts FormGeneralAccounts = new FormGeneralAccounts(this);
                    FormGeneralAccounts.CreateAccountOnLoad = true;
                    FormGeneralAccounts.ParentId = (ChartOfAccountsGridView1.CurrentRow.Cells[(int)ChartOfAccountColumn.PARENT_ID].Value != null) ? ChartOfAccountsGridView1.CurrentRow.Cells[(int)ChartOfAccountColumn.PARENT_ID].Value.ToString() : "";
                    FormGeneralAccounts.Id = ChartOfAccountsGridView1.CurrentRow.Cells[(int)ChartOfAccountColumn.ID].Value.ToString();
                    FormGeneralAccounts.ShowDialog(this);
                }
                else if (ChartOfAccountsGridView1.CurrentRow.Cells[(int)ChartOfAccountColumn.ROW_TYPE].Value.ToString() == "1")
                {
                    FormCustomers FormCustomers = new FormCustomers(this);
                    FormCustomers.CreateCustomerOnLoad = true;
                    FormCustomers.ParentId = (ChartOfAccountsGridView1.CurrentRow.Cells[(int)ChartOfAccountColumn.PARENT_ID].Value != null) ? ChartOfAccountsGridView1.CurrentRow.Cells[(int)ChartOfAccountColumn.PARENT_ID].Value.ToString() : "";
                    FormCustomers.Id = ChartOfAccountsGridView1.CurrentRow.Cells[(int)ChartOfAccountColumn.ID].Value.ToString();
                    FormCustomers.ShowDialog(this);
                }
                else if (ChartOfAccountsGridView1.CurrentRow.Cells[(int)ChartOfAccountColumn.ROW_TYPE].Value.ToString() == "2")
                {
                    FormSupplier FormSupplier = new FormSupplier(this);
                    FormSupplier.CreateSupplierOnLoad = true;
                    FormSupplier.ParentId = (ChartOfAccountsGridView1.CurrentRow.Cells[(int)ChartOfAccountColumn.PARENT_ID].Value != null) ? ChartOfAccountsGridView1.CurrentRow.Cells[(int)ChartOfAccountColumn.PARENT_ID].Value.ToString() : "";
                    FormSupplier.Id = ChartOfAccountsGridView1.CurrentRow.Cells[(int)ChartOfAccountColumn.ID].Value.ToString();
                    FormSupplier.ShowDialog(this);
                }
                else if (ChartOfAccountsGridView1.CurrentRow.Cells[(int)ChartOfAccountColumn.ROW_TYPE].Value.ToString() == "3")
                {
                    FormEmployee FormEmployee = new FormEmployee(this);
                    FormEmployee.CreateEmployeeOnLoad = true;
                    FormEmployee.ParentId = (ChartOfAccountsGridView1.CurrentRow.Cells[(int)ChartOfAccountColumn.PARENT_ID].Value != null) ? ChartOfAccountsGridView1.CurrentRow.Cells[(int)ChartOfAccountColumn.PARENT_ID].Value.ToString() : "";
                    FormEmployee.Id = ChartOfAccountsGridView1.CurrentRow.Cells[(int)ChartOfAccountColumn.ID].Value.ToString();
                    FormEmployee.ShowDialog(this);
                }
                LoadAccount();
                if (ChartOfAccountsGridView1.Rows.Count > 0)
                {
                    ChartOfAccountsGridView1.CurrentCell = ChartOfAccountsGridView1[0, Index];
                    ChartOfAccountsGridView1.Rows[Index].Selected = true;
                }
            }
        }
        private void LoadAccount()
        {
            //load the data
            ChartOfAccountsGridView1.Rows.Clear();
            RptChartOfAccount RptChartOfAccount = new RptChartOfAccount();
            RptChartOfAccount.Company = Global.Company;
            RptChartOfAccount.SearchText = TextBoxSearch.Text.Trim();
            RptChartOfAccount.GenerateReport();
            if (AccountFilerComboBox1.SelectedIndex > -1)
            {
                IList<RptChartOfAccountLineItem> Accounts = new List<RptChartOfAccountLineItem>();
                if (AccountFilerComboBox1.SelectedIndex == 0)
                {
                    Accounts = RptChartOfAccount.LineItems;
                }
                else if (AccountFilerComboBox1.SelectedIndex == 1)
                {
                    Accounts = RptChartOfAccount.LineItems.Where(x => x.AccountTypeId == AccountType.ACCOUNT).ToList();
                }
                else if (AccountFilerComboBox1.SelectedIndex == 2)
                {
                    Accounts = RptChartOfAccount.LineItems.Where(item => item.AccountTypeId == AccountType.SUPPLIER).ToList();
                }
                else if (AccountFilerComboBox1.SelectedIndex == 3)
                {
                    Accounts = RptChartOfAccount.LineItems.Where(item => item.AccountTypeId == AccountType.CUSTOMER).ToList();
                }
                else if (AccountFilerComboBox1.SelectedIndex == 4)
                {
                    Accounts = RptChartOfAccount.LineItems.Where(item => item.AccountTypeId == AccountType.EMPLOYEE).ToList();
                }
                foreach (RptChartOfAccountLineItem Account in Accounts)
                {
                    int i = ChartOfAccountsGridView1.Rows.Add();

                    ChartOfAccountsGridView1.Rows[i].Cells[(int)ChartOfAccountColumn.NAME].Value = Account.AccName;
                    ChartOfAccountsGridView1.Rows[i].Cells[(int)ChartOfAccountColumn.DESCRIPTION].Value = Account.AccDescription;
                    ChartOfAccountsGridView1.Rows[i].Cells[(int)ChartOfAccountColumn.TYPE].Value = Account.AccType;
                    ChartOfAccountsGridView1.Rows[i].Cells[(int)ChartOfAccountColumn.DETAILS_TYPE].Value = Account.AccDetailType;
                    ChartOfAccountsGridView1.Rows[i].Cells[(int)ChartOfAccountColumn.BALANCE].Value = Math.Abs(Account.AccBalance);
                    ChartOfAccountsGridView1.Rows[i].Cells[(int)ChartOfAccountColumn.ID].Value = Account.Id;
                    ChartOfAccountsGridView1.Rows[i].Cells[(int)ChartOfAccountColumn.PARENT_ID].Value = Account.ParentAccountId;
                    ChartOfAccountsGridView1.Rows[i].Cells[(int)ChartOfAccountColumn.ROW_TYPE].Value = (int)Account.AccountTypeId;
                    ChartOfAccountsGridView1.Rows[i].Cells[(int)ChartOfAccountColumn.CRDR].Value = (Account.BalanceType == CrDr.DR ? "DR" : "CR");
                }
                if (ChartOfAccountsGridView1.Rows.Count > 0)
                {
                    EnableButton(true);
                }
                else
                {
                    EnableButton(false);
                }
                ChartOfAccountsGridView1.ReadOnly = true;
            }
        }
        private void EnableButton(bool Enable)
        {
            BtnPrint.Enabled = Enable;
            BtnSave.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
        }
        private void FormChartOfAccounts_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void ResetForm()
        {
            ChartofAccountErrorMsg.Text = string.Empty;
            TextBoxSearch.Clear();
            AccountFilerComboBox1.SelectedIndex = 0;
            LoadAccount();
            TextBoxSearch.TextBox.Select();
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)ChartOfAccountsGridView1.Columns["Balance"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
        }
        private void ChartOfAccountsGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == (int)ChartOfAccountColumn.EDIT && e.RowIndex > -1)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                var w = imageList1.Images[0].Width;
                var h = imageList1.Images[0].Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;
                e.Graphics.DrawImage(imageList1.Images[0], new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            List<string> headingTest = new List<string>();
            headingTest.Add(this.Text);
            ChartOfAccount.ExportToFileOrPrint(ChartOfAccountsGridView1, headingTest, "Chart_Of_Account", "pdf", false);
            Cursor.Current = Cursors.Default;
        }
        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            List<string> headingTest = new List<string>();
            headingTest.Add(this.Text);
            ChartOfAccount.ExportToFileOrPrint(ChartOfAccountsGridView1, headingTest, "Chart_Of_Account", "pdf", true);
            Cursor.Current = Cursors.Default;
        }
        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (ChartOfAccountsGridView1.Rows.Count > 0)
                {
                    ChartOfAccountsGridView1.Select();
                    ChartOfAccountsGridView1.CurrentCell = ChartOfAccountsGridView1[0, 0];
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                LoadAccount();
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (keyData == (Keys.F2))
                {
                    SearchBtn.PerformClick();
                    return true;
                }
                if (keyData == (Keys.F9))
                {
                    BtnPrint.PerformClick();
                    return true;
                }
                if (keyData == (Keys.F8))
                {
                    BtnSave.PerformClick();
                    return true;
                }
                if (keyData == (Keys.F10))
                {
                    BtnExit.PerformClick();
                }
                else if (keyData == (Keys.Escape))
                {
                    BtnReset.PerformClick();
                }
                if (ChartOfAccountsGridView1.Focused)
                {
                    if (keyData == (Keys.Tab) && ChartOfAccountsGridView1.CurrentRow.Index > -1)
                    {
                        if (ChartOfAccountsGridView1.CurrentCell.RowIndex != ChartOfAccountsGridView1.Rows.Count - 1)
                        {
                            ChartOfAccountsGridView1.CurrentCell = ChartOfAccountsGridView1[0, ChartOfAccountsGridView1.CurrentCell.RowIndex + 1];
                        }
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && ChartOfAccountsGridView1.CurrentRow.Index > -1)
                    {
                        if (ChartOfAccountsGridView1.CurrentRow.Index != 0)
                        {
                            ChartOfAccountsGridView1.CurrentCell = ChartOfAccountsGridView1[0, ChartOfAccountsGridView1.CurrentCell.RowIndex - 1];
                        }
                        else
                        {
                            TextBoxSearch.Focus();
                        }
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
        private void SearchBtn_Click(object sender, EventArgs e)
        {
            ChartofAccountErrorMsg.Text = string.Empty;
            if (AccountFilerComboBox1.SelectedIndex < 0)
            {
                ChartofAccountErrorMsg.Text = "Please select account filter.";
                AccountFilerComboBox1.Select();
            }
            else
            {
                LoadAccount();
            }
        }

        private void AccountFilerComboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                SearchBtn_Click(sender, e);
            }
        }

        private void toolStrip_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnSave.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnSave.Select();
            }
        }

        private void BtnExit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxSearch.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnReset.Select();
            }
        }

        private void BtnSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnPrint.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxSearch.Focus();
            }
        }
    }
}
