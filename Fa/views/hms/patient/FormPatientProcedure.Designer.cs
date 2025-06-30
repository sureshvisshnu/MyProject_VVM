namespace fa.views.hms.patient
{
    partial class FormPatientProcedure
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPatientProcedure));
            groupBox1 = new GroupBox();
            TextBoxStatus = new TextBox();
            label6 = new Label();
            TextBoxPatientProcedureRequestOn = new TextBox();
            label5 = new Label();
            TextBoxPatientProcedureRequestBy = new TextBox();
            label4 = new Label();
            TextBoxPatientProcedureDisc = new TextBox();
            label3 = new Label();
            TextBoxPatientProcedureName = new TextBox();
            label1 = new Label();
            label2 = new Label();
            PatientProcedureInfo = new controls.hms.PatientInfoMin();
            statusStrip1 = new StatusStrip();
            ErrorMsgProcedure = new ToolStripStatusLabel();
            DataGridViewPatientProcedure = new controls.DataViewVerticalScroll();
            SerialNo = new DataGridViewTextBoxColumn();
            Date = new controls.grid.DataGridViewCalendarColumn();
            PerformedBy = new DataGridViewTextBoxColumn();
            Note = new DataGridViewTextBoxColumn();
            Status = new DataGridViewComboBoxColumn();
            Remove = new DataGridViewTextBoxColumn();
            Id = new DataGridViewTextBoxColumn();
            BtnExit = new Button();
            BtnCancel = new Button();
            BtnSave = new Button();
            groupBox1.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DataGridViewPatientProcedure).BeginInit();
            SuspendLayout();
            // 
            // PatientIdTransport
            // 
            PatientIdTransport.Size = new Size(100, 21);
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(108, 220);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(108, 193);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(108, 166);
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // groupBox1
            // 
            groupBox1.BackColor = SystemColors.Control;
            groupBox1.Controls.Add(TextBoxStatus);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(TextBoxPatientProcedureRequestOn);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(TextBoxPatientProcedureRequestBy);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(TextBoxPatientProcedureDisc);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(TextBoxPatientProcedureName);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(203, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(800, 175);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Procedure Details";
            // 
            // TextBoxStatus
            // 
            TextBoxStatus.BackColor = Color.White;
            TextBoxStatus.Location = new Point(432, 117);
            TextBoxStatus.Name = "TextBoxStatus";
            TextBoxStatus.ReadOnly = true;
            TextBoxStatus.Size = new Size(121, 21);
            TextBoxStatus.TabIndex = 26;
            TextBoxStatus.TabStop = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(431, 101);
            label6.Name = "label6";
            label6.Size = new Size(38, 13);
            label6.TabIndex = 8;
            label6.Text = "Status";
            // 
            // TextBoxPatientProcedureRequestOn
            // 
            TextBoxPatientProcedureRequestOn.BackColor = Color.White;
            TextBoxPatientProcedureRequestOn.Location = new Point(432, 76);
            TextBoxPatientProcedureRequestOn.Name = "TextBoxPatientProcedureRequestOn";
            TextBoxPatientProcedureRequestOn.ReadOnly = true;
            TextBoxPatientProcedureRequestOn.Size = new Size(344, 21);
            TextBoxPatientProcedureRequestOn.TabIndex = 7;
            TextBoxPatientProcedureRequestOn.TabStop = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(432, 60);
            label5.Name = "label5";
            label5.Size = new Size(76, 13);
            label5.TabIndex = 6;
            label5.Text = "Requested On";
            // 
            // TextBoxPatientProcedureRequestBy
            // 
            TextBoxPatientProcedureRequestBy.BackColor = Color.White;
            TextBoxPatientProcedureRequestBy.Location = new Point(432, 35);
            TextBoxPatientProcedureRequestBy.Name = "TextBoxPatientProcedureRequestBy";
            TextBoxPatientProcedureRequestBy.ReadOnly = true;
            TextBoxPatientProcedureRequestBy.Size = new Size(344, 21);
            TextBoxPatientProcedureRequestBy.TabIndex = 5;
            TextBoxPatientProcedureRequestBy.TabStop = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(432, 19);
            label4.Name = "label4";
            label4.Size = new Size(74, 13);
            label4.TabIndex = 4;
            label4.Text = "Requested By";
            // 
            // TextBoxPatientProcedureDisc
            // 
            TextBoxPatientProcedureDisc.BackColor = Color.White;
            TextBoxPatientProcedureDisc.Location = new Point(11, 76);
            TextBoxPatientProcedureDisc.MaxLength = 250;
            TextBoxPatientProcedureDisc.Multiline = true;
            TextBoxPatientProcedureDisc.Name = "TextBoxPatientProcedureDisc";
            TextBoxPatientProcedureDisc.ReadOnly = true;
            TextBoxPatientProcedureDisc.Size = new Size(406, 89);
            TextBoxPatientProcedureDisc.TabIndex = 3;
            TextBoxPatientProcedureDisc.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(11, 60);
            label3.Name = "label3";
            label3.Size = new Size(60, 13);
            label3.TabIndex = 2;
            label3.Text = "Description";
            // 
            // TextBoxPatientProcedureName
            // 
            TextBoxPatientProcedureName.BackColor = Color.White;
            TextBoxPatientProcedureName.Location = new Point(11, 35);
            TextBoxPatientProcedureName.Name = "TextBoxPatientProcedureName";
            TextBoxPatientProcedureName.ReadOnly = true;
            TextBoxPatientProcedureName.Size = new Size(406, 21);
            TextBoxPatientProcedureName.TabIndex = 1;
            TextBoxPatientProcedureName.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(11, 19);
            label1.Name = "label1";
            label1.Size = new Size(34, 13);
            label1.TabIndex = 0;
            label1.Text = "Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(215, 190);
            label2.Name = "label2";
            label2.Size = new Size(49, 13);
            label2.TabIndex = 4;
            label2.Text = "Progress";
            // 
            // PatientProcedureInfo
            // 
            PatientProcedureInfo.AutoSize = true;
            PatientProcedureInfo.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            PatientProcedureInfo.Location = new Point(1, 0);
            PatientProcedureInfo.Margin = new Padding(4, 3, 4, 3);
            PatientProcedureInfo.Name = "PatientProcedureInfo";
            PatientProcedureInfo.PatientId = null;
            PatientProcedureInfo.Short = false;
            PatientProcedureInfo.Size = new Size(197, 643);
            PatientProcedureInfo.TabIndex = 0;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsgProcedure });
            statusStrip1.Location = new Point(0, 572);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1013, 22);
            statusStrip1.TabIndex = 67;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsgProcedure
            // 
            ErrorMsgProcedure.Name = "ErrorMsgProcedure";
            ErrorMsgProcedure.Size = new Size(19, 17);
            ErrorMsgProcedure.Text = "    ";
            // 
            // DataGridViewPatientProcedure
            // 
            DataGridViewPatientProcedure.AllowUserToDeleteRows = false;
            DataGridViewPatientProcedure.AllowUserToResizeColumns = false;
            DataGridViewPatientProcedure.AllowUserToResizeRows = false;
            DataGridViewPatientProcedure.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            DataGridViewPatientProcedure.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            DataGridViewPatientProcedure.ColumnHeadersHeight = 20;
            DataGridViewPatientProcedure.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DataGridViewPatientProcedure.Columns.AddRange(new DataGridViewColumn[] { SerialNo, Date, PerformedBy, Note, Status, Remove, Id });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = Color.White;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            DataGridViewPatientProcedure.DefaultCellStyle = dataGridViewCellStyle3;
            DataGridViewPatientProcedure.EditMode = DataGridViewEditMode.EditOnEnter;
            DataGridViewPatientProcedure.EnableHeadersVisualStyles = false;
            DataGridViewPatientProcedure.GridColor = SystemColors.Control;
            DataGridViewPatientProcedure.Location = new Point(203, 206);
            DataGridViewPatientProcedure.Name = "DataGridViewPatientProcedure";
            DataGridViewPatientProcedure.RowHeadersVisible = false;
            DataGridViewPatientProcedure.RowTemplate.Height = 20;
            DataGridViewPatientProcedure.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataGridViewPatientProcedure.ShowCellToolTips = false;
            DataGridViewPatientProcedure.Size = new Size(798, 312);
            DataGridViewPatientProcedure.TabIndex = 69;
            DataGridViewPatientProcedure.CellClick += DataGridViewPatientProcedure_CellClick;
            DataGridViewPatientProcedure.CellEnter += DataGridViewPatientProcedure_CellEnter;
            DataGridViewPatientProcedure.DataError += DataGridViewPatientProcedure_DataError;
            DataGridViewPatientProcedure.EditingControlShowing += DataGridViewPatientProcedure_EditingControlShowing;
            DataGridViewPatientProcedure.RowsAdded += DataGridViewPatientProcedure_RowsAdded;
            // 
            // SerialNo
            // 
            SerialNo.HeaderText = "#";
            SerialNo.Name = "SerialNo";
            SerialNo.Resizable = DataGridViewTriState.False;
            SerialNo.SortMode = DataGridViewColumnSortMode.NotSortable;
            SerialNo.Width = 50;
            // 
            // Date
            // 
            Date.HeaderText = "Date";
            Date.Name = "Date";
            Date.Resizable = DataGridViewTriState.False;
            // 
            // PerformedBy
            // 
            PerformedBy.HeaderText = "Performed By";
            PerformedBy.Name = "PerformedBy";
            PerformedBy.Resizable = DataGridViewTriState.False;
            PerformedBy.SortMode = DataGridViewColumnSortMode.NotSortable;
            PerformedBy.Width = 150;
            // 
            // Note
            // 
            Note.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Note.HeaderText = "Note";
            Note.Name = "Note";
            Note.Resizable = DataGridViewTriState.False;
            Note.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Status
            // 
            Status.FlatStyle = FlatStyle.Flat;
            Status.HeaderText = "Status";
            Status.Name = "Status";
            Status.Resizable = DataGridViewTriState.True;
            Status.Width = 150;
            // 
            // Remove
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Remove.DefaultCellStyle = dataGridViewCellStyle2;
            Remove.HeaderText = "...";
            Remove.Name = "Remove";
            Remove.Resizable = DataGridViewTriState.False;
            Remove.SortMode = DataGridViewColumnSortMode.NotSortable;
            Remove.Width = 25;
            // 
            // Id
            // 
            Id.HeaderText = "Id";
            Id.Name = "Id";
            Id.Resizable = DataGridViewTriState.False;
            Id.SortMode = DataGridViewColumnSortMode.NotSortable;
            Id.Visible = false;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(918, 536);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(83, 23);
            BtnExit.TabIndex = 72;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(740, 536);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(83, 23);
            BtnCancel.TabIndex = 71;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // BtnSave
            // 
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(829, 536);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(83, 23);
            BtnSave.TabIndex = 70;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // FormPatientProcedure
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1013, 594);
            Controls.Add(BtnExit);
            Controls.Add(BtnCancel);
            Controls.Add(BtnSave);
            Controls.Add(DataGridViewPatientProcedure);
            Controls.Add(statusStrip1);
            Controls.Add(label2);
            Controls.Add(groupBox1);
            Controls.Add(PatientProcedureInfo);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormPatientProcedure";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Patient Procedure";
            Load += FormPatientProcedure_Load;
            Controls.SetChildIndex(PatientProcedureInfo, 0);
            Controls.SetChildIndex(groupBox1, 0);
            Controls.SetChildIndex(label2, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(DataGridViewPatientProcedure, 0);
            Controls.SetChildIndex(BtnSave, 0);
            Controls.SetChildIndex(BtnCancel, 0);
            Controls.SetChildIndex(BtnExit, 0);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DataGridViewPatientProcedure).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private controls.hms.PatientInfoMin PatientProcedureInfo;
        private GroupBox groupBox1;
        private Label label2;
        private Label label6;
        private TextBox TextBoxPatientProcedureRequestOn;
        private Label label5;
        private TextBox TextBoxPatientProcedureRequestBy;
        private Label label4;
        private TextBox TextBoxPatientProcedureDisc;
        private Label label3;
        private TextBox TextBoxPatientProcedureName;
        private Label label1;
        private TextBox TextBoxStatus;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel ErrorMsgProcedure;
        private controls.DataViewVerticalScroll DataGridViewPatientProcedure;
        private DataGridViewTextBoxColumn SerialNo;
        private controls.grid.DataGridViewCalendarColumn Date;
        private DataGridViewTextBoxColumn PerformedBy;
        private DataGridViewTextBoxColumn Note;
        private DataGridViewComboBoxColumn Status;
        private DataGridViewTextBoxColumn Remove;
        private DataGridViewTextBoxColumn Id;
        private Button BtnExit;
        private Button BtnCancel;
        private Button BtnSave;
    }
}