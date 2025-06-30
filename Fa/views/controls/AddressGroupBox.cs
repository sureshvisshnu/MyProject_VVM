using System;
using System.Windows.Forms;
using fa.libraries.Validation;

namespace fa.views.controls
{
    public enum Fields
    {
        address1,
        address2,
        city,
        dist,
        state,
        pin
    }
    public partial class AddressGroupBox : UserControl
    {
        KeypressValidation KeypressValidation = null!;
        public AddressGroupBox()
        {
            KeypressValidation = KeypressValidation.Instance;
            InitializeComponent();
        }
        public void Clear()
        {
            TextBoxAddress1.Text = TextBoxAddress2.Text = TextBoxCity.Text= TextBoxDistrict.Text= TextBoxState.Text=TextBoxPin.Text =null;            
        }
        public void selected_field(Fields Address)
        {
            if(Address == Fields.address1)
            {
                TextBoxAddress1.Select();
            }
            if (Address == Fields.address2)
            {
                TextBoxAddress2.Select();
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
                TextBoxAddress2.ReadOnly = value;
                TextBoxCity.ReadOnly = value;
                TextBoxState.ReadOnly = value;
                TextBoxPin.ReadOnly = value;
                TextBoxDistrict.ReadOnly = value;

                TextBoxAddress1.TabStop = !value;
                TextBoxAddress2.TabStop = !value;
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
        public string AddressLine2
        {
            get
            {
                return TextBoxAddress2.Text;
            }
            set
            {
                TextBoxAddress2.Text = value;
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
        private void TextBoxStreet_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab && TextBoxAddress1.TabStop)
            {
                e.IsInputKey = true;
                TextBoxAddress2.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab && TextBoxAddress1.TabStop)
            {
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
        private void TextBoxStreet_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                e.Handled = true;
            }
            KeypressValidation.Keypress_NameCheckingAddress(sender, e);
        }
        private void TextBoxState_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameCheckingAddress(sender, e);
        }
        private void TextBoxStreet_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteCheckingAddress(sender, e, "NameChecking");
        }     
        private void TextBoxStreet_MouseDown(object sender, MouseEventArgs e)
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
        private void TextBoxAddress2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenuAddress(TextBoxAddress1, "NameChecking");
            }
        }
        private void AddressGroupBox_BackColorChanged(object sender, EventArgs e)
        {
            groupBox1.BackColor = this.BackColor;
        }       
    }
}
