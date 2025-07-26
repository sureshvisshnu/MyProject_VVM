using ExcelDataReader;
using fa.api.Accounting;
using fa.api.catalog;
using fa.api.utils;
using fa.libraries.utils;
using fa.libraries.Validation;
using fa.model.Accounting.Masters;
using fa.model.Catalog;
using fa.model.Common;
using fa.views.controls;
using fa.views.hms.masters.upload;
using Fa.model.Accounting.Masters;
using Fa.Utils.utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace fa.views.account.masters
{
    public enum SupplierFormTaxInfoTableColumn
    {
        NAME, DNAME, VALUE, INVOICE, REPORT, REMOVE, ID, MASTERID, REQUIR
    }
    public partial class FormSupplier : FormBase
    {
        public static string EnterEmailErrorMsg = "Please enter valid email.";
        public static string EnterWebsiteErrorMsg = "Please enter valid Website.";
        public static string SaveSuccessText = "Saved success...";
        public static string CreateSupplierOnloadText = "New Supplier (Vendor)";
        public static string UpdateSupplierOnloadText = "Update Supplier";
        public static string DeleteConfirmText = "Do you want to delete the Supplier {0}?";
        public static string DeleteErrorText = "Error Deleting the Supplier!, Please retry";
        public static string IsbranchDeleteErrorText = "Cannot delete Supplier with sub Supplier";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string EnterValidAsOfDateErrorMsg = "Please enter valid as of date";
        public static string UniqueSupplierNameErrorMsg = "Supplier {0} already exists in Company {1}";
        public static string EnterSupplierNameErrorMsg = "Please enter Supplier name";
        public static string ChooseParentErrorMsg = "Please choose parent Supplier";
        public static string EnterAsOfDateErrorMsg = "Please enter proper ASof Date";
        public static string ChooseBalanceTypeErrorMsg = "Please choose balance Type";
        public static string TaxInfoGrid_MantatoryFiledErrorMsg = "Please enter {0}.";
        public static string ChooseStateErrorMsg = "Please choose state";
        public bool CreateSupplierOnLoad = false;
        public string? ParentId;
        public string? Id;
        SupplierManager SupplierManager = null!;
        StateManager StateManager = null!;
        CountryManager CountryManager = null!;
        AddressManager AddressManager = null!;
        ContactInfoManager ContactInfoManager = null!;
        TaxinfoManager TaxinfoManager = null!;
        KeypressValidation KeypressValidation = null!;
        DateValidation DateValidation = null!;
        FormBase parent = null!;
        public FormSupplier(object sender)
        {
            if (sender is FormBase)
            {
                parent = (FormBase)sender;
            }
            SupplierManager = SupplierManager.Instance;
            StateManager = StateManager.Instance;
            CountryManager = CountryManager.Instance;
            AddressManager = AddressManager.Instance;
            ContactInfoManager = ContactInfoManager.Instance;
            TaxinfoManager = TaxinfoManager.Instance;
            KeypressValidation = KeypressValidation.Instance;
            DateValidation = DateValidation.Instance;
            InitializeComponent();
            excludedObjects = new string[] { "TextBoxSupplierSearch" };
        }
        private void FormSupplier_Load(object sender, EventArgs e)
        {
            try
            {
                //ImportSypplier();
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                EnableForm(false);
                LoadComboBox();
                LoadSupplierWithFilter();
                LoadAllProductsToTreeView();
                if (TreeViewSupplier.Nodes.Count == 0)
                {
                    BtnSupplierNew.Focus();
                }
                else
                {
                    TextBoxSupplierSearch.Focus();
                }
                if (CreateSupplierOnLoad)
                {
                    this.BtnSupplierEdit.Visible = false;
                    BtnSupplierCancel.Visible = false;
                    BtnSupplierNew.Visible = false;
                    BtnSupplierDelete.Visible = false;
                    TreeViewSupplier.Visible = false;
                    TabControlSupplier.Location = new Point(12, 12);
                    BtnSupplierSave.Location = new Point(TabControlSupplier.Width - 74, TabControlSupplier.Height + 15);
                    this.Size = new Size(TabControlSupplier.Right + 25, TabControlSupplier.Bottom + 90);
                    this.CenterToParent();
                    if (Id == null)
                    {
                        this.Text = CreateSupplierOnloadText;
                        BtnSupplierNew_Click(this, null!);
                    }
                    else
                    {
                        this.Text = UpdateSupplierOnloadText;
                        TreeNode TreeNode = new TreeNode();
                        TreeNode = ParentId == "" ? TreeViewSupplier.Nodes[Id] : TreeViewSupplier.Nodes[ParentId].Nodes[Id];
                        TreeViewSupplier.SelectedNode = TreeNode;
                        BtnSupplierEdit_Click(this, null!);
                    }
                }
                this.formIsDirty = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        public static string GetReadFileName(string FileNames, string Title, string ProposedFileName)
        {
            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
            OpenFileDialog1.Filter = FileNames;
            OpenFileDialog1.Title = Title;
            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(OpenFileDialog1.FileName))
                {
                    return OpenFileDialog1.FileName;
                }
            }
            return null!;
        }
        string TextBoxFileName = string.Empty;
        private void ImportSypplier()
        {
            String FileName = GetReadFileName("Excel | *.xlsx", "Select the File to upload", "");
            if (FileName != null)
            {
                TextBoxFileName = FileName;
            }
            using (var stream = File.Open(FileName!, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int rowcount = reader.RowCount;
                    this.UseWaitCursor = true;
                    Cursor.Current = Cursors.WaitCursor;
                    int row = 1;
                    do
                    {
                        while (reader.Read())
                        {
                            string Name = "";
                            try
                            {
                                if (row == 1)
                                {
                                    row++;
                                    continue;
                                }
                                Name = reader.GetValue(0) != null ? reader.GetValue(0).ToString()!.Trim() : string.Empty;
                                string[] Dl = new string[2];
                                Dl[0] = reader.GetString(1);
                                Dl[1] = reader.GetString(2);

                                if (!string.IsNullOrEmpty(Name))
                                {
                                    Supplier Supplier = new Supplier
                                    {
                                        Id = 0L,
                                        CompanyId = Global.Company.CompanyId,
                                        Name = Name,
                                    };

                                    IList<Supplier> llSupplier = SupplierManager.Instance.ListSupplierByCompanyId(Global.Company.CompanyId);
                                    int i = 0;
                                    Supplier.SupplierLicenceDetail = new List<SupplierLicenceDetail>();
                                    foreach (CompanySupplierLicenseMaster li in Global.Company.CompanySupplierLicenseMaster)
                                    {
                                        SupplierLicenceDetail SupplierLicenceDetail = new SupplierLicenceDetail();
                                        SupplierLicenceDetail.CompanySupplierLicenseMasterId = li.Id;
                                        SupplierLicenceDetail.SupplierLicenceId = 0L;
                                        SupplierLicenceDetail.DisplayName = li.DisplayName;
                                        SupplierLicenceDetail.Value = Dl[i];
                                        SupplierLicenceDetail.CompanyId = Global.Company.CompanyId;
                                        Supplier.SupplierLicenceDetail.Add(SupplierLicenceDetail);
                                        i++;
                                    }
                                    SupplierManager.Instance.AddSupplier(Supplier);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ex.HResult.ToString() == "-2146233080")
                                {
                                    Cursor.Current = Cursors.Default;
                                    this.UseWaitCursor = false;
                                    throw new IndexOutOfRangeException("-2146233080", ex);
                                }

                            }
                            row++;

                        }
                    } while (reader.NextResult());

                    this.UseWaitCursor = false;
                    Cursor.Current = Cursors.Default;
                }
            }
        }
        private void LoadComboBox()
        {
            ComboUtils.InitializeSupplierCombo(ComboBoxSupplierParentAccount, Global.Company.CompanyId);
            ResetLicenseInfo();
        }
        private void ResetLicenseInfo()
        {
            int LicenseTypeCount = Global.Company.CompanySupplierLicenseMaster.Count;
            if (LicenseTypeCount > 0)
            {
                int i = 0;
                SupplierLicenceInfoGrid.Rows.Clear();
                SupplierLicenceInfoGrid.AllowUserToAddRows = false;
                SupplierLicenceInfoGrid.Rows.Add(LicenseTypeCount);
                foreach (CompanySupplierLicenseMaster CompanySupplierLicenseMaster in Global.Company.CompanySupplierLicenseMaster)
                {
                    SupplierLicenceInfoGrid.Rows[i].Cells[(int)SupplierFormTaxInfoTableColumn.NAME].Value = CompanySupplierLicenseMaster.Name;
                    SupplierLicenceInfoGrid.Rows[i].Cells[(int)SupplierFormTaxInfoTableColumn.DNAME].Value = CompanySupplierLicenseMaster.DisplayName;
                    SupplierLicenceInfoGrid.Rows[i].Cells[(int)SupplierFormTaxInfoTableColumn.REPORT].Value = CompanySupplierLicenseMaster.IncludeInReport;
                    SupplierLicenceInfoGrid.Rows[i].Cells[(int)SupplierFormTaxInfoTableColumn.INVOICE].Value = CompanySupplierLicenseMaster.IncludeInInvoice;
                    SupplierLicenceInfoGrid.Rows[i].Cells[(int)SupplierFormTaxInfoTableColumn.MASTERID].Value = CompanySupplierLicenseMaster.Id;
                    SupplierLicenceInfoGrid.Rows[i].Cells[(int)SupplierFormTaxInfoTableColumn.REQUIR].Value = CompanySupplierLicenseMaster.Required;
                    i++;
                }
            }
        }
        private void LoadSupplierWithFilter()
        {
            string FilterString = TextBoxSupplierSearch.Text.Trim();
            TreeViewSupplier.Nodes.Clear();
            IList<Supplier> Supplier = SupplierManager.ListSupplierByCompanyId(Global.Company.CompanyId);
            if (Supplier != null)
            {
                foreach (var lSupplier in Supplier)
                {
                    if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim()) || lSupplier.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        if (lSupplier.ParentAccountId == null)
                        {
                            TreeViewSupplier.Nodes.Add(lSupplier.Id.ToString(), lSupplier.Name);
                            TreeViewSupplier.Nodes[lSupplier.Id.ToString()].ImageIndex = 0;
                            foreach (var llSupplier in Supplier)
                            {
                                if (lSupplier.Id == llSupplier.ParentAccountId)
                                {
                                    if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim()) || llSupplier.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                                    {
                                        TreeViewSupplier.Nodes[lSupplier.Id.ToString()].Nodes.Add(llSupplier.Id.ToString(), llSupplier.Name);
                                        TreeViewSupplier.Nodes[lSupplier.Id.ToString()].Nodes[llSupplier.Id.ToString()].ImageIndex = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (TreeViewSupplier.Nodes.Count > 0)
            {
                if (!string.IsNullOrEmpty(TextBoxSupplierSearch.Text))
                {
                    TreeViewSupplier.ExpandAll();
                }
                TreeViewSupplier.SelectedNode = TreeViewSupplier.Nodes[0];
            }
        }
        private Supplier GetSupplierInfo()
        {
            if (TreeViewSupplier.SelectedNode != null)
            {
                Supplier lSupplier = SupplierManager.GetSupplierById(long.Parse(TreeViewSupplier.SelectedNode.Name));
                if (lSupplier != null)
                {
                    Supplier SupplierFromDB = SupplierManager.GetSupplierById(lSupplier.Id);
                    return SupplierFromDB;
                }
            }
            return null!;
        }
        private void LoadSupplierInfo()
        {
            Supplier SupplierFromDB = GetSupplierInfo();
            if (SupplierFromDB != null)
            {
                TextBoxSupplierAccountId.Text = SupplierFromDB.Id.ToString();
                TextBoxSupplierName.Text = SupplierFromDB.Name;
                TextBoxSupplierDisplayAs.Text = SupplierFromDB.DisplayAs;
                TextBoxSupplierDescription.Text = SupplierFromDB.Discription;
                CheckBoxSupplierIsbranch.Checked = SupplierFromDB.IsSubAccount;
                DateTimePickerSupplier.Date = (DateTime)DateUtils.ToDate(SupplierFromDB.BalanceAsOf.Date.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                TextBoxSupplierBalance.Text = Math.Abs(SupplierFromDB.Balance).ToString(TextUtils.DecimalPlace(TextBoxSupplierBalance.Decimals));
                ComboBoxBalanceType.SelectedIndex = SupplierFromDB.Balance < 0 ? 1 : 0;
                if (SupplierFromDB.AddressId != null)
                {
                    Address BillingAddress = AddressManager.GetAddressById((long)SupplierFromDB.AddressId);
                    if (BillingAddress != null)
                    {
                        AddressGroupBoxSupplier.AddressLine1 = BillingAddress.AddressLine1;
                        AddressGroupBoxSupplier.AddressLine2 = BillingAddress.AddressLine2;
                        AddressGroupBoxSupplier.CityName = BillingAddress.CityOrTown;
                        AddressGroupBoxSupplier.DistrictName = BillingAddress.District;
                        AddressGroupBoxSupplier.PinCode = BillingAddress.PinCode;
                        AddressGroupBoxSupplier.StateId = BillingAddress.StatesId != null ? (long)BillingAddress.StatesId : 0L;

                    }
                }
                if (SupplierFromDB.ContactInfo != null)
                {
                    ContactInfo lContactInfo = SupplierFromDB.ContactInfo;
                    TextBoxSupplierPhone.Text = lContactInfo.Phone;
                    TextBoxSupplierMobile.Text = lContactInfo.Mobile;
                    TextBoxSupplierFax.Text = lContactInfo.Fax;
                    TextBoxSupplierEmail.Text = lContactInfo.Email;
                    TextBoxSupplierWebsite.Text = lContactInfo.WebSite;
                }
                if (SupplierFromDB.SupplierLicenceDetail.Count > 0)
                {
                    foreach (DataGridViewRow Row in SupplierLicenceInfoGrid.Rows)
                    {
                        SupplierLicenceDetail SupplierLicenceDetail = SupplierFromDB.SupplierLicenceDetail.FirstOrDefault(x => x.CompanySupplierLicenseMasterId == (long)Row.Cells[(int)SupplierFormTaxInfoTableColumn.MASTERID].Value)!;
                        if (SupplierLicenceDetail != null)
                        {
                            Row.Cells[(int)SupplierFormTaxInfoTableColumn.DNAME].Value = SupplierLicenceDetail.DisplayName;
                            Row.Cells[(int)SupplierFormTaxInfoTableColumn.VALUE].Value = SupplierLicenceDetail.Value;
                            Row.Cells[(int)SupplierFormTaxInfoTableColumn.ID].Value = SupplierLicenceDetail.SupplierLicenceId;
                        }
                    }
                }

                if (SupplierFromDB.IsSubAccount)
                {
                    Supplier Supplier = SupplierManager.GetSupplierById((long)SupplierFromDB.ParentAccountId!);
                    if (Supplier != null)
                    {
                        ComboUtils.InitializeSupplierCombo(ComboBoxSupplierParentAccount, Global.Company.CompanyId);
                        ComboBoxSupplierParentAccount.SelectedIndex = ComboBoxSupplierParentAccount.FindStringExact(Supplier.Name);
                    }
                }
                else
                {
                    ComboBoxSupplierParentAccount.SelectedIndex = -1;
                }

            }
            else
            {
                DisplaySystemError("Somthing went wrong, the selected supplier is not valid.");
                return;
            }
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            this.formIsDirty = false;
            LoadSupplierWithFilter();
            return;
        }
        private Supplier GetSupplierFromForm()
        {
            Supplier lSupplier = new Supplier();
            if (!string.IsNullOrEmpty(TextBoxSupplierAccountId.Text))
            {
                lSupplier.Id = Convert.ToInt64(TextBoxSupplierAccountId.Text);
            }
            else
            {
                lSupplier.Id = 0L;
            }
            lSupplier.Name = TextBoxSupplierName.Text.Trim();
            lSupplier.Discription = TextBoxSupplierDescription.Text.Trim();
            lSupplier.DisplayAs = TextBoxSupplierDisplayAs.Text.Trim();
            float Balance = string.IsNullOrEmpty(TextBoxSupplierBalance.Text) ? 0 : float.Parse(TextBoxSupplierBalance.Text);
            lSupplier.Balance = (Balance != 0 && ComboBoxBalanceType.SelectedIndex == 1) ? -Balance : Balance;
            lSupplier.BalanceAsOf = (DateTime)DateTimePickerSupplier.Date!;
            lSupplier.CompanyId = Global.Company.CompanyId;
            lSupplier.AccountType = AccountType.SUPPLIER;
            lSupplier.GSTNo = TextBoxGSTNo.Text.Trim();
            if (CheckBoxSupplierIsbranch.Checked == true)
            {
                if (ComboBoxSupplierParentAccount.SelectedIndex > -1)
                {
                    Supplier Supplier = (Supplier)ComboBoxSupplierParentAccount.Items[ComboBoxSupplierParentAccount.SelectedIndex];
                    if (Supplier != null)
                    {
                        Supplier = SupplierManager.GetSupplierById(Supplier.Id);
                        if (Supplier != null)
                        {
                            lSupplier.ParentAccountId = Supplier.Id;
                            lSupplier.IsSubAccount = true;
                        }
                    }
                }
            }
            else
            {
                lSupplier.IsSubAccount = false;
            }
            if (SupplierLicenceInfoGrid.Rows.Count > 0)
            {
                lSupplier.SupplierLicenceDetail = new List<SupplierLicenceDetail>() { };
                for (int i = 0; i < SupplierLicenceInfoGrid.Rows.Count; i++)
                {
                    SupplierLicenceDetail SupplierLicenceDetail = new SupplierLicenceDetail();
                    SupplierLicenceDetail.CompanySupplierLicenseMasterId = SupplierLicenceInfoGrid.Rows[i].Cells[(int)SupplierFormTaxInfoTableColumn.MASTERID].Value == null ? 0L : (long)SupplierLicenceInfoGrid.Rows[i].Cells[(int)SupplierFormTaxInfoTableColumn.MASTERID].Value;
                    SupplierLicenceDetail.SupplierLicenceId = SupplierLicenceInfoGrid.Rows[i].Cells[(int)SupplierFormTaxInfoTableColumn.ID].Value == null ? 0L : (long)SupplierLicenceInfoGrid.Rows[i].Cells[(int)SupplierFormTaxInfoTableColumn.ID].Value;
                    SupplierLicenceDetail.DisplayName = SupplierLicenceInfoGrid.Rows[i].Cells[(int)SupplierFormTaxInfoTableColumn.DNAME].Value.ToString()!.Trim();
                    SupplierLicenceDetail.Value = SupplierLicenceInfoGrid.Rows[i].Cells[(int)SupplierFormTaxInfoTableColumn.VALUE].Value != null ? SupplierLicenceInfoGrid.Rows[i].Cells[(int)SupplierFormTaxInfoTableColumn.VALUE].Value.ToString()!.Trim() : null;
                    SupplierLicenceDetail.CompanyId = Global.Company.CompanyId;

                    lSupplier.SupplierLicenceDetail.Add(SupplierLicenceDetail);
                }
            }
            return lSupplier;
        }
        private Address GetSupplierAddressFromForm()
        {
            Address lAddress = new Address();
            lAddress.AddressLine1 = AddressGroupBoxSupplier.AddressLine1;
            lAddress.AddressLine2 = AddressGroupBoxSupplier.AddressLine2;
            lAddress.CityOrTown = AddressGroupBoxSupplier.CityName;
            lAddress.District = AddressGroupBoxSupplier.DistrictName;
            lAddress.PinCode = AddressGroupBoxSupplier.PinCode;
            lAddress.StatesId = AddressGroupBoxSupplier.StateId;
            return lAddress;
        }
        private ContactInfo GetSupplierContactInfoFromForm()
        {
            ContactInfo lContactInfo = new ContactInfo();
            lContactInfo.Phone = TextBoxSupplierPhone.Text.Trim().Replace("-", "");
            lContactInfo.Mobile = TextBoxSupplierMobile.Text.Trim().Replace("-", "");
            lContactInfo.Fax = TextBoxSupplierFax.Text.Trim();
            lContactInfo.Email = TextBoxSupplierEmail.Text.Trim();
            lContactInfo.WebSite = TextBoxSupplierWebsite.Text.Trim();
            return lContactInfo;
        }
        private void TreeViewSupplier_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                TreeNode node = e.Node!;
                node.SelectedImageIndex = node.ImageIndex;

                ResetForm();
                LoadComboBox();
                LoadSupplierInfo();
                EnableForm(false);
                this.formIsDirty = false;

                // ✅ Load all products fresh (reset both trees)
                LoadAllProductsToTreeView();

                // ✅ Then load selected products based on current supplier
                Supplier selectedSupplier = GetSupplierInfo();
                if (selectedSupplier != null)
                {
                    LoadLinkedProductsForSupplier(selectedSupplier);
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void TextBoxSupplierSearch_TextChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            LoadSupplierWithFilter();
            if (TreeViewSupplier.Nodes.Count > 0) { BtnSupplierEdit.Enabled = true; BtnSupplierDelete.Enabled = true; }
            else { BtnSupplierEdit.Enabled = false; BtnSupplierDelete.Enabled = false; }
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }

        private void BtnSupplierNew_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            LoadComboBox();
            AddressGroupBoxSupplier.StateId = (long)Global.Company.Address.StatesId!;
            TabControlSupplier.SelectedTab = TabGeneralPage;
            TextBoxSupplierName.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void BtnSupplierDelete_Click(object sender, EventArgs e)
        {
            ToolStripStatusLabelErrorSupplier.Text = "";
            if (string.IsNullOrEmpty(TextBoxSupplierAccountId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this supplier is still valid.");
                return;
            }
            Supplier SupplierInfo = GetSupplierInfo();
            if (SupplierInfo != null)
            {
                DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, SupplierInfo.Name), "Delete Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        Supplier lSupplier = SupplierManager.CheckSupplierSubSupplier(SupplierInfo.Id);
                        if (lSupplier == null)
                        {
                            if (SupplierManager.DeleteSupplier(SupplierInfo.Id))
                            {
                                ResetForm();
                                TabControlSupplier.SelectedTab = TabGeneralPage;
                                LoadComboBox();
                                LoadSupplierWithFilter();
                                TextBoxSupplierSearch.Clear();
                                EnableForm(false);
                            }
                            else
                            {
                                ToolStripStatusLabelErrorSupplier.Text = DeleteErrorText;
                            }
                            this.formIsDirty = false;
                        }
                        else
                        {
                            ToolStripStatusLabelErrorSupplier.Text = IsbranchDeleteErrorText;
                        }
                    }
                    catch
                    {

                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
            else
            {
                DisplaySystemError("Somting went wrong, please check this supplier is still valid.");
                return;
            }
        }
        private void BtnSupplierEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxSupplierAccountId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this supplier is still valid.");
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            LoadSupplierInfo();
            EnableForm(true);
            if (CheckBoxSupplierIsbranch.Checked == false)
            {
                CheckBoxSupplierIsbranch.Enabled = false;
                ComboBoxSupplierParentAccount.Visible = false;
            }
            TabControlSupplier.SelectedTab = TabGeneralPage;
            TextBoxSupplierName.Select();
            ToolStripStatusLabelErrorSupplier.Text = "";
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void BtnSupplierCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TabControlSupplier.SelectedTab = TabGeneralPage;
                    TextBoxSupplierName.Select();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            TabControlSupplier.SelectedTab = TabGeneralPage;
            LoadComboBox();
            LoadAllProductsToTreeView();
            if (string.IsNullOrEmpty(TextBoxSupplierSearch.Text))
            {
                if (TreeViewSupplier.Nodes.Count > 0)
                {
                    LoadSupplierInfo();
                }
            }
            else
            {
                TextBoxSupplierSearch.Clear();
            }
            EnableForm(false);
            TextBoxSupplierSearch.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void BtnSupplierSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (validateForm())
                {
                    Supplier lSupplier = GetSupplierFromForm();
                    // Fill selected products
                    lSupplier.SupplierProducts = GetSelectedSupplierProducts(lSupplier.Id);
                    Supplier lSupplierFromDB = null!;
                    if (lSupplier.Id == 0)
                    {
                        Supplier lSupplierName = SupplierManager.GetSupplierByName(lSupplier.Name, Global.Company.CompanyId);
                        if (lSupplierName == null)
                        {
                            lSupplier.Address = GetSupplierAddressFromForm();
                            lSupplier.ContactInfo = GetSupplierContactInfoFromForm();
                            lSupplierFromDB = SupplierManager.AddSupplier(lSupplier);
                        }
                        else
                        {
                            ToolStripStatusLabelErrorSupplier.Text = string.Format(UniqueSupplierNameErrorMsg, lSupplier.Name, Global.Company.Name);
                            TabControlSupplier.SelectedTab = TabGeneralPage;
                            TextBoxSupplierName.Select();
                            return;
                        }
                    }
                    else
                    {
                        Supplier lSupplierById = SupplierManager.GetSupplierById(lSupplier.Id);
                        if (lSupplierById != null)
                        {
                            Supplier lSupplierName = SupplierManager.CheckSupplierNameInUpdate(lSupplier.Name, lSupplier.Id, Global.Company.CompanyId);
                            if (lSupplierName == null)
                            {
                                Address lAddress = GetSupplierAddressFromForm();
                                ContactInfo lContactInfo = GetSupplierContactInfoFromForm();
                                lSupplier.ContactInfoId = lSupplierById.ContactInfoId;
                                lSupplier.AddressId = lSupplierById.AddressId;
                                lContactInfo.Id = (long)lSupplier.ContactInfoId!;
                                lAddress.AddressId = (long)lSupplier.AddressId!;
                                lSupplierFromDB = SupplierManager.UpdateSupplier(lSupplier);
                                Address lAddress1 = AddressManager.UpdateAddress(lAddress);
                                ContactInfo llContactInfo = ContactInfoManager.UpdateContactInfo(lContactInfo);
                            }
                            else
                            {
                                ToolStripStatusLabelErrorSupplier.Text = string.Format(UniqueSupplierNameErrorMsg, lSupplier.Name, Global.Company.Name);
                                TabControlSupplier.SelectedTab = TabGeneralPage;
                                TextBoxSupplierName.Select();
                                return;
                            }
                        }
                        else
                        {
                            DisplaySystemError("Somting went wrong, please check this supplier is still valid.");
                            return;
                        }
                    }
                    if (CreateSupplierOnLoad)
                    {
                        if (parent != null)
                        {
                            parent.AccountIdTransport.ResetText();
                            parent.AccountIdTransport.Text = lSupplierFromDB.Id.ToString();
                        }
                        this.Close();
                    }
                    ResetForm();
                    TabControlSupplier.SelectedTab = TabGeneralPage;
                    LoadComboBox();
                    if (string.IsNullOrEmpty(TextBoxSupplierSearch.Text))
                    {
                        LoadSupplierWithFilter();
                    }
                    else
                    {
                        TextBoxSupplierSearch.Clear();
                    }
                    TreeNode TreeNode = new TreeNode();
                    TreeNode = lSupplierFromDB.ParentAccountId == null ? TreeViewSupplier.Nodes[lSupplierFromDB.Id.ToString()] : TreeViewSupplier.Nodes[lSupplierFromDB.ParentAccountId.ToString()].Nodes[lSupplierFromDB.Id.ToString()];
                    TreeViewSupplier.SelectedNode = TreeNode;
                    TreeViewSupplier.Focus();
                    EnableForm(false);
                    ToolStripStatusLabelErrorSupplier.Text = SaveSuccessText;
                    this.formIsDirty = false;
                }
            }
            catch
            {

            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void BtnCurrencyExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private Boolean validateForm()
        {
            ToolStripStatusLabelErrorSupplier.Text = "";
            if (string.IsNullOrEmpty(TextBoxSupplierName.Text.Trim()))
            {
                ToolStripStatusLabelErrorSupplier.Text = EnterSupplierNameErrorMsg;
                TabControlSupplier.SelectedTab = TabGeneralPage;
                TextBoxSupplierName.Select();
                return false;
            }
            if (CheckBoxSupplierIsbranch.Checked == true && ComboBoxSupplierParentAccount.SelectedIndex < 0)
            {
                ToolStripStatusLabelErrorSupplier.Text = ChooseParentErrorMsg;
                TabControlSupplier.SelectedTab = TabGeneralPage;
                ComboBoxSupplierParentAccount.Select();
                return false;
            }
            if (CheckBoxSupplierIsbranch.Checked == true && ComboBoxSupplierParentAccount.SelectedIndex >= 0 && SupplierManager.GetSupplierById(((Supplier)ComboBoxSupplierParentAccount.Items[ComboBoxSupplierParentAccount.SelectedIndex]).Id) == null)
            {
                ToolStripStatusLabelErrorSupplier.Text = "Somting went wrong, please check this parent supplier is still valid.";
                TabControlSupplier.SelectedTab = TabGeneralPage;
                ComboBoxSupplierParentAccount.Select();
                return false;
            }
            if (ComboBoxBalanceType.SelectedIndex == -1)
            {
                ToolStripStatusLabelErrorSupplier.Text = ChooseBalanceTypeErrorMsg;
                TabControlSupplier.SelectedTab = TabGeneralPage;
                ComboBoxBalanceType.Select();
                return false;
            }
            if (DateTimePickerSupplier.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerSupplier.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ToolStripStatusLabelErrorSupplier.Text = EnterValidAsOfDateErrorMsg;
                TabControlSupplier.SelectedTab = TabGeneralPage;
                DateTimePickerSupplier.Focus();
                return false;
            }
            if (!string.IsNullOrEmpty(TextBoxSupplierEmail.Text.Trim()) && !KeypressValidation.EmailValidation(TextBoxSupplierEmail.Text.Trim()))
            {
                ToolStripStatusLabelErrorSupplier.Text = EnterEmailErrorMsg;
                TabControlSupplier.SelectedTab = TabContactInfo;
                TextBoxSupplierEmail.Select();
                return false;
            }
            if (!string.IsNullOrEmpty(TextBoxSupplierWebsite.Text.Trim()) && !KeypressValidation.WebsiteValidation(TextBoxSupplierWebsite.Text.Trim()))
            {
                ToolStripStatusLabelErrorSupplier.Text = EnterWebsiteErrorMsg;
                TabControlSupplier.SelectedTab = TabContactInfo;
                TextBoxSupplierWebsite.Select();
                return false;
            }
            if (AddressGroupBoxSupplier.StateId == 0L)
            {
                ToolStripStatusLabelErrorSupplier.Text = ChooseStateErrorMsg;
                TabControlSupplier.SelectedTab = TabContactInfo;
                AddressGroupBoxSupplier.selected_field(Fields.state);
                return false;
            }

            if (SupplierLicenceInfoGrid.Rows.Count > 0)
            {
                for (int i = 0; i < SupplierLicenceInfoGrid.Rows.Count; i++)
                {
                    if ((SupplierLicenceInfoGrid.Rows[i].Cells[(int)SupplierFormTaxInfoTableColumn.VALUE].Value == null || string.IsNullOrEmpty(SupplierLicenceInfoGrid.Rows[i].Cells[(int)SupplierFormTaxInfoTableColumn.VALUE].Value.ToString()!.Trim())) && (bool)SupplierLicenceInfoGrid.Rows[i].Cells[(int)SupplierFormTaxInfoTableColumn.REQUIR].Value)
                    {
                        ToolStripStatusLabelErrorSupplier.Text = string.Format(TaxInfoGrid_MantatoryFiledErrorMsg, SupplierLicenceInfoGrid.Columns[(int)SupplierFormTaxInfoTableColumn.VALUE].HeaderText);
                        TabControlSupplier.SelectedTab = TabLicenseInfo;
                        SupplierLicenceInfoGrid.Select();
                        SupplierLicenceInfoGrid.CurrentCell = SupplierLicenceInfoGrid[(int)SupplierFormTaxInfoTableColumn.VALUE, i];
                        return false;
                    }
                }
            }
            return true;
        }
        private void ResetForm()
        {
            ToolStripStatusLabelErrorSupplier.Text = "";
            AddressGroupBoxSupplier.Clear();
            AddressGroupBoxSupplier.CountryId = (long)Global.Company.CountryId!;
            AddressGroupBoxSupplier.StateId = (long)Global.Company.Address.StatesId!;
            TextBoxSupplierAccountId.ResetText();
            TextBoxSupplierName.ResetText();
            TextBoxSupplierDisplayAs.ResetText();
            CheckBoxSupplierIsbranch.Checked = false;
            TextBoxSupplierDescription.ResetText();
            TextBoxSupplierBalance.ResetText();
            TextBoxSupplierBalance.Decimals = Global.Company.PrimaryCurrency.RoundingPrecision;
            TextBoxSupplierEmail.ResetText();
            TextBoxSupplierFax.ResetText();
            TextBoxSupplierPhone.ResetText();
            TextBoxSupplierMobile.ResetText();
            TextBoxSupplierWebsite.ResetText();
            ComboBoxSupplierParentAccount.SelectedIndex = -1;
            ComboBoxBalanceType.SelectedIndex = 0;
            DateTimePickerSupplier.Format = Global.Company.DateFormat;
            DateTimePickerSupplier.MaxDate = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
            DateTimePickerSupplier.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
        }
        private void EnableForm(Boolean enable)
        {
            if (TreeViewSupplier.Nodes.Count > 0)
            {
                TreeViewSupplier.Enabled = !enable;
                TextBoxSupplierSearch.ReadOnly = enable;
                TextBoxSupplierSearch.TabStop = !enable;
            }
            else
            {
                TreeViewSupplier.Enabled = false;
                TextBoxSupplierSearch.ReadOnly = true;
                TextBoxSupplierSearch.TabStop = false;
            }
            TextBoxSupplierName.ReadOnly = !enable;
            TextBoxSupplierPhone.ReadOnly = !enable;
            AddressGroupBoxSupplier.ReadOnly = !enable;
            TextBoxSupplierWebsite.ReadOnly = !enable;
            TextBoxSupplierMobile.ReadOnly = !enable;
            TextBoxSupplierFax.ReadOnly = !enable;
            TextBoxSupplierEmail.ReadOnly = !enable;
            TextBoxSupplierDisplayAs.ReadOnly = !enable;
            TextBoxSupplierDescription.ReadOnly = !enable;
            TextBoxSupplierBalance.ReadOnly = !enable;
            TextBoxSupplierName.TabStop = enable;
            TextBoxSupplierPhone.TabStop = enable;
            TextBoxSupplierWebsite.TabStop = enable;
            TextBoxSupplierMobile.TabStop = enable;
            TextBoxSupplierFax.TabStop = enable;
            TextBoxSupplierEmail.TabStop = enable;
            TextBoxSupplierDisplayAs.TabStop = enable;
            TextBoxSupplierDescription.TabStop = enable;
            TextBoxSupplierBalance.TabStop = enable;
            AddressGroupBoxSupplier.TabStop = enable;
            CheckBoxSupplierIsbranch.TabStop = enable;
            if (ComboBoxSupplierParentAccount.Items.Count > 0)
            {
                CheckBoxSupplierIsbranch.Enabled = enable;
            }
            DateTimePickerSupplier.ReadOnly = !enable;
            DateTimePickerSupplier.TabStop = enable;
            SupplierLicenceInfoGrid.Enabled = enable;
            ComboBoxBalanceType.Visible = enable;
            if (CheckBoxSupplierIsbranch.Checked && enable)
                ComboBoxSupplierParentAccount.Visible = true;
            else
                ComboBoxSupplierParentAccount.Visible = false;
            if (!enable)
            {
                BtnSupplierCancel.Enabled = enable;
                if (TreeViewSupplier.SelectedNode == null)
                {
                    BtnSupplierDelete.Enabled = enable;
                    BtnSupplierEdit.Enabled = enable;
                }
                else
                {
                    BtnSupplierDelete.Enabled = !enable;
                    BtnSupplierEdit.Enabled = !enable;
                }
                BtnSupplierNew.Enabled = !enable;
                BtnSupplierSave.Enabled = enable;
            }
            else
            {
                BtnSupplierCancel.Enabled = enable;
                BtnSupplierDelete.Enabled = !enable;
                BtnSupplierEdit.Enabled = !enable;
                BtnSupplierNew.Enabled = !enable;
                BtnSupplierSave.Enabled = enable;
            }
        }
        int Index = -1;
        private void CheckBoxSupplierIsbranch_CheckedChanged(object sender, EventArgs e)
        {
            ComboBoxSupplierParentAccount.Visible = CheckBoxSupplierIsbranch.Checked;
            if (!CheckBoxSupplierIsbranch.Checked) { LabelSupplierParent.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular); Index = ComboBoxSupplierParentAccount.SelectedIndex; ComboBoxSupplierParentAccount.SelectedIndex = -1; }
            else if (CheckBoxSupplierIsbranch.Checked) { LabelSupplierParent.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold); ComboBoxSupplierParentAccount.SelectedIndex = Index; }
        }
        private void TextBoxSupplierBalance_Leave(object sender, EventArgs e)
        {
            if (TextBoxSupplierBalance.Text == "." || TextBoxSupplierBalance.Text == "")
            {
                TextBoxSupplierBalance.Text = "0.00";
            }
            else
            {
                TextBoxSupplierBalance.Text = String.Format("{0:0.00}", float.Parse(TextBoxSupplierBalance.Text));
            }
        }
        private void TextBoxSupplierSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxSupplierName_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxSupplierDisplayAs_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }

        private void TextBoxSupplierBalance_KeyPress(object sender, KeyPressEventArgs e)
        {
            string Unselected = TextBoxSupplierBalance.Text;
            if (TextBoxSupplierBalance.SelectionLength > 0) { Unselected = TextBoxSupplierBalance.Text.Remove(TextBoxSupplierBalance.SelectionStart, TextBoxSupplierBalance.SelectionLength); }
            KeypressValidation.Keypress_NumberDot(sender, e, Unselected);
        }

        private void TextBoxSupplierEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_EmailChecking(sender, e);
        }
        private void TextBoxSupplierWebsite_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_WebsiteNameChecking(sender, e);
        }

        private void TextBoxSupplierCst_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_TaxDetailsNumberChecking(sender, e);
        }
        private void TextBoxSupplierGst_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_TaxDetailsNumberChecking(sender, e);
        }
        private void TextBoxSupplierPan_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_TaxDetailsNumberChecking(sender, e);
        }


        private void TextBoxSupplierSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                TreeViewSupplier.Select();
            }
        }
        private void DateTimePickerSupplier_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlSupplier.SelectedTab = TabContactInfo;
                TextBoxSupplierPhone.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TabControlSupplier.SelectedTab = TabGeneralPage;
                TextBoxSupplierBalance.Select();
            }
        }
        private void TextBoxSupplierPhone_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlSupplier.SelectedTab = TabContactInfo;
                TextBoxSupplierMobile.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TabControlSupplier.SelectedTab = TabGeneralPage;
                DateTimePickerSupplier.Focus();
            }
        }
        private void AddressGroupBoxSupplier_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (SupplierLicenceInfoGrid.Rows.Count > 0)
                {
                    TabControlSupplier.SelectedTab = TabLicenseInfo;
                    SupplierLicenceInfoGrid.Focus();
                    SupplierLicenceInfoGrid.CurrentCell = SupplierLicenceInfoGrid[(int)SupplierFormTaxInfoTableColumn.DNAME, 0];
                }
                else
                {
                    TabControlSupplier.SelectedTab = TabContactInfo;
                    BtnSupplierSave.Focus();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TabControlSupplier.SelectedTab = TabContactInfo;
                TextBoxSupplierWebsite.Select();
            }
        }

        private void BtnSupplierSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlSupplier.SelectedTab = TabGeneralPage;
                TextBoxSupplierName.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (SupplierLicenceInfoGrid.Rows.Count > 0)
                {
                    TabControlSupplier.SelectedTab = TabLicenseInfo;
                    SupplierLicenceInfoGrid.Focus();
                    SupplierLicenceInfoGrid.CurrentCell = SupplierLicenceInfoGrid[(int)SupplierFormTaxInfoTableColumn.DNAME, 0];
                    SupplierLicenceInfoGrid.BeginEdit(true);
                }
                else
                {
                    TabControlSupplier.SelectedTab = TabContactInfo;
                    AddressGroupBoxSupplier.selected_field(Fields.pin);
                }
            }
        }

        private void TextBoxSupplierName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxSupplierDisplayAs.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                BtnSupplierSave.Select();
            }
        }
        private void ComboBoxSupplierParentAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxSupplierParentAccount.DroppedDown = false;
        }



        private void TextBoxSupplierName_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "NameChecking");

        }

        private void TextBoxSupplierName_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxSupplierName, "NameChecking");
            }
        }

        private void TextBoxSupplierEmail_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "EmailChecking");

        }

        private void TextBoxSupplierEmail_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxSupplierEmail, "EmailChecking");
            }
        }

        private void TextBoxSupplierWebsite_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "WebsiteNameChecking");

        }

        private void TextBoxSupplierWebsite_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxSupplierWebsite, "WebsiteNameChecking");
            }

        }

        private void TextBoxSupplierDisplayAs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxSupplierDisplayAs, "NameChecking");
            }
        }

        private void TextBoxSupplierBalance_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "NumberDot");

        }

        private void TextBoxSupplierBalance_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxSupplierBalance, "NumberDot");
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnSupplierNew.PerformClick();
            }
            if (keyData == (Keys.F4))
            {
                BtnSupplierDelete.PerformClick();
            }
            if (keyData == (Keys.F7))
            {
                BtnSupplierEdit.PerformClick();
            }
            if (keyData == (Keys.F8))
            {
                BtnSupplierSave.PerformClick();
            }
            if (keyData == (Keys.F10))
            {
                BtnCurrencyExit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                if (CreateSupplierOnLoad)
                {
                    BtnCurrencyExit.PerformClick();
                    return true;
                }
                BtnSupplierCancel.PerformClick();
                return true;
            }
            try
            {
                if (SupplierLicenceInfoGrid.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && SupplierLicenceInfoGrid.CurrentCell.ColumnIndex == (int)SupplierFormTaxInfoTableColumn.VALUE)
                    {
                        if (SupplierLicenceInfoGrid.CurrentRow.Index == SupplierLicenceInfoGrid.RowCount - 1)
                        {
                            SupplierLicenceInfoGrid.CurrentCell = null;
                            BtnSupplierSave.Focus();
                        }
                        else
                        {
                            SendKeys.Send("{tab}{tab}");
                        }
                    }

                    if (keyData == (Keys.Tab | Keys.Shift) && SupplierLicenceInfoGrid.CurrentCell!.ColumnIndex == (int)SupplierFormTaxInfoTableColumn.DNAME)
                    {
                        if (SupplierLicenceInfoGrid.CurrentRow.Index != 0)
                        {
                            SendKeys.Send("{tab}{tab}");
                        }
                        if (SupplierLicenceInfoGrid.CurrentRow.Index == 0)
                        {
                            TabControlSupplier.SelectedTab = TabContactInfo;
                            AddressGroupBoxSupplier.selected_field(Fields.pin);
                            SupplierLicenceInfoGrid.CurrentCell = null;
                        }
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void FormSupplier_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty && !CreateSupplierOnLoad)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TabControlSupplier.SelectedTab = TabGeneralPage;
                    TextBoxSupplierName.Select();
                    e.Cancel = true;
                }
            }
        }

        private void SupplierLicenceInfoGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void SupplierLicenceInfoGrid_Leave(object sender, EventArgs e)
        {
            SupplierLicenceInfoGrid.CurrentCell = null;
        }

        private void SupplierLicenceInfoGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            SupplierLicenceInfoGrid.Rows[e.RowIndex].Cells[(int)SupplierFormTaxInfoTableColumn.NAME].ReadOnly = true;
            SupplierLicenceInfoGrid.Rows[e.RowIndex].Cells[(int)SupplierFormTaxInfoTableColumn.DNAME].ReadOnly = false;
            SupplierLicenceInfoGrid.Rows[e.RowIndex].Cells[(int)SupplierFormTaxInfoTableColumn.VALUE].ReadOnly = false;
            SupplierLicenceInfoGrid.Rows[e.RowIndex].Cells[(int)SupplierFormTaxInfoTableColumn.REPORT].ReadOnly = true;
        }
        private void LoadSupplierProducts(Supplier supplier)
        {
            IList<Product> allProducts = CatalogProductManager.Instance.ListProductByCompanyId(Global.Company.CompanyId).ToArray<Product>();
            if (allProducts == null || allProducts.Count == 0)
            {
                Console.WriteLine("No products found in the catalog");
                return;
            }
        }
        private void LoadAllProductsToTreeView()
        {
            TreeViewProduct.Nodes.Clear();
            TreeViewSelectedProduct.Nodes.Clear();

            string filterAvailable = TextBoxProductSearch.Text.Trim();

            IList<Product> allProducts = CatalogProductManager.Instance
                .ListProductByCompanyId(Global.Company.CompanyId)
                .OrderBy(p => p.Name)
                .ToList();

            foreach (var product in allProducts)
            {
                if (string.IsNullOrEmpty(filterAvailable) || product.Name.IndexOf(filterAvailable, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    TreeViewProduct.Nodes.Add(new TreeNode
                    {
                        Text = $"{product.MaterialId} - {product.Name}",
                        Name = product.Id.ToString(),
                        Tag = product
                    });
                }
            }

            TreeViewProduct.ExpandAll();
        }
        private void LoadLinkedProductsForSupplier(Supplier supplier)
        {
            if (supplier == null || supplier.SupplierProducts == null)
                return;

            string filterSelected = TextBoxSelectedProductSearch.Text.Trim();

            // Track product IDs already added
            HashSet<long> linkedProductIds = supplier.SupplierProducts.Select(sp => sp.ProductId).ToHashSet();

            // Remove linked products from TreeViewProduct
            foreach (TreeNode node in TreeViewProduct.Nodes.Cast<TreeNode>().ToList())
            {
                if (node.Tag is Product product && linkedProductIds.Contains(product.Id))
                {
                    TreeViewProduct.Nodes.Remove(node);

                    if (string.IsNullOrEmpty(filterSelected) || product.Name.IndexOf(filterSelected, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        TreeViewSelectedProduct.Nodes.Add(new TreeNode
                        {
                            Text = node.Text,
                            Name = node.Name,
                            Tag = product
                        });
                    }
                }
            }

            TreeViewSelectedProduct.ExpandAll();
        }

        private void LoadTreeViewProductsForSupplier()
        {
            string filterAvailable = TextBoxProductSearch.Text.Trim();
            string filterSelected = TextBoxSelectedProductSearch.Text.Trim();

            TreeViewProduct.Nodes.Clear();
            TreeViewSelectedProduct.Nodes.Clear();
            IList<Product> allProducts = CatalogProductManager.Instance.ListProductByCompanyId(Global.Company.CompanyId)
                                                .OrderBy(p => p.Name).ToList();
            Supplier supplier = new Supplier();
            HashSet<long> linkedProductIds = supplier.SupplierProducts?.Select(sp => sp.ProductId).ToHashSet() ?? new HashSet<long>();

            // Available Products
            foreach (var product in allProducts)
            {
                if (!linkedProductIds.Contains(product.Id) &&
                    (string.IsNullOrEmpty(filterAvailable) || product.Name.IndexOf(filterAvailable, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    TreeNode node = new TreeNode
                    {
                        Text = $"{product.MaterialId} - {product.Name}",
                        Name = product.Id.ToString(),
                        Tag = product
                    };
                    TreeViewProduct.Nodes.Add(node);
                }
            }

            // Linked (Selected) Products
            foreach (var product in allProducts)
            {
                if (linkedProductIds.Contains(product.Id) &&
                    (string.IsNullOrEmpty(filterSelected) || product.Name.IndexOf(filterSelected, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    TreeNode node = new TreeNode
                    {
                        Text = $"{product.MaterialId} - {product.Name}",
                        Name = product.Id.ToString(),
                        Tag = product
                    };
                    TreeViewSelectedProduct.Nodes.Add(node);
                }
            }

            TreeViewProduct.ExpandAll();
            TreeViewSelectedProduct.ExpandAll();
        }

        private void TextBoxProductSearch_TextChanged(object sender, EventArgs e)
        {
            string filter = TextBoxProductSearch.Text.Trim();
            TreeViewProduct.Nodes.Clear();

            IList<Product> allProducts = CatalogProductManager.Instance.ListProductByCompanyId(Global.Company.CompanyId)
                                                .OrderBy(p => p.Name).ToList();

            // Get product IDs already shown in TreeViewSelectedProduct
            HashSet<long> selectedProductIds = TreeViewSelectedProduct.Nodes.Cast<TreeNode>()
                .Select(n => (n.Tag as Product)?.Id ?? 0)
                .ToHashSet();

            foreach (var product in allProducts)
            {
                if (selectedProductIds.Contains(product.Id)) continue;

                if (string.IsNullOrEmpty(filter)
                    || (!string.IsNullOrEmpty(product.Name) && product.Name.Contains(filter, StringComparison.OrdinalIgnoreCase))
                    || (!string.IsNullOrEmpty(product.MaterialId) && product.MaterialId.Contains(filter, StringComparison.OrdinalIgnoreCase))
                    || (!string.IsNullOrEmpty(product.HSNCode) && product.HSNCode.Contains(filter, StringComparison.OrdinalIgnoreCase))
                    || (!string.IsNullOrEmpty(product.UOM) && product.UOM.Contains(filter, StringComparison.OrdinalIgnoreCase)))
                {
                    TreeViewProduct.Nodes.Add(new TreeNode
                    {
                        Text = $"{product.MaterialId} - {product.Name}",
                        Name = product.Id.ToString(),
                        Tag = product
                    });
                }
            }

            TreeViewProduct.ExpandAll();
        }

        private void TextBoxSelectedProductSearch_TextChanged(object sender, EventArgs e)
        {
            string filter = TextBoxSelectedProductSearch.Text.Trim();
            TreeViewSelectedProduct.Nodes.Clear();

            foreach (TreeNode node in TreeViewSelectedProduct.Nodes.Cast<TreeNode>().ToList())
            {
                Product? product = node.Tag as Product;
                if (product == null) continue;

                if (string.IsNullOrEmpty(filter)
                    || (!string.IsNullOrEmpty(product.Name) && product.Name.Contains(filter, StringComparison.OrdinalIgnoreCase))
                    || (!string.IsNullOrEmpty(product.MaterialId) && product.MaterialId.Contains(filter, StringComparison.OrdinalIgnoreCase))
                    || (!string.IsNullOrEmpty(product.HSNCode) && product.HSNCode.Contains(filter, StringComparison.OrdinalIgnoreCase))
                    || (!string.IsNullOrEmpty(product.UOM) && product.UOM.Contains(filter, StringComparison.OrdinalIgnoreCase)))
                {
                    TreeViewSelectedProduct.Nodes.Add(new TreeNode
                    {
                        Text = $"{product.MaterialId} - {product.Name}",
                        Name = product.Id.ToString(),
                        Tag = product
                    });
                }
            }

            TreeViewSelectedProduct.ExpandAll();
        }

        private void BtnSelectProduct_Click(object sender, EventArgs e)
        {
            TextBoxSelectedProductSearch.ResetText();

            for (int i = TreeViewProduct.Nodes.Count - 1; i >= 0; i--)
            {
                TreeNode node = TreeViewProduct.Nodes[i];
                if (node.Checked)
                {
                    MoveProductToSelected(node);
                }
            }
        }

        private void BtnRemoveProduct_Click(object sender, EventArgs e)
        {
            TextBoxProductSearch.ResetText();

            for (int i = TreeViewSelectedProduct.Nodes.Count - 1; i >= 0; i--)
            {
                TreeNode node = TreeViewSelectedProduct.Nodes[i];
                if (node.Checked)
                {
                    MoveProductToAvailable(node);
                }
            }
        }
        private void MoveProductToSelected(TreeNode node)
        {
            if (node?.Tag is Product product)
            {
                // Remove from available
                TreeViewProduct.Nodes.Remove(node);

                // Uncheck to avoid accidental double move
                node.Checked = false;

                // Add to selected
                TreeViewSelectedProduct.Nodes.Add(node);
            }
        }
        private void MoveProductToAvailable(TreeNode node)
        {
            if (node?.Tag is Product product)
            {
                // Remove from selected
                TreeViewSelectedProduct.Nodes.Remove(node);

                // Uncheck to avoid accidental double move
                node.Checked = false;

                // Add back to available
                TreeViewProduct.Nodes.Add(node);
            }
        }
        private List<SupplierProduct> GetSelectedSupplierProducts(long supplierId)
        {
            var selected = new List<SupplierProduct>();

            foreach (TreeNode node in TreeViewSelectedProduct.Nodes)
            {
                if (node.Tag is Product product)
                {
                    selected.Add(new SupplierProduct
                    {
                        SupplierId = supplierId,
                        ProductId = product.Id
                    });
                }
            }

            return selected;
        }

    }
}
