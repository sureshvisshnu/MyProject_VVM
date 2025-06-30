namespace fa.views.controls.hms
{
    partial class PatientVitalEntry
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            label14 = new Label();
            label13 = new Label();
            label12 = new Label();
            label11 = new Label();
            label10 = new Label();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label15 = new Label();
            GroupBoxVitalEntry = new GroupBox();
            TextBoxVitalEntryWeight = new TextBox();
            TextBoxVitalEntryBMI = new text.CurrencyTextBox();
            TextBoxVitalEntryTemperature = new text.CurrencyTextBox();
            TextBoxVitalEntryOxigenLevel = new text.NumberTextBox(components);
            TextBoxVitalEntryBloodPressureOver = new text.NumberTextBox(components);
            TextBoxVitalEntryBloodPressure = new text.NumberTextBox(components);
            TextBoxVitalEntryRespiratoryRate = new text.NumberTextBox(components);
            TextBoxVitalEntryPulse = new text.NumberTextBox(components);
            TextBoxVitalEntryHeight = new text.NumberTextBox(components);
            label16 = new Label();
            GroupBoxVitalEntry.SuspendLayout();
            SuspendLayout();
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(8, 302);
            label14.Name = "label14";
            label14.Size = new Size(102, 13);
            label14.TabIndex = 15;
            label14.Text = "Blood Oxygen Level";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(8, 262);
            label13.Name = "label13";
            label13.Size = new Size(78, 13);
            label13.TabIndex = 14;
            label13.Text = "Blood Pressure";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(7, 222);
            label12.Name = "label12";
            label12.Size = new Size(89, 13);
            label12.TabIndex = 13;
            label12.Text = "Respiratory Rate";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(8, 182);
            label11.Name = "label11";
            label11.Size = new Size(78, 13);
            label11.TabIndex = 12;
            label11.Text = "Pulse (Per Min)";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(7, 102);
            label10.Name = "label10";
            label10.Size = new Size(86, 13);
            label10.TabIndex = 11;
            label10.Text = "BMI (Calculated)";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(7, 142);
            label9.Name = "label9";
            label9.Size = new Size(69, 13);
            label9.TabIndex = 10;
            label9.Text = "Temperature";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(7, 62);
            label8.Name = "label8";
            label8.Size = new Size(41, 13);
            label8.TabIndex = 9;
            label8.Text = "Weight";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(7, 22);
            label7.Name = "label7";
            label7.Size = new Size(38, 13);
            label7.TabIndex = 8;
            label7.Text = "Height";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(59, 43);
            label1.Name = "label1";
            label1.Size = new Size(20, 13);
            label1.TabIndex = 17;
            label1.Text = "cm";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(59, 83);
            label2.Name = "label2";
            label2.Size = new Size(19, 13);
            label2.TabIndex = 19;
            label2.Text = "Kg";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = SystemColors.Window;
            label3.Location = new Point(60, 163);
            label3.Name = "label3";
            label3.Size = new Size(19, 13);
            label3.TabIndex = 22;
            label3.Text = "°C";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = SystemColors.Window;
            label4.Location = new Point(62, 203);
            label4.Name = "label4";
            label4.Size = new Size(27, 13);
            label4.TabIndex = 24;
            label4.Text = "/min";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = SystemColors.Window;
            label5.Location = new Point(63, 243);
            label5.Name = "label5";
            label5.Size = new Size(27, 13);
            label5.TabIndex = 26;
            label5.Text = "/min";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = SystemColors.Window;
            label6.Location = new Point(63, 283);
            label6.Name = "label6";
            label6.Size = new Size(11, 13);
            label6.TabIndex = 29;
            label6.Text = "/";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(63, 323);
            label15.Name = "label15";
            label15.Size = new Size(18, 13);
            label15.TabIndex = 31;
            label15.Text = "%";
            // 
            // GroupBoxVitalEntry
            // 
            GroupBoxVitalEntry.BackColor = Color.White;
            GroupBoxVitalEntry.Controls.Add(TextBoxVitalEntryWeight);
            GroupBoxVitalEntry.Controls.Add(TextBoxVitalEntryBMI);
            GroupBoxVitalEntry.Controls.Add(TextBoxVitalEntryTemperature);
            GroupBoxVitalEntry.Controls.Add(TextBoxVitalEntryOxigenLevel);
            GroupBoxVitalEntry.Controls.Add(TextBoxVitalEntryBloodPressureOver);
            GroupBoxVitalEntry.Controls.Add(TextBoxVitalEntryBloodPressure);
            GroupBoxVitalEntry.Controls.Add(TextBoxVitalEntryRespiratoryRate);
            GroupBoxVitalEntry.Controls.Add(TextBoxVitalEntryPulse);
            GroupBoxVitalEntry.Controls.Add(TextBoxVitalEntryHeight);
            GroupBoxVitalEntry.Controls.Add(label7);
            GroupBoxVitalEntry.Controls.Add(label15);
            GroupBoxVitalEntry.Controls.Add(label8);
            GroupBoxVitalEntry.Controls.Add(label9);
            GroupBoxVitalEntry.Controls.Add(label6);
            GroupBoxVitalEntry.Controls.Add(label10);
            GroupBoxVitalEntry.Controls.Add(label11);
            GroupBoxVitalEntry.Controls.Add(label12);
            GroupBoxVitalEntry.Controls.Add(label5);
            GroupBoxVitalEntry.Controls.Add(label13);
            GroupBoxVitalEntry.Controls.Add(label14);
            GroupBoxVitalEntry.Controls.Add(label4);
            GroupBoxVitalEntry.Controls.Add(label1);
            GroupBoxVitalEntry.Controls.Add(label3);
            GroupBoxVitalEntry.Controls.Add(label2);
            GroupBoxVitalEntry.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            GroupBoxVitalEntry.Location = new Point(3, 19);
            GroupBoxVitalEntry.Name = "GroupBoxVitalEntry";
            GroupBoxVitalEntry.Size = new Size(164, 489);
            GroupBoxVitalEntry.TabIndex = 0;
            GroupBoxVitalEntry.TabStop = false;
            GroupBoxVitalEntry.ClientSizeChanged += GroupBoxVitalEntry_ClientSizeChanged;
            // 
            // TextBoxVitalEntryWeight
            // 
            TextBoxVitalEntryWeight.BackColor = Color.White;
            TextBoxVitalEntryWeight.Location = new Point(10, 78);
            TextBoxVitalEntryWeight.MaxLength = 7;
            TextBoxVitalEntryWeight.Name = "TextBoxVitalEntryWeight";
            TextBoxVitalEntryWeight.Size = new Size(49, 21);
            TextBoxVitalEntryWeight.TabIndex = 1;
            TextBoxVitalEntryWeight.Text = "0.000";
            TextBoxVitalEntryWeight.TextAlign = HorizontalAlignment.Right;
            TextBoxVitalEntryWeight.TextChanged += TextBoxVitalEntryHeight_TextChanged;
            TextBoxVitalEntryWeight.KeyPress += TextBoxVitalEntryWeight_KeyPress;
            TextBoxVitalEntryWeight.Leave += TextBoxVitalEntryWeight_Leave;
            // 
            // TextBoxVitalEntryBMI
            // 
            TextBoxVitalEntryBMI.BackColor = Color.White;
            TextBoxVitalEntryBMI.Decimals = 2;
            TextBoxVitalEntryBMI.Length = 10;
            TextBoxVitalEntryBMI.Location = new Point(10, 119);
            TextBoxVitalEntryBMI.MaxLength = 7;
            TextBoxVitalEntryBMI.Name = "TextBoxVitalEntryBMI";
            TextBoxVitalEntryBMI.ReadOnly = true;
            TextBoxVitalEntryBMI.Size = new Size(49, 21);
            TextBoxVitalEntryBMI.TabIndex = 2;
            TextBoxVitalEntryBMI.TabStop = false;
            TextBoxVitalEntryBMI.Text = "0.00";
            TextBoxVitalEntryBMI.TextAlign = HorizontalAlignment.Right;
            TextBoxVitalEntryBMI.Leave += TextBoxVitalEntryBMI_Leave;
            // 
            // TextBoxVitalEntryTemperature
            // 
            TextBoxVitalEntryTemperature.Decimals = 2;
            TextBoxVitalEntryTemperature.Length = 10;
            TextBoxVitalEntryTemperature.Location = new Point(10, 159);
            TextBoxVitalEntryTemperature.MaxLength = 3;
            TextBoxVitalEntryTemperature.Name = "TextBoxVitalEntryTemperature";
            TextBoxVitalEntryTemperature.Size = new Size(49, 21);
            TextBoxVitalEntryTemperature.TabIndex = 3;
            TextBoxVitalEntryTemperature.Text = "0.00";
            TextBoxVitalEntryTemperature.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxVitalEntryOxigenLevel
            // 
            TextBoxVitalEntryOxigenLevel.BackColor = Color.White;
            TextBoxVitalEntryOxigenLevel.Location = new Point(11, 320);
            TextBoxVitalEntryOxigenLevel.MaxLength = 3;
            TextBoxVitalEntryOxigenLevel.Name = "TextBoxVitalEntryOxigenLevel";
            TextBoxVitalEntryOxigenLevel.Size = new Size(49, 21);
            TextBoxVitalEntryOxigenLevel.TabIndex = 8;
            TextBoxVitalEntryOxigenLevel.TextAlign = HorizontalAlignment.Right;
            TextBoxVitalEntryOxigenLevel.Leave += TextBoxVitalEntryOxigenLevel_Leave;
            // 
            // TextBoxVitalEntryBloodPressureOver
            // 
            TextBoxVitalEntryBloodPressureOver.BackColor = Color.White;
            TextBoxVitalEntryBloodPressureOver.Location = new Point(81, 280);
            TextBoxVitalEntryBloodPressureOver.MaxLength = 3;
            TextBoxVitalEntryBloodPressureOver.Name = "TextBoxVitalEntryBloodPressureOver";
            TextBoxVitalEntryBloodPressureOver.Size = new Size(49, 21);
            TextBoxVitalEntryBloodPressureOver.TabIndex = 7;
            TextBoxVitalEntryBloodPressureOver.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxVitalEntryBloodPressure
            // 
            TextBoxVitalEntryBloodPressure.BackColor = Color.White;
            TextBoxVitalEntryBloodPressure.Location = new Point(11, 280);
            TextBoxVitalEntryBloodPressure.MaxLength = 3;
            TextBoxVitalEntryBloodPressure.Name = "TextBoxVitalEntryBloodPressure";
            TextBoxVitalEntryBloodPressure.Size = new Size(49, 21);
            TextBoxVitalEntryBloodPressure.TabIndex = 6;
            TextBoxVitalEntryBloodPressure.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxVitalEntryRespiratoryRate
            // 
            TextBoxVitalEntryRespiratoryRate.BackColor = Color.White;
            TextBoxVitalEntryRespiratoryRate.Location = new Point(10, 240);
            TextBoxVitalEntryRespiratoryRate.MaxLength = 3;
            TextBoxVitalEntryRespiratoryRate.Name = "TextBoxVitalEntryRespiratoryRate";
            TextBoxVitalEntryRespiratoryRate.Size = new Size(49, 21);
            TextBoxVitalEntryRespiratoryRate.TabIndex = 5;
            TextBoxVitalEntryRespiratoryRate.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxVitalEntryPulse
            // 
            TextBoxVitalEntryPulse.BackColor = Color.White;
            TextBoxVitalEntryPulse.Location = new Point(10, 199);
            TextBoxVitalEntryPulse.MaxLength = 3;
            TextBoxVitalEntryPulse.Name = "TextBoxVitalEntryPulse";
            TextBoxVitalEntryPulse.Size = new Size(49, 21);
            TextBoxVitalEntryPulse.TabIndex = 4;
            TextBoxVitalEntryPulse.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxVitalEntryHeight
            // 
            TextBoxVitalEntryHeight.BackColor = Color.White;
            TextBoxVitalEntryHeight.Location = new Point(10, 39);
            TextBoxVitalEntryHeight.MaxLength = 3;
            TextBoxVitalEntryHeight.Name = "TextBoxVitalEntryHeight";
            TextBoxVitalEntryHeight.Size = new Size(49, 21);
            TextBoxVitalEntryHeight.TabIndex = 0;
            TextBoxVitalEntryHeight.TextAlign = HorizontalAlignment.Right;
            TextBoxVitalEntryHeight.TextChanged += TextBoxVitalEntryHeight_TextChanged;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(3, 4);
            label16.Name = "label16";
            label16.Size = new Size(61, 13);
            label16.TabIndex = 33;
            label16.Text = "Vitals Entry";
            // 
            // PatientVitalEntry
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            Controls.Add(label16);
            Controls.Add(GroupBoxVitalEntry);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "PatientVitalEntry";
            Size = new Size(173, 514);
            ClientSizeChanged += PatientVitalEntry_ClientSizeChanged;
            Enter += PatientVitalEntry_Enter;
            GroupBoxVitalEntry.ResumeLayout(false);
            GroupBoxVitalEntry.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox GroupBoxVitalEntry;
        private System.Windows.Forms.Label label16;
        private text.NumberTextBox TextBoxVitalEntryHeight;
        private text.NumberTextBox TextBoxVitalEntryOxigenLevel;
        private text.NumberTextBox TextBoxVitalEntryBloodPressureOver;
        private text.NumberTextBox TextBoxVitalEntryBloodPressure;
        private text.NumberTextBox TextBoxVitalEntryRespiratoryRate;
        private text.NumberTextBox TextBoxVitalEntryPulse;
        private text.CurrencyTextBox TextBoxVitalEntryTemperature;
        private text.CurrencyTextBox TextBoxVitalEntryBMI;
        private TextBox TextBoxVitalEntryWeight;
    }
}
