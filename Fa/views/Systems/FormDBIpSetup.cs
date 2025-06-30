using fa.model.OrderManagement;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;


namespace Fa.views.Systems
{
    public partial class FormDBIpSetup : Form
    {
        public static string SaveSuccessText = "Saved...";
        public static string EnterIpIdErrorMsg = "Please enter valid system name..";
        public string DBInfoMsg = null!;

        public FormDBIpSetup()
        {
            InitializeComponent();
            //TextBoxIpAddress.KeyPress += new KeyPressEventHandler(TextBoxIpAddress_KeyPress);
        }

        private void BtnWorkStationCancel_Click(object sender, EventArgs e)
        {
            DBIPSetupErrorMsg.Text = "";
            TextBoxIpAddress.ResetText();
            TextBoxIpAddress.Select();
        }

        private void BtnWorkStationSave_Click(object sender, EventArgs e)
        {
            //Cursor.Current = Cursors.WaitCursor;
            //if (ValidateForm())
            //{
            //    RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Ab2App");
            //    if (key != null)
            //    {
            //        key.Close();
            //        Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\Ab2App");
            //    }
            //    key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Ab2App");
            //    key.SetValue("DefaultIpAddress", string.IsNullOrEmpty(TextBoxIpAddress.Text) ? "localhost" : TextBoxIpAddress.Text);

            //    DBIPSetupErrorMsg.Text = SaveSuccessText;
            //}
            //Cursor.Current = Cursors.Default;

            Cursor.Current = Cursors.WaitCursor;
            if (ValidateForm())
            {
                RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Ab2App");
                if (key != null)
                {
                    key.Close();
                    Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\Ab2App");
                }
                key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Ab2App");
                key.SetValue("DefaultHostName", string.IsNullOrEmpty(TextBoxIpAddress.Text) ? "localhost" : TextBoxIpAddress.Text);

                DBIPSetupErrorMsg.Text = SaveSuccessText;
            }
            Cursor.Current = Cursors.Default;
        }
        private Boolean ValidateForm()
        {
            DBIPSetupErrorMsg.Text = "";

            string ipAddress = TextBoxIpAddress.Text.Trim();

            if (string.IsNullOrEmpty(ipAddress))
            {
                DBIPSetupErrorMsg.Text = EnterIpIdErrorMsg;
                TextBoxIpAddress.Select();
                return false;
            }

            //if (!IPAddress.TryParse(ipAddress, out IPAddress? parsedIp) || ipAddress == "127.0.0.1" || ipAddress == "::1")
            //{
            //    DBIPSetupErrorMsg.Text = EnterIpIdErrorMsg;
            //    TextBoxIpAddress.Select();
            //    return false;
            //}

            return true;
        }

        private void BtnIpIdSetupExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormDBIpSetup_Load(object sender, EventArgs e)
        {
            DBIPSetupErrorMsg.Text = DBInfoMsg;
        }

        private void TextBoxIpAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!char.IsControl(e.KeyChar))
            //{
            //    if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            //    {
            //        e.Handled = true;
            //    }
            //    else
            //    {
            //        int dotCount = (sender as TextBox)!.Text.Count(c => c == '.');
            //        if (e.KeyChar == '.' && dotCount >= 4)
            //        {
            //            e.Handled = true;
            //        }
            //    }
            //}
        }
    }
}
