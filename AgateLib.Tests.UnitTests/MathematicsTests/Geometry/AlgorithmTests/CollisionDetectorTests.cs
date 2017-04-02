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
			var b = Rectangle.FromLTRB(10, 9, 20, 20).ToPolygon();

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
				{10, 10 },
				{5, 10 },
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
			var pa = Diamond;
			var pb = new RectangleF(0.5f, -1, 1, 2).ToPolygon();

			var gjk = new GilbertJohnsonKeerthiAlgorithm();

			var simplex = gjk.FindMinkowskiSimplex(pa, pb);
			var epa = new ExpandingPolytopAlgorithm();

			var pv = epa.PenetrationDepth(
				v => GilbertJohnsonKeerthiAlgorithm.PolygonSupport(pa, v),
				v => GilbertJohnsonKeerthiAlgorithm.PolygonSupport(pb, v),
				simplex.Simplex);

			Assert.AreEqual(new Vector2(0.5, 0), pv);
		}

		[TestMethod]
		public void CD_GJK_EPA_GradualPenetrationDepth()
		{
			for (float d = 0.1f; d < 1; d += 0.05f)
			{
				var pa = Diamond;
				var pb = new RectangleF(1 - d, -1, 1, 2).ToPolygon();

				var gjk = new GilbertJohnsonKeerthiAlgorithm();

				var simplex = gjk.FindMinkowskiSimplex(pa, pb);
				var epa = new ExpandingPolytopAlgorithm();

				var pv = epa.PenetrationDepth(
					v => GilbertJohnsonKeerthiAlgorithm.PolygonSupport(pa, v),
					v => GilbertJohnsonKeerthiAlgorithm.PolygonSupport(pb, v),
					simplex.Simplex);

				Assert.IsTrue(new Vector2(d, 0).Equals(pv, 1e-6), $"Penetration depth test failed at {d}");
			}
		}
	}
}
