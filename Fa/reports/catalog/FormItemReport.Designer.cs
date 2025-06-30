namespace fa.reports.catalog
{
    partial class FormItemReport
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
            DataGridViewCellStyle dataGridViewCellStyle14 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
            views.controls.ComboTreeView.ComboTreeNode comboTreeNode1 = new views.controls.ComboTreeView.ComboTreeNode();
            views.controls.ComboTreeView.ComboTreeNode comboTreeNode2 = new views.controls.ComboTreeView.ComboTreeNode();
            views.controls.ComboTreeView.ComboTreeNode comboTreeNode3 = new views.controls.ComboTreeView.ComboTreeNode();
            views.controls.ComboTreeView.ComboTreeNode comboTreeNode4 = new views.controls.ComboTreeView.ComboTreeNode();
            views.controls.ComboTreeView.ComboTreeNode comboTreeNode5 = new views.controls.ComboTreeView.ComboTreeNode();
            views.controls.ComboTreeView.ComboTreeNode comboTreeNode6 = new views.controls.ComboTreeView.ComboTreeNode();
            views.controls.ComboTreeView.ComboTreeNode comboTreeNode7 = new views.controls.ComboTreeView.ComboTreeNode();
            views.controls.ComboTreeView.ComboTreeNode comboTreeNode8 = new views.controls.ComboTreeView.ComboTreeNode();
            views.controls.ComboTreeView.ComboTreeNode comboTreeNode9 = new views.controls.ComboTreeView.ComboTreeNode();
            views.controls.ComboTreeView.ComboTreeNode comboTreeNode10 = new views.controls.ComboTreeView.ComboTreeNode();
            views.controls.ComboTreeView.ComboTreeNode comboTreeNode11 = new views.controls.ComboTreeView.ComboTreeNode();
            views.controls.ComboTreeView.ComboTreeNode comboTreeNode12 = new views.controls.ComboTreeView.ComboTreeNode();
            views.controls.ComboTreeView.ComboTreeNode comboTreeNode13 = new views.controls.ComboTreeView.ComboTreeNode();
            views.controls.ComboTreeView.ComboTreeNode comboTreeNode14 = new views.controls.ComboTreeView.ComboTreeNode();
            views.controls.ComboTreeView.ComboTreeNode comboTreeNode15 = new views.controls.ComboTreeView.ComboTreeNode();
            views.controls.ComboTreeView.ComboTreeNode comboTreeNode16 = new views.controls.ComboTreeView.ComboTreeNode();
            views.controls.ComboTreeView.ComboTreeNode comboTreeNode17 = new views.controls.ComboTreeView.ComboTreeNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormItemReport));
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
            BtnExit = new Button();
            BtnCancel = new Button();
            BtnPrint = new Button();
            BtnSave = new Button();
            statusStrip1 = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            GridViewItems = new views.controls.DataViewVerticalScroll();
            ab2ToolStrip1 = new views.controls.Ab2ToolStrip();
            LabelCategory = new ToolStripLabel();
            ComboBoxCategory = new views.controls.ToolstripCheckedTreeComboBox();
            toolStripSeparator2 = new ToolStripSeparator();
            toolStripLabel1 = new ToolStripLabel();
            ComboBoxColumns = new views.controls.ToolStripComboTree();
            toolStripSeparator3 = new ToolStripSeparator();
            BtnGo = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            ToolStripBtnsave = new ToolStripButton();
            ToolStripBtnPrint = new ToolStripButton();
            RowNumber = new DataGridViewTextBoxColumn();
            MaterialId = new DataGridViewTextBoxColumn();
            ProductName = new DataGridViewTextBoxColumn();
            PurchaseUOM = new DataGridViewTextBoxColumn();
            RetainXFactor = new DataGridViewTextBoxColumn();
            RetailUOM = new DataGridViewTextBoxColumn();
            WholesaleXFactor = new DataGridViewTextBoxColumn();
            WholeSaleUOM = new DataGridViewTextBoxColumn();
            PurchasePrice = new DataGridViewTextBoxColumn();
            Cost = new DataGridViewTextBoxColumn();
            RetailPrice = new DataGridViewTextBoxColumn();
            WholesalePrice = new DataGridViewTextBoxColumn();
            msrp = new DataGridViewTextBoxColumn();
            OpStock = new DataGridViewTextBoxColumn();
            PurchaseQty = new DataGridViewTextBoxColumn();
            SaleQty = new DataGridViewTextBoxColumn();
            ClosingQty = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewItems).BeginInit();
            ab2ToolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(1169, 489);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(72, 23);
            BtnExit.TabIndex = 13;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            BtnExit.PreviewKeyDown += BtnExit_PreviewKeyDown;
            // 
            // BtnCancel
            // 
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(919, 489);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(82, 23);
            BtnCancel.TabIndex = 12;
            BtnCancel.Text = "Reset [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Enabled = false;
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(1088, 489);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 23);
            BtnPrint.TabIndex = 11;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnSave
            // 
            BtnSave.Enabled = false;
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(1007, 489);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 10;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsg });
            statusStrip1.Location = new Point(0, 517);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1323, 22);
            statusStrip1.TabIndex = 14;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(100, 17);
            ErrorMsg.Text = "                               ";
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
            GridViewItems.Columns.AddRange(new DataGridViewColumn[] { RowNumber, MaterialId, ProductName, PurchaseUOM, RetainXFactor, RetailUOM, WholesaleXFactor, WholeSaleUOM, PurchasePrice, Cost, RetailPrice, WholesalePrice, msrp, OpStock, PurchaseQty, SaleQty, ClosingQty, Column1, Column2 });
            dataGridViewCellStyle14.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = SystemColors.Window;
            dataGridViewCellStyle14.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle14.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = DataGridViewTriState.False;
            GridViewItems.DefaultCellStyle = dataGridViewCellStyle14;
            GridViewItems.EnableHeadersVisualStyles = false;
            GridViewItems.Location = new Point(4, 42);
            GridViewItems.Name = "GridViewItems";
            GridViewItems.ReadOnly = true;
            GridViewItems.RowHeadersVisible = false;
            dataGridViewCellStyle15.WrapMode = DataGridViewTriState.True;
            GridViewItems.RowsDefaultCellStyle = dataGridViewCellStyle15;
            GridViewItems.RowTemplate.Height = 18;
            GridViewItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewItems.ShowCellToolTips = false;
            GridViewItems.Size = new Size(1315, 441);
            GridViewItems.TabIndex = 3;
            GridViewItems.CellFormatting += GridViewItems_CellFormatting;
            GridViewItems.CellPainting += GridViewItems_CellPainting;
            GridViewItems.RowPostPaint += GridViewItems_RowPostPaint;
            // 
            // ab2ToolStrip1
            // 
            ab2ToolStrip1.BackColor = SystemColors.ControlLight;
            ab2ToolStrip1.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ab2ToolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            ab2ToolStrip1.Items.AddRange(new ToolStripItem[] { LabelCategory, ComboBoxCategory, toolStripSeparator2, toolStripLabel1, ComboBoxColumns, toolStripSeparator3, BtnGo, toolStripSeparator1, ToolStripBtnsave, ToolStripBtnPrint });
            ab2ToolStrip1.Location = new Point(0, 0);
            ab2ToolStrip1.Name = "ab2ToolStrip1";
            ab2ToolStrip1.Padding = new Padding(5);
            ab2ToolStrip1.Size = new Size(1323, 39);
            ab2ToolStrip1.TabIndex = 2;
            ab2ToolStrip1.Text = "ab2ToolStrip1";
            ab2ToolStrip1.ItemClicked += ab2ToolStrip1_ItemClicked;
            // 
            // LabelCategory
            // 
            LabelCategory.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelCategory.Name = "LabelCategory";
            LabelCategory.Size = new Size(52, 26);
            LabelCategory.Text = "Category";
            // 
            // ComboBoxCategory
            // 
            ComboBoxCategory.AutoSize = false;
            ComboBoxCategory.Name = "ComboBoxCategory";
            ComboBoxCategory.SelectedNode = null;
            ComboBoxCategory.Size = new Size(175, 26);
            ComboBoxCategory.NodeClickedEvent += ComboBoxCategory_NodeClickedEvent;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 29);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(47, 26);
            toolStripLabel1.Text = "Columns";
            // 
            // ComboBoxColumns
            // 
            ComboBoxColumns.BackColor = Color.White;
            ComboBoxColumns.Name = "ComboBoxColumns";
            comboTreeNode1.Expanded = false;
            comboTreeNode1.Name = "0";
            comboTreeNode1.Tag = null;
            comboTreeNode1.Text = "Serial Number";
            comboTreeNode1.ToolTip = null;
            comboTreeNode2.Expanded = false;
            comboTreeNode2.Name = "1";
            comboTreeNode2.Tag = null;
            comboTreeNode2.Text = "Product Code";
            comboTreeNode2.ToolTip = null;
            comboTreeNode3.Expanded = false;
            comboTreeNode3.Name = "2";
            comboTreeNode3.Tag = null;
            comboTreeNode3.Text = "Product Name";
            comboTreeNode3.ToolTip = null;
            comboTreeNode4.Expanded = false;
            comboTreeNode4.Name = "3";
            comboTreeNode4.Tag = null;
            comboTreeNode4.Text = "Product UOM";
            comboTreeNode4.ToolTip = null;
            comboTreeNode5.Expanded = false;
            comboTreeNode5.Name = "4";
            comboTreeNode5.Tag = null;
            comboTreeNode5.Text = "Retail X Factor";
            comboTreeNode5.ToolTip = null;
            comboTreeNode6.Expanded = false;
            comboTreeNode6.Name = "5";
            comboTreeNode6.Tag = null;
            comboTreeNode6.Text = "Retail UOM";
            comboTreeNode6.ToolTip = null;
            comboTreeNode7.Expanded = false;
            comboTreeNode7.Name = "6";
            comboTreeNode7.Tag = null;
            comboTreeNode7.Text = "Whole Sale X Factor";
            comboTreeNode7.ToolTip = null;
            comboTreeNode8.Expanded = false;
            comboTreeNode8.Name = "7";
            comboTreeNode8.Tag = null;
            comboTreeNode8.Text = "Whole Sale UOM";
            comboTreeNode8.ToolTip = null;
            comboTreeNode9.Expanded = false;
            comboTreeNode9.Name = "8";
            comboTreeNode9.Tag = null;
            comboTreeNode9.Text = "Purchase Price";
            comboTreeNode9.ToolTip = null;
            comboTreeNode10.Expanded = false;
            comboTreeNode10.Name = "9";
            comboTreeNode10.Tag = null;
            comboTreeNode10.Text = "Cost";
            comboTreeNode10.ToolTip = null;
            comboTreeNode11.Expanded = false;
            comboTreeNode11.Name = "10";
            comboTreeNode11.Tag = null;
            comboTreeNode11.Text = "Retail Price";
            comboTreeNode11.ToolTip = null;
            comboTreeNode12.Expanded = false;
            comboTreeNode12.Name = "11";
            comboTreeNode12.Tag = null;
            comboTreeNode12.Text = "Whole Sale Price";
            comboTreeNode12.ToolTip = null;
            comboTreeNode13.Expanded = false;
            comboTreeNode13.Name = "12";
            comboTreeNode13.Tag = null;
            comboTreeNode13.Text = "MSRP";
            comboTreeNode13.ToolTip = null;
            comboTreeNode14.Expanded = false;
            comboTreeNode14.Name = "13";
            comboTreeNode14.Tag = null;
            comboTreeNode14.Text = "Open Stock";
            comboTreeNode14.ToolTip = null;
            comboTreeNode15.Expanded = false;
            comboTreeNode15.Name = "14";
            comboTreeNode15.Tag = null;
            comboTreeNode15.Text = "Purchase Quantity";
            comboTreeNode15.ToolTip = null;
            comboTreeNode16.Expanded = false;
            comboTreeNode16.Name = "15";
            comboTreeNode16.Tag = null;
            comboTreeNode16.Text = "Sales Quantity";
            comboTreeNode16.ToolTip = null;
            comboTreeNode17.Expanded = false;
            comboTreeNode17.Name = "16";
            comboTreeNode17.Tag = null;
            comboTreeNode17.Text = "Current Quantity";
            comboTreeNode17.ToolTip = null;
            ComboBoxColumns.Nodes.Add(comboTreeNode1);
            ComboBoxColumns.Nodes.Add(comboTreeNode2);
            ComboBoxColumns.Nodes.Add(comboTreeNode3);
            ComboBoxColumns.Nodes.Add(comboTreeNode4);
            ComboBoxColumns.Nodes.Add(comboTreeNode5);
            ComboBoxColumns.Nodes.Add(comboTreeNode6);
            ComboBoxColumns.Nodes.Add(comboTreeNode7);
            ComboBoxColumns.Nodes.Add(comboTreeNode8);
            ComboBoxColumns.Nodes.Add(comboTreeNode9);
            ComboBoxColumns.Nodes.Add(comboTreeNode10);
            ComboBoxColumns.Nodes.Add(comboTreeNode11);
            ComboBoxColumns.Nodes.Add(comboTreeNode12);
            ComboBoxColumns.Nodes.Add(comboTreeNode13);
            ComboBoxColumns.Nodes.Add(comboTreeNode14);
            ComboBoxColumns.Nodes.Add(comboTreeNode15);
            ComboBoxColumns.Nodes.Add(comboTreeNode16);
            ComboBoxColumns.Nodes.Add(comboTreeNode17);
            ComboBoxColumns.SelectedNode = null;
            ComboBoxColumns.Size = new Size(175, 26);
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 29);
            // 
            // BtnGo
            // 
            BtnGo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnGo.Image = (Image)resources.GetObject("BtnGo.Image");
            BtnGo.ImageTransparentColor = Color.Magenta;
            BtnGo.Name = "BtnGo";
            BtnGo.Size = new Size(26, 26);
            BtnGo.Text = "Go";
            BtnGo.Click += BtnGo_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 29);
            // 
            // ToolStripBtnsave
            // 
            ToolStripBtnsave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnsave.Image = (Image)resources.GetObject("ToolStripBtnsave.Image");
            ToolStripBtnsave.ImageTransparentColor = Color.Black;
            ToolStripBtnsave.Name = "ToolStripBtnsave";
            ToolStripBtnsave.Size = new Size(23, 26);
            ToolStripBtnsave.Text = "Save";
            ToolStripBtnsave.Click += BtnSave_Click;
            // 
            // ToolStripBtnPrint
            // 
            ToolStripBtnPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnPrint.Image = (Image)resources.GetObject("ToolStripBtnPrint.Image");
            ToolStripBtnPrint.ImageTransparentColor = Color.Black;
            ToolStripBtnPrint.Name = "ToolStripBtnPrint";
            ToolStripBtnPrint.Size = new Size(23, 26);
            ToolStripBtnPrint.Text = "Print";
            ToolStripBtnPrint.Click += BtnPrint_Click;
            // 
            // RowNumber
            // 
            RowNumber.Frozen = true;
            RowNumber.HeaderText = "#";
            RowNumber.Name = "RowNumber";
            RowNumber.ReadOnly = true;
            RowNumber.Resizable = DataGridViewTriState.False;
            RowNumber.SortMode = DataGridViewColumnSortMode.NotSortable;
            RowNumber.Width = 50;
            // 
            // MaterialId
            // 
            MaterialId.Frozen = true;
            MaterialId.HeaderText = "Product Code";
            MaterialId.Name = "MaterialId";
            MaterialId.ReadOnly = true;
            MaterialId.Resizable = DataGridViewTriState.False;
            MaterialId.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // ProductName
            // 
            ProductName.Frozen = true;
            ProductName.HeaderText = "Product Name";
            ProductName.Name = "ProductName";
            ProductName.ReadOnly = true;
            ProductName.Resizable = DataGridViewTriState.False;
            ProductName.SortMode = DataGridViewColumnSortMode.NotSortable;
            ProductName.Width = 180;
            // 
            // PurchaseUOM
            // 
            PurchaseUOM.Frozen = true;
            PurchaseUOM.HeaderText = "P. UOM";
            PurchaseUOM.Name = "PurchaseUOM";
            PurchaseUOM.ReadOnly = true;
            PurchaseUOM.Resizable = DataGridViewTriState.False;
            PurchaseUOM.SortMode = DataGridViewColumnSortMode.NotSortable;
            PurchaseUOM.Width = 70;
            // 
            // RetainXFactor
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            RetainXFactor.DefaultCellStyle = dataGridViewCellStyle2;
            RetainXFactor.Frozen = true;
            RetainXFactor.HeaderText = "R. XF";
            RetainXFactor.Name = "RetainXFactor";
            RetainXFactor.ReadOnly = true;
            RetainXFactor.Resizable = DataGridViewTriState.False;
            RetainXFactor.SortMode = DataGridViewColumnSortMode.NotSortable;
            RetainXFactor.Width = 70;
            // 
            // RetailUOM
            // 
            RetailUOM.Frozen = true;
            RetailUOM.HeaderText = "R. UOM";
            RetailUOM.Name = "RetailUOM";
            RetailUOM.ReadOnly = true;
            RetailUOM.Resizable = DataGridViewTriState.False;
            RetailUOM.SortMode = DataGridViewColumnSortMode.NotSortable;
            RetailUOM.Width = 70;
            // 
            // WholesaleXFactor
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            WholesaleXFactor.DefaultCellStyle = dataGridViewCellStyle3;
            WholesaleXFactor.Frozen = true;
            WholesaleXFactor.HeaderText = "W. XF";
            WholesaleXFactor.Name = "WholesaleXFactor";
            WholesaleXFactor.ReadOnly = true;
            WholesaleXFactor.Resizable = DataGridViewTriState.False;
            WholesaleXFactor.SortMode = DataGridViewColumnSortMode.NotSortable;
            WholesaleXFactor.Width = 70;
            // 
            // WholeSaleUOM
            // 
            WholeSaleUOM.Frozen = true;
            WholeSaleUOM.HeaderText = "W. UOM";
            WholeSaleUOM.Name = "WholeSaleUOM";
            WholeSaleUOM.ReadOnly = true;
            WholeSaleUOM.Resizable = DataGridViewTriState.False;
            WholeSaleUOM.SortMode = DataGridViewColumnSortMode.NotSortable;
            WholeSaleUOM.Width = 70;
            // 
            // PurchasePrice
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
            PurchasePrice.DefaultCellStyle = dataGridViewCellStyle4;
            PurchasePrice.Frozen = true;
            PurchasePrice.HeaderText = "P. Price";
            PurchasePrice.Name = "PurchasePrice";
            PurchasePrice.ReadOnly = true;
            PurchasePrice.Resizable = DataGridViewTriState.False;
            PurchasePrice.SortMode = DataGridViewColumnSortMode.NotSortable;
            PurchasePrice.Width = 70;
            // 
            // Cost
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
            Cost.DefaultCellStyle = dataGridViewCellStyle5;
            Cost.Frozen = true;
            Cost.HeaderText = "Cost";
            Cost.Name = "Cost";
            Cost.ReadOnly = true;
            Cost.Resizable = DataGridViewTriState.False;
            Cost.SortMode = DataGridViewColumnSortMode.NotSortable;
            Cost.Width = 70;
            // 
            // RetailPrice
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
            RetailPrice.DefaultCellStyle = dataGridViewCellStyle6;
            RetailPrice.Frozen = true;
            RetailPrice.HeaderText = "R. Price";
            RetailPrice.Name = "RetailPrice";
            RetailPrice.ReadOnly = true;
            RetailPrice.Resizable = DataGridViewTriState.False;
            RetailPrice.SortMode = DataGridViewColumnSortMode.NotSortable;
            RetailPrice.Width = 70;
            // 
            // WholesalePrice
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight;
            WholesalePrice.DefaultCellStyle = dataGridViewCellStyle7;
            WholesalePrice.Frozen = true;
            WholesalePrice.HeaderText = "W. Price";
            WholesalePrice.Name = "WholesalePrice";
            WholesalePrice.ReadOnly = true;
            WholesalePrice.Resizable = DataGridViewTriState.False;
            WholesalePrice.SortMode = DataGridViewColumnSortMode.NotSortable;
            WholesalePrice.Width = 70;
            // 
            // msrp
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleRight;
            msrp.DefaultCellStyle = dataGridViewCellStyle8;
            msrp.Frozen = true;
            msrp.HeaderText = "MSRP";
            msrp.Name = "msrp";
            msrp.ReadOnly = true;
            msrp.Resizable = DataGridViewTriState.False;
            msrp.SortMode = DataGridViewColumnSortMode.NotSortable;
            msrp.Width = 70;
            // 
            // OpStock
            // 
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleRight;
            OpStock.DefaultCellStyle = dataGridViewCellStyle9;
            OpStock.Frozen = true;
            OpStock.HeaderText = "Op.Stock";
            OpStock.Name = "OpStock";
            OpStock.ReadOnly = true;
            OpStock.Resizable = DataGridViewTriState.False;
            OpStock.SortMode = DataGridViewColumnSortMode.NotSortable;
            OpStock.Width = 70;
            // 
            // PurchaseQty
            // 
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleRight;
            PurchaseQty.DefaultCellStyle = dataGridViewCellStyle10;
            PurchaseQty.Frozen = true;
            PurchaseQty.HeaderText = "P. Qty";
            PurchaseQty.Name = "PurchaseQty";
            PurchaseQty.ReadOnly = true;
            PurchaseQty.Resizable = DataGridViewTriState.False;
            PurchaseQty.SortMode = DataGridViewColumnSortMode.NotSortable;
            PurchaseQty.Width = 65;
            // 
            // SaleQty
            // 
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleRight;
            SaleQty.DefaultCellStyle = dataGridViewCellStyle11;
            SaleQty.Frozen = true;
            SaleQty.HeaderText = "S. Qty";
            SaleQty.Name = "SaleQty";
            SaleQty.ReadOnly = true;
            SaleQty.Resizable = DataGridViewTriState.False;
            SaleQty.SortMode = DataGridViewColumnSortMode.NotSortable;
            SaleQty.Width = 65;
            // 
            // ClosingQty
            // 
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleRight;
            ClosingQty.DefaultCellStyle = dataGridViewCellStyle12;
            ClosingQty.Frozen = true;
            ClosingQty.HeaderText = "C. Qty";
            ClosingQty.Name = "ClosingQty";
            ClosingQty.ReadOnly = true;
            ClosingQty.Resizable = DataGridViewTriState.False;
            ClosingQty.SortMode = DataGridViewColumnSortMode.NotSortable;
            ClosingQty.Width = 65;
            // 
            // Column1
            // 
            dataGridViewCellStyle13.BackColor = Color.Silver;
            dataGridViewCellStyle13.ForeColor = Color.Silver;
            dataGridViewCellStyle13.SelectionBackColor = Color.Silver;
            dataGridViewCellStyle13.SelectionForeColor = Color.Silver;
            Column1.DefaultCellStyle = dataGridViewCellStyle13;
            Column1.Frozen = true;
            Column1.HeaderText = "";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Visible = false;
            Column1.Width = 15;
            // 
            // Column2
            // 
            Column2.Frozen = true;
            Column2.HeaderText = "CategoryName";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Visible = false;
            // 
            // FormItemReport
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1323, 539);
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
            Name = "FormItemReport";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Item Report";
            Load += FormItemReport_Load;
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
        private ToolStripLabel LabelCategory;
        private ToolStripButton BtnGo;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton ToolStripBtnsave;
        private ToolStripButton ToolStripBtnPrint;
        private views.controls.DataViewVerticalScroll GridViewItems;
        private Button BtnExit;
        private Button BtnCancel;
        private Button BtnPrint;
        private Button BtnSave;
        private ToolStripLabel toolStripLabel1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel ErrorMsg;
        private views.controls.ToolStripComboTree ComboBoxColumns;
        private ToolStripSeparator toolStripSeparator2;
        private views.controls.ToolstripCheckedTreeComboBox ComboBoxCategory;
        private ToolStripSeparator toolStripSeparator3;
        private DataGridViewTextBoxColumn RowNumber;
        private DataGridViewTextBoxColumn MaterialId;
        private DataGridViewTextBoxColumn ProductName;
        private DataGridViewTextBoxColumn PurchaseUOM;
        private DataGridViewTextBoxColumn RetainXFactor;
        private DataGridViewTextBoxColumn RetailUOM;
        private DataGridViewTextBoxColumn WholesaleXFactor;
        private DataGridViewTextBoxColumn WholeSaleUOM;
        private DataGridViewTextBoxColumn PurchasePrice;
        private DataGridViewTextBoxColumn Cost;
        private DataGridViewTextBoxColumn RetailPrice;
        private DataGridViewTextBoxColumn WholesalePrice;
        private DataGridViewTextBoxColumn msrp;
        private DataGridViewTextBoxColumn OpStock;
        private DataGridViewTextBoxColumn PurchaseQty;
        private DataGridViewTextBoxColumn SaleQty;
        private DataGridViewTextBoxColumn ClosingQty;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
    }
}