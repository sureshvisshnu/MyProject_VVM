namespace Fa.views.utils.TextSearch
{
    public class ComboBoxTextSearchHelper
    {
        private readonly ToolStripComboBox toolStripComboBox;
        private readonly ComboBox comboBox;
        private readonly System.Windows.Forms.Timer searchTimer = new System.Windows.Forms.Timer();
        private string lastTypedText = string.Empty;

        public ComboBoxTextSearchHelper(ToolStripComboBox toolStripComboBox)
        {
            this.toolStripComboBox = toolStripComboBox;
            searchTimer.Interval = 500; // Adjust the delay in milliseconds (e.g., 500 ms = 0.5 seconds)
            searchTimer.Tick += SearchTimer_Tick;

            toolStripComboBox.TextChanged += ToolStripComboBox_TextChanged;
        }

        public ComboBoxTextSearchHelper(ComboBox comboBox)
        {
            this.comboBox = comboBox;
            searchTimer.Interval = 500; // Adjust the delay in milliseconds (e.g., 500 ms = 0.5 seconds)
            searchTimer.Tick += SearchTimer_Tick;

            comboBox.TextChanged += ComboBox_TextChanged;
        }

        private void ToolStripComboBox_TextChanged(object? sender, EventArgs e)
        {
            lastTypedText = toolStripComboBox.Text.ToLower();
            searchTimer.Stop();
            searchTimer.Start();
        }

        private void ComboBox_TextChanged(object? sender, EventArgs e)
        {
            lastTypedText = comboBox.Text.ToLower();
            searchTimer.Stop();
            searchTimer.Start();
        }

        private void SearchTimer_Tick(object? sender, EventArgs e)
        {
            searchTimer.Stop();

            if (lastTypedText.Length >= 2)
            {
                if (toolStripComboBox != null)
                {
                    SearchToolStripComboBoxItems();
                }
                else if (comboBox != null)
                {
                    SearchComboBoxItems();
                }
            }
        }

        private void SearchToolStripComboBoxItems()
        {
            for (int i = 0; i < toolStripComboBox.Items.Count; i++)
            {
                object item = toolStripComboBox.Items[i];
                if (item != null)
                {
                    string itemText = item.ToString().ToLower();
                    if (itemText.Contains(lastTypedText))
                    {
                        toolStripComboBox.SelectedIndex = i;
                        return;
                    }
                }
            }
        }

        private void SearchComboBoxItems()
        {
            string searchText = comboBox.Text.ToLower();
            comboBox.Items.Clear(); // Clear existing items

            // Iterate through all items in the ComboBox
            foreach (object item in comboBox.Items)
            {
                if (item != null)
                {
                    string itemText = item.ToString().ToLower();
                    // Check if the item contains the typed text
                    if (itemText.Contains(searchText))
                    {
                        // Add the item to the dropdown list
                        comboBox.Items.Add(item);
                    }
                }
            }

            // Show the dropdown list if there are matching items
            if (comboBox.Items.Count > 0)
            {
                comboBox.DroppedDown = true;
            }
            else
            {
                // Hide the dropdown list if no matching items are found
                comboBox.DroppedDown = false;
            }
        }


    }
}
