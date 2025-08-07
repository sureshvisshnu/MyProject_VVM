namespace Fa.views.catalog
{
    partial class FormBarCodeCounterSetUp
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
            BarCodeSetUpToolStrip = new StatusStrip();
            PrintErrorMsg = new ToolStripStatusLabel();
            label4 = new Label();
            TextBoxXFactorWholeSale = new TextBox();
            TextBoxXFactorRetail = new TextBox();
            label9 = new Label();
            ComboBoxLabelSize = new ComboBox();
            LabelLabelSize = new Label();
            BtnPriceCalculatorSave = new Button();
            BtnPriceCalculatorCancel = new Button();
            BarCodeSetUpToolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // BarCodeSetUpToolStrip
            // 
            BarCodeSetUpToolStrip.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BarCodeSetUpToolStrip.Items.AddRange(new ToolStripItem[] { PrintErrorMsg });
            BarCodeSetUpToolStrip.Location = new Point(0, 184);
            BarCodeSetUpToolStrip.Name = "BarCodeSetUpToolStrip";
            BarCodeSetUpToolStrip.Size = new Size(484, 22);
            BarCodeSetUpToolStrip.TabIndex = 11;
            // 
            // PrintErrorMsg
            // 
            PrintErrorMsg.Name = "PrintErrorMsg";
            PrintErrorMsg.Size = new Size(31, 17);
            PrintErrorMsg.Text = "        ";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(23, 120);
            label4.Name = "label4";
            label4.Size = new Size(119, 13);
            label4.TabIndex = 478;
            label4.Text = "Wasted Label Count";
            // 
            // TextBoxXFactorWholeSale
            // 
            TextBoxXFactorWholeSale.BackColor = SystemColors.Window;
            TextBoxXFactorWholeSale.Location = new Point(23, 141);
            TextBoxXFactorWholeSale.MaxLength = 5;
            TextBoxXFactorWholeSale.Name = "TextBoxXFactorWholeSale";
            TextBoxXFactorWholeSale.ReadOnly = true;
            TextBoxXFactorWholeSale.Size = new Size(110, 23);
            TextBoxXFactorWholeSale.TabIndex = 477;
            TextBoxXFactorWholeSale.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxXFactorRetail
            // 
            TextBoxXFactorRetail.BackColor = SystemColors.Window;
            TextBoxXFactorRetail.Location = new Point(23, 91);
            TextBoxXFactorRetail.MaxLength = 5;
            TextBoxXFactorRetail.Name = "TextBoxXFactorRetail";
            TextBoxXFactorRetail.ReadOnly = true;
            TextBoxXFactorRetail.Size = new Size(110, 23);
            TextBoxXFactorRetail.TabIndex = 476;
            TextBoxXFactorRetail.TextAlign = HorizontalAlignment.Right;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label9.Location = new Point(23, 74);
            label9.Name = "label9";
            label9.Size = new Size(159, 13);
            label9.TabIndex = 475;
            label9.Text = "BarCode Label Initial Count";
            // 
            // ComboBoxLabelSize
            // 
            ComboBoxLabelSize.FormattingEnabled = true;
            ComboBoxLabelSize.Items.AddRange(new object[] { "25 mm * 20 mm", "35 mm * 25 mm", "50 mm * 25 mm", "100 mm* 23 mm" });
            ComboBoxLabelSize.Location = new Point(23, 44);
            ComboBoxLabelSize.Name = "ComboBoxLabelSize";
            ComboBoxLabelSize.Size = new Size(110, 23);
            ComboBoxLabelSize.TabIndex = 479;
            ComboBoxLabelSize.Visible = false;
            // 
            // LabelLabelSize
            // 
            LabelLabelSize.AutoSize = true;
            LabelLabelSize.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelLabelSize.Location = new Point(23, 27);
            LabelLabelSize.Name = "LabelLabelSize";
            LabelLabelSize.Size = new Size(113, 13);
            LabelLabelSize.TabIndex = 480;
            LabelLabelSize.Text = "BarCode Label Size";
            LabelLabelSize.Visible = false;
            // 
            // BtnPriceCalculatorSave
            // 
            BtnPriceCalculatorSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPriceCalculatorSave.Location = new Point(381, 141);
            BtnPriceCalculatorSave.Name = "BtnPriceCalculatorSave";
            BtnPriceCalculatorSave.Size = new Size(91, 23);
            BtnPriceCalculatorSave.TabIndex = 481;
            BtnPriceCalculatorSave.Text = "Save [F8]";
            BtnPriceCalculatorSave.UseVisualStyleBackColor = true;
            BtnPriceCalculatorSave.Click += BtnPriceCalculatorSave_Click;
            // 
            // BtnPriceCalculatorCancel
            // 
            BtnPriceCalculatorCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnPriceCalculatorCancel.Location = new Point(287, 141);
            BtnPriceCalculatorCancel.Name = "BtnPriceCalculatorCancel";
            BtnPriceCalculatorCancel.Size = new Size(89, 23);
            BtnPriceCalculatorCancel.TabIndex = 482;
            BtnPriceCalculatorCancel.Text = "Cancel [Esc]";
            BtnPriceCalculatorCancel.UseVisualStyleBackColor = true;
            // 
            // FormBarCodeCounterSetUp
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 206);
            Controls.Add(BtnPriceCalculatorSave);
            Controls.Add(BtnPriceCalculatorCancel);
            Controls.Add(ComboBoxLabelSize);
            Controls.Add(LabelLabelSize);
            Controls.Add(label4);
            Controls.Add(TextBoxXFactorWholeSale);
            Controls.Add(TextBoxXFactorRetail);
            Controls.Add(label9);
            Controls.Add(BarCodeSetUpToolStrip);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormBarCodeCounterSetUp";
            Text = "Bar Code Counting SetUp";
            BarCodeSetUpToolStrip.ResumeLayout(false);
            BarCodeSetUpToolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip BarCodeSetUpToolStrip;
        private ToolStripStatusLabel PrintErrorMsg;
        private Label label4;
        private TextBox TextBoxXFactorWholeSale;
        private TextBox TextBoxXFactorRetail;
        private Label label9;
        private ComboBox ComboBoxLabelSize;
        private Label LabelLabelSize;
        private Button BtnPriceCalculatorSave;
        private Button BtnPriceCalculatorCancel;
    }
}