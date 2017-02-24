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
	public class Vector2fTests
	{
		[TestMethod]
		public void V2f_DotProductOrtho()
		{
			Vector2f v1 = Vector2f.UnitX;
			Vector2f v2 = Vector2f.UnitY * 4;

			Assert.AreEqual(0, v1.DotProduct(v2));
		}

		[TestMethod]
		public void V2f_Magnitude()
		{
			Vector2f v = new Vector2f(3, 4);

			Assert.AreEqual(5, v.Magnitude, 0.00001);
		}

		[TestMethod]
		public void V2f_Equals()
		{
			Vector2f a = Vector2f.UnitX;
			Vector2f b = Vector2f.UnitX * 1.0000001;

			Assert.IsTrue(a.Equals(b, 0.00001));
		}

		[TestMethod]
		public void V2f_FromPolar()
		{
			Vector2f a = Vector2f.FromPolar(1, Math.PI * 0.5);
			Vector2f b = Vector2f.FromPolar(1, Math.PI * 1.0);
			Vector2f c = Vector2f.FromPolar(1, Math.PI * 1.5);
			Vector2f d = Vector2f.FromPolar(1, Math.PI * 2.0);

			Assert.IsTrue(a.Equals(Vector2f.UnitY, .000001));
			Assert.IsTrue(b.Equals(-Vector2f.UnitX, .000001));
			Assert.IsTrue(c.Equals(-Vector2f.UnitY, .000001));
			Assert.IsTrue(d.Equals(Vector2f.UnitX, .000001));
		}

		[TestMethod]
		public void V2f_AngleBetween()
		{
			Vector2f a = 2 * Vector2f.UnitX;
			Vector2f b = 2 * Vector2f.UnitY;

			Assert.AreEqual(Math.PI * 0.5, Vector2f.AngleBetween(a, b), 0.00001);
		}
	}
}
