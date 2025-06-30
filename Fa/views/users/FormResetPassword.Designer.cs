namespace fa.views.users
{
    partial class FormResetPassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormResetPassword));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            TextBoxResetPasswordCurrentPassword = new TextBox();
            TextBoxResetPasswordNewPassword = new TextBox();
            TextBoxResetPasswordReenterNewPassword = new TextBox();
            BtnResetPasswordReset = new Button();
            BtnResetPasswordCancel = new Button();
            Print = new StatusStrip();
            ResetPasswordErrorMsg = new ToolStripStatusLabel();
            Print.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(13, 8);
            label1.Name = "label1";
            label1.Size = new Size(107, 13);
            label1.TabIndex = 0;
            label1.Text = "Current Password";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(14, 50);
            label2.Name = "label2";
            label2.Size = new Size(87, 13);
            label2.TabIndex = 1;
            label2.Text = "New Password";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(11, 92);
            label3.Name = "label3";
            label3.Size = new Size(138, 13);
            label3.TabIndex = 2;
            label3.Text = "Re Enter New Password";
            // 
            // TextBoxResetPasswordCurrentPassword
            // 
            TextBoxResetPasswordCurrentPassword.Location = new Point(14, 25);
            TextBoxResetPasswordCurrentPassword.MaxLength = 60;
            TextBoxResetPasswordCurrentPassword.Name = "TextBoxResetPasswordCurrentPassword";
            TextBoxResetPasswordCurrentPassword.Size = new Size(329, 21);
            TextBoxResetPasswordCurrentPassword.TabIndex = 3;
            TextBoxResetPasswordCurrentPassword.UseSystemPasswordChar = true;
            TextBoxResetPasswordCurrentPassword.KeyDown += TextBoxResetPasswordCurrentPassword_KeyDown;
            TextBoxResetPasswordCurrentPassword.KeyPress += TextBoxResetPasswordCurrentPassword_KeyPress;
            TextBoxResetPasswordCurrentPassword.MouseDown += TextBoxResetPasswordCurrentPassword_MouseDown;
            // 
            // TextBoxResetPasswordNewPassword
            // 
            TextBoxResetPasswordNewPassword.Location = new Point(14, 67);
            TextBoxResetPasswordNewPassword.MaxLength = 60;
            TextBoxResetPasswordNewPassword.Name = "TextBoxResetPasswordNewPassword";
            TextBoxResetPasswordNewPassword.Size = new Size(329, 21);
            TextBoxResetPasswordNewPassword.TabIndex = 4;
            TextBoxResetPasswordNewPassword.UseSystemPasswordChar = true;
            TextBoxResetPasswordNewPassword.KeyDown += TextBoxResetPasswordCurrentPassword_KeyDown;
            TextBoxResetPasswordNewPassword.KeyPress += TextBoxResetPasswordNewPassword_KeyPress;
            TextBoxResetPasswordNewPassword.MouseDown += TextBoxResetPasswordNewPassword_MouseDown;
            // 
            // TextBoxResetPasswordReenterNewPassword
            // 
            TextBoxResetPasswordReenterNewPassword.Location = new Point(14, 110);
            TextBoxResetPasswordReenterNewPassword.MaxLength = 60;
            TextBoxResetPasswordReenterNewPassword.Name = "TextBoxResetPasswordReenterNewPassword";
            TextBoxResetPasswordReenterNewPassword.Size = new Size(329, 21);
            TextBoxResetPasswordReenterNewPassword.TabIndex = 5;
            TextBoxResetPasswordReenterNewPassword.UseSystemPasswordChar = true;
            TextBoxResetPasswordReenterNewPassword.KeyDown += TextBoxResetPasswordCurrentPassword_KeyDown;
            TextBoxResetPasswordReenterNewPassword.KeyPress += TextBoxResetPasswordReenterNewPassword_KeyPress;
            TextBoxResetPasswordReenterNewPassword.MouseDown += TextBoxResetPasswordReenterNewPassword_MouseDown;
            // 
            // BtnResetPasswordReset
            // 
            BtnResetPasswordReset.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnResetPasswordReset.Location = new Point(259, 142);
            BtnResetPasswordReset.Name = "BtnResetPasswordReset";
            BtnResetPasswordReset.Size = new Size(84, 23);
            BtnResetPasswordReset.TabIndex = 6;
            BtnResetPasswordReset.Text = "Reset [F8]";
            BtnResetPasswordReset.UseVisualStyleBackColor = true;
            BtnResetPasswordReset.Click += BtnResetPasswordReset_Click;
            // 
            // BtnResetPasswordCancel
            // 
            BtnResetPasswordCancel.DialogResult = DialogResult.Cancel;
            BtnResetPasswordCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnResetPasswordCancel.Location = new Point(169, 142);
            BtnResetPasswordCancel.Name = "BtnResetPasswordCancel";
            BtnResetPasswordCancel.Size = new Size(84, 23);
            BtnResetPasswordCancel.TabIndex = 7;
            BtnResetPasswordCancel.Text = "Cancel [Esc]";
            BtnResetPasswordCancel.UseVisualStyleBackColor = true;
            BtnResetPasswordCancel.Click += BtnResetPasswordCancel_Click;
            // 
            // Print
            // 
            Print.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Print.Items.AddRange(new ToolStripItem[] { ResetPasswordErrorMsg });
            Print.Location = new Point(0, 177);
            Print.Name = "Print";
            Print.Size = new Size(358, 22);
            Print.TabIndex = 12;
            // 
            // ResetPasswordErrorMsg
            // 
            ResetPasswordErrorMsg.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            ResetPasswordErrorMsg.Name = "ResetPasswordErrorMsg";
            ResetPasswordErrorMsg.Size = new Size(31, 17);
            ResetPasswordErrorMsg.Text = "        ";
            // 
            // FormResetPassword
            // 
            AcceptButton = BtnResetPasswordReset;
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            CancelButton = BtnResetPasswordCancel;
            ClientSize = new Size(358, 199);
            Controls.Add(TextBoxResetPasswordReenterNewPassword);
            Controls.Add(TextBoxResetPasswordCurrentPassword);
            Controls.Add(TextBoxResetPasswordNewPassword);
            Controls.Add(Print);
            Controls.Add(BtnResetPasswordCancel);
            Controls.Add(BtnResetPasswordReset);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormResetPassword";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Reset Password";
            Print.ResumeLayout(false);
            Print.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextBoxResetPasswordCurrentPassword;
        private System.Windows.Forms.TextBox TextBoxResetPasswordNewPassword;
        private System.Windows.Forms.TextBox TextBoxResetPasswordReenterNewPassword;
        private System.Windows.Forms.Button BtnResetPasswordReset;
        private System.Windows.Forms.Button BtnResetPasswordCancel;
        private System.Windows.Forms.StatusStrip Print;
        private System.Windows.Forms.ToolStripStatusLabel ResetPasswordErrorMsg;
    }
}