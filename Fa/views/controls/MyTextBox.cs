using System.Windows.Forms;
using System.ComponentModel;
class ExtdTextBox : TextBox
{
    private IContainer components;

    protected override void WndProc(ref Message m)
    {
        // Trap WM_PASTE:
        if (m.Msg == 0x302 && Clipboard.ContainsText())
        {
            this.SelectedText = Clipboard.GetText().Replace('\n', ' ');
            return;
        }
        base.WndProc(ref m);
    }

    private void InitializeComponent()
    {
            this.SuspendLayout();
            this.ResumeLayout(false);

    }
}