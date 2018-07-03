using AgateLib.Mathematics.Geometry.Algorithms;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace AgateLib.Tests.MathematicsTests.Geometry.AlgorithmTests
{
    public class LineAlgorithmTests
    {
        [Fact]
        public void Line_SideOf()
        {
            LineAlgorithms.SideOf(Vector2.Zero, Vector2.UnitX, Vector2.UnitY).Should().Be(-1);
            LineAlgorithms.SideOf(Vector2.Zero, Vector2.UnitX, -Vector2.UnitY).Should().Be(1);
        }
    }
}
