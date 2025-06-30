using fa.api.utils;
using fa.views.controls.grid;
using System;
using System.Windows.Forms;

namespace fa.views.purchase
{
    public partial class FormBatchEntry : FormBase
    {
        public FormBatchEntry()
        {
            InitializeComponent();
        }
        private void FormBatchEntry_Load(object sender, EventArgs e)
        {
            DataGridViewBatchEntry.Rows.Clear();
            BatchErrorMsg.Text = "";
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)DataGridViewBatchEntry.Columns["Amount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
        }

        private void btnBatchEntrySave_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                this.Close();
            }
        }
        public Boolean Validation()
        {
            BatchErrorMsg.Text = "";
              int Count = DataGridViewBatchEntry.Rows.Count;
            if (Count > 1)
            {
                for (int i = 0; i < Count - 1; i++)
                {
                    for (int j = 1; j < 5; j++)
                    {
                        if (DataGridViewBatchEntry.Rows[i].Cells[j].Value == null || DataGridViewBatchEntry.Rows[i].Cells[j].Value.Equals(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) || DataGridViewBatchEntry.Rows[i].Cells[j].Value.Equals("0"))
                        {
                            DataGridViewBatchEntry.Select();
                            DataGridViewBatchEntry.CurrentCell = DataGridViewBatchEntry[j, i];
                            BatchErrorMsg.Text = "Please enter Proper values";
                            return false;
                        }
                    }
                }
            }
            else
            {
                DataGridViewBatchEntry.CurrentCell = DataGridViewBatchEntry[1, 0];
                BatchErrorMsg.Text = "Please enter Batch Details";
                return false;
            }
            return true;
        }
        private void btnBatchEntryCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
           
            if (keyData == (Keys.F8))
            {
                BtnBatchEntrySave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnBatchEntryCancel.PerformClick();
                return true;
            }
            try
            {
                if (keyData == (Keys.Tab) && DataGridViewBatchEntry.CurrentCell.ColumnIndex == 4)
                {
                    if (DataGridViewBatchEntry.CurrentCell.RowIndex != DataGridViewBatchEntry.Rows.Count - 1)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                    else
                    {
                        SendKeys.Send("{tab}");
                    }
                }
                if (keyData == (Keys.Tab | Keys.Shift) && DataGridViewBatchEntry.CurrentCell.ColumnIndex == 1)
                {
                    if (DataGridViewBatchEntry.CurrentRow.Index != 0)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                    else
                    {
                        SendKeys.Send("{tab}");
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void DataGridViewBatchEntry_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewBatchEntry.Rows[e.RowIndex].Cells[0].ReadOnly = true;
            DataGridViewBatchEntry.Rows[e.RowIndex].Cells[2].ReadOnly = true;
            DataGridViewBatchEntry.Rows[e.RowIndex].Cells[3].ReadOnly = true;
            DataGridViewBatchEntry.Rows[e.RowIndex].Cells[4].ReadOnly = true;
            DataGridViewBatchEntry.Rows[e.RowIndex].Cells[5].ReadOnly = true;
            if (DataGridViewBatchEntry.Rows[e.RowIndex].Cells[1].Value != null)
            {
                DataGridViewBatchEntry.Rows[e.RowIndex].Cells[2].ReadOnly = false;
                DataGridViewBatchEntry.Rows[e.RowIndex].Cells[3].ReadOnly = false;
                DataGridViewBatchEntry.Rows[e.RowIndex].Cells[4].ReadOnly = false;
                DataGridViewBatchEntry.Rows[e.RowIndex].Cells[5].ReadOnly = false;
            }
        }

        private void DataGridViewBatchEntry_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 5 && !DataGridViewBatchEntry.Rows[e.RowIndex].Cells[5].ReadOnly)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete row " + DataGridViewBatchEntry.Rows[e.RowIndex].Cells[0].Value.ToString() + "?", "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        DataGridViewBatchEntry.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        DataGridViewBatchEntry.Rows.RemoveAt(e.RowIndex);
                        ReSequence();
                    }
                }
            }
        }
        private void ReSequence()
        {
            for (int i = 0; i < DataGridViewBatchEntry.Rows.Count; i++)
            {
                DataGridViewBatchEntry.Rows[i].Cells[0].Value = i + 1;
            }
        }

        private void DataGridViewBatchEntry_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewBatchEntry.Rows[e.RowIndex].Cells[0].Value = DataGridViewBatchEntry.Rows.Count;
        }

        private void DataGridViewBatchEntry_Enter(object sender, EventArgs e)
        {
            DataGridViewBatchEntry.CurrentCell = DataGridViewBatchEntry[1, 0];
        }

        private void DataGridViewBatchEntry_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                if (DataGridViewBatchEntry.CurrentCell.Value == null || DataGridViewBatchEntry.CurrentCell.Value.Equals(string.Empty))
                {
                    DataGridViewBatchEntry.CurrentCell.Value = "0";
                }              
            }
        }
    }
}
