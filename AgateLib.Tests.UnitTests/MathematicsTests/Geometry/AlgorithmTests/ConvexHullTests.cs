using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Algorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.MathematicsTests.Geometry.AlgorithmTests
{
	[TestClass]
	public class ConvexHullTests
	{
		private Polygon Diamond { get; } = new Polygon
			{
				{ 1, 0 },
				{ 0, 1 },
				{ -1, 0 },
				{ 0, -1 },
			};

		private Polygon TetrisL { get; } = new Polygon
			{
				Vector2.Zero,
				{ 2, 0 },
				{ 2, 1 },
				{ 1, 1 },
				{ 1, 3 },
				{ 0, 3 },
			};

		[TestMethod]
		public void ConvexHull_ConvexShape()
		{
			var result = new QuickHull().FindConvexHull(Diamond);

			VerifyContainment(result, Diamond);
		}

		[TestMethod]
		public void ConvexHull_OfConcaveShape()
		{
			var result = new QuickHull().FindConvexHull(TetrisL);

			Assert.IsTrue(result.IsConvex, "Convex hull isn't convex!");

			VerifyContainment(result, TetrisL);
		}

		private void VerifyContainment(Polygon result, Polygon initial)
		{
			foreach (var pt in initial)
			{
				Assert.IsTrue(result.AreaContains(pt),
					$"Convex hull does not contain point {pt}");
			}
		}

	}
}
