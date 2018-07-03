using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;
using Xunit;
using AgateLib.Mathematics;
using FluentAssertions;
using Microsoft.Xna.Framework;

namespace AgateLib.Tests.MathematicsTests
{
    public class RectangleFTests
    {
        [Fact]
        public void RectF_ContractByFloat()
        {
            var rect = new RectangleF(5, 10, 15, 20);

            var result = rect.Contract(1);

            result.Should().Be(new RectangleF(6, 11, 13, 18));
        }

        [Fact]
        public void RectF_ContractBySize()
        {
            var rect = new RectangleF(5, 10, 15, 20);

            var result = rect.Contract(new SizeF(1, 2));

            result.Should().Be(new RectangleF(6, 12, 13, 16));
        }

        [Fact]
        public void RectF_ContainsVector()
        {
            var rect = new RectangleF(1, 2, 3, 4);

            rect.Contains(1, 2).Should().BeTrue();
            rect.Contains(0.9f, 2).Should().BeFalse();
            rect.Contains(1, 1.9f).Should().BeFalse();
            rect.Contains(4.1f, 2).Should().BeFalse();
            rect.Contains(1, 6.1f).Should().BeFalse();
        }

        [Fact]
        public void RectF_Intersection()
        {
            var a = new RectangleF(1, 2, 10, 12);
            var b = new RectangleF(5, 7, 14, 32);

            RectangleF.Intersection(a, b).Should().Be(RectangleF.FromLTRB(5, 7, 11, 14));
        }

        [Fact]
        public void RectF_ToPolygon()
        {
            var rect = new RectangleF(1, 2, 3, 4);
            var poly = rect.ToPolygon();

            poly.Count.Should().Be(4);
            poly.Points.Contains(new Vector2(1, 2)).Should().BeTrue();
            poly.Points.Contains(new Vector2(4, 2)).Should().BeTrue();
            poly.Points.Contains(new Vector2(4, 6)).Should().BeTrue();
            poly.Points.Contains(new Vector2(1, 6)).Should().BeTrue();
        }
    }
}
