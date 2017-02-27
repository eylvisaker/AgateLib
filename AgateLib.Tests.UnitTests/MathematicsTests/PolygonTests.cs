using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YamlDotNet.Serialization;

namespace AgateLib.UnitTests.MathematicsTests
{
	[TestClass]
	public class PolygonTests
	{
		private Polygon Diamond { get; } = new Polygon
			{
				{ 1, 0 },
				{ 0, 1 },
				{ -1, 0 },
				{ 0, -1 },
			};

		private Polygon OddConcave { get; } = new Polygon
			{
				Vector2.Zero,
				{ 1, 0 },
				{ 1.5, 0.5 },
				{ 2, 0 },
				{ 2, 2 },
			};

		[TestMethod]
		public void Poly_BoundingRect()
		{
			Assert.AreEqual(Rectangle.FromLTRB(-1, -1, 1, 1), Diamond.BoundingRect);
		}

		[TestMethod]
		public void Poly_IsConvex()
		{
			Assert.IsTrue(Diamond.IsConvex, "Diamond should report convexity.");
			Assert.IsTrue(OddConcave.IsConcave, "Odd concave shape should report concavity.");
		}

		[TestMethod]
		public void Poly_PointOnEdgeIsInside()
		{
			Assert.IsTrue(Diamond.Contains(new Vector2(0.5, 0.5)));
		}

		[TestMethod]
		public void Poly_DiamondContainsPoint()
		{
			Assert.IsTrue(Diamond.Contains(new Vector2(0, 0)));

			Assert.IsTrue(Diamond.Translate(new Vector2(100, 15)).Contains(new Vector2(100, 15)));
		}

		[TestMethod]
		public void Poly_OddConcaveContainsPoint()
		{
			Assert.IsFalse(OddConcave.Contains(new Vector2(1.5, 0)));
		}

		[TestMethod]
		public void Poly_SerializationRoundTrip()
		{
			var serializer = new SerializerBuilder()
				.WithTypeConvertersForAgateLibMathematics()
				.Build();

			var deserializer = new DeserializerBuilder()
				.WithTypeConvertersForAgateLibMathematics()
				.Build();

			var result = deserializer.Deserialize<Polygon>(serializer.Serialize(Diamond));

			Assert.AreEqual(Diamond.Points.Count, result.Points.Count);

			for (var index = 0; index < Diamond.Points.Count; index++)
			{
				Assert.AreEqual(Diamond.Points[index], result.Points[index]);
			}
		}

		[TestMethod]
		public void Poly_Rotate()
		{
			var a = Rectangle.FromLTRB(0, 0, 10, 5).ToPolygon();
			var b = a.RotateDegrees(90, new Vector2(10, 5));

			Assert.AreEqual(4, b.Count);

			Assert.IsTrue(b.Any(x => x.Equals(new Vector2(5, 5), 1e-8)));
			Assert.IsTrue(b.Any(x => x.Equals(new Vector2(10, 5), 1e-8)));
			Assert.IsTrue(b.Any(x => x.Equals(new Vector2(10, 15), 1e-8)));
			Assert.IsTrue(b.Any(x => x.Equals(new Vector2(5, 15), 1e-8)));
		}
	}
}
