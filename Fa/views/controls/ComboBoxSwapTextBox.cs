namespace fa.views.controls
{
    public partial class ComboBoxSwapTextBox : ComboBox
    {
        public TextBox TextBox = null;
        public ComboBoxSwapTextBox()
        {
            InitializeComponent();
            TextBox = new TextBox();
            base.Visible = true;
            TextBox.Location = base.Location;
            TextBox.Size = base.Size;
            TextBox.Visible = false;
            TextBox.TabStop = false;
            TxtVisible = true;
        }

        private void ComboBoxSwapTextBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SelectedData = base.SelectedItem == null ? "" : base.SelectedItem.ToString();
            TextBox.Text = SelectedData;
        }
        private bool _TxtVisible;
        public bool TxtVisible
        {
            get
            {
                return _TxtVisible;
            }
            set
            {
                _TxtVisible = value;
                TextBox.Visible = value;

            }
        }
        private void ComboBoxSwapTextBox_VisibleChanged(object sender, EventArgs e)
        {
            if (base.Visible == false)
            {
                if (this.Parent != null) { this.Parent.Controls.Add(TextBox); }
                TextBox.Visible = TxtVisible;
                TextBox.ReadOnly = true;
                TextBox.BackColor = System.Drawing.Color.White;
                TextBox.BringToFront();
            }
            else
            {
                this.BringToFront();
            }
        }

        private void ComboBoxSwapTextBox_LocationChanged(object sender, EventArgs e)
        {
            TextBox.Location = base.Location;
            TextBox.Size = base.Size;
        }

        private void ComboBoxSwapTextBox_SizeChanged(object sender, EventArgs e)
        {
            TextBox.Location = this.Location;
            TextBox.Size = this.Size;
        }

        private void ComboBoxSwapTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.DroppedDown = false;
        }

        private void ComboBoxSwapTextBox_EnabledChanged(object sender, EventArgs e)
        {

        }
    }
}
