using fa.views.controls.text;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace fa.views.controls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip |
                                      ToolStripItemDesignerAvailability.ToolStrip)]
    public partial class ToolStripLocation : ToolStripControlHost
    {
        private UserControlPoint LocationTextBox = new UserControlPoint();
        public ToolStripLocation() : base(new UserControlPoint())
        {
            this.Size = new Size(100, 25);
            this.LocationTextBox = this.Control as UserControlPoint;
        }
        protected override void OnSubscribeControlEvents(Control c)
        {
            base.OnSubscribeControlEvents(c);
        }

        protected override void OnUnsubscribeControlEvents(Control c)
        {
            base.OnUnsubscribeControlEvents(c);
        }
    }
}
