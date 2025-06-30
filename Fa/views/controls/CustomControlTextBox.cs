using System;
using System.Windows.Forms;

namespace fa.views.controls
{
    public partial class CustomControlTextBox : MaskedTextBox
    {
        public CustomControlTextBox()
        {
            InitializeComponent();
            this.Mask = "00000.00";
            this.Text = "    000";
            this.TextAlign = HorizontalAlignment.Right;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
           // string[] text = null;
           // if (base.SelectionStart == 0 && base.SelectionLength==5)
           // {
           //     text = base.Text.Split('.');
           //     base.Text = e.KeyChar + "    " + text[1];
           //     base.SelectionStart = 1;
           // }
           //// string[] totalLength = base.Text.Split('.');
           //// string appendBeforeZero = totalLength[0].Trim().Length == 4 ? "0" : totalLength[0].Trim().Length == 3 ? "00" : totalLength[0].Trim().Length == 2 ? "000" : totalLength[0].Trim().Length == 1 ? "0000" : totalLength[0].Trim().Length == 0 ? "00000" : "";
           // //string appendAfterZero = "";


           // if (e.KeyChar == '.')
           // {
           //     if (text[0].Trim().Length < 5)
           //     {
           //         Check();
           //         base.Text.Remove(7, 2);
           //         base.SelectionStart = 7;
           //     }               
           //     e.Handled = true;
           // }

           // if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == '.')))
           // {
           //     e.Handled = true;
           // }
           // else if (e.KeyChar == '.' && base.Text.IndexOf('.') > -1)
           // {
           //     e.Handled = true;
           // }
            base.OnKeyPress(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            //base.SelectionStart = 0;
            //base.SelectionLength = 5;
            //string[] beforeDecimal = base.Text.Split('.');
            //base.SelectionLength = beforeDecimal[0].Length;
            //base.SelectionLength = base.Text.Length;
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            //Check();
            base.OnLostFocus(e);
        }

        protected void Check()
        {
            string[] totalLength = base.Text.Split('.');
            string appendBeforeZero = totalLength[0].Trim().Length == 4 ? " " : totalLength[0].Trim().Length == 3 ? "  " : totalLength[0].Trim().Length == 2 ? "   " : totalLength[0].Trim().Length == 1 ? "    " : totalLength[0].Trim().Length == 0 ? "    0" : "";
            string appendAfterZero = totalLength[1] == null || totalLength[1] == "" ? "00" : totalLength[1];
            if (totalLength[0].Trim().Length < 5)
            {
                base.Text = appendBeforeZero + totalLength[0].Trim() + appendAfterZero;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {

                if (keyData == (Keys.Control | Keys.A))
                {
                    int index = base.SelectionStart + base.SelectionLength;
                    if (index <= 5)
                    {
                        string[] beforeDecimal = base.Text.Split('.');
                        base.SelectionStart = 0;
                        base.SelectionLength = beforeDecimal[0].Length;
                        return true;

                    }
                    else if (index > 5)
                    {
                        base.SelectionStart = base.Text.Length - 2;

                        base.SelectionLength = 2;
                        return true;

                    }

                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void CustomControlTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //if(e.KeyCode==Keys.Back)
            //{
            //    e.IsInputKey = true;
                
            //}
        }

        private void CustomControlTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==(char)8)
            {
                //string uu = base.Text;
                //uu.Remove(base.SelectionStart, 1);
                //uu.Insert(base.SelectionStart, " ");
                //base.Text = uu;
                //e.Handled = true;
            }
        }

        private void CustomControlTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            int Selection = base.SelectionStart;
            if (e.KeyCode==Keys.Back && Selection < 6 && Selection>0)
            {
                string uu = base.Text.Remove(Selection - 1, 1);
                uu=uu.Insert(Selection-1, " ");
                string[] Text = uu.Split('.');
                //uu.Remove(base.SelectionStart, 1);
                //uu.Insert(base.SelectionStart,"");
                base.Text = Text[0]+Text[1];
                base.SelectionStart = Selection-1;
                e.Handled = true;
            }
        }
    }
}
