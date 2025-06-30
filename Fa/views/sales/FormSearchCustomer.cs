using fa.api.Accounting;
using fa.model.Accounting.Masters;
using fa.views.account.masters;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace fa.views.sales
{
    public partial class FormSearchCustomer : Form
    {
        public static string SearchOutput = "No Customer found!";
        public static string NoCustomerfoundErrorMsg = "You do not have any Customer, Please add a Customer!";
        public static string ChooseCustomerErrorMsg = "Please choose Customer";
        public static string EnterCustomerNameErrorMsg = "Please enter Customer Name";
        public static string DeleteAccountErrorMsg = "Your choosen customer  is remove, Please press Go button then choose customer";

        FormBase parent = null;
        public FormSearchCustomer(Object sender)
        {
            parent = (FormBase)sender;
            InitializeComponent();
        }

        private void FormSearchCustomer_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            loadDefaultCustomer();
            TextBoxSearchCustomer.TextBox.Select();
            Cursor.Current = Cursors.Default;
        }
        private void ResetForm()
        {
            TextBoxSearchCustomer.TextBox.ResetText();
            BtnSelect.Enabled = false;
            loadDefaultCustomer();
        }
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (GridViewCustomerSearch.Rows.Count > 0)
            {
                if (GridViewCustomerSearch.CurrentRow.Index > -1)
                {
                    //if (CustomerManager.Instance.GetCustomerById(long.Parse(GridViewCustomerSearch.CurrentRow.Cells[3].Value.ToString())) != null)
                    //{
                    parent.AccountIdTransport.ResetText();
                    parent.AccountIdTransport.Text = GridViewCustomerSearch.CurrentRow.Cells[3].Value.ToString();
                    this.Close();
                    //}
                    //else
                    //{
                    //    SearchErrorMsg.Text = DeleteAccountErrorMsg;
                    //}
                }
                else
                {
                    SearchErrorMsg.Text = ChooseCustomerErrorMsg;
                }
            }
        }
        private void BtnSearchCustomer_Click(object sender, EventArgs e)
        {
            SearchErrorMsg.Text = "";
            if (!string.IsNullOrEmpty(TextBoxSearchCustomer.Text.Trim()))
            {
                IList<Customer> Customer = CustomerManager.Instance.ListCustomerByName(TextBoxSearchCustomer.Text, Global.Company.CompanyId);
                LoadCustomers(Customer);
            }
            else
            {
                SearchErrorMsg.Text = EnterCustomerNameErrorMsg;
            }
        }
        private void loadDefaultCustomer()
        {

            IList<Customer> Customer = CustomerManager.Instance.ListCustomerByCompanyId(Global.Company.CompanyId);
            if (Customer.Count > 0)
            {
                LoadCustomers(Customer);
            }
            else
            {
                SearchErrorMsg.Text = NoCustomerfoundErrorMsg;
            }
        }
        private void LoadCustomers(IList<Customer> Customers)
        {
            GridViewCustomerSearch.Rows.Clear();
            BtnSelect.Enabled = false;
            if (Customers.Count > 0)
            {
                GridViewCustomerSearch.Rows.Add(Customers.Count);
                int i = 0;
                foreach (Customer lCustomer in Customers)
                {
                    GridViewCustomerSearch.Rows[i].Cells[0].Value = lCustomer.Name;
                    GridViewCustomerSearch.Rows[i].Cells[1].Value = lCustomer.BillingAddress.FullAddressInSingleLine;
                    if (lCustomer.ContactInfo != null)
                    {
                        GridViewCustomerSearch.Rows[i].Cells[2].Value = lCustomer.ContactInfo.Phone;
                    }
                    GridViewCustomerSearch.Rows[i].Cells[3].Value = lCustomer.Id;

                    i++;
                }
                BtnSelect.Enabled = true;
            }
            else
            {
                SearchErrorMsg.Text = SearchOutput;
            }
        }
        private void BtnNewCustomer_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormCustomers FormCustomer = new FormCustomers(this);
            FormCustomer.CreateCustomerOnLoad = true;
            FormCustomer.ShowDialog(this);
            if (string.IsNullOrEmpty(TextBoxSearchCustomer.Text))
            {
                loadDefaultCustomer();
            }
            else
            {
                BtnSearchCustomer_Click(sender, e);
            }
            Cursor.Current = Cursors.Default;
        }

        private void GridViewCustomerSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                BtnSelect_Click(sender, e);
            }
        }
        private bool GridFocus = false;
        private void GridViewCustomerSearch_Leave(object sender, EventArgs e)
        {
            GridFocus = false;
        }

        private void GridViewCustomerSearch_Enter(object sender, EventArgs e)
        {
            GridFocus = true;
        }

        private void GridViewCustomerSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == Convert.ToChar(Keys.Enter))
            {
                BtnSelect_Click(sender, e);
            }
        }

        private void TextBoxSearchCustomer_TextChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (string.IsNullOrEmpty(TextBoxSearchCustomer.Text))
            {
                loadDefaultCustomer();
            }
            Cursor.Current = Cursors.Default;
        }

        private void TextBoxSearchCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearchCustomer_Click(sender, e);
            }
            if (e.KeyCode == Keys.Down)
            {
                if (GridViewCustomerSearch.Rows.Count > 0)
                {
                    GridViewCustomerSearch.Select();
                    GridViewCustomerSearch.CurrentCell = GridViewCustomerSearch[0, 0];
                }
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
                BtnNewCustomer.PerformClick();
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
                    if (keyData == (Keys.Tab) && GridViewCustomerSearch.CurrentRow.Index > -1)
                    {
                        if (GridViewCustomerSearch.CurrentCell.RowIndex != GridViewCustomerSearch.Rows.Count - 1)
                        {
                            GridViewCustomerSearch.CurrentCell = GridViewCustomerSearch[0, GridViewCustomerSearch.CurrentCell.RowIndex + 1];
                        }
                        else
                        {
                            if (GridViewCustomerSearch.CurrentCell.ColumnIndex == 2)
                            {
                                SendKeys.Send("{+tab}");
                            }
                            BtnSelect.Select();
                        }

                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewCustomerSearch.CurrentRow.Index > -1)
                    {
                        if (GridViewCustomerSearch.CurrentRow.Index != 0)
                        {
                            GridViewCustomerSearch.CurrentCell = GridViewCustomerSearch[0, GridViewCustomerSearch.CurrentCell.RowIndex];
                        }
                        else
                        {
                            TextBoxSearchCustomer.TextBox.Select();

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
                TextBoxSearchCustomer.TextBox.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewCustomerSearch.Rows.Count > 0)
                {
                    GridViewCustomerSearch.Select();
                    GridViewCustomerSearch.CurrentCell = GridViewCustomerSearch[0, 0];
                }
                else
                {
                    TextBoxSearchCustomer.TextBox.Select();
                }
            }
        }
    }
}
