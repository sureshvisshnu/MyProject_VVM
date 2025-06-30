using fa.views.controls.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace fa.views.controls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.MenuStrip)]
    public partial class ToolstripDelayedTextBox : ToolStripControlHost
    {
        private DelayedTextChangeTextBox DelayedTextChangeTextBox = new DelayedTextChangeTextBox();
        public ToolstripDelayedTextBox() : base(new DelayedTextChangeTextBox())
        {
            this.AutoSize = false;
            this.Size = new Size(150, 22);
            this.DelayedTextChangeTextBox = this.Control as DelayedTextChangeTextBox;
            this.DelayedTextChangeTextBox.BorderStyle = BorderStyle.FixedSingle;
        }
        public bool Delay
        {
            get
            {
                return DelayedTextChangeTextBox.Delay;
            }
            set { DelayedTextChangeTextBox.Delay = value; }
        }
        public int DelayTime
        {
            get
            {
                return DelayedTextChangeTextBox.DelayTime;
            }
            set { DelayedTextChangeTextBox.DelayTime = value; }
        }
        public void ResetText()
        {
            DelayedTextChangeTextBox.ResetText();
        }
        protected override void OnSubscribeControlEvents(Control c)
        {
            base.OnSubscribeControlEvents(c);
        }

        protected override void OnUnsubscribeControlEvents(Control c)
        {
            base.OnUnsubscribeControlEvents(c);
        }       
        public void Select()
        {
            DelayedTextChangeTextBox.Select();
        }
    }
}
