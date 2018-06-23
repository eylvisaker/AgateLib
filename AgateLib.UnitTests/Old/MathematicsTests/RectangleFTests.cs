using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgateLib.Mathematics;

namespace AgateLib.UnitTests.MathematicsTests
{
	[TestClass]
	public class RectangleFTests
	{
		[TestMethod]
		public void RectF_ContractByFloat()
		{
			var rect = new RectangleF(5, 10, 15, 20);

			var result = rect.Contract(1);

			Assert.AreEqual(new RectangleF(6, 11, 13, 18), result);
		}

		[TestMethod]
		public void RectF_ContractBySize()
		{
			var rect = new RectangleF(5, 10, 15, 20);

			var result = rect.Contract(new SizeF(1, 2));

			Assert.AreEqual(new RectangleF(6, 12, 13, 16), result);
		}

		[TestMethod]
		public void RectF_ContainsVector()
		{
			var rect = new RectangleF(1, 2, 3, 4);

			Assert.IsTrue(rect.Contains(1, 2));
			Assert.IsFalse(rect.Contains(0.9f, 2));
			Assert.IsFalse(rect.Contains(1, 1.9f));
			Assert.IsFalse(rect.Contains(4.1f, 2));
			Assert.IsFalse(rect.Contains(1, 6.1f));
		}

		[TestMethod]
		public void RectF_Intersection()
		{
			var a = new RectangleF(1, 2, 10, 12);
			var b = new RectangleF(5, 7, 14, 32);

			Assert.AreEqual(RectangleF.FromLTRB(5, 7, 11, 14), RectangleF.Intersection(a, b));
		}

		[TestMethod]
		public void RectF_ToPolygon()
		{
			var rect = new RectangleF(1, 2, 3, 4);
			var poly = rect.ToPolygon();

			Assert.AreEqual(4, poly.Count);
			Assert.IsTrue(poly.Points.Contains(new Vector2(1, 2)));
			Assert.IsTrue(poly.Points.Contains(new Vector2(4, 2)));
			Assert.IsTrue(poly.Points.Contains(new Vector2(4, 6)));
			Assert.IsTrue(poly.Points.Contains(new Vector2(1, 6)));
		}
	}
}
