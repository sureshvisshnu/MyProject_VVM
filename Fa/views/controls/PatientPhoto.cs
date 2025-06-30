using System.Drawing;
using System.Windows.Forms;

namespace fa.views.controls
{
    public partial class PatientPhoto : UserControl
    {
        public PatientPhoto()
        {
            InitializeComponent();
        }
        public void Clear()
        {
            pictureBox1.Image = fa.Properties.Resources.patientphoto;
        }
        public Image Photo
        {           
            set
            {
                pictureBox1.Image = value;               
            }
        }

        private void PatientPhoto_ClientSizeChanged(object sender, System.EventArgs e)
        {
            pictureBox1.Width = this.Width - 5;
            pictureBox1.Height = this.Height - 25;
        }

        private void PatientPhoto_Load(object sender, System.EventArgs e)
        {

        }
    }
}
