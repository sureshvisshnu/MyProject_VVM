namespace Fa.reports.sales
{
    partial class FormDeliveryReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDeliveryReport));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            BerklyDeliveryReportToolStri = new fa.views.controls.Ab2ToolStrip();
            ToolStripLabelFrom = new ToolStripLabel();
            DeliveryReportFromDate = new fa.views.controls.ToolStripCalendar();
            ToolStripLabelTo = new ToolStripLabel();
            DeliveryReportToDate = new fa.views.controls.ToolStripCalendar();
            BtnGo = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            ToolStripBtnsave = new ToolStripButton();
            ToolStripBtnPrint = new ToolStripButton();
            BerklyDeliveryReportStatusStrip = new StatusStrip();
            DeliveryReportErrorMsg = new ToolStripStatusLabel();
            DeliveryReportGridView = new fa.views.controls.DataViewVerticalScroll();
            BtnExit = new Button();
            BtnCancel = new Button();
            BtnPrint = new Button();
            BtnSave = new Button();
            RowNumber = new DataGridViewTextBoxColumn();
            Date = new DataGridViewTextBoxColumn();
            CustomerName = new DataGridViewTextBoxColumn();
            Amount = new fa.views.controls.grid.DataGridViewCurrencyColumn();
            PaymentType = new DataGridViewTextBoxColumn();
            Status = new DataGridViewTextBoxColumn();
            BerklyDeliveryReportToolStri.SuspendLayout();
            BerklyDeliveryReportStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DeliveryReportGridView).BeginInit();
            SuspendLayout();
            // 
            // BerklyDeliveryReportToolStri
            // 
            BerklyDeliveryReportToolStri.BackColor = SystemColors.ControlLight;
            BerklyDeliveryReportToolStri.GripStyle = ToolStripGripStyle.Hidden;
            BerklyDeliveryReportToolStri.Items.AddRange(new ToolStripItem[] { ToolStripLabelFrom, DeliveryReportFromDate, ToolStripLabelTo, DeliveryReportToDate, BtnGo, toolStripSeparator1, ToolStripBtnsave, ToolStripBtnPrint });
            BerklyDeliveryReportToolStri.Location = new Point(0, 0);
            BerklyDeliveryReportToolStri.Name = "BerklyDeliveryReportToolStri";
            BerklyDeliveryReportToolStri.Padding = new Padding(5);
            BerklyDeliveryReportToolStri.Size = new Size(842, 38);
            BerklyDeliveryReportToolStri.TabIndex = 0;
            BerklyDeliveryReportToolStri.Text = "DeliveryReportToolStrip";
            // 
            // ToolStripLabelFrom
            // 
            ToolStripLabelFrom.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ToolStripLabelFrom.Name = "ToolStripLabelFrom";
            ToolStripLabelFrom.Size = new Size(31, 25);
            ToolStripLabelFrom.Text = "From";
            // 
            // DeliveryReportFromDate
            // 
            DeliveryReportFromDate.BackColor = Color.White;
            DeliveryReportFromDate.Date = null;
            DeliveryReportFromDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DeliveryReportFromDate.Format = "MM/dd/yyyy";
            DeliveryReportFromDate.MaxDate = new DateTime(9997, 12, 31, 7, 32, 58, 0);
            DeliveryReportFromDate.MinDate = new DateTime(1900, 1, 1, 23, 58, 26, 0);
            DeliveryReportFromDate.Name = "DeliveryReportFromDate";
            DeliveryReportFromDate.Size = new Size(97, 25);
            DeliveryReportFromDate.Text = "toolStripCalendar1";
            // 
            // ToolStripLabelTo
            // 
            ToolStripLabelTo.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ToolStripLabelTo.Name = "ToolStripLabelTo";
            ToolStripLabelTo.Size = new Size(19, 25);
            ToolStripLabelTo.Text = "To";
            // 
            // DeliveryReportToDate
            // 
            DeliveryReportToDate.BackColor = Color.White;
            DeliveryReportToDate.Date = null;
            DeliveryReportToDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DeliveryReportToDate.Format = "MM/dd/yyyy";
            DeliveryReportToDate.MaxDate = new DateTime(9997, 12, 31, 7, 32, 58, 0);
            DeliveryReportToDate.MinDate = new DateTime(1900, 1, 1, 23, 58, 26, 0);
            DeliveryReportToDate.Name = "DeliveryReportToDate";
            DeliveryReportToDate.Size = new Size(97, 25);
            DeliveryReportToDate.Text = "toolStripCalendar1";
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
            // ToolStripBtnsave
            // 
            ToolStripBtnsave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnsave.Image = (Image)resources.GetObject("ToolStripBtnsave.Image");
            ToolStripBtnsave.ImageTransparentColor = Color.Black;
            ToolStripBtnsave.Name = "ToolStripBtnsave";
            ToolStripBtnsave.Size = new Size(23, 25);
            ToolStripBtnsave.Text = "Save";
            ToolStripBtnsave.Click += ToolStripBtnsave_Click;
            // 
            // ToolStripBtnPrint
            // 
            ToolStripBtnPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            ToolStripBtnPrint.Image = (Image)resources.GetObject("ToolStripBtnPrint.Image");
            ToolStripBtnPrint.ImageTransparentColor = Color.Black;
            ToolStripBtnPrint.Name = "ToolStripBtnPrint";
            ToolStripBtnPrint.Size = new Size(23, 25);
            ToolStripBtnPrint.Text = "Print";
            ToolStripBtnPrint.Click += ToolStripBtnPrint_Click;
            // 
            // BerklyDeliveryReportStatusStrip
            // 
            BerklyDeliveryReportStatusStrip.Items.AddRange(new ToolStripItem[] { DeliveryReportErrorMsg });
            BerklyDeliveryReportStatusStrip.Location = new Point(0, 405);
            BerklyDeliveryReportStatusStrip.Name = "BerklyDeliveryReportStatusStrip";
            BerklyDeliveryReportStatusStrip.Size = new Size(842, 22);
            BerklyDeliveryReportStatusStrip.TabIndex = 1;
            BerklyDeliveryReportStatusStrip.Text = "DeliveryReportStatusStrip";
            // 
            // DeliveryReportErrorMsg
            // 
            DeliveryReportErrorMsg.Name = "DeliveryReportErrorMsg";
            DeliveryReportErrorMsg.Size = new Size(0, 17);
            // 
            // DeliveryReportGridView
            // 
            DeliveryReportGridView.AllowUserToAddRows = false;
            DeliveryReportGridView.AllowUserToDeleteRows = false;
            DeliveryReportGridView.AllowUserToResizeColumns = false;
            DeliveryReportGridView.AllowUserToResizeRows = false;
            DeliveryReportGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            DeliveryReportGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            DeliveryReportGridView.ColumnHeadersHeight = 20;
            DeliveryReportGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DeliveryReportGridView.Columns.AddRange(new DataGridViewColumn[] { RowNumber, Date, CustomerName, Amount, PaymentType, Status });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            DeliveryReportGridView.DefaultCellStyle = dataGridViewCellStyle3;
            DeliveryReportGridView.EnableHeadersVisualStyles = false;
            DeliveryReportGridView.Location = new Point(5, 40);
            DeliveryReportGridView.MultiSelect = false;
            DeliveryReportGridView.Name = "DeliveryReportGridView";
            DeliveryReportGridView.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            DeliveryReportGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            DeliveryReportGridView.RowHeadersVisible = false;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.ForeColor = Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = Color.White;
            dataGridViewCellStyle5.SelectionForeColor = Color.Black;
            DeliveryReportGridView.RowsDefaultCellStyle = dataGridViewCellStyle5;
            DeliveryReportGridView.RowTemplate.DefaultCellStyle.BackColor = SystemColors.Window;
            DeliveryReportGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DeliveryReportGridView.ShowCellToolTips = false;
            DeliveryReportGridView.Size = new Size(833, 320);
            DeliveryReportGridView.TabIndex = 5;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(748, 371);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(72, 23);
            BtnExit.TabIndex = 21;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(498, 371);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(82, 23);
            BtnCancel.TabIndex = 20;
            BtnCancel.Text = "Reset [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Enabled = false;
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(667, 371);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(75, 23);
            BtnPrint.TabIndex = 19;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // BtnSave
            // 
            BtnSave.Enabled = false;
            BtnSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(586, 371);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 18;
            BtnSave.Text = "Save [F8]";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
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
            // Date
            // 
            Date.Frozen = true;
            Date.HeaderText = "Date";
            Date.Name = "Date";
            Date.ReadOnly = true;
            Date.Resizable = DataGridViewTriState.False;
            Date.SortMode = DataGridViewColumnSortMode.NotSortable;
            Date.Width = 125;
            // 
            // CustomerName
            // 
            CustomerName.Frozen = true;
            CustomerName.HeaderText = "Customer Name";
            CustomerName.Name = "CustomerName";
            CustomerName.ReadOnly = true;
            CustomerName.Resizable = DataGridViewTriState.False;
            CustomerName.SortMode = DataGridViewColumnSortMode.NotSortable;
            CustomerName.Width = 250;
            // 
            // Amount
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            Amount.DefaultCellStyle = dataGridViewCellStyle2;
            Amount.HeaderText = "Amount";
            Amount.Name = "Amount";
            Amount.ReadOnly = true;
            Amount.Resizable = DataGridViewTriState.False;
            Amount.Width = 110;
            // 
            // PaymentType
            // 
            PaymentType.HeaderText = "Payment Type";
            PaymentType.Name = "PaymentType";
            PaymentType.ReadOnly = true;
            PaymentType.Resizable = DataGridViewTriState.False;
            PaymentType.SortMode = DataGridViewColumnSortMode.NotSortable;
            PaymentType.Width = 125;
            // 
            // Status
            // 
            Status.HeaderText = "Status";
            Status.Name = "Status";
            Status.ReadOnly = true;
            Status.Resizable = DataGridViewTriState.False;
            Status.SortMode = DataGridViewColumnSortMode.NotSortable;
            Status.Width = 150;
            // 
            // FormDeliveryReport
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(842, 427);
            Controls.Add(BtnExit);
            Controls.Add(BtnCancel);
            Controls.Add(BtnPrint);
            Controls.Add(BtnSave);
            Controls.Add(BerklyDeliveryReportStatusStrip);
            Controls.Add(BerklyDeliveryReportToolStri);
            Controls.Add(DeliveryReportGridView);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormDeliveryReport";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Delivery Report";
            Load += FormDeliveryReport_Load;
            BerklyDeliveryReportToolStri.ResumeLayout(false);
            BerklyDeliveryReportToolStri.PerformLayout();
            BerklyDeliveryReportStatusStrip.ResumeLayout(false);
            BerklyDeliveryReportStatusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DeliveryReportGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private fa.views.controls.Ab2ToolStrip BerklyDeliveryReportToolStri;
        private StatusStrip BerklyDeliveryReportStatusStrip;
        private ToolStripLabel ToolStripLabelFrom;
        private fa.views.controls.ToolStripCalendar DeliveryReportFromDate;
        private ToolStripLabel ToolStripLabelTo;
        private fa.views.controls.ToolStripCalendar DeliveryReportToDate;
        private fa.views.controls.DataViewVerticalScroll DeliveryReportGridView;
        private ToolStripStatusLabel DeliveryReportErrorMsg;
        private Button BtnExit;
        private Button BtnCancel;
        private Button BtnPrint;
        private Button BtnSave;
        private ToolStripButton BtnGo;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton ToolStripBtnsave;
        private ToolStripButton ToolStripBtnPrint;
        private DataGridViewTextBoxColumn RowNumber;
        private DataGridViewTextBoxColumn Date;
        private DataGridViewTextBoxColumn CustomerName;
        private fa.views.controls.grid.DataGridViewCurrencyColumn Amount;
        private DataGridViewTextBoxColumn PaymentType;
        private DataGridViewTextBoxColumn Status;
    }
}