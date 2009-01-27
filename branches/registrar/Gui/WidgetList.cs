using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Gui
{
    public sealed class WidgetList : IList<Widget>
    {
        List<Widget> mChildren = new List<Widget>();
        Container mOwner;

        public WidgetList(Container owner)
        {
            mOwner = owner;
        }
        private void AddChild(Widget child)
        {
            if (child.Parent != null)
                throw new AgateGuiException("The passed widget already has a parent.");

            child.Parent = mOwner;
            mChildren.Add(child);

            OnListUpdated();
        }
        private void RemoveAllChildren()
        {
            if (mChildren.Count == 0)
                return;

            foreach (Widget child in mChildren)
                child.Parent = null;

            mChildren.Clear();

            OnListUpdated();
        }
        private void RemoveChild(Widget child)
        {
            if (mChildren.Remove(child))
            {
                child.Parent = null;
                OnListUpdated();
            }
        }

        void OnListUpdated()
        {
            if (ListUpdated != null)
                ListUpdated(this, EventArgs.Empty);
        }
        public event EventHandler ListUpdated;

        public IEnumerable<Widget> VisibleItems
        {
            get
            {
                return mChildren.Where(x => x.Visible);
            }
        }

        #region IList<Widget> Members

        public int IndexOf(Widget item)
        {
            return mChildren.IndexOf(item);
        }

        public void Insert(int index, Widget item)
        {
            if (item == null) 
                throw new ArgumentNullException("item");
            if (item.Parent != null)
                throw new AgateGuiException("The passed widget already has a parent.");

            item.Parent = mOwner;
            mChildren.Insert(index, item);

            OnListUpdated();
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= mChildren.Count)
                throw new IndexOutOfRangeException();

            mChildren[index].Parent = null;
            mChildren.RemoveAt(index);

            OnListUpdated();
        }

        public Widget this[int index]
        {
            get
            {
                return mChildren[index];
            }
            set
            {
                if (value == null) 
                    throw new ArgumentNullException();
                if (index < 0 || index >= mChildren.Count) 
                    throw new IndexOutOfRangeException();
                
                if (value == mChildren[index]) 
                    return;

                value.Parent = mOwner;
                mChildren[index].Parent = null;
                mChildren[index] = value;
                
                OnListUpdated();
            }
        }

        #endregion
        #region ICollection<Widget> Members

        public void Add(Widget item)
        {
            AddChild(item);
        }

        public void Clear()
        {
            RemoveAllChildren();
        }

        public bool Contains(Widget item)
        {
            return mChildren.Contains(item);
        }

        void ICollection<Widget>.CopyTo(Widget[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return mChildren.Count; }
        }

        bool ICollection<Widget>.IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Widget item)
        {
            if (mChildren.Contains(item))
            {
                RemoveChild(item);
                return true;
            }
            else
                return false;
        }

        #endregion
        #region IEnumerable<Widget> Members

        public IEnumerator<Widget> GetEnumerator()
        {
            return mChildren.GetEnumerator();
        }

        #endregion
        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
