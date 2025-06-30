namespace fa.reports.Inventory
{
    partial class FormStockMovementReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStockMovementReport));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.StockMovementRptToolStrip = new fa.views.controls.Ab2ToolStrip();
            this.ToolStripLocationLabel = new System.Windows.Forms.ToolStripLabel();
            this.CheckedTreeComboLocation = new fa.views.controls.ToolstripCheckedTreeComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripItemLabel = new System.Windows.Forms.ToolStripLabel();
            this.ComboBoxItem = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripFromLabel = new System.Windows.Forms.ToolStripLabel();
            this.FromDate = new fa.views.controls.ToolStripCalendar();
            this.ToolStripToLabel = new System.Windows.Forms.ToolStripLabel();
            this.ToDate = new fa.views.controls.ToolStripCalendar();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.BtnGo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.BtnToolStripSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.BtnToolStripPrint = new System.Windows.Forms.ToolStripButton();
            this.GridViewStockMovement = new fa.views.controls.DataViewVerticalScroll();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Reference = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Source = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Destination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Balance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusStripStockMovementReport = new System.Windows.Forms.StatusStrip();
            this.ErrorMsgStockMovementReport = new System.Windows.Forms.ToolStripStatusLabel();
            this.BtnExit = new System.Windows.Forms.Button();
            this.BtnReset = new System.Windows.Forms.Button();
            this.BtnPrint = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.StockMovementRptToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewStockMovement)).BeginInit();
            this.StatusStripStockMovementReport.SuspendLayout();
            this.SuspendLayout();
            // 
            // StockMovementRptToolStrip
            // 
            this.StockMovementRptToolStrip.BackColor = System.Drawing.SystemColors.ControlLight;
            this.StockMovementRptToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.StockMovementRptToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.StockMovementRptToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripLocationLabel,
            this.CheckedTreeComboLocation,
            this.toolStripSeparator1,
            this.ToolStripItemLabel,
            this.ComboBoxItem,
            this.toolStripSeparator4,
            this.ToolStripFromLabel,
            this.FromDate,
            this.ToolStripToLabel,
            this.ToDate,
            this.toolStripSeparator5,
            this.BtnGo,
            this.toolStripSeparator2,
            this.BtnToolStripSave,
            this.toolStripSeparator3,
            this.BtnToolStripPrint});
            this.StockMovementRptToolStrip.Location = new System.Drawing.Point(0, 0);
            this.StockMovementRptToolStrip.Margin = new System.Windows.Forms.Padding(4);
            this.StockMovementRptToolStrip.Name = "StockMovementRptToolStrip";
            this.StockMovementRptToolStrip.Padding = new System.Windows.Forms.Padding(4);
            this.StockMovementRptToolStrip.Size = new System.Drawing.Size(1321, 36);
            this.StockMovementRptToolStrip.TabIndex = 3;
            this.StockMovementRptToolStrip.Text = "ab2ToolStrip1";
            // 
            // ToolStripLocationLabel
            // 
            this.ToolStripLocationLabel.Name = "ToolStripLocationLabel";
            this.ToolStripLocationLabel.Size = new System.Drawing.Size(53, 25);
            this.ToolStripLocationLabel.Text = "Location";
            // 
            // CheckedTreeComboLocation
            // 
            this.CheckedTreeComboLocation.AutoSize = false;
            this.CheckedTreeComboLocation.Name = "CheckedTreeComboLocation";
            this.CheckedTreeComboLocation.SelectedNode = null;
            this.CheckedTreeComboLocation.Size = new System.Drawing.Size(150, 21);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // ToolStripItemLabel
            // 
            this.ToolStripItemLabel.Name = "ToolStripItemLabel";
            this.ToolStripItemLabel.Size = new System.Drawing.Size(31, 25);
            this.ToolStripItemLabel.Text = "Item";
            // 
            // ComboBoxItem
            // 
            this.ComboBoxItem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ComboBoxItem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboBoxItem.AutoSize = false;
            this.ComboBoxItem.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.ComboBoxItem.Name = "ComboBoxItem";
            this.ComboBoxItem.Size = new System.Drawing.Size(150, 23);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 28);
            // 
            // ToolStripFromLabel
            // 
            this.ToolStripFromLabel.Name = "ToolStripFromLabel";
            this.ToolStripFromLabel.Size = new System.Drawing.Size(35, 25);
            this.ToolStripFromLabel.Text = "From";
            // 
            // FromDate
            // 
            this.FromDate.BackColor = System.Drawing.Color.White;
            this.FromDate.Date = null;
            this.FromDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FromDate.Format = "MM/dd/yyyy";
            this.FromDate.MaxDate = new System.DateTime(9997, 12, 31, 7, 30, 34, 0);
            this.FromDate.MinDate = new System.DateTime(1900, 1, 1, 23, 3, 20, 0);
            this.FromDate.Name = "FromDate";
            this.FromDate.Size = new System.Drawing.Size(97, 25);
            this.FromDate.Text = "toolStripCalendar2";
            // 
            // ToolStripToLabel
            // 
            this.ToolStripToLabel.Name = "ToolStripToLabel";
            this.ToolStripToLabel.Size = new System.Drawing.Size(20, 25);
            this.ToolStripToLabel.Text = "To";
            // 
            // ToDate
            // 
            this.ToDate.BackColor = System.Drawing.Color.White;
            this.ToDate.Date = null;
            this.ToDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ToDate.Format = "MM/dd/yyyy";
            this.ToDate.MaxDate = new System.DateTime(9997, 12, 31, 7, 30, 34, 0);
            this.ToDate.MinDate = new System.DateTime(1900, 1, 1, 23, 3, 20, 0);
            this.ToDate.Name = "ToDate";
            this.ToDate.Size = new System.Drawing.Size(97, 25);
            this.ToDate.Text = "toolStripCalendar1";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.ForeColor = System.Drawing.SystemColors.Control;
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 28);
            // 
            // BtnGo
            // 
            this.BtnGo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BtnGo.Image = ((System.Drawing.Image)(resources.GetObject("BtnGo.Image")));
            this.BtnGo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnGo.Name = "BtnGo";
            this.BtnGo.Size = new System.Drawing.Size(26, 25);
            this.BtnGo.Text = "Go";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 28);
            // 
            // BtnToolStripSave
            // 
            this.BtnToolStripSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnToolStripSave.Image = ((System.Drawing.Image)(resources.GetObject("BtnToolStripSave.Image")));
            this.BtnToolStripSave.ImageTransparentColor = System.Drawing.Color.Black;
            this.BtnToolStripSave.Name = "BtnToolStripSave";
            this.BtnToolStripSave.Size = new System.Drawing.Size(24, 25);
            this.BtnToolStripSave.Text = "Save";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 28);
            // 
            // BtnToolStripPrint
            // 
            this.BtnToolStripPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnToolStripPrint.Image = ((System.Drawing.Image)(resources.GetObject("BtnToolStripPrint.Image")));
            this.BtnToolStripPrint.ImageTransparentColor = System.Drawing.Color.Black;
            this.BtnToolStripPrint.Name = "BtnToolStripPrint";
            this.BtnToolStripPrint.Size = new System.Drawing.Size(24, 25);
            this.BtnToolStripPrint.Text = "Print";
            // 
            // GridViewStockMovement
            // 
            this.GridViewStockMovement.AllowUserToAddRows = false;
            this.GridViewStockMovement.AllowUserToDeleteRows = false;
            this.GridViewStockMovement.AllowUserToResizeColumns = false;
            this.GridViewStockMovement.AllowUserToResizeRows = false;
            this.GridViewStockMovement.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.GridViewStockMovement.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewStockMovement.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Date,
            this.Reference,
            this.Source,
            this.Destination,
            this.Type,
            this.Qty,
            this.Price,
            this.Value,
            this.Balance});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.GridViewStockMovement.DefaultCellStyle = dataGridViewCellStyle4;
            this.GridViewStockMovement.Location = new System.Drawing.Point(7, 41);
            this.GridViewStockMovement.Margin = new System.Windows.Forms.Padding(2);
            this.GridViewStockMovement.Name = "GridViewStockMovement";
            this.GridViewStockMovement.ReadOnly = true;
            this.GridViewStockMovement.RowHeadersVisible = false;
            this.GridViewStockMovement.RowHeadersWidth = 51;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GridViewStockMovement.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.GridViewStockMovement.RowTemplate.Height = 24;
            this.GridViewStockMovement.Size = new System.Drawing.Size(1307, 431);
            this.GridViewStockMovement.TabIndex = 4;
            // 
            // Date
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.Date.DefaultCellStyle = dataGridViewCellStyle1;
            this.Date.HeaderText = "Date";
            this.Date.MinimumWidth = 6;
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            this.Date.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Date.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Reference
            // 
            this.Reference.HeaderText = "Reference";
            this.Reference.MinimumWidth = 6;
            this.Reference.Name = "Reference";
            this.Reference.ReadOnly = true;
            this.Reference.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Reference.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Source
            // 
            this.Source.HeaderText = "Source";
            this.Source.MinimumWidth = 6;
            this.Source.Name = "Source";
            this.Source.ReadOnly = true;
            this.Source.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Source.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Source.Width = 200;
            // 
            // Destination
            // 
            this.Destination.HeaderText = "Destination";
            this.Destination.MinimumWidth = 6;
            this.Destination.Name = "Destination";
            this.Destination.ReadOnly = true;
            this.Destination.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Destination.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Destination.Width = 200;
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.MinimumWidth = 6;
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            this.Type.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Type.Width = 225;
            // 
            // Qty
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Qty.DefaultCellStyle = dataGridViewCellStyle2;
            this.Qty.HeaderText = "Qty";
            this.Qty.MinimumWidth = 6;
            this.Qty.Name = "Qty";
            this.Qty.ReadOnly = true;
            this.Qty.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Qty.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Price
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Price.DefaultCellStyle = dataGridViewCellStyle3;
            this.Price.HeaderText = "Price";
            this.Price.MinimumWidth = 6;
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            this.Price.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Price.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Price.Width = 110;
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            this.Value.Width = 125;
            // 
            // Balance
            // 
            this.Balance.HeaderText = "Running Balance";
            this.Balance.Name = "Balance";
            this.Balance.ReadOnly = true;
            this.Balance.Width = 120;
            // 
            // StatusStripStockMovementReport
            // 
            this.StatusStripStockMovementReport.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.StatusStripStockMovementReport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ErrorMsgStockMovementReport});
            this.StatusStripStockMovementReport.Location = new System.Drawing.Point(0, 517);
            this.StatusStripStockMovementReport.Name = "StatusStripStockMovementReport";
            this.StatusStripStockMovementReport.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.StatusStripStockMovementReport.Size = new System.Drawing.Size(1321, 22);
            this.StatusStripStockMovementReport.TabIndex = 5;
            this.StatusStripStockMovementReport.Text = "statusStrip1";
            // 
            // ErrorMsgStockMovementReport
            // 
            this.ErrorMsgStockMovementReport.Name = "ErrorMsgStockMovementReport";
            this.ErrorMsgStockMovementReport.Size = new System.Drawing.Size(34, 17);
            this.ErrorMsgStockMovementReport.Text = "         ";
            // 
            // BtnExit
            // 
            this.BtnExit.Location = new System.Drawing.Point(1212, 484);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(72, 23);
            this.BtnExit.TabIndex = 25;
            this.BtnExit.Text = "Exit [F10]";
            this.BtnExit.UseVisualStyleBackColor = true;
            // 
            // BtnReset
            // 
            this.BtnReset.Location = new System.Drawing.Point(962, 484);
            this.BtnReset.Name = "BtnReset";
            this.BtnReset.Size = new System.Drawing.Size(82, 23);
            this.BtnReset.TabIndex = 24;
            this.BtnReset.Text = "Reset [Esc]";
            this.BtnReset.UseVisualStyleBackColor = true;
            // 
            // BtnPrint
            // 
            this.BtnPrint.Enabled = false;
            this.BtnPrint.Location = new System.Drawing.Point(1131, 484);
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.Size = new System.Drawing.Size(75, 23);
            this.BtnPrint.TabIndex = 23;
            this.BtnPrint.Text = "Print [F9]";
            this.BtnPrint.UseVisualStyleBackColor = true;
            // 
            // BtnSave
            // 
            this.BtnSave.Enabled = false;
            this.BtnSave.Location = new System.Drawing.Point(1050, 484);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 23);
            this.BtnSave.TabIndex = 22;
            this.BtnSave.Text = "Save [F8]";
            this.BtnSave.UseVisualStyleBackColor = true;
            // 
            // FormStockMovementReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1321, 539);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.BtnReset);
            this.Controls.Add(this.BtnPrint);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.StatusStripStockMovementReport);
            this.Controls.Add(this.GridViewStockMovement);
            this.Controls.Add(this.StockMovementRptToolStrip);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormStockMovementReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Stock Movement Report";
            this.StockMovementRptToolStrip.ResumeLayout(false);
            this.StockMovementRptToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewStockMovement)).EndInit();
            this.StatusStripStockMovementReport.ResumeLayout(false);
            this.StatusStripStockMovementReport.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private views.controls.Ab2ToolStrip StockMovementRptToolStrip;
        private System.Windows.Forms.ToolStripLabel ToolStripItemLabel;
        private System.Windows.Forms.ToolStripComboBox ComboBoxItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel ToolStripLocationLabel;
        private views.controls.ToolstripCheckedTreeComboBox CheckedTreeComboLocation;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel ToolStripFromLabel;
        private views.controls.ToolStripCalendar FromDate;
        private System.Windows.Forms.ToolStripLabel ToolStripToLabel;
        private views.controls.ToolStripCalendar ToDate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton BtnGo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton BtnToolStripSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton BtnToolStripPrint;
        private views.controls.DataViewVerticalScroll GridViewStockMovement;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Reference;
        private System.Windows.Forms.DataGridViewTextBoxColumn Source;
        private System.Windows.Forms.DataGridViewTextBoxColumn Destination;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn Balance;
        private System.Windows.Forms.StatusStrip StatusStripStockMovementReport;
        private System.Windows.Forms.ToolStripStatusLabel ErrorMsgStockMovementReport;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Button BtnReset;
        private System.Windows.Forms.Button BtnPrint;
        private System.Windows.Forms.Button BtnSave;
    }
}