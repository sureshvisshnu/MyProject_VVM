namespace Fa.views.catalog
{
    partial class FormModifyXFactor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormModifyXFactor));
            statusStrip1 = new StatusStrip();
            CatalogErrorMsg = new ToolStripStatusLabel();
            BtnXFactorSave = new Button();
            BtnXFactorCancel = new Button();
            TextBoxXFactorWholeSale = new TextBox();
            label18 = new Label();
            TextBoxXFactorRetail = new TextBox();
            label16 = new Label();
            TextBoxProductName = new TextBox();
            TextBoxProductCode = new TextBox();
            label2 = new Label();
            label1 = new Label();
            ComboBoxProductRetailUOM = new fa.views.controls.ComboBoxSwapTextBox();
            ComboBoxProductWholeSaleUOM = new fa.views.controls.ComboBoxSwapTextBox();
            label31 = new Label();
            label29 = new Label();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(201, 78);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(201, 58);
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { CatalogErrorMsg });
            statusStrip1.Location = new Point(0, 227);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(335, 22);
            statusStrip1.TabIndex = 521;
            statusStrip1.Text = "VVMstatusPriceStrip";
            // 
            // CatalogErrorMsg
            // 
            CatalogErrorMsg.Name = "CatalogErrorMsg";
            CatalogErrorMsg.Size = new Size(25, 17);
            CatalogErrorMsg.Text = "      ";
            // 
            // BtnXFactorSave
            // 
            BtnXFactorSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnXFactorSave.Location = new Point(236, 194);
            BtnXFactorSave.Name = "BtnXFactorSave";
            BtnXFactorSave.Size = new Size(91, 23);
            BtnXFactorSave.TabIndex = 519;
            BtnXFactorSave.Text = "Save [F8]";
            BtnXFactorSave.UseVisualStyleBackColor = true;
            BtnXFactorSave.Click += BtnPriceCalculatorSave_Click;
            // 
            // BtnXFactorCancel
            // 
            BtnXFactorCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnXFactorCancel.Location = new Point(142, 194);
            BtnXFactorCancel.Name = "BtnXFactorCancel";
            BtnXFactorCancel.Size = new Size(89, 23);
            BtnXFactorCancel.TabIndex = 520;
            BtnXFactorCancel.Text = "Cancel [Esc]";
            BtnXFactorCancel.UseVisualStyleBackColor = true;
            // 
            // TextBoxXFactorWholeSale
            // 
            TextBoxXFactorWholeSale.BackColor = SystemColors.Window;
            TextBoxXFactorWholeSale.Location = new Point(142, 163);
            TextBoxXFactorWholeSale.MaxLength = 5;
            TextBoxXFactorWholeSale.Name = "TextBoxXFactorWholeSale";
            TextBoxXFactorWholeSale.ReadOnly = true;
            TextBoxXFactorWholeSale.Size = new Size(54, 23);
            TextBoxXFactorWholeSale.TabIndex = 518;
            TextBoxXFactorWholeSale.TextAlign = HorizontalAlignment.Right;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label18.Location = new Point(142, 147);
            label18.Name = "label18";
            label18.Size = new Size(120, 13);
            label18.TabIndex = 517;
            label18.Text = "Whole Sale X-Factor";
            // 
            // TextBoxXFactorRetail
            // 
            TextBoxXFactorRetail.BackColor = SystemColors.Window;
            TextBoxXFactorRetail.Location = new Point(142, 120);
            TextBoxXFactorRetail.MaxLength = 5;
            TextBoxXFactorRetail.Name = "TextBoxXFactorRetail";
            TextBoxXFactorRetail.ReadOnly = true;
            TextBoxXFactorRetail.Size = new Size(54, 23);
            TextBoxXFactorRetail.TabIndex = 516;
            TextBoxXFactorRetail.TextAlign = HorizontalAlignment.Right;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label16.Location = new Point(142, 104);
            label16.Name = "label16";
            label16.Size = new Size(91, 13);
            label16.TabIndex = 515;
            label16.Text = "Retail X-Factor";
            // 
            // TextBoxProductName
            // 
            TextBoxProductName.Location = new Point(18, 29);
            TextBoxProductName.Name = "TextBoxProductName";
            TextBoxProductName.ReadOnly = true;
            TextBoxProductName.Size = new Size(300, 23);
            TextBoxProductName.TabIndex = 512;
            TextBoxProductName.TabStop = false;
            // 
            // TextBoxProductCode
            // 
            TextBoxProductCode.Location = new Point(15, 73);
            TextBoxProductCode.Name = "TextBoxProductCode";
            TextBoxProductCode.ReadOnly = true;
            TextBoxProductCode.Size = new Size(139, 23);
            TextBoxProductCode.TabIndex = 511;
            TextBoxProductCode.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(15, 11);
            label2.Name = "label2";
            label2.Size = new Size(74, 13);
            label2.TabIndex = 514;
            label2.Text = "Product Name";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(12, 57);
            label1.Name = "label1";
            label1.Size = new Size(72, 13);
            label1.TabIndex = 513;
            label1.Text = "Product Code";
            // 
            // ComboBoxProductRetailUOM
            // 
            ComboBoxProductRetailUOM.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxProductRetailUOM.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxProductRetailUOM.FormattingEnabled = true;
            ComboBoxProductRetailUOM.Location = new Point(13, 121);
            ComboBoxProductRetailUOM.MaxLength = 30;
            ComboBoxProductRetailUOM.Name = "ComboBoxProductRetailUOM";
            ComboBoxProductRetailUOM.Size = new Size(93, 23);
            ComboBoxProductRetailUOM.TabIndex = 524;
            ComboBoxProductRetailUOM.TxtVisible = true;
            ComboBoxProductRetailUOM.Visible = false;
            // 
            // ComboBoxProductWholeSaleUOM
            // 
            ComboBoxProductWholeSaleUOM.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxProductWholeSaleUOM.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxProductWholeSaleUOM.FormattingEnabled = true;
            ComboBoxProductWholeSaleUOM.Location = new Point(13, 164);
            ComboBoxProductWholeSaleUOM.MaxLength = 30;
            ComboBoxProductWholeSaleUOM.Name = "ComboBoxProductWholeSaleUOM";
            ComboBoxProductWholeSaleUOM.Size = new Size(113, 23);
            ComboBoxProductWholeSaleUOM.TabIndex = 525;
            ComboBoxProductWholeSaleUOM.TxtVisible = true;
            ComboBoxProductWholeSaleUOM.Visible = false;
            // 
            // label31
            // 
            label31.AutoSize = true;
            label31.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label31.Location = new Point(13, 104);
            label31.Name = "label31";
            label31.Size = new Size(69, 13);
            label31.TabIndex = 522;
            label31.Text = "Retail UOM";
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label29.Location = new Point(13, 147);
            label29.Name = "label29";
            label29.Size = new Size(94, 13);
            label29.TabIndex = 523;
            label29.Text = "Wholesale UOM";
            // 
            // FormModifyXFactor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(335, 249);
            Controls.Add(ComboBoxProductWholeSaleUOM);
            Controls.Add(ComboBoxProductRetailUOM);
            Controls.Add(label31);
            Controls.Add(label29);
            Controls.Add(statusStrip1);
            Controls.Add(BtnXFactorSave);
            Controls.Add(BtnXFactorCancel);
            Controls.Add(TextBoxXFactorWholeSale);
            Controls.Add(label18);
            Controls.Add(TextBoxXFactorRetail);
            Controls.Add(label16);
            Controls.Add(TextBoxProductName);
            Controls.Add(TextBoxProductCode);
            Controls.Add(label2);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormModifyXFactor";
            Text = "UOM and X Factor Modifier";
            Load += FormModifyXFactor_Load;
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(label2, 0);
            Controls.SetChildIndex(TextBoxProductCode, 0);
            Controls.SetChildIndex(TextBoxProductName, 0);
            Controls.SetChildIndex(label16, 0);
            Controls.SetChildIndex(TextBoxXFactorRetail, 0);
            Controls.SetChildIndex(label18, 0);
            Controls.SetChildIndex(TextBoxXFactorWholeSale, 0);
            Controls.SetChildIndex(BtnXFactorCancel, 0);
            Controls.SetChildIndex(BtnXFactorSave, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(label29, 0);
            Controls.SetChildIndex(label31, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(checkBoxIsPatient, 0);
            Controls.SetChildIndex(ComboBoxProductRetailUOM, 0);
            Controls.SetChildIndex(ComboBoxProductWholeSaleUOM, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusStrip1;
        private ToolStripStatusLabel CatalogErrorMsg;
        private Button BtnXFactorSave;
        private Button BtnXFactorCancel;
        private TextBox TextBoxXFactorWholeSale;
        private Label label18;
        private TextBox TextBoxXFactorRetail;
        private Label label16;
        private TextBox TextBoxProductName;
        private TextBox TextBoxProductCode;
        private Label label2;
        private Label label1;
        private fa.views.controls.ComboBoxSwapTextBox ComboBoxProductRetailUOM;
        private fa.views.controls.ComboBoxSwapTextBox ComboBoxProductWholeSaleUOM;
        private Label label31;
        private Label label29;
    }
}