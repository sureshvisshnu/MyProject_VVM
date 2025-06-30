namespace fa.views.controls.accounting
{
    partial class DiscountGrid
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.GridViewDiscount = new fa.views.controls.DataViewVerticalScroll();
            this.SeqNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiscountDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiscountType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Value = new fa.views.controls.grid.DataGridViewCurrencyColumn();
            this.DiscountAmount = new fa.views.controls.grid.DataGridViewCurrencyColumn();
            this.TotalAmount = new fa.views.controls.grid.DataGridViewCurrencyColumn();
            this.DeleteDiscount = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewDiscount)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Discounts";
            // 
            // GridViewDiscount
            // 
            this.GridViewDiscount.AllowUserToDeleteRows = false;
            this.GridViewDiscount.AllowUserToResizeColumns = false;
            this.GridViewDiscount.AllowUserToResizeRows = false;
            this.GridViewDiscount.BackgroundColor = System.Drawing.SystemColors.Control;
            this.GridViewDiscount.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewDiscount.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SeqNumber,
            this.DiscountDescription,
            this.DiscountType,
            this.Value,
            this.DiscountAmount,
            this.TotalAmount,
            this.DeleteDiscount});
            this.GridViewDiscount.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.GridViewDiscount.Location = new System.Drawing.Point(0, 20);
            this.GridViewDiscount.MultiSelect = false;
            this.GridViewDiscount.Name = "GridViewDiscount";
            this.GridViewDiscount.RowHeadersVisible = false;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.Black;
            this.GridViewDiscount.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.GridViewDiscount.Size = new System.Drawing.Size(446, 130);
            this.GridViewDiscount.TabIndex = 0;
            this.GridViewDiscount.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewDiscount_CellClick);
            this.GridViewDiscount.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewDiscount_CellEndEdit);
            this.GridViewDiscount.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewDiscount_CellEnter);
            this.GridViewDiscount.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.GridViewDiscount_DataError);
            this.GridViewDiscount.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.GridViewDiscount_EditingControlShowing);
            this.GridViewDiscount.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.GridViewDiscount_RowsAdded);
            this.GridViewDiscount.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.GridViewDiscount_RowsRemoved);
            // 
            // SeqNumber
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.SeqNumber.DefaultCellStyle = dataGridViewCellStyle7;
            this.SeqNumber.HeaderText = "#";
            this.SeqNumber.Name = "SeqNumber";
            this.SeqNumber.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SeqNumber.Width = 25;
            // 
            // DiscountDescription
            // 
            this.DiscountDescription.HeaderText = "Description";
            this.DiscountDescription.MaxInputLength = 250;
            this.DiscountDescription.Name = "DiscountDescription";
            // 
            // DiscountType
            // 
            this.DiscountType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.DiscountType.HeaderText = "Type";
            this.DiscountType.Name = "DiscountType";
            this.DiscountType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DiscountType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.DiscountType.Width = 75;
            // 
            // Value
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Value.DefaultCellStyle = dataGridViewCellStyle8;
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Value.Width = 50;
            // 
            // DiscountAmount
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.DiscountAmount.DefaultCellStyle = dataGridViewCellStyle9;
            this.DiscountAmount.HeaderText = "Amount";
            this.DiscountAmount.Name = "DiscountAmount";
            this.DiscountAmount.ReadOnly = true;
            this.DiscountAmount.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DiscountAmount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.DiscountAmount.Width = 75;
            // 
            // TotalAmount
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.TotalAmount.DefaultCellStyle = dataGridViewCellStyle10;
            this.TotalAmount.HeaderText = "Total";
            this.TotalAmount.Name = "TotalAmount";
            this.TotalAmount.ReadOnly = true;
            this.TotalAmount.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TotalAmount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.TotalAmount.Width = 75;
            // 
            // DeleteDiscount
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.NullValue = "X";
            this.DeleteDiscount.DefaultCellStyle = dataGridViewCellStyle11;
            this.DeleteDiscount.HeaderText = " ";
            this.DeleteDiscount.Name = "DeleteDiscount";
            this.DeleteDiscount.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DeleteDiscount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.DeleteDiscount.Width = 25;
            // 
            // DiscountGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GridViewDiscount);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DiscountGrid";
            this.Size = new System.Drawing.Size(446, 150);
            this.SizeChanged += new System.EventHandler(this.DiscountGrid_SizeChanged);
            this.Enter += new System.EventHandler(this.DiscountGrid_Enter);
            this.Resize += new System.EventHandler(this.DiscountGrid_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewDiscount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataViewVerticalScroll GridViewDiscount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn SeqNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiscountDescription;
        private System.Windows.Forms.DataGridViewComboBoxColumn DiscountType;
        private grid.DataGridViewCurrencyColumn Value;
        private grid.DataGridViewCurrencyColumn DiscountAmount;
        private grid.DataGridViewCurrencyColumn TotalAmount;
        private System.Windows.Forms.DataGridViewButtonColumn DeleteDiscount;
    }
}
