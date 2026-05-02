namespace fa.views.purchase
{
    partial class FormSearchItems
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
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSearchItems));
            toolStrip = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            TextBoxSearchProduct = new fa.views.controls.ToolstripDelayedTextBox();
            BtnSearchSelect = new Button();
            statusStrip1 = new StatusStrip();
            PatientSearchErrorMsg = new ToolStripStatusLabel();
            BtnSearchCancel = new Button();
            BtnNewProduct = new Button();
            GridViewItems = new fa.views.controls.DataViewVerticalScroll();
            groupBox1 = new GroupBox();
            TextBoxSupplier = new TextBox();
            TextBoxProductFamily = new TextBox();
            TextBoxManufacturer = new TextBox();
            TextBoxCategory = new TextBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            BtnSearchItemReload = new Button();
            PatientName = new DataGridViewTextBoxColumn();
            PatientAddress = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            sprice = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            toolStrip.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewItems).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Size = new Size(116, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Size = new Size(116, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Size = new Size(116, 21);
            // 
            // checkBoxIsPatient
            // 
            checkBoxIsPatient.Size = new Size(69, 17);
            // 
            // toolStrip
            // 
            toolStrip.BackColor = SystemColors.ControlLight;
            toolStrip.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip.Items.AddRange(new ToolStripItem[] { toolStripLabel1, TextBoxSearchProduct });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Padding = new Padding(5);
            toolStrip.Size = new Size(873, 34);
            toolStrip.TabIndex = 27;
            toolStrip.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(59, 21);
            toolStripLabel1.Text = "Search For";
            // 
            // TextBoxSearchProduct
            // 
            TextBoxSearchProduct.AutoSize = false;
            TextBoxSearchProduct.Delay = true;
            TextBoxSearchProduct.DelayTime = 1000;
            TextBoxSearchProduct.Name = "TextBoxSearchProduct";
            TextBoxSearchProduct.Size = new Size(300, 21);
            TextBoxSearchProduct.KeyDown += TextBoxSearchProduct_KeyDown;
            TextBoxSearchProduct.TextChanged += TextBoxSearchProduct_TextChanged;
            // 
            // BtnSearchSelect
            // 
            BtnSearchSelect.Enabled = false;
            BtnSearchSelect.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSearchSelect.Location = new Point(531, 276);
            BtnSearchSelect.Name = "BtnSearchSelect";
            BtnSearchSelect.Size = new Size(88, 23);
            BtnSearchSelect.TabIndex = 29;
            BtnSearchSelect.Text = "Select [F8]";
            BtnSearchSelect.UseVisualStyleBackColor = true;
            BtnSearchSelect.Click += BtnSearchSelect_Click;
            BtnSearchSelect.PreviewKeyDown += BtnSearchSelect_PreviewKeyDown;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { PatientSearchErrorMsg });
            statusStrip1.Location = new Point(0, 310);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(873, 22);
            statusStrip1.TabIndex = 31;
            statusStrip1.Text = "statusStrip1";
            // 
            // PatientSearchErrorMsg
            // 
            PatientSearchErrorMsg.Name = "PatientSearchErrorMsg";
            PatientSearchErrorMsg.Size = new Size(46, 17);
            PatientSearchErrorMsg.Text = "             ";
            // 
            // BtnSearchCancel
            // 
            BtnSearchCancel.DialogResult = DialogResult.Cancel;
            BtnSearchCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSearchCancel.Location = new Point(437, 276);
            BtnSearchCancel.Name = "BtnSearchCancel";
            BtnSearchCancel.Size = new Size(88, 23);
            BtnSearchCancel.TabIndex = 30;
            BtnSearchCancel.Text = "Cancel [Esc]";
            BtnSearchCancel.UseVisualStyleBackColor = true;
            BtnSearchCancel.PreviewKeyDown += BtnSearchSelect_PreviewKeyDown;
            // 
            // BtnNewProduct
            // 
            BtnNewProduct.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnNewProduct.Location = new Point(12, 276);
            BtnNewProduct.Name = "BtnNewProduct";
            BtnNewProduct.Size = new Size(123, 23);
            BtnNewProduct.TabIndex = 32;
            BtnNewProduct.Text = "New Product [F3]";
            BtnNewProduct.UseVisualStyleBackColor = true;
            BtnNewProduct.Click += BtnNewProduct_Click;
            // 
            // GridViewItems
            // 
            GridViewItems.AllowUserToAddRows = false;
            GridViewItems.AllowUserToDeleteRows = false;
            GridViewItems.AllowUserToResizeColumns = false;
            GridViewItems.AllowUserToResizeRows = false;
            GridViewItems.BackgroundColor = SystemColors.Control;
            GridViewItems.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewItems.ColumnHeadersHeight = 20;
            GridViewItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewItems.Columns.AddRange(new DataGridViewColumn[] { PatientName, PatientAddress, Column2, sprice, Column1 });
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Window;
            dataGridViewCellStyle6.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle6.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            GridViewItems.DefaultCellStyle = dataGridViewCellStyle6;
            GridViewItems.EditMode = DataGridViewEditMode.EditProgrammatically;
            GridViewItems.EnableHeadersVisualStyles = false;
            GridViewItems.Location = new Point(3, 37);
            GridViewItems.MultiSelect = false;
            GridViewItems.Name = "GridViewItems";
            GridViewItems.ReadOnly = true;
            GridViewItems.RowHeadersVisible = false;
            GridViewItems.RowTemplate.Height = 20;
            GridViewItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewItems.ShowCellToolTips = false;
            GridViewItems.ShowEditingIcon = false;
            GridViewItems.Size = new Size(628, 230);
            GridViewItems.TabIndex = 28;
            GridViewItems.CellDoubleClick += GridViewItems_CellDoubleClick;
            GridViewItems.CellEnter += GridViewItems_CellEnter;
            GridViewItems.RowEnter += GridViewItems_RowEnter;
            GridViewItems.Enter += GridViewItems_Enter;
            GridViewItems.KeyDown += GridViewItems_KeyDown;
            GridViewItems.Leave += GridViewItems_Leave;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = SystemColors.Control;
            groupBox1.Controls.Add(TextBoxSupplier);
            groupBox1.Controls.Add(TextBoxProductFamily);
            groupBox1.Controls.Add(TextBoxManufacturer);
            groupBox1.Controls.Add(TextBoxCategory);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            groupBox1.Location = new Point(635, 31);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(235, 276);
            groupBox1.TabIndex = 33;
            groupBox1.TabStop = false;
            groupBox1.Text = "Item Details";
            // 
            // TextBoxSupplier
            // 
            TextBoxSupplier.BackColor = Color.White;
            TextBoxSupplier.Location = new Point(9, 161);
            TextBoxSupplier.Name = "TextBoxSupplier";
            TextBoxSupplier.ReadOnly = true;
            TextBoxSupplier.Size = new Size(182, 21);
            TextBoxSupplier.TabIndex = 7;
            TextBoxSupplier.TabStop = false;
            // 
            // TextBoxProductFamily
            // 
            TextBoxProductFamily.BackColor = Color.White;
            TextBoxProductFamily.Location = new Point(9, 79);
            TextBoxProductFamily.Name = "TextBoxProductFamily";
            TextBoxProductFamily.ReadOnly = true;
            TextBoxProductFamily.Size = new Size(210, 21);
            TextBoxProductFamily.TabIndex = 6;
            TextBoxProductFamily.TabStop = false;
            // 
            // TextBoxManufacturer
            // 
            TextBoxManufacturer.BackColor = Color.White;
            TextBoxManufacturer.Location = new Point(9, 120);
            TextBoxManufacturer.Name = "TextBoxManufacturer";
            TextBoxManufacturer.ReadOnly = true;
            TextBoxManufacturer.Size = new Size(182, 21);
            TextBoxManufacturer.TabIndex = 5;
            TextBoxManufacturer.TabStop = false;
            // 
            // TextBoxCategory
            // 
            TextBoxCategory.BackColor = Color.White;
            TextBoxCategory.Location = new Point(9, 38);
            TextBoxCategory.Name = "TextBoxCategory";
            TextBoxCategory.ReadOnly = true;
            TextBoxCategory.Size = new Size(210, 21);
            TextBoxCategory.TabIndex = 4;
            TextBoxCategory.TabStop = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 145);
            label4.Name = "label4";
            label4.Size = new Size(45, 13);
            label4.TabIndex = 3;
            label4.Text = "Supplier";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 104);
            label3.Name = "label3";
            label3.Size = new Size(72, 13);
            label3.TabIndex = 2;
            label3.Text = "Manufacturer";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 63);
            label2.Name = "label2";
            label2.Size = new Size(77, 13);
            label2.TabIndex = 1;
            label2.Text = "Product Family";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 22);
            label1.Name = "label1";
            label1.Size = new Size(52, 13);
            label1.TabIndex = 0;
            label1.Text = "Category";
            // 
            // BtnSearchItemReload
            // 
            BtnSearchItemReload.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSearchItemReload.Location = new Point(141, 276);
            BtnSearchItemReload.Name = "BtnSearchItemReload";
            BtnSearchItemReload.Size = new Size(88, 23);
            BtnSearchItemReload.TabIndex = 34;
            BtnSearchItemReload.Text = "Reload";
            BtnSearchItemReload.UseVisualStyleBackColor = true;
            BtnSearchItemReload.Click += BtnSearchItemReload_Click;
            // 
            // PatientName
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            PatientName.DefaultCellStyle = dataGridViewCellStyle2;
            PatientName.HeaderText = "Name";
            PatientName.Name = "PatientName";
            PatientName.ReadOnly = true;
            PatientName.Resizable = DataGridViewTriState.False;
            PatientName.SortMode = DataGridViewColumnSortMode.NotSortable;
            PatientName.Width = 250;
            // 
            // PatientAddress
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            PatientAddress.DefaultCellStyle = dataGridViewCellStyle3;
            PatientAddress.HeaderText = "MaterialID";
            PatientAddress.Name = "PatientAddress";
            PatientAddress.ReadOnly = true;
            PatientAddress.Resizable = DataGridViewTriState.False;
            PatientAddress.SortMode = DataGridViewColumnSortMode.NotSortable;
            PatientAddress.Width = 125;
            // 
            // Column2
            // 
            Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            Column2.DefaultCellStyle = dataGridViewCellStyle4;
            Column2.HeaderText = "UOM";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Resizable = DataGridViewTriState.False;
            Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // sprice
            // 
            sprice.HeaderText = " S RATE";
            sprice.Name = "sprice";
            sprice.ReadOnly = true;
            sprice.Width = 75;
            // 
            // Column1
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            Column1.DefaultCellStyle = dataGridViewCellStyle5;
            Column1.HeaderText = "Id";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Visible = false;
            // 
            // FormSearchItems
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnSearchCancel;
            ClientSize = new Size(873, 332);
            Controls.Add(BtnSearchItemReload);
            Controls.Add(groupBox1);
            Controls.Add(BtnNewProduct);
            Controls.Add(toolStrip);
            Controls.Add(BtnSearchSelect);
            Controls.Add(GridViewItems);
            Controls.Add(statusStrip1);
            Controls.Add(BtnSearchCancel);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSearchItems";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Search Items";
            Load += FormSearchItems_Load;
            Controls.SetChildIndex(checkBoxIsPatient, 0);
            Controls.SetChildIndex(BtnSearchCancel, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(GridViewItems, 0);
            Controls.SetChildIndex(BtnSearchSelect, 0);
            Controls.SetChildIndex(toolStrip, 0);
            Controls.SetChildIndex(BtnNewProduct, 0);
            Controls.SetChildIndex(groupBox1, 0);
            Controls.SetChildIndex(BtnSearchItemReload, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewItems).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip;
        private ToolStripLabel toolStripLabel1;
        private Button BtnSearchSelect;
        private controls.DataViewVerticalScroll GridViewItems;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel PatientSearchErrorMsg;
        private Button BtnSearchCancel;
        private Button BtnNewProduct;
        private GroupBox groupBox1;
        private TextBox TextBoxCategory;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox TextBoxSupplier;
        private TextBox TextBoxProductFamily;
        private TextBox TextBoxManufacturer;
        private Button BtnSearchItemReload;
        private controls.ToolstripDelayedTextBox TextBoxSearchProduct;
        private DataGridViewTextBoxColumn PatientName;
        private DataGridViewTextBoxColumn PatientAddress;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn sprice;
        private DataGridViewTextBoxColumn Column1;
    }
}