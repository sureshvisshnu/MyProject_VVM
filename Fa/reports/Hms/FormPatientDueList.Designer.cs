namespace Fa.reports.Hms
{
    partial class FormPatientDueList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPatientDueList));
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            ab2ToolStrip1 = new fa.views.controls.Ab2ToolStrip();
            LablePatientType = new ToolStripLabel();
            CheckedTreeComboBoxType = new fa.views.controls.ToolstripCheckedTreeComboBox();
            WardSeparator = new ToolStripSeparator();
            toolStripLabelWard = new ToolStripLabel();
            ComboBoxWard = new fa.views.controls.ToolstripCheckedTreeComboBox();
            PatientSeparator = new ToolStripSeparator();
            toolStripLabelPatient = new ToolStripLabel();
            CheckedListComboBoxPatient = new views.controls.ToolstripCheckedListComboBox();
            DateSeparator = new ToolStripSeparator();
            toolStripLabel6 = new ToolStripLabel();
            PatientDueListFromDate = new fa.views.controls.ToolStripCalendar();
            toolStripLabel7 = new ToolStripLabel();
            PatientDueListToDate = new fa.views.controls.ToolStripCalendar();
            toolStripSeparator7 = new ToolStripSeparator();
            ToolStripBtnGo = new ToolStripButton();
            toolStripSeparator10 = new ToolStripSeparator();
            ToolStripBtnSave = new ToolStripButton();
            toolStripSeparator11 = new ToolStripSeparator();
            ToolStripBtnPrint = new ToolStripButton();
            DataGridViewPatientDueList = new fa.views.controls.DataViewVerticalScroll();
            GridviewSNo = new DataGridViewTextBoxColumn();
            GridViewName = new DataGridViewTextBoxColumn();
            GridViewAge = new DataGridViewTextBoxColumn();
            GridViewPatId = new DataGridViewTextBoxColumn();
            GridViewFeeType = new DataGridViewTextBoxColumn();
            GridviewDescription = new DataGridViewTextBoxColumn();
            GridViewDueAmpount = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            BtnReset = new Button();
            BtnExit = new Button();
            BtnPrint = new Button();
            BtnSave = new Button();
            ErrorMsg = new StatusStrip();
            PatientDueListErrorMsg = new ToolStripStatusLabel();
            ab2ToolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DataGridViewPatientDueList).BeginInit();
            ErrorMsg.SuspendLayout();
            SuspendLayout();
            // 
            // ab2ToolStrip1
            // 
            ab2ToolStrip1.BackColor = SystemColors.ControlLight;
            ab2ToolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            ab2ToolStrip1.Items.AddRange(new ToolStripItem[] { LablePatientType, CheckedTreeComboBoxType, WardSeparator, toolStripLabelWard, ComboBoxWard, PatientSeparator, toolStripLabelPatient, CheckedListComboBoxPatient, DateSeparator, toolStripLabel6, PatientDueListFromDate, toolStripLabel7, PatientDueListToDate, toolStripSeparator7, ToolStripBtnGo, toolStripSeparator10, ToolStripBtnSave, toolStripSeparator11, ToolStripBtnPrint });
            ab2ToolStrip1.Location = new Point(0, 0);
            ab2ToolStrip1.Name = "ab2ToolStrip1";
            ab2ToolStrip1.Padding = new Padding(5);
            ab2ToolStrip1.Size = new Size(1098, 38);
            ab2ToolStrip1.TabIndex = 26;
            ab2ToolStrip1.Text = "ab2ToolStrip1";
            // 
            // LablePatientType
            // 
            LablePatientType.Name = "LablePatientType";
            LablePatientType.Size = new Size(31, 25);
            LablePatientType.Text = "Type";
            // 
            // CheckedTreeComboBoxType
            // 
            CheckedTreeComboBoxType.AutoSize = false;
            CheckedTreeComboBoxType.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            CheckedTreeComboBoxType.Name = "CheckedTreeComboBoxType";
            CheckedTreeComboBoxType.SelectedNode = null;
            CheckedTreeComboBoxType.Size = new Size(154, 21);
            CheckedTreeComboBoxType.NodeClickedEvent += CheckedTreeComboBoxType_NodeClickedEvent;
            // 
            // WardSeparator
            // 
            WardSeparator.Name = "WardSeparator";
            WardSeparator.Size = new Size(6, 28);
            WardSeparator.Visible = false;
            // 
            // toolStripLabelWard
            // 
            toolStripLabelWard.Name = "toolStripLabelWard";
            toolStripLabelWard.Size = new Size(35, 25);
            toolStripLabelWard.Text = "Ward";
            toolStripLabelWard.Visible = false;
            // 
            // ComboBoxWard
            // 
            ComboBoxWard.AutoSize = false;
            ComboBoxWard.Name = "ComboBoxWard";
            ComboBoxWard.SelectedNode = null;
            ComboBoxWard.Size = new Size(190, 21);
            ComboBoxWard.Visible = false;
            ComboBoxWard.NodeClickedEvent += ComboBoxWard_NodeClickedEvent;
            // 
            // PatientSeparator
            // 
            PatientSeparator.Name = "PatientSeparator";
            PatientSeparator.Size = new Size(6, 28);
            PatientSeparator.Visible = false;
            // 
            // toolStripLabelPatient
            // 
            toolStripLabelPatient.Name = "toolStripLabelPatient";
            toolStripLabelPatient.Size = new Size(44, 25);
            toolStripLabelPatient.Text = "Patient";
            toolStripLabelPatient.Visible = false;
            // 
            // CheckedListComboBoxPatient
            // 
            CheckedListComboBoxPatient.AutoSize = false;
            CheckedListComboBoxPatient.Name = "CheckedListComboBoxPatient";
            CheckedListComboBoxPatient.Size = new Size(200, 22);
            CheckedListComboBoxPatient.Visible = false;
            CheckedListComboBoxPatient.ItemCheckedEvent += CheckedListComboBoxPatient_ItemCheckedEvent;
            // 
            // DateSeparator
            // 
            DateSeparator.Name = "DateSeparator";
            DateSeparator.Size = new Size(6, 28);
            // 
            // toolStripLabel6
            // 
            toolStripLabel6.Name = "toolStripLabel6";
            toolStripLabel6.Size = new Size(35, 25);
            toolStripLabel6.Text = "From";
            // 
            // PatientDueListFromDate
            // 
            PatientDueListFromDate.BackColor = Color.White;
            PatientDueListFromDate.Date = null;
            PatientDueListFromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            PatientDueListFromDate.Format = "MM/dd/yyyy";
            PatientDueListFromDate.MaxDate = new DateTime(9997, 12, 31, 9, 8, 46, 0);
            PatientDueListFromDate.MinDate = new DateTime(1900, 1, 1, 23, 1, 20, 0);
            PatientDueListFromDate.Name = "PatientDueListFromDate";
            PatientDueListFromDate.Size = new Size(97, 25);
            PatientDueListFromDate.Text = "Calender";
            // 
            // toolStripLabel7
            // 
            toolStripLabel7.Name = "toolStripLabel7";
            toolStripLabel7.Size = new Size(19, 25);
            toolStripLabel7.Text = "To";
            // 
            // PatientDueListToDate
            // 
            PatientDueListToDate.BackColor = Color.White;
            PatientDueListToDate.Date = null;
            PatientDueListToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            PatientDueListToDate.Format = "MM/dd/yyyy";
            PatientDueListToDate.MaxDate = new DateTime(9997, 12, 31, 9, 8, 46, 0);
            PatientDueListToDate.MinDate = new DateTime(1900, 1, 1, 23, 1, 20, 0);
            PatientDueListToDate.Name = "PatientDueListToDate";
            PatientDueListToDate.Size = new Size(97, 25);
            PatientDueListToDate.Text = "Calender";
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
            // DataGridViewPatientDueList
            // 
            DataGridViewPatientDueList.AllowUserToAddRows = false;
            DataGridViewPatientDueList.AllowUserToDeleteRows = false;
            DataGridViewPatientDueList.AllowUserToResizeColumns = false;
            DataGridViewPatientDueList.AllowUserToResizeRows = false;
            DataGridViewPatientDueList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = SystemColors.Control;
            dataGridViewCellStyle7.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle7.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle7.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            DataGridViewPatientDueList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            DataGridViewPatientDueList.ColumnHeadersHeight = 20;
            DataGridViewPatientDueList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DataGridViewPatientDueList.Columns.AddRange(new DataGridViewColumn[] { GridviewSNo, GridViewName, GridViewAge, GridViewPatId, GridViewFeeType, GridviewDescription, GridViewDueAmpount, Column1 });
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle11.BackColor = SystemColors.Window;
            dataGridViewCellStyle11.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle11.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.False;
            DataGridViewPatientDueList.DefaultCellStyle = dataGridViewCellStyle11;
            DataGridViewPatientDueList.EnableHeadersVisualStyles = false;
            DataGridViewPatientDueList.Location = new Point(1, 38);
            DataGridViewPatientDueList.MultiSelect = false;
            DataGridViewPatientDueList.Name = "DataGridViewPatientDueList";
            DataGridViewPatientDueList.ReadOnly = true;
            DataGridViewPatientDueList.RowHeadersVisible = false;
            dataGridViewCellStyle12.BackColor = Color.White;
            dataGridViewCellStyle12.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle12.ForeColor = Color.Black;
            dataGridViewCellStyle12.SelectionBackColor = Color.White;
            dataGridViewCellStyle12.SelectionForeColor = Color.Black;
            DataGridViewPatientDueList.RowsDefaultCellStyle = dataGridViewCellStyle12;
            DataGridViewPatientDueList.RowTemplate.Height = 20;
            DataGridViewPatientDueList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataGridViewPatientDueList.ShowCellToolTips = false;
            DataGridViewPatientDueList.Size = new Size(1096, 455);
            DataGridViewPatientDueList.TabIndex = 27;
            DataGridViewPatientDueList.CellPainting += DataGridViewPatientDueList_CellPainting;
            DataGridViewPatientDueList.RowPostPaint += DataGridViewPatientDueList_RowPostPaint;
            // 
            // GridviewSNo
            // 
            GridviewSNo.HeaderText = "#";
            GridviewSNo.Name = "GridviewSNo";
            GridviewSNo.ReadOnly = true;
            GridviewSNo.Resizable = DataGridViewTriState.False;
            GridviewSNo.SortMode = DataGridViewColumnSortMode.NotSortable;
            GridviewSNo.Width = 40;
            // 
            // GridViewName
            // 
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            GridViewName.DefaultCellStyle = dataGridViewCellStyle8;
            GridViewName.HeaderText = "Name & Address";
            GridViewName.Name = "GridViewName";
            GridViewName.ReadOnly = true;
            GridViewName.SortMode = DataGridViewColumnSortMode.NotSortable;
            GridViewName.Width = 210;
            // 
            // GridViewAge
            // 
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.TopRight;
            GridViewAge.DefaultCellStyle = dataGridViewCellStyle9;
            GridViewAge.HeaderText = "Age";
            GridViewAge.Name = "GridViewAge";
            GridViewAge.ReadOnly = true;
            GridViewAge.SortMode = DataGridViewColumnSortMode.NotSortable;
            GridViewAge.Width = 75;
            // 
            // GridViewPatId
            // 
            GridViewPatId.HeaderText = "Patient Id";
            GridViewPatId.Name = "GridViewPatId";
            GridViewPatId.ReadOnly = true;
            GridViewPatId.SortMode = DataGridViewColumnSortMode.NotSortable;
            GridViewPatId.Width = 150;
            // 
            // GridViewFeeType
            // 
            GridViewFeeType.HeaderText = "Fee Type";
            GridViewFeeType.Name = "GridViewFeeType";
            GridViewFeeType.ReadOnly = true;
            GridViewFeeType.SortMode = DataGridViewColumnSortMode.NotSortable;
            GridViewFeeType.Width = 200;
            // 
            // GridviewDescription
            // 
            GridviewDescription.HeaderText = "Description";
            GridviewDescription.Name = "GridviewDescription";
            GridviewDescription.ReadOnly = true;
            GridviewDescription.SortMode = DataGridViewColumnSortMode.NotSortable;
            GridviewDescription.Width = 250;
            // 
            // GridViewDueAmpount
            // 
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.TopRight;
            GridViewDueAmpount.DefaultCellStyle = dataGridViewCellStyle10;
            GridViewDueAmpount.HeaderText = "Amount";
            GridViewDueAmpount.Name = "GridViewDueAmpount";
            GridViewDueAmpount.ReadOnly = true;
            GridViewDueAmpount.SortMode = DataGridViewColumnSortMode.NotSortable;
            GridViewDueAmpount.Width = 150;
            // 
            // Column1
            // 
            Column1.HeaderText = "OpAmt";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Visible = false;
            // 
            // BtnReset
            // 
            BtnReset.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnReset.Location = new Point(743, 511);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new Size(82, 21);
            BtnReset.TabIndex = 32;
            BtnReset.Text = "Reset [Esc]";
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(993, 511);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(75, 21);
            BtnExit.TabIndex = 31;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(912, 511);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 21);
            BtnPrint.TabIndex = 30;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnSave
            // 
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(831, 511);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 21);
            BtnSave.TabIndex = 29;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // ErrorMsg
            // 
            ErrorMsg.Items.AddRange(new ToolStripItem[] { PatientDueListErrorMsg });
            ErrorMsg.Location = new Point(0, 546);
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(1098, 22);
            ErrorMsg.TabIndex = 28;
            ErrorMsg.Text = "statusStrip1";
            // 
            // PatientDueListErrorMsg
            // 
            PatientDueListErrorMsg.Name = "PatientDueListErrorMsg";
            PatientDueListErrorMsg.Size = new Size(13, 17);
            PatientDueListErrorMsg.Text = "  ";
            // 
            // FormPatientDueList
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1098, 568);
            Controls.Add(BtnReset);
            Controls.Add(BtnExit);
            Controls.Add(BtnPrint);
            Controls.Add(BtnSave);
            Controls.Add(ErrorMsg);
            Controls.Add(DataGridViewPatientDueList);
            Controls.Add(ab2ToolStrip1);
            Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormPatientDueList";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Patient due list";
            Load += FormPatientDueList_Load;
            ab2ToolStrip1.ResumeLayout(false);
            ab2ToolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DataGridViewPatientDueList).EndInit();
            ErrorMsg.ResumeLayout(false);
            ErrorMsg.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private fa.views.controls.Ab2ToolStrip ab2ToolStrip1;
        private ToolStripLabel toolStripLabel6;
        private fa.views.controls.ToolStripCalendar PatientDueListFromDate;
        private ToolStripLabel toolStripLabel7;
        private fa.views.controls.ToolStripCalendar PatientDueListToDate;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripLabel toolStripLabel9;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripLabel toolStripLabel8;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripButton ToolStripBtnGo;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripButton ToolStripBtnSave;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripButton ToolStripBtnPrint;
        private ToolStripSeparator toolStripSeparatorType;
        private fa.views.controls.DataViewVerticalScroll DataGridViewPatientDueList;
        private Button BtnReset;
        private Button BtnExit;
        private Button BtnPrint;
        private Button BtnSave;
        private StatusStrip ErrorMsg;
        private ToolStripStatusLabel PatientDueListErrorMsg;
        private fa.views.controls.ToolstripCheckedTreeComboBox ComboBoxWard;
        private DataGridViewTextBoxColumn GridviewSNo;
        private DataGridViewTextBoxColumn GridViewName;
        private DataGridViewTextBoxColumn GridViewAge;
        private DataGridViewTextBoxColumn GridViewPatId;
        private DataGridViewTextBoxColumn GridViewFeeType;
        private DataGridViewTextBoxColumn GridviewDescription;
        private DataGridViewTextBoxColumn GridViewDueAmpount;
        private DataGridViewTextBoxColumn Column1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel toolStripLabelWard;
        private fa.views.controls.ToolstripCheckedTreeComboBox CheckedTreeComboBoxType;
        private ToolStripLabel LablePatientType;
        private ToolStripSeparator WardSeparator;
        private ToolStripSeparator PatientSeparator;
        private ToolStripSeparator DateSeparator;
        private ToolStripLabel toolStripLabelPatient;
        private views.controls.ToolstripCheckedListComboBox CheckedListComboBoxPatient;
    }
}