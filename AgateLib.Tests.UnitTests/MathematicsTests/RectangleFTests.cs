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
	public class RectangleFTests
	{
		[TestMethod]
		public void Rect_ContractByInt()
		{
			var rect = new RectangleF(5, 10, 15, 20);

			var result = rect.Contract(1);

			Assert.AreEqual(new RectangleF(6, 11, 13, 18), result);
		}

		[TestMethod]
		public void Rect_ContractBySize()
		{
			var rect = new RectangleF(5, 10, 15, 20);

			var result = rect.Contract(new SizeF(1, 2));

			Assert.AreEqual(new RectangleF(6, 12, 13, 16), result);
		}
	}
}
