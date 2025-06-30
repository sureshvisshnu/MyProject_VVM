
namespace fa.views.inventory
{
    partial class FormIntraStockRequest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIntraStockRequest));
            DataGridViewCellStyle dataGridViewCellStyle43 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle46 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle44 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle45 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle47 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle63 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle48 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle49 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle50 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle51 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle52 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle53 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle54 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle55 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle56 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle57 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle58 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle59 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle60 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle61 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle62 = new DataGridViewCellStyle();
            toolStrip1 = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            TextBoxStockMovementSearch = new ToolStripTextBox();
            toolStripSeparator1 = new ToolStripSeparator();
            BtnStockSearch = new ToolStripButton();
            ComboBoxFromLocation = new controls.ComboBoxSwapTextBox();
            ComboBoxToLocation = new controls.ComboBoxSwapTextBox();
            label2 = new Label();
            label12 = new Label();
            DatetimePickerStockMovementDate = new controls.text.DateWithCalendar();
            label4 = new Label();
            StockMovementReferenceNumber = new Label();
            label1 = new Label();
            GridViewStockItemTotal = new DataGridView();
            Total = new DataGridViewTextBoxColumn();
            Value = new controls.grid.DataGridViewQuantityColumn();
            dummy = new DataGridViewTextBoxColumn();
            Column8 = new DataGridViewTextBoxColumn();
            GridViewStockMovementItem = new controls.DataViewVerticalScroll();
            Column1 = new DataGridViewTextBoxColumn();
            Account = new DataGridViewTextBoxColumn();
            UnitOfMeasure = new DataGridViewComboBoxColumn();
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
            Delete = new DataGridViewButtonColumn();
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
            StockMovementProductDetails = new controls.text.ProductDetails();
            StatusStripPurchase = new StatusStrip();
            ToolStripStatusLabelErrorPurchase = new ToolStripStatusLabel();
            BtnStockMovementExit = new Button();
            BtnStockMovementPrint = new Button();
            BtnStockMovementCancel = new Button();
            BtnStockMovementSave = new Button();
            BtnStockMovementDelete = new Button();
            BtnStockMovementNew = new Button();
            TextBoxStockMovementId = new TextBox();
            TimerStock = new System.Windows.Forms.Timer(components);
            BtnStockRequestComplete = new Button();
            LastStockMovementReferenceNumber = new Label();
            label3 = new Label();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewStockItemTotal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)GridViewStockMovementItem).BeginInit();
            StatusStripPurchase.SuspendLayout();
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
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, TextBoxStockMovementSearch, toolStripSeparator1, BtnStockSearch });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new Padding(5);
            toolStrip1.Size = new Size(1103, 33);
            toolStrip1.TabIndex = 107;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(42, 20);
            toolStripLabel1.Text = "Search";
            // 
            // TextBoxStockMovementSearch
            // 
            TextBoxStockMovementSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxStockMovementSearch.MaxLength = 30;
            TextBoxStockMovementSearch.Name = "TextBoxStockMovementSearch";
            TextBoxStockMovementSearch.Size = new Size(200, 23);
            TextBoxStockMovementSearch.KeyDown += TextBoxStockMovementSearch_KeyDown;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 23);
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
            // ComboBoxFromLocation
            // 
            ComboBoxFromLocation.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxFromLocation.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxFromLocation.FormattingEnabled = true;
            ComboBoxFromLocation.Location = new Point(609, 60);
            ComboBoxFromLocation.Name = "ComboBoxFromLocation";
            ComboBoxFromLocation.Size = new Size(204, 21);
            ComboBoxFromLocation.TabIndex = 173;
            ComboBoxFromLocation.TxtVisible = true;
            ComboBoxFromLocation.SelectedIndexChanged += ComboBoxFromLocation_SelectedIndexChanged;
            ComboBoxFromLocation.TextChanged += ComboBoxFromLocation_TextChanged;
            ComboBoxFromLocation.Enter += ComboBoxFromLocation_Enter;
            ComboBoxFromLocation.PreviewKeyDown += ComboBoxFromLocation_PreviewKeyDown;
            // 
            // ComboBoxToLocation
            // 
            ComboBoxToLocation.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxToLocation.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxToLocation.FormattingEnabled = true;
            ComboBoxToLocation.Location = new Point(398, 60);
            ComboBoxToLocation.Name = "ComboBoxToLocation";
            ComboBoxToLocation.Size = new Size(204, 21);
            ComboBoxToLocation.TabIndex = 174;
            ComboBoxToLocation.TxtVisible = true;
            ComboBoxToLocation.SelectedIndexChanged += ComboBoxToLocation_SelectedIndexChanged;
            ComboBoxToLocation.TextChanged += ComboBoxToLocation_TextChanged;
            ComboBoxToLocation.PreviewKeyDown += ComboBoxToLocation_PreviewKeyDown;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(609, 44);
            label2.Name = "label2";
            label2.Size = new Size(90, 13);
            label2.TabIndex = 179;
            label2.Text = "Stock Location";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label12.Location = new Point(398, 44);
            label12.Name = "label12";
            label12.Size = new Size(140, 13);
            label12.TabIndex = 178;
            label12.Text = "Stock Request Location";
            // 
            // DatetimePickerStockMovementDate
            // 
            DatetimePickerStockMovementDate.BackColor = Color.White;
            DatetimePickerStockMovementDate.BorderStyle = BorderStyle.FixedSingle;
            DatetimePickerStockMovementDate.Date = null;
            DatetimePickerStockMovementDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DatetimePickerStockMovementDate.Format = "MM/dd/yyyy";
            DatetimePickerStockMovementDate.Location = new Point(301, 59);
            DatetimePickerStockMovementDate.Margin = new Padding(4, 3, 4, 3);
            DatetimePickerStockMovementDate.MaxDate = new DateTime(9997, 12, 31, 8, 13, 39, 0);
            DatetimePickerStockMovementDate.MinDate = new DateTime(1900, 1, 1, 23, 43, 31, 0);
            DatetimePickerStockMovementDate.Name = "DatetimePickerStockMovementDate";
            DatetimePickerStockMovementDate.ReadOnly = false;
            DatetimePickerStockMovementDate.Size = new Size(93, 21);
            DatetimePickerStockMovementDate.TabIndex = 172;
            DatetimePickerStockMovementDate.PreviewKeyDown += DatetimePickerStockMovementDate_PreviewKeyDown;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(300, 43);
            label4.Name = "label4";
            label4.Size = new Size(34, 13);
            label4.TabIndex = 177;
            label4.Text = "Date";
            // 
            // StockMovementReferenceNumber
            // 
            StockMovementReferenceNumber.AutoSize = true;
            StockMovementReferenceNumber.Font = new Font("Tahoma", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            StockMovementReferenceNumber.Location = new Point(10, 56);
            StockMovementReferenceNumber.Name = "StockMovementReferenceNumber";
            StockMovementReferenceNumber.Size = new Size(114, 25);
            StockMovementReferenceNumber.TabIndex = 176;
            StockMovementReferenceNumber.Text = "SR0000000";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(12, 43);
            label1.Name = "label1";
            label1.Size = new Size(65, 13);
            label1.TabIndex = 175;
            label1.Text = "Reference";
            // 
            // GridViewStockItemTotal
            // 
            GridViewStockItemTotal.AllowUserToAddRows = false;
            GridViewStockItemTotal.AllowUserToDeleteRows = false;
            GridViewStockItemTotal.AllowUserToResizeColumns = false;
            GridViewStockItemTotal.AllowUserToResizeRows = false;
            GridViewStockItemTotal.BackgroundColor = SystemColors.ButtonFace;
            dataGridViewCellStyle43.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle43.BackColor = SystemColors.Control;
            dataGridViewCellStyle43.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle43.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle43.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle43.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle43.WrapMode = DataGridViewTriState.True;
            GridViewStockItemTotal.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle43;
            GridViewStockItemTotal.ColumnHeadersHeight = 20;
            GridViewStockItemTotal.ColumnHeadersVisible = false;
            GridViewStockItemTotal.Columns.AddRange(new DataGridViewColumn[] { Total, Value, dummy, Column8 });
            GridViewStockItemTotal.Enabled = false;
            GridViewStockItemTotal.Location = new Point(15, 493);
            GridViewStockItemTotal.Name = "GridViewStockItemTotal";
            GridViewStockItemTotal.ReadOnly = true;
            GridViewStockItemTotal.RowHeadersVisible = false;
            dataGridViewCellStyle46.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle46.BackColor = SystemColors.Control;
            dataGridViewCellStyle46.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle46.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle46.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle46.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle46.WrapMode = DataGridViewTriState.True;
            GridViewStockItemTotal.RowsDefaultCellStyle = dataGridViewCellStyle46;
            GridViewStockItemTotal.Size = new Size(804, 25);
            GridViewStockItemTotal.TabIndex = 183;
            GridViewStockItemTotal.TabStop = false;
            // 
            // Total
            // 
            dataGridViewCellStyle44.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle44.BackColor = Color.White;
            dataGridViewCellStyle44.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            dataGridViewCellStyle44.ForeColor = Color.Black;
            dataGridViewCellStyle44.NullValue = "Total :";
            dataGridViewCellStyle44.SelectionBackColor = Color.White;
            dataGridViewCellStyle44.SelectionForeColor = Color.Black;
            Total.DefaultCellStyle = dataGridViewCellStyle44;
            Total.HeaderText = "Total";
            Total.Name = "Total";
            Total.ReadOnly = true;
            Total.Width = 474;
            // 
            // Value
            // 
            dataGridViewCellStyle45.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle45.BackColor = Color.White;
            dataGridViewCellStyle45.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            dataGridViewCellStyle45.ForeColor = Color.Black;
            dataGridViewCellStyle45.NullValue = "0.00";
            dataGridViewCellStyle45.SelectionBackColor = Color.White;
            dataGridViewCellStyle45.SelectionForeColor = Color.Black;
            Value.DefaultCellStyle = dataGridViewCellStyle45;
            Value.HeaderText = "Value";
            Value.Name = "Value";
            Value.ReadOnly = true;
            Value.Resizable = DataGridViewTriState.True;
            Value.SortMode = DataGridViewColumnSortMode.Automatic;
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
            // GridViewStockMovementItem
            // 
            GridViewStockMovementItem.AllowUserToAddRows = false;
            GridViewStockMovementItem.AllowUserToDeleteRows = false;
            GridViewStockMovementItem.AllowUserToResizeRows = false;
            GridViewStockMovementItem.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle47.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle47.BackColor = SystemColors.Control;
            dataGridViewCellStyle47.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle47.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle47.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle47.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle47.WrapMode = DataGridViewTriState.True;
            GridViewStockMovementItem.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle47;
            GridViewStockMovementItem.ColumnHeadersHeight = 20;
            GridViewStockMovementItem.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewStockMovementItem.Columns.AddRange(new DataGridViewColumn[] { Column1, Account, UnitOfMeasure, Column3, Column4, Column14, Column15, Column2, Cost, Column5, Column10, Column7, Column13, Column6, Delete, Column11, Column12, PurchDetailID, Column9, Column16, Column17, Column18, Column19, Column20, Column21, Column22 });
            GridViewStockMovementItem.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewStockMovementItem.EnableHeadersVisualStyles = false;
            GridViewStockMovementItem.Location = new Point(15, 86);
            GridViewStockMovementItem.MultiSelect = false;
            GridViewStockMovementItem.Name = "GridViewStockMovementItem";
            GridViewStockMovementItem.RowHeadersVisible = false;
            dataGridViewCellStyle63.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle63.SelectionForeColor = SystemColors.ActiveCaptionText;
            GridViewStockMovementItem.RowsDefaultCellStyle = dataGridViewCellStyle63;
            GridViewStockMovementItem.RowTemplate.Height = 20;
            GridViewStockMovementItem.ScrollBars = ScrollBars.Vertical;
            GridViewStockMovementItem.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewStockMovementItem.ShowCellToolTips = false;
            GridViewStockMovementItem.Size = new Size(804, 408);
            GridViewStockMovementItem.TabIndex = 182;
            GridViewStockMovementItem.CellClick += GridViewStockMovementItem_CellClick;
            GridViewStockMovementItem.CellEndEdit += GridViewStockMovementItem_CellEndEdit;
            GridViewStockMovementItem.CellEnter += GridViewStockMovementItem_CellEnter;
            GridViewStockMovementItem.DataError += GridViewStockMovementItem_DataError;
            GridViewStockMovementItem.EditingControlShowing += GridViewStockMovementItem_EditingControlShowing;
            GridViewStockMovementItem.RowEnter += GridViewStockMovementItem_RowEnter;
            GridViewStockMovementItem.RowsAdded += GridViewStockMovementItem_RowsAdded;
            // 
            // Column1
            // 
            dataGridViewCellStyle48.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle48.WrapMode = DataGridViewTriState.True;
            Column1.DefaultCellStyle = dataGridViewCellStyle48;
            Column1.HeaderText = "#";
            Column1.Name = "Column1";
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 30;
            // 
            // Account
            // 
            dataGridViewCellStyle49.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle49.WrapMode = DataGridViewTriState.True;
            Account.DefaultCellStyle = dataGridViewCellStyle49;
            Account.HeaderText = "Items [ F2 ]";
            Account.MaxInputLength = 35;
            Account.Name = "Account";
            Account.Resizable = DataGridViewTriState.False;
            Account.SortMode = DataGridViewColumnSortMode.NotSortable;
            Account.Width = 314;
            // 
            // UnitOfMeasure
            // 
            dataGridViewCellStyle50.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle50.WrapMode = DataGridViewTriState.True;
            UnitOfMeasure.DefaultCellStyle = dataGridViewCellStyle50;
            UnitOfMeasure.FlatStyle = FlatStyle.Flat;
            UnitOfMeasure.HeaderText = "UOM";
            UnitOfMeasure.Name = "UnitOfMeasure";
            UnitOfMeasure.Resizable = DataGridViewTriState.False;
            UnitOfMeasure.Width = 130;
            // 
            // Column3
            // 
            dataGridViewCellStyle51.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle51.NullValue = "0";
            dataGridViewCellStyle51.WrapMode = DataGridViewTriState.True;
            Column3.DefaultCellStyle = dataGridViewCellStyle51;
            Column3.HeaderText = "Quantity";
            Column3.Name = "Column3";
            Column3.Resizable = DataGridViewTriState.False;
            // 
            // Column4
            // 
            dataGridViewCellStyle52.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle52.NullValue = "0";
            dataGridViewCellStyle52.WrapMode = DataGridViewTriState.True;
            Column4.DefaultCellStyle = dataGridViewCellStyle52;
            Column4.HeaderText = "Free";
            Column4.Name = "Column4";
            Column4.Resizable = DataGridViewTriState.False;
            Column4.Visible = false;
            // 
            // Column14
            // 
            dataGridViewCellStyle53.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle53.WrapMode = DataGridViewTriState.True;
            Column14.DefaultCellStyle = dataGridViewCellStyle53;
            Column14.HeaderText = "Batch No";
            Column14.MaxInputLength = 10;
            Column14.Name = "Column14";
            Column14.Resizable = DataGridViewTriState.False;
            Column14.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column14.Width = 95;
            // 
            // Column15
            // 
            Column15.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle54.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle54.WrapMode = DataGridViewTriState.True;
            Column15.DefaultCellStyle = dataGridViewCellStyle54;
            Column15.HeaderText = "Exp Date";
            Column15.Name = "Column15";
            Column15.Resizable = DataGridViewTriState.False;
            // 
            // Column2
            // 
            dataGridViewCellStyle55.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle55.NullValue = "0.00";
            Column2.DefaultCellStyle = dataGridViewCellStyle55;
            Column2.HeaderText = "Price";
            Column2.Name = "Column2";
            Column2.Resizable = DataGridViewTriState.False;
            Column2.Visible = false;
            Column2.Width = 75;
            // 
            // Cost
            // 
            dataGridViewCellStyle56.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle56.NullValue = "0.00";
            Cost.DefaultCellStyle = dataGridViewCellStyle56;
            Cost.HeaderText = "Cost";
            Cost.Name = "Cost";
            Cost.Resizable = DataGridViewTriState.False;
            Cost.Visible = false;
            Cost.Width = 75;
            // 
            // Column5
            // 
            dataGridViewCellStyle57.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle57.NullValue = "0.00";
            Column5.DefaultCellStyle = dataGridViewCellStyle57;
            Column5.HeaderText = "Tax%";
            Column5.Name = "Column5";
            Column5.Resizable = DataGridViewTriState.False;
            Column5.Visible = false;
            Column5.Width = 50;
            // 
            // Column10
            // 
            dataGridViewCellStyle58.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle58.NullValue = "0.00";
            Column10.DefaultCellStyle = dataGridViewCellStyle58;
            Column10.FillWeight = 50F;
            Column10.HeaderText = "Tax";
            Column10.Name = "Column10";
            Column10.Resizable = DataGridViewTriState.False;
            Column10.Visible = false;
            Column10.Width = 50;
            // 
            // Column7
            // 
            Column7.Currencylength = 6;
            dataGridViewCellStyle59.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle59.NullValue = "0.00";
            Column7.DefaultCellStyle = dataGridViewCellStyle59;
            Column7.HeaderText = "Discount%";
            Column7.Name = "Column7";
            Column7.Resizable = DataGridViewTriState.False;
            Column7.Visible = false;
            Column7.Width = 65;
            // 
            // Column13
            // 
            dataGridViewCellStyle60.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle60.NullValue = "0.00";
            Column13.DefaultCellStyle = dataGridViewCellStyle60;
            Column13.HeaderText = "Discount";
            Column13.Name = "Column13";
            Column13.Resizable = DataGridViewTriState.False;
            Column13.Visible = false;
            Column13.Width = 50;
            // 
            // Column6
            // 
            dataGridViewCellStyle61.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle61.NullValue = "0.00";
            Column6.DefaultCellStyle = dataGridViewCellStyle61;
            Column6.HeaderText = "Amount";
            Column6.Name = "Column6";
            Column6.Resizable = DataGridViewTriState.False;
            Column6.Visible = false;
            // 
            // Delete
            // 
            dataGridViewCellStyle62.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle62.NullValue = "X";
            Delete.DefaultCellStyle = dataGridViewCellStyle62;
            Delete.HeaderText = "";
            Delete.Name = "Delete";
            Delete.Resizable = DataGridViewTriState.False;
            Delete.Width = 25;
            // 
            // Column11
            // 
            Column11.HeaderText = "ProductId";
            Column11.Name = "Column11";
            Column11.Resizable = DataGridViewTriState.False;
            Column11.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column11.Visible = false;
            // 
            // Column12
            // 
            Column12.HeaderText = "IsBatch";
            Column12.Name = "Column12";
            Column12.Resizable = DataGridViewTriState.False;
            Column12.Visible = false;
            // 
            // PurchDetailID
            // 
            PurchDetailID.HeaderText = "PurchDetailID";
            PurchDetailID.Name = "PurchDetailID";
            PurchDetailID.Resizable = DataGridViewTriState.False;
            PurchDetailID.Visible = false;
            // 
            // Column9
            // 
            Column9.HeaderText = "BatchId";
            Column9.Name = "Column9";
            Column9.Visible = false;
            // 
            // Column16
            // 
            Column16.HeaderText = "Retail";
            Column16.Name = "Column16";
            Column16.Visible = false;
            // 
            // Column17
            // 
            Column17.HeaderText = "WholeSale";
            Column17.Name = "Column17";
            Column17.Visible = false;
            // 
            // Column18
            // 
            Column18.HeaderText = "Msrp";
            Column18.Name = "Column18";
            Column18.Visible = false;
            // 
            // Column19
            // 
            Column19.HeaderText = "RUOM";
            Column19.Name = "Column19";
            Column19.Visible = false;
            // 
            // Column20
            // 
            Column20.HeaderText = "RXFACT";
            Column20.Name = "Column20";
            Column20.Visible = false;
            // 
            // Column21
            // 
            Column21.HeaderText = "WUOM";
            Column21.Name = "Column21";
            Column21.Visible = false;
            // 
            // Column22
            // 
            Column22.HeaderText = "WXFACT";
            Column22.Name = "Column22";
            Column22.Visible = false;
            // 
            // StockMovementProductDetails
            // 
            StockMovementProductDetails.BatchId = 0L;
            StockMovementProductDetails.CurrentDate = null;
            StockMovementProductDetails.EditableStock = 0D;
            StockMovementProductDetails.Location = new Point(825, 42);
            StockMovementProductDetails.LocationId = 0L;
            StockMovementProductDetails.Margin = new Padding(4, 3, 4, 3);
            StockMovementProductDetails.Name = "StockMovementProductDetails";
            StockMovementProductDetails.ProductId = 0L;
            StockMovementProductDetails.Size = new Size(273, 513);
            StockMovementProductDetails.TabIndex = 184;
            // 
            // StatusStripPurchase
            // 
            StatusStripPurchase.Items.AddRange(new ToolStripItem[] { ToolStripStatusLabelErrorPurchase });
            StatusStripPurchase.Location = new Point(0, 567);
            StatusStripPurchase.Name = "StatusStripPurchase";
            StatusStripPurchase.Size = new Size(1103, 22);
            StatusStripPurchase.TabIndex = 185;
            StatusStripPurchase.Text = "statusStrip1";
            // 
            // ToolStripStatusLabelErrorPurchase
            // 
            ToolStripStatusLabelErrorPurchase.Name = "ToolStripStatusLabelErrorPurchase";
            ToolStripStatusLabelErrorPurchase.Size = new Size(94, 17);
            ToolStripStatusLabelErrorPurchase.Text = "                             ";
            // 
            // BtnStockMovementExit
            // 
            BtnStockMovementExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnStockMovementExit.Location = new Point(732, 531);
            BtnStockMovementExit.Name = "BtnStockMovementExit";
            BtnStockMovementExit.Size = new Size(75, 23);
            BtnStockMovementExit.TabIndex = 191;
            BtnStockMovementExit.Text = "Exit [F10]";
            BtnStockMovementExit.UseVisualStyleBackColor = true;
            BtnStockMovementExit.Click += BtnStockMovementExit_Click;
            // 
            // BtnStockMovementPrint
            // 
            BtnStockMovementPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnStockMovementPrint.Location = new Point(329, 531);
            BtnStockMovementPrint.Name = "BtnStockMovementPrint";
            BtnStockMovementPrint.Size = new Size(83, 23);
            BtnStockMovementPrint.TabIndex = 188;
            BtnStockMovementPrint.Text = "Print [F9]";
            BtnStockMovementPrint.UseVisualStyleBackColor = true;
            BtnStockMovementPrint.Click += BtnStockMovementPrint_Click;
            // 
            // BtnStockMovementCancel
            // 
            BtnStockMovementCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnStockMovementCancel.Location = new Point(554, 531);
            BtnStockMovementCancel.Name = "BtnStockMovementCancel";
            BtnStockMovementCancel.Size = new Size(83, 23);
            BtnStockMovementCancel.TabIndex = 187;
            BtnStockMovementCancel.Text = "Cancel [Esc]";
            BtnStockMovementCancel.UseVisualStyleBackColor = true;
            BtnStockMovementCancel.Click += BtnStockMovementCancel_Click;
            // 
            // BtnStockMovementSave
            // 
            BtnStockMovementSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnStockMovementSave.Location = new Point(643, 531);
            BtnStockMovementSave.Name = "BtnStockMovementSave";
            BtnStockMovementSave.Size = new Size(83, 23);
            BtnStockMovementSave.TabIndex = 186;
            BtnStockMovementSave.Text = "Save [F8]";
            BtnStockMovementSave.UseVisualStyleBackColor = true;
            BtnStockMovementSave.Click += BtnStockMovementSave_Click;
            BtnStockMovementSave.PreviewKeyDown += BtnStockMovementSave_PreviewKeyDown;
            // 
            // BtnStockMovementDelete
            // 
            BtnStockMovementDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnStockMovementDelete.Location = new Point(109, 531);
            BtnStockMovementDelete.Name = "BtnStockMovementDelete";
            BtnStockMovementDelete.Size = new Size(83, 23);
            BtnStockMovementDelete.TabIndex = 189;
            BtnStockMovementDelete.Text = "Delete [F4]";
            BtnStockMovementDelete.UseVisualStyleBackColor = true;
            BtnStockMovementDelete.Click += BtnStockMovementDelete_Click;
            // 
            // BtnStockMovementNew
            // 
            BtnStockMovementNew.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnStockMovementNew.Location = new Point(20, 531);
            BtnStockMovementNew.Name = "BtnStockMovementNew";
            BtnStockMovementNew.Size = new Size(83, 23);
            BtnStockMovementNew.TabIndex = 190;
            BtnStockMovementNew.Text = "New [F3]";
            BtnStockMovementNew.UseVisualStyleBackColor = true;
            BtnStockMovementNew.Click += BtnStockMovementNew_Click;
            // 
            // TextBoxStockMovementId
            // 
            TextBoxStockMovementId.Location = new Point(286, 567);
            TextBoxStockMovementId.Margin = new Padding(2);
            TextBoxStockMovementId.Name = "TextBoxStockMovementId";
            TextBoxStockMovementId.Size = new Size(79, 21);
            TextBoxStockMovementId.TabIndex = 192;
            TextBoxStockMovementId.Visible = false;
            // 
            // TimerStock
            // 
            TimerStock.Interval = 400;
            // 
            // BtnStockRequestComplete
            // 
            BtnStockRequestComplete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnStockRequestComplete.Location = new Point(418, 531);
            BtnStockRequestComplete.Name = "BtnStockRequestComplete";
            BtnStockRequestComplete.Size = new Size(130, 23);
            BtnStockRequestComplete.TabIndex = 193;
            BtnStockRequestComplete.Text = "Complete Request";
            BtnStockRequestComplete.UseVisualStyleBackColor = true;
            BtnStockRequestComplete.Click += BtnStockRequestComplete_Click;
            // 
            // LastStockMovementReferenceNumber
            // 
            LastStockMovementReferenceNumber.AutoSize = true;
            LastStockMovementReferenceNumber.Font = new Font("Tahoma", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            LastStockMovementReferenceNumber.Location = new Point(167, 56);
            LastStockMovementReferenceNumber.Name = "LastStockMovementReferenceNumber";
            LastStockMovementReferenceNumber.Size = new Size(114, 25);
            LastStockMovementReferenceNumber.TabIndex = 239;
            LastStockMovementReferenceNumber.Text = "SR0000000";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(169, 41);
            label3.Name = "label3";
            label3.Size = new Size(92, 13);
            label3.TabIndex = 238;
            label3.Text = "Last Reference";
            // 
            // FormIntraStockRequest
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1103, 589);
            Controls.Add(LastStockMovementReferenceNumber);
            Controls.Add(label3);
            Controls.Add(ComboBoxToLocation);
            Controls.Add(ComboBoxFromLocation);
            Controls.Add(BtnStockRequestComplete);
            Controls.Add(TextBoxStockMovementId);
            Controls.Add(BtnStockMovementExit);
            Controls.Add(BtnStockMovementPrint);
            Controls.Add(BtnStockMovementCancel);
            Controls.Add(BtnStockMovementSave);
            Controls.Add(BtnStockMovementDelete);
            Controls.Add(BtnStockMovementNew);
            Controls.Add(StatusStripPurchase);
            Controls.Add(StockMovementProductDetails);
            Controls.Add(GridViewStockItemTotal);
            Controls.Add(GridViewStockMovementItem);
            Controls.Add(label2);
            Controls.Add(label12);
            Controls.Add(DatetimePickerStockMovementDate);
            Controls.Add(label4);
            Controls.Add(StockMovementReferenceNumber);
            Controls.Add(label1);
            Controls.Add(toolStrip1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormIntraStockRequest";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Intra Stock Request";
            FormClosing += FormStockMovement_FormClosing;
            Load += FormIntraStockRequest_Load;
            Controls.SetChildIndex(toolStrip1, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(StockMovementReferenceNumber, 0);
            Controls.SetChildIndex(label4, 0);
            Controls.SetChildIndex(DatetimePickerStockMovementDate, 0);
            Controls.SetChildIndex(label12, 0);
            Controls.SetChildIndex(label2, 0);
            Controls.SetChildIndex(GridViewStockMovementItem, 0);
            Controls.SetChildIndex(GridViewStockItemTotal, 0);
            Controls.SetChildIndex(StockMovementProductDetails, 0);
            Controls.SetChildIndex(StatusStripPurchase, 0);
            Controls.SetChildIndex(BtnStockMovementNew, 0);
            Controls.SetChildIndex(BtnStockMovementDelete, 0);
            Controls.SetChildIndex(BtnStockMovementSave, 0);
            Controls.SetChildIndex(BtnStockMovementCancel, 0);
            Controls.SetChildIndex(BtnStockMovementPrint, 0);
            Controls.SetChildIndex(BtnStockMovementExit, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(TextBoxStockMovementId, 0);
            Controls.SetChildIndex(BtnStockRequestComplete, 0);
            Controls.SetChildIndex(ComboBoxFromLocation, 0);
            Controls.SetChildIndex(ComboBoxToLocation, 0);
            Controls.SetChildIndex(label3, 0);
            Controls.SetChildIndex(LastStockMovementReferenceNumber, 0);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewStockItemTotal).EndInit();
            ((System.ComponentModel.ISupportInitialize)GridViewStockMovementItem).EndInit();
            StatusStripPurchase.ResumeLayout(false);
            StatusStripPurchase.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripTextBox TextBoxStockMovementSearch;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton BtnStockSearch;
        private controls.ComboBoxSwapTextBox ComboBoxFromLocation;
        private controls.ComboBoxSwapTextBox ComboBoxToLocation;
        private Label label2;
        private Label label12;
        private controls.text.DateWithCalendar DatetimePickerStockMovementDate;
        private Label label4;
        private Label StockMovementReferenceNumber;
        private Label label1;
        private DataGridView GridViewStockItemTotal;
        private controls.DataViewVerticalScroll GridViewStockMovementItem;
        private controls.text.ProductDetails StockMovementProductDetails;
        private StatusStrip StatusStripPurchase;
        private ToolStripStatusLabel ToolStripStatusLabelErrorPurchase;
        private Button BtnStockMovementExit;
        private Button BtnStockMovementPrint;
        private Button BtnStockMovementCancel;
        private Button BtnStockMovementSave;
        private Button BtnStockMovementDelete;
        private Button BtnStockMovementNew;
        private TextBox TextBoxStockMovementId;
        private System.Windows.Forms.Timer TimerStock;
        private Button BtnStockRequestComplete;
        private DataGridViewTextBoxColumn Total;
        private controls.grid.DataGridViewQuantityColumn Value;
        private DataGridViewTextBoxColumn dummy;
        private DataGridViewTextBoxColumn Column8;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Account;
        private DataGridViewComboBoxColumn UnitOfMeasure;
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
        private DataGridViewButtonColumn Delete;
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
        private Label LastStockMovementReferenceNumber;
        private Label label3;
    }
}