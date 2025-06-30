namespace fa.reports.Hms
{
    partial class FormPatientLedger
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPatientLedger));
            BtnExit = new Button();
            BtnReset = new Button();
            BtnPrint = new Button();
            BtnSave = new Button();
            PatientLedgerStatusStrip = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            PatientLedgerDataGridView = new views.controls.DataViewVerticalScroll();
            Column1 = new DataGridViewTextBoxColumn();
            Date = new DataGridViewTextBoxColumn();
            Description = new DataGridViewTextBoxColumn();
            Column8 = new DataGridViewTextBoxColumn();
            FeeCharged = new DataGridViewTextBoxColumn();
            Debit = new DataGridViewTextBoxColumn();
            Balance = new DataGridViewTextBoxColumn();
            ID = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewButtonColumn();
            Column5 = new DataGridViewCheckBoxColumn();
            Column3 = new DataGridViewCheckBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            Column7 = new DataGridViewTextBoxColumn();
            Column9 = new DataGridViewTextBoxColumn();
            Column10 = new DataGridViewCheckBoxColumn();
            LedgerFrmToolStrip = new ToolStrip();
            LabelCostCenter = new ToolStripLabel();
            TextBoxPatientSearch = new ToolStripTextBox();
            BtnPatientSearch = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripLabel3 = new ToolStripLabel();
            PatientLedgerFromDate = new views.controls.ToolStripCalendar();
            toolStripLabel2 = new ToolStripLabel();
            PatientLedgerToDate = new views.controls.ToolStripCalendar();
            toolStripSeparator5 = new ToolStripSeparator();
            CheckBoxLoadAllTransaction = new views.controls.ToolStripCheckBox();
            toolStripSeparator6 = new ToolStripSeparator();
            RunReportButton = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            ToolStripBtnSave = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            ToolStripBtnPrint = new ToolStripButton();
            ImageListReceiveAmount = new ImageList(components);
            BtnLineItem = new Button();
            PatientLedgerStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PatientLedgerDataGridView).BeginInit();
            LedgerFrmToolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // PatientIdTransport
            // 
            PatientIdTransport.Size = new Size(100, 21);
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
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(1015, 493);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(72, 23);
            BtnExit.TabIndex = 24;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnReset
            // 
            BtnReset.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnReset.Location = new Point(765, 493);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new Size(82, 23);
            BtnReset.TabIndex = 23;
            BtnReset.Text = "Reset [Esc]";
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Enabled = false;
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(934, 493);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 23);
            BtnPrint.TabIndex = 22;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnSave
            // 
            BtnSave.Enabled = false;
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(853, 493);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 21;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // PatientLedgerStatusStrip
            // 
            PatientLedgerStatusStrip.Items.AddRange(new ToolStripItem[] { ErrorMsg });
            PatientLedgerStatusStrip.Location = new Point(0, 526);
            PatientLedgerStatusStrip.Name = "PatientLedgerStatusStrip";
            PatientLedgerStatusStrip.Size = new Size(1113, 22);
            PatientLedgerStatusStrip.TabIndex = 19;
            PatientLedgerStatusStrip.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(64, 17);
            ErrorMsg.Text = "                   ";
            // 
            // PatientLedgerDataGridView
            // 
            PatientLedgerDataGridView.AllowUserToAddRows = false;
            PatientLedgerDataGridView.AllowUserToDeleteRows = false;
            PatientLedgerDataGridView.AllowUserToResizeColumns = false;
            PatientLedgerDataGridView.AllowUserToResizeRows = false;
            PatientLedgerDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            PatientLedgerDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            PatientLedgerDataGridView.ColumnHeadersHeight = 20;
            PatientLedgerDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            PatientLedgerDataGridView.Columns.AddRange(new DataGridViewColumn[] { Column1, Date, Description, Column8, FeeCharged, Debit, Balance, ID, Column2, Column5, Column3, Column4, Column6, Column7, Column9, Column10 });
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = SystemColors.Window;
            dataGridViewCellStyle10.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle10.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.False;
            PatientLedgerDataGridView.DefaultCellStyle = dataGridViewCellStyle10;
            PatientLedgerDataGridView.EnableHeadersVisualStyles = false;
            PatientLedgerDataGridView.Location = new Point(4, 41);
            PatientLedgerDataGridView.Name = "PatientLedgerDataGridView";
            PatientLedgerDataGridView.ReadOnly = true;
            PatientLedgerDataGridView.RowHeadersVisible = false;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.True;
            PatientLedgerDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle11;
            PatientLedgerDataGridView.RowTemplate.Height = 20;
            PatientLedgerDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            PatientLedgerDataGridView.ShowCellToolTips = false;
            PatientLedgerDataGridView.Size = new Size(1097, 441);
            PatientLedgerDataGridView.TabIndex = 20;
            PatientLedgerDataGridView.CellClick += PatientLedgerDataGridView_CellClick;
            PatientLedgerDataGridView.DataError += PatientLedgerDataGridView_DataError;
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            Column1.DefaultCellStyle = dataGridViewCellStyle2;
            Column1.HeaderText = "...";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 25;
            // 
            // Date
            // 
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.White;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            Date.DefaultCellStyle = dataGridViewCellStyle3;
            Date.HeaderText = "Date";
            Date.Name = "Date";
            Date.ReadOnly = true;
            Date.Resizable = DataGridViewTriState.False;
            Date.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Description
            // 
            Description.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.ForeColor = Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = Color.White;
            dataGridViewCellStyle4.SelectionForeColor = Color.Black;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            Description.DefaultCellStyle = dataGridViewCellStyle4;
            Description.HeaderText = "Description";
            Description.Name = "Description";
            Description.ReadOnly = true;
            Description.Resizable = DataGridViewTriState.False;
            Description.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column8
            // 
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.ForeColor = Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = Color.White;
            dataGridViewCellStyle5.SelectionForeColor = Color.Black;
            Column8.DefaultCellStyle = dataGridViewCellStyle5;
            Column8.HeaderText = "OP/IP";
            Column8.Name = "Column8";
            Column8.ReadOnly = true;
            Column8.Resizable = DataGridViewTriState.False;
            Column8.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column8.Width = 50;
            // 
            // FeeCharged
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.BackColor = Color.White;
            dataGridViewCellStyle6.ForeColor = Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = Color.White;
            dataGridViewCellStyle6.SelectionForeColor = Color.Black;
            FeeCharged.DefaultCellStyle = dataGridViewCellStyle6;
            FeeCharged.HeaderText = "Debit";
            FeeCharged.Name = "FeeCharged";
            FeeCharged.ReadOnly = true;
            FeeCharged.Resizable = DataGridViewTriState.False;
            FeeCharged.SortMode = DataGridViewColumnSortMode.NotSortable;
            FeeCharged.Width = 125;
            // 
            // Debit
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle7.BackColor = Color.White;
            dataGridViewCellStyle7.ForeColor = Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = Color.White;
            dataGridViewCellStyle7.SelectionForeColor = Color.Black;
            Debit.DefaultCellStyle = dataGridViewCellStyle7;
            Debit.HeaderText = "Credit";
            Debit.Name = "Debit";
            Debit.ReadOnly = true;
            Debit.Resizable = DataGridViewTriState.False;
            Debit.SortMode = DataGridViewColumnSortMode.NotSortable;
            Debit.Width = 125;
            // 
            // Balance
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle8.BackColor = Color.White;
            dataGridViewCellStyle8.ForeColor = Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = Color.White;
            dataGridViewCellStyle8.SelectionForeColor = Color.Black;
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
            ID.HeaderText = "LedgerPatientID";
            ID.Name = "ID";
            ID.ReadOnly = true;
            ID.Resizable = DataGridViewTriState.False;
            ID.SortMode = DataGridViewColumnSortMode.NotSortable;
            ID.Visible = false;
            ID.Width = 25;
            // 
            // Column2
            // 
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.ForeColor = Color.Black;
            dataGridViewCellStyle9.SelectionBackColor = Color.White;
            dataGridViewCellStyle9.SelectionForeColor = Color.Black;
            Column2.DefaultCellStyle = dataGridViewCellStyle9;
            Column2.HeaderText = "...";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Resizable = DataGridViewTriState.False;
            Column2.Width = 50;
            // 
            // Column5
            // 
            Column5.HeaderText = "IsCreateInvoice";
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            Column5.Resizable = DataGridViewTriState.False;
            Column5.Visible = false;
            // 
            // Column3
            // 
            Column3.HeaderText = "IsPrintInvoice";
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            Column3.Resizable = DataGridViewTriState.False;
            Column3.Visible = false;
            // 
            // Column4
            // 
            Column4.HeaderText = "InvId";
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            Column4.Resizable = DataGridViewTriState.False;
            Column4.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column4.Visible = false;
            // 
            // Column6
            // 
            Column6.HeaderText = "LedgDate";
            Column6.Name = "Column6";
            Column6.ReadOnly = true;
            Column6.Resizable = DataGridViewTriState.False;
            Column6.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column6.Visible = false;
            // 
            // Column7
            // 
            Column7.HeaderText = "OpId";
            Column7.Name = "Column7";
            Column7.ReadOnly = true;
            Column7.Resizable = DataGridViewTriState.False;
            Column7.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column7.Visible = false;
            // 
            // Column9
            // 
            Column9.HeaderText = "LedgId";
            Column9.Name = "Column9";
            Column9.ReadOnly = true;
            Column9.Resizable = DataGridViewTriState.False;
            Column9.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column9.Visible = false;
            // 
            // Column10
            // 
            Column10.HeaderText = "IsDeleteInvoice";
            Column10.Name = "Column10";
            Column10.ReadOnly = true;
            Column10.Resizable = DataGridViewTriState.False;
            Column10.Visible = false;
            // 
            // LedgerFrmToolStrip
            // 
            LedgerFrmToolStrip.BackColor = SystemColors.ControlLight;
            LedgerFrmToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            LedgerFrmToolStrip.Items.AddRange(new ToolStripItem[] { LabelCostCenter, TextBoxPatientSearch, BtnPatientSearch, toolStripSeparator1, toolStripLabel3, PatientLedgerFromDate, toolStripLabel2, PatientLedgerToDate, toolStripSeparator5, CheckBoxLoadAllTransaction, toolStripSeparator6, RunReportButton, toolStripSeparator2, ToolStripBtnSave, toolStripSeparator3, ToolStripBtnPrint });
            LedgerFrmToolStrip.Location = new Point(0, 0);
            LedgerFrmToolStrip.Name = "LedgerFrmToolStrip";
            LedgerFrmToolStrip.Padding = new Padding(5);
            LedgerFrmToolStrip.Size = new Size(1113, 42);
            LedgerFrmToolStrip.TabIndex = 18;
            LedgerFrmToolStrip.Text = "toolStrip1";
            // 
            // LabelCostCenter
            // 
            LabelCostCenter.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            LabelCostCenter.Name = "LabelCostCenter";
            LabelCostCenter.Size = new Size(46, 29);
            LabelCostCenter.Text = "Patient";
            // 
            // TextBoxPatientSearch
            // 
            TextBoxPatientSearch.BackColor = Color.White;
            TextBoxPatientSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxPatientSearch.Name = "TextBoxPatientSearch";
            TextBoxPatientSearch.Size = new Size(200, 32);
            TextBoxPatientSearch.KeyDown += TextBoxPatientSearch_KeyDown;
            // 
            // BtnPatientSearch
            // 
            BtnPatientSearch.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnPatientSearch.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            BtnPatientSearch.Image = (Image)resources.GetObject("BtnPatientSearch.Image");
            BtnPatientSearch.ImageTransparentColor = Color.Magenta;
            BtnPatientSearch.Name = "BtnPatientSearch";
            BtnPatientSearch.Size = new Size(118, 29);
            BtnPatientSearch.Text = "Patient Search [F2]";
            BtnPatientSearch.Click += BtnPatientSearch_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 32);
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new Size(46, 29);
            toolStripLabel3.Text = "From : ";
            // 
            // PatientLedgerFromDate
            // 
            PatientLedgerFromDate.BackColor = Color.White;
            PatientLedgerFromDate.Date = null;
            PatientLedgerFromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            PatientLedgerFromDate.Format = "MM/dd/yyyy";
            PatientLedgerFromDate.MaxDate = new DateTime(9997, 12, 31, 9, 21, 58, 0);
            PatientLedgerFromDate.MinDate = new DateTime(1900, 1, 1, 23, 3, 7, 0);
            PatientLedgerFromDate.Name = "PatientLedgerFromDate";
            PatientLedgerFromDate.Size = new Size(97, 29);
            PatientLedgerFromDate.Text = "toolStripCalendar1";
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(30, 29);
            toolStripLabel2.Text = "To :";
            // 
            // PatientLedgerToDate
            // 
            PatientLedgerToDate.BackColor = Color.White;
            PatientLedgerToDate.Date = null;
            PatientLedgerToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            PatientLedgerToDate.Format = "MM/dd/yyyy";
            PatientLedgerToDate.MaxDate = new DateTime(9997, 12, 31, 9, 21, 58, 0);
            PatientLedgerToDate.MinDate = new DateTime(1900, 1, 1, 23, 3, 7, 0);
            PatientLedgerToDate.Name = "PatientLedgerToDate";
            PatientLedgerToDate.Size = new Size(97, 29);
            PatientLedgerToDate.Text = "toolStripCalendar1";
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(6, 32);
            // 
            // CheckBoxLoadAllTransaction
            // 
            CheckBoxLoadAllTransaction.BackColor = SystemColors.ControlLight;
            CheckBoxLoadAllTransaction.Checked = false;
            CheckBoxLoadAllTransaction.CheckState = CheckState.Unchecked;
            CheckBoxLoadAllTransaction.Name = "CheckBoxLoadAllTransaction";
            CheckBoxLoadAllTransaction.Padding = new Padding(5);
            CheckBoxLoadAllTransaction.Size = new Size(147, 29);
            CheckBoxLoadAllTransaction.Text = "Load All Transactions";
            CheckBoxLoadAllTransaction.CheckedChanged += CheckBoxLoadAllTransaction_CheckedChanged;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(6, 32);
            // 
            // RunReportButton
            // 
            RunReportButton.BackgroundImageLayout = ImageLayout.None;
            RunReportButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            RunReportButton.ImageTransparentColor = Color.Magenta;
            RunReportButton.Name = "RunReportButton";
            RunReportButton.Size = new Size(70, 29);
            RunReportButton.Text = "Run Report";
            RunReportButton.Click += RunReportButton_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 32);
            // 
            // ToolStripBtnSave
            // 
            ToolStripBtnSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnSave.Image = (Image)resources.GetObject("ToolStripBtnSave.Image");
            ToolStripBtnSave.ImageTransparentColor = Color.Black;
            ToolStripBtnSave.Name = "ToolStripBtnSave";
            ToolStripBtnSave.Size = new Size(23, 29);
            ToolStripBtnSave.Text = "Save";
            ToolStripBtnSave.Click += BtnSave_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 32);
            // 
            // ToolStripBtnPrint
            // 
            ToolStripBtnPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnPrint.Image = (Image)resources.GetObject("ToolStripBtnPrint.Image");
            ToolStripBtnPrint.ImageTransparentColor = Color.Black;
            ToolStripBtnPrint.Name = "ToolStripBtnPrint";
            ToolStripBtnPrint.Size = new Size(23, 29);
            ToolStripBtnPrint.Text = "Print";
            ToolStripBtnPrint.Click += BtnPrint_Click;
            // 
            // ImageListReceiveAmount
            // 
            ImageListReceiveAmount.ColorDepth = ColorDepth.Depth8Bit;
            ImageListReceiveAmount.ImageStream = (ImageListStreamer)resources.GetObject("ImageListReceiveAmount.ImageStream");
            ImageListReceiveAmount.TransparentColor = Color.Transparent;
            ImageListReceiveAmount.Images.SetKeyName(0, "print.ico");
            // 
            // BtnLineItem
            // 
            BtnLineItem.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnLineItem.Location = new Point(12, 493);
            BtnLineItem.Name = "BtnLineItem";
            BtnLineItem.Size = new Size(130, 23);
            BtnLineItem.TabIndex = 67;
            BtnLineItem.Text = "Manage Line Items";
            BtnLineItem.UseVisualStyleBackColor = true;
            BtnLineItem.Click += BtnLineItem_Click;
            // 
            // FormPatientLedger
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1113, 548);
            Controls.Add(BtnLineItem);
            Controls.Add(BtnExit);
            Controls.Add(BtnReset);
            Controls.Add(BtnPrint);
            Controls.Add(BtnSave);
            Controls.Add(PatientLedgerStatusStrip);
            Controls.Add(PatientLedgerDataGridView);
            Controls.Add(LedgerFrmToolStrip);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormPatientLedger";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Patient Ledger";
            Load += FormPatientLedger_Load;
            Controls.SetChildIndex(LedgerFrmToolStrip, 0);
            Controls.SetChildIndex(PatientLedgerDataGridView, 0);
            Controls.SetChildIndex(PatientLedgerStatusStrip, 0);
            Controls.SetChildIndex(BtnSave, 0);
            Controls.SetChildIndex(BtnPrint, 0);
            Controls.SetChildIndex(BtnReset, 0);
            Controls.SetChildIndex(BtnExit, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(BtnLineItem, 0);
            PatientLedgerStatusStrip.ResumeLayout(false);
            PatientLedgerStatusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PatientLedgerDataGridView).EndInit();
            LedgerFrmToolStrip.ResumeLayout(false);
            LedgerFrmToolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnExit;
        private Button BtnReset;
        private Button BtnPrint;
        private Button BtnSave;
        private StatusStrip PatientLedgerStatusStrip;
        private ToolStripStatusLabel ErrorMsg;
        private views.controls.DataViewVerticalScroll PatientLedgerDataGridView;
        private ToolStrip LedgerFrmToolStrip;
        private ToolStripLabel LabelCostCenter;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel toolStripLabel3;
        private views.controls.ToolStripCalendar PatientLedgerFromDate;
        private ToolStripLabel toolStripLabel2;
        private views.controls.ToolStripCalendar PatientLedgerToDate;
        private ToolStripButton RunReportButton;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton ToolStripBtnSave;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton ToolStripBtnPrint;
        private ToolStripTextBox TextBoxPatientSearch;
        private ToolStripButton BtnPatientSearch;
        private views.controls.ToolStripCheckBox CheckBoxLoadAllTransaction;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator6;
        private ImageList ImageListReceiveAmount;
        private Button BtnLineItem;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Date;
        private DataGridViewTextBoxColumn Description;
        private DataGridViewTextBoxColumn Column8;
        private DataGridViewTextBoxColumn FeeCharged;
        private DataGridViewTextBoxColumn Debit;
        private DataGridViewTextBoxColumn Balance;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewButtonColumn Column2;
        private DataGridViewCheckBoxColumn Column5;
        private DataGridViewCheckBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column6;
        private DataGridViewTextBoxColumn Column7;
        private DataGridViewTextBoxColumn Column9;
        private DataGridViewCheckBoxColumn Column10;
    }
}