namespace fa.reports.account.transaction.trialbalance
{
    partial class FormTrialBalance
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTrialBalance));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            TrialbalanceToolStrip = new views.controls.Ab2ToolStrip();
            LabelCostCenter = new ToolStripLabel();
            ComboBoxCostCenter = new ToolStripComboBox();
            BtnPeriod = new ToolStripDropDownButton();
            thisMonthToolStripMenuItem = new ToolStripMenuItem();
            thisQuarterToolStripMenuItem = new ToolStripMenuItem();
            thisYearToolStripMenuItem = new ToolStripMenuItem();
            lastYearToolStripMenuItem = new ToolStripMenuItem();
            customToolStripMenuItem = new ToolStripMenuItem();
            toolStripLabel1 = new ToolStripLabel();
            FromDate = new views.controls.ToolStripCalendar();
            toolStripLabel2 = new ToolStripLabel();
            ToDate = new views.controls.ToolStripCalendar();
            BtnRunReport = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            ToolStripBtnSave = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            ToolStripBtnPrint = new ToolStripButton();
            toolStripSeparator = new ToolStripSeparator();
            statusStrip1 = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            TrialBalanceGrid = new views.controls.DataViewVerticalScroll();
            AccountName = new DataGridViewTextBoxColumn();
            Debit = new DataGridViewTextBoxColumn();
            Credit = new DataGridViewTextBoxColumn();
            AccountId = new DataGridViewTextBoxColumn();
            BtnExit = new Button();
            BtnReset = new Button();
            BtnPrint = new Button();
            BtnSave = new Button();
            TrialbalanceToolStrip.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TrialBalanceGrid).BeginInit();
            SuspendLayout();
            // 
            // TrialbalanceToolStrip
            // 
            TrialbalanceToolStrip.AutoSize = false;
            TrialbalanceToolStrip.BackColor = SystemColors.ControlLight;
            TrialbalanceToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            TrialbalanceToolStrip.ImageScalingSize = new Size(20, 20);
            TrialbalanceToolStrip.Items.AddRange(new ToolStripItem[] { LabelCostCenter, ComboBoxCostCenter, BtnPeriod, toolStripLabel1, FromDate, toolStripLabel2, ToDate, BtnRunReport, toolStripSeparator1, ToolStripBtnSave, toolStripSeparator2, ToolStripBtnPrint, toolStripSeparator });
            TrialbalanceToolStrip.Location = new Point(0, 0);
            TrialbalanceToolStrip.Name = "TrialbalanceToolStrip";
            TrialbalanceToolStrip.Padding = new Padding(12, 7, 5, 5);
            TrialbalanceToolStrip.Size = new Size(931, 38);
            TrialbalanceToolStrip.TabIndex = 0;
            TrialbalanceToolStrip.Text = "ab2ToolStrip1";
            // 
            // LabelCostCenter
            // 
            LabelCostCenter.Name = "LabelCostCenter";
            LabelCostCenter.Size = new Size(69, 23);
            LabelCostCenter.Text = "Cost Center";
            // 
            // ComboBoxCostCenter
            // 
            ComboBoxCostCenter.FlatStyle = FlatStyle.Standard;
            ComboBoxCostCenter.Name = "ComboBoxCostCenter";
            ComboBoxCostCenter.Size = new Size(200, 26);
            // 
            // BtnPeriod
            // 
            BtnPeriod.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnPeriod.DropDownItems.AddRange(new ToolStripItem[] { thisMonthToolStripMenuItem, thisQuarterToolStripMenuItem, thisYearToolStripMenuItem, lastYearToolStripMenuItem, customToolStripMenuItem });
            BtnPeriod.Image = (Image)resources.GetObject("BtnPeriod.Image");
            BtnPeriod.ImageTransparentColor = Color.Magenta;
            BtnPeriod.Name = "BtnPeriod";
            BtnPeriod.Size = new Size(54, 23);
            BtnPeriod.Text = "Period";
            BtnPeriod.ToolTipText = "Period";
            BtnPeriod.Visible = false;
            // 
            // thisMonthToolStripMenuItem
            // 
            thisMonthToolStripMenuItem.Name = "thisMonthToolStripMenuItem";
            thisMonthToolStripMenuItem.Size = new Size(138, 22);
            thisMonthToolStripMenuItem.Text = "This Month";
            // 
            // thisQuarterToolStripMenuItem
            // 
            thisQuarterToolStripMenuItem.Name = "thisQuarterToolStripMenuItem";
            thisQuarterToolStripMenuItem.Size = new Size(138, 22);
            thisQuarterToolStripMenuItem.Text = "This Quarter";
            // 
            // thisYearToolStripMenuItem
            // 
            thisYearToolStripMenuItem.Name = "thisYearToolStripMenuItem";
            thisYearToolStripMenuItem.Size = new Size(138, 22);
            thisYearToolStripMenuItem.Text = "This Year";
            // 
            // lastYearToolStripMenuItem
            // 
            lastYearToolStripMenuItem.Name = "lastYearToolStripMenuItem";
            lastYearToolStripMenuItem.Size = new Size(138, 22);
            lastYearToolStripMenuItem.Text = "Last Year";
            // 
            // customToolStripMenuItem
            // 
            customToolStripMenuItem.Name = "customToolStripMenuItem";
            customToolStripMenuItem.Size = new Size(138, 22);
            customToolStripMenuItem.Text = "Custom";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(39, 23);
            toolStripLabel1.Text = "As On";
            // 
            // FromDate
            // 
            FromDate.BackColor = Color.White;
            FromDate.Date = null;
            FromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FromDate.Format = "MM/dd/yyyy";
            FromDate.MaxDate = new DateTime(9997, 12, 31, 1, 13, 24, 0);
            FromDate.MinDate = new DateTime(1900, 1, 1, 23, 32, 14, 0);
            FromDate.Name = "FromDate";
            FromDate.Size = new Size(97, 23);
            FromDate.Text = "toolStripCalendar1";
            FromDate.Visible = false;
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(19, 23);
            toolStripLabel2.Text = "To";
            toolStripLabel2.Visible = false;
            // 
            // ToDate
            // 
            ToDate.BackColor = Color.White;
            ToDate.Date = null;
            ToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ToDate.Format = "MM/dd/yyyy";
            ToDate.MaxDate = new DateTime(9997, 12, 31, 1, 13, 24, 0);
            ToDate.MinDate = new DateTime(1900, 1, 1, 23, 32, 14, 0);
            ToDate.Name = "ToDate";
            ToDate.Size = new Size(97, 23);
            ToDate.Text = "toolStripCalendar2";
            // 
            // BtnRunReport
            // 
            BtnRunReport.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnRunReport.Image = (Image)resources.GetObject("BtnRunReport.Image");
            BtnRunReport.ImageTransparentColor = Color.Magenta;
            BtnRunReport.Name = "BtnRunReport";
            BtnRunReport.Size = new Size(70, 23);
            BtnRunReport.Text = "Run Report";
            BtnRunReport.Click += BtnRunReport_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 26);
            // 
            // ToolStripBtnSave
            // 
            ToolStripBtnSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnSave.Image = (Image)resources.GetObject("ToolStripBtnSave.Image");
            ToolStripBtnSave.ImageTransparentColor = Color.Magenta;
            ToolStripBtnSave.Name = "ToolStripBtnSave";
            ToolStripBtnSave.Size = new Size(24, 23);
            ToolStripBtnSave.Text = "&Save";
            ToolStripBtnSave.Click += BtnSave_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 26);
            // 
            // ToolStripBtnPrint
            // 
            ToolStripBtnPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnPrint.Image = (Image)resources.GetObject("ToolStripBtnPrint.Image");
            ToolStripBtnPrint.ImageTransparentColor = Color.Magenta;
            ToolStripBtnPrint.Name = "ToolStripBtnPrint";
            ToolStripBtnPrint.Size = new Size(24, 23);
            ToolStripBtnPrint.Text = "&Print";
            ToolStripBtnPrint.Click += BtnPrint_Click;
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new Size(6, 26);
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsg });
            statusStrip1.Location = new Point(0, 483);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(931, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(37, 17);
            ErrorMsg.Text = "          ";
            // 
            // TrialBalanceGrid
            // 
            TrialBalanceGrid.AllowUserToAddRows = false;
            TrialBalanceGrid.AllowUserToDeleteRows = false;
            TrialBalanceGrid.AllowUserToResizeColumns = false;
            TrialBalanceGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            TrialBalanceGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            TrialBalanceGrid.ColumnHeadersHeight = 20;
            TrialBalanceGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            TrialBalanceGrid.Columns.AddRange(new DataGridViewColumn[] { AccountName, Debit, Credit, AccountId });
            TrialBalanceGrid.EnableHeadersVisualStyles = false;
            TrialBalanceGrid.Location = new Point(2, 40);
            TrialBalanceGrid.Name = "TrialBalanceGrid";
            TrialBalanceGrid.ReadOnly = true;
            TrialBalanceGrid.RowHeadersVisible = false;
            TrialBalanceGrid.RowHeadersWidth = 51;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.ForeColor = Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = Color.White;
            dataGridViewCellStyle5.SelectionForeColor = Color.Black;
            TrialBalanceGrid.RowsDefaultCellStyle = dataGridViewCellStyle5;
            TrialBalanceGrid.RowTemplate.Height = 20;
            TrialBalanceGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            TrialBalanceGrid.ShowCellToolTips = false;
            TrialBalanceGrid.Size = new Size(926, 395);
            TrialBalanceGrid.TabIndex = 2;
            TrialBalanceGrid.CellBeginEdit += TrialBalanceGrid_CellBeginEdit;
            TrialBalanceGrid.CellClick += TrialBalanceGrid_CellClick;
            TrialBalanceGrid.CellEndEdit += TrialBalanceGrid_CellEndEdit;
            TrialBalanceGrid.CellEnter += TrialBalanceGrid_CellEnter;
            // 
            // AccountName
            // 
            dataGridViewCellStyle2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            AccountName.DefaultCellStyle = dataGridViewCellStyle2;
            AccountName.HeaderText = "Account Name";
            AccountName.MinimumWidth = 6;
            AccountName.Name = "AccountName";
            AccountName.ReadOnly = true;
            AccountName.Resizable = DataGridViewTriState.False;
            AccountName.SortMode = DataGridViewColumnSortMode.NotSortable;
            AccountName.Width = 555;
            // 
            // Debit
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Debit.DefaultCellStyle = dataGridViewCellStyle3;
            Debit.HeaderText = "Debit";
            Debit.MinimumWidth = 6;
            Debit.Name = "Debit";
            Debit.ReadOnly = true;
            Debit.Resizable = DataGridViewTriState.False;
            Debit.SortMode = DataGridViewColumnSortMode.NotSortable;
            Debit.Width = 175;
            // 
            // Credit
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Credit.DefaultCellStyle = dataGridViewCellStyle4;
            Credit.HeaderText = "Credit";
            Credit.MinimumWidth = 6;
            Credit.Name = "Credit";
            Credit.ReadOnly = true;
            Credit.Resizable = DataGridViewTriState.False;
            Credit.SortMode = DataGridViewColumnSortMode.NotSortable;
            Credit.Width = 175;
            // 
            // AccountId
            // 
            AccountId.HeaderText = "Id";
            AccountId.MinimumWidth = 6;
            AccountId.Name = "AccountId";
            AccountId.ReadOnly = true;
            AccountId.Resizable = DataGridViewTriState.False;
            AccountId.SortMode = DataGridViewColumnSortMode.NotSortable;
            AccountId.Visible = false;
            AccountId.Width = 125;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(835, 449);
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
            BtnReset.Location = new Point(585, 449);
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
            BtnPrint.Location = new Point(754, 449);
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
            BtnSave.Location = new Point(673, 449);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 18;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // FormTrialBalance
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(931, 505);
            Controls.Add(BtnExit);
            Controls.Add(BtnReset);
            Controls.Add(BtnPrint);
            Controls.Add(BtnSave);
            Controls.Add(TrialBalanceGrid);
            Controls.Add(statusStrip1);
            Controls.Add(TrialbalanceToolStrip);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormTrialBalance";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Trial Balance";
            Load += FormTrialBalance_Load;
            TrialbalanceToolStrip.ResumeLayout(false);
            TrialbalanceToolStrip.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)TrialBalanceGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private views.controls.Ab2ToolStrip TrialbalanceToolStrip;
        private StatusStrip statusStrip1;
        private views.controls.DataViewVerticalScroll TrialBalanceGrid;
        private ToolStripDropDownButton BtnPeriod;
        private ToolStripMenuItem thisMonthToolStripMenuItem;
        private ToolStripMenuItem thisQuarterToolStripMenuItem;
        private ToolStripMenuItem thisYearToolStripMenuItem;
        private ToolStripMenuItem lastYearToolStripMenuItem;
        private ToolStripMenuItem customToolStripMenuItem;
        private ToolStripLabel toolStripLabel1;
        private views.controls.ToolStripCalendar FromDate;
        private ToolStripLabel toolStripLabel2;
        private views.controls.ToolStripCalendar ToDate;
        private ToolStripButton BtnRunReport;
        private ToolStripLabel LabelCostCenter;
        private ToolStripComboBox ComboBoxCostCenter;
        private ToolStripStatusLabel ErrorMsg;
        private Button BtnExit;
        private Button BtnReset;
        private Button BtnPrint;
        private Button BtnSave;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton ToolStripBtnSave;
        private ToolStripButton ToolStripBtnPrint;
        private ToolStripSeparator toolStripSeparator;
        private DataGridViewTextBoxColumn AccountName;
        private DataGridViewTextBoxColumn Debit;
        private DataGridViewTextBoxColumn Credit;
        private DataGridViewTextBoxColumn AccountId;
    }
}