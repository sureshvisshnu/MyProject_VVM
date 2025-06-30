namespace Fa.reports.Inventory
{
    partial class FormExpiryReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExpiryReport));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            ExpiryReportFrmToolStrip = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            ComboExpiryReportLocation = new fa.views.controls.ToolstripCheckedTreeComboBox();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripLabel3 = new ToolStripLabel();
            ExpiryReportFromDate = new fa.views.controls.ToolStripCalendar();
            toolStripSeparator6 = new ToolStripSeparator();
            toolStripLabel2 = new ToolStripLabel();
            ExpirydateComboBox = new ToolStripComboBox();
            toolStripSeparator5 = new ToolStripSeparator();
            RunReportButton = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            ToolStripBtnSave = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            ToolStripBtnPrint = new ToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            ExpiryReportDataGridView = new fa.views.controls.DataViewVerticalScroll();
            Column1 = new DataGridViewTextBoxColumn();
            MId = new DataGridViewTextBoxColumn();
            NAM = new DataGridViewTextBoxColumn();
            UOM = new DataGridViewTextBoxColumn();
            BatchDetail = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            CSTK = new DataGridViewTextBoxColumn();
            Location = new DataGridViewTextBoxColumn();
            ExpiryReportstatusStrip1 = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            BtnExit = new Button();
            BtnPrint = new Button();
            BtnSave = new Button();
            BtnReset = new Button();
            ExpiryReportFrmToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ExpiryReportDataGridView).BeginInit();
            ExpiryReportstatusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Size = new Size(116, 22);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Size = new Size(116, 22);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Size = new Size(116, 22);
            // 
            // ExpiryReportFrmToolStrip
            // 
            ExpiryReportFrmToolStrip.BackColor = SystemColors.Control;
            ExpiryReportFrmToolStrip.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ExpiryReportFrmToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            ExpiryReportFrmToolStrip.ImageScalingSize = new Size(20, 20);
            ExpiryReportFrmToolStrip.Items.AddRange(new ToolStripItem[] { toolStripLabel1, ComboExpiryReportLocation, toolStripSeparator1, toolStripLabel3, ExpiryReportFromDate, toolStripSeparator6, toolStripLabel2, ExpirydateComboBox, toolStripSeparator5, RunReportButton, toolStripSeparator2, ToolStripBtnSave, toolStripSeparator3, ToolStripBtnPrint, toolStripSeparator4 });
            ExpiryReportFrmToolStrip.Location = new Point(0, 0);
            ExpiryReportFrmToolStrip.Name = "ExpiryReportFrmToolStrip";
            ExpiryReportFrmToolStrip.Padding = new Padding(5);
            ExpiryReportFrmToolStrip.Size = new Size(994, 38);
            ExpiryReportFrmToolStrip.TabIndex = 2;
            ExpiryReportFrmToolStrip.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(53, 25);
            toolStripLabel1.Text = "Location";
            // 
            // ComboExpiryReportLocation
            // 
            ComboExpiryReportLocation.AutoSize = false;
            ComboExpiryReportLocation.Name = "ComboExpiryReportLocation";
            ComboExpiryReportLocation.SelectedNode = null;
            ComboExpiryReportLocation.Size = new Size(175, 21);
            ComboExpiryReportLocation.NodeClickedEvent += ComboExpiryReportLocation_NodeClickedEvent;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 28);
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new Size(45, 25);
            toolStripLabel3.Text = "From : ";
            // 
            // ExpiryReportFromDate
            // 
            ExpiryReportFromDate.BackColor = Color.White;
            ExpiryReportFromDate.Date = null;
            ExpiryReportFromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ExpiryReportFromDate.Format = "MM/dd/yyyy";
            ExpiryReportFromDate.MaxDate = new DateTime(9997, 12, 31, 7, 32, 58, 0);
            ExpiryReportFromDate.MinDate = new DateTime(1900, 1, 1, 23, 58, 26, 0);
            ExpiryReportFromDate.Name = "ExpiryReportFromDate";
            ExpiryReportFromDate.Size = new Size(97, 25);
            ExpiryReportFromDate.Text = "toolStripCalendar1";
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(6, 28);
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(66, 25);
            toolStripLabel2.Text = "Expires in :";
            // 
            // ExpirydateComboBox
            // 
            ExpirydateComboBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            ExpirydateComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            ExpirydateComboBox.DropDownWidth = 75;
            ExpirydateComboBox.FlatStyle = FlatStyle.Standard;
            ExpirydateComboBox.Items.AddRange(new object[] { "15 ", "30 ", "45 ", "60 " });
            ExpirydateComboBox.Name = "ExpirydateComboBox";
            ExpirydateComboBox.Overflow = ToolStripItemOverflow.Never;
            ExpirydateComboBox.Size = new Size(75, 28);
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(6, 28);
            // 
            // RunReportButton
            // 
            RunReportButton.BackgroundImageLayout = ImageLayout.None;
            RunReportButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            RunReportButton.ImageTransparentColor = Color.Magenta;
            RunReportButton.Name = "RunReportButton";
            RunReportButton.Size = new Size(26, 25);
            RunReportButton.Text = "Go";
            RunReportButton.Click += RunReportButton_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 28);
            // 
            // ToolStripBtnSave
            // 
            ToolStripBtnSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnSave.Image = (Image)resources.GetObject("ToolStripBtnSave.Image");
            ToolStripBtnSave.ImageTransparentColor = Color.Black;
            ToolStripBtnSave.Name = "ToolStripBtnSave";
            ToolStripBtnSave.Size = new Size(24, 25);
            ToolStripBtnSave.Text = "Save";
            ToolStripBtnSave.Click += BtnSave_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 28);
            // 
            // ToolStripBtnPrint
            // 
            ToolStripBtnPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnPrint.Image = (Image)resources.GetObject("ToolStripBtnPrint.Image");
            ToolStripBtnPrint.ImageTransparentColor = Color.Black;
            ToolStripBtnPrint.Name = "ToolStripBtnPrint";
            ToolStripBtnPrint.Size = new Size(24, 25);
            ToolStripBtnPrint.Text = "Print";
            ToolStripBtnPrint.Click += BtnPrint_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 28);
            // 
            // ExpiryReportDataGridView
            // 
            ExpiryReportDataGridView.AllowUserToAddRows = false;
            ExpiryReportDataGridView.AllowUserToDeleteRows = false;
            ExpiryReportDataGridView.AllowUserToResizeColumns = false;
            ExpiryReportDataGridView.AllowUserToResizeRows = false;
            ExpiryReportDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            ExpiryReportDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            ExpiryReportDataGridView.ColumnHeadersHeight = 20;
            ExpiryReportDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            ExpiryReportDataGridView.Columns.AddRange(new DataGridViewColumn[] { Column1, MId, NAM, UOM, BatchDetail, Column2, CSTK, Location });
            ExpiryReportDataGridView.EnableHeadersVisualStyles = false;
            ExpiryReportDataGridView.Location = new Point(0, 38);
            ExpiryReportDataGridView.Name = "ExpiryReportDataGridView";
            ExpiryReportDataGridView.ReadOnly = true;
            ExpiryReportDataGridView.RowHeadersVisible = false;
            ExpiryReportDataGridView.RowHeadersWidth = 51;
            ExpiryReportDataGridView.RowTemplate.DefaultCellStyle.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ExpiryReportDataGridView.RowTemplate.Height = 20;
            ExpiryReportDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ExpiryReportDataGridView.ShowCellToolTips = false;
            ExpiryReportDataGridView.Size = new Size(993, 412);
            ExpiryReportDataGridView.TabIndex = 4;
            ExpiryReportDataGridView.CellPainting += ExpiryReportDataGridView_CellPainting;
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            Column1.DefaultCellStyle = dataGridViewCellStyle2;
            Column1.Frozen = true;
            Column1.HeaderText = "#";
            Column1.MinimumWidth = 6;
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 65;
            // 
            // MId
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            MId.DefaultCellStyle = dataGridViewCellStyle3;
            MId.Frozen = true;
            MId.HeaderText = " Code";
            MId.MinimumWidth = 6;
            MId.Name = "MId";
            MId.ReadOnly = true;
            MId.Resizable = DataGridViewTriState.False;
            MId.SortMode = DataGridViewColumnSortMode.NotSortable;
            MId.Width = 125;
            // 
            // NAM
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopLeft;
            NAM.DefaultCellStyle = dataGridViewCellStyle4;
            NAM.Frozen = true;
            NAM.HeaderText = "Name";
            NAM.MinimumWidth = 6;
            NAM.Name = "NAM";
            NAM.ReadOnly = true;
            NAM.Resizable = DataGridViewTriState.False;
            NAM.SortMode = DataGridViewColumnSortMode.NotSortable;
            NAM.Width = 345;
            // 
            // UOM
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.TopLeft;
            UOM.DefaultCellStyle = dataGridViewCellStyle5;
            UOM.Frozen = true;
            UOM.HeaderText = "UOM";
            UOM.MinimumWidth = 6;
            UOM.Name = "UOM";
            UOM.ReadOnly = true;
            UOM.Resizable = DataGridViewTriState.False;
            UOM.SortMode = DataGridViewColumnSortMode.NotSortable;
            UOM.Width = 125;
            // 
            // BatchDetail
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            BatchDetail.DefaultCellStyle = dataGridViewCellStyle6;
            BatchDetail.HeaderText = "Batch ";
            BatchDetail.MinimumWidth = 6;
            BatchDetail.Name = "BatchDetail";
            BatchDetail.ReadOnly = true;
            BatchDetail.Resizable = DataGridViewTriState.False;
            BatchDetail.SortMode = DataGridViewColumnSortMode.NotSortable;
            BatchDetail.Width = 125;
            // 
            // Column2
            // 
            Column2.HeaderText = " Exp Date";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Resizable = DataGridViewTriState.False;
            Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // CSTK
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle7.Format = "N2";
            dataGridViewCellStyle7.NullValue = "0.00";
            CSTK.DefaultCellStyle = dataGridViewCellStyle7;
            CSTK.HeaderText = "Quantity";
            CSTK.MinimumWidth = 6;
            CSTK.Name = "CSTK";
            CSTK.ReadOnly = true;
            CSTK.Resizable = DataGridViewTriState.False;
            CSTK.SortMode = DataGridViewColumnSortMode.NotSortable;
            CSTK.Width = 80;
            // 
            // Location
            // 
            Location.HeaderText = "Column2";
            Location.MinimumWidth = 6;
            Location.Name = "Location";
            Location.ReadOnly = true;
            Location.Resizable = DataGridViewTriState.False;
            Location.SortMode = DataGridViewColumnSortMode.NotSortable;
            Location.Visible = false;
            Location.Width = 125;
            // 
            // ExpiryReportstatusStrip1
            // 
            ExpiryReportstatusStrip1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ExpiryReportstatusStrip1.ImageScalingSize = new Size(20, 20);
            ExpiryReportstatusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsg });
            ExpiryReportstatusStrip1.Location = new Point(0, 497);
            ExpiryReportstatusStrip1.Name = "ExpiryReportstatusStrip1";
            ExpiryReportstatusStrip1.Size = new Size(994, 22);
            ExpiryReportstatusStrip1.TabIndex = 26;
            ExpiryReportstatusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(0, 17);
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(906, 462);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(72, 26);
            BtnExit.TabIndex = 25;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Enabled = false;
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(825, 462);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 26);
            BtnPrint.TabIndex = 24;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnSave
            // 
            BtnSave.Enabled = false;
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(744, 462);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 26);
            BtnSave.TabIndex = 23;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // BtnReset
            // 
            BtnReset.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnReset.Location = new Point(655, 462);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new Size(81, 26);
            BtnReset.TabIndex = 27;
            BtnReset.Text = "Reset [Esc]";
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // FormExpiryReport
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(994, 519);
            Controls.Add(BtnReset);
            Controls.Add(ExpiryReportstatusStrip1);
            Controls.Add(BtnExit);
            Controls.Add(BtnPrint);
            Controls.Add(BtnSave);
            Controls.Add(ExpiryReportFrmToolStrip);
            Controls.Add(ExpiryReportDataGridView);
            Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormExpiryReport";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Expiry Report";
            Load += ExpiryReport_Load;
            Controls.SetChildIndex(ExpiryReportDataGridView, 0);
            Controls.SetChildIndex(ExpiryReportFrmToolStrip, 0);
            Controls.SetChildIndex(BtnSave, 0);
            Controls.SetChildIndex(BtnPrint, 0);
            Controls.SetChildIndex(BtnExit, 0);
            Controls.SetChildIndex(ExpiryReportstatusStrip1, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(BtnReset, 0);
            ExpiryReportFrmToolStrip.ResumeLayout(false);
            ExpiryReportFrmToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ExpiryReportDataGridView).EndInit();
            ExpiryReportstatusStrip1.ResumeLayout(false);
            ExpiryReportstatusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip ExpiryReportFrmToolStrip;
        private ToolStripLabel toolStripLabel1;
        private fa.views.controls.ToolstripCheckedTreeComboBox ComboExpiryReportLocation;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel toolStripLabel3;
        private fa.views.controls.ToolStripCalendar ExpiryReportFromDate;
        private ToolStripLabel toolStripLabel2;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripButton RunReportButton;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton ToolStripBtnSave;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton ToolStripBtnPrint;
        private ToolStripSeparator toolStripSeparator4;
        private fa.views.controls.DataViewVerticalScroll ExpiryReportDataGridView;
        private StatusStrip ExpiryReportstatusStrip1;
        private ToolStripStatusLabel ErrorMsg;
        private Button BtnExit;
        private Button BtnPrint;
        private Button BtnSave;
        private Button BtnReset;
        private ToolStripComboBox ExpirydateComboBox;
        private ToolStripSeparator toolStripSeparator6;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn MId;
        private DataGridViewTextBoxColumn NAM;
        private DataGridViewTextBoxColumn UOM;
        private DataGridViewTextBoxColumn BatchDetail;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn CSTK;
        private DataGridViewTextBoxColumn Location;
    }
}