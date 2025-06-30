namespace fa.views.hms.ip
{
    partial class FormIPRegistration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIPRegistration));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            TextBoxPatientName = new TextBox();
            TextBoxPatientAge = new TextBox();
            BtnCancel = new Button();
            BtnAdmit = new Button();
            statusStrip1 = new StatusStrip();
            InPatientErrorMsg = new ToolStripStatusLabel();
            TextBoxPatientDOB = new ExtdTextBox();
            PatientNumberIp = new controls.PatientNumberControl();
            TextBoxInpatientAdmissionId = new TextBox();
            TextBoxPatientId = new TextBox();
            PatientPhoto = new controls.PatientPhoto();
            GroupBoxPatientAddress = new controls.text.AddressGroupBoxWithoutLandMark();
            label13 = new Label();
            ComboBoxDepartment = new ComboBox();
            label12 = new Label();
            ComboBoxPrimaryConsultant = new controls.ComboBoxSwapTextBox();
            ComboBoxSecondaryConsultant = new controls.ComboBoxSwapTextBox();
            ComboBoxPrimaryNurse = new controls.ComboBoxSwapTextBox();
            ComboBoxSecondaryNurse = new controls.ComboBoxSwapTextBox();
            ComboBoxWard = new controls.ComboBoxSwapTextBox();
            ComboBoxSwapTextBoxBed = new controls.ComboBoxSwapTextBox();
            ComboBoxSelectInsurance = new controls.ComboBoxSwapTextBox();
            LabelSelectInsurance = new Label();
            label4 = new Label();
            TextBoxAddress = new TextBox();
            CheckBoxBillToInsurance = new CheckBox();
            LabelInsurance = new Label();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // PatientIdTransport
            // 
            PatientIdTransport.Location = new Point(12, 812);
            PatientIdTransport.Size = new Size(100, 21);
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(823, 366);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(503, 371);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(503, 345);
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // checkBoxIsPatient
            // 
            checkBoxIsPatient.Size = new Size(69, 17);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(13, 11);
            label1.Name = "label1";
            label1.Size = new Size(39, 13);
            label1.TabIndex = 42;
            label1.Text = "Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(13, 52);
            label2.Name = "label2";
            label2.Size = new Size(33, 13);
            label2.TabIndex = 43;
            label2.Text = "DOB";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(161, 52);
            label3.Name = "label3";
            label3.Size = new Size(29, 13);
            label3.TabIndex = 44;
            label3.Text = "Age";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label6.Location = new Point(15, 254);
            label6.Name = "label6";
            label6.Size = new Size(103, 13);
            label6.TabIndex = 49;
            label6.Text = "Primary (Doctor)";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(15, 297);
            label7.Name = "label7";
            label7.Size = new Size(101, 13);
            label7.TabIndex = 50;
            label7.Text = "Secondary (Doctor)";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(13, 380);
            label8.Name = "label8";
            label8.Size = new Size(97, 13);
            label8.TabIndex = 52;
            label8.Text = "Secondary (Nurse)";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label9.Location = new Point(14, 338);
            label9.Name = "label9";
            label9.Size = new Size(97, 13);
            label9.TabIndex = 51;
            label9.Text = "Primary (Nurse)";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label10.Location = new Point(257, 255);
            label10.Name = "label10";
            label10.Size = new Size(37, 13);
            label10.TabIndex = 57;
            label10.Text = "Ward";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label11.Location = new Point(257, 295);
            label11.Name = "label11";
            label11.Size = new Size(28, 13);
            label11.TabIndex = 58;
            label11.Text = "Bed";
            // 
            // TextBoxPatientName
            // 
            TextBoxPatientName.BackColor = Color.White;
            TextBoxPatientName.Location = new Point(16, 28);
            TextBoxPatientName.Name = "TextBoxPatientName";
            TextBoxPatientName.ReadOnly = true;
            TextBoxPatientName.Size = new Size(394, 21);
            TextBoxPatientName.TabIndex = 500;
            TextBoxPatientName.TabStop = false;
            // 
            // TextBoxPatientAge
            // 
            TextBoxPatientAge.BackColor = Color.White;
            TextBoxPatientAge.Location = new Point(164, 69);
            TextBoxPatientAge.Name = "TextBoxPatientAge";
            TextBoxPatientAge.ReadOnly = true;
            TextBoxPatientAge.Size = new Size(39, 21);
            TextBoxPatientAge.TabIndex = 65;
            TextBoxPatientAge.TabStop = false;
            // 
            // BtnCancel
            // 
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(413, 435);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(84, 23);
            BtnCancel.TabIndex = 8;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // BtnAdmit
            // 
            BtnAdmit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnAdmit.Location = new Point(503, 435);
            BtnAdmit.Name = "BtnAdmit";
            BtnAdmit.Size = new Size(84, 23);
            BtnAdmit.TabIndex = 7;
            BtnAdmit.Text = "Admit [F8]";
            BtnAdmit.UseVisualStyleBackColor = true;
            BtnAdmit.Click += BtnAdmit_Click;
            BtnAdmit.PreviewKeyDown += BtnAdmit_PreviewKeyDown;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { InPatientErrorMsg });
            statusStrip1.Location = new Point(0, 471);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(628, 22);
            statusStrip1.TabIndex = 70;
            statusStrip1.Text = "statusStrip1";
            // 
            // InPatientErrorMsg
            // 
            InPatientErrorMsg.Name = "InPatientErrorMsg";
            InPatientErrorMsg.Size = new Size(0, 17);
            // 
            // TextBoxPatientDOB
            // 
            TextBoxPatientDOB.BackColor = Color.White;
            TextBoxPatientDOB.Location = new Point(16, 69);
            TextBoxPatientDOB.Name = "TextBoxPatientDOB";
            TextBoxPatientDOB.ReadOnly = true;
            TextBoxPatientDOB.Size = new Size(100, 21);
            TextBoxPatientDOB.TabIndex = 64;
            TextBoxPatientDOB.TabStop = false;
            // 
            // PatientNumberIp
            // 
            PatientNumberIp.Location = new Point(424, 240);
            PatientNumberIp.Margin = new Padding(4, 3, 4, 3);
            PatientNumberIp.Name = "PatientNumberIp";
            PatientNumberIp.PatientNumber = "";
            PatientNumberIp.Size = new Size(191, 80);
            PatientNumberIp.TabIndex = 505;
            // 
            // TextBoxInpatientAdmissionId
            // 
            TextBoxInpatientAdmissionId.Location = new Point(834, 230);
            TextBoxInpatientAdmissionId.Name = "TextBoxInpatientAdmissionId";
            TextBoxInpatientAdmissionId.Size = new Size(100, 21);
            TextBoxInpatientAdmissionId.TabIndex = 506;
            TextBoxInpatientAdmissionId.TabStop = false;
            TextBoxInpatientAdmissionId.Visible = false;
            // 
            // TextBoxPatientId
            // 
            TextBoxPatientId.Location = new Point(814, 303);
            TextBoxPatientId.Name = "TextBoxPatientId";
            TextBoxPatientId.Size = new Size(100, 21);
            TextBoxPatientId.TabIndex = 507;
            TextBoxPatientId.TabStop = false;
            TextBoxPatientId.Visible = false;
            // 
            // PatientPhoto
            // 
            PatientPhoto.Location = new Point(426, 12);
            PatientPhoto.Margin = new Padding(4, 3, 4, 3);
            PatientPhoto.Name = "PatientPhoto";
            PatientPhoto.Size = new Size(187, 222);
            PatientPhoto.TabIndex = 508;
            // 
            // GroupBoxPatientAddress
            // 
            GroupBoxPatientAddress.AddressLine1 = "";
            GroupBoxPatientAddress.BackColor = SystemColors.Control;
            GroupBoxPatientAddress.CityName = "";
            GroupBoxPatientAddress.DistrictName = "";
            GroupBoxPatientAddress.GroupName = "";
            GroupBoxPatientAddress.Location = new Point(657, 67);
            GroupBoxPatientAddress.Margin = new Padding(4, 3, 4, 3);
            GroupBoxPatientAddress.Name = "GroupBoxPatientAddress";
            GroupBoxPatientAddress.PinCode = "";
            GroupBoxPatientAddress.ReadOnly = true;
            GroupBoxPatientAddress.Size = new Size(465, 157);
            GroupBoxPatientAddress.StateName = "";
            GroupBoxPatientAddress.TabIndex = 71;
            GroupBoxPatientAddress.TabStop = false;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(15, 93);
            label13.Name = "label13";
            label13.Size = new Size(46, 13);
            label13.TabIndex = 510;
            label13.Text = "Address";
            // 
            // ComboBoxDepartment
            // 
            ComboBoxDepartment.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxDepartment.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxDepartment.FormattingEnabled = true;
            ComboBoxDepartment.Location = new Point(260, 351);
            ComboBoxDepartment.Name = "ComboBoxDepartment";
            ComboBoxDepartment.Size = new Size(150, 21);
            ComboBoxDepartment.TabIndex = 6;
            ComboBoxDepartment.Visible = false;
            ComboBoxDepartment.KeyPress += ComboBoxDepartment_KeyPress;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(257, 335);
            label12.Name = "label12";
            label12.Size = new Size(64, 13);
            label12.TabIndex = 59;
            label12.Text = "Department";
            label12.Visible = false;
            // 
            // ComboBoxPrimaryConsultant
            // 
            ComboBoxPrimaryConsultant.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxPrimaryConsultant.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxPrimaryConsultant.FormattingEnabled = true;
            ComboBoxPrimaryConsultant.Location = new Point(16, 271);
            ComboBoxPrimaryConsultant.Name = "ComboBoxPrimaryConsultant";
            ComboBoxPrimaryConsultant.Size = new Size(228, 21);
            ComboBoxPrimaryConsultant.TabIndex = 512;
            ComboBoxPrimaryConsultant.TxtVisible = true;
            ComboBoxPrimaryConsultant.PreviewKeyDown += ComboBoxPrimaryConsultant_PreviewKeyDown;
            // 
            // ComboBoxSecondaryConsultant
            // 
            ComboBoxSecondaryConsultant.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxSecondaryConsultant.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxSecondaryConsultant.FormattingEnabled = true;
            ComboBoxSecondaryConsultant.Location = new Point(16, 313);
            ComboBoxSecondaryConsultant.Name = "ComboBoxSecondaryConsultant";
            ComboBoxSecondaryConsultant.Size = new Size(228, 21);
            ComboBoxSecondaryConsultant.TabIndex = 514;
            ComboBoxSecondaryConsultant.TxtVisible = true;
            // 
            // ComboBoxPrimaryNurse
            // 
            ComboBoxPrimaryNurse.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxPrimaryNurse.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxPrimaryNurse.FormattingEnabled = true;
            ComboBoxPrimaryNurse.Location = new Point(16, 355);
            ComboBoxPrimaryNurse.Name = "ComboBoxPrimaryNurse";
            ComboBoxPrimaryNurse.Size = new Size(228, 21);
            ComboBoxPrimaryNurse.TabIndex = 516;
            ComboBoxPrimaryNurse.TxtVisible = true;
            // 
            // ComboBoxSecondaryNurse
            // 
            ComboBoxSecondaryNurse.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxSecondaryNurse.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxSecondaryNurse.FormattingEnabled = true;
            ComboBoxSecondaryNurse.Location = new Point(16, 397);
            ComboBoxSecondaryNurse.Name = "ComboBoxSecondaryNurse";
            ComboBoxSecondaryNurse.Size = new Size(228, 21);
            ComboBoxSecondaryNurse.TabIndex = 518;
            ComboBoxSecondaryNurse.TxtVisible = true;
            // 
            // ComboBoxWard
            // 
            ComboBoxWard.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxWard.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxWard.FormattingEnabled = true;
            ComboBoxWard.Location = new Point(260, 271);
            ComboBoxWard.Name = "ComboBoxWard";
            ComboBoxWard.Size = new Size(150, 21);
            ComboBoxWard.TabIndex = 520;
            ComboBoxWard.TxtVisible = true;
            ComboBoxWard.SelectedIndexChanged += ComboBoxWard_SelectedIndexChanged;
            // 
            // ComboBoxSwapTextBoxBed
            // 
            ComboBoxSwapTextBoxBed.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxSwapTextBoxBed.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxSwapTextBoxBed.FormattingEnabled = true;
            ComboBoxSwapTextBoxBed.Location = new Point(260, 311);
            ComboBoxSwapTextBoxBed.Name = "ComboBoxSwapTextBoxBed";
            ComboBoxSwapTextBoxBed.Size = new Size(150, 21);
            ComboBoxSwapTextBoxBed.TabIndex = 522;
            ComboBoxSwapTextBoxBed.TxtVisible = true;
            ComboBoxSwapTextBoxBed.PreviewKeyDown += ComboBoxSwapTextBoxBed_PreviewKeyDown;
            // 
            // ComboBoxSelectInsurance
            // 
            ComboBoxSelectInsurance.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxSelectInsurance.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxSelectInsurance.FormattingEnabled = true;
            ComboBoxSelectInsurance.Location = new Point(18, 229);
            ComboBoxSelectInsurance.Name = "ComboBoxSelectInsurance";
            ComboBoxSelectInsurance.Size = new Size(361, 21);
            ComboBoxSelectInsurance.TabIndex = 537;
            ComboBoxSelectInsurance.TxtVisible = true;
            ComboBoxSelectInsurance.PreviewKeyDown += ComboBoxSelectInsurance_PreviewKeyDown;
            // 
            // LabelSelectInsurance
            // 
            LabelSelectInsurance.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelSelectInsurance.Location = new Point(15, 210);
            LabelSelectInsurance.Name = "LabelSelectInsurance";
            LabelSelectInsurance.Size = new Size(149, 15);
            LabelSelectInsurance.TabIndex = 538;
            LabelSelectInsurance.Text = "Select Insureance to Bill";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(15, 169);
            label4.Name = "label4";
            label4.Size = new Size(106, 13);
            label4.TabIndex = 536;
            label4.Text = "Insurance Details";
            // 
            // TextBoxAddress
            // 
            TextBoxAddress.BackColor = SystemColors.Window;
            TextBoxAddress.Location = new Point(18, 109);
            TextBoxAddress.Multiline = true;
            TextBoxAddress.Name = "TextBoxAddress";
            TextBoxAddress.ReadOnly = true;
            TextBoxAddress.Size = new Size(361, 57);
            TextBoxAddress.TabIndex = 535;
            TextBoxAddress.TabStop = false;
            // 
            // CheckBoxBillToInsurance
            // 
            CheckBoxBillToInsurance.Enabled = false;
            CheckBoxBillToInsurance.Location = new Point(18, 186);
            CheckBoxBillToInsurance.Name = "CheckBoxBillToInsurance";
            CheckBoxBillToInsurance.Size = new Size(164, 16);
            CheckBoxBillToInsurance.TabIndex = 534;
            CheckBoxBillToInsurance.Text = "Bill to Insurance";
            CheckBoxBillToInsurance.UseVisualStyleBackColor = true;
            CheckBoxBillToInsurance.CheckedChanged += CheckBoxBillToInsurance_CheckedChanged;
            CheckBoxBillToInsurance.PreviewKeyDown += CheckBoxBillToInsurance_PreviewKeyDown;
            // 
            // LabelInsurance
            // 
            LabelInsurance.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point);
            LabelInsurance.ForeColor = Color.ForestGreen;
            LabelInsurance.Location = new Point(425, 335);
            LabelInsurance.Name = "LabelInsurance";
            LabelInsurance.Size = new Size(178, 26);
            LabelInsurance.TabIndex = 539;
            LabelInsurance.Text = "**NO INSURANCE**";
            LabelInsurance.TextAlign = ContentAlignment.TopCenter;
            // 
            // FormIPRegistration
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(628, 493);
            Controls.Add(ComboBoxSwapTextBoxBed);
            Controls.Add(ComboBoxWard);
            Controls.Add(ComboBoxSecondaryNurse);
            Controls.Add(ComboBoxPrimaryNurse);
            Controls.Add(ComboBoxSecondaryConsultant);
            Controls.Add(ComboBoxPrimaryConsultant);
            Controls.Add(ComboBoxSelectInsurance);
            Controls.Add(LabelInsurance);
            Controls.Add(LabelSelectInsurance);
            Controls.Add(label4);
            Controls.Add(TextBoxAddress);
            Controls.Add(CheckBoxBillToInsurance);
            Controls.Add(label6);
            Controls.Add(label7);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label10);
            Controls.Add(label13);
            Controls.Add(GroupBoxPatientAddress);
            Controls.Add(PatientPhoto);
            Controls.Add(TextBoxPatientId);
            Controls.Add(ComboBoxDepartment);
            Controls.Add(TextBoxInpatientAdmissionId);
            Controls.Add(PatientNumberIp);
            Controls.Add(label12);
            Controls.Add(BtnAdmit);
            Controls.Add(statusStrip1);
            Controls.Add(label11);
            Controls.Add(BtnCancel);
            Controls.Add(TextBoxPatientAge);
            Controls.Add(TextBoxPatientDOB);
            Controls.Add(TextBoxPatientName);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormIPRegistration";
            StartPosition = FormStartPosition.CenterParent;
            Text = "In Patient Registration";
            FormClosing += FormIPRegistration_FormClosing;
            Load += FormIPRegistration_Load;
            Controls.SetChildIndex(checkBoxIsPatient, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(label2, 0);
            Controls.SetChildIndex(label3, 0);
            Controls.SetChildIndex(TextBoxPatientName, 0);
            Controls.SetChildIndex(TextBoxPatientDOB, 0);
            Controls.SetChildIndex(TextBoxPatientAge, 0);
            Controls.SetChildIndex(BtnCancel, 0);
            Controls.SetChildIndex(label11, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(BtnAdmit, 0);
            Controls.SetChildIndex(label12, 0);
            Controls.SetChildIndex(PatientNumberIp, 0);
            Controls.SetChildIndex(TextBoxInpatientAdmissionId, 0);
            Controls.SetChildIndex(ComboBoxDepartment, 0);
            Controls.SetChildIndex(TextBoxPatientId, 0);
            Controls.SetChildIndex(PatientPhoto, 0);
            Controls.SetChildIndex(GroupBoxPatientAddress, 0);
            Controls.SetChildIndex(label13, 0);
            Controls.SetChildIndex(label10, 0);
            Controls.SetChildIndex(label8, 0);
            Controls.SetChildIndex(label9, 0);
            Controls.SetChildIndex(label7, 0);
            Controls.SetChildIndex(label6, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(CheckBoxBillToInsurance, 0);
            Controls.SetChildIndex(TextBoxAddress, 0);
            Controls.SetChildIndex(label4, 0);
            Controls.SetChildIndex(LabelSelectInsurance, 0);
            Controls.SetChildIndex(LabelInsurance, 0);
            Controls.SetChildIndex(ComboBoxSelectInsurance, 0);
            Controls.SetChildIndex(ComboBoxPrimaryConsultant, 0);
            Controls.SetChildIndex(ComboBoxSecondaryConsultant, 0);
            Controls.SetChildIndex(ComboBoxPrimaryNurse, 0);
            Controls.SetChildIndex(ComboBoxSecondaryNurse, 0);
            Controls.SetChildIndex(ComboBoxWard, 0);
            Controls.SetChildIndex(ComboBoxSwapTextBoxBed, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox TextBoxPatientName;
        private ExtdTextBox TextBoxPatientDOB;
        private System.Windows.Forms.TextBox TextBoxPatientAge;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnAdmit;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel InPatientErrorMsg;
        private controls.PatientNumberControl PatientNumberIp;
        public System.Windows.Forms.TextBox TextBoxInpatientAdmissionId;
        private System.Windows.Forms.TextBox TextBoxPatientId;
        private controls.PatientPhoto PatientPhoto;
        private controls.text.AddressGroupBoxWithoutLandMark GroupBoxPatientAddress;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox ComboBoxDepartment;
        private System.Windows.Forms.Label label12;
        private controls.ComboBoxSwapTextBox ComboBoxPrimaryConsultant;
        private controls.ComboBoxSwapTextBox ComboBoxSecondaryConsultant;
        private controls.ComboBoxSwapTextBox ComboBoxPrimaryNurse;
        private controls.ComboBoxSwapTextBox ComboBoxSecondaryNurse;
        private controls.ComboBoxSwapTextBox ComboBoxWard;
        private controls.ComboBoxSwapTextBox ComboBoxSwapTextBoxBed;
        private controls.ComboBoxSwapTextBox ComboBoxSelectInsurance;
        private System.Windows.Forms.Label LabelSelectInsurance;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TextBoxAddress;
        private System.Windows.Forms.CheckBox CheckBoxBillToInsurance;
        private System.Windows.Forms.Label LabelInsurance;
    }
}