using fa.api.utils;
using fa.model.Common;
using fa.report.accounting.transcation;
using fa.views.utils.Report.Account.transaction;
using Fa.reports.Hms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.reports.account.transaction
{
    enum TransactionTableColumn
    {
        DATE, REF, ACCOUNT, DESC, AMOUNT
    }
    public partial class FormAccountTransactions : Form
    {
        public static string EnterValidDateErrorMsg = "Enter valid Date";
        public static string SelectTransactionErrorMsg = "Please select Transaction.";
        RbtTransaction RbtTransaction = null!;
        public FormAccountTransactions()
        {
            InitializeComponent();
        }
        private void FormAccountTransactions_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                ComboBoxTransaction.Select();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void BtnRunReport_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                GridViewTransaction.Rows.Clear();
                EnableButton(false);
                if (FormValidate())
                {
                    LoadTransaction();
                }
            }
            catch (Exception ex)
            {
                TransactionErrorMsg.Text = "Error fetching ledger (Error: " + (ex.InnerException != null ? ex.InnerException.Message : ex.Message) + ")";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void LoadTransaction()
        {
            RbtTransaction = new RbtTransaction();
            RbtTransaction.FromDate = (DateTime)FromDate.Date!;
            RbtTransaction.ToDate = (DateTime)ToDate.Date!;
            RbtTransaction.Company = Global.Company;
            RbtTransaction.Transaction = ComboBoxTransaction.Text;
            RbtTransaction.GenerateReport();

            if (RbtTransaction.TransactionLineItems != null && RbtTransaction.TransactionLineItems.Count > 0)
            {
                EnableButton(true);
                Currency Currency = api.Accounting.CurrencyManager.Instance.GetCurrencyById((long)Global.Company.PrimaryCurrencyId!);
                GridViewTransaction.Rows.Clear();

                int j = 2;
                Color[] RowColor = new Color[2] { Color.White, Color.WhiteSmoke };
                int row = -1;

                decimal totalDr = 0m, totalCr = 0m;

                foreach (TransactionLineItem LineItem in RbtTransaction.TransactionLineItems.OrderBy(x => x.Date))
                {
                    row = GridViewTransaction.Rows.Add();
                    GridViewTransaction.Rows[row].DefaultCellStyle.BackColor = RowColor[j % 2];
                    GridViewTransaction.Rows[row].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];

                    GridViewTransaction.Rows[row].Cells[(int)TransactionTableColumn.DATE].Value = LineItem.Date.ToString(Global.Company.DateFormat);
                    GridViewTransaction.Rows[row].Cells[(int)TransactionTableColumn.REF].Value = LineItem.Reference;
                    GridViewTransaction.Rows[row].Cells[(int)TransactionTableColumn.ACCOUNT].Value = LineItem.Account?.Name ?? "";
                    GridViewTransaction.Rows[row].Cells[(int)TransactionTableColumn.DESC].Value = LineItem.Description;

                    if (RbtTransaction.Transaction == "Journal")
                    {
                        decimal amount = (decimal)LineItem.Amount; 

                        if (amount < 0)
                        {
                            totalCr += Math.Abs(amount); 
                            GridViewTransaction.Rows[row].Cells[(int)TransactionTableColumn.AMOUNT].Value =
                                $"{Math.Abs(amount).ToString(Currency.CurrencyFormat)} CR";
                        }
                        else
                        {
                            totalDr += amount; 
                            GridViewTransaction.Rows[row].Cells[(int)TransactionTableColumn.AMOUNT].Value =
                                $"{amount.ToString(Currency.CurrencyFormat)} DR";
                        }
                    }
                    else
                    {
                        GridViewTransaction.Rows[row].Cells[(int)TransactionTableColumn.AMOUNT].Value =
                            LineItem.Amount.ToString(Currency.CurrencyFormat);
                    }

                    j++;
                }

                if (RbtTransaction.Transaction == "Journal")
                {
                    row = GridViewTransaction.Rows.Add();
                    GridViewTransaction.Rows[row].DefaultCellStyle.BackColor = RowColor[j % 2];
                    GridViewTransaction.Rows[row].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                    GridViewTransaction.Rows[row].Cells[(int)TransactionTableColumn.DESC].Value = "Total DR";
                    GridViewTransaction.Rows[row].Cells[(int)TransactionTableColumn.AMOUNT].Value =
                        totalDr.ToString(Currency.CurrencyFormat) + " DR";

                    row = GridViewTransaction.Rows.Add();
                    GridViewTransaction.Rows[row].DefaultCellStyle.BackColor = RowColor[j % 2];
                    GridViewTransaction.Rows[row].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                    GridViewTransaction.Rows[row].Cells[(int)TransactionTableColumn.DESC].Value = "Total CR";
                    GridViewTransaction.Rows[row].Cells[(int)TransactionTableColumn.AMOUNT].Value =
                        totalCr.ToString(Currency.CurrencyFormat) + " CR";
                }
                else
                {
                    row = GridViewTransaction.Rows.Add();
                    GridViewTransaction.Rows[row].DefaultCellStyle.BackColor = RowColor[j % 2];
                    GridViewTransaction.Rows[row].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                    GridViewTransaction.Rows[row].Cells[(int)TransactionTableColumn.DESC].Value = "Total";
                    GridViewTransaction.Rows[row].Cells[(int)TransactionTableColumn.AMOUNT].Value =
                        RbtTransaction.Total.ToString(Currency.CurrencyFormat);
                }
            }
            else
            {
                TransactionErrorMsg.Text = "No Record Found..";
            }
        }


        private bool FormValidate()
        {
            TransactionErrorMsg.Text = "";
            if (ComboBoxTransaction.SelectedIndex < 0)
            {
                TransactionErrorMsg.Text = SelectTransactionErrorMsg;
                ComboBoxTransaction.Select();
                return false;
            }
            if (FromDate.Date == null || !DateUtils.ValidDate(((DateTime)FromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                TransactionErrorMsg.Text = EnterValidDateErrorMsg;
                FromDate.Focus();
                return false;
            }
            if (ToDate.Date == null || !DateUtils.ValidDate(((DateTime)ToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                TransactionErrorMsg.Text = EnterValidDateErrorMsg;
                ToDate.Focus();
                return false;
            }
            return true;
        }
        private void EnableButton(bool Enable)
        {
            BtnPrint.Enabled = Enable;
            BtnSave.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
        }
        private void ResetForm()
        {
            GridViewTransaction.Rows.Clear();
            TransactionErrorMsg.Text = "";
            EnableButton(false);
            ComboBoxTransaction.SelectedIndex = -1;
            FromDate.Format = Global.Company.DateFormat;
            ToDate.Format = Global.Company.DateFormat;
            //ToDate.MinDate = Global.getCurrentFiscalYearStartDate();
            //ToDate.MaxDate = Global.getCurrentFiscalYearEndDate();
            //FromDate.MinDate = Global.getCurrentFiscalYearStartDate();
            //FromDate.MaxDate = Global.getCurrentFiscalYearEndDate();
            //FromDate.Date = DateUtils.ReportFromDate(Global.Company.DateFormat, 30);
            FromDate.Date = Global.getTransactionDate().AddDays(-30);
            ToDate.Date = Global.getTransactionDate();
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

        private void BtnReset_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            ComboBoxTransaction.Select();
            Cursor.Current = Cursors.Default;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Transaction Transaction = new Transaction();
            Transaction.ExportToFileOrPrint(RbtTransaction, false);
            //TransactionUsingHtml TransactionUsingHtml = new TransactionUsingHtml();
            //TransactionUsingHtml.LaserPrint(RbtTransaction, false);
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Transaction Transaction = new Transaction();
            Transaction.ExportToFileOrPrint(RbtTransaction, true);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GridViewTransaction_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewTransaction.Rows[e.RowIndex].Cells[(int)TransactionTableColumn.DATE].Value == null)
            {
                if (e.ColumnIndex == (int)TransactionTableColumn.DATE || e.ColumnIndex == (int)TransactionTableColumn.REF)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
        }
                
        private void ComboBoxTransaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GridViewTransaction.Rows.Count > 0)
            {
                GridViewTransaction.Rows.Clear();
            }
        }
    }
}
