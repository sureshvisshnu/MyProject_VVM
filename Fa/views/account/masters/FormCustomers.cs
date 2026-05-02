using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using fa.api.Accounting;
using fa.model.Accounting.Masters;
using fa.model.Common;
using fa.libraries.Validation;
using fa.libraries.utils;
using fa.api.utils;
using fa.views.controls;
using System.Linq;
using fa.model.UserProfile;
using VisioForge.MediaFramework.Helpers;

namespace fa.views.account.masters
{
    public enum CustomerFormTaxInfoTableColumn
    {
        NAME, DNAME, VALUE, INVOICE, REPORT, REMOVE, ID, MASTERID, REQUIR
    }
    public partial class FormCustomers : FormBase
    {
        public static string EnterEmailErrorMsg = "Please enter valid email.";
        public static string EnterWebsiteErrorMsg = "Please enter valid Website.";
        public static string SaveSuccessText = "Save success...";
        public static string DeleteSuccessText = "Delete success...";
        public static string CreateCustomerOnloadText = "New Customer";
        public static string NoCustomerFoundText = "No Customer Found!";
        public static string UpdateCustomerOnloadText = "Update Customer";
        public static string DeleteConfirmText = "Do you want to delete the customer {0}?";
        public static string DeleteErrorText = "Could not delete the customer!, please retry";
        public static string IsbranchDeleteErrorText = "Could not delete customer with sub customer";
        public static string CancelConfirmText = "There are unsaved changes, do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, do you want exit?";
        public static string UniqueCustomerNameErrorMsg = "Customer {0} already exists in company {1}";
        public static string EnterCustomerNameErrorMsg = "Please enter customer name";
        public static string ChooseParentErrorMsg = "Please select parent customer";
        public static string EnterAsOfDateErrorMsg = "Invalid as of date";
        public static string ChoosePaymentMethodErrorMsg = "Please select payment method";
        public static string ChoosePaymentTermErrorMsg = "Please select payment term";
        public static string EnterPaymentLimitErrorMsg = "Invalid credit limit value";
        public static string ChooseBalanceTypeErrorMsg = "Please select balance Type";
        public static string EnterValidAsOfDateErrorMsg = "Please enter valid as of date";
        public static string ChooseStateErrorMsg = "Please choose state";
        public static string TaxInfoGrid_MantatoryFiledErrorMsg = "Please enter licence information if any! ";

        public bool CreateCustomerOnLoad = false;
        public string? ParentId;
        public string? Id;
        CustomerManager CustomerManager = null!;
        PaymentMethodManager PaymentMethodManager = null!;
        PaymentTermManager PaymentTermManager = null!;
        StateManager StateManager = null!;
        CountryManager CountryManager = null!;
        AddressManager AddressManager = null!;
        ContactInfoManager ContactInfoManager = null!;
        KeypressValidation KeypressValidation = null!;
        DateValidation DateValidation = null!;
        FormBase parent = null!;

        public FormCustomers(object sender)
        {
            if (sender is FormBase)
            {
                parent = (FormBase)sender;
            }
            CustomerManager = CustomerManager.Instance;
            PaymentMethodManager = PaymentMethodManager.Instance;
            PaymentTermManager = PaymentTermManager.Instance;
            StateManager = StateManager.Instance;
            CountryManager = CountryManager.Instance;
            AddressManager = AddressManager.Instance;
            ContactInfoManager = ContactInfoManager.Instance;
            KeypressValidation = KeypressValidation.Instance;
            DateValidation = DateValidation.Instance;
            InitializeComponent();
            excludedObjects = new string[] { "TextBoxCustomerSearch" };

        }
        private void FormCustomers_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                EnableForm(false);
                LoadComboBox();
                LoadCustomersWithFilter();
                BillingAddressGroupBoxCustomer.StateId = (long)Global.Company.Address.StatesId!;
                ShippingAddressGroupBoxCustomer.StateId = (long)Global.Company.Address.StatesId!;
                if (CreateCustomerOnLoad)
                {
                    this.BtnCustomerEdit.Visible = false;
                    BtnCustomerCancel.Visible = false;
                    BtnCustomerNew.Visible = false;
                    BtnCustomerDelete.Visible = false;
                    TreeViewCustomer.Visible = false;
                    TabControlCustomer.Location = new Point(12, 12);
                    BtnCustomerSave.Location = new Point(TabControlCustomer.Width - 74, TabControlCustomer.Height + 15);
                    this.Size = new Size(TabControlCustomer.Right + 25, TabControlCustomer.Bottom + 90);
                    this.CenterToParent();
                    if (Id == null)
                    {
                        this.Text = CreateCustomerOnloadText;
                        BtnCustomerNew_Click(this, null!);
                    }
                    else
                    {
                        this.Text = UpdateCustomerOnloadText;
                        TreeNode TreeNode = new TreeNode();
                        TreeNode = ParentId == "" ? TreeViewCustomer.Nodes[Id] : TreeViewCustomer.Nodes[ParentId].Nodes[Id];
                        TreeViewCustomer.SelectedNode = TreeNode;
                        BtnCustomerEdit_Click(this, null!);
                    }
                }
                if (TreeViewCustomer.Nodes.Count > 0)
                {
                    ToolStripStatusLabelErrorCustomer.Text = "";
                }
                this.formIsDirty = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        private void LoadComboBox()
        {
            ComboUtils.InitializePaymentMethodCombo(ComboBoxCustomerPaymentMethod, Global.Company.CompanyId);
            ComboUtils.InitializePaymentTermCombo(ComboBoxCustomerPaymentTerm, Global.Company.CompanyId);
            ComboUtils.InitializeCustomerCombo(ComboBoxCustomerParentAccount, Global.Company.CompanyId);
            ResetLicenseInfo();
        }
        private void ResetLicenseInfo()
        {
            int LicenseTypeCount = Global.Company.CompanyCustomerLicenseMaster.Count;
            if (LicenseTypeCount > 0)
            {
                int i = 0;
                CustomerLicenceInfoGrid.Rows.Clear();
                CustomerLicenceInfoGrid.AllowUserToAddRows = false;
                CustomerLicenceInfoGrid.Rows.Add(LicenseTypeCount);
                foreach (CompanyCustomerLicenseMaster CompanyCustomerLicenseMaster in Global.Company.CompanyCustomerLicenseMaster)
                {

                    CustomerLicenceInfoGrid.Rows[i].Cells[(int)CustomerFormTaxInfoTableColumn.NAME].Value = CompanyCustomerLicenseMaster.Name;
                    CustomerLicenceInfoGrid.Rows[i].Cells[(int)CustomerFormTaxInfoTableColumn.DNAME].Value = CompanyCustomerLicenseMaster.DisplayName;
                    CustomerLicenceInfoGrid.Rows[i].Cells[(int)CustomerFormTaxInfoTableColumn.INVOICE].Value = CompanyCustomerLicenseMaster.IncludeInInvoice;
                    CustomerLicenceInfoGrid.Rows[i].Cells[(int)CustomerFormTaxInfoTableColumn.REPORT].Value = CompanyCustomerLicenseMaster.IncludeInReport;
                    CustomerLicenceInfoGrid.Rows[i].Cells[(int)CustomerFormTaxInfoTableColumn.MASTERID].Value = CompanyCustomerLicenseMaster.Id;
                    CustomerLicenceInfoGrid.Rows[i].Cells[(int)CustomerFormTaxInfoTableColumn.REQUIR].Value = CompanyCustomerLicenseMaster.Required;

                    i++;
                }
            }
        }

        private void LoadCustomersWithFilter()
        {
            string FilterString = TextBoxCustomerSearch.Text.Trim();
            TreeViewCustomer.Nodes.Clear();
            IList<Customer> Customers = CustomerManager.ListCustomerByCompanyId(Global.Company.CompanyId);
            if (Customers != null)
            {
                foreach (var lCustomer in Customers)
                {
                    if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim()) || lCustomer.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        if (lCustomer.ParentAccountId == null)
                        {
                            TreeViewCustomer.Nodes.Add(lCustomer.Id.ToString(), lCustomer.Name);
                            TreeViewCustomer.Nodes[lCustomer.Id.ToString()].ImageIndex = 0;
                            foreach (var llCustomer in Customers)
                            {
                                if (lCustomer.Id == llCustomer.ParentAccountId)
                                {
                                    if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim()) || llCustomer.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                                    {
                                        TreeViewCustomer.Nodes[lCustomer.Id.ToString()].Nodes.Add(llCustomer.Id.ToString(), llCustomer.Name);
                                        TreeViewCustomer.Nodes[lCustomer.Id.ToString()].Nodes[llCustomer.Id.ToString()].ImageIndex = 0;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            if (TreeViewCustomer.Nodes.Count > 0)
            {
                if (!string.IsNullOrEmpty(TextBoxCustomerSearch.Text))
                {
                    TreeViewCustomer.ExpandAll();
                }
                TreeViewCustomer.SelectedNode = TreeViewCustomer.Nodes[0];
                TextBoxCustomerSearch.Select();
            }
            else
            {
                BtnCustomerNew.Select();
            }
        }
        private Customer GetCustomerInfo()
        {
            if (TreeViewCustomer.SelectedNode != null)
            {
                Customer lCustomer = CustomerManager.GetCustomerById(long.Parse(TreeViewCustomer.SelectedNode.Name));
                if (lCustomer != null)
                {
                    Customer CustomerFromDB = CustomerManager.GetCustomerById(lCustomer.Id);
                    return CustomerFromDB;
                }
            }
            return null!;
        }
        private void LoadCustomerInfo()
        {
            Customer CustomerFromDB = GetCustomerInfo();
            if (CustomerFromDB != null)
            {
                TextBoxCustomerAccountId.Text = CustomerFromDB.Id.ToString();
                TextBoxCustomerName.Text = CustomerFromDB.Name;
                TextBoxCustomerDisplayAs.Text = CustomerFromDB.DisplayAs;
                TextBoxCustomerDescription.Text = CustomerFromDB.Discription;
                CheckBoxCustomerIsbranch.Checked = CustomerFromDB.IsSubAccount;
                TextBoxCustomerNameonCheck.Text = CustomerFromDB.DisplayNameOnCheck;
                CheckBoxUseDisplayName.Checked = CustomerFromDB.Name == CustomerFromDB.DisplayNameOnCheck ? true : false;
                CheckBoxBillByPrevious.Checked = CustomerFromDB.BillWithPreviousPrice;
                DateTimePickerCustomer.Date = (DateTime)DateUtils.ToDate(CustomerFromDB.BalanceAsOf.Date.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                TextBoxCustomerBalance.Text = Math.Abs(CustomerFromDB.Balance).ToString(TextUtils.DecimalPlace(TextBoxCustomerBalance.Decimals));
                ComboBoxBalanceType.SelectedIndex = CustomerFromDB.Balance < 0 ? 1 : 0;
                BillingAddressGroupBoxCustomer.CountryId = (long)Global.Company.CountryId!;
                ShippingAddressGroupBoxCustomer.CountryId = (long)Global.Company.CountryId;
                Address BillingAddress = AddressManager.GetAddressById((long)CustomerFromDB.BillingAddressId!);
                if (BillingAddress != null)
                {
                    BillingAddressGroupBoxCustomer.AddressLine1 = BillingAddress.AddressLine1;
                    BillingAddressGroupBoxCustomer.AddressLine2 = BillingAddress.AddressLine2;
                    BillingAddressGroupBoxCustomer.CityName = BillingAddress.CityOrTown;
                    BillingAddressGroupBoxCustomer.DistrictName = BillingAddress.District;
                    BillingAddressGroupBoxCustomer.PinCode = BillingAddress.PinCode;
                    BillingAddressGroupBoxCustomer.StateId = BillingAddress.StatesId != null ? (long)BillingAddress.StatesId : 0L;

                }
                if (CustomerFromDB.ShippingAddressId != null)
                {
                    Address ShippingAddress = AddressManager.GetAddressById((long)CustomerFromDB.ShippingAddressId);
                    if (ShippingAddress != null)
                    {
                        ShippingAddressGroupBoxCustomer.ReadOnly = true;
                        CheckBoxCustomerUseBillingAddress.Enabled = false;
                        CheckBoxCustomerUseBillingAddress.Checked = true;
                        ShippingAddressGroupBoxCustomer.AddressLine1 = ShippingAddress.AddressLine1;
                        ShippingAddressGroupBoxCustomer.AddressLine2 = ShippingAddress.AddressLine2;

                        ShippingAddressGroupBoxCustomer.CityName = ShippingAddress.CityOrTown;
                        ShippingAddressGroupBoxCustomer.DistrictName = ShippingAddress.District;
                        ShippingAddressGroupBoxCustomer.PinCode = ShippingAddress.PinCode;
                        ShippingAddressGroupBoxCustomer.StateId = ShippingAddress.StatesId != null ? (long)ShippingAddress.StatesId : 0L; ;
                    }
                }

                if (CustomerFromDB.ContactInfo != null)
                {
                    ContactInfo lContactInfo = CustomerFromDB.ContactInfo;
                    TextBoxCustomerPhone.Text = lContactInfo.Phone;
                    TextBoxCustomerMobile.Text = lContactInfo.Mobile;
                    TextBoxCustomerFax.Text = lContactInfo.Fax;
                    TextBoxCustomerEmail.Text = lContactInfo.Email;
                    TextBoxCustomerWebsite.Text = lContactInfo.WebSite;
                }
                TextBoxCustomerNameonCheck.Text = CustomerFromDB.DisplayNameOnCheck;
                if (CustomerFromDB.PaymentMethodId != null)
                {
                    PaymentMethod lPaymentMethod = PaymentMethodManager.GetPaymentMethodById((long)CustomerFromDB.PaymentMethodId);
                    if (lPaymentMethod != null)
                    {
                        ComboBoxCustomerPaymentMethod.SelectedIndex = ComboBoxCustomerPaymentMethod.FindStringExact(lPaymentMethod.Name);
                    }
                }
                if (CustomerFromDB.PaymentTermId != null)
                {
                    PaymentTerm lPaymentTerm = PaymentTermManager.GetPaymentTermById((long)CustomerFromDB.PaymentTermId);
                    if (lPaymentTerm != null)
                    {
                        ComboBoxCustomerPaymentTerm.SelectedIndex = ComboBoxCustomerPaymentTerm.FindStringExact(lPaymentTerm.Name);
                    }
                }
                string PaymentLimit = string.IsNullOrEmpty(CustomerFromDB.PaymentLimit.ToString()) ? "0.00" : CustomerFromDB.PaymentLimit.ToString()!;
                TextBoxCustomerPaymentLimit.Text = Math.Round(double.Parse(PaymentLimit)).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                RadioCustomerLockBill.Checked = CustomerFromDB.LockBill;
                if (CustomerFromDB.IsSubAccount)
                {
                    Customer Customer = CustomerManager.GetCustomerById((long)CustomerFromDB.ParentAccountId!);
                    if (Customer != null)
                    {
                        ComboUtils.InitializeCustomerCombo(ComboBoxCustomerParentAccount, Global.Company.CompanyId);
                        ComboBoxCustomerParentAccount.SelectedIndex = ComboBoxCustomerParentAccount.FindStringExact(Customer.Name);
                    }
                }
                else
                {
                    ComboBoxCustomerParentAccount.SelectedIndex = -1;
                }

                if (CustomerFromDB.CustomerLicenceDetail.Count > 0)
                {
                    foreach (DataGridViewRow Row in CustomerLicenceInfoGrid.Rows)
                    {
                        CustomerLicenceDetail CustomerLicenceDetail = CustomerFromDB.CustomerLicenceDetail.FirstOrDefault(x => x.CompanyCustomerLicenseMasterId == (long)Row.Cells[7].Value)!;
                        if (CustomerLicenceDetail != null)
                        {
                            Row.Cells[(int)CustomerFormTaxInfoTableColumn.DNAME].Value = CustomerLicenceDetail.DisplayName;
                            Row.Cells[(int)CustomerFormTaxInfoTableColumn.VALUE].Value = CustomerLicenceDetail.Value;
                            Row.Cells[(int)CustomerFormTaxInfoTableColumn.ID].Value = CustomerLicenceDetail.CustomerLicenceId;
                        }
                    }
                }
            }
            else
            {
                return;
            }
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            this.formIsDirty = false;
            LoadCustomersWithFilter();
            return;
        }
        private Customer GetCustomerFromForm()
        {
            Customer lCustomer = new Customer();
            if (!string.IsNullOrEmpty(TextBoxCustomerAccountId.Text))
            {
                lCustomer.Id = Convert.ToInt64(TextBoxCustomerAccountId.Text);
            }
            else
            {
                lCustomer.Id = 0L;
            }
            lCustomer.Name = TextBoxCustomerName.Text.Trim();
            lCustomer.Discription = TextBoxCustomerDescription.Text.Trim();
            lCustomer.DisplayAs = TextBoxCustomerDisplayAs.Text.Trim();
            lCustomer.CompanyId = Global.Company.CompanyId;
            float Balance = string.IsNullOrEmpty(TextBoxCustomerBalance.Text) ? 0 : float.Parse(TextBoxCustomerBalance.Text);
            lCustomer.Balance = (Balance != 0 && ComboBoxBalanceType.SelectedIndex == 1) ? -Balance : Balance;
            lCustomer.BalanceAsOf = (DateTime)DateTimePickerCustomer.Date!;
            lCustomer.AccountType = AccountType.CUSTOMER;
            lCustomer.GSTNo = TextBoxGSTNo.Text.Trim();
            lCustomer.BillWithPreviousPrice = CheckBoxBillByPrevious.Checked;
            if (CheckBoxCustomerIsbranch.Checked == true)
            {
                if (ComboBoxCustomerParentAccount.SelectedIndex > -1)
                {
                    Customer Customer = (Customer)ComboBoxCustomerParentAccount.Items[ComboBoxCustomerParentAccount.SelectedIndex];
                    if (Customer != null)
                    {
                        Customer = CustomerManager.GetCustomerById(Customer.Id);
                        if (Customer != null)
                        {
                            lCustomer.ParentAccountId = Customer.Id;
                            lCustomer.IsSubAccount = true;
                        }
                    }
                }
            }
            else
            {
                lCustomer.IsSubAccount = false;
            }
            lCustomer.Notes = TextBoxCustomerDescription.Text.Trim();
            lCustomer.DisplayNameOnCheck = TextBoxCustomerNameonCheck.Text.Trim();
            if (ComboBoxCustomerPaymentMethod.SelectedIndex > -1)
            {
                PaymentMethod PaymentMethod = (PaymentMethod)ComboBoxCustomerPaymentMethod.Items[ComboBoxCustomerPaymentMethod.SelectedIndex];
                if (PaymentMethod != null)
                {
                    PaymentMethod = PaymentMethodManager.GetPaymentMethodById(PaymentMethod.Id);
                    if (PaymentMethod != null)
                    {
                        lCustomer.PaymentMethodId = PaymentMethod.Id;
                    }
                }
            }
            if (ComboBoxCustomerPaymentTerm.SelectedIndex > -1)
            {
                PaymentTerm PaymentTerm = (PaymentTerm)ComboBoxCustomerPaymentTerm.Items[ComboBoxCustomerPaymentTerm.SelectedIndex];
                if (PaymentTerm != null)
                {
                    PaymentTerm = PaymentTermManager.GetPaymentTermById(PaymentTerm.Id);
                    if (PaymentTerm != null)
                    {
                        lCustomer.PaymentTermId = PaymentTerm.Id;
                    }
                }
            }
            double PaymentLimit = string.IsNullOrEmpty(TextBoxCustomerPaymentLimit.Text) ? 0.00 : double.Parse(TextBoxCustomerPaymentLimit.Text);
            lCustomer.PaymentLimit = PaymentLimit;
            lCustomer.LockBill = RadioCustomerLockBill.Checked;

            if (CustomerLicenceInfoGrid.Rows.Count > 0)
            {
                lCustomer.CustomerLicenceDetail = new List<CustomerLicenceDetail>() { };
                for (int i = 0; i < CustomerLicenceInfoGrid.Rows.Count; i++)
                {
                    CustomerLicenceDetail CustomerLicenceDetail = new CustomerLicenceDetail();
                    CustomerLicenceDetail.CompanyCustomerLicenseMasterId = CustomerLicenceInfoGrid.Rows[i].Cells[(int)CustomerFormTaxInfoTableColumn.MASTERID].Value == null ? 0L : (long)CustomerLicenceInfoGrid.Rows[i].Cells[(int)CustomerFormTaxInfoTableColumn.MASTERID].Value;
                    CustomerLicenceDetail.CustomerLicenceId = CustomerLicenceInfoGrid.Rows[i].Cells[(int)CustomerFormTaxInfoTableColumn.ID].Value == null ? 0L : (long)CustomerLicenceInfoGrid.Rows[i].Cells[(int)CustomerFormTaxInfoTableColumn.ID].Value;
                    CustomerLicenceDetail.DisplayName = CustomerLicenceInfoGrid.Rows[i].Cells[(int)CustomerFormTaxInfoTableColumn.DNAME].Value?.ToString()?? "";
                    CustomerLicenceDetail.Value = CustomerLicenceInfoGrid.Rows[i].Cells[(int)CustomerFormTaxInfoTableColumn.VALUE].Value != null ? CustomerLicenceInfoGrid.Rows[i].Cells[(int)CustomerFormTaxInfoTableColumn.VALUE].Value.ToString()!.Trim() : null;
                    CustomerLicenceDetail.CompanyId = Global.Company.CompanyId;
                    lCustomer.CustomerLicenceDetail.Add(CustomerLicenceDetail);
                }
            }

            return lCustomer;
        }
        private Address GetCustomerBillingAddressFromForm()
        {
            Address lAddress = new Address();
            lAddress.AddressLine1 = BillingAddressGroupBoxCustomer.AddressLine1.Trim();
            lAddress.AddressLine2 = BillingAddressGroupBoxCustomer.AddressLine2.Trim();
            lAddress.CityOrTown = BillingAddressGroupBoxCustomer.CityName.Trim();
            lAddress.District = BillingAddressGroupBoxCustomer.DistrictName.Trim();
            lAddress.PinCode = BillingAddressGroupBoxCustomer.PinCode.Trim();
            lAddress.StatesId = BillingAddressGroupBoxCustomer.StateId;
            return lAddress;
        }
        private Address GetCustomerShippingAddressFromForm()
        {
            Address lAddress = new Address();
            lAddress.AddressLine1 = ShippingAddressGroupBoxCustomer.AddressLine1.Trim();
            lAddress.AddressLine2 = ShippingAddressGroupBoxCustomer.AddressLine2.Trim();
            lAddress.CityOrTown = ShippingAddressGroupBoxCustomer.CityName.Trim();
            lAddress.District = ShippingAddressGroupBoxCustomer.DistrictName.Trim();
            lAddress.PinCode = ShippingAddressGroupBoxCustomer.PinCode.Trim();
            lAddress.StatesId = ShippingAddressGroupBoxCustomer.StateId;
            return lAddress;
        }
        private ContactInfo GetCustomerContactInfoFromForm()
        {
            ContactInfo lContactInfo = new ContactInfo();
            lContactInfo.Phone = TextBoxCustomerPhone.Text.Trim().Replace("-", "");
            lContactInfo.Mobile = TextBoxCustomerMobile.Text.Trim().Replace("-", "");
            lContactInfo.Fax = TextBoxCustomerFax.Text.Trim();
            lContactInfo.Email = TextBoxCustomerEmail.Text.Trim();
            lContactInfo.WebSite = TextBoxCustomerWebsite.Text.Trim();
            return lContactInfo;
        }
        private void TreeViewCustomer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                TreeNode node = e.Node!;
                node.SelectedImageIndex = node.ImageIndex;
                ResetForm();
                LoadComboBox();
                LoadCustomerInfo();
                EnableForm(false);
                ToolStripStatusLabelErrorCustomer.Text = "";
                this.formIsDirty = false;
            }
            catch
            {

            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }
        private void TextBoxCustomerSearch_TextChanged(object sender, EventArgs e)
        {
            ResetForm();
            LoadCustomersWithFilter();
            if (TreeViewCustomer.Nodes.Count > 0) { BtnCustomerEdit.Enabled = true; BtnCustomerDelete.Enabled = true; }
            else { BtnCustomerEdit.Enabled = false; BtnCustomerDelete.Enabled = false; }
            TextBoxCustomerSearch.Select();
            this.formIsDirty = false;
        }

        private void BtnCustomerNew_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            LoadComboBox();
            EnableForm(true);
            BillingAddressGroupBoxCustomer.CountryId = (long)Global.Company.CountryId!;
            ShippingAddressGroupBoxCustomer.CountryId = (long)Global.Company.CountryId;
            BillingAddressGroupBoxCustomer.StateId = (long)Global.Company.Address.StatesId!;
            ShippingAddressGroupBoxCustomer.StateId = (long)Global.Company.Address.StatesId!;
            ToolStripStatusLabelErrorCustomer.Text = "";
            TabControlCustomer.SelectedTab = TabCustomer;
            TextBoxCustomerName.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }

        private void BtnCustomerDelete_Click(object sender, EventArgs e)
        {
            ToolStripStatusLabelErrorCustomer.Text = "";
            if (string.IsNullOrEmpty(TextBoxCustomerAccountId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this customer is still valid.");
                return;
            }
            Customer CustomerInfo = GetCustomerInfo();
            if (CustomerInfo != null)
            {
                DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, CustomerInfo.Name), "Delete Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        Customer lCustomer = CustomerManager.CheckCustomerSubCustomers(CustomerInfo.Id);
                        if (lCustomer == null)
                        {
                            Customer Customer = CustomerManager.GetCustomerById(CustomerInfo.Id);
                            if (Customer != null)
                            {
                                if (CustomerManager.DeleteCustomer(CustomerInfo.Id))
                                {
                                    ResetForm();
                                    TabControlCustomer.SelectedTab = TabCustomer;
                                    LoadComboBox();
                                    LoadCustomersWithFilter();
                                    TextBoxCustomerSearch.Clear();
                                    EnableForm(false);
                                    if (TreeViewCustomer.Nodes.Count > 0)
                                    {
                                        ToolStripStatusLabelErrorCustomer.Text = DeleteSuccessText;
                                    }
                                    else
                                    {
                                        ToolStripStatusLabelErrorCustomer.Text = NoCustomerFoundText;
                                    }
                                }
                                else
                                {
                                    ToolStripStatusLabelErrorCustomer.Text = DeleteErrorText;
                                }
                            }
                            this.formIsDirty = false;
                        }
                        else
                        {
                            ToolStripStatusLabelErrorCustomer.Text = IsbranchDeleteErrorText;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }

                }
            }
            else
            {
                DisplaySystemError("Somting went wrong, please check this customer is still valid.");
                return;
            }
        }
        private void BtnCustomerEdit_Click(object sender, EventArgs e)
        {
            User user = Global.User;
            if (string.IsNullOrEmpty(TextBoxCustomerAccountId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this customer is still valid.");
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            LoadCustomerInfo();
            EnableForm(true);
            if (CheckBoxCustomerIsbranch.Checked == false)
            {
                CheckBoxCustomerIsbranch.Enabled = false;
                ComboBoxCustomerParentAccount.Visible = false;
            }
            if (CheckBoxCustomerUseBillingAddress.Checked == true)
            {
                ShippingAddressGroupBoxCustomer.ReadOnly = true;
            }
            if (user.IsSuperAdmin)
            {
                TextBoxCustomerPaymentLimit.ReadOnly = false;
                RadioCustomerLockBill.Enabled = true;
            }
            else
            {
                TextBoxCustomerPaymentLimit.ReadOnly = true;
                RadioCustomerLockBill.Enabled = false;
            }
            TabControlCustomer.SelectedTab = TabCustomer;
            TextBoxCustomerName.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void BtnCustomerCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TabControlCustomer.SelectedTab = TabCustomer;
                    TextBoxCustomerName.Select();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            BillingAddressGroupBoxCustomer.StateId = (long)Global.Company.Address.StatesId!;
            ShippingAddressGroupBoxCustomer.StateId = (long)Global.Company.Address.StatesId!;
            TabControlCustomer.SelectedTab = TabCustomer;
            LoadComboBox();
            if (string.IsNullOrEmpty(TextBoxCustomerSearch.Text))
            {
                LoadCustomerInfo();
            }
            else
            {
                TextBoxCustomerSearch.Clear();
            }
            EnableForm(false);
            if (TreeViewCustomer.Nodes.Count > 0)
            {
                ToolStripStatusLabelErrorCustomer.Text = "";
                TextBoxCustomerSearch.Select();
            }
            else
            {
                ToolStripStatusLabelErrorCustomer.Text = NoCustomerFoundText;
                BtnCustomerNew.Select();
            }
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }

        private void BtnCustomerSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Cursor.Current = Cursors.WaitCursor;
                Customer lCustomer = GetCustomerFromForm();
                Customer lCustomerFromDB = null!;
                try
                {
                    if (lCustomer.Id == 0)
                    {
                        Customer lCustomerName = CustomerManager.GetCustomerByName(lCustomer.Name, Global.Company.CompanyId);
                        if (lCustomerName == null)
                        {
                            lCustomer.BillingAddress = GetCustomerBillingAddressFromForm();
                            lCustomer.ShippingAddress = GetCustomerShippingAddressFromForm();
                            lCustomer.ContactInfo = GetCustomerContactInfoFromForm();
                            lCustomerFromDB = CustomerManager.AddCustomer(lCustomer);
                        }
                        else
                        {
                            ToolStripStatusLabelErrorCustomer.Text = string.Format(UniqueCustomerNameErrorMsg, lCustomer.Name, Global.Company.Name);
                            TabControlCustomer.SelectedTab = TabCustomer;
                            TextBoxCustomerName.Select();
                            return;
                        }
                    }
                    else
                    {
                        Customer lCustomerById = CustomerManager.GetCustomerById(lCustomer.Id);
                        if (lCustomerById != null)
                        {
                            Customer lCustomerName = CustomerManager.CheckCustomerNameInUpdate(lCustomer.Name, lCustomer.Id, Global.Company.CompanyId);
                            if (lCustomerName == null)
                            {
                                Address lBillingAddress = GetCustomerBillingAddressFromForm();
                                Address lShippingAddress = GetCustomerShippingAddressFromForm();
                                ContactInfo lContactInfo = GetCustomerContactInfoFromForm();
                                lCustomer.ContactInfoId = lCustomerById.ContactInfoId;
                                lCustomer.BillingAddressId = lCustomerById.BillingAddressId;
                                lCustomer.ShippingAddressId = lCustomerById.ShippingAddressId;
                                lContactInfo.Id = (long)lCustomer.ContactInfoId!;
                                lBillingAddress.AddressId = (long)lCustomer.BillingAddressId!;
                                lShippingAddress.AddressId = (long)lCustomer.ShippingAddressId!;
                                lCustomerFromDB = CustomerManager.UpdateCustomer(lCustomer);
                                Address lBillingAddressFromDB = AddressManager.UpdateAddress(lBillingAddress);
                                Address lShippingAddressFromDB = AddressManager.UpdateAddress(lShippingAddress);
                                ContactInfo lContactInfoFromDB = ContactInfoManager.UpdateContactInfo(lContactInfo);
                            }
                            else
                            {
                                ToolStripStatusLabelErrorCustomer.Text = string.Format(UniqueCustomerNameErrorMsg, lCustomer.Name, Global.Company.Name);
                                TabControlCustomer.SelectedTab = TabCustomer;
                                TextBoxCustomerName.Select();
                                return;
                            }
                        }
                        else
                        {
                            DisplaySystemError("Somting went wrong, please check this customer is still valid.");
                            return;
                        }
                    }
                    ResetForm();
                    TabControlCustomer.SelectedTab = TabCustomer;
                    ComboUtils.InitializeCustomerCombo(ComboBoxCustomerParentAccount, Global.Company.CompanyId);
                    if (string.IsNullOrEmpty(TextBoxCustomerSearch.Text))
                    {
                        LoadCustomersWithFilter();
                    }
                    else
                    {
                        TextBoxCustomerSearch.Clear();
                    }
                    TreeNode TreeNode = new TreeNode();
                    TreeNode = lCustomerFromDB.ParentAccountId == null ? TreeViewCustomer.Nodes[lCustomerFromDB.Id.ToString()] : TreeViewCustomer.Nodes[lCustomerFromDB.ParentAccountId.ToString()].Nodes[lCustomerFromDB.Id.ToString()];
                    TreeViewCustomer.SelectedNode = TreeNode;
                    TreeViewCustomer.Focus();
                    EnableForm(false);
                    ToolStripStatusLabelErrorCustomer.Text = SaveSuccessText;
                    if (CreateCustomerOnLoad)
                    {
                        if (parent != null)
                        {
                            parent.AccountIdTransport.ResetText();
                            parent.AccountIdTransport.Text = lCustomerFromDB.Id.ToString();
                        }
                        this.Close();
                    }
                }
                catch
                {

                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
                this.formIsDirty = false;
            }
        }
        private void BtnCurrencyExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnCustomerAddNewTerm_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormPaymentTerm FormPaymentTerm = new FormPaymentTerm();
            FormPaymentTerm.CreatePaymentTermOnLoad = true;
            FormPaymentTerm.ShowDialog();
            ComboUtils.InitializePaymentTermCombo(ComboBoxCustomerPaymentTerm, Global.Company.CompanyId);
            ComboBoxCustomerPaymentTerm.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }

        private void BtnCustomerAddNewPayment_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormPaymentMethod FormPaymentMethod = new FormPaymentMethod();
            FormPaymentMethod.CreatePaymentMethodOnLoad = true;
            FormPaymentMethod.ShowDialog();
            ComboUtils.InitializePaymentMethodCombo(ComboBoxCustomerPaymentMethod, Global.Company.CompanyId);
            ComboBoxCustomerPaymentMethod.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }

        private Boolean ValidateForm()
        {
            ToolStripStatusLabelErrorCustomer.Text = "";
            if (string.IsNullOrEmpty(TextBoxCustomerName.Text.Trim()))
            {
                ToolStripStatusLabelErrorCustomer.Text = EnterCustomerNameErrorMsg;
                TabControlCustomer.SelectedTab = TabCustomer;
                TextBoxCustomerName.Select();
                return false;
            }

            if (CheckBoxCustomerIsbranch.Checked == true && ComboBoxCustomerParentAccount.SelectedIndex < 0)
            {
                ToolStripStatusLabelErrorCustomer.Text = ChooseParentErrorMsg;
                TabControlCustomer.SelectedTab = TabCustomer;
                ComboBoxCustomerParentAccount.Select();
                return false;
            }
            if (CheckBoxCustomerIsbranch.Checked == true && ComboBoxCustomerParentAccount.SelectedIndex >= 0 && CustomerManager.GetCustomerById(((Customer)ComboBoxCustomerParentAccount.Items[ComboBoxCustomerParentAccount.SelectedIndex]).Id) == null)
            {
                ToolStripStatusLabelErrorCustomer.Text = "Somting went wrong, please check this parent customer is still valid.";
                TabControlCustomer.SelectedTab = TabCustomer;
                ComboBoxCustomerParentAccount.Select();
                return false;
            }
            if (ComboBoxBalanceType.SelectedIndex == -1)
            {
                ToolStripStatusLabelErrorCustomer.Text = ChooseBalanceTypeErrorMsg;
                TabControlCustomer.SelectedTab = TabCustomer;
                ComboBoxBalanceType.Select();
                return false;
            }
            if (DateTimePickerCustomer.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerCustomer.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ToolStripStatusLabelErrorCustomer.Text = EnterValidAsOfDateErrorMsg;
                DateTimePickerCustomer.Select();
                return false;
            }
            if (ComboBoxCustomerPaymentMethod.SelectedIndex < 0)
            {
                ToolStripStatusLabelErrorCustomer.Text = ChoosePaymentMethodErrorMsg;
                TabControlCustomer.SelectedTab = TabBillingInfo;
                ComboBoxCustomerPaymentMethod.Select();
                return false;
            }
            if (ComboBoxCustomerPaymentMethod.SelectedIndex >= 0 && PaymentMethodManager.GetPaymentMethodById(((PaymentMethod)ComboBoxCustomerPaymentMethod.Items[ComboBoxCustomerPaymentMethod.SelectedIndex]).Id) == null)
            {
                ToolStripStatusLabelErrorCustomer.Text = "Somting went wrong, please check selected payment method is still valid.";
                TabControlCustomer.SelectedTab = TabBillingInfo;
                ComboBoxCustomerPaymentMethod.Select();
                return false;
            }
            if (ComboBoxCustomerPaymentTerm.SelectedIndex < 0)
            {
                ToolStripStatusLabelErrorCustomer.Text = ChoosePaymentTermErrorMsg;
                TabControlCustomer.SelectedTab = TabBillingInfo;
                ComboBoxCustomerPaymentTerm.Select();
                return false;
            }
            if (ComboBoxCustomerPaymentTerm.SelectedIndex >= 0 && PaymentTermManager.GetPaymentTermById(((PaymentTerm)ComboBoxCustomerPaymentTerm.Items[ComboBoxCustomerPaymentTerm.SelectedIndex]).Id) == null)
            {
                ToolStripStatusLabelErrorCustomer.Text = "Somting went wrong, please check selected payment term is still valid.";
                TabControlCustomer.SelectedTab = TabBillingInfo;
                ComboBoxCustomerPaymentTerm.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxCustomerPaymentLimit.Text.Trim()))
            {
                ToolStripStatusLabelErrorCustomer.Text = EnterPaymentLimitErrorMsg;
                TabControlCustomer.SelectedTab = TabBillingInfo;
                TextBoxCustomerPaymentLimit.Select();
                return false;
            }
            if (BillingAddressGroupBoxCustomer.StateId == 0L)
            {
                ToolStripStatusLabelErrorCustomer.Text = ChooseStateErrorMsg;
                TabControlCustomer.SelectedTab = TabBillingInfo;
                BillingAddressGroupBoxCustomer.selected_field(Fields.state);
                return false;
            }
            if (ShippingAddressGroupBoxCustomer.StateId == 0L)
            {
                ToolStripStatusLabelErrorCustomer.Text = ChooseStateErrorMsg;
                TabControlCustomer.SelectedTab = TabShippingInfo;
                ShippingAddressGroupBoxCustomer.selected_field(Fields.state);
                return false;
            }
            if (!string.IsNullOrEmpty(TextBoxCustomerEmail.Text.Trim()) && !KeypressValidation.EmailValidation(TextBoxCustomerEmail.Text.Trim()))
            {
                ToolStripStatusLabelErrorCustomer.Text = EnterEmailErrorMsg;
                TabControlCustomer.SelectedTab = TabContactInfo;
                TextBoxCustomerEmail.Select();
                return false;
            }
            if (!string.IsNullOrEmpty(TextBoxCustomerWebsite.Text.Trim()) && !KeypressValidation.WebsiteValidation(TextBoxCustomerWebsite.Text.Trim()))
            {
                ToolStripStatusLabelErrorCustomer.Text = EnterWebsiteErrorMsg;
                TabControlCustomer.SelectedTab = TabContactInfo;
                TextBoxCustomerWebsite.Select();
                return false;
            }

            if (CustomerLicenceInfoGrid.Rows.Count > 0)
            {
                for (int i = 0; i < CustomerLicenceInfoGrid.Rows.Count; i++)
                {
                    if ((CustomerLicenceInfoGrid.Rows[i].Cells[(int)CustomerFormTaxInfoTableColumn.VALUE].Value == null || string.IsNullOrEmpty(CustomerLicenceInfoGrid.Rows[i].Cells[(int)CustomerFormTaxInfoTableColumn.VALUE].Value.ToString()!.Trim())) && (bool)CustomerLicenceInfoGrid.Rows[i].Cells[(int)CustomerFormTaxInfoTableColumn.REQUIR].Value)
                    {
                        ToolStripStatusLabelErrorCustomer.Text = string.Format(TaxInfoGrid_MantatoryFiledErrorMsg, CustomerLicenceInfoGrid.Columns[(int)CustomerFormTaxInfoTableColumn.VALUE].HeaderText);
                        TabControlCustomer.SelectedTab = TabLicenseInfo;
                        CustomerLicenceInfoGrid.Select();
                        CustomerLicenceInfoGrid.CurrentCell = CustomerLicenceInfoGrid[(int)CustomerFormTaxInfoTableColumn.VALUE, i];
                        CustomerLicenceInfoGrid.BeginEdit(true);
                        return false;
                    }

                }
            }


            return true;
        }

        private void ResetForm()
        {
            BillingAddressGroupBoxCustomer.Clear();
            BillingAddressGroupBoxCustomer.CountryId = (long)Global.Company.CountryId!;
            BillingAddressGroupBoxCustomer.StateId = (long)Global.Company.Address.StatesId!;
            ShippingAddressGroupBoxCustomer.StateId = (long)Global.Company.Address.StatesId!;
            ShippingAddressGroupBoxCustomer.Clear();
            ShippingAddressGroupBoxCustomer.CountryId = (long)Global.Company.CountryId;
            TextBoxCustomerAccountId.ResetText();
            TextBoxCustomerName.ResetText();
            TextBoxCustomerDisplayAs.ResetText();
            CheckBoxCustomerIsbranch.Checked = false;
            CheckBoxUseDisplayName.Checked = false;
            CheckBoxCustomerUseBillingAddress.Checked = false;
            TextBoxCustomerDescription.ResetText();
            TextBoxCustomerNameonCheck.ResetText();
            TextBoxGSTNo.ResetText();
            TextBoxCustomerEmail.ResetText();
            TextBoxCustomerFax.ResetText();
            TextBoxCustomerPhone.ResetText();
            TextBoxCustomerMobile.ResetText();
            TextBoxCustomerWebsite.ResetText();
            ComboBoxBalanceType.SelectedIndex = 0;
            ComboBoxCustomerParentAccount.SelectedIndex = -1;
            ComboBoxCustomerPaymentMethod.SelectedIndex = -1;
            ComboBoxCustomerPaymentTerm.SelectedIndex = -1;
            TextBoxCustomerPaymentLimit.Text = "0.00";
            TextBoxCustomerPaymentLimit.Decimals = Global.Company.PrimaryCurrency.RoundingPrecision;
            RadioCustomerLockBill.Checked = false;
            TextBoxCustomerBalance.ResetText();
            TextBoxCustomerBalance.Decimals = Global.Company.PrimaryCurrency.RoundingPrecision;
            DateTimePickerCustomer.Format = Global.Company.DateFormat;
            DateTimePickerCustomer.MaxDate = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
            DateTimePickerCustomer.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
            CustomerLicenceInfoGrid.Rows.Clear();
        }

        private void EnableForm(Boolean enable)
        {
            User user = Global.User;
            if (TreeViewCustomer.Nodes.Count > 0)
            {
                TreeViewCustomer.Enabled = !enable;
                TextBoxCustomerSearch.ReadOnly = enable;
                TextBoxCustomerSearch.TabStop = !enable;
            }
            else
            {
                TreeViewCustomer.Enabled = false;
                TextBoxCustomerSearch.ReadOnly = true;
                TextBoxCustomerSearch.TabStop = false;
                BtnCustomerNew.Select();
            }
            CustomerLicenceInfoGrid.Enabled = enable;

            TextBoxCustomerNameonCheck.ReadOnly = !enable;
            ShippingAddressGroupBoxCustomer.ReadOnly = !enable;
            BillingAddressGroupBoxCustomer.ReadOnly = !enable;
            TextBoxCustomerDescription.ReadOnly = !enable;
            TextBoxCustomerDisplayAs.ReadOnly = !enable;
            TextBoxCustomerEmail.ReadOnly = !enable;
            TextBoxCustomerFax.ReadOnly = !enable;
            TextBoxCustomerPhone.ReadOnly = !enable;
            TextBoxCustomerName.ReadOnly = !enable;
            TextBoxCustomerMobile.ReadOnly = !enable;
            TextBoxCustomerWebsite.ReadOnly = !enable;
            TextBoxCustomerBalance.ReadOnly = !enable;
            TextBoxGSTNo.ReadOnly = !enable;
            if (user.IsSuperAdmin)
            {
                TextBoxCustomerPaymentLimit.ReadOnly = !enable;
                RadioCustomerLockBill.Enabled = enable;
            }
            else
            {
                TextBoxCustomerPaymentLimit.ReadOnly = true;
                RadioCustomerLockBill.Enabled = false;
            }
            RadioCustomerLockBill.TabStop = enable;
            TextBoxCustomerPaymentLimit.TabStop = enable;
            TextBoxCustomerBalance.TabStop = enable;
            TextBoxCustomerNameonCheck.TabStop = enable;
            BillingAddressGroupBoxCustomer.TabStop = enable;
            ShippingAddressGroupBoxCustomer.TabStop = enable;
            TextBoxCustomerDescription.TabStop = enable;
            TextBoxCustomerDisplayAs.TabStop = enable;
            TextBoxCustomerEmail.TabStop = enable;
            TextBoxCustomerFax.TabStop = enable;
            TextBoxCustomerPhone.TabStop = enable;
            TextBoxCustomerName.TabStop = enable;
            TextBoxCustomerMobile.TabStop = enable;
            TextBoxCustomerWebsite.TabStop = enable;
            ComboBoxBalanceType.Visible = enable;
            if (ComboBoxCustomerParentAccount.Items.Count > 0)
            {
                CheckBoxCustomerIsbranch.Enabled = enable;
            }
            ComboBoxCustomerPaymentMethod.Visible = enable;
            ComboBoxCustomerPaymentTerm.Visible = enable;
            BtnCustomerAddNewPayment.Enabled = enable;
            BtnCustomerAddNewTerm.Enabled = enable;
            CheckBoxUseDisplayName.Enabled = enable;
            CheckBoxCustomerUseBillingAddress.Enabled = enable;
            DateTimePickerCustomer.ReadOnly = !enable;
            DateTimePickerCustomer.TabStop = enable;
            if (CheckBoxCustomerIsbranch.Checked && enable)
                ComboBoxCustomerParentAccount.Visible = true;
            else
                ComboBoxCustomerParentAccount.Visible = false;

            if (!enable)
            {
                BtnCustomerCancel.Enabled = enable;

                if (TreeViewCustomer.SelectedNode == null)
                {
                    BtnCustomerDelete.Enabled = enable;
                    BtnCustomerEdit.Enabled = enable;
                }
                else
                {
                    BtnCustomerDelete.Enabled = !enable;
                    BtnCustomerEdit.Enabled = !enable;
                }
                BtnCustomerNew.Enabled = !enable;
                BtnCustomerSave.Enabled = enable;
            }
            else
            {
                BtnCustomerCancel.Enabled = enable;
                BtnCustomerDelete.Enabled = !enable;
                BtnCustomerEdit.Enabled = !enable;
                BtnCustomerNew.Enabled = !enable;
                BtnCustomerSave.Enabled = enable;
            }
        }
        int Index = -1;
        private void CheckBoxCustomerIsbranch_CheckedChanged(object sender, EventArgs e)
        {
            ComboBoxCustomerParentAccount.Visible = CheckBoxCustomerIsbranch.Checked;
            if (!CheckBoxCustomerIsbranch.Checked) { LabelCustomerParent.Font = new Font("Tahoma", 8, FontStyle.Regular); Index = ComboBoxCustomerParentAccount.SelectedIndex; ComboBoxCustomerParentAccount.SelectedIndex = -1; }
            else if (CheckBoxCustomerIsbranch.Checked) { LabelCustomerParent.Font = new Font("Tahoma", 8, FontStyle.Bold); ComboBoxCustomerParentAccount.SelectedIndex = Index; }
        }

        private void CheckBoxUseDisplayName_CheckedChanged(object sender, EventArgs e)
        {
            string Temp = TextBoxCustomerNameonCheck.Text;
            TextBoxCustomerNameonCheck.Enabled = !CheckBoxUseDisplayName.Checked;
            LabelCustomerNameonCheck.Font = !CheckBoxUseDisplayName.Checked ? new Font("Tahoma", 8, FontStyle.Regular) : new Font("Tahoma", 8, FontStyle.Bold);
            if (CheckBoxUseDisplayName.Checked)
            {
                if (!string.IsNullOrEmpty(TextBoxCustomerDisplayAs.Text.Trim()))
                {
                    TextBoxCustomerNameonCheck.Text = TextBoxCustomerDisplayAs.Text;
                }
                else
                {
                    TextBoxCustomerNameonCheck.Text = Temp;
                }
            }
        }
        private void CheckBoxCustomerUseBillingAddress_CheckedChanged(object sender, EventArgs e)
        {
            ShippingAddressGroupBoxCustomer.ReadOnly = CheckBoxCustomerUseBillingAddress.Checked;
            if (CheckBoxCustomerUseBillingAddress.Checked == true)
            {
                ShippingAddressGroupBoxCustomer.AddressLine1 = BillingAddressGroupBoxCustomer.AddressLine1;
                ShippingAddressGroupBoxCustomer.AddressLine2 = BillingAddressGroupBoxCustomer.AddressLine2;
                ShippingAddressGroupBoxCustomer.CityName = BillingAddressGroupBoxCustomer.CityName;
                ShippingAddressGroupBoxCustomer.DistrictName = BillingAddressGroupBoxCustomer.DistrictName;
                ShippingAddressGroupBoxCustomer.StateName = BillingAddressGroupBoxCustomer.StateName;
                ShippingAddressGroupBoxCustomer.PinCode = BillingAddressGroupBoxCustomer.PinCode;
            }
            else
            {
                ShippingAddressGroupBoxCustomer.Enabled = true;
                ShippingAddressGroupBoxCustomer.AddressLine1 = "";
                ShippingAddressGroupBoxCustomer.AddressLine2 = "";
                ShippingAddressGroupBoxCustomer.CityName = "";
                ShippingAddressGroupBoxCustomer.DistrictName = "";
                ShippingAddressGroupBoxCustomer.StateName = "";
                ShippingAddressGroupBoxCustomer.PinCode = "";
            }
        }
        private void TextBoxCustomerBalance_Leave(object sender, EventArgs e)
        {
            if (TextBoxCustomerBalance.Text == "." || TextBoxCustomerBalance.Text == "")
            {
                TextBoxCustomerBalance.Text = "0.00";
            }
            else
            {
                TextBoxCustomerBalance.Text = String.Format("{0:0.00}", float.Parse(TextBoxCustomerBalance.Text));
            }
        }
        private void TextBoxCustomerSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxCustomerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }

        private void TextBoxCustomerDisplayAs_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }


        private void TextBoxCustomerNameonCheck_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }

        private void TextBoxCustomerBalance_KeyPress(object sender, KeyPressEventArgs e)
        {
            string Unselected = TextBoxCustomerBalance.Text;
            if (TextBoxCustomerBalance.SelectionLength > 0) { Unselected = TextBoxCustomerBalance.Text.Remove(TextBoxCustomerBalance.SelectionStart, TextBoxCustomerBalance.SelectionLength); }
            KeypressValidation.Keypress_NumberDot(sender, e, Unselected);
        }

        private void TextBoxCustomerEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_EmailChecking(sender, e);

        }

        private void TextBoxCustomerWebsite_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_WebsiteNameChecking(sender, e);
        }
        private void TextBoxCustomerSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                TreeViewCustomer.Select();
            }
        }
        private void TextBoxCustomerAsof_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                DateTimePickerCustomer.Focus();
                SendKeys.Send("{F4}");
            }
        }

        private void DateTimePickerCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                SendKeys.Send("{F4}");
            }
        }
        private void DateTimePickerCustomer_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

            if (e.KeyCode == Keys.Tab && DateTimePickerCustomer.TabStop)
            {
                e.IsInputKey = true;
                TabControlCustomer.SelectedTab = TabBillingInfo;
                ComboBoxCustomerPaymentMethod.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab && DateTimePickerCustomer.TabStop)
            {
                TabControlCustomer.SelectedTab = TabCustomer;
                TextBoxCustomerBalance.Select();
            }
        }
        private void ComboBoxCustomerPaymentMethod_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlCustomer.SelectedTab = TabBillingInfo;
                ComboBoxCustomerPaymentTerm.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TabControlCustomer.SelectedTab = TabCustomer;
                DateTimePickerCustomer.Focus();
            }
        }
        private void BillingAddressGroupBoxCustomer_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && BillingAddressGroupBoxCustomer.TabStop)
            {
                e.IsInputKey = true;
                TabControlCustomer.SelectedTab = TabShippingInfo;
                CheckBoxCustomerUseBillingAddress.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab && BillingAddressGroupBoxCustomer.TabStop)
            {
                TabControlCustomer.SelectedTab = TabBillingInfo;
                RadioCustomerLockBill.Focus();

            }
        }

        private void CheckBoxCustomerUseBillingAddress_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (ShippingAddressGroupBoxCustomer.ReadOnly == true)
                {
                    TabControlCustomer.SelectedTab = TabContactInfo;
                    TextBoxCustomerPhone.Select();
                }
                else
                {
                    TabControlCustomer.SelectedTab = TabShippingInfo;
                    ShippingAddressGroupBoxCustomer.selected_field(Fields.address1);
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TabControlCustomer.SelectedTab = TabBillingInfo;
                BillingAddressGroupBoxCustomer.selected_field(Fields.pin);
            }
        }
        private void ShippingAddressGroupBoxCustomer_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && ShippingAddressGroupBoxCustomer.TabStop)
            {
                e.IsInputKey = true;
                TabControlCustomer.SelectedTab = TabContactInfo;
                TextBoxCustomerPhone.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab && ShippingAddressGroupBoxCustomer.TabStop)
            {

                TabControlCustomer.SelectedTab = TabShippingInfo;
                CheckBoxCustomerUseBillingAddress.Select();
            }
        }

        private void TextBoxCustomerPhone_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && TextBoxCustomerPhone.TabStop)
            {
                e.IsInputKey = true;
                TabControlCustomer.SelectedTab = TabContactInfo;
                TextBoxCustomerMobile.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab && TextBoxCustomerPhone.TabStop)
            {
                if (ShippingAddressGroupBoxCustomer.ReadOnly == true)
                {
                    TabControlCustomer.SelectedTab = TabShippingInfo;
                    CheckBoxCustomerUseBillingAddress.Select();
                }
                else
                {
                    TabControlCustomer.SelectedTab = TabShippingInfo;
                    ShippingAddressGroupBoxCustomer.selected_field(Fields.pin);
                }
            }
        }
        private void BtnCustomerSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlCustomer.SelectedTab = TabCustomer;
                TextBoxCustomerName.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (CustomerLicenceInfoGrid.Rows.Count > 0)
                {
                    TabControlCustomer.SelectedTab = TabLicenseInfo;
                    CustomerLicenceInfoGrid.Focus();
                    CustomerLicenceInfoGrid.CurrentCell = CustomerLicenceInfoGrid[(int)CustomerFormTaxInfoTableColumn.DNAME, 0];
                    CustomerLicenceInfoGrid.BeginEdit(true);
                }
                else
                {
                    TabControlCustomer.SelectedTab = TabContactInfo;
                    TextBoxCustomerWebsite.Select();
                }
            }
        }
        private void TextBoxCustomerName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && TextBoxCustomerName.TabStop)
            {
                e.IsInputKey = true;
                TextBoxCustomerDisplayAs.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab && TextBoxCustomerName.TabStop)
            {
                e.IsInputKey = true;
                BtnCustomerSave.Select();
            }
        }
        private void ComboBoxCustomerParentAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxCustomerParentAccount.DroppedDown = false;
        }

        private void ComboBoxCustomerPaymentMethod_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxCustomerPaymentMethod.DroppedDown = false;
        }

        private void ComboBoxCustomerPaymentTerm_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxCustomerPaymentTerm.DroppedDown = false;
        }


        private void TextBoxCustomerName_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "NameChecking");

        }

        private void TextBoxCustomerName_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxCustomerName, "NameChecking");
            }
        }

        private void TextBoxCustomerDisplayAs_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "NameChecking");

        }

        private void TextBoxCustomerDisplayAs_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxCustomerDisplayAs, "NameChecking");
            }
        }

        private void TextBoxCustomerNameonCheck_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "NameChecking");

        }

        private void TextBoxCustomerNameonCheck_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxCustomerNameonCheck, "NameChecking");
            }
        }


        private void TextBoxCustomerEmail_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "EmailChecking");

        }

        private void TextBoxCustomerEmail_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxCustomerEmail, "EmailChecking");
            }
        }

        private void TextBoxCustomerWebsite_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_WebPasteChecking(sender, e, "WebsiteNameChecking");
        }

        private void TextBoxCustomerWebsite_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxCustomerWebsite, "WebsiteNameChecking");
            }
        }

        private void TextBoxCustomerBalance_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "NumberDot");

        }

        private void TextBoxCustomerBalance_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxCustomerBalance, "NumberDot");
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnCustomerNew.PerformClick();
            }
            if (keyData == (Keys.F4))
            {
                BtnCustomerDelete.PerformClick();
            }
            if (keyData == (Keys.F7))
            {
                BtnCustomerEdit.PerformClick();
            }
            if (keyData == (Keys.F8))
            {
                BtnCustomerSave.PerformClick();
            }
            if (keyData == (Keys.F10))
            {
                BtnCurrencyExit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                if (CreateCustomerOnLoad)
                {
                    BtnCurrencyExit.PerformClick();
                    return true;
                }
                BtnCustomerCancel.PerformClick();
                return true;
            }
            try
            {
                if (CustomerLicenceInfoGrid.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && CustomerLicenceInfoGrid.CurrentCell.ColumnIndex == (int)CustomerFormTaxInfoTableColumn.VALUE)
                    {
                        if (CustomerLicenceInfoGrid.CurrentRow.Index == CustomerLicenceInfoGrid.RowCount - 1)
                        {
                            BtnCustomerSave.Focus();
                            CustomerLicenceInfoGrid.CurrentCell = null;
                        }
                        else
                        {
                            SendKeys.Send("{tab}{tab}{tab}");
                        }
                    }

                    if (keyData == (Keys.Tab | Keys.Shift) && CustomerLicenceInfoGrid.CurrentCell!.ColumnIndex == (int)CustomerFormTaxInfoTableColumn.DNAME)
                    {
                        if (CustomerLicenceInfoGrid.CurrentRow.Index != 0)
                        {
                            SendKeys.Send("{tab}{tab}{tab}");
                        }
                        if (CustomerLicenceInfoGrid.CurrentRow.Index == 0)
                        {
                            TabControlCustomer.SelectedTab = TabContactInfo;
                            TextBoxCustomerWebsite.Focus();
                            CustomerLicenceInfoGrid.CurrentCell = null;
                        }
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void FormCustomers_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty && !CreateCustomerOnLoad)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TabControlCustomer.SelectedTab = TabCustomer;
                    TextBoxCustomerName.Select();
                    e.Cancel = true;
                }
            }
        }

        private void TextBoxCustomerName_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxCustomerName.Text) || !string.IsNullOrEmpty(TextBoxCustomerDisplayAs.Text))
            {
                CheckBoxUseDisplayName.Enabled = true;
                if (CheckBoxUseDisplayName.Checked && !string.IsNullOrEmpty(TextBoxCustomerDisplayAs.Text))
                {
                    TextBoxCustomerNameonCheck.Text = TextBoxCustomerDisplayAs.Text;
                }
                else if (CheckBoxUseDisplayName.Checked && !string.IsNullOrEmpty(TextBoxCustomerName.Text))
                {
                    TextBoxCustomerNameonCheck.Text = TextBoxCustomerName.Text;
                }
            }
            else
            {
                if (CheckBoxUseDisplayName.Checked) { TextBoxCustomerNameonCheck.Text = string.Empty; }
                CheckBoxUseDisplayName.Checked = CheckBoxUseDisplayName.Enabled = false;
            }

        }

        private void BillingAddressGroupBoxCustomer_Leave(object sender, EventArgs e)
        {
            if (CheckBoxCustomerUseBillingAddress.Checked)
            {
                ShippingAddressGroupBoxCustomer.AddressLine1 = BillingAddressGroupBoxCustomer.AddressLine1;
                ShippingAddressGroupBoxCustomer.AddressLine2 = BillingAddressGroupBoxCustomer.AddressLine2;
                ShippingAddressGroupBoxCustomer.CityName = BillingAddressGroupBoxCustomer.CityName;
                ShippingAddressGroupBoxCustomer.DistrictName = BillingAddressGroupBoxCustomer.DistrictName;
                ShippingAddressGroupBoxCustomer.StateName = BillingAddressGroupBoxCustomer.StateName;
                ShippingAddressGroupBoxCustomer.PinCode = BillingAddressGroupBoxCustomer.PinCode;
            }
        }

        private void TextBoxCustomerWebsite_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && TextBoxCustomerWebsite.TabStop)
            {
                e.IsInputKey = true;
                if (CustomerLicenceInfoGrid.Rows.Count > 0)
                {
                    TabControlCustomer.SelectedTab = TabLicenseInfo;
                    CustomerLicenceInfoGrid.Focus();
                    CustomerLicenceInfoGrid.CurrentCell = CustomerLicenceInfoGrid[(int)CustomerFormTaxInfoTableColumn.DNAME, 0];
                    CustomerLicenceInfoGrid.BeginEdit(true);
                }
                else
                {
                    BtnCustomerSave.Select();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab && TextBoxCustomerWebsite.TabStop)
            {
                TabControlCustomer.SelectedTab = TabContactInfo;
                TextBoxCustomerEmail.Select();
            }
        }

        private void CustomerLicenceInfoGrid_Leave(object sender, EventArgs e)
        {
            CustomerLicenceInfoGrid.CurrentCell = null;
        }

        private void CustomerLicenceInfoGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            CustomerLicenceInfoGrid.Rows[e.RowIndex].Cells[(int)CustomerFormTaxInfoTableColumn.NAME].ReadOnly = true;
            CustomerLicenceInfoGrid.Rows[e.RowIndex].Cells[(int)CustomerFormTaxInfoTableColumn.DNAME].ReadOnly = false;
            CustomerLicenceInfoGrid.Rows[e.RowIndex].Cells[(int)CustomerFormTaxInfoTableColumn.VALUE].ReadOnly = false;
            CustomerLicenceInfoGrid.Rows[e.RowIndex].Cells[(int)CustomerFormTaxInfoTableColumn.INVOICE].ReadOnly = true;
            CustomerLicenceInfoGrid.Rows[e.RowIndex].Cells[(int)CustomerFormTaxInfoTableColumn.REPORT].ReadOnly = true;
        }

        private void CustomerLicenceInfoGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox textBox)
            {
                textBox.TextAlign = HorizontalAlignment.Left;
                textBox.Select(0, 0);
            }
        }
    }
}
