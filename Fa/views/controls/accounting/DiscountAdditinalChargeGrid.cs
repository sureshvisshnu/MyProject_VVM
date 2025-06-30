using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using fa.model.Accounting.Masters;
using fa.api.Accounting;
using fa.api.utils;
using fa.model.OrderManagement;
using fa.views.controls.grid;

namespace fa.views.controls.accounting
{
    public enum GridType
    {
        Purchase, Sales
    }
    public enum AddChargeGridColumn
    {
        SNO, NAME, DISCRIPTION, TYPE, VALUE, AMOUNT, ACTION, TOTAL, REMOVE, ID, ACCOUNTID
    }
    public partial class DiscountAdditinalChargeGrid : UserControl
    {
        public DiscountAdditinalChargeGrid()
        {
            InitializeComponent();
        }
        private GridType Result = GridType.Purchase;
        public GridType GridType
        {
            get
            {
                return Result;
            }
            set
            {
                Result = value;
            }
        }
        public void Clear()
        {
            GridView.Rows.Clear();
            InputAmount = 0;
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
        private double _InputAmount = 0;
        public double InputAmount
        {
            get
            {
                return _InputAmount;
            }
            set
            {
                _InputAmount = value;
                CalculateAllDiscountAdditinalCharge();
            }
        }
        private double _OutputAmount = 0;
        public double OutputAmount
        {
            get
            {
                return _OutputAmount;
            }
            set
            {
                _OutputAmount = value;
            }
        }
        private double _Quantity = 0;
        public double Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                _Quantity = value;
            }
        }
        private IList<AdditionalTransaction> _AdditionalTransactions = null;
        public IList<AdditionalTransaction> AdditionalTransactions
        {
            get
            {
                return ListDiscountAdditinalCharge();
            }
            set
            {
                _AdditionalTransactions = value;
                LoadDiscountAdditinalCharge(_AdditionalTransactions);
            }
        }

        private void LoadDiscountAdditinalCharge(IList<AdditionalTransaction> lAdditionalTransactions)
        {
            if (lAdditionalTransactions != null && lAdditionalTransactions.Count > 0)
            {
                GridView.Rows.Clear();
                int i = 0;
                IList<CompanyAdditionalTransactionSetup> TransactionSetup = LoadCompanyAdditionalTransaction();
                GridView.Rows.Add(lAdditionalTransactions.Count);
                foreach (AdditionalTransaction AdditionalTransactions in lAdditionalTransactions)
                {
                    LoadGridCombo(i);
                    GridView.Rows[i].Cells[(int)AddChargeGridColumn.SNO].Value = i + 1;
                    GridView.Rows[i].Cells[(int)AddChargeGridColumn.NAME].Value = AdditionalTransactions.Name;
                    GridView.Rows[i].Cells[(int)AddChargeGridColumn.DISCRIPTION].Value = AdditionalTransactions.DisplayName;
                    GridView.Rows[i].Cells[(int)AddChargeGridColumn.TYPE].Value = ((AdditionalTransactionType)AdditionalTransactions.Type).ToString();
                    GridView.Rows[i].Cells[(int)AddChargeGridColumn.VALUE].Value = AdditionalTransactions.Value.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridView.Rows[i].Cells[(int)AddChargeGridColumn.AMOUNT].Value = AdditionalTransactions.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridView.Rows[i].Cells[(int)AddChargeGridColumn.ID].Value = AdditionalTransactions.Name;
                    GridView.Rows[i].Cells[(int)AddChargeGridColumn.ACCOUNTID].Value = AdditionalTransactions.AccountId;


                    if (AdditionalTransactions.Action == AdditionalTransactionAction.DR)
                    {
                        GridView.Rows[i].Cells[(int)AddChargeGridColumn.ACTION].Value = "+";
                    }
                    else
                    {
                        GridView.Rows[i].Cells[(int)AddChargeGridColumn.ACTION].Value = "-";
                    }

                    if (TransactionSetup.FirstOrDefault(x => x.Name == AdditionalTransactions.Name) == null)
                    {
                        GridView.Rows[i].Cells[(int)AddChargeGridColumn.NAME].ReadOnly = true;
                    }
                    i++;
                }
                ReSequence();
                CalculateAllDiscountAdditinalCharge();
            }
        }
        private void CalculateAllDiscountAdditinalCharge()
        {
            double AdditionalChargeTotal = _InputAmount;
            if (GridView.Rows.Count > 1)
            {
                for (int i = 0; i < GridView.Rows.Count - 1; i++)
                {
                    if (GridView.Rows[i].Cells[(int)AddChargeGridColumn.NAME].Value != null && !string.IsNullOrEmpty(GridView.Rows[i].Cells[(int)AddChargeGridColumn.VALUE].Value.ToString()))
                    {
                        double Value = double.Parse(GridView.Rows[i].Cells[(int)AddChargeGridColumn.VALUE].Value.ToString());
                        if (Value > 0 && _InputAmount > 0)
                        {
                            if (GridView.Rows[i].Cells[(int)AddChargeGridColumn.TYPE].Value.ToString() == "PERCENT")
                            {
                                double NewAmount = _InputAmount * (Value / 100);
                                GridView.Rows[i].Cells[(int)AddChargeGridColumn.AMOUNT].Value = NewAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                if (GridView.Rows[i].Cells[(int)AddChargeGridColumn.ACTION].Value.ToString() == "+")
                                {
                                    AdditionalChargeTotal = AdditionalChargeTotal + NewAmount;
                                }
                                else
                                {
                                    AdditionalChargeTotal = AdditionalChargeTotal - NewAmount;
                                }
                                GridView.Rows[i].Cells[(int)AddChargeGridColumn.TOTAL].Value = AdditionalChargeTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            }
                            else
                            {
                                GridView.Rows[i].Cells[(int)AddChargeGridColumn.AMOUNT].Value = Value.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                if (GridView.Rows[i].Cells[(int)AddChargeGridColumn.ACTION].Value.ToString() == "+")
                                {
                                    AdditionalChargeTotal = AdditionalChargeTotal + Value;
                                }
                                else
                                {
                                    AdditionalChargeTotal = AdditionalChargeTotal - Value;
                                }
                                GridView.Rows[i].Cells[(int)AddChargeGridColumn.TOTAL].Value = AdditionalChargeTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            }
                        }
                        else
                        {
                            GridView.Rows[i].Cells[(int)AddChargeGridColumn.AMOUNT].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                            GridView.Rows[i].Cells[(int)AddChargeGridColumn.TOTAL].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                        }
                    }
                    else
                    {
                        GridView.Rows[i].Cells[(int)AddChargeGridColumn.AMOUNT].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                        GridView.Rows[i].Cells[(int)AddChargeGridColumn.TOTAL].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                    }
                }
            }
            _OutputAmount = AdditionalChargeTotal;
        }
        private IList<AdditionalTransaction> ListDiscountAdditinalCharge()
        {
            IList<AdditionalTransaction> PurchaseAdditionalTransactions = null;
            int Count = GridView.Rows.Count;
            if (Count > 1)
            {
                PurchaseAdditionalTransactions = new List<AdditionalTransaction>();
                for (int i = 0; i < GridView.Rows.Count - 1; i++)
                {
                    AdditionalTransaction lPurchaseAdditionalTransactions = new AdditionalTransaction();
                    lPurchaseAdditionalTransactions.Sequence = i + 1;
                    lPurchaseAdditionalTransactions.Name = GridView.Rows[i].Cells[(int)AddChargeGridColumn.NAME].FormattedValue.ToString();
                    lPurchaseAdditionalTransactions.DisplayName = GridView.Rows[i].Cells[(int)AddChargeGridColumn.DISCRIPTION].Value.ToString();
                    lPurchaseAdditionalTransactions.Type = (AdditionalTransactionType)Enum.Parse(typeof(AdditionalTransactionType), GridView.Rows[i].Cells[(int)AddChargeGridColumn.TYPE].Value.ToString(), true);
                    lPurchaseAdditionalTransactions.Value = double.Parse(GridView.Rows[i].Cells[(int)AddChargeGridColumn.VALUE].Value.ToString());
                    lPurchaseAdditionalTransactions.Amount = double.Parse(GridView.Rows[i].Cells[(int)AddChargeGridColumn.AMOUNT].Value.ToString());
                    lPurchaseAdditionalTransactions.Action = (GridView.Rows[i].Cells[(int)AddChargeGridColumn.ACTION].Value.ToString() == "+") ? AdditionalTransactionAction.DR : AdditionalTransactionAction.CR;
                    lPurchaseAdditionalTransactions.AccountId = (long)GridView.Rows[i].Cells[(int)AddChargeGridColumn.ACCOUNTID].Value;
                    PurchaseAdditionalTransactions.Add(lPurchaseAdditionalTransactions);
                }
            }
            return PurchaseAdditionalTransactions;
        }

        private void GridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridView.Rows[e.RowIndex].Cells[(int)AddChargeGridColumn.SNO].ReadOnly = true;
            GridView.Rows[e.RowIndex].Cells[(int)AddChargeGridColumn.DISCRIPTION].ReadOnly = true;
            GridView.Rows[e.RowIndex].Cells[(int)AddChargeGridColumn.TYPE].ReadOnly = true;
            GridView.Rows[e.RowIndex].Cells[(int)AddChargeGridColumn.VALUE].ReadOnly = true;
            GridView.Rows[e.RowIndex].Cells[(int)AddChargeGridColumn.AMOUNT].ReadOnly = true;
            GridView.Rows[e.RowIndex].Cells[(int)AddChargeGridColumn.ACTION].ReadOnly = true;
            GridView.Rows[e.RowIndex].Cells[(int)AddChargeGridColumn.TOTAL].ReadOnly = true;
            if (GridView.Rows[e.RowIndex].Cells[(int)AddChargeGridColumn.ID].Value != null)
            {
                GridView.Rows[e.RowIndex].Cells[(int)AddChargeGridColumn.DISCRIPTION].ReadOnly = false;
                GridView.Rows[e.RowIndex].Cells[(int)AddChargeGridColumn.VALUE].ReadOnly = false;
            }
            if (e.ColumnIndex == 4)
            {
                GridView.CurrentCell = GridView[(int)AddChargeGridColumn.VALUE, e.RowIndex];
            }
        }


        private void GridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

            if (e.Control is CurrencyEditingControl)
            {
                if (GridView.CurrentCell.ColumnIndex == 4)
                {
                    if (GridView.Rows[GridView.CurrentCell.RowIndex].Cells[(int)AddChargeGridColumn.TYPE].Value != null && GridView.Rows[GridView.CurrentCell.RowIndex].Cells[(int)AddChargeGridColumn.TYPE].Value.ToString() == "PERCENT")
                    {
                        (e.Control as CurrencyEditingControl).CurrencyLength = 6;
                    }
                    else
                    {
                        (e.Control as CurrencyEditingControl).CurrencyLength = 10;
                    }

                }
            }
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
                ((ComboBox)e.Control).KeyDown += GridViewAdditionalCharge_KeyDown1;

                GridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                if (GridView.CurrentCell.ColumnIndex == 1)
                {
                    ((ComboBox)e.Control).SelectedIndexChanged -= new EventHandler(NameColumnComboSelectionChanged);
                    ((ComboBox)e.Control).SelectedIndexChanged += new EventHandler(NameColumnComboSelectionChanged);

                    ((ComboBox)e.Control).TextChanged -= NameColumnComboTextChanged;
                    ((ComboBox)e.Control).TextChanged += NameColumnComboTextChanged;
                }
            }
        }

        private void GridViewAdditionalCharge_KeyDown1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && GridView.CurrentCell.ColumnIndex == (int)AddChargeGridColumn.NAME)
            {
                GridView.Rows[GridView.CurrentRow.Index].Cells[(int)AddChargeGridColumn.NAME].Value = null;
                GridView.Rows[GridView.CurrentRow.Index].Cells[(int)AddChargeGridColumn.ID].Value = null;
                GridView.CommitEdit(DataGridViewDataErrorContexts.Commit);

                DataGridViewComboBoxEditingControl ck = new DataGridViewComboBoxEditingControl();
                ck.SelectedIndex = -1;
                NameColumnComboSelectionChanged(ck, e);
            }

        }
        private void NameColumnComboTextChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == -1 && !string.IsNullOrEmpty(((ComboBox)sender).Text))
            {
                int index = ((ComboBox)sender).FindStringExact(((ComboBox)sender).Text);
                if (index != -1)
                {
                    DataGridViewComboBoxEditingControl ck = (DataGridViewComboBoxEditingControl)sender;
                    ck.SelectedIndex = index;
                    NameColumnComboSelectionChanged(ck, e);
                }
                else
                {
                    GridView.Rows[GridView.CurrentRow.Index].Cells[(int)AddChargeGridColumn.NAME].Value = null;
                    GridView.Rows[GridView.CurrentRow.Index].Cells[(int)AddChargeGridColumn.ID].Value = null;
                    Reset(GridView.CurrentRow.Index);
                    this.OnLoad(e);
                }
            }
            else if (((ComboBox)sender).SelectedIndex != -1)
            {
                NameColumnComboSelectionChanged(sender, e);
            }

        }
        private void NameColumnComboSelectionChanged(object sender, EventArgs e)
        {
            int Index = GridView.CurrentCell.RowIndex;
            if (((ComboBox)sender).SelectedIndex > -1)
            {
                GridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                CompanyAdditionalTransactionSetup CompanyAdditionalTransactionSetup = (CompanyAdditionalTransactionSetup)(((ComboBox)sender).Items[((ComboBox)sender).SelectedIndex]);
                if (GridView.Rows[Index].Cells[(int)AddChargeGridColumn.NAME].Value != null && GridView.Rows[Index].Cells[(int)AddChargeGridColumn.ID].Value != null && CompanyAdditionalTransactionSetup.Name == GridView.Rows[Index].Cells[(int)AddChargeGridColumn.ID].Value.ToString())
                {
                    GridView.Rows[Index].Cells[(int)AddChargeGridColumn.ID].Value = CompanyAdditionalTransactionSetup.Name;
                    return;
                }
                if (GridView.Rows[Index].Cells[(int)AddChargeGridColumn.NAME].Value != null && GridView.Rows[Index].Cells[(int)AddChargeGridColumn.ID].Value != null && GridView.Rows[Index].Cells[(int)AddChargeGridColumn.NAME].Value.ToString() == GridView.Rows[Index].Cells[(int)AddChargeGridColumn.ID].Value.ToString())
                {
                    GridView.Rows[Index].Cells[(int)AddChargeGridColumn.ID].Value = GridView.Rows[Index].Cells[(int)AddChargeGridColumn.NAME].Value;
                    return;
                }

                GridView.Rows[Index].Cells[(int)AddChargeGridColumn.ID].Value = CompanyAdditionalTransactionSetup.Name;

                if (CompanyAdditionalTransactionSetup != null)
                {
                    GridView.Rows[Index].Cells[(int)AddChargeGridColumn.ACCOUNTID].Value = CompanyAdditionalTransactionSetup.AccountId;
                    GridView.Rows[Index].Cells[(int)AddChargeGridColumn.DISCRIPTION].Value = CompanyAdditionalTransactionSetup.DisplayName;
                    GridView.Rows[Index].Cells[(int)AddChargeGridColumn.TYPE].Value = ((AdditionalTransactionType)CompanyAdditionalTransactionSetup.Type).ToString();

                    if (CompanyAdditionalTransactionSetup.TransactionAction == AdditionalTransactionAction.DR)
                    {
                        GridView.Rows[Index].Cells[(int)AddChargeGridColumn.ACTION].Value = "+";
                    }
                    else
                    {
                        GridView.Rows[Index].Cells[(int)AddChargeGridColumn.ACTION].Value = "-";
                    }
                }
                CalculateAllDiscountAdditinalCharge();
                GridView.Rows[Index].Cells[(int)AddChargeGridColumn.VALUE].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                GridView.Rows[Index].Cells[(int)AddChargeGridColumn.AMOUNT].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                GridView.Rows[Index].Cells[(int)AddChargeGridColumn.TOTAL].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);


                GridView.BeginInvoke(new MethodInvoker(delegate ()
                {
                    GridView.CurrentCell = GridView[(int)AddChargeGridColumn.DISCRIPTION, Index];
                    GridView.CurrentCell = GridView[(int)AddChargeGridColumn.NAME, Index];

                }));
            }
            else
            {
                if (GridView.Rows[Index].Cells[(int)AddChargeGridColumn.NAME].Value == null)
                {
                    Reset(Index);
                    this.OnLoad(e);
                }
            }

        }
        private void Reset(int Index)
        {
            GridView.Rows[Index].Cells[(int)AddChargeGridColumn.ACCOUNTID].Value = null;
            GridView.Rows[Index].Cells[(int)AddChargeGridColumn.DISCRIPTION].Value = null;
            GridView.Rows[Index].Cells[(int)AddChargeGridColumn.TYPE].Value = null;
            GridView.Rows[Index].Cells[(int)AddChargeGridColumn.ACTION].Value = null;

            GridView.Rows[Index].Cells[(int)AddChargeGridColumn.VALUE].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            GridView.Rows[Index].Cells[(int)AddChargeGridColumn.AMOUNT].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            GridView.Rows[Index].Cells[(int)AddChargeGridColumn.TOTAL].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            CalculateAllDiscountAdditinalCharge();
        }
        private void GridViewAdditionalCharge_KeyPress1(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)GridView.EditingControl).DroppedDown = false;
        }

        private IList<CompanyAdditionalTransactionSetup> LoadCompanyAdditionalTransaction()
        {
            IList<CompanyAdditionalTransactionSetup> TransactionSetup = null;
            if (GridType == GridType.Purchase)
            {
                TransactionSetup = CompanyManager.Instance.GetPurchaseSetupById(Global.Company.CompanyPurchaseSetupId).AdditionalTransactions;
            }
            else
            {
                TransactionSetup = CompanyManager.Instance.GetCompanySalesSetupWithInclude(Global.Company).AdditionalTransactions;
            }

            return TransactionSetup;
        }
        private void LoadGridCombo(int i)
        {
            (GridView.Rows[i].Cells[(int)AddChargeGridColumn.NAME] as DataGridViewComboBoxCell).DataSource = null;
            (GridView.Rows[i].Cells[(int)AddChargeGridColumn.NAME] as DataGridViewComboBoxCell).DataSource = LoadCompanyAdditionalTransaction();
            (GridView.Rows[i].Cells[(int)AddChargeGridColumn.NAME] as DataGridViewComboBoxCell).ValueMember = "Id";
            (GridView.Rows[i].Cells[(int)AddChargeGridColumn.NAME] as DataGridViewComboBoxCell).DisplayMember = "Name";
        }
        private void GridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            GridView.Rows[e.RowIndex].Cells[(int)AddChargeGridColumn.SNO].Value = GridView.Rows.Count;
            GridView.Rows[e.RowIndex].Cells[(int)AddChargeGridColumn.VALUE].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            GridView.Rows[e.RowIndex].Cells[(int)AddChargeGridColumn.AMOUNT].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            GridView.Rows[e.RowIndex].Cells[(int)AddChargeGridColumn.TOTAL].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            LoadGridCombo(e.RowIndex);
        }
        private void ReSequence()
        {
            for (int i = 0; i < GridView.Rows.Count; i++)
            {
                GridView.Rows[i].Cells[(int)AddChargeGridColumn.SNO].Value = i + 1;
            }
        }
        private void GridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == (int)AddChargeGridColumn.NAME || e.ColumnIndex == (int)AddChargeGridColumn.VALUE)
                {
                    CalculateAllDiscountAdditinalCharge();
                    this.OnLoad(e);
                    if(!string.IsNullOrEmpty(GridView.Rows[e.RowIndex].Cells[(int)AddChargeGridColumn.VALUE].Value.ToString()))
                    {
                        double value = double.Parse(GridView.Rows[e.RowIndex].Cells[(int)AddChargeGridColumn.VALUE].Value.ToString());
                        value.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    }
                }
            }
        }
        string Msg;
        public String ErrorMsg()
        {
            return Msg;
        }
        public Boolean IsDiscountAdditionalChargeValidationResult()
        {
            Msg = string.Empty;
            int Count = GridView.Rows.Count;
            if (Count > 1)
            {
                for (int i = 0; i < Count - 1; i++)
                {
                    for (int j = 1; j < 5; j++)
                    {
                        if (j == (int)AddChargeGridColumn.TYPE) { continue; }
                        if (GridView.Rows[i].Cells[j].Value == null || string.IsNullOrEmpty(GridView.Rows[i].Cells[j].Value.ToString().Trim()) || GridView.Rows[i].Cells[j].Value.Equals(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) || GridView.Rows[i].Cells[j].Value.Equals("0") || GridView.Rows[i].Cells[j].Value.ToString().Trim() == "0")
                        {
                            Msg = "Please Enter Discount/Additional Charge " + GridView.Columns[j].HeaderText;
                            GridView.Select();
                            GridView.BeginInvoke(new MethodInvoker(delegate ()
                            {
                                GridView.CurrentCell = GridView[j, i];
                                GridView.BeginEdit(true);
                            }));
                            return false;
                        }
                        if (j == 4 && GridView.Rows[i].Cells[(int)AddChargeGridColumn.TYPE].Value.ToString() == "PERCENT" &&
                            double.Parse(GridView.Rows[i].Cells[j].Value.ToString()) > 100)
                        {
                            Msg = "Please Enter Discount/Additional Charge Valid Percentage";
                            GridView.Select();
                            GridView.BeginInvoke(new MethodInvoker(delegate ()
                            {
                                GridView.CurrentCell = GridView[j, i];
                                GridView.BeginEdit(true);
                            }));
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private void GridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void GridView_Resize(object sender, EventArgs e)
        {
            SizeChange();
        }
        private void GridView_SizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }
        private void SizeChange()
        {
            GridView.Size = new Size(this.Width, this.Height - 24);
            if (this.Width > 833)
            {
                GridView.Columns[2].Width = 210 + ((this.Width - 833) / 2 + (this.Width - 833) % 2);
                GridView.Columns[1].Width = 100 + ((this.Width - 833) / 2 + (this.Width - 833) % 2);
                //}
                //else
                //{
                //    GridView.Columns[2].Width = GridView.Width - 800;
                //    GridView.Columns[1].Width = GridView.Width - 790;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (keyData == (Keys.Tab) && GridView.CurrentCell.ColumnIndex == (int)AddChargeGridColumn.NAME)
                {
                    GridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    int Index = GridView.CurrentCell.RowIndex;
                    GridView.Select();
                    GridView.BeginInvoke(new MethodInvoker(delegate ()
                    {
                        GridView.CurrentCell = GridView[(int)AddChargeGridColumn.VALUE, Index];
                    }));
                    return false;
                }
                if (keyData == (Keys.Tab) && GridView.CurrentCell.ColumnIndex == (int)AddChargeGridColumn.VALUE)
                {
                    if (GridView.CurrentCell.RowIndex != GridView.Rows.Count - 1)
                    {
                        SendKeys.Send("{tab}{tab}{tab}{tab}{tab}");
                    }
                    else
                    {
                        SendKeys.Send("{tab}{tab}{tab}{tab}");
                    }
                }
                if (keyData == (Keys.Tab | Keys.Shift) && GridView.CurrentCell.ColumnIndex == (int)AddChargeGridColumn.VALUE)
                {
                    SendKeys.Send("{tab}{tab}");
                }
                if (keyData == (Keys.Tab | Keys.Shift) && GridView.CurrentCell.ColumnIndex == (int)AddChargeGridColumn.NAME)
                {
                    if (GridView.CurrentRow.Index != 0)
                    {
                        SendKeys.Send("{tab}{tab}{tab}{tab}{tab}");
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

        private void GridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == (int)AddChargeGridColumn.REMOVE && GridView.Rows.Count - 1 != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete row " + GridView.Rows[e.RowIndex].Cells[(int)AddChargeGridColumn.SNO].Value.ToString() + "?", "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        GridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridView.Rows.RemoveAt(e.RowIndex);
                    }
                    ReSequence();
                    CalculateAllDiscountAdditinalCharge();
                    this.OnLoad(e);
                }
            }
        }
        public bool IsDirty;
        private void DiscountAdditinalChargeGrid_Enter(object sender, EventArgs e)
        {

            GridView.BeginInvoke(new MethodInvoker(delegate ()
            {
                GridView.CurrentCell = GridView[(int)AddChargeGridColumn.NAME, 0];
            }));
        }

        private void GridView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab && GridView.CurrentCell == GridView[0, 0])
            {
                this.OnPreviewKeyDown(e);
            }
        }

        private void GridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && GridView.CurrentCell != null && GridView.CurrentCell.Value != null && !IsDirty)
            {
                IsDirty = true;
                OnTabIndexChanged(e);
            }
            else if (IsDirty)
            {
                IsDirty = false;
                OnTabIndexChanged(e);
            }
        }

        private void DiscountAdditinalChargeGrid_SizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }

        private void GridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                var Acc = LoadCompanyAdditionalTransaction().FirstOrDefault(x => x.Name == GridView.CurrentCell.EditedFormattedValue.ToString());
                if (Acc != null)
                {
                    GridView.CurrentCell.Value = Acc.Id;
                }
                if (GridView.CurrentRow.Cells[1].Value != null && GridView.CurrentRow.Cells[2].Value == null)
                {
                    //SendKeys.Send("+{TAB}");
                }
            }
            if(e.ColumnIndex == 4)
            {
                if (!string.IsNullOrEmpty(GridView.Rows[e.RowIndex].Cells[(int)AddChargeGridColumn.VALUE].Value.ToString()))
                {
                    double value = double.Parse(GridView.Rows[e.RowIndex].Cells[(int)AddChargeGridColumn.VALUE].Value.ToString());
                    value.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                }
            }
        }

        private void GridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}