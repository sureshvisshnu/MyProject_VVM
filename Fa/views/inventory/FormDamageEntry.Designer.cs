
namespace fa.views.inventory
{
    partial class FormDamageEntry
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
            DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle27 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle28 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle16 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle17 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle18 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle19 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle20 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle21 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle22 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle23 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle24 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle25 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle26 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDamageEntry));
            ComboBoxLocation = new controls.ComboBoxSwapTextBox();
            TextBoxStockDamageId = new TextBox();
            GridViewStockDamageItem = new controls.DataViewVerticalScroll();
            Column1 = new DataGridViewTextBoxColumn();
            Account = new DataGridViewTextBoxColumn();
            UnitOfMeasure = new DataGridViewComboBoxColumn();
            Column3 = new controls.grid.DataGridViewQuantityColumn();
            Column14 = new DataGridViewTextBoxColumn();
            Column15 = new controls.grid.DataGridViewCalendarColumn();
            Description = new DataGridViewTextBoxColumn();
            Delete = new DataGridViewTextBoxColumn();
            Cost = new controls.grid.DataGridViewCurrencyColumn();
            Column2 = new controls.grid.DataGridViewCurrencyColumn();
            Column6 = new controls.grid.DataGridViewCurrencyColumn();
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
            statusStrip1 = new StatusStrip();
            StockDamageErrorMsg = new ToolStripStatusLabel();
            label12 = new Label();
            DatetimePickerStockDamageDate = new controls.text.DateWithCalendar();
            label4 = new Label();
            BtnStockDamageExit = new Button();
            BtnStockDamagePrint = new Button();
            BtnStockDamageCancel = new Button();
            BtnStockDamageSave = new Button();
            BtnStockDamageDelete = new Button();
            BtnStockDamageNew = new Button();
            StockDamageProductDetails = new controls.text.ProductDetails();
            StockDamageReferenceNumber = new Label();
            LabelRefence = new Label();
            toolStrip1 = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            TextBoxStockDamageSearch = new ToolStripTextBox();
            toolStripSeparator1 = new ToolStripSeparator();
            BtnStockDamageSearch = new ToolStripButton();
            TimerStock = new System.Windows.Forms.Timer(components);
            LastStockDamageReferenceNumber = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)GridViewStockDamageItem).BeginInit();
            statusStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
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
            // ComboBoxLocation
            // 
            ComboBoxLocation.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxLocation.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxLocation.FormattingEnabled = true;
            ComboBoxLocation.Location = new Point(403, 54);
            ComboBoxLocation.Name = "ComboBoxLocation";
            ComboBoxLocation.Size = new Size(204, 21);
            ComboBoxLocation.TabIndex = 228;
            ComboBoxLocation.TxtVisible = true;
            ComboBoxLocation.SelectedIndexChanged += ComboBoxLocation_SelectedIndexChanged;
            ComboBoxLocation.PreviewKeyDown += ComboBoxLocation_PreviewKeyDown;
            // 
            // TextBoxStockDamageId
            // 
            TextBoxStockDamageId.Location = new Point(251, 533);
            TextBoxStockDamageId.Margin = new Padding(2);
            TextBoxStockDamageId.Name = "TextBoxStockDamageId";
            TextBoxStockDamageId.Size = new Size(79, 21);
            TextBoxStockDamageId.TabIndex = 233;
            TextBoxStockDamageId.Visible = false;
            // 
            // GridViewStockDamageItem
            // 
            GridViewStockDamageItem.AllowUserToAddRows = false;
            GridViewStockDamageItem.AllowUserToDeleteRows = false;
            GridViewStockDamageItem.AllowUserToResizeColumns = false;
            GridViewStockDamageItem.AllowUserToResizeRows = false;
            GridViewStockDamageItem.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            GridViewStockDamageItem.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle15.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = SystemColors.Control;
            dataGridViewCellStyle15.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle15.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle15.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle15.WrapMode = DataGridViewTriState.True;
            GridViewStockDamageItem.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle15;
            GridViewStockDamageItem.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GridViewStockDamageItem.Columns.AddRange(new DataGridViewColumn[] { Column1, Account, UnitOfMeasure, Column3, Column14, Column15, Description, Delete, Cost, Column2, Column6, Column11, Column12, PurchDetailID, Column9, Column16, Column17, Column18, Column19, Column20, Column21, Column22 });
            dataGridViewCellStyle27.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle27.BackColor = SystemColors.Window;
            dataGridViewCellStyle27.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle27.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle27.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle27.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle27.WrapMode = DataGridViewTriState.True;
            GridViewStockDamageItem.DefaultCellStyle = dataGridViewCellStyle27;
            GridViewStockDamageItem.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewStockDamageItem.EnableHeadersVisualStyles = false;
            GridViewStockDamageItem.Location = new Point(12, 86);
            GridViewStockDamageItem.MultiSelect = false;
            GridViewStockDamageItem.Name = "GridViewStockDamageItem";
            GridViewStockDamageItem.RowHeadersVisible = false;
            dataGridViewCellStyle28.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle28.SelectionForeColor = SystemColors.ActiveCaptionText;
            dataGridViewCellStyle28.WrapMode = DataGridViewTriState.True;
            GridViewStockDamageItem.RowsDefaultCellStyle = dataGridViewCellStyle28;
            GridViewStockDamageItem.ScrollBars = ScrollBars.Vertical;
            GridViewStockDamageItem.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewStockDamageItem.ShowCellToolTips = false;
            GridViewStockDamageItem.Size = new Size(825, 437);
            GridViewStockDamageItem.TabIndex = 232;
            GridViewStockDamageItem.CellClick += GridViewStockDamageItem_CellClick;
            GridViewStockDamageItem.CellEndEdit += GridViewStockDamageItem_CellEndEdit;
            GridViewStockDamageItem.CellEnter += GridViewStockDamageItem_CellEnter;
            GridViewStockDamageItem.CellFormatting += GridViewStockDamageItem_CellFormatting;
            GridViewStockDamageItem.DataError += GridViewStockDamageItem_DataError;
            GridViewStockDamageItem.EditingControlShowing += GridViewStockDamageItem_EditingControlShowing;
            GridViewStockDamageItem.RowEnter += GridViewStockDamageItem_RowEnter;
            GridViewStockDamageItem.RowsAdded += GridViewStockDamageItem_RowsAdded;
            GridViewStockDamageItem.KeyDown += GridViewStockDamageItem_KeyDown;
            GridViewStockDamageItem.KeyPress += GridViewStockDamageItem_KeyPress;
            // 
            // Column1
            // 
            dataGridViewCellStyle16.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle16.WrapMode = DataGridViewTriState.True;
            Column1.DefaultCellStyle = dataGridViewCellStyle16;
            Column1.HeaderText = "#";
            Column1.Name = "Column1";
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 30;
            // 
            // Account
            // 
            Account.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle17.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle17.WrapMode = DataGridViewTriState.True;
            Account.DefaultCellStyle = dataGridViewCellStyle17;
            Account.HeaderText = "Items [ F2 ]";
            Account.MaxInputLength = 35;
            Account.Name = "Account";
            Account.Resizable = DataGridViewTriState.False;
            Account.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // UnitOfMeasure
            // 
            dataGridViewCellStyle18.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle18.WrapMode = DataGridViewTriState.True;
            UnitOfMeasure.DefaultCellStyle = dataGridViewCellStyle18;
            UnitOfMeasure.FlatStyle = FlatStyle.Popup;
            UnitOfMeasure.HeaderText = "UOM";
            UnitOfMeasure.Name = "UnitOfMeasure";
            UnitOfMeasure.Resizable = DataGridViewTriState.False;
            UnitOfMeasure.Width = 75;
            // 
            // Column3
            // 
            dataGridViewCellStyle19.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle19.NullValue = "0";
            dataGridViewCellStyle19.WrapMode = DataGridViewTriState.True;
            Column3.DefaultCellStyle = dataGridViewCellStyle19;
            Column3.HeaderText = "Quantity";
            Column3.Name = "Column3";
            Column3.Resizable = DataGridViewTriState.False;
            Column3.Width = 75;
            // 
            // Column14
            // 
            dataGridViewCellStyle20.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle20.WrapMode = DataGridViewTriState.True;
            Column14.DefaultCellStyle = dataGridViewCellStyle20;
            Column14.HeaderText = "Batch No";
            Column14.MaxInputLength = 10;
            Column14.Name = "Column14";
            Column14.Resizable = DataGridViewTriState.False;
            Column14.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column15
            // 
            dataGridViewCellStyle21.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle21.WrapMode = DataGridViewTriState.True;
            Column15.DefaultCellStyle = dataGridViewCellStyle21;
            Column15.HeaderText = "Exp Date";
            Column15.Name = "Column15";
            Column15.Resizable = DataGridViewTriState.False;
            Column15.Width = 80;
            // 
            // Description
            // 
            dataGridViewCellStyle22.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle22.WrapMode = DataGridViewTriState.True;
            Description.DefaultCellStyle = dataGridViewCellStyle22;
            Description.HeaderText = "Description";
            Description.MaxInputLength = 250;
            Description.Name = "Description";
            Description.Resizable = DataGridViewTriState.False;
            Description.SortMode = DataGridViewColumnSortMode.NotSortable;
            Description.Width = 200;
            // 
            // Delete
            // 
            dataGridViewCellStyle23.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle23.BackColor = Color.White;
            dataGridViewCellStyle23.ForeColor = Color.Black;
            dataGridViewCellStyle23.NullValue = "X";
            dataGridViewCellStyle23.SelectionBackColor = Color.White;
            dataGridViewCellStyle23.SelectionForeColor = Color.Black;
            Delete.DefaultCellStyle = dataGridViewCellStyle23;
            Delete.HeaderText = "";
            Delete.Name = "Delete";
            Delete.Resizable = DataGridViewTriState.False;
            Delete.SortMode = DataGridViewColumnSortMode.NotSortable;
            Delete.Width = 25;
            // 
            // Cost
            // 
            dataGridViewCellStyle24.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle24.NullValue = "0.00";
            Cost.DefaultCellStyle = dataGridViewCellStyle24;
            Cost.HeaderText = "Cost";
            Cost.Name = "Cost";
            Cost.Resizable = DataGridViewTriState.False;
            Cost.Visible = false;
            Cost.Width = 75;
            // 
            // Column2
            // 
            dataGridViewCellStyle25.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle25.NullValue = "0.00";
            Column2.DefaultCellStyle = dataGridViewCellStyle25;
            Column2.HeaderText = "Price";
            Column2.Name = "Column2";
            Column2.Resizable = DataGridViewTriState.False;
            Column2.Visible = false;
            Column2.Width = 75;
            // 
            // Column6
            // 
            dataGridViewCellStyle26.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle26.NullValue = "0.00";
            Column6.DefaultCellStyle = dataGridViewCellStyle26;
            Column6.HeaderText = "Amount";
            Column6.Name = "Column6";
            Column6.Resizable = DataGridViewTriState.False;
            Column6.Visible = false;
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
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { StockDamageErrorMsg });
            statusStrip1.Location = new Point(0, 566);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1127, 22);
            statusStrip1.TabIndex = 231;
            statusStrip1.Text = "statusStrip1";
            // 
            // StockDamageErrorMsg
            // 
            StockDamageErrorMsg.Name = "StockDamageErrorMsg";
            StockDamageErrorMsg.Size = new Size(28, 17);
            StockDamageErrorMsg.Text = "       ";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label12.Location = new Point(400, 39);
            label12.Name = "label12";
            label12.Size = new Size(90, 13);
            label12.TabIndex = 230;
            label12.Text = "Stock Location";
            // 
            // DatetimePickerStockDamageDate
            // 
            DatetimePickerStockDamageDate.BackColor = Color.White;
            DatetimePickerStockDamageDate.BorderStyle = BorderStyle.FixedSingle;
            DatetimePickerStockDamageDate.Date = null;
            DatetimePickerStockDamageDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DatetimePickerStockDamageDate.Format = "MM/dd/yyyy";
            DatetimePickerStockDamageDate.Location = new Point(303, 54);
            DatetimePickerStockDamageDate.Margin = new Padding(4, 3, 4, 3);
            DatetimePickerStockDamageDate.MaxDate = new DateTime(9997, 12, 31, 8, 13, 39, 0);
            DatetimePickerStockDamageDate.MinDate = new DateTime(1900, 1, 1, 23, 43, 31, 0);
            DatetimePickerStockDamageDate.Name = "DatetimePickerStockDamageDate";
            DatetimePickerStockDamageDate.ReadOnly = false;
            DatetimePickerStockDamageDate.Size = new Size(93, 21);
            DatetimePickerStockDamageDate.TabIndex = 227;
            DatetimePickerStockDamageDate.PreviewKeyDown += DatetimePickerStockDamageDate_PreviewKeyDown;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(301, 39);
            label4.Name = "label4";
            label4.Size = new Size(34, 13);
            label4.TabIndex = 229;
            label4.Text = "Date";
            // 
            // BtnStockDamageExit
            // 
            BtnStockDamageExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnStockDamageExit.Location = new Point(755, 533);
            BtnStockDamageExit.Name = "BtnStockDamageExit";
            BtnStockDamageExit.Size = new Size(75, 23);
            BtnStockDamageExit.TabIndex = 226;
            BtnStockDamageExit.Text = "Exit [F10]";
            BtnStockDamageExit.UseVisualStyleBackColor = true;
            BtnStockDamageExit.Click += BtnStockDamageExit_Click;
            // 
            // BtnStockDamagePrint
            // 
            BtnStockDamagePrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnStockDamagePrint.Location = new Point(488, 533);
            BtnStockDamagePrint.Name = "BtnStockDamagePrint";
            BtnStockDamagePrint.Size = new Size(83, 23);
            BtnStockDamagePrint.TabIndex = 223;
            BtnStockDamagePrint.Text = "Print [F9]";
            BtnStockDamagePrint.UseVisualStyleBackColor = true;
            BtnStockDamagePrint.Click += BtnStockDamagePrint_Click;
            // 
            // BtnStockDamageCancel
            // 
            BtnStockDamageCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnStockDamageCancel.Location = new Point(577, 533);
            BtnStockDamageCancel.Name = "BtnStockDamageCancel";
            BtnStockDamageCancel.Size = new Size(83, 23);
            BtnStockDamageCancel.TabIndex = 222;
            BtnStockDamageCancel.Text = "Cancel [Esc]";
            BtnStockDamageCancel.UseVisualStyleBackColor = true;
            BtnStockDamageCancel.Click += BtnStockDamageCancel_Click;
            // 
            // BtnStockDamageSave
            // 
            BtnStockDamageSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnStockDamageSave.Location = new Point(666, 533);
            BtnStockDamageSave.Name = "BtnStockDamageSave";
            BtnStockDamageSave.Size = new Size(83, 23);
            BtnStockDamageSave.TabIndex = 221;
            BtnStockDamageSave.Text = "Save [F8]";
            BtnStockDamageSave.UseVisualStyleBackColor = true;
            BtnStockDamageSave.Click += BtnStockDamageSave_Click;
            BtnStockDamageSave.PreviewKeyDown += BtnStockDamageSave_PreviewKeyDown;
            // 
            // BtnStockDamageDelete
            // 
            BtnStockDamageDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnStockDamageDelete.Location = new Point(107, 533);
            BtnStockDamageDelete.Name = "BtnStockDamageDelete";
            BtnStockDamageDelete.Size = new Size(83, 23);
            BtnStockDamageDelete.TabIndex = 224;
            BtnStockDamageDelete.Text = "Delete [F4]";
            BtnStockDamageDelete.UseVisualStyleBackColor = true;
            BtnStockDamageDelete.Click += BtnStockDamageDelete_Click;
            // 
            // BtnStockDamageNew
            // 
            BtnStockDamageNew.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnStockDamageNew.Location = new Point(18, 533);
            BtnStockDamageNew.Name = "BtnStockDamageNew";
            BtnStockDamageNew.Size = new Size(83, 23);
            BtnStockDamageNew.TabIndex = 225;
            BtnStockDamageNew.Text = "New [F3]";
            BtnStockDamageNew.UseVisualStyleBackColor = true;
            BtnStockDamageNew.Click += BtnStockDamageNew_Click;
            // 
            // StockDamageProductDetails
            // 
            StockDamageProductDetails.BatchId = 0L;
            StockDamageProductDetails.CurrentDate = null;
            StockDamageProductDetails.EditableStock = 0D;
            StockDamageProductDetails.Location = new Point(844, 39);
            StockDamageProductDetails.LocationId = 0L;
            StockDamageProductDetails.Margin = new Padding(4, 3, 4, 3);
            StockDamageProductDetails.Name = "StockDamageProductDetails";
            StockDamageProductDetails.ProductId = 0L;
            StockDamageProductDetails.Size = new Size(274, 520);
            StockDamageProductDetails.TabIndex = 220;
            // 
            // StockDamageReferenceNumber
            // 
            StockDamageReferenceNumber.AutoSize = true;
            StockDamageReferenceNumber.Font = new Font("Tahoma", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            StockDamageReferenceNumber.Location = new Point(7, 54);
            StockDamageReferenceNumber.Name = "StockDamageReferenceNumber";
            StockDamageReferenceNumber.Size = new Size(115, 25);
            StockDamageReferenceNumber.TabIndex = 219;
            StockDamageReferenceNumber.Text = "SD0000000";
            // 
            // LabelRefence
            // 
            LabelRefence.AutoSize = true;
            LabelRefence.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelRefence.Location = new Point(9, 39);
            LabelRefence.Name = "LabelRefence";
            LabelRefence.Size = new Size(65, 13);
            LabelRefence.TabIndex = 218;
            LabelRefence.Text = "Reference";
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = SystemColors.ControlLight;
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, TextBoxStockDamageSearch, toolStripSeparator1, BtnStockDamageSearch });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new Padding(5);
            toolStrip1.Size = new Size(1127, 33);
            toolStrip1.TabIndex = 217;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(42, 20);
            toolStripLabel1.Text = "Search";
            // 
            // TextBoxStockDamageSearch
            // 
            TextBoxStockDamageSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxStockDamageSearch.MaxLength = 30;
            TextBoxStockDamageSearch.Name = "TextBoxStockDamageSearch";
            TextBoxStockDamageSearch.Size = new Size(200, 23);
            TextBoxStockDamageSearch.KeyDown += TextBoxStockDamageSearch_KeyDown;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 23);
            // 
            // BtnStockDamageSearch
            // 
            BtnStockDamageSearch.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnStockDamageSearch.Image = (Image)resources.GetObject("BtnStockDamageSearch.Image");
            BtnStockDamageSearch.ImageTransparentColor = Color.Magenta;
            BtnStockDamageSearch.Name = "BtnStockDamageSearch";
            BtnStockDamageSearch.Size = new Size(26, 20);
            BtnStockDamageSearch.Text = "Go";
            BtnStockDamageSearch.Click += BtnStockDamageSearch_Click;
            // 
            // TimerStock
            // 
            TimerStock.Interval = 400;
            TimerStock.Tick += TimerStock_Tick;
            // 
            // LastStockDamageReferenceNumber
            // 
            LastStockDamageReferenceNumber.AutoSize = true;
            LastStockDamageReferenceNumber.Font = new Font("Tahoma", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            LastStockDamageReferenceNumber.Location = new Point(158, 54);
            LastStockDamageReferenceNumber.Name = "LastStockDamageReferenceNumber";
            LastStockDamageReferenceNumber.Size = new Size(115, 25);
            LastStockDamageReferenceNumber.TabIndex = 235;
            LastStockDamageReferenceNumber.Text = "SD0000000";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(160, 39);
            label2.Name = "label2";
            label2.Size = new Size(92, 13);
            label2.TabIndex = 234;
            label2.Text = "Last Reference";
            // 
            // FormDamageEntry
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1127, 588);
            Controls.Add(LastStockDamageReferenceNumber);
            Controls.Add(label2);
            Controls.Add(ComboBoxLocation);
            Controls.Add(TextBoxStockDamageId);
            Controls.Add(GridViewStockDamageItem);
            Controls.Add(statusStrip1);
            Controls.Add(label12);
            Controls.Add(DatetimePickerStockDamageDate);
            Controls.Add(label4);
            Controls.Add(BtnStockDamageExit);
            Controls.Add(BtnStockDamagePrint);
            Controls.Add(BtnStockDamageCancel);
            Controls.Add(BtnStockDamageSave);
            Controls.Add(BtnStockDamageDelete);
            Controls.Add(BtnStockDamageNew);
            Controls.Add(StockDamageProductDetails);
            Controls.Add(StockDamageReferenceNumber);
            Controls.Add(LabelRefence);
            Controls.Add(toolStrip1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormDamageEntry";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Damage Entry";
            FormClosing += FormDamageEntry_FormClosing;
            Load += FormDamageEntry_Load;
            Controls.SetChildIndex(toolStrip1, 0);
            Controls.SetChildIndex(LabelRefence, 0);
            Controls.SetChildIndex(StockDamageReferenceNumber, 0);
            Controls.SetChildIndex(StockDamageProductDetails, 0);
            Controls.SetChildIndex(BtnStockDamageNew, 0);
            Controls.SetChildIndex(BtnStockDamageDelete, 0);
            Controls.SetChildIndex(BtnStockDamageSave, 0);
            Controls.SetChildIndex(BtnStockDamageCancel, 0);
            Controls.SetChildIndex(BtnStockDamagePrint, 0);
            Controls.SetChildIndex(BtnStockDamageExit, 0);
            Controls.SetChildIndex(label4, 0);
            Controls.SetChildIndex(DatetimePickerStockDamageDate, 0);
            Controls.SetChildIndex(label12, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(GridViewStockDamageItem, 0);
            Controls.SetChildIndex(TextBoxStockDamageId, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(ComboBoxLocation, 0);
            Controls.SetChildIndex(label2, 0);
            Controls.SetChildIndex(LastStockDamageReferenceNumber, 0);
            ((System.ComponentModel.ISupportInitialize)GridViewStockDamageItem).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private controls.ComboBoxSwapTextBox ComboBoxLocation;
        private TextBox TextBoxStockDamageId;
        private controls.DataViewVerticalScroll GridViewStockDamageItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel StockDamageErrorMsg;
        private Label label12;
        private controls.text.DateWithCalendar DatetimePickerStockDamageDate;
        private Label label4;
        private Button BtnStockDamageExit;
        private Button BtnStockDamagePrint;
        private Button BtnStockDamageCancel;
        private Button BtnStockDamageSave;
        private Button BtnStockDamageDelete;
        private Button BtnStockDamageNew;
        private controls.text.ProductDetails StockDamageProductDetails;
        private Label StockDamageReferenceNumber;
        private Label LabelRefence;
        private ToolStrip toolStrip1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripTextBox TextBoxStockDamageSearch;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton BtnStockDamageSearch;
        private System.Windows.Forms.Timer TimerStock;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Account;
        private DataGridViewComboBoxColumn UnitOfMeasure;
        private controls.grid.DataGridViewQuantityColumn Column3;
        private DataGridViewTextBoxColumn Column14;
        private controls.grid.DataGridViewCalendarColumn Column15;
        private DataGridViewTextBoxColumn Description;
        private DataGridViewTextBoxColumn Delete;
        private controls.grid.DataGridViewCurrencyColumn Cost;
        private controls.grid.DataGridViewCurrencyColumn Column2;
        private controls.grid.DataGridViewCurrencyColumn Column6;
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
        private Label LastStockDamageReferenceNumber;
        private Label label2;
    }
}