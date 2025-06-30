namespace fa.views.hms.helper
{
    partial class FormSelectProcedures
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectProcedures));
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            GridViewSelectProcedures = new controls.DataViewVerticalScroll();
            BtnSelectProceduresCancel = new Button();
            BtnSelectProceduresDone = new Button();
            SelectProceduresStatusStrip = new StatusStrip();
            ErrorMsgSelectProcedures = new ToolStripStatusLabel();
            ProcedureSearchTextBox = new controls.text.DelayedTextChangeTextBox();
            ProcedureCheckedListBox = new CheckedListBox();
            BtnRefreshProcedure = new Button();
            BtnProceduresNew = new Button();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new controls.grid.DataGridViewCurrencyColumn();
            Column5 = new DataGridViewTextBoxColumn();
            Column9 = new DataGridViewTextBoxColumn();
            Column10 = new DataGridViewTextBoxColumn();
            Column11 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)GridViewSelectProcedures).BeginInit();
            SelectProceduresStatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // PatientIdTransport
            // 
            PatientIdTransport.Size = new Size(100, 21);
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(176, 206);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(176, 180);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(176, 154);
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // checkBoxIsPatient
            // 
            checkBoxIsPatient.Size = new Size(69, 17);
            // 
            // GridViewSelectProcedures
            // 
            GridViewSelectProcedures.AllowUserToAddRows = false;
            GridViewSelectProcedures.AllowUserToDeleteRows = false;
            GridViewSelectProcedures.AllowUserToResizeColumns = false;
            GridViewSelectProcedures.AllowUserToResizeRows = false;
            GridViewSelectProcedures.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            GridViewSelectProcedures.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewSelectProcedures.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewSelectProcedures.ColumnHeadersHeight = 20;
            GridViewSelectProcedures.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewSelectProcedures.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column5, Column9, Column10, Column11, Column4 });
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Window;
            dataGridViewCellStyle6.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle6.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            GridViewSelectProcedures.DefaultCellStyle = dataGridViewCellStyle6;
            GridViewSelectProcedures.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewSelectProcedures.EnableHeadersVisualStyles = false;
            GridViewSelectProcedures.Location = new Point(200, 11);
            GridViewSelectProcedures.MultiSelect = false;
            GridViewSelectProcedures.Name = "GridViewSelectProcedures";
            GridViewSelectProcedures.RowHeadersVisible = false;
            GridViewSelectProcedures.RowTemplate.Height = 20;
            GridViewSelectProcedures.ScrollBars = ScrollBars.Vertical;
            GridViewSelectProcedures.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewSelectProcedures.ShowCellToolTips = false;
            GridViewSelectProcedures.Size = new Size(722, 315);
            GridViewSelectProcedures.TabIndex = 2;
            GridViewSelectProcedures.CellBeginEdit += GridViewSelectProcedures_CellBeginEdit;
            GridViewSelectProcedures.CellClick += GridViewSelectProcedures_CellClick;
            GridViewSelectProcedures.CellEndEdit += GridViewSelectProcedures_CellEndEdit;
            GridViewSelectProcedures.CellEnter += GridViewSelectProcedures_CellEnter;
            GridViewSelectProcedures.CellLeave += GridViewSelectProcedures_CellLeave;
            GridViewSelectProcedures.DataError += GridViewSelectProcedures_DataError;
            GridViewSelectProcedures.EditingControlShowing += GridViewSelectProcedures_EditingControlShowing;
            GridViewSelectProcedures.SelectionChanged += GridViewSelectProcedures_SelectionChanged;
            GridViewSelectProcedures.Enter += GridViewSelectProcedures_Enter;
            // 
            // BtnSelectProceduresCancel
            // 
            BtnSelectProceduresCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSelectProceduresCancel.Location = new Point(731, 332);
            BtnSelectProceduresCancel.Name = "BtnSelectProceduresCancel";
            BtnSelectProceduresCancel.Size = new Size(93, 23);
            BtnSelectProceduresCancel.TabIndex = 4;
            BtnSelectProceduresCancel.Text = "Reset [Esc]";
            BtnSelectProceduresCancel.UseVisualStyleBackColor = true;
            BtnSelectProceduresCancel.Click += BtnSelectProceduresCancel_Click;
            // 
            // BtnSelectProceduresDone
            // 
            BtnSelectProceduresDone.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSelectProceduresDone.Location = new Point(830, 331);
            BtnSelectProceduresDone.Name = "BtnSelectProceduresDone";
            BtnSelectProceduresDone.Size = new Size(93, 23);
            BtnSelectProceduresDone.TabIndex = 3;
            BtnSelectProceduresDone.Text = "Done [F8]";
            BtnSelectProceduresDone.UseVisualStyleBackColor = true;
            BtnSelectProceduresDone.Click += BtnSelectProceduresDone_Click;
            BtnSelectProceduresDone.PreviewKeyDown += BtnSelectProceduresDone_PreviewKeyDown;
            // 
            // SelectProceduresStatusStrip
            // 
            SelectProceduresStatusStrip.Items.AddRange(new ToolStripItem[] { ErrorMsgSelectProcedures });
            SelectProceduresStatusStrip.Location = new Point(0, 366);
            SelectProceduresStatusStrip.Name = "SelectProceduresStatusStrip";
            SelectProceduresStatusStrip.Size = new Size(933, 22);
            SelectProceduresStatusStrip.TabIndex = 70;
            SelectProceduresStatusStrip.Text = "statusStrip1";
            // 
            // ErrorMsgSelectProcedures
            // 
            ErrorMsgSelectProcedures.Name = "ErrorMsgSelectProcedures";
            ErrorMsgSelectProcedures.Size = new Size(16, 17);
            ErrorMsgSelectProcedures.Text = "   ";
            // 
            // ProcedureSearchTextBox
            // 
            ProcedureSearchTextBox.BorderStyle = BorderStyle.FixedSingle;
            ProcedureSearchTextBox.Delay = false;
            ProcedureSearchTextBox.DelayTime = 1000;
            ProcedureSearchTextBox.Location = new Point(11, 11);
            ProcedureSearchTextBox.MaxLength = 35;
            ProcedureSearchTextBox.Name = "ProcedureSearchTextBox";
            ProcedureSearchTextBox.Searchstartfrom = 1;
            ProcedureSearchTextBox.Size = new Size(156, 21);
            ProcedureSearchTextBox.TabIndex = 0;
            ProcedureSearchTextBox.TextChanged += ProcedureSearchTextBox_TextChanged;
            // 
            // ProcedureCheckedListBox
            // 
            ProcedureCheckedListBox.CheckOnClick = true;
            ProcedureCheckedListBox.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ProcedureCheckedListBox.FormattingEnabled = true;
            ProcedureCheckedListBox.Location = new Point(11, 34);
            ProcedureCheckedListBox.Name = "ProcedureCheckedListBox";
            ProcedureCheckedListBox.Size = new Size(180, 292);
            ProcedureCheckedListBox.TabIndex = 1;
            ProcedureCheckedListBox.ItemCheck += ProcedureCheckedListBox_ItemCheck;
            ProcedureCheckedListBox.PreviewKeyDown += ProcedureCheckedListBox_PreviewKeyDown;
            // 
            // BtnRefreshProcedure
            // 
            BtnRefreshProcedure.BackgroundImage = (Image)resources.GetObject("BtnRefreshProcedure.BackgroundImage");
            BtnRefreshProcedure.BackgroundImageLayout = ImageLayout.Stretch;
            BtnRefreshProcedure.Image = (Image)resources.GetObject("BtnRefreshProcedure.Image");
            BtnRefreshProcedure.Location = new Point(168, 10);
            BtnRefreshProcedure.Name = "BtnRefreshProcedure";
            BtnRefreshProcedure.Size = new Size(23, 22);
            BtnRefreshProcedure.TabIndex = 85;
            BtnRefreshProcedure.TabStop = false;
            BtnRefreshProcedure.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnRefreshProcedure.UseVisualStyleBackColor = true;
            BtnRefreshProcedure.Click += BtnRefreshProcedure_Click;
            // 
            // BtnProceduresNew
            // 
            BtnProceduresNew.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnProceduresNew.Location = new Point(11, 332);
            BtnProceduresNew.Name = "BtnProceduresNew";
            BtnProceduresNew.Size = new Size(93, 23);
            BtnProceduresNew.TabIndex = 87;
            BtnProceduresNew.Text = "New [F3]";
            BtnProceduresNew.UseVisualStyleBackColor = true;
            BtnProceduresNew.Click += BtnProceduresNew_Click;
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            Column1.DefaultCellStyle = dataGridViewCellStyle2;
            Column1.HeaderText = "Name";
            Column1.Name = "Column1";
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 240;
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
            Column2.Width = 359;
            // 
            // Column3
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            Column3.DefaultCellStyle = dataGridViewCellStyle4;
            Column3.HeaderText = "Fee";
            Column3.Name = "Column3";
            Column3.Resizable = DataGridViewTriState.False;
            Column3.Width = 75;
            // 
            // Column5
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.TopCenter;
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
            // Column4
            // 
            Column4.HeaderText = "IsActive";
            Column4.Name = "Column4";
            Column4.Resizable = DataGridViewTriState.False;
            Column4.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column4.Visible = false;
            // 
            // FormSelectProcedures
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(933, 388);
            Controls.Add(BtnProceduresNew);
            Controls.Add(ProcedureSearchTextBox);
            Controls.Add(ProcedureCheckedListBox);
            Controls.Add(BtnRefreshProcedure);
            Controls.Add(SelectProceduresStatusStrip);
            Controls.Add(GridViewSelectProcedures);
            Controls.Add(BtnSelectProceduresCancel);
            Controls.Add(BtnSelectProceduresDone);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSelectProcedures";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select Medical Procedures";
            Load += FormSelectProcedures_Load;
            Controls.SetChildIndex(checkBoxIsPatient, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(BtnSelectProceduresDone, 0);
            Controls.SetChildIndex(BtnSelectProceduresCancel, 0);
            Controls.SetChildIndex(GridViewSelectProcedures, 0);
            Controls.SetChildIndex(SelectProceduresStatusStrip, 0);
            Controls.SetChildIndex(BtnRefreshProcedure, 0);
            Controls.SetChildIndex(ProcedureCheckedListBox, 0);
            Controls.SetChildIndex(ProcedureSearchTextBox, 0);
            Controls.SetChildIndex(BtnProceduresNew, 0);
            ((System.ComponentModel.ISupportInitialize)GridViewSelectProcedures).EndInit();
            SelectProceduresStatusStrip.ResumeLayout(false);
            SelectProceduresStatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private controls.DataViewVerticalScroll GridViewSelectProcedures;
        private System.Windows.Forms.Button BtnSelectProceduresCancel;
        private System.Windows.Forms.Button BtnSelectProceduresDone;
        private System.Windows.Forms.StatusStrip SelectProceduresStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel ErrorMsgSelectProcedures;
        private controls.text.DelayedTextChangeTextBox ProcedureSearchTextBox;
        private System.Windows.Forms.CheckedListBox ProcedureCheckedListBox;
        private System.Windows.Forms.Button BtnRefreshProcedure;
        private Button BtnProceduresNew;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private controls.grid.DataGridViewCurrencyColumn Column3;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column9;
        private DataGridViewTextBoxColumn Column10;
        private DataGridViewTextBoxColumn Column11;
        private DataGridViewTextBoxColumn Column4;
    }
}