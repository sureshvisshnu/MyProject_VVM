namespace Fa.reports.Hms
{
    partial class FormPatientDetailsReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPatientDetailsReport));
            DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle27 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle28 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle16 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle17 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle18 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle19 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle20 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle21 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle22 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle23 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle24 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle25 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle26 = new DataGridViewCellStyle();
            toolStripPatientDetails = new ToolStrip();
            LableType = new ToolStripLabel();
            ComboBoxType = new ToolStripComboBox();
            WardSeparator = new ToolStripSeparator();
            LabelFrom = new ToolStripLabel();
            PatientDetailsFromDate = new fa.views.controls.ToolStripCalendar();
            LabelTo = new ToolStripLabel();
            PatientDetailsToDate = new fa.views.controls.ToolStripCalendar();
            toolStripSeparator7 = new ToolStripSeparator();
            BtnGo = new ToolStripButton();
            toolStripSeparator10 = new ToolStripSeparator();
            ToolStripBtnSave = new ToolStripButton();
            toolStripSeparator11 = new ToolStripSeparator();
            ToolStripBtnPrint = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            statusStripPatientDetails = new StatusStrip();
            PatientDetailsErrMsg = new ToolStripStatusLabel();
            GridviewPatientDetails = new fa.views.controls.DataViewVerticalScroll();
            SNo = new DataGridViewTextBoxColumn();
            PatientName = new DataGridViewTextBoxColumn();
            PatientID = new DataGridViewTextBoxColumn();
            Gender = new DataGridViewTextBoxColumn();
            DoB = new DataGridViewTextBoxColumn();
            Age = new DataGridViewTextBoxColumn();
            BloodGroup = new DataGridViewTextBoxColumn();
            PAddress = new DataGridViewTextBoxColumn();
            Phone = new DataGridViewTextBoxColumn();
            GuardianDetails = new DataGridViewTextBoxColumn();
            EmergencyDetails = new DataGridViewTextBoxColumn();
            Date = new DataGridViewTextBoxColumn();
            BtnReset = new Button();
            BtnExit = new Button();
            BtnPrint = new Button();
            BtnSave = new Button();
            toolStripPatientDetails.SuspendLayout();
            statusStripPatientDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridviewPatientDetails).BeginInit();
            SuspendLayout();
            // 
            // toolStripPatientDetails
            // 
            toolStripPatientDetails.AutoSize = false;
            toolStripPatientDetails.BackColor = SystemColors.ControlLight;
            toolStripPatientDetails.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripPatientDetails.Items.AddRange(new ToolStripItem[] { LableType, ComboBoxType, WardSeparator, LabelFrom, PatientDetailsFromDate, LabelTo, PatientDetailsToDate, toolStripSeparator7, BtnGo, toolStripSeparator10, ToolStripBtnSave, toolStripSeparator11, ToolStripBtnPrint, toolStripSeparator1 });
            toolStripPatientDetails.Location = new Point(0, 0);
            toolStripPatientDetails.Name = "toolStripPatientDetails";
            toolStripPatientDetails.ShowItemToolTips = false;
            toolStripPatientDetails.Size = new Size(1236, 30);
            toolStripPatientDetails.TabIndex = 0;
            // 
            // LableType
            // 
            LableType.Name = "LableType";
            LableType.Size = new Size(31, 27);
            LableType.Text = "Type";
            // 
            // ComboBoxType
            // 
            ComboBoxType.CausesValidation = false;
            ComboBoxType.FlatStyle = FlatStyle.Standard;
            ComboBoxType.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ComboBoxType.Items.AddRange(new object[] { "By Date" });
            ComboBoxType.Margin = new Padding(0, 1, 0, 2);
            ComboBoxType.Name = "ComboBoxType";
            ComboBoxType.Size = new Size(130, 27);
            ComboBoxType.Text = "By Date";
            // 
            // WardSeparator
            // 
            WardSeparator.Name = "WardSeparator";
            WardSeparator.Size = new Size(6, 30);
            // 
            // LabelFrom
            // 
            LabelFrom.Name = "LabelFrom";
            LabelFrom.Size = new Size(31, 27);
            LabelFrom.Text = "From";
            // 
            // PatientDetailsFromDate
            // 
            PatientDetailsFromDate.BackColor = Color.White;
            PatientDetailsFromDate.Date = null;
            PatientDetailsFromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            PatientDetailsFromDate.Format = "MM/dd/yyyy";
            PatientDetailsFromDate.MaxDate = new DateTime(9997, 12, 31, 9, 8, 46, 0);
            PatientDetailsFromDate.MinDate = new DateTime(1900, 1, 1, 23, 1, 20, 0);
            PatientDetailsFromDate.Name = "PatientDetailsFromDate";
            PatientDetailsFromDate.Size = new Size(97, 27);
            PatientDetailsFromDate.Text = "Calender";
            // 
            // LabelTo
            // 
            LabelTo.Name = "LabelTo";
            LabelTo.Size = new Size(19, 27);
            LabelTo.Text = "To";
            // 
            // PatientDetailsToDate
            // 
            PatientDetailsToDate.BackColor = Color.White;
            PatientDetailsToDate.Date = null;
            PatientDetailsToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            PatientDetailsToDate.Format = "MM/dd/yyyy";
            PatientDetailsToDate.MaxDate = new DateTime(9997, 12, 31, 9, 8, 46, 0);
            PatientDetailsToDate.MinDate = new DateTime(1900, 1, 1, 23, 1, 20, 0);
            PatientDetailsToDate.Name = "PatientDetailsToDate";
            PatientDetailsToDate.Size = new Size(97, 27);
            PatientDetailsToDate.Text = "Calender";
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new Size(6, 30);
            // 
            // BtnGo
            // 
            BtnGo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnGo.Image = (Image)resources.GetObject("BtnGo.Image");
            BtnGo.ImageTransparentColor = Color.Magenta;
            BtnGo.Name = "BtnGo";
            BtnGo.Size = new Size(24, 27);
            BtnGo.Text = "Go";
            BtnGo.Click += BtnGo_Click;
            // 
            // toolStripSeparator10
            // 
            toolStripSeparator10.Name = "toolStripSeparator10";
            toolStripSeparator10.Size = new Size(6, 30);
            // 
            // ToolStripBtnSave
            // 
            ToolStripBtnSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnSave.Enabled = false;
            ToolStripBtnSave.Image = (Image)resources.GetObject("ToolStripBtnSave.Image");
            ToolStripBtnSave.ImageTransparentColor = Color.Black;
            ToolStripBtnSave.Name = "ToolStripBtnSave";
            ToolStripBtnSave.Size = new Size(23, 27);
            ToolStripBtnSave.Text = "Save";
            ToolStripBtnSave.Click += BtnSave_Click;
            // 
            // toolStripSeparator11
            // 
            toolStripSeparator11.Name = "toolStripSeparator11";
            toolStripSeparator11.Size = new Size(6, 30);
            // 
            // ToolStripBtnPrint
            // 
            ToolStripBtnPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnPrint.Enabled = false;
            ToolStripBtnPrint.Image = (Image)resources.GetObject("ToolStripBtnPrint.Image");
            ToolStripBtnPrint.ImageTransparentColor = Color.Black;
            ToolStripBtnPrint.Name = "ToolStripBtnPrint";
            ToolStripBtnPrint.Size = new Size(23, 27);
            ToolStripBtnPrint.Text = "Print";
            ToolStripBtnPrint.Click += BtnPrint_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 30);
            // 
            // statusStripPatientDetails
            // 
            statusStripPatientDetails.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            statusStripPatientDetails.Items.AddRange(new ToolStripItem[] { PatientDetailsErrMsg });
            statusStripPatientDetails.Location = new Point(0, 613);
            statusStripPatientDetails.Name = "statusStripPatientDetails";
            statusStripPatientDetails.Padding = new Padding(1, 0, 12, 0);
            statusStripPatientDetails.Size = new Size(1236, 22);
            statusStripPatientDetails.TabIndex = 1;
            // 
            // PatientDetailsErrMsg
            // 
            PatientDetailsErrMsg.BackColor = SystemColors.Control;
            PatientDetailsErrMsg.Name = "PatientDetailsErrMsg";
            PatientDetailsErrMsg.Size = new Size(23, 15);
            PatientDetailsErrMsg.Text = "    ";
            // 
            // GridviewPatientDetails
            // 
            GridviewPatientDetails.AllowUserToAddRows = false;
            GridviewPatientDetails.AllowUserToDeleteRows = false;
            GridviewPatientDetails.AllowUserToResizeColumns = false;
            GridviewPatientDetails.AllowUserToResizeRows = false;
            GridviewPatientDetails.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dataGridViewCellStyle15.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = SystemColors.Control;
            dataGridViewCellStyle15.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle15.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle15.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle15.WrapMode = DataGridViewTriState.True;
            GridviewPatientDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle15;
            GridviewPatientDetails.ColumnHeadersHeight = 20;
            GridviewPatientDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridviewPatientDetails.Columns.AddRange(new DataGridViewColumn[] { SNo, PatientName, PatientID, Gender, DoB, Age, BloodGroup, PAddress, Phone, GuardianDetails, EmergencyDetails, Date });
            dataGridViewCellStyle27.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle27.BackColor = SystemColors.Window;
            dataGridViewCellStyle27.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle27.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle27.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle27.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle27.WrapMode = DataGridViewTriState.True;
            GridviewPatientDetails.DefaultCellStyle = dataGridViewCellStyle27;
            GridviewPatientDetails.EnableHeadersVisualStyles = false;
            GridviewPatientDetails.Location = new Point(3, 31);
            GridviewPatientDetails.MultiSelect = false;
            GridviewPatientDetails.Name = "GridviewPatientDetails";
            GridviewPatientDetails.ReadOnly = true;
            GridviewPatientDetails.RowHeadersVisible = false;
            dataGridViewCellStyle28.WrapMode = DataGridViewTriState.True;
            GridviewPatientDetails.RowsDefaultCellStyle = dataGridViewCellStyle28;
            GridviewPatientDetails.RowTemplate.Height = 25;
            GridviewPatientDetails.ScrollBars = ScrollBars.Vertical;
            GridviewPatientDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridviewPatientDetails.ShowCellToolTips = false;
            GridviewPatientDetails.Size = new Size(1231, 533);
            GridviewPatientDetails.TabIndex = 2;
            GridviewPatientDetails.CellPainting += GridviewPatientDetails_CellPainting;
            GridviewPatientDetails.RowPostPaint += GridviewPatientDetails_RowPostPaint;
            // 
            // SNo
            // 
            dataGridViewCellStyle16.Alignment = DataGridViewContentAlignment.TopLeft;
            SNo.DefaultCellStyle = dataGridViewCellStyle16;
            SNo.HeaderText = "#";
            SNo.Name = "SNo";
            SNo.ReadOnly = true;
            SNo.Resizable = DataGridViewTriState.False;
            SNo.SortMode = DataGridViewColumnSortMode.NotSortable;
            SNo.Width = 40;
            // 
            // PatientName
            // 
            dataGridViewCellStyle17.Alignment = DataGridViewContentAlignment.TopLeft;
            PatientName.DefaultCellStyle = dataGridViewCellStyle17;
            PatientName.HeaderText = "Patient Name";
            PatientName.Name = "PatientName";
            PatientName.ReadOnly = true;
            PatientName.Resizable = DataGridViewTriState.False;
            PatientName.SortMode = DataGridViewColumnSortMode.NotSortable;
            PatientName.Width = 170;
            // 
            // PatientID
            // 
            dataGridViewCellStyle18.Alignment = DataGridViewContentAlignment.TopLeft;
            PatientID.DefaultCellStyle = dataGridViewCellStyle18;
            PatientID.HeaderText = "Patient ID";
            PatientID.Name = "PatientID";
            PatientID.ReadOnly = true;
            PatientID.Resizable = DataGridViewTriState.False;
            PatientID.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Gender
            // 
            dataGridViewCellStyle19.Alignment = DataGridViewContentAlignment.TopLeft;
            Gender.DefaultCellStyle = dataGridViewCellStyle19;
            Gender.HeaderText = "Gender";
            Gender.Name = "Gender";
            Gender.ReadOnly = true;
            Gender.Resizable = DataGridViewTriState.False;
            Gender.SortMode = DataGridViewColumnSortMode.NotSortable;
            Gender.Width = 75;
            // 
            // DoB
            // 
            dataGridViewCellStyle20.Alignment = DataGridViewContentAlignment.TopLeft;
            DoB.DefaultCellStyle = dataGridViewCellStyle20;
            DoB.HeaderText = "DOB";
            DoB.Name = "DoB";
            DoB.ReadOnly = true;
            DoB.Resizable = DataGridViewTriState.False;
            DoB.SortMode = DataGridViewColumnSortMode.NotSortable;
            DoB.Width = 80;
            // 
            // Age
            // 
            dataGridViewCellStyle21.Alignment = DataGridViewContentAlignment.TopRight;
            Age.DefaultCellStyle = dataGridViewCellStyle21;
            Age.HeaderText = "Age";
            Age.Name = "Age";
            Age.ReadOnly = true;
            Age.Resizable = DataGridViewTriState.False;
            Age.SortMode = DataGridViewColumnSortMode.NotSortable;
            Age.Width = 35;
            // 
            // BloodGroup
            // 
            dataGridViewCellStyle22.Alignment = DataGridViewContentAlignment.TopLeft;
            BloodGroup.DefaultCellStyle = dataGridViewCellStyle22;
            BloodGroup.HeaderText = "Blood Group";
            BloodGroup.Name = "BloodGroup";
            BloodGroup.ReadOnly = true;
            BloodGroup.Resizable = DataGridViewTriState.False;
            BloodGroup.SortMode = DataGridViewColumnSortMode.NotSortable;
            BloodGroup.Width = 80;
            // 
            // PAddress
            // 
            dataGridViewCellStyle23.Alignment = DataGridViewContentAlignment.TopLeft;
            PAddress.DefaultCellStyle = dataGridViewCellStyle23;
            PAddress.HeaderText = "Address";
            PAddress.Name = "PAddress";
            PAddress.ReadOnly = true;
            PAddress.Resizable = DataGridViewTriState.False;
            PAddress.SortMode = DataGridViewColumnSortMode.NotSortable;
            PAddress.Width = 175;
            // 
            // Phone
            // 
            dataGridViewCellStyle24.Alignment = DataGridViewContentAlignment.TopLeft;
            Phone.DefaultCellStyle = dataGridViewCellStyle24;
            Phone.HeaderText = "Phone/Mobile";
            Phone.Name = "Phone";
            Phone.ReadOnly = true;
            Phone.Resizable = DataGridViewTriState.False;
            Phone.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // GuardianDetails
            // 
            dataGridViewCellStyle25.Alignment = DataGridViewContentAlignment.TopLeft;
            GuardianDetails.DefaultCellStyle = dataGridViewCellStyle25;
            GuardianDetails.HeaderText = "Guardian Details";
            GuardianDetails.Name = "GuardianDetails";
            GuardianDetails.ReadOnly = true;
            GuardianDetails.Resizable = DataGridViewTriState.False;
            GuardianDetails.SortMode = DataGridViewColumnSortMode.NotSortable;
            GuardianDetails.Width = 180;
            // 
            // EmergencyDetails
            // 
            dataGridViewCellStyle26.Alignment = DataGridViewContentAlignment.TopLeft;
            EmergencyDetails.DefaultCellStyle = dataGridViewCellStyle26;
            EmergencyDetails.HeaderText = "Emergency Details";
            EmergencyDetails.Name = "EmergencyDetails";
            EmergencyDetails.ReadOnly = true;
            EmergencyDetails.Resizable = DataGridViewTriState.False;
            EmergencyDetails.SortMode = DataGridViewColumnSortMode.NotSortable;
            EmergencyDetails.Width = 176;
            // 
            // Date
            // 
            Date.HeaderText = "Date";
            Date.Name = "Date";
            Date.ReadOnly = true;
            Date.Resizable = DataGridViewTriState.False;
            Date.SortMode = DataGridViewColumnSortMode.NotSortable;
            Date.Visible = false;
            // 
            // BtnReset
            // 
            BtnReset.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnReset.Location = new Point(893, 579);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new Size(81, 23);
            BtnReset.TabIndex = 40;
            BtnReset.Text = "Reset [Esc]";
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(1148, 579);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(74, 23);
            BtnExit.TabIndex = 39;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Enabled = false;
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(1067, 579);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 23);
            BtnPrint.TabIndex = 38;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnSave
            // 
            BtnSave.Enabled = false;
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(980, 579);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(81, 23);
            BtnSave.TabIndex = 37;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // FormPatientDetailsReport
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1236, 635);
            Controls.Add(BtnReset);
            Controls.Add(BtnExit);
            Controls.Add(BtnPrint);
            Controls.Add(BtnSave);
            Controls.Add(GridviewPatientDetails);
            Controls.Add(statusStripPatientDetails);
            Controls.Add(toolStripPatientDetails);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormPatientDetailsReport";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Patient Details Report";
            Load += FormPatientDetailsReport_Load;
            toolStripPatientDetails.ResumeLayout(false);
            toolStripPatientDetails.PerformLayout();
            statusStripPatientDetails.ResumeLayout(false);
            statusStripPatientDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridviewPatientDetails).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStripPatientDetails;
        private StatusStrip statusStripPatientDetails;
        private ToolStripStatusLabel PatientDetailsErrMsg;
        private ToolStripLabel LableType;
        private ToolStripSeparator WardSeparator;
        private ToolStripLabel LabelFrom;
        private fa.views.controls.ToolStripCalendar PatientDetailsFromDate;
        private ToolStripLabel LabelTo;
        private fa.views.controls.ToolStripCalendar PatientDetailsToDate;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripButton BtnGo;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripButton ToolStripBtnSave;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripButton ToolStripBtnPrint;
        private ToolStripSeparator toolStripSeparator1;
        private fa.views.controls.DataViewVerticalScroll GridviewPatientDetails;
        private ToolStripComboBox ComboBoxType;
        private Button BtnReset;
        private Button BtnExit;
        private Button BtnPrint;
        private Button BtnSave;
        private DataGridViewTextBoxColumn SNo;
        private DataGridViewTextBoxColumn PatientName;
        private DataGridViewTextBoxColumn PatientID;
        private DataGridViewTextBoxColumn Gender;
        private DataGridViewTextBoxColumn DoB;
        private DataGridViewTextBoxColumn Age;
        private DataGridViewTextBoxColumn BloodGroup;
        private DataGridViewTextBoxColumn PAddress;
        private DataGridViewTextBoxColumn Phone;
        private DataGridViewTextBoxColumn GuardianDetails;
        private DataGridViewTextBoxColumn EmergencyDetails;
        private DataGridViewTextBoxColumn Date;
    }
}