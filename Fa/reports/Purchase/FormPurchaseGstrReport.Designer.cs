namespace fa.reports.Purchase
{
    partial class FormPurchaseGstrReport
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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPurchaseGstrReport));
            statusStrip1 = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            BtnExit = new Button();
            BtnCancel = new Button();
            BtnSave = new Button();
            GridViewBtoB = new views.controls.DataViewVerticalScroll();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            Column9 = new DataGridViewTextBoxColumn();
            dataGridViewCurrencyColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewCurrencyColumn6 = new DataGridViewTextBoxColumn();
            Column8 = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            Column7 = new DataGridViewTextBoxColumn();
            ab2ToolStrip1 = new views.controls.Ab2ToolStrip();
            toolStripLabel2 = new ToolStripLabel();
            FromDate = new views.controls.ToolStripCalendar();
            toolStripLabel3 = new ToolStripLabel();
            ToDate = new views.controls.ToolStripCalendar();
            BtnGo = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            ToolStripSave = new ToolStripButton();
            ToolStripPrint = new ToolStripButton();
            BtnPrint = new Button();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewBtoB).BeginInit();
            ab2ToolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsg });
            statusStrip1.Location = new Point(0, 540);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1224, 22);
            statusStrip1.TabIndex = 13;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(55, 17);
            ErrorMsg.Text = "                ";
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(1120, 505);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(72, 23);
            BtnExit.TabIndex = 19;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(863, 505);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(82, 23);
            BtnCancel.TabIndex = 18;
            BtnCancel.Text = "Reset [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // BtnSave
            // 
            BtnSave.Enabled = false;
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(1039, 505);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 17;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // GridViewBtoB
            // 
            GridViewBtoB.AllowUserToAddRows = false;
            GridViewBtoB.AllowUserToDeleteRows = false;
            GridViewBtoB.AllowUserToResizeColumns = false;
            GridViewBtoB.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewBtoB.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewBtoB.ColumnHeadersHeight = 20;
            GridViewBtoB.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewBtoB.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, Column9, dataGridViewCurrencyColumn3, dataGridViewCurrencyColumn6, Column8, Column1, Column2, Column3, Column4, Column5, Column6, Column7 });
            GridViewBtoB.EnableHeadersVisualStyles = false;
            GridViewBtoB.Location = new Point(4, 41);
            GridViewBtoB.Name = "GridViewBtoB";
            GridViewBtoB.ReadOnly = true;
            GridViewBtoB.RowHeadersVisible = false;
            dataGridViewCellStyle10.BackColor = Color.White;
            dataGridViewCellStyle10.ForeColor = Color.Black;
            dataGridViewCellStyle10.SelectionBackColor = Color.White;
            dataGridViewCellStyle10.SelectionForeColor = Color.Black;
            GridViewBtoB.RowsDefaultCellStyle = dataGridViewCellStyle10;
            GridViewBtoB.RowTemplate.Height = 20;
            GridViewBtoB.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewBtoB.ShowCellToolTips = false;
            GridViewBtoB.Size = new Size(1216, 450);
            GridViewBtoB.TabIndex = 12;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewTextBoxColumn4.HeaderText = "#";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            dataGridViewTextBoxColumn4.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn4.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn4.Width = 25;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "GSTN ";
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.ReadOnly = true;
            dataGridViewTextBoxColumn5.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn5.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column9
            // 
            Column9.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column9.HeaderText = "Supplier Name";
            Column9.Name = "Column9";
            Column9.ReadOnly = true;
            Column9.Resizable = DataGridViewTriState.False;
            Column9.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewCurrencyColumn3
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCurrencyColumn3.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCurrencyColumn3.HeaderText = "Invoice";
            dataGridViewCurrencyColumn3.Name = "dataGridViewCurrencyColumn3";
            dataGridViewCurrencyColumn3.ReadOnly = true;
            dataGridViewCurrencyColumn3.Resizable = DataGridViewTriState.False;
            dataGridViewCurrencyColumn3.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewCurrencyColumn3.Width = 90;
            // 
            // dataGridViewCurrencyColumn6
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCurrencyColumn6.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCurrencyColumn6.HeaderText = "Date";
            dataGridViewCurrencyColumn6.Name = "dataGridViewCurrencyColumn6";
            dataGridViewCurrencyColumn6.ReadOnly = true;
            dataGridViewCurrencyColumn6.Resizable = DataGridViewTriState.False;
            dataGridViewCurrencyColumn6.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewCurrencyColumn6.Width = 90;
            // 
            // Column8
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
            Column8.DefaultCellStyle = dataGridViewCellStyle5;
            Column8.HeaderText = "Value";
            Column8.Name = "Column8";
            Column8.ReadOnly = true;
            Column8.Resizable = DataGridViewTriState.False;
            Column8.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column8.Width = 90;
            // 
            // Column1
            // 
            Column1.HeaderText = "Place";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 140;
            // 
            // Column2
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
            Column2.DefaultCellStyle = dataGridViewCellStyle6;
            Column2.HeaderText = "Rev. Chg";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Resizable = DataGridViewTriState.True;
            Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column2.Width = 80;
            // 
            // Column3
            // 
            Column3.HeaderText = "Type";
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            Column3.Resizable = DataGridViewTriState.False;
            Column3.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column3.Width = 90;
            // 
            // Column4
            // 
            Column4.HeaderText = "Ecom GSTIN";
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            Column4.Resizable = DataGridViewTriState.False;
            Column4.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column5
            // 
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight;
            Column5.DefaultCellStyle = dataGridViewCellStyle7;
            Column5.HeaderText = "Rate";
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            Column5.Resizable = DataGridViewTriState.False;
            Column5.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column5.Width = 70;
            // 
            // Column6
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleRight;
            Column6.DefaultCellStyle = dataGridViewCellStyle8;
            Column6.HeaderText = "Taxable Value";
            Column6.Name = "Column6";
            Column6.ReadOnly = true;
            Column6.Resizable = DataGridViewTriState.False;
            Column6.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Column7
            // 
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleRight;
            Column7.DefaultCellStyle = dataGridViewCellStyle9;
            Column7.HeaderText = "Tax Amount";
            Column7.Name = "Column7";
            Column7.ReadOnly = true;
            Column7.Resizable = DataGridViewTriState.False;
            Column7.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // ab2ToolStrip1
            // 
            ab2ToolStrip1.BackColor = SystemColors.ControlLight;
            ab2ToolStrip1.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ab2ToolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            ab2ToolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel2, FromDate, toolStripLabel3, ToDate, BtnGo, toolStripSeparator1, ToolStripSave, ToolStripPrint });
            ab2ToolStrip1.Location = new Point(0, 0);
            ab2ToolStrip1.Name = "ab2ToolStrip1";
            ab2ToolStrip1.Padding = new Padding(5);
            ab2ToolStrip1.Size = new Size(1224, 38);
            ab2ToolStrip1.TabIndex = 3;
            ab2ToolStrip1.Text = "ab2ToolStrip1";
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(34, 25);
            toolStripLabel2.Text = "From";
            // 
            // FromDate
            // 
            FromDate.BackColor = Color.White;
            FromDate.Date = null;
            FromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FromDate.Format = "MM/dd/yyyy";
            FromDate.MaxDate = new DateTime(9997, 12, 31, 9, 39, 11, 0);
            FromDate.MinDate = new DateTime(1900, 1, 1, 18, 35, 12, 0);
            FromDate.Name = "FromDate";
            FromDate.Size = new Size(97, 25);
            FromDate.Text = "toolStripCalendar1";
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new Size(22, 25);
            toolStripLabel3.Text = "To";
            // 
            // ToDate
            // 
            ToDate.BackColor = Color.White;
            ToDate.Date = null;
            ToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ToDate.Format = "MM/dd/yyyy";
            ToDate.MaxDate = new DateTime(9997, 12, 31, 9, 39, 11, 0);
            ToDate.MinDate = new DateTime(1900, 1, 1, 18, 35, 13, 0);
            ToDate.Name = "ToDate";
            ToDate.Size = new Size(97, 25);
            ToDate.Text = "toolStripCalendar2";
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
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 28);
            // 
            // ToolStripSave
            // 
            ToolStripSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripSave.Image = (Image)resources.GetObject("ToolStripSave.Image");
            ToolStripSave.ImageTransparentColor = Color.Black;
            ToolStripSave.Name = "ToolStripSave";
            ToolStripSave.Size = new Size(23, 25);
            ToolStripSave.Text = "Save";
            ToolStripSave.Click += BtnSave_Click;
            // 
            // ToolStripPrint
            // 
            ToolStripPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripPrint.Image = (Image)resources.GetObject("ToolStripPrint.Image");
            ToolStripPrint.ImageTransparentColor = Color.Black;
            ToolStripPrint.Name = "ToolStripPrint";
            ToolStripPrint.Size = new Size(23, 25);
            ToolStripPrint.Text = "Print";
            ToolStripPrint.Click += BtnPrint_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(951, 505);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(82, 23);
            BtnPrint.TabIndex = 18;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // FormPurchaseGstrReport
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1224, 562);
            Controls.Add(BtnExit);
            Controls.Add(BtnPrint);
            Controls.Add(BtnCancel);
            Controls.Add(BtnSave);
            Controls.Add(statusStrip1);
            Controls.Add(ab2ToolStrip1);
            Controls.Add(GridViewBtoB);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormPurchaseGstrReport";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Purchase Gstr Report";
            Load += FormPurchaseGstrReport_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewBtoB).EndInit();
            ab2ToolStrip1.ResumeLayout(false);
            ab2ToolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private views.controls.Ab2ToolStrip ab2ToolStrip1;
        private ToolStripLabel toolStripLabel2;
        private views.controls.ToolStripCalendar FromDate;
        private ToolStripLabel toolStripLabel3;
        private views.controls.ToolStripCalendar ToDate;
        private ToolStripButton BtnGo;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton ToolStripSave;
        private views.controls.DataViewVerticalScroll GridViewBtoB;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel ErrorMsg;
        private Button BtnExit;
        private Button BtnCancel;
        private Button BtnSave;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn Column9;
        private DataGridViewTextBoxColumn dataGridViewCurrencyColumn3;
        private DataGridViewTextBoxColumn dataGridViewCurrencyColumn6;
        private DataGridViewTextBoxColumn Column8;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column6;
        private DataGridViewTextBoxColumn Column7;
        private Button BtnPrint;
        private ToolStripButton ToolStripPrint;
    }
}