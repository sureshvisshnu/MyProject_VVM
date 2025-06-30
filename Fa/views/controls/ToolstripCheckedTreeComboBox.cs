using fa.views.controls.ComboTreeView;
using fa.views.controls.text;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace fa.views.controls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.MenuStrip)]
    public partial class ToolstripCheckedTreeComboBox : ToolStripControlHost
    {
        private CheckedTreeComboBox CheckedTreeComboBox = new CheckedTreeComboBox();
        public event NodeClickedDelegate NodeClickedEvent;
        public ToolstripCheckedTreeComboBox() : base(new CheckedTreeComboBox())
        {
            this.CheckedTreeComboBox = (CheckedTreeComboBox)base.Control;
            this.CheckedTreeComboBox = this.Control as CheckedTreeComboBox;
            this.CheckedTreeComboBox.NodeClickedEvent += this.CheckedTreeComboBox_NodeClicked;
            this.CheckedTreeComboBox.PreviewKeyDown += this.PreviewKeyDown_event;
            this.CheckedTreeComboBox.MouseClick += this.MouseClick_event;
        }
        void PreviewKeyDown_event(object sender, PreviewKeyDownEventArgs e)
        {
            KeyEventArgs KeyEventArgs = new KeyEventArgs(e.KeyData);
            base.OnKeyDown(KeyEventArgs);
        }
        void MouseClick_event(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }
        public void CheckedTreeComboBox_NodeClicked(object sender, ComboTreeNodeEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (NodeClickedEvent != null)
                {
                    NodeClickedEvent(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally { Cursor.Current = Cursors.Default; }
        }
        public ComboTreeNode TreeNodes
        {
            set
            {
                CheckedTreeComboBox.TreeNodes = value;
            }
        }
        public ComboTreeNodeCollection Nodes
        {
            get
            {
                return CheckedTreeComboBox.Nodes;
            }
            set
            {
                CheckedTreeComboBox.Nodes = value;
            }
        }
        public IList<ComboTreeNode> CheckedNodes
        {
            get
            {
                return CheckedTreeComboBox.CheckedNodes;
            }
        }
        public ComboTreeNode SelectedNode
        {
            get
            {
                return CheckedTreeComboBox.SelectedNode;
            }
            set
            {
                CheckedTreeComboBox.SelectedNode = value;
            }
        }      
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
            base.OnKeyDown(e);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {          
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
