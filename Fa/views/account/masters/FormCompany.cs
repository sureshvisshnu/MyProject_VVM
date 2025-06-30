using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using fa.api.Accounting;
using fa.model.Accounting.Masters;
using fa.model.Common;
using fa.libraries.utils;
using fa.libraries.Validation;
using fa.api.UserProfile;
using System.IO;
using fa.views.controls;
using Fa.api.catalog;
using System.Linq;
using fa.model.System;
using fa.model.OrderManagement;
using fa.api.System;
using fa.api.utils;
using fa.model.Accounting.Transaction;
//using FADataAccessLibrary.Migrations;
using FADataAccessLibrary.Model.Common;
using Fa.api.OrderManagement;
using fa.views.controls.grid;
using fa.model.Hms.Master;
using fa.api.Hms;
using fa.model.Hms.Op;
using fa.model.Hms.Ip;
using VisioForge.Libs.NDI;
using VisioForge.Libs.WindowsMediaLib;

namespace fa.views.account.masters
{
    public enum CompanyReferenceTableColumn
    {
        TYPE, PREFIX, SEED, DAILYRESET, PRINTTYPE, DOTMATRIX, ROUNDOFF, TYPEID, HASROUNDOFF, HASPRINTERSETUP, HASDOTMATRIX, HASDAILYRESET
    }
    public enum CompanyTaxInfoTableColumn
    {
        NAME, DNAME, VALUE, INVOICE, REPORT, REMOVE, ID
    }
    public enum CustomerTaxInfoTableColumn
    {
        NAME, DNAME, REQUIR, REPORT, INVOICE, REMOVE, ID
    }
    public enum SupplierTaxInfoTableColumn
    {
        NAME, DNAME, REQUIR, REPORT, REMOVE, ID
    }
    public enum CompanySaleTaxReceivableTableColumn
    {
        NAME, ACCOUNT, ID, ACCID, TAXID
    }
    public enum CompanyNarrationTableColumn
    {
        NAME
    }
    public partial class FormCompany : FormBase
    {
        public static string SaveSuccessText = "Saved success...";
        public static string CreateCompanyOnloadText = "Create New Company";
        public static string DeleteConfirmText = "Do you want to delete the Company {0}? \r\n [This will delete all Transactions, Catalog, Cost Centers, Accounts associate to this company]";
        public static string DeleteErrorText = "Error Deleting the Company!, Please retry";
        public static string IsbranchDeleteErrorText = "Cannot delete company with sub company";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string EnterCompanyEmailErrorMsg = "Please enter valid email.";
        public static string EnterCompanyWebsiteErrorMsg = "Please enter valid Website.";
        public static string GridViewSalesTax_ConfirmRowDeleteText = "Do you want to delete row {0}?";
        public static string GridViewSalesTax_RowDeleteErrorMsg = "Do not delete this Tax, Its used in some were else like Catalog";
        public static string EnterUPIIdErrorMsg = "Please enter UPI ID url";
        public static string TaxInfoGrid_MantatoryFiledErrorMsg = "Please enter {0}.";
        public static string TaxInfoGrid_UniqueTaxNameErrorMsg = "Name {0} Repeated again";
        public static string UniqueCompanyNameErrorMsg = "Company {0} already exists";
        public static string EnterCompanyNameErrorMsg = "Please enter company name";
        public static string ChooseParentErrorMsg = "Please choose parent company";
        public static string ChooseCountryErrorMsg = "Please choose country";
        public static string ChooseStateErrorMsg = "Please choose state";
        public static string ChooseCompanyTypeErrorMsg = "please choose company type";
        public static string ChooseFiscalYearErrorMsg = "please choose fiscal year";
        public static string ChooseIncomeTaxYearErrorMsg = "Please choose income tax year";
        public static string ChooseAccountingMethodErrorMsg = "please choose accounting method";
        public static string ChooseCurrencyErrorMsg = "please choose currency";
        public static string ChoosePricisionErrorMsg = "Quantity Precision limits from {0} to {1} only";
        public static string ChooseDateFormatErrorMsg = "please choose date format";
        public static string ChooseBusinessTypeErrorMsg = "please choose business type";
        public static string ChoosePriceTypeErrorMsg = "please select invoice price type";
        public static string EnterPurEntrySeedErrorMsg = "Please enter Purchase entry seed";
        public static string EnterPurOrderSeedErrorMsg = "Please enter purchase order seed";
        public static string EnterBillSeedErrorMsg = "Please enter purchase Return seed";
        public static string EnterSalEntrySeedErrorMsg = "Please enter sale entry seed";
        public static string EnterSalQuoteSeedErrorMsg = "Please enter sale quote seed";
        public static string EnterInvoiceSeedErrorMsg = "Please enter Sale Return seed";
        public static string InvalidPurEntrySeedErrorMsg = "Purchase Entry Seed start from 1, Please enter valid Seed";
        public static string InvalidPurOrderSeedErrorMsg = "Purchase order Seed start from 1, Please enter valid Seed";
        public static string InvalidBillSeedErrorMsg = "Purchase return Seed start from 1, Please enter valid Seed";
        public static string InvalidSalEntrySeedErrorMsg = "Sale Entry Seed start from 1, Please enter valid Seed";
        public static string InvalidSalQuoteSeedErrorMsg = "Sale Quote Seed start from 1, Please enter valid Seed";
        public static string InvalidInvoiceSeedErrorMsg = "Sale Return Seed start from 1, Please enter valid Seed";
        public static string ChoosePaperFormatErrorMsg = "Please choose {0} paper format";
        public static string InvalidSeedErrorMsg = "{0} Seed start from 1, Please enter valid Seed";
        public static string EnterSeedErrorMsg = "Please enter {0} seed";

        Container parent;
        public bool CreateCompanyOnLoad = false;
        UserManager UserManager = null!;
        AddressManager AddressManager = null!;
        CompanyManager CompanyManager = null!;
        StateManager StateManager = null!;
        CountryManager CountryManager = null!;
        CompanyTypeManager CompanyTypeManager = null!;
        KeypressValidation Keypress_Validation = null!;
        CostCenterManager CostCenterManager = null!;
        AccountManager AccountManager = null!;
        SupplierManager SupplierManager = null!;
        CustomerManager CustomerManager = null!;
        ContactInfoManager ContactInfoManager = null!;
        TaxinfoManager TaxinfoManager = null!;
        fa.api.Accounting.CurrencyManager CurrencyManager = null!;
        AccountingMethodManager AccountingMethodManager = null!;
        bool[] DaybookHasEntry = null!;


        public FormCompany(object sender)
        {
            parent = (Container)sender;
            UserManager = UserManager.Instance;
            AddressManager = AddressManager.Instance;
            CompanyManager = CompanyManager.Instance;
            StateManager = StateManager.Instance;
            CountryManager = CountryManager.Instance;
            CompanyTypeManager = CompanyTypeManager.Instance;
            Keypress_Validation = KeypressValidation.Instance;
            CostCenterManager = CostCenterManager.Instance;
            AccountManager = AccountManager.Instance;
            SupplierManager = SupplierManager.Instance;
            CustomerManager = CustomerManager.Instance;
            ContactInfoManager = ContactInfoManager.Instance;
            TaxinfoManager = TaxinfoManager.Instance;
            CurrencyManager = fa.api.Accounting.CurrencyManager.Instance;
            AccountingMethodManager = AccountingMethodManager.Instance;
            InitializeComponent();
            excludedObjects = new string[] { "TextBoxCompanySearch" };
        }

        private void FormCompany_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                IntializeBusinessType();
                ResetForm();
                EnableForm(false);
                LoadComboBox();
                LoadCompaniesWithFilter();
                if (TreeViewCompany.Nodes.Count > 0) { DelayedTextBoxCompanySearch.Select(); } else { BtnCompanyNew.Select(); }
                if (CreateCompanyOnLoad)
                {
                    this.Text = CreateCompanyOnloadText;
                    BtnCompanyNew.Visible = false;
                    BtnCompanyDelete.Visible = false;
                    BtnCompanyCancel.Visible = false;
                    this.BtnCompanyEdit.Visible = false;
                    TreeViewCompany.Visible = false;
                    TabControlCompany.Location = new Point(12, 12);
                    BtnCompanySave.Location = new Point(TabControlCompany.Width - 74, TabControlCompany.Height + 15);
                    this.Size = new Size(TabControlCompany.Right + 25, TabControlCompany.Bottom + 90);
                    this.CenterToParent();
                    BtnCompanyNew_Click(this, null!);
                }
                this.formIsDirty = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void IntializeBusinessType()
        {
            ComboBoxBusinessType.Items.Clear();
            if (Global.softwareType == SoftwareType.MEDICARE)
            {
                ComboBoxBusinessType.Items.Add("Hospital");
                ComboBoxBusinessType.SelectedIndex = 0;
            }
            else
            {
                ComboBoxBusinessType.Items.AddRange(new[] { "Retail", "Whole Sale", "Professional Office", "Pharmacy" });
                ComboBoxBusinessType.SelectedIndex = 0;
            }
        }
        private void LoadComboBox()
        {
            ComboUtils.InitializeCountryCombo(ComboBoxCompanyCountry);
            ComboUtils.InitializeCurrencyCombo(ComboBoxCompanyCurrency);
            ComboUtils.InitializeCompanyTypeCombo(ComboBoxCompanyType);
            ComboUtils.InitializeAccountMethodCombo(ComboBoxCompanyAccountMethod);
            ComboUtils.InitializeCompanyCombo(ComboBoxParentCompany, CompanyManager.GetAccessibleParentCompanies(Global.User));
        }

        private void InitializeAccountMaps(long CompanyId)
        {
            ComboUtils.InitializeAccountCombo(ComboBoxCompanyUndepositedFundAccount, CompanyId);
            ComboUtils.InitializeAccountCombo(ComboBoxCompanyCashOnHandAccount, CompanyId);
            ComboUtils.InitializeAccountCombo(ComboBoxCompanySalesAccount, CompanyId);
            ComboUtils.InitializeAccountCombo(ComboBoxCompanySalesReturnFeeAccount, CompanyId);
            ComboUtils.InitializeAccountCombo(ComboBoxCompanyPurchaseAccount, CompanyId);
            ComboUtils.InitializeAccountCombo(ComboBoxCompanyAccountRecivable, CompanyId);
            ComboUtils.InitializeAccountCombo(ComboBoxCompanyAccountPayable, CompanyId);
            ComboUtils.InitializeAccountCombo(ComboBoxCompanySalesTaxPayableAccount, CompanyId);
            ComboUtils.InitializeAccountCombo(comboBoxCompanyRoundOffAccount, CompanyId);
        }
        private void LoadCompaniesWithFilter()
        {
            string FilterString = DelayedTextBoxCompanySearch.Text.Trim();
            TreeViewCompany.Nodes.Clear();
            //IList<Company> Companies = CompanyManager.GetCompanies();
            IList<Company> Companies = CompanyManager.GetCompaniesBySoftwareType(Global.softwareType);
            if (Companies != null)
            {
                foreach (var lCompany in Companies)
                {
                    if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim()) || lCompany.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        TreeNode treeNode = new TreeNode();
                        treeNode.Text = lCompany.ToString();
                        treeNode.Name = lCompany.CompanyId.ToString();
                        if (lCompany.ParentCompanyId == null)
                        {
                            treeNode.ImageIndex = 0;
                            TreeViewCompany.Nodes.Add(treeNode);
                        }
                        else
                        {
                            TreeNode[] items = TreeViewCompany.Nodes.Find(lCompany.ParentCompanyId.ToString(), true);
                            if (items.Count() > 0)
                            {
                                TreeNode item = items[0];
                                if (item != null)
                                {
                                    treeNode.ImageIndex = 1;
                                    item.Nodes.Add(treeNode);
                                }
                            }
                        }
                    }
                }
            }
            if (TreeViewCompany.Nodes.Count > 0)
            {
                if (!string.IsNullOrEmpty(DelayedTextBoxCompanySearch.Text))
                {
                    TreeViewCompany.ExpandAll();
                }
                TreeViewCompany.SelectedNode = TreeViewCompany.Nodes[0];
            }
        }
        private Company GetCompanyInfo()
        {
            if (TreeViewCompany.SelectedNode != null)
            {
                Company CompanyFromDB = CompanyManager.GetCompany(long.Parse(TreeViewCompany.SelectedNode.Name));
                return CompanyFromDB;
            }
            return null!;
        }
        private void LoadCompanyInfo()
        {
            Company CompanyFromDB = GetCompanyInfo();
            if (CompanyFromDB != null)
            {
                this.Text = "Company - " + CompanyFromDB.Name;
                //load default account combo
                InitializeAccountMaps(CompanyFromDB.CompanyId);
                TextBoxCompanyId.Text = CompanyFromDB.CompanyId.ToString();
                TextBoxCompanyName.Text = CompanyFromDB.Name;
                TextBoxCompanyLegalName.Text = CompanyFromDB.DisplayAs;
                CheckBoxIsBranchOffice.Checked = CompanyFromDB.IsBranch;
                CheckBoxHasProductCatalog.Checked = CompanyFromDB.HasProductCatalog;
                CheckBoxMaintainRackNumber.Checked = CompanyFromDB.MaintainRackNumber;
                TextBoxCompanySlogan.Text = CompanyFromDB.Slogan;
                if (CompanyFromDB.Logo != null)
                {
                    try
                    {
                        using (MemoryStream stream = new MemoryStream(CompanyFromDB.Logo))
                        {
                            CompanyPictureBoxLogo.Image = System.Drawing.Image.FromStream(stream);
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine("Invalid image format: " + ex.Message);
                    }
                }
                if (CompanyFromDB.Country != null)
                {
                    ComboBoxCompanyCountry.SelectedIndex = ComboBoxCompanyCountry.FindStringExact(CompanyFromDB.Country.Name);
                }
                if (CompanyFromDB.Address != null)
                {
                    Address lAddress = CompanyFromDB.Address;
                    AddressGroupBoxCompany.AddressLine1 = lAddress.AddressLine1;
                    AddressGroupBoxCompany.AddressLine2 = lAddress.AddressLine2;
                    AddressGroupBoxCompany.CityName = lAddress.CityOrTown;
                    AddressGroupBoxCompany.DistrictName = lAddress.District;
                    AddressGroupBoxCompany.StateId = lAddress.StatesId != null ? (long)lAddress.StatesId : 0L;
                    AddressGroupBoxCompany.PinCode = lAddress.PinCode;

                }
                if (CompanyFromDB.ContactInfo != null)
                {
                    TextBoxCompanyPhone.Text = CompanyFromDB.ContactInfo.Phone;
                    TextBoxCompanyMobile.Text = CompanyFromDB.ContactInfo.Mobile;
                    TextBoxCompanyFax.Text = CompanyFromDB.ContactInfo.Fax;
                    TextBoxCompanyEmail.Text = CompanyFromDB.ContactInfo.Email;
                    TextBoxCompanyWebsite.Text = CompanyFromDB.ContactInfo.WebSite;
                }


                if (CompanyFromDB.TaxInfo != null)
                {
                    for (int i = 0; i < CompanyDataGridViewTaxType.Rows.Count; i++)
                    {
                        if (CompanyDataGridViewTaxType.Rows[i].Cells[0].Value.ToString() == "PAN")
                        {
                            CompanyDataGridViewTaxType.Rows[i].Cells[1].Value = CompanyFromDB.TaxInfo.PAN;
                        }
                        if (CompanyDataGridViewTaxType.Rows[i].Cells[0].Value.ToString() == "CST")
                        {
                            CompanyDataGridViewTaxType.Rows[i].Cells[1].Value = CompanyFromDB.TaxInfo.CST;
                        }
                        if (CompanyDataGridViewTaxType.Rows[i].Cells[0].Value.ToString() == "GST")
                        {
                            CompanyDataGridViewTaxType.Rows[i].Cells[1].Value = CompanyFromDB.TaxInfo.GST;
                        }
                        if (CompanyDataGridViewTaxType.Rows[i].Cells[0].Value.ToString() == "TIN")
                        {
                            CompanyDataGridViewTaxType.Rows[i].Cells[1].Value = CompanyFromDB.TaxInfo.TIN;
                        }
                    }
                }

                if (CompanyFromDB.CompanyType != null)
                {
                    ComboBoxCompanyType.SelectedIndex = ComboBoxCompanyType.FindStringExact(CompanyFromDB.CompanyType.Name);
                }

                if (CompanyFromDB.PrimaryCurrency != null)
                {
                    ComboBoxCompanyCurrency.SelectedIndex = ComboBoxCompanyCurrency.FindStringExact(CompanyFromDB.PrimaryCurrency.Name);
                }
                if (Convert.ToString(CompanyFromDB.QuantityPricision) != null)
                {
                    ComboBoxCompanyQtyPricision.SelectedIndex = ComboBoxCompanyQtyPricision.FindStringExact(CompanyFromDB.QuantityPricision.ToString());
                }
                if (CompanyFromDB.AccountingMethod != null)
                {
                    ComboBoxCompanyAccountMethod.SelectedIndex = ComboBoxCompanyAccountMethod.FindStringExact(CompanyFromDB.AccountingMethod.Name);
                }
                if (CompanyFromDB.DateFormat != null)
                {
                    ComboBoxCompanyDateFormat.SelectedIndex = ComboBoxCompanyDateFormat.FindStringExact(CompanyFromDB.DateFormat.ToString());
                }

                if (!CompanyFromDB.AccountingStartDate.Equals(null))
                {
                    ComboBoxCompanyFirstMonthFinYear.SelectedIndex = CompanyFromDB.AccountingStartDate - 1;
                }
                if (!CompanyFromDB.IncomeTaxStartDate.Equals(null))
                {
                    ComboBoxCompanyFirstMonthIncomTaxYear.SelectedIndex = CompanyFromDB.IncomeTaxStartDate - 1;
                }

                if (CompanyFromDB.IsBranch)
                {
                    Company lCompany = CompanyManager.GetCompany((long)CompanyFromDB.ParentCompanyId!);
                    if (lCompany != null)
                    {
                        ComboBoxParentCompany.SelectedIndex = ComboBoxParentCompany.FindStringExact(lCompany.Name);
                    }
                }
                if (CompanyFromDB.SalesTaxAccountMaps.Count > 0)
                {
                    IList<Account> Account = AccountManager.ListCompanySalesTaxReceivableAccounts(CompanyFromDB.CompanyId, Global.IncludedAccountCompanySalesTaxReceivable);
                    int j = 0;
                    foreach (var SalesAccount in CompanyFromDB.SalesTaxAccountMaps)
                    {
                        (GridViewSalesTaxReceivable.Rows[j].Cells[(int)CompanySaleTaxReceivableTableColumn.ACCOUNT] as DataGridViewComboBoxCell)!.DataSource = null;
                        (GridViewSalesTaxReceivable.Rows[j].Cells[(int)CompanySaleTaxReceivableTableColumn.ACCOUNT] as DataGridViewComboBoxCell)!.DataSource = Account;
                        (GridViewSalesTaxReceivable.Rows[j].Cells[(int)CompanySaleTaxReceivableTableColumn.ACCOUNT] as DataGridViewComboBoxCell)!.ValueMember = "Id";
                        (GridViewSalesTaxReceivable.Rows[j].Cells[(int)CompanySaleTaxReceivableTableColumn.ACCOUNT] as DataGridViewComboBoxCell)!.DisplayMember = "Name";
                        GridViewSalesTaxReceivable.Rows[j].Cells[(int)CompanySaleTaxReceivableTableColumn.NAME].Value = SalesAccount.Name;
                        GridViewSalesTaxReceivable.Rows[j].Cells[(int)CompanySaleTaxReceivableTableColumn.ACCOUNT].Value = SalesAccount.AccountId;
                        GridViewSalesTaxReceivable.Rows[j].Cells[(int)CompanySaleTaxReceivableTableColumn.ID].Value = SalesAccount.MapId;
                        GridViewSalesTaxReceivable.Rows[j].Cells[(int)CompanySaleTaxReceivableTableColumn.ACCID].Value = SalesAccount.AccountId;
                        GridViewSalesTaxReceivable.Rows[j].Cells[(int)CompanySaleTaxReceivableTableColumn.TAXID].Value = SalesAccount.CountrySaleTaxId;

                        j++;
                    }

                }

                if (CompanyFromDB.CompanyLicence.Count > 0)
                {
                    CompanyTaxInfoGrid.Rows.Add(CompanyFromDB.CompanyLicence.Count);
                    int j = 0;
                    foreach (var CompanyLicence in CompanyFromDB.CompanyLicence)
                    {
                        CompanyTaxInfoGrid.Rows[j].Cells[(int)CompanyTaxInfoTableColumn.NAME].Value = CompanyLicence.Name;
                        CompanyTaxInfoGrid.Rows[j].Cells[(int)CompanyTaxInfoTableColumn.DNAME].Value = CompanyLicence.DisplayName;
                        CompanyTaxInfoGrid.Rows[j].Cells[(int)CompanyTaxInfoTableColumn.VALUE].Value = CompanyLicence.Value;
                        CompanyTaxInfoGrid.Rows[j].Cells[(int)CompanyTaxInfoTableColumn.INVOICE].Value = CompanyLicence.IncludeInInvoice;
                        CompanyTaxInfoGrid.Rows[j].Cells[(int)CompanyTaxInfoTableColumn.REPORT].Value = CompanyLicence.IncludeInReport;
                        CompanyTaxInfoGrid.Rows[j].Cells[(int)CompanyTaxInfoTableColumn.ID].Value = CompanyLicence.Id;
                        j++;
                    }

                }

                if (CompanyFromDB.CompanyCustomerLicenseMaster.Count > 0)
                {
                    CustomerTaxInfoGrid.Rows.Add(CompanyFromDB.CompanyCustomerLicenseMaster.Count);
                    int j = 0;
                    foreach (var CompanyCustomerLicence in CompanyFromDB.CompanyCustomerLicenseMaster)
                    {
                        CustomerTaxInfoGrid.Rows[j].Cells[(int)CustomerTaxInfoTableColumn.NAME].Value = CompanyCustomerLicence.Name;
                        CustomerTaxInfoGrid.Rows[j].Cells[(int)CustomerTaxInfoTableColumn.DNAME].Value = CompanyCustomerLicence.DisplayName;
                        CustomerTaxInfoGrid.Rows[j].Cells[(int)CustomerTaxInfoTableColumn.REQUIR].Value = CompanyCustomerLicence.Required;
                        CustomerTaxInfoGrid.Rows[j].Cells[(int)CustomerTaxInfoTableColumn.REPORT].Value = CompanyCustomerLicence.IncludeInReport;
                        CustomerTaxInfoGrid.Rows[j].Cells[(int)CustomerTaxInfoTableColumn.INVOICE].Value = CompanyCustomerLicence.IncludeInInvoice;
                        CustomerTaxInfoGrid.Rows[j].Cells[(int)CustomerTaxInfoTableColumn.ID].Value = CompanyCustomerLicence.Id;
                        j++;
                    }

                }

                if (CompanyFromDB.CompanySupplierLicenseMaster.Count > 0)
                {
                    SupplierTaxInfoGrid.Rows.Add(CompanyFromDB.CompanySupplierLicenseMaster.Count);
                    int j = 0;
                    foreach (var CompanySupplierLicence in CompanyFromDB.CompanySupplierLicenseMaster)
                    {
                        SupplierTaxInfoGrid.Rows[j].Cells[(int)SupplierTaxInfoTableColumn.NAME].Value = CompanySupplierLicence.Name;
                        SupplierTaxInfoGrid.Rows[j].Cells[(int)SupplierTaxInfoTableColumn.DNAME].Value = CompanySupplierLicence.DisplayName;
                        SupplierTaxInfoGrid.Rows[j].Cells[(int)SupplierTaxInfoTableColumn.REQUIR].Value = CompanySupplierLicence.Required;
                        SupplierTaxInfoGrid.Rows[j].Cells[(int)SupplierTaxInfoTableColumn.REPORT].Value = CompanySupplierLicence.IncludeInReport;
                        SupplierTaxInfoGrid.Rows[j].Cells[(int)SupplierTaxInfoTableColumn.ID].Value = CompanySupplierLicence.Id;
                        j++;
                    }

                }


                if (CompanyFromDB.UndepositedFundAccount != null)
                {
                    ComboBoxCompanyUndepositedFundAccount.SelectedIndex = ComboBoxCompanyUndepositedFundAccount.FindStringExact(CompanyFromDB.UndepositedFundAccount.Name);
                }
                if (CompanyFromDB.CashOnHandAccount != null)
                {
                    ComboBoxCompanyCashOnHandAccount.SelectedIndex = ComboBoxCompanyCashOnHandAccount.FindStringExact(CompanyFromDB.CashOnHandAccount.Name);
                }
                if (CompanyFromDB.SalesAccount != null)
                {
                    ComboBoxCompanySalesAccount.SelectedIndex = ComboBoxCompanySalesAccount.FindStringExact(CompanyFromDB.SalesAccount.Name);
                }
                if (CompanyFromDB.SalesReturnFeeAccount != null)
                {
                    ComboBoxCompanySalesReturnFeeAccount.SelectedIndex = ComboBoxCompanySalesReturnFeeAccount.FindStringExact(CompanyFromDB.SalesReturnFeeAccount.Name);
                }
                if (CompanyFromDB.PurchaseAccount != null)
                {
                    ComboBoxCompanyPurchaseAccount.SelectedIndex = ComboBoxCompanyPurchaseAccount.FindStringExact(CompanyFromDB.PurchaseAccount.Name);
                }
                if (CompanyFromDB.AccountRecivable != null)
                {
                    ComboBoxCompanyAccountRecivable.SelectedIndex = ComboBoxCompanyAccountRecivable.FindStringExact(CompanyFromDB.AccountRecivable.Name);
                }
                if (CompanyFromDB.AccountPayable != null)
                {
                    ComboBoxCompanyAccountPayable.SelectedIndex = ComboBoxCompanyAccountPayable.FindStringExact(CompanyFromDB.AccountPayable.Name);
                }
                if (CompanyFromDB.IncomceAccount != null)
                {
                    ComboBoxCompanySalesTaxPayableAccount.SelectedIndex = ComboBoxCompanySalesTaxPayableAccount.FindStringExact(CompanyFromDB.IncomceAccount.Name);
                }
                if (CompanyFromDB.RoundOffAccount != null)
                {
                    comboBoxCompanyRoundOffAccount.SelectedIndex = comboBoxCompanyRoundOffAccount.FindStringExact(CompanyFromDB.RoundOffAccount.Name);
                }
                if (CompanyFromDB.IdSpaces != null)
                {
                    // ComboBoxCompanyYear.SelectedIndex= ComboBoxCompanyYear.FindStringExact(CompanyFromDB.IdSpaces.FirstOrDefault());
                }
                GridViewMiscellaneousTransSales.CompanyId = CompanyFromDB.CompanyId;
                GridViewMiscellaneousTransPurchase.CompanyId = CompanyFromDB.CompanyId;
                GridViewMiscellaneousTransSales.Clear();
                GridViewMiscellaneousTransPurchase.Clear();

                if (CompanyFromDB.CompanyPurchaseSetup != null)
                {
                    CompanyPurchaseSetup PurchaseSetup = CompanyManager.GetPurchaseSetupById(CompanyFromDB.CompanyPurchaseSetupId);
                    if (PurchaseSetup.AdditionalTransactions != null)
                    {
                        if (PurchaseSetup.AdditionalTransactions != null && PurchaseSetup.AdditionalTransactions.Count > 0)
                            GridViewMiscellaneousTransPurchase.CompanyTransactionSetups = PurchaseSetup.AdditionalTransactions;
                    }
                }
                if (CompanyFromDB.CompanySalesSetup != null)
                {
                    CompanySalesSetup SalesSetup = CompanyManager.GetCompanySalesSetupWithInclude(CompanyFromDB);
                    if (SalesSetup.AdditionalTransactions != null)
                    {
                        if (SalesSetup.AdditionalTransactions != null && SalesSetup.AdditionalTransactions.Count > 0)
                            GridViewMiscellaneousTransSales.CompanyTransactionSetups = SalesSetup.AdditionalTransactions;
                    }

                    if (!SalesSetup.PriceType.Equals(null))
                    {
                        ComboBoxInvoicePriceBy.SelectedIndex = (int)SalesSetup.PriceType;
                    }
                    YesNoRadioCombineItem.Checked = SalesSetup.CombineItem;
                    CheckBoxIncludeTax.Checked = SalesSetup.IncludingTax;
                    CheckBoxIncludeTax.Enabled = false;
                    CheckBoxIsReceivePayment.Checked = SalesSetup.IsReceivePayment;
                    CheckBoxIsDelivery.Checked = SalesSetup.IsDelivery;
                    CheckBoxIsNegativeStockAllow.Checked = SalesSetup.IsNegativeStockAllowed;
                    CheckBoxPrintQRCode.Checked = SalesSetup.IsPrintQRCode;
                    TextBoxQRCode.Text = SalesSetup.UPIId;
                    YesNoRbtSalesType.Checked = (SalesSetup.DefaultSalesType == SaleMethod.Credit) ? true : false;
                    CheckBoxDisplayBankDetails.Checked = SalesSetup.IsBankDetailDisplayOnInvoice;
                    CheckBoxDisplayDeclaration.Checked = SalesSetup.IsDeclarationDisplayOnInvoice;
                    TextBoxCompanyBankDetails.Text = !string.IsNullOrEmpty(SalesSetup.BankDetails) ? SalesSetup.BankDetails.Replace(",", System.Environment.NewLine) : string.Empty;
                    TextBoxCompanyDeclaration.Text = !string.IsNullOrEmpty(SalesSetup.Declarations) ? SalesSetup.Declarations.Replace(",", System.Environment.NewLine) : string.Empty;
                }
                if (!CompanyFromDB.BusinessType.Equals(null))
                {
                    int BusinessTypeIndex = (int)CompanyFromDB.BusinessType;

                    if (BusinessTypeIndex >= 0 && BusinessTypeIndex < ComboBoxBusinessType.Items.Count)
                    {
                        ComboBoxBusinessType.SelectedIndex = BusinessTypeIndex;
                    }
                    else if (BusinessTypeIndex == ComboBoxBusinessType.Items.Count)
                    {
                        ComboBoxBusinessType.SelectedIndex = ComboBoxBusinessType.Items.Count - 1;
                    }
                }

                ComboUtils.InitializeYearCombo(ComboBoxCompanyYear, CompanyFromDB);
            }
            else
            {
                DisplaySystemError("Somthing went wrong, the selected company is not valid.");
                return;
            }
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            this.formIsDirty = false;
            LoadCompaniesWithFilter();
            return;
        }

        private Company GetCompanyFromForm()
        {
            Company lCompany = new Company();
            if (!string.IsNullOrEmpty(TextBoxCompanyId.Text))
            {
                lCompany.CompanyId = Convert.ToInt64(TextBoxCompanyId.Text);
            }
            else
            {
                lCompany.CompanyId = 0L;
            }
            lCompany.Name = TextBoxCompanyName.Text.Trim();
            lCompany.DisplayAs = TextBoxCompanyLegalName.Text.Trim();
            lCompany.IsBranch = CheckBoxIsBranchOffice.Checked;
            lCompany.DateFormat = ComboBoxCompanyDateFormat.Text;
            lCompany.Slogan = TextBoxCompanySlogan.Text;
            lCompany.HasProductCatalog = CheckBoxHasProductCatalog.Checked;
            lCompany.MaintainRackNumber = CheckBoxMaintainRackNumber.Checked;
            lCompany.QuantityPricision = int.Parse(ComboBoxCompanyQtyPricision.SelectedItem.ToString()!);
            if (ComboBoxCompanyFirstMonthFinYear.SelectedIndex > -1)
            {
                lCompany.AccountingStartDate = ComboBoxCompanyFirstMonthFinYear.SelectedIndex + 1;
            }

            if (ComboBoxCompanyFirstMonthIncomTaxYear.SelectedIndex > -1)
            {
                lCompany.IncomeTaxStartDate = ComboBoxCompanyFirstMonthIncomTaxYear.SelectedIndex + 1;
            }
            if (ComboBoxCompanyAccountMethod.SelectedIndex > -1)
            {
                AccountingMethod lAccountingMethod = (AccountingMethod)ComboBoxCompanyAccountMethod.Items[ComboBoxCompanyAccountMethod.SelectedIndex];
                if (lAccountingMethod != null)
                {
                    lAccountingMethod = AccountingMethodManager.GetAccountingMethodById(lAccountingMethod.AccountingMethodId);
                    if (lAccountingMethod != null)
                    {
                        lCompany.AccountingMethodId = lAccountingMethod.AccountingMethodId;
                    }
                }
            }

            if (ComboBoxCompanyType.SelectedIndex > -1)
            {
                CompanyType lCompanyType = (CompanyType)ComboBoxCompanyType.Items[ComboBoxCompanyType.SelectedIndex];
                if (lCompanyType != null)
                {
                    lCompanyType = CompanyTypeManager.GetCompanyTypeById(lCompanyType.Id);
                    if (lCompanyType != null)
                    {
                        lCompany.CompanyTypeId = lCompanyType.Id;
                    }
                }
            }
            if (ComboBoxCompanyCountry.SelectedIndex > -1)
            {
                Country lCountry = (Country)ComboBoxCompanyCountry.Items[ComboBoxCompanyCountry.SelectedIndex];
                if (lCountry != null)
                {
                    lCompany.CountryId = lCountry.Id;
                }
            }

            if (ComboBoxCompanyCurrency.SelectedIndex > -1)
            {
                Currency lCurrency = (Currency)ComboBoxCompanyCurrency.Items[ComboBoxCompanyCurrency.SelectedIndex];
                if (lCurrency != null)
                {
                    lCompany.PrimaryCurrencyId = lCurrency.CurrencyId;
                }
            }

            if (CheckBoxIsBranchOffice.Checked == true)
            {
                Company lCompanyBranch = (Company)ComboBoxParentCompany.Items[ComboBoxParentCompany.SelectedIndex];
                if (lCompanyBranch != null)
                {
                    lCompany.ParentCompanyId = lCompanyBranch.CompanyId;
                }
                lCompany.IsBranch = true;
            }
            else
            {
                lCompany.IsBranch = false;
            }
            if (CompanyPictureBoxLogo.Image != null)
            {
                MemoryStream Stream = new MemoryStream();
                CompanyPictureBoxLogo.Image.Save(Stream, System.Drawing.Imaging.ImageFormat.Png);
                lCompany.Logo = Stream.ToArray();
            }
            Account Account = null!;
            if (ComboBoxCompanyUndepositedFundAccount.SelectedIndex > -1)
            {
                Account = (Account)ComboBoxCompanyUndepositedFundAccount.Items[ComboBoxCompanyUndepositedFundAccount.SelectedIndex];
                if (Account != null)
                {
                    lCompany.UndepositedFundAccountId = Account.Id;
                }
            }
            if (ComboBoxCompanyCashOnHandAccount.SelectedIndex > -1)
            {
                Account = (Account)ComboBoxCompanyCashOnHandAccount.Items[ComboBoxCompanyCashOnHandAccount.SelectedIndex];
                if (Account != null)
                {
                    lCompany.CashOnHandAccountId = Account.Id;
                }
            }
            if (ComboBoxCompanySalesAccount.SelectedIndex > -1)
            {
                Account = (Account)ComboBoxCompanySalesAccount.Items[ComboBoxCompanySalesAccount.SelectedIndex];
                if (Account != null)
                {
                    lCompany.SalesAccountId = Account.Id;
                }
            }
            if (ComboBoxCompanySalesReturnFeeAccount.SelectedIndex > -1)
            {
                Account = (Account)ComboBoxCompanySalesReturnFeeAccount.Items[ComboBoxCompanySalesReturnFeeAccount.SelectedIndex];
                if (Account != null)
                {
                    lCompany.SalesReturnFeeAccountId = Account.Id;
                }
            }
            if (ComboBoxCompanyPurchaseAccount.SelectedIndex > -1)
            {
                Account = (Account)ComboBoxCompanyPurchaseAccount.Items[ComboBoxCompanyPurchaseAccount.SelectedIndex];
                if (Account != null)
                {
                    lCompany.PurchaseAccountId = Account.Id;
                }
            }
            if (ComboBoxCompanyAccountRecivable.SelectedIndex > -1)
            {
                Account = (Account)ComboBoxCompanyAccountRecivable.Items[ComboBoxCompanyAccountRecivable.SelectedIndex];
                if (Account != null)
                {
                    lCompany.AccountRecivableId = Account.Id;
                }
            }
            if (ComboBoxCompanyAccountPayable.SelectedIndex > -1)
            {
                Account = (Account)ComboBoxCompanyAccountPayable.Items[ComboBoxCompanyAccountPayable.SelectedIndex];
                if (Account != null)
                {
                    lCompany.AccountPayableId = Account.Id;
                }
            }
            if (ComboBoxCompanySalesTaxPayableAccount.SelectedIndex > -1)
            {
                Account = (Account)ComboBoxCompanySalesTaxPayableAccount.Items[ComboBoxCompanySalesTaxPayableAccount.SelectedIndex];
                if (Account != null)
                {
                    lCompany.IncomceAccountId = Account.Id;
                }
            }
            if (comboBoxCompanyRoundOffAccount.SelectedIndex > -1)
            {
                Account = (Account)comboBoxCompanyRoundOffAccount.Items[comboBoxCompanyRoundOffAccount.SelectedIndex];
                if (Account != null)
                {
                    lCompany.RoundOffAccountId = Account.Id;
                }
            }
            if (GridViewSalesTaxReceivable.Rows.Count > 0)
            {
                lCompany.SalesTaxAccountMaps = new List<CompanySalesTaxAccountMap>() { };
                for (int i = 0; i < GridViewSalesTaxReceivable.Rows.Count; i++)
                {
                    CompanySalesTaxAccountMap CompanySalesTaxAccountMap = new CompanySalesTaxAccountMap();
                    CompanySalesTaxAccountMap.MapId = GridViewSalesTaxReceivable.Rows[i].Cells[(int)CompanySaleTaxReceivableTableColumn.ID].Value == null ? 0L : (long)GridViewSalesTaxReceivable.Rows[i].Cells[(int)CompanySaleTaxReceivableTableColumn.ID].Value;
                    CompanySalesTaxAccountMap.CountrySaleTaxId = GridViewSalesTaxReceivable.Rows[i].Cells[(int)CompanySaleTaxReceivableTableColumn.TAXID].Value == null ? 0L : (long)GridViewSalesTaxReceivable.Rows[i].Cells[(int)CompanySaleTaxReceivableTableColumn.TAXID].Value;
                    CompanySalesTaxAccountMap.Name = GridViewSalesTaxReceivable.Rows[i].Cells[(int)CompanySaleTaxReceivableTableColumn.NAME].Value.ToString()!.Trim();
                    if (GridViewSalesTaxReceivable.Rows[i].Cells[(int)CompanySaleTaxReceivableTableColumn.ACCOUNT].Value != null)
                    {
                        Account lAccount = AccountManager.Instance.GetAccountById((long)GridViewSalesTaxReceivable.Rows[i].Cells[(int)CompanySaleTaxReceivableTableColumn.ACCOUNT].Value);
                        if (lAccount != null)
                        {
                            CompanySalesTaxAccountMap.AccountId = lAccount.Id;
                        }
                    }
                    lCompany.SalesTaxAccountMaps.Add(CompanySalesTaxAccountMap);
                }
            }

            if (CompanyTaxInfoGrid.Rows.Count > 0)
            {
                lCompany.CompanyLicence = new List<CompanyLicence>() { };
                for (int i = 0; i < CompanyTaxInfoGrid.Rows.Count - 1; i++)
                {
                    CompanyLicence CompanyLicence = new CompanyLicence();
                    CompanyLicence.Id = CompanyTaxInfoGrid.Rows[i].Cells[(int)CompanyTaxInfoTableColumn.ID].Value == null ? 0L : (long)CompanyTaxInfoGrid.Rows[i].Cells[(int)CompanyTaxInfoTableColumn.ID].Value;
                    CompanyLicence.Name = CompanyTaxInfoGrid.Rows[i].Cells[(int)CompanyTaxInfoTableColumn.NAME].Value.ToString();
                    if (CompanyTaxInfoGrid.Rows[i].Cells[(int)CompanyTaxInfoTableColumn.DNAME].Value != null)
                    {
                        CompanyLicence.DisplayName = CompanyTaxInfoGrid.Rows[i].Cells[(int)CompanyTaxInfoTableColumn.DNAME].Value.ToString()!.Trim();
                    }
                    CompanyLicence.Value = CompanyTaxInfoGrid.Rows[i].Cells[(int)CompanyTaxInfoTableColumn.VALUE].Value.ToString()!.Trim();
                    CompanyLicence.IncludeInInvoice = CompanyTaxInfoGrid.Rows[i].Cells[(int)CompanyTaxInfoTableColumn.INVOICE].Value != null ? (bool)CompanyTaxInfoGrid.Rows[i].Cells[(int)CompanyTaxInfoTableColumn.INVOICE].Value : false;
                    CompanyLicence.IncludeInReport = CompanyTaxInfoGrid.Rows[i].Cells[(int)CompanyTaxInfoTableColumn.REPORT].Value != null ? (bool)CompanyTaxInfoGrid.Rows[i].Cells[(int)CompanyTaxInfoTableColumn.REPORT].Value : false;

                    lCompany.CompanyLicence.Add(CompanyLicence);
                }
            }

            if (CustomerTaxInfoGrid.Rows.Count > 0)
            {
                lCompany.CompanyCustomerLicenseMaster = new List<CompanyCustomerLicenseMaster>() { };
                for (int i = 0; i < CustomerTaxInfoGrid.Rows.Count - 1; i++)
                {
                    CompanyCustomerLicenseMaster CompanyCustomerLicenseMaster = new CompanyCustomerLicenseMaster();
                    CompanyCustomerLicenseMaster.Id = CustomerTaxInfoGrid.Rows[i].Cells[(int)CustomerTaxInfoTableColumn.ID].Value == null ? 0L : (long)CustomerTaxInfoGrid.Rows[i].Cells[(int)CustomerTaxInfoTableColumn.ID].Value;
                    CompanyCustomerLicenseMaster.Name = CustomerTaxInfoGrid.Rows[i].Cells[(int)CustomerTaxInfoTableColumn.NAME].Value.ToString()!.Trim();
                    CompanyCustomerLicenseMaster.DisplayName = CustomerTaxInfoGrid.Rows[i].Cells[(int)CustomerTaxInfoTableColumn.DNAME].Value?.ToString()?? "";
                    CompanyCustomerLicenseMaster.Required = CustomerTaxInfoGrid.Rows[i].Cells[(int)CustomerTaxInfoTableColumn.REQUIR].Value != null ? (bool)CustomerTaxInfoGrid.Rows[i].Cells[(int)CustomerTaxInfoTableColumn.REQUIR].Value : false;
                    CompanyCustomerLicenseMaster.IncludeInReport = CustomerTaxInfoGrid.Rows[i].Cells[(int)CustomerTaxInfoTableColumn.REPORT].Value != null ? (bool)CustomerTaxInfoGrid.Rows[i].Cells[(int)CustomerTaxInfoTableColumn.REPORT].Value : false;
                    CompanyCustomerLicenseMaster.IncludeInInvoice = CustomerTaxInfoGrid.Rows[i].Cells[(int)CustomerTaxInfoTableColumn.INVOICE].Value != null ? (bool)CustomerTaxInfoGrid.Rows[i].Cells[(int)CustomerTaxInfoTableColumn.INVOICE].Value : false;

                    lCompany.CompanyCustomerLicenseMaster.Add(CompanyCustomerLicenseMaster);
                }
            }

            if (SupplierTaxInfoGrid.Rows.Count > 0)
            {
                lCompany.CompanySupplierLicenseMaster = new List<CompanySupplierLicenseMaster>() { };
                for (int i = 0; i < SupplierTaxInfoGrid.Rows.Count - 1; i++)
                {
                    CompanySupplierLicenseMaster CompanySupplierLicenseMaster = new CompanySupplierLicenseMaster();
                    CompanySupplierLicenseMaster.Id = SupplierTaxInfoGrid.Rows[i].Cells[(int)SupplierTaxInfoTableColumn.ID].Value == null ? 0L : (long)SupplierTaxInfoGrid.Rows[i].Cells[(int)SupplierTaxInfoTableColumn.ID].Value;
                    CompanySupplierLicenseMaster.Name = SupplierTaxInfoGrid.Rows[i].Cells[(int)SupplierTaxInfoTableColumn.NAME].Value.ToString()!.Trim();
                    CompanySupplierLicenseMaster.DisplayName = SupplierTaxInfoGrid.Rows[i].Cells[(int)SupplierTaxInfoTableColumn.DNAME].Value.ToString()!.Trim();
                    CompanySupplierLicenseMaster.Required = SupplierTaxInfoGrid.Rows[i].Cells[(int)SupplierTaxInfoTableColumn.REQUIR].Value != null ? (bool)SupplierTaxInfoGrid.Rows[i].Cells[(int)SupplierTaxInfoTableColumn.REQUIR].Value : false;
                    CompanySupplierLicenseMaster.IncludeInReport = SupplierTaxInfoGrid.Rows[i].Cells[(int)SupplierTaxInfoTableColumn.REPORT].Value != null ? (bool)SupplierTaxInfoGrid.Rows[i].Cells[(int)SupplierTaxInfoTableColumn.REPORT].Value : false;

                    lCompany.CompanySupplierLicenseMaster.Add(CompanySupplierLicenseMaster);
                }
            }
            if (Global.softwareType == SoftwareType.MEDICARE)
            {
                if (ComboBoxBusinessType.Items.Count >= 1 && ComboBoxBusinessType.Items.Contains("Hospital"))
                {
                    lCompany.BusinessType = BuisnessType.Hospital;
                }
            }
            else if (Global.softwareType == SoftwareType.VVMATRIX && ComboBoxBusinessType.SelectedIndex >= 0)
            {
                switch (ComboBoxBusinessType.SelectedIndex)
                {
                    case 0:
                        lCompany.BusinessType = BuisnessType.Retail;
                        break;
                    case 1:
                        lCompany.BusinessType = BuisnessType.Wholesale;
                        break;
                    case 2:
                        lCompany.BusinessType = BuisnessType.Professionals;
                        break;
                    case 3:
                        lCompany.BusinessType = BuisnessType.Pharmacy;
                        break;
                    default:
                        break;
                }
            }
            lCompany.Narration = new List<Narration>();

            return lCompany;
        }
        private List<IdSpace> ListCompanyIdSpacesFromForm()
        {
            Company Company = CompanyManager.GetCompanyForModel(long.Parse(TextBoxCompanyId.Text));
            List<IdSpace> lIdSpace = new List<IdSpace>();
            String Year = (string)ComboBoxCompanyYear.Items[ComboBoxCompanyYear.SelectedIndex];
            string[] Years = Year.Split('-');
            string Start = new DateTime(int.Parse(Years[0]), Company.AccountingStartDate, 1).ToString(Global.Company.DateFormat);
            string End = new DateTime(int.Parse(Years[0]), Company.AccountingStartDate, 1).AddYears(1).AddDays(-1).ToString(Global.Company.DateFormat);
            foreach (DataGridViewRow Row in GridViewCompanyYear.Rows)
            {
                IdSpace CompanyEntryConfiguration = new IdSpace();
                CompanyEntryConfiguration.EntryType = (EntryType)Row.Cells[(int)CompanyReferenceTableColumn.TYPEID].Value;
                CompanyEntryConfiguration.IsResetDaily = Row.Cells[(int)CompanyReferenceTableColumn.DAILYRESET].Value != null ? (bool)Row.Cells[(int)CompanyReferenceTableColumn.DAILYRESET].Value : false;
                CompanyEntryConfiguration.IsDotMatrix = Row.Cells[(int)CompanyReferenceTableColumn.DOTMATRIX].Value != null ? (bool)Row.Cells[(int)CompanyReferenceTableColumn.DOTMATRIX].Value : false;
                CompanyEntryConfiguration.Prefix = Row.Cells[(int)CompanyReferenceTableColumn.PREFIX].Value != null ? Row.Cells[(int)CompanyReferenceTableColumn.PREFIX].Value.ToString() : string.Empty;
                CompanyEntryConfiguration.Seed = long.Parse(Row.Cells[(int)CompanyReferenceTableColumn.SEED].Value.ToString()!);
                CompanyEntryConfiguration.RunningSeed = long.Parse(Row.Cells[(int)CompanyReferenceTableColumn.SEED].Value.ToString()!);
                CompanyEntryConfiguration.YearStartDate = DateTime.ParseExact(Start, Global.Company.DateFormat, null);
                CompanyEntryConfiguration.YearEndDate = DateTime.ParseExact(End, Global.Company.DateFormat, null);
                CompanyEntryConfiguration.Date = Global.getTransactionDate();
                if (Row.Cells[(int)CompanyReferenceTableColumn.PRINTTYPE].Value != null)
                {
                    CompanyEntryConfiguration.PrintPaperFormat_Id = (long)Row.Cells[(int)CompanyReferenceTableColumn.PRINTTYPE].Value;
                }
                CompanyEntryConfiguration.RoundOff = Row.Cells[(int)CompanyReferenceTableColumn.ROUNDOFF].Value != null ? double.Parse(Row.Cells[(int)CompanyReferenceTableColumn.ROUNDOFF].Value.ToString()!) : 0;
                lIdSpace.Add(CompanyEntryConfiguration);
            }
            return lIdSpace;
        }
        private CompanyPurchaseSetup GetCompanyPurchaseSetupFromForm()
        {
            CompanyPurchaseSetup lCompanyPurchaseSetup = new CompanyPurchaseSetup();
            if (GridViewMiscellaneousTransPurchase.CompanyTransactionSetups != null)
            {
                lCompanyPurchaseSetup.AdditionalTransactions = GridViewMiscellaneousTransPurchase.CompanyTransactionSetups.ToList();
            }
            return lCompanyPurchaseSetup;
        }

        private CompanySalesSetup GetCompanySalesSetupFromForm()
        {
            CompanySalesSetup lCompanySalesSetup = new CompanySalesSetup();
            if (GridViewMiscellaneousTransSales.CompanyTransactionSetups != null)
            {
                lCompanySalesSetup.AdditionalTransactions = GridViewMiscellaneousTransSales.CompanyTransactionSetups.ToList();
            }
            lCompanySalesSetup.PriceType = (PriceType)ComboBoxInvoicePriceBy.SelectedIndex;
            lCompanySalesSetup.CombineItem = YesNoRadioCombineItem.Checked;
            lCompanySalesSetup.IsReceivePayment = CheckBoxIsReceivePayment.Checked;
            lCompanySalesSetup.IncludingTax = CheckBoxIncludeTax.Checked;
            lCompanySalesSetup.IsDelivery = CheckBoxIsDelivery.Checked;
            lCompanySalesSetup.IsNegativeStockAllowed = CheckBoxIsNegativeStockAllow.Checked;
            lCompanySalesSetup.IsPrintQRCode = CheckBoxPrintQRCode.Checked;
            lCompanySalesSetup.UPIId = TextBoxQRCode.Text;
            lCompanySalesSetup.DefaultSalesType = (YesNoRbtSalesType.Checked) ? SaleMethod.Credit : SaleMethod.Cash;
            lCompanySalesSetup.IsBankDetailDisplayOnInvoice = CheckBoxDisplayBankDetails.Checked;
            lCompanySalesSetup.IsDeclarationDisplayOnInvoice = CheckBoxDisplayDeclaration.Checked;
            lCompanySalesSetup.BankDetails = TextBoxCompanyBankDetails.Text.Replace("\r", "").Replace(",", "").Replace("\n", ",");
            lCompanySalesSetup.Declarations = TextBoxCompanyDeclaration.Text.Replace("\r", "").Replace(",", "").Replace("\n", ",");
            return lCompanySalesSetup;
        }
        private CompanyStockMovementSetup GetCompanyStockMovementSetupFromForm()
        {
            CompanyStockMovementSetup lCompanyStockMovementSetup = new CompanyStockMovementSetup();
            return lCompanyStockMovementSetup;
        }
        private Address GetCompanyAddressFromForm()
        {
            Address lAddress = new Address();
            lAddress.AddressLine1 = AddressGroupBoxCompany.AddressLine1.Trim();
            lAddress.AddressLine2 = AddressGroupBoxCompany.AddressLine2.Trim();
            lAddress.CityOrTown = AddressGroupBoxCompany.CityName.Trim();
            lAddress.District = AddressGroupBoxCompany.DistrictName.Trim();
            lAddress.PinCode = AddressGroupBoxCompany.PinCode.Trim();
            lAddress.StatesId = AddressGroupBoxCompany.StateId;

            return lAddress;
        }
        private TaxInfo GetCompanyTaxInfoFromForm()
        {
            TaxInfo lTaxInfo = new TaxInfo();
            for (int i = 0; i < CompanyDataGridViewTaxType.Rows.Count; i++)
            {
                if (CompanyDataGridViewTaxType.Rows[i].Cells[0].Value.ToString() == "PAN" && CompanyDataGridViewTaxType.Rows[i].Cells[1].Value != null)
                {
                    lTaxInfo.PAN = CompanyDataGridViewTaxType.Rows[i].Cells[1].Value.ToString();
                }
                if (CompanyDataGridViewTaxType.Rows[i].Cells[0].Value.ToString() == "CST" && CompanyDataGridViewTaxType.Rows[i].Cells[1].Value != null)
                {
                    lTaxInfo.CST = CompanyDataGridViewTaxType.Rows[i].Cells[1].Value.ToString();
                }
                if (CompanyDataGridViewTaxType.Rows[i].Cells[0].Value.ToString() == "GST" && CompanyDataGridViewTaxType.Rows[i].Cells[1].Value != null)
                {
                    lTaxInfo.GST = CompanyDataGridViewTaxType.Rows[i].Cells[1].Value.ToString();
                }
                if (CompanyDataGridViewTaxType.Rows[i].Cells[0].Value.ToString() == "TIN" && CompanyDataGridViewTaxType.Rows[i].Cells[1].Value != null)
                {
                    lTaxInfo.TIN = CompanyDataGridViewTaxType.Rows[i].Cells[1].Value.ToString();
                }
            }
            return lTaxInfo;
        }
        private ContactInfo GetCompanyContactInfoFromForm()
        {
            ContactInfo lContactInfo = new ContactInfo
            {
                Phone = TextBoxCompanyPhone.Text.Trim().Replace("-", ""),
                Mobile = TextBoxCompanyMobile.Text.Trim().Replace("-", ""),
                Fax = TextBoxCompanyFax.Text.Trim(),
                Email = TextBoxCompanyEmail.Text.Trim(),
                WebSite = TextBoxCompanyWebsite.Text.Trim()
            };
            return lContactInfo;
        }
        private void TreeViewCompany_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            TreeNode node = e.Node!;
            node.SelectedImageIndex = node.ImageIndex;
            ResetForm();
            LoadComboBox();
            LoadCompanyInfo();
            EnableForm(false);
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void LoadCompanyCostCenterInContainer()
        {
            List<Company> AccessibleCompanies = CompanyManager.GetAccessibleCompanies(Global.User);
            if (AccessibleCompanies.Count > 0)
            {
                parent.RenderLoginDetails();
            }
        }
        private void BtnCompanyNew_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            LoadComboBox();
            EnableForm(true);
            ComboBoxCompanyYear.Visible = false;
            EnableDefaultAccount(false);
            GridViewMiscellaneousTransPurchase.Enabled = false;
            GridViewMiscellaneousTransSales.Enabled = false;
            TabControlCompany.SelectedTab = TabCompanyInfo;
            TextBoxCompanyName.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }

        private void BtnCompanyDelete_Click(object sender, EventArgs e)
        {
            toolStripStatusLabelError.Text = "";
            if (string.IsNullOrEmpty(TextBoxCompanyId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this company is still valid.");
                return;
            }
            Company CompanyInfo = GetCompanyInfo();
            if (CompanyInfo != null)
            {
                DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, CompanyInfo.Name), "Delete Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Company lCompany = CompanyManager.CheckCompanyBranch(CompanyInfo.CompanyId);
                    if (lCompany == null)
                    {
                        bool Delete = CompanyManager.DeleteCompany(CompanyInfo.CompanyId);
                        if (Delete)
                        {
                            IList<Company> Company = CompanyManager.GetCompanies();
                            if (Company.Count < 1)
                            {
                                parent.Logout();
                                this.Hide();
                            }
                            else
                            {
                                if (CompanyInfo.CompanyId == Global.Company.CompanyId)
                                {
                                    Global.Company = null!;
                                    this.Hide();
                                    this.Close();
                                }
                                else
                                {
                                    parent.RenderLoginDetails();
                                    ResetForm();
                                    TabControlCompany.SelectedTab = TabCompanyInfo;
                                    LoadComboBox();
                                    LoadCompaniesWithFilter();
                                    EnableForm(false);
                                }
                            }

                        }
                        else
                        {
                            toolStripStatusLabelError.Text = DeleteErrorText;
                        }
                    }
                    else
                    {
                        toolStripStatusLabelError.Text = IsbranchDeleteErrorText;
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            else
            {
                DisplaySystemError("Somting went wrong, please check this company is still valid.");
                return;
            }
        }

        private void BtnCompanyEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxCompanyId.Text))
            {
                DisplaySystemError("Somting went wrong, please check this company is still valid.");
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            EnableForm(true);
            ComboBoxCompanyCountry.Visible = false;
            if (CheckBoxIsBranchOffice.Checked == false)
            {
                CheckBoxIsBranchOffice.Enabled = false;
                ComboBoxParentCompany.Visible = false;
            }
            CheckBoxIncludeTax.Enabled = ComboBoxInvoicePriceBy.SelectedIndex == 3 ? false : true;

            TabControlCompany.SelectedTab = TabCompanyInfo;
            TextBoxCompanyName.Focus();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }

        private void BtnCompanyCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TabControlCompany.SelectedTab = TabCompanyInfo;
                    TextBoxCompanyName.Select();
                    return;
                }
            }
            if (CreateCompanyOnLoad)
            {
                this.Close();
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                TabControlCompany.SelectedTab = TabCompanyInfo;
                LoadComboBox();
                if (string.IsNullOrEmpty(DelayedTextBoxCompanySearch.Text))
                {
                    LoadCompanyInfo();
                }
                else
                {
                    DelayedTextBoxCompanySearch.Clear();
                }
                EnableForm(false);
                DelayedTextBoxCompanySearch.Select();
                this.formIsDirty = false;
                Cursor.Current = Cursors.Default;
            }

        }
        private void BtnCompanySave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Cursor.Current = Cursors.WaitCursor;
                Company lCompany = GetCompanyFromForm();
                if (CompanyManager.CompanyNameUniqueById(lCompany))
                {
                    if (lCompany.CompanyId == 0)
                    {
                        if (CompanyManager.GetCompanies().Count == 0)
                        {
                            DateTime? toDate = DateUtils.ToDate(DateTime.Now.ToString(lCompany.DateFormat.ToString()), lCompany.DateFormat);
                            Global.setTransactionDate(toDate ?? DateTime.Now);
                        }
                        lCompany.Address = GetCompanyAddressFromForm();
                        lCompany.TaxInfo = GetCompanyTaxInfoFromForm();
                        lCompany.ContactInfo = GetCompanyContactInfoFromForm();
                        lCompany.CompanyPurchaseSetup = GetCompanyPurchaseSetupFromForm();
                        lCompany.CompanySalesSetup = GetCompanySalesSetupFromForm();
                        lCompany.CompanyStockMovementSetup = GetCompanyStockMovementSetupFromForm();
                        CompanyManager.AddCompany(lCompany);
                        LoadCompanyCostCenterInContainer();
                    }
                    else
                    {
                        Company lCompanyById = CompanyManager.GetCompany(lCompany.CompanyId);
                        if (lCompanyById != null)
                        {
                            Address lAddress = GetCompanyAddressFromForm();
                            ContactInfo lContactInfo = GetCompanyContactInfoFromForm();
                            TaxInfo lTaxInfo = GetCompanyTaxInfoFromForm();
                            CompanyPurchaseSetup CompanyPurchaseSetup = GetCompanyPurchaseSetupFromForm();
                            CompanySalesSetup CompanySalesSetup = GetCompanySalesSetupFromForm();
                            CompanyStockMovementSetup CompanyStockMovementSetup = GetCompanyStockMovementSetupFromForm();
                            lCompany.CompanyPurchaseSetupId = CompanyPurchaseSetup.Id = (long)lCompanyById.CompanyPurchaseSetupId!;
                            lCompany.CompanySalesSetupId = CompanySalesSetup.Id = (long)lCompanyById.CompanySalesSetupId!;
                            lCompany.CompanyStockMovementSetupId = CompanyStockMovementSetup.Id = (long)lCompanyById.CompanyStockMovementSetupId!;
                            if (ComboBoxCompanyYear.SelectedIndex > -1)
                            {
                                lCompany.IdSpaces = ListCompanyIdSpacesFromForm();
                            }
                            lCompany.CompanyPurchaseSetup = CompanyPurchaseSetup;
                            lCompany.CompanySalesSetup = CompanySalesSetup;
                            lCompany.CompanyStockMovementSetup = CompanyStockMovementSetup;
                            lCompany.AddressId = lCompanyById.AddressId;
                            lCompany.ContactInfoId = lCompanyById.ContactInfoId;
                            lCompany.TaxInfoId = lCompanyById.TaxInfoId;
                            lAddress.AddressId = (long)lCompanyById.AddressId!;
                            lContactInfo.Id = (long)lCompanyById.ContactInfoId!;
                            lTaxInfo.Id = (long)lCompanyById.TaxInfoId!;
                            CompanyManager.UpdateCompany(lCompany);
                            Address lAddressFromDB = AddressManager.UpdateAddress(lAddress);
                            ContactInfo lContactInfoFromDB = ContactInfoManager.UpdateContactInfo(lContactInfo);
                            TaxInfo lAddressTaxInfoFromDB = TaxinfoManager.UpdateTaxInfo(lTaxInfo);
                            if (Global.Company.CompanyId == lCompany.CompanyId)
                            {
                                Global.Company = CompanyManager.GetCompany(lCompany.CompanyId);
                                parent.LoadGlobalData();
                                LoadCompanyCostCenterInContainer();
                                parent.EnableReceivePayment();
                            }
                            else
                            {
                                LoadCompanyCostCenterInContainer();
                            }
                        }
                        else
                        {
                            DisplaySystemError("Somting went wrong, please check this company is still valid.");
                            return;
                        }
                    }
                    if (CreateCompanyOnLoad)
                    {
                        this.Hide();
                        Cursor.Current = Cursors.WaitCursor;
                    }
                    else
                    {
                        ResetForm();
                        TabControlCompany.SelectedTab = TabCompanyInfo;
                        LoadComboBox();
                        if (string.IsNullOrEmpty(DelayedTextBoxCompanySearch.Text))
                        {
                            LoadCompaniesWithFilter();
                        }
                        else
                        {
                            DelayedTextBoxCompanySearch.Clear();
                        }
                        TreeNode TreeNode = new TreeNode();
                        TreeNode = lCompany.ParentCompanyId == null ? TreeViewCompany.Nodes[lCompany.CompanyId.ToString()] : TreeViewCompany.Nodes[lCompany.ParentCompanyId.ToString()].Nodes[lCompany.CompanyId.ToString()];
                        TreeViewCompany.SelectedNode = TreeNode;
                        TreeViewCompany.Focus();
                        EnableForm(false);
                    }
                    this.formIsDirty = false;
                    if (CreateCompanyOnLoad)
                    {
                        Global.Company = CompanyManager.GetCompany(lCompany.CompanyId);
                        Cursor.Current = Cursors.Default;
                        this.Close();
                    }
                }
                else
                {
                    toolStripStatusLabelError.Text = string.Format(UniqueCompanyNameErrorMsg, lCompany.Name);
                    TabControlCompany.SelectedTab = TabCompanyInfo;
                    TextBoxCompanyName.Select();
                }
                toolStripStatusLabelError.Text = SaveSuccessText;
            }
            Cursor.Current = Cursors.Default;
        }
        private void BtnCurrencyExit_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Close();
            Cursor.Current = Cursors.Default;
        }
        private Boolean ValidateForm()
        {
            toolStripStatusLabelError.Text = "";
            if (string.IsNullOrEmpty(TextBoxCompanyName.Text.Trim()))
            {
                toolStripStatusLabelError.Text = EnterCompanyNameErrorMsg;
                TabControlCompany.SelectedTab = TabCompanyInfo;
                TextBoxCompanyName.Select();
                return false;
            }

            if (CheckBoxIsBranchOffice.Checked == true && ComboBoxParentCompany.SelectedIndex < 0)
            {
                toolStripStatusLabelError.Text = ChooseParentErrorMsg;
                toolStripStatusLabelError.Text = ChooseParentErrorMsg;
                TabControlCompany.SelectedTab = TabCompanyInfo;
                ComboBoxParentCompany.Select();
                return false;

            }
            if (CheckBoxIsBranchOffice.Checked == true && ComboBoxParentCompany.SelectedIndex >= 0 && CompanyManager.GetCompany(((Company)ComboBoxParentCompany.Items[ComboBoxParentCompany.SelectedIndex]).CompanyId) == null)
            {
                toolStripStatusLabelError.Text = "Somting went wrong, please check this parent company is still valid.";
                TabControlCompany.SelectedTab = TabCompanyInfo;
                ComboBoxParentCompany.Select();
                return false;

            }
            if (ComboBoxCompanyCountry.SelectedIndex < 0)
            {
                toolStripStatusLabelError.Text = ChooseCountryErrorMsg;
                TabControlCompany.SelectedTab = TabCompanyInfo;
                ComboBoxCompanyCountry.Select();
                return false;
            }
            if (AddressGroupBoxCompany.StateId == 0L)
            {
                toolStripStatusLabelError.Text = ChooseStateErrorMsg;
                TabControlCompany.SelectedTab = TabCompanyInfo;
                AddressGroupBoxCompany.selected_field(Fields.state);
                return false;
            }
            if (!string.IsNullOrEmpty(TextBoxCompanyEmail.Text.Trim()) && !Keypress_Validation.EmailValidation(TextBoxCompanyEmail.Text.Trim()))
            {
                toolStripStatusLabelError.Text = EnterCompanyEmailErrorMsg;
                TabControlCompany.SelectedTab = TabCompanyInfo;
                TextBoxCompanyEmail.Select();
                return false;
            }
            if (!string.IsNullOrEmpty(TextBoxCompanyWebsite.Text.Trim()) && !Keypress_Validation.WebsiteValidation(TextBoxCompanyWebsite.Text.Trim()))
            {
                toolStripStatusLabelError.Text = EnterCompanyWebsiteErrorMsg;
                TabControlCompany.SelectedTab = TabCompanyInfo;
                TextBoxCompanyWebsite.Select();
                return false;
            }
            if (ComboBoxCompanyType.SelectedIndex < 0)
            {
                toolStripStatusLabelError.Text = ChooseCompanyTypeErrorMsg;
                TabControlCompany.SelectedTab = TabSettings;
                ComboBoxCompanyType.Select();
                return false;
            }

            if (ComboBoxCompanyFirstMonthFinYear.SelectedIndex < 0)
            {
                toolStripStatusLabelError.Text = ChooseFiscalYearErrorMsg;
                TabControlCompany.SelectedTab = TabSettings;
                ComboBoxCompanyFirstMonthFinYear.Select();
                return false;

            }

            if (ComboBoxCompanyFirstMonthIncomTaxYear.SelectedIndex < 0)
            {
                toolStripStatusLabelError.Text = ChooseIncomeTaxYearErrorMsg;
                TabControlCompany.SelectedTab = TabSettings;
                ComboBoxCompanyFirstMonthIncomTaxYear.Select();
                return false;
            }

            if (ComboBoxCompanyAccountMethod.SelectedIndex < 0)
            {
                toolStripStatusLabelError.Text = ChooseAccountingMethodErrorMsg;
                TabControlCompany.SelectedTab = TabSettings;
                ComboBoxCompanyAccountMethod.Select();
                return false;

            }
            if (ComboBoxCompanyQtyPricision.SelectedIndex < 0)
            {
                toolStripStatusLabelError.Text = string.Format(ChoosePricisionErrorMsg, ComboBoxCompanyQtyPricision.Items[0].ToString(), ComboBoxCompanyQtyPricision.Items[ComboBoxCompanyQtyPricision.Items.Count - 1].ToString());
                TabControlCompany.SelectedTab = TabSettings;
                ComboBoxCompanyQtyPricision.Select();
                return false;
            }
            if (ComboBoxCompanyCurrency.SelectedIndex < 0)
            {
                toolStripStatusLabelError.Text = ChooseCurrencyErrorMsg;
                TabControlCompany.SelectedTab = TabSettings;
                ComboBoxCompanyCurrency.Select();
                return false;
            }
            if (ComboBoxCompanyDateFormat.SelectedIndex < 0)
            {
                toolStripStatusLabelError.Text = ChooseDateFormatErrorMsg;
                TabControlCompany.SelectedTab = TabSettings;
                ComboBoxCompanyDateFormat.Select();
                return false;
            }
            if (ComboBoxBusinessType.SelectedIndex < 0)
            {
                toolStripStatusLabelError.Text = ChooseBusinessTypeErrorMsg;
                TabControlCompany.SelectedTab = TabSettings;
                ComboBoxBusinessType.Select();
                return false;
            }

            if (GridViewSalesTaxReceivable.Rows.Count > 0 && !string.IsNullOrEmpty(TextBoxCompanyId.Text))
            {
                for (int i = 0; i < GridViewSalesTaxReceivable.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < GridViewSalesTaxReceivable.Rows.Count; j++)
                    {
                        if (GridViewSalesTaxReceivable.Rows[i].Cells[j].Value == null || string.IsNullOrEmpty(GridViewSalesTaxReceivable.Rows[i].Cells[j].Value.ToString()!.Trim()))
                        {
                            toolStripStatusLabelError.Text = string.Format(TaxInfoGrid_MantatoryFiledErrorMsg, GridViewSalesTaxReceivable.Columns[j].HeaderText);
                            TabControlCompany.SelectedTab = TabAccountPreference;
                            GridViewSalesTaxReceivable.Select();
                            GridViewSalesTaxReceivable.CurrentCell = GridViewSalesTaxReceivable[j, i];
                            return false;
                        }
                    }
                    int Count = 0;
                    for (int k = 0; k < GridViewSalesTaxReceivable.Rows.Count - 1; k++)
                    {
                        if (GridViewSalesTaxReceivable.Rows[k].Cells[0].Value != null)
                        {
                            if (GridViewSalesTaxReceivable.Rows[i].Cells[0].Value.ToString()!.Trim() == GridViewSalesTaxReceivable.Rows[k].Cells[0].Value.ToString()!.Trim())
                            {
                                Count++;
                            }
                        }
                        if (Count > 1)
                        {
                            toolStripStatusLabelError.Text = string.Format(TaxInfoGrid_UniqueTaxNameErrorMsg, GridViewSalesTaxReceivable.Rows[k].Cells[0].Value.ToString()!.Trim());
                            TabControlCompany.SelectedTab = TabAccountPreference;
                            GridViewSalesTaxReceivable.Select();
                            GridViewSalesTaxReceivable.CurrentCell = GridViewSalesTaxReceivable[0, k];
                            return false;
                        }
                    }
                }
            }

            //company license
            if (CompanyTaxInfoGrid.Rows.Count > 1)
            {
                for (int i = 0; i < CompanyTaxInfoGrid.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (j == 1) { continue; }
                        if (CompanyTaxInfoGrid.Rows[i].Cells[j].Value == null || string.IsNullOrEmpty(CompanyTaxInfoGrid.Rows[i].Cells[j].Value.ToString()!.Trim()))
                        {
                            toolStripStatusLabelError.Text = string.Format(TaxInfoGrid_MantatoryFiledErrorMsg, CompanyTaxInfoGrid.Columns[j].HeaderText);
                            TabControlCompany.SelectedTab = TabLicenseInfo;
                            CompanyTaxInfoGrid.Select();
                            CompanyTaxInfoGrid.CurrentCell = CompanyTaxInfoGrid[j, i];
                            return false;
                        }
                    }
                    int Count = 0;
                    for (int k = 0; k < CompanyTaxInfoGrid.Rows.Count - 1; k++)
                    {
                        if (CompanyTaxInfoGrid.Rows[k].Cells[0].Value != null)
                        {
                            if (CompanyTaxInfoGrid.Rows[i].Cells[0].Value.ToString()!.Trim() == CompanyTaxInfoGrid.Rows[k].Cells[0].Value.ToString()!.Trim())
                            {
                                Count++;
                            }
                        }
                        if (Count > 1)
                        {
                            toolStripStatusLabelError.Text = string.Format(TaxInfoGrid_UniqueTaxNameErrorMsg, CompanyTaxInfoGrid.Rows[k].Cells[0].Value.ToString()!.Trim());
                            TabControlCompany.SelectedTab = TabLicenseInfo;
                            CompanyTaxInfoGrid.Select();
                            CompanyTaxInfoGrid.CurrentCell = CompanyTaxInfoGrid[0, k];
                            return false;
                        }
                    }
                }
            }




            if (!GridViewMiscellaneousTransPurchase.IsValidationResult())
            {
                TabControlCompany.SelectedTab = TabPurchase;
                toolStripStatusLabelError.Text = GridViewMiscellaneousTransPurchase.ErrorMsg();
                GridViewMiscellaneousTransPurchase.Focus();
                return false;
            }
            if (ComboBoxInvoicePriceBy.SelectedIndex < 0)
            {
                toolStripStatusLabelError.Text = ChoosePriceTypeErrorMsg;
                TabControlCompany.SelectedTab = TabSales;
                ComboBoxInvoicePriceBy.Select();
                return false;
            }
            if (CheckBoxPrintQRCode.Checked && string.IsNullOrEmpty(TextBoxQRCode.Text))
            {
                toolStripStatusLabelError.Text = EnterUPIIdErrorMsg;
                TabControlCompany.SelectedTab = TabSales;
                TextBoxQRCode.Select();
                return false;
            }
            if (!GridViewMiscellaneousTransSales.IsValidationResult())
            {
                TabControlCompany.SelectedTab = TabSales;
                GridViewMiscellaneousTransSales.Focus();
                toolStripStatusLabelError.Text = GridViewMiscellaneousTransSales.ErrorMsg();
                return false;
            }

            //customer license
            if (!ValidateGrid(CustomerTaxInfoGrid)) { return false; }
            //Supplier license
            if (!ValidateGrid(SupplierTaxInfoGrid)) { return false; }
            //company reference
            if (!ValidateReferenceGrid(GridViewCompanyYear)) { return false; }
            return true;
        }

        private bool ValidateGrid(DataViewVerticalScroll GridView)
        {
            if (GridView.Rows.Count > 1)
            {
                for (int i = 0; i < GridView.Rows.Count - 1; i++)
                {
                    if (GridView.Rows[i].Cells[0].Value == null || string.IsNullOrEmpty(GridView.Rows[i].Cells[0].Value.ToString()!.Trim()))
                    {
                        toolStripStatusLabelError.Text = string.Format(TaxInfoGrid_MantatoryFiledErrorMsg, GridView.Columns[0].HeaderText);
                        TabControlCompany.SelectedTab = TabTaxInfo;
                        GridView.Select();
                        GridView.CurrentCell = GridView[0, i];
                        GridView.BeginEdit(true);
                        return false;
                    }
                    int Count = 0;
                    for (int k = 0; k < GridView.Rows.Count - 1; k++)
                    {
                        if (GridView.Rows[k].Cells[0].Value != null)
                        {
                            if (GridView.Rows[i].Cells[0].Value.ToString()!.Trim() == GridView.Rows[k].Cells[0].Value.ToString()!.Trim())
                            {
                                Count++;
                            }
                        }
                        if (Count > 1)
                        {
                            toolStripStatusLabelError.Text = string.Format(TaxInfoGrid_UniqueTaxNameErrorMsg, GridView.Rows[k].Cells[0].Value.ToString()!.Trim());
                            TabControlCompany.SelectedTab = TabTaxInfo;
                            GridView.Select();
                            GridView.CurrentCell = GridView[0, k];
                            GridView.BeginEdit(true);
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        private bool ValidateReferenceGrid(DataViewVerticalScroll GridView)
        {
            for (int i = 0; i < GridView.Rows.Count - 1; i++)
            {
                if (GridView.Rows[i].Cells[(int)CompanyReferenceTableColumn.SEED].Value == null || string.IsNullOrEmpty(GridView.Rows[i].Cells[(int)CompanyReferenceTableColumn.SEED].Value.ToString()!.Trim()) || long.Parse(GridView.Rows[i].Cells[(int)CompanyReferenceTableColumn.SEED].Value.ToString()!) == 0)
                {
                    toolStripStatusLabelError.Text = string.Format(EnterSeedErrorMsg, GridView.Rows[i].Cells[(int)CompanyReferenceTableColumn.TYPE].Value.ToString());
                    TabControlCompany.SelectedTab = TabReference;
                    GridView.Select();
                    GridView.CurrentCell = GridView[(int)CompanyReferenceTableColumn.SEED, i];
                    GridView.BeginEdit(true);
                    return false;
                }
                if ((bool)GridView.Rows[i].Cells[(int)CompanyReferenceTableColumn.HASPRINTERSETUP].Value && (GridView.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE].Value == null || string.IsNullOrEmpty(GridView.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE].Value.ToString()!.Trim())))
                {
                    toolStripStatusLabelError.Text = string.Format(ChoosePaperFormatErrorMsg, GridView.Rows[i].Cells[(int)CompanyReferenceTableColumn.TYPE].Value.ToString());
                    TabControlCompany.SelectedTab = TabReference;
                    GridView.Select();
                    GridView.CurrentCell = GridView[(int)CompanyReferenceTableColumn.PRINTTYPE, i];
                    GridView.BeginEdit(true);
                    return false;
                }
            }
            return true;
        }
        private void ResetForm()
        {
            toolStripStatusLabelError.Text = "";
            AddressGroupBoxCompany.Clear();
            TextBoxCompanyId.ResetText();
            TextBoxCompanyName.ResetText();
            TextBoxCompanyLegalName.ResetText();
            TextBoxCompanySlogan.ResetText();
            CheckBoxIsBranchOffice.Checked = false;
            Index = -1;
            TextBoxCompanyWebsite.ResetText();
            TextBoxCompanyEmail.ResetText();
            TextBoxCompanyPhone.ResetText();
            TextBoxCompanyMobile.ResetText();
            TextBoxCompanyFax.ResetText();
            ComboBoxCompanyCountry.SelectedIndex = -1;
            ComboBoxParentCompany.SelectedIndex = -1;
            ComboBoxCompanyType.SelectedIndex = -1;
            ComboBoxCompanyFirstMonthIncomTaxYear.SelectedIndex = -1;
            ComboBoxCompanyFirstMonthFinYear.SelectedIndex = -1;
            ComboBoxCompanyDateFormat.SelectedIndex = -1;
            ComboBoxCompanyCurrency.SelectedIndex = -1;
            ComboBoxCompanyAccountMethod.SelectedIndex = -1;
            ComboBoxCompanyQtyPricision.SelectedIndex = -1;
            ComboBoxCompanyYear.SelectedIndex = -1;
            ComboBoxCompanyUndepositedFundAccount.SelectedIndex = -1;
            ComboBoxCompanyCashOnHandAccount.SelectedIndex = -1;
            ComboBoxCompanySalesAccount.SelectedIndex = -1;
            ComboBoxCompanySalesReturnFeeAccount.SelectedIndex = -1;
            ComboBoxCompanyPurchaseAccount.SelectedIndex = -1;
            ComboBoxCompanyAccountRecivable.SelectedIndex = -1;
            ComboBoxCompanyAccountPayable.SelectedIndex = -1;
            ComboBoxCompanySalesTaxPayableAccount.SelectedIndex = -1;
            comboBoxCompanyRoundOffAccount.SelectedIndex = -1;
            CompanyPictureBoxLogo.Image = null;
            CompanyDataGridViewTaxType.Rows.Clear();
            GridViewSalesTaxReceivable.Rows.Clear();
            CompanyTaxInfoGrid.Rows.Clear();
            CustomerTaxInfoGrid.Rows.Clear();
            SupplierTaxInfoGrid.Rows.Clear();
            CheckBoxHasProductCatalog.Checked = false;
            CheckBoxMaintainRackNumber.Checked = false;
            GridViewMiscellaneousTransSales.CompanyId = null;
            GridViewMiscellaneousTransPurchase.CompanyId = null;
            GridViewMiscellaneousTransPurchase.Clear();
            GridViewMiscellaneousTransSales.Clear();
            GridViewCompanyYear.Rows.Clear();
            ComboBoxBusinessType.SelectedIndex = -1;
            ComboBoxBusinessType.SelectedIndex = 0;
            ComboBoxInvoicePriceBy.SelectedIndex = -1;
            ComboBoxInvoicePriceBy.SelectedIndex = 0;
            YesNoRadioCombineItem.Checked = false;
            CheckBoxIsDelivery.Checked = false;
            CheckBoxIsReceivePayment.Checked = false;
            YesNoRbtSalesType.Checked = false;
            CheckBoxIsNegativeStockAllow.Checked = false;
            TextBoxQRCode.ResetText();
            CheckBoxPrintQRCode.Checked = false;
            CheckBoxDisplayBankDetails.Checked = false;
            CheckBoxDisplayDeclaration.Checked = false;
            TextBoxCompanyBankDetails.ResetText();
            TextBoxCompanyDeclaration.ResetText();
            CheckBoxIncludeTax.Checked = false;
            CheckBoxIncludeTax.Enabled = true;
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridViewCompanyYear.Columns["RoundOff"];
            if (Global.Company != null && int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
        }
        private void EnableDefaultAccount(Boolean enable)
        {
            GridViewSalesTaxReceivable.Enabled = enable;
            ComboBoxCompanyUndepositedFundAccount.Visible = enable;
            ComboBoxCompanyCashOnHandAccount.Visible = enable;
            ComboBoxCompanySalesAccount.Visible = enable;
            ComboBoxCompanySalesReturnFeeAccount.Visible = enable;
            ComboBoxCompanyPurchaseAccount.Visible = enable;
            ComboBoxCompanyAccountRecivable.Visible = enable;
            ComboBoxCompanyAccountPayable.Visible = enable;
            ComboBoxCompanySalesTaxPayableAccount.Visible = enable;
            comboBoxCompanyRoundOffAccount.Visible = enable;
        }
        private void EnablePurchaseSalesReference(Boolean enable)
        {
            GridViewMiscellaneousTransPurchase.Enabled = enable;
            GridViewMiscellaneousTransSales.Enabled = enable;
            //ComboBoxCompanyYear.Visible = false;
            GridViewCompanyYear.ReadOnly = true;
            BtnCompanyAddYear.Enabled = false;
            long CompanyId = string.IsNullOrEmpty(TextBoxCompanyId.Text) ? 0L : long.Parse(TextBoxCompanyId.Text);
            if (CompanyId != 0L)
            {
                //ComboBoxCompanyYear.Visible = true;
                BtnCompanyAddYear.Enabled = enable;
                GridViewCompanyYear.ReadOnly = !enable;
            }
            CheckBoxDisplayBankDetails.Enabled = enable;
            CheckBoxDisplayDeclaration.Enabled = enable;

            TextBoxCompanyBankDetails.ReadOnly = !enable;
            TextBoxCompanyDeclaration.ReadOnly = !enable;
            TextBoxCompanyBankDetails.TabStop = enable;
            TextBoxCompanyDeclaration.TabStop = enable;
            TextBoxQRCode.ReadOnly = true;
            TextBoxQRCode.TabStop = false;
            ComboBoxInvoicePriceBy.Visible = enable;
            YesNoRbtSalesType.Enabled = enable;
            YesNoRadioCombineItem.Enabled = enable;
            CheckBoxIsNegativeStockAllow.Enabled = enable;
            CheckBoxPrintQRCode.Enabled = enable;
            if (enable && CheckBoxPrintQRCode.Checked)
            {
                TextBoxQRCode.ReadOnly = !enable;
                TextBoxQRCode.TabStop = !enable;
            }

        }
        private void EnableForm(Boolean enable)
        {

            if (TreeViewCompany.Nodes.Count > 0)
            {
                TreeViewCompany.Enabled = !enable;
                DelayedTextBoxCompanySearch.ReadOnly = enable;
                DelayedTextBoxCompanySearch.TabStop = !enable;
            }
            else
            {
                TreeViewCompany.Enabled = false;
                DelayedTextBoxCompanySearch.ReadOnly = true;
                DelayedTextBoxCompanySearch.TabStop = false;
                BtnCompanyNew.Select();
            }
            //ComboBoxCompanyState.Visible = false;
            ComboBoxCompanyYear.Visible = enable;
            TextBoxCompanyName.ReadOnly = !enable;
            TextBoxCompanyLegalName.ReadOnly = !enable;
            TextBoxCompanySlogan.ReadOnly = !enable;
            AddressGroupBoxCompany.ReadOnly = !enable;
            TextBoxCompanyWebsite.ReadOnly = !enable;
            TextBoxCompanyEmail.ReadOnly = !enable;
            TextBoxCompanyMobile.ReadOnly = !enable;
            TextBoxCompanyPhone.ReadOnly = !enable;
            TextBoxCompanyFax.ReadOnly = !enable;

            TextBoxCompanyName.TabStop = enable;
            TextBoxCompanyLegalName.TabStop = enable;
            AddressGroupBoxCompany.TabStop = enable;
            TextBoxCompanySlogan.TabStop = enable;
            TextBoxCompanyWebsite.TabStop = enable;
            TextBoxCompanyEmail.TabStop = enable;
            TextBoxCompanyPhone.TabStop = enable;
            TextBoxCompanyMobile.TabStop = enable;
            TextBoxCompanyFax.TabStop = enable;
            CheckBoxCompanyMultiCurrency.Enabled = enable;
            ComboBoxCompanyCountry.Visible = enable;
            //ComboBoxCompanyState.Visible = enable;
            if (ComboBoxParentCompany.Items.Count > 0)
            {
                CheckBoxIsBranchOffice.Enabled = enable;
            }

            EnableDefaultAccount(enable);
            EnablePurchaseSalesReference(enable);
            ComboBoxParentCompany.Visible = enable;
            ComboBoxCompanyType.Visible = enable;
            ComboBoxCompanyFirstMonthIncomTaxYear.Visible = enable;
            ComboBoxCompanyFirstMonthFinYear.Visible = enable;
            if (enable && !string.IsNullOrEmpty(TextBoxCompanyId.Text) && CompanyManager.IsCompanyHaveIdSpaceEntries(long.Parse(TextBoxCompanyId.Text)))
            {
                ComboBoxCompanyFirstMonthFinYear.Visible = false;
                ComboBoxCompanyFirstMonthIncomTaxYear.Visible = false;
            }
            ComboBoxCompanyDateFormat.Visible = enable;
            ComboBoxCompanyCurrency.Visible = enable;
            ComboBoxCompanyQtyPricision.Visible = enable;
            ComboBoxCompanyAccountMethod.Visible = enable;
            BtnCompanyLogo.Enabled = enable;
            BtnCompanyDeleteLogo.Enabled = enable;
            CompanyDataGridViewTaxType.Enabled = enable;
            CheckBoxHasProductCatalog.Enabled = enable;
            CheckBoxMaintainRackNumber.Enabled = enable ? CheckBoxHasProductCatalog.Checked : enable;
            ComboBoxBusinessType.Visible = enable;
            if (enable && !string.IsNullOrEmpty(TextBoxCompanyId.Text) && CompanyManager.IsCompanyHaveInventory(long.Parse(TextBoxCompanyId.Text)))
            {
                ComboBoxBusinessType.Visible = false;
            }
            //licence
            CompanyTaxInfoGrid.Enabled = enable;
            CompanyTaxInfoGrid.AllowUserToAddRows = enable;
            CustomerTaxInfoGrid.Enabled = enable;
            CustomerTaxInfoGrid.AllowUserToAddRows = enable;
            SupplierTaxInfoGrid.Enabled = enable;
            SupplierTaxInfoGrid.AllowUserToAddRows = enable;

            CheckBoxIsReceivePayment.Enabled = enable;
            if (enable)
            {
                CheckBoxIsDelivery.Enabled = CheckBoxIsReceivePayment.Checked;
            }
            else
            {
                CheckBoxIsDelivery.Enabled = enable;
            }

            if (CheckBoxIsBranchOffice.Checked && enable)
                ComboBoxParentCompany.Visible = true;
            else
                ComboBoxParentCompany.Visible = false;

            if (!enable)
            {
                BtnCompanyCancel.Enabled = enable;
                if (TreeViewCompany.SelectedNode == null)
                {
                    BtnCompanyDelete.Enabled = enable;
                    BtnCompanyEdit.Enabled = enable;
                }
                else
                {
                    BtnCompanyDelete.Enabled = !enable;
                    BtnCompanyEdit.Enabled = !enable;
                }
                BtnCompanyNew.Enabled = Global.User.IsSuperAdmin ? !enable : enable;
                BtnCompanySave.Enabled = enable;
            }
            else
            {
                BtnCompanyCancel.Enabled = enable;
                BtnCompanyDelete.Enabled = !enable;
                BtnCompanyEdit.Enabled = !enable;
                BtnCompanyNew.Enabled = !enable;
                BtnCompanySave.Enabled = enable;
            }
        }
        int Index = -1;

        public int WM_PASTE { get; private set; }

        private void CheckBoxIsBranchOffice_CheckedChanged(object sender, EventArgs e)
        {
            if (ComboBoxParentCompany.SelectedIndex > -1 && !string.IsNullOrEmpty(TextBoxCompanyId.Text) && Index == -1) { Index = ComboBoxParentCompany.SelectedIndex; }

            ComboBoxParentCompany.Visible = CheckBoxIsBranchOffice.Checked;
            if (!CheckBoxIsBranchOffice.Checked) { LabelCompanyParentCompany.Font = new Font("Tahoma", 8, FontStyle.Regular); ComboBoxParentCompany.SelectedIndex = -1; }
            else if (CheckBoxIsBranchOffice.Checked) { LabelCompanyParentCompany.Font = new Font("Tahoma", 8, FontStyle.Bold); ComboBoxParentCompany.SelectedIndex = Index; }
        }
        private void TextBoxCompanySearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            Keypress_Validation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxCompanyName_KeyPress(object sender, KeyPressEventArgs e)
        {
            Keypress_Validation.Keypress_NameChecking(sender, e);

        }
        private void TextBoxCompanyLegalName_KeyPress(object sender, KeyPressEventArgs e)
        {
            Keypress_Validation.Keypress_NameChecking(sender, e);
        }

        private void TextBoxCompanyEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            Keypress_Validation.Keypress_EmailChecking(sender, e);
        }
        private void TextBoxCompanyWebsite_KeyPress(object sender, KeyPressEventArgs e)
        {
            Keypress_Validation.Keypress_WebsiteNameChecking(sender, e);
        }

        private void TextBoxCompanySearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                TreeViewCompany.Select();
            }
        }
        private void TextBoxCompanyName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab && TextBoxCompanyName.TabStop)
            {
                e.IsInputKey = true;
                TabControlCompany.SelectedTab = TabCompanyInfo;
                TextBoxCompanyLegalName.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab && TextBoxCompanyName.TabStop)
            {
                e.IsInputKey = true;
                BtnCompanySave.Select();
            }
        }
        private void TextBoxCompanySlogan_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab && e.KeyCode == Keys.Tab && TextBoxCompanySlogan.TabStop)
            {
                e.IsInputKey = true;
                TabControlCompany.SelectedTab = TabCompanyInfo;
                TextBoxCompanyLegalName.Select();
            }
            if (e.Modifiers != Keys.Shift && TextBoxCompanySlogan.TabStop)
            {
                e.IsInputKey = false;
            }
        }
        private void BtnCompanySave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlCompany.SelectedTab = TabCompanyInfo;
                TextBoxCompanyName.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewCompanyYear.Rows.Count > 0)
                {
                    TabControlCompany.SelectedTab = TabReference;
                    GridViewCompanyYear.CurrentCell = GridViewCompanyYear[4, GridViewCompanyYear.Rows.Count - 1];
                }
                else
                {
                    TabControlCompany.SelectedTab = TabSales;
                    if (GridViewMiscellaneousTransSales.Enabled)
                    {
                        GridViewMiscellaneousTransSales.Focus();
                    }
                    else
                    {
                        CheckBoxDisplayDeclaration.Select();
                    }
                }
            }
        }

        private void AddressGroupBoxCompany_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab && AddressGroupBoxCompany.TabStop)
            {
                e.IsInputKey = true;
                TabControlCompany.SelectedTab = TabCompanyInfo;
                BtnCompanyLogo.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab && AddressGroupBoxCompany.TabStop)
            {
                e.IsInputKey = true;
                TabControlCompany.SelectedTab = TabCompanyInfo;
                if (ComboBoxCompanyCountry.Visible) { ComboBoxCompanyCountry.Select(); }
                else if (CheckBoxIsBranchOffice.Checked) { ComboBoxParentCompany.Select(); }
                else if (CheckBoxIsBranchOffice.Enabled) { CheckBoxIsBranchOffice.Select(); }
                else if (TextBoxCompanySlogan.Enabled) { TextBoxCompanySlogan.Select(); }
                else { TextBoxCompanyLegalName.Select(); }
            }
        }

        private void TextBoxCompanyWebsite_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && TextBoxCompanyWebsite.TabStop)
            {
                e.IsInputKey = true;
                TabControlCompany.SelectedTab = TabSettings;
                ComboBoxCompanyType.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab && TextBoxCompanyWebsite.TabStop)
            {
                TabControlCompany.SelectedTab = TabCompanyInfo;
                TextBoxCompanyEmail.Select();
            }
        }

        private void ComboBoxCompanyType_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlCompany.SelectedTab = TabSettings;
                if (ComboBoxCompanyFirstMonthFinYear.Visible)
                {
                    ComboBoxCompanyFirstMonthFinYear.Select();
                }
                else
                {
                    ComboBoxCompanyAccountMethod.Select();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TabControlCompany.SelectedTab = TabCompanyInfo;
                TextBoxCompanyWebsite.Select();

            }
        }
        private void ComboBoxBusinessType_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (ComboBoxCompanyUndepositedFundAccount.TextBox.ReadOnly)
                {
                    TabControlCompany.SelectedTab = TabLicenseInfo;
                    CompanyTaxInfoGrid.CurrentCell = CompanyTaxInfoGrid[0, 0];
                    CompanyTaxInfoGrid.BeginEdit(true);
                }
                else
                {
                    TabControlCompany.SelectedTab = TabAccountPreference;
                    ComboBoxCompanyUndepositedFundAccount.TextBox.Select();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlCompany.SelectedTab = TabSettings;
                CheckBoxMaintainRackNumber.Select();
            }
        }

        private void UndepositedFundAccountCombo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlCompany.SelectedTab = TabAccountPreference;
                ComboBoxCompanyCashOnHandAccount.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (ComboBoxBusinessType.TextBox.Visible)
                {
                    TabControlCompany.SelectedTab = TabSettings;
                    ComboBoxBusinessType.Select();
                }
                else
                {
                    TabControlCompany.SelectedTab = TabSettings;
                    CheckBoxMaintainRackNumber.Select();
                }
            }
        }

        private void CompanyDataGridViewTaxType_Leave(object sender, EventArgs e)
        {

        }
        private void ComboBoxInvoicePriceBy_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                TabControlCompany.SelectedTab = TabSales;
                CheckBoxIncludeTax.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewMiscellaneousTransPurchase.Enabled)
                {
                    TabControlCompany.SelectedTab = TabPurchase;
                    GridViewMiscellaneousTransPurchase.Focus();
                }
                else
                {
                    TabControlCompany.SelectedTab = TabTaxInfo;
                    SupplierTaxInfoGrid.CurrentCell = SupplierTaxInfoGrid[0, 0];
                    SupplierTaxInfoGrid.BeginEdit(true);
                }
            }
        }
        private void GridViewSalesTaxReceivable_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                if (GridViewSalesTaxReceivable.Rows.Count - 1 == GridViewSalesTaxReceivable.CurrentRow.Index && GridViewSalesTaxReceivable.CurrentCell.ColumnIndex >= 1)
                {
                    TabControlCompany.SelectedTab = TabLicenseInfo;
                    CompanyTaxInfoGrid.CurrentCell = CompanyTaxInfoGrid[0, 0];
                    CompanyTaxInfoGrid.BeginEdit(true);
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewSalesTaxReceivable.CurrentCell == GridViewSalesTaxReceivable[0, 0])
                {
                    TabControlCompany.SelectedTab = TabAccountPreference;
                    ComboBoxCompanySalesTaxPayableAccount.Select();
                }

            }
        }

        private void GridViewMiscellaneousTransPurchase_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                TabControlCompany.SelectedTab = TabSales;
                ComboBoxInvoicePriceBy.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TabControlCompany.SelectedTab = TabTaxInfo;
                CustomerTaxInfoGrid.CurrentCell = CustomerTaxInfoGrid[0, 0];
                CustomerTaxInfoGrid.BeginEdit(true);
            }
        }
        private void GridViewMiscellaneousTransSales_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlCompany.SelectedTab = TabReference;
                ComboBoxCompanyYear.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TabControlCompany.SelectedTab = TabSales;
                TextBoxCompanyDeclaration.Select();
            }
        }

        private void ComboBoxCompanyYear_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlCompany.SelectedTab = TabReference;
                GridViewCompanyYear.Focus();
                GridViewCompanyYear.CurrentCell = GridViewCompanyYear[1, 0];
                GridViewCompanyYear.BeginEdit(true);
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlCompany.SelectedTab = TabSales;
                if (GridViewMiscellaneousTransSales.Enabled)
                {
                    GridViewMiscellaneousTransSales.Focus();
                }
                else
                {
                    CheckBoxDisplayDeclaration.Select();
                }
            }
        }
        private void CheckBoxDisplayDeclaration_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab && e.KeyCode == Keys.Tab && CheckBoxDisplayDeclaration.TabStop)
            {
                e.IsInputKey = true;
                TabControlCompany.SelectedTab = TabSales;
                TextBoxCompanyBankDetails.Select();
            }
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxCompanyDeclaration.Select();
            }
        }

        private void ComboBoxCompanyCountry_TextChanged(object sender, EventArgs e)
        {
            if (ComboBoxCompanyCountry.SelectedIndex > -1)
            {
                Country lCountry = (Country)ComboBoxCompanyCountry.Items[ComboBoxCompanyCountry.SelectedIndex];
                Country CountryFromDB = CountryManager.GetCountryInfoById(lCountry.Id);
                if (CountryFromDB != null)
                {
                    if (CountryFromDB.DefaultDateFormat != null)
                    {
                        ComboBoxCompanyDateFormat.SelectedIndex = ComboBoxCompanyDateFormat.FindStringExact(CountryFromDB.DefaultDateFormat);
                    }
                    Currency Currency = CurrencyManager.GetCurrencyById((long)CountryFromDB.DefaultCurrencyId!);
                    if (Currency != null)
                    {
                        ComboBoxCompanyCurrency.SelectedIndex = ComboBoxCompanyCurrency.FindStringExact(Currency.Name);
                    }
                    GridViewSalesTaxReceivable.AllowUserToAddRows = false;
                    GridViewSalesTaxReceivable.Rows.Clear();
                    List<CountrySaleTax> lCountryTax = CountryManager.Instance.ListCountryTaxByCountryId(CountryFromDB.Id);
                    if (lCountryTax != null && lCountryTax.Count > 0)
                    {
                        int i = 0;
                        foreach (CountrySaleTax countryTax in lCountryTax)
                        {
                            GridViewSalesTaxReceivable.Rows.Add();
                            GridViewSalesTaxReceivable.Rows[i].Cells[(int)CompanySaleTaxReceivableTableColumn.NAME].Value = countryTax.Name;
                            GridViewSalesTaxReceivable.Rows[i].Cells[(int)CompanySaleTaxReceivableTableColumn.TAXID].Value = countryTax.Id;
                            i++;
                        }
                        GridViewSalesTaxReceivable.Columns[0].ReadOnly = true;
                    }
                    if (CountryFromDB.CompanyType != null)
                    {
                        ComboBoxCompanyType.SelectedIndex = ComboBoxCompanyType.FindStringExact(CountryFromDB.CompanyType.Name);
                    }
                    if (!CountryFromDB.AccountingStartDate.Equals(null))
                    {
                        ComboBoxCompanyFirstMonthFinYear.SelectedIndex = CountryFromDB.AccountingStartDate - 1;
                    }
                    if (!CountryFromDB.IncomeTaxStartDate.Equals(null))
                    {
                        ComboBoxCompanyFirstMonthIncomTaxYear.SelectedIndex = CountryFromDB.IncomeTaxStartDate - 1;
                    }
                    if (CountryFromDB.AccountingMethod != null)
                    {
                        ComboBoxCompanyAccountMethod.SelectedIndex = ComboBoxCompanyAccountMethod.FindStringExact(CountryFromDB.AccountingMethod.Name);
                    }
                }
            }
            else
            {
                CompanyDataGridViewTaxType.Rows.Clear();
            }
        }
        private void ComboBoxCompanyDateFormat_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxCompanyDateFormat.DroppedDown = false;
        }
        private void ComboBoxCompanyCurrency_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxCompanyCurrency.DroppedDown = false;
        }

        private void ComboBoxCompanyAccountMethod_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxCompanyAccountMethod.DroppedDown = false;
        }

        private void ComboBoxCompanyFirstMonthIncomTaxYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxCompanyFirstMonthIncomTaxYear.DroppedDown = false;
        }

        private void ComboBoxCompanyFirstMonthFinYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxCompanyFirstMonthFinYear.DroppedDown = false;
        }

        private void ComboBoxCompanyType_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxCompanyType.DroppedDown = false;
        }

        private void ComboBoxCompanyCountry_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxCompanyCountry.DroppedDown = false;
        }


        private void ComboBoxParentCompany_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxParentCompany.DroppedDown = false;
        }

        private void BtnCompanyLogo_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.AddExtension = true;
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Image Files(*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg";

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CompanyPictureBoxLogo.Image = new Bitmap(openFileDialog.FileName);
            }

        }
        private void CompanyDataGridViewTaxType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (CompanyDataGridViewTaxType.Rows.Count > 0)
            {
                if (CompanyDataGridViewTaxType.CurrentCell.ColumnIndex == 1)
                {
                    Keypress_Validation.Keypress_TaxDetailsNumberChecking(sender, e);
                }
            }
        }
        private void CompanyDataGridViewTaxType_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                Keypress_Validation.Keypress_PasteChecking(sender, e, "TaxDetailsNumberChecking");
            }
        }
        DataGridViewEditingControlShowingEventArgs BackupContextMenuStrip = null!;
        private void CompanyDataGridViewTaxType_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (BackupContextMenuStrip == null)
            {
                BackupContextMenuStrip = e;
            }
            if (CompanyDataGridViewTaxType.CurrentCell.ColumnIndex == 1)
            {
                Keypress_Validation.AddContextMenuGridCell(e, CompanyDataGridViewTaxType, "TaxDetailsNumberChecking", CompanyDataGridViewTaxType.CurrentCell.ColumnIndex);

                e.Control.KeyPress += new KeyPressEventHandler(CompanyDataGridViewTaxType_KeyPress!);

                if (e.Control is DataGridViewTextBoxEditingControl)
                {
                    DataGridViewTextBoxEditingControl? tb = e.Control as DataGridViewTextBoxEditingControl;

                    tb!.KeyDown += CompanyDataGridViewTaxType_KeyDown!;
                }
            }
            else
            {
                e.Control.ContextMenuStrip = BackupContextMenuStrip.Control.ContextMenuStrip;
            }
        }
        private void CompanyDataGridViewTaxType_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void TextBoxCompanyName_KeyDown(object sender, KeyEventArgs e)
        {
            Keypress_Validation.Keypress_PasteChecking(sender, e, "NameChecking");
        }
        private void TextBoxCompanyName_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Keypress_Validation.AddContextMenu(TextBoxCompanyName, "NameChecking");
            }
        }
        private void TextBoxCompanyLegalName_KeyDown(object sender, KeyEventArgs e)
        {
            Keypress_Validation.Keypress_PasteChecking(sender, e, "NameChecking");
        }
        private void TextBoxCompanyLegalName_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Keypress_Validation.AddContextMenu(TextBoxCompanyLegalName, "NameChecking");
            }
        }
        private void TextBoxCompanyStreet_KeyDown(object sender, KeyEventArgs e)
        {
            Keypress_Validation.Keypress_PasteChecking(sender, e, "NameChecking");
        }
        private void TextBoxCompanyEmail_KeyDown(object sender, KeyEventArgs e)
        {
            Keypress_Validation.Keypress_PasteChecking(sender, e, "EmailChecking");
        }
        private void TextBoxCompanyEmail_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Keypress_Validation.AddContextMenu(TextBoxCompanyEmail, "EmailChecking");
            }
        }
        private void TextBoxCompanyWebsite_KeyDown(object sender, KeyEventArgs e)
        {
            Keypress_Validation.Keypress_WebPasteChecking(sender, e, "WebsiteNameChecking");
        }
        private void TextBoxCompanyWebsite_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Keypress_Validation.AddContextMenu(TextBoxCompanyWebsite, "WebsiteNameChecking");
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F3 && BtnCompanyNew.Enabled)
            {
                BtnCompanyNew.PerformClick();
            }
            if (keyData == (Keys.F4))
            {
                BtnCompanyDelete.PerformClick();
            }
            if (keyData == (Keys.F7))
            {
                BtnCompanyEdit.PerformClick();
            }
            if (keyData == (Keys.F8))
            {
                BtnCompanySave.PerformClick();
            }
            if (keyData == (Keys.F10))
            {
                BtnCompanyExit.PerformClick();
            }
            if (keyData == (Keys.Tab) && ActiveControl == BtnCompanySave)
            {
                TabControlCompany.SelectedTab = TabCompanyInfo;
                TextBoxCompanyName.Select();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCompanyCancel.PerformClick();
            }
            try
            {
                if (CompanyTaxInfoGrid.CurrentCell != null && CompanyTaxInfoGrid.CanFocus)
                {
                    if (keyData == (Keys.Tab) && CompanyTaxInfoGrid.CurrentCell.ColumnIndex == (int)CompanyTaxInfoTableColumn.REPORT)
                    {
                        if (CompanyTaxInfoGrid.CurrentRow.Index == CompanyTaxInfoGrid.RowCount - 1)
                        {
                            TabControlCompany.SelectedTab = TabTaxInfo;
                            CustomerTaxInfoGrid.CurrentCell = CustomerTaxInfoGrid[0, 0];
                            CustomerTaxInfoGrid.BeginEdit(true);
                        }
                        else
                        {
                            SendKeys.Send("{tab}");
                        }
                    }

                    if (keyData == (Keys.Tab | Keys.Shift) && CompanyTaxInfoGrid.CurrentCell.ColumnIndex == (int)CompanyTaxInfoTableColumn.NAME)
                    {
                        if (CompanyTaxInfoGrid.CurrentRow.Index != 0)
                        {
                            SendKeys.Send("{tab}");
                        }
                        if (CompanyTaxInfoGrid.CurrentRow.Index == 0)
                        {
                            if (GridViewSalesTaxReceivable.Enabled)
                            {
                                TabControlCompany.SelectedTab = TabAccountPreference;
                                comboBoxCompanyRoundOffAccount.Select();
                            }
                            else
                            {
                                TabControlCompany.SelectedTab = TabSettings;
                                ComboBoxBusinessType.Focus();
                            }
                            CompanyTaxInfoGrid.CurrentCell = null;
                        }
                    }
                }

                if (CustomerTaxInfoGrid.CurrentCell != null && CustomerTaxInfoGrid.CanFocus)
                {
                    if (keyData == (Keys.Tab) && CustomerTaxInfoGrid.CurrentCell.ColumnIndex == (int)CustomerTaxInfoTableColumn.INVOICE)
                    {
                        SendKeys.Send("{tab}");
                    }

                    if (keyData == (Keys.Tab | Keys.Shift) && CustomerTaxInfoGrid.CurrentCell.ColumnIndex == (int)CustomerTaxInfoTableColumn.NAME)
                    {
                        if (CustomerTaxInfoGrid.CurrentRow.Index != 0)
                        {
                            SendKeys.Send("{tab}");
                        }
                        if (CustomerTaxInfoGrid.CurrentRow.Index == 0 && TabControlCompany.SelectedTab == TabTaxInfo)
                        {
                            TabControlCompany.SelectedTab = TabLicenseInfo;
                            CompanyTaxInfoGrid.CurrentCell = CompanyTaxInfoGrid[0, 0];
                            CompanyTaxInfoGrid.BeginEdit(true);
                        }
                    }
                }

                if (SupplierTaxInfoGrid.CurrentCell != null && SupplierTaxInfoGrid.CanFocus)
                {
                    if (keyData == (Keys.Tab) && SupplierTaxInfoGrid.CurrentCell.ColumnIndex == (int)SupplierTaxInfoTableColumn.REPORT)
                    {
                        if (SupplierTaxInfoGrid.CurrentRow.Index == SupplierTaxInfoGrid.RowCount - 1)
                        {
                            if (GridViewMiscellaneousTransPurchase.Enabled)
                            {
                                TabControlCompany.SelectedTab = TabPurchase;
                                GridViewMiscellaneousTransPurchase.Select();
                                SupplierTaxInfoGrid.CurrentCell = null;
                            }
                            else
                            {
                                TabControlCompany.SelectedTab = TabSales;
                                ComboBoxInvoicePriceBy.Select();
                            }
                        }
                        else
                        {
                            SendKeys.Send("{tab}");
                        }

                    }

                    if (keyData == (Keys.Tab | Keys.Shift) && SupplierTaxInfoGrid.CurrentCell!.ColumnIndex == (int)SupplierTaxInfoTableColumn.NAME)
                    {
                        if (SupplierTaxInfoGrid.CurrentRow.Index != 0)
                        {
                            SendKeys.Send("{tab}");
                        }
                        else if (SupplierTaxInfoGrid.CurrentRow.Index == 0)
                        {
                            CustomerTaxInfoGrid.CurrentCell = CustomerTaxInfoGrid[(int)CustomerTaxInfoTableColumn.NAME, 0]; CustomerTaxInfoGrid.BeginEdit(true);
                        }
                    }
                }

                if (GridViewSalesTaxReceivable.CurrentCell != null && GridViewSalesTaxReceivable.Focused)
                {
                    if (keyData == (Keys.Tab) && GridViewSalesTaxReceivable.CurrentCell.ColumnIndex == (int)CompanySaleTaxReceivableTableColumn.ACCOUNT)
                    {
                        if (GridViewSalesTaxReceivable.CurrentCell.RowIndex != GridViewSalesTaxReceivable.Rows.Count - 1)
                        {
                            SendKeys.Send("{tab}");
                        }
                    }

                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewSalesTaxReceivable.CurrentCell.ColumnIndex == (int)CompanySaleTaxReceivableTableColumn.NAME)
                    {
                        if (GridViewSalesTaxReceivable.CurrentRow.Index != 0)
                        {
                            SendKeys.Send("{tab}");
                        }
                    }
                }
                if (GridViewCompanyYear.CurrentCell != null && GridViewCompanyYear.CanFocus)
                {
                    if (keyData == (Keys.Tab) && GridViewCompanyYear.CurrentCell.ColumnIndex == (int)CompanyReferenceTableColumn.ROUNDOFF)
                    {
                        if (GridViewCompanyYear.CurrentRow.Index == GridViewCompanyYear.RowCount - 1)
                        {
                            TabControlCompany.SelectedTab = TabReference;
                            BtnCompanySave.Select();
                        }
                        else
                        {
                            GridViewCompanyYear.CurrentCell = GridViewCompanyYear[(int)CompanyReferenceTableColumn.PREFIX, GridViewCompanyYear.CurrentRow.Index + 1];
                        }
                    }
                    if (keyData == (Keys.Tab) && GridViewCompanyYear.CurrentCell.ColumnIndex == (int)CompanyReferenceTableColumn.DAILYRESET)
                    {
                        if (!(bool)GridViewCompanyYear.CurrentRow.Cells[(int)CompanyReferenceTableColumn.HASPRINTERSETUP].Value && !(bool)GridViewCompanyYear.CurrentRow.Cells[(int)CompanyReferenceTableColumn.HASROUNDOFF].Value)
                        {
                            if (GridViewCompanyYear.CurrentRow.Index == GridViewCompanyYear.RowCount - 1)
                            {
                                TabControlCompany.SelectedTab = TabReference;
                                BtnCompanySave.Select();
                            }
                            else
                            {
                                GridViewCompanyYear.CurrentCell = GridViewCompanyYear[(int)CompanyReferenceTableColumn.TYPE, GridViewCompanyYear.CurrentRow.Index + 1];
                            }
                        }
                        else if (!(bool)GridViewCompanyYear.CurrentRow.Cells[(int)CompanyReferenceTableColumn.HASPRINTERSETUP].Value)
                        {
                            GridViewCompanyYear.CurrentCell = GridViewCompanyYear[(int)CompanyReferenceTableColumn.DOTMATRIX, GridViewCompanyYear.CurrentRow.Index];
                        }
                    }
                    if (keyData == (Keys.Tab) && GridViewCompanyYear.CurrentCell.ColumnIndex == (int)CompanyReferenceTableColumn.PRINTTYPE)
                    {
                        if (!(bool)GridViewCompanyYear.CurrentRow.Cells[(int)CompanyReferenceTableColumn.HASDOTMATRIX].Value && !(bool)GridViewCompanyYear.CurrentRow.Cells[(int)CompanyReferenceTableColumn.HASROUNDOFF].Value)
                        {
                            if (GridViewCompanyYear.CurrentRow.Index == GridViewCompanyYear.RowCount - 1)
                            {
                                TabControlCompany.SelectedTab = TabReference;
                                BtnCompanySave.Select();
                            }
                            else
                            {
                                GridViewCompanyYear.CurrentCell = GridViewCompanyYear[(int)CompanyReferenceTableColumn.PREFIX, GridViewCompanyYear.CurrentRow.Index + 1];
                            }
                        }
                        else if (!(bool)GridViewCompanyYear.CurrentRow.Cells[(int)CompanyReferenceTableColumn.HASDOTMATRIX].Value)
                        {
                            GridViewCompanyYear.CurrentCell = GridViewCompanyYear[(int)CompanyReferenceTableColumn.ROUNDOFF, GridViewCompanyYear.CurrentRow.Index];
                        }
                    }
                    if (keyData == (Keys.Tab) && GridViewCompanyYear.CurrentCell.ColumnIndex == (int)CompanyReferenceTableColumn.DOTMATRIX)
                    {
                        if (!(bool)GridViewCompanyYear.CurrentRow.Cells[(int)CompanyReferenceTableColumn.HASROUNDOFF].Value)
                        {
                            if (GridViewCompanyYear.CurrentRow.Index == GridViewCompanyYear.RowCount - 1)
                            {
                                TabControlCompany.SelectedTab = TabReference;
                                BtnCompanySave.Select();
                            }
                            else
                            {
                                GridViewCompanyYear.CurrentCell = GridViewCompanyYear[(int)CompanyReferenceTableColumn.PREFIX, GridViewCompanyYear.CurrentRow.Index + 1];
                            }
                        }
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewCompanyYear.CurrentCell.ColumnIndex == (int)CompanyReferenceTableColumn.PREFIX)
                    {
                        int Index = GridViewCompanyYear.CurrentCell.RowIndex;
                        if (GridViewCompanyYear.CurrentCell.RowIndex == 0)
                        {
                            ComboBoxCompanyYear.Focus();
                        }
                        else if (!(bool)GridViewCompanyYear.Rows[Index - 1].Cells[(int)CompanyReferenceTableColumn.HASPRINTERSETUP].Value && !(bool)GridViewCompanyYear.Rows[Index - 1].Cells[(int)CompanyReferenceTableColumn.HASROUNDOFF].Value)
                        {
                            GridViewCompanyYear.CurrentCell = GridViewCompanyYear[(int)CompanyReferenceTableColumn.DAILYRESET, GridViewCompanyYear.CurrentRow.Index - 1];
                        }
                        else if (!(bool)GridViewCompanyYear.Rows[Index - 1].Cells[(int)CompanyReferenceTableColumn.HASDOTMATRIX].Value && !(bool)GridViewCompanyYear.Rows[Index - 1].Cells[(int)CompanyReferenceTableColumn.HASROUNDOFF].Value)
                        {
                            GridViewCompanyYear.CurrentCell = GridViewCompanyYear[(int)CompanyReferenceTableColumn.PRINTTYPE, GridViewCompanyYear.CurrentRow.Index - 1];
                        }
                        else if (!(bool)GridViewCompanyYear.Rows[Index - 1].Cells[(int)CompanyReferenceTableColumn.HASROUNDOFF].Value)
                        {
                            GridViewCompanyYear.CurrentCell = GridViewCompanyYear[(int)CompanyReferenceTableColumn.DOTMATRIX, GridViewCompanyYear.CurrentRow.Index - 1];
                        }
                        else
                        {
                            GridViewCompanyYear.CurrentCell = GridViewCompanyYear[(int)CompanyReferenceTableColumn.ROUNDOFF, GridViewCompanyYear.CurrentRow.Index - 1];
                            SendKeys.Send("{tab-}");
                        }
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewCompanyYear.CurrentCell.ColumnIndex == (int)CompanyReferenceTableColumn.ROUNDOFF)
                    {
                        if (!(bool)GridViewCompanyYear.CurrentRow.Cells[(int)CompanyReferenceTableColumn.HASPRINTERSETUP].Value)
                        {
                            GridViewCompanyYear.CurrentCell = GridViewCompanyYear[(int)CompanyReferenceTableColumn.DAILYRESET, GridViewCompanyYear.CurrentRow.Index];
                        }
                        else if (!(bool)GridViewCompanyYear.CurrentRow.Cells[(int)CompanyReferenceTableColumn.HASDOTMATRIX].Value)
                        {
                            GridViewCompanyYear.CurrentCell = GridViewCompanyYear[(int)CompanyReferenceTableColumn.PRINTTYPE, GridViewCompanyYear.CurrentRow.Index];
                        }
                    }
                }


            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void dataViewVerticalScroll1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxCompanyId.Text.Trim()))
            {
                IList<Account> Account = AccountManager.ListCompanySalesTaxReceivableAccounts(long.Parse(TextBoxCompanyId.Text), Global.IncludedAccountCompanySalesTaxReceivable);
                if (Account != null)
                {
                    (GridViewSalesTaxReceivable.Rows[e.RowIndex].Cells[(int)CompanySaleTaxReceivableTableColumn.ACCOUNT] as DataGridViewComboBoxCell)!.DataSource = null;
                    (GridViewSalesTaxReceivable.Rows[e.RowIndex].Cells[(int)CompanySaleTaxReceivableTableColumn.ACCOUNT] as DataGridViewComboBoxCell)!.DataSource = Account;
                    (GridViewSalesTaxReceivable.Rows[e.RowIndex].Cells[(int)CompanySaleTaxReceivableTableColumn.ACCOUNT] as DataGridViewComboBoxCell)!.ValueMember = "Id";
                    (GridViewSalesTaxReceivable.Rows[e.RowIndex].Cells[(int)CompanySaleTaxReceivableTableColumn.ACCOUNT] as DataGridViewComboBoxCell)!.DisplayMember = "Name";

                }
            }
        }

        private void GridViewSalesTaxReceivable_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                if (GridViewSalesTaxReceivable.CurrentCell.Value == null)
                { ((ComboBox)e.Control).SelectedIndex = -1; }
                e.Control.KeyPress += new KeyPressEventHandler(GridViewSalesTaxReceivable_KeyPress!);
            }
        }
        private void GridViewSalesTaxReceivable_KeyPress(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)GridViewSalesTaxReceivable.EditingControl).DroppedDown = false;
        }

        private void GridViewSalesTaxReceivable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                toolStripStatusLabelError.Text = "";
                if (e.ColumnIndex == 2 && (GridViewSalesTaxReceivable.Rows.Count - 1) != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show(string.Format(GridViewSalesTax_ConfirmRowDeleteText, (e.RowIndex + 1).ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        if (GridViewSalesTaxReceivable.Rows[e.RowIndex].Cells[3].Value != null)
                        {
                            if (CatalogSalesTaxMapManager.Instance.CatalogSalesTaxMapCompanySalesTaxMap((long)GridViewSalesTaxReceivable.Rows[e.RowIndex].Cells[3].Value))
                            {
                                toolStripStatusLabelError.Text = GridViewSalesTax_RowDeleteErrorMsg;
                                return;
                            }
                        }
                        GridViewSalesTaxReceivable.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewSalesTaxReceivable.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
        }

        private void GridViewSalesTaxReceivable_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewSalesTaxReceivable.Rows[e.RowIndex].Cells[(int)CompanySaleTaxReceivableTableColumn.ACCOUNT].ReadOnly = true;
        }
        private void BtnCompanyDeleteLogo_Click(object sender, EventArgs e)
        {
            CompanyPictureBoxLogo.Image = null;
        }
        private void FormCompany_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TabControlCompany.SelectedTab = TabCompanyInfo;
                    TextBoxCompanyName.Select();
                    e.Cancel = true;
                }
            }
        }
        private void GridViewSalesTaxReceivable_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void CompanyTaxInfoGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            CellEnter(CompanyTaxInfoGrid, e);
        }
        private void CustomerTaxInfoGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            CellEnter(CustomerTaxInfoGrid, e);
            if (CustomerTaxInfoGrid.Rows[e.RowIndex].Cells[(int)CustomerTaxInfoTableColumn.ID].Value != null)
            {
                if (CompanyManager.CustomerLicenseInfo((long)CustomerTaxInfoGrid.Rows[e.RowIndex].Cells[(int)CustomerTaxInfoTableColumn.ID].Value) != null)
                {
                    CustomerTaxInfoGrid.Rows[e.RowIndex].Cells[(int)CustomerTaxInfoTableColumn.NAME].ReadOnly = true;
                    CustomerTaxInfoGrid.Rows[e.RowIndex].Cells[(int)CustomerTaxInfoTableColumn.REMOVE].ReadOnly = true;
                }
            }
        }
        private void SupplierTaxInfoGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            CellEnter(SupplierTaxInfoGrid, e);
            if (SupplierTaxInfoGrid.Rows[e.RowIndex].Cells[(int)SupplierTaxInfoTableColumn.ID].Value != null)
            {
                if (CompanyManager.SupplierLicenseInfo((long)SupplierTaxInfoGrid.Rows[e.RowIndex].Cells[(int)SupplierTaxInfoTableColumn.ID].Value) != null)
                {
                    SupplierTaxInfoGrid.Rows[e.RowIndex].Cells[(int)SupplierTaxInfoTableColumn.NAME].ReadOnly = true;
                    SupplierTaxInfoGrid.Rows[e.RowIndex].Cells[(int)SupplierTaxInfoTableColumn.REMOVE].ReadOnly = true;
                }
            }
        }
        private void CellEnter(DataViewVerticalScroll GridView, DataGridViewCellEventArgs e)
        {
            GridView.Rows[e.RowIndex].Cells[1].ReadOnly = true;
            GridView.Rows[e.RowIndex].Cells[2].ReadOnly = true;
            GridView.Rows[e.RowIndex].Cells[3].ReadOnly = true;
            if (GridView.Name != "SupplierTaxInfoGrid")
            {
                GridView.Rows[e.RowIndex].Cells[4].ReadOnly = true;
            }
            if (GridView.Rows[e.RowIndex].Cells[0].Value != null)
            {
                GridView.Rows[e.RowIndex].Cells[1].ReadOnly = false;
                GridView.Rows[e.RowIndex].Cells[2].ReadOnly = false;
                GridView.Rows[e.RowIndex].Cells[3].ReadOnly = false;
                if (GridView.Name != "SupplierTaxInfoGrid")
                {
                    GridView.Rows[e.RowIndex].Cells[4].ReadOnly = false;
                }
            }
        }
        private void CompanyTaxInfoGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CellClick(CompanyTaxInfoGrid, e);
        }
        private void CustomerTaxInfoGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CellClick(CustomerTaxInfoGrid, e);
        }
        private void SupplierTaxInfoGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CellClick(SupplierTaxInfoGrid, e);
        }
        private void CellClick(DataViewVerticalScroll GridView, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (GridView.Name == "SupplierTaxInfoGrid")
                {
                    if (e.ColumnIndex == (int)SupplierTaxInfoTableColumn.REMOVE && !GridView.Rows[e.RowIndex].IsNewRow)
                    {
                        DialogResult Result = MessageBox.Show("Do you want to delete row " + (e.RowIndex + 1).ToString() /*GridView.Rows[e.RowIndex].Cells[(int)SupplierTaxInfoTableColumn.NAME].Value*/ + "?", "Delete Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (Result == DialogResult.Yes)
                        {
                            GridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            GridView.Rows.RemoveAt(e.RowIndex);
                        }
                    }
                }
                else
                {
                    if (e.ColumnIndex == (int)CompanyTaxInfoTableColumn.REMOVE && !GridView.Rows[e.RowIndex].IsNewRow)
                    {
                        DialogResult Result = MessageBox.Show("Do you want to delete row " + (e.RowIndex + 1).ToString() + "?", "Delete Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (Result == DialogResult.Yes)
                        {
                            GridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            GridView.Rows.RemoveAt(e.RowIndex);
                        }
                    }
                }
            }
        }
        private void CompanyTaxInfoGrid_Leave(object sender, EventArgs e)
        {
            CompanyTaxInfoGrid.BeginInvoke(new MethodInvoker(delegate ()
            {
                if (CompanyTaxInfoGrid.Rows.Count > 0)
                {
                    CompanyTaxInfoGrid.CurrentCell = CompanyTaxInfoGrid[5, 0];
                }

            }));
        }
        private void CustomerTaxInfoGrid_Leave(object sender, EventArgs e)
        {
            CustomerTaxInfoGrid.CurrentCell = null;
        }
        private void SupplierTaxInfoGrid_Leave(object sender, EventArgs e)
        {
            SupplierTaxInfoGrid.CurrentCell = null;
        }
        private void CheckBoxIsReceivePayment_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxIsReceivePayment.Checked)
            {
                CheckBoxIsDelivery.Enabled = true;
            }
            else
            {
                CheckBoxIsDelivery.Checked = false;
                CheckBoxIsDelivery.Enabled = false;
            }
        }
        private void ComboBoxBusinessType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Global.softwareType == SoftwareType.MEDICARE)
            {
                ComboBoxInvoicePriceBy.SelectedIndex = (int)PriceType.Retail;
            }
            else
            {
                if (ComboBoxBusinessType.SelectedIndex == (int)BuisnessType.Wholesale)
                {
                    ComboBoxInvoicePriceBy.SelectedIndex = (int)PriceType.Wholesale;
                }
                else
                {
                    ComboBoxInvoicePriceBy.SelectedIndex = (int)PriceType.Retail;
                }
            }
            if (!string.IsNullOrEmpty(TextBoxCompanyId.Text))
            {
                GridViewCompanyYear.Rows.Clear();
                if (ComboBoxCompanyYear.SelectedIndex > -1)
                {
                    loadIdSpace();
                }
            }
        }
        private void ComboBoxCompanyQtyPricision_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Instance.Keypress_Num(sender, e);
        }
        private void DelayedTextBoxCompanySearch_TextChanged(object sender, EventArgs e)
        {
            ResetForm();
            LoadCompaniesWithFilter();
            if (TreeViewCompany.Nodes.Count > 0) { BtnCompanyEdit.Enabled = true; BtnCompanyDelete.Enabled = true; }
            else { BtnCompanyEdit.Enabled = false; BtnCompanyDelete.Enabled = false; }
            this.formIsDirty = false;
        }
        private void DelayedTextBoxCompanySearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                TreeViewCompany.Select();
            }
        }
        private void ComboBoxCompanyYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewCompanyYear.Rows.Clear();
            if (ComboBoxCompanyYear.SelectedIndex > -1)
            {
                loadIdSpace();
            }
            this.formIsDirty = false;
        }
        private void loadIdSpace()
        {
            Company Company = CompanyManager.GetCompanyForModel(long.Parse(TextBoxCompanyId.Text));
            String Year = (string)ComboBoxCompanyYear.Items[ComboBoxCompanyYear.SelectedIndex];
            string[] Years = Year.Split('-');
            string Start = new DateTime(int.Parse(Years[0]), Company.AccountingStartDate, 1).ToString(Global.Company.DateFormat);
            string End = new DateTime(int.Parse(Years[0]), Company.AccountingStartDate, 1).AddYears(1).AddDays(-1).ToString(Global.Company.DateFormat);

            BuisnessType Type = GetBuisnessType();
            List<IdSpace> IdSpaces = CompanyManager.Instance.ListIdSpace(Type, long.Parse(TextBoxCompanyId.Text), DateTime.ParseExact(Start, Global.Company.DateFormat, null), DateTime.ParseExact(End, Global.Company.DateFormat, null));

            List<IdSpaceEntryTypeDetail> LIdSpaceEntryTypeDetail = CompanyManager.Instance.GetIdSpaceEntryTypeDetails();
            var EntryTypes = new List<EntryType> { EntryType.PATIENT_ID, EntryType.OP_ID, EntryType.IP_ID, EntryType.PRESCRIPTION };

            if (Global.softwareType == SoftwareType.MEDICARE)
            {
                foreach (var entryType in EntryTypes)
                {
                    if (IdSpaces.Count < LIdSpaceEntryTypeDetail.Count - 1)
                    {
                        if (!IdSpaces.Any(x => x.EntryType == entryType))
                        {
                            IdSpace idSpace = new IdSpace
                            {
                                EntryType = entryType,
                                HasDotMatrix = false,
                                HasPrinterSetup = entryType == EntryType.PRESCRIPTION ? true : false,
                                HasRoundOff = false,
                                IsResetDaily = false,
                                IsDotMatrix = false,
                                RoundOff = 0,
                                Seed = 1,
                                Date = fa.Data.Global.TransactionDate,
                                Prefix = string.Empty,
                                CompanyId = Global.Company.CompanyId,
                                RunningSeed = 1,
                                YearStartDate = DateTime.ParseExact(Start, Global.Company.DateFormat, null),
                                YearEndDate = DateTime.ParseExact(End, Global.Company.DateFormat, null),
                                PrintPaperFormat_Id = (entryType == EntryType.PRESCRIPTION || entryType == EntryType.PATIENT_INVOICE) ? 2 : (entryType == EntryType.OP_TOKEN || entryType == EntryType.PATIENT_FEE_RECEIPT) ? 6 : 5
                            };
                            IdSpaces.Add(idSpace);
                        }
                    }
                }
            }

            if (IdSpaces != null && IdSpaces.Count > 0)
            {
                IList<PrintPaperFormat> PrintPaperFormat = PaperFormatManager.Instance.ListPrintPaperFormat();
                DaybookHasEntry = CompanyManager.HasEntry(long.Parse(TextBoxCompanyId.Text), DateTime.ParseExact(Start, Global.Company.DateFormat, null), DateTime.ParseExact(End, Global.Company.DateFormat, null));
                int i = 0;
                foreach (IdSpace IdSpace in IdSpaces.OrderBy(x => x.EntryType))
                {
                    GridViewCompanyYear.Rows.Add();
                    GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.TYPEID].Value = IdSpace.EntryType;
                    GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.TYPE].Value = ComboUtils.GetEntryTypeName(IdSpace.EntryType);
                    GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.PREFIX].Value = IdSpace.Prefix;
                    GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.SEED].Value = IdSpace.Seed;
                    GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.DAILYRESET].Value = IdSpace.IsResetDaily;
                    GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.HASROUNDOFF].Value = IdSpace.HasRoundOff;
                    GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.HASPRINTERSETUP].Value = IdSpace.HasPrinterSetup;
                    GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.HASDOTMATRIX].Value = IdSpace.HasDotMatrix;
                    if (IdSpace.HasPrinterSetup)
                    {
                        (GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE] as DataGridViewComboBoxCell)!.DataSource = null;
                        if (IdSpace.EntryType == EntryType.SALES || IdSpace.EntryType == EntryType.SALES_QUOTE || IdSpace.EntryType == EntryType.SALES_RETURN)
                        {
                            (GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE] as DataGridViewComboBoxCell)!.DataSource = PrintPaperFormat.Where(x => x.Name != "A5 PORTRAIT").ToList();
                        }
                        else if (IdSpace.EntryType == EntryType.PRESCRIPTION)
                        {
                            (GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE] as DataGridViewComboBoxCell)!.DataSource = PrintPaperFormat.Where(x => x.Name == "A4 PORTRAIT" || x.Name == "A5 LANDSCAPE").ToList();
                        }
                        else
                        {
                            (GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE] as DataGridViewComboBoxCell)!.DataSource = PrintPaperFormat.Where(x => x.Name == "A5 LANDSCAPE").ToList();
                        }
                        (GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE] as DataGridViewComboBoxCell)!.ValueMember = "Id";
                        (GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE] as DataGridViewComboBoxCell)!.DisplayMember = "Name";
                        GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE].Value = IdSpace.PrintPaperFormat_Id == null ? ((PrintPaperFormat)(GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE] as DataGridViewComboBoxCell)!.Items[0]).Id : IdSpace.PrintPaperFormat_Id;
                        if (IdSpace.HasDotMatrix)
                        {
                            GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.DOTMATRIX].Value = IdSpace.IsDotMatrix;
                        }
                        else
                        {
                            GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.DOTMATRIX] = new DataGridViewTextBoxCell();
                            GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.DOTMATRIX].ReadOnly = true;
                        }
                    }
                    else
                    {
                        GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE] = new DataGridViewTextBoxCell();
                        GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.DOTMATRIX] = new DataGridViewTextBoxCell();
                        GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE].ReadOnly = true;
                        GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.DOTMATRIX].ReadOnly = true;
                    }
                    if (IdSpace.HasRoundOff)
                    {
                        GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.ROUNDOFF].Value = IdSpace.RoundOff;
                    }
                    else
                    {
                        GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.ROUNDOFF] = new DataGridViewTextBoxCell();
                        GridViewCompanyYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.ROUNDOFF].ReadOnly = true;
                    }
                    i++;
                }
            }
        }
        private BuisnessType GetBuisnessType()
        {
            Company Company = CompanyManager.GetCompanyForModel(long.Parse(TextBoxCompanyId.Text));
            BuisnessType BType = Company.BusinessType;
            if (BType != BuisnessType.Hospital)
            {
                if (ComboBoxBusinessType.SelectedIndex > -1)
                {
                    BType = (BuisnessType)ComboBoxBusinessType.SelectedIndex;
                }
            }
            return BType;
        }
        private void GridViewCompanyYear_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)CompanyReferenceTableColumn.TYPE)
            {
                GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.TYPE].ReadOnly = true;
            }
            if (e.ColumnIndex == (int)CompanyReferenceTableColumn.SEED && !string.IsNullOrEmpty(TextBoxCompanyId.Text))
            {
                GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.SEED].ReadOnly = DaybookHasEntry[(int)GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.TYPEID].Value];
                IList<Patient> patients = PatientManager.Instance.GetPatientContainsPrefix(GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.PREFIX].Value != null ? GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.PREFIX].Value.ToString() : "", Global.Company.CompanyId);
                if (patients != null && patients.Count > 0 && GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.TYPE].Value.ToString() == "Patient ID")
                {
                    GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.SEED].ReadOnly = true;
                    return;
                }
                IList<Registration> OPNumber = OpManager.Instance.GetOPNumberContainsPrefix(GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.PREFIX].Value != null ? GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.PREFIX].Value.ToString() : "", Global.Company.CompanyId);
                if (OPNumber != null && OPNumber.Count > 0 && GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.TYPE].Value.ToString() == "Patient OP Number")
                {
                    GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.SEED].ReadOnly = true;
                    return;
                }
                IList<InPatientAdmission> IPNumber = IpManager.Instance.GetIPNumberContainsPrefix(GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.PREFIX].Value != null ? GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.PREFIX].Value.ToString() : "", Global.Company.CompanyId);
                if (IPNumber != null && IPNumber.Count > 0 && GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.TYPE].Value.ToString() == "Patient IP Number")
                {
                    GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.SEED].ReadOnly = true;
                    return;
                }
            }
            if (e.ColumnIndex == (int)CompanyReferenceTableColumn.PRINTTYPE && !(bool)GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.HASPRINTERSETUP].Value)
            {
                GridViewCompanyYear.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
                GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.DOTMATRIX].ReadOnly = true;
            }
            if (e.ColumnIndex == (int)CompanyReferenceTableColumn.DOTMATRIX && !(bool)GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.HASDOTMATRIX].Value)
            {
                GridViewCompanyYear.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
            }
            if (e.ColumnIndex == (int)CompanyReferenceTableColumn.ROUNDOFF && !(bool)GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.HASROUNDOFF].Value)
            {
                GridViewCompanyYear.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
            }
            if (e.ColumnIndex == (int)CompanyReferenceTableColumn.DAILYRESET)
            {
                if ((EntryType)GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.TYPEID].Value == EntryType.PATIENT_ID
                    || (EntryType)GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.TYPEID].Value == EntryType.IP_ID
                    || (EntryType)GridViewCompanyYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.TYPEID].Value == EntryType.OP_ID)
                {
                    GridViewCompanyYear.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
                }
            }
        }
        private void GridViewCompanyYear_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void GridViewCompanyYear_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                if (GridViewCompanyYear.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }
                e.Control.KeyPress += new KeyPressEventHandler(GridViewCompanyYear_KeyPress!);
            }
        }
        private void GridViewCompanyYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)GridViewCompanyYear.EditingControl).DroppedDown = false;
        }

        private void BtnCompanyAddYear_Click(object sender, EventArgs e)
        {
            long CompanyId = long.Parse(TextBoxCompanyId.Text);
            FormFiscalYear FormFiscalYear = new FormFiscalYear();
            FormFiscalYear.CompanyId = CompanyId;
            FormFiscalYear.ShowDialog();
            ComboUtils.InitializeYearCombo(ComboBoxCompanyYear, CompanyManager.Instance.GetCompanyForModel(CompanyId));
            this.formIsDirty = false;
        }

        private void ComboBoxInvoicePriceBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxInvoicePriceBy.SelectedIndex == 2)
            {
                CheckBoxIncludeTax.Checked = true;
                CheckBoxIncludeTax.Enabled = false;
            }
            else
            {
                CheckBoxIncludeTax.Checked = false;
                CheckBoxIncludeTax.Enabled = true;
            }
        }

        private void ComboBoxCompanyState_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxCompanyCountry.DroppedDown = false;
        }

        private void CheckBoxPrintQRCode_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxPrintQRCode.Checked)
            {
                LabelUPIId.Font = new Font("Tahoma", 8, FontStyle.Bold);
                TextBoxQRCode.ReadOnly = false;
                TextBoxQRCode.TabStop = true;
            }
            else
            {
                TextBoxQRCode.ResetText();
                LabelUPIId.Font = new Font("Tahoma", 8, FontStyle.Regular);
                TextBoxQRCode.ReadOnly = true;
                TextBoxQRCode.TabStop = false;
            }
        }

        private void ComboBoxCompanyCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxCompanyCountry.SelectedIndex > -1)
            {
                AddressGroupBoxCompany.CountryId = ((Country)ComboBoxCompanyCountry.Items[ComboBoxCompanyCountry.SelectedIndex]).Id;
            }
        }

        private void CheckBoxMaintainRackNumber_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                if (ComboBoxBusinessType.Visible == false)
                {
                    TabControlCompany.SelectedTab = TabAccountPreference;
                    ComboBoxCompanyUndepositedFundAccount.TextBox.Select();
                }
            }
        }

        private void ComboBoxCompanySalesTaxPayableAccount_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                comboBoxCompanyRoundOffAccount.Select();
            }
        }

        private void CheckBoxHasProductCatalog_CheckedChanged(object sender, EventArgs e)
        {
            CheckBoxMaintainRackNumber.Checked = !CheckBoxHasProductCatalog.Checked ? CheckBoxHasProductCatalog.Checked : CheckBoxMaintainRackNumber.Checked;
            CheckBoxMaintainRackNumber.Enabled = CheckBoxHasProductCatalog.Checked;
        }

        private void comboBoxCompanyRoundOffAccount_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                TabControlCompany.SelectedTab = TabLicenseInfo;
                CompanyTaxInfoGrid.CurrentCell = CompanyTaxInfoGrid[0, 0];
                CompanyTaxInfoGrid.BeginEdit(true);
            }
        }

        private void TextBoxCompanyPhone_Enter(object sender, EventArgs e)
        {
            selectedTextFocus(sender);
        }
        private void TextBoxCompanyMobile_Enter(object sender, EventArgs e)
        {
            selectedTextFocus(sender);
        }
        private void TextBoxCompanyFax_Enter(object sender, EventArgs e)
        {
            MaskedTextBox? maskedTextBox = sender as MaskedTextBox;
            if (maskedTextBox != null)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    maskedTextBox.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                    int inputLength = maskedTextBox.Text.Length;
                    maskedTextBox.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;
                    maskedTextBox.Select(0, inputLength);
                });
            }
        }
        private void selectedTextFocus(object sender)
        {
            MaskedTextBox? maskedTextBox = sender as MaskedTextBox;
            if (maskedTextBox != null)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    maskedTextBox.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                    int inputLength = maskedTextBox.Text.Length > 4 ? maskedTextBox.Text.Length + 1 : maskedTextBox.Text.Length;
                    maskedTextBox.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;
                    maskedTextBox.Select(0, inputLength);
                });
            }
        }

        private void TextBoxCompanyName_Enter(object sender, EventArgs e)
        {
            TextBoxSelectedTextFocus(sender);
        }
        private void TextBoxCompanyLegalName_Enter(object sender, EventArgs e)
        {
            TextBoxSelectedTextFocus(sender);
        }
        private void TextBoxCompanySlogan_Enter(object sender, EventArgs e)
        {
            TextBoxSelectedTextFocus(sender);
        }
        private void TextBoxCompanyEmail_Enter(object sender, EventArgs e)
        {
            TextBoxSelectedTextFocus(sender);
        }
        private void TextBoxCompanyWebsite_Enter(object sender, EventArgs e)
        {
            TextBoxSelectedTextFocus(sender);
        }
        private void TextBoxSelectedTextFocus(object sender)
        {
            TextBox? TextBox = sender as TextBox;
            if (TextBox != null)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    int inputLength = TextBox.Text.Length;
                    TextBox.Select(0, inputLength);
                });
            }
        }
        private void TextBoxCompanyBankDetails_Enter(object sender, EventArgs e)
        {
            TextBoxSelectedTextFocus(sender);
        }
        private void TextBoxCompanyDeclaration_Enter(object sender, EventArgs e)
        {
            TextBoxSelectedTextFocus(sender);
        }
        private void TextBoxQRCode_Enter(object sender, EventArgs e)
        {
            TextBoxSelectedTextFocus(sender);
        }
    }
}
