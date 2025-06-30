namespace fa.views.hms.ward
{
    partial class FormBedType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBedType));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            statusStrip1 = new StatusStrip();
            BedTypeErrorMsg = new ToolStripStatusLabel();
            BtnBedTypeExit = new Button();
            TreeViewBedType = new TreeView();
            ImageListCompany = new ImageList(components);
            BtnBedTypeSave = new Button();
            BtnBedTypeCancel = new Button();
            BtnBedTypeEdit = new Button();
            BtnBedTypeDelete = new Button();
            BtnBedTypeNew = new Button();
            LabelCompanyName = new Label();
            TextBoxBedTypeName = new TextBox();
            TabControlBedType = new TabControl();
            TabBedTypeInfo = new TabPage();
            label1 = new Label();
            GridViewRateInfo = new controls.DataViewVerticalScroll();
            RentType = new DataGridViewComboBoxColumn();
            Rate = new controls.grid.DataGridViewCurrencyColumn();
            Column1 = new DataGridViewButtonColumn();
            TextBoxBedTypeId = new TextBox();
            TextBoxBedTypeSearch = new controls.text.DelayedTextChangeTextBox();
            statusStrip1.SuspendLayout();
            TabControlBedType.SuspendLayout();
            TabBedTypeInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewRateInfo).BeginInit();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(172, 215);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(172, 189);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(172, 163);
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { BedTypeErrorMsg });
            statusStrip1.Location = new Point(0, 409);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(783, 22);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // BedTypeErrorMsg
            // 
            BedTypeErrorMsg.Name = "BedTypeErrorMsg";
            BedTypeErrorMsg.Size = new Size(52, 17);
            BedTypeErrorMsg.Text = "               ";
            // 
            // BtnBedTypeExit
            // 
            BtnBedTypeExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnBedTypeExit.Location = new Point(689, 381);
            BtnBedTypeExit.Name = "BtnBedTypeExit";
            BtnBedTypeExit.Size = new Size(83, 23);
            BtnBedTypeExit.TabIndex = 9;
            BtnBedTypeExit.Text = "Exit [F10]";
            BtnBedTypeExit.UseVisualStyleBackColor = true;
            BtnBedTypeExit.Click += BtnBedTypeExit_Click;
            // 
            // TreeViewBedType
            // 
            TreeViewBedType.HideSelection = false;
            TreeViewBedType.ImageIndex = 0;
            TreeViewBedType.ImageList = ImageListCompany;
            TreeViewBedType.Location = new Point(11, 38);
            TreeViewBedType.Name = "TreeViewBedType";
            TreeViewBedType.SelectedImageIndex = 0;
            TreeViewBedType.Size = new Size(239, 339);
            TreeViewBedType.TabIndex = 1;
            TreeViewBedType.AfterSelect += TreeViewBedType_AfterSelect;
            // 
            // ImageListCompany
            // 
            ImageListCompany.ColorDepth = ColorDepth.Depth8Bit;
            ImageListCompany.ImageStream = (ImageListStreamer)resources.GetObject("ImageListCompany.ImageStream");
            ImageListCompany.TransparentColor = Color.Transparent;
            ImageListCompany.Images.SetKeyName(0, "bed.png");
            // 
            // BtnBedTypeSave
            // 
            BtnBedTypeSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnBedTypeSave.Location = new Point(600, 381);
            BtnBedTypeSave.Name = "BtnBedTypeSave";
            BtnBedTypeSave.Size = new Size(83, 23);
            BtnBedTypeSave.TabIndex = 7;
            BtnBedTypeSave.Text = "Save [F8]";
            BtnBedTypeSave.UseVisualStyleBackColor = true;
            BtnBedTypeSave.Click += BtnBedTypeSave_Click;
            BtnBedTypeSave.PreviewKeyDown += BtnBedTypeSave_PreviewKeyDown;
            // 
            // BtnBedTypeCancel
            // 
            BtnBedTypeCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnBedTypeCancel.Location = new Point(511, 381);
            BtnBedTypeCancel.Name = "BtnBedTypeCancel";
            BtnBedTypeCancel.Size = new Size(83, 23);
            BtnBedTypeCancel.TabIndex = 8;
            BtnBedTypeCancel.Text = "Cancel [Esc]";
            BtnBedTypeCancel.UseVisualStyleBackColor = true;
            BtnBedTypeCancel.Click += BtnBedTypeCancel_Click;
            // 
            // BtnBedTypeEdit
            // 
            BtnBedTypeEdit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnBedTypeEdit.Location = new Point(190, 381);
            BtnBedTypeEdit.Name = "BtnBedTypeEdit";
            BtnBedTypeEdit.Size = new Size(75, 23);
            BtnBedTypeEdit.TabIndex = 4;
            BtnBedTypeEdit.Text = "Edit [F7]";
            BtnBedTypeEdit.UseVisualStyleBackColor = true;
            BtnBedTypeEdit.Click += BtnBedTypeEdit_Click;
            // 
            // BtnBedTypeDelete
            // 
            BtnBedTypeDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnBedTypeDelete.Location = new Point(101, 381);
            BtnBedTypeDelete.Name = "BtnBedTypeDelete";
            BtnBedTypeDelete.Size = new Size(83, 23);
            BtnBedTypeDelete.TabIndex = 3;
            BtnBedTypeDelete.Text = "Delete [F4]";
            BtnBedTypeDelete.UseVisualStyleBackColor = true;
            BtnBedTypeDelete.Click += BtnBedTypeDelete_Click;
            // 
            // BtnBedTypeNew
            // 
            BtnBedTypeNew.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnBedTypeNew.Location = new Point(12, 381);
            BtnBedTypeNew.Name = "BtnBedTypeNew";
            BtnBedTypeNew.Size = new Size(83, 23);
            BtnBedTypeNew.TabIndex = 2;
            BtnBedTypeNew.Text = "New [F3]";
            BtnBedTypeNew.UseVisualStyleBackColor = true;
            BtnBedTypeNew.Click += BtnBedTypeNew_Click;
            // 
            // LabelCompanyName
            // 
            LabelCompanyName.AutoSize = true;
            LabelCompanyName.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelCompanyName.Location = new Point(16, 21);
            LabelCompanyName.Name = "LabelCompanyName";
            LabelCompanyName.Size = new Size(39, 13);
            LabelCompanyName.TabIndex = 0;
            LabelCompanyName.Text = "Name";
            // 
            // TextBoxBedTypeName
            // 
            TextBoxBedTypeName.BackColor = Color.White;
            TextBoxBedTypeName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxBedTypeName.Location = new Point(19, 39);
            TextBoxBedTypeName.MaxLength = 10;
            TextBoxBedTypeName.Name = "TextBoxBedTypeName";
            TextBoxBedTypeName.ReadOnly = true;
            TextBoxBedTypeName.Size = new Size(401, 21);
            TextBoxBedTypeName.TabIndex = 5;
            TextBoxBedTypeName.PreviewKeyDown += TextBoxBedTypeName_PreviewKeyDown;
            // 
            // TabControlBedType
            // 
            TabControlBedType.Controls.Add(TabBedTypeInfo);
            TabControlBedType.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            TabControlBedType.Location = new Point(257, 11);
            TabControlBedType.Name = "TabControlBedType";
            TabControlBedType.SelectedIndex = 0;
            TabControlBedType.ShowToolTips = true;
            TabControlBedType.Size = new Size(519, 366);
            TabControlBedType.TabIndex = 5;
            TabControlBedType.TabStop = false;
            // 
            // TabBedTypeInfo
            // 
            TabBedTypeInfo.BackColor = Color.White;
            TabBedTypeInfo.Controls.Add(label1);
            TabBedTypeInfo.Controls.Add(GridViewRateInfo);
            TabBedTypeInfo.Controls.Add(TextBoxBedTypeName);
            TabBedTypeInfo.Controls.Add(LabelCompanyName);
            TabBedTypeInfo.Location = new Point(4, 22);
            TabBedTypeInfo.Name = "TabBedTypeInfo";
            TabBedTypeInfo.Padding = new Padding(3);
            TabBedTypeInfo.Size = new Size(511, 340);
            TabBedTypeInfo.TabIndex = 0;
            TabBedTypeInfo.Text = "Bed Type Info";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 65);
            label1.Name = "label1";
            label1.Size = new Size(105, 13);
            label1.TabIndex = 34;
            label1.Text = "Rate Information";
            // 
            // GridViewRateInfo
            // 
            GridViewRateInfo.AllowUserToAddRows = false;
            GridViewRateInfo.AllowUserToDeleteRows = false;
            GridViewRateInfo.AllowUserToResizeColumns = false;
            GridViewRateInfo.AllowUserToResizeRows = false;
            GridViewRateInfo.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewRateInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewRateInfo.ColumnHeadersHeight = 20;
            GridViewRateInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewRateInfo.Columns.AddRange(new DataGridViewColumn[] { RentType, Rate, Column1 });
            GridViewRateInfo.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewRateInfo.EnableHeadersVisualStyles = false;
            GridViewRateInfo.Location = new Point(19, 83);
            GridViewRateInfo.MultiSelect = false;
            GridViewRateInfo.Name = "GridViewRateInfo";
            GridViewRateInfo.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = Color.White;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.WindowText;
            GridViewRateInfo.RowsDefaultCellStyle = dataGridViewCellStyle4;
            GridViewRateInfo.RowTemplate.Height = 20;
            GridViewRateInfo.ScrollBars = ScrollBars.Vertical;
            GridViewRateInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewRateInfo.ShowCellToolTips = false;
            GridViewRateInfo.Size = new Size(470, 219);
            GridViewRateInfo.TabIndex = 6;
            GridViewRateInfo.CellContentClick += GridViewRateInfo_CellContentClick;
            GridViewRateInfo.CellEnter += GridViewRateInfo_CellEnter;
            GridViewRateInfo.CellLeave += GridViewRateInfo_CellLeave;
            GridViewRateInfo.DataError += GridViewRateInfo_DataError;
            GridViewRateInfo.EditingControlShowing += GridViewRateInfo_EditingControlShowing;
            GridViewRateInfo.RowsAdded += GridViewRateInfo_RowsAdded;
            // 
            // RentType
            // 
            RentType.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            RentType.FlatStyle = FlatStyle.Flat;
            RentType.HeaderText = "Rent Period";
            RentType.Items.AddRange(new object[] { "MONTHLY", "DAILY", "WEEKLY", "HOURLY", "TWELVEHOURS", "FOURHOURS" });
            RentType.Name = "RentType";
            RentType.Resizable = DataGridViewTriState.False;
            // 
            // Rate
            // 
            Rate.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.NullValue = "0.00";
            Rate.DefaultCellStyle = dataGridViewCellStyle2;
            Rate.HeaderText = "Rate";
            Rate.Name = "Rate";
            Rate.Resizable = DataGridViewTriState.False;
            // 
            // Column1
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.ForeColor = Color.Silver;
            dataGridViewCellStyle3.SelectionForeColor = Color.Silver;
            Column1.DefaultCellStyle = dataGridViewCellStyle3;
            Column1.HeaderText = "";
            Column1.Name = "Column1";
            Column1.Resizable = DataGridViewTriState.False;
            Column1.Width = 25;
            // 
            // TextBoxBedTypeId
            // 
            TextBoxBedTypeId.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxBedTypeId.Location = new Point(311, 383);
            TextBoxBedTypeId.Name = "TextBoxBedTypeId";
            TextBoxBedTypeId.Size = new Size(126, 21);
            TextBoxBedTypeId.TabIndex = 37;
            TextBoxBedTypeId.TabStop = false;
            TextBoxBedTypeId.Visible = false;
            // 
            // TextBoxBedTypeSearch
            // 
            TextBoxBedTypeSearch.BackColor = SystemColors.Window;
            TextBoxBedTypeSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxBedTypeSearch.Delay = true;
            TextBoxBedTypeSearch.DelayTime = 1000;
            TextBoxBedTypeSearch.Location = new Point(11, 11);
            TextBoxBedTypeSearch.MaxLength = 35;
            TextBoxBedTypeSearch.Name = "TextBoxBedTypeSearch";
            TextBoxBedTypeSearch.Searchstartfrom = 2;
            TextBoxBedTypeSearch.Size = new Size(239, 21);
            TextBoxBedTypeSearch.TabIndex = 38;
            TextBoxBedTypeSearch.TextChanged += TextBoxBedTypeSearch_TextChanged;
            TextBoxBedTypeSearch.KeyDown += TextBoxBedTypeSearch_KeyDown;
            // 
            // FormBedType
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(783, 431);
            Controls.Add(TextBoxBedTypeSearch);
            Controls.Add(BtnBedTypeExit);
            Controls.Add(TreeViewBedType);
            Controls.Add(BtnBedTypeCancel);
            Controls.Add(BtnBedTypeSave);
            Controls.Add(BtnBedTypeDelete);
            Controls.Add(BtnBedTypeNew);
            Controls.Add(BtnBedTypeEdit);
            Controls.Add(TextBoxBedTypeId);
            Controls.Add(statusStrip1);
            Controls.Add(TabControlBedType);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormBedType";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bed/Room Type";
            FormClosing += FormBedType_FormClosing;
            Load += BedType_Load;
            Controls.SetChildIndex(TabControlBedType, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(TextBoxBedTypeId, 0);
            Controls.SetChildIndex(BtnBedTypeEdit, 0);
            Controls.SetChildIndex(BtnBedTypeNew, 0);
            Controls.SetChildIndex(BtnBedTypeDelete, 0);
            Controls.SetChildIndex(BtnBedTypeSave, 0);
            Controls.SetChildIndex(BtnBedTypeCancel, 0);
            Controls.SetChildIndex(TreeViewBedType, 0);
            Controls.SetChildIndex(BtnBedTypeExit, 0);
            Controls.SetChildIndex(TextBoxBedTypeSearch, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            TabControlBedType.ResumeLayout(false);
            TabBedTypeInfo.ResumeLayout(false);
            TabBedTypeInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewRateInfo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusStrip1;
        private Button BtnBedTypeExit;
        private TreeView TreeViewBedType;
        private ImageList ImageListCompany;
        private Button BtnBedTypeSave;
        private Button BtnBedTypeCancel;
        private Button BtnBedTypeEdit;
        private Button BtnBedTypeDelete;
        private Button BtnBedTypeNew;
        private Label LabelCompanyName;
        private TextBox TextBoxBedTypeName;
        private TabControl TabControlBedType;
        private TabPage TabBedTypeInfo;
        private TextBox TextBoxBedTypeId;
        private controls.DataViewVerticalScroll GridViewRateInfo;
        private Label label1;
        private ToolStripStatusLabel BedTypeErrorMsg;
        private DataGridViewComboBoxColumn RentType;
        private controls.grid.DataGridViewCurrencyColumn Rate;
        private DataGridViewButtonColumn Column1;
        private controls.text.DelayedTextChangeTextBox TextBoxBedTypeSearch;
    }
}