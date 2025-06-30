using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Fa.views.controls.ComboBoxSearchable
{
    public partial class ComboBoxWithSearchBox : UserControl
    {
        //public ComboBox ComboBox => comboBox;
        public ComboBoxWithSearchBox()
        {
            InitializeComponent();
            ComboBox.DropDownStyle = ComboBoxStyle.DropDown;
            ComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            ComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            TextBox.TextChanged += TextBox_TextChanged;
        }
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = TextBox.Text.ToLower();
            ComboBox.BeginUpdate();
            ComboBox.Items.Clear();
                foreach (string item in Items)
                {
                    if (item.ToLower().Contains(searchText))
                    {
                    ComboBox.Items.Add(item);
                }
            }
            ComboBox.EndUpdate();
            ComboBox.DroppedDown = true; // Automatically open dropdown to show filtered items
        }
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<string> Items { get; } = new List<string>();

        public void AddItem(string item)
        {
            Items.Add(item);
            ComboBox.Items.Add(item);
        }

        public void ClearItems()
        {
            Items.Clear();
            ComboBox.Items.Clear();
        }
        private void ComboBoxWithSearchBox_Load(object sender, EventArgs e)
        {

        }
    }
}
