using NPOI.SS.Formula.Functions;

namespace fa.reports.Inventory
{
    partial class FormItemLedger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormItemLedger));
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle19 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle20 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle14 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle16 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle17 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle18 = new DataGridViewCellStyle();
            statusStrip1 = new StatusStrip();
            ErrorMsgItemLedger = new ToolStripStatusLabel();
            BtnPrint = new Button();
            BtnSave = new Button();
            BtnExit = new Button();
            ab2ToolStrip1 = new views.controls.Ab2ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            CheckedComboBoxItem = new views.controls.ToolstripCheckedComboBox();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripLabel5 = new ToolStripLabel();
            CheckedTreeComboLocation = new views.controls.ToolstripCheckedTreeComboBox();
            toolStripSeparator4 = new ToolStripSeparator();
            toolStripLabel2 = new ToolStripLabel();
            FromDate = new views.controls.ToolStripCalendar();
            toolStripLabel3 = new ToolStripLabel();
            ToDate = new views.controls.ToolStripCalendar();
            toolStripSeparator5 = new ToolStripSeparator();
            CheckBoxLabeledBatchWise = new views.controls.ToolStripCheckBox();
            CheckBoxAddValue = new views.controls.ToolStripCheckBox();
            BtnGo = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            BtnToolStripSave = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            BtnToolStripPrint = new ToolStripButton();
            GridViewItemLedger = new views.controls.DataViewVerticalScroll();
            btnReset = new Button();
            Date = new DataGridViewTextBoxColumn();
            TransactionType = new DataGridViewTextBoxColumn();
            From = new DataGridViewTextBoxColumn();
            To = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Description = new DataGridViewTextBoxColumn();
            BatchDetail = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Uom = new DataGridViewTextBoxColumn();
            Qty = new DataGridViewTextBoxColumn();
            Price = new DataGridViewTextBoxColumn();
            Value = new DataGridViewTextBoxColumn();
            Stock = new DataGridViewTextBoxColumn();
            ProductName = new DataGridViewTextBoxColumn();
            LID = new DataGridViewTextBoxColumn();
            statusStrip1.SuspendLayout();
            ab2ToolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewItemLedger).BeginInit();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsgItemLedger });
            statusStrip1.Location = new Point(0, 599);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 10, 0);
            statusStrip1.Size = new Size(1299, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsgItemLedger
            // 
            ErrorMsgItemLedger.Name = "ErrorMsgItemLedger";
            ErrorMsgItemLedger.Size = new Size(34, 17);
            ErrorMsgItemLedger.Text = "         ";
            // 
            // BtnPrint
            // 
            BtnPrint.Location = new Point(1125, 560);
            BtnPrint.Margin = new Padding(2);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(74, 24);
            BtnPrint.TabIndex = 3;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnSave
            // 
            BtnSave.Location = new Point(1051, 560);
            BtnSave.Margin = new Padding(2);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(70, 24);
            BtnSave.TabIndex = 4;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // BtnExit
            // 
            BtnExit.Location = new Point(1203, 560);
            BtnExit.Margin = new Padding(2);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(67, 24);
            BtnExit.TabIndex = 5;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // ab2ToolStrip1
            // 
            ab2ToolStrip1.AutoSize = false;
            ab2ToolStrip1.BackColor = SystemColors.ControlLight;
            ab2ToolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            ab2ToolStrip1.ImageScalingSize = new Size(20, 20);
            ab2ToolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, CheckedComboBoxItem, toolStripSeparator1, toolStripLabel5, CheckedTreeComboLocation, toolStripSeparator4, toolStripLabel2, FromDate, toolStripLabel3, ToDate, toolStripSeparator5, CheckBoxLabeledBatchWise, CheckBoxAddValue, BtnGo, toolStripSeparator2, BtnToolStripSave, toolStripSeparator3, BtnToolStripPrint });
            ab2ToolStrip1.Location = new Point(0, 0);
            ab2ToolStrip1.Margin = new Padding(4);
            ab2ToolStrip1.Name = "ab2ToolStrip1";
            ab2ToolStrip1.Padding = new Padding(4);
            ab2ToolStrip1.Size = new Size(1299, 36);
            ab2ToolStrip1.TabIndex = 2;
            ab2ToolStrip1.Text = "ab2ToolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(31, 25);
            toolStripLabel1.Text = "Item";
            // 
            // CheckedComboBoxItem
            // 
            CheckedComboBoxItem.Delay = true;
            CheckedComboBoxItem.DelayTime = 1500;
            CheckedComboBoxItem.MaxDropDownItems = 30;
            CheckedComboBoxItem.Name = "CheckedComboBoxItem";
            CheckedComboBoxItem.Searchstartfrom = 2;
            CheckedComboBoxItem.Size = new Size(350, 25);
            CheckedComboBoxItem.ItemCheckedEvent += CheckedComboBoxItem_ItemCheckedEvent;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 28);
            // 
            // toolStripLabel5
            // 
            toolStripLabel5.Name = "toolStripLabel5";
            toolStripLabel5.Size = new Size(53, 25);
            toolStripLabel5.Text = "Location";
            // 
            // CheckedTreeComboLocation
            // 
            CheckedTreeComboLocation.AutoSize = false;
            CheckedTreeComboLocation.Name = "CheckedTreeComboLocation";
            CheckedTreeComboLocation.SelectedNode = null;
            CheckedTreeComboLocation.Size = new Size(171, 21);
            CheckedTreeComboLocation.NodeClickedEvent += CheckedTreeComboLocation_NodeClickedEvent;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 28);
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(35, 25);
            toolStripLabel2.Text = "From";
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
            // toolStripLabel3
            // 
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new Size(19, 25);
            toolStripLabel3.Text = "To";
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
            // CheckBoxLabeledBatchWise
            // 
            CheckBoxLabeledBatchWise.BackColor = SystemColors.ControlLight;
            CheckBoxLabeledBatchWise.Checked = false;
            CheckBoxLabeledBatchWise.CheckState = CheckState.Unchecked;
            CheckBoxLabeledBatchWise.Name = "CheckBoxLabeledBatchWise";
            CheckBoxLabeledBatchWise.Size = new Size(81, 25);
            CheckBoxLabeledBatchWise.Text = "BatchWise";
            // 
            // CheckBoxAddValue
            // 
            CheckBoxAddValue.AutoSize = false;
            CheckBoxAddValue.BackColor = SystemColors.ControlLight;
            CheckBoxAddValue.Checked = false;
            CheckBoxAddValue.CheckState = CheckState.Unchecked;
            CheckBoxAddValue.Name = "CheckBoxAddValue";
            CheckBoxAddValue.Size = new Size(90, 19);
            CheckBoxAddValue.Text = "Show Value";
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
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 28);
            // 
            // BtnToolStripSave
            // 
            BtnToolStripSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            BtnToolStripSave.Image = (Image)resources.GetObject("BtnToolStripSave.Image");
            BtnToolStripSave.ImageTransparentColor = Color.Black;
            BtnToolStripSave.Name = "BtnToolStripSave";
            BtnToolStripSave.Size = new Size(24, 25);
            BtnToolStripSave.Text = "Save";
            BtnToolStripSave.Click += BtnSave_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 28);
            // 
            // BtnToolStripPrint
            // 
            BtnToolStripPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            BtnToolStripPrint.Image = (Image)resources.GetObject("BtnToolStripPrint.Image");
            BtnToolStripPrint.ImageTransparentColor = Color.Black;
            BtnToolStripPrint.Name = "BtnToolStripPrint";
            BtnToolStripPrint.Size = new Size(24, 25);
            BtnToolStripPrint.Text = "Print";
            BtnToolStripPrint.Click += BtnPrint_Click;
            // 
            // GridViewItemLedger
            // 
            GridViewItemLedger.AllowUserToAddRows = false;
            GridViewItemLedger.AllowUserToDeleteRows = false;
            GridViewItemLedger.AllowUserToResizeColumns = false;
            GridViewItemLedger.AllowUserToResizeRows = false;
            GridViewItemLedger.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = SystemColors.Control;
            dataGridViewCellStyle11.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle11.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle11.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.True;
            GridViewItemLedger.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
            GridViewItemLedger.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GridViewItemLedger.Columns.AddRange(new DataGridViewColumn[] { Date, TransactionType, From, To, Column2, Description, BatchDetail, Column1, Uom, Qty, Price, Value, Stock, ProductName, LID });
            dataGridViewCellStyle19.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.BackColor = Color.White;
            dataGridViewCellStyle19.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle19.ForeColor = Color.Black;
            dataGridViewCellStyle19.SelectionBackColor = Color.White;
            dataGridViewCellStyle19.SelectionForeColor = Color.Black;
            dataGridViewCellStyle19.WrapMode = DataGridViewTriState.False;
            GridViewItemLedger.DefaultCellStyle = dataGridViewCellStyle19;
            GridViewItemLedger.EnableHeadersVisualStyles = false;
            GridViewItemLedger.Location = new Point(5, 40);
            GridViewItemLedger.Margin = new Padding(2);
            GridViewItemLedger.Name = "GridViewItemLedger";
            GridViewItemLedger.ReadOnly = true;
            GridViewItemLedger.RowHeadersVisible = false;
            GridViewItemLedger.RowHeadersWidth = 51;
            dataGridViewCellStyle20.BackColor = Color.White;
            dataGridViewCellStyle20.ForeColor = Color.Black;
            dataGridViewCellStyle20.SelectionBackColor = Color.White;
            dataGridViewCellStyle20.SelectionForeColor = Color.Black;
            dataGridViewCellStyle20.WrapMode = DataGridViewTriState.True;
            GridViewItemLedger.RowsDefaultCellStyle = dataGridViewCellStyle20;
            GridViewItemLedger.RowTemplate.Height = 24;
            GridViewItemLedger.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewItemLedger.ShowCellToolTips = false;
            GridViewItemLedger.Size = new Size(1290, 504);
            GridViewItemLedger.TabIndex = 0;
            GridViewItemLedger.CellPainting += GridViewItemLedger_CellPainting;
            GridViewItemLedger.RowPostPaint += GridViewItemLedger_RowPostPaint;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(971, 560);
            btnReset.Margin = new Padding(2);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(75, 24);
            btnReset.TabIndex = 6;
            btnReset.Text = "Reset [Esc]";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // Date
            // 
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.TopLeft;
            Date.DefaultCellStyle = dataGridViewCellStyle12;
            Date.HeaderText = "Date";
            Date.MinimumWidth = 6;
            Date.Name = "Date";
            Date.ReadOnly = true;
            Date.Resizable = DataGridViewTriState.False;
            Date.SortMode = DataGridViewColumnSortMode.NotSortable;
            Date.Width = 75;
            // 
            // TransactionType
            // 
            TransactionType.HeaderText = "Transaction Type";
            TransactionType.MinimumWidth = 6;
            TransactionType.Name = "TransactionType";
            TransactionType.ReadOnly = true;
            TransactionType.Resizable = DataGridViewTriState.False;
            TransactionType.SortMode = DataGridViewColumnSortMode.NotSortable;
            TransactionType.Width = 113;
            // 
            // From
            // 
            From.HeaderText = "From";
            From.MinimumWidth = 6;
            From.Name = "From";
            From.ReadOnly = true;
            From.Resizable = DataGridViewTriState.False;
            From.SortMode = DataGridViewColumnSortMode.NotSortable;
            From.Width = 130;
            // 
            // To
            // 
            To.HeaderText = "To";
            To.MinimumWidth = 6;
            To.Name = "To";
            To.ReadOnly = true;
            To.Resizable = DataGridViewTriState.False;
            To.SortMode = DataGridViewColumnSortMode.NotSortable;
            To.Width = 130;
            // 
            // Column2
            // 
            dataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.TopLeft;
            Column2.DefaultCellStyle = dataGridViewCellStyle13;
            Column2.HeaderText = "Rack No.";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Resizable = DataGridViewTriState.False;
            Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column2.Width = 75;
            // 
            // Description
            // 
            Description.HeaderText = "Description";
            Description.MinimumWidth = 6;
            Description.Name = "Description";
            Description.ReadOnly = true;
            Description.Resizable = DataGridViewTriState.False;
            Description.SortMode = DataGridViewColumnSortMode.NotSortable;
            Description.Width = 180;
            // 
            // BatchDetail
            // 
            BatchDetail.HeaderText = "Batch #";
            BatchDetail.MinimumWidth = 6;
            BatchDetail.Name = "BatchDetail";
            BatchDetail.ReadOnly = true;
            BatchDetail.Resizable = DataGridViewTriState.False;
            BatchDetail.SortMode = DataGridViewColumnSortMode.NotSortable;
            BatchDetail.Width = 75;
            // 
            // Column1
            // 
            dataGridViewCellStyle14.Alignment = DataGridViewContentAlignment.TopLeft;
            Column1.DefaultCellStyle = dataGridViewCellStyle14;
            Column1.HeaderText = "Expiry";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 75;
            // 
            // Uom
            // 
            Uom.HeaderText = "UOM";
            Uom.Name = "Uom";
            Uom.ReadOnly = true;
            Uom.Resizable = DataGridViewTriState.False;
            Uom.SortMode = DataGridViewColumnSortMode.NotSortable;
            Uom.Width = 75;
            // 
            // Qty
            // 
            dataGridViewCellStyle15.Alignment = DataGridViewContentAlignment.MiddleRight;
            Qty.DefaultCellStyle = dataGridViewCellStyle15;
            Qty.HeaderText = "Qty";
            Qty.MinimumWidth = 6;
            Qty.Name = "Qty";
            Qty.ReadOnly = true;
            Qty.Resizable = DataGridViewTriState.False;
            Qty.SortMode = DataGridViewColumnSortMode.NotSortable;
            Qty.Width = 75;
            // 
            // Price
            // 
            dataGridViewCellStyle16.Alignment = DataGridViewContentAlignment.MiddleRight;
            Price.DefaultCellStyle = dataGridViewCellStyle16;
            Price.HeaderText = "Cost";
            Price.Name = "Price";
            Price.ReadOnly = true;
            Price.SortMode = DataGridViewColumnSortMode.NotSortable;
            Price.Width = 75;
            // 
            // Value
            // 
            dataGridViewCellStyle17.Alignment = DataGridViewContentAlignment.MiddleRight;
            Value.DefaultCellStyle = dataGridViewCellStyle17;
            Value.HeaderText = "Value";
            Value.Name = "Value";
            Value.ReadOnly = true;
            Value.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Stock
            // 
            dataGridViewCellStyle18.Alignment = DataGridViewContentAlignment.MiddleRight;
            Stock.DefaultCellStyle = dataGridViewCellStyle18;
            Stock.HeaderText = "Stock";
            Stock.MinimumWidth = 6;
            Stock.Name = "Stock";
            Stock.ReadOnly = true;
            Stock.Resizable = DataGridViewTriState.False;
            Stock.SortMode = DataGridViewColumnSortMode.NotSortable;
            Stock.Width = 95;
            // 
            // ProductName
            // 
            ProductName.HeaderText = "ProductName";
            ProductName.Name = "ProductName";
            ProductName.ReadOnly = true;
            ProductName.Visible = false;
            // 
            // LID
            // 
            LID.HeaderText = "LID";
            LID.Name = "LID";
            LID.ReadOnly = true;
            LID.Visible = false;
            // 
            // FormItemLedger
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1299, 621);
            Controls.Add(btnReset);
            Controls.Add(BtnExit);
            Controls.Add(BtnSave);
            Controls.Add(BtnPrint);
            Controls.Add(ab2ToolStrip1);
            Controls.Add(statusStrip1);
            Controls.Add(GridViewItemLedger);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormItemLedger";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Item Ledger (Stock)";
            Load += FormItemLedger_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ab2ToolStrip1.ResumeLayout(false);
            ab2ToolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewItemLedger).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private views.controls.DataViewVerticalScroll GridViewItemLedger;
        private StatusStrip statusStrip1;
        private views.controls.Ab2ToolStrip ab2ToolStrip1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel toolStripLabel2;
        private ToolStripLabel toolStripLabel3;
        private ToolStripButton BtnGo;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton BtnToolStripSave;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton BtnToolStripPrint;
        private Button BtnPrint;
        private Button BtnSave;
        private Button BtnExit;
        private ToolStripLabel toolStripLabel5;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private views.controls.ToolStripCalendar ToDate;
        private views.controls.ToolStripCalendar FromDate;
        private ToolStripStatusLabel ErrorMsgItemLedger;
        private BarcodeLib.Barcode barcodeXML1;
        private views.controls.ToolStripCheckBox CheckBoxLabeledBatchWise;
        private views.controls.ToolstripCheckedTreeComboBox CheckedTreeComboLocation;
        private views.controls.ToolStripCheckBox CheckBoxAddValue;
        private Button btnReset;
        private views.controls.ToolstripCheckedComboBox CheckedComboBoxItem;
        private DataGridViewTextBoxColumn Date;
        private DataGridViewTextBoxColumn TransactionType;
        private DataGridViewTextBoxColumn From;
        private DataGridViewTextBoxColumn To;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Description;
        private DataGridViewTextBoxColumn BatchDetail;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Uom;
        private DataGridViewTextBoxColumn Qty;
        private DataGridViewTextBoxColumn Price;
        private DataGridViewTextBoxColumn Value;
        private DataGridViewTextBoxColumn Stock;
        private DataGridViewTextBoxColumn ProductName;
        private DataGridViewTextBoxColumn LID;
    }
}