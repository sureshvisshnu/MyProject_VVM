namespace fa.views.hms.Masters
{
    partial class FormSymptom
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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSymptom));
            statusStrip1 = new StatusStrip();
            SymptomErrorMsg = new ToolStripStatusLabel();
            BtnSymptomExit = new Button();
            BtnSymptomDelete = new Button();
            BtnSymptomSave = new Button();
            BtnSymptomCancel = new Button();
            TextBoxSymptomId = new TextBox();
            BtnSymptomEdit = new Button();
            TabControlSymptoms = new TabControl();
            TabPageSymptomsDetails = new TabPage();
            ComboBoxSymptomCategory = new controls.ComboBoxSwapTextBox();
            TextBoxMedicalSymptomCode = new controls.text.NameTextBoxAllowSpace(components);
            label1 = new Label();
            TextBoxSymptomDisplayas = new controls.text.NameTextBoxAllowSpace(components);
            label4 = new Label();
            TextBoxSymptomName = new controls.text.NameTextBoxAllowSpace(components);
            TextBoxSymptomReasonInactive = new TextBox();
            CheckBoxSymptomIsActive = new CheckBox();
            labelReasonforInactive = new Label();
            GridViewKeywords = new controls.DataViewVerticalScroll();
            SNO = new DataGridViewTextBoxColumn();
            Keywords = new controls.grid.DataGridViewNameColumn();
            DeleteKWords = new DataGridViewButtonColumn();
            id = new DataGridViewTextBoxColumn();
            LabelCostCenterDisplayAs = new Label();
            TextBoxSymptomDescription = new TextBox();
            LabelCostCenterDescription = new Label();
            LabelCostCenterName = new Label();
            TreeViewSymptom = new TreeView();
            contextMenuStripCategory = new ContextMenuStrip(components);
            newCategoryToolStripMenuItem = new ToolStripMenuItem();
            newSymptomToolStripMenuItem = new ToolStripMenuItem();
            ImageListSymptoms = new ImageList(components);
            BtnImport = new Button();
            BtnExport = new Button();
            TextBoxSymptomSearch = new controls.text.DelayedTextChangeTextBox();
            TabPageSymptomCategory = new TabPage();
            ComboBoxParentSymptomCategory = new controls.ComboBoxSwapTextBox();
            TextBoxSymptomCategoryDescription = new TextBox();
            LabelSymptomCategoryDescription = new Label();
            TextBoxSymptomCategoryDisplayAs = new TextBox();
            TextBoxSymptomCategoryName = new TextBox();
            label14 = new Label();
            label15 = new Label();
            LabelParentSymptomCategory = new Label();
            TabControlSymptomCategory = new TabControl();
            contextMenuStripSymptoms = new ContextMenuStrip(components);
            newSymptomToolStripMenuItem1 = new ToolStripMenuItem();
            BtnSymptomNew = new Dropdown_Button.UserControlButtonWithMenu();
            TextBoxSymptomCategoryId = new TextBox();
            statusStrip1.SuspendLayout();
            TabControlSymptoms.SuspendLayout();
            TabPageSymptomsDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewKeywords).BeginInit();
            contextMenuStripCategory.SuspendLayout();
            TabPageSymptomCategory.SuspendLayout();
            TabControlSymptomCategory.SuspendLayout();
            contextMenuStripSymptoms.SuspendLayout();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(172, 215);
            ProductIdTransport.Size = new Size(100, 24);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(172, 189);
            ProductBatchIdTransport.Size = new Size(100, 24);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(172, 163);
            AccountIdTransport.Size = new Size(100, 24);
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { SymptomErrorMsg });
            statusStrip1.Location = new Point(0, 468);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(998, 26);
            statusStrip1.TabIndex = 36;
            statusStrip1.Text = "statusStrip1";
            // 
            // SymptomErrorMsg
            // 
            SymptomErrorMsg.Name = "SymptomErrorMsg";
            SymptomErrorMsg.Size = new Size(65, 20);
            SymptomErrorMsg.Text = "              ";
            // 
            // BtnSymptomExit
            // 
            BtnSymptomExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSymptomExit.Location = new Point(901, 429);
            BtnSymptomExit.Name = "BtnSymptomExit";
            BtnSymptomExit.Size = new Size(83, 23);
            BtnSymptomExit.TabIndex = 16;
            BtnSymptomExit.Text = "Exit [F10]";
            BtnSymptomExit.UseVisualStyleBackColor = true;
            BtnSymptomExit.Click += BtnSymptomExit_Click;
            // 
            // BtnSymptomDelete
            // 
            BtnSymptomDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSymptomDelete.Location = new Point(112, 429);
            BtnSymptomDelete.Name = "BtnSymptomDelete";
            BtnSymptomDelete.Size = new Size(83, 23);
            BtnSymptomDelete.TabIndex = 3;
            BtnSymptomDelete.Text = "Delete [F4]";
            BtnSymptomDelete.UseVisualStyleBackColor = true;
            BtnSymptomDelete.Click += BtnSymptomDelete_Click;
            // 
            // BtnSymptomSave
            // 
            BtnSymptomSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSymptomSave.Location = new Point(812, 429);
            BtnSymptomSave.Name = "BtnSymptomSave";
            BtnSymptomSave.Size = new Size(83, 23);
            BtnSymptomSave.TabIndex = 12;
            BtnSymptomSave.Text = "Save [F8]";
            BtnSymptomSave.UseVisualStyleBackColor = true;
            BtnSymptomSave.Click += BtnSymptomSave_Click;
            BtnSymptomSave.PreviewKeyDown += BtnSymptomSave_PreviewKeyDown;
            // 
            // BtnSymptomCancel
            // 
            BtnSymptomCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSymptomCancel.Location = new Point(723, 429);
            BtnSymptomCancel.Name = "BtnSymptomCancel";
            BtnSymptomCancel.Size = new Size(83, 23);
            BtnSymptomCancel.TabIndex = 13;
            BtnSymptomCancel.Text = "Cancel [Esc]";
            BtnSymptomCancel.UseVisualStyleBackColor = true;
            BtnSymptomCancel.Click += BtnSymptomCancel_Click;
            // 
            // TextBoxSymptomId
            // 
            TextBoxSymptomId.Location = new Point(451, 430);
            TextBoxSymptomId.Name = "TextBoxSymptomId";
            TextBoxSymptomId.Size = new Size(100, 24);
            TextBoxSymptomId.TabIndex = 35;
            TextBoxSymptomId.Visible = false;
            // 
            // BtnSymptomEdit
            // 
            BtnSymptomEdit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSymptomEdit.Location = new Point(201, 429);
            BtnSymptomEdit.Name = "BtnSymptomEdit";
            BtnSymptomEdit.Size = new Size(83, 23);
            BtnSymptomEdit.TabIndex = 4;
            BtnSymptomEdit.Text = "Edit [F7]";
            BtnSymptomEdit.UseVisualStyleBackColor = true;
            BtnSymptomEdit.Click += BtnSymptomEdit_Click;
            // 
            // TabControlSymptoms
            // 
            TabControlSymptoms.Controls.Add(TabPageSymptomsDetails);
            TabControlSymptoms.Location = new Point(262, 15);
            TabControlSymptoms.Name = "TabControlSymptoms";
            TabControlSymptoms.SelectedIndex = 0;
            TabControlSymptoms.Size = new Size(720, 408);
            TabControlSymptoms.TabIndex = 3;
            // 
            // TabPageSymptomsDetails
            // 
            TabPageSymptomsDetails.Controls.Add(ComboBoxSymptomCategory);
            TabPageSymptomsDetails.Controls.Add(TextBoxMedicalSymptomCode);
            TabPageSymptomsDetails.Controls.Add(label1);
            TabPageSymptomsDetails.Controls.Add(TextBoxSymptomDisplayas);
            TabPageSymptomsDetails.Controls.Add(label4);
            TabPageSymptomsDetails.Controls.Add(TextBoxSymptomName);
            TabPageSymptomsDetails.Controls.Add(TextBoxSymptomReasonInactive);
            TabPageSymptomsDetails.Controls.Add(CheckBoxSymptomIsActive);
            TabPageSymptomsDetails.Controls.Add(labelReasonforInactive);
            TabPageSymptomsDetails.Controls.Add(GridViewKeywords);
            TabPageSymptomsDetails.Controls.Add(LabelCostCenterDisplayAs);
            TabPageSymptomsDetails.Controls.Add(TextBoxSymptomDescription);
            TabPageSymptomsDetails.Controls.Add(LabelCostCenterDescription);
            TabPageSymptomsDetails.Controls.Add(LabelCostCenterName);
            TabPageSymptomsDetails.Location = new Point(4, 26);
            TabPageSymptomsDetails.Name = "TabPageSymptomsDetails";
            TabPageSymptomsDetails.Padding = new Padding(3);
            TabPageSymptomsDetails.Size = new Size(712, 378);
            TabPageSymptomsDetails.TabIndex = 0;
            TabPageSymptomsDetails.Text = "Diagnosis Details";
            TabPageSymptomsDetails.UseVisualStyleBackColor = true;
            // 
            // ComboBoxSymptomCategory
            // 
            ComboBoxSymptomCategory.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxSymptomCategory.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxSymptomCategory.FormattingEnabled = true;
            ComboBoxSymptomCategory.Location = new Point(18, 151);
            ComboBoxSymptomCategory.Name = "ComboBoxSymptomCategory";
            ComboBoxSymptomCategory.Size = new Size(196, 25);
            ComboBoxSymptomCategory.TabIndex = 7;
            ComboBoxSymptomCategory.TxtVisible = true;
            ComboBoxSymptomCategory.KeyPress += comboBoxSwapTextBoxSymptomCategory_KeyPress;
            // 
            // TextBoxMedicalSymptomCode
            // 
            TextBoxMedicalSymptomCode.BackColor = Color.White;
            TextBoxMedicalSymptomCode.Location = new Point(18, 71);
            TextBoxMedicalSymptomCode.MaxLength = 50;
            TextBoxMedicalSymptomCode.Name = "TextBoxMedicalSymptomCode";
            TextBoxMedicalSymptomCode.Size = new Size(445, 24);
            TextBoxMedicalSymptomCode.TabIndex = 5;
            TextBoxMedicalSymptomCode.PreviewKeyDown += TextBoxMedicalSymptomCode_PreviewKeyDown;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(15, 53);
            label1.Name = "label1";
            label1.Size = new Size(113, 17);
            label1.TabIndex = 58;
            label1.Text = "Diagnosis Code";
            // 
            // TextBoxSymptomDisplayas
            // 
            TextBoxSymptomDisplayas.BackColor = Color.White;
            TextBoxSymptomDisplayas.Location = new Point(18, 112);
            TextBoxSymptomDisplayas.MaxLength = 50;
            TextBoxSymptomDisplayas.Name = "TextBoxSymptomDisplayas";
            TextBoxSymptomDisplayas.Size = new Size(445, 24);
            TextBoxSymptomDisplayas.TabIndex = 6;
            TextBoxSymptomDisplayas.KeyPress += TextBoxSymptomName_KeyPress;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(14, 135);
            label4.Name = "label4";
            label4.Size = new Size(72, 17);
            label4.TabIndex = 56;
            label4.Text = "Category";
            // 
            // TextBoxSymptomName
            // 
            TextBoxSymptomName.BackColor = Color.White;
            TextBoxSymptomName.Location = new Point(18, 32);
            TextBoxSymptomName.MaxLength = 50;
            TextBoxSymptomName.Name = "TextBoxSymptomName";
            TextBoxSymptomName.Size = new Size(445, 24);
            TextBoxSymptomName.TabIndex = 4;
            TextBoxSymptomName.KeyPress += TextBoxSymptomName_KeyPress;
            TextBoxSymptomName.PreviewKeyDown += TextBoxSymptomName_PreviewKeyDown;
            // 
            // TextBoxSymptomReasonInactive
            // 
            TextBoxSymptomReasonInactive.BackColor = Color.White;
            TextBoxSymptomReasonInactive.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxSymptomReasonInactive.Location = new Point(18, 304);
            TextBoxSymptomReasonInactive.MaxLength = 512;
            TextBoxSymptomReasonInactive.Multiline = true;
            TextBoxSymptomReasonInactive.Name = "TextBoxSymptomReasonInactive";
            TextBoxSymptomReasonInactive.Size = new Size(445, 59);
            TextBoxSymptomReasonInactive.TabIndex = 10;
            // 
            // CheckBoxSymptomIsActive
            // 
            CheckBoxSymptomIsActive.AutoSize = true;
            CheckBoxSymptomIsActive.Location = new Point(18, 261);
            CheckBoxSymptomIsActive.Name = "CheckBoxSymptomIsActive";
            CheckBoxSymptomIsActive.Size = new Size(88, 21);
            CheckBoxSymptomIsActive.TabIndex = 9;
            CheckBoxSymptomIsActive.Text = "Is Active?";
            CheckBoxSymptomIsActive.UseVisualStyleBackColor = true;
            CheckBoxSymptomIsActive.CheckedChanged += CheckBoxSymptomIsActive_CheckedChanged;
            CheckBoxSymptomIsActive.PreviewKeyDown += CheckBoxSymptomIsActive_PreviewKeyDown;
            // 
            // labelReasonforInactive
            // 
            labelReasonforInactive.AutoSize = true;
            labelReasonforInactive.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            labelReasonforInactive.Location = new Point(15, 285);
            labelReasonforInactive.Name = "labelReasonforInactive";
            labelReasonforInactive.Size = new Size(126, 17);
            labelReasonforInactive.TabIndex = 19;
            labelReasonforInactive.Text = "Reason for Inactive";
            // 
            // GridViewKeywords
            // 
            GridViewKeywords.AllowUserToResizeColumns = false;
            GridViewKeywords.AllowUserToResizeRows = false;
            GridViewKeywords.BackgroundColor = Color.White;
            GridViewKeywords.ColumnHeadersHeight = 20;
            GridViewKeywords.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewKeywords.Columns.AddRange(new DataGridViewColumn[] { SNO, Keywords, DeleteKWords, id });
            GridViewKeywords.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewKeywords.EnableHeadersVisualStyles = false;
            GridViewKeywords.Location = new Point(477, 33);
            GridViewKeywords.Name = "GridViewKeywords";
            GridViewKeywords.RowHeadersVisible = false;
            GridViewKeywords.RowHeadersWidth = 51;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            GridViewKeywords.RowsDefaultCellStyle = dataGridViewCellStyle2;
            GridViewKeywords.ShowCellToolTips = false;
            GridViewKeywords.Size = new Size(203, 330);
            GridViewKeywords.TabIndex = 11;
            GridViewKeywords.CellClick += GridViewKeywords_CellClick;
            GridViewKeywords.DataError += GridViewKeywords_DataError;
            GridViewKeywords.RowsAdded += GridViewKeywords_RowsAdded;
            GridViewKeywords.Enter += GridViewKeywords_Enter;
            // 
            // SNO
            // 
            SNO.HeaderText = "#";
            SNO.MinimumWidth = 6;
            SNO.Name = "SNO";
            SNO.ReadOnly = true;
            SNO.SortMode = DataGridViewColumnSortMode.NotSortable;
            SNO.Width = 25;
            // 
            // Keywords
            // 
            Keywords.HeaderText = "Keywords";
            Keywords.MinimumWidth = 6;
            Keywords.Name = "Keywords";
            Keywords.Resizable = DataGridViewTriState.False;
            Keywords.Width = 150;
            // 
            // DeleteKWords
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "X";
            DeleteKWords.DefaultCellStyle = dataGridViewCellStyle1;
            DeleteKWords.HeaderText = "...";
            DeleteKWords.MinimumWidth = 6;
            DeleteKWords.Name = "DeleteKWords";
            DeleteKWords.Resizable = DataGridViewTriState.False;
            DeleteKWords.Width = 25;
            // 
            // id
            // 
            id.HeaderText = "Id";
            id.MinimumWidth = 6;
            id.Name = "id";
            id.Visible = false;
            id.Width = 6;
            // 
            // LabelCostCenterDisplayAs
            // 
            LabelCostCenterDisplayAs.AutoSize = true;
            LabelCostCenterDisplayAs.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelCostCenterDisplayAs.Location = new Point(15, 95);
            LabelCostCenterDisplayAs.Name = "LabelCostCenterDisplayAs";
            LabelCostCenterDisplayAs.Size = new Size(69, 17);
            LabelCostCenterDisplayAs.TabIndex = 14;
            LabelCostCenterDisplayAs.Text = "Display As";
            // 
            // TextBoxSymptomDescription
            // 
            TextBoxSymptomDescription.BackColor = Color.White;
            TextBoxSymptomDescription.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxSymptomDescription.Location = new Point(18, 198);
            TextBoxSymptomDescription.MaxLength = 512;
            TextBoxSymptomDescription.Multiline = true;
            TextBoxSymptomDescription.Name = "TextBoxSymptomDescription";
            TextBoxSymptomDescription.Size = new Size(445, 59);
            TextBoxSymptomDescription.TabIndex = 8;
            // 
            // LabelCostCenterDescription
            // 
            LabelCostCenterDescription.AutoSize = true;
            LabelCostCenterDescription.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelCostCenterDescription.Location = new Point(15, 181);
            LabelCostCenterDescription.Name = "LabelCostCenterDescription";
            LabelCostCenterDescription.Size = new Size(76, 17);
            LabelCostCenterDescription.TabIndex = 12;
            LabelCostCenterDescription.Text = "Description";
            // 
            // LabelCostCenterName
            // 
            LabelCostCenterName.AutoSize = true;
            LabelCostCenterName.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelCostCenterName.Location = new Point(15, 16);
            LabelCostCenterName.Name = "LabelCostCenterName";
            LabelCostCenterName.Size = new Size(48, 17);
            LabelCostCenterName.TabIndex = 11;
            LabelCostCenterName.Text = "Name";
            // 
            // TreeViewSymptom
            // 
            TreeViewSymptom.ContextMenuStrip = contextMenuStripCategory;
            TreeViewSymptom.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TreeViewSymptom.HideSelection = false;
            TreeViewSymptom.ImageIndex = 0;
            TreeViewSymptom.ImageList = ImageListSymptoms;
            TreeViewSymptom.Location = new Point(17, 42);
            TreeViewSymptom.Name = "TreeViewSymptom";
            TreeViewSymptom.SelectedImageIndex = 0;
            TreeViewSymptom.Size = new Size(239, 377);
            TreeViewSymptom.TabIndex = 1;
            TreeViewSymptom.AfterSelect += TreeViewSymptom_AfterSelect;
            TreeViewSymptom.NodeMouseClick += TreeViewSymptom_NodeMouseClick;
            TreeViewSymptom.Enter += TreeViewSymptom_Enter;
            // 
            // contextMenuStripCategory
            // 
            contextMenuStripCategory.ImageScalingSize = new Size(20, 20);
            contextMenuStripCategory.Items.AddRange(new ToolStripItem[] { newCategoryToolStripMenuItem, newSymptomToolStripMenuItem });
            contextMenuStripCategory.Name = "contextMenuStripCategory";
            contextMenuStripCategory.Size = new Size(215, 84);
            // 
            // newCategoryToolStripMenuItem
            // 
            newCategoryToolStripMenuItem.Image = (Image)resources.GetObject("newCategoryToolStripMenuItem.Image");
            newCategoryToolStripMenuItem.Name = "newCategoryToolStripMenuItem";
            newCategoryToolStripMenuItem.Size = new Size(214, 26);
            newCategoryToolStripMenuItem.Text = "New Category";
            newCategoryToolStripMenuItem.Click += newCategoryToolStripMenuItem_Click;
            // 
            // newSymptomToolStripMenuItem
            // 
            newSymptomToolStripMenuItem.Image = (Image)resources.GetObject("newSymptomToolStripMenuItem.Image");
            newSymptomToolStripMenuItem.Name = "newSymptomToolStripMenuItem";
            newSymptomToolStripMenuItem.Size = new Size(214, 26);
            newSymptomToolStripMenuItem.Text = "New Diagnosis";
            newSymptomToolStripMenuItem.Click += newSymptomToolStripMenuItem_Click;
            // 
            // ImageListSymptoms
            // 
            ImageListSymptoms.ColorDepth = ColorDepth.Depth8Bit;
            ImageListSymptoms.ImageStream = (ImageListStreamer)resources.GetObject("ImageListSymptoms.ImageStream");
            ImageListSymptoms.TransparentColor = Color.Transparent;
            ImageListSymptoms.Images.SetKeyName(0, "Category.png");
            ImageListSymptoms.Images.SetKeyName(1, "Symptoms.png");
            // 
            // BtnImport
            // 
            BtnImport.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnImport.Location = new Point(545, 429);
            BtnImport.Name = "BtnImport";
            BtnImport.Size = new Size(83, 23);
            BtnImport.TabIndex = 15;
            BtnImport.Text = "Import";
            BtnImport.UseVisualStyleBackColor = true;
            BtnImport.Click += BtnImport_Click;
            // 
            // BtnExport
            // 
            BtnExport.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExport.Location = new Point(634, 429);
            BtnExport.Name = "BtnExport";
            BtnExport.Size = new Size(83, 23);
            BtnExport.TabIndex = 14;
            BtnExport.Text = "Export";
            BtnExport.UseVisualStyleBackColor = true;
            BtnExport.Click += BtnExport_Click;
            // 
            // TextBoxSymptomSearch
            // 
            TextBoxSymptomSearch.BackColor = SystemColors.Window;
            TextBoxSymptomSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxSymptomSearch.Delay = true;
            TextBoxSymptomSearch.DelayTime = 1000;
            TextBoxSymptomSearch.Location = new Point(17, 15);
            TextBoxSymptomSearch.MaxLength = 35;
            TextBoxSymptomSearch.Name = "TextBoxSymptomSearch";
            TextBoxSymptomSearch.Searchstartfrom = 2;
            TextBoxSymptomSearch.Size = new Size(239, 24);
            TextBoxSymptomSearch.TabIndex = 0;
            TextBoxSymptomSearch.TextChanged += TextBoxSymptomSearch_TextChanged;
            TextBoxSymptomSearch.KeyDown += TextBoxSymptomSearch_KeyDown;
            // 
            // TabPageSymptomCategory
            // 
            TabPageSymptomCategory.Controls.Add(ComboBoxParentSymptomCategory);
            TabPageSymptomCategory.Controls.Add(TextBoxSymptomCategoryDescription);
            TabPageSymptomCategory.Controls.Add(LabelSymptomCategoryDescription);
            TabPageSymptomCategory.Controls.Add(TextBoxSymptomCategoryDisplayAs);
            TabPageSymptomCategory.Controls.Add(TextBoxSymptomCategoryName);
            TabPageSymptomCategory.Controls.Add(label14);
            TabPageSymptomCategory.Controls.Add(label15);
            TabPageSymptomCategory.Controls.Add(LabelParentSymptomCategory);
            TabPageSymptomCategory.Location = new Point(4, 26);
            TabPageSymptomCategory.Name = "TabPageSymptomCategory";
            TabPageSymptomCategory.Padding = new Padding(3);
            TabPageSymptomCategory.Size = new Size(712, 362);
            TabPageSymptomCategory.TabIndex = 0;
            TabPageSymptomCategory.Text = "Category";
            TabPageSymptomCategory.UseVisualStyleBackColor = true;
            // 
            // ComboBoxParentSymptomCategory
            // 
            ComboBoxParentSymptomCategory.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxParentSymptomCategory.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxParentSymptomCategory.FormattingEnabled = true;
            ComboBoxParentSymptomCategory.Location = new Point(22, 183);
            ComboBoxParentSymptomCategory.Name = "ComboBoxParentSymptomCategory";
            ComboBoxParentSymptomCategory.Size = new Size(300, 25);
            ComboBoxParentSymptomCategory.TabIndex = 54;
            ComboBoxParentSymptomCategory.TxtVisible = true;
            ComboBoxParentSymptomCategory.SelectedIndexChanged += comboBoxSwapTextBoxParentSymptomCategory_SelectedIndexChanged;
            // 
            // TextBoxSymptomCategoryDescription
            // 
            TextBoxSymptomCategoryDescription.BackColor = SystemColors.Window;
            TextBoxSymptomCategoryDescription.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxSymptomCategoryDescription.Location = new Point(22, 112);
            TextBoxSymptomCategoryDescription.MaxLength = 250;
            TextBoxSymptomCategoryDescription.Multiline = true;
            TextBoxSymptomCategoryDescription.Name = "TextBoxSymptomCategoryDescription";
            TextBoxSymptomCategoryDescription.ReadOnly = true;
            TextBoxSymptomCategoryDescription.Size = new Size(368, 49);
            TextBoxSymptomCategoryDescription.TabIndex = 53;
            // 
            // LabelSymptomCategoryDescription
            // 
            LabelSymptomCategoryDescription.AutoSize = true;
            LabelSymptomCategoryDescription.Location = new Point(19, 95);
            LabelSymptomCategoryDescription.Name = "LabelSymptomCategoryDescription";
            LabelSymptomCategoryDescription.Size = new Size(76, 17);
            LabelSymptomCategoryDescription.TabIndex = 58;
            LabelSymptomCategoryDescription.Text = "Description";
            // 
            // TextBoxSymptomCategoryDisplayAs
            // 
            TextBoxSymptomCategoryDisplayAs.BackColor = SystemColors.Window;
            TextBoxSymptomCategoryDisplayAs.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxSymptomCategoryDisplayAs.Location = new Point(22, 71);
            TextBoxSymptomCategoryDisplayAs.MaxLength = 50;
            TextBoxSymptomCategoryDisplayAs.Name = "TextBoxSymptomCategoryDisplayAs";
            TextBoxSymptomCategoryDisplayAs.ReadOnly = true;
            TextBoxSymptomCategoryDisplayAs.Size = new Size(368, 24);
            TextBoxSymptomCategoryDisplayAs.TabIndex = 52;
            // 
            // TextBoxSymptomCategoryName
            // 
            TextBoxSymptomCategoryName.BackColor = SystemColors.Window;
            TextBoxSymptomCategoryName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxSymptomCategoryName.Location = new Point(22, 31);
            TextBoxSymptomCategoryName.MaxLength = 30;
            TextBoxSymptomCategoryName.Name = "TextBoxSymptomCategoryName";
            TextBoxSymptomCategoryName.ReadOnly = true;
            TextBoxSymptomCategoryName.Size = new Size(368, 24);
            TextBoxSymptomCategoryName.TabIndex = 51;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label14.Location = new Point(19, 54);
            label14.Name = "label14";
            label14.Size = new Size(69, 17);
            label14.TabIndex = 56;
            label14.Text = "Display As";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label15.Location = new Point(19, 13);
            label15.Name = "label15";
            label15.Size = new Size(48, 17);
            label15.TabIndex = 55;
            label15.Text = "Name";
            // 
            // LabelParentSymptomCategory
            // 
            LabelParentSymptomCategory.AutoSize = true;
            LabelParentSymptomCategory.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelParentSymptomCategory.Location = new Point(19, 165);
            LabelParentSymptomCategory.Name = "LabelParentSymptomCategory";
            LabelParentSymptomCategory.Size = new Size(109, 17);
            LabelParentSymptomCategory.TabIndex = 57;
            LabelParentSymptomCategory.Text = "Parent Category";
            // 
            // TabControlSymptomCategory
            // 
            TabControlSymptomCategory.Controls.Add(TabPageSymptomCategory);
            TabControlSymptomCategory.Location = new Point(262, 15);
            TabControlSymptomCategory.Name = "TabControlSymptomCategory";
            TabControlSymptomCategory.SelectedIndex = 0;
            TabControlSymptomCategory.Size = new Size(720, 392);
            TabControlSymptomCategory.TabIndex = 37;
            // 
            // contextMenuStripSymptoms
            // 
            contextMenuStripSymptoms.ImageScalingSize = new Size(20, 20);
            contextMenuStripSymptoms.Items.AddRange(new ToolStripItem[] { newSymptomToolStripMenuItem1 });
            contextMenuStripSymptoms.Name = "contextMenuStripSymptoms";
            contextMenuStripSymptoms.Size = new Size(182, 30);
            // 
            // newSymptomToolStripMenuItem1
            // 
            newSymptomToolStripMenuItem1.Image = (Image)resources.GetObject("newSymptomToolStripMenuItem1.Image");
            newSymptomToolStripMenuItem1.Name = "newSymptomToolStripMenuItem1";
            newSymptomToolStripMenuItem1.Size = new Size(181, 26);
            newSymptomToolStripMenuItem1.Text = "New Diagnosis";
            // 
            // BtnSymptomNew
            // 
            BtnSymptomNew.ButtonText = "New [F3]";
            BtnSymptomNew.ContextMenuStrip = contextMenuStripCategory;
            BtnSymptomNew.ImageList = ImageListSymptoms;
            BtnSymptomNew.Items = (System.Collections.ObjectModel.Collection<string>)resources.GetObject("BtnSymptomNew.Items");
            BtnSymptomNew.Location = new Point(27, 429);
            BtnSymptomNew.Margin = new Padding(4, 3, 4, 3);
            BtnSymptomNew.Name = "BtnSymptomNew";
            BtnSymptomNew.Size = new Size(94, 28);
            BtnSymptomNew.TabIndex = 2;
            BtnSymptomNew.ItemClickedEvent += BtnSymptomsNew_ItemClickedEvent;
            // 
            // TextBoxSymptomCategoryId
            // 
            TextBoxSymptomCategoryId.Location = new Point(345, 430);
            TextBoxSymptomCategoryId.Name = "TextBoxSymptomCategoryId";
            TextBoxSymptomCategoryId.Size = new Size(100, 24);
            TextBoxSymptomCategoryId.TabIndex = 38;
            TextBoxSymptomCategoryId.Visible = false;
            // 
            // FormSymptom
            // 
            AutoScaleDimensions = new SizeF(8F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(998, 494);
            Controls.Add(BtnSymptomDelete);
            Controls.Add(TextBoxSymptomCategoryId);
            Controls.Add(BtnSymptomNew);
            Controls.Add(TextBoxSymptomSearch);
            Controls.Add(BtnImport);
            Controls.Add(BtnExport);
            Controls.Add(statusStrip1);
            Controls.Add(BtnSymptomExit);
            Controls.Add(BtnSymptomSave);
            Controls.Add(BtnSymptomCancel);
            Controls.Add(TextBoxSymptomId);
            Controls.Add(BtnSymptomEdit);
            Controls.Add(TreeViewSymptom);
            Controls.Add(TabControlSymptoms);
            Controls.Add(TabControlSymptomCategory);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSymptom";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Diagnosis";
            FormClosing += FormSymptom_FormClosing;
            Load += FormSymptom_Load;
            Controls.SetChildIndex(TabControlSymptomCategory, 0);
            Controls.SetChildIndex(TabControlSymptoms, 0);
            Controls.SetChildIndex(TreeViewSymptom, 0);
            Controls.SetChildIndex(BtnSymptomEdit, 0);
            Controls.SetChildIndex(TextBoxSymptomId, 0);
            Controls.SetChildIndex(BtnSymptomCancel, 0);
            Controls.SetChildIndex(BtnSymptomSave, 0);
            Controls.SetChildIndex(BtnSymptomExit, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(BtnExport, 0);
            Controls.SetChildIndex(BtnImport, 0);
            Controls.SetChildIndex(TextBoxSymptomSearch, 0);
            Controls.SetChildIndex(BtnSymptomNew, 0);
            Controls.SetChildIndex(TextBoxSymptomCategoryId, 0);
            Controls.SetChildIndex(BtnSymptomDelete, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            TabControlSymptoms.ResumeLayout(false);
            TabPageSymptomsDetails.ResumeLayout(false);
            TabPageSymptomsDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewKeywords).EndInit();
            contextMenuStripCategory.ResumeLayout(false);
            TabPageSymptomCategory.ResumeLayout(false);
            TabPageSymptomCategory.PerformLayout();
            TabControlSymptomCategory.ResumeLayout(false);
            contextMenuStripSymptoms.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TreeView TreeViewSymptom;
        private TabControl TabControlSymptoms;
        private TabPage TabPageSymptomsDetails;
        private TextBox TextBoxSymptomReasonInactive;
        private CheckBox CheckBoxSymptomIsActive;
        private Label labelReasonforInactive;
        private controls.DataViewVerticalScroll GridViewKeywords;
        private Label LabelCostCenterDisplayAs;
        private TextBox TextBoxSymptomDescription;
        private Label LabelCostCenterDescription;
        private Label LabelCostCenterName;
        private Button BtnSymptomExit;
        private Button BtnSymptomDelete;
        private Button BtnSymptomSave;
        private Button BtnSymptomCancel;
        private TextBox TextBoxSymptomId;
        private Button BtnSymptomEdit;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel SymptomErrorMsg;
        private controls.text.NameTextBoxAllowSpace TextBoxSymptomName;
        private controls.text.NameTextBoxAllowSpace TextBoxSymptomDisplayas;
        private DataGridViewTextBoxColumn SNO;
        private controls.grid.DataGridViewNameColumn Keywords;
        private DataGridViewButtonColumn DeleteKWords;
        private DataGridViewTextBoxColumn id;
        private Button BtnImport;
        private Button BtnExport;
        private controls.text.DelayedTextChangeTextBox TextBoxSymptomSearch;
        private controls.ComboBoxSwapTextBox ComboBoxSymptomCategory;
        private Label label4;
        private TabPage TabPageSymptomCategory;
        private TabControl TabControlSymptomCategory;
        private controls.ComboBoxSwapTextBox ComboBoxParentSymptomCategory;
        private TextBox TextBoxSymptomCategoryDescription;
        private Label LabelSymptomCategoryDescription;
        private TextBox TextBoxSymptomCategoryDisplayAs;
        private TextBox TextBoxSymptomCategoryName;
        private Label label14;
        private Label label15;
        private Label LabelParentSymptomCategory;
        private ContextMenuStrip contextMenuStripCategory;
        private ToolStripMenuItem newCategoryToolStripMenuItem;
        private ToolStripMenuItem newSymptomToolStripMenuItem;
        private ContextMenuStrip contextMenuStripSymptoms;
        private ToolStripMenuItem newSymptomToolStripMenuItem1;
        private Dropdown_Button.UserControlButtonWithMenu BtnSymptomNew;
        private ImageList ImageListSymptoms;
        private TextBox TextBoxSymptomCategoryId;
        private controls.text.NameTextBoxAllowSpace TextBoxMedicalSymptomCode;
        private Label label1;
    }
}