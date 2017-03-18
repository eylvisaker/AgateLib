using System;
using System.Collections.Generic;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Quality;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Interface for the primitive renderer. This can draw lines and shapes.
	/// </summary>
	public interface IPrimitiveRenderer
	{
		/// <summary>
		/// Draws a set of lines. The lineType parameter controls how
		/// lines are connected.
		/// </summary>
		/// <param name="lineType">The type of lines to draw.</param>
		/// <param name="color">The color of lines to draw.</param>
		/// <param name="points">The points that are used to 
		/// build the individual line segments.</param>
		void DrawLines(LineType lineType, Color color, IEnumerable<Vector2f> points);

		/// <summary>
		/// Draws a filled convex polygon. 
		/// </summary>
		/// <remarks>The point list passed in is assumed to be
		/// convex - if it is not the polygon won't be drawn correctly.</remarks>
		/// <param name="color"></param>
		/// <param name="points"></param>
		void FillConvexPolygon(Color color, IEnumerable<Vector2f> points);
	}
}