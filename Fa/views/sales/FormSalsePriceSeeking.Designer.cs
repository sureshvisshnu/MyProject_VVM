namespace Fa.views.sales
{
    partial class FormSalsePriceSeeking
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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSalsePriceSeeking));
            GridViewItems = new fa.views.controls.DataViewVerticalScroll();
            sno = new DataGridViewTextBoxColumn();
            SalesDate = new DataGridViewTextBoxColumn();
            price = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            groupBox1 = new GroupBox();
            TextBoxSupplier = new TextBox();
            TextBoxProductFamily = new TextBox();
            TextBoxManufacturer = new TextBox();
            TextBoxCategory = new TextBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            BtnSearchSelect = new Button();
            toolStrip = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            TextBoxSearchProduct = new fa.views.controls.ToolstripDelayedTextBox();
            ((System.ComponentModel.ISupportInitialize)GridViewItems).BeginInit();
            groupBox1.SuspendLayout();
            toolStrip.SuspendLayout();
            SuspendLayout();
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
            GridViewItems.Columns.AddRange(new DataGridViewColumn[] { sno, SalesDate, price, Column1 });
            GridViewItems.EditMode = DataGridViewEditMode.EditProgrammatically;
            GridViewItems.EnableHeadersVisualStyles = false;
            GridViewItems.Location = new Point(13, 31);
            GridViewItems.MultiSelect = false;
            GridViewItems.Name = "GridViewItems";
            GridViewItems.ReadOnly = true;
            GridViewItems.RowHeadersVisible = false;
            GridViewItems.RowTemplate.Height = 20;
            GridViewItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewItems.ShowCellToolTips = false;
            GridViewItems.ShowEditingIcon = false;
            GridViewItems.Size = new Size(220, 152);
            GridViewItems.TabIndex = 29;
            // 
            // sno
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            sno.DefaultCellStyle = dataGridViewCellStyle2;
            sno.HeaderText = "Sl. No";
            sno.Name = "sno";
            sno.ReadOnly = true;
            sno.Resizable = DataGridViewTriState.False;
            sno.SortMode = DataGridViewColumnSortMode.NotSortable;
            sno.Width = 40;
            // 
            // SalesDate
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            SalesDate.DefaultCellStyle = dataGridViewCellStyle3;
            SalesDate.HeaderText = "Saled Date";
            SalesDate.Name = "SalesDate";
            SalesDate.ReadOnly = true;
            SalesDate.Resizable = DataGridViewTriState.False;
            SalesDate.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // price
            // 
            price.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            price.DefaultCellStyle = dataGridViewCellStyle4;
            price.HeaderText = "Rate";
            price.Name = "price";
            price.ReadOnly = true;
            price.Resizable = DataGridViewTriState.False;
            price.SortMode = DataGridViewColumnSortMode.NotSortable;
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
            groupBox1.Location = new Point(266, 31);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(171, 173);
            groupBox1.TabIndex = 35;
            groupBox1.TabStop = false;
            groupBox1.Text = "Item Details";
            // 
            // TextBoxSupplier
            // 
            TextBoxSupplier.BackColor = Color.White;
            TextBoxSupplier.Location = new Point(8, 140);
            TextBoxSupplier.Name = "TextBoxSupplier";
            TextBoxSupplier.ReadOnly = true;
            TextBoxSupplier.Size = new Size(157, 21);
            TextBoxSupplier.TabIndex = 7;
            TextBoxSupplier.TabStop = false;
            // 
            // TextBoxProductFamily
            // 
            TextBoxProductFamily.BackColor = Color.White;
            TextBoxProductFamily.Location = new Point(8, 68);
            TextBoxProductFamily.Name = "TextBoxProductFamily";
            TextBoxProductFamily.ReadOnly = true;
            TextBoxProductFamily.Size = new Size(181, 21);
            TextBoxProductFamily.TabIndex = 6;
            TextBoxProductFamily.TabStop = false;
            // 
            // TextBoxManufacturer
            // 
            TextBoxManufacturer.BackColor = Color.White;
            TextBoxManufacturer.Location = new Point(8, 104);
            TextBoxManufacturer.Name = "TextBoxManufacturer";
            TextBoxManufacturer.ReadOnly = true;
            TextBoxManufacturer.Size = new Size(157, 21);
            TextBoxManufacturer.TabIndex = 5;
            TextBoxManufacturer.TabStop = false;
            // 
            // TextBoxCategory
            // 
            TextBoxCategory.BackColor = Color.White;
            TextBoxCategory.Location = new Point(8, 33);
            TextBoxCategory.Name = "TextBoxCategory";
            TextBoxCategory.ReadOnly = true;
            TextBoxCategory.Size = new Size(181, 21);
            TextBoxCategory.TabIndex = 4;
            TextBoxCategory.TabStop = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(5, 126);
            label4.Name = "label4";
            label4.Size = new Size(45, 13);
            label4.TabIndex = 3;
            label4.Text = "Supplier";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(5, 90);
            label3.Name = "label3";
            label3.Size = new Size(72, 13);
            label3.TabIndex = 2;
            label3.Text = "Manufacturer";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(5, 55);
            label2.Name = "label2";
            label2.Size = new Size(77, 13);
            label2.TabIndex = 1;
            label2.Text = "Product Family";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(5, 19);
            label1.Name = "label1";
            label1.Size = new Size(52, 13);
            label1.TabIndex = 0;
            label1.Text = "Category";
            // 
            // BtnSearchSelect
            // 
            BtnSearchSelect.Enabled = false;
            BtnSearchSelect.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSearchSelect.Location = new Point(158, 187);
            BtnSearchSelect.Name = "BtnSearchSelect";
            BtnSearchSelect.Size = new Size(75, 20);
            BtnSearchSelect.TabIndex = 34;
            BtnSearchSelect.Text = "OK";
            BtnSearchSelect.UseVisualStyleBackColor = true;
            BtnSearchSelect.Click += BtnSearchSelect_Click;
            // 
            // toolStrip
            // 
            toolStrip.BackColor = SystemColors.ControlLight;
            toolStrip.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip.Items.AddRange(new ToolStripItem[] { toolStripLabel1, TextBoxSearchProduct });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Padding = new Padding(4, 4, 4, 4);
            toolStrip.Size = new Size(447, 32);
            toolStrip.TabIndex = 36;
            toolStrip.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(74, 21);
            toolStripLabel1.Text = "Product Name";
            // 
            // TextBoxSearchProduct
            // 
            TextBoxSearchProduct.AutoSize = false;
            TextBoxSearchProduct.Delay = true;
            TextBoxSearchProduct.DelayTime = 1000;
            TextBoxSearchProduct.Name = "TextBoxSearchProduct";
            TextBoxSearchProduct.Size = new Size(172, 21);
            // 
            // FormSalsePriceSeeking
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnSearchSelect;
            ClientSize = new Size(447, 218);
            Controls.Add(toolStrip);
            Controls.Add(groupBox1);
            Controls.Add(BtnSearchSelect);
            Controls.Add(GridViewItems);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSalsePriceSeeking";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Previous Price Search";
            ((System.ComponentModel.ISupportInitialize)GridViewItems).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private fa.views.controls.DataViewVerticalScroll GridViewItems;
        private DataGridViewTextBoxColumn sno;
        private DataGridViewTextBoxColumn SalesDate;
        private DataGridViewTextBoxColumn price;
        private DataGridViewTextBoxColumn Column1;
        private GroupBox groupBox1;
        private TextBox TextBoxSupplier;
        private TextBox TextBoxProductFamily;
        private TextBox TextBoxManufacturer;
        private TextBox TextBoxCategory;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button BtnSearchSelect;
        private ToolStrip toolStrip;
        private ToolStripLabel toolStripLabel1;
        private fa.views.controls.ToolstripDelayedTextBox TextBoxSearchProduct;
    }
}