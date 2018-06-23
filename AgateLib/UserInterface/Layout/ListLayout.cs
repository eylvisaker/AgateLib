//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AgateLib.Mathematics.Geometry;
using AgateLib.Quality;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Layout
{
    public interface IListLayout : IWidgetLayout, IList<IWidget>
    {
        event EventHandler FocusChanged;

        /// <summary>
        /// Index of the widget that has focus.
        /// </summary>
        int FocusIndex { get; set; }
    }

    public abstract class ListLayout : IListLayout
    {
        private int focusIndex = -1;
        private bool layoutDirty;

        public event WidgetEventHandler WidgetAdded;

        public event EventHandler FocusChanged;

        protected List<IWidget> items { get; private set; } = new List<IWidget>();

        public int Count => items.Count;

        public IWidget Focus
        {
            get
            {
                if (FocusIndex < 0 || FocusIndex >= items.Count)
                    return null;

                return items[FocusIndex];
            }
            set
            {
                Require.That(value == null || items.Contains(value), "Focus must be contained in the layout.");

                FocusIndex = items.IndexOf(value);
            }
        }

        public int FocusIndex
        {
            get => focusIndex;
            set
            {
                if (focusIndex == value)
                {
                    return;
                }

                if (Focus != null)
                    Focus.Display.HasFocus = false;

                focusIndex = value;

                if (Focus != null)
                    Focus.Display.HasFocus = true;

                FocusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public WidgetRegion FocusLayout => Focus?.Display.Region;

        public IEnumerable<IWidget> Items => items;
        
        bool ICollection<IWidget>.IsReadOnly => false;

        public IWidget this[int index]
        {
            get => items[index];
            set => items[index] = value 
                    ?? throw new ArgumentNullException("Cannot set null widget.");
        }

        public void Clear()
        {
            items.Clear();
            layoutDirty = true;
        }

        public void Add(IWidget widget)
        {
            if (widget == null)
                throw new ArgumentNullException(nameof(widget));

            items.Add(widget);

            if (FocusIndex == -1)
                FocusIndex = items.IndexOf(widget);

            layoutDirty = true;

            OnWidgetAdded(widget);
        }

        public bool Remove(IWidget widget)
        {
            var result = items.Remove(widget);

            layoutDirty |= result;

            return result;
        }

        public virtual void Initialize()
        {
            foreach (var item in Items)
                item.Initialize();
        }

        public abstract Size ComputeIdealSize(Size maxSize, IWidgetRenderContext renderContext);

        public abstract void InputEvent(WidgetEventArgs input);

        public abstract void ApplyLayout(Size size, IWidgetRenderContext renderContext);

        public int IndexOf(IWidget item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, IWidget widget)
        {
            if (widget == null)
                throw new ArgumentNullException(nameof(widget));

            items.Insert(index, widget);

            WidgetAdded?.Invoke(widget);
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }

        public bool Contains(IWidget item)
        {
            return items.Contains(item);
        }

        void ICollection<IWidget>.CopyTo(IWidget[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IWidget> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected virtual void OnWidgetAdded(IWidget widget)
        {
            WidgetAdded?.Invoke(widget);
        }
    }
}
