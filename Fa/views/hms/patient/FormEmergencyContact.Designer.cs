namespace fa.views.hms.patient
{
    partial class FormEmergencyContact
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmergencyContact));
            statusStrip1 = new StatusStrip();
            EmergencyContErrorMsg = new ToolStripStatusLabel();
            ToolStripStatusLabelErrorEmergency = new ToolStripStatusLabel();
            BtnEmergencyContSave = new Button();
            BtnEmergencyContCancel = new Button();
            backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            TextBoxEmergencyContLastName = new TextBox();
            TextBoxEmergencyContInitial = new TextBox();
            TextBoxEmergencyContFirstName = new TextBox();
            label2 = new Label();
            label1 = new Label();
            RbtEmergencyContGender = new controls.GenderRadio();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            ComboBoxSwapTextBoxEmergencyContRelationShip = new controls.ComboBoxSwapTextBox();
            TextBoxEmergencyContId = new TextBox();
            TextBoxPatientId = new TextBox();
            TextBoxEmergencyContMobile = new controls.text.PhoneTextBox();
            TextBoxEmergencyContPhone = new controls.text.PhoneTextBox();
            label6 = new Label();
            GroupBoxEmergencyContAddress = new controls.AddressGroupBoxWithStateSelection();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // PatientIdTransport
            // 
            PatientIdTransport.Size = new Size(100, 21);
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(365, 276);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(172, 225);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(172, 199);
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // checkBoxIsPatient
            // 
            checkBoxIsPatient.Size = new Size(69, 17);
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { EmergencyContErrorMsg, ToolStripStatusLabelErrorEmergency });
            statusStrip1.Location = new Point(0, 416);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(488, 22);
            statusStrip1.TabIndex = 51;
            statusStrip1.Text = "statusStrip1";
            // 
            // EmergencyContErrorMsg
            // 
            EmergencyContErrorMsg.Name = "EmergencyContErrorMsg";
            EmergencyContErrorMsg.Size = new Size(0, 17);
            // 
            // ToolStripStatusLabelErrorEmergency
            // 
            ToolStripStatusLabelErrorEmergency.BackColor = SystemColors.Control;
            ToolStripStatusLabelErrorEmergency.Name = "ToolStripStatusLabelErrorEmergency";
            ToolStripStatusLabelErrorEmergency.Size = new Size(151, 17);
            ToolStripStatusLabelErrorEmergency.Text = "                                                ";
            // 
            // BtnEmergencyContSave
            // 
            BtnEmergencyContSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnEmergencyContSave.Location = new Point(381, 386);
            BtnEmergencyContSave.Name = "BtnEmergencyContSave";
            BtnEmergencyContSave.Size = new Size(84, 23);
            BtnEmergencyContSave.TabIndex = 8;
            BtnEmergencyContSave.Text = "Save [F8]";
            BtnEmergencyContSave.UseVisualStyleBackColor = true;
            BtnEmergencyContSave.Click += BtnEmergencyContSave_Click;
            // 
            // BtnEmergencyContCancel
            // 
            BtnEmergencyContCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnEmergencyContCancel.Location = new Point(291, 386);
            BtnEmergencyContCancel.Name = "BtnEmergencyContCancel";
            BtnEmergencyContCancel.Size = new Size(84, 23);
            BtnEmergencyContCancel.TabIndex = 9;
            BtnEmergencyContCancel.Text = "Cancel [Esc]";
            BtnEmergencyContCancel.UseVisualStyleBackColor = true;
            BtnEmergencyContCancel.Click += BtnEmergencyContCancel_Click;
            // 
            // TextBoxEmergencyContLastName
            // 
            TextBoxEmergencyContLastName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxEmergencyContLastName.Location = new Point(201, 30);
            TextBoxEmergencyContLastName.MaxLength = 30;
            TextBoxEmergencyContLastName.Name = "TextBoxEmergencyContLastName";
            TextBoxEmergencyContLastName.Size = new Size(178, 21);
            TextBoxEmergencyContLastName.TabIndex = 2;
            // 
            // TextBoxEmergencyContInitial
            // 
            TextBoxEmergencyContInitial.CharacterCasing = CharacterCasing.Upper;
            TextBoxEmergencyContInitial.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxEmergencyContInitial.Location = new Point(178, 30);
            TextBoxEmergencyContInitial.MaxLength = 1;
            TextBoxEmergencyContInitial.Name = "TextBoxEmergencyContInitial";
            TextBoxEmergencyContInitial.Size = new Size(17, 21);
            TextBoxEmergencyContInitial.TabIndex = 1;
            // 
            // TextBoxEmergencyContFirstName
            // 
            TextBoxEmergencyContFirstName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxEmergencyContFirstName.Location = new Point(15, 30);
            TextBoxEmergencyContFirstName.MaxLength = 30;
            TextBoxEmergencyContFirstName.Name = "TextBoxEmergencyContFirstName";
            TextBoxEmergencyContFirstName.Size = new Size(156, 21);
            TextBoxEmergencyContFirstName.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(12, 54);
            label2.Name = "label2";
            label2.Size = new Size(48, 13);
            label2.TabIndex = 28;
            label2.Text = "Gender";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(12, 13);
            label1.Name = "label1";
            label1.Size = new Size(39, 13);
            label1.TabIndex = 27;
            label1.Text = "Name";
            // 
            // RbtEmergencyContGender
            // 
            RbtEmergencyContGender.BackColor = SystemColors.Window;
            RbtEmergencyContGender.BorderStyle = BorderStyle.FixedSingle;
            RbtEmergencyContGender.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            RbtEmergencyContGender.Gender = controls.GenderSelection.Male;
            RbtEmergencyContGender.Location = new Point(15, 70);
            RbtEmergencyContGender.Name = "RbtEmergencyContGender";
            RbtEmergencyContGender.Size = new Size(179, 22);
            RbtEmergencyContGender.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(12, 308);
            label3.Name = "label3";
            label3.Size = new Size(77, 13);
            label3.TabIndex = 52;
            label3.Text = "Relationship";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(12, 349);
            label4.Name = "label4";
            label4.Size = new Size(42, 13);
            label4.TabIndex = 54;
            label4.Text = "Phone";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(142, 349);
            label5.Name = "label5";
            label5.Size = new Size(37, 13);
            label5.TabIndex = 55;
            label5.Text = "Mobile";
            // 
            // ComboBoxSwapTextBoxEmergencyContRelationShip
            // 
            ComboBoxSwapTextBoxEmergencyContRelationShip.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxSwapTextBoxEmergencyContRelationShip.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxSwapTextBoxEmergencyContRelationShip.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ComboBoxSwapTextBoxEmergencyContRelationShip.FormattingEnabled = true;
            ComboBoxSwapTextBoxEmergencyContRelationShip.Items.AddRange(new object[] { "MOTHER", "FATHER", "GRANDPARENT", "GAURDIAN", "FRIEND", "SELF", "HUSBAND", "SPOUSE" });
            ComboBoxSwapTextBoxEmergencyContRelationShip.Location = new Point(15, 324);
            ComboBoxSwapTextBoxEmergencyContRelationShip.Name = "ComboBoxSwapTextBoxEmergencyContRelationShip";
            ComboBoxSwapTextBoxEmergencyContRelationShip.Size = new Size(121, 21);
            ComboBoxSwapTextBoxEmergencyContRelationShip.TabIndex = 5;
            ComboBoxSwapTextBoxEmergencyContRelationShip.TxtVisible = true;
            ComboBoxSwapTextBoxEmergencyContRelationShip.KeyPress += ComboBoxSwapTextBoxEmergencyContRelationShip_KeyPress;
            // 
            // TextBoxEmergencyContId
            // 
            TextBoxEmergencyContId.Location = new Point(316, 324);
            TextBoxEmergencyContId.Name = "TextBoxEmergencyContId";
            TextBoxEmergencyContId.Size = new Size(100, 21);
            TextBoxEmergencyContId.TabIndex = 63;
            TextBoxEmergencyContId.TabStop = false;
            TextBoxEmergencyContId.Visible = false;
            // 
            // TextBoxPatientId
            // 
            TextBoxPatientId.Location = new Point(210, 324);
            TextBoxPatientId.Name = "TextBoxPatientId";
            TextBoxPatientId.Size = new Size(100, 21);
            TextBoxPatientId.TabIndex = 508;
            TextBoxPatientId.TabStop = false;
            TextBoxPatientId.Visible = false;
            // 
            // TextBoxEmergencyContMobile
            // 
            TextBoxEmergencyContMobile.AreaCodeLength = 5;
            TextBoxEmergencyContMobile.Length = 13;
            TextBoxEmergencyContMobile.Location = new Point(142, 365);
            TextBoxEmergencyContMobile.Mask = "#####-#######";
            TextBoxEmergencyContMobile.Name = "TextBoxEmergencyContMobile";
            TextBoxEmergencyContMobile.Size = new Size(100, 21);
            TextBoxEmergencyContMobile.TabIndex = 7;
            // 
            // TextBoxEmergencyContPhone
            // 
            TextBoxEmergencyContPhone.AreaCodeLength = 5;
            TextBoxEmergencyContPhone.Length = 13;
            TextBoxEmergencyContPhone.Location = new Point(15, 365);
            TextBoxEmergencyContPhone.Mask = "#####-#######";
            TextBoxEmergencyContPhone.Name = "TextBoxEmergencyContPhone";
            TextBoxEmergencyContPhone.Size = new Size(121, 21);
            TextBoxEmergencyContPhone.TabIndex = 6;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label6.Location = new Point(12, 95);
            label6.Name = "label6";
            label6.Size = new Size(46, 13);
            label6.TabIndex = 509;
            label6.Text = "Address";
            // 
            // GroupBoxEmergencyContAddress
            // 
            GroupBoxEmergencyContAddress.AddressLine1 = "";
            GroupBoxEmergencyContAddress.AddressLine2 = "";
            GroupBoxEmergencyContAddress.CityName = "";
            GroupBoxEmergencyContAddress.CountryId = 0L;
            GroupBoxEmergencyContAddress.DistrictName = "";
            GroupBoxEmergencyContAddress.GroupName = "";
            GroupBoxEmergencyContAddress.Location = new Point(12, 111);
            GroupBoxEmergencyContAddress.Name = "GroupBoxEmergencyContAddress";
            GroupBoxEmergencyContAddress.PinCode = "";
            GroupBoxEmergencyContAddress.ReadOnly = true;
            GroupBoxEmergencyContAddress.Size = new Size(464, 194);
            GroupBoxEmergencyContAddress.StateId = 0L;
            GroupBoxEmergencyContAddress.StateName = "";
            GroupBoxEmergencyContAddress.StateSelectedIndex = -1;
            GroupBoxEmergencyContAddress.TabIndex = 4;
            // 
            // FormEmergencyContact
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(488, 438);
            Controls.Add(ComboBoxSwapTextBoxEmergencyContRelationShip);
            Controls.Add(GroupBoxEmergencyContAddress);
            Controls.Add(TextBoxEmergencyContMobile);
            Controls.Add(TextBoxEmergencyContPhone);
            Controls.Add(label6);
            Controls.Add(TextBoxPatientId);
            Controls.Add(TextBoxEmergencyContId);
            Controls.Add(label4);
            Controls.Add(label5);
            Controls.Add(label3);
            Controls.Add(statusStrip1);
            Controls.Add(BtnEmergencyContSave);
            Controls.Add(BtnEmergencyContCancel);
            Controls.Add(RbtEmergencyContGender);
            Controls.Add(TextBoxEmergencyContLastName);
            Controls.Add(TextBoxEmergencyContInitial);
            Controls.Add(TextBoxEmergencyContFirstName);
            Controls.Add(label2);
            Controls.Add(label1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormEmergencyContact";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Emergency Contact Details";
            FormClosing += FormEmergencyContact_FormClosing;
            Load += FormEmergencyContact_Load;
            Controls.SetChildIndex(checkBoxIsPatient, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(label2, 0);
            Controls.SetChildIndex(TextBoxEmergencyContFirstName, 0);
            Controls.SetChildIndex(TextBoxEmergencyContInitial, 0);
            Controls.SetChildIndex(TextBoxEmergencyContLastName, 0);
            Controls.SetChildIndex(RbtEmergencyContGender, 0);
            Controls.SetChildIndex(BtnEmergencyContCancel, 0);
            Controls.SetChildIndex(BtnEmergencyContSave, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(label3, 0);
            Controls.SetChildIndex(label5, 0);
            Controls.SetChildIndex(label4, 0);
            Controls.SetChildIndex(TextBoxEmergencyContId, 0);
            Controls.SetChildIndex(TextBoxPatientId, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(label6, 0);
            Controls.SetChildIndex(TextBoxEmergencyContPhone, 0);
            Controls.SetChildIndex(TextBoxEmergencyContMobile, 0);
            Controls.SetChildIndex(GroupBoxEmergencyContAddress, 0);
            Controls.SetChildIndex(ComboBoxSwapTextBoxEmergencyContRelationShip, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusStrip1;
        private Button BtnEmergencyContSave;
        private Button BtnEmergencyContCancel;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private TextBox TextBoxEmergencyContLastName;
        private TextBox TextBoxEmergencyContInitial;
        private TextBox TextBoxEmergencyContFirstName;
        private Label label2;
        private Label label1;
        private controls.GenderRadio RbtEmergencyContGender;
        private Label label3;
        private Label label4;
        private Label label5;
        private controls.ComboBoxSwapTextBox ComboBoxSwapTextBoxEmergencyContRelationShip;
        private ToolStripStatusLabel EmergencyContErrorMsg;
        private TextBox TextBoxEmergencyContId;
        private controls.text.PhoneTextBox TextBoxEmergencyContMobile;
        private controls.text.PhoneTextBox TextBoxEmergencyContPhone;
        public TextBox TextBoxPatientId;
        private Label label6;
        private controls.AddressGroupBoxWithStateSelection GroupBoxEmergencyContAddress;
        private ToolStripStatusLabel ToolStripStatusLabelErrorEmergency;
    }
}