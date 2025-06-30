namespace fa.views.sales
{
    partial class PriceOverride
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PriceOverride));
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TextBoxOverridePassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.BtnOverride = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.TextBoxOverridePrice = new fa.views.controls.text.CurrencyTextBox();
            this.TextBoxOverrideUser = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ComboBoxProductWholeSaleUOM = new fa.views.controls.ComboBoxSwapTextBox();
            this.ComboBoxProductRetailUOM = new fa.views.controls.ComboBoxSwapTextBox();
            this.ComboBoxProductPurchesUOM = new fa.views.controls.ComboBoxSwapTextBox();
            this.ItemTaxDetails = new fa.views.controls.accounting.ItemTaxDetails();
            this.label29 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.TextBoxPurchaseEntryMsrp = new fa.views.controls.text.CurrencyTextBox();
            this.TextBoxPurchaseEntryProductName = new System.Windows.Forms.TextBox();
            this.TextBoxPurchaseEntryWholeSalePrice = new fa.views.controls.text.CurrencyTextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.TextBoxPurchaseEntryRetailPrice = new fa.views.controls.text.CurrencyTextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.TextBoxProductPurchasePrice = new fa.views.controls.text.CurrencyTextBox();
            this.label39 = new System.Windows.Forms.Label();
            this.TextBoxProductXFactorWholeSale = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.TextBoxProductXFactorRetail = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.TextBoxPurchaseEntryMaterialId = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ErrorMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.TextBoxPrice = new fa.views.controls.text.CurrencyTextBox();
            this.itemTaxDetails1 = new fa.views.controls.accounting.ItemTaxDetails();
            this.itemTaxDetails2 = new fa.views.controls.accounting.ItemTaxDetails();
            this.itemTaxDetails3 = new fa.views.controls.accounting.ItemTaxDetails();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProductIdTransport
            // 
            this.ProductIdTransport.Location = new System.Drawing.Point(12, 436);
            this.ProductIdTransport.Size = new System.Drawing.Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            this.ProductBatchIdTransport.Location = new System.Drawing.Point(12, 462);
            this.ProductBatchIdTransport.Size = new System.Drawing.Size(100, 21);
            // 
            // AccountIdTransport
            // 
            this.AccountIdTransport.Location = new System.Drawing.Point(172, 163);
            this.AccountIdTransport.Size = new System.Drawing.Size(100, 21);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Override Price";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Override User";
            // 
            // TextBoxOverridePassword
            // 
            this.TextBoxOverridePassword.Location = new System.Drawing.Point(12, 112);
            this.TextBoxOverridePassword.Name = "TextBoxOverridePassword";
            this.TextBoxOverridePassword.Size = new System.Drawing.Size(134, 21);
            this.TextBoxOverridePassword.TabIndex = 2;
            this.TextBoxOverridePassword.UseSystemPasswordChar = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Password";
            // 
            // BtnOverride
            // 
            this.BtnOverride.Location = new System.Drawing.Point(154, 526);
            this.BtnOverride.Name = "BtnOverride";
            this.BtnOverride.Size = new System.Drawing.Size(75, 23);
            this.BtnOverride.TabIndex = 3;
            this.BtnOverride.Text = "Override";
            this.BtnOverride.UseVisualStyleBackColor = true;
            this.BtnOverride.Click += new System.EventHandler(this.BtnOverride_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(12, 526);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(136, 23);
            this.BtnCancel.TabIndex = 4;
            this.BtnCancel.Text = "Cancel Override [Esc]";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // TextBoxOverridePrice
            // 
            this.TextBoxOverridePrice.Decimals = 2;
            this.TextBoxOverridePrice.Length = 10;
            this.TextBoxOverridePrice.Location = new System.Drawing.Point(12, 34);
            this.TextBoxOverridePrice.Name = "TextBoxOverridePrice";
            this.TextBoxOverridePrice.Size = new System.Drawing.Size(100, 21);
            this.TextBoxOverridePrice.TabIndex = 0;
            this.TextBoxOverridePrice.Text = "0.00";
            this.TextBoxOverridePrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // TextBoxOverrideUser
            // 
            this.TextBoxOverrideUser.Location = new System.Drawing.Point(12, 73);
            this.TextBoxOverrideUser.Name = "TextBoxOverrideUser";
            this.TextBoxOverrideUser.Size = new System.Drawing.Size(134, 21);
            this.TextBoxOverrideUser.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ComboBoxProductWholeSaleUOM);
            this.groupBox2.Controls.Add(this.ComboBoxProductRetailUOM);
            this.groupBox2.Controls.Add(this.ComboBoxProductPurchesUOM);
            this.groupBox2.Controls.Add(this.ItemTaxDetails);
            this.groupBox2.Controls.Add(this.label29);
            this.groupBox2.Controls.Add(this.label24);
            this.groupBox2.Controls.Add(this.TextBoxPurchaseEntryMsrp);
            this.groupBox2.Controls.Add(this.TextBoxPurchaseEntryProductName);
            this.groupBox2.Controls.Add(this.TextBoxPurchaseEntryWholeSalePrice);
            this.groupBox2.Controls.Add(this.label35);
            this.groupBox2.Controls.Add(this.TextBoxPurchaseEntryRetailPrice);
            this.groupBox2.Controls.Add(this.label34);
            this.groupBox2.Controls.Add(this.TextBoxProductPurchasePrice);
            this.groupBox2.Controls.Add(this.label39);
            this.groupBox2.Controls.Add(this.TextBoxProductXFactorWholeSale);
            this.groupBox2.Controls.Add(this.label32);
            this.groupBox2.Controls.Add(this.TextBoxProductXFactorRetail);
            this.groupBox2.Controls.Add(this.label28);
            this.groupBox2.Controls.Add(this.label31);
            this.groupBox2.Controls.Add(this.label30);
            this.groupBox2.Controls.Add(this.TextBoxPurchaseEntryMaterialId);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(245, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(231, 539);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Product Details";
            // 
            // ComboBoxProductWholeSaleUOM
            // 
            this.ComboBoxProductWholeSaleUOM.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ComboBoxProductWholeSaleUOM.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboBoxProductWholeSaleUOM.FormattingEnabled = true;
            this.ComboBoxProductWholeSaleUOM.Location = new System.Drawing.Point(9, 209);
            this.ComboBoxProductWholeSaleUOM.MaxLength = 30;
            this.ComboBoxProductWholeSaleUOM.Name = "ComboBoxProductWholeSaleUOM";
            this.ComboBoxProductWholeSaleUOM.Size = new System.Drawing.Size(108, 21);
            this.ComboBoxProductWholeSaleUOM.TabIndex = 16;
            this.ComboBoxProductWholeSaleUOM.TabStop = false;
            this.ComboBoxProductWholeSaleUOM.TxtVisible = true;
            // 
            // ComboBoxProductRetailUOM
            // 
            this.ComboBoxProductRetailUOM.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ComboBoxProductRetailUOM.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboBoxProductRetailUOM.FormattingEnabled = true;
            this.ComboBoxProductRetailUOM.Location = new System.Drawing.Point(8, 165);
            this.ComboBoxProductRetailUOM.MaxLength = 30;
            this.ComboBoxProductRetailUOM.Name = "ComboBoxProductRetailUOM";
            this.ComboBoxProductRetailUOM.Size = new System.Drawing.Size(108, 21);
            this.ComboBoxProductRetailUOM.TabIndex = 14;
            this.ComboBoxProductRetailUOM.TabStop = false;
            this.ComboBoxProductRetailUOM.TxtVisible = true;
            // 
            // ComboBoxProductPurchesUOM
            // 
            this.ComboBoxProductPurchesUOM.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ComboBoxProductPurchesUOM.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboBoxProductPurchesUOM.FormattingEnabled = true;
            this.ComboBoxProductPurchesUOM.Location = new System.Drawing.Point(9, 122);
            this.ComboBoxProductPurchesUOM.MaxLength = 30;
            this.ComboBoxProductPurchesUOM.Name = "ComboBoxProductPurchesUOM";
            this.ComboBoxProductPurchesUOM.Size = new System.Drawing.Size(107, 21);
            this.ComboBoxProductPurchesUOM.TabIndex = 43;
            this.ComboBoxProductPurchesUOM.TabStop = false;
            this.ComboBoxProductPurchesUOM.TxtVisible = true;
            // 
            // ItemTaxDetails
            // 
            this.ItemTaxDetails.Enabled = false;
            this.ItemTaxDetails.EnableEdit = false;
            this.ItemTaxDetails.Location = new System.Drawing.Point(9, 329);
            this.ItemTaxDetails.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ItemTaxDetails.Name = "ItemTaxDetails";
            this.ItemTaxDetails.ProductId = ((long)(0));
            this.ItemTaxDetails.Size = new System.Drawing.Size(224, 201);
            this.ItemTaxDetails.TabIndex = 17;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label29.Location = new System.Drawing.Point(7, 191);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(94, 13);
            this.label29.TabIndex = 50;
            this.label29.Text = "Wholesale UOM";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(7, 329);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(60, 13);
            this.label24.TabIndex = 76;
            this.label24.Text = "Tax Details";
            // 
            // TextBoxPurchaseEntryMsrp
            // 
            this.TextBoxPurchaseEntryMsrp.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxPurchaseEntryMsrp.Decimals = 2;
            this.TextBoxPurchaseEntryMsrp.Length = 10;
            this.TextBoxPurchaseEntryMsrp.Location = new System.Drawing.Point(99, 296);
            this.TextBoxPurchaseEntryMsrp.Name = "TextBoxPurchaseEntryMsrp";
            this.TextBoxPurchaseEntryMsrp.ReadOnly = true;
            this.TextBoxPurchaseEntryMsrp.Size = new System.Drawing.Size(71, 21);
            this.TextBoxPurchaseEntryMsrp.TabIndex = 21;
            this.TextBoxPurchaseEntryMsrp.TabStop = false;
            this.TextBoxPurchaseEntryMsrp.Text = "0.00";
            this.TextBoxPurchaseEntryMsrp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // TextBoxPurchaseEntryProductName
            // 
            this.TextBoxPurchaseEntryProductName.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxPurchaseEntryProductName.Location = new System.Drawing.Point(9, 78);
            this.TextBoxPurchaseEntryProductName.Name = "TextBoxPurchaseEntryProductName";
            this.TextBoxPurchaseEntryProductName.ReadOnly = true;
            this.TextBoxPurchaseEntryProductName.Size = new System.Drawing.Size(215, 21);
            this.TextBoxPurchaseEntryProductName.TabIndex = 13;
            this.TextBoxPurchaseEntryProductName.TabStop = false;
            // 
            // TextBoxPurchaseEntryWholeSalePrice
            // 
            this.TextBoxPurchaseEntryWholeSalePrice.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxPurchaseEntryWholeSalePrice.Decimals = 2;
            this.TextBoxPurchaseEntryWholeSalePrice.Length = 10;
            this.TextBoxPurchaseEntryWholeSalePrice.Location = new System.Drawing.Point(10, 296);
            this.TextBoxPurchaseEntryWholeSalePrice.Name = "TextBoxPurchaseEntryWholeSalePrice";
            this.TextBoxPurchaseEntryWholeSalePrice.ReadOnly = true;
            this.TextBoxPurchaseEntryWholeSalePrice.Size = new System.Drawing.Size(70, 21);
            this.TextBoxPurchaseEntryWholeSalePrice.TabIndex = 20;
            this.TextBoxPurchaseEntryWholeSalePrice.TabStop = false;
            this.TextBoxPurchaseEntryWholeSalePrice.Text = "0.00";
            this.TextBoxPurchaseEntryWholeSalePrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(6, 279);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(82, 13);
            this.label35.TabIndex = 71;
            this.label35.Text = "Wholesale Price";
            // 
            // TextBoxPurchaseEntryRetailPrice
            // 
            this.TextBoxPurchaseEntryRetailPrice.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxPurchaseEntryRetailPrice.Decimals = 2;
            this.TextBoxPurchaseEntryRetailPrice.Length = 10;
            this.TextBoxPurchaseEntryRetailPrice.Location = new System.Drawing.Point(99, 254);
            this.TextBoxPurchaseEntryRetailPrice.Name = "TextBoxPurchaseEntryRetailPrice";
            this.TextBoxPurchaseEntryRetailPrice.ReadOnly = true;
            this.TextBoxPurchaseEntryRetailPrice.Size = new System.Drawing.Size(70, 21);
            this.TextBoxPurchaseEntryRetailPrice.TabIndex = 19;
            this.TextBoxPurchaseEntryRetailPrice.TabStop = false;
            this.TextBoxPurchaseEntryRetailPrice.Text = "0.00";
            this.TextBoxPurchaseEntryRetailPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(96, 236);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(60, 13);
            this.label34.TabIndex = 69;
            this.label34.Text = "Retail Price";
            // 
            // TextBoxProductPurchasePrice
            // 
            this.TextBoxProductPurchasePrice.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxProductPurchasePrice.Decimals = 2;
            this.TextBoxProductPurchasePrice.Length = 10;
            this.TextBoxProductPurchasePrice.Location = new System.Drawing.Point(9, 254);
            this.TextBoxProductPurchasePrice.Name = "TextBoxProductPurchasePrice";
            this.TextBoxProductPurchasePrice.ReadOnly = true;
            this.TextBoxProductPurchasePrice.Size = new System.Drawing.Size(70, 21);
            this.TextBoxProductPurchasePrice.TabIndex = 18;
            this.TextBoxProductPurchasePrice.TabStop = false;
            this.TextBoxProductPurchasePrice.Text = "0.00";
            this.TextBoxProductPurchasePrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(6, 236);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(77, 13);
            this.label39.TabIndex = 65;
            this.label39.Text = "Purchase Price";
            // 
            // TextBoxProductXFactorWholeSale
            // 
            this.TextBoxProductXFactorWholeSale.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxProductXFactorWholeSale.Location = new System.Drawing.Point(126, 209);
            this.TextBoxProductXFactorWholeSale.MaxLength = 5;
            this.TextBoxProductXFactorWholeSale.Name = "TextBoxProductXFactorWholeSale";
            this.TextBoxProductXFactorWholeSale.ReadOnly = true;
            this.TextBoxProductXFactorWholeSale.Size = new System.Drawing.Size(52, 21);
            this.TextBoxProductXFactorWholeSale.TabIndex = 17;
            this.TextBoxProductXFactorWholeSale.TabStop = false;
            this.TextBoxProductXFactorWholeSale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label32.Location = new System.Drawing.Point(123, 191);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(55, 13);
            this.label32.TabIndex = 55;
            this.label32.Text = "X-Factor";
            // 
            // TextBoxProductXFactorRetail
            // 
            this.TextBoxProductXFactorRetail.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxProductXFactorRetail.Location = new System.Drawing.Point(126, 164);
            this.TextBoxProductXFactorRetail.MaxLength = 5;
            this.TextBoxProductXFactorRetail.Name = "TextBoxProductXFactorRetail";
            this.TextBoxProductXFactorRetail.ReadOnly = true;
            this.TextBoxProductXFactorRetail.Size = new System.Drawing.Size(53, 21);
            this.TextBoxProductXFactorRetail.TabIndex = 15;
            this.TextBoxProductXFactorRetail.TabStop = false;
            this.TextBoxProductXFactorRetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label28.Location = new System.Drawing.Point(124, 144);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(55, 13);
            this.label28.TabIndex = 53;
            this.label28.Text = "X-Factor";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label31.Location = new System.Drawing.Point(7, 146);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(69, 13);
            this.label31.TabIndex = 47;
            this.label31.Text = "Retail UOM";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label30.Location = new System.Drawing.Point(7, 103);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(88, 13);
            this.label30.TabIndex = 44;
            this.label30.Text = "Purchase UOM";
            // 
            // TextBoxPurchaseEntryMaterialId
            // 
            this.TextBoxPurchaseEntryMaterialId.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxPurchaseEntryMaterialId.Location = new System.Drawing.Point(9, 38);
            this.TextBoxPurchaseEntryMaterialId.Name = "TextBoxPurchaseEntryMaterialId";
            this.TextBoxPurchaseEntryMaterialId.ReadOnly = true;
            this.TextBoxPurchaseEntryMaterialId.Size = new System.Drawing.Size(215, 21);
            this.TextBoxPurchaseEntryMaterialId.TabIndex = 3;
            this.TextBoxPurchaseEntryMaterialId.TabStop = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(96, 280);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(34, 13);
            this.label14.TabIndex = 8;
            this.label14.Text = "MSRP";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 62);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(34, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Id";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ErrorMsg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 551);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(493, 22);
            this.statusStrip1.TabIndex = 15;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            this.ErrorMsg.Name = "ErrorMsg";
            this.ErrorMsg.Size = new System.Drawing.Size(61, 17);
            this.ErrorMsg.Text = "                  ";
            // 
            // TextBoxPrice
            // 
            this.TextBoxPrice.Decimals = 2;
            this.TextBoxPrice.Length = 10;
            this.TextBoxPrice.Location = new System.Drawing.Point(118, 34);
            this.TextBoxPrice.Name = "TextBoxPrice";
            this.TextBoxPrice.Size = new System.Drawing.Size(100, 21);
            this.TextBoxPrice.TabIndex = 16;
            this.TextBoxPrice.Text = "0.00";
            this.TextBoxPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TextBoxPrice.Visible = false;
            // 
            // itemTaxDetails1
            // 
            this.itemTaxDetails1.Enabled = false;
            this.itemTaxDetails1.EnableEdit = false;
            this.itemTaxDetails1.Location = new System.Drawing.Point(0, 0);
            this.itemTaxDetails1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.itemTaxDetails1.Name = "itemTaxDetails1";
            this.itemTaxDetails1.ProductId = ((long)(0));
            this.itemTaxDetails1.Size = new System.Drawing.Size(224, 201);
            this.itemTaxDetails1.TabIndex = 0;
            // 
            // itemTaxDetails2
            // 
            this.itemTaxDetails2.Enabled = false;
            this.itemTaxDetails2.EnableEdit = false;
            this.itemTaxDetails2.Location = new System.Drawing.Point(0, 0);
            this.itemTaxDetails2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.itemTaxDetails2.Name = "itemTaxDetails2";
            this.itemTaxDetails2.ProductId = ((long)(0));
            this.itemTaxDetails2.Size = new System.Drawing.Size(224, 201);
            this.itemTaxDetails2.TabIndex = 0;
            // 
            // itemTaxDetails3
            // 
            this.itemTaxDetails3.Enabled = false;
            this.itemTaxDetails3.EnableEdit = false;
            this.itemTaxDetails3.Location = new System.Drawing.Point(0, 0);
            this.itemTaxDetails3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.itemTaxDetails3.Name = "itemTaxDetails3";
            this.itemTaxDetails3.ProductId = ((long)(0));
            this.itemTaxDetails3.Size = new System.Drawing.Size(224, 201);
            this.itemTaxDetails3.TabIndex = 0;
            // 
            // PriceOverride
            // 
            this.AcceptButton = this.BtnOverride;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnCancel;
            this.ClientSize = new System.Drawing.Size(493, 573);
            this.Controls.Add(this.TextBoxPrice);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.TextBoxOverridePrice);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOverride);
            this.Controls.Add(this.TextBoxOverridePassword);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TextBoxOverrideUser);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PriceOverride";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Price Override";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PriceOverride_FormClosing);
            this.Load += new System.EventHandler(this.PriceOverride_Load);
            this.Controls.SetChildIndex(this.AccountIdTransport, 0);
            this.Controls.SetChildIndex(this.ProductBatchIdTransport, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.TextBoxOverrideUser, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.TextBoxOverridePassword, 0);
            this.Controls.SetChildIndex(this.BtnOverride, 0);
            this.Controls.SetChildIndex(this.BtnCancel, 0);
            this.Controls.SetChildIndex(this.TextBoxOverridePrice, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.Controls.SetChildIndex(this.ProductIdTransport, 0);
            this.Controls.SetChildIndex(this.statusStrip1, 0);
            this.Controls.SetChildIndex(this.TextBoxPrice, 0);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextBoxOverridePassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button BtnOverride;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.TextBox TextBoxOverrideUser;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label24;
        private controls.text.CurrencyTextBox TextBoxPurchaseEntryMsrp;
        private System.Windows.Forms.TextBox TextBoxPurchaseEntryProductName;
        private controls.text.CurrencyTextBox TextBoxPurchaseEntryWholeSalePrice;
        private System.Windows.Forms.Label label35;
        private controls.text.CurrencyTextBox TextBoxPurchaseEntryRetailPrice;
        private System.Windows.Forms.Label label34;
        private controls.text.CurrencyTextBox TextBoxProductPurchasePrice;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.TextBox TextBoxProductXFactorWholeSale;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TextBox TextBoxProductXFactorRetail;
        private System.Windows.Forms.Label label28;
        private controls.ComboBoxSwapTextBox ComboBoxProductWholeSaleUOM;
        private controls.ComboBoxSwapTextBox ComboBoxProductRetailUOM;
        private System.Windows.Forms.Label label31;
        private controls.ComboBoxSwapTextBox ComboBoxProductPurchesUOM;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TextBox TextBoxPurchaseEntryMaterialId;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label6;
        public controls.text.CurrencyTextBox TextBoxOverridePrice;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ErrorMsg;
        public controls.text.CurrencyTextBox TextBoxPrice;
        private controls.accounting.ItemTaxDetails itemTaxDetails1;
        private controls.accounting.ItemTaxDetails itemTaxDetails2;
        private controls.accounting.ItemTaxDetails itemTaxDetails3;
        private controls.accounting.ItemTaxDetails ItemTaxDetails;
    }
}