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
		private Polygon Diamond { get; } = new Polygon(new[]
			{
				new Vector2(1, 0),
				new Vector2(0, 1),
				new Vector2(-1, 0),
				new Vector2(0, -1),
			});

		private Polygon OddConcave { get; } = new Polygon(new[]
			{
				Vector2.Zero,
				new Vector2(1, 0),
				new Vector2(1.5, 0.5),
				new Vector2(2, 0),
				new Vector2(2, 2),
			});

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

			Assert.IsTrue(Translate(Diamond, new Vector2(100, 15)).Contains(new Vector2(100, 15)));
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

		private Polygon Translate(Polygon polygon, Vector2 amount)
		{
			return new Polygon(polygon.Points.Select(x => amount + x));
		}
	}
}
