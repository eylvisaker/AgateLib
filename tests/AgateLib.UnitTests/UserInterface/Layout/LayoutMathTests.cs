﻿using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Layout;
using FluentAssertions;
using Xunit;

namespace AgateLib.Demo.UserInterface.Layout
{
    public class LayoutMathTests
    {
        [Theory]
        [InlineData(200, 100, 300, 150, 200, 100)]
        [InlineData(200, 100, 180, 150, 180, 100)]
        [InlineData(200, 100, 220, 90, 200, 90)]
        public void ConstrainMaxSizeWithMaxSizeConstraintTest(
            int initialWidth,
            int initialHeight,
            int maxWidth,
            int maxHeight,
            int expectedMaxWidth,
            int expectedMaxHeight)
        {
            Size initial = new Size(initialWidth, initialHeight);

            SizeConstraints constraints = new SizeConstraints
            {
                MaxWidth = maxWidth,
                MaxHeight = maxHeight,
            };

            Size result = LayoutMath.ConstrainMaxSize(initial, constraints);

            result.Width.Should().Be(expectedMaxWidth);
            result.Height.Should().Be(expectedMaxHeight);
        }

        [Theory]
        [InlineData(200, 100, 300, 150, 200, 100)]
        [InlineData(200, 100, 180, 150, 180, 100)]
        [InlineData(200, 100, 220, 90, 200, 90)]
        public void ConstrainMaxSizeWithFixedSizeConstraintTest(
            int initialWidth,
            int initialHeight,
            int maxWidth,
            int maxHeight,
            int expectedMaxWidth,
            int expectedMaxHeight)
        {
            Size initial = new Size(initialWidth, initialHeight);

            SizeConstraints constraints = new SizeConstraints
            {
                MaxWidth = maxWidth,
                MaxHeight = maxHeight,
            };

            Size result = LayoutMath.ConstrainMaxSize(initial, constraints);

            result.Width.Should().Be(expectedMaxWidth);
            result.Height.Should().Be(expectedMaxHeight);
        }
    }
}
