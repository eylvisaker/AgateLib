﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Layout;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Widgets
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

        public Grid(GridProps props) : base(props)
        {
            Children = Finalize(props.Children).ToList();

            sizeGrid = new Size[Children.Count];

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

        IRenderElement ChildAt(int x, int y) => Children[y * Columns + x];
        Size SizeAt(int x, int y) => sizeGrid[y * Columns + x];
        void SetSizeAt(int x, int y, Size value) => sizeGrid[y * Columns + x] = value;

        public override Size CalcIdealContentSize(IWidgetRenderContext renderContext, Size maxSize)
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

                    item.RecalculateSize(renderContext, maxSize);

                    var itemIdealSize = item.Display.Region.Size.IdealContentSize;

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

        public override void DoLayout(IWidgetRenderContext renderContext, Size size)
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

        public override void Draw(IWidgetRenderContext renderContext, Rectangle clientArea)
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

        public override void OnChildNavigate(IRenderElement child, MenuInputButton button)
        {
            switch (button)
            {
                case MenuInputButton.Right:
                    MoveRight();
                    break;

                case MenuInputButton.Left:
                    MoveLeft();
                    break;

                case MenuInputButton.Up:
                    MoveUp();
                    break;

                case MenuInputButton.Down:
                    MoveDown();
                    break;

                default:
                    base.OnChildNavigate(child, button);
                    break;
            }
        }

        public void MoveDown()
        {
            var newFocus = this.focusPoint;

            do
            {
                newFocus.Y++;

                if (newFocus.Y >= Rows)
                {
                    if (NavigationWrap == GridNavigationWrap.None)
                    {
                        Parent?.OnChildNavigate(this, MenuInputButton.Down);
                        return;
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

            SetFocus(newFocus);
        }

        public void MoveUp()
        {
            var newFocus = this.focusPoint;

            do
            {
                newFocus.Y--;

                if (newFocus.Y < 0)
                {
                    newFocus.Y = Rows - 1;

                    if (NavigationWrap == GridNavigationWrap.None)
                    {
                        Parent?.OnChildNavigate(this, MenuInputButton.Up);
                        return;
                    }
                    if (NavigationWrap == GridNavigationWrap.Sparse)
                    {
                        newFocus.X--;
                        if (newFocus.X < 0)
                            newFocus.X = Columns - 1;
                    }
                }
            } while (this[newFocus] == null || !this[newFocus].CanHaveFocus);

            SetFocus(newFocus);
        }

        public void MoveLeft()
        {
            var newFocus = this.focusPoint;

            do
            {
                newFocus.X--;

                if (newFocus.X < 0)
                {
                    newFocus.X = Columns - 1;

                    if (NavigationWrap == GridNavigationWrap.None)
                    {
                        Parent?.OnChildNavigate(this, MenuInputButton.Left);
                        return;
                    }

                    if (NavigationWrap == GridNavigationWrap.Sparse)
                    {
                        newFocus.Y--;

                        if (newFocus.Y < 0)
                            newFocus.Y = Rows - 1;
                    }
                }

            } while (this[newFocus] == null || !this[newFocus].CanHaveFocus);

            SetFocus(newFocus);
        }

        public void MoveRight()
        {
            var newFocus = this.focusPoint;

            do
            {
                newFocus.X++;

                if (newFocus.X >= Columns)
                {
                    if (NavigationWrap == GridNavigationWrap.None)
                    {
                        Parent?.OnChildNavigate(this, MenuInputButton.Right);
                        return;
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

            SetFocus(newFocus);
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
        IList<IRenderable> children = new List<IRenderable>();

        public int Columns { get; set; }

        public string StyleTypeId { get; set; }

        public GridNavigationWrap GridNavigationWrap { get; set; }

        public IList<IRenderable> Children
        {
            get => children;
            set => children = value ?? throw new ArgumentNullException(nameof(Children));
        }

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