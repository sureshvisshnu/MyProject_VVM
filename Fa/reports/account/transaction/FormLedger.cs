using fa.libraries.utils;
using fa.api.Accounting;
using fa.model.Accounting.Masters;
using System;
using System.Windows.Forms;
using fa.api.utils;
using fa.views.controls.ComboTreeView;
using Fa.report.accounting.master;
using fa.views.utils.Report.Account;
using fa.model.Common;
using System.Drawing;
using ScottPlot.Palettes;
using System.Data;
using fa.model.Hms.Master;
using fa.views.controls;

namespace fa.views.account.transactions
{
    enum LedgerTableColumn
    {
        ICON, DATE, DESC, CREDIT, DEBIT, BALANCE, ID
    }
    public partial class FormLedger : Form
    {
        AccountManager AccountManager = null!;
        RptLedger RptLedger = null!;

        public static string InformationMsg = "No Information Found..!";

        public FormLedger()
        {
            InitializeComponent();
            AccountManager = AccountManager.Instance;
        }
        private void FormLedger_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ComboUtils.InitializeAllAccountComboForLedger(ComboBoxAccounts, (long)Global.Company.CompanyId);
                ComboUtils.InitializeCostCenterComboByUserAccess(ComboBoxCostCenter, (long)Global.Company.CompanyId);
                ResetForm();
                ComboBoxAccounts.Select();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private bool FormValidate()
        {
            ErrorMsg.Text = "";
            if (CheckedTreeUtils.SelectedNodes(ComboBoxAccounts).Count < 1)
            {
                ErrorMsg.Text = "Please Choose Account";
                ComboBoxAccounts.Select();
                return false;
            }
            if (LedgerFromDate.Date == null || !DateUtils.ValidDate(((DateTime)LedgerFromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsg.Text = "Enter valid Date";
                LedgerFromDate.Focus();
                return false;
            }
            if (LedgerToDate.Date == null || !DateUtils.ValidDate(((DateTime)LedgerToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsg.Text = "Enter valid Date";
                LedgerToDate.Focus();
                return false;
            }
            return true;
        }
        private void RunReportButton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                LedgerAccountDataGridView.Rows.Clear();
                EnableButton(false);

                if (FormValidate())
                {
                    LoadAccount();
                }
            }
            catch (Exception ex)
            {
                ErrorMsg.Text = "Errod fetching ledger (Error:" + ex.InnerException.Message + ")";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void LoadAccount()
        {
            RptLedger = new RptLedger();
            RptLedger.FromDate = (DateTime)LedgerFromDate.Date!;
            RptLedger.ToDate = (DateTime)LedgerToDate.Date!;
            RptLedger.Company = Global.Company;
            RptLedger.AccountIds = CheckedTreeUtils.SelectedNodes(ComboBoxAccounts).ToArray();
            if (ComboBoxCostCenter.Visible && ComboBoxCostCenter.SelectedIndex > 0) 
            {
                RptLedger.CostCenterId = ((CostCenter)ComboBoxCostCenter.Items[ComboBoxCostCenter.SelectedIndex]).CostCenterId;
            }
            RptLedger.GenerateReport();
            bool ComboBoxNodes = SelectedNodesText(ComboBoxAccounts) == "All Accounts" ? true : false;
            if(ComboBoxNodes == false)
            {
                var accountsWithoutLineItems = RptLedger.AccountIds.Where(accountId => !RptLedger.Ledgers.Any(ledger => ledger.Account.Id == accountId && ledger.LineItems.Count > 0)).Select(accountId => api.Accounting.AccountManager.Instance.GetAccountById(accountId).DisplayAs).ToList();
                if (accountsWithoutLineItems.Count > 0)
                {
                    var message = "The following accounts have no Transaction:\n" + string.Join("\n", accountsWithoutLineItems.Select((name, index) => $"{index + 1}. {name}"));

                    MessageBox.Show(message, "No Transaction", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LedgerAccountDataGridView.Columns[(int)LedgerTableColumn.DESC].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            LedgerAccountDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            if (RptLedger.Ledgers != null && RptLedger.Ledgers.Count > 0)
            {
                EnableButton(true);
                Currency Currency = api.Accounting.CurrencyManager.Instance.GetCurrencyById((long)Global.Company.PrimaryCurrencyId);
                LedgerAccountDataGridView.Rows.Clear();
                double totalCredit = 0;
                double totalDebit = 0;
                foreach (Fa.report.accounting.master.Ledger Ledger in RptLedger.Ledgers)
                {
                    //if (Ledger.LineItems.Count > 0)
                    {
                        //Print Account Name
                        int row = LedgerAccountDataGridView.Rows.Add();
                        //seting a color for row.
                        LedgerAccountDataGridView.Rows[row].DefaultCellStyle.BackColor = Color.Gray;
                        LedgerAccountDataGridView.Rows[row].DefaultCellStyle.ForeColor = Color.White;
                        LedgerAccountDataGridView.Rows[row].DefaultCellStyle.SelectionBackColor = Color.Gray;
                        LedgerAccountDataGridView.Rows[row].DefaultCellStyle.SelectionForeColor = Color.White;

                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.ICON].Value = "-";
                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.DESC].Value = Ledger.Account.DisplayAs;
                        //Print opening Balance
                        row = LedgerAccountDataGridView.Rows.Add();
                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.DATE].Value = RptLedger.FromDate.ToString(Global.Company.DateFormat);
                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.DESC].Value = "Op. Balance";
                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.BALANCE].Value = Math.Abs(Ledger.OpeningBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + " " + (Ledger.OpeningBalance == 0 ? "      " : Ledger.OpeningBalance > 0 ? " DR" : " CR"); // Math.Abs(Ledger.OpeningBalance).ToString(TextUtils.DecimalPlace(Currency.RoundingPrecision)) + (Ledger.CreditOrDebit(Ledger.OpeningBalance) == report.common.CrDr.CR ? " CR" : " DR");
                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.ID].Value = Ledger.Account.Id;
                        DateTime? lDate = null;
                        totalCredit = 0;
                        totalDebit = 0;
                        double RBalance = Ledger.OpeningBalance;
                        //Print Line items
                        foreach (LedgerLineItem LineItem in Ledger.LineItems)
                        {
                            row = LedgerAccountDataGridView.Rows.Add();
                            if (lDate != LineItem.Date.Date)
                            {
                                LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.DATE].Value = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                                lDate = LineItem.Date.Date;
                            }
                            if (LineItem.CreditOrDebit() == report.common.CrDr.CR)
                            {
                                LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.DESC].Value = "By " + (LineItem.Account != null ? LineItem.Account.DisplayAs : LineItem.Patient.Name) + Environment.NewLine + LineItem.Description;
                                LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.CREDIT].Value = Math.Abs(LineItem.Amount).ToString(TextUtils.DecimalPlace(Currency.RoundingPrecision));
                                RBalance -= Math.Abs(LineItem.Amount);
                                totalCredit += Math.Abs(LineItem.Amount);
                            }
                            else
                            {
                                LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.DESC].Value = "To " + (LineItem.Account != null ? LineItem.Account.DisplayAs : LineItem.Patient.Name) + (!String.IsNullOrEmpty(LineItem.Description) ? Environment.NewLine + "For " + LineItem.Description.Replace("By", "").Trim() : "");
                                LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.DEBIT].Value = Math.Abs(LineItem.Amount).ToString(TextUtils.DecimalPlace(Currency.RoundingPrecision));
                                RBalance += Math.Abs(LineItem.Amount);
                                totalDebit += Math.Abs(LineItem.Amount);
                            }
                            LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.BALANCE].Value = Math.Abs(RBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + " " + (RBalance == 0 ? "      " : RBalance > 0 ? " DR" : " CR");  // Math.Abs(RBalance).ToString(TextUtils.DecimalPlace(Currency.RoundingPrecision)) + " " + Ledger.CreditOrDebit(RBalance);
                            LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.ID].Value = LineItem.Account.Id;
                        }
                        //Print Closing Balance
                        row = LedgerAccountDataGridView.Rows.Add();
                        LedgerAccountDataGridView.Rows[row].DefaultCellStyle.BackColor = SystemColors.Control;
                        LedgerAccountDataGridView.Rows[row].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.DESC].Value = "Sub Total";
                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.CREDIT].Value = totalCredit.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.DEBIT].Value = totalDebit.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                        row = LedgerAccountDataGridView.Rows.Add();
                        LedgerAccountDataGridView.Rows[row].DefaultCellStyle.BackColor = Color.LightGray;
                        LedgerAccountDataGridView.Rows[row].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.DESC].Value = "Cl. Balance";
                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.BALANCE].Value = Math.Abs(Ledger.ClosingBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + " " + (Ledger.ClosingBalance == 0 ? "      " : Ledger.ClosingBalance > 0 ? " DR" : " CR");  // Math.Abs(Ledger.ClosingBalance).ToString(TextUtils.DecimalPlace(Currency.RoundingPrecision)) + (Ledger.CreditOrDebit(Ledger.ClosingBalance) == report.common.CrDr.CR ? " CR" : " DR");
                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.ID].Value = Ledger.Account.Id;
                    }
                }
            }
            else
            {
                ErrorMsg.Text = "No Record Found..";
            }
        }
        private string SelectedNodesText(ToolstripCheckedTreeComboBox ComboTreeBox)
        {
            int i = 0;
            string Name = string.Empty;
            if (ComboTreeBox.Nodes.Count > 0)
            {
                foreach (ComboTreeNode ComboTreeNode in ComboTreeBox.Nodes)
                {
                    if (ComboTreeNode != null)
                    {
                        if (ComboTreeNode.Checked == true)
                        {
                            if (ComboTreeNode.Name == "All")
                            {
                                Name = string.Empty;
                                Name = "All Accounts";
                                break;
                            }
                            else
                            {
                                Name += string.IsNullOrEmpty(Name) ? ComboTreeNode.Text : (", " + ComboTreeNode.Text);
                            }
                            i++;
                        }
                    }
                }
            }
            return Name;
        }
        private void EnableButton(bool Enable)
        {
            BtnPrint.Enabled = Enable;
            BtnSave.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
        }
        private void BtnReset_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            ComboBoxAccounts.Select();
            this.Text = "Ledger";
            Cursor.Current = Cursors.Default;
        }
        private void ResetForm()
        {
            LedgerAccountDataGridView.Rows.Clear();
            ErrorMsg.Text = "";
            EnableButton(false);
            if (ComboBoxCostCenter.Items.Count > 1)
            {
                LabelCostCenter.Visible = true;
                ComboBoxCostCenter.Visible = true;
                ComboBoxCostCenter.SelectedIndex = 0;
            }
            else
            {
                LabelCostCenter.Visible = false;
                ComboBoxCostCenter.Visible = false;
            }
            ComboBoxAccounts.SelectedNode = null;
            foreach (ComboTreeNode ComboTreeNode in ComboBoxAccounts.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            LedgerFromDate.Format = Global.Company.DateFormat;
            LedgerToDate.Format = Global.Company.DateFormat;
            //LedgerToDate.MinDate = Global.getCurrentFiscalYearStartDate();
            //LedgerToDate.MaxDate = Global.getCurrentFiscalYearEndDate();
            //LedgerFromDate.MinDate = Global.getCurrentFiscalYearStartDate();
            //LedgerFromDate.MaxDate = Global.getCurrentFiscalYearEndDate();
            //LedgerFromDate.Date = DateUtils.ReportFromDate(Global.Company.DateFormat, 30);
            LedgerFromDate.Date = Global.getTransactionDate().AddDays(-30);
            LedgerToDate.Date = Global.getTransactionDate();
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            utils.Report.Account.Ledger Ledger = new utils.Report.Account.Ledger();
            Ledger.ExportToFileOrPrint(RptLedger, false);
            Cursor.Current = Cursors.Default;
        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            utils.Report.Account.Ledger Ledger = new utils.Report.Account.Ledger();
            Ledger.ExportToFileOrPrint(RptLedger, true);
            Cursor.Current = Cursors.Default;
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
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
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void DisplayCheckedAccount()
        {
            string CheckedNodes = string.Empty;
            if (ComboBoxAccounts.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboBoxAccounts.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All Accounts"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                CheckedNodes = " @ " + CheckedNodes;
            }
            this.Text = "Ledger" + CheckedNodes;
        }
        private void ComboBoxAccounts_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }
        private void ComboBoxAccounts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Tab && ComboBoxCostCenter.Visible)
            {
                ComboBoxCostCenter.Focus();
            }
        }
        private void LedgerAccountDataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                var value = LedgerAccountDataGridView[2, e.RowIndex].Value != null ? LedgerAccountDataGridView[2, e.RowIndex].Value.ToString() : "";
                if (value == "Sub Total" || value == "Cl. Balance")
                {
                    if (e.ColumnIndex == 0 || e.ColumnIndex == 1)
                    {
                        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    }
                    if (e.ColumnIndex < 3)
                    {
                        e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    }
                    if (e.ColumnIndex == 2)
                    {
                        e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                    }
                }
            }
        }
        private void LoadLedgerDataReport1(DataTable ledgerReport, double OpeningBal)
        {
            double dailyCreditBalance = 0;
            double dailyDebitBalance = 0;
            double CashBalance = 0;
            bool isOpeningBalance = true;

            LedgerAccountDataGridView.Columns[(int)LedgerTableColumn.DESC].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            LedgerAccountDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            if (ledgerReport != null && ledgerReport.Rows.Count > 0)
            {
                int NewRow = 0;
                EnableButton(true);

                AddLedgerCustomRowBalance(OpeningBal, "Opening Balance", true);
                double DailyBalance = OpeningBal;
                string LastDate = null!;

                string LCreditValueString = "0";
                string LDebitValueString = "0";

                foreach (DataRow row in ledgerReport.Rows)
                {
                    string stringLineItemDate;

                    if (row["Date"] != DBNull.Value)
                    {
                        stringLineItemDate = Convert.ToDateTime(row["Date"]).ToString(Global.Company.DateFormat);
                    }
                    else
                    {
                        stringLineItemDate = "";
                    }

                    if (!isOpeningBalance)
                    {
                        if (LedgerAccountDataGridView.Rows.Count > 0 && LedgerAccountDataGridView.Rows[0].Index != NewRow)
                        {
                            string FCreditValueString = LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.CREDIT].Value != null ? LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.CREDIT].Value.ToString()! : "0";
                            string FDebitValueString = LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DEBIT].Value != null ? LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DEBIT].Value.ToString()! : "0";
                            dailyCreditBalance += double.TryParse(FCreditValueString, out double FCredit) ? FCredit : 0;
                            dailyDebitBalance += double.TryParse(FDebitValueString, out double FDebit) ? FDebit : 0;

                            // Add "Day Total's" row
                            NewRow = LedgerAccountDataGridView.Rows.Add();
                            LedgerAccountDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.Control;
                            LedgerAccountDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                            LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DESC].Value = "Day Total's";
                            LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.CREDIT].Value = Math.Abs(dailyCreditBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DEBIT].Value = Math.Abs(dailyDebitBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

                            // Add "Cash Balance" row
                            NewRow = LedgerAccountDataGridView.Rows.Add();
                            LedgerAccountDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.ControlLight;
                            LedgerAccountDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.ControlLight;
                            LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DESC].Value = "Cash Balance";
                            CashBalance = dailyCreditBalance - dailyDebitBalance;
                            if (dailyCreditBalance > dailyDebitBalance)
                            {
                                LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.CREDIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            }
                            else
                            {
                                LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DEBIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            }
                        }

                        NewRow = LedgerAccountDataGridView.Rows.Add();
                        LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DATE].Value = stringLineItemDate;
                        LastDate = stringLineItemDate;
                        dailyCreditBalance = 0;
                        dailyDebitBalance = 0;
                    }
                    else
                    {
                        NewRow = LedgerAccountDataGridView.Rows.Add();
                    }

                    // Process credit and debit for each row
                    LCreditValueString = row["Credit"] != DBNull.Value ? row["Credit"].ToString()! : "0";
                    LDebitValueString = row["Debit"] != DBNull.Value ? row["Debit"].ToString()! : "0";
                    dailyCreditBalance += double.TryParse(LCreditValueString, out double Credit) ? Credit : 0;
                    dailyDebitBalance += double.TryParse(LDebitValueString, out double Debit) ? Debit : 0;

                    if (row["Type"].ToString() == "CR")
                    {
                        LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DESC].Value = "By " + (!string.IsNullOrEmpty(row["Description"].ToString()) ? Environment.NewLine + row["Description"] : "");
                        LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.CREDIT].Value = Math.Abs(Convert.ToDouble(row["Credit"])).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    }
                    else if (row["Type"].ToString() == "DR")
                    {
                        LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DESC].Value = "To " + Environment.NewLine + row["Description"];
                        LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DEBIT].Value = Math.Abs(Convert.ToDouble(row["Debit"])).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    }

                    isOpeningBalance = false; // Now we are processing actual transactions
                }

                // Final Day Total's and Cash Balance for the last date after processing all rows
                dailyCreditBalance += double.TryParse(LCreditValueString, out double LCredit) ? LCredit : 0;
                dailyDebitBalance += double.TryParse(LDebitValueString, out double LDebit) ? LDebit : 0;

                // Add totals and balance at the end of the loop (after all rows processed)
                NewRow = LedgerAccountDataGridView.Rows.Add();
                LedgerAccountDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.Control;
                LedgerAccountDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DESC].Value = "Day Total's";
                LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.CREDIT].Value = Math.Abs(dailyCreditBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DEBIT].Value = Math.Abs(dailyDebitBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

                NewRow = LedgerAccountDataGridView.Rows.Add();
                LedgerAccountDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.ControlLight;
                LedgerAccountDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.ControlLight;
                LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DESC].Value = "Cash Balance";
                CashBalance = dailyCreditBalance - dailyDebitBalance;
                if (dailyCreditBalance > dailyDebitBalance)
                {
                    LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.CREDIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                }
                else
                {
                    LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DEBIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                }

                AddLedgerTotal(ledgerReport);
            }
            else
            {
                ErrorMsg.Text = InformationMsg;
            }
        }
        private void LoadLedgerDataReport(DataTable ledgerReport, double OpeningBal)
        {
            double dailyCreditBalance = 0;
            double dailyDebitBalance = 0;
            double CashBalance = 0;
            bool isOpeningBalance = true;

            LedgerAccountDataGridView.Columns[(int)LedgerTableColumn.DESC].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            LedgerAccountDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            if (ledgerReport != null && ledgerReport.Rows.Count > 0)
            {
                int NewRow = 0;
                EnableButton(true);

                AddLedgerCustomRowBalance(OpeningBal, "Opening Balance", true);
                double DailyBalance = OpeningBal;
                string LastDate = null!;

                string LCreditValueString = "0";
                string LDebitValueString = "0";

                foreach (DataRow row in ledgerReport.Rows)
                {
                    string stringLineItemDate;

                    if (row["Date"] != DBNull.Value)
                    {
                        stringLineItemDate = Convert.ToDateTime(row["Date"]).ToString(Global.Company.DateFormat);
                    }
                    else
                    {
                        stringLineItemDate = "";
                    }

                    if (!isOpeningBalance)
                    {
                        if (LedgerAccountDataGridView.Rows.Count > 0 && LedgerAccountDataGridView.Rows[0].Index != NewRow)
                        {
                            string FCreditValueString = LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.CREDIT].Value != null ? LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.CREDIT].Value.ToString()! : "0";
                            string FDebitValueString = LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DEBIT].Value != null ? LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DEBIT].Value.ToString()! : "0";
                            dailyCreditBalance += double.TryParse(FCreditValueString, out double FCredit) ? FCredit : 0;
                            dailyDebitBalance += double.TryParse(FDebitValueString, out double FDebit) ? FDebit : 0;

                            // Add "Day Total's" row
                            NewRow = LedgerAccountDataGridView.Rows.Add();
                            LedgerAccountDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.Control;
                            LedgerAccountDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                            LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DESC].Value = "Day Total's";
                            LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.CREDIT].Value = Math.Abs(dailyCreditBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DEBIT].Value = Math.Abs(dailyDebitBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

                            // Add "Cash Balance" row
                            NewRow = LedgerAccountDataGridView.Rows.Add();
                            LedgerAccountDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.ControlLight;
                            LedgerAccountDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.ControlLight;
                            LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DESC].Value = "Cash Balance";
                            CashBalance = dailyCreditBalance - dailyDebitBalance;
                            if (dailyCreditBalance > dailyDebitBalance)
                            {
                                LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.CREDIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            }
                            else
                            {
                                LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DEBIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            }
                        }

                        NewRow = LedgerAccountDataGridView.Rows.Add();
                        LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DATE].Value = stringLineItemDate;
                        LastDate = stringLineItemDate;
                        dailyCreditBalance = 0;
                        dailyDebitBalance = 0;
                    }
                    else
                    {
                        NewRow = LedgerAccountDataGridView.Rows.Add();
                    }

                    // Process credit and debit for each row
                    LCreditValueString = row["Credit"] != DBNull.Value ? row["Credit"].ToString()! : "0";
                    LDebitValueString = row["Debit"] != DBNull.Value ? row["Debit"].ToString()! : "0";
                    dailyCreditBalance += double.TryParse(LCreditValueString, out double Credit) ? Credit : 0;
                    dailyDebitBalance += double.TryParse(LDebitValueString, out double Debit) ? Debit : 0;

                    if (row["Type"].ToString() == "CR")
                    {
                        LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DESC].Value = "By " + (!string.IsNullOrEmpty(row["Description"].ToString()) ? Environment.NewLine + row["Description"] : "");
                        LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.CREDIT].Value = Math.Abs(Convert.ToDouble(row["Credit"])).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    }
                    else if (row["Type"].ToString() == "DR")
                    {
                        LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DESC].Value = "To " + Environment.NewLine + row["Description"];
                        LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DEBIT].Value = Math.Abs(Convert.ToDouble(row["Debit"])).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    }

                    isOpeningBalance = false; // Now we are processing actual transactions
                }

                // Final Day Total's and Cash Balance for the last date after processing all rows
                dailyCreditBalance += double.TryParse(LCreditValueString, out double LCredit) ? LCredit : 0;
                dailyDebitBalance += double.TryParse(LDebitValueString, out double LDebit) ? LDebit : 0;

                // Add totals and balance at the end of the loop (after all rows processed)
                NewRow = LedgerAccountDataGridView.Rows.Add();
                LedgerAccountDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.Control;
                LedgerAccountDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DESC].Value = "Day Total's";
                LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.CREDIT].Value = Math.Abs(dailyCreditBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DEBIT].Value = Math.Abs(dailyDebitBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

                NewRow = LedgerAccountDataGridView.Rows.Add();
                LedgerAccountDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.ControlLight;
                LedgerAccountDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.ControlLight;
                LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DESC].Value = "Cash Balance";
                CashBalance = dailyCreditBalance - dailyDebitBalance;
                if (dailyCreditBalance > dailyDebitBalance)
                {
                    LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.CREDIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                }
                else
                {
                    LedgerAccountDataGridView.Rows[NewRow].Cells[(int)LedgerTableColumn.DEBIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                }

                AddLedgerTotal(ledgerReport);
            }
            else
            {
                ErrorMsg.Text = InformationMsg;
            }
        }


        private void LoadLedgerDataReportError(DataTable LedgerReport, double openingBalance)
        {
            LedgerAccountDataGridView.Columns[(int)LedgerTableColumn.DESC].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            LedgerAccountDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            if (LedgerReport != null && LedgerReport.Rows.Count > 0)
            {
                EnableButton(true);
                Currency currency = api.Accounting.CurrencyManager.Instance.GetCurrencyById((long)Global.Company.PrimaryCurrencyId!);
                LedgerAccountDataGridView.Rows.Clear();
                double totalCredit = 0;
                double totalDebit = 0;
                double RBalance = openingBalance; // Initialize RBalance with openingBalance
                DateTime? lDate = null; // Declare lDate at the start

                foreach (DataRow Ledger in LedgerReport.Rows)
                {
                    if (Ledger.ItemArray.Length > 0 || (double)Ledger["OpeningBalance"] != 0) // Ensure this accesses the correct value
                    {
                        // Print Account Name
                        int row = LedgerAccountDataGridView.Rows.Add();
                        // Setting a color for the row.
                        LedgerAccountDataGridView.Rows[row].DefaultCellStyle.BackColor = Color.Gray;
                        LedgerAccountDataGridView.Rows[row].DefaultCellStyle.ForeColor = Color.White;
                        LedgerAccountDataGridView.Rows[row].DefaultCellStyle.SelectionBackColor = Color.Gray;
                        LedgerAccountDataGridView.Rows[row].DefaultCellStyle.SelectionForeColor = Color.White;

                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.ICON].Value = "-";
                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.DESC].Value = Ledger["Description"].ToString(); // Ensure proper column access

                        // Print opening Balance
                        row = LedgerAccountDataGridView.Rows.Add();
                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.DATE].Value = RptLedger.FromDate.ToString(Global.Company.DateFormat);
                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.DESC].Value = "Op. Balance";
                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.BALANCE].Value = openingBalance; // Math.Abs((double)Ledger["OpeningBalance"]).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) +
                        //(Ledger["OpeningBalance"] == DBNull.Value ? "      " : (double)Ledger["OpeningBalance"] > 0 ? " DR" : " CR");

                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.ID].Value = Ledger["AccountId"]; // Ensure proper column access

                        AddLedgerCustomRowBalance(openingBalance, "Opening Balance", true);
                        // Print Line items
                        foreach (DataRow LineItem in Ledger.GetChildRows("LineItems")) // Adjust as necessary based on your DataSet
                        {
                            row = LedgerAccountDataGridView.Rows.Add();
                            if (lDate != (DateTime?)LineItem["Date"])
                            {
                                LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.DATE].Value = DateUtils.FormatDate((DateTime)LineItem["Date"], Global.Company.DateFormat);
                                lDate = (DateTime?)LineItem["Date"];
                            }

                            if ((report.common.CrDr)LineItem["CreditOrDebit"] == report.common.CrDr.CR)
                            {
                                LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.DESC].Value = "By " + (LineItem["Account"] != DBNull.Value ? LineItem["AccountDisplayAs"].ToString() : LineItem["PatientName"].ToString()) +
                                Environment.NewLine + LineItem["Description"].ToString();
                                LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.CREDIT].Value = Math.Abs((double)LineItem["Amount"]).ToString(TextUtils.DecimalPlace(currency.RoundingPrecision));
                                RBalance -= Math.Abs((double)LineItem["Amount"]);
                                totalCredit += Math.Abs((double)LineItem["Amount"]);
                            }
                            else
                            {
                                LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.DESC].Value = "To " + (LineItem["Account"] != DBNull.Value ? LineItem["AccountDisplayAs"].ToString() : LineItem["PatientName"].ToString()) +
                                (!string.IsNullOrEmpty(LineItem["Description"].ToString()) ? Environment.NewLine + LineItem["Description"].ToString() : "");
                                LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.DEBIT].Value = Math.Abs((double)LineItem["Amount"]).ToString(TextUtils.DecimalPlace(currency.RoundingPrecision));
                                RBalance += Math.Abs((double)LineItem["Amount"]);
                                totalDebit += Math.Abs((double)LineItem["Amount"]);
                            }

                            LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.BALANCE].Value = Math.Abs(RBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) +
                            (RBalance == 0 ? "      " : RBalance > 0 ? " DR" : " CR");
                            LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.ID].Value = LineItem["AccountId"]; // Ensure proper column access
                        }

                        // Print Closing Balance
                        row = LedgerAccountDataGridView.Rows.Add();
                        LedgerAccountDataGridView.Rows[row].DefaultCellStyle.BackColor = SystemColors.Control;
                        LedgerAccountDataGridView.Rows[row].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.DESC].Value = "Sub Total";
                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.CREDIT].Value = totalCredit.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.DEBIT].Value = totalDebit.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                        row = LedgerAccountDataGridView.Rows.Add();
                        LedgerAccountDataGridView.Rows[row].DefaultCellStyle.BackColor = Color.LightGray;
                        LedgerAccountDataGridView.Rows[row].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.DESC].Value = "Cl. Balance";
                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.BALANCE].Value = Math.Abs((double)Ledger["ClosingBalance"]).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) +
                        (Ledger["ClosingBalance"] == DBNull.Value ? "      " : (double)Ledger["ClosingBalance"] > 0 ? " DR" : " CR");
                        LedgerAccountDataGridView.Rows[row].Cells[(int)LedgerTableColumn.ID].Value = Ledger["AccountId"]; // Ensure proper column access
                    }
                }
            }
            else
            {
                ErrorMsg.Text = InformationMsg;
            }
        }


        private void AddDayTotalsAndCashBalance(ref double dailyCreditBalance, ref double dailyDebitBalance)
        {
            int newRow = LedgerAccountDataGridView.Rows.Add();
            LedgerAccountDataGridView.Rows[newRow].DefaultCellStyle.BackColor = SystemColors.Control;
            LedgerAccountDataGridView.Rows[newRow].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
            LedgerAccountDataGridView.Rows[newRow].Cells[(int)LedgerTableColumn.DESC].Value = "Day Total's";
            LedgerAccountDataGridView.Rows[newRow].Cells[(int)LedgerTableColumn.CREDIT].Value = Math.Abs(dailyCreditBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            LedgerAccountDataGridView.Rows[newRow].Cells[(int)LedgerTableColumn.DEBIT].Value = Math.Abs(dailyDebitBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

            newRow = LedgerAccountDataGridView.Rows.Add();
            LedgerAccountDataGridView.Rows[newRow].DefaultCellStyle.BackColor = SystemColors.ControlLight;
            LedgerAccountDataGridView.Rows[newRow].DefaultCellStyle.SelectionBackColor = SystemColors.ControlLight;
            LedgerAccountDataGridView.Rows[newRow].Cells[(int)LedgerTableColumn.DESC].Value = "Cash Balance";
            double cashBalance = dailyCreditBalance - dailyDebitBalance;

            if (dailyCreditBalance > dailyDebitBalance)
            {
                LedgerAccountDataGridView.Rows[newRow].Cells[(int)LedgerTableColumn.CREDIT].Value = Math.Abs(cashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            }
            else
            {
                LedgerAccountDataGridView.Rows[newRow].Cells[(int)LedgerTableColumn.DEBIT].Value = Math.Abs(cashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            }

            // Reset daily balances for the next iteration
            dailyCreditBalance = 0;
            dailyDebitBalance = 0;
        }

        private void AddLedgerRow(DataRow row, string date)
        {
            int newRow = LedgerAccountDataGridView.Rows.Add();
            LedgerAccountDataGridView.Rows[newRow].Cells[(int)LedgerTableColumn.DATE].Value = date;

            if (row["Type"].ToString() == "CR")
            {
                LedgerAccountDataGridView.Rows[newRow].Cells[(int)LedgerTableColumn.DESC].Value = "By " + (!string.IsNullOrEmpty(row["Description"].ToString()) ? Environment.NewLine + row["Description"] : "");
                LedgerAccountDataGridView.Rows[newRow].Cells[(int)LedgerTableColumn.CREDIT].Value = Math.Abs(Convert.ToDouble(row["Credit"])).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            }
            else if (row["Type"].ToString() == "DR")
            {
                LedgerAccountDataGridView.Rows[newRow].Cells[(int)LedgerTableColumn.DESC].Value = "To " + Environment.NewLine + row["Description"];
                LedgerAccountDataGridView.Rows[newRow].Cells[(int)LedgerTableColumn.DEBIT].Value = Math.Abs(Convert.ToDouble(row["Debit"])).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            }
        }

        private void AddLedgerTotal(DataTable ledgerReport)
        {
            // Initialize variables to store total credit and debit
            double totalCredit = 0;
            double totalDebit = 0;

            // Loop through the DataTable and sum up the credit and debit values
            foreach (DataRow row in ledgerReport.Rows)
            {
                // Assuming the columns for credit and debit are named "Credit" and "Debit"
                double credit = row["Credit"] != DBNull.Value ? Convert.ToDouble(row["Credit"]) : 0;
                double debit = row["Debit"] != DBNull.Value ? Convert.ToDouble(row["Debit"]) : 0;

                totalCredit += credit;
                totalDebit += debit;
            }

            // Add "Total" row to the DataGridView
            int newRow = LedgerAccountDataGridView.Rows.Add();
            LedgerAccountDataGridView.Rows[newRow].Cells[(int)LedgerTableColumn.DESC].Value = "Total";
            LedgerAccountDataGridView.Rows[newRow].Cells[(int)LedgerTableColumn.CREDIT].Value = Math.Abs(totalCredit).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            LedgerAccountDataGridView.Rows[newRow].Cells[(int)LedgerTableColumn.DEBIT].Value = Math.Abs(totalDebit).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

            // Add "Closing Cash Balance" row
            newRow = LedgerAccountDataGridView.Rows.Add();
            LedgerAccountDataGridView.Rows[newRow].Cells[(int)LedgerTableColumn.DESC].Value = "Closing Cash Balance";

            double closingBalance = totalCredit - totalDebit;
            if (closingBalance > 0)
                LedgerAccountDataGridView.Rows[newRow].Cells[(int)LedgerTableColumn.CREDIT].Value = Math.Abs(closingBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            else
                LedgerAccountDataGridView.Rows[newRow].Cells[(int)LedgerTableColumn.DEBIT].Value = Math.Abs(closingBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
        }

        private void AddLedgerCustomRowBalance(double balance, string description, bool isCredit)
        {
            int newRow = LedgerAccountDataGridView.Rows.Add();

            // Set the description
            LedgerAccountDataGridView.Rows[newRow].Cells[(int)LedgerTableColumn.DESC].Value = description;

            // Add the balance to either the credit or debit column based on isCredit
            if (isCredit)
            {
                LedgerAccountDataGridView.Rows[newRow].Cells[(int)LedgerTableColumn.CREDIT].Value = Math.Abs(balance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            }
            else
            {
                LedgerAccountDataGridView.Rows[newRow].Cells[(int)LedgerTableColumn.DEBIT].Value = Math.Abs(balance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            }
        }

        private void fORLedgerReport()
        {
            //  From Line no 102 -----------------
            //DataTable openingBalanceTable = RptLedger.GetLedgerOpeningBalance((DateTime)(LedgerFromDate.Date ?? DateTime.MinValue), (DateTime)(LedgerToDate.Date ?? DateTime.MinValue), Global.Company.CompanyId, new List<long> { (long)Global.Company.CashOnHandAccountId! } );

            //var openingBalanceDataTable = RptLedger.GetLedgerOpeningBalance(
            //        LedgerFromDate.Date ?? DateTime.MinValue,
            //        LedgerToDate.Date ?? DateTime.MinValue,
            //        Global.Company.CompanyId,
            //        RptLedger.AccountIds  // Directly pass the array of selected account IDs
            //    );
            //double finalOpeningBalance = Convert.ToDouble(openingBalanceDataTable.Rows[0]["OpeningBalance"]);

            //// Assuming you have a List<long> of account IDs, possibly similar to the ones used previously

            //// Update the method call to include accountIds
            //DataTable LedgerReport = RptLedger.GetLedgerReport(
            //        LedgerFromDate.Date ?? DateTime.MinValue,
            //        LedgerToDate.Date ?? DateTime.MinValue,
            //        Global.Company.CompanyId,
            //        RptLedger.AccountIds, // Pass the account IDs list
            //        finalOpeningBalance
            //    );

            // LoadLedgerDataReport(LedgerReport, finalOpeningBalance);

            // -----------------
        }
    }
}
