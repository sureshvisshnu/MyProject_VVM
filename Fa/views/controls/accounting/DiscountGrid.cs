using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using fa.model.OrderManagement;
using fa.model.Accounting.Transaction;
using fa.api.utils;

namespace fa.views.controls.accounting
{
    public partial class DiscountGrid : UserControl
    {
        public DiscountGrid()
        {
            InitializeComponent();
        }
        public void Clear()
        {
            GridViewDiscount.Rows.Clear();
        }
        public int RowCount
        {
            get
            {
                return GridViewDiscount.Rows.Count;
            }
        }
        
        private double _Amount =0;
        public double Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                _Amount = value;
                CalculateAllDiscount();
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
        private IList<DiscountDetail> _PurchaseDiscounts = null;
        public IList<DiscountDetail> PurchaseDiscounts
        {
            get
            {
                return ListDiscount();
            }
            set
            {
                _PurchaseDiscounts = value;
                LoadDiscounts(_PurchaseDiscounts);
            }
        }

        public double GetDiscountAmount()
        {
            double discount = 0.00;
            if (GridViewDiscount.Rows.Count > 1)
            {
                for (int i = 0; i < GridViewDiscount.Rows.Count - 1; i++)
                {
                    if (GridViewDiscount.Rows[i].Cells[2].Value != null && GridViewDiscount.Rows[i].Cells[3].Value != null)
                    {
                        float Value = float.Parse(GridViewDiscount.Rows[i].Cells[3].Value.ToString());
                        if (GridViewDiscount.Rows[i].Cells[2].Value.ToString()=="PERCENT")
                        {
                            double NewAmount = _Amount * (Value / 100);
                            discount = discount + NewAmount;
                        }
                        else
                        {
                            discount = discount + Value;
                        }
                    }
                }
                return discount;
            }
            return discount;
        }
        private void CalculateAllDiscount()
        {
            if(GridViewDiscount.Rows.Count>1)
            {
                double DiscountTotal = _Amount;
                for(int i=0;i< GridViewDiscount.Rows.Count-1;i++)
                {
                    if (GridViewDiscount.Rows[i].Cells[2].Value != null && GridViewDiscount.Rows[i].Cells[3].Value != null)
                    {
                        float Value = float.Parse(GridViewDiscount.Rows[i].Cells[3].Value.ToString());
                        if (Value > 0 && _Amount > 0)
                        {
                            if (GridViewDiscount.Rows[i].Cells[2].Value.ToString() == "PERCENT")
                            {
                                double NewAmount = _Amount * (Value / 100);
                                GridViewDiscount.Rows[i].Cells[4].Value = NewAmount;

                                DiscountTotal = DiscountTotal - NewAmount;
                                GridViewDiscount.Rows[i].Cells[5].Value = DiscountTotal;
                            }
                            else
                            {
                                GridViewDiscount.Rows[i].Cells[4].Value = Value;

                                DiscountTotal = DiscountTotal - Value;
                                GridViewDiscount.Rows[i].Cells[5].Value = DiscountTotal;
                            }
                        }
                    }
                }


            }      
        }
        
        private void GridViewDiscount_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
                {
                    CalculateAllDiscount();
                    this.OnLoad(e);
                }
            }
        }
        
        private void GridViewDiscount_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                if (GridViewDiscount.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }
                e.Control.KeyPress += new KeyPressEventHandler(GridViewDiscount_KeyPress1);
            }
        }
        private void GridViewDiscount_KeyPress1(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)GridViewDiscount.EditingControl).DroppedDown = false;
        }
       
        private void GridViewDiscount_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            GridViewDiscount.Rows[e.RowIndex].Cells[0].Value= GridViewDiscount.Rows.Count;
            var values = from Enum en in Enum.GetValues(typeof(DiscountType)) select new { ID = en, Name = en.ToString() };
            (GridViewDiscount.Rows[e.RowIndex].Cells[2] as DataGridViewComboBoxCell).DataSource = null;
            (GridViewDiscount.Rows[e.RowIndex].Cells[2] as DataGridViewComboBoxCell).DataSource = values.ToList();
            (GridViewDiscount.Rows[e.RowIndex].Cells[2] as DataGridViewComboBoxCell).ValueMember = "ID";
            (GridViewDiscount.Rows[e.RowIndex].Cells[2] as DataGridViewComboBoxCell).DisplayMember = "Name";
        }
        private void LoadDiscounts(IList<DiscountDetail> lPurchaseDiscounts)
        {
            if (lPurchaseDiscounts != null)
            {
                GridViewDiscount.Rows.Clear();
                int i = 0;
                foreach (DiscountDetail PurchDisc in lPurchaseDiscounts)
                {
                    if (!string.IsNullOrEmpty(PurchDisc.DiscountDescription))
                    {
                        GridViewDiscount.Rows.Add(1);
                        GridViewDiscount.Rows[i].Cells[0].Value = i + 1;
                        GridViewDiscount.Rows[i].Cells[1].Value = PurchDisc.DiscountDescription;
                        GridViewDiscount.Rows[i].Cells[2].Value = ((DiscountType)PurchDisc.DisccountType).ToString();
                        GridViewDiscount.Rows[i].Cells[3].Value = PurchDisc.Discount;
                        GridViewDiscount.Rows[i].Cells[4].Value = PurchDisc.DiscountAmount;
                        i++;
                    }
                }
                ReSequence();
                CalculateAllDiscount();
            }
        }

        private IList<DiscountDetail> ListDiscount()
        {
            IList<DiscountDetail> PurchaseDiscount = null;
            int Count = GridViewDiscount.Rows.Count;
            if (Count > 1)
            {
               PurchaseDiscount = new List<DiscountDetail>();
                for (int i = 0; i < GridViewDiscount.Rows.Count - 1; i++)
                {
                    DiscountDetail lPurchaseDiscount = new DiscountDetail();
                    lPurchaseDiscount.DiscountSequence = i+1;
                    lPurchaseDiscount.DiscountDescription = GridViewDiscount.Rows[i].Cells[1].Value.ToString();
                    lPurchaseDiscount.DisccountType =(DiscountType)Enum.Parse(typeof(DiscountType), GridViewDiscount.Rows[i].Cells[2].Value.ToString(), true);
                    lPurchaseDiscount.Discount =float.Parse(GridViewDiscount.Rows[i].Cells[3].Value.ToString());
                    lPurchaseDiscount.DiscountAmount =float.Parse(GridViewDiscount.Rows[i].Cells[4].Value.ToString());
                    PurchaseDiscount.Add(lPurchaseDiscount);
                }
            }
            return PurchaseDiscount;
        }

        private void GridViewDiscount_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 6 && !GridViewDiscount.Rows[e.RowIndex].Cells[6].ReadOnly)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete row " + GridViewDiscount.Rows[e.RowIndex].Cells[0].Value.ToString() + "?", "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        GridViewDiscount.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewDiscount.Rows.RemoveAt(e.RowIndex);
                        ReSequence();
                    }
                }
            }
        }
        private void ReSequence()
        {
            for (int i = 0; i < GridViewDiscount.Rows.Count; i++)
            {
                GridViewDiscount.Rows[i].Cells[0].Value = i + 1;
            }
        }
        private void GridViewDiscount_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewDiscount.Rows[e.RowIndex].Cells[0].ReadOnly = true;
            GridViewDiscount.Rows[e.RowIndex].Cells[2].ReadOnly = true;
            GridViewDiscount.Rows[e.RowIndex].Cells[3].ReadOnly = true;
            GridViewDiscount.Rows[e.RowIndex].Cells[4].ReadOnly = true;
            GridViewDiscount.Rows[e.RowIndex].Cells[5].ReadOnly = true;
            GridViewDiscount.Rows[e.RowIndex].Cells[6].ReadOnly = true;
            if (GridViewDiscount.Rows[e.RowIndex].Cells[1].Value != null)
            {
                GridViewDiscount.Rows[e.RowIndex].Cells[2].ReadOnly = false;
                GridViewDiscount.Rows[e.RowIndex].Cells[3].ReadOnly = false;
                GridViewDiscount.Rows[e.RowIndex].Cells[6].ReadOnly = false;
            }
        }
        string ErrorMsg;
        public String DiscountErrorMsg()
        {
            return ErrorMsg;
        }
        public Boolean IsDiscountValidationResult()
        {
            ErrorMsg = string.Empty;
            int Count = GridViewDiscount.Rows.Count;
            if (Count > 1)
            {
                for (int i = 0; i < Count - 1; i++)
                {
                    for (int j = 1; j < 4; j++)
                    {
                        if (GridViewDiscount.Rows[i].Cells[j].Value == null || GridViewDiscount.Rows[i].Cells[j].Value.Equals(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) || GridViewDiscount.Rows[i].Cells[j].Value.Equals("0"))
                        {
                            if(j==1)
                            {
                                ErrorMsg = "Please Enter Discount Description";
                            }
                            else if(j==2)
                            {
                                ErrorMsg = "Please Enter Discount Type";
                            }
                            else if (j == 3)
                            {
                                ErrorMsg = "Please Enter Discount Value";
                            }
                            GridViewDiscount.Select();
                            GridViewDiscount.CurrentCell = GridViewDiscount[j, i];
                            GridViewDiscount.BeginEdit(true);
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
                if (keyData == (Keys.Tab) && GridViewDiscount.CurrentCell.ColumnIndex == 3)
                {
                    if (GridViewDiscount.CurrentCell.RowIndex != GridViewDiscount.Rows.Count - 1)
                    {
                        SendKeys.Send("{tab}{tab}{tab}{tab}");
                    }
                    else
                    {
                        SendKeys.Send("{tab}{tab}{tab}");
                    }
                }            
                if (keyData == (Keys.Tab | Keys.Shift) && GridViewDiscount.CurrentCell.ColumnIndex == 1)
                {
                    if (GridViewDiscount.CurrentRow.Index != 0)
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

        private void DiscountGrid_Enter(object sender, EventArgs e)
        {
            GridViewDiscount.CurrentCell = GridViewDiscount[1, 0];
            GridViewDiscount.CurrentCell.Selected = true;
        }

        private void GridViewDiscount_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void GridViewDiscount_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            this.OnLoad(e);
        }

        private void DiscountGrid_Resize(object sender, EventArgs e)
        {
            SizeChange();
        }

        private void DiscountGrid_SizeChanged(object sender, EventArgs e)
        {
            SizeChange();          
        }
        private void SizeChange()
        {
            GridViewDiscount.Size = new Size(this.Width - 5, this.Height - 24);
            GridViewDiscount.Columns[1].Width = GridViewDiscount.Width - 350;
        }
    }
}
