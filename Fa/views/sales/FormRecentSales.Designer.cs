namespace fa.views.sales
{
    partial class FormRecentSales
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecentSales));
            BtnCancel = new Button();
            BtnSelect = new Button();
            statusStrip1 = new StatusStrip();
            RecentPurchaseErrorMsg = new ToolStripStatusLabel();
            GridViewRecentSales = new controls.DataViewVerticalScroll();
            Date = new DataGridViewTextBoxColumn();
            Customer = new DataGridViewTextBoxColumn();
            InvNo = new DataGridViewTextBoxColumn();
            Amount = new controls.grid.DataGridViewCurrencyColumn();
            Column10 = new DataGridViewTextBoxColumn();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewRecentSales).BeginInit();
            SuspendLayout();
            // 
            // BtnCancel
            // 
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(335, 230);
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
            BtnSelect.Location = new Point(429, 230);
            BtnSelect.Name = "BtnSelect";
            BtnSelect.Size = new Size(88, 23);
            BtnSelect.TabIndex = 1;
            BtnSelect.Text = "Select [F8]";
            BtnSelect.UseVisualStyleBackColor = true;
            BtnSelect.Click += BtnSelect_Click;
            BtnSelect.PreviewKeyDown += BtnSelect_PreviewKeyDown;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { RecentPurchaseErrorMsg });
            statusStrip1.Location = new Point(0, 263);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(529, 22);
            statusStrip1.TabIndex = 153;
            statusStrip1.Text = "statusStrip1";
            // 
            // RecentPurchaseErrorMsg
            // 
            RecentPurchaseErrorMsg.Name = "RecentPurchaseErrorMsg";
            RecentPurchaseErrorMsg.Size = new Size(49, 17);
            RecentPurchaseErrorMsg.Text = "              ";
            // 
            // GridViewRecentSales
            // 
            GridViewRecentSales.AllowUserToAddRows = false;
            GridViewRecentSales.AllowUserToDeleteRows = false;
            GridViewRecentSales.AllowUserToResizeColumns = false;
            GridViewRecentSales.AllowUserToResizeRows = false;
            GridViewRecentSales.BackgroundColor = SystemColors.Control;
            GridViewRecentSales.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewRecentSales.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewRecentSales.ColumnHeadersHeight = 20;
            GridViewRecentSales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewRecentSales.Columns.AddRange(new DataGridViewColumn[] { Date, Customer, InvNo, Amount, Column10 });
            GridViewRecentSales.EnableHeadersVisualStyles = false;
            GridViewRecentSales.Location = new Point(4, 3);
            GridViewRecentSales.MultiSelect = false;
            GridViewRecentSales.Name = "GridViewRecentSales";
            GridViewRecentSales.ReadOnly = true;
            GridViewRecentSales.RowHeadersVisible = false;
            GridViewRecentSales.RowTemplate.Height = 20;
            GridViewRecentSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewRecentSales.ShowCellToolTips = false;
            GridViewRecentSales.Size = new Size(522, 219);
            GridViewRecentSales.TabIndex = 0;
            GridViewRecentSales.TabStop = false;
            GridViewRecentSales.CellDoubleClick += GridViewRecentSales_CellDoubleClick;
            GridViewRecentSales.KeyDown += GridViewRecentSales_KeyDown;
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
            // Customer
            // 
            Customer.HeaderText = "Customer";
            Customer.Name = "Customer";
            Customer.ReadOnly = true;
            Customer.Resizable = DataGridViewTriState.False;
            Customer.SortMode = DataGridViewColumnSortMode.NotSortable;
            Customer.Width = 225;
            // 
            // InvNo
            // 
            InvNo.HeaderText = "Inv No";
            InvNo.Name = "InvNo";
            InvNo.ReadOnly = true;
            InvNo.Resizable = DataGridViewTriState.False;
            InvNo.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Amount
            // 
            Amount.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
            // FormRecentSales
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnCancel;
            ClientSize = new Size(529, 285);
            Controls.Add(BtnCancel);
            Controls.Add(BtnSelect);
            Controls.Add(statusStrip1);
            Controls.Add(GridViewRecentSales);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormRecentSales";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Recent Sales";
            Load += FormRecentSales_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewRecentSales).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnSelect;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel RecentPurchaseErrorMsg;
        private controls.DataViewVerticalScroll GridViewRecentSales;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Customer;
        private System.Windows.Forms.DataGridViewTextBoxColumn InvNo;
        private controls.grid.DataGridViewCurrencyColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
    }
}