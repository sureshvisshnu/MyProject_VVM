namespace fa.views.catalog
{
    partial class FormCatalogTokenPrint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCatalogTokenPrint));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ComboBoxDefaultPrinter = new fa.views.controls.ComboBoxSwapTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TextBoxPrintQuantity = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Print = new System.Windows.Forms.StatusStrip();
            this.PrintErrorMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.BtnPrint = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.Print.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ComboBoxDefaultPrinter);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.TextBoxPrintQuantity);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(9, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(255, 120);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // ComboBoxDefaultPrinter
            // 
            this.ComboBoxDefaultPrinter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ComboBoxDefaultPrinter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ComboBoxDefaultPrinter.FormattingEnabled = true;
            this.ComboBoxDefaultPrinter.Location = new System.Drawing.Point(9, 77);
            this.ComboBoxDefaultPrinter.MaxLength = 30;
            this.ComboBoxDefaultPrinter.Name = "ComboBoxDefaultPrinter";
            this.ComboBoxDefaultPrinter.Size = new System.Drawing.Size(215, 21);
            this.ComboBoxDefaultPrinter.TabIndex = 18;
            this.ComboBoxDefaultPrinter.TxtVisible = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(6, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Choose Printer";
            // 
            // TextBoxPrintQuantity
            // 
            this.TextBoxPrintQuantity.Location = new System.Drawing.Point(9, 35);
            this.TextBoxPrintQuantity.MaxLength = 3;
            this.TextBoxPrintQuantity.Name = "TextBoxPrintQuantity";
            this.TextBoxPrintQuantity.Size = new System.Drawing.Size(100, 21);
            this.TextBoxPrintQuantity.TabIndex = 16;
            this.TextBoxPrintQuantity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxPrintQuantity_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Quantity";
            // 
            // Print
            // 
            this.Print.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Print.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PrintErrorMsg});
            this.Print.Location = new System.Drawing.Point(0, 159);
            this.Print.Name = "Print";
            this.Print.Size = new System.Drawing.Size(273, 22);
            this.Print.TabIndex = 13;
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
            this.BtnPrint.Location = new System.Drawing.Point(178, 129);
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.Size = new System.Drawing.Size(83, 23);
            this.BtnPrint.TabIndex = 11;
            this.BtnPrint.Text = "Print [F9]";
            this.BtnPrint.UseVisualStyleBackColor = true;
            this.BtnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BtnCancel.Location = new System.Drawing.Point(89, 129);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(83, 23);
            this.BtnCancel.TabIndex = 12;
            this.BtnCancel.Text = "Cancel [Esc]";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // FormCatalogTokenPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 181);
            this.Controls.Add(this.Print);
            this.Controls.Add(this.BtnPrint);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCatalogTokenPrint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Print";
            this.Load += new System.EventHandler(this.FormCatalogTokenPrint_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.Print.ResumeLayout(false);
            this.Print.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private controls.ComboBoxSwapTextBox ComboBoxDefaultPrinter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextBoxPrintQuantity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip Print;
        private System.Windows.Forms.ToolStripStatusLabel PrintErrorMsg;
        private System.Windows.Forms.Button BtnPrint;
        private System.Windows.Forms.Button BtnCancel;
    }
}