using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.MathematicsTests
{
	[TestClass]
	public class SizeTests
	{
		[TestMethod]
		public void Size_ToVector2()
		{
			var result = Vector2.UnitX + (Vector2)new Size(10, 12);

			Assert.AreEqual(new Vector2(11, 12), result);
		}

		[TestMethod]
		public void Size_ToVector2f()
		{
			var result = Vector2f.UnitX + (Vector2f)new Size(10, 12);

			Assert.AreEqual(new Vector2f(11, 12), result);
		}
	}
}
