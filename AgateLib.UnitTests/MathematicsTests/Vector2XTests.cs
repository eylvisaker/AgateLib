using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace AgateLib.Tests.MathematicsTests
{
    public class Vector2Tests
    {
        [Fact]
        public void V2_Parse()
        {
            Vector2 result = Vector2X.Parse("3.5,-4.2");

            result.X.Should().BeApproximately(3.5f, 1e-6f);
            result.Y.Should().BeApproximately(-4.2f, 1e-6f);
        }

        [Fact]
        public void V2_ParseScientificNotation()
        {
            Vector2 result = Vector2X.Parse("-1.2e+12,2.4e-18");

            result.X.Should().BeApproximately(-1.2e+12f, 1e-6f);
            result.Y.Should().BeApproximately(2.4e-18f, 1e-6f);
        }

        [Fact]
        public void V2_RotateEquals()
        {
            Vector2 a = new Vector2(3, 4);
            Vector2 b = a.RotateDegrees(90);
            var expected = new Vector2(4, -3);

            Vector2X.Equals(b, expected).Should().BeTrue();
        }

        [Fact]
        public void V2_Equals()
        {
            Vector2 a = Vector2.UnitX;
            Vector2 b = Vector2.UnitX * 1.000001f;

            Vector2X.Equals(a, b, 0.0001f).Should().BeTrue();
        }

        [Fact]
        public void V2_FromPolar()
        {
            Vector2 a = Vector2X.FromPolar(1, MathF.PI * 0.5f);
            Vector2 b = Vector2X.FromPolar(1, MathF.PI * 1.0f);
            Vector2 c = Vector2X.FromPolar(1, MathF.PI * 1.5f);
            Vector2 d = Vector2X.FromPolar(1, MathF.PI * 2.0f);

            Vector2X.Equals(a, Vector2.UnitY, .000001f);
            Vector2X.Equals(b, Vector2.UnitX, .000001f);
            Vector2X.Equals(c, Vector2.UnitY, .000001f);
            Vector2X.Equals(d, Vector2.UnitX, .000001f);
        }
        [Fact]
        public void V2_ProjectionContract()
        {
            ProjectionTest(new Vector2(1, 0), new Vector2(1, 1), new Vector2(0.5f, 0.5f));

            for (int i = 0; i < 360; i++)
            {
                const float radius = 2;

                Vector2 v = Vector2X.FromPolarDegrees(radius, i);
                Vector2 direction = new Vector2(1, 1);
                direction.Normalize();
                Vector2 expected = radius * MathF.Cos((i - 45) * MathF.PI / 180) * direction;

                ProjectionTest(v, direction, expected);
            }
        }

        private static void ProjectionTest(Vector2 v, Vector2 direction, Vector2 expected)
        {
            Vector2 result = v.ProjectionOn(direction);

            var perp = v - result;

            Vector2.Dot(direction, perp).Should().BeApproximately(0, 1e-5f);

            Vector2X.Equals(result, expected).Should().BeTrue(
                $"Projection of {v} on {direction} should yield {expected} but got {result} instead.");
        }
    }
}
