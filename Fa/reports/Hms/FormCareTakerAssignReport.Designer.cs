namespace Fa.reports.Hms
{
    partial class FormCareTakerAssignReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCareTakerAssignReport));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            BerklySoftToolStrip = new fa.views.controls.Ab2ToolStrip();
            toolStripLabel11 = new ToolStripLabel();
            ComboBoxReportType = new ToolStripComboBox();
            LabelType = new ToolStripLabel();
            ComboBoxDepartment = new fa.views.controls.ToolstripCheckedTreeComboBox();
            ComboBoxNurse = new fa.views.controls.ToolstripCheckedTreeComboBox();
            ComboBoxTechnician = new fa.views.controls.ToolstripCheckedTreeComboBox();
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
            BerklySoftStatusStrip = new StatusStrip();
            CareTakerReportErrorMsg = new ToolStripStatusLabel();
            GridviewCareTakerReport = new fa.views.controls.DataViewVerticalScroll();
            CTFrom = new DataGridViewTextBoxColumn();
            CTTo = new DataGridViewTextBoxColumn();
            Dept = new DataGridViewTextBoxColumn();
            CTPrimaryDr = new DataGridViewTextBoxColumn();
            CTSecondryDr = new DataGridViewTextBoxColumn();
            PNurse = new DataGridViewTextBoxColumn();
            SNurse = new DataGridViewTextBoxColumn();
            CTNotes = new DataGridViewTextBoxColumn();
            CTAuthorize = new DataGridViewTextBoxColumn();
            AdmissionId = new DataGridViewTextBoxColumn();
            RowHead = new DataGridViewTextBoxColumn();
            BtnReset = new Button();
            BtnExit = new Button();
            BtnPrint = new Button();
            BtnSave = new Button();
            BerklySoftToolStrip.SuspendLayout();
            BerklySoftStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridviewCareTakerReport).BeginInit();
            SuspendLayout();
            // 
            // BerklySoftToolStrip
            // 
            BerklySoftToolStrip.BackColor = SystemColors.ControlLight;
            BerklySoftToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            BerklySoftToolStrip.Items.AddRange(new ToolStripItem[] { toolStripLabel11, ComboBoxReportType, LabelType, ComboBoxDepartment, ComboBoxNurse, ComboBoxTechnician, ComboBoxConsultant, toolStripSeparator1, toolStripLabel6, CTReportFromDate, toolStripLabel7, CTReportToDate, toolStripSeparator12, ToolStripBtnGo, ToolStripBtnSave, toolStripSeparator11, ToolStripBtnPrint });
            BerklySoftToolStrip.Location = new Point(0, 0);
            BerklySoftToolStrip.Name = "BerklySoftToolStrip";
            BerklySoftToolStrip.Padding = new Padding(5);
            BerklySoftToolStrip.Size = new Size(1340, 38);
            BerklySoftToolStrip.TabIndex = 0;
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
            ComboBoxReportType.Items.AddRange(new object[] { "By Date", "By Consultant", "By Nurse", "By Technician", "By Department" });
            ComboBoxReportType.Name = "ComboBoxReportType";
            ComboBoxReportType.Size = new Size(120, 28);
            ComboBoxReportType.DropDown += ComboBoxReportType_DropDown;
            ComboBoxReportType.DropDownClosed += ComboBoxReportType_DropDownClosed;
            ComboBoxReportType.SelectedIndexChanged += ComboBoxReportType_SelectedIndexChanged;
            ComboBoxReportType.TextChanged += ComboBoxReportType_TextChanged;
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
            // ComboBoxNurse
            // 
            ComboBoxNurse.AutoSize = false;
            ComboBoxNurse.Name = "ComboBoxNurse";
            ComboBoxNurse.SelectedNode = null;
            ComboBoxNurse.Size = new Size(93, 21);
            ComboBoxNurse.Visible = false;
            ComboBoxNurse.NodeClickedEvent += ComboBoxNurse_NodeClickedEvent;
            // 
            // ComboBoxTechnician
            // 
            ComboBoxTechnician.AutoSize = false;
            ComboBoxTechnician.Name = "ComboBoxTechnician";
            ComboBoxTechnician.SelectedNode = null;
            ComboBoxTechnician.Size = new Size(93, 21);
            ComboBoxTechnician.Visible = false;
            ComboBoxTechnician.NodeClickedEvent += ComboBoxTechnician_NodeClickedEvent;
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
            CTReportFromDate.MaxDate = new DateTime(9997, 12, 31, 9, 8, 46, 0);
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
            CTReportToDate.MaxDate = new DateTime(9997, 12, 31, 9, 8, 46, 0);
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
            ToolStripBtnPrint.Image = (Image)resources.GetObject("ToolStripBtnPrint.Image");
            ToolStripBtnPrint.ImageTransparentColor = Color.Black;
            ToolStripBtnPrint.Name = "ToolStripBtnPrint";
            ToolStripBtnPrint.Size = new Size(23, 25);
            ToolStripBtnPrint.Text = "Print";
            ToolStripBtnPrint.Click += BtnPrint_Click;
            // 
            // BerklySoftStatusStrip
            // 
            BerklySoftStatusStrip.Items.AddRange(new ToolStripItem[] { CareTakerReportErrorMsg });
            BerklySoftStatusStrip.Location = new Point(0, 561);
            BerklySoftStatusStrip.Name = "BerklySoftStatusStrip";
            BerklySoftStatusStrip.Size = new Size(1340, 22);
            BerklySoftStatusStrip.TabIndex = 1;
            BerklySoftStatusStrip.Text = "BerklySoft Status Strip";
            // 
            // CareTakerReportErrorMsg
            // 
            CareTakerReportErrorMsg.Name = "CareTakerReportErrorMsg";
            CareTakerReportErrorMsg.Size = new Size(13, 17);
            CareTakerReportErrorMsg.Text = "  ";
            // 
            // GridviewCareTakerReport
            // 
            GridviewCareTakerReport.AllowUserToAddRows = false;
            GridviewCareTakerReport.AllowUserToDeleteRows = false;
            GridviewCareTakerReport.AllowUserToResizeColumns = false;
            GridviewCareTakerReport.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridviewCareTakerReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridviewCareTakerReport.ColumnHeadersHeight = 20;
            GridviewCareTakerReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridviewCareTakerReport.Columns.AddRange(new DataGridViewColumn[] { CTFrom, CTTo, Dept, CTPrimaryDr, CTSecondryDr, PNurse, SNurse, CTNotes, CTAuthorize, AdmissionId, RowHead });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Window;
            dataGridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            GridviewCareTakerReport.DefaultCellStyle = dataGridViewCellStyle4;
            GridviewCareTakerReport.EnableHeadersVisualStyles = false;
            GridviewCareTakerReport.Location = new Point(2, 41);
            GridviewCareTakerReport.MultiSelect = false;
            GridviewCareTakerReport.Name = "GridviewCareTakerReport";
            GridviewCareTakerReport.ReadOnly = true;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Control;
            dataGridViewCellStyle5.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle5.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            GridviewCareTakerReport.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            GridviewCareTakerReport.RowHeadersVisible = false;
            GridviewCareTakerReport.RowTemplate.Height = 20;
            GridviewCareTakerReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridviewCareTakerReport.ShowCellToolTips = false;
            GridviewCareTakerReport.Size = new Size(1335, 474);
            GridviewCareTakerReport.TabIndex = 4;
            GridviewCareTakerReport.CellFormatting += GridviewCareTakerReport_CellFormatting;
            GridviewCareTakerReport.CellPainting += GridviewCareTakerReport_CellPainting;
            GridviewCareTakerReport.RowPostPaint += GridviewCareTakerReport_RowPostPaint;
            // 
            // CTFrom
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            CTFrom.DefaultCellStyle = dataGridViewCellStyle2;
            CTFrom.HeaderText = "From";
            CTFrom.Name = "CTFrom";
            CTFrom.ReadOnly = true;
            CTFrom.Resizable = DataGridViewTriState.False;
            CTFrom.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // CTTo
            // 
            CTTo.HeaderText = "To";
            CTTo.Name = "CTTo";
            CTTo.ReadOnly = true;
            CTTo.Resizable = DataGridViewTriState.False;
            CTTo.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Dept
            // 
            Dept.HeaderText = "Department";
            Dept.Name = "Dept";
            Dept.ReadOnly = true;
            Dept.SortMode = DataGridViewColumnSortMode.NotSortable;
            Dept.Width = 135;
            // 
            // CTPrimaryDr
            // 
            CTPrimaryDr.HeaderText = "Primary Doctor";
            CTPrimaryDr.Name = "CTPrimaryDr";
            CTPrimaryDr.ReadOnly = true;
            CTPrimaryDr.Resizable = DataGridViewTriState.False;
            CTPrimaryDr.SortMode = DataGridViewColumnSortMode.NotSortable;
            CTPrimaryDr.Width = 160;
            // 
            // CTSecondryDr
            // 
            CTSecondryDr.HeaderText = "Secondary Doctor";
            CTSecondryDr.Name = "CTSecondryDr";
            CTSecondryDr.ReadOnly = true;
            CTSecondryDr.Resizable = DataGridViewTriState.False;
            CTSecondryDr.SortMode = DataGridViewColumnSortMode.NotSortable;
            CTSecondryDr.Width = 160;
            // 
            // PNurse
            // 
            PNurse.HeaderText = "Primary Nurse";
            PNurse.Name = "PNurse";
            PNurse.ReadOnly = true;
            PNurse.SortMode = DataGridViewColumnSortMode.NotSortable;
            PNurse.Width = 160;
            // 
            // SNurse
            // 
            SNurse.HeaderText = "Secondary Nurse";
            SNurse.Name = "SNurse";
            SNurse.ReadOnly = true;
            SNurse.SortMode = DataGridViewColumnSortMode.NotSortable;
            SNurse.Width = 160;
            // 
            // CTNotes
            // 
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            CTNotes.DefaultCellStyle = dataGridViewCellStyle3;
            CTNotes.HeaderText = "Notes";
            CTNotes.Name = "CTNotes";
            CTNotes.ReadOnly = true;
            CTNotes.Resizable = DataGridViewTriState.False;
            CTNotes.SortMode = DataGridViewColumnSortMode.NotSortable;
            CTNotes.Width = 180;
            // 
            // CTAuthorize
            // 
            CTAuthorize.HeaderText = "Authorized By";
            CTAuthorize.Name = "CTAuthorize";
            CTAuthorize.ReadOnly = true;
            CTAuthorize.SortMode = DataGridViewColumnSortMode.NotSortable;
            CTAuthorize.Width = 160;
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
            // BtnReset
            // 
            BtnReset.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnReset.Location = new Point(1002, 527);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new Size(82, 23);
            BtnReset.TabIndex = 28;
            BtnReset.Text = "Reset [Esc]";
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(1252, 527);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(75, 23);
            BtnExit.TabIndex = 27;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(1171, 527);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 23);
            BtnPrint.TabIndex = 26;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnSave
            // 
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(1090, 527);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 25;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // FormCareTakerAssignReport
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1340, 583);
            Controls.Add(BtnReset);
            Controls.Add(BtnExit);
            Controls.Add(BtnPrint);
            Controls.Add(BtnSave);
            Controls.Add(BerklySoftStatusStrip);
            Controls.Add(BerklySoftToolStrip);
            Controls.Add(GridviewCareTakerReport);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormCareTakerAssignReport";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Care Taker Assign Report";
            Load += FormCareTakerAssignReport_Load;
            BerklySoftToolStrip.ResumeLayout(false);
            BerklySoftToolStrip.PerformLayout();
            BerklySoftStatusStrip.ResumeLayout(false);
            BerklySoftStatusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridviewCareTakerReport).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private fa.views.controls.Ab2ToolStrip BerklySoftToolStrip;
        private ToolStripLabel toolStripLabel11;
        private ToolStripComboBox ComboBoxReportType;
        private ToolStripLabel LabelType;
        private fa.views.controls.ToolstripCheckedTreeComboBox ComboBoxConsultant;
        private fa.views.controls.ToolstripCheckedTreeComboBox ComboBoxDepartment;
        private ToolStripSeparator toolStripSeparator12;
        private ToolStripButton ToolStripBtnGo;
        private ToolStripButton ToolStripBtnSave;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripButton ToolStripBtnPrint;
        private StatusStrip BerklySoftStatusStrip;
        private ToolStripStatusLabel CareTakerReportErrorMsg;
        private fa.views.controls.DataViewVerticalScroll GridviewCareTakerReport;
        private Button BtnReset;
        private Button BtnExit;
        private Button BtnPrint;
        private Button BtnSave;
        private fa.views.controls.ToolStripCalendar CTReportToDate;
        private ToolStripLabel toolStripLabel7;
        private fa.views.controls.ToolStripCalendar CTReportFromDate;
        private ToolStripLabel toolStripLabel6;
        private ToolStripSeparator toolStripSeparator1;
        private DataGridViewTextBoxColumn CTFrom;
        private DataGridViewTextBoxColumn CTTo;
        private DataGridViewTextBoxColumn Dept;
        private DataGridViewTextBoxColumn CTPrimaryDr;
        private DataGridViewTextBoxColumn CTSecondryDr;
        private DataGridViewTextBoxColumn PNurse;
        private DataGridViewTextBoxColumn SNurse;
        private DataGridViewTextBoxColumn CTNotes;
        private DataGridViewTextBoxColumn CTAuthorize;
        private DataGridViewTextBoxColumn AdmissionId;
        private DataGridViewTextBoxColumn RowHead;
        private fa.views.controls.ToolstripCheckedTreeComboBox ComboBoxNurse;
        private fa.views.controls.ToolstripCheckedTreeComboBox ComboBoxTechnician;
    }
}