using fa.api.Accounting;
using fa.libraries.utils;
using fa.model.Accounting.Masters;
using fa.model.hms.config;
using fa.api.Hms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using fa.api.System;
using fa.model.System;
using fa.model.Hms.common;
using fa.api.UserProfile;
using System.Linq;
using fa.views.account.masters;
using fa.views.controls;
using fa.api.utils;
using fa.views.controls.grid;
using DocumentFormat.OpenXml.ExtendedProperties;

namespace fa.views.hms.config
{
    public partial class FormHospitalSettings : FormPatientBase
    {
        public static string ChoosePaperFormatErrorMsg = "Please choose {0} paper format";
        public static string InvalidSeedErrorMsg = "{0} Seed start from 1, Please enter valid Seed";
        public static string EnterSeedErrorMsg = "Please enter {0} seed";
        public static string EnterFeeErrorMsg = "Please enter {0} registration fee.";
        public static string SelectAccountErrorMsg = "Please select {0} registration account.";
        public static string SelectCustomerErrorMsg = "Please select valid Customer.";

        public static string EnterNameErrorMsg = "Please enter name";
        public static string UniqueNameErrorMsg = "Invoice group {0} already exists";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string DeleteConfirmText = "Do you want to delete the invoice group {0}?";
        public static string DeleteErrorText = "Error Deleting the invoice group!, Please retry";

        Container parent;
        public long ConfigurationId = 0L;
        bool[] HasEntry = null;
        public FormHospitalSettings(object sender)
        {
            parent = (Container)sender;
            InitializeComponent();
        }
        private void FormHospitalSettings_Load(object sender, EventArgs e)
        {
            ResetForm();
            ResetInvGroup();
            LoadTransactionTypeGroupMappings();
            LoadInvoiceGroup();
            TextBoxRegistrationFee.Select();
            this.formIsDirty = false;
        }
        private void LoadComboBox()
        {
            ComboUtils.InitializeYearCombo(ComboBoxHmsYear, Global.Company);
        }
        private void ResetForm()
        {
            TabControlHospitalSetting.SelectedTab = TabOpRegistration;
            HMSConfigErrorMsg.Text = string.Empty;
            TextBoxIpRegistrationFee.Text = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            TextBoxRegistrationFee.Text = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            ComboUtils.InitializeAllCustomerCombo(ComboBoxPatientPurchaseAccount, Global.Company.CompanyId);
            ComboUtils.InitializeAllAccountCombo(ComboBoxIpRegistrationAccount, Global.Company.CompanyId);
            ComboUtils.InitializeAllAccountCombo(ComboBoxOpRegistrationAccount, Global.Company.CompanyId);
            ComboBoxIpRegistrationAccount.SelectedIndex = -1;
            ComboBoxOpRegistrationAccount.SelectedIndex = -1;
            ComboBoxHmsYear.SelectedIndex = -1;
            GridViewHmsYear.Rows.Clear();
            LoadConfiguration();
            LoadComboBox();
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridViewHmsYear.Columns["RoundOff"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
        }
        private void LoadConfiguration()
        {
            HospitalConfiguration HospitalConfiguration = HospitalConfigurationManager.Instance.GetSettingsByCompanyId(Global.Company.CompanyId);
            if (HospitalConfiguration != null)
            {
                ConfigurationId = HospitalConfiguration.Id;
                TextBoxIpRegistrationFee.Text = HospitalConfiguration.DefaultIPConsultingFee.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                TextBoxRegistrationFee.Text = HospitalConfiguration.DefaultOPConsultingFee.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                if (HospitalConfiguration.IPRegistrationFeeAccountId != null)
                {
                    Account Account = AccountManager.Instance.GetAccountById((long)HospitalConfiguration.IPRegistrationFeeAccountId);
                    if (Account != null)
                    {
                        ComboBoxIpRegistrationAccount.SelectedIndex = ComboBoxIpRegistrationAccount.FindStringExact(Account.Name);
                    }
                }
                if (HospitalConfiguration.OPRegistrationFeeAccountId != null)
                {
                    Account Account = AccountManager.Instance.GetAccountById((long)HospitalConfiguration.OPRegistrationFeeAccountId);
                    if (Account != null)
                    {
                        ComboBoxOpRegistrationAccount.SelectedIndex = ComboBoxOpRegistrationAccount.FindStringExact(Account.Name);
                    }
                }
                if (Global.Company.PatientPurchaseAccountId != null)
                {
                    Customer Customer = CustomerManager.Instance.GetCustomerById((long)Global.Company.PatientPurchaseAccountId);
                    if (Customer != null)
                    {
                        ComboBoxPatientPurchaseAccount.SelectedIndex = ComboBoxPatientPurchaseAccount.FindStringExact(Customer.Name);
                    }
                }
                CheckBoxInvoiceGroupEnable.Checked = HospitalConfiguration.HasInvoicingGroup;

                CheckBoxDigitalSingnature.Checked = HospitalConfiguration.IsDigitalSignatureDisplayOnPresc;
                CheckBoxSingnatureName.Checked = HospitalConfiguration.IsSignatureNameDisplayOnPresc;

                checkBoxLabtestDigSignature.Checked = HospitalConfiguration.IsDigitalSignatureDisplayOnLabtest;
                checkBoxLabTechnicianSignName.Checked = HospitalConfiguration.IsSignatureNameDisplayOnLabtest;

                CheckBoxFooterDetailsPresc.Checked = HospitalConfiguration.IsFooterDetailsDisplayOnPresc;
                CheckBoxFooterDetailsLabtest.Checked = HospitalConfiguration.IsFooterDetailsDisplayOnLabtest;

                TextBoxFooterDetailsPresc.Text = HospitalConfiguration.FooterDetailsPrescroption;
                TextBoxFooterDetailsLabtest.Text = HospitalConfiguration.FooterDetailsLabtest;
            }
        }

        private void BtnHMSSettingCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            this.formIsDirty = false;
        }
        private HospitalConfiguration HospitalConfigurationFromForm()
        {
            HospitalConfiguration HospitalConfiguration = new HospitalConfiguration();
            HospitalConfiguration.CompanyId = Global.Company.CompanyId;
            HospitalConfiguration.Id = ConfigurationId;
            HospitalConfiguration.DefaultOPConsultingFee = double.Parse(TextBoxRegistrationFee.Text);
            HospitalConfiguration.DefaultIPConsultingFee = double.Parse(TextBoxIpRegistrationFee.Text);
            HospitalConfiguration.HasInvoicingGroup = CheckBoxInvoiceGroupEnable.Checked;
            if (ComboBoxOpRegistrationAccount.SelectedIndex > -1)
            {
                Account Account = ((Account)ComboBoxOpRegistrationAccount.Items[ComboBoxOpRegistrationAccount.SelectedIndex]);
                if (Account != null)
                {
                    HospitalConfiguration.OPRegistrationFeeAccountId = Account.Id;
                }
            }
            if (ComboBoxIpRegistrationAccount.SelectedIndex > -1)
            {
                Account Account = ((Account)ComboBoxIpRegistrationAccount.Items[ComboBoxIpRegistrationAccount.SelectedIndex]);
                if (Account != null)
                {
                    HospitalConfiguration.IPRegistrationFeeAccountId = Account.Id;
                }
            }
            if (ComboBoxPatientPurchaseAccount.SelectedIndex > -1)
            {
                Customer Customer = ((Customer)ComboBoxPatientPurchaseAccount.Items[ComboBoxPatientPurchaseAccount.SelectedIndex]);
                if (Customer != null)
                {
                    HospitalConfiguration.PatientPurchaseAccount = Customer;
                }
            }
            HospitalConfiguration.IsDigitalSignatureDisplayOnPresc = CheckBoxDigitalSingnature.Checked;
            HospitalConfiguration.IsSignatureNameDisplayOnPresc = CheckBoxSingnatureName.Checked;

            HospitalConfiguration.IsDigitalSignatureDisplayOnLabtest = checkBoxLabtestDigSignature.Checked;
            HospitalConfiguration.IsSignatureNameDisplayOnLabtest = checkBoxLabTechnicianSignName.Checked;

            HospitalConfiguration.IsFooterDetailsDisplayOnPresc = CheckBoxFooterDetailsPresc.Checked;
            HospitalConfiguration.IsFooterDetailsDisplayOnLabtest = CheckBoxFooterDetailsLabtest.Checked;

            HospitalConfiguration.FooterDetailsPrescroption = TextBoxFooterDetailsPresc.Text;
            HospitalConfiguration.FooterDetailsLabtest = TextBoxFooterDetailsLabtest.Text;

            return HospitalConfiguration;
        }
        private List<IdSpace> ListHospitalIdspaceFromForm()
        {
            List<IdSpace> lIdSpace = new List<IdSpace>();
            String Year = (string)ComboBoxHmsYear.Items[ComboBoxHmsYear.SelectedIndex];
            string[] Years = Year.Split('-');
            string Start = new DateTime(int.Parse(Years[0]), Global.Company.AccountingStartDate, 1).ToString(Global.Company.DateFormat);
            string End = new DateTime(int.Parse(Years[0]), Global.Company.AccountingStartDate, 1).AddYears(1).AddDays(-1).ToString(Global.Company.DateFormat);
            foreach (DataGridViewRow Row in GridViewHmsYear.Rows)
            {
                IdSpace CompanyEntryConfiguration = new IdSpace();
                CompanyEntryConfiguration.EntryType = (EntryType)Row.Cells[(int)CompanyReferenceTableColumn.TYPEID].Value;
                CompanyEntryConfiguration.IsResetDaily = (bool)Row.Cells[(int)CompanyReferenceTableColumn.DAILYRESET].Value;
                CompanyEntryConfiguration.IsDotMatrix = Row.Cells[(int)CompanyReferenceTableColumn.DOTMATRIX].Value != null ? (bool)Row.Cells[(int)CompanyReferenceTableColumn.DOTMATRIX].Value : false;
                CompanyEntryConfiguration.Prefix = Row.Cells[(int)CompanyReferenceTableColumn.PREFIX].Value != null ? Row.Cells[(int)CompanyReferenceTableColumn.PREFIX].Value.ToString() : string.Empty;
                CompanyEntryConfiguration.Seed = long.Parse(Row.Cells[(int)CompanyReferenceTableColumn.SEED].Value.ToString());
                CompanyEntryConfiguration.RunningSeed = long.Parse(Row.Cells[(int)CompanyReferenceTableColumn.SEED].Value.ToString());
                CompanyEntryConfiguration.YearStartDate = DateTime.ParseExact(Start, Global.Company.DateFormat, null);
                CompanyEntryConfiguration.YearEndDate = DateTime.ParseExact(End, Global.Company.DateFormat, null);
                CompanyEntryConfiguration.Date = Global.getTransactionDate();
                if (Row.Cells[(int)CompanyReferenceTableColumn.PRINTTYPE].Value != null)
                {
                    CompanyEntryConfiguration.PrintPaperFormat_Id = (long)Row.Cells[(int)CompanyReferenceTableColumn.PRINTTYPE].Value;
                }
                CompanyEntryConfiguration.RoundOff = Row.Cells[(int)CompanyReferenceTableColumn.ROUNDOFF].Value != null ? double.Parse(Row.Cells[(int)CompanyReferenceTableColumn.ROUNDOFF].Value.ToString()) : 0;
                lIdSpace.Add(CompanyEntryConfiguration);
            }
            return lIdSpace;
        }
        private void BtnHMSSettingSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (ValidateForm())
            {
                HospitalConfiguration HospitalConfiguration = HospitalConfigurationFromForm();
                if (HospitalConfiguration.Id == 0L)
                {
                    HospitalConfigurationManager.Instance.AddHospitalConfiguration(HospitalConfiguration, ListHospitalIdspaceFromForm());
                }
                else
                {
                    HospitalConfigurationManager.Instance.UpdateHospitalConfiguration(HospitalConfiguration, ListHospitalIdspaceFromForm());
                }
                Global.Company = CompanyManager.Instance.GetCompany(Global.Company.CompanyId);
                ResetForm();
                HMSConfigErrorMsg.Text = "Saved success.";
                this.formIsDirty = false;
            }
            Cursor.Current = Cursors.Default;
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(TextBoxRegistrationFee.Text))
            {
                HMSConfigErrorMsg.Text = string.Format(EnterFeeErrorMsg, "out patient");
                TabControlHospitalSetting.SelectedTab = TabOpRegistration;
                TextBoxRegistrationFee.Select();
                return false;
            }
            if (ComboBoxOpRegistrationAccount.SelectedIndex < 0)
            {
                HMSConfigErrorMsg.Text = string.Format(SelectAccountErrorMsg, "out patient");
                TabControlHospitalSetting.SelectedTab = TabOpRegistration;
                ComboBoxOpRegistrationAccount.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxIpRegistrationFee.Text))
            {
                HMSConfigErrorMsg.Text = string.Format(EnterFeeErrorMsg, "in patient");
                TabControlHospitalSetting.SelectedTab = TabIpRegistration;
                TextBoxIpRegistrationFee.Select();
                return false;
            }
            if (ComboBoxIpRegistrationAccount.SelectedIndex < 0)
            {
                HMSConfigErrorMsg.Text = string.Format(SelectAccountErrorMsg, "in patient");
                TabControlHospitalSetting.SelectedTab = TabIpRegistration;
                ComboBoxIpRegistrationAccount.Select();
                return false;
            }
            //company reference
            if (!ValidateReferenceGrid(GridViewHmsYear)) { return false; }
            return true;
        }
        private bool ValidateReferenceGrid(DataViewVerticalScroll GridView)
        {
            for (int i = 0; i < GridView.Rows.Count - 1; i++)
            {
                if (GridView.Rows[i].Cells[(int)CompanyReferenceTableColumn.SEED].Value == null || string.IsNullOrEmpty(GridView.Rows[i].Cells[(int)CompanyReferenceTableColumn.SEED].Value.ToString().Trim()) || long.Parse(GridView.Rows[i].Cells[(int)CompanyReferenceTableColumn.SEED].Value.ToString()) == 0)
                {
                    HMSConfigErrorMsg.Text = string.Format(EnterSeedErrorMsg, GridView.Rows[i].Cells[(int)CompanyReferenceTableColumn.TYPE].Value.ToString());
                    TabControlHospitalSetting.SelectedTab = TabPrinterSetup;
                    GridView.Select();
                    GridView.CurrentCell = GridView[(int)CompanyReferenceTableColumn.SEED, i];
                    GridView.BeginEdit(true);
                    return false;
                }
                if ((bool)GridView.Rows[i].Cells[(int)CompanyReferenceTableColumn.HASPRINTERSETUP].Value && (GridView.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE].Value == null || string.IsNullOrEmpty(GridView.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE].Value.ToString().Trim())))
                {
                    HMSConfigErrorMsg.Text = string.Format(ChoosePaperFormatErrorMsg, GridView.Rows[i].Cells[(int)CompanyReferenceTableColumn.TYPE].Value.ToString());
                    TabControlHospitalSetting.SelectedTab = TabPrinterSetup;
                    GridView.Select();
                    GridView.CurrentCell = GridView[(int)CompanyReferenceTableColumn.PRINTTYPE, i];
                    GridView.BeginEdit(true);
                    return false;
                }
            }
            return true;
        }
        private void BtnHMSSettingExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ComboBoxOpRegistrationAccount_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TabIpRegistration;
                TextBoxIpRegistrationFee.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TabOpRegistration;
                TextBoxRegistrationFee.Focus();
            }
        }

        private void TextBoxRegistrationFee_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TabOpRegistration;
                ComboBoxOpRegistrationAccount.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TabOpRegistration;
                BtnHMSSettingSave.Focus();
            }
        }

        private void TextBoxIpRegistrationFee_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TabIpRegistration;
                ComboBoxIpRegistrationAccount.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TabOpRegistration;
                ComboBoxOpRegistrationAccount.Focus();
            }
        }

        private void BtnHMSSettingSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TabOpRegistration;
                TextBoxRegistrationFee.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TabControlMedicalTestSetup;
                TextBoxFooterDetailsLabtest.Focus();
            }
        }

        private void ComboBoxIpRegistrationAccount_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TabPrinterSetup;
                ComboBoxHmsYear.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TabIpRegistration;
                TextBoxIpRegistrationFee.Focus();
            }
        }
        private void ComboBoxPatientPurchaseAccount_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TapInvoiceGroup;
                CheckBoxInvoiceGroupEnable.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TabPrinterSetup;
                ComboBoxHmsYear.Focus();
            }
        }
        private void CheckBoxInvoiceGroupEnable_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TapInvoiceGroup;
                ListBoxInvoiceGroup.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TabDefaultAccounts;
                ComboBoxPatientPurchaseAccount.Select();
            }
        }
        private void BtnUpdateInvGroup_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TabControlPrescriptionSetup;
                CheckBoxDigitalSingnature.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TapInvoiceGroup;
                ListBoxInvoiceGroup.Select();
            }
        }
        private void ComboBoxHmsYear_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TabPrinterSetup;
                GridViewHmsYear.CurrentCell = GridViewHmsYear[1, 0];
                GridViewHmsYear.BeginEdit(true);
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TabIpRegistration;
                ComboBoxIpRegistrationAccount.Select();
            }
        }
        private void LoadInvoiceGroup()
        {
            ListBoxInvoiceGroup.Items.Clear();
            IList<PatientLedgerTransactionTypeGroup> TransactionTypeGroup = HospitalInvoiceGroupManager.Instance.ListAllPatientLedgerTransactionTypeGroup(Global.Company.CompanyId);
            foreach (var Group in TransactionTypeGroup)
            {
                ListBoxInvoiceGroup.Items.Add(Group);
            }
            if (ListBoxInvoiceGroup.Items.Count > 0)
            {
                ListBoxInvoiceGroup.SelectedIndex = 0;
            }
        }
        private PatientLedgerTransactionTypeGroup GetTransactionTypeGroup()
        {
            PatientLedgerTransactionTypeGroup lTransactionTypeGroup = (ListBoxInvoiceGroup.SelectedIndex > -1 ? (PatientLedgerTransactionTypeGroup)ListBoxInvoiceGroup.Items[ListBoxInvoiceGroup.SelectedIndex] : null);
            return (lTransactionTypeGroup != null ? HospitalInvoiceGroupManager.Instance.GetPatientLedgerTransactionTypeGroupById(lTransactionTypeGroup.Id) : null);
        }
        private void LoadTransactionTypeGroup()
        {
            PatientLedgerTransactionTypeGroup TransactionTypeGroupFromDB = GetTransactionTypeGroup();
            if (TransactionTypeGroupFromDB != null)
            {
                TextBoxInvoiceGroupName.Text = TransactionTypeGroupFromDB.Name;
                TextBoxInvoiceGroupDescription.Text = TransactionTypeGroupFromDB.Description;
                LoadTransactionTypeGroupMappings();
                if (TransactionTypeGroupFromDB.PatientLedgerTransactionTypeGroupMappings.Count > 0)
                {
                    foreach (var Mappings in TransactionTypeGroupFromDB.PatientLedgerTransactionTypeGroupMappings)
                    {
                        CheckedListBoxInvoiceGroup.Items.Add(ComboUtils.GetTypeDescription(Mappings.TransactionType), true);
                    }
                }
            }
            else
            {
                DisplaySystemError("Somthing went wrong, the selected transaction type is not valid.");
                return;
            }
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            this.formIsDirty = false;
            LoadTransactionTypeGroup();
            return;
        }
        private void LoadTransactionTypeGroupMappings()
        {
            CheckedListBoxInvoiceGroup.Items.Clear();
            IList<PatientLedgerTransactionTypeGroupMapping> GroupMapping = HospitalInvoiceGroupManager.Instance.ListAllPatientLedgerTransactionTypeGroupMapping(Global.Company.CompanyId);

            List<TransactionType> Types = Enum.GetValues(typeof(TransactionType)).Cast<TransactionType>().ToList();
            foreach (var Type in Types)
            {
                if (GroupMapping.FirstOrDefault(x => x.TransactionType == Type) == null && Type != TransactionType.PAYMENT && Type != TransactionType.WAIVER)
                {
                    CheckedListBoxInvoiceGroup.Items.Add(ComboUtils.GetTypeDescription(Type));
                }
            }
            if (CheckedListBoxInvoiceGroup.Items.Count > 0)
            {
                CheckedListBoxInvoiceGroup.SelectedIndex = 0;
            }
        }
        private void ResetInvGroup()
        {
            HMSConfigErrorMsg.Text = string.Empty;
            TextBoxInvoiceGroupDescription.ResetText();
            TextBoxInvoiceGroupName.ResetText();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if (keyData == (Keys.F8))
            {
                BtnHMSSettingSave.PerformClick();
            }
            if (keyData == (Keys.F10))
            {
                BtnHMSSettingExit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnHMSSettingCancel.PerformClick();
                return true;
            }
            if (GridViewHmsYear.CurrentCell != null && GridViewHmsYear.CanFocus)
            {
                if (keyData == (Keys.Tab) && GridViewHmsYear.CurrentCell.ColumnIndex == (int)CompanyReferenceTableColumn.ROUNDOFF)
                {
                    if (GridViewHmsYear.CurrentRow.Index == GridViewHmsYear.RowCount - 1)
                    {
                        TabControlHospitalSetting.SelectedTab = TabDefaultAccounts;
                        ComboBoxPatientPurchaseAccount.Select();
                    }
                    else
                    {
                        GridViewHmsYear.CurrentCell = GridViewHmsYear[(int)CompanyReferenceTableColumn.PREFIX, GridViewHmsYear.CurrentRow.Index + 1];
                    }
                }
                if (keyData == (Keys.Tab) && GridViewHmsYear.CurrentCell.ColumnIndex == (int)CompanyReferenceTableColumn.DAILYRESET)
                {
                    if (!(bool)GridViewHmsYear.CurrentRow.Cells[(int)CompanyReferenceTableColumn.HASPRINTERSETUP].Value && !(bool)GridViewHmsYear.CurrentRow.Cells[(int)CompanyReferenceTableColumn.HASROUNDOFF].Value)
                    {
                        if (GridViewHmsYear.CurrentRow.Index == GridViewHmsYear.RowCount - 1)
                        {
                            TabControlHospitalSetting.SelectedTab = TabDefaultAccounts;
                            ComboBoxPatientPurchaseAccount.Select();
                        }
                        else
                        {
                            GridViewHmsYear.CurrentCell = GridViewHmsYear[(int)CompanyReferenceTableColumn.TYPE, GridViewHmsYear.CurrentRow.Index + 1];
                        }
                    }
                    else if (!(bool)GridViewHmsYear.CurrentRow.Cells[(int)CompanyReferenceTableColumn.HASPRINTERSETUP].Value)
                    {
                        GridViewHmsYear.CurrentCell = GridViewHmsYear[(int)CompanyReferenceTableColumn.DOTMATRIX, GridViewHmsYear.CurrentRow.Index];
                    }
                }
                if (keyData == (Keys.Tab) && GridViewHmsYear.CurrentCell.ColumnIndex == (int)CompanyReferenceTableColumn.PRINTTYPE)
                {
                    if (!(bool)GridViewHmsYear.CurrentRow.Cells[(int)CompanyReferenceTableColumn.HASDOTMATRIX].Value && !(bool)GridViewHmsYear.CurrentRow.Cells[(int)CompanyReferenceTableColumn.HASROUNDOFF].Value)
                    {
                        if (GridViewHmsYear.CurrentRow.Index == GridViewHmsYear.RowCount - 1)
                        {
                            TabControlHospitalSetting.SelectedTab = TabDefaultAccounts;
                            ComboBoxPatientPurchaseAccount.Select();
                        }
                        else
                        {
                            GridViewHmsYear.CurrentCell = GridViewHmsYear[(int)CompanyReferenceTableColumn.PREFIX, GridViewHmsYear.CurrentRow.Index + 1];
                        }
                    }
                    else if (!(bool)GridViewHmsYear.CurrentRow.Cells[(int)CompanyReferenceTableColumn.HASDOTMATRIX].Value)
                    {
                        GridViewHmsYear.CurrentCell = GridViewHmsYear[(int)CompanyReferenceTableColumn.ROUNDOFF, GridViewHmsYear.CurrentRow.Index];
                    }
                }
                if (keyData == (Keys.Tab) && GridViewHmsYear.CurrentCell.ColumnIndex == (int)CompanyReferenceTableColumn.DOTMATRIX)
                {
                    if (!(bool)GridViewHmsYear.CurrentRow.Cells[(int)CompanyReferenceTableColumn.HASROUNDOFF].Value)
                    {
                        if (GridViewHmsYear.CurrentRow.Index == GridViewHmsYear.RowCount - 1)
                        {
                            TabControlHospitalSetting.SelectedTab = TabDefaultAccounts;
                            ComboBoxPatientPurchaseAccount.Select();
                        }
                        else
                        {
                            GridViewHmsYear.CurrentCell = GridViewHmsYear[(int)CompanyReferenceTableColumn.PREFIX, GridViewHmsYear.CurrentRow.Index + 1];
                        }
                    }
                }
                if (keyData == (Keys.Tab | Keys.Shift) && GridViewHmsYear.CurrentCell.ColumnIndex == (int)CompanyReferenceTableColumn.PREFIX)
                {
                    int Index = GridViewHmsYear.CurrentCell.RowIndex;
                    if (GridViewHmsYear.CurrentCell.RowIndex == 0)
                    {
                        ComboBoxHmsYear.Focus();
                    }
                    else if (!(bool)GridViewHmsYear.Rows[Index - 1].Cells[(int)CompanyReferenceTableColumn.HASPRINTERSETUP].Value && !(bool)GridViewHmsYear.Rows[Index - 1].Cells[(int)CompanyReferenceTableColumn.HASROUNDOFF].Value)
                    {
                        GridViewHmsYear.CurrentCell = GridViewHmsYear[(int)CompanyReferenceTableColumn.DAILYRESET, GridViewHmsYear.CurrentRow.Index - 1];
                    }
                    else if (!(bool)GridViewHmsYear.Rows[Index - 1].Cells[(int)CompanyReferenceTableColumn.HASDOTMATRIX].Value && !(bool)GridViewHmsYear.Rows[Index - 1].Cells[(int)CompanyReferenceTableColumn.HASROUNDOFF].Value)
                    {
                        GridViewHmsYear.CurrentCell = GridViewHmsYear[(int)CompanyReferenceTableColumn.PRINTTYPE, GridViewHmsYear.CurrentRow.Index - 1];
                    }
                    else if (!(bool)GridViewHmsYear.Rows[Index - 1].Cells[(int)CompanyReferenceTableColumn.HASROUNDOFF].Value)
                    {
                        GridViewHmsYear.CurrentCell = GridViewHmsYear[(int)CompanyReferenceTableColumn.DOTMATRIX, GridViewHmsYear.CurrentRow.Index - 1];
                    }
                    else
                    {
                        GridViewHmsYear.CurrentCell = GridViewHmsYear[(int)CompanyReferenceTableColumn.ROUNDOFF, GridViewHmsYear.CurrentRow.Index - 1];
                    }
                }
                if (keyData == (Keys.Tab | Keys.Shift) && GridViewHmsYear.CurrentCell.ColumnIndex == (int)CompanyReferenceTableColumn.ROUNDOFF)
                {
                    if (!(bool)GridViewHmsYear.CurrentRow.Cells[(int)CompanyReferenceTableColumn.HASPRINTERSETUP].Value)
                    {
                        GridViewHmsYear.CurrentCell = GridViewHmsYear[(int)CompanyReferenceTableColumn.DAILYRESET, GridViewHmsYear.CurrentRow.Index];
                    }
                    else if (!(bool)GridViewHmsYear.CurrentRow.Cells[(int)CompanyReferenceTableColumn.HASDOTMATRIX].Value)
                    {
                        GridViewHmsYear.CurrentCell = GridViewHmsYear[(int)CompanyReferenceTableColumn.PRINTTYPE, GridViewHmsYear.CurrentRow.Index];
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void FormHospitalSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxInvoiceGroupName.Select();
                    e.Cancel = true;
                }
            }
        }

        private void ListBoxInvoiceGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetInvGroup();
            LoadTransactionTypeGroup();
            ListBoxInvoiceGroup.Select();
            this.formIsDirty = false;
        }

        private void BtnUpdateInvGroup_Click(object sender, EventArgs e)
        {
            FormInvoiceGrouping FormInvoiceGrouping = new FormInvoiceGrouping();
            FormInvoiceGrouping.ShowDialog();
            ResetInvGroup();
            LoadTransactionTypeGroupMappings();
            LoadInvoiceGroup();
            this.formIsDirty = false;
        }

        private void ComboBoxHmsYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewHmsYear.Rows.Clear();
            if (ComboBoxHmsYear.SelectedIndex > -1)
            {
                String Year = (string)ComboBoxHmsYear.Items[ComboBoxHmsYear.SelectedIndex];
                string[] Years = Year.Split('-');
                string Start = new DateTime(int.Parse(Years[0]), Global.Company.AccountingStartDate, 1).ToString(Global.Company.DateFormat);
                string End = new DateTime(int.Parse(Years[0]), Global.Company.AccountingStartDate, 1).AddYears(1).AddDays(-1).ToString(Global.Company.DateFormat);
                List<IdSpace> IdSpaces = CompanyManager.Instance.ListIdSpaceForHospital(Global.Company.CompanyId, DateTime.ParseExact(Start, Global.Company.DateFormat, null), DateTime.ParseExact(End, Global.Company.DateFormat, null));
                if (IdSpaces != null && IdSpaces.Count > 0)
                {
                    IList<PrintPaperFormat> PrintPaperFormat = PaperFormatManager.Instance.ListPrintPaperFormat();
                    HasEntry = HospitalConfigurationManager.HasEntry(Global.Company.CompanyId, DateTime.ParseExact(Start, Global.Company.DateFormat, null), DateTime.ParseExact(End, Global.Company.DateFormat, null));
                    int i = 0;
                    foreach (IdSpace IdSpace in IdSpaces.OrderBy(x => x.EntryType))
                    {
                        GridViewHmsYear.Rows.Add();
                        GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.TYPEID].Value = IdSpace.EntryType;
                        GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.TYPE].Value = ComboUtils.GetEntryTypeName(IdSpace.EntryType);
                        GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.PREFIX].Value = IdSpace.Prefix;
                        GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.SEED].Value = IdSpace.Seed;
                        GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.DAILYRESET].Value = IdSpace.IsResetDaily;
                        GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.HASROUNDOFF].Value = IdSpace.HasRoundOff;
                        GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.HASPRINTERSETUP].Value = IdSpace.HasPrinterSetup;
                        GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.HASDOTMATRIX].Value = IdSpace.EntryType == EntryType.PATIENT_INVOICE ? true : IdSpace.HasDotMatrix;
                        if (IdSpace.HasPrinterSetup)
                        {
                            (GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE] as DataGridViewComboBoxCell)!.DataSource = null;
                            if (IdSpace.EntryType == EntryType.PATIENT_INVOICE)
                            {
                                (GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE] as DataGridViewComboBoxCell)!.DataSource = PrintPaperFormat.Where(x => x.Name == "A4 PORTRAIT" || x.Name == "A5 LANDSCAPE").ToList();
                            }
                            else
                            {
                                (GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE] as DataGridViewComboBoxCell)!.DataSource = PrintPaperFormat.Where(x => x.Name == "80 MM ROLL").ToList();
                            }
                            (GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE] as DataGridViewComboBoxCell)!.ValueMember = "Id";
                            (GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE] as DataGridViewComboBoxCell)!.DisplayMember = "Name";
                            GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE].Value = IdSpace.PrintPaperFormat_Id == null ? ((PrintPaperFormat)(GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE] as DataGridViewComboBoxCell)!.Items[0]).Id : IdSpace.PrintPaperFormat_Id;
                            if (IdSpace.HasDotMatrix || IdSpace.EntryType == EntryType.PATIENT_INVOICE)
                            {
                                GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.DOTMATRIX].Value = IdSpace.IsDotMatrix;
                                if (IdSpace.EntryType == EntryType.PATIENT_INVOICE && !IdSpace.IsDotMatrix)
                                {
                                    GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.DOTMATRIX] = new DataGridViewTextBoxCell();
                                    GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.DOTMATRIX].ReadOnly = true;
                                }
                                else if (IdSpace.EntryType == EntryType.PATIENT_INVOICE && IdSpace.IsDotMatrix)
                                {
                                    GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.DOTMATRIX] = new DataGridViewCheckBoxCell();
                                    GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.DOTMATRIX].Value = IdSpace.IsDotMatrix;
                                    GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.DOTMATRIX].ReadOnly = true;
                                }
                            }
                            else
                            {
                                GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.DOTMATRIX] = new DataGridViewTextBoxCell();
                                GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.DOTMATRIX].ReadOnly = true;
                            }
                        }
                        else
                        {
                            GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE] = new DataGridViewTextBoxCell();
                            GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.DOTMATRIX] = new DataGridViewTextBoxCell();
                            GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.PRINTTYPE].ReadOnly = true;
                            GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.DOTMATRIX].ReadOnly = true;
                        }
                        if (IdSpace.HasRoundOff)
                        {
                            GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.ROUNDOFF].Value = IdSpace.RoundOff;
                        }
                        else
                        {
                            GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.ROUNDOFF] = new DataGridViewTextBoxCell();
                            GridViewHmsYear.Rows[i].Cells[(int)CompanyReferenceTableColumn.ROUNDOFF].ReadOnly = true;
                        }
                        i++;
                    }
                }
            }
            this.formIsDirty = false;
        }

        private void GridViewHmsYear_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)CompanyReferenceTableColumn.TYPE)
            {
                GridViewHmsYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.TYPE].ReadOnly = true;
            }
            if (e.ColumnIndex == (int)CompanyReferenceTableColumn.SEED)
            {
                GridViewHmsYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.SEED].ReadOnly = HasEntry[(int)GridViewHmsYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.TYPEID].Value];
            }
            if (e.ColumnIndex == (int)CompanyReferenceTableColumn.PRINTTYPE && !(bool)GridViewHmsYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.HASPRINTERSETUP].Value)
            {
                GridViewHmsYear.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
                GridViewHmsYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.DOTMATRIX].ReadOnly = true;
            }
            if (e.ColumnIndex == (int)CompanyReferenceTableColumn.DOTMATRIX)
            {
                GridViewHmsYear.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
            }
            if (e.ColumnIndex == (int)CompanyReferenceTableColumn.ROUNDOFF && !(bool)GridViewHmsYear.Rows[e.RowIndex].Cells[(int)CompanyReferenceTableColumn.HASROUNDOFF].Value)
            {
                GridViewHmsYear.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
            }
        }
        private void GridViewHmsYear_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void GridViewHmsYear_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                if (GridViewHmsYear.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }
                e.Control.KeyPress += new KeyPressEventHandler(GridViewHmsYear_KeyPress);
                ((ComboBox)e.Control).SelectedIndexChanged -= new EventHandler(PrintTypeColumnComboSelectionChanged!);
                ((ComboBox)e.Control).SelectedIndexChanged += new EventHandler(PrintTypeColumnComboSelectionChanged!);
            }
        }
        private void PrintTypeColumnComboSelectionChanged(object sender, EventArgs e)
        {
            int Index = GridViewHmsYear.CurrentCell.RowIndex;
            if (((ComboBox)sender).SelectedIndex > -1)
            {
                if (((ComboBox)sender).SelectedIndex == 1)
                {
                    GridViewHmsYear.Rows[Index].Cells[(int)CompanyReferenceTableColumn.DOTMATRIX] = new DataGridViewCheckBoxCell();
                    GridViewHmsYear.Rows[Index].Cells[(int)CompanyReferenceTableColumn.DOTMATRIX].Value = true;
                }
                else
                {
                    GridViewHmsYear.Rows[Index].Cells[(int)CompanyReferenceTableColumn.DOTMATRIX] = new DataGridViewTextBoxCell();
                    GridViewHmsYear.Rows[Index].Cells[(int)CompanyReferenceTableColumn.DOTMATRIX].ReadOnly = true;
                }
            }
        }
        private void GridViewHmsYear_KeyPress(object? sender, KeyPressEventArgs e)
        {
            ((ComboBox)GridViewHmsYear.EditingControl).DroppedDown = false;
        }
        private void BtnHmsAddYear_Click(object sender, EventArgs e)
        {
            FormFiscalYear FormFiscalYear = new FormFiscalYear();
            FormFiscalYear.CompanyId = Global.Company.CompanyId;
            FormFiscalYear.ShowDialog();
            ComboUtils.InitializeYearCombo(ComboBoxHmsYear, Global.Company);
            this.formIsDirty = false;
        }

        private void CheckBoxDigitalSingnature_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TapInvoiceGroup;
                BtnUpdateInvGroup.Focus();
            }
        }

        private void TextBoxFooterDetailsPresc_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TabControlMedicalTestSetup;
                checkBoxLabtestDigSignature.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TabControlPrescriptionSetup;
                CheckBoxFooterDetailsPresc.Focus();
            }
        }

        private void checkBoxLabtestDigSignature_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TabControlPrescriptionSetup;
                TextBoxFooterDetailsPresc.Focus();
            }
        }

        private void TextBoxFooterDetailsLabtest_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TabControlMedicalTestSetup;
                BtnHMSSettingSave.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlHospitalSetting.SelectedTab = TabControlMedicalTestSetup;
                CheckBoxFooterDetailsLabtest.Focus();
            }
        }
    }
}
