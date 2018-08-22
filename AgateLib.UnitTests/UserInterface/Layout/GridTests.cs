using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Microsoft.Xna.Framework;
using AgateLib.UserInterface;
using AgateLib.Tests;
using Moq;

namespace AgateLib.UserInterface.Layout
{
    public class GridTests
    {
        private Mock<RenderElement<RenderElementProps>> parent;
        private Grid grid;
        private GridNavigationWrap navigationWrap;

        [Fact]
        public void NavigateCheckerboardExitRight()
        {
            navigationWrap = GridNavigationWrap.None;

            SetLayout(
                "x x x ",
                " x x  ",
                "x x x ",
                " x x  ",
                "      ");

            UserInterfaceAction? parentNavigate = null;

            parent.Setup(x => x.OnChildAction(grid, It.IsAny<UserInterfaceActionEventArgs>()))
                .Callback<IRenderElement, UserInterfaceActionEventArgs>((_, args) =>
                {
                    parentNavigate = args.Action;
                });

            grid.FocusPoint.Should().Be(Point.Zero);

            grid.MoveRight();
            grid.FocusPoint.Should().Be(new Point(2, 0));

            grid.MoveRight();
            grid.FocusPoint.Should().Be(new Point(4, 0));
            parentNavigate.Should().BeNull();

            grid.MoveRight();
            grid.FocusPoint.Should().Be(new Point(4, 0));
            parentNavigate.Should().Be(UserInterfaceAction.Right);
        }

        [Fact]
        public void NavigateCheckerboardExitLeft()
        {
            navigationWrap = GridNavigationWrap.None;

            SetLayout(
                "x x x ",
                " x x  ",
                "x x x ",
                " x x  ",
                "      ");

            UserInterfaceAction? parentNavigate = null;

            parent.Setup(x => x.OnChildAction(grid, It.IsAny<UserInterfaceActionEventArgs>()))
                .Callback<IRenderElement, UserInterfaceActionEventArgs>((_, args) =>
                {
                    parentNavigate = args.Action;
                });

            grid.FocusPoint.Should().Be(Point.Zero);

            grid.MoveRight();
            grid.FocusPoint.Should().Be(new Point(2, 0));

            grid.MoveLeft();
            grid.FocusPoint.Should().Be(new Point(0, 0));
            parentNavigate.Should().BeNull();

            grid.MoveLeft();
            grid.FocusPoint.Should().Be(new Point(0, 0));
            parentNavigate.Should().Be(UserInterfaceAction.Left);
        }

        [Fact]
        public void NavigateCheckerboardExitUp()
        {
            navigationWrap = GridNavigationWrap.None;

            SetLayout(
                "x x x ",
                " x x  ",
                "x x x ",
                " x x  ",
                "      ");

            UserInterfaceAction? parentNavigate = null;

            parent.Setup(x => x.OnChildAction(grid, It.IsAny<UserInterfaceActionEventArgs>()))
                .Callback<IRenderElement, UserInterfaceActionEventArgs>((_, args) =>
                {
                    parentNavigate = args.Action;
                });

            grid.FocusPoint.Should().Be(Point.Zero);

            grid.MoveDown();
            grid.FocusPoint.Should().Be(new Point(0, 2));

            grid.MoveUp();
            grid.FocusPoint.Should().Be(new Point(0, 0));
            parentNavigate.Should().BeNull();

            grid.MoveUp();
            grid.FocusPoint.Should().Be(new Point(0, 0));
            parentNavigate.Should().Be(UserInterfaceAction.Up);
        }

        [Fact]
        public void NavigateCheckerboardExitDown()
        {
            navigationWrap = GridNavigationWrap.None;

            SetLayout(
                "x x x ",
                " x x  ",
                "x x x ",
                " x x  ",
                "      ");

            UserInterfaceAction? parentNavigate = null;

            parent.Setup(x => x.OnChildAction(grid, It.IsAny<UserInterfaceActionEventArgs>()))
                .Callback<IRenderElement, UserInterfaceActionEventArgs>((_, args) =>
                {
                    parentNavigate = args.Action;
                });

            grid.FocusPoint.Should().Be(Point.Zero);

            grid.MoveDown();
            grid.FocusPoint.Should().Be(new Point(0, 2));
            parentNavigate.Should().BeNull();

            grid.MoveDown();
            grid.FocusPoint.Should().Be(new Point(0, 2));
            parentNavigate.Should().Be(UserInterfaceAction.Down);
        }

        [Fact]
        public void SparseNavigateCheckerboard()
        {
            navigationWrap = GridNavigationWrap.Sparse;

            SetLayout(
                "x x x ",
                " x x  ",
                "x x x ",
                " x x  ",
                "      ");

            grid.FocusPoint.Should().Be(Point.Zero);

            grid.MoveRight();
            grid.FocusPoint.Should().Be(new Point(2, 0));

            grid.MoveDown();
            grid.FocusPoint.Should().Be(new Point(2, 2));

            grid.MoveRight();
            grid.FocusPoint.Should().Be(new Point(4, 2));

            grid.MoveUp();
            grid.FocusPoint.Should().Be(new Point(4, 0));

            grid.MoveRight();
            grid.FocusPoint.Should().Be(new Point(1, 1));
        }

        [Fact]
        public void SparseNavigateRight()
        {
            navigationWrap = GridNavigationWrap.Sparse;

            SetLayout(
                "x x   ",
                " x    ",
                "   xx ",
                " x x  ",
                "      ");

            grid.FocusPoint.Should().Be(Point.Zero);

            grid.MoveRight(); grid.FocusPoint.Should().Be(new Point(2, 0));
            grid.MoveRight(); grid.FocusPoint.Should().Be(new Point(1, 1));
            grid.MoveRight(); grid.FocusPoint.Should().Be(new Point(3, 2));
            grid.MoveRight(); grid.FocusPoint.Should().Be(new Point(4, 2));
            grid.MoveRight(); grid.FocusPoint.Should().Be(new Point(1, 3));
            grid.MoveRight(); grid.FocusPoint.Should().Be(new Point(3, 3));
            grid.MoveRight(); grid.FocusPoint.Should().Be(Point.Zero);
        }

        [Fact]
        public void SparseNavigateLeft()
        {
            navigationWrap = GridNavigationWrap.Sparse;

            // TODO: Use non-square grid in all of these!!!!
            SetLayout(
                "x x   ",
                " x    ",
                "   xx ",
                " x x  ",
                "      ");

            grid.FocusPoint.Should().Be(Point.Zero);
            grid.MoveLeft(); grid.FocusPoint.Should().Be(new Point(3, 3));
            grid.MoveLeft(); grid.FocusPoint.Should().Be(new Point(1, 3));
            grid.MoveLeft(); grid.FocusPoint.Should().Be(new Point(4, 2));
            grid.MoveLeft(); grid.FocusPoint.Should().Be(new Point(3, 2));
            grid.MoveLeft(); grid.FocusPoint.Should().Be(new Point(1, 1));
            grid.MoveLeft(); grid.FocusPoint.Should().Be(new Point(2, 0));
            grid.MoveLeft(); grid.FocusPoint.Should().Be(Point.Zero);
        }

        [Fact]
        public void SparseNavigateDown()
        {
            navigationWrap = GridNavigationWrap.Sparse;

            SetLayout(
                "x x   ",
                " x    ",
                "   xx ",
                " x x  ",
                "      ");

            grid.FocusPoint.Should().Be(Point.Zero);

            grid.MoveDown(); grid.FocusPoint.Should().Be(new Point(1, 1));
            grid.MoveDown(); grid.FocusPoint.Should().Be(new Point(1, 3));
            grid.MoveDown(); grid.FocusPoint.Should().Be(new Point(2, 0));
            grid.MoveDown(); grid.FocusPoint.Should().Be(new Point(3, 2));
            grid.MoveDown(); grid.FocusPoint.Should().Be(new Point(3, 3));
            grid.MoveDown(); grid.FocusPoint.Should().Be(new Point(4, 2));
            grid.MoveDown(); grid.FocusPoint.Should().Be(Point.Zero);
        }

        [Fact]
        public void SparseNavigateUp()
        {
            navigationWrap = GridNavigationWrap.Sparse;

            SetLayout(
                "x x   ",
                " x    ",
                "   xx ",
                " x x  ",
                "      ");

            grid.FocusPoint.Should().Be(Point.Zero);

            grid.MoveUp(); grid.FocusPoint.Should().Be(new Point(4, 2));
            grid.MoveUp(); grid.FocusPoint.Should().Be(new Point(3, 3));
            grid.MoveUp(); grid.FocusPoint.Should().Be(new Point(3, 2));
            grid.MoveUp(); grid.FocusPoint.Should().Be(new Point(2, 0));
            grid.MoveUp(); grid.FocusPoint.Should().Be(new Point(1, 3));
            grid.MoveUp(); grid.FocusPoint.Should().Be(new Point(1, 1));
            grid.MoveUp(); grid.FocusPoint.Should().Be(Point.Zero);
        }

        [Fact]
        public void DenseNavigate()
        {
            navigationWrap = GridNavigationWrap.Dense;

            SetLayout(
                "x xxx",
                " x xx",
                "x  xx",
                " x x ",
                " xxx ",
                "x   x");

            grid.FocusPoint.Should().Be(Point.Zero);

            grid.MoveRight(); grid.FocusPoint.Should().Be(new Point(2, 0));
            grid.MoveRight(); grid.FocusPoint.Should().Be(new Point(3, 0));
            grid.MoveRight(); grid.FocusPoint.Should().Be(new Point(4, 0));
            grid.MoveRight(); grid.FocusPoint.Should().Be(new Point(0, 0));
            grid.MoveLeft(); grid.FocusPoint.Should().Be(new Point(4, 0));

            grid.MoveDown(); grid.FocusPoint.Should().Be(new Point(4, 1));
            grid.MoveRight(); grid.FocusPoint.Should().Be(new Point(1, 1));
            grid.MoveRight(); grid.FocusPoint.Should().Be(new Point(3, 1));
            grid.MoveRight(); grid.FocusPoint.Should().Be(new Point(4, 1));

            grid.MoveDown(); grid.FocusPoint.Should().Be(new Point(4, 2));
            grid.MoveRight(); grid.FocusPoint.Should().Be(new Point(0, 2));
            grid.MoveRight(); grid.FocusPoint.Should().Be(new Point(3, 2));
            grid.MoveRight(); grid.FocusPoint.Should().Be(new Point(4, 2));

            grid.MoveDown(); grid.FocusPoint.Should().Be(new Point(4, 5));
            grid.MoveDown(); grid.FocusPoint.Should().Be(new Point(4, 0));
            grid.MoveUp(); grid.FocusPoint.Should().Be(new Point(4, 5));
            grid.MoveDown(); grid.FocusPoint.Should().Be(new Point(4, 0));
            grid.MoveLeft();
            grid.MoveDown();
            grid.MoveDown();
            grid.MoveDown(); grid.FocusPoint.Should().Be(new Point(3, 3));

            grid.MoveRight(); grid.FocusPoint.Should().Be(new Point(1, 3));
            grid.MoveRight(); grid.FocusPoint.Should().Be(new Point(3, 3));
            grid.MoveLeft(); grid.FocusPoint.Should().Be(new Point(1, 3));
            grid.MoveLeft(); grid.FocusPoint.Should().Be(new Point(3, 3));

            grid.MoveDown(); grid.FocusPoint.Should().Be(new Point(3, 4));
            grid.MoveRight(); grid.FocusPoint.Should().Be(new Point(1, 4));
            grid.MoveRight(); grid.FocusPoint.Should().Be(new Point(2, 4));
            grid.MoveRight(); grid.FocusPoint.Should().Be(new Point(3, 4));
        }

        private void SetLayout(params string[] layoutStrings)
        {
            foreach (var line in layoutStrings)
                line.Length.Should().Be(layoutStrings[0].Length,
                "All lines in the layout should be the same length");

            var gridProps = new GridProps()
            {
                GridNavigationWrap = navigationWrap,
                Columns = layoutStrings[0].Length
            };

            gridProps.GridNavigationWrap = navigationWrap;

            for (int y = 0; y < layoutStrings.Length; y++)
            {
                for (int x = 0; x < layoutStrings[y].Length; x++)
                {
                    bool canHaveFocus = true;

                    if (layoutStrings[y][x] == ' ')
                        canHaveFocus = false;    

                    (var widget, var element) = CommonMocks.Widget($"{x},{y}", elementCanHaveFocus: canHaveFocus);

                    gridProps.Add(widget.Object);
                }
            }

            (var parentWidget, var parent) = CommonMocks.Widget("parent", elementCanHaveFocus: true);
            this.parent = parent;

            grid = new Grid(gridProps);

            var displaySystem = new Mock<IDisplaySystem>();
            displaySystem.Setup(x => x.ParentOf(grid)).Returns(
                new Func<IRenderElement, IRenderElement>(_ => this.parent.Object));

            grid.Display.System = displaySystem.Object;


            parent.Object.Children.Add(grid);
        }
    }
}
