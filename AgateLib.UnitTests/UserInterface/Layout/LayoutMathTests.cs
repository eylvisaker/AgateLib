using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Layout;
using AgateLib.UserInterface.Styling;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AgateLib.Tests.UserInterface.Layout
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
