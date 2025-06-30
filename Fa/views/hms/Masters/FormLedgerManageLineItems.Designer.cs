namespace fa.views.hms.Masters
{
    partial class FormLedgerManageLineItems
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
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle14 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLedgerManageLineItems));
            PatientInfoMiniHorizontal = new controls.hms.PatientInfoMiniHorizontal();
            DataGridViewLineItems = new controls.DataViewVerticalScroll();
            BtnExit = new Button();
            statusStrip1 = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            BtnSave = new Button();
            DataGridViewInvoiceTotal = new DataGridView();
            Total = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            empty = new DataGridViewTextBoxColumn();
            Sequence = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Description = new DataGridViewTextBoxColumn();
            FeeCharged = new controls.grid.DataGridViewCurrencyColumn();
            DeleteRow = new DataGridViewButtonColumn();
            Id = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)DataGridViewLineItems).BeginInit();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DataGridViewInvoiceTotal).BeginInit();
            SuspendLayout();
            // 
            // PatientInfoMiniHorizontal
            // 
            PatientInfoMiniHorizontal.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            PatientInfoMiniHorizontal.Location = new Point(2, -2);
            PatientInfoMiniHorizontal.Name = "PatientInfoMiniHorizontal";
            PatientInfoMiniHorizontal.PatientId = null;
            PatientInfoMiniHorizontal.Size = new Size(764, 121);
            PatientInfoMiniHorizontal.TabIndex = 11;
            PatientInfoMiniHorizontal.TabStop = false;
            // 
            // DataGridViewLineItems
            // 
            DataGridViewLineItems.AllowUserToAddRows = false;
            DataGridViewLineItems.AllowUserToDeleteRows = false;
            DataGridViewLineItems.AllowUserToResizeColumns = false;
            DataGridViewLineItems.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = Color.White;
            dataGridViewCellStyle1.SelectionForeColor = Color.Black;
            DataGridViewLineItems.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            DataGridViewLineItems.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            DataGridViewLineItems.BackgroundColor = SystemColors.ButtonFace;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            DataGridViewLineItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            DataGridViewLineItems.ColumnHeadersHeight = 20;
            DataGridViewLineItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DataGridViewLineItems.Columns.AddRange(new DataGridViewColumn[] { Sequence, Column1, Description, FeeCharged, DeleteRow, Id, Column5, Column3 });
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = Color.White;
            dataGridViewCellStyle8.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle8.ForeColor = Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = Color.White;
            dataGridViewCellStyle8.SelectionForeColor = Color.Black;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            DataGridViewLineItems.DefaultCellStyle = dataGridViewCellStyle8;
            DataGridViewLineItems.EditMode = DataGridViewEditMode.EditOnEnter;
            DataGridViewLineItems.EnableHeadersVisualStyles = false;
            DataGridViewLineItems.Location = new Point(6, 122);
            DataGridViewLineItems.MultiSelect = false;
            DataGridViewLineItems.Name = "DataGridViewLineItems";
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = Color.White;
            dataGridViewCellStyle9.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle9.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = Color.White;
            dataGridViewCellStyle9.SelectionForeColor = Color.Black;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            DataGridViewLineItems.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            DataGridViewLineItems.RowHeadersVisible = false;
            DataGridViewLineItems.RowHeadersWidth = 25;
            dataGridViewCellStyle10.BackColor = Color.White;
            dataGridViewCellStyle10.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle10.SelectionBackColor = Color.White;
            dataGridViewCellStyle10.SelectionForeColor = Color.FromArgb(64, 64, 64);
            DataGridViewLineItems.RowsDefaultCellStyle = dataGridViewCellStyle10;
            DataGridViewLineItems.RowTemplate.Height = 20;
            DataGridViewLineItems.ScrollBars = ScrollBars.Vertical;
            DataGridViewLineItems.SelectionMode = DataGridViewSelectionMode.CellSelect;
            DataGridViewLineItems.ShowCellToolTips = false;
            DataGridViewLineItems.Size = new Size(768, 272);
            DataGridViewLineItems.TabIndex = 2;
            DataGridViewLineItems.CellClick += DataGridViewInvoice_CellClick;
            DataGridViewLineItems.CellEndEdit += DataGridViewInvoice_CellEndEdit;
            DataGridViewLineItems.CellEnter += DataGridViewInvoice_CellEnter;
            DataGridViewLineItems.DataError += DataGridViewInvoice_DataError;
            DataGridViewLineItems.RowsAdded += DataGridViewLineItems_RowsAdded;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(691, 428);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(75, 22);
            BtnExit.TabIndex = 8;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsg });
            statusStrip1.Location = new Point(0, 463);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(781, 22);
            statusStrip1.TabIndex = 76;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(28, 17);
            ErrorMsg.Text = "       ";
            // 
            // BtnSave
            // 
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(602, 428);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(83, 22);
            BtnSave.TabIndex = 3;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            BtnSave.PreviewKeyDown += BtnSave_PreviewKeyDown;
            // 
            // DataGridViewInvoiceTotal
            // 
            DataGridViewInvoiceTotal.BackgroundColor = SystemColors.ButtonFace;
            DataGridViewInvoiceTotal.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridViewInvoiceTotal.ColumnHeadersVisible = false;
            DataGridViewInvoiceTotal.Columns.AddRange(new DataGridViewColumn[] { Total, Column2, empty });
            dataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = SystemColors.Window;
            dataGridViewCellStyle13.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle13.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle13.SelectionBackColor = Color.White;
            dataGridViewCellStyle13.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = DataGridViewTriState.False;
            DataGridViewInvoiceTotal.DefaultCellStyle = dataGridViewCellStyle13;
            DataGridViewInvoiceTotal.Enabled = false;
            DataGridViewInvoiceTotal.Location = new Point(6, 392);
            DataGridViewInvoiceTotal.Name = "DataGridViewInvoiceTotal";
            DataGridViewInvoiceTotal.ReadOnly = true;
            DataGridViewInvoiceTotal.RowHeadersVisible = false;
            dataGridViewCellStyle14.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle14.BackColor = SystemColors.Control;
            dataGridViewCellStyle14.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle14.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle14.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle14.WrapMode = DataGridViewTriState.True;
            DataGridViewInvoiceTotal.RowsDefaultCellStyle = dataGridViewCellStyle14;
            DataGridViewInvoiceTotal.ScrollBars = ScrollBars.None;
            DataGridViewInvoiceTotal.Size = new Size(768, 22);
            DataGridViewInvoiceTotal.TabIndex = 78;
            DataGridViewInvoiceTotal.TabStop = false;
            // 
            // Total
            // 
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.Font = new Font("Tahoma", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            Total.DefaultCellStyle = dataGridViewCellStyle11;
            Total.HeaderText = "";
            Total.Name = "Total";
            Total.ReadOnly = true;
            Total.Width = 598;
            // 
            // Column2
            // 
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleRight;
            Column2.DefaultCellStyle = dataGridViewCellStyle12;
            Column2.HeaderText = "Column2";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Width = 125;
            // 
            // empty
            // 
            empty.HeaderText = "empty";
            empty.Name = "empty";
            empty.ReadOnly = true;
            empty.Width = 25;
            // 
            // Sequence
            // 
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.White;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            Sequence.DefaultCellStyle = dataGridViewCellStyle3;
            Sequence.HeaderText = "#";
            Sequence.Name = "Sequence";
            Sequence.Resizable = DataGridViewTriState.False;
            Sequence.SortMode = DataGridViewColumnSortMode.NotSortable;
            Sequence.Width = 30;
            // 
            // Column1
            // 
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.ForeColor = Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = Color.White;
            dataGridViewCellStyle4.SelectionForeColor = Color.Black;
            Column1.DefaultCellStyle = dataGridViewCellStyle4;
            Column1.HeaderText = "Name [F2]";
            Column1.Name = "Column1";
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 215;
            // 
            // Description
            // 
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.ForeColor = Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = Color.White;
            dataGridViewCellStyle5.SelectionForeColor = Color.Black;
            Description.DefaultCellStyle = dataGridViewCellStyle5;
            Description.HeaderText = "Description";
            Description.MaxInputLength = 250;
            Description.Name = "Description";
            Description.Resizable = DataGridViewTriState.False;
            Description.SortMode = DataGridViewColumnSortMode.NotSortable;
            Description.Width = 353;
            // 
            // FeeCharged
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle6.BackColor = Color.White;
            dataGridViewCellStyle6.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle6.ForeColor = Color.Black;
            dataGridViewCellStyle6.NullValue = "0.00";
            dataGridViewCellStyle6.SelectionBackColor = Color.White;
            dataGridViewCellStyle6.SelectionForeColor = Color.Black;
            FeeCharged.DefaultCellStyle = dataGridViewCellStyle6;
            FeeCharged.HeaderText = "Fee Charged";
            FeeCharged.Name = "FeeCharged";
            FeeCharged.Resizable = DataGridViewTriState.False;
            FeeCharged.Width = 125;
            // 
            // DeleteRow
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = Color.White;
            dataGridViewCellStyle7.ForeColor = Color.Black;
            dataGridViewCellStyle7.NullValue = "X";
            dataGridViewCellStyle7.SelectionBackColor = Color.White;
            dataGridViewCellStyle7.SelectionForeColor = Color.Black;
            DeleteRow.DefaultCellStyle = dataGridViewCellStyle7;
            DeleteRow.HeaderText = "";
            DeleteRow.Name = "DeleteRow";
            DeleteRow.Resizable = DataGridViewTriState.False;
            DeleteRow.Width = 25;
            // 
            // Id
            // 
            Id.HeaderText = "Id";
            Id.Name = "Id";
            Id.ReadOnly = true;
            Id.Resizable = DataGridViewTriState.False;
            Id.SortMode = DataGridViewColumnSortMode.NotSortable;
            Id.Visible = false;
            Id.Width = 5;
            // 
            // Column5
            // 
            Column5.HeaderText = "InvId";
            Column5.Name = "Column5";
            Column5.Resizable = DataGridViewTriState.False;
            Column5.Visible = false;
            // 
            // Column3
            // 
            Column3.HeaderText = "FeeId";
            Column3.Name = "Column3";
            Column3.Visible = false;
            // 
            // FormLedgerManageLineItems
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(781, 485);
            Controls.Add(DataGridViewInvoiceTotal);
            Controls.Add(BtnExit);
            Controls.Add(statusStrip1);
            Controls.Add(BtnSave);
            Controls.Add(DataGridViewLineItems);
            Controls.Add(PatientInfoMiniHorizontal);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormLedgerManageLineItems";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Manage Line Items";
            FormClosing += FormGenerateInvoice_FormClosing;
            Load += FormGenerateInvoice_Load;
            Controls.SetChildIndex(PatientInfoMiniHorizontal, 0);
            Controls.SetChildIndex(DataGridViewLineItems, 0);
            Controls.SetChildIndex(BtnSave, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(BtnExit, 0);
            Controls.SetChildIndex(DataGridViewInvoiceTotal, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            ((System.ComponentModel.ISupportInitialize)DataGridViewLineItems).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DataGridViewInvoiceTotal).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private controls.hms.PatientInfoMiniHorizontal PatientInfoMiniHorizontal;
        private controls.DataViewVerticalScroll DataGridViewLineItems;
        private Button BtnExit;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel ErrorMsg;
        private Button BtnSave;
        private DataGridView DataGridViewInvoiceTotal;
        private DataGridViewTextBoxColumn Total;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn empty;
        private DataGridViewTextBoxColumn Sequence;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Description;
        private controls.grid.DataGridViewCurrencyColumn FeeCharged;
        private DataGridViewButtonColumn DeleteRow;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column3;
    }
}