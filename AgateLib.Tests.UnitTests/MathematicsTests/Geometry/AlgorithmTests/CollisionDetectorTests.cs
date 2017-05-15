using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Algorithms.CollisionDetection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.MathematicsTests.Geometry.AlgorithmTests
{
	[TestClass]
	public class CollisionDetectorTests : PolygonUnitTest
	{
		CollisionDetector collider = new CollisionDetector();

		[TestMethod]
		public void CD_RectsCollide()
		{
			var a = Rectangle.FromLTRB(0, 0, 10, 10).ToPolygon();
			var b = Rectangle.FromLTRB(9, 9, 20, 20).ToPolygon();

			Assert.IsTrue(collider.DoPolygonsIntersect(a, b));
		}

		[TestMethod]
		public void CD_RectsDontCollide()
		{
			var a = Rectangle.FromLTRB(0, 0, 10, 10).ToPolygon();
			var b = Rectangle.FromLTRB(8, 11, 20, 20).ToPolygon();

			Assert.IsFalse(collider.DoPolygonsIntersect(a, b));
		}

		[TestMethod]
		public void CD_SharedCornerDoesntCollide()
		{
			var a = Rectangle.FromLTRB(0, 0, 10, 10).ToPolygon();
			var b = Rectangle.FromLTRB(10, 10, 20, 20).ToPolygon();

			Assert.IsFalse(collider.DoPolygonsIntersect(a, b));
		}

		[TestMethod]
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

			var b = a.RotateDegrees(180.0, new Vector2(5, 5));

			Assert.IsTrue(collider.DoPolygonsIntersect(a, b));
		}

		[TestMethod]
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

			var b = Rectangle.FromLTRB(2, 2, 3, 3).ToPolygon();

			Assert.IsFalse(collider.DoPolygonsIntersect(a, b));
		}

		[TestMethod]
		public void CD_GJKCollisionRectangles()
		{
			var pa = new Rectangle(-2, 0, 1, 1).ToPolygon();
			var pb = new Rectangle(2, 0, 1, 1).ToPolygon();

			var gjk = new GilbertJohnsonKeerthiAlgorithm();

			Assert.AreEqual(3, gjk.DistanceBetween(pa, pb));
			Assert.IsFalse(gjk.AreColliding(pa, pb));

			pb.TranslateSelf(-3.1 * Vector2.UnitX);

			Assert.AreEqual(0, gjk.DistanceBetween(pa, pb));
			Assert.IsTrue(gjk.AreColliding(pa, pb));
		}

		[TestMethod]
		public void CD_GJKDistanceRectangles()
		{
			var pa = new Rectangle(-2, 0, 1, 1).ToPolygon();
			var pb = new Rectangle(2, 0, 1, 1).ToPolygon();

			var gjk = new GilbertJohnsonKeerthiAlgorithm();

			Assert.AreEqual(3, gjk.DistanceBetween(pa, pb));
		}


		[TestMethod]
		public void CD_GJKCollisionL()
		{
			var pa = TetrisL.Translate(-2 * Vector2.UnitX);
			var pb = TetrisL.Translate(2 * Vector2.UnitX);

			var gjk = new GilbertJohnsonKeerthiAlgorithm();

			Assert.AreEqual(2, gjk.DistanceBetween(pa, pb));
			Assert.IsFalse(gjk.AreColliding(pa, pb));

			pb.TranslateSelf(-2.1 * Vector2.UnitX);

			Assert.AreEqual(0, gjk.DistanceBetween(pa, pb));
			Assert.IsTrue(gjk.AreColliding(pa, pb));
		}

		[TestMethod]
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

			Assert.IsTrue(simplex.ContainsOrigin);
			Assert.AreEqual(new Vector2(0.5, 0), pv);
		}

		[TestMethod]
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

				Assert.IsTrue(simplex.ContainsOrigin, $"Collision not detected by Minkowski simplex at {d}.");
				Assert.IsTrue(new Vector2(d, 0).Equals(pv.Value, 1e-6), $"Penetration depth test failed at {d}.");
			}
		}

		[TestMethod]
		public void CD_ContactPoint()
		{
			var pa = Diamond;
			var pb = new RectangleF(0.9f, -1, 1, 2).ToPolygon();

			var contactPoint = collider.FindConvexContactPoint(pa, pb);

			Assert.AreEqual(pa, contactPoint.FirstPolygon);
			Assert.AreEqual(pb, contactPoint.SecondPolygon);

			Assert.IsTrue(new Vector2(0.1, 0).Equals(contactPoint.PenetrationDepth, 1e-6),
				$"Penetration depth failed. Expected (0.1, 0) but got {contactPoint.PenetrationDepth}.");

			Assert.AreEqual(new Vector2(1, 0), contactPoint.FirstPolygonContactPoint);
			Assert.AreEqual(new Vector2(-0.5, 0), contactPoint.SecondPolygonContactPoint);
		}

		[TestMethod]
		public void CD_ContactPointGradual()
		{
			for (float d = 0.1f; d < 1; d += 0.05f)
			{
				var pa = new Polygon(Diamond.Points.Select(x => 10 * x));
				var pb = new RectangleF(10 - d, -10, 10, 20).ToPolygon();

				var contactPoint = collider.FindConvexContactPoint(pa, pb);

				Assert.AreEqual(pa, contactPoint.FirstPolygon);
				Assert.AreEqual(pb, contactPoint.SecondPolygon);

				Assert.IsTrue(new Vector2(d, 0).Equals(contactPoint.PenetrationDepth, 1e-6), $"Penetration depth test failed at {d}.");

				Assert.IsTrue(new Vector2(10, 0).Equals(contactPoint.FirstPolygonContactPoint, 1e-6),
					$"Contact point on diamond failed at {d}. Expected (10, 0) but got {contactPoint.FirstPolygonContactPoint}");
				Assert.IsTrue(new Vector2(-5, 0).Equals(contactPoint.SecondPolygonContactPoint, 1e-6),
					$"Contact point on rectangle failed at {d}. Expected (-5, 0) but got {contactPoint.SecondPolygonContactPoint}");
			}
		}

		[TestMethod]
		public void CD_EdgeTouching()
		{
			var polyA = new Polygon
			{
				{3126, 535 },
				{3126, 608 },
				{3156, 608 },
				{3156,535 },
			};

			var polyB = new Polygon
			{
				{3104, 608 },
				{3104, 640 },
				{3136, 640 },
				{3136, 608 },
			};

			var contactPoint = collider.FindConvexContactPoint(polyA, polyB);

			Assert.AreEqual(Point.Zero, contactPoint.PenetrationDepth);
		}

		[TestMethod]
		public void CD_CloseApproach()
		{
			var polyA = new Polygon
			{
				{607.7, 644.8 },
				{637.7, 644.8 },
				{637.7, 717.8 },
				{637.7,717.8 },
			};

			var polyB = new Polygon
			{
				{564, 718},
				{564, 750},
				{692, 750},
				{692, 718},
			};

			var contactPoint = collider.FindConvexContactPoint(polyA, polyB);

			Assert.AreEqual(0.2, contactPoint.DistanceToContact, 1e-6);
		}

		/// <summary>
		/// OddCase and OddCase2 have demonstrated an instability - one passed and the other failed.
		/// These are here to prevent a regression.
		/// </summary>
		[TestMethod]
		public void CD_OddCase()
		{
			var polyA = new Polygon
			{
				 {1418.51965332031,409.606781005859},
				 {1448.51965332031,409.606781005859},
				 {1448.51965332031,482.606781005859},
				 {1418.51965332031,482.606781005859}
			};

			var polyB = new Polygon
			{
				{1328,482},
				{1328,514},
				{1456,514},
				{1456,482}
			};

			var contactPoint = collider.FindConvexContactPoint(polyA, polyB);

			Assert.IsTrue(contactPoint.Contact);
			Assert.AreEqual(0.606781, contactPoint.PenetrationDepth.Magnitude, 1e-6);
		}


		[TestMethod]
		public void CD_OddCase2()
		{
			var polyA = new RectangleF(1418.51965332031f, 409.606781005859f, 30, 73).ToPolygon();
			var polyB = new RectangleF(1328, 482, 128, 32).ToPolygon();

			var contactPoint = collider.FindConvexContactPoint(polyA, polyB);

			Assert.IsTrue(contactPoint.Contact);
			Assert.AreEqual(0.606781, contactPoint.PenetrationDepth.Magnitude, 1e-6);
		}
	}
}
