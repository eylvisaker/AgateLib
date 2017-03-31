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
		public void CD_GJKCollision()
		{
			var pa = new Rectangle(-2, 0, 1, 1).ToPolygon();
			var pb = new Rectangle(2, 0, 1, 1).ToPolygon();

			var gjk = new GilbertJohnsonKeerthiAlgorithm();

			Assert.IsFalse(gjk.AreColliding(pa, pb));

			pb.TranslateSelf(-3.1 * Vector2.UnitX);

			Assert.IsTrue(gjk.AreColliding(pa, pb));
		}
	}
}
