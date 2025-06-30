namespace fa.views.catalog
{
    partial class FormItemPriceCalculator
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormItemPriceCalculator));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            TextBoxProductCode = new TextBox();
            TextBoxProductName = new TextBox();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            BtnPriceCalculatorSave = new Button();
            BtnPriceCalculatorCancel = new Button();
            statusStrip1 = new StatusStrip();
            CatalogErrorMsg = new ToolStripStatusLabel();
            TextBoxWholesaleMargin = new controls.text.CurrencyTextBox();
            TextBoxRetailMargin = new controls.text.CurrencyTextBox();
            TextBoxProductCost = new controls.text.CurrencyTextBox();
            TextBoxWholesalePrice = new controls.text.NumberTextBox(components);
            TextBoxRetailPrice = new controls.text.NumberTextBox(components);
            label8 = new Label();
            label9 = new Label();
            TextBoxXFactorRetail = new TextBox();
            TextBoxXFactorWholeSale = new TextBox();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(18, 242);
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(18, 216);
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(260, 97);
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(15, 12);
            label1.Name = "label1";
            label1.Size = new Size(72, 13);
            label1.TabIndex = 0;
            label1.Text = "Product Code";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(15, 52);
            label2.Name = "label2";
            label2.Size = new Size(74, 13);
            label2.TabIndex = 1;
            label2.Text = "Product Name";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(15, 94);
            label3.Name = "label3";
            label3.Size = new Size(90, 13);
            label3.TabIndex = 2;
            label3.Text = "Purchase Price";
            // 
            // TextBoxProductCode
            // 
            TextBoxProductCode.Location = new Point(18, 28);
            TextBoxProductCode.Name = "TextBoxProductCode";
            TextBoxProductCode.ReadOnly = true;
            TextBoxProductCode.Size = new Size(139, 21);
            TextBoxProductCode.TabIndex = 44;
            TextBoxProductCode.TabStop = false;
            // 
            // TextBoxProductName
            // 
            TextBoxProductName.Location = new Point(18, 70);
            TextBoxProductName.Name = "TextBoxProductName";
            TextBoxProductName.ReadOnly = true;
            TextBoxProductName.Size = new Size(333, 21);
            TextBoxProductName.TabIndex = 444;
            TextBoxProductName.TabStop = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(15, 134);
            label4.Name = "label4";
            label4.Size = new Size(98, 13);
            label4.TabIndex = 6;
            label4.Text = "Retail Margin %";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label5.Location = new Point(15, 174);
            label5.Name = "label5";
            label5.Size = new Size(123, 13);
            label5.TabIndex = 8;
            label5.Text = "Wholesale Margin %";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label6.Location = new Point(223, 174);
            label6.Name = "label6";
            label6.Size = new Size(82, 13);
            label6.TabIndex = 12;
            label6.Text = "Wholesale Price";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label7.Location = new Point(223, 134);
            label7.Name = "label7";
            label7.Size = new Size(60, 13);
            label7.TabIndex = 10;
            label7.Text = "Retail Price";
            // 
            // BtnPriceCalculatorSave
            // 
            BtnPriceCalculatorSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPriceCalculatorSave.Location = new Point(260, 218);
            BtnPriceCalculatorSave.Name = "BtnPriceCalculatorSave";
            BtnPriceCalculatorSave.Size = new Size(91, 23);
            BtnPriceCalculatorSave.TabIndex = 4;
            BtnPriceCalculatorSave.Text = "Save [F8]";
            BtnPriceCalculatorSave.UseVisualStyleBackColor = true;
            BtnPriceCalculatorSave.Click += BtnCatalogSave_Click;
            // 
            // BtnPriceCalculatorCancel
            // 
            BtnPriceCalculatorCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPriceCalculatorCancel.Location = new Point(165, 218);
            BtnPriceCalculatorCancel.Name = "BtnPriceCalculatorCancel";
            BtnPriceCalculatorCancel.Size = new Size(89, 23);
            BtnPriceCalculatorCancel.TabIndex = 5;
            BtnPriceCalculatorCancel.Text = "Cancel [Esc]";
            BtnPriceCalculatorCancel.UseVisualStyleBackColor = true;
            BtnPriceCalculatorCancel.Click += BtnCatalogCancel_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { CatalogErrorMsg });
            statusStrip1.Location = new Point(0, 273);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(390, 22);
            statusStrip1.TabIndex = 54;
            statusStrip1.Text = "statusStrip1";
            // 
            // CatalogErrorMsg
            // 
            CatalogErrorMsg.Name = "CatalogErrorMsg";
            CatalogErrorMsg.Size = new Size(25, 17);
            CatalogErrorMsg.Text = "      ";
            // 
            // TextBoxWholesaleMargin
            // 
            TextBoxWholesaleMargin.Decimals = 2;
            TextBoxWholesaleMargin.Length = 6;
            TextBoxWholesaleMargin.Location = new Point(18, 190);
            TextBoxWholesaleMargin.Name = "TextBoxWholesaleMargin";
            TextBoxWholesaleMargin.Size = new Size(120, 21);
            TextBoxWholesaleMargin.TabIndex = 3;
            TextBoxWholesaleMargin.Text = "0.00";
            TextBoxWholesaleMargin.TextAlign = HorizontalAlignment.Right;
            TextBoxWholesaleMargin.TextChanged += TextBoxProductCost_TextChanged;
            // 
            // TextBoxRetailMargin
            // 
            TextBoxRetailMargin.Decimals = 2;
            TextBoxRetailMargin.Length = 6;
            TextBoxRetailMargin.Location = new Point(18, 150);
            TextBoxRetailMargin.Name = "TextBoxRetailMargin";
            TextBoxRetailMargin.Size = new Size(120, 21);
            TextBoxRetailMargin.TabIndex = 2;
            TextBoxRetailMargin.Text = "0.00";
            TextBoxRetailMargin.TextAlign = HorizontalAlignment.Right;
            TextBoxRetailMargin.TextChanged += TextBoxProductCost_TextChanged;
            // 
            // TextBoxProductCost
            // 
            TextBoxProductCost.Decimals = 2;
            TextBoxProductCost.Length = 10;
            TextBoxProductCost.Location = new Point(18, 110);
            TextBoxProductCost.Name = "TextBoxProductCost";
            TextBoxProductCost.Size = new Size(120, 21);
            TextBoxProductCost.TabIndex = 1;
            TextBoxProductCost.Text = "0.00";
            TextBoxProductCost.TextAlign = HorizontalAlignment.Right;
            TextBoxProductCost.TextChanged += TextBoxProductCost_TextChanged;
            // 
            // TextBoxWholesalePrice
            // 
            TextBoxWholesalePrice.BackColor = SystemColors.Window;
            TextBoxWholesalePrice.Location = new Point(223, 190);
            TextBoxWholesalePrice.Name = "TextBoxWholesalePrice";
            TextBoxWholesalePrice.ReadOnly = true;
            TextBoxWholesalePrice.Size = new Size(127, 21);
            TextBoxWholesalePrice.TabIndex = 53;
            TextBoxWholesalePrice.TabStop = false;
            TextBoxWholesalePrice.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxRetailPrice
            // 
            TextBoxRetailPrice.BackColor = SystemColors.Window;
            TextBoxRetailPrice.Location = new Point(223, 150);
            TextBoxRetailPrice.Name = "TextBoxRetailPrice";
            TextBoxRetailPrice.ReadOnly = true;
            TextBoxRetailPrice.Size = new Size(127, 21);
            TextBoxRetailPrice.TabIndex = 52;
            TextBoxRetailPrice.TabStop = false;
            TextBoxRetailPrice.TextAlign = HorizontalAlignment.Right;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label8.Location = new Point(152, 174);
            label8.Name = "label8";
            label8.Size = new Size(55, 13);
            label8.TabIndex = 448;
            label8.Text = "X-Factor";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label9.Location = new Point(151, 134);
            label9.Name = "label9";
            label9.Size = new Size(55, 13);
            label9.TabIndex = 447;
            label9.Text = "X-Factor";
            // 
            // TextBoxXFactorRetail
            // 
            TextBoxXFactorRetail.BackColor = SystemColors.Window;
            TextBoxXFactorRetail.Location = new Point(152, 150);
            TextBoxXFactorRetail.MaxLength = 5;
            TextBoxXFactorRetail.Name = "TextBoxXFactorRetail";
            TextBoxXFactorRetail.ReadOnly = true;
            TextBoxXFactorRetail.Size = new Size(54, 21);
            TextBoxXFactorRetail.TabIndex = 449;
            TextBoxXFactorRetail.TextAlign = HorizontalAlignment.Right;
            TextBoxXFactorRetail.KeyPress += TextBoxXFactorRetail_KeyPress;
            // 
            // TextBoxXFactorWholeSale
            // 
            TextBoxXFactorWholeSale.BackColor = SystemColors.Window;
            TextBoxXFactorWholeSale.Location = new Point(152, 190);
            TextBoxXFactorWholeSale.MaxLength = 5;
            TextBoxXFactorWholeSale.Name = "TextBoxXFactorWholeSale";
            TextBoxXFactorWholeSale.ReadOnly = true;
            TextBoxXFactorWholeSale.Size = new Size(54, 21);
            TextBoxXFactorWholeSale.TabIndex = 450;
            TextBoxXFactorWholeSale.TextAlign = HorizontalAlignment.Right;
            TextBoxXFactorWholeSale.KeyPress += TextBoxXFactorRetail_KeyPress;
            // 
            // FormItemPriceCalculator
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(390, 295);
            Controls.Add(TextBoxXFactorWholeSale);
            Controls.Add(TextBoxXFactorRetail);
            Controls.Add(label8);
            Controls.Add(label9);
            Controls.Add(TextBoxWholesaleMargin);
            Controls.Add(TextBoxRetailMargin);
            Controls.Add(TextBoxProductCost);
            Controls.Add(statusStrip1);
            Controls.Add(TextBoxWholesalePrice);
            Controls.Add(TextBoxRetailPrice);
            Controls.Add(BtnPriceCalculatorSave);
            Controls.Add(BtnPriceCalculatorCancel);
            Controls.Add(label6);
            Controls.Add(label7);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(TextBoxProductName);
            Controls.Add(TextBoxProductCode);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormItemPriceCalculator";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Product Price Calculator";
            Load += FormItemPriceCalculator_Load;
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(label2, 0);
            Controls.SetChildIndex(label3, 0);
            Controls.SetChildIndex(TextBoxProductCode, 0);
            Controls.SetChildIndex(TextBoxProductName, 0);
            Controls.SetChildIndex(label4, 0);
            Controls.SetChildIndex(label5, 0);
            Controls.SetChildIndex(label7, 0);
            Controls.SetChildIndex(label6, 0);
            Controls.SetChildIndex(BtnPriceCalculatorCancel, 0);
            Controls.SetChildIndex(BtnPriceCalculatorSave, 0);
            Controls.SetChildIndex(TextBoxRetailPrice, 0);
            Controls.SetChildIndex(TextBoxWholesalePrice, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(TextBoxProductCost, 0);
            Controls.SetChildIndex(TextBoxRetailMargin, 0);
            Controls.SetChildIndex(TextBoxWholesaleMargin, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(label9, 0);
            Controls.SetChildIndex(label8, 0);
            Controls.SetChildIndex(TextBoxXFactorRetail, 0);
            Controls.SetChildIndex(TextBoxXFactorWholeSale, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextBoxProductCode;
        private System.Windows.Forms.TextBox TextBoxProductName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button BtnPriceCalculatorSave;
        private System.Windows.Forms.Button BtnPriceCalculatorCancel;
        private controls.text.NumberTextBox TextBoxWholesalePrice;
        private controls.text.NumberTextBox TextBoxRetailPrice;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel CatalogErrorMsg;
        private controls.text.CurrencyTextBox TextBoxProductCost;
        private controls.text.CurrencyTextBox TextBoxRetailMargin;
        private controls.text.CurrencyTextBox TextBoxWholesaleMargin;
        private controls.text.CurrencyTextBox currencyTextBox1;
        private controls.text.CurrencyTextBox currencyTextBox2;
        private Label label8;
        private Label label9;
        private TextBox TextBoxProductXFactorRetail;
        private TextBox textBox1;
        private TextBox TextBoxXFactorRetail;
        private TextBox TextBoxXFactorWholeSale;
    }
}