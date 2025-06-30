using System.Windows.Forms;

namespace fa.views.controls
{
    public partial class Ab2ToolStrip : ToolStrip
    {
 

        public Ab2ToolStrip()
        {         
            InitializeComponent();
            
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);
        }
    }
}
