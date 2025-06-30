namespace Fa.reports.Hms
{
    partial class FormLabTestReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLabTestReport));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            statusStripLabTestRpt = new StatusStrip();
            LabTestRptErrMsg = new ToolStripStatusLabel();
            toolStripLabTestRpt = new ToolStrip();
            toolStripLabel2 = new ToolStripLabel();
            ComboBoxType = new ToolStripComboBox();
            toolStripSeparatorthree = new ToolStripSeparator();
            LabelType = new ToolStripLabel();
            CheckedTreeComboBoxPatient = new fa.views.controls.ToolstripCheckedTreeComboBox();
            CheckedTreeComboBoxLabTestName = new fa.views.controls.ToolstripCheckedTreeComboBox();
            toolStripSeparatorTwo = new ToolStripSeparator();
            toolStripLabel6 = new ToolStripLabel();
            LabTestFromDate = new fa.views.controls.ToolStripCalendar();
            toolStripSeparator9 = new ToolStripSeparator();
            toolStripLabel7 = new ToolStripLabel();
            LabTestToDate = new fa.views.controls.ToolStripCalendar();
            toolStripSeparator7 = new ToolStripSeparator();
            ToolStripBtnGo = new ToolStripButton();
            toolStripSeparator10 = new ToolStripSeparator();
            ToolStripBtnSave = new ToolStripButton();
            toolStripSeparator11 = new ToolStripSeparator();
            ToolStripBtnPrint = new ToolStripButton();
            GridViewByDateLabTestReport = new fa.views.controls.DataViewVerticalScroll();
            BtnReset = new Button();
            BtnExit = new Button();
            BtnPrint = new Button();
            BtnSave = new Button();
            ByDateTestDate = new DataGridViewTextBoxColumn();
            ByDatePatientId = new DataGridViewTextBoxColumn();
            ByDatePatientName = new DataGridViewTextBoxColumn();
            ByDateTestName = new DataGridViewTextBoxColumn();
            ByDateElementName = new DataGridViewTextBoxColumn();
            ByDateUOM = new DataGridViewTextBoxColumn();
            ByDateClass = new DataGridViewTextBoxColumn();
            ByDateSubClass = new DataGridViewTextBoxColumn();
            SingleValue = new DataGridViewTextBoxColumn();
            ByDateRangeFrom = new DataGridViewTextBoxColumn();
            ByDateRangeTo = new DataGridViewTextBoxColumn();
            ByDateResult = new DataGridViewTextBoxColumn();
            RowHead = new DataGridViewTextBoxColumn();
            statusStripLabTestRpt.SuspendLayout();
            toolStripLabTestRpt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewByDateLabTestReport).BeginInit();
            SuspendLayout();
            // 
            // statusStripLabTestRpt
            // 
            statusStripLabTestRpt.Items.AddRange(new ToolStripItem[] { LabTestRptErrMsg });
            statusStripLabTestRpt.Location = new Point(0, 538);
            statusStripLabTestRpt.Name = "statusStripLabTestRpt";
            statusStripLabTestRpt.Size = new Size(1251, 22);
            statusStripLabTestRpt.TabIndex = 0;
            statusStripLabTestRpt.Text = "statusStrip1";
            // 
            // LabTestRptErrMsg
            // 
            LabTestRptErrMsg.Name = "LabTestRptErrMsg";
            LabTestRptErrMsg.Size = new Size(37, 17);
            LabTestRptErrMsg.Text = "          ";
            // 
            // toolStripLabTestRpt
            // 
            toolStripLabTestRpt.BackColor = SystemColors.ControlLight;
            toolStripLabTestRpt.GripStyle = ToolStripGripStyle.Hidden;
            toolStripLabTestRpt.Items.AddRange(new ToolStripItem[] { toolStripLabel2, ComboBoxType, toolStripSeparatorthree, LabelType, CheckedTreeComboBoxPatient, CheckedTreeComboBoxLabTestName, toolStripSeparatorTwo, toolStripLabel6, LabTestFromDate, toolStripSeparator9, toolStripLabel7, LabTestToDate, toolStripSeparator7, ToolStripBtnGo, toolStripSeparator10, ToolStripBtnSave, toolStripSeparator11, ToolStripBtnPrint });
            toolStripLabTestRpt.Location = new Point(0, 0);
            toolStripLabTestRpt.Name = "toolStripLabTestRpt";
            toolStripLabTestRpt.Padding = new Padding(5);
            toolStripLabTestRpt.ShowItemToolTips = false;
            toolStripLabTestRpt.Size = new Size(1251, 38);
            toolStripLabTestRpt.TabIndex = 1;
            toolStripLabTestRpt.Text = "toolStrip1";
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(35, 25);
            toolStripLabel2.Text = "Type";
            // 
            // ComboBoxType
            // 
            ComboBoxType.CausesValidation = false;
            ComboBoxType.FlatStyle = FlatStyle.Standard;
            ComboBoxType.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ComboBoxType.Items.AddRange(new object[] { "By Date", "By Patient", "By Test Name" });
            ComboBoxType.Margin = new Padding(0, 1, 0, 2);
            ComboBoxType.Name = "ComboBoxType";
            ComboBoxType.Size = new Size(151, 25);
            ComboBoxType.Text = "By Date";
            ComboBoxType.DropDown += ComboBoxType_DropDown;
            ComboBoxType.DropDownClosed += ComboBoxType_DropDownClosed;
            ComboBoxType.SelectedIndexChanged += ComboBoxType_SelectedIndexChanged;
            ComboBoxType.TextUpdate += ComboBoxType_TextUpdate;
            ComboBoxType.KeyDown += ComboBoxType_KeyDown;
            ComboBoxType.KeyPress += ComboBoxType_KeyPress;
            // 
            // toolStripSeparatorthree
            // 
            toolStripSeparatorthree.Name = "toolStripSeparatorthree";
            toolStripSeparatorthree.Size = new Size(6, 28);
            // 
            // LabelType
            // 
            LabelType.Name = "LabelType";
            LabelType.Size = new Size(44, 25);
            LabelType.Text = "Patient";
            LabelType.Visible = false;
            // 
            // CheckedTreeComboBoxPatient
            // 
            CheckedTreeComboBoxPatient.AutoSize = false;
            CheckedTreeComboBoxPatient.Name = "CheckedTreeComboBoxPatient";
            CheckedTreeComboBoxPatient.SelectedNode = null;
            CheckedTreeComboBoxPatient.Size = new Size(150, 25);
            CheckedTreeComboBoxPatient.Visible = false;
            CheckedTreeComboBoxPatient.NodeClickedEvent += CheckedTreeComboBoxPatient_NodeClickedEvent;
            // 
            // CheckedTreeComboBoxLabTestName
            // 
            CheckedTreeComboBoxLabTestName.AutoSize = false;
            CheckedTreeComboBoxLabTestName.Name = "CheckedTreeComboBoxLabTestName";
            CheckedTreeComboBoxLabTestName.SelectedNode = null;
            CheckedTreeComboBoxLabTestName.Size = new Size(100, 25);
            CheckedTreeComboBoxLabTestName.Visible = false;
            CheckedTreeComboBoxLabTestName.NodeClickedEvent += CheckedTreeComboBoxLabTestName_NodeClickedEvent;
            // 
            // toolStripSeparatorTwo
            // 
            toolStripSeparatorTwo.Name = "toolStripSeparatorTwo";
            toolStripSeparatorTwo.Size = new Size(6, 28);
            toolStripSeparatorTwo.Visible = false;
            // 
            // toolStripLabel6
            // 
            toolStripLabel6.Name = "toolStripLabel6";
            toolStripLabel6.Size = new Size(35, 25);
            toolStripLabel6.Text = "From";
            // 
            // LabTestFromDate
            // 
            LabTestFromDate.BackColor = Color.White;
            LabTestFromDate.Date = null;
            LabTestFromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabTestFromDate.Format = "MM/dd/yyyy";
            LabTestFromDate.MaxDate = new DateTime(9997, 12, 31, 8, 36, 19, 0);
            LabTestFromDate.MinDate = new DateTime(1900, 1, 1, 23, 1, 20, 0);
            LabTestFromDate.Name = "LabTestFromDate";
            LabTestFromDate.Size = new Size(97, 25);
            LabTestFromDate.Text = "Calender";
            // 
            // toolStripSeparator9
            // 
            toolStripSeparator9.Name = "toolStripSeparator9";
            toolStripSeparator9.Size = new Size(6, 28);
            // 
            // toolStripLabel7
            // 
            toolStripLabel7.Name = "toolStripLabel7";
            toolStripLabel7.Size = new Size(19, 25);
            toolStripLabel7.Text = "To";
            // 
            // LabTestToDate
            // 
            LabTestToDate.BackColor = Color.White;
            LabTestToDate.Date = null;
            LabTestToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabTestToDate.Format = "MM/dd/yyyy";
            LabTestToDate.MaxDate = new DateTime(9997, 12, 31, 8, 36, 19, 0);
            LabTestToDate.MinDate = new DateTime(1900, 1, 1, 23, 1, 20, 0);
            LabTestToDate.Name = "LabTestToDate";
            LabTestToDate.Size = new Size(97, 25);
            LabTestToDate.Text = "Calender";
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new Size(6, 28);
            // 
            // ToolStripBtnGo
            // 
            ToolStripBtnGo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            ToolStripBtnGo.Image = (Image)resources.GetObject("ToolStripBtnGo.Image");
            ToolStripBtnGo.ImageTransparentColor = Color.Magenta;
            ToolStripBtnGo.Name = "ToolStripBtnGo";
            ToolStripBtnGo.Size = new Size(26, 25);
            ToolStripBtnGo.Text = "Go";
            ToolStripBtnGo.Click += ToolStripBtnGo_Click;
            // 
            // toolStripSeparator10
            // 
            toolStripSeparator10.Name = "toolStripSeparator10";
            toolStripSeparator10.Size = new Size(6, 28);
            // 
            // ToolStripBtnSave
            // 
            ToolStripBtnSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnSave.Enabled = false;
            ToolStripBtnSave.Image = (Image)resources.GetObject("ToolStripBtnSave.Image");
            ToolStripBtnSave.ImageTransparentColor = Color.Black;
            ToolStripBtnSave.Name = "ToolStripBtnSave";
            ToolStripBtnSave.Size = new Size(23, 25);
            ToolStripBtnSave.Text = "Save";
            ToolStripBtnSave.Click += BtnSave_Click;
            // 
            // toolStripSeparator11
            // 
            toolStripSeparator11.Name = "toolStripSeparator11";
            toolStripSeparator11.Size = new Size(6, 28);
            // 
            // ToolStripBtnPrint
            // 
            ToolStripBtnPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnPrint.Enabled = false;
            ToolStripBtnPrint.Image = (Image)resources.GetObject("ToolStripBtnPrint.Image");
            ToolStripBtnPrint.ImageTransparentColor = Color.Black;
            ToolStripBtnPrint.Name = "ToolStripBtnPrint";
            ToolStripBtnPrint.Size = new Size(23, 25);
            ToolStripBtnPrint.Text = "Print";
            ToolStripBtnPrint.Click += BtnPrint_Click;
            // 
            // GridViewByDateLabTestReport
            // 
            GridViewByDateLabTestReport.AllowUserToAddRows = false;
            GridViewByDateLabTestReport.AllowUserToDeleteRows = false;
            GridViewByDateLabTestReport.AllowUserToResizeColumns = false;
            GridViewByDateLabTestReport.AllowUserToResizeRows = false;
            GridViewByDateLabTestReport.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewByDateLabTestReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewByDateLabTestReport.ColumnHeadersHeight = 20;
            GridViewByDateLabTestReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewByDateLabTestReport.Columns.AddRange(new DataGridViewColumn[] { ByDateTestDate, ByDatePatientId, ByDatePatientName, ByDateTestName, ByDateElementName, ByDateUOM, ByDateClass, ByDateSubClass, SingleValue, ByDateRangeFrom, ByDateRangeTo, ByDateResult, RowHead });
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Window;
            dataGridViewCellStyle8.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle8.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            GridViewByDateLabTestReport.DefaultCellStyle = dataGridViewCellStyle8;
            GridViewByDateLabTestReport.EnableHeadersVisualStyles = false;
            GridViewByDateLabTestReport.Location = new Point(2, 41);
            GridViewByDateLabTestReport.MultiSelect = false;
            GridViewByDateLabTestReport.Name = "GridViewByDateLabTestReport";
            GridViewByDateLabTestReport.ReadOnly = true;
            GridViewByDateLabTestReport.RowHeadersVisible = false;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            GridViewByDateLabTestReport.RowsDefaultCellStyle = dataGridViewCellStyle9;
            GridViewByDateLabTestReport.RowTemplate.Height = 20;
            GridViewByDateLabTestReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewByDateLabTestReport.ShowCellToolTips = false;
            GridViewByDateLabTestReport.Size = new Size(1248, 450);
            GridViewByDateLabTestReport.TabIndex = 2;
            GridViewByDateLabTestReport.CellPainting += GridViewByDateLabTestReport_CellPainting;
            GridViewByDateLabTestReport.RowPostPaint += GridViewByDateLabTestReport_RowPostPaint;
            // 
            // BtnReset
            // 
            BtnReset.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnReset.Location = new Point(912, 505);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new Size(82, 21);
            BtnReset.TabIndex = 36;
            BtnReset.Text = "Reset [Esc]";
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(1162, 505);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(75, 21);
            BtnExit.TabIndex = 35;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Enabled = false;
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(1081, 505);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 21);
            BtnPrint.TabIndex = 34;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnSave
            // 
            BtnSave.Enabled = false;
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(1000, 505);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 21);
            BtnSave.TabIndex = 33;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // ByDateTestDate
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            ByDateTestDate.DefaultCellStyle = dataGridViewCellStyle2;
            ByDateTestDate.HeaderText = "Date";
            ByDateTestDate.Name = "ByDateTestDate";
            ByDateTestDate.ReadOnly = true;
            ByDateTestDate.Resizable = DataGridViewTriState.False;
            ByDateTestDate.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateTestDate.Width = 97;
            // 
            // ByDatePatientId
            // 
            ByDatePatientId.HeaderText = "Patient Id";
            ByDatePatientId.Name = "ByDatePatientId";
            ByDatePatientId.ReadOnly = true;
            ByDatePatientId.Resizable = DataGridViewTriState.False;
            ByDatePatientId.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // ByDatePatientName
            // 
            ByDatePatientName.HeaderText = "Patient Name";
            ByDatePatientName.Name = "ByDatePatientName";
            ByDatePatientName.ReadOnly = true;
            ByDatePatientName.Resizable = DataGridViewTriState.False;
            ByDatePatientName.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDatePatientName.Width = 150;
            // 
            // ByDateTestName
            // 
            ByDateTestName.HeaderText = "Test Name";
            ByDateTestName.Name = "ByDateTestName";
            ByDateTestName.ReadOnly = true;
            ByDateTestName.Resizable = DataGridViewTriState.False;
            ByDateTestName.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateTestName.Width = 180;
            // 
            // ByDateElementName
            // 
            ByDateElementName.HeaderText = "Element";
            ByDateElementName.Name = "ByDateElementName";
            ByDateElementName.ReadOnly = true;
            ByDateElementName.Resizable = DataGridViewTriState.False;
            ByDateElementName.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateElementName.Width = 150;
            // 
            // ByDateUOM
            // 
            ByDateUOM.HeaderText = "UOM";
            ByDateUOM.Name = "ByDateUOM";
            ByDateUOM.ReadOnly = true;
            ByDateUOM.Resizable = DataGridViewTriState.False;
            ByDateUOM.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateUOM.Width = 80;
            // 
            // ByDateClass
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            ByDateClass.DefaultCellStyle = dataGridViewCellStyle3;
            ByDateClass.HeaderText = "Class";
            ByDateClass.Name = "ByDateClass";
            ByDateClass.ReadOnly = true;
            ByDateClass.Resizable = DataGridViewTriState.False;
            ByDateClass.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateClass.Width = 80;
            // 
            // ByDateSubClass
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopLeft;
            ByDateSubClass.DefaultCellStyle = dataGridViewCellStyle4;
            ByDateSubClass.HeaderText = "SubClass";
            ByDateSubClass.Name = "ByDateSubClass";
            ByDateSubClass.ReadOnly = true;
            ByDateSubClass.Resizable = DataGridViewTriState.False;
            ByDateSubClass.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateSubClass.Width = 70;
            // 
            // SingleValue
            // 
            SingleValue.HeaderText = "Single Value";
            SingleValue.Name = "SingleValue";
            SingleValue.ReadOnly = true;
            SingleValue.Resizable = DataGridViewTriState.False;
            SingleValue.SortMode = DataGridViewColumnSortMode.NotSortable;
            SingleValue.Width = 80;
            // 
            // ByDateRangeFrom
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.TopLeft;
            ByDateRangeFrom.DefaultCellStyle = dataGridViewCellStyle5;
            ByDateRangeFrom.HeaderText = "Range From";
            ByDateRangeFrom.Name = "ByDateRangeFrom";
            ByDateRangeFrom.ReadOnly = true;
            ByDateRangeFrom.Resizable = DataGridViewTriState.False;
            ByDateRangeFrom.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateRangeFrom.Width = 80;
            // 
            // ByDateRangeTo
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.TopLeft;
            ByDateRangeTo.DefaultCellStyle = dataGridViewCellStyle6;
            ByDateRangeTo.HeaderText = "Range To";
            ByDateRangeTo.Name = "ByDateRangeTo";
            ByDateRangeTo.ReadOnly = true;
            ByDateRangeTo.Resizable = DataGridViewTriState.False;
            ByDateRangeTo.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateRangeTo.Width = 80;
            // 
            // ByDateResult
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.TopLeft;
            ByDateResult.DefaultCellStyle = dataGridViewCellStyle7;
            ByDateResult.HeaderText = "Result";
            ByDateResult.Name = "ByDateResult";
            ByDateResult.ReadOnly = true;
            ByDateResult.Resizable = DataGridViewTriState.False;
            ByDateResult.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateResult.Width = 80;
            // 
            // RowHead
            // 
            RowHead.HeaderText = "Row Heading";
            RowHead.MinimumWidth = 2;
            RowHead.Name = "RowHead";
            RowHead.ReadOnly = true;
            RowHead.Visible = false;
            RowHead.Width = 2;
            // 
            // FormLabTestReport
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1251, 560);
            Controls.Add(BtnReset);
            Controls.Add(BtnExit);
            Controls.Add(BtnPrint);
            Controls.Add(BtnSave);
            Controls.Add(toolStripLabTestRpt);
            Controls.Add(statusStripLabTestRpt);
            Controls.Add(GridViewByDateLabTestReport);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormLabTestReport";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "LabTest Report";
            Load += FormLabTestReport_Load;
            MouseMove += FormLabTestReport_MouseMove;
            statusStripLabTestRpt.ResumeLayout(false);
            statusStripLabTestRpt.PerformLayout();
            toolStripLabTestRpt.ResumeLayout(false);
            toolStripLabTestRpt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewByDateLabTestReport).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusStripLabTestRpt;
        private ToolStripStatusLabel LabTestRptErrMsg;
        private ToolStrip toolStripLabTestRpt;
        private ToolStripLabel LabelType;
        private fa.views.controls.ToolstripCheckedTreeComboBox CheckedTreeComboBoxPatient;
        private ToolStripLabel toolStripLabel6;
        private fa.views.controls.ToolStripCalendar LabTestFromDate;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripLabel toolStripLabel7;
        private fa.views.controls.ToolStripCalendar LabTestToDate;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripButton ToolStripBtnGo;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripButton ToolStripBtnSave;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripButton ToolStripBtnPrint;
        private ToolStripSeparator toolStripSeparatorTwo;
        private fa.views.controls.ToolstripCheckedTreeComboBox CheckedTreeComboBoxLabTestName;
        private fa.views.controls.DataViewVerticalScroll GridViewByDateLabTestReport;
        private Button BtnReset;
        private Button BtnExit;
        private Button BtnPrint;
        private Button BtnSave;
        private ToolStripSeparator toolStripSeparatorthree;
        private ToolStripLabel toolStripLabel2;
        private ToolStripComboBox ComboBoxType;
        private fa.views.controls.ToolstripComboBoxWithSearchFilter toolstripComboBoxWithSearchFilter1;
        private fa.views.controls.ToolstripComboBoxWithSearchFilter toolstripComboBoxWithSearchFilter2;
        private DataGridViewTextBoxColumn ByDateTestDate;
        private DataGridViewTextBoxColumn ByDatePatientId;
        private DataGridViewTextBoxColumn ByDatePatientName;
        private DataGridViewTextBoxColumn ByDateTestName;
        private DataGridViewTextBoxColumn ByDateElementName;
        private DataGridViewTextBoxColumn ByDateUOM;
        private DataGridViewTextBoxColumn ByDateClass;
        private DataGridViewTextBoxColumn ByDateSubClass;
        private DataGridViewTextBoxColumn SingleValue;
        private DataGridViewTextBoxColumn ByDateRangeFrom;
        private DataGridViewTextBoxColumn ByDateRangeTo;
        private DataGridViewTextBoxColumn ByDateResult;
        private DataGridViewTextBoxColumn RowHead;
    }
}