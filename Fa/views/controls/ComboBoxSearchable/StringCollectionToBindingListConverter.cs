using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Fa.views.controls.ComboBoxSearchable
{
    public class StringCollectionToBindingListConverter
    {
        public static BindingList<string> ConvertToStringBindingList(StringCollection stringCollection)
        {
            if (stringCollection == null)
                throw new ArgumentNullException(nameof(stringCollection));

            BindingList<string> bindingList = new BindingList<string>();
            foreach (string item in stringCollection)
            {
                bindingList.Add(item);
            }
            return bindingList;
        }

        public static StringCollection ConvertToSringCollection(BindingList<string> bindingList)
        {
            if (bindingList == null)
                throw new ArgumentNullException(nameof(bindingList));

            StringCollection stringCollection = new StringCollection();
            foreach (string item in bindingList)
            {
                stringCollection.Add(item);
            }
            return stringCollection;
        }
    }
}
