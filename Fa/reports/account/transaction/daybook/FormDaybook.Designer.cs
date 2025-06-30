namespace fa.reports.account.transaction
{
    partial class FormDaybook
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDaybook));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            ToolStrip = new ToolStrip();
            LabelCostCenter = new ToolStripLabel();
            ComboBoxCostCenter = new ToolStripComboBox();
            toolStripSeparatorCostCenter = new ToolStripSeparator();
            toolStripLabel3 = new ToolStripLabel();
            FromDate = new views.controls.ToolStripCalendar();
            toolStripSeparator4 = new ToolStripSeparator();
            toolStripLabel2 = new ToolStripLabel();
            ToDate = new views.controls.ToolStripCalendar();
            toolStripSeparator1 = new ToolStripSeparator();
            RunReportButton = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            ToolStripBtnSave = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            ToolStripBtnPrint = new ToolStripButton();
            toolStripSeparator5 = new ToolStripSeparator();
            LedgerStatusStrip = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            DaybookDataGridView = new views.controls.DataViewVerticalScroll();
            Date = new DataGridViewTextBoxColumn();
            Description = new DataGridViewTextBoxColumn();
            Debit = new DataGridViewTextBoxColumn();
            CR = new DataGridViewTextBoxColumn();
            ID = new DataGridViewTextBoxColumn();
            BtnExit = new Button();
            BtnReset = new Button();
            BtnPrint = new Button();
            BtnSave = new Button();
            ToolStrip.SuspendLayout();
            LedgerStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DaybookDataGridView).BeginInit();
            SuspendLayout();
            // 
            // ToolStrip
            // 
            ToolStrip.BackColor = SystemColors.ControlLight;
            ToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            ToolStrip.Items.AddRange(new ToolStripItem[] { LabelCostCenter, ComboBoxCostCenter, toolStripSeparatorCostCenter, toolStripLabel3, FromDate, toolStripSeparator4, toolStripLabel2, ToDate, toolStripSeparator1, RunReportButton, toolStripSeparator2, ToolStripBtnSave, toolStripSeparator3, ToolStripBtnPrint, toolStripSeparator5 });
            ToolStrip.Location = new Point(0, 0);
            ToolStrip.Name = "ToolStrip";
            ToolStrip.Padding = new Padding(6);
            ToolStrip.Size = new Size(1089, 40);
            ToolStrip.TabIndex = 1;
            ToolStrip.Text = "toolStrip1";
            ToolStrip.ItemClicked += LedgerFrmToolStrip_ItemClicked;
            // 
            // LabelCostCenter
            // 
            LabelCostCenter.Name = "LabelCostCenter";
            LabelCostCenter.Size = new Size(69, 25);
            LabelCostCenter.Text = "Cost Center";
            // 
            // ComboBoxCostCenter
            // 
            ComboBoxCostCenter.FlatStyle = FlatStyle.Standard;
            ComboBoxCostCenter.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ComboBoxCostCenter.Name = "ComboBoxCostCenter";
            ComboBoxCostCenter.Size = new Size(233, 28);
            // 
            // toolStripSeparatorCostCenter
            // 
            toolStripSeparatorCostCenter.Name = "toolStripSeparatorCostCenter";
            toolStripSeparatorCostCenter.Size = new Size(6, 28);
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new Size(34, 25);
            toolStripLabel3.Text = "From";
            // 
            // FromDate
            // 
            FromDate.BackColor = Color.White;
            FromDate.Date = null;
            FromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FromDate.Format = "MM/dd/yyyy";
            FromDate.MaxDate = new DateTime(9997, 12, 31, 9, 49, 50, 0);
            FromDate.MinDate = new DateTime(1900, 1, 1, 17, 47, 28, 0);
            FromDate.Name = "FromDate";
            FromDate.Size = new Size(97, 25);
            FromDate.Text = "toolStripCalendar1";
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 28);
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(22, 25);
            toolStripLabel2.Text = "To";
            // 
            // ToDate
            // 
            ToDate.BackColor = Color.White;
            ToDate.Date = null;
            ToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ToDate.Format = "MM/dd/yyyy";
            ToDate.MaxDate = new DateTime(9997, 12, 31, 9, 49, 50, 0);
            ToDate.MinDate = new DateTime(1900, 1, 1, 17, 47, 28, 0);
            ToDate.Name = "ToDate";
            ToDate.Size = new Size(97, 25);
            ToDate.Text = "toolStripCalendar1";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 28);
            // 
            // RunReportButton
            // 
            RunReportButton.BackgroundImageLayout = ImageLayout.None;
            RunReportButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            RunReportButton.ImageTransparentColor = Color.Magenta;
            RunReportButton.Name = "RunReportButton";
            RunReportButton.Size = new Size(70, 25);
            RunReportButton.Text = "Run Report";
            RunReportButton.Click += RunReportButton_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 28);
            // 
            // ToolStripBtnSave
            // 
            ToolStripBtnSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnSave.Image = (Image)resources.GetObject("ToolStripBtnSave.Image");
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
            ToolStripBtnPrint.ImageTransparentColor = Color.Black;
            ToolStripBtnPrint.Name = "ToolStripBtnPrint";
            ToolStripBtnPrint.Size = new Size(23, 25);
            ToolStripBtnPrint.Text = "Print";
            ToolStripBtnPrint.Click += BtnPrint_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(6, 28);
            // 
            // LedgerStatusStrip
            // 
            LedgerStatusStrip.Items.AddRange(new ToolStripItem[] { ErrorMsg });
            LedgerStatusStrip.Location = new Point(0, 616);
            LedgerStatusStrip.Name = "LedgerStatusStrip";
            LedgerStatusStrip.Padding = new Padding(1, 0, 16, 0);
            LedgerStatusStrip.Size = new Size(1089, 22);
            LedgerStatusStrip.TabIndex = 2;
            LedgerStatusStrip.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(64, 17);
            ErrorMsg.Text = "                   ";
            // 
            // DaybookDataGridView
            // 
            DaybookDataGridView.AllowUserToAddRows = false;
            DaybookDataGridView.AllowUserToDeleteRows = false;
            DaybookDataGridView.AllowUserToResizeColumns = false;
            DaybookDataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            DaybookDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            DaybookDataGridView.ColumnHeadersHeight = 20;
            DaybookDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DaybookDataGridView.Columns.AddRange(new DataGridViewColumn[] { Date, Description, Debit, CR, ID });
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Window;
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle6.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            DaybookDataGridView.DefaultCellStyle = dataGridViewCellStyle6;
            DaybookDataGridView.EnableHeadersVisualStyles = false;
            DaybookDataGridView.Location = new Point(3, 43);
            DaybookDataGridView.Margin = new Padding(4, 3, 4, 3);
            DaybookDataGridView.Name = "DaybookDataGridView";
            DaybookDataGridView.ReadOnly = true;
            DaybookDataGridView.RowHeadersVisible = false;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            DaybookDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle7;
            DaybookDataGridView.RowTemplate.Height = 20;
            DaybookDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DaybookDataGridView.ShowCellToolTips = false;
            DaybookDataGridView.Size = new Size(1083, 515);
            DaybookDataGridView.TabIndex = 3;
            DaybookDataGridView.CellPainting += DaybookDataGridView_CellPainting;
            // 
            // Date
            // 
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            Date.DefaultCellStyle = dataGridViewCellStyle2;
            Date.HeaderText = "Date";
            Date.Name = "Date";
            Date.ReadOnly = true;
            Date.Resizable = DataGridViewTriState.False;
            Date.SortMode = DataGridViewColumnSortMode.NotSortable;
            Date.Width = 105;
            // 
            // Description
            // 
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.White;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            Description.DefaultCellStyle = dataGridViewCellStyle3;
            Description.HeaderText = "Description";
            Description.Name = "Description";
            Description.ReadOnly = true;
            Description.Resizable = DataGridViewTriState.False;
            Description.SortMode = DataGridViewColumnSortMode.NotSortable;
            Description.Width = 658;
            // 
            // Debit
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = Color.White;
            dataGridViewCellStyle4.SelectionForeColor = Color.Black;
            Debit.DefaultCellStyle = dataGridViewCellStyle4;
            Debit.HeaderText = "Debit";
            Debit.Name = "Debit";
            Debit.ReadOnly = true;
            Debit.Resizable = DataGridViewTriState.False;
            Debit.SortMode = DataGridViewColumnSortMode.NotSortable;
            Debit.Width = 150;
            // 
            // CR
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle5.ForeColor = Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = Color.White;
            dataGridViewCellStyle5.SelectionForeColor = Color.Black;
            CR.DefaultCellStyle = dataGridViewCellStyle5;
            CR.HeaderText = "Credit";
            CR.Name = "CR";
            CR.ReadOnly = true;
            CR.Resizable = DataGridViewTriState.False;
            CR.SortMode = DataGridViewColumnSortMode.NotSortable;
            CR.Width = 150;
            // 
            // ID
            // 
            ID.HeaderText = "ID";
            ID.Name = "ID";
            ID.ReadOnly = true;
            ID.Resizable = DataGridViewTriState.False;
            ID.SortMode = DataGridViewColumnSortMode.NotSortable;
            ID.Visible = false;
            ID.Width = 25;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(988, 573);
            BtnExit.Margin = new Padding(4, 3, 4, 3);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(84, 24);
            BtnExit.TabIndex = 21;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnReset
            // 
            BtnReset.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnReset.Location = new Point(696, 573);
            BtnReset.Margin = new Padding(4, 3, 4, 3);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new Size(96, 24);
            BtnReset.TabIndex = 20;
            BtnReset.Text = "Reset [Esc]";
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Enabled = false;
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(894, 573);
            BtnPrint.Margin = new Padding(4, 3, 4, 3);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(88, 24);
            BtnPrint.TabIndex = 19;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnSave
            // 
            BtnSave.Enabled = false;
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(799, 573);
            BtnSave.Margin = new Padding(4, 3, 4, 3);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(88, 24);
            BtnSave.TabIndex = 18;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // FormDaybook
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1089, 638);
            Controls.Add(BtnExit);
            Controls.Add(BtnReset);
            Controls.Add(BtnPrint);
            Controls.Add(BtnSave);
            Controls.Add(DaybookDataGridView);
            Controls.Add(LedgerStatusStrip);
            Controls.Add(ToolStrip);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormDaybook";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Daybook";
            Load += FormDaybook_Load;
            ToolStrip.ResumeLayout(false);
            ToolStrip.PerformLayout();
            LedgerStatusStrip.ResumeLayout(false);
            LedgerStatusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DaybookDataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip ToolStrip;
        private ToolStripLabel LabelCostCenter;
        private ToolStripLabel toolStripLabel3;
        private views.controls.ToolStripCalendar FromDate;
        private ToolStripLabel toolStripLabel2;
        private views.controls.ToolStripCalendar ToDate;
        private ToolStripButton RunReportButton;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton ToolStripBtnSave;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton ToolStripBtnPrint;
        private ToolStripSeparator toolStripSeparator4;
        private StatusStrip LedgerStatusStrip;
        private ToolStripStatusLabel ErrorMsg;
        private views.controls.DataViewVerticalScroll DaybookDataGridView;
        private Button BtnExit;
        private Button BtnReset;
        private Button BtnPrint;
        private Button BtnSave;
        private ToolStripComboBox ComboBoxCostCenter;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparatorCostCenter;
        private ToolStripSeparator toolStripSeparator5;
        private DataGridViewTextBoxColumn Date;
        private DataGridViewTextBoxColumn Description;
        private DataGridViewTextBoxColumn Debit;
        private DataGridViewTextBoxColumn CR;
        private DataGridViewTextBoxColumn ID;
    }
}