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
	public class SimplePolygonDetectionTests
	{
		[TestMethod]
		public void Polygon_RectangleIsSimple()
		{
			Assert.IsTrue(new Rectangle(1, 2, 3, 4).ToPolygon().IsSimple);
		}

		[TestMethod]
		public void Polygon_ZeroPolygonIsSimple()
		{
			var poly = new Rectangle().ToPolygon();

			Assert.AreEqual(4, poly.Count);
			Assert.IsTrue(new Rectangle().ToPolygon().IsSimple);
		}

		[TestMethod]
		public void Polygon_WithHoleIsComplex()
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
				{0, 0},
				{3, 0},
				{3, 2},
				{1, 2},
				{1, 1},
				{2, 1},
				{2, 3},
				{0, 3}
			});

			Assert.IsTrue(p.IsComplex);
		}

		[TestMethod]
		public void Polygon_WithCrossingIsComplex()
		{
			// This polygon looks like this:
			//    ┌┐
			//    └┼┐
			//     └┘	
			// where the two middle line segments are long and cross each other
			var p = new Polygon(new Vector2List
			{
				{0, 0},
				{1, 0},
				{1, 1},
				{-1, 1},
				{-1, 2},
				{0, 3},
			});

			Assert.IsTrue(p.IsComplex);
		}

		[TestMethod]
		public void Polygon_DiagonalLinesIsComplex()
		{
			// Same as PolygonWithCrossingIsComplex, but the upper-left and lower-right lines are diagonal.
			var p = new Polygon(new Vector2List
			{
				{0, 0},
				{1, 1},
				{-1, 1},
				{0, 3},
			});

			Assert.IsTrue(p.IsComplex);
		}

		[TestMethod]
		public void Polygon_ComplexBroken()
		{
			var p = new Polygon
			{
				{40, 0},
				{2.5711333389961, 1.86803771595309},
				{12.3606797749979, 38.0422606518061},
				{-0.982085545888505, 3.02254851667882},
				{-32.3606797749979, 23.5114100916989},
				{-3.17809558621519, 3.89191604793398E-16},
				{-32.3606797749979, -23.5114100916989},
				{-0.982085545888505, -3.02254851667882},
				{12.3606797749979, -38.0422606518061},
				{2.5711333389961, -1.86803771595309},
			};

			Assert.IsTrue(p.IsSimple);
		}
	}
}
