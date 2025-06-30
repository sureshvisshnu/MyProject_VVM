using fa.libraries.Validation;
using System.ComponentModel;
using System.Windows.Forms;

namespace fa.views.controls.text
{
    public partial class NumberTextBox : TextBox
    {
        public NumberTextBox()
        {
            InitializeComponent();
        }

        public NumberTextBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private void NumberTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Instance.Keypress_PasteChecking(sender, e, "Number");
        }

        private void NumberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Instance.Keypress_Num(sender, e);
        }

        private void NumberTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.Instance.AddContextMenu(this, "Number");
            }
        }
    }
}
