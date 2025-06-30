namespace fa.views.hms.ward
{
    partial class FormWardAndBed
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWardAndBed));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            BtnWardExit = new Button();
            TabBedTypeInfo = new TabPage();
            ComboBoxSwapTextBoxBedType = new controls.ComboBoxSwapTextBox();
            ComboBoxSwapTextBoxWard = new controls.ComboBoxSwapTextBox();
            label3 = new Label();
            label2 = new Label();
            TextBoxBedName = new TextBox();
            LabelCompanyName = new Label();
            BtnWardDelete = new Button();
            TabControlBed = new TabControl();
            TextBoxWardBedId = new TextBox();
            statusStrip1 = new StatusStrip();
            WardErrorMsg = new ToolStripStatusLabel();
            TreeViewWardBed = new TreeView();
            ContextMenuStripWardBed = new ContextMenuStrip(components);
            newWardToolStripMenuItem = new ToolStripMenuItem();
            newBedToolStripMenuItem = new ToolStripMenuItem();
            ImageListWardBed = new ImageList(components);
            TabControlWard = new TabControl();
            tabPage1 = new TabPage();
            InventoryLocationcheckBox = new CheckBox();
            linkLabelWardBedType = new LinkLabel();
            label1 = new Label();
            GridViewBedInfo = new controls.DataViewVerticalScroll();
            BedNameOrId = new controls.grid.DataGridViewNameColumn();
            Type = new DataGridViewComboBoxColumn();
            Column1 = new DataGridViewButtonColumn();
            Column2 = new DataGridViewTextBoxColumn();
            TextBoxWardName = new TextBox();
            label5 = new Label();
            BtnWardSave = new Button();
            BtnWardCancel = new Button();
            BtnWardEdit = new Button();
            TextBoxWardSearch = new controls.text.DelayedTextChangeTextBox();
            BtnWardBedNew = new Dropdown_Button.UserControlButtonWithMenu();
            TabBedTypeInfo.SuspendLayout();
            TabControlBed.SuspendLayout();
            statusStrip1.SuspendLayout();
            ContextMenuStripWardBed.SuspendLayout();
            TabControlWard.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewBedInfo).BeginInit();
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
            // BtnWardExit
            // 
            BtnWardExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnWardExit.Location = new Point(697, 378);
            BtnWardExit.Name = "BtnWardExit";
            BtnWardExit.Size = new Size(83, 23);
            BtnWardExit.TabIndex = 9;
            BtnWardExit.Text = "Exit [F10]";
            BtnWardExit.UseVisualStyleBackColor = true;
            BtnWardExit.Click += BtnWardExit_Click;
            // 
            // TabBedTypeInfo
            // 
            TabBedTypeInfo.BackColor = SystemColors.Window;
            TabBedTypeInfo.Controls.Add(ComboBoxSwapTextBoxWard);
            TabBedTypeInfo.Controls.Add(ComboBoxSwapTextBoxBedType);
            TabBedTypeInfo.Controls.Add(label3);
            TabBedTypeInfo.Controls.Add(label2);
            TabBedTypeInfo.Controls.Add(TextBoxBedName);
            TabBedTypeInfo.Controls.Add(LabelCompanyName);
            TabBedTypeInfo.Location = new Point(4, 22);
            TabBedTypeInfo.Name = "TabBedTypeInfo";
            TabBedTypeInfo.Padding = new Padding(3);
            TabBedTypeInfo.Size = new Size(528, 340);
            TabBedTypeInfo.TabIndex = 0;
            TabBedTypeInfo.Text = "Bed Info";
            // 
            // ComboBoxSwapTextBoxBedType
            // 
            ComboBoxSwapTextBoxBedType.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxSwapTextBoxBedType.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxSwapTextBoxBedType.FormattingEnabled = true;
            ComboBoxSwapTextBoxBedType.Location = new Point(20, 73);
            ComboBoxSwapTextBoxBedType.Name = "ComboBoxSwapTextBoxBedType";
            ComboBoxSwapTextBoxBedType.Size = new Size(229, 21);
            ComboBoxSwapTextBoxBedType.TabIndex = 7;
            ComboBoxSwapTextBoxBedType.TxtVisible = true;
            ComboBoxSwapTextBoxBedType.KeyPress += ComboBoxSwapTextBoxBedType_KeyPress;
            // 
            // ComboBoxSwapTextBoxWard
            // 
            ComboBoxSwapTextBoxWard.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxSwapTextBoxWard.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxSwapTextBoxWard.FormattingEnabled = true;
            ComboBoxSwapTextBoxWard.Location = new Point(20, 116);
            ComboBoxSwapTextBoxWard.Name = "ComboBoxSwapTextBoxWard";
            ComboBoxSwapTextBoxWard.Size = new Size(229, 21);
            ComboBoxSwapTextBoxWard.TabIndex = 8;
            ComboBoxSwapTextBoxWard.TxtVisible = true;
            ComboBoxSwapTextBoxWard.KeyPress += ComboBoxSwapTextBoxWard_KeyPress;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(18, 98);
            label3.Name = "label3";
            label3.Size = new Size(37, 13);
            label3.TabIndex = 36;
            label3.Text = "Ward";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(18, 55);
            label2.Name = "label2";
            label2.Size = new Size(59, 13);
            label2.TabIndex = 34;
            label2.Text = "Bed Type";
            // 
            // TextBoxBedName
            // 
            TextBoxBedName.BackColor = Color.White;
            TextBoxBedName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxBedName.Location = new Point(20, 29);
            TextBoxBedName.MaxLength = 10;
            TextBoxBedName.Name = "TextBoxBedName";
            TextBoxBedName.ReadOnly = true;
            TextBoxBedName.Size = new Size(401, 21);
            TextBoxBedName.TabIndex = 6;
            TextBoxBedName.PreviewKeyDown += TextBoxBedName_PreviewKeyDown;
            // 
            // LabelCompanyName
            // 
            LabelCompanyName.AutoSize = true;
            LabelCompanyName.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelCompanyName.Location = new Point(17, 13);
            LabelCompanyName.Name = "LabelCompanyName";
            LabelCompanyName.Size = new Size(57, 13);
            LabelCompanyName.TabIndex = 0;
            LabelCompanyName.Text = "Name/Id";
            // 
            // BtnWardDelete
            // 
            BtnWardDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnWardDelete.Location = new Point(108, 378);
            BtnWardDelete.Name = "BtnWardDelete";
            BtnWardDelete.Size = new Size(83, 23);
            BtnWardDelete.TabIndex = 3;
            BtnWardDelete.Text = "Delete [F4]";
            BtnWardDelete.UseVisualStyleBackColor = true;
            BtnWardDelete.Click += BtnWardDelete_Click;
            // 
            // TabControlBed
            // 
            TabControlBed.Controls.Add(TabBedTypeInfo);
            TabControlBed.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            TabControlBed.Location = new Point(257, 6);
            TabControlBed.Name = "TabControlBed";
            TabControlBed.SelectedIndex = 0;
            TabControlBed.ShowToolTips = true;
            TabControlBed.Size = new Size(536, 366);
            TabControlBed.TabIndex = 4;
            TabControlBed.TabStop = false;
            // 
            // TextBoxWardBedId
            // 
            TextBoxWardBedId.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxWardBedId.Location = new Point(328, 378);
            TextBoxWardBedId.Name = "TextBoxWardBedId";
            TextBoxWardBedId.Size = new Size(126, 21);
            TextBoxWardBedId.TabIndex = 45;
            TextBoxWardBedId.TabStop = false;
            TextBoxWardBedId.Visible = false;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { WardErrorMsg });
            statusStrip1.Location = new Point(0, 413);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(801, 22);
            statusStrip1.TabIndex = 38;
            statusStrip1.Text = "statusStrip1";
            // 
            // WardErrorMsg
            // 
            WardErrorMsg.Name = "WardErrorMsg";
            WardErrorMsg.Size = new Size(61, 17);
            WardErrorMsg.Text = "                  ";
            // 
            // TreeViewWardBed
            // 
            TreeViewWardBed.ContextMenuStrip = ContextMenuStripWardBed;
            TreeViewWardBed.HideSelection = false;
            TreeViewWardBed.ImageIndex = 0;
            TreeViewWardBed.ImageList = ImageListWardBed;
            TreeViewWardBed.Location = new Point(11, 33);
            TreeViewWardBed.Name = "TreeViewWardBed";
            TreeViewWardBed.SelectedImageIndex = 0;
            TreeViewWardBed.Size = new Size(239, 339);
            TreeViewWardBed.TabIndex = 1;
            TreeViewWardBed.AfterSelect += TreeViewWard_AfterSelect;
            // 
            // ContextMenuStripWardBed
            // 
            ContextMenuStripWardBed.AccessibleRole = AccessibleRole.DropList;
            ContextMenuStripWardBed.Items.AddRange(new ToolStripItem[] { newWardToolStripMenuItem, newBedToolStripMenuItem });
            ContextMenuStripWardBed.Name = "contextMenuStrip1";
            ContextMenuStripWardBed.Size = new Size(130, 48);
            // 
            // newWardToolStripMenuItem
            // 
            newWardToolStripMenuItem.Image = (Image)resources.GetObject("newWardToolStripMenuItem.Image");
            newWardToolStripMenuItem.Name = "newWardToolStripMenuItem";
            newWardToolStripMenuItem.Size = new Size(129, 22);
            newWardToolStripMenuItem.Text = "New Ward";
            newWardToolStripMenuItem.Click += newWardToolStripMenuItem_Click;
            // 
            // newBedToolStripMenuItem
            // 
            newBedToolStripMenuItem.Image = (Image)resources.GetObject("newBedToolStripMenuItem.Image");
            newBedToolStripMenuItem.Name = "newBedToolStripMenuItem";
            newBedToolStripMenuItem.Size = new Size(129, 22);
            newBedToolStripMenuItem.Text = "New Bed";
            newBedToolStripMenuItem.Click += newBedToolStripMenuItem_Click;
            // 
            // ImageListWardBed
            // 
            ImageListWardBed.ColorDepth = ColorDepth.Depth8Bit;
            ImageListWardBed.ImageStream = (ImageListStreamer)resources.GetObject("ImageListWardBed.ImageStream");
            ImageListWardBed.TransparentColor = Color.Transparent;
            ImageListWardBed.Images.SetKeyName(0, "ward2.png");
            ImageListWardBed.Images.SetKeyName(1, "bed1.png");
            // 
            // TabControlWard
            // 
            TabControlWard.Controls.Add(tabPage1);
            TabControlWard.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            TabControlWard.Location = new Point(258, 6);
            TabControlWard.Name = "TabControlWard";
            TabControlWard.SelectedIndex = 0;
            TabControlWard.ShowToolTips = true;
            TabControlWard.Size = new Size(536, 366);
            TabControlWard.TabIndex = 5;
            TabControlWard.TabStop = false;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = SystemColors.Window;
            tabPage1.Controls.Add(InventoryLocationcheckBox);
            tabPage1.Controls.Add(linkLabelWardBedType);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(GridViewBedInfo);
            tabPage1.Controls.Add(TextBoxWardName);
            tabPage1.Controls.Add(label5);
            tabPage1.Location = new Point(4, 22);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(528, 340);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Ward Info";
            // 
            // InventoryLocationcheckBox
            // 
            InventoryLocationcheckBox.AutoSize = true;
            InventoryLocationcheckBox.Checked = true;
            InventoryLocationcheckBox.CheckState = CheckState.Checked;
            InventoryLocationcheckBox.Location = new Point(19, 283);
            InventoryLocationcheckBox.Margin = new Padding(2);
            InventoryLocationcheckBox.Name = "InventoryLocationcheckBox";
            InventoryLocationcheckBox.Size = new Size(222, 17);
            InventoryLocationcheckBox.TabIndex = 36;
            InventoryLocationcheckBox.Text = "Maintain Inventory at this location";
            InventoryLocationcheckBox.UseVisualStyleBackColor = true;
            InventoryLocationcheckBox.Click += InventoryLocationcheckBox_Click;
            InventoryLocationcheckBox.PreviewKeyDown += InventoryLocationcheckBox_PreviewKeyDown;
            // 
            // linkLabelWardBedType
            // 
            linkLabelWardBedType.AutoSize = true;
            linkLabelWardBedType.LinkArea = new LinkArea(0, 19);
            linkLabelWardBedType.LinkColor = Color.Black;
            linkLabelWardBedType.Location = new Point(370, 63);
            linkLabelWardBedType.Name = "linkLabelWardBedType";
            linkLabelWardBedType.Size = new Size(119, 18);
            linkLabelWardBedType.TabIndex = 35;
            linkLabelWardBedType.TabStop = true;
            linkLabelWardBedType.Text = "Add A New Bed type";
            linkLabelWardBedType.UseCompatibleTextRendering = true;
            linkLabelWardBedType.LinkClicked += linkLabelWardBedType_LinkClicked;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 63);
            label1.Name = "label1";
            label1.Size = new Size(34, 13);
            label1.TabIndex = 34;
            label1.Text = "Beds";
            // 
            // GridViewBedInfo
            // 
            GridViewBedInfo.AllowUserToAddRows = false;
            GridViewBedInfo.AllowUserToResizeColumns = false;
            GridViewBedInfo.AllowUserToResizeRows = false;
            GridViewBedInfo.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewBedInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewBedInfo.ColumnHeadersHeight = 20;
            GridViewBedInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewBedInfo.Columns.AddRange(new DataGridViewColumn[] { BedNameOrId, Type, Column1, Column2 });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            GridViewBedInfo.DefaultCellStyle = dataGridViewCellStyle2;
            GridViewBedInfo.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewBedInfo.EnableHeadersVisualStyles = false;
            GridViewBedInfo.Location = new Point(19, 81);
            GridViewBedInfo.MultiSelect = false;
            GridViewBedInfo.Name = "GridViewBedInfo";
            GridViewBedInfo.RowHeadersVisible = false;
            GridViewBedInfo.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.White;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            GridViewBedInfo.RowsDefaultCellStyle = dataGridViewCellStyle3;
            GridViewBedInfo.RowTemplate.Height = 20;
            GridViewBedInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewBedInfo.ShowCellErrors = false;
            GridViewBedInfo.ShowCellToolTips = false;
            GridViewBedInfo.Size = new Size(495, 195);
            GridViewBedInfo.TabIndex = 6;
            GridViewBedInfo.CellContentClick += GridViewBedInfo_CellContentClick;
            GridViewBedInfo.CellEnter += GridViewBedInfo_CellEnter;
            GridViewBedInfo.CellLeave += GridViewBedInfo_CellLeave;
            GridViewBedInfo.DataError += GridViewBedInfo_DataError;
            GridViewBedInfo.EditingControlShowing += GridViewBedInfo_EditingControlShowing;
            GridViewBedInfo.RowsAdded += GridViewBedInfo_RowsAdded;
            // 
            // BedNameOrId
            // 
            BedNameOrId.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            BedNameOrId.HeaderText = "Name/Id";
            BedNameOrId.Name = "BedNameOrId";
            BedNameOrId.NameLength = 10;
            BedNameOrId.Resizable = DataGridViewTriState.False;
            // 
            // Type
            // 
            Type.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Type.FlatStyle = FlatStyle.Flat;
            Type.HeaderText = "Type";
            Type.Name = "Type";
            Type.Resizable = DataGridViewTriState.False;
            // 
            // Column1
            // 
            Column1.HeaderText = "";
            Column1.Name = "Column1";
            Column1.Resizable = DataGridViewTriState.False;
            Column1.Width = 25;
            // 
            // Column2
            // 
            Column2.HeaderText = "BedId";
            Column2.Name = "Column2";
            Column2.Visible = false;
            // 
            // TextBoxWardName
            // 
            TextBoxWardName.BackColor = Color.White;
            TextBoxWardName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxWardName.Location = new Point(19, 39);
            TextBoxWardName.MaxLength = 30;
            TextBoxWardName.Name = "TextBoxWardName";
            TextBoxWardName.ReadOnly = true;
            TextBoxWardName.Size = new Size(401, 21);
            TextBoxWardName.TabIndex = 5;
            TextBoxWardName.PreviewKeyDown += TextBoxWardName_PreviewKeyDown;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label5.Location = new Point(16, 21);
            label5.Name = "label5";
            label5.Size = new Size(39, 13);
            label5.TabIndex = 0;
            label5.Text = "Name";
            // 
            // BtnWardSave
            // 
            BtnWardSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnWardSave.Location = new Point(608, 378);
            BtnWardSave.Name = "BtnWardSave";
            BtnWardSave.Size = new Size(83, 23);
            BtnWardSave.TabIndex = 7;
            BtnWardSave.Text = "Save [F8]";
            BtnWardSave.UseVisualStyleBackColor = true;
            BtnWardSave.Click += BtnWardSave_Click;
            BtnWardSave.PreviewKeyDown += BtnWardSave_PreviewKeyDown;
            // 
            // BtnWardCancel
            // 
            BtnWardCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnWardCancel.Location = new Point(519, 378);
            BtnWardCancel.Name = "BtnWardCancel";
            BtnWardCancel.Size = new Size(83, 23);
            BtnWardCancel.TabIndex = 8;
            BtnWardCancel.Text = "Cancel [Esc]";
            BtnWardCancel.UseVisualStyleBackColor = true;
            BtnWardCancel.Click += BtnWardCancel_Click;
            // 
            // BtnWardEdit
            // 
            BtnWardEdit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnWardEdit.Location = new Point(197, 378);
            BtnWardEdit.Name = "BtnWardEdit";
            BtnWardEdit.Size = new Size(75, 23);
            BtnWardEdit.TabIndex = 4;
            BtnWardEdit.Text = "Edit [F7]";
            BtnWardEdit.UseVisualStyleBackColor = true;
            BtnWardEdit.Click += BtnWardEdit_Click;
            // 
            // TextBoxWardSearch
            // 
            TextBoxWardSearch.BackColor = SystemColors.Window;
            TextBoxWardSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxWardSearch.Delay = true;
            TextBoxWardSearch.DelayTime = 1000;
            TextBoxWardSearch.Location = new Point(11, 6);
            TextBoxWardSearch.MaxLength = 35;
            TextBoxWardSearch.Name = "TextBoxWardSearch";
            TextBoxWardSearch.Searchstartfrom = 2;
            TextBoxWardSearch.Size = new Size(239, 21);
            TextBoxWardSearch.TabIndex = 46;
            TextBoxWardSearch.TextChanged += TextBoxWardSearch_TextChanged;
            TextBoxWardSearch.KeyDown += TextBoxWardSearch_KeyDown;
            // 
            // BtnWardBedNew
            // 
            BtnWardBedNew.ButtonText = "New [F3]";
            BtnWardBedNew.ContextMenuStrip = ContextMenuStripWardBed;
            BtnWardBedNew.ImageList = ImageListWardBed;
            BtnWardBedNew.Items = (System.Collections.ObjectModel.Collection<string>)resources.GetObject("BtnWardBedNew.Items");
            BtnWardBedNew.Location = new Point(15, 377);
            BtnWardBedNew.Margin = new Padding(4, 3, 4, 3);
            BtnWardBedNew.Name = "BtnWardBedNew";
            BtnWardBedNew.Size = new Size(102, 29);
            BtnWardBedNew.TabIndex = 2;
            BtnWardBedNew.ItemClickedEvent += BtnWardBedNew_ItemClickedEvent;
            // 
            // FormWardAndBed
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(801, 435);
            Controls.Add(BtnWardDelete);
            Controls.Add(BtnWardBedNew);
            Controls.Add(TextBoxWardSearch);
            Controls.Add(BtnWardExit);
            Controls.Add(BtnWardCancel);
            Controls.Add(BtnWardSave);
            Controls.Add(TreeViewWardBed);
            Controls.Add(BtnWardEdit);
            Controls.Add(TextBoxWardBedId);
            Controls.Add(statusStrip1);
            Controls.Add(TabControlWard);
            Controls.Add(TabControlBed);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormWardAndBed";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Ward and Bed";
            FormClosing += FormWardAndBed_FormClosing;
            Load += FormWardAndBed_Load;
            Controls.SetChildIndex(TabControlBed, 0);
            Controls.SetChildIndex(TabControlWard, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(TextBoxWardBedId, 0);
            Controls.SetChildIndex(BtnWardEdit, 0);
            Controls.SetChildIndex(TreeViewWardBed, 0);
            Controls.SetChildIndex(BtnWardSave, 0);
            Controls.SetChildIndex(BtnWardCancel, 0);
            Controls.SetChildIndex(BtnWardExit, 0);
            Controls.SetChildIndex(TextBoxWardSearch, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(BtnWardBedNew, 0);
            Controls.SetChildIndex(BtnWardDelete, 0);
            TabBedTypeInfo.ResumeLayout(false);
            TabBedTypeInfo.PerformLayout();
            TabControlBed.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ContextMenuStripWardBed.ResumeLayout(false);
            TabControlWard.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewBedInfo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnWardExit;
        private TabPage TabBedTypeInfo;
        private TextBox TextBoxBedName;
        private Label LabelCompanyName;
        private Button BtnWardDelete;
        private TabControl TabControlBed;
        private TextBox TextBoxWardBedId;
        private StatusStrip statusStrip1;
        private Label label2;
        private controls.ComboBoxSwapTextBox ComboBoxSwapTextBoxBedType;
        private Label label3;
        private controls.ComboBoxSwapTextBox ComboBoxSwapTextBoxWard;
        private TreeView TreeViewWardBed;
        private TabControl TabControlWard;
        private TabPage tabPage1;
        private Button BtnWardSave;
        private Button BtnWardCancel;
        private Button BtnWardEdit;
        private TextBox TextBoxWardName;
        private Label label5;
        private Label label1;
        private controls.DataViewVerticalScroll GridViewBedInfo;
        private LinkLabel linkLabelWardBedType;
        private ToolStripStatusLabel WardErrorMsg;
        private ContextMenuStrip ContextMenuStripWardBed;
        private ToolStripMenuItem newWardToolStripMenuItem;
        private ToolStripMenuItem newBedToolStripMenuItem;
        private ImageList ImageListWardBed;
        private CheckBox InventoryLocationcheckBox;
        private controls.text.DelayedTextChangeTextBox TextBoxWardSearch;
        private Dropdown_Button.UserControlButtonWithMenu BtnWardBedNew;
        private controls.grid.DataGridViewNameColumn BedNameOrId;
        private DataGridViewComboBoxColumn Type;
        private DataGridViewButtonColumn Column1;
        private DataGridViewTextBoxColumn Column2;
    }
}