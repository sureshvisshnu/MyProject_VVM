namespace Fa.reports.sales
{
    partial class FormItemSalesReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormItemSalesReport));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            StatusStripSales = new StatusStrip();
            ItemSalesReportErrorMsg = new ToolStripStatusLabel();
            VVMatrixToolStrip = new fa.views.controls.Ab2ToolStrip();
            toolStripLabel4 = new ToolStripLabel();
            ComboBoxReportType = new ToolStripComboBox();
            VVMatrixToolStripSeparator2 = new ToolStripSeparator();
            LabelSelectItem = new ToolStripLabel();
            ComboBoxSelectCustomer = new fa.views.controls.ToolstripCheckedTreeComboBox();
            ComboBoxSelectItem = new fa.views.controls.ToolstripCheckedTreeComboBox();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripLabel1 = new ToolStripLabel();
            ComboStockReportLocation = new fa.views.controls.ToolstripCheckedTreeComboBox();
            toolStripLabel3 = new ToolStripLabel();
            StockReportFromDate = new fa.views.controls.ToolStripCalendar();
            toolStripLabel2 = new ToolStripLabel();
            StockReportToDate = new fa.views.controls.ToolStripCalendar();
            toolStripSeparator5 = new ToolStripSeparator();
            BtnGo = new ToolStripButton();
            VVMatrixToolStripSeparator1 = new ToolStripSeparator();
            ToolStripBtnsave = new ToolStripButton();
            ToolStripBtnPrint = new ToolStripButton();
            DataGridViewItemSales = new fa.views.controls.DataViewVerticalScroll();
            Column1 = new DataGridViewTextBoxColumn();
            product = new DataGridViewTextBoxColumn();
            customer = new DataGridViewTextBoxColumn();
            salesdatefrom = new DataGridViewTextBoxColumn();
            billno = new DataGridViewTextBoxColumn();
            price = new DataGridViewTextBoxColumn();
            qty = new DataGridViewTextBoxColumn();
            salesid = new DataGridViewTextBoxColumn();
            id = new DataGridViewTextBoxColumn();
            BtnExit = new Button();
            BtnCancel = new Button();
            BtnPrint = new Button();
            BtnSave = new Button();
            LabelTotalQuantity = new Label();
            LabelTotalAmount = new Label();
            LabelTotalSales = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            BtnExport = new Button();
            StatusStripSales.SuspendLayout();
            VVMatrixToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DataGridViewItemSales).BeginInit();
            SuspendLayout();
            // 
            // StatusStripSales
            // 
            StatusStripSales.Items.AddRange(new ToolStripItem[] { ItemSalesReportErrorMsg });
            StatusStripSales.Location = new Point(0, 564);
            StatusStripSales.Name = "StatusStripSales";
            StatusStripSales.Size = new Size(984, 22);
            StatusStripSales.TabIndex = 190;
            StatusStripSales.Text = "Item Sales Report Filtered";
            // 
            // ItemSalesReportErrorMsg
            // 
            ItemSalesReportErrorMsg.Name = "ItemSalesReportErrorMsg";
            ItemSalesReportErrorMsg.Size = new Size(94, 17);
            ItemSalesReportErrorMsg.Text = "                             ";
            // 
            // VVMatrixToolStrip
            // 
            VVMatrixToolStrip.BackColor = SystemColors.ControlLight;
            VVMatrixToolStrip.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            VVMatrixToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            VVMatrixToolStrip.Items.AddRange(new ToolStripItem[] { toolStripLabel4, ComboBoxReportType, VVMatrixToolStripSeparator2, LabelSelectItem, ComboBoxSelectCustomer, ComboBoxSelectItem, toolStripSeparator1, toolStripLabel1, ComboStockReportLocation, toolStripLabel3, StockReportFromDate, toolStripLabel2, StockReportToDate, toolStripSeparator5, BtnGo, VVMatrixToolStripSeparator1, ToolStripBtnsave, ToolStripBtnPrint });
            VVMatrixToolStrip.Location = new Point(0, 0);
            VVMatrixToolStrip.Name = "VVMatrixToolStrip";
            VVMatrixToolStrip.Padding = new Padding(5);
            VVMatrixToolStrip.Size = new Size(984, 38);
            VVMatrixToolStrip.TabIndex = 191;
            VVMatrixToolStrip.Text = "ab2ToolStrip1";
            // 
            // toolStripLabel4
            // 
            toolStripLabel4.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripLabel4.Name = "toolStripLabel4";
            toolStripLabel4.Size = new Size(36, 25);
            toolStripLabel4.Text = "Select";
            // 
            // ComboBoxReportType
            // 
            ComboBoxReportType.FlatStyle = FlatStyle.Standard;
            ComboBoxReportType.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ComboBoxReportType.Items.AddRange(new object[] { "By Item", "By Customer" });
            ComboBoxReportType.Name = "ComboBoxReportType";
            ComboBoxReportType.Size = new Size(125, 28);
            ComboBoxReportType.Text = "By Item";
            ComboBoxReportType.SelectedIndexChanged += ComboBoxReportType_SelectedIndexChanged;
            // 
            // VVMatrixToolStripSeparator2
            // 
            VVMatrixToolStripSeparator2.Name = "VVMatrixToolStripSeparator2";
            VVMatrixToolStripSeparator2.Size = new Size(6, 28);
            // 
            // LabelSelectItem
            // 
            LabelSelectItem.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelSelectItem.Name = "LabelSelectItem";
            LabelSelectItem.Size = new Size(29, 25);
            LabelSelectItem.Text = "Item";
            LabelSelectItem.Visible = false;
            // 
            // ComboBoxSelectCustomer
            // 
            ComboBoxSelectCustomer.AutoSize = false;
            ComboBoxSelectCustomer.Name = "ComboBoxSelectCustomer";
            ComboBoxSelectCustomer.SelectedNode = null;
            ComboBoxSelectCustomer.Size = new Size(100, 21);
            ComboBoxSelectCustomer.Visible = false;
            // 
            // ComboBoxSelectItem
            // 
            ComboBoxSelectItem.AutoSize = false;
            ComboBoxSelectItem.Name = "ComboBoxSelectItem";
            ComboBoxSelectItem.SelectedNode = null;
            ComboBoxSelectItem.Size = new Size(100, 21);
            ComboBoxSelectItem.Visible = false;
            ComboBoxSelectItem.NodeClickedEvent += ComboBoxSelectItem_NodeClickedEvent;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 28);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(53, 25);
            toolStripLabel1.Text = "Location";
            // 
            // ComboStockReportLocation
            // 
            ComboStockReportLocation.AutoSize = false;
            ComboStockReportLocation.Name = "ComboStockReportLocation";
            ComboStockReportLocation.SelectedNode = null;
            ComboStockReportLocation.Size = new Size(125, 21);
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new Size(45, 25);
            toolStripLabel3.Text = "From : ";
            // 
            // StockReportFromDate
            // 
            StockReportFromDate.AutoSize = false;
            StockReportFromDate.BackColor = Color.White;
            StockReportFromDate.Date = null;
            StockReportFromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            StockReportFromDate.Format = "MM/dd/yyyy";
            StockReportFromDate.MaxDate = new DateTime(9997, 12, 31, 7, 32, 58, 0);
            StockReportFromDate.MinDate = new DateTime(1900, 1, 1, 23, 58, 26, 0);
            StockReportFromDate.Name = "StockReportFromDate";
            StockReportFromDate.Size = new Size(85, 25);
            StockReportFromDate.Text = "toolStripCalendar1";
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(26, 25);
            toolStripLabel2.Text = "To :";
            // 
            // StockReportToDate
            // 
            StockReportToDate.AutoSize = false;
            StockReportToDate.BackColor = Color.White;
            StockReportToDate.Date = null;
            StockReportToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            StockReportToDate.Format = "MM/dd/yyyy";
            StockReportToDate.MaxDate = new DateTime(9997, 12, 31, 7, 32, 58, 0);
            StockReportToDate.MinDate = new DateTime(1900, 1, 1, 23, 58, 26, 0);
            StockReportToDate.Name = "StockReportToDate";
            StockReportToDate.Size = new Size(85, 25);
            StockReportToDate.Text = "toolStripCalendar1";
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(6, 28);
            // 
            // BtnGo
            // 
            BtnGo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnGo.Image = (Image)resources.GetObject("BtnGo.Image");
            BtnGo.ImageTransparentColor = Color.Magenta;
            BtnGo.Name = "BtnGo";
            BtnGo.Size = new Size(26, 25);
            BtnGo.Text = "Go";
            BtnGo.Click += BtnGo_Click;
            // 
            // VVMatrixToolStripSeparator1
            // 
            VVMatrixToolStripSeparator1.Name = "VVMatrixToolStripSeparator1";
            VVMatrixToolStripSeparator1.Size = new Size(6, 28);
            // 
            // ToolStripBtnsave
            // 
            ToolStripBtnsave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnsave.Image = (Image)resources.GetObject("ToolStripBtnsave.Image");
            ToolStripBtnsave.ImageTransparentColor = Color.Black;
            ToolStripBtnsave.Name = "ToolStripBtnsave";
            ToolStripBtnsave.Size = new Size(23, 25);
            ToolStripBtnsave.Text = "Save";
            // 
            // ToolStripBtnPrint
            // 
            ToolStripBtnPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnPrint.Image = (Image)resources.GetObject("ToolStripBtnPrint.Image");
            ToolStripBtnPrint.ImageTransparentColor = Color.Black;
            ToolStripBtnPrint.Name = "ToolStripBtnPrint";
            ToolStripBtnPrint.Size = new Size(23, 25);
            ToolStripBtnPrint.Text = "Print";
            // 
            // DataGridViewItemSales
            // 
            DataGridViewItemSales.AllowUserToAddRows = false;
            DataGridViewItemSales.AllowUserToDeleteRows = false;
            DataGridViewItemSales.AllowUserToResizeColumns = false;
            DataGridViewItemSales.AllowUserToResizeRows = false;
            DataGridViewItemSales.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            DataGridViewItemSales.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            DataGridViewItemSales.ColumnHeadersHeight = 20;
            DataGridViewItemSales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DataGridViewItemSales.Columns.AddRange(new DataGridViewColumn[] { Column1, product, customer, salesdatefrom, billno, price, qty, salesid, id });
            DataGridViewItemSales.EnableHeadersVisualStyles = false;
            DataGridViewItemSales.Location = new Point(10, 42);
            DataGridViewItemSales.Name = "DataGridViewItemSales";
            DataGridViewItemSales.ReadOnly = true;
            DataGridViewItemSales.RowHeadersVisible = false;
            DataGridViewItemSales.RowTemplate.Height = 20;
            DataGridViewItemSales.ScrollBars = ScrollBars.Vertical;
            DataGridViewItemSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataGridViewItemSales.ShowCellToolTips = false;
            DataGridViewItemSales.Size = new Size(965, 451);
            DataGridViewItemSales.TabIndex = 192;
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            Column1.DefaultCellStyle = dataGridViewCellStyle2;
            Column1.HeaderText = "#";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 40;
            // 
            // product
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            product.DefaultCellStyle = dataGridViewCellStyle3;
            product.HeaderText = "Product";
            product.Name = "product";
            product.ReadOnly = true;
            product.Resizable = DataGridViewTriState.False;
            product.SortMode = DataGridViewColumnSortMode.NotSortable;
            product.Width = 250;
            // 
            // customer
            // 
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            customer.DefaultCellStyle = dataGridViewCellStyle4;
            customer.HeaderText = "Customer";
            customer.Name = "customer";
            customer.ReadOnly = true;
            customer.Resizable = DataGridViewTriState.False;
            customer.SortMode = DataGridViewColumnSortMode.NotSortable;
            customer.Width = 300;
            // 
            // salesdatefrom
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            salesdatefrom.DefaultCellStyle = dataGridViewCellStyle5;
            salesdatefrom.HeaderText = "Sales Date";
            salesdatefrom.Name = "salesdatefrom";
            salesdatefrom.ReadOnly = true;
            salesdatefrom.SortMode = DataGridViewColumnSortMode.NotSortable;
            salesdatefrom.Width = 75;
            // 
            // billno
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleCenter;
            billno.DefaultCellStyle = dataGridViewCellStyle6;
            billno.HeaderText = "Bill No";
            billno.Name = "billno";
            billno.ReadOnly = true;
            billno.Width = 60;
            // 
            // price
            // 
            price.HeaderText = "Price";
            price.Name = "price";
            price.ReadOnly = true;
            price.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // qty
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleCenter;
            qty.DefaultCellStyle = dataGridViewCellStyle7;
            qty.HeaderText = "Qty";
            qty.Name = "qty";
            qty.ReadOnly = true;
            qty.Width = 120;
            // 
            // salesid
            // 
            salesid.HeaderText = "SALES ID";
            salesid.Name = "salesid";
            salesid.ReadOnly = true;
            salesid.Visible = false;
            // 
            // id
            // 
            id.HeaderText = "REPORT ID";
            id.Name = "id";
            id.ReadOnly = true;
            id.Visible = false;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(851, 529);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(72, 23);
            BtnExit.TabIndex = 196;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(601, 529);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(82, 23);
            BtnCancel.TabIndex = 195;
            BtnCancel.Text = "Reset [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnPrint
            // 
            BtnPrint.Enabled = false;
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(770, 529);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 23);
            BtnPrint.TabIndex = 194;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnSave
            // 
            BtnSave.Enabled = false;
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(689, 529);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 193;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            // 
            // LabelTotalQuantity
            // 
            LabelTotalQuantity.AutoSize = true;
            LabelTotalQuantity.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            LabelTotalQuantity.ForeColor = Color.DarkBlue;
            LabelTotalQuantity.Location = new Point(136, 509);
            LabelTotalQuantity.Name = "LabelTotalQuantity";
            LabelTotalQuantity.Size = new Size(14, 15);
            LabelTotalQuantity.TabIndex = 197;
            LabelTotalQuantity.Text = "0";
            // 
            // LabelTotalAmount
            // 
            LabelTotalAmount.AutoSize = true;
            LabelTotalAmount.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            LabelTotalAmount.ForeColor = Color.DarkBlue;
            LabelTotalAmount.Location = new Point(136, 529);
            LabelTotalAmount.Name = "LabelTotalAmount";
            LabelTotalAmount.Size = new Size(31, 15);
            LabelTotalAmount.TabIndex = 198;
            LabelTotalAmount.Text = "0.00";
            // 
            // LabelTotalSales
            // 
            LabelTotalSales.AutoSize = true;
            LabelTotalSales.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            LabelTotalSales.ForeColor = Color.DarkBlue;
            LabelTotalSales.Location = new Point(356, 509);
            LabelTotalSales.Name = "LabelTotalSales";
            LabelTotalSales.Size = new Size(14, 15);
            LabelTotalSales.TabIndex = 199;
            LabelTotalSales.Text = "0";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.FromArgb(192, 0, 0);
            label1.Location = new Point(12, 509);
            label1.Name = "label1";
            label1.Size = new Size(123, 15);
            label1.TabIndex = 200;
            label1.Text = "Total Quanttity Sold :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label2.ForeColor = Color.FromArgb(192, 0, 0);
            label2.Location = new Point(12, 529);
            label2.Name = "label2";
            label2.Size = new Size(118, 15);
            label2.TabIndex = 201;
            label2.Text = "Total Amount Sold  :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label3.ForeColor = Color.FromArgb(192, 0, 0);
            label3.Location = new Point(255, 509);
            label3.Name = "label3";
            label3.Size = new Size(100, 15);
            label3.TabIndex = 202;
            label3.Text = "Total Item Sold  :";
            // 
            // BtnExport
            // 
            BtnExport.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExport.Location = new Point(513, 529);
            BtnExport.Name = "BtnExport";
            BtnExport.Size = new Size(82, 23);
            BtnExport.TabIndex = 203;
            BtnExport.Text = "Export";
            BtnExport.UseVisualStyleBackColor = true;
            BtnExport.Click += BtnExport_Click;
            // 
            // FormItemSalesReport
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 586);
            Controls.Add(BtnExport);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(LabelTotalSales);
            Controls.Add(LabelTotalAmount);
            Controls.Add(LabelTotalQuantity);
            Controls.Add(BtnExit);
            Controls.Add(BtnCancel);
            Controls.Add(BtnPrint);
            Controls.Add(BtnSave);
            Controls.Add(DataGridViewItemSales);
            Controls.Add(VVMatrixToolStrip);
            Controls.Add(StatusStripSales);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormItemSalesReport";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Item Sales Report";
            Load += FormItemSalesReport_Load;
            StatusStripSales.ResumeLayout(false);
            StatusStripSales.PerformLayout();
            VVMatrixToolStrip.ResumeLayout(false);
            VVMatrixToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DataGridViewItemSales).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip StatusStripSales;
        private ToolStripStatusLabel ItemSalesReportErrorMsg;
        private fa.views.controls.Ab2ToolStrip VVMatrixToolStrip;
        private ToolStripLabel LabelSelectItem;
        private ToolStripSeparator VVMatrixToolStripSeparator2;
        private fa.views.controls.ToolStripComboTree ComboBoxColumns;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton BtnGo;
        private ToolStripSeparator VVMatrixToolStripSeparator1;
        private ToolStripButton ToolStripBtnsave;
        private ToolStripButton ToolStripBtnPrint;
        private fa.views.controls.DataViewVerticalScroll DataGridViewItemSales;
        private Button BtnExit;
        private Button BtnCancel;
        private Button BtnPrint;
        private Button BtnSave;
        private Label LabelTotalQuantity;
        private Label LabelTotalAmount;
        private Label LabelTotalSales;
        private fa.views.controls.ToolstripCheckedTreeComboBox ComboBoxSelectItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel toolStripLabel1;
        private fa.views.controls.ToolstripCheckedTreeComboBox ComboStockReportLocation;
        private ToolStripLabel toolStripLabel3;
        private fa.views.controls.ToolStripCalendar StockReportFromDate;
        private ToolStripLabel toolStripLabel2;
        private fa.views.controls.ToolStripCalendar StockReportToDate;
        private ToolStripSeparator toolStripSeparator5;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button BtnExport;
        private fa.views.controls.ToolstripCheckedTreeComboBox ComboBoxSelectCustomer;
        private ToolStripLabel toolStripLabel4;
        private ToolStripComboBox ComboBoxReportType;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn product;
        private DataGridViewTextBoxColumn customer;
        private DataGridViewTextBoxColumn salesdatefrom;
        private DataGridViewTextBoxColumn billno;
        private DataGridViewTextBoxColumn price;
        private DataGridViewTextBoxColumn qty;
        private DataGridViewTextBoxColumn salesid;
        private DataGridViewTextBoxColumn id;
    }
}