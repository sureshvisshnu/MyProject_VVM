namespace fa.views.account.transactions
{
    partial class FormRecentInvoice
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecentInvoice));
            label1 = new Label();
            DataGridViewRecentInvoice = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)DataGridViewRecentInvoice).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(9, 14);
            label1.Name = "label1";
            label1.Size = new Size(84, 13);
            label1.TabIndex = 3;
            label1.Text = "Recent Invoices";
            // 
            // DataGridViewRecentInvoice
            // 
            DataGridViewRecentInvoice.AllowUserToAddRows = false;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DataGridViewRecentInvoice.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            DataGridViewRecentInvoice.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridViewRecentInvoice.Location = new Point(12, 39);
            DataGridViewRecentInvoice.Name = "DataGridViewRecentInvoice";
            DataGridViewRecentInvoice.RowHeadersVisible = false;
            DataGridViewRecentInvoice.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataGridViewRecentInvoice.ShowCellToolTips = false;
            DataGridViewRecentInvoice.Size = new Size(765, 418);
            DataGridViewRecentInvoice.TabIndex = 2;
            DataGridViewRecentInvoice.CellContentClick += DataGridViewRecentInvoice_CellContentClick;
            // 
            // FormRecentInvoice
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(789, 468);
            Controls.Add(label1);
            Controls.Add(DataGridViewRecentInvoice);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormRecentInvoice";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RecentInvoice";
            Load += FormRecentInvoice_Load;
            ((System.ComponentModel.ISupportInitialize)DataGridViewRecentInvoice).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView DataGridViewRecentInvoice;
    }
}