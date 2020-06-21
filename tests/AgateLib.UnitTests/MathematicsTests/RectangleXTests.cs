using AgateLib.Mathematics.Geometry;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace AgateLib.Tests.MathematicsTests
{
    public class RectangleXTests
    {
        [Fact]
        public void Rect_ContractByInt()
        {
            var rect = new Rectangle(5, 10, 15, 20);

            var result = rect.Contract(1);

            result.Should().Be(new Rectangle(6, 11, 13, 18));
        }

        [Fact]
        public void Rect_ContractBySize()
        {
            var rect = new Rectangle(5, 10, 15, 20);

            var result = rect.Contract(new Size(1, 2));

            result.Should().Be(new Rectangle(6, 12, 13, 16));
        }

        [Fact]
        public void Rect_MultiUnion()
        {
            var result = RectangleX.Union(
                new Rectangle(1, 2, 3, 4),
                new Rectangle(5, 6, 7, 8),
                new Rectangle(9, 10, 11, 12),
                new Rectangle(2, 2, 2, 2));

            result.Should().Be(new Rectangle(1, 2, 19, 20));
        }

        [Fact]
        public void Rect_CenterPoint_EvenValues()
        {
            var rect = new Rectangle(10, 10, 40, 30);

            rect.CenterPoint().Should().Be(rect.Center.ToVector2());
        }

        [Fact]
        public void Rect_CenterPoint_OddValues()
        {
            var rect = new Rectangle(10, 12, 5, 3);

            rect.CenterPoint().Should().Be(new Vector2(12.5f, 13.5f));
        }
    }
}
