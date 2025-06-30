using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace fa.views.controls.text
{
    public partial class UserControlPoint : UserControl
    {
        public UserControlPoint()
        {
            InitializeComponent();
        }
        [Browsable(true)]
        public override string Text
        {
            get
            {
                return (Row.Text+Separator.Text+Column.Text);
            }

            set
            {
               
            }
        }
        private int Rows;
        [Browsable(true)]
        public int NoOfRow
        {
            get
            {
                return Rows;
            }
            set
            {
                Rows =(value==null?1: value.ToString()==string.Empty?1:value);
            }
        }
        private int Cols;
        [Browsable(true)]
        public int NoOfColoumns
        {
            get
            {
                return Cols;
            }
            set
            {
                Cols = (value == null ? 1 : value.ToString() == string.Empty ? 1 : value);
            }
        }
        public void clear()
        {
            Row.Text = "1";
            Column.Text = "1";
        }

        public override void ResetText()
        {
            Row.Text = "1";
            Column.Text = "1";
            base.ResetText();
        }
        private void UserControlPoint_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==',')
            {
                Column.Select();
            }
            
        }

        private void Row_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
            {
                if (Char.IsDigit(e.KeyChar))
                {
                    string Unselected = Row.Text;
                    string Temp = e.KeyChar.ToString();
                    if (Row.SelectionLength > 0) { Unselected = Row.Text.Remove(Row.SelectionStart, Row.SelectionLength); }
                    Unselected = Unselected.Insert(Row.SelectionStart, Temp);
                    if (Unselected.Length > 0 && int.Parse(Unselected) > NoOfRow)
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                    e.Handled = true;
                }
            }
            this.OnKeyPress(e);
        }

        private void Column_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
            {
                if (Char.IsDigit(e.KeyChar))
                {
                    if (int.Parse(e.KeyChar.ToString()) > NoOfColoumns)
                    {
                        e.Handled = true;
                    }
                }
            }
            this.OnKeyPress(e);
        }

        private void Row_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            this.OnPreviewKeyDown(e);
        }

        private void Column_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            this.OnPreviewKeyDown(e);
        }

        private void UserControlPoint_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                Column.Select();
            }
            if (e.KeyCode == Keys.Up)
            {
                Row.Select();
            }
        }

        private void Row_Leave(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(Row.Text.Trim())|| int.Parse(Row.Text) == 0)
            {
                Row.Text = "1";
            }
            //else if(Row.Text.Length==1)
            //{
            //    Row.Text = "0"+ Row.Text;
            //}
            
        }

        private void Column_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Column.Text.Trim())|| int.Parse(Column.Text) == 0)
            {
                Column.Text = "1";
            }                       
        }
    }
}
