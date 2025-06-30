namespace fa.views.hms.ip
{
    partial class FormDischargePatient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDischargePatient));
            groupBox1 = new GroupBox();
            ComboBoxDisChargeBy = new controls.ComboBoxSwapTextBox();
            TextBoxWaiveBalance = new controls.text.CurrencyTextBox();
            label9 = new Label();
            linkLabelLedger = new LinkLabel();
            CheckBoxWaive = new CheckBox();
            TextBoxBalance = new controls.text.CurrencyTextBox();
            label8 = new Label();
            CheckBoxDiceased = new CheckBox();
            DateTimePickerDisCharOn = new controls.text.DateWithCalendar();
            label6 = new Label();
            label5 = new Label();
            DateTimePickerNextFollowOn = new controls.text.DateWithCalendar();
            label1 = new Label();
            TextBoxDischargeSummary = new TextBox();
            label4 = new Label();
            TextBoxTreatmentSummary = new TextBox();
            label3 = new Label();
            TextBoxDiagnosisSummary = new TextBox();
            label2 = new Label();
            BtnCancel = new Button();
            BtnSave = new Button();
            BtnExit = new Button();
            statusStrip1 = new StatusStrip();
            ErrorMsgDisPatient = new ToolStripStatusLabel();
            PatientInfo = new controls.hms.PatientInfoMin();
            BtnPrint = new Button();
            BtnDischarge = new Button();
            groupBox1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // PatientIdTransport
            // 
            PatientIdTransport.Size = new Size(100, 21);
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(100, 210);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(100, 183);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(86, 162);
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(ComboBoxDisChargeBy);
            groupBox1.Controls.Add(TextBoxWaiveBalance);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(linkLabelLedger);
            groupBox1.Controls.Add(CheckBoxWaive);
            groupBox1.Controls.Add(TextBoxBalance);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(CheckBoxDiceased);
            groupBox1.Controls.Add(DateTimePickerDisCharOn);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(DateTimePickerNextFollowOn);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(TextBoxDischargeSummary);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(TextBoxTreatmentSummary);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(TextBoxDiagnosisSummary);
            groupBox1.Controls.Add(label2);
            groupBox1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            groupBox1.Location = new Point(206, 5);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(800, 518);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            // 
            // ComboBoxDisChargeBy
            // 
            ComboBoxDisChargeBy.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxDisChargeBy.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxDisChargeBy.FormattingEnabled = true;
            ComboBoxDisChargeBy.Location = new Point(421, 464);
            ComboBoxDisChargeBy.Name = "ComboBoxDisChargeBy";
            ComboBoxDisChargeBy.Size = new Size(262, 21);
            ComboBoxDisChargeBy.TabIndex = 11;
            ComboBoxDisChargeBy.TxtVisible = true;
            ComboBoxDisChargeBy.KeyDown += ComboBoxDisChargeBy_KeyDown;
            ComboBoxDisChargeBy.PreviewKeyDown += ComboBoxDisChargeBy_PreviewKeyDown;
            // 
            // TextBoxWaiveBalance
            // 
            TextBoxWaiveBalance.BackColor = Color.White;
            TextBoxWaiveBalance.Decimals = 2;
            TextBoxWaiveBalance.Length = 10;
            TextBoxWaiveBalance.Location = new Point(420, 336);
            TextBoxWaiveBalance.Name = "TextBoxWaiveBalance";
            TextBoxWaiveBalance.ReadOnly = true;
            TextBoxWaiveBalance.Size = new Size(128, 21);
            TextBoxWaiveBalance.TabIndex = 6;
            TextBoxWaiveBalance.TabStop = false;
            TextBoxWaiveBalance.Text = "0.00";
            TextBoxWaiveBalance.TextAlign = HorizontalAlignment.Right;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label9.Location = new Point(417, 319);
            label9.Name = "label9";
            label9.Size = new Size(77, 13);
            label9.TabIndex = 0;
            label9.Text = "Waive Balance";
            // 
            // linkLabelLedger
            // 
            linkLabelLedger.AutoSize = true;
            linkLabelLedger.Location = new Point(418, 371);
            linkLabelLedger.Name = "linkLabelLedger";
            linkLabelLedger.Size = new Size(106, 13);
            linkLabelLedger.TabIndex = 7;
            linkLabelLedger.TabStop = true;
            linkLabelLedger.Text = "Open Patient Ledger";
            linkLabelLedger.LinkClicked += linkLabelLedger_LinkClicked;
            // 
            // CheckBoxWaive
            // 
            CheckBoxWaive.AutoSize = true;
            CheckBoxWaive.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            CheckBoxWaive.Location = new Point(565, 293);
            CheckBoxWaive.Name = "CheckBoxWaive";
            CheckBoxWaive.Size = new Size(101, 17);
            CheckBoxWaive.TabIndex = 5;
            CheckBoxWaive.TabStop = false;
            CheckBoxWaive.Text = "Waive Balance?";
            CheckBoxWaive.UseVisualStyleBackColor = true;
            CheckBoxWaive.CheckedChanged += CheckBoxWaive_CheckedChanged;
            // 
            // TextBoxBalance
            // 
            TextBoxBalance.BackColor = Color.White;
            TextBoxBalance.Decimals = 2;
            TextBoxBalance.Length = 10;
            TextBoxBalance.Location = new Point(420, 289);
            TextBoxBalance.Name = "TextBoxBalance";
            TextBoxBalance.ReadOnly = true;
            TextBoxBalance.Size = new Size(128, 21);
            TextBoxBalance.TabIndex = 4;
            TextBoxBalance.TabStop = false;
            TextBoxBalance.Text = "0.00";
            TextBoxBalance.TextAlign = HorizontalAlignment.Right;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label8.Location = new Point(417, 272);
            label8.Name = "label8";
            label8.Size = new Size(44, 13);
            label8.TabIndex = 0;
            label8.Text = "Balance";
            // 
            // CheckBoxDiceased
            // 
            CheckBoxDiceased.AutoSize = true;
            CheckBoxDiceased.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            CheckBoxDiceased.Location = new Point(655, 415);
            CheckBoxDiceased.Name = "CheckBoxDiceased";
            CheckBoxDiceased.Size = new Size(111, 17);
            CheckBoxDiceased.TabIndex = 10;
            CheckBoxDiceased.Text = "Patient Diseased?";
            CheckBoxDiceased.UseVisualStyleBackColor = true;
            CheckBoxDiceased.CheckedChanged += CheckBoxDiceased_CheckedChanged;
            // 
            // DateTimePickerDisCharOn
            // 
            DateTimePickerDisCharOn.BackColor = Color.White;
            DateTimePickerDisCharOn.BorderStyle = BorderStyle.FixedSingle;
            DateTimePickerDisCharOn.Date = null;
            DateTimePickerDisCharOn.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DateTimePickerDisCharOn.Format = "MM/dd/yyyy";
            DateTimePickerDisCharOn.Location = new Point(421, 415);
            DateTimePickerDisCharOn.MaxDate = new DateTime(9997, 12, 31, 7, 33, 58, 0);
            DateTimePickerDisCharOn.MinDate = new DateTime(1900, 1, 1, 23, 35, 36, 0);
            DateTimePickerDisCharOn.Name = "DateTimePickerDisCharOn";
            DateTimePickerDisCharOn.ReadOnly = true;
            DateTimePickerDisCharOn.Size = new Size(93, 21);
            DateTimePickerDisCharOn.TabIndex = 8;
            DateTimePickerDisCharOn.TabStop = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label6.Location = new Point(420, 398);
            label6.Name = "label6";
            label6.Size = new Size(77, 13);
            label6.TabIndex = 0;
            label6.Text = "Discharged On";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label5.Location = new Point(421, 448);
            label5.Name = "label5";
            label5.Size = new Size(87, 13);
            label5.TabIndex = 0;
            label5.Text = "Discharged By";
            // 
            // DateTimePickerNextFollowOn
            // 
            DateTimePickerNextFollowOn.BackColor = Color.White;
            DateTimePickerNextFollowOn.BorderStyle = BorderStyle.FixedSingle;
            DateTimePickerNextFollowOn.Date = null;
            DateTimePickerNextFollowOn.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DateTimePickerNextFollowOn.Format = "MM/dd/yyyy";
            DateTimePickerNextFollowOn.Location = new Point(536, 415);
            DateTimePickerNextFollowOn.MaxDate = new DateTime(9997, 12, 31, 7, 33, 58, 0);
            DateTimePickerNextFollowOn.MinDate = new DateTime(1900, 1, 1, 23, 35, 36, 0);
            DateTimePickerNextFollowOn.Name = "DateTimePickerNextFollowOn";
            DateTimePickerNextFollowOn.ReadOnly = false;
            DateTimePickerNextFollowOn.Size = new Size(93, 21);
            DateTimePickerNextFollowOn.TabIndex = 9;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(12, 17);
            label1.Name = "label1";
            label1.Size = new Size(119, 13);
            label1.TabIndex = 0;
            label1.Text = "Diagnosis Summary";
            // 
            // TextBoxDischargeSummary
            // 
            TextBoxDischargeSummary.BackColor = Color.White;
            TextBoxDischargeSummary.Location = new Point(12, 282);
            TextBoxDischargeSummary.MaxLength = 99999;
            TextBoxDischargeSummary.Multiline = true;
            TextBoxDischargeSummary.Name = "TextBoxDischargeSummary";
            TextBoxDischargeSummary.ScrollBars = ScrollBars.Vertical;
            TextBoxDischargeSummary.Size = new Size(377, 225);
            TextBoxDischargeSummary.TabIndex = 3;
            TextBoxDischargeSummary.KeyPress += TextBoxDischargeSummary_KeyPress;
            TextBoxDischargeSummary.PreviewKeyDown += TextBoxDischargeSummary_PreviewKeyDown;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(534, 398);
            label4.Name = "label4";
            label4.Size = new Size(103, 13);
            label4.TabIndex = 0;
            label4.Text = "Next Followup On";
            // 
            // TextBoxTreatmentSummary
            // 
            TextBoxTreatmentSummary.BackColor = Color.White;
            TextBoxTreatmentSummary.Location = new Point(412, 33);
            TextBoxTreatmentSummary.MaxLength = 99999;
            TextBoxTreatmentSummary.Multiline = true;
            TextBoxTreatmentSummary.Name = "TextBoxTreatmentSummary";
            TextBoxTreatmentSummary.ScrollBars = ScrollBars.Vertical;
            TextBoxTreatmentSummary.Size = new Size(377, 225);
            TextBoxTreatmentSummary.TabIndex = 2;
            TextBoxTreatmentSummary.KeyPress += TextBoxTreatmentSummary_KeyPress;
            TextBoxTreatmentSummary.PreviewKeyDown += TextBoxTreatmentSummary_PreviewKeyDown;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(12, 266);
            label3.Name = "label3";
            label3.Size = new Size(121, 13);
            label3.TabIndex = 0;
            label3.Text = "Discharge Summary";
            // 
            // TextBoxDiagnosisSummary
            // 
            TextBoxDiagnosisSummary.BackColor = Color.White;
            TextBoxDiagnosisSummary.Location = new Point(12, 35);
            TextBoxDiagnosisSummary.MaxLength = 99999;
            TextBoxDiagnosisSummary.Multiline = true;
            TextBoxDiagnosisSummary.Name = "TextBoxDiagnosisSummary";
            TextBoxDiagnosisSummary.ScrollBars = ScrollBars.Vertical;
            TextBoxDiagnosisSummary.Size = new Size(377, 225);
            TextBoxDiagnosisSummary.TabIndex = 1;
            TextBoxDiagnosisSummary.KeyPress += TextBoxDiagnosisSummary_KeyPress;
            TextBoxDiagnosisSummary.PreviewKeyDown += TextBoxDiagnosisSummary_PreviewKeyDown;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(412, 17);
            label2.Name = "label2";
            label2.Size = new Size(126, 13);
            label2.TabIndex = 0;
            label2.Text = "Treatment Summary";
            // 
            // BtnCancel
            // 
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(535, 535);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(96, 23);
            BtnCancel.TabIndex = 10;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // BtnSave
            // 
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(637, 535);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(96, 23);
            BtnSave.TabIndex = 12;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            BtnSave.PreviewKeyDown += BtnSave_PreviewKeyDown;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(854, 535);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(118, 23);
            BtnExit.TabIndex = 12;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsgDisPatient });
            statusStrip1.Location = new Point(0, 568);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1008, 22);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsgDisPatient
            // 
            ErrorMsgDisPatient.Name = "ErrorMsgDisPatient";
            ErrorMsgDisPatient.Size = new Size(16, 17);
            ErrorMsgDisPatient.Text = "   ";
            // 
            // PatientInfo
            // 
            PatientInfo.AutoSize = true;
            PatientInfo.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            PatientInfo.Location = new Point(3, 3);
            PatientInfo.Margin = new Padding(4);
            PatientInfo.MaximumSize = new Size(197, 600);
            PatientInfo.MinimumSize = new Size(197, 505);
            PatientInfo.Name = "PatientInfo";
            PatientInfo.PatientChild = Global.SelectGender.Transgender;
            PatientInfo.PatientGender = Global.SelectGender.Transgender;
            PatientInfo.PatientId = null;
            PatientInfo.Short = true;
            PatientInfo.Size = new Size(197, 527);
            PatientInfo.TabIndex = 5;
            PatientInfo.TabStop = false;
            // 
            // BtnPrint
            // 
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(433, 535);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(96, 23);
            BtnPrint.TabIndex = 11;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnDischarge
            // 
            BtnDischarge.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnDischarge.Location = new Point(739, 535);
            BtnDischarge.Name = "BtnDischarge";
            BtnDischarge.Size = new Size(109, 23);
            BtnDischarge.TabIndex = 13;
            BtnDischarge.Text = "Discharge [F7]";
            BtnDischarge.UseVisualStyleBackColor = true;
            BtnDischarge.Click += BtnDischarge_Click;
            // 
            // FormDischargePatient
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1008, 590);
            Controls.Add(BtnDischarge);
            Controls.Add(BtnPrint);
            Controls.Add(PatientInfo);
            Controls.Add(BtnExit);
            Controls.Add(BtnSave);
            Controls.Add(statusStrip1);
            Controls.Add(groupBox1);
            Controls.Add(BtnCancel);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormDischargePatient";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Discharge Patient";
            FormClosing += FormDischargePatient_FormClosing;
            Load += FormDischargePatient_Load;
            Controls.SetChildIndex(BtnCancel, 0);
            Controls.SetChildIndex(groupBox1, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(BtnSave, 0);
            Controls.SetChildIndex(BtnExit, 0);
            Controls.SetChildIndex(PatientInfo, 0);
            Controls.SetChildIndex(BtnPrint, 0);
            Controls.SetChildIndex(BtnDischarge, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private GroupBox groupBox1;
        private Label label1;
        private TextBox TextBoxDiagnosisSummary;
        private controls.text.DateWithCalendar DateTimePickerDisCharOn;
        private Label label6;
        private Label label5;
        private controls.text.DateWithCalendar DateTimePickerNextFollowOn;
        private Label label4;
        private CheckBox CheckBoxDiceased;
        private Label label8;
        private controls.text.CurrencyTextBox TextBoxBalance;
        private Button BtnCancel;
        private Button BtnSave;
        private Button BtnExit;
        private StatusStrip statusStrip1;
        public controls.hms.PatientInfoMin PatientInfo;
        private CheckBox CheckBoxWaive;
        private ToolStripStatusLabel ErrorMsgDisPatient;
        private controls.ComboBoxSwapTextBox ComboBoxDisChargeBy;
        private LinkLabel linkLabelLedger;
        public Button BtnPrint;
        private Button BtnDischarge;
        private controls.text.CurrencyTextBox TextBoxWaiveBalance;
        private Label label9;
        private Button BtnSelectPrescriptionAddCustomRow;
        private TextBox TextBoxTreatmentSummary;
        private Label label2;
        private TextBox TextBoxDischargeSummary;
        private Label label3;
    }
}