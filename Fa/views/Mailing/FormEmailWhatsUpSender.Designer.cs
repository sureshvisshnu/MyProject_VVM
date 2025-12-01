namespace Fa.views.Mailing
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmailWhatsUpSender));
            statusStrip1 = new StatusStrip();
            WorkStationErrorMsg = new ToolStripStatusLabel();
            BtnWorkStationExit = new Button();
            BtnWorkStationCancel = new Button();
            BtnWorkStationSave = new Button();
            WhatsupgroupBox = new GroupBox();
            TextBoxWhatsUpNo = new fa.views.controls.text.NameTextBoxAllowSpace(components);
            emailgroupBox = new GroupBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            TextBoxCCCMail = new fa.views.controls.text.NameTextBoxAllowSpace(components);
            TextBoxReceiverMAil = new fa.views.controls.text.NameTextBoxAllowSpace(components);
            TextBoxMailPassword = new fa.views.controls.text.NameTextBoxAllowSpace(components);
            TextBoxSendermail = new fa.views.controls.text.NameTextBoxAllowSpace(components);
            checkBoxGST = new CheckBox();
            statusStrip1.SuspendLayout();
            WhatsupgroupBox.SuspendLayout();
            emailgroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = SystemColors.Control;
            statusStrip1.Items.AddRange(new ToolStripItem[] { WorkStationErrorMsg });
            statusStrip1.Location = new Point(0, 409);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(284, 22);
            statusStrip1.TabIndex = 19;
            statusStrip1.Text = "statusStrip1";
            // 
            // WorkStationErrorMsg
            // 
            WorkStationErrorMsg.Name = "WorkStationErrorMsg";
            WorkStationErrorMsg.Size = new Size(34, 17);
            WorkStationErrorMsg.Text = "         ";
            // 
            // BtnWorkStationExit
            // 
            BtnWorkStationExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnWorkStationExit.Location = new Point(191, 373);
            BtnWorkStationExit.Name = "BtnWorkStationExit";
            BtnWorkStationExit.Size = new Size(75, 23);
            BtnWorkStationExit.TabIndex = 22;
            BtnWorkStationExit.Text = "Exit [F10]";
            BtnWorkStationExit.UseVisualStyleBackColor = true;
            // 
            // BtnWorkStationCancel
            // 
            BtnWorkStationCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnWorkStationCancel.Location = new Point(13, 373);
            BtnWorkStationCancel.Name = "BtnWorkStationCancel";
            BtnWorkStationCancel.Size = new Size(83, 23);
            BtnWorkStationCancel.TabIndex = 21;
            BtnWorkStationCancel.Text = "Cancel [Esc]";
            BtnWorkStationCancel.UseVisualStyleBackColor = true;
            // 
            // BtnWorkStationSave
            // 
            BtnWorkStationSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnWorkStationSave.Location = new Point(102, 373);
            BtnWorkStationSave.Name = "BtnWorkStationSave";
            BtnWorkStationSave.Size = new Size(83, 23);
            BtnWorkStationSave.TabIndex = 20;
            BtnWorkStationSave.Text = "Save [F8]";
            BtnWorkStationSave.UseVisualStyleBackColor = true;
            // 
            // WhatsupgroupBox
            // 
            WhatsupgroupBox.Controls.Add(TextBoxWhatsUpNo);
            WhatsupgroupBox.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            WhatsupgroupBox.Location = new Point(13, 22);
            WhatsupgroupBox.Name = "WhatsupgroupBox";
            WhatsupgroupBox.Size = new Size(265, 58);
            WhatsupgroupBox.TabIndex = 23;
            WhatsupgroupBox.TabStop = false;
            WhatsupgroupBox.Text = "What's Up No :";
            // 
            // TextBoxWhatsUpNo
            // 
            TextBoxWhatsUpNo.Location = new Point(10, 22);
            TextBoxWhatsUpNo.MaxLength = 150;
            TextBoxWhatsUpNo.Name = "TextBoxWhatsUpNo";
            TextBoxWhatsUpNo.Size = new Size(250, 21);
            TextBoxWhatsUpNo.TabIndex = 7;
            // 
            // emailgroupBox
            // 
            emailgroupBox.Controls.Add(label4);
            emailgroupBox.Controls.Add(label3);
            emailgroupBox.Controls.Add(label2);
            emailgroupBox.Controls.Add(label1);
            emailgroupBox.Controls.Add(TextBoxCCCMail);
            emailgroupBox.Controls.Add(TextBoxReceiverMAil);
            emailgroupBox.Controls.Add(TextBoxMailPassword);
            emailgroupBox.Controls.Add(TextBoxSendermail);
            emailgroupBox.Location = new Point(13, 111);
            emailgroupBox.Name = "emailgroupBox";
            emailgroupBox.Size = new Size(265, 245);
            emailgroupBox.TabIndex = 24;
            emailgroupBox.TabStop = false;
            emailgroupBox.Text = "E mail Details";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 192);
            label4.Name = "label4";
            label4.Size = new Size(96, 15);
            label4.TabIndex = 14;
            label4.Text = "Receiver CC Mail";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 141);
            label3.Name = "label3";
            label3.Size = new Size(83, 15);
            label3.TabIndex = 13;
            label3.Text = "Receiver Email";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 83);
            label2.Name = "label2";
            label2.Size = new Size(133, 15);
            label2.TabIndex = 12;
            label2.Text = "Sender E-Mail Password";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 25);
            label1.Name = "label1";
            label1.Size = new Size(75, 15);
            label1.TabIndex = 11;
            label1.Text = "Sender Email";
            // 
            // TextBoxCCCMail
            // 
            TextBoxCCCMail.Location = new Point(6, 211);
            TextBoxCCCMail.MaxLength = 150;
            TextBoxCCCMail.Name = "TextBoxCCCMail";
            TextBoxCCCMail.Size = new Size(250, 23);
            TextBoxCCCMail.TabIndex = 10;
            // 
            // TextBoxReceiverMAil
            // 
            TextBoxReceiverMAil.Location = new Point(6, 160);
            TextBoxReceiverMAil.MaxLength = 150;
            TextBoxReceiverMAil.Name = "TextBoxReceiverMAil";
            TextBoxReceiverMAil.Size = new Size(250, 23);
            TextBoxReceiverMAil.TabIndex = 9;
            // 
            // TextBoxMailPassword
            // 
            TextBoxMailPassword.Location = new Point(6, 102);
            TextBoxMailPassword.MaxLength = 150;
            TextBoxMailPassword.Name = "TextBoxMailPassword";
            TextBoxMailPassword.PasswordChar = '*';
            TextBoxMailPassword.Size = new Size(250, 23);
            TextBoxMailPassword.TabIndex = 8;
            // 
            // TextBoxSendermail
            // 
            TextBoxSendermail.Location = new Point(6, 44);
            TextBoxSendermail.MaxLength = 150;
            TextBoxSendermail.Name = "TextBoxSendermail";
            TextBoxSendermail.Size = new Size(250, 23);
            TextBoxSendermail.TabIndex = 7;
            // 
            // checkBoxGST
            // 
            checkBoxGST.AutoSize = true;
            checkBoxGST.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            checkBoxGST.Location = new Point(13, 86);
            checkBoxGST.Name = "checkBoxGST";
            checkBoxGST.Size = new Size(80, 17);
            checkBoxGST.TabIndex = 222;
            checkBoxGST.Text = "Send E mail";
            checkBoxGST.UseVisualStyleBackColor = true;
            // 
            // FormEmailWhatsUpSender
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 431);
            Controls.Add(checkBoxGST);
            Controls.Add(emailgroupBox);
            Controls.Add(WhatsupgroupBox);
            Controls.Add(BtnWorkStationExit);
            Controls.Add(BtnWorkStationCancel);
            Controls.Add(BtnWorkStationSave);
            Controls.Add(statusStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormEmailWhatsUpSender";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Email / WhatsUp Sending";
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(BtnWorkStationSave, 0);
            Controls.SetChildIndex(BtnWorkStationCancel, 0);
            Controls.SetChildIndex(BtnWorkStationExit, 0);
            Controls.SetChildIndex(WhatsupgroupBox, 0);
            Controls.SetChildIndex(emailgroupBox, 0);
            Controls.SetChildIndex(checkBoxGST, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(checkBoxIsPatient, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            WhatsupgroupBox.ResumeLayout(false);
            WhatsupgroupBox.PerformLayout();
            emailgroupBox.ResumeLayout(false);
            emailgroupBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusStrip1;
        private ToolStripStatusLabel WorkStationErrorMsg;
        private Button BtnWorkStationExit;
        private Button BtnWorkStationCancel;
        private Button BtnWorkStationSave;
        private GroupBox WhatsupgroupBox;
        private GroupBox emailgroupBox;
        private fa.views.controls.text.NameTextBoxAllowSpace TextBoxWhatsUpNo;
        private fa.views.controls.text.NameTextBoxAllowSpace TextBoxCCCMail;
        private fa.views.controls.text.NameTextBoxAllowSpace TextBoxReceiverMAil;
        private fa.views.controls.text.NameTextBoxAllowSpace TextBoxMailPassword;
        private fa.views.controls.text.NameTextBoxAllowSpace TextBoxSendermail;
        private Label label2;
        private Label label1;
        private Label label4;
        private Label label3;
        private CheckBox checkBoxGST;
    }
}