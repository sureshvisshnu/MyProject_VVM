using fa.api.utils;
using fa.views.utils.Report.Purchase;
using Fa.report.Purchase;
using Fa.report.sales;
using Fa.views.utils.Report.Purchase;
using Fa.views.utils.Report.Sale;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.reports.Purchase
{
    enum GstrReportBtoBTableColumn
    {
        SNO, GSTN, NAME, INVOICE, DATE, VALUE, PLACE, REV_CHAR, TYPE, ECOM_GSTIN, RATE, TAXABLE_VALUE, CESS_AMOUNT
    }
    public partial class FormPurchaseGstrReport : Form
    {
        PurchaseGstrReportBtoB PurchaseGstrReportBtoB = null!;

        public static string EnterValidDateErrorMsg = "Please enter current date to future date.";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";

        public FormPurchaseGstrReport()
        {
            InitializeComponent();
        }

        private void FormPurchaseGstrReport_Load(object sender, EventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;
            FromDate.Format = Global.Company.DateFormat;
            ToDate.Format = Global.Company.DateFormat;
            FromDate.Date = DateTime.Now.AddDays(-30);
            ToDate.Date = DateTime.Now.AddDays(1);
            ResetForm();
            Cursor.Current = Cursors.Default;
        }
        private void EnableButtons(bool Enable)
        {
            BtnSave.Enabled = Enable;
            BtnPrint.Enabled = Enable;
            ToolStripSave.Enabled = Enable;
            ToolStripPrint.Enabled = Enable;
        }
        private void ResetForm()
        {
            ErrorMsg.Text = string.Empty;
            GridViewBtoB.Rows.Clear();
            FromDate.Format = Global.Company.DateFormat;
            ToDate.Format = Global.Company.DateFormat;
            FromDate.Date = DateTime.Now.AddDays(-30);
            ToDate.Date = DateTime.Now.AddDays(1);
            EnableButtons(false);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
                return true;
            }
            if (keyData == (Keys.F9))
            {
                BtnPrint.PerformClick();
                return true;
            }
            if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                PurchaseGstrReportSaveExcel PurchaseGstrReportSaveExcel = new PurchaseGstrReportSaveExcel();
                PurchaseGstrReportBtoB = new PurchaseGstrReportBtoB();
                PurchaseGstrReportBtoB.FromDate = (DateTime)FromDate.Date;
                PurchaseGstrReportBtoB.ToDate = (DateTime)ToDate.Date;
                PurchaseGstrReportBtoB.Company = Global.Company;
                PurchaseGstrReportBtoB.GenerateReport();
                PurchaseGstrReportSaveExcel.SaveGstrReportToExcel(PurchaseGstrReportBtoB);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnGo_Click(object sender, EventArgs e)
        {
            generateGstnBtoBReport();

        }
        private void generateGstnBtoBReport()
        {
            if (Validation()) 
            { 
                ErrorMsg.Text = string.Empty;
                EnableButtons(false);
                GridViewBtoB.Rows.Clear();
                PurchaseGstrReportBtoB = new PurchaseGstrReportBtoB();
                PurchaseGstrReportBtoB.FromDate = (DateTime)FromDate.Date;
                PurchaseGstrReportBtoB.ToDate = (DateTime)ToDate.Date;
                PurchaseGstrReportBtoB.Company = Global.Company;
                PurchaseGstrReportBtoB.GenerateReport();
                if (PurchaseGstrReportBtoB.LineItems != null && PurchaseGstrReportBtoB.LineItems.Count > 0)
                {
                    EnableButtons(true);
                    GridViewBtoB.Rows.Add(PurchaseGstrReportBtoB.LineItems.Count);
                    int rowCount = 0;
                    double[] TaxRate = new double[5] { 0, 5, 12, 18, 28 };
                    double[] TaxableValue = new double[5];
                    double[] TaxPaid = new double[5];

                    foreach (PurchaseGstrReportBtoBLineItem LineItem in PurchaseGstrReportBtoB.LineItems.OrderBy(x => x.Rate))
                    {
                        if (LineItem.Rate == 0)
                        {
                            TaxableValue[0] += LineItem.TaxableValue;
                            TaxPaid[0] += (LineItem.value - LineItem.TaxableValue);
                        }
                        else if (LineItem.Rate == 5)
                        {
                            TaxableValue[1] += LineItem.TaxableValue;
                            TaxPaid[1] += (LineItem.value - LineItem.TaxableValue);
                        }
                        else if (LineItem.Rate == 12)
                        {
                            TaxableValue[2] += LineItem.TaxableValue;
                            TaxPaid[2] += (LineItem.value - LineItem.TaxableValue);
                        }
                        else if (LineItem.Rate == 18)
                        {
                            TaxableValue[3] += LineItem.TaxableValue;
                            TaxPaid[3] += (LineItem.value - LineItem.TaxableValue);
                        }
                        else if (LineItem.Rate == 28)
                        {
                            TaxableValue[4] += LineItem.TaxableValue;
                            TaxPaid[4] += (LineItem.value - LineItem.TaxableValue);
                        }
                    }
                    string cusName = string.Empty;
                    string cusPlace = string.Empty;
                    foreach (PurchaseGstrReportBtoBLineItem LineItem in PurchaseGstrReportBtoB.LineItems)
                    {
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.SNO].Value = rowCount + 1;
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.GSTN].Value = LineItem.Gstn;
                        if (string.IsNullOrEmpty(cusName) || cusName != LineItem.Name)
                        {
                            GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.NAME].Value = LineItem.Name;
                            cusName = LineItem.Name;
                        }
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.INVOICE].Value = LineItem.Invoice;
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.DATE].Value = LineItem.Date.ToString("dd-MM-yyyy");
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.VALUE].Value = LineItem.value.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        if (string.IsNullOrEmpty(cusPlace) || cusPlace != Global.Company.Address.FullAddressInSingleLine)
                        {
                            GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.PLACE].Value = Global.Company.Address.FullAddressInSingleLine;
                            cusPlace = Global.Company.Address.FullAddressInSingleLine;
                        }
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.REV_CHAR].Value = LineItem.RevCharge;

                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.TYPE].Value = LineItem.Type;
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.ECOM_GSTIN].Value = LineItem.EcomGstn;
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.RATE].Value = LineItem.Rate.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.TAXABLE_VALUE].Value = LineItem.TaxableValue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.CESS_AMOUNT].Value = LineItem.CessAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        rowCount++;
                    }
                    GridViewBtoB.Rows.Add(2);
                    //rowCount++;

                    GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.GSTN].Value = "Sub totals Taxable Value";
                    GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.NAME].Value = "Exp";

                    GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.GSTN].Value = "Sub totals Tax Paid";
                    GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.NAME].Value = "";

                    GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.INVOICE].Value = TaxableValue[0].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                    GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.INVOICE].Value = TaxPaid[0].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;

                    for (int j = 1; j < 5; j++)
                    {
                        if (TaxRate[j].ToString() == "5")
                        {
                            GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.DATE].Value = "Gst " + TaxRate[j] + "%";
                            GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.DATE].Value = "Gst " + TaxRate[j] + "%";

                            GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.VALUE].Value = TaxableValue[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                            GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.VALUE].Value = TaxPaid[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                        }
                        else if (TaxRate[j].ToString() == "12")
                        {
                            GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.PLACE].Value = "Gst " + TaxRate[j] + "%";
                            GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.PLACE].Value = "Gst " + TaxRate[j] + "%";

                            GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.REV_CHAR].Value = TaxableValue[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                            GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.REV_CHAR].Value = TaxPaid[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                        }
                        else if (TaxRate[j].ToString() == "18")
                        {
                            GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.TYPE].Value = "Gst " + TaxRate[j] + "%";
                            GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.TYPE].Value = "Gst " + TaxRate[j] + "%";

                            GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.ECOM_GSTIN].Value = TaxableValue[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                            GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.ECOM_GSTIN].Value = TaxPaid[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                        }
                        else if (TaxRate[j].ToString() == "28")
                        {
                            GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.RATE].Value = "Gst " + TaxRate[j] + "%";
                            GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.RATE].Value = "Gst " + TaxRate[j] + "%";

                            GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.TAXABLE_VALUE].Value = TaxableValue[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                            GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.TAXABLE_VALUE].Value = TaxPaid[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                        }

                    }
                }
                else
                {
                    ErrorMsg.Text = "No Information Found..!";
                }
            }
        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            string lFromDate = ((DateTime)FromDate.Date).ToString(Global.Company.DateFormat);
            string lToDate = ((DateTime)ToDate.Date).ToString(Global.Company.DateFormat);

            PurchaseGstrReportPrintA4 purchaseGstrReportPrintA4 = new PurchaseGstrReportPrintA4();
            purchaseGstrReportPrintA4.ExportOrPrintToFile(GridViewBtoB, PurchaseGstrReportNames.getPurchaseReportName(PurchaseGstrReportType.BUSINESS_TO_BUSINESS), "PurchaseGSTRReportB2B", "pdf", true, lFromDate, lToDate);
        }
        private bool Validation()
        {
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
    }
}
