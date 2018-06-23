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
using System.Linq;
using System.Text;
using AgateLib.Mathematics.Geometry;
using AgateLib.Quality;
using AgateLib.UserInterface.Widgets;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Layout
{
    /// <summary>
    /// Class which contains a grid layout of items, where the grid cells are a fixed size and do not
    /// expand for their contents. Item can be added to cover multiple cells of the grid.
    /// Does not handle navigation.
    /// </summary>
    public class FixedGridLayout : IWidgetLayout
    {
        class GridItem
        {
            public GridItem(IWidget item, Rectangle area)
            {
                Widget = item;
                Area = area;
            }

            public Rectangle Area { get; set; }

            public IWidget Widget { get; set; }
        }

        private List<GridItem> items = new List<GridItem>();
        private GridItem focus;
        private int rows;
        private int columns;
        private SizeMetrics sizeMetrics;

        public FixedGridLayout(int columns, int rows)
        {
            Rows = rows;
            Columns = columns;
        }

        public event WidgetEventHandler WidgetAdded;

        public IEnumerable<IWidget> Items => items.Select(gi => gi.Widget);

        public IWidget Focus
        {
            get => focus?.Widget;
            set
            {
                if (value == Focus)
                    return;

                if (Focus != null)
                {
                    Focus.Display.HasFocus = false;
                }

                if (value == null)
                {
                    focus = null;
                }
                else
                {
                    focus = items.First(gi => gi.Widget == value);
                    Focus.Display.HasFocus = true;
                }
            }
        }

        public WidgetRegion FocusLayout => Focus?.Display.Region;

        public int Count => items.Count;

        public bool IsReadOnly => false;

        public int Rows
        {
            get => rows;
            set
            {
                Require.ArgumentInRange(value > 0, nameof(Rows), "Rows must be positive");
                rows = value;
            }
        }

        public int Columns
        {
            get => columns;
            set
            {
                Require.ArgumentInRange(value > 0, nameof(Columns), "Columns must be positive");
                columns = value;
            }
        }

        void ICollection<IWidget>.Add(IWidget widget)
        {
            EnsureDoesNotContain(widget);

            for (int j = 0; j < Rows; j++)
            {
                for (int i = 0; i < Columns; i++)
                {
                    if (IsOccupied(i, j))
                        continue;

                    Add(widget, new Rectangle(i, j, 1, 1));
                    return;
                }
            }
        }

        public void Add(IWidget widget, Point location, Size? size = null)
        {
            Add(widget, new Rectangle(location, size ?? new Size(1, 1)));
        }

        public void Add(IWidget widget, Rectangle area)
        {
            EnsureDoesNotContain(widget);

            items.Add(new GridItem(widget, area));

            Focus = widget;

            WidgetAdded?.Invoke(widget);
        }

        /// <summary>
        /// Returns true if a widget occupies the specified space.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsOccupied(int x, int y)
        {
            return WidgetAt(x, y) != null;
        }

        /// <summary>
        /// Returns the first widget that occupies the specified space.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public IWidget WidgetAt(int x, int y)
        {
            foreach (var item in items)
            {
                if (item.Area.Contains(x, y))
                    return item.Widget;
            }

            return null;
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(IWidget item) => items.Any(x => x.Widget == item);

        void ICollection<IWidget>.CopyTo(IWidget[] array, int arrayIndex)
        {
            items.Select(x => x.Widget == items).ToArray().CopyTo(array, arrayIndex);
        }

        public IEnumerator<IWidget> GetEnumerator() => items.Select(x => x.Widget).GetEnumerator();

        public void InputEvent(WidgetEventArgs input)
        {
            Focus?.ProcessEvent(input);
        }

        public bool Remove(IWidget item)
        {
            return items.RemoveAll(x => x.Widget == item) > 0;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void ApplyLayout(Size size, IWidgetRenderContext renderContext)
        {
            int columnWidth = size.Width / Columns;
            int rowHeight = size.Height / Rows;

            foreach (var item in items)
            {
                var widget = item.Widget;

                int destx = ColumnStart(size, item.Area.X);
                int desty = RowStart(size, item.Area.Y);

                widget.Display.Region.Position = new Point(
                    destx + widget.Display.Region.MarginToContentOffset.Left,
                    desty + widget.Display.Region.MarginToContentOffset.Top);

                widget.Display.Region.ContentSize = new Size(
                    columnWidth * item.Area.Width - widget.Display.Region.MarginToContentOffset.Width,
                    rowHeight * item.Area.Height - widget.Display.Region.MarginToContentOffset.Height);
            }
        }

        public Size ComputeIdealSize(Size maxSize, IWidgetRenderContext renderContext)
        {
            int columnWidth = maxSize.Width / Columns;
            int rowHeight = maxSize.Height / Rows;

            foreach (var item in items)
            {
                var widget = item.Widget;

                Size itemMaxSize = new Size(item.Area.Width * columnWidth
                    - widget.Display.Region.MarginToContentOffset.Width,
                    item.Area.Height * rowHeight
                    - widget.Display.Region.MarginToContentOffset.Height);

                widget.RecalculateSize(renderContext, itemMaxSize);
            }

            return maxSize;
        }

        private int RowStart(Size size, int y)
        {
            return size.Height / Rows * y;
        }

        private int ColumnStart(Size size, int x)
        {
            return size.Width / Columns * x;
        }

        private void EnsureDoesNotContain(IWidget item)
        {
            if (Contains(item))
                throw new ArgumentException("Cannot add an item which already exists.");
        }

    }
}
