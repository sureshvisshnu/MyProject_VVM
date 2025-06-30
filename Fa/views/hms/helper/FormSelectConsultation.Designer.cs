namespace fa.views.hms.helper
{
    partial class FormSelectConsultation
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
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectConsultation));
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            statusStrip1 = new StatusStrip();
            ErrorMsgSelectConsultation = new ToolStripStatusLabel();
            BtnSelectConsultationCancel = new Button();
            BtnSelectConsultationDone = new Button();
            GridViewSelectConsultation = new controls.DataViewVerticalScroll();
            GridViewSelectConsultationTotal = new controls.DataViewVerticalScroll();
            Column4 = new DataGridViewTextBoxColumn();
            Column6 = new controls.grid.DataGridViewCurrencyColumn();
            Column7 = new DataGridViewTextBoxColumn();
            ConsultationSearchTextBox = new controls.text.DelayedTextChangeTextBox();
            ConsultationCheckedListBox = new CheckedListBox();
            BtnRefreshConsultation = new Button();
            BtnConsultationsNew = new Button();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new controls.grid.DataGridViewCurrencyColumn();
            Column5 = new DataGridViewTextBoxColumn();
            Column9 = new DataGridViewTextBoxColumn();
            Column10 = new DataGridViewTextBoxColumn();
            Column11 = new DataGridViewTextBoxColumn();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewSelectConsultation).BeginInit();
            ((System.ComponentModel.ISupportInitialize)GridViewSelectConsultationTotal).BeginInit();
            SuspendLayout();
            // 
            // PatientIdTransport
            // 
            PatientIdTransport.Size = new Size(100, 21);
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(172, 242);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(172, 216);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(172, 190);
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // checkBoxIsPatient
            // 
            checkBoxIsPatient.Size = new Size(69, 17);
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsgSelectConsultation });
            statusStrip1.Location = new Point(0, 366);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(969, 22);
            statusStrip1.TabIndex = 33;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsgSelectConsultation
            // 
            ErrorMsgSelectConsultation.Name = "ErrorMsgSelectConsultation";
            ErrorMsgSelectConsultation.Size = new Size(16, 17);
            ErrorMsgSelectConsultation.Text = "   ";
            // 
            // BtnSelectConsultationCancel
            // 
            BtnSelectConsultationCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSelectConsultationCancel.Location = new Point(767, 332);
            BtnSelectConsultationCancel.Name = "BtnSelectConsultationCancel";
            BtnSelectConsultationCancel.Size = new Size(93, 23);
            BtnSelectConsultationCancel.TabIndex = 4;
            BtnSelectConsultationCancel.Text = "Reset [Esc]";
            BtnSelectConsultationCancel.UseVisualStyleBackColor = true;
            BtnSelectConsultationCancel.Click += BtnSelectConsultationCancel_Click;
            // 
            // BtnSelectConsultationDone
            // 
            BtnSelectConsultationDone.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSelectConsultationDone.Location = new Point(866, 332);
            BtnSelectConsultationDone.Name = "BtnSelectConsultationDone";
            BtnSelectConsultationDone.Size = new Size(93, 23);
            BtnSelectConsultationDone.TabIndex = 3;
            BtnSelectConsultationDone.Text = "Done [F8]";
            BtnSelectConsultationDone.UseVisualStyleBackColor = true;
            BtnSelectConsultationDone.Click += BtnSelectConsultationDone_Click;
            BtnSelectConsultationDone.PreviewKeyDown += BtnSelectConsultationDone_PreviewKeyDown;
            // 
            // GridViewSelectConsultation
            // 
            GridViewSelectConsultation.AllowUserToAddRows = false;
            GridViewSelectConsultation.AllowUserToDeleteRows = false;
            GridViewSelectConsultation.AllowUserToResizeColumns = false;
            GridViewSelectConsultation.AllowUserToResizeRows = false;
            GridViewSelectConsultation.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            GridViewSelectConsultation.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewSelectConsultation.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewSelectConsultation.ColumnHeadersHeight = 20;
            GridViewSelectConsultation.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewSelectConsultation.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column5, Column9, Column10, Column11 });
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Window;
            dataGridViewCellStyle6.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle6.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            GridViewSelectConsultation.DefaultCellStyle = dataGridViewCellStyle6;
            GridViewSelectConsultation.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewSelectConsultation.EnableHeadersVisualStyles = false;
            GridViewSelectConsultation.Location = new Point(200, 11);
            GridViewSelectConsultation.MultiSelect = false;
            GridViewSelectConsultation.Name = "GridViewSelectConsultation";
            GridViewSelectConsultation.RowHeadersVisible = false;
            GridViewSelectConsultation.RowTemplate.Height = 20;
            GridViewSelectConsultation.ScrollBars = ScrollBars.Vertical;
            GridViewSelectConsultation.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewSelectConsultation.ShowCellToolTips = false;
            GridViewSelectConsultation.Size = new Size(757, 293);
            GridViewSelectConsultation.TabIndex = 2;
            GridViewSelectConsultation.CellBeginEdit += GridViewSelectConsultation_CellBeginEdit;
            GridViewSelectConsultation.CellClick += GridViewSelectConsultation_CellClick;
            GridViewSelectConsultation.CellEndEdit += GridViewSelectConsultation_CellEndEdit;
            GridViewSelectConsultation.CellEnter += GridViewSelectConsultation_CellEnter;
            GridViewSelectConsultation.CellFormatting += GridViewSelectConsultation_CellFormatting;
            GridViewSelectConsultation.DataError += GridViewSelectConsultation_DataError;
            GridViewSelectConsultation.Enter += GridViewSelectConsultation_Enter;
            // 
            // GridViewSelectConsultationTotal
            // 
            GridViewSelectConsultationTotal.BackgroundColor = SystemColors.Window;
            GridViewSelectConsultationTotal.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GridViewSelectConsultationTotal.ColumnHeadersVisible = false;
            GridViewSelectConsultationTotal.Columns.AddRange(new DataGridViewColumn[] { Column4, Column6, Column7 });
            GridViewSelectConsultationTotal.Location = new Point(200, 303);
            GridViewSelectConsultationTotal.Name = "GridViewSelectConsultationTotal";
            GridViewSelectConsultationTotal.RowHeadersVisible = false;
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle10.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.True;
            GridViewSelectConsultationTotal.RowsDefaultCellStyle = dataGridViewCellStyle10;
            GridViewSelectConsultationTotal.ScrollBars = ScrollBars.None;
            GridViewSelectConsultationTotal.ShowCellToolTips = false;
            GridViewSelectConsultationTotal.Size = new Size(757, 23);
            GridViewSelectConsultationTotal.TabIndex = 36;
            // 
            // Column4
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.BackColor = Color.White;
            dataGridViewCellStyle7.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            dataGridViewCellStyle7.ForeColor = Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = Color.White;
            dataGridViewCellStyle7.SelectionForeColor = Color.Black;
            Column4.DefaultCellStyle = dataGridViewCellStyle7;
            Column4.HeaderText = "Space";
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            Column4.Resizable = DataGridViewTriState.False;
            Column4.Width = 635;
            // 
            // Column6
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.BackColor = Color.White;
            dataGridViewCellStyle8.ForeColor = Color.Black;
            dataGridViewCellStyle8.NullValue = "0.00";
            dataGridViewCellStyle8.SelectionBackColor = Color.White;
            dataGridViewCellStyle8.SelectionForeColor = Color.Black;
            Column6.DefaultCellStyle = dataGridViewCellStyle8;
            Column6.HeaderText = "Total";
            Column6.Name = "Column6";
            Column6.ReadOnly = true;
            Column6.Resizable = DataGridViewTriState.False;
            Column6.SortMode = DataGridViewColumnSortMode.Automatic;
            Column6.Width = 75;
            // 
            // Column7
            // 
            Column7.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle9.BackColor = Color.White;
            dataGridViewCellStyle9.ForeColor = Color.Black;
            dataGridViewCellStyle9.SelectionBackColor = Color.White;
            dataGridViewCellStyle9.SelectionForeColor = Color.Black;
            Column7.DefaultCellStyle = dataGridViewCellStyle9;
            Column7.HeaderText = "Close";
            Column7.Name = "Column7";
            Column7.ReadOnly = true;
            Column7.Resizable = DataGridViewTriState.False;
            // 
            // ConsultationSearchTextBox
            // 
            ConsultationSearchTextBox.BorderStyle = BorderStyle.FixedSingle;
            ConsultationSearchTextBox.Delay = false;
            ConsultationSearchTextBox.DelayTime = 1000;
            ConsultationSearchTextBox.Location = new Point(11, 11);
            ConsultationSearchTextBox.MaxLength = 35;
            ConsultationSearchTextBox.Name = "ConsultationSearchTextBox";
            ConsultationSearchTextBox.Searchstartfrom = 1;
            ConsultationSearchTextBox.Size = new Size(156, 21);
            ConsultationSearchTextBox.TabIndex = 0;
            ConsultationSearchTextBox.TextChanged += ConsultationSearchTextBox_TextChanged;
            // 
            // ConsultationCheckedListBox
            // 
            ConsultationCheckedListBox.CheckOnClick = true;
            ConsultationCheckedListBox.FormattingEnabled = true;
            ConsultationCheckedListBox.Location = new Point(11, 34);
            ConsultationCheckedListBox.Name = "ConsultationCheckedListBox";
            ConsultationCheckedListBox.Size = new Size(180, 292);
            ConsultationCheckedListBox.TabIndex = 1;
            ConsultationCheckedListBox.ItemCheck += ConsultationCheckedListBox_ItemCheck;
            ConsultationCheckedListBox.PreviewKeyDown += ConsultationCheckedListBox_PreviewKeyDown;
            // 
            // BtnRefreshConsultation
            // 
            BtnRefreshConsultation.BackgroundImage = (Image)resources.GetObject("BtnRefreshConsultation.BackgroundImage");
            BtnRefreshConsultation.BackgroundImageLayout = ImageLayout.Stretch;
            BtnRefreshConsultation.Image = (Image)resources.GetObject("BtnRefreshConsultation.Image");
            BtnRefreshConsultation.Location = new Point(168, 10);
            BtnRefreshConsultation.Name = "BtnRefreshConsultation";
            BtnRefreshConsultation.Size = new Size(23, 22);
            BtnRefreshConsultation.TabIndex = 88;
            BtnRefreshConsultation.TabStop = false;
            BtnRefreshConsultation.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnRefreshConsultation.UseVisualStyleBackColor = true;
            BtnRefreshConsultation.Click += BtnRefreshConsultation_Click;
            // 
            // BtnConsultationsNew
            // 
            BtnConsultationsNew.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnConsultationsNew.Location = new Point(11, 333);
            BtnConsultationsNew.Name = "BtnConsultationsNew";
            BtnConsultationsNew.Size = new Size(93, 23);
            BtnConsultationsNew.TabIndex = 89;
            BtnConsultationsNew.Text = "New [F3]";
            BtnConsultationsNew.UseVisualStyleBackColor = true;
            BtnConsultationsNew.Click += BtnConsultationsNew_Click;
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            Column1.DefaultCellStyle = dataGridViewCellStyle2;
            Column1.HeaderText = "Name";
            Column1.Name = "Column1";
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 200;
            // 
            // Column2
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.White;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            Column2.DefaultCellStyle = dataGridViewCellStyle3;
            Column2.HeaderText = "Description";
            Column2.MaxInputLength = 250;
            Column2.Name = "Column2";
            Column2.Resizable = DataGridViewTriState.False;
            Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column2.Width = 434;
            // 
            // Column3
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.ForeColor = Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = Color.White;
            dataGridViewCellStyle4.SelectionForeColor = Color.Black;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            Column3.DefaultCellStyle = dataGridViewCellStyle4;
            Column3.HeaderText = "Fee";
            Column3.Name = "Column3";
            Column3.Resizable = DataGridViewTriState.False;
            Column3.Width = 75;
            // 
            // Column5
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.ForeColor = Color.Black;
            dataGridViewCellStyle5.NullValue = "X";
            dataGridViewCellStyle5.SelectionBackColor = Color.White;
            dataGridViewCellStyle5.SelectionForeColor = Color.Black;
            Column5.DefaultCellStyle = dataGridViewCellStyle5;
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
            Column10.HeaderText = "CID";
            Column10.Name = "Column10";
            Column10.Resizable = DataGridViewTriState.False;
            Column10.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column10.Visible = false;
            // 
            // Column11
            // 
            Column11.HeaderText = "ConsName";
            Column11.Name = "Column11";
            Column11.Resizable = DataGridViewTriState.False;
            Column11.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column11.Visible = false;
            // 
            // FormSelectConsultation
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(969, 388);
            Controls.Add(BtnConsultationsNew);
            Controls.Add(ConsultationSearchTextBox);
            Controls.Add(ConsultationCheckedListBox);
            Controls.Add(BtnRefreshConsultation);
            Controls.Add(GridViewSelectConsultationTotal);
            Controls.Add(GridViewSelectConsultation);
            Controls.Add(statusStrip1);
            Controls.Add(BtnSelectConsultationCancel);
            Controls.Add(BtnSelectConsultationDone);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSelectConsultation";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select Consultations";
            Load += FormSelectConsultation_Load;
            Controls.SetChildIndex(checkBoxIsPatient, 0);
            Controls.SetChildIndex(BtnSelectConsultationDone, 0);
            Controls.SetChildIndex(BtnSelectConsultationCancel, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(GridViewSelectConsultation, 0);
            Controls.SetChildIndex(GridViewSelectConsultationTotal, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(BtnRefreshConsultation, 0);
            Controls.SetChildIndex(ConsultationCheckedListBox, 0);
            Controls.SetChildIndex(ConsultationSearchTextBox, 0);
            Controls.SetChildIndex(BtnConsultationsNew, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewSelectConsultation).EndInit();
            ((System.ComponentModel.ISupportInitialize)GridViewSelectConsultationTotal).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button BtnSelectConsultationCancel;
        private System.Windows.Forms.Button BtnSelectConsultationDone;
        private controls.DataViewVerticalScroll GridViewSelectConsultation;
        private System.Windows.Forms.ToolStripStatusLabel ErrorMsgSelectConsultation;
        private controls.DataViewVerticalScroll GridViewSelectConsultationTotal;
        private controls.text.DelayedTextChangeTextBox ConsultationSearchTextBox;
        private System.Windows.Forms.CheckedListBox ConsultationCheckedListBox;
        private System.Windows.Forms.Button BtnRefreshConsultation;
        private Button BtnConsultationsNew;
        private DataGridViewTextBoxColumn Column4;
        private controls.grid.DataGridViewCurrencyColumn Column6;
        private DataGridViewTextBoxColumn Column7;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private controls.grid.DataGridViewCurrencyColumn Column3;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column9;
        private DataGridViewTextBoxColumn Column10;
        private DataGridViewTextBoxColumn Column11;
    }
}