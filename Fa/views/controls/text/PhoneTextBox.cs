using System;
using System.Windows.Forms;

namespace fa.views.controls.text
{
    public partial class PhoneTextBox : MaskedTextBox
    {
        private string Seperator = "-";
        public PhoneTextBox()
        {
            this.Text = "";
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
        
        private int _PhoneCodeLength = 7;
        public int AreaCodeLength
        {
            get { return _PhoneCodeLength; }
            set
            {
                _PhoneCodeLength = value;
                formatText();
            }
        }

        private int _AreaCodelength = 3;
        public int Length
        {
            get
            {
                return _AreaCodelength;
            }
            set
            {
                _AreaCodelength = value;
                formatText();
            }
        }

        public override string Text { get => base.Text.Trim().Length > 1 ? base.Text:string.Empty; set => base.Text = value; }
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

        private void HighlightPhone()  
        {
            int areaCodePosition = this.Text.IndexOf(Seperator);
            this.SelectionStart = areaCodePosition + 1;
            this.SelectionLength = this.Length - this.AreaCodeLength;
            this.Select();
        }

        private void formatText()
        {
            string format = "9";
            format = format.PadLeft(this.AreaCodeLength, '9');
            format = format + Seperator;
            format = format.PadRight(this.Length, '9');
            this.Mask = format;
            /*
            if (!string.IsNullOrEmpty(this.Text))
            {
                //decimal text = Convert.ToDecimal(this.Text);
                this.Text = String.Format("{0:" + format + "}", this.Text);
            }
            else
            {
                this.Text = String.Format("{0:" + format + "}", "-");
            }*/
        }

        private void CurrencyTextBox_Load()
        {
            formatText();
        }

        private void PhoneTextBox_KeyDown(object sender, KeyEventArgs e)
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
            || e.KeyCode == Keys.OemMinus
            || e.KeyCode == Keys.OemPeriod
            ||(e.Modifiers ==Keys.Control && (e.KeyCode == Keys.V|| e.KeyCode == Keys.C || e.KeyCode == Keys.X))
            )
            {
                int intCursorPos = this.SelectionStart;
                int SeperatorPlace = this.Text.IndexOf(Seperator); 
                int tmpLength = (SeperatorPlace == -1 && !(e.KeyCode == Keys.OemMinus) ? this.Length - this.AreaCodeLength - 1 : this.Length);
                if (this.Text.Length >= tmpLength)
                {
                    e.SuppressKeyPress = true;
                }

                //allowed
                else
                if (e.KeyCode == Keys.OemMinus  && this.Text.Contains(Seperator))

                {
                    HighlightPhone();
                    e.SuppressKeyPress = true;
                }

                else if (e.KeyCode == Keys.Right)
                {

                }
                else if (e.KeyCode == Keys.Left)
                {

                }


                if (this.Text.Contains(Seperator) && intCursorPos > SeperatorPlace + (this.Length-this.AreaCodeLength))
                {
                    e.SuppressKeyPress = true;
                }
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
                    int SeperatorPlace = this.Text.IndexOf(Seperator);
                    this.SelectionStart = 0;
                    this.SelectionLength = (SeperatorPlace > -1 ? SeperatorPlace : this.Text.Length) + 1;
                    this.Select();
                }
                else if (e.KeyCode == Keys.Down)
                {
                    int SeperatorPlace = this.Text.IndexOf(Seperator);
                    if (SeperatorPlace > -1)
                    {
                        HighlightPhone();
                        e.SuppressKeyPress = true;
                    }

                }

            }
            else
            {
                //System.Console.WriteLine(e.KeyCode);
                e.SuppressKeyPress = true;
            }
        }

        private void PhoneTextBox_Leave(object sender, EventArgs e)
        {
            //formatText();
            //this.SelectAll();
        }


    }
}
