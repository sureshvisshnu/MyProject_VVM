namespace fa.views.account.transactions
{
    partial class FormLedger
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLedger));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            LedgerFrmToolStrip = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            ComboBoxAccounts = new controls.ToolstripCheckedTreeComboBox();
            LabelCostCenter = new ToolStripLabel();
            ComboBoxCostCenter = new ToolStripComboBox();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripLabel3 = new ToolStripLabel();
            LedgerFromDate = new controls.ToolStripCalendar();
            toolStripLabel2 = new ToolStripLabel();
            LedgerToDate = new controls.ToolStripCalendar();
            toolStripSeparator6 = new ToolStripSeparator();
            RunReportButton = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            ToolStripBtnSave = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            ToolStripBtnPrint = new ToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            LedgerStatusStrip = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            LedgerAccountDataGridView = new controls.DataViewVerticalScroll();
            Column1 = new DataGridViewTextBoxColumn();
            Date = new DataGridViewTextBoxColumn();
            Description = new DataGridViewTextBoxColumn();
            CR = new DataGridViewTextBoxColumn();
            Debit = new DataGridViewTextBoxColumn();
            Balance = new DataGridViewTextBoxColumn();
            ID = new DataGridViewTextBoxColumn();
            imageList1 = new ImageList(components);
            BtnExit = new Button();
            BtnReset = new Button();
            BtnPrint = new Button();
            BtnSave = new Button();
            LedgerFrmToolStrip.SuspendLayout();
            LedgerStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)LedgerAccountDataGridView).BeginInit();
            SuspendLayout();
            // 
            // LedgerFrmToolStrip
            // 
            LedgerFrmToolStrip.AutoSize = false;
            LedgerFrmToolStrip.BackColor = SystemColors.ControlLight;
            LedgerFrmToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            LedgerFrmToolStrip.Items.AddRange(new ToolStripItem[] { toolStripLabel1, ComboBoxAccounts, LabelCostCenter, ComboBoxCostCenter, toolStripSeparator1, toolStripLabel3, LedgerFromDate, toolStripLabel2, LedgerToDate, toolStripSeparator6, RunReportButton, toolStripSeparator2, ToolStripBtnSave, toolStripSeparator3, ToolStripBtnPrint, toolStripSeparator4 });
            LedgerFrmToolStrip.Location = new Point(0, 0);
            LedgerFrmToolStrip.Name = "LedgerFrmToolStrip";
            LedgerFrmToolStrip.Padding = new Padding(5);
            LedgerFrmToolStrip.Size = new Size(1106, 39);
            LedgerFrmToolStrip.TabIndex = 0;
            LedgerFrmToolStrip.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(57, 26);
            toolStripLabel1.Text = "Accounts";
            // 
            // ComboBoxAccounts
            // 
            ComboBoxAccounts.AutoSize = false;
            ComboBoxAccounts.Name = "ComboBoxAccounts";
            ComboBoxAccounts.SelectedNode = null;
            ComboBoxAccounts.Size = new Size(220, 26);
            ComboBoxAccounts.NodeClickedEvent += ComboBoxAccounts_NodeClickedEvent;
            ComboBoxAccounts.KeyDown += ComboBoxAccounts_KeyDown;
            // 
            // LabelCostCenter
            // 
            LabelCostCenter.Name = "LabelCostCenter";
            LabelCostCenter.Size = new Size(69, 26);
            LabelCostCenter.Text = "Cost Center";
            // 
            // ComboBoxCostCenter
            // 
            ComboBoxCostCenter.FlatStyle = FlatStyle.Standard;
            ComboBoxCostCenter.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ComboBoxCostCenter.Name = "ComboBoxCostCenter";
            ComboBoxCostCenter.Size = new Size(200, 29);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 29);
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new Size(34, 26);
            toolStripLabel3.Text = "From";
            // 
            // LedgerFromDate
            // 
            LedgerFromDate.BackColor = Color.White;
            LedgerFromDate.Date = null;
            LedgerFromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LedgerFromDate.Format = "MM/dd/yyyy";
            LedgerFromDate.MaxDate = new DateTime(9997, 12, 31, 9, 49, 32, 0);
            LedgerFromDate.MinDate = new DateTime(1900, 1, 1, 18, 16, 54, 0);
            LedgerFromDate.Name = "LedgerFromDate";
            LedgerFromDate.Size = new Size(97, 26);
            LedgerFromDate.Text = "toolStripCalendar1";
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(22, 26);
            toolStripLabel2.Text = "To";
            // 
            // LedgerToDate
            // 
            LedgerToDate.BackColor = Color.White;
            LedgerToDate.Date = null;
            LedgerToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LedgerToDate.Format = "MM/dd/yyyy";
            LedgerToDate.MaxDate = new DateTime(9997, 12, 31, 9, 49, 32, 0);
            LedgerToDate.MinDate = new DateTime(1900, 1, 1, 18, 16, 54, 0);
            LedgerToDate.Name = "LedgerToDate";
            LedgerToDate.Size = new Size(97, 26);
            LedgerToDate.Text = "toolStripCalendar1";
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(6, 29);
            // 
            // RunReportButton
            // 
            RunReportButton.BackgroundImageLayout = ImageLayout.None;
            RunReportButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            RunReportButton.ImageTransparentColor = Color.Magenta;
            RunReportButton.Name = "RunReportButton";
            RunReportButton.Size = new Size(70, 26);
            RunReportButton.Text = "Run Report";
            RunReportButton.Click += RunReportButton_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 29);
            // 
            // ToolStripBtnSave
            // 
            ToolStripBtnSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnSave.Image = (Image)resources.GetObject("ToolStripBtnSave.Image");
            ToolStripBtnSave.ImageTransparentColor = Color.Black;
            ToolStripBtnSave.Name = "ToolStripBtnSave";
            ToolStripBtnSave.Size = new Size(23, 26);
            ToolStripBtnSave.Text = "Save";
            ToolStripBtnSave.Click += BtnSave_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 29);
            // 
            // ToolStripBtnPrint
            // 
            ToolStripBtnPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnPrint.Image = (Image)resources.GetObject("ToolStripBtnPrint.Image");
            ToolStripBtnPrint.ImageTransparentColor = Color.Black;
            ToolStripBtnPrint.Name = "ToolStripBtnPrint";
            ToolStripBtnPrint.Size = new Size(23, 26);
            ToolStripBtnPrint.Text = "Print";
            ToolStripBtnPrint.Click += BtnPrint_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 29);
            // 
            // LedgerStatusStrip
            // 
            LedgerStatusStrip.Items.AddRange(new ToolStripItem[] { ErrorMsg });
            LedgerStatusStrip.Location = new Point(0, 526);
            LedgerStatusStrip.Name = "LedgerStatusStrip";
            LedgerStatusStrip.Size = new Size(1106, 22);
            LedgerStatusStrip.TabIndex = 1;
            LedgerStatusStrip.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(64, 17);
            ErrorMsg.Text = "                   ";
            // 
            // LedgerAccountDataGridView
            // 
            LedgerAccountDataGridView.AllowUserToAddRows = false;
            LedgerAccountDataGridView.AllowUserToDeleteRows = false;
            LedgerAccountDataGridView.AllowUserToResizeColumns = false;
            LedgerAccountDataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            LedgerAccountDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            LedgerAccountDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Control;
            dataGridViewCellStyle2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            LedgerAccountDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            LedgerAccountDataGridView.ColumnHeadersHeight = 20;
            LedgerAccountDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            LedgerAccountDataGridView.Columns.AddRange(new DataGridViewColumn[] { Column1, Date, Description, CR, Debit, Balance, ID });
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = SystemColors.Window;
            dataGridViewCellStyle10.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle10.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.True;
            LedgerAccountDataGridView.DefaultCellStyle = dataGridViewCellStyle10;
            LedgerAccountDataGridView.EnableHeadersVisualStyles = false;
            LedgerAccountDataGridView.Location = new Point(4, 41);
            LedgerAccountDataGridView.Name = "LedgerAccountDataGridView";
            LedgerAccountDataGridView.ReadOnly = true;
            LedgerAccountDataGridView.RowHeadersVisible = false;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.True;
            LedgerAccountDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle11;
            LedgerAccountDataGridView.RowTemplate.Height = 20;
            LedgerAccountDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            LedgerAccountDataGridView.ShowCellToolTips = false;
            LedgerAccountDataGridView.Size = new Size(1098, 441);
            LedgerAccountDataGridView.TabIndex = 2;
            LedgerAccountDataGridView.CellPainting += LedgerAccountDataGridView_CellPainting;
            // 
            // Column1
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.White;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            Column1.DefaultCellStyle = dataGridViewCellStyle3;
            Column1.HeaderText = "...";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 25;
            // 
            // Date
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.ForeColor = Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = Color.White;
            dataGridViewCellStyle4.SelectionForeColor = Color.Black;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            Date.DefaultCellStyle = dataGridViewCellStyle4;
            Date.HeaderText = "Date";
            Date.Name = "Date";
            Date.ReadOnly = true;
            Date.Resizable = DataGridViewTriState.False;
            Date.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Description
            // 
            Description.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.ForeColor = Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = Color.White;
            dataGridViewCellStyle5.SelectionForeColor = Color.Black;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            Description.DefaultCellStyle = dataGridViewCellStyle5;
            Description.HeaderText = "Description";
            Description.Name = "Description";
            Description.ReadOnly = true;
            Description.Resizable = DataGridViewTriState.False;
            Description.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // CR
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle6.BackColor = Color.White;
            dataGridViewCellStyle6.ForeColor = Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = Color.White;
            dataGridViewCellStyle6.SelectionForeColor = Color.Black;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            CR.DefaultCellStyle = dataGridViewCellStyle6;
            CR.HeaderText = "Credit";
            CR.Name = "CR";
            CR.ReadOnly = true;
            CR.Resizable = DataGridViewTriState.False;
            CR.SortMode = DataGridViewColumnSortMode.NotSortable;
            CR.Width = 150;
            // 
            // Debit
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle7.BackColor = Color.White;
            dataGridViewCellStyle7.ForeColor = Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = Color.White;
            dataGridViewCellStyle7.SelectionForeColor = Color.Black;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            Debit.DefaultCellStyle = dataGridViewCellStyle7;
            Debit.HeaderText = "Debit";
            Debit.Name = "Debit";
            Debit.ReadOnly = true;
            Debit.Resizable = DataGridViewTriState.False;
            Debit.SortMode = DataGridViewColumnSortMode.NotSortable;
            Debit.Width = 150;
            // 
            // Balance
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle8.BackColor = Color.White;
            dataGridViewCellStyle8.ForeColor = Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = Color.White;
            dataGridViewCellStyle8.SelectionForeColor = Color.Black;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            Balance.DefaultCellStyle = dataGridViewCellStyle8;
            Balance.HeaderText = "Balance";
            Balance.Name = "Balance";
            Balance.ReadOnly = true;
            Balance.Resizable = DataGridViewTriState.False;
            Balance.SortMode = DataGridViewColumnSortMode.NotSortable;
            Balance.Width = 150;
            // 
            // ID
            // 
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            ID.DefaultCellStyle = dataGridViewCellStyle9;
            ID.HeaderText = "LedgerAccountID";
            ID.Name = "ID";
            ID.ReadOnly = true;
            ID.Resizable = DataGridViewTriState.False;
            ID.SortMode = DataGridViewColumnSortMode.NotSortable;
            ID.Visible = false;
            ID.Width = 25;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth8Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "edit.ico");
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(1022, 493);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(72, 23);
            BtnExit.TabIndex = 17;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnReset
            // 
            BtnReset.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnReset.Location = new Point(772, 493);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new Size(82, 23);
            BtnReset.TabIndex = 16;
            BtnReset.Text = "Reset [Esc]";
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Enabled = false;
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(941, 493);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 23);
            BtnPrint.TabIndex = 15;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnSave
            // 
            BtnSave.Enabled = false;
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(860, 493);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 14;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // FormLedger
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1106, 548);
            Controls.Add(BtnExit);
            Controls.Add(BtnReset);
            Controls.Add(BtnPrint);
            Controls.Add(BtnSave);
            Controls.Add(LedgerStatusStrip);
            Controls.Add(LedgerAccountDataGridView);
            Controls.Add(LedgerFrmToolStrip);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormLedger";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Ledger";
            Load += FormLedger_Load;
            LedgerFrmToolStrip.ResumeLayout(false);
            LedgerFrmToolStrip.PerformLayout();
            LedgerStatusStrip.ResumeLayout(false);
            LedgerStatusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)LedgerAccountDataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip LedgerFrmToolStrip;
        private StatusStrip LedgerStatusStrip;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel toolStripLabel3;
        private ToolStripLabel toolStripLabel2;
        private ToolStripSeparator toolStripSeparator2;
        private fa.views.controls.DataViewVerticalScroll LedgerAccountDataGridView;
        private ToolStripButton ToolStripBtnSave;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton ToolStripBtnPrint;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripButton RunReportButton;
        private ImageList imageList1;
        private controls.ToolStripCalendar LedgerFromDate;
        private controls.ToolStripCalendar LedgerToDate;
        private ToolStripLabel toolStripLabel1;
        private ToolStripLabel LabelCostCenter;
        private Button BtnExit;
        private Button BtnReset;
        private Button BtnPrint;
        private Button BtnSave;
        private ToolStripStatusLabel ErrorMsg;
        private controls.ToolstripCheckedTreeComboBox ComboBoxAccounts;
        private ToolStripComboBox ComboBoxCostCenter;
        private ToolStripSeparator toolStripSeparator6;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Date;
        private DataGridViewTextBoxColumn Description;
        private DataGridViewTextBoxColumn CR;
        private DataGridViewTextBoxColumn Debit;
        private DataGridViewTextBoxColumn Balance;
        private DataGridViewTextBoxColumn ID;
    }
}