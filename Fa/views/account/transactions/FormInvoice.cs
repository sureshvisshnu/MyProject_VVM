using fa.libraries.utils;
using fa.libraries.Validation;
using fa.api.Accounting;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.views.account.masters;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using fa.views.utils.Invoices;
using fa.views.common;
using fa.views.utils;
using fa.views.controls.grid;
using fa.views.employee;
using fa.model.OrderManagement;
using fa.views.controls.accounting;
using fa.views.sales;
using Dropdown_Button;
using VisioForge.Libs.MediaFoundation.OPM;
namespace fa.views.account.transactions
{
    public partial class FormInvoice : FormBase
    {
        public long CustomerId = 0L;
        public static string SaveSuccessText = "Saved success...";
        public static string DeleteConfirmText = "Do you want to delete the Invoice {0}?";
        public static string DeleteErrorText = "Error Deleting the Invoice!, Please retry";
        public static string NotAllowDeleteErrorText = "Do not Delete Invoice it used in Receipt";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string Grid_ConfirmRowDeleteText = "Do you want to delete row {0}?";
        public static string Grid_EmptyErrorMsg = "Please enter Invoice details.";
        public static string Grid_MantatoryFiledErrorMsg = "Please enter {0}.";
        public static string SearchBoxEmptyErrorMsg = "Please enter search text, it could Customer Name or Invoice Date or Invoice Number.";
        public static string SearchOutput = "No Entry Found!";
        public static string ChooseTermErrorMsg = "Please choose Term";
        public static string ChooseCustomerErrorMsg = "Please choose Customer";
        public static string EnterInvoiceDateErrorMsg = "Please enter proper Invoice Date";
        public static string RefNoErrorMsg = "Please contact administrator to generate reference number.";
        InvoiceManager InvoiceManager = null;
        CustomerManager CustomerManager = null;
        SupplierManager SupplierManager = null;
        PaymentTermManager PaymentTermManager = null;
        DateValidation DateValidation = null;
        KeypressValidation KeypressValidation = null;
        AccountManager AccountManager = null;
        AddressManager AddressManager = null;
        InvoiceSavePrintA4 InvoiceSavePrintA4 = null;
        public FormInvoice()
        {
            InitializeComponent();
            AccountManager = AccountManager.Instance;
            InvoiceManager = InvoiceManager.Instance;
            CustomerManager = CustomerManager.Instance;
            SupplierManager = SupplierManager.Instance;
            PaymentTermManager = PaymentTermManager.Instance;
            AddressManager = AddressManager.Instance;
            DateValidation = DateValidation.Instance;
            KeypressValidation = KeypressValidation.Instance;
            InvoiceSavePrintA4 = new InvoiceSavePrintA4();
            excludedObjects = new string[] { "toolStrip1", "GridViewInvoiceSearch", "DiscountAdditinalChargeGrid" };
        }
        private void FormInvoice_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ComboUtils.InitializePaymentTermCombo(ComboBoxInvoiceTerms, Global.Company.CompanyId);
                ResetForm();
                EnableForm(true);
                DataGridViewInvoiceDetails.Columns["Rate"].DefaultCellStyle.Format = Global.Company.PrimaryCurrency.RoundingPrecision.ToString();
                DataGridViewInvoiceDetails.Columns["Quantity"].DefaultCellStyle.Format = Global.Company.QuantityPricision.ToString();
                DataGridViewInvoiceDetails.Columns["Amount"].DefaultCellStyle.Format = Global.Company.PrimaryCurrency.RoundingPrecision.ToString();
                RecentInvoices();
                TextBoxInvoiceCustomer.Select();
                this.formIsDirty = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private Invoice GetInvoiceFromForm()
        {
            Invoice lInvoice = new Invoice();
            lInvoice.InvoiceId = TextBoxInvoiceId.Text == string.Empty ? 0L : Convert.ToInt64(TextBoxInvoiceId.Text);
            lInvoice.ReferenceNumber = InvoiceRefNo.Text;
            Account lAccount = AccountManager.GetAccountById(long.Parse(TextBoxInvoiceCustomer.Id));
            if (lAccount != null)
            {
                lInvoice.CustomerId = lAccount.Id;
            }
            PaymentTerm lPaymentTerm = (PaymentTerm)ComboBoxInvoiceTerms.Items[ComboBoxInvoiceTerms.SelectedIndex];
            if (lPaymentTerm != null)
            {
                PaymentTerm PaymentTerm = PaymentTermManager.GetPaymentTermById(lPaymentTerm.Id);
                if (PaymentTerm != null)
                {
                    lInvoice.TermId = PaymentTerm.Id;
                }
            }
            lInvoice.Total = (float.Parse(DataGridViewInvoiceDetailsTotal.Rows[0].Cells[1].Value.ToString()));
            lInvoice.InvoiceDate = (DateTime)DateTimePickerInvoiceDate.Date;
            lInvoice.DueDate = (DateTime)DateUtils.ToDate(TextBoxInvoiceDueDate.Text, Global.Company.DateFormat);
            lInvoice.Memo = TextBoxInvoiveMemo.Text;
            lInvoice.InternalMemo = TextBoxInvoiveInternalMemo.Text;
            lInvoice.CompanyId = Global.Company.CompanyId;
            if (Global.CostCenter != null)
            {
                lInvoice.CostCenterId = Global.CostCenter.CostCenterId;
            }
            for (int i = 0; i < DataGridViewInvoiceDetails.Rows.Count - 1; i++)
            {
                InvoiceDetail InvoiceDetail = new InvoiceDetail();
                Account Account = AccountManager.Instance.GetAccountById((long)DataGridViewInvoiceDetails.Rows[i].Cells[1].Value);
                if (Account != null)
                {
                    InvoiceDetail.SalesAccountId = Account.Id;
                    InvoiceDetail.Description = (DataGridViewInvoiceDetails.Rows[i].Cells[2].Value != null) ? DataGridViewInvoiceDetails.Rows[i].Cells[2].Value.ToString() : "";
                    InvoiceDetail.Rate = (float.Parse(DataGridViewInvoiceDetails.Rows[i].Cells[3].Value.ToString()));
                    InvoiceDetail.Quantity = int.Parse(DataGridViewInvoiceDetails.Rows[i].Cells[4].Value.ToString());
                    InvoiceDetail.Total = (float.Parse(DataGridViewInvoiceDetails.Rows[i].Cells[5].Value.ToString()));
                    lInvoice.InvoiceDetails.Add(InvoiceDetail);
                }
            }
            if (DiscountAdditinalChargeGrid.AdditionalTransactions != null)
            {
                lInvoice.InvoiceAdditionalTransactions = new List<InvoiceAdditionalTransaction>();
                foreach (AdditionalTransaction AdditionalTransaction in DiscountAdditinalChargeGrid.AdditionalTransactions.ToList())
                {
                    InvoiceAdditionalTransaction InvoiceAdditionalTransaction = new InvoiceAdditionalTransaction();
                    InvoiceAdditionalTransaction.Action = AdditionalTransaction.Action;
                    InvoiceAdditionalTransaction.Amount = AdditionalTransaction.Amount;
                    InvoiceAdditionalTransaction.DisplayName = AdditionalTransaction.DisplayName;
                    InvoiceAdditionalTransaction.Name = AdditionalTransaction.Name;
                    InvoiceAdditionalTransaction.Sequence = AdditionalTransaction.Sequence;
                    InvoiceAdditionalTransaction.Type = AdditionalTransaction.Type;
                    InvoiceAdditionalTransaction.Value = AdditionalTransaction.Value;
                    InvoiceAdditionalTransaction.AccountId = AdditionalTransaction.AccountId;
                    lInvoice.InvoiceAdditionalTransactions.Add(InvoiceAdditionalTransaction);
                }
            }
            lInvoice.Total = float.Parse(LabelInvoiceFinalAmount.Text);
            return lInvoice;
        }
        private void ComboBoxInvoiceTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxInvoiceTerms.SelectedIndex > -1)
            {
                if (DateTimePickerInvoiceDate.Date != null && DateUtils.ValidDate(((DateTime)DateTimePickerInvoiceDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
                {
                    DueDateChecking();
                }
            }
        }
        private void DateTimePickerInvoiceDate_Leave(object sender, EventArgs e)
        {
            if (DateTimePickerInvoiceDate.Date != null && DateUtils.ValidDate(((DateTime)DateTimePickerInvoiceDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DueDateChecking();
            }
            else
            {
                TextBoxInvoiceDueDate.ResetText();
            }
        }
        private void BtnInvoiceNew_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (Result == DialogResult.Yes)
                {
                    if (ValidateForm())
                    {
                        BtnInvoiceSave_Click(sender, e);
                    }
                }
                if (Result == DialogResult.Cancel)
                {
                    TextBoxInvoiceCustomer.Select();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            TextBoxInvoiceCustomer.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void BtnInvoiceDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxInvoiceId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this invoice is still valid.");
                return;
            }
            long InvoiceID = Convert.ToInt64(TextBoxInvoiceId.Text);
            Invoice Invoice = InvoiceManager.GetInvoice(InvoiceID);
            if (Invoice != null)
            {
                DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, InvoiceRefNo.Text), "Delete Confirm",
              MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    if (Invoice.Paid != 0)
                    {
                        ToolStripStatusLabelErrorInvoice.Text = NotAllowDeleteErrorText;
                        ResetTimmer();
                        return;
                    }
                    else
                    {
                        bool DeleteResult = InvoiceManager.DeleteInvoice(InvoiceID);
                        if (DeleteResult)
                        {
                            ResetForm();
                            EnableForm(true);
                            if (string.IsNullOrEmpty(TextBoxInvoiceSearch.Text))
                            {
                                RecentInvoices();
                            }
                            else
                            {
                                InvoiceSearchGo_Click(sender, e);
                            }
                            TextBoxInvoiceCustomer.Select();
                            this.formIsDirty = false;
                        }
                        else
                        {
                            ToolStripStatusLabelErrorInvoice.Text = DeleteErrorText;
                        }
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            else
            {
                DisplaySystemError("Somting went wrong, please check this invoice is still valid.");
                return;
            }
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            this.formIsDirty = false;
            BtnInvoiceCancel.PerformClick();
            RecentInvoices();
            return;
        }
        private void BtnInvoicePrint_Click(object sender, EventArgs e)
        {
            if (InvoiceManager.GetInvoice(long.Parse(TextBoxInvoiceId.Text)) != null)
            {
                PrinterSetup.TransactionPrintSetup(long.Parse(TextBoxInvoiceId.Text), TransactionTypes.INVOICE);
            }
            else
            {
                DisplaySystemError("Somting went wrong, please check this invoice is still valid.");
                return;
            }
        }
        private void BtnInvoiceCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxInvoiceCustomer.Select();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            TextBoxInvoiceCustomer.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void BtnInvoiceSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Cursor.Current = Cursors.WaitCursor;
                Invoice lInvoice = GetInvoiceFromForm();
                Invoice lInvoiceFromDB = null;
                //calculation
                lInvoice.Balance = lInvoice.Total;
                lInvoice.Paid = 0F;
                if (lInvoice.InvoiceId == 0)
                {
                    string RefNo = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.INVOICE, (DateTime)DateTimePickerInvoiceDate.Date);
                    if (!string.IsNullOrEmpty(RefNo))
                    {
                        lInvoice.ReferenceNumber = RefNo;
                        lInvoiceFromDB = new Invoice();
                        try
                        {
                            lInvoiceFromDB = InvoiceManager.AddInvoice(lInvoice);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            return;
                        }
                    }
                    else
                    {
                        ToolStripStatusLabelErrorInvoice.Text = RefNoErrorMsg;
                        return;
                    }
                }
                else
                {
                    Invoice InvoiceInfo = InvoiceManager.GetInvoice(lInvoice.InvoiceId);
                    if (InvoiceInfo != null)
                    {
                        if (InvoiceInfo.Total > lInvoice.Total)
                        {
                            lInvoice.Balance = InvoiceInfo.Balance - (InvoiceInfo.Total - lInvoice.Total);
                        }
                        else
                        {
                            lInvoice.Balance = InvoiceInfo.Balance + (lInvoice.Total - InvoiceInfo.Total);
                        }
                        lInvoice.Paid = InvoiceInfo.Paid;
                        lInvoiceFromDB = new Invoice();
                        lInvoiceFromDB = InvoiceManager.UpdateInvoice(lInvoice);
                    }
                    else
                    {
                        DisplaySystemError("Somting went wrong, please check this invoice is still valid.");
                        return;
                    }
                }
                TextBoxInvoiceId.Text = lInvoiceFromDB.InvoiceId.ToString();
                InvoiceRefNo.Text = lInvoiceFromDB.ReferenceNumber;
                LastInvoiceRefNo.Text = CompanyManager.Instance.GetAccountPrevRef(Global.Company, EntryType.INVOICE, (DateTime)DateTimePickerInvoiceDate.Date!);
                if (string.IsNullOrEmpty(TextBoxInvoiceSearch.Text))
                {
                    RecentInvoices();
                }
                else
                {
                    InvoiceSearchGo_Click(sender, e);
                }
                if (lInvoiceFromDB != null)
                {
                    EnableForm(false);
                    BtnInvoicePrint.Select();
                    ToolStripStatusLabelErrorInvoice.Text = SaveSuccessText;
                }
                this.formIsDirty = false;
                Cursor.Current = Cursors.Default;
            }
        }
        private Boolean ValidateForm()
        {
            if (string.IsNullOrEmpty(TextBoxInvoiceCustomer.Text.Trim()))
            {
                TextBoxInvoiceCustomer.Select();
                ToolStripStatusLabelErrorInvoice.Text = ChooseCustomerErrorMsg;
                ResetTimmer();
                return false;
            }
            //if (!string.IsNullOrEmpty(TextBoxInvoiceCustomer.Text.Trim()) && CustomerManager.GetCustomerById(long.Parse(TextBoxInvoiceCustomer.Id)) == null)
            //{
            //    if (SupplierManager.GetSupplierById(long.Parse(TextBoxInvoiceCustomer.Id)) == null)
            //    {
            //        TextBoxInvoiceCustomer.Select();
            //        ToolStripStatusLabelErrorInvoice.Text = "Somting went wrong, please check this customer is still valid.";
            //        ResetTimmer();
            //        return false;
            //    }
            //}
            if (!DiscountAdditinalChargeGrid.IsDiscountAdditionalChargeValidationResult())
            {
                ToolStripStatusLabelErrorInvoice.Text = DiscountAdditinalChargeGrid.ErrorMsg();
                ResetTimmer();
                return false;
            }
            if (ComboBoxInvoiceTerms.SelectedIndex < 0)
            {
                ComboBoxInvoiceTerms.Select();
                ToolStripStatusLabelErrorInvoice.Text = ChooseTermErrorMsg;
                ResetTimmer();
                return false;
            }
            if (DateTimePickerInvoiceDate.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerInvoiceDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DateTimePickerInvoiceDate.Focus();
                ToolStripStatusLabelErrorInvoice.Text = EnterInvoiceDateErrorMsg;
                ResetTimmer();
                return false;
            }
            //if (DateTimePickerInvoiceDate.Date != null && !DateUtils.ValidDate_TillFinancialPeriodsStartEndDate(((DateTime)DateTimePickerInvoiceDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            //{
            //    DateTimePickerInvoiceDate.Focus();
            //    ToolStripStatusLabelErrorInvoice.Text = EnterInvoiceDateErrorMsg;
            //    ResetTimmer();
            //    return false;      
            //}
            if (!TextBoxInvoiceDueDate.MaskFull && !TextBoxInvoiceDueDate.MaskCompleted)
            {
                DueDateChecking();
            }
            int Count = DataGridViewInvoiceDetails.Rows.Count;
            if (Count > 1)
            {
                for (int i = 0; i < Count - 1; i++)
                {
                    for (int j = 1; j < 6; j++)
                    {
                        if (DataGridViewInvoiceDetails.Rows[i].Cells[j].Value == null || DataGridViewInvoiceDetails.Rows[i].Cells[j].Value.Equals(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) || DataGridViewInvoiceDetails.Rows[i].Cells[j].Value.Equals("0"))
                        {
                            DataGridViewInvoiceDetails.Select();
                            DataGridViewInvoiceDetails.CurrentCell = DataGridViewInvoiceDetails[j, i];
                            DataGridViewInvoiceDetails.BeginEdit(true);
                            ToolStripStatusLabelErrorInvoice.Text = string.Format(Grid_MantatoryFiledErrorMsg, DataGridViewInvoiceDetails.Columns[j].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                    }
                }
            }
            else
            {
                DataGridViewInvoiceDetails.Select();
                DataGridViewInvoiceDetails.CurrentCell = DataGridViewInvoiceDetails[1, 0];
                DataGridViewInvoiceDetails.BeginEdit(true);
                ToolStripStatusLabelErrorInvoice.Text = Grid_EmptyErrorMsg;
                ResetTimmer();
                return false;
            }
            return true;
        }
        private void ResetForm()
        {
            InvoiceRefNo.Text = "000000";
            TextBoxInvoiceSearch.TextBox.ResetText();
            ToolStripStatusLabelErrorInvoice.Text = "";
            TextBoxInvoiceId.ResetText();
            TextBoxInvoiveMemo.ResetText();
            TextBoxInvoiveInternalMemo.ResetText();
            DateTimePickerInvoiceDate.Format = Global.Company.DateFormat;
            DateTimePickerInvoiceDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            TextBoxInvoiceDueDate.ResetText();
            this.TextBoxInvoiceDueDate.Mask = DateValidation.ChangeMaskFormat((DateTime)DateTimePickerInvoiceDate.Date);
            this.TextBoxInvoiceDueDate.Text = ((DateTime)DateTimePickerInvoiceDate.Date).ToString(Global.Company.DateFormat);
            ComboBoxInvoiceTerms.ResetText();
            ComboBoxInvoiceTerms.SelectedIndex = -1;
            TextBoxInvoiceCustomer.ResetText();
            DiscountAdditinalChargeGrid.GridType = GridType.Sales;
            DiscountAdditinalChargeGrid.Clear();
            LabelInvoiceFinalAmount.Text = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            DataGridViewInvoiceDetails.Rows.Clear();
            DataGridViewInvoiceDetailsTotal.Rows[0].Cells[0].Value = "Total : ";
            DataGridViewInvoiceDetailsTotal.Rows[0].Cells[1].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)DataGridViewInvoiceDetails.Columns["Amount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            DataGridViewCurrencyColumn currencyColumn1 = (DataGridViewCurrencyColumn)DataGridViewInvoiceDetails.Columns["Rate"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces1))
            {
                currencyColumn1.DefaultCellStyle.Format = $"N{decimalPlaces1}";
            }
            LastInvoiceRefNo.Text = CompanyManager.Instance.GetAccountPrevRef(Global.Company, EntryType.INVOICE, (DateTime)DateTimePickerInvoiceDate.Date!);
        }
        private void EnableForm(Boolean enable)
        {
            DataGridViewInvoiceDetails.ScrollBars = ScrollBars.Vertical;
            DiscountAdditinalChargeGrid.Enabled = true;
            if (enable)
            {
                BtnInvoiceNew.Enabled = !enable;
                BtnInvoiceDelete.Enabled = !enable;
                BtnInvoicePrint.Enabled = !enable;
                BtnInvoiceCancel.Enabled = enable;
                BtnInvoiceSave.Enabled = enable;
                BtnReceivePayment.Enabled = !enable;
            }
            else
            {
                BtnInvoiceNew.Enabled = !enable;
                BtnInvoiceDelete.Enabled = !enable;
                BtnInvoicePrint.Enabled = !enable;
                BtnInvoiceCancel.Enabled = !enable;
                BtnInvoiceSave.Enabled = !enable;
                BtnReceivePayment.Enabled = !enable;
            }
        }
        private void ReSequence()
        {
            for (int i = 0; i < DataGridViewInvoiceDetails.Rows.Count; i++)
            {
                DataGridViewInvoiceDetails.Rows[i].Cells[0].Value = i + 1;
            }
        }
        private void ComputeFormTotal()
        {
            double TotalAmount = 0.00;
            for (int i = 0; i < DataGridViewInvoiceDetails.Rows.Count - 1; i++)
            {
                double Rate = (DataGridViewInvoiceDetails.Rows[i].Cells[3].Value) == null ? 0.00 : (float.Parse(DataGridViewInvoiceDetails.Rows[i].Cells[3].Value.ToString()));
                double Quantity = (DataGridViewInvoiceDetails.Rows[i].Cells[4].Value) == null ? 0.00 : (float.Parse(DataGridViewInvoiceDetails.Rows[i].Cells[4].Value.ToString()));
                DataGridViewInvoiceDetails.Rows[i].Cells[5].Value = (Rate * Quantity).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                double Amount = (DataGridViewInvoiceDetails.Rows[i].Cells[5].Value) == null ? 0.00 : (float.Parse(DataGridViewInvoiceDetails.Rows[i].Cells[5].Value.ToString()));
                TotalAmount = TotalAmount + Amount;
            }
            DiscountAdditinalChargeGrid.InputAmount = TotalAmount;
            DataGridViewInvoiceDetailsTotal.Rows[0].Cells[1].Value = TotalAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            LabelInvoiceFinalAmount.Text = DiscountAdditinalChargeGrid.OutputAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
        }
        private void DataGridViewInvoiceDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (DataGridViewInvoiceDetails.CurrentCell.ColumnIndex == 3 ||
                DataGridViewInvoiceDetails.CurrentCell.ColumnIndex == 4 ||
                DataGridViewInvoiceDetails.CurrentCell.ColumnIndex == 5)
            {
                Row_Added();
            }
        }
        private void Row_Added()
        {
            ComputeFormTotal();
        }
        private void DataGridViewInvoiceDetails_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                SendKeys.Send("{tab}");
            }
            DataGridViewInvoiceDetails.Rows[e.RowIndex].Cells[0].ReadOnly = true;
            DataGridViewInvoiceDetails.Rows[e.RowIndex].Cells[2].ReadOnly = true;
            DataGridViewInvoiceDetails.Rows[e.RowIndex].Cells[3].ReadOnly = true;
            DataGridViewInvoiceDetails.Rows[e.RowIndex].Cells[4].ReadOnly = true;
            DataGridViewInvoiceDetails.Rows[e.RowIndex].Cells[5].ReadOnly = true;
            DataGridViewInvoiceDetails.Rows[e.RowIndex].Cells[6].ReadOnly = true;
            if (DataGridViewInvoiceDetails.Rows[e.RowIndex].Cells[1].Value != null)
            {
                DataGridViewInvoiceDetails.Rows[e.RowIndex].Cells[2].ReadOnly = false;
                DataGridViewInvoiceDetails.Rows[e.RowIndex].Cells[3].ReadOnly = false;
                DataGridViewInvoiceDetails.Rows[e.RowIndex].Cells[4].ReadOnly = false;
                DataGridViewInvoiceDetails.Rows[e.RowIndex].Cells[6].ReadOnly = false;
            }
        }
        private void DataGridViewInvoiceDetails_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        DataGridViewEditingControlShowingEventArgs BackupContextMenuStrip = null;
        private void DataGridViewInvoiceDetails_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (BackupContextMenuStrip == null)
            {
                BackupContextMenuStrip = e;
            }
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).FormattingEnabled = true;

                if (DataGridViewInvoiceDetails.CurrentCell.Value == null)
                { ((ComboBox)e.Control).SelectedIndex = -1; }
                e.Control.KeyPress += new KeyPressEventHandler(DataGridViewInvoiceDetails_KeyPress1);
            }
            else
            {
                e.Control.ContextMenuStrip = BackupContextMenuStrip.Control.ContextMenuStrip;
            }
        }

        private void DataGridViewInvoiceDetails_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            IList<Account> Account = AccountManager.ListInvoiceService(Global.Company.CompanyId, Global.IncludedAccountInvoiceService);
            if (Account != null)
            {
                DataGridViewInvoiceDetails.Rows[e.RowIndex].Cells[0].Value = DataGridViewInvoiceDetails.Rows.Count;
                DataGridViewInvoiceDetails.Rows[e.RowIndex].Cells[3].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                DataGridViewInvoiceDetails.Rows[e.RowIndex].Cells[4].Value = "0";
                DataGridViewInvoiceDetails.Rows[e.RowIndex].Cells[5].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                DataGridViewInvoiceDetails.Rows[e.RowIndex].Cells[6].Value = "X";
                (DataGridViewInvoiceDetails.Rows[e.RowIndex].Cells[1] as DataGridViewComboBoxCell).DataSource = null;
                (DataGridViewInvoiceDetails.Rows[e.RowIndex].Cells[1] as DataGridViewComboBoxCell).DataSource = Account;
                (DataGridViewInvoiceDetails.Rows[e.RowIndex].Cells[1] as DataGridViewComboBoxCell).ValueMember = "Id";
                (DataGridViewInvoiceDetails.Rows[e.RowIndex].Cells[1] as DataGridViewComboBoxCell).DisplayMember = "Name";
            }
        }
        private void DataGridViewInvoiceDetails_KeyPress1(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)DataGridViewInvoiceDetails.EditingControl).DroppedDown = false;
        }
        private void ComboBoxInvoiceTerms_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxInvoiceTerms.DroppedDown = false;
        }
        private void DueDateChecking()
        {
            if (ComboBoxInvoiceTerms.SelectedIndex == 0)
            {
                TextBoxInvoiceDueDate.Text = ((DateTime)DateTimePickerInvoiceDate.Date).Date.ToString(Global.Company.DateFormat);
            }
            if (ComboBoxInvoiceTerms.SelectedIndex == 1)
            {
                TextBoxInvoiceDueDate.Text = ((DateTime)DateTimePickerInvoiceDate.Date).AddDays(15).Date.ToString(Global.Company.DateFormat);
            }
            if (ComboBoxInvoiceTerms.SelectedIndex == 2)
            {
                TextBoxInvoiceDueDate.Text = ((DateTime)DateTimePickerInvoiceDate.Date).AddDays(30).Date.ToString(Global.Company.DateFormat);
            }
        }
        private void TextBoxInvoiceCustomer_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxInvoiceTerms.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnInvoiceSave.Select();
            }
        }
        private void TextBoxInvoiveInternalMemo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DataGridViewInvoiceDetails.Select();
                DataGridViewInvoiceDetails.CurrentCell = DataGridViewInvoiceDetails[0, 0];
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxInvoiveMemo.Focus();
            }
        }
        private void BtnInvoiceSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxInvoiceCustomer.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (DataGridViewInvoiceDetails.Rows.Count > 0)
                {
                    DiscountAdditinalChargeGrid.Focus();
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F2))
            {
                BtnInvoiceSearchCustomer.PerformClick();
                return true;
            }
            if (keyData == Keys.Tab && InvoiceSearchGo.Selected)
            {
                TextBoxInvoiceCustomer.Focus();
                return true;
            }
            if (keyData == (Keys.F3))
            {
                if (BtnInvoiceNew.Enabled)
                {
                    BtnInvoiceNew.PerformClick();
                }
                else
                {
                    if (TextBoxInvoiceCustomer.Focused)
                    {
                        BtnInvoiceNewCustomers.ShowDropDown();
                    }
                }
            }
            else if (keyData == (Keys.F4))
            {
                if (BtnInvoiceDelete.Enabled)
                {
                    BtnInvoiceDelete.PerformClick();
                }
            }
            else if (keyData == (Keys.F9))
            {
                BtnInvoicePrint.PerformClick();
            }
            else if (keyData == (Keys.F8))
            {
                BtnInvoiceSave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnInvoiceCancel.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F10))
            {
                BtnInvoiceExit.PerformClick();
                return true;
            }
            if (keyData == Keys.Left && ActiveControl == BtnInvoiceSave)
            {
                BtnInvoiceCancel.Select();
                return true;
            }
            if (keyData == Keys.Right && ActiveControl == BtnInvoiceSave)
            {
                BtnInvoiceExit.Select();
                return true;
            }
            if (keyData == Keys.Right && ActiveControl == BtnInvoiceCancel)
            {
                BtnInvoiceSave.Select();
                return true;
            }
            if (keyData == Keys.Left && ActiveControl == BtnInvoiceCancel)
            {
                BtnInvoiceExit.Select();
                return true;
            }
            if (keyData == Keys.Left && ActiveControl == BtnInvoiceExit)
            {
                BtnInvoiceSave.Select();
                return true;
            }
            if (keyData == Keys.Right && ActiveControl == BtnInvoiceExit)
            {
                BtnInvoiceCancel.Select();
                return true;
            }
            try
            {
                if (DataGridViewInvoiceDetails.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && DataGridViewInvoiceDetails.CurrentCell.RowIndex == DataGridViewInvoiceDetails.Rows.Count - 1 && DataGridViewInvoiceDetails.CurrentCell.ColumnIndex == 4)
                    {
                        ActiveControl = DiscountAdditinalChargeGrid;
                        DiscountAdditinalChargeGrid.Focus();
                    }
                    else if (keyData == (Keys.Tab) && DataGridViewInvoiceDetails.CurrentCell.ColumnIndex == 4)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && DataGridViewInvoiceDetails.CurrentCell.ColumnIndex == 1 && DataGridViewInvoiceDetails.CurrentRow.Index != 0)
                    {
                        SendKeys.Send("{tab}{tab}");
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
            TimerInvoice.Stop();
            TimerInvoice.Start();
        }
        private void TimerInvoice_Tick(object sender, EventArgs e)
        {
            this.ToolStripStatusLabelErrorInvoice.Visible = !this.ToolStripStatusLabelErrorInvoice.Visible;
            blinkCount++;
            if (blinkCount == 3 * 2)
            {
                TimerInvoice.Stop();
                ToolStripStatusLabelErrorInvoice.Visible = true;
            }
        }
        private void InvoiceSearchGo_Click(object sender, EventArgs e)
        {
            if (isValidSearchCriteria())
            {
                GridViewInvoiceSearch.Rows.Clear();
                IList<Invoice> InvoiceInfo = SearchInvoice();
                LoadInvoice(InvoiceInfo);
            }
        }
        private bool isValidSearchCriteria()
        {
            ToolStripStatusLabelErrorInvoice.Text = "";
            if (string.IsNullOrEmpty(TextBoxInvoiceSearch.Text))
            {
                ToolStripStatusLabelErrorInvoice.Text = SearchBoxEmptyErrorMsg;
                TextBoxInvoiceSearch.TextBox.Select();
                return false;
            }
            return true;
        }
        private IList<Invoice> SearchInvoice()
        {
            string text = TextBoxInvoiceSearch.Text;
            IList<Invoice> InvoiceInfo = null;
            if (TextUtils.isReferenceNumber(text))
            {
                InvoiceInfo = InvoiceManager.GetInvoiceByReferenceNo(text, Global.Company.CompanyId);
            }
            else if (DateUtils.ValidDate(text, Global.Company.DateFormat))
            {
                DateTime? Date = (DateTime)DateUtils.ToDate(text, Global.Company.DateFormat);
                InvoiceInfo = InvoiceManager.GetInvoiceByDate(Date, Global.Company.CompanyId);
            }
            else
            {
                InvoiceInfo = InvoiceManager.GetInvoiceByCustomerName(text, Global.Company.CompanyId);
            }
            return InvoiceInfo;
        }
        private void RecentInvoices()
        {
            GridViewInvoiceSearch.Rows.Clear();
            IList<Invoice> InvoiceInfo = InvoiceManager.GetRecentInvoices(Global.Company.CompanyId);
            LoadInvoice(InvoiceInfo);
        }
        public void LoadInvoice(IList<Invoice> InvoiceInfo)
        {
            ToolStripStatusLabelErrorInvoice.Text = "";
            if (InvoiceInfo.Count > 0)
            {
                GridViewInvoiceSearch.Rows.Add(InvoiceInfo.Count);
                int i = 0;
                foreach (var lInvoiceInfo in InvoiceInfo)
                {
                    GridViewInvoiceSearch.Rows[i].Cells[0].Value = lInvoiceInfo.ReferenceNumber;
                    GridViewInvoiceSearch.Rows[i].Cells[1].Value = lInvoiceInfo.InvoiceDate.ToString(Global.Company.DateFormat);
                    GridViewInvoiceSearch.Rows[i].Cells[2].Value = lInvoiceInfo.Customer.Name;
                    GridViewInvoiceSearch.Rows[i].Cells[3].Value = lInvoiceInfo.Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewInvoiceSearch.Rows[i].Cells[4].Value = lInvoiceInfo.InvoiceId;
                    i++;
                }
            }
            else
            {
                ToolStripStatusLabelErrorInvoice.Text = SearchOutput;
            }
        }
        private void TextBoxInvoiceSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                InvoiceSearchGo_Click(sender, e);
            }
        }
        private void TextBoxInvoiceSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxInvoiceSearch.Text))
            {
                RecentInvoices();
            }
        }
        public FormReceipts FormReceipts = null;
        private void BtnReceivePayment_Click(object sender, EventArgs e)
        {
            if (FormReceipts == null || FormReceipts.IsDisposed)
            {
                FormReceipts = new FormReceipts();
            }
            FormReceipts.CustomerId = long.Parse(TextBoxInvoiceCustomer.Id);
            FormReceipts.ShowDialog(this);
        }
        private void DisplayInvoiceDetails(long InvoiceNumber, object sender, DataGridViewCellEventArgs e)
        {
            if (this.formIsDirty)
            {
                DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
               MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (Result == DialogResult.Yes)
                {
                    if (ValidateForm())
                    {
                        BtnInvoiceSave_Click(sender, e);
                        LoadInvoice(InvoiceNumber);
                    }
                }
                else if (Result == DialogResult.No)
                {
                    LoadInvoice(InvoiceNumber);
                }
            }
            else
            {
                LoadInvoice(InvoiceNumber);
            }
        }
        private void GridViewInvoiceSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (GridViewInvoiceSearch.Rows.Count != 0 && GridViewInvoiceSearch.Rows != null)
                {
                    DisplayInvoiceDetails((long)GridViewInvoiceSearch.Rows[e.RowIndex].Cells[4].Value, sender, e);
                }
            }
        }
        private void LoadInvoice(long InvoiceId)
        {
            ResetForm();
            EnableForm(false);
            Invoice Invoice = InvoiceManager.GetInvoice(InvoiceId);
            if (Invoice != null)
            {
                InvoiceRefNo.Text = Invoice.ReferenceNumber;
                TextBoxInvoiceId.Text = Invoice.InvoiceId.ToString();
                TextBoxInvoiceCustomer.Text = Invoice.Customer.Name;
                TextBoxInvoiceCustomer.Id = Invoice.Customer.Id.ToString();
                ComboBoxInvoiceTerms.SelectedIndex = ComboBoxInvoiceTerms.FindStringExact(Invoice.Term.Name);
                DateTimePickerInvoiceDate.Date = (DateTime)DateUtils.ToDate(Invoice.InvoiceDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                TextBoxInvoiceDueDate.Text = Invoice.DueDate.ToString(Global.Company.DateFormat);
                TextBoxInvoiveMemo.Text = Invoice.Memo;
                TextBoxInvoiveInternalMemo.Text = Invoice.InternalMemo;
                if (Invoice.InvoiceDetails.Count > 0)
                {
                    DataGridViewInvoiceDetails.Rows.Add(Invoice.InvoiceDetails.Count);
                    int i = 0;
                    IList<Account> Account = AccountManager.ListInvoiceService(Global.Company.CompanyId, Global.IncludedAccountInvoiceService);
                    foreach (var InvoiceDetail in Invoice.InvoiceDetails)
                    {
                        (DataGridViewInvoiceDetails.Rows[i].Cells[1] as DataGridViewComboBoxCell).DataSource = null;
                        (DataGridViewInvoiceDetails.Rows[i].Cells[1] as DataGridViewComboBoxCell).DataSource = Account;
                        (DataGridViewInvoiceDetails.Rows[i].Cells[1] as DataGridViewComboBoxCell).ValueMember = "Id";
                        (DataGridViewInvoiceDetails.Rows[i].Cells[1] as DataGridViewComboBoxCell).DisplayMember = "Name";
                        DataGridViewInvoiceDetails.Rows[i].Cells[6].Value = "X";
                        DataGridViewInvoiceDetails.Rows[i].Cells[0].Value = i + 1;
                        DataGridViewInvoiceDetails.Rows[i].Cells[1].Value = InvoiceDetail.SalesAccountId;
                        DataGridViewInvoiceDetails.Rows[i].Cells[2].Value = InvoiceDetail.Description;
                        DataGridViewInvoiceDetails.Rows[i].Cells[3].Value = InvoiceDetail.Rate.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        DataGridViewInvoiceDetails.Rows[i].Cells[4].Value = InvoiceDetail.Quantity;
                        DataGridViewInvoiceDetails.Rows[i].Cells[5].Value = InvoiceDetail.Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        i++;
                    }
                    if (Invoice.InvoiceAdditionalTransactions.Count > 0)
                    {
                        DiscountAdditinalChargeGrid.AdditionalTransactions = Invoice.InvoiceAdditionalTransactions.ToList<AdditionalTransaction>();
                    }
                    Row_Added();
                    ReSequence();
                }
                this.formIsDirty = false;
            }
            else
            {
                MessageBox.Show("Somthing went wrong, the selected invoice is not valid.");
                return;
            }
        }
        private void TextBoxInvoiveInternalMemo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                e.Handled = true;
            }
        }
        private void FormInvoice_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxInvoiceCustomer.Select();
                    e.Cancel = true;
                }
            }
        }
        private void BtnInvoiceExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void DataGridViewInvoiceDetails_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                Account Acc = AccountManager.GetAccountByName(DataGridViewInvoiceDetails.CurrentCell.EditedFormattedValue.ToString(), Global.Company.CompanyId);
                if (Acc != null)
                {
                    DataGridViewInvoiceDetails.CurrentCell.Value = Acc.Id;
                }
            }
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
            long? TempSupplierId = TextBoxInvoiceCustomer.Id == null ? 0L : long.Parse(TextBoxInvoiceCustomer.Id);
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
                Customer customer = CustomerManager.Instance.GetCustomerByName(Account.Name, Global.Company.CompanyId);
                if (Account != null)
                {
                    TextBoxInvoiceCustomer.Text = Account.Name;
                    if (customer != null)
                    {
                        int index = ComboBoxInvoiceTerms.FindStringExact(customer.PaymentTerm.Name);
                        ComboBoxInvoiceTerms.SelectedIndex = index > -1 ? index : 0;
                    }
                    TextBoxInvoiceCustomer.Id = CustomerId.ToString();
                }
                else
                {
                    CustomerId = (long)TempSupplierId;
                    MessageBox.Show("Somting went wrong, please check this account is still valid.");
                    return;
                }
            }
            else
            {
                if (TempSupplierId != null)
                {
                    CustomerId = (long)TempSupplierId;
                }
            }
            TextBoxInvoiceCustomer.Select();
            Cursor.Current = Cursors.Default;
        }
        private void GridViewInvoiceSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (GridViewInvoiceSearch.Rows.Count != 0 && GridViewInvoiceSearch.Rows != null)
                {
                    SendKeys.Send("{home}");
                    var eventArgs = new DataGridViewCellEventArgs(GridViewInvoiceSearch.CurrentCell.ColumnIndex, GridViewInvoiceSearch.CurrentCell.RowIndex);
                    DisplayInvoiceDetails((long)GridViewInvoiceSearch.Rows[GridViewInvoiceSearch.CurrentCell.RowIndex].Cells[4].Value, sender, eventArgs);
                    e.Handled = true;
                }
            }
        }
        private void GridViewInvoiceSearch_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Focus);
            e.Handled = true;
        }
        private void BtnInvoiceNewCustomers_ItemClickedEvent(object sender, ToolStripItemClickedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            long? TempCustomerId = TextBoxInvoiceCustomer.Id == null ? 0L : long.Parse(TextBoxInvoiceCustomer.Id);
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
                Account Account = AccountManager.Instance.GetAccountById(CustomerId);
                if (Account != null)
                {
                    TextBoxInvoiceCustomer.Text = Account.Name;
                    TextBoxInvoiceCustomer.Id = CustomerId.ToString();
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
                    this.formIsDirty = false;
                }
            }
            TextBoxInvoiceCustomer.Select();
            Cursor.Current = Cursors.Default;
        }
        private void ResetToAccounts()
        {
            List<AccountHelperData> Account = AccountHelper.GetHelpData(1, false, false, true, false, null, Global.Company);
            if (DataGridViewInvoiceDetails.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in DataGridViewInvoiceDetails.Rows)
                {
                    (row.Cells[1] as DataGridViewComboBoxCell).DataSource = Account;
                }
            }
        }
        private void Row_Removed()
        {
            ReSequence();
            ComputeFormTotal();
        }
        private void DataGridViewInvoiceDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 6 && (DataGridViewInvoiceDetails.Rows.Count - 1) != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, DataGridViewInvoiceDetails.Rows[e.RowIndex].Cells[0].Value.ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        DataGridViewInvoiceDetails.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        DataGridViewInvoiceDetails.Rows.RemoveAt(e.RowIndex);
                        Row_Removed();
                    }
                }
            }
        }

        private void DataGridViewInvoiceDetails_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (DataGridViewInvoiceDetails.Columns[e.ColumnIndex].Name == "Quantity")
            {
                if (e.Value != null && decimal.TryParse(e.Value.ToString(), out decimal result))
                {
                    e.Value = result.ToString($"F{Global.Company.QuantityPricision}");
                    e.FormattingApplied = true;
                }
            }
        }

        private void DiscountAdditinalChargeGrid_Load(object sender, EventArgs e)
        {
            LabelInvoiceFinalAmount.Text = DiscountAdditinalChargeGrid.OutputAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
        }

        private void DiscountAdditinalChargeGrid_TabIndexChanged(object sender, EventArgs e)
        {
            if (!this.formIsDirty)
            {
                this.InputControls_OnChange(sender, e);
            }
        }

        private void DiscountAdditinalChargeGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            int RowIndex = DataGridViewInvoiceDetails.Rows.Count -1;
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DataGridViewInvoiceDetails.Select();
                DataGridViewInvoiceDetails.CurrentCell = DataGridViewInvoiceDetails[4, RowIndex];
            }
        }
    }
}
