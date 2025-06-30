namespace fa.reports.master
{
    partial class FormChartOfAccounts
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChartOfAccounts));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            toolStrip = new ToolStrip();
            toolStripLabel2 = new ToolStripLabel();
            TextBoxSearch = new ToolStripTextBox();
            toolStripLabel1 = new ToolStripLabel();
            AccountFilerComboBox1 = new ToolStripComboBox();
            SearchBtn = new ToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            ToolStripBtnSave = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            ToolStripBtnPrint = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            statusStrip1 = new StatusStrip();
            ChartofAccountErrorMsg = new ToolStripStatusLabel();
            imageList1 = new ImageList(components);
            BtnExit = new Button();
            BtnReset = new Button();
            BtnPrint = new Button();
            BtnSave = new Button();
            ChartOfAccountsGridView1 = new views.controls.DataViewVerticalScroll();
            AccountName = new DataGridViewTextBoxColumn();
            Description = new DataGridViewTextBoxColumn();
            Type = new DataGridViewTextBoxColumn();
            DetailType = new DataGridViewTextBoxColumn();
            Balance = new views.controls.grid.DataGridViewCurrencyColumn();
            CRDR = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Edit = new DataGridViewButtonColumn();
            toolStrip.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ChartOfAccountsGridView1).BeginInit();
            SuspendLayout();
            // 
            // toolStrip
            // 
            toolStrip.BackColor = SystemColors.ControlLight;
            toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip.Items.AddRange(new ToolStripItem[] { toolStripLabel2, TextBoxSearch, toolStripLabel1, AccountFilerComboBox1, SearchBtn, toolStripSeparator4, ToolStripBtnSave, toolStripSeparator1, ToolStripBtnPrint, toolStripSeparator3 });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Padding = new Padding(5);
            toolStrip.Size = new Size(1082, 33);
            toolStrip.TabIndex = 2;
            toolStrip.Text = "ToolStrip";
            toolStrip.PreviewKeyDown += toolStrip_PreviewKeyDown;
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(54, 20);
            toolStripLabel2.Text = "Search : ";
            // 
            // TextBoxSearch
            // 
            TextBoxSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxSearch.Name = "TextBoxSearch";
            TextBoxSearch.Size = new Size(300, 23);
            TextBoxSearch.KeyDown += toolStripTextBox1_KeyDown;
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(59, 20);
            toolStripLabel1.Text = "A/C Filter";
            // 
            // AccountFilerComboBox1
            // 
            AccountFilerComboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            AccountFilerComboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            AccountFilerComboBox1.FlatStyle = FlatStyle.Standard;
            AccountFilerComboBox1.Items.AddRange(new object[] { "All", "General A/C", "Suppliers (Vendor)", "Cusotmers", "Employees" });
            AccountFilerComboBox1.Name = "AccountFilerComboBox1";
            AccountFilerComboBox1.Size = new Size(200, 23);
            AccountFilerComboBox1.KeyDown += AccountFilerComboBox1_KeyDown;
            // 
            // SearchBtn
            // 
            SearchBtn.DisplayStyle = ToolStripItemDisplayStyle.Text;
            SearchBtn.Image = (Image)resources.GetObject("SearchBtn.Image");
            SearchBtn.ImageTransparentColor = Color.Magenta;
            SearchBtn.Name = "SearchBtn";
            SearchBtn.Size = new Size(26, 20);
            SearchBtn.Text = "Go";
            SearchBtn.Click += SearchBtn_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 23);
            // 
            // ToolStripBtnSave
            // 
            ToolStripBtnSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnSave.Image = (Image)resources.GetObject("ToolStripBtnSave.Image");
            ToolStripBtnSave.ImageTransparentColor = Color.Black;
            ToolStripBtnSave.Name = "ToolStripBtnSave";
            ToolStripBtnSave.Size = new Size(23, 20);
            ToolStripBtnSave.Text = "Save";
            ToolStripBtnSave.Click += saveToolStripButton_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 23);
            // 
            // ToolStripBtnPrint
            // 
            ToolStripBtnPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnPrint.Image = (Image)resources.GetObject("ToolStripBtnPrint.Image");
            ToolStripBtnPrint.ImageTransparentColor = Color.Black;
            ToolStripBtnPrint.Name = "ToolStripBtnPrint";
            ToolStripBtnPrint.Size = new Size(23, 20);
            ToolStripBtnPrint.Text = "Print";
            ToolStripBtnPrint.Click += printToolStripButton_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 23);
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ChartofAccountErrorMsg });
            statusStrip1.Location = new Point(0, 481);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1082, 22);
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "statusStrip1";
            // 
            // ChartofAccountErrorMsg
            // 
            ChartofAccountErrorMsg.Name = "ChartofAccountErrorMsg";
            ChartofAccountErrorMsg.Size = new Size(28, 17);
            ChartofAccountErrorMsg.Text = "       ";
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth8Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "edit.png");
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(998, 446);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(72, 23);
            BtnExit.TabIndex = 21;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            BtnExit.PreviewKeyDown += BtnExit_PreviewKeyDown;
            // 
            // BtnReset
            // 
            BtnReset.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnReset.Location = new Point(748, 446);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new Size(82, 23);
            BtnReset.TabIndex = 20;
            BtnReset.Text = "Reset [Esc]";
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Enabled = false;
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(917, 446);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 23);
            BtnPrint.TabIndex = 19;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += printToolStripButton_Click;
            // 
            // BtnSave
            // 
            BtnSave.Enabled = false;
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(836, 446);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 18;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += saveToolStripButton_Click;
            BtnSave.PreviewKeyDown += BtnSave_PreviewKeyDown;
            // 
            // ChartOfAccountsGridView1
            // 
            ChartOfAccountsGridView1.AllowUserToAddRows = false;
            ChartOfAccountsGridView1.AllowUserToDeleteRows = false;
            ChartOfAccountsGridView1.AllowUserToResizeColumns = false;
            ChartOfAccountsGridView1.AllowUserToResizeRows = false;
            ChartOfAccountsGridView1.BackgroundColor = SystemColors.ControlLight;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.ControlLight;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.ControlLight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            ChartOfAccountsGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            ChartOfAccountsGridView1.ColumnHeadersHeight = 20;
            ChartOfAccountsGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            ChartOfAccountsGridView1.Columns.AddRange(new DataGridViewColumn[] { AccountName, Description, Type, DetailType, Balance, CRDR, Column1, Column2, Column3, Edit });
            ChartOfAccountsGridView1.EnableHeadersVisualStyles = false;
            ChartOfAccountsGridView1.Location = new Point(5, 36);
            ChartOfAccountsGridView1.Name = "ChartOfAccountsGridView1";
            ChartOfAccountsGridView1.RowHeadersVisible = false;
            ChartOfAccountsGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ChartOfAccountsGridView1.ShowCellToolTips = false;
            ChartOfAccountsGridView1.Size = new Size(1072, 394);
            ChartOfAccountsGridView1.TabIndex = 3;
            ChartOfAccountsGridView1.CellContentClick += ChartOfAccountsGridView1_CellContentClick;
            ChartOfAccountsGridView1.CellPainting += ChartOfAccountsGridView1_CellPainting;
            // 
            // AccountName
            // 
            AccountName.HeaderText = "Name";
            AccountName.Name = "AccountName";
            AccountName.Resizable = DataGridViewTriState.False;
            AccountName.SortMode = DataGridViewColumnSortMode.NotSortable;
            AccountName.Width = 200;
            // 
            // Description
            // 
            Description.HeaderText = "Description";
            Description.Name = "Description";
            Description.Resizable = DataGridViewTriState.False;
            Description.SortMode = DataGridViewColumnSortMode.NotSortable;
            Description.Width = 300;
            // 
            // Type
            // 
            Type.HeaderText = "Type";
            Type.Name = "Type";
            Type.Resizable = DataGridViewTriState.False;
            Type.SortMode = DataGridViewColumnSortMode.NotSortable;
            Type.Width = 200;
            // 
            // DetailType
            // 
            DetailType.HeaderText = "Detail Type";
            DetailType.Name = "DetailType";
            DetailType.Resizable = DataGridViewTriState.False;
            DetailType.SortMode = DataGridViewColumnSortMode.NotSortable;
            DetailType.Width = 200;
            // 
            // Balance
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            Balance.DefaultCellStyle = dataGridViewCellStyle2;
            Balance.HeaderText = "Balance";
            Balance.Name = "Balance";
            Balance.Resizable = DataGridViewTriState.False;
            Balance.Width = 75;
            // 
            // CRDR
            // 
            CRDR.HeaderText = "CR/DR";
            CRDR.Name = "CRDR";
            CRDR.Resizable = DataGridViewTriState.False;
            CRDR.SortMode = DataGridViewColumnSortMode.NotSortable;
            CRDR.Width = 50;
            // 
            // Column1
            // 
            Column1.HeaderText = "Id";
            Column1.Name = "Column1";
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Visible = false;
            // 
            // Column2
            // 
            Column2.HeaderText = "ParentAccountId";
            Column2.Name = "Column2";
            Column2.Resizable = DataGridViewTriState.False;
            Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column2.Visible = false;
            // 
            // Column3
            // 
            Column3.HeaderText = "Type";
            Column3.Name = "Column3";
            Column3.Resizable = DataGridViewTriState.False;
            Column3.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column3.Visible = false;
            // 
            // Edit
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Format = "dv";
            Edit.DefaultCellStyle = dataGridViewCellStyle3;
            Edit.HeaderText = "...";
            Edit.Name = "Edit";
            Edit.Resizable = DataGridViewTriState.False;
            Edit.Width = 25;
            // 
            // FormChartOfAccounts
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1082, 503);
            Controls.Add(BtnExit);
            Controls.Add(BtnReset);
            Controls.Add(BtnPrint);
            Controls.Add(BtnSave);
            Controls.Add(statusStrip1);
            Controls.Add(ChartOfAccountsGridView1);
            Controls.Add(toolStrip);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormChartOfAccounts";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Chart of Accounts";
            Load += FormChartOfAccounts_Load;
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ChartOfAccountsGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip;
        private ToolStripButton ToolStripBtnSave;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton ToolStripBtnPrint;
        private ToolStripLabel toolStripLabel1;
        private ToolStripComboBox AccountFilerComboBox1;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripTextBox TextBoxSearch;
        private ToolStripSeparator toolStripSeparator4;
        private StatusStrip statusStrip1;
        private views.controls.DataViewVerticalScroll ChartOfAccountsGridView1;
        private ToolStripLabel toolStripLabel2;
        private ImageList imageList1;
        private Button BtnExit;
        private Button BtnReset;
        private Button BtnPrint;
        private Button BtnSave;
        private DataGridViewTextBoxColumn AccountName;
        private DataGridViewTextBoxColumn Description;
        private DataGridViewTextBoxColumn Type;
        private DataGridViewTextBoxColumn DetailType;
        private views.controls.grid.DataGridViewCurrencyColumn Balance;
        private DataGridViewTextBoxColumn CRDR;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewButtonColumn Edit;
        private ToolStripButton SearchBtn;
        private ToolStripStatusLabel ChartofAccountErrorMsg;
    }
}