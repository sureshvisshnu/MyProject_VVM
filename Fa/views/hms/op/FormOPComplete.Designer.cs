namespace fa.views.hms.op
{
    partial class FormOPComplete
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOPComplete));
            this.CheckBoxDoctorConsultation = new System.Windows.Forms.CheckBox();
            this.CheckBoxFeesPaid = new System.Windows.Forms.CheckBox();
            this.CheckBoxNurseStationActivities = new System.Windows.Forms.CheckBox();
            this.PatientInfoMiniHorizontal = new fa.views.controls.hms.PatientInfoMin();
            this.CheckBoxTechnicianStationActivities = new System.Windows.Forms.CheckBox();
            this.BtnCompleteVisit = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusVisitComplete = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ComboBoxSelectInsurance = new fa.views.controls.ComboBoxSwapTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CheckBoxBillToInsurance = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TextBoxOpRegistrationId = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PatientIdTransport
            // 
            this.PatientIdTransport.Location = new System.Drawing.Point(12, 812);
            this.PatientIdTransport.Size = new System.Drawing.Size(100, 21);
            // 
            // ProductIdTransport
            // 
            this.ProductIdTransport.Location = new System.Drawing.Point(548, 231);
            this.ProductIdTransport.Size = new System.Drawing.Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            this.ProductBatchIdTransport.Location = new System.Drawing.Point(548, 205);
            this.ProductBatchIdTransport.Size = new System.Drawing.Size(100, 21);
            // 
            // AccountIdTransport
            // 
            this.AccountIdTransport.Location = new System.Drawing.Point(548, 179);
            this.AccountIdTransport.Size = new System.Drawing.Size(100, 21);
            // 
            // CheckBoxDoctorConsultation
            // 
            this.CheckBoxDoctorConsultation.AutoSize = true;
            this.CheckBoxDoctorConsultation.Enabled = false;
            this.CheckBoxDoctorConsultation.Location = new System.Drawing.Point(20, 47);
            this.CheckBoxDoctorConsultation.Name = "CheckBoxDoctorConsultation";
            this.CheckBoxDoctorConsultation.Size = new System.Drawing.Size(128, 17);
            this.CheckBoxDoctorConsultation.TabIndex = 1;
            this.CheckBoxDoctorConsultation.Text = "Doctor\'s Consultation";
            this.CheckBoxDoctorConsultation.UseVisualStyleBackColor = true;
            // 
            // CheckBoxFeesPaid
            // 
            this.CheckBoxFeesPaid.AutoSize = true;
            this.CheckBoxFeesPaid.Location = new System.Drawing.Point(20, 167);
            this.CheckBoxFeesPaid.Name = "CheckBoxFeesPaid";
            this.CheckBoxFeesPaid.Size = new System.Drawing.Size(46, 17);
            this.CheckBoxFeesPaid.TabIndex = 2;
            this.CheckBoxFeesPaid.Text = "Paid";
            this.CheckBoxFeesPaid.UseVisualStyleBackColor = true;
            this.CheckBoxFeesPaid.CheckedChanged += new System.EventHandler(this.CheckBoxFeesPaid_CheckedChanged);
            this.CheckBoxFeesPaid.Click += new System.EventHandler(this.CheckBoxFeesPaid_Click);
            // 
            // CheckBoxNurseStationActivities
            // 
            this.CheckBoxNurseStationActivities.AutoSize = true;
            this.CheckBoxNurseStationActivities.Location = new System.Drawing.Point(20, 70);
            this.CheckBoxNurseStationActivities.Name = "CheckBoxNurseStationActivities";
            this.CheckBoxNurseStationActivities.Size = new System.Drawing.Size(145, 17);
            this.CheckBoxNurseStationActivities.TabIndex = 3;
            this.CheckBoxNurseStationActivities.Text = "Nursing Station Activities";
            this.CheckBoxNurseStationActivities.UseVisualStyleBackColor = true;
            this.CheckBoxNurseStationActivities.CheckedChanged += new System.EventHandler(this.CheckBoxNurseStationActivities_CheckedChanged);
            // 
            // PatientInfoMiniHorizontal
            // 
            this.PatientInfoMiniHorizontal.AutoSize = true;
            this.PatientInfoMiniHorizontal.Location = new System.Drawing.Point(6, 12);
            this.PatientInfoMiniHorizontal.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PatientInfoMiniHorizontal.MaximumSize = new System.Drawing.Size(197, 589);
            this.PatientInfoMiniHorizontal.MinimumSize = new System.Drawing.Size(197, 589);
            this.PatientInfoMiniHorizontal.Name = "PatientInfoMiniHorizontal";
            this.PatientInfoMiniHorizontal.PatientId = null;
            this.PatientInfoMiniHorizontal.Short = false;
            this.PatientInfoMiniHorizontal.Size = new System.Drawing.Size(197, 589);
            this.PatientInfoMiniHorizontal.TabIndex = 68;
            this.PatientInfoMiniHorizontal.TabStop = false;
            // 
            // CheckBoxTechnicianStationActivities
            // 
            this.CheckBoxTechnicianStationActivities.AutoSize = true;
            this.CheckBoxTechnicianStationActivities.Location = new System.Drawing.Point(20, 93);
            this.CheckBoxTechnicianStationActivities.Name = "CheckBoxTechnicianStationActivities";
            this.CheckBoxTechnicianStationActivities.Size = new System.Drawing.Size(122, 17);
            this.CheckBoxTechnicianStationActivities.TabIndex = 69;
            this.CheckBoxTechnicianStationActivities.Text = "Technician Activities";
            this.CheckBoxTechnicianStationActivities.UseVisualStyleBackColor = true;
            this.CheckBoxTechnicianStationActivities.CheckedChanged += new System.EventHandler(this.CheckBoxTechnicianStationActivities_CheckedChanged);
            // 
            // BtnCompleteVisit
            // 
            this.BtnCompleteVisit.Location = new System.Drawing.Point(339, 603);
            this.BtnCompleteVisit.Name = "BtnCompleteVisit";
            this.BtnCompleteVisit.Size = new System.Drawing.Size(110, 24);
            this.BtnCompleteVisit.TabIndex = 71;
            this.BtnCompleteVisit.Text = "Complete Visit [F8]";
            this.BtnCompleteVisit.UseVisualStyleBackColor = true;
            this.BtnCompleteVisit.Click += new System.EventHandler(this.BtnCompleteVisit_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(243, 603);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(84, 24);
            this.BtnCancel.TabIndex = 72;
            this.BtnCancel.Text = "Reset [Esc]";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusVisitComplete});
            this.statusStrip1.Location = new System.Drawing.Point(0, 637);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(469, 22);
            this.statusStrip1.TabIndex = 73;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusVisitComplete
            // 
            this.toolStripStatusVisitComplete.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusVisitComplete.Name = "toolStripStatusVisitComplete";
            this.toolStripStatusVisitComplete.Size = new System.Drawing.Size(0, 17);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.ComboBoxSelectInsurance);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.CheckBoxBillToInsurance);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.CheckBoxDoctorConsultation);
            this.groupBox1.Controls.Add(this.CheckBoxFeesPaid);
            this.groupBox1.Controls.Add(this.CheckBoxNurseStationActivities);
            this.groupBox1.Controls.Add(this.CheckBoxTechnicianStationActivities);
            this.groupBox1.Location = new System.Drawing.Point(219, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(239, 578);
            this.groupBox1.TabIndex = 74;
            this.groupBox1.TabStop = false;
            // 
            // ComboBoxSelectInsurance
            // 
            this.ComboBoxSelectInsurance.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ComboBoxSelectInsurance.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboBoxSelectInsurance.FormattingEnabled = true;
            this.ComboBoxSelectInsurance.Location = new System.Drawing.Point(20, 212);
            this.ComboBoxSelectInsurance.Name = "ComboBoxSelectInsurance";
            this.ComboBoxSelectInsurance.Size = new System.Drawing.Size(210, 21);
            this.ComboBoxSelectInsurance.TabIndex = 77;
            this.ComboBoxSelectInsurance.TxtVisible = true;
            this.ComboBoxSelectInsurance.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(17, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 13);
            this.label2.TabIndex = 77;
            this.label2.Text = "Activities Completed";
            // 
            // CheckBoxBillToInsurance
            // 
            this.CheckBoxBillToInsurance.Enabled = false;
            this.CheckBoxBillToInsurance.Location = new System.Drawing.Point(20, 190);
            this.CheckBoxBillToInsurance.Name = "CheckBoxBillToInsurance";
            this.CheckBoxBillToInsurance.Size = new System.Drawing.Size(164, 16);
            this.CheckBoxBillToInsurance.TabIndex = 75;
            this.CheckBoxBillToInsurance.Text = "Bill to Insurance";
            this.CheckBoxBillToInsurance.UseVisualStyleBackColor = true;
            this.CheckBoxBillToInsurance.CheckedChanged += new System.EventHandler(this.CheckBoxBillToInsurance_CheckedChanged);
            this.CheckBoxBillToInsurance.Click += new System.EventHandler(this.CheckBoxBillToInsurance_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(17, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 76;
            this.label1.Text = "Billing";
            // 
            // TextBoxOpRegistrationId
            // 
            this.TextBoxOpRegistrationId.Location = new System.Drawing.Point(548, 270);
            this.TextBoxOpRegistrationId.Name = "TextBoxOpRegistrationId";
            this.TextBoxOpRegistrationId.Size = new System.Drawing.Size(100, 21);
            this.TextBoxOpRegistrationId.TabIndex = 70;
            this.TextBoxOpRegistrationId.TabStop = false;
            this.TextBoxOpRegistrationId.Visible = false;
            // 
            // FormOPComplete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(469, 659);
            this.Controls.Add(this.TextBoxOpRegistrationId);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnCompleteVisit);
            this.Controls.Add(this.PatientInfoMiniHorizontal);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormOPComplete";
            this.Text = "Complete OP Visit";
            this.Load += new System.EventHandler(this.FormOPComplete_Load);
            this.Controls.SetChildIndex(this.PatientInfoMiniHorizontal, 0);
            this.Controls.SetChildIndex(this.BtnCompleteVisit, 0);
            this.Controls.SetChildIndex(this.BtnCancel, 0);
            this.Controls.SetChildIndex(this.statusStrip1, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.TextBoxOpRegistrationId, 0);
            this.Controls.SetChildIndex(this.PatientIdTransport, 0);
            this.Controls.SetChildIndex(this.ProductIdTransport, 0);
            this.Controls.SetChildIndex(this.ProductBatchIdTransport, 0);
            this.Controls.SetChildIndex(this.AccountIdTransport, 0);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox CheckBoxDoctorConsultation;
        private System.Windows.Forms.CheckBox CheckBoxFeesPaid;
        private System.Windows.Forms.CheckBox CheckBoxNurseStationActivities;
        private controls.hms.PatientInfoMin PatientInfoMiniHorizontal;
        private System.Windows.Forms.CheckBox CheckBoxTechnicianStationActivities;
        private System.Windows.Forms.Button BtnCompleteVisit;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TextBox TextBoxOpRegistrationId;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusVisitComplete;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private controls.ComboBoxSwapTextBox ComboBoxSelectInsurance;
        private System.Windows.Forms.CheckBox CheckBoxBillToInsurance;
    }
}