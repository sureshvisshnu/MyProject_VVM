using Fa.views.controls.ComboBoxSearchable;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace YourNamespace
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.MenuStrip)]
    public class ToolStripComboBoxWithSearchBox : ToolStripControlHost
    {
        private StringCollection items = new StringCollection();

        // Make the collection property visible in the designer
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(StringCollectionEditor), typeof(UITypeEditor))]
        public StringCollection Items
        {
            get { return items; }
            set
            {
                items = value;
                UpdateComboBoxItems(); // Update the ComboBoxWithSearchBox control
            }
        }

        // Constructor
        public ToolStripComboBoxWithSearchBox() : base(new ComboBoxWithSearchBox())
        {
        }

        // Expose the ComboBoxWithSearchBox control
        public ComboBoxWithSearchBox ComboBoxWithSearchBoxControl
        {
            get { return Control as ComboBoxWithSearchBox; }
        }

        private void UpdateComboBoxItems()
        {
            if (ComboBoxWithSearchBoxControl != null)
            {
                ComboBoxWithSearchBoxControl.Items.Clear(); // Clear the ComboBox items
                foreach (string item in items)
                {
                    ComboBoxWithSearchBoxControl.Items.Add(item); // Add each item from the StringCollection to the ComboBox
                }
            }
        }
    }
}
