namespace fa.views.sales
{
    partial class FormSaleDelivery
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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSaleDelivery));
            TabControlInvoiceDetails = new TabControl();
            TabDetails = new TabPage();
            TextBoxRefNo = new TextBox();
            DateTimePickerInvoiceDate = new controls.text.DateWithCalendar();
            label18 = new Label();
            TextBoxAddress = new TextBox();
            TextBoxName = new TextBox();
            label17 = new Label();
            label16 = new Label();
            label5 = new Label();
            GridViewPendingInvoice = new controls.DataViewVerticalScroll();
            InvoiceDate = new DataGridViewTextBoxColumn();
            InvoiceNumber = new DataGridViewTextBoxColumn();
            InvAmount = new controls.grid.DataGridViewCurrencyColumn();
            Column1 = new DataGridViewTextBoxColumn();
            TextBoxCashAmount = new controls.text.CurrencyTextBox();
            label3 = new Label();
            BtnDeliver = new Button();
            BtnCancel = new Button();
            label1 = new Label();
            TextBoxSaleId = new TextBox();
            statusStrip1 = new StatusStrip();
            TabControlInvoiceDetails.SuspendLayout();
            TabDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewPendingInvoice).BeginInit();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // TabControlInvoiceDetails
            // 
            TabControlInvoiceDetails.Controls.Add(TabDetails);
            TabControlInvoiceDetails.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TabControlInvoiceDetails.Location = new Point(339, 12);
            TabControlInvoiceDetails.Name = "TabControlInvoiceDetails";
            TabControlInvoiceDetails.SelectedIndex = 0;
            TabControlInvoiceDetails.Size = new Size(321, 309);
            TabControlInvoiceDetails.TabIndex = 3;
            TabControlInvoiceDetails.TabStop = false;
            // 
            // TabDetails
            // 
            TabDetails.Controls.Add(TextBoxRefNo);
            TabDetails.Controls.Add(DateTimePickerInvoiceDate);
            TabDetails.Controls.Add(label18);
            TabDetails.Controls.Add(TextBoxAddress);
            TabDetails.Controls.Add(TextBoxName);
            TabDetails.Controls.Add(label17);
            TabDetails.Controls.Add(label16);
            TabDetails.Controls.Add(label5);
            TabDetails.Location = new Point(4, 22);
            TabDetails.Name = "TabDetails";
            TabDetails.Padding = new Padding(3);
            TabDetails.Size = new Size(313, 283);
            TabDetails.TabIndex = 0;
            TabDetails.Text = "Invoice Details";
            TabDetails.UseVisualStyleBackColor = true;
            // 
            // TextBoxRefNo
            // 
            TextBoxRefNo.BackColor = Color.White;
            TextBoxRefNo.Location = new Point(7, 28);
            TextBoxRefNo.Name = "TextBoxRefNo";
            TextBoxRefNo.ReadOnly = true;
            TextBoxRefNo.Size = new Size(132, 21);
            TextBoxRefNo.TabIndex = 11;
            TextBoxRefNo.TabStop = false;
            // 
            // DateTimePickerInvoiceDate
            // 
            DateTimePickerInvoiceDate.BackColor = Color.White;
            DateTimePickerInvoiceDate.BorderStyle = BorderStyle.FixedSingle;
            DateTimePickerInvoiceDate.Date = null;
            DateTimePickerInvoiceDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DateTimePickerInvoiceDate.Format = "MM/dd/yyyy";
            DateTimePickerInvoiceDate.Location = new Point(7, 69);
            DateTimePickerInvoiceDate.MaxDate = new DateTime(9997, 12, 31, 9, 13, 23, 0);
            DateTimePickerInvoiceDate.MinDate = new DateTime(1900, 1, 1, 22, 34, 39, 0);
            DateTimePickerInvoiceDate.Name = "DateTimePickerInvoiceDate";
            DateTimePickerInvoiceDate.ReadOnly = true;
            DateTimePickerInvoiceDate.Size = new Size(93, 21);
            DateTimePickerInvoiceDate.TabIndex = 10;
            DateTimePickerInvoiceDate.TabStop = false;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(4, 11);
            label18.Name = "label18";
            label18.Size = new Size(53, 13);
            label18.TabIndex = 6;
            label18.Text = "Invoice #";
            // 
            // TextBoxAddress
            // 
            TextBoxAddress.BackColor = Color.White;
            TextBoxAddress.Location = new Point(7, 155);
            TextBoxAddress.Multiline = true;
            TextBoxAddress.Name = "TextBoxAddress";
            TextBoxAddress.ReadOnly = true;
            TextBoxAddress.Size = new Size(241, 88);
            TextBoxAddress.TabIndex = 4;
            TextBoxAddress.TabStop = false;
            // 
            // TextBoxName
            // 
            TextBoxName.BackColor = Color.White;
            TextBoxName.Location = new Point(7, 113);
            TextBoxName.Name = "TextBoxName";
            TextBoxName.ReadOnly = true;
            TextBoxName.Size = new Size(241, 21);
            TextBoxName.TabIndex = 7;
            TextBoxName.TabStop = false;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(4, 53);
            label17.Name = "label17";
            label17.Size = new Size(30, 13);
            label17.TabIndex = 2;
            label17.Text = "Date";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(4, 138);
            label16.Name = "label16";
            label16.Size = new Size(46, 13);
            label16.TabIndex = 1;
            label16.Text = "Address";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(4, 96);
            label5.Name = "label5";
            label5.Size = new Size(34, 13);
            label5.TabIndex = 0;
            label5.Text = "Name";
            // 
            // GridViewPendingInvoice
            // 
            GridViewPendingInvoice.AllowUserToAddRows = false;
            GridViewPendingInvoice.AllowUserToDeleteRows = false;
            GridViewPendingInvoice.AllowUserToResizeColumns = false;
            GridViewPendingInvoice.AllowUserToResizeRows = false;
            GridViewPendingInvoice.BackgroundColor = SystemColors.Window;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewPendingInvoice.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewPendingInvoice.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GridViewPendingInvoice.Columns.AddRange(new DataGridViewColumn[] { InvoiceDate, InvoiceNumber, InvAmount, Column1 });
            GridViewPendingInvoice.EnableHeadersVisualStyles = false;
            GridViewPendingInvoice.Location = new Point(12, 34);
            GridViewPendingInvoice.Name = "GridViewPendingInvoice";
            GridViewPendingInvoice.ReadOnly = true;
            GridViewPendingInvoice.RowHeadersVisible = false;
            GridViewPendingInvoice.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewPendingInvoice.ShowCellToolTips = false;
            GridViewPendingInvoice.Size = new Size(321, 369);
            GridViewPendingInvoice.TabIndex = 4;
            GridViewPendingInvoice.TabStop = false;
            GridViewPendingInvoice.RowEnter += GridViewPendingInvoice_RowEnter;
            // 
            // InvoiceDate
            // 
            InvoiceDate.HeaderText = "Date";
            InvoiceDate.Name = "InvoiceDate";
            InvoiceDate.ReadOnly = true;
            InvoiceDate.Resizable = DataGridViewTriState.False;
            InvoiceDate.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // InvoiceNumber
            // 
            InvoiceNumber.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            InvoiceNumber.HeaderText = "Invoice #";
            InvoiceNumber.Name = "InvoiceNumber";
            InvoiceNumber.ReadOnly = true;
            InvoiceNumber.Resizable = DataGridViewTriState.False;
            InvoiceNumber.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // InvAmount
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            InvAmount.DefaultCellStyle = dataGridViewCellStyle2;
            InvAmount.HeaderText = "Amount";
            InvAmount.Name = "InvAmount";
            InvAmount.ReadOnly = true;
            InvAmount.Resizable = DataGridViewTriState.False;
            // 
            // Column1
            // 
            Column1.HeaderText = "Id";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Visible = false;
            // 
            // TextBoxCashAmount
            // 
            TextBoxCashAmount.BackColor = SystemColors.Window;
            TextBoxCashAmount.Decimals = 2;
            TextBoxCashAmount.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxCashAmount.Length = 10;
            TextBoxCashAmount.Location = new Point(527, 327);
            TextBoxCashAmount.Name = "TextBoxCashAmount";
            TextBoxCashAmount.ReadOnly = true;
            TextBoxCashAmount.Size = new Size(129, 30);
            TextBoxCashAmount.TabIndex = 6;
            TextBoxCashAmount.TabStop = false;
            TextBoxCashAmount.Text = "0.00";
            TextBoxCashAmount.TextAlign = HorizontalAlignment.Right;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(346, 330);
            label3.Name = "label3";
            label3.Size = new Size(151, 23);
            label3.TabIndex = 5;
            label3.Text = "Amount received";
            // 
            // BtnDeliver
            // 
            BtnDeliver.Location = new Point(572, 380);
            BtnDeliver.Name = "BtnDeliver";
            BtnDeliver.Size = new Size(79, 23);
            BtnDeliver.TabIndex = 16;
            BtnDeliver.Text = "Deliver [F9]";
            BtnDeliver.UseVisualStyleBackColor = true;
            BtnDeliver.Click += BtnDeliver_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Location = new Point(491, 380);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 23);
            BtnCancel.TabIndex = 17;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(12, 16);
            label1.Name = "label1";
            label1.Size = new Size(149, 13);
            label1.TabIndex = 18;
            label1.Text = "Pending Invoices For Delivery";
            // 
            // TextBoxSaleId
            // 
            TextBoxSaleId.Location = new Point(382, 382);
            TextBoxSaleId.Name = "TextBoxSaleId";
            TextBoxSaleId.Size = new Size(100, 21);
            TextBoxSaleId.TabIndex = 55;
            TextBoxSaleId.TabStop = false;
            TextBoxSaleId.Visible = false;
            // 
            // statusStrip1
            // 
            statusStrip1.Location = new Point(0, 413);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(663, 22);
            statusStrip1.TabIndex = 56;
            statusStrip1.Text = "statusStrip1";
            // 
            // FormSaleDelivery
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(663, 435);
            Controls.Add(GridViewPendingInvoice);
            Controls.Add(statusStrip1);
            Controls.Add(TextBoxSaleId);
            Controls.Add(BtnDeliver);
            Controls.Add(BtnCancel);
            Controls.Add(TabControlInvoiceDetails);
            Controls.Add(TextBoxCashAmount);
            Controls.Add(label3);
            Controls.Add(label1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSaleDelivery";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Delivery";
            Load += FormSaleDelivery_Load;
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(label3, 0);
            Controls.SetChildIndex(TextBoxCashAmount, 0);
            Controls.SetChildIndex(TabControlInvoiceDetails, 0);
            Controls.SetChildIndex(BtnCancel, 0);
            Controls.SetChildIndex(BtnDeliver, 0);
            Controls.SetChildIndex(TextBoxSaleId, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(GridViewPendingInvoice, 0);
            TabControlInvoiceDetails.ResumeLayout(false);
            TabDetails.ResumeLayout(false);
            TabDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewPendingInvoice).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TabControl TabControlInvoiceDetails;
        private System.Windows.Forms.TabPage TabDetails;
        private System.Windows.Forms.TextBox TextBoxRefNo;
        private controls.text.DateWithCalendar DateTimePickerInvoiceDate;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox TextBoxAddress;
        private System.Windows.Forms.TextBox TextBoxName;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label5;
        private controls.DataViewVerticalScroll GridViewPendingInvoice;
        private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceNumber;
        private controls.grid.DataGridViewCurrencyColumn InvAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private controls.text.CurrencyTextBox TextBoxCashAmount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BtnDeliver;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxSaleId;
        private System.Windows.Forms.StatusStrip statusStrip1;
    }
}