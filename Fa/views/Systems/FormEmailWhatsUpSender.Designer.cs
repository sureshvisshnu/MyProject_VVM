namespace Fa.views.Systems
{
    partial class FormEmailWhatsUpSender
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmailWhatsUpSender));
            TextBoxReceiverWhatsUpNo = new TextBox();
            BtnWhatsUEmailExit = new Button();
            BtnWhatsUpEmailSend = new Button();
            BtnWhatsUpEmailCancel = new Button();
            TextBoxReceiverEmailAddress = new TextBox();
            TextBoxMailPassWord = new TextBox();
            TextBoxSnderEmailAddress = new TextBox();
            groupBox1 = new GroupBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            LabelCustomerDisplayAs = new Label();
            TextBoxCCEmailAddress = new TextBox();
            LabelCustomerName = new Label();
            Print = new StatusStrip();
            WhatsUpEmailErrorMsg = new ToolStripStatusLabel();
            label4 = new Label();
            FileName = new Label();
            CheckBoxSendEmail = new CheckBox();
            groupBox1.SuspendLayout();
            Print.SuspendLayout();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Location = new Point(125, 129);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Location = new Point(125, 99);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Location = new Point(125, 69);
            // 
            // checkBoxIsPatient
            // 
            checkBoxIsPatient.Location = new Point(180, 17);
            // 
            // TextBoxReceiverWhatsUpNo
            // 
            TextBoxReceiverWhatsUpNo.BackColor = SystemColors.Window;
            TextBoxReceiverWhatsUpNo.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxReceiverWhatsUpNo.Location = new Point(14, 60);
            TextBoxReceiverWhatsUpNo.MaxLength = 20;
            TextBoxReceiverWhatsUpNo.Name = "TextBoxReceiverWhatsUpNo";
            TextBoxReceiverWhatsUpNo.Size = new Size(237, 21);
            TextBoxReceiverWhatsUpNo.TabIndex = 7;
            // 
            // BtnWhatsUEmailExit
            // 
            BtnWhatsUEmailExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnWhatsUEmailExit.Location = new Point(166, 289);
            BtnWhatsUEmailExit.Name = "BtnWhatsUEmailExit";
            BtnWhatsUEmailExit.Size = new Size(75, 23);
            BtnWhatsUEmailExit.TabIndex = 34;
            BtnWhatsUEmailExit.Text = "Exit [F10]";
            BtnWhatsUEmailExit.UseVisualStyleBackColor = true;
            // 
            // BtnWhatsUpEmailSend
            // 
            BtnWhatsUpEmailSend.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnWhatsUpEmailSend.Location = new Point(100, 289);
            BtnWhatsUpEmailSend.Name = "BtnWhatsUpEmailSend";
            BtnWhatsUpEmailSend.Size = new Size(60, 23);
            BtnWhatsUpEmailSend.TabIndex = 32;
            BtnWhatsUpEmailSend.Text = "Send";
            BtnWhatsUpEmailSend.UseVisualStyleBackColor = true;
            BtnWhatsUpEmailSend.Click += BtnWhatsUpEmailSend_Click;
            // 
            // BtnWhatsUpEmailCancel
            // 
            BtnWhatsUpEmailCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnWhatsUpEmailCancel.Location = new Point(11, 289);
            BtnWhatsUpEmailCancel.Name = "BtnWhatsUpEmailCancel";
            BtnWhatsUpEmailCancel.Size = new Size(83, 23);
            BtnWhatsUpEmailCancel.TabIndex = 33;
            BtnWhatsUpEmailCancel.Text = "Cancel [Esc]";
            BtnWhatsUpEmailCancel.UseVisualStyleBackColor = true;
            // 
            // TextBoxReceiverEmailAddress
            // 
            TextBoxReceiverEmailAddress.BackColor = SystemColors.Window;
            TextBoxReceiverEmailAddress.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxReceiverEmailAddress.Location = new Point(6, 110);
            TextBoxReceiverEmailAddress.MaxLength = 100;
            TextBoxReceiverEmailAddress.Name = "TextBoxReceiverEmailAddress";
            TextBoxReceiverEmailAddress.Size = new Size(221, 21);
            TextBoxReceiverEmailAddress.TabIndex = 35;
            // 
            // TextBoxMailPassWord
            // 
            TextBoxMailPassWord.BackColor = SystemColors.Window;
            TextBoxMailPassWord.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxMailPassWord.Location = new Point(6, 72);
            TextBoxMailPassWord.MaxLength = 100;
            TextBoxMailPassWord.Name = "TextBoxMailPassWord";
            TextBoxMailPassWord.PasswordChar = '*';
            TextBoxMailPassWord.Size = new Size(221, 21);
            TextBoxMailPassWord.TabIndex = 36;
            // 
            // TextBoxSnderEmailAddress
            // 
            TextBoxSnderEmailAddress.BackColor = SystemColors.Window;
            TextBoxSnderEmailAddress.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxSnderEmailAddress.Location = new Point(6, 34);
            TextBoxSnderEmailAddress.MaxLength = 100;
            TextBoxSnderEmailAddress.Name = "TextBoxSnderEmailAddress";
            TextBoxSnderEmailAddress.Size = new Size(221, 21);
            TextBoxSnderEmailAddress.TabIndex = 37;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(LabelCustomerDisplayAs);
            groupBox1.Controls.Add(TextBoxCCEmailAddress);
            groupBox1.Controls.Add(TextBoxSnderEmailAddress);
            groupBox1.Controls.Add(TextBoxReceiverEmailAddress);
            groupBox1.Controls.Add(TextBoxMailPassWord);
            groupBox1.Location = new Point(14, 105);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(237, 180);
            groupBox1.TabIndex = 38;
            groupBox1.TabStop = false;
            groupBox1.Text = "Email";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(6, 95);
            label3.Name = "label3";
            label3.Size = new Size(76, 13);
            label3.TabIndex = 42;
            label3.Text = "Receiver Email";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(6, 57);
            label2.Name = "label2";
            label2.Size = new Size(55, 13);
            label2.TabIndex = 41;
            label2.Text = "PassWord";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(6, 133);
            label1.Name = "label1";
            label1.Size = new Size(94, 13);
            label1.TabIndex = 40;
            label1.Text = "Receiver CC Amail";
            // 
            // LabelCustomerDisplayAs
            // 
            LabelCustomerDisplayAs.AutoSize = true;
            LabelCustomerDisplayAs.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelCustomerDisplayAs.Location = new Point(6, 19);
            LabelCustomerDisplayAs.Name = "LabelCustomerDisplayAs";
            LabelCustomerDisplayAs.Size = new Size(41, 13);
            LabelCustomerDisplayAs.TabIndex = 39;
            LabelCustomerDisplayAs.Text = "Sender";
            // 
            // TextBoxCCEmailAddress
            // 
            TextBoxCCEmailAddress.BackColor = SystemColors.Window;
            TextBoxCCEmailAddress.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxCCEmailAddress.Location = new Point(6, 148);
            TextBoxCCEmailAddress.MaxLength = 100;
            TextBoxCCEmailAddress.Name = "TextBoxCCEmailAddress";
            TextBoxCCEmailAddress.Size = new Size(221, 21);
            TextBoxCCEmailAddress.TabIndex = 38;
            // 
            // LabelCustomerName
            // 
            LabelCustomerName.AutoSize = true;
            LabelCustomerName.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelCustomerName.Location = new Point(14, 45);
            LabelCustomerName.Name = "LabelCustomerName";
            LabelCustomerName.Size = new Size(81, 13);
            LabelCustomerName.TabIndex = 39;
            LabelCustomerName.Text = "What's Up No";
            // 
            // Print
            // 
            Print.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Print.Items.AddRange(new ToolStripItem[] { WhatsUpEmailErrorMsg });
            Print.Location = new Point(0, 324);
            Print.Name = "Print";
            Print.Size = new Size(263, 22);
            Print.TabIndex = 40;
            // 
            // WhatsUpEmailErrorMsg
            // 
            WhatsUpEmailErrorMsg.Name = "WhatsUpEmailErrorMsg";
            WhatsUpEmailErrorMsg.Size = new Size(31, 17);
            WhatsUpEmailErrorMsg.Text = "        ";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(14, 5);
            label4.Name = "label4";
            label4.Size = new Size(77, 13);
            label4.TabIndex = 43;
            label4.Text = "File Attached :";
            // 
            // FileName
            // 
            FileName.AutoSize = true;
            FileName.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FileName.Location = new Point(14, 25);
            FileName.Name = "FileName";
            FileName.Size = new Size(112, 13);
            FileName.TabIndex = 44;
            FileName.Text = "Name of Attached File";
            // 
            // CheckBoxSendEmail
            // 
            CheckBoxSendEmail.AutoSize = true;
            CheckBoxSendEmail.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            CheckBoxSendEmail.Location = new Point(14, 85);
            CheckBoxSendEmail.Name = "CheckBoxSendEmail";
            CheckBoxSendEmail.Size = new Size(77, 17);
            CheckBoxSendEmail.TabIndex = 45;
            CheckBoxSendEmail.Text = "Send Email";
            CheckBoxSendEmail.UseVisualStyleBackColor = true;
            // 
            // FormEmailWhatsUpSender
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(263, 346);
            Controls.Add(CheckBoxSendEmail);
            Controls.Add(FileName);
            Controls.Add(label4);
            Controls.Add(Print);
            Controls.Add(LabelCustomerName);
            Controls.Add(groupBox1);
            Controls.Add(BtnWhatsUEmailExit);
            Controls.Add(BtnWhatsUpEmailSend);
            Controls.Add(BtnWhatsUpEmailCancel);
            Controls.Add(TextBoxReceiverWhatsUpNo);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormEmailWhatsUpSender";
            StartPosition = FormStartPosition.CenterParent;
            Text = "What's Up  / E-mail Sender";
            Load += FormEmailWhatsUpSender_Load;
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(checkBoxIsPatient, 0);
            Controls.SetChildIndex(TextBoxReceiverWhatsUpNo, 0);
            Controls.SetChildIndex(BtnWhatsUpEmailCancel, 0);
            Controls.SetChildIndex(BtnWhatsUpEmailSend, 0);
            Controls.SetChildIndex(BtnWhatsUEmailExit, 0);
            Controls.SetChildIndex(groupBox1, 0);
            Controls.SetChildIndex(LabelCustomerName, 0);
            Controls.SetChildIndex(Print, 0);
            Controls.SetChildIndex(label4, 0);
            Controls.SetChildIndex(FileName, 0);
            Controls.SetChildIndex(CheckBoxSendEmail, 0);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            Print.ResumeLayout(false);
            Print.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox TextBoxReceiverWhatsUpNo;
        private Button BtnWhatsUEmailExit;
        private Button BtnWhatsUpEmailSend;
        private Button BtnWhatsUpEmailCancel;
        private TextBox TextBoxReceiverEmailAddress;
        private TextBox TextBoxMailPassWord;
        private TextBox TextBoxSnderEmailAddress;
        private GroupBox groupBox1;
        private TextBox TextBoxCCEmailAddress;
        private Label LabelCustomerName;
        private StatusStrip Print;
        private ToolStripStatusLabel WhatsUpEmailErrorMsg;
        private Label label3;
        private Label label2;
        private Label label1;
        private Label LabelCustomerDisplayAs;
        private Label label4;
        private Label FileName;
        private CheckBox CheckBoxSendEmail;
    }
}