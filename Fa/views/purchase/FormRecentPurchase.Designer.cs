namespace fa.views.purchase
{
    partial class FormRecentPurchase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecentPurchase));
            GridViewPurchaseRecentInvoice = new controls.DataViewVerticalScroll();
            Date = new DataGridViewTextBoxColumn();
            Supplier = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Amount = new controls.grid.DataGridViewCurrencyColumn();
            Column10 = new DataGridViewTextBoxColumn();
            statusStrip1 = new StatusStrip();
            RecentPurchaseErrorMsg = new ToolStripStatusLabel();
            BtnCancel = new Button();
            BtnSelect = new Button();
            ((System.ComponentModel.ISupportInitialize)GridViewPurchaseRecentInvoice).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // GridViewPurchaseRecentInvoice
            // 
            GridViewPurchaseRecentInvoice.AllowUserToAddRows = false;
            GridViewPurchaseRecentInvoice.AllowUserToDeleteRows = false;
            GridViewPurchaseRecentInvoice.AllowUserToResizeColumns = false;
            GridViewPurchaseRecentInvoice.AllowUserToResizeRows = false;
            GridViewPurchaseRecentInvoice.BackgroundColor = SystemColors.Control;
            GridViewPurchaseRecentInvoice.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewPurchaseRecentInvoice.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewPurchaseRecentInvoice.ColumnHeadersHeight = 20;
            GridViewPurchaseRecentInvoice.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewPurchaseRecentInvoice.Columns.AddRange(new DataGridViewColumn[] { Date, Supplier, Column1, Amount, Column10 });
            GridViewPurchaseRecentInvoice.EnableHeadersVisualStyles = false;
            GridViewPurchaseRecentInvoice.Location = new Point(7, 5);
            GridViewPurchaseRecentInvoice.MultiSelect = false;
            GridViewPurchaseRecentInvoice.Name = "GridViewPurchaseRecentInvoice";
            GridViewPurchaseRecentInvoice.ReadOnly = true;
            GridViewPurchaseRecentInvoice.RowHeadersVisible = false;
            GridViewPurchaseRecentInvoice.RowTemplate.Height = 20;
            GridViewPurchaseRecentInvoice.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewPurchaseRecentInvoice.ShowCellToolTips = false;
            GridViewPurchaseRecentInvoice.Size = new Size(548, 219);
            GridViewPurchaseRecentInvoice.TabIndex = 0;
            GridViewPurchaseRecentInvoice.TabStop = false;
            GridViewPurchaseRecentInvoice.CellDoubleClick += GridViewPurchaseRecentInvoice_CellDoubleClick;
            GridViewPurchaseRecentInvoice.KeyDown += GridViewPurchaseRecentInvoice_KeyDown;
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
            // Supplier
            // 
            Supplier.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Supplier.HeaderText = "Supplier";
            Supplier.Name = "Supplier";
            Supplier.ReadOnly = true;
            Supplier.Resizable = DataGridViewTriState.False;
            Supplier.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column1
            // 
            Column1.HeaderText = "Inv No";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
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
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { RecentPurchaseErrorMsg });
            statusStrip1.Location = new Point(0, 261);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(561, 22);
            statusStrip1.TabIndex = 149;
            statusStrip1.Text = "statusStrip1";
            // 
            // RecentPurchaseErrorMsg
            // 
            RecentPurchaseErrorMsg.Name = "RecentPurchaseErrorMsg";
            RecentPurchaseErrorMsg.Size = new Size(49, 17);
            RecentPurchaseErrorMsg.Text = "              ";
            // 
            // BtnCancel
            // 
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(361, 231);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(88, 23);
            BtnCancel.TabIndex = 2;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // BtnSelect
            // 
            BtnSelect.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSelect.Location = new Point(454, 231);
            BtnSelect.Name = "BtnSelect";
            BtnSelect.Size = new Size(88, 23);
            BtnSelect.TabIndex = 1;
            BtnSelect.Text = "Select [F8]";
            BtnSelect.UseVisualStyleBackColor = true;
            BtnSelect.Click += BtnSelect_Click;
            BtnSelect.PreviewKeyDown += BtnSelect_PreviewKeyDown;
            // 
            // FormRecentPurchase
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnCancel;
            ClientSize = new Size(561, 283);
            Controls.Add(BtnCancel);
            Controls.Add(BtnSelect);
            Controls.Add(statusStrip1);
            Controls.Add(GridViewPurchaseRecentInvoice);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormRecentPurchase";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Recent Purchase";
            Load += FormRecentPurchase_Load;
            ((System.ComponentModel.ISupportInitialize)GridViewPurchaseRecentInvoice).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private controls.DataViewVerticalScroll GridViewPurchaseRecentInvoice;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel RecentPurchaseErrorMsg;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnSelect;
        private DataGridViewTextBoxColumn Date;
        private DataGridViewTextBoxColumn Supplier;
        private DataGridViewTextBoxColumn Column1;
        private controls.grid.DataGridViewCurrencyColumn Amount;
        private DataGridViewTextBoxColumn Column10;
    }
}