using fa.api.Accounting;
using fa.model.Accounting.Masters;
using Microsoft.Win32;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.ServiceProcess;
using fa.views.Systems;
using Fa.views.Systems;

namespace fa.views
{
    public partial class SplashScreen : Form
    {
        public bool IsStopSplashScreen = true;

        public SplashScreen()
        {
            InitializeComponent();
        }
        private bool IsHideSplashScreen = false;
        public bool IsShowContainerScreen = false;
        public bool IsShowWorkStation = false;
        public string myIpId = null!;
        public string myhostName = null!;
        private bool IsShowDBIpSetup = false;

        private void TimerTick(object sender, EventArgs e)
        {
            progressBar1.Increment(1);
            if (IsHideSplashScreen)
            {
                this.Hide();
                if (IsShowContainerScreen)
                {
                    IsShowContainerScreen = false;
                    Container Container = new Container(this);
                    Container.Show();
                }
                else if (IsShowWorkStation)
                {
                    IsShowWorkStation = false;
                    Console.WriteLine("MySQL is not installed on this machine.");
                    if (IsShowDBIpSetup)
                    {
                        FormDBIpSetup formDBIpSetup = new FormDBIpSetup
                        {
                            DBInfoMsg = "Required DB is not installed on this machine."
                        };
                        formDBIpSetup.Show();
                    }
                }
            }

            if (progressBar1.Value > 120)
            {
                progressBar1.Value = 0;
            }

            if (progressBar1.Value == 100)
            {
                IsHideSplashScreen = true;
            }
        }
        private void SplashScreen_Load(object sender, EventArgs e)
        {
            TimerTick(sender, e); 

            try
            {
                if (IsMySqlInstalled())
                {
                    Global.isLocalDB = true;
                    GetDefaultIpId();
                    LoadSoftwareImage();
                    loadAllDefaultValues();
                    IsShowWorkStation = false;
                    IsShowContainerScreen = true;
                }
                else
                {
                    RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\VVMatrix");
                    //if (key?.GetValue("DefaultIpAddress") != null && !string.IsNullOrEmpty(key.GetValue("DefaultIpAddress")?.ToString()))
                    if (key?.GetValue("DefaultHostName") != null && !string.IsNullOrEmpty(key.GetValue("DefaultHostName")?.ToString()))
                    {
                        Global.isLocalDB = true;
                        //myIpId = key.GetValue("DefaultIpAddress")?.ToString()!;
                        myhostName = key.GetValue("DefaultHostName")?.ToString()!;
                        //if (myIpId == "localhost")
                        if (myhostName == Environment.MachineName)
                        {
                            IsShowWorkStation = true;
                            IsShowDBIpSetup = true;
                        }
                        else
                        {
                            GetDefaultIpId();
                            LoadSoftwareImage();
                            loadAllDefaultValues();
                            IsShowWorkStation = false;
                            IsShowContainerScreen = true;
                        }
                    }
                    else
                    {
                        IsShowContainerScreen = false;
                        IsShowDBIpSetup = true;
                        IsShowWorkStation = true;
                    }
                    key?.Close();
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Could not Start the application. Please contact your administrator or support.";
                string caption = Global.softwareType == SoftwareType.MEDICARE ? "MEDICARE - Error" : "Equals - Error";
                MessageBox.Show(errorMessage, caption);
                Environment.Exit(1);
                Console.WriteLine(ex.Message);
            }
        }
        private void loadAllDefaultValues()
        {
            CompanyManager cm = CompanyManager.Instance;
        }
        private void LoadSoftwareImage()
        {
            if (Global.softwareType == SoftwareType.MEDICARE)
            {
                pictureBox1.Visible = true;
                pictureBox2.Visible = false;
            }
            else
            {
                pictureBox1.Visible = false;
                pictureBox2.Visible = true;
            }
        }
        private void GetDefaultIpId()
        {
            try
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\VVMApp"))
                {
                    if (key != null)
                    {
                        //fa.Data.Global.IpAddressDefault = key.GetValue("DefaultIpAddress")?.ToString() ?? "localhost";
                        fa.Data.Global.DefaultHostName = key.GetValue("DefaultHostName")?.ToString() ?? Environment.MachineName;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading default host name from registry: " + ex.Message);
            }
            //fa.Data.Global.IpAddressDefault = "localhost";
            fa.Data.Global.DefaultHostName = Environment.MachineName;
        }

        public static bool IsMySqlInstalled()
        {
            return IsMySqlServiceRunning() || CheckIfServiceIsPrescent();
        }

        private static bool IsMySqlServiceRunning()
        {
            try
            {
                ServiceController sc = new ServiceController("MySQL");
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        private static bool CheckIfServiceIsPrescent()
        {
            string[] possiblePaths = {
                    @"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe",
                    @"C:\Program Files\MySQL\MySQL Server 5.7\bin\mysql.exe",
                    @"C:\Program Files\MySQL\MySQL Server 5.6\bin\mysql.exe",
                    @"C:\Program Files (x86)\MySQL\MySQL Server 8.0\bin\mysql.exe",
                    @"C:\Program Files (x86)\MySQL\MySQL Server 5.7\bin\mysql.exe",
                    @"C:\Program Files (x86)\MySQL\MySQL Server 5.6\bin\mysql.exe"
                };
            foreach (string path in possiblePaths)
            {
                if (System.IO.File.Exists(path))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
