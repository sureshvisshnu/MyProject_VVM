namespace fa.views.inventory
{
    partial class FormIntraStockReceive
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIntraStockReceive));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle17 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle14 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle16 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle20 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle18 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle19 = new DataGridViewCellStyle();
            toolStrip1 = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            ComboLocationSelection = new ToolStripComboBox();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripLabel2 = new ToolStripLabel();
            ComboIncomingRefernce = new ToolStripComboBox();
            toolStripSeparator2 = new ToolStripSeparator();
            BtnStockSearch = new ToolStripButton();
            LabelLoctnTo = new Label();
            LabelLoctnFrom = new Label();
            DatetimePickerStockMovementDate = new controls.text.DateWithCalendar();
            LabelTransactnDate = new Label();
            StockMovementReferenceNumber = new Label();
            LabelRefence = new Label();
            GridViewStockMovementItem = new controls.DataViewVerticalScroll();
            Column1 = new DataGridViewTextBoxColumn();
            Account = new DataGridViewTextBoxColumn();
            UnitOfMeasure = new DataGridViewTextBoxColumn();
            Column3 = new controls.grid.DataGridViewQuantityColumn();
            Column4 = new controls.grid.DataGridViewQuantityColumn();
            Column14 = new DataGridViewTextBoxColumn();
            Column15 = new controls.grid.DataGridViewCalendarColumn();
            Column2 = new controls.grid.DataGridViewCurrencyColumn();
            Cost = new controls.grid.DataGridViewCurrencyColumn();
            Column5 = new controls.grid.DataGridViewCurrencyColumn();
            Column10 = new controls.grid.DataGridViewCurrencyColumn();
            Column7 = new controls.grid.DataGridViewCurrencyColumn();
            Column13 = new controls.grid.DataGridViewCurrencyColumn();
            Column6 = new controls.grid.DataGridViewCurrencyColumn();
            Delete = new DataGridViewTextBoxColumn();
            Column11 = new DataGridViewTextBoxColumn();
            Column12 = new DataGridViewCheckBoxColumn();
            PurchDetailID = new DataGridViewTextBoxColumn();
            Column9 = new DataGridViewTextBoxColumn();
            Column16 = new DataGridViewTextBoxColumn();
            Column17 = new DataGridViewTextBoxColumn();
            Column18 = new DataGridViewTextBoxColumn();
            Column19 = new DataGridViewTextBoxColumn();
            Column20 = new DataGridViewTextBoxColumn();
            Column21 = new DataGridViewTextBoxColumn();
            Column22 = new DataGridViewTextBoxColumn();
            BtnStockMovementExit = new Button();
            BtnStockMovementCancel = new Button();
            BtnStockMovementSave = new Button();
            StatusStripPurchase = new StatusStrip();
            ErrorMsgIntrastockReceive = new ToolStripStatusLabel();
            TxtBoxLoctnFrom = new TextBox();
            TxtBoxLoctnTo = new TextBox();
            GridViewStockItemTotal = new DataGridView();
            Total = new DataGridViewTextBoxColumn();
            Value = new controls.grid.DataGridViewQuantityColumn();
            dummy = new DataGridViewTextBoxColumn();
            Column8 = new DataGridViewTextBoxColumn();
            LastStockMovementReferenceNumber = new Label();
            label2 = new Label();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewStockMovementItem).BeginInit();
            StatusStripPurchase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewStockItemTotal).BeginInit();
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
            // toolStrip1
            // 
            toolStrip1.BackColor = SystemColors.ControlLight;
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, ComboLocationSelection, toolStripSeparator1, toolStripLabel2, ComboIncomingRefernce, toolStripSeparator2, BtnStockSearch });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new Padding(5);
            toolStrip1.Size = new Size(874, 33);
            toolStrip1.TabIndex = 107;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(53, 20);
            toolStripLabel1.Text = "Location";
            // 
            // ComboLocationSelection
            // 
            ComboLocationSelection.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboLocationSelection.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboLocationSelection.AutoSize = false;
            ComboLocationSelection.FlatStyle = FlatStyle.Standard;
            ComboLocationSelection.Name = "ComboLocationSelection";
            ComboLocationSelection.Size = new Size(150, 23);
            ComboLocationSelection.SelectedIndexChanged += ComboLocationSelection_SelectedIndexChanged;
            ComboLocationSelection.TextChanged += ComboLocationSelection_SelectedIndexChanged;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 23);
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(90, 20);
            toolStripLabel2.Text = "Incoming Stock";
            // 
            // ComboIncomingRefernce
            // 
            ComboIncomingRefernce.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboIncomingRefernce.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboIncomingRefernce.AutoSize = false;
            ComboIncomingRefernce.FlatStyle = FlatStyle.Standard;
            ComboIncomingRefernce.Name = "ComboIncomingRefernce";
            ComboIncomingRefernce.Size = new Size(150, 23);
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 23);
            // 
            // BtnStockSearch
            // 
            BtnStockSearch.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnStockSearch.Image = (Image)resources.GetObject("BtnStockSearch.Image");
            BtnStockSearch.ImageTransparentColor = Color.Magenta;
            BtnStockSearch.Name = "BtnStockSearch";
            BtnStockSearch.Size = new Size(26, 20);
            BtnStockSearch.Text = "Go";
            BtnStockSearch.Click += BtnStockSearch_Click;
            // 
            // LabelLoctnTo
            // 
            LabelLoctnTo.AutoSize = true;
            LabelLoctnTo.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelLoctnTo.Location = new Point(575, 38);
            LabelLoctnTo.Name = "LabelLoctnTo";
            LabelLoctnTo.Size = new Size(107, 13);
            LabelLoctnTo.TabIndex = 180;
            LabelLoctnTo.Text = "Stock Location To";
            // 
            // LabelLoctnFrom
            // 
            LabelLoctnFrom.AutoSize = true;
            LabelLoctnFrom.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelLoctnFrom.Location = new Point(401, 38);
            LabelLoctnFrom.Name = "LabelLoctnFrom";
            LabelLoctnFrom.Size = new Size(122, 13);
            LabelLoctnFrom.TabIndex = 179;
            LabelLoctnFrom.Text = "Stock Location From";
            // 
            // DatetimePickerStockMovementDate
            // 
            DatetimePickerStockMovementDate.BackColor = Color.White;
            DatetimePickerStockMovementDate.BorderStyle = BorderStyle.FixedSingle;
            DatetimePickerStockMovementDate.Date = null;
            DatetimePickerStockMovementDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DatetimePickerStockMovementDate.Format = "MM/dd/yyyy";
            DatetimePickerStockMovementDate.Location = new Point(304, 53);
            DatetimePickerStockMovementDate.Margin = new Padding(4, 3, 4, 3);
            DatetimePickerStockMovementDate.MaxDate = new DateTime(9997, 12, 31, 0, 17, 7, 0);
            DatetimePickerStockMovementDate.MinDate = new DateTime(1900, 1, 1, 23, 15, 2, 0);
            DatetimePickerStockMovementDate.Name = "DatetimePickerStockMovementDate";
            DatetimePickerStockMovementDate.ReadOnly = true;
            DatetimePickerStockMovementDate.Size = new Size(93, 21);
            DatetimePickerStockMovementDate.TabIndex = 172;
            DatetimePickerStockMovementDate.TabStop = false;
            // 
            // LabelTransactnDate
            // 
            LabelTransactnDate.AutoSize = true;
            LabelTransactnDate.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelTransactnDate.Location = new Point(302, 38);
            LabelTransactnDate.Name = "LabelTransactnDate";
            LabelTransactnDate.Size = new Size(34, 13);
            LabelTransactnDate.TabIndex = 178;
            LabelTransactnDate.Text = "Date";
            // 
            // StockMovementReferenceNumber
            // 
            StockMovementReferenceNumber.AutoSize = true;
            StockMovementReferenceNumber.Font = new Font("Tahoma", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            StockMovementReferenceNumber.Location = new Point(10, 49);
            StockMovementReferenceNumber.Name = "StockMovementReferenceNumber";
            StockMovementReferenceNumber.Size = new Size(114, 25);
            StockMovementReferenceNumber.TabIndex = 177;
            StockMovementReferenceNumber.Text = "SR0000000";
            // 
            // LabelRefence
            // 
            LabelRefence.AutoSize = true;
            LabelRefence.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelRefence.Location = new Point(13, 38);
            LabelRefence.Name = "LabelRefence";
            LabelRefence.Size = new Size(65, 13);
            LabelRefence.TabIndex = 176;
            LabelRefence.Text = "Reference";
            // 
            // GridViewStockMovementItem
            // 
            GridViewStockMovementItem.AllowUserToAddRows = false;
            GridViewStockMovementItem.AllowUserToDeleteRows = false;
            GridViewStockMovementItem.AllowUserToResizeRows = false;
            GridViewStockMovementItem.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewStockMovementItem.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewStockMovementItem.ColumnHeadersHeight = 20;
            GridViewStockMovementItem.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewStockMovementItem.Columns.AddRange(new DataGridViewColumn[] { Column1, Account, UnitOfMeasure, Column3, Column4, Column14, Column15, Column2, Cost, Column5, Column10, Column7, Column13, Column6, Delete, Column11, Column12, PurchDetailID, Column9, Column16, Column17, Column18, Column19, Column20, Column21, Column22 });
            GridViewStockMovementItem.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewStockMovementItem.EnableHeadersVisualStyles = false;
            GridViewStockMovementItem.Location = new Point(12, 83);
            GridViewStockMovementItem.MultiSelect = false;
            GridViewStockMovementItem.Name = "GridViewStockMovementItem";
            GridViewStockMovementItem.ReadOnly = true;
            GridViewStockMovementItem.RowHeadersVisible = false;
            dataGridViewCellStyle17.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle17.SelectionForeColor = SystemColors.ActiveCaptionText;
            GridViewStockMovementItem.RowsDefaultCellStyle = dataGridViewCellStyle17;
            GridViewStockMovementItem.RowTemplate.Height = 20;
            GridViewStockMovementItem.ScrollBars = ScrollBars.Vertical;
            GridViewStockMovementItem.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewStockMovementItem.ShowCellToolTips = false;
            GridViewStockMovementItem.Size = new Size(852, 339);
            GridViewStockMovementItem.TabIndex = 175;
            GridViewStockMovementItem.TabStop = false;
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            Column1.DefaultCellStyle = dataGridViewCellStyle2;
            Column1.HeaderText = "#";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 30;
            // 
            // Account
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            Account.DefaultCellStyle = dataGridViewCellStyle3;
            Account.HeaderText = "Items [ F2 ]";
            Account.MaxInputLength = 35;
            Account.Name = "Account";
            Account.ReadOnly = true;
            Account.Resizable = DataGridViewTriState.False;
            Account.SortMode = DataGridViewColumnSortMode.NotSortable;
            Account.Width = 342;
            // 
            // UnitOfMeasure
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            UnitOfMeasure.DefaultCellStyle = dataGridViewCellStyle4;
            UnitOfMeasure.HeaderText = "UOM";
            UnitOfMeasure.Name = "UnitOfMeasure";
            UnitOfMeasure.ReadOnly = true;
            UnitOfMeasure.Resizable = DataGridViewTriState.False;
            UnitOfMeasure.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle5.NullValue = "0";
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            Column3.DefaultCellStyle = dataGridViewCellStyle5;
            Column3.HeaderText = "Quantity";
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            Column3.Resizable = DataGridViewTriState.False;
            Column3.Width = 75;
            // 
            // Column4
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle6.NullValue = "0";
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            Column4.DefaultCellStyle = dataGridViewCellStyle6;
            Column4.HeaderText = "Free";
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            Column4.Resizable = DataGridViewTriState.False;
            Column4.Width = 75;
            // 
            // Column14
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            Column14.DefaultCellStyle = dataGridViewCellStyle7;
            Column14.HeaderText = "Batch No";
            Column14.MaxInputLength = 10;
            Column14.Name = "Column14";
            Column14.ReadOnly = true;
            Column14.Resizable = DataGridViewTriState.False;
            Column14.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column14.Width = 110;
            // 
            // Column15
            // 
            Column15.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            Column15.DefaultCellStyle = dataGridViewCellStyle8;
            Column15.HeaderText = "Exp Date";
            Column15.Name = "Column15";
            Column15.ReadOnly = true;
            Column15.Resizable = DataGridViewTriState.False;
            // 
            // Column2
            // 
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.NullValue = "0.00";
            Column2.DefaultCellStyle = dataGridViewCellStyle9;
            Column2.HeaderText = "Price";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Resizable = DataGridViewTriState.False;
            Column2.Visible = false;
            Column2.Width = 75;
            // 
            // Cost
            // 
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.NullValue = "0.00";
            Cost.DefaultCellStyle = dataGridViewCellStyle10;
            Cost.HeaderText = "Cost";
            Cost.Name = "Cost";
            Cost.ReadOnly = true;
            Cost.Resizable = DataGridViewTriState.False;
            Cost.Visible = false;
            Cost.Width = 75;
            // 
            // Column5
            // 
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.NullValue = "0.00";
            Column5.DefaultCellStyle = dataGridViewCellStyle11;
            Column5.HeaderText = "Tax%";
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            Column5.Resizable = DataGridViewTriState.False;
            Column5.Visible = false;
            Column5.Width = 50;
            // 
            // Column10
            // 
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle12.NullValue = "0.00";
            Column10.DefaultCellStyle = dataGridViewCellStyle12;
            Column10.FillWeight = 50F;
            Column10.HeaderText = "Tax";
            Column10.Name = "Column10";
            Column10.ReadOnly = true;
            Column10.Resizable = DataGridViewTriState.False;
            Column10.Visible = false;
            Column10.Width = 50;
            // 
            // Column7
            // 
            Column7.Currencylength = 6;
            dataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle13.NullValue = "0.00";
            Column7.DefaultCellStyle = dataGridViewCellStyle13;
            Column7.HeaderText = "Discount%";
            Column7.Name = "Column7";
            Column7.ReadOnly = true;
            Column7.Resizable = DataGridViewTriState.False;
            Column7.Visible = false;
            Column7.Width = 65;
            // 
            // Column13
            // 
            dataGridViewCellStyle14.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle14.NullValue = "0.00";
            Column13.DefaultCellStyle = dataGridViewCellStyle14;
            Column13.HeaderText = "Discount";
            Column13.Name = "Column13";
            Column13.ReadOnly = true;
            Column13.Resizable = DataGridViewTriState.False;
            Column13.Visible = false;
            Column13.Width = 50;
            // 
            // Column6
            // 
            dataGridViewCellStyle15.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle15.NullValue = "0.00";
            Column6.DefaultCellStyle = dataGridViewCellStyle15;
            Column6.HeaderText = "Amount";
            Column6.Name = "Column6";
            Column6.ReadOnly = true;
            Column6.Resizable = DataGridViewTriState.False;
            Column6.Visible = false;
            // 
            // Delete
            // 
            dataGridViewCellStyle16.NullValue = "X";
            Delete.DefaultCellStyle = dataGridViewCellStyle16;
            Delete.HeaderText = "";
            Delete.Name = "Delete";
            Delete.ReadOnly = true;
            Delete.Resizable = DataGridViewTriState.False;
            Delete.SortMode = DataGridViewColumnSortMode.NotSortable;
            Delete.Visible = false;
            Delete.Width = 25;
            // 
            // Column11
            // 
            Column11.HeaderText = "ProductId";
            Column11.Name = "Column11";
            Column11.ReadOnly = true;
            Column11.Resizable = DataGridViewTriState.False;
            Column11.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column11.Visible = false;
            // 
            // Column12
            // 
            Column12.HeaderText = "IsBatch";
            Column12.Name = "Column12";
            Column12.ReadOnly = true;
            Column12.Resizable = DataGridViewTriState.False;
            Column12.Visible = false;
            // 
            // PurchDetailID
            // 
            PurchDetailID.HeaderText = "PurchDetailID";
            PurchDetailID.Name = "PurchDetailID";
            PurchDetailID.ReadOnly = true;
            PurchDetailID.Resizable = DataGridViewTriState.False;
            PurchDetailID.Visible = false;
            // 
            // Column9
            // 
            Column9.HeaderText = "BatchId";
            Column9.Name = "Column9";
            Column9.ReadOnly = true;
            Column9.Visible = false;
            // 
            // Column16
            // 
            Column16.HeaderText = "Retail";
            Column16.Name = "Column16";
            Column16.ReadOnly = true;
            Column16.Visible = false;
            // 
            // Column17
            // 
            Column17.HeaderText = "WholeSale";
            Column17.Name = "Column17";
            Column17.ReadOnly = true;
            Column17.Visible = false;
            // 
            // Column18
            // 
            Column18.HeaderText = "Msrp";
            Column18.Name = "Column18";
            Column18.ReadOnly = true;
            Column18.Visible = false;
            // 
            // Column19
            // 
            Column19.HeaderText = "RUOM";
            Column19.Name = "Column19";
            Column19.ReadOnly = true;
            Column19.Visible = false;
            // 
            // Column20
            // 
            Column20.HeaderText = "RXFACT";
            Column20.Name = "Column20";
            Column20.ReadOnly = true;
            Column20.Visible = false;
            // 
            // Column21
            // 
            Column21.HeaderText = "WUOM";
            Column21.Name = "Column21";
            Column21.ReadOnly = true;
            Column21.Visible = false;
            // 
            // Column22
            // 
            Column22.HeaderText = "WXFACT";
            Column22.Name = "Column22";
            Column22.ReadOnly = true;
            Column22.Visible = false;
            // 
            // BtnStockMovementExit
            // 
            BtnStockMovementExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnStockMovementExit.Location = new Point(763, 457);
            BtnStockMovementExit.Name = "BtnStockMovementExit";
            BtnStockMovementExit.Size = new Size(75, 23);
            BtnStockMovementExit.TabIndex = 185;
            BtnStockMovementExit.Text = "Exit [F10]";
            BtnStockMovementExit.UseVisualStyleBackColor = true;
            BtnStockMovementExit.Click += BtnStockMovementExit_Click;
            // 
            // BtnStockMovementCancel
            // 
            BtnStockMovementCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnStockMovementCancel.Location = new Point(579, 457);
            BtnStockMovementCancel.Name = "BtnStockMovementCancel";
            BtnStockMovementCancel.Size = new Size(83, 23);
            BtnStockMovementCancel.TabIndex = 184;
            BtnStockMovementCancel.Text = "Cancel [Esc]";
            BtnStockMovementCancel.UseVisualStyleBackColor = true;
            BtnStockMovementCancel.Click += BtnStockMovementCancel_Click;
            // 
            // BtnStockMovementSave
            // 
            BtnStockMovementSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnStockMovementSave.Location = new Point(667, 457);
            BtnStockMovementSave.Name = "BtnStockMovementSave";
            BtnStockMovementSave.Size = new Size(90, 23);
            BtnStockMovementSave.TabIndex = 183;
            BtnStockMovementSave.Text = "Receive [F8]";
            BtnStockMovementSave.UseVisualStyleBackColor = true;
            BtnStockMovementSave.Click += BtnStockMovementSave_Click;
            BtnStockMovementSave.PreviewKeyDown += BtnStockMovementSave_PreviewKeyDown;
            // 
            // StatusStripPurchase
            // 
            StatusStripPurchase.Items.AddRange(new ToolStripItem[] { ErrorMsgIntrastockReceive });
            StatusStripPurchase.Location = new Point(0, 492);
            StatusStripPurchase.Name = "StatusStripPurchase";
            StatusStripPurchase.Size = new Size(874, 22);
            StatusStripPurchase.TabIndex = 186;
            StatusStripPurchase.Text = "statusStrip1";
            // 
            // ErrorMsgIntrastockReceive
            // 
            ErrorMsgIntrastockReceive.Name = "ErrorMsgIntrastockReceive";
            ErrorMsgIntrastockReceive.Size = new Size(94, 17);
            ErrorMsgIntrastockReceive.Text = "                             ";
            // 
            // TxtBoxLoctnFrom
            // 
            TxtBoxLoctnFrom.BackColor = Color.White;
            TxtBoxLoctnFrom.Location = new Point(404, 53);
            TxtBoxLoctnFrom.Name = "TxtBoxLoctnFrom";
            TxtBoxLoctnFrom.ReadOnly = true;
            TxtBoxLoctnFrom.Size = new Size(168, 21);
            TxtBoxLoctnFrom.TabIndex = 187;
            TxtBoxLoctnFrom.TabStop = false;
            // 
            // TxtBoxLoctnTo
            // 
            TxtBoxLoctnTo.BackColor = Color.White;
            TxtBoxLoctnTo.Location = new Point(578, 53);
            TxtBoxLoctnTo.Name = "TxtBoxLoctnTo";
            TxtBoxLoctnTo.ReadOnly = true;
            TxtBoxLoctnTo.Size = new Size(168, 21);
            TxtBoxLoctnTo.TabIndex = 188;
            TxtBoxLoctnTo.TabStop = false;
            // 
            // GridViewStockItemTotal
            // 
            GridViewStockItemTotal.AllowUserToAddRows = false;
            GridViewStockItemTotal.AllowUserToDeleteRows = false;
            GridViewStockItemTotal.AllowUserToResizeColumns = false;
            GridViewStockItemTotal.AllowUserToResizeRows = false;
            GridViewStockItemTotal.BackgroundColor = SystemColors.ButtonFace;
            GridViewStockItemTotal.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GridViewStockItemTotal.ColumnHeadersVisible = false;
            GridViewStockItemTotal.Columns.AddRange(new DataGridViewColumn[] { Total, Value, dummy, Column8 });
            GridViewStockItemTotal.Enabled = false;
            GridViewStockItemTotal.Location = new Point(12, 421);
            GridViewStockItemTotal.Name = "GridViewStockItemTotal";
            GridViewStockItemTotal.ReadOnly = true;
            GridViewStockItemTotal.RowHeadersVisible = false;
            dataGridViewCellStyle20.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle20.BackColor = SystemColors.Control;
            dataGridViewCellStyle20.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle20.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle20.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle20.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle20.WrapMode = DataGridViewTriState.True;
            GridViewStockItemTotal.RowsDefaultCellStyle = dataGridViewCellStyle20;
            GridViewStockItemTotal.Size = new Size(852, 24);
            GridViewStockItemTotal.TabIndex = 189;
            GridViewStockItemTotal.TabStop = false;
            // 
            // Total
            // 
            dataGridViewCellStyle18.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle18.BackColor = Color.White;
            dataGridViewCellStyle18.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            dataGridViewCellStyle18.ForeColor = Color.Black;
            dataGridViewCellStyle18.NullValue = "Total :";
            dataGridViewCellStyle18.SelectionBackColor = Color.White;
            dataGridViewCellStyle18.SelectionForeColor = Color.Black;
            Total.DefaultCellStyle = dataGridViewCellStyle18;
            Total.HeaderText = "Total";
            Total.Name = "Total";
            Total.ReadOnly = true;
            Total.Width = 472;
            // 
            // Value
            // 
            dataGridViewCellStyle19.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle19.BackColor = Color.White;
            dataGridViewCellStyle19.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            dataGridViewCellStyle19.ForeColor = Color.Black;
            dataGridViewCellStyle19.NullValue = "0.00";
            dataGridViewCellStyle19.SelectionBackColor = Color.White;
            dataGridViewCellStyle19.SelectionForeColor = Color.Black;
            Value.DefaultCellStyle = dataGridViewCellStyle19;
            Value.HeaderText = "Value";
            Value.Name = "Value";
            Value.ReadOnly = true;
            Value.Resizable = DataGridViewTriState.True;
            Value.SortMode = DataGridViewColumnSortMode.Automatic;
            Value.Width = 75;
            // 
            // dummy
            // 
            dummy.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dummy.HeaderText = "dummy";
            dummy.Name = "dummy";
            dummy.ReadOnly = true;
            // 
            // Column8
            // 
            Column8.HeaderText = "";
            Column8.Name = "Column8";
            Column8.ReadOnly = true;
            Column8.Resizable = DataGridViewTriState.True;
            Column8.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column8.Visible = false;
            Column8.Width = 25;
            // 
            // LastStockMovementReferenceNumber
            // 
            LastStockMovementReferenceNumber.AutoSize = true;
            LastStockMovementReferenceNumber.Font = new Font("Tahoma", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            LastStockMovementReferenceNumber.Location = new Point(172, 49);
            LastStockMovementReferenceNumber.Name = "LastStockMovementReferenceNumber";
            LastStockMovementReferenceNumber.Size = new Size(114, 25);
            LastStockMovementReferenceNumber.TabIndex = 237;
            LastStockMovementReferenceNumber.Text = "SR0000000";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(174, 34);
            label2.Name = "label2";
            label2.Size = new Size(92, 13);
            label2.TabIndex = 236;
            label2.Text = "Last Reference";
            // 
            // FormIntraStockReceive
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(874, 514);
            Controls.Add(LastStockMovementReferenceNumber);
            Controls.Add(label2);
            Controls.Add(GridViewStockItemTotal);
            Controls.Add(TxtBoxLoctnTo);
            Controls.Add(TxtBoxLoctnFrom);
            Controls.Add(StatusStripPurchase);
            Controls.Add(BtnStockMovementExit);
            Controls.Add(BtnStockMovementCancel);
            Controls.Add(BtnStockMovementSave);
            Controls.Add(LabelLoctnTo);
            Controls.Add(LabelLoctnFrom);
            Controls.Add(DatetimePickerStockMovementDate);
            Controls.Add(LabelTransactnDate);
            Controls.Add(StockMovementReferenceNumber);
            Controls.Add(LabelRefence);
            Controls.Add(GridViewStockMovementItem);
            Controls.Add(toolStrip1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormIntraStockReceive";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Intra Stock Receive";
            Load += FormIntraStockReceive_Load;
            Controls.SetChildIndex(toolStrip1, 0);
            Controls.SetChildIndex(GridViewStockMovementItem, 0);
            Controls.SetChildIndex(LabelRefence, 0);
            Controls.SetChildIndex(StockMovementReferenceNumber, 0);
            Controls.SetChildIndex(LabelTransactnDate, 0);
            Controls.SetChildIndex(DatetimePickerStockMovementDate, 0);
            Controls.SetChildIndex(LabelLoctnFrom, 0);
            Controls.SetChildIndex(LabelLoctnTo, 0);
            Controls.SetChildIndex(BtnStockMovementSave, 0);
            Controls.SetChildIndex(BtnStockMovementCancel, 0);
            Controls.SetChildIndex(BtnStockMovementExit, 0);
            Controls.SetChildIndex(StatusStripPurchase, 0);
            Controls.SetChildIndex(TxtBoxLoctnFrom, 0);
            Controls.SetChildIndex(TxtBoxLoctnTo, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(GridViewStockItemTotal, 0);
            Controls.SetChildIndex(label2, 0);
            Controls.SetChildIndex(LastStockMovementReferenceNumber, 0);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewStockMovementItem).EndInit();
            StatusStripPurchase.ResumeLayout(false);
            StatusStripPurchase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewStockItemTotal).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripButton BtnStockSearch;
        private ToolStripLabel toolStripLabel2;
        private ToolStripComboBox ComboIncomingRefernce;
        private Label LabelLoctnTo;
        private Label LabelLoctnFrom;
        private controls.text.DateWithCalendar DatetimePickerStockMovementDate;
        private Label LabelTransactnDate;
        private Label StockMovementReferenceNumber;
        private Label LabelRefence;
        private controls.DataViewVerticalScroll GridViewStockMovementItem;
        private Button BtnStockMovementExit;
        private Button BtnStockMovementCancel;
        private Button BtnStockMovementSave;
        private StatusStrip StatusStripPurchase;
        private ToolStripStatusLabel ErrorMsgIntrastockReceive;
        private TextBox TxtBoxLoctnFrom;
        private TextBox TxtBoxLoctnTo;
        private ToolStripComboBox ComboLocationSelection;
        private DataGridView GridViewStockItemTotal;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Account;
        private DataGridViewTextBoxColumn UnitOfMeasure;
        private controls.grid.DataGridViewQuantityColumn Column3;
        private controls.grid.DataGridViewQuantityColumn Column4;
        private DataGridViewTextBoxColumn Column14;
        private controls.grid.DataGridViewCalendarColumn Column15;
        private controls.grid.DataGridViewCurrencyColumn Column2;
        private controls.grid.DataGridViewCurrencyColumn Cost;
        private controls.grid.DataGridViewCurrencyColumn Column5;
        private controls.grid.DataGridViewCurrencyColumn Column10;
        private controls.grid.DataGridViewCurrencyColumn Column7;
        private controls.grid.DataGridViewCurrencyColumn Column13;
        private controls.grid.DataGridViewCurrencyColumn Column6;
        private DataGridViewTextBoxColumn Delete;
        private DataGridViewTextBoxColumn Column11;
        private DataGridViewCheckBoxColumn Column12;
        private DataGridViewTextBoxColumn PurchDetailID;
        private DataGridViewTextBoxColumn Column9;
        private DataGridViewTextBoxColumn Column16;
        private DataGridViewTextBoxColumn Column17;
        private DataGridViewTextBoxColumn Column18;
        private DataGridViewTextBoxColumn Column19;
        private DataGridViewTextBoxColumn Column20;
        private DataGridViewTextBoxColumn Column21;
        private DataGridViewTextBoxColumn Column22;
        private DataGridViewTextBoxColumn Total;
        private controls.grid.DataGridViewQuantityColumn Value;
        private DataGridViewTextBoxColumn dummy;
        private DataGridViewTextBoxColumn Column8;
        private Label LastStockMovementReferenceNumber;
        private Label label2;
    }
}