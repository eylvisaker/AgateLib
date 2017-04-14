using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.MathematicsTests.Geometry
{
	[TestClass]
	public class PolygonEdgeTests: PolygonUnitTest
	{
		[TestMethod]
		public void Edge_SquareNormals()
		{
			var square = new Rectangle(-1, -1, 2, 2).ToPolygon();

			// A square of size 2 centered at the origin will have edge normals equal to the center point
			// for each edge.
			// Example: the side of the square with endpoints (1, 1), (1, -1) has center point (1, 0) and normal (1, 0)
			int count = 0;

			foreach (var edge in square.Edges)
			{
				Vector2 centerPoint = edge.LineSegment.Center;
				Assert.AreEqual(centerPoint, edge.Normal);

				count++;
			}

			Assert.AreEqual(4, count);
		}

		[TestMethod]
		public void Edge_SquareNormalsReverseWinding()
		{
			var square = new Polygon(new Rectangle(-1, -1, 2, 2).ToPolygon().Reverse());

			// A square of size 2 centered at the origin will have edge normals equal to the center point
			// for each edge.
			// Example: the side of the square with endpoints (1, 1), (1, -1) has center point (1, 0) and normal (1, 0)
			int count = 0;

			foreach (var edge in square.Edges)
			{
				Vector2 centerPoint = edge.LineSegment.Center;
				Assert.AreEqual(centerPoint, edge.Normal);

				count++;
			}

			Assert.AreEqual(4, count);
		}
	}
}
