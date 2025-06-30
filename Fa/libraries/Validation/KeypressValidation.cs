using Google.Protobuf.WellKnownTypes;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace fa.libraries.Validation
{
    public class KeypressValidation 
    {
        private static volatile KeypressValidation instance;
        private static object syncRoot = new Object();
        KeypressValidation()
        {

        }
        public static KeypressValidation Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new KeypressValidation();
                    }
                }

                return instance;
            }
        }
        public void Keypress_NameCheckingNonAllowSpace(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
            {
                Regex Regex = new Regex("^[a-zA-Z0-9!@#?$%^&*()_:=+.,-/]*$");
                if ((Char.IsLetterOrDigit(e.KeyChar) || Regex.IsMatch(e.KeyChar.ToString()) || e.KeyChar == '\\'))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
        }
        public void Keypress_NameChecking(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
            {
                Regex Regex = new Regex("^[a-zA-Z0-9!@#$%^&*()_:=+.,-/]*$");
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
        public void Keypress_NameCheckingAddress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
            {
                Regex Regex = new Regex("^[a-zA-Z0-9!@#$%^&*:;()_+.,-/]*$");
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
        public void Keypress_NameCheckingProduct(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
            {
                Regex Regex = new Regex("^[a-zA-Z0-9!@$%^&*()_+.,\"'-/]*$");
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
        public void Keypress_PasteChecking(object sender, KeyEventArgs e , string validation)
        {
            if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                Regex Regex = validation == "NameChecking" ? new Regex("^[a-zA-Z0-9!@#$%^&*()_+.,-/ ]*$") : validation == "NameCheckingNonAllowSpace" ? new Regex("^[a-zA-Z0-9!@#$%^?&*()_:=+.,-/]*$"): validation == "EmailChecking" ? new Regex("^[a-zA-Z0-9@.-_]*$") : validation == "WebsiteNameChecking" ? new Regex(@"^[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$") : validation == "NumberDot" ? new Regex("^\\$?(\\d{1,3},?(\\d{3},?)*\\d{3}(.\\d{0,3})?|\\d{1,3}(.\\d{2})?)$") : validation == "NumberDotNegativeSign" ? new Regex("^\\$[-]?(\\d{1,3},?(\\d{3},?)*\\d{3}(.\\d{0,3})?|\\d{1,3}(.\\d{2})?)$") : validation == "TaxDetailsNumberChecking" ? new Regex("^[a-zA-Z0-9-]*$") : validation == "LoginChecking" ? new Regex("^[a-zA-Z0-9,.@]*$") : validation == "Number" ? new Regex("^[0-9]*$") : null!;

                var clipData = Clipboard.GetDataObject().GetData(typeof(string));
                if (Regex !=null && !Regex.Match(clipData.ToString()!).Success)
                {
                    e.SuppressKeyPress = true;
                }
            }
        }
        public void Keypress_WebPasteChecking(object sender, KeyEventArgs e, string validation)
        {
            if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                var clipData = Clipboard.GetDataObject().GetData(typeof(string));
                Console.WriteLine($"Clipboard content: {clipData}");

                Regex regex = validation == "WebsiteNameChecking"
                    ? new Regex(@"^[a-zA-Z0-9\-._~:/?#\[\]@!$&'()*+,;=`]*$")
                    : null!;

                if (regex != null && !regex.IsMatch(clipData.ToString()!))
                {
                    Console.WriteLine($"Validation failed for: {clipData}");
                    e.SuppressKeyPress = true;
                }
            }
        }
        public void Keypress_PasteCheckingAddress(object sender, KeyEventArgs e, string validation)
        {
            if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                Regex Regex = validation == "NameChecking" ? new Regex("^[a-zA-Z0-9!@#$%^&*:;()_+.,-/ ]*$") : validation == "NameCheckingNonAllowSpace" ? new Regex("^[a-zA-Z0-9!@#$%^?&*()_:=+.,-/]*$") : validation == "EmailChecking" ? new Regex("^[a-zA-Z0-9@.-_]*$") : validation == "WebsiteNameChecking" ? new Regex(@"^[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$") : validation == "NumberDot" ? new Regex("^\\$?(\\d{1,3},?(\\d{3},?)*\\d{3}(.\\d{0,3})?|\\d{1,3}(.\\d{2})?)$") : validation == "TaxDetailsNumberChecking" ? new Regex("^[a-zA-Z0-9-]*$") : validation == "LoginChecking" ? new Regex("^[a-zA-Z0-9,.@]*$") : validation == "Number" ? new Regex("^[0-9]*$") : null;

                var clipData = Clipboard.GetDataObject().GetData(typeof(string));
                if (Regex != null && !Regex.Match(clipData.ToString()).Success)
                {
                    e.SuppressKeyPress = true;
                }
            }
        }
        public void Keypress_PasteCheckingProduct(object sender, KeyEventArgs e, string validation)
        {
            if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                Regex Regex = validation == "NameChecking" ? new Regex("^[a-zA-Z0-9!@$%^&*()_+.,'\"-/ ]*$") : validation == "NameCheckingNonAllowSpace" ? new Regex("^[a-zA-Z0-9!@#$%^?&*()_:=+.,-/]*$") : validation == "EmailChecking" ? new Regex("^[a-zA-Z0-9@.-_]*$") : validation == "WebsiteNameChecking" ? new Regex(@"^[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$") : validation == "NumberDot" ? new Regex("^\\$?(\\d{1,3},?(\\d{3},?)*\\d{3}(.\\d{0,3})?|\\d{1,3}(.\\d{2})?)$") : validation == "TaxDetailsNumberChecking" ? new Regex("^[a-zA-Z0-9-]*$") : validation == "LoginChecking" ? new Regex("^[a-zA-Z0-9,.@]*$") : validation == "Number" ? new Regex("^[0-9]*$") : null;

                var clipData = Clipboard.GetDataObject().GetData(typeof(string));
                if (Regex != null && !Regex.Match(clipData.ToString()).Success)
                {
                    e.SuppressKeyPress = true;
                }
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
            if(txtBox.Text.Contains("."))
            {
                if(Clipboard.GetText().Contains("."))
                {
                    return;
                }
            }
            if (Clipboard.ContainsText())
            {
                Regex Regex = validation == "NameChecking" ? new Regex("^[a-zA-Z0-9!@#$%^&*()_+.,-/ ]*$") : validation == "NameCheckingNonAllowSpace" ? new Regex("^[a-zA-Z0-9!@#$%^?&*()_:=+.,-/]*$") : validation == "EmailChecking" ? new Regex("^[a-zA-Z0-9@.-_]*$") : validation == "WebsiteNameChecking" ? new Regex(@"^[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$") : validation == "NumberDot" ? new Regex("^\\$?(\\d{1,3},?(\\d{3},?)*\\d{3}(.\\d{0,3})?|\\d{1,3}(.\\d{2})?)$") : validation == "TaxDetailsNumberChecking" ? new Regex("^[a-zA-Z0-9-]*$") : validation == "LoginChecking" ? new Regex("^[a-zA-Z0-9,.@]*$") : validation == "Number" ? new Regex("^[0-9]*$") : null;
                var clipData = Clipboard.GetDataObject().GetData(typeof(string));
                if (Regex != null && Regex.Match(clipData.ToString()).Success)
                {
                    txtBox.Paste();
                }
            }
        }
        public void AddContextMenuAddress(TextBox txtBox, string validation)
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
                tsmiPaste.Click += new EventHandler((s, e) => Mousepress_PasteActionAddress(s, e, txtBox, validation));
                cms.Items.Add(tsmiPaste);
                txtBox.ContextMenuStrip = cms;
            }
        }
        void Mousepress_PasteActionAddress(object sender, EventArgs e, TextBox txtBox, string validation)
        {
            if (txtBox.Text.Contains("."))
            {
                if (Clipboard.GetText().Contains("."))
                {
                    return;
                }
            }
            if (Clipboard.ContainsText())
            {
                Regex Regex = validation == "NameChecking" ? new Regex("^[a-zA-Z0-9!@#$%^&*:;()_+.,-/ ]*$") : validation == "NameCheckingNonAllowSpace" ? new Regex("^[a-zA-Z0-9!@#$%^?&*()_:=+.,-/]*$") : validation == "EmailChecking" ? new Regex("^[a-zA-Z0-9@.-_]*$") : validation == "WebsiteNameChecking" ? new Regex(@"^[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$") : validation == "NumberDot" ? new Regex("^\\$?(\\d{1,3},?(\\d{3},?)*\\d{3}(.\\d{0,3})?|\\d{1,3}(.\\d{2})?)$") : validation == "TaxDetailsNumberChecking" ? new Regex("^[a-zA-Z0-9-]*$") : validation == "LoginChecking" ? new Regex("^[a-zA-Z0-9,.@]*$") : validation == "Number" ? new Regex("^[0-9]*$") : null;
                var clipData = Clipboard.GetDataObject().GetData(typeof(string));
                if (Regex != null && Regex.Match(clipData.ToString()).Success)
                {
                    txtBox.Paste();
                }
            }
        }
        public void AddContextMenuProduct(TextBox txtBox, string validation)
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
                tsmiPaste.Click += new EventHandler((s, e) => Mousepress_PasteActionProduct(s, e, txtBox, validation));
                cms.Items.Add(tsmiPaste);
                txtBox.ContextMenuStrip = cms;
            }
        }
        void Mousepress_PasteActionProduct(object sender, EventArgs e, TextBox txtBox, string validation)
        {
            if (txtBox.Text.Contains("."))
            {
                if (Clipboard.GetText().Contains("."))
                {
                    return;
                }
            }
            if (Clipboard.ContainsText())
            {
                Regex Regex = validation == "NameChecking" ? new Regex("^[a-zA-Z0-9!@$%^&*()_+.,'\"-/ ]*$") : validation == "NameCheckingNonAllowSpace" ? new Regex("^[a-zA-Z0-9!@#$%^?&*()_:=+.,-/]*$") : validation == "EmailChecking" ? new Regex("^[a-zA-Z0-9@.-_]*$") : validation == "WebsiteNameChecking" ? new Regex(@"^[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$") : validation == "NumberDot" ? new Regex("^\\$?(\\d{1,3},?(\\d{3},?)*\\d{3}(.\\d{0,3})?|\\d{1,3}(.\\d{2})?)$") : validation == "TaxDetailsNumberChecking" ? new Regex("^[a-zA-Z0-9-]*$") : validation == "LoginChecking" ? new Regex("^[a-zA-Z0-9,.@]*$") : validation == "Number" ? new Regex("^[0-9]*$") : null;
                var clipData = Clipboard.GetDataObject().GetData(typeof(string));
                if (Regex != null && Regex.Match(clipData.ToString()).Success)
                {
                    txtBox.Paste();
                }
            }
        }
        public void AddContextMenuGridCell(DataGridViewEditingControlShowingEventArgs dgvControl, DataGridView dgv, string validation, int index)
        {
            ContextMenuStrip mnu = new ContextMenuStrip();
            ToolStripMenuItem mnuCopy = new ToolStripMenuItem("Copy");
            ToolStripMenuItem mnuCut = new ToolStripMenuItem("Cut");
            ToolStripMenuItem mnuPaste = new ToolStripMenuItem("Paste");
            mnu.Items.AddRange(new ToolStripItem[] { mnuCopy, mnuCut, mnuPaste });
            dgvControl.Control.ContextMenuStrip = mnu;

            mnuCopy.Click += (s, e) =>
            {
                if (dgv.CurrentCell != null && dgv.Rows.Count > 0 && dgv.Columns.Count > 0)
                {
                    object cellValue = dgv.Rows[dgv.CurrentCell.RowIndex].Cells[dgv.CurrentCell.ColumnIndex].Value;
                    if (cellValue != null)
                        Clipboard.SetData(DataFormats.Text, cellValue.ToString());
                }
            };

            mnuCut.Click += (s, e) =>
            {
                if (dgv.CurrentCell != null && dgv.Rows.Count > 0 && dgv.Columns.Count > 0)
                {
                    object cellValue = dgv.Rows[dgv.CurrentCell.RowIndex].Cells[dgv.CurrentCell.ColumnIndex].Value;
                    if (cellValue != null)
                    {
                        Clipboard.SetData(DataFormats.Text, cellValue.ToString());
                        dgv.Rows[dgv.CurrentCell.RowIndex].Cells[dgv.CurrentCell.ColumnIndex].Value = string.Empty;
                        dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        dgv.CurrentCell = dgv[0, dgv.CurrentCell.RowIndex];
                        dgv.CurrentCell = dgv[dgv.CurrentCell.ColumnIndex, dgv.CurrentCell.RowIndex];
                    }
                }
            };

            mnuPaste.Click += new EventHandler((s, e) => Mousepress_PasteActionGridCell(s, e, dgv, validation));
        }
        void Mousepress_PasteActionGridCell(object sender, EventArgs e, DataGridView dgv, string validation)
        {
            if (Clipboard.ContainsText())
            {
               
                Regex Regex = validation == "NameChecking" ? new Regex("^[a-zA-Z0-9!@#$%^&*()_+.,-/ ]*$"): validation == "NameCheckingProduct" ? new Regex("^[a-zA-Z0-9!@$%^&*()_+.,'\"-/ ]*$") : validation == "EmailChecking" ? new Regex("^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$") : validation == "WebsiteNameChecking" ? new Regex(@"^[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$") : validation == "NumberDot" ? new Regex("^\\$?(\\d{1,3},?(\\d{3},?)*\\d{3}(.\\d{0,3})?|\\d{1,3}(.\\d{2})?)$") : validation == "TaxDetailsNumberChecking" ? new Regex("^[a-zA-Z0-9-]*$") : validation == "LoginChecking" ? new Regex("^[a-zA-Z0-9,.@]*$") : validation == "Number" ? new Regex("^[0-9]*$") : null;
                var clipData = Clipboard.GetDataObject().GetData(typeof(string));
                if (Regex != null && Regex.Match(clipData.ToString()).Success)
                {
                    dgv.Rows[dgv.CurrentCell.RowIndex].Cells[dgv.CurrentCell.ColumnIndex].Value = Clipboard.GetData(DataFormats.Text);
                    dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    dgv.Select();
                    dgv.CurrentCell = dgv[0, dgv.CurrentCell.RowIndex];
                    dgv.CurrentCell = dgv[dgv.CurrentCell.ColumnIndex, dgv.CurrentCell.RowIndex];
                }
            }
        }
      

        public void Keypress_WebsiteNameChecking(object sender, KeyPressEventArgs e)
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
        public void Keypress_NumberDotBraket(object sender, KeyPressEventArgs e, string txt)
        {
            if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
            {
                if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == '.') || (e.KeyChar == ',') || (e.KeyChar == '(') || (e.KeyChar == ')')))
                {
                    e.Handled = true;
                }
                else if (e.KeyChar == '.' && txt.IndexOf('.') > -1)
                {
                    e.Handled = true;
                }
            }
        }
        public bool PhoneValidation(string PhoneText)
        {
            MaskedTextProvider PhoneTextProvide = new MaskedTextProvider(Global.Company.PhoneFormat);
            PhoneTextProvide.Set(PhoneText);
            var isValid = PhoneTextProvide.MaskFull;
            if (isValid)
            {
                return true;

            }            
            return false;
        }
        public bool MobileValidation(string MobileText)
        {
            MaskedTextProvider MobileTextProvider = new MaskedTextProvider(Global.Company.MobileFormat);
            MobileTextProvider.Set(MobileText);
            var isValid = MobileTextProvider.MaskFull;
            if (isValid)
            {
                return true;

            }
            return false;
        }
        public bool EmailValidation(string Text)
        {
            Regex Regex = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}$",RegexOptions.IgnoreCase);
            if(Regex.IsMatch(Text))
            {
                return true;
            }
            return false;
        }
        public bool WebsiteValidation(string source)
        {
            Regex Regex = new Regex(@"(([\w]+:)?\/\/)?(([\d\w]|%[a-fA-f\d]{2,2})+(:([\d\w]|%[a-fA-f\d]{2,2})+)?@)?([\d\w][-\d\w]{0,253}[\d\w]\.)+[\w]{2,63}(:[\d]+)?(\/([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)*(\?(&?([-+_~.\d\w]|%[a-fA-f\d]{2,2})=?)*)?(#([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)?", RegexOptions.IgnoreCase);           
            if(Regex.IsMatch(source))
            {
                return true;
            }
            return false;
        }
        public void Keypress_TaxDetailsNumberChecking(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
            {
                if (!(Char.IsLetterOrDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == '-') || (e.KeyChar == '.')))
                    e.Handled = true;
            }
        }
        public void Keypress_LoginChecking(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape)&& !char.IsControl(e.KeyChar))
            {
                if (!(Char.IsLetterOrDigit(e.KeyChar) || (e.KeyChar == ',') || (e.KeyChar == '.') || (e.KeyChar == '@')))
                    e.Handled = true;
            }
        }
        public void Keypress_EmailChecking(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
            {
                if (!(Char.IsLetterOrDigit(e.KeyChar) || (e.KeyChar == '.') || (e.KeyChar == '@') || (e.KeyChar == '-') || (e.KeyChar == '_')))
                {
                    e.Handled = true;
                }
            }
        }

        public void Keypress_NumberDot(object sender, KeyPressEventArgs e, string txt)
        {
            if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
            {

                if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == '.')))
                {
                    e.Handled = true;
                }
                else if (e.KeyChar == '.' && txt.IndexOf('.') > -1)
                {
                    e.Handled = false;
                }
            }
        }
        public void Keypress_Number(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
            {

                if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == '.')))
                {
                    e.Handled = true;
                }
            }
        }
        public void Keypress_Num(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
            {

                if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                {
                    e.Handled = true;
                }
            }
        }
        
    }
}
