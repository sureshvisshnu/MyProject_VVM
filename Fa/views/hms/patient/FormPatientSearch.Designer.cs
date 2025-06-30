namespace fa.views.hms.patient
{
    partial class FormPatientSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPatientSearch));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            toolStrip = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            TextBoxSearchPatientByName = new ToolStripTextBox();
            BtnSearchPatientByName = new ToolStripButton();
            BtnPatientSelect = new Button();
            GridViewPatientSearchResult = new controls.DataViewVerticalScroll();
            statusStrip1 = new StatusStrip();
            PatientSearchErrorMsg = new ToolStripStatusLabel();
            BtnPatientCancel = new Button();
            PatientName = new DataGridViewTextBoxColumn();
            PatientAddress = new DataGridViewTextBoxColumn();
            PatientDateOfBirth = new DataGridViewTextBoxColumn();
            PatientNumber = new DataGridViewTextBoxColumn();
            AssigneeDoctor = new DataGridViewTextBoxColumn();
            Status = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewPatientSearchResult).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // toolStrip
            // 
            toolStrip.BackColor = SystemColors.ControlLight;
            toolStrip.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip.Items.AddRange(new ToolStripItem[] { toolStripLabel1, TextBoxSearchPatientByName, BtnSearchPatientByName });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Padding = new Padding(5);
            toolStrip.Size = new Size(894, 31);
            toolStrip.TabIndex = 0;
            toolStrip.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(59, 18);
            toolStripLabel1.Text = "Search For";
            // 
            // TextBoxSearchPatientByName
            // 
            TextBoxSearchPatientByName.BorderStyle = BorderStyle.FixedSingle;
            TextBoxSearchPatientByName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxSearchPatientByName.HideSelection = false;
            TextBoxSearchPatientByName.MaxLength = 63;
            TextBoxSearchPatientByName.Name = "TextBoxSearchPatientByName";
            TextBoxSearchPatientByName.Size = new Size(350, 21);
            TextBoxSearchPatientByName.KeyDown += TextBoxSearchPatientByName_KeyDown;
            // 
            // BtnSearchPatientByName
            // 
            BtnSearchPatientByName.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnSearchPatientByName.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSearchPatientByName.Image = (Image)resources.GetObject("BtnSearchPatientByName.Image");
            BtnSearchPatientByName.ImageTransparentColor = Color.Magenta;
            BtnSearchPatientByName.Name = "BtnSearchPatientByName";
            BtnSearchPatientByName.Size = new Size(26, 18);
            BtnSearchPatientByName.Text = "Go";
            BtnSearchPatientByName.Click += BtnSearchPatientByName_Click;
            // 
            // BtnPatientSelect
            // 
            BtnPatientSelect.Enabled = false;
            BtnPatientSelect.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPatientSelect.Location = new Point(784, 368);
            BtnPatientSelect.Name = "BtnPatientSelect";
            BtnPatientSelect.Size = new Size(88, 23);
            BtnPatientSelect.TabIndex = 2;
            BtnPatientSelect.Text = "Select [F8]";
            BtnPatientSelect.UseVisualStyleBackColor = true;
            BtnPatientSelect.Click += BtnPatientSelect_Click;
            BtnPatientSelect.PreviewKeyDown += BtnPatientSelect_PreviewKeyDown;
            // 
            // GridViewPatientSearchResult
            // 
            GridViewPatientSearchResult.AllowUserToAddRows = false;
            GridViewPatientSearchResult.AllowUserToDeleteRows = false;
            GridViewPatientSearchResult.AllowUserToResizeColumns = false;
            GridViewPatientSearchResult.AllowUserToResizeRows = false;
            GridViewPatientSearchResult.BackgroundColor = SystemColors.Control;
            GridViewPatientSearchResult.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewPatientSearchResult.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewPatientSearchResult.ColumnHeadersHeight = 20;
            GridViewPatientSearchResult.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewPatientSearchResult.Columns.AddRange(new DataGridViewColumn[] { PatientName, PatientAddress, PatientDateOfBirth, PatientNumber, AssigneeDoctor, Status, Column1 });
            GridViewPatientSearchResult.EditMode = DataGridViewEditMode.EditProgrammatically;
            GridViewPatientSearchResult.EnableHeadersVisualStyles = false;
            GridViewPatientSearchResult.Location = new Point(12, 43);
            GridViewPatientSearchResult.MultiSelect = false;
            GridViewPatientSearchResult.Name = "GridViewPatientSearchResult";
            GridViewPatientSearchResult.ReadOnly = true;
            GridViewPatientSearchResult.RowHeadersVisible = false;
            GridViewPatientSearchResult.RowTemplate.Height = 20;
            GridViewPatientSearchResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewPatientSearchResult.ShowCellToolTips = false;
            GridViewPatientSearchResult.ShowEditingIcon = false;
            GridViewPatientSearchResult.Size = new Size(871, 309);
            GridViewPatientSearchResult.TabIndex = 1;
            GridViewPatientSearchResult.CellContentDoubleClick += GridViewPatientSearchResult_CellContentDoubleClick;
            GridViewPatientSearchResult.CellDoubleClick += GridViewPatientSearchResult_CellContentDoubleClick;
            GridViewPatientSearchResult.PreviewKeyDown += GridViewPatientSearchResult_PreviewKeyDown;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { PatientSearchErrorMsg });
            statusStrip1.Location = new Point(0, 406);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(894, 22);
            statusStrip1.TabIndex = 26;
            statusStrip1.Text = "statusStrip1";
            // 
            // PatientSearchErrorMsg
            // 
            PatientSearchErrorMsg.Name = "PatientSearchErrorMsg";
            PatientSearchErrorMsg.Size = new Size(0, 17);
            // 
            // BtnPatientCancel
            // 
            BtnPatientCancel.DialogResult = DialogResult.Cancel;
            BtnPatientCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPatientCancel.Location = new Point(690, 368);
            BtnPatientCancel.Name = "BtnPatientCancel";
            BtnPatientCancel.Size = new Size(88, 23);
            BtnPatientCancel.TabIndex = 3;
            BtnPatientCancel.Text = "Cancel [Esc]";
            BtnPatientCancel.UseVisualStyleBackColor = true;
            BtnPatientCancel.Click += BtnPatientCancel_Click;
            BtnPatientCancel.PreviewKeyDown += BtnPatientSelect_PreviewKeyDown;
            // 
            // PatientName
            // 
            PatientName.HeaderText = "Name";
            PatientName.Name = "PatientName";
            PatientName.ReadOnly = true;
            PatientName.Resizable = DataGridViewTriState.False;
            PatientName.SortMode = DataGridViewColumnSortMode.NotSortable;
            PatientName.Width = 200;
            // 
            // PatientAddress
            // 
            PatientAddress.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            PatientAddress.HeaderText = "Address";
            PatientAddress.Name = "PatientAddress";
            PatientAddress.ReadOnly = true;
            PatientAddress.Resizable = DataGridViewTriState.False;
            PatientAddress.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // PatientDateOfBirth
            // 
            PatientDateOfBirth.HeaderText = "DOB";
            PatientDateOfBirth.Name = "PatientDateOfBirth";
            PatientDateOfBirth.ReadOnly = true;
            PatientDateOfBirth.Resizable = DataGridViewTriState.False;
            PatientDateOfBirth.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // PatientNumber
            // 
            PatientNumber.HeaderText = "Patient #";
            PatientNumber.Name = "PatientNumber";
            PatientNumber.ReadOnly = true;
            PatientNumber.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // AssigneeDoctor
            // 
            AssigneeDoctor.HeaderText = "Doctor / Consultant";
            AssigneeDoctor.Name = "AssigneeDoctor";
            AssigneeDoctor.ReadOnly = true;
            AssigneeDoctor.SortMode = DataGridViewColumnSortMode.NotSortable;
            AssigneeDoctor.Width = 150;
            // 
            // Status
            // 
            Status.HeaderText = "Status";
            Status.Name = "Status";
            Status.ReadOnly = true;
            Status.SortMode = DataGridViewColumnSortMode.NotSortable;
            Status.Width = 60;
            // 
            // Column1
            // 
            Column1.HeaderText = "Id";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Visible = false;
            // 
            // FormPatientSearch
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnPatientCancel;
            ClientSize = new Size(894, 428);
            Controls.Add(BtnPatientCancel);
            Controls.Add(statusStrip1);
            Controls.Add(BtnPatientSelect);
            Controls.Add(GridViewPatientSearchResult);
            Controls.Add(toolStrip);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormPatientSearch";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Patient Search";
            Load += FormPatientSearch_Load;
            Controls.SetChildIndex(toolStrip, 0);
            Controls.SetChildIndex(GridViewPatientSearchResult, 0);
            Controls.SetChildIndex(BtnPatientSelect, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(BtnPatientCancel, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewPatientSearchResult).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox TextBoxSearchPatientByName;
        private controls.DataViewVerticalScroll GridViewPatientSearchResult;
        private System.Windows.Forms.Button BtnPatientSelect;
        private System.Windows.Forms.ToolStripButton BtnSearchPatientByName;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel PatientSearchErrorMsg;
        private System.Windows.Forms.Button BtnPatientCancel;
        private DataGridViewTextBoxColumn PatientName;
        private DataGridViewTextBoxColumn PatientAddress;
        private DataGridViewTextBoxColumn PatientDateOfBirth;
        private DataGridViewTextBoxColumn PatientNumber;
        private DataGridViewTextBoxColumn AssigneeDoctor;
        private DataGridViewTextBoxColumn Status;
        private DataGridViewTextBoxColumn Column1;
    }
}