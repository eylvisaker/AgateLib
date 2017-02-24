using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.MathematicsTests
{
	[TestClass]
	public class Vector2Tests
	{
		[TestMethod]
		public void V2_DotProductOrtho()
		{
			Vector2 v1 = Vector2.UnitX;
			Vector2 v2 = Vector2.UnitY * 4;

			Assert.AreEqual(0, v1.DotProduct(v2));
		}

		[TestMethod]
		public void V2_Magnitude()
		{
			Vector2 v = new Vector2(3, 4);

			Assert.AreEqual(5, v.Magnitude, 0.00001);
		}

		[TestMethod]
		public void V2_Equals()
		{
			Vector2 a = Vector2.UnitX;
			Vector2 b = Vector2.UnitX * 1.0000001;

			Assert.IsTrue(a.Equals(b, 0.00001));
		}

		[TestMethod]
		public void V2_FromPolar()
		{
			Vector2 a = Vector2.FromPolar(1, Math.PI * 0.5);
			Vector2 b = Vector2.FromPolar(1, Math.PI * 1.0);
			Vector2 c = Vector2.FromPolar(1, Math.PI * 1.5);
			Vector2 d = Vector2.FromPolar(1, Math.PI * 2.0);

			Assert.IsTrue(a.Equals(Vector2.UnitY, .000001));
			Assert.IsTrue(b.Equals(-Vector2.UnitX, .000001));
			Assert.IsTrue(c.Equals(-Vector2.UnitY, .000001));
			Assert.IsTrue(d.Equals(Vector2.UnitX, .000001));
		}

	}
}
