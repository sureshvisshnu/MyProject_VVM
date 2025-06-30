using fa.api.Accounting;
using fa.api.Accounting.Transactions;
using fa.api.utils;
using fa.model.Accounting.Transactions;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace fa.views.account.transactions
{
    public partial class FormRecentDebitNote : Form
    {
        public IList<DebitNote> DebitNoteInfo;
        public IList<CreditNote> CreditNoteInfo;
        public static string DeletedEntryErrorMsg = "Your selected entry is removed, please reopen the form then select the entry";
        FormBase parent = null;
        public FormRecentDebitNote(Object sender)
        {
            if (sender is FormDebitNote)
            {
                parent = (FormDebitNote)sender;
            }
            else if (sender is FormCreditNote)
            {
                parent = (FormCreditNote)sender;
            }
            InitializeComponent();
        }

        private void FormRecentDebitNote_Load(object sender, EventArgs e)
        {
            LoadPurchaseEntry();
            GridViewDebitNoteRecentInvoice.Select();
        }
        public void LoadPurchaseEntry()
        {
            if (parent is FormDebitNote)
            {
                if (DebitNoteInfo.Count > 0)
                {
                    GridViewDebitNoteRecentInvoice.Rows.Add(DebitNoteInfo.Count);
                    int i = 0;
                    foreach (var lDebitNoteInfo in DebitNoteInfo)
                    {
                        GridViewDebitNoteRecentInvoice.Rows[i].Cells[0].Value = lDebitNoteInfo.TransactionDate.ToString(Global.Company.DateFormat);
                        GridViewDebitNoteRecentInvoice.Rows[i].Cells[1].Value = lDebitNoteInfo.Account.Name;
                        GridViewDebitNoteRecentInvoice.Rows[i].Cells[2].Value = lDebitNoteInfo.ReferenceNumber;
                        GridViewDebitNoteRecentInvoice.Rows[i].Cells[3].Value = lDebitNoteInfo.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewDebitNoteRecentInvoice.Rows[i].Cells[4].Value = lDebitNoteInfo.DebitNoteId;
                        i++;
                    }
                }
            }
            else
            {
                if (CreditNoteInfo.Count > 0)
                {
                    GridViewDebitNoteRecentInvoice.Rows.Add(CreditNoteInfo.Count);
                    GridViewDebitNoteRecentInvoice.Columns[1].HeaderText = "Customer";
                    int i = 0;
                    foreach (var lCreditNoteInfo in CreditNoteInfo)
                    {
                        GridViewDebitNoteRecentInvoice.Rows[i].Cells[0].Value = lCreditNoteInfo.TransactionDate.ToString(Global.Company.DateFormat);
                        GridViewDebitNoteRecentInvoice.Rows[i].Cells[1].Value = lCreditNoteInfo.Account.Name;
                        GridViewDebitNoteRecentInvoice.Rows[i].Cells[2].Value = lCreditNoteInfo.ReferenceNumber;
                        GridViewDebitNoteRecentInvoice.Rows[i].Cells[3].Value = lCreditNoteInfo.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewDebitNoteRecentInvoice.Rows[i].Cells[4].Value = lCreditNoteInfo.CreditNoteId;
                        i++;
                    }
                }
            }
        }

        private void GridViewDebitNoteRecentInvoice_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (GridViewDebitNoteRecentInvoice.Rows.Count > 0)
            {
                if (GridViewDebitNoteRecentInvoice.CurrentRow.Index > -1)
                {

                    if (parent is FormDebitNote)
                    {
                        //if(DebitNoteManager.Instance.GetDebitNote((long)GridViewDebitNoteRecentInvoice.CurrentRow.Cells[4].Value)!=null)
                        //{
                        ((FormDebitNote)parent).SearchDebitNoteId = (long)GridViewDebitNoteRecentInvoice.CurrentRow.Cells[4].Value;
                        this.Close();
                        //}
                        //else
                        //{
                        //RecentPurchaseErrorMsg.Text = DeletedEntryErrorMsg;
                        //}
                    }
                    else if (parent is FormCreditNote)
                    {
                        //if(CreditNoteManager.Instance.GetCreditNote((long)GridViewDebitNoteRecentInvoice.CurrentRow.Cells[4].Value) != null)
                        //{
                        ((FormCreditNote)parent).SearchCreditNoteId = (long)GridViewDebitNoteRecentInvoice.CurrentRow.Cells[4].Value;
                        this.Close();
                        //}
                        //else
                        //{
                        //RecentPurchaseErrorMsg.Text = DeletedEntryErrorMsg;

                        //}
                    }

                }
                else
                {
                    RecentPurchaseErrorMsg.Text = "Please select Invoice";
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

                if (keyData == (Keys.Tab) && GridViewDebitNoteRecentInvoice.CurrentRow.Index > -1)
                {
                    if (GridViewDebitNoteRecentInvoice.CurrentCell.RowIndex != GridViewDebitNoteRecentInvoice.Rows.Count - 1)
                    {
                        GridViewDebitNoteRecentInvoice.CurrentCell = GridViewDebitNoteRecentInvoice[0, GridViewDebitNoteRecentInvoice.CurrentCell.RowIndex + 1];
                    }
                    else
                    {
                        BtnSelect.Select();
                    }

                }
                if (keyData == (Keys.Tab | Keys.Shift) && GridViewDebitNoteRecentInvoice.CurrentRow.Index > -1)
                {
                    if (GridViewDebitNoteRecentInvoice.CurrentRow.Index != 0)
                    {
                        GridViewDebitNoteRecentInvoice.CurrentCell = GridViewDebitNoteRecentInvoice[0, GridViewDebitNoteRecentInvoice.CurrentCell.RowIndex];
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

        private void GridViewDebitNoteRecentInvoice_KeyDown(object sender, KeyEventArgs e)
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
                if (GridViewDebitNoteRecentInvoice.Rows.Count > 0)
                {
                    GridViewDebitNoteRecentInvoice.Focus();
                    GridViewDebitNoteRecentInvoice.CurrentCell = GridViewDebitNoteRecentInvoice[0, 0];

                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewDebitNoteRecentInvoice.Rows.Count > 0)
                {
                    GridViewDebitNoteRecentInvoice.Focus();
                    GridViewDebitNoteRecentInvoice.CurrentCell = GridViewDebitNoteRecentInvoice[0, GridViewDebitNoteRecentInvoice.Rows.Count - 1];

                }
            }
        }
    }
}
