using System;
using System.Collections.Specialized;

namespace Fa.views.controls.ComboBoxSearchable
{
    public class ObservableStringCollection : StringCollection
    {
        public event EventHandler CollectionChanged;

        public ObservableStringCollection()
        {
        }

        public ObservableStringCollection(string[] value)
        {
            AddRange(value);
        }

        public new void Add(string value)
        {
            base.Add(value);
            OnCollectionChanged();
        }

        public new void AddRange(string[] value)
        {
            base.AddRange(value);
            OnCollectionChanged();
        }

        public new void Clear()
        {
            base.Clear();
            OnCollectionChanged();
        }

        public new void Insert(int index, string value)
        {
            base.Insert(index, value);
            OnCollectionChanged();
        }

        public new void Remove(string value)
        {
            base.Remove(value);
            OnCollectionChanged();
        }

        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            OnCollectionChanged();
        }

        protected virtual void OnCollectionChanged()
        {
            CollectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
