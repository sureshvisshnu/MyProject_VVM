namespace fa.views.hms.op
{
    partial class FormOPRegistration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOPRegistration));
            toolStripOpRegistration = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            TextBoxOpSearch = new ToolStripTextBox();
            BtnOpSearch = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripLabel2 = new ToolStripLabel();
            ComboBoxOpStatus = new ToolStripComboBox();
            statusStrip1 = new StatusStrip();
            OpRegistrationErrorMsg = new ToolStripStatusLabel();
            BtnPatientSearch = new Button();
            BtnCancel = new Button();
            BtnSave = new Button();
            BtnNewPatient = new Button();
            GroupBoxOutpatientRegister = new GroupBox();
            ComboBoxSelectInsurance = new controls.ComboBoxSwapTextBox();
            ComboBoxSwapTextBoxOpRegistrationDepartment = new controls.ComboBoxSwapTextBox();
            ComboBoxSwapTextBoxOpRegistrationConsultant = new controls.ComboBoxSwapTextBox();
            LabelSelectInsurance = new Label();
            label12 = new Label();
            TextBoxAddress = new TextBox();
            label5 = new Label();
            CheckBoxBillToInsurance = new CheckBox();
            LabelInsurance = new Label();
            LinkLabelChangePatientInfo = new LinkLabel();
            FeeGroupBox1 = new GroupBox();
            TextBoxOpRegistrationAmountReceived = new controls.text.CurrencyTextBox();
            label11 = new Label();
            LabelOpRegFee = new Label();
            CheckBoxOpRegistrationNoFeeReceived = new CheckBox();
            BtnDelete = new Button();
            BtnNew = new Button();
            btnprintOpRegistration = new Button();
            PatientPhoto = new controls.PatientPhoto();
            PatientNumberOp = new controls.PatientNumberControl();
            BtnSwitchToIP = new Button();
            LabelTokenNumber = new Label();
            TextBoxOpRegistrationReasonForVisit = new TextBox();
            label6 = new Label();
            label8 = new Label();
            label7 = new Label();
            RbtOpRegistrationGender = new controls.GenderRadio();
            label4 = new Label();
            TextBoxOpRegistrationAge = new TextBox();
            TextBoxOpRegistrationDOB = new TextBox();
            label3 = new Label();
            label2 = new Label();
            TextBoxOpRegistrationName = new TextBox();
            label1 = new Label();
            label9 = new Label();
            GroupBoxOpRegistrationAddress = new controls.text.AddressGroupBoxWithoutLandMark();
            TextBoxPatientId = new TextBox();
            TextBoxOpRegistrationId = new TextBox();
            OpQueueGrid = new controls.hms.OPQueueGrid();
            toolStripOpRegistration.SuspendLayout();
            statusStrip1.SuspendLayout();
            GroupBoxOutpatientRegister.SuspendLayout();
            FeeGroupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // PatientIdTransport
            // 
            PatientIdTransport.Location = new Point(12, 812);
            PatientIdTransport.Size = new Size(100, 21);
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(172, 215);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(172, 189);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(172, 163);
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // checkBoxIsPatient
            // 
            checkBoxIsPatient.Size = new Size(69, 17);
            // 
            // toolStripOpRegistration
            // 
            toolStripOpRegistration.BackColor = SystemColors.ControlLight;
            toolStripOpRegistration.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripOpRegistration.GripStyle = ToolStripGripStyle.Hidden;
            toolStripOpRegistration.ImageScalingSize = new Size(20, 20);
            toolStripOpRegistration.Items.AddRange(new ToolStripItem[] { toolStripLabel1, TextBoxOpSearch, BtnOpSearch, toolStripSeparator1, toolStripLabel2, ComboBoxOpStatus });
            toolStripOpRegistration.Location = new Point(0, 0);
            toolStripOpRegistration.Name = "toolStripOpRegistration";
            toolStripOpRegistration.Padding = new Padding(3, 5, 3, 3);
            toolStripOpRegistration.Size = new Size(1345, 31);
            toolStripOpRegistration.TabIndex = 0;
            toolStripOpRegistration.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(44, 20);
            toolStripLabel1.Text = "Search";
            // 
            // TextBoxOpSearch
            // 
            TextBoxOpSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxOpSearch.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxOpSearch.MaxLength = 63;
            TextBoxOpSearch.Name = "TextBoxOpSearch";
            TextBoxOpSearch.Size = new Size(250, 23);
            TextBoxOpSearch.KeyDown += TextBoxOpSearch_KeyDown;
            // 
            // BtnOpSearch
            // 
            BtnOpSearch.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnOpSearch.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnOpSearch.Image = (Image)resources.GetObject("BtnOpSearch.Image");
            BtnOpSearch.ImageTransparentColor = Color.Magenta;
            BtnOpSearch.Name = "BtnOpSearch";
            BtnOpSearch.Size = new Size(24, 20);
            BtnOpSearch.Text = "Go";
            BtnOpSearch.Click += BtnOpSearch_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 23);
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(38, 20);
            toolStripLabel2.Text = "Status";
            // 
            // ComboBoxOpStatus
            // 
            ComboBoxOpStatus.AutoCompleteCustomSource.AddRange(new string[] { "Open", "Finished", "" });
            ComboBoxOpStatus.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxOpStatus.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxOpStatus.FlatStyle = FlatStyle.Standard;
            ComboBoxOpStatus.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ComboBoxOpStatus.Items.AddRange(new object[] { "All", "Open", "Completed" });
            ComboBoxOpStatus.Name = "ComboBoxOpStatus";
            ComboBoxOpStatus.Size = new Size(121, 23);
            ComboBoxOpStatus.SelectedIndexChanged += ComboBoxOpStatus_SelectedIndexChanged;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { OpRegistrationErrorMsg });
            statusStrip1.Location = new Point(0, 581);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1345, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // OpRegistrationErrorMsg
            // 
            OpRegistrationErrorMsg.BackColor = SystemColors.Control;
            OpRegistrationErrorMsg.Name = "OpRegistrationErrorMsg";
            OpRegistrationErrorMsg.Size = new Size(0, 17);
            // 
            // BtnPatientSearch
            // 
            BtnPatientSearch.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPatientSearch.Location = new Point(312, 32);
            BtnPatientSearch.Name = "BtnPatientSearch";
            BtnPatientSearch.Size = new Size(129, 23);
            BtnPatientSearch.TabIndex = 2;
            BtnPatientSearch.Text = "Patient Search [F2]";
            BtnPatientSearch.UseVisualStyleBackColor = true;
            BtnPatientSearch.Click += BtnPatientSearch_Click;
            BtnPatientSearch.PreviewKeyDown += BtnPatientSearch_PreviewKeyDown;
            // 
            // BtnCancel
            // 
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(584, 491);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(84, 23);
            BtnCancel.TabIndex = 17;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // BtnSave
            // 
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(674, 491);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(70, 23);
            BtnSave.TabIndex = 12;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            BtnSave.PreviewKeyDown += BtnSave_PreviewKeyDown;
            // 
            // BtnNewPatient
            // 
            BtnNewPatient.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnNewPatient.Location = new Point(447, 32);
            BtnNewPatient.Name = "BtnNewPatient";
            BtnNewPatient.Size = new Size(119, 23);
            BtnNewPatient.TabIndex = 3;
            BtnNewPatient.Text = "New Patient [F3]";
            BtnNewPatient.UseVisualStyleBackColor = true;
            BtnNewPatient.Click += BtnNewPatient_Click;
            // 
            // GroupBoxOutpatientRegister
            // 
            GroupBoxOutpatientRegister.BackColor = SystemColors.Control;
            GroupBoxOutpatientRegister.Controls.Add(ComboBoxSwapTextBoxOpRegistrationDepartment);
            GroupBoxOutpatientRegister.Controls.Add(ComboBoxSelectInsurance);
            GroupBoxOutpatientRegister.Controls.Add(ComboBoxSwapTextBoxOpRegistrationConsultant);
            GroupBoxOutpatientRegister.Controls.Add(LabelSelectInsurance);
            GroupBoxOutpatientRegister.Controls.Add(label12);
            GroupBoxOutpatientRegister.Controls.Add(TextBoxAddress);
            GroupBoxOutpatientRegister.Controls.Add(label5);
            GroupBoxOutpatientRegister.Controls.Add(CheckBoxBillToInsurance);
            GroupBoxOutpatientRegister.Controls.Add(LabelInsurance);
            GroupBoxOutpatientRegister.Controls.Add(LinkLabelChangePatientInfo);
            GroupBoxOutpatientRegister.Controls.Add(FeeGroupBox1);
            GroupBoxOutpatientRegister.Controls.Add(BtnDelete);
            GroupBoxOutpatientRegister.Controls.Add(BtnNew);
            GroupBoxOutpatientRegister.Controls.Add(btnprintOpRegistration);
            GroupBoxOutpatientRegister.Controls.Add(PatientPhoto);
            GroupBoxOutpatientRegister.Controls.Add(PatientNumberOp);
            GroupBoxOutpatientRegister.Controls.Add(BtnSwitchToIP);
            GroupBoxOutpatientRegister.Controls.Add(LabelTokenNumber);
            GroupBoxOutpatientRegister.Controls.Add(BtnSave);
            GroupBoxOutpatientRegister.Controls.Add(BtnCancel);
            GroupBoxOutpatientRegister.Controls.Add(BtnNewPatient);
            GroupBoxOutpatientRegister.Controls.Add(TextBoxOpRegistrationReasonForVisit);
            GroupBoxOutpatientRegister.Controls.Add(BtnPatientSearch);
            GroupBoxOutpatientRegister.Controls.Add(label6);
            GroupBoxOutpatientRegister.Controls.Add(label8);
            GroupBoxOutpatientRegister.Controls.Add(label7);
            GroupBoxOutpatientRegister.Controls.Add(RbtOpRegistrationGender);
            GroupBoxOutpatientRegister.Controls.Add(label4);
            GroupBoxOutpatientRegister.Controls.Add(TextBoxOpRegistrationAge);
            GroupBoxOutpatientRegister.Controls.Add(TextBoxOpRegistrationDOB);
            GroupBoxOutpatientRegister.Controls.Add(label3);
            GroupBoxOutpatientRegister.Controls.Add(label2);
            GroupBoxOutpatientRegister.Controls.Add(TextBoxOpRegistrationName);
            GroupBoxOutpatientRegister.Controls.Add(label1);
            GroupBoxOutpatientRegister.Controls.Add(label9);
            GroupBoxOutpatientRegister.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            GroupBoxOutpatientRegister.Location = new Point(563, 34);
            GroupBoxOutpatientRegister.Name = "GroupBoxOutpatientRegister";
            GroupBoxOutpatientRegister.Size = new Size(768, 535);
            GroupBoxOutpatientRegister.TabIndex = 0;
            GroupBoxOutpatientRegister.TabStop = false;
            // 
            // ComboBoxSelectInsurance
            // 
            ComboBoxSelectInsurance.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxSelectInsurance.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxSelectInsurance.FormattingEnabled = true;
            ComboBoxSelectInsurance.Location = new Point(14, 275);
            ComboBoxSelectInsurance.Name = "ComboBoxSelectInsurance";
            ComboBoxSelectInsurance.Size = new Size(361, 21);
            ComboBoxSelectInsurance.TabIndex = 5;
            ComboBoxSelectInsurance.TxtVisible = true;
            ComboBoxSelectInsurance.Visible = false;
            // 
            // ComboBoxSwapTextBoxOpRegistrationDepartment
            // 
            ComboBoxSwapTextBoxOpRegistrationDepartment.FormattingEnabled = true;
            ComboBoxSwapTextBoxOpRegistrationDepartment.Location = new Point(14, 365);
            ComboBoxSwapTextBoxOpRegistrationDepartment.Name = "ComboBoxSwapTextBoxOpRegistrationDepartment";
            ComboBoxSwapTextBoxOpRegistrationDepartment.Size = new Size(361, 21);
            ComboBoxSwapTextBoxOpRegistrationDepartment.TabIndex = 7;
            ComboBoxSwapTextBoxOpRegistrationDepartment.TxtVisible = true;
            ComboBoxSwapTextBoxOpRegistrationDepartment.Visible = false;
            // 
            // ComboBoxSwapTextBoxOpRegistrationConsultant
            // 
            ComboBoxSwapTextBoxOpRegistrationConsultant.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxSwapTextBoxOpRegistrationConsultant.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxSwapTextBoxOpRegistrationConsultant.FormattingEnabled = true;
            ComboBoxSwapTextBoxOpRegistrationConsultant.Location = new Point(14, 322);
            ComboBoxSwapTextBoxOpRegistrationConsultant.Name = "ComboBoxSwapTextBoxOpRegistrationConsultant";
            ComboBoxSwapTextBoxOpRegistrationConsultant.Size = new Size(361, 21);
            ComboBoxSwapTextBoxOpRegistrationConsultant.TabIndex = 6;
            ComboBoxSwapTextBoxOpRegistrationConsultant.TxtVisible = true;
            ComboBoxSwapTextBoxOpRegistrationConsultant.SelectedIndexChanged += ComboBoxSwapTextBoxConsultant_SelectedIndexChanged;
            ComboBoxSwapTextBoxOpRegistrationConsultant.TextChanged += ComboBoxSwapTextBoxConsultant_SelectedIndexChanged;
            ComboBoxSwapTextBoxOpRegistrationConsultant.KeyPress += ComboBoxSwapTextBoxOpRegistrationConsultant_KeyPress;
            // 
            // LabelSelectInsurance
            // 
            LabelSelectInsurance.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelSelectInsurance.Location = new Point(11, 256);
            LabelSelectInsurance.Name = "LabelSelectInsurance";
            LabelSelectInsurance.Size = new Size(149, 15);
            LabelSelectInsurance.TabIndex = 72;
            LabelSelectInsurance.Text = "Select Insurance to Bill";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label12.Location = new Point(11, 215);
            label12.Name = "label12";
            label12.Size = new Size(106, 13);
            label12.TabIndex = 70;
            label12.Text = "Insurance Details";
            // 
            // TextBoxAddress
            // 
            TextBoxAddress.BackColor = SystemColors.Window;
            TextBoxAddress.Location = new Point(14, 155);
            TextBoxAddress.Multiline = true;
            TextBoxAddress.Name = "TextBoxAddress";
            TextBoxAddress.ReadOnly = true;
            TextBoxAddress.Size = new Size(361, 57);
            TextBoxAddress.TabIndex = 0;
            TextBoxAddress.TabStop = false;
            // 
            // label5
            // 
            label5.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(11, 139);
            label5.Name = "label5";
            label5.Size = new Size(78, 13);
            label5.TabIndex = 68;
            label5.Text = "Address";
            // 
            // CheckBoxBillToInsurance
            // 
            CheckBoxBillToInsurance.Enabled = false;
            CheckBoxBillToInsurance.Location = new Point(14, 232);
            CheckBoxBillToInsurance.Name = "CheckBoxBillToInsurance";
            CheckBoxBillToInsurance.Size = new Size(164, 16);
            CheckBoxBillToInsurance.TabIndex = 4;
            CheckBoxBillToInsurance.Text = "Bill to Insurance";
            CheckBoxBillToInsurance.UseVisualStyleBackColor = true;
            CheckBoxBillToInsurance.CheckedChanged += CheckBoxBillToInsurance_CheckedChanged;
            // 
            // LabelInsurance
            // 
            LabelInsurance.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point);
            LabelInsurance.ForeColor = Color.ForestGreen;
            LabelInsurance.Location = new Point(580, 347);
            LabelInsurance.Name = "LabelInsurance";
            LabelInsurance.Size = new Size(178, 26);
            LabelInsurance.TabIndex = 60;
            LabelInsurance.Text = "**NO INSURANCE**";
            LabelInsurance.TextAlign = ContentAlignment.TopCenter;
            // 
            // LinkLabelChangePatientInfo
            // 
            LinkLabelChangePatientInfo.AutoSize = true;
            LinkLabelChangePatientInfo.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LinkLabelChangePatientInfo.LinkColor = Color.Black;
            LinkLabelChangePatientInfo.Location = new Point(447, 98);
            LinkLabelChangePatientInfo.Name = "LinkLabelChangePatientInfo";
            LinkLabelChangePatientInfo.Size = new Size(122, 13);
            LinkLabelChangePatientInfo.TabIndex = 59;
            LinkLabelChangePatientInfo.TabStop = true;
            LinkLabelChangePatientInfo.Text = "Change Patient Info ";
            LinkLabelChangePatientInfo.LinkClicked += LinkLabelChangePatientInfo_LinkClicked;
            // 
            // FeeGroupBox1
            // 
            FeeGroupBox1.BackColor = SystemColors.Control;
            FeeGroupBox1.Controls.Add(TextBoxOpRegistrationAmountReceived);
            FeeGroupBox1.Controls.Add(label11);
            FeeGroupBox1.Controls.Add(LabelOpRegFee);
            FeeGroupBox1.Controls.Add(CheckBoxOpRegistrationNoFeeReceived);
            FeeGroupBox1.Location = new Point(387, 304);
            FeeGroupBox1.Name = "FeeGroupBox1";
            FeeGroupBox1.Size = new Size(175, 174);
            FeeGroupBox1.TabIndex = 9;
            FeeGroupBox1.TabStop = false;
            FeeGroupBox1.Text = "Registration Fee";
            // 
            // TextBoxOpRegistrationAmountReceived
            // 
            TextBoxOpRegistrationAmountReceived.Decimals = 2;
            TextBoxOpRegistrationAmountReceived.Length = 10;
            TextBoxOpRegistrationAmountReceived.Location = new Point(11, 119);
            TextBoxOpRegistrationAmountReceived.Name = "TextBoxOpRegistrationAmountReceived";
            TextBoxOpRegistrationAmountReceived.Size = new Size(130, 21);
            TextBoxOpRegistrationAmountReceived.TabIndex = 10;
            TextBoxOpRegistrationAmountReceived.Text = "0.00";
            TextBoxOpRegistrationAmountReceived.TextAlign = HorizontalAlignment.Right;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label11.Location = new Point(8, 102);
            label11.Name = "label11";
            label11.Size = new Size(107, 13);
            label11.TabIndex = 0;
            label11.Text = "Amount Received";
            // 
            // LabelOpRegFee
            // 
            LabelOpRegFee.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelOpRegFee.Location = new Point(8, 30);
            LabelOpRegFee.Name = "LabelOpRegFee";
            LabelOpRegFee.Size = new Size(155, 68);
            LabelOpRegFee.TabIndex = 0;
            LabelOpRegFee.Text = "Registration Fee of {XX} {Currency} is required to register an Out Patient";
            // 
            // CheckBoxOpRegistrationNoFeeReceived
            // 
            CheckBoxOpRegistrationNoFeeReceived.AutoSize = true;
            CheckBoxOpRegistrationNoFeeReceived.Location = new Point(11, 145);
            CheckBoxOpRegistrationNoFeeReceived.Name = "CheckBoxOpRegistrationNoFeeReceived";
            CheckBoxOpRegistrationNoFeeReceived.Size = new Size(83, 17);
            CheckBoxOpRegistrationNoFeeReceived.TabIndex = 11;
            CheckBoxOpRegistrationNoFeeReceived.Text = "Fee Waived";
            CheckBoxOpRegistrationNoFeeReceived.UseVisualStyleBackColor = true;
            CheckBoxOpRegistrationNoFeeReceived.CheckedChanged += CheckBoxOpRegistrationNoFeeReceived_CheckedChanged;
            // 
            // BtnDelete
            // 
            BtnDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnDelete.Location = new Point(95, 491);
            BtnDelete.Name = "BtnDelete";
            BtnDelete.Size = new Size(90, 23);
            BtnDelete.TabIndex = 20;
            BtnDelete.Text = "Delete [F4]";
            BtnDelete.UseVisualStyleBackColor = true;
            BtnDelete.Click += BtnDelete_Click;
            // 
            // BtnNew
            // 
            BtnNew.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnNew.Location = new Point(14, 491);
            BtnNew.Name = "BtnNew";
            BtnNew.Size = new Size(75, 23);
            BtnNew.TabIndex = 21;
            BtnNew.Text = "New [F3]";
            BtnNew.UseVisualStyleBackColor = true;
            BtnNew.Click += BtnNew_Click;
            // 
            // btnprintOpRegistration
            // 
            btnprintOpRegistration.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            btnprintOpRegistration.Location = new Point(495, 491);
            btnprintOpRegistration.Name = "btnprintOpRegistration";
            btnprintOpRegistration.Size = new Size(84, 23);
            btnprintOpRegistration.TabIndex = 18;
            btnprintOpRegistration.Text = "Print [F9]";
            btnprintOpRegistration.UseVisualStyleBackColor = true;
            btnprintOpRegistration.Click += BtnPrintOpRegistration_Click;
            // 
            // PatientPhoto
            // 
            PatientPhoto.Location = new Point(574, 38);
            PatientPhoto.Margin = new Padding(5, 3, 5, 3);
            PatientPhoto.Name = "PatientPhoto";
            PatientPhoto.Size = new Size(187, 239);
            PatientPhoto.TabIndex = 0;
            // 
            // PatientNumberOp
            // 
            PatientNumberOp.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            PatientNumberOp.Location = new Point(577, 275);
            PatientNumberOp.Margin = new Padding(4, 3, 4, 3);
            PatientNumberOp.Name = "PatientNumberOp";
            PatientNumberOp.PatientNumber = "";
            PatientNumberOp.Size = new Size(208, 68);
            PatientNumberOp.TabIndex = 52;
            // 
            // BtnSwitchToIP
            // 
            BtnSwitchToIP.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSwitchToIP.Location = new Point(191, 491);
            BtnSwitchToIP.Name = "BtnSwitchToIP";
            BtnSwitchToIP.Size = new Size(84, 23);
            BtnSwitchToIP.TabIndex = 19;
            BtnSwitchToIP.Text = "Admit [F6]";
            BtnSwitchToIP.UseVisualStyleBackColor = true;
            BtnSwitchToIP.Click += BtnSwitchToIP_Click;
            // 
            // LabelTokenNumber
            // 
            LabelTokenNumber.BackColor = SystemColors.Window;
            LabelTokenNumber.BorderStyle = BorderStyle.FixedSingle;
            LabelTokenNumber.Font = new Font("Tahoma", 36F, FontStyle.Regular, GraphicsUnit.Point);
            LabelTokenNumber.Location = new Point(580, 402);
            LabelTokenNumber.Name = "LabelTokenNumber";
            LabelTokenNumber.Size = new Size(171, 71);
            LabelTokenNumber.TabIndex = 51;
            LabelTokenNumber.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // TextBoxOpRegistrationReasonForVisit
            // 
            TextBoxOpRegistrationReasonForVisit.BackColor = Color.White;
            TextBoxOpRegistrationReasonForVisit.Location = new Point(14, 407);
            TextBoxOpRegistrationReasonForVisit.MaxLength = 310;
            TextBoxOpRegistrationReasonForVisit.Multiline = true;
            TextBoxOpRegistrationReasonForVisit.Name = "TextBoxOpRegistrationReasonForVisit";
            TextBoxOpRegistrationReasonForVisit.ReadOnly = true;
            TextBoxOpRegistrationReasonForVisit.Size = new Size(361, 71);
            TextBoxOpRegistrationReasonForVisit.TabIndex = 8;
            TextBoxOpRegistrationReasonForVisit.Click += TextBoxOpRegistrationReasonForVisit_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label6.Location = new Point(11, 389);
            label6.Name = "label6";
            label6.Size = new Size(97, 13);
            label6.TabIndex = 40;
            label6.Text = "Reason For Visit";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label8.Location = new Point(11, 347);
            label8.Name = "label8";
            label8.Size = new Size(64, 13);
            label8.TabIndex = 42;
            label8.Text = "Department";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label7.Location = new Point(11, 304);
            label7.Name = "label7";
            label7.Size = new Size(95, 13);
            label7.TabIndex = 41;
            label7.Text = "Doctor/Consultant";
            // 
            // RbtOpRegistrationGender
            // 
            RbtOpRegistrationGender.BackColor = SystemColors.Control;
            RbtOpRegistrationGender.Enabled = false;
            RbtOpRegistrationGender.Gender = controls.GenderSelection.None;
            RbtOpRegistrationGender.Location = new Point(14, 86);
            RbtOpRegistrationGender.Margin = new Padding(5, 3, 5, 3);
            RbtOpRegistrationGender.Name = "RbtOpRegistrationGender";
            RbtOpRegistrationGender.Size = new Size(208, 25);
            RbtOpRegistrationGender.TabIndex = 0;
            RbtOpRegistrationGender.TabStop = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(11, 58);
            label4.Name = "label4";
            label4.Size = new Size(48, 13);
            label4.TabIndex = 37;
            label4.Text = "Gender";
            // 
            // TextBoxOpRegistrationAge
            // 
            TextBoxOpRegistrationAge.BackColor = SystemColors.Window;
            TextBoxOpRegistrationAge.Location = new Point(134, 114);
            TextBoxOpRegistrationAge.Name = "TextBoxOpRegistrationAge";
            TextBoxOpRegistrationAge.ReadOnly = true;
            TextBoxOpRegistrationAge.Size = new Size(26, 21);
            TextBoxOpRegistrationAge.TabIndex = 0;
            TextBoxOpRegistrationAge.TabStop = false;
            TextBoxOpRegistrationAge.Click += TextBoxOpRegistrationAge_Click;
            // 
            // TextBoxOpRegistrationDOB
            // 
            TextBoxOpRegistrationDOB.BackColor = SystemColors.Window;
            TextBoxOpRegistrationDOB.Location = new Point(14, 114);
            TextBoxOpRegistrationDOB.Name = "TextBoxOpRegistrationDOB";
            TextBoxOpRegistrationDOB.ReadOnly = true;
            TextBoxOpRegistrationDOB.Size = new Size(100, 21);
            TextBoxOpRegistrationDOB.TabIndex = 0;
            TextBoxOpRegistrationDOB.TabStop = false;
            TextBoxOpRegistrationDOB.Click += TextBoxOpRegistrationDOB_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(131, 96);
            label3.Name = "label3";
            label3.Size = new Size(29, 13);
            label3.TabIndex = 0;
            label3.Text = "Age";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(11, 96);
            label2.Name = "label2";
            label2.Size = new Size(30, 13);
            label2.TabIndex = 0;
            label2.Text = "DOB";
            // 
            // TextBoxOpRegistrationName
            // 
            TextBoxOpRegistrationName.BackColor = SystemColors.Window;
            TextBoxOpRegistrationName.Location = new Point(14, 33);
            TextBoxOpRegistrationName.MaxLength = 63;
            TextBoxOpRegistrationName.Name = "TextBoxOpRegistrationName";
            TextBoxOpRegistrationName.ReadOnly = true;
            TextBoxOpRegistrationName.Size = new Size(292, 21);
            TextBoxOpRegistrationName.TabIndex = 1;
            TextBoxOpRegistrationName.TabStop = false;
            TextBoxOpRegistrationName.Click += TextBoxOpRegistrationName_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(11, 17);
            label1.Name = "label1";
            label1.Size = new Size(39, 13);
            label1.TabIndex = 0;
            label1.Text = "Name";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label9.Location = new Point(580, 389);
            label9.Name = "label9";
            label9.Size = new Size(89, 13);
            label9.TabIndex = 50;
            label9.Text = "Token Number";
            // 
            // GroupBoxOpRegistrationAddress
            // 
            GroupBoxOpRegistrationAddress.AddressLine1 = "";
            GroupBoxOpRegistrationAddress.BackColor = SystemColors.Control;
            GroupBoxOpRegistrationAddress.CityName = "";
            GroupBoxOpRegistrationAddress.DistrictName = "";
            GroupBoxOpRegistrationAddress.GroupName = "";
            GroupBoxOpRegistrationAddress.Location = new Point(710, 622);
            GroupBoxOpRegistrationAddress.Margin = new Padding(4, 3, 4, 3);
            GroupBoxOpRegistrationAddress.Name = "GroupBoxOpRegistrationAddress";
            GroupBoxOpRegistrationAddress.PinCode = "";
            GroupBoxOpRegistrationAddress.ReadOnly = true;
            GroupBoxOpRegistrationAddress.Size = new Size(560, 158);
            GroupBoxOpRegistrationAddress.StateName = "";
            GroupBoxOpRegistrationAddress.TabIndex = 3;
            // 
            // TextBoxPatientId
            // 
            TextBoxPatientId.Location = new Point(498, 622);
            TextBoxPatientId.Name = "TextBoxPatientId";
            TextBoxPatientId.Size = new Size(100, 21);
            TextBoxPatientId.TabIndex = 53;
            TextBoxPatientId.TabStop = false;
            TextBoxPatientId.Visible = false;
            // 
            // TextBoxOpRegistrationId
            // 
            TextBoxOpRegistrationId.Location = new Point(604, 622);
            TextBoxOpRegistrationId.Name = "TextBoxOpRegistrationId";
            TextBoxOpRegistrationId.Size = new Size(100, 21);
            TextBoxOpRegistrationId.TabIndex = 21;
            TextBoxOpRegistrationId.TabStop = false;
            TextBoxOpRegistrationId.Visible = false;
            // 
            // OpQueueGrid
            // 
            OpQueueGrid.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            OpQueueGrid.IsDoctor = false;
            OpQueueGrid.IsMyQueue = true;
            OpQueueGrid.Location = new Point(16, 41);
            OpQueueGrid.Margin = new Padding(4, 3, 4, 3);
            OpQueueGrid.Name = "OpQueueGrid";
            OpQueueGrid.OpId = null;
            OpQueueGrid.OPStatus = model.Hms.Op.Status.ALL;
            OpQueueGrid.SearchString = "";
            OpQueueGrid.SingleClickSelection = true;
            OpQueueGrid.Size = new Size(530, 528);
            OpQueueGrid.TabIndex = 0;
            OpQueueGrid.Click += OpQueueGrid_DoubleClick;
            OpQueueGrid.KeyDown += OpQueueGrid_KeyDown;
            OpQueueGrid.KeyUp += OpQueueGrid_KeyUp;
            OpQueueGrid.PreviewKeyDown += OpQueueGrid_PreviewKeyDown;
            // 
            // FormOPRegistration
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1345, 603);
            Controls.Add(OpQueueGrid);
            Controls.Add(GroupBoxOutpatientRegister);
            Controls.Add(statusStrip1);
            Controls.Add(toolStripOpRegistration);
            Controls.Add(TextBoxPatientId);
            Controls.Add(GroupBoxOpRegistrationAddress);
            Controls.Add(TextBoxOpRegistrationId);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormOPRegistration";
            StartPosition = FormStartPosition.CenterParent;
            Text = "O.P.Registration";
            FormClosing += FormOPRegistration_FormClosing;
            Load += FormOPRegistration_Load;
            Controls.SetChildIndex(checkBoxIsPatient, 0);
            Controls.SetChildIndex(TextBoxOpRegistrationId, 0);
            Controls.SetChildIndex(GroupBoxOpRegistrationAddress, 0);
            Controls.SetChildIndex(TextBoxPatientId, 0);
            Controls.SetChildIndex(toolStripOpRegistration, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(GroupBoxOutpatientRegister, 0);
            Controls.SetChildIndex(OpQueueGrid, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            toolStripOpRegistration.ResumeLayout(false);
            toolStripOpRegistration.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            GroupBoxOutpatientRegister.ResumeLayout(false);
            GroupBoxOutpatientRegister.PerformLayout();
            FeeGroupBox1.ResumeLayout(false);
            FeeGroupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripOpRegistration;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox TextBoxOpSearch;
        private System.Windows.Forms.ToolStripButton BtnOpSearch;
        private System.Windows.Forms.ToolStripComboBox ComboBoxOpStatus;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Button BtnPatientSearch;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button BtnNewPatient;
        private System.Windows.Forms.GroupBox GroupBoxOutpatientRegister;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxOpRegistrationName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextBoxOpRegistrationAge;
        private System.Windows.Forms.TextBox TextBoxOpRegistrationDOB;
        private System.Windows.Forms.Label label4;
        private controls.GenderRadio RbtOpRegistrationGender;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TextBoxOpRegistrationReasonForVisit;
        private controls.ComboBoxSwapTextBox ComboBoxSwapTextBoxOpRegistrationConsultant;
        private controls.ComboBoxSwapTextBox ComboBoxSwapTextBoxOpRegistrationDepartment;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label LabelTokenNumber;
        private System.Windows.Forms.Button BtnSwitchToIP;
        private System.Windows.Forms.ToolStripStatusLabel OpRegistrationErrorMsg;
        public System.Windows.Forms.TextBox TextBoxOpRegistrationId;
        private controls.PatientNumberControl PatientNumberOp;
        private controls.hms.OPQueueGrid OpQueueGrid;
        private System.Windows.Forms.TextBox TextBoxPatientId;
        private controls.PatientPhoto PatientPhoto;
        private System.Windows.Forms.Button btnprintOpRegistration;
        private controls.text.AddressGroupBoxWithoutLandMark GroupBoxOpRegistrationAddress;
        private System.Windows.Forms.Button BtnDelete;
        private System.Windows.Forms.Button BtnNew;
        private System.Windows.Forms.CheckBox CheckBoxOpRegistrationNoFeeReceived;
        private System.Windows.Forms.Label LabelOpRegFee;
        private System.Windows.Forms.GroupBox FeeGroupBox1;
        private System.Windows.Forms.Label label11;
        private controls.text.CurrencyTextBox TextBoxOpRegistrationAmountReceived;
        private System.Windows.Forms.LinkLabel LinkLabelChangePatientInfo;
        private System.Windows.Forms.CheckBox CheckBoxBillToInsurance;
        private controls.ComboBoxSwapTextBox ComboBoxSelectInsurance;
        private System.Windows.Forms.Label LabelSelectInsurance;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox TextBoxAddress;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label LabelInsurance;
    }
}