using fa.api.Accounting;
using fa.model.Accounting.Masters;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace fa.views.account.masters
{
    public partial class FormSearchSupplier : FormBase
    {
        public static string SearchOutput = "No Supplier found!";
        public static string NoSupplierfoundErrorMsg = "You do not have any Supplier, Please add a Supplier!";
        public static string ChooseSupplierErrorMsg = "Please choose Supplier";
        public static string EnterSupplierNameErrorMsg = "Please enter Supplier Name";
        public static string DeleteAccountErrorMsg = "Your choosen supplier  is remove, Please press Go button then choose supplier";

        FormBase parent = null;
        public FormSearchSupplier(object sender)
        {
            parent = (FormBase)sender;
            InitializeComponent();
        }

        private void FormSearchSupplier_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            loadDefaultSupplier();
            TextBoxSearchSupplier.TextBox.Select();
            Cursor.Current = Cursors.Default;
        }

        private void ResetForm()
        {
            TextBoxSearchSupplier.TextBox.ResetText();
            BtnSelect.Enabled = false;
            loadDefaultSupplier();
        }
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (GridViewSupplierSearch.Rows.Count > 0)
            {
                if (GridViewSupplierSearch.CurrentRow.Index > -1)
                {
                    //if (SupplierManager.Instance.GetSupplierById(long.Parse(GridViewSupplierSearch.CurrentRow.Cells[3].Value.ToString())) != null)
                    //{
                        parent.AccountIdTransport.ResetText();
                        parent.AccountIdTransport.Text = GridViewSupplierSearch.CurrentRow.Cells[3].Value.ToString();
                        this.Close();
                    //}
                    //else
                    //{
                    //    SearchErrorMsg.Text = DeleteAccountErrorMsg;
                    //}
                }
                else
                {
                    SearchErrorMsg.Text =ChooseSupplierErrorMsg;
                }
            }
        }

       
        private void BtnSearchSupplier_Click(object sender, EventArgs e)
        {
            SearchErrorMsg.Text = "";
            if (!string.IsNullOrEmpty(TextBoxSearchSupplier.Text.Trim()))
            {
                IList<Supplier> Supplier = SupplierManager.Instance.ListSupplierByName(TextBoxSearchSupplier.Text,Global.Company.CompanyId);
                LoadSuppliers(Supplier);
            }
            else
            {
                SearchErrorMsg.Text = EnterSupplierNameErrorMsg;
            }
        }
        
        private void loadDefaultSupplier()
        {
            IList<Supplier> Supplier = SupplierManager.Instance.ListSupplierByCompanyId(Global.Company.CompanyId);
            if (Supplier.Count > 0)
            {
                LoadSuppliers(Supplier);
            }
            else
            {
                SearchErrorMsg.Text = NoSupplierfoundErrorMsg;
            }
        }
        private void LoadSuppliers(IList<Supplier> Suppliers)
        {
            GridViewSupplierSearch.Rows.Clear();
            BtnSelect.Enabled = false;
            if (Suppliers.Count > 0)
            {
                GridViewSupplierSearch.Rows.Add(Suppliers.Count);
                int i = 0;
                foreach (Supplier lSupplier in Suppliers)
                {
                    GridViewSupplierSearch.Rows[i].Cells[0].Value = lSupplier.Name;
                    GridViewSupplierSearch.Rows[i].Cells[1].Value = lSupplier.Address.FullAddressInSingleLine;
                    if (lSupplier.ContactInfo != null)
                    {
                        GridViewSupplierSearch.Rows[i].Cells[2].Value = lSupplier.ContactInfo.Phone;
                    }
                    GridViewSupplierSearch.Rows[i].Cells[3].Value = lSupplier.Id;

                    i++;
                }
                BtnSelect.Enabled = true;
            }
            else
            {
                SearchErrorMsg.Text = SearchOutput;
            }
        }
        private void BtnNewSupplier_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormSupplier FormSupplier = new FormSupplier(this);
            FormSupplier.CreateSupplierOnLoad = true;
            FormSupplier.ShowDialog(this);
            if (string.IsNullOrEmpty(TextBoxSearchSupplier.Text))
            {
                loadDefaultSupplier();
            }
            else
            {
                BtnSearchSupplier_Click(sender, e);
            }
            Cursor.Current = Cursors.Default;
        }
        private void GridViewSupplierSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex > -1)
            {
                BtnSelect_Click(sender, e);
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSelect.PerformClick();
            }
            if (keyData == (Keys.F3))
            {
                BtnNewSupplier.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return true;
            }
            try
            {
                if (GridFocus)
                {
                    if (keyData == (Keys.Tab) && GridViewSupplierSearch.CurrentRow.Index > -1)
                    {
                        if (GridViewSupplierSearch.CurrentCell.RowIndex != GridViewSupplierSearch.Rows.Count - 1)
                        {
                            GridViewSupplierSearch.CurrentCell = GridViewSupplierSearch[0, GridViewSupplierSearch.CurrentCell.RowIndex + 1];
                        }
                        else
                        {
                            if (GridViewSupplierSearch.CurrentCell.ColumnIndex == 2)
                            {
                                SendKeys.Send("{+tab}");
                            }
                            BtnSelect.Select();
                        }

                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewSupplierSearch.CurrentRow.Index > -1)
                    {
                        if (GridViewSupplierSearch.CurrentRow.Index != 0)
                        {
                            GridViewSupplierSearch.CurrentCell = GridViewSupplierSearch[0, GridViewSupplierSearch.CurrentCell.RowIndex];
                        }
                        else
                        {                          
                            TextBoxSearchSupplier.TextBox.Select();
                            if (GridViewSupplierSearch.CurrentCell.ColumnIndex == 0)
                            {
                                TextBoxSearchSupplier.Focus();
                            }
                        }
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
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxSearchSupplier.TextBox.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewSupplierSearch.Rows.Count > 0)
                {
                    GridViewSupplierSearch.Select();
                    GridViewSupplierSearch.CurrentCell = GridViewSupplierSearch[0, 0];
                }
                else
                {
                    TextBoxSearchSupplier.TextBox.Select();
                }
            }
        }
        private void TextBoxSearchSupplier_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearchSupplier_Click(sender, e);
            }
            if (e.KeyCode == Keys.Down)
            {
                if (GridViewSupplierSearch.Rows.Count > 0)
                {
                    GridViewSupplierSearch.Select();
                    GridViewSupplierSearch.CurrentCell= GridViewSupplierSearch[0,0];
                }
            }
        }

        private void TextBoxSearchSupplier_TextChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (string.IsNullOrEmpty(TextBoxSearchSupplier.Text))
            {
                loadDefaultSupplier();
            }
            Cursor.Current = Cursors.Default;
        }





        private bool GridFocus = false;
        private void GridViewSupplierSearch_Leave(object sender, EventArgs e)
        {
            GridFocus = false;
        }
        private void GridViewSupplierSearch_Enter(object sender, EventArgs e)
        {
            GridFocus = true; ;
        }

        private void GridViewSupplierSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == Convert.ToChar(Keys.Enter))
            {           
                BtnSelect_Click(sender, e);
            }
        }
    }
}
