namespace fa.views.hms.helper
{
    partial class FormSelectSymptom
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
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectSymptom));
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            BtnSymptomDone = new Button();
            BtnSymptomCancel = new Button();
            statusStrip1 = new StatusStrip();
            ErrorMsgSelectedSymptom = new ToolStripStatusLabel();
            GridViewSelectedSymptoms = new controls.DataViewVerticalScroll();
            DiagnosisSearchTextBox = new controls.text.DelayedTextChangeTextBox();
            DiagnosisCheckedListBox = new CheckedListBox();
            BtnRefreshDiagnosis = new Button();
            BtnNewDiagnosis = new Button();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            Column9 = new DataGridViewTextBoxColumn();
            Column10 = new DataGridViewTextBoxColumn();
            Column11 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewSelectedSymptoms).BeginInit();
            SuspendLayout();
            // 
            // PatientIdTransport
            // 
            PatientIdTransport.Size = new Size(100, 21);
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(249, 225);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(249, 199);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(249, 173);
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // BtnSymptomDone
            // 
            BtnSymptomDone.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSymptomDone.Location = new Point(748, 335);
            BtnSymptomDone.Name = "BtnSymptomDone";
            BtnSymptomDone.Size = new Size(75, 23);
            BtnSymptomDone.TabIndex = 3;
            BtnSymptomDone.Text = "Done [F8]";
            BtnSymptomDone.UseVisualStyleBackColor = true;
            BtnSymptomDone.Click += BtnSymptomsDone_Click;
            BtnSymptomDone.PreviewKeyDown += BtnSymptomDone_PreviewKeyDown;
            // 
            // BtnSymptomCancel
            // 
            BtnSymptomCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSymptomCancel.Location = new Point(657, 335);
            BtnSymptomCancel.Name = "BtnSymptomCancel";
            BtnSymptomCancel.Size = new Size(85, 23);
            BtnSymptomCancel.TabIndex = 4;
            BtnSymptomCancel.Text = "Reset [Esc]";
            BtnSymptomCancel.UseVisualStyleBackColor = true;
            BtnSymptomCancel.Click += BtnSymptomCancel_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsgSelectedSymptom });
            statusStrip1.Location = new Point(0, 366);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(833, 22);
            statusStrip1.TabIndex = 68;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsgSelectedSymptom
            // 
            ErrorMsgSelectedSymptom.Name = "ErrorMsgSelectedSymptom";
            ErrorMsgSelectedSymptom.Size = new Size(16, 17);
            ErrorMsgSelectedSymptom.Text = "   ";
            // 
            // GridViewSelectedSymptoms
            // 
            GridViewSelectedSymptoms.AllowUserToAddRows = false;
            GridViewSelectedSymptoms.AllowUserToDeleteRows = false;
            GridViewSelectedSymptoms.AllowUserToResizeColumns = false;
            GridViewSelectedSymptoms.AllowUserToResizeRows = false;
            GridViewSelectedSymptoms.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            GridViewSelectedSymptoms.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewSelectedSymptoms.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewSelectedSymptoms.ColumnHeadersHeight = 20;
            GridViewSelectedSymptoms.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewSelectedSymptoms.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column5, Column9, Column10, Column11, Column3 });
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Window;
            dataGridViewCellStyle5.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle5.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            GridViewSelectedSymptoms.DefaultCellStyle = dataGridViewCellStyle5;
            GridViewSelectedSymptoms.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewSelectedSymptoms.EnableHeadersVisualStyles = false;
            GridViewSelectedSymptoms.Location = new Point(200, 11);
            GridViewSelectedSymptoms.MultiSelect = false;
            GridViewSelectedSymptoms.Name = "GridViewSelectedSymptoms";
            GridViewSelectedSymptoms.RowHeadersVisible = false;
            GridViewSelectedSymptoms.RowTemplate.Height = 20;
            GridViewSelectedSymptoms.ScrollBars = ScrollBars.Vertical;
            GridViewSelectedSymptoms.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewSelectedSymptoms.ShowCellToolTips = false;
            GridViewSelectedSymptoms.Size = new Size(623, 314);
            GridViewSelectedSymptoms.TabIndex = 2;
            GridViewSelectedSymptoms.CellBeginEdit += GridViewSelectedSymptoms_CellBeginEdit;
            GridViewSelectedSymptoms.CellClick += GridViewSelectedSymptoms_CellClick;
            GridViewSelectedSymptoms.CellEndEdit += GridViewSelectedSymptoms_CellEndEdit;
            GridViewSelectedSymptoms.CellEnter += GridViewSelectedSymptoms_CellEnter;
            GridViewSelectedSymptoms.DataError += GridViewSelectedSymptoms_DataError;
            GridViewSelectedSymptoms.EditingControlShowing += GridViewSelectedSymptoms_EditingControlShowing;
            GridViewSelectedSymptoms.SelectionChanged += GridViewSelectedSymptoms_SelectionChanged;
            GridViewSelectedSymptoms.Enter += GridViewSelectedSymptoms_Enter;
            // 
            // DiagnosisSearchTextBox
            // 
            DiagnosisSearchTextBox.BorderStyle = BorderStyle.FixedSingle;
            DiagnosisSearchTextBox.Delay = false;
            DiagnosisSearchTextBox.DelayTime = 1000;
            DiagnosisSearchTextBox.Location = new Point(11, 11);
            DiagnosisSearchTextBox.MaxLength = 35;
            DiagnosisSearchTextBox.Name = "DiagnosisSearchTextBox";
            DiagnosisSearchTextBox.Searchstartfrom = 1;
            DiagnosisSearchTextBox.Size = new Size(156, 21);
            DiagnosisSearchTextBox.TabIndex = 0;
            DiagnosisSearchTextBox.TextChanged += DiagnosisSearchTextBox_TextChanged;
            // 
            // DiagnosisCheckedListBox
            // 
            DiagnosisCheckedListBox.CheckOnClick = true;
            DiagnosisCheckedListBox.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DiagnosisCheckedListBox.FormattingEnabled = true;
            DiagnosisCheckedListBox.Location = new Point(11, 34);
            DiagnosisCheckedListBox.Name = "DiagnosisCheckedListBox";
            DiagnosisCheckedListBox.Size = new Size(180, 292);
            DiagnosisCheckedListBox.TabIndex = 1;
            DiagnosisCheckedListBox.ItemCheck += DiagnosisCheckedListBox_ItemCheck;
            DiagnosisCheckedListBox.PreviewKeyDown += DiagnosisCheckedListBox_PreviewKeyDown;
            // 
            // BtnRefreshDiagnosis
            // 
            BtnRefreshDiagnosis.BackgroundImage = (Image)resources.GetObject("BtnRefreshDiagnosis.BackgroundImage");
            BtnRefreshDiagnosis.BackgroundImageLayout = ImageLayout.Stretch;
            BtnRefreshDiagnosis.Image = (Image)resources.GetObject("BtnRefreshDiagnosis.Image");
            BtnRefreshDiagnosis.Location = new Point(168, 10);
            BtnRefreshDiagnosis.Name = "BtnRefreshDiagnosis";
            BtnRefreshDiagnosis.Size = new Size(23, 22);
            BtnRefreshDiagnosis.TabIndex = 85;
            BtnRefreshDiagnosis.TabStop = false;
            BtnRefreshDiagnosis.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnRefreshDiagnosis.UseVisualStyleBackColor = true;
            BtnRefreshDiagnosis.Click += BtnRefreshDiagnosis_Click;
            // 
            // BtnNewDiagnosis
            // 
            BtnNewDiagnosis.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnNewDiagnosis.Location = new Point(11, 335);
            BtnNewDiagnosis.Name = "BtnNewDiagnosis";
            BtnNewDiagnosis.Size = new Size(85, 23);
            BtnNewDiagnosis.TabIndex = 4;
            BtnNewDiagnosis.Text = "New [F3]";
            BtnNewDiagnosis.UseVisualStyleBackColor = true;
            BtnNewDiagnosis.Click += BtnNewDiagnosis_Click;
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            Column1.DefaultCellStyle = dataGridViewCellStyle2;
            Column1.HeaderText = "Name";
            Column1.MaxInputLength = 250;
            Column1.Name = "Column1";
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 278;
            // 
            // Column2
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            Column2.DefaultCellStyle = dataGridViewCellStyle3;
            Column2.HeaderText = "Description";
            Column2.MaxInputLength = 250;
            Column2.Name = "Column2";
            Column2.Resizable = DataGridViewTriState.False;
            Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column2.Width = 297;
            // 
            // Column5
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.ForeColor = Color.Black;
            dataGridViewCellStyle4.NullValue = "X";
            dataGridViewCellStyle4.SelectionBackColor = Color.White;
            dataGridViewCellStyle4.SelectionForeColor = Color.Black;
            Column5.DefaultCellStyle = dataGridViewCellStyle4;
            Column5.HeaderText = "...";
            Column5.Name = "Column5";
            Column5.Resizable = DataGridViewTriState.False;
            Column5.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column5.Width = 25;
            // 
            // Column9
            // 
            Column9.HeaderText = "ID";
            Column9.Name = "Column9";
            Column9.Resizable = DataGridViewTriState.False;
            Column9.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column9.Visible = false;
            // 
            // Column10
            // 
            Column10.HeaderText = "PID";
            Column10.Name = "Column10";
            Column10.Resizable = DataGridViewTriState.False;
            Column10.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column10.Visible = false;
            // 
            // Column11
            // 
            Column11.HeaderText = "ProcdName";
            Column11.Name = "Column11";
            Column11.Resizable = DataGridViewTriState.False;
            Column11.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column11.Visible = false;
            // 
            // Column3
            // 
            Column3.HeaderText = "IsActive";
            Column3.Name = "Column3";
            Column3.Resizable = DataGridViewTriState.False;
            Column3.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column3.Visible = false;
            // 
            // FormSelectSymptom
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(833, 388);
            Controls.Add(DiagnosisSearchTextBox);
            Controls.Add(DiagnosisCheckedListBox);
            Controls.Add(BtnRefreshDiagnosis);
            Controls.Add(GridViewSelectedSymptoms);
            Controls.Add(statusStrip1);
            Controls.Add(BtnNewDiagnosis);
            Controls.Add(BtnSymptomCancel);
            Controls.Add(BtnSymptomDone);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSelectSymptom";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Diagnosis";
            Load += SelectSymptom_Load;
            Controls.SetChildIndex(BtnSymptomDone, 0);
            Controls.SetChildIndex(BtnSymptomCancel, 0);
            Controls.SetChildIndex(BtnNewDiagnosis, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(GridViewSelectedSymptoms, 0);
            Controls.SetChildIndex(BtnRefreshDiagnosis, 0);
            Controls.SetChildIndex(DiagnosisCheckedListBox, 0);
            Controls.SetChildIndex(DiagnosisSearchTextBox, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewSelectedSymptoms).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button BtnSymptomCancel;
        public System.Windows.Forms.Button BtnSymptomDone;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ErrorMsgSelectedSymptom;
        private controls.DataViewVerticalScroll GridViewSelectedSymptoms;
        private controls.text.DelayedTextChangeTextBox DiagnosisSearchTextBox;
        private System.Windows.Forms.CheckedListBox DiagnosisCheckedListBox;
        private System.Windows.Forms.Button BtnRefreshDiagnosis;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column9;
        private DataGridViewTextBoxColumn Column10;
        private DataGridViewTextBoxColumn Column11;
        private DataGridViewTextBoxColumn Column3;
        private Button button1;
        private Button BtnNewDiagnosis;
    }
}