using System;
using System.Windows.Forms;
using fa.api.security;
using fa.api.UserProfile;
using fa.model.UserProfile;
using fa.views;
using fa.libraries.Validation;
using fa.api.Log;
using fa.model.Accounting.Masters;
namespace fa
{
    public partial class FormLogin : Form
    {
        public static string AccountLockErrorMsg = "Your Account is locked, Please contact administrator!";
        public static string LoginFailedErrorMsg = "Login Failed, Please verify your User Name/Password !";
        public static string UserNameFailedErrorMsg = "Login Failed, Please verify your User Name !";
        public static string PassWordFailedErrorMsg = "Login Failed, Please verify your Password !";


        Container parent;
        public FormLogin()
        {
            InitializeComponent();
        }
        public FormLogin(object sender)
        {
            InitializeComponent();
            LoadLoginLogo();
            parent = (Container)sender;
        }
        private void LoadLoginLogo()
        {
            if (Global.softwareType == SoftwareType.MEDICARE)
            {
                pictureBox1.Show();
                pictureBox2.Hide();
            }
            else
            {
                pictureBox1.Hide();
                pictureBox2.Show();
            }
        }
        private void CheckEnableLoginBtn()
        {
            if (TextBoxLoginLogin.Text.Length > 0 && TextBoxLoginPassword.Text.Length > 0)
            {
                BtnLoginLogin.Enabled = true;
            }
            else
            {
                BtnLoginLogin.Enabled = false;
            }
        }
        private void TextBoxLoginLogin_TextChanged(object sender, EventArgs e)
        {
            CheckEnableLoginBtn();
        }
        private void TextBoxLoginPassword_TextChanged(object sender, EventArgs e)
        {
            CheckEnableLoginBtn();
        }
        private void BtnLoginLogin_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var userName = TextBoxLoginLogin.Text;
            var password = TextBoxLoginPassword.Text;
            Encryptor encryptor = new Encryptor();
            try
            {
                UserManager UserManager = UserManager.Instance;
                User User = UserManager.GetUserByLogin(userName);
                if (User != null)
                {
                    if (User.Login == userName)
                    {
                        if(User.Password == encryptor.EncryptText(password))
                        {
                            Boolean allLoginConditionMet = true;
                            //should reset password..
                            if (User.IsLocked)
                            {
                                MessageBox.Show(AccountLockErrorMsg);
                                allLoginConditionMet = false;
                            }
                            if (allLoginConditionMet)
                            {
                                Global.User = User;
                                Global.isAuthenticated = true;
                                Global.isDateModification = User.IsSuperAdmin ? true : false;
                                fa.Data.Global.User = User;
                                fa.Data.Global.isAuthenticated = true;
                                //parent.SetMainMenu();
                                this.Hide();
                            }
                        }
                        else
                        {
                            Global.User = null;
                            Global.isAuthenticated = false;
                            MessageBox.Show(PassWordFailedErrorMsg);
                        }
                    }
                    else
                    {
                        Global.User = null;
                        Global.isAuthenticated = false;
                        MessageBox.Show(UserNameFailedErrorMsg);
                    }
                }
                else
                {
                    Global.User = null;
                    Global.isAuthenticated = false;
                    MessageBox.Show(LoginFailedErrorMsg);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Exception " + Ex.Message);
                Logger.LogError(Ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void BtnLoginCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        KeypressValidation KeypressValidation = KeypressValidation.Instance;
        private void TextBoxLoginLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_LoginChecking(sender, e);
        }
        private void TextBoxLoginPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }

        private void TextBoxLoginLogin_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "LoginChecking");

        }
        private void TextBoxLoginLogin_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxLoginLogin, "LoginChecking");
            }
        }

        private void TextBoxLoginPassword_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteChecking(sender, e, "NameChecking");

        }

        private void TextBoxLoginPassword_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenu(TextBoxLoginPassword, "NameChecking");
            }
        }
    }
}
