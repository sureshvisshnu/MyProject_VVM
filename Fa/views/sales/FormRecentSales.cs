using fa.api.OrderManagement;
using fa.api.utils;
using fa.model.OrderManagement;
using fa.views.controls.grid;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace fa.views.sales
{
    public partial class FormRecentSales : Form
    {
        public static string ChooseInvoiceErrorMsg = "Please select invoice";
        public static string DeletedInvoiceErrorMsg = "Your Select Sale Entry is Remove, Please press Go button then select Sale Entry";


        public IList<SaleEntry> SaleEntryInfo;
        public Entrytype RecentEntrytype = Entrytype.SALE;
        FormBase parent = null;
        public FormRecentSales(Object sender)
        {
            if (sender is FormItembasedSales)
            {
                parent = (FormItembasedSales)sender;
            }
            else if (sender is FormQuote)
            {
                parent = (FormQuote)sender;
            }
            else
            {
                parent = (FormSalesReturn)sender;
            }
            InitializeComponent();
        }

        private void FormRecentSales_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            LoadSaleEntry();
            GridViewRecentSales.Select();
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridViewRecentSales.Columns["Amount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            Cursor.Current = Cursors.Default;
        }

        private void GridViewRecentSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                BtnSelect_Click(sender, e);
            }
        }
        public void LoadSaleEntry()
        {
            if (SaleEntryInfo.Count > 0)
            {
                GridViewRecentSales.Rows.Add(SaleEntryInfo.Count);
                int i = 0;
                foreach (var lSaleEntryInfo in SaleEntryInfo)
                {
                    GridViewRecentSales.Rows[i].Cells[0].Value = lSaleEntryInfo.SaleDate.ToString(Global.Company.DateFormat);
                    GridViewRecentSales.Rows[i].Cells[1].Value = (lSaleEntryInfo.AccountsId == null) ? lSaleEntryInfo.CustomerName : lSaleEntryInfo.Account.Name;
                    GridViewRecentSales.Rows[i].Cells[2].Value = lSaleEntryInfo.RefNumber;
                    GridViewRecentSales.Rows[i].Cells[3].Value = lSaleEntryInfo.NetAmount.ToString(TextUtils.DecimalPlace(api.Accounting.CurrencyManager.Instance.GetCurrency(Global.Company).RoundingPrecision));
                    GridViewRecentSales.Rows[i].Cells[4].Value = lSaleEntryInfo.Id;
                    i++;
                }
            }
        }
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (GridViewRecentSales.Rows.Count > 0)
            {
                if (GridViewRecentSales.CurrentRow.Index > -1)
                {
                    //if (SalesManager.Instance.GetSaleEntry((long)GridViewRecentSales.CurrentRow.Cells[4].Value) != null)
                    //{
                    if (parent is FormQuote)
                    {
                        ((FormQuote)parent).SearchSalesId = (long)GridViewRecentSales.CurrentRow.Cells[4].Value;
                    }
                    else if (parent is FormItembasedSales)
                    {
                        ((FormItembasedSales)parent).SearchSalesId = (long)GridViewRecentSales.CurrentRow.Cells[4].Value;
                    }
                    else
                    {
                        if (RecentEntrytype == Entrytype.RETURN)
                        {
                            ((FormSalesReturn)parent).SearchSaleReturnId = (long)GridViewRecentSales.CurrentRow.Cells[4].Value;
                        }
                        else
                        {
                            ((FormSalesReturn)parent).SearchSalesId = (long)GridViewRecentSales.CurrentRow.Cells[4].Value;
                        }
                    }
                    this.Close();
                    //}
                    //else
                    //{
                    //    RecentPurchaseErrorMsg.Text = DeletedInvoiceErrorMsg;
                    //}
                }
                else
                {
                    RecentPurchaseErrorMsg.Text = ChooseInvoiceErrorMsg;
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSelect.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return true;
            }
            try
            {

                if (keyData == (Keys.Tab) && GridViewRecentSales.CurrentRow.Index > -1)
                {
                    if (GridViewRecentSales.CurrentCell.RowIndex != GridViewRecentSales.Rows.Count - 1)
                    {
                        GridViewRecentSales.CurrentCell = GridViewRecentSales[0, GridViewRecentSales.CurrentCell.RowIndex + 1];
                    }
                    else
                    {
                        BtnSelect.Select();
                    }

                }
                if (keyData == (Keys.Tab | Keys.Shift) && GridViewRecentSales.CurrentRow.Index > -1)
                {
                    if (GridViewRecentSales.CurrentRow.Index != 0)
                    {
                        GridViewRecentSales.CurrentCell = GridViewRecentSales[0, GridViewRecentSales.CurrentCell.RowIndex];
                    }
                    else
                    {
                        BtnSelect.Select();

                    }
                }

            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void GridViewRecentSales_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == Convert.ToChar(Keys.Enter))
            {
                BtnSelect_Click(sender, e);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSelect_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                if (GridViewRecentSales.Rows.Count > 0)
                {
                    GridViewRecentSales.Focus();
                    GridViewRecentSales.CurrentCell = GridViewRecentSales[0, 0];

                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewRecentSales.Rows.Count > 0)
                {
                    GridViewRecentSales.Focus();
                    GridViewRecentSales.CurrentCell = GridViewRecentSales[0, GridViewRecentSales.Rows.Count - 1];

                }
            }

        }
    }
}
