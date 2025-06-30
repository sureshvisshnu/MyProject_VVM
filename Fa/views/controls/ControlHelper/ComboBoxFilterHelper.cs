using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fa.views.controls.ControlHelper
{
    public static class ComboBoxFilterHelper
    {
        public static void ApplySearchableComboBox<T>(ComboBox comboBox, List<T> dataSource, string displayMember, string valueMember = "Id")
        {
            // Store original data
            var originalData = new List<T>(dataSource);

            // Set up initial properties
            comboBox.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox.AutoCompleteMode = AutoCompleteMode.None; // We'll handle this manually
            comboBox.DataSource = originalData;
            comboBox.DisplayMember = displayMember;
            comboBox.ValueMember = valueMember;

            // Handle text changes
            comboBox.TextChanged += (sender, e) =>
            {
                string searchText = comboBox.Text.ToLower();

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    comboBox.DataSource = originalData;
                }
                else
                {
                    comboBox.DataSource = originalData
                        .Where(item =>
                        {
                            string displayValue = item.GetType().GetProperty(displayMember)?
                                .GetValue(item)?.ToString()?.ToLower() ?? "";
                            return displayValue.Contains(searchText);
                        })
                        .ToList();
                }

                comboBox.DroppedDown = true;
                comboBox.SelectionStart = comboBox.Text.Length;
                comboBox.Select(comboBox.Text.Length, 0);
            };

            // Handle when dropdown is shown
            comboBox.DropDown += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(comboBox.Text))
                {
                    comboBox.DataSource = originalData;
                }
            };

            // Handle when leaving the control
            comboBox.Leave += (sender, e) =>
            {
                if (comboBox.SelectedIndex == -1 && !string.IsNullOrWhiteSpace(comboBox.Text))
                {
                    comboBox.Text = "";
                    comboBox.DataSource = originalData;
                }
            };
        }
    }
}
