using AgateLib.Mathematics.Geometry.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Mathematics.Geometry.Builders
{
	/// <summary>
	/// Builds a pointed star
	/// </summary>
	public class StarBuilder
	{
		/// <summary>
		/// Builds a pointed star.
		/// </summary>
		/// <param name="pointCount">Number of points in the star.</param>
		/// <param name="size">The size of the star.</param>
		/// <param name="innerSize">The inner size of the star. It is recommended that this be between 0.2 and 0.7 times the value of size.</param>
		/// <param name="location"></param>
		/// <param name="angle">The rotation angle for the star.</param>
		/// <returns></returns>
		public Polygon BuildStar(int pointCount, double size, double innerSize, Vector2 location = new Vector2(), double angle = 0)
		{
			var outerpoints = RegularShape(angle, pointCount, size);
			var innerPoints = RegularShape(Math.PI / pointCount + angle, pointCount, innerSize);

			var result = new Polygon();

			for (int i = 0; i < outerpoints.Count; i++)
			{
				result.Add(outerpoints[i]);
				result.Add(innerPoints[i]);
			}

			return result.Translate(location);
		}

		private static Polygon RegularShape(double angle, int pointCount, double size)
		{
			double step = 2 * Math.PI / pointCount;

			Polygon result = new Polygon();

			for (int i = 0; i < pointCount; i++)
			{
				result.Add(Vector2.FromPolar(size, angle));
				angle += step;
			}

			return result;
		}
	}
}
