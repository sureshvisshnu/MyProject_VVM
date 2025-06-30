using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.Libs.MediaFoundation.OPM;
using Timer = System.Windows.Forms.Timer;

namespace fa.views.controls.ComboListView
{
    public delegate void ItemCheckedDelegate(object sender, ItemCheckedEventArgs e);

    public class CheckedComboBox : ComboBox
    {
        private int _threshold = 1000;
        private int _SearchFrom;
        private bool _Delay;
        private int _SelectItemIndex;

        public bool Delay
        {
            get
            {
                return _Delay;
            }
            set { _Delay = value; }
        }
        public int DelayTime
        {
            get
            {
                return _threshold;
            }
            set { _threshold = value; }
        }
        public int Searchstartfrom
        {
            get
            {
                return _SearchFrom;
            }
            set { _SearchFrom = value != 0 ? value : 1; }
        }

        public System.Windows.Forms.Timer Timer;

        public CheckedListBox tempcclb = new CheckedListBox();
        public void Add(string n, long d)
        {
            CCBoxItem item = new CCBoxItem(n, d);
            this.Items.Add(item);
            tempcclb.Items.Add(item);
        }
        public void AddRange(CCBoxItem[] item)
        {
            this.Items.AddRange(item);
            tempcclb.Items.AddRange(item);
        }
        public void Remove(string n, long d)
        {
            CCBoxItem item = new CCBoxItem(n, d);
            this.Items.Remove(item);
            tempcclb.Items.Remove(item);
        }
        /// <summary>
        /// Internal class to represent the dropdown list of the CheckedComboBox
        /// </summary>
        internal class Dropdown : Form
        {
            // ---------------------------------- internal class CCBoxEventArgs --------------------------------------------
            /// <summary>
            /// Custom EventArgs encapsulating value as to whether the combo box value(s) should be assignd to or not.
            /// </summary>
            internal class CCBoxEventArgs : EventArgs
            {
                private bool assignValues;
                public bool AssignValues
                {
                    get { return assignValues; }
                    set { assignValues = value; }
                }
                private EventArgs e;
                public EventArgs EventArgs
                {
                    get { return e; }
                    set { e = value; }
                }
                public CCBoxEventArgs(EventArgs e, bool assignValues) : base()
                {
                    this.e = e;
                    this.assignValues = assignValues;
                }
            }

            // ---------------------------------- internal class CustomCheckedListBox --------------------------------------------

            /// <summary>
            /// A custom CheckedListBox being shown within the dropdown form representing the dropdown list of the CheckedComboBox.
            /// </summary>
            ///             

            internal class CustomCheckedListBox : CheckedListBox
            {
                private int curSelIndex = -1;
                //public event EventHandler<int> ItemselectindexValueChanged;

                public CustomCheckedListBox() : base()
                {
                    this.SelectionMode = SelectionMode.One;
                    this.HorizontalScrollbar = true;
                }

                protected override void OnItemCheck(ItemCheckEventArgs ice)
                {
                    base.OnItemCheck(ice);
                    ((CheckedComboBox.Dropdown)Parent).ccbParent.ItemCheck(this, ice);
                }
                /// <summary>
                /// Intercepts the keyboard input, [Enter] confirms a selection and [Esc] cancels it.
                /// </summary>
                /// <param name="e">The Key event arguments</param>
                protected override void OnKeyDown(KeyEventArgs e)
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        // Enact selection.
                        ((CheckedComboBox.Dropdown)Parent).OnDeactivate(new CCBoxEventArgs(null!, true));
                        e.Handled = true;

                    }
                    else if (e.KeyCode == Keys.Escape)
                    {
                        // Cancel selection.
                        ((CheckedComboBox.Dropdown)Parent).OnDeactivate(new CCBoxEventArgs(null!, false));
                        e.Handled = true;

                    }
                    else if (e.KeyCode == Keys.Delete)
                    {
                        e.Handled = true;
                    }
                    // If no Enter or Esc keys presses, let the base class handle it.
                    base.OnKeyDown(e);
                }
                protected override void OnMouseHover(EventArgs e)
                {
                    base.OnMouseHover(e);
                }

                protected override void OnMouseLeave(EventArgs e)
                {
                    base.OnMouseLeave(e);
                }

                protected override void OnSelectedIndexChanged(EventArgs e)
                {
                    base.OnSelectedIndexChanged(e);
                }

                protected override void OnMouseMove(MouseEventArgs e)
                {
                    base.OnMouseMove(e);
                }

            } // end internal class CustomCheckedListBox

            // --------------------------------------------------------------------------------------------------------

            // ********************************************* Data *********************************************

            private CheckedComboBox ccbParent;

            // Keeps track of whether checked item(s) changed, hence the value of the CheckedComboBox as a whole changed.
            // This is simply done via maintaining the old string-representation of the value(s) and the new one and comparing them!
            private string oldStrValue = "";
            public bool ValueChanged
            {
                get
                {
                    string newStrValue = ccbParent.Text;
                    if ((oldStrValue.Length > 0) && (newStrValue.Length > 0))
                    {
                        return (oldStrValue.CompareTo(newStrValue) != 0);
                    }
                    else
                    {
                        return (oldStrValue.Length != newStrValue.Length);
                    }
                }
            }

            // Array holding the checked states of the items. This will be used to reverse any changes if user cancels selection.
            bool[] checkedStateArr;

            // Whether the dropdown is closed.
            private bool dropdownClosed = true;

            private CustomCheckedListBox cclb;
            public CustomCheckedListBox List
            {
                get { return cclb; }
                set { cclb = value; }
            }


            // ********************************************* Construction *********************************************

            public Dropdown(CheckedComboBox ccbParent)
            {
                this.ccbParent = ccbParent;
                InitializeComponent();
                this.ShowInTaskbar = false;
                // Add a handler to notify our parent of ItemCheck events.
                this.cclb!.ItemCheck += new ItemCheckEventHandler(this.cclb_ItemCheck!);
            }

            // ********************************************* Methods *********************************************

            // Create a CustomCheckedListBox which fills up the entire form area.
            private void InitializeComponent()
            {
                this.cclb = new CustomCheckedListBox();
                this.SuspendLayout();
                // 
                // cclb
                // 
                this.cclb.BorderStyle = System.Windows.Forms.BorderStyle.None;
                this.cclb.Dock = System.Windows.Forms.DockStyle.Fill;
                this.cclb.FormattingEnabled = true;
                this.cclb.Location = new Point(0, 0);
                this.cclb.Name = "cclb";
                this.cclb.Size = new Size(47, 15);
                this.cclb.TabIndex = 0;
                this.DoubleBuffered = false;

                // 
                // Dropdown
                // 
                this.AutoScaleDimensions = new SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.BackColor = System.Drawing.SystemColors.Menu;
                this.ClientSize = new Size(47, 16);
                this.ControlBox = false;
                this.Controls.Add(this.cclb);
                this.ForeColor = System.Drawing.SystemColors.ControlText;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
                this.MinimizeBox = false;
                this.Name = "ccbParent";
                this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                this.SetStyle(
    ControlStyles.AllPaintingInWmPaint |
    ControlStyles.UserPaint |
    ControlStyles.DoubleBuffer,
    false);
                this.ResumeLayout(false);
            }

            /// <summary>
            /// Closes the dropdown portion and enacts any changes according to the specified boolean parameter.
            /// NOTE: even though the caller might ask for changes to be enacted, this doesn't necessarily mean
            ///       that any changes have occurred as such. Caller should check the ValueChanged property of the
            ///       CheckedComboBox (after the dropdown has closed) to determine any actual value changes.
            /// </summary>
            /// <param name="enactChanges"></param>
            public void CloseDropdown(bool enactChanges)
            {
                if (dropdownClosed)
                {
                    return;
                }
                Debug.WriteLine("CloseDropdown");
                // Perform the actual selection and display of checked items.
                if (enactChanges)
                {
                    //ccbParent.SelectedIndex = -1;
                    // Set the text portion equal to the string comprising all checked items (if any, otherwise empty!).
                    //ccbParent.Text = GetCheckedItemsStringValue();

                }
                else
                {
                    // Caller cancelled selection - need to restore the checked items to their original state.
                    for (int i = 0; i < cclb.Items.Count; i++)
                    {
                        cclb.SetItemChecked(i, checkedStateArr[i]);
                    }
                }
                // From now on the dropdown is considered closed. We set the flag here to prevent OnDeactivate() calling
                // this method once again after hiding this window.
                dropdownClosed = true;
                // Set the focus to our parent CheckedComboBox and hide the dropdown check list.
                //ccbParent.Focus();
                if (/*string.IsNullOrEmpty(ccbParent.Text)*/ List.Items.Count == 0)
                {
                    this.Hide();
                }
                // Notify CheckedComboBox that its dropdown is closed. (NOTE: it does not matter which parameters we pass to
                // OnDropDownClosed() as long as the argument is CCBoxEventArgs so that the method knows the notification has
                // come from our code and not from the framework).
                ccbParent.OnDropDownClosed(new CCBoxEventArgs(null!, false));
            }

            protected override void OnActivated(EventArgs e)
            {
                Debug.WriteLine("OnActivated");
                base.OnActivated(e);
                dropdownClosed = false;
                // Assign the old string value to compare with the new value for any changes.
                oldStrValue = ccbParent.Text;
                // Make a copy of the checked state of each item, in cace caller cancels selection.
                checkedStateArr = new bool[cclb.Items.Count];
                for (int i = 0; i < cclb.Items.Count; i++)
                {
                    checkedStateArr[i] = cclb.GetItemChecked(i);
                }
            }

            protected override void OnDeactivate(EventArgs e)
            {
                Debug.WriteLine("OnDeactivate");
                base.OnDeactivate(e);
                CCBoxEventArgs? ce = e as CCBoxEventArgs;
                if (ce != null)
                {
                    CloseDropdown(ce.AssignValues);

                }
                else
                {
                    // If not custom event arguments passed, means that this method was called from the
                    // framework. We assume that the checked values should be registered regardless.
                    CloseDropdown(true);
                }
            }

            private void cclb_ItemCheck(object sender, ItemCheckEventArgs e)
            {
                int index = ccbParent.tempcclb.FindStringExact(List.GetItemText(List.Items[e.Index]));
                if (index > -1)
                {
                    ccbParent.tempcclb.SetItemChecked(index, e.NewValue == 0 ? false : true);
                }
            }

        } // end internal class Dropdown

        // ******************************** Data ********************************
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null!;
        // A form-derived object representing the drop-down list of the checked combo box.
        private Dropdown dropdown;


        // The valueSeparator character(s) between the ticked elements as they appear in the 
        // text portion of the CheckedComboBox.
        private string valueSeparator;
        public string ValueSeparator
        {
            get { return valueSeparator; }
            set { valueSeparator = value; }
        }

        public bool CheckOnClick
        {
            get { return dropdown.List.CheckOnClick; }
            set { dropdown.List.CheckOnClick = value; }
        }

        public new string DisplayMember
        {
            get { return dropdown.List.DisplayMember; }
            set { dropdown.List.DisplayMember = value; }
        }

        public new CheckedListBox.ObjectCollection Items
        {
            get { return dropdown.List.Items; }
        }

        public CheckedListBox.CheckedItemCollection CheckedItems
        {
            get { return dropdown.List.CheckedItems; }
        }

        public CheckedListBox.CheckedIndexCollection CheckedIndices
        {
            get { return dropdown.List.CheckedIndices; }
        }

        public bool ValueChanged
        {
            get { return dropdown.ValueChanged; }
        }

        // Event handler for when an item check state changes.
        public event ItemCheckEventHandler ItemCheck;

        // ******************************** Construction ********************************

        public CheckedComboBox() : base()
        {
            // We want to do the drawing of the dropdown.
            this.DrawMode = DrawMode.OwnerDrawVariable;
            // Default value separator.
            valueSeparator = ", ";
            // This prevents the actual ComboBox dropdown to show, although it's not strickly-speaking necessary.
            // But including this remove a slight flickering just before our dropdown appears (which is caused by
            // the empty-dropdown list of the ComboBox which is displayed for fractions of a second).
            this.DropDownHeight = 1;
            // This is the default setting - text portion is editable and user must click the arrow button
            // to see the list portion. Although we don't want to allow the user to edit the text portion
            // the DropDownList style is not being used because for some reason it wouldn't allow the text
            // portion to be programmatically set. Hence we set it as editable but disable keyboard input (see below).
            this.DropDownStyle = ComboBoxStyle.DropDown;
            this.dropdown = new Dropdown(this);
            // CheckOnClick style for the dropdown (NOTE: must be set after dropdown is created).
            this.CheckOnClick = true;
        }


        // ******************************** Operations ********************************

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);
            if (dropdown.List.Items.Count >= 1)
            {
                DoDropDown();
            }
        }
        bool isDropdown = false;
        private void DoDropDown()
        {

            isDropdown = false;
            string SearchPattern = this.Text;
            //if (!string.IsNullOrEmpty(SearchPattern) && SearchPattern.Length > 2)
            {
                Rectangle rect = RectangleToScreen(this.ClientRectangle);
                dropdown.Location = new Point(rect.X, rect.Y + this.Size.Height);
                int count = dropdown.List.Items.Count;
                if (count > this.MaxDropDownItems)
                {
                    count = this.MaxDropDownItems;
                }
                else if (count >= 1 && count <= this.MaxDropDownItems)
                {
                    count = count + 1;
                }

                dropdown.Size = new Size(this.Size.Width, (dropdown.List.ItemHeight) * count + 2);
                if (!dropdown.Visible)
                {
                    isDropdown = true;
                    dropdown.Show(this);
                    //Parent.Focus();
                    //SendKeys.Send("{End}");
                }
            }
        }
        private int CalculateRequiredDropdownHeight()
        {
            // Calculate the total height needed based on the number of items
            int itemHeight = this.ItemHeight; // Replace with your actual item height
            int visibleItems = this.MaxDropDownItems; // Replace with your actual number of visible items
            int padding = 2; // Adjust this value as needed

            int totalHeight = Math.Min(this.Items.Count, visibleItems) * itemHeight + padding;

            return totalHeight;
        }
        protected override void OnSelectedItemChanged(EventArgs e)
        {
            base.OnSelectedItemChanged(e);
        }
        protected override void OnDropDownClosed(EventArgs e)
        {
            // Call the handlers for this event only if the call comes from our code - NOT the framework's!
            // NOTE: that is because the events were being fired in a wrong order, due to the actual dropdown list
            //       of the ComboBox which lies underneath our dropdown and gets involved every time.
            if (e is Dropdown.CCBoxEventArgs)
            {
                base.OnDropDownClosed(e);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                // Signal that the dropdown is "down". This is required so that the behaviour of the dropdown is the same
                // when it is a result of user pressing the Down_Arrow (which we handle and the framework wouldn't know that
                // the list portion is down unless we tell it so).
                // NOTE: all that so the DropDownClosed event fires correctly!                
                OnDropDown(null!);
            }
            // Make sure that certain keys or combinations are not blocked.
            e.Handled = !e.Alt && !(e.KeyCode == Keys.Tab) &&
                !((e.KeyCode == Keys.Left) || (e.KeyCode == Keys.Right) || (e.KeyCode == Keys.Home) || (e.KeyCode == Keys.End) || (this is CheckedComboBox && e.KeyCode == Keys.Delete));

            base.OnKeyDown(e);
        }
        protected override void OnClick(EventArgs e)
        {
            if (!isDropdown)
            {
                dropdown.Visible = false;
            }
            base.OnClick(e);
            this.Focus();
        }
        bool IsShiftKeyPress = false;
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            IsShiftKeyPress = (Control.ModifierKeys & Keys.Shift) == Keys.Shift ? true : false;
            base.OnKeyPress(e);
        }
        protected override void OnLeave(EventArgs e)
        {
            if (dropdown.Visible)
            {
                dropdown.Visible = false;
            }
            base.OnLeave(e);
        }
        public static async Task<bool> IsMatchCustomDelayed(string input, string pattern, bool ignoreCase = false, bool matchWholeWord = false, int delayMilliseconds = 0)
        {
            if (delayMilliseconds > 0)
            {
                await Task.Delay(delayMilliseconds);
            }
            return IsMatchCustom(input, pattern, ignoreCase, matchWholeWord);
        }
        public static bool IsMatchCustom(string input, string pattern, bool ignoreCase = false, bool matchWholeWord = false)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            RegexOptions options = RegexOptions.None;

            if (ignoreCase)
            {
                options |= RegexOptions.IgnoreCase;
            }

            string regexPattern = matchWholeWord
                ? @"\b" + Regex.Escape(pattern) + @"\b"
                : Regex.Escape(pattern);

            // Create a Regex object with the specified pattern and options
            Regex regex = new Regex(regexPattern, options);

            // Use the IsMatch method to check if the input string matches the pattern
            return regex.IsMatch(input);
        }
        public int FindStringExactCustom(string searchText, CheckedListBox comboBox)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                string itemText = comboBox.GetItemText(comboBox.Items[i]);
                if (string.Equals(itemText, searchText, StringComparison.CurrentCultureIgnoreCase))
                {
                    return i; // Return the index of the found item
                }
            }
            return -1; // Return -1 if the item is not found
        }
        private void ExecuteFunction(EventArgs e)
        {
            try
            {
                string searchPattern = this.Text.Trim();

                HashSet<string> checkedItems = new HashSet<string>();
                foreach (var item in tempcclb.CheckedItems)
                {
                    checkedItems.Add(item.ToString()!);
                }

                this.Items.Clear();

                if (string.IsNullOrEmpty(searchPattern))
                {
                    this.Items.AddRange(tempcclb.Items.Cast<object>().ToArray());
                }
                else
                {
                    foreach (var item in tempcclb.Items)
                    {
                        string itemText = item.ToString()!;
                        bool isMatch = IsMatchCustom(itemText, searchPattern, ignoreCase: true, matchWholeWord: false);

                        if (isMatch)
                        {
                            this.Items.Add(item);
                        }
                    }
                }

                for (int i = 0; i < this.Items.Count; i++)
                {
                    string itemText = this.Items[i].ToString()!;
                    if (checkedItems.Contains(itemText))
                    {
                        this.SetItemChecked(i, true); 
                    }
                }

                if (this.Items.Count > 0)
                {
                    DoDropDown();
                }
                else
                {
                    dropdown.Visible = false;
                }

                this.SelectionStart = this.Text.Length;
                this.SelectionLength = 0;

                base.OnTextChanged(e);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void handleTypingTimerTimeout(object sender, EventArgs e)
        {
            var timer = sender as Timer;
            if (timer == null)
            {
                return;
            }
            timer.Stop();
            ExecuteFunction(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (Delay)
            {
                if (Timer == null)
                {
                    Timer = new Timer();
                    Timer.Interval = DelayTime;
                    Timer.Tick += new EventHandler(this.handleTypingTimerTimeout!);
                }
                Timer.Stop();
                Timer.Start();
            }
            else
            {
                if (this.Text.Length == 0 || this.Text.Length >= Searchstartfrom)
                {
                    ExecuteFunction(e);
                    base.OnTextChanged(e);
                }
            }

        }
        private void LoadAllItems()
        {
            // Load all items into the ComboBox.
            this.Items.Clear();
            this.Items.AddRange(tempcclb.Items);
            foreach (var Item in tempcclb.CheckedItems)
            {
                int index = dropdown.List.FindStringExact(((CCBoxItem)Item).Name.ToLower()) == -1 ? dropdown.List.FindStringExact(((CCBoxItem)Item).Name.ToUpper()) : dropdown.List.FindStringExact(((CCBoxItem)Item).Name.ToLower());
                if (index > -1)
                {
                    this.SetItemChecked(index, true);
                }
            }
        }
        public string GetCheckedItemsStringValue()
        {
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < tempcclb.CheckedItems.Count; i++)
            {
                sb.Append(tempcclb.GetItemText(tempcclb.CheckedItems[i])).Append(this.ValueSeparator);
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - this.ValueSeparator.Length, this.ValueSeparator.Length);
            }
            return sb.ToString();
        }
        public string GetCheckedItemsString
        {
            get
            {
                return GetCheckedItemsStringValue();
            }
        }
        public bool GetItemChecked(int index)
        {
            if (index < 0 || index > Items.Count)
            {
                throw new ArgumentOutOfRangeException("index", "value out of range");
            }
            else
            {
                return dropdown.List.GetItemChecked(index);
            }
        }
        public void Reset()
        {
            this.Items.Clear();
            this.Items.AddRange(tempcclb.Items);
            tempcclb.Items.Clear();
            tempcclb.Items.AddRange(this.Items);
        }
        public CheckedListBox.ObjectCollection CurrentItems
        {
            get { return dropdown.List.Items; }
        }
        public CheckedListBox.CheckedItemCollection CurrentCheckedItems
        {
            get { return tempcclb.CheckedItems; }
        }
        public void SetItemChecked(int index, bool isChecked)
        {
            if (index < 0 || index > Items.Count)
            {
                throw new ArgumentOutOfRangeException("index", "value out of range");
            }
            else
            {
                dropdown.List.SetItemChecked(index, isChecked);

                int Nindex = tempcclb.FindStringExact(dropdown.List.GetItemText(dropdown.List.Items[index]));
                if (Nindex > -1)
                {
                    tempcclb.SetItemChecked(Nindex, isChecked);
                }
            }
        }

        public CheckState GetItemCheckState(int index)
        {
            if (index < 0 || index > Items.Count)
            {
                throw new ArgumentOutOfRangeException("index", "value out of range");
            }
            else
            {
                return dropdown.List.GetItemCheckState(index);
            }
        }

        public void SetItemCheckState(int index, CheckState state)
        {
            if (index < 0 || index > Items.Count)
            {
                throw new ArgumentOutOfRangeException("index", "value out of range");
            }
            else
            {
                dropdown.List.SetItemCheckState(index, state);
                // Need to update the Text.
                //this.Text = dropdown.GetCheckedItemsStringValue();
            }
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // CheckedComboBox
            // 
            AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            AutoCompleteSource = AutoCompleteSource.ListItems;
            TextChanged += CheckedComboBox_TextChanged!;
            KeyPress += CheckedComboBox_KeyPress!;
            MouseHover += CheckedComboBox_MouseHover!;
            MouseMove += CheckedComboBox_MouseMove!;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CheckedComboBox_KeyDown!);
            ResumeLayout(false);
        }
        private void CheckedComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Timer != null)
                {
                    Timer.Stop();
                }
                if (this.Text.Length == 0 || this.Text.Length >= Searchstartfrom)
                {
                    ExecuteFunction(e);
                }
            }
        }
        private void CheckedComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void CheckedComboBox_TextChanged(object sender, EventArgs e)
        {

        }
        private void CheckedComboBox_MouseHover(object sender, EventArgs e)
        {
        }

        private void CheckedComboBox_MouseMove(object sender, MouseEventArgs e)
        {
        }
    } // end public class CheckedComboBox
    public class CCBoxItem
    {
        private long val;
        public long Value
        {
            get { return val; }
            set { val = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public CCBoxItem()
        {
        }

        public CCBoxItem(string name, long val)
        {
            this.name = name;
            this.val = val;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
