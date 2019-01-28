﻿using AgateLib.Mathematics.Geometry;
using AgateLib.Tests;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Moq;
using Xunit;

namespace AgateLib.UserInterface.Layout
{
    public class GridLayoutTests
    {
        private Mock<RenderElement<RenderElementProps>> parent;
        private Grid grid;
        private GridNavigationWrap navigationWrap;

        [Fact]
        public void GridWithRightAlignText()
        {
            void AlignRight(LabelProps labelProps)
            {
                labelProps.Style = new InlineElementStyle
                {
                    TextAlign = TextAlign.Right
                };
            }

            var context = CommonMocks.RenderContext();

            grid = new Grid(new GridProps { Columns = 2 }
                .Add("Strength").Add("12", AlignRight)
                .Add("Dexterity").Add("8", AlignRight));

            Size maxSize = new Size(300, 300);
            Size idealSize = grid.CalcIdealContentSize(context.Object, maxSize);
            grid.DoLayout(context.Object, idealSize);

            idealSize.Should().Be(new Size(55, 20));

            grid.Children[1].Display.ContentRect.Should().Be(
                new Rectangle(45, 0, 10, 10));

            grid.Children[3].Display.ContentRect.Should().Be(
                new Rectangle(45, 10, 10, 10));
        }
    }
}