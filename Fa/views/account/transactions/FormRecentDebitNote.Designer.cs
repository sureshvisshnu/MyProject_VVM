namespace fa.views.account.transactions
{
    partial class FormRecentDebitNote
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecentDebitNote));
            BtnCancel = new Button();
            BtnSelect = new Button();
            statusStrip1 = new StatusStrip();
            RecentPurchaseErrorMsg = new ToolStripStatusLabel();
            GridViewDebitNoteRecentInvoice = new controls.DataViewVerticalScroll();
            Date = new DataGridViewTextBoxColumn();
            Supplier = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Amount = new DataGridViewTextBoxColumn();
            Column10 = new DataGridViewTextBoxColumn();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewDebitNoteRecentInvoice).BeginInit();
            SuspendLayout();
            // 
            // BtnCancel
            // 
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(343, 227);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(88, 23);
            BtnCancel.TabIndex = 152;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // BtnSelect
            // 
            BtnSelect.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSelect.Location = new Point(436, 227);
            BtnSelect.Name = "BtnSelect";
            BtnSelect.Size = new Size(88, 23);
            BtnSelect.TabIndex = 151;
            BtnSelect.Text = "Select [F8]";
            BtnSelect.UseVisualStyleBackColor = true;
            BtnSelect.Click += BtnSelect_Click;
            BtnSelect.PreviewKeyDown += BtnSelect_PreviewKeyDown;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { RecentPurchaseErrorMsg });
            statusStrip1.Location = new Point(0, 259);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(534, 22);
            statusStrip1.TabIndex = 153;
            statusStrip1.Text = "statusStrip1";
            // 
            // RecentPurchaseErrorMsg
            // 
            RecentPurchaseErrorMsg.Name = "RecentPurchaseErrorMsg";
            RecentPurchaseErrorMsg.Size = new Size(49, 17);
            RecentPurchaseErrorMsg.Text = "              ";
            // 
            // GridViewDebitNoteRecentInvoice
            // 
            GridViewDebitNoteRecentInvoice.AllowUserToAddRows = false;
            GridViewDebitNoteRecentInvoice.AllowUserToDeleteRows = false;
            GridViewDebitNoteRecentInvoice.AllowUserToResizeColumns = false;
            GridViewDebitNoteRecentInvoice.AllowUserToResizeRows = false;
            GridViewDebitNoteRecentInvoice.BackgroundColor = SystemColors.ControlLight;
            GridViewDebitNoteRecentInvoice.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewDebitNoteRecentInvoice.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewDebitNoteRecentInvoice.ColumnHeadersHeight = 20;
            GridViewDebitNoteRecentInvoice.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewDebitNoteRecentInvoice.Columns.AddRange(new DataGridViewColumn[] { Date, Supplier, Column1, Amount, Column10 });
            GridViewDebitNoteRecentInvoice.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewDebitNoteRecentInvoice.EnableHeadersVisualStyles = false;
            GridViewDebitNoteRecentInvoice.Location = new Point(7, 3);
            GridViewDebitNoteRecentInvoice.MultiSelect = false;
            GridViewDebitNoteRecentInvoice.Name = "GridViewDebitNoteRecentInvoice";
            GridViewDebitNoteRecentInvoice.ReadOnly = true;
            GridViewDebitNoteRecentInvoice.RowHeadersVisible = false;
            GridViewDebitNoteRecentInvoice.RowTemplate.Height = 20;
            GridViewDebitNoteRecentInvoice.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewDebitNoteRecentInvoice.ShowCellToolTips = false;
            GridViewDebitNoteRecentInvoice.Size = new Size(521, 219);
            GridViewDebitNoteRecentInvoice.TabIndex = 150;
            GridViewDebitNoteRecentInvoice.TabStop = false;
            GridViewDebitNoteRecentInvoice.CellDoubleClick += GridViewDebitNoteRecentInvoice_CellDoubleClick;
            GridViewDebitNoteRecentInvoice.KeyDown += GridViewDebitNoteRecentInvoice_KeyDown;
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
            Column1.HeaderText = "Ref No";
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
            Amount.SortMode = DataGridViewColumnSortMode.NotSortable;
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
            // FormRecentDebitNote
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(534, 281);
            Controls.Add(BtnCancel);
            Controls.Add(BtnSelect);
            Controls.Add(statusStrip1);
            Controls.Add(GridViewDebitNoteRecentInvoice);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormRecentDebitNote";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Recent Debit Notes";
            Load += FormRecentDebitNote_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewDebitNoteRecentInvoice).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnSelect;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel RecentPurchaseErrorMsg;
        private controls.DataViewVerticalScroll GridViewDebitNoteRecentInvoice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Supplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private DataGridViewTextBoxColumn Amount;
    }
}