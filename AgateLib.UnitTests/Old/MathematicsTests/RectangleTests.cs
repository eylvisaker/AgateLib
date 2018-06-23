using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.MathematicsTests
{
	[TestClass]
	public class RectangleTests
	{
		[TestMethod]
		public void Rect_ContractByInt()
		{
			var rect = new Rectangle(5, 10, 15, 20);

			var result = rect.Contract(1);

			Assert.AreEqual(new Rectangle(6, 11, 13, 18), result);
		}

		[TestMethod]
		public void Rect_ContractBySize()
		{
			var rect = new Rectangle(5, 10, 15, 20);

			var result = rect.Contract(new Size(1, 2));

			Assert.AreEqual(new Rectangle(6, 12, 13, 16), result);
		}

		[TestMethod]
		public void Rect_MultiUnion()
		{
			var result = Rectangle.Union(
				new Rectangle(1, 2, 3, 4),
				new Rectangle(5, 6, 7, 8),
				new Rectangle(9, 10, 11, 12),
				new Rectangle(2, 2, 2, 2));

			Assert.AreEqual(new Rectangle(1, 2, 19, 20), result);
		}
	}
}
