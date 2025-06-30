namespace fa.views.hms.config
{
    partial class FormHospitalSettings
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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHospitalSettings));
            TabControlHospitalSetting = new TabControl();
            TabOpRegistration = new TabPage();
            ComboBoxOpRegistrationAccount = new controls.ComboBoxSwapTextBox();
            TextBoxRegistrationFee = new controls.text.CurrencyTextBox();
            label3 = new Label();
            label1 = new Label();
            TabIpRegistration = new TabPage();
            ComboBoxIpRegistrationAccount = new controls.ComboBoxSwapTextBox();
            TextBoxIpRegistrationFee = new controls.text.CurrencyTextBox();
            label2 = new Label();
            label4 = new Label();
            TabPrinterSetup = new TabPage();
            ComboBoxHmsYear = new controls.ComboBoxSwapTextBox();
            BtnHmsAddYear = new Button();
            GridViewHmsYear = new controls.DataViewVerticalScroll();
            Type = new DataGridViewTextBoxColumn();
            Prefix = new DataGridViewTextBoxColumn();
            Seed = new controls.grid.DataGridViewNumberColumn();
            DailyReset = new DataGridViewCheckBoxColumn();
            PrintType = new DataGridViewComboBoxColumn();
            IsDotMatrix = new DataGridViewCheckBoxColumn();
            RoundOff = new controls.grid.DataGridViewCurrencyColumn();
            TypeId = new DataGridViewTextBoxColumn();
            HasRoundOff = new DataGridViewCheckBoxColumn();
            HasPrinterSetup = new DataGridViewCheckBoxColumn();
            HasDotmatrix = new DataGridViewCheckBoxColumn();
            TabDefaultAccounts = new TabPage();
            ComboBoxPatientPurchaseAccount = new controls.ComboBoxSwapTextBox();
            UndepositedFundAccountLbl = new Label();
            TapInvoiceGroup = new TabPage();
            BtnUpdateInvGroup = new Button();
            GroupBoxInvoiceGroup = new GroupBox();
            CheckedListBoxInvoiceGroup = new CheckedListBox();
            TextBoxInvoiceGroupName = new controls.text.NameTextBoxAllowSpace(components);
            TextBoxInvoiceGroupDescription = new TextBox();
            LabelCostCenterDescription = new Label();
            LabelCostCenterName = new Label();
            label8 = new Label();
            ListBoxInvoiceGroup = new ListBox();
            CheckBoxInvoiceGroupEnable = new CheckBox();
            TabControlPrescriptionSetup = new TabPage();
            TextBoxFooterDetailsPresc = new TextBox();
            CheckBoxFooterDetailsPresc = new CheckBox();
            label5 = new Label();
            CheckBoxDigitalSingnature = new CheckBox();
            CheckBoxSingnatureName = new CheckBox();
            TabControlMedicalTestSetup = new TabPage();
            label6 = new Label();
            checkBoxLabTechnicianSignName = new CheckBox();
            checkBoxLabtestDigSignature = new CheckBox();
            TextBoxFooterDetailsLabtest = new TextBox();
            CheckBoxFooterDetailsLabtest = new CheckBox();
            BtnHMSSettingCancel = new Button();
            BtnHMSSettingSave = new Button();
            BtnHMSSettingExit = new Button();
            statusStrip1 = new StatusStrip();
            HMSConfigErrorMsg = new ToolStripStatusLabel();
            TabControlHospitalSetting.SuspendLayout();
            TabOpRegistration.SuspendLayout();
            TabIpRegistration.SuspendLayout();
            TabPrinterSetup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewHmsYear).BeginInit();
            TabDefaultAccounts.SuspendLayout();
            TapInvoiceGroup.SuspendLayout();
            GroupBoxInvoiceGroup.SuspendLayout();
            TabControlPrescriptionSetup.SuspendLayout();
            TabControlMedicalTestSetup.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // PatientIdTransport
            // 
            PatientIdTransport.Size = new Size(100, 21);
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(232, 447);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(124, 447);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(16, 447);
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // TabControlHospitalSetting
            // 
            TabControlHospitalSetting.Controls.Add(TabOpRegistration);
            TabControlHospitalSetting.Controls.Add(TabIpRegistration);
            TabControlHospitalSetting.Controls.Add(TabPrinterSetup);
            TabControlHospitalSetting.Controls.Add(TabDefaultAccounts);
            TabControlHospitalSetting.Controls.Add(TapInvoiceGroup);
            TabControlHospitalSetting.Controls.Add(TabControlPrescriptionSetup);
            TabControlHospitalSetting.Controls.Add(TabControlMedicalTestSetup);
            TabControlHospitalSetting.Location = new Point(12, 12);
            TabControlHospitalSetting.Name = "TabControlHospitalSetting";
            TabControlHospitalSetting.SelectedIndex = 0;
            TabControlHospitalSetting.Size = new Size(776, 420);
            TabControlHospitalSetting.TabIndex = 0;
            // 
            // TabOpRegistration
            // 
            TabOpRegistration.Controls.Add(ComboBoxOpRegistrationAccount);
            TabOpRegistration.Controls.Add(TextBoxRegistrationFee);
            TabOpRegistration.Controls.Add(label3);
            TabOpRegistration.Controls.Add(label1);
            TabOpRegistration.Location = new Point(4, 22);
            TabOpRegistration.Name = "TabOpRegistration";
            TabOpRegistration.Padding = new Padding(3);
            TabOpRegistration.Size = new Size(768, 394);
            TabOpRegistration.TabIndex = 0;
            TabOpRegistration.Text = "OP Registration";
            TabOpRegistration.UseVisualStyleBackColor = true;
            // 
            // ComboBoxOpRegistrationAccount
            // 
            ComboBoxOpRegistrationAccount.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxOpRegistrationAccount.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxOpRegistrationAccount.FormattingEnabled = true;
            ComboBoxOpRegistrationAccount.Location = new Point(12, 67);
            ComboBoxOpRegistrationAccount.Name = "ComboBoxOpRegistrationAccount";
            ComboBoxOpRegistrationAccount.Size = new Size(189, 21);
            ComboBoxOpRegistrationAccount.TabIndex = 1;
            ComboBoxOpRegistrationAccount.TxtVisible = true;
            ComboBoxOpRegistrationAccount.PreviewKeyDown += ComboBoxOpRegistrationAccount_PreviewKeyDown;
            // 
            // TextBoxRegistrationFee
            // 
            TextBoxRegistrationFee.Decimals = 2;
            TextBoxRegistrationFee.Length = 10;
            TextBoxRegistrationFee.Location = new Point(12, 23);
            TextBoxRegistrationFee.Name = "TextBoxRegistrationFee";
            TextBoxRegistrationFee.Size = new Size(98, 21);
            TextBoxRegistrationFee.TabIndex = 0;
            TextBoxRegistrationFee.Text = "0.00";
            TextBoxRegistrationFee.TextAlign = HorizontalAlignment.Right;
            TextBoxRegistrationFee.PreviewKeyDown += TextBoxRegistrationFee_PreviewKeyDown;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(12, 50);
            label3.Name = "label3";
            label3.Size = new Size(101, 13);
            label3.TabIndex = 53;
            label3.Text = "Registration A/C";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(12, 6);
            label1.Name = "label1";
            label1.Size = new Size(86, 13);
            label1.TabIndex = 2;
            label1.Text = "Registration Fee";
            // 
            // TabIpRegistration
            // 
            TabIpRegistration.Controls.Add(ComboBoxIpRegistrationAccount);
            TabIpRegistration.Controls.Add(TextBoxIpRegistrationFee);
            TabIpRegistration.Controls.Add(label2);
            TabIpRegistration.Controls.Add(label4);
            TabIpRegistration.Location = new Point(4, 24);
            TabIpRegistration.Name = "TabIpRegistration";
            TabIpRegistration.Size = new Size(768, 392);
            TabIpRegistration.TabIndex = 1;
            TabIpRegistration.Text = "IP Registration";
            TabIpRegistration.UseVisualStyleBackColor = true;
            // 
            // ComboBoxIpRegistrationAccount
            // 
            ComboBoxIpRegistrationAccount.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxIpRegistrationAccount.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxIpRegistrationAccount.FormattingEnabled = true;
            ComboBoxIpRegistrationAccount.Location = new Point(12, 67);
            ComboBoxIpRegistrationAccount.Name = "ComboBoxIpRegistrationAccount";
            ComboBoxIpRegistrationAccount.Size = new Size(189, 21);
            ComboBoxIpRegistrationAccount.TabIndex = 3;
            ComboBoxIpRegistrationAccount.TxtVisible = true;
            ComboBoxIpRegistrationAccount.PreviewKeyDown += ComboBoxIpRegistrationAccount_PreviewKeyDown;
            // 
            // TextBoxIpRegistrationFee
            // 
            TextBoxIpRegistrationFee.Decimals = 2;
            TextBoxIpRegistrationFee.Length = 10;
            TextBoxIpRegistrationFee.Location = new Point(12, 23);
            TextBoxIpRegistrationFee.Name = "TextBoxIpRegistrationFee";
            TextBoxIpRegistrationFee.Size = new Size(98, 21);
            TextBoxIpRegistrationFee.TabIndex = 2;
            TextBoxIpRegistrationFee.Text = "0.00";
            TextBoxIpRegistrationFee.TextAlign = HorizontalAlignment.Right;
            TextBoxIpRegistrationFee.PreviewKeyDown += TextBoxIpRegistrationFee_PreviewKeyDown;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(12, 50);
            label2.Name = "label2";
            label2.Size = new Size(101, 13);
            label2.TabIndex = 57;
            label2.Text = "Registration A/C";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(12, 6);
            label4.Name = "label4";
            label4.Size = new Size(86, 13);
            label4.TabIndex = 54;
            label4.Text = "Registration Fee";
            // 
            // TabPrinterSetup
            // 
            TabPrinterSetup.Controls.Add(ComboBoxHmsYear);
            TabPrinterSetup.Controls.Add(BtnHmsAddYear);
            TabPrinterSetup.Controls.Add(GridViewHmsYear);
            TabPrinterSetup.Location = new Point(4, 24);
            TabPrinterSetup.Name = "TabPrinterSetup";
            TabPrinterSetup.Padding = new Padding(3);
            TabPrinterSetup.Size = new Size(768, 392);
            TabPrinterSetup.TabIndex = 2;
            TabPrinterSetup.Text = "Print Setup";
            TabPrinterSetup.UseVisualStyleBackColor = true;
            // 
            // ComboBoxHmsYear
            // 
            ComboBoxHmsYear.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxHmsYear.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxHmsYear.FormattingEnabled = true;
            ComboBoxHmsYear.Location = new Point(12, 6);
            ComboBoxHmsYear.Name = "ComboBoxHmsYear";
            ComboBoxHmsYear.Size = new Size(136, 21);
            ComboBoxHmsYear.TabIndex = 4;
            ComboBoxHmsYear.TxtVisible = true;
            ComboBoxHmsYear.SelectedIndexChanged += ComboBoxHmsYear_SelectedIndexChanged;
            ComboBoxHmsYear.PreviewKeyDown += ComboBoxHmsYear_PreviewKeyDown;
            // 
            // BtnHmsAddYear
            // 
            BtnHmsAddYear.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnHmsAddYear.Location = new Point(158, 6);
            BtnHmsAddYear.Name = "BtnHmsAddYear";
            BtnHmsAddYear.Size = new Size(62, 23);
            BtnHmsAddYear.TabIndex = 135;
            BtnHmsAddYear.Text = "New";
            BtnHmsAddYear.UseVisualStyleBackColor = true;
            BtnHmsAddYear.Click += BtnHmsAddYear_Click;
            // 
            // GridViewHmsYear
            // 
            GridViewHmsYear.AllowUserToAddRows = false;
            GridViewHmsYear.AllowUserToDeleteRows = false;
            GridViewHmsYear.AllowUserToResizeColumns = false;
            GridViewHmsYear.AllowUserToResizeRows = false;
            GridViewHmsYear.BackgroundColor = SystemColors.Control;
            GridViewHmsYear.ColumnHeadersHeight = 20;
            GridViewHmsYear.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewHmsYear.Columns.AddRange(new DataGridViewColumn[] { Type, Prefix, Seed, DailyReset, PrintType, IsDotMatrix, RoundOff, TypeId, HasRoundOff, HasPrinterSetup, HasDotmatrix });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.White;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            GridViewHmsYear.DefaultCellStyle = dataGridViewCellStyle3;
            GridViewHmsYear.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewHmsYear.EnableHeadersVisualStyles = false;
            GridViewHmsYear.Location = new Point(12, 36);
            GridViewHmsYear.Name = "GridViewHmsYear";
            GridViewHmsYear.RowHeadersVisible = false;
            GridViewHmsYear.RowTemplate.Height = 20;
            GridViewHmsYear.ShowCellToolTips = false;
            GridViewHmsYear.Size = new Size(733, 210);
            GridViewHmsYear.TabIndex = 3;
            GridViewHmsYear.CellEnter += GridViewHmsYear_CellEnter;
            GridViewHmsYear.DataError += GridViewHmsYear_DataError;
            GridViewHmsYear.EditingControlShowing += GridViewHmsYear_EditingControlShowing;
            // 
            // Type
            // 
            Type.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Type.HeaderText = "Type";
            Type.Name = "Type";
            Type.Resizable = DataGridViewTriState.False;
            Type.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Prefix
            // 
            Prefix.HeaderText = "Prefix";
            Prefix.MaxInputLength = 10;
            Prefix.Name = "Prefix";
            Prefix.Resizable = DataGridViewTriState.False;
            Prefix.SortMode = DataGridViewColumnSortMode.NotSortable;
            Prefix.Width = 80;
            // 
            // Seed
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.TopRight;
            Seed.DefaultCellStyle = dataGridViewCellStyle1;
            Seed.HeaderText = "Seed";
            Seed.Name = "Seed";
            Seed.NumberLength = 6;
            Seed.Resizable = DataGridViewTriState.False;
            Seed.Width = 50;
            // 
            // DailyReset
            // 
            DailyReset.HeaderText = "Daily Reset";
            DailyReset.Name = "DailyReset";
            DailyReset.Resizable = DataGridViewTriState.False;
            DailyReset.Width = 90;
            // 
            // PrintType
            // 
            PrintType.FlatStyle = FlatStyle.Flat;
            PrintType.HeaderText = "Print Type";
            PrintType.Name = "PrintType";
            PrintType.Resizable = DataGridViewTriState.True;
            PrintType.Width = 150;
            // 
            // IsDotMatrix
            // 
            IsDotMatrix.HeaderText = "Is DotMatrix";
            IsDotMatrix.Name = "IsDotMatrix";
            IsDotMatrix.Resizable = DataGridViewTriState.False;
            IsDotMatrix.Width = 90;
            // 
            // RoundOff
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopRight;
            RoundOff.DefaultCellStyle = dataGridViewCellStyle2;
            RoundOff.HeaderText = "Round Off";
            RoundOff.Name = "RoundOff";
            RoundOff.Resizable = DataGridViewTriState.False;
            RoundOff.Width = 80;
            // 
            // TypeId
            // 
            TypeId.HeaderText = "TypeId";
            TypeId.Name = "TypeId";
            TypeId.Visible = false;
            // 
            // HasRoundOff
            // 
            HasRoundOff.HeaderText = "HasRoundOff";
            HasRoundOff.Name = "HasRoundOff";
            HasRoundOff.Visible = false;
            // 
            // HasPrinterSetup
            // 
            HasPrinterSetup.HeaderText = "HasPrinterSetup";
            HasPrinterSetup.Name = "HasPrinterSetup";
            HasPrinterSetup.Visible = false;
            // 
            // HasDotmatrix
            // 
            HasDotmatrix.HeaderText = "HasDotmatrix";
            HasDotmatrix.Name = "HasDotmatrix";
            HasDotmatrix.Visible = false;
            // 
            // TabDefaultAccounts
            // 
            TabDefaultAccounts.Controls.Add(ComboBoxPatientPurchaseAccount);
            TabDefaultAccounts.Controls.Add(UndepositedFundAccountLbl);
            TabDefaultAccounts.Location = new Point(4, 24);
            TabDefaultAccounts.Name = "TabDefaultAccounts";
            TabDefaultAccounts.Size = new Size(768, 392);
            TabDefaultAccounts.TabIndex = 4;
            TabDefaultAccounts.Text = "Default Accounts";
            TabDefaultAccounts.UseVisualStyleBackColor = true;
            // 
            // ComboBoxPatientPurchaseAccount
            // 
            ComboBoxPatientPurchaseAccount.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxPatientPurchaseAccount.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxPatientPurchaseAccount.FormattingEnabled = true;
            ComboBoxPatientPurchaseAccount.Location = new Point(12, 24);
            ComboBoxPatientPurchaseAccount.Name = "ComboBoxPatientPurchaseAccount";
            ComboBoxPatientPurchaseAccount.Size = new Size(189, 21);
            ComboBoxPatientPurchaseAccount.TabIndex = 20;
            ComboBoxPatientPurchaseAccount.TxtVisible = true;
            ComboBoxPatientPurchaseAccount.PreviewKeyDown += ComboBoxPatientPurchaseAccount_PreviewKeyDown;
            // 
            // UndepositedFundAccountLbl
            // 
            UndepositedFundAccountLbl.AutoSize = true;
            UndepositedFundAccountLbl.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            UndepositedFundAccountLbl.Location = new Point(12, 6);
            UndepositedFundAccountLbl.Name = "UndepositedFundAccountLbl";
            UndepositedFundAccountLbl.Size = new Size(130, 13);
            UndepositedFundAccountLbl.TabIndex = 54;
            UndepositedFundAccountLbl.Text = "Patient Purchase Account";
            // 
            // TapInvoiceGroup
            // 
            TapInvoiceGroup.Controls.Add(BtnUpdateInvGroup);
            TapInvoiceGroup.Controls.Add(GroupBoxInvoiceGroup);
            TapInvoiceGroup.Controls.Add(ListBoxInvoiceGroup);
            TapInvoiceGroup.Controls.Add(CheckBoxInvoiceGroupEnable);
            TapInvoiceGroup.Location = new Point(4, 24);
            TapInvoiceGroup.Name = "TapInvoiceGroup";
            TapInvoiceGroup.Size = new Size(768, 392);
            TapInvoiceGroup.TabIndex = 5;
            TapInvoiceGroup.Text = "Invoicing Group";
            TapInvoiceGroup.UseVisualStyleBackColor = true;
            // 
            // BtnUpdateInvGroup
            // 
            BtnUpdateInvGroup.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnUpdateInvGroup.Location = new Point(615, 316);
            BtnUpdateInvGroup.Name = "BtnUpdateInvGroup";
            BtnUpdateInvGroup.Size = new Size(141, 23);
            BtnUpdateInvGroup.TabIndex = 23;
            BtnUpdateInvGroup.Text = "Update Invoice Group";
            BtnUpdateInvGroup.UseVisualStyleBackColor = true;
            BtnUpdateInvGroup.Click += BtnUpdateInvGroup_Click;
            BtnUpdateInvGroup.PreviewKeyDown += BtnUpdateInvGroup_PreviewKeyDown;
            // 
            // GroupBoxInvoiceGroup
            // 
            GroupBoxInvoiceGroup.Controls.Add(CheckedListBoxInvoiceGroup);
            GroupBoxInvoiceGroup.Controls.Add(TextBoxInvoiceGroupName);
            GroupBoxInvoiceGroup.Controls.Add(TextBoxInvoiceGroupDescription);
            GroupBoxInvoiceGroup.Controls.Add(LabelCostCenterDescription);
            GroupBoxInvoiceGroup.Controls.Add(LabelCostCenterName);
            GroupBoxInvoiceGroup.Controls.Add(label8);
            GroupBoxInvoiceGroup.Enabled = false;
            GroupBoxInvoiceGroup.Location = new Point(214, 22);
            GroupBoxInvoiceGroup.Name = "GroupBoxInvoiceGroup";
            GroupBoxInvoiceGroup.Size = new Size(542, 274);
            GroupBoxInvoiceGroup.TabIndex = 40;
            GroupBoxInvoiceGroup.TabStop = false;
            GroupBoxInvoiceGroup.Text = "Details";
            // 
            // CheckedListBoxInvoiceGroup
            // 
            CheckedListBoxInvoiceGroup.CheckOnClick = true;
            CheckedListBoxInvoiceGroup.Enabled = false;
            CheckedListBoxInvoiceGroup.FormattingEnabled = true;
            CheckedListBoxInvoiceGroup.Location = new Point(17, 157);
            CheckedListBoxInvoiceGroup.Name = "CheckedListBoxInvoiceGroup";
            CheckedListBoxInvoiceGroup.ScrollAlwaysVisible = true;
            CheckedListBoxInvoiceGroup.Size = new Size(275, 100);
            CheckedListBoxInvoiceGroup.TabIndex = 25;
            // 
            // TextBoxInvoiceGroupName
            // 
            TextBoxInvoiceGroupName.BackColor = Color.White;
            TextBoxInvoiceGroupName.Location = new Point(17, 38);
            TextBoxInvoiceGroupName.MaxLength = 50;
            TextBoxInvoiceGroupName.Name = "TextBoxInvoiceGroupName";
            TextBoxInvoiceGroupName.ReadOnly = true;
            TextBoxInvoiceGroupName.Size = new Size(459, 21);
            TextBoxInvoiceGroupName.TabIndex = 41;
            TextBoxInvoiceGroupName.TabStop = false;
            // 
            // TextBoxInvoiceGroupDescription
            // 
            TextBoxInvoiceGroupDescription.BackColor = Color.White;
            TextBoxInvoiceGroupDescription.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxInvoiceGroupDescription.Location = new Point(17, 78);
            TextBoxInvoiceGroupDescription.MaxLength = 250;
            TextBoxInvoiceGroupDescription.Multiline = true;
            TextBoxInvoiceGroupDescription.Name = "TextBoxInvoiceGroupDescription";
            TextBoxInvoiceGroupDescription.ReadOnly = true;
            TextBoxInvoiceGroupDescription.Size = new Size(459, 59);
            TextBoxInvoiceGroupDescription.TabIndex = 43;
            TextBoxInvoiceGroupDescription.TabStop = false;
            // 
            // LabelCostCenterDescription
            // 
            LabelCostCenterDescription.AutoSize = true;
            LabelCostCenterDescription.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelCostCenterDescription.Location = new Point(14, 62);
            LabelCostCenterDescription.Name = "LabelCostCenterDescription";
            LabelCostCenterDescription.Size = new Size(60, 13);
            LabelCostCenterDescription.TabIndex = 16;
            LabelCostCenterDescription.Text = "Description";
            // 
            // LabelCostCenterName
            // 
            LabelCostCenterName.AutoSize = true;
            LabelCostCenterName.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelCostCenterName.Location = new Point(14, 22);
            LabelCostCenterName.Name = "LabelCostCenterName";
            LabelCostCenterName.Size = new Size(39, 13);
            LabelCostCenterName.TabIndex = 15;
            LabelCostCenterName.Text = "Name";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label8.Location = new Point(14, 140);
            label8.Name = "label8";
            label8.Size = new Size(90, 13);
            label8.TabIndex = 18;
            label8.Text = "Transaction Type";
            // 
            // ListBoxInvoiceGroup
            // 
            ListBoxInvoiceGroup.FormattingEnabled = true;
            ListBoxInvoiceGroup.Location = new Point(12, 28);
            ListBoxInvoiceGroup.Name = "ListBoxInvoiceGroup";
            ListBoxInvoiceGroup.Size = new Size(196, 264);
            ListBoxInvoiceGroup.TabIndex = 22;
            ListBoxInvoiceGroup.SelectedIndexChanged += ListBoxInvoiceGroup_SelectedIndexChanged;
            // 
            // CheckBoxInvoiceGroupEnable
            // 
            CheckBoxInvoiceGroupEnable.AutoSize = true;
            CheckBoxInvoiceGroupEnable.Location = new Point(12, 6);
            CheckBoxInvoiceGroupEnable.Name = "CheckBoxInvoiceGroupEnable";
            CheckBoxInvoiceGroupEnable.Size = new Size(64, 17);
            CheckBoxInvoiceGroupEnable.TabIndex = 21;
            CheckBoxInvoiceGroupEnable.Text = "Enabled";
            CheckBoxInvoiceGroupEnable.UseVisualStyleBackColor = true;
            CheckBoxInvoiceGroupEnable.PreviewKeyDown += CheckBoxInvoiceGroupEnable_PreviewKeyDown;
            // 
            // TabControlPrescriptionSetup
            // 
            TabControlPrescriptionSetup.Controls.Add(TextBoxFooterDetailsPresc);
            TabControlPrescriptionSetup.Controls.Add(CheckBoxFooterDetailsPresc);
            TabControlPrescriptionSetup.Controls.Add(label5);
            TabControlPrescriptionSetup.Controls.Add(CheckBoxDigitalSingnature);
            TabControlPrescriptionSetup.Controls.Add(CheckBoxSingnatureName);
            TabControlPrescriptionSetup.Location = new Point(4, 22);
            TabControlPrescriptionSetup.Name = "TabControlPrescriptionSetup";
            TabControlPrescriptionSetup.Padding = new Padding(3);
            TabControlPrescriptionSetup.Size = new Size(768, 394);
            TabControlPrescriptionSetup.TabIndex = 6;
            TabControlPrescriptionSetup.Text = "Prescription Setup";
            TabControlPrescriptionSetup.UseVisualStyleBackColor = true;
            // 
            // TextBoxFooterDetailsPresc
            // 
            TextBoxFooterDetailsPresc.BackColor = Color.White;
            TextBoxFooterDetailsPresc.Location = new Point(12, 93);
            TextBoxFooterDetailsPresc.Multiline = true;
            TextBoxFooterDetailsPresc.Name = "TextBoxFooterDetailsPresc";
            TextBoxFooterDetailsPresc.Size = new Size(347, 80);
            TextBoxFooterDetailsPresc.TabIndex = 27;
            TextBoxFooterDetailsPresc.PreviewKeyDown += TextBoxFooterDetailsPresc_PreviewKeyDown;
            // 
            // CheckBoxFooterDetailsPresc
            // 
            CheckBoxFooterDetailsPresc.AutoSize = true;
            CheckBoxFooterDetailsPresc.Location = new Point(12, 70);
            CheckBoxFooterDetailsPresc.Name = "CheckBoxFooterDetailsPresc";
            CheckBoxFooterDetailsPresc.Size = new Size(203, 17);
            CheckBoxFooterDetailsPresc.TabIndex = 26;
            CheckBoxFooterDetailsPresc.Text = "Display Footer details on prescription";
            CheckBoxFooterDetailsPresc.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 6);
            label5.Name = "label5";
            label5.Size = new Size(63, 13);
            label5.TabIndex = 85;
            label5.Text = "Prescription";
            // 
            // CheckBoxDigitalSingnature
            // 
            CheckBoxDigitalSingnature.AutoSize = true;
            CheckBoxDigitalSingnature.Location = new Point(12, 24);
            CheckBoxDigitalSingnature.Name = "CheckBoxDigitalSingnature";
            CheckBoxDigitalSingnature.Size = new Size(213, 17);
            CheckBoxDigitalSingnature.TabIndex = 24;
            CheckBoxDigitalSingnature.Text = "Display digital signature on prescription";
            CheckBoxDigitalSingnature.UseVisualStyleBackColor = true;
            CheckBoxDigitalSingnature.PreviewKeyDown += CheckBoxDigitalSingnature_PreviewKeyDown;
            // 
            // CheckBoxSingnatureName
            // 
            CheckBoxSingnatureName.AutoSize = true;
            CheckBoxSingnatureName.Location = new Point(12, 47);
            CheckBoxSingnatureName.Name = "CheckBoxSingnatureName";
            CheckBoxSingnatureName.Size = new Size(211, 17);
            CheckBoxSingnatureName.TabIndex = 25;
            CheckBoxSingnatureName.Text = "Display signature name on prescription";
            CheckBoxSingnatureName.UseVisualStyleBackColor = true;
            // 
            // TabControlMedicalTestSetup
            // 
            TabControlMedicalTestSetup.Controls.Add(label6);
            TabControlMedicalTestSetup.Controls.Add(checkBoxLabTechnicianSignName);
            TabControlMedicalTestSetup.Controls.Add(checkBoxLabtestDigSignature);
            TabControlMedicalTestSetup.Controls.Add(TextBoxFooterDetailsLabtest);
            TabControlMedicalTestSetup.Controls.Add(CheckBoxFooterDetailsLabtest);
            TabControlMedicalTestSetup.Location = new Point(4, 22);
            TabControlMedicalTestSetup.Name = "TabControlMedicalTestSetup";
            TabControlMedicalTestSetup.Padding = new Padding(3);
            TabControlMedicalTestSetup.Size = new Size(768, 394);
            TabControlMedicalTestSetup.TabIndex = 7;
            TabControlMedicalTestSetup.Text = "Medical Test Setup";
            TabControlMedicalTestSetup.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 6);
            label6.Name = "label6";
            label6.Size = new Size(66, 13);
            label6.TabIndex = 94;
            label6.Text = "Medical Test";
            // 
            // checkBoxLabTechnicianSignName
            // 
            checkBoxLabTechnicianSignName.AutoSize = true;
            checkBoxLabTechnicianSignName.Location = new Point(12, 47);
            checkBoxLabTechnicianSignName.Name = "checkBoxLabTechnicianSignName";
            checkBoxLabTechnicianSignName.Size = new Size(212, 17);
            checkBoxLabTechnicianSignName.TabIndex = 29;
            checkBoxLabTechnicianSignName.Text = "Display signature name on medical test";
            checkBoxLabTechnicianSignName.UseVisualStyleBackColor = true;
            // 
            // checkBoxLabtestDigSignature
            // 
            checkBoxLabtestDigSignature.AutoSize = true;
            checkBoxLabtestDigSignature.Location = new Point(12, 24);
            checkBoxLabtestDigSignature.Name = "checkBoxLabtestDigSignature";
            checkBoxLabtestDigSignature.Size = new Size(214, 17);
            checkBoxLabtestDigSignature.TabIndex = 28;
            checkBoxLabtestDigSignature.Text = "Display digital signature on medical test";
            checkBoxLabtestDigSignature.UseVisualStyleBackColor = true;
            checkBoxLabtestDigSignature.PreviewKeyDown += checkBoxLabtestDigSignature_PreviewKeyDown;
            // 
            // TextBoxFooterDetailsLabtest
            // 
            TextBoxFooterDetailsLabtest.BackColor = Color.White;
            TextBoxFooterDetailsLabtest.Location = new Point(12, 93);
            TextBoxFooterDetailsLabtest.Multiline = true;
            TextBoxFooterDetailsLabtest.Name = "TextBoxFooterDetailsLabtest";
            TextBoxFooterDetailsLabtest.Size = new Size(347, 80);
            TextBoxFooterDetailsLabtest.TabIndex = 31;
            TextBoxFooterDetailsLabtest.PreviewKeyDown += TextBoxFooterDetailsLabtest_PreviewKeyDown;
            // 
            // CheckBoxFooterDetailsLabtest
            // 
            CheckBoxFooterDetailsLabtest.AutoSize = true;
            CheckBoxFooterDetailsLabtest.Location = new Point(12, 70);
            CheckBoxFooterDetailsLabtest.Name = "CheckBoxFooterDetailsLabtest";
            CheckBoxFooterDetailsLabtest.Size = new Size(204, 17);
            CheckBoxFooterDetailsLabtest.TabIndex = 30;
            CheckBoxFooterDetailsLabtest.Text = "Display Footer details on medical test";
            CheckBoxFooterDetailsLabtest.UseVisualStyleBackColor = true;
            // 
            // BtnHMSSettingCancel
            // 
            BtnHMSSettingCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnHMSSettingCancel.Location = new Point(519, 447);
            BtnHMSSettingCancel.Name = "BtnHMSSettingCancel";
            BtnHMSSettingCancel.Size = new Size(91, 23);
            BtnHMSSettingCancel.TabIndex = 33;
            BtnHMSSettingCancel.Text = "Cancel [Esc]";
            BtnHMSSettingCancel.UseVisualStyleBackColor = true;
            BtnHMSSettingCancel.Click += BtnHMSSettingCancel_Click;
            // 
            // BtnHMSSettingSave
            // 
            BtnHMSSettingSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnHMSSettingSave.Location = new Point(616, 447);
            BtnHMSSettingSave.Name = "BtnHMSSettingSave";
            BtnHMSSettingSave.Size = new Size(75, 23);
            BtnHMSSettingSave.TabIndex = 32;
            BtnHMSSettingSave.Text = "Save [F8]";
            BtnHMSSettingSave.UseVisualStyleBackColor = true;
            BtnHMSSettingSave.Click += BtnHMSSettingSave_Click;
            BtnHMSSettingSave.PreviewKeyDown += BtnHMSSettingSave_PreviewKeyDown;
            // 
            // BtnHMSSettingExit
            // 
            BtnHMSSettingExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnHMSSettingExit.Location = new Point(697, 447);
            BtnHMSSettingExit.Name = "BtnHMSSettingExit";
            BtnHMSSettingExit.Size = new Size(75, 23);
            BtnHMSSettingExit.TabIndex = 34;
            BtnHMSSettingExit.Text = "Exit [F10]";
            BtnHMSSettingExit.UseVisualStyleBackColor = true;
            BtnHMSSettingExit.Click += BtnHMSSettingExit_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { HMSConfigErrorMsg });
            statusStrip1.Location = new Point(0, 482);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(800, 22);
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "statusStrip1";
            // 
            // HMSConfigErrorMsg
            // 
            HMSConfigErrorMsg.Name = "HMSConfigErrorMsg";
            HMSConfigErrorMsg.Size = new Size(25, 17);
            HMSConfigErrorMsg.Text = "      ";
            // 
            // FormHospitalSettings
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 504);
            Controls.Add(statusStrip1);
            Controls.Add(BtnHMSSettingExit);
            Controls.Add(BtnHMSSettingSave);
            Controls.Add(BtnHMSSettingCancel);
            Controls.Add(TabControlHospitalSetting);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormHospitalSettings";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Hospital Settings";
            FormClosing += FormHospitalSettings_FormClosing;
            Load += FormHospitalSettings_Load;
            Controls.SetChildIndex(TabControlHospitalSetting, 0);
            Controls.SetChildIndex(BtnHMSSettingCancel, 0);
            Controls.SetChildIndex(BtnHMSSettingSave, 0);
            Controls.SetChildIndex(BtnHMSSettingExit, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            TabControlHospitalSetting.ResumeLayout(false);
            TabOpRegistration.ResumeLayout(false);
            TabOpRegistration.PerformLayout();
            TabIpRegistration.ResumeLayout(false);
            TabIpRegistration.PerformLayout();
            TabPrinterSetup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)GridViewHmsYear).EndInit();
            TabDefaultAccounts.ResumeLayout(false);
            TabDefaultAccounts.PerformLayout();
            TapInvoiceGroup.ResumeLayout(false);
            TapInvoiceGroup.PerformLayout();
            GroupBoxInvoiceGroup.ResumeLayout(false);
            GroupBoxInvoiceGroup.PerformLayout();
            TabControlPrescriptionSetup.ResumeLayout(false);
            TabControlPrescriptionSetup.PerformLayout();
            TabControlMedicalTestSetup.ResumeLayout(false);
            TabControlMedicalTestSetup.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TabControl TabControlHospitalSetting;
        private System.Windows.Forms.TabPage TabOpRegistration;
        private System.Windows.Forms.Button BtnHMSSettingCancel;
        private System.Windows.Forms.Button BtnHMSSettingSave;
        private System.Windows.Forms.Button BtnHMSSettingExit;
        private System.Windows.Forms.Label label1;
        private controls.ComboBoxSwapTextBox ComboBoxOpRegistrationAccount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage TabIpRegistration;
        private controls.ComboBoxSwapTextBox ComboBoxIpRegistrationAccount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private controls.text.CurrencyTextBox TextBoxRegistrationFee;
        private controls.text.CurrencyTextBox TextBoxIpRegistrationFee;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel HMSConfigErrorMsg;
        private System.Windows.Forms.TabPage TabPrinterSetup;
        private System.Windows.Forms.TabPage TabDefaultAccounts;
        private controls.ComboBoxSwapTextBox ComboBoxPatientPurchaseAccount;
        private System.Windows.Forms.Label UndepositedFundAccountLbl;
        private System.Windows.Forms.TabPage TapInvoiceGroup;
        private System.Windows.Forms.GroupBox GroupBoxInvoiceGroup;
        private System.Windows.Forms.ListBox ListBoxInvoiceGroup;
        private System.Windows.Forms.CheckBox CheckBoxInvoiceGroupEnable;
        private controls.text.NameTextBoxAllowSpace TextBoxInvoiceGroupName;
        private System.Windows.Forms.TextBox TextBoxInvoiceGroupDescription;
        private System.Windows.Forms.Label LabelCostCenterDescription;
        private System.Windows.Forms.Label LabelCostCenterName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckedListBox CheckedListBoxInvoiceGroup;
        private System.Windows.Forms.Button BtnUpdateInvGroup;
        private controls.ComboBoxSwapTextBox ComboBoxHmsYear;
        private controls.DataViewVerticalScroll GridViewHmsYear;
        private System.Windows.Forms.Button BtnHmsAddYear;
        private DataGridViewTextBoxColumn Type;
        private DataGridViewTextBoxColumn Prefix;
        private controls.grid.DataGridViewNumberColumn Seed;
        private DataGridViewCheckBoxColumn DailyReset;
        private DataGridViewComboBoxColumn PrintType;
        private DataGridViewCheckBoxColumn IsDotMatrix;
        private controls.grid.DataGridViewCurrencyColumn RoundOff;
        private DataGridViewTextBoxColumn TypeId;
        private DataGridViewCheckBoxColumn HasRoundOff;
        private DataGridViewCheckBoxColumn HasPrinterSetup;
        private DataGridViewCheckBoxColumn HasDotmatrix;
        private Button BtnCompanyLogo;
        private TabPage TabControlPrescriptionSetup;
        private CheckBox CheckBoxDigitalSingnature;
        private CheckBox CheckBoxSingnatureName;
        private Label label5;
        private TabPage TabControlMedicalTestSetup;
        private TextBox TextBoxFooterDetailsLabtest;
        private CheckBox CheckBoxFooterDetailsLabtest;
        private TextBox TextBoxFooterDetailsPresc;
        private CheckBox CheckBoxFooterDetailsPresc;
        private Label label6;
        private CheckBox checkBoxLabTechnicianSignName;
        private CheckBox checkBoxLabtestDigSignature;
    }
}