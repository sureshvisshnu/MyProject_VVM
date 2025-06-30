namespace Fa.views.inventory
{
    partial class FormInventoryUpload
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInventoryUpload));
            LabelFileName = new Label();
            TextBoxFileName = new TextBox();
            BtnBrowse = new Button();
            BtnUpload = new Button();
            InventoryUploadprogressBar = new ProgressBar();
            Accesslabel = new Label();
            textBoxAccessLog = new TextBox();
            labelErrorLog = new Label();
            textBoxErrorLog = new TextBox();
            BtnCancel = new Button();
            BtnExit = new Button();
            labelTimeDuration = new Label();
            LabelTimeTacken = new Label();
            statusStrip1 = new StatusStrip();
            ErrorMsgStockUpload = new ToolStripStatusLabel();
            InventoryUploadTimer = new System.Windows.Forms.Timer(components);
            BtnDownload = new Button();
            BackgroundWorker = new System.ComponentModel.BackgroundWorker();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // LabelFileName
            // 
            LabelFileName.AutoSize = true;
            LabelFileName.Location = new Point(12, 9);
            LabelFileName.Name = "LabelFileName";
            LabelFileName.Size = new Size(53, 13);
            LabelFileName.TabIndex = 0;
            LabelFileName.Text = "File Name";
            // 
            // TextBoxFileName
            // 
            TextBoxFileName.BackColor = Color.White;
            TextBoxFileName.Location = new Point(12, 25);
            TextBoxFileName.Name = "TextBoxFileName";
            TextBoxFileName.ReadOnly = true;
            TextBoxFileName.Size = new Size(280, 21);
            TextBoxFileName.TabIndex = 1;
            TextBoxFileName.TabStop = false;
            TextBoxFileName.WordWrap = false;
            // 
            // BtnBrowse
            // 
            BtnBrowse.Location = new Point(297, 23);
            BtnBrowse.Margin = new Padding(2);
            BtnBrowse.Name = "BtnBrowse";
            BtnBrowse.Size = new Size(58, 23);
            BtnBrowse.TabIndex = 2;
            BtnBrowse.Text = "Browse";
            BtnBrowse.UseVisualStyleBackColor = true;
            BtnBrowse.Click += BtnBrowse_Click;
            // 
            // BtnUpload
            // 
            BtnUpload.Location = new Point(360, 23);
            BtnUpload.Name = "BtnUpload";
            BtnUpload.Size = new Size(58, 23);
            BtnUpload.TabIndex = 3;
            BtnUpload.Text = "Upload";
            BtnUpload.UseVisualStyleBackColor = true;
            BtnUpload.Click += BtnUpload_Click;
            // 
            // InventoryUploadprogressBar
            // 
            InventoryUploadprogressBar.Location = new Point(12, 52);
            InventoryUploadprogressBar.Name = "InventoryUploadprogressBar";
            InventoryUploadprogressBar.Size = new Size(479, 23);
            InventoryUploadprogressBar.TabIndex = 4;
            // 
            // Accesslabel
            // 
            Accesslabel.AutoSize = true;
            Accesslabel.Location = new Point(12, 78);
            Accesslabel.Name = "Accesslabel";
            Accesslabel.Size = new Size(24, 13);
            Accesslabel.TabIndex = 0;
            Accesslabel.Text = "Log";
            // 
            // textBoxAccessLog
            // 
            textBoxAccessLog.Location = new Point(12, 94);
            textBoxAccessLog.MaxLength = 294967295;
            textBoxAccessLog.Multiline = true;
            textBoxAccessLog.Name = "textBoxAccessLog";
            textBoxAccessLog.ReadOnly = true;
            textBoxAccessLog.ScrollBars = ScrollBars.Vertical;
            textBoxAccessLog.Size = new Size(479, 185);
            textBoxAccessLog.TabIndex = 1;
            textBoxAccessLog.WordWrap = false;
            // 
            // labelErrorLog
            // 
            labelErrorLog.AutoSize = true;
            labelErrorLog.Location = new Point(12, 282);
            labelErrorLog.Name = "labelErrorLog";
            labelErrorLog.Size = new Size(31, 13);
            labelErrorLog.TabIndex = 0;
            labelErrorLog.Text = "Error";
            // 
            // textBoxErrorLog
            // 
            textBoxErrorLog.BackColor = SystemColors.Control;
            textBoxErrorLog.Location = new Point(12, 298);
            textBoxErrorLog.MaxLength = 294967295;
            textBoxErrorLog.Multiline = true;
            textBoxErrorLog.Name = "textBoxErrorLog";
            textBoxErrorLog.ReadOnly = true;
            textBoxErrorLog.ScrollBars = ScrollBars.Vertical;
            textBoxErrorLog.Size = new Size(479, 223);
            textBoxErrorLog.TabIndex = 1;
            textBoxErrorLog.WordWrap = false;
            // 
            // BtnCancel
            // 
            BtnCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(322, 527);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(88, 23);
            BtnCancel.TabIndex = 2;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Visible = false;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // BtnExit
            // 
            BtnExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnExit.Location = new Point(416, 527);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(75, 23);
            BtnExit.TabIndex = 3;
            BtnExit.Text = "Exit [F10]";
            BtnExit.UseVisualStyleBackColor = true;
            BtnExit.Click += BtnExit_Click;
            // 
            // labelTimeDuration
            // 
            labelTimeDuration.AutoSize = true;
            labelTimeDuration.Location = new Point(12, 532);
            labelTimeDuration.Name = "labelTimeDuration";
            labelTimeDuration.Size = new Size(144, 13);
            labelTimeDuration.TabIndex = 0;
            labelTimeDuration.Text = "Time Duration (In Seconds) :";
            // 
            // LabelTimeTacken
            // 
            LabelTimeTacken.AutoSize = true;
            LabelTimeTacken.Location = new Point(153, 532);
            LabelTimeTacken.Name = "LabelTimeTacken";
            LabelTimeTacken.Size = new Size(29, 13);
            LabelTimeTacken.TabIndex = 0;
            LabelTimeTacken.Text = "0.00";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsgStockUpload });
            statusStrip1.Location = new Point(0, 558);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(503, 22);
            statusStrip1.TabIndex = 5;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsgStockUpload
            // 
            ErrorMsgStockUpload.Name = "ErrorMsgStockUpload";
            ErrorMsgStockUpload.Size = new Size(43, 17);
            ErrorMsgStockUpload.Text = "            ";
            // 
            // InventoryUploadTimer
            // 
            InventoryUploadTimer.Interval = 60000;
            // 
            // BtnDownload
            // 
            BtnDownload.Location = new Point(424, 23);
            BtnDownload.Name = "BtnDownload";
            BtnDownload.Size = new Size(67, 23);
            BtnDownload.TabIndex = 3;
            BtnDownload.Text = "Download";
            BtnDownload.UseVisualStyleBackColor = true;
            BtnDownload.Click += BtnDownload_Click;
            // 
            // BackgroundWorker
            // 
            BackgroundWorker.DoWork += BackgroundWorker_DoWork;
            // 
            // FormInventoryUpload
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(503, 580);
            Controls.Add(statusStrip1);
            Controls.Add(InventoryUploadprogressBar);
            Controls.Add(BtnExit);
            Controls.Add(BtnDownload);
            Controls.Add(BtnUpload);
            Controls.Add(BtnCancel);
            Controls.Add(BtnBrowse);
            Controls.Add(textBoxErrorLog);
            Controls.Add(textBoxAccessLog);
            Controls.Add(TextBoxFileName);
            Controls.Add(LabelTimeTacken);
            Controls.Add(labelTimeDuration);
            Controls.Add(labelErrorLog);
            Controls.Add(Accesslabel);
            Controls.Add(LabelFileName);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormInventoryUpload";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Stock Upload";
            FormClosing += FormInventoryUpload_FormClosing;
            Load += FormInventoryUpload_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label LabelFileName;
        private TextBox TextBoxFileName;
        private Button BtnBrowse;
        private Button BtnUpload;
        private ProgressBar InventoryUploadprogressBar;
        private Button BtnInsertProductFileImport;
        private Button InsertProductSampleDownload;
        private Button InsertAccountSampleDownLoad;
        private Label Accesslabel;
        private TextBox textBoxAccessLog;
        private Label labelErrorLog;
        private TextBox textBoxErrorLog;
        private Button BtnCancel;
        private Button BtnExit;
        private Label labelTimeDuration;
        private Label LabelTimeTacken;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel ErrorMsgStockUpload;
        private System.Windows.Forms.Timer InventoryUploadTimer;
        private Button BtnDownload;
        private System.ComponentModel.BackgroundWorker BackgroundWorker;
    }
}