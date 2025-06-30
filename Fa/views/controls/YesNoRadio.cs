using System;
using System.Windows.Forms;

namespace fa.views.controls
{
    public partial class YesNoRadio : UserControl
    {
      
        private bool Check=false;       
        public YesNoRadio()
        {
            InitializeComponent();
        }
        
        public string FirstButtonName
        {
            get
            {
                return radioButton1.Text;
            }
            set
            {
                radioButton1.Text = value;
            }
        }
        public string SecondButtonName
        {
            get
            {
                return radioButton2.Text;
            }
            set
            {
                radioButton2.Text = value;
            }
        }
        public bool Checked
        {
            get
            {
                if (radioButton1.Checked) Check = true; else Check = false;
                return Check;
            }
            set
            {
               Check = value;
                if (value) radioButton1.Checked = true; else radioButton2.Checked = true;
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                radioButton2.Checked = false;
            }
            OnLoad(e);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                radioButton1.Checked = false;
            }
            OnLoad(e);
        }
        private void YesNoRadio_SizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }

        private void YesNoRadio_Resize(object sender, EventArgs e)
        {
            SizeChange();
        }
        private void SizeChange()
        {
            if (this.Width > 102)
            {
                radioButton1.Width = (this.Width/2);
                radioButton2.Width = (this.Width/2);
            }
        }

        private void radioButton1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            OnPreviewKeyDown(e);
        }

        private void radioButton2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            OnPreviewKeyDown(e);
        }
    }
}
