using fa.api.OrderManagement;
using fa.model.Accounting.Transactions;
using fa.model.OrderManagement;
using fa.views.controls.grid;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace fa.views.sales
{
    public enum GridRecentPaymentColumn
    {
        DATE, REFNO, NAME, AMOUNT, ID
    }
    public partial class FormRecentSalePayment : FormBase
    {
        public static string ChooseInvoiceErrorMsg = "Please select invoice";
        public static string DeletedInvoiceErrorMsg = "Your Select Entry is Remove, Please press Go button then select Entry";

        public IList<SaleEntry> SaleEntryInfo;
        FormPOSReceivePayment parent = null;
        public FormRecentSalePayment(Object sender)
        {
            parent = (FormPOSReceivePayment)sender;
            InitializeComponent();
        }

        private void FormRecentSalePayment_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            LoadRecentPayment();
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridViewRecentSalesPayment.Columns["Amount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            Cursor.Current = Cursors.Default;
        }
        private void LoadRecentPayment()
        {
            GridViewRecentSalesPayment.Rows.Clear();
            if (SaleEntryInfo.Count > 0)
            {
                GridViewRecentSalesPayment.Rows.Add(SaleEntryInfo.Count);
                int i = 0;
                foreach (SaleEntry Entry in SaleEntryInfo)
                {
                    GridViewRecentSalesPayment.Rows[i].Cells[(int)GridRecentPaymentColumn.DATE].Value = Entry.SaleDate.ToString(Global.Company.DateFormat);
                    GridViewRecentSalesPayment.Rows[i].Cells[(int)GridRecentPaymentColumn.REFNO].Value = Entry.RefNumber;
                    GridViewRecentSalesPayment.Rows[i].Cells[(int)GridRecentPaymentColumn.NAME].Value = Entry.CustomerName;

                    GridViewRecentSalesPayment.Rows[i].Cells[(int)GridRecentPaymentColumn.AMOUNT].Value = Entry.SalePayment != null ? (Entry.SalePayment.TransctionType == PaymentType.CASH) ? Entry.SalePayment.Amount : Entry.NetAmount : Entry.NetAmount;
                    GridViewRecentSalesPayment.Rows[i].Cells[(int)GridRecentPaymentColumn.ID].Value = Entry.Id;
                    i++;
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSelect.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (GridViewRecentSalesPayment.Rows.Count > 0)
            {
                if (GridViewRecentSalesPayment.CurrentRow.Index > -1)
                {
                    //if (SalesManager.Instance.GetSaleEntry((long)GridViewRecentSalesPayment.CurrentRow.Cells[(int)GridRecentPaymentColumn.ID].Value) != null)
                    //{
                    parent.SearchSalesId = (long)GridViewRecentSalesPayment.CurrentRow.Cells[(int)GridRecentPaymentColumn.ID].Value;
                    this.Close();
                    //}
                    //else
                    //{
                    //    ErrorMsg.Text = DeletedInvoiceErrorMsg;
                    //}
                }
                else
                {
                    ErrorMsg.Text = ChooseInvoiceErrorMsg;
                }
            }
        }

        private void GridViewRecentSalesPayment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == Convert.ToChar(Keys.Enter))
            {
                BtnSelect_Click(sender, e);
            }
        }

        private void GridViewRecentSalesPayment_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                long Id = (long)GridViewRecentSalesPayment.Rows[e.RowIndex].Cells[(int)GridRecentPaymentColumn.ID].Value;
                parent.SearchSalesId = Id;
                this.Close();
            }
        }
    }
}
