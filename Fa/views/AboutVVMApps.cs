using fa.api.Accounting;
using fa.model.Accounting.Masters;
using System;
using System.Windows.Forms;

namespace fa.views
{
    public partial class AboutVVMApps : Form
    {
        public AboutVVMApps()
        {
            InitializeComponent();
            LoadCompanyLogo();
        }
        private void LoadCompanyLogo()
        {
            if (Global.softwareType == SoftwareType.MEDICARE)
            {
                label3.Text = "Hospital/Pharmacy Management System, developed and supported by";
                pictureBox1.Show();
                pictureBox2.Hide();

            }
            else
            {
                label3.Text = "Financial accounting for any small and medium-sized business, developed and supported by";
                pictureBox1.Hide();
                pictureBox2.Show();
            }
        }

        private string GetImagePathForLoginType(BuisnessType loginType)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Directory.GetParent(currentDirectory)?.Parent?.FullName;
            string imagePath = string.Empty;
            if (projectRoot != null)
            {
                imagePath = Path.Combine(projectRoot, "Resources", "terapeia_logo_v3.png");

            }
            else
            {
                // Handle the case where projectRoot is null
            }

            switch (loginType)
            {
                case BuisnessType.Hospital:
                    return imagePath;
                default:
                    imagePath = Path.Combine(Application.StartupPath, "Resources", "BerklySoftEquals.png");
                    return imagePath;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
