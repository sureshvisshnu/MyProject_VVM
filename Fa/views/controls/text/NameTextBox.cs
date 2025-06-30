using fa.libraries.Validation;
using System.ComponentModel;
using System.Windows.Forms;

namespace fa.views.controls.text
{
    public partial class NameTextBox : TextBox
    {
        public NameTextBox()
        {
            InitializeComponent();
        }

        public NameTextBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        private void NameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Instance.Keypress_PasteChecking(sender, e, "NameCheckingNonAllowSpace");
        }
        private void NameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Instance.Keypress_NameCheckingNonAllowSpace(sender, e);
        }
        private void NameTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.Instance.AddContextMenu(this, "NameCheckingNonAllowSpace");
            }
        }
    }
}
