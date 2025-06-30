using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using fa.model.Accounting.Masters;
using fa.api.Accounting;

namespace fa.views.controls.accounting
{
    public enum TransactionalGridColumn
    {
        NAME, DISPLAYNAME, ACCOUNT, TYPE, ACTION, REMOVE, ID
    }
    public partial class MiscellaneousTranscationalGrid : UserControl
    {
        public MiscellaneousTranscationalGrid()
        {
            InitializeComponent();
        }
        public string HeaderText
        {
            get
            {
                return Header.Text;
            }
            set
            {
                Header.Text = value;
            }
        }
        long? _CompanyId;
        public long? CompanyId
        {
            get
            {
                return _CompanyId;
            }
            set
            {
                _CompanyId = value;
            }
        }
        public void Clear()
        {
            GridView.Rows.Clear();
        }
        public IList<CompanyAdditionalTransactionSetup> CompanyTransactionSetups
        {
            get
            {
                return ListAdditionalTransaction();
            }
            set
            {
                if (value != null)
                {
                    LoadAdditionalTransaction(value);
                }
            }
        }
        private IList<CompanyAdditionalTransactionSetup> ListAdditionalTransaction()
        {
            IList<CompanyAdditionalTransactionSetup> AdditionalTransactions = null;
            if (GridView.Rows.Count > 0)
            {
                AdditionalTransactions = new List<CompanyAdditionalTransactionSetup>();
                for (int i = 0; i < GridView.Rows.Count - 1; i++)
                {
                    CompanyAdditionalTransactionSetup CompanyAdditionalTransactionSetup = new CompanyAdditionalTransactionSetup();
                    CompanyAdditionalTransactionSetup.Name = GridView.Rows[i].Cells[(int)TransactionalGridColumn.NAME].Value.ToString().Trim();
                    CompanyAdditionalTransactionSetup.DisplayName = GridView.Rows[i].Cells[(int)TransactionalGridColumn.DISPLAYNAME].Value != null ? ((string)GridView.Rows[i].Cells[(int)TransactionalGridColumn.DISPLAYNAME].Value).Trim() : String.Empty;
                    CompanyAdditionalTransactionSetup.AccountId = (long)GridView.Rows[i].Cells[(int)TransactionalGridColumn.ACCOUNT].Value;
                    CompanyAdditionalTransactionSetup.Type = (AdditionalTransactionType)Enum.Parse(typeof(AdditionalTransactionType), GridView.Rows[i].Cells[(int)TransactionalGridColumn.TYPE].Value.ToString(), true);
                    CompanyAdditionalTransactionSetup.TransactionAction = (AdditionalTransactionAction)Enum.Parse(typeof(AdditionalTransactionAction), GridView.Rows[i].Cells[(int)TransactionalGridColumn.ACTION].Value.ToString(), true);
                    CompanyAdditionalTransactionSetup.Id = (GridView.Rows[i].Cells[(int)TransactionalGridColumn.ID].Value != null) ? (long)GridView.Rows[i].Cells[(int)TransactionalGridColumn.ID].Value : 0L;
                    AdditionalTransactions.Add(CompanyAdditionalTransactionSetup);
                }
            }
            return AdditionalTransactions;
        }
        private void LoadAdditionalTransaction(IList<CompanyAdditionalTransactionSetup> lCompanyAdditionalTransactionSetup)
        {
            if (lCompanyAdditionalTransactionSetup != null && lCompanyAdditionalTransactionSetup.Count > 0)
            {
                GridView.Rows.Clear();
                GridView.Rows.Add(lCompanyAdditionalTransactionSetup.Count);
                int j = 0;
                foreach (var MiscellaneousTransSales in lCompanyAdditionalTransactionSetup)
                {
                    LoadMiscellaneousTransCombo(j);
                    GridView.Rows[j].Cells[(int)TransactionalGridColumn.NAME].Value = MiscellaneousTransSales.Name;
                    GridView.Rows[j].Cells[(int)TransactionalGridColumn.DISPLAYNAME].Value = MiscellaneousTransSales.DisplayName;
                    GridView.Rows[j].Cells[(int)TransactionalGridColumn.ACCOUNT].Value = MiscellaneousTransSales.AccountId;
                    GridView.Rows[j].Cells[(int)TransactionalGridColumn.TYPE].Value = (AdditionalTransactionType)MiscellaneousTransSales.Type;
                    GridView.Rows[j].Cells[(int)TransactionalGridColumn.ACTION].Value = (AdditionalTransactionAction)MiscellaneousTransSales.TransactionAction;
                    GridView.Rows[j].Cells[(int)TransactionalGridColumn.REMOVE].Value = "X";
                    GridView.Rows[j].Cells[(int)TransactionalGridColumn.ID].Value = MiscellaneousTransSales.Id;
                    j++;
                }

            }
        }
        private void LoadMiscellaneousTransCombo(int Index)
        {
            if (CompanyId != null)
            {
                IList<Account> Account = AccountManager.Instance.ListAccountByCompanyId((long)CompanyId);
                if (Account != null)
                {
                    (GridView.Rows[Index].Cells[(int)TransactionalGridColumn.ACCOUNT] as DataGridViewComboBoxCell).DataSource = null;
                    (GridView.Rows[Index].Cells[(int)TransactionalGridColumn.ACCOUNT] as DataGridViewComboBoxCell).DataSource = Account;
                    (GridView.Rows[Index].Cells[(int)TransactionalGridColumn.ACCOUNT] as DataGridViewComboBoxCell).ValueMember = "Id";
                    (GridView.Rows[Index].Cells[(int)TransactionalGridColumn.ACCOUNT] as DataGridViewComboBoxCell).DisplayMember = "Name";

                    var Type = from Enum en in Enum.GetValues(typeof(AdditionalTransactionType)) select new { ID = en, Name = en.ToString() };
                    (GridView.Rows[Index].Cells[(int)TransactionalGridColumn.TYPE] as DataGridViewComboBoxCell).DataSource = null;
                    (GridView.Rows[Index].Cells[(int)TransactionalGridColumn.TYPE] as DataGridViewComboBoxCell).DataSource = Type.ToList();
                    (GridView.Rows[Index].Cells[(int)TransactionalGridColumn.TYPE] as DataGridViewComboBoxCell).ValueMember = "ID";
                    (GridView.Rows[Index].Cells[(int)TransactionalGridColumn.TYPE] as DataGridViewComboBoxCell).DisplayMember = "Name";

                    var Action = from Enum en in Enum.GetValues(typeof(AdditionalTransactionAction)) select new { ID = en, Name = en.ToString() };
                    (GridView.Rows[Index].Cells[(int)TransactionalGridColumn.ACTION] as DataGridViewComboBoxCell).DataSource = null;
                    (GridView.Rows[Index].Cells[(int)TransactionalGridColumn.ACTION] as DataGridViewComboBoxCell).DataSource = Action.ToList();
                    (GridView.Rows[Index].Cells[(int)TransactionalGridColumn.ACTION] as DataGridViewComboBoxCell).ValueMember = "ID";
                    (GridView.Rows[Index].Cells[(int)TransactionalGridColumn.ACTION] as DataGridViewComboBoxCell).DisplayMember = "Name";
                }
            }
        }
        private void MiscellaneousTranscationalGrid_Resize(object sender, EventArgs e)
        {
            SizeChange();
        }

        private void MiscellaneousTranscationalGrid_SizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }
        private void SizeChange()
        {
            GridView.Size = new Size(this.Width - 5, this.Height - 24);
            //if (this.Width > 750)
            //{
            GridView.Columns[1].Width = GridView.Width - 550;
            //}
        }

        private void GridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void MiscellaneousTranscationalGrid_Enter(object sender, EventArgs e)
        {
            GridView.CurrentCell = GridView[0, 0];
        }
        private void GridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            LoadMiscellaneousTransCombo(e.RowIndex);
        }

        private void GridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == (int)TransactionalGridColumn.REMOVE && !GridView.Rows[e.RowIndex].IsNewRow)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete row " + (e.RowIndex + 1).ToString()/* GridView.Rows[e.RowIndex].Cells[(int)TransactionalGridColumn.NAME].Value.ToString()*/ + "?", "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        GridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridView.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
        }
        string Msg;
        public String ErrorMsg()
        {
            IsValidationResult();
            return Msg;
        }
        public Boolean IsValidationResult()
        {
            Msg = string.Empty;
            if (GridView.Rows.Count > 1)
            {
                for (int i = 0; i < GridView.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (j == 1)
                        {
                            continue;
                        }
                        if (GridView.Rows[i].Cells[j].Value == null || string.IsNullOrEmpty(GridView.Rows[i].Cells[j].Value.ToString().Trim()))
                        {
                            Msg = "Please enter " + GridView.Columns[j].HeaderText;
                            GridView.Select();
                            GridView.BeginInvoke(new MethodInvoker(delegate ()
                            {
                                GridView.CurrentCell = GridView[j, i];
                                GridView.BeginEdit(true);
                            }));
                            return false;
                        }
                        int Count = 0;
                        for (int k = 0; k < GridView.Rows.Count - 1; k++)
                        {
                            if (GridView.Rows[k].Cells[0].Value != null)
                            {
                                if (GridView.Rows[i].Cells[0].Value.ToString().Trim() == GridView.Rows[k].Cells[0].Value.ToString().Trim())
                                {
                                    Count++;
                                }
                            }
                            if (Count > 1)
                            {
                                Msg = "Name " + GridView.Rows[k].Cells[0].Value.ToString().Trim() + " Repeated";
                                GridView.Select();
                                GridView.BeginInvoke(new MethodInvoker(delegate ()
                                {
                                    GridView.CurrentCell = GridView[0, k];
                                    GridView.BeginEdit(true);
                                }));
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        public void FocusCurrentCell()
        {
            GridView.CurrentCell = GridView[GridView.CurrentCell.ColumnIndex, GridView.CurrentRow.Index];
            GridView.BeginEdit(true);
        }

        private void GridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                if (GridView.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }
                e.Control.KeyPress += new KeyPressEventHandler(GridViewAdditionalCharge_KeyPress1);
            }
        }
        private void GridViewAdditionalCharge_KeyPress1(object sender, KeyPressEventArgs e)
        {
            if (GridView.CurrentCell.ColumnIndex == 3 || GridView.CurrentCell.ColumnIndex == 4)
            {
                e.KeyChar = Convert.ToChar(e.KeyChar.ToString().ToUpper());
            }
            ((ComboBox)GridView.EditingControl).DroppedDown = false;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (keyData == (Keys.Tab) && GridView.CurrentCell.ColumnIndex == (int)TransactionalGridColumn.ACTION)
                {
                    SendKeys.Send("{tab}");
                }
                if (keyData == (Keys.Tab | Keys.Shift) && GridView.CurrentCell.ColumnIndex == (int)TransactionalGridColumn.NAME)
                {
                    if (GridView.CurrentRow.Index != 0)
                    {
                        SendKeys.Send("{tab}");
                    }
                    else
                    {
                        PreviewKeyDownEventArgs args = new PreviewKeyDownEventArgs(keyData);
                        OnPreviewKeyDown(args);
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void GridView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (GridView.Rows.Count - 1 == GridView.CurrentCell.RowIndex && (GridView.CurrentCell.ColumnIndex - 1) == (int)TransactionalGridColumn.ACTION)
            {
                OnPreviewKeyDown(e);
            }
        }

        private void GridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridView.Rows[e.RowIndex].Cells[(int)TransactionalGridColumn.DISPLAYNAME].ReadOnly = true;
            GridView.Rows[e.RowIndex].Cells[(int)TransactionalGridColumn.ACCOUNT].ReadOnly = true;
            GridView.Rows[e.RowIndex].Cells[(int)TransactionalGridColumn.TYPE].ReadOnly = true;
            GridView.Rows[e.RowIndex].Cells[(int)TransactionalGridColumn.ACTION].ReadOnly = true;
            GridView.Rows[e.RowIndex].Cells[(int)TransactionalGridColumn.REMOVE].ReadOnly = true;

            if (GridView.Rows[e.RowIndex].Cells[(int)TransactionalGridColumn.NAME].Value != null)
            {
                GridView.Rows[e.RowIndex].Cells[(int)TransactionalGridColumn.DISPLAYNAME].ReadOnly = false;
                GridView.Rows[e.RowIndex].Cells[(int)TransactionalGridColumn.ACCOUNT].ReadOnly = false;
                GridView.Rows[e.RowIndex].Cells[(int)TransactionalGridColumn.TYPE].ReadOnly = false;
                GridView.Rows[e.RowIndex].Cells[(int)TransactionalGridColumn.ACTION].ReadOnly = false;
                GridView.Rows[e.RowIndex].Cells[(int)TransactionalGridColumn.REMOVE].ReadOnly = false;
            }
        }

        private void GridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                Account Acc = AccountManager.Instance.GetAccountByName(GridView.CurrentCell.EditedFormattedValue.ToString(), Global.Company.CompanyId);
                if (Acc != null)
                {
                    GridView.CurrentCell.Value = Acc.Id;

                }
                GridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                return;
            }
            if (e.ColumnIndex == 3)
            {
                var Type = from Enum en in Enum.GetValues(typeof(AdditionalTransactionType)) select new { ID = en, Name = en.ToString() };
                var Acc = Type.FirstOrDefault(x => x.Name == GridView.CurrentCell.EditedFormattedValue.ToString().ToUpper());
                if (Acc != null)
                {
                    GridView.CurrentCell.Value = Acc.ID;
                }
            }
            if (e.ColumnIndex == 4)
            {
                var Action = from Enum en in Enum.GetValues(typeof(AdditionalTransactionAction)) select new { ID = en, Name = en.ToString() };
                var Acc = Action.FirstOrDefault(x => x.Name == GridView.CurrentCell.EditedFormattedValue.ToString().ToUpper());
                if (Acc != null)
                {
                    GridView.CurrentCell.Value = Acc.ID;
                }
            }
        }
    }
}
