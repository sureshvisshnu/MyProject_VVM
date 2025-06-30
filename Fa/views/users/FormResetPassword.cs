using fa.api.UserProfile;
using System;
using System.Windows.Forms;
using fa.model.UserProfile;
using fa.api.security;
using fa.libraries.Validation;

namespace fa.views.users
{
    public partial class FormResetPassword : Form
    {
        public static string SaveSuccess = "Password change successful.";
        public static string EnterCurrentPassWrongErrorMsg = "CurrentPassword is Wrong, Please verify your CurrentPassword !";
        public static string EnterCurrentPassErrorMsg = "Please enter current password";
        public static string EnterNewPassErrorMsg = "Please enter New password";
        public static string EnterConfirmPassErrorMsg = "Please enter Reenter password";
        public static string PassMismatchErrorMsg = "New Password and Reenter password are not match,\n Please enter Correctly";
        UserManager UserManager = null;
        Encryptor Encryptor = null;
        KeypressValidation KeypressValidation = null;
        public FormResetPassword()
        {
            UserManager = UserManager.Instance;
            Encryptor = new Encryptor();
            KeypressValidation = KeypressValidation.Instance;
            InitializeComponent();
        }
        private void BtnResetPasswordCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            TextBoxResetPasswordCurrentPassword.Select();
        }
        private void BtnResetPasswordReset_Click(object sender, EventArgs e)
        {
            User User = UserManager.GetUserByLogin(Global.User.Login);
            if (User != null)
            {
                if (ValidateForm())
                {
                    if (User.Password == Encryptor.EncryptText(TextBoxResetPasswordCurrentPassword.Text))
                    {
                        User.Password = Encryptor.EncryptText(TextBoxResetPasswordNewPassword.Text);
                        User.IsResetPassword = false;
                        User UserPasswordChange = UserManager.UpdateUser(User);
                        //this.Hide();
                        MessageBox.Show(SaveSuccess);
                        this.Close();
                    }
                    else
                    {
                        ResetPasswordErrorMsg.Text = EnterCurrentPassWrongErrorMsg;
                        TextBoxResetPasswordCurrentPassword.Select();
                    }
                }
            }
            else
            {
                MessageBox.Show("Somthing went wrong, the selected user is not valid.");
                return;
            }
        }
        private Boolean ValidateForm()
        {
            ResetPasswordErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxResetPasswordCurrentPassword.Text))
            {
                ResetPasswordErrorMsg.Text = EnterCurrentPassErrorMsg;
                TextBoxResetPasswordCurrentPassword.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxResetPasswordNewPassword.Text))
            {
                ResetPasswordErrorMsg.Text = EnterNewPassErrorMsg;
                TextBoxResetPasswordNewPassword.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxResetPasswordReenterNewPassword.Text))
            {
                ResetPasswordErrorMsg.Text = EnterConfirmPassErrorMsg;
                TextBoxResetPasswordReenterNewPassword.Select();
                return false;
            }
            if (TextBoxResetPasswordNewPassword.Text != TextBoxResetPasswordReenterNewPassword.Text)
            {
                ResetPasswordErrorMsg.Text = PassMismatchErrorMsg;
                TextBoxResetPasswordReenterNewPassword.Select();
                return false;
            }
            return true;
        }
        private void ResetForm()
        {
            TextBoxResetPasswordCurrentPassword.ResetText();
            TextBoxResetPasswordNewPassword.ResetText();
            TextBoxResetPasswordReenterNewPassword.ResetText();
        }
        private void TextBoxResetPasswordCurrentPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxResetPasswordNewPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxResetPasswordReenterNewPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }

        private void TextBoxResetPasswordCurrentPassword_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "NameChecking");

        }

        private void TextBoxResetPasswordCurrentPassword_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxResetPasswordCurrentPassword, "NameChecking");
            }
        }

        private void TextBoxResetPasswordNewPassword_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxResetPasswordNewPassword, "NameChecking");
            }
        }

        private void TextBoxResetPasswordReenterNewPassword_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxResetPasswordReenterNewPassword, "NameChecking");
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnResetPasswordReset.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
