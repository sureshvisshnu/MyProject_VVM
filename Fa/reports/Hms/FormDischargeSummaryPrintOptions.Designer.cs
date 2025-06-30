namespace fa.reports.Hms
{
    partial class FormDischargeSummaryPrintOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDischargeSummaryPrintOptions));
            this.CheckBoxDischargeSummary = new System.Windows.Forms.CheckBox();
            this.CheckBoxPartientChart = new System.Windows.Forms.CheckBox();
            this.BtnPrint = new System.Windows.Forms.Button();
            this.BtnExit = new System.Windows.Forms.Button();
            this.CheckBoxFull = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ErrorMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.PictureBoxMedHisUnChecked = new System.Windows.Forms.PictureBox();
            this.PictureBoxMedicalHis = new System.Windows.Forms.PictureBox();
            this.CheckBoxPartientIPChart = new System.Windows.Forms.CheckBox();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxMedHisUnChecked)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxMedicalHis)).BeginInit();
            this.SuspendLayout();
            // 
            // CheckBoxDischargeSummary
            // 
            this.CheckBoxDischargeSummary.AutoSize = true;
            this.CheckBoxDischargeSummary.Checked = true;
            this.CheckBoxDischargeSummary.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxDischargeSummary.Location = new System.Drawing.Point(10, 9);
            this.CheckBoxDischargeSummary.Name = "CheckBoxDischargeSummary";
            this.CheckBoxDischargeSummary.Size = new System.Drawing.Size(120, 17);
            this.CheckBoxDischargeSummary.TabIndex = 0;
            this.CheckBoxDischargeSummary.Text = "Discharge Summary";
            this.CheckBoxDischargeSummary.UseVisualStyleBackColor = true;
            this.CheckBoxDischargeSummary.CheckedChanged += new System.EventHandler(this.CheckBoxDischargeSummary_CheckedChanged);
            // 
            // CheckBoxPartientChart
            // 
            this.CheckBoxPartientChart.AutoSize = true;
            this.CheckBoxPartientChart.Location = new System.Drawing.Point(10, 57);
            this.CheckBoxPartientChart.Name = "CheckBoxPartientChart";
            this.CheckBoxPartientChart.Size = new System.Drawing.Size(97, 17);
            this.CheckBoxPartientChart.TabIndex = 1;
            this.CheckBoxPartientChart.Text = "Patient Chart (";
            this.CheckBoxPartientChart.UseVisualStyleBackColor = true;
            this.CheckBoxPartientChart.CheckedChanged += new System.EventHandler(this.CheckBoxPartientChart_CheckedChanged);
            // 
            // BtnPrint
            // 
            this.BtnPrint.Location = new System.Drawing.Point(136, 157);
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.Size = new System.Drawing.Size(75, 23);
            this.BtnPrint.TabIndex = 2;
            this.BtnPrint.Text = "Print [F9]";
            this.BtnPrint.UseVisualStyleBackColor = true;
            this.BtnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.Location = new System.Drawing.Point(217, 157);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(75, 23);
            this.BtnExit.TabIndex = 3;
            this.BtnExit.Text = "Exit [F10]";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // CheckBoxFull
            // 
            this.CheckBoxFull.AutoSize = true;
            this.CheckBoxFull.Enabled = false;
            this.CheckBoxFull.Location = new System.Drawing.Point(99, 57);
            this.CheckBoxFull.Name = "CheckBoxFull";
            this.CheckBoxFull.Size = new System.Drawing.Size(49, 17);
            this.CheckBoxFull.TabIndex = 4;
            this.CheckBoxFull.Text = "Full )";
            this.CheckBoxFull.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(10, 97);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(2);
            this.label1.Size = new System.Drawing.Size(282, 48);
            this.label1.TabIndex = 5;
            this.label1.Text = "By default, system prints the discharge summary only, please select the patient c" +
    "hart options if additional details are needed.";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ErrorMsg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 194);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(304, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            this.ErrorMsg.Name = "ErrorMsg";
            this.ErrorMsg.Size = new System.Drawing.Size(13, 17);
            this.ErrorMsg.Text = "  ";
            // 
            // PictureBoxMedHisUnChecked
            // 
            this.PictureBoxMedHisUnChecked.Image = ((System.Drawing.Image)(resources.GetObject("PictureBoxMedHisUnChecked.Image")));
            this.PictureBoxMedHisUnChecked.Location = new System.Drawing.Point(406, 88);
            this.PictureBoxMedHisUnChecked.Name = "PictureBoxMedHisUnChecked";
            this.PictureBoxMedHisUnChecked.Size = new System.Drawing.Size(72, 73);
            this.PictureBoxMedHisUnChecked.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBoxMedHisUnChecked.TabIndex = 516;
            this.PictureBoxMedHisUnChecked.TabStop = false;
            // 
            // PictureBoxMedicalHis
            // 
            this.PictureBoxMedicalHis.Image = ((System.Drawing.Image)(resources.GetObject("PictureBoxMedicalHis.Image")));
            this.PictureBoxMedicalHis.Location = new System.Drawing.Point(406, 9);
            this.PictureBoxMedicalHis.Name = "PictureBoxMedicalHis";
            this.PictureBoxMedicalHis.Size = new System.Drawing.Size(72, 73);
            this.PictureBoxMedicalHis.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBoxMedicalHis.TabIndex = 515;
            this.PictureBoxMedicalHis.TabStop = false;
            // 
            // CheckBoxPartientIPChart
            // 
            this.CheckBoxPartientIPChart.AutoSize = true;
            this.CheckBoxPartientIPChart.Location = new System.Drawing.Point(10, 33);
            this.CheckBoxPartientIPChart.Name = "CheckBoxPartientIPChart";
            this.CheckBoxPartientIPChart.Size = new System.Drawing.Size(66, 17);
            this.CheckBoxPartientIPChart.TabIndex = 517;
            this.CheckBoxPartientIPChart.Text = "IP Chart";
            this.CheckBoxPartientIPChart.UseVisualStyleBackColor = true;
            this.CheckBoxPartientIPChart.CheckedChanged += new System.EventHandler(this.CheckBoxDischargeSummary_CheckedChanged);
            // 
            // FormDischargeSummaryPrintOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 216);
            this.Controls.Add(this.CheckBoxPartientIPChart);
            this.Controls.Add(this.PictureBoxMedHisUnChecked);
            this.Controls.Add(this.PictureBoxMedicalHis);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CheckBoxFull);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.BtnPrint);
            this.Controls.Add(this.CheckBoxPartientChart);
            this.Controls.Add(this.CheckBoxDischargeSummary);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDischargeSummaryPrintOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print Discharge Summary";
            this.Load += new System.EventHandler(this.FormDischargeSummaryPrintOptions_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxMedHisUnChecked)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxMedicalHis)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.CheckBox CheckBoxDischargeSummary;
        private System.Windows.Forms.CheckBox CheckBoxPartientChart;
        private System.Windows.Forms.Button BtnPrint;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.CheckBox CheckBoxFull;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ErrorMsg;
        private System.Windows.Forms.PictureBox PictureBoxMedHisUnChecked;
        private System.Windows.Forms.PictureBox PictureBoxMedicalHis;
        public System.Windows.Forms.CheckBox CheckBoxPartientIPChart;
    }
}