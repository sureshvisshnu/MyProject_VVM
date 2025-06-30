namespace Fa.views.purchase
{
    partial class FormPurchaseOrder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPurchaseOrder));
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            TextBoxPurchaseOrderSupplier = new fa.views.controls.text.IDTextBox();
            BtnPurchaseOrderSearchSupplier = new Button();
            TextBoxPurchaseOrderAddress = new TextBox();
            labelPurchaseOrderAddress = new Label();
            labelPurchaseOrderSupplier = new Label();
            labelPurchaseOrderMethod = new Label();
            YesNoRbtPurchaseOrderMethod = new fa.views.controls.YesNoRadio();
            PurchaseOrderReferenceNumber = new Label();
            DatetimePickerPurchaseOrderDate = new fa.views.controls.text.DateWithCalendar();
            labelPurchaseOrderDate = new Label();
            groupBoxPurchaseOrderReferenceNumber = new GroupBox();
            groupBoxPurchaseOrderDateMethod = new GroupBox();
            ComboBoxPurchaseOrderInventoryLocation = new fa.views.controls.ComboBoxSwapTextBox();
            label12 = new Label();
            YesNoRadioPurchaseOrderType = new fa.views.controls.YesNoRadio();
            labelPurchaseOrderType = new Label();
            toolStripLabelPurchaseOrderSearch = new ToolStripLabel();
            TextBoxPurchaseOrderSearch = new ToolStripTextBox();
            BtnPurchaseOrderSearch = new ToolStripButton();
            toolStripPurchaseOrder = new ToolStrip();
            statusStripPurchaseOrder = new StatusStrip();
            ToolStripStatusLabelErrorPurchaseOrder = new ToolStripStatusLabel();
            GridViewPurchaseOrderItemTotal = new DataGridView();
            Total = new DataGridViewTextBoxColumn();
            Value = new fa.views.controls.grid.DataGridViewCurrencyColumn();
            dataGridViewButtonColumn1 = new DataGridViewTextBoxColumn();
            GridViewPurchaseOrderItem = new fa.views.controls.DataViewVerticalScroll();
            Column1 = new DataGridViewTextBoxColumn();
            Account = new DataGridViewTextBoxColumn();
            UnitOfMeasure = new DataGridViewTextBoxColumn();
            Column3 = new fa.views.controls.grid.DataGridViewQuantityColumn();
            QuotePrice = new fa.views.controls.grid.DataGridViewCurrencyColumn();
            QuoteAmount = new fa.views.controls.grid.DataGridViewCurrencyColumn();
            Delete = new DataGridViewButtonColumn();
            Column11 = new DataGridViewTextBoxColumn();
            Column12 = new DataGridViewCheckBoxColumn();
            SalesDetailID = new DataGridViewTextBoxColumn();
            Column9 = new DataGridViewTextBoxColumn();
            Column16 = new fa.views.controls.grid.DataGridViewCurrencyColumn();
            Column17 = new fa.views.controls.grid.DataGridViewCurrencyColumn();
            Column18 = new fa.views.controls.grid.DataGridViewCurrencyColumn();
            Column19 = new DataGridViewTextBoxColumn();
            Column20 = new DataGridViewTextBoxColumn();
            Column21 = new DataGridViewTextBoxColumn();
            Column22 = new DataGridViewTextBoxColumn();
            BtnPurchaseOrderCreateSale = new Button();
            BtnPurchaseOrderDelete = new Button();
            BtnPurchaseOrderNew = new Button();
            BtnPurchaseOrderExit = new Button();
            BtnPurchaseOrderPrint = new Button();
            BtnPurchaseOrderCancel = new Button();
            BtnPurchaseOrderSave = new Button();
            TextBoxPurchaseOrderProductName = new TextBox();
            TextBoxPurchaseOrderId = new TextBox();
            TimerPurchaseOrder = new System.Windows.Forms.Timer(components);
            BtnPurchaseOrderNewSupplier = new Dropdown_Button.UserControlButtonWithMenu();
            ImageListInvoice = new ImageList(components);
            labelPurchaseMemo = new Label();
            TextBoxPurchaseMemo = new TextBox();
            groupBox1 = new GroupBox();
            PrevPurchaseOrderReferenceNumber = new Label();
            groupBoxPurchaseOrderReferenceNumber.SuspendLayout();
            groupBoxPurchaseOrderDateMethod.SuspendLayout();
            toolStripPurchaseOrder.SuspendLayout();
            statusStripPurchaseOrder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewPurchaseOrderItemTotal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)GridViewPurchaseOrderItem).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(477, 101);
            ProductIdTransport.Size = new Size(116, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(477, 71);
            ProductBatchIdTransport.Size = new Size(116, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(477, 41);
            AccountIdTransport.Size = new Size(116, 21);
            // 
            // TextBoxPurchaseOrderSupplier
            // 
            TextBoxPurchaseOrderSupplier.BackColor = SystemColors.Window;
            TextBoxPurchaseOrderSupplier.Id = null;
            TextBoxPurchaseOrderSupplier.Location = new Point(12, 60);
            TextBoxPurchaseOrderSupplier.MaxLength = 30;
            TextBoxPurchaseOrderSupplier.Name = "TextBoxPurchaseOrderSupplier";
            TextBoxPurchaseOrderSupplier.Size = new Size(201, 21);
            TextBoxPurchaseOrderSupplier.TabIndex = 1;
            TextBoxPurchaseOrderSupplier.TextChanged += TextBoxPurchaseOrderSupplier_TextChanged;
            TextBoxPurchaseOrderSupplier.PreviewKeyDown += TextBoxPurchaseOrderSupplier_PreviewKeyDown;
            // 
            // BtnPurchaseOrderSearchSupplier
            // 
            BtnPurchaseOrderSearchSupplier.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnPurchaseOrderSearchSupplier.Location = new Point(218, 60);
            BtnPurchaseOrderSearchSupplier.Name = "BtnPurchaseOrderSearchSupplier";
            BtnPurchaseOrderSearchSupplier.Size = new Size(72, 23);
            BtnPurchaseOrderSearchSupplier.TabIndex = 1;
            BtnPurchaseOrderSearchSupplier.Text = "Search [F2]";
            BtnPurchaseOrderSearchSupplier.UseVisualStyleBackColor = true;
            BtnPurchaseOrderSearchSupplier.Click += BtnPurchaseOrderSearchSupplier_Click;
            // 
            // TextBoxPurchaseOrderAddress
            // 
            TextBoxPurchaseOrderAddress.Location = new Point(12, 101);
            TextBoxPurchaseOrderAddress.MaxLength = 250;
            TextBoxPurchaseOrderAddress.Multiline = true;
            TextBoxPurchaseOrderAddress.Name = "TextBoxPurchaseOrderAddress";
            TextBoxPurchaseOrderAddress.Size = new Size(347, 60);
            TextBoxPurchaseOrderAddress.TabIndex = 2;
            TextBoxPurchaseOrderAddress.KeyDown += TextBoxPurchaseOrderAddress_KeyDown;
            TextBoxPurchaseOrderAddress.Leave += TextBoxPurchaseOrderAddress_Leave;
            TextBoxPurchaseOrderAddress.PreviewKeyDown += TextBoxPurchaseOrderAddress_PreviewKeyDown;
            // 
            // labelPurchaseOrderAddress
            // 
            labelPurchaseOrderAddress.AutoSize = true;
            labelPurchaseOrderAddress.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            labelPurchaseOrderAddress.Location = new Point(12, 85);
            labelPurchaseOrderAddress.Name = "labelPurchaseOrderAddress";
            labelPurchaseOrderAddress.Size = new Size(46, 13);
            labelPurchaseOrderAddress.TabIndex = 294;
            labelPurchaseOrderAddress.Text = "Address";
            // 
            // labelPurchaseOrderSupplier
            // 
            labelPurchaseOrderSupplier.AutoSize = true;
            labelPurchaseOrderSupplier.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            labelPurchaseOrderSupplier.Location = new Point(12, 44);
            labelPurchaseOrderSupplier.Name = "labelPurchaseOrderSupplier";
            labelPurchaseOrderSupplier.Size = new Size(53, 13);
            labelPurchaseOrderSupplier.TabIndex = 293;
            labelPurchaseOrderSupplier.Text = "Supplier";
            // 
            // labelPurchaseOrderMethod
            // 
            labelPurchaseOrderMethod.AutoSize = true;
            labelPurchaseOrderMethod.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            labelPurchaseOrderMethod.Location = new Point(115, 15);
            labelPurchaseOrderMethod.Name = "labelPurchaseOrderMethod";
            labelPurchaseOrderMethod.Size = new Size(50, 13);
            labelPurchaseOrderMethod.TabIndex = 299;
            labelPurchaseOrderMethod.Text = "Method";
            // 
            // YesNoRbtPurchaseOrderMethod
            // 
            YesNoRbtPurchaseOrderMethod.BackColor = SystemColors.Control;
            YesNoRbtPurchaseOrderMethod.Checked = false;
            YesNoRbtPurchaseOrderMethod.FirstButtonName = "Credit";
            YesNoRbtPurchaseOrderMethod.Location = new Point(116, 32);
            YesNoRbtPurchaseOrderMethod.Margin = new Padding(4, 3, 4, 3);
            YesNoRbtPurchaseOrderMethod.Name = "YesNoRbtPurchaseOrderMethod";
            YesNoRbtPurchaseOrderMethod.SecondButtonName = "Cash";
            YesNoRbtPurchaseOrderMethod.Size = new Size(133, 25);
            YesNoRbtPurchaseOrderMethod.TabIndex = 5;
            YesNoRbtPurchaseOrderMethod.PreviewKeyDown += YesNoRbtPurchaseOrderMethod_PreviewKeyDown;
            // 
            // PurchaseOrderReferenceNumber
            // 
            PurchaseOrderReferenceNumber.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            PurchaseOrderReferenceNumber.Location = new Point(6, 15);
            PurchaseOrderReferenceNumber.Name = "PurchaseOrderReferenceNumber";
            PurchaseOrderReferenceNumber.Size = new Size(107, 25);
            PurchaseOrderReferenceNumber.TabIndex = 298;
            PurchaseOrderReferenceNumber.Text = "000000000";
            // 
            // DatetimePickerPurchaseOrderDate
            // 
            DatetimePickerPurchaseOrderDate.BackColor = Color.White;
            DatetimePickerPurchaseOrderDate.BorderStyle = BorderStyle.FixedSingle;
            DatetimePickerPurchaseOrderDate.Date = null;
            DatetimePickerPurchaseOrderDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DatetimePickerPurchaseOrderDate.Format = "MM/dd/yyyy";
            DatetimePickerPurchaseOrderDate.Location = new Point(13, 32);
            DatetimePickerPurchaseOrderDate.MaxDate = new DateTime(9997, 12, 31, 6, 34, 11, 0);
            DatetimePickerPurchaseOrderDate.MinDate = new DateTime(1900, 1, 1, 19, 28, 17, 0);
            DatetimePickerPurchaseOrderDate.Name = "DatetimePickerPurchaseOrderDate";
            DatetimePickerPurchaseOrderDate.ReadOnly = false;
            DatetimePickerPurchaseOrderDate.Size = new Size(93, 21);
            DatetimePickerPurchaseOrderDate.TabIndex = 4;
            DatetimePickerPurchaseOrderDate.PreviewKeyDown += DatetimePickerPurchaseOrderDate_PreviewKeyDown;
            // 
            // labelPurchaseOrderDate
            // 
            labelPurchaseOrderDate.AutoSize = true;
            labelPurchaseOrderDate.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            labelPurchaseOrderDate.Location = new Point(13, 15);
            labelPurchaseOrderDate.Name = "labelPurchaseOrderDate";
            labelPurchaseOrderDate.Size = new Size(34, 13);
            labelPurchaseOrderDate.TabIndex = 301;
            labelPurchaseOrderDate.Text = "Date";
            // 
            // groupBoxPurchaseOrderReferenceNumber
            // 
            groupBoxPurchaseOrderReferenceNumber.Controls.Add(PurchaseOrderReferenceNumber);
            groupBoxPurchaseOrderReferenceNumber.Location = new Point(627, 36);
            groupBoxPurchaseOrderReferenceNumber.Name = "groupBoxPurchaseOrderReferenceNumber";
            groupBoxPurchaseOrderReferenceNumber.Size = new Size(128, 56);
            groupBoxPurchaseOrderReferenceNumber.TabIndex = 302;
            groupBoxPurchaseOrderReferenceNumber.TabStop = false;
            groupBoxPurchaseOrderReferenceNumber.Text = "Ref #";
            // 
            // groupBoxPurchaseOrderDateMethod
            // 
            groupBoxPurchaseOrderDateMethod.Controls.Add(ComboBoxPurchaseOrderInventoryLocation);
            groupBoxPurchaseOrderDateMethod.Controls.Add(label12);
            groupBoxPurchaseOrderDateMethod.Controls.Add(YesNoRbtPurchaseOrderMethod);
            groupBoxPurchaseOrderDateMethod.Controls.Add(labelPurchaseOrderMethod);
            groupBoxPurchaseOrderDateMethod.Controls.Add(DatetimePickerPurchaseOrderDate);
            groupBoxPurchaseOrderDateMethod.Controls.Add(labelPurchaseOrderDate);
            groupBoxPurchaseOrderDateMethod.Location = new Point(365, 50);
            groupBoxPurchaseOrderDateMethod.Name = "groupBoxPurchaseOrderDateMethod";
            groupBoxPurchaseOrderDateMethod.Size = new Size(254, 111);
            groupBoxPurchaseOrderDateMethod.TabIndex = 3;
            groupBoxPurchaseOrderDateMethod.TabStop = false;
            // 
            // ComboBoxPurchaseOrderInventoryLocation
            // 
            ComboBoxPurchaseOrderInventoryLocation.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxPurchaseOrderInventoryLocation.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxPurchaseOrderInventoryLocation.FormattingEnabled = true;
            ComboBoxPurchaseOrderInventoryLocation.Location = new Point(13, 79);
            ComboBoxPurchaseOrderInventoryLocation.Margin = new Padding(4, 3, 4, 3);
            ComboBoxPurchaseOrderInventoryLocation.MaxLength = 4;
            ComboBoxPurchaseOrderInventoryLocation.Name = "ComboBoxPurchaseOrderInventoryLocation";
            ComboBoxPurchaseOrderInventoryLocation.Size = new Size(230, 21);
            ComboBoxPurchaseOrderInventoryLocation.TabIndex = 6;
            ComboBoxPurchaseOrderInventoryLocation.TxtVisible = true;
            ComboBoxPurchaseOrderInventoryLocation.Leave += ComboBoxPurchaseOrderInventoryLocation_Leave;
            ComboBoxPurchaseOrderInventoryLocation.PreviewKeyDown += ComboBoxPurchaseOrderInventoryLocation_PreviewKeyDown;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label12.Location = new Point(13, 59);
            label12.Margin = new Padding(4, 0, 4, 0);
            label12.Name = "label12";
            label12.Size = new Size(90, 13);
            label12.TabIndex = 303;
            label12.Text = "Stock Location";
            // 
            // YesNoRadioPurchaseOrderType
            // 
            YesNoRadioPurchaseOrderType.BackColor = SystemColors.Control;
            YesNoRadioPurchaseOrderType.Checked = true;
            YesNoRadioPurchaseOrderType.FirstButtonName = "Retail";
            YesNoRadioPurchaseOrderType.Location = new Point(201, 471);
            YesNoRadioPurchaseOrderType.Margin = new Padding(4, 3, 4, 3);
            YesNoRadioPurchaseOrderType.Name = "YesNoRadioPurchaseOrderType";
            YesNoRadioPurchaseOrderType.SecondButtonName = "Wholsale";
            YesNoRadioPurchaseOrderType.Size = new Size(160, 18);
            YesNoRadioPurchaseOrderType.TabIndex = 319;
            YesNoRadioPurchaseOrderType.Visible = false;
            // 
            // labelPurchaseOrderType
            // 
            labelPurchaseOrderType.AutoSize = true;
            labelPurchaseOrderType.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            labelPurchaseOrderType.Location = new Point(201, 455);
            labelPurchaseOrderType.Name = "labelPurchaseOrderType";
            labelPurchaseOrderType.Size = new Size(90, 13);
            labelPurchaseOrderType.TabIndex = 320;
            labelPurchaseOrderType.Text = "Purchase Type";
            labelPurchaseOrderType.Visible = false;
            // 
            // toolStripLabelPurchaseOrderSearch
            // 
            toolStripLabelPurchaseOrderSearch.Name = "toolStripLabelPurchaseOrderSearch";
            toolStripLabelPurchaseOrderSearch.Size = new Size(129, 30);
            toolStripLabelPurchaseOrderSearch.Text = " Search Purchase Order";
            // 
            // TextBoxPurchaseOrderSearch
            // 
            TextBoxPurchaseOrderSearch.AutoSize = false;
            TextBoxPurchaseOrderSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxPurchaseOrderSearch.MaxLength = 30;
            TextBoxPurchaseOrderSearch.Name = "TextBoxPurchaseOrderSearch";
            TextBoxPurchaseOrderSearch.Size = new Size(233, 23);
            TextBoxPurchaseOrderSearch.Leave += TextBoxPurchaseOrderSearch_Leave;
            TextBoxPurchaseOrderSearch.KeyDown += TextBoxPurchaseOrderSearch_KeyDown;
            // 
            // BtnPurchaseOrderSearch
            // 
            BtnPurchaseOrderSearch.AutoSize = false;
            BtnPurchaseOrderSearch.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnPurchaseOrderSearch.Image = (Image)resources.GetObject("BtnPurchaseOrderSearch.Image");
            BtnPurchaseOrderSearch.ImageTransparentColor = Color.Magenta;
            BtnPurchaseOrderSearch.Name = "BtnPurchaseOrderSearch";
            BtnPurchaseOrderSearch.Size = new Size(26, 20);
            BtnPurchaseOrderSearch.Text = "Go";
            BtnPurchaseOrderSearch.Click += BtnPurchaseOrderSearch_Click;
            // 
            // toolStripPurchaseOrder
            // 
            toolStripPurchaseOrder.AutoSize = false;
            toolStripPurchaseOrder.BackColor = SystemColors.ControlLight;
            toolStripPurchaseOrder.GripStyle = ToolStripGripStyle.Hidden;
            toolStripPurchaseOrder.Items.AddRange(new ToolStripItem[] { toolStripLabelPurchaseOrderSearch, TextBoxPurchaseOrderSearch, BtnPurchaseOrderSearch });
            toolStripPurchaseOrder.Location = new Point(0, 0);
            toolStripPurchaseOrder.Name = "toolStripPurchaseOrder";
            toolStripPurchaseOrder.Size = new Size(895, 33);
            toolStripPurchaseOrder.TabIndex = 289;
            // 
            // statusStripPurchaseOrder
            // 
            statusStripPurchaseOrder.Items.AddRange(new ToolStripItem[] { ToolStripStatusLabelErrorPurchaseOrder });
            statusStripPurchaseOrder.Location = new Point(0, 491);
            statusStripPurchaseOrder.Name = "statusStripPurchaseOrder";
            statusStripPurchaseOrder.Size = new Size(895, 22);
            statusStripPurchaseOrder.TabIndex = 304;
            statusStripPurchaseOrder.Text = "statusStrip1";
            // 
            // ToolStripStatusLabelErrorPurchaseOrder
            // 
            ToolStripStatusLabelErrorPurchaseOrder.Name = "ToolStripStatusLabelErrorPurchaseOrder";
            ToolStripStatusLabelErrorPurchaseOrder.Size = new Size(94, 17);
            ToolStripStatusLabelErrorPurchaseOrder.Text = "                             ";
            // 
            // GridViewPurchaseOrderItemTotal
            // 
            GridViewPurchaseOrderItemTotal.BackgroundColor = SystemColors.ButtonFace;
            GridViewPurchaseOrderItemTotal.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GridViewPurchaseOrderItemTotal.ColumnHeadersVisible = false;
            GridViewPurchaseOrderItemTotal.Columns.AddRange(new DataGridViewColumn[] { Total, Value, dataGridViewButtonColumn1 });
            GridViewPurchaseOrderItemTotal.Enabled = false;
            GridViewPurchaseOrderItemTotal.Location = new Point(12, 413);
            GridViewPurchaseOrderItemTotal.Margin = new Padding(4, 3, 4, 3);
            GridViewPurchaseOrderItemTotal.Name = "GridViewPurchaseOrderItemTotal";
            GridViewPurchaseOrderItemTotal.ReadOnly = true;
            GridViewPurchaseOrderItemTotal.RowHeadersVisible = false;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            GridViewPurchaseOrderItemTotal.RowsDefaultCellStyle = dataGridViewCellStyle3;
            GridViewPurchaseOrderItemTotal.Size = new Size(877, 24);
            GridViewPurchaseOrderItemTotal.TabIndex = 307;
            GridViewPurchaseOrderItemTotal.TabStop = false;
            // 
            // Total
            // 
            Total.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.NullValue = "Total :";
            Total.DefaultCellStyle = dataGridViewCellStyle1;
            Total.HeaderText = "Total";
            Total.Name = "Total";
            Total.ReadOnly = true;
            // 
            // Value
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle2.NullValue = "0.00";
            Value.DefaultCellStyle = dataGridViewCellStyle2;
            Value.HeaderText = "Value";
            Value.Name = "Value";
            Value.ReadOnly = true;
            Value.Resizable = DataGridViewTriState.True;
            Value.SortMode = DataGridViewColumnSortMode.Automatic;
            Value.Width = 80;
            // 
            // dataGridViewButtonColumn1
            // 
            dataGridViewButtonColumn1.HeaderText = "";
            dataGridViewButtonColumn1.Name = "dataGridViewButtonColumn1";
            dataGridViewButtonColumn1.ReadOnly = true;
            dataGridViewButtonColumn1.Resizable = DataGridViewTriState.True;
            dataGridViewButtonColumn1.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewButtonColumn1.Width = 25;
            // 
            // GridViewPurchaseOrderItem
            // 
            GridViewPurchaseOrderItem.AllowUserToAddRows = false;
            GridViewPurchaseOrderItem.AllowUserToDeleteRows = false;
            GridViewPurchaseOrderItem.AllowUserToResizeRows = false;
            GridViewPurchaseOrderItem.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            GridViewPurchaseOrderItem.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            GridViewPurchaseOrderItem.ColumnHeadersHeight = 20;
            GridViewPurchaseOrderItem.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewPurchaseOrderItem.Columns.AddRange(new DataGridViewColumn[] { Column1, Account, UnitOfMeasure, Column3, QuotePrice, QuoteAmount, Delete, Column11, Column12, SalesDetailID, Column9, Column16, Column17, Column18, Column19, Column20, Column21, Column22 });
            GridViewPurchaseOrderItem.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewPurchaseOrderItem.EnableHeadersVisualStyles = false;
            GridViewPurchaseOrderItem.Location = new Point(12, 171);
            GridViewPurchaseOrderItem.Margin = new Padding(4, 3, 4, 3);
            GridViewPurchaseOrderItem.MultiSelect = false;
            GridViewPurchaseOrderItem.Name = "GridViewPurchaseOrderItem";
            GridViewPurchaseOrderItem.RowHeadersVisible = false;
            dataGridViewCellStyle9.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle9.SelectionForeColor = SystemColors.ActiveCaptionText;
            GridViewPurchaseOrderItem.RowsDefaultCellStyle = dataGridViewCellStyle9;
            GridViewPurchaseOrderItem.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewPurchaseOrderItem.ShowCellToolTips = false;
            GridViewPurchaseOrderItem.Size = new Size(877, 266);
            GridViewPurchaseOrderItem.TabIndex = 8;
            GridViewPurchaseOrderItem.CellClick += GridViewPurchaseOrderItem_CellClick;
            GridViewPurchaseOrderItem.CellEndEdit += GridViewPurchaseOrderItem_CellEndEdit;
            GridViewPurchaseOrderItem.CellEnter += GridViewPurchaseOrderItem_CellEnter;
            GridViewPurchaseOrderItem.DataError += GridViewPurchaseOrderItem_DataError;
            GridViewPurchaseOrderItem.EditingControlShowing += GridViewPurchaseOrderItem_EditingControlShowing;
            GridViewPurchaseOrderItem.RowsAdded += GridViewPurchaseOrderItem_RowsAdded;
            GridViewPurchaseOrderItem.Leave += GridViewPurchaseOrderItem_Leave;
            // 
            // Column1
            // 
            Column1.HeaderText = "#";
            Column1.Name = "Column1";
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 30;
            // 
            // Account
            // 
            Account.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Account.HeaderText = "Items [F2]";
            Account.MaxInputLength = 35;
            Account.Name = "Account";
            Account.Resizable = DataGridViewTriState.False;
            Account.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // UnitOfMeasure
            // 
            UnitOfMeasure.HeaderText = "UOM";
            UnitOfMeasure.MaxInputLength = 20;
            UnitOfMeasure.Name = "UnitOfMeasure";
            UnitOfMeasure.Resizable = DataGridViewTriState.False;
            UnitOfMeasure.SortMode = DataGridViewColumnSortMode.NotSortable;
            UnitOfMeasure.Width = 50;
            // 
            // Column3
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.NullValue = "0";
            Column3.DefaultCellStyle = dataGridViewCellStyle5;
            Column3.HeaderText = "Quantity";
            Column3.Name = "Column3";
            Column3.Resizable = DataGridViewTriState.False;
            Column3.Width = 50;
            // 
            // QuotePrice
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.NullValue = "0.00";
            QuotePrice.DefaultCellStyle = dataGridViewCellStyle6;
            QuotePrice.HeaderText = "Price";
            QuotePrice.Name = "QuotePrice";
            QuotePrice.Resizable = DataGridViewTriState.False;
            QuotePrice.Width = 60;
            // 
            // QuoteAmount
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.NullValue = "0.00";
            QuoteAmount.DefaultCellStyle = dataGridViewCellStyle7;
            QuoteAmount.HeaderText = "Amount";
            QuoteAmount.Name = "QuoteAmount";
            QuoteAmount.Resizable = DataGridViewTriState.False;
            QuoteAmount.Width = 80;
            // 
            // Delete
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.NullValue = "X";
            Delete.DefaultCellStyle = dataGridViewCellStyle8;
            Delete.HeaderText = "";
            Delete.Name = "Delete";
            Delete.Resizable = DataGridViewTriState.False;
            Delete.Width = 25;
            // 
            // Column11
            // 
            Column11.HeaderText = "ProductId";
            Column11.Name = "Column11";
            Column11.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column11.Visible = false;
            // 
            // Column12
            // 
            Column12.HeaderText = "IsBatch";
            Column12.Name = "Column12";
            Column12.Visible = false;
            // 
            // SalesDetailID
            // 
            SalesDetailID.HeaderText = "SalesDetailID";
            SalesDetailID.Name = "SalesDetailID";
            SalesDetailID.Visible = false;
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
            Column16.Resizable = DataGridViewTriState.True;
            Column16.SortMode = DataGridViewColumnSortMode.Automatic;
            Column16.Visible = false;
            // 
            // Column17
            // 
            Column17.HeaderText = "Wholesale";
            Column17.Name = "Column17";
            Column17.Resizable = DataGridViewTriState.True;
            Column17.SortMode = DataGridViewColumnSortMode.Automatic;
            Column17.Visible = false;
            // 
            // Column18
            // 
            Column18.HeaderText = "MSRP";
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
            Column20.HeaderText = "RXFactor";
            Column20.Name = "Column20";
            Column20.Resizable = DataGridViewTriState.True;
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
            Column22.HeaderText = "WXFactor";
            Column22.Name = "Column22";
            Column22.Visible = false;
            // 
            // BtnPurchaseOrderCreateSale
            // 
            BtnPurchaseOrderCreateSale.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPurchaseOrderCreateSale.Location = new Point(510, 456);
            BtnPurchaseOrderCreateSale.Name = "BtnPurchaseOrderCreateSale";
            BtnPurchaseOrderCreateSale.Size = new Size(110, 23);
            BtnPurchaseOrderCreateSale.TabIndex = 11;
            BtnPurchaseOrderCreateSale.Text = "Create Purchase";
            BtnPurchaseOrderCreateSale.UseVisualStyleBackColor = true;
            BtnPurchaseOrderCreateSale.Click += BtnPurchaseOrderCreateSale_Click;
            // 
            // BtnPurchaseOrderDelete
            // 
            BtnPurchaseOrderDelete.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPurchaseOrderDelete.Location = new Point(105, 456);
            BtnPurchaseOrderDelete.Name = "BtnPurchaseOrderDelete";
            BtnPurchaseOrderDelete.Size = new Size(83, 23);
            BtnPurchaseOrderDelete.TabIndex = 12;
            BtnPurchaseOrderDelete.Text = "Delete [F4]";
            BtnPurchaseOrderDelete.UseVisualStyleBackColor = true;
            BtnPurchaseOrderDelete.Click += BtnPurchaseOrderDelete_Click;
            // 
            // BtnPurchaseOrderNew
            // 
            BtnPurchaseOrderNew.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPurchaseOrderNew.Location = new Point(16, 456);
            BtnPurchaseOrderNew.Name = "BtnPurchaseOrderNew";
            BtnPurchaseOrderNew.Size = new Size(83, 23);
            BtnPurchaseOrderNew.TabIndex = 13;
            BtnPurchaseOrderNew.Text = "New [F3]";
            BtnPurchaseOrderNew.UseVisualStyleBackColor = true;
            BtnPurchaseOrderNew.Click += BtnPurchaseOrderNew_Click;
            // 
            // BtnPurchaseOrderExit
            // 
            BtnPurchaseOrderExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPurchaseOrderExit.Location = new Point(804, 456);
            BtnPurchaseOrderExit.Name = "BtnPurchaseOrderExit";
            BtnPurchaseOrderExit.Size = new Size(75, 23);
            BtnPurchaseOrderExit.TabIndex = 14;
            BtnPurchaseOrderExit.Text = "Exit [F10]";
            BtnPurchaseOrderExit.UseVisualStyleBackColor = true;
            BtnPurchaseOrderExit.Click += BtnPurchaseOrderExit_Click;
            // 
            // BtnPurchaseOrderPrint
            // 
            BtnPurchaseOrderPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPurchaseOrderPrint.Location = new Point(420, 456);
            BtnPurchaseOrderPrint.Name = "BtnPurchaseOrderPrint";
            BtnPurchaseOrderPrint.Size = new Size(83, 23);
            BtnPurchaseOrderPrint.TabIndex = 10;
            BtnPurchaseOrderPrint.Text = "Print [F9]";
            BtnPurchaseOrderPrint.UseVisualStyleBackColor = true;
            BtnPurchaseOrderPrint.Click += BtnPurchaseOrderPrint_Click;
            // 
            // BtnPurchaseOrderCancel
            // 
            BtnPurchaseOrderCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPurchaseOrderCancel.Location = new Point(627, 456);
            BtnPurchaseOrderCancel.Name = "BtnPurchaseOrderCancel";
            BtnPurchaseOrderCancel.Size = new Size(83, 23);
            BtnPurchaseOrderCancel.TabIndex = 9;
            BtnPurchaseOrderCancel.Text = "Cancel [Esc]";
            BtnPurchaseOrderCancel.UseVisualStyleBackColor = true;
            BtnPurchaseOrderCancel.Click += BtnPurchaseOrderCancel_Click;
            // 
            // BtnPurchaseOrderSave
            // 
            BtnPurchaseOrderSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPurchaseOrderSave.Location = new Point(716, 456);
            BtnPurchaseOrderSave.Name = "BtnPurchaseOrderSave";
            BtnPurchaseOrderSave.Size = new Size(83, 23);
            BtnPurchaseOrderSave.TabIndex = 9;
            BtnPurchaseOrderSave.Text = "Save [F8]";
            BtnPurchaseOrderSave.UseVisualStyleBackColor = true;
            BtnPurchaseOrderSave.Click += BtnPurchaseOrderSave_Click;
            // 
            // TextBoxPurchaseOrderProductName
            // 
            TextBoxPurchaseOrderProductName.BackColor = SystemColors.Window;
            TextBoxPurchaseOrderProductName.Location = new Point(336, 455);
            TextBoxPurchaseOrderProductName.MaxLength = 35;
            TextBoxPurchaseOrderProductName.Name = "TextBoxPurchaseOrderProductName";
            TextBoxPurchaseOrderProductName.Size = new Size(83, 21);
            TextBoxPurchaseOrderProductName.TabIndex = 318;
            TextBoxPurchaseOrderProductName.Visible = false;
            // 
            // TextBoxPurchaseOrderId
            // 
            TextBoxPurchaseOrderId.Location = new Point(252, 455);
            TextBoxPurchaseOrderId.Margin = new Padding(2);
            TextBoxPurchaseOrderId.Name = "TextBoxPurchaseOrderId";
            TextBoxPurchaseOrderId.Size = new Size(79, 21);
            TextBoxPurchaseOrderId.TabIndex = 317;
            TextBoxPurchaseOrderId.Visible = false;
            // 
            // BtnPurchaseOrderNewSupplier
            // 
            BtnPurchaseOrderNewSupplier.ButtonText = "New [F3]";
            BtnPurchaseOrderNewSupplier.ImageList = ImageListInvoice;
            BtnPurchaseOrderNewSupplier.Items = (System.Collections.ObjectModel.Collection<string>)resources.GetObject("BtnPurchaseOrderNewSupplier.Items");
            BtnPurchaseOrderNewSupplier.Location = new Point(292, 61);
            BtnPurchaseOrderNewSupplier.Margin = new Padding(4, 3, 4, 3);
            BtnPurchaseOrderNewSupplier.Name = "BtnPurchaseOrderNewSupplier";
            BtnPurchaseOrderNewSupplier.Size = new Size(83, 27);
            BtnPurchaseOrderNewSupplier.TabIndex = 321;
            BtnPurchaseOrderNewSupplier.TabStop = false;
            BtnPurchaseOrderNewSupplier.ItemClickedEvent += BtnPurchaseOrderNewSupplier_ItemClickedEvent;
            // 
            // ImageListInvoice
            // 
            ImageListInvoice.ColorDepth = ColorDepth.Depth8Bit;
            ImageListInvoice.ImageStream = (ImageListStreamer)resources.GetObject("ImageListInvoice.ImageStream");
            ImageListInvoice.TransparentColor = Color.Transparent;
            ImageListInvoice.Images.SetKeyName(0, "Supplier3.ico");
            ImageListInvoice.Images.SetKeyName(1, "customers.ico");
            // 
            // labelPurchaseMemo
            // 
            labelPurchaseMemo.AutoSize = true;
            labelPurchaseMemo.Location = new Point(627, 94);
            labelPurchaseMemo.Name = "labelPurchaseMemo";
            labelPurchaseMemo.Size = new Size(35, 13);
            labelPurchaseMemo.TabIndex = 322;
            labelPurchaseMemo.Text = "Memo";
            // 
            // TextBoxPurchaseMemo
            // 
            TextBoxPurchaseMemo.Location = new Point(627, 110);
            TextBoxPurchaseMemo.MaxLength = 250;
            TextBoxPurchaseMemo.Multiline = true;
            TextBoxPurchaseMemo.Name = "TextBoxPurchaseMemo";
            TextBoxPurchaseMemo.Size = new Size(262, 51);
            TextBoxPurchaseMemo.TabIndex = 7;
            TextBoxPurchaseMemo.KeyDown += TextBoxPurchaseMemo_KeyDown;
            TextBoxPurchaseMemo.PreviewKeyDown += TextBoxPurchaseMemo_PreviewKeyDown;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(PrevPurchaseOrderReferenceNumber);
            groupBox1.Location = new Point(761, 36);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(128, 56);
            groupBox1.TabIndex = 323;
            groupBox1.TabStop = false;
            groupBox1.Text = "Last Ref #";
            // 
            // PrevPurchaseOrderReferenceNumber
            // 
            PrevPurchaseOrderReferenceNumber.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            PrevPurchaseOrderReferenceNumber.Location = new Point(6, 15);
            PrevPurchaseOrderReferenceNumber.Name = "PrevPurchaseOrderReferenceNumber";
            PrevPurchaseOrderReferenceNumber.Size = new Size(107, 25);
            PrevPurchaseOrderReferenceNumber.TabIndex = 298;
            PrevPurchaseOrderReferenceNumber.Text = "000000000";
            // 
            // FormPurchaseOrder
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(895, 513);
            Controls.Add(groupBox1);
            Controls.Add(TextBoxPurchaseMemo);
            Controls.Add(labelPurchaseMemo);
            Controls.Add(groupBoxPurchaseOrderDateMethod);
            Controls.Add(BtnPurchaseOrderNewSupplier);
            Controls.Add(YesNoRadioPurchaseOrderType);
            Controls.Add(labelPurchaseOrderType);
            Controls.Add(TextBoxPurchaseOrderProductName);
            Controls.Add(TextBoxPurchaseOrderId);
            Controls.Add(BtnPurchaseOrderCreateSale);
            Controls.Add(BtnPurchaseOrderDelete);
            Controls.Add(BtnPurchaseOrderNew);
            Controls.Add(BtnPurchaseOrderExit);
            Controls.Add(BtnPurchaseOrderPrint);
            Controls.Add(BtnPurchaseOrderCancel);
            Controls.Add(BtnPurchaseOrderSave);
            Controls.Add(GridViewPurchaseOrderItemTotal);
            Controls.Add(GridViewPurchaseOrderItem);
            Controls.Add(statusStripPurchaseOrder);
            Controls.Add(groupBoxPurchaseOrderReferenceNumber);
            Controls.Add(TextBoxPurchaseOrderSupplier);
            Controls.Add(BtnPurchaseOrderSearchSupplier);
            Controls.Add(TextBoxPurchaseOrderAddress);
            Controls.Add(labelPurchaseOrderAddress);
            Controls.Add(labelPurchaseOrderSupplier);
            Controls.Add(toolStripPurchaseOrder);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormPurchaseOrder";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Purchase Order";
            FormClosing += FormPurchaseOrder_FormClosing;
            Load += FormPurchaseOrder_Load;
            Controls.SetChildIndex(toolStripPurchaseOrder, 0);
            Controls.SetChildIndex(labelPurchaseOrderSupplier, 0);
            Controls.SetChildIndex(labelPurchaseOrderAddress, 0);
            Controls.SetChildIndex(TextBoxPurchaseOrderAddress, 0);
            Controls.SetChildIndex(BtnPurchaseOrderSearchSupplier, 0);
            Controls.SetChildIndex(TextBoxPurchaseOrderSupplier, 0);
            Controls.SetChildIndex(groupBoxPurchaseOrderReferenceNumber, 0);
            Controls.SetChildIndex(statusStripPurchaseOrder, 0);
            Controls.SetChildIndex(GridViewPurchaseOrderItem, 0);
            Controls.SetChildIndex(GridViewPurchaseOrderItemTotal, 0);
            Controls.SetChildIndex(BtnPurchaseOrderSave, 0);
            Controls.SetChildIndex(BtnPurchaseOrderCancel, 0);
            Controls.SetChildIndex(BtnPurchaseOrderPrint, 0);
            Controls.SetChildIndex(BtnPurchaseOrderExit, 0);
            Controls.SetChildIndex(BtnPurchaseOrderNew, 0);
            Controls.SetChildIndex(BtnPurchaseOrderDelete, 0);
            Controls.SetChildIndex(BtnPurchaseOrderCreateSale, 0);
            Controls.SetChildIndex(TextBoxPurchaseOrderId, 0);
            Controls.SetChildIndex(TextBoxPurchaseOrderProductName, 0);
            Controls.SetChildIndex(labelPurchaseOrderType, 0);
            Controls.SetChildIndex(YesNoRadioPurchaseOrderType, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(BtnPurchaseOrderNewSupplier, 0);
            Controls.SetChildIndex(groupBoxPurchaseOrderDateMethod, 0);
            Controls.SetChildIndex(labelPurchaseMemo, 0);
            Controls.SetChildIndex(TextBoxPurchaseMemo, 0);
            Controls.SetChildIndex(groupBox1, 0);
            groupBoxPurchaseOrderReferenceNumber.ResumeLayout(false);
            groupBoxPurchaseOrderDateMethod.ResumeLayout(false);
            groupBoxPurchaseOrderDateMethod.PerformLayout();
            toolStripPurchaseOrder.ResumeLayout(false);
            toolStripPurchaseOrder.PerformLayout();
            statusStripPurchaseOrder.ResumeLayout(false);
            statusStripPurchaseOrder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewPurchaseOrderItemTotal).EndInit();
            ((System.ComponentModel.ISupportInitialize)GridViewPurchaseOrderItem).EndInit();
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private fa.views.controls.text.IDTextBox TextBoxPurchaseOrderSupplier;
        private Button BtnPurchaseOrderSearchSupplier;
        private TextBox TextBoxPurchaseOrderAddress;
        private Label labelPurchaseOrderAddress;
        private Label labelPurchaseOrderSupplier;
        private Label labelPurchaseOrderMethod;
        private fa.views.controls.YesNoRadio YesNoRbtPurchaseOrderMethod;
        private Label PurchaseOrderReferenceNumber;
        private fa.views.controls.text.DateWithCalendar DatetimePickerPurchaseOrderDate;
        private Label labelPurchaseOrderDate;
        private GroupBox groupBoxPurchaseOrderReferenceNumber;
        private GroupBox groupBoxPurchaseOrderDateMethod;
        private ToolStripLabel toolStripLabelPurchaseOrderSearch;
        private ToolStripTextBox TextBoxPurchaseOrderSearch;
        private ToolStripButton BtnPurchaseOrderSearch;
        private ToolStrip toolStripPurchaseOrder;
        private StatusStrip statusStripPurchaseOrder;
        private ToolStripStatusLabel ToolStripStatusLabelErrorPurchaseOrder;
        private DataGridView GridViewPurchaseOrderItemTotal;
        private fa.views.controls.DataViewVerticalScroll GridViewPurchaseOrderItem;
        private Button BtnPurchaseOrderCreateSale;
        private Button BtnPurchaseOrderDelete;
        private Button BtnPurchaseOrderNew;
        private Button BtnPurchaseOrderExit;
        private Button BtnPurchaseOrderPrint;
        private Button BtnPurchaseOrderCancel;
        private Button BtnPurchaseOrderSave;
        private TextBox TextBoxPurchaseOrderProductName;
        private TextBox TextBoxPurchaseOrderId;
        private fa.views.controls.YesNoRadio YesNoRadioPurchaseOrderType;
        private Label labelPurchaseOrderType;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Account;
        private DataGridViewTextBoxColumn UnitOfMeasure;
        private fa.views.controls.grid.DataGridViewQuantityColumn Column3;
        private fa.views.controls.grid.DataGridViewCurrencyColumn Column2;
        private fa.views.controls.grid.DataGridViewCurrencyColumn Column6;
        private DataGridViewButtonColumn Delete;
        private DataGridViewTextBoxColumn Column11;
        private DataGridViewCheckBoxColumn Column12;
        private DataGridViewTextBoxColumn SalesDetailID;
        private DataGridViewTextBoxColumn Column9;
        private fa.views.controls.grid.DataGridViewCurrencyColumn Column16;
        private fa.views.controls.grid.DataGridViewCurrencyColumn Column17;
        private fa.views.controls.grid.DataGridViewCurrencyColumn Column18;
        private DataGridViewTextBoxColumn Column19;
        private DataGridViewTextBoxColumn Column20;
        private DataGridViewTextBoxColumn Column21;
        private DataGridViewTextBoxColumn Column22;
        private System.Windows.Forms.Timer TimerPurchaseOrder;
        private fa.views.controls.ComboBoxSwapTextBox ComboBoxPurchaseOrderInventoryLocation;
        private Label label12;
        private Dropdown_Button.UserControlButtonWithMenu BtnPurchaseOrderNewSupplier;
        private ImageList ImageListInvoice;
        private Label labelPurchaseMemo;
        private TextBox TextBoxPurchaseMemo;
        private GroupBox groupBox1;
        private Label PrevPurchaseOrderReferenceNumber;
        private DataGridViewTextBoxColumn Total;
        private fa.views.controls.grid.DataGridViewCurrencyColumn Value;
        private DataGridViewTextBoxColumn dataGridViewButtonColumn1;
        private fa.views.controls.grid.DataGridViewCurrencyColumn QuotePrice;
        private fa.views.controls.grid.DataGridViewCurrencyColumn QuoteAmount;
    }
}