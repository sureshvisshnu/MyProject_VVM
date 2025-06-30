namespace fa.views.Systems
{
    partial class FormWorkStationSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWorkStationSetup));
            label1 = new Label();
            label2 = new Label();
            BtnWorkStationExit = new Button();
            BtnWorkStationCancel = new Button();
            BtnWorkStationSave = new Button();
            statusStrip1 = new StatusStrip();
            WorkStationErrorMsg = new ToolStripStatusLabel();
            groupBox1 = new GroupBox();
            ComboBoxDefaultStockLocation = new controls.ComboBoxSwapTextBox();
            ComboBoxDefaultPrinter = new controls.ComboBoxSwapTextBox();
            ComboBoxTockenPrinter = new controls.ComboBoxSwapTextBox();
            label7 = new Label();
            TextBoxIpAddress = new controls.text.NameTextBoxAllowSpace(components);
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            TextBoxWorkStationName = new controls.text.NameTextBoxAllowSpace(components);
            TextBoxWorkStationID = new controls.text.NameTextBox(components);
            label3 = new Label();
            buttonBackup = new Button();
            statusStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 15);
            label1.Name = "label1";
            label1.Size = new Size(83, 13);
            label1.TabIndex = 0;
            label1.Text = "Work Station ID";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(13, 60);
            label2.Name = "label2";
            label2.Size = new Size(99, 13);
            label2.TabIndex = 1;
            label2.Text = "Work Station Name";
            // 
            // BtnWorkStationExit
            // 
            BtnWorkStationExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnWorkStationExit.Location = new Point(308, 316);
            BtnWorkStationExit.Name = "BtnWorkStationExit";
            BtnWorkStationExit.Size = new Size(75, 23);
            BtnWorkStationExit.TabIndex = 10;
            BtnWorkStationExit.Text = "Exit [F10]";
            BtnWorkStationExit.UseVisualStyleBackColor = true;
            BtnWorkStationExit.Click += BtnWorkStationExit_Click;
            // 
            // BtnWorkStationCancel
            // 
            BtnWorkStationCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnWorkStationCancel.Location = new Point(130, 316);
            BtnWorkStationCancel.Name = "BtnWorkStationCancel";
            BtnWorkStationCancel.Size = new Size(83, 23);
            BtnWorkStationCancel.TabIndex = 8;
            BtnWorkStationCancel.Text = "Cancel [Esc]";
            BtnWorkStationCancel.UseVisualStyleBackColor = true;
            BtnWorkStationCancel.Click += BtnWorkStationCancel_Click;
            // 
            // BtnWorkStationSave
            // 
            BtnWorkStationSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnWorkStationSave.Location = new Point(219, 316);
            BtnWorkStationSave.Name = "BtnWorkStationSave";
            BtnWorkStationSave.Size = new Size(83, 23);
            BtnWorkStationSave.TabIndex = 7;
            BtnWorkStationSave.Text = "Save [F8]";
            BtnWorkStationSave.UseVisualStyleBackColor = true;
            BtnWorkStationSave.Click += BtnWorkStationSave_Click;
            BtnWorkStationSave.PreviewKeyDown += BtnWorkStationSave_PreviewKeyDown;
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = SystemColors.Control;
            statusStrip1.Items.AddRange(new ToolStripItem[] { WorkStationErrorMsg });
            statusStrip1.Location = new Point(0, 354);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(407, 22);
            statusStrip1.TabIndex = 18;
            statusStrip1.Text = "statusStrip1";
            // 
            // WorkStationErrorMsg
            // 
            WorkStationErrorMsg.Name = "WorkStationErrorMsg";
            WorkStationErrorMsg.Size = new Size(34, 17);
            WorkStationErrorMsg.Text = "         ";
            // 
            // groupBox1
            // 
            groupBox1.BackColor = SystemColors.Control;
            groupBox1.Controls.Add(ComboBoxTockenPrinter);
            groupBox1.Controls.Add(ComboBoxDefaultPrinter);
            groupBox1.Controls.Add(ComboBoxDefaultStockLocation);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(TextBoxIpAddress);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(TextBoxWorkStationName);
            groupBox1.Controls.Add(TextBoxWorkStationID);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label3);
            groupBox1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(384, 298);
            groupBox1.TabIndex = 19;
            groupBox1.TabStop = false;
            // 
            // ComboBoxDefaultStockLocation
            // 
            ComboBoxDefaultStockLocation.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxDefaultStockLocation.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxDefaultStockLocation.FormattingEnabled = true;
            ComboBoxDefaultStockLocation.Location = new Point(13, 212);
            ComboBoxDefaultStockLocation.MaxLength = 30;
            ComboBoxDefaultStockLocation.Name = "ComboBoxDefaultStockLocation";
            ComboBoxDefaultStockLocation.Size = new Size(200, 21);
            ComboBoxDefaultStockLocation.TabIndex = 5;
            ComboBoxDefaultStockLocation.TxtVisible = true;
            // 
            // ComboBoxDefaultPrinter
            // 
            ComboBoxDefaultPrinter.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxDefaultPrinter.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxDefaultPrinter.FormattingEnabled = true;
            ComboBoxDefaultPrinter.Location = new Point(13, 122);
            ComboBoxDefaultPrinter.MaxLength = 30;
            ComboBoxDefaultPrinter.Name = "ComboBoxDefaultPrinter";
            ComboBoxDefaultPrinter.Size = new Size(302, 21);
            ComboBoxDefaultPrinter.TabIndex = 3;
            ComboBoxDefaultPrinter.TxtVisible = true;
            // 
            // ComboBoxTockenPrinter
            // 
            ComboBoxTockenPrinter.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxTockenPrinter.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxTockenPrinter.FormattingEnabled = true;
            ComboBoxTockenPrinter.Location = new Point(13, 167);
            ComboBoxTockenPrinter.MaxLength = 30;
            ComboBoxTockenPrinter.Name = "ComboBoxTockenPrinter";
            ComboBoxTockenPrinter.Size = new Size(302, 21);
            ComboBoxTockenPrinter.TabIndex = 4;
            ComboBoxTockenPrinter.TxtVisible = true;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label7.ForeColor = Color.DarkGray;
            label7.Location = new Point(219, 261);
            label7.Name = "label7";
            label7.Size = new Size(155, 15);
            label7.TabIndex = 8;
            label7.Text = "Example DESKTOP-XXXXX";
            // 
            // TextBoxIpAddress
            // 
            TextBoxIpAddress.Location = new Point(13, 258);
            TextBoxIpAddress.MaxLength = 150;
            TextBoxIpAddress.Name = "TextBoxIpAddress";
            TextBoxIpAddress.Size = new Size(200, 21);
            TextBoxIpAddress.TabIndex = 6;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(13, 240);
            label6.Name = "label6";
            label6.Size = new Size(108, 13);
            label6.TabIndex = 5;
            label6.Text = "System/Server Name";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(13, 195);
            label5.Name = "label5";
            label5.Size = new Size(114, 13);
            label5.TabIndex = 4;
            label5.Text = "Default Stock Location";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(13, 150);
            label4.Name = "label4";
            label4.Size = new Size(71, 13);
            label4.TabIndex = 3;
            label4.Text = "Token Printer";
            // 
            // TextBoxWorkStationName
            // 
            TextBoxWorkStationName.Location = new Point(13, 77);
            TextBoxWorkStationName.MaxLength = 30;
            TextBoxWorkStationName.Name = "TextBoxWorkStationName";
            TextBoxWorkStationName.Size = new Size(190, 21);
            TextBoxWorkStationName.TabIndex = 2;
            // 
            // TextBoxWorkStationID
            // 
            TextBoxWorkStationID.Location = new Point(13, 32);
            TextBoxWorkStationID.MaxLength = 10;
            TextBoxWorkStationID.Name = "TextBoxWorkStationID";
            TextBoxWorkStationID.Size = new Size(100, 21);
            TextBoxWorkStationID.TabIndex = 1;
            TextBoxWorkStationID.PreviewKeyDown += TextBoxWorkStationID_PreviewKeyDown;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(13, 105);
            label3.Name = "label3";
            label3.Size = new Size(77, 13);
            label3.TabIndex = 2;
            label3.Text = "Default Printer";
            // 
            // buttonBackup
            // 
            buttonBackup.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonBackup.Location = new Point(35, 316);
            buttonBackup.Name = "buttonBackup";
            buttonBackup.Size = new Size(89, 23);
            buttonBackup.TabIndex = 9;
            buttonBackup.Text = " Backup [F6]";
            buttonBackup.UseVisualStyleBackColor = true;
            buttonBackup.Click += buttonBackup_Click;
            // 
            // FormWorkStationSetup
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(407, 376);
            Controls.Add(groupBox1);
            Controls.Add(statusStrip1);
            Controls.Add(BtnWorkStationExit);
            Controls.Add(buttonBackup);
            Controls.Add(BtnWorkStationCancel);
            Controls.Add(BtnWorkStationSave);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormWorkStationSetup";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Work Station Setup";
            Load += FormWorkStationSetup_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private controls.text.NameTextBoxAllowSpace TextBoxWorkStationName;
        private controls.text.NameTextBox TextBoxWorkStationID;
        private System.Windows.Forms.Button BtnWorkStationExit;
        private System.Windows.Forms.Button BtnWorkStationCancel;
        private System.Windows.Forms.Button BtnWorkStationSave;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel WorkStationErrorMsg;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private controls.ComboBoxSwapTextBox ComboBoxDefaultPrinter;
        private controls.ComboBoxSwapTextBox ComboBoxTockenPrinter;
        private System.Windows.Forms.Label label4;
        private controls.ComboBoxSwapTextBox ComboBoxDefaultStockLocation;
        private System.Windows.Forms.Label label5;
        private controls.text.NameTextBoxAllowSpace TextBoxIpAddress;
        private Label label6;
        private Label label7;
        private Button buttonBackup;
    }
}