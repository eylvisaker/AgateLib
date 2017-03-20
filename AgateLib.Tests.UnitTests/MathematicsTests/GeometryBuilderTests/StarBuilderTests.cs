using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UnitTests.MathematicsTests.GeometryBuilderTests
{
	[TestClass]
	public class StarBuilderTests
	{
		[TestMethod]
		public void StarBuild_FivePointedStarPoints()
		{
			var radii = new[] { 8, 4 };

			var star = new StarBuilder().BuildStar(5, radii[0], radii[1], Vector2.Zero, 0);

			Assert.AreEqual(10, star.Points.Count);

			int index = 0;
			double lastAngle = -1;

			foreach(var point in star.Points)
			{
				var radius = point.Magnitude;
				var angle = point.Angle;
				if (angle < 0) angle += Math.PI * 2;

				Assert.AreEqual(radii[index % 2], radius, 1e-8);
				Assert.IsTrue(angle > lastAngle, $"Expected new angle to be larger but instead lastAngle = {lastAngle} and angle = {angle}");

				lastAngle = angle;

				index++;
			}
		}

		[TestMethod]
		public void StarBuild_FivePointedStarCenter()
		{
			var center = new Vector2(10, 12);

			var star = new StarBuilder().BuildStar(5, 8, 4, center, 1);

			Assert.AreEqual(10, star.Points.Count);

			var avg = star.Points.Average();

			Assert.IsTrue(center.Equals(avg, 1e-6), $"Expected {center}, actual {avg}");
		}

		[TestMethod]
		public void StarBuild_SixPointedStarPoints()
		{
			var radii = new[] { 8, 4 };

			var star = new StarBuilder().BuildStar(6, radii[0], radii[1]);

			Assert.AreEqual(12, star.Points.Count);

			int index = 0;
			double lastAngle = -1;

			foreach (var point in star.Points)
			{
				var radius = point.Magnitude;
				var angle = point.Angle;
				if (angle < 0) angle += Math.PI * 2;

				Assert.AreEqual(radii[index % 2], radius, 1e-8);
				Assert.IsTrue(angle > lastAngle, $"Expected new angle to be larger but instead lastAngle = {lastAngle} and angle = {angle}");

				lastAngle = angle;

				index++;
			}
		}

		[TestMethod]
		public void StarBuild_SixPointedStarCenter()
		{
			var center = new Vector2(10, 12);

			var star = new StarBuilder().BuildStar(6, 8, 4, center, 1);

			Assert.AreEqual(12, star.Points.Count);

			var avg = star.Points.Average();

			Assert.IsTrue(center.Equals(avg, 1e-6), $"Expected {center}, actual {avg}");
		}

	}
}
