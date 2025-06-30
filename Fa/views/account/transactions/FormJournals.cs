using System.Windows.Forms;
using System;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.api.Accounting;
using System.Collections.Generic;
using fa.model.Accounting.Transaction;
using fa.api.System;
using fa.views.utils.Journal;
using Fa.api.Accounting.DoubleEntry;
using fa.views.utils;
using fa.views.controls.grid;
using fa.views.employee;
using fa.views.account.masters;
using Fa.views.common;
using System.Security.Principal;
using MathNet.Numerics;
using Syncfusion.Styles;
using fa.views.purchase;

namespace fa.views.account.transactions
{
    public partial class FormJournal : FormBase
    {
        public static string SaveSuccessText = "Saved success...";
        public static string DeleteConfirmText = "Do you want to delete the Journal {0}?";
        public static string DeleteErrorText = "Error Deleting the Journal!, Please retry";
        public static string NotAllowDeleteErrorText = "Do not Delete Journal it used in Some ware else";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string Grid_ConfirmRowDeleteText = "Do you want to delete row {0}?";
        public static string Grid_EmptyErrorMsg = "Please enter Journal details.";
        public static string Grid_MantatoryFiledErrorMsg = "Please enter {0}.";
        public static string Grid_AmountEnterErrorMsg = "Please check the {0}.";
        public static string SearchBoxEmptyErrorMsg = "Please enter search text, it could Journal Date or Journal Number.";
        public static string WentWrongErrorMsg = "Somting went wrong, please check this journal is still valid.";
        public static string SearchOutput = "No Entry Found!";

        public static string EnterJournalDateErrorMsg = "Please enter proper Journal Date";
        public static string RefNoErrorMsg = "Please contact administrator to generate reference number.";
        public static string ToolTipMsg = "Press F2 for Account search";

        JournalManager JournalManager = null!;
        public long SearchJournalId = 0L;
        public bool SelectGenAccount = false;
        private bool _isSelectingAccount = false;
        private ToolTip _gridToolTip = new ToolTip();
        private int _lastToolTipColumn = -1;
        DataGridViewEditingControlShowingEventArgs BackupContextMenuStrip = null!;

        public FormJournal()
        {
            JournalManager = JournalManager.Instance;
            InitializeComponent();

            _gridToolTip.AutoPopDelay = 3000;
            _gridToolTip.InitialDelay = 300;
            _gridToolTip.ReshowDelay = 100;
            _gridToolTip.ShowAlways = true;

            GridViewJournal.CellMouseEnter += GridViewJournal_CellMouseEnter!;
            GridViewJournal.CellMouseLeave += GridViewJournal_CellMouseLeave!;
            GridViewJournal.CellMouseMove += GridViewJournal_CellMouseMove!;


            excludedObjects = new string[] { "toolStrip1" };
        }

        private void FormJournal_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            DatePickerJournal.Select();
            this.formIsDirty = false;
        }
        private void GridViewJournal_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            ShowToolTipForCell(e.ColumnIndex, e.RowIndex);
        }
        private void GridViewJournal_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 1 || e.ColumnIndex == 5)
            {
                if (e.ColumnIndex != _lastToolTipColumn)
                {
                    ShowToolTipForCell(e.ColumnIndex, e.RowIndex);
                }
            }
        }
        private void ShowToolTipForCell(int columnIndex, int rowIndex)
        {
            if (rowIndex >= 0 && (columnIndex == 1 || columnIndex == 5))
            {
                _lastToolTipColumn = columnIndex;

                var cellRect = GridViewJournal.GetCellDisplayRectangle(columnIndex, rowIndex, false);
                var screenPos = GridViewJournal.PointToScreen(new Point(cellRect.Right - 20, cellRect.Bottom));

                _gridToolTip.Show(ToolTipMsg, GridViewJournal,
                    GridViewJournal.PointToClient(screenPos), 3000);

                GridViewJournal.Rows[rowIndex].Cells[columnIndex].Style.BackColor = Color.LightYellow;
            }
        }

        private void GridViewJournal_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            _lastToolTipColumn = -1;
            _gridToolTip.Hide(GridViewJournal);

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                GridViewJournal.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor =
                    GridViewJournal.DefaultCellStyle.BackColor;
            }
        }

        private Journal GetJournalFromForm()
        {
            Journal lJournal = new Journal();
            lJournal.JournalId = TextBoxJournalId.Text == string.Empty ? 0L : Convert.ToInt64(TextBoxJournalId.Text);
            lJournal.ReferenceNumber = JournalRefNo.Text;
            lJournal.Amount = decimal.Parse(GridViewJournalTotal.Rows[0].Cells[2].Value.ToString()!);
            lJournal.TransactionDate = (DateTime)DatePickerJournal.Date!;
            lJournal.Memo = TextBoxJournalMemo.Text;
            lJournal.CompanyId = Global.Company.CompanyId;

            if (Global.CostCenter != null)
            {
                lJournal.CostCenterId = Global.CostCenter.CostCenterId;
            }

            for (int i = 0; i < GridViewJournal.Rows.Count - 1; i++)
            {
                if (GridViewJournal.Rows[i].Cells[1].Value == null ||
                    string.IsNullOrEmpty(GridViewJournal.Rows[i].Cells[1].Value.ToString()))
                {
                    continue;
                }

                JournalDetail JournalDetail = new JournalDetail();

                long toAccountId = 0;

                DataGridViewCell cell = GridViewJournal.Rows[i].Cells[1];
                if (cell.Tag is Account acc)
                {
                    toAccountId = acc.Id;
                    Account ToAccount = AccountManager.Instance.GetAccountById(toAccountId);
                    if (ToAccount != null)
                    {
                        JournalDetail.ToAccountId = ToAccount.Id;

                        if (GridViewJournal.Rows[i].Cells[5].Value != null &&
                            !string.IsNullOrEmpty(GridViewJournal.Rows[i].Cells[5].Value.ToString()))
                        {
                            long referenceAccountId = 0;
                            DataGridViewCell Fromcell = GridViewJournal.Rows[i].Cells[5];
                            if (Fromcell.Tag is Account Fromacc)
                            {
                                referenceAccountId = Fromacc.Id;
                                Account FromAccount = AccountManager.Instance.GetAccountById(referenceAccountId);
                                if (FromAccount != null)
                                {
                                    JournalDetail.ReferenceAccountId = FromAccount.Id;
                                }
                            }
                        }

                        JournalDetail.Description = GridViewJournal.Rows[i].Cells[4].Value?.ToString() ?? "";

                        decimal debitAmount = 0;
                        decimal creditAmount = 0;

                        if (GridViewJournal.Rows[i].Cells[2].Value != null &&
                            decimal.TryParse(GridViewJournal.Rows[i].Cells[2].Value.ToString(), out debitAmount) &&
                            debitAmount > 0)
                        {
                            JournalDetail.Amount = debitAmount;
                        }
                        else if (GridViewJournal.Rows[i].Cells[3].Value != null &&
                                 decimal.TryParse(GridViewJournal.Rows[i].Cells[3].Value.ToString(), out creditAmount))
                        {
                            JournalDetail.Amount = creditAmount * -1;
                        }

                        lJournal.JournalDetails.Add(JournalDetail);
                    }
                }
            }
            return lJournal;
        }
        private void BtnJournalNew_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (Result == DialogResult.Yes)
                {
                    if (ValidateForm())
                    {
                        BtnJournalSave_Click(sender, e);
                    }
                }
                if (Result == DialogResult.Cancel)
                {
                    DatePickerJournal.Focus();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            DatePickerJournal.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }

        private void BtnJournalDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxJournalId.Text))
            {
                MessageBox.Show(WentWrongErrorMsg);
                return;
            }
            DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, JournalRefNo.Text), "Delete Confirm",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                long JournalID = Convert.ToInt64(TextBoxJournalId.Text);
                Journal Journal = JournalManager.GetJournal(JournalID);
                if (Journal != null)
                {
                    bool DeleteResult = JournalManager.DeleteJournal(JournalID);
                    if (DeleteResult)
                    {
                        ResetForm();
                        EnableForm(true);
                        DatePickerJournal.Focus();
                        this.formIsDirty = false;
                    }
                    else
                    {
                        JournalErrorMsg.Text = NotAllowDeleteErrorText;
                    }
                }
                else
                {
                    DisplaySystemError(WentWrongErrorMsg);
                    return;
                }
                Cursor.Current = Cursors.Default;
            }
        }
        private void BtnJournalPrint_Click(object sender, EventArgs e)
        {
            if (JournalManager.GetJournal(long.Parse(TextBoxJournalId.Text)) != null)
            {
                PrinterSetup.TransactionPrintSetup(long.Parse(TextBoxJournalId.Text), TransactionTypes.JOURNAL);
            }
            else
            {
                DisplaySystemError(WentWrongErrorMsg);
                return;
            }
        }
        private void BtnJournalCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    DatePickerJournal.Focus();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            DatePickerJournal.Focus();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }

        private void BtnJournalSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Cursor.Current = Cursors.WaitCursor;
                Journal lJournal = GetJournalFromForm();
                Journal lJournalFromDB = null!;
                if (lJournal.JournalId == 0)
                {
                    string RefNo = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.JOURNAL, (DateTime)DatePickerJournal.Date!);
                    if (!string.IsNullOrEmpty(RefNo))
                    {
                        lJournal.ReferenceNumber = RefNo;
                        lJournalFromDB = new Journal();
                        try
                        {
                            lJournalFromDB = JournalManager.AddJournal(lJournal);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            return;
                        }
                    }
                    else
                    {
                        JournalErrorMsg.Text = RefNoErrorMsg;
                        return;
                    }
                }
                else
                {
                    if (JournalManager.GetJournal(lJournal.JournalId) != null)
                    {
                        lJournalFromDB = new Journal();
                        lJournalFromDB = JournalManager.UpdateJournal(lJournal);
                    }
                    else
                    {
                        DisplaySystemError(WentWrongErrorMsg);
                        return;
                    }
                }

                TextBoxJournalId.Text = lJournalFromDB.JournalId.ToString();
                JournalRefNo.Text = lJournalFromDB.ReferenceNumber;
                LastJournalRefNo.Text = CompanyManager.Instance.GetAccountPrevRef(Global.Company, EntryType.JOURNAL, (DateTime)DatePickerJournal.Date!);
                if (lJournalFromDB != null)
                {
                    EnableForm(false);
                    BtnJournalPrint.Select();
                    JournalErrorMsg.Text = SaveSuccessText;
                }
                this.formIsDirty = false;
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnJournalExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private Boolean ValidateForm()
        {

            if (DatePickerJournal.Date == null || !DateUtils.ValidDate(((DateTime)DatePickerJournal.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DatePickerJournal.Select();
                JournalErrorMsg.Text = EnterJournalDateErrorMsg;
                ResetTimmer();
                return false;
            }

            int Count = GridViewJournal.Rows.Count;
            if (Count > 1)
            {
                for (int i = 0; i < Count - 1; i++)
                {
                    for (int j = 1; j < 4; j++)
                    {
                        if (GridViewJournal.Rows[i].Cells[j].Value == null || GridViewJournal.Rows[i].Cells[j].Value.Equals(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) || GridViewJournal.Rows[i].Cells[j].Value.Equals("0"))
                        {
                            if ((j == 2 && GridViewJournal.Rows[i].Cells[j + 1].Value != null
                                && float.Parse(GridViewJournal.Rows[i].Cells[j + 1].Value.ToString()!) > 0)
                                ||
                                (j == 3 && GridViewJournal.Rows[i].Cells[j - 1].Value != null
                                && float.Parse(GridViewJournal.Rows[i].Cells[j - 1].Value.ToString()!) > 0))
                            {
                                continue;
                            }
                            GridViewJournal.Select();
                            GridViewJournal.CurrentCell = GridViewJournal[j, i];
                            GridViewJournal.BeginEdit(true);
                            JournalErrorMsg.Text = string.Format(Grid_MantatoryFiledErrorMsg, GridViewJournal.Columns[j].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                        if (j == 2 || j == 3)
                        {
                            float debit = GridViewJournal.Rows[i].Cells[2].Value != null ? float.Parse(GridViewJournal.Rows[i].Cells[2].Value.ToString()!) : 0;
                            float credit = GridViewJournal.Rows[i].Cells[3].Value != null ? float.Parse(GridViewJournal.Rows[i].Cells[3].Value.ToString()!) : 0;
                            if (debit > 0 && credit > 0)
                            {
                                GridViewJournal.Select();
                                GridViewJournal.CurrentCell = GridViewJournal[2, i];
                                GridViewJournal.BeginEdit(true);
                                JournalErrorMsg.Text = string.Format(Grid_AmountEnterErrorMsg, "amount");
                                ResetTimmer();
                                return false;
                            }
                        }
                    }
                }
            }
            else
            {
                GridViewJournal.Select();
                GridViewJournal.CurrentCell = GridViewJournal[1, 0];
                GridViewJournal.BeginEdit(true);
                JournalErrorMsg.Text = Grid_EmptyErrorMsg;
                ResetTimmer();
                return false;
            }
            float Debit = float.Parse(GridViewJournalTotal.Rows[0].Cells[2].Value.ToString()!);
            float Credit = float.Parse(GridViewJournalTotal.Rows[0].Cells[3].Value.ToString()!);
            if (Debit != Credit)
            {
                GridViewJournal.Select();
                GridViewJournal.CurrentCell = GridViewJournal[2, 0];
                GridViewJournal.BeginEdit(true);
                JournalErrorMsg.Text = string.Format(Grid_AmountEnterErrorMsg, "amount");
                ResetTimmer();
                return false;
            }
            foreach (DataGridViewRow row in GridViewJournal.Rows)
            {
                int rowIndex = row.Index;
                if (rowIndex == GridViewJournal.Rows.Count - 1)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(row.Cells[4].Value as string))
                {
                    JournalErrorMsg.Text = string.Format(Grid_MantatoryFiledErrorMsg, "Discription");
                    GridViewJournal.Rows[rowIndex].Cells[4].Selected = true;
                    GridViewJournal.CurrentCell = GridViewJournal.Rows[rowIndex].Cells[4];
                    GridViewJournal.BeginEdit(true);
                    return false;
                }
            }
            return true;
        }
        private void ResetForm()
        {
            JournalRefNo.Text = "000000";
            TextBoxJournalSearch.TextBox.ResetText();
            JournalErrorMsg.Text = "";
            TextBoxJournalId.ResetText();
            TextBoxJournalMemo.ResetText();
            DatePickerJournal.ResetText();
            DatePickerJournal.Format = Global.Company.DateFormat;
            DatePickerJournal.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
            DatePickerJournal.MinDate = Global.getCurrentFiscalYearStartDate();
            DatePickerJournal.MaxDate = Global.getCurrentFiscalYearEndDate();
            GridViewJournal.Rows.Clear();
            GridViewJournalTotal.Rows[0].Cells[1].Value = "Total : ";
            GridViewJournalTotal.Rows[0].Cells[2].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            GridViewJournalTotal.Rows[0].Cells[3].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridViewJournal.Columns["Debits"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            DataGridViewCurrencyColumn currencyColumn1 = (DataGridViewCurrencyColumn)GridViewJournal.Columns["Credits"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces1)) currencyColumn1.DecimalPlaces = decimalPlaces1;
            LastJournalRefNo.Text = CompanyManager.Instance.GetAccountPrevRef(Global.Company, EntryType.JOURNAL, (DateTime)DatePickerJournal.Date!);
        }
        private void EnableForm(Boolean enable)
        {

            GridViewJournal.ScrollBars = ScrollBars.Vertical;
            if (enable)
            {
                BtnJournalNew.Enabled = !enable;
                BtnJournalDelete.Enabled = !enable;
                BtnJournalPrint.Enabled = !enable;
                BtnJournalCancel.Enabled = enable;
                BtnJournalSave.Enabled = enable;
            }
            else
            {
                BtnJournalNew.Enabled = !enable;
                BtnJournalDelete.Enabled = !enable;
                BtnJournalPrint.Enabled = !enable;
                BtnJournalCancel.Enabled = !enable;
                BtnJournalSave.Enabled = enable;

            }
        }
        private void ReSequence()
        {
            for (int i = 0; i < GridViewJournal.Rows.Count; i++)
            {
                GridViewJournal.Rows[i].Cells[0].Value = i + 1;
            }
        }
        private void ComputeFormTotal()
        {
            double TotalDebitAmount = 0.00;
            double TotalCreditAmount = 0.00;

            for (int i = 0; i < GridViewJournal.Rows.Count - 1; i++)
            {
                double Amount = (GridViewJournal.Rows[i].Cells[2].Value) == null ? 0.00 : (float.Parse(GridViewJournal.Rows[i].Cells[2].Value.ToString()!));
                TotalDebitAmount = TotalDebitAmount + Amount;
                Amount = (GridViewJournal.Rows[i].Cells[3].Value) == null ? 0.00 : (float.Parse(GridViewJournal.Rows[i].Cells[3].Value.ToString()!));
                TotalCreditAmount = TotalCreditAmount + Amount;
            }
            GridViewJournalTotal.Rows[0].Cells[2].Value = TotalDebitAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            GridViewJournalTotal.Rows[0].Cells[3].Value = TotalCreditAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

        }
        private void Row_Removed()
        {
            ReSequence();
            ComputeFormTotal();
        }
        private void Row_Added()
        {
            ComputeFormTotal();
        }
        private void GridViewJournal_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 6 && (GridViewJournal.Rows.Count - 1) != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, GridViewJournal.Rows[e.RowIndex].Cells[0].Value.ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        GridViewJournal.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewJournal.Rows.RemoveAt(e.RowIndex);
                        Row_Removed();
                    }
                }
            }
        }

        private void GridViewJournal_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (GridViewJournal.CurrentCell.ColumnIndex == 2 ||
                GridViewJournal.CurrentCell.ColumnIndex == 3)
            {

                Row_Added();
            }
        }

        private void GridViewJournal_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;


            if (!_isSelectingAccount && (e.ColumnIndex == 1 || e.ColumnIndex == 5))
            {
                GridViewJournal.BeginEdit(true);
            }
            if (e.ColumnIndex == 0)
            {
                SendKeys.Send("{tab}");
            }
            GridViewJournal.Rows[e.RowIndex].Cells[0].ReadOnly = true;
            GridViewJournal.Rows[e.RowIndex].Cells[1].ReadOnly = true;
            GridViewJournal.Rows[e.RowIndex].Cells[2].ReadOnly = true;
            GridViewJournal.Rows[e.RowIndex].Cells[3].ReadOnly = true;
            GridViewJournal.Rows[e.RowIndex].Cells[4].ReadOnly = true;
            GridViewJournal.Rows[e.RowIndex].Cells[5].ReadOnly = true;
            GridViewJournal.Rows[e.RowIndex].Cells[6].ReadOnly = true;
            if (GridViewJournal.Rows[e.RowIndex].Cells[1].Value != null)
            {
                GridViewJournal.Rows[e.RowIndex].Cells[1].ReadOnly = false;
                GridViewJournal.Rows[e.RowIndex].Cells[2].ReadOnly = false;
                GridViewJournal.Rows[e.RowIndex].Cells[3].ReadOnly = false;
                GridViewJournal.Rows[e.RowIndex].Cells[4].ReadOnly = false;
                GridViewJournal.Rows[e.RowIndex].Cells[5].ReadOnly = false;
                GridViewJournal.Rows[e.RowIndex].Cells[6].ReadOnly = false;
            }
        }

        private void GridViewJournal_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void GridViewJournal_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (BackupContextMenuStrip == null)
            {
                BackupContextMenuStrip = e;
            }
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                if (GridViewJournal.CurrentCell.Value == null)
                { ((ComboBox)e.Control).SelectedIndex = -1; }
                e.Control.KeyPress += new KeyPressEventHandler(GridViewJournal_KeyPress1!);
            }

            else
            {
                e.Control.ContextMenuStrip = BackupContextMenuStrip.Control.ContextMenuStrip;
            }
        }

        private void GridViewJournal_KeyPress1(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)GridViewJournal.EditingControl).DroppedDown = false;
        }

        private void GridViewJournal_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            GridViewJournal.Rows[e.RowIndex].Cells[0].Value = GridViewJournal.Rows.Count;
            GridViewJournal.Rows[e.RowIndex].Cells[1].Value = "";
            GridViewJournal.Rows[e.RowIndex].Cells[2].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            GridViewJournal.Rows[e.RowIndex].Cells[3].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            GridViewJournal.Rows[e.RowIndex].Cells[4].Value = "";
            GridViewJournal.Rows[e.RowIndex].Cells[5].Value = "";
            GridViewJournal.Rows[e.RowIndex].Cells[6].Value = "X";
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F3)
            {
                if (BtnJournalNew.Enabled) BtnJournalNew.PerformClick();
                else BtnJournalNewAccounts.ShowDropDown();
                return true;
            }
            else if (keyData == Keys.F4)
            {
                BtnJournalDelete.PerformClick();
                return true;
            }
            else if (keyData == Keys.F9)
            {
                BtnJournalPrint.PerformClick();
                return true;
            }
            else if (keyData == Keys.F8)
            {
                BtnJournalSave.PerformClick();
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                BtnJournalCancel.PerformClick();
                return true;
            }
            else if (keyData == Keys.F10)
            {
                BtnJournalExit.PerformClick();
                return true;
            }

            if (keyData == Keys.Tab && JournalSearchGo.Selected)
            {
                DatePickerJournal.Focus();
                return true;
            }

            if (HandleButtonNavigation(keyData))
                return true;

            if (GridViewJournal.CurrentCell != null)
            {
                GridViewJournal.CommitEdit(DataGridViewDataErrorContexts.Commit);

                if (keyData == Keys.F2 && (GridViewJournal.CurrentCell.ColumnIndex == 1 ||
                                           GridViewJournal.CurrentCell.ColumnIndex == 5))
                {
                    _gridToolTip.Hide(GridViewJournal);
                    SelectGenAccount = GridViewJournal.CurrentCell.ColumnIndex == 1;
                    _gridToolTip.RemoveAll();
                    OpenAccountSelectionForm(SelectGenAccount);
                    return true;
                }

                if (keyData == Keys.Tab || keyData == (Keys.Tab | Keys.Shift))
                {
                    return HandleGridTabNavigation(keyData);
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private bool HandleButtonNavigation(Keys keyData)
        {
            if (keyData == Keys.Left && ActiveControl == BtnJournalSave)
            {
                BtnJournalCancel.Select();
                return true;
            }
            if (keyData == Keys.Right && ActiveControl == BtnJournalSave)
            {
                BtnJournalExit.Select();
                return true;
            }
            if (keyData == Keys.Right && ActiveControl == BtnJournalCancel)
            {
                BtnJournalSave.Select();
                return true;
            }
            if (keyData == Keys.Left && ActiveControl == BtnJournalCancel)
            {
                BtnJournalExit.Select();
                return true;
            }
            if (keyData == Keys.Left && ActiveControl == BtnJournalExit)
            {
                BtnJournalSave.Select();
                return true;
            }
            if (keyData == Keys.Right && ActiveControl == BtnJournalExit)
            {
                BtnJournalCancel.Select();
                return true;
            }
            return false;
        }

        private bool HandleGridTabNavigation(Keys keyData)
        {
            int currentCol = GridViewJournal.CurrentCell.ColumnIndex;
            int currentRow = GridViewJournal.CurrentCell.RowIndex;
            bool shiftPressed = (keyData & Keys.Shift) == Keys.Shift;

            if (shiftPressed)
            {
                if (currentCol > 0)
                {
                    GridViewJournal.CurrentCell = GridViewJournal[currentCol - 1, currentRow];
                }
                else if (currentRow > 0)
                {
                    GridViewJournal.CurrentCell = GridViewJournal[GridViewJournal.ColumnCount - 1, currentRow - 1];
                }
                return true;
            }
            else
            {
                if (currentCol < GridViewJournal.ColumnCount - 1)
                {
                    GridViewJournal.CurrentCell = GridViewJournal[currentCol + 1, currentRow];
                }
                else if (currentRow < GridViewJournal.Rows.Count - 1)
                {
                    GridViewJournal.CurrentCell = GridViewJournal[0, currentRow + 1];
                }
                else
                {
                    SendKeys.Send("{tab}");
                }
                return true;
            }
        }
        public int blinkCount;
        private void ResetTimmer()
        {
            blinkCount = 0;
            TimerJournal.Stop();
            TimerJournal.Start();
        }
        private void TimerJournal_Tick(object sender, EventArgs e)
        {
            this.JournalErrorMsg.Visible = !this.JournalErrorMsg.Visible;
            blinkCount++;
            if (blinkCount == 3 * 2)
            {
                TimerJournal.Stop();
                JournalErrorMsg.Visible = true;
            }
        }

        private void JournalSearchGo_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SearchJournal();
            if (SearchJournalId != 0)
            {
                if (this.formIsDirty)
                {
                    DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                    if (Result == DialogResult.Yes)
                    {
                        if (ValidateForm())
                        {
                            BtnJournalSave_Click(sender, e);
                            LoadJournal(SearchJournalId);
                        }
                    }
                    else if (Result == DialogResult.No)
                    {
                        LoadJournal(SearchJournalId);
                    }
                }
                else
                {
                    LoadJournal(SearchJournalId);
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void LoadJournal(long JournalId)
        {
            ResetForm();
            EnableForm(false);
            Journal Journal = JournalManager.GetJournal(JournalId);
            if (Journal != null)
            {
                JournalRefNo.Text = Journal.ReferenceNumber;
                TextBoxJournalId.Text = Journal.JournalId.ToString();
                DatePickerJournal.Date = (DateTime)DateUtils.ToDate(Journal.TransactionDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                TextBoxJournalMemo.Text = Journal.Memo;
                if (Journal.JournalDetails.Count > 0)
                {
                    IList<AccountHelperData> FromAccount = AccountHelper.GetHelpData(0L, true, true, false, true, string.Empty, Global.Company);
                    IList<Account> ToAccount = AccountManager.Instance.GetAllAccountsByCompanyId(Global.Company.CompanyId);
                    if (FromAccount != null && ToAccount != null)
                    {
                        GridViewJournal.Rows.Add(Journal.JournalDetails.Count);
                        int i = 0;
                        foreach (var JournalDetail in Journal.JournalDetails)
                        {
                            GridViewJournal.Rows[i].Cells[6].Value = "X";

                            GridViewJournal.Rows[i].Cells[0].Value = i + 1;
                            string AcntName = "";
                            JournalDetail JournalAccount = JournalManager.GetJournalDetailAccount((long)JournalDetail.JournalDetailId);
                            if (JournalAccount != null)
                            {
                                AcntName = "";
                                if (JournalAccount.ToAccountId != null)
                                {
                                    Account lAccount = AccountManager.Instance.GetAccountById((long)JournalAccount.ToAccountId!);
                                    if (lAccount != null)
                                    {
                                        AcntName = lAccount.Name;
                                    }
                                }
                            }
                            GridViewJournal.Rows[i].Cells[1].Value = AcntName;
                            if (JournalDetail.Amount > 0)
                            {
                                GridViewJournal.Rows[i].Cells[2].Value = JournalDetail.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                GridViewJournal.Rows[i].Cells[3].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                            }
                            else
                            {
                                GridViewJournal.Rows[i].Cells[3].Value = Math.Abs(JournalDetail.Amount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                GridViewJournal.Rows[i].Cells[2].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                            }
                            GridViewJournal.Rows[i].Cells[4].Value = JournalDetail.Description;
                            if (JournalAccount != null)
                            {
                                AcntName = "";
                                if (JournalAccount.ReferenceAccountId != null)
                                {
                                    Account lAccount = AccountManager.Instance.GetAccountById((long)JournalAccount.ReferenceAccountId!);
                                    if (lAccount != null)
                                    {
                                        AcntName = lAccount.Name;
                                    }
                                }
                            }
                            GridViewJournal.Rows[i].Cells[5].Value = AcntName;
                            i++;
                        }
                    }
                    Row_Added();
                    ReSequence();
                }
                this.formIsDirty = false;
            }
            else
            {
                SearchJournalId = 0L;
                MessageBox.Show(WentWrongErrorMsg);
                return;
            }
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            BtnJournalCancel.PerformClick();
            return;
        }
        private void SearchJournal()
        {
            string text = TextBoxJournalSearch.Text;
            IList<Journal> JournalInfo = null!;

            if (DateUtils.ValidDate(text, Global.Company.DateFormat))
            {
                DateTime? Date = (DateTime)DateUtils.ToDate(text, Global.Company.DateFormat)!;
                JournalInfo = JournalManager.GetJournalByDate(Date, Global.Company.CompanyId);
            }
            else
            {
                JournalInfo = JournalManager.GetJournalByReferenceNo(text, Global.Company.CompanyId);
            }
            if (JournalInfo.Count > 0)
            {
                LoadJournal(JournalInfo);
            }
            else
            {
                SearchJournalId = 0L;
                JournalErrorMsg.Text = SearchOutput;
            }
        }
        public void LoadJournal(IList<Journal> JournalInfo)
        {
            SearchJournalId = 0L;
            FormRecentJournal FormRecentJournal = new FormRecentJournal(this);
            FormRecentJournal.JournalInfo = JournalInfo;
            FormRecentJournal.ShowDialog();
        }

        private void BtnJournalSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                DatePickerJournal.Focus();

            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewJournal.Rows.Count > 0)
                {
                    GridViewJournal.Select();
                    GridViewJournal.CurrentCell = GridViewJournal[5, GridViewJournal.Rows.Count - 1];
                    GridViewJournal.BeginEdit(true);
                }
            }
        }

        private void TextBoxJournalMemo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                TextBoxJournalMemo.SelectionLength = 0;
                e.IsInputKey = true;
                if (GridViewJournal.Rows.Count > 0)
                {
                    GridViewJournal.Select();
                    GridViewJournal.CurrentCell = GridViewJournal[0, 0];
                    GridViewJournal.BeginEdit(true);
                }

            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxJournalMemo.SelectionLength = 0;
                DatePickerJournal.Focus();
            }
        }

        private void TextBoxJournalMemo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                e.Handled = true;
            }
        }

        private void FormJournal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    DatePickerJournal.Focus();
                    e.Cancel = true;
                }
            }
        }

        private void TextBoxJournalSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                JournalSearchGo.PerformClick();
            }
        }

        private void GridViewJournal_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
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

        private void GridViewJournal_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                if (GridViewJournal.CurrentCell == GridViewJournal[0, 0])
                {
                    e.IsInputKey = true;
                    TextBoxJournalMemo.Select();
                }
            }
        }

        private void OpenAccountSelectionForm(bool isGeneralAccount)
        {
            _isSelectingAccount = true;
            try
            {
                var currentCell = GridViewJournal.CurrentCell;
                string currentValue = currentCell.Value?.ToString()!;
                Account? currentAccount = currentCell.Tag as Account;

                using (var form = new FormSelectAccount())
                {
                    form.IsAllGeneralAccount = isGeneralAccount;

                    PositionFormNearCell(form);

                    if (currentAccount != null)
                    {
                        form.PreselectedAccountCode = currentAccount.Name;
                    }
                    else if (!string.IsNullOrEmpty(currentValue))
                    {
                        string accountCode = currentValue.Split('-')[0].Trim();
                        form.PreselectedAccountCode = accountCode;
                    }

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        UpdateJournalCell(currentCell, form.SelectedAccount!, isGeneralAccount);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting account: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isSelectingAccount = false;
            }
        }

        private void PositionFormNearCell(Form form)
        {
            Rectangle cellRect = GridViewJournal.GetCellDisplayRectangle(
                GridViewJournal.CurrentCell.ColumnIndex,
                GridViewJournal.CurrentCell.RowIndex,
                false);

            Point formLocation = GridViewJournal.PointToScreen(
                new Point(cellRect.Left, cellRect.Bottom));

            form.StartPosition = FormStartPosition.Manual;
            form.Location = formLocation;
        }

        private void UpdateJournalCell(DataGridViewCell cell, Account account, bool isGeneralAccount)
        {
            if (account == null) return;
            _isSelectingAccount = true;
            try
            {
                cell.Value = account.Name;
                cell.Tag = account;

                GridViewJournal.SuspendLayout();
                GridViewJournal.BeginEdit(false);
                GridViewJournal.Rows[cell.RowIndex].Cells[isGeneralAccount ? 1 : 5].Value = account.Name;
                GridViewJournal.EndEdit();
                GridViewJournal.Rows[cell.RowIndex].Cells[isGeneralAccount ? 1 : 5].Tag = account;

                if (cell.ColumnIndex < GridViewJournal.ColumnCount - 1)
                {
                    int nextCol = cell.ColumnIndex + 1;
                    GridViewJournal.CurrentCell = GridViewJournal.Rows[cell.RowIndex].Cells[nextCol];
                }
                else
                {
                    if (cell.RowIndex < GridViewJournal.Rows.Count - 1)
                    {
                        GridViewJournal.CurrentCell = GridViewJournal.Rows[cell.RowIndex + 1].Cells[0];
                    }
                    else
                    {
                        GridViewJournal.CurrentCell = cell;
                    }
                }

                GridViewJournal.BeginEdit(true);
                GridViewJournal.NotifyCurrentCellDirty(true);
                GridViewJournal.EndEdit();
            }
            finally
            {
                _isSelectingAccount = false;
                GridViewJournal.ResumeLayout();

                if (GridViewJournal.CurrentCell != null)
                {
                    GridViewJournal.FirstDisplayedScrollingRowIndex = GridViewJournal.CurrentCell.RowIndex;
                }
            }
        }

        private void DatePickerJournal_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxJournalMemo.Select();
            }
            else if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnJournalSave.Select();
            }
        }

        private void GridViewJournal_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (!_isSelectingAccount && (e.ColumnIndex == 1 || e.ColumnIndex == 5))
            {
                GridViewJournal.CommitEdit(DataGridViewDataErrorContexts.LeaveControl);
            }
            if (e.ColumnIndex == 1 || e.ColumnIndex == 5)
            {
                if (_isSelectingAccount) return;

                var cell = GridViewJournal.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string cellValue = cell.EditedFormattedValue?.ToString()!;

                if (!string.IsNullOrEmpty(cellValue))
                {
                    try
                    {
                        if (cell.Tag is Account existingAccount &&
                            existingAccount.Name.Equals(cellValue, StringComparison.OrdinalIgnoreCase))
                        {
                            return;
                        }

                        Account acc = AccountManager.Instance.CheckAllAccountByName(
                            cellValue,
                            Global.Company.CompanyId);

                        if (acc != null)
                        {
                            cell.Value = acc.Name;
                            cell.Tag = acc;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Account validation failed: {ex.Message}");
                    }
                }
            }
        }

        private void BtnJournalNewAccounts_ItemClickedEvent(object sender, ToolStripItemClickedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (e.ClickedItem.Text == "Account")
            {
                FormGeneralAccounts FormGeneralAccounts = new FormGeneralAccounts(this);
                FormGeneralAccounts.CreateAccountOnLoad = true;
                FormGeneralAccounts.ShowDialog(this);
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
            Cursor.Current = Cursors.Default;
        }

        private void GridViewJournal_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                e.ColumnIndex != (int)PurchaseEntryTableColumn.SNO &&
                e.ColumnIndex != (int)PurchaseEntryTableColumn.REMOVE)
            {
                BtnJournalSave.Enabled = true;
            }

        }
    }
}
