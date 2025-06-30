using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text.RegularExpressions;
namespace fa.views.controls
{
    public partial class ValidationTextBox : TextBox
    {
        private int _maxlength;
        private ValidationType _validationType;

        public ValidationTextBox()
        {
            InitializeComponent();
            SetTextBoxProperties();
        }

        [Browsable(true)]
        public ValidationType ValidationType
        {
            get { return _validationType; }
            set { _validationType = value; }
        }

        [Browsable(false)]
        public int MaxLengths
        {
            get { return _maxlength; }
            set { _maxlength = value; }
        }
       
        void SetTextBoxProperties()
        {
            _maxlength =
               _validationType.ToString() == "NameChecking" || _validationType.ToString() == "TaxDetailsNumberChecking" ? 20 :
               _validationType.ToString() == "EmailChecking" || _validationType.ToString() == "WebsiteNameChecking" ? 50 :
               //_validationType.ToString() == "NumberDot" ? 15 : 
               _validationType.ToString() == "TaxDetailsNumberChecking" ? 30 : 100;
            MaxLengths = _maxlength;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if ((keyData == (Keys.V | Keys.Control)) && Clipboard.ContainsText())
                {
                    Regex Regex = _validationType.ToString() == "NameChecking" ? new Regex("^[a-zA-Z0-9!@#$%^&*()_+.,-/ ]*$") :
                                 _validationType.ToString() == "EmailChecking" ? new Regex("^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$") :
                                 _validationType.ToString() == "WebsiteNameChecking" ? new Regex(@"^[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$") :
                                 //_validationType.ToString() == "NumberDot" ? new Regex("^\\$?(\\d{1,3},?(\\d{3},?)*\\d{3}(.\\d{0,3})?|\\d{1,3}(.\\d{2})?)$") :
                                 _validationType.ToString() == "TaxDetailsNumberChecking" ? new Regex("^[a-zA-Z0-9-]*$") :
                                 _validationType.ToString() == "LoginChecking" ? new Regex("^[a-zA-Z0-9,.@]*$") :
                                 _validationType.ToString() == "Number" ? new Regex("^[0-9]*$") : null;

                    var ClipData = Clipboard.GetDataObject().GetData(typeof(string));
                    if (Regex != null && !Regex.Match(ClipData.ToString()).Success)
                    {
                        return true;
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        void Mousepress_PasteAction(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                Regex Regex =
                        _validationType.ToString() == "NameChecking" ? new Regex("^[a-zA-Z0-9!@#$%^&*()_+.,-/ ]*$") :
                        _validationType.ToString() == "EmailChecking" ? new Regex("^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$") :
                        _validationType.ToString() == "WebsiteNameChecking" ? new Regex(@"^[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$") :
                        // _validationType.ToString() == "NumberDot" ? new Regex("^\\$?(\\d{1,3},?(\\d{3},?)*\\d{3}(.\\d{0,3})?|\\d{1,3}(.\\d{2})?)$") :
                        _validationType.ToString() == "TaxDetailsNumberChecking" ? new Regex("^[a-zA-Z0-9-]*$") :
                        _validationType.ToString() == "LoginChecking" ? new Regex("^[a-zA-Z0-9,.@]*$") :
                        _validationType.ToString() == "Number" ? new Regex("^[0-9]*$") : null; var clipData = Clipboard.GetDataObject().GetData(typeof(string));

                if (Regex != null && Regex.Match(clipData.ToString()).Success)
                {
                    base.Paste();
                }
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                AddContextMenu();
            }
            base.OnMouseDown(e);
        }

        public void AddContextMenu()
        {
            if (base.ContextMenuStrip == null)
            {
                ContextMenuStrip cms = new ContextMenuStrip();
                ToolStripMenuItem tsmiCut = new ToolStripMenuItem("Cut");
                tsmiCut.Click += (sender, e) => base.Cut();
                cms.Items.Add(tsmiCut);
                ToolStripMenuItem tsmiCopy = new ToolStripMenuItem("Copy");
                tsmiCopy.Click += (sender, e) => base.Copy();
                cms.Items.Add(tsmiCopy);
                ToolStripMenuItem tsmiPaste = new ToolStripMenuItem("Paste");
                tsmiPaste.Click += new EventHandler((s, e) => Mousepress_PasteAction(s, e));
                cms.Items.Add(tsmiPaste);
                base.ContextMenuStrip = cms;
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (_validationType.ToString() == "NameChecking")
            {
                if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
                {
                    Regex Regex = new Regex("^[a-zA-Z0-9!@#$%^&*()_+.,-/]*$");
                    if ((Char.IsLetterOrDigit(e.KeyChar) || Regex.IsMatch(e.KeyChar.ToString()) || e.KeyChar == '\\' || char.IsWhiteSpace(e.KeyChar)))
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
            else if (_validationType.ToString() == "WebsiteNameChecking")
            {
                if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
                {
                    Regex Regex = new Regex("^[-._~:/?#\\[\\]@!$&'()*+,;=`]*$");
                    if ((Char.IsLetterOrDigit(e.KeyChar) || Regex.IsMatch(e.KeyChar.ToString()) || char.IsWhiteSpace(e.KeyChar)))
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }

            }
            else if (_validationType.ToString() == "TaxDetailsNumberChecking")
            {
                if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
                {
                    if (!(Char.IsLetterOrDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == '-')))
                        e.Handled = true;
                }

            }
            else if (_validationType.ToString() == "LoginChecking")
            {
                if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
                {
                    if (!(Char.IsLetterOrDigit(e.KeyChar) || (e.KeyChar == ',') || (e.KeyChar == '.') || (e.KeyChar == '@')))
                        e.Handled = true;
                }
            }
            else if (_validationType.ToString() == "EmailChecking")
            {
                if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
                {
                    if (!(Char.IsLetterOrDigit(e.KeyChar) || (e.KeyChar == '.') || (e.KeyChar == '@')))
                    {
                        e.Handled = true;
                    }
                }
            }
            //else if (_validationType.ToString() == "NumberDot")
            //{
            //    if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
            //    {

            //        if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == '.')))
            //        {
            //            e.Handled = true;
            //        }
            //        else if (e.KeyChar == '.' && base.Text.IndexOf('.') > -1)
            //        {
            //            e.Handled = true;
            //        }
            //    }
            //}
            else if (_validationType.ToString() == "Number")
            {
                if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
                {

                    if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == '.')))
                    {
                        e.Handled = true;
                    }
                }
            }
            base.OnKeyPress(e);
        }
    }
    public enum ValidationType
    {
        NameChecking, EmailChecking, WebsiteNameChecking, TaxDetailsNumberChecking, LoginChecking, Number
    };
}
