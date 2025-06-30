namespace fa.views.hms.op
{
    partial class FormOPRegistrationPrint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOPRegistrationPrint));
            CheckBoxToken = new CheckBox();
            CheckBoxFeeReceipt = new CheckBox();
            BtnPrint = new Button();
            BtnCancel = new Button();
            PrintDocumentDirect = new System.Drawing.Printing.PrintDocument();
            PrintPreviewDialogDirect = new PrintPreviewDialog();
            SuspendLayout();
            // 
            // CheckBoxToken
            // 
            CheckBoxToken.AutoSize = true;
            CheckBoxToken.Checked = true;
            CheckBoxToken.CheckState = CheckState.Checked;
            CheckBoxToken.Location = new Point(12, 12);
            CheckBoxToken.Name = "CheckBoxToken";
            CheckBoxToken.Size = new Size(55, 17);
            CheckBoxToken.TabIndex = 0;
            CheckBoxToken.Text = "Token";
            CheckBoxToken.UseVisualStyleBackColor = true;
            CheckBoxToken.CheckedChanged += CheckBoxToken_CheckedChanged;
            // 
            // CheckBoxFeeReceipt
            // 
            CheckBoxFeeReceipt.AutoSize = true;
            CheckBoxFeeReceipt.Checked = true;
            CheckBoxFeeReceipt.CheckState = CheckState.Checked;
            CheckBoxFeeReceipt.Location = new Point(12, 35);
            CheckBoxFeeReceipt.Name = "CheckBoxFeeReceipt";
            CheckBoxFeeReceipt.Size = new Size(83, 17);
            CheckBoxFeeReceipt.TabIndex = 1;
            CheckBoxFeeReceipt.Text = "Fee Receipt";
            CheckBoxFeeReceipt.UseVisualStyleBackColor = true;
            CheckBoxFeeReceipt.CheckedChanged += CheckBoxFeeReceipt_CheckedChanged;
            // 
            // BtnPrint
            // 
            BtnPrint.Location = new Point(93, 72);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 23);
            BtnPrint.TabIndex = 2;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Location = new Point(12, 72);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 23);
            BtnCancel.TabIndex = 3;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // PrintDocumentDirect
            // 
            PrintDocumentDirect.PrintPage += PrintDocumentDirect_PrintPage;
            PrintDocumentDirect.QueryPageSettings += PrintDocumentDirect_QueryPageSettings;
            // 
            // PrintPreviewDialogDirect
            // 
            PrintPreviewDialogDirect.AutoScrollMargin = new Size(0, 0);
            PrintPreviewDialogDirect.AutoScrollMinSize = new Size(0, 0);
            PrintPreviewDialogDirect.ClientSize = new Size(400, 300);
            PrintPreviewDialogDirect.Document = PrintDocumentDirect;
            PrintPreviewDialogDirect.Enabled = true;
            PrintPreviewDialogDirect.Icon = (Icon)resources.GetObject("PrintPreviewDialogDirect.Icon");
            PrintPreviewDialogDirect.Name = "PrintPreviewDialogDirect";
            PrintPreviewDialogDirect.Visible = false;
            // 
            // FormOPRegistrationPrint
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(182, 109);
            Controls.Add(BtnCancel);
            Controls.Add(BtnPrint);
            Controls.Add(CheckBoxFeeReceipt);
            Controls.Add(CheckBoxToken);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormOPRegistrationPrint";
            StartPosition = FormStartPosition.CenterParent;
            Text = "OP Registration Print";
            Load += FormOPRegistrationPrint_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.CheckBox CheckBoxToken;
        private System.Windows.Forms.CheckBox CheckBoxFeeReceipt;
        private System.Windows.Forms.Button BtnPrint;
        private System.Windows.Forms.Button BtnCancel;
        private System.Drawing.Printing.PrintDocument PrintDocumentDirect;
        private System.Windows.Forms.PrintPreviewDialog PrintPreviewDialogDirect;
    }
}