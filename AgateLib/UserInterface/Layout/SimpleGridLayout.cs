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
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Widgets;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Layout
{
    /// <summary>
    /// Class which contains a grid layout of items. Each item exists in a single grid cell, and
    /// items can be added at any point on the grid. 
    /// Rows and columns scale to the size of the largest item contained within them.
    /// Allows navigation by the user.
    /// </summary>
    public class SimpleGridLayout : IWidgetLayout
    {
        private IWidget[,] grid;
        private Size[,] sizeGrid;

        private int[] columnWidths;
        private int[] rowHeights;

        private Point focusPoint = new Point(-1, -1);

        private WidgetEventArgs widgetEventArgs = new WidgetEventArgs();

        /// <summary>
        /// Event raised whenever a widget is added to the layout.
        /// </summary>
        public event WidgetEventHandler WidgetAdded;

        /// <summary>
        /// Event raised when the user navigates to another menu item.
        /// </summary>
        public event EventHandler FocusMoved;

        public Point FocusPoint
        {
            get => focusPoint;
            set
            {
                focusPoint = value;
                UpdateFocusProperties();
            }
        }

        public IWidget Focus
        {
            get
            {
                if (focusPoint.X < 0 || focusPoint.Y < 0)
                    return null;

                return this[focusPoint];
            }
            set
            {
                if (value == null)
                {
                    FocusPoint = new Point(-1, -1);
                    return;
                }

                for (int j = 0; j < Rows; j++)
                {
                    for (int i = 0; i < Columns; i++)
                    {
                        if (this[i, j] == value)
                        {
                            FocusPoint = new Point(i, j);
                            return;
                        }
                    }
                }

                throw new ArgumentException(
                    "Focus must be contained in the layout.");
            }
        }

        private void UpdateFocusProperties()
        {
            var oldFocus = Items.FirstOrDefault(x => x.Display.HasFocus);

            if (oldFocus == Focus)
                return;

            foreach (var item in Items)
            {
                item.Display.HasFocus = item == Focus;
            }

            oldFocus?.ProcessEvent(
                widgetEventArgs.Initialize(WidgetEventType.FocusLost));

            Focus?.ProcessEvent(
                widgetEventArgs.Initialize(WidgetEventType.FocusGained));

            FocusMoved?.Invoke(this, EventArgs.Empty);
        }

        public IWidget this[int x, int y]
        {
            get { return grid[x, y]; }
            set
            {
                grid[x, y] = value;

                if (value != null)
                {
                    WidgetAdded?.Invoke(value);

                    if (FocusPoint.X < 0)
                    {
                        FocusPoint = new Point(x, y);
                    }
                }
            }
        }

        public IWidget this[Point pt]
        {
            get => this[pt.X, pt.Y];
            set => this[pt.X, pt.Y] = value;
        }

        public int Columns { get; private set; }

        public int Rows { get; private set; }

        /// <summary>
        /// Gets the total width in pixels of the layout.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the total height in pixels of the layout.
        /// </summary>
        public int Height { get; private set; }

        public IEnumerable<IWidget> Items
        {
            get
            {
                for (int y = 0; y < Rows; y++)
                {
                    for (int x = 0; x < Columns; x++)
                    {
                        var value = this[x, y];

                        if (value != null)
                            yield return value;
                    }
                }
            }
        }

        public int Count => Items.Count();

        bool ICollection<IWidget>.IsReadOnly => false;

        public NavigationBehavior NavigationBehavior { get; set; }

        public NavigationBehavior ActualNavigationBehavior
        {
            get
            {
                if (NavigationBehavior != NavigationBehavior.Auto)
                    return NavigationBehavior;

                var minCount = (Rows * Columns) / 2 + 1;

                if (Count < minCount) return NavigationBehavior.Sparse;
                if (AnyRowBlank) return NavigationBehavior.Sparse;
                if (AnyColumnBlank) return NavigationBehavior.Sparse;

                return NavigationBehavior.Dense;
            }
        }

        bool AnyRowBlank
        {
            get
            {
                for (int y = 0; y < Rows; y++)
                {
                    bool rowBlank = true;

                    for (int x = 0; x < Columns; x++)
                    {
                        if (this[x, y] != null)
                        {
                            rowBlank = false;
                            break;
                        }
                    }

                    if (rowBlank)
                        return true;
                }

                return false;
            }
        }

        bool AnyColumnBlank
        {
            get
            {
                for (int x = 0; x < Columns; x++)
                {
                    bool colBlank = true;

                    for (int y = 0; y < Rows; y++)
                    {
                        if (this[x, y] != null)
                        {
                            colBlank = false;
                            break;
                        }
                    }

                    if (colBlank)
                        return true;
                }

                return false;
            }
        }
        
        /// <summary>
        /// Resizes the grid.
        /// </summary>
        /// <param name="columns">The number of columns.</param>
        /// <param name="rows">The number of rows.</param>
        public void ResizeGrid(int columns, int rows)
        {
            Columns = columns;
            Rows = rows;

            grid = new IWidget[columns, rows];
            sizeGrid = new Size[columns, rows];

            columnWidths = new int[columns];
            rowHeights = new int[rows];
        }

        public void ApplyLayout(Size size, IWidgetRenderContext renderContext)
        {
            int desty = 0;

            for (int y = 0; y < Rows; y++)
            {
                int destx = 0;

                for (int x = 0; x < Columns; x++)
                {
                    var item = grid[x, y];

                    if (item == null)
                    {
                        destx += columnWidths[x];
                        continue;
                    }

                    item.Display.Region.Position = new Point(
                        destx + item.Display.Region.MarginToContentOffset.Left,
                        desty + item.Display.Region.MarginToContentOffset.Top);

                    item.Display.Region.ContentSize = sizeGrid[x, y];

                    destx += columnWidths[x];
                }

                desty += rowHeights[y];
            }

            Width = columnWidths.Sum();
            Height = rowHeights.Sum();
        }

        public Size ComputeIdealSize(Size maxSize, IWidgetRenderContext renderContext)
        {
            Array.Clear(columnWidths, 0, columnWidths.Length);
            Array.Clear(rowHeights, 0, rowHeights.Length);

            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    var item = grid[x, y];

                    sizeGrid[x, y] = Size.Empty;

                    if (item == null)
                        continue;

                    var itemBox = item.Display.Region.MarginToContentOffset;

                    var itemMaxSize = LayoutMath.ItemContentMaxSize(itemBox, maxSize);

                    item.RecalculateSize(renderContext, maxSize);

                    var itemIdealSize = item.Display.Region.Size.IdealContentSize;

                    sizeGrid[x, y] = itemIdealSize;

                    columnWidths[x] = Math.Max(columnWidths[x], itemIdealSize.Width + itemBox.Width);
                    rowHeights[y] = Math.Max(rowHeights[y], itemIdealSize.Height + itemBox.Height);
                }
            }

            for (int i = 0; i < Columns; i++)
                columnWidths[i] = Math.Min(maxSize.Width, columnWidths[i]);

            for (int i = 0; i < Rows; i++)
                rowHeights[i] = Math.Min(maxSize.Height, rowHeights[i]);

            var idealWidth = columnWidths.Sum();
            var idealHeight = rowHeights.Sum();

            return new Size(idealWidth, idealHeight);
        }

        public void InputEvent(WidgetEventArgs input)
        {
            var currentItem = this[focusPoint];

            if (currentItem != null)
            {
                currentItem.ProcessEvent(input);

                if (input.Handled)
                    return;
            }

            if (input.EventType == WidgetEventType.ButtonDown)
            {
                bool moved = false;

                switch (input.Button)
                {
                    case MenuInputButton.Right:
                        MoveRight();
                        input.Handled = true;
                        break;

                    case MenuInputButton.Left:
                        MoveLeft();
                        input.Handled = true;
                        break;

                    case MenuInputButton.Up:
                        MoveUp();
                        input.Handled = true;
                        break;

                    case MenuInputButton.Down:
                        MoveDown();
                        input.Handled = true;
                        break;
                }
            }
            else if (input.EventType == WidgetEventType.ButtonUp)
            {
                switch (input.Button)
                {
                    case MenuInputButton.Right:
                    case MenuInputButton.Left:
                    case MenuInputButton.Up:
                    case MenuInputButton.Down:
                        input.Handled = true;
                        break;
                }
            }
        }
        
        public void MoveDown()
        {
            do
            {
                focusPoint.Y++;

                if (focusPoint.Y >= Rows)
                {
                    focusPoint.Y = 0;

                    if (ActualNavigationBehavior == NavigationBehavior.Sparse)
                    {
                        focusPoint.X++;
                        if (focusPoint.X >= Columns)
                            focusPoint.X = 0;
                    }
                }

            } while (this[focusPoint] == null);

            UpdateFocusProperties();
        }

        public void MoveUp()
        {
            do
            {
                focusPoint.Y--;

                if (focusPoint.Y < 0)
                {
                    focusPoint.Y = Rows - 1;

                    if (ActualNavigationBehavior == NavigationBehavior.Sparse)
                    {
                        focusPoint.X--;
                        if (focusPoint.X < 0)
                            focusPoint.X = Columns - 1;
                    }
                }
            } while (this[focusPoint] == null);

            UpdateFocusProperties();
        }

        public void MoveLeft()
        {
            do
            {
                focusPoint.X--;

                if (focusPoint.X < 0)
                {
                    focusPoint.X = Columns - 1;

                    if (ActualNavigationBehavior == NavigationBehavior.Sparse)
                    {
                        focusPoint.Y--;

                        if (focusPoint.Y < 0)
                            focusPoint.Y = Rows - 1;
                    }
                }
                
            } while (this[focusPoint] == null);

            UpdateFocusProperties();
        }

        public void MoveRight()
        {
            do
            {
                focusPoint.X++;

                if (focusPoint.X >= Columns)
                {
                    focusPoint.X = 0;

                    if (ActualNavigationBehavior == NavigationBehavior.Sparse)
                    {
                        focusPoint.Y++;

                        if (focusPoint.Y >= Rows)
                            focusPoint.Y = 0;
                    }
                }
                
            } while (this[focusPoint] == null);

            UpdateFocusProperties();
        }

        WidgetRegion emptyItemLayout = new WidgetRegion(new WidgetStyle());

        /// <summary>
        /// Gets the margin rectangle of the control located in the grid at the specified point.
        /// </summary>
        /// <param name="pt">The point of the control, in grid coordinates.</param>
        /// <returns></returns>
        public WidgetRegion LayoutOf(Point pt)
        {
            var widget = this[pt];

            if (widget == null)
            {
                int x = 0, y = 0;

                for (int i = 0; i < pt.X && i < Columns; i++)
                {
                    x += columnWidths[i];
                }
                for (int j = 0; j < pt.Y && j < Rows; j++)
                {
                    y += rowHeights[j];
                }

                emptyItemLayout.Position = new Point(x, y);
                emptyItemLayout.ContentSize = new Size(columnWidths[pt.X], rowHeights[pt.Y]);

                return emptyItemLayout;
            }
            else
            {
                return widget.Display.Region;
            }
        }

        public void Add(IWidget widget)
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    if (this[x, y] == null)
                    {
                        // this[x,y]= invokes the WidgetAdded event.
                        this[x, y] = widget;
                        return;
                    }
                }
            }

            throw new IndexOutOfRangeException("Not enough room in the grid to add more items.");
        }

        public void Clear()
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    this[x, y] = null;
                }
            }
        }

        public void Initialize()
        {
            foreach (var widget in Items)
                widget.Initialize();
        }

        public bool Contains(IWidget item) => Items.Contains(item);

        public void CopyTo(IWidget[] array, int arrayIndex)
            => Items.Where(x => x != null).ToArray().CopyTo(array, arrayIndex);

        public bool Remove(IWidget item)
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    if (this[x, y] == item)
                    {
                        this[x, y] = null;
                        return true;
                    }
                }
            }

            return false;
        }

        public IEnumerator<IWidget> GetEnumerator() => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Enum which describes how the SimpleGridLayout navigation is performed.
    /// </summary>
    public enum NavigationBehavior
    {
        /// <summary>
        /// Automatically determines whether to use sparse or dense navigation
        /// techniques depending on how much of the grid is filled.
        /// </summary>
        /// <remarks>
        /// Sparse navigation is chosen if:
        ///  * Less than N / 2 + 1 cells are filled
        ///  * Any row or column has zero or one items in it.
        ///  </remarks>
        Auto,
        
        /// <summary>
        /// Performs navigation for a sparsely populated grid. This causes
        /// wrap-arounds on the right and bottom to advance to the next row and column.
        /// Left and top navigation behaves oppositely.
        /// </summary>
        Sparse,

        /// <summary>
        /// Performs navigation for a densely populated grid. This
        /// causes wrap-arounds on the right and bottom to stay in the same
        /// row and column. Left and top navigation behaves the same.
        /// </summary>
        Dense,
    }
}
