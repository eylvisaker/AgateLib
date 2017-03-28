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