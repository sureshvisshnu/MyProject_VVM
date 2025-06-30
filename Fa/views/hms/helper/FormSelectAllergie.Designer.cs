namespace Fa.views.hms.helper
{
    partial class FormSelectAllergie
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectAllergie));
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            AllergieSearchTextBox = new fa.views.controls.text.DelayedTextChangeTextBox();
            AllergieCheckedListBox = new CheckedListBox();
            BtnRefreshAllergie = new Button();
            statusStrip1 = new StatusStrip();
            ErrorMsgSelectedAllergie = new ToolStripStatusLabel();
            BtnNewAllergie = new Button();
            BtnAllergieCancel = new Button();
            BtnAllergieDone = new Button();
            GridViewSelectedAllergie = new fa.views.controls.DataViewVerticalScroll();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            Column9 = new DataGridViewTextBoxColumn();
            Column10 = new DataGridViewTextBoxColumn();
            Column11 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewSelectedAllergie).BeginInit();
            SuspendLayout();
            // 
            // AllergieSearchTextBox
            // 
            AllergieSearchTextBox.BorderStyle = BorderStyle.FixedSingle;
            AllergieSearchTextBox.Delay = false;
            AllergieSearchTextBox.DelayTime = 1000;
            AllergieSearchTextBox.Location = new Point(11, 6);
            AllergieSearchTextBox.MaxLength = 35;
            AllergieSearchTextBox.Name = "AllergieSearchTextBox";
            AllergieSearchTextBox.Searchstartfrom = 1;
            AllergieSearchTextBox.Size = new Size(156, 23);
            AllergieSearchTextBox.TabIndex = 86;
            AllergieSearchTextBox.TextChanged += AllergieSearchTextBox_TextChanged;
            // 
            // AllergieCheckedListBox
            // 
            AllergieCheckedListBox.CheckOnClick = true;
            AllergieCheckedListBox.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            AllergieCheckedListBox.FormattingEnabled = true;
            AllergieCheckedListBox.Location = new Point(11, 29);
            AllergieCheckedListBox.Name = "AllergieCheckedListBox";
            AllergieCheckedListBox.Size = new Size(180, 292);
            AllergieCheckedListBox.TabIndex = 87;
            AllergieCheckedListBox.ItemCheck += AllergieCheckedListBox_ItemCheck;
            AllergieCheckedListBox.PreviewKeyDown += AllergieCheckedListBox_PreviewKeyDown;
            // 
            // BtnRefreshAllergie
            // 
            BtnRefreshAllergie.BackgroundImage = (Image)resources.GetObject("BtnRefreshAllergie.BackgroundImage");
            BtnRefreshAllergie.BackgroundImageLayout = ImageLayout.Stretch;
            BtnRefreshAllergie.Image = (Image)resources.GetObject("BtnRefreshAllergie.Image");
            BtnRefreshAllergie.Location = new Point(168, 5);
            BtnRefreshAllergie.Name = "BtnRefreshAllergie";
            BtnRefreshAllergie.Size = new Size(23, 22);
            BtnRefreshAllergie.TabIndex = 92;
            BtnRefreshAllergie.TabStop = false;
            BtnRefreshAllergie.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnRefreshAllergie.UseVisualStyleBackColor = true;
            BtnRefreshAllergie.Click += BtnRefreshAllergie_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsgSelectedAllergie });
            statusStrip1.Location = new Point(0, 366);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(833, 22);
            statusStrip1.TabIndex = 91;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsgSelectedAllergie
            // 
            ErrorMsgSelectedAllergie.Name = "ErrorMsgSelectedAllergie";
            ErrorMsgSelectedAllergie.Size = new Size(16, 17);
            ErrorMsgSelectedAllergie.Text = "   ";
            // 
            // BtnNewAllergie
            // 
            BtnNewAllergie.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnNewAllergie.Location = new Point(11, 330);
            BtnNewAllergie.Name = "BtnNewAllergie";
            BtnNewAllergie.Size = new Size(85, 23);
            BtnNewAllergie.TabIndex = 89;
            BtnNewAllergie.Text = "New [F3]";
            BtnNewAllergie.UseVisualStyleBackColor = true;
            BtnNewAllergie.Click += BtnNewAllergie_Click;
            // 
            // BtnAllergieCancel
            // 
            BtnAllergieCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnAllergieCancel.Location = new Point(657, 330);
            BtnAllergieCancel.Name = "BtnAllergieCancel";
            BtnAllergieCancel.Size = new Size(85, 23);
            BtnAllergieCancel.TabIndex = 90;
            BtnAllergieCancel.Text = "Reset [Esc]";
            BtnAllergieCancel.UseVisualStyleBackColor = true;
            BtnAllergieCancel.Click += BtnAllergieCancel_Click;
            // 
            // BtnAllergieDone
            // 
            BtnAllergieDone.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnAllergieDone.Location = new Point(748, 330);
            BtnAllergieDone.Name = "BtnAllergieDone";
            BtnAllergieDone.Size = new Size(75, 23);
            BtnAllergieDone.TabIndex = 88;
            BtnAllergieDone.Text = "Done [F8]";
            BtnAllergieDone.UseVisualStyleBackColor = true;
            BtnAllergieDone.Click += BtnAllergiesDone_Click;
            BtnAllergieDone.PreviewKeyDown += BtnAllergieDone_PreviewKeyDown;
            // 
            // GridViewSelectedAllergie
            // 
            GridViewSelectedAllergie.AllowUserToAddRows = false;
            GridViewSelectedAllergie.AllowUserToDeleteRows = false;
            GridViewSelectedAllergie.AllowUserToResizeColumns = false;
            GridViewSelectedAllergie.AllowUserToResizeRows = false;
            GridViewSelectedAllergie.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            GridViewSelectedAllergie.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Control;
            dataGridViewCellStyle6.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle6.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            GridViewSelectedAllergie.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            GridViewSelectedAllergie.ColumnHeadersHeight = 20;
            GridViewSelectedAllergie.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewSelectedAllergie.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column5, Column9, Column10, Column11, Column3 });
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = SystemColors.Window;
            dataGridViewCellStyle10.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle10.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.True;
            GridViewSelectedAllergie.DefaultCellStyle = dataGridViewCellStyle10;
            GridViewSelectedAllergie.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewSelectedAllergie.EnableHeadersVisualStyles = false;
            GridViewSelectedAllergie.Location = new Point(200, 7);
            GridViewSelectedAllergie.MultiSelect = false;
            GridViewSelectedAllergie.Name = "GridViewSelectedAllergie";
            GridViewSelectedAllergie.RowHeadersVisible = false;
            GridViewSelectedAllergie.RowTemplate.Height = 20;
            GridViewSelectedAllergie.ScrollBars = ScrollBars.Vertical;
            GridViewSelectedAllergie.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewSelectedAllergie.ShowCellToolTips = false;
            GridViewSelectedAllergie.Size = new Size(623, 314);
            GridViewSelectedAllergie.TabIndex = 93;
            GridViewSelectedAllergie.CellBeginEdit += GridViewSelectedAllergies_CellBeginEdit;
            GridViewSelectedAllergie.CellClick += GridViewSelectedAllergies_CellClick;
            GridViewSelectedAllergie.CellEndEdit += GridViewSelectedAllergies_CellEndEdit;
            GridViewSelectedAllergie.CellEnter += GridViewSelectedAllergies_CellEnter;
            GridViewSelectedAllergie.DataError += GridViewSelectedAllergies_DataError;
            GridViewSelectedAllergie.EditingControlShowing += GridViewSelectedAllergies_EditingControlShowing;
            GridViewSelectedAllergie.SelectionChanged += GridViewSelectedAllergies_SelectionChanged;
            GridViewSelectedAllergie.Enter += GridViewSelectedAllergies_Enter;
            // 
            // Column1
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.TopLeft;
            Column1.DefaultCellStyle = dataGridViewCellStyle7;
            Column1.HeaderText = "Name";
            Column1.MaxInputLength = 250;
            Column1.Name = "Column1";
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 278;
            // 
            // Column2
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            Column2.DefaultCellStyle = dataGridViewCellStyle8;
            Column2.HeaderText = "Description";
            Column2.MaxInputLength = 250;
            Column2.Name = "Column2";
            Column2.Resizable = DataGridViewTriState.False;
            Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column2.Width = 297;
            // 
            // Column5
            // 
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle9.BackColor = Color.White;
            dataGridViewCellStyle9.ForeColor = Color.Black;
            dataGridViewCellStyle9.NullValue = "X";
            dataGridViewCellStyle9.SelectionBackColor = Color.White;
            dataGridViewCellStyle9.SelectionForeColor = Color.Black;
            Column5.DefaultCellStyle = dataGridViewCellStyle9;
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
            // FormSelectAllergie
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(833, 388);
            Controls.Add(GridViewSelectedAllergie);
            Controls.Add(AllergieSearchTextBox);
            Controls.Add(AllergieCheckedListBox);
            Controls.Add(BtnRefreshAllergie);
            Controls.Add(statusStrip1);
            Controls.Add(BtnNewAllergie);
            Controls.Add(BtnAllergieCancel);
            Controls.Add(BtnAllergieDone);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSelectAllergie";
            Text = "Select Allergie";
            Load += SelectAllergie_Load;
            Controls.SetChildIndex(BtnAllergieDone, 0);
            Controls.SetChildIndex(BtnAllergieCancel, 0);
            Controls.SetChildIndex(BtnNewAllergie, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(BtnRefreshAllergie, 0);
            Controls.SetChildIndex(AllergieCheckedListBox, 0);
            Controls.SetChildIndex(AllergieSearchTextBox, 0);
            Controls.SetChildIndex(GridViewSelectedAllergie, 0);
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewSelectedAllergie).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private fa.views.controls.text.DelayedTextChangeTextBox AllergieSearchTextBox;
        private CheckedListBox AllergieCheckedListBox;
        private Button BtnRefreshAllergie;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel ErrorMsgSelectedAllergie;
        private Button BtnNewAllergie;
        private Button BtnAllergieCancel;
        public Button BtnAllergieDone;
        private fa.views.controls.DataViewVerticalScroll GridViewSelectedAllergie;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column9;
        private DataGridViewTextBoxColumn Column10;
        private DataGridViewTextBoxColumn Column11;
        private DataGridViewTextBoxColumn Column3;
    }
}