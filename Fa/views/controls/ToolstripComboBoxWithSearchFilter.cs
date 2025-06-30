using Cairo;
using Fa.views.controls.ComboBoxSearchable;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using VisioForge.MediaFramework.DSP;

namespace fa.views.controls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.MenuStrip)]
    public class ToolstripComboBoxWithSearchFilter : ToolStripControlHost
    {

        [Description("extra free-form attributes on this thing.")]
        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor," +
        "System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
        typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(CsvConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<String> AdditionalStuff
        {
            get
            {
                if (_attributes == null)
                    _attributes = new List<String>();
                return _attributes;
            }
        }
        private List<String> _attributes;
        //private List<string> myList = new List<string>();
        //[Editor("System.Windows.Forms.Design.StringCollectionEditor, " +
        //    "System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
        //    typeof(UITypeEditor))]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]

        //public List<string> MyList
        //{
        //    get
        //    {
        //        return myList;
        //    }
        //    set
        //    {
        //        myList = value;
        //    }
        //}
        /// <summary>
        /// undo here
        /// </summary>

        // Make the collection property visible in the designer
        //[Browsable(true)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //[Editor(typeof(StringCollectionEditor), typeof(UITypeEditor))]
        //public StringCollection Items
        //{
        //    get { return items; }
        //    set
        //    {
        //        items = value;
        //        UpdateComboBoxItems(); // Update the ComboBoxWithSearchFilter control
        //    }
        //}
        // upto here
        // Constructor
        public ToolstripComboBoxWithSearchFilter() : base(new ComboBoxWithSearchFilter())
        {
            MyMethod();
        }

        // Expose the ComboBoxWithSearchFilter control
        public ComboBoxWithSearchFilter? ComboBoxWithSearchFilterControl
        {
            get { return Control as ComboBoxWithSearchFilter; }
        }

        public void MyMethod()
        {
            // Call the ComboBoxWithSearchFilterControl property
            ComboBoxWithSearchFilter? comboBox = ComboBoxWithSearchFilterControl;

            // Check if the comboBox is not null before using it
            if (comboBox != null)
            {
                // Use the comboBox instance
                comboBox.UpdateComboBox(); // Replace SomeMethod with an actual method of ComboBoxWithSearchFilter
            }
            else
            {
                // Handle the case where the ComboBoxWithSearchFilterControl is null
                Console.WriteLine("ComboBoxWithSearchFilterControl is null");
            }
        }

        private void UpdateComboBoxItems()
        {
            //if (ComboBoxWithSearchFilterControl != null)
            //{
            //    ComboBoxWithSearchFilterControl.Items.Clear(); // Clear the ComboBox items
            //    foreach (string item in items)
            //    {
            //        ComboBoxWithSearchFilterControl.Items.Add(item); // Add each item from the StringCollection to the ComboBox
            //    }
            //    // Clear existing items and add new items from the ToolstripComboBoxWithSearchFilter
            //    //ComboBoxWithSearchFilterControl.Items.Clear();
            //    //ComboBoxWithSearchFilterControl.Items.AddRange(items.ToArray());
            //}
        }
    }
    public class CsvConverter : TypeConverter
    {
        // Overrides the ConvertTo method of TypeConverter.
        public override object ConvertTo(ITypeDescriptorContext context,
           CultureInfo culture, object value, Type destinationType)
        {
            List<String> v = value as List<String>;
            if (destinationType == typeof(string))
            {
                return String.Join(",", v.ToArray());
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    public class MyStringListEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            // Your custom editor logic goes here
            return base.EditValue(context, provider, value);
        }
    }
}
