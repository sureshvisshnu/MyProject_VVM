namespace fa.reports.catalog
{
    partial class FormPriceList
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
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPriceList));
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            BtnExit = new Button();
            BtnCancel = new Button();
            BtnPrint = new Button();
            BtnSave = new Button();
            statusStrip1 = new StatusStrip();
            ErrorMsgItemLedger = new ToolStripStatusLabel();
            GridViewItems = new views.controls.DataViewVerticalScroll();
            ab2ToolStrip1 = new views.controls.Ab2ToolStrip();
            CheckBoxStockItem = new views.controls.ToolStripCheckBox();
            toolStripSeparator2 = new ToolStripSeparator();
            LabelCategory = new ToolStripLabel();
            ComboBoxSelectType = new ToolStripComboBox();
            BtnGo = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            ToolStripBtnsave = new ToolStripButton();
            ToolStripBtnPrint = new ToolStripButton();
            RowNumber = new DataGridViewTextBoxColumn();
            MaterialId = new DataGridViewTextBoxColumn();
            ProductName = new DataGridViewTextBoxColumn();
            RetailUOM = new DataGridViewTextBoxColumn();
            RetailPrice = new views.controls.grid.DataGridViewCurrencyColumn();
            WholeSaleUOM = new DataGridViewTextBoxColumn();
            WholesalePrice = new views.controls.grid.DataGridViewCurrencyColumn();
            msrp = new views.controls.grid.DataGridViewCurrencyColumn();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewItems).BeginInit();
            ab2ToolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(723, 401);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(72, 23);
            BtnExit.TabIndex = 17;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(473, 401);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(82, 23);
            BtnCancel.TabIndex = 16;
            BtnCancel.Text = "Reset [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Enabled = false;
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(642, 401);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 23);
            BtnPrint.TabIndex = 15;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnSave
            // 
            BtnSave.Enabled = false;
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(561, 401);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 14;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsgItemLedger });
            statusStrip1.Location = new Point(0, 431);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 10, 0);
            statusStrip1.Size = new Size(820, 22);
            statusStrip1.TabIndex = 18;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsgItemLedger
            // 
            ErrorMsgItemLedger.Name = "ErrorMsgItemLedger";
            ErrorMsgItemLedger.Size = new Size(34, 17);
            ErrorMsgItemLedger.Text = "         ";
            // 
            // GridViewItems
            // 
            GridViewItems.AllowUserToAddRows = false;
            GridViewItems.AllowUserToDeleteRows = false;
            GridViewItems.AllowUserToResizeColumns = false;
            GridViewItems.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewItems.ColumnHeadersHeight = 20;
            GridViewItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewItems.Columns.AddRange(new DataGridViewColumn[] { RowNumber, MaterialId, ProductName, RetailUOM, RetailPrice, WholeSaleUOM, WholesalePrice, msrp });
            GridViewItems.EnableHeadersVisualStyles = false;
            GridViewItems.Location = new Point(1, 33);
            GridViewItems.Name = "GridViewItems";
            GridViewItems.ReadOnly = true;
            GridViewItems.RowHeadersVisible = false;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.True;
            GridViewItems.RowsDefaultCellStyle = dataGridViewCellStyle10;
            GridViewItems.RowTemplate.Height = 20;
            GridViewItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewItems.ShowCellToolTips = false;
            GridViewItems.Size = new Size(818, 359);
            GridViewItems.TabIndex = 4;
            // 
            // ab2ToolStrip1
            // 
            ab2ToolStrip1.BackColor = SystemColors.ControlLight;
            ab2ToolStrip1.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ab2ToolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            ab2ToolStrip1.Items.AddRange(new ToolStripItem[] { CheckBoxStockItem, toolStripSeparator2, LabelCategory, ComboBoxSelectType, BtnGo, toolStripSeparator1, ToolStripBtnsave, ToolStripBtnPrint });
            ab2ToolStrip1.Location = new Point(0, 0);
            ab2ToolStrip1.Name = "ab2ToolStrip1";
            ab2ToolStrip1.Padding = new Padding(5);
            ab2ToolStrip1.Size = new Size(820, 33);
            ab2ToolStrip1.TabIndex = 3;
            ab2ToolStrip1.Text = "ab2ToolStrip1";
            // 
            // CheckBoxStockItem
            // 
            CheckBoxStockItem.BackColor = SystemColors.ControlLight;
            CheckBoxStockItem.Checked = false;
            CheckBoxStockItem.CheckState = CheckState.Unchecked;
            CheckBoxStockItem.Name = "CheckBoxStockItem";
            CheckBoxStockItem.Size = new Size(155, 20);
            CheckBoxStockItem.Text = "Show Stock Items Only";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 23);
            // 
            // LabelCategory
            // 
            LabelCategory.Name = "LabelCategory";
            LabelCategory.Size = new Size(35, 20);
            LabelCategory.Text = "Type";
            // 
            // ComboBoxSelectType
            // 
            ComboBoxSelectType.FlatStyle = FlatStyle.Standard;
            ComboBoxSelectType.Items.AddRange(new object[] { "Retail", "Wholesale" });
            ComboBoxSelectType.Name = "ComboBoxSelectType";
            ComboBoxSelectType.Size = new Size(121, 23);
            ComboBoxSelectType.SelectedIndexChanged += ComboBoxSelectType_SelectedIndexChanged;
            // 
            // BtnGo
            // 
            BtnGo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnGo.Image = (Image)resources.GetObject("BtnGo.Image");
            BtnGo.ImageTransparentColor = Color.Magenta;
            BtnGo.Name = "BtnGo";
            BtnGo.Size = new Size(26, 20);
            BtnGo.Text = "Go";
            BtnGo.Click += BtnGo_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 23);
            // 
            // ToolStripBtnsave
            // 
            ToolStripBtnsave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnsave.Image = (Image)resources.GetObject("ToolStripBtnsave.Image");
            ToolStripBtnsave.ImageTransparentColor = Color.Black;
            ToolStripBtnsave.Name = "ToolStripBtnsave";
            ToolStripBtnsave.Size = new Size(23, 20);
            ToolStripBtnsave.Text = "Save";
            ToolStripBtnsave.Click += BtnSave_Click;
            // 
            // ToolStripBtnPrint
            // 
            ToolStripBtnPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnPrint.Image = (Image)resources.GetObject("ToolStripBtnPrint.Image");
            ToolStripBtnPrint.ImageTransparentColor = Color.Black;
            ToolStripBtnPrint.Name = "ToolStripBtnPrint";
            ToolStripBtnPrint.Size = new Size(23, 20);
            ToolStripBtnPrint.Text = "Print";
            ToolStripBtnPrint.Click += BtnPrint_Click;
            // 
            // RowNumber
            // 
            dataGridViewCellStyle2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            RowNumber.DefaultCellStyle = dataGridViewCellStyle2;
            RowNumber.Frozen = true;
            RowNumber.HeaderText = "#";
            RowNumber.Name = "RowNumber";
            RowNumber.ReadOnly = true;
            RowNumber.SortMode = DataGridViewColumnSortMode.NotSortable;
            RowNumber.Width = 50;
            // 
            // MaterialId
            // 
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            MaterialId.DefaultCellStyle = dataGridViewCellStyle3;
            MaterialId.Frozen = true;
            MaterialId.HeaderText = "Product Code";
            MaterialId.Name = "MaterialId";
            MaterialId.ReadOnly = true;
            MaterialId.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // ProductName
            // 
            dataGridViewCellStyle4.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ProductName.DefaultCellStyle = dataGridViewCellStyle4;
            ProductName.Frozen = true;
            ProductName.HeaderText = "Product Name";
            ProductName.Name = "ProductName";
            ProductName.ReadOnly = true;
            ProductName.SortMode = DataGridViewColumnSortMode.NotSortable;
            ProductName.Width = 243;
            // 
            // RetailUOM
            // 
            dataGridViewCellStyle5.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            RetailUOM.DefaultCellStyle = dataGridViewCellStyle5;
            RetailUOM.HeaderText = "Retail UOM";
            RetailUOM.Name = "RetailUOM";
            RetailUOM.ReadOnly = true;
            RetailUOM.SortMode = DataGridViewColumnSortMode.NotSortable;
            RetailUOM.Width = 75;
            // 
            // RetailPrice
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            RetailPrice.DefaultCellStyle = dataGridViewCellStyle6;
            RetailPrice.HeaderText = "Retail Price";
            RetailPrice.Name = "RetailPrice";
            RetailPrice.ReadOnly = true;
            RetailPrice.Resizable = DataGridViewTriState.False;
            RetailPrice.Width = 75;
            // 
            // WholeSaleUOM
            // 
            dataGridViewCellStyle7.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            WholeSaleUOM.DefaultCellStyle = dataGridViewCellStyle7;
            WholeSaleUOM.HeaderText = "Wholsale UOM";
            WholeSaleUOM.Name = "WholeSaleUOM";
            WholeSaleUOM.ReadOnly = true;
            WholeSaleUOM.SortMode = DataGridViewColumnSortMode.NotSortable;
            WholeSaleUOM.Width = 85;
            // 
            // WholesalePrice
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            WholesalePrice.DefaultCellStyle = dataGridViewCellStyle8;
            WholesalePrice.HeaderText = "Wholsale Price";
            WholesalePrice.Name = "WholesalePrice";
            WholesalePrice.ReadOnly = true;
            WholesalePrice.Resizable = DataGridViewTriState.False;
            WholesalePrice.Width = 85;
            // 
            // msrp
            // 
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            msrp.DefaultCellStyle = dataGridViewCellStyle9;
            msrp.HeaderText = "MSRP";
            msrp.Name = "msrp";
            msrp.ReadOnly = true;
            msrp.Resizable = DataGridViewTriState.False;
            msrp.Width = 78;
            // 
            // FormPriceList
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(820, 453);
            Controls.Add(statusStrip1);
            Controls.Add(BtnExit);
            Controls.Add(BtnCancel);
            Controls.Add(BtnPrint);
            Controls.Add(BtnSave);
            Controls.Add(GridViewItems);
            Controls.Add(ab2ToolStrip1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormPriceList";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Price List";
            Load += FormPriceList_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewItems).EndInit();
            ab2ToolStrip1.ResumeLayout(false);
            ab2ToolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private views.controls.Ab2ToolStrip ab2ToolStrip1;
        private System.Windows.Forms.ToolStripLabel LabelCategory;
        private System.Windows.Forms.ToolStripButton BtnGo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ToolStripBtnsave;
        private System.Windows.Forms.ToolStripButton ToolStripBtnPrint;
        private views.controls.DataViewVerticalScroll GridViewItems;
        private views.controls.ToolStripCheckBox CheckBoxStockItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnPrint;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.ToolStripComboBox ComboBoxSelectType;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ErrorMsgItemLedger;
        private DataGridViewTextBoxColumn RowNumber;
        private DataGridViewTextBoxColumn MaterialId;
        private DataGridViewTextBoxColumn ProductName;
        private DataGridViewTextBoxColumn RetailUOM;
        private views.controls.grid.DataGridViewCurrencyColumn RetailPrice;
        private DataGridViewTextBoxColumn WholeSaleUOM;
        private views.controls.grid.DataGridViewCurrencyColumn WholesalePrice;
        private views.controls.grid.DataGridViewCurrencyColumn msrp;
    }
}