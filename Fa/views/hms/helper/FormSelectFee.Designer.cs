namespace Fa.views.hms.helper
{
    partial class FormSelectFee
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectFee));
            BtnCancel = new Button();
            BtnSelect = new Button();
            GridViewFee = new fa.views.controls.DataViewVerticalScroll();
            Date = new DataGridViewTextBoxColumn();
            Customer = new DataGridViewTextBoxColumn();
            Amount = new fa.views.controls.grid.DataGridViewCurrencyColumn();
            Column10 = new DataGridViewTextBoxColumn();
            statusStrip1 = new StatusStrip();
            RecentInvoiceErrorMsg = new ToolStripStatusLabel();
            toolStrip1 = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            TextBoxFeeSearch = new ToolStripTextBox();
            BtnFeeSearch = new ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)GridViewFee).BeginInit();
            statusStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // BtnCancel
            // 
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(426, 261);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(88, 23);
            BtnCancel.TabIndex = 5;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // BtnSelect
            // 
            BtnSelect.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSelect.Location = new Point(520, 261);
            BtnSelect.Name = "BtnSelect";
            BtnSelect.Size = new Size(88, 23);
            BtnSelect.TabIndex = 4;
            BtnSelect.Text = "Select [F8]";
            BtnSelect.UseVisualStyleBackColor = true;
            BtnSelect.Click += BtnSelect_Click;
            BtnSelect.PreviewKeyDown += BtnSelect_PreviewKeyDown;
            // 
            // GridViewFee
            // 
            GridViewFee.AllowUserToAddRows = false;
            GridViewFee.AllowUserToDeleteRows = false;
            GridViewFee.AllowUserToResizeColumns = false;
            GridViewFee.AllowUserToResizeRows = false;
            GridViewFee.BackgroundColor = SystemColors.Control;
            GridViewFee.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewFee.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewFee.ColumnHeadersHeight = 20;
            GridViewFee.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewFee.Columns.AddRange(new DataGridViewColumn[] { Date, Customer, Amount, Column10 });
            GridViewFee.EditMode = DataGridViewEditMode.EditOnEnter;
            GridViewFee.EnableHeadersVisualStyles = false;
            GridViewFee.Location = new Point(3, 33);
            GridViewFee.MultiSelect = false;
            GridViewFee.Name = "GridViewFee";
            GridViewFee.ReadOnly = true;
            GridViewFee.RowHeadersVisible = false;
            GridViewFee.RowTemplate.Height = 20;
            GridViewFee.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewFee.ShowCellToolTips = false;
            GridViewFee.Size = new Size(612, 217);
            GridViewFee.TabIndex = 3;
            GridViewFee.TabStop = false;
            GridViewFee.CellDoubleClick += GridViewRecentInvoice_CellDoubleClick;
            GridViewFee.KeyDown += GridViewRecentInvoice_KeyDown;
            // 
            // Date
            // 
            Date.HeaderText = "Name";
            Date.Name = "Date";
            Date.ReadOnly = true;
            Date.Resizable = DataGridViewTriState.False;
            Date.SortMode = DataGridViewColumnSortMode.NotSortable;
            Date.Width = 160;
            // 
            // Customer
            // 
            Customer.HeaderText = "Description";
            Customer.Name = "Customer";
            Customer.ReadOnly = true;
            Customer.Resizable = DataGridViewTriState.False;
            Customer.SortMode = DataGridViewColumnSortMode.NotSortable;
            Customer.Width = 330;
            // 
            // Amount
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            Amount.DefaultCellStyle = dataGridViewCellStyle2;
            Amount.HeaderText = "Fee";
            Amount.Name = "Amount";
            Amount.ReadOnly = true;
            Amount.Resizable = DataGridViewTriState.False;
            // 
            // Column10
            // 
            Column10.HeaderText = "Id";
            Column10.Name = "Column10";
            Column10.ReadOnly = true;
            Column10.Resizable = DataGridViewTriState.False;
            Column10.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column10.Visible = false;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { RecentInvoiceErrorMsg });
            statusStrip1.Location = new Point(0, 295);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(617, 22);
            statusStrip1.TabIndex = 154;
            statusStrip1.Text = "statusStrip1";
            // 
            // RecentInvoiceErrorMsg
            // 
            RecentInvoiceErrorMsg.Name = "RecentInvoiceErrorMsg";
            RecentInvoiceErrorMsg.Size = new Size(49, 17);
            RecentInvoiceErrorMsg.Text = "              ";
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = SystemColors.ControlLight;
            toolStrip1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, TextBoxFeeSearch, BtnFeeSearch });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new Padding(5);
            toolStrip1.Size = new Size(617, 32);
            toolStrip1.TabIndex = 155;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(61, 19);
            toolStripLabel1.Text = "Search Fee";
            // 
            // TextBoxFeeSearch
            // 
            TextBoxFeeSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxFeeSearch.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxFeeSearch.MaxLength = 30;
            TextBoxFeeSearch.Name = "TextBoxFeeSearch";
            TextBoxFeeSearch.Size = new Size(200, 22);
            // 
            // BtnFeeSearch
            // 
            BtnFeeSearch.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnFeeSearch.ImageTransparentColor = Color.Magenta;
            BtnFeeSearch.Name = "BtnFeeSearch";
            BtnFeeSearch.Size = new Size(24, 19);
            BtnFeeSearch.Text = "Go";
            BtnFeeSearch.Click += BtnFeeSearch_Click;
            // 
            // FormSelectFee
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(617, 317);
            Controls.Add(toolStrip1);
            Controls.Add(statusStrip1);
            Controls.Add(BtnCancel);
            Controls.Add(BtnSelect);
            Controls.Add(GridViewFee);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSelectFee";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select Fee";
            Load += FormSelectGeneratedInvoice_Load;
            Controls.SetChildIndex(GridViewFee, 0);
            Controls.SetChildIndex(BtnSelect, 0);
            Controls.SetChildIndex(BtnCancel, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(toolStrip1, 0);
            ((System.ComponentModel.ISupportInitialize)GridViewFee).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnCancel;
        private Button BtnSelect;
        private fa.views.controls.DataViewVerticalScroll GridViewFee;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel RecentInvoiceErrorMsg;
        private DataGridViewTextBoxColumn Date;
        private DataGridViewTextBoxColumn Customer;
        private fa.views.controls.grid.DataGridViewCurrencyColumn Amount;
        private DataGridViewTextBoxColumn Column10;
        private ToolStrip toolStrip1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripTextBox TextBoxFeeSearch;
        private ToolStripButton BtnFeeSearch;
    }
}