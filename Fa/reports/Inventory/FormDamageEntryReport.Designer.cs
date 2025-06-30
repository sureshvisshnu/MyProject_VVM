namespace Fa.reports.Inventory
{
    partial class FormDamageEntryReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDamageEntryReport));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle18 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle14 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle16 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle17 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle19 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle26 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle20 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle21 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle22 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle23 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle24 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle25 = new DataGridViewCellStyle();
            ab2ToolStrip1 = new fa.views.controls.Ab2ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            damageEntryComboBoxType = new ToolStripComboBox();
            toolStripSeparator2 = new ToolStripSeparator();
            LabelLocation = new ToolStripLabel();
            CheckedTreeComboLocation = new fa.views.controls.ToolstripCheckedTreeComboBox();
            toolStripLabel2 = new ToolStripLabel();
            FromDate = new fa.views.controls.ToolStripCalendar();
            toolStripLabel3 = new ToolStripLabel();
            ToDate = new fa.views.controls.ToolStripCalendar();
            BtnGo = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            ToolStripSave = new ToolStripButton();
            ToolStripPrint = new ToolStripButton();
            GridviewForByItem = new fa.views.controls.DataViewVerticalScroll();
            ByItemSNo = new DataGridViewTextBoxColumn();
            ByItemCode = new DataGridViewTextBoxColumn();
            ByItemName = new DataGridViewTextBoxColumn();
            ByItemBatchNo = new DataGridViewTextBoxColumn();
            ByItemExpDte = new DataGridViewTextBoxColumn();
            ByItemQty = new fa.views.controls.grid.DataGridViewQuantityColumn();
            ByItemFree = new DataGridViewTextBoxColumn();
            ByItemId = new DataGridViewTextBoxColumn();
            statusStrip1 = new StatusStrip();
            DamageEntryErrMsg = new ToolStripStatusLabel();
            DamageBtnReset = new Button();
            DamageBtnPrint = new Button();
            DamageBtnSave = new Button();
            DamageBtnExit = new Button();
            GridviewForByDate = new fa.views.controls.DataViewVerticalScroll();
            ByDateSno = new DataGridViewTextBoxColumn();
            ByDateDate = new DataGridViewTextBoxColumn();
            ByDateReference = new DataGridViewTextBoxColumn();
            ByDateLocation = new DataGridViewTextBoxColumn();
            ByDateQty = new fa.views.controls.grid.DataGridViewQuantityColumn();
            ByDateFree = new DataGridViewTextBoxColumn();
            ByDateEnterBy = new DataGridViewTextBoxColumn();
            ByDateId = new DataGridViewTextBoxColumn();
            GridviewForByLocation = new fa.views.controls.DataViewVerticalScroll();
            ByLocationSno = new DataGridViewTextBoxColumn();
            ByLocationDate = new DataGridViewTextBoxColumn();
            ByLocationReference = new DataGridViewTextBoxColumn();
            ByLocationQty = new fa.views.controls.grid.DataGridViewQuantityColumn();
            ByLocationFree = new DataGridViewTextBoxColumn();
            ByLocationEnteredBy = new DataGridViewTextBoxColumn();
            ByLocationId = new DataGridViewTextBoxColumn();
            ab2ToolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridviewForByItem).BeginInit();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridviewForByDate).BeginInit();
            ((System.ComponentModel.ISupportInitialize)GridviewForByLocation).BeginInit();
            SuspendLayout();
            // 
            // ab2ToolStrip1
            // 
            ab2ToolStrip1.BackColor = SystemColors.ControlLight;
            ab2ToolStrip1.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ab2ToolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            ab2ToolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, damageEntryComboBoxType, toolStripSeparator2, LabelLocation, CheckedTreeComboLocation, toolStripLabel2, FromDate, toolStripLabel3, ToDate, BtnGo, toolStripSeparator1, ToolStripSave, ToolStripPrint });
            ab2ToolStrip1.Location = new Point(0, 0);
            ab2ToolStrip1.Name = "ab2ToolStrip1";
            ab2ToolStrip1.Padding = new Padding(5);
            ab2ToolStrip1.Size = new Size(954, 41);
            ab2ToolStrip1.TabIndex = 1;
            ab2ToolStrip1.Text = "ab2ToolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(35, 28);
            toolStripLabel1.Text = "Type";
            // 
            // damageEntryComboBoxType
            // 
            damageEntryComboBoxType.FlatStyle = FlatStyle.Standard;
            damageEntryComboBoxType.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            damageEntryComboBoxType.Items.AddRange(new object[] { "By Date", "By Item", "By Location" });
            damageEntryComboBoxType.Name = "damageEntryComboBoxType";
            damageEntryComboBoxType.Size = new Size(151, 31);
            damageEntryComboBoxType.Text = "By Date";
            damageEntryComboBoxType.SelectedIndexChanged += damageEntryComboBox_SelectedIndexChanged;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 31);
            // 
            // LabelLocation
            // 
            LabelLocation.Name = "LabelLocation";
            LabelLocation.Size = new Size(53, 28);
            LabelLocation.Text = "Location";
            // 
            // CheckedTreeComboLocation
            // 
            CheckedTreeComboLocation.AutoSize = false;
            CheckedTreeComboLocation.Name = "CheckedTreeComboLocation";
            CheckedTreeComboLocation.SelectedNode = null;
            CheckedTreeComboLocation.Size = new Size(161, 28);
            CheckedTreeComboLocation.NodeClickedEvent += ComboDamEryRptLocation_NodeClickedEvent;
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(34, 28);
            toolStripLabel2.Text = "From";
            // 
            // FromDate
            // 
            FromDate.BackColor = Color.White;
            FromDate.Date = null;
            FromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FromDate.Format = "MM/dd/yyyy";
            FromDate.MaxDate = new DateTime(9997, 12, 31, 0, 20, 3, 0);
            FromDate.MinDate = new DateTime(1900, 1, 1, 23, 52, 50, 0);
            FromDate.Name = "FromDate";
            FromDate.Size = new Size(97, 28);
            FromDate.Text = "toolStripCalendar1";
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new Size(22, 28);
            toolStripLabel3.Text = "To";
            // 
            // ToDate
            // 
            ToDate.BackColor = Color.White;
            ToDate.Date = null;
            ToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ToDate.Format = "MM/dd/yyyy";
            ToDate.MaxDate = new DateTime(9997, 12, 31, 0, 20, 3, 0);
            ToDate.MinDate = new DateTime(1900, 1, 1, 23, 52, 50, 0);
            ToDate.Name = "ToDate";
            ToDate.Size = new Size(97, 28);
            ToDate.Text = "toolStripCalendar2";
            // 
            // BtnGo
            // 
            BtnGo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnGo.Image = (Image)resources.GetObject("BtnGo.Image");
            BtnGo.ImageTransparentColor = Color.Magenta;
            BtnGo.Name = "BtnGo";
            BtnGo.Size = new Size(26, 28);
            BtnGo.Text = "Go";
            BtnGo.Click += BtnGo_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 31);
            // 
            // ToolStripSave
            // 
            ToolStripSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripSave.Image = (Image)resources.GetObject("ToolStripSave.Image");
            ToolStripSave.ImageTransparentColor = Color.Black;
            ToolStripSave.Name = "ToolStripSave";
            ToolStripSave.Size = new Size(23, 28);
            ToolStripSave.Text = "Save";
            ToolStripSave.Click += DamageBtnSave_Click;
            // 
            // ToolStripPrint
            // 
            ToolStripPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripPrint.Image = (Image)resources.GetObject("ToolStripPrint.Image");
            ToolStripPrint.ImageTransparentColor = Color.Black;
            ToolStripPrint.Name = "ToolStripPrint";
            ToolStripPrint.Size = new Size(23, 28);
            ToolStripPrint.Text = "Print";
            ToolStripPrint.Click += DamageBtnPrint_Click;
            // 
            // GridviewForByItem
            // 
            GridviewForByItem.AllowUserToAddRows = false;
            GridviewForByItem.AllowUserToDeleteRows = false;
            GridviewForByItem.AllowUserToResizeColumns = false;
            GridviewForByItem.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridviewForByItem.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridviewForByItem.ColumnHeadersHeight = 20;
            GridviewForByItem.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridviewForByItem.Columns.AddRange(new DataGridViewColumn[] { ByItemSNo, ByItemCode, ByItemName, ByItemBatchNo, ByItemExpDte, ByItemQty, ByItemFree, ByItemId });
            GridviewForByItem.EditMode = DataGridViewEditMode.EditProgrammatically;
            GridviewForByItem.EnableHeadersVisualStyles = false;
            GridviewForByItem.Location = new Point(3, 41);
            GridviewForByItem.Name = "GridviewForByItem";
            GridviewForByItem.ReadOnly = true;
            GridviewForByItem.RowHeadersVisible = false;
            dataGridViewCellStyle9.BackColor = SystemColors.Control;
            dataGridViewCellStyle9.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle9.SelectionForeColor = SystemColors.WindowText;
            GridviewForByItem.RowsDefaultCellStyle = dataGridViewCellStyle9;
            GridviewForByItem.RowTemplate.Height = 20;
            GridviewForByItem.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridviewForByItem.ShowCellToolTips = false;
            GridviewForByItem.Size = new Size(948, 451);
            GridviewForByItem.TabIndex = 2;
            GridviewForByItem.Visible = false;
            // 
            // ByItemSNo
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            ByItemSNo.DefaultCellStyle = dataGridViewCellStyle2;
            ByItemSNo.HeaderText = "#";
            ByItemSNo.Name = "ByItemSNo";
            ByItemSNo.ReadOnly = true;
            ByItemSNo.Resizable = DataGridViewTriState.False;
            ByItemSNo.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByItemSNo.Width = 40;
            // 
            // ByItemCode
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            ByItemCode.DefaultCellStyle = dataGridViewCellStyle3;
            ByItemCode.HeaderText = "Code";
            ByItemCode.Name = "ByItemCode";
            ByItemCode.ReadOnly = true;
            ByItemCode.Resizable = DataGridViewTriState.False;
            ByItemCode.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByItemCode.Width = 150;
            // 
            // ByItemName
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopLeft;
            ByItemName.DefaultCellStyle = dataGridViewCellStyle4;
            ByItemName.HeaderText = "Name";
            ByItemName.Name = "ByItemName";
            ByItemName.ReadOnly = true;
            ByItemName.Resizable = DataGridViewTriState.False;
            ByItemName.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByItemName.Width = 280;
            // 
            // ByItemBatchNo
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.TopLeft;
            ByItemBatchNo.DefaultCellStyle = dataGridViewCellStyle5;
            ByItemBatchNo.HeaderText = "Batch No";
            ByItemBatchNo.Name = "ByItemBatchNo";
            ByItemBatchNo.ReadOnly = true;
            ByItemBatchNo.Resizable = DataGridViewTriState.False;
            ByItemBatchNo.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByItemBatchNo.Width = 120;
            // 
            // ByItemExpDte
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.TopLeft;
            ByItemExpDte.DefaultCellStyle = dataGridViewCellStyle6;
            ByItemExpDte.HeaderText = "Exp Date";
            ByItemExpDte.Name = "ByItemExpDte";
            ByItemExpDte.ReadOnly = true;
            ByItemExpDte.Resizable = DataGridViewTriState.False;
            ByItemExpDte.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByItemExpDte.Width = 140;
            // 
            // ByItemQty
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.TopRight;
            ByItemQty.DefaultCellStyle = dataGridViewCellStyle7;
            ByItemQty.HeaderText = "Quantity";
            ByItemQty.Name = "ByItemQty";
            ByItemQty.ReadOnly = true;
            ByItemQty.Resizable = DataGridViewTriState.False;
            // 
            // ByItemFree
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.TopRight;
            ByItemFree.DefaultCellStyle = dataGridViewCellStyle8;
            ByItemFree.HeaderText = "Free";
            ByItemFree.Name = "ByItemFree";
            ByItemFree.ReadOnly = true;
            ByItemFree.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // ByItemId
            // 
            ByItemId.HeaderText = "Id";
            ByItemId.Name = "ByItemId";
            ByItemId.ReadOnly = true;
            ByItemId.Visible = false;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { DamageEntryErrMsg });
            statusStrip1.Location = new Point(0, 540);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(954, 22);
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            // 
            // DamageEntryErrMsg
            // 
            DamageEntryErrMsg.Name = "DamageEntryErrMsg";
            DamageEntryErrMsg.Size = new Size(28, 17);
            DamageEntryErrMsg.Text = "       ";
            // 
            // DamageBtnReset
            // 
            DamageBtnReset.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            DamageBtnReset.Location = new Point(580, 506);
            DamageBtnReset.Name = "DamageBtnReset";
            DamageBtnReset.Size = new Size(82, 23);
            DamageBtnReset.TabIndex = 4;
            DamageBtnReset.Text = "Reset [Esc]";
            DamageBtnReset.UseVisualStyleBackColor = true;
            DamageBtnReset.Click += DamageBtnReset_Click;
            // 
            // DamageBtnPrint
            // 
            DamageBtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            DamageBtnPrint.Location = new Point(749, 506);
            DamageBtnPrint.Name = "DamageBtnPrint";
            DamageBtnPrint.Size = new Size(76, 23);
            DamageBtnPrint.TabIndex = 4;
            DamageBtnPrint.Text = "Print [F9]";
            DamageBtnPrint.UseVisualStyleBackColor = true;
            DamageBtnPrint.Click += DamageBtnPrint_Click;
            // 
            // DamageBtnSave
            // 
            DamageBtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            DamageBtnSave.Location = new Point(667, 506);
            DamageBtnSave.Name = "DamageBtnSave";
            DamageBtnSave.Size = new Size(74, 23);
            DamageBtnSave.TabIndex = 4;
            DamageBtnSave.Text = "Save [F8]";
            DamageBtnSave.UseVisualStyleBackColor = true;
            DamageBtnSave.Click += DamageBtnSave_Click;
            // 
            // DamageBtnExit
            // 
            DamageBtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            DamageBtnExit.Location = new Point(829, 506);
            DamageBtnExit.Name = "DamageBtnExit";
            DamageBtnExit.Size = new Size(82, 23);
            DamageBtnExit.TabIndex = 4;
            DamageBtnExit.Text = "Exit [F10]";
            DamageBtnExit.UseVisualStyleBackColor = true;
            DamageBtnExit.Click += DamageBtnExit_Click;
            // 
            // GridviewForByDate
            // 
            GridviewForByDate.AllowUserToAddRows = false;
            GridviewForByDate.AllowUserToDeleteRows = false;
            GridviewForByDate.AllowUserToResizeColumns = false;
            GridviewForByDate.AllowUserToResizeRows = false;
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = SystemColors.Control;
            dataGridViewCellStyle10.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle10.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle10.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.True;
            GridviewForByDate.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            GridviewForByDate.ColumnHeadersHeight = 20;
            GridviewForByDate.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridviewForByDate.Columns.AddRange(new DataGridViewColumn[] { ByDateSno, ByDateDate, ByDateReference, ByDateLocation, ByDateQty, ByDateFree, ByDateEnterBy, ByDateId });
            GridviewForByDate.EnableHeadersVisualStyles = false;
            GridviewForByDate.Location = new Point(3, 41);
            GridviewForByDate.MultiSelect = false;
            GridviewForByDate.Name = "GridviewForByDate";
            GridviewForByDate.ReadOnly = true;
            GridviewForByDate.RowHeadersVisible = false;
            dataGridViewCellStyle18.BackColor = SystemColors.Control;
            dataGridViewCellStyle18.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle18.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle18.SelectionForeColor = SystemColors.WindowText;
            GridviewForByDate.RowsDefaultCellStyle = dataGridViewCellStyle18;
            GridviewForByDate.RowTemplate.Height = 20;
            GridviewForByDate.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridviewForByDate.ShowCellToolTips = false;
            GridviewForByDate.Size = new Size(948, 451);
            GridviewForByDate.TabIndex = 5;
            // 
            // ByDateSno
            // 
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.TopLeft;
            ByDateSno.DefaultCellStyle = dataGridViewCellStyle11;
            ByDateSno.HeaderText = "#";
            ByDateSno.Name = "ByDateSno";
            ByDateSno.ReadOnly = true;
            ByDateSno.Resizable = DataGridViewTriState.False;
            ByDateSno.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateSno.Width = 40;
            // 
            // ByDateDate
            // 
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.TopLeft;
            ByDateDate.DefaultCellStyle = dataGridViewCellStyle12;
            ByDateDate.HeaderText = "Date";
            ByDateDate.Name = "ByDateDate";
            ByDateDate.ReadOnly = true;
            ByDateDate.Resizable = DataGridViewTriState.False;
            ByDateDate.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateDate.Width = 150;
            // 
            // ByDateReference
            // 
            dataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.TopLeft;
            ByDateReference.DefaultCellStyle = dataGridViewCellStyle13;
            ByDateReference.HeaderText = "Reference";
            ByDateReference.Name = "ByDateReference";
            ByDateReference.ReadOnly = true;
            ByDateReference.Resizable = DataGridViewTriState.False;
            ByDateReference.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateReference.Width = 150;
            // 
            // ByDateLocation
            // 
            dataGridViewCellStyle14.Alignment = DataGridViewContentAlignment.TopLeft;
            ByDateLocation.DefaultCellStyle = dataGridViewCellStyle14;
            ByDateLocation.HeaderText = "Location";
            ByDateLocation.Name = "ByDateLocation";
            ByDateLocation.ReadOnly = true;
            ByDateLocation.Resizable = DataGridViewTriState.False;
            ByDateLocation.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateLocation.Width = 180;
            // 
            // ByDateQty
            // 
            dataGridViewCellStyle15.Alignment = DataGridViewContentAlignment.TopRight;
            ByDateQty.DefaultCellStyle = dataGridViewCellStyle15;
            ByDateQty.HeaderText = "Quantity";
            ByDateQty.Name = "ByDateQty";
            ByDateQty.ReadOnly = true;
            ByDateQty.Resizable = DataGridViewTriState.False;
            // 
            // ByDateFree
            // 
            dataGridViewCellStyle16.Alignment = DataGridViewContentAlignment.TopRight;
            ByDateFree.DefaultCellStyle = dataGridViewCellStyle16;
            ByDateFree.HeaderText = "Free";
            ByDateFree.Name = "ByDateFree";
            ByDateFree.ReadOnly = true;
            ByDateFree.Resizable = DataGridViewTriState.False;
            ByDateFree.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // ByDateEnterBy
            // 
            dataGridViewCellStyle17.Alignment = DataGridViewContentAlignment.TopLeft;
            ByDateEnterBy.DefaultCellStyle = dataGridViewCellStyle17;
            ByDateEnterBy.HeaderText = "Entered By";
            ByDateEnterBy.Name = "ByDateEnterBy";
            ByDateEnterBy.ReadOnly = true;
            ByDateEnterBy.Resizable = DataGridViewTriState.False;
            ByDateEnterBy.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByDateEnterBy.Width = 210;
            // 
            // ByDateId
            // 
            ByDateId.HeaderText = "Id";
            ByDateId.Name = "ByDateId";
            ByDateId.ReadOnly = true;
            ByDateId.Visible = false;
            // 
            // GridviewForByLocation
            // 
            GridviewForByLocation.AllowUserToAddRows = false;
            GridviewForByLocation.AllowUserToDeleteRows = false;
            GridviewForByLocation.AllowUserToResizeColumns = false;
            GridviewForByLocation.AllowUserToResizeRows = false;
            dataGridViewCellStyle19.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.BackColor = SystemColors.Control;
            dataGridViewCellStyle19.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle19.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle19.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle19.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle19.WrapMode = DataGridViewTriState.True;
            GridviewForByLocation.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle19;
            GridviewForByLocation.ColumnHeadersHeight = 20;
            GridviewForByLocation.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridviewForByLocation.Columns.AddRange(new DataGridViewColumn[] { ByLocationSno, ByLocationDate, ByLocationReference, ByLocationQty, ByLocationFree, ByLocationEnteredBy, ByLocationId });
            GridviewForByLocation.EnableHeadersVisualStyles = false;
            GridviewForByLocation.Location = new Point(3, 41);
            GridviewForByLocation.MultiSelect = false;
            GridviewForByLocation.Name = "GridviewForByLocation";
            GridviewForByLocation.ReadOnly = true;
            GridviewForByLocation.RowHeadersVisible = false;
            dataGridViewCellStyle26.BackColor = SystemColors.Control;
            dataGridViewCellStyle26.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle26.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle26.SelectionForeColor = SystemColors.WindowText;
            GridviewForByLocation.RowsDefaultCellStyle = dataGridViewCellStyle26;
            GridviewForByLocation.RowTemplate.Height = 20;
            GridviewForByLocation.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridviewForByLocation.ShowCellToolTips = false;
            GridviewForByLocation.Size = new Size(948, 451);
            GridviewForByLocation.TabIndex = 6;
            GridviewForByLocation.Visible = false;
            GridviewForByLocation.CellPainting += GridviewForByLocation_CellPainting;
            GridviewForByLocation.RowPostPaint += GridviewForByLocation_RowPostPaint;
            // 
            // ByLocationSno
            // 
            dataGridViewCellStyle20.Alignment = DataGridViewContentAlignment.TopLeft;
            ByLocationSno.DefaultCellStyle = dataGridViewCellStyle20;
            ByLocationSno.HeaderText = "#";
            ByLocationSno.Name = "ByLocationSno";
            ByLocationSno.ReadOnly = true;
            ByLocationSno.Resizable = DataGridViewTriState.False;
            ByLocationSno.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByLocationSno.Width = 80;
            // 
            // ByLocationDate
            // 
            dataGridViewCellStyle21.Alignment = DataGridViewContentAlignment.TopLeft;
            ByLocationDate.DefaultCellStyle = dataGridViewCellStyle21;
            ByLocationDate.HeaderText = "Date";
            ByLocationDate.Name = "ByLocationDate";
            ByLocationDate.ReadOnly = true;
            ByLocationDate.Resizable = DataGridViewTriState.False;
            ByLocationDate.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByLocationDate.Width = 200;
            // 
            // ByLocationReference
            // 
            dataGridViewCellStyle22.Alignment = DataGridViewContentAlignment.TopLeft;
            ByLocationReference.DefaultCellStyle = dataGridViewCellStyle22;
            ByLocationReference.HeaderText = "Reference";
            ByLocationReference.Name = "ByLocationReference";
            ByLocationReference.ReadOnly = true;
            ByLocationReference.Resizable = DataGridViewTriState.False;
            ByLocationReference.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByLocationReference.Width = 130;
            // 
            // ByLocationQty
            // 
            dataGridViewCellStyle23.Alignment = DataGridViewContentAlignment.TopRight;
            ByLocationQty.DefaultCellStyle = dataGridViewCellStyle23;
            ByLocationQty.HeaderText = "Quantity";
            ByLocationQty.Name = "ByLocationQty";
            ByLocationQty.ReadOnly = true;
            ByLocationQty.Resizable = DataGridViewTriState.False;
            ByLocationQty.SortMode = DataGridViewColumnSortMode.Programmatic;
            ByLocationQty.Width = 130;
            // 
            // ByLocationFree
            // 
            dataGridViewCellStyle24.Alignment = DataGridViewContentAlignment.TopRight;
            ByLocationFree.DefaultCellStyle = dataGridViewCellStyle24;
            ByLocationFree.HeaderText = "Free";
            ByLocationFree.Name = "ByLocationFree";
            ByLocationFree.ReadOnly = true;
            ByLocationFree.Resizable = DataGridViewTriState.False;
            ByLocationFree.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByLocationFree.Width = 130;
            // 
            // ByLocationEnteredBy
            // 
            dataGridViewCellStyle25.Alignment = DataGridViewContentAlignment.TopLeft;
            ByLocationEnteredBy.DefaultCellStyle = dataGridViewCellStyle25;
            ByLocationEnteredBy.HeaderText = "Entered By";
            ByLocationEnteredBy.Name = "ByLocationEnteredBy";
            ByLocationEnteredBy.ReadOnly = true;
            ByLocationEnteredBy.SortMode = DataGridViewColumnSortMode.NotSortable;
            ByLocationEnteredBy.Width = 260;
            // 
            // ByLocationId
            // 
            ByLocationId.HeaderText = "Id";
            ByLocationId.Name = "ByLocationId";
            ByLocationId.ReadOnly = true;
            ByLocationId.Visible = false;
            // 
            // FormDamageEntryReport
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(954, 562);
            Controls.Add(DamageBtnExit);
            Controls.Add(DamageBtnSave);
            Controls.Add(DamageBtnPrint);
            Controls.Add(DamageBtnReset);
            Controls.Add(statusStrip1);
            Controls.Add(ab2ToolStrip1);
            Controls.Add(GridviewForByDate);
            Controls.Add(GridviewForByItem);
            Controls.Add(GridviewForByLocation);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormDamageEntryReport";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Damage Entry Report";
            Load += FormDamageEntryReport_Load;
            ab2ToolStrip1.ResumeLayout(false);
            ab2ToolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridviewForByItem).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridviewForByDate).EndInit();
            ((System.ComponentModel.ISupportInitialize)GridviewForByLocation).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private fa.views.controls.Ab2ToolStrip ab2ToolStrip1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripComboBox damageEntryComboBox;
        private ToolStripLabel toolStripLabel2;
        private fa.views.controls.ToolStripCalendar FromDate;
        private ToolStripLabel toolStripLabel3;
        private fa.views.controls.ToolStripCalendar ToDate;
        private ToolStripButton BtnGo;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton ToolStripSave;
        private ToolStripButton ToolStripPrint;
        private fa.views.controls.DataViewVerticalScroll GridviewForByItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel DamageEntryErrMsg;
        private Button DamageBtnReset;
        private Button DamageBtnPrint;
        private Button DamageBtnSave;
        private Button DamageBtnExit;
        private fa.views.controls.DataViewVerticalScroll GridviewForByDate;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripLabel LabelLocation;
        private fa.views.controls.ToolstripCheckedTreeComboBox CheckedTreeComboLocation;
        private fa.views.controls.DataViewVerticalScroll GridviewForByLocation;
        private DataGridViewTextBoxColumn ByItemItemCode;
        private DataGridViewTextBoxColumn ByItemItemName;
        private DataGridViewTextBoxColumn ByItemDescription;
        private DataGridViewTextBoxColumn ByLocationItem;
        private DataGridViewTextBoxColumn ByDateItem;
        private ToolStripComboBox damageEntryComboBoxType;
        private DataGridViewTextBoxColumn ByDateBatNo;
        private DataGridViewTextBoxColumn ByDateExpDate;
        private DataGridViewTextBoxColumn ByDateUOM;
        private DataGridViewTextBoxColumn ByDateCreatedBy;
        private DataGridViewTextBoxColumn ByItemSNo;
        private DataGridViewTextBoxColumn ByItemCode;
        private DataGridViewTextBoxColumn ByItemName;
        private DataGridViewTextBoxColumn ByItemBatchNo;
        private DataGridViewTextBoxColumn ByItemExpDte;
        private fa.views.controls.grid.DataGridViewQuantityColumn ByItemQty;
        private DataGridViewTextBoxColumn ByItemFree;
        private DataGridViewTextBoxColumn ByItemId;
        private DataGridViewTextBoxColumn ByDateSno;
        private DataGridViewTextBoxColumn ByDateDate;
        private DataGridViewTextBoxColumn ByDateReference;
        private DataGridViewTextBoxColumn ByDateLocation;
        private fa.views.controls.grid.DataGridViewQuantityColumn ByDateQty;
        private DataGridViewTextBoxColumn ByDateFree;
        private DataGridViewTextBoxColumn ByDateEnterBy;
        private DataGridViewTextBoxColumn ByDateId;
        private DataGridViewTextBoxColumn ByLocationSno;
        private DataGridViewTextBoxColumn ByLocationDate;
        private DataGridViewTextBoxColumn ByLocationReference;
        private fa.views.controls.grid.DataGridViewQuantityColumn ByLocationQty;
        private DataGridViewTextBoxColumn ByLocationFree;
        private DataGridViewTextBoxColumn ByLocationEnteredBy;
        private DataGridViewTextBoxColumn ByLocationId;
    }
}