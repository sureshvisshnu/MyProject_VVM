namespace fa
{
    partial class FormLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            label1 = new Label();
            label2 = new Label();
            TextBoxLoginLogin = new TextBox();
            TextBoxLoginPassword = new TextBox();
            BtnLoginCancel = new Button();
            BtnLoginLogin = new Button();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.Indigo;
            label1.Location = new Point(95, 17);
            label1.Name = "label1";
            label1.Size = new Size(37, 13);
            label1.TabIndex = 0;
            label1.Text = "Login";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.ForeColor = Color.Indigo;
            label2.Location = new Point(95, 58);
            label2.Name = "label2";
            label2.Size = new Size(61, 13);
            label2.TabIndex = 1;
            label2.Text = "Password";
            // 
            // TextBoxLoginLogin
            // 
            TextBoxLoginLogin.ForeColor = Color.RoyalBlue;
            TextBoxLoginLogin.Location = new Point(98, 33);
            TextBoxLoginLogin.MaxLength = 60;
            TextBoxLoginLogin.Name = "TextBoxLoginLogin";
            TextBoxLoginLogin.Size = new Size(260, 21);
            TextBoxLoginLogin.TabIndex = 2;
            TextBoxLoginLogin.TextChanged += TextBoxLoginLogin_TextChanged;
            TextBoxLoginLogin.KeyDown += TextBoxLoginLogin_KeyDown;
            TextBoxLoginLogin.KeyPress += TextBoxLoginLogin_KeyPress;
            TextBoxLoginLogin.MouseDown += TextBoxLoginLogin_MouseDown;
            // 
            // TextBoxLoginPassword
            // 
            TextBoxLoginPassword.ForeColor = Color.RoyalBlue;
            TextBoxLoginPassword.Location = new Point(98, 75);
            TextBoxLoginPassword.MaxLength = 60;
            TextBoxLoginPassword.Name = "TextBoxLoginPassword";
            TextBoxLoginPassword.PasswordChar = '*';
            TextBoxLoginPassword.Size = new Size(260, 21);
            TextBoxLoginPassword.TabIndex = 3;
            TextBoxLoginPassword.TextChanged += TextBoxLoginPassword_TextChanged;
            TextBoxLoginPassword.KeyDown += TextBoxLoginPassword_KeyDown;
            TextBoxLoginPassword.KeyPress += TextBoxLoginPassword_KeyPress;
            TextBoxLoginPassword.MouseDown += TextBoxLoginPassword_MouseDown;
            // 
            // BtnLoginCancel
            // 
            BtnLoginCancel.DialogResult = DialogResult.Cancel;
            BtnLoginCancel.Location = new Point(186, 110);
            BtnLoginCancel.Name = "BtnLoginCancel";
            BtnLoginCancel.Size = new Size(75, 23);
            BtnLoginCancel.TabIndex = 4;
            BtnLoginCancel.Text = "Cancel";
            BtnLoginCancel.UseVisualStyleBackColor = true;
            BtnLoginCancel.Click += BtnLoginCancel_Click;
            // 
            // BtnLoginLogin
            // 
            BtnLoginLogin.Enabled = false;
            BtnLoginLogin.Location = new Point(274, 110);
            BtnLoginLogin.Name = "BtnLoginLogin";
            BtnLoginLogin.Size = new Size(75, 23);
            BtnLoginLogin.TabIndex = 5;
            BtnLoginLogin.Text = "Login";
            BtnLoginLogin.UseVisualStyleBackColor = true;
            BtnLoginLogin.Click += BtnLoginLogin_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 24);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(77, 76);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BackgroundImage = (Image)resources.GetObject("pictureBox2.BackgroundImage");
            pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox2.Location = new Point(12, 24);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(80, 76);
            pictureBox2.TabIndex = 7;
            pictureBox2.TabStop = false;
            // 
            // FormLogin
            // 
            AcceptButton = BtnLoginLogin;
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLightLight;
            CancelButton = BtnLoginCancel;
            ClientSize = new Size(370, 147);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(BtnLoginLogin);
            Controls.Add(BtnLoginCancel);
            Controls.Add(TextBoxLoginPassword);
            Controls.Add(TextBoxLoginLogin);
            Controls.Add(label2);
            Controls.Add(label1);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormLogin";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Login";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextBoxLoginLogin;
        private System.Windows.Forms.TextBox TextBoxLoginPassword;
        private System.Windows.Forms.Button BtnLoginCancel;
        private System.Windows.Forms.Button BtnLoginLogin;
        private System.Windows.Forms.PictureBox pictureBox1;
        private PictureBox pictureBox2;
    }
}