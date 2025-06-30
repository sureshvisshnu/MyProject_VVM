using fa.libraries.Validation;
using fa.api.Accounting;
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
    public enum DebitNoteTableColumn
    {
        SNO, ACCOUNT, DESC, AMOUNT, REMOVE, ID
    }
    public partial class FormDebitNote : FormBase
    {
        public long SupplierId = 0L;

        public static string SaveSuccessText = "Saved success...";
        public static string DeleteConfirmText = "Do you want to delete the DebitNote {0}?";
        public static string DeleteErrorText = "Error Deleting the DebitNote!, Please retry";
        public static string NotAllowDeleteErrorText = "Do not Delete DebitNote it used in payment";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string Grid_ConfirmRowDeleteText = "Do you want to delete row {0}?";
        public static string Grid_EmptyErrorMsg = "Please enter DebitNote details.";
        public static string Grid_MantatoryFiledErrorMsg = "Please enter {0}.";
        public static string SearchBoxEmptyErrorMsg = "Please enter search text, it could Supplier name or DebitNote Date or DebitNote Number.";
        public static string SearchOutput = "No Entry Found!";

        public static string ChooseSupplierErrorMsg = "Please choose Supplier";
        public static string EnterDebitNoteDateErrorMsg = "Please enter proper DebitNote Date";
        public static string RefNoErrorMsg = "Please contact administrator to generate reference number.";

        SupplierManager SupplierManager = null;
        DebitNoteManager DebitNoteManager = null;
        DateValidation DateValidation = null;
        KeypressValidation KeypressValidation = null;
        AccountManager AccountManager = null;

        public DebitNote DebitNoteFormData = null;
        public long SearchDebitNoteId = 0L;
        public FormDebitNote()
        {
            InitializeComponent();
            AccountManager = AccountManager.Instance;
            SupplierManager = SupplierManager.Instance;
            DebitNoteManager = DebitNoteManager.Instance;
            DateValidation = DateValidation.Instance;
            KeypressValidation = KeypressValidation.Instance;
            excludedObjects = new string[] { "toolStripDebiteNote" };
        }

        private void FormDebitNote_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                DateTimePickerDebitNote.Format = Global.Company.DateFormat;
                DateTimePickerDebitNote.MinDate = Global.getCurrentFiscalYearStartDate();
                DateTimePickerDebitNote.MaxDate = Global.getCurrentFiscalYearEndDate();
                EnableForm(true);
                if (DebitNoteFormData != null)
                {
                    LoadDebitNoteEdit(DebitNoteFormData);
                }
                DataGridViewDebitNoteDetails.Columns["Amount"].DefaultCellStyle.Format = "#######.##";
                this.formIsDirty = false;

            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private DebitNote LoadDebitNoteEdit(DebitNote DebitNoteEditData)
        {

            //  ComboBoxDebitNoteSupplier.SelectedItem = DebitNoteEditData.Supplier.DisplayAs;
            DateTimePickerDebitNote.Date = (DateTime)DateUtils.ToDate(DebitNoteEditData.TransactionDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            TextBoxDebitNoteNote.Text = DebitNoteEditData.Note;
            TextBoxDebitNoteInternalNote.Text = DebitNoteEditData.InternalNote;
            DebitNoteRefNo.Text = DebitNoteEditData.DebitNoteId.ToString();
            EnableForm(false);
            TextBoxDebitNoteNote.ReadOnly = false;
            TextBoxDebitNoteInternalNote.ReadOnly = false;
            DataGridViewDebitNoteDetails.ReadOnly = false;
            BtnDebitNoteNew.Enabled = false;
            BtnDebitNoteSave.Enabled = true;
            BtnDebitNoteCancel.Enabled = true;
#pragma warning disable 0219 // variable declared but not used.
            int index = -1;
#pragma warning restore 0219
            //foreach (var item in ComboBoxDebitNoteSupplier.Items)
            //{
            //    index++;
            //    if (item.ToString().ToLower().Equals(DebitNoteEditData.Supplier.DisplayAs.ToLower()))
            //    {
            //        ComboBoxDebitNoteSupplier.SelectedIndex = index;
            //    }
            //}

            TextBoxDebitNoteId.Text = DebitNoteEditData.DebitNoteId.ToString();

            IList<DebitNoteDetail> DebitNoteGridData = DebitNoteManager.GetAllDebitNoteById(DebitNoteEditData.DebitNoteId);
            //  DataGridViewDebitNoteDetails.DataSource = null;

            if (DebitNoteGridData != null)
            {
                IList<Account> Account = AccountManager.ListDebitNoteService(Global.Company.CompanyId, Global.IncludedAccountDebitNoteService);


                // DataGridViewDebitNoteDetails.DataSource = DebitNoteGridData;
                // DataGridViewDebitNoteDetails.Refresh();
                // DataGridViewDebitNoteDetails.ClearSelection();
                for (int i = 0; i < DebitNoteGridData.Count; i++)
                {
                    if (Account.Count > 0 && DebitNoteFormData != null)
                    {
                        //   this.DataGridViewDebitNoteDetails.Rows.Add(i, DebitNoteGridData[i].Account, DebitNoteGridData[i].Description, DebitNoteGridData[i].Amount);


                        this.DataGridViewDebitNoteDetails.Rows.Add();
                        DataGridViewDebitNoteDetails.Rows[i].Cells[0].Value = DataGridViewDebitNoteDetails.Rows.Count;
                        DataGridViewDebitNoteDetails.Rows[i].Cells[3].Value = "0.00";

                        DataGridViewDebitNoteDetails.Rows[i].Cells[4].Value = "X";
                        (DataGridViewDebitNoteDetails.Rows[i].Cells[1] as DataGridViewComboBoxCell).Items.Clear();
                        (DataGridViewDebitNoteDetails.Rows[i].Cells[1] as DataGridViewComboBoxCell).DataSource = Account;
                        (DataGridViewDebitNoteDetails.Rows[i].Cells[1] as DataGridViewComboBoxCell).ValueMember = "Id";
                        (DataGridViewDebitNoteDetails.Rows[i].Cells[1] as DataGridViewComboBoxCell)!.DisplayMember = "Name";

                        //  DataGridViewDebitNoteDetails.Rows[i].Cells[1].Selected = DebitNoteGridData[i].Account;
                        DataGridViewDebitNoteDetails.Rows[i].Cells[2].Value = DebitNoteGridData[i].Description;
                        DataGridViewDebitNoteDetails.Rows[i].Cells[3].Value = DebitNoteGridData[i].Amount;
                        DataGridViewDebitNoteDetails.Rows[i].Cells[1].Value = DebitNoteGridData[i].AccountId;
                    }

                }


            }

            return DebitNoteEditData;
        }

        private DebitNote GetDebitNoteFromForm()
        {
            DebitNote lDebitNote = new DebitNote();
            lDebitNote.DebitNoteId = TextBoxDebitNoteId.Text == string.Empty ? 0L : Convert.ToInt64(TextBoxDebitNoteId.Text);
            lDebitNote.ReferenceNumber = DebitNoteRefNo.Text;
            if(!string.IsNullOrEmpty(TextBoxDebitNoteSupplier.Id))
            {
                Account lAccount = AccountManager.GetAccountById(long.Parse(TextBoxDebitNoteSupplier.Id));
                if (lAccount != null)
                {
                    lDebitNote.AccountId = lAccount.Id;
                }
            }
            lDebitNote.TransactionDate = (DateTime)DateTimePickerDebitNote.Date!;
            lDebitNote.Note = TextBoxDebitNoteNote.Text;
            lDebitNote.InternalNote = TextBoxDebitNoteInternalNote.Text;
            lDebitNote.Amount = (float.Parse(DataGridViewDebitNoteDetailsTotal.Rows[0].Cells[1].Value.ToString()!));
            lDebitNote.CompanyId = Global.Company.CompanyId;
            for (int i = 0; i < DataGridViewDebitNoteDetails.Rows.Count - 1; i++)
            {
                DebitNoteDetail DebitNoteDetail = new DebitNoteDetail();
                Account Account = AccountManager.GetAccountById((long)DataGridViewDebitNoteDetails.Rows[i].Cells[1].Value);
                if (Account != null)
                {
                    DebitNoteDetail.AccountId = Account.Id;
                    DebitNoteDetail.Description = (DataGridViewDebitNoteDetails.Rows[i].Cells[2].Value != null) ? DataGridViewDebitNoteDetails.Rows[i].Cells[2].Value.ToString() : "";
                    DebitNoteDetail.Amount = (float.Parse(DataGridViewDebitNoteDetails.Rows[i].Cells[3].Value.ToString()!));
                    lDebitNote.DebitNoteDetails.Add(DebitNoteDetail);
                }
            }
            return lDebitNote;
        }

        //private void ComboBoxDebitNoteSupplier_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    this.ComboBoxDebitNoteSupplier.DroppedDown = false;
        //}

        private void BtnDebitNoteNew_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                {
                    if (ValidateForm())
                    {
                        BtnDebitNoteSave_Click(sender, e);
                    }
                }
                if (Result == DialogResult.Cancel)
                {
                    TextBoxDebitNoteSupplier.Select();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            TextBoxDebitNoteSupplier.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }

        private void BtnDebitNoteDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxDebitNoteId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this debitnote is still valid.");
                return;
            }
            DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, DebitNoteRefNo.Text), "Delete Confirm",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                bool DeleteResult = DebitNoteManager.DeleteDebitNote(Convert.ToInt64(TextBoxDebitNoteId.Text));
                if (DeleteResult)
                {
                    ResetForm();
                    EnableForm(true);
                    BtnDebitNoteNew.Select();
                    this.formIsDirty = false;
                }
                else
                {
                    DisplaySystemError("Somting went wrong, please check this debitnote is still valid.");
                    return;
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnDebitNotePrint_Click(object sender, EventArgs e)
        {
            if (DebitNoteManager.GetDebitNote(long.Parse(TextBoxDebitNoteId.Text)) != null)
            {
                PrinterSetup.TransactionPrintSetup(long.Parse(TextBoxDebitNoteId.Text), TransactionTypes.DEBITNOTE);
            }
            else
            {
                DisplaySystemError("Somting went wrong, please check this debit note is still valid.");
                return;
            }
        }

        private void BtnDebitNoteCancel_Click(object sender, EventArgs e)
        {

            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxDebitNoteSupplier.Select();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(false);
            BtnDebitNoteNew.Select();
            if (DebitNoteFormData != null)
            {
                this.Close();
            }
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }

        private void BtnDebitNoteSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Cursor.Current = Cursors.WaitCursor;
                DebitNote lDebitNote = GetDebitNoteFromForm();
                //calculation
                lDebitNote.Balance = lDebitNote.Amount;
                lDebitNote.Paid = 0F;
                if (lDebitNote.DebitNoteId == 0)
                {
                    string RefNo = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.DEBIT_NOTE, (DateTime)DateTimePickerDebitNote.Date);
                    if (!string.IsNullOrEmpty(RefNo))
                    {
                        lDebitNote.ReferenceNumber = RefNo;
                        DebitNoteManager.AddDebitNote(lDebitNote);
                    }
                    else
                    {
                        ToolStripStatusLabelErrorDebitNote.Text = RefNoErrorMsg;
                        return;
                    }
                }
                else
                {
                    if (DebitNoteManager.Instance.GetDebitNotes(lDebitNote.DebitNoteId) != null)
                    {
                        DebitNoteManager.UpdateDebitNote(lDebitNote);
                    }
                    else
                    {
                        DisplaySystemError("Somting went wrong, please check this debitnote is still valid.");
                        return;
                    }

                }
                TextBoxDebitNoteId.Text = lDebitNote.DebitNoteId.ToString();
                DebitNoteRefNo.Text = lDebitNote.ReferenceNumber;
                LastDebitNoteRefNo.Text = CompanyManager.Instance.GetAccountPrevRef(Global.Company, EntryType.DEBIT_NOTE, (DateTime)DateTimePickerDebitNote.Date!);
                if (lDebitNote != null)
                {
                    EnableForm(true);
                    BtnDebitNoteDelete.Enabled = true;
                    BtnDebitNotePrint.Enabled = true;
                    BtnDebitNoteNew.Enabled = true;
                    BtnDebitNotePrint.Select();
                }
                ToolStripStatusLabelErrorDebitNote.Text = SaveSuccessText;
                this.formIsDirty = false;
                Cursor.Current = Cursors.Default;
            }
        }

        private Boolean ValidateForm()
        {
            ToolStripStatusLabelErrorDebitNote.Text = "";
            if (string.IsNullOrEmpty(TextBoxDebitNoteSupplier.Text.Trim()))
            {
                ToolStripStatusLabelErrorDebitNote.Text = ChooseSupplierErrorMsg;
                TextBoxDebitNoteSupplier.Select();
                return false;
            }
            if (!string.IsNullOrEmpty(TextBoxDebitNoteSupplier.Text.Trim()) && AccountManager.Instance.GetAccountById(long.Parse(TextBoxDebitNoteSupplier.Id)) == null)
            {
                ToolStripStatusLabelErrorDebitNote.Text = "Somting went wrong, please check this account is still valid.";
                TextBoxDebitNoteSupplier.Select();
                return false;
            }
            if (DateTimePickerDebitNote.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerDebitNote.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ToolStripStatusLabelErrorDebitNote.Text = EnterDebitNoteDateErrorMsg;
                DateTimePickerDebitNote.Select();
                return false;
            }

            int Count = DataGridViewDebitNoteDetails.Rows.Count;
            if (Count > 1)
            {
                for (int i = 0; i < Count - 1; i++)
                {
                    for (int j = 1; j < 4; j++)
                    {
                        var cellValue = DataGridViewDebitNoteDetails.Rows[i].Cells[j].Value;

                        string debitDecimalValue = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);


                        if (cellValue == null || cellValue == DBNull.Value || string.IsNullOrWhiteSpace(cellValue?.ToString()) ||
                            (decimal.TryParse(cellValue?.ToString(), out decimal valuednumber) && valuednumber == 0.00m) ||
                            cellValue!.ToString()!.Equals(debitDecimalValue))  
                        {
                            ToolStripStatusLabelErrorDebitNote.Text = string.Format(Grid_MantatoryFiledErrorMsg, DataGridViewDebitNoteDetails.Columns[j].HeaderText);
                            DataGridViewDebitNoteDetails.Select();
                            DataGridViewDebitNoteDetails.CurrentCell = DataGridViewDebitNoteDetails.Rows[i].Cells[j]; 
                            DataGridViewDebitNoteDetails.BeginEdit(true);
                            return false;
                        }
                    }
                }
            }
            else
            {
                ToolStripStatusLabelErrorDebitNote.Text = Grid_EmptyErrorMsg;
                DataGridViewDebitNoteDetails.Select();
                DataGridViewDebitNoteDetails.CurrentCell = DataGridViewDebitNoteDetails.Rows[0].Cells[1]; 
                DataGridViewDebitNoteDetails.BeginEdit(true);
                return false;
            }

            return true;
        }

        private void ResetForm()
        {
            TextBoxSearchDebitNote.TextBox.ResetText();
            ToolStripStatusLabelErrorDebitNote.Text = "";
            DebitNoteRefNo.Text = "000000";
            DateTimePickerDebitNote.Format = Global.Company.DateFormat;
            DateTimePickerDebitNote.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            DateTimePickerDebitNote.MinDate = Global.getCurrentFiscalYearStartDate();
            DateTimePickerDebitNote.MaxDate = Global.getCurrentFiscalYearEndDate();
            TextBoxDebitNoteId.ResetText();
            TextBoxDebitNoteInternalNote.ResetText();
            TextBoxDebitNoteNote.ResetText();
            DataGridViewDebitNoteDetails.Rows.Clear();
            DataGridViewDebitNoteDetailsTotal.Rows[0].Cells[0].Value = "Total : ";
            DataGridViewDebitNoteDetailsTotal.Rows[0].Cells[1].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            TextBoxDebitNoteSupplier.ResetText();
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)DataGridViewDebitNoteDetails.Columns["Amount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            LastDebitNoteRefNo.Text = CompanyManager.Instance.GetAccountPrevRef(Global.Company, EntryType.DEBIT_NOTE, (DateTime)DateTimePickerDebitNote.Date!);
        }

        private void EnableForm(Boolean enable)
        {
            TextBoxDebitNoteInternalNote.ReadOnly = !enable;
            TextBoxDebitNoteNote.ReadOnly = !enable;
            DataGridViewDebitNoteDetails.ReadOnly = !enable;
            DataGridViewDebitNoteDetailsTotal.ReadOnly = !enable;
            TextBoxDebitNoteInternalNote.TabStop = enable;
            TextBoxDebitNoteNote.TabStop = enable;
            DataGridViewDebitNoteDetails.TabStop = enable;
            DataGridViewDebitNoteDetailsTotal.TabStop = enable;
            TextBoxDebitNoteSupplier.ReadOnly = !enable;
            TextBoxDebitNoteSupplier.TabStop = enable;
            DateTimePickerDebitNote.Enabled = enable;


            if (enable)
            {
                if (string.IsNullOrEmpty(TextBoxDebitNoteId.Text))
                {
                    BtnDebitNoteNew.Enabled = !enable;
                    BtnDebitNoteDelete.Enabled = !enable;
                    BtnDebitNotePrint.Enabled = !enable;
                }
                else
                {
                    BtnDebitNoteNew.Enabled = enable;
                    BtnDebitNoteDelete.Enabled = enable;
                    BtnDebitNotePrint.Enabled = enable;
                }
                BtnDebitNoteSave.Enabled = enable;
                BtnDebitNoteCancel.Enabled = enable;
            }
            else
            {
                BtnDebitNoteNew.Enabled = !enable;
                BtnDebitNoteDelete.Enabled = enable;
                BtnDebitNotePrint.Enabled = enable;
                BtnDebitNoteSave.Enabled = enable;
                BtnDebitNoteCancel.Enabled = enable;
            }
        }

        private void DataGridViewDebitNoteDetails_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (DebitNoteFormData == null)
                {
                    DataGridViewDebitNoteDetails.Rows[e.RowIndex].Cells[0].Value = DataGridViewDebitNoteDetails.Rows.Count;
                    DataGridViewDebitNoteDetails.Rows[e.RowIndex].Cells[2].Value = "";
                    DataGridViewDebitNoteDetails.Rows[e.RowIndex].Cells[3].Value = "0.00";
                    DataGridViewDebitNoteDetails.Rows[e.RowIndex].Cells[4].Value = "X";
                    AddAccount(e.RowIndex);
                }
            }
        }
        private void AddAccount(int Index)
        {
            IList<Account> Account = AccountManager.ListDebitNoteService(Global.Company.CompanyId, Global.IncludedAccountDebitNoteService);
            if (Account.Count > 0)
            {
                (DataGridViewDebitNoteDetails.Rows[Index].Cells[1] as DataGridViewComboBoxCell).DataSource = null;
                (DataGridViewDebitNoteDetails.Rows[Index].Cells[1] as DataGridViewComboBoxCell).DataSource = Account;
                (DataGridViewDebitNoteDetails.Rows[Index].Cells[1] as DataGridViewComboBoxCell).ValueMember = "Id";
                (DataGridViewDebitNoteDetails.Rows[Index].Cells[1] as DataGridViewComboBoxCell).DisplayMember = "Name";
            }
        }
        private void DataGridViewDebitNoteDetails_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F2))
            {
                BtnDebitNoteSearchSupplier.PerformClick();
                return true;
            }
            if (keyData == (Keys.F3))
            {
                if (BtnDebitNoteNew.Enabled)
                {
                    BtnDebitNoteNew.PerformClick();
                }
                else
                {
                    if (TextBoxDebitNoteSupplier.Focused)
                        BtnDebitNoteNewSupplier.ShowDropDown();
                }
            }
            else if (keyData == (Keys.F4))
            {
                BtnDebitNoteDelete.PerformClick();
            }
            else if (keyData == (Keys.F9))
            {
                BtnDebitNotePrint.PerformClick();
            }
            else if (keyData == (Keys.F8))
            {
                BtnDebitNoteSave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnDebitNoteCancel.PerformClick();
            }
            else if (keyData == (Keys.F10))
            {
                BtnDebitNoteExit.PerformClick();
                return true;
            }
            //if (keyData == Keys.Tab && TextBoxSearchDebitNote.Selected)
            //{
            //    BtnSearchDebitNote.Select();
            //    return true;
            //}
            //if (keyData == Keys.Tab && BtnSearchDebitNote.Selected)
            //{
            //    TextBoxDebitNoteSupplier.Focus();
            //    return true;
            //}
            if (keyData == Keys.Left && ActiveControl == BtnDebitNoteSave)
            {
                BtnDebitNoteCancel.Select();
                return true;
            }
            if (keyData == Keys.Right && ActiveControl == BtnDebitNoteSave)
            {
                BtnDebitNoteExit.Select();
                return true;
            }
            if (keyData == Keys.Right && ActiveControl == BtnDebitNoteCancel)
            {
                BtnDebitNoteSave.Select();
                return true;
            }
            if (keyData == Keys.Left && ActiveControl == BtnDebitNoteCancel)
            {
                BtnDebitNoteExit.Select();
                return true;
            }
            if (keyData == Keys.Left && ActiveControl == BtnDebitNoteExit)
            {
                BtnDebitNoteSave.Select();
                return true;
            }
            if (keyData == Keys.Right && ActiveControl == BtnDebitNoteExit)
            {
                BtnDebitNoteCancel.Select();
                return true;
            }
            if (keyData == Keys.Tab && this.ActiveControl == toolStripDebiteNote && BtnSearchDebitNote.Selected)
            {
                TextBoxDebitNoteSupplier.Focus();
                return true;
            }
            try
            {
                if (DataGridViewDebitNoteDetails.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && DataGridViewDebitNoteDetails.CurrentCell.ColumnIndex == 3)
                    {
                        if (DataGridViewDebitNoteDetails.CurrentCell.RowIndex != DataGridViewDebitNoteDetails.Rows.Count - 1)
                        {
                            SendKeys.Send("{tab}{tab}");
                        }
                        else
                        {
                            SendKeys.Send("{tab}");
                        }
                    }
                    if (keyData == (Keys.Shift | Keys.Tab) && DataGridViewDebitNoteDetails.CurrentCell.ColumnIndex == 1)
                    {
                        if (DataGridViewDebitNoteDetails.CurrentRow.Index != 0)
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

        private void DataGridViewDebitNoteDetails_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewDebitNoteDetails.Rows[e.RowIndex].Cells[0].ReadOnly = true;
            DataGridViewDebitNoteDetails.Rows[e.RowIndex].Cells[2].ReadOnly = true;
            DataGridViewDebitNoteDetails.Rows[e.RowIndex].Cells[3].ReadOnly = true;
            DataGridViewDebitNoteDetails.Rows[e.RowIndex].Cells[4].ReadOnly = true;
            if (DataGridViewDebitNoteDetails.Rows[e.RowIndex].Cells[1].Value != null)
            {
                DataGridViewDebitNoteDetails.Rows[e.RowIndex].Cells[2].ReadOnly = false;
                DataGridViewDebitNoteDetails.Rows[e.RowIndex].Cells[3].ReadOnly = false;
                DataGridViewDebitNoteDetails.Rows[e.RowIndex].Cells[4].ReadOnly = false;
            }
        }
        DataGridViewEditingControlShowingEventArgs BackupContextMenuStrip = null;

        private void DataGridViewDebitNoteDetails_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
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
                if (DataGridViewDebitNoteDetails.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }
                e.Control.KeyPress += new KeyPressEventHandler(DataGridViewBillDetails_KeyPress1);
            }

            if (DataGridViewDebitNoteDetails.CurrentCell.ColumnIndex == 3)
            {
                KeypressValidation.AddContextMenuGridCell(e, DataGridViewDebitNoteDetails, "NumberDot", DataGridViewDebitNoteDetails.CurrentCell.ColumnIndex);

                e.Control.KeyPress += new KeyPressEventHandler(DataGridViewDebitNoteDetails_KeyPress);
                if (e.Control is DataGridViewTextBoxEditingControl)
                {
                    DataGridViewTextBoxEditingControl tb = e.Control as DataGridViewTextBoxEditingControl;
                    tb.KeyDown += DataGridViewDebitNoteDetails_KeyDown;
                }
            }
            else
            {
                e.Control.ContextMenuStrip = BackupContextMenuStrip.Control.ContextMenuStrip;

            }
        }
        private void DataGridViewDebitNoteDetails_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                KeypressValidation.Keypress_PasteChecking(sender, e, "NumberDot");
            }
        }
        private void DataGridViewBillDetails_KeyPress1(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)DataGridViewDebitNoteDetails.EditingControl).DroppedDown = false;
        }
        private void DataGridViewDebitNoteDetails_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (DataGridViewDebitNoteDetails.Rows.Count > 0)
            {
                if (DataGridViewDebitNoteDetails.CurrentCell.ColumnIndex == 3)
                {
                    KeypressValidation.Keypress_Number(sender, e);
                }
            }
        }

        private void DataGridViewDebitNoteDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                double TotalAmount = 0.00;
                if (e.ColumnIndex == 4 && !DataGridViewDebitNoteDetails.Rows[e.RowIndex].Cells[4].ReadOnly)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, DataGridViewDebitNoteDetails.Rows[e.RowIndex].Cells[0].Value.ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        DataGridViewDebitNoteDetails.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        DataGridViewDebitNoteDetails.Rows.RemoveAt(e.RowIndex);

                        for (int i = 0; i < DataGridViewDebitNoteDetails.Rows.Count; i++)
                        {
                            DataGridViewDebitNoteDetails.Rows[i].Cells[0].Value = i + 1;
                        }

                        for (int i = 0; i < DataGridViewDebitNoteDetails.Rows.Count - 1; i++)
                        {
                            double Amount = (DataGridViewDebitNoteDetails.Rows[i].Cells[3].Value) == null ? 0.00 : (float.Parse(DataGridViewDebitNoteDetails.Rows[i].Cells[3].Value.ToString()));
                            TotalAmount = TotalAmount + Amount;
                        }
                        DataGridViewDebitNoteDetailsTotal.Rows[0].Cells[1].Value = String.Format("{0:0.00}", TotalAmount);
                    }
                }
            }
        }

        private void DataGridViewDebitNoteDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            double TotalAmount = 0.00;
            if (DataGridViewDebitNoteDetails.CurrentCell.ColumnIndex == 3)
            {
                if (DataGridViewDebitNoteDetails.CurrentCell.Value == null || DataGridViewDebitNoteDetails.CurrentCell.Value.Equals(string.Empty))
                {
                    DataGridViewDebitNoteDetails.CurrentCell.Value = "0.00";
                }
                else
                {
                    DataGridViewDebitNoteDetails.CurrentCell.Value = String.Format("{0:0.00}", float.Parse(DataGridViewDebitNoteDetails.CurrentCell.Value.ToString()));
                }
                for (int i = 0; i < DataGridViewDebitNoteDetails.Rows.Count - 1; i++)
                {
                    double Amount = (DataGridViewDebitNoteDetails.Rows[i].Cells[3].Value) == null ? 0.00 : (float.Parse(DataGridViewDebitNoteDetails.Rows[i].Cells[3].Value.ToString()));
                    TotalAmount = TotalAmount + Amount;
                }
                DataGridViewDebitNoteDetailsTotal.Rows[0].Cells[1].Value = String.Format("{0:0.00}", TotalAmount);
            }
        }
        private void CalculateTotal()
        {
            double TotalAmount = 0.00;
            foreach (DataGridViewRow row in DataGridViewDebitNoteDetails.Rows)
            {
                double Amount = (row.Cells[3].Value) == null ? 0.00 : (float.Parse(row.Cells[3].Value.ToString()));
                TotalAmount = TotalAmount + Amount;
            }
            DataGridViewDebitNoteDetailsTotal.Rows[0].Cells[1].Value = String.Format("{0:0.00}", TotalAmount);
        }

        private void DataGridViewDebitNoteDetailsTotal_CellEnter_1(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewDebitNoteDetailsTotal.ReadOnly = true;
        }

        private void TextBoxDebitNoteInternalNote_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TextBoxDebitNoteInternalNote.SelectionLength = 0;
                e.IsInputKey = true;
                DataGridViewDebitNoteDetails.Select();
                DataGridViewDebitNoteDetails.CurrentCell = DataGridViewDebitNoteDetails[1, 0];
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TextBoxDebitNoteInternalNote.SelectionLength = 0;
                e.IsInputKey = true;
                TextBoxDebitNoteNote.Select();
            }
        }

        private void BtnDebitNoteSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxDebitNoteSupplier.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (DataGridViewDebitNoteDetails.Rows.Count > 0)
                {
                    DataGridViewDebitNoteDetails.Select();
                    DataGridViewDebitNoteDetails.CurrentCell = DataGridViewDebitNoteDetails[3, DataGridViewDebitNoteDetails.Rows.Count - 1];
                }
            }
        }
        private void TextBoxDebitNoteSupplier_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                DateTimePickerDebitNote.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnDebitNoteSave.Select();
            }
        }

        private void TextBoxDebitNoteInternalNote_TextChanged(object sender, EventArgs e)
        {
            if (TextBoxDebitNoteInternalNote.Text.Contains("\t"))
            {
                TextBoxDebitNoteInternalNote.Text = TextBoxDebitNoteInternalNote.Text.Trim();
            }
        }

        private void BtnDebitNoteExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DataGridViewDebitNoteDetails_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                Account Acc = AccountManager.Instance.GetAccountByName(DataGridViewDebitNoteDetails.CurrentCell.EditedFormattedValue.ToString(), Global.Company.CompanyId);
                if (Acc != null)
                {
                    DataGridViewDebitNoteDetails.CurrentCell.Value = Acc.Id;
                }
            }
        }

        private void FormDebitNote_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    e.Cancel = true;
                    TextBoxDebitNoteSupplier.Select();
                }
            }
        }
        private void LoadDebitNote(long DebitId)
        {
            ResetForm();
            DebitNote DebitNote = DebitNoteManager.GetDebitNote(DebitId);
            if (DebitNote != null)
            {
                DebitNoteRefNo.Text = DebitNote.ReferenceNumber;
                TextBoxDebitNoteId.Text = DebitNote.DebitNoteId.ToString();
                TextBoxDebitNoteSupplier.Text = DebitNote.Account.Name;
                TextBoxDebitNoteSupplier.Id = DebitNote.Account.Id.ToString();
                DateTimePickerDebitNote.Date = (DateTime)DateUtils.ToDate(DebitNote.TransactionDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                TextBoxDebitNoteInternalNote.Text = DebitNote.InternalNote;
                TextBoxDebitNoteNote.Text = DebitNote.Note;
                if (DebitNote.DebitNoteDetails.Count > 0)
                {
                    DataGridViewDebitNoteDetails.Rows.Add(DebitNote.DebitNoteDetails.Count);
                    int i = 0;
                    foreach (var Details in DebitNote.DebitNoteDetails)
                    {
                        AddAccount(i);
                        DataGridViewDebitNoteDetails.Rows[i].Cells[(int)DebitNoteTableColumn.SNO].Value = i + 1;
                        DataGridViewDebitNoteDetails.Rows[i].Cells[(int)DebitNoteTableColumn.ACCOUNT].Value = Details.AccountId;
                        DataGridViewDebitNoteDetails.Rows[i].Cells[(int)DebitNoteTableColumn.DESC].Value = Details.Description;

                        DataGridViewDebitNoteDetails.Rows[i].Cells[(int)DebitNoteTableColumn.AMOUNT].Value = Details.Amount;
                        DataGridViewDebitNoteDetails.Rows[i].Cells[(int)DebitNoteTableColumn.ID].Value = Details.DebitNoteDetailsId;

                        i++;
                    }
                    ReSequence();
                    CalculateTotal();
                }
                EnableForm(true);
                TextBoxDebitNoteSupplier.Focus();
                this.formIsDirty = false;
            }
            else
            {
                SearchDebitNoteId = 0L;
                MessageBox.Show("Somting went wrong, please check this debitnote is still valid.");
                return;
            }
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            BtnDebitNoteCancel.PerformClick();
            return;
        }
        private void ReSequence()
        {
            for (int i = 0; i < DataGridViewDebitNoteDetails.Rows.Count; i++)
            {
                DataGridViewDebitNoteDetails.Rows[i].Cells[(int)DebitNoteTableColumn.SNO].Value = i + 1;
            }
        }
        private void BtnSearchDebitNote_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (isValidSearchCriteria())
            {
                RecentDebits();
                if (SearchDebitNoteId != 0)
                {
                    if (this.formIsDirty)
                    {
                        DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                        if (Result == DialogResult.Yes)
                        {
                            if (ValidateForm())
                            {
                                BtnDebitNoteSave_Click(sender, e);
                                LoadDebitNote(SearchDebitNoteId);
                            }
                        }
                        else if (Result == DialogResult.No)
                        {
                            LoadDebitNote(SearchDebitNoteId);
                        }
                    }
                    else
                    {
                        LoadDebitNote(SearchDebitNoteId);
                    }
                }
            }
            Cursor.Current = Cursors.Default;
        }
        private bool isValidSearchCriteria()
        {
            ToolStripStatusLabelErrorDebitNote.Text = "";
            if (string.IsNullOrEmpty(TextBoxSearchDebitNote.Text.Trim()))
            {
                ToolStripStatusLabelErrorDebitNote.Text = SearchBoxEmptyErrorMsg;
                TextBoxSearchDebitNote.TextBox.Focus();
                return false;
            }
            return true;
        }
        private void RecentDebits()
        {
            ToolStripStatusLabelErrorDebitNote.Text = "";
            String SearchText = TextBoxSearchDebitNote.Text.Trim();
            IList<DebitNote> DebitNoteInfo = null;

            if (TextUtils.isAmount(SearchText))
            {
                DebitNoteInfo = DebitNoteManager.GetDebitNoteByAmount(float.Parse(SearchText), SearchText, Global.Company.CompanyId);
            }
            else if (DateUtils.ValidDate(SearchText, Global.Company.DateFormat))
            {
                DateTime? Date = (DateTime)DateUtils.ToDate(SearchText, Global.Company.DateFormat);
                DebitNoteInfo = DebitNoteManager.GetDebitNoteByDate((DateTime)Date, Global.Company.CompanyId);
            }
            else
            {
                DebitNoteInfo = DebitNoteManager.GetDebitNoteBySupplierName(SearchText, Global.Company.CompanyId);
            }
            if (DebitNoteInfo.Count > 0)
            {
                LoadSales(DebitNoteInfo);
            }
            else
            {
                SearchDebitNoteId = 0L;
                ToolStripStatusLabelErrorDebitNote.Text = SearchOutput;
            }
        }
        public void LoadSales(IList<DebitNote> DebitNoteInfo)
        {
            SearchDebitNoteId = 0L;
            FormRecentDebitNote FormRecentDebitNote = new FormRecentDebitNote(this);
            FormRecentDebitNote.DebitNoteInfo = DebitNoteInfo;
            FormRecentDebitNote.ShowDialog();
        }
        protected override void AccountIdTransportReload(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(AccountIdTransport.Text))
            {
                SupplierId = long.Parse(AccountIdTransport.Text);
            }
        }
        private void BtnDebitNoteSearchSupplier_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            long? TempSupplierId = TextBoxDebitNoteSupplier.Id == null ? 0L : long.Parse(TextBoxDebitNoteSupplier.Id);
            SupplierId = 0L;
            FormAccountSearch FormSearchAccount = new FormAccountSearch(this);
            FormSearchAccount.IncludeCustomers = true;
            FormSearchAccount.IncludeSuppliers = true;
            FormSearchAccount.IncludeEmployees = true;
            FormSearchAccount.IncludeGeneralAccounts = true;
            FormSearchAccount.ShowDialog();
            if (SupplierId != 0)
            {
                Account Account = AccountManager.Instance.GetAccountById(SupplierId);
                if (Account != null)
                {
                    TextBoxDebitNoteSupplier.Text = Account.Name;
                    TextBoxDebitNoteSupplier.Id = SupplierId.ToString();
                }
                else
                {
                    SupplierId = (long)TempSupplierId;
                    MessageBox.Show("Somting went wrong, please check this account is still valid.");
                    return;
                }
            }
            else
            {
                if (TempSupplierId != null)
                {
                    SupplierId = (long)TempSupplierId;
                }
            }
            TextBoxDebitNoteSupplier.Select();
            Cursor.Current = Cursors.Default;
        }

        private void TextBoxSearchDebitNote_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearchDebitNote.PerformClick();
                TextBoxDebitNoteSupplier.Select();
            }
        }

        private void DataGridViewDebitNoteDetails_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

        private void BtnExpenseNewSupplier_ItemClickedEvent(object sender, ToolStripItemClickedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            long? TempSupplierId = TextBoxDebitNoteSupplier.Id == null ? 0L : long.Parse(TextBoxDebitNoteSupplier.Id);
            SupplierId = 0L;
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
            if (SupplierId != 0)
            {
                Supplier Supplier = SupplierManager.Instance.GetSupplierById(SupplierId);
                if (Supplier != null)
                {
                    TextBoxDebitNoteSupplier.Text = Supplier.Name;
                    TextBoxDebitNoteSupplier.Id = SupplierId.ToString();
                }
                else
                {
                    SupplierId = (long)TempSupplierId;
                    MessageBox.Show("Somting went wrong, please check this supplier is still valid.");
                    return;
                }

            }
            else
            {
                if (TempSupplierId != null)
                {
                    SupplierId = (long)TempSupplierId;
                    this.formIsDirty = false;
                }
            }
            TextBoxDebitNoteSupplier.Select();
            Cursor.Current = Cursors.Default;
        }
        private void ResetToAccounts()
        {
            List<AccountHelperData> Account = AccountHelper.GetHelpData(1, false, false, true, false, null, Global.Company);
            if (DataGridViewDebitNoteDetails.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in DataGridViewDebitNoteDetails.Rows)
                {
                    (row.Cells[1] as DataGridViewComboBoxCell).DataSource = Account;
                }
            }
        }
    }
}
