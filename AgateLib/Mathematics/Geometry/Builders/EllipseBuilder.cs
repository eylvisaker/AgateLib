﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Mathematics.Geometry.Builders
{
	/// <summary>
	/// Class which builds ellipse shapes.
	/// </summary>
	public class EllipseBuilder
	{
		/// <summary>
		/// Scale factor that determines the number of points in the ellipse. This value is multiplied
		/// by the ellipse's circumference to get the number of points. The default should work fine
		/// for coordinate systems where 1, 2, 3 map to adjacent pixels, however if you are using a
		/// different coordinate system you may need to increase this value.
		/// </summary>
		public double PointCountFactor { get; set; } = 1;

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
		public IEnumerable<Vector2> BuildEllipse(Vector2 center, double majorAxisRadius, double minorAxisRadius, double rotationAngle)
		{
			double h = Math.Pow(majorAxisRadius - minorAxisRadius, 2) / Math.Pow(majorAxisRadius + minorAxisRadius, 2);

			// Ramanujan's second approximation to the circumference of an ellipse.
			double circumference =
				Math.PI * (majorAxisRadius + minorAxisRadius) * (1 + 3 * h / (10 + Math.Sqrt(4 - 3 * h)));

			// we will take the circumference as being the number of points to draw
			// on the ellipse.
			int ptCount = (int)Math.Ceiling(circumference * PointCountFactor);

			List<Vector2> result = new List<Vector2>();

			double step = 2 * Math.PI / (ptCount - 1);

			for (int i = 0; i < ptCount; i++)
			{
				var angle = step * i + rotationAngle;

				result.Add(new Vector2(
					center.X + majorAxisRadius * Math.Cos(angle),
					center.Y + minorAxisRadius * Math.Sin(angle)));
			}

			return result;
		}

		/// <summary>
		/// Builds a point list for an ellipse.
		/// </summary>
		/// <param name="rect">The rectangle in which the ellipse should be inscribed.</param>
		public IEnumerable<Vector2> BuildEllipse(RectangleF rect)
		{
			Vector2 center = new Vector2(rect.Left + rect.Width / 2,
				rect.Top + rect.Height / 2);

			double radiusX = rect.Width / 2;
			double radiusY = rect.Height / 2;

			return BuildEllipse(center, radiusX, radiusY, 0);
		}

		/// <summary>
		/// Builds a point list for a circle.
		/// </summary>
		/// <param name="center"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		public IEnumerable<Vector2> BuildCircle(Vector2 center, double radius) => BuildEllipse(center, radius, radius, 0);
	}
}
