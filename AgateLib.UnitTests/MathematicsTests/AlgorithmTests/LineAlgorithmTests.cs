using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry.Algorithms;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace AgateLib.Tests.MathematicsTests.AlgorithmTests
{
    public class LineAlgorithmTests
    {
        [Fact]
        public void Line_SegmentIntersectionHighSymmetry()
        {
            var result = LineAlgorithms.LineSegmentIntersection(
                Vector2.UnitX,
                -Vector2.UnitX,
                Vector2.UnitY,
                -Vector2.UnitY);

            result.Should().NotBeNull();
            result.U1.Should().BeApproximately(0.5f, 0.0001f);
            result.U2.Should().BeApproximately(0.5f, 0.0001f);
            Vector2X.Equals(Vector2.Zero, result.IntersectionPoint).Should().BeTrue("Lines should intersect at origin");
        }

        [Fact]
        public void Line_SegmentIntersectionLowSymmetry()
        {
            var result = LineAlgorithms.LineSegmentIntersection(
                new Vector2(704, 336), new Vector2(569, 299),
                new Vector2(436, 218), new Vector2(446, 357));

            result.Should().NotBeNull();
            Vector2X.Equals(result.IntersectionPoint, new Vector2(439.269f, 263.444f), 1e-3f).Should().BeTrue(
                "Intersection was not in correct position.");
        }

        [Fact]
        public void Line_SegmentIntersectionParallel()
        {
            var result = LineAlgorithms.LineSegmentIntersection(
                Vector2.UnitX,
                -Vector2.UnitX,
                Vector2.UnitX + Vector2.UnitY,
                -Vector2.UnitX + Vector2.UnitY);

            result.Should().BeNull();
        }

        [Fact]
        public void Line_Side()
        {
            LineAlgorithms.SideOf(Vector2.Zero, Vector2.UnitY, Vector2.UnitX).Should().Be(1);

            LineAlgorithms.SideOf(Vector2.Zero, Vector2.UnitX + Vector2.UnitY,
                Vector2.UnitY).Should().BeApproximately(-MathF.Sqrt(0.5f), 1e-6f);
        }
    }
}
