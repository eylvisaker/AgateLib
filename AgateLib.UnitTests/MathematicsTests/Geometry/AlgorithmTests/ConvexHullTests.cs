using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Algorithms.ConvexDecomposition;
using FluentAssertions;
using Xunit;

namespace AgateLib.Tests.MathematicsTests.Geometry.AlgorithmTests
{
    public class ConvexHullTests : PolygonUnitTest
    {
        [Fact]
        public void ConvexHull_ConvexShape()
        {
            var result = new QuickHull().FindConvexHull(Diamond);

            VerifyContainment(result, Diamond);
        }

        [Fact]
        public void ConvexHull_OfConcaveShape()
        {
            var result = new QuickHull().FindConvexHull(TetrisL);

            result.IsConvex.Should().BeTrue("Convex hull isn't convex!");

            VerifyContainment(result, TetrisL);
        }

        private void VerifyContainment(Polygon result, Polygon initial)
        {
            foreach (var pt in initial)
            {
                result.AreaContains(pt).Should().BeTrue(
                    $"Convex hull does not contain point {pt}");
            }
        }

    }
}
