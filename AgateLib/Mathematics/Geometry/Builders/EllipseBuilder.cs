//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

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
		/// for coordinate systems where integers map directly to pixels, however if you are using a
		/// different coordinate system you may need to adjust this value.
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
		public Polygon BuildEllipse(Microsoft.Xna.Framework.Vector2 center, double majorAxisRadius, double minorAxisRadius, double rotationAngle)
		{
			double h = Math.Pow(majorAxisRadius - minorAxisRadius, 2) / Math.Pow(majorAxisRadius + minorAxisRadius, 2);

			// Ramanujan's second approximation to the circumference of an ellipse.
			double circumference =
				Math.PI * (majorAxisRadius + minorAxisRadius) * (1 + 3 * h / (10 + Math.Sqrt(4 - 3 * h)));

			// we will take the circumference as being the number of points to draw
			// on the ellipse.
			int ptCount = (int)Math.Ceiling(circumference * PointCountFactor);

			List<Microsoft.Xna.Framework.Vector2> result = new List<Microsoft.Xna.Framework.Vector2>();

			double step = 2 * Math.PI / (ptCount - 1);

			for (int i = 0; i < ptCount; i++)
			{
				var angle = step * i + rotationAngle;

				result.Add(new Microsoft.Xna.Framework.Vector2(
					(float)(center.X + majorAxisRadius * Math.Cos(angle)),
					(float)(center.Y + minorAxisRadius * (float)Math.Sin(angle))));
			}

			return new Polygon(result);
		}

		/// <summary>
		/// Builds a point list for an ellipse.
		/// </summary>
		/// <param name="rect">The rectangle in which the ellipse should be inscribed.</param>
		public Polygon BuildEllipse(Rectangle rect) => BuildEllipse((RectangleF)rect);

		/// <summary>
		/// Builds a point list for an ellipse.
		/// </summary>
		/// <param name="rect">The rectangle in which the ellipse should be inscribed.</param>
		public Polygon BuildEllipse(RectangleF rect)
		{
			Microsoft.Xna.Framework.Vector2 center = new Microsoft.Xna.Framework.Vector2(rect.Left + rect.Width / 2,
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
		public Polygon BuildCircle(Microsoft.Xna.Framework.Vector2 center, double radius) => BuildEllipse(center, radius, radius, 0);
	}
}
