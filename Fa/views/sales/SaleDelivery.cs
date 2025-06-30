using fa.api.OrderManagement;
using fa.api.utils;
using fa.model.OrderManagement;
using fa.views.controls.grid;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace fa.views.sales
{
    public partial class FormSaleDelivery : FormBase
    {
        public FormSaleDelivery()
        {
            InitializeComponent();
        }

        private void FormSaleDelivery_Load(object sender, EventArgs e)
        {
            ResetForm();
            LoadPendingInvoice();
        }
        private void LoadPendingInvoice()
        {
            GridViewPendingInvoice.Rows.Clear();
            List<SaleEntry> SaleEntrys = SalesManager.Instance.GetSaleDeliveryPendingInvoiceByCompanyId(Global.Company.CompanyId);
            if (SaleEntrys != null && SaleEntrys.Count > 0)
            {
                GridViewPendingInvoice.Rows.Add(SaleEntrys.Count);
                int i = 0;
                foreach (SaleEntry Entry in SaleEntrys)
                {
                    GridViewPendingInvoice.Rows[i].Cells[(int)GridPendingInvColumn.DATE].Value = Entry.SaleDate.ToString(Global.Company.DateFormat);
                    GridViewPendingInvoice.Rows[i].Cells[(int)GridPendingInvColumn.REFNO].Value = Entry.RefNumber;
                    GridViewPendingInvoice.Rows[i].Cells[(int)GridPendingInvColumn.AMOUNT].Value = Entry.NetAmount;
                    GridViewPendingInvoice.Rows[i].Cells[(int)GridPendingInvColumn.ID].Value = Entry.Id;
                    i++;
                }
                BtnDeliver.Enabled = true;
                GridViewPendingInvoice.Select();
            }
            else
            {
                BtnDeliver.Enabled = false;
            }
        }
        private void ResetForm()
        {
            TextBoxAddress.ResetText();
            TextBoxName.ResetText();
            TextBoxRefNo.ResetText();
            DateTimePickerInvoiceDate.Format = Global.Company.DateFormat;
            DateTimePickerInvoiceDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            TextBoxSaleId.ResetText();
            TextBoxCashAmount.ResetText();
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridViewPendingInvoice.Columns["InvAmount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
        }

        private void GridViewPendingInvoice_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (GridViewPendingInvoice.Rows[e.RowIndex].Cells[(int)GridPendingInvColumn.ID].Value != null)
                {
                    ResetForm();
                    LoadPaymentDetails((long)GridViewPendingInvoice.Rows[e.RowIndex].Cells[(int)GridPendingInvColumn.ID].Value);
                }
            }
        }
        private void LoadPaymentDetails(long SaleEntryId)
        {
            SaleEntry SaleEntry = SalesManager.Instance.GetSaleEntry(SaleEntryId);
            if (SaleEntry != null)
            {
                TextBoxSaleId.Text = SaleEntry.Id.ToString();
                TextBoxCashAmount.Text = SaleEntry.NetAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                TextBoxAddress.Text = SaleEntry.CustomerAddress.Replace(" ,","," + System.Environment.NewLine);
                TextBoxName.Text = SaleEntry.CustomerName;
                TextBoxRefNo.Text = SaleEntry.RefNumber;
                DateTimePickerInvoiceDate.Date = (DateTime)DateUtils.ToDate(SaleEntry.SaleDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            }
            else
            {
                MessageBox.Show("The selected sale is not available anymore");
                ResetForm();
                LoadPendingInvoice();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (GridViewPendingInvoice.Rows.Count > 0)
            {
                ResetForm();
                LoadPendingInvoice();
            }
            else
            {
                this.Close();
            }
        }

        private void BtnDeliver_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxSaleId.Text))
            {
                MessageBox.Show("The selected sale is not available anymore");
                return;
            }
            UpdateSale(true);
            ResetForm();
            LoadPendingInvoice();
        }
        private void UpdateSale(bool Deliver)
        {
            SaleEntry SaleEntry = SalesManager.Instance.GetSaleEntry(long.Parse(TextBoxSaleId.Text));
            if (SaleEntry != null)
            {
                SaleEntry.hasDelivered = Deliver;
                SalesManager.Instance.UpdateSaleEntry(SaleEntry);
            }
            else
            {
                MessageBox.Show("The selected sale is not available anymore");
                ResetForm();
                LoadPendingInvoice();
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
            }
            if (keyData == (Keys.F9))
            {
                BtnDeliver.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        
    }
}
