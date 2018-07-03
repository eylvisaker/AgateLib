using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Algorithms.CollisionDetection;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace AgateLib.Tests.MathematicsTests.Geometry.AlgorithmTests
{
    public class CollisionDetectorTests : PolygonUnitTest
    {
        CollisionDetector collider = new CollisionDetector();

        [Fact]
        public void CD_RectsCollide()
        {
            var a = RectangleX.FromLTRB(0, 0, 10, 10).ToPolygon();
            var b = RectangleX.FromLTRB(9, 9, 20, 20).ToPolygon();

            collider.DoPolygonsIntersect(a, b).Should().BeTrue();
        }

        [Fact]
        public void CD_RectsDontCollide()
        {
            var a = RectangleX.FromLTRB(0, 0, 10, 10).ToPolygon();
            var b = RectangleX.FromLTRB(8, 11, 20, 20).ToPolygon();

            collider.DoPolygonsIntersect(a, b).Should().BeFalse();
        }

        [Fact]
        public void CD_SharedCornerDoesntCollide()
        {
            var a = RectangleX.FromLTRB(0, 0, 10, 10).ToPolygon();
            var b = RectangleX.FromLTRB(10, 10, 20, 20).ToPolygon();

            collider.DoPolygonsIntersect(a, b).Should().BeFalse();
        }

        [Fact]
        public void CD_ConcavePolygonsCollide()
        {
            var a = new Polygon
            {
                {0, 0},
                {10, 0},
                {10, 10},
                {5, 10},
                {5, 5},
                {0, 5}
            };

            var b = a.RotateDegrees(180.0f, new Vector2(5, 5));

            collider.DoPolygonsIntersect(a, b).Should().BeTrue();
        }

        [Fact]
        public void CD_ConcavePolygonsDontCollide()
        {
            var a = new Polygon
            {
                {0, 0},
                {5, 0},
                {5, 4},
                {4, 4},
                {4, 1},
                {1, 1},
                {1, 4},
                {0, 4},
            };

            var b = RectangleX.FromLTRB(2, 2, 3, 3).ToPolygon();

            collider.DoPolygonsIntersect(a, b).Should().BeFalse();
        }

        [Fact]
        public void CD_GJKCollisionRectangles()
        {
            var pa = new Rectangle(-2, 0, 1, 1).ToPolygon();
            var pb = new Rectangle(2, 0, 1, 1).ToPolygon();

            var gjk = new GilbertJohnsonKeerthiAlgorithm();

            gjk.DistanceBetween(pa, pb).Should().Be(3);
            gjk.AreColliding(pa, pb).Should().BeFalse();

            pb.TranslateSelf(-3.1f * Vector2.UnitX);

            gjk.DistanceBetween(pa, pb).Should().Be(0);
            gjk.AreColliding(pa, pb).Should().BeTrue();
        }

        [Fact]
        public void CD_GJKDistanceRectangles()
        {
            var pa = new Rectangle(-2, 0, 1, 1).ToPolygon();
            var pb = new Rectangle(2, 0, 1, 1).ToPolygon();

            var gjk = new GilbertJohnsonKeerthiAlgorithm();

            gjk.DistanceBetween(pa, pb).Should().Be(3);
        }


        [Fact]
        public void CD_GJKCollisionL()
        {
            var pa = TetrisL.Translate(-2 * Vector2.UnitX);
            var pb = TetrisL.Translate(2 * Vector2.UnitX);

            var gjk = new GilbertJohnsonKeerthiAlgorithm();

            gjk.DistanceBetween(pa, pb).Should().Be(2);
            gjk.AreColliding(pa, pb).Should().BeFalse();

            pb.TranslateSelf(-2.1f * Vector2.UnitX);

            gjk.DistanceBetween(pa, pb).Should().Be(0);
            gjk.AreColliding(pa, pb).Should().BeTrue();
        }

        [Fact]
        public void CD_GJK_EPA_PenetrationDepth()
        {
            var pa = new Polygon(Diamond.Points.Select(x => 10 * x));
            var pb = new RectangleF(9.5f, -10, 10, 20).ToPolygon();

            var gjk = new GilbertJohnsonKeerthiAlgorithm();

            var simplex = gjk.FindMinkowskiSimplex(pa, pb);
            var epa = new ExpandingPolytopeAlgorithm();

            var pv = epa.PenetrationDepth(
                v => GilbertJohnsonKeerthiAlgorithm.PolygonSupport(pa, v),
                v => GilbertJohnsonKeerthiAlgorithm.PolygonSupport(pb, v),
                simplex);

            simplex.ContainsOrigin.Should().BeTrue();
            pv.Should().Be(new Vector2(0.5f, 0));
        }

        [Fact]
        public void CD_GJK_EPA_GradualPenetrationDepth()
        {
            for (float d = 0.05f; d < 1; d += 0.05f)
            {
                var pa = new Polygon(Diamond.Points.Select(x => 10 * x));
                var pb = new RectangleF(10 - d, -10, 10, 20).ToPolygon();

                var gjk = new GilbertJohnsonKeerthiAlgorithm();

                var simplex = gjk.FindMinkowskiSimplex(pa, pb);
                var epa = new ExpandingPolytopeAlgorithm();

                var pv = epa.PenetrationDepth(
                    v => GilbertJohnsonKeerthiAlgorithm.PolygonSupport(pa, v),
                    v => GilbertJohnsonKeerthiAlgorithm.PolygonSupport(pb, v),
                    simplex);

                simplex.ContainsOrigin.Should().BeTrue($"Collision not detected by Minkowski simplex at {d}.");
                Vector2X.Equals(new Vector2(d, 0), pv.Value, 1e-6f).Should().BeTrue($"Penetration depth test failed at {d}.");
            }
        }

        [Fact]
        public void CD_ContactPoint()
        {
            var pa = Diamond;
            var pb = new RectangleF(0.9f, -1, 1, 2).ToPolygon();

            var contactPoint = collider.FindConvexContactPoint(pa, pb);

            contactPoint.FirstPolygon.Should().BeEquivalentTo(pa);
            contactPoint.SecondPolygon.Should().BeEquivalentTo(pb);

            Vector2X.Equals(new Vector2(0.1f, 0), contactPoint.PenetrationDepth, 1e-6f).Should().BeTrue(
                $"Penetration depth failed. Expected (0.1, 0) but got {contactPoint.PenetrationDepth}.");

            contactPoint.FirstPolygonContactPoint.Should().Be(new Vector2(1, 0));
            contactPoint.SecondPolygonContactPoint.Should().Be(new Vector2(-0.5f, 0));
        }

        [Fact]
        public void CD_ContactPointGradual()
        {
            for (float d = 0.1f; d < 1; d += 0.05f)
            {
                var pa = new Polygon(Diamond.Points.Select(x => 10 * x));
                var pb = new RectangleF(10 - d, -10, 10, 20).ToPolygon();

                var contactPoint = collider.FindConvexContactPoint(pa, pb);

                contactPoint.FirstPolygon.Should().BeEquivalentTo(pa);
                contactPoint.SecondPolygon.Should().BeEquivalentTo(pb);

                Vector2X.Equals(new Vector2(d, 0), contactPoint.PenetrationDepth, 1e-6f).Should().BeTrue($"Penetration depth test failed at {d}.");

                Vector2X.Equals(new Vector2(10, 0), contactPoint.FirstPolygonContactPoint, 1e-6f).Should().BeTrue(
                    $"Contact point on diamond failed at {d}. Expected (10, 0) but got {contactPoint.FirstPolygonContactPoint}");
                Vector2X.Equals(new Vector2(-5, 0), contactPoint.SecondPolygonContactPoint, 1e-6f).Should().BeTrue(
                    $"Contact point on rectangle failed at {d}. Expected (-5, 0) but got {contactPoint.SecondPolygonContactPoint}");
            }
        }

        [Fact]
        public void CD_EdgeTouching()
        {
            var polyA = new Polygon
            {
                {3126, 535},
                {3126, 608},
                {3156, 608},
                {3156, 535},
            };

            var polyB = new Polygon
            {
                {3104, 608},
                {3104, 640},
                {3136, 640},
                {3136, 608},
            };

            var contactPoint = collider.FindConvexContactPoint(polyA, polyB);

            contactPoint.PenetrationDepth.Should().Be(Vector2.Zero);
        }

        [Fact]
        public void CD_CloseApproach()
        {
            var polyA = new Polygon
            {
                {607.7f, 644.8f},
                {637.7f, 644.8f},
                {637.7f, 717.8f},
                {637.7f, 717.8f},
            };

            var polyB = new Polygon
            {
                {564, 718},
                {564, 750},
                {692, 750},
                {692, 718},
            };

            var contactPoint = collider.FindConvexContactPoint(polyA, polyB);

            contactPoint.DistanceToContact.Should().BeApproximately(0.2f, 1e-4f);
        }

        /// <summary>
        /// OddCase and OddCase2 have demonstrated an instability - one passed and the other failed.
        /// These are here to prevent a regression.
        /// </summary>
        [Fact]
        public void CD_OddCase()
        {
            var polyA = new Polygon
            {
                 {1418.51965332031f,409.606781005859f},
                 {1448.51965332031f,409.606781005859f},
                 {1448.51965332031f,482.606781005859f},
                 {1418.51965332031f,482.606781005859f}
            };

            var polyB = new Polygon
            {
                {1328,482},
                {1328,514},
                {1456,514},
                {1456,482}
            };

            var contactPoint = collider.FindConvexContactPoint(polyA, polyB);

            contactPoint.Contact.Should().BeTrue();
            contactPoint.PenetrationDepth.Length().Should().BeApproximately(0.606781f, 1e-4f);
        }


        [Fact]
        public void CD_OddCase2()
        {
            var polyA = new RectangleF(1418.51965332031f, 409.606781005859f, 30, 73).ToPolygon();
            var polyB = new RectangleF(1328, 482, 128, 32).ToPolygon();

            var contactPoint = collider.FindConvexContactPoint(polyA, polyB);

            contactPoint.Contact.Should().BeTrue();
            contactPoint.PenetrationDepth.Length().Should().BeApproximately(0.606781f, 1e-4f);
        }
    }
}
