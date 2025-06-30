namespace Fa.reports.Hms
{
    partial class FromDischargePatientDetailReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FromDischargePatientDetailReport));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            ab2ToolStrip1 = new fa.views.controls.Ab2ToolStrip();
            toolStripLabelType = new ToolStripLabel();
            ComboBoxReportType = new ToolStripComboBox();
            LabelType = new ToolStripLabel();
            ComboBoxInsurance = new fa.views.controls.ToolstripCheckedTreeComboBox();
            ComboBoxWard = new fa.views.controls.ToolstripCheckedTreeComboBox();
            ComboBoxDepartment = new fa.views.controls.ToolstripCheckedTreeComboBox();
            ComboBoxConsultant = new fa.views.controls.ToolstripCheckedTreeComboBox();
            toolStripSeparator12 = new ToolStripSeparator();
            toolStripLabel6 = new ToolStripLabel();
            DischargeReportFromDate = new fa.views.controls.ToolStripCalendar();
            toolStripLabel7 = new ToolStripLabel();
            DischargeReportToDate = new fa.views.controls.ToolStripCalendar();
            toolStripSeparator7 = new ToolStripSeparator();
            ToolStripBtnGo = new ToolStripButton();
            toolStripSeparator10 = new ToolStripSeparator();
            ToolStripBtnSave = new ToolStripButton();
            toolStripSeparator11 = new ToolStripSeparator();
            ToolStripBtnPrint = new ToolStripButton();
            DischargeReportDataGridView = new fa.views.controls.DataViewVerticalScroll();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn10 = new DataGridViewTextBoxColumn();
            Shead = new DataGridViewTextBoxColumn();
            ErrorMsg = new StatusStrip();
            DischargeReportErrorMsg = new ToolStripStatusLabel();
            BtnReset = new Button();
            BtnExit = new Button();
            BtnPrint = new Button();
            BtnSave = new Button();
            ab2ToolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DischargeReportDataGridView).BeginInit();
            ErrorMsg.SuspendLayout();
            SuspendLayout();
            // 
            // PatientIdTransport
            // 
            PatientIdTransport.Location = new Point(12, 812);
            PatientIdTransport.Margin = new Padding(3);
            PatientIdTransport.Size = new Size(100, 21);
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(172, 215);
            ProductIdTransport.Margin = new Padding(3);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(172, 189);
            ProductBatchIdTransport.Margin = new Padding(3);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(172, 163);
            AccountIdTransport.Margin = new Padding(3);
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // ab2ToolStrip1
            // 
            ab2ToolStrip1.BackColor = SystemColors.ControlLight;
            ab2ToolStrip1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ab2ToolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            ab2ToolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabelType, ComboBoxReportType, LabelType, ComboBoxInsurance, ComboBoxWard, ComboBoxDepartment, ComboBoxConsultant, toolStripSeparator12, toolStripLabel6, DischargeReportFromDate, toolStripLabel7, DischargeReportToDate, toolStripSeparator7, ToolStripBtnGo, toolStripSeparator10, ToolStripBtnSave, toolStripSeparator11, ToolStripBtnPrint });
            ab2ToolStrip1.Location = new Point(0, 0);
            ab2ToolStrip1.Name = "ab2ToolStrip1";
            ab2ToolStrip1.Padding = new Padding(4);
            ab2ToolStrip1.Size = new Size(1164, 37);
            ab2ToolStrip1.TabIndex = 26;
            ab2ToolStrip1.Text = "ab2ToolStrip1";
            // 
            // toolStripLabelType
            // 
            toolStripLabelType.Name = "toolStripLabelType";
            toolStripLabelType.Size = new Size(31, 26);
            toolStripLabelType.Text = "Type";
            // 
            // ComboBoxReportType
            // 
            ComboBoxReportType.FlatStyle = FlatStyle.Standard;
            ComboBoxReportType.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ComboBoxReportType.Items.AddRange(new object[] { "By Date", "By Consultant", "By Department", "By Insurance", "By Ward" });
            ComboBoxReportType.Name = "ComboBoxReportType";
            ComboBoxReportType.Size = new Size(103, 29);
            ComboBoxReportType.Text = "By Date";
            ComboBoxReportType.SelectedIndexChanged += ComboBoxReportType_SelectedIndexChanged;
            // 
            // LabelType
            // 
            LabelType.Name = "LabelType";
            LabelType.Size = new Size(59, 26);
            LabelType.Text = "Consultant";
            LabelType.TextAlign = ContentAlignment.MiddleLeft;
            LabelType.Visible = false;
            // 
            // ComboBoxInsurance
            // 
            ComboBoxInsurance.AutoSize = false;
            ComboBoxInsurance.Name = "ComboBoxInsurance";
            ComboBoxInsurance.SelectedNode = null;
            ComboBoxInsurance.Size = new Size(3, 21);
            ComboBoxInsurance.Visible = false;
            ComboBoxInsurance.NodeClickedEvent += ComboBoxInsurance_NodeClickedEvent;
            // 
            // ComboBoxWard
            // 
            ComboBoxWard.AutoSize = false;
            ComboBoxWard.Name = "ComboBoxWard";
            ComboBoxWard.SelectedNode = null;
            ComboBoxWard.Size = new Size(234, 26);
            ComboBoxWard.Visible = false;
            ComboBoxWard.NodeClickedEvent += ComboBoxWard_NodeClickedEvent;
            // 
            // ComboBoxDepartment
            // 
            ComboBoxDepartment.AutoSize = false;
            ComboBoxDepartment.Name = "ComboBoxDepartment";
            ComboBoxDepartment.SelectedNode = null;
            ComboBoxDepartment.Size = new Size(139, 26);
            ComboBoxDepartment.Visible = false;
            ComboBoxDepartment.NodeClickedEvent += ComboBoxDepartment_NodeClickedEvent;
            // 
            // ComboBoxConsultant
            // 
            ComboBoxConsultant.AutoSize = false;
            ComboBoxConsultant.Name = "ComboBoxConsultant";
            ComboBoxConsultant.SelectedNode = null;
            ComboBoxConsultant.Size = new Size(3, 21);
            ComboBoxConsultant.Visible = false;
            ComboBoxConsultant.NodeClickedEvent += ComboBoxConsultant_NodeClickedEvent;
            // 
            // toolStripSeparator12
            // 
            toolStripSeparator12.Name = "toolStripSeparator12";
            toolStripSeparator12.Size = new Size(6, 29);
            // 
            // toolStripLabel6
            // 
            toolStripLabel6.Name = "toolStripLabel6";
            toolStripLabel6.Size = new Size(31, 26);
            toolStripLabel6.Text = "From";
            // 
            // DischargeReportFromDate
            // 
            DischargeReportFromDate.BackColor = Color.White;
            DischargeReportFromDate.Date = null;
            DischargeReportFromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DischargeReportFromDate.Format = "MM/dd/yyyy";
            DischargeReportFromDate.MaxDate = new DateTime(9997, 12, 31, 9, 8, 46, 0);
            DischargeReportFromDate.MinDate = new DateTime(1900, 1, 1, 23, 1, 20, 0);
            DischargeReportFromDate.Name = "DischargeReportFromDate";
            DischargeReportFromDate.Size = new Size(97, 26);
            DischargeReportFromDate.Text = "Calender";
            // 
            // toolStripLabel7
            // 
            toolStripLabel7.Name = "toolStripLabel7";
            toolStripLabel7.Size = new Size(19, 26);
            toolStripLabel7.Text = "To";
            // 
            // DischargeReportToDate
            // 
            DischargeReportToDate.BackColor = Color.White;
            DischargeReportToDate.Date = null;
            DischargeReportToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DischargeReportToDate.Format = "MM/dd/yyyy";
            DischargeReportToDate.MaxDate = new DateTime(9997, 12, 31, 9, 8, 46, 0);
            DischargeReportToDate.MinDate = new DateTime(1900, 1, 1, 23, 1, 20, 0);
            DischargeReportToDate.Name = "DischargeReportToDate";
            DischargeReportToDate.Size = new Size(97, 26);
            DischargeReportToDate.Text = "Calender";
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new Size(6, 29);
            // 
            // ToolStripBtnGo
            // 
            ToolStripBtnGo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            ToolStripBtnGo.Image = (Image)resources.GetObject("ToolStripBtnGo.Image");
            ToolStripBtnGo.ImageTransparentColor = Color.Magenta;
            ToolStripBtnGo.Name = "ToolStripBtnGo";
            ToolStripBtnGo.Size = new Size(24, 26);
            ToolStripBtnGo.Text = "Go";
            ToolStripBtnGo.Click += ToolStripBtnGo_Click;
            // 
            // toolStripSeparator10
            // 
            toolStripSeparator10.Name = "toolStripSeparator10";
            toolStripSeparator10.Size = new Size(6, 29);
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
            // toolStripSeparator11
            // 
            toolStripSeparator11.Name = "toolStripSeparator11";
            toolStripSeparator11.Size = new Size(6, 29);
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
            // DischargeReportDataGridView
            // 
            DischargeReportDataGridView.AllowUserToAddRows = false;
            DischargeReportDataGridView.AllowUserToDeleteRows = false;
            DischargeReportDataGridView.AllowUserToResizeColumns = false;
            DischargeReportDataGridView.AllowUserToResizeRows = false;
            DischargeReportDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            DischargeReportDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            DischargeReportDataGridView.ColumnHeadersHeight = 20;
            DischargeReportDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DischargeReportDataGridView.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn7, dataGridViewTextBoxColumn8, dataGridViewTextBoxColumn9, dataGridViewTextBoxColumn10, Shead });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Window;
            dataGridViewCellStyle4.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            DischargeReportDataGridView.DefaultCellStyle = dataGridViewCellStyle4;
            DischargeReportDataGridView.EnableHeadersVisualStyles = false;
            DischargeReportDataGridView.Location = new Point(3, 36);
            DischargeReportDataGridView.Name = "DischargeReportDataGridView";
            DischargeReportDataGridView.ReadOnly = true;
            DischargeReportDataGridView.RowHeadersVisible = false;
            DischargeReportDataGridView.RowTemplate.Height = 20;
            DischargeReportDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DischargeReportDataGridView.ShowCellToolTips = false;
            DischargeReportDataGridView.Size = new Size(1155, 423);
            DischargeReportDataGridView.TabIndex = 27;
            DischargeReportDataGridView.CellFormatting += DischargeReportDataGridView_CellFormatting;
            DischargeReportDataGridView.CellPainting += DischargeReportDataGridView_CellPainting;
            DischargeReportDataGridView.RowPostPaint += DischargeReportDataGridView_RowPostPaint;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Ward";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            dataGridViewTextBoxColumn1.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn1.Width = 130;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Bed";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            dataGridViewTextBoxColumn2.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Patient Id";
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            dataGridViewTextBoxColumn3.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewTextBoxColumn4.HeaderText = "Name & Address";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            dataGridViewTextBoxColumn4.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn4.Width = 188;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "DOB";
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.ReadOnly = true;
            dataGridViewTextBoxColumn5.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn5.Width = 80;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewTextBoxColumn6.HeaderText = "Age";
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.ReadOnly = true;
            dataGridViewTextBoxColumn6.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn6.Width = 50;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.HeaderText = "Admitted On";
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            dataGridViewTextBoxColumn7.ReadOnly = true;
            dataGridViewTextBoxColumn7.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn7.Width = 90;
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewTextBoxColumn8.HeaderText = "Primary Doctor";
            dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            dataGridViewTextBoxColumn8.ReadOnly = true;
            dataGridViewTextBoxColumn8.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn8.Width = 150;
            // 
            // dataGridViewTextBoxColumn9
            // 
            dataGridViewTextBoxColumn9.HeaderText = "Primary Nurse";
            dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            dataGridViewTextBoxColumn9.ReadOnly = true;
            dataGridViewTextBoxColumn9.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn9.Width = 150;
            // 
            // dataGridViewTextBoxColumn10
            // 
            dataGridViewTextBoxColumn10.HeaderText = "Discharged On";
            dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            dataGridViewTextBoxColumn10.ReadOnly = true;
            dataGridViewTextBoxColumn10.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn10.Width = 90;
            // 
            // Shead
            // 
            Shead.HeaderText = "subHead";
            Shead.Name = "Shead";
            Shead.ReadOnly = true;
            Shead.Resizable = DataGridViewTriState.False;
            Shead.SortMode = DataGridViewColumnSortMode.NotSortable;
            Shead.Visible = false;
            // 
            // ErrorMsg
            // 
            ErrorMsg.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ErrorMsg.Items.AddRange(new ToolStripItem[] { DischargeReportErrorMsg });
            ErrorMsg.Location = new Point(0, 506);
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Padding = new Padding(1, 0, 12, 0);
            ErrorMsg.Size = new Size(1164, 22);
            ErrorMsg.TabIndex = 28;
            ErrorMsg.Text = "statusStrip1";
            // 
            // DischargeReportErrorMsg
            // 
            DischargeReportErrorMsg.Name = "DischargeReportErrorMsg";
            DischargeReportErrorMsg.Size = new Size(13, 17);
            DischargeReportErrorMsg.Text = "  ";
            // 
            // BtnReset
            // 
            BtnReset.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnReset.Location = new Point(741, 473);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new Size(96, 23);
            BtnReset.TabIndex = 32;
            BtnReset.Text = "Reset [Esc]";
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(1047, 473);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(96, 23);
            BtnExit.TabIndex = 31;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(945, 473);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(96, 23);
            BtnPrint.TabIndex = 30;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnSave
            // 
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(843, 473);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(96, 23);
            BtnSave.TabIndex = 29;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // FromDischargePatientDetailReport
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1164, 528);
            Controls.Add(BtnReset);
            Controls.Add(BtnExit);
            Controls.Add(BtnPrint);
            Controls.Add(BtnSave);
            Controls.Add(ErrorMsg);
            Controls.Add(ab2ToolStrip1);
            Controls.Add(DischargeReportDataGridView);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FromDischargePatientDetailReport";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Discharge Patients Detail Report";
            Load += FromDischargePatientDetailReport_Load;
            Controls.SetChildIndex(DischargeReportDataGridView, 0);
            Controls.SetChildIndex(ab2ToolStrip1, 0);
            Controls.SetChildIndex(ErrorMsg, 0);
            Controls.SetChildIndex(BtnSave, 0);
            Controls.SetChildIndex(BtnPrint, 0);
            Controls.SetChildIndex(BtnExit, 0);
            Controls.SetChildIndex(BtnReset, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            ab2ToolStrip1.ResumeLayout(false);
            ab2ToolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DischargeReportDataGridView).EndInit();
            ErrorMsg.ResumeLayout(false);
            ErrorMsg.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private fa.views.controls.Ab2ToolStrip ab2ToolStrip1;
        private ToolStripLabel toolStripLabelType;
        private ToolStripComboBox ComboBoxReportType;
        private ToolStripLabel LabelType;
        private fa.views.controls.ToolstripCheckedTreeComboBox ComboBoxInsurance;
        private fa.views.controls.ToolstripCheckedTreeComboBox ComboBoxWard;
        private fa.views.controls.ToolstripCheckedTreeComboBox ComboBoxDepartment;
        private fa.views.controls.ToolstripCheckedTreeComboBox ComboBoxConsultant;
        private ToolStripSeparator toolStripSeparator12;
        private ToolStripLabel toolStripLabel6;
        private fa.views.controls.ToolStripCalendar DischargeReportFromDate;
        private ToolStripLabel toolStripLabel7;
        private fa.views.controls.ToolStripCalendar DischargeReportToDate;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripButton ToolStripBtnGo;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripButton ToolStripBtnSave;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripButton ToolStripBtnPrint;
        private fa.views.controls.DataViewVerticalScroll DischargeReportDataGridView;
        private StatusStrip ErrorMsg;
        private ToolStripStatusLabel DischargeReportErrorMsg;
        private Button BtnReset;
        private Button BtnExit;
        private Button BtnPrint;
        private Button BtnSave;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private DataGridViewTextBoxColumn Shead;
    }
}