using fa.views.controls.ComboTreeView;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace fa.views.controls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.MenuStrip)]
    public class ToolStripComboTree : ToolStripControlHost
    {
        private ComboTreeBox ComboTreeBox = new ComboTreeBox();

        public ToolStripComboTree() : base(new ComboTreeBox())
        {
            this.Size = new Size(100, 22);
            this.ComboTreeBox = this.Control as ComboTreeBox;
            this.ComboTreeBox.ShowCheckBoxes = true;
        }

        public override string Text
        {
            get
            {
                return ComboTreeBox.Text;
            }
            set
            {
                ComboTreeBox.Text = value;
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("The collection of top-level nodes contained by the control."), Category("Data")]
        public ComboTreeNodeCollection Nodes
        {
            get
            {
                return ComboTreeBox.Nodes;
            }
            set
            {
                ComboTreeBox.Nodes = value;
            }
        }
        public ComboTreeNode SelectedNode
        {
            get
            {
                return ComboTreeBox.SelectedNode;
            }
            set
            {
                ComboTreeBox.SelectedNode = value;
            }
        }
        //public ComboTreeNodeEventArgs ComboTreeNodeEventArgs AfterCheck
        //{
        //    add { this.ComboTreeBox.AfterCheck += value; }
        //    remove { this.ComboTreeBox.AfterCheck -= value; }
        //}
        protected override void OnSubscribeControlEvents(Control c)
        {
            base.OnSubscribeControlEvents(c);
        }

        protected override void OnUnsubscribeControlEvents(Control c)
        {
            base.OnUnsubscribeControlEvents(c);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            string cc=this.Text;
            base.OnKeyDown(e);
        }
        
    }
    }
