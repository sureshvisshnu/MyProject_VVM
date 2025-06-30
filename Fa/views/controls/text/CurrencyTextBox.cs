using fa.api.utils;
using fa.libraries.Validation;
using fa.model.Common;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace fa.views.controls.text
{
    public partial class CurrencyTextBox : TextBox
    {
        private string Seperator = ".";
        private int _decimals = 2;
        
    public int Decimals {
            get { return _decimals; }
            set { _decimals = value;
                formatText();
            } }
        private int _length = 10;
        public int Length { get
            {
                return _length;
            }
            set
            {
                _length = value;
                formatText();
            }
        }


        public CurrencyTextBox()
        {   
            if(Global.Company!=null)
            {
                Currency Currency = api.Accounting.CurrencyManager.Instance.GetCurrencyById((long)Global.Company.PrimaryCurrencyId);
                _decimals = Currency.RoundingPrecision;
                this.Text =TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            }
            else
            {
                this.Text = "0";
            }
            InitializeComponent();            
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }


        protected void SetTextProgrammatically(string value)
        {
            // save current cursor position and selection
            int start = this.SelectionStart;
            int length = this.SelectionLength;

            // update text
            this.Text = value;

            // restore cursor position and selection
            this.SelectionStart = start;
            this.SelectionLength = length;
        }

        private void HighlisghtDecimals()
        {
            int decimalPlace = this.Text.IndexOf(Seperator);
            this.SelectionStart = decimalPlace + 1;
            this.SelectionLength = this.Decimals;
            this.Select();
        }
       
        private void formatText()
        {
            string format = "0";
            format=format.PadLeft((this.Length -1) - (this.Decimals),'#');
            format = format + Seperator;
            format=format.PadRight(this.Length, '0');
            System.Console.WriteLine(format);
            if (!string.IsNullOrEmpty(this.Text))
            {
                if (!(this.Text.Contains(".") && this.Text.Length==1))
                {
                    decimal text = Convert.ToDecimal(this.Text);
                    this.Text = String.Format("{0:" + format + "}", text);
                }
                else
                {
                    this.Text = String.Format("{0:" + format + "}", 0);
                }
            }
            else
            {
                this.Text = String.Format("{0:" + format + "}", 0);
            }
        }


        private void CurrencyTextBox_Load()
        {
            formatText();
        }

        private void CurrencyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D0
            || e.KeyCode == Keys.D1
            || e.KeyCode == Keys.D2
            || e.KeyCode == Keys.D3
            || e.KeyCode == Keys.D4
            || e.KeyCode == Keys.D5
            || e.KeyCode == Keys.D6
            || e.KeyCode == Keys.D7
            || e.KeyCode == Keys.D8
            || e.KeyCode == Keys.D9
            || e.KeyCode == Keys.NumPad0
            || e.KeyCode == Keys.NumPad1
            || e.KeyCode == Keys.NumPad2
            || e.KeyCode == Keys.NumPad3
            || e.KeyCode == Keys.NumPad4
            || e.KeyCode == Keys.NumPad5
            || e.KeyCode == Keys.NumPad6
            || e.KeyCode == Keys.NumPad7
            || e.KeyCode == Keys.NumPad8
            || e.KeyCode == Keys.NumPad9
            || e.KeyCode == Keys.Decimal
            || e.KeyCode == Keys.OemPeriod
            )
            {
                this.MaxLength = Length;
                int intCursorPos = this.SelectionStart;
                int decimalPlace = this.Text.IndexOf(Seperator);
                int tmpLength = (decimalPlace == -1 && !(e.KeyCode == Keys.Decimal || e.KeyCode == Keys.OemPeriod) ? this.Length - this.Decimals -1 : this.Length);
                if (this.Text.Length >= tmpLength && this.SelectedText.Length == 0 && !(e.KeyCode == Keys.Decimal || e.KeyCode == Keys.OemPeriod))
                {
                    e.SuppressKeyPress = true;
                }

                //allowed
                else
                if ((e.KeyCode == Keys.Decimal || e.KeyCode == Keys.OemPeriod) && this.Text.Contains(Seperator))

                {
                    HighlisghtDecimals();
                    e.SuppressKeyPress = true;
                }

                else if (e.KeyCode == Keys.Right)
                {

                }
                else if (e.KeyCode == Keys.Left)
                {

                }
                string[] dec;
                if (this.Text.Contains(Seperator) && intCursorPos > decimalPlace/* + this.DecimalPlace*/)
                {
                    int dselectionlen = this.SelectionLength;
                    dec = this.Text.Split('.');
                    if (dec.Length == 2 && dec[1].Length == (2 - dselectionlen))
                    {
                        e.SuppressKeyPress = true;
                    }
                }

                //if (this.Text.Contains(Seperator) && intCursorPos > decimalPlace + this.Decimals)
                //{
                //    e.SuppressKeyPress = true;
                //}
            }
            else if (e.KeyCode == Keys.Back
                || e.KeyCode == Keys.Delete
                || e.KeyCode == Keys.Left
                || e.KeyCode == Keys.Right
                || e.KeyCode == Keys.Up
                || e.KeyCode == Keys.Down)
            {
                if (e.KeyCode == Keys.Up)
                {
                    int decimalPlace = this.Text.IndexOf(Seperator);
                    this.SelectionStart = 0;
                    this.SelectionLength = (decimalPlace > -1 ? decimalPlace : this.Text.Length) + 1;
                    this.Select();
                }
                else if (e.KeyCode == Keys.Down)
                {
                    int decimalPlace = this.Text.IndexOf(Seperator);
                    if (decimalPlace > -1)
                    {
                        HighlisghtDecimals();
                        e.SuppressKeyPress = true;
                    }

                }

            }
            else if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                if (Clipboard.GetText().Contains(".") && Clipboard.GetText().Length <= Length)
                {
                    this.MaxLength = Length;
                }
                else
                {
                    this.MaxLength = (Length - (Decimals + 1));
                }
                KeypressValidation.Instance.Keypress_PasteChecking(this, e, "NumberDot");
            }
            else if (((e.KeyCode == Keys.X || e.KeyCode == Keys.C || e.KeyCode == Keys.Z || e.KeyCode == Keys.Y) && e.Control))
            {
            }
            else
            {
                //System.Console.WriteLine(e.KeyCode);
                e.SuppressKeyPress = true;
            }
        }

        private void CurrencyTextBox_Leave(object sender, EventArgs e)
        {
            if (!this.ReadOnly)
            {
                formatText();
                this.SelectAll();
            }
        }

        //public override string Text { get { return base.Text; } set { base.Text = (Convert.ToDecimal(value)).ToString(DefaultValue()); } }
        //public override string Text { get => base.Text; set => base.Text = (Convert.ToDecimal(value)).ToString(DefaultValue()); }

        private String DefaultValue()
        {
            string Result = "0.";
            for (int i = 0; i < this._decimals; i++)
            {
                Result = Result + "0";
            }
            return Result;
        }

        private void CurrencyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
            {

                if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == '.')))
                {
                    e.Handled = true;
                }
                else if (e.KeyChar == '.' && this.Text.IndexOf('.') > -1)
                {
                    e.Handled = true;
                }
            }
        }

        private void CurrencyTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Clipboard.GetText().Contains(".") && Clipboard.GetText().Length <= Length)
                {
                    this.MaxLength = Length;
                }              
                else
                {
                    this.MaxLength = (Length - (Decimals + 1));
                }
                this.ContextMenuStrip = null;
                AddContextMenu(this, "NumberDot");
            }
        }
        public void AddContextMenu(TextBox txtBox, string validation)
        {
            if (txtBox.ContextMenuStrip == null)
            {
                ContextMenuStrip cms = new ContextMenuStrip();
                ToolStripMenuItem tsmiCut = new ToolStripMenuItem("Cut");
                tsmiCut.Click += (sender, e) => txtBox.Cut();
                cms.Items.Add(tsmiCut);
                ToolStripMenuItem tsmiCopy = new ToolStripMenuItem("Copy");
                tsmiCopy.Click += (sender, e) => txtBox.Copy();
                cms.Items.Add(tsmiCopy);
                ToolStripMenuItem tsmiPaste = new ToolStripMenuItem("Paste");
                tsmiPaste.Click += new EventHandler((s, e) => Mousepress_PasteAction(s, e, txtBox, validation));
                cms.Items.Add(tsmiPaste);
                txtBox.ContextMenuStrip = cms;
            }
        }
        void Mousepress_PasteAction(object sender, EventArgs e, TextBox txtBox, string validation)
        {
            if (!this.SelectedText.Contains("."))
            {
                if (Clipboard.GetText().Contains("."))
                {
                    return;
                }
            }
            if (Clipboard.ContainsText())
            {
                Regex Regex = validation == "NameChecking" ? new Regex("^[a-zA-Z0-9!@#$%^&*()_+.,-/ ]*$") : validation == "NameCheckingNonAllowSpace" ? new Regex("^[a-zA-Z0-9!@#$%^&*()_+.,-/]*$") : validation == "EmailChecking" ? new Regex("^[a-zA-Z0-9@.-_]*$") : validation == "WebsiteNameChecking" ? new Regex(@"^[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$") : validation == "NumberDot" ? new Regex("^\\$?(\\d{1,3},?(\\d{3},?)*\\d{3}(.\\d{0,3})?|\\d{1,3}(.\\d{2})?)$") : validation == "TaxDetailsNumberChecking" ? new Regex("^[a-zA-Z0-9-]*$") : validation == "LoginChecking" ? new Regex("^[a-zA-Z0-9,.@]*$") : validation == "Number" ? new Regex("^[0-9]*$") : null;
                var clipData = Clipboard.GetDataObject().GetData(typeof(string));
                if (Regex != null && Regex.Match(clipData.ToString()).Success)
                {
                    txtBox.Paste();
                }
            }
        }
    }
}
