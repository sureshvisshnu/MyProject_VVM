namespace fa.views.employee
{
    partial class FormEmployee
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmployee));
            contextMenuStrip2 = new ContextMenuStrip(components);
            newEmployeeToolStripMenuItem1 = new ToolStripMenuItem();
            contextMenuStrip1 = new ContextMenuStrip(components);
            newDepartmentToolStripMenuItem = new ToolStripMenuItem();
            newEmployeeToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            EmployeeDepartmentErrorMsg = new ToolStripStatusLabel();
            TabControlEmployee = new TabControl();
            TabPageEmployeeDetails = new TabPage();
            comboBoxSwapTextBoxEmployeeDepartment = new controls.ComboBoxSwapTextBox();
            ComboBoxSwapTextBoxEmployeeTitle = new controls.ComboBoxSwapTextBox();
            checkBoxIsServiceProvider = new CheckBox();
            CameraPanel = new Panel();
            CameraPhotoTaker = new Fa.views.controls.Photo.CameraControl();
            AddressGroupBoxEmployee = new controls.AddressGroupBoxWithStateSelection();
            label5 = new Label();
            DateTimePickerEmployee = new controls.text.DateWithCalendar();
            label4 = new Label();
            BtnAddTitle = new Button();
            label2 = new Label();
            label1 = new Label();
            TextBoxEmployeeLegalName = new TextBox();
            TextBoxEmployeeName = new TextBox();
            LabelCompanyLegalname = new Label();
            LabelCompanyName = new Label();
            TabPageContactDetail = new TabPage();
            TextBoxEmployeeMobile = new controls.text.PhoneTextBox();
            TextBoxEmployeePhone = new controls.text.PhoneTextBox();
            TextBoxEmployeePan = new TextBox();
            TextBoxEmployeeFax = new MaskedTextBox();
            TextBoxEmployeeEmail = new TextBox();
            label11 = new Label();
            label10 = new Label();
            label9 = new Label();
            label8 = new Label();
            label17 = new Label();
            TabPageSignatureDetails = new TabPage();
            BtnDigitalSignatureDeletePresc = new Button();
            CheckBoxAllowDigtalsignature = new CheckBox();
            DigitalSignaturePictureBoxPresc = new PictureBox();
            CheckBoxRxSymbolPresc = new CheckBox();
            BtnDigitalSignatureImpoortPresc = new Button();
            label3 = new Label();
            TreeViewEmployeeDepartment = new TreeView();
            ImageListEmployeeDepartment = new ImageList(components);
            BtnDelete = new Button();
            BtnExit = new Button();
            TabControlDepartment = new TabControl();
            TabPageDepartmentDetails = new TabPage();
            comboBoxSwapTextBoxParentDepartment = new controls.ComboBoxSwapTextBox();
            TextBoxDepartmentDescription = new TextBox();
            LabelCustomerDescription = new Label();
            TextBoxDepartmentDisplayAs = new TextBox();
            TextBoxDepartmentName = new TextBox();
            label14 = new Label();
            label15 = new Label();
            LabelParentDepartment = new Label();
            BtnDepartmentSave = new Button();
            BtnDepartmentCancel = new Button();
            BtnDepartmentEdit = new Button();
            TextBoxEmpDepId = new TextBox();
            TextBoxSearch = new controls.text.DelayedTextChangeTextBox();
            BtnNew = new Dropdown_Button.UserControlButtonWithMenu();
            contextMenuStrip2.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            TabControlEmployee.SuspendLayout();
            TabPageEmployeeDetails.SuspendLayout();
            CameraPanel.SuspendLayout();
            TabPageContactDetail.SuspendLayout();
            TabPageSignatureDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DigitalSignaturePictureBoxPresc).BeginInit();
            TabControlDepartment.SuspendLayout();
            TabPageDepartmentDetails.SuspendLayout();
            SuspendLayout();
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
            // contextMenuStrip2
            // 
            contextMenuStrip2.Items.AddRange(new ToolStripItem[] { newEmployeeToolStripMenuItem1 });
            contextMenuStrip2.Name = "contextMenuStrip2";
            contextMenuStrip2.Size = new Size(154, 26);
            // 
            // newEmployeeToolStripMenuItem1
            // 
            newEmployeeToolStripMenuItem1.Image = (Image)resources.GetObject("newEmployeeToolStripMenuItem1.Image");
            newEmployeeToolStripMenuItem1.Name = "newEmployeeToolStripMenuItem1";
            newEmployeeToolStripMenuItem1.Size = new Size(153, 22);
            newEmployeeToolStripMenuItem1.Text = "New Employee";
            newEmployeeToolStripMenuItem1.Click += newEmployeeToolStripMenuItem_Click;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.AccessibleRole = AccessibleRole.DropList;
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { newDepartmentToolStripMenuItem, newEmployeeToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(165, 48);
            // 
            // newDepartmentToolStripMenuItem
            // 
            newDepartmentToolStripMenuItem.Image = (Image)resources.GetObject("newDepartmentToolStripMenuItem.Image");
            newDepartmentToolStripMenuItem.Name = "newDepartmentToolStripMenuItem";
            newDepartmentToolStripMenuItem.Size = new Size(164, 22);
            newDepartmentToolStripMenuItem.Text = "New Department";
            newDepartmentToolStripMenuItem.Click += newDepartmentToolStripMenuItem_Click;
            // 
            // newEmployeeToolStripMenuItem
            // 
            newEmployeeToolStripMenuItem.Image = (Image)resources.GetObject("newEmployeeToolStripMenuItem.Image");
            newEmployeeToolStripMenuItem.Name = "newEmployeeToolStripMenuItem";
            newEmployeeToolStripMenuItem.Size = new Size(164, 22);
            newEmployeeToolStripMenuItem.Text = "New Employee";
            newEmployeeToolStripMenuItem.Click += newEmployeeToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { EmployeeDepartmentErrorMsg });
            statusStrip1.Location = new Point(0, 539);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(829, 22);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // EmployeeDepartmentErrorMsg
            // 
            EmployeeDepartmentErrorMsg.Name = "EmployeeDepartmentErrorMsg";
            EmployeeDepartmentErrorMsg.Size = new Size(0, 17);
            // 
            // TabControlEmployee
            // 
            TabControlEmployee.Controls.Add(TabPageEmployeeDetails);
            TabControlEmployee.Controls.Add(TabPageContactDetail);
            TabControlEmployee.Controls.Add(TabPageSignatureDetails);
            TabControlEmployee.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TabControlEmployee.Location = new Point(257, 12);
            TabControlEmployee.Name = "TabControlEmployee";
            TabControlEmployee.SelectedIndex = 0;
            TabControlEmployee.Size = new Size(568, 474);
            TabControlEmployee.TabIndex = 4;
            TabControlEmployee.TabStop = false;
            TabControlEmployee.Visible = false;
            // 
            // TabPageEmployeeDetails
            // 
            TabPageEmployeeDetails.Controls.Add(ComboBoxSwapTextBoxEmployeeTitle);
            TabPageEmployeeDetails.Controls.Add(comboBoxSwapTextBoxEmployeeDepartment);
            TabPageEmployeeDetails.Controls.Add(checkBoxIsServiceProvider);
            TabPageEmployeeDetails.Controls.Add(CameraPanel);
            TabPageEmployeeDetails.Controls.Add(AddressGroupBoxEmployee);
            TabPageEmployeeDetails.Controls.Add(label5);
            TabPageEmployeeDetails.Controls.Add(DateTimePickerEmployee);
            TabPageEmployeeDetails.Controls.Add(label4);
            TabPageEmployeeDetails.Controls.Add(BtnAddTitle);
            TabPageEmployeeDetails.Controls.Add(label2);
            TabPageEmployeeDetails.Controls.Add(label1);
            TabPageEmployeeDetails.Controls.Add(TextBoxEmployeeLegalName);
            TabPageEmployeeDetails.Controls.Add(TextBoxEmployeeName);
            TabPageEmployeeDetails.Controls.Add(LabelCompanyLegalname);
            TabPageEmployeeDetails.Controls.Add(LabelCompanyName);
            TabPageEmployeeDetails.Location = new Point(4, 22);
            TabPageEmployeeDetails.Name = "TabPageEmployeeDetails";
            TabPageEmployeeDetails.Padding = new Padding(3);
            TabPageEmployeeDetails.Size = new Size(560, 448);
            TabPageEmployeeDetails.TabIndex = 0;
            TabPageEmployeeDetails.Text = "Employee Details";
            TabPageEmployeeDetails.UseVisualStyleBackColor = true;
            // 
            // comboBoxSwapTextBoxEmployeeDepartment
            // 
            comboBoxSwapTextBoxEmployeeDepartment.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBoxSwapTextBoxEmployeeDepartment.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBoxSwapTextBoxEmployeeDepartment.FormattingEnabled = true;
            comboBoxSwapTextBoxEmployeeDepartment.Location = new Point(18, 109);
            comboBoxSwapTextBoxEmployeeDepartment.Name = "comboBoxSwapTextBoxEmployeeDepartment";
            comboBoxSwapTextBoxEmployeeDepartment.Size = new Size(196, 21);
            comboBoxSwapTextBoxEmployeeDepartment.TabIndex = 8;
            comboBoxSwapTextBoxEmployeeDepartment.TxtVisible = true;
            comboBoxSwapTextBoxEmployeeDepartment.KeyPress += comboBoxSwapTextBoxEmployeeDepartment_KeyPress;
            // 
            // ComboBoxSwapTextBoxEmployeeTitle
            // 
            ComboBoxSwapTextBoxEmployeeTitle.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxSwapTextBoxEmployeeTitle.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxSwapTextBoxEmployeeTitle.FormattingEnabled = true;
            ComboBoxSwapTextBoxEmployeeTitle.Items.AddRange(new object[] { "dvs", "bgdfz", "vdfv" });
            ComboBoxSwapTextBoxEmployeeTitle.Location = new Point(18, 192);
            ComboBoxSwapTextBoxEmployeeTitle.Name = "ComboBoxSwapTextBoxEmployeeTitle";
            ComboBoxSwapTextBoxEmployeeTitle.Size = new Size(196, 21);
            ComboBoxSwapTextBoxEmployeeTitle.TabIndex = 10;
            ComboBoxSwapTextBoxEmployeeTitle.TxtVisible = true;
            ComboBoxSwapTextBoxEmployeeTitle.KeyPress += comboBoxSwapTextBoxEmployeeDepartment_KeyPress;
            // 
            // checkBoxIsServiceProvider
            // 
            checkBoxIsServiceProvider.AutoSize = true;
            checkBoxIsServiceProvider.Location = new Point(18, 221);
            checkBoxIsServiceProvider.Name = "checkBoxIsServiceProvider";
            checkBoxIsServiceProvider.Size = new Size(116, 17);
            checkBoxIsServiceProvider.TabIndex = 59;
            checkBoxIsServiceProvider.Text = "Is Service Provider";
            checkBoxIsServiceProvider.UseVisualStyleBackColor = true;
            // 
            // CameraPanel
            // 
            CameraPanel.Controls.Add(CameraPhotoTaker);
            CameraPanel.Location = new Point(411, 10);
            CameraPanel.Name = "CameraPanel";
            CameraPanel.Size = new Size(140, 204);
            CameraPanel.TabIndex = 58;
            // 
            // CameraPhotoTaker
            // 
            CameraPhotoTaker.Location = new Point(4, -1);
            CameraPhotoTaker.Name = "CameraPhotoTaker";
            CameraPhotoTaker.Photo = null;
            CameraPhotoTaker.Size = new Size(133, 194);
            CameraPhotoTaker.TabIndex = 57;
            // 
            // AddressGroupBoxEmployee
            // 
            AddressGroupBoxEmployee.AddressLine1 = "";
            AddressGroupBoxEmployee.AddressLine2 = "";
            AddressGroupBoxEmployee.CityName = "";
            AddressGroupBoxEmployee.CountryId = 0L;
            AddressGroupBoxEmployee.DistrictName = "";
            AddressGroupBoxEmployee.GroupName = "";
            AddressGroupBoxEmployee.Location = new Point(11, 257);
            AddressGroupBoxEmployee.Name = "AddressGroupBoxEmployee";
            AddressGroupBoxEmployee.PinCode = "";
            AddressGroupBoxEmployee.ReadOnly = true;
            AddressGroupBoxEmployee.Size = new Size(536, 186);
            AddressGroupBoxEmployee.StateId = 0L;
            AddressGroupBoxEmployee.StateName = "";
            AddressGroupBoxEmployee.TabIndex = 11;
            AddressGroupBoxEmployee.PreviewKeyDown += AddressGroupBoxEmployee_PreviewKeyDown;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(15, 244);
            label5.Name = "label5";
            label5.Size = new Size(46, 13);
            label5.TabIndex = 55;
            label5.Text = "Address";
            // 
            // DateTimePickerEmployee
            // 
            DateTimePickerEmployee.BackColor = Color.White;
            DateTimePickerEmployee.BorderStyle = BorderStyle.FixedSingle;
            DateTimePickerEmployee.Date = null;
            DateTimePickerEmployee.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DateTimePickerEmployee.Format = "MM/dd/yyyy";
            DateTimePickerEmployee.Location = new Point(18, 150);
            DateTimePickerEmployee.Margin = new Padding(4, 3, 4, 3);
            DateTimePickerEmployee.MaxDate = new DateTime(9997, 12, 31, 7, 46, 35, 0);
            DateTimePickerEmployee.MinDate = new DateTime(1900, 1, 1, 19, 5, 48, 0);
            DateTimePickerEmployee.Name = "DateTimePickerEmployee";
            DateTimePickerEmployee.ReadOnly = false;
            DateTimePickerEmployee.Size = new Size(93, 21);
            DateTimePickerEmployee.TabIndex = 9;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(15, 91);
            label4.Name = "label4";
            label4.Size = new Size(76, 13);
            label4.TabIndex = 54;
            label4.Text = "Department";
            // 
            // BtnAddTitle
            // 
            BtnAddTitle.Location = new Point(215, 191);
            BtnAddTitle.Name = "BtnAddTitle";
            BtnAddTitle.Size = new Size(50, 23);
            BtnAddTitle.TabIndex = 10;
            BtnAddTitle.TabStop = false;
            BtnAddTitle.Text = "Add";
            BtnAddTitle.UseVisualStyleBackColor = true;
            BtnAddTitle.Click += BtnAddTitle_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(15, 174);
            label2.Name = "label2";
            label2.Size = new Size(32, 13);
            label2.TabIndex = 47;
            label2.Text = "Title";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(15, 134);
            label1.Name = "label1";
            label1.Size = new Size(78, 13);
            label1.TabIndex = 46;
            label1.Text = "Date of Birth";
            // 
            // TextBoxEmployeeLegalName
            // 
            TextBoxEmployeeLegalName.BackColor = Color.White;
            TextBoxEmployeeLegalName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxEmployeeLegalName.Location = new Point(18, 67);
            TextBoxEmployeeLegalName.MaxLength = 50;
            TextBoxEmployeeLegalName.Name = "TextBoxEmployeeLegalName";
            TextBoxEmployeeLegalName.ReadOnly = true;
            TextBoxEmployeeLegalName.Size = new Size(380, 21);
            TextBoxEmployeeLegalName.TabIndex = 7;
            TextBoxEmployeeLegalName.KeyDown += TextBoxEmployeeName_KeyDown;
            TextBoxEmployeeLegalName.KeyPress += TextBoxEmployeeName_KeyPress;
            TextBoxEmployeeLegalName.MouseDown += TextBoxEmployeeLegalName_MouseDown;
            // 
            // TextBoxEmployeeName
            // 
            TextBoxEmployeeName.BackColor = Color.White;
            TextBoxEmployeeName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxEmployeeName.Location = new Point(18, 23);
            TextBoxEmployeeName.MaxLength = 20;
            TextBoxEmployeeName.Name = "TextBoxEmployeeName";
            TextBoxEmployeeName.ReadOnly = true;
            TextBoxEmployeeName.Size = new Size(380, 21);
            TextBoxEmployeeName.TabIndex = 6;
            TextBoxEmployeeName.KeyDown += TextBoxEmployeeName_KeyDown;
            TextBoxEmployeeName.KeyPress += TextBoxEmployeeName_KeyPress;
            TextBoxEmployeeName.MouseDown += TextBoxEmployeeName_MouseDown;
            TextBoxEmployeeName.PreviewKeyDown += TextBoxEmployeeName_PreviewKeyDown;
            // 
            // LabelCompanyLegalname
            // 
            LabelCompanyLegalname.AutoSize = true;
            LabelCompanyLegalname.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelCompanyLegalname.Location = new Point(15, 50);
            LabelCompanyLegalname.Name = "LabelCompanyLegalname";
            LabelCompanyLegalname.Size = new Size(56, 13);
            LabelCompanyLegalname.TabIndex = 37;
            LabelCompanyLegalname.Text = "Display As";
            // 
            // LabelCompanyName
            // 
            LabelCompanyName.AutoSize = true;
            LabelCompanyName.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelCompanyName.Location = new Point(15, 7);
            LabelCompanyName.Name = "LabelCompanyName";
            LabelCompanyName.Size = new Size(39, 13);
            LabelCompanyName.TabIndex = 36;
            LabelCompanyName.Text = "Name";
            // 
            // TabPageContactDetail
            // 
            TabPageContactDetail.Controls.Add(TextBoxEmployeeMobile);
            TabPageContactDetail.Controls.Add(TextBoxEmployeePhone);
            TabPageContactDetail.Controls.Add(TextBoxEmployeePan);
            TabPageContactDetail.Controls.Add(TextBoxEmployeeFax);
            TabPageContactDetail.Controls.Add(TextBoxEmployeeEmail);
            TabPageContactDetail.Controls.Add(label11);
            TabPageContactDetail.Controls.Add(label10);
            TabPageContactDetail.Controls.Add(label9);
            TabPageContactDetail.Controls.Add(label8);
            TabPageContactDetail.Controls.Add(label17);
            TabPageContactDetail.Location = new Point(4, 22);
            TabPageContactDetail.Name = "TabPageContactDetail";
            TabPageContactDetail.Padding = new Padding(3);
            TabPageContactDetail.Size = new Size(192, 74);
            TabPageContactDetail.TabIndex = 1;
            TabPageContactDetail.Text = "Contact and Tax Info";
            TabPageContactDetail.UseVisualStyleBackColor = true;
            // 
            // TextBoxEmployeeMobile
            // 
            TextBoxEmployeeMobile.AreaCodeLength = 5;
            TextBoxEmployeeMobile.BackColor = SystemColors.Window;
            TextBoxEmployeeMobile.Length = 13;
            TextBoxEmployeeMobile.Location = new Point(18, 71);
            TextBoxEmployeeMobile.Mask = "#####-#######";
            TextBoxEmployeeMobile.Name = "TextBoxEmployeeMobile";
            TextBoxEmployeeMobile.ReadOnly = true;
            TextBoxEmployeeMobile.Size = new Size(100, 21);
            TextBoxEmployeeMobile.TabIndex = 13;
            // 
            // TextBoxEmployeePhone
            // 
            TextBoxEmployeePhone.AreaCodeLength = 5;
            TextBoxEmployeePhone.BackColor = SystemColors.Window;
            TextBoxEmployeePhone.Length = 13;
            TextBoxEmployeePhone.Location = new Point(18, 27);
            TextBoxEmployeePhone.Mask = "99999-9999999";
            TextBoxEmployeePhone.Name = "TextBoxEmployeePhone";
            TextBoxEmployeePhone.ReadOnly = true;
            TextBoxEmployeePhone.Size = new Size(100, 21);
            TextBoxEmployeePhone.TabIndex = 12;
            TextBoxEmployeePhone.PreviewKeyDown += TextBoxEmployeePhone_PreviewKeyDown;
            // 
            // TextBoxEmployeePan
            // 
            TextBoxEmployeePan.BackColor = SystemColors.Window;
            TextBoxEmployeePan.Location = new Point(18, 202);
            TextBoxEmployeePan.MaxLength = 20;
            TextBoxEmployeePan.Name = "TextBoxEmployeePan";
            TextBoxEmployeePan.ReadOnly = true;
            TextBoxEmployeePan.Size = new Size(218, 21);
            TextBoxEmployeePan.TabIndex = 16;
            TextBoxEmployeePan.KeyDown += TextBoxEmployeePan_KeyDown;
            TextBoxEmployeePan.KeyPress += TextBoxEmployeePan_KeyPress;
            TextBoxEmployeePan.MouseDown += TextBoxEmployeePan_MouseDown;
            // 
            // TextBoxEmployeeFax
            // 
            TextBoxEmployeeFax.BackColor = SystemColors.Window;
            TextBoxEmployeeFax.Location = new Point(18, 117);
            TextBoxEmployeeFax.Mask = "99999999999999999999";
            TextBoxEmployeeFax.Name = "TextBoxEmployeeFax";
            TextBoxEmployeeFax.ReadOnly = true;
            TextBoxEmployeeFax.Size = new Size(165, 21);
            TextBoxEmployeeFax.TabIndex = 14;
            // 
            // TextBoxEmployeeEmail
            // 
            TextBoxEmployeeEmail.BackColor = SystemColors.Window;
            TextBoxEmployeeEmail.Location = new Point(18, 160);
            TextBoxEmployeeEmail.MaxLength = 50;
            TextBoxEmployeeEmail.Name = "TextBoxEmployeeEmail";
            TextBoxEmployeeEmail.ReadOnly = true;
            TextBoxEmployeeEmail.Size = new Size(218, 21);
            TextBoxEmployeeEmail.TabIndex = 15;
            TextBoxEmployeeEmail.KeyDown += TextBoxEmployeeEmail_KeyDown;
            TextBoxEmployeeEmail.KeyPress += TextBoxEmployeeEmail_KeyPress;
            TextBoxEmployeeEmail.MouseDown += TextBoxEmployeeEmail_MouseDown;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(18, 144);
            label11.Name = "label11";
            label11.Size = new Size(31, 13);
            label11.TabIndex = 27;
            label11.Text = "Email";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(18, 100);
            label10.Name = "label10";
            label10.Size = new Size(25, 13);
            label10.TabIndex = 26;
            label10.Text = "Fax";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(18, 55);
            label9.Name = "label9";
            label9.Size = new Size(37, 13);
            label9.TabIndex = 25;
            label9.Text = "Mobile";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label8.Location = new Point(18, 11);
            label8.Name = "label8";
            label8.Size = new Size(37, 13);
            label8.TabIndex = 24;
            label8.Text = "Phone";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label17.Location = new Point(18, 186);
            label17.Name = "label17";
            label17.Size = new Size(27, 13);
            label17.TabIndex = 32;
            label17.Text = "PAN";
            // 
            // TabPageSignatureDetails
            // 
            TabPageSignatureDetails.Controls.Add(BtnDigitalSignatureDeletePresc);
            TabPageSignatureDetails.Controls.Add(CheckBoxAllowDigtalsignature);
            TabPageSignatureDetails.Controls.Add(DigitalSignaturePictureBoxPresc);
            TabPageSignatureDetails.Controls.Add(CheckBoxRxSymbolPresc);
            TabPageSignatureDetails.Controls.Add(BtnDigitalSignatureImpoortPresc);
            TabPageSignatureDetails.Controls.Add(label3);
            TabPageSignatureDetails.Location = new Point(4, 22);
            TabPageSignatureDetails.Name = "TabPageSignatureDetails";
            TabPageSignatureDetails.Padding = new Padding(3);
            TabPageSignatureDetails.Size = new Size(560, 448);
            TabPageSignatureDetails.TabIndex = 2;
            TabPageSignatureDetails.Text = "Signature Details";
            TabPageSignatureDetails.UseVisualStyleBackColor = true;
            // 
            // BtnDigitalSignatureDeletePresc
            // 
            BtnDigitalSignatureDeletePresc.BackColor = Color.White;
            BtnDigitalSignatureDeletePresc.BackgroundImage = (Image)resources.GetObject("BtnDigitalSignatureDeletePresc.BackgroundImage");
            BtnDigitalSignatureDeletePresc.BackgroundImageLayout = ImageLayout.Stretch;
            BtnDigitalSignatureDeletePresc.Location = new Point(310, 71);
            BtnDigitalSignatureDeletePresc.Margin = new Padding(2);
            BtnDigitalSignatureDeletePresc.Name = "BtnDigitalSignatureDeletePresc";
            BtnDigitalSignatureDeletePresc.Size = new Size(24, 24);
            BtnDigitalSignatureDeletePresc.TabIndex = 80;
            BtnDigitalSignatureDeletePresc.UseVisualStyleBackColor = false;
            BtnDigitalSignatureDeletePresc.Click += BtnDigitalSignatureDeletePresc_Click;
            // 
            // CheckBoxAllowDigtalsignature
            // 
            CheckBoxAllowDigtalsignature.AutoSize = true;
            CheckBoxAllowDigtalsignature.Location = new Point(18, 35);
            CheckBoxAllowDigtalsignature.Name = "CheckBoxAllowDigtalsignature";
            CheckBoxAllowDigtalsignature.Size = new Size(139, 17);
            CheckBoxAllowDigtalsignature.TabIndex = 83;
            CheckBoxAllowDigtalsignature.Text = "Display digital signature";
            CheckBoxAllowDigtalsignature.UseVisualStyleBackColor = true;
            CheckBoxAllowDigtalsignature.CheckedChanged += CheckBoxAllowDigtalsignature_CheckedChanged;
            // 
            // DigitalSignaturePictureBoxPresc
            // 
            DigitalSignaturePictureBoxPresc.BackColor = SystemColors.Window;
            DigitalSignaturePictureBoxPresc.BorderStyle = BorderStyle.FixedSingle;
            DigitalSignaturePictureBoxPresc.Location = new Point(18, 71);
            DigitalSignaturePictureBoxPresc.Margin = new Padding(2);
            DigitalSignaturePictureBoxPresc.Name = "DigitalSignaturePictureBoxPresc";
            DigitalSignaturePictureBoxPresc.Size = new Size(289, 69);
            DigitalSignaturePictureBoxPresc.SizeMode = PictureBoxSizeMode.StretchImage;
            DigitalSignaturePictureBoxPresc.TabIndex = 79;
            DigitalSignaturePictureBoxPresc.TabStop = false;
            // 
            // CheckBoxRxSymbolPresc
            // 
            CheckBoxRxSymbolPresc.AutoSize = true;
            CheckBoxRxSymbolPresc.Location = new Point(18, 12);
            CheckBoxRxSymbolPresc.Name = "CheckBoxRxSymbolPresc";
            CheckBoxRxSymbolPresc.Size = new Size(190, 17);
            CheckBoxRxSymbolPresc.TabIndex = 82;
            CheckBoxRxSymbolPresc.Text = "Rx symbol display on prescription?";
            CheckBoxRxSymbolPresc.UseVisualStyleBackColor = true;
            // 
            // BtnDigitalSignatureImpoortPresc
            // 
            BtnDigitalSignatureImpoortPresc.Location = new Point(310, 117);
            BtnDigitalSignatureImpoortPresc.Margin = new Padding(2);
            BtnDigitalSignatureImpoortPresc.Name = "BtnDigitalSignatureImpoortPresc";
            BtnDigitalSignatureImpoortPresc.Size = new Size(47, 24);
            BtnDigitalSignatureImpoortPresc.TabIndex = 77;
            BtnDigitalSignatureImpoortPresc.Text = "Import";
            BtnDigitalSignatureImpoortPresc.UseVisualStyleBackColor = true;
            BtnDigitalSignatureImpoortPresc.Click += BtnDigitalSignatureImpoortPresc_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(18, 55);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(53, 13);
            label3.TabIndex = 78;
            label3.Text = "Signature";
            // 
            // TreeViewEmployeeDepartment
            // 
            TreeViewEmployeeDepartment.ContextMenuStrip = contextMenuStrip1;
            TreeViewEmployeeDepartment.HideSelection = false;
            TreeViewEmployeeDepartment.ImageIndex = 0;
            TreeViewEmployeeDepartment.ImageList = ImageListEmployeeDepartment;
            TreeViewEmployeeDepartment.Location = new Point(13, 36);
            TreeViewEmployeeDepartment.Name = "TreeViewEmployeeDepartment";
            TreeViewEmployeeDepartment.SelectedImageIndex = 0;
            TreeViewEmployeeDepartment.Size = new Size(239, 450);
            TreeViewEmployeeDepartment.TabIndex = 1;
            TreeViewEmployeeDepartment.AfterSelect += TreeViewEmployeeDepartment_AfterSelect;
            TreeViewEmployeeDepartment.NodeMouseClick += TreeViewEmployeeDepartment_NodeMouseClick;
            // 
            // ImageListEmployeeDepartment
            // 
            ImageListEmployeeDepartment.ColorDepth = ColorDepth.Depth8Bit;
            ImageListEmployeeDepartment.ImageStream = (ImageListStreamer)resources.GetObject("ImageListEmployeeDepartment.ImageStream");
            ImageListEmployeeDepartment.TransparentColor = Color.Transparent;
            ImageListEmployeeDepartment.Images.SetKeyName(0, "dep.png");
            ImageListEmployeeDepartment.Images.SetKeyName(1, "employee.png");
            // 
            // BtnDelete
            // 
            BtnDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnDelete.Location = new Point(94, 502);
            BtnDelete.Name = "BtnDelete";
            BtnDelete.Size = new Size(83, 23);
            BtnDelete.TabIndex = 3;
            BtnDelete.Text = "Delete [F4]";
            BtnDelete.UseVisualStyleBackColor = true;
            BtnDelete.Click += BtnDelete_Click;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(728, 501);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(83, 23);
            BtnExit.TabIndex = 13;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // TabControlDepartment
            // 
            TabControlDepartment.Controls.Add(TabPageDepartmentDetails);
            TabControlDepartment.Location = new Point(257, 12);
            TabControlDepartment.Name = "TabControlDepartment";
            TabControlDepartment.SelectedIndex = 0;
            TabControlDepartment.Size = new Size(568, 474);
            TabControlDepartment.TabIndex = 4;
            TabControlDepartment.TabStop = false;
            // 
            // TabPageDepartmentDetails
            // 
            TabPageDepartmentDetails.Controls.Add(comboBoxSwapTextBoxParentDepartment);
            TabPageDepartmentDetails.Controls.Add(TextBoxDepartmentDescription);
            TabPageDepartmentDetails.Controls.Add(LabelCustomerDescription);
            TabPageDepartmentDetails.Controls.Add(TextBoxDepartmentDisplayAs);
            TabPageDepartmentDetails.Controls.Add(TextBoxDepartmentName);
            TabPageDepartmentDetails.Controls.Add(label14);
            TabPageDepartmentDetails.Controls.Add(label15);
            TabPageDepartmentDetails.Controls.Add(LabelParentDepartment);
            TabPageDepartmentDetails.Location = new Point(4, 22);
            TabPageDepartmentDetails.Name = "TabPageDepartmentDetails";
            TabPageDepartmentDetails.Padding = new Padding(3);
            TabPageDepartmentDetails.Size = new Size(560, 448);
            TabPageDepartmentDetails.TabIndex = 0;
            TabPageDepartmentDetails.Text = "Department Details";
            TabPageDepartmentDetails.UseVisualStyleBackColor = true;
            // 
            // comboBoxSwapTextBoxParentDepartment
            // 
            comboBoxSwapTextBoxParentDepartment.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBoxSwapTextBoxParentDepartment.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBoxSwapTextBoxParentDepartment.FormattingEnabled = true;
            comboBoxSwapTextBoxParentDepartment.Location = new Point(22, 183);
            comboBoxSwapTextBoxParentDepartment.Name = "comboBoxSwapTextBoxParentDepartment";
            comboBoxSwapTextBoxParentDepartment.Size = new Size(300, 21);
            comboBoxSwapTextBoxParentDepartment.TabIndex = 10;
            comboBoxSwapTextBoxParentDepartment.TxtVisible = true;
            comboBoxSwapTextBoxParentDepartment.SelectedIndexChanged += comboBoxSwapTextBoxParentDepartment_SelectedIndexChanged;
            comboBoxSwapTextBoxParentDepartment.KeyPress += comboBoxSwapTextBoxParentDepartment_KeyPress;
            // 
            // TextBoxDepartmentDescription
            // 
            TextBoxDepartmentDescription.BackColor = SystemColors.Window;
            TextBoxDepartmentDescription.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxDepartmentDescription.Location = new Point(22, 112);
            TextBoxDepartmentDescription.MaxLength = 250;
            TextBoxDepartmentDescription.Multiline = true;
            TextBoxDepartmentDescription.Name = "TextBoxDepartmentDescription";
            TextBoxDepartmentDescription.ReadOnly = true;
            TextBoxDepartmentDescription.Size = new Size(368, 49);
            TextBoxDepartmentDescription.TabIndex = 8;
            TextBoxDepartmentDescription.KeyPress += TextBoxDepartmentName_KeyPress;
            // 
            // LabelCustomerDescription
            // 
            LabelCustomerDescription.AutoSize = true;
            LabelCustomerDescription.Location = new Point(19, 95);
            LabelCustomerDescription.Name = "LabelCustomerDescription";
            LabelCustomerDescription.Size = new Size(60, 13);
            LabelCustomerDescription.TabIndex = 50;
            LabelCustomerDescription.Text = "Description";
            // 
            // TextBoxDepartmentDisplayAs
            // 
            TextBoxDepartmentDisplayAs.BackColor = SystemColors.Window;
            TextBoxDepartmentDisplayAs.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxDepartmentDisplayAs.Location = new Point(22, 71);
            TextBoxDepartmentDisplayAs.MaxLength = 50;
            TextBoxDepartmentDisplayAs.Name = "TextBoxDepartmentDisplayAs";
            TextBoxDepartmentDisplayAs.ReadOnly = true;
            TextBoxDepartmentDisplayAs.Size = new Size(368, 21);
            TextBoxDepartmentDisplayAs.TabIndex = 7;
            TextBoxDepartmentDisplayAs.KeyDown += TextBoxDepartmentName_KeyDown;
            TextBoxDepartmentDisplayAs.KeyPress += TextBoxDepartmentName_KeyPress;
            TextBoxDepartmentDisplayAs.MouseDown += TextBoxDepartmentDisplayAs_MouseDown;
            // 
            // TextBoxDepartmentName
            // 
            TextBoxDepartmentName.BackColor = SystemColors.Window;
            TextBoxDepartmentName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxDepartmentName.Location = new Point(22, 31);
            TextBoxDepartmentName.MaxLength = 20;
            TextBoxDepartmentName.Name = "TextBoxDepartmentName";
            TextBoxDepartmentName.ReadOnly = true;
            TextBoxDepartmentName.Size = new Size(368, 21);
            TextBoxDepartmentName.TabIndex = 6;
            TextBoxDepartmentName.KeyDown += TextBoxDepartmentName_KeyDown;
            TextBoxDepartmentName.KeyPress += TextBoxDepartmentName_KeyPress;
            TextBoxDepartmentName.MouseDown += TextBoxDepartmentName_MouseDown;
            TextBoxDepartmentName.PreviewKeyDown += TextBoxDepartmentName_PreviewKeyDown;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label14.Location = new Point(19, 54);
            label14.Name = "label14";
            label14.Size = new Size(56, 13);
            label14.TabIndex = 37;
            label14.Text = "Display As";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label15.Location = new Point(19, 13);
            label15.Name = "label15";
            label15.Size = new Size(39, 13);
            label15.TabIndex = 36;
            label15.Text = "Name";
            // 
            // LabelParentDepartment
            // 
            LabelParentDepartment.AutoSize = true;
            LabelParentDepartment.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelParentDepartment.Location = new Point(19, 165);
            LabelParentDepartment.Name = "LabelParentDepartment";
            LabelParentDepartment.Size = new Size(99, 13);
            LabelParentDepartment.TabIndex = 48;
            LabelParentDepartment.Text = "Parent Department";
            // 
            // BtnDepartmentSave
            // 
            BtnDepartmentSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnDepartmentSave.Location = new Point(639, 501);
            BtnDepartmentSave.Name = "BtnDepartmentSave";
            BtnDepartmentSave.Size = new Size(83, 23);
            BtnDepartmentSave.TabIndex = 11;
            BtnDepartmentSave.Text = "Save [F8]";
            BtnDepartmentSave.UseVisualStyleBackColor = true;
            BtnDepartmentSave.Click += BtnDepartmentSave_Click;
            BtnDepartmentSave.PreviewKeyDown += BtnDepartmentSave_PreviewKeyDown;
            // 
            // BtnDepartmentCancel
            // 
            BtnDepartmentCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnDepartmentCancel.Location = new Point(550, 501);
            BtnDepartmentCancel.Name = "BtnDepartmentCancel";
            BtnDepartmentCancel.Size = new Size(83, 23);
            BtnDepartmentCancel.TabIndex = 12;
            BtnDepartmentCancel.Text = "Cancel [Esc]";
            BtnDepartmentCancel.UseVisualStyleBackColor = true;
            BtnDepartmentCancel.Click += BtnDepartmentCancel_Click;
            // 
            // BtnDepartmentEdit
            // 
            BtnDepartmentEdit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnDepartmentEdit.Location = new Point(183, 502);
            BtnDepartmentEdit.Name = "BtnDepartmentEdit";
            BtnDepartmentEdit.Size = new Size(83, 23);
            BtnDepartmentEdit.TabIndex = 4;
            BtnDepartmentEdit.Text = "Edit [F7]";
            BtnDepartmentEdit.UseVisualStyleBackColor = true;
            BtnDepartmentEdit.Click += BtnDepartmentEdit_Click;
            // 
            // TextBoxEmpDepId
            // 
            TextBoxEmpDepId.Location = new Point(381, 505);
            TextBoxEmpDepId.Name = "TextBoxEmpDepId";
            TextBoxEmpDepId.Size = new Size(100, 21);
            TextBoxEmpDepId.TabIndex = 15;
            TextBoxEmpDepId.Visible = false;
            // 
            // TextBoxSearch
            // 
            TextBoxSearch.BackColor = SystemColors.Window;
            TextBoxSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxSearch.Delay = true;
            TextBoxSearch.DelayTime = 1000;
            TextBoxSearch.Location = new Point(13, 12);
            TextBoxSearch.MaxLength = 30;
            TextBoxSearch.Name = "TextBoxSearch";
            TextBoxSearch.Searchstartfrom = 2;
            TextBoxSearch.Size = new Size(239, 21);
            TextBoxSearch.TabIndex = 16;
            TextBoxSearch.TextChanged += TextBoxSearch_TextChanged;
            TextBoxSearch.KeyDown += TextBoxSearch_KeyDown;
            // 
            // BtnNew
            // 
            BtnNew.ButtonText = "New [F3]";
            BtnNew.ContextMenuStrip = contextMenuStrip1;
            BtnNew.ImageList = ImageListEmployeeDepartment;
            BtnNew.Items = (System.Collections.ObjectModel.Collection<string>)resources.GetObject("BtnNew.Items");
            BtnNew.Location = new Point(9, 502);
            BtnNew.Margin = new Padding(4, 3, 4, 3);
            BtnNew.Name = "BtnNew";
            BtnNew.Size = new Size(94, 28);
            BtnNew.TabIndex = 2;
            BtnNew.ItemClickedEvent += BtnNew_ItemClickedEvent;
            // 
            // FormEmployee
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(829, 561);
            Controls.Add(BtnDelete);
            Controls.Add(BtnNew);
            Controls.Add(TextBoxSearch);
            Controls.Add(TextBoxEmpDepId);
            Controls.Add(BtnExit);
            Controls.Add(TreeViewEmployeeDepartment);
            Controls.Add(statusStrip1);
            Controls.Add(BtnDepartmentEdit);
            Controls.Add(BtnDepartmentCancel);
            Controls.Add(BtnDepartmentSave);
            Controls.Add(TabControlEmployee);
            Controls.Add(TabControlDepartment);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormEmployee";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Employees";
            FormClosing += FormEmployee_FormClosing;
            Load += Employee_Load;
            Controls.SetChildIndex(TabControlDepartment, 0);
            Controls.SetChildIndex(TabControlEmployee, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(BtnDepartmentSave, 0);
            Controls.SetChildIndex(BtnDepartmentCancel, 0);
            Controls.SetChildIndex(BtnDepartmentEdit, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(TreeViewEmployeeDepartment, 0);
            Controls.SetChildIndex(BtnExit, 0);
            Controls.SetChildIndex(TextBoxEmpDepId, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(TextBoxSearch, 0);
            Controls.SetChildIndex(BtnNew, 0);
            Controls.SetChildIndex(BtnDelete, 0);
            contextMenuStrip2.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            TabControlEmployee.ResumeLayout(false);
            TabPageEmployeeDetails.ResumeLayout(false);
            TabPageEmployeeDetails.PerformLayout();
            CameraPanel.ResumeLayout(false);
            TabPageContactDetail.ResumeLayout(false);
            TabPageContactDetail.PerformLayout();
            TabPageSignatureDetails.ResumeLayout(false);
            TabPageSignatureDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DigitalSignaturePictureBoxPresc).EndInit();
            TabControlDepartment.ResumeLayout(false);
            TabPageDepartmentDetails.ResumeLayout(false);
            TabPageDepartmentDetails.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusStrip1;
        private TabControl TabControlEmployee;
        private TabPage TabPageEmployeeDetails;
        private TabPage TabPageContactDetail;
        private TreeView TreeViewEmployeeDepartment;
        private Button BtnDelete;
        private Button BtnExit;
        private ContextMenuStrip contextMenuStrip2;
        private ToolStripMenuItem newEmployeeToolStripMenuItem1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem newDepartmentToolStripMenuItem;
        private ToolStripMenuItem newEmployeeToolStripMenuItem;
        private TextBox TextBoxEmployeeLegalName;
        private TextBox TextBoxEmployeeName;
        private Label LabelCompanyLegalname;
        private Label LabelCompanyName;
        private Label label1;
        private Label label2;
        private Button BtnAddTitle;
        private controls.ComboBoxSwapTextBox ComboBoxSwapTextBoxEmployeeTitle;
        private MaskedTextBox TextBoxEmployeeFax;
        private TextBox TextBoxEmployeeEmail;
        private Label label11;
        private Label label10;
        private Label label9;
        private Label label8;
        private Label label17;
        private TextBox TextBoxEmployeePan;
        private TabControl TabControlDepartment;
        private TabPage TabPageDepartmentDetails;
        private TextBox TextBoxDepartmentDisplayAs;
        private TextBox TextBoxDepartmentName;
        private Label label14;
        private Label label15;
        private Button BtnDepartmentSave;
        private Button BtnDepartmentCancel;
        private Button BtnDepartmentEdit;
        private Label LabelParentDepartment;
        private TextBox TextBoxDepartmentDescription;
        private Label LabelCustomerDescription;
        private ImageList ImageListEmployeeDepartment;
        private TextBox TextBoxEmpDepId;
        private Label label4;
        private controls.ComboBoxSwapTextBox comboBoxSwapTextBoxEmployeeDepartment;
        private controls.ComboBoxSwapTextBox comboBoxSwapTextBoxParentDepartment;
        private ToolStripStatusLabel EmployeeDepartmentErrorMsg;
        private controls.text.PhoneTextBox TextBoxEmployeeMobile;
        private controls.text.PhoneTextBox TextBoxEmployeePhone;
        private controls.text.DateWithCalendar DateTimePickerEmployee;
        private Label label5;
        private controls.text.DelayedTextChangeTextBox TextBoxSearch;
        private Dropdown_Button.UserControlButtonWithMenu BtnNew;
        private controls.AddressGroupBoxWithStateSelection AddressGroupBoxEmployee;
        private Fa.views.controls.Photo.CameraControl CameraPhotoTaker;
        private Panel CameraPanel;
        private CheckBox checkBoxIsServiceProvider;
        private TabPage TabPageSignatureDetails;
        private GroupBox groupBoxDigitalSignaturePresc;
        private Button BtnDigitalSignatureDeletePresc;
        private PictureBox DigitalSignaturePictureBoxPresc;
        private Button BtnDigitalSignatureImpoortPresc;
        private Label label3;
        private CheckBox CheckBoxRxSymbolPresc;
        private CheckBox CheckBoxAllowDigtalsignature;
    }
}