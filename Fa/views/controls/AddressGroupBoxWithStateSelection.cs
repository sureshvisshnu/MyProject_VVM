using fa.api.Accounting;
using fa.libraries.utils;
using fa.libraries.Validation;
using fa.model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.controls
{
    public partial class AddressGroupBoxWithStateSelection : UserControl
    {
        KeypressValidation KeypressValidation = null!;
        public AddressGroupBoxWithStateSelection()
        {
            KeypressValidation = KeypressValidation.Instance;
            InitializeComponent();
        }
        public void Clear()
        {
            ComboUtils.InitializeStateCombo(ComboBoxSwapTextBoxState, CountryId);
            TextBoxAddressStreet.Text = TextBoxAddress2.Text = TextBoxCity.Text = TextBoxDistrict.Text = ComboBoxSwapTextBoxState.Text = TextBoxPin.Text = null;
            ComboBoxSwapTextBoxState.SelectedIndex = -1;
        }
        public void selected_field(Fields Address)
        {
            if (Address == Fields.address1)
            {
                TextBoxAddressStreet.Select();
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
                ComboBoxSwapTextBoxState.Select();
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
                return TextBoxAddressStreet.ReadOnly;
            }
            set
            {
                TextBoxAddressStreet.ReadOnly = value;
                TextBoxAddress2.ReadOnly = value;
                TextBoxCity.ReadOnly = value;
                ComboBoxSwapTextBoxState.Visible = !value;
                TextBoxPin.ReadOnly = value;
                TextBoxDistrict.ReadOnly = value;

                TextBoxAddressStreet.TabStop = !value;
                TextBoxAddress2.TabStop = !value;
                TextBoxCity.TabStop = !value;
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
                return TextBoxAddressStreet.Text;
            }
            set
            {
                TextBoxAddressStreet.Text = value;
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
                return ComboBoxSwapTextBoxState.Text;
            }
            set
            {
                ComboBoxSwapTextBoxState.Text = value;
            }
        }
        public int StateSelectedIndex
        {
            get
            {
                return ComboBoxSwapTextBoxState.SelectedIndex; ;
            }
            set
            {
                ComboBoxSwapTextBoxState.SelectedIndex = value;
            }
        }
        private long _StateId = 0L;
        public long StateId
        {
            get
            {
                return _StateId;
            }
            set
            {
                _StateId = value;
                if (value != 0L)
                {
                    State lState = StateManager.Instance.GetStateById(value);
                    if (lState != null)
                    {
                        ComboBoxSwapTextBoxState.SelectedIndex = ComboBoxSwapTextBoxState.FindStringExact(lState.Name);
                    }
                }
            }
        }
        private long _CountryId = Global.Company != null ? (long)Global.Company.CountryId! : 0L;
        public long CountryId
        {
            get
            {
                return _CountryId;
            }
            set
            {
                _CountryId = value;
                if (value != 0L)
                {
                    ComboUtils.InitializeStateCombo(ComboBoxSwapTextBoxState, value);
                }
                ComboBoxSwapTextBoxState.ResetText();
                ComboBoxSwapTextBoxState.SelectedIndex = -1;
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
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab && TextBoxAddressStreet.TabStop)
            {
                e.IsInputKey = true;
                TextBoxAddress2.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab && TextBoxAddressStreet.TabStop)
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
                ComboBoxSwapTextBoxState.Select();
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

        private void TextBoxAddress2_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameCheckingAddress(sender, e);
        }

        private void TextBoxAddress1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenuAddress(TextBoxAddressStreet, "NameChecking");
            }
        }

        private void TextBoxAddress2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.AddContextMenuAddress(TextBoxAddress2, "NameChecking");
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

        private void ComboBoxSwapTextBoxState_SelectedIndexChanged(object sender, EventArgs e)
        {
            StateSelectedIndex = ComboBoxSwapTextBoxState.SelectedIndex;
            if (ComboBoxSwapTextBoxState.SelectedIndex > -1)
            {
                State lState = StateManager.Instance.GetStateById(((State)ComboBoxSwapTextBoxState.Items[ComboBoxSwapTextBoxState.SelectedIndex]).Id);
                if (lState != null)
                {
                    _StateId = lState.Id;
                }
            }
            else
            {
                _StateId = 0L;
            }
        }

        private void TextBoxAddressStreet_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Keypress_PasteCheckingAddress(sender, e, "NameChecking");

        }

        private void TextBoxCity_TextChanged(object sender, EventArgs e)
        {
        }

        private void TextBoxPin_Enter(object sender, EventArgs e)
        {
            MaskedTextBox? maskedTextBox = sender as MaskedTextBox;
            if (maskedTextBox != null)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    maskedTextBox.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                    int inputLength = maskedTextBox.Text.Length;
                    maskedTextBox.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;
                    maskedTextBox.Select(0, inputLength);
                });
            }
        }
        private void TextBoxSelectedTextFocus(object sender)
        {
            TextBox? TextBox = sender as TextBox;
            if (TextBox != null)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    int inputLength = TextBox.Text.Length;
                    TextBox.Select(0, inputLength);
                });
            }
        }

        private void TextBoxAddressStreet_Enter(object sender, EventArgs e)
        {
            TextBoxSelectedTextFocus(sender);
        }
        private void TextBoxAddress2_Enter(object sender, EventArgs e)
        {
            TextBoxSelectedTextFocus(sender);
        }
        private void TextBoxCity_Enter(object sender, EventArgs e)
        {
            TextBoxSelectedTextFocus(sender);
        }
        private void TextBoxDistrict_Enter(object sender, EventArgs e)
        {
            TextBoxSelectedTextFocus(sender);
        }
        public event EventHandler StateComboBoxSelectedIndexChanged
        {
            add { ComboBoxSwapTextBoxState.SelectedIndexChanged += value; }
            remove { ComboBoxSwapTextBoxState.SelectedIndexChanged -= value; }
        }
    }
}
