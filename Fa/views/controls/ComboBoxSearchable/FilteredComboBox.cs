using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Fa.views.controls.ComboBoxSearchable
{
    public class FilteredComboBox : ComboBox
    {
        private string oldFilter = string.Empty;
        private string currentFilter = string.Empty;
        private bool isFiltering = false;

        public FilteredComboBox()
        {
            this.DropDownStyle = ComboBoxStyle.DropDown;
            this.AutoCompleteMode = AutoCompleteMode.None;
            this.KeyPress += FilteredComboBox_KeyPress;
            this.TextChanged += FilteredComboBox_TextChanged;
        }

        private void FilteredComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.DroppedDown = false;
                this.SelectedIndex = -1;
                this.Text = currentFilter;
                e.Handled = true;
            }
            else if (e.KeyChar == (char)Keys.Enter)
            {
                this.DroppedDown = false;
                e.Handled = true;
            }
        }

        private void FilteredComboBox_TextChanged(object sender, EventArgs e)
        {
            if (this.Text != oldFilter)
            {
                if (!isFiltering)
                {
                    isFiltering = true;
                    try
                    {
                        PerformFilter();
                        this.DroppedDown = true;
                        Cursor.Current = Cursors.Default;
                    }
                    finally
                    {
                        isFiltering = false;
                    }
                }
                currentFilter = this.Text;
            }
        }

        private void PerformFilter()
        {
            var currentText = this.Text;
            var items = this.Items.Cast<object>().ToList();

            if (string.IsNullOrWhiteSpace(currentText))
            {
                this.Items.Clear();
                this.Items.AddRange(items.ToArray());
                return;
            }

            var filteredItems = items
                .Where(item => item.ToString().IndexOf(currentText, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToArray();

            this.Items.Clear();
            this.Items.AddRange(filteredItems);
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            if (!isFiltering)
            {
                isFiltering = true;
                try
                {
                    var items = this.Items.Cast<object>().ToList();
                    this.Items.Clear();
                    this.Items.AddRange(items.ToArray());
                }
                finally
                {
                    isFiltering = false;
                }
            }
            base.OnDropDownClosed(e);
        }
    }
}
