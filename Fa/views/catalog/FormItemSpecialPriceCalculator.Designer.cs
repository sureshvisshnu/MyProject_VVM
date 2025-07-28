namespace Fa.views.catalog
{
    partial class FormItemSpecialPriceCalculator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormItemSpecialPriceCalculator));
            statusStrip1 = new StatusStrip();
            CatalogErrorMsg = new ToolStripStatusLabel();
            TextBoxXFactorWholeSale = new TextBox();
            TextBoxXFactorRetail = new TextBox();
            label8 = new Label();
            label9 = new Label();
            TextBoxWholesaleMargin = new fa.views.controls.text.CurrencyTextBox();
            TextBoxRetailMargin = new fa.views.controls.text.CurrencyTextBox();
            TextBoxPurchasePrice = new fa.views.controls.text.CurrencyTextBox();
            TextBoxWholesalePrice = new fa.views.controls.text.NumberTextBox(components);
            TextBoxRetailPrice = new fa.views.controls.text.NumberTextBox(components);
            BtnPriceCalculatorSave = new Button();
            BtnPriceCalculatorCancel = new Button();
            label6 = new Label();
            label7 = new Label();
            label5 = new Label();
            label4 = new Label();
            TextBoxProductName = new TextBox();
            TextBoxProductCode = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            TextBoxProductCost = new fa.views.controls.text.CurrencyTextBox();
            label10 = new Label();
            TextBoxAddedCostPercentage = new fa.views.controls.text.CurrencyTextBox();
            label11 = new Label();
            TextBoxMrpPrice = new fa.views.controls.text.CurrencyTextBox();
            label12 = new Label();
            label13 = new Label();
            comboMrpPercentage = new ComboBox();
            TextBoxMrpPercentage = new fa.views.controls.text.CurrencyTextBox();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(256, 306);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(132, 306);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(13, 306);
            // 
            // checkBoxIsPatient
            // 
            checkBoxIsPatient.Location = new Point(15, 285);
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { CatalogErrorMsg });
            statusStrip1.Location = new Point(0, 327);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(360, 22);
            statusStrip1.TabIndex = 55;
            statusStrip1.Text = "VVMstatusPriceStrip";
            // 
            // CatalogErrorMsg
            // 
            CatalogErrorMsg.Name = "CatalogErrorMsg";
            CatalogErrorMsg.Size = new Size(25, 17);
            CatalogErrorMsg.Text = "      ";
            // 
            // TextBoxXFactorWholeSale
            // 
            TextBoxXFactorWholeSale.BackColor = SystemColors.Window;
            TextBoxXFactorWholeSale.Location = new Point(150, 257);
            TextBoxXFactorWholeSale.MaxLength = 5;
            TextBoxXFactorWholeSale.Name = "TextBoxXFactorWholeSale";
            TextBoxXFactorWholeSale.ReadOnly = true;
            TextBoxXFactorWholeSale.Size = new Size(54, 23);
            TextBoxXFactorWholeSale.TabIndex = 470;
            TextBoxXFactorWholeSale.TextAlign = HorizontalAlignment.Right;
            TextBoxXFactorWholeSale.KeyPress += TextBoxXFactorWholeSale_KeyPress;
            // 
            // TextBoxXFactorRetail
            // 
            TextBoxXFactorRetail.BackColor = SystemColors.Window;
            TextBoxXFactorRetail.Location = new Point(150, 217);
            TextBoxXFactorRetail.MaxLength = 5;
            TextBoxXFactorRetail.Name = "TextBoxXFactorRetail";
            TextBoxXFactorRetail.ReadOnly = true;
            TextBoxXFactorRetail.Size = new Size(54, 23);
            TextBoxXFactorRetail.TabIndex = 469;
            TextBoxXFactorRetail.TextAlign = HorizontalAlignment.Right;
            TextBoxXFactorRetail.KeyPress += TextBoxXFactorRetail_KeyPress;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label8.Location = new Point(150, 241);
            label8.Name = "label8";
            label8.Size = new Size(55, 13);
            label8.TabIndex = 468;
            label8.Text = "X-Factor";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label9.Location = new Point(149, 201);
            label9.Name = "label9";
            label9.Size = new Size(55, 13);
            label9.TabIndex = 467;
            label9.Text = "X-Factor";
            // 
            // TextBoxWholesaleMargin
            // 
            TextBoxWholesaleMargin.Decimals = 2;
            TextBoxWholesaleMargin.Length = 6;
            TextBoxWholesaleMargin.Location = new Point(16, 257);
            TextBoxWholesaleMargin.Name = "TextBoxWholesaleMargin";
            TextBoxWholesaleMargin.Size = new Size(120, 23);
            TextBoxWholesaleMargin.TabIndex = 6;
            TextBoxWholesaleMargin.Text = "0.00";
            TextBoxWholesaleMargin.TextAlign = HorizontalAlignment.Right;
            TextBoxWholesaleMargin.TextChanged += TextBoxWholesaleMargin_TextChanged;
            // 
            // TextBoxRetailMargin
            // 
            TextBoxRetailMargin.Decimals = 2;
            TextBoxRetailMargin.Length = 6;
            TextBoxRetailMargin.Location = new Point(16, 217);
            TextBoxRetailMargin.Name = "TextBoxRetailMargin";
            TextBoxRetailMargin.Size = new Size(120, 23);
            TextBoxRetailMargin.TabIndex = 5;
            TextBoxRetailMargin.Text = "0.00";
            TextBoxRetailMargin.TextAlign = HorizontalAlignment.Right;
            TextBoxRetailMargin.TextChanged += TextBoxRetailMargin_TextChanged;
            // 
            // TextBoxPurchasePrice
            // 
            TextBoxPurchasePrice.Decimals = 2;
            TextBoxPurchasePrice.Length = 10;
            TextBoxPurchasePrice.Location = new Point(15, 117);
            TextBoxPurchasePrice.Name = "TextBoxPurchasePrice";
            TextBoxPurchasePrice.Size = new Size(120, 23);
            TextBoxPurchasePrice.TabIndex = 2;
            TextBoxPurchasePrice.Text = "0.00";
            TextBoxPurchasePrice.TextAlign = HorizontalAlignment.Right;
            TextBoxPurchasePrice.TextChanged += TextBoxPurchasePrice_TextChanged;
            // 
            // TextBoxWholesalePrice
            // 
            TextBoxWholesalePrice.BackColor = SystemColors.Window;
            TextBoxWholesalePrice.Location = new Point(228, 257);
            TextBoxWholesalePrice.Name = "TextBoxWholesalePrice";
            TextBoxWholesalePrice.ReadOnly = true;
            TextBoxWholesalePrice.Size = new Size(120, 23);
            TextBoxWholesalePrice.TabIndex = 465;
            TextBoxWholesalePrice.TabStop = false;
            TextBoxWholesalePrice.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxRetailPrice
            // 
            TextBoxRetailPrice.BackColor = SystemColors.Window;
            TextBoxRetailPrice.Location = new Point(228, 217);
            TextBoxRetailPrice.Name = "TextBoxRetailPrice";
            TextBoxRetailPrice.ReadOnly = true;
            TextBoxRetailPrice.Size = new Size(120, 23);
            TextBoxRetailPrice.TabIndex = 464;
            TextBoxRetailPrice.TabStop = false;
            TextBoxRetailPrice.TextAlign = HorizontalAlignment.Right;
            // 
            // BtnPriceCalculatorSave
            // 
            BtnPriceCalculatorSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPriceCalculatorSave.Location = new Point(257, 285);
            BtnPriceCalculatorSave.Name = "BtnPriceCalculatorSave";
            BtnPriceCalculatorSave.Size = new Size(91, 23);
            BtnPriceCalculatorSave.TabIndex = 7;
            BtnPriceCalculatorSave.Text = "Save [F8]";
            BtnPriceCalculatorSave.UseVisualStyleBackColor = true;
            BtnPriceCalculatorSave.Click += BtnPriceCalculatorSave_Click;
            // 
            // BtnPriceCalculatorCancel
            // 
            BtnPriceCalculatorCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPriceCalculatorCancel.Location = new Point(163, 285);
            BtnPriceCalculatorCancel.Name = "BtnPriceCalculatorCancel";
            BtnPriceCalculatorCancel.Size = new Size(89, 23);
            BtnPriceCalculatorCancel.TabIndex = 8;
            BtnPriceCalculatorCancel.Text = "Cancel [Esc]";
            BtnPriceCalculatorCancel.UseVisualStyleBackColor = true;
            BtnPriceCalculatorCancel.Click += BtnPriceCalculatorCancel_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label6.Location = new Point(228, 241);
            label6.Name = "label6";
            label6.Size = new Size(82, 13);
            label6.TabIndex = 462;
            label6.Text = "Wholesale Price";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label7.Location = new Point(228, 201);
            label7.Name = "label7";
            label7.Size = new Size(60, 13);
            label7.TabIndex = 461;
            label7.Text = "Retail Price";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label5.Location = new Point(13, 241);
            label5.Name = "label5";
            label5.Size = new Size(123, 13);
            label5.TabIndex = 460;
            label5.Text = "Wholesale Margin %";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(13, 201);
            label4.Name = "label4";
            label4.Size = new Size(98, 13);
            label4.TabIndex = 459;
            label4.Text = "Retail Margin %";
            // 
            // TextBoxProductName
            // 
            TextBoxProductName.Location = new Point(15, 77);
            TextBoxProductName.Name = "TextBoxProductName";
            TextBoxProductName.ReadOnly = true;
            TextBoxProductName.Size = new Size(333, 23);
            TextBoxProductName.TabIndex = 1;
            TextBoxProductName.TabStop = false;
            // 
            // TextBoxProductCode
            // 
            TextBoxProductCode.Location = new Point(15, 35);
            TextBoxProductCode.Name = "TextBoxProductCode";
            TextBoxProductCode.ReadOnly = true;
            TextBoxProductCode.Size = new Size(139, 23);
            TextBoxProductCode.TabIndex = 0;
            TextBoxProductCode.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(12, 101);
            label3.Name = "label3";
            label3.Size = new Size(90, 13);
            label3.TabIndex = 454;
            label3.Text = "Purchase Price";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(12, 59);
            label2.Name = "label2";
            label2.Size = new Size(74, 13);
            label2.TabIndex = 452;
            label2.Text = "Product Name";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(12, 19);
            label1.Name = "label1";
            label1.Size = new Size(72, 13);
            label1.TabIndex = 451;
            label1.Text = "Product Code";
            // 
            // TextBoxProductCost
            // 
            TextBoxProductCost.Decimals = 2;
            TextBoxProductCost.Length = 10;
            TextBoxProductCost.Location = new Point(228, 117);
            TextBoxProductCost.Name = "TextBoxProductCost";
            TextBoxProductCost.Size = new Size(120, 23);
            TextBoxProductCost.TabIndex = 471;
            TextBoxProductCost.Text = "0.00";
            TextBoxProductCost.TextAlign = HorizontalAlignment.Right;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label10.Location = new Point(228, 101);
            label10.Name = "label10";
            label10.Size = new Size(63, 13);
            label10.TabIndex = 472;
            label10.Text = "Cost Price";
            // 
            // TextBoxAddedCostPercentage
            // 
            TextBoxAddedCostPercentage.Decimals = 2;
            TextBoxAddedCostPercentage.Length = 6;
            TextBoxAddedCostPercentage.Location = new Point(148, 117);
            TextBoxAddedCostPercentage.Name = "TextBoxAddedCostPercentage";
            TextBoxAddedCostPercentage.Size = new Size(54, 23);
            TextBoxAddedCostPercentage.TabIndex = 3;
            TextBoxAddedCostPercentage.Text = "0.00";
            TextBoxAddedCostPercentage.TextAlign = HorizontalAlignment.Right;
            TextBoxAddedCostPercentage.TextChanged += TextBoxAddedPercentsage_TextChanged;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label11.Location = new Point(148, 101);
            label11.Name = "label11";
            label11.Size = new Size(70, 13);
            label11.TabIndex = 474;
            label11.Text = "Expense %";
            // 
            // TextBoxMrpPrice
            // 
            TextBoxMrpPrice.Decimals = 2;
            TextBoxMrpPrice.Length = 10;
            TextBoxMrpPrice.Location = new Point(149, 159);
            TextBoxMrpPrice.Name = "TextBoxMrpPrice";
            TextBoxMrpPrice.Size = new Size(54, 23);
            TextBoxMrpPrice.TabIndex = 475;
            TextBoxMrpPrice.Text = "0.00";
            TextBoxMrpPrice.TextAlign = HorizontalAlignment.Right;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label12.Location = new Point(146, 143);
            label12.Name = "label12";
            label12.Size = new Size(63, 13);
            label12.TabIndex = 476;
            label12.Text = "MRP Price";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label13.Location = new Point(13, 143);
            label13.Name = "label13";
            label13.Size = new Size(100, 13);
            label13.TabIndex = 478;
            label13.Text = "MRP Percentage";
            // 
            // comboMrpPercentage
            // 
            comboMrpPercentage.FormattingEnabled = true;
            comboMrpPercentage.Location = new Point(13, 159);
            comboMrpPercentage.Name = "comboMrpPercentage";
            comboMrpPercentage.Size = new Size(121, 23);
            comboMrpPercentage.TabIndex = 4;
            comboMrpPercentage.SelectedIndexChanged += comboMrpPercentage_SelectedIndexChanged;
            comboMrpPercentage.TextChanged += comboMrpPercentage_TextChanged;
            // 
            // TextBoxMrpPercentage
            // 
            TextBoxMrpPercentage.Decimals = 2;
            TextBoxMrpPercentage.Length = 6;
            TextBoxMrpPercentage.Location = new Point(256, 159);
            TextBoxMrpPercentage.Name = "TextBoxMrpPercentage";
            TextBoxMrpPercentage.Size = new Size(54, 23);
            TextBoxMrpPercentage.TabIndex = 479;
            TextBoxMrpPercentage.Text = "0.00";
            TextBoxMrpPercentage.TextAlign = HorizontalAlignment.Right;
            TextBoxMrpPercentage.Visible = false;
            // 
            // FormItemSpecialPriceCalculator
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(360, 349);
            Controls.Add(TextBoxMrpPercentage);
            Controls.Add(comboMrpPercentage);
            Controls.Add(label13);
            Controls.Add(TextBoxMrpPrice);
            Controls.Add(label12);
            Controls.Add(label11);
            Controls.Add(TextBoxAddedCostPercentage);
            Controls.Add(TextBoxProductCost);
            Controls.Add(label10);
            Controls.Add(TextBoxXFactorWholeSale);
            Controls.Add(TextBoxXFactorRetail);
            Controls.Add(label8);
            Controls.Add(label9);
            Controls.Add(TextBoxWholesaleMargin);
            Controls.Add(TextBoxRetailMargin);
            Controls.Add(TextBoxPurchasePrice);
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
            Controls.Add(statusStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormItemSpecialPriceCalculator";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Special Price Calculator";
            Load += FormItemSpecialPriceCalculator_Load;
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(checkBoxIsPatient, 0);
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
            Controls.SetChildIndex(TextBoxPurchasePrice, 0);
            Controls.SetChildIndex(TextBoxRetailMargin, 0);
            Controls.SetChildIndex(TextBoxWholesaleMargin, 0);
            Controls.SetChildIndex(label9, 0);
            Controls.SetChildIndex(label8, 0);
            Controls.SetChildIndex(TextBoxXFactorRetail, 0);
            Controls.SetChildIndex(TextBoxXFactorWholeSale, 0);
            Controls.SetChildIndex(label10, 0);
            Controls.SetChildIndex(TextBoxProductCost, 0);
            Controls.SetChildIndex(TextBoxAddedCostPercentage, 0);
            Controls.SetChildIndex(label11, 0);
            Controls.SetChildIndex(label12, 0);
            Controls.SetChildIndex(TextBoxMrpPrice, 0);
            Controls.SetChildIndex(label13, 0);
            Controls.SetChildIndex(comboMrpPercentage, 0);
            Controls.SetChildIndex(TextBoxMrpPercentage, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusStrip1;
        private ToolStripStatusLabel CatalogErrorMsg;
        private TextBox TextBoxXFactorWholeSale;
        private TextBox TextBoxXFactorRetail;
        private Label label8;
        private Label label9;
        private fa.views.controls.text.CurrencyTextBox TextBoxWholesaleMargin;
        private fa.views.controls.text.CurrencyTextBox TextBoxRetailMargin;
        private fa.views.controls.text.CurrencyTextBox TextBoxPurchasePrice;
        private fa.views.controls.text.NumberTextBox TextBoxWholesalePrice;
        private fa.views.controls.text.NumberTextBox TextBoxRetailPrice;
        private Button BtnPriceCalculatorSave;
        private Button BtnPriceCalculatorCancel;
        private Label label6;
        private Label label7;
        private Label label5;
        private Label label4;
        private TextBox TextBoxProductName;
        private TextBox TextBoxProductCode;
        private Label label3;
        private Label label2;
        private Label label1;
        private fa.views.controls.text.CurrencyTextBox TextBoxProductCost;
        private Label label10;
        private fa.views.controls.text.CurrencyTextBox TextBoxAddedCostPercentage;
        private Label label11;
        private fa.views.controls.text.CurrencyTextBox TextBoxMrpPrice;
        private Label label12;
        private Label label13;
        private ComboBox comboMrpPercentage;
        private fa.views.controls.text.CurrencyTextBox TextBoxMrpPercentage;
    }
}