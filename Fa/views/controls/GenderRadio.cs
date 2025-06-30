using System;
using System.Windows.Forms;

namespace fa.views.controls
{
    public enum GenderSelection
    {
        None,
        Male,
        Female,
        Others

    }
    public partial class GenderRadio : UserControl
    {

        public GenderRadio()
        {
            InitializeComponent();
        }

       
        private GenderSelection Result = GenderSelection.None;
        public GenderSelection Gender
        {
            get
            {
                if (!radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked) { Result = GenderSelection.None; }
                if (radioButton1.Checked) { Result = GenderSelection.Male; }
                if (radioButton2.Checked) { Result = GenderSelection.Female; }
                if (radioButton3.Checked) { Result = GenderSelection.Others; }
                return Result;
            }
            set
            {
                Result = value;
                switch (Result)
                {
                    case GenderSelection.None:
                        radioButton1.Checked = false;
                        radioButton2.Checked = false;
                        radioButton3.Checked = false;
                        break;
                    case GenderSelection.Male:
                        radioButton1.Checked = true;
                        break;
                    case GenderSelection.Female:
                        radioButton2.Checked = true;
                        break;
                    case GenderSelection.Others:
                        radioButton3.Checked = true;
                        break;
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                radioButton2.Checked = false;
                radioButton3.Checked = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                radioButton1.Checked = false;
                radioButton3.Checked = false;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                radioButton2.Checked = false;
                radioButton1.Checked = false;

            }
        }
    }
}
