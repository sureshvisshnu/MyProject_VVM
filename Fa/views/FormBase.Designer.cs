namespace fa.views
{
    partial class FormBase
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
            ProductIdTransport = new TextBox();
            ProductBatchIdTransport = new TextBox();
            AccountIdTransport = new TextBox();
            checkBoxIsPatient = new CheckBox();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(201, 248);
            ProductIdTransport.Margin = new Padding(4, 3, 4, 3);
            ProductIdTransport.Name = "ProductIdTransport";
            ProductIdTransport.Size = new Size(116, 23);
            ProductIdTransport.TabIndex = 0;
            ProductIdTransport.Visible = false;
            ProductIdTransport.TextChanged += ProductIdTransportReload;
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(201, 218);
            ProductBatchIdTransport.Margin = new Padding(4, 3, 4, 3);
            ProductBatchIdTransport.Name = "ProductBatchIdTransport";
            ProductBatchIdTransport.Size = new Size(116, 23);
            ProductBatchIdTransport.TabIndex = 1;
            ProductBatchIdTransport.Visible = false;
            ProductBatchIdTransport.TextChanged += ProductBatchIdTransportReload;
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(201, 188);
            AccountIdTransport.Margin = new Padding(4, 3, 4, 3);
            AccountIdTransport.Name = "AccountIdTransport";
            AccountIdTransport.Size = new Size(116, 23);
            AccountIdTransport.TabIndex = 3;
            AccountIdTransport.Visible = false;
            AccountIdTransport.TextChanged += AccountIdTransportReload;
            // 
            // checkBoxIsPatient
            // 
            checkBoxIsPatient.AutoSize = true;
            checkBoxIsPatient.Location = new Point(201, 163);
            checkBoxIsPatient.Name = "checkBoxIsPatient";
            checkBoxIsPatient.Size = new Size(71, 19);
            checkBoxIsPatient.TabIndex = 4;
            checkBoxIsPatient.Text = "Ispatient";
            checkBoxIsPatient.UseVisualStyleBackColor = true;
            checkBoxIsPatient.Visible = false;
            // 
            // FormBase
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(331, 301);
            Controls.Add(checkBoxIsPatient);
            Controls.Add(AccountIdTransport);
            Controls.Add(ProductBatchIdTransport);
            Controls.Add(ProductIdTransport);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4, 3, 4, 3);
            Name = "FormBase";
            Text = "FormBase";
            Load += FormBase_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public System.Windows.Forms.TextBox ProductIdTransport;
        public System.Windows.Forms.TextBox ProductBatchIdTransport;
        public System.Windows.Forms.TextBox AccountIdTransport;
        public CheckBox checkBoxIsPatient;
    }
}