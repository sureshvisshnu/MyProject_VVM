namespace Fa.views.catalog
{
    partial class FormSpecialPrice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSpecialPrice));
            TextBoxXFactorWholeSale = new TextBox();
            label18 = new Label();
            TextBoxSpecialMargin = new fa.views.controls.text.CurrencyTextBox();
            label19 = new Label();
            TextBoxXFactorRetail = new TextBox();
            label16 = new Label();
            TextBoxLineMargin = new fa.views.controls.text.CurrencyTextBox();
            label17 = new Label();
            TextBoxSpecialPrice = new fa.views.controls.text.NumberTextBox(components);
            label15 = new Label();
            TextBoxLinePrice = new fa.views.controls.text.NumberTextBox(components);
            label14 = new Label();
            TextBoxProductName = new TextBox();
            TextBoxProductCode = new TextBox();
            label2 = new Label();
            label1 = new Label();
            BtnPriceCalculatorSave = new Button();
            BtnPriceCalculatorCancel = new Button();
            statusStrip1 = new StatusStrip();
            CatalogErrorMsg = new ToolStripStatusLabel();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(200, 78);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(177, 46);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(177, 13);
            // 
            // checkBoxIsPatient
            // 
            checkBoxIsPatient.Location = new Point(177, 15);
            // 
            // TextBoxXFactorWholeSale
            // 
            TextBoxXFactorWholeSale.BackColor = SystemColors.Window;
            TextBoxXFactorWholeSale.Location = new Point(149, 170);
            TextBoxXFactorWholeSale.MaxLength = 5;
            TextBoxXFactorWholeSale.Name = "TextBoxXFactorWholeSale";
            TextBoxXFactorWholeSale.ReadOnly = true;
            TextBoxXFactorWholeSale.Size = new Size(54, 23);
            TextBoxXFactorWholeSale.TabIndex = 507;
            TextBoxXFactorWholeSale.TextAlign = HorizontalAlignment.Right;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label18.Location = new Point(149, 154);
            label18.Name = "label18";
            label18.Size = new Size(55, 13);
            label18.TabIndex = 506;
            label18.Text = "X-Factor";
            // 
            // TextBoxSpecialMargin
            // 
            TextBoxSpecialMargin.Decimals = 2;
            TextBoxSpecialMargin.Length = 6;
            TextBoxSpecialMargin.Location = new Point(11, 170);
            TextBoxSpecialMargin.Name = "TextBoxSpecialMargin";
            TextBoxSpecialMargin.Size = new Size(120, 23);
            TextBoxSpecialMargin.TabIndex = 504;
            TextBoxSpecialMargin.Text = "0.00";
            TextBoxSpecialMargin.TextAlign = HorizontalAlignment.Right;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label19.Location = new Point(11, 154);
            label19.Name = "label19";
            label19.Size = new Size(136, 13);
            label19.TabIndex = 505;
            label19.Text = "Special Price Margin %";
            // 
            // TextBoxXFactorRetail
            // 
            TextBoxXFactorRetail.BackColor = SystemColors.Window;
            TextBoxXFactorRetail.Location = new Point(149, 131);
            TextBoxXFactorRetail.MaxLength = 5;
            TextBoxXFactorRetail.Name = "TextBoxXFactorRetail";
            TextBoxXFactorRetail.ReadOnly = true;
            TextBoxXFactorRetail.Size = new Size(54, 23);
            TextBoxXFactorRetail.TabIndex = 503;
            TextBoxXFactorRetail.TextAlign = HorizontalAlignment.Right;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label16.Location = new Point(149, 115);
            label16.Name = "label16";
            label16.Size = new Size(55, 13);
            label16.TabIndex = 502;
            label16.Text = "X-Factor";
            // 
            // TextBoxLineMargin
            // 
            TextBoxLineMargin.Decimals = 2;
            TextBoxLineMargin.Length = 6;
            TextBoxLineMargin.Location = new Point(15, 131);
            TextBoxLineMargin.Name = "TextBoxLineMargin";
            TextBoxLineMargin.Size = new Size(120, 23);
            TextBoxLineMargin.TabIndex = 500;
            TextBoxLineMargin.Text = "0.00";
            TextBoxLineMargin.TextAlign = HorizontalAlignment.Right;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label17.Location = new Point(12, 115);
            label17.Name = "label17";
            label17.Size = new Size(119, 13);
            label17.TabIndex = 501;
            label17.Text = "Line Price Margin %";
            // 
            // TextBoxSpecialPrice
            // 
            TextBoxSpecialPrice.BackColor = SystemColors.Window;
            TextBoxSpecialPrice.Location = new Point(227, 170);
            TextBoxSpecialPrice.Name = "TextBoxSpecialPrice";
            TextBoxSpecialPrice.Size = new Size(100, 23);
            TextBoxSpecialPrice.TabIndex = 499;
            TextBoxSpecialPrice.TabStop = false;
            TextBoxSpecialPrice.Text = "0.00";
            TextBoxSpecialPrice.TextAlign = HorizontalAlignment.Right;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label15.Location = new Point(227, 154);
            label15.Name = "label15";
            label15.Size = new Size(66, 13);
            label15.TabIndex = 498;
            label15.Text = "Special Price";
            // 
            // TextBoxLinePrice
            // 
            TextBoxLinePrice.BackColor = SystemColors.Window;
            TextBoxLinePrice.Location = new Point(227, 131);
            TextBoxLinePrice.Name = "TextBoxLinePrice";
            TextBoxLinePrice.Size = new Size(100, 23);
            TextBoxLinePrice.TabIndex = 497;
            TextBoxLinePrice.TabStop = false;
            TextBoxLinePrice.Text = "0.00";
            TextBoxLinePrice.TextAlign = HorizontalAlignment.Right;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label14.Location = new Point(227, 115);
            label14.Name = "label14";
            label14.Size = new Size(52, 13);
            label14.TabIndex = 496;
            label14.Text = "Line Price";
            // 
            // TextBoxProductName
            // 
            TextBoxProductName.Location = new Point(15, 79);
            TextBoxProductName.Name = "TextBoxProductName";
            TextBoxProductName.ReadOnly = true;
            TextBoxProductName.Size = new Size(300, 23);
            TextBoxProductName.TabIndex = 493;
            TextBoxProductName.TabStop = false;
            // 
            // TextBoxProductCode
            // 
            TextBoxProductCode.Location = new Point(15, 37);
            TextBoxProductCode.Name = "TextBoxProductCode";
            TextBoxProductCode.ReadOnly = true;
            TextBoxProductCode.Size = new Size(139, 23);
            TextBoxProductCode.TabIndex = 492;
            TextBoxProductCode.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(12, 61);
            label2.Name = "label2";
            label2.Size = new Size(74, 13);
            label2.TabIndex = 495;
            label2.Text = "Product Name";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(12, 21);
            label1.Name = "label1";
            label1.Size = new Size(72, 13);
            label1.TabIndex = 494;
            label1.Text = "Product Code";
            // 
            // BtnPriceCalculatorSave
            // 
            BtnPriceCalculatorSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPriceCalculatorSave.Location = new Point(236, 199);
            BtnPriceCalculatorSave.Name = "BtnPriceCalculatorSave";
            BtnPriceCalculatorSave.Size = new Size(91, 23);
            BtnPriceCalculatorSave.TabIndex = 508;
            BtnPriceCalculatorSave.Text = "Save [F8]";
            BtnPriceCalculatorSave.UseVisualStyleBackColor = true;
            BtnPriceCalculatorSave.Click += BtnPriceCalculatorSave_Click;
            // 
            // BtnPriceCalculatorCancel
            // 
            BtnPriceCalculatorCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPriceCalculatorCancel.Location = new Point(142, 199);
            BtnPriceCalculatorCancel.Name = "BtnPriceCalculatorCancel";
            BtnPriceCalculatorCancel.Size = new Size(89, 23);
            BtnPriceCalculatorCancel.TabIndex = 509;
            BtnPriceCalculatorCancel.Text = "Cancel [Esc]";
            BtnPriceCalculatorCancel.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { CatalogErrorMsg });
            statusStrip1.Location = new Point(0, 227);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(335, 22);
            statusStrip1.TabIndex = 510;
            statusStrip1.Text = "VVMstatusPriceStrip";
            // 
            // CatalogErrorMsg
            // 
            CatalogErrorMsg.Name = "CatalogErrorMsg";
            CatalogErrorMsg.Size = new Size(25, 17);
            CatalogErrorMsg.Text = "      ";
            // 
            // FormSpecialPrice
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(335, 249);
            Controls.Add(statusStrip1);
            Controls.Add(BtnPriceCalculatorSave);
            Controls.Add(BtnPriceCalculatorCancel);
            Controls.Add(TextBoxXFactorWholeSale);
            Controls.Add(label18);
            Controls.Add(TextBoxSpecialMargin);
            Controls.Add(label19);
            Controls.Add(TextBoxXFactorRetail);
            Controls.Add(label16);
            Controls.Add(TextBoxLineMargin);
            Controls.Add(label17);
            Controls.Add(TextBoxSpecialPrice);
            Controls.Add(label15);
            Controls.Add(TextBoxLinePrice);
            Controls.Add(label14);
            Controls.Add(TextBoxProductName);
            Controls.Add(TextBoxProductCode);
            Controls.Add(label2);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSpecialPrice";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Special lPrice";
            Load += FormSpecialPrice_Load;
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(checkBoxIsPatient, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(label2, 0);
            Controls.SetChildIndex(TextBoxProductCode, 0);
            Controls.SetChildIndex(TextBoxProductName, 0);
            Controls.SetChildIndex(label14, 0);
            Controls.SetChildIndex(TextBoxLinePrice, 0);
            Controls.SetChildIndex(label15, 0);
            Controls.SetChildIndex(TextBoxSpecialPrice, 0);
            Controls.SetChildIndex(label17, 0);
            Controls.SetChildIndex(TextBoxLineMargin, 0);
            Controls.SetChildIndex(label16, 0);
            Controls.SetChildIndex(TextBoxXFactorRetail, 0);
            Controls.SetChildIndex(label19, 0);
            Controls.SetChildIndex(TextBoxSpecialMargin, 0);
            Controls.SetChildIndex(label18, 0);
            Controls.SetChildIndex(TextBoxXFactorWholeSale, 0);
            Controls.SetChildIndex(BtnPriceCalculatorCancel, 0);
            Controls.SetChildIndex(BtnPriceCalculatorSave, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox TextBoxXFactorWholeSale;
        private Label label18;
        private fa.views.controls.text.CurrencyTextBox TextBoxSpecialMargin;
        private Label label19;
        private TextBox TextBoxXFactorRetail;
        private Label label16;
        private fa.views.controls.text.CurrencyTextBox TextBoxLineMargin;
        private Label label17;
        private fa.views.controls.text.NumberTextBox TextBoxSpecialPrice;
        private Label label15;
        private fa.views.controls.text.NumberTextBox TextBoxLinePrice;
        private Label label14;
        private TextBox TextBoxProductName;
        private TextBox TextBoxProductCode;
        private Label label2;
        private Label label1;
        private Button BtnPriceCalculatorSave;
        private Button BtnPriceCalculatorCancel;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel CatalogErrorMsg;
    }
}