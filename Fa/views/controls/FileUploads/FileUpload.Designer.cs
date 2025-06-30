namespace fa.views.controls.fileupload
{
    partial class FileUpload
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
            progressBar1 = new ProgressBar();
            ChkBoxHasHeader = new CheckBox();
            label4 = new Label();
            BtnCancel = new Button();
            TxtErrorLog = new TextBox();
            BtnExit = new Button();
            TextBoxFileName = new TextBox();
            BtnChoose = new Button();
            BtnUpload = new Button();
            label3 = new Label();
            label2 = new Label();
            TxtAccessLog = new TextBox();
            label1 = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            BGWorker = new System.ComponentModel.BackgroundWorker();
            TimerTextBox = new Label();
            checkBoxRefreshData = new CheckBox();
            SuspendLayout();
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(3, 89);
            progressBar1.Margin = new Padding(4, 3, 4, 3);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(556, 27);
            progressBar1.TabIndex = 66;
            // 
            // ChkBoxHasHeader
            // 
            ChkBoxHasHeader.AutoSize = true;
            ChkBoxHasHeader.Checked = true;
            ChkBoxHasHeader.CheckState = CheckState.Checked;
            ChkBoxHasHeader.Location = new Point(2, 48);
            ChkBoxHasHeader.Margin = new Padding(4, 3, 4, 3);
            ChkBoxHasHeader.Name = "ChkBoxHasHeader";
            ChkBoxHasHeader.Size = new Size(225, 19);
            ChkBoxHasHeader.TabIndex = 65;
            ChkBoxHasHeader.Text = "This file has header (Skip the first row)";
            ChkBoxHasHeader.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(4, 615);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(156, 15);
            label4.TabIndex = 63;
            label4.Text = "Time Duration (In Seconds) :";
            // 
            // BtnCancel
            // 
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(364, 609);
            BtnCancel.Margin = new Padding(4, 3, 4, 3);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(103, 27);
            BtnCancel.TabIndex = 62;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Visible = false;
            // 
            // TxtErrorLog
            // 
            TxtErrorLog.Location = new Point(1, 385);
            TxtErrorLog.Margin = new Padding(4, 3, 4, 3);
            TxtErrorLog.MaxLength = 294967295;
            TxtErrorLog.Multiline = true;
            TxtErrorLog.Name = "TxtErrorLog";
            TxtErrorLog.ReadOnly = true;
            TxtErrorLog.ScrollBars = ScrollBars.Vertical;
            TxtErrorLog.Size = new Size(558, 220);
            TxtErrorLog.TabIndex = 61;
            TxtErrorLog.WordWrap = false;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(474, 609);
            BtnExit.Margin = new Padding(4, 3, 4, 3);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(88, 27);
            BtnExit.TabIndex = 57;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            BtnExit.MouseLeave += BtnExit_MouseLeave;
            BtnExit.MouseHover += BtnExit_MouseHover;
            // 
            // TextBoxFileName
            // 
            TextBoxFileName.BackColor = SystemColors.Window;
            TextBoxFileName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxFileName.Location = new Point(2, 18);
            TextBoxFileName.Margin = new Padding(4, 3, 4, 3);
            TextBoxFileName.Name = "TextBoxFileName";
            TextBoxFileName.ReadOnly = true;
            TextBoxFileName.Size = new Size(415, 21);
            TextBoxFileName.TabIndex = 59;
            TextBoxFileName.TabStop = false;
            // 
            // BtnChoose
            // 
            BtnChoose.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnChoose.Location = new Point(424, 16);
            BtnChoose.Margin = new Padding(2);
            BtnChoose.Name = "BtnChoose";
            BtnChoose.Size = new Size(63, 27);
            BtnChoose.TabIndex = 55;
            BtnChoose.Text = "Browse";
            BtnChoose.UseVisualStyleBackColor = true;
            BtnChoose.Click += BtnChoose_Click;
            // 
            // BtnUpload
            // 
            BtnUpload.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnUpload.Location = new Point(492, 16);
            BtnUpload.Margin = new Padding(4, 3, 4, 3);
            BtnUpload.Name = "BtnUpload";
            BtnUpload.Size = new Size(68, 27);
            BtnUpload.TabIndex = 56;
            BtnUpload.Text = "Upload";
            BtnUpload.UseVisualStyleBackColor = true;
            BtnUpload.Click += BtnUpload_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(1, 2);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(53, 13);
            label3.TabIndex = 60;
            label3.Text = "File Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(4, 122);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(60, 13);
            label2.TabIndex = 58;
            label2.Text = "Access Log";
            // 
            // TxtAccessLog
            // 
            TxtAccessLog.Location = new Point(2, 143);
            TxtAccessLog.Margin = new Padding(4, 3, 4, 3);
            TxtAccessLog.MaxLength = 294967295;
            TxtAccessLog.Multiline = true;
            TxtAccessLog.Name = "TxtAccessLog";
            TxtAccessLog.ReadOnly = true;
            TxtAccessLog.ScrollBars = ScrollBars.Vertical;
            TxtAccessLog.Size = new Size(558, 220);
            TxtAccessLog.TabIndex = 67;
            TxtAccessLog.WordWrap = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(5, 367);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(51, 13);
            label1.TabIndex = 68;
            label1.Text = "Error Log";
            // 
            // timer1
            // 
            timer1.Interval = 1000;
            // 
            // TimerTextBox
            // 
            TimerTextBox.AutoSize = true;
            TimerTextBox.Location = new Point(173, 615);
            TimerTextBox.Margin = new Padding(4, 0, 4, 0);
            TimerTextBox.Name = "TimerTextBox";
            TimerTextBox.Size = new Size(49, 15);
            TimerTextBox.TabIndex = 70;
            TimerTextBox.Text = "00.00.00";
            // 
            // checkBoxRefreshData
            // 
            checkBoxRefreshData.AutoSize = true;
            checkBoxRefreshData.Location = new Point(2, 68);
            checkBoxRefreshData.Margin = new Padding(4, 3, 4, 3);
            checkBoxRefreshData.Name = "checkBoxRefreshData";
            checkBoxRefreshData.Size = new Size(107, 19);
            checkBoxRefreshData.TabIndex = 65;
            checkBoxRefreshData.Text = "Refresh all Data";
            checkBoxRefreshData.UseVisualStyleBackColor = true;
            checkBoxRefreshData.Visible = false;
            checkBoxRefreshData.CheckedChanged += checkBoxRefreshData_CheckedChanged;
            // 
            // FileUpload
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(TimerTextBox);
            Controls.Add(label1);
            Controls.Add(TxtAccessLog);
            Controls.Add(progressBar1);
            Controls.Add(checkBoxRefreshData);
            Controls.Add(ChkBoxHasHeader);
            Controls.Add(label4);
            Controls.Add(BtnCancel);
            Controls.Add(TxtErrorLog);
            Controls.Add(BtnExit);
            Controls.Add(TextBoxFileName);
            Controls.Add(BtnChoose);
            Controls.Add(BtnUpload);
            Controls.Add(label3);
            Controls.Add(label2);
            Margin = new Padding(4, 3, 4, 3);
            Name = "FileUpload";
            Size = new Size(562, 639);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox ChkBoxHasHeader;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.TextBox TxtErrorLog;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.TextBox TextBoxFileName;
        private System.Windows.Forms.Button BtnChoose;
        private System.Windows.Forms.Button BtnUpload;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtAccessLog;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Timer timer1;
        private System.ComponentModel.BackgroundWorker BGWorker;
        private System.Windows.Forms.Label TimerTextBox;
        private CheckBox checkBoxRefreshData;
    }
}
