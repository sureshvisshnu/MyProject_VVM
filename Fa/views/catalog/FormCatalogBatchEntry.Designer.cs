namespace fa.views.catalog
{
    partial class FormCatalogBatchEntry
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
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCatalogBatchEntry));
            BatchGrid = new controls.DataViewVerticalScroll();
            BtnSave = new Button();
            BtnExit = new Button();
            BtnCancel = new Button();
            statusStrip1 = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            label1 = new Label();
            TextBoxOpeningStock = new controls.text.CurrencyTextBox();
            Column9 = new DataGridViewTextBoxColumn();
            Column17 = new DataGridViewComboBoxColumn();
            Column3 = new controls.grid.DataGridViewQuantityColumn();
            Column19 = new DataGridViewComboBoxColumn();
            AsOf = new controls.grid.DataGridViewCalendarColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new controls.grid.DataGridViewCalendarColumn();
            PurchasePrice = new controls.grid.DataGridViewCurrencyColumn();
            Cost = new controls.grid.DataGridViewCurrencyColumn();
            RetailPrice = new controls.grid.DataGridViewCurrencyColumn();
            WholeSalePrice = new controls.grid.DataGridViewCurrencyColumn();
            Msrp = new controls.grid.DataGridViewCurrencyColumn();
            Column13 = new DataGridViewTextBoxColumn();
            Column14 = new controls.grid.DataGridViewNumberColumn();
            Column15 = new DataGridViewTextBoxColumn();
            Column16 = new controls.grid.DataGridViewNumberColumn();
            Column12 = new DataGridViewButtonColumn();
            Column10 = new DataGridViewButtonColumn();
            Column11 = new DataGridViewButtonColumn();
            Column18 = new DataGridViewTextBoxColumn();
            Column20 = new DataGridViewTextBoxColumn();
            Column21 = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)BatchGrid).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(189, 450);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(189, 424);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(189, 398);
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // BatchGrid
            // 
            BatchGrid.AllowUserToDeleteRows = false;
            BatchGrid.AllowUserToResizeColumns = false;
            BatchGrid.AllowUserToResizeRows = false;
            BatchGrid.BackgroundColor = SystemColors.Control;
            BatchGrid.BorderStyle = BorderStyle.Fixed3D;
            BatchGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            BatchGrid.Columns.AddRange(new DataGridViewColumn[] { Column9, Column17, Column3, Column19, AsOf, Column1, Column2, PurchasePrice, Cost, RetailPrice, WholeSalePrice, Msrp, Column13, Column14, Column15, Column16, Column12, Column10, Column11, Column18, Column20, Column21 });
            BatchGrid.EditMode = DataGridViewEditMode.EditOnEnter;
            BatchGrid.EnableHeadersVisualStyles = false;
            BatchGrid.Location = new Point(0, -1);
            BatchGrid.Name = "BatchGrid";
            BatchGrid.RowHeadersVisible = false;
            dataGridViewCellStyle10.BackColor = Color.White;
            dataGridViewCellStyle10.ForeColor = Color.Black;
            dataGridViewCellStyle10.SelectionBackColor = Color.White;
            dataGridViewCellStyle10.SelectionForeColor = Color.Black;
            BatchGrid.RowsDefaultCellStyle = dataGridViewCellStyle10;
            BatchGrid.RowTemplate.Height = 20;
            BatchGrid.ShowCellToolTips = false;
            BatchGrid.Size = new Size(1254, 214);
            BatchGrid.TabIndex = 0;
            BatchGrid.CellClick += BatchGrid_CellClick;
            BatchGrid.CellEndEdit += BatchGrid_CellEndEdit;
            BatchGrid.CellEnter += BatchGrid_CellEnter;
            BatchGrid.DataError += BatchGrid_DataError;
            BatchGrid.EditingControlShowing += BatchGrid_EditingControlShowing;
            BatchGrid.RowsAdded += BatchGrid_RowsAdded;
            // 
            // BtnSave
            // 
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(1075, 226);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 1;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            BtnSave.PreviewKeyDown += BtnSave_PreviewKeyDown;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(1158, 226);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(75, 23);
            BtnExit.TabIndex = 2;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(993, 226);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 23);
            BtnCancel.TabIndex = 3;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsg });
            statusStrip1.Location = new Point(0, 260);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1254, 22);
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(34, 17);
            ErrorMsg.Text = "         ";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(9, 231);
            label1.Name = "label1";
            label1.Size = new Size(52, 13);
            label1.TabIndex = 5;
            label1.Text = "Total Qty";
            // 
            // TextBoxOpeningStock
            // 
            TextBoxOpeningStock.BackColor = Color.White;
            TextBoxOpeningStock.Decimals = 2;
            TextBoxOpeningStock.Length = 10;
            TextBoxOpeningStock.Location = new Point(65, 228);
            TextBoxOpeningStock.Name = "TextBoxOpeningStock";
            TextBoxOpeningStock.ReadOnly = true;
            TextBoxOpeningStock.Size = new Size(100, 21);
            TextBoxOpeningStock.TabIndex = 7;
            TextBoxOpeningStock.TabStop = false;
            TextBoxOpeningStock.Text = "0.00";
            TextBoxOpeningStock.TextAlign = HorizontalAlignment.Right;
            // 
            // Column9
            // 
            Column9.HeaderText = "#";
            Column9.Name = "Column9";
            Column9.Resizable = DataGridViewTriState.False;
            Column9.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column9.Width = 30;
            // 
            // Column17
            // 
            Column17.FlatStyle = FlatStyle.Flat;
            Column17.HeaderText = "Location";
            Column17.Name = "Column17";
            Column17.Width = 110;
            // 
            // Column3
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.NullValue = "0";
            Column3.DefaultCellStyle = dataGridViewCellStyle1;
            Column3.HeaderText = "QTY";
            Column3.Name = "Column3";
            Column3.Resizable = DataGridViewTriState.False;
            Column3.Width = 50;
            // 
            // Column19
            // 
            Column19.FlatStyle = FlatStyle.Flat;
            Column19.HeaderText = "Stock UOM";
            Column19.Name = "Column19";
            Column19.Width = 70;
            // 
            // AsOf
            // 
            AsOf.HeaderText = "As of";
            AsOf.Name = "AsOf";
            AsOf.Width = 80;
            // 
            // Column1
            // 
            Column1.HeaderText = "Batch No";
            Column1.MaxInputLength = 10;
            Column1.Name = "Column1";
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 90;
            // 
            // Column2
            // 
            Column2.HeaderText = "Expiry Date";
            Column2.Name = "Column2";
            Column2.Resizable = DataGridViewTriState.False;
            Column2.Width = 80;
            // 
            // PurchasePrice
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.NullValue = "0.00";
            PurchasePrice.DefaultCellStyle = dataGridViewCellStyle2;
            PurchasePrice.HeaderText = "Purchase Price";
            PurchasePrice.Name = "PurchasePrice";
            PurchasePrice.Resizable = DataGridViewTriState.False;
            PurchasePrice.Width = 80;
            // 
            // Cost
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.NullValue = "0.00";
            Cost.DefaultCellStyle = dataGridViewCellStyle3;
            Cost.HeaderText = "Cost";
            Cost.Name = "Cost";
            Cost.Resizable = DataGridViewTriState.False;
            Cost.Width = 75;
            // 
            // RetailPrice
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.NullValue = "0.00";
            RetailPrice.DefaultCellStyle = dataGridViewCellStyle4;
            RetailPrice.HeaderText = "Retail Price";
            RetailPrice.Name = "RetailPrice";
            RetailPrice.Resizable = DataGridViewTriState.False;
            RetailPrice.Width = 80;
            // 
            // WholeSalePrice
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.NullValue = "0.00";
            WholeSalePrice.DefaultCellStyle = dataGridViewCellStyle5;
            WholeSalePrice.HeaderText = "Wholesale Price";
            WholeSalePrice.Name = "WholeSalePrice";
            WholeSalePrice.Resizable = DataGridViewTriState.False;
            WholeSalePrice.Width = 80;
            // 
            // Msrp
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.NullValue = "0.00";
            Msrp.DefaultCellStyle = dataGridViewCellStyle6;
            Msrp.HeaderText = "MSRP";
            Msrp.Name = "Msrp";
            Msrp.Resizable = DataGridViewTriState.False;
            Msrp.Width = 80;
            // 
            // Column13
            // 
            Column13.HeaderText = "Retail UOM";
            Column13.MaxInputLength = 30;
            Column13.Name = "Column13";
            Column13.Resizable = DataGridViewTriState.False;
            Column13.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column13.Width = 60;
            // 
            // Column14
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.NullValue = "0";
            Column14.DefaultCellStyle = dataGridViewCellStyle7;
            Column14.HeaderText = "Retail X-Factor";
            Column14.Name = "Column14";
            Column14.NumberLength = 5;
            Column14.Resizable = DataGridViewTriState.False;
            Column14.Width = 50;
            // 
            // Column15
            // 
            Column15.HeaderText = "Wholesale UOM";
            Column15.MaxInputLength = 30;
            Column15.Name = "Column15";
            Column15.Resizable = DataGridViewTriState.False;
            Column15.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column15.Width = 60;
            // 
            // Column16
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.NullValue = "0";
            Column16.DefaultCellStyle = dataGridViewCellStyle8;
            Column16.HeaderText = "Wholesale X-Factor";
            Column16.Name = "Column16";
            Column16.NumberLength = 5;
            Column16.Resizable = DataGridViewTriState.False;
            Column16.Width = 60;
            // 
            // Column12
            // 
            Column12.HeaderText = "QR Code";
            Column12.Name = "Column12";
            Column12.Width = 70;
            // 
            // Column10
            // 
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.NullValue = "X";
            Column10.DefaultCellStyle = dataGridViewCellStyle9;
            Column10.HeaderText = "...";
            Column10.Name = "Column10";
            Column10.Resizable = DataGridViewTriState.False;
            Column10.Width = 25;
            // 
            // Column11
            // 
            Column11.HeaderText = "BatchId";
            Column11.Name = "Column11";
            Column11.Resizable = DataGridViewTriState.False;
            Column11.Visible = false;
            // 
            // Column18
            // 
            Column18.HeaderText = "InventoryId";
            Column18.Name = "Column18";
            Column18.Visible = false;
            // 
            // Column20
            // 
            Column20.HeaderText = "DetailID";
            Column20.Name = "Column20";
            Column20.Visible = false;
            // 
            // Column21
            // 
            Column21.HeaderText = "LocationID";
            Column21.Name = "Column21";
            Column21.Visible = false;
            // 
            // FormCatalogBatchEntry
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1254, 282);
            Controls.Add(TextBoxOpeningStock);
            Controls.Add(label1);
            Controls.Add(statusStrip1);
            Controls.Add(BtnCancel);
            Controls.Add(BtnExit);
            Controls.Add(BtnSave);
            Controls.Add(BatchGrid);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormCatalogBatchEntry";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Opening Stock";
            FormClosing += FormCatalogBatchEntry_FormClosing;
            Load += FormCatalogBatchEntry_Load;
            Leave += BtnExit_Click;
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(BatchGrid, 0);
            Controls.SetChildIndex(BtnSave, 0);
            Controls.SetChildIndex(BtnExit, 0);
            Controls.SetChildIndex(BtnCancel, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(TextBoxOpeningStock, 0);
            ((System.ComponentModel.ISupportInitialize)BatchGrid).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private controls.DataViewVerticalScroll BatchGrid;
        private Button BtnSave;
        private Button BtnExit;
        private Button BtnCancel;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel ErrorMsg;
        private Label label1;
        private controls.text.CurrencyTextBox TextBoxOpeningStock;
        private controls.grid.DataGridViewCurrencyColumn Column4;
        private controls.grid.DataGridViewCurrencyColumn Column5;
        private controls.grid.DataGridViewCurrencyColumn Column6;
        private controls.grid.DataGridViewCurrencyColumn Column7;
        private controls.grid.DataGridViewCurrencyColumn Column8;
        private DataGridViewTextBoxColumn Column9;
        private DataGridViewComboBoxColumn Column17;
        private controls.grid.DataGridViewQuantityColumn Column3;
        private DataGridViewComboBoxColumn Column19;
        private controls.grid.DataGridViewCalendarColumn AsOf;
        private DataGridViewTextBoxColumn Column1;
        private controls.grid.DataGridViewCalendarColumn Column2;
        private controls.grid.DataGridViewCurrencyColumn PurchasePrice;
        private controls.grid.DataGridViewCurrencyColumn Cost;
        private controls.grid.DataGridViewCurrencyColumn RetailPrice;
        private controls.grid.DataGridViewCurrencyColumn WholeSalePrice;
        private controls.grid.DataGridViewCurrencyColumn Msrp;
        private DataGridViewTextBoxColumn Column13;
        private controls.grid.DataGridViewNumberColumn Column14;
        private DataGridViewTextBoxColumn Column15;
        private controls.grid.DataGridViewNumberColumn Column16;
        private DataGridViewButtonColumn Column12;
        private DataGridViewButtonColumn Column10;
        private DataGridViewButtonColumn Column11;
        private DataGridViewTextBoxColumn Column18;
        private DataGridViewTextBoxColumn Column20;
        private DataGridViewTextBoxColumn Column21;
    }
}