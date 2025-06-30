using fa.api.Accounting;
using fa.api.utils;
using fa.model.Accounting.Transaction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.account.transactions
{
    public partial class FormRecentJournal : Form
    {
        public IList<Journal> JournalInfo;
        FormBase parent = null;
        public static string DeletedEntryErrorMsg = "Your selected entry is removed, please reopen the form then select the entry";

        public FormRecentJournal(object sender)
        {
            if (sender is FormJournal)
            {
                parent = (FormJournal)sender;
            }
            InitializeComponent();
        }

        private void FormRecentJournal_Load(object sender, EventArgs e)
        {
            LoadJournalEntry();
            GridViewJournalRecent.Select();
        }
        public void LoadJournalEntry()
        {
            if (parent is FormJournal)
            {
                if (JournalInfo.Count > 0)
                {
                    GridViewJournalRecent.Rows.Add(JournalInfo.Count);
                    int i = 0;
                    foreach (var lJournalInfo in JournalInfo)
                    {
                        GridViewJournalRecent.Rows[i].Cells[0].Value = lJournalInfo.TransactionDate.ToString(Global.Company.DateFormat);
                        GridViewJournalRecent.Rows[i].Cells[1].Value = lJournalInfo.ReferenceNumber;
                        GridViewJournalRecent.Rows[i].Cells[2].Value = lJournalInfo.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewJournalRecent.Rows[i].Cells[3].Value = lJournalInfo.JournalId;
                        i++;
                    }
                }
            }
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (GridViewJournalRecent.Rows.Count > 0)
            {
                if (GridViewJournalRecent.CurrentRow.Index > -1)
                {
                    if (parent is FormJournal)
                    {
                        //if (JournalManager.Instance.GetJournal((long)GridViewJournalRecent.CurrentRow.Cells[3].Value) != null)
                        //{
                        ((FormJournal)parent).SearchJournalId = (long)GridViewJournalRecent.CurrentRow.Cells[3].Value;
                        this.Close();
                        //}
                        //else
                        //{
                        //    ErrorMsg.Text = DeletedEntryErrorMsg;
                        //}
                    }
                }
                else
                {
                    ErrorMsg.Text = "Please select Journal";
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
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return true;
            }
            try
            {

                if (keyData == (Keys.Tab) && GridViewJournalRecent.CurrentRow.Index > -1)
                {
                    if (GridViewJournalRecent.CurrentCell.RowIndex != GridViewJournalRecent.Rows.Count - 1)
                    {
                        GridViewJournalRecent.CurrentCell = GridViewJournalRecent[0, GridViewJournalRecent.CurrentCell.RowIndex + 1];
                    }
                    else
                    {
                        BtnSelect.Select();
                    }

                }
                if (keyData == (Keys.Tab | Keys.Shift) && GridViewJournalRecent.CurrentRow.Index > -1)
                {
                    if (GridViewJournalRecent.CurrentRow.Index != 0)
                    {
                        GridViewJournalRecent.CurrentCell = GridViewJournalRecent[0, GridViewJournalRecent.CurrentCell.RowIndex];
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

        private void BtnSelect_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                if (GridViewJournalRecent.Rows.Count > 0)
                {
                    GridViewJournalRecent.Focus();
                    GridViewJournalRecent.CurrentCell = GridViewJournalRecent[0, 0];

                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewJournalRecent.Rows.Count > 0)
                {
                    GridViewJournalRecent.Focus();
                    GridViewJournalRecent.CurrentCell = GridViewJournalRecent[0, GridViewJournalRecent.Rows.Count - 1];
                }
            }
        }

        private void GridViewJournalRecent_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                BtnSelect_Click(sender, e);
            }
        }

        private void GridViewJournalRecent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == Convert.ToChar(Keys.Enter))
            {
                BtnSelect_Click(sender, e);
            }
        }
    }
}
