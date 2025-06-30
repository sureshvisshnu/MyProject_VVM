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
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip |
                                      ToolStripItemDesignerAvailability.ToolStrip)]
    public partial class ToolStripCheckBox : ToolStripControlHost
    {
        private CheckBox CheckBox = new CheckBox();
        public ToolStripCheckBox() : base(new CheckBox())
        {
             this.CheckBox = (CheckBox)base.Control;
            this.CheckBox.BackColor = BackColor;
        }
        public event EventHandler CheckedChanged
		{
			add { this.CheckBox.CheckedChanged += value; }
			remove { this.CheckBox.CheckedChanged -= value; }
		}
		public event EventHandler CheckStateChanged
		{
			add { this.CheckBox.CheckStateChanged += value; }
			remove { this.CheckBox.CheckStateChanged -= value; }
		}
        
		public bool Checked
		{
			get { return this.CheckBox.Checked; }
			set { this.CheckBox.Checked = value; }
		}
		public CheckState CheckState
		{
			get { return this.CheckBox.CheckState; }
			set { this.CheckBox.CheckState = value; }
		}

        public override Color BackColor
        {
            set { this.CheckBox.BackColor = value; }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
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
