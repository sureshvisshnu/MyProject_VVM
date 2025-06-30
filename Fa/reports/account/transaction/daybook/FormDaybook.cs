using fa.api.utils;
using fa.common;
using fa.libraries.utils;
using fa.model.Accounting.Masters;
using fa.reports.account.transaction.daybook;
using fa.reports.common.documents;
using Fa.report.accounting.master;
using fa.report.common;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using fa.views.utils.Report.Account.transaction;
using fa.reports.sales;
using System.Data;

namespace fa.reports.account.transaction
{


    enum GridColumn
    {
        DATE, DESC, DEBIT, CREDIT, BALANCE, ID
    }

    public partial class FormDaybook : Form
    {
        public static string EnterValidDateErrorMsg = "Enter valid date";
        public static string InformationMsg = "No Information Found..!";
        RptDayBook db = null;
        public FormDaybook()
        {
            InitializeComponent();
        }

        private void RunReportButton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (FormValidate())
                {
                    RunReport();
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
        private bool FormValidate()
        {
            ErrorMsg.Text = "";
            DaybookDataGridView.Rows.Clear();
            if (FromDate.Date == null || !DateUtils.ValidDate(((DateTime)FromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsg.Text = EnterValidDateErrorMsg;
                FromDate.Focus();
                return false;
            }
            if (ToDate.Date == null || !DateUtils.ValidDate(((DateTime)ToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsg.Text = EnterValidDateErrorMsg;
                ToDate.Focus();
                return false;
            }
            return true;
        }
        private void RunReport()
        {
            db = new RptDayBook();
            db.Company = Global.Company;
            if (ComboBoxCostCenter.Visible && ComboBoxCostCenter.SelectedIndex > 0) { db.CostcenterId = ((CostCenter)ComboBoxCostCenter.Items[ComboBoxCostCenter.SelectedIndex]).CostCenterId; }
            db.FromDate = (DateTime)FromDate.Date;
            db.ToDate = (DateTime)ToDate.Date;
            //DataTable daybookReport = db.GetDaybookReport((DateTime)(FromDate.Date ?? DateTime.MinValue), (DateTime)(ToDate.Date ?? DateTime.MinValue), Global.Company.CompanyId);
            db.GenerateReport();

            LoadData(db);

        }
        private void LoadData(RptDayBook DayBook)
        {
            double dailyCreditBalance = 0;
            double dailyDebitBalance = 0;
            double CashBalance = 0;
            DaybookDataGridView.Columns[(int)GridColumn.DESC].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            DaybookDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            List<RptDayBookLineItem> LineItems = (List<RptDayBookLineItem>)DayBook.LineItems;
            if (LineItems != null && LineItems.Count > 0)
            {
                int NewRow = 0;
                EnableButton(true);
                double DailyBalance = DayBook.OpeningBalance.Amount;
                String LastDate = null;
                AddCustomRowBalance(DayBook.OpeningBalance);
                foreach (RptDayBookLineItem LineItem in LineItems)
                {
                    double ZeroAmount = Math.Round(LineItem.Amount, Global.Company.PrimaryCurrency.RoundingPrecision);

                    if (ZeroAmount == 0)
                    {
                        continue;
                    }
                    String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                    if (LastDate != stringLineItemDate)
                    {
                        if (DaybookDataGridView.Rows.Count > 0 && DaybookDataGridView.Rows[0].Index != NewRow)
                        {
                            string FCreditValueString = DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value != null ? DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value.ToString()! : "0";
                            string FdebitValueString = DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value != null ? DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value.ToString()! : "0";
                            dailyCreditBalance += double.TryParse(FCreditValueString, out double FCredit) ? FCredit : 0;
                            dailyDebitBalance += double.TryParse(FdebitValueString, out double FDebit) ? FDebit : 0;

                            NewRow = DaybookDataGridView.Rows.Add();
                            DaybookDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.Control;
                            DaybookDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "Day Total's";
                            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(dailyCreditBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(dailyDebitBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

                            NewRow = DaybookDataGridView.Rows.Add();
                            DaybookDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.ControlLight;
                            DaybookDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.ControlLight;
                            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "Cash Balance";
                            CashBalance = dailyCreditBalance - dailyDebitBalance;
                            if (dailyCreditBalance > dailyDebitBalance)
                            {
                                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            }
                            else
                            {
                                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            }
                        }
                        NewRow = DaybookDataGridView.Rows.Add();
                        DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DATE].Value = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                        LastDate = stringLineItemDate;
                        dailyCreditBalance = 0;
                        dailyDebitBalance = 0;
                    }
                    else
                    {
                        NewRow = DaybookDataGridView.Rows.Add();
                    }
                    string CreditValueString = DaybookDataGridView.Rows[NewRow - 1].Cells[(int)GridColumn.CREDIT].Value != null ? DaybookDataGridView.Rows[NewRow - 1].Cells[(int)GridColumn.CREDIT].Value.ToString() : "0";
                    string debitValueString = DaybookDataGridView.Rows[NewRow - 1].Cells[(int)GridColumn.DEBIT].Value != null ? DaybookDataGridView.Rows[NewRow - 1].Cells[(int)GridColumn.DEBIT].Value.ToString() : "0";
                    dailyCreditBalance += double.TryParse(CreditValueString, out double Credit) ? Credit : 0;
                    dailyDebitBalance += double.TryParse(debitValueString, out double Debit) ? Debit : 0;
                    if (LineItem.CreditOrDebit() == CrDr.CR)
                    {
                        if (LineItem.Patient != null && LineItem.Patient.Id != null)
                        {
                            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value =  (!String.IsNullOrEmpty(LineItem.Description) ? LineItem.Description : "") + Environment.NewLine + "By " + (LineItem.Account != null ? LineItem.Account.DisplayAs : LineItem.Patient.Name);
                            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(LineItem.Amount).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                        }
                        else
                        {
                            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "By " + (LineItem.Account != null ? LineItem.Account.DisplayAs : LineItem.Patient.Name) + (!String.IsNullOrEmpty(LineItem.Description) ? Environment.NewLine + LineItem.Description : "");
                            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(LineItem.Amount).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                        }
                    }
                    else
                    {
                        DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "To " + (LineItem.Account != null ? LineItem.Account.DisplayAs : LineItem.Patient.Name) + Environment.NewLine + LineItem.Description;
                        DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(LineItem.Amount).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    }
                }
                string LCreditValueString = DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value != null ? DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value.ToString() : "0";
                string LdebitValueString = DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value != null ? DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value.ToString() : "0";
                dailyCreditBalance += double.TryParse(LCreditValueString, out double LCredit) ? LCredit : 0;
                dailyDebitBalance += double.TryParse(LdebitValueString, out double LDebit) ? LDebit : 0;

                NewRow = DaybookDataGridView.Rows.Add();
                DaybookDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.Control;
                DaybookDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "Day Total's";
                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(dailyCreditBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(dailyDebitBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

                NewRow = DaybookDataGridView.Rows.Add();
                DaybookDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.ControlLight;
                DaybookDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.ControlLight;
                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "Cash Balance";
                CashBalance = dailyCreditBalance - dailyDebitBalance;
                if (dailyCreditBalance > dailyDebitBalance)
                {
                    DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                }
                else
                {
                    DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                }
                AddTotal(DayBook);
            }
            else if (DayBook.OpeningBalance.Amount != 0)
            {
                AddCustomRowBalance(DayBook.OpeningBalance);
                AddTotal(DayBook);
            }
            else
            {
                ErrorMsg.Text = InformationMsg;
            }
        }

        private void AddTotal(RptDayBook dBook)
        {
            int NewRow = DaybookDataGridView.Rows.Add();
            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "Total";
            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(dBook.TotalCredit).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(dBook.TotalDebit).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            NewRow = DaybookDataGridView.Rows.Add();
            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "Closing Cash Balance";
            if (dBook.TotalCredit - dBook.TotalDebit > 0)
                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(dBook.TotalCredit - dBook.TotalDebit).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            else
                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(dBook.TotalCredit - dBook.TotalDebit).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
        }

        private void AddCustomRowBalance(RptDayBookLineItem LineItem)
        {
            int NewRow = DaybookDataGridView.Rows.Add();
            String OpeningBalanceDate = DateUtils.FormatDate(db.FromDate, Global.Company.DateFormat);
            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DATE].Value = OpeningBalanceDate;
            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = LineItem.Description;
            if (LineItem.CreditOrDebit() == CrDr.CR)
            {
                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(LineItem.Amount).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            }
            else
            {
                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(LineItem.Amount).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            }
        }

        private void LedgerFrmToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private void FormDaybook_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ComboUtils.InitializeCostCenterComboByUserAccess(ComboBoxCostCenter, (long)Global.Company.CompanyId);
                ResetForm();
                if (FormValidate())
                {
                    RunReport();
                    if (ComboBoxCostCenter.Items.Count > 1)
                    {
                        ComboBoxCostCenter.Select();
                    }
                    else
                    {
                        FromDate.Select();
                    }
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void ResetForm()
        {
            DaybookDataGridView.Rows.Clear();
            ErrorMsg.Text = "";
            EnableButton(false);
            if (ComboBoxCostCenter.Items.Count > 1)
            {
                LabelCostCenter.Visible = true;
                ComboBoxCostCenter.Visible = true;
                toolStripSeparatorCostCenter.Visible = true;
                ComboBoxCostCenter.SelectedIndex = 0;
            }
            else
            {
                LabelCostCenter.Visible = false;
                ComboBoxCostCenter.Visible = false;
                toolStripSeparatorCostCenter.Visible = false;
            }
            FromDate.Format = Global.Company.DateFormat;
            ToDate.Format = Global.Company.DateFormat;
            //ToDate.MinDate = Global.getCurrentFiscalYearStartDate();
            //ToDate.MaxDate = Global.getCurrentFiscalYearEndDate();
            //FromDate.MinDate = Global.getCurrentFiscalYearStartDate();
            //FromDate.MaxDate = Global.getCurrentFiscalYearEndDate();
            //FromDate.Date = DateUtils.ReportFromDate(Global.Company.DateFormat, 1);
            FromDate.Date = Global.getTransactionDate().AddDays(-30);
            ToDate.Date = Global.getTransactionDate();
            EnableButton(false);
        }

        private void EnableButton(bool Enable)
        {
            BtnPrint.Enabled = Enable;
            BtnSave.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            //PdfDocument document = new PdfDaybookA4();
            //document.Company = Global.Company;
            //document.PageSize = iTextSharp.text.PageSize.A4;
            //document.Render();

            DayBook DayBook = new DayBook();
            DayBook.ExportToFileOrPrint(db, false);
            Cursor.Current = Cursors.Default;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DayBook DayBook = new DayBook();
            DayBook.ExportToFileOrPrint(db, true);
            Cursor.Current = Cursors.Default;
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            if (ComboBoxCostCenter.Visible)
            {
                ComboBoxCostCenter.Select();
            }
            else
            {
                FromDate.Select();
            }
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
        private void DaybookDataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && (DaybookDataGridView.Rows[e.RowIndex].Cells[(int)GridColumn.DESC].Value == "Day Total's" || DaybookDataGridView.Rows[e.RowIndex].Cells[(int)GridColumn.DESC].Value == "Cash Balance"))
            {
                if (e.ColumnIndex == (int)GridColumn.DESC)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (e.ColumnIndex == (int)GridColumn.DATE)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
            if (e.RowIndex > -1 && (e.RowIndex == DaybookDataGridView.Rows.Count -1 || e.RowIndex == DaybookDataGridView.Rows.Count - 2))
            {
                if (e.ColumnIndex == (int)GridColumn.DATE)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)GridColumn.DESC)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }
        private void LoadDayBookDataReport(DataTable daybookReport, double OpeningBal)
        {
            double dailyCreditBalance = 0;
            double dailyDebitBalance = 0;
            double CashBalance = 0;
            bool isOpeningBalance = true; 

            DaybookDataGridView.Columns[(int)GridColumn.DESC].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            DaybookDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            if (daybookReport != null && daybookReport.Rows.Count > 0)
            {
                int NewRow = 0;
                EnableButton(true);

                AddDayBookCustomRowBalance(OpeningBal, "Opening Balance", true);
                double DailyBalance = OpeningBal;
                string LastDate = null!;

                string LCreditValueString = "0";
                string LDebitValueString = "0";

                foreach (DataRow row in daybookReport.Rows)
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
                    string debitAmount = Math.Abs(Convert.ToDouble(row["Debit"])).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    string creditAmount = Math.Abs(Convert.ToDouble(row["Credit"])).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

                    if (double.Parse(debitAmount) == 0 && double.Parse(creditAmount) == 0)
                    {
                        continue;
                    }
                    if (!isOpeningBalance)
                    {
                        if (LastDate != stringLineItemDate)
                        {
                            if (DaybookDataGridView.Rows.Count > 0 && DaybookDataGridView.Rows[0].Index != NewRow)
                            {
                                string FCreditValueString = DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value != null ? DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value.ToString()! : "0";
                                string FDebitValueString = DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value != null ? DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value.ToString()! : "0";
                                dailyCreditBalance += double.TryParse(FCreditValueString, out double FCredit) ? FCredit : 0;
                                dailyDebitBalance += double.TryParse(FDebitValueString, out double FDebit) ? FDebit : 0;

                                // Add "Day Total's" row
                                NewRow = DaybookDataGridView.Rows.Add();
                                DaybookDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.Control;
                                DaybookDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "Day Total's";
                                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(dailyCreditBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(dailyDebitBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

                                // Add "Cash Balance" row
                                NewRow = DaybookDataGridView.Rows.Add();
                                DaybookDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.ControlLight;
                                DaybookDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.ControlLight;
                                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "Cash Balance";
                                CashBalance = dailyCreditBalance - dailyDebitBalance;
                                if (dailyCreditBalance > dailyDebitBalance)
                                {
                                    DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                                }
                                else
                                {
                                    DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                                }
                            }

                            NewRow = DaybookDataGridView.Rows.Add();
                            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DATE].Value = stringLineItemDate;
                            LastDate = stringLineItemDate;
                            dailyCreditBalance = 0;
                            dailyDebitBalance = 0;
                        }
                        else
                        {
                            NewRow = DaybookDataGridView.Rows.Add();
                        }
                    }

                    // Process credit and debit for each row
                    LCreditValueString = row["Credit"] != DBNull.Value ? row["Credit"].ToString()! : "0";
                    LDebitValueString = row["Debit"] != DBNull.Value ? row["Debit"].ToString()! : "0";
                    dailyCreditBalance += double.TryParse(LCreditValueString, out double Credit) ? Credit : 0;
                    dailyDebitBalance += double.TryParse(LDebitValueString, out double Debit) ? Debit : 0;

                    if (row["Type"].ToString() == "CR")
                    {
                        DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "By " + (!string.IsNullOrEmpty(row["Description"].ToString()) ? Environment.NewLine + row["Description"] : "");
                        DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(Convert.ToDouble(row["Credit"])).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    }
                    else if (row["Type"].ToString() == "DR")
                    {
                        DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "To " + Environment.NewLine + row["Description"];
                        DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(Convert.ToDouble(row["Debit"])).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    }

                    isOpeningBalance = false; // Now we are processing actual transactions
                }

                // Final Day Total's and Cash Balance for the last date after processing all rows
                dailyCreditBalance += double.TryParse(LCreditValueString, out double LCredit) ? LCredit : 0;
                dailyDebitBalance += double.TryParse(LDebitValueString, out double LDebit) ? LDebit : 0;

                // Add totals and balance at the end of the loop (after all rows processed)
                NewRow = DaybookDataGridView.Rows.Add();
                DaybookDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.Control;
                DaybookDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "Day Total's";
                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(dailyCreditBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(dailyDebitBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

                NewRow = DaybookDataGridView.Rows.Add();
                DaybookDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.ControlLight;
                DaybookDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.ControlLight;
                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "Cash Balance";
                CashBalance = dailyCreditBalance - dailyDebitBalance;
                if (dailyCreditBalance > dailyDebitBalance)
                {
                    DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                }
                else
                {
                    DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                }

                AddDayBookTotal(daybookReport);
            }
            else
            {
                ErrorMsg.Text = InformationMsg;
            }
        }

        private void LoadDayBookDataReport30092024(DataTable daybookReport, double OpeningBal)
        {
            double dailyCreditBalance = 0;
            double dailyDebitBalance = 0;
            double CashBalance = 0;
            bool isFirstRow = true; // Flag to track if we're processing the first row (Opening Balance)

            DaybookDataGridView.Columns[(int)GridColumn.DESC].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            DaybookDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            if (daybookReport != null && daybookReport.Rows.Count > 0)
            {
                int NewRow = 0;
                EnableButton(true);

                // Adding the opening balance row
                AddDayBookCustomRowBalance(OpeningBal, "Opening Balance", true);
                double DailyBalance = OpeningBal;
                string LastDate = null!;

                foreach (DataRow row in daybookReport.Rows)
                {
                    string stringLineItemDate;

                    if (row["Date"] != DBNull.Value)
                    {
                        stringLineItemDate = Convert.ToDateTime(row["Date"]).ToString(Global.Company.DateFormat);
                    }
                    else
                    {
                        stringLineItemDate = ""; // or some default value if the date is missing
                    }

                    if (LastDate != stringLineItemDate && !isFirstRow)
                    {
                        // Adding "Day Total's" and "Cash Balance" only after the first (Opening Balance) row
                        if (DaybookDataGridView.Rows.Count > 0 && DaybookDataGridView.Rows[0].Index != NewRow)
                        {
                            string FCreditValueString = DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value != null ? DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value.ToString() : "0";
                            string FDebitValueString = DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value != null ? DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value.ToString() : "0";
                            dailyCreditBalance += double.TryParse(FCreditValueString, out double FCredit) ? FCredit : 0;
                            dailyDebitBalance += double.TryParse(FDebitValueString, out double FDebit) ? FDebit : 0;

                            // Adding day totals
                            NewRow = DaybookDataGridView.Rows.Add();
                            DaybookDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.Control;
                            DaybookDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "Day Total's";
                            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(dailyCreditBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(dailyDebitBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

                            // Adding cash balance
                            NewRow = DaybookDataGridView.Rows.Add();
                            DaybookDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.ControlLight;
                            DaybookDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.ControlLight;
                            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "Cash Balance";
                            CashBalance = dailyCreditBalance - dailyDebitBalance;
                            if (dailyCreditBalance > dailyDebitBalance)
                            {
                                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            }
                            else
                            {
                                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            }
                        }
                        // Reset balances for the new day
                        dailyCreditBalance = 0;
                        dailyDebitBalance = 0;
                    }

                    // Process daybook entries
                    NewRow = DaybookDataGridView.Rows.Add();
                    DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DATE].Value = stringLineItemDate;
                    LastDate = stringLineItemDate;

                    string CreditValueString = row["Credit"] != DBNull.Value ? row["Credit"].ToString() : "0";
                    string debitValueString = row["Debit"] != DBNull.Value ? row["Debit"].ToString() : "0";
                    dailyCreditBalance += double.TryParse(CreditValueString, out double Credit) ? Credit : 0;
                    dailyDebitBalance += double.TryParse(debitValueString, out double Debit) ? Debit : 0;

                    if (row["Type"].ToString() == "CR")
                    {
                        DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "By " + (!string.IsNullOrEmpty(row["Description"].ToString()) ? Environment.NewLine + row["Description"] : "");
                        DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(Convert.ToDouble(row["Credit"])).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    }
                    else if (row["Type"].ToString() == "DR")
                    {
                        DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "To " + Environment.NewLine + row["Description"];
                        DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(Convert.ToDouble(row["Debit"])).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    }

                    // Set flag to false after processing the opening balance row
                    isFirstRow = false;
                }

                // Finally, add the total row at the end
                NewRow = DaybookDataGridView.Rows.Add();
                DaybookDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.Control;
                DaybookDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "Day Total's";
                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(dailyCreditBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(dailyDebitBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

                // Add cash balance at the end
                NewRow = DaybookDataGridView.Rows.Add();
                DaybookDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.ControlLight;
                DaybookDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.ControlLight;
                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "Cash Balance";
                CashBalance = dailyCreditBalance - dailyDebitBalance;
                if (dailyCreditBalance > dailyDebitBalance)
                {
                    DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                }
                else
                {
                    DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                }

                // Add any additional total or summary rows here
                AddDayBookTotal(daybookReport);
            }
            else
            {
                ErrorMsg.Text = InformationMsg;
            }
        }
        private void LoadDayBookDataReport30092024Before(DataTable daybookReport, double OpeningBal)
        {
            double dailyCreditBalance = 0;
            double dailyDebitBalance = 0;
            double CashBalance = 0;

            // Assuming that the DataTable columns are already defined with appropriate data (Date, Credit, Debit, Description, etc.)
            DaybookDataGridView.Columns[(int)GridColumn.DESC].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            DaybookDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;

            if (daybookReport != null && daybookReport.Rows.Count > 0)
            {
                int NewRow = 0;
                EnableButton(true);

                // Assuming that the opening balance is part of the DataTable in a special row or available separately
                DataRow openingBalanceRow = daybookReport.Rows[0]; // Example: first row is OpeningBalance
                double openingBalance = OpeningBal; //Convert.ToDouble(openingBalanceRow["OpeningBalanceAmount"]); // Adjust this as per DataTable structure
                AddDayBookCustomRowBalance(openingBalance, "Opening Balance", true);  // or false for debit
                double DailyBalance = openingBalance;
                string LastDate = null!;

                //for (int i = 0; i < daybookReport.Rows.Count; i++)
                //{

                //}



                foreach (DataRow row in daybookReport.Rows)
                {
                    string stringLineItemDate;

                    if (row["Date"] != DBNull.Value)
                    {
                        stringLineItemDate = Convert.ToDateTime(row["Date"]).ToString(Global.Company.DateFormat);
                    }
                    else
                    {
                        stringLineItemDate = ""; // or some default value if the date is missing
                    }
                    if (LastDate != stringLineItemDate)
                    {
                        if (DaybookDataGridView.Rows.Count > 0 && DaybookDataGridView.Rows[0].Index != NewRow)
                        {
                            string FCreditValueString = DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value != null ? DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value.ToString() : "0";
                            string FDebitValueString = DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value != null ? DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value.ToString() : "0";
                            dailyCreditBalance += double.TryParse(FCreditValueString, out double FCredit) ? FCredit : 0;
                            dailyDebitBalance += double.TryParse(FDebitValueString, out double FDebit) ? FDebit : 0;

                            NewRow = DaybookDataGridView.Rows.Add();
                            DaybookDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.Control;
                            DaybookDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "Day Total's";
                            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(dailyCreditBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(dailyDebitBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

                            NewRow = DaybookDataGridView.Rows.Add();
                            DaybookDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.ControlLight;
                            DaybookDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.ControlLight;
                            DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "Cash Balance";
                            CashBalance = dailyCreditBalance - dailyDebitBalance;
                            if (dailyCreditBalance > dailyDebitBalance)
                            {
                                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            }
                            else
                            {
                                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            }
                        }
                        NewRow = DaybookDataGridView.Rows.Add();
                        DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DATE].Value = stringLineItemDate;
                        LastDate = stringLineItemDate;
                        dailyCreditBalance = 0;
                        dailyDebitBalance = 0;
                    }
                    else
                    {
                        NewRow = DaybookDataGridView.Rows.Add();
                    }

                    string CreditValueString = row["Credit"] != DBNull.Value ? row["Credit"].ToString() : "0";
                    string debitValueString = row["Debit"] != DBNull.Value ? row["Debit"].ToString() : "0";
                    dailyCreditBalance += double.TryParse(CreditValueString, out double Credit) ? Credit : 0;
                    dailyDebitBalance += double.TryParse(debitValueString, out double Debit) ? Debit : 0;

                    if (row["Type"].ToString() == "CR") // Assuming there's a column to identify Credit/Debit
                    {
                        DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "By " + /* row["AccountName"] + */ (!string.IsNullOrEmpty(row["Description"].ToString()) ? Environment.NewLine + row["Description"] : "");
                        DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(Convert.ToDouble(row["Credit"])).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    }
                    else if (row["Type"].ToString() == "DR")
                    {
                        DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "To " + /* row["AccountName"] + */ Environment.NewLine + row["Description"];
                        DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(Convert.ToDouble(row["Debit"])).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    }
                }
                string LCreditValueString = "0";
                string LDebitValueString = "0";
                if (daybookReport.Rows.Count > 0)
                {
                    NewRow = 0; // Or however you determine which row to access
                    if (NewRow < daybookReport.Rows.Count)
                    {
                        LCreditValueString = daybookReport.Rows[NewRow]["Credit"] != DBNull.Value ? daybookReport.Rows[NewRow]["Credit"].ToString()! : "0";
                        // Proceed with your logic
                    }
                    else
                    {
                        // Handle case where NewRow is out of bounds
                        LCreditValueString = "0"; // Default value or error handling
                    }
                }


                //string LCreditValueString = daybookReport.Rows[NewRow]["Credit"] != DBNull.Value ? daybookReport.Rows[NewRow]["Credit"].ToString() : "0";
                //string LDebitValueString = daybookReport.Rows[NewRow]["Debit"] != DBNull.Value ? daybookReport.Rows[NewRow]["Debit"].ToString() : "0";
                dailyCreditBalance += double.TryParse(LCreditValueString, out double LCredit) ? LCredit : 0;
                dailyDebitBalance += double.TryParse(LDebitValueString, out double LDebit) ? LDebit : 0;

                NewRow = DaybookDataGridView.Rows.Add();
                DaybookDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.Control;
                DaybookDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "Day Total's";
                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(dailyCreditBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(dailyDebitBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

                NewRow = DaybookDataGridView.Rows.Add();
                DaybookDataGridView.Rows[NewRow].DefaultCellStyle.BackColor = SystemColors.ControlLight;
                DaybookDataGridView.Rows[NewRow].DefaultCellStyle.SelectionBackColor = SystemColors.ControlLight;
                DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DESC].Value = "Cash Balance";
                CashBalance = dailyCreditBalance - dailyDebitBalance;
                if (dailyCreditBalance > dailyDebitBalance)
                {
                    DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                }
                else
                {
                    DaybookDataGridView.Rows[NewRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(CashBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                }

                // Assuming AddTotal accepts a DataTable or can work with its structure
                AddDayBookTotal(daybookReport);
            }
            else
            {
                ErrorMsg.Text = InformationMsg;
            }
        }

        private void AddDayBookTotal(DataTable daybookReport)
        {
            // Initialize variables to store total credit and debit
            double totalCredit = 0;
            double totalDebit = 0;

            // Loop through the DataTable and sum up the credit and debit values
            foreach (DataRow row in daybookReport.Rows)
            {
                // Assuming the columns for credit and debit are named "Credit" and "Debit"
                double credit = row["Credit"] != DBNull.Value ? Convert.ToDouble(row["Credit"]) : 0;
                double debit = row["Debit"] != DBNull.Value ? Convert.ToDouble(row["Debit"]) : 0;

                totalCredit += credit;
                totalDebit += debit;
            }

            // Add "Total" row to the DataGridView
            int newRow = DaybookDataGridView.Rows.Add();
            DaybookDataGridView.Rows[newRow].Cells[(int)GridColumn.DESC].Value = "Total";
            DaybookDataGridView.Rows[newRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(totalCredit).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            DaybookDataGridView.Rows[newRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(totalDebit).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

            // Add "Closing Cash Balance" row
            newRow = DaybookDataGridView.Rows.Add();
            DaybookDataGridView.Rows[newRow].Cells[(int)GridColumn.DESC].Value = "Closing Cash Balance";

            double closingBalance = totalCredit - totalDebit;
            if (closingBalance > 0)
                DaybookDataGridView.Rows[newRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(closingBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            else
                DaybookDataGridView.Rows[newRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(closingBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
        }
        private void AddDayBookCustomRowBalance(double balance, string description, bool isCredit)
        {
            int newRow = DaybookDataGridView.Rows.Add();

            // Set the description
            DaybookDataGridView.Rows[newRow].Cells[(int)GridColumn.DESC].Value = description;

            // Add the balance to either the credit or debit column based on isCredit
            if (isCredit)
            {
                DaybookDataGridView.Rows[newRow].Cells[(int)GridColumn.CREDIT].Value = Math.Abs(balance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            }
            else
            {
                DaybookDataGridView.Rows[newRow].Cells[(int)GridColumn.DEBIT].Value = Math.Abs(balance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            }
        }
    }
}
