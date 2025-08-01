using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Management;
using fa.libraries.utils;
using fa.model.OrderManagement;
using Fa.views.utils.Common;

namespace fa.views.Systems
{
    public partial class FormWorkStationSetup : Form
    {
        public static string SaveSuccessText = "Saved...";
        public static string EnterIdErrorMsg = "Please enter WorkStation ID";
        public static string EnterNameErrorMsg = "Please enter WorkStation Name";
        public static string ChoosePrinterErrorMsg = "Please Select Default Printer";
        public static string ChooseTockenPrinterErrorMsg = "Please Select Tocken Printer";


        public bool WorkStationSetupOnLoad = false;
        public FormWorkStationSetup()
        {
            InitializeComponent();
        }

        private void FormWorkStationSetup_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (Global.isLocalDB == false)
            {
                SetRemoteDBStatus();
            }
            Resetform();
            if (Global.isLocalDB == false && Global.isRemoteDB == false)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
                TextBoxIpAddress.Select();
            }
            else
            {
                TextBoxWorkStationID.Select();
            }
            Cursor.Current = Cursors.Default;

        }
        //List<string> GetAvailablePrinter()
        //{
        //    var PrinterList = new List<string>();

        //    var printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");
        //    foreach (var printer in printerQuery.Get())
        //    {
        //        var name = printer.Properties["Name"].Value;
        //        var status = printer.GetPropertyValue("Status");
        //        var isDefault = printer.GetPropertyValue("Default");
        //        var isNetworkPrinter = (bool) printer.GetPropertyValue("Network");               
        //        var CurrentUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\');
        //        PrinterList.Add(name.ToString());
        //    }
        //    return PrinterList;
        //}
        private void LoadWorkStation()
        {
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Ab2App");
            if (key != null)
            {
                if (key.GetValue("WorkStationID") != null)
                {
                    TextBoxWorkStationID.Text = key.GetValue("WorkStationID")?.ToString();
                }
                if (key.GetValue("WorkStationName") != null)
                {
                    TextBoxWorkStationName.Text = key.GetValue("WorkStationName")?.ToString();
                }
                if (key.GetValue("DefaultPrinter") != null && !string.IsNullOrEmpty(key.GetValue("DefaultPrinter")?.ToString()))
                {
                    ComboBoxDefaultPrinter.SelectedIndex = ComboBoxDefaultPrinter.FindStringExact(key.GetValue("DefaultPrinter")?.ToString());
                }
                else
                {
                    ComboBoxDefaultPrinter.SelectedIndex = ComboBoxDefaultPrinter.FindStringExact("None");
                }
                if (key.GetValue("TokenPrinter") != null && !string.IsNullOrEmpty(key.GetValue("TokenPrinter")?.ToString()))
                {
                    ComboBoxTockenPrinter.SelectedIndex = ComboBoxTockenPrinter.FindStringExact(key.GetValue("TokenPrinter").ToString());
                }
                else
                {
                    ComboBoxTockenPrinter.SelectedIndex = ComboBoxTockenPrinter.FindStringExact("None");
                }
                if (key.GetValue("StockLocation") != null && !string.IsNullOrEmpty(key.GetValue("StockLocation")?.ToString()))
                {
                    ComboBoxDefaultStockLocation.SelectedIndex = ComboBoxDefaultStockLocation.FindStringExact(key.GetValue("StockLocation").ToString());
                }
                else
                {
                    ComboBoxDefaultStockLocation.SelectedIndex = -1;
                }
                //if (key.GetValue("DefaultIpAddress") != null && !string.IsNullOrEmpty(key.GetValue("DefaultIpAddress")?.ToString()))
                //{
                //    TextBoxIpAddress.Text = key.GetValue("DefaultIpAddress")?.ToString();
                //}
                if (key.GetValue("DefaultHostName") != null && !string.IsNullOrEmpty(key.GetValue("DefaultHostName")?.ToString()))
                {
                    TextBoxIpAddress.Text = key.GetValue("DefaultHostName")?.ToString();
                }
                key.Close();
            }
        }
        private void TextBoxWorkStationID_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxWorkStationName.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnWorkStationSave.Select();
            }
        }

        private void BtnWorkStationSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxWorkStationID.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxIpAddress.Select();
            }
        }

        private void BtnWorkStationCancel_Click(object sender, EventArgs e)
        {
            Resetform();
            TextBoxWorkStationID.Select();
        }

        private void BtnWorkStationSave_Click(object sender, EventArgs e)
        {
            //if(ValidateForm())
            //{
            Cursor.Current = Cursors.WaitCursor;
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Ab2App");
            if (key != null)
            {
                key.Close();
                Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\Ab2App");
            }
            key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Ab2App");
            key.SetValue("WorkStationID", TextBoxWorkStationID.Text);
            key.SetValue("WorkStationName", TextBoxWorkStationName.Text.Trim());
            key.SetValue("DefaultPrinter", (ComboBoxDefaultPrinter.SelectedIndex < 1 ? "" : ComboBoxDefaultPrinter.Text));
            key.SetValue("TokenPrinter", (ComboBoxTockenPrinter.SelectedIndex < 1 ? "" : ComboBoxTockenPrinter.Text));
            key.SetValue("BarCodePrinter", (ComboBoxDefaultBarCodePrinter.SelectedIndex < 1 ? "" : ComboBoxDefaultBarCodePrinter.Text));
            key.SetValue("StockLocation", (ComboBoxDefaultStockLocation.SelectedIndex < 0 ? "" : ((InventoryLocation)ComboBoxDefaultStockLocation.Items[ComboBoxDefaultStockLocation.SelectedIndex]).Name));
            //key.SetValue("DefaultIpAddress", string.IsNullOrEmpty(TextBoxIpAddress.Text) ? "localhost" : TextBoxIpAddress.Text);
            key.SetValue("DefaultHostName", string.IsNullOrEmpty(TextBoxIpAddress.Text) ? "localhost" : TextBoxIpAddress.Text);

            Global.WorkStation = key;
            fa.Data.Global.DefaultHostName = key.GetValue("DefaultHostName")?.ToString() ?? Environment.MachineName;
            LoadWorkStation();
            WorkStationErrorMsg.Text = SaveSuccessText;
            if (WorkStationSetupOnLoad)
            {
                this.Close();
            }
            Cursor.Current = Cursors.Default;

            //}
        }
        private Boolean ValidateForm()
        {
            WorkStationErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxWorkStationID.Text.Trim()))
            {
                WorkStationErrorMsg.Text = EnterIdErrorMsg;
                TextBoxWorkStationID.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxWorkStationName.Text.Trim()))
            {
                WorkStationErrorMsg.Text = EnterNameErrorMsg;
                TextBoxWorkStationName.Select();
                return false;
            }
            if (string.IsNullOrEmpty(ComboBoxDefaultPrinter.Text.Trim()))
            {
                WorkStationErrorMsg.Text = ChoosePrinterErrorMsg;
                ComboBoxDefaultPrinter.Select();
                return false;
            }
            if (string.IsNullOrEmpty(ComboBoxTockenPrinter.Text.Trim()))
            {
                WorkStationErrorMsg.Text = ChoosePrinterErrorMsg;
                ComboBoxTockenPrinter.Select();
                return false;
            }
            return true;
        }
        private void BtnWorkStationExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Resetform()
        {
            WorkStationErrorMsg.Text = "";
            TextBoxWorkStationID.ResetText();
            TextBoxWorkStationName.ResetText();
            TextBoxIpAddress.ResetText();
            ComboBoxDefaultPrinter.Items.Clear();
            ComboBoxDefaultPrinter.Items.Add("None");
            ComboBoxDefaultPrinter.Items.AddRange(ComboUtils.GetAvailablePrinter().ToArray<string>());
            ComboBoxDefaultPrinter.SelectedIndex = 0;
            ComboBoxTockenPrinter.Items.Clear();
            ComboBoxTockenPrinter.Items.Add("None");
            ComboBoxDefaultBarCodePrinter.Items.Add("None");
            ComboBoxDefaultBarCodePrinter.Items.AddRange(ComboUtils.GetAvailablePrinter().ToArray<string>());
            ComboBoxTockenPrinter.Items.AddRange(ComboUtils.GetAvailablePrinter().ToArray<string>());
            ComboBoxTockenPrinter.SelectedIndex = 0;
            if (Global.isLocalDB == true || Global.isRemoteDB == true)
            {
                ComboUtils.InitializeStockLocationCombo(ComboBoxDefaultStockLocation, Global.Company.CompanyId);
            }
            ComboBoxDefaultStockLocation.SelectedIndex = -1;
            LoadWorkStation();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnWorkStationSave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnWorkStationCancel.PerformClick();
                return false;
            }
            else if (keyData == (Keys.F10))
            {
                BtnWorkStationExit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F6))
            {
                buttonBackup.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void SetRemoteDBStatus()
        {
            //RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Ab2App");
            //if (key?.GetValue("DefaultIpAddress") != null && !string.IsNullOrEmpty(key.GetValue("DefaultIpAddress")?.ToString()))
            //{
            //    Global.isRemoteDB = true;
            //}
            //else { Global.isRemoteDB = false; }
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Ab2App");
            if (key?.GetValue("DefaultHostName") != null && !string.IsNullOrEmpty(key.GetValue("DefaultHostName")?.ToString()))
            {
                Global.isRemoteDB = true;
            }
            else
            {
                Global.isRemoteDB = false;
            }
        }
        private void buttonBackup_Click(object sender, EventArgs e)
        {
            BackupData backupData = new BackupData();
            backupData.BackupMyData();
        }
    }
}
