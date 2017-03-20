using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.MathematicsTests.AlgorithmTests
{
	[TestClass]
	public class ComplexPolygonDetectionTests
	{
		[TestMethod]
		public void RectangleIsSimple()
		{
			Assert.IsTrue(new Rectangle(1, 2, 3, 4).ToPolygon().IsSimple);
		}

		[TestMethod]
		public void PolygonWithHoleIsComplex()
		{
			// This polygon looks like this:
			//   ┌─┐
			//   │┌┼┐
			//   │└┘│
			//   └──┘
			// where the crossing at (2, 2) happens because 
			// the line segments go from (1, 2) - (3, 2) and (2, 1) - (2, 3)
			var p = new Polygon(new Vector2List
			{
				{ 0, 0 },
				{ 3, 0 },
				{ 3, 2 },
				{ 1, 2 },
				{ 1, 1 },
				{ 2, 1 },
				{ 2, 3 },
				{ 0, 3 }
			});

			Assert.IsTrue(p.IsComplex);
		}

		[TestMethod]
		public void PolygonWithCrossingIsComplex()
		{
			// This polygon looks like this:
			//    ┌┐
			//    └┼┐
			//     └┘	
			// where the two middle line segments are long and cross each other
			var p = new Polygon(new Vector2List
			{
				{ 0, 0 },
				{ 1, 0 },
				{ 1, 1 },
				{ -1, 1 },
				{ -1, 2 },
				{ 0, 3 },
			});

			Assert.IsTrue(p.IsComplex);
		}

		[TestMethod]
		public void PolygonDiagnolLinesIsComplex()
		{
			// Same as PolygonWithCrossingIsComplex, but the upper-left and lower-right lines are diagonal.
			var p = new Polygon(new Vector2List
			{
				{ 0, 0 },
				{ 1, 1 },
				{ -1, 1 },
				{ 0, 3 },
			});

			Assert.IsTrue(p.IsComplex);
		}
	}
}
