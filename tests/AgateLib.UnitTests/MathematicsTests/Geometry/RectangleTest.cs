using AgateLib.Mathematics.Geometry;
using FluentAssertions;
using Xunit;

namespace AgateLib.Tests.MathematicsTests.Geometry
{
    public class RectangleTest
    {
        [Fact]
        public void FromStringDelimited()
        {
            var rect = RectangleX.Parse("2, 3, 4, 5");

            rect.X.Should().Be(2);
            rect.Y.Should().Be(3);
            rect.Width.Should().Be(4);
            rect.Height.Should().Be(5);
        }
    }
}