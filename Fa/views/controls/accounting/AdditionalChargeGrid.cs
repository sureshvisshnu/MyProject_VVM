using System;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using fa.model.Accounting.Transaction;
using fa.api.utils;

namespace fa.views.controls.accounting
{
    public partial class AdditionalChargeGrid : UserControl
    {
        public AdditionalChargeGrid()
        {
            InitializeComponent();
        }
        public void Clear()
        {
            GridViewAdditionalCharge.Rows.Clear();
        }
        private double _Amount = 0;
        public double Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                _Amount = value;
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

        private double _AmountIncludeDiscount = 0;
        public double AmountIncludeDiscount
        {
            get
            {
                return _AmountIncludeDiscount;
            }
            set
            {
                _AmountIncludeDiscount = value;
                CalculateAllAdditionalCharge();
            }
        }
        //private IList<PurchaseAdditionalCharge> _PurchaseAdditionalCharges = null;
        //public IList<PurchaseAdditionalCharge> PurchaseAdditionalCharges
        //{
        //    get
        //    {
        //        return ListAdditionalCharge();
        //    }
        //    set
        //    {
        //        _PurchaseAdditionalCharges = value;
        //        LoadAdditionalCharges(_PurchaseAdditionalCharges);
        //    }
        //}
        public double GetAdditionalChargeAmount()
        {
            double AdditionalCharge = 0.00;
            if (GridViewAdditionalCharge.Rows.Count > 1)
            {
                for (int i = 0; i < GridViewAdditionalCharge.Rows.Count - 1; i++)
                {
                    if (GridViewAdditionalCharge.Rows[i].Cells[2].Value != null && GridViewAdditionalCharge.Rows[i].Cells[3].Value != null)
                    {
                        float Value = float.Parse(GridViewAdditionalCharge.Rows[i].Cells[3].Value.ToString());
                        if (GridViewAdditionalCharge.Rows[i].Cells[2].Value.ToString() == "PERCENT")
                        {
                            double NewAmount = _Amount * (Value / 100);
                            AdditionalCharge = AdditionalCharge + NewAmount;
                        }
                        else
                        {
                            AdditionalCharge = AdditionalCharge + Value;
                        }
                    }
                }
                return AdditionalCharge;
            }
            return AdditionalCharge;
        }
        private void CalculateAllAdditionalCharge()
        {
            if (GridViewAdditionalCharge.Rows.Count > 1)
            {
                double AdditionalChargeTotal = _AmountIncludeDiscount;
                for (int i = 0; i < GridViewAdditionalCharge.Rows.Count - 1; i++)
                {
                    if (GridViewAdditionalCharge.Rows[i].Cells[2].Value != null && GridViewAdditionalCharge.Rows[i].Cells[3].Value != null)
                    {
                        float Value = float.Parse(GridViewAdditionalCharge.Rows[i].Cells[3].Value.ToString());
                        if (Value > 0 && _Amount > 0)
                        {
                            if (GridViewAdditionalCharge.Rows[i].Cells[2].Value.ToString()== "PERCENT")
                            {
                                double NewAmount = _Amount * (Value / 100);
                                GridViewAdditionalCharge.Rows[i].Cells[4].Value = NewAmount;

                                AdditionalChargeTotal = AdditionalChargeTotal + NewAmount;
                                GridViewAdditionalCharge.Rows[i].Cells[5].Value = AdditionalChargeTotal;
                            }
                            else
                            {
                                GridViewAdditionalCharge.Rows[i].Cells[4].Value = Value;

                                AdditionalChargeTotal = AdditionalChargeTotal + Value;
                                GridViewAdditionalCharge.Rows[i].Cells[5].Value = AdditionalChargeTotal;
                            }
                        }
                    }
                }
            }
        }
        private void GridViewAdditionalCharge_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
                {
                    CalculateAllAdditionalCharge();
                    this.OnLoad(e);
                }
            }
        }

        private void GridViewAdditionalCharge_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                if (GridViewAdditionalCharge.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }
                e.Control.KeyPress += new KeyPressEventHandler(GridViewAdditionalCharge_KeyPress1);
            }
        }
        private void GridViewAdditionalCharge_KeyPress1(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)GridViewAdditionalCharge.EditingControl).DroppedDown = false;
        }

        private void GridViewAdditionalCharge_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            GridViewAdditionalCharge.Rows[e.RowIndex].Cells[0].Value = GridViewAdditionalCharge.Rows.Count;
            var values = from Enum en in Enum.GetValues(typeof(DiscountType)) select new { ID = en, Name = en.ToString() };
            (GridViewAdditionalCharge.Rows[e.RowIndex].Cells[2] as DataGridViewComboBoxCell).DataSource = null;
            (GridViewAdditionalCharge.Rows[e.RowIndex].Cells[2] as DataGridViewComboBoxCell).DataSource = values.ToList();
            (GridViewAdditionalCharge.Rows[e.RowIndex].Cells[2] as DataGridViewComboBoxCell).ValueMember = "ID";
            (GridViewAdditionalCharge.Rows[e.RowIndex].Cells[2] as DataGridViewComboBoxCell).DisplayMember = "Name";
        }
        //private void LoadAdditionalCharges(IList<PurchaseAdditionalCharge> lPurchaseAdditionalCharges)
        //{
        //    if (lPurchaseAdditionalCharges != null)
        //    {
        //        GridViewAdditionalCharge.Rows.Clear();
        //        GridViewAdditionalCharge.Rows.Add(lPurchaseAdditionalCharges.Count);
        //        int i = 0;
        //        foreach (PurchaseAdditionalCharge PurchCharge in lPurchaseAdditionalCharges)
        //        {
        //            GridViewAdditionalCharge.Rows[i].Cells[0].Value = i + 1;
        //            GridViewAdditionalCharge.Rows[i].Cells[1].Value = PurchCharge.ChargeDescription;
        //            var values = from Enum en in Enum.GetValues(typeof(DiscountType)) select new { ID = en, Name = en.ToString() };
        //            (GridViewAdditionalCharge.Rows[i].Cells[2] as DataGridViewComboBoxCell).DataSource = null;
        //            (GridViewAdditionalCharge.Rows[i].Cells[2] as DataGridViewComboBoxCell).DataSource = values.ToList();
        //            (GridViewAdditionalCharge.Rows[i].Cells[2] as DataGridViewComboBoxCell).ValueMember = "ID";
        //            (GridViewAdditionalCharge.Rows[i].Cells[2] as DataGridViewComboBoxCell).DisplayMember = "Name";
        //            GridViewAdditionalCharge.Rows[i].Cells[2].Value = (DiscountType)PurchCharge.ChargeType;
        //            GridViewAdditionalCharge.Rows[i].Cells[3].Value = PurchCharge.Charge;
        //            GridViewAdditionalCharge.Rows[i].Cells[4].Value = PurchCharge.Amount;
        //            i++;
        //        }
        //        ReSequence();
        //        CalculateAllAdditionalCharge();
        //    }
        //}

        //private IList<PurchaseAdditionalCharge> ListAdditionalCharge()
        //{
        //    IList<PurchaseAdditionalCharge> PurchaseAdditionalCharge = null;
        //    int Count = GridViewAdditionalCharge.Rows.Count;
        //    if (Count > 1)
        //    {
        //        PurchaseAdditionalCharge = new List<PurchaseAdditionalCharge>();
        //        for (int i = 0; i < GridViewAdditionalCharge.Rows.Count - 1; i++)
        //        {
        //            PurchaseAdditionalCharge lPurchaseAdditionalCharge = new PurchaseAdditionalCharge();
        //            lPurchaseAdditionalCharge.DiscountSequence = i + 1;
        //            lPurchaseAdditionalCharge.ChargeDescription = GridViewAdditionalCharge.Rows[i].Cells[1].Value.ToString();
        //            lPurchaseAdditionalCharge.ChargeType = (DiscountType)Enum.Parse(typeof(DiscountType), GridViewAdditionalCharge.Rows[i].Cells[2].Value.ToString(), true);
        //            lPurchaseAdditionalCharge.Charge = float.Parse(GridViewAdditionalCharge.Rows[i].Cells[3].Value.ToString());
        //            lPurchaseAdditionalCharge.Amount = float.Parse(GridViewAdditionalCharge.Rows[i].Cells[4].Value.ToString());
        //            PurchaseAdditionalCharge.Add(lPurchaseAdditionalCharge);
        //        }
        //    }
        //    return PurchaseAdditionalCharge;
        //}

        private void GridViewAdditionalCharge_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 6 && !GridViewAdditionalCharge.Rows[e.RowIndex].Cells[6].ReadOnly)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete row " + GridViewAdditionalCharge.Rows[e.RowIndex].Cells[0].Value.ToString() + "?", "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        GridViewAdditionalCharge.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewAdditionalCharge.Rows.RemoveAt(e.RowIndex);
                        ReSequence();
                    }
                }               
            }
        }
        private void ReSequence()
        {
            for (int i = 0; i < GridViewAdditionalCharge.Rows.Count; i++)
            {
                GridViewAdditionalCharge.Rows[i].Cells[0].Value = i + 1;
            }
        }

        private void GridViewAdditionalCharge_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewAdditionalCharge.Rows[e.RowIndex].Cells[0].ReadOnly = true;
            GridViewAdditionalCharge.Rows[e.RowIndex].Cells[2].ReadOnly = true;
            GridViewAdditionalCharge.Rows[e.RowIndex].Cells[3].ReadOnly = true;
            GridViewAdditionalCharge.Rows[e.RowIndex].Cells[4].ReadOnly = true;
            GridViewAdditionalCharge.Rows[e.RowIndex].Cells[5].ReadOnly = true;
            GridViewAdditionalCharge.Rows[e.RowIndex].Cells[6].ReadOnly = true;
            if (GridViewAdditionalCharge.Rows[e.RowIndex].Cells[1].Value != null)
            {
                GridViewAdditionalCharge.Rows[e.RowIndex].Cells[2].ReadOnly = false;
                GridViewAdditionalCharge.Rows[e.RowIndex].Cells[3].ReadOnly = false;
                GridViewAdditionalCharge.Rows[e.RowIndex].Cells[6].ReadOnly = false;
            }
        }
        string ErrorMsg;
        public String AdditionalChargeErrorMsg()
        {
            return ErrorMsg;
        }
        public Boolean IsAdditionalChargeValidationResult()
        {
            ErrorMsg = string.Empty;
            int Count = GridViewAdditionalCharge.Rows.Count;
            if (Count > 1)
            {
                for (int i = 0; i < Count - 1; i++)
                {                   
                    for (int j = 1; j < 4; j++)
                    {
                        if (GridViewAdditionalCharge.Rows[i].Cells[j].Value == null || GridViewAdditionalCharge.Rows[i].Cells[j].Value.Equals(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) || GridViewAdditionalCharge.Rows[i].Cells[j].Value.Equals("0"))
                        {
                            if (j == 1)
                            {
                                ErrorMsg = "Please Enter Additional Charge Description";
                            }
                            else if (j == 2)
                            {
                                ErrorMsg = "Please Enter Additional Charge Type";
                            }
                            else if (j == 3)
                            {
                                ErrorMsg = "Please Enter Additional Charge Value";
                            }
                            GridViewAdditionalCharge.Select();
                            GridViewAdditionalCharge.CurrentCell = GridViewAdditionalCharge[j, i];
                            GridViewAdditionalCharge.BeginEdit(true);

                            return false;
                        }
                    }
                }
            }
            return true;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (keyData == (Keys.Tab) && GridViewAdditionalCharge.CurrentCell.ColumnIndex == 3)
                {
                    if (GridViewAdditionalCharge.CurrentCell.RowIndex != GridViewAdditionalCharge.Rows.Count - 1)
                    {
                        SendKeys.Send("{tab}{tab}{tab}{tab}");
                    }
                    else
                    {
                        SendKeys.Send("{tab}{tab}{tab}");
                    }
                }
                if (keyData == (Keys.Tab | Keys.Shift) && GridViewAdditionalCharge.CurrentCell.ColumnIndex == 1)
                {
                    if (GridViewAdditionalCharge.CurrentRow.Index != 0)
                    {
                        SendKeys.Send("{tab}{tab}{tab}{tab}");
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
        private void AdditionalChargeGrid_Enter(object sender, EventArgs e)
        {
            GridViewAdditionalCharge.CurrentCell = GridViewAdditionalCharge[1, 0];
            GridViewAdditionalCharge.CurrentCell.Selected = true;
        }

        private void GridViewAdditionalCharge_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void GridViewAdditionalCharge_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            this.OnLoad(e);
        }

        private void AdditionalChargeGrid_Resize(object sender, EventArgs e)
        {
            SizeChange();
        }

        private void AdditionalChargeGrid_SizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }
        private void SizeChange()
        {
            GridViewAdditionalCharge.Size = new Size(this.Width - 5, this.Height - 24);
            GridViewAdditionalCharge.Columns[1].Width = GridViewAdditionalCharge.Width - 350;
        }
    }
}
