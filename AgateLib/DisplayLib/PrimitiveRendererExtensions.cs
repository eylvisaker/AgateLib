//
//    Copyright (c) 2006-2017 Erik Ylvisaker
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Builders;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Extensions for IPrimitiveRenderer
	/// </summary>
	public static class PrimitiveRendererExtensions
	{
		/// <summary>
		/// Draws a set of lines. The lineType parameter controls how
		/// lines are connected.
		/// </summary>
		/// <param name="lineType">The type of lines to draw.</param>
		/// <param name="color">The color of lines to draw.</param>
		/// <param name="points">The points that are used to 
		/// build the individual line segments.</param>
		public static void DrawLines(this IPrimitiveRenderer primitives, LineType lineType, Color color,
			IEnumerable<Vector2> points)
		{
			primitives.DrawLines(lineType, color, points.Select(x => (Vector2f)x));
		}

		/// <summary>
		/// Draws a filled convex polygon.
		/// </summary>
		/// <param name="color"></param>
		/// <param name="points"></param>
		public static void FillPolygon(this IPrimitiveRenderer primitives, Color color, IEnumerable<Vector2> points)
		{
			primitives.FillConvexPolygon(color, points.Select(x => (Vector2f)x));
		}

		/// <summary>
		///     Draws a line between the two points specified.
		/// </summary>
		/// <param name="primitives"></param>
		/// <param name="color"></param>
		/// <param name="a"></param>
		/// <param name="b"></param>
		public static void DrawLine(this IPrimitiveRenderer primitives, Color color, Vector2f a, Vector2f b)
		{
			primitives.DrawLines(LineType.LineSegments, color,
				new[] { a, b });
		}

		/// <summary>
		///     Draws a line between the two points specified.
		/// </summary>
		/// <param name="primitives"></param>
		/// <param name="color"></param>
		/// <param name="a"></param>
		/// <param name="b"></param>
		public static void DrawLine(this IPrimitiveRenderer primitives, Color color, Vector2 a, Vector2 b)
		{
			primitives.DrawLines(LineType.LineSegments, color,
				new[] { (Vector2f)a, (Vector2f)b });
		}

		/// <summary>
		/// Draws the outline of a rectangle.
		/// </summary>
		/// <param name="primitives"></param>
		/// <param name="color"></param>
		/// <param name="rect"></param>
		public static void DrawRect(this IPrimitiveRenderer primitives, Color color, Rectangle rect)
		{
			DrawRect(primitives, color, (RectangleF)rect);
		}

		/// <summary>
		/// Draws the outline of a rectangle.
		/// </summary>
		/// <param name="primitives"></param>
		/// <param name="color"></param>
		/// <param name="rect"></param>
		public static void DrawRect(this IPrimitiveRenderer primitives, Color color, RectangleF rect)
		{
			primitives.DrawLines(LineType.Polygon, color,
				new QuadrilateralBuilder().BuildRectangle(rect));
		}

		/// <summary>
		/// Draws the outline of an ellipse, inscribed inside a rectangle.
		/// </summary>
		/// <param name="primitives"></param>
		/// <param name="color"></param>
		/// <param name="boundingRect">The rectangle the circle should be inscribed in.</param>
		public static void DrawEllipse(this IPrimitiveRenderer primitives, Color color, Rectangle boundingRect)
		{
			DrawEllipse(primitives, color, (RectangleF)boundingRect);
		}

		/// <summary>
		/// Draws the outline of an ellipse, inscribed inside a rectangle.
		/// </summary>
		/// <param name="primitives"></param>
		/// <param name="color"></param>
		/// <param name="boundingRect">The rectangle the circle should be inscribed in.</param>
		public static void DrawEllipse(this IPrimitiveRenderer primitives, Color color, RectangleF boundingRect)
		{
			primitives.DrawLines(LineType.Polygon, color,
				new EllipseBuilder().BuildEllipse(boundingRect));
		}

		/// <summary>
		/// Draws an unfilled polygon.
		/// </summary>
		/// <param name="primitives"></param>
		/// <param name="color"></param>
		/// <param name="polygon"></param>
		public static void DrawPolygon(this IPrimitiveRenderer primitives, Color color, Polygon polygon)
		{
			primitives.DrawLines(LineType.Polygon, color, polygon.Points);
		}

		/// <summary>
		/// Draws a filled polygon.
		/// </summary>
		/// <param name="primitives"></param>
		/// <param name="color"></param>
		/// <param name="polygon"></param>
		public static void FillPolygon(this IPrimitiveRenderer primitives, Color color, Polygon polygon)
		{
			if (polygon.IsConvex)
			{
				primitives.FillPolygon(color, polygon.Points);
			}
			else
			{
				foreach (var convexPoly in polygon.ConvexDecomposition)
				{
					primitives.FillPolygon(color, convexPoly.Points);
				}
			}
		}

		/// <summary>
		/// Draws a filled rectangle.
		/// </summary>
		/// <param name="primitives"></param>
		/// <param name="color"></param>
		/// <param name="rect"></param>
		public static void FillRect(this IPrimitiveRenderer primitives, Color color, Rectangle rect)
		{
			FillRect(primitives, color, (RectangleF)rect);
		}

		/// <summary>
		/// Draws a filled rectangle.
		/// </summary>
		/// <param name="primitives"></param>
		/// <param name="color"></param>
		/// <param name="rect"></param>
		public static void FillRect(this IPrimitiveRenderer primitives, Color color, RectangleF rect)
		{
			primitives.FillPolygon(color, new QuadrilateralBuilder().BuildRectangle(rect));
		}

		/// <summary>
		/// Draws a filled ellipse, inscribed inside a rectangle.
		/// </summary>
		/// <param name="primitives"></param>
		/// <param name="color"></param>
		/// <param name="boundingRect"></param>
		public static void FillEllipse(this IPrimitiveRenderer primitives, Color color, Rectangle boundingRect)
		{
			FillEllipse(primitives, color, (RectangleF)boundingRect);
		}

		/// <summary>
		/// Draws a filled ellipse, inscribed inside a rectangle.
		/// </summary>
		/// <param name="primitives"></param>
		/// <param name="color"></param>
		/// <param name="boundingRect"></param>
		public static void FillEllipse(this IPrimitiveRenderer primitives, Color color, RectangleF boundingRect)
		{
			primitives.FillPolygon(color, new EllipseBuilder().BuildEllipse(boundingRect).ToArray());
		}

		/// <summary>
		/// Draws a filled ellipse.
		/// </summary>
		/// <param name="primitives"></param>
		/// <param name="color"></param>
		/// <param name="center"></param>
		/// <param name="majorAxisRadius"></param>
		/// <param name="minorAxisRadius"></param>
		/// <param name="rotationAngle"></param>
		public static void FillEllipse(this IPrimitiveRenderer primitives, Color color, Vector2 center,
			double majorAxisRadius, double minorAxisRadius, double rotationAngle)
		{
			primitives.FillPolygon(color, new EllipseBuilder().BuildEllipse(center, majorAxisRadius, minorAxisRadius, rotationAngle).ToArray());
		}

		/// <summary>
		/// Draws a filled circle.
		/// </summary>
		/// <param name="primitives"></param>
		/// <param name="color"></param>
		/// <param name="center"></param>
		/// <param name="radius"></param>
		public static void FillCircle(this IPrimitiveRenderer primitives, Color color, Vector2 center, double radius)
		{
			primitives.FillPolygon(color, new EllipseBuilder().BuildCircle(center, radius).ToArray());
		}
	}
}
