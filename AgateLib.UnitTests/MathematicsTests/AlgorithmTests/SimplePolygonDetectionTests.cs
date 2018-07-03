using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace AgateLib.Tests.MathematicsTests.AlgorithmTests
{
    public class SimplePolygonDetectionTests
    {
        public SimplePolygonDetectionTests()
        {
            Polygon.AssumeAllPolygonsAreSimple = false;
        }

        [Fact]
        public void Polygon_RectangleIsSimple()
        {
            new Rectangle(1, 2, 3, 4).ToPolygon().IsSimple.Should().BeTrue();
        }

        [Fact]
        public void Polygon_ZeroPolygonIsSimple()
        {
            var poly = new Rectangle().ToPolygon();

            poly.Count.Should().Be(4);
            poly.IsSimple.Should().BeTrue();
        }

        [Fact]
        public void Polygon_WithHoleIsComplex()
        {
            // This polygon looks like this:
            //   ┌─┐
            //   │┌┼┐
            //   │└┘│
            //   └──┘
            // where the crossing at (2, 2) happens because 
            // the line segments go from (1, 2) - (3, 2) and (2, 1) - (2, 3)
            var p = new Polygon(new Vector2List
            {
                {0, 0},
                {3, 0},
                {3, 2},
                {1, 2},
                {1, 1},
                {2, 1},
                {2, 3},
                {0, 3}
            });

            p.IsComplex.Should().BeTrue();
        }

        [Fact]
        public void Polygon_WithCrossingIsComplex()
        {
            // This polygon looks like this:
            //    ┌┐
            //    └┼┐
            //     └┘	
            // where the two middle line segments are long and cross each other
            var p = new Polygon(new Vector2List
            {
                {0, 0},
                {1, 0},
                {1, 1},
                {-1, 1},
                {-1, 2},
                {0, 3},
            });

            p.IsComplex.Should().BeTrue();
        }

        [Fact]
        public void Polygon_DiagonalLinesIsComplex()
        {
            // Same as PolygonWithCrossingIsComplex, but the upper-left and lower-right lines are diagonal.
            var p = new Polygon(new Vector2List
            {
                {0, 0},
                {1, 1},
                {-1, 1},
                {0, 3},
            });

            p.IsComplex.Should().BeTrue();
        }

        [Fact]
        public void Polygon_ComplexBroken()
        {
            var p = new Polygon
            {
                {40, 0},
                {2.5711333389961f, 1.86803771595309f},
                {12.3606797749979f, 38.0422606518061f},
                {-0.982085545888505f, 3.02254851667882f},
                {-32.3606797749979f, 23.5114100916989f},
                {-3.17809558621519f, 3.89191604793398E-16f},
                {-32.3606797749979f, -23.5114100916989f},
                {-0.982085545888505f, -3.02254851667882f},
                {12.3606797749979f, -38.0422606518061f},
                {2.5711333389961f, -1.86803771595309f},
            };

            p.IsSimple.Should().BeTrue();
        }
    }
}
