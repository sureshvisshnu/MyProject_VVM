using fa.views.controls.ComboListView;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using static fa.views.controls.ComboListView.CheckedComboBox.Dropdown;

namespace Fa.views.controls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.MenuStrip)]
    public partial class ToolstripCheckedListComboBox : ToolStripControlHost
    {
        public event EventHandler? ItemCheckedEvent;
        public CheckedListComboBox? CheckedListComboBox => Control as CheckedListComboBox;

        public ToolstripCheckedListComboBox() : base(new CheckedListComboBox())
        {
            InitializeComponent();
            if (CheckedListComboBox != null)
            {
                CheckedListComboBox.ItemCheckedEvent += OnItemChecked;
            }
        }

        private void OnItemChecked(object? sender, CheckedListComboBox.SelectedItemsChangedEventArgs e)
        {
            ItemCheckedEvent?.Invoke(this, EventArgs.Empty);
        }

        public void AddNode(string text, int id, bool isChecked = false)
        {
            CheckedListComboBox!.AddNode(text, id, isChecked);
        }
        public List<string> OriginalItems
        {
            get
            {
                return CheckedListComboBox?.OriginalItems() ?? new List<string>();
            }
        }
        public List<string> Nodes
        {
            get
            {
                return CheckedListComboBox?.GetNodes() ?? new List<string>();
            }
        }
        public Dictionary<string, bool> checkedStates
        {
            get
            {
                return CheckedListComboBox?.GetcheckedStates() ?? new Dictionary<string, bool>();
            }
        }
        public void Reset()
        {
            CheckedListComboBox?.Reset();
        }
    }

    public class CheckedListComboBox : ComboBox
    {
        public delegate void ItemCheckedDelegate();
        public event EventHandler<SelectedItemsChangedEventArgs> ItemCheckedEvent;

        private CheckedListBox checkedListBox;
        private Form dropDownForm;
        private bool isDropDownVisible = false;
        private const string AllOptionText = "All";
        private bool isUpdating = false;
        private Dictionary<string, bool> checkedStates = new Dictionary<string, bool>();
        private Dictionary<string, long> idMapping = new Dictionary<string, long>();
        private List<string> originalItems = new List<string>();
        private bool isFocusChangeIntentional = false;
        private Form? _parentForm;
        private IMessageFilter mouseMessageFilter;

        public int MaxDropDownHeight { get; set; } = 180;

        public CheckedListComboBox()
        {
            checkedListBox = new CheckedListBox
            {
                CheckOnClick = true,
                IntegralHeight = false
            };

            checkedListBox.ItemCheck += CheckedListBox_ItemCheck;

            dropDownForm = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual,
                ShowInTaskbar = false,
                TopMost = true,
                BackColor = Color.White,
                Size = new Size(this.Width, MaxDropDownHeight)
            };
            dropDownForm.Deactivate += DropDownForm_Deactivate;
            dropDownForm.Controls.Add(checkedListBox);
            this.AutoCompleteMode = AutoCompleteMode.None;
            this.AutoCompleteSource = AutoCompleteSource.None;

            this.DropDownStyle = ComboBoxStyle.DropDown;
            this.TextChanged += CheckedComboBox_TextChanged;

            this.ParentChanged += CheckedComboBox_ParentChanged;
            this.MouseDown += (sender, e) => ShowCheckedListBox();
            checkedListBox.KeyDown += CheckedListBox_KeyDown;
            mouseMessageFilter = new MouseMessageFilter(this);
            Application.AddMessageFilter(mouseMessageFilter);
        }

        public void AddNode(string text, long id, bool isChecked = false)
        {
            if (checkedListBox.Items.Count == 1 && !checkedListBox.Items.Contains(AllOptionText))
            {
                checkedListBox.Items.Insert(0, AllOptionText);
                checkedListBox.SetItemChecked(0, false);
                var newCheckedStates = new Dictionary<string, bool>
                {
                    { AllOptionText, false }
                };
                foreach (var item in checkedStates)
                {
                    newCheckedStates[item.Key] = item.Value;
                }
                checkedStates = newCheckedStates;
                originalItems.Insert(0, AllOptionText);
            }

            checkedListBox.Items.Add(text);
            checkedListBox.SetItemChecked(checkedListBox.Items.Count - 1, isChecked);
            originalItems.Add(text);

            checkedStates[text] = isChecked;
            idMapping[text] = id;
        }

        protected override void OnClick(EventArgs e)
        {
            if (!isDropDownVisible)
            {
                HideCheckedListBox();
            }
            base.OnClick(e);
        }

        public List<string>? OriginalItems()
        {
            return originalItems.Cast<string>().ToList();
        }

        public long GetId(string text)
        {
            if (idMapping.ContainsKey(text))
            {
                return idMapping[text];
            }
            return -1;
        }

        public void ClearNodes()
        {
            checkedListBox.Items.Clear();
            checkedStates.Clear();
            idMapping.Clear();
        }
        public void Reset()
        {
            this.Text = string.Empty;
            checkedListBox.BeginUpdate();
            try
            {
                checkedListBox.Items.Clear();
                foreach (var key in checkedStates.Keys.ToList())
                {
                    checkedStates[key] = false;
                }
                if (checkedStates.Count > 1)
                {
                    checkedListBox.Items.Add(AllOptionText, false);
                }
                foreach (var item in checkedStates.Keys)
                {
                    if (item != AllOptionText)
                    {
                        checkedListBox.Items.Add(item, false);
                    }
                }
            }
            finally
            {
                checkedListBox.EndUpdate();
            }
            OnSelectedItemsChanged(GetcheckedStates());
            HideCheckedListBox();
        }

        public List<string> GetNodes()
        {
            return checkedListBox.Items.Cast<string>().ToList();
        }

        public List<string> GetCheckedItems()
        {
            return checkedListBox.CheckedItems.Cast<string>().ToList();
        }
        public Dictionary<string, bool> GetcheckedStates()
        {
            return new Dictionary<string, bool>(checkedStates);
        }

        public void SetNodeCheckedState(string text, bool isChecked)
        {
            int index = checkedListBox.Items.IndexOf(text);
            if (index >= 0)
            {
                checkedListBox.SetItemChecked(index, isChecked);
                checkedStates[text] = isChecked;
            }
        }

        private void CheckedComboBox_TextChanged(object? sender, EventArgs e)
        {
            string filterText = this.Text.Trim().ToLower();
            Cursor.Current = Cursors.WaitCursor;

            checkedListBox.BeginUpdate();
            try
            {
                if (string.IsNullOrEmpty(filterText))
                {
                    checkedListBox.Items.Clear();
                    var itemsToAdd = new List<object>();
                    if (checkedStates.Count > 1)
                    {
                        itemsToAdd.Add(AllOptionText);
                    }
                    itemsToAdd.AddRange(checkedStates.Keys.Where(item => item != AllOptionText));
                    checkedListBox.Items.AddRange(itemsToAdd.ToArray());
                    for (int i = 0; i < checkedListBox.Items.Count; i++)
                    {
                        string itemText = checkedListBox.Items[i].ToString()!;
                        checkedListBox.SetItemChecked(i, checkedStates.ContainsKey(itemText) && checkedStates[itemText]);
                    }
                }
                else if (filterText.Length >= 3)
                {
                    checkedListBox.Items.Clear();
                    var filteredItems = checkedStates.Keys
                        .Where(item => item.ToLower().Contains(filterText) && item != AllOptionText)
                        .ToList();
                    checkedListBox.Items.AddRange(filteredItems.ToArray());
                    for (int i = 0; i < checkedListBox.Items.Count; i++)
                    {
                        string itemText = checkedListBox.Items[i].ToString()!;
                        checkedListBox.SetItemChecked(i, checkedStates.ContainsKey(itemText) && checkedStates[itemText]);
                    }
                }
                if (isDropDownVisible)
                {
                    isDropDownVisible = false;
                    ShowCheckedListBox();
                }
            }
            finally
            {
                checkedListBox.EndUpdate();
                Cursor.Current = Cursors.Default;
            }
        }

        private void CheckedListBox_ItemCheck(object? sender, ItemCheckEventArgs e)
        {
            if (isUpdating) return;
            isUpdating = true;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string itemText = checkedListBox.Items[e.Index].ToString()!;

                if (itemText == AllOptionText)
                {
                    bool newCheckedState = e.NewValue == CheckState.Checked;
                    foreach (var key in checkedStates.Keys.ToList())
                    {
                        checkedStates[key] = newCheckedState;
                    }
                    for (int i = 1; i < checkedListBox.Items.Count; i++)
                    {
                        checkedListBox.SetItemChecked(i, newCheckedState);
                    }
                }
                else
                {
                    checkedStates[itemText] = e.NewValue == CheckState.Checked;
                    UpdateAllOptionState(e);
                }
                OnSelectedItemsChanged(GetcheckedStates());
            }
            finally
            {
                isUpdating = false;
            }
        }
        public class SelectedItemsChangedEventArgs : EventArgs
        {
            private Dictionary<string, bool> selectedItems;

            public List<string> SelectedItems { get; }

            public SelectedItemsChangedEventArgs(List<string> selectedItems)
            {
                SelectedItems = selectedItems;
            }

            public SelectedItemsChangedEventArgs(Dictionary<string, bool> selectedItems)
            {
                this.selectedItems = selectedItems;
            }
        }
        protected virtual void OnSelectedItemsChanged(Dictionary<string, bool> selectedItems)
        {
            ItemCheckedEvent?.Invoke(this, new SelectedItemsChangedEventArgs(selectedItems));
        }

        private void UpdateAllOptionState(ItemCheckEventArgs e)
        {
            checkedListBox.ItemCheck -= CheckedListBox_ItemCheck;
            try
            {
                if (checkedStates.ContainsKey(AllOptionText))
                {
                    bool allChecked = checkedStates.Keys.Where(key => key != AllOptionText).All(key => checkedStates[key]);
                    if (checkedListBox.Items.Count == checkedStates.Count)
                    {
                        checkedListBox.SetItemChecked(0, allChecked);
                    }
                    checkedStates[AllOptionText] = allChecked;
                }
            }
            finally
            {
                checkedListBox.ItemCheck += CheckedListBox_ItemCheck;
            }
        }

        private void ShowCheckedListBox()
        {
            if (isDropDownVisible) return;

            int itemHeight = checkedListBox.ItemHeight;
            int itemsCount = checkedListBox.Items.Count;
            int requiredHeight = itemHeight * itemsCount + 2;

            int dropdownHeight = Math.Min(requiredHeight, MaxDropDownHeight);

            Point location = this.Parent.PointToScreen(new Point(this.Left, this.Bottom));
            dropDownForm.Location = location;
            dropDownForm.Size = new Size(this.Width, dropdownHeight + 2);
            checkedListBox.Dock = DockStyle.Fill;
            Size size = checkedListBox.Size;
            dropDownForm.Size = size;

            dropDownForm.Show();
            isDropDownVisible = true;

            isFocusChangeIntentional = true;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (this.SelectionLength == this.Text.Length)
            {
                this.SelectionStart = this.Text.Length;
                this.SelectionLength = 0;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (this.SelectionLength == this.Text.Length)
            {
                this.SelectionStart = this.Text.Length;
                this.SelectionLength = 0;
            }
        }
        private void CheckedListBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                HideCheckedListBox();
                e.Handled = true;
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back && this.SelectionLength == this.Text.Length)
            {
                this.Text = string.Empty;
                e.Handled = true;
            }
            else
            {
                base.OnKeyDown(e);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                if (isDropDownVisible)
                {
                    HideCheckedListBox();
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void HideCheckedListBox()
        {
            if (!isDropDownVisible) return;

            dropDownForm.Hide();
            isDropDownVisible = false;
        }

        private void DropDownForm_Deactivate(object? sender, EventArgs e)
        {
            if (!isFocusChangeIntentional)
            {
                HideCheckedListBox();
            }
            isFocusChangeIntentional = false;
        }

        private void CheckedComboBox_ParentChanged(object? sender, EventArgs e)
        {
            if (_parentForm != null)
            {
                _parentForm.FormClosing -= ParentForm_FormClosing;
                _parentForm.FormClosed -= ParentForm_FormClosed;
            }

            _parentForm = this.FindForm();

            if (_parentForm != null)
            {
                _parentForm.FormClosing += ParentForm_FormClosing;
                _parentForm.FormClosed += ParentForm_FormClosed;
            }
        }

        private void ParentForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            HideCheckedListBox();
            Application.RemoveMessageFilter(mouseMessageFilter);
            if (dropDownForm != null)
            {
                dropDownForm.Dispose();
                dropDownForm = null!;
            }

            if (checkedListBox != null)
            {
                checkedListBox.Dispose();
                checkedListBox = null!;
            }
        }

        private void ParentForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            HideCheckedListBox();
            Application.RemoveMessageFilter(mouseMessageFilter);
            if (dropDownForm != null)
            {
                dropDownForm.Dispose();
                dropDownForm = null!;
            }

            if (checkedListBox != null)
            {
                checkedListBox.Dispose();
                checkedListBox = null!;
            }
            mouseMessageFilter = null!;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Application.RemoveMessageFilter(mouseMessageFilter);
            }
            base.Dispose(disposing);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_LBUTTONDOWN = 0x0201;
            const int CB_SHOWDROPDOWN = 0x014F;

            if (m.Msg == WM_LBUTTONDOWN || m.Msg == CB_SHOWDROPDOWN)
            {
                ShowCheckedListBox();
                return;
            }
            base.WndProc(ref m);
        }

        private class MouseMessageFilter : IMessageFilter
        {
            private readonly CheckedListComboBox comboBox;

            public MouseMessageFilter(CheckedListComboBox comboBox)
            {
                this.comboBox = comboBox;
            }

            public bool PreFilterMessage(ref Message m)
            {
                const int WM_LBUTTONDOWN = 0x0201;
                const int WM_RBUTTONDOWN = 0x0204;

                if (m.Msg == WM_LBUTTONDOWN || m.Msg == WM_RBUTTONDOWN)
                {
                    if (comboBox.isDropDownVisible)
                    {
                        Point clickPoint = Control.MousePosition;
                        if (!comboBox.dropDownForm.Bounds.Contains(clickPoint) &&
                            !comboBox.RectangleToScreen(comboBox.ClientRectangle).Contains(clickPoint))
                        {
                            comboBox.HideCheckedListBox();
                        }
                    }
                }
                return false;
            }
        }
    }
}