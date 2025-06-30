using fa.libraries.utils;
using fa.libraries.Validation;
using fa.api.Accounting;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using fa.views.account.masters;
using fa.views.utils.Bills;
using fa.views.common;
using fa.views.utils;
using fa.views.controls.grid;
using fa.views.employee;
namespace fa.views.account.transactions
{
    public partial class FormBill : FormBase
    {
        public long SupplierId = 0L;
        public static string SaveSuccessText = "Saved success...";
        public static string DeleteConfirmText = "Do you want to delete the Bill {0}?";
        public static string DeleteErrorText = "Error Deleting the BILL!, Please retry";
        public static string NotAllowDeleteErrorText = "Do not Delete bill it used in payment";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string NewConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string Grid_ConfirmRowDeleteText = "Do you want to delete row {0}?";
        public static string Grid_EmptyErrorMsg = "Please enter bill details.";
        public static string Grid_MantatoryFiledErrorMsg = "Please enter {0}.";
        public static string SearchBoxEmptyErrorMsg = "Please enter search text, it could Supplier Name or Bill Date or Bill Number.";
        public static string SearchOutput = "No Entry Found!";
        public static string ChooseTermErrorMsg = "Please choose Term";
        public static string ChooseSupplierErrorMsg = "Please choose Supplier";
        public static string EnterBillDateErrorMsg = "Please enter proper Bill Date";
        public static string RefNoErrorMsg = "Please contact administrator to generate reference number.";
        SupplierManager SupplierManager = null;
        PaymentTermManager PaymentTermManager = null;
        BillManager BillManager = null;
        DateValidation DateValidation = null;
        AddressManager AddressManager = null;
        KeypressValidation KeypressValidation = null;
        BillSavePrintA4 BillSavePrintA4 = null;
        public FormBill()
        {
            InitializeComponent();
            SupplierManager = SupplierManager.Instance;
            PaymentTermManager = PaymentTermManager.Instance;
            BillManager = BillManager.Instance;
            DateValidation = DateValidation.Instance;
            KeypressValidation = KeypressValidation.Instance;
            AddressManager = AddressManager.Instance;
            BillSavePrintA4 = new BillSavePrintA4();
            excludedObjects = new string[] { "toolStrip1", "GridViewBillSearch" };
        }
        private void FormBill_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ComboUtils.InitializePaymentTermCombo(ComboBoxBillTerm, Global.Company.CompanyId);
                ResetForm();
                EnableForm(true);
                DataGridViewBillDetails.Columns["Amount"].DefaultCellStyle.Format = "#######.##";
                RecentBills();
                TextBoxBillSupplier.Select();
                this.formIsDirty = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private Bill GetBillFromForm()
        {
            Bill lBill = new Bill();
            lBill.BillId = TextBoxBillId.Text == string.Empty ? 0L : Convert.ToInt64(TextBoxBillId.Text);
            lBill.ReferenceNumber = BillReferenceNumber.Text;
            Account lAccount = AccountManager.Instance.GetAccountById(long.Parse(TextBoxBillSupplier.Id));
            if (lAccount != null)
            {
                lBill.VendorId = lAccount.Id;
            }
            PaymentTerm lPaymentTerm = (PaymentTerm)ComboBoxBillTerm.Items[ComboBoxBillTerm.SelectedIndex];
            if (lPaymentTerm != null)
            {
                PaymentTerm PaymentTerm = PaymentTermManager.GetPaymentTermById(lPaymentTerm.Id);
                if (PaymentTerm != null)
                {
                    lBill.TermId = PaymentTerm.Id;

                }
            }
            lBill.BillDate = (DateTime)DateTimePickerBillDate.Date;
            lBill.DueDate = (DateTime)DateUtils.ToDate(TextBoxBillDueDate.Text, Global.Company.DateFormat);
            lBill.Memo = TextBoxBillMemo.Text;
            lBill.InternalMemo = TextBoxBillMemo.Text;
            lBill.Total = (float.Parse(DataGridViewBillDetailsTotal.Rows[0].Cells[1].Value.ToString()));
            lBill.CompanyId = Global.Company.CompanyId;
            if (Global.CostCenter != null)
            {
                lBill.CostCenterId = Global.CostCenter.CostCenterId;
            }
            for (int i = 0; i < DataGridViewBillDetails.Rows.Count - 1; i++)
            {
                BillDetail BillDetail = new BillDetail();
                Account Account = AccountManager.Instance.GetAccountById((long)DataGridViewBillDetails.Rows[i].Cells[1].Value);
                if (Account != null)
                {
                    BillDetail.AccountId = Account.Id;
                    BillDetail.Description = (DataGridViewBillDetails.Rows[i].Cells[2].Value != null) ? DataGridViewBillDetails.Rows[i].Cells[2].Value.ToString() : "";
                    BillDetail.Amount = (float.Parse(DataGridViewBillDetails.Rows[i].Cells[3].Value.ToString()));
                    lBill.BillDetails.Add(BillDetail);
                }
            }
            lBill.BillAttachments = GetBillAttachmentFromFrom();
            return lBill;
        }
        private List<BillAttachment> GetBillAttachmentFromFrom()
        {
            List<BillAttachment> lBillAttachment = new List<BillAttachment>();
            if (OpenFileDialogBill.FileName != string.Empty)
            {
                BillAttachment BillAttachment = new BillAttachment();
                BillAttachment.Attachment = File.ReadAllBytes(OpenFileDialogBill.FileName);
                lBillAttachment.Add(BillAttachment);
            }
            return lBillAttachment;
        }
        private void DateTimePickerBillDate_Leave(object sender, EventArgs e)
        {
            if (DateTimePickerBillDate.Date != null && DateUtils.ValidDate(((DateTime)DateTimePickerBillDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DueDateChecking();
            }
            else
            {
                TextBoxBillDueDate.ResetText();
            }
        }
        private void ComboBoxBillTerm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxBillTerm.SelectedIndex > -1)
            {
                if (DateTimePickerBillDate.Date != null && DateUtils.ValidDate(((DateTime)DateTimePickerBillDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
                {
                    DueDateChecking();
                }
            }
        }
        private void TextBoxBillDueDate_Leave(object sender, EventArgs e)
        {
            if (!TextBoxBillDueDate.ReadOnly)
            {
                DueDateChecking();
            }
        }
        private void BtnBillAttachment_Click(object sender, EventArgs e)
        {
            OpenFileDialogBill.CheckFileExists = true;
            OpenFileDialogBill.AddExtension = true;
            OpenFileDialogBill.Filter = "All Files|*.*";
            if (OpenFileDialogBill.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var size = new FileInfo(OpenFileDialogBill.FileName).Length;
                if (size > 1048576)
                {
                }
                else
                {
                    TextBoxBillFilePath.Visible = true;
                    BtnBillFileDownload.Visible = true;
                    BtnBillFileDelete.Visible = true;
                    TextBoxBillFilePath.Text = OpenFileDialogBill.FileName.ToString();
                    TextBoxBillFilePath.SelectionStart = TextBoxBillFilePath.Text.Length;
                }
            }
        }
        private void BtnBillNew_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                {
                    BtnBillSave.PerformClick();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            TextBoxBillSupplier.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void BtnBillDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxBillId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this bill is still valid.");
                return;
            }
            long BillID = Convert.ToInt64(TextBoxBillId.Text);
            Bill Bill = BillManager.GetBill(BillID);
            if (Bill != null)
            {
                DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, BillReferenceNumber.Text), "Delete Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    if (Bill.Paid != 0)
                    {
                        ToolStripStatusLabelErrorBill.Text = NotAllowDeleteErrorText;
                        ResetTimmer();
                    }
                    else
                    {
                        bool DeleteResult = BillManager.DeleteBill(BillID);
                        if (DeleteResult)
                        {
                            ResetForm();
                            EnableForm(true);
                            if (string.IsNullOrEmpty(TextBoxBillSearch.Text))
                            {
                                RecentBills();
                            }
                            else
                            {
                                BillSearchGo_Click(sender, e);
                            }
                            TextBoxBillSupplier.Select();
                            this.formIsDirty = false;
                        }
                        else
                        {
                            MessageBox.Show(DeleteErrorText);
                        }
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            else
            {
                DisplaySystemError("Somting went wrong, please check this bill is still valid.");
                return;
            }
        }
        private void BtnBillPrint_Click(object sender, EventArgs e)
        {
            if (BillManager.GetBill(long.Parse(TextBoxBillId.Text)) != null)
            {
                PrinterSetup.TransactionPrintSetup(long.Parse(TextBoxBillId.Text), TransactionTypes.BILL);
            }
            else
            {
                DisplaySystemError("Somting went wrong, please check this bill is still valid.");
                return;
            }
        }
        private void BtnBillCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxBillSupplier.Select();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            TextBoxBillSupplier.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void BtnBillSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Cursor.Current = Cursors.WaitCursor;
                Bill lBill = GetBillFromForm();
                Bill lBillFromDB = null;
                //calculation
                lBill.Balance = lBill.Total;
                lBill.Paid = 0F;
                if (lBill.BillId == 0)
                {
                    string RefNo = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.BILL, (DateTime)DateTimePickerBillDate.Date);
                    if (!string.IsNullOrEmpty(RefNo))
                    {
                        lBillFromDB = new Bill();
                        lBill.ReferenceNumber = RefNo;
                        lBillFromDB = BillManager.AddBill(lBill);
                    }
                    else
                    {
                        ToolStripStatusLabelErrorBill.Text = RefNoErrorMsg;
                        return;
                    }
                }
                else
                {
                    Bill BillInfo = BillManager.GetBill(lBill.BillId);
                    if (BillInfo != null)
                    {
                        if (BillInfo.Total > lBill.Total)
                        {
                            lBill.Balance = BillInfo.Balance - (BillInfo.Total - lBill.Total);
                        }
                        else
                        {
                            lBill.Balance = BillInfo.Balance + (lBill.Total - BillInfo.Total);
                        }
                        lBill.Paid = BillInfo.Paid;
                        lBillFromDB = new Bill();
                        lBillFromDB = BillManager.UpdateBill(lBill);
                    }
                    else
                    {
                        DisplaySystemError("Somting went wrong, please check this bill is still valid.");
                        return;
                    }
                }
                TextBoxBillId.Text = lBillFromDB.BillId.ToString();
                BillReferenceNumber.Text = lBillFromDB.ReferenceNumber;
                LastBillReferenceNumber.Text = CompanyManager.Instance.GetAccountPrevRef(Global.Company, EntryType.BILL, (DateTime)DateTimePickerBillDate.Date!);
                if (string.IsNullOrEmpty(TextBoxBillSearch.Text))
                {
                    RecentBills();
                }
                else
                {
                    BillSearchGo_Click(sender, e);
                }
                if (lBillFromDB != null)
                {
                    EnableForm(false);
                    BtnBillPrint.Select();
                }
                ToolStripStatusLabelErrorBill.Text = SaveSuccessText;
                this.formIsDirty = false;
                Cursor.Current = Cursors.Default;
            }
        }
        private Boolean ValidateForm()
        {
            if (string.IsNullOrEmpty(TextBoxBillSupplier.Text.Trim()))
            {
                TextBoxBillSupplier.Select();
                ToolStripStatusLabelErrorBill.Text = ChooseSupplierErrorMsg;
                ResetTimmer();
                return false;
            }
            if (!string.IsNullOrEmpty(TextBoxBillSupplier.Text.Trim()) && AccountManager.Instance.GetAccountById(long.Parse(TextBoxBillSupplier.Id)) == null)
            {
                TextBoxBillSupplier.Select();
                ToolStripStatusLabelErrorBill.Text = "Somting went wrong, please check this vendor is still valid.";
                ResetTimmer();
                return false;
            }
            if (ComboBoxBillTerm.SelectedIndex < 0)
            {
                ComboBoxBillTerm.Select();
                ToolStripStatusLabelErrorBill.Text = ChooseTermErrorMsg;
                ResetTimmer();
                return false;
            }
            if (DateTimePickerBillDate.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerBillDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DateTimePickerBillDate.Select();
                ToolStripStatusLabelErrorBill.Text = EnterBillDateErrorMsg;
                ResetTimmer();
                return false;
            }
            int Count = DataGridViewBillDetails.Rows.Count;
            if (Count > 1)
            {
                for (int i = 0; i < Count - 1; i++)
                {
                    for (int j = 1; j < 4; j++)
                    {
                        if (j == 2) { continue; }
                        if (DataGridViewBillDetails.Rows[i].Cells[j].Value == null || DataGridViewBillDetails.Rows[i].Cells[j].Value.Equals(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)))
                        {
                            DataGridViewBillDetails.Select();
                            DataGridViewBillDetails.CurrentCell = DataGridViewBillDetails[j, i];
                            DataGridViewBillDetails.BeginEdit(true);
                            ToolStripStatusLabelErrorBill.Text = string.Format(Grid_MantatoryFiledErrorMsg, DataGridViewBillDetails.Columns[j].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                    }
                }
            }
            else
            {
                DataGridViewBillDetails.Select();
                DataGridViewBillDetails.CurrentCell = DataGridViewBillDetails[1, 0];
                DataGridViewBillDetails.BeginEdit(true);
                ToolStripStatusLabelErrorBill.Text = Grid_EmptyErrorMsg;
                ResetTimmer();
                return false;
            }
            
            return true;
        }
        private void ResetForm()
        {
            TextBoxBillFilePath.ResetText();
            BillReferenceNumber.Text = "000000";
            ToolStripStatusLabelErrorBill.Text = "";
            TextBoxBillSearch.TextBox.ResetText();
            DateTimePickerBillDate.Format = Global.Company.DateFormat;
            DateTimePickerBillDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
            DateTimePickerBillDate.MinDate = Global.getCurrentFiscalYearStartDate();
            DateTimePickerBillDate.MaxDate = Global.getCurrentFiscalYearEndDate();
            TextBoxBillDueDate.ResetText();
            this.TextBoxBillDueDate.Mask = DateValidation.ChangeMaskFormat((DateTime)DateTimePickerBillDate.Date);
            this.TextBoxBillDueDate.Text = ((DateTime)DateTimePickerBillDate.Date).ToString(Global.Company.DateFormat);
            OpenFileDialogBill.FileName = null;
            SaveFileDialogBill.FileName = null;
            TextBoxBillId.ResetText();
            TextBoxBillMemo.ResetText();
            TextBoxBillInternalMemo.ResetText();
            TextBoxBillSupplier.ResetText();
            ComboBoxBillTerm.ResetText();
            ComboBoxBillTerm.SelectedIndex = -1;
            DataGridViewBillDetails.Rows.Clear();
            DataGridViewBillDetailsTotal.Rows[0].Cells[0].Value = "Total : ";
            DataGridViewBillDetailsTotal.Rows[0].Cells[1].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)DataGridViewBillDetails.Columns["Amount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            LastBillReferenceNumber.Text = CompanyManager.Instance.GetAccountPrevRef(Global.Company, EntryType.BILL, (DateTime)DateTimePickerBillDate.Date!);
        }
        private void EnableForm(Boolean enable)
        {
            if (enable)
            {
                BtnBillNew.Enabled = !enable;
                BtnBillDelete.Enabled = !enable;
                BtnBillPrint.Enabled = !enable;
                BtnBillSave.Enabled = enable;
                BtnBillCancel.Enabled = enable;
                BtnPayablePayment.Enabled = !enable;
            }
            else
            {
                BtnBillNew.Enabled = !enable;
                BtnBillDelete.Enabled = !enable;
                BtnBillPrint.Enabled = !enable;
                BtnBillSave.Enabled = !enable;
                BtnBillCancel.Enabled = !enable;
                BtnPayablePayment.Enabled = !enable;
            }
        }
        private void ReSequence()
        {
            for (int i = 0; i < DataGridViewBillDetails.Rows.Count; i++)
            {
                DataGridViewBillDetails.Rows[i].Cells[0].Value = i + 1;
            }
        }
        private void ComputeFormTotal()
        {
            double TotalAmount = 0.00;
            for (int i = 0; i < DataGridViewBillDetails.Rows.Count - 1; i++)
            {
                double Amount = (DataGridViewBillDetails.Rows[i].Cells[3].Value) == null ? 0.00 : (float.Parse(DataGridViewBillDetails.Rows[i].Cells[3].Value.ToString()));
                TotalAmount = TotalAmount + Amount;
            }
            DataGridViewBillDetailsTotal.Rows[0].Cells[1].Value = TotalAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
        }
        private void DataGridViewBillDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 4 && (DataGridViewBillDetails.Rows.Count - 1) != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, DataGridViewBillDetails.Rows[e.RowIndex].Cells[0].Value.ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        DataGridViewBillDetails.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        DataGridViewBillDetails.Rows.RemoveAt(e.RowIndex);
                        Row_Removed();
                    }
                }
            }
        }
        private void Row_Removed()
        {
            ReSequence();
            ComputeFormTotal();
        }
        private void DataGridViewBillDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (DataGridViewBillDetails.CurrentCell.ColumnIndex == 3)
            {
                if (DataGridViewBillDetails.CurrentCell.Value == null || DataGridViewBillDetails.CurrentCell.Value.Equals(string.Empty))
                {
                    DataGridViewBillDetails.CurrentCell.Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                }
                else
                {
                    DataGridViewBillDetails.CurrentCell.Value = float.Parse(DataGridViewBillDetails.CurrentCell.Value.ToString()).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                }
                Row_Added();
            }
        }
        private void Row_Added()
        {
            ComputeFormTotal();
        }
        private void DataGridViewBillDetails_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                SendKeys.Send("{tab}");
            }
            DataGridViewBillDetails.Rows[e.RowIndex].Cells[0].ReadOnly = true;
            DataGridViewBillDetails.Rows[e.RowIndex].Cells[2].ReadOnly = true;
            DataGridViewBillDetails.Rows[e.RowIndex].Cells[3].ReadOnly = true;
            DataGridViewBillDetails.Rows[e.RowIndex].Cells[4].ReadOnly = true;
            if (DataGridViewBillDetails.Rows[e.RowIndex].Cells[1].Value != null)
            {
                DataGridViewBillDetails.Rows[e.RowIndex].Cells[2].ReadOnly = false;
                DataGridViewBillDetails.Rows[e.RowIndex].Cells[3].ReadOnly = false;
                DataGridViewBillDetails.Rows[e.RowIndex].Cells[4].ReadOnly = false;
            }
        }
        private void DataGridViewBillDetails_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        DataGridViewEditingControlShowingEventArgs BackupContextMenuStrip = null;
        private void DataGridViewBillDetails_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
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
                ((ComboBox)e.Control).FormattingEnabled = true;
                if (DataGridViewBillDetails.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }
                e.Control.KeyPress += new KeyPressEventHandler(DataGridViewBillDetails_KeyPress1);
            }
            else
            {
                e.Control.ContextMenuStrip = BackupContextMenuStrip.Control.ContextMenuStrip;
            }
        }
        private void DataGridViewBillDetails_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            IList<Account> Account = AccountManager.Instance.GetAllGeneralAccountsByCompanyId(Global.Company.CompanyId);
            if (Account != null)
            {
                DataGridViewBillDetails.Rows[e.RowIndex].Cells[0].Value = DataGridViewBillDetails.Rows.Count;
                DataGridViewBillDetails.Rows[e.RowIndex].Cells[3].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                DataGridViewBillDetails.Rows[e.RowIndex].Cells[4].Value = "X";
                (DataGridViewBillDetails.Rows[e.RowIndex].Cells[1] as DataGridViewComboBoxCell).DataSource = null;
                (DataGridViewBillDetails.Rows[e.RowIndex].Cells[1] as DataGridViewComboBoxCell).DataSource = Account;
                (DataGridViewBillDetails.Rows[e.RowIndex].Cells[1] as DataGridViewComboBoxCell).ValueMember = "Id";
                (DataGridViewBillDetails.Rows[e.RowIndex].Cells[1] as DataGridViewComboBoxCell).DisplayMember = "Name";
                (DataGridViewBillDetails.Rows[e.RowIndex].Cells[1] as DataGridViewComboBoxCell).AutoComplete = true;
            }
        }
        private void DataGridViewBillDetails_KeyPress1(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)DataGridViewBillDetails.EditingControl).DroppedDown = false;
        }
        private void DataGridViewBillDetails_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (DataGridViewBillDetails.Rows.Count > 0 && DataGridViewBillDetails.CurrentCell != null)
            {
                DataGridViewBillDetails.CommitEdit(DataGridViewDataErrorContexts.Commit);
                if (DataGridViewBillDetails.CurrentCell.ColumnIndex == 3)
                {
                    if (DataGridViewBillDetails.CurrentCell.Value != null)
                    {
                        KeypressValidation.Keypress_NumberDot(sender, e, DataGridViewBillDetails.CurrentCell.Value.ToString());
                    }
                    else
                    {
                        KeypressValidation.Keypress_Number(sender, e);
                    }
                }
            }
        }
        private void DataGridViewBillDetails_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                KeypressValidation.Keypress_PasteChecking(sender, e, "NumberDot");
            }
        }
        private void BtnBillFileDownload_Click(object sender, EventArgs e)
        {
            if (OpenFileDialogBill.FileName != null)
            {
                string extesion = Path.GetExtension(OpenFileDialogBill.FileName);
                SaveFileDialogBill.Filter = "All Files|*.*";
                SaveFileDialogBill.DefaultExt = extesion;
                SaveFileDialogBill.AddExtension = true;
                SaveFileDialogBill.ShowDialog();
                if (SaveFileDialogBill.FileName != "")
                {
                    if (extesion == ".xlsx" || extesion == ".pdf" || extesion == ".txt" || extesion == ".docx")
                    {
                        File.WriteAllBytes(SaveFileDialogBill.FileName, File.ReadAllBytes(OpenFileDialogBill.FileName));
                    }
                    else if (extesion == ".Jpeg")
                    {
                        FileStream fs = (FileStream)SaveFileDialogBill.OpenFile();
                        Image.FromFile(OpenFileDialogBill.FileName).Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                        fs.Close();
                    }
                    else if (extesion == ".png")
                    {
                        FileStream fs = (FileStream)SaveFileDialogBill.OpenFile();
                        Image.FromFile(OpenFileDialogBill.FileName).Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                        fs.Close();
                    }
                }
            }
        }
        private void BtnBillFileDelete_Click(object sender, EventArgs e)
        {
            TextBoxBillFilePath.Visible = false;
            BtnBillFileDownload.Visible = false;
            BtnBillFileDelete.Visible = false;
            OpenFileDialogBill.FileName = null;
        }
        private void ComboBoxBillTerm_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxBillTerm.DroppedDown = false;
        }
        private void DueDateChecking()
        {
            if (DateTimePickerBillDate.Date != null)
            {
                if (ComboBoxBillTerm.SelectedIndex == 0)
                {
                    TextBoxBillDueDate.Text = ((DateTime)DateTimePickerBillDate.Date).Date.ToString(Global.Company.DateFormat);
                }
                if (ComboBoxBillTerm.SelectedIndex == 1)
                {
                    TextBoxBillDueDate.Text = ((DateTime)DateTimePickerBillDate.Date).AddDays(15).Date.ToString(Global.Company.DateFormat);
                }
                if (ComboBoxBillTerm.SelectedIndex == 2)
                {
                    TextBoxBillDueDate.Text = ((DateTime)DateTimePickerBillDate.Date).AddDays(30).Date.ToString(Global.Company.DateFormat);
                }
            }
        }
        private void BtnBillAttachment_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DataGridViewBillDetails.Select();
                DataGridViewBillDetails.CurrentCell = DataGridViewBillDetails[0, 0];
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxBillInternalMemo.Select();
            }
        }
        private void BtnBillCancel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnBillSave.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (DataGridViewBillDetails.Rows.Count > 0)
                {
                    DataGridViewBillDetails.Select();
                    DataGridViewBillDetails.CurrentCell = DataGridViewBillDetails[3, DataGridViewBillDetails.Rows.Count - 1];
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F2))
            {
                BtnBillSearchSupplier.PerformClick();
                return true;
            }
            if (keyData == (Keys.F3))
            {
                if (BtnBillNew.Enabled)
                {
                    BtnBillNew.PerformClick();
                }
                else
                {
                    if (TextBoxBillSupplier.Focused)
                        BtnBillNewSupplier.ShowDropDown();
                }
            }
            else if (keyData == (Keys.F4))
            {
                BtnBillDelete.PerformClick();
            }
            else if (keyData == (Keys.F9))
            {
                BtnBillPrint.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F8))
            {
                BtnBillSave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnBillCancel.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F10))
            {
                BtnBillExit.PerformClick();
                return true;
            }
            if (keyData == Keys.Tab && BillSearchGo.Selected)
            {
                TextBoxBillSupplier.Select();
                return true;
            }
            if (keyData == Keys.Left && ActiveControl == BtnBillSave)
            {
                BtnBillCancel.Select();
                return true;
            }
            if (keyData == Keys.Right && ActiveControl == BtnBillSave)
            {
                BtnBillExit.Select();
                return true;
            }
            if (keyData == Keys.Right && ActiveControl == BtnBillCancel)
            {
                BtnBillSave.Select();
                return true;
            }
            if (keyData == Keys.Left && ActiveControl == BtnBillCancel)
            {
                BtnBillExit.Select();
                return true;
            }
            if (keyData == Keys.Left && ActiveControl == BtnBillExit)
            {
                BtnBillSave.Select();
                return true;
            }
            if (keyData == Keys.Right && ActiveControl == BtnBillExit)
            {
                BtnBillCancel.Select();
                return true;
            }
            if (keyData == Keys.Tab && ActiveControl == TextBoxBillInternalMemo)
            {
                DataGridViewBillDetails.Select();
                DataGridViewBillDetails.CurrentCell = DataGridViewBillDetails[0, 0];
                DataGridViewBillDetails.BeginEdit(true);
                return true;
            }
            try
            {
                if (DataGridViewBillDetails.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && DataGridViewBillDetails.CurrentCell.ColumnIndex == 3)
                    {
                        SendKeys.Send("{tab}");
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && DataGridViewBillDetails.CurrentCell.ColumnIndex == 1 && DataGridViewBillDetails.CurrentRow.Index != 0)
                    {
                        SendKeys.Send("{tab}");
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        public int blinkCount;
        private void ResetTimmer()
        {
            blinkCount = 0;
            TimerBill.Stop();
            TimerBill.Start();
        }
        private void TimerBill_Tick(object sender, EventArgs e)
        {
            this.ToolStripStatusLabelErrorBill.Visible = !this.ToolStripStatusLabelErrorBill.Visible;
            blinkCount++;
            if (blinkCount == 3 * 2)
            {
                TimerBill.Stop();
                ToolStripStatusLabelErrorBill.Visible = true;
            }
        }
        private void BillSearchGo_Click(object sender, EventArgs e)
        {
            if (isValidSearchCriteria())
            {
                GridViewBillSearch.Rows.Clear();
                IList<Bill> BillInfo = SearchBill();
                LoadBill(BillInfo);
            }
        }
        private bool isValidSearchCriteria()
        {
            ToolStripStatusLabelErrorBill.Text = "";
            if (string.IsNullOrEmpty(TextBoxBillSearch.Text))
            {
                ToolStripStatusLabelErrorBill.Text = SearchBoxEmptyErrorMsg;
                TextBoxBillSearch.TextBox.Select();
                return false;
            }
            return true;
        }
        private IList<Bill> SearchBill()
        {
            string text = TextBoxBillSearch.Text;
            IList<Bill> BillInfo = null;
            if (TextUtils.isReferenceNumber(text))
            {
                BillInfo = BillManager.GetBillByReferenceNo(text, Global.Company.CompanyId);
            }
            else if (DateUtils.ValidDate(text, Global.Company.DateFormat))
            {
                DateTime? Date = (DateTime)DateUtils.ToDate(text, Global.Company.DateFormat);
                BillInfo = BillManager.GetBillByDate(Date, Global.Company.CompanyId);
            }
            else
            {
                BillInfo = BillManager.GetBillBySupplerName(text, Global.Company.CompanyId);
            }

            return BillInfo;
        }
        private void RecentBills()
        {
            GridViewBillSearch.Rows.Clear();
            IList<Bill> BillInfo = BillManager.GetRecentBills(Global.Company.CompanyId);
            LoadBill(BillInfo);
        }
        public void LoadBill(IList<Bill> BillInfo)
        {
            ToolStripStatusLabelErrorBill.Text = "";
            if (BillInfo.Count > 0)
            {
                GridViewBillSearch.Rows.Add(BillInfo.Count);
                int i = 0;
                foreach (var lBillInfo in BillInfo)
                {
                    GridViewBillSearch.Rows[i].Cells[0].Value = lBillInfo.ReferenceNumber;
                    GridViewBillSearch.Rows[i].Cells[1].Value = lBillInfo.BillDate.ToString(Global.Company.DateFormat);
                    GridViewBillSearch.Rows[i].Cells[2].Value = lBillInfo.Vendor.Name;
                    GridViewBillSearch.Rows[i].Cells[3].Value = lBillInfo.Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewBillSearch.Rows[i].Cells[4].Value = lBillInfo.BillId;
                    i++;
                }
            }
            else
            {
                ToolStripStatusLabelErrorBill.Text = SearchOutput;
            }
        }
        private void TextBoxBillSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BillSearchGo_Click(sender, e);
            }
        }
        private void TextBoxBillSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxBillSearch.Text))
            {
                RecentBills();
            }
        }
        private void GridViewBillSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                long InvoiceNumber = (long)GridViewBillSearch.Rows[e.RowIndex].Cells[4].Value;
                if (this.formIsDirty)
                {
                    DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                   MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                    if (Result == DialogResult.Yes)
                    {
                        if (ValidateForm())
                        {
                            BtnBillSave_Click(sender, e);
                            LoadBill(InvoiceNumber);
                        }
                    }
                    else if (Result == DialogResult.No)
                    {
                        LoadBill(InvoiceNumber);
                    }
                }
                else
                {
                    LoadBill(InvoiceNumber);
                }
            }
        }
        private void LoadBill(long BillId)
        {
            ResetForm();
            EnableForm(false);
            Bill Bill = BillManager.GetBill(BillId);
            if (Bill != null)
            {
                BillReferenceNumber.Text = Bill.ReferenceNumber;
                TextBoxBillId.Text = Bill.BillId.ToString();
                TextBoxBillSupplier.Text = Bill.Vendor.Name;
                TextBoxBillSupplier.Id = Bill.Vendor.Id.ToString();
                ComboBoxBillTerm.SelectedIndex = ComboBoxBillTerm.FindStringExact(Bill.Term.Name);
                DateTimePickerBillDate.Date = (DateTime)DateUtils.ToDate(Bill.BillDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                TextBoxBillDueDate.Text = Bill.DueDate.ToString(Global.Company.DateFormat);
                TextBoxBillMemo.Text = Bill.Memo;
                TextBoxBillInternalMemo.Text = Bill.InternalMemo;
                if (Bill.BillDetails.Count > 0)
                {
                    DataGridViewBillDetails.Rows.Add(Bill.BillDetails.Count);
                    int i = 0;
                    IList<Account> Account = AccountManager.Instance.GetAllGeneralAccountsByCompanyId(Global.Company.CompanyId);
                    foreach (var BillDetail in Bill.BillDetails)
                    {
                        (DataGridViewBillDetails.Rows[i].Cells[1] as DataGridViewComboBoxCell).DataSource = null;
                        (DataGridViewBillDetails.Rows[i].Cells[1] as DataGridViewComboBoxCell).DataSource = Account;
                        (DataGridViewBillDetails.Rows[i].Cells[1] as DataGridViewComboBoxCell).ValueMember = "Id";
                        (DataGridViewBillDetails.Rows[i].Cells[1] as DataGridViewComboBoxCell).DisplayMember = "Name";
                        DataGridViewBillDetails.Rows[i].Cells[4].Value = "X";
                        DataGridViewBillDetails.Rows[i].Cells[0].Value = i + 1;
                        DataGridViewBillDetails.Rows[i].Cells[1].Value = BillDetail.AccountId;
                        DataGridViewBillDetails.Rows[i].Cells[2].Value = BillDetail.Description;
                        DataGridViewBillDetails.Rows[i].Cells[3].Value = BillDetail.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        i++;
                    }
                    Row_Added();
                    ReSequence();
                }
                this.formIsDirty = false;
            }
            else
            {
                MessageBox.Show("Somthing went wrong, the selected bill is not valid.");
                return;
            }
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            this.formIsDirty = false;
            BtnBillCancel.PerformClick();
            RecentBills();
            return;
        }
        private void TextBoxBillSupplier_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxBillTerm.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnBillSave.Select();
            }
        }
        private void BtnBillSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxBillSupplier.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (DataGridViewBillDetails.Rows.Count > 0)
                {
                    DataGridViewBillDetails.Select();
                    DataGridViewBillDetails.CurrentCell = DataGridViewBillDetails[4, DataGridViewBillDetails.Rows.Count - 1];
                }
            }
        }
        private void BtnBillExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void FormBill_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxBillSupplier.Select();
                    e.Cancel = true;
                }
            }
        }
        public FormPayment FormPayment = null;
        private void BtnPayablePayment_Click(object sender, EventArgs e)
        {
            if (FormPayment == null || FormPayment.IsDisposed)
            {
                FormPayment = new FormPayment();
            }
            FormPayment.SupplierId = long.Parse(TextBoxBillSupplier.Id);
            FormPayment.ShowDialog(this);
        }
        private void DataGridViewBillDetails_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                Account Acc = AccountManager.Instance.GetAccountByName(DataGridViewBillDetails.CurrentCell.EditedFormattedValue.ToString(), Global.Company.CompanyId);
                if (Acc != null)
                {
                    DataGridViewBillDetails.CurrentCell.Value = Acc.Id;
                }
            }
        }
        protected override void AccountIdTransportReload(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(AccountIdTransport.Text))
            {
                SupplierId = long.Parse(AccountIdTransport.Text);
            }
        }
        private void BtnBillSearchSupplier_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            long? TempSupplierId = TextBoxBillSupplier.Id == null ? 0L : long.Parse(TextBoxBillSupplier.Id);
            SupplierId = 0L;
            FormAccountSearch FormSearchAccount = new FormAccountSearch(this);
            FormSearchAccount.IncludeCustomers = true;
            FormSearchAccount.IncludeSuppliers = true;
            FormSearchAccount.IncludeEmployees = true;
            FormSearchAccount.IncludeGeneralAccounts = true;
            FormSearchAccount.ShowDialog();
            if (SupplierId != 0)
            {
                Account Supplier = AccountManager.Instance.GetAccountById(SupplierId);
                if (Supplier != null)
                {
                    TextBoxBillSupplier.Text = Supplier.Name;
                    TextBoxBillSupplier.Id = SupplierId.ToString();
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
                }
            }
            TextBoxBillSupplier.Select();
            Cursor.Current = Cursors.Default;
        }
        private void BtnBillNewSupplier_ItemClickedEvent(object sender, ToolStripItemClickedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            long? TempSupplierId = TextBoxBillSupplier.Id == null ? 0L : long.Parse(TextBoxBillSupplier.Id);
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
                Account Supplier = AccountManager.Instance.GetAccountById(SupplierId);
                if (Supplier != null)
                {
                    TextBoxBillSupplier.Text = Supplier.Name;
                    TextBoxBillSupplier.Id = SupplierId.ToString();
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
            TextBoxBillSupplier.Select();
            Cursor.Current = Cursors.Default;
        }
        private void ResetToAccounts()
        {
            List<AccountHelperData> Account = AccountHelper.GetHelpData(1, false, false, true, false, null, Global.Company);
            if (DataGridViewBillDetails.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in DataGridViewBillDetails.Rows)
                {
                    (row.Cells[1] as DataGridViewComboBoxCell).DataSource = Account;
                }
            }
        }
        private void DataGridViewBillDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
