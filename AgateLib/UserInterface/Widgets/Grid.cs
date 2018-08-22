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

using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Layout;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.UserInterface
{
    /// <summary>
    /// Class which contains a grid layout of items. Each item exists in a single grid cell.
    /// Rows and columns scale to the size of the largest item contained within them.
    /// </summary>
    public class Grid : RenderElement<GridProps>
    {
        private Size[] sizeGrid;

        private int[] columnWidths;
        private int[] rowHeights;

        private Point focusPoint = new Point(-1, -1);

        private UserInterfaceActionEventArgs navigateEvent = new UserInterfaceActionEventArgs();

        public Grid(GridProps props) : base(props)
        {
            Children = Finalize(props.Children).ToList();

            sizeGrid = new Size[Columns * Rows];

            columnWidths = new int[Columns];
            rowHeights = new int[Rows];

            focusPoint = LocationOf(Children.FirstOrDefault(x => x.CanHaveFocus));
        }

        public Point LocationOf(IRenderElement item)
        {
            if (item == null)
                return new Point(-1, -1);

            var index = Children.IndexOf(item);

            if (index == -1)
                return new Point(-1, -1);

            return new Point(index % Columns, index / Columns);
        }

        private IRenderElement ChildAt(int x, int y)
        {
            var index = y * Columns + x;

            if (index >= Children.Count)
                return null;

            return Children[index];
        }

        private Size SizeAt(int x, int y) => sizeGrid[y * Columns + x];
        private void SetSizeAt(int x, int y, Size value) => sizeGrid[y * Columns + x] = value;

        public override Size CalcIdealContentSize(IUserInterfaceRenderContext renderContext, Size maxSize)
        {
            Array.Clear(columnWidths, 0, columnWidths.Length);
            Array.Clear(rowHeights, 0, rowHeights.Length);

            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    var item = ChildAt(x, y);

                    SetSizeAt(x, y, Size.Empty);

                    if (item == null)
                        continue;

                    var itemBox = item.Display.Region.MarginToContentOffset;

                    var itemMaxSize = LayoutMath.ItemContentMaxSize(itemBox, maxSize);

                    item.Display.Region.IdealContentSize
                        = item.CalcIdealContentSize(renderContext, maxSize);

                    var itemIdealSize = item.Display.Region.CalcConstrainedContentSize(maxSize);

                    SetSizeAt(x, y, itemIdealSize);

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

        public override void DoLayout(IUserInterfaceRenderContext renderContext, Size size)
        {
            int desty = 0;

            for (int y = 0; y < Rows; y++)
            {
                int destx = 0;

                for (int x = 0; x < Columns; x++)
                {
                    var item = ChildAt(x, y);

                    if (item == null)
                    {
                        destx += columnWidths[x];
                        continue;
                    }

                    item.Display.ContentRect = new Rectangle(
                        destx + item.Display.Region.MarginToContentOffset.Left,
                        desty + item.Display.Region.MarginToContentOffset.Top,
                        SizeAt(x, y).Width,
                        SizeAt(x, y).Height);

                    destx += columnWidths[x];
                }

                desty += rowHeights[y];
            }

            Width = columnWidths.Sum();
            Height = rowHeights.Sum();
        }

        public override void Draw(IUserInterfaceRenderContext renderContext, Rectangle clientArea)
        {
            renderContext.DrawChildren(clientArea, Children);
        }

        public Point FocusPoint
        {
            get => focusPoint;
            private set
            {
                focusPoint = value;
            }
        }

        public IRenderElement this[int x, int y]
        {
            get { return ChildAt(x, y); }
        }

        public IRenderElement this[Point pt]
        {
            get => this[pt.X, pt.Y];
        }

        public int Columns => Props.Columns;

        public int Rows
        {
            get
            {
                int fullRows = Children.Count / Columns;
                int extraKids = Children.Count % Columns;

                return fullRows + (extraKids > 0 ? 1 : 0);
            }
        }

        /// <summary>
        /// Gets the total width in pixels of the layout.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the total height in pixels of the layout.
        /// </summary>
        public int Height { get; private set; }

        public GridNavigationWrap NavigationWrap => Props.GridNavigationWrap;

        public override string StyleTypeId => FirstNotNullOrWhitespace(Props.StyleTypeId, "grid");

        public override bool CanHaveFocus => base.CanHaveFocus;

        public override void OnChildAction(IRenderElement child, UserInterfaceActionEventArgs args)
        {
            bool moved = false;
            switch (args.Action)
            {
                case UserInterfaceAction.Right:
                    moved = MoveRight();
                    break;

                case UserInterfaceAction.Left:
                    moved = MoveLeft();
                    break;

                case UserInterfaceAction.Up:
                    moved = MoveUp();
                    break;

                case UserInterfaceAction.Down:
                    moved = MoveDown();
                    break;

                default:
                    if (Props.AllowNavigate)
                    {
                        base.OnChildAction(child, args);
                        return;
                    }

                    break;
            }

            if (moved)
                Display.System.PlaySound(this, UserInterfaceSound.Navigate);
        }

        public bool MoveDown()
        {
            Point newFocus = this.focusPoint;

            do
            {
                newFocus.Y++;

                if (newFocus.Y >= Rows)
                {
                    if (NavigationWrap == GridNavigationWrap.None)
                    {
                        if (Props.AllowNavigate)
                            Parent?.OnChildAction(this, navigateEvent.Reset(UserInterfaceAction.Down));

                        return false;
                    }

                    newFocus.Y = 0;

                    if (NavigationWrap == GridNavigationWrap.Sparse)
                    {
                        newFocus.X++;
                        if (newFocus.X >= Columns)
                            newFocus.X = 0;
                    }
                }

            } while (this[newFocus] == null || !this[newFocus].CanHaveFocus);

            bool result = newFocus != this.focusPoint;

            SetFocus(newFocus);

            return result;
        }

        public bool MoveUp()
        {
            Point newFocus = this.focusPoint;

            do
            {
                newFocus.Y--;

                if (newFocus.Y < 0)
                {
                    newFocus.Y = Rows - 1;

                    if (NavigationWrap == GridNavigationWrap.None)
                    {
                        if (Props.AllowNavigate)
                            Parent?.OnChildAction(this, navigateEvent.Reset(UserInterfaceAction.Up));
                        return false;
                    }
                    if (NavigationWrap == GridNavigationWrap.Sparse)
                    {
                        newFocus.X--;
                        if (newFocus.X < 0)
                            newFocus.X = Columns - 1;
                    }
                }
            } while (this[newFocus] == null || !this[newFocus].CanHaveFocus);

            bool result = newFocus != this.focusPoint;

            SetFocus(newFocus);

            return result;
        }

        public bool MoveLeft()
        {
            Point newFocus = this.focusPoint;

            do
            {
                newFocus.X--;

                if (newFocus.X < 0)
                {
                    newFocus.X = Columns - 1;

                    if (NavigationWrap == GridNavigationWrap.None)
                    {
                        if (Props.AllowNavigate)
                            Parent?.OnChildAction(this, navigateEvent.Reset(UserInterfaceAction.Left));

                        return false;
                    }

                    if (NavigationWrap == GridNavigationWrap.Sparse)
                    {
                        newFocus.Y--;

                        if (newFocus.Y < 0)
                            newFocus.Y = Rows - 1;
                    }
                }

            } while (this[newFocus] == null || !this[newFocus].CanHaveFocus);

            bool result = newFocus != this.focusPoint;

            SetFocus(newFocus);

            return result;
        }

        public bool MoveRight()
        {
            Point newFocus = this.focusPoint;

            do
            {
                newFocus.X++;

                if (newFocus.X >= Columns)
                {
                    if (NavigationWrap == GridNavigationWrap.None)
                    {
                        if (Props.AllowNavigate)
                            Parent?.OnChildAction(this, navigateEvent.Reset(UserInterfaceAction.Right));

                        return false;
                    }

                    newFocus.X = 0;

                    if (NavigationWrap == GridNavigationWrap.Sparse)
                    {
                        newFocus.Y++;

                        if (newFocus.Y >= Rows)
                            newFocus.Y = 0;
                    }
                }

            } while (this[newFocus] == null || !this[newFocus].CanHaveFocus);

            bool result = newFocus != this.focusPoint;

            SetFocus(newFocus);

            return result;
        }

        private void SetFocus(Point newFocus)
        {
            if (focusPoint == newFocus)
                return;

            focusPoint = newFocus;
            SetGlobalFocus();
        }

        private void SetGlobalFocus()
        {
            Display.System.SetFocus(this[focusPoint]);
        }
    }

    /// <summary>
    /// Props class for a Grid object.
    /// 
    /// </summary>
    public class GridProps : RenderElementProps
    {
        private IList<IRenderable> children = new List<IRenderable>();

        public int Columns { get; set; }

        public string StyleTypeId { get; set; }

        public GridNavigationWrap GridNavigationWrap { get; set; }

        public IList<IRenderable> Children
        {
            get => children;
            set => children = value ?? throw new ArgumentNullException(nameof(Children));
        }

        public bool AllowNavigate { get; set; } = true;

        /// <summary>
        /// Puts an item at the specified location in the grid.
        /// Works as a fluent interface.
        /// </summary>
        /// <param name="child"></param>
        /// 
        /// 
        /// <returns></returns>
        public GridProps Add(IRenderable child)
        {
            children.Add(child);
            return this;
        }
    }

    /// <summary>
    /// Enum which describes how the SimpleGridLayout navigation is performed.
    /// </summary>
    public enum GridNavigationWrap
    {
        /// <summary>
        /// Wrapping does not occur at the boundaries, instead navigation events 
        /// at the edge of the grid will be propagated to the grid's parent to handle.
        /// </summary>
        None,

        /// <summary>
        /// Performs navigation for a sparsely populated grid. This causes
        /// wrap-arounds on the right and bottom to advance to the next row and column.
        /// Left and top navigation behaves oppositely.
        /// This prevents automatic navigation out of the edges of the grid.
        /// </summary>
        Sparse,

        /// <summary>
        /// Performs navigation for a densely populated grid. This
        /// causes wrap-arounds on the right and bottom to stay in the same
        /// row and column. Left and top navigation behaves the same.
        /// This prevents automatic navigation out of the edges of the grid.
        /// </summary>
        Dense,
    }
}
