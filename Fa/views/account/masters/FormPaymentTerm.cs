using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using fa.model.Accounting.Masters;
using fa.api.Accounting;
using fa.libraries.Validation;

namespace fa.views.account.masters
{
    public partial class FormPaymentTerm : FormBase
    {
        public static string CreatePaymentTermOnLoadText = "Create new payment term";
        public static string SaveSuccessText = "Saved success...";
        public static string DeleteSuccessText = "Payment term {0} deleted successfully...";
        public static string DeleteConfirmText = "Do you want to delete the payment term {0}?";
        public static string DeleteErrorText = "Error deleting the payment term!, Please retry";
        public static string NotAllowDeleteErrorText = "Could not delete payment term,{0}!, Please retry.";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want exit?";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string UniqueNameErrorMsg = "Payment term {0} already exists";
        public static string EnterNameErrorMsg = "Please enter payment term name";
        public static string EnterLeadPriodeErrorMsg = "Please enter lead periode";
        public static string EnterNumberOfDayErrorMsg = "Please enter number of day";
        public static string ChooseDueDateErrorMsg = "Please choose due date";


        public bool CreatePaymentTermOnLoad = false;
        PaymentTermManager PaymentTermManager = null;
        KeypressValidation KeypressValidation = null;
        public FormPaymentTerm()
        {
            PaymentTermManager = PaymentTermManager.Instance;
            KeypressValidation = KeypressValidation.Instance;
            InitializeComponent();
        }
        private void FormPaymentTerm_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(false);
            LoadPaymentTermWithFilter();
            if (CreatePaymentTermOnLoad)
            {
                this.Text = CreatePaymentTermOnLoadText;
                this.BtnPaymentTermEdit.Visible = false;
                BtnPaymentTermDelete.Visible = false;
                BtnPaymentTermNew.Visible = false;
                TextBoxPaymentTermSearch.Visible = false;
                ListBoxPaymentTerm.Visible = false;
                TabControlPaymentTerm.Location = new Point(12, 12);
                BtnPaymentTermSave.Location = new Point(TabControlPaymentTerm.Width - 74, TabControlPaymentTerm.Height + 15);
                BtnPaymentTermCancel.Location = new Point(BtnPaymentTermSave.Location.X - 88, BtnPaymentTermSave.Location.Y);
                this.Size = new Size(TabControlPaymentTerm.Right + 27, TabControlPaymentTerm.Bottom + 90);
                this.CenterToParent();
                BtnPaymentTermNew_Click(this, null);
            }
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void LoadPaymentTermWithFilter()
        {
            string FilterString = TextBoxPaymentTermSearch.Text.Trim();
            ListBoxPaymentTerm.Items.Clear();
            IList<PaymentTerm> PaymentTerms = PaymentTermManager.GetAllPaymentTermByCompanyId(Global.Company.CompanyId);
            foreach (var lPaymentTerm in PaymentTerms)
            {
                if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim()) || lPaymentTerm.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    ListBoxPaymentTerm.Items.Add(lPaymentTerm);
                }
            }

            if (ListBoxPaymentTerm.Items.Count > 0)
            {
                ListBoxPaymentTerm.SelectedIndex = 0;
            }
        }
        private PaymentTerm GetPaymentTermInfo()
        {
            int Index = ListBoxPaymentTerm.SelectedIndex;
            if (Index > -1)
            {
                PaymentTerm lPaymentTerm = (PaymentTerm)ListBoxPaymentTerm.Items[Index];
                if (lPaymentTerm != null)
                {
                    PaymentTerm PaymentTermFromDB = PaymentTermManager.GetPaymentTermById(lPaymentTerm.Id);
                    return PaymentTermFromDB;
                }
            }
            return null;
        }
        private void LoadPaymentTermInfo()
        {
            PaymentTerm PaymentTermFromDB = GetPaymentTermInfo();
            if (PaymentTermFromDB != null)
            {
                TextBoxPaymentTermId.Text = PaymentTermFromDB.Id.ToString();
                TextBoxPaymentTermName.Text = PaymentTermFromDB.Name;
                TextBoxPaymentTermNoofday.Text = PaymentTermFromDB.NoOfDays.ToString();
                //TextBoxPaymentTermLeadPeriode.Text = PaymentTermFromDB.LeadPeriodToDue.ToString();
                //if(PaymentTermFromDB.FixedDays)
                //{
                //    RadioButtonPaymentTermYes.Checked = true;
                //}
                //else
                //{
                //    RadioButtonPaymentTermNo.Checked = true;
                //}
                //ComboBoxPaymentTermDueonDate.SelectedIndex = ComboBoxPaymentTermDueonDate.FindStringExact(PaymentTermFromDB.DueOnDate.ToString());
            }
            else
            {
                DisplaySystemError("Somthing went wrong, the selected payment term is not valid.");
                return;
            }
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            this.formIsDirty = false;
            LoadPaymentTermWithFilter();
            return;
        }
        private PaymentTerm GetPaymentTermFromForm()
        {
            PaymentTerm lPaymentTerm = new PaymentTerm();
            if (!string.IsNullOrEmpty(TextBoxPaymentTermId.Text))
            {
                lPaymentTerm.Id = Convert.ToInt64(TextBoxPaymentTermId.Text);
            }
            else
            {
                lPaymentTerm.Id = 0L;
            }
            lPaymentTerm.Name = TextBoxPaymentTermName.Text.Trim();
            lPaymentTerm.NoOfDays = Convert.ToInt32(TextBoxPaymentTermNoofday.Text.Replace(" ", "").Trim());
            lPaymentTerm.CompanyId = Global.Company.CompanyId;
            return lPaymentTerm;
        }
        private void ListBoxPaymentTerm_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetForm();
            LoadPaymentTermInfo();
            EnableForm(false);
        }
        private void TextBoxPaymentTermSearch_TextChanged(object sender, EventArgs e)
        {
            ResetForm();
            LoadPaymentTermWithFilter();
            if (ListBoxPaymentTerm.Items.Count > 0) { BtnPaymentTermEdit.Enabled = true; BtnPaymentTermDelete.Enabled = true; }
            else { BtnPaymentTermEdit.Enabled = false; BtnPaymentTermDelete.Enabled = false; }
        }
        private void BtnPaymentTermNew_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            TextBoxPaymentTermName.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void BtnPaymentTermDelete_Click(object sender, EventArgs e)
        {
            ToolStripStatusLabelErrorPaymentTerm.Text = string.Empty;
            if (string.IsNullOrEmpty(TextBoxPaymentTermId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this payment term is still valid.");
                return;
            }
            PaymentTerm PaymentTermInfo = GetPaymentTermInfo();
            if (PaymentTermInfo != null)
            {
                DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, PaymentTermInfo.Name), "Delete Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                {
                    Boolean deleted = PaymentTermManager.DeletePaymentTerm(PaymentTermInfo.Id);
                    if (deleted)
                    {
                        ResetForm();
                        LoadPaymentTermWithFilter();
                        EnableForm(false);
                        ToolStripStatusLabelErrorPaymentTerm.Text = string.Format(DeleteSuccessText, PaymentTermInfo.Name);
                    }
                    else
                    {
                        ToolStripStatusLabelErrorPaymentTerm.Text = string.Format(NotAllowDeleteErrorText, PaymentTermInfo.Name);
                    }
                    this.formIsDirty = false;
                }
            }
            else
            {
                DisplaySystemError("Somting went wrong, please check this payment term is still valid.");
                return;
            }
        }
        private void BtnPaymentTermEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxPaymentTermId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this payment term is still valid.");
                return;
            }
            ToolStripStatusLabelErrorPaymentTerm.Text = "";
            LoadPaymentTermInfo();
            EnableForm(true);
            TextBoxPaymentTermName.Select();
            this.formIsDirty = false;
        }
        private void BtnPaymentTermCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TabControlPaymentTerm.SelectedTab = TabDetails;
                    TextBoxPaymentTermName.Select();
                    return;
                }
            }
            if (CreatePaymentTermOnLoad)
            {
                this.Close();
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                if (string.IsNullOrEmpty(TextBoxPaymentTermSearch.Text))
                {
                    LoadPaymentTermInfo();
                }
                else
                {
                    TextBoxPaymentTermSearch.Clear();
                }
                EnableForm(false);
                TextBoxPaymentTermSearch.Select();
                Cursor.Current = Cursors.Default;
            }
        }
        private void BtnPaymentTermSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Cursor.Current = Cursors.WaitCursor;
                PaymentTerm PaymentTermInfo = GetPaymentTermFromForm();
                PaymentTerm lPaymentTermFromDB = null;
                if (PaymentTermInfo.Id == 0)
                {
                    PaymentTerm lPaymentTermName = PaymentTermManager.GetPaymentTermByName(Global.Company.CompanyId, PaymentTermInfo.Name);
                    if (lPaymentTermName == null)
                    {
                        lPaymentTermFromDB = PaymentTermManager.AddPaymentTerm(PaymentTermInfo);
                        ToolStripStatusLabelErrorPaymentTerm.Text = string.Format(SaveSuccessText, PaymentTermInfo.Name);
                    }
                    else
                    {
                        ToolStripStatusLabelErrorPaymentTerm.Text = string.Format(UniqueNameErrorMsg, PaymentTermInfo.Name);
                        TextBoxPaymentTermName.Select();
                        return;
                    }
                }
                else
                {
                    PaymentTerm lPaymentTermById = PaymentTermManager.GetPaymentTermById(PaymentTermInfo.Id);
                    if (lPaymentTermById != null)
                    {
                        PaymentTerm lPaymentTermName = PaymentTermManager.CheckPaymentTermNameInUpdate(Global.Company.CompanyId, PaymentTermInfo.Name, lPaymentTermById.Id);
                        if (lPaymentTermName == null)
                        {
                            lPaymentTermFromDB = PaymentTermManager.UpdatePaymentTerm(PaymentTermInfo);
                        }
                        else
                        {
                            ToolStripStatusLabelErrorPaymentTerm.Text = string.Format(UniqueNameErrorMsg, PaymentTermInfo.Name);
                            TextBoxPaymentTermName.Select();
                            return;
                        }
                    }
                    else
                    {
                        DisplaySystemError("Somting went wrong, please check this payment term is still valid.");
                        return;
                    }
                }
                ResetForm();
                if (string.IsNullOrEmpty(TextBoxPaymentTermSearch.Text))
                {
                    LoadPaymentTermWithFilter();
                }
                else
                {
                    TextBoxPaymentTermSearch.Clear();
                }
                ListBoxPaymentTerm.SelectedIndex = ListBoxPaymentTerm.FindStringExact(PaymentTermInfo.Name);
                ToolStripStatusLabelErrorPaymentTerm.Text = string.Format(SaveSuccessText, PaymentTermInfo.Name);
                EnableForm(false);
                if (CreatePaymentTermOnLoad)
                {
                    this.Close();
                }
                Cursor.Current = Cursors.Default;
            }
        }
        private void BtnPaymentTermExit_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }

        private Boolean ValidateForm()
        {
            ToolStripStatusLabelErrorPaymentTerm.Text = "";
            if (string.IsNullOrEmpty(TextBoxPaymentTermName.Text.Trim()))
            {
                ToolStripStatusLabelErrorPaymentTerm.Text = EnterNameErrorMsg;
                TextBoxPaymentTermName.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxPaymentTermNoofday.Text))
            {
                ToolStripStatusLabelErrorPaymentTerm.Text = EnterNumberOfDayErrorMsg;
                TextBoxPaymentTermNoofday.Select();
                return false;
            }
            //if (string.IsNullOrEmpty(TextBoxPaymentTermLeadPeriode.Text) && !RadioButtonPaymentTermYes.Checked)
            //{
            //    ToolStripStatusLabelErrorPaymentTerm.Text =EnterLeadPriodeErrorMsg;                
            //    TextBoxPaymentTermLeadPeriode.Select();
            //    return false;
            //}
            //if (ComboBoxPaymentTermDueonDate.SelectedIndex<0 && !RadioButtonPaymentTermYes.Checked)
            //{
            //    ToolStripStatusLabelErrorPaymentTerm.Text = ChooseDueDateErrorMsg;
            //    ComboBoxPaymentTermDueonDate.Select();
            //    return false;
            //}
            return true;
        }
        private void ResetForm()
        {
            ToolStripStatusLabelErrorPaymentTerm.Text = "";
            TextBoxPaymentTermId.ResetText();
            //TextBoxPaymentTermLeadPeriode.ResetText();
            TextBoxPaymentTermName.ResetText();
            TextBoxPaymentTermNoofday.ResetText();
            //RadioButtonPaymentTermYes.Checked = true;
            //RadioButtonPaymentTermNo.Checked = false;
            //ComboBoxPaymentTermDueonDate.SelectedIndex = -1;
        }
        private void EnableForm(Boolean enable)
        {
            if (ListBoxPaymentTerm.Items.Count > 0)
            {
                TextBoxPaymentTermSearch.ReadOnly = enable;
                TextBoxPaymentTermSearch.TabStop = !enable;
                ListBoxPaymentTerm.Enabled = !enable;
            }
            else
            {
                ListBoxPaymentTerm.Enabled = false;
                TextBoxPaymentTermSearch.ReadOnly = true;
                TextBoxPaymentTermSearch.TabStop = false;
                BtnPaymentTermNew.Select();
            }
            TextBoxPaymentTermNoofday.ReadOnly = !enable;
            TextBoxPaymentTermName.ReadOnly = !enable;
            TextBoxPaymentTermNoofday.TabStop = enable;
            TextBoxPaymentTermName.TabStop = enable;
            if (!enable)
            {
                BtnPaymentTermCancel.Enabled = enable;
                if (ListBoxPaymentTerm.SelectedIndex < 0)
                {
                    BtnPaymentTermDelete.Enabled = enable;
                    BtnPaymentTermEdit.Enabled = enable;
                }
                else
                {
                    BtnPaymentTermDelete.Enabled = !enable;
                    BtnPaymentTermEdit.Enabled = !enable;
                }
                BtnPaymentTermNew.Enabled = !enable;
                BtnPaymentTermSave.Enabled = enable;
            }
            else
            {
                BtnPaymentTermNew.Enabled = !enable;
                BtnPaymentTermDelete.Enabled = !enable;
                BtnPaymentTermEdit.Enabled = !enable;
                BtnPaymentTermSave.Enabled = enable;
                BtnPaymentTermCancel.Enabled = enable;
            }
        }

        private void TextBoxPaymentTermName_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxPaymentTermSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                ListBoxPaymentTerm.Focus();
                /*
                 SendKeys.Send("DOWN");
                
                if (ListBoxPaymentTerm.Items.Count > 1)
                {
                    ListBoxPaymentTerm.Select();
                    if ((ListBoxPaymentTerm.SelectedIndex + 1) == ListBoxPaymentTerm.Items.Count)
                    {
                        ListBoxPaymentTerm.SelectedIndex = 0;
                    }
                    else
                    {
                        ListBoxPaymentTerm.SelectedIndex = ListBoxPaymentTerm.SelectedIndex + 1;
                    }
                }
                else
                {
                    ListBoxPaymentTerm.Select();
                }*/
            }
        }

        private void ComboBoxPaymentTermDueonDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            // this.ComboBoxPaymentTermDueonDate.DroppedDown = false;
        }

        private void TextBoxPaymentTermName_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "NameChecking");

        }

        private void TextBoxPaymentTermName_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxPaymentTermName, "NameChecking");
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnPaymentTermNew.PerformClick();
            }
            if (keyData == (Keys.F4))
            {
                BtnPaymentTermDelete.PerformClick();
            }
            if (keyData == (Keys.F7))
            {
                BtnPaymentTermEdit.PerformClick();
            }
            if (keyData == (Keys.F8))
            {
                BtnPaymentTermSave.PerformClick();
            }
            if (keyData == (Keys.F10))
            {
                BtnPaymentTermExit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnPaymentTermCancel.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void BtnPaymentTermSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxPaymentTermName.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxPaymentTermNoofday.Select();
            }
        }

        private void TextBoxPaymentTermName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxPaymentTermNoofday.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnPaymentTermSave.Focus();
            }
        }

        private void TextBoxPaymentTermNoofday_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (TextBoxPaymentTermNoofday.SelectionLength == TextBoxPaymentTermNoofday.TextLength && char.IsDigit(e.KeyChar))
            {
                TextBoxPaymentTermNoofday.Text = "";
            }
            if (TextBoxPaymentTermNoofday.SelectionLength < TextBoxPaymentTermNoofday.TextLength && char.IsDigit(e.KeyChar))
            {
                TextBoxPaymentTermNoofday.SelectedText = "";
            }
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            if (TextBoxPaymentTermNoofday.TextLength == 5 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
