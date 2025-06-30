namespace fa.views.controls.accounting
{
    partial class AdditionalChargeGrid
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
            this.GridViewAdditionalCharge = new fa.views.controls.DataViewVerticalScroll();
            this.SeqNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiscountDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiscountType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Value = new fa.views.controls.grid.DataGridViewCurrencyColumn();
            this.DiscountAmount = new fa.views.controls.grid.DataGridViewCurrencyColumn();
            this.TotalAmount = new fa.views.controls.grid.DataGridViewCurrencyColumn();
            this.DeleteAdditionalCharges = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewAdditionalCharge)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Additional Charges";
            // 
            // GridViewAdditionalCharge
            // 
            this.GridViewAdditionalCharge.AllowUserToDeleteRows = false;
            this.GridViewAdditionalCharge.AllowUserToResizeColumns = false;
            this.GridViewAdditionalCharge.AllowUserToResizeRows = false;
            this.GridViewAdditionalCharge.BackgroundColor = System.Drawing.SystemColors.Control;
            this.GridViewAdditionalCharge.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewAdditionalCharge.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SeqNumber,
            this.DiscountDescription,
            this.DiscountType,
            this.Value,
            this.DiscountAmount,
            this.TotalAmount,
            this.DeleteAdditionalCharges});
            this.GridViewAdditionalCharge.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.GridViewAdditionalCharge.Location = new System.Drawing.Point(3, 20);
            this.GridViewAdditionalCharge.MultiSelect = false;
            this.GridViewAdditionalCharge.Name = "GridViewAdditionalCharge";
            this.GridViewAdditionalCharge.RowHeadersVisible = false;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.Black;
            this.GridViewAdditionalCharge.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.GridViewAdditionalCharge.Size = new System.Drawing.Size(447, 130);
            this.GridViewAdditionalCharge.TabIndex = 2;
            this.GridViewAdditionalCharge.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewAdditionalCharge_CellClick);
            this.GridViewAdditionalCharge.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewAdditionalCharge_CellEndEdit);
            this.GridViewAdditionalCharge.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewAdditionalCharge_CellEnter);
            this.GridViewAdditionalCharge.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.GridViewAdditionalCharge_DataError);
            this.GridViewAdditionalCharge.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.GridViewAdditionalCharge_EditingControlShowing);
            this.GridViewAdditionalCharge.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.GridViewAdditionalCharge_RowsAdded);
            this.GridViewAdditionalCharge.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.GridViewAdditionalCharge_RowsRemoved);
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
            // DeleteAdditionalCharges
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle11.NullValue = "X";
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.Black;
            this.DeleteAdditionalCharges.DefaultCellStyle = dataGridViewCellStyle11;
            this.DeleteAdditionalCharges.HeaderText = " ";
            this.DeleteAdditionalCharges.Name = "DeleteAdditionalCharges";
            this.DeleteAdditionalCharges.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DeleteAdditionalCharges.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.DeleteAdditionalCharges.Width = 25;
            // 
            // AdditionalChargeGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GridViewAdditionalCharge);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "AdditionalChargeGrid";
            this.Size = new System.Drawing.Size(450, 150);
            this.SizeChanged += new System.EventHandler(this.AdditionalChargeGrid_SizeChanged);
            this.Enter += new System.EventHandler(this.AdditionalChargeGrid_Enter);
            this.Resize += new System.EventHandler(this.AdditionalChargeGrid_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewAdditionalCharge)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DataViewVerticalScroll GridViewAdditionalCharge;
        private System.Windows.Forms.DataGridViewTextBoxColumn SeqNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiscountDescription;
        private System.Windows.Forms.DataGridViewComboBoxColumn DiscountType;
        private grid.DataGridViewCurrencyColumn Value;
        private grid.DataGridViewCurrencyColumn DiscountAmount;
        private grid.DataGridViewCurrencyColumn TotalAmount;
        private System.Windows.Forms.DataGridViewButtonColumn DeleteAdditionalCharges;
    }
}
