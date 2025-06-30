using fa.api.OrderManagement;
using fa.api.utils;
using fa.model.OrderManagement;
using fa.views.controls.grid;
using fa.views.purchase;
using Fa.views.purchase;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace fa.views.purchase
{
    public partial class FormRecentPurchase : Form
    {
        public static string ChooseInvoiceErrorMsg = "Please select {0}";
        public static string DeletedInvoiceErrorMsg = "Your Select Purchase Entry is Remove, Please press Go button then select Purchase Entry";

        public PurchaseEntrytype RecentEntrytype = PurchaseEntrytype.PURCHASE;
        public IList<PurchaseEntry> PurchaseEntryInfo;
        FormBase parent = null;
        public FormRecentPurchase(Object sender)
        {
            if (sender is FormPurchaseEntryNew)
            {
                parent = (FormPurchaseEntryNew)sender;
            }
            else if(sender is FormPurchaseReturn)
            {
                parent = (FormPurchaseReturn)sender;
            }
            else
            {
                parent = (FormPurchaseOrder)sender;
            }
            InitializeComponent();
        }
        private void FormRecentPurchase_Load(object sender, EventArgs e)
        {
            LoadPurchaseEntry();
            GridViewPurchaseRecentInvoice.Select();
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridViewPurchaseRecentInvoice.Columns["Amount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
        }
        private void GridViewPurchaseRecentInvoice_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                BtnSelect_Click(sender, e);
            }
        }

        public void LoadPurchaseEntry()
        {
            if (PurchaseEntryInfo.Count > 0)
            {
                GridViewPurchaseRecentInvoice.Rows.Add(PurchaseEntryInfo.Count);
                int i = 0;
                foreach (var lPurchaseEntryInfo in PurchaseEntryInfo)
                {
                    GridViewPurchaseRecentInvoice.Rows[i].Cells[0].Value = lPurchaseEntryInfo.RefDate.ToString(Global.Company.DateFormat);
                    GridViewPurchaseRecentInvoice.Rows[i].Cells[1].Value = lPurchaseEntryInfo.Account == null ? lPurchaseEntryInfo.SupplierName : lPurchaseEntryInfo.Account.Name;
                    GridViewPurchaseRecentInvoice.Rows[i].Cells[2].Value = lPurchaseEntryInfo.RefNumber;
                    GridViewPurchaseRecentInvoice.Rows[i].Cells[3].Value = lPurchaseEntryInfo.NetAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewPurchaseRecentInvoice.Rows[i].Cells[4].Value = lPurchaseEntryInfo.Id;
                    i++;
                }
            }
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (GridViewPurchaseRecentInvoice.Rows.Count > 0)
            {
                if (GridViewPurchaseRecentInvoice.CurrentRow.Index > -1)
                {
                    if (parent is FormPurchaseEntryNew)
                    {
                        ((FormPurchaseEntryNew)parent).SearchPurchaseId = (long)GridViewPurchaseRecentInvoice.CurrentRow.Cells[4].Value;
                    }
                    else if(parent is FormPurchaseOrder)
                    {
                        ((FormPurchaseOrder)parent).SearchPurchaseId = (long)GridViewPurchaseRecentInvoice.CurrentRow.Cells[4].Value;
                    }
                    else
                    {
                        if (RecentEntrytype == PurchaseEntrytype.RETURN)
                        {
                            ((FormPurchaseReturn)parent).SearchPurchaseReturnId = (long)GridViewPurchaseRecentInvoice.CurrentRow.Cells[4].Value;
                        }
                        else
                        {
                            ((FormPurchaseReturn)parent).SearchPurchaseId = (long)GridViewPurchaseRecentInvoice.CurrentRow.Cells[4].Value;
                        }
                    }
                    this.Close();
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
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void GridViewPurchaseRecentInvoice_KeyDown(object sender, KeyEventArgs e)
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

        }
    }
}
