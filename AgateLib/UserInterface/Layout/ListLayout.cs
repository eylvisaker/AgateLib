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
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Layout
{
    public interface IListLayout : IWidgetLayout, IList<IRenderWidget>
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

        public ListLayout() {
        }
        public event WidgetEventHandler WidgetAdded;

        public event EventHandler FocusChanged;

        protected List<IRenderElement> items { get; private set; } = new List<IRenderElement>();

        public int Count => items.Count;

        public IRenderElement Focus
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

        bool ICollection<IRenderWidget>.IsReadOnly => false;

        IEnumerable<IRenderElement> IWidgetLayout.Items => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public WidgetDisplay Display => throw new NotImplementedException();

        public string StyleTypeIdentifier => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public bool CanHaveFocus => throw new NotImplementedException();

        public IEnumerable<IRenderElement> Children => throw new NotImplementedException();

        IRenderElement IRenderElement.Focus { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IRenderElementStyle Style => throw new NotImplementedException();

        public string StyleClass => throw new NotImplementedException();

        public string StyleId => throw new NotImplementedException();

        IRenderWidget IList<IRenderWidget>.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRenderElement this[int index]
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

        public void Add(IRenderWidget widget)
        {
            if (widget == null)
                throw new ArgumentNullException(nameof(widget));

            items.Add(widget);

            if (FocusIndex == -1)
                FocusIndex = items.IndexOf(widget);

            layoutDirty = true;

            OnWidgetAdded(widget);
        }

        public bool Remove(IRenderWidget widget)
        {
            var result = items.Remove(widget);

            layoutDirty |= result;

            return result;
        }

        public virtual void Initialize()
        { }

        public abstract Size ComputeIdealSize(Size maxSize, IWidgetRenderContext renderContext);

        public abstract void InputEvent(WidgetEventArgs input);

        public abstract void ApplyLayout(Size size, IWidgetRenderContext renderContext);

        public int IndexOf(IRenderWidget item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, IRenderWidget widget)
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

        public bool Contains(IRenderWidget item)
        {
            return items.Contains(item);
        }

        void ICollection<IRenderWidget>.CopyTo(IRenderWidget[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IRenderWidget> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected virtual void OnWidgetAdded(IRenderWidget widget)
        {
            WidgetAdded?.Invoke(widget);
        }

        public void SetChildren(IEnumerable<IRenderElement> children)
        {
            items.Clear();
            items.AddRange(children);
        }

        public void Add(IRenderElement item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(IRenderElement item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(IRenderElement[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(IRenderElement item)
        {
            throw new NotImplementedException();
        }

        public void Draw(IWidgetRenderContext renderContext, Rectangle clientArea)
        {
            throw new NotImplementedException();
        }

        public void Update(IWidgetRenderContext renderContext)
        {
            throw new NotImplementedException();
        }

        public Size ComputeIdealSize(IWidgetRenderContext renderContext, Size maxSize)
        {
            throw new NotImplementedException();
        }

        public void ProcessEvent(WidgetEventArgs widgetEventArgs)
        {
            throw new NotImplementedException();
        }

        public IRenderElement Render()
        {
            throw new NotImplementedException();
        }
    }
}
