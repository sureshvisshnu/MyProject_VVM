namespace fa.views.catalog
{
    partial class FormCatalogQRCodePrint
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCatalogQRCodePrint));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ComboBoxDefaultPrinter = new fa.views.controls.ComboBoxSwapTextBox();
            this.LabelChoosePrinter = new System.Windows.Forms.Label();
            this.BatchQRCode = new fa.views.controls.QRCodeControl();
            this.ComboBoxQRCodeSize = new System.Windows.Forms.ComboBox();
            this.LabelQRCodeSize = new System.Windows.Forms.Label();
            this.ComboBoxLabelSize = new System.Windows.Forms.ComboBox();
            this.LabelLabelSize = new System.Windows.Forms.Label();
            this.YesNoRadioPaperSize = new fa.views.controls.YesNoRadio();
            this.label2 = new System.Windows.Forms.Label();
            this.TextBoxPrintQuantity = new System.Windows.Forms.TextBox();
            this.TextBoxStartLocation = new fa.views.controls.text.UserControlPoint();
            this.Row = new fa.views.controls.text.NumberTextBox(this.components);
            this.Separator = new fa.views.controls.text.NumberTextBox(this.components);
            this.Column = new fa.views.controls.text.NumberTextBox(this.components);
            this.LabelStartLocation = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Print = new System.Windows.Forms.StatusStrip();
            this.PrintErrorMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.BtnPrint = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.TextBoxStartLocation.SuspendLayout();
            this.Print.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox2.Controls.Add(this.ComboBoxDefaultPrinter);
            this.groupBox2.Controls.Add(this.LabelChoosePrinter);
            this.groupBox2.Controls.Add(this.BatchQRCode);
            this.groupBox2.Controls.Add(this.ComboBoxQRCodeSize);
            this.groupBox2.Controls.Add(this.LabelQRCodeSize);
            this.groupBox2.Controls.Add(this.ComboBoxLabelSize);
            this.groupBox2.Controls.Add(this.LabelLabelSize);
            this.groupBox2.Controls.Add(this.YesNoRadioPaperSize);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.TextBoxPrintQuantity);
            this.groupBox2.Controls.Add(this.TextBoxStartLocation);
            this.groupBox2.Controls.Add(this.LabelStartLocation);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(8, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(352, 229);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            // 
            // ComboBoxDefaultPrinter
            // 
            this.ComboBoxDefaultPrinter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ComboBoxDefaultPrinter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboBoxDefaultPrinter.FormattingEnabled = true;
            this.ComboBoxDefaultPrinter.Location = new System.Drawing.Point(17, 197);
            this.ComboBoxDefaultPrinter.MaxLength = 30;
            this.ComboBoxDefaultPrinter.Name = "ComboBoxDefaultPrinter";
            this.ComboBoxDefaultPrinter.Size = new System.Drawing.Size(215, 21);
            this.ComboBoxDefaultPrinter.TabIndex = 17;
            this.ComboBoxDefaultPrinter.TxtVisible = true;
            // 
            // LabelChoosePrinter
            // 
            this.LabelChoosePrinter.AutoSize = true;
            this.LabelChoosePrinter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LabelChoosePrinter.Location = new System.Drawing.Point(17, 180);
            this.LabelChoosePrinter.Name = "LabelChoosePrinter";
            this.LabelChoosePrinter.Size = new System.Drawing.Size(90, 13);
            this.LabelChoosePrinter.TabIndex = 18;
            this.LabelChoosePrinter.Text = "Choose Printer";
            // 
            // BatchQRCode
            // 
            this.BatchQRCode.Caption = "QR Code";
            this.BatchQRCode.Location = new System.Drawing.Point(236, 31);
            this.BatchQRCode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BatchQRCode.Name = "BatchQRCode";
            this.BatchQRCode.Size = new System.Drawing.Size(91, 94);
            this.BatchQRCode.TabIndex = 16;
            this.BatchQRCode.TabStop = false;
            // 
            // ComboBoxQRCodeSize
            // 
            this.ComboBoxQRCodeSize.FormattingEnabled = true;
            this.ComboBoxQRCodeSize.Items.AddRange(new object[] {
            "1\" X 1\"",
            "1.5\" X 1.5\"",
            "2\" X 2\""});
            this.ComboBoxQRCodeSize.Location = new System.Drawing.Point(17, 113);
            this.ComboBoxQRCodeSize.Name = "ComboBoxQRCodeSize";
            this.ComboBoxQRCodeSize.Size = new System.Drawing.Size(136, 21);
            this.ComboBoxQRCodeSize.TabIndex = 2;
            this.ComboBoxQRCodeSize.SelectedIndexChanged += new System.EventHandler(this.ComboBoxQRCodeSize_SelectedIndexChanged);
            this.ComboBoxQRCodeSize.TextChanged += new System.EventHandler(this.ComboBoxQRCodeSize_SelectedIndexChanged);
            // 
            // LabelQRCodeSize
            // 
            this.LabelQRCodeSize.AutoSize = true;
            this.LabelQRCodeSize.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LabelQRCodeSize.Location = new System.Drawing.Point(14, 96);
            this.LabelQRCodeSize.Name = "LabelQRCodeSize";
            this.LabelQRCodeSize.Size = new System.Drawing.Size(80, 13);
            this.LabelQRCodeSize.TabIndex = 14;
            this.LabelQRCodeSize.Text = "QR Code Size";
            // 
            // ComboBoxLabelSize
            // 
            this.ComboBoxLabelSize.FormattingEnabled = true;
            this.ComboBoxLabelSize.Items.AddRange(new object[] {
            "35 mm * 25 mm",
            "50 mm * 25 mm",
            "100 mm* 23 mm"});
            this.ComboBoxLabelSize.Location = new System.Drawing.Point(159, 157);
            this.ComboBoxLabelSize.Name = "ComboBoxLabelSize";
            this.ComboBoxLabelSize.Size = new System.Drawing.Size(121, 21);
            this.ComboBoxLabelSize.TabIndex = 4;
            this.ComboBoxLabelSize.Visible = false;
            // 
            // LabelLabelSize
            // 
            this.LabelLabelSize.AutoSize = true;
            this.LabelLabelSize.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LabelLabelSize.Location = new System.Drawing.Point(156, 138);
            this.LabelLabelSize.Name = "LabelLabelSize";
            this.LabelLabelSize.Size = new System.Drawing.Size(63, 13);
            this.LabelLabelSize.TabIndex = 13;
            this.LabelLabelSize.Text = "Label Size";
            this.LabelLabelSize.Visible = false;
            // 
            // YesNoRadioPaperSize
            // 
            this.YesNoRadioPaperSize.BackColor = System.Drawing.SystemColors.Window;
            this.YesNoRadioPaperSize.Checked = false;
            this.YesNoRadioPaperSize.FirstButtonName = "Label";
            this.YesNoRadioPaperSize.Location = new System.Drawing.Point(12, 71);
            this.YesNoRadioPaperSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.YesNoRadioPaperSize.Name = "YesNoRadioPaperSize";
            this.YesNoRadioPaperSize.SecondButtonName = "A4";
            this.YesNoRadioPaperSize.Size = new System.Drawing.Size(110, 23);
            this.YesNoRadioPaperSize.TabIndex = 1;
            this.YesNoRadioPaperSize.Load += new System.EventHandler(this.YesNoRadioPaperSize_Load);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(14, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Paper Size";
            // 
            // TextBoxPrintQuantity
            // 
            this.TextBoxPrintQuantity.Location = new System.Drawing.Point(17, 31);
            this.TextBoxPrintQuantity.MaxLength = 3;
            this.TextBoxPrintQuantity.Name = "TextBoxPrintQuantity";
            this.TextBoxPrintQuantity.Size = new System.Drawing.Size(105, 21);
            this.TextBoxPrintQuantity.TabIndex = 0;
            this.TextBoxPrintQuantity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxPrintQuantity_KeyPress);
            // 
            // TextBoxStartLocation
            // 
            this.TextBoxStartLocation.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxStartLocation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxStartLocation.Controls.Add(this.Row);
            this.TextBoxStartLocation.Controls.Add(this.Separator);
            this.TextBoxStartLocation.Controls.Add(this.Column);
            this.TextBoxStartLocation.Location = new System.Drawing.Point(17, 155);
            this.TextBoxStartLocation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TextBoxStartLocation.Name = "TextBoxStartLocation";
            this.TextBoxStartLocation.NoOfColoumns = 0;
            this.TextBoxStartLocation.NoOfRow = 0;
            this.TextBoxStartLocation.Size = new System.Drawing.Size(136, 21);
            this.TextBoxStartLocation.TabIndex = 3;
            this.TextBoxStartLocation.Visible = false;
            this.TextBoxStartLocation.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TextBoxStartLocation_PreviewKeyDown);
            // 
            // Row
            // 
            this.Row.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Row.Location = new System.Drawing.Point(1, 3);
            this.Row.MaxLength = 1;
            this.Row.Name = "Row";
            this.Row.Size = new System.Drawing.Size(15, 14);
            this.Row.TabIndex = 0;
            this.Row.Text = "1";
            this.Row.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Separator
            // 
            this.Separator.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Separator.Enabled = false;
            this.Separator.Location = new System.Drawing.Point(12, 3);
            this.Separator.MaxLength = 1;
            this.Separator.Name = "Separator";
            this.Separator.Size = new System.Drawing.Size(9, 14);
            this.Separator.TabIndex = 0;
            this.Separator.TabStop = false;
            this.Separator.Text = ",";
            this.Separator.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Column
            // 
            this.Column.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Column.Location = new System.Drawing.Point(22, 3);
            this.Column.MaxLength = 1;
            this.Column.Name = "Column";
            this.Column.Size = new System.Drawing.Size(9, 14);
            this.Column.TabIndex = 1;
            this.Column.Text = "1";
            this.Column.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // LabelStartLocation
            // 
            this.LabelStartLocation.AutoSize = true;
            this.LabelStartLocation.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LabelStartLocation.Location = new System.Drawing.Point(14, 138);
            this.LabelStartLocation.Name = "LabelStartLocation";
            this.LabelStartLocation.Size = new System.Drawing.Size(87, 13);
            this.LabelStartLocation.TabIndex = 5;
            this.LabelStartLocation.Text = "Start Location";
            this.LabelStartLocation.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Quantity";
            // 
            // Print
            // 
            this.Print.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Print.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PrintErrorMsg});
            this.Print.Location = new System.Drawing.Point(0, 269);
            this.Print.Name = "Print";
            this.Print.Size = new System.Drawing.Size(368, 22);
            this.Print.TabIndex = 15;
            // 
            // PrintErrorMsg
            // 
            this.PrintErrorMsg.Name = "PrintErrorMsg";
            this.PrintErrorMsg.Size = new System.Drawing.Size(31, 17);
            this.PrintErrorMsg.Text = "        ";
            // 
            // BtnPrint
            // 
            this.BtnPrint.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BtnPrint.Location = new System.Drawing.Point(273, 237);
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.Size = new System.Drawing.Size(83, 23);
            this.BtnPrint.TabIndex = 5;
            this.BtnPrint.Text = "Print [F9]";
            this.BtnPrint.UseVisualStyleBackColor = true;
            this.BtnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BtnCancel.Location = new System.Drawing.Point(184, 237);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(83, 23);
            this.BtnCancel.TabIndex = 6;
            this.BtnCancel.Text = "Cancel [Esc]";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // FormCatalogQRCodePrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 291);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.Print);
            this.Controls.Add(this.BtnPrint);
            this.Controls.Add(this.BtnCancel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCatalogQRCodePrint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QR Code Print";
            this.Load += new System.EventHandler(this.FormCatalogQRCodePrint_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.TextBoxStartLocation.ResumeLayout(false);
            this.TextBoxStartLocation.PerformLayout();
            this.Print.ResumeLayout(false);
            this.Print.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox ComboBoxLabelSize;
        private System.Windows.Forms.Label LabelLabelSize;
        private controls.YesNoRadio YesNoRadioPaperSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextBoxPrintQuantity;
        private controls.text.UserControlPoint TextBoxStartLocation;
        private controls.text.NumberTextBox Row;
        private controls.text.NumberTextBox Separator;
        private controls.text.NumberTextBox Column;
        private System.Windows.Forms.Label LabelStartLocation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip Print;
        private System.Windows.Forms.ToolStripStatusLabel PrintErrorMsg;
        private System.Windows.Forms.Button BtnPrint;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Label LabelQRCodeSize;
        private System.Windows.Forms.ComboBox ComboBoxQRCodeSize;
        private controls.QRCodeControl BatchQRCode;
        private controls.ComboBoxSwapTextBox ComboBoxDefaultPrinter;
        private System.Windows.Forms.Label LabelChoosePrinter;
    }
}