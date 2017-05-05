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
		public void V2_Parse()
		{
			Vector2 result = Vector2.Parse("3.5,-4.2");

			Assert.AreEqual(3.5, result.X, 1e-8);
			Assert.AreEqual(-4.2, result.Y, 1e-8);
		}

		[TestMethod]
		public void V2_ParseScientificNotation()
		{
			Vector2 result = Vector2.Parse("-1.2e+12,2.4e-18");

			Assert.AreEqual(-1.2e+12, result.X, 1e4);
			Assert.AreEqual(2.4e-18, result.Y, 1e-26);
		}

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
		public void V2_Rotate()
		{
			Vector2 a = new Vector2(3, 4);
			Vector2 b = a.RotateDegrees(90);
			var expected = new Vector2(4, -3);

			Assert.IsTrue(b.Equals(expected, 1e-8),
				$"Expected {expected} but got {b}");
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

		[TestMethod]
		public void V2_BulkAdd()
		{
			var array = new[] { new Vector2(1, 2), new Vector2(2, 3), new Vector2(3, 4), };
			var offset = new Vector2(10, 12);

			var result = (array + offset).ToList();

			Assert.AreEqual(new Vector2(11, 14), result[0]);
			Assert.AreEqual(new Vector2(12, 15), result[1]);
			Assert.AreEqual(new Vector2(13, 16), result[2]);
		}

		[TestMethod]
		public void V2_BulkSubtract()
		{
			var array = new[] { new Vector2(1, 2), new Vector2(2, 3), new Vector2(3, 4), };
			var offset = new Vector2(10, 12);

			var result = (array - offset).ToList();

			Assert.AreEqual(new Vector2(-9, -10), result[0]);
			Assert.AreEqual(new Vector2(-8, -9), result[1]);
			Assert.AreEqual(new Vector2(-7, -8), result[2]);

			result = (offset - array).ToList();

			Assert.AreEqual(new Vector2(9, 10), result[0]);
			Assert.AreEqual(new Vector2(8, 9), result[1]);
			Assert.AreEqual(new Vector2(7, 8), result[2]);
		}

		[TestMethod]
		public void V2_ProjectionContract()
		{
			ProjectionTest(new Vector2(1, 0), new Vector2(1, 1), new Vector2(0.5, 0.5));

			for (int i = 0; i < 360; i++)
			{
				const double radius = 2;

				Vector2 v = Vector2.FromPolarDegrees(radius, i);
				Vector2 direction = new Vector2(1, 1);
				Vector2 expected = radius * Math.Cos((i - 45) * Math.PI / 180) * direction.Normalize();

				ProjectionTest(v, direction, expected);
			}
		}

		private static void ProjectionTest(Vector2 v, Vector2 direction, Vector2 expected)
		{
			Vector2 result = v.ProjectionOn(direction);

			var perp = v - result;

			Assert.AreEqual(0, direction.DotProduct(perp), 1e-8);

			Assert.IsTrue(result.Equals(expected, 1e-8),
				$"Projection of {v} on {direction} should yield {expected} but got {result} instead.");
		}
	}
}
