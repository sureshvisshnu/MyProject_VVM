namespace fa.views.sales
{
    partial class FormRecentSalePayment
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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecentSalePayment));
            BtnCancel = new Button();
            BtnSelect = new Button();
            statusStrip1 = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            GridViewRecentSalesPayment = new controls.DataViewVerticalScroll();
            Date = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Customer = new DataGridViewTextBoxColumn();
            Amount = new controls.grid.DataGridViewCurrencyColumn();
            Column10 = new DataGridViewTextBoxColumn();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewRecentSalesPayment).BeginInit();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(12, 234);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(37, 225);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(172, 163);
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // BtnCancel
            // 
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(295, 225);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(88, 23);
            BtnCancel.TabIndex = 156;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // BtnSelect
            // 
            BtnSelect.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSelect.Location = new Point(389, 225);
            BtnSelect.Name = "BtnSelect";
            BtnSelect.Size = new Size(88, 23);
            BtnSelect.TabIndex = 155;
            BtnSelect.Text = "Select [F8]";
            BtnSelect.UseVisualStyleBackColor = true;
            BtnSelect.Click += BtnSelect_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsg });
            statusStrip1.Location = new Point(0, 256);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(489, 22);
            statusStrip1.TabIndex = 157;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(49, 17);
            ErrorMsg.Text = "              ";
            // 
            // GridViewRecentSalesPayment
            // 
            GridViewRecentSalesPayment.AllowUserToAddRows = false;
            GridViewRecentSalesPayment.AllowUserToDeleteRows = false;
            GridViewRecentSalesPayment.AllowUserToResizeColumns = false;
            GridViewRecentSalesPayment.AllowUserToResizeRows = false;
            GridViewRecentSalesPayment.BackgroundColor = SystemColors.Control;
            GridViewRecentSalesPayment.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewRecentSalesPayment.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewRecentSalesPayment.ColumnHeadersHeight = 20;
            GridViewRecentSalesPayment.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewRecentSalesPayment.Columns.AddRange(new DataGridViewColumn[] { Date, Column1, Customer, Amount, Column10 });
            GridViewRecentSalesPayment.EnableHeadersVisualStyles = false;
            GridViewRecentSalesPayment.Location = new Point(0, 0);
            GridViewRecentSalesPayment.MultiSelect = false;
            GridViewRecentSalesPayment.Name = "GridViewRecentSalesPayment";
            GridViewRecentSalesPayment.ReadOnly = true;
            GridViewRecentSalesPayment.RowHeadersVisible = false;
            GridViewRecentSalesPayment.RowTemplate.Height = 20;
            GridViewRecentSalesPayment.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewRecentSalesPayment.ShowCellToolTips = false;
            GridViewRecentSalesPayment.Size = new Size(489, 219);
            GridViewRecentSalesPayment.TabIndex = 154;
            GridViewRecentSalesPayment.TabStop = false;
            GridViewRecentSalesPayment.CellDoubleClick += GridViewRecentSalesPayment_CellDoubleClick;
            GridViewRecentSalesPayment.KeyDown += GridViewRecentSalesPayment_KeyDown;
            // 
            // Date
            // 
            Date.HeaderText = "Date";
            Date.Name = "Date";
            Date.ReadOnly = true;
            Date.Resizable = DataGridViewTriState.False;
            Date.SortMode = DataGridViewColumnSortMode.NotSortable;
            Date.Width = 75;
            // 
            // Column1
            // 
            Column1.HeaderText = "Invoice";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Customer
            // 
            Customer.HeaderText = "Customer";
            Customer.Name = "Customer";
            Customer.ReadOnly = true;
            Customer.Resizable = DataGridViewTriState.False;
            Customer.SortMode = DataGridViewColumnSortMode.NotSortable;
            Customer.Width = 190;
            // 
            // Amount
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            Amount.DefaultCellStyle = dataGridViewCellStyle2;
            Amount.HeaderText = "Amount";
            Amount.Name = "Amount";
            Amount.ReadOnly = true;
            Amount.Resizable = DataGridViewTriState.False;
            // 
            // Column10
            // 
            Column10.HeaderText = "Id";
            Column10.Name = "Column10";
            Column10.ReadOnly = true;
            Column10.Resizable = DataGridViewTriState.False;
            Column10.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column10.Visible = false;
            // 
            // FormRecentSalePayment
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(489, 278);
            Controls.Add(BtnCancel);
            Controls.Add(BtnSelect);
            Controls.Add(statusStrip1);
            Controls.Add(GridViewRecentSalesPayment);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormRecentSalePayment";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Recent Payment";
            Load += FormRecentSalePayment_Load;
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(GridViewRecentSalesPayment, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(BtnSelect, 0);
            Controls.SetChildIndex(BtnCancel, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewRecentSalesPayment).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnSelect;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ErrorMsg;
        private controls.DataViewVerticalScroll GridViewRecentSalesPayment;
        private DataGridViewTextBoxColumn Date;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Customer;
        private controls.grid.DataGridViewCurrencyColumn Amount;
        private DataGridViewTextBoxColumn Column10;
    }
}