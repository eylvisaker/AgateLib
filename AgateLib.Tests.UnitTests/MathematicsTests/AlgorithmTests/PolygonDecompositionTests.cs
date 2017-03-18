using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics;

namespace AgateLib.UnitTests.MathematicsTests.AlgorithmTests
{
	[TestClass]
	public class PolygonDecompositionTests
	{
		private Polygon TetrisL { get; } = new Polygon
			{
				Vector2.Zero,
				{ 2, 0 },
				{ 2, 1 },
				{ 1, 1 },
				{ 1, 3 },
				{ 0, 3 },
			};

		private Polygon TetrisT { get; } = new Polygon
			{
				Vector2.Zero,
				{ 3, 0 },
				{ 3, 1 },
				{ 2, 1 },
				{ 2, 2 },
				{ 1, 2 },
				{ 1, 1 },
				{ 0, 1 }
			};

		private Polygon Cross { get; } = new Polygon
			{
				Vector2.Zero,
				{ 1, 0 },
				{ 1, -1 },
				{ 2, -1 },
				{ 2, 0 },
				{ 3, 0 },
				{ 3, 1 },
				{ 2, 1 },
				{ 2, 2 },
				{ 1, 2 },
				{ 1, 1 },
				{ 0, 1 }
			};

		[TestMethod]
		public void Poly_ConvexDecompositionL()
		{
			var convexPolys = TetrisL.ConvexDecomposition;

			Assert.IsTrue(convexPolys.All(p => p.IsConvex));

			Assert.AreEqual(2, convexPolys.Count());
			Assert.AreEqual(4, convexPolys.First().Points.Count);
			Assert.AreEqual(4, convexPolys.First().Points.Count);
		}

		[TestMethod]
		public void Poly_ConvexDecompositionT()
		{
			var convexPolys = TetrisT.ConvexDecomposition;

			Assert.IsTrue(convexPolys.All(p => p.IsConvex));

			Assert.AreEqual(2, convexPolys.Count());
			Assert.AreEqual(4, convexPolys.First().Points.Count);
			Assert.AreEqual(4, convexPolys.First().Points.Count);
		}

		[TestMethod]
		public void Poly_ConvexDecompositionCross()
		{
			var convexPolys = Cross.ConvexDecomposition;

			Assert.IsTrue(convexPolys.All(p => p.IsConvex));

			Assert.AreEqual(3, convexPolys.Count());
		}
	}
}
