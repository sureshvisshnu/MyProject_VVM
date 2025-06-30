using fa.api.Accounting;
using fa.api.utils;
using fa.context;
using fa.libraries.utils;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.Catalog;
using fa.model.OrderManagement;
using fa.views.utils.Report.Hms;
using Fa.report.accounting.master;
using Fa.views.utils.Report.Account;
using Fa.views.utils.Report.Account.transaction;
using FADataAccessLibrary.report.accounting.TrialBalance;
using FADataAccessLibrary.report.Stock;
using SharpCompress;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.reports.account.transaction.trialbalance
{
    enum TrialBalanceGridColumn
    {
        ACCOUNT_NAME, DEBIT, CREDIT, ID
    }

    public partial class FormTrialBalance : Form
    {
        RptTrialBalance RptTrialBalance = null!;
        public DateTime? CompanyAccountingStartDate { get; set; }
        public static string SearchErrorMsg = "No Entry Found!";
        public FormTrialBalance()
        {
            InitializeComponent();
        }
        double oldamount = 0.00;
        private void TrialBalanceGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex > -1 && TrialBalanceGrid.Rows[e.RowIndex].Cells[(int)TrialBalanceGridColumn.ACCOUNT_NAME].Value != null &&
                TrialBalanceGrid.Rows[e.RowIndex].Cells[(int)TrialBalanceGridColumn.ACCOUNT_NAME].Value.ToString() == "OpeningStockAmount")
            {
                if (e.ColumnIndex == (int)TrialBalanceGridColumn.DEBIT || e.ColumnIndex == (int)TrialBalanceGridColumn.CREDIT)
                {
                    oldamount = TrialBalanceGrid.CurrentRow.Cells[e.ColumnIndex].Value != null ? Double.Parse(TrialBalanceGrid.CurrentRow.Cells[e.ColumnIndex].Value.ToString()) : 0.00;
                }
            }
        }

        private void TrialBalanceGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && TrialBalanceGrid.Rows[e.RowIndex].Cells[(int)TrialBalanceGridColumn.ACCOUNT_NAME].Value != null &&
                TrialBalanceGrid.Rows[e.RowIndex].Cells[(int)TrialBalanceGridColumn.ACCOUNT_NAME].Value.ToString() == "OpeningStockAmount")
            {
                double NewAmount = 0.00;
                double difference = 0.00;
                double CurrentTotal = TrialBalanceGrid.Rows[TrialBalanceGrid.Rows.Count - 1].Cells[e.ColumnIndex].Value != null ? double.Parse(TrialBalanceGrid.Rows[TrialBalanceGrid.Rows.Count - 1].Cells[e.ColumnIndex].Value.ToString()) : 0.00;
                if (e.ColumnIndex == (int)TrialBalanceGridColumn.DEBIT || e.ColumnIndex == (int)TrialBalanceGridColumn.CREDIT)
                {
                    NewAmount = TrialBalanceGrid.CurrentRow.Cells[e.ColumnIndex].Value != null ? Double.Parse(TrialBalanceGrid.CurrentRow.Cells[e.ColumnIndex].Value.ToString()) : 0.00;
                    TrialBalanceGrid.CurrentRow.Cells[e.ColumnIndex].Value = NewAmount.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                }
                if (oldamount > NewAmount)
                {
                    difference = oldamount - NewAmount;
                    TrialBalanceGrid.Rows[TrialBalanceGrid.Rows.Count - 1].Cells[e.ColumnIndex].Value = (CurrentTotal - difference).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                }
                else if (NewAmount > oldamount)
                {
                    difference = NewAmount - oldamount;
                    TrialBalanceGrid.Rows[TrialBalanceGrid.Rows.Count - 1].Cells[e.ColumnIndex].Value = (CurrentTotal + difference).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

                }
            }
        }

        private void TrialBalanceGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (TrialBalanceGrid.Rows[e.RowIndex].Cells[(int)TrialBalanceGridColumn.ACCOUNT_NAME].Value != null &&
                TrialBalanceGrid.Rows[e.RowIndex].Cells[(int)TrialBalanceGridColumn.ACCOUNT_NAME].Value.ToString() == "OpeningStockAmount")
                {
                    TrialBalanceGrid.ReadOnly = false;
                    TrialBalanceGrid.CurrentRow.Cells[(int)TrialBalanceGridColumn.ACCOUNT_NAME].ReadOnly = true;
                    TrialBalanceGrid.CurrentRow.Cells[(int)TrialBalanceGridColumn.CREDIT].ReadOnly = false;
                    TrialBalanceGrid.CurrentRow.Cells[(int)TrialBalanceGridColumn.DEBIT].ReadOnly = false;
                }
                else
                {
                    TrialBalanceGrid.CurrentRow.Cells[(int)TrialBalanceGridColumn.ACCOUNT_NAME].ReadOnly = true;
                    TrialBalanceGrid.CurrentRow.Cells[(int)TrialBalanceGridColumn.CREDIT].ReadOnly = true;
                    TrialBalanceGrid.CurrentRow.Cells[(int)TrialBalanceGridColumn.DEBIT].ReadOnly = true;
                }
            }
        }
        private void TrialBalanceGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        private void BtnRunReport_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            RptTrialBalance = new RptTrialBalance();
            RptTrialBalance.Company = Global.Company;
            GetCompanyStartYear();
            RptTrialBalance.FromDate = (DateTime)CompanyAccountingStartDate!;
            RptTrialBalance.ToDate = (DateTime)ToDate.Date!;
            RptTrialBalance.GenerateReport();
            EnableButton(false);
            ErrorMsg.Text = string.Empty;
            TrialBalanceGrid.Rows.Clear();
            int row = 0;
            string AccountType = string.Empty;
            double CreitTotal = 0.00;
            double DebitTotal = 0.00;
            if (!(RptTrialBalance.LineItems.Count == 0 && RptTrialBalance.TotalOpeningStockValue == 0))
            {
                row = TrialBalanceGrid.Rows.Add();
                TrialBalanceGrid.Rows[row].Cells[(int)TrialBalanceGridColumn.ACCOUNT_NAME].Value = "OpeningStockAmount";
                if (RptTrialBalance.TotalOpeningStockValue < 0)
                {
                    TrialBalanceGrid.Rows[row].Cells[(int)TrialBalanceGridColumn.CREDIT].Value = Math.Abs(RptTrialBalance.TotalOpeningStockValue).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    CreitTotal += Math.Abs(RptTrialBalance.TotalOpeningStockValue);
                }
                else
                {
                    TrialBalanceGrid.Rows[row].Cells[(int)TrialBalanceGridColumn.DEBIT].Value = Math.Abs(RptTrialBalance.TotalOpeningStockValue).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    DebitTotal += Math.Abs(RptTrialBalance.TotalOpeningStockValue);
                }
            }
            if (RptTrialBalance.LineItems.Count > 0)
            {
                string GroupName = String.Empty;
                double GroupCreitTotal = 0.00;
                double GroupDebitTotal = 0.00;
                int Count = 1;
                foreach (RptTrialBalanceLineItem LineItem in RptTrialBalance.LineItems.OrderBy(x => x.GroupName))
                {
                    if (GroupName != LineItem.GroupName.ToString() || RptTrialBalance.LineItems.Count == Count)
                    {
                        if (Count != 1 && RptTrialBalance.LineItems.Count != Count)
                        {
                            row = TrialBalanceGrid.Rows.Add();
                            TrialBalanceGrid.Rows[row].Cells[(int)TrialBalanceGridColumn.ACCOUNT_NAME].Value = GroupName;
                            TrialBalanceGrid.Rows[row].Cells[(int)TrialBalanceGridColumn.CREDIT].Value = GroupCreitTotal == 0 ? string.Empty : Math.Abs(GroupCreitTotal).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            TrialBalanceGrid.Rows[row].Cells[(int)TrialBalanceGridColumn.DEBIT].Value = GroupDebitTotal == 0 ?string.Empty: Math.Abs(GroupDebitTotal).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                        }

                        GroupCreitTotal = 0.00;
                        GroupDebitTotal = 0.00;
                        GroupName = LineItem.GroupName.ToString();
                    }
                    if (LineItem.CreditOrDebit() == report.common.CrDr.CR)
                    {
                        CreitTotal += Math.Abs(LineItem.Amount);
                        GroupCreitTotal += Math.Abs(LineItem.Amount);
                    }
                    else
                    {
                        DebitTotal += Math.Abs(LineItem.Amount);
                        GroupDebitTotal += Math.Abs(LineItem.Amount);
                    }
                    if (RptTrialBalance.LineItems.Count == Count)
                    {
                        row = TrialBalanceGrid.Rows.Add();
                        TrialBalanceGrid.Rows[row].Cells[(int)TrialBalanceGridColumn.ACCOUNT_NAME].Value = GroupName;
                        TrialBalanceGrid.Rows[row].Cells[(int)TrialBalanceGridColumn.CREDIT].Value = Math.Abs(GroupCreitTotal).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                        TrialBalanceGrid.Rows[row].Cells[(int)TrialBalanceGridColumn.DEBIT].Value = Math.Abs(GroupDebitTotal).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    }
                    Count++;
                }
                //Balance Printing
                row = TrialBalanceGrid.Rows.Add();
                TrialBalanceGrid.Rows[row].Cells[(int)TrialBalanceGridColumn.ACCOUNT_NAME].Value = "Total";
                TrialBalanceGrid.Rows[row].Cells[(int)TrialBalanceGridColumn.CREDIT].Value = Math.Abs(CreitTotal).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                TrialBalanceGrid.Rows[row].Cells[(int)TrialBalanceGridColumn.DEBIT].Value = Math.Abs(DebitTotal).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                EnableButton(true);
            }
            else if (RptTrialBalance.LineItems.Count == 0 && TrialBalanceGrid.Rows.Count != 0)
            {
                row = TrialBalanceGrid.Rows.Add();
                TrialBalanceGrid.Rows[row].Cells[(int)TrialBalanceGridColumn.ACCOUNT_NAME].Value = "Total";
                TrialBalanceGrid.Rows[row].Cells[(int)TrialBalanceGridColumn.CREDIT].Value = Math.Abs(CreitTotal).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                TrialBalanceGrid.Rows[row].Cells[(int)TrialBalanceGridColumn.DEBIT].Value = Math.Abs(DebitTotal).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                EnableButton(true);
            }
            else
            {

                ErrorMsg.Text = SearchErrorMsg;
            }
            Cursor.Current = Cursors.Default;

        }
        private void GetCompanyStartYear()
        {
            Company Company = CompanyManager.Instance.GetCompanyForModel(Global.Company.CompanyId);
            if (Company != null)
            {
                CompanyAccountingStartDate = Company.CreatedDate;
            }
        }

        private void FormTrialBalance_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ComboUtils.InitializeCostCenterComboByUserAccess(ComboBoxCostCenter, (long)Global.Company.CompanyId);
                ResetForm();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void ResetForm()
        {
            TrialBalanceGrid.Rows.Clear();
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

            FromDate.Format = Global.Company.DateFormat;
            FromDate.Date = Global.getTransactionDate().AddDays(-30);
            ToDate.Format = Global.Company.DateFormat;
            ToDate.Date = Global.getTransactionDate();
        }
        private void EnableButton(bool Enable)
        {
            BtnPrint.Enabled = Enable;
            BtnSave.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            Cursor.Current = Cursors.Default;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DateTime AcountingStartDate = Global.getCurrentFiscalYearStartDate();
            DateTime AcountingEndDate = (DateTime)ToDate.Date!;
            TrialBalanceReportSavePrint TBReportSavePrint = new TrialBalanceReportSavePrint();
            TBReportSavePrint.ExportOrPrintToFile( TrialBalanceGrid, AcountingStartDate, AcountingEndDate, "pdf", false);
            Cursor.Current = Cursors.Default;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DateTime AcountingStartDate = Global.getCurrentFiscalYearStartDate();
            DateTime AcountingEndDate = (DateTime)ToDate.Date!;
            TrialBalanceReportSavePrint TBReportSavePrint = new TrialBalanceReportSavePrint();
            TBReportSavePrint.ExportOrPrintToFile(TrialBalanceGrid, AcountingStartDate, AcountingEndDate, "pdf", true);
            Cursor.Current = Cursors.Default;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                BtnReset.PerformClick();
                return true;
            }
            if (keyData == Keys.F8)
            {
                BtnSave.PerformClick();
                return true;
            }
            if (keyData == Keys.F9)
            {
                BtnPrint.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


    }
}
