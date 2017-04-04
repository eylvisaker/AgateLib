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
namespace AgateLib.Mathematics.Geometry.Algorithms
{
	/// <summary>
	/// Structure which contains information about an intersection between
	/// line segments. This is returned from LineAlgorithms.LineSegmentIntersection.
	/// </summary>
	public class LineSegmentIntersection
	{
		/// <summary>
		/// Constructs a LineSegmentIntersection object.
		/// </summary>
		/// <param name="u1"></param>
		/// <param name="u2"></param>
		/// <param name="linesParallel"></param>
		public LineSegmentIntersection(Vector2 intersectionPoint, double u1, double u2)
		{
			IntersectionPoint = intersectionPoint;

			U1 = u1;
			U2 = u2;
		}

		/// <summary>
		/// Gets the point of intersection of the two lines.
		/// </summary>
		public Vector2 IntersectionPoint { get; private set; }

		/// <summary>
		/// Position in first line segment where the intersection takes place.
		/// If this value is outside the range (0, 1) then the point of intersection
		/// is outside the first line segment.
		/// </summary>
		public double U1 { get; private set; }

		/// <summary>
		/// Position in second line segment where the intersection takes place.
		/// If this value is outside the range (0, 1) then the point of intersection
		/// is outside the second line segment.
		/// </summary>
		public double U2 { get; private set; }
		
		/// <summary>
		/// Returns true if the first line segment contains the intersection point.
		/// </summary>
		public bool WithinFirstSegment => U1 >= 0 && U1 <= 1;

		/// <summary>
		/// Returns true if the second line segment contains the intersection point.
		/// </summary>
		public bool WithinSecondSegment => U2 >= 0 && U2 <= 1;

	}

}
