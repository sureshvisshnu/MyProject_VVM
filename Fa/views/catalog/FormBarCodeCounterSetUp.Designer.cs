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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBarCodeCounterSetUp));
            BarCodeSetUpToolStrip = new StatusStrip();
            PrintErrorMsg = new ToolStripStatusLabel();
            label4 = new Label();
            label9 = new Label();
            ComboBoxLabelType = new ComboBox();
            LabelLabelSize = new Label();
            BtnPriceCalculatorSave = new Button();
            BtnPriceCalculatorCancel = new Button();
            NumericUpDownLabelsPerRow = new NumericUpDown();
            label1 = new Label();
            NumericUpDownTotalLabelCount = new NumericUpDown();
            NumericUpDownRunningCount = new NumericUpDown();
            NumericUpDownWastedLabels = new NumericUpDown();
            label2 = new Label();
            DateTimePickerDateLoaded = new fa.views.controls.text.DateWithCalendar();
            label3 = new Label();
            DateTimePickerDateEnded = new CheckBox();
            NumericUpDownFlagCount = new NumericUpDown();
            label5 = new Label();
            TextBoxBarCodeId = new TextBox();
            ComboBoxLabelSize = new ComboBox();
            label6 = new Label();
            BarCodeSetUpToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NumericUpDownLabelsPerRow).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDownTotalLabelCount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDownRunningCount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDownWastedLabels).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDownFlagCount).BeginInit();
            SuspendLayout();
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(179, 163);
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
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label9.Location = new Point(23, 74);
            label9.Name = "label9";
            label9.Size = new Size(123, 13);
            label9.TabIndex = 475;
            label9.Text = "BarCode Label Count";
            // 
            // ComboBoxLabelType
            // 
            ComboBoxLabelType.FormattingEnabled = true;
            ComboBoxLabelType.Items.AddRange(new object[] { "25 mm * 20 mm", "35 mm * 25 mm", "50 mm * 25 mm", "100 mm* 23 mm" });
            ComboBoxLabelType.Location = new Point(23, 44);
            ComboBoxLabelType.Name = "ComboBoxLabelType";
            ComboBoxLabelType.Size = new Size(110, 23);
            ComboBoxLabelType.TabIndex = 479;
            // 
            // LabelLabelSize
            // 
            LabelLabelSize.AutoSize = true;
            LabelLabelSize.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelLabelSize.Location = new Point(23, 27);
            LabelLabelSize.Name = "LabelLabelSize";
            LabelLabelSize.Size = new Size(118, 13);
            LabelLabelSize.TabIndex = 480;
            LabelLabelSize.Text = "BarCode Label Type";
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
            // NumericUpDownLabelsPerRow
            // 
            NumericUpDownLabelsPerRow.Location = new Point(152, 45);
            NumericUpDownLabelsPerRow.Name = "NumericUpDownLabelsPerRow";
            NumericUpDownLabelsPerRow.Size = new Size(86, 23);
            NumericUpDownLabelsPerRow.TabIndex = 484;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(152, 27);
            label1.Name = "label1";
            label1.Size = new Size(86, 13);
            label1.TabIndex = 485;
            label1.Text = "Label Per Row";
            // 
            // NumericUpDownTotalLabelCount
            // 
            NumericUpDownTotalLabelCount.Location = new Point(23, 92);
            NumericUpDownTotalLabelCount.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            NumericUpDownTotalLabelCount.Name = "NumericUpDownTotalLabelCount";
            NumericUpDownTotalLabelCount.Size = new Size(110, 23);
            NumericUpDownTotalLabelCount.TabIndex = 486;
            // 
            // NumericUpDownRunningCount
            // 
            NumericUpDownRunningCount.Location = new Point(152, 92);
            NumericUpDownRunningCount.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            NumericUpDownRunningCount.Name = "NumericUpDownRunningCount";
            NumericUpDownRunningCount.Size = new Size(86, 23);
            NumericUpDownRunningCount.TabIndex = 487;
            // 
            // NumericUpDownWastedLabels
            // 
            NumericUpDownWastedLabels.Location = new Point(23, 136);
            NumericUpDownWastedLabels.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
            NumericUpDownWastedLabels.Name = "NumericUpDownWastedLabels";
            NumericUpDownWastedLabels.Size = new Size(110, 23);
            NumericUpDownWastedLabels.TabIndex = 488;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(152, 76);
            label2.Name = "label2";
            label2.Size = new Size(122, 13);
            label2.TabIndex = 489;
            label2.Text = "Running Label Count";
            // 
            // DateTimePickerDateLoaded
            // 
            DateTimePickerDateLoaded.BackColor = Color.White;
            DateTimePickerDateLoaded.BorderStyle = BorderStyle.FixedSingle;
            DateTimePickerDateLoaded.Date = null;
            DateTimePickerDateLoaded.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DateTimePickerDateLoaded.Format = "MM/dd/yyyy";
            DateTimePickerDateLoaded.Location = new Point(378, 44);
            DateTimePickerDateLoaded.Margin = new Padding(4, 3, 4, 3);
            DateTimePickerDateLoaded.MaxDate = new DateTime(9997, 12, 31, 8, 8, 41, 0);
            DateTimePickerDateLoaded.MinDate = new DateTime(1900, 1, 1, 22, 27, 56, 0);
            DateTimePickerDateLoaded.Name = "DateTimePickerDateLoaded";
            DateTimePickerDateLoaded.ReadOnly = false;
            DateTimePickerDateLoaded.Size = new Size(93, 21);
            DateTimePickerDateLoaded.TabIndex = 490;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(378, 27);
            label3.Name = "label3";
            label3.Size = new Size(87, 13);
            label3.TabIndex = 491;
            label3.Text = "Installed Date";
            // 
            // DateTimePickerDateEnded
            // 
            DateTimePickerDateEnded.AutoSize = true;
            DateTimePickerDateEnded.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            DateTimePickerDateEnded.Location = new Point(378, 75);
            DateTimePickerDateEnded.Name = "DateTimePickerDateEnded";
            DateTimePickerDateEnded.Size = new Size(76, 17);
            DateTimePickerDateEnded.TabIndex = 492;
            DateTimePickerDateEnded.Text = "End Date";
            DateTimePickerDateEnded.UseVisualStyleBackColor = true;
            // 
            // NumericUpDownFlagCount
            // 
            NumericUpDownFlagCount.Location = new Point(152, 136);
            NumericUpDownFlagCount.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
            NumericUpDownFlagCount.Name = "NumericUpDownFlagCount";
            NumericUpDownFlagCount.Size = new Size(86, 23);
            NumericUpDownFlagCount.TabIndex = 494;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label5.Location = new Point(152, 120);
            label5.Name = "label5";
            label5.Size = new Size(99, 13);
            label5.TabIndex = 493;
            label5.Text = "Flag Label Count";
            // 
            // TextBoxBarCodeId
            // 
            TextBoxBarCodeId.Location = new Point(378, 110);
            TextBoxBarCodeId.Margin = new Padding(2);
            TextBoxBarCodeId.Name = "TextBoxBarCodeId";
            TextBoxBarCodeId.Size = new Size(79, 23);
            TextBoxBarCodeId.TabIndex = 495;
            TextBoxBarCodeId.TabStop = false;
            TextBoxBarCodeId.Visible = false;
            // 
            // ComboBoxLabelSize
            // 
            ComboBoxLabelSize.FormattingEnabled = true;
            ComboBoxLabelSize.Items.AddRange(new object[] { "q812", "q820", "q850" });
            ComboBoxLabelSize.Location = new Point(261, 44);
            ComboBoxLabelSize.Name = "ComboBoxLabelSize";
            ComboBoxLabelSize.Size = new Size(79, 23);
            ComboBoxLabelSize.TabIndex = 496;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label6.Location = new Point(259, 27);
            label6.Name = "label6";
            label6.Size = new Size(63, 13);
            label6.TabIndex = 497;
            label6.Text = "Label Size";
            // 
            // FormBarCodeCounterSetUp
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 206);
            Controls.Add(label6);
            Controls.Add(ComboBoxLabelSize);
            Controls.Add(TextBoxBarCodeId);
            Controls.Add(NumericUpDownFlagCount);
            Controls.Add(label5);
            Controls.Add(DateTimePickerDateEnded);
            Controls.Add(label3);
            Controls.Add(DateTimePickerDateLoaded);
            Controls.Add(label2);
            Controls.Add(NumericUpDownWastedLabels);
            Controls.Add(NumericUpDownRunningCount);
            Controls.Add(NumericUpDownTotalLabelCount);
            Controls.Add(label1);
            Controls.Add(NumericUpDownLabelsPerRow);
            Controls.Add(BtnPriceCalculatorSave);
            Controls.Add(BtnPriceCalculatorCancel);
            Controls.Add(ComboBoxLabelType);
            Controls.Add(LabelLabelSize);
            Controls.Add(label4);
            Controls.Add(label9);
            Controls.Add(BarCodeSetUpToolStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormBarCodeCounterSetUp";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Bar Code Counting SetUp";
            Controls.SetChildIndex(BarCodeSetUpToolStrip, 0);
            Controls.SetChildIndex(label9, 0);
            Controls.SetChildIndex(label4, 0);
            Controls.SetChildIndex(LabelLabelSize, 0);
            Controls.SetChildIndex(ComboBoxLabelType, 0);
            Controls.SetChildIndex(BtnPriceCalculatorCancel, 0);
            Controls.SetChildIndex(BtnPriceCalculatorSave, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(checkBoxIsPatient, 0);
            Controls.SetChildIndex(NumericUpDownLabelsPerRow, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(NumericUpDownTotalLabelCount, 0);
            Controls.SetChildIndex(NumericUpDownRunningCount, 0);
            Controls.SetChildIndex(NumericUpDownWastedLabels, 0);
            Controls.SetChildIndex(label2, 0);
            Controls.SetChildIndex(DateTimePickerDateLoaded, 0);
            Controls.SetChildIndex(label3, 0);
            Controls.SetChildIndex(DateTimePickerDateEnded, 0);
            Controls.SetChildIndex(label5, 0);
            Controls.SetChildIndex(NumericUpDownFlagCount, 0);
            Controls.SetChildIndex(TextBoxBarCodeId, 0);
            Controls.SetChildIndex(ComboBoxLabelSize, 0);
            Controls.SetChildIndex(label6, 0);
            BarCodeSetUpToolStrip.ResumeLayout(false);
            BarCodeSetUpToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)NumericUpDownLabelsPerRow).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDownTotalLabelCount).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDownRunningCount).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDownWastedLabels).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDownFlagCount).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip BarCodeSetUpToolStrip;
        private ToolStripStatusLabel PrintErrorMsg;
        private Label label4;
        private Label label9;
        private ComboBox ComboBoxLabelType;
        private Label LabelLabelSize;
        private Button BtnPriceCalculatorSave;
        private Button BtnPriceCalculatorCancel;
        private NumericUpDown NumericUpDownLabelsPerRow;
        private Label label1;
        private NumericUpDown NumericUpDownTotalLabelCount;
        private NumericUpDown NumericUpDownRunningCount;
        private NumericUpDown NumericUpDownWastedLabels;
        private Label label2;
        private fa.views.controls.text.DateWithCalendar DateTimePickerDateLoaded;
        private Label label3;
        private CheckBox DateTimePickerDateEnded;
        private NumericUpDown NumericUpDownFlagCount;
        private Label label5;
        private TextBox TextBoxBarCodeId;
        private ComboBox ComboBoxLabelSize;
        private Label label6;
    }
}