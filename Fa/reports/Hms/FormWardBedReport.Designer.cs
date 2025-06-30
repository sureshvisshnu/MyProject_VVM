namespace Fa.reports.Hms
{
    partial class FormWardBedReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWardBedReport));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            ToolStripWarBed = new ToolStrip();
            toolStripLabelWard = new ToolStripLabel();
            ComboBoxWard = new fa.views.controls.ToolstripCheckedTreeComboBox();
            LabelFromDate = new ToolStripLabel();
            WardandBedFromDate = new fa.views.controls.ToolStripCalendar();
            LabelToDate = new ToolStripLabel();
            WardandBedToDate = new fa.views.controls.ToolStripCalendar();
            RunWardBedReportButton = new ToolStripButton();
            ToolStripWardBedReportSave = new ToolStripButton();
            ToolStripWardBedReportPrint = new ToolStripButton();
            gridViewWarBedReport = new fa.views.controls.DataViewVerticalScroll();
            statusStripWardBed = new StatusStrip();
            WardBedReportErrMsg = new ToolStripStatusLabel();
            BtnExit = new Button();
            BtnPrint = new Button();
            BtnSave = new Button();
            BtnReset = new Button();
            Ward = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column9 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            Column7 = new DataGridViewTextBoxColumn();
            Column8 = new DataGridViewTextBoxColumn();
            ToolStripWarBed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridViewWarBedReport).BeginInit();
            statusStripWardBed.SuspendLayout();
            SuspendLayout();
            // 
            // ToolStripWarBed
            // 
            ToolStripWarBed.BackColor = SystemColors.ControlLight;
            ToolStripWarBed.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ToolStripWarBed.GripStyle = ToolStripGripStyle.Hidden;
            ToolStripWarBed.Items.AddRange(new ToolStripItem[] { toolStripLabelWard, ComboBoxWard, LabelFromDate, WardandBedFromDate, LabelToDate, WardandBedToDate, RunWardBedReportButton, ToolStripWardBedReportSave, ToolStripWardBedReportPrint });
            ToolStripWarBed.Location = new Point(0, 0);
            ToolStripWarBed.Name = "ToolStripWarBed";
            ToolStripWarBed.Padding = new Padding(4);
            ToolStripWarBed.Size = new Size(1110, 37);
            ToolStripWarBed.TabIndex = 1;
            ToolStripWarBed.Text = "toolStrip1";
            // 
            // toolStripLabelWard
            // 
            toolStripLabelWard.Name = "toolStripLabelWard";
            toolStripLabelWard.Size = new Size(33, 26);
            toolStripLabelWard.Text = "Ward";
            // 
            // ComboBoxWard
            // 
            ComboBoxWard.AutoSize = false;
            ComboBoxWard.Name = "ComboBoxWard";
            ComboBoxWard.SelectedNode = null;
            ComboBoxWard.Size = new Size(180, 26);
            ComboBoxWard.NodeClickedEvent += ComboBoxWard_NodeClickedEvent;
            // 
            // LabelFromDate
            // 
            LabelFromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelFromDate.Name = "LabelFromDate";
            LabelFromDate.Size = new Size(31, 26);
            LabelFromDate.Text = "From";
            // 
            // WardandBedFromDate
            // 
            WardandBedFromDate.BackColor = Color.White;
            WardandBedFromDate.Date = null;
            WardandBedFromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            WardandBedFromDate.Format = "MM/dd/yyyy";
            WardandBedFromDate.MaxDate = new DateTime(9997, 12, 31, 9, 57, 9, 0);
            WardandBedFromDate.MinDate = new DateTime(1900, 1, 1, 23, 1, 0, 0);
            WardandBedFromDate.Name = "WardandBedFromDate";
            WardandBedFromDate.Size = new Size(97, 26);
            WardandBedFromDate.Text = "Calendar";
            // 
            // LabelToDate
            // 
            LabelToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelToDate.Name = "LabelToDate";
            LabelToDate.Size = new Size(19, 26);
            LabelToDate.Text = "To";
            // 
            // WardandBedToDate
            // 
            WardandBedToDate.BackColor = Color.White;
            WardandBedToDate.Date = null;
            WardandBedToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            WardandBedToDate.Format = "MM/dd/yyyy";
            WardandBedToDate.MaxDate = new DateTime(9997, 12, 31, 9, 57, 9, 0);
            WardandBedToDate.MinDate = new DateTime(1900, 1, 1, 23, 1, 0, 0);
            WardandBedToDate.Name = "WardandBedToDate";
            WardandBedToDate.Size = new Size(97, 26);
            WardandBedToDate.Text = "Calendar";
            // 
            // RunWardBedReportButton
            // 
            RunWardBedReportButton.AutoToolTip = false;
            RunWardBedReportButton.BackgroundImageLayout = ImageLayout.None;
            RunWardBedReportButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            RunWardBedReportButton.ImageTransparentColor = Color.Magenta;
            RunWardBedReportButton.Name = "RunWardBedReportButton";
            RunWardBedReportButton.Size = new Size(24, 26);
            RunWardBedReportButton.Text = "Go";
            RunWardBedReportButton.Click += RunWardBedReportButton_Click;
            // 
            // ToolStripWardBedReportSave
            // 
            ToolStripWardBedReportSave.AutoToolTip = false;
            ToolStripWardBedReportSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripWardBedReportSave.Image = (Image)resources.GetObject("ToolStripWardBedReportSave.Image");
            ToolStripWardBedReportSave.ImageTransparentColor = Color.Black;
            ToolStripWardBedReportSave.Name = "ToolStripWardBedReportSave";
            ToolStripWardBedReportSave.Size = new Size(23, 26);
            ToolStripWardBedReportSave.Text = "Save";
            ToolStripWardBedReportSave.Click += BtnSave_Click;
            // 
            // ToolStripWardBedReportPrint
            // 
            ToolStripWardBedReportPrint.AutoToolTip = false;
            ToolStripWardBedReportPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripWardBedReportPrint.Image = (Image)resources.GetObject("ToolStripWardBedReportPrint.Image");
            ToolStripWardBedReportPrint.ImageTransparentColor = Color.Black;
            ToolStripWardBedReportPrint.Name = "ToolStripWardBedReportPrint";
            ToolStripWardBedReportPrint.Size = new Size(23, 26);
            ToolStripWardBedReportPrint.Text = "Print";
            ToolStripWardBedReportPrint.Click += BtnPrint_Click;
            // 
            // gridViewWarBedReport
            // 
            gridViewWarBedReport.AllowUserToAddRows = false;
            gridViewWarBedReport.AllowUserToDeleteRows = false;
            gridViewWarBedReport.AllowUserToResizeColumns = false;
            gridViewWarBedReport.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            gridViewWarBedReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            gridViewWarBedReport.ColumnHeadersHeight = 20;
            gridViewWarBedReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            gridViewWarBedReport.Columns.AddRange(new DataGridViewColumn[] { Ward, Column3, Column2, Column9, Column4, Column5, Column6, Column7, Column8 });
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = SystemColors.Window;
            dataGridViewCellStyle11.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle11.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle11.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.False;
            gridViewWarBedReport.DefaultCellStyle = dataGridViewCellStyle11;
            gridViewWarBedReport.EnableHeadersVisualStyles = false;
            gridViewWarBedReport.Location = new Point(3, 39);
            gridViewWarBedReport.Name = "gridViewWarBedReport";
            gridViewWarBedReport.ReadOnly = true;
            gridViewWarBedReport.RowHeadersVisible = false;
            gridViewWarBedReport.RowTemplate.Height = 20;
            gridViewWarBedReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridViewWarBedReport.ShowCellToolTips = false;
            gridViewWarBedReport.Size = new Size(1104, 482);
            gridViewWarBedReport.TabIndex = 2;
            // 
            // statusStripWardBed
            // 
            statusStripWardBed.Items.AddRange(new ToolStripItem[] { WardBedReportErrMsg });
            statusStripWardBed.Location = new Point(0, 561);
            statusStripWardBed.Name = "statusStripWardBed";
            statusStripWardBed.Size = new Size(1110, 22);
            statusStripWardBed.TabIndex = 3;
            statusStripWardBed.Text = "statusStrip1";
            // 
            // WardBedReportErrMsg
            // 
            WardBedReportErrMsg.Name = "WardBedReportErrMsg";
            WardBedReportErrMsg.Size = new Size(28, 17);
            WardBedReportErrMsg.Text = "       ";
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(1011, 529);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(72, 23);
            BtnExit.TabIndex = 4;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Enabled = false;
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(930, 529);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 23);
            BtnPrint.TabIndex = 5;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnSave
            // 
            BtnSave.Enabled = false;
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(849, 529);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 6;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // BtnReset
            // 
            BtnReset.Enabled = false;
            BtnReset.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnReset.Location = new Point(757, 529);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new Size(86, 23);
            BtnReset.TabIndex = 6;
            BtnReset.Text = "Reset [Esc]";
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // Ward
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            Ward.DefaultCellStyle = dataGridViewCellStyle2;
            Ward.HeaderText = "Ward";
            Ward.Name = "Ward";
            Ward.ReadOnly = true;
            Ward.Resizable = DataGridViewTriState.False;
            Ward.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            Column3.DefaultCellStyle = dataGridViewCellStyle3;
            Column3.HeaderText = "Bed Name";
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            Column3.Resizable = DataGridViewTriState.False;
            Column3.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column3.Width = 120;
            // 
            // Column2
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            Column2.DefaultCellStyle = dataGridViewCellStyle4;
            Column2.HeaderText = "Bed Type";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Resizable = DataGridViewTriState.False;
            Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column2.Width = 130;
            // 
            // Column9
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            Column9.DefaultCellStyle = dataGridViewCellStyle5;
            Column9.HeaderText = "Rent Period";
            Column9.Name = "Column9";
            Column9.ReadOnly = true;
            Column9.Resizable = DataGridViewTriState.False;
            Column9.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column9.Width = 140;
            // 
            // Column4
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            Column4.DefaultCellStyle = dataGridViewCellStyle6;
            Column4.HeaderText = "Rent";
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            Column4.Resizable = DataGridViewTriState.False;
            Column4.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column5
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            Column5.DefaultCellStyle = dataGridViewCellStyle7;
            Column5.HeaderText = "Admitted On";
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            Column5.Resizable = DataGridViewTriState.False;
            Column5.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column5.Width = 130;
            // 
            // Column6
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            Column6.DefaultCellStyle = dataGridViewCellStyle8;
            Column6.HeaderText = "IP Number";
            Column6.Name = "Column6";
            Column6.ReadOnly = true;
            Column6.Resizable = DataGridViewTriState.False;
            Column6.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column6.Width = 130;
            // 
            // Column7
            // 
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            Column7.DefaultCellStyle = dataGridViewCellStyle9;
            Column7.HeaderText = "Patient Name";
            Column7.Name = "Column7";
            Column7.ReadOnly = true;
            Column7.Resizable = DataGridViewTriState.False;
            Column7.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column7.Width = 150;
            // 
            // Column8
            // 
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.True;
            Column8.DefaultCellStyle = dataGridViewCellStyle10;
            Column8.HeaderText = "Available";
            Column8.Name = "Column8";
            Column8.ReadOnly = true;
            Column8.Resizable = DataGridViewTriState.False;
            Column8.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column8.Width = 85;
            // 
            // FormWardBedReport
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1110, 583);
            Controls.Add(BtnReset);
            Controls.Add(BtnSave);
            Controls.Add(BtnPrint);
            Controls.Add(BtnExit);
            Controls.Add(statusStripWardBed);
            Controls.Add(gridViewWarBedReport);
            Controls.Add(ToolStripWarBed);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormWardBedReport";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Ward and Bed Report";
            Load += FormWardBedReport_Load;
            ToolStripWarBed.ResumeLayout(false);
            ToolStripWarBed.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridViewWarBedReport).EndInit();
            statusStripWardBed.ResumeLayout(false);
            statusStripWardBed.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip ToolStripWarBed;
        private fa.views.controls.DataViewVerticalScroll gridViewWarBedReport;
        private StatusStrip statusStripWardBed;
        private Button BtnExit;
        private Button BtnPrint;
        private Button BtnSave;
        private ToolStripLabel toolStripLabelWard;
        private ToolStripLabel LabelFromDate;
        private fa.views.controls.ToolStripCalendar WardandBedFromDate;
        private ToolStripLabel LabelToDate;
        private fa.views.controls.ToolStripCalendar WardandBedToDate;
        private ToolStripButton RunWardBedReportButton;
        private ToolStripButton ToolStripWardBedReportSave;
        private ToolStripButton ToolStripWardBedReportPrint;
        private Button BtnReset;
        private DataGridViewTextBoxColumn Date;
        private DataGridViewTextBoxColumn Column1;
        private ToolStripStatusLabel WardBedReportErrMsg;
        private DataGridViewTextBoxColumn Ward;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column9;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column6;
        private DataGridViewTextBoxColumn Column7;
        private DataGridViewTextBoxColumn Column8;
        private fa.views.controls.ToolstripCheckedTreeComboBox ComboBoxWard;
    }
}