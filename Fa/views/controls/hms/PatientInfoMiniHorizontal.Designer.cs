namespace fa.views.controls.hms
{
    partial class PatientInfoMiniHorizontal
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
            TextBoxPatientAge = new TextBox();
            LabelAge = new Label();
            LabelDOB = new Label();
            TextBoxPatientAddress = new TextBox();
            TextBoxPatientName = new TextBox();
            label2 = new Label();
            label1 = new Label();
            TextBoxPatientDOB = new text.DateWithCalendar();
            PatientPhoto = new PatientPhoto();
            PatientNumber = new PatientNumberControl();
            SuspendLayout();
            // 
            // TextBoxPatientAge
            // 
            TextBoxPatientAge.BackColor = Color.White;
            TextBoxPatientAge.Location = new Point(431, 40);
            TextBoxPatientAge.Name = "TextBoxPatientAge";
            TextBoxPatientAge.ReadOnly = true;
            TextBoxPatientAge.Size = new Size(34, 21);
            TextBoxPatientAge.TabIndex = 4;
            TextBoxPatientAge.TabStop = false;
            TextBoxPatientAge.PreviewKeyDown += TextBoxPatientName_PreviewKeyDown;
            // 
            // LabelAge
            // 
            LabelAge.AutoSize = true;
            LabelAge.Location = new Point(399, 43);
            LabelAge.Name = "LabelAge";
            LabelAge.Size = new Size(26, 13);
            LabelAge.TabIndex = 50;
            LabelAge.Text = "Age";
            // 
            // LabelDOB
            // 
            LabelDOB.AutoSize = true;
            LabelDOB.Location = new Point(396, 17);
            LabelDOB.Name = "LabelDOB";
            LabelDOB.Size = new Size(28, 13);
            LabelDOB.TabIndex = 48;
            LabelDOB.Text = "DOB";
            // 
            // TextBoxPatientAddress
            // 
            TextBoxPatientAddress.BackColor = Color.White;
            TextBoxPatientAddress.Location = new Point(63, 43);
            TextBoxPatientAddress.Multiline = true;
            TextBoxPatientAddress.Name = "TextBoxPatientAddress";
            TextBoxPatientAddress.ReadOnly = true;
            TextBoxPatientAddress.Size = new Size(324, 63);
            TextBoxPatientAddress.TabIndex = 2;
            TextBoxPatientAddress.TabStop = false;
            TextBoxPatientAddress.PreviewKeyDown += TextBoxPatientName_PreviewKeyDown;
            // 
            // TextBoxPatientName
            // 
            TextBoxPatientName.BackColor = Color.White;
            TextBoxPatientName.Location = new Point(63, 15);
            TextBoxPatientName.Name = "TextBoxPatientName";
            TextBoxPatientName.ReadOnly = true;
            TextBoxPatientName.Size = new Size(324, 21);
            TextBoxPatientName.TabIndex = 1;
            TextBoxPatientName.TabStop = false;
            TextBoxPatientName.PreviewKeyDown += TextBoxPatientName_PreviewKeyDown;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(11, 43);
            label2.Name = "label2";
            label2.Size = new Size(46, 13);
            label2.TabIndex = 45;
            label2.Text = "Address";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(11, 18);
            label1.Name = "label1";
            label1.Size = new Size(34, 13);
            label1.TabIndex = 44;
            label1.Text = "Name";
            // 
            // TextBoxPatientDOB
            // 
            TextBoxPatientDOB.BackColor = Color.White;
            TextBoxPatientDOB.BorderStyle = BorderStyle.FixedSingle;
            TextBoxPatientDOB.Date = null;
            TextBoxPatientDOB.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxPatientDOB.Format = "MM/dd/yyyy";
            TextBoxPatientDOB.Location = new Point(431, 13);
            TextBoxPatientDOB.Margin = new Padding(4, 3, 4, 3);
            TextBoxPatientDOB.MaxDate = new DateTime(9997, 12, 31, 8, 1, 36, 0);
            TextBoxPatientDOB.MinDate = new DateTime(1900, 1, 1, 19, 41, 10, 0);
            TextBoxPatientDOB.Name = "TextBoxPatientDOB";
            TextBoxPatientDOB.ReadOnly = true;
            TextBoxPatientDOB.Size = new Size(93, 21);
            TextBoxPatientDOB.TabIndex = 3;
            TextBoxPatientDOB.TabStop = false;
            TextBoxPatientDOB.PreviewKeyDown += TextBoxPatientName_PreviewKeyDown;
            // 
            // PatientPhoto
            // 
            PatientPhoto.Location = new Point(888, 4);
            PatientPhoto.Name = "PatientPhoto";
            PatientPhoto.Size = new Size(87, 115);
            PatientPhoto.TabIndex = 87;
            // 
            // PatientNumber
            // 
            PatientNumber.Location = new Point(690, 4);
            PatientNumber.Name = "PatientNumber";
            PatientNumber.PatientNumber = "";
            PatientNumber.Size = new Size(192, 80);
            PatientNumber.TabIndex = 88;
            // 
            // PatientInfoMiniHorizontal
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(PatientNumber);
            Controls.Add(PatientPhoto);
            Controls.Add(TextBoxPatientDOB);
            Controls.Add(TextBoxPatientAge);
            Controls.Add(LabelAge);
            Controls.Add(LabelDOB);
            Controls.Add(TextBoxPatientAddress);
            Controls.Add(TextBoxPatientName);
            Controls.Add(label2);
            Controls.Add(label1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "PatientInfoMiniHorizontal";
            Size = new Size(975, 121);
            ClientSizeChanged += PatientInfoMiniHorizontal_ClientSizeChanged;
            Resize += PatientInfoMin_Resize;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.TextBox TextBoxPatientAge;
        private System.Windows.Forms.Label LabelAge;
        private System.Windows.Forms.Label LabelDOB;
        private System.Windows.Forms.TextBox TextBoxPatientAddress;
        private System.Windows.Forms.TextBox TextBoxPatientName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private text.DateWithCalendar TextBoxPatientDOB;
        private PatientPhoto PatientPhoto;
        private PatientNumberControl PatientNumber;
    }
}
