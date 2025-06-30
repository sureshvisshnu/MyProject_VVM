namespace Fa.reports.Hms
{
    partial class FormCareTakerUnAssignReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCareTakerUnAssignReport));
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            BerklySoftToolStrip = new fa.views.controls.Ab2ToolStrip();
            toolStripLabel11 = new ToolStripLabel();
            ComboBoxReportType = new ToolStripComboBox();
            LabelType = new ToolStripLabel();
            ComboBoxDepartment = new fa.views.controls.ToolstripCheckedTreeComboBox();
            ComboBoxConsultant = new fa.views.controls.ToolstripCheckedTreeComboBox();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripLabel6 = new ToolStripLabel();
            CTReportFromDate = new fa.views.controls.ToolStripCalendar();
            toolStripLabel7 = new ToolStripLabel();
            CTReportToDate = new fa.views.controls.ToolStripCalendar();
            toolStripSeparator12 = new ToolStripSeparator();
            ToolStripBtnGo = new ToolStripButton();
            ToolStripBtnSave = new ToolStripButton();
            toolStripSeparator11 = new ToolStripSeparator();
            ToolStripBtnPrint = new ToolStripButton();
            GridviewCareTakerReport = new fa.views.controls.DataViewVerticalScroll();
            slno = new DataGridViewTextBoxColumn();
            name = new DataGridViewTextBoxColumn();
            age = new DataGridViewTextBoxColumn();
            address = new DataGridViewTextBoxColumn();
            dept = new DataGridViewTextBoxColumn();
            adate = new DataGridViewTextBoxColumn();
            TaskDate = new DataGridViewTextBoxColumn();
            AdmissionId = new DataGridViewTextBoxColumn();
            RowHead = new DataGridViewTextBoxColumn();
            BerklySoftStatusStrip = new StatusStrip();
            CareTakerReportErrorMsg = new ToolStripStatusLabel();
            BtnReset = new Button();
            BtnExit = new Button();
            BtnPrint = new Button();
            BtnSave = new Button();
            BerklySoftToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridviewCareTakerReport).BeginInit();
            BerklySoftStatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // BerklySoftToolStrip
            // 
            BerklySoftToolStrip.BackColor = SystemColors.ControlLight;
            BerklySoftToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            BerklySoftToolStrip.Items.AddRange(new ToolStripItem[] { toolStripLabel11, ComboBoxReportType, LabelType, ComboBoxDepartment, ComboBoxConsultant, toolStripSeparator1, toolStripLabel6, CTReportFromDate, toolStripLabel7, CTReportToDate, toolStripSeparator12, ToolStripBtnGo, ToolStripBtnSave, toolStripSeparator11, ToolStripBtnPrint });
            BerklySoftToolStrip.Location = new Point(0, 0);
            BerklySoftToolStrip.Name = "BerklySoftToolStrip";
            BerklySoftToolStrip.Padding = new Padding(5);
            BerklySoftToolStrip.Size = new Size(1087, 38);
            BerklySoftToolStrip.TabIndex = 1;
            BerklySoftToolStrip.Text = "Berkly Soft ToolStrip";
            // 
            // toolStripLabel11
            // 
            toolStripLabel11.Name = "toolStripLabel11";
            toolStripLabel11.Size = new Size(31, 25);
            toolStripLabel11.Text = "Type";
            // 
            // ComboBoxReportType
            // 
            ComboBoxReportType.FlatStyle = FlatStyle.Standard;
            ComboBoxReportType.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ComboBoxReportType.Items.AddRange(new object[] { "By Date", "By Consultant", "By Department" });
            ComboBoxReportType.Margin = new Padding(0, 1, 0, 2);
            ComboBoxReportType.Name = "ComboBoxReportType";
            ComboBoxReportType.Size = new Size(120, 25);
            ComboBoxReportType.DropDown += ComboBoxReportType_DropDown;
            ComboBoxReportType.DropDownClosed += ComboBoxReportType_DropDownClosed;
            ComboBoxReportType.SelectedIndexChanged += ComboBoxReportType_SelectedIndexChanged;
            ComboBoxReportType.TextUpdate += ComboBoxReportType_TextUpdate;
            ComboBoxReportType.KeyDown += ComboBoxReportType_KeyDown;
            ComboBoxReportType.KeyPress += ComboBoxReportType_KeyPress;
            // 
            // LabelType
            // 
            LabelType.Name = "LabelType";
            LabelType.Size = new Size(65, 25);
            LabelType.Text = "Consultant";
            LabelType.TextAlign = ContentAlignment.MiddleLeft;
            LabelType.Visible = false;
            // 
            // ComboBoxDepartment
            // 
            ComboBoxDepartment.AutoSize = false;
            ComboBoxDepartment.Name = "ComboBoxDepartment";
            ComboBoxDepartment.SelectedNode = null;
            ComboBoxDepartment.Size = new Size(93, 21);
            ComboBoxDepartment.Visible = false;
            ComboBoxDepartment.NodeClickedEvent += ComboBoxDepartment_NodeClickedEvent;
            // 
            // ComboBoxConsultant
            // 
            ComboBoxConsultant.AutoSize = false;
            ComboBoxConsultant.Name = "ComboBoxConsultant";
            ComboBoxConsultant.SelectedNode = null;
            ComboBoxConsultant.Size = new Size(93, 21);
            ComboBoxConsultant.Visible = false;
            ComboBoxConsultant.NodeClickedEvent += ComboBoxConsultant_NodeClickedEvent;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 28);
            // 
            // toolStripLabel6
            // 
            toolStripLabel6.Name = "toolStripLabel6";
            toolStripLabel6.Size = new Size(35, 25);
            toolStripLabel6.Text = "From";
            // 
            // CTReportFromDate
            // 
            CTReportFromDate.BackColor = Color.White;
            CTReportFromDate.Date = null;
            CTReportFromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            CTReportFromDate.Format = "MM/dd/yyyy";
            CTReportFromDate.MaxDate = new DateTime(9997, 12, 31, 4, 17, 51, 0);
            CTReportFromDate.MinDate = new DateTime(1900, 1, 1, 23, 1, 20, 0);
            CTReportFromDate.Name = "CTReportFromDate";
            CTReportFromDate.Size = new Size(97, 25);
            CTReportFromDate.Text = "Calender";
            // 
            // toolStripLabel7
            // 
            toolStripLabel7.Name = "toolStripLabel7";
            toolStripLabel7.Size = new Size(19, 25);
            toolStripLabel7.Text = "To";
            // 
            // CTReportToDate
            // 
            CTReportToDate.BackColor = Color.White;
            CTReportToDate.Date = null;
            CTReportToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            CTReportToDate.Format = "MM/dd/yyyy";
            CTReportToDate.MaxDate = new DateTime(9997, 12, 31, 4, 17, 51, 0);
            CTReportToDate.MinDate = new DateTime(1900, 1, 1, 23, 1, 20, 0);
            CTReportToDate.Name = "CTReportToDate";
            CTReportToDate.Size = new Size(97, 25);
            CTReportToDate.Text = "Calender";
            // 
            // toolStripSeparator12
            // 
            toolStripSeparator12.Name = "toolStripSeparator12";
            toolStripSeparator12.Size = new Size(6, 28);
            // 
            // ToolStripBtnGo
            // 
            ToolStripBtnGo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            ToolStripBtnGo.Image = (Image)resources.GetObject("ToolStripBtnGo.Image");
            ToolStripBtnGo.ImageTransparentColor = Color.Black;
            ToolStripBtnGo.Name = "ToolStripBtnGo";
            ToolStripBtnGo.Size = new Size(26, 25);
            ToolStripBtnGo.Text = "Go";
            ToolStripBtnGo.Click += ToolStripBtnGo_Click;
            // 
            // ToolStripBtnSave
            // 
            ToolStripBtnSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnSave.Image = (Image)resources.GetObject("ToolStripBtnSave.Image");
            ToolStripBtnSave.ImageTransparentColor = Color.Black;
            ToolStripBtnSave.Name = "ToolStripBtnSave";
            ToolStripBtnSave.Size = new Size(23, 25);
            ToolStripBtnSave.Text = "Save";
            ToolStripBtnSave.Click += ToolStripBtnSave_Click;
            // 
            // toolStripSeparator11
            // 
            toolStripSeparator11.Name = "toolStripSeparator11";
            toolStripSeparator11.Size = new Size(6, 28);
            // 
            // ToolStripBtnPrint
            // 
            ToolStripBtnPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnPrint.Image = (Image)resources.GetObject("ToolStripBtnPrint.Image");
            ToolStripBtnPrint.ImageTransparentColor = Color.Black;
            ToolStripBtnPrint.Name = "ToolStripBtnPrint";
            ToolStripBtnPrint.Size = new Size(23, 25);
            ToolStripBtnPrint.Text = "Print";
            ToolStripBtnPrint.Click += ToolStripBtnPrint_Click;
            // 
            // GridviewCareTakerReport
            // 
            GridviewCareTakerReport.AllowUserToAddRows = false;
            GridviewCareTakerReport.AllowUserToDeleteRows = false;
            GridviewCareTakerReport.AllowUserToResizeColumns = false;
            GridviewCareTakerReport.AllowUserToResizeRows = false;
            dataGridViewCellStyle9.BackColor = SystemColors.Control;
            dataGridViewCellStyle9.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle9.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle9.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            GridviewCareTakerReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            GridviewCareTakerReport.ColumnHeadersHeight = 20;
            GridviewCareTakerReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridviewCareTakerReport.Columns.AddRange(new DataGridViewColumn[] { slno, name, age, address, dept, adate, TaskDate, AdmissionId, RowHead });
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = SystemColors.Window;
            dataGridViewCellStyle11.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle11.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.False;
            GridviewCareTakerReport.DefaultCellStyle = dataGridViewCellStyle11;
            GridviewCareTakerReport.EnableHeadersVisualStyles = false;
            GridviewCareTakerReport.Location = new Point(2, 41);
            GridviewCareTakerReport.MultiSelect = false;
            GridviewCareTakerReport.Name = "GridviewCareTakerReport";
            GridviewCareTakerReport.ReadOnly = true;
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = SystemColors.Control;
            dataGridViewCellStyle12.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle12.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = DataGridViewTriState.True;
            GridviewCareTakerReport.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            GridviewCareTakerReport.RowHeadersVisible = false;
            GridviewCareTakerReport.RowTemplate.Height = 20;
            GridviewCareTakerReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridviewCareTakerReport.ShowCellToolTips = false;
            GridviewCareTakerReport.Size = new Size(1082, 374);
            GridviewCareTakerReport.TabIndex = 5;
            GridviewCareTakerReport.CellFormatting += GridviewCareTakerReport_CellFormatting;
            GridviewCareTakerReport.CellPainting += GridviewCareTakerReport_CellPainting;
            GridviewCareTakerReport.RowPostPaint += GridviewCareTakerReport_RowPostPaint;
            // 
            // slno
            // 
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.TopLeft;
            slno.DefaultCellStyle = dataGridViewCellStyle10;
            slno.Frozen = true;
            slno.HeaderText = "#";
            slno.Name = "slno";
            slno.ReadOnly = true;
            slno.Resizable = DataGridViewTriState.False;
            slno.SortMode = DataGridViewColumnSortMode.NotSortable;
            slno.Width = 50;
            // 
            // name
            // 
            name.Frozen = true;
            name.HeaderText = "Name";
            name.Name = "name";
            name.ReadOnly = true;
            name.Resizable = DataGridViewTriState.False;
            name.SortMode = DataGridViewColumnSortMode.NotSortable;
            name.Width = 200;
            // 
            // age
            // 
            age.Frozen = true;
            age.HeaderText = "Age";
            age.Name = "age";
            age.ReadOnly = true;
            age.SortMode = DataGridViewColumnSortMode.NotSortable;
            age.Width = 50;
            // 
            // address
            // 
            address.Frozen = true;
            address.HeaderText = "Address";
            address.Name = "address";
            address.ReadOnly = true;
            address.Resizable = DataGridViewTriState.False;
            address.SortMode = DataGridViewColumnSortMode.NotSortable;
            address.Width = 250;
            // 
            // dept
            // 
            dept.Frozen = true;
            dept.HeaderText = "Department";
            dept.Name = "dept";
            dept.ReadOnly = true;
            dept.Resizable = DataGridViewTriState.False;
            dept.SortMode = DataGridViewColumnSortMode.NotSortable;
            dept.Width = 200;
            // 
            // adate
            // 
            adate.Frozen = true;
            adate.HeaderText = "Task Status";
            adate.Name = "adate";
            adate.ReadOnly = true;
            adate.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // TaskDate
            // 
            TaskDate.HeaderText = "Date";
            TaskDate.Name = "TaskDate";
            TaskDate.ReadOnly = true;
            TaskDate.Width = 200;
            // 
            // AdmissionId
            // 
            AdmissionId.HeaderText = "Admission Id";
            AdmissionId.MinimumWidth = 2;
            AdmissionId.Name = "AdmissionId";
            AdmissionId.ReadOnly = true;
            AdmissionId.SortMode = DataGridViewColumnSortMode.NotSortable;
            AdmissionId.Visible = false;
            AdmissionId.Width = 2;
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
            // BerklySoftStatusStrip
            // 
            BerklySoftStatusStrip.Items.AddRange(new ToolStripItem[] { CareTakerReportErrorMsg });
            BerklySoftStatusStrip.Location = new Point(0, 461);
            BerklySoftStatusStrip.Name = "BerklySoftStatusStrip";
            BerklySoftStatusStrip.Size = new Size(1087, 22);
            BerklySoftStatusStrip.TabIndex = 6;
            BerklySoftStatusStrip.Text = "BerklySoft Status Strip";
            // 
            // CareTakerReportErrorMsg
            // 
            CareTakerReportErrorMsg.Name = "CareTakerReportErrorMsg";
            CareTakerReportErrorMsg.Size = new Size(13, 17);
            CareTakerReportErrorMsg.Text = "  ";
            // 
            // BtnReset
            // 
            BtnReset.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnReset.Location = new Point(754, 428);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new Size(82, 23);
            BtnReset.TabIndex = 32;
            BtnReset.Text = "Reset [Esc]";
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(1004, 428);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(75, 23);
            BtnExit.TabIndex = 31;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(923, 428);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 23);
            BtnPrint.TabIndex = 30;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnSave
            // 
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(842, 428);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 29;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // FormCareTakerUnAssignReport
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1087, 483);
            Controls.Add(BtnReset);
            Controls.Add(BtnExit);
            Controls.Add(BtnPrint);
            Controls.Add(BtnSave);
            Controls.Add(BerklySoftStatusStrip);
            Controls.Add(GridviewCareTakerReport);
            Controls.Add(BerklySoftToolStrip);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormCareTakerUnAssignReport";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Care Taker UnAssign Report";
            Load += FormCareTakerUnAssignReport_Load;
            MouseMove += FormCareTakerUnAssignReport_MouseMove;
            BerklySoftToolStrip.ResumeLayout(false);
            BerklySoftToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridviewCareTakerReport).EndInit();
            BerklySoftStatusStrip.ResumeLayout(false);
            BerklySoftStatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private fa.views.controls.Ab2ToolStrip BerklySoftToolStrip;
        private ToolStripLabel toolStripLabel11;
        private ToolStripComboBox ComboBoxReportType;
        private ToolStripLabel LabelType;
        private fa.views.controls.ToolstripCheckedTreeComboBox ComboBoxDepartment;
        private fa.views.controls.ToolstripCheckedTreeComboBox ComboBoxConsultant;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel toolStripLabel6;
        private fa.views.controls.ToolStripCalendar CTReportFromDate;
        private ToolStripLabel toolStripLabel7;
        private fa.views.controls.ToolStripCalendar CTReportToDate;
        private ToolStripSeparator toolStripSeparator12;
        private ToolStripButton ToolStripBtnGo;
        private ToolStripButton ToolStripBtnSave;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripButton ToolStripBtnPrint;
        private fa.views.controls.DataViewVerticalScroll GridviewCareTakerReport;
        private StatusStrip BerklySoftStatusStrip;
        private ToolStripStatusLabel CareTakerReportErrorMsg;
        private Button BtnReset;
        private Button BtnExit;
        private Button BtnPrint;
        private Button BtnSave;
        private DataGridViewTextBoxColumn slno;
        private DataGridViewTextBoxColumn name;
        private DataGridViewTextBoxColumn age;
        private DataGridViewTextBoxColumn address;
        private DataGridViewTextBoxColumn dept;
        private DataGridViewTextBoxColumn adate;
        private DataGridViewTextBoxColumn TaskDate;
        private DataGridViewTextBoxColumn AdmissionId;
        private DataGridViewTextBoxColumn RowHead;
    }
}