namespace Fa.reports.account.transaction
{
    partial class FormProfitLoss
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
            DataGridViewCellStyle dataGridViewCellStyle25 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle28 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProfitLoss));
            DataGridViewCellStyle dataGridViewCellStyle26 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle27 = new DataGridViewCellStyle();
            BtnExit = new Button();
            BtnReset = new Button();
            BtnPrint = new Button();
            BtnSave = new Button();
            ProfitLossGrid = new fa.views.controls.DataViewVerticalScroll();
            TrialbalanceToolStrip = new fa.views.controls.Ab2ToolStrip();
            LabelCostCenter = new ToolStripLabel();
            ComboBoxCostCenter = new ToolStripComboBox();
            BtnPeriod = new ToolStripDropDownButton();
            thisMonthToolStripMenuItem = new ToolStripMenuItem();
            thisQuarterToolStripMenuItem = new ToolStripMenuItem();
            thisYearToolStripMenuItem = new ToolStripMenuItem();
            lastYearToolStripMenuItem = new ToolStripMenuItem();
            customToolStripMenuItem = new ToolStripMenuItem();
            toolStripLabel1 = new ToolStripLabel();
            FromDate = new fa.views.controls.ToolStripCalendar();
            toolStripLabel2 = new ToolStripLabel();
            ToDate = new fa.views.controls.ToolStripCalendar();
            BtnRunReport = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            ToolStripBtnSave = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            ToolStripBtnPrint = new ToolStripButton();
            toolStripSeparator = new ToolStripSeparator();
            statusStrip1 = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            Discription = new DataGridViewTextBoxColumn();
            Amount = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)ProfitLossGrid).BeginInit();
            TrialbalanceToolStrip.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(833, 450);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(72, 23);
            BtnExit.TabIndex = 27;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnReset
            // 
            BtnReset.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnReset.Location = new Point(583, 450);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new Size(82, 23);
            BtnReset.TabIndex = 26;
            BtnReset.Text = "Reset [Esc]";
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Enabled = false;
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(752, 450);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 23);
            BtnPrint.TabIndex = 25;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnSave
            // 
            BtnSave.Enabled = false;
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(671, 450);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 24;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // ProfitLossGrid
            // 
            ProfitLossGrid.AllowUserToAddRows = false;
            ProfitLossGrid.AllowUserToDeleteRows = false;
            ProfitLossGrid.AllowUserToResizeColumns = false;
            ProfitLossGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle25.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle25.BackColor = SystemColors.Control;
            dataGridViewCellStyle25.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle25.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle25.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle25.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle25.WrapMode = DataGridViewTriState.True;
            ProfitLossGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle25;
            ProfitLossGrid.ColumnHeadersHeight = 20;
            ProfitLossGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            ProfitLossGrid.Columns.AddRange(new DataGridViewColumn[] { Discription, Amount });
            ProfitLossGrid.EnableHeadersVisualStyles = false;
            ProfitLossGrid.Location = new Point(2, 41);
            ProfitLossGrid.Name = "ProfitLossGrid";
            ProfitLossGrid.ReadOnly = true;
            ProfitLossGrid.RowHeadersVisible = false;
            ProfitLossGrid.RowHeadersWidth = 51;
            dataGridViewCellStyle28.BackColor = Color.White;
            dataGridViewCellStyle28.ForeColor = Color.Black;
            dataGridViewCellStyle28.SelectionBackColor = Color.White;
            dataGridViewCellStyle28.SelectionForeColor = Color.Black;
            ProfitLossGrid.RowsDefaultCellStyle = dataGridViewCellStyle28;
            ProfitLossGrid.RowTemplate.Height = 20;
            ProfitLossGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ProfitLossGrid.ShowCellToolTips = false;
            ProfitLossGrid.Size = new Size(926, 395);
            ProfitLossGrid.TabIndex = 23;
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
            TrialbalanceToolStrip.Size = new Size(930, 38);
            TrialbalanceToolStrip.TabIndex = 22;
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
            thisMonthToolStripMenuItem.Size = new Size(180, 22);
            thisMonthToolStripMenuItem.Text = "This Month";
            // 
            // thisQuarterToolStripMenuItem
            // 
            thisQuarterToolStripMenuItem.Name = "thisQuarterToolStripMenuItem";
            thisQuarterToolStripMenuItem.Size = new Size(180, 22);
            thisQuarterToolStripMenuItem.Text = "This Quarter";
            // 
            // thisYearToolStripMenuItem
            // 
            thisYearToolStripMenuItem.Name = "thisYearToolStripMenuItem";
            thisYearToolStripMenuItem.Size = new Size(180, 22);
            thisYearToolStripMenuItem.Text = "This Year";
            // 
            // lastYearToolStripMenuItem
            // 
            lastYearToolStripMenuItem.Name = "lastYearToolStripMenuItem";
            lastYearToolStripMenuItem.Size = new Size(180, 22);
            lastYearToolStripMenuItem.Text = "Last Year";
            // 
            // customToolStripMenuItem
            // 
            customToolStripMenuItem.Name = "customToolStripMenuItem";
            customToolStripMenuItem.Size = new Size(180, 22);
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
            statusStrip1.Location = new Point(0, 482);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(930, 22);
            statusStrip1.TabIndex = 28;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(37, 17);
            ErrorMsg.Text = "          ";
            // 
            // Discription
            // 
            dataGridViewCellStyle26.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Discription.DefaultCellStyle = dataGridViewCellStyle26;
            Discription.HeaderText = "Discription";
            Discription.MinimumWidth = 6;
            Discription.Name = "Discription";
            Discription.ReadOnly = true;
            Discription.Resizable = DataGridViewTriState.False;
            Discription.SortMode = DataGridViewColumnSortMode.NotSortable;
            Discription.Width = 650;
            // 
            // Amount
            // 
            dataGridViewCellStyle27.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle27.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Amount.DefaultCellStyle = dataGridViewCellStyle27;
            Amount.HeaderText = "Amount";
            Amount.MinimumWidth = 6;
            Amount.Name = "Amount";
            Amount.ReadOnly = true;
            Amount.Resizable = DataGridViewTriState.False;
            Amount.SortMode = DataGridViewColumnSortMode.NotSortable;
            Amount.Width = 250;
            // 
            // FormProfitLoss
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(930, 504);
            Controls.Add(statusStrip1);
            Controls.Add(BtnExit);
            Controls.Add(BtnReset);
            Controls.Add(BtnPrint);
            Controls.Add(BtnSave);
            Controls.Add(ProfitLossGrid);
            Controls.Add(TrialbalanceToolStrip);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormProfitLoss";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Profit Loss";
            Load += FormProfitLoss_Load;
            ((System.ComponentModel.ISupportInitialize)ProfitLossGrid).EndInit();
            TrialbalanceToolStrip.ResumeLayout(false);
            TrialbalanceToolStrip.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnExit;
        private Button BtnReset;
        private Button BtnPrint;
        private Button BtnSave;
        private fa.views.controls.DataViewVerticalScroll ProfitLossGrid;
        private fa.views.controls.Ab2ToolStrip TrialbalanceToolStrip;
        private ToolStripLabel LabelCostCenter;
        private ToolStripComboBox ComboBoxCostCenter;
        private ToolStripDropDownButton BtnPeriod;
        private ToolStripMenuItem thisMonthToolStripMenuItem;
        private ToolStripMenuItem thisQuarterToolStripMenuItem;
        private ToolStripMenuItem thisYearToolStripMenuItem;
        private ToolStripMenuItem lastYearToolStripMenuItem;
        private ToolStripMenuItem customToolStripMenuItem;
        private ToolStripLabel toolStripLabel1;
        private fa.views.controls.ToolStripCalendar FromDate;
        private ToolStripLabel toolStripLabel2;
        private fa.views.controls.ToolStripCalendar ToDate;
        private ToolStripButton BtnRunReport;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton ToolStripBtnSave;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton ToolStripBtnPrint;
        private ToolStripSeparator toolStripSeparator;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel ErrorMsg;
        private DataGridViewTextBoxColumn Discription;
        private DataGridViewTextBoxColumn Amount;
    }
}