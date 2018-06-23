using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry.Algorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UnitTests.MathematicsTests.AlgorithmTests
{
	[TestClass]
	public class LineAlgorithmTests
	{
		[TestMethod]
		public void Line_SegmentIntersectionHighSymmetry()
		{
			var result = LineAlgorithms.LineSegmentIntersection(
				Vector2.UnitX, 
				-Vector2.UnitX,
				Vector2.UnitY, 
				-Vector2.UnitY);

			Assert.IsNotNull(result);
			Assert.AreEqual(0.5, result.U1);
			Assert.AreEqual(0.5, result.U2);
			Assert.AreEqual(Vector2.Zero, result.IntersectionPoint);
		}

		[TestMethod]
		public void Line_SegmentIntersectionLowSymmetry()
		{
			var result = LineAlgorithms.LineSegmentIntersection(
				new Vector2(704, 336), new Vector2(569, 299),
				new Vector2(436, 218), new Vector2(446, 357));

			Assert.IsNotNull(result);
			Assert.IsTrue(new Vector2(439.269, 263.444).Equals(result.IntersectionPoint, 1e-3),
				"Intersection was not in correct position.");
		}

		[TestMethod]
		public void Line_SegmentIntersectionParallel()
		{
			var result = LineAlgorithms.LineSegmentIntersection(
				Vector2.UnitX,
				-Vector2.UnitX,
				Vector2.UnitX + Vector2.UnitY, 
				-Vector2.UnitX + Vector2.UnitY);

			Assert.IsNull(result);
		}

		[TestMethod]
		public void Line_Side()
		{
			Assert.AreEqual(1, 
				LineAlgorithms.SideOf(Vector2.Zero, Vector2.UnitY, Vector2.UnitX));

			Assert.AreEqual(-Math.Sqrt(0.5),
				LineAlgorithms.SideOf(Vector2.Zero, Vector2.UnitX + Vector2.UnitY,
					Vector2.UnitY), 1e-8);
		}
	}
}
