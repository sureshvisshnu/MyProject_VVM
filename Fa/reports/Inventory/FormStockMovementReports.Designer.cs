namespace Fa.reports.Inventory
{
    partial class FormStockMovementReports
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStockMovementReports));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            StkMvmtRptToolStrip = new fa.views.controls.Ab2ToolStrip();
            toolStripLabelType = new ToolStripLabel();
            ComboBoxType = new ToolStripComboBox();
            ToolStripLocationLabel = new ToolStripLabel();
            CheckedTreeComboLocation = new fa.views.controls.ToolstripCheckedTreeComboBox();
            ToolStripFromLabel = new ToolStripLabel();
            FromDate = new fa.views.controls.ToolStripCalendar();
            toolStripSeparator4 = new ToolStripSeparator();
            ToolStripToLabel = new ToolStripLabel();
            ToDate = new fa.views.controls.ToolStripCalendar();
            toolStripSeparator5 = new ToolStripSeparator();
            BtnGo = new ToolStripButton();
            BtnToolStripSave = new ToolStripButton();
            BtnToolStripPrint = new ToolStripButton();
            GridviewForByDate = new fa.views.controls.DataViewVerticalScroll();
            ByDateSNo = new DataGridViewTextBoxColumn();
            ByDateDate = new DataGridViewTextBoxColumn();
            ByDateReference = new DataGridViewTextBoxColumn();
            ByDateSource = new DataGridViewTextBoxColumn();
            ByDateDestination = new DataGridViewTextBoxColumn();
            ByDateQuantity = new fa.views.controls.grid.DataGridViewQuantityColumn();
            ByDateFree = new fa.views.controls.grid.DataGridViewQuantityColumn();
            ByDateMovedBy = new DataGridViewTextBoxColumn();
            ByDateId = new DataGridViewTextBoxColumn();
            statusStripStockMovement = new StatusStrip();
            StckMvtRptErrMsg = new ToolStripStatusLabel();
            BtnSMRReset = new Button();
            BtnSMRExit = new Button();
            BtnSMRPrint = new Button();
            BtnSMRSave = new Button();
            GridviewForByItem = new fa.views.controls.DataViewVerticalScroll();
            ByItemSno = new DataGridViewTextBoxColumn();
            ByItemCode = new DataGridViewTextBoxColumn();
            ByItemName = new DataGridViewTextBoxColumn();
            ByItemBatch = new DataGridViewTextBoxColumn();
            ByItemExpDate = new DataGridViewTextBoxColumn();
            ByItemQty = new fa.views.controls.grid.DataGridViewQuantityColumn();
            ByItemFree = new fa.views.controls.grid.DataGridViewQuantityColumn();
            ByItemId = new DataGridViewTextBoxColumn();
            GridviewForByLocation = new fa.views.controls.DataViewVerticalScroll();
            ByLocationSno = new DataGridViewTextBoxColumn();
            ByLocationDate = new DataGridViewTextBoxColumn();
            ByLocationReference = new DataGridViewTextBoxColumn();
            ByLocationDestination = new DataGridViewTextBoxColumn();
            ByLocationQty = new fa.views.controls.grid.DataGridViewQuantityColumn();
            ByLocationFree = new fa.views.controls.grid.DataGridViewQuantityColumn();
            ByLocationMovedBy = new DataGridViewTextBoxColumn();
            ByLocationId = new DataGridViewTextBoxColumn();
            StkMvmtRptToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridviewForByDate).BeginInit();
            statusStripStockMovement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridviewForByItem).BeginInit();
            ((System.ComponentModel.ISupportInitialize)GridviewForByLocation).BeginInit();
            SuspendLayout();
            // 
            // StkMvmtRptToolStrip
            // 
            StkMvmtRptToolStrip.BackColor = SystemColors.ControlLight;
            StkMvmtRptToolStrip.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            StkMvmtRptToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            StkMvmtRptToolStrip.Items.AddRange(new ToolStripItem[] { toolStripLabelType, ComboBoxType, ToolStripLocationLabel, CheckedTreeComboLocation, ToolStripFromLabel, FromDate, toolStripSeparator4, ToolStripToLabel, ToDate, toolStripSeparator5, BtnGo, BtnToolStripSave, BtnToolStripPrint });
            StkMvmtRptToolStrip.Location = new Point(0, 0);
            StkMvmtRptToolStrip.Name = "StkMvmtRptToolStrip";
            StkMvmtRptToolStrip.Padding = new Padding(5);
            StkMvmtRptToolStrip.Size = new Size(972, 38);
            StkMvmtRptToolStrip.TabIndex = 0;
            // 
            // toolStripLabelType
            // 
            toolStripLabelType.Name = "toolStripLabelType";
            toolStripLabelType.Size = new Size(31, 25);
            toolStripLabelType.Text = "Type";
            // 
            // ComboBoxType
            // 
            ComboBoxType.FlatStyle = FlatStyle.Standard;
            ComboBoxType.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ComboBoxType.Items.AddRange(new object[] { "By Date", "By Item", "By Location" });
            ComboBoxType.Name = "ComboBoxType";
            ComboBoxType.Size = new Size(150, 28);
            ComboBoxType.Text = "By Date";
            ComboBoxType.SelectedIndexChanged += ComboBoxType_SelectedIndexChanged;
            // 
            // ToolStripLocationLabel
            // 
            ToolStripLocationLabel.Name = "ToolStripLocationLabel";
            ToolStripLocationLabel.Size = new Size(47, 25);
            ToolStripLocationLabel.Text = "Location";
            // 
            // CheckedTreeComboLocation
            // 
            CheckedTreeComboLocation.AutoSize = false;
            CheckedTreeComboLocation.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            CheckedTreeComboLocation.Name = "CheckedTreeComboLocation";
            CheckedTreeComboLocation.SelectedNode = null;
            CheckedTreeComboLocation.Size = new Size(200, 21);
            CheckedTreeComboLocation.NodeClickedEvent += CheckedTreeComboLocation_NodeClickedEvent;
            // 
            // ToolStripFromLabel
            // 
            ToolStripFromLabel.Name = "ToolStripFromLabel";
            ToolStripFromLabel.Size = new Size(31, 25);
            ToolStripFromLabel.Text = "From";
            // 
            // FromDate
            // 
            FromDate.BackColor = Color.White;
            FromDate.Date = null;
            FromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FromDate.Format = "MM/dd/yyyy";
            FromDate.MaxDate = new DateTime(9997, 12, 31, 7, 30, 34, 0);
            FromDate.MinDate = new DateTime(1900, 1, 1, 23, 3, 20, 0);
            FromDate.Name = "FromDate";
            FromDate.Size = new Size(97, 25);
            FromDate.Text = "toolStripCalendar2";
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 28);
            // 
            // ToolStripToLabel
            // 
            ToolStripToLabel.Name = "ToolStripToLabel";
            ToolStripToLabel.Size = new Size(19, 25);
            ToolStripToLabel.Text = "To";
            // 
            // ToDate
            // 
            ToDate.BackColor = Color.White;
            ToDate.Date = null;
            ToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ToDate.Format = "MM/dd/yyyy";
            ToDate.MaxDate = new DateTime(9997, 12, 31, 7, 30, 34, 0);
            ToDate.MinDate = new DateTime(1900, 1, 1, 23, 3, 20, 0);
            ToDate.Name = "ToDate";
            ToDate.Size = new Size(97, 25);
            ToDate.Text = "toolStripCalendar1";
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.ForeColor = SystemColors.Control;
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(6, 28);
            // 
            // BtnGo
            // 
            BtnGo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnGo.Image = (Image)resources.GetObject("BtnGo.Image");
            BtnGo.ImageTransparentColor = Color.Magenta;
            BtnGo.Name = "BtnGo";
            BtnGo.Size = new Size(24, 25);
            BtnGo.Text = "Go";
            BtnGo.Click += BtnGo_Click;
            // 
            // BtnToolStripSave
            // 
            BtnToolStripSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            BtnToolStripSave.Image = (Image)resources.GetObject("BtnToolStripSave.Image");
            BtnToolStripSave.ImageTransparentColor = Color.Black;
            BtnToolStripSave.Name = "BtnToolStripSave";
            BtnToolStripSave.Size = new Size(23, 25);
            BtnToolStripSave.Text = "Save";
            BtnToolStripSave.Click += BtnSMRSave_Click;
            // 
            // BtnToolStripPrint
            // 
            BtnToolStripPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            BtnToolStripPrint.Image = (Image)resources.GetObject("BtnToolStripPrint.Image");
            BtnToolStripPrint.ImageTransparentColor = Color.Black;
            BtnToolStripPrint.Name = "BtnToolStripPrint";
            BtnToolStripPrint.Size = new Size(23, 25);
            BtnToolStripPrint.Text = "Print";
            BtnToolStripPrint.Click += BtnSMRPrint_Click;
            // 
            // GridviewForByDate
            // 
            GridviewForByDate.AllowUserToAddRows = false;
            GridviewForByDate.AllowUserToDeleteRows = false;
            GridviewForByDate.AllowUserToResizeColumns = false;
            GridviewForByDate.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridviewForByDate.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridviewForByDate.ColumnHeadersHeight = 20;
            GridviewForByDate.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridviewForByDate.Columns.AddRange(new DataGridViewColumn[] { ByDateSNo, ByDateDate, ByDateReference, ByDateSource, ByDateDestination, ByDateQuantity, ByDateFree, ByDateMovedBy, ByDateId });
            GridviewForByDate.EnableHeadersVisualStyles = false;
            GridviewForByDate.Location = new Point(2, 40);
            GridviewForByDate.MultiSelect = false;
            GridviewForByDate.Name = "GridviewForByDate";
            GridviewForByDate.ReadOnly = true;
            GridviewForByDate.RowHeadersVisible = false;
            GridviewForByDate.RowTemplate.Height = 20;
            GridviewForByDate.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridviewForByDate.ShowCellToolTips = false;
            GridviewForByDate.Size = new Size(969, 431);
            GridviewForByDate.TabIndex = 1;
            // 
            // ByDateSNo
            // 
            ByDateSNo.HeaderText = "#";
            ByDateSNo.Name = "ByDateSNo";
            ByDateSNo.ReadOnly = true;
            ByDateSNo.Resizable = DataGridViewTriState.False;
            ByDateSNo.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateSNo.Width = 40;
            // 
            // ByDateDate
            // 
            ByDateDate.HeaderText = "Date";
            ByDateDate.Name = "ByDateDate";
            ByDateDate.ReadOnly = true;
            ByDateDate.Resizable = DataGridViewTriState.False;
            ByDateDate.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateDate.Width = 120;
            // 
            // ByDateReference
            // 
            ByDateReference.HeaderText = "Reference";
            ByDateReference.Name = "ByDateReference";
            ByDateReference.ReadOnly = true;
            ByDateReference.Resizable = DataGridViewTriState.False;
            ByDateReference.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // ByDateSource
            // 
            ByDateSource.HeaderText = "Source";
            ByDateSource.Name = "ByDateSource";
            ByDateSource.ReadOnly = true;
            ByDateSource.Resizable = DataGridViewTriState.False;
            ByDateSource.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateSource.Width = 150;
            // 
            // ByDateDestination
            // 
            ByDateDestination.HeaderText = "Destination";
            ByDateDestination.Name = "ByDateDestination";
            ByDateDestination.ReadOnly = true;
            ByDateDestination.Resizable = DataGridViewTriState.False;
            ByDateDestination.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateDestination.Width = 170;
            // 
            // ByDateQuantity
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopRight;
            ByDateQuantity.DefaultCellStyle = dataGridViewCellStyle2;
            ByDateQuantity.HeaderText = "Quantity";
            ByDateQuantity.Name = "ByDateQuantity";
            ByDateQuantity.ReadOnly = true;
            ByDateQuantity.Resizable = DataGridViewTriState.False;
            // 
            // ByDateFree
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopRight;
            ByDateFree.DefaultCellStyle = dataGridViewCellStyle3;
            ByDateFree.HeaderText = "Free";
            ByDateFree.Name = "ByDateFree";
            ByDateFree.ReadOnly = true;
            ByDateFree.Resizable = DataGridViewTriState.False;
            // 
            // ByDateMovedBy
            // 
            ByDateMovedBy.HeaderText = "Moved By";
            ByDateMovedBy.Name = "ByDateMovedBy";
            ByDateMovedBy.ReadOnly = true;
            ByDateMovedBy.Resizable = DataGridViewTriState.False;
            ByDateMovedBy.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateMovedBy.Width = 170;
            // 
            // ByDateId
            // 
            ByDateId.HeaderText = "Id";
            ByDateId.Name = "ByDateId";
            ByDateId.ReadOnly = true;
            ByDateId.Visible = false;
            // 
            // statusStripStockMovement
            // 
            statusStripStockMovement.Items.AddRange(new ToolStripItem[] { StckMvtRptErrMsg });
            statusStripStockMovement.Location = new Point(0, 517);
            statusStripStockMovement.Name = "statusStripStockMovement";
            statusStripStockMovement.Size = new Size(972, 22);
            statusStripStockMovement.TabIndex = 2;
            // 
            // StckMvtRptErrMsg
            // 
            StckMvtRptErrMsg.Name = "StckMvtRptErrMsg";
            StckMvtRptErrMsg.Size = new Size(31, 17);
            StckMvtRptErrMsg.Text = "        ";
            // 
            // BtnSMRReset
            // 
            BtnSMRReset.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSMRReset.Location = new Point(621, 486);
            BtnSMRReset.Name = "BtnSMRReset";
            BtnSMRReset.Size = new Size(82, 23);
            BtnSMRReset.TabIndex = 3;
            BtnSMRReset.Text = "Reset [Esc]";
            BtnSMRReset.UseVisualStyleBackColor = true;
            BtnSMRReset.Click += BtnSMRCancel_Click;
            // 
            // BtnSMRExit
            // 
            BtnSMRExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSMRExit.Location = new Point(870, 486);
            BtnSMRExit.Name = "BtnSMRExit";
            BtnSMRExit.Size = new Size(72, 23);
            BtnSMRExit.TabIndex = 33;
            BtnSMRExit.Text = "Exit [F10]";
            BtnSMRExit.UseVisualStyleBackColor = true;
            BtnSMRExit.Click += BtnSMRExit_Click;
            // 
            // BtnSMRPrint
            // 
            BtnSMRPrint.Enabled = false;
            BtnSMRPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSMRPrint.Location = new Point(789, 486);
            BtnSMRPrint.Name = "BtnSMRPrint";
            BtnSMRPrint.Size = new Size(75, 23);
            BtnSMRPrint.TabIndex = 32;
            BtnSMRPrint.Text = "Print [F9]";
            BtnSMRPrint.UseVisualStyleBackColor = true;
            BtnSMRPrint.Click += BtnSMRPrint_Click;
            // 
            // BtnSMRSave
            // 
            BtnSMRSave.Enabled = false;
            BtnSMRSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSMRSave.Location = new Point(708, 486);
            BtnSMRSave.Name = "BtnSMRSave";
            BtnSMRSave.Size = new Size(75, 23);
            BtnSMRSave.TabIndex = 31;
            BtnSMRSave.Text = "Save [F8]";
            BtnSMRSave.UseVisualStyleBackColor = true;
            BtnSMRSave.Click += BtnSMRSave_Click;
            // 
            // GridviewForByItem
            // 
            GridviewForByItem.AllowUserToAddRows = false;
            GridviewForByItem.AllowUserToDeleteRows = false;
            GridviewForByItem.AllowUserToResizeColumns = false;
            GridviewForByItem.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            GridviewForByItem.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            GridviewForByItem.ColumnHeadersHeight = 20;
            GridviewForByItem.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridviewForByItem.Columns.AddRange(new DataGridViewColumn[] { ByItemSno, ByItemCode, ByItemName, ByItemBatch, ByItemExpDate, ByItemQty, ByItemFree, ByItemId });
            GridviewForByItem.EnableHeadersVisualStyles = false;
            GridviewForByItem.Location = new Point(2, 40);
            GridviewForByItem.MultiSelect = false;
            GridviewForByItem.Name = "GridviewForByItem";
            GridviewForByItem.ReadOnly = true;
            GridviewForByItem.RowHeadersVisible = false;
            GridviewForByItem.RowTemplate.Height = 20;
            GridviewForByItem.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridviewForByItem.ShowCellToolTips = false;
            GridviewForByItem.Size = new Size(969, 431);
            GridviewForByItem.TabIndex = 34;
            GridviewForByItem.Visible = false;
            // 
            // ByItemSno
            // 
            ByItemSno.HeaderText = "#";
            ByItemSno.Name = "ByItemSno";
            ByItemSno.ReadOnly = true;
            ByItemSno.Resizable = DataGridViewTriState.False;
            ByItemSno.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByItemSno.Width = 40;
            // 
            // ByItemCode
            // 
            ByItemCode.HeaderText = "Code";
            ByItemCode.Name = "ByItemCode";
            ByItemCode.ReadOnly = true;
            ByItemCode.Resizable = DataGridViewTriState.False;
            ByItemCode.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByItemCode.Width = 160;
            // 
            // ByItemName
            // 
            ByItemName.HeaderText = "Name";
            ByItemName.Name = "ByItemName";
            ByItemName.ReadOnly = true;
            ByItemName.Resizable = DataGridViewTriState.False;
            ByItemName.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByItemName.Width = 310;
            // 
            // ByItemBatch
            // 
            ByItemBatch.HeaderText = "Batch No";
            ByItemBatch.Name = "ByItemBatch";
            ByItemBatch.ReadOnly = true;
            ByItemBatch.Resizable = DataGridViewTriState.False;
            ByItemBatch.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByItemBatch.Width = 130;
            // 
            // ByItemExpDate
            // 
            ByItemExpDate.HeaderText = "Exp Date";
            ByItemExpDate.Name = "ByItemExpDate";
            ByItemExpDate.ReadOnly = true;
            ByItemExpDate.Resizable = DataGridViewTriState.False;
            ByItemExpDate.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByItemExpDate.Width = 130;
            // 
            // ByItemQty
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.TopRight;
            ByItemQty.DefaultCellStyle = dataGridViewCellStyle5;
            ByItemQty.HeaderText = "Quantity";
            ByItemQty.Name = "ByItemQty";
            ByItemQty.ReadOnly = true;
            ByItemQty.Resizable = DataGridViewTriState.False;
            ByItemQty.Width = 90;
            // 
            // ByItemFree
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.TopRight;
            ByItemFree.DefaultCellStyle = dataGridViewCellStyle6;
            ByItemFree.HeaderText = "Free";
            ByItemFree.Name = "ByItemFree";
            ByItemFree.ReadOnly = true;
            ByItemFree.Resizable = DataGridViewTriState.False;
            ByItemFree.Width = 90;
            // 
            // ByItemId
            // 
            ByItemId.HeaderText = "Id";
            ByItemId.Name = "ByItemId";
            ByItemId.ReadOnly = true;
            ByItemId.Visible = false;
            // 
            // GridviewForByLocation
            // 
            GridviewForByLocation.AllowUserToAddRows = false;
            GridviewForByLocation.AllowUserToDeleteRows = false;
            GridviewForByLocation.AllowUserToResizeColumns = false;
            GridviewForByLocation.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = SystemColors.Control;
            dataGridViewCellStyle7.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle7.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle7.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            GridviewForByLocation.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            GridviewForByLocation.ColumnHeadersHeight = 20;
            GridviewForByLocation.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridviewForByLocation.Columns.AddRange(new DataGridViewColumn[] { ByLocationSno, ByLocationDate, ByLocationReference, ByLocationDestination, ByLocationQty, ByLocationFree, ByLocationMovedBy, ByLocationId });
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = SystemColors.Window;
            dataGridViewCellStyle10.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle10.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle10.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.False;
            GridviewForByLocation.DefaultCellStyle = dataGridViewCellStyle10;
            GridviewForByLocation.EnableHeadersVisualStyles = false;
            GridviewForByLocation.Location = new Point(2, 40);
            GridviewForByLocation.MultiSelect = false;
            GridviewForByLocation.Name = "GridviewForByLocation";
            GridviewForByLocation.ReadOnly = true;
            GridviewForByLocation.RowHeadersVisible = false;
            GridviewForByLocation.RowTemplate.Height = 20;
            GridviewForByLocation.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridviewForByLocation.ShowCellToolTips = false;
            GridviewForByLocation.Size = new Size(969, 431);
            GridviewForByLocation.TabIndex = 35;
            GridviewForByLocation.Visible = false;
            GridviewForByLocation.CellPainting += GridviewForByLocation_CellPainting;
            GridviewForByLocation.RowPostPaint += GridviewForByLocation_RowPostPaint;
            // 
            // ByLocationSno
            // 
            ByLocationSno.HeaderText = "#";
            ByLocationSno.Name = "ByLocationSno";
            ByLocationSno.ReadOnly = true;
            ByLocationSno.Resizable = DataGridViewTriState.False;
            ByLocationSno.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByLocationSno.Width = 60;
            // 
            // ByLocationDate
            // 
            ByLocationDate.HeaderText = "Date";
            ByLocationDate.Name = "ByLocationDate";
            ByLocationDate.ReadOnly = true;
            ByLocationDate.Resizable = DataGridViewTriState.False;
            ByLocationDate.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByLocationDate.Width = 150;
            // 
            // ByLocationReference
            // 
            ByLocationReference.HeaderText = "Reference";
            ByLocationReference.Name = "ByLocationReference";
            ByLocationReference.ReadOnly = true;
            ByLocationReference.Resizable = DataGridViewTriState.False;
            ByLocationReference.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByLocationReference.Width = 120;
            // 
            // ByLocationDestination
            // 
            ByLocationDestination.HeaderText = "Destination";
            ByLocationDestination.Name = "ByLocationDestination";
            ByLocationDestination.ReadOnly = true;
            ByLocationDestination.Resizable = DataGridViewTriState.False;
            ByLocationDestination.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByLocationDestination.Width = 220;
            // 
            // ByLocationQty
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.TopRight;
            ByLocationQty.DefaultCellStyle = dataGridViewCellStyle8;
            ByLocationQty.HeaderText = "Quantity";
            ByLocationQty.Name = "ByLocationQty";
            ByLocationQty.ReadOnly = true;
            ByLocationQty.Resizable = DataGridViewTriState.False;
            // 
            // ByLocationFree
            // 
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.TopRight;
            ByLocationFree.DefaultCellStyle = dataGridViewCellStyle9;
            ByLocationFree.HeaderText = "Free";
            ByLocationFree.Name = "ByLocationFree";
            ByLocationFree.ReadOnly = true;
            ByLocationFree.Resizable = DataGridViewTriState.False;
            // 
            // ByLocationMovedBy
            // 
            ByLocationMovedBy.HeaderText = "Moved By";
            ByLocationMovedBy.Name = "ByLocationMovedBy";
            ByLocationMovedBy.ReadOnly = true;
            ByLocationMovedBy.Resizable = DataGridViewTriState.False;
            ByLocationMovedBy.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByLocationMovedBy.Width = 200;
            // 
            // ByLocationId
            // 
            ByLocationId.HeaderText = "Id";
            ByLocationId.Name = "ByLocationId";
            ByLocationId.ReadOnly = true;
            ByLocationId.Visible = false;
            // 
            // FormStockMovementReports
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(972, 539);
            Controls.Add(BtnSMRExit);
            Controls.Add(BtnSMRPrint);
            Controls.Add(BtnSMRSave);
            Controls.Add(BtnSMRReset);
            Controls.Add(statusStripStockMovement);
            Controls.Add(StkMvmtRptToolStrip);
            Controls.Add(GridviewForByItem);
            Controls.Add(GridviewForByDate);
            Controls.Add(GridviewForByLocation);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormStockMovementReports";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Stock Movement Report";
            Load += FormStockMovementReports_Load;
            StkMvmtRptToolStrip.ResumeLayout(false);
            StkMvmtRptToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridviewForByDate).EndInit();
            statusStripStockMovement.ResumeLayout(false);
            statusStripStockMovement.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridviewForByItem).EndInit();
            ((System.ComponentModel.ISupportInitialize)GridviewForByLocation).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private fa.views.controls.Ab2ToolStrip StkMvmtRptToolStrip;
        private ToolStripLabel toolStripLabelType;
        private ToolStripComboBox ComboBoxType;
        private ToolStripLabel ToolStripLocationLabel;
        private fa.views.controls.ToolstripCheckedTreeComboBox CheckedTreeComboLocation;
        private ToolStripLabel ToolStripFromLabel;
        private fa.views.controls.ToolStripCalendar FromDate;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripLabel ToolStripToLabel;
        private fa.views.controls.ToolStripCalendar ToDate;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripButton BtnGo;
        private ToolStripButton BtnToolStripSave;
        private ToolStripButton BtnToolStripPrint;
        private fa.views.controls.DataViewVerticalScroll GridviewForByDate;
        private StatusStrip statusStripStockMovement;
        private ToolStripStatusLabel StckMvtRptErrMsg;
        private Button BtnSMRReset;
        private Button BtnSMRExit;
        private Button BtnSMRPrint;
        private Button BtnSMRSave;
        private fa.views.controls.DataViewVerticalScroll GridviewForByItem;
        private DataGridViewTextBoxColumn Column9;
        private DataGridViewTextBoxColumn Column10;
        private DataGridViewTextBoxColumn Column11;
        private DataGridViewTextBoxColumn Column12;
        private fa.views.controls.grid.DataGridViewQuantityColumn Column14;
        private fa.views.controls.DataViewVerticalScroll GridviewForByLocation;
        private fa.views.controls.grid.DataGridViewQuantityColumn Column13;
        private DataGridViewTextBoxColumn Column15;
        private DataGridViewTextBoxColumn ByDayeSNo;
        private DataGridViewTextBoxColumn ByItemSno;
        private DataGridViewTextBoxColumn ByItemCode;
        private DataGridViewTextBoxColumn ByItemName;
        private DataGridViewTextBoxColumn ByItemBatch;
        private DataGridViewTextBoxColumn ByItemExpDate;
        private fa.views.controls.grid.DataGridViewQuantityColumn ByItemQty;
        private fa.views.controls.grid.DataGridViewQuantityColumn ByItemFree;
        private DataGridViewTextBoxColumn ByItemId;
        private DataGridViewTextBoxColumn ByDateSNo;
        private DataGridViewTextBoxColumn ByDateDate;
        private DataGridViewTextBoxColumn ByDateReference;
        private DataGridViewTextBoxColumn ByDateSource;
        private DataGridViewTextBoxColumn ByDateDestination;
        private fa.views.controls.grid.DataGridViewQuantityColumn ByDateQuantity;
        private fa.views.controls.grid.DataGridViewQuantityColumn ByDateFree;
        private DataGridViewTextBoxColumn ByDateMovedBy;
        private DataGridViewTextBoxColumn ByDateId;
        private DataGridViewTextBoxColumn ByLocationSno;
        private DataGridViewTextBoxColumn ByLocationDate;
        private DataGridViewTextBoxColumn ByLocationReference;
        private DataGridViewTextBoxColumn ByLocationDestination;
        private fa.views.controls.grid.DataGridViewQuantityColumn ByLocationQty;
        private fa.views.controls.grid.DataGridViewQuantityColumn ByLocationFree;
        private DataGridViewTextBoxColumn ByLocationMovedBy;
        private DataGridViewTextBoxColumn ByLocationId;
    }
}