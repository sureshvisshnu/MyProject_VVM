using fa;
using fa.api.Accounting;
using fa.model.Accounting.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Fa.views.common
{
    public partial class FormSelectAccount : Form
    {
        public Account? SelectedAccount { get; private set; }
        public bool IsAllGeneralAccount { get; set; } = false;
        public string? PreselectedAccountCode { get; set; }

        private List<Account>? _allAccounts;
        private List<Account>? _filteredAccounts = new List<Account>();

        public FormSelectAccount()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            AccountSearchTextBox.PlaceholderText = "Search by code or name...";
            AccountSearchTextBox.TextChanged += AccountSearchTextBox_TextChanged!;
            AccountSearchTextBox.KeyDown += AccountSearchTextBox_KeyDown!;

            AccountsListBox.DisplayMember = "DisplayText";
            AccountsListBox.ValueMember = "Id";
            AccountsListBox.SelectionMode = SelectionMode.One;
            AccountsListBox.DoubleClick += AccountsListBox_DoubleClick!;
            AccountsListBox.KeyDown += AccountsListBox_KeyDown!;

            BtnSelectAccountDone.Click += BtnSelectAccountDone_Click!;
            BtnSelectAccountDone.Enabled = false;
            BtnSelectAccountCancel.Click += BtnSelectAccountCancel_Click!;

            AccountsListBox.SelectedIndexChanged += AccountsListBox_SelectedIndexChanged!;
            BtnSelectAccountDone.Enabled = false;
        }

        private void FormSelectAccount_Load(object sender, EventArgs e)
        {
            BtnSelectAccountDone.Enabled = false;
            LoadAccounts();
            ApplyFilter();

            if (!string.IsNullOrEmpty(PreselectedAccountCode))
            {
                var accountToSelect = _allAccounts.FirstOrDefault(a =>
                    a.Name.Equals(PreselectedAccountCode, StringComparison.OrdinalIgnoreCase) ||
                    a.DisplayAs.Contains(PreselectedAccountCode));

                if (accountToSelect != null)
                {
                    AccountsListBox.SelectedItem = accountToSelect;
                    AccountsListBox.TopIndex = AccountsListBox.SelectedIndex;
                }
            }

            AccountSearchTextBox.Focus();
        }

        private void LoadAccounts()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (IsAllGeneralAccount)
                {
                    _allAccounts = AccountManager.Instance.GetAllAccountsByCompanyId(Global.Company.CompanyId)
                        .OrderBy(a => a.Name)
                        .ToList();
                }
                else
                {
                    var accountHelperData = AccountHelper.GetHelpData(
                        0L,              
                        true,            
                        true,             
                        false,           
                        true,            
                        string.Empty,    
                        Global.Company); 

                    _allAccounts = accountHelperData
                        .Select(a => new Account
                        {
                            Id = a.Id,
                            Name = a.Name

                        })
                        .OrderBy(a => a.Name)
                        .ToList();
                }

                this.Text = IsAllGeneralAccount ? "Select General Account" : "Select Account";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load accounts: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void ApplyFilter(string filterText = "")
        {
            try
            {
                AccountsListBox.BeginUpdate();
                AccountsListBox.Items.Clear();
                _filteredAccounts!.Clear();

                var filtered = string.IsNullOrWhiteSpace(filterText)
                    ? _allAccounts
                    : _allAccounts!.Where(a =>
                        a.Name.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (a.DisplayAs?.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (a.Discription?.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0))
                    .ToList();

                foreach (var account in filtered)
                {
                    AccountsListBox.Items.Add(account);
                    _filteredAccounts.Add(account);
                }

                BtnSelectAccountDone.Enabled = AccountsListBox.Items.Count > 0 && AccountsListBox.SelectedIndex >= 0;
            }
            finally
            {
                AccountsListBox.EndUpdate();
            }
        }

        private void AccountSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter(AccountSearchTextBox.Text.Trim());
        }

        private void AccountSearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && AccountsListBox.Items.Count > 0)
            {
                AccountsListBox.Focus();
                if (AccountsListBox.SelectedIndex == -1)
                    AccountsListBox.SelectedIndex = 0;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter && AccountsListBox.SelectedItem != null)
            {
                SelectAccountAndClose();
                e.Handled = true;
            }
        }

        private void AccountsListBox_DoubleClick(object sender, EventArgs e)
        {
            if (AccountsListBox.SelectedItem != null)
                SelectAccountAndClose();
        }

        private void AccountsListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && AccountsListBox.SelectedItem != null)
            {
                SelectAccountAndClose();
                e.Handled = true;
            }
        }

        private void AccountsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            BtnSelectAccountDone.Enabled = AccountsListBox.SelectedIndex >= 0;
        }

        private void BtnSelectAccountDone_Click(object sender, EventArgs e)
        {
            SelectAccountAndClose();
        }

        private void BtnSelectAccountCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void SelectAccountAndClose()
        {
            SelectedAccount = AccountsListBox.SelectedItem as Account;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void AccountsListBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            BtnSelectAccountDone.Enabled = AccountsListBox.SelectedIndex >= 0;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F8)
            {
                BtnSelectAccountDone.PerformClick();
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                BtnSelectAccountCancel.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
   
    // Extension class for Account to provide display text
    public static class AccountExtensions
    {
        public static string DisplayText(this Account account)
        {
            return string.IsNullOrWhiteSpace(account.DisplayAs)
                ? $"{account.Name} - {account.Discription?.Truncate(30)}"
                : account.DisplayAs;
        }

        public static string Truncate(this string value, int maxLength)
        {
            return string.IsNullOrEmpty(value)
                ? value
                : value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
        }
    }
}