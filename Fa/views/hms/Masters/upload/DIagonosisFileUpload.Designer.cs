namespace fa.views.hms.masters.upload
{
    partial class DiagonosisFileUpload
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiagonosisFileUpload));
            FileUploadDiagonosis = new controls.fileupload.FileUpload();
            SuspendLayout();
            // 
            // FileUploadDiagonosis
            // 
            FileUploadDiagonosis.Location = new Point(12, 12);
            FileUploadDiagonosis.Margin = new Padding(4, 3, 4, 3);
            FileUploadDiagonosis.Name = "FileUploadDiagonosis";
            FileUploadDiagonosis.Processor = null;
            FileUploadDiagonosis.Size = new Size(482, 554);
            FileUploadDiagonosis.TabIndex = 0;
            FileUploadDiagonosis.OnClickUpload += FileUploadDiagonosis_OnClickUpload;
            FileUploadDiagonosis.OnClickExit += FileUploadDiagonosis_OnClickExit;
            // 
            // DiagonosisFileUpload
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(505, 574);
            Controls.Add(FileUploadDiagonosis);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DiagonosisFileUpload";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Diagonosis File Upload";
            FormClosing += DiagonosisFileUpload_FormClosing;
            ResumeLayout(false);
        }

        #endregion

        private controls.fileupload.FileUpload FileUploadDiagonosis;
    }
}