namespace fa.views.account.masters
{
    partial class FormCustomers
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCustomers));
            TabControlCustomer = new TabControl();
            TabCustomer = new TabPage();
            ComboBoxBalanceType = new fa.views.controls.ComboBoxSwapTextBox();
            ComboBoxCustomerParentAccount = new fa.views.controls.ComboBoxSwapTextBox();
            TextBoxGSTNo = new TextBox();
            labelGSTNo = new Label();
            TextBoxCustomerBalance = new fa.views.controls.text.CurrencyTextBox();
            DateTimePickerCustomer = new fa.views.controls.text.DateWithCalendar();
            TextBoxCustomerNameonCheck = new TextBox();
            CheckBoxUseDisplayName = new CheckBox();
            CheckBoxCustomerIsbranch = new CheckBox();
            TextBoxCustomerDescription = new TextBox();
            TextBoxCustomerDisplayAs = new TextBox();
            TextBoxCustomerName = new TextBox();
            LabelCustomerName = new Label();
            LabelCustomerDisplayAs = new Label();
            LabelCustomerDescription = new Label();
            LabelCustomerBranch = new Label();
            LabelCustomerBalance = new Label();
            LabelCustomerNameonCheck = new Label();
            LabelCustomerAsof = new Label();
            LabelCustomerParent = new Label();
            TabBillingInfo = new TabPage();
            ComboBoxCustomerPaymentTerm = new fa.views.controls.ComboBoxSwapTextBox();
            ComboBoxCustomerPaymentMethod = new fa.views.controls.ComboBoxSwapTextBox();
            BillingAddressGroupBoxCustomer = new fa.views.controls.AddressGroupBoxWithStateSelection();
            label4 = new Label();
            RadioCustomerLockBill = new fa.views.controls.YesNoRadio();
            TextBoxCustomerPaymentLimit = new fa.views.controls.text.CurrencyTextBox();
            label1 = new Label();
            label2 = new Label();
            BtnCustomerAddNewTerm = new Button();
            LabelCustomerPaymentTerm = new Label();
            BtnCustomerAddNewPayment = new Button();
            LabelCustomerPaymentMethod = new Label();
            TabShippingInfo = new TabPage();
            ShippingAddressGroupBoxCustomer = new fa.views.controls.AddressGroupBoxWithStateSelection();
            label3 = new Label();
            CheckBoxCustomerUseBillingAddress = new CheckBox();
            TabContactInfo = new TabPage();
            TextBoxCustomerMobile = new fa.views.controls.text.PhoneTextBox();
            TextBoxCustomerPhone = new fa.views.controls.text.PhoneTextBox();
            TextBoxCustomerFax = new MaskedTextBox();
            TextBoxCustomerWebsite = new TextBox();
            TextBoxCustomerEmail = new TextBox();
            LabelCustomerWebsite = new Label();
            LabelCustomerEmail = new Label();
            LabelCustomerFax = new Label();
            LabelCustomerMobile = new Label();
            LabelCustomerPhone = new Label();
            TabLicenseInfo = new TabPage();
            CustomerLicenceInfoGrid = new fa.views.controls.DataViewVerticalScroll();
            CustomerTaxIdName = new fa.views.controls.grid.DataGridViewNameColumn();
            CustomerTaxDisplayName = new fa.views.controls.grid.DataGridViewNameColumn();
            CustomerTaxIdValue = new fa.views.controls.grid.DataGridViewNameColumn();
            CustomerTaxIdPrintOnInvoice = new DataGridViewCheckBoxColumn();
            CustomerTaxIdPrintOnReport = new DataGridViewCheckBoxColumn();
            CustomerTaxIdDelete = new DataGridViewButtonColumn();
            CustomerTaxIdId = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewCheckBoxColumn();
            BtnCustomerSave = new Button();
            BtnCustomerCancel = new Button();
            BtnCustomerEdit = new Button();
            BtnCustomerNew = new Button();
            BtnCustomerDelete = new Button();
            TreeViewCustomer = new TreeView();
            ImageListCustomer = new ImageList(components);
            TextBoxCustomerAccountId = new MaskedTextBox();
            BtnCurrencyExit = new Button();
            toolTip1 = new ToolTip(components);
            statusStrip1 = new StatusStrip();
            ToolStripStatusLabelErrorCustomer = new ToolStripStatusLabel();
            TextBoxCustomerSearch = new fa.views.controls.text.DelayedTextChangeTextBox();
            TabControlCustomer.SuspendLayout();
            TabCustomer.SuspendLayout();
            TabBillingInfo.SuspendLayout();
            TabShippingInfo.SuspendLayout();
            TabContactInfo.SuspendLayout();
            TabLicenseInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)CustomerLicenceInfoGrid).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // checkBoxIsPatient
            // 
            checkBoxIsPatient.Size = new Size(69, 17);
            // 
            // TabControlCustomer
            // 
            TabControlCustomer.Controls.Add(TabCustomer);
            TabControlCustomer.Controls.Add(TabBillingInfo);
            TabControlCustomer.Controls.Add(TabShippingInfo);
            TabControlCustomer.Controls.Add(TabContactInfo);
            TabControlCustomer.Controls.Add(TabLicenseInfo);
            TabControlCustomer.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TabControlCustomer.Location = new Point(258, 12);
            TabControlCustomer.Name = "TabControlCustomer";
            TabControlCustomer.SelectedIndex = 0;
            TabControlCustomer.Size = new Size(707, 366);
            TabControlCustomer.TabIndex = 4;
            TabControlCustomer.TabStop = false;
            // 
            // TabCustomer
            // 
            TabCustomer.BackColor = SystemColors.Window;
            TabCustomer.Controls.Add(ComboBoxCustomerParentAccount);
            TabCustomer.Controls.Add(ComboBoxBalanceType);
            TabCustomer.Controls.Add(TextBoxGSTNo);
            TabCustomer.Controls.Add(labelGSTNo);
            TabCustomer.Controls.Add(TextBoxCustomerBalance);
            TabCustomer.Controls.Add(DateTimePickerCustomer);
            TabCustomer.Controls.Add(TextBoxCustomerNameonCheck);
            TabCustomer.Controls.Add(CheckBoxUseDisplayName);
            TabCustomer.Controls.Add(CheckBoxCustomerIsbranch);
            TabCustomer.Controls.Add(TextBoxCustomerDescription);
            TabCustomer.Controls.Add(TextBoxCustomerDisplayAs);
            TabCustomer.Controls.Add(TextBoxCustomerName);
            TabCustomer.Controls.Add(LabelCustomerName);
            TabCustomer.Controls.Add(LabelCustomerDisplayAs);
            TabCustomer.Controls.Add(LabelCustomerDescription);
            TabCustomer.Controls.Add(LabelCustomerBranch);
            TabCustomer.Controls.Add(LabelCustomerBalance);
            TabCustomer.Controls.Add(LabelCustomerNameonCheck);
            TabCustomer.Controls.Add(LabelCustomerAsof);
            TabCustomer.Controls.Add(LabelCustomerParent);
            TabCustomer.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TabCustomer.Location = new Point(4, 22);
            TabCustomer.Name = "TabCustomer";
            TabCustomer.Padding = new Padding(3);
            TabCustomer.Size = new Size(699, 340);
            TabCustomer.TabIndex = 0;
            TabCustomer.Text = "Customer";
            // 
            // ComboBoxBalanceType
            // 
            ComboBoxBalanceType.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxBalanceType.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxBalanceType.FormattingEnabled = true;
            ComboBoxBalanceType.Items.AddRange(new object[] { "DR", "CR" });
            ComboBoxBalanceType.Location = new Point(495, 308);
            ComboBoxBalanceType.Name = "ComboBoxBalanceType";
            ComboBoxBalanceType.Size = new Size(40, 21);
            ComboBoxBalanceType.TabIndex = 15;
            ComboBoxBalanceType.TxtVisible = true;
            // 
            // ComboBoxCustomerParentAccount
            // 
            ComboBoxCustomerParentAccount.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxCustomerParentAccount.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxCustomerParentAccount.FormattingEnabled = true;
            ComboBoxCustomerParentAccount.Location = new Point(19, 208);
            ComboBoxCustomerParentAccount.Name = "ComboBoxCustomerParentAccount";
            ComboBoxCustomerParentAccount.Size = new Size(356, 21);
            ComboBoxCustomerParentAccount.TabIndex = 10;
            ComboBoxCustomerParentAccount.TxtVisible = true;
            ComboBoxCustomerParentAccount.KeyPress += ComboBoxCustomerParentAccount_KeyPress;
            // 
            // TextBoxGSTNo
            // 
            TextBoxGSTNo.BackColor = SystemColors.Window;
            TextBoxGSTNo.Location = new Point(19, 308);
            TextBoxGSTNo.MaxLength = 100;
            TextBoxGSTNo.Name = "TextBoxGSTNo";
            TextBoxGSTNo.ReadOnly = true;
            TextBoxGSTNo.Size = new Size(356, 21);
            TextBoxGSTNo.TabIndex = 13;
            // 
            // labelGSTNo
            // 
            labelGSTNo.AutoSize = true;
            labelGSTNo.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            labelGSTNo.Location = new Point(19, 292);
            labelGSTNo.Name = "labelGSTNo";
            labelGSTNo.Size = new Size(42, 13);
            labelGSTNo.TabIndex = 37;
            labelGSTNo.Text = "GST No";
            // 
            // TextBoxCustomerBalance
            // 
            TextBoxCustomerBalance.BackColor = SystemColors.Window;
            TextBoxCustomerBalance.Decimals = 2;
            TextBoxCustomerBalance.Length = 10;
            TextBoxCustomerBalance.Location = new Point(392, 308);
            TextBoxCustomerBalance.Name = "TextBoxCustomerBalance";
            TextBoxCustomerBalance.ReadOnly = true;
            TextBoxCustomerBalance.Size = new Size(100, 21);
            TextBoxCustomerBalance.TabIndex = 14;
            TextBoxCustomerBalance.Text = "0.00";
            TextBoxCustomerBalance.TextAlign = HorizontalAlignment.Right;
            // 
            // DateTimePickerCustomer
            // 
            DateTimePickerCustomer.BackColor = Color.White;
            DateTimePickerCustomer.BorderStyle = BorderStyle.FixedSingle;
            DateTimePickerCustomer.Date = null;
            DateTimePickerCustomer.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DateTimePickerCustomer.Format = "MM/dd/yyyy";
            DateTimePickerCustomer.Location = new Point(575, 308);
            DateTimePickerCustomer.MaxDate = new DateTime(9997, 12, 31, 9, 16, 13, 0);
            DateTimePickerCustomer.MinDate = new DateTime(1900, 1, 1, 19, 20, 54, 0);
            DateTimePickerCustomer.Name = "DateTimePickerCustomer";
            DateTimePickerCustomer.ReadOnly = false;
            DateTimePickerCustomer.Size = new Size(93, 21);
            DateTimePickerCustomer.TabIndex = 16;
            DateTimePickerCustomer.PreviewKeyDown += DateTimePickerCustomer_PreviewKeyDown;
            // 
            // TextBoxCustomerNameonCheck
            // 
            TextBoxCustomerNameonCheck.BackColor = SystemColors.Window;
            TextBoxCustomerNameonCheck.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxCustomerNameonCheck.Location = new Point(19, 247);
            TextBoxCustomerNameonCheck.MaxLength = 50;
            TextBoxCustomerNameonCheck.Name = "TextBoxCustomerNameonCheck";
            TextBoxCustomerNameonCheck.ReadOnly = true;
            TextBoxCustomerNameonCheck.Size = new Size(356, 21);
            TextBoxCustomerNameonCheck.TabIndex = 11;
            TextBoxCustomerNameonCheck.KeyDown += TextBoxCustomerNameonCheck_KeyDown;
            TextBoxCustomerNameonCheck.KeyPress += TextBoxCustomerNameonCheck_KeyPress;
            TextBoxCustomerNameonCheck.MouseDown += TextBoxCustomerNameonCheck_MouseDown;
            // 
            // CheckBoxUseDisplayName
            // 
            CheckBoxUseDisplayName.AutoSize = true;
            CheckBoxUseDisplayName.Enabled = false;
            CheckBoxUseDisplayName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            CheckBoxUseDisplayName.Location = new Point(19, 272);
            CheckBoxUseDisplayName.Name = "CheckBoxUseDisplayName";
            CheckBoxUseDisplayName.Size = new Size(111, 17);
            CheckBoxUseDisplayName.TabIndex = 12;
            CheckBoxUseDisplayName.Text = "Use Display Name";
            CheckBoxUseDisplayName.UseVisualStyleBackColor = true;
            CheckBoxUseDisplayName.CheckedChanged += CheckBoxUseDisplayName_CheckedChanged;
            // 
            // CheckBoxCustomerIsbranch
            // 
            CheckBoxCustomerIsbranch.AutoSize = true;
            CheckBoxCustomerIsbranch.Enabled = false;
            CheckBoxCustomerIsbranch.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            CheckBoxCustomerIsbranch.Location = new Point(19, 172);
            CheckBoxCustomerIsbranch.Name = "CheckBoxCustomerIsbranch";
            CheckBoxCustomerIsbranch.Size = new Size(43, 17);
            CheckBoxCustomerIsbranch.TabIndex = 9;
            CheckBoxCustomerIsbranch.Text = "Yes";
            CheckBoxCustomerIsbranch.UseVisualStyleBackColor = true;
            CheckBoxCustomerIsbranch.CheckedChanged += CheckBoxCustomerIsbranch_CheckedChanged;
            // 
            // TextBoxCustomerDescription
            // 
            TextBoxCustomerDescription.BackColor = SystemColors.Window;
            TextBoxCustomerDescription.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxCustomerDescription.Location = new Point(19, 104);
            TextBoxCustomerDescription.MaxLength = 250;
            TextBoxCustomerDescription.Multiline = true;
            TextBoxCustomerDescription.Name = "TextBoxCustomerDescription";
            TextBoxCustomerDescription.ReadOnly = true;
            TextBoxCustomerDescription.Size = new Size(444, 49);
            TextBoxCustomerDescription.TabIndex = 8;
            // 
            // TextBoxCustomerDisplayAs
            // 
            TextBoxCustomerDisplayAs.BackColor = SystemColors.Window;
            TextBoxCustomerDisplayAs.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxCustomerDisplayAs.Location = new Point(19, 65);
            TextBoxCustomerDisplayAs.MaxLength = 100;
            TextBoxCustomerDisplayAs.Name = "TextBoxCustomerDisplayAs";
            TextBoxCustomerDisplayAs.ReadOnly = true;
            TextBoxCustomerDisplayAs.Size = new Size(444, 21);
            TextBoxCustomerDisplayAs.TabIndex = 7;
            TextBoxCustomerDisplayAs.TextChanged += TextBoxCustomerName_TextChanged;
            TextBoxCustomerDisplayAs.KeyDown += TextBoxCustomerDisplayAs_KeyDown;
            TextBoxCustomerDisplayAs.KeyPress += TextBoxCustomerDisplayAs_KeyPress;
            TextBoxCustomerDisplayAs.MouseDown += TextBoxCustomerDisplayAs_MouseDown;
            // 
            // TextBoxCustomerName
            // 
            TextBoxCustomerName.BackColor = SystemColors.Window;
            TextBoxCustomerName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxCustomerName.Location = new Point(19, 25);
            TextBoxCustomerName.MaxLength = 30;
            TextBoxCustomerName.Name = "TextBoxCustomerName";
            TextBoxCustomerName.ReadOnly = true;
            TextBoxCustomerName.Size = new Size(444, 21);
            TextBoxCustomerName.TabIndex = 6;
            TextBoxCustomerName.TextChanged += TextBoxCustomerName_TextChanged;
            TextBoxCustomerName.KeyDown += TextBoxCustomerName_KeyDown;
            TextBoxCustomerName.KeyPress += TextBoxCustomerName_KeyPress;
            TextBoxCustomerName.MouseDown += TextBoxCustomerName_MouseDown;
            TextBoxCustomerName.PreviewKeyDown += TextBoxCustomerName_PreviewKeyDown;
            // 
            // LabelCustomerName
            // 
            LabelCustomerName.AutoSize = true;
            LabelCustomerName.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelCustomerName.Location = new Point(16, 9);
            LabelCustomerName.Name = "LabelCustomerName";
            LabelCustomerName.Size = new Size(39, 13);
            LabelCustomerName.TabIndex = 0;
            LabelCustomerName.Text = "Name";
            // 
            // LabelCustomerDisplayAs
            // 
            LabelCustomerDisplayAs.AutoSize = true;
            LabelCustomerDisplayAs.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelCustomerDisplayAs.Location = new Point(16, 49);
            LabelCustomerDisplayAs.Name = "LabelCustomerDisplayAs";
            LabelCustomerDisplayAs.Size = new Size(56, 13);
            LabelCustomerDisplayAs.TabIndex = 2;
            LabelCustomerDisplayAs.Text = "Dispaly As";
            // 
            // LabelCustomerDescription
            // 
            LabelCustomerDescription.AutoSize = true;
            LabelCustomerDescription.Location = new Point(16, 89);
            LabelCustomerDescription.Name = "LabelCustomerDescription";
            LabelCustomerDescription.Size = new Size(60, 13);
            LabelCustomerDescription.TabIndex = 4;
            LabelCustomerDescription.Text = "Description";
            // 
            // LabelCustomerBranch
            // 
            LabelCustomerBranch.AutoSize = true;
            LabelCustomerBranch.Location = new Point(16, 156);
            LabelCustomerBranch.Name = "LabelCustomerBranch";
            LabelCustomerBranch.Size = new Size(40, 13);
            LabelCustomerBranch.TabIndex = 17;
            LabelCustomerBranch.Text = "Branch";
            // 
            // LabelCustomerBalance
            // 
            LabelCustomerBalance.AutoSize = true;
            LabelCustomerBalance.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelCustomerBalance.Location = new Point(389, 292);
            LabelCustomerBalance.Name = "LabelCustomerBalance";
            LabelCustomerBalance.Size = new Size(44, 13);
            LabelCustomerBalance.TabIndex = 35;
            LabelCustomerBalance.Text = "Balance";
            // 
            // LabelCustomerNameonCheck
            // 
            LabelCustomerNameonCheck.AutoSize = true;
            LabelCustomerNameonCheck.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelCustomerNameonCheck.Location = new Point(16, 231);
            LabelCustomerNameonCheck.Name = "LabelCustomerNameonCheck";
            LabelCustomerNameonCheck.Size = new Size(91, 13);
            LabelCustomerNameonCheck.TabIndex = 22;
            LabelCustomerNameonCheck.Text = "Name On Cheque";
            // 
            // LabelCustomerAsof
            // 
            LabelCustomerAsof.AutoSize = true;
            LabelCustomerAsof.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelCustomerAsof.Location = new Point(572, 292);
            LabelCustomerAsof.Name = "LabelCustomerAsof";
            LabelCustomerAsof.Size = new Size(38, 13);
            LabelCustomerAsof.TabIndex = 36;
            LabelCustomerAsof.Text = "As of ";
            // 
            // LabelCustomerParent
            // 
            LabelCustomerParent.AutoSize = true;
            LabelCustomerParent.Location = new Point(16, 190);
            LabelCustomerParent.Name = "LabelCustomerParent";
            LabelCustomerParent.Size = new Size(39, 13);
            LabelCustomerParent.TabIndex = 18;
            LabelCustomerParent.Text = "Parent";
            // 
            // TabBillingInfo
            // 
            TabBillingInfo.BackColor = SystemColors.Window;
            TabBillingInfo.Controls.Add(ComboBoxCustomerPaymentTerm);
            TabBillingInfo.Controls.Add(ComboBoxCustomerPaymentMethod);
            TabBillingInfo.Controls.Add(BillingAddressGroupBoxCustomer);
            TabBillingInfo.Controls.Add(label4);
            TabBillingInfo.Controls.Add(RadioCustomerLockBill);
            TabBillingInfo.Controls.Add(TextBoxCustomerPaymentLimit);
            TabBillingInfo.Controls.Add(label1);
            TabBillingInfo.Controls.Add(label2);
            TabBillingInfo.Controls.Add(BtnCustomerAddNewTerm);
            TabBillingInfo.Controls.Add(LabelCustomerPaymentTerm);
            TabBillingInfo.Controls.Add(BtnCustomerAddNewPayment);
            TabBillingInfo.Controls.Add(LabelCustomerPaymentMethod);
            TabBillingInfo.Location = new Point(4, 22);
            TabBillingInfo.Name = "TabBillingInfo";
            TabBillingInfo.Padding = new Padding(3);
            TabBillingInfo.Size = new Size(192, 74);
            TabBillingInfo.TabIndex = 1;
            TabBillingInfo.Text = "Billing Details";
            // 
            // ComboBoxCustomerPaymentTerm
            // 
            ComboBoxCustomerPaymentTerm.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxCustomerPaymentTerm.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxCustomerPaymentTerm.FormattingEnabled = true;
            ComboBoxCustomerPaymentTerm.Location = new Point(28, 77);
            ComboBoxCustomerPaymentTerm.Name = "ComboBoxCustomerPaymentTerm";
            ComboBoxCustomerPaymentTerm.Size = new Size(131, 21);
            ComboBoxCustomerPaymentTerm.TabIndex = 17;
            ComboBoxCustomerPaymentTerm.TxtVisible = true;
            ComboBoxCustomerPaymentTerm.KeyPress += ComboBoxCustomerPaymentTerm_KeyPress;
            // 
            // ComboBoxCustomerPaymentMethod
            // 
            ComboBoxCustomerPaymentMethod.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxCustomerPaymentMethod.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxCustomerPaymentMethod.FormattingEnabled = true;
            ComboBoxCustomerPaymentMethod.Location = new Point(28, 32);
            ComboBoxCustomerPaymentMethod.Name = "ComboBoxCustomerPaymentMethod";
            ComboBoxCustomerPaymentMethod.Size = new Size(131, 21);
            ComboBoxCustomerPaymentMethod.TabIndex = 16;
            ComboBoxCustomerPaymentMethod.TxtVisible = true;
            ComboBoxCustomerPaymentMethod.KeyPress += ComboBoxCustomerPaymentMethod_KeyPress;
            ComboBoxCustomerPaymentMethod.PreviewKeyDown += ComboBoxCustomerPaymentMethod_PreviewKeyDown;
            // 
            // BillingAddressGroupBoxCustomer
            // 
            BillingAddressGroupBoxCustomer.AddressLine1 = "";
            BillingAddressGroupBoxCustomer.AddressLine2 = "";
            BillingAddressGroupBoxCustomer.CityName = "";
            BillingAddressGroupBoxCustomer.CountryId = 0L;
            BillingAddressGroupBoxCustomer.DistrictName = "";
            BillingAddressGroupBoxCustomer.GroupName = "";
            BillingAddressGroupBoxCustomer.Location = new Point(24, 117);
            BillingAddressGroupBoxCustomer.Name = "BillingAddressGroupBoxCustomer";
            BillingAddressGroupBoxCustomer.PinCode = "";
            BillingAddressGroupBoxCustomer.ReadOnly = true;
            BillingAddressGroupBoxCustomer.Size = new Size(536, 220);
            BillingAddressGroupBoxCustomer.StateId = 0L;
            BillingAddressGroupBoxCustomer.StateName = "";
            BillingAddressGroupBoxCustomer.StateSelectedIndex = -1;
            BillingAddressGroupBoxCustomer.TabIndex = 20;
            BillingAddressGroupBoxCustomer.Load += BillingAddressGroupBoxCustomer_Leave;
            BillingAddressGroupBoxCustomer.PreviewKeyDown += BillingAddressGroupBoxCustomer_PreviewKeyDown;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(25, 106);
            label4.Name = "label4";
            label4.Size = new Size(46, 13);
            label4.TabIndex = 45;
            label4.Text = "Address";
            // 
            // RadioCustomerLockBill
            // 
            RadioCustomerLockBill.BackColor = SystemColors.Window;
            RadioCustomerLockBill.BorderStyle = BorderStyle.FixedSingle;
            RadioCustomerLockBill.Checked = false;
            RadioCustomerLockBill.FirstButtonName = "Yes";
            RadioCustomerLockBill.Location = new Point(322, 75);
            RadioCustomerLockBill.Margin = new Padding(4, 3, 4, 3);
            RadioCustomerLockBill.Name = "RadioCustomerLockBill";
            RadioCustomerLockBill.SecondButtonName = "No  ";
            RadioCustomerLockBill.Size = new Size(131, 21);
            RadioCustomerLockBill.TabIndex = 19;
            // 
            // TextBoxCustomerPaymentLimit
            // 
            TextBoxCustomerPaymentLimit.BackColor = SystemColors.Window;
            TextBoxCustomerPaymentLimit.Decimals = 2;
            TextBoxCustomerPaymentLimit.Length = 10;
            TextBoxCustomerPaymentLimit.Location = new Point(322, 32);
            TextBoxCustomerPaymentLimit.Name = "TextBoxCustomerPaymentLimit";
            TextBoxCustomerPaymentLimit.ReadOnly = true;
            TextBoxCustomerPaymentLimit.Size = new Size(131, 21);
            TextBoxCustomerPaymentLimit.TabIndex = 18;
            TextBoxCustomerPaymentLimit.Text = "0.00";
            TextBoxCustomerPaymentLimit.TextAlign = HorizontalAlignment.Right;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(318, 58);
            label1.Name = "label1";
            label1.Size = new Size(89, 13);
            label1.TabIndex = 44;
            label1.Text = "Lock Credit Bill";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(318, 13);
            label2.Name = "label2";
            label2.Size = new Size(72, 13);
            label2.TabIndex = 43;
            label2.Text = "Credit Limit";
            // 
            // BtnCustomerAddNewTerm
            // 
            BtnCustomerAddNewTerm.Location = new Point(164, 76);
            BtnCustomerAddNewTerm.Name = "BtnCustomerAddNewTerm";
            BtnCustomerAddNewTerm.Size = new Size(112, 23);
            BtnCustomerAddNewTerm.TabIndex = 40;
            BtnCustomerAddNewTerm.TabStop = false;
            BtnCustomerAddNewTerm.Text = "Add New Term";
            BtnCustomerAddNewTerm.UseVisualStyleBackColor = true;
            BtnCustomerAddNewTerm.Click += BtnCustomerAddNewTerm_Click;
            // 
            // LabelCustomerPaymentTerm
            // 
            LabelCustomerPaymentTerm.AutoSize = true;
            LabelCustomerPaymentTerm.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelCustomerPaymentTerm.Location = new Point(24, 58);
            LabelCustomerPaymentTerm.Name = "LabelCustomerPaymentTerm";
            LabelCustomerPaymentTerm.Size = new Size(89, 13);
            LabelCustomerPaymentTerm.TabIndex = 39;
            LabelCustomerPaymentTerm.Text = "Payment term";
            // 
            // BtnCustomerAddNewPayment
            // 
            BtnCustomerAddNewPayment.Location = new Point(164, 31);
            BtnCustomerAddNewPayment.Name = "BtnCustomerAddNewPayment";
            BtnCustomerAddNewPayment.Size = new Size(113, 21);
            BtnCustomerAddNewPayment.TabIndex = 38;
            BtnCustomerAddNewPayment.TabStop = false;
            BtnCustomerAddNewPayment.Text = "Add New Payment";
            BtnCustomerAddNewPayment.UseVisualStyleBackColor = true;
            BtnCustomerAddNewPayment.Click += BtnCustomerAddNewPayment_Click;
            // 
            // LabelCustomerPaymentMethod
            // 
            LabelCustomerPaymentMethod.AutoSize = true;
            LabelCustomerPaymentMethod.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelCustomerPaymentMethod.Location = new Point(24, 13);
            LabelCustomerPaymentMethod.Name = "LabelCustomerPaymentMethod";
            LabelCustomerPaymentMethod.Size = new Size(104, 13);
            LabelCustomerPaymentMethod.TabIndex = 37;
            LabelCustomerPaymentMethod.Text = "Payment Method";
            // 
            // TabShippingInfo
            // 
            TabShippingInfo.BackColor = SystemColors.Window;
            TabShippingInfo.Controls.Add(ShippingAddressGroupBoxCustomer);
            TabShippingInfo.Controls.Add(label3);
            TabShippingInfo.Controls.Add(CheckBoxCustomerUseBillingAddress);
            TabShippingInfo.Location = new Point(4, 22);
            TabShippingInfo.Name = "TabShippingInfo";
            TabShippingInfo.Size = new Size(192, 74);
            TabShippingInfo.TabIndex = 2;
            TabShippingInfo.Text = "Shipping Details";
            // 
            // ShippingAddressGroupBoxCustomer
            // 
            ShippingAddressGroupBoxCustomer.AddressLine1 = "";
            ShippingAddressGroupBoxCustomer.AddressLine2 = "";
            ShippingAddressGroupBoxCustomer.CityName = "";
            ShippingAddressGroupBoxCustomer.CountryId = 0L;
            ShippingAddressGroupBoxCustomer.DistrictName = "";
            ShippingAddressGroupBoxCustomer.GroupName = "";
            ShippingAddressGroupBoxCustomer.Location = new Point(18, 51);
            ShippingAddressGroupBoxCustomer.Name = "ShippingAddressGroupBoxCustomer";
            ShippingAddressGroupBoxCustomer.PinCode = "";
            ShippingAddressGroupBoxCustomer.ReadOnly = true;
            ShippingAddressGroupBoxCustomer.Size = new Size(536, 220);
            ShippingAddressGroupBoxCustomer.StateId = 0L;
            ShippingAddressGroupBoxCustomer.StateName = "";
            ShippingAddressGroupBoxCustomer.StateSelectedIndex = -1;
            ShippingAddressGroupBoxCustomer.TabIndex = 22;
            ShippingAddressGroupBoxCustomer.PreviewKeyDown += ShippingAddressGroupBoxCustomer_PreviewKeyDown;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(22, 39);
            label3.Name = "label3";
            label3.Size = new Size(46, 13);
            label3.TabIndex = 23;
            label3.Text = "Address";
            // 
            // CheckBoxCustomerUseBillingAddress
            // 
            CheckBoxCustomerUseBillingAddress.AutoSize = true;
            CheckBoxCustomerUseBillingAddress.Location = new Point(25, 17);
            CheckBoxCustomerUseBillingAddress.Name = "CheckBoxCustomerUseBillingAddress";
            CheckBoxCustomerUseBillingAddress.Size = new Size(115, 17);
            CheckBoxCustomerUseBillingAddress.TabIndex = 21;
            CheckBoxCustomerUseBillingAddress.Text = "Use Billing Address";
            CheckBoxCustomerUseBillingAddress.UseVisualStyleBackColor = true;
            CheckBoxCustomerUseBillingAddress.CheckedChanged += CheckBoxCustomerUseBillingAddress_CheckedChanged;
            CheckBoxCustomerUseBillingAddress.PreviewKeyDown += CheckBoxCustomerUseBillingAddress_PreviewKeyDown;
            // 
            // TabContactInfo
            // 
            TabContactInfo.BackColor = SystemColors.Window;
            TabContactInfo.Controls.Add(TextBoxCustomerMobile);
            TabContactInfo.Controls.Add(TextBoxCustomerPhone);
            TabContactInfo.Controls.Add(TextBoxCustomerFax);
            TabContactInfo.Controls.Add(TextBoxCustomerWebsite);
            TabContactInfo.Controls.Add(TextBoxCustomerEmail);
            TabContactInfo.Controls.Add(LabelCustomerWebsite);
            TabContactInfo.Controls.Add(LabelCustomerEmail);
            TabContactInfo.Controls.Add(LabelCustomerFax);
            TabContactInfo.Controls.Add(LabelCustomerMobile);
            TabContactInfo.Controls.Add(LabelCustomerPhone);
            TabContactInfo.Location = new Point(4, 22);
            TabContactInfo.Name = "TabContactInfo";
            TabContactInfo.Size = new Size(192, 74);
            TabContactInfo.TabIndex = 3;
            TabContactInfo.Text = "Contact Details";
            // 
            // TextBoxCustomerMobile
            // 
            TextBoxCustomerMobile.AreaCodeLength = 5;
            TextBoxCustomerMobile.BackColor = SystemColors.Window;
            TextBoxCustomerMobile.Length = 13;
            TextBoxCustomerMobile.Location = new Point(19, 73);
            TextBoxCustomerMobile.Mask = "#####-#######";
            TextBoxCustomerMobile.Name = "TextBoxCustomerMobile";
            TextBoxCustomerMobile.ReadOnly = true;
            TextBoxCustomerMobile.Size = new Size(100, 21);
            TextBoxCustomerMobile.TabIndex = 24;
            // 
            // TextBoxCustomerPhone
            // 
            TextBoxCustomerPhone.AreaCodeLength = 5;
            TextBoxCustomerPhone.BackColor = SystemColors.Window;
            TextBoxCustomerPhone.Length = 13;
            TextBoxCustomerPhone.Location = new Point(19, 33);
            TextBoxCustomerPhone.Mask = "99999-9999999";
            TextBoxCustomerPhone.Name = "TextBoxCustomerPhone";
            TextBoxCustomerPhone.ReadOnly = true;
            TextBoxCustomerPhone.Size = new Size(100, 21);
            TextBoxCustomerPhone.TabIndex = 23;
            TextBoxCustomerPhone.PreviewKeyDown += TextBoxCustomerPhone_PreviewKeyDown;
            // 
            // TextBoxCustomerFax
            // 
            TextBoxCustomerFax.BackColor = SystemColors.Window;
            TextBoxCustomerFax.Location = new Point(19, 115);
            TextBoxCustomerFax.Mask = "99999999999999999999";
            TextBoxCustomerFax.Name = "TextBoxCustomerFax";
            TextBoxCustomerFax.ReadOnly = true;
            TextBoxCustomerFax.Size = new Size(205, 21);
            TextBoxCustomerFax.TabIndex = 25;
            // 
            // TextBoxCustomerWebsite
            // 
            TextBoxCustomerWebsite.BackColor = SystemColors.Window;
            TextBoxCustomerWebsite.Location = new Point(19, 200);
            TextBoxCustomerWebsite.MaxLength = 50;
            TextBoxCustomerWebsite.Name = "TextBoxCustomerWebsite";
            TextBoxCustomerWebsite.ReadOnly = true;
            TextBoxCustomerWebsite.Size = new Size(205, 21);
            TextBoxCustomerWebsite.TabIndex = 27;
            TextBoxCustomerWebsite.KeyDown += TextBoxCustomerWebsite_KeyDown;
            TextBoxCustomerWebsite.KeyPress += TextBoxCustomerWebsite_KeyPress;
            TextBoxCustomerWebsite.MouseDown += TextBoxCustomerWebsite_MouseDown;
            TextBoxCustomerWebsite.PreviewKeyDown += TextBoxCustomerWebsite_PreviewKeyDown;
            // 
            // TextBoxCustomerEmail
            // 
            TextBoxCustomerEmail.BackColor = SystemColors.Window;
            TextBoxCustomerEmail.Location = new Point(19, 157);
            TextBoxCustomerEmail.MaxLength = 50;
            TextBoxCustomerEmail.Name = "TextBoxCustomerEmail";
            TextBoxCustomerEmail.ReadOnly = true;
            TextBoxCustomerEmail.Size = new Size(205, 21);
            TextBoxCustomerEmail.TabIndex = 26;
            TextBoxCustomerEmail.KeyDown += TextBoxCustomerEmail_KeyDown;
            TextBoxCustomerEmail.KeyPress += TextBoxCustomerEmail_KeyPress;
            TextBoxCustomerEmail.MouseDown += TextBoxCustomerEmail_MouseDown;
            // 
            // LabelCustomerWebsite
            // 
            LabelCustomerWebsite.AutoSize = true;
            LabelCustomerWebsite.Location = new Point(16, 182);
            LabelCustomerWebsite.Name = "LabelCustomerWebsite";
            LabelCustomerWebsite.Size = new Size(46, 13);
            LabelCustomerWebsite.TabIndex = 4;
            LabelCustomerWebsite.Text = "Website";
            // 
            // LabelCustomerEmail
            // 
            LabelCustomerEmail.AutoSize = true;
            LabelCustomerEmail.Location = new Point(16, 140);
            LabelCustomerEmail.Name = "LabelCustomerEmail";
            LabelCustomerEmail.Size = new Size(31, 13);
            LabelCustomerEmail.TabIndex = 3;
            LabelCustomerEmail.Text = "Email";
            // 
            // LabelCustomerFax
            // 
            LabelCustomerFax.AutoSize = true;
            LabelCustomerFax.Location = new Point(16, 98);
            LabelCustomerFax.Name = "LabelCustomerFax";
            LabelCustomerFax.Size = new Size(25, 13);
            LabelCustomerFax.TabIndex = 2;
            LabelCustomerFax.Text = "Fax";
            // 
            // LabelCustomerMobile
            // 
            LabelCustomerMobile.AutoSize = true;
            LabelCustomerMobile.Location = new Point(16, 56);
            LabelCustomerMobile.Name = "LabelCustomerMobile";
            LabelCustomerMobile.Size = new Size(37, 13);
            LabelCustomerMobile.TabIndex = 1;
            LabelCustomerMobile.Text = "Mobile";
            // 
            // LabelCustomerPhone
            // 
            LabelCustomerPhone.AutoSize = true;
            LabelCustomerPhone.Location = new Point(16, 16);
            LabelCustomerPhone.Name = "LabelCustomerPhone";
            LabelCustomerPhone.Size = new Size(37, 13);
            LabelCustomerPhone.TabIndex = 0;
            LabelCustomerPhone.Text = "Phone";
            // 
            // TabLicenseInfo
            // 
            TabLicenseInfo.Controls.Add(CustomerLicenceInfoGrid);
            TabLicenseInfo.Location = new Point(4, 22);
            TabLicenseInfo.Name = "TabLicenseInfo";
            TabLicenseInfo.Padding = new Padding(3);
            TabLicenseInfo.Size = new Size(192, 74);
            TabLicenseInfo.TabIndex = 4;
            TabLicenseInfo.Text = "Tax Info";
            TabLicenseInfo.UseVisualStyleBackColor = true;
            // 
            // CustomerLicenceInfoGrid
            // 
            CustomerLicenceInfoGrid.AllowUserToAddRows = false;
            CustomerLicenceInfoGrid.AllowUserToDeleteRows = false;
            CustomerLicenceInfoGrid.AllowUserToResizeColumns = false;
            CustomerLicenceInfoGrid.AllowUserToResizeRows = false;
            CustomerLicenceInfoGrid.BackgroundColor = SystemColors.Control;
            CustomerLicenceInfoGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            CustomerLicenceInfoGrid.Columns.AddRange(new DataGridViewColumn[] { CustomerTaxIdName, CustomerTaxDisplayName, CustomerTaxIdValue, CustomerTaxIdPrintOnInvoice, CustomerTaxIdPrintOnReport, CustomerTaxIdDelete, CustomerTaxIdId, Column1, Column2 });
            CustomerLicenceInfoGrid.EditMode = DataGridViewEditMode.EditOnEnter;
            CustomerLicenceInfoGrid.EnableHeadersVisualStyles = false;
            CustomerLicenceInfoGrid.Location = new Point(14, 16);
            CustomerLicenceInfoGrid.Name = "CustomerLicenceInfoGrid";
            CustomerLicenceInfoGrid.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.ForeColor = Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = Color.White;
            dataGridViewCellStyle4.SelectionForeColor = Color.Black;
            CustomerLicenceInfoGrid.RowsDefaultCellStyle = dataGridViewCellStyle4;
            CustomerLicenceInfoGrid.RowTemplate.Height = 20;
            CustomerLicenceInfoGrid.ScrollBars = ScrollBars.Vertical;
            CustomerLicenceInfoGrid.ShowCellToolTips = false;
            CustomerLicenceInfoGrid.Size = new Size(671, 309);
            CustomerLicenceInfoGrid.TabIndex = 28;
            CustomerLicenceInfoGrid.CellEnter += CustomerLicenceInfoGrid_CellEnter;
            CustomerLicenceInfoGrid.EditingControlShowing += CustomerLicenceInfoGrid_EditingControlShowing;
            CustomerLicenceInfoGrid.Leave += CustomerLicenceInfoGrid_Leave;
            // 
            // CustomerTaxIdName
            // 
            CustomerTaxIdName.HeaderText = "Name";
            CustomerTaxIdName.Name = "CustomerTaxIdName";
            CustomerTaxIdName.NameLength = 20;
            CustomerTaxIdName.Resizable = DataGridViewTriState.False;
            CustomerTaxIdName.Width = 180;
            // 
            // CustomerTaxDisplayName
            // 
            CustomerTaxDisplayName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CustomerTaxDisplayName.HeaderText = "Display Name";
            CustomerTaxDisplayName.Name = "CustomerTaxDisplayName";
            CustomerTaxDisplayName.NameLength = 20;
            CustomerTaxDisplayName.Resizable = DataGridViewTriState.False;
            // 
            // CustomerTaxIdValue
            // 
            CustomerTaxIdValue.HeaderText = "Value";
            CustomerTaxIdValue.Name = "CustomerTaxIdValue";
            CustomerTaxIdValue.NameLength = 20;
            CustomerTaxIdValue.Resizable = DataGridViewTriState.False;
            // 
            // CustomerTaxIdPrintOnInvoice
            // 
            CustomerTaxIdPrintOnInvoice.HeaderText = "Print On Invoice";
            CustomerTaxIdPrintOnInvoice.Name = "CustomerTaxIdPrintOnInvoice";
            CustomerTaxIdPrintOnInvoice.Resizable = DataGridViewTriState.False;
            CustomerTaxIdPrintOnInvoice.Width = 80;
            // 
            // CustomerTaxIdPrintOnReport
            // 
            CustomerTaxIdPrintOnReport.HeaderText = "Print On Report";
            CustomerTaxIdPrintOnReport.Name = "CustomerTaxIdPrintOnReport";
            CustomerTaxIdPrintOnReport.Resizable = DataGridViewTriState.False;
            CustomerTaxIdPrintOnReport.Width = 80;
            // 
            // CustomerTaxIdDelete
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = "X";
            CustomerTaxIdDelete.DefaultCellStyle = dataGridViewCellStyle3;
            CustomerTaxIdDelete.HeaderText = "...";
            CustomerTaxIdDelete.Name = "CustomerTaxIdDelete";
            CustomerTaxIdDelete.Resizable = DataGridViewTriState.False;
            CustomerTaxIdDelete.Visible = false;
            CustomerTaxIdDelete.Width = 25;
            // 
            // CustomerTaxIdId
            // 
            CustomerTaxIdId.HeaderText = "Id";
            CustomerTaxIdId.Name = "CustomerTaxIdId";
            CustomerTaxIdId.Resizable = DataGridViewTriState.False;
            CustomerTaxIdId.SortMode = DataGridViewColumnSortMode.NotSortable;
            CustomerTaxIdId.Visible = false;
            // 
            // Column1
            // 
            Column1.HeaderText = "MasterId";
            Column1.Name = "Column1";
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Visible = false;
            // 
            // Column2
            // 
            Column2.HeaderText = "Requir";
            Column2.Name = "Column2";
            Column2.Resizable = DataGridViewTriState.False;
            Column2.Visible = false;
            // 
            // BtnCustomerSave
            // 
            BtnCustomerSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCustomerSave.Location = new Point(789, 393);
            BtnCustomerSave.Name = "BtnCustomerSave";
            BtnCustomerSave.Size = new Size(83, 23);
            BtnCustomerSave.TabIndex = 29;
            BtnCustomerSave.Text = "Save [F8]";
            BtnCustomerSave.UseVisualStyleBackColor = true;
            BtnCustomerSave.Click += BtnCustomerSave_Click;
            BtnCustomerSave.PreviewKeyDown += BtnCustomerSave_PreviewKeyDown;
            // 
            // BtnCustomerCancel
            // 
            BtnCustomerCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCustomerCancel.Location = new Point(700, 393);
            BtnCustomerCancel.Name = "BtnCustomerCancel";
            BtnCustomerCancel.Size = new Size(83, 23);
            BtnCustomerCancel.TabIndex = 30;
            BtnCustomerCancel.Text = "Cancel [Esc]";
            BtnCustomerCancel.UseVisualStyleBackColor = true;
            BtnCustomerCancel.Click += BtnCustomerCancel_Click;
            // 
            // BtnCustomerEdit
            // 
            BtnCustomerEdit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCustomerEdit.Location = new Point(192, 393);
            BtnCustomerEdit.Name = "BtnCustomerEdit";
            BtnCustomerEdit.Size = new Size(83, 23);
            BtnCustomerEdit.TabIndex = 4;
            BtnCustomerEdit.Text = "Edit [F7]";
            BtnCustomerEdit.UseVisualStyleBackColor = true;
            BtnCustomerEdit.Click += BtnCustomerEdit_Click;
            // 
            // BtnCustomerNew
            // 
            BtnCustomerNew.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCustomerNew.Location = new Point(14, 393);
            BtnCustomerNew.Name = "BtnCustomerNew";
            BtnCustomerNew.Size = new Size(83, 23);
            BtnCustomerNew.TabIndex = 2;
            BtnCustomerNew.Text = "New [F3]";
            BtnCustomerNew.UseVisualStyleBackColor = true;
            BtnCustomerNew.Click += BtnCustomerNew_Click;
            // 
            // BtnCustomerDelete
            // 
            BtnCustomerDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCustomerDelete.Location = new Point(103, 393);
            BtnCustomerDelete.Name = "BtnCustomerDelete";
            BtnCustomerDelete.Size = new Size(83, 23);
            BtnCustomerDelete.TabIndex = 3;
            BtnCustomerDelete.Text = "Delete [F4]";
            BtnCustomerDelete.UseVisualStyleBackColor = true;
            BtnCustomerDelete.Click += BtnCustomerDelete_Click;
            // 
            // TreeViewCustomer
            // 
            TreeViewCustomer.Enabled = false;
            TreeViewCustomer.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TreeViewCustomer.HideSelection = false;
            TreeViewCustomer.Location = new Point(12, 39);
            TreeViewCustomer.Name = "TreeViewCustomer";
            TreeViewCustomer.Size = new Size(239, 339);
            TreeViewCustomer.TabIndex = 1;
            TreeViewCustomer.AfterSelect += TreeViewCustomer_AfterSelect;
            // 
            // ImageListCustomer
            // 
            ImageListCustomer.ColorDepth = ColorDepth.Depth8Bit;
            ImageListCustomer.ImageStream = (ImageListStreamer)resources.GetObject("ImageListCustomer.ImageStream");
            ImageListCustomer.TransparentColor = Color.Transparent;
            ImageListCustomer.Images.SetKeyName(0, "customers.ico");
            // 
            // TextBoxCustomerAccountId
            // 
            TextBoxCustomerAccountId.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxCustomerAccountId.Location = new Point(423, 395);
            TextBoxCustomerAccountId.Name = "TextBoxCustomerAccountId";
            TextBoxCustomerAccountId.Size = new Size(100, 21);
            TextBoxCustomerAccountId.TabIndex = 21;
            TextBoxCustomerAccountId.Visible = false;
            // 
            // BtnCurrencyExit
            // 
            BtnCurrencyExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCurrencyExit.Location = new Point(878, 393);
            BtnCurrencyExit.Name = "BtnCurrencyExit";
            BtnCurrencyExit.Size = new Size(83, 23);
            BtnCurrencyExit.TabIndex = 31;
            BtnCurrencyExit.Text = "Exit [F10]";
            BtnCurrencyExit.UseVisualStyleBackColor = true;
            BtnCurrencyExit.Click += BtnCurrencyExit_Click;
            // 
            // toolTip1
            // 
            toolTip1.AutoPopDelay = 5000;
            toolTip1.BackColor = Color.LemonChiffon;
            toolTip1.InitialDelay = 500;
            toolTip1.OwnerDraw = true;
            toolTip1.ReshowDelay = 50;
            toolTip1.ToolTipIcon = ToolTipIcon.Info;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ToolStripStatusLabelErrorCustomer });
            statusStrip1.Location = new Point(0, 432);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(975, 22);
            statusStrip1.TabIndex = 56;
            statusStrip1.Text = "sdfdsf sdf sdf";
            // 
            // ToolStripStatusLabelErrorCustomer
            // 
            ToolStripStatusLabelErrorCustomer.Name = "ToolStripStatusLabelErrorCustomer";
            ToolStripStatusLabelErrorCustomer.Size = new Size(151, 17);
            ToolStripStatusLabelErrorCustomer.Text = "                                                ";
            // 
            // TextBoxCustomerSearch
            // 
            TextBoxCustomerSearch.BackColor = SystemColors.Window;
            TextBoxCustomerSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxCustomerSearch.Delay = true;
            TextBoxCustomerSearch.DelayTime = 1000;
            TextBoxCustomerSearch.Location = new Point(12, 12);
            TextBoxCustomerSearch.MaxLength = 35;
            TextBoxCustomerSearch.Name = "TextBoxCustomerSearch";
            TextBoxCustomerSearch.Searchstartfrom = 2;
            TextBoxCustomerSearch.Size = new Size(239, 21);
            TextBoxCustomerSearch.TabIndex = 0;
            TextBoxCustomerSearch.TextChanged += TextBoxCustomerSearch_TextChanged;
            TextBoxCustomerSearch.KeyDown += TextBoxCustomerSearch_KeyDown;
            // 
            // FormCustomers
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(975, 454);
            Controls.Add(TabControlCustomer);
            Controls.Add(statusStrip1);
            Controls.Add(BtnCurrencyExit);
            Controls.Add(TextBoxCustomerAccountId);
            Controls.Add(TreeViewCustomer);
            Controls.Add(BtnCustomerDelete);
            Controls.Add(BtnCustomerSave);
            Controls.Add(BtnCustomerCancel);
            Controls.Add(BtnCustomerNew);
            Controls.Add(BtnCustomerEdit);
            Controls.Add(TextBoxCustomerSearch);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormCustomers";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Customers";
            FormClosing += FormCustomers_FormClosing;
            Load += FormCustomers_Load;
            Controls.SetChildIndex(checkBoxIsPatient, 0);
            Controls.SetChildIndex(TextBoxCustomerSearch, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(BtnCustomerEdit, 0);
            Controls.SetChildIndex(BtnCustomerNew, 0);
            Controls.SetChildIndex(BtnCustomerCancel, 0);
            Controls.SetChildIndex(BtnCustomerSave, 0);
            Controls.SetChildIndex(BtnCustomerDelete, 0);
            Controls.SetChildIndex(TreeViewCustomer, 0);
            Controls.SetChildIndex(TextBoxCustomerAccountId, 0);
            Controls.SetChildIndex(BtnCurrencyExit, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(TabControlCustomer, 0);
            TabControlCustomer.ResumeLayout(false);
            TabCustomer.ResumeLayout(false);
            TabCustomer.PerformLayout();
            TabBillingInfo.ResumeLayout(false);
            TabBillingInfo.PerformLayout();
            TabShippingInfo.ResumeLayout(false);
            TabShippingInfo.PerformLayout();
            TabContactInfo.ResumeLayout(false);
            TabContactInfo.PerformLayout();
            TabLicenseInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)CustomerLicenceInfoGrid).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TabControl TabControlCustomer;
        private TabPage TabCustomer;
        private TabPage TabBillingInfo;
        private Button BtnCustomerNew;
        private Button BtnCustomerDelete;
        private Button BtnCustomerEdit;
        private Button BtnCustomerCancel;
        private Button BtnCustomerSave;
        private Label LabelCustomerName;
        private Label LabelCustomerDisplayAs;
        private TextBox TextBoxCustomerName;
        private TextBox TextBoxCustomerDisplayAs;
        private Label LabelCustomerDescription;
        private TextBox TextBoxCustomerDescription;
        private Label LabelCustomerBranch;
        private CheckBox CheckBoxCustomerIsbranch;
        private Label LabelCustomerParent;
        private TabPage TabShippingInfo;
        private CheckBox CheckBoxCustomerUseBillingAddress;
        private Label LabelCustomerNameonCheck;
        private TextBox TextBoxCustomerNameonCheck;
        private CheckBox CheckBoxUseDisplayName;
        private TabPage TabContactInfo;
        private Label LabelCustomerPhone;
        private Label LabelCustomerMobile;
        private Label LabelCustomerFax;
        private Label LabelCustomerEmail;
        private Label LabelCustomerWebsite;
        private TextBox TextBoxCustomerWebsite;
        private TextBox TextBoxCustomerEmail;
        private TreeView TreeViewCustomer;
        private MaskedTextBox TextBoxCustomerAccountId;
        private Button BtnCurrencyExit;
        private ToolTip toolTip1;
        private MaskedTextBox TextBoxCustomerFax;
        private ImageList ImageListCustomer;
        private Button BtnCustomerAddNewTerm;
        private Label LabelCustomerPaymentTerm;
        private Button BtnCustomerAddNewPayment;
        private Label LabelCustomerPaymentMethod;
        private Label LabelCustomerAsof;
        private Label LabelCustomerBalance;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel ToolStripStatusLabelErrorCustomer;
        private controls.text.DateWithCalendar DateTimePickerCustomer;
        private controls.ComboBoxSwapTextBox ComboBoxCustomerPaymentTerm;
        private controls.ComboBoxSwapTextBox ComboBoxCustomerPaymentMethod;
        private controls.ComboBoxSwapTextBox ComboBoxCustomerParentAccount;
        private controls.text.CurrencyTextBox TextBoxCustomerBalance;
        private controls.text.PhoneTextBox TextBoxCustomerMobile;
        private controls.text.PhoneTextBox TextBoxCustomerPhone;
        private TabPage TabLicenseInfo;
        private Label label1;
        private Label label2;
        private controls.text.CurrencyTextBox TextBoxCustomerPaymentLimit;
        private controls.YesNoRadio RadioCustomerLockBill;
        private controls.ComboBoxSwapTextBox ComboBoxBalanceType;
        private controls.DataViewVerticalScroll CustomerLicenceInfoGrid;
        private controls.grid.DataGridViewNameColumn CustomerTaxIdName;
        private controls.grid.DataGridViewNameColumn CustomerTaxDisplayName;
        private controls.grid.DataGridViewNameColumn CustomerTaxIdValue;
        private DataGridViewCheckBoxColumn CustomerTaxIdPrintOnInvoice;
        private DataGridViewCheckBoxColumn CustomerTaxIdPrintOnReport;
        private DataGridViewButtonColumn CustomerTaxIdDelete;
        private DataGridViewTextBoxColumn CustomerTaxIdId;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewCheckBoxColumn Column2;
        private Label label3;
        private Label label4;
        private controls.text.DelayedTextChangeTextBox TextBoxCustomerSearch;
        private controls.AddressGroupBoxWithStateSelection BillingAddressGroupBoxCustomer;
        private controls.AddressGroupBoxWithStateSelection ShippingAddressGroupBoxCustomer;
        private TextBox TextBoxGSTNo;
        private Label labelGSTNo;
    }
}