namespace Fa.views.hms.op
{
    partial class FormAppointment
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
            toolStripAppointment = new ToolStrip();
            LabelFrom = new ToolStripLabel();
            FromDate = new fa.views.controls.ToolStripCalendar();
            toolStripSeparator4 = new ToolStripSeparator();
            toolStripLabel1 = new ToolStripLabel();
            ComboBoxStartingTime = new ToolStripComboBox();
            toolStripSeparator1 = new ToolStripSeparator();
            LabelTo = new ToolStripLabel();
            ToDate = new fa.views.controls.ToolStripCalendar();
            toolStripSeparator2 = new ToolStripSeparator();
            toolStripLabel3 = new ToolStripLabel();
            ComboBoxEndTime = new ToolStripComboBox();
            toolStripSeparator3 = new ToolStripSeparator();
            labelDuration = new ToolStripLabel();
            statusStripAppointment = new StatusStrip();
            OpRegistrationErrorMsg = new ToolStripStatusLabel();
            AppointmentErrMsg = new ToolStripStatusLabel();
            groupBoxAppointment = new GroupBox();
            ComboBoxConsultant = new fa.views.controls.ComboBoxSwapTextBox();
            labelAppointmentErrMsg = new Label();
            TextBoxAppointmentDepartment = new TextBox();
            textBoxAppointmentID = new TextBox();
            TextBoxPatientId = new TextBox();
            label8 = new Label();
            BtnSave = new Button();
            buttonDelete = new Button();
            BtnCancel = new Button();
            PatientPhoto = new fa.views.controls.PatientPhoto();
            PatientNumberOp = new fa.views.controls.PatientNumberControl();
            TextBoxAppointmentReasonForVisit = new TextBox();
            label7 = new Label();
            label6 = new Label();
            TextBoxAptPatientAddress = new TextBox();
            label5 = new Label();
            BtnNewPatient = new Button();
            BtnPatientSearch = new Button();
            PatientGender = new fa.views.controls.GenderRadio();
            label4 = new Label();
            TextBoxAppointmentAge = new TextBox();
            TextBoxAppointmentDOB = new TextBox();
            label3 = new Label();
            label2 = new Label();
            TextBoxAppointmentName = new TextBox();
            label1 = new Label();
            toolStripAppointment.SuspendLayout();
            statusStripAppointment.SuspendLayout();
            groupBoxAppointment.SuspendLayout();
            SuspendLayout();
            // 
            // PatientIdTransport
            // 
            PatientIdTransport.Size = new Size(116, 21);
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Size = new Size(135, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Size = new Size(135, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Size = new Size(135, 21);
            // 
            // checkBoxIsPatient
            // 
            checkBoxIsPatient.Size = new Size(69, 17);
            // 
            // toolStripAppointment
            // 
            toolStripAppointment.AutoSize = false;
            toolStripAppointment.BackColor = SystemColors.ControlLight;
            toolStripAppointment.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripAppointment.GripStyle = ToolStripGripStyle.Hidden;
            toolStripAppointment.ImageScalingSize = new Size(20, 20);
            toolStripAppointment.Items.AddRange(new ToolStripItem[] { LabelFrom, FromDate, toolStripSeparator4, toolStripLabel1, ComboBoxStartingTime, toolStripSeparator1, LabelTo, ToDate, toolStripSeparator2, toolStripLabel3, ComboBoxEndTime, toolStripSeparator3, labelDuration });
            toolStripAppointment.Location = new Point(0, 0);
            toolStripAppointment.Margin = new Padding(1);
            toolStripAppointment.Name = "toolStripAppointment";
            toolStripAppointment.Padding = new Padding(3, 5, 3, 3);
            toolStripAppointment.Size = new Size(654, 32);
            toolStripAppointment.TabIndex = 2;
            toolStripAppointment.Text = "toolStrip1";
            // 
            // LabelFrom
            // 
            LabelFrom.Name = "LabelFrom";
            LabelFrom.Size = new Size(31, 21);
            LabelFrom.Text = "From";
            // 
            // FromDate
            // 
            FromDate.BackColor = Color.White;
            FromDate.Date = null;
            FromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FromDate.Format = "MM/dd/yyyy";
            FromDate.MaxDate = new DateTime(9997, 12, 31, 9, 8, 46, 0);
            FromDate.MinDate = new DateTime(1900, 1, 1, 23, 1, 20, 0);
            FromDate.Name = "FromDate";
            FromDate.Size = new Size(97, 21);
            FromDate.Text = "Calender";
            FromDate.ValueChanged += FromDate_ValueChanged;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 24);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(29, 21);
            toolStripLabel1.Text = "Time";
            // 
            // ComboBoxStartingTime
            // 
            ComboBoxStartingTime.CausesValidation = false;
            ComboBoxStartingTime.FlatStyle = FlatStyle.Standard;
            ComboBoxStartingTime.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ComboBoxStartingTime.Margin = new Padding(0, 1, 0, 2);
            ComboBoxStartingTime.Name = "ComboBoxStartingTime";
            ComboBoxStartingTime.Size = new Size(80, 21);
            ComboBoxStartingTime.SelectedIndexChanged += ComboBoxStartingTime_SelectedIndexChanged;
            ComboBoxStartingTime.Leave += ComboBoxStartingTime_Leave;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 24);
            // 
            // LabelTo
            // 
            LabelTo.Name = "LabelTo";
            LabelTo.Size = new Size(19, 21);
            LabelTo.Text = "To";
            // 
            // ToDate
            // 
            ToDate.BackColor = Color.White;
            ToDate.Date = null;
            ToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ToDate.Format = "MM/dd/yyyy";
            ToDate.MaxDate = new DateTime(9997, 12, 31, 9, 8, 46, 0);
            ToDate.MinDate = new DateTime(1900, 1, 1, 23, 1, 20, 0);
            ToDate.Name = "ToDate";
            ToDate.Size = new Size(97, 21);
            ToDate.Text = "Calender";
            ToDate.ValueChanged += ToDate_ValueChanged;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 24);
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new Size(29, 21);
            toolStripLabel3.Text = "Time";
            // 
            // ComboBoxEndTime
            // 
            ComboBoxEndTime.CausesValidation = false;
            ComboBoxEndTime.FlatStyle = FlatStyle.Standard;
            ComboBoxEndTime.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ComboBoxEndTime.Margin = new Padding(0, 1, 0, 2);
            ComboBoxEndTime.Name = "ComboBoxEndTime";
            ComboBoxEndTime.Size = new Size(80, 21);
            ComboBoxEndTime.SelectedIndexChanged += ComboBoxEndTime_SelectedIndexChanged;
            ComboBoxEndTime.Leave += ComboBoxEndTime_Leave;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 24);
            // 
            // labelDuration
            // 
            labelDuration.Name = "labelDuration";
            labelDuration.Size = new Size(28, 21);
            labelDuration.Text = "       ";
            // 
            // statusStripAppointment
            // 
            statusStripAppointment.ImageScalingSize = new Size(20, 20);
            statusStripAppointment.Items.AddRange(new ToolStripItem[] { OpRegistrationErrorMsg, AppointmentErrMsg });
            statusStripAppointment.Location = new Point(0, 581);
            statusStripAppointment.Name = "statusStripAppointment";
            statusStripAppointment.Size = new Size(654, 22);
            statusStripAppointment.TabIndex = 3;
            statusStripAppointment.Text = "statusStrip1";
            // 
            // OpRegistrationErrorMsg
            // 
            OpRegistrationErrorMsg.BackColor = SystemColors.Control;
            OpRegistrationErrorMsg.Name = "OpRegistrationErrorMsg";
            OpRegistrationErrorMsg.Size = new Size(0, 17);
            // 
            // AppointmentErrMsg
            // 
            AppointmentErrMsg.Name = "AppointmentErrMsg";
            AppointmentErrMsg.Size = new Size(25, 17);
            AppointmentErrMsg.Text = "      ";
            // 
            // groupBoxAppointment
            // 
            groupBoxAppointment.Controls.Add(ComboBoxConsultant);
            groupBoxAppointment.Controls.Add(labelAppointmentErrMsg);
            groupBoxAppointment.Controls.Add(TextBoxAppointmentDepartment);
            groupBoxAppointment.Controls.Add(textBoxAppointmentID);
            groupBoxAppointment.Controls.Add(TextBoxPatientId);
            groupBoxAppointment.Controls.Add(label8);
            groupBoxAppointment.Controls.Add(BtnSave);
            groupBoxAppointment.Controls.Add(buttonDelete);
            groupBoxAppointment.Controls.Add(BtnCancel);
            groupBoxAppointment.Controls.Add(PatientPhoto);
            groupBoxAppointment.Controls.Add(PatientNumberOp);
            groupBoxAppointment.Controls.Add(TextBoxAppointmentReasonForVisit);
            groupBoxAppointment.Controls.Add(label7);
            groupBoxAppointment.Controls.Add(label6);
            groupBoxAppointment.Controls.Add(TextBoxAptPatientAddress);
            groupBoxAppointment.Controls.Add(label5);
            groupBoxAppointment.Controls.Add(BtnNewPatient);
            groupBoxAppointment.Controls.Add(BtnPatientSearch);
            groupBoxAppointment.Controls.Add(PatientGender);
            groupBoxAppointment.Controls.Add(label4);
            groupBoxAppointment.Controls.Add(TextBoxAppointmentAge);
            groupBoxAppointment.Controls.Add(TextBoxAppointmentDOB);
            groupBoxAppointment.Controls.Add(label3);
            groupBoxAppointment.Controls.Add(label2);
            groupBoxAppointment.Controls.Add(TextBoxAppointmentName);
            groupBoxAppointment.Controls.Add(label1);
            groupBoxAppointment.Location = new Point(4, 34);
            groupBoxAppointment.Name = "groupBoxAppointment";
            groupBoxAppointment.Size = new Size(645, 536);
            groupBoxAppointment.TabIndex = 4;
            groupBoxAppointment.TabStop = false;
            // 
            // ComboBoxConsultant
            // 
            ComboBoxConsultant.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxConsultant.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxConsultant.FormattingEnabled = true;
            ComboBoxConsultant.Location = new Point(7, 291);
            ComboBoxConsultant.Name = "ComboBoxConsultant";
            ComboBoxConsultant.Size = new Size(361, 21);
            ComboBoxConsultant.TabIndex = 102;
            ComboBoxConsultant.TxtVisible = true;
            ComboBoxConsultant.SelectedIndexChanged += ComboBoxConsultant_SelectedIndexChanged;
            ComboBoxConsultant.TextChanged += ComboBoxConsultant_SelectedIndexChanged;
            // 
            // labelAppointmentErrMsg
            // 
            labelAppointmentErrMsg.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point);
            labelAppointmentErrMsg.ForeColor = Color.Red;
            labelAppointmentErrMsg.Location = new Point(445, 291);
            labelAppointmentErrMsg.Name = "labelAppointmentErrMsg";
            labelAppointmentErrMsg.Size = new Size(193, 99);
            labelAppointmentErrMsg.TabIndex = 103;
            labelAppointmentErrMsg.Text = " ";
            labelAppointmentErrMsg.Visible = false;
            // 
            // TextBoxAppointmentDepartment
            // 
            TextBoxAppointmentDepartment.BackColor = SystemColors.Window;
            TextBoxAppointmentDepartment.Location = new Point(7, 350);
            TextBoxAppointmentDepartment.Name = "TextBoxAppointmentDepartment";
            TextBoxAppointmentDepartment.ReadOnly = true;
            TextBoxAppointmentDepartment.Size = new Size(361, 21);
            TextBoxAppointmentDepartment.TabIndex = 101;
            // 
            // textBoxAppointmentID
            // 
            textBoxAppointmentID.Location = new Point(231, 502);
            textBoxAppointmentID.Name = "textBoxAppointmentID";
            textBoxAppointmentID.Size = new Size(100, 21);
            textBoxAppointmentID.TabIndex = 98;
            textBoxAppointmentID.Visible = false;
            // 
            // TextBoxPatientId
            // 
            TextBoxPatientId.Location = new Point(125, 502);
            TextBoxPatientId.Name = "TextBoxPatientId";
            TextBoxPatientId.Size = new Size(100, 21);
            TextBoxPatientId.TabIndex = 98;
            TextBoxPatientId.Visible = false;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label8.Location = new Point(8, 327);
            label8.Name = "label8";
            label8.Size = new Size(64, 13);
            label8.TabIndex = 97;
            label8.Text = "Department";
            // 
            // BtnSave
            // 
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(553, 502);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(70, 23);
            BtnSave.TabIndex = 93;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // buttonDelete
            // 
            buttonDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonDelete.Location = new Point(463, 502);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(84, 23);
            buttonDelete.TabIndex = 94;
            buttonDelete.Text = "Delete [F4]";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += buttonDelete_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(373, 502);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(84, 23);
            BtnCancel.TabIndex = 94;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // PatientPhoto
            // 
            PatientPhoto.Location = new Point(445, 14);
            PatientPhoto.Margin = new Padding(4, 3, 4, 3);
            PatientPhoto.Name = "PatientPhoto";
            PatientPhoto.Size = new Size(155, 190);
            PatientPhoto.TabIndex = 87;
            // 
            // PatientNumberOp
            // 
            PatientNumberOp.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            PatientNumberOp.Location = new Point(445, 210);
            PatientNumberOp.Margin = new Padding(4, 3, 4, 3);
            PatientNumberOp.Name = "PatientNumberOp";
            PatientNumberOp.PatientNumber = "";
            PatientNumberOp.Size = new Size(194, 79);
            PatientNumberOp.TabIndex = 90;
            // 
            // TextBoxAppointmentReasonForVisit
            // 
            TextBoxAppointmentReasonForVisit.BackColor = Color.White;
            TextBoxAppointmentReasonForVisit.Location = new Point(8, 414);
            TextBoxAppointmentReasonForVisit.MaxLength = 310;
            TextBoxAppointmentReasonForVisit.Multiline = true;
            TextBoxAppointmentReasonForVisit.Name = "TextBoxAppointmentReasonForVisit";
            TextBoxAppointmentReasonForVisit.Size = new Size(361, 71);
            TextBoxAppointmentReasonForVisit.TabIndex = 84;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label7.Location = new Point(7, 270);
            label7.Name = "label7";
            label7.Size = new Size(95, 13);
            label7.TabIndex = 86;
            label7.Text = "Doctor/Consultant";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label6.Location = new Point(7, 390);
            label6.Name = "label6";
            label6.Size = new Size(97, 13);
            label6.TabIndex = 85;
            label6.Text = "Reason For Visit";
            // 
            // TextBoxAptPatientAddress
            // 
            TextBoxAptPatientAddress.BackColor = SystemColors.Window;
            TextBoxAptPatientAddress.Location = new Point(8, 185);
            TextBoxAptPatientAddress.Multiline = true;
            TextBoxAptPatientAddress.Name = "TextBoxAptPatientAddress";
            TextBoxAptPatientAddress.ReadOnly = true;
            TextBoxAptPatientAddress.Size = new Size(361, 57);
            TextBoxAptPatientAddress.TabIndex = 69;
            TextBoxAptPatientAddress.TabStop = false;
            // 
            // label5
            // 
            label5.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(7, 164);
            label5.Name = "label5";
            label5.Size = new Size(78, 13);
            label5.TabIndex = 81;
            label5.Text = "Address";
            // 
            // BtnNewPatient
            // 
            BtnNewPatient.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnNewPatient.Location = new Point(306, 63);
            BtnNewPatient.Name = "BtnNewPatient";
            BtnNewPatient.Size = new Size(129, 23);
            BtnNewPatient.TabIndex = 78;
            BtnNewPatient.Text = "New Patient [F3]";
            BtnNewPatient.UseVisualStyleBackColor = true;
            BtnNewPatient.Click += BtnNewPatient_Click;
            // 
            // BtnPatientSearch
            // 
            BtnPatientSearch.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPatientSearch.Location = new Point(306, 34);
            BtnPatientSearch.Name = "BtnPatientSearch";
            BtnPatientSearch.Size = new Size(129, 23);
            BtnPatientSearch.TabIndex = 77;
            BtnPatientSearch.Text = "Patient Search [F2]";
            BtnPatientSearch.UseVisualStyleBackColor = true;
            BtnPatientSearch.Click += BtnPatientSearch_Click;
            // 
            // PatientGender
            // 
            PatientGender.BackColor = SystemColors.Control;
            PatientGender.Enabled = false;
            PatientGender.Gender = fa.views.controls.GenderSelection.None;
            PatientGender.Location = new Point(6, 82);
            PatientGender.Margin = new Padding(4, 3, 4, 3);
            PatientGender.Name = "PatientGender";
            PatientGender.Size = new Size(178, 22);
            PatientGender.TabIndex = 70;
            PatientGender.TabStop = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(7, 62);
            label4.Name = "label4";
            label4.Size = new Size(48, 13);
            label4.TabIndex = 79;
            label4.Text = "Gender";
            // 
            // TextBoxAppointmentAge
            // 
            TextBoxAppointmentAge.BackColor = SystemColors.Window;
            TextBoxAppointmentAge.Location = new Point(128, 133);
            TextBoxAppointmentAge.Name = "TextBoxAppointmentAge";
            TextBoxAppointmentAge.ReadOnly = true;
            TextBoxAppointmentAge.Size = new Size(26, 21);
            TextBoxAppointmentAge.TabIndex = 71;
            TextBoxAppointmentAge.TabStop = false;
            // 
            // TextBoxAppointmentDOB
            // 
            TextBoxAppointmentDOB.BackColor = SystemColors.Window;
            TextBoxAppointmentDOB.Location = new Point(8, 133);
            TextBoxAppointmentDOB.Name = "TextBoxAppointmentDOB";
            TextBoxAppointmentDOB.ReadOnly = true;
            TextBoxAppointmentDOB.Size = new Size(100, 21);
            TextBoxAppointmentDOB.TabIndex = 72;
            TextBoxAppointmentDOB.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(125, 111);
            label3.Name = "label3";
            label3.Size = new Size(29, 13);
            label3.TabIndex = 73;
            label3.Text = "Age";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(7, 111);
            label2.Name = "label2";
            label2.Size = new Size(30, 13);
            label2.TabIndex = 74;
            label2.Text = "DOB";
            // 
            // TextBoxAppointmentName
            // 
            TextBoxAppointmentName.BackColor = SystemColors.Window;
            TextBoxAppointmentName.Location = new Point(8, 34);
            TextBoxAppointmentName.MaxLength = 63;
            TextBoxAppointmentName.Name = "TextBoxAppointmentName";
            TextBoxAppointmentName.ReadOnly = true;
            TextBoxAppointmentName.Size = new Size(292, 21);
            TextBoxAppointmentName.TabIndex = 76;
            TextBoxAppointmentName.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(7, 14);
            label1.Name = "label1";
            label1.Size = new Size(39, 13);
            label1.TabIndex = 75;
            label1.Text = "Name";
            // 
            // FormAppointment
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(654, 603);
            Controls.Add(groupBoxAppointment);
            Controls.Add(statusStripAppointment);
            Controls.Add(toolStripAppointment);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormAppointment";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Appointment";
            Load += FormAppointment_Load;
            Controls.SetChildIndex(toolStripAppointment, 0);
            Controls.SetChildIndex(statusStripAppointment, 0);
            Controls.SetChildIndex(groupBoxAppointment, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(checkBoxIsPatient, 0);
            toolStripAppointment.ResumeLayout(false);
            toolStripAppointment.PerformLayout();
            statusStripAppointment.ResumeLayout(false);
            statusStripAppointment.PerformLayout();
            groupBoxAppointment.ResumeLayout(false);
            groupBoxAppointment.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ToolStrip toolStripAppointment;
        private StatusStrip statusStripAppointment;
        private ToolStripStatusLabel OpRegistrationErrorMsg;
        private ToolStripStatusLabel AppointmentErrMsg;
        private GroupBox groupBoxAppointment;
        private TextBox TextBoxAptPatientAddress;
        private Label label5;
        private Button BtnNewPatient;
        private Button BtnPatientSearch;
        private fa.views.controls.GenderRadio PatientGender;
        private Label label4;
        private TextBox TextBoxAppointmentAge;
        private TextBox TextBoxAppointmentDOB;
        private Label label3;
        private Label label2;
        private TextBox TextBoxAppointmentName;
        private Label label1;
        private TextBox TextBoxAppointmentReasonForVisit;
        private Label label6;
        private Label label7;
        private fa.views.controls.PatientPhoto PatientPhoto;
        private fa.views.controls.PatientNumberControl PatientNumberOp;
        private Button BtnSave;
        private Button BtnCancel;
        private Label label8;
        private TextBox TextBoxPatientId;
        private TextBox TextBoxAppointmentDepartment;
        private TextBox textBoxAppointmentID;
        private Button buttonDelete;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripLabel toolStripLabel3;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripLabel labelDuration;
        private ToolStripComboBox ComboBoxStartingTime;
        private ToolStripComboBox ComboBoxEndTime;
        private fa.views.controls.ComboBoxSwapTextBox ComboBoxConsultant;
        private ToolStripLabel LabelFrom;
        private fa.views.controls.ToolStripCalendar FromDate;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripLabel LabelTo;
        private fa.views.controls.ToolStripCalendar ToDate;
        private Label labelAppointmentErrMsg;
    }
}