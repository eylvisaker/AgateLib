using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.Sprites
{
    class FrameList<T> : IList<T> , IFrameList  where T : ISpriteFrame 
    {
        List<T> mFrames = new List<T>();

        #region IList<T> Members

        public int IndexOf(T item)
        {
            return mFrames.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            mFrames.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            mFrames.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return mFrames[index];
            }
            set
            {
                mFrames[index] = value;
            }
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            mFrames.Add(item);
        }

        public void Clear()
        {
            mFrames.Clear();
        }

        public bool Contains(T item)
        {
            return mFrames.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            mFrames.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return mFrames.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return mFrames.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return mFrames.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion


        #region IFrameList Members

        ISpriteFrame IFrameList.this[int index]
        {
            get { return mFrames[index]; }
        }

        int IFrameList.IndexOf(ISpriteFrame item)
        {
            if (item is T)
            {
                return mFrames.IndexOf((T)item);
            }
            else
                return -1;
        }

        bool IFrameList.Contains(ISpriteFrame item)
        {
            if (item is T)
                return mFrames.Contains((T)item);
            else
                return false;
        }

        void IFrameList.CopyTo(ISpriteFrame[] array, int arrayIndex)
        {
            Array.Copy(mFrames.ToArray(), 0, array, arrayIndex, mFrames.Count); 
        }

        #endregion
    }
}
