using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace Dropdown_Button
{

    public delegate void ItemClickedDelegate(object sender, ToolStripItemClickedEventArgs e);

    public partial class UserControlButtonWithMenu : UserControl
    {

        public event ItemClickedDelegate ItemClickedEvent;

        #region Members
        private Collection<string> _Item = new Collection<string>();
        private Collection<string> Item
        {
            get
            {
                return _Item;
            }
            set
            {
                _Item = value;
            }
        }
        private ImageList Img;
        private string Txt;

        #endregion

        #region Property @Ab2
        [Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design",
                    "System.Drawing.Design.UITypeEditor, System.Drawing")]
        public Collection<string> Items
        {
            get
            {
                return Item;
            }
            set
            {
                Item = value;
            }
        }

        public ImageList ImageList
        {
            get
            {
                return Img;
            }
            set
            {
                Img = value;
            }
        }

        public string ButtonText
        {
            get
            {
                return Txt;
            }
            set
            {
                Txt = value;
                btnDropDown.Text = value;
            }
        }

        #endregion


        #region Constructors
        public UserControlButtonWithMenu()
        {
            InitializeComponent();
            // Align the image right of the button
            btnDropDown.ImageAlign = ContentAlignment.MiddleRight;
            //Align the text left of the button.
            btnDropDown.TextAlign = ContentAlignment.MiddleCenter;
        }
        #endregion

        #region Methods
        public void ShowDropDown()
        {

            if (this.Visible)
            {

                ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                //adding contextMenuStrip items acconrding to LstOfValues count
                if (Item != null)
                {
                    for (int i = 0; i < Item.Count; i++)
                    {
                        //add the item
                        contextMenuStrip.Items.Add(Item[i]);
                        //add the image
                        if (Img != null && Img.Images.Count >= Item.Count)
                        {
                            contextMenuStrip.Items[i].Image = Img.Images[i];
                        }
                    }
                }
                //adding ItemClicked event to contextMenuStrip
                contextMenuStrip.ItemClicked += contextMenuStrip_ItemClicked;
                // Handle the KeyDown event to check for function keys
                contextMenuStrip.KeyDown += contextMenuStrip_KeyDown;
                //show menu strip control
                contextMenuStrip.Show(btnDropDown, new Point(0, btnDropDown.Height));
            }
        }

        #endregion

        #region Events

        private void btnDropDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.Visible)
                    ShowDropDown();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void contextMenuStrip_ItemClicked(object? sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                ToolStripItem item = e.ClickedItem;
                //set the text of the button
                //btnDropDown.Text = item.Text;
                if (ItemClickedEvent != null)
                {
                    ItemClickedEvent(sender!, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public event KeyEventHandler FunctionKeyPressed;
        private void contextMenuStrip_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode >= Keys.F1 && e.KeyCode <= Keys.F12)
            {
                FunctionKeyPressed?.Invoke(this, e);
                // Optionally, you can pass the event back to the main form
                // this.ParentForm?.OnKeyDown(e);
            }
        }

        #endregion

    }
}