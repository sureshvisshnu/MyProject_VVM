namespace fa.views.hms.patient
{
    partial class FormParentOrGuardian
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormParentOrGuardian));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label5 = new Label();
            TextBoxGuardianFirstName = new TextBox();
            TextBoxGuardianInitial = new TextBox();
            TextBoxGuardianLastName = new TextBox();
            TextBoxGuardianTaxId = new ExtdTextBox();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            RbtGuardianGender = new controls.GenderRadio();
            BtnGuardianCancel = new Button();
            BtnGuardianSave = new Button();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            TextBoxGuardianOccupation = new TextBox();
            TextBoxGuardianEmployer = new TextBox();
            statusStrip1 = new StatusStrip();
            GuardianErrorMsg = new ToolStripStatusLabel();
            ToolStripStatusLabelErrorParent = new ToolStripStatusLabel();
            label11 = new Label();
            ComboBoxSwapTextBoxGuardianRelationShip = new controls.ComboBoxSwapTextBox();
            label4 = new Label();
            TextBoxGuardianAge = new TextBox();
            label12 = new Label();
            label13 = new Label();
            TextBoxGuardianId = new TextBox();
            TextBoxPatientId = new TextBox();
            DateTimePickerGuardianDob = new controls.text.DateWithCalendar();
            TextBoxGuardianMobile = new controls.text.PhoneTextBox();
            TextBoxGuardianPhone = new controls.text.PhoneTextBox();
            TextBoxGuardianIncome = new controls.text.CurrencyTextBox();
            label6 = new Label();
            GroupBoxGuardianAddress = new controls.AddressGroupBoxWithStateSelection();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // PatientIdTransport
            // 
            PatientIdTransport.Size = new Size(100, 21);
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(455, 391);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(455, 364);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(455, 337);
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // checkBoxIsPatient
            // 
            checkBoxIsPatient.Size = new Size(69, 17);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(12, 16);
            label1.Name = "label1";
            label1.Size = new Size(39, 13);
            label1.TabIndex = 1;
            label1.Text = "Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(12, 56);
            label2.Name = "label2";
            label2.Size = new Size(48, 13);
            label2.TabIndex = 2;
            label2.Text = "Gender";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(15, 97);
            label3.Name = "label3";
            label3.Size = new Size(30, 13);
            label3.TabIndex = 3;
            label3.Text = "DOB";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(12, 137);
            label5.Name = "label5";
            label5.Size = new Size(38, 13);
            label5.TabIndex = 5;
            label5.Text = "Tax Id";
            // 
            // TextBoxGuardianFirstName
            // 
            TextBoxGuardianFirstName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxGuardianFirstName.Location = new Point(15, 32);
            TextBoxGuardianFirstName.MaxLength = 30;
            TextBoxGuardianFirstName.Name = "TextBoxGuardianFirstName";
            TextBoxGuardianFirstName.Size = new Size(156, 21);
            TextBoxGuardianFirstName.TabIndex = 1;
            // 
            // TextBoxGuardianInitial
            // 
            TextBoxGuardianInitial.CharacterCasing = CharacterCasing.Upper;
            TextBoxGuardianInitial.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxGuardianInitial.Location = new Point(178, 32);
            TextBoxGuardianInitial.MaxLength = 1;
            TextBoxGuardianInitial.Name = "TextBoxGuardianInitial";
            TextBoxGuardianInitial.Size = new Size(17, 21);
            TextBoxGuardianInitial.TabIndex = 2;
            // 
            // TextBoxGuardianLastName
            // 
            TextBoxGuardianLastName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxGuardianLastName.Location = new Point(201, 32);
            TextBoxGuardianLastName.MaxLength = 30;
            TextBoxGuardianLastName.Name = "TextBoxGuardianLastName";
            TextBoxGuardianLastName.Size = new Size(178, 21);
            TextBoxGuardianLastName.TabIndex = 3;
            // 
            // TextBoxGuardianTaxId
            // 
            TextBoxGuardianTaxId.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxGuardianTaxId.Location = new Point(15, 153);
            TextBoxGuardianTaxId.MaxLength = 20;
            TextBoxGuardianTaxId.Name = "TextBoxGuardianTaxId";
            TextBoxGuardianTaxId.Size = new Size(100, 21);
            TextBoxGuardianTaxId.TabIndex = 6;
            // 
            // RbtGuardianGender
            // 
            RbtGuardianGender.BackColor = SystemColors.Window;
            RbtGuardianGender.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            RbtGuardianGender.Gender = controls.GenderSelection.Male;
            RbtGuardianGender.Location = new Point(10, 72);
            RbtGuardianGender.Name = "RbtGuardianGender";
            RbtGuardianGender.Size = new Size(179, 22);
            RbtGuardianGender.TabIndex = 4;
            // 
            // BtnGuardianCancel
            // 
            BtnGuardianCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnGuardianCancel.Location = new Point(279, 441);
            BtnGuardianCancel.Name = "BtnGuardianCancel";
            BtnGuardianCancel.Size = new Size(88, 23);
            BtnGuardianCancel.TabIndex = 15;
            BtnGuardianCancel.Text = "Cancel [Esc]";
            BtnGuardianCancel.UseVisualStyleBackColor = true;
            BtnGuardianCancel.Click += BtnGuardianCancel_Click;
            // 
            // BtnGuardianSave
            // 
            BtnGuardianSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnGuardianSave.Location = new Point(373, 441);
            BtnGuardianSave.Name = "BtnGuardianSave";
            BtnGuardianSave.Size = new Size(75, 23);
            BtnGuardianSave.TabIndex = 14;
            BtnGuardianSave.Text = "Save [F8]";
            BtnGuardianSave.UseVisualStyleBackColor = true;
            BtnGuardianSave.Click += BtnGuardianSave_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label8.Location = new Point(285, 97);
            label8.Name = "label8";
            label8.Size = new Size(61, 13);
            label8.TabIndex = 19;
            label8.Text = "Occupation";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label9.Location = new Point(285, 137);
            label9.Name = "label9";
            label9.Size = new Size(42, 13);
            label9.TabIndex = 20;
            label9.Text = "Income";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label10.Location = new Point(285, 177);
            label10.Name = "label10";
            label10.Size = new Size(51, 13);
            label10.TabIndex = 21;
            label10.Text = "Employer";
            // 
            // TextBoxGuardianOccupation
            // 
            TextBoxGuardianOccupation.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxGuardianOccupation.Location = new Point(288, 113);
            TextBoxGuardianOccupation.MaxLength = 35;
            TextBoxGuardianOccupation.Name = "TextBoxGuardianOccupation";
            TextBoxGuardianOccupation.Size = new Size(176, 21);
            TextBoxGuardianOccupation.TabIndex = 9;
            // 
            // TextBoxGuardianEmployer
            // 
            TextBoxGuardianEmployer.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxGuardianEmployer.Location = new Point(288, 195);
            TextBoxGuardianEmployer.MaxLength = 35;
            TextBoxGuardianEmployer.Name = "TextBoxGuardianEmployer";
            TextBoxGuardianEmployer.Size = new Size(175, 21);
            TextBoxGuardianEmployer.TabIndex = 11;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { GuardianErrorMsg, ToolStripStatusLabelErrorParent });
            statusStrip1.Location = new Point(0, 478);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(476, 22);
            statusStrip1.TabIndex = 25;
            statusStrip1.Text = "statusStrip1";
            // 
            // GuardianErrorMsg
            // 
            GuardianErrorMsg.Name = "GuardianErrorMsg";
            GuardianErrorMsg.Size = new Size(0, 17);
            // 
            // ToolStripStatusLabelErrorParent
            // 
            ToolStripStatusLabelErrorParent.BackColor = SystemColors.Control;
            ToolStripStatusLabelErrorParent.Name = "ToolStripStatusLabelErrorParent";
            ToolStripStatusLabelErrorParent.Size = new Size(151, 17);
            ToolStripStatusLabelErrorParent.Text = "                                                ";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label11.Location = new Point(12, 425);
            label11.Name = "label11";
            label11.Size = new Size(77, 13);
            label11.TabIndex = 26;
            label11.Text = "Relationship";
            // 
            // ComboBoxSwapTextBoxGuardianRelationShip
            // 
            ComboBoxSwapTextBoxGuardianRelationShip.AutoCompleteCustomSource.AddRange(new string[] { "MOTHER", "FATHER", "GRANDPARENT", "GAURDIAN", "FRIEND", "SELF" });
            ComboBoxSwapTextBoxGuardianRelationShip.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxSwapTextBoxGuardianRelationShip.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxSwapTextBoxGuardianRelationShip.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ComboBoxSwapTextBoxGuardianRelationShip.FormattingEnabled = true;
            ComboBoxSwapTextBoxGuardianRelationShip.Items.AddRange(new object[] { "MOTHER", "FATHER", "GRANDPARENT", "GAURDIAN", "FRIEND", "SELF", "HUSBAND", "SPOUSE" });
            ComboBoxSwapTextBoxGuardianRelationShip.Location = new Point(15, 443);
            ComboBoxSwapTextBoxGuardianRelationShip.Name = "ComboBoxSwapTextBoxGuardianRelationShip";
            ComboBoxSwapTextBoxGuardianRelationShip.Size = new Size(236, 21);
            ComboBoxSwapTextBoxGuardianRelationShip.TabIndex = 13;
            ComboBoxSwapTextBoxGuardianRelationShip.TxtVisible = true;
            ComboBoxSwapTextBoxGuardianRelationShip.KeyPress += ComboBoxSwapTextBoxGuardianRelationShip_KeyPress;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(149, 97);
            label4.Name = "label4";
            label4.Size = new Size(29, 13);
            label4.TabIndex = 51;
            label4.Text = "Age";
            // 
            // TextBoxGuardianAge
            // 
            TextBoxGuardianAge.BackColor = Color.White;
            TextBoxGuardianAge.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxGuardianAge.Location = new Point(152, 113);
            TextBoxGuardianAge.Name = "TextBoxGuardianAge";
            TextBoxGuardianAge.ReadOnly = true;
            TextBoxGuardianAge.Size = new Size(37, 21);
            TextBoxGuardianAge.TabIndex = 50;
            TextBoxGuardianAge.TabStop = false;
            TextBoxGuardianAge.TextAlign = HorizontalAlignment.Right;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label12.Location = new Point(149, 177);
            label12.Name = "label12";
            label12.Size = new Size(37, 13);
            label12.TabIndex = 55;
            label12.Text = "Mobile";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label13.Location = new Point(12, 177);
            label13.Name = "label13";
            label13.Size = new Size(37, 13);
            label13.TabIndex = 54;
            label13.Text = "Phone";
            // 
            // TextBoxGuardianId
            // 
            TextBoxGuardianId.Location = new Point(12, 441);
            TextBoxGuardianId.Name = "TextBoxGuardianId";
            TextBoxGuardianId.Size = new Size(100, 21);
            TextBoxGuardianId.TabIndex = 59;
            TextBoxGuardianId.TabStop = false;
            TextBoxGuardianId.Visible = false;
            // 
            // TextBoxPatientId
            // 
            TextBoxPatientId.Location = new Point(118, 441);
            TextBoxPatientId.Name = "TextBoxPatientId";
            TextBoxPatientId.Size = new Size(100, 21);
            TextBoxPatientId.TabIndex = 509;
            TextBoxPatientId.TabStop = false;
            TextBoxPatientId.Visible = false;
            // 
            // DateTimePickerGuardianDob
            // 
            DateTimePickerGuardianDob.BackColor = Color.White;
            DateTimePickerGuardianDob.BorderStyle = BorderStyle.FixedSingle;
            DateTimePickerGuardianDob.Date = null;
            DateTimePickerGuardianDob.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DateTimePickerGuardianDob.Format = "MM/dd/yyyy";
            DateTimePickerGuardianDob.Location = new Point(15, 113);
            DateTimePickerGuardianDob.MaxDate = new DateTime(9997, 12, 31, 0, 20, 35, 0);
            DateTimePickerGuardianDob.MinDate = new DateTime(1900, 1, 1, 22, 19, 23, 0);
            DateTimePickerGuardianDob.Name = "DateTimePickerGuardianDob";
            DateTimePickerGuardianDob.ReadOnly = false;
            DateTimePickerGuardianDob.Size = new Size(93, 21);
            DateTimePickerGuardianDob.TabIndex = 5;
            DateTimePickerGuardianDob.Leave += DateTimePickerGuardianDob_Leave;
            // 
            // TextBoxGuardianMobile
            // 
            TextBoxGuardianMobile.AreaCodeLength = 5;
            TextBoxGuardianMobile.Length = 13;
            TextBoxGuardianMobile.Location = new Point(152, 195);
            TextBoxGuardianMobile.Mask = "#####-#######";
            TextBoxGuardianMobile.Name = "TextBoxGuardianMobile";
            TextBoxGuardianMobile.Size = new Size(100, 21);
            TextBoxGuardianMobile.TabIndex = 8;
            // 
            // TextBoxGuardianPhone
            // 
            TextBoxGuardianPhone.AreaCodeLength = 5;
            TextBoxGuardianPhone.Length = 13;
            TextBoxGuardianPhone.Location = new Point(15, 193);
            TextBoxGuardianPhone.Mask = "#####-#######";
            TextBoxGuardianPhone.Name = "TextBoxGuardianPhone";
            TextBoxGuardianPhone.Size = new Size(100, 21);
            TextBoxGuardianPhone.TabIndex = 7;
            // 
            // TextBoxGuardianIncome
            // 
            TextBoxGuardianIncome.Decimals = 2;
            TextBoxGuardianIncome.Length = 10;
            TextBoxGuardianIncome.Location = new Point(288, 153);
            TextBoxGuardianIncome.Name = "TextBoxGuardianIncome";
            TextBoxGuardianIncome.Size = new Size(91, 21);
            TextBoxGuardianIncome.TabIndex = 10;
            TextBoxGuardianIncome.Text = "0.00";
            TextBoxGuardianIncome.TextAlign = HorizontalAlignment.Right;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label6.Location = new Point(12, 217);
            label6.Name = "label6";
            label6.Size = new Size(46, 13);
            label6.TabIndex = 510;
            label6.Text = "Address";
            // 
            // GroupBoxGuardianAddress
            // 
            GroupBoxGuardianAddress.AddressLine1 = "";
            GroupBoxGuardianAddress.AddressLine2 = "";
            GroupBoxGuardianAddress.CityName = "";
            GroupBoxGuardianAddress.CountryId = 0L;
            GroupBoxGuardianAddress.DistrictName = "";
            GroupBoxGuardianAddress.GroupName = "";
            GroupBoxGuardianAddress.Location = new Point(10, 231);
            GroupBoxGuardianAddress.Name = "GroupBoxGuardianAddress";
            GroupBoxGuardianAddress.PinCode = "";
            GroupBoxGuardianAddress.ReadOnly = true;
            GroupBoxGuardianAddress.Size = new Size(464, 192);
            GroupBoxGuardianAddress.StateId = 0L;
            GroupBoxGuardianAddress.StateName = "";
            GroupBoxGuardianAddress.StateSelectedIndex = -1;
            GroupBoxGuardianAddress.TabIndex = 12;
            // 
            // FormParentOrGuardian
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(476, 500);
            Controls.Add(ComboBoxSwapTextBoxGuardianRelationShip);
            Controls.Add(GroupBoxGuardianAddress);
            Controls.Add(TextBoxGuardianAge);
            Controls.Add(label6);
            Controls.Add(TextBoxGuardianIncome);
            Controls.Add(TextBoxGuardianMobile);
            Controls.Add(TextBoxGuardianPhone);
            Controls.Add(DateTimePickerGuardianDob);
            Controls.Add(TextBoxPatientId);
            Controls.Add(TextBoxGuardianId);
            Controls.Add(label12);
            Controls.Add(label13);
            Controls.Add(label11);
            Controls.Add(statusStrip1);
            Controls.Add(TextBoxGuardianEmployer);
            Controls.Add(TextBoxGuardianOccupation);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(BtnGuardianSave);
            Controls.Add(BtnGuardianCancel);
            Controls.Add(RbtGuardianGender);
            Controls.Add(TextBoxGuardianTaxId);
            Controls.Add(TextBoxGuardianLastName);
            Controls.Add(TextBoxGuardianInitial);
            Controls.Add(TextBoxGuardianFirstName);
            Controls.Add(label5);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(label4);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormParentOrGuardian";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Parent/Guardian Details";
            FormClosing += FormParentOrGuardian_FormClosing;
            Load += FormParentOrGuardian_Load;
            Controls.SetChildIndex(checkBoxIsPatient, 0);
            Controls.SetChildIndex(label4, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(label2, 0);
            Controls.SetChildIndex(label3, 0);
            Controls.SetChildIndex(label5, 0);
            Controls.SetChildIndex(TextBoxGuardianFirstName, 0);
            Controls.SetChildIndex(TextBoxGuardianInitial, 0);
            Controls.SetChildIndex(TextBoxGuardianLastName, 0);
            Controls.SetChildIndex(TextBoxGuardianTaxId, 0);
            Controls.SetChildIndex(RbtGuardianGender, 0);
            Controls.SetChildIndex(BtnGuardianCancel, 0);
            Controls.SetChildIndex(BtnGuardianSave, 0);
            Controls.SetChildIndex(label8, 0);
            Controls.SetChildIndex(label9, 0);
            Controls.SetChildIndex(label10, 0);
            Controls.SetChildIndex(TextBoxGuardianOccupation, 0);
            Controls.SetChildIndex(TextBoxGuardianEmployer, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(label11, 0);
            Controls.SetChildIndex(label13, 0);
            Controls.SetChildIndex(label12, 0);
            Controls.SetChildIndex(TextBoxGuardianId, 0);
            Controls.SetChildIndex(TextBoxPatientId, 0);
            Controls.SetChildIndex(DateTimePickerGuardianDob, 0);
            Controls.SetChildIndex(TextBoxGuardianPhone, 0);
            Controls.SetChildIndex(TextBoxGuardianMobile, 0);
            Controls.SetChildIndex(TextBoxGuardianIncome, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(label6, 0);
            Controls.SetChildIndex(TextBoxGuardianAge, 0);
            Controls.SetChildIndex(GroupBoxGuardianAddress, 0);
            Controls.SetChildIndex(ComboBoxSwapTextBoxGuardianRelationShip, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label5;
        private TextBox TextBoxGuardianFirstName;
        private TextBox TextBoxGuardianInitial;
        private TextBox TextBoxGuardianLastName;
        private ExtdTextBox TextBoxGuardianTaxId;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private controls.GenderRadio RbtGuardianGender;
        private Button BtnGuardianCancel;
        private Button BtnGuardianSave;
        private Label label8;
        private Label label9;
        private Label label10;
        private TextBox TextBoxGuardianOccupation;
        private TextBox TextBoxGuardianEmployer;
        private StatusStrip statusStrip1;
        private Label label11;
        private controls.ComboBoxSwapTextBox ComboBoxSwapTextBoxGuardianRelationShip;
        private Label label4;
        private TextBox TextBoxGuardianAge;
        private Label label12;
        private Label label13;
        private ToolStripStatusLabel GuardianErrorMsg;
        private TextBox TextBoxGuardianId;
        private controls.text.DateWithCalendar DateTimePickerGuardianDob;
        private controls.text.PhoneTextBox TextBoxGuardianMobile;
        private controls.text.PhoneTextBox TextBoxGuardianPhone;
        public TextBox TextBoxPatientId;
        private controls.text.CurrencyTextBox TextBoxGuardianIncome;
        private Label label6;
        private controls.AddressGroupBoxWithStateSelection GroupBoxGuardianAddress;
        private ToolStripStatusLabel ToolStripStatusLabelErrorInvoice;
        private ToolStripStatusLabel ToolStripStatusLabelErrorParent;
    }
}