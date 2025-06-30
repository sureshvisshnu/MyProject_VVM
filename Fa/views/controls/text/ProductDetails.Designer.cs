namespace fa.views.controls.text
{
    partial class ProductDetails
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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            GroupBoxProductDetail = new GroupBox();
            GridViewQOH = new DataViewVerticalScroll();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            WholeSale = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            TextBoxSalesProductUOM = new TextBox();
            label6 = new Label();
            TextBoxSalesProductName = new TextBox();
            label30 = new Label();
            TextBoxSalesMaterialId = new TextBox();
            label10 = new Label();
            label5 = new Label();
            ProductItemTaxDetails = new accounting.ItemTaxDetails();
            GroupBoxProductDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewQOH).BeginInit();
            SuspendLayout();
            // 
            // GroupBoxProductDetail
            // 
            GroupBoxProductDetail.Controls.Add(GridViewQOH);
            GroupBoxProductDetail.Controls.Add(TextBoxSalesProductUOM);
            GroupBoxProductDetail.Controls.Add(label6);
            GroupBoxProductDetail.Controls.Add(TextBoxSalesProductName);
            GroupBoxProductDetail.Controls.Add(label30);
            GroupBoxProductDetail.Controls.Add(TextBoxSalesMaterialId);
            GroupBoxProductDetail.Controls.Add(label10);
            GroupBoxProductDetail.Controls.Add(label5);
            GroupBoxProductDetail.Controls.Add(ProductItemTaxDetails);
            GroupBoxProductDetail.Location = new Point(1, 1);
            GroupBoxProductDetail.Margin = new Padding(4, 3, 4, 3);
            GroupBoxProductDetail.Name = "GroupBoxProductDetail";
            GroupBoxProductDetail.Padding = new Padding(4, 3, 4, 3);
            GroupBoxProductDetail.Size = new Size(275, 624);
            GroupBoxProductDetail.TabIndex = 0;
            GroupBoxProductDetail.TabStop = false;
            GroupBoxProductDetail.Text = "Product Details";
            GroupBoxProductDetail.SizeChanged += GroupBoxProductDetail_SizeChanged;
            GroupBoxProductDetail.Enter += GroupBoxProductDetail_Enter;
            // 
            // GridViewQOH
            // 
            GridViewQOH.AllowUserToAddRows = false;
            GridViewQOH.AllowUserToDeleteRows = false;
            GridViewQOH.AllowUserToResizeColumns = false;
            GridViewQOH.AllowUserToResizeRows = false;
            GridViewQOH.BackgroundColor = SystemColors.Control;
            GridViewQOH.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GridViewQOH.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, WholeSale, Column3 });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = Color.White;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            GridViewQOH.DefaultCellStyle = dataGridViewCellStyle3;
            GridViewQOH.Location = new Point(10, 400);
            GridViewQOH.Margin = new Padding(4, 3, 4, 3);
            GridViewQOH.Name = "GridViewQOH";
            GridViewQOH.ReadOnly = true;
            GridViewQOH.RowHeadersVisible = false;
            GridViewQOH.ShowCellToolTips = false;
            GridViewQOH.Size = new Size(250, 186);
            GridViewQOH.TabIndex = 90;
            GridViewQOH.SizeChanged += GroupBoxProductDetail_SizeChanged;
            // 
            // Column1
            // 
            Column1.HeaderText = "Location";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 80;
            // 
            // Column2
            // 
            Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.TopRight;
            Column2.DefaultCellStyle = dataGridViewCellStyle1;
            Column2.HeaderText = "Retail";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Resizable = DataGridViewTriState.False;
            Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // WholeSale
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopRight;
            WholeSale.DefaultCellStyle = dataGridViewCellStyle2;
            WholeSale.HeaderText = "WholeSale";
            WholeSale.Name = "WholeSale";
            WholeSale.ReadOnly = true;
            WholeSale.Resizable = DataGridViewTriState.False;
            WholeSale.SortMode = DataGridViewColumnSortMode.NotSortable;
            WholeSale.Width = 70;
            // 
            // Column3
            // 
            Column3.HeaderText = "ID";
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            Column3.Visible = false;
            // 
            // TextBoxSalesProductUOM
            // 
            TextBoxSalesProductUOM.BackColor = SystemColors.Window;
            TextBoxSalesProductUOM.Location = new Point(10, 134);
            TextBoxSalesProductUOM.Margin = new Padding(4, 3, 4, 3);
            TextBoxSalesProductUOM.MaxLength = 30;
            TextBoxSalesProductUOM.Name = "TextBoxSalesProductUOM";
            TextBoxSalesProductUOM.Size = new Size(124, 23);
            TextBoxSalesProductUOM.TabIndex = 83;
            TextBoxSalesProductUOM.TabStop = false;
            TextBoxSalesProductUOM.Click += TextBoxSalesProductUOM_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(7, 381);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(100, 15);
            label6.TabIndex = 85;
            label6.Text = "Quanity On Hand";
            // 
            // TextBoxSalesProductName
            // 
            TextBoxSalesProductName.BackColor = SystemColors.Window;
            TextBoxSalesProductName.Location = new Point(10, 87);
            TextBoxSalesProductName.Margin = new Padding(4, 3, 4, 3);
            TextBoxSalesProductName.MaxLength = 35;
            TextBoxSalesProductName.Name = "TextBoxSalesProductName";
            TextBoxSalesProductName.Size = new Size(253, 23);
            TextBoxSalesProductName.TabIndex = 81;
            TextBoxSalesProductName.Click += TextBoxSalesProductName_Click;
            TextBoxSalesProductName.SizeChanged += GroupBoxProductDetail_SizeChanged;
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label30.Location = new Point(7, 114);
            label30.Margin = new Padding(4, 0, 4, 0);
            label30.Name = "label30";
            label30.Size = new Size(88, 13);
            label30.TabIndex = 84;
            label30.Text = "Purchase UOM";
            // 
            // TextBoxSalesMaterialId
            // 
            TextBoxSalesMaterialId.BackColor = SystemColors.Window;
            TextBoxSalesMaterialId.Location = new Point(10, 42);
            TextBoxSalesMaterialId.Margin = new Padding(4, 3, 4, 3);
            TextBoxSalesMaterialId.Name = "TextBoxSalesMaterialId";
            TextBoxSalesMaterialId.ReadOnly = true;
            TextBoxSalesMaterialId.Size = new Size(251, 23);
            TextBoxSalesMaterialId.TabIndex = 82;
            TextBoxSalesMaterialId.TabStop = false;
            TextBoxSalesMaterialId.Click += TextBoxSalesMaterialId_Click;
            TextBoxSalesMaterialId.SizeChanged += GroupBoxProductDetail_SizeChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(7, 68);
            label10.Margin = new Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new Size(39, 15);
            label10.TabIndex = 80;
            label10.Text = "Name";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(7, 23);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(17, 15);
            label5.TabIndex = 79;
            label5.Text = "Id";
            // 
            // ProductItemTaxDetails
            // 
            ProductItemTaxDetails.CurrentDate = null;
            ProductItemTaxDetails.EnableEdit = false;
            ProductItemTaxDetails.Location = new Point(7, 160);
            ProductItemTaxDetails.Margin = new Padding(5, 3, 5, 3);
            ProductItemTaxDetails.Name = "ProductItemTaxDetails";
            ProductItemTaxDetails.ProductId = 0L;
            ProductItemTaxDetails.Size = new Size(256, 204);
            ProductItemTaxDetails.TabIndex = 1;
            ProductItemTaxDetails.SizeChanged += GroupBoxProductDetail_SizeChanged;
            // 
            // ProductDetails
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(GroupBoxProductDetail);
            Margin = new Padding(4, 3, 4, 3);
            Name = "ProductDetails";
            Size = new Size(285, 625);
            ClientSizeChanged += ProductDetails_ClientSizeChanged;
            GroupBoxProductDetail.ResumeLayout(false);
            GroupBoxProductDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewQOH).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox GroupBoxProductDetail;
        private accounting.ItemTaxDetails ProductItemTaxDetails;
        private TextBox TextBoxSalesProductUOM;
        private Label label6;
        private TextBox TextBoxSalesProductName;
        private Label label30;
        private TextBox TextBoxSalesMaterialId;
        private Label label10;
        private Label label5;
        private DataViewVerticalScroll GridViewQOH;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn WholeSale;
        private DataGridViewTextBoxColumn Column3;
    }
}
