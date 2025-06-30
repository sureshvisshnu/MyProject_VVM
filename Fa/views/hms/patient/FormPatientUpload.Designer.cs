namespace Fa.views.hms.patient
{
    partial class FormPatientUpload
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
            patientFileUpload = new controls.hms.PatientFileUpload();
            SuspendLayout();
            // 
            // patientFileUpload
            // 
            patientFileUpload.Location = new Point(4, 4);
            patientFileUpload.Margin = new Padding(4, 3, 4, 3);
            patientFileUpload.Name = "patientFileUpload";
            patientFileUpload.Processor = null;
            patientFileUpload.selectedSoftware = "None";
            patientFileUpload.Size = new Size(482, 595);
            patientFileUpload.TabIndex = 0;
            patientFileUpload.OnClickUpload += PatientFileUpload_OnClickUpload;
            patientFileUpload.OnClickExit += FileUploadMedicalTest_OnClickExit;
            patientFileUpload.OnClickDownload += patientFileUpload_OnClickDownload;
            // 
            // FormPatientUpload
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(489, 603);
            Controls.Add(patientFileUpload);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormPatientUpload";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Patient Upload";
            FormClosing += MedicalTestFileUpload_FormClosing;
            ResumeLayout(false);
        }

        #endregion

        private controls.hms.PatientFileUpload patientFileUpload;
    }
}