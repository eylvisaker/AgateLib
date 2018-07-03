using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry.Builders;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace AgateLib.Tests.MathematicsTests.GeometryBuilderTests
{
    public class StarBuilderTests
    {
        [Fact]
        public void StarBuild_FivePointedStarPoints()
        {
            var radii = new[] { 8, 4 };

            var star = new StarBuilder().BuildStar(5, radii[0], radii[1], Vector2.Zero, 0);

            star.Points.Count.Should().Be(10);

            int index = 0;
            float lastAngle = -1;

            foreach (var point in star.Points)
            {
                var radius = point.Length();
                var angle = point.Angle();
                if (angle < 0) angle += MathF.PI * 2;

                radius.Should().BeApproximately(radii[index % 2], 1e-6f);
                angle.Should().BeGreaterThan(lastAngle,
                    $"Expected new angle to be larger but instead lastAngle = {lastAngle} and angle = {angle}");

                lastAngle = angle;

                index++;
            }
        }

        [Fact]
        public void StarBuild_FivePointedStarCenter()
        {
            var center = new Vector2(10, 12);

            var star = new StarBuilder().BuildStar(5, 8, 4, center, 1);

            star.Points.Count.Should().Be(10);

            var avg = star.Points.Average();

            Vector2X.Equals(center, avg, 1e-6f).Should().BeTrue($"Expected {center}, actual {avg}");
        }

        [Fact]
        public void StarBuild_SixPointedStarPoints()
        {
            var radii = new[] { 8, 4 };

            var star = new StarBuilder().BuildStar(6, radii[0], radii[1]);

            star.Points.Count.Should().Be(12);

            int index = 0;
            float lastAngle = -1;

            foreach (var point in star.Points)
            {
                var radius = point.Length();
                var angle = point.Angle();
                if (angle < 0) angle += MathF.PI * 2;

                radius.Should().BeApproximately(radii[index % 2], 1e-8f);
                angle.Should().BeGreaterThan(lastAngle,
                    $"Expected new angle to be larger but instead lastAngle = {lastAngle} and angle = {angle}");

                lastAngle = angle;

                index++;
            }
        }

        [Fact]
        public void StarBuild_SixPointedStarCenter()
        {
            var center = new Vector2(10, 12);

            var star = new StarBuilder().BuildStar(6, 8, 4, center, 1);

            star.Points.Count.Should().Be(12);

            var avg = star.Points.Average();

            Vector2X.Equals(center, avg, 1e-6f).Should().BeTrue($"Expected {center}, actual {avg}");
        }

    }
}
