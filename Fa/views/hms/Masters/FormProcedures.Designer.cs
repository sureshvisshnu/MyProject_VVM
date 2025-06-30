namespace fa.views.hms.masters
{
    partial class FormProcedures
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProcedures));
            TabControlMedicalProcedure = new TabControl();
            TabProcedureInfo = new TabPage();
            comboBoxSwapTextBoxMedicalProcedureCategory = new controls.ComboBoxSwapTextBox();
            TextBoxMedicalProcedureCode = new controls.text.NameTextBoxAllowSpace(components);
            label4 = new Label();
            LabelMedicalProcedureCategory = new Label();
            TextBoxcMedicalProcedureCurrency = new controls.text.CurrencyTextBox();
            label3 = new Label();
            TextBoxMedicalProcedureDisplayName = new controls.text.NameTextBoxAllowSpace(components);
            TextBoxMedicalProcedureName = new controls.text.NameTextBoxAllowSpace(components);
            label2 = new Label();
            GridViewKeywords = new controls.DataViewVerticalScroll();
            dataGridViewTextBoxColumn11 = new DataGridViewTextBoxColumn();
            Keywords = new controls.grid.DataGridViewNameColumn();
            DeleteKWords = new DataGridViewButtonColumn();
            dataGridViewTextBoxColumn12 = new DataGridViewTextBoxColumn();
            TextBoxMedicalProcedureReason = new TextBox();
            CheckBoxMedicalProcedureIsActive = new CheckBox();
            label1 = new Label();
            TextBoxMedicalProcedureDescription = new TextBox();
            LabelCostCenterDescription = new Label();
            LabelCostCenterName = new Label();
            GridViewElementInfo = new controls.DataViewVerticalScroll();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn9 = new controls.grid.DataGridViewNameColumn();
            Description = new DataGridViewTextBoxColumn();
            Fee = new controls.grid.DataGridViewCurrencyColumn();
            Remove = new DataGridViewButtonColumn();
            dataGridViewTextBoxColumn10 = new DataGridViewTextBoxColumn();
            BtnMedicalProcedureDelete = new Button();
            TreeViewMedicalProcedure = new TreeView();
            contextMenuStripCategory = new ContextMenuStrip(components);
            NewCategoryToolStrip = new ToolStripMenuItem();
            NewMedicalProcedureToolStrip = new ToolStripMenuItem();
            imageListMedicalProcedure = new ImageList(components);
            BtnMedicalProcedureEdit = new Button();
            BtnMedicalProcedureExit = new Button();
            BtnMedicalProcedureSave = new Button();
            BtnMedicalProcedureCancel = new Button();
            statusStrip1 = new StatusStrip();
            MedicalProcedureErrorMsg = new ToolStripStatusLabel();
            TextBoxMedicalProcedureId = new controls.text.NameTextBoxAllowSpace(components);
            CheckBoxMedicalProcedureHasElement = new CheckBox();
            BtnExport = new Button();
            BtnImport = new Button();
            TextBoxProcedureSearch = new controls.text.DelayedTextChangeTextBox();
            TabControlMedicalProcedureCategory = new TabControl();
            TabPageMedicalProcedureCategory = new TabPage();
            comboBoxSwapTextBoxMedicalProcedureParentCategory = new controls.ComboBoxSwapTextBox();
            TextBoxMedicalProcedureCategoryDescription = new TextBox();
            TextBoxMedicalProcedureCategoryDisplayAs = new TextBox();
            TextBoxMedicalProcedureCategoryName = new TextBox();
            LabelMedicalProcedureCategoryParent = new Label();
            LabelMedicalProcedureCategoryDescription = new Label();
            LabelMedicalProcedureCategoryDisplayAs = new Label();
            LabelMedicalProcedureCategoryName = new Label();
            TextBoxMedicalProcedureCategoryId = new TextBox();
            contextMenuStripMedicalProcedure = new ContextMenuStrip(components);
            NewMedicalProcedureToolStripMenuItem = new ToolStripMenuItem();
            BtnMedicalProcedureNew = new Dropdown_Button.UserControlButtonWithMenu();
            TabControlMedicalProcedure.SuspendLayout();
            TabProcedureInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewKeywords).BeginInit();
            ((System.ComponentModel.ISupportInitialize)GridViewElementInfo).BeginInit();
            contextMenuStripCategory.SuspendLayout();
            statusStrip1.SuspendLayout();
            TabControlMedicalProcedureCategory.SuspendLayout();
            TabPageMedicalProcedureCategory.SuspendLayout();
            contextMenuStripMedicalProcedure.SuspendLayout();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Size = new Size(100, 21);
            ProductBatchIdTransport.TabIndex = 0;
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Size = new Size(100, 21);
            AccountIdTransport.TabIndex = 0;
            // 
            // TabControlMedicalProcedure
            // 
            TabControlMedicalProcedure.Controls.Add(TabProcedureInfo);
            TabControlMedicalProcedure.Location = new Point(309, 12);
            TabControlMedicalProcedure.Name = "TabControlMedicalProcedure";
            TabControlMedicalProcedure.SelectedIndex = 0;
            TabControlMedicalProcedure.Size = new Size(801, 432);
            TabControlMedicalProcedure.TabIndex = 5;
            // 
            // TabProcedureInfo
            // 
            TabProcedureInfo.Controls.Add(comboBoxSwapTextBoxMedicalProcedureCategory);
            TabProcedureInfo.Controls.Add(TextBoxMedicalProcedureCode);
            TabProcedureInfo.Controls.Add(label4);
            TabProcedureInfo.Controls.Add(LabelMedicalProcedureCategory);
            TabProcedureInfo.Controls.Add(TextBoxcMedicalProcedureCurrency);
            TabProcedureInfo.Controls.Add(label3);
            TabProcedureInfo.Controls.Add(TextBoxMedicalProcedureDisplayName);
            TabProcedureInfo.Controls.Add(TextBoxMedicalProcedureName);
            TabProcedureInfo.Controls.Add(label2);
            TabProcedureInfo.Controls.Add(GridViewKeywords);
            TabProcedureInfo.Controls.Add(TextBoxMedicalProcedureReason);
            TabProcedureInfo.Controls.Add(CheckBoxMedicalProcedureIsActive);
            TabProcedureInfo.Controls.Add(label1);
            TabProcedureInfo.Controls.Add(TextBoxMedicalProcedureDescription);
            TabProcedureInfo.Controls.Add(LabelCostCenterDescription);
            TabProcedureInfo.Controls.Add(LabelCostCenterName);
            TabProcedureInfo.Location = new Point(4, 22);
            TabProcedureInfo.Name = "TabProcedureInfo";
            TabProcedureInfo.Padding = new Padding(3);
            TabProcedureInfo.Size = new Size(793, 406);
            TabProcedureInfo.TabIndex = 0;
            TabProcedureInfo.Text = "Procedure Details";
            TabProcedureInfo.UseVisualStyleBackColor = true;
            // 
            // comboBoxSwapTextBoxMedicalProcedureCategory
            // 
            comboBoxSwapTextBoxMedicalProcedureCategory.FormattingEnabled = true;
            comboBoxSwapTextBoxMedicalProcedureCategory.Location = new Point(18, 230);
            comboBoxSwapTextBoxMedicalProcedureCategory.Name = "comboBoxSwapTextBoxMedicalProcedureCategory";
            comboBoxSwapTextBoxMedicalProcedureCategory.Size = new Size(330, 21);
            comboBoxSwapTextBoxMedicalProcedureCategory.TabIndex = 10;
            comboBoxSwapTextBoxMedicalProcedureCategory.TxtVisible = true;
            // 
            // TextBoxMedicalProcedureCode
            // 
            TextBoxMedicalProcedureCode.BackColor = Color.White;
            TextBoxMedicalProcedureCode.Location = new Point(18, 74);
            TextBoxMedicalProcedureCode.MaxLength = 50;
            TextBoxMedicalProcedureCode.Name = "TextBoxMedicalProcedureCode";
            TextBoxMedicalProcedureCode.Size = new Size(531, 21);
            TextBoxMedicalProcedureCode.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(15, 56);
            label4.Name = "label4";
            label4.Size = new Size(96, 13);
            label4.TabIndex = 27;
            label4.Text = "Procedure Code";
            // 
            // LabelMedicalProcedureCategory
            // 
            LabelMedicalProcedureCategory.AutoSize = true;
            LabelMedicalProcedureCategory.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelMedicalProcedureCategory.Location = new Point(18, 214);
            LabelMedicalProcedureCategory.Name = "LabelMedicalProcedureCategory";
            LabelMedicalProcedureCategory.Size = new Size(59, 13);
            LabelMedicalProcedureCategory.TabIndex = 25;
            LabelMedicalProcedureCategory.Text = "Category";
            // 
            // TextBoxcMedicalProcedureCurrency
            // 
            TextBoxcMedicalProcedureCurrency.BackColor = Color.White;
            TextBoxcMedicalProcedureCurrency.Decimals = 2;
            TextBoxcMedicalProcedureCurrency.Length = 10;
            TextBoxcMedicalProcedureCurrency.Location = new Point(18, 369);
            TextBoxcMedicalProcedureCurrency.Name = "TextBoxcMedicalProcedureCurrency";
            TextBoxcMedicalProcedureCurrency.ReadOnly = true;
            TextBoxcMedicalProcedureCurrency.Size = new Size(100, 21);
            TextBoxcMedicalProcedureCurrency.TabIndex = 13;
            TextBoxcMedicalProcedureCurrency.Text = "0.00";
            TextBoxcMedicalProcedureCurrency.TextAlign = HorizontalAlignment.Right;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(15, 351);
            label3.Name = "label3";
            label3.Size = new Size(25, 13);
            label3.TabIndex = 24;
            label3.Text = "Fee";
            // 
            // TextBoxMedicalProcedureDisplayName
            // 
            TextBoxMedicalProcedureDisplayName.BackColor = Color.White;
            TextBoxMedicalProcedureDisplayName.Location = new Point(18, 114);
            TextBoxMedicalProcedureDisplayName.MaxLength = 50;
            TextBoxMedicalProcedureDisplayName.Name = "TextBoxMedicalProcedureDisplayName";
            TextBoxMedicalProcedureDisplayName.Size = new Size(531, 21);
            TextBoxMedicalProcedureDisplayName.TabIndex = 8;
            // 
            // TextBoxMedicalProcedureName
            // 
            TextBoxMedicalProcedureName.BackColor = Color.White;
            TextBoxMedicalProcedureName.Location = new Point(18, 32);
            TextBoxMedicalProcedureName.MaxLength = 50;
            TextBoxMedicalProcedureName.Name = "TextBoxMedicalProcedureName";
            TextBoxMedicalProcedureName.Size = new Size(531, 21);
            TextBoxMedicalProcedureName.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(15, 95);
            label2.Name = "label2";
            label2.Size = new Size(71, 13);
            label2.TabIndex = 22;
            label2.Text = "Display Name";
            // 
            // GridViewKeywords
            // 
            GridViewKeywords.AllowUserToResizeColumns = false;
            GridViewKeywords.AllowUserToResizeRows = false;
            GridViewKeywords.BackgroundColor = Color.White;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewKeywords.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewKeywords.ColumnHeadersHeight = 20;
            GridViewKeywords.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewKeywords.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn11, Keywords, DeleteKWords, dataGridViewTextBoxColumn12 });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            GridViewKeywords.DefaultCellStyle = dataGridViewCellStyle3;
            GridViewKeywords.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewKeywords.EnableHeadersVisualStyles = false;
            GridViewKeywords.Location = new Point(558, 14);
            GridViewKeywords.Name = "GridViewKeywords";
            GridViewKeywords.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = Color.White;
            dataGridViewCellStyle4.SelectionForeColor = Color.Black;
            GridViewKeywords.RowsDefaultCellStyle = dataGridViewCellStyle4;
            GridViewKeywords.ShowCellToolTips = false;
            GridViewKeywords.Size = new Size(221, 376);
            GridViewKeywords.TabIndex = 14;
            GridViewKeywords.CellClick += GridViewKeywords_CellClick;
            GridViewKeywords.DataError += GridViewKeywords_DataError;
            GridViewKeywords.RowsAdded += GridViewKeywords_RowsAdded;
            GridViewKeywords.Enter += GridViewKeywords_Enter;
            // 
            // dataGridViewTextBoxColumn11
            // 
            dataGridViewTextBoxColumn11.HeaderText = "#";
            dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            dataGridViewTextBoxColumn11.ReadOnly = true;
            dataGridViewTextBoxColumn11.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn11.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn11.Width = 25;
            // 
            // Keywords
            // 
            Keywords.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Keywords.HeaderText = "Keywords";
            Keywords.Name = "Keywords";
            Keywords.NameLength = 30;
            Keywords.Resizable = DataGridViewTriState.False;
            // 
            // DeleteKWords
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = "X";
            DeleteKWords.DefaultCellStyle = dataGridViewCellStyle2;
            DeleteKWords.HeaderText = "...";
            DeleteKWords.Name = "DeleteKWords";
            DeleteKWords.ReadOnly = true;
            DeleteKWords.Resizable = DataGridViewTriState.False;
            DeleteKWords.Width = 25;
            // 
            // dataGridViewTextBoxColumn12
            // 
            dataGridViewTextBoxColumn12.HeaderText = "Id";
            dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            dataGridViewTextBoxColumn12.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn12.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn12.Visible = false;
            dataGridViewTextBoxColumn12.Width = 5;
            // 
            // TextBoxMedicalProcedureReason
            // 
            TextBoxMedicalProcedureReason.BackColor = Color.White;
            TextBoxMedicalProcedureReason.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxMedicalProcedureReason.Location = new Point(18, 288);
            TextBoxMedicalProcedureReason.MaxLength = 250;
            TextBoxMedicalProcedureReason.Multiline = true;
            TextBoxMedicalProcedureReason.Name = "TextBoxMedicalProcedureReason";
            TextBoxMedicalProcedureReason.Size = new Size(531, 59);
            TextBoxMedicalProcedureReason.TabIndex = 12;
            // 
            // CheckBoxMedicalProcedureIsActive
            // 
            CheckBoxMedicalProcedureIsActive.AutoSize = true;
            CheckBoxMedicalProcedureIsActive.Location = new Point(18, 254);
            CheckBoxMedicalProcedureIsActive.Name = "CheckBoxMedicalProcedureIsActive";
            CheckBoxMedicalProcedureIsActive.Size = new Size(73, 17);
            CheckBoxMedicalProcedureIsActive.TabIndex = 11;
            CheckBoxMedicalProcedureIsActive.Text = "Is Active?";
            CheckBoxMedicalProcedureIsActive.UseVisualStyleBackColor = true;
            CheckBoxMedicalProcedureIsActive.CheckedChanged += CheckBoxMedicalProcedureIsActive_CheckedChanged;
            CheckBoxMedicalProcedureIsActive.PreviewKeyDown += CheckBoxMedicalProcedureIsActive_PreviewKeyDown;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(15, 272);
            label1.Name = "label1";
            label1.Size = new Size(102, 13);
            label1.TabIndex = 19;
            label1.Text = "Reason for Inactive";
            // 
            // TextBoxMedicalProcedureDescription
            // 
            TextBoxMedicalProcedureDescription.BackColor = Color.White;
            TextBoxMedicalProcedureDescription.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxMedicalProcedureDescription.Location = new Point(18, 149);
            TextBoxMedicalProcedureDescription.MaxLength = 250;
            TextBoxMedicalProcedureDescription.Multiline = true;
            TextBoxMedicalProcedureDescription.Name = "TextBoxMedicalProcedureDescription";
            TextBoxMedicalProcedureDescription.Size = new Size(531, 59);
            TextBoxMedicalProcedureDescription.TabIndex = 9;
            // 
            // LabelCostCenterDescription
            // 
            LabelCostCenterDescription.AutoSize = true;
            LabelCostCenterDescription.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelCostCenterDescription.Location = new Point(15, 136);
            LabelCostCenterDescription.Name = "LabelCostCenterDescription";
            LabelCostCenterDescription.Size = new Size(60, 13);
            LabelCostCenterDescription.TabIndex = 12;
            LabelCostCenterDescription.Text = "Description";
            // 
            // LabelCostCenterName
            // 
            LabelCostCenterName.AutoSize = true;
            LabelCostCenterName.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelCostCenterName.Location = new Point(15, 14);
            LabelCostCenterName.Name = "LabelCostCenterName";
            LabelCostCenterName.Size = new Size(39, 13);
            LabelCostCenterName.TabIndex = 11;
            LabelCostCenterName.Text = "Name";
            // 
            // GridViewElementInfo
            // 
            GridViewElementInfo.AllowUserToDeleteRows = false;
            GridViewElementInfo.AllowUserToResizeColumns = false;
            GridViewElementInfo.AllowUserToResizeRows = false;
            GridViewElementInfo.BackgroundColor = SystemColors.Control;
            GridViewElementInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GridViewElementInfo.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn9, Description, Fee, Remove, dataGridViewTextBoxColumn10 });
            GridViewElementInfo.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewElementInfo.Location = new Point(272, 518);
            GridViewElementInfo.Name = "GridViewElementInfo";
            GridViewElementInfo.RowHeadersVisible = false;
            dataGridViewCellStyle7.BackColor = Color.White;
            dataGridViewCellStyle7.ForeColor = Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = Color.White;
            dataGridViewCellStyle7.SelectionForeColor = Color.Black;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            GridViewElementInfo.RowsDefaultCellStyle = dataGridViewCellStyle7;
            GridViewElementInfo.ScrollBars = ScrollBars.Vertical;
            GridViewElementInfo.Size = new Size(771, 349);
            GridViewElementInfo.TabIndex = 12;
            GridViewElementInfo.CellClick += GridViewElementInfo_CellClick;
            GridViewElementInfo.CellEnter += GridViewElementInfo_CellEnter;
            GridViewElementInfo.CellFormatting += GridViewElementInfo_CellFormatting;
            GridViewElementInfo.DataError += GridViewElementInfo_DataError;
            GridViewElementInfo.EditingControlShowing += GridViewElementInfo_EditingControlShowing;
            GridViewElementInfo.RowsAdded += GridViewElementInfo_RowsAdded;
            GridViewElementInfo.Enter += GridViewElementInfo_Enter;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "#";
            dataGridViewTextBoxColumn1.MaxInputLength = 30;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            dataGridViewTextBoxColumn1.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn1.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn1.Width = 25;
            // 
            // dataGridViewTextBoxColumn9
            // 
            dataGridViewTextBoxColumn9.HeaderText = "Element Name";
            dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            dataGridViewTextBoxColumn9.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn9.Width = 220;
            // 
            // Description
            // 
            Description.HeaderText = "Description";
            Description.MaxInputLength = 250;
            Description.Name = "Description";
            Description.Width = 375;
            // 
            // Fee
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
            Fee.DefaultCellStyle = dataGridViewCellStyle5;
            Fee.HeaderText = "Fee";
            Fee.Name = "Fee";
            Fee.Resizable = DataGridViewTriState.True;
            Fee.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // Remove
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.NullValue = "X";
            Remove.DefaultCellStyle = dataGridViewCellStyle6;
            Remove.HeaderText = "...";
            Remove.Name = "Remove";
            Remove.Width = 25;
            // 
            // dataGridViewTextBoxColumn10
            // 
            dataGridViewTextBoxColumn10.HeaderText = "ID";
            dataGridViewTextBoxColumn10.MaxInputLength = 50;
            dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            dataGridViewTextBoxColumn10.Resizable = DataGridViewTriState.True;
            dataGridViewTextBoxColumn10.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn10.Visible = false;
            // 
            // BtnMedicalProcedureDelete
            // 
            BtnMedicalProcedureDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnMedicalProcedureDelete.Location = new Point(99, 448);
            BtnMedicalProcedureDelete.Name = "BtnMedicalProcedureDelete";
            BtnMedicalProcedureDelete.Size = new Size(83, 23);
            BtnMedicalProcedureDelete.TabIndex = 0;
            BtnMedicalProcedureDelete.Text = "Delete [F4]";
            BtnMedicalProcedureDelete.UseVisualStyleBackColor = true;
            BtnMedicalProcedureDelete.Click += BtnMedicalProcedureDelete_Click;
            // 
            // TreeViewMedicalProcedure
            // 
            TreeViewMedicalProcedure.ContextMenuStrip = contextMenuStripCategory;
            TreeViewMedicalProcedure.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TreeViewMedicalProcedure.HideSelection = false;
            TreeViewMedicalProcedure.ImageIndex = 0;
            TreeViewMedicalProcedure.ImageList = imageListMedicalProcedure;
            TreeViewMedicalProcedure.Location = new Point(12, 39);
            TreeViewMedicalProcedure.Name = "TreeViewMedicalProcedure";
            TreeViewMedicalProcedure.SelectedImageIndex = 0;
            TreeViewMedicalProcedure.Size = new Size(287, 401);
            TreeViewMedicalProcedure.TabIndex = 0;
            TreeViewMedicalProcedure.AfterSelect += TreeViewMedicalProcedure_AfterSelect;
            TreeViewMedicalProcedure.NodeMouseClick += TreeViewMedicalProcedure_NodeMouseClick;
            TreeViewMedicalProcedure.Enter += TreeViewMedicalProcedure_Enter;
            // 
            // contextMenuStripCategory
            // 
            contextMenuStripCategory.Items.AddRange(new ToolStripItem[] { NewCategoryToolStrip, NewMedicalProcedureToolStrip });
            contextMenuStripCategory.Name = "contextMenuStripCategory";
            contextMenuStripCategory.Size = new Size(174, 48);
            // 
            // NewCategoryToolStrip
            // 
            NewCategoryToolStrip.Image = (Image)resources.GetObject("NewCategoryToolStrip.Image");
            NewCategoryToolStrip.Name = "NewCategoryToolStrip";
            NewCategoryToolStrip.Size = new Size(173, 22);
            NewCategoryToolStrip.Text = "Category";
            NewCategoryToolStrip.Click += MedicalProcedureCategorytoolstrip_Click;
            // 
            // NewMedicalProcedureToolStrip
            // 
            NewMedicalProcedureToolStrip.Image = (Image)resources.GetObject("NewMedicalProcedureToolStrip.Image");
            NewMedicalProcedureToolStrip.Name = "NewMedicalProcedureToolStrip";
            NewMedicalProcedureToolStrip.Size = new Size(173, 22);
            NewMedicalProcedureToolStrip.Text = "Medical Procedure";
            NewMedicalProcedureToolStrip.Click += newMedicalProcedureToolStripMenuItem_Click;
            // 
            // imageListMedicalProcedure
            // 
            imageListMedicalProcedure.ColorDepth = ColorDepth.Depth8Bit;
            imageListMedicalProcedure.ImageStream = (ImageListStreamer)resources.GetObject("imageListMedicalProcedure.ImageStream");
            imageListMedicalProcedure.TransparentColor = Color.Transparent;
            imageListMedicalProcedure.Images.SetKeyName(0, "Category.png");
            imageListMedicalProcedure.Images.SetKeyName(1, "medical_procedure.png");
            // 
            // BtnMedicalProcedureEdit
            // 
            BtnMedicalProcedureEdit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnMedicalProcedureEdit.Location = new Point(190, 448);
            BtnMedicalProcedureEdit.Name = "BtnMedicalProcedureEdit";
            BtnMedicalProcedureEdit.Size = new Size(83, 23);
            BtnMedicalProcedureEdit.TabIndex = 0;
            BtnMedicalProcedureEdit.Text = "Edit [F7]";
            BtnMedicalProcedureEdit.UseVisualStyleBackColor = true;
            BtnMedicalProcedureEdit.Click += BtnMedicalProcedureEdit_Click;
            // 
            // BtnMedicalProcedureExit
            // 
            BtnMedicalProcedureExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnMedicalProcedureExit.Location = new Point(1018, 449);
            BtnMedicalProcedureExit.Name = "BtnMedicalProcedureExit";
            BtnMedicalProcedureExit.Size = new Size(83, 23);
            BtnMedicalProcedureExit.TabIndex = 16;
            BtnMedicalProcedureExit.Text = "Exit [F10]";
            BtnMedicalProcedureExit.UseVisualStyleBackColor = true;
            BtnMedicalProcedureExit.Click += BtnMedicalProcedureExit_Click;
            // 
            // BtnMedicalProcedureSave
            // 
            BtnMedicalProcedureSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnMedicalProcedureSave.Location = new Point(929, 449);
            BtnMedicalProcedureSave.Name = "BtnMedicalProcedureSave";
            BtnMedicalProcedureSave.Size = new Size(83, 23);
            BtnMedicalProcedureSave.TabIndex = 5;
            BtnMedicalProcedureSave.Text = "Save [F8]";
            BtnMedicalProcedureSave.UseVisualStyleBackColor = true;
            BtnMedicalProcedureSave.Click += BtnMedicalProcedureSave_Click;
            // 
            // BtnMedicalProcedureCancel
            // 
            BtnMedicalProcedureCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnMedicalProcedureCancel.Location = new Point(839, 449);
            BtnMedicalProcedureCancel.Name = "BtnMedicalProcedureCancel";
            BtnMedicalProcedureCancel.Size = new Size(83, 23);
            BtnMedicalProcedureCancel.TabIndex = 13;
            BtnMedicalProcedureCancel.Text = "Cancel [Esc]";
            BtnMedicalProcedureCancel.UseVisualStyleBackColor = true;
            BtnMedicalProcedureCancel.Click += BtnMedicalProcedureCancel_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { MedicalProcedureErrorMsg });
            statusStrip1.Location = new Point(0, 486);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1119, 22);
            statusStrip1.TabIndex = 20;
            statusStrip1.Text = "statusStrip1";
            // 
            // MedicalProcedureErrorMsg
            // 
            MedicalProcedureErrorMsg.Name = "MedicalProcedureErrorMsg";
            MedicalProcedureErrorMsg.Size = new Size(43, 17);
            MedicalProcedureErrorMsg.Text = "            ";
            // 
            // TextBoxMedicalProcedureId
            // 
            TextBoxMedicalProcedureId.BackColor = Color.White;
            TextBoxMedicalProcedureId.Location = new Point(161, 541);
            TextBoxMedicalProcedureId.MaxLength = 50;
            TextBoxMedicalProcedureId.Name = "TextBoxMedicalProcedureId";
            TextBoxMedicalProcedureId.Size = new Size(91, 21);
            TextBoxMedicalProcedureId.TabIndex = 21;
            TextBoxMedicalProcedureId.Visible = false;
            // 
            // CheckBoxMedicalProcedureHasElement
            // 
            CheckBoxMedicalProcedureHasElement.AutoSize = true;
            CheckBoxMedicalProcedureHasElement.Location = new Point(161, 518);
            CheckBoxMedicalProcedureHasElement.Name = "CheckBoxMedicalProcedureHasElement";
            CheckBoxMedicalProcedureHasElement.Size = new Size(95, 17);
            CheckBoxMedicalProcedureHasElement.TabIndex = 10;
            CheckBoxMedicalProcedureHasElement.Text = "Has Elements?";
            CheckBoxMedicalProcedureHasElement.UseVisualStyleBackColor = true;
            CheckBoxMedicalProcedureHasElement.Visible = false;
            CheckBoxMedicalProcedureHasElement.CheckedChanged += CheckBoxMedicalProcedureHasElement_CheckedChanged;
            // 
            // BtnExport
            // 
            BtnExport.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExport.Location = new Point(749, 449);
            BtnExport.Name = "BtnExport";
            BtnExport.Size = new Size(83, 23);
            BtnExport.TabIndex = 14;
            BtnExport.Text = "Export";
            BtnExport.UseVisualStyleBackColor = true;
            BtnExport.Click += BtnExport_Click;
            // 
            // BtnImport
            // 
            BtnImport.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnImport.Location = new Point(660, 449);
            BtnImport.Name = "BtnImport";
            BtnImport.Size = new Size(83, 23);
            BtnImport.TabIndex = 15;
            BtnImport.Text = "Import";
            BtnImport.UseVisualStyleBackColor = true;
            BtnImport.Click += BtnImport_Click;
            // 
            // TextBoxProcedureSearch
            // 
            TextBoxProcedureSearch.BackColor = SystemColors.Window;
            TextBoxProcedureSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxProcedureSearch.Delay = true;
            TextBoxProcedureSearch.DelayTime = 1000;
            TextBoxProcedureSearch.Location = new Point(12, 12);
            TextBoxProcedureSearch.MaxLength = 35;
            TextBoxProcedureSearch.Name = "TextBoxProcedureSearch";
            TextBoxProcedureSearch.Searchstartfrom = 2;
            TextBoxProcedureSearch.Size = new Size(287, 21);
            TextBoxProcedureSearch.TabIndex = 0;
            TextBoxProcedureSearch.TextChanged += TextBoxProcedureSearch_TextChanged;
            TextBoxProcedureSearch.KeyDown += TextBoxProcedureSearch_KeyDown;
            // 
            // TabControlMedicalProcedureCategory
            // 
            TabControlMedicalProcedureCategory.Controls.Add(TabPageMedicalProcedureCategory);
            TabControlMedicalProcedureCategory.Location = new Point(309, 12);
            TabControlMedicalProcedureCategory.Name = "TabControlMedicalProcedureCategory";
            TabControlMedicalProcedureCategory.SelectedIndex = 0;
            TabControlMedicalProcedureCategory.Size = new Size(801, 401);
            TabControlMedicalProcedureCategory.TabIndex = 0;
            // 
            // TabPageMedicalProcedureCategory
            // 
            TabPageMedicalProcedureCategory.Controls.Add(comboBoxSwapTextBoxMedicalProcedureParentCategory);
            TabPageMedicalProcedureCategory.Controls.Add(TextBoxMedicalProcedureCategoryDescription);
            TabPageMedicalProcedureCategory.Controls.Add(TextBoxMedicalProcedureCategoryDisplayAs);
            TabPageMedicalProcedureCategory.Controls.Add(TextBoxMedicalProcedureCategoryName);
            TabPageMedicalProcedureCategory.Controls.Add(LabelMedicalProcedureCategoryParent);
            TabPageMedicalProcedureCategory.Controls.Add(LabelMedicalProcedureCategoryDescription);
            TabPageMedicalProcedureCategory.Controls.Add(LabelMedicalProcedureCategoryDisplayAs);
            TabPageMedicalProcedureCategory.Controls.Add(LabelMedicalProcedureCategoryName);
            TabPageMedicalProcedureCategory.Location = new Point(4, 22);
            TabPageMedicalProcedureCategory.Name = "TabPageMedicalProcedureCategory";
            TabPageMedicalProcedureCategory.Padding = new Padding(3);
            TabPageMedicalProcedureCategory.Size = new Size(793, 375);
            TabPageMedicalProcedureCategory.TabIndex = 0;
            TabPageMedicalProcedureCategory.Text = "Category";
            TabPageMedicalProcedureCategory.UseVisualStyleBackColor = true;
            // 
            // comboBoxSwapTextBoxMedicalProcedureParentCategory
            // 
            comboBoxSwapTextBoxMedicalProcedureParentCategory.FormattingEnabled = true;
            comboBoxSwapTextBoxMedicalProcedureParentCategory.Location = new Point(18, 214);
            comboBoxSwapTextBoxMedicalProcedureParentCategory.Name = "comboBoxSwapTextBoxMedicalProcedureParentCategory";
            comboBoxSwapTextBoxMedicalProcedureParentCategory.Size = new Size(354, 21);
            comboBoxSwapTextBoxMedicalProcedureParentCategory.TabIndex = 4;
            comboBoxSwapTextBoxMedicalProcedureParentCategory.TxtVisible = true;
            // 
            // TextBoxMedicalProcedureCategoryDescription
            // 
            TextBoxMedicalProcedureCategoryDescription.BackColor = SystemColors.Window;
            TextBoxMedicalProcedureCategoryDescription.Location = new Point(18, 112);
            TextBoxMedicalProcedureCategoryDescription.Multiline = true;
            TextBoxMedicalProcedureCategoryDescription.Name = "TextBoxMedicalProcedureCategoryDescription";
            TextBoxMedicalProcedureCategoryDescription.Size = new Size(524, 84);
            TextBoxMedicalProcedureCategoryDescription.TabIndex = 3;
            // 
            // TextBoxMedicalProcedureCategoryDisplayAs
            // 
            TextBoxMedicalProcedureCategoryDisplayAs.BackColor = SystemColors.Window;
            TextBoxMedicalProcedureCategoryDisplayAs.Location = new Point(18, 72);
            TextBoxMedicalProcedureCategoryDisplayAs.Name = "TextBoxMedicalProcedureCategoryDisplayAs";
            TextBoxMedicalProcedureCategoryDisplayAs.Size = new Size(524, 21);
            TextBoxMedicalProcedureCategoryDisplayAs.TabIndex = 2;
            // 
            // TextBoxMedicalProcedureCategoryName
            // 
            TextBoxMedicalProcedureCategoryName.BackColor = SystemColors.Window;
            TextBoxMedicalProcedureCategoryName.Location = new Point(18, 32);
            TextBoxMedicalProcedureCategoryName.Name = "TextBoxMedicalProcedureCategoryName";
            TextBoxMedicalProcedureCategoryName.Size = new Size(524, 21);
            TextBoxMedicalProcedureCategoryName.TabIndex = 1;
            // 
            // LabelMedicalProcedureCategoryParent
            // 
            LabelMedicalProcedureCategoryParent.AutoSize = true;
            LabelMedicalProcedureCategoryParent.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelMedicalProcedureCategoryParent.Location = new Point(18, 199);
            LabelMedicalProcedureCategoryParent.Name = "LabelMedicalProcedureCategoryParent";
            LabelMedicalProcedureCategoryParent.Size = new Size(100, 13);
            LabelMedicalProcedureCategoryParent.TabIndex = 0;
            LabelMedicalProcedureCategoryParent.Text = "Parent Category";
            // 
            // LabelMedicalProcedureCategoryDescription
            // 
            LabelMedicalProcedureCategoryDescription.AutoSize = true;
            LabelMedicalProcedureCategoryDescription.Location = new Point(18, 96);
            LabelMedicalProcedureCategoryDescription.Name = "LabelMedicalProcedureCategoryDescription";
            LabelMedicalProcedureCategoryDescription.Size = new Size(60, 13);
            LabelMedicalProcedureCategoryDescription.TabIndex = 0;
            LabelMedicalProcedureCategoryDescription.Text = "Description";
            // 
            // LabelMedicalProcedureCategoryDisplayAs
            // 
            LabelMedicalProcedureCategoryDisplayAs.AutoSize = true;
            LabelMedicalProcedureCategoryDisplayAs.Location = new Point(18, 56);
            LabelMedicalProcedureCategoryDisplayAs.Name = "LabelMedicalProcedureCategoryDisplayAs";
            LabelMedicalProcedureCategoryDisplayAs.Size = new Size(56, 13);
            LabelMedicalProcedureCategoryDisplayAs.TabIndex = 0;
            LabelMedicalProcedureCategoryDisplayAs.Text = "Display As";
            // 
            // LabelMedicalProcedureCategoryName
            // 
            LabelMedicalProcedureCategoryName.AutoSize = true;
            LabelMedicalProcedureCategoryName.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelMedicalProcedureCategoryName.Location = new Point(18, 16);
            LabelMedicalProcedureCategoryName.Name = "LabelMedicalProcedureCategoryName";
            LabelMedicalProcedureCategoryName.Size = new Size(39, 13);
            LabelMedicalProcedureCategoryName.TabIndex = 0;
            LabelMedicalProcedureCategoryName.Text = "Name";
            // 
            // TextBoxMedicalProcedureCategoryId
            // 
            TextBoxMedicalProcedureCategoryId.Location = new Point(330, 450);
            TextBoxMedicalProcedureCategoryId.Name = "TextBoxMedicalProcedureCategoryId";
            TextBoxMedicalProcedureCategoryId.Size = new Size(100, 21);
            TextBoxMedicalProcedureCategoryId.TabIndex = 28;
            TextBoxMedicalProcedureCategoryId.Visible = false;
            // 
            // contextMenuStripMedicalProcedure
            // 
            contextMenuStripMedicalProcedure.Items.AddRange(new ToolStripItem[] { NewMedicalProcedureToolStripMenuItem });
            contextMenuStripMedicalProcedure.Name = "contextMenuStripMedicalProcedure";
            contextMenuStripMedicalProcedure.Size = new Size(174, 26);
            // 
            // NewMedicalProcedureToolStripMenuItem
            // 
            NewMedicalProcedureToolStripMenuItem.Image = (Image)resources.GetObject("NewMedicalProcedureToolStripMenuItem.Image");
            NewMedicalProcedureToolStripMenuItem.Name = "NewMedicalProcedureToolStripMenuItem";
            NewMedicalProcedureToolStripMenuItem.Size = new Size(173, 22);
            NewMedicalProcedureToolStripMenuItem.Text = "Medical Procedure";
            // 
            // BtnMedicalProcedureNew
            // 
            BtnMedicalProcedureNew.AllowDrop = true;
            BtnMedicalProcedureNew.ButtonText = "New [F3]";
            BtnMedicalProcedureNew.ContextMenuStrip = contextMenuStripCategory;
            BtnMedicalProcedureNew.ImageList = imageListMedicalProcedure;
            BtnMedicalProcedureNew.Items = (System.Collections.ObjectModel.Collection<string>)resources.GetObject("BtnMedicalProcedureNew.Items");
            BtnMedicalProcedureNew.Location = new Point(13, 447);
            BtnMedicalProcedureNew.Margin = new Padding(4, 3, 4, 3);
            BtnMedicalProcedureNew.Name = "BtnMedicalProcedureNew";
            BtnMedicalProcedureNew.Size = new Size(94, 28);
            BtnMedicalProcedureNew.TabIndex = 29;
            BtnMedicalProcedureNew.ItemClickedEvent += BtnMedicalProcedureNew_ItemClickedEvent;
            // 
            // FormProcedures
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1119, 508);
            Controls.Add(TextBoxMedicalProcedureCategoryId);
            Controls.Add(TextBoxProcedureSearch);
            Controls.Add(BtnImport);
            Controls.Add(BtnExport);
            Controls.Add(GridViewElementInfo);
            Controls.Add(TextBoxMedicalProcedureId);
            Controls.Add(BtnMedicalProcedureEdit);
            Controls.Add(CheckBoxMedicalProcedureHasElement);
            Controls.Add(statusStrip1);
            Controls.Add(TreeViewMedicalProcedure);
            Controls.Add(BtnMedicalProcedureExit);
            Controls.Add(BtnMedicalProcedureDelete);
            Controls.Add(BtnMedicalProcedureSave);
            Controls.Add(BtnMedicalProcedureCancel);
            Controls.Add(BtnMedicalProcedureNew);
            Controls.Add(TabControlMedicalProcedure);
            Controls.Add(TabControlMedicalProcedureCategory);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormProcedures";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Medical Procedures";
            Load += FormProcedures_Load;
            Controls.SetChildIndex(TabControlMedicalProcedureCategory, 0);
            Controls.SetChildIndex(TabControlMedicalProcedure, 0);
            Controls.SetChildIndex(BtnMedicalProcedureNew, 0);
            Controls.SetChildIndex(BtnMedicalProcedureCancel, 0);
            Controls.SetChildIndex(BtnMedicalProcedureSave, 0);
            Controls.SetChildIndex(BtnMedicalProcedureDelete, 0);
            Controls.SetChildIndex(BtnMedicalProcedureExit, 0);
            Controls.SetChildIndex(TreeViewMedicalProcedure, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(CheckBoxMedicalProcedureHasElement, 0);
            Controls.SetChildIndex(BtnMedicalProcedureEdit, 0);
            Controls.SetChildIndex(TextBoxMedicalProcedureId, 0);
            Controls.SetChildIndex(GridViewElementInfo, 0);
            Controls.SetChildIndex(BtnExport, 0);
            Controls.SetChildIndex(BtnImport, 0);
            Controls.SetChildIndex(TextBoxProcedureSearch, 0);
            Controls.SetChildIndex(TextBoxMedicalProcedureCategoryId, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            TabControlMedicalProcedure.ResumeLayout(false);
            TabProcedureInfo.ResumeLayout(false);
            TabProcedureInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewKeywords).EndInit();
            ((System.ComponentModel.ISupportInitialize)GridViewElementInfo).EndInit();
            contextMenuStripCategory.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            TabControlMedicalProcedureCategory.ResumeLayout(false);
            TabPageMedicalProcedureCategory.ResumeLayout(false);
            TabPageMedicalProcedureCategory.PerformLayout();
            contextMenuStripMedicalProcedure.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl TabControlMedicalProcedure;
        private TabPage TabProcedureInfo;
        private controls.text.NameTextBoxAllowSpace TextBoxMedicalProcedureDisplayName;
        private controls.text.NameTextBoxAllowSpace TextBoxMedicalProcedureName;
        private Label label2;
        private controls.DataViewVerticalScroll GridViewKeywords;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private controls.grid.DataGridViewNameColumn Keywords;
        private DataGridViewButtonColumn DeleteKWords;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private TextBox TextBoxMedicalProcedureReason;
        private CheckBox CheckBoxMedicalProcedureIsActive;
        private Label label1;
        private TextBox TextBoxMedicalProcedureDescription;
        private Label LabelCostCenterDescription;
        private Label LabelCostCenterName;
        private controls.DataViewVerticalScroll GridViewElementInfo;
        private Button BtnMedicalProcedureDelete;
        private TreeView TreeViewMedicalProcedure;
        private Button BtnMedicalProcedureEdit;
        private Button BtnMedicalProcedureExit;
        private Button BtnMedicalProcedureSave;
        private Button BtnMedicalProcedureCancel;
        private Label label3;
        private controls.text.CurrencyTextBox TextBoxcMedicalProcedureCurrency;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel MedicalProcedureErrorMsg;
        private controls.text.NameTextBoxAllowSpace TextBoxMedicalProcedureId;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private controls.grid.DataGridViewNameColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn Description;
        private controls.grid.DataGridViewCurrencyColumn Fee;
        private DataGridViewButtonColumn Remove;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private CheckBox CheckBoxMedicalProcedureHasElement;
        private Button BtnExport;
        private Button BtnImport;
        private controls.text.DelayedTextChangeTextBox TextBoxProcedureSearch;
        private controls.ComboBoxSwapTextBox comboBoxSwapTextBoxMedicalProcedureCategory;
        private Label LabelMedicalProcedureCategory;
        private TabControl TabControlMedicalProcedureCategory;
        private TabPage TabPageMedicalProcedureCategory;
        private TextBox TextBoxMedicalProcedureCategoryDescription;
        private TextBox TextBoxMedicalProcedureCategoryDisplayAs;
        private TextBox TextBoxMedicalProcedureCategoryName;
        private Label LabelMedicalProcedureCategoryParent;
        private Label LabelMedicalProcedureCategoryDescription;
        private Label LabelMedicalProcedureCategoryDisplayAs;
        private Label LabelMedicalProcedureCategoryName;
        private controls.ComboBoxSwapTextBox comboBoxSwapTextBoxMedicalProcedureParentCategory;
        private TextBox TextBoxMedicalProcedureCategoryId;
        private ContextMenuStrip contextMenuStripCategory;
        private ToolStripMenuItem NewCategoryToolStrip;
        private ToolStripMenuItem NewMedicalProcedureToolStrip;
        private ContextMenuStrip contextMenuStripMedicalProcedure;
        private ToolStripMenuItem NewMedicalProcedureToolStripMenuItem;
        private Dropdown_Button.UserControlButtonWithMenu BtnMedicalProcedureNew;
        private ImageList imageListMedicalProcedure;
        private controls.text.NameTextBoxAllowSpace TextBoxMedicalProcedureCode;
        private Label label4;
    }
}