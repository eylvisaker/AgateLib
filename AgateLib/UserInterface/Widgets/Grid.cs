using System;
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
        IRenderElement[,] grid;

        private Size[,] sizeGrid;

        private int[] columnWidths;
        private int[] rowHeights;

        private Point focusPoint = new Point(-1, -1);

        public Grid(GridProps props) : base(props)
        {
            sizeGrid = new Size[Columns, Rows];
            grid = new IRenderElement[Columns, Rows];
            columnWidths = new int[Columns];
            rowHeights = new int[Rows];

            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    grid[x, y] = Finalize(props[x, y]);

                    if (grid[x, y] != null && focusPoint.X == -1)
                        focusPoint = new Point(x, y);
                }
            }

            var cs = new List<IRenderElement>();
            cs.AddRange(grid.Cast<IRenderElement>().Where(x => x != null));
            Children = cs;
        }

        public override Size CalcIdealContentSize(IWidgetRenderContext renderContext, Size maxSize)
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

        public override void DoLayout(IWidgetRenderContext renderContext, Size size)
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

                    item.Display.ContentRect = new Rectangle(
                        destx + item.Display.Region.MarginToContentOffset.Left,
                        desty + item.Display.Region.MarginToContentOffset.Top,
                        sizeGrid[x, y].Width,
                        sizeGrid[x, y].Height);

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
            get { return grid[x, y]; }
        }

        public IRenderElement this[Point pt]
        {
            get => this[pt.X, pt.Y];
        }

        public int Columns => Props.Columns;

        public int Rows => Props.Rows;

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
        IRenderable[,] children = new IRenderable[0, 0];

        public int Rows => children.GetUpperBound(1) + 1;
        public int Columns => children.GetUpperBound(0) + 1;

        public string StyleTypeId { get; set; }

        public GridNavigationWrap GridNavigationWrap { get; set; }

        public IRenderable this[int x, int y]
        {
            get => children[x, y];
            private set => children[x, y] = value;
        }

        /// <summary>
        /// Resizes the grid props. Existing children are preserved if they are
        /// within the bounds of the new grid. Works as a fluent interface.
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public GridProps Resize(int columns, int rows)
        {
            var newc = new IRenderable[columns, rows];

            for (int y = 0; y < rows && y < Rows; y++)
            {
                for (int x = 0; x < columns && x < Columns; x++)
                {
                    newc[x, y] = children[x, y];
                }
            }

            children = newc;

            return this;
        }

        /// <summary>
        /// Puts an item at the specified location in the grid.
        /// Works as a fluent interface.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        public GridProps Put(int x, int y, IRenderable child)
        {
            this[x, y] = child;
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
