namespace Fa.reports.Hms
{
    partial class FormFeeCollectionReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFeeCollectionReport));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            ToolStripFeeCollectionReport = new ToolStrip();
            toolStripLabelFeeCollectionType = new ToolStripLabel();
            ComboBoxFeeCollectionType = new ToolStripComboBox();
            toolStripSeparator2 = new ToolStripSeparator();
            LabelType = new ToolStripLabel();
            CheckedTreeComboBoxConsultant = new fa.views.controls.ToolstripCheckedTreeComboBox();
            CheckedTreeComboBoxConsultations = new fa.views.controls.ToolstripCheckedTreeComboBox();
            LabelFromDate = new ToolStripLabel();
            FeeCollectionFromDate = new fa.views.controls.ToolStripCalendar();
            LabelToDate = new ToolStripLabel();
            FeeCollectionToDate = new fa.views.controls.ToolStripCalendar();
            toolStripSeparator1 = new ToolStripSeparator();
            FeeCollectionGotButton = new ToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            ToolStripFeeCollectionReportSave = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            ToolStripFeeCollectionReportPrint = new ToolStripButton();
            toolStripSeparator5 = new ToolStripSeparator();
            GridViewFeeCollection = new fa.views.controls.DataViewVerticalScroll();
            StatusStripFeeCollection = new StatusStrip();
            FeeCollectionReportErrMsg = new ToolStripStatusLabel();
            BtnReset = new Button();
            BtnSave = new Button();
            BtnPrint = new Button();
            BtnExit = new Button();
            ByDateSno = new DataGridViewTextBoxColumn();
            ByDateDt = new DataGridViewTextBoxColumn();
            ByDateName = new DataGridViewTextBoxColumn();
            ByDateOpIp = new DataGridViewTextBoxColumn();
            ByDateConsultant = new DataGridViewTextBoxColumn();
            FeeType = new DataGridViewTextBoxColumn();
            ByDateCharged = new DataGridViewTextBoxColumn();
            ByDateId = new DataGridViewTextBoxColumn();
            RowHeading = new DataGridViewTextBoxColumn();
            ToolStripFeeCollectionReport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewFeeCollection).BeginInit();
            StatusStripFeeCollection.SuspendLayout();
            SuspendLayout();
            // 
            // ToolStripFeeCollectionReport
            // 
            ToolStripFeeCollectionReport.BackColor = SystemColors.ControlLight;
            ToolStripFeeCollectionReport.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ToolStripFeeCollectionReport.GripStyle = ToolStripGripStyle.Hidden;
            ToolStripFeeCollectionReport.Items.AddRange(new ToolStripItem[] { toolStripLabelFeeCollectionType, ComboBoxFeeCollectionType, toolStripSeparator2, LabelType, CheckedTreeComboBoxConsultant, CheckedTreeComboBoxConsultations, LabelFromDate, FeeCollectionFromDate, LabelToDate, FeeCollectionToDate, toolStripSeparator1, FeeCollectionGotButton, toolStripSeparator4, ToolStripFeeCollectionReportSave, toolStripSeparator3, ToolStripFeeCollectionReportPrint, toolStripSeparator5 });
            ToolStripFeeCollectionReport.Location = new Point(0, 0);
            ToolStripFeeCollectionReport.Name = "ToolStripFeeCollectionReport";
            ToolStripFeeCollectionReport.Padding = new Padding(5);
            ToolStripFeeCollectionReport.Size = new Size(1116, 38);
            ToolStripFeeCollectionReport.TabIndex = 2;
            ToolStripFeeCollectionReport.Text = "toolStrip1";
            // 
            // toolStripLabelFeeCollectionType
            // 
            toolStripLabelFeeCollectionType.Name = "toolStripLabelFeeCollectionType";
            toolStripLabelFeeCollectionType.Size = new Size(90, 25);
            toolStripLabelFeeCollectionType.Text = "Fee Charge Type";
            // 
            // ComboBoxFeeCollectionType
            // 
            ComboBoxFeeCollectionType.FlatStyle = FlatStyle.Standard;
            ComboBoxFeeCollectionType.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ComboBoxFeeCollectionType.Items.AddRange(new object[] { "By Date", "By Consultant", "By Fees", "By OP", "By IP" });
            ComboBoxFeeCollectionType.Name = "ComboBoxFeeCollectionType";
            ComboBoxFeeCollectionType.Size = new Size(103, 28);
            ComboBoxFeeCollectionType.Text = "By Date";
            ComboBoxFeeCollectionType.DropDown += ComboBoxFeeCollectionType_DropDown;
            ComboBoxFeeCollectionType.DropDownClosed += ComboBoxFeeCollectionType_DropDownClosed;
            ComboBoxFeeCollectionType.SelectedIndexChanged += ComboBoxFeeCollectionType_SelectedIndexChanged;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 28);
            // 
            // LabelType
            // 
            LabelType.Name = "LabelType";
            LabelType.Size = new Size(59, 25);
            LabelType.Text = "Consultant";
            LabelType.TextAlign = ContentAlignment.MiddleLeft;
            LabelType.Visible = false;
            // 
            // CheckedTreeComboBoxConsultant
            // 
            CheckedTreeComboBoxConsultant.AutoSize = false;
            CheckedTreeComboBoxConsultant.Name = "CheckedTreeComboBoxConsultant";
            CheckedTreeComboBoxConsultant.SelectedNode = null;
            CheckedTreeComboBoxConsultant.Size = new Size(147, 21);
            CheckedTreeComboBoxConsultant.Visible = false;
            CheckedTreeComboBoxConsultant.NodeClickedEvent += CheckedTreeComboBoxConsultant_NodeClickedEvent;
            // 
            // CheckedTreeComboBoxConsultations
            // 
            CheckedTreeComboBoxConsultations.AutoSize = false;
            CheckedTreeComboBoxConsultations.Name = "CheckedTreeComboBoxConsultations";
            CheckedTreeComboBoxConsultations.SelectedNode = null;
            CheckedTreeComboBoxConsultations.Size = new Size(147, 21);
            CheckedTreeComboBoxConsultations.Visible = false;
            CheckedTreeComboBoxConsultations.NodeClickedEvent += CheckedTreeComboBoxConsultations_NodeClickedEvent;
            // 
            // LabelFromDate
            // 
            LabelFromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelFromDate.Name = "LabelFromDate";
            LabelFromDate.Size = new Size(31, 25);
            LabelFromDate.Text = "From";
            // 
            // FeeCollectionFromDate
            // 
            FeeCollectionFromDate.BackColor = Color.White;
            FeeCollectionFromDate.Date = null;
            FeeCollectionFromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FeeCollectionFromDate.Format = "MM/dd/yyyy";
            FeeCollectionFromDate.MaxDate = new DateTime(9997, 12, 31, 0, 6, 19, 0);
            FeeCollectionFromDate.MinDate = new DateTime(1900, 1, 1, 23, 55, 17, 0);
            FeeCollectionFromDate.Name = "FeeCollectionFromDate";
            FeeCollectionFromDate.Size = new Size(97, 25);
            FeeCollectionFromDate.Text = "Calendar";
            // 
            // LabelToDate
            // 
            LabelToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelToDate.Name = "LabelToDate";
            LabelToDate.Size = new Size(19, 25);
            LabelToDate.Text = "To";
            // 
            // FeeCollectionToDate
            // 
            FeeCollectionToDate.BackColor = Color.White;
            FeeCollectionToDate.Date = null;
            FeeCollectionToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FeeCollectionToDate.Format = "MM/dd/yyyy";
            FeeCollectionToDate.MaxDate = new DateTime(9997, 12, 31, 0, 6, 19, 0);
            FeeCollectionToDate.MinDate = new DateTime(1900, 1, 1, 23, 55, 17, 0);
            FeeCollectionToDate.Name = "FeeCollectionToDate";
            FeeCollectionToDate.Size = new Size(97, 25);
            FeeCollectionToDate.Text = "Calendar";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 28);
            // 
            // FeeCollectionGotButton
            // 
            FeeCollectionGotButton.AutoToolTip = false;
            FeeCollectionGotButton.BackgroundImageLayout = ImageLayout.None;
            FeeCollectionGotButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            FeeCollectionGotButton.ImageTransparentColor = Color.Magenta;
            FeeCollectionGotButton.Name = "FeeCollectionGotButton";
            FeeCollectionGotButton.Size = new Size(24, 25);
            FeeCollectionGotButton.Text = "Go";
            FeeCollectionGotButton.Click += FeeCollectionGotButton_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 28);
            // 
            // ToolStripFeeCollectionReportSave
            // 
            ToolStripFeeCollectionReportSave.AutoToolTip = false;
            ToolStripFeeCollectionReportSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripFeeCollectionReportSave.Image = (Image)resources.GetObject("ToolStripFeeCollectionReportSave.Image");
            ToolStripFeeCollectionReportSave.ImageTransparentColor = Color.Black;
            ToolStripFeeCollectionReportSave.Name = "ToolStripFeeCollectionReportSave";
            ToolStripFeeCollectionReportSave.Size = new Size(23, 25);
            ToolStripFeeCollectionReportSave.Text = "Save";
            ToolStripFeeCollectionReportSave.Click += ToolStripFeeCollectionReportSave_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 28);
            // 
            // ToolStripFeeCollectionReportPrint
            // 
            ToolStripFeeCollectionReportPrint.AutoToolTip = false;
            ToolStripFeeCollectionReportPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripFeeCollectionReportPrint.Image = (Image)resources.GetObject("ToolStripFeeCollectionReportPrint.Image");
            ToolStripFeeCollectionReportPrint.ImageTransparentColor = Color.Black;
            ToolStripFeeCollectionReportPrint.Name = "ToolStripFeeCollectionReportPrint";
            ToolStripFeeCollectionReportPrint.Size = new Size(23, 25);
            ToolStripFeeCollectionReportPrint.Text = "Print";
            ToolStripFeeCollectionReportPrint.Click += ToolStripFeeCollectionReportPrint_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(6, 28);
            // 
            // GridViewFeeCollection
            // 
            GridViewFeeCollection.AllowUserToAddRows = false;
            GridViewFeeCollection.AllowUserToDeleteRows = false;
            GridViewFeeCollection.AllowUserToResizeColumns = false;
            GridViewFeeCollection.AllowUserToResizeRows = false;
            GridViewFeeCollection.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            GridViewFeeCollection.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewFeeCollection.ColumnHeadersHeight = 20;
            GridViewFeeCollection.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewFeeCollection.Columns.AddRange(new DataGridViewColumn[] { ByDateSno, ByDateDt, ByDateName, ByDateOpIp, ByDateConsultant, FeeType, ByDateCharged, ByDateId, RowHeading });
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Window;
            dataGridViewCellStyle8.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle8.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            GridViewFeeCollection.DefaultCellStyle = dataGridViewCellStyle8;
            GridViewFeeCollection.EnableHeadersVisualStyles = false;
            GridViewFeeCollection.Location = new Point(2, 40);
            GridViewFeeCollection.Name = "GridViewFeeCollection";
            GridViewFeeCollection.ReadOnly = true;
            GridViewFeeCollection.RowHeadersVisible = false;
            GridViewFeeCollection.RowHeadersWidth = 51;
            GridViewFeeCollection.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            GridViewFeeCollection.RowTemplate.Height = 20;
            GridViewFeeCollection.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewFeeCollection.ShowCellToolTips = false;
            GridViewFeeCollection.Size = new Size(1111, 383);
            GridViewFeeCollection.TabIndex = 116;
            GridViewFeeCollection.CellClick += GridViewFeeCollection_CellClick;
            GridViewFeeCollection.CellFormatting += GridViewFeeCollection_CellFormatting;
            GridViewFeeCollection.CellPainting += GridViewFeeCollection_CellPainting;
            GridViewFeeCollection.RowPostPaint += GridViewFeeCollection_RowPostPaint;
            GridViewFeeCollection.SelectionChanged += GridViewFeeCollection_SelectionChanged;
            // 
            // StatusStripFeeCollection
            // 
            StatusStripFeeCollection.ImageScalingSize = new Size(24, 24);
            StatusStripFeeCollection.Items.AddRange(new ToolStripItem[] { FeeCollectionReportErrMsg });
            StatusStripFeeCollection.Location = new Point(0, 468);
            StatusStripFeeCollection.Name = "StatusStripFeeCollection";
            StatusStripFeeCollection.Padding = new Padding(1, 0, 12, 0);
            StatusStripFeeCollection.Size = new Size(1116, 22);
            StatusStripFeeCollection.TabIndex = 117;
            StatusStripFeeCollection.Text = "statusStrip1";
            // 
            // FeeCollectionReportErrMsg
            // 
            FeeCollectionReportErrMsg.Name = "FeeCollectionReportErrMsg";
            FeeCollectionReportErrMsg.Size = new Size(28, 17);
            FeeCollectionReportErrMsg.Text = "       ";
            // 
            // BtnReset
            // 
            BtnReset.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnReset.Location = new Point(769, 434);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new Size(87, 24);
            BtnReset.TabIndex = 120;
            BtnReset.Text = "Reset [Esc]";
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // BtnSave
            // 
            BtnSave.Enabled = false;
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(862, 434);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(77, 24);
            BtnSave.TabIndex = 121;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Enabled = false;
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(945, 434);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(77, 24);
            BtnPrint.TabIndex = 119;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(1028, 434);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(72, 24);
            BtnExit.TabIndex = 118;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // ByDateSno
            // 
            dataGridViewCellStyle2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ByDateSno.DefaultCellStyle = dataGridViewCellStyle2;
            ByDateSno.Frozen = true;
            ByDateSno.HeaderText = "#";
            ByDateSno.MinimumWidth = 8;
            ByDateSno.Name = "ByDateSno";
            ByDateSno.ReadOnly = true;
            ByDateSno.Resizable = DataGridViewTriState.False;
            ByDateSno.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateSno.Width = 50;
            // 
            // ByDateDt
            // 
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ByDateDt.DefaultCellStyle = dataGridViewCellStyle3;
            ByDateDt.Frozen = true;
            ByDateDt.HeaderText = "Date";
            ByDateDt.MinimumWidth = 8;
            ByDateDt.Name = "ByDateDt";
            ByDateDt.ReadOnly = true;
            ByDateDt.Resizable = DataGridViewTriState.False;
            ByDateDt.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // ByDateName
            // 
            dataGridViewCellStyle4.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ByDateName.DefaultCellStyle = dataGridViewCellStyle4;
            ByDateName.Frozen = true;
            ByDateName.HeaderText = "Patient Name";
            ByDateName.MinimumWidth = 8;
            ByDateName.Name = "ByDateName";
            ByDateName.ReadOnly = true;
            ByDateName.Resizable = DataGridViewTriState.False;
            ByDateName.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateName.Width = 225;
            // 
            // ByDateOpIp
            // 
            dataGridViewCellStyle5.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ByDateOpIp.DefaultCellStyle = dataGridViewCellStyle5;
            ByDateOpIp.Frozen = true;
            ByDateOpIp.HeaderText = "OP/IP";
            ByDateOpIp.MinimumWidth = 8;
            ByDateOpIp.Name = "ByDateOpIp";
            ByDateOpIp.ReadOnly = true;
            ByDateOpIp.Resizable = DataGridViewTriState.False;
            ByDateOpIp.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateOpIp.Width = 125;
            // 
            // ByDateConsultant
            // 
            dataGridViewCellStyle6.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ByDateConsultant.DefaultCellStyle = dataGridViewCellStyle6;
            ByDateConsultant.Frozen = true;
            ByDateConsultant.HeaderText = "Consultant";
            ByDateConsultant.MinimumWidth = 8;
            ByDateConsultant.Name = "ByDateConsultant";
            ByDateConsultant.ReadOnly = true;
            ByDateConsultant.Resizable = DataGridViewTriState.False;
            ByDateConsultant.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateConsultant.Width = 200;
            // 
            // FeeType
            // 
            FeeType.Frozen = true;
            FeeType.HeaderText = "Fee Type";
            FeeType.MinimumWidth = 6;
            FeeType.Name = "FeeType";
            FeeType.ReadOnly = true;
            FeeType.Resizable = DataGridViewTriState.False;
            FeeType.SortMode = DataGridViewColumnSortMode.NotSortable;
            FeeType.Width = 260;
            // 
            // ByDateCharged
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle7.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ByDateCharged.DefaultCellStyle = dataGridViewCellStyle7;
            ByDateCharged.Frozen = true;
            ByDateCharged.HeaderText = "Charge Amount";
            ByDateCharged.MinimumWidth = 8;
            ByDateCharged.Name = "ByDateCharged";
            ByDateCharged.ReadOnly = true;
            ByDateCharged.Resizable = DataGridViewTriState.False;
            ByDateCharged.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateCharged.Width = 130;
            // 
            // ByDateId
            // 
            ByDateId.Frozen = true;
            ByDateId.HeaderText = "Id";
            ByDateId.MinimumWidth = 8;
            ByDateId.Name = "ByDateId";
            ByDateId.ReadOnly = true;
            ByDateId.Resizable = DataGridViewTriState.False;
            ByDateId.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateId.Visible = false;
            ByDateId.Width = 150;
            // 
            // RowHeading
            // 
            RowHeading.Frozen = true;
            RowHeading.HeaderText = "Title";
            RowHeading.MinimumWidth = 8;
            RowHeading.Name = "RowHeading";
            RowHeading.ReadOnly = true;
            RowHeading.Resizable = DataGridViewTriState.False;
            RowHeading.SortMode = DataGridViewColumnSortMode.NotSortable;
            RowHeading.Visible = false;
            RowHeading.Width = 150;
            // 
            // FormFeeCollectionReport
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1116, 490);
            Controls.Add(BtnReset);
            Controls.Add(BtnSave);
            Controls.Add(BtnPrint);
            Controls.Add(BtnExit);
            Controls.Add(StatusStripFeeCollection);
            Controls.Add(GridViewFeeCollection);
            Controls.Add(ToolStripFeeCollectionReport);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormFeeCollectionReport";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Fee Charge Report";
            Load += FormFeeCollectionReport_Load;
            ToolStripFeeCollectionReport.ResumeLayout(false);
            ToolStripFeeCollectionReport.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewFeeCollection).EndInit();
            StatusStripFeeCollection.ResumeLayout(false);
            StatusStripFeeCollection.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip ToolStripFeeCollectionReport;
        private ToolStripLabel toolStripLabelFeeCollectionType;
        private ToolStripLabel LabelFromDate;
        private fa.views.controls.ToolStripCalendar FeeCollectionFromDate;
        private ToolStripLabel LabelToDate;
        private fa.views.controls.ToolStripCalendar FeeCollectionToDate;
        private ToolStripButton FeeCollectionGotButton;
        private ToolStripButton ToolStripFeeCollectionReportSave;
        private ToolStripButton ToolStripFeeCollectionReportPrint;
        private fa.views.controls.DataViewVerticalScroll GridViewFeeCollection;
        private StatusStrip StatusStripFeeCollection;
        private ToolStripStatusLabel FeeCollectionReportErrMsg;
        private Button BtnReset;
        private Button BtnSave;
        private Button BtnPrint;
        private Button BtnExit;
        private fa.views.controls.ToolstripCheckedTreeComboBox CheckedTreeComboBoxConsultant;
        private ToolStripLabel LabelType;
        private ToolStripComboBox ComboBoxFeeCollectionType;
        private fa.views.controls.ToolstripCheckedTreeComboBox CheckedTreeComboBoxConsultations;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton ToolStripBtnSave;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator5;
        private DataGridViewTextBoxColumn ByDateSno;
        private DataGridViewTextBoxColumn ByDateDt;
        private DataGridViewTextBoxColumn ByDateName;
        private DataGridViewTextBoxColumn ByDateOpIp;
        private DataGridViewTextBoxColumn ByDateConsultant;
        private DataGridViewTextBoxColumn FeeType;
        private DataGridViewTextBoxColumn ByDateCharged;
        private DataGridViewTextBoxColumn ByDateId;
        private DataGridViewTextBoxColumn RowHeading;
    }
}