using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using fa.api.Accounting;
using fa.model.Accounting.Masters;
using fa.libraries.Validation;
using fa.libraries.utils;
using fa.api.utils;
using fa.api.Log;

namespace fa.views.account.masters
{
    public partial class FormGeneralAccounts : FormBase
    {
        public static string SaveSuccessText = "Saved";
        public static string UpdateAccountOnloadText = "Update Account";
        public static string DeleteConfirmText = "Do you want to delete the Account {0}?";
        public static string DeleteErrorText = "Error deleting the account!, Please retry";
        public static string IsbranchDeleteErrorText = "Cannot delete account with child account";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want exit?";

        public static string UniqueAccountNameErrorMsg = "Account {0} already exists in company {1}";
        public static string EnterAccountNameErrorMsg = "Please enter account name";
        public static string ChooseParentErrorMsg = "Please select parent account";
        public static string EnterAsOfDateErrorMsg = "Please enter as of date";
        public static string EnterValidAsOfDateErrorMsg = "Please enter valid as of date";
        public static string ChooseAccountTypeErrorMsg = "Please select account type";
        public static string ChooseAccountDetailTypeErrorMsg = "Please choose Account Detail Type";
        public static string ChooseBalanceTypeErrorMsg = "Please choose balance Type";
        public static string CreateAccountOnloadText = "New Account";

        public bool CreateAccountOnLoad = false;
        public string ParentId;
        public string Id;

        AccountManager AccountManager = null!;
        AccountGroupManager AccountGroupManager = null!;
        KeypressValidation KeypressValidation = null!;
        CompanyManager CompanyManager = null!;
        DateValidation DateValidation = null!;
        FormBase parent = null!;

        public FormGeneralAccounts(object sender)
        {
            if (sender is FormBase)
            {
                parent = (FormBase)sender;
            }
            AccountManager = AccountManager.Instance;
            AccountGroupManager = AccountGroupManager.Instance;
            KeypressValidation = KeypressValidation.Instance;
            CompanyManager = CompanyManager.Instance;
            DateValidation = DateValidation.Instance;
            InitializeComponent();
            excludedObjects = new string[] { "TextBoxGeneralAccountSearch" };
        }
        private void FormGeneralAccounts_Load(object sender, EventArgs e)
        {
            ResetForm();
            ComboUtils.InitializeAccountCombo(ComboBoxGeneralAccountParentAccount, Global.Company.CompanyId);
            ComboUtils.InitializeAccountGroupCombo(ComboBoxGeneralAccountGroup);
            LoadAccountsWithFilter();
            //EnableForm(false);


            if (CreateAccountOnLoad)
            {
                TextBoxGeneralAccountSearch.Visible = false;
                this.Text = UpdateAccountOnloadText;
                this.BtnGeneralAccountEdit.Visible = false;
                BtnGeneralAccountCancel.Visible = false;
                BtnGeneralAccountNew.Visible = false;
                BtnGeneralAccountDelete.Visible = false;
                TreeViewGeneralAccount.Visible = false;
                groupBox1.Location = new Point(12, 12);
                BtnGeneralAccountSave.Location = new Point(groupBox1.Width - 74, groupBox1.Height + 15);
                this.Size = new Size(groupBox1.Right + 25, groupBox1.Bottom + 90);
                this.CenterToParent();
                if (Id == null)
                {
                    this.Text = CreateAccountOnloadText;
                    BtnGeneralAccountNew_Click(this, null!);
                }
                else
                {
                    TreeNode TreeNode = new TreeNode();
                    TreeNode = ParentId == "" ? TreeViewGeneralAccount.Nodes[Id] : TreeViewGeneralAccount.Nodes[ParentId].Nodes[Id];
                    TreeViewGeneralAccount.SelectedNode = TreeNode;
                    BtnGeneralAccountEdit_Click(this, null!);
                }
            }
            this.formIsDirty = false;
        }

        private void LoadAccountsWithFilter()
        {
            string FilterString = TextBoxGeneralAccountSearch.Text.Trim();
            TreeViewGeneralAccount.Nodes.Clear();
            IList<Account> Accounts = AccountManager.Instance.GetAllGeneralAccountsByCompanyId(Global.Company.CompanyId);

            if (Accounts != null)
            {
                foreach (var lAccounts in Accounts)
                {
                    if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim()) || lAccounts.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        if (lAccounts.ParentAccountId == null)
                        {
                            TreeViewGeneralAccount.Nodes.Add(lAccounts.Id.ToString(), lAccounts.Name);
                            TreeViewGeneralAccount.Nodes[lAccounts.Id.ToString()].ImageIndex = 0;
                            foreach (var llAccounts in Accounts)
                            {
                                if (lAccounts.Id == llAccounts.ParentAccountId)
                                {
                                    if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim()) || llAccounts.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                                    {
                                        TreeViewGeneralAccount.Nodes[lAccounts.Id.ToString()].Nodes.Add(llAccounts.Id.ToString(), llAccounts.Name);
                                        TreeViewGeneralAccount.Nodes[lAccounts.Id.ToString()].Nodes[llAccounts.Id.ToString()].ImageIndex = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (TreeViewGeneralAccount.Nodes.Count > 0)
            {
                if (!string.IsNullOrEmpty(TextBoxGeneralAccountSearch.Text))
                {
                    TreeViewGeneralAccount.ExpandAll();
                }
                TreeViewGeneralAccount.SelectedNode = TreeViewGeneralAccount.Nodes[0];

            }
        }
        private Account GetAccountInfo()
        {
            if (TreeViewGeneralAccount.SelectedNode != null)
            {
                Account lAccount = AccountManager.Instance.GetAccountById(long.Parse(TreeViewGeneralAccount.SelectedNode.Name));
                if (lAccount != null)
                {
                    Account AccountFromDB = AccountManager.Instance.GetAccountById(lAccount.Id);
                    return AccountFromDB;
                }
            }
            return null!;
        }
        private void LoadAccountInfo()
        {

            Account AccountFromDB = GetAccountInfo();
            if (AccountFromDB != null)
            {
                TextBoxGeneralAccountId.Text = AccountFromDB.Id.ToString();
                TextBoxGeneralAccountName.Text = AccountFromDB.Name;
                TextBoxGeneralAccountDisplayAs.Text = AccountFromDB.DisplayAs;
                TextBoxGeneralAccountDescription.Text = AccountFromDB.Discription;
                TextBoxGeneralAccountBalance.Text = Math.Abs(AccountFromDB.Balance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                DateTimePickerGeneralAccount.Date = (DateTime)DateUtils.ToDate(AccountFromDB.BalanceAsOf.Date.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                CheckBoxGeneralAccount.Checked = AccountFromDB.IsSubAccount;
                if (AccountFromDB.IsSubAccount)
                {
                    Account Account = AccountManager.GetAccountById((long)AccountFromDB.ParentAccountId!);
                    if (Account != null)
                    {
                        ComboUtils.InitializeAccountCombo(ComboBoxGeneralAccountParentAccount, Global.Company.CompanyId);
                        ComboBoxGeneralAccountParentAccount.SelectedIndex = ComboBoxGeneralAccountParentAccount.FindStringExact(Account.Name);
                    }
                }
                else
                {
                    ComboBoxGeneralAccountParentAccount.SelectedIndex = -1;
                }
                if (AccountFromDB.AccountGroup != null)
                {
                    ComboBoxGeneralAccountGroup.SelectedIndex = ComboBoxGeneralAccountGroup.FindStringExact(AccountFromDB.AccountGroup.ParentAccountGroup.Name);
                    ComboBoxGeneralAccountDetailGroup.SelectedIndex = ComboBoxGeneralAccountDetailGroup.FindStringExact(AccountFromDB.AccountGroup.Name);
                }
                //Set the CR/DR
                ComboBoxBalanceType.SelectedIndex = (int)AccountFromDB.BalanceType();
            }
            else
            {
                DisplaySystemError("Somthing went wrong, the selected account is not valid.");
                return;
            }
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            this.formIsDirty = false;
            LoadAccountsWithFilter();
            return;
        }
        private Account GetAccountsFromForm()
        {
            Account lAccount = new Account();
            if (!string.IsNullOrEmpty(TextBoxGeneralAccountId.Text))
            {
                lAccount.Id = Convert.ToInt64(TextBoxGeneralAccountId.Text);
            }
            else
            {
                lAccount.Id = 0L;
            }
            lAccount.Name = TextBoxGeneralAccountName.Text.Trim();
            lAccount.Discription = TextBoxGeneralAccountDescription.Text.Trim();
            float Balance = string.IsNullOrEmpty(TextBoxGeneralAccountBalance.Text) ? 0 : float.Parse(TextBoxGeneralAccountBalance.Text);
            if (ComboBoxGeneralAccountGroup.SelectedIndex > -1)
            {
                AccountGroup AccountGroup = null!;
                if (ComboBoxGeneralAccountDetailGroup.SelectedIndex > -1)
                {
                    AccountGroup = (AccountGroup)ComboBoxGeneralAccountDetailGroup.Items[ComboBoxGeneralAccountDetailGroup.SelectedIndex];
                }
                else
                {
                    AccountGroup = (AccountGroup)ComboBoxGeneralAccountGroup.Items[ComboBoxGeneralAccountGroup.SelectedIndex];
                }
                if (AccountGroup != null)
                {
                    AccountGroup = AccountGroupManager.GetAccountGroupById(AccountGroup.Id);
                    if (AccountGroup != null)
                    {
                        lAccount.AccountGroupId = AccountGroup.Id;
                        //lAccount.AccountGroup = AccountGroup;
                    }
                }
            }

            //set the amount
            if (ComboBoxBalanceType.SelectedIndex == 0)
            {
                lAccount.Debit(Balance);
            }
            else
            {
                lAccount.Credit(Balance);
            }

            //lAccount.Balance = (Balance!=0 && ComboBoxBalanceType.SelectedIndex==0)?-Balance : Balance;
            //vincent
            //lAccount.BalanceAsOf = (DateTime)DateUtils.ToDate(TextBoxGeneralAccountAsof.Text, CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
            lAccount.BalanceAsOf = (DateTime)DateTimePickerGeneralAccount.Date!;
            lAccount.CompanyId = Global.Company.CompanyId;
            lAccount.DisplayAs = TextBoxGeneralAccountDisplayAs.Text;
            lAccount.IsSubAccount = false;
            lAccount.AccountType = AccountType.ACCOUNT;
            if (CheckBoxGeneralAccount.Checked == true)
            {
                if (ComboBoxGeneralAccountParentAccount.SelectedIndex > -1)
                {
                    Account Account = (Account)ComboBoxGeneralAccountParentAccount.Items[ComboBoxGeneralAccountParentAccount.SelectedIndex];
                    if (Account != null)
                    {
                        Account = AccountManager.GetAccountById(Account.Id);
                        if (Account != null)
                        {
                            lAccount.ParentAccountId = Account.Id;
                            lAccount.IsSubAccount = true;
                        }
                    }
                }
            }
            return lAccount;
        }
        private void TreeViewGeneralAccount_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                TreeNode node = e.Node!;
                node.SelectedImageIndex = node.ImageIndex;
                //ResetForm();
                LoadAccountInfo();
                EnableForm(false);
                this.formIsDirty = false;
            }
            catch
            {
                ToolStripStatusLabelErrorGeneralAccount.Text = "Error Loading Accounts";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        private void ComboBoxGeneralAccountDetailType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxGeneralAccountDetailGroup.SelectedIndex > -1)
            {
                AccountGroup AccountGroup = (AccountGroup)ComboBoxGeneralAccountDetailGroup.Items[ComboBoxGeneralAccountDetailGroup.SelectedIndex];
                if (AccountGroup != null)
                {
                    AccountGroup lAccountGroup = AccountGroupManager.GetAccountGroupById(AccountGroup.Id);
                    WebBrowserGeneralAccount.Document.Write(string.Empty);
                    WebBrowserGeneralAccount.DocumentText = "<p style='font-family: Tahoma; font-size:8.25pt'>" + lAccountGroup.Description + "</p>";
                }
            }
        }
        private void TextBoxGeneralAccountSearch_TextChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            LoadAccountsWithFilter();
            if (TreeViewGeneralAccount.Nodes.Count > 0) { BtnGeneralAccountEdit.Enabled = true; BtnGeneralAccountDelete.Enabled = true; }
            else { BtnGeneralAccountEdit.Enabled = false; BtnGeneralAccountDelete.Enabled = false; }
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void ComboBoxGeneralAccountGroup_TextChanged(object sender, EventArgs e)
        {
            if (ComboBoxGeneralAccountGroup.SelectedIndex > -1)
            {
                AccountGroup AccountGroup = (AccountGroup)ComboBoxGeneralAccountGroup.Items[ComboBoxGeneralAccountGroup.SelectedIndex];
                ComboUtils.InitializeAccountDetailTypeCombo(ComboBoxGeneralAccountDetailGroup, AccountGroup.Id);
            }
        }

        private void BtnGeneralAccountNew_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            ComboUtils.InitializeAccountCombo(ComboBoxGeneralAccountParentAccount, Global.Company.CompanyId);
            ComboUtils.InitializeAccountGroupCombo(ComboBoxGeneralAccountGroup);
            TextBoxGeneralAccountName.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }

        private void BtnGeneralAccountDelete_Click(object sender, EventArgs e)
        {
            ToolStripStatusLabelErrorGeneralAccount.Text = "";
            if (string.IsNullOrEmpty(TextBoxGeneralAccountId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this account is still valid.");
                return;
            }
            Account AccountInfo = GetAccountInfo();
            if (AccountInfo != null)
            {
                DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, AccountInfo.Name), "Delete Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        Account Account = AccountManager.CheckSubAccount(AccountInfo.Id);
                        if (Account == null)
                        {
                            bool Delete = AccountManager.DeleteAccount(AccountInfo.Id);
                            if (Delete)
                            {
                                ResetForm();
                                ComboUtils.InitializeAccountCombo(ComboBoxGeneralAccountParentAccount, Global.Company.CompanyId);
                                ComboUtils.InitializeAccountGroupCombo(ComboBoxGeneralAccountGroup);
                                LoadAccountsWithFilter();
                                EnableForm(false);
                                SetFocus();
                                this.formIsDirty = false;

                            }
                            else
                            {
                                ToolStripStatusLabelErrorGeneralAccount.Text = DeleteErrorText;
                            }
                        }
                        else
                        {
                            ToolStripStatusLabelErrorGeneralAccount.Text = IsbranchDeleteErrorText;
                        }
                    }
                    catch(Exception ex) 
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }

                }
            }
            else
            {
                DisplaySystemError("Somting went wrong, please check this account is still valid.");
                return;
            }
        }
        private void BtnGeneralAccountEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxGeneralAccountId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this account is still valid.");
                return;
            }
            LoadAccountInfo();
            EnableForm(true);
            if (CheckBoxGeneralAccount.Checked == false)
            {
                CheckBoxGeneralAccount.Enabled = false;
                ComboBoxGeneralAccountParentAccount.Visible = false;
            }
            TextBoxGeneralAccountName.Select();
            this.formIsDirty = false;

        }
        private void BtnGeneralAccountCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxGeneralAccountName.Select();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            ComboUtils.InitializeAccountCombo(ComboBoxGeneralAccountParentAccount, Global.Company.CompanyId);
            ComboUtils.InitializeAccountGroupCombo(ComboBoxGeneralAccountGroup);
            if (string.IsNullOrEmpty(TextBoxGeneralAccountSearch.Text))
            {
                LoadAccountInfo();
            }
            else
            {
                TextBoxGeneralAccountSearch.Clear();
            }
            EnableForm(false);
            SetFocus();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void SetFocus()
        {
            if (TreeViewGeneralAccount.Nodes.Count > 0)
            {
                TextBoxGeneralAccountSearch.Select();
            }
            else
            {
                BtnGeneralAccountNew.Select();
            }
        }
        private void BtnGeneralAccountSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Account lAccount = GetAccountsFromForm();
                    Account AccountFromDB = null!;
                    if (lAccount.Id == 0)
                    {
                        Account lAccountByName = AccountManager.GetAccountByName(lAccount.Name, Global.Company.CompanyId);
                        if (lAccountByName == null)
                        {
                            AccountFromDB = AccountManager.AddAccount(lAccount);
                        }
                        else
                        {
                            ToolStripStatusLabelErrorGeneralAccount.Text = string.Format(UniqueAccountNameErrorMsg, lAccount.Name, Global.Company.Name);
                            TextBoxGeneralAccountName.Select();
                            return;
                        }
                    }
                    else
                    {
                        Account lAccountById = AccountManager.GetAccountById(lAccount.Id);
                        if (lAccountById != null)
                        {
                            Account lAccountName = AccountManager.CheckAccountNameInUpdate(lAccount.Name, lAccount.Id, Global.Company.CompanyId);
                            if (lAccountName == null)
                            {
                                AccountFromDB = AccountManager.UpdateAccount(lAccount);
                            }
                            else
                            {
                                ToolStripStatusLabelErrorGeneralAccount.Text = string.Format(UniqueAccountNameErrorMsg, lAccount.Name, Global.Company.Name);
                                TextBoxGeneralAccountName.Select();
                                return;
                            }
                        }
                        else
                        {
                            DisplaySystemError("Somting went wrong, please check this account is still valid.");
                            return;
                        }
                    }
                    ResetForm();
                    ComboUtils.InitializeAccountCombo(ComboBoxGeneralAccountParentAccount, Global.Company.CompanyId);
                    if (string.IsNullOrEmpty(TextBoxGeneralAccountSearch.Text))
                    {
                        LoadAccountsWithFilter();
                    }
                    else
                    {
                        TextBoxGeneralAccountSearch.Clear();
                    }
                    TreeNode TreeNode = new TreeNode();
                    TreeNode = AccountFromDB.ParentAccountId == null ? TreeViewGeneralAccount.Nodes[AccountFromDB.Id.ToString()] : TreeViewGeneralAccount.Nodes[AccountFromDB.ParentAccountId.ToString()].Nodes[AccountFromDB.Id.ToString()];
                    TreeViewGeneralAccount.SelectedNode = TreeNode;
                    TreeViewGeneralAccount.Focus();
                    EnableForm(false);
                    if (CreateAccountOnLoad)
                    {
                        if (parent != null)
                        {
                            parent.AccountIdTransport.ResetText();
                            parent.AccountIdTransport.Text = AccountFromDB.Id.ToString();
                        }
                        this.Close();
                    }
                    this.formIsDirty = false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                ToolStripStatusLabelErrorGeneralAccount.Text = "Error saving the account, please contact administrator.";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }
        private void BtnCurrencyExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private Boolean ValidateForm()
        {
            ToolStripStatusLabelErrorGeneralAccount.Text = "";
            if (string.IsNullOrEmpty(TextBoxGeneralAccountName.Text.Trim()))
            {
                ToolStripStatusLabelErrorGeneralAccount.Text = EnterAccountNameErrorMsg;
                TextBoxGeneralAccountName.Select();
                return false;
            }
            if (CheckBoxGeneralAccount.Checked == true && ComboBoxGeneralAccountParentAccount.SelectedIndex < 0)
            {
                ToolStripStatusLabelErrorGeneralAccount.Text = ChooseParentErrorMsg;
                ComboBoxGeneralAccountParentAccount.Select();
                return false;
            }
            if (CheckBoxGeneralAccount.Checked == true && ComboBoxGeneralAccountParentAccount.SelectedIndex >= 0 && AccountManager.GetAccountById(((Account)ComboBoxGeneralAccountParentAccount.Items[ComboBoxGeneralAccountParentAccount.SelectedIndex]).Id) == null)
            {
                ToolStripStatusLabelErrorGeneralAccount.Text = "Somting went wrong, please check this parent account is still valid.";
                ComboBoxGeneralAccountParentAccount.Select();
                return false;
            }
            if (ComboBoxGeneralAccountGroup.SelectedIndex < 0)
            {
                ToolStripStatusLabelErrorGeneralAccount.Text = ChooseAccountTypeErrorMsg;
                ComboBoxGeneralAccountGroup.Select();
                return false;

            }
            if (ComboBoxGeneralAccountGroup.SelectedIndex >= 0 && AccountGroupManager.GetAccountGroupById(((AccountGroup)ComboBoxGeneralAccountGroup.Items[ComboBoxGeneralAccountGroup.SelectedIndex]).Id) == null)
            {
                ToolStripStatusLabelErrorGeneralAccount.Text = "Somting went wrong, please check this account group is still valid.";
                ComboBoxGeneralAccountGroup.Select();
                return false;

            }
            if (ComboBoxGeneralAccountDetailGroup.SelectedIndex < 0)
            {
                ToolStripStatusLabelErrorGeneralAccount.Text = ChooseAccountDetailTypeErrorMsg;
                ComboBoxGeneralAccountDetailGroup.Select();
                return false;
            }
            if (ComboBoxGeneralAccountDetailGroup.SelectedIndex >= 0 && AccountGroupManager.GetAccountGroupById(((AccountGroup)ComboBoxGeneralAccountDetailGroup.Items[ComboBoxGeneralAccountDetailGroup.SelectedIndex]).Id) == null)
            {
                ToolStripStatusLabelErrorGeneralAccount.Text = "Somting went wrong, please check this account detail type is still valid.";
                ComboBoxGeneralAccountDetailGroup.Select();
                return false;

            }
            if (ComboBoxBalanceType.SelectedIndex == -1)
            {
                ToolStripStatusLabelErrorGeneralAccount.Text = ChooseBalanceTypeErrorMsg;
                ComboBoxBalanceType.Select();
                return false;
            }
            if (DateTimePickerGeneralAccount.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerGeneralAccount.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ToolStripStatusLabelErrorGeneralAccount.Text = EnterValidAsOfDateErrorMsg;
                DateTimePickerGeneralAccount.Focus();
                return false;
            }
            return true;
        }
        private void ResetForm()
        {
            ToolStripStatusLabelErrorGeneralAccount.Text = "";
            CheckBoxGeneralAccount.Checked = false;
            WebBrowserGeneralAccount.DocumentText = "";
            TextBoxGeneralAccountId.ResetText();
            TextBoxGeneralAccountName.ResetText();
            TextBoxGeneralAccountDisplayAs.ResetText();
            TextBoxGeneralAccountDescription.ResetText();
            TextBoxGeneralAccountBalance.ResetText();
            TextBoxGeneralAccountBalance.Decimals = Global.Company.PrimaryCurrency.RoundingPrecision;
            DateTimePickerGeneralAccount.Format = Global.Company.DateFormat;
            DateTimePickerGeneralAccount.MaxDate = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
            DateTimePickerGeneralAccount.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
            ComboBoxGeneralAccountDetailGroup.SelectedIndex = -1;
            ComboBoxGeneralAccountParentAccount.SelectedIndex = -1;
            ComboBoxGeneralAccountGroup.SelectedIndex = -1;
            ComboBoxBalanceType.SelectedIndex = 0;
        }
        private void EnableForm(Boolean enable)
        {
            if (TreeViewGeneralAccount.Nodes.Count > 0)
            {
                TreeViewGeneralAccount.Enabled = !enable;
                TextBoxGeneralAccountSearch.ReadOnly = enable;
                TextBoxGeneralAccountSearch.TabStop = !enable;
            }
            else
            {
                TreeViewGeneralAccount.Enabled = false;
                TextBoxGeneralAccountSearch.ReadOnly = true;
                TextBoxGeneralAccountSearch.TabStop = false;
                BtnGeneralAccountNew.Select();
            }
            TextBoxGeneralAccountName.ReadOnly = !enable;
            TextBoxGeneralAccountDisplayAs.ReadOnly = !enable;
            TextBoxGeneralAccountDescription.ReadOnly = !enable;
            TextBoxGeneralAccountBalance.ReadOnly = !enable;
            WebBrowserGeneralAccount.IsWebBrowserContextMenuEnabled = true;
            DateTimePickerGeneralAccount.ReadOnly = !enable;
            DateTimePickerGeneralAccount.TabStop = enable;
            TextBoxGeneralAccountName.TabStop = enable;
            TextBoxGeneralAccountDisplayAs.TabStop = enable;
            TextBoxGeneralAccountDescription.TabStop = enable;
            TextBoxGeneralAccountBalance.TabStop = enable;
            ComboBoxGeneralAccountDetailGroup.Visible = enable;
            ComboBoxGeneralAccountGroup.Visible = enable;
            ComboBoxBalanceType.Visible = enable;
            if (ComboBoxGeneralAccountParentAccount.Items.Count > 0)
            {
                CheckBoxGeneralAccount.Enabled = enable;
            }
            ComboBoxGeneralAccountParentAccount.Visible = enable;
            if (CheckBoxGeneralAccount.Checked && enable) { ComboBoxGeneralAccountParentAccount.Visible = true; }
            else { ComboBoxGeneralAccountParentAccount.Visible = false; }
            if (!enable)
            {
                BtnGeneralAccountCancel.Enabled = enable;
                if (TreeViewGeneralAccount.SelectedNode == null)
                {
                    BtnGeneralAccountDelete.Enabled = enable;
                    BtnGeneralAccountEdit.Enabled = enable;
                }
                else
                {
                    BtnGeneralAccountDelete.Enabled = !enable;
                    BtnGeneralAccountEdit.Enabled = !enable;
                }

                BtnGeneralAccountNew.Enabled = !enable;
                BtnGeneralAccountSave.Enabled = enable;
            }
            else
            {
                BtnGeneralAccountCancel.Enabled = enable;
                BtnGeneralAccountDelete.Enabled = !enable;
                BtnGeneralAccountEdit.Enabled = !enable;
                BtnGeneralAccountNew.Enabled = !enable;
                BtnGeneralAccountSave.Enabled = enable;
            }
        }
        int Index = -1;
        private void CheckBoxGeneralAccount_CheckedChanged(object sender, EventArgs e)
        {
            ComboBoxGeneralAccountParentAccount.Visible = CheckBoxGeneralAccount.Checked;
            if (!CheckBoxGeneralAccount.Checked) { LabelGeneralAccountParentAccount.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular); Index = ComboBoxGeneralAccountParentAccount.SelectedIndex; ComboBoxGeneralAccountParentAccount.SelectedIndex = -1; }
            else if (CheckBoxGeneralAccount.Checked) { LabelGeneralAccountParentAccount.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold); ComboBoxGeneralAccountParentAccount.SelectedIndex = Index; }
        }
        private void TextBoxGeneralAccountBalance_Leave_1(object sender, EventArgs e)
        {
            if (TextBoxGeneralAccountBalance.Text == "." || TextBoxGeneralAccountBalance.Text == "")
            {
                TextBoxGeneralAccountBalance.Text = "0.00";
            }
            else
            {
                TextBoxGeneralAccountBalance.Text = String.Format("{0:0.00}", float.Parse(TextBoxGeneralAccountBalance.Text));
            }
        }
        private void TextBoxGeneralAccountSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }

        private void TextBoxGeneralAccountBalance_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            string Unselected = TextBoxGeneralAccountBalance.Text;
            if (TextBoxGeneralAccountBalance.SelectionLength > 0) { Unselected = TextBoxGeneralAccountBalance.Text.Remove(TextBoxGeneralAccountBalance.SelectionStart, TextBoxGeneralAccountBalance.SelectionLength); }
            KeypressValidation.Keypress_NumberDot(sender, e, Unselected);
        }
        private void TextBoxGeneralAccountName_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void ComboBoxGeneralAccountParentAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxGeneralAccountParentAccount.DroppedDown = false;
        }

        private void ComboBoxGeneralAccountGroup_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxGeneralAccountGroup.DroppedDown = false;
        }

        private void ComboBoxGeneralAccountDetailType_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxGeneralAccountDetailGroup.DroppedDown = false;
        }

        private void TextBoxGeneralAccountSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                TreeViewGeneralAccount.Select();
            }
        }

        private void TextBoxGeneralAccountName_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "NameChecking");
        }

        private void TextBoxGeneralAccountName_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxGeneralAccountName, "NameChecking");
            }
        }

        private void TextBoxGeneralAccountDisplayAs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxGeneralAccountDisplayAs, "NameChecking");
            }
        }

        private void TextBoxGeneralAccountBalance_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "NumberDot");

        }

        private void TextBoxGeneralAccountBalance_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxGeneralAccountBalance, "NumberDot");
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnGeneralAccountNew.PerformClick();
            }
            else if (keyData == (Keys.F4))
            {
                BtnGeneralAccountDelete.PerformClick();
            }
            else if (keyData == (Keys.F7))
            {
                BtnGeneralAccountEdit.PerformClick();
            }
            else if (keyData == (Keys.F8))
            {
                BtnGeneralAccountSave.PerformClick();
            }
            else if (keyData == (Keys.F10))
            {
                BtnCurrencyExit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnGeneralAccountCancel.PerformClick();
                return true;
            }
            if (keyData == Keys.Tab && ActiveControl == BtnGeneralAccountSave)
            {
                TextBoxGeneralAccountName.Select();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void FormGeneralAccounts_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxGeneralAccountName.Select();
                    e.Cancel = true;
                }
            }
        }
        private void TextBoxGeneralAccountName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxGeneralAccountDisplayAs.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnGeneralAccountSave.Select();
            }
        }

    }
}
