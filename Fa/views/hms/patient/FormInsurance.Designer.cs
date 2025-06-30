namespace fa.views.hms.patient
{
    partial class FormInsurance
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInsurance));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            TextBoxInsuranceName = new TextBox();
            TextBoxInsurancePolicy = new TextBox();
            TextBoxInsuranceGroup = new TextBox();
            label7 = new Label();
            RbtInsurancePrimaryInsurance = new controls.YesNoRadio();
            ComboBoxSwapTextBoxInsuranceInsurer = new controls.ComboBoxSwapTextBox();
            TextBoxInsuranceEmployerName = new TextBox();
            TextBoxInsuranceAdditionalInfo = new TextBox();
            label8 = new Label();
            statusStrip1 = new StatusStrip();
            InsuranceErrorMsg = new ToolStripStatusLabel();
            ToolStripStatusLabelErrorInsurance = new ToolStripStatusLabel();
            BtnInsuranceCancel = new Button();
            BtnInsuranceSave = new Button();
            TextBoxInsuranceId = new TextBox();
            ComboBoxSwapTextBoxInsurerRelationShip = new controls.ComboBoxSwapTextBox();
            label11 = new Label();
            TextBoxPatientId = new TextBox();
            TextBoxInsurancePhone = new controls.text.PhoneTextBox();
            label9 = new Label();
            RbtInsuranceActiveInsurance = new controls.YesNoRadio();
            label10 = new Label();
            groupBox1 = new GroupBox();
            CheckBoxOPCoverage = new CheckBox();
            CheckBoxIPCoverage = new CheckBox();
            GroupBoxInsuranceAddress = new controls.AddressGroupBoxWithStateSelection();
            statusStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // PatientIdTransport
            // 
            PatientIdTransport.Size = new Size(100, 21);
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(172, 325);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(172, 299);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(172, 273);
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // checkBoxIsPatient
            // 
            checkBoxIsPatient.Location = new Point(234, 181);
            checkBoxIsPatient.Size = new Size(69, 17);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(109, 13);
            label1.TabIndex = 0;
            label1.Text = "Carrier/Insurance";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(12, 55);
            label2.Name = "label2";
            label2.Size = new Size(52, 13);
            label2.TabIndex = 1;
            label2.Text = "Policy #";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(187, 55);
            label3.Name = "label3";
            label3.Size = new Size(53, 13);
            label3.TabIndex = 2;
            label3.Text = "Group #";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(12, 141);
            label4.Name = "label4";
            label4.Size = new Size(49, 13);
            label4.TabIndex = 3;
            label4.Text = "Insurer";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(12, 221);
            label5.Name = "label5";
            label5.Size = new Size(81, 13);
            label5.TabIndex = 4;
            label5.Text = "Employer Name";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label6.Location = new Point(24, 472);
            label6.Name = "label6";
            label6.Size = new Size(84, 13);
            label6.TabIndex = 5;
            label6.Text = "Employer Phone";
            // 
            // TextBoxInsuranceName
            // 
            TextBoxInsuranceName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxInsuranceName.Location = new Point(15, 31);
            TextBoxInsuranceName.MaxLength = 35;
            TextBoxInsuranceName.Name = "TextBoxInsuranceName";
            TextBoxInsuranceName.Size = new Size(383, 21);
            TextBoxInsuranceName.TabIndex = 0;
            // 
            // TextBoxInsurancePolicy
            // 
            TextBoxInsurancePolicy.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxInsurancePolicy.Location = new Point(15, 71);
            TextBoxInsurancePolicy.MaxLength = 35;
            TextBoxInsurancePolicy.Name = "TextBoxInsurancePolicy";
            TextBoxInsurancePolicy.Size = new Size(139, 21);
            TextBoxInsurancePolicy.TabIndex = 1;
            // 
            // TextBoxInsuranceGroup
            // 
            TextBoxInsuranceGroup.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxInsuranceGroup.Location = new Point(190, 71);
            TextBoxInsuranceGroup.MaxLength = 35;
            TextBoxInsuranceGroup.Name = "TextBoxInsuranceGroup";
            TextBoxInsuranceGroup.Size = new Size(139, 21);
            TextBoxInsuranceGroup.TabIndex = 2;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label7.Location = new Point(12, 97);
            label7.Name = "label7";
            label7.Size = new Size(112, 13);
            label7.TabIndex = 9;
            label7.Text = "Primary Insurance";
            // 
            // RbtInsurancePrimaryInsurance
            // 
            RbtInsurancePrimaryInsurance.BackColor = SystemColors.Window;
            RbtInsurancePrimaryInsurance.Checked = true;
            RbtInsurancePrimaryInsurance.FirstButtonName = "Yes";
            RbtInsurancePrimaryInsurance.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            RbtInsurancePrimaryInsurance.Location = new Point(13, 116);
            RbtInsurancePrimaryInsurance.Name = "RbtInsurancePrimaryInsurance";
            RbtInsurancePrimaryInsurance.SecondButtonName = "No  ";
            RbtInsurancePrimaryInsurance.Size = new Size(107, 23);
            RbtInsurancePrimaryInsurance.TabIndex = 3;
            // 
            // ComboBoxSwapTextBoxInsuranceInsurer
            // 
            ComboBoxSwapTextBoxInsuranceInsurer.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxSwapTextBoxInsuranceInsurer.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxSwapTextBoxInsuranceInsurer.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ComboBoxSwapTextBoxInsuranceInsurer.FormattingEnabled = true;
            ComboBoxSwapTextBoxInsuranceInsurer.Location = new Point(15, 157);
            ComboBoxSwapTextBoxInsuranceInsurer.Name = "ComboBoxSwapTextBoxInsuranceInsurer";
            ComboBoxSwapTextBoxInsuranceInsurer.Size = new Size(263, 21);
            ComboBoxSwapTextBoxInsuranceInsurer.TabIndex = 5;
            ComboBoxSwapTextBoxInsuranceInsurer.TxtVisible = true;
            ComboBoxSwapTextBoxInsuranceInsurer.KeyPress += ComboBoxSwapTextBoxInsuranceInsurer_KeyPress;
            // 
            // TextBoxInsuranceEmployerName
            // 
            TextBoxInsuranceEmployerName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxInsuranceEmployerName.Location = new Point(15, 237);
            TextBoxInsuranceEmployerName.Name = "TextBoxInsuranceEmployerName";
            TextBoxInsuranceEmployerName.Size = new Size(383, 21);
            TextBoxInsuranceEmployerName.TabIndex = 7;
            // 
            // TextBoxInsuranceAdditionalInfo
            // 
            TextBoxInsuranceAdditionalInfo.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxInsuranceAdditionalInfo.Location = new Point(483, 116);
            TextBoxInsuranceAdditionalInfo.MaxLength = 250;
            TextBoxInsuranceAdditionalInfo.Multiline = true;
            TextBoxInsuranceAdditionalInfo.Name = "TextBoxInsuranceAdditionalInfo";
            TextBoxInsuranceAdditionalInfo.Size = new Size(264, 85);
            TextBoxInsuranceAdditionalInfo.TabIndex = 12;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label8.Location = new Point(483, 97);
            label8.Name = "label8";
            label8.Size = new Size(113, 13);
            label8.TabIndex = 16;
            label8.Text = "Additional Information";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { InsuranceErrorMsg, ToolStripStatusLabelErrorInsurance });
            statusStrip1.Location = new Point(0, 535);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(762, 22);
            statusStrip1.TabIndex = 17;
            statusStrip1.Text = "statusStrip1";
            // 
            // InsuranceErrorMsg
            // 
            InsuranceErrorMsg.Name = "InsuranceErrorMsg";
            InsuranceErrorMsg.Size = new Size(0, 17);
            // 
            // ToolStripStatusLabelErrorInsurance
            // 
            ToolStripStatusLabelErrorInsurance.BackColor = SystemColors.Control;
            ToolStripStatusLabelErrorInsurance.Name = "ToolStripStatusLabelErrorInsurance";
            ToolStripStatusLabelErrorInsurance.Size = new Size(151, 17);
            ToolStripStatusLabelErrorInsurance.Text = "                                                ";
            // 
            // BtnInsuranceCancel
            // 
            BtnInsuranceCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnInsuranceCancel.Location = new Point(573, 489);
            BtnInsuranceCancel.Name = "BtnInsuranceCancel";
            BtnInsuranceCancel.Size = new Size(84, 23);
            BtnInsuranceCancel.TabIndex = 14;
            BtnInsuranceCancel.Text = "Cancel [Esc]";
            BtnInsuranceCancel.UseVisualStyleBackColor = true;
            BtnInsuranceCancel.Click += BtnInsuranceCancel_Click;
            // 
            // BtnInsuranceSave
            // 
            BtnInsuranceSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnInsuranceSave.Location = new Point(663, 489);
            BtnInsuranceSave.Name = "BtnInsuranceSave";
            BtnInsuranceSave.Size = new Size(84, 23);
            BtnInsuranceSave.TabIndex = 13;
            BtnInsuranceSave.Text = "Save [F8]";
            BtnInsuranceSave.UseVisualStyleBackColor = true;
            BtnInsuranceSave.Click += BtnInsuranceSave_Click;
            // 
            // TextBoxInsuranceId
            // 
            TextBoxInsuranceId.Location = new Point(438, 455);
            TextBoxInsuranceId.Name = "TextBoxInsuranceId";
            TextBoxInsuranceId.Size = new Size(100, 21);
            TextBoxInsuranceId.TabIndex = 20;
            TextBoxInsuranceId.TabStop = false;
            TextBoxInsuranceId.Visible = false;
            // 
            // ComboBoxSwapTextBoxInsurerRelationShip
            // 
            ComboBoxSwapTextBoxInsurerRelationShip.AutoCompleteCustomSource.AddRange(new string[] { "MOTHER", "FATHER", "GRANDPARENT", "GAURDIAN", "FRIEND", "SELF" });
            ComboBoxSwapTextBoxInsurerRelationShip.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxSwapTextBoxInsurerRelationShip.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxSwapTextBoxInsurerRelationShip.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ComboBoxSwapTextBoxInsurerRelationShip.FormattingEnabled = true;
            ComboBoxSwapTextBoxInsurerRelationShip.Items.AddRange(new object[] { "MOTHER", "FATHER", "GRANDPARENT", "GAURDIAN", "FRIEND", "SELF", "HUSBAND", "SPOUSE" });
            ComboBoxSwapTextBoxInsurerRelationShip.Location = new Point(15, 197);
            ComboBoxSwapTextBoxInsurerRelationShip.Name = "ComboBoxSwapTextBoxInsurerRelationShip";
            ComboBoxSwapTextBoxInsurerRelationShip.Size = new Size(128, 21);
            ComboBoxSwapTextBoxInsurerRelationShip.TabIndex = 6;
            ComboBoxSwapTextBoxInsurerRelationShip.TxtVisible = true;
            ComboBoxSwapTextBoxInsurerRelationShip.KeyPress += ComboBoxSwapTextBoxInsuranceInsurer_KeyPress;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label11.Location = new Point(12, 181);
            label11.Name = "label11";
            label11.Size = new Size(77, 13);
            label11.TabIndex = 28;
            label11.Text = "Relationship";
            // 
            // TextBoxPatientId
            // 
            TextBoxPatientId.Location = new Point(332, 455);
            TextBoxPatientId.Name = "TextBoxPatientId";
            TextBoxPatientId.Size = new Size(100, 21);
            TextBoxPatientId.TabIndex = 509;
            TextBoxPatientId.TabStop = false;
            TextBoxPatientId.Visible = false;
            // 
            // TextBoxInsurancePhone
            // 
            TextBoxInsurancePhone.AreaCodeLength = 5;
            TextBoxInsurancePhone.Length = 13;
            TextBoxInsurancePhone.Location = new Point(24, 490);
            TextBoxInsurancePhone.Mask = "#####-#######";
            TextBoxInsurancePhone.Name = "TextBoxInsurancePhone";
            TextBoxInsurancePhone.Size = new Size(100, 21);
            TextBoxInsurancePhone.TabIndex = 9;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label9.Location = new Point(12, 261);
            label9.Name = "label9";
            label9.Size = new Size(46, 13);
            label9.TabIndex = 512;
            label9.Text = "Address";
            // 
            // RbtInsuranceActiveInsurance
            // 
            RbtInsuranceActiveInsurance.BackColor = SystemColors.Window;
            RbtInsuranceActiveInsurance.Checked = true;
            RbtInsuranceActiveInsurance.FirstButtonName = "Yes";
            RbtInsuranceActiveInsurance.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            RbtInsuranceActiveInsurance.Location = new Point(150, 116);
            RbtInsuranceActiveInsurance.Name = "RbtInsuranceActiveInsurance";
            RbtInsuranceActiveInsurance.SecondButtonName = "No  ";
            RbtInsuranceActiveInsurance.Size = new Size(107, 23);
            RbtInsuranceActiveInsurance.TabIndex = 4;
            // 
            // label10
            // 
            label10.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label10.Location = new Point(149, 97);
            label10.Name = "label10";
            label10.Size = new Size(75, 13);
            label10.TabIndex = 514;
            label10.Text = "Active";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(CheckBoxOPCoverage);
            groupBox1.Controls.Add(CheckBoxIPCoverage);
            groupBox1.Location = new Point(483, 15);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(264, 75);
            groupBox1.TabIndex = 10;
            groupBox1.TabStop = false;
            groupBox1.Text = "Coverage";
            // 
            // CheckBoxOPCoverage
            // 
            CheckBoxOPCoverage.AutoSize = true;
            CheckBoxOPCoverage.Location = new Point(19, 49);
            CheckBoxOPCoverage.Name = "CheckBoxOPCoverage";
            CheckBoxOPCoverage.Size = new Size(40, 17);
            CheckBoxOPCoverage.TabIndex = 11;
            CheckBoxOPCoverage.Text = "OP";
            CheckBoxOPCoverage.UseVisualStyleBackColor = true;
            // 
            // CheckBoxIPCoverage
            // 
            CheckBoxIPCoverage.AutoSize = true;
            CheckBoxIPCoverage.Location = new Point(19, 26);
            CheckBoxIPCoverage.Name = "CheckBoxIPCoverage";
            CheckBoxIPCoverage.Size = new Size(36, 17);
            CheckBoxIPCoverage.TabIndex = 10;
            CheckBoxIPCoverage.Text = "IP";
            CheckBoxIPCoverage.UseVisualStyleBackColor = true;
            // 
            // GroupBoxInsuranceAddress
            // 
            GroupBoxInsuranceAddress.AddressLine1 = "";
            GroupBoxInsuranceAddress.AddressLine2 = "";
            GroupBoxInsuranceAddress.CityName = "";
            GroupBoxInsuranceAddress.CountryId = 0L;
            GroupBoxInsuranceAddress.DistrictName = "";
            GroupBoxInsuranceAddress.GroupName = "";
            GroupBoxInsuranceAddress.Location = new Point(12, 275);
            GroupBoxInsuranceAddress.Name = "GroupBoxInsuranceAddress";
            GroupBoxInsuranceAddress.PinCode = "";
            GroupBoxInsuranceAddress.ReadOnly = true;
            GroupBoxInsuranceAddress.Size = new Size(465, 196);
            GroupBoxInsuranceAddress.StateId = 0L;
            GroupBoxInsuranceAddress.StateName = "";
            GroupBoxInsuranceAddress.StateSelectedIndex = -1;
            GroupBoxInsuranceAddress.TabIndex = 8;
            // 
            // FormInsurance
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(762, 557);
            Controls.Add(ComboBoxSwapTextBoxInsuranceInsurer);
            Controls.Add(ComboBoxSwapTextBoxInsurerRelationShip);
            Controls.Add(GroupBoxInsuranceAddress);
            Controls.Add(groupBox1);
            Controls.Add(RbtInsuranceActiveInsurance);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(TextBoxInsurancePhone);
            Controls.Add(TextBoxPatientId);
            Controls.Add(label11);
            Controls.Add(TextBoxInsuranceId);
            Controls.Add(BtnInsuranceSave);
            Controls.Add(BtnInsuranceCancel);
            Controls.Add(TextBoxInsuranceAdditionalInfo);
            Controls.Add(statusStrip1);
            Controls.Add(TextBoxInsuranceEmployerName);
            Controls.Add(label8);
            Controls.Add(RbtInsurancePrimaryInsurance);
            Controls.Add(label7);
            Controls.Add(TextBoxInsuranceGroup);
            Controls.Add(TextBoxInsurancePolicy);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(TextBoxInsuranceName);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormInsurance";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Insurance Details";
            FormClosing += FormInsurance_FormClosing;
            Load += FormInsurance_Load;
            Controls.SetChildIndex(checkBoxIsPatient, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(label2, 0);
            Controls.SetChildIndex(label3, 0);
            Controls.SetChildIndex(label4, 0);
            Controls.SetChildIndex(TextBoxInsuranceName, 0);
            Controls.SetChildIndex(label5, 0);
            Controls.SetChildIndex(label6, 0);
            Controls.SetChildIndex(TextBoxInsurancePolicy, 0);
            Controls.SetChildIndex(TextBoxInsuranceGroup, 0);
            Controls.SetChildIndex(label7, 0);
            Controls.SetChildIndex(RbtInsurancePrimaryInsurance, 0);
            Controls.SetChildIndex(label8, 0);
            Controls.SetChildIndex(TextBoxInsuranceEmployerName, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(TextBoxInsuranceAdditionalInfo, 0);
            Controls.SetChildIndex(BtnInsuranceCancel, 0);
            Controls.SetChildIndex(BtnInsuranceSave, 0);
            Controls.SetChildIndex(TextBoxInsuranceId, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(label11, 0);
            Controls.SetChildIndex(TextBoxPatientId, 0);
            Controls.SetChildIndex(TextBoxInsurancePhone, 0);
            Controls.SetChildIndex(label9, 0);
            Controls.SetChildIndex(label10, 0);
            Controls.SetChildIndex(RbtInsuranceActiveInsurance, 0);
            Controls.SetChildIndex(groupBox1, 0);
            Controls.SetChildIndex(GroupBoxInsuranceAddress, 0);
            Controls.SetChildIndex(ComboBoxSwapTextBoxInsurerRelationShip, 0);
            Controls.SetChildIndex(ComboBoxSwapTextBoxInsuranceInsurer, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox TextBoxInsuranceName;
        private TextBox TextBoxInsurancePolicy;
        private TextBox TextBoxInsuranceGroup;
        private Label label7;
        private controls.YesNoRadio RbtInsurancePrimaryInsurance;
        private controls.ComboBoxSwapTextBox ComboBoxSwapTextBoxInsuranceInsurer;
        private TextBox TextBoxInsuranceEmployerName;
        private TextBox TextBoxInsuranceAdditionalInfo;
        private Label label8;
        private StatusStrip statusStrip1;
        private Button BtnInsuranceCancel;
        private Button BtnInsuranceSave;
        private ToolStripStatusLabel InsuranceErrorMsg;
        private controls.ComboBoxSwapTextBox ComboBoxSwapTextBoxInsurerRelationShip;
        private Label label11;
        private TextBox TextBoxInsuranceId;
        private controls.text.PhoneTextBox TextBoxInsurancePhone;
        public TextBox TextBoxPatientId;
        private Label label9;
        private controls.YesNoRadio RbtInsuranceActiveInsurance;
        private Label label10;
        private GroupBox groupBox1;
        private CheckBox CheckBoxOPCoverage;
        private CheckBox CheckBoxIPCoverage;
        private controls.AddressGroupBoxWithStateSelection GroupBoxInsuranceAddress;
        private ToolStripStatusLabel ToolStripStatusLabelErrorInsurance;
    }
}