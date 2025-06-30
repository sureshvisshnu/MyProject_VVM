using Syncfusion.Windows.Forms.Tools;
using System.Windows.Forms;

namespace fa.views.controls.text
{
    public partial class IDTextBox : TextBox
    {

        public IDTextBox()
        {
            InitializeComponent();
        }

        string _Id;
        public string Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
                OnModifiedChanged(EventArgs.Empty);
            }
        }
       
    }
}
