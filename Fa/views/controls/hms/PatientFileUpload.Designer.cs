namespace Fa.views.controls.hms
{
    partial class PatientFileUpload
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
            ChkBoxHasHeader = new CheckBox();
            TextBoxFileName = new TextBox();
            BtnChoose = new Button();
            BtnUpload = new Button();
            labelFileName = new Label();
            label1 = new Label();
            TxtAccessLog = new TextBox();
            progressBar1 = new ProgressBar();
            TxtErrorLog = new TextBox();
            label2 = new Label();
            TimerTextBox = new Label();
            label4 = new Label();
            BtnCancel = new Button();
            BtnExit = new Button();
            labelSoftwareType = new Label();
            comboBoxSoftwareType = new ComboBox();
            checkBoxPatientDublicate = new CheckBox();
            BtnDownload = new Button();
            SuspendLayout();
            // 
            // ChkBoxHasHeader
            // 
            ChkBoxHasHeader.AutoSize = true;
            ChkBoxHasHeader.Checked = true;
            ChkBoxHasHeader.CheckState = CheckState.Checked;
            ChkBoxHasHeader.Location = new Point(4, 50);
            ChkBoxHasHeader.Margin = new Padding(4, 3, 4, 3);
            ChkBoxHasHeader.Name = "ChkBoxHasHeader";
            ChkBoxHasHeader.Size = new Size(225, 19);
            ChkBoxHasHeader.TabIndex = 70;
            ChkBoxHasHeader.Text = "This file has header (Skip the first row)";
            ChkBoxHasHeader.UseVisualStyleBackColor = true;
            // 
            // TextBoxFileName
            // 
            TextBoxFileName.BackColor = SystemColors.Window;
            TextBoxFileName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxFileName.Location = new Point(4, 20);
            TextBoxFileName.Margin = new Padding(4, 3, 4, 3);
            TextBoxFileName.Name = "TextBoxFileName";
            TextBoxFileName.ReadOnly = true;
            TextBoxFileName.Size = new Size(337, 21);
            TextBoxFileName.TabIndex = 1;
            TextBoxFileName.TabStop = false;
            // 
            // BtnChoose
            // 
            BtnChoose.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnChoose.Location = new Point(344, 19);
            BtnChoose.Margin = new Padding(2);
            BtnChoose.Name = "BtnChoose";
            BtnChoose.Size = new Size(63, 25);
            BtnChoose.TabIndex = 2;
            BtnChoose.Text = "Browse";
            BtnChoose.UseVisualStyleBackColor = true;
            BtnChoose.Click += BtnChoose_Click;
            // 
            // BtnUpload
            // 
            BtnUpload.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnUpload.Location = new Point(409, 19);
            BtnUpload.Margin = new Padding(4, 3, 4, 3);
            BtnUpload.Name = "BtnUpload";
            BtnUpload.Size = new Size(68, 25);
            BtnUpload.TabIndex = 3;
            BtnUpload.Text = "Upload";
            BtnUpload.UseVisualStyleBackColor = true;
            BtnUpload.Click += BtnUpload_Click;
            // 
            // labelFileName
            // 
            labelFileName.AutoSize = true;
            labelFileName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            labelFileName.Location = new Point(3, 2);
            labelFileName.Margin = new Padding(4, 0, 4, 0);
            labelFileName.Name = "labelFileName";
            labelFileName.Size = new Size(53, 13);
            labelFileName.TabIndex = 69;
            labelFileName.Text = "File Name";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(5, 399);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(51, 13);
            label1.TabIndex = 75;
            label1.Text = "Error Log";
            // 
            // TxtAccessLog
            // 
            TxtAccessLog.Location = new Point(2, 174);
            TxtAccessLog.Margin = new Padding(4, 3, 4, 3);
            TxtAccessLog.MaxLength = 294967295;
            TxtAccessLog.Multiline = true;
            TxtAccessLog.Name = "TxtAccessLog";
            TxtAccessLog.ReadOnly = true;
            TxtAccessLog.ScrollBars = ScrollBars.Vertical;
            TxtAccessLog.Size = new Size(558, 220);
            TxtAccessLog.TabIndex = 74;
            TxtAccessLog.WordWrap = false;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(2, 123);
            progressBar1.Margin = new Padding(4, 3, 4, 3);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(556, 27);
            progressBar1.TabIndex = 73;
            // 
            // TxtErrorLog
            // 
            TxtErrorLog.Location = new Point(1, 420);
            TxtErrorLog.Margin = new Padding(4, 3, 4, 3);
            TxtErrorLog.MaxLength = 294967295;
            TxtErrorLog.Multiline = true;
            TxtErrorLog.Name = "TxtErrorLog";
            TxtErrorLog.ReadOnly = true;
            TxtErrorLog.ScrollBars = ScrollBars.Vertical;
            TxtErrorLog.Size = new Size(558, 220);
            TxtErrorLog.TabIndex = 72;
            TxtErrorLog.WordWrap = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(0, 154);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(60, 13);
            label2.TabIndex = 71;
            label2.Text = "Access Log";
            // 
            // TimerTextBox
            // 
            TimerTextBox.AutoSize = true;
            TimerTextBox.Location = new Point(169, 659);
            TimerTextBox.Margin = new Padding(4, 0, 4, 0);
            TimerTextBox.Name = "TimerTextBox";
            TimerTextBox.Size = new Size(49, 15);
            TimerTextBox.TabIndex = 79;
            TimerTextBox.Text = "00.00.00";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(0, 659);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(156, 15);
            label4.TabIndex = 78;
            label4.Text = "Time Duration (In Seconds) :";
            // 
            // BtnCancel
            // 
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(360, 653);
            BtnCancel.Margin = new Padding(4, 3, 4, 3);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(103, 27);
            BtnCancel.TabIndex = 7;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Visible = false;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(470, 653);
            BtnExit.Margin = new Padding(4, 3, 4, 3);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(88, 27);
            BtnExit.TabIndex = 6;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            BtnExit.MouseLeave += BtnExit_MouseLeave;
            BtnExit.MouseHover += BtnExit_MouseHover;
            // 
            // labelSoftwareType
            // 
            labelSoftwareType.AutoSize = true;
            labelSoftwareType.Location = new Point(4, 72);
            labelSoftwareType.Name = "labelSoftwareType";
            labelSoftwareType.Size = new Size(80, 15);
            labelSoftwareType.TabIndex = 80;
            labelSoftwareType.Text = "Software Type";
            // 
            // comboBoxSoftwareType
            // 
            comboBoxSoftwareType.FormattingEnabled = true;
            comboBoxSoftwareType.Items.AddRange(new object[] { "None", "Equals" });
            comboBoxSoftwareType.Location = new Point(3, 92);
            comboBoxSoftwareType.Name = "comboBoxSoftwareType";
            comboBoxSoftwareType.Size = new Size(214, 23);
            comboBoxSoftwareType.TabIndex = 4;
            comboBoxSoftwareType.SelectedIndexChanged += comboBoxSoftwareType_SelectedIndexChanged;
            // 
            // checkBoxPatientDublicate
            // 
            checkBoxPatientDublicate.AutoSize = true;
            checkBoxPatientDublicate.Checked = true;
            checkBoxPatientDublicate.CheckState = CheckState.Checked;
            checkBoxPatientDublicate.Location = new Point(238, 92);
            checkBoxPatientDublicate.Margin = new Padding(4, 3, 4, 3);
            checkBoxPatientDublicate.Name = "checkBoxPatientDublicate";
            checkBoxPatientDublicate.Size = new Size(162, 19);
            checkBoxPatientDublicate.TabIndex = 5;
            checkBoxPatientDublicate.Text = "Overwrite the same data ?";
            checkBoxPatientDublicate.UseVisualStyleBackColor = true;
            checkBoxPatientDublicate.CheckedChanged += checkBoxPatientDublicate_CheckedChanged;
            // 
            // BtnDownload
            // 
            BtnDownload.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnDownload.Location = new Point(479, 19);
            BtnDownload.Margin = new Padding(4, 3, 4, 3);
            BtnDownload.Name = "BtnDownload";
            BtnDownload.Size = new Size(74, 25);
            BtnDownload.TabIndex = 3;
            BtnDownload.Text = "Download";
            BtnDownload.UseVisualStyleBackColor = true;
            BtnDownload.Click += BtnDownload_Click;
            // 
            // PatientFileUpload
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(comboBoxSoftwareType);
            Controls.Add(labelSoftwareType);
            Controls.Add(TimerTextBox);
            Controls.Add(label4);
            Controls.Add(BtnCancel);
            Controls.Add(BtnExit);
            Controls.Add(label1);
            Controls.Add(TxtAccessLog);
            Controls.Add(progressBar1);
            Controls.Add(TxtErrorLog);
            Controls.Add(label2);
            Controls.Add(checkBoxPatientDublicate);
            Controls.Add(ChkBoxHasHeader);
            Controls.Add(TextBoxFileName);
            Controls.Add(BtnChoose);
            Controls.Add(BtnDownload);
            Controls.Add(BtnUpload);
            Controls.Add(labelFileName);
            Margin = new Padding(4, 3, 4, 3);
            Name = "PatientFileUpload";
            Size = new Size(562, 687);
            Load += PatientFileUpload_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox ChkBoxHasHeader;
        private TextBox TextBoxFileName;
        private Button BtnChoose;
        private Button BtnUpload;
        private Label labelFileName;
        private Label label1;
        private TextBox TxtAccessLog;
        private ProgressBar progressBar1;
        private TextBox TxtErrorLog;
        private Label label2;
        private Label TimerTextBox;
        private Label label4;
        private Button BtnCancel;
        private Button BtnExit;
        private Label labelSoftwareType;
        private ComboBox comboBoxSoftwareType;
        private CheckBox checkBoxPatientDublicate;
        private Button BtnDownload;
    }
}
