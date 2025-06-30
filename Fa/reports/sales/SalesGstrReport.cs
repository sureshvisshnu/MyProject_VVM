using DocumentFormat.OpenXml.VariantTypes;
using fa.api.utils;
using fa.libraries.utils;
using fa.report.sales;
using fa.views.utils.Report.Sale;
using Fa.report.sales;
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

namespace fa.reports.sales
{
    enum GstrReportTypeTableColumn
    {
        HSN, BTOB, BTOCL, BTOCS, BTOBA
    }
    enum GstrReportHsnTableColumn
    {
        SNO, HSN, DESC, UQC, T_QTY, T_VALUE, TAXABLE_VALUE, CEN_TAX, STATE_TAX, CESS_AMOUNT
    }
    enum GstrReportBtoBTableColumn
    {
        SNO, GSTN, NAME, INVOICE, DATE, VALUE, PLACE, REV_CHAR, TYPE, ECOM_GSTIN, RATE, TAXABLE_VALUE, CESS_AMOUNT
    }
    enum GstrReportBtoCLTableColumn
    {
        SNO, INVOICE, DATE, VALUE, PLACE, RATE, TAXABLE_VALUE, CESS_AMOUNT, ECOM_GSTIN
    }
    enum GstrReportBtoCSTableColumn
    {
        SNO, TYPE, PLACE, RATE, TAXABLE_VALUE, CESS_AMOUNT, ECOM_GSTIN
    }
    public partial class FormSalesGstrReport : Form
    {
        SalesGstrReportHsn lSalesGstrReportHsn = null;
        SalesGstrReportBtoB lSalesGstrReportBtoB = null;
        SalesGstrReportBtoCL lSalesGstrReportBtoCL = null;
        SalesGstrReportBtoCS lSalesGstrReportBtoCS = null;
        SalesGstrReportBtoBA lSalesGstrReportBtoBA = null;
        //private ComboBox ComboBoxReportType;

        public static string EnterValidDateErrorMsg = "Please enter current date to future date.";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";

        public FormSalesGstrReport()
        {
            InitializeComponent();
        }

        private void SalesGstrReport_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FromDate.Format = Global.Company.DateFormat;
            ToDate.Format = Global.Company.DateFormat;
            FromDate.Date = Global.getTransactionDate().AddDays(-30);
            ToDate.Date = Global.getTransactionDate();
            ResetForm();
            Cursor.Current = Cursors.Default;
        }

        private void ComboBoxReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (ComboBoxReportType.SelectedIndex == (int)GstrReportTypeTableColumn.HSN)
            {
                this.Width = GridViewHsn.Width + 25;
                ButtonLocationChange();
                ResetForm();
                GridViewHsn.Visible = true;
                GridViewBtoB.Visible = false;
                GridViewBtoCL.Visible = false;
                GridViewBtoCS.Visible = false;
            }
            else if (ComboBoxReportType.SelectedIndex == (int)GstrReportTypeTableColumn.BTOB)
            {
                this.Width = GridViewBtoB.Width + 25;
                ButtonLocationChange();
                ResetForm();
                GridViewHsn.Visible = false;
                GridViewBtoB.Visible = true;
                GridViewBtoCL.Visible = false;
                GridViewBtoCS.Visible = false;
            }
            else if (ComboBoxReportType.SelectedIndex == (int)GstrReportTypeTableColumn.BTOBA)
            {
                this.Width = GridViewBtoB.Width + 25;
                ButtonLocationChange();
                ResetForm();
                GridViewHsn.Visible = false;
                GridViewBtoB.Visible = true;
                GridViewBtoCL.Visible = false;
                GridViewBtoCS.Visible = false;
            }
            else if (ComboBoxReportType.SelectedIndex == (int)GstrReportTypeTableColumn.BTOCL)
            {
                this.Width = GridViewBtoCL.Width + 25;
                ButtonLocationChange();
                ResetForm();
                GridViewHsn.Visible = false;
                GridViewBtoB.Visible = false;
                GridViewBtoCL.Visible = true;
                GridViewBtoCS.Visible = false;
            }
            else if (ComboBoxReportType.SelectedIndex == (int)GstrReportTypeTableColumn.BTOCS)
            {
                this.Width = GridViewBtoCS.Width + 25;
                ButtonLocationChange();
                ResetForm();
                GridViewHsn.Visible = false;
                GridViewBtoB.Visible = false;
                GridViewBtoCL.Visible = false;
                GridViewBtoCS.Visible = true;
            }

            Cursor.Current = Cursors.Default;
        }
        private void ButtonLocationChange()
        {
            BtnExit.Location = new Point((this.Width - 117), (this.Height - 96));
            BtnPrint.Location = new Point((this.Width - 117 - 79), (this.Height - 96));
            BtnSave.Location = new Point((this.Width - 117 - 159), (this.Height - 96));
            BtnCancel.Location = new Point((this.Width - 117 - 246), (this.Height - 96));
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
            ErrorMsg.Text = "";
            GridViewHsn.Rows.Clear();
            GridViewBtoB.Rows.Clear();
            GridViewBtoCL.Rows.Clear();
            GridViewBtoCS.Rows.Clear();
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
            ComboBoxReportType.SelectedIndex = 0;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                SaleGstrReportSaveExcel SaleGstrReportSaveExcel = new SaleGstrReportSaveExcel();


                lSalesGstrReportHsn = new SalesGstrReportHsn();
                lSalesGstrReportHsn.FromDate = (DateTime)FromDate.Date;
                lSalesGstrReportHsn.ToDate = (DateTime)ToDate.Date;
                lSalesGstrReportHsn.Company = Global.Company;
                lSalesGstrReportHsn.GenerateReport();

                lSalesGstrReportBtoB = new SalesGstrReportBtoB();
                lSalesGstrReportBtoB.FromDate = (DateTime)FromDate.Date;
                lSalesGstrReportBtoB.ToDate = (DateTime)ToDate.Date;
                lSalesGstrReportBtoB.Company = Global.Company;
                lSalesGstrReportBtoB.GenerateReport();

                lSalesGstrReportBtoCL = new SalesGstrReportBtoCL();
                lSalesGstrReportBtoCL.FromDate = (DateTime)FromDate.Date;
                lSalesGstrReportBtoCL.ToDate = (DateTime)ToDate.Date;
                lSalesGstrReportBtoCL.Company = Global.Company;
                lSalesGstrReportBtoCL.GenerateReport();

                lSalesGstrReportBtoCS = new SalesGstrReportBtoCS();
                lSalesGstrReportBtoCS.FromDate = (DateTime)FromDate.Date;
                lSalesGstrReportBtoCS.ToDate = (DateTime)ToDate.Date;
                lSalesGstrReportBtoCS.Company = Global.Company;
                lSalesGstrReportBtoCS.GenerateReport();

                lSalesGstrReportBtoBA = new SalesGstrReportBtoBA();
                lSalesGstrReportBtoBA.FromDate = (DateTime)FromDate.Date;
                lSalesGstrReportBtoBA.ToDate = (DateTime)ToDate.Date;
                lSalesGstrReportBtoBA.Company = Global.Company;
                lSalesGstrReportBtoBA.GenerateReport();

                int index = ComboBoxReportType.SelectedIndex;
                SaleGstrReportSaveExcel.SaveGstrReportToExcel(lSalesGstrReportHsn, lSalesGstrReportBtoB,
                                                              lSalesGstrReportBtoCL, lSalesGstrReportBtoCS, lSalesGstrReportBtoBA, index);

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
            Cursor.Current = Cursors.WaitCursor;
            if (Validation())
            {
                if (ComboBoxReportType.SelectedIndex == (int)GstrReportTypeTableColumn.HSN)
                {
                    generateGstnHsnReport();
                }
                else if (ComboBoxReportType.SelectedIndex == (int)GstrReportTypeTableColumn.BTOB)
                {
                    generateGstnBtoBReport();
                }
                else if (ComboBoxReportType.SelectedIndex == (int)GstrReportTypeTableColumn.BTOBA)
                {
                    generateGstnBtoBAReport();
                }
                else if (ComboBoxReportType.SelectedIndex == (int)GstrReportTypeTableColumn.BTOCL)
                {
                    generateGstnBtoCLReport();
                }
                else if (ComboBoxReportType.SelectedIndex == (int)GstrReportTypeTableColumn.BTOCS)
                {
                    generateGstnBtoCSReport();
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void generateGstnHsnReport()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewHsn.Rows.Clear();
            lSalesGstrReportHsn = new SalesGstrReportHsn();
            lSalesGstrReportHsn.FromDate = (DateTime)FromDate.Date;
            lSalesGstrReportHsn.ToDate = (DateTime)ToDate.Date;
            lSalesGstrReportHsn.Company = Global.Company;
            lSalesGstrReportHsn.GenerateReport();
            if (lSalesGstrReportHsn.LineItems != null && lSalesGstrReportHsn.LineItems.Count > 0)
            {
                EnableButtons(true);
                GridViewHsn.Rows.Add(lSalesGstrReportHsn.LineItems.Count);
                int rowCount = 0;
                foreach (SalesGstrReportHsnLineItem LineItem in lSalesGstrReportHsn.LineItems)
                {
                    GridViewHsn.Rows[rowCount].Cells[(int)GstrReportHsnTableColumn.SNO].Value = rowCount + 1;
                    GridViewHsn.Rows[rowCount].Cells[(int)GstrReportHsnTableColumn.HSN].Value = LineItem.Hsn;
                    GridViewHsn.Rows[rowCount].Cells[(int)GstrReportHsnTableColumn.DESC].Value = LineItem.Description;
                    GridViewHsn.Rows[rowCount].Cells[(int)GstrReportHsnTableColumn.UQC].Value = LineItem.Uqc;
                    GridViewHsn.Rows[rowCount].Cells[(int)GstrReportHsnTableColumn.T_QTY].Value = LineItem.TotalQuantity;
                    GridViewHsn.Rows[rowCount].Cells[(int)GstrReportHsnTableColumn.T_VALUE].Value = LineItem.TotalValue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                    GridViewHsn.Rows[rowCount].Cells[(int)GstrReportHsnTableColumn.TAXABLE_VALUE].Value = LineItem.TaxableValue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                    GridViewHsn.Rows[rowCount].Cells[(int)GstrReportHsnTableColumn.CEN_TAX].Value = LineItem.CentralTaxAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                    GridViewHsn.Rows[rowCount].Cells[(int)GstrReportHsnTableColumn.STATE_TAX].Value = LineItem.StateTaxAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                    GridViewHsn.Rows[rowCount].Cells[(int)GstrReportHsnTableColumn.CESS_AMOUNT].Value = LineItem.CessAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    rowCount++;
                }
            }
            else
            {
                ErrorMsg.Text = "No Information Found..!";
            }
        }
        private void generateGstnBtoBReport()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewBtoB.Rows.Clear();
            lSalesGstrReportBtoB = new SalesGstrReportBtoB();
            lSalesGstrReportBtoB.FromDate = (DateTime)FromDate.Date;
            lSalesGstrReportBtoB.ToDate = (DateTime)ToDate.Date;
            lSalesGstrReportBtoB.Company = Global.Company;
            lSalesGstrReportBtoB.GenerateReport();
            if (lSalesGstrReportBtoB.LineItems != null && lSalesGstrReportBtoB.LineItems.Count > 0)
            {
                EnableButtons(true);
                GridViewBtoB.Rows.Add(lSalesGstrReportBtoB.LineItems.Count);
                int rowCount = 0;

                double[] TaxRate = new double[5] { 0, 5, 12, 18, 28 };
                double[] TaxableValue = new double[5];
                double[] TaxPaid = new double[5];

                foreach (SalesGstrReportBtoBLineItem LineItem in lSalesGstrReportBtoB.LineItems.OrderBy(x => x.Rate))
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
                string cusAddress = string.Empty;
                foreach (SalesGstrReportBtoBLineItem LineItem in lSalesGstrReportBtoB.LineItems)
                {
                    GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.SNO].Value = rowCount + 1;
                    GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.GSTN].Value = LineItem.Gstn;
                    if (cusName == string.Empty || cusName != LineItem.Name)
                    {
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.NAME].Value = LineItem.Name;
                        cusName = LineItem.Name;
                    }
                    GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.INVOICE].Value = LineItem.Invoice;
                    GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.DATE].Value = LineItem.Date.Date.ToString(Global.Company.DateFormat);
                    GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.VALUE].Value = LineItem.value.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    if (cusAddress == string.Empty || cusAddress != Global.Company.Address.FullAddressInSingleLine)
                    {
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.PLACE].Value = Global.Company.Address.FullAddressInSingleLine;
                        cusAddress = Global.Company.Address.FullAddressInSingleLine;
                    }
                    GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.REV_CHAR].Value = LineItem.RevCharge;

                    GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.TYPE].Value = LineItem.Type;
                    GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.ECOM_GSTIN].Value = LineItem.EcomGstn;
                    GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.RATE].Value = LineItem.Rate.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
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

                GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.INVOICE].Value = TaxableValue[0].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.INVOICE].Value = TaxPaid[0].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                for (int j = 1; j < 5; j++)
                {
                    if (TaxRate[j].ToString() == "5")
                    {
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.DATE].Value = "Gst " + TaxRate[j] + "%";
                        GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.DATE].Value = "Gst " + TaxRate[j] + "%";

                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.VALUE].Value = TaxableValue[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.VALUE].Value = TaxPaid[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    }
                    else if (TaxRate[j].ToString() == "12")
                    {
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.PLACE].Value = "Gst " + TaxRate[j] + "%";
                        GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.PLACE].Value = "Gst " + TaxRate[j] + "%";

                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.REV_CHAR].Value = TaxableValue[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.REV_CHAR].Value = TaxPaid[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    }
                    else if (TaxRate[j].ToString() == "18")
                    {
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.TYPE].Value = "Gst " + TaxRate[j] + "%";
                        GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.TYPE].Value = "Gst " + TaxRate[j] + "%";

                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.ECOM_GSTIN].Value = TaxableValue[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.ECOM_GSTIN].Value = TaxPaid[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    }
                    else if (TaxRate[j].ToString() == "28")
                    {
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.RATE].Value = "Gst " + TaxRate[j] + "%";
                        GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.RATE].Value = "Gst " + TaxRate[j] + "%";

                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.TAXABLE_VALUE].Value = TaxableValue[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.TAXABLE_VALUE].Value = TaxPaid[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    }

                }
            }
            else
            {
                ErrorMsg.Text = "No Information Found..!";
            }
        }
        private void generateGstnBtoBAReport()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewBtoB.Rows.Clear();
            lSalesGstrReportBtoBA = new SalesGstrReportBtoBA();
            lSalesGstrReportBtoBA.FromDate = (DateTime)FromDate.Date;
            lSalesGstrReportBtoBA.ToDate = (DateTime)ToDate.Date;
            lSalesGstrReportBtoBA.Company = Global.Company;
            lSalesGstrReportBtoBA.GenerateReport();
            if (lSalesGstrReportBtoBA.LineItems != null && lSalesGstrReportBtoBA.LineItems.Count > 0)
            {
                EnableButtons(true);
                GridViewBtoB.Rows.Add(lSalesGstrReportBtoBA.LineItems.Count);
                int rowCount = 0;

                double[] TaxRate = new double[5] { 0, 5, 12, 18, 28 };
                double[] TaxableValue = new double[5];
                double[] TaxPaid = new double[5];

                foreach (SalesGstrReportBtoBLineItem LineItem in lSalesGstrReportBtoBA.LineItems.OrderBy(x => x.Rate))
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
                string cusAddress = string.Empty;
                foreach (SalesGstrReportBtoBLineItem LineItem in lSalesGstrReportBtoBA.LineItems)
                {
                    GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.SNO].Value = rowCount + 1;
                    GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.GSTN].Value = LineItem.Gstn;
                    if (cusName == string.Empty || cusName != LineItem.Name)
                    {
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.NAME].Value = LineItem.Name;
                        cusName = LineItem.Name;
                    }
                    GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.INVOICE].Value = LineItem.Invoice;
                    GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.DATE].Value = LineItem.Date.Date.ToString(Global.Company.DateFormat);
                    GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.VALUE].Value = LineItem.value.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    if (cusAddress == string.Empty || cusAddress != Global.Company.Address.FullAddressInSingleLine)
                    {
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.PLACE].Value = Global.Company.Address.FullAddressInSingleLine;
                        cusAddress = Global.Company.Address.FullAddressInSingleLine;
                    }
                    GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.REV_CHAR].Value = LineItem.RevCharge;

                    GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.TYPE].Value = LineItem.Type;
                    GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.ECOM_GSTIN].Value = LineItem.EcomGstn;
                    GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.RATE].Value = LineItem.Rate.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
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

                GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.INVOICE].Value = TaxableValue[0].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.INVOICE].Value = TaxPaid[0].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                for (int j = 1; j < 5; j++)
                {
                    if (TaxRate[j].ToString() == "5")
                    {
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.DATE].Value = "Gst " + TaxRate[j] + "%";
                        GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.DATE].Value = "Gst " + TaxRate[j] + "%";

                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.VALUE].Value = TaxableValue[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.VALUE].Value = TaxPaid[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    }
                    else if (TaxRate[j].ToString() == "12")
                    {
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.PLACE].Value = "Gst " + TaxRate[j] + "%";
                        GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.PLACE].Value = "Gst " + TaxRate[j] + "%";

                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.REV_CHAR].Value = TaxableValue[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.REV_CHAR].Value = TaxPaid[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    }
                    else if (TaxRate[j].ToString() == "18")
                    {
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.TYPE].Value = "Gst " + TaxRate[j] + "%";
                        GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.TYPE].Value = "Gst " + TaxRate[j] + "%";

                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.ECOM_GSTIN].Value = TaxableValue[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.ECOM_GSTIN].Value = TaxPaid[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    }
                    else if (TaxRate[j].ToString() == "28")
                    {
                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.RATE].Value = "Gst " + TaxRate[j] + "%";
                        GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.RATE].Value = "Gst " + TaxRate[j] + "%";

                        GridViewBtoB.Rows[rowCount].Cells[(int)GstrReportBtoBTableColumn.TAXABLE_VALUE].Value = TaxableValue[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewBtoB.Rows[rowCount + 1].Cells[(int)GstrReportBtoBTableColumn.TAXABLE_VALUE].Value = TaxPaid[j].ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    }

                }
            }
            else
            {
                ErrorMsg.Text = "No Information Found..!";
            }
        }
        private void generateGstnBtoCLReport()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewBtoCL.Rows.Clear();
            lSalesGstrReportBtoCL = new SalesGstrReportBtoCL();
            lSalesGstrReportBtoCL.FromDate = (DateTime)FromDate.Date;
            lSalesGstrReportBtoCL.ToDate = (DateTime)ToDate.Date;
            lSalesGstrReportBtoCL.Company = Global.Company;
            lSalesGstrReportBtoCL.GenerateReport();
            if (lSalesGstrReportBtoCL.LineItems != null && lSalesGstrReportBtoCL.LineItems.Count > 0)
            {
                EnableButtons(true);
                GridViewBtoCL.Rows.Add(lSalesGstrReportBtoCL.LineItems.Count);
                int rowCount = 0;
                foreach (SalesGstrReportBtoBLineItem LineItem in lSalesGstrReportBtoCL.LineItems)
                {
                    GridViewBtoCL.Rows[rowCount].Cells[(int)GstrReportBtoCLTableColumn.SNO].Value = rowCount + 1;
                    GridViewBtoCL.Rows[rowCount].Cells[(int)GstrReportBtoCLTableColumn.INVOICE].Value = LineItem.Invoice;
                    GridViewBtoCL.Rows[rowCount].Cells[(int)GstrReportBtoCLTableColumn.DATE].Value = LineItem.Date.ToString(Global.Company.DateFormat);
                    GridViewBtoCL.Rows[rowCount].Cells[(int)GstrReportBtoCLTableColumn.VALUE].Value = LineItem.value;
                    GridViewBtoCL.Rows[rowCount].Cells[(int)GstrReportBtoCLTableColumn.PLACE].Value = Global.Company.Address.FullAddressInSingleLine;

                    GridViewBtoCL.Rows[rowCount].Cells[(int)GstrReportBtoCLTableColumn.ECOM_GSTIN].Value = LineItem.EcomGstn;
                    GridViewBtoCL.Rows[rowCount].Cells[(int)GstrReportBtoCLTableColumn.RATE].Value = LineItem.Rate.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                    GridViewBtoCL.Rows[rowCount].Cells[(int)GstrReportBtoCLTableColumn.TAXABLE_VALUE].Value = LineItem.TaxableValue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                    GridViewBtoCL.Rows[rowCount].Cells[(int)GstrReportBtoCLTableColumn.CESS_AMOUNT].Value = LineItem.CessAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                    rowCount++;
                }
            }
            else
            {
                ErrorMsg.Text = "No Information Found..!";
            }
        }

        private void generateGstnBtoCSReport()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewBtoCS.Rows.Clear();
            lSalesGstrReportBtoCS = new SalesGstrReportBtoCS();
            lSalesGstrReportBtoCS.FromDate = (DateTime)FromDate.Date;
            lSalesGstrReportBtoCS.ToDate = (DateTime)ToDate.Date;
            lSalesGstrReportBtoCS.Company = Global.Company;
            lSalesGstrReportBtoCS.GenerateReport();
            if (lSalesGstrReportBtoCS.LineItems != null && lSalesGstrReportBtoCS.LineItems.Count > 0)
            {
                EnableButtons(true);
                GridViewBtoCS.Rows.Add(lSalesGstrReportBtoCS.LineItems.Count);
                int rowCount = 0;
                foreach (SalesGstrReportBtoBLineItem LineItem in lSalesGstrReportBtoCS.LineItems)
                {
                    GridViewBtoCS.Rows[rowCount].Cells[(int)GstrReportBtoCSTableColumn.SNO].Value = rowCount + 1;
                    GridViewBtoCS.Rows[rowCount].Cells[(int)GstrReportBtoCSTableColumn.PLACE].Value = Global.Company.Address.FullAddressInSingleLine;
                    GridViewBtoCS.Rows[rowCount].Cells[(int)GstrReportBtoCSTableColumn.TYPE].Value = LineItem.Type;
                    GridViewBtoCS.Rows[rowCount].Cells[(int)GstrReportBtoCSTableColumn.ECOM_GSTIN].Value = LineItem.EcomGstn;
                    GridViewBtoCS.Rows[rowCount].Cells[(int)GstrReportBtoCSTableColumn.RATE].Value = LineItem.Rate.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                    GridViewBtoCS.Rows[rowCount].Cells[(int)GstrReportBtoCSTableColumn.TAXABLE_VALUE].Value = LineItem.TaxableValue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                    GridViewBtoCS.Rows[rowCount].Cells[(int)GstrReportBtoCSTableColumn.CESS_AMOUNT].Value = LineItem.CessAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                    rowCount++;
                }
            }
            else
            {
                ErrorMsg.Text = "No Information Found..!";
            }
        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            string lFromDate = ((DateTime)FromDate.Date).ToString(Global.Company.DateFormat);
            string lToDate = ((DateTime)ToDate.Date).ToString(Global.Company.DateFormat);
            if (ComboBoxReportType.SelectedIndex == (int)GstrReportTypeTableColumn.HSN)
            {
                SaleGstrReportPrintpdf saleGSTRrepotPrintpdf = new SaleGstrReportPrintpdf();
                saleGSTRrepotPrintpdf.ExportOrPrintToFile(GridViewHsn, SalesGstrReportNames.getSalesReportName(SalesGstrReportType.HSN), "SalesGSTRReportHSN", "pdf", true, lFromDate, lToDate);
            }
            if (ComboBoxReportType.SelectedIndex == (int)GstrReportTypeTableColumn.BTOB)
            {
                SaleGstrReportPrintpdf saleGSTRrepotPrintpdf = new SaleGstrReportPrintpdf();
                saleGSTRrepotPrintpdf.ExportOrPrintToFile(GridViewBtoB, SalesGstrReportNames.getSalesReportName(SalesGstrReportType.BUSINESS_TO_BUSINESS), "SalesGSTRReportBTOB", "pdf", true, lFromDate, lToDate);
            }
            if (ComboBoxReportType.SelectedIndex == (int)GstrReportTypeTableColumn.BTOCL)
            {
                SaleGstrReportPrintpdf saleGSTRrepotPrintpdf = new SaleGstrReportPrintpdf();
                saleGSTRrepotPrintpdf.ExportOrPrintToFile(GridViewBtoCL, SalesGstrReportNames.getSalesReportName(SalesGstrReportType.BUSINESS_TO_CUST_LARGE), "SalesGSTRReportBTOCL", "pdf", true, lFromDate, lToDate);
            }
            if (ComboBoxReportType.SelectedIndex == (int)GstrReportTypeTableColumn.BTOCS)
            {
                SaleGstrReportPrintpdf saleGSTRrepotPrintpdf = new SaleGstrReportPrintpdf();
                saleGSTRrepotPrintpdf.ExportOrPrintToFile(GridViewBtoCS, SalesGstrReportNames.getSalesReportName(SalesGstrReportType.BUSINESS_TO_CUST_SMALL), "SalesGSTRReportBTOCS", "pdf", true, lFromDate, lToDate);
            }
            if (ComboBoxReportType.SelectedIndex == (int)GstrReportTypeTableColumn.BTOBA)
            {
                SaleGstrReportPrintpdf saleGSTRrepotPrintpdf = new SaleGstrReportPrintpdf();
                saleGSTRrepotPrintpdf.ExportOrPrintToFile(GridViewBtoB, SalesGstrReportNames.getSalesReportName(SalesGstrReportType.BUSINESS_TO_BUSINESS_OTHERS), "SalesGSTRReportBTOBO", "pdf", true, lFromDate, lToDate);
            }
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
