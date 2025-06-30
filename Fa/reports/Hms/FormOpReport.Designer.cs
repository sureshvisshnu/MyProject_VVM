namespace fa.reports.Hms
{
    partial class FormOpReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOpReport));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            ToolStrip = new ToolStrip();
            ToolStripLabelPatientTransferType = new ToolStripLabel();
            ComboBoxTypeSelection = new ToolStripComboBox();
            PatientVisittoolStripSeparator1 = new ToolStripSeparator();
            toolStripLabel3 = new ToolStripLabel();
            FromDate = new views.controls.ToolStripCalendar();
            toolStripLabel2 = new ToolStripLabel();
            ToDate = new views.controls.ToolStripCalendar();
            RunReportButton = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            ToolStripBtnSave = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            ToolStripBtnPrint = new ToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            GridviewOpReport = new views.controls.DataViewVerticalScroll();
            Sno = new DataGridViewTextBoxColumn();
            Date = new DataGridViewTextBoxColumn();
            Patient = new DataGridViewTextBoxColumn();
            PatientAge = new DataGridViewTextBoxColumn();
            PatientGender = new DataGridViewTextBoxColumn();
            PatientNumber = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Fee = new views.controls.grid.DataGridViewCurrencyColumn();
            BtnExit = new Button();
            BtnPrint = new Button();
            LedgerStatusStrip = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            BtnReset = new Button();
            BtnSave = new Button();
            ToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridviewOpReport).BeginInit();
            LedgerStatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // ToolStrip
            // 
            ToolStrip.AutoSize = false;
            ToolStrip.BackColor = SystemColors.ControlLight;
            ToolStrip.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            ToolStrip.Items.AddRange(new ToolStripItem[] { ToolStripLabelPatientTransferType, ComboBoxTypeSelection, PatientVisittoolStripSeparator1, toolStripLabel3, FromDate, toolStripLabel2, ToDate, RunReportButton, toolStripSeparator2, ToolStripBtnSave, toolStripSeparator3, ToolStripBtnPrint, toolStripSeparator4 });
            ToolStrip.Location = new Point(0, 0);
            ToolStrip.Name = "ToolStrip";
            ToolStrip.Padding = new Padding(5);
            ToolStrip.Size = new Size(1147, 30);
            ToolStrip.TabIndex = 2;
            ToolStrip.Text = "toolStrip1";
            // 
            // ToolStripLabelPatientTransferType
            // 
            ToolStripLabelPatientTransferType.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ToolStripLabelPatientTransferType.Name = "ToolStripLabelPatientTransferType";
            ToolStripLabelPatientTransferType.Size = new Size(31, 17);
            ToolStripLabelPatientTransferType.Text = "Type";
            // 
            // ComboBoxTypeSelection
            // 
            ComboBoxTypeSelection.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxTypeSelection.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxTypeSelection.AutoSize = false;
            ComboBoxTypeSelection.FlatStyle = FlatStyle.Standard;
            ComboBoxTypeSelection.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ComboBoxTypeSelection.Items.AddRange(new object[] { "By All Patient", "By New Patient", "By Repated Patient" });
            ComboBoxTypeSelection.Name = "ComboBoxTypeSelection";
            ComboBoxTypeSelection.Size = new Size(150, 21);
            ComboBoxTypeSelection.SelectedIndexChanged += ComboBoxTypeSelection_SelectedIndexChanged;
            // 
            // PatientVisittoolStripSeparator1
            // 
            PatientVisittoolStripSeparator1.Name = "PatientVisittoolStripSeparator1";
            PatientVisittoolStripSeparator1.Size = new Size(6, 20);
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new Size(38, 17);
            toolStripLabel3.Text = "From ";
            // 
            // FromDate
            // 
            FromDate.BackColor = Color.White;
            FromDate.Date = null;
            FromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FromDate.Format = "MM/dd/yyyy";
            FromDate.MaxDate = new DateTime(9997, 12, 31, 9, 28, 28, 0);
            FromDate.MinDate = new DateTime(1900, 1, 1, 21, 15, 25, 0);
            FromDate.Name = "FromDate";
            FromDate.Size = new Size(97, 17);
            FromDate.Text = "toolStripCalendar1";
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(22, 17);
            toolStripLabel2.Text = "To";
            // 
            // ToDate
            // 
            ToDate.BackColor = Color.White;
            ToDate.Date = null;
            ToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ToDate.Format = "MM/dd/yyyy";
            ToDate.MaxDate = new DateTime(9997, 12, 31, 9, 28, 28, 0);
            ToDate.MinDate = new DateTime(1900, 1, 1, 21, 15, 25, 0);
            ToDate.Name = "ToDate";
            ToDate.Size = new Size(97, 17);
            ToDate.Text = "toolStripCalendar1";
            // 
            // RunReportButton
            // 
            RunReportButton.BackgroundImageLayout = ImageLayout.None;
            RunReportButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            RunReportButton.ImageTransparentColor = Color.Magenta;
            RunReportButton.Name = "RunReportButton";
            RunReportButton.Size = new Size(26, 17);
            RunReportButton.Text = "Go";
            RunReportButton.Click += RunReportButton_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 20);
            // 
            // ToolStripBtnSave
            // 
            ToolStripBtnSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnSave.Image = (Image)resources.GetObject("ToolStripBtnSave.Image");
            ToolStripBtnSave.ImageTransparentColor = Color.Black;
            ToolStripBtnSave.Name = "ToolStripBtnSave";
            ToolStripBtnSave.Size = new Size(23, 17);
            ToolStripBtnSave.Text = "Save";
            ToolStripBtnSave.Click += BtnSave_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 20);
            // 
            // ToolStripBtnPrint
            // 
            ToolStripBtnPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnPrint.Image = (Image)resources.GetObject("ToolStripBtnPrint.Image");
            ToolStripBtnPrint.ImageTransparentColor = Color.Black;
            ToolStripBtnPrint.Name = "ToolStripBtnPrint";
            ToolStripBtnPrint.Size = new Size(23, 17);
            ToolStripBtnPrint.Text = "Print";
            ToolStripBtnPrint.Click += BtnPrint_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 20);
            // 
            // GridviewOpReport
            // 
            GridviewOpReport.AllowUserToAddRows = false;
            GridviewOpReport.AllowUserToDeleteRows = false;
            GridviewOpReport.AllowUserToResizeColumns = false;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridviewOpReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridviewOpReport.ColumnHeadersHeight = 20;
            GridviewOpReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridviewOpReport.Columns.AddRange(new DataGridViewColumn[] { Sno, Date, Patient, PatientAge, PatientGender, PatientNumber, Column1, Column2, Fee });
            GridviewOpReport.EnableHeadersVisualStyles = false;
            GridviewOpReport.Location = new Point(8, 37);
            GridviewOpReport.Name = "GridviewOpReport";
            GridviewOpReport.ReadOnly = true;
            dataGridViewCellStyle11.BackColor = SystemColors.Control;
            dataGridViewCellStyle11.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle11.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.True;
            GridviewOpReport.RowHeadersDefaultCellStyle = dataGridViewCellStyle11;
            GridviewOpReport.RowHeadersVisible = false;
            dataGridViewCellStyle12.BackColor = Color.White;
            dataGridViewCellStyle12.ForeColor = Color.Black;
            dataGridViewCellStyle12.SelectionBackColor = Color.White;
            dataGridViewCellStyle12.SelectionForeColor = Color.Black;
            GridviewOpReport.RowsDefaultCellStyle = dataGridViewCellStyle12;
            GridviewOpReport.RowTemplate.Height = 20;
            GridviewOpReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridviewOpReport.ShowCellToolTips = false;
            GridviewOpReport.Size = new Size(1132, 478);
            GridviewOpReport.TabIndex = 3;
            GridviewOpReport.CellPainting += GridviewOpReport_CellPainting;
            // 
            // Sno
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            Sno.DefaultCellStyle = dataGridViewCellStyle2;
            Sno.HeaderText = "Sno";
            Sno.Name = "Sno";
            Sno.ReadOnly = true;
            Sno.SortMode = DataGridViewColumnSortMode.NotSortable;
            Sno.Width = 65;
            // 
            // Date
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            Date.DefaultCellStyle = dataGridViewCellStyle3;
            Date.HeaderText = "Date";
            Date.Name = "Date";
            Date.ReadOnly = true;
            Date.Resizable = DataGridViewTriState.False;
            Date.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Patient
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            Patient.DefaultCellStyle = dataGridViewCellStyle4;
            Patient.HeaderText = "Patient Details";
            Patient.Name = "Patient";
            Patient.ReadOnly = true;
            Patient.Resizable = DataGridViewTriState.False;
            Patient.SortMode = DataGridViewColumnSortMode.NotSortable;
            Patient.Width = 335;
            // 
            // PatientAge
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.TopRight;
            PatientAge.DefaultCellStyle = dataGridViewCellStyle5;
            PatientAge.HeaderText = "Age";
            PatientAge.Name = "PatientAge";
            PatientAge.ReadOnly = true;
            PatientAge.SortMode = DataGridViewColumnSortMode.NotSortable;
            PatientAge.Width = 75;
            // 
            // PatientGender
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.TopLeft;
            PatientGender.DefaultCellStyle = dataGridViewCellStyle6;
            PatientGender.HeaderText = "Gender";
            PatientGender.Name = "PatientGender";
            PatientGender.ReadOnly = true;
            PatientGender.SortMode = DataGridViewColumnSortMode.NotSortable;
            PatientGender.Width = 75;
            // 
            // PatientNumber
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            PatientNumber.DefaultCellStyle = dataGridViewCellStyle7;
            PatientNumber.HeaderText = "PatientNumber";
            PatientNumber.Name = "PatientNumber";
            PatientNumber.ReadOnly = true;
            PatientNumber.Resizable = DataGridViewTriState.False;
            PatientNumber.SortMode = DataGridViewColumnSortMode.NotSortable;
            PatientNumber.Width = 130;
            // 
            // Column1
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            Column1.DefaultCellStyle = dataGridViewCellStyle8;
            Column1.HeaderText = "Token No";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 70;
            // 
            // Column2
            // 
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            Column2.DefaultCellStyle = dataGridViewCellStyle9;
            Column2.HeaderText = "Requested Doctor";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Resizable = DataGridViewTriState.False;
            Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column2.Width = 160;
            // 
            // Fee
            // 
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.True;
            Fee.DefaultCellStyle = dataGridViewCellStyle10;
            Fee.HeaderText = "Fee";
            Fee.Name = "Fee";
            Fee.ReadOnly = true;
            Fee.Resizable = DataGridViewTriState.False;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(1039, 525);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(72, 23);
            BtnExit.TabIndex = 23;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Enabled = false;
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(958, 525);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 23);
            BtnPrint.TabIndex = 22;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // LedgerStatusStrip
            // 
            LedgerStatusStrip.Items.AddRange(new ToolStripItem[] { ErrorMsg });
            LedgerStatusStrip.Location = new Point(0, 561);
            LedgerStatusStrip.Name = "LedgerStatusStrip";
            LedgerStatusStrip.Size = new Size(1147, 22);
            LedgerStatusStrip.TabIndex = 24;
            LedgerStatusStrip.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(64, 17);
            ErrorMsg.Text = "                   ";
            // 
            // BtnReset
            // 
            BtnReset.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnReset.Location = new Point(789, 525);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new Size(82, 23);
            BtnReset.TabIndex = 25;
            BtnReset.Text = "Reset [Esc]";
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // BtnSave
            // 
            BtnSave.Enabled = false;
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(877, 525);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 26;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // FormOpReport
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1147, 583);
            Controls.Add(BtnSave);
            Controls.Add(BtnReset);
            Controls.Add(LedgerStatusStrip);
            Controls.Add(BtnExit);
            Controls.Add(BtnPrint);
            Controls.Add(GridviewOpReport);
            Controls.Add(ToolStrip);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormOpReport";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Out Patient Report";
            Load += FormOpReport_Load;
            ToolStrip.ResumeLayout(false);
            ToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridviewOpReport).EndInit();
            LedgerStatusStrip.ResumeLayout(false);
            LedgerStatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip ToolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private views.controls.ToolStripCalendar FromDate;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private views.controls.ToolStripCalendar ToDate;
        private System.Windows.Forms.ToolStripButton RunReportButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton ToolStripBtnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton ToolStripBtnPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private views.controls.DataViewVerticalScroll GridviewOpReport;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Button BtnPrint;
        private System.Windows.Forms.StatusStrip LedgerStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel ErrorMsg;
        private System.Windows.Forms.Button BtnReset;
        private System.Windows.Forms.Button BtnSave;
        private views.controls.grid.DataGridViewCurrencyColumn Column3;
        private ToolStripLabel ToolStripLabelPatientTransferType;
        private ToolStripComboBox ComboBoxTypeSelection;
        private ToolStripSeparator PatientVisittoolStripSeparator1;
        private DataGridViewTextBoxColumn Sno;
        private DataGridViewTextBoxColumn Date;
        private DataGridViewTextBoxColumn Patient;
        private DataGridViewTextBoxColumn PatientAge;
        private DataGridViewTextBoxColumn PatientGender;
        private DataGridViewTextBoxColumn PatientNumber;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private views.controls.grid.DataGridViewCurrencyColumn Fee;
    }
}