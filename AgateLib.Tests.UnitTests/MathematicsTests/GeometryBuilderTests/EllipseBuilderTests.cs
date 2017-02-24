using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.MathematicsTests.GeometryBuilderTests
{
	[TestClass]
	public class EllipseBuilderTests
	{
		[TestMethod]
		public void GB_BuildEllipseAxisAligned()
		{
			var builder = new EllipseBuilder();

			const int majorAxis = 8;
			const int minorAxis = 4;

			var points = builder.BuildEllipse(Vector2.Zero, majorAxis, minorAxis, 0);

			foreach (var point in points)
			{
				// Equation for an ellipse:
				// (x/a)^2 + (y/b)^2 = 1
				var norm = Vector2.UnitX * point.X / majorAxis + Vector2.UnitY * point.Y / minorAxis;

				Assert.AreEqual(1, norm.MagnitudeSquared, 0.00001,
					$"Point {point} does not satisfy the ellipse equation.");
			}
		}

		[TestMethod]
		public void GB_BuildEllipseBoundingBox()
		{
			var builder = new EllipseBuilder();

			const int majorAxis = 8;
			const int minorAxis = 4;

			Rectangle boundingBox = new Rectangle(-majorAxis, -minorAxis, majorAxis * 2, minorAxis * 2);

			var points = builder.BuildEllipse((RectangleF) boundingBox);

			foreach (var point in points)
			{
				// Equation for an ellipse:
				// (x/a)^2 + (y/b)^2 = 1
				var norm = Vector2.UnitX * point.X / majorAxis + Vector2.UnitY * point.Y / minorAxis;

				Assert.AreEqual(1, norm.MagnitudeSquared, 0.00001,
					$"Point {point} does not satisfy the ellipse equation.");
			}
		}

		[TestMethod]
		public void GB_BuildCircle()
		{
			var builder = new EllipseBuilder();

			const int radius = 4;

			var points = builder.BuildCircle(Vector2.Zero, radius);

			foreach (var point in points)
			{
				Assert.AreEqual(radius, point.Magnitude, 0.00001,
					$"Point {point} should be at radius {radius} but instead was at {point.Magnitude}");
			}
		}
	}
}
