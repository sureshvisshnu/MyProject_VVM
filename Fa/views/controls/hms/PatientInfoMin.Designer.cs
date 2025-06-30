namespace fa.views.controls.hms
{
    partial class PatientInfoMin
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
            this.TextBoxPatientGaurdianName = new System.Windows.Forms.TextBox();
            this.TextBoxPatientAddress = new System.Windows.Forms.TextBox();
            this.TextBoxPatientAge = new System.Windows.Forms.TextBox();
            this.TextBoxPatientName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TextBoxPatientDOB = new fa.views.controls.text.DateWithCalendar();
            this.PatientPhoto = new fa.views.controls.PatientPhoto();
            this.PatientNumberOp = new fa.views.controls.PatientNumberControl();
            this.LabelInsurance = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 479);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 82;
            this.label5.Text = "Gaurdian Name";
            // 
            // TextBoxPatientGaurdianName
            // 
            this.TextBoxPatientGaurdianName.BackColor = System.Drawing.Color.White;
            this.TextBoxPatientGaurdianName.Location = new System.Drawing.Point(7, 496);
            this.TextBoxPatientGaurdianName.Name = "TextBoxPatientGaurdianName";
            this.TextBoxPatientGaurdianName.ReadOnly = true;
            this.TextBoxPatientGaurdianName.Size = new System.Drawing.Size(183, 21);
            this.TextBoxPatientGaurdianName.TabIndex = 80;
            // 
            // TextBoxPatientAddress
            // 
            this.TextBoxPatientAddress.BackColor = System.Drawing.Color.White;
            this.TextBoxPatientAddress.Location = new System.Drawing.Point(7, 417);
            this.TextBoxPatientAddress.Multiline = true;
            this.TextBoxPatientAddress.Name = "TextBoxPatientAddress";
            this.TextBoxPatientAddress.ReadOnly = true;
            this.TextBoxPatientAddress.Size = new System.Drawing.Size(183, 57);
            this.TextBoxPatientAddress.TabIndex = 79;
            // 
            // TextBoxPatientAge
            // 
            this.TextBoxPatientAge.BackColor = System.Drawing.Color.White;
            this.TextBoxPatientAge.Location = new System.Drawing.Point(107, 376);
            this.TextBoxPatientAge.Name = "TextBoxPatientAge";
            this.TextBoxPatientAge.ReadOnly = true;
            this.TextBoxPatientAge.Size = new System.Drawing.Size(35, 21);
            this.TextBoxPatientAge.TabIndex = 78;
            // 
            // TextBoxPatientName
            // 
            this.TextBoxPatientName.BackColor = System.Drawing.Color.White;
            this.TextBoxPatientName.Location = new System.Drawing.Point(8, 336);
            this.TextBoxPatientName.Name = "TextBoxPatientName";
            this.TextBoxPatientName.ReadOnly = true;
            this.TextBoxPatientName.Size = new System.Drawing.Size(183, 21);
            this.TextBoxPatientName.TabIndex = 76;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 401);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 75;
            this.label4.Text = "Address";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(104, 360);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 74;
            this.label3.Text = "Age";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 360);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 73;
            this.label2.Text = "DOB";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 320);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 72;
            this.label1.Text = "Name";
            // 
            // TextBoxPatientDOB
            // 
            this.TextBoxPatientDOB.BackColor = System.Drawing.Color.White;
            this.TextBoxPatientDOB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxPatientDOB.Date = null;
            this.TextBoxPatientDOB.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TextBoxPatientDOB.Format = "MM/dd/yyyy";
            this.TextBoxPatientDOB.Location = new System.Drawing.Point(8, 376);
            this.TextBoxPatientDOB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TextBoxPatientDOB.MaxDate = new System.DateTime(9997, 12, 31, 0, 3, 40, 0);
            this.TextBoxPatientDOB.MinDate = new System.DateTime(1900, 1, 1, 18, 50, 23, 0);
            this.TextBoxPatientDOB.Name = "TextBoxPatientDOB";
            this.TextBoxPatientDOB.ReadOnly = true;
            this.TextBoxPatientDOB.Size = new System.Drawing.Size(93, 21);
            this.TextBoxPatientDOB.TabIndex = 77;
            // 
            // PatientPhoto
            // 
            this.PatientPhoto.Location = new System.Drawing.Point(8, 3);
            this.PatientPhoto.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PatientPhoto.Name = "PatientPhoto";
            this.PatientPhoto.Size = new System.Drawing.Size(182, 210);
            this.PatientPhoto.TabIndex = 71;
            // 
            // PatientNumberOp
            // 
            this.PatientNumberOp.Location = new System.Drawing.Point(6, 219);
            this.PatientNumberOp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PatientNumberOp.Name = "PatientNumberOp";
            this.PatientNumberOp.PatientNumber = "";
            this.PatientNumberOp.Size = new System.Drawing.Size(186, 74);
            this.PatientNumberOp.TabIndex = 84;
            // 
            // LabelInsurance
            // 
            this.LabelInsurance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LabelInsurance.ForeColor = System.Drawing.Color.ForestGreen;
            this.LabelInsurance.Location = new System.Drawing.Point(10, 295);
            this.LabelInsurance.Name = "LabelInsurance";
            this.LabelInsurance.Size = new System.Drawing.Size(178, 22);
            this.LabelInsurance.TabIndex = 85;
            this.LabelInsurance.Text = "**NO INSURANCE**";
            this.LabelInsurance.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // PatientInfoMin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.LabelInsurance);
            this.Controls.Add(this.PatientNumberOp);
            this.Controls.Add(this.TextBoxPatientGaurdianName);
            this.Controls.Add(this.TextBoxPatientAddress);
            this.Controls.Add(this.TextBoxPatientAge);
            this.Controls.Add(this.TextBoxPatientDOB);
            this.Controls.Add(this.TextBoxPatientName);
            this.Controls.Add(this.PatientPhoto);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "PatientInfoMin";
            this.Size = new System.Drawing.Size(207, 520);
            this.ClientSizeChanged += new System.EventHandler(this.PatientInfoMin_ClientSizeChanged);
            this.Resize += new System.EventHandler(this.PatientInfoMin_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TextBoxPatientGaurdianName;
        private System.Windows.Forms.TextBox TextBoxPatientAddress;
        private System.Windows.Forms.TextBox TextBoxPatientAge;
        private text.DateWithCalendar TextBoxPatientDOB;
        private System.Windows.Forms.TextBox TextBoxPatientName;
        private PatientPhoto PatientPhoto;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private PatientNumberControl PatientNumberOp;
        private System.Windows.Forms.Label LabelInsurance;
    }
}
