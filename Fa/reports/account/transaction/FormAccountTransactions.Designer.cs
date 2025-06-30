namespace fa.reports.account.transaction
{
    partial class FormAccountTransactions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAccountTransactions));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            statusStrip1 = new StatusStrip();
            TransactionErrorMsg = new ToolStripStatusLabel();
            ab2ToolStrip1 = new views.controls.Ab2ToolStrip();
            ComboBoxTransaction = new ToolStripComboBox();
            toolStripLabel1 = new ToolStripLabel();
            FromDate = new views.controls.ToolStripCalendar();
            toolStripLabel2 = new ToolStripLabel();
            ToDate = new views.controls.ToolStripCalendar();
            BtnRunReport = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            ToolStripBtnSave = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            ToolStripBtnPrint = new ToolStripButton();
            BtnExit = new Button();
            BtnReset = new Button();
            BtnPrint = new Button();
            BtnSave = new Button();
            GridViewTransaction = new views.controls.DataViewVerticalScroll();
            Date = new DataGridViewTextBoxColumn();
            Reference = new DataGridViewTextBoxColumn();
            Account = new DataGridViewTextBoxColumn();
            Description = new DataGridViewTextBoxColumn();
            Amount = new DataGridViewTextBoxColumn();
            printDialog1 = new PrintDialog();
            statusStrip1.SuspendLayout();
            ab2ToolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewTransaction).BeginInit();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { TransactionErrorMsg });
            statusStrip1.Location = new Point(0, 567);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(923, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // TransactionErrorMsg
            // 
            TransactionErrorMsg.Name = "TransactionErrorMsg";
            TransactionErrorMsg.Size = new Size(0, 17);
            // 
            // ab2ToolStrip1
            // 
            ab2ToolStrip1.BackColor = SystemColors.ControlLight;
            ab2ToolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            ab2ToolStrip1.ImageScalingSize = new Size(24, 24);
            ab2ToolStrip1.Items.AddRange(new ToolStripItem[] { ComboBoxTransaction, toolStripLabel1, FromDate, toolStripLabel2, ToDate, BtnRunReport, toolStripSeparator1, ToolStripBtnSave, toolStripSeparator3, ToolStripBtnPrint });
            ab2ToolStrip1.Location = new Point(0, 0);
            ab2ToolStrip1.Name = "ab2ToolStrip1";
            ab2ToolStrip1.Padding = new Padding(5);
            ab2ToolStrip1.Size = new Size(923, 38);
            ab2ToolStrip1.TabIndex = 0;
            ab2ToolStrip1.Text = "ab2ToolStrip1";
            // 
            // ComboBoxTransaction
            // 
            ComboBoxTransaction.FlatStyle = FlatStyle.Standard;
            ComboBoxTransaction.Items.AddRange(new object[] { "Invoice", "Bill", "Receipt", "Payment", "Credit Note", "Debit Note", "Expense", "Journal" });
            ComboBoxTransaction.Name = "ComboBoxTransaction";
            ComboBoxTransaction.Size = new Size(121, 28);
            ComboBoxTransaction.SelectedIndexChanged += ComboBoxTransaction_SelectedIndexChanged;
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(35, 25);
            toolStripLabel1.Text = "From";
            // 
            // FromDate
            // 
            FromDate.BackColor = Color.White;
            FromDate.Date = null;
            FromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FromDate.Format = "MM/dd/yyyy";
            FromDate.MaxDate = new DateTime(9997, 12, 31, 9, 35, 26, 0);
            FromDate.MinDate = new DateTime(1900, 1, 1, 22, 56, 10, 0);
            FromDate.Name = "FromDate";
            FromDate.Size = new Size(97, 25);
            FromDate.Text = "toolStripCalendar1";
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(20, 25);
            toolStripLabel2.Text = "To";
            // 
            // ToDate
            // 
            ToDate.BackColor = Color.White;
            ToDate.Date = null;
            ToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ToDate.Format = "MM/dd/yyyy";
            ToDate.MaxDate = new DateTime(9997, 12, 31, 9, 49, 44, 0);
            ToDate.MinDate = new DateTime(1900, 1, 1, 22, 56, 10, 0);
            ToDate.Name = "ToDate";
            ToDate.Size = new Size(97, 25);
            ToDate.Text = "toolStripCalendar2";
            // 
            // BtnRunReport
            // 
            BtnRunReport.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnRunReport.Image = (Image)resources.GetObject("BtnRunReport.Image");
            BtnRunReport.ImageTransparentColor = Color.Magenta;
            BtnRunReport.Name = "BtnRunReport";
            BtnRunReport.Size = new Size(70, 25);
            BtnRunReport.Text = "Run Report";
            BtnRunReport.Click += BtnRunReport_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 28);
            // 
            // ToolStripBtnSave
            // 
            ToolStripBtnSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnSave.Image = (Image)resources.GetObject("ToolStripBtnSave.Image");
            ToolStripBtnSave.ImageScaling = ToolStripItemImageScaling.None;
            ToolStripBtnSave.ImageTransparentColor = Color.Black;
            ToolStripBtnSave.Name = "ToolStripBtnSave";
            ToolStripBtnSave.Size = new Size(23, 25);
            ToolStripBtnSave.Text = "Save";
            ToolStripBtnSave.Click += BtnSave_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 28);
            // 
            // ToolStripBtnPrint
            // 
            ToolStripBtnPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnPrint.Image = (Image)resources.GetObject("ToolStripBtnPrint.Image");
            ToolStripBtnPrint.ImageScaling = ToolStripItemImageScaling.None;
            ToolStripBtnPrint.ImageTransparentColor = Color.Black;
            ToolStripBtnPrint.Name = "ToolStripBtnPrint";
            ToolStripBtnPrint.Size = new Size(23, 25);
            ToolStripBtnPrint.Text = "Print";
            ToolStripBtnPrint.Click += BtnPrint_Click;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(834, 538);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(72, 23);
            BtnExit.TabIndex = 21;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnReset
            // 
            BtnReset.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnReset.Location = new Point(584, 538);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new Size(82, 23);
            BtnReset.TabIndex = 20;
            BtnReset.Text = "Reset [Esc]";
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Enabled = false;
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(753, 538);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 23);
            BtnPrint.TabIndex = 19;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnSave
            // 
            BtnSave.Enabled = false;
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(672, 538);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 18;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // GridViewTransaction
            // 
            GridViewTransaction.AllowUserToAddRows = false;
            GridViewTransaction.AllowUserToDeleteRows = false;
            GridViewTransaction.AllowUserToResizeColumns = false;
            GridViewTransaction.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            GridViewTransaction.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.ControlLight;
            dataGridViewCellStyle2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.ControlLight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            GridViewTransaction.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            GridViewTransaction.ColumnHeadersHeight = 20;
            GridViewTransaction.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewTransaction.Columns.AddRange(new DataGridViewColumn[] { Date, Reference, Account, Description, Amount });
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Window;
            dataGridViewCellStyle8.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle8.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = Color.White;
            dataGridViewCellStyle8.SelectionForeColor = Color.Black;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            GridViewTransaction.DefaultCellStyle = dataGridViewCellStyle8;
            GridViewTransaction.EnableHeadersVisualStyles = false;
            GridViewTransaction.Location = new Point(2, 41);
            GridViewTransaction.Name = "GridViewTransaction";
            GridViewTransaction.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            GridViewTransaction.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            GridViewTransaction.RowHeadersVisible = false;
            dataGridViewCellStyle10.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            GridViewTransaction.RowsDefaultCellStyle = dataGridViewCellStyle10;
            GridViewTransaction.RowTemplate.Height = 20;
            GridViewTransaction.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewTransaction.ShowCellToolTips = false;
            GridViewTransaction.Size = new Size(919, 489);
            GridViewTransaction.TabIndex = 22;
            GridViewTransaction.CellPainting += GridViewTransaction_CellPainting;
            // 
            // Date
            // 
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Date.DefaultCellStyle = dataGridViewCellStyle3;
            Date.HeaderText = "Date";
            Date.Name = "Date";
            Date.ReadOnly = true;
            Date.Resizable = DataGridViewTriState.False;
            Date.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Reference
            // 
            dataGridViewCellStyle4.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Reference.DefaultCellStyle = dataGridViewCellStyle4;
            Reference.HeaderText = "Reference";
            Reference.Name = "Reference";
            Reference.ReadOnly = true;
            Reference.Resizable = DataGridViewTriState.False;
            Reference.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Account
            // 
            dataGridViewCellStyle5.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Account.DefaultCellStyle = dataGridViewCellStyle5;
            Account.HeaderText = "Account";
            Account.Name = "Account";
            Account.ReadOnly = true;
            Account.Resizable = DataGridViewTriState.False;
            Account.SortMode = DataGridViewColumnSortMode.NotSortable;
            Account.Width = 200;
            // 
            // Description
            // 
            dataGridViewCellStyle6.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Description.DefaultCellStyle = dataGridViewCellStyle6;
            Description.HeaderText = "Description";
            Description.Name = "Description";
            Description.ReadOnly = true;
            Description.Resizable = DataGridViewTriState.False;
            Description.SortMode = DataGridViewColumnSortMode.NotSortable;
            Description.Width = 400;
            // 
            // Amount
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Amount.DefaultCellStyle = dataGridViewCellStyle7;
            Amount.HeaderText = "Amount";
            Amount.Name = "Amount";
            Amount.ReadOnly = true;
            Amount.Resizable = DataGridViewTriState.False;
            Amount.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // printDialog1
            // 
            printDialog1.UseEXDialog = true;
            // 
            // FormAccountTransactions
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(923, 589);
            Controls.Add(GridViewTransaction);
            Controls.Add(BtnExit);
            Controls.Add(BtnReset);
            Controls.Add(BtnPrint);
            Controls.Add(BtnSave);
            Controls.Add(statusStrip1);
            Controls.Add(ab2ToolStrip1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormAccountTransactions";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Transaction Reports";
            Load += FormAccountTransactions_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ab2ToolStrip1.ResumeLayout(false);
            ab2ToolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewTransaction).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private views.controls.Ab2ToolStrip ab2ToolStrip1;
        private System.Windows.Forms.ToolStripComboBox ComboBoxTransaction;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private views.controls.ToolStripCalendar FromDate;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private views.controls.ToolStripCalendar ToDate;
        private System.Windows.Forms.ToolStripButton BtnRunReport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ToolStripBtnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton ToolStripBtnPrint;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Button BtnReset;
        private System.Windows.Forms.Button BtnPrint;
        private System.Windows.Forms.Button BtnSave;
        private views.controls.DataViewVerticalScroll GridViewTransaction;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.ToolStripStatusLabel TransactionErrorMsg;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Reference;
        private System.Windows.Forms.DataGridViewTextBoxColumn Account;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
    }
}