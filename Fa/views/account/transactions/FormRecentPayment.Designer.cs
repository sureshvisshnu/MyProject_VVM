namespace fa.views.account.transactions
{
    partial class FormRecentPayment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecentPayment));
            BtnCancel = new Button();
            BtnSelect = new Button();
            statusStrip1 = new StatusStrip();
            RecentPaymentErrorMsg = new ToolStripStatusLabel();
            GridViewPaymentRecent = new controls.DataViewVerticalScroll();
            Date = new DataGridViewTextBoxColumn();
            Supplier = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Amount = new DataGridViewTextBoxColumn();
            Column10 = new DataGridViewTextBoxColumn();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewPaymentRecent).BeginInit();
            SuspendLayout();
            // 
            // BtnCancel
            // 
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(343, 227);
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
            BtnSelect.Location = new Point(436, 227);
            BtnSelect.Name = "BtnSelect";
            BtnSelect.Size = new Size(88, 23);
            BtnSelect.TabIndex = 155;
            BtnSelect.Text = "Select [F8]";
            BtnSelect.UseVisualStyleBackColor = true;
            BtnSelect.Click += BtnSelect_Click;
            BtnSelect.PreviewKeyDown += BtnSelect_PreviewKeyDown;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { RecentPaymentErrorMsg });
            statusStrip1.Location = new Point(0, 259);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(534, 22);
            statusStrip1.TabIndex = 157;
            statusStrip1.Text = "statusStrip1";
            // 
            // RecentPaymentErrorMsg
            // 
            RecentPaymentErrorMsg.Name = "RecentPaymentErrorMsg";
            RecentPaymentErrorMsg.Size = new Size(49, 17);
            RecentPaymentErrorMsg.Text = "              ";
            // 
            // GridViewPaymentRecent
            // 
            GridViewPaymentRecent.AllowUserToAddRows = false;
            GridViewPaymentRecent.AllowUserToDeleteRows = false;
            GridViewPaymentRecent.AllowUserToResizeColumns = false;
            GridViewPaymentRecent.AllowUserToResizeRows = false;
            GridViewPaymentRecent.BackgroundColor = SystemColors.ControlLight;
            GridViewPaymentRecent.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewPaymentRecent.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewPaymentRecent.ColumnHeadersHeight = 20;
            GridViewPaymentRecent.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewPaymentRecent.Columns.AddRange(new DataGridViewColumn[] { Date, Supplier, Column1, Amount, Column10 });
            GridViewPaymentRecent.EnableHeadersVisualStyles = false;
            GridViewPaymentRecent.Location = new Point(7, 3);
            GridViewPaymentRecent.MultiSelect = false;
            GridViewPaymentRecent.Name = "GridViewPaymentRecent";
            GridViewPaymentRecent.ReadOnly = true;
            GridViewPaymentRecent.RowHeadersVisible = false;
            GridViewPaymentRecent.RowTemplate.Height = 20;
            GridViewPaymentRecent.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewPaymentRecent.ShowCellToolTips = false;
            GridViewPaymentRecent.Size = new Size(521, 219);
            GridViewPaymentRecent.TabIndex = 154;
            GridViewPaymentRecent.TabStop = false;
            GridViewPaymentRecent.CellDoubleClick += GridViewPaymentRecent_CellDoubleClick;
            GridViewPaymentRecent.KeyDown += GridViewPaymentRecent_KeyDown;
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
            Supplier.HeaderText = "Supplier";
            Supplier.Name = "Supplier";
            Supplier.ReadOnly = true;
            Supplier.Resizable = DataGridViewTriState.False;
            Supplier.SortMode = DataGridViewColumnSortMode.NotSortable;
            Supplier.Width = 225;
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
            // FormRecentPayment
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(534, 281);
            Controls.Add(BtnCancel);
            Controls.Add(BtnSelect);
            Controls.Add(statusStrip1);
            Controls.Add(GridViewPaymentRecent);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormRecentPayment";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Recent Payment";
            Load += FormRecentPayment_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewPaymentRecent).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnSelect;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel RecentPaymentErrorMsg;
        private controls.DataViewVerticalScroll GridViewPaymentRecent;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Supplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private DataGridViewTextBoxColumn Amount;
    }
}