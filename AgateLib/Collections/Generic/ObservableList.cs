using System;
using System.Collections;
using System.Collections.Generic;

namespace AgateLib.Collections.Generic
{
    /// <summary>
    /// A list class which implements IList<typeparamref name="T"/> as well as IReadOnlyList<typeparamref name="T"/>.
    /// It also has a ListChanged event you can use to observe changes to the list. This may incur some performance
    /// cost when making many changes to the list. In this case, use PauseEvents to suspend events, and 
    /// then EmitListChanged once you're done changing the list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableList<T> : IList<T>, IReadOnlyList<T>
    {
        private List<T> items = new List<T>();

        public event EventHandler ListChanged;

        public bool PauseEvents { get; set; }

        public void Sort() => items.Sort();

        public void EmitListChanged()
        {
            if (PauseEvents)
                return;

            ListChanged?.Invoke(this, EventArgs.Empty);
        }

        public T this[int index]
        {
            get => ((IList<T>)items)[index];
            set
            {
                ((IList<T>)items)[index] = value;
                EmitListChanged();
            }
        }

        public int Count => ((IList<T>)items).Count;

        public bool IsReadOnly => ((IList<T>)items).IsReadOnly;

        public void Add(T item)
        {
            ((IList<T>)items).Add(item);
            EmitListChanged();
        }

        public void Clear()
        {
            ((IList<T>)items).Clear();
            EmitListChanged();
        }

        public bool Contains(T item)
        {
            return ((IList<T>)items).Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ((IList<T>)items).CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IList<T>)items).GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return ((IList<T>)items).IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            ((IList<T>)items).Insert(index, item);
            EmitListChanged();
        }

        public void AddRange(IEnumerable<T> enumerable)
        {
            items.AddRange(enumerable);
            EmitListChanged();
        }

        public bool Remove(T item)
        {
            bool result = ((IList<T>)items).Remove(item);
            EmitListChanged();
            return result;
        }

        public void RemoveAt(int index)
        {
            ((IList<T>)items).RemoveAt(index);
            EmitListChanged();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<T>)items).GetEnumerator();
        }
    }
}
