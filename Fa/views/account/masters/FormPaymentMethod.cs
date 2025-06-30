using System;
using System.Collections.Generic;
using System.Windows.Forms;
using fa.model.Accounting.Masters;
using fa.api.Accounting;
using fa.libraries.Validation;
using fa.libraries.utils;
using System.Drawing;

namespace fa.views.account.masters
{
    public partial class FormPaymentMethod : FormBase
    {
        public static string CreatePaymentMethodOnLoadText = "Create new payment method";
        public static string SaveSuccessText = "Saved success...";
        public static string DeleteSuccessText = "Deleted successfully...";
        public static string DeleteConfirmText = "Do you want to delete the payment method {0}?";
        public static string DeleteErrorText = "Error deleting the payment method!, Please retry";
        public static string NotAllowDeleteErrorText = "Could not delete payment method,{0}!, It is used in some where else!";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want exit?";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string UniqueNameErrorMsg = "Payment method {0} already exists";
        public static string EnterNameErrorMsg = "Please enter payment method name";
        public static string ChooseAccountErrorMsg = "Please choose account";


        public bool CreatePaymentMethodOnLoad = false;
        AccountManager AccountManager = null;
        PaymentMethodManager PaymentMethodManager = null;
        KeypressValidation KeypressValidation = null;
        public FormPaymentMethod()
        {
            AccountManager = AccountManager.Instance;
            PaymentMethodManager =  PaymentMethodManager.Instance;
            KeypressValidation =  KeypressValidation.Instance;
            InitializeComponent();           
        }
        private void FormPaymentMethod_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(false);
            ComboUtils.InitializeAllAccountCombo(ComboBoxPaymentMethodAccount, Global.Company.CompanyId);
            LoadPaymentMethodWithFilter();
            if (ListBoxPaymentMethod.Items.Count > 0)
            {
                ToolStripStatusLabelErrorPayment.Text = "";
            }
            else
            {
                ToolStripStatusLabelErrorPayment.Text = CreatePaymentMethodOnLoadText;
            }
            if (CreatePaymentMethodOnLoad)
            {
                this.Text = CreatePaymentMethodOnLoadText;
                this.BtnPaymentMethodEdit.Visible = false;
                TextBoxPaymentMethodSearch.Visible = false;
                BtnPaymentMethodDelete.Visible = false;
                BtnPaymentMethodNew.Visible = false;
                ListBoxPaymentMethod.Visible = false;
                TabControlPaymentMethod.Location = new Point(12, 12);
                BtnPaymentMethodSave.Location = new Point(TabControlPaymentMethod.Width - 74, TabControlPaymentMethod.Height + 15);
                BtnPaymentMethodCancel.Location = new Point(BtnPaymentMethodSave.Location.X-88, BtnPaymentMethodSave.Location.Y);
                this.Size = new Size(TabControlPaymentMethod.Right + 27, TabControlPaymentMethod.Bottom + 90);
                this.CenterToParent();
                BtnPaymentMethodNew_Click(this, null);
            }
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void LoadPaymentMethodWithFilter()
        {
            string FilterString = TextBoxPaymentMethodSearch.Text.Trim();
            ListBoxPaymentMethod.Items.Clear();
            IList<PaymentMethod> PaymentMethods = PaymentMethodManager.GetAllPaymentMethodByCompanyId(Global.Company.CompanyId);
            foreach (var lPaymentMethod in PaymentMethods)
            {
                if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim()) || lPaymentMethod.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    ListBoxPaymentMethod.Items.Add(lPaymentMethod);
                }
            }
            if (ListBoxPaymentMethod.Items.Count > 0)
            {
                ListBoxPaymentMethod.SelectedIndex = 0;
            }
        }
        private PaymentMethod GetPaymentMethodInfo()
        {
            int Index = ListBoxPaymentMethod.SelectedIndex;
            if (Index > -1)
            {
                PaymentMethod lPaymentMethod = (PaymentMethod)ListBoxPaymentMethod.Items[Index];
                if (lPaymentMethod != null)
                {
                    PaymentMethod PaymentMethodInfo = PaymentMethodManager.GetPaymentMethodById(lPaymentMethod.Id);
                    return PaymentMethodInfo;
                }
            }
            return null;
        }
        private void LoadPaymentMethodInfo()
        {
            PaymentMethod PaymentMethodFromDB = GetPaymentMethodInfo();
            if (PaymentMethodFromDB != null)
            {
                TextBoxPaymentMethodId.Text = PaymentMethodFromDB.Id.ToString();
                TextBoxPaymentMethodName.Text = PaymentMethodFromDB.Name;
                TextBoxPaymentMethodDisplayAs.Text = PaymentMethodFromDB.DisplayAs;
                if (PaymentMethodFromDB.CreditCard)
                {
                    RadioButtonPaymentMethodYes.Checked = true;
                }
                else
                {
                    RadioButtonPaymentMethodNo.Checked = true;
                } 
                if(PaymentMethodFromDB.AccountId!=null)
                {
                    Account Account = AccountManager.GetAccountById((long)PaymentMethodFromDB.AccountId);
                    if(Account!=null)
                    {
                        ComboBoxPaymentMethodAccount.SelectedIndex = ComboBoxPaymentMethodAccount.FindStringExact(Account.Name);
                    }
                }            
            }
            else
            {
                //DisplaySystemError("Somthing went wrong, the selected payment method is not valid.");
                return;
            }
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            this.formIsDirty = false;
            LoadPaymentMethodWithFilter();
            return;
        }
        private PaymentMethod GetPaymentMethodFromForm()
        {
            PaymentMethod lPaymentMethod = new PaymentMethod();
            if (!string.IsNullOrEmpty(TextBoxPaymentMethodId.Text))
            {
                lPaymentMethod.Id = Convert.ToInt64(TextBoxPaymentMethodId.Text);
            }
            else
            {
                lPaymentMethod.Id = 0L;
            }
            lPaymentMethod.Name = TextBoxPaymentMethodName.Text.Trim();
            lPaymentMethod.CreditCard = RadioButtonPaymentMethodYes.Checked;
            lPaymentMethod.DisplayAs = TextBoxPaymentMethodDisplayAs.Text.Trim();
            lPaymentMethod.CompanyId = Global.Company.CompanyId;
            Account Account = (Account)ComboBoxPaymentMethodAccount.Items[ComboBoxPaymentMethodAccount.SelectedIndex];
            if (Account != null)
            {
                lPaymentMethod.AccountId = Account.Id;
            }          
            return lPaymentMethod;
        }       
        private void ListBoxPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetForm();
            LoadPaymentMethodInfo();
            EnableForm(false);
        }
        private void TextBoxPaymentMethodSearch_TextChanged(object sender, EventArgs e)
        {
            ResetForm();
            ComboUtils.InitializeAllAccountCombo(ComboBoxPaymentMethodAccount, Global.Company.CompanyId);
            LoadPaymentMethodWithFilter();
            if (ListBoxPaymentMethod.Items.Count > 0) { BtnPaymentMethodEdit.Enabled = true; BtnPaymentMethodDelete.Enabled = true; }
            else { BtnPaymentMethodEdit.Enabled = false; BtnPaymentMethodDelete.Enabled = false; }
        }
        private void BtnPaymentMethodNew_Click(object sender, EventArgs e)
        {
            ResetForm();
            EnableForm(true);
            TextBoxPaymentMethodName.Select();
            this.formIsDirty = false;
        }
        private void BtnPaymentMethodDelete_Click(object sender, EventArgs e)
        {
            ToolStripStatusLabelErrorPayment.Text = "";
            if (string.IsNullOrEmpty(TextBoxPaymentMethodId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this payment method is still valid.");
                return;
            }
            PaymentMethod PaymentMethodInfo = GetPaymentMethodInfo();
            if (PaymentMethodInfo != null)
            {
                DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, PaymentMethodInfo.Name), "Delete Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning,MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                {
                    Boolean Deleted = PaymentMethodManager.DeletePaymentMethod(PaymentMethodInfo.Id);
                    if (Deleted)
                    {
                        ResetForm();
                        ComboUtils.InitializeAllAccountCombo(ComboBoxPaymentMethodAccount, Global.Company.CompanyId);
                        LoadPaymentMethodWithFilter();
                        EnableForm(false);
                        if (ListBoxPaymentMethod.Items.Count == 0)
                        {
                            ToolStripStatusLabelErrorPayment.Text = CreatePaymentMethodOnLoadText;
                        }
                        else
                        {
                            ToolStripStatusLabelErrorPayment.Text = DeleteSuccessText;
                        }
                    }
                    else
                    {
                        ToolStripStatusLabelErrorPayment.Text =string.Format(NotAllowDeleteErrorText,PaymentMethodInfo.Name);
                    }
                    this.formIsDirty = false;
                }
            }
            else
            {
                DisplaySystemError("Somting went wrong, please check this payment method is still valid.");
                return;
            }
        }
        private void BtnPaymentMethodEdit_Click(object sender, EventArgs e)
        {
            ToolStripStatusLabelErrorPayment.Text = "";
            if (string.IsNullOrEmpty(TextBoxPaymentMethodId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this payment method is still valid.");
                return;
            }
            LoadPaymentMethodInfo();
            EnableForm(true);
            TextBoxPaymentMethodName.Select();
            this.formIsDirty = false;
        }
        private void BtnPaymentMethodCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TabControlPaymentMethod.SelectedTab = TabDetails;
                    TextBoxPaymentMethodName.Select();
                    return;
                }
            }
            if (CreatePaymentMethodOnLoad)
            {
                this.Close();
            }
            else
            {
                ResetForm();
                ComboUtils.InitializeAllAccountCombo(ComboBoxPaymentMethodAccount, Global.Company.CompanyId);
                if (string.IsNullOrEmpty(TextBoxPaymentMethodSearch.Text))
                {
                    LoadPaymentMethodInfo();
                }
                else
                {
                    TextBoxPaymentMethodSearch.Clear();
                }
                EnableForm(false);
                if (ListBoxPaymentMethod.Items.Count > 0)
                {
                    ToolStripStatusLabelErrorPayment.Text = "";
                }
                else
                {
                    ToolStripStatusLabelErrorPayment.Text = CreatePaymentMethodOnLoadText;
                }
            }
        }
        private void BtnPaymentMethodSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Cursor.Current = Cursors.WaitCursor;
                PaymentMethod PaymentMethodInfo = GetPaymentMethodFromForm();
                PaymentMethod lPaymentMethodFromDB = null;
                if (PaymentMethodInfo.Id == 0)
                {
                    PaymentMethod lPaymentMethodName = PaymentMethodManager.GetPaymentMethodByName(Global.Company.CompanyId,PaymentMethodInfo.Name);
                    if (lPaymentMethodName == null)
                    {
                        lPaymentMethodFromDB=PaymentMethodManager.AddPaymentMethod(PaymentMethodInfo);                        
                    }
                    else
                    {
                        ToolStripStatusLabelErrorPayment.Text =string.Format(UniqueNameErrorMsg, PaymentMethodInfo.Name);
                        TextBoxPaymentMethodName.Select();
                        return;
                    }
                }
                else
                {
                    PaymentMethod lPaymentMethodById = PaymentMethodManager.GetPaymentMethodById(PaymentMethodInfo.Id);
                    if (lPaymentMethodById != null)
                    {
                        PaymentMethod lPaymentMethodName = PaymentMethodManager.CheckPaymentMethodNameInUpdate(Global.Company.CompanyId,PaymentMethodInfo.Name, lPaymentMethodById.Id);
                        if (lPaymentMethodName == null)
                        {
                            lPaymentMethodFromDB = PaymentMethodManager.UpdatePaymentMethod(PaymentMethodInfo);                  
                        }
                        else
                        {
                            ToolStripStatusLabelErrorPayment.Text = string.Format(UniqueNameErrorMsg, PaymentMethodInfo.Name);
                            TextBoxPaymentMethodName.Select();
                            return;
                        }
                    }
                    else
                    {
                        DisplaySystemError("Somting went wrong, please check this payment method is still valid.");
                        return;
                    }
                }
                ResetForm();
                ComboUtils.InitializeAllAccountCombo(ComboBoxPaymentMethodAccount, Global.Company.CompanyId);
                if (string.IsNullOrEmpty(TextBoxPaymentMethodSearch.Text))
                {
                    LoadPaymentMethodWithFilter();
                }
                else
                {
                    TextBoxPaymentMethodSearch.Clear();
                }
                ListBoxPaymentMethod.SelectedIndex = ListBoxPaymentMethod.FindStringExact(lPaymentMethodFromDB.Name);
                EnableForm(false);
                if (CreatePaymentMethodOnLoad)
                {
                    this.Close();
                }
                ToolStripStatusLabelErrorPayment.Text = string.Format(SaveSuccessText, PaymentMethodInfo.Name);
                Cursor.Current = Cursors.Default;
            }
        }
        private void BtnPaymentMethodExit_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }
        private Boolean ValidateForm()
        {
            ToolStripStatusLabelErrorPayment.Text = "";
            if (string.IsNullOrEmpty(TextBoxPaymentMethodName.Text.Trim()))
            {
                ToolStripStatusLabelErrorPayment.Text =EnterNameErrorMsg;
                TextBoxPaymentMethodName.Select();
                return false;
            }            
            if (ComboBoxPaymentMethodAccount.SelectedIndex<0)
            {
                ToolStripStatusLabelErrorPayment.Text =ChooseAccountErrorMsg;
                ComboBoxPaymentMethodAccount.Select();
                return false;
            }
            if (ComboBoxPaymentMethodAccount.SelectedIndex >= 0 && AccountManager.GetAccountById(((Account)ComboBoxPaymentMethodAccount.Items[ComboBoxPaymentMethodAccount.SelectedIndex]).Id)==null)
            {
                ToolStripStatusLabelErrorPayment.Text = "Somthing went wrong, selected account is not valid.";
                ComboBoxPaymentMethodAccount.Select();
                return false;
            }
            return true;
        }
        private void ResetForm()
        {
            ToolStripStatusLabelErrorPayment.Text = "";
            TextBoxPaymentMethodDisplayAs.ResetText();
            TextBoxPaymentMethodId.ResetText();
            TextBoxPaymentMethodName.ResetText();
            RadioButtonPaymentMethodYes.Checked = true;
            RadioButtonPaymentMethodNo.Checked = false;
            ComboBoxPaymentMethodAccount.SelectedIndex = -1;
        }
        private void EnableForm(Boolean enable)
        {
            if (ListBoxPaymentMethod.Items.Count > 0)
            {
                TextBoxPaymentMethodSearch.ReadOnly = enable;
                TextBoxPaymentMethodSearch.TabStop = !enable;
                ListBoxPaymentMethod.Enabled = !enable;
            }
            else
            {
                ListBoxPaymentMethod.Enabled = false;
                TextBoxPaymentMethodSearch.ReadOnly = true;
                TextBoxPaymentMethodSearch.TabStop = false;
                BtnPaymentMethodNew.Select();
            }
            TextBoxPaymentMethodName.ReadOnly = !enable;
            TextBoxPaymentMethodDisplayAs.ReadOnly = !enable;
            RadioButtonPaymentMethodNo.Enabled = enable;
            RadioButtonPaymentMethodYes.Enabled = enable;
            ComboBoxPaymentMethodAccount.Visible =enable;
            TextBoxPaymentMethodName.TabStop = enable;
            TextBoxPaymentMethodDisplayAs.TabStop = enable;
            if (!enable)
            {
                BtnPaymentMethodCancel.Enabled = enable;
                if (ListBoxPaymentMethod.SelectedIndex < 0)
                {
                    BtnPaymentMethodDelete.Enabled = enable;
                    BtnPaymentMethodEdit.Enabled = enable;
                }
                else
                {
                    BtnPaymentMethodDelete.Enabled = !enable;
                    BtnPaymentMethodEdit.Enabled = !enable;
                }
                BtnPaymentMethodNew.Enabled = !enable;
                BtnPaymentMethodSave.Enabled = enable;
            }
            else
            {
                BtnPaymentMethodNew.Enabled = !enable;
                BtnPaymentMethodDelete.Enabled = !enable;
                BtnPaymentMethodEdit.Enabled = !enable;
                BtnPaymentMethodSave.Enabled = enable;
                BtnPaymentMethodCancel.Enabled = enable;
            }
        }
        private void TextBoxPaymentMethodName_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxPaymentMethodDisplayAs_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxPaymentMethodSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (ListBoxPaymentMethod.Items.Count > 1)
                {
                    ListBoxPaymentMethod.Select();
                    if((ListBoxPaymentMethod.SelectedIndex+1)==ListBoxPaymentMethod.Items.Count)
                    {
                        ListBoxPaymentMethod.SelectedIndex = 0;
                    }
                    else
                    {
                        ListBoxPaymentMethod.SelectedIndex = ListBoxPaymentMethod.SelectedIndex + 1;
                    }                    
                }
                else
                {
                    ListBoxPaymentMethod.Select();
                }
            }
        }

        private void ComboBoxPaymentMethodAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxPaymentMethodAccount.DroppedDown = false;
        }

        private void TextBoxPaymentMethodName_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "NameChecking");

        }

        private void TextBoxPaymentMethodName_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxPaymentMethodName, "NameChecking");
            }
        }

        private void TextBoxPaymentMethodDisplayAs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxPaymentMethodDisplayAs, "NameChecking");
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnPaymentMethodNew.PerformClick();
            }
            if (keyData == (Keys.F4))
            {
                BtnPaymentMethodDelete.PerformClick();
            }
            if (keyData == (Keys.F7))
            {
                BtnPaymentMethodEdit.PerformClick();
            }
            if (keyData == (Keys.F8))
            {
                BtnPaymentMethodSave.PerformClick();
            }
            if (keyData == (Keys.F10))
            {
                BtnPaymentMethodExit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnPaymentMethodCancel.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void BtnPaymentMethodSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxPaymentMethodName.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (RadioButtonPaymentMethodNo.Checked)
                {
                    RadioButtonPaymentMethodNo.Select();
                }
                else
                {
                    RadioButtonPaymentMethodYes.Select();
                }
            }
        }

        private void TextBoxPaymentMethodName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxPaymentMethodDisplayAs.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnPaymentMethodSave.Select();
            }
        }
    }
}
