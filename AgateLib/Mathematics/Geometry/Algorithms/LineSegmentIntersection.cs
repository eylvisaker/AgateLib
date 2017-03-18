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
		public bool IntersectionWithinFirstSegment => U1 >= 0 && U1 <= 1;

		/// <summary>
		/// Returns true if the second line segment contains the intersection point.
		/// </summary>
		public bool IntersectionWithinSecondSegment => U2 >= 0 && U2 <= 1;

	}

}
