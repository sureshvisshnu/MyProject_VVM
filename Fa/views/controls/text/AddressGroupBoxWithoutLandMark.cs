using System;
using System.Windows.Forms;
using fa.libraries.Validation;
namespace fa.views.controls.text
{
    public enum Fields
    {
        address1,
        city,
        dist,
        state,
        pin
    }
    public partial class AddressGroupBoxWithoutLandMark : UserControl
    {
        KeypressValidation KeypressValidation = null;
        public AddressGroupBoxWithoutLandMark()
        {
            KeypressValidation = KeypressValidation.Instance;
            InitializeComponent();
        }
        public void Clear()
        {
            TextBoxAddress1.Text = TextBoxCity.Text = TextBoxDistrict.Text = TextBoxState.Text = TextBoxPin.Text = null;
        }
        public void selected_field(Fields Address)
        {
            if (Address == Fields.address1)
            {
                TextBoxAddress1.Focus();
            }            
            if (Address == Fields.city)
            {
                TextBoxCity.Select();
            }
            if (Address == Fields.dist)
            {
                TextBoxDistrict.Select();
            }
            if (Address == Fields.state)
            {
                TextBoxState.Select();
            }
            if (Address == Fields.pin)
            {
                TextBoxPin.Focus();
            }
        }
        public bool ReadOnly
        {
            get
            {
                return TextBoxAddress1.ReadOnly;
            }
            set
            {
                TextBoxAddress1.ReadOnly = value;
                TextBoxCity.ReadOnly = value;
                TextBoxState.ReadOnly = value;
                TextBoxPin.ReadOnly = value;
                TextBoxDistrict.ReadOnly = value;
                TextBoxAddress1.TabStop = !value;
                TextBoxCity.TabStop = !value;
                TextBoxState.TabStop = !value;
                TextBoxPin.TabStop = !value;
                TextBoxDistrict.TabStop = !value;
            }
        }
        public string GroupName
        {
            get
            {
                return groupBox1.Text;
            }
            set
            {
                groupBox1.Text = value;
            }
        }
        public string AddressLine1
        {
            get
            {
                return TextBoxAddress1.Text;
            }
            set
            {
                TextBoxAddress1.Text = value;
            }
        }     
        public string CityName
        {
            get
            {
                return TextBoxCity.Text;
            }
            set
            {
                TextBoxCity.Text = value;
            }
        }
        public string DistrictName
        {
            get
            {
                return TextBoxDistrict.Text;
            }
            set
            {
                TextBoxDistrict.Text = value;
            }
        }
        public string StateName
        {
            get
            {
                return TextBoxState.Text;
            }
            set
            {
                TextBoxState.Text = value;
            }
        }
        public string PinCode
        {
            get
            {
                return TextBoxPin.Text;
            }
            set
            {
                TextBoxPin.Text = value;
            }
        }
        private void TextBoxAddress1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab && TextBoxAddress1.TabStop)
            {
                TextBoxAddress1.SelectionLength = 0;
                e.IsInputKey = true;
                TextBoxCity.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab && TextBoxAddress1.TabStop)
            {
                TextBoxAddress1.SelectionLength = 0;
                base.OnPreviewKeyDown(e);
            }
        }
        private void TextBoxPin_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab && TextBoxPin.TabStop)
            {
                base.OnPreviewKeyDown(e);
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab && TextBoxPin.TabStop)
            {
                e.IsInputKey = true;
                TextBoxState.Select();
            }
        }
        private void TextBoxAddress1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                e.Handled = true;
            }
            KeypressValidation.Keypress_NameCheckingAddress(sender, e);
        }
        private void TextBoxCity_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameCheckingAddress(sender, e);
        }
        private void TextBoxAddress1_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteCheckingAddress(sender, e, "NameChecking");
        }
        private void TextBoxAddress1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenuAddress(TextBoxAddress1, "NameChecking");
            }
        }
        private void TextBoxCity_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenuAddress(TextBoxCity, "NameChecking");
            }
        }
        private void TextBoxDistrict_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenuAddress(TextBoxDistrict, "NameChecking");
            }
        }
        private void TextBoxState_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenuAddress(TextBoxState, "NameChecking");
            }
        }
        private void AddressGroupBoxWithoutLandMark_BackColorChanged(object sender, EventArgs e)
        {
            groupBox1.BackColor = this.BackColor;
        }
        private void AddressGroupBoxWithoutLandMark_ClientSizeChanged(object sender, EventArgs e)
        {
            groupBox1.Width = this.Width-10;
            groupBox1.Height = this.Height-5;
        }
    }
}
