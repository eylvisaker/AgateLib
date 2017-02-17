using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Mathematics.Geometry.Builders
{
	class EllipseBuilder
	{
		/// <summary>
		/// Builds a point list for an ellipse.
		/// </summary>
		/// <param name="center">The center point for the ellipse</param>
		/// <param name="majorAxisRadius">The major axis length for the ellipse.</param>
		/// <param name="minorAxisRadius">The minor axis length for the ellipse.</param>
		/// <param name="rotationAngle">The rotation angle in radians. If this
		/// is zero, the major axis is aligned with the X axis and the minor axis
		/// is aligned with the Y axis.</param>
		/// <returns></returns>
		public IEnumerable<Vector2> Build(Vector2 center, double majorAxisRadius, double minorAxisRadius, double rotationAngle)
		{
			double h = Math.Pow(majorAxisRadius - minorAxisRadius, 2) / Math.Pow(majorAxisRadius + minorAxisRadius, 2);

			//Ramanujan's second approximation to the circumference of an ellipse.
			double circumference =
				Math.PI * (majorAxisRadius + minorAxisRadius) * (1 + 3 * h / (10 + Math.Sqrt(4 - 3 * h)));

			// we will take the circumference as being the number of points to draw
			// on the ellipse.
			int ptCount = (int)Math.Ceiling(circumference * 2);

			List<Vector2> result = new List<Vector2>();

			double step = 2 * Math.PI / (ptCount - 1);

			for (int i = 0; i < ptCount; i++)
			{
				var angle = step * i + rotationAngle;

				result.Add(new Vector2(
					center.X + majorAxisRadius * Math.Cos(angle) + 0.5,
					center.Y + minorAxisRadius * Math.Sin(angle) + 0.5));
			}

			return result;
		}

		/// <summary>
		/// Builds a point list for an ellipse.
		/// </summary>
		/// <param name="rect">The rectangle in which the ellipse should be inscribed.</param>
		public IEnumerable<Vector2> Build(RectangleF rect)
		{
			Vector2 center = new Vector2(rect.Left + rect.Width / 2,
				rect.Top + rect.Height / 2);

			double radiusX = rect.Width / 2;
			double radiusY = rect.Height / 2;

			return Build(center, radiusX, radiusY, 0);
		}
	}
}
