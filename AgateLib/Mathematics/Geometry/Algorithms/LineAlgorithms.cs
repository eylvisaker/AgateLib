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

namespace AgateLib.Mathematics.Geometry.Algorithms
{
	/// <summary>
	/// Class which contains geometry algorithms for lines.
	/// </summary>
	public static class LineAlgorithms
	{
		public const double DefaultTolerance = 1e-8;

		/// <summary>
		/// Returns a value indicating whether the two lines defined by the line segments intersect.
		/// Returns null if the two lines are parallel.
		/// </summary>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <param name="tolerance"></param>
		/// <returns></returns>
		public static LineSegmentIntersection LineSegmentIntersection(LineSegment first, LineSegment second, double tolerance = DefaultTolerance)
		{
			return LineSegmentIntersection(first.Start, first.End, second.Start, second.End, tolerance);
		}

		/// <summary>
		/// Returns a value indicating whether lines defined by the segments R1a-R1b and R2a-R2b intersect.
		/// The result is null if the two lines are parallel.
		/// </summary>
		/// <remarks>
		/// This algorithm solves the equation for the intersection point x:
		///   x = R1a + u1 * (R1b - R1a)
		///   x = R2a + u2 * (R2b - R2a)
		/// 
		/// This results in the matrix equation:
		///   [ -l1_x  l2_x ] [ u1 ] = [ S_x ]
		///   [ -l1_y  l2_y ] [ u2 ] = [ S_y ]
		/// 
		/// where l1 is the vector pointing to R1b from R1a
		/// and l2 is the vector pointing to R2b from R1a
		/// and S is the vector pointing to R1a from R2a.
		/// </remarks>
		/// <param name="R1a">The start point of the first line segment.</param>
		/// <param name="R1b">The end point of the first line segment.</param>
		/// <param name="R2a">The start point of the second line segment.</param>
		/// <param name="R2b">The end point of the second line segment.</param>
		/// <returns></returns>
		public static LineSegmentIntersection LineSegmentIntersection(Vector2 R1a, Vector2 R1b, Vector2 R2a, Vector2 R2b,
			double tolerance = DefaultTolerance)
		{
			var line1 = R1b - R1a;
			var line2 = R2b - R2a;

			var determinant = line2.X * line1.Y - line1.X * line2.Y;

			if (Math.Abs(determinant) < tolerance)
				return null;

			var S = R1a - R2a;

			// Solution to the matrix equation is obtained by inverting the 2x2 matrix.
			var u1 = line2.Y * S.X - line2.X * S.Y;
			var u2 = line1.Y * S.X - line1.X * S.Y;

			u1 /= determinant;
			u2 /= determinant;

			return new LineSegmentIntersection(R1a + u1 * line1, u1, u2);
		}

		/// <summary>
		/// Returns true if the three points are collinear, up to a specified tolerance
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <returns></returns>
		public static bool AreCollinear(Vector2 a, Vector2 b, Vector2 c, double tolerance = 1e-6)
		{
			// algorithm from http://math.stackexchange.com/a/405981/212825
			var diff = (b.Y - a.Y) * (c.X - b.X) - (c.Y - b.Y) * (b.X - a.X);

			return Math.Abs(diff) < tolerance;
		}

		/// <summary>
		/// Returns a value indicating which side of the line AB the
		/// point pt is on, and how far away from the line it is. 
		/// If you stand at A and look at B, negative values will be
		/// to the left and positive values will be to the right.
		/// </summary>
		/// <param name="A"></param>
		/// <param name="B"></param>
		/// <param name="pt"></param>
		/// <returns></returns>
		public static double SideOf(Vector2 A, Vector2 B, Vector2 pt)
		{
			var toPt = pt - A;
			var diff = (B - A).Normalize();
			var projection = toPt.ProjectionOn(diff);
			var ortho = toPt - projection;

			return ortho.CrossProduct(diff);
		}

	}
}
