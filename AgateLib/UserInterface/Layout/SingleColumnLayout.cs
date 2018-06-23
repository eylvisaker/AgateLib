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
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Layout
{
    /// <summary>
    /// Lays items out in a single column.
    /// </summary>
    public class SingleColumnLayout : ListLayout
    {
        /// <summary>
        /// Gets or sets whether to wrap the cursor around the top/bottom of the menu.
        /// </summary>
        public bool TopBottomWrap { get; set; }

        public override void InputEvent(WidgetEventArgs input)
        {
            Focus?.ProcessEvent(input);

            if (input.Handled)
                return;

            if (input.EventType == WidgetEventType.ButtonDown)
            {
                int newFocus = FocusIndex;

                if (input.Button == MenuInputButton.Down)
                {
                    newFocus = FocusIndex + 1;

                    if (TopBottomWrap)
                        newFocus %= items.Count;
                }
                else if (input.Button == MenuInputButton.Up)
                {
                    newFocus = FocusIndex - 1;

                    if (TopBottomWrap)
                        newFocus = (newFocus + items.Count) % items.Count;
                }

                newFocus = Math.Min(newFocus, items.Count - 1);
                newFocus = Math.Max(newFocus, 0);

                input.Handled = newFocus != FocusIndex;

                FocusIndex = newFocus;
            }
        }
        
        public override Size ComputeIdealSize(Size maxSize, IWidgetRenderContext renderContext)
        {
            int idealWidth = 0;
            int idealHeight = 0;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (!item.Display.IsVisible)
                    continue;

                var itemBox = item.Display.Region.MarginToContentOffset;

                var itemMaxSize =
                    LayoutMath.ItemContentMaxSize(itemBox, maxSize);

                item.RecalculateSize(renderContext, maxSize);

                var itemIdealContentSize = item.Display.Region.Size.IdealContentSize;

                idealWidth = Math.Max(idealWidth, itemIdealContentSize.Width + itemBox.Width);

                idealHeight += itemIdealContentSize.Height + itemBox.Height;
            }

            return new Size(idealWidth, idealHeight);
        }

        public override void ApplyLayout(Size size, IWidgetRenderContext renderContext)
        {
            Point dest = new Point(0, 0);

            int itemWidth = size.Width;

            for (int i = 0; i < items.Count; i++)
            {
                if (!items[i].Display.IsVisible)
                    continue;

                var item = items[i];

                var itemDest = dest;
                var marginToContent = item.Display.Region.MarginToContentOffset;

                itemDest.X += marginToContent.Left;
                itemDest.Y += marginToContent.Top;

                item.Display.Region.Position = itemDest;
                var contentSize = item.Display.Region.Size.ComputeContentSize();

                contentSize.Width = itemWidth - item.Display.Region.MarginToContentOffset.Width;
                item.Display.Region.ContentSize = contentSize;

                dest.Y += item.Display.Region.MarginRect.Height;

                item.Display.HasFocus = item == Focus;
            }
        }

    }
}
