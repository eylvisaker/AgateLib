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
using AgateLib.Display;
using AgateLib.Mathematics.Geometry;
using AgateLib.Quality;
using AgateLib.UserInterface.Widgets;
using AgateLib.UserInterface.Rendering;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Layout
{
    public class AnchoredLayout : IWidgetLayout
    {
        List<IWidget> items = new List<IWidget>();

        int focusIndex = -1;

        public event WidgetEventHandler WidgetAdded;

        public IWidget Focus
        {
            get => focusIndex >= 0 ? items[focusIndex] : null;
            set
            {
                if (value == null)
                {
                    FocusIndex = -1;
                    return;
                }

                Require.That(items.Contains(value), "Focus must be an IWidget and it must be contained in the layout.");

                FocusIndex = items.IndexOf(value);
            }
        }

        public OriginAlignment Anchor { get; set; } = OriginAlignment.Center;

        public IEnumerable<IWidget> Items => items;

        public void Add(IWidget widget)
        {
            Require.ArgumentNotNull(widget, nameof(widget));

            items.Add(widget);

            ResetFocusWidget();

            WidgetAdded?.Invoke(widget);
        }

        public void Clear()
        {
            items.Clear();

            ResetFocusWidget();
        }

        public int FocusIndex
        {
            get => focusIndex;
            set
            {
                if (focusIndex < items.Count && focusIndex >= 0)
                {
                    focusIndex = value;
                }
                else
                {
                    focusIndex = -1;
                }

                ResetFocusWidget();
            }
        }

        public Size Size { get; set; }

        public int Count => items.Count;

        bool ICollection<IWidget>.IsReadOnly => false;
        
        public Size ComputeIdealSize(Size maxSize, IWidgetRenderContext renderContext)
        {
            foreach (var item in Items)
            {
                item.RecalculateSize(renderContext, maxSize);
            }

            return maxSize;
        }

        public void InputEvent(WidgetEventArgs input)
        {
            if (Focus != null)
            {
                if (Focus.Display.Animation.State != AnimationState.Static)
                    return;

                Focus.ProcessEvent(input);

                if (input.Handled)
                    return;
            }

            if (input.EventType == WidgetEventType.ButtonDown)
            {
                if (input.Button == MenuInputButton.PageDown)
                {
                    FocusIndex++;
                }
                else if (input.Button == MenuInputButton.PageUp)
                {
                    FocusIndex--;
                }
            }
        }

        public virtual void ApplyLayout(Size size, IWidgetRenderContext renderContext)
        {
            Size = size;

            foreach (var item in items)
            {
                item.Display.Region.ContentSize = item.Display.Region.Size.ComputeContentSize();

                var itemContentSize = item.Display.Region.ContentSize;

                var position = Origin.Calc(Anchor, size);
                var itemPosition = Origin.Calc(Anchor, itemContentSize);

                item.Display.Region.Position =
                    new Point(position.X - itemPosition.X, position.Y - itemPosition.Y);
            }
        }

        private void ResetFocusWidget()
        {
            if (items.Count == 0)
                focusIndex = -1;
            else if (focusIndex < 0)
                focusIndex = 0;

            var focusWidget = focusIndex >= 0 ? items[focusIndex] : null;

            foreach (var item in items)
                item.Display.HasFocus = item == focusWidget;
        }

        public bool Contains(IWidget item) => items.Contains(item);

        public void CopyTo(IWidget[] array, int arrayIndex)
            => items.CopyTo(array, arrayIndex);

        public bool Remove(IWidget item) => items.Remove(item);

        public IEnumerator<IWidget> GetEnumerator() => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
