namespace fa.views.hms
{
    partial class FormPatientBase
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
            this.PatientIdTransport = new System.Windows.Forms.TextBox();
            this.EmergencyContId = new System.Windows.Forms.TextBox();
            this.InsuranceId = new System.Windows.Forms.TextBox();
            this.GuardianId = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // PatientIdTransport
            // 
            this.PatientIdTransport.Location = new System.Drawing.Point(12, 812);
            this.PatientIdTransport.Name = "PatientIdTransport";
            this.PatientIdTransport.Size = new System.Drawing.Size(100, 20);
            this.PatientIdTransport.TabIndex = 0;
            this.PatientIdTransport.Visible = false;
            this.PatientIdTransport.TextChanged += new System.EventHandler(this.PatientIdTransportReload);
            // 
            // EmergencyContId
            // 
            this.EmergencyContId.Location = new System.Drawing.Point(12, 760);
            this.EmergencyContId.Name = "EmergencyContId";
            this.EmergencyContId.Size = new System.Drawing.Size(100, 20);
            this.EmergencyContId.TabIndex = 64;
            this.EmergencyContId.TabStop = false;
            this.EmergencyContId.Visible = false;
            // 
            // InsuranceId
            // 
            this.InsuranceId.Location = new System.Drawing.Point(12, 786);
            this.InsuranceId.Name = "InsuranceId";
            this.InsuranceId.Size = new System.Drawing.Size(100, 20);
            this.InsuranceId.TabIndex = 65;
            this.InsuranceId.TabStop = false;
            this.InsuranceId.Visible = false;
            // 
            // GuardianId
            // 
            this.GuardianId.Location = new System.Drawing.Point(12, 734);
            this.GuardianId.Name = "GuardianId";
            this.GuardianId.Size = new System.Drawing.Size(100, 20);
            this.GuardianId.TabIndex = 66;
            this.GuardianId.TabStop = false;
            this.GuardianId.Visible = false;
            // 
            // FormPatientBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 749);
            this.Controls.Add(this.GuardianId);
            this.Controls.Add(this.InsuranceId);
            this.Controls.Add(this.EmergencyContId);
            this.Controls.Add(this.PatientIdTransport);
            this.Name = "FormPatientBase";
            this.Text = "FormPatientBase";
            this.Load += new System.EventHandler(this.FormPatientBase_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox PatientIdTransport;
        private System.Windows.Forms.TextBox EmergencyContId;
        private System.Windows.Forms.TextBox InsuranceId;
        private System.Windows.Forms.TextBox GuardianId;
    }
}