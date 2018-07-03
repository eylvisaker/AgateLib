using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;
using YamlDotNet.Serialization;

namespace AgateLib.Tests.MathematicsTests
{
    public class PolygonTests : PolygonUnitTest
    {
        private Polygon OddConcave { get; } = new Polygon
            {
                Vector2.Zero,
                { 1, 0 },
                { 1.5f, 0.5f },
                { 2, 0 },
                { 2, 2 },
            };

        [Fact]
        public void Poly_Area()
        {
            Diamond.Area.Should().BeApproximately(2, 0.000001);
            TetrisL.Area.Should().BeApproximately(4, 0.000001);
        }

        [Fact]
        public void Poly_BoundingRect()
        {
            Diamond.BoundingRect.Should().Be(RectangleF.FromLTRB(-1, -1, 1, 1));
        }

        [Fact]
        public void Poly_IsConvex()
        {
            Diamond.IsConvex.Should().BeTrue("Diamond should report convexity.");
            OddConcave.IsConcave.Should().BeTrue("Odd concave shape should report concavity.");
        }

        [Fact]
        public void Poly_PointOnEdgeIsInside()
        {
            Diamond.AreaContains(new Vector2(0.5f, 0.5f)).Should().BeTrue();
        }

        [Fact]
        public void Poly_DiamondContainsPoint()
        {
            Diamond.AreaContains(new Vector2(0, 0)).Should().BeTrue();

            Diamond.Translate(new Vector2(100, 15)).AreaContains(new Vector2(100, 15)).Should().BeTrue();
        }

        [Fact]
        public void Poly_OddConcaveContainsPoint()
        {
            OddConcave.AreaContains(new Vector2(1.5f, 0)).Should().BeFalse();
        }

        [Fact]
        public void Poly_SerializationRoundTrip()
        {
            var serializer = new SerializerBuilder()
                .WithTypeConvertersForBasicStructures()
                .Build();

            var deserializer = new DeserializerBuilder()
                .WithTypeConvertersForBasicStructures()
                .Build();

            var result = deserializer.Deserialize<Polygon>(serializer.Serialize(Diamond));

            result.Points.Count.Should().Be(Diamond.Points.Count);

            for (var index = 0; index < Diamond.Points.Count; index++)
            {
                result.Points[index].Should().Be(Diamond.Points[index]);
            }
        }

        [Fact]
        public void Poly_Rotate()
        {
            var a = RectangleX.FromLTRB(0, 0, 10, 5).ToPolygon();
            var b = a.RotateDegrees(90, new Vector2(10, 5));

            b.Count.Should().Be(4);

            b.Any(x => Vector2X.Equals(x, new Vector2(5, 5), 1e-5f)).Should().BeTrue();
            b.Any(x => Vector2X.Equals(x, new Vector2(10, 5), 1e-5f)).Should().BeTrue();
            b.Any(x => Vector2X.Equals(x, new Vector2(10, 15), 1e-5f)).Should().BeTrue();
            b.Any(x => Vector2X.Equals(x, new Vector2(5, 15), 1e-5f)).Should().BeTrue();
        }
    }
}
