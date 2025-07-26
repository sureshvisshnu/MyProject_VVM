namespace fa.views.account.masters
{
    partial class FormSupplier
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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSupplier));
            TabControlSupplier = new TabControl();
            TabGeneralPage = new TabPage();
            ComboBoxBalanceType = new fa.views.controls.ComboBoxSwapTextBox();
            ComboBoxSupplierParentAccount = new fa.views.controls.ComboBoxSwapTextBox();
            TextBoxGSTNo = new TextBox();
            labelGSTNo = new Label();
            TextBoxSupplierBalance = new fa.views.controls.text.CurrencyTextBox();
            DateTimePickerSupplier = new fa.views.controls.text.DateWithCalendar();
            LabelSupplierAsof = new Label();
            LabelSupplierBalance = new Label();
            LabelSupplierParent = new Label();
            LabelSupplierBranch = new Label();
            CheckBoxSupplierIsbranch = new CheckBox();
            TextBoxSupplierDescription = new TextBox();
            TextBoxSupplierDisplayAs = new TextBox();
            TextBoxSupplierName = new TextBox();
            LabelSupplierDescription = new Label();
            LabelSupplierDisplayAs = new Label();
            LabelSupplierName = new Label();
            TabContactInfo = new TabPage();
            AddressGroupBoxSupplier = new fa.views.controls.AddressGroupBoxWithStateSelection();
            label4 = new Label();
            TextBoxSupplierMobile = new fa.views.controls.text.PhoneTextBox();
            TextBoxSupplierPhone = new fa.views.controls.text.PhoneTextBox();
            TextBoxSupplierFax = new MaskedTextBox();
            TextBoxSupplierWebsite = new TextBox();
            TextBoxSupplierEmail = new TextBox();
            LabelSupplierWebsite = new Label();
            LabelSupplierEmail = new Label();
            LabelSupplierFax = new Label();
            LabelSupplierMobile = new Label();
            LabelSupplierPhone = new Label();
            TabLicenseInfo = new TabPage();
            SupplierLicenceInfoGrid = new fa.views.controls.DataViewVerticalScroll();
            SupplierTaxIdName = new fa.views.controls.grid.DataGridViewNameColumn();
            SupplierTaxDisplayName = new fa.views.controls.grid.DataGridViewNameColumn();
            SupplierTaxIdValue = new fa.views.controls.grid.DataGridViewNameColumn();
            Column3 = new DataGridViewCheckBoxColumn();
            SupplierTaxIdPrintOnReport = new DataGridViewCheckBoxColumn();
            SupplierTaxIdDelete = new DataGridViewButtonColumn();
            CustomerTaxIdId = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewCheckBoxColumn();
            supplierproduct = new TabPage();
            TextBoxSelectedProductSearch = new fa.views.controls.text.NameTextBoxAllowSpace(components);
            BtnRemoveProduct = new Button();
            BtnSelectProduct = new Button();
            TreeViewSelectedProduct = new TreeView();
            TextBoxProductSearch = new fa.views.controls.text.NameTextBoxAllowSpace(components);
            TreeViewProduct = new TreeView();
            BtnSupplierSave = new Button();
            BtnSupplierCancel = new Button();
            BtnSupplierEdit = new Button();
            BtnSupplierNew = new Button();
            BtnSupplierDelete = new Button();
            TreeViewSupplier = new TreeView();
            ImageListSupplier = new ImageList(components);
            TextBoxSupplierAccountId = new MaskedTextBox();
            BtnCurrencyExit = new Button();
            statusStrip1 = new StatusStrip();
            ToolStripStatusLabelErrorSupplier = new ToolStripStatusLabel();
            CompanyDataGridViewTaxType = new fa.views.controls.DataViewVerticalScroll();
            TextBoxSupplierSearch = new fa.views.controls.text.DelayedTextChangeTextBox();
            TabControlSupplier.SuspendLayout();
            TabGeneralPage.SuspendLayout();
            TabContactInfo.SuspendLayout();
            TabLicenseInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SupplierLicenceInfoGrid).BeginInit();
            supplierproduct.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)CompanyDataGridViewTaxType).BeginInit();
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
            // TabControlSupplier
            // 
            TabControlSupplier.Controls.Add(TabGeneralPage);
            TabControlSupplier.Controls.Add(TabContactInfo);
            TabControlSupplier.Controls.Add(TabLicenseInfo);
            TabControlSupplier.Controls.Add(supplierproduct);
            TabControlSupplier.Location = new Point(258, 12);
            TabControlSupplier.Name = "TabControlSupplier";
            TabControlSupplier.SelectedIndex = 0;
            TabControlSupplier.Size = new Size(707, 441);
            TabControlSupplier.TabIndex = 5;
            TabControlSupplier.TabStop = false;
            // 
            // TabGeneralPage
            // 
            TabGeneralPage.BackColor = SystemColors.Window;
            TabGeneralPage.Controls.Add(ComboBoxSupplierParentAccount);
            TabGeneralPage.Controls.Add(ComboBoxBalanceType);
            TabGeneralPage.Controls.Add(TextBoxGSTNo);
            TabGeneralPage.Controls.Add(labelGSTNo);
            TabGeneralPage.Controls.Add(TextBoxSupplierBalance);
            TabGeneralPage.Controls.Add(DateTimePickerSupplier);
            TabGeneralPage.Controls.Add(LabelSupplierAsof);
            TabGeneralPage.Controls.Add(LabelSupplierBalance);
            TabGeneralPage.Controls.Add(LabelSupplierParent);
            TabGeneralPage.Controls.Add(LabelSupplierBranch);
            TabGeneralPage.Controls.Add(CheckBoxSupplierIsbranch);
            TabGeneralPage.Controls.Add(TextBoxSupplierDescription);
            TabGeneralPage.Controls.Add(TextBoxSupplierDisplayAs);
            TabGeneralPage.Controls.Add(TextBoxSupplierName);
            TabGeneralPage.Controls.Add(LabelSupplierDescription);
            TabGeneralPage.Controls.Add(LabelSupplierDisplayAs);
            TabGeneralPage.Controls.Add(LabelSupplierName);
            TabGeneralPage.Location = new Point(4, 22);
            TabGeneralPage.Name = "TabGeneralPage";
            TabGeneralPage.Padding = new Padding(3);
            TabGeneralPage.Size = new Size(699, 415);
            TabGeneralPage.TabIndex = 0;
            TabGeneralPage.Text = "Supplier";
            // 
            // ComboBoxBalanceType
            // 
            ComboBoxBalanceType.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxBalanceType.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxBalanceType.FormattingEnabled = true;
            ComboBoxBalanceType.Items.AddRange(new object[] { "DR", "CR" });
            ComboBoxBalanceType.Location = new Point(122, 265);
            ComboBoxBalanceType.Name = "ComboBoxBalanceType";
            ComboBoxBalanceType.Size = new Size(40, 21);
            ComboBoxBalanceType.TabIndex = 13;
            ComboBoxBalanceType.TxtVisible = true;
            // 
            // ComboBoxSupplierParentAccount
            // 
            ComboBoxSupplierParentAccount.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxSupplierParentAccount.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxSupplierParentAccount.FormattingEnabled = true;
            ComboBoxSupplierParentAccount.Location = new Point(19, 222);
            ComboBoxSupplierParentAccount.Name = "ComboBoxSupplierParentAccount";
            ComboBoxSupplierParentAccount.Size = new Size(356, 21);
            ComboBoxSupplierParentAccount.TabIndex = 11;
            ComboBoxSupplierParentAccount.TxtVisible = true;
            ComboBoxSupplierParentAccount.KeyPress += ComboBoxSupplierParentAccount_KeyPress;
            // 
            // TextBoxGSTNo
            // 
            TextBoxGSTNo.BackColor = SystemColors.Window;
            TextBoxGSTNo.Location = new Point(19, 312);
            TextBoxGSTNo.MaxLength = 100;
            TextBoxGSTNo.Name = "TextBoxGSTNo";
            TextBoxGSTNo.ReadOnly = true;
            TextBoxGSTNo.Size = new Size(356, 21);
            TextBoxGSTNo.TabIndex = 15;
            // 
            // labelGSTNo
            // 
            labelGSTNo.AutoSize = true;
            labelGSTNo.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            labelGSTNo.Location = new Point(19, 296);
            labelGSTNo.Name = "labelGSTNo";
            labelGSTNo.Size = new Size(42, 13);
            labelGSTNo.TabIndex = 26;
            labelGSTNo.Text = "GST No";
            // 
            // TextBoxSupplierBalance
            // 
            TextBoxSupplierBalance.BackColor = SystemColors.Window;
            TextBoxSupplierBalance.Decimals = 2;
            TextBoxSupplierBalance.Length = 10;
            TextBoxSupplierBalance.Location = new Point(19, 265);
            TextBoxSupplierBalance.Name = "TextBoxSupplierBalance";
            TextBoxSupplierBalance.ReadOnly = true;
            TextBoxSupplierBalance.Size = new Size(100, 21);
            TextBoxSupplierBalance.TabIndex = 12;
            TextBoxSupplierBalance.Text = "0.00";
            TextBoxSupplierBalance.TextAlign = HorizontalAlignment.Right;
            // 
            // DateTimePickerSupplier
            // 
            DateTimePickerSupplier.BackColor = Color.White;
            DateTimePickerSupplier.BorderStyle = BorderStyle.FixedSingle;
            DateTimePickerSupplier.Date = null;
            DateTimePickerSupplier.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DateTimePickerSupplier.Format = "MM/dd/yyyy";
            DateTimePickerSupplier.Location = new Point(199, 265);
            DateTimePickerSupplier.MaxDate = new DateTime(9997, 12, 31, 7, 44, 56, 0);
            DateTimePickerSupplier.MinDate = new DateTime(1900, 1, 1, 21, 36, 19, 0);
            DateTimePickerSupplier.Name = "DateTimePickerSupplier";
            DateTimePickerSupplier.ReadOnly = false;
            DateTimePickerSupplier.Size = new Size(93, 21);
            DateTimePickerSupplier.TabIndex = 14;
            DateTimePickerSupplier.PreviewKeyDown += DateTimePickerSupplier_PreviewKeyDown;
            // 
            // LabelSupplierAsof
            // 
            LabelSupplierAsof.AutoSize = true;
            LabelSupplierAsof.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelSupplierAsof.Location = new Point(196, 249);
            LabelSupplierAsof.Name = "LabelSupplierAsof";
            LabelSupplierAsof.Size = new Size(38, 13);
            LabelSupplierAsof.TabIndex = 25;
            LabelSupplierAsof.Text = "As of ";
            // 
            // LabelSupplierBalance
            // 
            LabelSupplierBalance.AutoSize = true;
            LabelSupplierBalance.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelSupplierBalance.Location = new Point(19, 248);
            LabelSupplierBalance.Name = "LabelSupplierBalance";
            LabelSupplierBalance.Size = new Size(44, 13);
            LabelSupplierBalance.TabIndex = 24;
            LabelSupplierBalance.Text = "Balance";
            // 
            // LabelSupplierParent
            // 
            LabelSupplierParent.AutoSize = true;
            LabelSupplierParent.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelSupplierParent.Location = new Point(19, 204);
            LabelSupplierParent.Name = "LabelSupplierParent";
            LabelSupplierParent.Size = new Size(39, 13);
            LabelSupplierParent.TabIndex = 22;
            LabelSupplierParent.Text = "Parent";
            // 
            // LabelSupplierBranch
            // 
            LabelSupplierBranch.AutoSize = true;
            LabelSupplierBranch.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelSupplierBranch.Location = new Point(19, 165);
            LabelSupplierBranch.Name = "LabelSupplierBranch";
            LabelSupplierBranch.Size = new Size(40, 13);
            LabelSupplierBranch.TabIndex = 21;
            LabelSupplierBranch.Text = "Branch";
            // 
            // CheckBoxSupplierIsbranch
            // 
            CheckBoxSupplierIsbranch.AutoSize = true;
            CheckBoxSupplierIsbranch.Enabled = false;
            CheckBoxSupplierIsbranch.Location = new Point(19, 184);
            CheckBoxSupplierIsbranch.Name = "CheckBoxSupplierIsbranch";
            CheckBoxSupplierIsbranch.Size = new Size(43, 17);
            CheckBoxSupplierIsbranch.TabIndex = 10;
            CheckBoxSupplierIsbranch.Text = "Yes";
            CheckBoxSupplierIsbranch.UseVisualStyleBackColor = true;
            CheckBoxSupplierIsbranch.CheckedChanged += CheckBoxSupplierIsbranch_CheckedChanged;
            // 
            // TextBoxSupplierDescription
            // 
            TextBoxSupplierDescription.BackColor = SystemColors.Window;
            TextBoxSupplierDescription.Location = new Point(19, 112);
            TextBoxSupplierDescription.MaxLength = 250;
            TextBoxSupplierDescription.Multiline = true;
            TextBoxSupplierDescription.Name = "TextBoxSupplierDescription";
            TextBoxSupplierDescription.ReadOnly = true;
            TextBoxSupplierDescription.Size = new Size(475, 50);
            TextBoxSupplierDescription.TabIndex = 9;
            // 
            // TextBoxSupplierDisplayAs
            // 
            TextBoxSupplierDisplayAs.BackColor = SystemColors.Window;
            TextBoxSupplierDisplayAs.Location = new Point(19, 72);
            TextBoxSupplierDisplayAs.MaxLength = 100;
            TextBoxSupplierDisplayAs.Name = "TextBoxSupplierDisplayAs";
            TextBoxSupplierDisplayAs.ReadOnly = true;
            TextBoxSupplierDisplayAs.Size = new Size(475, 21);
            TextBoxSupplierDisplayAs.TabIndex = 8;
            TextBoxSupplierDisplayAs.KeyDown += TextBoxSupplierName_KeyDown;
            TextBoxSupplierDisplayAs.KeyPress += TextBoxSupplierDisplayAs_KeyPress;
            TextBoxSupplierDisplayAs.MouseDown += TextBoxSupplierDisplayAs_MouseDown;
            // 
            // TextBoxSupplierName
            // 
            TextBoxSupplierName.BackColor = SystemColors.Window;
            TextBoxSupplierName.Location = new Point(19, 32);
            TextBoxSupplierName.MaxLength = 30;
            TextBoxSupplierName.Name = "TextBoxSupplierName";
            TextBoxSupplierName.ReadOnly = true;
            TextBoxSupplierName.Size = new Size(475, 21);
            TextBoxSupplierName.TabIndex = 7;
            TextBoxSupplierName.KeyDown += TextBoxSupplierName_KeyDown;
            TextBoxSupplierName.KeyPress += TextBoxSupplierName_KeyPress;
            TextBoxSupplierName.MouseDown += TextBoxSupplierName_MouseDown;
            TextBoxSupplierName.PreviewKeyDown += TextBoxSupplierName_PreviewKeyDown;
            // 
            // LabelSupplierDescription
            // 
            LabelSupplierDescription.AutoSize = true;
            LabelSupplierDescription.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelSupplierDescription.Location = new Point(19, 96);
            LabelSupplierDescription.Name = "LabelSupplierDescription";
            LabelSupplierDescription.Size = new Size(60, 13);
            LabelSupplierDescription.TabIndex = 2;
            LabelSupplierDescription.Text = "Description";
            // 
            // LabelSupplierDisplayAs
            // 
            LabelSupplierDisplayAs.AutoSize = true;
            LabelSupplierDisplayAs.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelSupplierDisplayAs.Location = new Point(19, 56);
            LabelSupplierDisplayAs.Name = "LabelSupplierDisplayAs";
            LabelSupplierDisplayAs.Size = new Size(56, 13);
            LabelSupplierDisplayAs.TabIndex = 1;
            LabelSupplierDisplayAs.Text = "Display As";
            // 
            // LabelSupplierName
            // 
            LabelSupplierName.AutoSize = true;
            LabelSupplierName.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelSupplierName.Location = new Point(19, 16);
            LabelSupplierName.Name = "LabelSupplierName";
            LabelSupplierName.Size = new Size(39, 13);
            LabelSupplierName.TabIndex = 0;
            LabelSupplierName.Text = "Name";
            // 
            // TabContactInfo
            // 
            TabContactInfo.BackColor = SystemColors.Window;
            TabContactInfo.Controls.Add(AddressGroupBoxSupplier);
            TabContactInfo.Controls.Add(label4);
            TabContactInfo.Controls.Add(TextBoxSupplierMobile);
            TabContactInfo.Controls.Add(TextBoxSupplierPhone);
            TabContactInfo.Controls.Add(TextBoxSupplierFax);
            TabContactInfo.Controls.Add(TextBoxSupplierWebsite);
            TabContactInfo.Controls.Add(TextBoxSupplierEmail);
            TabContactInfo.Controls.Add(LabelSupplierWebsite);
            TabContactInfo.Controls.Add(LabelSupplierEmail);
            TabContactInfo.Controls.Add(LabelSupplierFax);
            TabContactInfo.Controls.Add(LabelSupplierMobile);
            TabContactInfo.Controls.Add(LabelSupplierPhone);
            TabContactInfo.ForeColor = SystemColors.WindowText;
            TabContactInfo.Location = new Point(4, 24);
            TabContactInfo.Name = "TabContactInfo";
            TabContactInfo.Padding = new Padding(3);
            TabContactInfo.Size = new Size(699, 413);
            TabContactInfo.TabIndex = 1;
            TabContactInfo.Text = "Contact Info";
            // 
            // AddressGroupBoxSupplier
            // 
            AddressGroupBoxSupplier.AddressLine1 = "";
            AddressGroupBoxSupplier.AddressLine2 = "";
            AddressGroupBoxSupplier.CityName = "";
            AddressGroupBoxSupplier.CountryId = 0L;
            AddressGroupBoxSupplier.DistrictName = "";
            AddressGroupBoxSupplier.GroupName = "";
            AddressGroupBoxSupplier.Location = new Point(15, 219);
            AddressGroupBoxSupplier.Name = "AddressGroupBoxSupplier";
            AddressGroupBoxSupplier.PinCode = "";
            AddressGroupBoxSupplier.ReadOnly = true;
            AddressGroupBoxSupplier.Size = new Size(474, 190);
            AddressGroupBoxSupplier.StateId = 0L;
            AddressGroupBoxSupplier.StateName = "";
            AddressGroupBoxSupplier.StateSelectedIndex = -1;
            AddressGroupBoxSupplier.TabIndex = 20;
            AddressGroupBoxSupplier.PreviewKeyDown += AddressGroupBoxSupplier_PreviewKeyDown;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 208);
            label4.Name = "label4";
            label4.Size = new Size(46, 13);
            label4.TabIndex = 46;
            label4.Text = "Address";
            // 
            // TextBoxSupplierMobile
            // 
            TextBoxSupplierMobile.AreaCodeLength = 5;
            TextBoxSupplierMobile.BackColor = SystemColors.Window;
            TextBoxSupplierMobile.Length = 13;
            TextBoxSupplierMobile.Location = new Point(19, 62);
            TextBoxSupplierMobile.Mask = "99999-9999999";
            TextBoxSupplierMobile.Name = "TextBoxSupplierMobile";
            TextBoxSupplierMobile.ReadOnly = true;
            TextBoxSupplierMobile.Size = new Size(100, 21);
            TextBoxSupplierMobile.TabIndex = 16;
            // 
            // TextBoxSupplierPhone
            // 
            TextBoxSupplierPhone.AreaCodeLength = 5;
            TextBoxSupplierPhone.BackColor = SystemColors.Window;
            TextBoxSupplierPhone.Length = 13;
            TextBoxSupplierPhone.Location = new Point(19, 22);
            TextBoxSupplierPhone.Mask = "99999-9999999";
            TextBoxSupplierPhone.Name = "TextBoxSupplierPhone";
            TextBoxSupplierPhone.ReadOnly = true;
            TextBoxSupplierPhone.Size = new Size(100, 21);
            TextBoxSupplierPhone.TabIndex = 15;
            TextBoxSupplierPhone.PreviewKeyDown += TextBoxSupplierPhone_PreviewKeyDown;
            // 
            // TextBoxSupplierFax
            // 
            TextBoxSupplierFax.BackColor = SystemColors.Window;
            TextBoxSupplierFax.Location = new Point(19, 103);
            TextBoxSupplierFax.Mask = "99999999999999999999";
            TextBoxSupplierFax.Name = "TextBoxSupplierFax";
            TextBoxSupplierFax.ReadOnly = true;
            TextBoxSupplierFax.Size = new Size(205, 21);
            TextBoxSupplierFax.TabIndex = 17;
            // 
            // TextBoxSupplierWebsite
            // 
            TextBoxSupplierWebsite.BackColor = SystemColors.Window;
            TextBoxSupplierWebsite.Location = new Point(19, 183);
            TextBoxSupplierWebsite.MaxLength = 50;
            TextBoxSupplierWebsite.Name = "TextBoxSupplierWebsite";
            TextBoxSupplierWebsite.ReadOnly = true;
            TextBoxSupplierWebsite.Size = new Size(205, 21);
            TextBoxSupplierWebsite.TabIndex = 19;
            TextBoxSupplierWebsite.KeyDown += TextBoxSupplierWebsite_KeyDown;
            TextBoxSupplierWebsite.KeyPress += TextBoxSupplierWebsite_KeyPress;
            TextBoxSupplierWebsite.MouseDown += TextBoxSupplierWebsite_MouseDown;
            // 
            // TextBoxSupplierEmail
            // 
            TextBoxSupplierEmail.BackColor = SystemColors.Window;
            TextBoxSupplierEmail.Location = new Point(19, 143);
            TextBoxSupplierEmail.MaxLength = 50;
            TextBoxSupplierEmail.Name = "TextBoxSupplierEmail";
            TextBoxSupplierEmail.ReadOnly = true;
            TextBoxSupplierEmail.Size = new Size(205, 21);
            TextBoxSupplierEmail.TabIndex = 18;
            TextBoxSupplierEmail.KeyDown += TextBoxSupplierEmail_KeyDown;
            TextBoxSupplierEmail.KeyPress += TextBoxSupplierEmail_KeyPress;
            TextBoxSupplierEmail.MouseDown += TextBoxSupplierEmail_MouseDown;
            // 
            // LabelSupplierWebsite
            // 
            LabelSupplierWebsite.AutoSize = true;
            LabelSupplierWebsite.Location = new Point(16, 167);
            LabelSupplierWebsite.Name = "LabelSupplierWebsite";
            LabelSupplierWebsite.Size = new Size(46, 13);
            LabelSupplierWebsite.TabIndex = 14;
            LabelSupplierWebsite.Text = "Website";
            // 
            // LabelSupplierEmail
            // 
            LabelSupplierEmail.AutoSize = true;
            LabelSupplierEmail.Location = new Point(16, 127);
            LabelSupplierEmail.Name = "LabelSupplierEmail";
            LabelSupplierEmail.Size = new Size(31, 13);
            LabelSupplierEmail.TabIndex = 13;
            LabelSupplierEmail.Text = "Email";
            // 
            // LabelSupplierFax
            // 
            LabelSupplierFax.AutoSize = true;
            LabelSupplierFax.Location = new Point(16, 87);
            LabelSupplierFax.Name = "LabelSupplierFax";
            LabelSupplierFax.Size = new Size(25, 13);
            LabelSupplierFax.TabIndex = 12;
            LabelSupplierFax.Text = "Fax";
            // 
            // LabelSupplierMobile
            // 
            LabelSupplierMobile.AutoSize = true;
            LabelSupplierMobile.Location = new Point(16, 46);
            LabelSupplierMobile.Name = "LabelSupplierMobile";
            LabelSupplierMobile.Size = new Size(37, 13);
            LabelSupplierMobile.TabIndex = 11;
            LabelSupplierMobile.Text = "Mobile";
            // 
            // LabelSupplierPhone
            // 
            LabelSupplierPhone.AutoSize = true;
            LabelSupplierPhone.Location = new Point(16, 6);
            LabelSupplierPhone.Name = "LabelSupplierPhone";
            LabelSupplierPhone.Size = new Size(37, 13);
            LabelSupplierPhone.TabIndex = 10;
            LabelSupplierPhone.Text = "Phone";
            // 
            // TabLicenseInfo
            // 
            TabLicenseInfo.Controls.Add(SupplierLicenceInfoGrid);
            TabLicenseInfo.Location = new Point(4, 24);
            TabLicenseInfo.Name = "TabLicenseInfo";
            TabLicenseInfo.Padding = new Padding(3);
            TabLicenseInfo.Size = new Size(699, 413);
            TabLicenseInfo.TabIndex = 2;
            TabLicenseInfo.Text = "Tax Info";
            TabLicenseInfo.UseVisualStyleBackColor = true;
            // 
            // SupplierLicenceInfoGrid
            // 
            SupplierLicenceInfoGrid.AllowUserToAddRows = false;
            SupplierLicenceInfoGrid.AllowUserToDeleteRows = false;
            SupplierLicenceInfoGrid.AllowUserToResizeColumns = false;
            SupplierLicenceInfoGrid.AllowUserToResizeRows = false;
            SupplierLicenceInfoGrid.BackgroundColor = SystemColors.Control;
            SupplierLicenceInfoGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            SupplierLicenceInfoGrid.Columns.AddRange(new DataGridViewColumn[] { SupplierTaxIdName, SupplierTaxDisplayName, SupplierTaxIdValue, Column3, SupplierTaxIdPrintOnReport, SupplierTaxIdDelete, CustomerTaxIdId, Column1, Column2 });
            SupplierLicenceInfoGrid.EditMode = DataGridViewEditMode.EditOnEnter;
            SupplierLicenceInfoGrid.EnableHeadersVisualStyles = false;
            SupplierLicenceInfoGrid.Location = new Point(13, 15);
            SupplierLicenceInfoGrid.Name = "SupplierLicenceInfoGrid";
            SupplierLicenceInfoGrid.RowHeadersVisible = false;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            SupplierLicenceInfoGrid.RowsDefaultCellStyle = dataGridViewCellStyle2;
            SupplierLicenceInfoGrid.RowTemplate.Height = 20;
            SupplierLicenceInfoGrid.ScrollBars = ScrollBars.Vertical;
            SupplierLicenceInfoGrid.ShowCellToolTips = false;
            SupplierLicenceInfoGrid.Size = new Size(672, 369);
            SupplierLicenceInfoGrid.TabIndex = 21;
            SupplierLicenceInfoGrid.CellContentClick += SupplierLicenceInfoGrid_CellContentClick;
            SupplierLicenceInfoGrid.CellEnter += SupplierLicenceInfoGrid_CellEnter;
            SupplierLicenceInfoGrid.Leave += SupplierLicenceInfoGrid_Leave;
            // 
            // SupplierTaxIdName
            // 
            SupplierTaxIdName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            SupplierTaxIdName.HeaderText = "Name";
            SupplierTaxIdName.Name = "SupplierTaxIdName";
            SupplierTaxIdName.NameLength = 20;
            SupplierTaxIdName.Resizable = DataGridViewTriState.False;
            // 
            // SupplierTaxDisplayName
            // 
            SupplierTaxDisplayName.HeaderText = "Display Name";
            SupplierTaxDisplayName.Name = "SupplierTaxDisplayName";
            SupplierTaxDisplayName.NameLength = 20;
            SupplierTaxDisplayName.Resizable = DataGridViewTriState.False;
            SupplierTaxDisplayName.Width = 290;
            // 
            // SupplierTaxIdValue
            // 
            SupplierTaxIdValue.HeaderText = "Value";
            SupplierTaxIdValue.Name = "SupplierTaxIdValue";
            SupplierTaxIdValue.NameLength = 20;
            SupplierTaxIdValue.Resizable = DataGridViewTriState.False;
            // 
            // Column3
            // 
            Column3.HeaderText = "print on invoice";
            Column3.Name = "Column3";
            Column3.Visible = false;
            // 
            // SupplierTaxIdPrintOnReport
            // 
            SupplierTaxIdPrintOnReport.HeaderText = "Print On Report";
            SupplierTaxIdPrintOnReport.Name = "SupplierTaxIdPrintOnReport";
            SupplierTaxIdPrintOnReport.Resizable = DataGridViewTriState.False;
            SupplierTaxIdPrintOnReport.Width = 80;
            // 
            // SupplierTaxIdDelete
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "X";
            SupplierTaxIdDelete.DefaultCellStyle = dataGridViewCellStyle1;
            SupplierTaxIdDelete.HeaderText = "...";
            SupplierTaxIdDelete.Name = "SupplierTaxIdDelete";
            SupplierTaxIdDelete.Resizable = DataGridViewTriState.False;
            SupplierTaxIdDelete.SortMode = DataGridViewColumnSortMode.Automatic;
            SupplierTaxIdDelete.Visible = false;
            SupplierTaxIdDelete.Width = 25;
            // 
            // CustomerTaxIdId
            // 
            CustomerTaxIdId.HeaderText = "Id";
            CustomerTaxIdId.Name = "CustomerTaxIdId";
            CustomerTaxIdId.Visible = false;
            // 
            // Column1
            // 
            Column1.HeaderText = "MasterId";
            Column1.Name = "Column1";
            Column1.Visible = false;
            // 
            // Column2
            // 
            Column2.HeaderText = "Requir";
            Column2.Name = "Column2";
            Column2.Resizable = DataGridViewTriState.True;
            Column2.SortMode = DataGridViewColumnSortMode.Automatic;
            Column2.Visible = false;
            // 
            // supplierproduct
            // 
            supplierproduct.Controls.Add(TextBoxSelectedProductSearch);
            supplierproduct.Controls.Add(BtnRemoveProduct);
            supplierproduct.Controls.Add(BtnSelectProduct);
            supplierproduct.Controls.Add(TreeViewSelectedProduct);
            supplierproduct.Controls.Add(TextBoxProductSearch);
            supplierproduct.Controls.Add(TreeViewProduct);
            supplierproduct.Location = new Point(4, 24);
            supplierproduct.Name = "supplierproduct";
            supplierproduct.Padding = new Padding(3);
            supplierproduct.Size = new Size(699, 413);
            supplierproduct.TabIndex = 3;
            supplierproduct.Text = "Product Linking";
            supplierproduct.UseVisualStyleBackColor = true;
            // 
            // TextBoxSelectedProductSearch
            // 
            TextBoxSelectedProductSearch.Location = new Point(399, 23);
            TextBoxSelectedProductSearch.MaxLength = 50;
            TextBoxSelectedProductSearch.Name = "TextBoxSelectedProductSearch";
            TextBoxSelectedProductSearch.Size = new Size(239, 21);
            TextBoxSelectedProductSearch.TabIndex = 29;
            TextBoxSelectedProductSearch.TextChanged += TextBoxSelectedProductSearch_TextChanged;
            // 
            // BtnRemoveProduct
            // 
            BtnRemoveProduct.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnRemoveProduct.Location = new Point(306, 269);
            BtnRemoveProduct.Name = "BtnRemoveProduct";
            BtnRemoveProduct.Size = new Size(84, 23);
            BtnRemoveProduct.TabIndex = 28;
            BtnRemoveProduct.Text = "<-- Remove";
            BtnRemoveProduct.UseVisualStyleBackColor = true;
            BtnRemoveProduct.Click += BtnRemoveProduct_Click;
            // 
            // BtnSelectProduct
            // 
            BtnSelectProduct.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSelectProduct.Location = new Point(306, 142);
            BtnSelectProduct.Name = "BtnSelectProduct";
            BtnSelectProduct.Size = new Size(84, 23);
            BtnSelectProduct.TabIndex = 27;
            BtnSelectProduct.Text = "Select -->";
            BtnSelectProduct.UseVisualStyleBackColor = true;
            BtnSelectProduct.Click += BtnSelectProduct_Click;
            // 
            // TreeViewSelectedProduct
            // 
            TreeViewSelectedProduct.CheckBoxes = true;
            TreeViewSelectedProduct.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TreeViewSelectedProduct.HideSelection = false;
            TreeViewSelectedProduct.Location = new Point(399, 50);
            TreeViewSelectedProduct.Name = "TreeViewSelectedProduct";
            TreeViewSelectedProduct.Size = new Size(239, 341);
            TreeViewSelectedProduct.TabIndex = 26;
            // 
            // TextBoxProductSearch
            // 
            TextBoxProductSearch.Location = new Point(61, 23);
            TextBoxProductSearch.MaxLength = 50;
            TextBoxProductSearch.Name = "TextBoxProductSearch";
            TextBoxProductSearch.Size = new Size(239, 21);
            TextBoxProductSearch.TabIndex = 24;
            TextBoxProductSearch.TextChanged += TextBoxProductSearch_TextChanged;
            // 
            // TreeViewProduct
            // 
            TreeViewProduct.CheckBoxes = true;
            TreeViewProduct.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TreeViewProduct.HideSelection = false;
            TreeViewProduct.Location = new Point(61, 50);
            TreeViewProduct.Name = "TreeViewProduct";
            TreeViewProduct.Size = new Size(239, 341);
            TreeViewProduct.TabIndex = 25;
            // 
            // BtnSupplierSave
            // 
            BtnSupplierSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSupplierSave.Location = new Point(790, 465);
            BtnSupplierSave.Name = "BtnSupplierSave";
            BtnSupplierSave.Size = new Size(83, 24);
            BtnSupplierSave.TabIndex = 22;
            BtnSupplierSave.Text = "Save [F8]";
            BtnSupplierSave.UseVisualStyleBackColor = true;
            BtnSupplierSave.Click += BtnSupplierSave_Click;
            BtnSupplierSave.PreviewKeyDown += BtnSupplierSave_PreviewKeyDown;
            // 
            // BtnSupplierCancel
            // 
            BtnSupplierCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSupplierCancel.Location = new Point(701, 465);
            BtnSupplierCancel.Name = "BtnSupplierCancel";
            BtnSupplierCancel.Size = new Size(83, 24);
            BtnSupplierCancel.TabIndex = 23;
            BtnSupplierCancel.Text = "Cancel [Esc]";
            BtnSupplierCancel.UseVisualStyleBackColor = true;
            BtnSupplierCancel.Click += BtnSupplierCancel_Click;
            // 
            // BtnSupplierEdit
            // 
            BtnSupplierEdit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSupplierEdit.Location = new Point(189, 465);
            BtnSupplierEdit.Name = "BtnSupplierEdit";
            BtnSupplierEdit.Size = new Size(83, 24);
            BtnSupplierEdit.TabIndex = 5;
            BtnSupplierEdit.Text = "Edit [F7]";
            BtnSupplierEdit.UseVisualStyleBackColor = true;
            BtnSupplierEdit.Click += BtnSupplierEdit_Click;
            // 
            // BtnSupplierNew
            // 
            BtnSupplierNew.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSupplierNew.Location = new Point(11, 465);
            BtnSupplierNew.Name = "BtnSupplierNew";
            BtnSupplierNew.Size = new Size(83, 24);
            BtnSupplierNew.TabIndex = 3;
            BtnSupplierNew.Text = "New [F3]";
            BtnSupplierNew.UseVisualStyleBackColor = true;
            BtnSupplierNew.Click += BtnSupplierNew_Click;
            // 
            // BtnSupplierDelete
            // 
            BtnSupplierDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSupplierDelete.Location = new Point(100, 465);
            BtnSupplierDelete.Name = "BtnSupplierDelete";
            BtnSupplierDelete.Size = new Size(83, 24);
            BtnSupplierDelete.TabIndex = 4;
            BtnSupplierDelete.Text = "Delete [F4]";
            BtnSupplierDelete.UseVisualStyleBackColor = true;
            BtnSupplierDelete.Click += BtnSupplierDelete_Click;
            // 
            // TreeViewSupplier
            // 
            TreeViewSupplier.Enabled = false;
            TreeViewSupplier.HideSelection = false;
            TreeViewSupplier.Location = new Point(12, 39);
            TreeViewSupplier.Name = "TreeViewSupplier";
            TreeViewSupplier.Size = new Size(239, 410);
            TreeViewSupplier.TabIndex = 2;
            TreeViewSupplier.AfterSelect += TreeViewSupplier_AfterSelect;
            // 
            // ImageListSupplier
            // 
            ImageListSupplier.ColorDepth = ColorDepth.Depth8Bit;
            ImageListSupplier.ImageSize = new Size(16, 16);
            ImageListSupplier.TransparentColor = Color.Transparent;
            // 
            // TextBoxSupplierAccountId
            // 
            TextBoxSupplierAccountId.Location = new Point(414, 467);
            TextBoxSupplierAccountId.Name = "TextBoxSupplierAccountId";
            TextBoxSupplierAccountId.Size = new Size(100, 21);
            TextBoxSupplierAccountId.TabIndex = 22;
            TextBoxSupplierAccountId.Visible = false;
            // 
            // BtnCurrencyExit
            // 
            BtnCurrencyExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCurrencyExit.Location = new Point(879, 465);
            BtnCurrencyExit.Name = "BtnCurrencyExit";
            BtnCurrencyExit.Size = new Size(83, 24);
            BtnCurrencyExit.TabIndex = 24;
            BtnCurrencyExit.Text = "Exit [F10]";
            BtnCurrencyExit.UseVisualStyleBackColor = true;
            BtnCurrencyExit.Click += BtnCurrencyExit_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ToolStripStatusLabelErrorSupplier });
            statusStrip1.Location = new Point(0, 502);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(977, 22);
            statusStrip1.TabIndex = 56;
            statusStrip1.Text = "sdfdsf sdf sdf";
            // 
            // ToolStripStatusLabelErrorSupplier
            // 
            ToolStripStatusLabelErrorSupplier.Name = "ToolStripStatusLabelErrorSupplier";
            ToolStripStatusLabelErrorSupplier.Size = new Size(151, 17);
            ToolStripStatusLabelErrorSupplier.Text = "                                                ";
            // 
            // CompanyDataGridViewTaxType
            // 
            CompanyDataGridViewTaxType.AllowUserToResizeColumns = false;
            CompanyDataGridViewTaxType.AllowUserToResizeRows = false;
            CompanyDataGridViewTaxType.BackgroundColor = SystemColors.Control;
            CompanyDataGridViewTaxType.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            CompanyDataGridViewTaxType.EditMode = DataGridViewEditMode.EditOnEnter;
            CompanyDataGridViewTaxType.Location = new Point(26, 22);
            CompanyDataGridViewTaxType.Margin = new Padding(2);
            CompanyDataGridViewTaxType.Name = "CompanyDataGridViewTaxType";
            CompanyDataGridViewTaxType.RowHeadersVisible = false;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.ActiveCaptionText;
            CompanyDataGridViewTaxType.RowsDefaultCellStyle = dataGridViewCellStyle3;
            CompanyDataGridViewTaxType.RowTemplate.Height = 24;
            CompanyDataGridViewTaxType.ScrollBars = ScrollBars.Vertical;
            CompanyDataGridViewTaxType.Size = new Size(473, 215);
            CompanyDataGridViewTaxType.TabIndex = 28;
            // 
            // TextBoxSupplierSearch
            // 
            TextBoxSupplierSearch.BackColor = SystemColors.Window;
            TextBoxSupplierSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxSupplierSearch.Delay = true;
            TextBoxSupplierSearch.DelayTime = 1000;
            TextBoxSupplierSearch.Location = new Point(12, 12);
            TextBoxSupplierSearch.MaxLength = 35;
            TextBoxSupplierSearch.Name = "TextBoxSupplierSearch";
            TextBoxSupplierSearch.Searchstartfrom = 2;
            TextBoxSupplierSearch.Size = new Size(239, 21);
            TextBoxSupplierSearch.TabIndex = 1;
            TextBoxSupplierSearch.TextChanged += TextBoxSupplierSearch_TextChanged;
            TextBoxSupplierSearch.KeyDown += TextBoxSupplierSearch_KeyDown;
            // 
            // FormSupplier
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(977, 524);
            Controls.Add(TabControlSupplier);
            Controls.Add(statusStrip1);
            Controls.Add(BtnCurrencyExit);
            Controls.Add(BtnSupplierCancel);
            Controls.Add(BtnSupplierSave);
            Controls.Add(TextBoxSupplierAccountId);
            Controls.Add(TreeViewSupplier);
            Controls.Add(BtnSupplierDelete);
            Controls.Add(BtnSupplierEdit);
            Controls.Add(BtnSupplierNew);
            Controls.Add(TextBoxSupplierSearch);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSupplier";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Suppliers (Vendor)";
            FormClosing += FormSupplier_FormClosing;
            Load += FormSupplier_Load;
            Controls.SetChildIndex(checkBoxIsPatient, 0);
            Controls.SetChildIndex(TextBoxSupplierSearch, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(BtnSupplierNew, 0);
            Controls.SetChildIndex(BtnSupplierEdit, 0);
            Controls.SetChildIndex(BtnSupplierDelete, 0);
            Controls.SetChildIndex(TreeViewSupplier, 0);
            Controls.SetChildIndex(TextBoxSupplierAccountId, 0);
            Controls.SetChildIndex(BtnSupplierSave, 0);
            Controls.SetChildIndex(BtnSupplierCancel, 0);
            Controls.SetChildIndex(BtnCurrencyExit, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(TabControlSupplier, 0);
            TabControlSupplier.ResumeLayout(false);
            TabGeneralPage.ResumeLayout(false);
            TabGeneralPage.PerformLayout();
            TabContactInfo.ResumeLayout(false);
            TabContactInfo.PerformLayout();
            TabLicenseInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SupplierLicenceInfoGrid).EndInit();
            supplierproduct.ResumeLayout(false);
            supplierproduct.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)CompanyDataGridViewTaxType).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TabControl TabControlSupplier;
        private TabPage TabGeneralPage;
        private TabPage TabContactInfo;
        private Button BtnSupplierNew;
        private Button BtnSupplierDelete;
        private Button BtnSupplierEdit;
        private Button BtnSupplierSave;
        private Button BtnSupplierCancel;
        private Label LabelSupplierName;
        private Label LabelSupplierDisplayAs;
        private Label LabelSupplierDescription;
        private TextBox TextBoxSupplierDescription;
        private TextBox TextBoxSupplierDisplayAs;
        private TextBox TextBoxSupplierName;
        private Label LabelSupplierParent;
        private Label LabelSupplierBranch;
        private CheckBox CheckBoxSupplierIsbranch;
        private TextBox TextBoxSupplierWebsite;
        private TextBox TextBoxSupplierEmail;
        private Label LabelSupplierWebsite;
        private Label LabelSupplierEmail;
        private Label LabelSupplierFax;
        private Label LabelSupplierMobile;
        private Label LabelSupplierPhone;
        private Label LabelSupplierBalance;
        private Label LabelSupplierAsof;
        private TreeView TreeViewSupplier;
        private MaskedTextBox TextBoxSupplierAccountId;
        private Button BtnCurrencyExit;
        private MaskedTextBox TextBoxSupplierFax;
        private ImageList ImageListSupplier;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel ToolStripStatusLabelErrorSupplier;
        private controls.ComboBoxSwapTextBox ComboBoxSupplierParentAccount;
        private controls.text.DateWithCalendar DateTimePickerSupplier;
        private controls.text.CurrencyTextBox TextBoxSupplierBalance;
        private controls.text.PhoneTextBox TextBoxSupplierPhone;
        private controls.text.PhoneTextBox TextBoxSupplierMobile;
        private TabPage TabLicenseInfo;
        private controls.DataViewVerticalScroll CompanyDataGridViewTaxType;
        private controls.ComboBoxSwapTextBox ComboBoxBalanceType;
        private controls.DataViewVerticalScroll SupplierLicenceInfoGrid;
        private Label label4;
        private controls.text.DelayedTextChangeTextBox TextBoxSupplierSearch;
        private controls.grid.DataGridViewNameColumn SupplierTaxIdName;
        private controls.grid.DataGridViewNameColumn SupplierTaxDisplayName;
        private controls.grid.DataGridViewNameColumn SupplierTaxIdValue;
        private DataGridViewCheckBoxColumn Column3;
        private DataGridViewCheckBoxColumn SupplierTaxIdPrintOnReport;
        private DataGridViewButtonColumn SupplierTaxIdDelete;
        private DataGridViewTextBoxColumn CustomerTaxIdId;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewCheckBoxColumn Column2;
        private controls.AddressGroupBoxWithStateSelection AddressGroupBoxSupplier;
        private TabPage supplierproduct;
        private controls.text.NameTextBoxAllowSpace TextBoxSelectedProductSearch;
        private Button BtnRemoveProduct;
        private Button BtnSelectProduct;
        public TreeView TreeViewSelectedProduct;
        private controls.text.NameTextBoxAllowSpace TextBoxProductSearch;
        public TreeView TreeViewProduct;
        private TextBox TextBoxGSTNo;
        private Label labelGSTNo;
    }
}