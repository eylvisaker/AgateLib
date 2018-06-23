using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Layout
{
    public class SimpleGridLayoutTests : LayoutTests
    {
        private readonly SimpleGridLayout layout;

        public SimpleGridLayoutTests()
        {
            layout = new SimpleGridLayout();
            layout.ResizeGrid(5, 5);
        }

        protected override IWidgetLayout WidgetLayout => layout;

        [Fact]
        public void FirstAddedItemHasFocus()
        {
            layout.Focus.Should().BeNull();

            AddWidget(3, 2);

            layout.Focus.Should().Be(layout[3, 2]);
            layout.FocusPoint.Should().Be(new Point(3, 2));

            layout.Focus.Display.HasFocus.Should().BeTrue();
        }

        [Fact]
        public void SparseNavigateCheckerboard()
        {
            SetLayout(
                "x x x ",
                " x x  ",
                "x x x ",
                " x x  ",
                "      ");

            layout.FocusPoint.Should().Be(Point.Zero);

            layout.MoveRight();
            layout.FocusPoint.Should().Be(new Point(2, 0));

            layout.MoveDown();
            layout.FocusPoint.Should().Be(new Point(2, 2));

            layout.MoveRight();
            layout.FocusPoint.Should().Be(new Point(4, 2));

            layout.MoveUp();
            layout.FocusPoint.Should().Be(new Point(4, 0));

            layout.MoveRight();
            layout.FocusPoint.Should().Be(new Point(1, 1));
        }

        [Fact]
        public void SparseNavigateRight()
        {
            layout.NavigationBehavior = NavigationBehavior.Sparse;

            SetLayout(
                "x x   ",
                " x    ",
                "   xx ",
                " x x  ",
                "      ");

            layout.FocusPoint.Should().Be(Point.Zero);

            layout.MoveRight(); layout.FocusPoint.Should().Be(new Point(2, 0));
            layout.MoveRight(); layout.FocusPoint.Should().Be(new Point(1, 1));
            layout.MoveRight(); layout.FocusPoint.Should().Be(new Point(3, 2));
            layout.MoveRight(); layout.FocusPoint.Should().Be(new Point(4, 2));
            layout.MoveRight(); layout.FocusPoint.Should().Be(new Point(1, 3));
            layout.MoveRight(); layout.FocusPoint.Should().Be(new Point(3, 3));
            layout.MoveRight(); layout.FocusPoint.Should().Be(Point.Zero);
        }

        [Fact]
        public void SparseNavigateLeft()
        {
            layout.NavigationBehavior = NavigationBehavior.Sparse;

            // TODO: Use non-square grid in all of these!!!!
            SetLayout(
                "x x   ",
                " x    ",
                "   xx ",
                " x x  ",
                "      ");

            layout.FocusPoint.Should().Be(Point.Zero);
            layout.MoveLeft(); layout.FocusPoint.Should().Be(new Point(3, 3));
            layout.MoveLeft(); layout.FocusPoint.Should().Be(new Point(1, 3));
            layout.MoveLeft(); layout.FocusPoint.Should().Be(new Point(4, 2));
            layout.MoveLeft(); layout.FocusPoint.Should().Be(new Point(3, 2));
            layout.MoveLeft(); layout.FocusPoint.Should().Be(new Point(1, 1));
            layout.MoveLeft(); layout.FocusPoint.Should().Be(new Point(2, 0));
            layout.MoveLeft(); layout.FocusPoint.Should().Be(Point.Zero);
        }

        [Fact]
        public void SparseNavigateDown()
        {
            layout.NavigationBehavior = NavigationBehavior.Sparse;

            SetLayout(
                "x x   ",
                " x    ",
                "   xx ",
                " x x  ",
                "      ");

            layout.FocusPoint.Should().Be(Point.Zero);

            layout.MoveDown(); layout.FocusPoint.Should().Be(new Point(1, 1));
            layout.MoveDown(); layout.FocusPoint.Should().Be(new Point(1, 3));
            layout.MoveDown(); layout.FocusPoint.Should().Be(new Point(2, 0));
            layout.MoveDown(); layout.FocusPoint.Should().Be(new Point(3, 2));
            layout.MoveDown(); layout.FocusPoint.Should().Be(new Point(3, 3));
            layout.MoveDown(); layout.FocusPoint.Should().Be(new Point(4, 2));
            layout.MoveDown(); layout.FocusPoint.Should().Be(Point.Zero);
        }

        [Fact]
        public void SparseNavigateUp()
        {
            layout.NavigationBehavior = NavigationBehavior.Sparse;

            SetLayout(
                "x x   ",
                " x    ",
                "   xx ",
                " x x  ",
                "      ");

            layout.FocusPoint.Should().Be(Point.Zero);

            layout.MoveUp(); layout.FocusPoint.Should().Be(new Point(4, 2));
            layout.MoveUp(); layout.FocusPoint.Should().Be(new Point(3, 3));
            layout.MoveUp(); layout.FocusPoint.Should().Be(new Point(3, 2));
            layout.MoveUp(); layout.FocusPoint.Should().Be(new Point(2, 0));
            layout.MoveUp(); layout.FocusPoint.Should().Be(new Point(1, 3));
            layout.MoveUp(); layout.FocusPoint.Should().Be(new Point(1, 1));
            layout.MoveUp(); layout.FocusPoint.Should().Be(Point.Zero);
        }

        [Fact]
        public void DenseNavigate()
        {
            layout.NavigationBehavior = NavigationBehavior.Dense;

            SetLayout(
                "x xxx",
                " x xx",
                "x  xx",
                " x x ",
                " xxx ",
                "x   x");

            layout.FocusPoint.Should().Be(Point.Zero);

            layout.MoveRight(); layout.FocusPoint.Should().Be(new Point(2, 0));
            layout.MoveRight(); layout.FocusPoint.Should().Be(new Point(3, 0));
            layout.MoveRight(); layout.FocusPoint.Should().Be(new Point(4, 0));
            layout.MoveRight(); layout.FocusPoint.Should().Be(new Point(0, 0));
            layout.MoveLeft(); layout.FocusPoint.Should().Be(new Point(4, 0));

            layout.MoveDown(); layout.FocusPoint.Should().Be(new Point(4, 1));
            layout.MoveRight(); layout.FocusPoint.Should().Be(new Point(1, 1));
            layout.MoveRight(); layout.FocusPoint.Should().Be(new Point(3, 1));
            layout.MoveRight(); layout.FocusPoint.Should().Be(new Point(4, 1));

            layout.MoveDown(); layout.FocusPoint.Should().Be(new Point(4, 2));
            layout.MoveRight(); layout.FocusPoint.Should().Be(new Point(0, 2));
            layout.MoveRight(); layout.FocusPoint.Should().Be(new Point(3, 2));
            layout.MoveRight(); layout.FocusPoint.Should().Be(new Point(4, 2));

            layout.MoveDown(); layout.FocusPoint.Should().Be(new Point(4, 5));
            layout.MoveDown(); layout.FocusPoint.Should().Be(new Point(4, 0));
            layout.MoveUp(); layout.FocusPoint.Should().Be(new Point(4, 5));
            layout.MoveDown(); layout.FocusPoint.Should().Be(new Point(4, 0));
            layout.MoveLeft();
            layout.MoveDown();
            layout.MoveDown(); 
            layout.MoveDown(); layout.FocusPoint.Should().Be(new Point(3, 3));

            layout.MoveRight(); layout.FocusPoint.Should().Be(new Point(1, 3));
            layout.MoveRight(); layout.FocusPoint.Should().Be(new Point(3, 3));
            layout.MoveLeft(); layout.FocusPoint.Should().Be(new Point(1, 3));
            layout.MoveLeft(); layout.FocusPoint.Should().Be(new Point(3, 3));

            layout.MoveDown(); layout.FocusPoint.Should().Be(new Point(3, 4));
            layout.MoveRight(); layout.FocusPoint.Should().Be(new Point(1, 4));
            layout.MoveRight(); layout.FocusPoint.Should().Be(new Point(2, 4));
            layout.MoveRight(); layout.FocusPoint.Should().Be(new Point(3, 4));
        }

        private void SetLayout(params string[] layoutStrings)
        {
            foreach (var line in layoutStrings)
                line.Length.Should().Be(layoutStrings[0].Length,
                "All lines in the layout should be the same length");

            layout.ResizeGrid(layoutStrings[0].Length, layoutStrings.Length);
            layout.Clear();

            for (int y = 0; y < layoutStrings.Length; y++)
            {
                for (int x = 0; x < layoutStrings[y].Length; x++)
                {
                    if (layoutStrings[y][x] != ' ')
                        AddWidget(x, y);
                }
            }
        }

        private void AddWidget(int x, int y)
        {
            layout[x, y] = CommonMocks.Widget($"{x},{y}").Object;
        }
    }
}
