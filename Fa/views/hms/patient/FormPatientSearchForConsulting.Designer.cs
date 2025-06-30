namespace fa.views.hms.patient
{
    partial class FormPatientSearchForConsulting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPatientSearchForConsulting));
            GridViewPatientSearchResult = new controls.DataViewVerticalScroll();
            toolStrip = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            TextBoxSearchPatientByName = new ToolStripTextBox();
            BtnSearchPatientByName = new ToolStripButton();
            statusStrip1 = new StatusStrip();
            PatientConsultingSearchErrorMsg = new ToolStripStatusLabel();
            BtnConsultingPatientConsulting = new Button();
            PatientName = new DataGridViewTextBoxColumn();
            PatientAddress = new DataGridViewTextBoxColumn();
            PatientDateOfBirth = new DataGridViewTextBoxColumn();
            PatientNumber = new DataGridViewTextBoxColumn();
            AssigneeDoctor = new DataGridViewTextBoxColumn();
            Status = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)GridViewPatientSearchResult).BeginInit();
            toolStrip.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // PatientIdTransport
            // 
            PatientIdTransport.Size = new Size(100, 21);
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
            // GridViewPatientSearchResult
            // 
            GridViewPatientSearchResult.AllowUserToAddRows = false;
            GridViewPatientSearchResult.AllowUserToDeleteRows = false;
            GridViewPatientSearchResult.AllowUserToResizeColumns = false;
            GridViewPatientSearchResult.AllowUserToResizeRows = false;
            GridViewPatientSearchResult.BackgroundColor = SystemColors.ControlLight;
            GridViewPatientSearchResult.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewPatientSearchResult.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewPatientSearchResult.ColumnHeadersHeight = 20;
            GridViewPatientSearchResult.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewPatientSearchResult.Columns.AddRange(new DataGridViewColumn[] { PatientName, PatientAddress, PatientDateOfBirth, PatientNumber, AssigneeDoctor, Status, Column1 });
            GridViewPatientSearchResult.EditMode = DataGridViewEditMode.EditProgrammatically;
            GridViewPatientSearchResult.EnableHeadersVisualStyles = false;
            GridViewPatientSearchResult.Location = new Point(8, 42);
            GridViewPatientSearchResult.MultiSelect = false;
            GridViewPatientSearchResult.Name = "GridViewPatientSearchResult";
            GridViewPatientSearchResult.ReadOnly = true;
            GridViewPatientSearchResult.RowHeadersVisible = false;
            GridViewPatientSearchResult.RowTemplate.Height = 20;
            GridViewPatientSearchResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewPatientSearchResult.ShowCellToolTips = false;
            GridViewPatientSearchResult.ShowEditingIcon = false;
            GridViewPatientSearchResult.Size = new Size(880, 309);
            GridViewPatientSearchResult.TabIndex = 3;
            GridViewPatientSearchResult.CellContentDoubleClick += GridViewPatientSearchResult_CellContentDoubleClick;
            GridViewPatientSearchResult.CellDoubleClick += GridViewPatientSearchResult_CellDoubleClick;
            GridViewPatientSearchResult.PreviewKeyDown += GridViewPatientSearchResult_PreviewKeyDown;
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
            toolStrip.Size = new Size(897, 31);
            toolStrip.TabIndex = 2;
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
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { PatientConsultingSearchErrorMsg });
            statusStrip1.Location = new Point(0, 403);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(897, 22);
            statusStrip1.TabIndex = 29;
            statusStrip1.Text = "statusStrip1";
            // 
            // PatientConsultingSearchErrorMsg
            // 
            PatientConsultingSearchErrorMsg.Name = "PatientConsultingSearchErrorMsg";
            PatientConsultingSearchErrorMsg.Size = new Size(43, 17);
            PatientConsultingSearchErrorMsg.Text = "            ";
            // 
            // BtnConsultingPatientConsulting
            // 
            BtnConsultingPatientConsulting.Enabled = false;
            BtnConsultingPatientConsulting.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnConsultingPatientConsulting.Location = new Point(781, 367);
            BtnConsultingPatientConsulting.Name = "BtnConsultingPatientConsulting";
            BtnConsultingPatientConsulting.Size = new Size(88, 23);
            BtnConsultingPatientConsulting.TabIndex = 27;
            BtnConsultingPatientConsulting.Text = "Consult [F8]";
            BtnConsultingPatientConsulting.UseVisualStyleBackColor = true;
            BtnConsultingPatientConsulting.Click += BtnConsultingPatientConsulting_Click;
            BtnConsultingPatientConsulting.PreviewKeyDown += BtnConsultingPatientConsulting_PreviewKeyDown;
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
            PatientDateOfBirth.FillWeight = 80F;
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
            PatientNumber.Resizable = DataGridViewTriState.False;
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
            Status.Resizable = DataGridViewTriState.False;
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
            // FormPatientSearchForConsulting
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(897, 425);
            Controls.Add(statusStrip1);
            Controls.Add(BtnConsultingPatientConsulting);
            Controls.Add(GridViewPatientSearchResult);
            Controls.Add(toolStrip);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormPatientSearchForConsulting";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Patient Search For Consulting";
            Load += FormPatientSearchForConsulting_Load;
            Controls.SetChildIndex(toolStrip, 0);
            Controls.SetChildIndex(GridViewPatientSearchResult, 0);
            Controls.SetChildIndex(BtnConsultingPatientConsulting, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            ((System.ComponentModel.ISupportInitialize)GridViewPatientSearchResult).EndInit();
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private controls.DataViewVerticalScroll GridViewPatientSearchResult;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox TextBoxSearchPatientByName;
        private System.Windows.Forms.ToolStripButton BtnSearchPatientByName;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel PatientConsultingSearchErrorMsg;
        private System.Windows.Forms.Button BtnConsultingPatientConsulting;
        private DataGridViewTextBoxColumn PatientName;
        private DataGridViewTextBoxColumn PatientAddress;
        private DataGridViewTextBoxColumn PatientDateOfBirth;
        private DataGridViewTextBoxColumn PatientNumber;
        private DataGridViewTextBoxColumn AssigneeDoctor;
        private DataGridViewTextBoxColumn Status;
        private DataGridViewTextBoxColumn Column1;
    }
}