using fa.libraries.Validation;
using fa.api.Accounting;
using fa.api.Accounting.Transactions;
using fa.api.System;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transactions;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using fa.views.account.masters;
using fa.views.common;
using fa.views.utils;
using fa.views.controls.grid;
using fa.views.employee;
using VisioForge.MediaFramework.Helpers;

namespace fa.views.account.transactions
{
    public enum CreditNoteTableColumn
    {
        SNO, ACCOUNT, DESC, AMOUNT, REMOVE, ID
    }
    public partial class FormCreditNote : FormBase
    {
        public long CustomerId = 0L;

        public static string SaveSuccessText = "Saved success...";
        public static string DeleteConfirmText = "Do you want to delete the CreditNote {0}?";
        public static string DeleteErrorText = "Error Deleting the CreditNote!, Please retry";
        public static string NotAllowDeleteErrorText = "Do not Delete CreditNote it used in payment";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string Grid_ConfirmRowDeleteText = "Do you want to delete row {0}?";
        public static string Grid_EmptyErrorMsg = "Please enter CreditNote details.";
        public static string Grid_MantatoryFiledErrorMsg = "Please enter {0}.";
        public static string SearchBoxEmptyErrorMsg = "Please enter search text, it could customer name or CreditNote Date or CreditNote Number.";
        public static string SearchOutput = "No Entry Found!";

        public static string ChooseCustomeErrorMsg = "Please choose Customer";
        public static string EnterCreditNoteDateErrorMsg = "Please enter proper CreditNote Date";
        public static string RefNoErrorMsg = "Please contact administrator to generate reference number.";

        AccountManager AccountManager = null;
        CustomerManager CustomerManager = null;
        CreditNoteManager CreditNoteManager = null;
        DateValidation DateValidation = null;
        KeypressValidation KeypressValidation = null;
        public long SearchCreditNoteId = 0L;

        public FormCreditNote()
        {
            InitializeComponent();
            AccountManager = AccountManager.Instance;
            CustomerManager = CustomerManager.Instance;
            CreditNoteManager = CreditNoteManager.Instance;
            DateValidation = DateValidation.Instance;
            KeypressValidation = KeypressValidation.Instance;
            excludedObjects = new string[] { "toolStrip1" };

        }
        private void FormCreditNote_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                DateTimePickerCreditNote.Format = Global.Company.DateFormat;
                DateTimePickerCreditNote.MinDate = Global.getCurrentFiscalYearStartDate();
                DateTimePickerCreditNote.MaxDate = Global.getCurrentFiscalYearEndDate();
                EnableForm(true);
                DataGridViewCreditNoteDetails.Columns["Amount"].DefaultCellStyle.Format = "#######.##";
                DataGridViewCreditNoteDetailsTotal.ShowCellToolTips = false;
                foreach (DataGridViewColumn column in DataGridViewCreditNoteDetailsTotal.Columns)
                {
                    column.Resizable = DataGridViewTriState.False;
                }
                TextBoxCreditNoteCustomer.Select();
                this.formIsDirty = false;

            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        private CreditNote GetCreditNoteFromForm()
        {
            CreditNote lCreditNote = new CreditNote();
            lCreditNote.ReferenceNumber = CreditNoteRefNo.Text;
            if(! string.IsNullOrEmpty(TextBoxCreditNoteCustomer.Id))
            {
                Account Accounts = AccountManager.GetAccountById(long.Parse(TextBoxCreditNoteCustomer.Id));
                if (Accounts != null)
                {
                    lCreditNote.AccountId = Accounts.Id;
                }
            }
            lCreditNote.TransactionDate = (DateTime)DateTimePickerCreditNote.Date!;
            lCreditNote.CreditNoteId = TextBoxCreditNoteId.Text == string.Empty ? 0L : Convert.ToInt64(TextBoxCreditNoteId.Text);
            lCreditNote.Note = TextBoxCreditNoteNote.Text;
            lCreditNote.InternalNote = TextBoxCreditNoteInternalNote.Text;
            lCreditNote.Amount = (float.Parse(DataGridViewCreditNoteDetailsTotal.Rows[0].Cells[1].Value.ToString()!));
            lCreditNote.CompanyId = Global.Company.CompanyId;
            for (int i = 0; i < DataGridViewCreditNoteDetails.Rows.Count - 1; i++)
            {
                CreditNoteDetail CreditNoteDetail = new CreditNoteDetail();
                Account Account = AccountManager.Instance.GetAccountById((long)DataGridViewCreditNoteDetails.Rows[i].Cells[1].Value);
                if (Account != null)
                {
                    CreditNoteDetail.AccountId = Account.Id;
                    CreditNoteDetail.Description = (DataGridViewCreditNoteDetails.Rows[i].Cells[2].Value != null) ? DataGridViewCreditNoteDetails.Rows[i].Cells[2].Value.ToString() : "";
                    CreditNoteDetail.Amount = (float.Parse(DataGridViewCreditNoteDetails.Rows[i].Cells[3].Value.ToString()!));
                    lCreditNote.CreditNoteDetails.Add(CreditNoteDetail);
                }
            }
            return lCreditNote;
        }

        private void BtnCreditNoteNew_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                {
                    if (ValidateForm())
                    {
                        BtnCreditNoteSave_Click(sender, e);
                    }
                }
                if (Result == DialogResult.Cancel)
                {
                    TextBoxCreditNoteCustomer.Select();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            TextBoxCreditNoteCustomer.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }

        private void BtnCreditNoteDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxCreditNoteId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this creditnote is still valid.");
                return;
            }
            DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, CreditNoteRefNo.Text), "Delete Confirm",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                bool DeleteResult = CreditNoteManager.DeleteCreditNote(Convert.ToInt64(TextBoxCreditNoteId.Text));
                if (DeleteResult)
                {
                    ResetForm();
                    EnableForm(true);
                    BtnCreditNoteNew.Select();
                    this.formIsDirty = false;
                }
                else
                {
                    DisplaySystemError("Somting went wrong, please check this creditnote is still valid.");
                    return;
                }
                Cursor.Current = Cursors.Default;
            }
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            BtnCreditNoteCancel.PerformClick();
            return;
        }
        private void BtnCreditNotePrint_Click(object sender, EventArgs e)
        {
            if (CreditNoteManager.GetCreditNote(long.Parse(TextBoxCreditNoteId.Text)) != null)
            {
                PrinterSetup.TransactionPrintSetup(long.Parse(TextBoxCreditNoteId.Text), TransactionTypes.CREDITNOTE);
            }
            else
            {
                DisplaySystemError("Somting went wrong, please check this credit note is still valid.");
                return;
            }
        }
        private void BtnCreditNoteCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxCreditNoteCustomer.Select();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(false);
            BtnCreditNoteNew.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void BtnCreditNoteSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Cursor.Current = Cursors.WaitCursor;
                CreditNote lCreditNote = GetCreditNoteFromForm();
                //calculation
                lCreditNote.Balance = lCreditNote.Amount;
                lCreditNote.Paid = 0F;
                if (lCreditNote != null)
                {
                    if (lCreditNote.CreditNoteId == 0)
                    {
                        string RefNo = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.CREDIT_NOTE, (DateTime)DateTimePickerCreditNote.Date);
                        if (!string.IsNullOrEmpty(RefNo))
                        {
                            lCreditNote.ReferenceNumber = RefNo;
                            CreditNoteManager.AddCreditNote(lCreditNote);
                        }
                        else
                        {
                            ToolStripStatusLabelErrorCreditNote.Text = RefNoErrorMsg;
                            return;
                        }
                    }
                    else
                    {
                        if (CreditNoteManager.GetCreditNote(lCreditNote.CreditNoteId) != null)
                        {
                            CreditNoteManager.UpdateCreditNote(lCreditNote);
                        }
                        else
                        {
                            DisplaySystemError("Somting went wrong, please check this creditnote is still valid.");
                            return;
                        }
                    }

                    TextBoxCreditNoteId.Text = lCreditNote.CreditNoteId.ToString();
                    CreditNoteRefNo.Text = lCreditNote.ReferenceNumber;
                    LastCreditNoteRefNo.Text = CompanyManager.Instance.GetAccountPrevRef(Global.Company, EntryType.CREDIT_NOTE, (DateTime)DateTimePickerCreditNote.Date!);
                    EnableForm(true);
                    BtnCreditNoteDelete.Enabled = true;
                    BtnCreditNotePrint.Enabled = true;
                    BtnCreditNoteNew.Enabled = true;
                    BtnCreditNotePrint.Select();
                    ToolStripStatusLabelErrorCreditNote.Text = SaveSuccessText;
                    this.formIsDirty = false;
                    Cursor.Current = Cursors.Default;
                }
            }
        }
        private Boolean ValidateForm()
        {
            ToolStripStatusLabelErrorCreditNote.Text = "";
            if (string.IsNullOrEmpty(TextBoxCreditNoteCustomer.Text.Trim()))
            {
                ToolStripStatusLabelErrorCreditNote.Text = ChooseCustomeErrorMsg;
                TextBoxCreditNoteCustomer.Select();
                return false;
            }
            if (!string.IsNullOrEmpty(TextBoxCreditNoteCustomer.Text.Trim()) && AccountManager.Instance.GetAccountById(long.Parse(TextBoxCreditNoteCustomer.Id)) == null)
            {
                ToolStripStatusLabelErrorCreditNote.Text = "Somting went wrong, please check this account is still valid.";
                TextBoxCreditNoteCustomer.Select();
                return false;
            }
            if (DateTimePickerCreditNote.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerCreditNote.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ToolStripStatusLabelErrorCreditNote.Text = EnterCreditNoteDateErrorMsg;
                DateTimePickerCreditNote.Select();
                return false;
            }

            int Count = DataGridViewCreditNoteDetails.Rows.Count;
            if (Count > 1)
            {
                for (int i = 0; i < Count - 1; i++)
                {
                    for (int j = 1; j < 4; j++)
                    {
                        var cellValue = DataGridViewCreditNoteDetails.Rows[i].Cells[j].Value;

                        string creditDecimalValue = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);

                        if (cellValue == null || cellValue == DBNull.Value || string.IsNullOrWhiteSpace(cellValue?.ToString()) ||
                            (decimal.TryParse(cellValue?.ToString(), out decimal valuednumber) && valuednumber == 0.00m) ||
                            cellValue!.ToString()!.Equals(creditDecimalValue))  
                        {
                            ToolStripStatusLabelErrorCreditNote.Text = string.Format(Grid_MantatoryFiledErrorMsg, DataGridViewCreditNoteDetails.Columns[j].HeaderText);
                            DataGridViewCreditNoteDetails.Select();
                            DataGridViewCreditNoteDetails.CurrentCell = DataGridViewCreditNoteDetails.Rows[i].Cells[j]; 
                            DataGridViewCreditNoteDetails.BeginEdit(true);
                            return false;
                        }
                    }
                }
            }
            else
            {
                ToolStripStatusLabelErrorCreditNote.Text = Grid_EmptyErrorMsg;
                DataGridViewCreditNoteDetails.Select();
                DataGridViewCreditNoteDetails.CurrentCell = DataGridViewCreditNoteDetails.Rows[0].Cells[1]; 
                DataGridViewCreditNoteDetails.BeginEdit(true);
                return false;
            }
            return true;
        }
        private void ResetForm()
        {
            ToolStripStatusLabelErrorCreditNote.Text = "";
            CreditNoteRefNo.Text = "000000";
            TextBoxSearchCreditNote.TextBox.ResetText();
            DateTimePickerCreditNote.Format = Global.Company.DateFormat; TextBoxCreditNoteId.ResetText();
            DateTimePickerCreditNote.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            DateTimePickerCreditNote.MinDate = Global.getCurrentFiscalYearStartDate();
            DateTimePickerCreditNote.MaxDate = Global.getCurrentFiscalYearEndDate();
            TextBoxCreditNoteInternalNote.ResetText();
            TextBoxCreditNoteNote.ResetText();
            DataGridViewCreditNoteDetails.Rows.Clear();
            DataGridViewCreditNoteDetailsTotal.Rows[0].Cells[0].Value = "Total : ";
            DataGridViewCreditNoteDetailsTotal.Rows[0].Cells[1].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            TextBoxCreditNoteCustomer.ResetText();
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)DataGridViewCreditNoteDetails.Columns["Amount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            LastCreditNoteRefNo.Text = CompanyManager.Instance.GetAccountPrevRef(Global.Company, EntryType.CREDIT_NOTE, (DateTime)DateTimePickerCreditNote.Date!);
        }
        private void EnableForm(Boolean enable)
        {
            TextBoxCreditNoteInternalNote.ReadOnly = !enable;
            TextBoxCreditNoteNote.ReadOnly = !enable;
            DataGridViewCreditNoteDetails.Enabled = enable;
            DataGridViewCreditNoteDetailsTotal.Enabled = enable;

            TextBoxCreditNoteInternalNote.TabStop = enable;
            TextBoxCreditNoteNote.TabStop = enable;

            TextBoxCreditNoteCustomer.ReadOnly = !enable;
            TextBoxCreditNoteCustomer.TabStop = enable;
            DateTimePickerCreditNote.Enabled = enable;
            if (enable)
            {
                if (string.IsNullOrEmpty(TextBoxCreditNoteId.Text))
                {
                    BtnCreditNoteNew.Enabled = !enable;
                    BtnCreditNoteDelete.Enabled = !enable;
                    BtnCreditNotePrint.Enabled = !enable;
                }
                else
                {
                    BtnCreditNoteNew.Enabled = enable;
                    BtnCreditNoteDelete.Enabled = enable;
                    BtnCreditNotePrint.Enabled = enable;
                }
                BtnCreditNoteSave.Enabled = enable;
                BtnCreditNoteCancel.Enabled = enable;
            }
            else
            {
                BtnCreditNoteNew.Enabled = !enable;
                BtnCreditNoteDelete.Enabled = enable;
                BtnCreditNotePrint.Enabled = enable;
                BtnCreditNoteSave.Enabled = enable;
                BtnCreditNoteCancel.Enabled = enable;
            }
        }

        private void TextBoxCreditNoteDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                DateTimePickerCreditNote.Focus();
                SendKeys.Send("{F4}");
            }
        }

        private void DateTimePickerCreditNote_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                SendKeys.Send("{F4}");
            }
        }

        //private void ComboBoxCreditNoteCustomer_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    this.ComboBoxCreditNoteCustomer.DroppedDown = false;
        //}      

        private void DataGridViewCreditNoteDetails_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewCreditNoteDetails.Rows[e.RowIndex].Cells[0].Value = DataGridViewCreditNoteDetails.Rows.Count;
                DataGridViewCreditNoteDetails.Rows[e.RowIndex].Cells[2].Value = "";
                DataGridViewCreditNoteDetails.Rows[e.RowIndex].Cells[3].Value = "0.00";
                DataGridViewCreditNoteDetails.Rows[e.RowIndex].Cells[4].Value = "X";
                AddAccount(e.RowIndex);
            }
        }
        private void AddAccount(int Index)
        {
            IList<Account> Account = AccountManager.ListCreditNoteService(Global.Company.CompanyId, Global.IncludedAccountCreditNoteService);
            if (Account != null)
            {
                (DataGridViewCreditNoteDetails.Rows[Index].Cells[1] as DataGridViewComboBoxCell).DataSource = null;
                (DataGridViewCreditNoteDetails.Rows[Index].Cells[1] as DataGridViewComboBoxCell).DataSource = Account;
                (DataGridViewCreditNoteDetails.Rows[Index].Cells[1] as DataGridViewComboBoxCell).ValueMember = "Id";
                (DataGridViewCreditNoteDetails.Rows[Index].Cells[1] as DataGridViewComboBoxCell).DisplayMember = "Name";
            }
        }
        private void DataGridViewCreditNoteDetails_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F2))
            {
                BtnCreditNoteSearchCustomer.PerformClick();
                return true;
            }
            if (keyData == (Keys.F3))
            {
                if (BtnCreditNoteNew.Enabled)
                {
                    BtnCreditNoteNew.PerformClick();
                }
                else
                {
                    BtnCreditNoteNewCustomers.ShowDropDown();
                }
            }
            else if (keyData == (Keys.F4))
            {
                BtnCreditNoteDelete.PerformClick();
            }
            else if (keyData == (Keys.F9))
            {
                BtnCreditNotePrint.PerformClick();
            }
            else if (keyData == (Keys.F8))
            {
                BtnCreditNoteSave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCreditNoteCancel.PerformClick();
            }
            else if (keyData == (Keys.F10))
            {
                BtnCreditNoteExit.PerformClick();
                return true;
            }
            if (keyData == Keys.Left && ActiveControl == BtnCreditNoteSave)
            {
                BtnCreditNoteCancel.Select();
                return true;
            }
            if (keyData == Keys.Right && ActiveControl == BtnCreditNoteSave)
            {
                BtnCreditNoteExit.Select();
                return true;
            }
            if (keyData == Keys.Right && ActiveControl == BtnCreditNoteCancel)
            {
                BtnCreditNoteSave.Select();
                return true;
            }
            if (keyData == Keys.Left && ActiveControl == BtnCreditNoteCancel)
            {
                BtnCreditNoteExit.Select();
                return true;
            }
            if (keyData == Keys.Left && ActiveControl == BtnCreditNoteExit)
            {
                BtnCreditNoteSave.Select();
                return true;
            }
            if (keyData == Keys.Right && ActiveControl == BtnCreditNoteExit)
            {
                BtnCreditNoteCancel.Select();
                return true;
            }
            if (keyData == Keys.Tab && this.ActiveControl == toolStrip1 && BtnSearchCreditNote.Selected)
            {
                TextBoxCreditNoteCustomer.Select();
                return true;
            }
            try
            {
                if (DataGridViewCreditNoteDetails.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && DataGridViewCreditNoteDetails.CurrentCell.ColumnIndex == 3)
                    {
                        if (DataGridViewCreditNoteDetails.CurrentCell.RowIndex != DataGridViewCreditNoteDetails.Rows.Count - 1)
                        {
                            SendKeys.Send("{tab}{tab}");
                        }
                        else
                        {
                            SendKeys.Send("{tab}");
                        }
                    }
                    if (keyData == (Keys.Shift | Keys.Tab) && DataGridViewCreditNoteDetails.CurrentCell.ColumnIndex == 1)
                    {
                        if (DataGridViewCreditNoteDetails.CurrentRow.Index != 0)
                        {
                            SendKeys.Send("{tab}{tab}");
                        }
                        else
                        {
                            SendKeys.Send("{tab}");
                        }
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void DataGridViewCreditNoteDetails_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCreditNoteDetails.Rows[e.RowIndex].Cells[0].ReadOnly = true;
            DataGridViewCreditNoteDetails.Rows[e.RowIndex].Cells[2].ReadOnly = true;
            DataGridViewCreditNoteDetails.Rows[e.RowIndex].Cells[3].ReadOnly = true;
            DataGridViewCreditNoteDetails.Rows[e.RowIndex].Cells[4].ReadOnly = true;
            if (DataGridViewCreditNoteDetails.Rows[e.RowIndex].Cells[1].Value != null)
            {
                DataGridViewCreditNoteDetails.Rows[e.RowIndex].Cells[2].ReadOnly = false;
                DataGridViewCreditNoteDetails.Rows[e.RowIndex].Cells[3].ReadOnly = false;
                DataGridViewCreditNoteDetails.Rows[e.RowIndex].Cells[4].ReadOnly = false;
            }
        }
        DataGridViewEditingControlShowingEventArgs BackupContextMenuStrip = null;
        private void DataGridViewCreditNoteDetails_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (BackupContextMenuStrip == null)
            {
                BackupContextMenuStrip = e;
            }
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                if (DataGridViewCreditNoteDetails.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }
                e.Control.KeyPress += new KeyPressEventHandler(DataGridViewBillDetails_KeyPress1);
            }

            if (DataGridViewCreditNoteDetails.CurrentCell.ColumnIndex == 3)
            {
                KeypressValidation.AddContextMenuGridCell(e, DataGridViewCreditNoteDetails, "NumberDot", DataGridViewCreditNoteDetails.CurrentCell.ColumnIndex);

                e.Control.KeyPress += new KeyPressEventHandler(DataGridViewCreditNoteDetails_KeyPress);
                if (e.Control is DataGridViewTextBoxEditingControl)
                {
                    DataGridViewTextBoxEditingControl tb = e.Control as DataGridViewTextBoxEditingControl;
                    tb.KeyDown += DataGridViewCreditNoteDetails_KeyDown;
                }
            }
            else
            {
                e.Control.ContextMenuStrip = BackupContextMenuStrip.Control.ContextMenuStrip;
            }
        }
        private void DataGridViewCreditNoteDetails_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                if (DataGridViewCreditNoteDetails.CurrentCell.ColumnIndex == 3)
                {
                    DataGridViewCreditNoteDetails.CurrentCell = DataGridViewCreditNoteDetails[1, DataGridViewCreditNoteDetails.CurrentRow.Index + 1];
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                DataGridViewCreditNoteDetails.Select();
                DataGridViewCreditNoteDetails.CurrentCell = DataGridViewCreditNoteDetails[3, DataGridViewCreditNoteDetails.RowCount - 1];
            }
            if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                KeypressValidation.Keypress_PasteChecking(sender, e, "NumberDot");
            }

        }
        private void DataGridViewBillDetails_KeyPress1(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)DataGridViewCreditNoteDetails.EditingControl).DroppedDown = false;
        }
        private void DataGridViewCreditNoteDetails_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (DataGridViewCreditNoteDetails.Rows.Count > 0)
            {
                if (DataGridViewCreditNoteDetails.CurrentCell.ColumnIndex == 3)
                {
                    KeypressValidation.Keypress_Number(sender, e);
                }
            }
        }
        private void DataGridViewCreditNoteDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            double TotalAmount = 0.00;
            if (DataGridViewCreditNoteDetails.CurrentCell.ColumnIndex == 3)
            {
                if (DataGridViewCreditNoteDetails.CurrentCell.Value == null || DataGridViewCreditNoteDetails.CurrentCell.Value.Equals(string.Empty))
                {
                    DataGridViewCreditNoteDetails.CurrentCell.Value = "0.00";
                }
                else
                {
                    DataGridViewCreditNoteDetails.CurrentCell.Value = String.Format("{0:0.00}", float.Parse(DataGridViewCreditNoteDetails.CurrentCell.Value.ToString()));
                }
                for (int i = 0; i < DataGridViewCreditNoteDetails.Rows.Count - 1; i++)
                {
                    double Amount = (DataGridViewCreditNoteDetails.Rows[i].Cells[3].Value) == null ? 0.00 : (float.Parse(DataGridViewCreditNoteDetails.Rows[i].Cells[3].Value.ToString()));
                    TotalAmount = TotalAmount + Amount;
                }
                DataGridViewCreditNoteDetailsTotal.Rows[0].Cells[1].Value = String.Format("{0:0.00}", TotalAmount);
            }
        }
        private void CalculateTotal()
        {
            double TotalAmount = 0.00;
            foreach (DataGridViewRow row in DataGridViewCreditNoteDetails.Rows)
            {
                double Amount = (row.Cells[3].Value) == null ? 0.00 : (float.Parse(row.Cells[3].Value.ToString()));
                TotalAmount = TotalAmount + Amount;
            }
            DataGridViewCreditNoteDetailsTotal.Rows[0].Cells[1].Value = String.Format("{0:0.00}", TotalAmount);
        }
        private void DataGridViewCreditNoteDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                double TotalAmount = 0;
                if (e.ColumnIndex == 4 && !DataGridViewCreditNoteDetails.Rows[e.RowIndex].Cells[4].ReadOnly)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, DataGridViewCreditNoteDetails.Rows[e.RowIndex].Cells[0].Value.ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        DataGridViewCreditNoteDetails.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        DataGridViewCreditNoteDetails.Rows.RemoveAt(e.RowIndex);
                        for (int i = 0; i < DataGridViewCreditNoteDetails.Rows.Count; i++)
                        {
                            DataGridViewCreditNoteDetails.Rows[i].Cells[0].Value = i + 1;
                        }
                        for (int i = 0; i < DataGridViewCreditNoteDetails.Rows.Count - 1; i++)
                        {
                            double Amount = (DataGridViewCreditNoteDetails.Rows[i].Cells[3].Value) == null ? 0.00 : (float.Parse(DataGridViewCreditNoteDetails.Rows[i].Cells[3].Value.ToString()));
                            TotalAmount = TotalAmount + Amount;
                        }
                        DataGridViewCreditNoteDetailsTotal.Rows[0].Cells[1].Value = String.Format("{0:0.00}", TotalAmount);
                    }
                }
            }
        }

        private void TextBoxCreditNoteInternalNote_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TextBoxCreditNoteInternalNote.SelectionLength = 0;
                e.IsInputKey = true;
                DataGridViewCreditNoteDetails.Select();
                DataGridViewCreditNoteDetails.CurrentCell = DataGridViewCreditNoteDetails[1, 0];
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TextBoxCreditNoteInternalNote.SelectionLength = 0;
                e.IsInputKey = true;
                TextBoxCreditNoteNote.Select();
            }
        }
        private void BtnCreditNoteSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                TextBoxCreditNoteCustomer.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (DataGridViewCreditNoteDetails.Rows.Count > 0)
                {
                    DataGridViewCreditNoteDetails.Select();
                    DataGridViewCreditNoteDetails.CurrentCell = DataGridViewCreditNoteDetails[3, DataGridViewCreditNoteDetails.Rows.Count - 1];
                }
            }
        }
        private void TextBoxCreditNoteCustomer_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                DateTimePickerCreditNote.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnCreditNoteSave.Select();
            }
        }



        private void TextBoxCreditNoteInternalNote_TextChanged(object sender, EventArgs e)
        {
            if (TextBoxCreditNoteInternalNote.Text.Contains("\t"))
            {
                TextBoxCreditNoteInternalNote.Text = TextBoxCreditNoteInternalNote.Text.Trim();
            }
        }

        private void BtnCreditNoteExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DataGridViewCreditNoteDetails_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                Account Acc = AccountManager.Instance.GetAccountByName(DataGridViewCreditNoteDetails.CurrentCell.EditedFormattedValue.ToString(), Global.Company.CompanyId);
                if (Acc != null)
                {
                    DataGridViewCreditNoteDetails.CurrentCell.Value = Acc.Id;
                }
            }
        }

        private void FormCreditNote_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    e.Cancel = true;
                    TextBoxCreditNoteCustomer.Select();
                }
            }
        }
        private void LoadCreditNote(long DebitId)
        {
            ResetForm();
            CreditNote CreditNote = CreditNoteManager.GetCreditNote(DebitId);
            if (CreditNote != null)
            {
                CreditNoteRefNo.Text = CreditNote.ReferenceNumber;
                TextBoxCreditNoteId.Text = CreditNote.CreditNoteId.ToString();
                TextBoxCreditNoteCustomer.Text = CreditNote.Account.Name;
                TextBoxCreditNoteCustomer.Id = CreditNote.Account.Id.ToString();
                DateTimePickerCreditNote.Date = (DateTime)DateUtils.ToDate(CreditNote.TransactionDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                TextBoxCreditNoteInternalNote.Text = CreditNote.InternalNote;
                TextBoxCreditNoteNote.Text = CreditNote.Note;
                if (CreditNote.CreditNoteDetails.Count > 0)
                {
                    DataGridViewCreditNoteDetails.Rows.Add(CreditNote.CreditNoteDetails.Count);
                    int i = 0;
                    foreach (var Details in CreditNote.CreditNoteDetails)
                    {
                        AddAccount(i);
                        DataGridViewCreditNoteDetails.Rows[i].Cells[(int)CreditNoteTableColumn.SNO].Value = i + 1;
                        DataGridViewCreditNoteDetails.Rows[i].Cells[(int)CreditNoteTableColumn.ACCOUNT].Value = Details.AccountId;
                        DataGridViewCreditNoteDetails.Rows[i].Cells[(int)CreditNoteTableColumn.DESC].Value = Details.Description;

                        DataGridViewCreditNoteDetails.Rows[i].Cells[(int)CreditNoteTableColumn.AMOUNT].Value = Details.Amount;
                        DataGridViewCreditNoteDetails.Rows[i].Cells[(int)CreditNoteTableColumn.ID].Value = Details.CreditNoteDetailsId;

                        i++;
                    }
                    ReSequence();
                    CalculateTotal();
                }
                EnableForm(true);
                TextBoxCreditNoteCustomer.Focus();
                this.formIsDirty = false;
            }
            else
            {
                SearchCreditNoteId = 0L;
                MessageBox.Show("Somting went wrong, please check this creditnote is still valid.");
                return;
            }
        }
        private void ReSequence()
        {
            for (int i = 0; i < DataGridViewCreditNoteDetails.Rows.Count; i++)
            {
                DataGridViewCreditNoteDetails.Rows[i].Cells[(int)CreditNoteTableColumn.SNO].Value = i + 1;
            }
        }
        private void BtnSearchCreditNote_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (isValidSearchCriteria())
            {
                RecentDebits();
                if (SearchCreditNoteId != 0)
                {
                    if (this.formIsDirty)
                    {
                        DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                        if (Result == DialogResult.Yes)
                        {
                            if (ValidateForm())
                            {
                                BtnCreditNoteSave_Click(sender, e);
                                LoadCreditNote(SearchCreditNoteId);
                            }
                        }
                        else if (Result == DialogResult.No)
                        {
                            LoadCreditNote(SearchCreditNoteId);
                        }
                    }
                    else
                    {
                        LoadCreditNote(SearchCreditNoteId);
                    }
                }
            }
            Cursor.Current = Cursors.Default;
        }
        private bool isValidSearchCriteria()
        {
            ToolStripStatusLabelErrorCreditNote.Text = "";
            if (string.IsNullOrEmpty(TextBoxSearchCreditNote.Text.Trim()))
            {
                ToolStripStatusLabelErrorCreditNote.Text = SearchBoxEmptyErrorMsg;
                TextBoxSearchCreditNote.TextBox.Focus();
                return false;
            }
            return true;
        }
        private void RecentDebits()
        {
            ToolStripStatusLabelErrorCreditNote.Text = "";
            String SearchText = TextBoxSearchCreditNote.Text.Trim();
            IList<CreditNote> CreditNoteInfo = null;

            if (TextUtils.isAmount(SearchText))
            {
                CreditNoteInfo = CreditNoteManager.GetCreditNoteByAmount(float.Parse(SearchText), SearchText, Global.Company.CompanyId);
            }
            else if (DateUtils.ValidDate(SearchText, Global.Company.DateFormat))
            {
                DateTime? Date = (DateTime)DateUtils.ToDate(SearchText, Global.Company.DateFormat);
                CreditNoteInfo = CreditNoteManager.GetCreditNoteByDate((DateTime)Date, Global.Company.CompanyId);
            }
            else
            {
                CreditNoteInfo = CreditNoteManager.GetCreditNoteByCustomerName(SearchText, Global.Company.CompanyId);
            }
            if (CreditNoteInfo.Count > 0)
            {
                LoadSales(CreditNoteInfo);
            }
            else
            {
                SearchCreditNoteId = 0L;
                ToolStripStatusLabelErrorCreditNote.Text = SearchOutput;
            }
        }
        public void LoadSales(IList<CreditNote> CreditNoteInfo)
        {
            SearchCreditNoteId = 0L;
            FormRecentDebitNote FormRecentDebitNote = new FormRecentDebitNote(this);
            FormRecentDebitNote.Text = "Recent Credit Notes";
            FormRecentDebitNote.CreditNoteInfo = CreditNoteInfo;
            FormRecentDebitNote.ShowDialog();
        }

       
        protected override void AccountIdTransportReload(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(AccountIdTransport.Text))
            {
                CustomerId = long.Parse(AccountIdTransport.Text);
            }
        }
        private void BtnInvoiceSearchCustomer_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            long? TempCustomerId = TextBoxCreditNoteCustomer.Id == null ? 0L : long.Parse(TextBoxCreditNoteCustomer.Id);
            CustomerId = 0L;
            FormAccountSearch FormSearchAccount = new FormAccountSearch(this);
            FormSearchAccount.IncludeCustomers = true;
            FormSearchAccount.IncludeSuppliers = true;
            FormSearchAccount.IncludeEmployees = true;
            FormSearchAccount.IncludeGeneralAccounts = true;
            FormSearchAccount.ShowDialog();
            if (CustomerId != 0)
            {
                Account Account = AccountManager.Instance.GetAccountById(CustomerId);
                if (Account != null)
                {
                    TextBoxCreditNoteCustomer.Text = Account.Name;
                    TextBoxCreditNoteCustomer.Id = CustomerId.ToString();
                }
                else
                {
                    CustomerId = (long)TempCustomerId;
                    MessageBox.Show("Somting went wrong, please check this account is still valid.");
                    return;
                }
            }
            else
            {
                if (TempCustomerId != null)
                {
                    CustomerId = (long)TempCustomerId;
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void TextBoxSearchCreditNote_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearchCreditNote.PerformClick();
            }
        }

        private void DataGridViewCreditNoteDetails_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 3)
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
                else
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                }
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.SelectionForeColor = Color.Black;
                e.CellStyle.SelectionBackColor = Color.White;
            }
        }

        private void BtnCreditNoteNewCustomers_ItemClickedEvent(object sender, ToolStripItemClickedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            bool IsDirty = this.formIsDirty;
            long? TempCustomerId = TextBoxCreditNoteCustomer.Id == null ? 0L : long.Parse(TextBoxCreditNoteCustomer.Id);
            CustomerId = 0L;
            if (e.ClickedItem.Text == "Account")
            {
                FormGeneralAccounts FormGeneralAccounts = new FormGeneralAccounts(this);
                FormGeneralAccounts.CreateAccountOnLoad = true;
                FormGeneralAccounts.ShowDialog(this);
                ResetToAccounts();
            }
            else if (e.ClickedItem.Text == "Supplier")
            {
                FormSupplier FormSupplier = new FormSupplier(this);
                FormSupplier.CreateSupplierOnLoad = true;
                FormSupplier.ShowDialog(this);
            }
            else if (e.ClickedItem.Text == "Customer")
            {
                FormCustomers FormCustomers = new FormCustomers(this);
                FormCustomers.CreateCustomerOnLoad = true;
                FormCustomers.ShowDialog(this);
            }
            else if (e.ClickedItem.Text == "Employee")
            {
                FormEmployee FormEmployee = new FormEmployee(this);
                FormEmployee.CreateEmployeeOnLoad = true;
                FormEmployee.ShowDialog(this);
            }
            if (CustomerId != 0)
            {
                Customer customer = CustomerManager.Instance.GetCustomerById(CustomerId);
                if (customer != null)
                {
                    TextBoxCreditNoteCustomer.Text = customer.Name;
                    TextBoxCreditNoteCustomer.Id = CustomerId.ToString();
                }
                else
                {
                    CustomerId = (long)TempCustomerId;
                    MessageBox.Show("Somting went wrong, please check this customer is still valid.");
                    return;
                }
            }
            else
            {
                if (TempCustomerId != null)
                {
                    CustomerId = (long)TempCustomerId;
                    this.formIsDirty = IsDirty;
                }
            }
            Cursor.Current = Cursors.Default;
        }
        private void ResetToAccounts()
        {
            List<AccountHelperData> Account = AccountHelper.GetHelpData(1, false, false, true, false, null, Global.Company);
            if (DataGridViewCreditNoteDetails.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in DataGridViewCreditNoteDetails.Rows)
                {
                    (row.Cells[1] as DataGridViewComboBoxCell).DataSource = Account;
                }
            }
        }
    }
}
