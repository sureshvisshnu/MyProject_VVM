namespace fa.views.hms.masters.upload
{
    partial class ProcedureFileUpload
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcedureFileUpload));
            timer1 = new System.Windows.Forms.Timer(components);
            fileUpload1 = new controls.fileupload.FileUpload();
            SuspendLayout();
            // 
            // fileUpload1
            // 
            fileUpload1.Location = new Point(12, 12);
            fileUpload1.Margin = new Padding(4, 5, 4, 5);
            fileUpload1.Name = "fileUpload1";
            fileUpload1.Processor = null;
            fileUpload1.Size = new Size(482, 554);
            fileUpload1.TabIndex = 0;
            fileUpload1.OnClickUpload += fileUpload1_OnClickUpload_1;
            fileUpload1.OnClickExit += fileUpload1_OnClickExit;
            // 
            // ProcedureFileUpload
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(507, 577);
            Controls.Add(fileUpload1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ProcedureFileUpload";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ProcedureFileUpload";
            FormClosing += ProcedureFileUpload_FormClosing;
            ResumeLayout(false);
        }

        #endregion

        private controls.fileupload.FileUpload fileUpload1;
        private System.Windows.Forms.Timer timer1;
    }
}