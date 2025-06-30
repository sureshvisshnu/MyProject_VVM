namespace Fa.views.catalog
{
    partial class TaxCodeFileUpload
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaxCodeFileUpload));
            fileUpload1 = new fa.views.controls.fileupload.FileUpload();
            SuspendLayout();
            // 
            // fileUpload1
            // 
            fileUpload1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            fileUpload1.Location = new Point(12, 12);
            fileUpload1.Name = "fileUpload1";
            fileUpload1.Processor = null;
            fileUpload1.Size = new Size(484, 554);
            fileUpload1.TabIndex = 0;
            fileUpload1.OnClickUpload += fileUpload1_OnClickUpload;
            fileUpload1.OnClickExit += fileUpload1_OnClickExit;
            // 
            // TaxCodeFileUpload
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(507, 577);
            Controls.Add(fileUpload1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TaxCodeFileUpload";
            StartPosition = FormStartPosition.CenterParent;
            Text = "TaxCodeFileUpload";
            FormClosing += TaxCodeFileUpload_FormClosing;
            ResumeLayout(false);
        }

        #endregion

        private fa.views.controls.fileupload.FileUpload fileUpload1;
    }
}