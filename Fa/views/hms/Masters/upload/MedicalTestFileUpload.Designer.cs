namespace fa.views.hms.masters.upload
{
    partial class MedicalTestFileUpload
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MedicalTestFileUpload));
            FileUploadMedicalTest = new controls.fileupload.FileUpload();
            SuspendLayout();
            // 
            // FileUploadMedicalTest
            // 
            FileUploadMedicalTest.Location = new Point(9, 13);
            FileUploadMedicalTest.Margin = new Padding(4, 5, 4, 5);
            FileUploadMedicalTest.Name = "FileUploadMedicalTest";
            FileUploadMedicalTest.Processor = null;
            FileUploadMedicalTest.Size = new Size(482, 554);
            FileUploadMedicalTest.TabIndex = 1;
            FileUploadMedicalTest.OnClickUpload += FileUploadMedicalTest_OnClickUpload;
            FileUploadMedicalTest.OnClickExit += FileUploadMedicalTest_OnClickExit;
            FileUploadMedicalTest.ShowDialog += FileUploadMedicalTest_ShowDialog;
            // 
            // MedicalTestFileUpload
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(502, 578);
            Controls.Add(FileUploadMedicalTest);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MedicalTestFileUpload";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Medical Test File Upload";
            FormClosing += MedicalTestFileUpload_FormClosing;
            Load += MedicalTestFileUpload_Load;
            ResumeLayout(false);
        }

        #endregion

        private controls.fileupload.FileUpload FileUploadMedicalTest;
    }
}