namespace fa.views.catalog
{
    partial class FormCatalogBarCodePrint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCatalogBarCodePrint));
            label1 = new Label();
            label2 = new Label();
            TextBoxPrintQuantity = new TextBox();
            LabelStartLocation = new Label();
            BtnCancel = new Button();
            BtnPrint = new Button();
            Print = new StatusStrip();
            PrintErrorMsg = new ToolStripStatusLabel();
            TextBoxStartLocation = new fa.views.controls.text.UserControlPoint();
            groupBox2 = new GroupBox();
            ComboBoxDefaultPrinter = new fa.views.controls.ComboBoxSwapTextBox();
            label4 = new Label();
            TextBoxXFactorWholeSale = new TextBox();
            TextBoxXFactorRetail = new TextBox();
            label9 = new Label();
            label3 = new Label();
            ComboBoxLabelSize = new ComboBox();
            LabelLabelSize = new Label();
            YesNoRadioPaperSize = new fa.views.controls.YesNoRadio();
            Print.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(22, 209);
            ProductIdTransport.Size = new Size(116, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(20, 192);
            ProductBatchIdTransport.Size = new Size(116, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(20, 192);
            AccountIdTransport.Size = new Size(116, 21);
            // 
            // checkBoxIsPatient
            // 
            checkBoxIsPatient.Size = new Size(69, 17);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(14, 15);
            label1.Name = "label1";
            label1.Size = new Size(56, 13);
            label1.TabIndex = 0;
            label1.Text = "Quantity";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(14, 61);
            label2.Name = "label2";
            label2.Size = new Size(66, 13);
            label2.TabIndex = 1;
            label2.Text = "Paper Size";
            // 
            // TextBoxPrintQuantity
            // 
            TextBoxPrintQuantity.Location = new Point(17, 32);
            TextBoxPrintQuantity.MaxLength = 3;
            TextBoxPrintQuantity.Name = "TextBoxPrintQuantity";
            TextBoxPrintQuantity.Size = new Size(100, 21);
            TextBoxPrintQuantity.TabIndex = 0;
            TextBoxPrintQuantity.KeyPress += TextBoxPrintQuantity_KeyPress;
            // 
            // LabelStartLocation
            // 
            LabelStartLocation.AutoSize = true;
            LabelStartLocation.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelStartLocation.Location = new Point(14, 107);
            LabelStartLocation.Name = "LabelStartLocation";
            LabelStartLocation.Size = new Size(87, 13);
            LabelStartLocation.TabIndex = 5;
            LabelStartLocation.Text = "Start Location";
            LabelStartLocation.Visible = false;
            // 
            // BtnCancel
            // 
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(113, 207);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(83, 23);
            BtnCancel.TabIndex = 6;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // BtnPrint
            // 
            BtnPrint.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPrint.Location = new Point(202, 207);
            BtnPrint.Name = "BtnPrint";
            BtnPrint.Size = new Size(83, 23);
            BtnPrint.TabIndex = 5;
            BtnPrint.Text = "Print [F9]";
            BtnPrint.UseVisualStyleBackColor = true;
            BtnPrint.Click += BtnPrint_Click;
            // 
            // Print
            // 
            Print.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Print.Items.AddRange(new ToolStripItem[] { PrintErrorMsg });
            Print.Location = new Point(0, 239);
            Print.Name = "Print";
            Print.Size = new Size(316, 22);
            Print.TabIndex = 10;
            // 
            // PrintErrorMsg
            // 
            PrintErrorMsg.Name = "PrintErrorMsg";
            PrintErrorMsg.Size = new Size(31, 17);
            PrintErrorMsg.Text = "        ";
            // 
            // TextBoxStartLocation
            // 
            TextBoxStartLocation.BackColor = SystemColors.Window;
            TextBoxStartLocation.BorderStyle = BorderStyle.FixedSingle;
            TextBoxStartLocation.Location = new Point(17, 124);
            TextBoxStartLocation.Margin = new Padding(4, 3, 4, 3);
            TextBoxStartLocation.Name = "TextBoxStartLocation";
            TextBoxStartLocation.NoOfColoumns = 3;
            TextBoxStartLocation.NoOfRow = 7;
            TextBoxStartLocation.Size = new Size(136, 21);
            TextBoxStartLocation.TabIndex = 2;
            TextBoxStartLocation.Visible = false;
            // 
            // groupBox2
            // 
            groupBox2.BackColor = SystemColors.Window;
            groupBox2.Controls.Add(ComboBoxDefaultPrinter);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(TextBoxXFactorWholeSale);
            groupBox2.Controls.Add(TextBoxXFactorRetail);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(ComboBoxLabelSize);
            groupBox2.Controls.Add(LabelLabelSize);
            groupBox2.Controls.Add(YesNoRadioPaperSize);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(TextBoxPrintQuantity);
            groupBox2.Controls.Add(TextBoxStartLocation);
            groupBox2.Controls.Add(LabelStartLocation);
            groupBox2.Controls.Add(label1);
            groupBox2.Location = new Point(8, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(302, 200);
            groupBox2.TabIndex = 12;
            groupBox2.TabStop = false;
            // 
            // ComboBoxDefaultPrinter
            // 
            ComboBoxDefaultPrinter.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxDefaultPrinter.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxDefaultPrinter.FormattingEnabled = true;
            ComboBoxDefaultPrinter.Location = new Point(14, 168);
            ComboBoxDefaultPrinter.MaxLength = 30;
            ComboBoxDefaultPrinter.Name = "ComboBoxDefaultPrinter";
            ComboBoxDefaultPrinter.Size = new Size(215, 21);
            ComboBoxDefaultPrinter.TabIndex = 4;
            ComboBoxDefaultPrinter.TxtVisible = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(186, 61);
            label4.Name = "label4";
            label4.Size = new Size(81, 13);
            label4.TabIndex = 474;
            label4.Text = "Todays Label";
            // 
            // TextBoxXFactorWholeSale
            // 
            TextBoxXFactorWholeSale.BackColor = SystemColors.Window;
            TextBoxXFactorWholeSale.Location = new Point(186, 82);
            TextBoxXFactorWholeSale.MaxLength = 5;
            TextBoxXFactorWholeSale.Name = "TextBoxXFactorWholeSale";
            TextBoxXFactorWholeSale.ReadOnly = true;
            TextBoxXFactorWholeSale.Size = new Size(110, 21);
            TextBoxXFactorWholeSale.TabIndex = 473;
            TextBoxXFactorWholeSale.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxXFactorRetail
            // 
            TextBoxXFactorRetail.BackColor = SystemColors.Window;
            TextBoxXFactorRetail.Location = new Point(186, 32);
            TextBoxXFactorRetail.MaxLength = 5;
            TextBoxXFactorRetail.Name = "TextBoxXFactorRetail";
            TextBoxXFactorRetail.ReadOnly = true;
            TextBoxXFactorRetail.Size = new Size(110, 21);
            TextBoxXFactorRetail.TabIndex = 472;
            TextBoxXFactorRetail.TextAlign = HorizontalAlignment.Right;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label9.Location = new Point(186, 15);
            label9.Name = "label9";
            label9.Size = new Size(84, 13);
            label9.TabIndex = 471;
            label9.Text = "Balance Label";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(14, 151);
            label3.Name = "label3";
            label3.Size = new Size(90, 13);
            label3.TabIndex = 15;
            label3.Text = "Choose Printer";
            // 
            // ComboBoxLabelSize
            // 
            ComboBoxLabelSize.FormattingEnabled = true;
            ComboBoxLabelSize.Items.AddRange(new object[] { "25 mm * 20 mm", "35 mm * 25 mm", "50 mm * 25 mm", "100 mm* 23 mm" });
            ComboBoxLabelSize.Location = new Point(186, 124);
            ComboBoxLabelSize.Name = "ComboBoxLabelSize";
            ComboBoxLabelSize.Size = new Size(110, 21);
            ComboBoxLabelSize.TabIndex = 3;
            ComboBoxLabelSize.Visible = false;
            // 
            // LabelLabelSize
            // 
            LabelLabelSize.AutoSize = true;
            LabelLabelSize.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelLabelSize.Location = new Point(186, 107);
            LabelLabelSize.Name = "LabelLabelSize";
            LabelLabelSize.Size = new Size(63, 13);
            LabelLabelSize.TabIndex = 13;
            LabelLabelSize.Text = "Label Size";
            LabelLabelSize.Visible = false;
            // 
            // YesNoRadioPaperSize
            // 
            YesNoRadioPaperSize.BackColor = SystemColors.Window;
            YesNoRadioPaperSize.Checked = false;
            YesNoRadioPaperSize.FirstButtonName = "Label";
            YesNoRadioPaperSize.Location = new Point(12, 81);
            YesNoRadioPaperSize.Margin = new Padding(4, 3, 4, 3);
            YesNoRadioPaperSize.Name = "YesNoRadioPaperSize";
            YesNoRadioPaperSize.SecondButtonName = "A4";
            YesNoRadioPaperSize.Size = new Size(139, 23);
            YesNoRadioPaperSize.TabIndex = 1;
            YesNoRadioPaperSize.Load += YesNoRadioPaperSize_Load;
            // 
            // FormCatalogBarCodePrint
            // 
            AcceptButton = BtnPrint;
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            CancelButton = BtnCancel;
            ClientSize = new Size(316, 261);
            Controls.Add(BtnCancel);
            Controls.Add(groupBox2);
            Controls.Add(Print);
            Controls.Add(BtnPrint);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormCatalogBarCodePrint";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Print";
            Load += FormCatalogBarCodePrint_Load;
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(checkBoxIsPatient, 0);
            Controls.SetChildIndex(BtnPrint, 0);
            Controls.SetChildIndex(Print, 0);
            Controls.SetChildIndex(groupBox2, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(BtnCancel, 0);
            Print.ResumeLayout(false);
            Print.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox TextBoxPrintQuantity;
        private Label LabelStartLocation;
        private Button BtnCancel;
        private Button BtnPrint;
        private StatusStrip Print;
        private ToolStripStatusLabel PrintErrorMsg;
        private controls.text.UserControlPoint TextBoxStartLocation;
        private GroupBox groupBox2;
        private controls.YesNoRadio YesNoRadioPaperSize;
        private Label LabelLabelSize;
        private ComboBox ComboBoxLabelSize;
        private controls.ComboBoxSwapTextBox ComboBoxDefaultPrinter;
        private Label label3;
        private Label label4;
        private TextBox TextBoxXFactorWholeSale;
        private TextBox TextBoxXFactorRetail;
        private Label label9;
    }
}