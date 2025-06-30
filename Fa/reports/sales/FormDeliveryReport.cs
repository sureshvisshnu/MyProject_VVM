using fa.api.utils;
using fa.libraries.utils;
using fa.report.catalog;
using fa.report.Inventory;
using fa.reports.catalog;
using fa.reports.Inventory;
using FADataAccessLibrary.report.sales;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fa.report.sales;

using Fa.views.utils.Report.Sale;
using fa.views.controls.grid;
using fa;
using Global = fa.Global;

namespace Fa.reports.sales
{
    enum DeliveryReportTableColumn
    {
        SNO, DELIVERYDATE, NAME, AMOUNT, PAYMENTTYPE, STATUS
    }
    public partial class FormDeliveryReport : Form
    {
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string InformationMsg = "No Information Found..!";

        DeliveryReport DeliveryReport = null;

        public FormDeliveryReport()
        {
            InitializeComponent();
        }
        private void EnableButtons(bool Enable)
        {
            BtnSave.Enabled = Enable;
            BtnPrint.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            ToolStripBtnsave.Enabled = Enable;
        }
        private void ResetForm()
        {
            this.Text = "Delivery Report";

            DeliveryReportGridView.Rows.Clear();
            
            DeliveryReportErrorMsg.Text = "";
            EnableButtons(false);

            DeliveryReportFromDate.Format = fa.Global.Company.DateFormat;
            DeliveryReportFromDate.Date = fa.Global.getTransactionDate().AddDays(-30);
            DeliveryReportToDate.Format = fa.Global.Company.DateFormat;
            DeliveryReportToDate.Date = fa.Global.getTransactionDate();
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)DeliveryReportGridView.Columns["Amount"];
            if (int.TryParse(fa.Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
        }
        private bool FormValidate()
        {
            DeliveryReportErrorMsg.Text = "";
            if (DeliveryReportFromDate.Date == null || !DateUtils.ValidDate(((DateTime)DeliveryReportFromDate.Date).ToString(fa.Global.Company.DateFormat), fa.Global.Company.DateFormat))
            {
                DeliveryReportErrorMsg.Text = EnterValidDateErrorMsg;
                DeliveryReportFromDate.Focus();
                return false;
            }
            if (DeliveryReportToDate.Date == null || !DateUtils.ValidDate(((DateTime)DeliveryReportToDate.Date).ToString(fa.Global.Company.DateFormat), fa.Global.Company.DateFormat))
            {
                DeliveryReportErrorMsg.Text = EnterValidDateErrorMsg;
                DeliveryReportToDate.Focus();
                return false;
            }

            return true;
        }
        private void BtnGo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                DeliveryReportGridView.Rows.Clear();
                EnableButtons(false);
                if (FormValidate())
                {
                    LoadDeliveryReport();
                }
            }
            catch (Exception ex)
            {
                DeliveryReportErrorMsg.Text = "Error fetching Stock (Error:" + ex.InnerException.Message + ")";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void LoadDeliveryReport()
        {
            DeliveryReport = new DeliveryReport();
            DeliveryReport.Company = fa.Global.Company;
            DeliveryReport.CurrentDate = fa.Global.getTransactionDate();
            DeliveryReport.FromDate = (DateTime)DeliveryReportFromDate.Date;
            DeliveryReport.ToDate = (DateTime)DeliveryReportToDate.Date;
            DeliveryReport.CompanyId = fa.Global.Company.CompanyId;
            DeliveryReport.DeliveryFromDate = (DateTime)DeliveryReportFromDate.Date;
            DeliveryReport.DeliveryToDate = (DateTime)DeliveryReportToDate.Date;
            DeliveryReport.GenerateReport(); 
            if (DeliveryReport.LineItems != null && DeliveryReport.LineItems.Count > 0)
            {
                DeliveryReportGridView.Rows.Add(DeliveryReport.LineItems.Count);
                int rowCount = 0;
                foreach (DeliveryReportLineItem LineItem in DeliveryReport.LineItems)
                {
                    DeliveryReportGridView.Rows[rowCount].Cells[(int)DeliveryReportTableColumn.SNO].Value = rowCount + 1;
                    DeliveryReportGridView.Rows[rowCount].Cells[(int)DeliveryReportTableColumn.DELIVERYDATE].Value = LineItem.DeliveryDate.Date.ToString(fa.Global.Company.DateFormat);
                    DeliveryReportGridView.Rows[rowCount].Cells[(int)DeliveryReportTableColumn.NAME].Value = LineItem.CustomerName;
                    DeliveryReportGridView.Rows[rowCount].Cells[(int)DeliveryReportTableColumn.AMOUNT].Value = LineItem.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    DeliveryReportGridView.Rows[rowCount].Cells[(int)DeliveryReportTableColumn.PAYMENTTYPE].Value = LineItem.PaymentType;
                    DeliveryReportGridView.Rows[rowCount].Cells[(int)DeliveryReportTableColumn.STATUS].Value = LineItem.Status;
                    rowCount++;
                }
                EnableButtons(true);
                DeliveryReportGridView.ClearSelection();
            }
            else
            {
                DeliveryReportErrorMsg.Text = InformationMsg;
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DeliveryReportGridView.Rows.Clear();
            EnableButtons(false);
            ResetForm();
        }

        private void FormDeliveryReport_Load(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ToolStripBtnPrint_Click(object sender, EventArgs e)
        {
            DeliveryReportSavePrint DeliveryReportSavePrint = new DeliveryReportSavePrint();
            DeliveryReportSavePrint.ExportOrPrintToFile(DeliveryReport, "DeliveryReport", "pdf", true);
        }

        private void ToolStripBtnsave_Click(object sender, EventArgs e)
        {
            DeliveryReportSavePrint DeliveryReportSavePrint = new DeliveryReportSavePrint();
            DeliveryReportSavePrint.ExportOrPrintToFile(DeliveryReport, "DeliveryReport", "pdf", false);
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            DeliveryReportSavePrint DeliveryReportSavePrint = new DeliveryReportSavePrint();
            DeliveryReportSavePrint.ExportOrPrintToFile(DeliveryReport, "DeliveryReport", "pdf", true);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DeliveryReportSavePrint DeliveryReportSavePrint = new DeliveryReportSavePrint();
            DeliveryReportSavePrint.ExportOrPrintToFile(DeliveryReport, "DeliveryReport", "pdf", false);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
            }
            else if (keyData == (Keys.F9))
            {
                BtnPrint.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
