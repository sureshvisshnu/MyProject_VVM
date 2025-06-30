namespace fa.views.controls
{
    partial class PatientNumberControl
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
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBoxPatientNumber = new System.Windows.Forms.PictureBox();
            this.LabelValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPatientNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 41;
            this.label5.Text = "Patient Id";
            // 
            // pictureBoxPatientNumber
            // 
            this.pictureBoxPatientNumber.BackgroundImage = global::fa.Properties.Resources.barcode1;
            this.pictureBoxPatientNumber.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxPatientNumber.Location = new System.Drawing.Point(3, 16);
            this.pictureBoxPatientNumber.Name = "pictureBoxPatientNumber";
            this.pictureBoxPatientNumber.Size = new System.Drawing.Size(185, 33);
            this.pictureBoxPatientNumber.TabIndex = 40;
            this.pictureBoxPatientNumber.TabStop = false;
            // 
            // LabelValue
            // 
            this.LabelValue.Location = new System.Drawing.Point(0, 52);
            this.LabelValue.Name = "LabelValue";
            this.LabelValue.Size = new System.Drawing.Size(188, 15);
            this.LabelValue.TabIndex = 45;
            this.LabelValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PatientNumberControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LabelValue);
            this.Controls.Add(this.pictureBoxPatientNumber);
            this.Controls.Add(this.label5);
            this.Name = "PatientNumberControl";
            this.Size = new System.Drawing.Size(191, 69);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPatientNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBoxPatientNumber;
        private System.Windows.Forms.Label LabelValue;
    }
}
