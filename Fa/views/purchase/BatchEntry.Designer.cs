namespace fa.views.purchase
{
    partial class FormBatchEntry
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
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBatchEntry));
            BtnBatchEntryCancel = new Button();
            BtnBatchEntrySave = new Button();
            DataGridViewBatchEntry = new controls.DataViewVerticalScroll();
            statusStrip1 = new StatusStrip();
            BatchErrorMsg = new ToolStripStatusLabel();
            Sequence = new DataGridViewTextBoxColumn();
            BatchNumber = new DataGridViewTextBoxColumn();
            ExpiryDate = new controls.grid.DataGridViewCalendarColumn();
            BatchQuantity = new DataGridViewTextBoxColumn();
            Amount = new controls.grid.DataGridViewCurrencyColumn();
            Column2 = new DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)DataGridViewBatchEntry).BeginInit();
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
            // BtnBatchEntryCancel
            // 
            BtnBatchEntryCancel.DialogResult = DialogResult.Cancel;
            BtnBatchEntryCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnBatchEntryCancel.Location = new Point(312, 188);
            BtnBatchEntryCancel.Name = "BtnBatchEntryCancel";
            BtnBatchEntryCancel.Size = new Size(90, 23);
            BtnBatchEntryCancel.TabIndex = 2;
            BtnBatchEntryCancel.Text = "Cancel [ Esc ]";
            BtnBatchEntryCancel.UseVisualStyleBackColor = true;
            BtnBatchEntryCancel.Click += btnBatchEntryCancel_Click;
            // 
            // BtnBatchEntrySave
            // 
            BtnBatchEntrySave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnBatchEntrySave.Location = new Point(408, 188);
            BtnBatchEntrySave.Name = "BtnBatchEntrySave";
            BtnBatchEntrySave.Size = new Size(83, 23);
            BtnBatchEntrySave.TabIndex = 1;
            BtnBatchEntrySave.Text = "Save [ F8 ]";
            BtnBatchEntrySave.UseVisualStyleBackColor = true;
            BtnBatchEntrySave.Click += btnBatchEntrySave_Click;
            // 
            // DataGridViewBatchEntry
            // 
            DataGridViewBatchEntry.BackgroundColor = SystemColors.Control;
            DataGridViewBatchEntry.BorderStyle = BorderStyle.Fixed3D;
            DataGridViewBatchEntry.ColumnHeadersHeight = 20;
            DataGridViewBatchEntry.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DataGridViewBatchEntry.Columns.AddRange(new DataGridViewColumn[] { Sequence, BatchNumber, ExpiryDate, BatchQuantity, Amount, Column2 });
            DataGridViewBatchEntry.EditMode = DataGridViewEditMode.EditOnEnter;
            DataGridViewBatchEntry.EnableHeadersVisualStyles = false;
            DataGridViewBatchEntry.Location = new Point(2, 2);
            DataGridViewBatchEntry.Name = "DataGridViewBatchEntry";
            DataGridViewBatchEntry.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.ForeColor = Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = Color.White;
            dataGridViewCellStyle4.SelectionForeColor = Color.Black;
            DataGridViewBatchEntry.RowsDefaultCellStyle = dataGridViewCellStyle4;
            DataGridViewBatchEntry.RowTemplate.Height = 20;
            DataGridViewBatchEntry.ShowCellToolTips = false;
            DataGridViewBatchEntry.Size = new Size(500, 181);
            DataGridViewBatchEntry.TabIndex = 0;
            DataGridViewBatchEntry.CellClick += DataGridViewBatchEntry_CellClick;
            DataGridViewBatchEntry.CellEndEdit += DataGridViewBatchEntry_CellEndEdit;
            DataGridViewBatchEntry.CellEnter += DataGridViewBatchEntry_CellEnter;
            DataGridViewBatchEntry.RowsAdded += DataGridViewBatchEntry_RowsAdded;
            DataGridViewBatchEntry.Enter += DataGridViewBatchEntry_Enter;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { BatchErrorMsg });
            statusStrip1.Location = new Point(0, 216);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(503, 22);
            statusStrip1.TabIndex = 32;
            statusStrip1.Text = "statusStrip1";
            // 
            // BatchErrorMsg
            // 
            BatchErrorMsg.Name = "BatchErrorMsg";
            BatchErrorMsg.Size = new Size(46, 17);
            BatchErrorMsg.Text = "             ";
            // 
            // Sequence
            // 
            Sequence.HeaderText = "#";
            Sequence.Name = "Sequence";
            Sequence.Width = 25;
            // 
            // BatchNumber
            // 
            BatchNumber.HeaderText = "Batch Number";
            BatchNumber.MaxInputLength = 15;
            BatchNumber.Name = "BatchNumber";
            BatchNumber.Width = 150;
            // 
            // ExpiryDate
            // 
            ExpiryDate.HeaderText = "Expiry Date";
            ExpiryDate.Name = "ExpiryDate";
            ExpiryDate.Resizable = DataGridViewTriState.True;
            ExpiryDate.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // BatchQuantity
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.NullValue = "0";
            BatchQuantity.DefaultCellStyle = dataGridViewCellStyle1;
            BatchQuantity.HeaderText = "Qty";
            BatchQuantity.MaxInputLength = 6;
            BatchQuantity.Name = "BatchQuantity";
            BatchQuantity.Width = 75;
            // 
            // Amount
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            Amount.DefaultCellStyle = dataGridViewCellStyle2;
            Amount.HeaderText = "Amount";
            Amount.Name = "Amount";
            // 
            // Column2
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = "X";
            Column2.DefaultCellStyle = dataGridViewCellStyle3;
            Column2.HeaderText = "";
            Column2.Name = "Column2";
            Column2.Width = 25;
            // 
            // FormBatchEntry
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnBatchEntryCancel;
            ClientSize = new Size(503, 238);
            Controls.Add(statusStrip1);
            Controls.Add(BtnBatchEntrySave);
            Controls.Add(BtnBatchEntryCancel);
            Controls.Add(DataGridViewBatchEntry);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormBatchEntry";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Batch Entry";
            Load += FormBatchEntry_Load;
            Controls.SetChildIndex(DataGridViewBatchEntry, 0);
            Controls.SetChildIndex(BtnBatchEntryCancel, 0);
            Controls.SetChildIndex(BtnBatchEntrySave, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            ((System.ComponentModel.ISupportInitialize)DataGridViewBatchEntry).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private controls.DataViewVerticalScroll DataGridViewBatchEntry;
        private System.Windows.Forms.Button BtnBatchEntryCancel;
        private System.Windows.Forms.Button BtnBatchEntrySave;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel BatchErrorMsg;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sequence;
        private System.Windows.Forms.DataGridViewTextBoxColumn BatchNumber;
        private controls.grid.DataGridViewCalendarColumn ExpiryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn BatchQuantity;
        private controls.grid.DataGridViewCurrencyColumn Column1;
        private System.Windows.Forms.DataGridViewButtonColumn Column2;
        private controls.grid.DataGridViewCurrencyColumn Amount;
    }
}