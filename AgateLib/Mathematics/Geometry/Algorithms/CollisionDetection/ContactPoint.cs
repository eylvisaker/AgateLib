namespace AgateLib.Mathematics.Geometry.Algorithms.CollisionDetection
{
	/// <summary>
	/// Structure which contains information about collision between two polygons.
	/// </summary>
	public class ContactPoint
	{
		/// <summary>
		/// The first polygon.
		/// </summary>
		public Polygon FirstPolygon { get; set; }
		/// <summary>
		/// The second polygon.
		/// </summary>
		public Polygon SecondPolygon { get; set; }

		/// <summary>
		/// The point on the perimeter of the first polygon that contacts the second polygon.
		/// </summary>
		public Vector2 FirstPolygonContactPoint { get; set; }
		/// <summary>
		/// The point on the perimeter of the second polygon that contacts the first polygon.
		/// </summary>
		public Vector2 SecondPolygonContactPoint { get; set; }

		/// <summary>
		/// The penetration vector.
		/// </summary>
		public Vector2 PenetrationDepth { get; set; }

		/// <summary>
		/// True if the two polygons touch, false if they do not.
		/// </summary>
		public bool Contact { get; set; }

		/// <summary>
		/// The normal of the contact edge for the first polygon.
		/// </summary>
		public Vector2 FirstPolygonNormal { get; set; }

		/// <summary>
		/// The normal of the contact edge for the second polygon.
		/// </summary>
		public Vector2 SecondPolygonNormal { get; set; }

		/// <summary>
		/// If the two polygons do not touch, this is the closest distance between them.
		/// </summary>
		public double DistanceToContact { get; set; }
	}
}