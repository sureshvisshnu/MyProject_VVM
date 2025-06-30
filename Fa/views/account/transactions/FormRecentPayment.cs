using fa.api.Accounting;
using fa.api.utils;
using fa.model.Accounting.Transactions;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace fa.views.account.transactions
{


    public partial class FormRecentPayment : Form
    {
        public IList<Payment> PaymentInfo;
        public IList<Receipt> ReceiptInfo;
        FormBase parent = null;
        public static string DeletedEntryErrorMsg = "Your selected entry is removed, please reopen the form then select the entry";

        public FormRecentPayment(Object sender)
        {
            if (sender is FormPayment)
            {
                parent = (FormPayment)sender;
            }
            else if (sender is FormReceipts)
            {
                parent = (FormReceipts)sender;
            }
            InitializeComponent();
        }

        private void FormRecentPayment_Load(object sender, EventArgs e)
        {
            LoadPaymentEntry();
            GridViewPaymentRecent.Select();
        }
        public void LoadPaymentEntry()
        {
            if (parent is FormPayment)
            {
                if (PaymentInfo.Count > 0)
                {
                    GridViewPaymentRecent.Rows.Add(PaymentInfo.Count);
                    int i = 0;
                    foreach (var lPaymentInfo in PaymentInfo)
                    {
                        GridViewPaymentRecent.Rows[i].Cells[0].Value = lPaymentInfo.TransactionDate.ToString(Global.Company.DateFormat);
                        GridViewPaymentRecent.Rows[i].Cells[1].Value = lPaymentInfo.Account.Name;
                        GridViewPaymentRecent.Rows[i].Cells[2].Value = lPaymentInfo.Reference;
                        GridViewPaymentRecent.Rows[i].Cells[3].Value = lPaymentInfo.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewPaymentRecent.Rows[i].Cells[4].Value = lPaymentInfo.PaymentId;
                        i++;
                    }
                }
            }
            else
            {
                if (ReceiptInfo.Count > 0)
                {
                    GridViewPaymentRecent.Rows.Add(ReceiptInfo.Count);
                    GridViewPaymentRecent.Columns[1].HeaderText = "Customer";
                    int i = 0;
                    foreach (var lReceiptInfo in ReceiptInfo)
                    {
                        GridViewPaymentRecent.Rows[i].Cells[0].Value = lReceiptInfo.TransactionDate.ToString(Global.Company.DateFormat);
                        GridViewPaymentRecent.Rows[i].Cells[1].Value = lReceiptInfo.Account.Name;
                        GridViewPaymentRecent.Rows[i].Cells[2].Value = lReceiptInfo.Reference;
                        GridViewPaymentRecent.Rows[i].Cells[3].Value = lReceiptInfo.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewPaymentRecent.Rows[i].Cells[4].Value = lReceiptInfo.ReceiptId;
                        i++;
                    }
                }
            }
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (GridViewPaymentRecent.Rows.Count > 0)
            {
                if (GridViewPaymentRecent.CurrentRow.Index > -1)
                {
                    if (parent is FormPayment)
                    {
                        //if (PaymentManager.Instance.GetPayment((long)GridViewPaymentRecent.CurrentRow.Cells[4].Value) != null)
                        //{
                        ((FormPayment)parent).SearchPaymentId = (long)GridViewPaymentRecent.CurrentRow.Cells[4].Value;
                        this.Close();
                        //}
                        //else
                        //{
                        //    RecentPaymentErrorMsg.Text = DeletedEntryErrorMsg;
                        //}

                    }
                    else if (parent is FormReceipts)
                    {
                        //if (ReceiptManager.Instance.GetReceipt((long)GridViewPaymentRecent.CurrentRow.Cells[4].Value) != null)
                        //{
                        ((FormReceipts)parent).SearchReceiptId = (long)GridViewPaymentRecent.CurrentRow.Cells[4].Value;
                        this.Close();
                        //}
                        //else
                        //{
                        //    RecentPaymentErrorMsg.Text = DeletedEntryErrorMsg;
                        //}

                    }

                }
                else
                {
                    RecentPaymentErrorMsg.Text = "Please select Invoice";
                }
            }
        }

        private void GridViewPaymentRecent_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                BtnSelect_Click(sender, e);
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
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return true;
            }
            try
            {

                if (keyData == (Keys.Tab) && GridViewPaymentRecent.CurrentRow.Index > -1)
                {
                    if (GridViewPaymentRecent.CurrentCell.RowIndex != GridViewPaymentRecent.Rows.Count - 1)
                    {
                        GridViewPaymentRecent.CurrentCell = GridViewPaymentRecent[0, GridViewPaymentRecent.CurrentCell.RowIndex + 1];
                    }
                    else
                    {
                        BtnSelect.Select();
                    }

                }
                if (keyData == (Keys.Tab | Keys.Shift) && GridViewPaymentRecent.CurrentRow.Index > -1)
                {
                    if (GridViewPaymentRecent.CurrentRow.Index != 0)
                    {
                        GridViewPaymentRecent.CurrentCell = GridViewPaymentRecent[0, GridViewPaymentRecent.CurrentCell.RowIndex];
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

        private void GridViewPaymentRecent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == Convert.ToChar(Keys.Enter))
            {
                BtnSelect_Click(sender, e);
            }
        }

        private void BtnSelect_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                if (GridViewPaymentRecent.Rows.Count > 0)
                {
                    GridViewPaymentRecent.Focus();
                    GridViewPaymentRecent.CurrentCell = GridViewPaymentRecent[0, 0];

                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewPaymentRecent.Rows.Count > 0)
                {
                    GridViewPaymentRecent.Focus();
                    GridViewPaymentRecent.CurrentCell = GridViewPaymentRecent[0, GridViewPaymentRecent.Rows.Count - 1];
                }
            }
        }
    }
}
