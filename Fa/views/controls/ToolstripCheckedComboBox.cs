using fa.views.controls.ComboListView;
using fa.views.controls.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace fa.views.controls
{
    public delegate void ItemCheckedDelegate(object sender, ItemCheckEventArgs e);

    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.MenuStrip)]

    public partial class ToolstripCheckedComboBox : ToolStripControlHost
    {
        public event ItemCheckedDelegate? ItemCheckedEvent;
        public int MaxDropDownItems { get { return CheckedComboBox.MaxDropDownItems; } set { CheckedComboBox.MaxDropDownItems = value; } }

        private CheckedComboBox CheckedComboBox = new CheckedComboBox();
        public ToolstripCheckedComboBox() : base(new CheckedComboBox())
        {
            CheckedComboBox = (CheckedComboBox)base.Control;
            CheckedComboBox = this.Control as CheckedComboBox;
            CheckedComboBox.PreviewKeyDown += PreviewKeyDown_event!;
            CheckedComboBox.MouseClick += MouseClick_event!;
            CheckedComboBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cclb_ItemCheck!);
            InitializeComponent();
        }
        public int DelayTime
        {
            get
            {
                return CheckedComboBox.DelayTime;
            }
            set { CheckedComboBox.DelayTime = value; }
        }
        public int Searchstartfrom
        {
            get
            {
                return CheckedComboBox.Searchstartfrom;
            }
            set
            {
                CheckedComboBox.Searchstartfrom = value;
            }
        }
        public bool Delay
        {
            get
            {
                return CheckedComboBox.Delay;
            }
            set { CheckedComboBox.Delay = value; }
        }
        private void cclb_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (ItemCheckedEvent != null)
            {
                ItemCheckedEvent(sender, e);
            }
        }
        void MouseClick_event(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }
        public void Add(string n, long d)
        {
            CheckedComboBox.Add(n, d);
        }
        public void AddRange(CCBoxItem[] item)
        {
            CheckedComboBox.AddRange(item);
        }
        public void Remove(string n, long d)
        {
            CheckedComboBox.Remove(n, d);
        }
        public CheckedListBox.CheckedItemCollection CheckedItems
        {
            get
            {
                return CheckedComboBox.CurrentCheckedItems;
            }
        }
        public CheckedListBox.ObjectCollection Items
        {
            get { return CheckedComboBox.CurrentItems; }
        }
        public string GetCheckedItemsString
        {
            get
            {
                return CheckedComboBox.GetCheckedItemsString;
            }
        }
        public void Reset()
        {
            CheckedComboBox.Reset();
        }
        void PreviewKeyDown_event(object sender, PreviewKeyDownEventArgs e)
        {
            KeyEventArgs KeyEventArgs = new KeyEventArgs(e.KeyData);
            base.OnKeyDown(KeyEventArgs);
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
