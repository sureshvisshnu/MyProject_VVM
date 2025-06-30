using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Fa.views.controls.ComboBoxSearchable
{
    public partial class ComboBoxWithSearchFilter : UserControl
    {

        //You need to add reference to System.Design
        /// <summary>
        /// UN DO THIS
        /// </summary>

        private List<string> myList = new List<string>();
        //[Editor(typeof(MyStringCollectionEditor), typeof(UITypeEditor))]
        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor," +
        "System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
        typeof(System.Drawing.Design.UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<string> MyList
        {
            get
            {
                return myList;
            }
            set
            {
                myList = value;
                UpdateComboBox();
            }
        }
        public List<String> StringsContainer
        {
            get
            {
                if (_attributes == null)
                    _attributes = new List<String>();
                return _attributes;
            }
        }
        private List<String> _attributes;
        // UP TO HEWRE

        // public ComboBox ComboBoxControl => ComboBox;
        //private StringCollection dataItems = new StringCollection();
        //[Browsable(true)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //private StringCollection items = new StringCollection();
        //public StringCollection Items
        //{
        //    get { return items; }
        //    set
        //    {
        //        items = value;
        //        UpdateComboBox(); // Update the ComboBox whenever the StringCollection is modified
        //    }
        //}

        public void UpdateComboBox()
        {
            ComboBox.Items.Clear(); // Clear the ComboBox items
            foreach (string item in myList)
            {
                ComboBox.Items.Add(item); // Add each item from the StringCollection to the ComboBox
            }
        }

        public ComboBoxWithSearchFilter()
        {
            
            InitializeComponent();
            //ComboBox.DataSource = ComboBoxControl.DataSource;
        }

        //public void AddItem(string item)
        //{
        //    items.Add(item);
        //}

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ComboBox.BeginUpdate();
            ComboBox.DataSource = null; // Clear the data source
           // ComboBox.DataSource = items; // Rebind to the updated collection
            ComboBox.EndUpdate();
        }


        //public void PopulateComboBox()
        //{
        //    ComboBox.BeginUpdate();
        //    ComboBox.Items.Clear();
        //    foreach (string item in Items)
        //    {
        //        ComboBox.Items.Add(item);
        //    }
        //    ComboBox.EndUpdate();
        //}

        // Add conversion methods here
        //public BindingList<string> ConvertToStringBindingList()
        //{
        //    BindingList<string> bindingList = new BindingList<string>();
        //    foreach (string item in Items)
        //    {
        //        bindingList.Add(item);
        //    }
        //    return bindingList;
        //}

        //public void ConvertToSringCollection(BindingList<string> bindingList)
        //{
        //    Items.Clear();
        //    foreach (string item in bindingList)
        //    {
        //        Items.Add(item);
        //    }
        //}

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();
            StringCollection filteredItems = new StringCollection();

            //foreach (string item in items)
            //{
            //    if (item.ToLower().Contains(searchText))
            //    {
            //        filteredItems.Add(item);
            //    }
            //}

            ComboBox.BeginUpdate();
            ComboBox.Items.Clear();
            foreach (string item in filteredItems)
            {
                ComboBox.Items.Add(item);
            }
            ComboBox.EndUpdate();
        }

        private void ComboBoxWithSearchFilter_Load(object sender, EventArgs e)
        {

        }
    }
    public class MyStringCollectionEditor : CollectionEditor
    {
        public MyStringCollectionEditor() : base(type: typeof(List<String>)) { }
        protected override object CreateInstance(Type itemType)
        {
            return string.Empty;
        }
    }
    public class StringCollectionEditor : System.ComponentModel.Design.CollectionEditor
    {
        public StringCollectionEditor(Type type) : base(type)
        {
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(string);
        }

        protected override object CreateInstance(Type itemType)
        {
            return base.CreateInstance(typeof(string));
        }

        protected override string GetDisplayText(object value)
        {
            return value.ToString();
        }
    }
}
