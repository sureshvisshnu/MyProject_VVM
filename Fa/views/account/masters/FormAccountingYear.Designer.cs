
namespace fa.views.account.masters
{
    partial class FormFiscalYear
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFiscalYear));
            label1 = new Label();
            statusStrip1 = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            BtnFiscalYearSave = new Button();
            ComboBoxFiscalYear = new controls.ComboBoxSwapTextBox();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(33, 13);
            label1.TabIndex = 0;
            label1.Text = "Year";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsg });
            statusStrip1.Location = new Point(0, 58);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(256, 22);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(37, 17);
            ErrorMsg.Text = "          ";
            // 
            // BtnFiscalYearSave
            // 
            BtnFiscalYearSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnFiscalYearSave.Location = new Point(156, 23);
            BtnFiscalYearSave.Name = "BtnFiscalYearSave";
            BtnFiscalYearSave.Size = new Size(83, 23);
            BtnFiscalYearSave.TabIndex = 133;
            BtnFiscalYearSave.Text = "Save [F8]";
            BtnFiscalYearSave.UseVisualStyleBackColor = true;
            BtnFiscalYearSave.Click += BtnFiscalYearSave_Click;
            // 
            // ComboBoxFiscalYear
            // 
            ComboBoxFiscalYear.FormattingEnabled = true;
            ComboBoxFiscalYear.Location = new Point(15, 25);
            ComboBoxFiscalYear.Name = "ComboBoxFiscalYear";
            ComboBoxFiscalYear.Size = new Size(134, 21);
            ComboBoxFiscalYear.TabIndex = 134;
            ComboBoxFiscalYear.TxtVisible = true;
            // 
            // FormFiscalYear
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(256, 80);
            Controls.Add(ComboBoxFiscalYear);
            Controls.Add(BtnFiscalYearSave);
            Controls.Add(statusStrip1);
            Controls.Add(label1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormFiscalYear";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Create Reference";
            Load += FormFiscalYear_Load;
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(BtnFiscalYearSave, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(ComboBoxFiscalYear, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ErrorMsg;
        private System.Windows.Forms.Button BtnFiscalYearSave;
        private controls.ComboBoxSwapTextBox ComboBoxFiscalYear;
    }
}