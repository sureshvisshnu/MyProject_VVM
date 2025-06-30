namespace Fa.reports.Hms
{
    partial class FormPatientProcedureReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPatientProcedureReport));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            ab2ToolStripProRpt = new fa.views.controls.Ab2ToolStrip();
            ProRptFromLable = new ToolStripLabel();
            ProcedureRptFromDate = new fa.views.controls.ToolStripCalendar();
            ProRptToLable = new ToolStripLabel();
            ProcedureRptToDate = new fa.views.controls.ToolStripCalendar();
            ProcedureRptBtnGo = new ToolStripButton();
            ToolStripBtnSave = new ToolStripButton();
            ToolStripBtnPrint = new ToolStripButton();
            ProcedureRptToolStrip = new StatusStrip();
            ProcedureRptErrMsg = new ToolStripStatusLabel();
            GridviewProcedureRpt = new fa.views.controls.DataViewVerticalScroll();
            ProRptSno = new DataGridViewTextBoxColumn();
            ProRptDate = new DataGridViewTextBoxColumn();
            ProRptPatientId = new DataGridViewTextBoxColumn();
            ProRptPatient = new DataGridViewTextBoxColumn();
            ProRptProName = new DataGridViewTextBoxColumn();
            ProRptDoctor = new DataGridViewTextBoxColumn();
            ProRptNurse = new DataGridViewTextBoxColumn();
            ProRptStatus = new DataGridViewTextBoxColumn();
            buttonCancel = new Button();
            buttonPrint = new Button();
            buttonSave = new Button();
            buttonExit = new Button();
            ab2ToolStripProRpt.SuspendLayout();
            ProcedureRptToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridviewProcedureRpt).BeginInit();
            SuspendLayout();
            // 
            // ab2ToolStripProRpt
            // 
            ab2ToolStripProRpt.BackColor = SystemColors.ControlLight;
            ab2ToolStripProRpt.GripStyle = ToolStripGripStyle.Hidden;
            ab2ToolStripProRpt.Items.AddRange(new ToolStripItem[] { ProRptFromLable, ProcedureRptFromDate, ProRptToLable, ProcedureRptToDate, ProcedureRptBtnGo, ToolStripBtnSave, ToolStripBtnPrint });
            ab2ToolStripProRpt.Location = new Point(0, 0);
            ab2ToolStripProRpt.Name = "ab2ToolStripProRpt";
            ab2ToolStripProRpt.Padding = new Padding(5);
            ab2ToolStripProRpt.Size = new Size(1132, 38);
            ab2ToolStripProRpt.TabIndex = 0;
            ab2ToolStripProRpt.Text = "ab2ToolStrip1";
            // 
            // ProRptFromLable
            // 
            ProRptFromLable.Name = "ProRptFromLable";
            ProRptFromLable.Size = new Size(35, 25);
            ProRptFromLable.Text = "From";
            // 
            // ProcedureRptFromDate
            // 
            ProcedureRptFromDate.BackColor = Color.White;
            ProcedureRptFromDate.Date = null;
            ProcedureRptFromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ProcedureRptFromDate.Format = "MM/dd/yyyy";
            ProcedureRptFromDate.MaxDate = new DateTime(9997, 12, 31, 9, 8, 46, 0);
            ProcedureRptFromDate.MinDate = new DateTime(1900, 1, 1, 23, 1, 20, 0);
            ProcedureRptFromDate.Name = "ProcedureRptFromDate";
            ProcedureRptFromDate.Size = new Size(97, 25);
            ProcedureRptFromDate.Text = "Calender";
            // 
            // ProRptToLable
            // 
            ProRptToLable.Name = "ProRptToLable";
            ProRptToLable.Size = new Size(19, 25);
            ProRptToLable.Text = "To";
            // 
            // ProcedureRptToDate
            // 
            ProcedureRptToDate.BackColor = Color.White;
            ProcedureRptToDate.Date = null;
            ProcedureRptToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ProcedureRptToDate.Format = "MM/dd/yyyy";
            ProcedureRptToDate.MaxDate = new DateTime(9997, 12, 31, 9, 8, 46, 0);
            ProcedureRptToDate.MinDate = new DateTime(1900, 1, 1, 23, 1, 20, 0);
            ProcedureRptToDate.Name = "ProcedureRptToDate";
            ProcedureRptToDate.Size = new Size(97, 25);
            ProcedureRptToDate.Text = "Calender";
            // 
            // ProcedureRptBtnGo
            // 
            ProcedureRptBtnGo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            ProcedureRptBtnGo.Image = (Image)resources.GetObject("ProcedureRptBtnGo.Image");
            ProcedureRptBtnGo.ImageTransparentColor = Color.Magenta;
            ProcedureRptBtnGo.Name = "ProcedureRptBtnGo";
            ProcedureRptBtnGo.Size = new Size(26, 25);
            ProcedureRptBtnGo.Text = "Go";
            ProcedureRptBtnGo.Click += ProcedureRptBtnGo_Click;
            // 
            // ToolStripBtnSave
            // 
            ToolStripBtnSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnSave.Image = (Image)resources.GetObject("ToolStripBtnSave.Image");
            ToolStripBtnSave.ImageTransparentColor = Color.Black;
            ToolStripBtnSave.Name = "ToolStripBtnSave";
            ToolStripBtnSave.Size = new Size(23, 25);
            ToolStripBtnSave.Text = "Save";
            ToolStripBtnSave.Click += buttonSave_Click;
            // 
            // ToolStripBtnPrint
            // 
            ToolStripBtnPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnPrint.Image = (Image)resources.GetObject("ToolStripBtnPrint.Image");
            ToolStripBtnPrint.ImageTransparentColor = Color.Black;
            ToolStripBtnPrint.Name = "ToolStripBtnPrint";
            ToolStripBtnPrint.Size = new Size(23, 25);
            ToolStripBtnPrint.Text = "Print";
            ToolStripBtnPrint.Click += buttonPrint_Click;
            // 
            // ProcedureRptToolStrip
            // 
            ProcedureRptToolStrip.Items.AddRange(new ToolStripItem[] { ProcedureRptErrMsg });
            ProcedureRptToolStrip.Location = new Point(0, 489);
            ProcedureRptToolStrip.Name = "ProcedureRptToolStrip";
            ProcedureRptToolStrip.Size = new Size(1132, 22);
            ProcedureRptToolStrip.TabIndex = 1;
            ProcedureRptToolStrip.Text = "statusStrip1";
            // 
            // ProcedureRptErrMsg
            // 
            ProcedureRptErrMsg.Name = "ProcedureRptErrMsg";
            ProcedureRptErrMsg.Size = new Size(25, 17);
            ProcedureRptErrMsg.Text = "      ";
            // 
            // GridviewProcedureRpt
            // 
            GridviewProcedureRpt.AllowUserToAddRows = false;
            GridviewProcedureRpt.AllowUserToDeleteRows = false;
            GridviewProcedureRpt.AllowUserToResizeColumns = false;
            GridviewProcedureRpt.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridviewProcedureRpt.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridviewProcedureRpt.ColumnHeadersHeight = 20;
            GridviewProcedureRpt.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridviewProcedureRpt.Columns.AddRange(new DataGridViewColumn[] { ProRptSno, ProRptDate, ProRptPatientId, ProRptPatient, ProRptProName, ProRptDoctor, ProRptNurse, ProRptStatus });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            GridviewProcedureRpt.DefaultCellStyle = dataGridViewCellStyle2;
            GridviewProcedureRpt.EnableHeadersVisualStyles = false;
            GridviewProcedureRpt.Location = new Point(1, 40);
            GridviewProcedureRpt.MultiSelect = false;
            GridviewProcedureRpt.Name = "GridviewProcedureRpt";
            GridviewProcedureRpt.ReadOnly = true;
            GridviewProcedureRpt.RowHeadersVisible = false;
            GridviewProcedureRpt.RowTemplate.Height = 20;
            GridviewProcedureRpt.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridviewProcedureRpt.ShowCellToolTips = false;
            GridviewProcedureRpt.Size = new Size(1130, 395);
            GridviewProcedureRpt.TabIndex = 2;
            // 
            // ProRptSno
            // 
            ProRptSno.HeaderText = "#";
            ProRptSno.Name = "ProRptSno";
            ProRptSno.ReadOnly = true;
            ProRptSno.Resizable = DataGridViewTriState.False;
            ProRptSno.SortMode = DataGridViewColumnSortMode.NotSortable;
            ProRptSno.Width = 40;
            // 
            // ProRptDate
            // 
            ProRptDate.HeaderText = "Date";
            ProRptDate.Name = "ProRptDate";
            ProRptDate.ReadOnly = true;
            ProRptDate.Resizable = DataGridViewTriState.False;
            ProRptDate.SortMode = DataGridViewColumnSortMode.NotSortable;
            ProRptDate.Width = 120;
            // 
            // ProRptPatientId
            // 
            ProRptPatientId.HeaderText = "PatientId";
            ProRptPatientId.Name = "ProRptPatientId";
            ProRptPatientId.ReadOnly = true;
            ProRptPatientId.Resizable = DataGridViewTriState.False;
            ProRptPatientId.SortMode = DataGridViewColumnSortMode.NotSortable;
            ProRptPatientId.Width = 160;
            // 
            // ProRptPatient
            // 
            ProRptPatient.HeaderText = "Patient";
            ProRptPatient.Name = "ProRptPatient";
            ProRptPatient.ReadOnly = true;
            ProRptPatient.Resizable = DataGridViewTriState.False;
            ProRptPatient.SortMode = DataGridViewColumnSortMode.NotSortable;
            ProRptPatient.Width = 160;
            // 
            // ProRptProName
            // 
            ProRptProName.HeaderText = "Procedure Name";
            ProRptProName.Name = "ProRptProName";
            ProRptProName.ReadOnly = true;
            ProRptProName.Resizable = DataGridViewTriState.False;
            ProRptProName.SortMode = DataGridViewColumnSortMode.NotSortable;
            ProRptProName.Width = 160;
            // 
            // ProRptDoctor
            // 
            ProRptDoctor.HeaderText = "Doctor";
            ProRptDoctor.Name = "ProRptDoctor";
            ProRptDoctor.ReadOnly = true;
            ProRptDoctor.Resizable = DataGridViewTriState.False;
            ProRptDoctor.SortMode = DataGridViewColumnSortMode.NotSortable;
            ProRptDoctor.Width = 150;
            // 
            // ProRptNurse
            // 
            ProRptNurse.HeaderText = "Nurse";
            ProRptNurse.Name = "ProRptNurse";
            ProRptNurse.ReadOnly = true;
            ProRptNurse.Resizable = DataGridViewTriState.False;
            ProRptNurse.SortMode = DataGridViewColumnSortMode.NotSortable;
            ProRptNurse.Width = 150;
            // 
            // ProRptStatus
            // 
            ProRptStatus.HeaderText = "Status";
            ProRptStatus.Name = "ProRptStatus";
            ProRptStatus.ReadOnly = true;
            ProRptStatus.Resizable = DataGridViewTriState.False;
            ProRptStatus.SortMode = DataGridViewColumnSortMode.NotSortable;
            ProRptStatus.Width = 170;
            // 
            // buttonCancel
            // 
            buttonCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonCancel.Location = new Point(797, 451);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(80, 23);
            buttonCancel.TabIndex = 3;
            buttonCancel.Text = "Reset [Esc]";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonPrint
            // 
            buttonPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonPrint.Location = new Point(883, 451);
            buttonPrint.Name = "buttonPrint";
            buttonPrint.Size = new Size(75, 23);
            buttonPrint.TabIndex = 4;
            buttonPrint.Text = "Print [F9]";
            buttonPrint.UseVisualStyleBackColor = true;
            buttonPrint.Click += buttonPrint_Click;
            // 
            // buttonSave
            // 
            buttonSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonSave.Location = new Point(964, 451);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 23);
            buttonSave.TabIndex = 5;
            buttonSave.Text = "Save [F8]";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // buttonExit
            // 
            buttonExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonExit.Location = new Point(1045, 451);
            buttonExit.Name = "buttonExit";
            buttonExit.Size = new Size(75, 23);
            buttonExit.TabIndex = 6;
            buttonExit.Text = "Exit [F10]";
            buttonExit.UseVisualStyleBackColor = true;
            buttonExit.Click += buttonExit_Click;
            // 
            // FormPatientProcedureReport
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1132, 511);
            Controls.Add(buttonExit);
            Controls.Add(buttonSave);
            Controls.Add(buttonPrint);
            Controls.Add(buttonCancel);
            Controls.Add(GridviewProcedureRpt);
            Controls.Add(ProcedureRptToolStrip);
            Controls.Add(ab2ToolStripProRpt);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormPatientProcedureReport";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Patient Procedure Report";
            Load += FormPatientProcedureReport_Load;
            Click += FormPatientProcedureReport_Load;
            ab2ToolStripProRpt.ResumeLayout(false);
            ab2ToolStripProRpt.PerformLayout();
            ProcedureRptToolStrip.ResumeLayout(false);
            ProcedureRptToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridviewProcedureRpt).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private fa.views.controls.Ab2ToolStrip ab2ToolStripProRpt;
        private ToolStripLabel ProRptFromLable;
        private fa.views.controls.ToolStripCalendar ProcedureRptFromDate;
        private ToolStripLabel ProRptToLable;
        private fa.views.controls.ToolStripCalendar ProcedureRptToDate;
        private ToolStripButton ProcedureRptBtnGo;
        private ToolStripButton ToolStripBtnSave;
        private ToolStripButton ToolStripBtnPrint;
        private StatusStrip ProcedureRptToolStrip;
        private ToolStripStatusLabel ProcedureRptErrMsg;
        private fa.views.controls.DataViewVerticalScroll GridviewProcedureRpt;
        private Button buttonCancel;
        private Button buttonPrint;
        private Button buttonSave;
        private Button buttonExit;
        private DataGridViewTextBoxColumn ProRptSno;
        private DataGridViewTextBoxColumn ProRptDate;
        private DataGridViewTextBoxColumn ProRptPatientId;
        private DataGridViewTextBoxColumn ProRptPatient;
        private DataGridViewTextBoxColumn ProRptProName;
        private DataGridViewTextBoxColumn ProRptDoctor;
        private DataGridViewTextBoxColumn ProRptNurse;
        private DataGridViewTextBoxColumn ProRptStatus;
    }
}