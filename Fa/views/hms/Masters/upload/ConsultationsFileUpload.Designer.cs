namespace fa.views.hms.masters.upload
{
    partial class ConsultationsFileUpload
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsultationsFileUpload));
            this.FileUploadConsultation = new fa.views.controls.fileupload.FileUpload();
            this.SuspendLayout();
            // 
            // FileUploadConsultation
            // 
            this.FileUploadConsultation.Location = new System.Drawing.Point(12, 12);
            this.FileUploadConsultation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.FileUploadConsultation.Name = "FileUploadConsultation";
            this.FileUploadConsultation.Processor = null;
            this.FileUploadConsultation.Size = new System.Drawing.Size(481, 556);
            this.FileUploadConsultation.TabIndex = 0;
            this.FileUploadConsultation.OnClickUpload += new System.EventHandler(this.FileUploadConsultation_OnClickUpload);
            this.FileUploadConsultation.OnClickExit += new System.EventHandler(this.FileUploadConsultation_OnClickExit);
            // 
            // ConsultationsFileUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 574);
            this.Controls.Add(this.FileUploadConsultation);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConsultationsFileUpload";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Consultations File Upload";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConsultationsFileUpload_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private controls.fileupload.FileUpload FileUploadConsultation;
    }
}