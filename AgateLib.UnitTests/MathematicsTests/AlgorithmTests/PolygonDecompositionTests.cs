using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using FluentAssertions;

namespace AgateLib.Tests.MathematicsTests.AlgorithmTests
{
    public class PolygonDecompositionTests
    {
        private Polygon TetrisL { get; } = new Polygon
            {
                Vector2.Zero,
                { 2, 0 },
                { 2, 1 },
                { 1, 1 },
                { 1, 3 },
                { 0, 3 },
            };

        private Polygon TetrisT { get; } = new Polygon
            {
                Vector2.Zero,
                { 3, 0 },
                { 3, 1 },
                { 2, 1 },
                { 2, 2 },
                { 1, 2 },
                { 1, 1 },
                { 0, 1 }
            };

        private Polygon Cross { get; } = new Polygon
            {
                Vector2.Zero,
                { 1, 0 },
                { 1, -1 },
                { 2, -1 },
                { 2, 0 },
                { 3, 0 },
                { 3, 1 },
                { 2, 1 },
                { 2, 2 },
                { 1, 2 },
                { 1, 1 },
                { 0, 1 }
            };

        [Fact]
        public void Poly_ConvexDecompositionL()
        {
            var convexPolys = TetrisL.ConvexDecomposition;

            convexPolys.All(p => p.IsConvex).Should().BeTrue();

            convexPolys.Count().Should().Be(2);
            convexPolys.First().Points.Count.Should().Be(4);
            convexPolys.First().Points.Count.Should().Be(4);
        }

        [Fact]
        public void Poly_ConvexDecompositionT()
        {
            var convexPolys = TetrisT.ConvexDecomposition;

            convexPolys.All(p => p.IsConvex).Should().BeTrue();

            convexPolys.Count().Should().Be(2);
            convexPolys.First().Points.Count.Should().Be(4);
            convexPolys.First().Points.Count.Should().Be(4);
        }

        [Fact]
        public void Poly_ConvexDecompositionCross()
        {
            var convexPolys = Cross.ConvexDecomposition;

            convexPolys.All(p => p.IsConvex).Should().BeTrue();

            convexPolys.Count().Should().Be(3);
        }
    }
}
