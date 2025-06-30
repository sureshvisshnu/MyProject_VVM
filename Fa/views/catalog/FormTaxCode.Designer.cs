namespace fa.views.catalog
{
    partial class FormTaxCode
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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTaxCode));
            GridViewTaxCodeList = new controls.DataViewVerticalScroll();
            Code = new DataGridViewTextBoxColumn();
            Description = new DataGridViewTextBoxColumn();
            Id = new DataGridViewTextBoxColumn();
            TextBoxTaxCodeSearch = new controls.text.DelayedTextChangeTextBox();
            TabControlTaxCode = new TabControl();
            TabTaxCodeDetail = new TabPage();
            LabelTaxcodeHistorical = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            DataGridViewTaxCodeTaxCodeHistory = new controls.DataViewVerticalScroll();
            LabelTaxcodeActive = new Syncfusion.Windows.Forms.Tools.AutoLabel();
            DataGridViewTaxCodeCurrentTaxCode = new controls.DataViewVerticalScroll();
            TaxName = new DataGridViewComboBoxColumn();
            Percentage = new controls.grid.DataGridViewCurrencyColumn();
            EffectiveFrom = new controls.grid.DataGridViewCalendarColumn();
            EffectiveTo = new controls.grid.DataGridViewCalendarColumn();
            Remove = new DataGridViewTextBoxColumn();
            Mapid = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            TextBoxTaxCodeCode = new TextBox();
            TextBoxTaxCodeDetail = new TextBox();
            LabelTaxCodeDetail = new Label();
            LabelTaxCodeCode = new Label();
            BtnTaxCodeReport = new Button();
            BtnTaxCodeImport = new Button();
            TextBoxTaxCodeId = new TextBox();
            BtnTaxCodeSave = new Button();
            BtnTaxCodeCancel = new Button();
            BtnTaxCodeDelete = new Button();
            BtnTaxCodeExit = new Button();
            statusStrip1 = new StatusStrip();
            TaxCodeErrorMsg = new ToolStripStatusLabel();
            BtnTaxCodeNew = new Button();
            BtnTaxCodeExport = new Button();
            BtnTaxCodeEdit = new Button();
            TaxName1 = new DataGridViewTextBoxColumn();
            dataGridViewCurrencyColumn1 = new controls.grid.DataGridViewCurrencyColumn();
            HEffectiveFrom = new controls.grid.DataGridViewCalendarColumn();
            HEffectiveTo = new controls.grid.DataGridViewCalendarColumn();
            Remover = new DataGridViewTextBoxColumn();
            mapids = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)GridViewTaxCodeList).BeginInit();
            TabControlTaxCode.SuspendLayout();
            TabTaxCodeDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DataGridViewTaxCodeTaxCodeHistory).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DataGridViewTaxCodeCurrentTaxCode).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
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
            // GridViewTaxCodeList
            // 
            GridViewTaxCodeList.AllowUserToAddRows = false;
            GridViewTaxCodeList.AllowUserToDeleteRows = false;
            GridViewTaxCodeList.AllowUserToResizeColumns = false;
            GridViewTaxCodeList.AllowUserToResizeRows = false;
            GridViewTaxCodeList.BackgroundColor = SystemColors.Window;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewTaxCodeList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewTaxCodeList.ColumnHeadersHeight = 20;
            GridViewTaxCodeList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewTaxCodeList.Columns.AddRange(new DataGridViewColumn[] { Code, Description, Id });
            GridViewTaxCodeList.EnableHeadersVisualStyles = false;
            GridViewTaxCodeList.Location = new Point(12, 40);
            GridViewTaxCodeList.MultiSelect = false;
            GridViewTaxCodeList.Name = "GridViewTaxCodeList";
            GridViewTaxCodeList.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Control;
            dataGridViewCellStyle2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            GridViewTaxCodeList.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            GridViewTaxCodeList.RowHeadersVisible = false;
            GridViewTaxCodeList.RowTemplate.Height = 20;
            GridViewTaxCodeList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewTaxCodeList.ShowCellToolTips = false;
            GridViewTaxCodeList.Size = new Size(296, 481);
            GridViewTaxCodeList.TabIndex = 1;
            GridViewTaxCodeList.CellClick += GridViewTaxCode_CellClick;
            GridViewTaxCodeList.Leave += GridViewTaxCode_Leave;
            // 
            // Code
            // 
            Code.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Code.HeaderText = "Tax Code";
            Code.Name = "Code";
            Code.ReadOnly = true;
            Code.Resizable = DataGridViewTriState.False;
            Code.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Description
            // 
            Description.HeaderText = "Desccription";
            Description.Name = "Description";
            Description.ReadOnly = true;
            Description.Resizable = DataGridViewTriState.False;
            Description.SortMode = DataGridViewColumnSortMode.NotSortable;
            Description.Width = 174;
            // 
            // Id
            // 
            Id.HeaderText = "Id";
            Id.Name = "Id";
            Id.ReadOnly = true;
            Id.Visible = false;
            // 
            // TextBoxTaxCodeSearch
            // 
            TextBoxTaxCodeSearch.BackColor = SystemColors.Window;
            TextBoxTaxCodeSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxTaxCodeSearch.Delay = false;
            TextBoxTaxCodeSearch.DelayTime = 1000;
            TextBoxTaxCodeSearch.Location = new Point(13, 13);
            TextBoxTaxCodeSearch.MaxLength = 10;
            TextBoxTaxCodeSearch.Name = "TextBoxTaxCodeSearch";
            TextBoxTaxCodeSearch.Searchstartfrom = 1;
            TextBoxTaxCodeSearch.Size = new Size(295, 21);
            TextBoxTaxCodeSearch.TabIndex = 0;
            TextBoxTaxCodeSearch.TextChanged += TextBoxTaxCodeSearch_TextChanged;
            // 
            // TabControlTaxCode
            // 
            TabControlTaxCode.Controls.Add(TabTaxCodeDetail);
            TabControlTaxCode.Location = new Point(314, 13);
            TabControlTaxCode.Name = "TabControlTaxCode";
            TabControlTaxCode.SelectedIndex = 0;
            TabControlTaxCode.Size = new Size(626, 511);
            TabControlTaxCode.TabIndex = 5;
            // 
            // TabTaxCodeDetail
            // 
            TabTaxCodeDetail.Controls.Add(LabelTaxcodeHistorical);
            TabTaxCodeDetail.Controls.Add(DataGridViewTaxCodeTaxCodeHistory);
            TabTaxCodeDetail.Controls.Add(LabelTaxcodeActive);
            TabTaxCodeDetail.Controls.Add(DataGridViewTaxCodeCurrentTaxCode);
            TabTaxCodeDetail.Controls.Add(TextBoxTaxCodeCode);
            TabTaxCodeDetail.Controls.Add(TextBoxTaxCodeDetail);
            TabTaxCodeDetail.Controls.Add(LabelTaxCodeDetail);
            TabTaxCodeDetail.Controls.Add(LabelTaxCodeCode);
            TabTaxCodeDetail.Location = new Point(4, 22);
            TabTaxCodeDetail.Name = "TabTaxCodeDetail";
            TabTaxCodeDetail.Padding = new Padding(3);
            TabTaxCodeDetail.Size = new Size(618, 485);
            TabTaxCodeDetail.TabIndex = 0;
            TabTaxCodeDetail.Text = "Tax Details";
            TabTaxCodeDetail.UseVisualStyleBackColor = true;
            // 
            // LabelTaxcodeHistorical
            // 
            LabelTaxcodeHistorical.Location = new Point(9, 292);
            LabelTaxcodeHistorical.Name = "LabelTaxcodeHistorical";
            LabelTaxcodeHistorical.Size = new Size(50, 13);
            LabelTaxcodeHistorical.TabIndex = 8;
            LabelTaxcodeHistorical.Text = "Historical";
            // 
            // DataGridViewTaxCodeTaxCodeHistory
            // 
            DataGridViewTaxCodeTaxCodeHistory.AllowUserToAddRows = false;
            DataGridViewTaxCodeTaxCodeHistory.AllowUserToDeleteRows = false;
            DataGridViewTaxCodeTaxCodeHistory.AllowUserToResizeColumns = false;
            DataGridViewTaxCodeTaxCodeHistory.AllowUserToResizeRows = false;
            DataGridViewTaxCodeTaxCodeHistory.BackgroundColor = Color.White;
            DataGridViewTaxCodeTaxCodeHistory.ColumnHeadersHeight = 20;
            DataGridViewTaxCodeTaxCodeHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DataGridViewTaxCodeTaxCodeHistory.Columns.AddRange(new DataGridViewColumn[] { TaxName1, dataGridViewCurrencyColumn1, HEffectiveFrom, HEffectiveTo, Remover, mapids, dataGridViewTextBoxColumn2 });
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Window;
            dataGridViewCellStyle6.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle6.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            DataGridViewTaxCodeTaxCodeHistory.DefaultCellStyle = dataGridViewCellStyle6;
            DataGridViewTaxCodeTaxCodeHistory.EnableHeadersVisualStyles = false;
            DataGridViewTaxCodeTaxCodeHistory.Location = new Point(9, 308);
            DataGridViewTaxCodeTaxCodeHistory.Name = "DataGridViewTaxCodeTaxCodeHistory";
            DataGridViewTaxCodeTaxCodeHistory.ReadOnly = true;
            DataGridViewTaxCodeTaxCodeHistory.RowHeadersVisible = false;
            DataGridViewTaxCodeTaxCodeHistory.RowTemplate.Height = 20;
            DataGridViewTaxCodeTaxCodeHistory.ShowCellToolTips = false;
            DataGridViewTaxCodeTaxCodeHistory.Size = new Size(571, 130);
            DataGridViewTaxCodeTaxCodeHistory.TabIndex = 7;
            DataGridViewTaxCodeTaxCodeHistory.DataError += DataGridViewTaxHistory_DataError;
            // 
            // LabelTaxcodeActive
            // 
            LabelTaxcodeActive.Location = new Point(9, 143);
            LabelTaxcodeActive.Name = "LabelTaxcodeActive";
            LabelTaxcodeActive.Size = new Size(37, 13);
            LabelTaxcodeActive.TabIndex = 6;
            LabelTaxcodeActive.Text = "Active";
            // 
            // DataGridViewTaxCodeCurrentTaxCode
            // 
            DataGridViewTaxCodeCurrentTaxCode.AllowUserToDeleteRows = false;
            DataGridViewTaxCodeCurrentTaxCode.AllowUserToResizeColumns = false;
            DataGridViewTaxCodeCurrentTaxCode.AllowUserToResizeRows = false;
            DataGridViewTaxCodeCurrentTaxCode.BackgroundColor = Color.White;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = SystemColors.Control;
            dataGridViewCellStyle7.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            DataGridViewTaxCodeCurrentTaxCode.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            DataGridViewTaxCodeCurrentTaxCode.ColumnHeadersHeight = 20;
            DataGridViewTaxCodeCurrentTaxCode.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DataGridViewTaxCodeCurrentTaxCode.Columns.AddRange(new DataGridViewColumn[] { TaxName, Percentage, EffectiveFrom, EffectiveTo, Remove, Mapid, dataGridViewTextBoxColumn1 });
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = SystemColors.Window;
            dataGridViewCellStyle10.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle10.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.False;
            DataGridViewTaxCodeCurrentTaxCode.DefaultCellStyle = dataGridViewCellStyle10;
            DataGridViewTaxCodeCurrentTaxCode.EditMode = DataGridViewEditMode.EditOnEnter;
            DataGridViewTaxCodeCurrentTaxCode.EnableHeadersVisualStyles = false;
            DataGridViewTaxCodeCurrentTaxCode.Location = new Point(9, 159);
            DataGridViewTaxCodeCurrentTaxCode.MultiSelect = false;
            DataGridViewTaxCodeCurrentTaxCode.Name = "DataGridViewTaxCodeCurrentTaxCode";
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.True;
            DataGridViewTaxCodeCurrentTaxCode.RowHeadersDefaultCellStyle = dataGridViewCellStyle11;
            DataGridViewTaxCodeCurrentTaxCode.RowHeadersVisible = false;
            dataGridViewCellStyle12.BackColor = Color.White;
            dataGridViewCellStyle12.ForeColor = Color.Black;
            dataGridViewCellStyle12.SelectionBackColor = Color.White;
            dataGridViewCellStyle12.SelectionForeColor = Color.Black;
            DataGridViewTaxCodeCurrentTaxCode.RowsDefaultCellStyle = dataGridViewCellStyle12;
            DataGridViewTaxCodeCurrentTaxCode.RowTemplate.Height = 20;
            DataGridViewTaxCodeCurrentTaxCode.ShowCellToolTips = false;
            DataGridViewTaxCodeCurrentTaxCode.Size = new Size(571, 130);
            DataGridViewTaxCodeCurrentTaxCode.TabIndex = 7;
            DataGridViewTaxCodeCurrentTaxCode.CellClick += DataGridViewCurrentTax_CellClick;
            DataGridViewTaxCodeCurrentTaxCode.CellEnter += DataGridViewCurrentTax_CellEnter;
            DataGridViewTaxCodeCurrentTaxCode.CellFormatting += DataGridViewTaxCodeCurrentTaxCode_CellFormatting;
            DataGridViewTaxCodeCurrentTaxCode.CellLeave += DataGridViewTaxCodeCurrentTaxCode_CellLeave;
            DataGridViewTaxCodeCurrentTaxCode.DataError += DataGridViewCurrentTax_DataError;
            DataGridViewTaxCodeCurrentTaxCode.EditingControlShowing += DataGridViewCurrentTax_EditingControlShowing;
            DataGridViewTaxCodeCurrentTaxCode.RowsAdded += DataGridViewCurrentTax_RowsAdded;
            // 
            // TaxName
            // 
            TaxName.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            TaxName.FlatStyle = FlatStyle.Flat;
            TaxName.HeaderText = "Name";
            TaxName.Name = "TaxName";
            TaxName.Resizable = DataGridViewTriState.False;
            TaxName.Width = 250;
            // 
            // Percentage
            // 
            Percentage.Currencylength = 5;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleRight;
            Percentage.DefaultCellStyle = dataGridViewCellStyle8;
            Percentage.HeaderText = "Percentage";
            Percentage.Name = "Percentage";
            Percentage.Width = 80;
            // 
            // EffectiveFrom
            // 
            EffectiveFrom.FillWeight = 80F;
            EffectiveFrom.HeaderText = "Effective From";
            EffectiveFrom.Name = "EffectiveFrom";
            // 
            // EffectiveTo
            // 
            EffectiveTo.FillWeight = 80F;
            EffectiveTo.HeaderText = "To";
            EffectiveTo.Name = "EffectiveTo";
            EffectiveTo.Resizable = DataGridViewTriState.True;
            // 
            // Remove
            // 
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Remove.DefaultCellStyle = dataGridViewCellStyle9;
            Remove.HeaderText = "...";
            Remove.Name = "Remove";
            Remove.Width = 20;
            // 
            // Mapid
            // 
            Mapid.HeaderText = "Mapid";
            Mapid.Name = "Mapid";
            Mapid.Visible = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Id";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.Visible = false;
            // 
            // TextBoxTaxCodeCode
            // 
            TextBoxTaxCodeCode.BackColor = Color.White;
            TextBoxTaxCodeCode.Location = new Point(9, 31);
            TextBoxTaxCodeCode.MaxLength = 10;
            TextBoxTaxCodeCode.Name = "TextBoxTaxCodeCode";
            TextBoxTaxCodeCode.Size = new Size(571, 21);
            TextBoxTaxCodeCode.TabIndex = 5;
            TextBoxTaxCodeCode.PreviewKeyDown += TextBoxTaxCode_PreviewKeyDown;
            // 
            // TextBoxTaxCodeDetail
            // 
            TextBoxTaxCodeDetail.BackColor = Color.White;
            TextBoxTaxCodeDetail.Location = new Point(9, 71);
            TextBoxTaxCodeDetail.MaxLength = 250;
            TextBoxTaxCodeDetail.Multiline = true;
            TextBoxTaxCodeDetail.Name = "TextBoxTaxCodeDetail";
            TextBoxTaxCodeDetail.Size = new Size(571, 69);
            TextBoxTaxCodeDetail.TabIndex = 6;
            TextBoxTaxCodeDetail.KeyPress += TextBoxTaxCodeDetail_KeyPress;
            TextBoxTaxCodeDetail.PreviewKeyDown += TextBoxTaxCodeDetail_PreviewKeyDown;
            // 
            // LabelTaxCodeDetail
            // 
            LabelTaxCodeDetail.AutoSize = true;
            LabelTaxCodeDetail.Location = new Point(9, 55);
            LabelTaxCodeDetail.Name = "LabelTaxCodeDetail";
            LabelTaxCodeDetail.Size = new Size(39, 13);
            LabelTaxCodeDetail.TabIndex = 1;
            LabelTaxCodeDetail.Text = "Details";
            // 
            // LabelTaxCodeCode
            // 
            LabelTaxCodeCode.AutoSize = true;
            LabelTaxCodeCode.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelTaxCodeCode.Location = new Point(9, 15);
            LabelTaxCodeCode.Name = "LabelTaxCodeCode";
            LabelTaxCodeCode.Size = new Size(59, 13);
            LabelTaxCodeCode.TabIndex = 0;
            LabelTaxCodeCode.Text = "Tax Code";
            // 
            // BtnTaxCodeReport
            // 
            BtnTaxCodeReport.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnTaxCodeReport.Location = new Point(324, 540);
            BtnTaxCodeReport.Name = "BtnTaxCodeReport";
            BtnTaxCodeReport.Size = new Size(110, 23);
            BtnTaxCodeReport.TabIndex = 12;
            BtnTaxCodeReport.Text = "Generate Report";
            BtnTaxCodeReport.UseVisualStyleBackColor = true;
            BtnTaxCodeReport.Click += BtnTaxCodeReport_Click;
            // 
            // BtnTaxCodeImport
            // 
            BtnTaxCodeImport.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnTaxCodeImport.Location = new Point(556, 540);
            BtnTaxCodeImport.Name = "BtnTaxCodeImport";
            BtnTaxCodeImport.Size = new Size(110, 23);
            BtnTaxCodeImport.TabIndex = 10;
            BtnTaxCodeImport.Text = "Import From File";
            BtnTaxCodeImport.UseVisualStyleBackColor = true;
            BtnTaxCodeImport.Click += BtnTaxCodeImport_Click;
            // 
            // TextBoxTaxCodeId
            // 
            TextBoxTaxCodeId.Location = new Point(1006, 442);
            TextBoxTaxCodeId.Name = "TextBoxTaxCodeId";
            TextBoxTaxCodeId.Size = new Size(100, 21);
            TextBoxTaxCodeId.TabIndex = 65;
            TextBoxTaxCodeId.Visible = false;
            // 
            // BtnTaxCodeSave
            // 
            BtnTaxCodeSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnTaxCodeSave.Location = new Point(767, 540);
            BtnTaxCodeSave.Name = "BtnTaxCodeSave";
            BtnTaxCodeSave.Size = new Size(75, 23);
            BtnTaxCodeSave.TabIndex = 8;
            BtnTaxCodeSave.Text = "Save [F8]";
            BtnTaxCodeSave.UseVisualStyleBackColor = true;
            BtnTaxCodeSave.Click += BtnTaxCodeSave_Click;
            BtnTaxCodeSave.PreviewKeyDown += BtnTaxCodeSave_PreviewKeyDown;
            // 
            // BtnTaxCodeCancel
            // 
            BtnTaxCodeCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnTaxCodeCancel.Location = new Point(672, 540);
            BtnTaxCodeCancel.Name = "BtnTaxCodeCancel";
            BtnTaxCodeCancel.Size = new Size(89, 23);
            BtnTaxCodeCancel.TabIndex = 9;
            BtnTaxCodeCancel.Text = "Cancel [Esc]";
            BtnTaxCodeCancel.UseVisualStyleBackColor = true;
            BtnTaxCodeCancel.Click += BtnTaxCodeCancel_Click;
            // 
            // BtnTaxCodeDelete
            // 
            BtnTaxCodeDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnTaxCodeDelete.Location = new Point(100, 540);
            BtnTaxCodeDelete.Name = "BtnTaxCodeDelete";
            BtnTaxCodeDelete.Size = new Size(80, 23);
            BtnTaxCodeDelete.TabIndex = 3;
            BtnTaxCodeDelete.Text = "Delete [F4]";
            BtnTaxCodeDelete.UseVisualStyleBackColor = true;
            BtnTaxCodeDelete.Click += BtnTaxCodeDelete_Click;
            // 
            // BtnTaxCodeExit
            // 
            BtnTaxCodeExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnTaxCodeExit.Location = new Point(848, 540);
            BtnTaxCodeExit.Name = "BtnTaxCodeExit";
            BtnTaxCodeExit.Size = new Size(75, 23);
            BtnTaxCodeExit.TabIndex = 13;
            BtnTaxCodeExit.Text = "Exit [F10]";
            BtnTaxCodeExit.UseVisualStyleBackColor = true;
            BtnTaxCodeExit.Click += BtnTaxCodeExit_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { TaxCodeErrorMsg });
            statusStrip1.Location = new Point(0, 584);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(941, 22);
            statusStrip1.TabIndex = 70;
            statusStrip1.Text = "sdfdsf sdf sdf";
            // 
            // TaxCodeErrorMsg
            // 
            TaxCodeErrorMsg.Name = "TaxCodeErrorMsg";
            TaxCodeErrorMsg.Size = new Size(151, 17);
            TaxCodeErrorMsg.Text = "                                                ";
            // 
            // BtnTaxCodeNew
            // 
            BtnTaxCodeNew.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnTaxCodeNew.Location = new Point(15, 540);
            BtnTaxCodeNew.Name = "BtnTaxCodeNew";
            BtnTaxCodeNew.Size = new Size(80, 23);
            BtnTaxCodeNew.TabIndex = 2;
            BtnTaxCodeNew.Text = "New [F3]";
            BtnTaxCodeNew.UseVisualStyleBackColor = true;
            BtnTaxCodeNew.Click += BtnTaxCodeNew_Click;
            // 
            // BtnTaxCodeExport
            // 
            BtnTaxCodeExport.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnTaxCodeExport.Location = new Point(440, 540);
            BtnTaxCodeExport.Name = "BtnTaxCodeExport";
            BtnTaxCodeExport.Size = new Size(110, 23);
            BtnTaxCodeExport.TabIndex = 11;
            BtnTaxCodeExport.Text = "Export To File";
            BtnTaxCodeExport.UseVisualStyleBackColor = true;
            BtnTaxCodeExport.Click += BtnTaxCodeExport_Click;
            // 
            // BtnTaxCodeEdit
            // 
            BtnTaxCodeEdit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnTaxCodeEdit.Location = new Point(185, 540);
            BtnTaxCodeEdit.Name = "BtnTaxCodeEdit";
            BtnTaxCodeEdit.Size = new Size(80, 23);
            BtnTaxCodeEdit.TabIndex = 4;
            BtnTaxCodeEdit.Text = "Edit [F7]";
            BtnTaxCodeEdit.UseVisualStyleBackColor = true;
            BtnTaxCodeEdit.Click += BtnTaxCodeEdit_Click;
            // 
            // TaxName1
            // 
            TaxName1.HeaderText = "Name";
            TaxName1.Name = "TaxName1";
            TaxName1.ReadOnly = true;
            TaxName1.SortMode = DataGridViewColumnSortMode.NotSortable;
            TaxName1.Width = 250;
            // 
            // dataGridViewCurrencyColumn1
            // 
            dataGridViewCurrencyColumn1.Currencylength = 5;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCurrencyColumn1.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCurrencyColumn1.HeaderText = "Percentage";
            dataGridViewCurrencyColumn1.Name = "dataGridViewCurrencyColumn1";
            dataGridViewCurrencyColumn1.ReadOnly = true;
            dataGridViewCurrencyColumn1.Width = 80;
            // 
            // HEffectiveFrom
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            HEffectiveFrom.DefaultCellStyle = dataGridViewCellStyle4;
            HEffectiveFrom.HeaderText = "Effective From";
            HEffectiveFrom.Name = "HEffectiveFrom";
            HEffectiveFrom.ReadOnly = true;
            // 
            // HEffectiveTo
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            HEffectiveTo.DefaultCellStyle = dataGridViewCellStyle5;
            HEffectiveTo.HeaderText = "To";
            HEffectiveTo.Name = "HEffectiveTo";
            HEffectiveTo.ReadOnly = true;
            HEffectiveTo.Width = 120;
            // 
            // Remover
            // 
            Remover.HeaderText = "---";
            Remover.Name = "Remover";
            Remover.ReadOnly = true;
            Remover.Visible = false;
            // 
            // mapids
            // 
            mapids.HeaderText = "MapId";
            mapids.Name = "mapids";
            mapids.ReadOnly = true;
            mapids.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Id";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            dataGridViewTextBoxColumn2.Visible = false;
            // 
            // FormTaxCode
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(941, 606);
            Controls.Add(BtnTaxCodeEdit);
            Controls.Add(BtnTaxCodeExport);
            Controls.Add(BtnTaxCodeNew);
            Controls.Add(statusStrip1);
            Controls.Add(BtnTaxCodeReport);
            Controls.Add(BtnTaxCodeImport);
            Controls.Add(TextBoxTaxCodeId);
            Controls.Add(BtnTaxCodeSave);
            Controls.Add(BtnTaxCodeCancel);
            Controls.Add(BtnTaxCodeDelete);
            Controls.Add(BtnTaxCodeExit);
            Controls.Add(TabControlTaxCode);
            Controls.Add(TextBoxTaxCodeSearch);
            Controls.Add(GridViewTaxCodeList);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormTaxCode";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Tax Code";
            FormClosing += FormTaxCode_FormClosing;
            Load += FormTaxCode_Load;
            Controls.SetChildIndex(GridViewTaxCodeList, 0);
            Controls.SetChildIndex(TextBoxTaxCodeSearch, 0);
            Controls.SetChildIndex(TabControlTaxCode, 0);
            Controls.SetChildIndex(BtnTaxCodeExit, 0);
            Controls.SetChildIndex(BtnTaxCodeDelete, 0);
            Controls.SetChildIndex(BtnTaxCodeCancel, 0);
            Controls.SetChildIndex(BtnTaxCodeSave, 0);
            Controls.SetChildIndex(TextBoxTaxCodeId, 0);
            Controls.SetChildIndex(BtnTaxCodeImport, 0);
            Controls.SetChildIndex(BtnTaxCodeReport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(BtnTaxCodeNew, 0);
            Controls.SetChildIndex(BtnTaxCodeExport, 0);
            Controls.SetChildIndex(BtnTaxCodeEdit, 0);
            ((System.ComponentModel.ISupportInitialize)GridViewTaxCodeList).EndInit();
            TabControlTaxCode.ResumeLayout(false);
            TabTaxCodeDetail.ResumeLayout(false);
            TabTaxCodeDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DataGridViewTaxCodeTaxCodeHistory).EndInit();
            ((System.ComponentModel.ISupportInitialize)DataGridViewTaxCodeCurrentTaxCode).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private controls.DataViewVerticalScroll GridViewTaxCodeList;
        private controls.text.DelayedTextChangeTextBox TextBoxTaxCodeSearch;
        private TabControl TabControlTaxCode;
        private TabPage TabTaxCodeDetail;
        private Button BtnTaxCodeReport;
        private Button BtnTaxCodeImport;
        private TextBox TextBoxTaxCodeId;
        private Button BtnTaxCodeSave;
        private Button BtnTaxCodeCancel;
        private Button BtnTaxCodeDelete;
        private Button BtnTaxCodeExit;
        private Label LabelTaxCodeCode;
        private Label LabelTaxCodeDetail;
        private TextBox TextBoxTaxCodeCode;
        private TextBox TextBoxTaxCodeDetail;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel TaxCodeErrorMsg;
        private Button BtnTaxCodeNew;
        private Syncfusion.Windows.Forms.Tools.AutoLabel LabelTaxcodeHistorical;
        private controls.DataViewVerticalScroll DataGridViewTaxCodeTaxCodeHistory;
        private Syncfusion.Windows.Forms.Tools.AutoLabel LabelTaxcodeActive;
        private controls.DataViewVerticalScroll DataGridViewTaxCodeCurrentTaxCode;
        private Button BtnTaxCodeExport;
        private Button BtnTaxCodeEdit;
        private DataGridViewTextBoxColumn Code;
        private DataGridViewTextBoxColumn Description;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewComboBoxColumn TaxName;
        private controls.grid.DataGridViewCurrencyColumn Percentage;
        private controls.grid.DataGridViewCalendarColumn EffectiveFrom;
        private controls.grid.DataGridViewCalendarColumn EffectiveTo;
        private DataGridViewTextBoxColumn Remove;
        private DataGridViewTextBoxColumn Mapid;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn TaxName1;
        private controls.grid.DataGridViewCurrencyColumn dataGridViewCurrencyColumn1;
        private controls.grid.DataGridViewCalendarColumn HEffectiveFrom;
        private controls.grid.DataGridViewCalendarColumn HEffectiveTo;
        private DataGridViewTextBoxColumn Remover;
        private DataGridViewTextBoxColumn mapids;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}